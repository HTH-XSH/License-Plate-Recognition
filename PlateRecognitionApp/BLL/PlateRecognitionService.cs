using System;
using System.Text;
using HalconDotNet;
using PlateRecognitionApp.DAL;
using PlateRecognitionApp.Models;

namespace PlateRecognitionApp.BLL
{
    /// <summary>
    /// BLL 业务层：把 ROI 提取、字符预处理和 OCR 识别串成完整的车牌识别流程，并通过 Models 返回结果。
    /// 它接收 UI 传入的 HALCON 图像和 ROI 数值，但不读取窗体控件、不显示消息框；模型文件路径检查交给 DAL。
    /// UI 在图片和 ROI 准备好后调用此服务，成功返回的 HALCON 结果由调用方按文档释放。
    /// internal 限定本类只在当前程序集内使用，sealed 表示不通过继承扩展业务服务。
    /// </summary>
    internal sealed class PlateRecognitionService
    {
        // DAL 只负责找到并检查模型文件；实际读取分类器和调用 OCR 算子由本业务服务完成。
        private readonly OcrModelRepository modelRepo;

        /// <summary>创建使用默认程序运行目录查找 OCR 模型的业务服务。</summary>
        public PlateRecognitionService()
            : this(new OcrModelRepository())
        {
        }

        /// <summary>供本程序集中的调用方传入模型仓库；空仓库不符合服务的运行条件。</summary>
        internal PlateRecognitionService(OcrModelRepository repo)
        {
            if (repo == null) throw new ArgumentNullException("repo");
            modelRepo = repo;
        }

        /// <summary>
        /// 在原图上创建旋转矩形 ROI，并用 ReduceDomain 限制图像的有效处理域。
        /// 返回图像仍保留原图矩阵尺寸和坐标系，不等同于裁切出一个新的小尺寸图像。
        /// </summary>
        /// <param name="image">待处理原图；必须是有效的 HALCON 图像对象，本方法不会释放它。</param>
        /// <param name="row">ROI 中心行坐标，单位为像素。</param>
        /// <param name="column">ROI 中心列坐标，单位为像素。</param>
        /// <param name="phi">ROI 第一条边相对水平轴的角度，单位为弧度。</param>
        /// <param name="length1">沿 phi 方向的半边长。</param>
        /// <param name="length2">垂直于 phi 方向的半边长。</param>
        /// <returns>有效域被限制到原图与 ROI 交集的图像；调用方负责 Dispose。</returns>
        /// <exception cref="ArgumentNullException">image 为空时抛出。</exception>
        /// <exception cref="HalconException">HALCON 无法创建 ROI 或处理图像时抛出。</exception>
        public HObject ExtractRoi(HObject image, double row, double column, double phi, double length1, double length2)
        {
            if (image == null) throw new ArgumentNullException("image");

            // roi 是本方法的临时输入；roiImg 成功返回时转交给调用方，失败时由 finally 释放。
            HObject roi = null;
            HObject roiImg = null;

            try
            {
                roi = CreateRoiRegion(row, column, phi, length1, length2);
                // 输入是原图和旋转矩形区域；新有效域取原图域与 ROI 的交集，图像矩阵尺寸不变。
                // 若 ROI 与图像没有重叠，有效域会为空，后续处理可能得不到字符。
                HOperatorSet.ReduceDomain(image, roi, out roiImg);

                // 返回前把 roiImg 的释放责任转给调用方；置 null 是避免 finally 再释放同一资源。
                HObject result = roiImg;
                roiImg = null;
                return result;
            }
            finally
            {
                if (roi != null) roi.Dispose();
                if (roiImg != null) roiImg.Dispose();
            }
        }

