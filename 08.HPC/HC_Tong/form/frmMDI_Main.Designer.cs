namespace HC_Tong
{
    partial class frmMDI_Main
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
            FarPoint.Win.Spread.FlatFocusIndicatorRenderer flatFocusIndicatorRenderer1 = new FarPoint.Win.Spread.FlatFocusIndicatorRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer3 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer4 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer5 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer6 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuJep00 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPat00 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOsha = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOsha01 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOsha02 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOsha03 = new System.Windows.Forms.ToolStripMenuItem();
            this.panMain = new System.Windows.Forms.Panel();
            this.SSMagam = new FarPoint.Win.Spread.FpSpread();
            this.SSMagam_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSub01 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.pnlCall = new System.Windows.Forms.Panel();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.prsBar = new System.Windows.Forms.ProgressBar();
            this.panSub02 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panMagam = new System.Windows.Forms.Panel();
            this.SSTong = new FarPoint.Win.Spread.FpSpread();
            this.SSTong_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSMagam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSMagam_Sheet1)).BeginInit();
            this.panSub01.SuspendLayout();
            this.pnlCall.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panSub02.SuspendLayout();
            this.panMagam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSTong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSTong_Sheet1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuJep00,
            this.menuPat00,
            this.menuOsha});
            this.menuStrip1.Location = new System.Drawing.Point(0, 37);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(666, 24);
            this.menuStrip1.TabIndex = 264;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuJep00
            // 
            this.menuJep00.Name = "menuJep00";
            this.menuJep00.Size = new System.Drawing.Size(67, 20);
            this.menuJep00.Text = "마감업무";
            // 
            // menuPat00
            // 
            this.menuPat00.Name = "menuPat00";
            this.menuPat00.Size = new System.Drawing.Size(67, 20);
            this.menuPat00.Text = "통계업무";
            // 
            // menuOsha
            // 
            this.menuOsha.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOsha01,
            this.menuOsha02,
            this.menuOsha03});
            this.menuOsha.Name = "menuOsha";
            this.menuOsha.Size = new System.Drawing.Size(67, 20);
            this.menuOsha.Text = "보건관리";
            // 
            // menuOsha01
            // 
            this.menuOsha01.Name = "menuOsha01";
            this.menuOsha01.Size = new System.Drawing.Size(210, 22);
            this.menuOsha01.Text = "일반건강진단 유소견자수";
            this.menuOsha01.Click += new System.EventHandler(this.menuOsha01_Click);
            // 
            // menuOsha02
            // 
            this.menuOsha02.Name = "menuOsha02";
            this.menuOsha02.Size = new System.Drawing.Size(210, 22);
            this.menuOsha02.Text = "특별건강진단 유소견자수";
            this.menuOsha02.Click += new System.EventHandler(this.menuOsha02_Click);
            // 
            // menuOsha03
            // 
            this.menuOsha03.Name = "menuOsha03";
            this.menuOsha03.Size = new System.Drawing.Size(210, 22);
            this.menuOsha03.Text = "사후관리(대행)";
            this.menuOsha03.Click += new System.EventHandler(this.menuOsha03_Click);
            // 
            // panMain
            // 
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMain.Controls.Add(this.button1);
            this.panMain.Controls.Add(this.SSMagam);
            this.panMain.Controls.Add(this.panSub01);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.panMain.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panMain.Location = new System.Drawing.Point(0, 61);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(666, 267);
            this.panMain.TabIndex = 265;
            // 
            // SSMagam
            // 
            this.SSMagam.AccessibleDescription = "SSMagam, Sheet1, Row 0, Column 0, ";
            this.SSMagam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSMagam.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SSMagam.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SSMagam.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SSMagam.HorizontalScrollBar.Renderer = flatScrollBarRenderer3;
            this.SSMagam.HorizontalScrollBar.TabIndex = 86;
            this.SSMagam.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SSMagam.Location = new System.Drawing.Point(0, 28);
            this.SSMagam.Name = "SSMagam";
            this.SSMagam.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSMagam_Sheet1});
            this.SSMagam.Size = new System.Drawing.Size(664, 237);
            this.SSMagam.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SSMagam.TabIndex = 5;
            this.SSMagam.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SSMagam.VerticalScrollBar.Name = "";
            flatScrollBarRenderer4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SSMagam.VerticalScrollBar.Renderer = flatScrollBarRenderer4;
            this.SSMagam.VerticalScrollBar.TabIndex = 87;
            this.SSMagam.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // SSMagam_Sheet1
            // 
            this.SSMagam_Sheet1.Reset();
            this.SSMagam_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSMagam_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSMagam_Sheet1.ColumnCount = 5;
            this.SSMagam_Sheet1.RowCount = 1;
            this.SSMagam_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMagam_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMagam_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SSMagam_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMagam_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMagam_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMagam_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SSMagam_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMagam_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMagam_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMagam_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SSMagam_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMagam_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMagam_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMagam_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.SSMagam_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMagam_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMagam_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMagam_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.SSMagam_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMagam_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSMagam_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSMagam_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMagam_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMagam_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.SSMagam_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMagam_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSMagam_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSMagam_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SSMagam_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSMagam_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSMagam_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSub01
            // 
            this.panSub01.BackColor = System.Drawing.Color.RoyalBlue;
            this.panSub01.Controls.Add(this.lblTitleSub0);
            this.panSub01.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub01.Location = new System.Drawing.Point(0, 0);
            this.panSub01.Name = "panSub01";
            this.panSub01.Size = new System.Drawing.Size(664, 28);
            this.panSub01.TabIndex = 2;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 9);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(57, 12);
            this.lblTitleSub0.TabIndex = 23;
            this.lblTitleSub0.Text = "마감업무";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlCall
            // 
            this.pnlCall.BackColor = System.Drawing.SystemColors.Window;
            this.pnlCall.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCall.Controls.Add(this.dateTimePicker1);
            this.pnlCall.Controls.Add(this.btnStart);
            this.pnlCall.Controls.Add(this.btnExit);
            this.pnlCall.Controls.Add(this.lblTitle);
            this.pnlCall.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCall.Location = new System.Drawing.Point(0, 0);
            this.pnlCall.Name = "pnlCall";
            this.pnlCall.Size = new System.Drawing.Size(666, 37);
            this.pnlCall.TabIndex = 257;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(347, 7);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(104, 25);
            this.dateTimePicker1.TabIndex = 34;
            // 
            // btnStart
            // 
            this.btnStart.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnStart.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnStart.Location = new System.Drawing.Point(500, 0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(82, 35);
            this.btnStart.TabIndex = 33;
            this.btnStart.Text = "시작/중지";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(582, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 35);
            this.btnExit.TabIndex = 31;
            this.btnExit.Text = "닫 기(&X)";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(6, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(248, 21);
            this.lblTitle.TabIndex = 30;
            this.lblTitle.Text = "건강증진센터 마감 및 통계 Build";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.prsBar);
            this.panel2.Controls.Add(this.panSub02);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 600);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(666, 28);
            this.panel2.TabIndex = 267;
            // 
            // prsBar
            // 
            this.prsBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.prsBar.Location = new System.Drawing.Point(89, -3);
            this.prsBar.Name = "prsBar";
            this.prsBar.Size = new System.Drawing.Size(575, 29);
            this.prsBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prsBar.TabIndex = 125;
            this.prsBar.Value = 50;
            // 
            // panSub02
            // 
            this.panSub02.BackColor = System.Drawing.Color.RoyalBlue;
            this.panSub02.Controls.Add(this.label1);
            this.panSub02.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub02.Location = new System.Drawing.Point(0, 0);
            this.panSub02.Name = "panSub02";
            this.panSub02.Size = new System.Drawing.Size(89, 26);
            this.panSub02.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(13, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "진행율(%)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panMagam
            // 
            this.panMagam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMagam.Controls.Add(this.SSTong);
            this.panMagam.Controls.Add(this.tabControl1);
            this.panMagam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMagam.Location = new System.Drawing.Point(0, 328);
            this.panMagam.Name = "panMagam";
            this.panMagam.Size = new System.Drawing.Size(666, 272);
            this.panMagam.TabIndex = 268;
            // 
            // SSTong
            // 
            this.SSTong.AccessibleDescription = "SSTong, Sheet1, Row 0, Column 0, ";
            this.SSTong.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSTong.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SSTong.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SSTong.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SSTong.HorizontalScrollBar.Renderer = flatScrollBarRenderer5;
            this.SSTong.HorizontalScrollBar.TabIndex = 88;
            this.SSTong.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SSTong.Location = new System.Drawing.Point(0, 31);
            this.SSTong.Name = "SSTong";
            this.SSTong.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSTong_Sheet1});
            this.SSTong.Size = new System.Drawing.Size(664, 239);
            this.SSTong.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SSTong.TabIndex = 6;
            this.SSTong.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SSTong.VerticalScrollBar.Name = "";
            flatScrollBarRenderer6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SSTong.VerticalScrollBar.Renderer = flatScrollBarRenderer6;
            this.SSTong.VerticalScrollBar.TabIndex = 89;
            this.SSTong.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // SSTong_Sheet1
            // 
            this.SSTong_Sheet1.Reset();
            this.SSTong_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSTong_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSTong_Sheet1.ColumnCount = 5;
            this.SSTong_Sheet1.RowCount = 1;
            this.SSTong_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSTong_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSTong_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SSTong_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSTong_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSTong_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSTong_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SSTong_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSTong_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSTong_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSTong_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SSTong_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSTong_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSTong_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSTong_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.SSTong_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSTong_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSTong_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSTong_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.SSTong_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSTong_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSTong_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSTong_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSTong_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSTong_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.SSTong_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSTong_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSTong_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSTong_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SSTong_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSTong_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSTong_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(664, 31);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(656, 1);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "일일통계";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(656, 1);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "월별통계";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(411, 153);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmMDI_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 628);
            this.Controls.Add(this.panMagam);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.pnlCall);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMDI_Main";
            this.Text = "건강증진센터 마감 및 통계 Build";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSMagam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSMagam_Sheet1)).EndInit();
            this.panSub01.ResumeLayout(false);
            this.panSub01.PerformLayout();
            this.pnlCall.ResumeLayout(false);
            this.pnlCall.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panSub02.ResumeLayout(false);
            this.panSub02.PerformLayout();
            this.panMagam.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSTong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSTong_Sheet1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuJep00;
        private System.Windows.Forms.ToolStripMenuItem menuPat00;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.Panel pnlCall;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panSub01;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ProgressBar prsBar;
        private System.Windows.Forms.Panel panSub02;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panMagam;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private FarPoint.Win.Spread.FpSpread SSMagam;
        private FarPoint.Win.Spread.SheetView SSMagam_Sheet1;
        private FarPoint.Win.Spread.FpSpread SSTong;
        private FarPoint.Win.Spread.SheetView SSTong_Sheet1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ToolStripMenuItem menuOsha;
        private System.Windows.Forms.ToolStripMenuItem menuOsha01;
        private System.Windows.Forms.ToolStripMenuItem menuOsha02;
        private System.Windows.Forms.ToolStripMenuItem menuOsha03;
        private System.Windows.Forms.Button button1;
    }
}