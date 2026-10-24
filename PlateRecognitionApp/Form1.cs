using HalconDotNet;
using PlateRecognitionApp.BLL;
using PlateRecognitionApp.Models;
using System;
using System.Windows.Forms;

namespace PlateRecognitionApp
{
    /// <summary>
    /// UI 层窗体：接收按钮和鼠标操作、显示 HALCON 图像及提示信息，并把图像处理请求交给 BLL。
    /// 窗体不负责实现车牌分割或 OCR 算法；它只持有当前原图和跨按钮使用的预处理结果。
    /// </summary>
    public partial class Form1 : Form
    {
        // 命名习惯：类名用大驼峰（Form1）；字段和局部变量用小驼峰（currentImage）。
        // btn 表示按钮，track 表示滑动条，lbl 表示标签；看到前缀就能大致知道控件类型。
        // 窗体通过服务调用业务流程；识别服务不依赖本窗体或任何 WinForms 控件。
        private readonly PlateRecognitionService service = new PlateRecognitionService();

        // 当前窗体拥有的原始图像；替换图片或关闭窗体时由 UI 释放。
        private HObject currentImage;

        // 预处理按钮与 OCR 按钮之间共享的结果；该对象拥有字符区域和 OCR 图像。
        private PreprocessResult prepResult;

        // 标记 ROI 参数是否已由用户绘制。五个几何值是普通数值，不是需要 Dispose 的 HALCON 对象。
        private bool hasRoi;
        private double roiRow;
        private double roiCol;
        private double roiAngle;
        private double roiLen1;
        private double roiLen2;

        // 与业务层当前算法中的默认常量保持一致；“恢复默认”按钮会把滑动条设置回这些值。
        private const int DefaultThreshold = 124;
        private const int DefaultWidth = 8;
        private const int DefaultArea = 200;

        /// <summary>创建窗体并初始化 Designer 中定义的控件和事件绑定。</summary>
        public Form1()
        {
            // 滑动条、数值标签和恢复按钮现在都在 Form1.Designer.cs 中声明，
            // 所以既会在程序运行时显示，也能在 Visual Studio 窗体设计器画布上直接编辑。
            InitializeComponent();
        }

        /// <summary>把阈值滑块当前数值显示在右侧，并使旧的预处理结果失效。</summary>
        private void trackThreshold_ValueChanged(object sender, EventArgs e)
        {
            lblThresholdValue.Text = trackThreshold.Value.ToString();
            OnParamChanged();
        }

        /// <summary>把闭运算核宽度显示成像素数，并使旧的预处理结果失效。</summary>
        private void trackWidth_ValueChanged(object sender, EventArgs e)
        {
            lblWidthValue.Text = trackWidth.Value.ToString() + " px";
            OnParamChanged();
        }

        /// <summary>把候选区域面积下限显示成平方像素，并使旧的预处理结果失效。</summary>
        private void trackArea_ValueChanged(object sender, EventArgs e)
        {
            lblAreaValue.Text = trackArea.Value.ToString() + " px²";
            OnParamChanged();
        }

        /// <summary>
        /// 任一识别参数变化后，旧预处理字符区域不再与当前设置对应，因此立即释放缓存。
        /// 此处只更新界面和状态，不重新跑图像算法；要应用新数值，请点击“图像预处理”。
        /// </summary>
        private void OnParamChanged()
        {
            if (prepResult != null)
            {
                ClearPrepResult();
                try
                {
                    // 参数一变，之前叠加在图像上的候选区域也过期；重新显示原图会一并清除旧叠加。
                    // 只在第一次改动时刷新，因为刷新后结果字段为空，拖动过程的后续数值不会反复重载整张图。
                    if (currentImage != null) hWindow.HobjectToHimage(currentImage);
                }
                catch (HalconException)
                {
                    // 数值标签必须持续响应鼠标；显示刷新失败时保留参数修改，并在说明标签中提示。
                    lblHint.Text = "参数已更改；原图刷新失败，请重新加载图片后再预处理。";
                    return;
                }
                lblHint.Text = "参数已更改；请点击“图像预处理”，再执行 OCR。";
            }
        }