        /// <summary>创建旋转矩形区域供 UI 预览或 ExtractRoi 限制图像有效域；返回区域由调用方负责 Dispose。</summary>
        /// <param name="row">矩形中心行坐标，单位为像素。</param>
        /// <param name="column">矩形中心列坐标，单位为像素。</param>
        /// <param name="phi">第一条边相对水平轴的角度，单位为弧度。</param>
        /// <param name="length1">沿 phi 方向的半边长。</param>
        /// <param name="length2">垂直于 phi 方向的半边长。</param>
        /// <returns>新建的旋转矩形区域，调用方负责 Dispose。</returns>
        /// <exception cref="HalconException">HALCON 无法创建指定 ROI 时抛出。</exception>
        public HObject CreateRoiRegion(double row, double column, double phi, double length1, double length2)
        {
            // HOperatorSet.GenRectangle2 接收 HTuple；这些 tuple 只服务于本次算子调用，在 finally 中释放。
            HObject roi = null;
            HTuple roiRow = null;
            HTuple roiColumn = null;
            HTuple roiPhi = null;
            HTuple roiLength1 = null;
            HTuple roiLength2 = null;

            try
            {
                roiRow = new HTuple(row);
                roiColumn = new HTuple(column);
                roiPhi = new HTuple(phi);
                roiLength1 = new HTuple(length1);
                roiLength2 = new HTuple(length2);

                // 创建旋转矩形：row/column 是中心坐标，phi 是弧度角，length1/length2 是两个方向的半边长。
                // 区域可以超出图像边界；随后 ReduceDomain 会与原图有效域取交集。
                HOperatorSet.GenRectangle2(out roi, roiRow, roiColumn, roiPhi, roiLength1, roiLength2);
                // 区域对象交给调用方，tuple 参数仍由当前方法释放。
                HObject result = roi;
                roi = null;
                return result;
            }
            finally
            {
                if (roi != null) roi.Dispose();
                if (roiRow != null) roiRow.Dispose();
                if (roiColumn != null) roiColumn.Dispose();
                if (roiPhi != null) roiPhi.Dispose();
                if (roiLength1 != null) roiLength1.Dispose();
                if (roiLength2 != null) roiLength2.Dispose();
            }
        }

