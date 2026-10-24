namespace PlateRecognitionApp
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            // 使用教学提供的 HWindow_Final 外层控件；它内部包含 HWindowControl，并提供适应窗口、缩放/拖动、状态栏和右键菜单。
            this.hWindow = new ChoiceTech.Halcon.Control.HWindow_Final();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSelectRoi = new System.Windows.Forms.Button();
            this.btnPreviewRoi = new System.Windows.Forms.Button();
            this.btnPreprocess = new System.Windows.Forms.Button();
            this.btnOcr = new System.Windows.Forms.Button();
            // 以下控件直接由窗体设计器管理：可在设计画布选中，也可在属性窗口修改文字、颜色和范围。
            this.rowThreshold = new System.Windows.Forms.TableLayoutPanel();
            this.lblThreshold = new System.Windows.Forms.Label();
            this.trackThreshold = new System.Windows.Forms.TrackBar();
            this.lblThresholdValue = new System.Windows.Forms.Label();
            this.rowWidth = new System.Windows.Forms.TableLayoutPanel();
            this.lblWidthTitle = new System.Windows.Forms.Label();
            this.trackWidth = new System.Windows.Forms.TrackBar();
            this.lblWidthValue = new System.Windows.Forms.Label();
            this.rowArea = new System.Windows.Forms.TableLayoutPanel();
            this.lblAreaTitle = new System.Windows.Forms.Label();
            this.trackArea = new System.Windows.Forms.TrackBar();
            this.lblAreaValue = new System.Windows.Forms.Label();
            this.rowBottom = new System.Windows.Forms.TableLayoutPanel();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblHint = new System.Windows.Forms.Label();
            this.mainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.imagePanel = new System.Windows.Forms.Panel();
            this.rightLayout = new System.Windows.Forms.TableLayoutPanel();
            this.colorBar = new System.Windows.Forms.Panel();
            this.mainLayout.SuspendLayout();
            this.imagePanel.SuspendLayout();
            this.rightLayout.SuspendLayout();
            this.rowThreshold.SuspendLayout();
            this.rowWidth.SuspendLayout();
            this.rowArea.SuspendLayout();
            this.rowBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // hWindow
            // 
            this.hWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindow.Location = new System.Drawing.Point(0, 25);
            this.hWindow.Name = "hWindow";
            this.hWindow.Size = new System.Drawing.Size(905, 893);
            this.hWindow.TabIndex = 0;
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOpen.BackColor = System.Drawing.Color.Gray;
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOpen.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpen.ForeColor = System.Drawing.Color.White;
            this.btnOpen.Location = new System.Drawing.Point(118, 58);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(165, 65);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "选择图片";
            this.btnOpen.UseVisualStyleBackColor = false;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSelectRoi
            // 
            this.btnSelectRoi.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSelectRoi.BackColor = System.Drawing.Color.Gray;
            this.btnSelectRoi.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSelectRoi.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectRoi.ForeColor = System.Drawing.Color.White;
            this.btnSelectRoi.Location = new System.Drawing.Point(119, 241);
            this.btnSelectRoi.Name = "btnSelectRoi";
            this.btnSelectRoi.Size = new System.Drawing.Size(162, 64);
            this.btnSelectRoi.TabIndex = 2;
            this.btnSelectRoi.Text = "框选车牌";
            this.btnSelectRoi.UseVisualStyleBackColor = false;
            this.btnSelectRoi.Click += new System.EventHandler(this.btnSelectRoi_Click);
            // 
            // btnPreviewRoi
            // 
            this.btnPreviewRoi.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPreviewRoi.BackColor = System.Drawing.Color.Gray;
            this.btnPreviewRoi.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPreviewRoi.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPreviewRoi.ForeColor = System.Drawing.Color.White;
            this.btnPreviewRoi.Location = new System.Drawing.Point(119, 424);
            this.btnPreviewRoi.Name = "btnPreviewRoi";
            this.btnPreviewRoi.Size = new System.Drawing.Size(162, 62);
            this.btnPreviewRoi.TabIndex = 3;
            this.btnPreviewRoi.Text = "提取 ROI";
            this.btnPreviewRoi.UseVisualStyleBackColor = false;
            this.btnPreviewRoi.Click += new System.EventHandler(this.btnPreviewRoi_Click);
            // 
            // btnPreprocess
            // 
            this.btnPreprocess.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPreprocess.BackColor = System.Drawing.Color.Gray;
            this.btnPreprocess.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPreprocess.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPreprocess.ForeColor = System.Drawing.Color.White;
            this.btnPreprocess.Location = new System.Drawing.Point(114, 606);
            this.btnPreprocess.Name = "btnPreprocess";
            this.btnPreprocess.Size = new System.Drawing.Size(173, 62);
            this.btnPreprocess.TabIndex = 4;
            this.btnPreprocess.Text = "图像预处理";
            this.btnPreprocess.UseVisualStyleBackColor = false;
            this.btnPreprocess.Click += new System.EventHandler(this.btnPreprocess_Click);
            // 
            // btnOcr
            // 
            this.btnOcr.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOcr.BackColor = System.Drawing.Color.Gray;
            this.btnOcr.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOcr.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOcr.ForeColor = System.Drawing.Color.White;
            this.btnOcr.Location = new System.Drawing.Point(119, 789);
            this.btnOcr.Name = "btnOcr";
            this.btnOcr.Size = new System.Drawing.Size(163, 61);
            this.btnOcr.TabIndex = 5;
            this.btnOcr.Text = "OCR 识别";
            this.btnOcr.UseVisualStyleBackColor = false;
            this.btnOcr.Click += new System.EventHandler(this.btnOcr_Click);
            // 
            // rowThreshold
            // 第 6 行参数区：标题在上方，滑动条在左下，实时数值在滑动条右侧。
            // 
            this.rowThreshold.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.rowThreshold.ColumnCount = 2;
            this.rowThreshold.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76F));
            this.rowThreshold.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.rowThreshold.Controls.Add(this.lblThreshold, 0, 0);
            this.rowThreshold.Controls.Add(this.trackThreshold, 0, 1);
            this.rowThreshold.Controls.Add(this.lblThresholdValue, 1, 1);
            this.rowThreshold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rowThreshold.Margin = new System.Windows.Forms.Padding(3);
            this.rowThreshold.Name = "rowThreshold";
            this.rowThreshold.RowCount = 2;
            this.rowThreshold.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.rowThreshold.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.rowThreshold.Size = new System.Drawing.Size(395, 95);
            this.rowThreshold.TabIndex = 6;
            // 
            // lblThreshold
            // 
            this.lblThreshold.AutoEllipsis = true;
            this.lblThreshold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblThreshold.Font = new System.Drawing.Font("微软雅黑", 8.5F);
            this.lblThreshold.ForeColor = System.Drawing.Color.White;
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.rowThreshold.SetColumnSpan(this.lblThreshold, 2);
            this.lblThreshold.TabIndex = 0;
            this.lblThreshold.Text = "阈值上限（灰度值 0～255）";
            this.lblThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackThreshold
            // ValueChanged 每变一次就更新右侧数值；拖动时不会立即重跑图像算法。
            // 
            this.trackThreshold.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.trackThreshold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackThreshold.LargeChange = 25;
            this.trackThreshold.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.trackThreshold.Maximum = 255;
            this.trackThreshold.Minimum = 0;
            this.trackThreshold.Name = "trackThreshold";
            this.trackThreshold.Size = new System.Drawing.Size(284, 55);
            this.trackThreshold.SmallChange = 1;
            this.trackThreshold.TabIndex = 1;
            this.trackThreshold.TickFrequency = 25;
            this.trackThreshold.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.trackThreshold.Value = 124;
            this.trackThreshold.ValueChanged += new System.EventHandler(this.trackThreshold_ValueChanged);
            // 
            // lblThresholdValue
            // 
            this.lblThresholdValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblThresholdValue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.lblThresholdValue.ForeColor = System.Drawing.Color.White;
            this.lblThresholdValue.Name = "lblThresholdValue";
            this.lblThresholdValue.TabIndex = 2;
            this.lblThresholdValue.Text = "124";
            this.lblThresholdValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rowWidth
            // 第 7 行参数区：闭运算核的宽度可调，高度仍由算法固定为 5 像素。
            // 
            this.rowWidth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.rowWidth.ColumnCount = 2;
            this.rowWidth.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76F));
            this.rowWidth.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.rowWidth.Controls.Add(this.lblWidthTitle, 0, 0);
            this.rowWidth.Controls.Add(this.trackWidth, 0, 1);
            this.rowWidth.Controls.Add(this.lblWidthValue, 1, 1);
            this.rowWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rowWidth.Margin = new System.Windows.Forms.Padding(3);
            this.rowWidth.Name = "rowWidth";
            this.rowWidth.RowCount = 2;
            this.rowWidth.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.rowWidth.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.rowWidth.Size = new System.Drawing.Size(395, 95);
            this.rowWidth.TabIndex = 7;
            // 
            // lblWidthTitle
            // 
            this.lblWidthTitle.AutoEllipsis = true;
            this.lblWidthTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWidthTitle.Font = new System.Drawing.Font("微软雅黑", 8.5F);
            this.lblWidthTitle.ForeColor = System.Drawing.Color.White;
            this.lblWidthTitle.Name = "lblWidthTitle";
            this.lblWidthTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.rowWidth.SetColumnSpan(this.lblWidthTitle, 2);
            this.lblWidthTitle.TabIndex = 0;
            this.lblWidthTitle.Text = "闭运算核宽度（高度固定 5 px）";
            this.lblWidthTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackWidth
            // 
            this.trackWidth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.trackWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackWidth.LargeChange = 1;
            this.trackWidth.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.trackWidth.Maximum = 20;
            this.trackWidth.Minimum = 1;
            this.trackWidth.Name = "trackWidth";
            this.trackWidth.Size = new System.Drawing.Size(284, 55);
            this.trackWidth.SmallChange = 1;
            this.trackWidth.TabIndex = 1;
            this.trackWidth.TickFrequency = 1;
            this.trackWidth.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.trackWidth.Value = 8;
            this.trackWidth.ValueChanged += new System.EventHandler(this.trackWidth_ValueChanged);
            // 
            // lblWidthValue
            // 
            this.lblWidthValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWidthValue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.lblWidthValue.ForeColor = System.Drawing.Color.White;
            this.lblWidthValue.Name = "lblWidthValue";
            this.lblWidthValue.TabIndex = 2;
            this.lblWidthValue.Text = "8 px";
            this.lblWidthValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rowArea
            // 第 8 行参数区：面积下限的范围是 0～5000 平方像素。
            // 
            this.rowArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.rowArea.ColumnCount = 2;
            this.rowArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76F));
            this.rowArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.rowArea.Controls.Add(this.lblAreaTitle, 0, 0);
            this.rowArea.Controls.Add(this.trackArea, 0, 1);
            this.rowArea.Controls.Add(this.lblAreaValue, 1, 1);
            this.rowArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rowArea.Margin = new System.Windows.Forms.Padding(3);
            this.rowArea.Name = "rowArea";
            this.rowArea.RowCount = 2;
            this.rowArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.rowArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.rowArea.Size = new System.Drawing.Size(395, 95);
            this.rowArea.TabIndex = 8;
            // 
            // lblAreaTitle
            // 
            this.lblAreaTitle.AutoEllipsis = true;
            this.lblAreaTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAreaTitle.Font = new System.Drawing.Font("微软雅黑", 8.5F);
            this.lblAreaTitle.ForeColor = System.Drawing.Color.White;
            this.lblAreaTitle.Name = "lblAreaTitle";
            this.lblAreaTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.rowArea.SetColumnSpan(this.lblAreaTitle, 2);
            this.lblAreaTitle.TabIndex = 0;
            this.lblAreaTitle.Text = "候选区域最小面积（0～5000 px²）";
            this.lblAreaTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trackArea
            // 
            this.trackArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.trackArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackArea.LargeChange = 500;
            this.trackArea.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.trackArea.Maximum = 5000;
            this.trackArea.Minimum = 0;
            this.trackArea.Name = "trackArea";
            this.trackArea.Size = new System.Drawing.Size(284, 55);
            this.trackArea.SmallChange = 1;
            this.trackArea.TabIndex = 1;
            this.trackArea.TickFrequency = 500;
            this.trackArea.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
            this.trackArea.Value = 200;
            this.trackArea.ValueChanged += new System.EventHandler(this.trackArea_ValueChanged);
            // 
            // lblAreaValue
            // 
            this.lblAreaValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAreaValue.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.lblAreaValue.ForeColor = System.Drawing.Color.White;
            this.lblAreaValue.Name = "lblAreaValue";
            this.lblAreaValue.TabIndex = 2;
            this.lblAreaValue.Text = "200 px²";
            this.lblAreaValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rowBottom
            // 最后一行：恢复默认参数按钮和操作提示。
            // 
            this.rowBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.rowBottom.ColumnCount = 2;
            this.rowBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38F));
            this.rowBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62F));
            this.rowBottom.Controls.Add(this.btnReset, 0, 0);
            this.rowBottom.Controls.Add(this.lblHint, 1, 0);
            this.rowBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rowBottom.Margin = new System.Windows.Forms.Padding(3);
            this.rowBottom.Name = "rowBottom";
            this.rowBottom.RowCount = 1;
            this.rowBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rowBottom.Size = new System.Drawing.Size(395, 95);
            this.rowBottom.TabIndex = 9;
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.Gray;
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReset.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold);
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Margin = new System.Windows.Forms.Padding(4);
            this.btnReset.Name = "btnReset";
            this.btnReset.TabIndex = 0;
            this.btnReset.Text = "恢复默认参数";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblHint
            // 
            this.lblHint.AutoEllipsis = true;
            this.lblHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHint.Font = new System.Drawing.Font("微软雅黑", 8.5F);
            this.lblHint.ForeColor = System.Drawing.Color.White;
            this.lblHint.Name = "lblHint";
            this.lblHint.Padding = new System.Windows.Forms.Padding(6, 0, 2, 0);
            this.lblHint.TabIndex = 1;
            this.lblHint.Text = "拖动后数值立即更新；点击“图像预处理”应用。";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mainLayout
            // 
            this.mainLayout.ColumnCount = 2;
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.97866F));
            this.mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.02134F));
            this.mainLayout.Controls.Add(this.imagePanel, 0, 0);
            this.mainLayout.Controls.Add(this.rightLayout, 1, 0);
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.RowCount = 1;
            this.mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.mainLayout.Size = new System.Drawing.Size(1312, 918);
            this.mainLayout.TabIndex = 6;
            // 
            // imagePanel
            // 
            this.imagePanel.Controls.Add(this.hWindow);
            this.imagePanel.Controls.Add(this.colorBar);
            this.imagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagePanel.Location = new System.Drawing.Point(0, 0);
            this.imagePanel.Margin = new System.Windows.Forms.Padding(0);
            this.imagePanel.Name = "imagePanel";
            this.imagePanel.Size = new System.Drawing.Size(905, 918);
            this.imagePanel.TabIndex = 0;
            // 
            // rightLayout
            // 
            this.rightLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.rightLayout.ColumnCount = 1;
            this.rightLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rightLayout.Controls.Add(this.btnOpen, 0, 0);
            this.rightLayout.Controls.Add(this.btnSelectRoi, 0, 1);
            this.rightLayout.Controls.Add(this.btnPreviewRoi, 0, 2);
            this.rightLayout.Controls.Add(this.btnPreprocess, 0, 3);
            this.rightLayout.Controls.Add(this.btnOcr, 0, 4);
            this.rightLayout.Controls.Add(this.rowThreshold, 0, 5);
            this.rightLayout.Controls.Add(this.rowWidth, 0, 6);
            this.rightLayout.Controls.Add(this.rowArea, 0, 7);
            this.rightLayout.Controls.Add(this.rowBottom, 0, 8);
            this.rightLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightLayout.Location = new System.Drawing.Point(908, 3);
            this.rightLayout.Name = "rightLayout";
            this.rightLayout.RowCount = 9;
            // 右侧保留原有 5 个操作按钮，另外 3 行放预处理参数滑动条，最后一行放“恢复默认”和说明。
            // 使用百分比高度，让按钮和参数区随窗体高度一起伸缩。
            this.rightLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11F));
            this.rightLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11F));
            this.rightLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11F));
            this.rightLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11F));
            this.rightLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11F));
            this.rightLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11F));
            this.rightLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11F));
            this.rightLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.11F));
            this.rightLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.12F));
            this.rightLayout.Size = new System.Drawing.Size(401, 912);
            this.rightLayout.TabIndex = 1;
            // 
            // colorBar
            // 
            this.colorBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            // 小色条停靠在图像区顶部；HWindow_Final 填充其余区域。
            this.colorBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.colorBar.Location = new System.Drawing.Point(0, 0);
            this.colorBar.Margin = new System.Windows.Forms.Padding(0);
            this.colorBar.Name = "colorBar";
            this.colorBar.Size = new System.Drawing.Size(905, 25);
            this.colorBar.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 918);
            this.Controls.Add(this.mainLayout);
            this.Name = "Form1";
            this.Text = "Form1";
            this.mainLayout.ResumeLayout(false);
            this.imagePanel.ResumeLayout(false);
            this.rowThreshold.ResumeLayout(false);
            this.rowWidth.ResumeLayout(false);
            this.rowArea.ResumeLayout(false);
            this.rowBottom.ResumeLayout(false);
            this.rightLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ChoiceTech.Halcon.Control.HWindow_Final hWindow;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnSelectRoi;
        private System.Windows.Forms.Button btnPreviewRoi;
        private System.Windows.Forms.Button btnPreprocess;
        private System.Windows.Forms.Button btnOcr;
        private System.Windows.Forms.TableLayoutPanel rowThreshold;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.TrackBar trackThreshold;
        private System.Windows.Forms.Label lblThresholdValue;
        private System.Windows.Forms.TableLayoutPanel rowWidth;
        private System.Windows.Forms.Label lblWidthTitle;
        private System.Windows.Forms.TrackBar trackWidth;
        private System.Windows.Forms.Label lblWidthValue;
        private System.Windows.Forms.TableLayoutPanel rowArea;
        private System.Windows.Forms.Label lblAreaTitle;
        private System.Windows.Forms.TrackBar trackArea;
        private System.Windows.Forms.Label lblAreaValue;
        private System.Windows.Forms.TableLayoutPanel rowBottom;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblHint;
        private System.Windows.Forms.TableLayoutPanel mainLayout;
        private System.Windows.Forms.Panel imagePanel;
        private System.Windows.Forms.Panel colorBar;
        private System.Windows.Forms.TableLayoutPanel rightLayout;
    }
}