        /// <summary>把三个滑动条恢复到与原始算法一致的阈值、核宽和面积下限。</summary>
        private void btnReset_Click(object sender, EventArgs e)
        {
            // 每次 Value 改变会自动更新数值标签，并触发旧结果失效处理；无需手动改标签文字。
            trackThreshold.Value = DefaultThreshold;
            trackWidth.Value = DefaultWidth;
            trackArea.Value = DefaultArea;

            // 如果所有滑块原本就已是默认值，缓存的预处理结果仍然对应当前参数，无需强制用户重复计算。
            if (prepResult != null)
                lblHint.Text = "当前参数已是默认值；已有预处理结果仍有效。";
            else
                lblHint.Text = "已恢复默认值；点击“图像预处理”应用默认参数。";
        }

        /// <summary>
        /// 选择并显示原图。只有新图成功读取和显示后才替换当前图像；替换时使旧 ROI 和预处理结果失效。
        /// </summary>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "选择车牌图片";
                dialog.Filter = "图像文件|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";

                // 用户取消选择时不改变当前图片或已有处理状态。
                if (dialog.ShowDialog() != DialogResult.OK) return;

                // newImg 暂时由本方法持有；成功后会转交给窗体的 currentImage 字段。
                HObject newImg = null;

                try
                {
                    // 读取用户选中的图像文件；格式不受支持或文件损坏时由 HALCON 报错。
                    HOperatorSet.ReadImage(out newImg, dialog.FileName);
                    // 把图像交给 HWindow_Final 显示。控件会维护缩放、平移和背景图；同时清除上一张图的叠加对象。
                    // 控件内部保存自己的 HImage 包装，因此 newImg 仍由窗体持有，可供 ROI 和识别流程重复使用。
                    hWindow.HobjectToHimage(newImg);

                    // 先把新图设为当前图，再清理旧图状态；oldImage 暂时保存旧图引用以便明确释放。
                    HObject oldImage = currentImage;
                    currentImage = newImg;
                    newImg = null;
                    try
                    {
                        ClearPrepResult();
                    }
                    finally
                    {
                        // 新图不再对应旧 ROI；即使释放旧结果时发生异常，也尝试使旧图和 ROI 失效。
                        ClearRoi();
                        if (oldImage != null) oldImage.Dispose();
                    }
                }
                catch (HalconException ex)
                {
                    // 仅处理 HALCON 读取、尺寸查询或窗口显示阶段的错误，并把原因反馈给用户。
                    MessageBox.Show(ex.Message, "图像加载失败");
                }
                finally
                {
                    // 若新图未成功转交给窗体字段，在这里释放本地 HALCON 图像。
                    DisposeHalconObject(newImg);
                }
            }
        }

        /// <summary>让用户绘制旋转矩形 ROI，并将绘制结果保存供预览和预处理按钮使用。</summary>
        private void btnSelectRoi_Click(object sender, EventArgs e)
        {
            // 没有原图就无法解释鼠标坐标，也无法建立 ROI，因此停止本次操作。
            if (currentImage == null)
            {
                MessageBox.Show("请先选择图片！");
                return;
            }

            HObject roi = null;
            try
            {
                // 先回到原图，保证用户看得到车牌；HWindow_Final 会在载入新背景时清除上一阶段的显示叠加。
                hWindow.HobjectToHimage(currentImage);

                // DrawModel 暂停控件自己的鼠标缩放/拖动处理，避免它和 HALCON 的交互式画框争抢鼠标。
                // 用户按下、拖动并松开鼠标后，HALCON 返回中心行列、角度（弧度）和两个半边长。
                hWindow.DrawModel = true;
                try
                {
                    hWindow.hWindowControl.HalconWindow.DrawRectangle2(
                        out roiRow,
                        out roiCol,
                        out roiAngle,
                        out roiLen1,
                        out roiLen2);
                }
                finally
                {
                    // 无论绘制成功还是被取消/抛错，都恢复控件的缩放、拖动和右键菜单功能。
                    hWindow.DrawModel = false;
                }

                hasRoi = true;
                // 新 ROI 不再匹配旧字符区域和 OCR 图像，先释放旧的跨按钮结果。
                ClearPrepResult();

                // BLL 根据保存的几何值创建区域；窗体只负责把区域交给控件叠加在原图上。
                roi = service.CreateRoiRegion(roiRow, roiCol, roiAngle, roiLen1, roiLen2);
                // HWindow_Final.DispObj 会复制显示对象以便缩放/重绘时重画；临时 roi 仍由本事件释放。
                hWindow.DispObj(roi, "green");
            }
            catch (HalconException ex)
            {
                // 捕获绘图、创建 ROI 或显示区域时的 HALCON 错误。
                MessageBox.Show(ex.Message, "ROI 框选失败");
            }
            finally
            {
                // roi 是本事件临时创建的显示对象，不跨按钮保存。
                DisposeHalconObject(roi);
            }
        }

        /// <summary>预览原图在 ROI 有效域内的内容；返回图像由本事件在显示后释放。</summary>
        private void btnPreviewRoi_Click(object sender, EventArgs e)
        {
            if (!EnsureImageAndRoi()) return;

            HObject roiImg = null;
            try
            {
                // ROI 区域构造及 ReduceDomain 在 BLL；UI 先保留原图为背景，再把 ROI 有效域叠加显示。
                roiImg = service.ExtractRoi(currentImage, roiRow, roiCol, roiAngle, roiLen1, roiLen2);
                // 把原图设为背景可保证控件右键“保存原始图像”始终保存用户加载的原图，而不是处理中的中间图。
                hWindow.HobjectToHimage(currentImage);
                // ROI 图像的有效域受限于所选区域；控件只把这个对象作为显示叠加，不改变原图和其所有权。
                hWindow.DispObj(roiImg);
            }
            catch (HalconException ex)
            {
                // 反馈 ROI 提取或窗口显示阶段的 HALCON 错误。
                MessageBox.Show(ex.Message, "ROI 提取失败");
            }
            finally
            {
                // ExtractRoi 返回的图像归调用方持有；显示结束后由本事件释放。
                DisposeHalconObject(roiImg);
            }
        }

        /// <summary>调用 BLL 完成图像预处理，并保存结果供 OCR 按钮继续使用。</summary>
        private void btnPreprocess_Click(object sender, EventArgs e)
        {
            if (!EnsureImageAndRoi()) return;

            // 本次重算开始后旧结果不再作为 OCR 输入；若新计算失败，OCR 按钮也不会误用旧结果。
            ClearPrepResult();
            PreprocessResult newPrep = null;
            try
            {
                // 先回到原图，避免计算失败时屏幕仍留着已作废的旧候选区域。
                hWindow.HobjectToHimage(currentImage);

                // BLL 返回的新结果暂由本地变量持有，显示成功后才转交给窗体字段。
                // 从滑动条读取当下参数并传给 BLL；如果用户只拖动滑块、不点击本按钮，算法不会提前重算。
                newPrep = service.Preprocess(
                    currentImage,
                    roiRow,
                    roiCol,
                    roiAngle,
                    roiLen1,
                    roiLen2,
                    trackThreshold.Value,
                    trackWidth.Value,
                    trackArea.Value);
                // 原图仍留在控件的背景图槽位，反色 ROI 图像作为叠加显示；因此右键保存原图不会保存中间结果。
                // 再把已筛选的字符候选区域叠成绿色。显示对象由控件复制，业务结果仍供 OCR 使用。
                hWindow.DispObj(newPrep.OcrImage);
                hWindow.DispObj(newPrep.SortedCharacters, "green");

                // 字段接管整个 PreprocessResult 的释放责任；置空本地引用以避免 finally 重复释放。
                prepResult = newPrep;
                newPrep = null;
                lblHint.Text = "本次参数已应用；若继续调参，请再次点击“图像预处理”。";
                MessageBox.Show(
                    "预处理完成，找到 " + prepResult.CharacterCount + " 个字符候选区域。\r\n\r\n" +
                    "阈值上限：" + trackThreshold.Value + "\r\n" +
                    "闭运算核宽：" + trackWidth.Value + " px（高度固定 5 px）\r\n" +
                    "最小区域面积：" + trackArea.Value + " px²",
                    "预处理结果");
            }
            catch (HalconException ex)
            {
                // 预处理或显示区域时出现的 HALCON 错误由 UI 转换为用户提示。
                MessageBox.Show(ex.Message, "图像预处理失败");
            }
            finally
            {
                // 只有尚未转交给字段的结果才在此处销毁；已保存结果要供 OCR 继续使用。
                if (newPrep != null) newPrep.Dispose();
            }
        }

        /// <summary>将已保存的预处理结果交给 BLL 识别，并显示不含 HALCON 资源的车牌字符串。</summary>
        private void btnOcr_Click(object sender, EventArgs e)
        {
            // OCR 必须使用当前 ROI 对应的预处理结果，不能只凭原图直接进入本处理步骤。
            if (prepResult == null)
            {
                MessageBox.Show("请先加载图片、框选 ROI，并点击预处理按钮！");
                return;
            }

            try
            {
                // Recognize 会在内部读取并释放 OCR 分类器；预处理结果仍由窗体持有，可再次识别。
                PlateRecognitionResult result = service.Recognize(prepResult);
                MessageBox.Show("车牌识别结果：" + result.PlateNumber, "OCR 识别完成");
            }
            catch (Exception ex)
            {
                // 此处既可能收到 HALCON 识别异常，也可能收到 DAL 报告的模型缺失异常。
                MessageBox.Show(ex.Message, "OCR 识别失败");
            }
        }

        /// <summary>检查后续 ROI 操作所需的原图和 ROI 是否都已准备好。</summary>
        private bool EnsureImageAndRoi()
        {
            // 原图是 BLL 提取 ROI 的输入；未加载时给出提示并由调用方提前结束事件。
            if (currentImage == null)
            {
                MessageBox.Show("请先选择图片！");
                return false;
            }

            // ROI 参数由鼠标绘制产生；初始值为 0 不代表用户已完成框选。
            if (!hasRoi)
            {
                MessageBox.Show("请先框选车牌 ROI！");
                return false;
            }

            return true;
        }

        /// <summary>使跨按钮共享的预处理结果失效，并释放其拥有的字符区域和 OCR 图像。</summary>
        private void ClearPrepResult()
        {
            // 先清空字段，确保即使 Dispose 抛出异常，后续事件也不会再把旧结果当作有效输入。
            PreprocessResult previousResult = prepResult;
            prepResult = null;
            if (previousResult != null) previousResult.Dispose();
        }

        /// <summary>将 ROI 标记设为未选择并重置几何数值；double 字段本身无需释放。</summary>
        private void ClearRoi()
        {
            hasRoi = false;
            roiRow = 0;
            roiCol = 0;
            roiAngle = 0;
            roiLen1 = 0;
            roiLen2 = 0;
        }

        /// <summary>释放可能为空的 HALCON 图像/区域引用，供窗体事件的 finally 使用。</summary>
        private static void DisposeHalconObject(HObject halconObject)
        {
            if (halconObject != null) halconObject.Dispose();
        }

        /// <summary>关闭窗体时清理窗体持有的预处理结果、原图和 ROI 状态。</summary>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                // ClearPrepResult 负责释放结果对象所拥有的两个 HALCON 对象。
                ClearPrepResult();
            }
            finally
            {
                // 原图由窗体独立持有；无论预处理结果清理是否成功，都继续释放原图并调用基类关闭逻辑。
                HObject oldImage = currentImage;
                currentImage = null;
                ClearRoi();
                try
                {
                    if (oldImage != null) oldImage.Dispose();
                }
                finally
                {
                    base.OnFormClosed(e);
                }
            }
        }
    }
}