        /// <summary>
        /// 对原图的 ROI 依次执行灰度转换、反色、阈值分割、闭运算、连通域筛选和排序。
        /// 阈值上限、闭运算核宽和候选区域面积下限由 UI 滑动条传入；其余算法步骤与原流程相同。
        /// 返回对象持有排序后的字符区域及 OCR 所需图像，由调用方 Dispose。
        /// </summary>
        /// <param name="image">待处理的 RGB 原图；本方法不会释放调用方传入的图像。</param>
        /// <param name="row">ROI 中心行坐标，单位为像素。</param>
        /// <param name="column">ROI 中心列坐标，单位为像素。</param>
        /// <param name="phi">ROI 第一条边相对水平轴的角度，单位为弧度。</param>
        /// <param name="length1">沿 phi 方向的半边长。</param>
        /// <param name="length2">垂直于 phi 方向的半边长。</param>
        /// <param name="maxGray">反色图像阈值上限，Byte 灰度范围为 0～255。</param>
        /// <param name="kernelWidth">闭运算矩形结构元素宽度，单位为像素；高度在当前算法中固定为 5。</param>
        /// <param name="minArea">候选区域面积下限，单位为像素平方；上限在当前算法中固定为 5000。</param>
        /// <returns>字符区域、OCR 图像和字符数量组成的结果对象；调用方负责 Dispose。</returns>
        /// <exception cref="ArgumentNullException">image 为空时抛出。</exception>
        /// <exception cref="ArgumentOutOfRangeException">预处理参数超出本算法支持的范围时抛出。</exception>
        /// <exception cref="HalconException">图像格式不符合算子要求或 HALCON 处理失败时抛出。</exception>
        public PreprocessResult Preprocess(
            HObject image,
            double row,
            double column,
            double phi,
            double length1,
            double length2,
            int maxGray,
            int kernelWidth,
            int minArea)
        {
            if (image == null) throw new ArgumentNullException("image");

            // 界面 TrackBar 已限制范围，但业务层也要检查输入：服务以后可能被别的窗体或代码直接调用。
            // 这样参数正确性由处理算法本身保证，而不依赖某个特定 UI 控件。
            if (maxGray < 0 || maxGray > 255)
                throw new ArgumentOutOfRangeException("maxGray", "阈值上限必须在 0 到 255 之间。");
            if (kernelWidth < 1 || kernelWidth > 100)
                throw new ArgumentOutOfRangeException("kernelWidth", "闭运算核宽必须在 1 到 100 像素之间。");
            if (minArea < 0 || minArea > 5000)
                throw new ArgumentOutOfRangeException("minArea", "候选区域面积下限必须在 0 到 5000 之间。");

            // 下列 HObject 是本次流程的局部中间结果，正常结束和发生 HALCON 异常时都在 finally 中清理。
            // sortedReg 与 invertImg 若成功交给 PreprocessResult，则会先置 null，避免重复释放。
            HObject roiImg = null;
            HObject grayImg = null;
            HObject invertImg = null;
            HObject thresholdReg = null;
            HObject closedReg = null;
            HObject connectedReg = null;
            HObject selectedReg = null;
            HObject filteredReg = null;
            HObject sortedReg = null;
            HTuple charCount = null;

            try
            {
                // 复用 ROI 提取流程，保证预览按钮和预处理按钮使用相同的 ROI 几何定义。
                roiImg = ExtractRoi(image, row, column, phi, length1, length2);

                // 灰度转换：RGB 输入按 0.299R + 0.587G + 0.114B 加权生成单通道灰度图，便于后续按亮度分割。
                // HALCON 文档规定单通道输入会沿用其图像引用作为输出，不执行 RGB 加权转换。
                HOperatorSet.Rgb1ToGray(roiImg, out grayImg);
                // 灰度反转：将 ROI 像素灰度值反相；Byte 图像按 255 - g 计算，其他像素类型按其类型规则处理。
                // 阈值选中的究竟是字符还是背景取决于原图明暗分布，不能仅凭“反色”假定目标类别。
                HOperatorSet.InvertImage(grayImg, out invertImg);
                // 阈值分割：从反色图像中提取灰度值处于 0～maxGray 的像素。
                // maxGray 来自窗体滑动条；调低可能漏掉笔画，调高可能把背景也提取进来。
                HOperatorSet.Threshold(invertImg, out thresholdReg, 0, maxGray);
                // 闭运算：用矩形结构元素处理阈值区域，填补较小空隙并连接相近的断裂笔画。
                // 宽度由滑动条控制，高度暂时固定为 5；过小可能连不上，过大可能粘连相邻字符。
                HOperatorSet.ClosingRectangle1(thresholdReg, out closedReg, kernelWidth, 5);
                // 连通域分析：按像素邻接关系把区域分成独立对象；默认 8 邻域会把对角接触也视为连通。
                // 这里得到的仍是候选区域，可能包含字符、粘连字符或噪声。
                HOperatorSet.Connection(closedReg, out connectedReg);
                // 面积筛选：只保留面积在 minArea～5000 之间的区域。
                // 下限由滑动条控制，方便抑制小噪点或保留更小的字符；上限仍是现有算法的固定值。
                HOperatorSet.SelectShape(connectedReg, out selectedReg, "area", "and", minArea, 5000);
                // 高度筛选：在面积筛选结果中保留高度 15～200 像素的区域，进一步排除过矮或过高对象。
                // 这是第二次独立筛选；图像缩放或 ROI 尺寸变化可能导致真实字符被排除。
                HOperatorSet.SelectShape(selectedReg, out filteredReg, "height", "and", 15, 200);
                // 按 character 阅读顺序整理区域；true 表示升序，column 表示先按列、再按行确定方向。
                // 单行车牌因此从左向右排列，第一项继续使用中文 OCR 模型。
                HOperatorSet.SortRegion(filteredReg, out sortedReg, "character", "true", "column");
                // CountObj 统计对象元组中的区域个数，而不是图像像素数；0 个候选仍可供 UI 显示为空结果。
                HOperatorSet.CountObj(sortedReg, out charCount);

                // 构造结果对象并把两个 HALCON 对象的所有权交给它；字符数量复制为普通 int，不依赖 count tuple。
                PreprocessResult result = new PreprocessResult(sortedReg, invertImg, charCount.I);
                sortedReg = null;
                invertImg = null;
                return result;
            }
            finally
            {
                // 未转交给结果对象的临时图像、区域和 tuple 在此逐一释放；finally 也会在算子抛错时执行。
                if (roiImg != null) roiImg.Dispose();
                if (grayImg != null) grayImg.Dispose();
                if (invertImg != null) invertImg.Dispose();
                if (thresholdReg != null) thresholdReg.Dispose();
                if (closedReg != null) closedReg.Dispose();
                if (connectedReg != null) connectedReg.Dispose();
                if (selectedReg != null) selectedReg.Dispose();
                if (filteredReg != null) filteredReg.Dispose();
                if (sortedReg != null) sortedReg.Dispose();
                if (charCount != null) charCount.Dispose();
            }
        }

