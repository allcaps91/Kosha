namespace ComSupLibB.SupLbEx
{
    partial class frmComSupLbExSEND01
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer enhancedFocusIndicatorRenderer2 = new FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer3 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer4 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdo_OptA3 = new System.Windows.Forms.RadioButton();
            this.rdo_OptA2 = new System.Windows.Forms.RadioButton();
            this.rdo_OptA1 = new System.Windows.Forms.RadioButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdo_Opt2 = new System.Windows.Forms.RadioButton();
            this.rdo_Opt1 = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnSave_Send = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnView_Send = new System.Windows.Forms.Button();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.circProgress = new DevComponents.DotNetBar.Controls.CircularProgress();
            this.ssMain = new FarPoint.Win.Spread.FpSpread();
            this.ssMain_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.chkPart = new System.Windows.Forms.CheckBox();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain_Sheet1)).BeginInit();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Padding = new System.Windows.Forms.Padding(1);
            this.panTitle.Size = new System.Drawing.Size(1313, 48);
            this.panTitle.TabIndex = 13;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(1240, 1);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 46);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(6, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(219, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "병원체 검사 결과 자동 신고";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.panel9);
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 48);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Size = new System.Drawing.Size(1313, 73);
            this.panel1.TabIndex = 88;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdo_OptA3);
            this.groupBox3.Controls.Add(this.rdo_OptA2);
            this.groupBox3.Controls.Add(this.rdo_OptA1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(385, 4);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Size = new System.Drawing.Size(183, 65);
            this.groupBox3.TabIndex = 25;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "전송구분별조회";
            // 
            // rdo_OptA3
            // 
            this.rdo_OptA3.AutoSize = true;
            this.rdo_OptA3.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdo_OptA3.Location = new System.Drawing.Point(107, 22);
            this.rdo_OptA3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdo_OptA3.Name = "rdo_OptA3";
            this.rdo_OptA3.Size = new System.Drawing.Size(65, 39);
            this.rdo_OptA3.TabIndex = 2;
            this.rdo_OptA3.Text = "미전송";
            this.rdo_OptA3.UseVisualStyleBackColor = true;
            // 
            // rdo_OptA2
            // 
            this.rdo_OptA2.AutoSize = true;
            this.rdo_OptA2.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdo_OptA2.Location = new System.Drawing.Point(55, 22);
            this.rdo_OptA2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdo_OptA2.Name = "rdo_OptA2";
            this.rdo_OptA2.Size = new System.Drawing.Size(52, 39);
            this.rdo_OptA2.TabIndex = 1;
            this.rdo_OptA2.Text = "전송";
            this.rdo_OptA2.UseVisualStyleBackColor = true;
            // 
            // rdo_OptA1
            // 
            this.rdo_OptA1.AutoSize = true;
            this.rdo_OptA1.Checked = true;
            this.rdo_OptA1.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdo_OptA1.Location = new System.Drawing.Point(3, 22);
            this.rdo_OptA1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdo_OptA1.Name = "rdo_OptA1";
            this.rdo_OptA1.Size = new System.Drawing.Size(52, 39);
            this.rdo_OptA1.TabIndex = 0;
            this.rdo_OptA1.TabStop = true;
            this.rdo_OptA1.Text = "전체";
            this.rdo_OptA1.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(376, 4);
            this.panel5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(9, 65);
            this.panel5.TabIndex = 24;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdo_Opt2);
            this.groupBox2.Controls.Add(this.rdo_Opt1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(234, 4);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(142, 65);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "조회기간기준";
            // 
            // rdo_Opt2
            // 
            this.rdo_Opt2.AutoSize = true;
            this.rdo_Opt2.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdo_Opt2.Location = new System.Drawing.Point(73, 22);
            this.rdo_Opt2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdo_Opt2.Name = "rdo_Opt2";
            this.rdo_Opt2.Size = new System.Drawing.Size(65, 39);
            this.rdo_Opt2.TabIndex = 1;
            this.rdo_Opt2.Text = "의뢰일";
            this.rdo_Opt2.UseVisualStyleBackColor = true;
            // 
            // rdo_Opt1
            // 
            this.rdo_Opt1.AutoSize = true;
            this.rdo_Opt1.Checked = true;
            this.rdo_Opt1.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdo_Opt1.Location = new System.Drawing.Point(3, 22);
            this.rdo_Opt1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdo_Opt1.Name = "rdo_Opt1";
            this.rdo_Opt1.Size = new System.Drawing.Size(70, 39);
            this.rdo_Opt1.TabIndex = 0;
            this.rdo_Opt1.TabStop = true;
            this.rdo_Opt1.Text = "결과일 ";
            this.rdo_Opt1.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(225, 4);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(9, 65);
            this.panel4.TabIndex = 22;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtpTDate);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.dtpFDate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(3, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 5, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(222, 65);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "조회기간";
            // 
            // dtpTDate
            // 
            this.dtpTDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(126, 23);
            this.dtpTDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(89, 25);
            this.dtpTDate.TabIndex = 20;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(95, 23);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.panel3.Size = new System.Drawing.Size(31, 38);
            this.panel3.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(5, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 17);
            this.label2.TabIndex = 18;
            this.label2.Text = " ~ ";
            // 
            // dtpFDate
            // 
            this.dtpFDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(3, 23);
            this.dtpFDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(92, 25);
            this.dtpFDate.TabIndex = 16;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.btnSave_Send);
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Controls.Add(this.btnView_Send);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(946, 4);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(1, 10, 1, 10);
            this.panel2.Size = new System.Drawing.Size(364, 65);
            this.panel2.TabIndex = 20;
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(42, 10);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(71, 45);
            this.btnSearch.TabIndex = 32;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(113, 10);
            this.panel6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(9, 45);
            this.panel6.TabIndex = 31;
            // 
            // btnSave_Send
            // 
            this.btnSave_Send.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave_Send.Location = new System.Drawing.Point(122, 10);
            this.btnSave_Send.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave_Send.Name = "btnSave_Send";
            this.btnSave_Send.Size = new System.Drawing.Size(116, 45);
            this.btnSave_Send.TabIndex = 30;
            this.btnSave_Send.Text = "선택결과전송";
            this.btnSave_Send.UseVisualStyleBackColor = true;
            // 
            // panel7
            // 
            this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(238, 10);
            this.panel7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(9, 45);
            this.panel7.TabIndex = 29;
            // 
            // btnView_Send
            // 
            this.btnView_Send.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnView_Send.Location = new System.Drawing.Point(247, 10);
            this.btnView_Send.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnView_Send.Name = "btnView_Send";
            this.btnView_Send.Size = new System.Drawing.Size(116, 45);
            this.btnView_Send.TabIndex = 28;
            this.btnView_Send.Text = "신고내용조회";
            this.btnView_Send.UseVisualStyleBackColor = true;
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 121);
            this.panTitleSub0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1313, 38);
            this.panTitleSub0.TabIndex = 89;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 8);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(57, 12);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "검사내역";
            // 
            // panMain
            // 
            this.panMain.Controls.Add(this.circProgress);
            this.panMain.Controls.Add(this.ssMain);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 159);
            this.panMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panMain.Name = "panMain";
            this.panMain.Padding = new System.Windows.Forms.Padding(1);
            this.panMain.Size = new System.Drawing.Size(1313, 593);
            this.panMain.TabIndex = 90;
            // 
            // circProgress
            // 
            this.circProgress.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.circProgress.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.circProgress.Location = new System.Drawing.Point(503, 101);
            this.circProgress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.circProgress.Name = "circProgress";
            this.circProgress.Size = new System.Drawing.Size(259, 363);
            this.circProgress.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.circProgress.TabIndex = 3;
            this.circProgress.Visible = false;
            // 
            // ssMain
            // 
            this.ssMain.AccessibleDescription = "ssMain, Sheet1, Row 0, Column 0, ";
            this.ssMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssMain.FocusRenderer = enhancedFocusIndicatorRenderer2;
            this.ssMain.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssMain.HorizontalScrollBar.Name = "";
            this.ssMain.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer3;
            this.ssMain.HorizontalScrollBar.TabIndex = 23;
            this.ssMain.Location = new System.Drawing.Point(1, 1);
            this.ssMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ssMain.Name = "ssMain";
            this.ssMain.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssMain_Sheet1});
            this.ssMain.Size = new System.Drawing.Size(1311, 591);
            this.ssMain.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;
            this.ssMain.TabIndex = 0;
            this.ssMain.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssMain.VerticalScrollBar.Name = "";
            this.ssMain.VerticalScrollBar.Renderer = enhancedScrollBarRenderer4;
            this.ssMain.VerticalScrollBar.TabIndex = 24;
            // 
            // ssMain_Sheet1
            // 
            this.ssMain_Sheet1.Reset();
            this.ssMain_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssMain_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssMain_Sheet1.ColumnCount = 6;
            this.ssMain_Sheet1.RowCount = 3;
            this.ssMain_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.ssMain_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.ssMain_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.ColumnHeader.AutoText = FarPoint.Win.Spread.HeaderAutoText.Numbers;
            this.ssMain_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.ssMain_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarEnhanced";
            this.ssMain_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.FilterBarHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(236)))), ((int)(((byte)(247)))));
            this.ssMain_Sheet1.FilterBarHeaderStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ssMain_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMain_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderEnhanced";
            this.ssMain_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMain_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssMain_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssMain_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssMain_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.ssMain_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.ssMain_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new System.Drawing.Point(568, 4);
            this.panel8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(9, 65);
            this.panel8.TabIndex = 26;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.chkPart);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel9.Location = new System.Drawing.Point(577, 4);
            this.panel9.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(147, 65);
            this.panel9.TabIndex = 27;
            // 
            // chkPart
            // 
            this.chkPart.AutoSize = true;
            this.chkPart.Location = new System.Drawing.Point(6, 32);
            this.chkPart.Name = "chkPart";
            this.chkPart.Size = new System.Drawing.Size(136, 21);
            this.chkPart.TabIndex = 0;
            this.chkPart.Text = "부분입력상태 조회";
            this.chkPart.UseVisualStyleBackColor = true;
            // 
            // frmComSupLbExSEND01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1313, 752);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmComSupLbExSEND01";
            this.Text = "frmComSupLbExSEND01";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain_Sheet1)).EndInit();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panMain;
        private FarPoint.Win.Spread.FpSpread ssMain;
        private FarPoint.Win.Spread.SheetView ssMain_Sheet1;
        private DevComponents.DotNetBar.Controls.CircularProgress circProgress;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdo_OptA3;
        private System.Windows.Forms.RadioButton rdo_OptA2;
        private System.Windows.Forms.RadioButton rdo_OptA1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdo_Opt2;
        private System.Windows.Forms.RadioButton rdo_Opt1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnSave_Send;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button btnView_Send;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.CheckBox chkPart;
        private System.Windows.Forms.Panel panel8;
    }
}