        /// <summary>
        /// 对预处理结果中的字符逐个执行 OCR：首字符使用中文模型，其余字符使用数字/字母模型，
        /// 最后应用现有的 I→1、O→0 规则。输入结果由调用方持有，本方法不会释放它。
        /// </summary>
        /// <param name="prep">尚未 Dispose 且字符数大于零的预处理结果。</param>
        /// <returns>仅包含车牌字符串和字符数量的普通 C# 结果对象，不拥有 HALCON 资源。</returns>
        /// <exception cref="ArgumentNullException">预处理结果为空时抛出。</exception>
        /// <exception cref="InvalidOperationException">预处理结果中没有候选字符时抛出。</exception>
        /// <exception cref="FileNotFoundException">DAL 检查到一个或多个 OCR 模型文件缺失时抛出。</exception>
        /// <exception cref="HalconException">读取模型或执行字符识别失败时抛出。</exception>
        public PlateRecognitionResult Recognize(PreprocessResult prep)
        {
            if (prep == null) throw new ArgumentNullException("prep");
            // 空字符集不具备 OCR 输入；提前结束也避免加载两个不会使用的分类器。
            if (prep.CharacterCount == 0) throw new InvalidOperationException("没有找到符合条件的字符，请检查预处理结果！");

            // 路径由 DAL 依据程序运行目录解析，并在返回前确认两个模型文件都存在。
            string chineseModelPath;
            string alphanumericModelPath;
            modelRepo.GetModelPaths(out chineseModelPath, out alphanumericModelPath);

            // ReadOcrClassMlp 创建的分类器句柄由本次 Recognize 调用持有，完成或失败后均在 finally 清理。
            HTuple chineseOcrHandle = null;
            HTuple alphanumericOcrHandle = null;
            // 只有模型读取成功后才调用 ClearOcrClassMlp；bool 用来区分“已创建的句柄”和仅分配的 tuple。
            bool chineseHandleCreated = false;
            bool alphanumericHandleCreated = false;

            try
            {
                // 从文件读入离线训练好的中文 OCR 分类器并取得 HALCON 句柄；文件内容无效时会抛出异常。
                HOperatorSet.ReadOcrClassMlp(chineseModelPath, out chineseOcrHandle);
                chineseHandleCreated = true;
                // 读入数字/英文字母分类器；若此步失败，finally 仍会清理已成功读取的中文分类器。
                HOperatorSet.ReadOcrClassMlp(alphanumericModelPath, out alphanumericOcrHandle);
                alphanumericHandleCreated = true;

                // 识别结果逐项追加到托管字符串缓冲区；字符顺序来自预处理阶段的 SortRegion。
                StringBuilder plateNumber = new StringBuilder();
                // HALCON 的 SelectObj 索引从 1 开始，因此循环范围是 1 到候选字符数（含首尾）。
                for (int index = 1; index <= prep.CharacterCount; index++)
                {
                    // 每轮创建一个字符区域和两个输出 tuple；本轮结束后都由下面的 finally 释放。
                    HObject currentCharacter = null;
                    HTuple classValue = null;
                    HTuple confidence = null;

                    try
                    {
                        // 从已排序对象元组中取出当前单个字符区域；源区域仍由 PreprocessResult 持有。
                        HOperatorSet.SelectObj(prep.SortedCharacters, out currentCharacter, index);
                        if (index == 1)
                        {
                            // 输入为字符区域、反色图像和中文模型句柄；输出类别及置信度，本流程只处理一个字符。
                            HOperatorSet.DoOcrMultiClassMlp(currentCharacter, prep.OcrImage, chineseOcrHandle, out classValue, out confidence);
                        }
                        else
                        {
                            // 后续字符使用数字/字母模型；confidence 是概率值，但现有流程不按置信度过滤。
                            HOperatorSet.DoOcrMultiClassMlp(currentCharacter, prep.OcrImage, alphanumericOcrHandle, out classValue, out confidence);
                        }

                        if (classValue == null || classValue.Length == 0)
                        {
                            // 本次没有返回类别时用问号占位，保持车牌字符串的位置与候选字符一致。
                            plateNumber.Append('?');
                        }
                        else
                        {
                            string recognizedCharacter = classValue[0].S;
                            // 仅修正数字/字母模型识别的后续字符，不对首个中文字符应用该规则。
                            if (index > 1 && recognizedCharacter == "I") recognizedCharacter = "1";
                            else if (index > 1 && recognizedCharacter == "O") recognizedCharacter = "0";
                            plateNumber.Append(recognizedCharacter);
                        }
                    }
                    finally
                    {
                        // 无论本字符识别成功或抛出异常，都释放 SelectObj 和 OCR 调用生成的临时对象。
                        if (currentCharacter != null) currentCharacter.Dispose();
                        if (classValue != null) classValue.Dispose();
                        if (confidence != null) confidence.Dispose();
                    }
                }

                // PlateRecognitionResult 只保存托管字符串和整数，因此返回它不会转移任何 HALCON 句柄。
                return new PlateRecognitionResult(plateNumber.ToString(), prep.CharacterCount);
            }
            finally
            {
                // 内层 finally 保证即使清理中文句柄时出错，也仍尝试清理字母数字句柄。
                try
                {
                    if (chineseHandleCreated) ClearOcrHandle(chineseOcrHandle);
                    else if (chineseOcrHandle != null) chineseOcrHandle.Dispose();
                }
                finally
                {
                    if (alphanumericHandleCreated) ClearOcrHandle(alphanumericOcrHandle);
                    else if (alphanumericOcrHandle != null) alphanumericOcrHandle.Dispose();
                }
            }
        }

        /// <summary>释放成功读取的 HALCON OCR 分类器，并在 finally 中释放承载句柄的 HTuple。</summary>
        /// <param name="ocrHandle">已由 ReadOcrClassMlp 成功创建的分类器句柄。</param>
        private static void ClearOcrHandle(HTuple ocrHandle)
        {
            try
            {
                // ClearOcrClassMlp 释放分类器占用的 HALCON 内存，并使该句柄失效。
                HOperatorSet.ClearOcrClassMlp(ocrHandle);
            }
            finally
            {
                // 即使 HALCON 清理算子抛错，也仍释放此 .NET HTuple 包装对象。
                if (ocrHandle != null) ocrHandle.Dispose();
            }
        }
    }
}
