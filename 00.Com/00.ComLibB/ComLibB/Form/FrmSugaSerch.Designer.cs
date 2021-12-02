namespace ComLibB
{
    partial class FrmSugaSerch
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
        private void InitializeComponent ()
        {
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.DateTimeCellType dateTimeCellType1 = new FarPoint.Win.Spread.CellType.DateTimeCellType();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSugaSerch));
            FarPoint.Win.Spread.CellType.DateTimeCellType dateTimeCellType2 = new FarPoint.Win.Spread.CellType.DateTimeCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.pan0 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pan2 = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pan1 = new System.Windows.Forms.Panel();
            this.grbCheeke = new System.Windows.Forms.GroupBox();
            this.optDelet = new System.Windows.Forms.RadioButton();
            this.optActive = new System.Windows.Forms.RadioButton();
            this.optAll = new System.Windows.Forms.RadioButton();
            this.grbCode = new System.Windows.Forms.GroupBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.grbGubun = new System.Windows.Forms.GroupBox();
            this.optGanGaga = new System.Windows.Forms.RadioButton();
            this.optNameCode = new System.Windows.Forms.RadioButton();
            this.optSugaCode = new System.Windows.Forms.RadioButton();
            this.optName = new System.Windows.Forms.RadioButton();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.pan0.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pan2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.pan1.SuspendLayout();
            this.grbCheeke.SuspendLayout();
            this.grbCode.SuspendLayout();
            this.grbGubun.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pan0
            // 
            this.pan0.Controls.Add(this.panel2);
            this.pan0.Controls.Add(this.panTitleSub0);
            this.pan0.Controls.Add(this.panTitle);
            this.pan0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan0.Location = new System.Drawing.Point(0, 0);
            this.pan0.Name = "pan0";
            this.pan0.Size = new System.Drawing.Size(1060, 567);
            this.pan0.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.pan2);
            this.panel2.Controls.Add(this.pan1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 62);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1060, 505);
            this.panel2.TabIndex = 13;
            // 
            // pan2
            // 
            this.pan2.BackColor = System.Drawing.Color.White;
            this.pan2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pan2.Controls.Add(this.ssView);
            this.pan2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan2.Location = new System.Drawing.Point(0, 46);
            this.pan2.Name = "pan2";
            this.pan2.Size = new System.Drawing.Size(1056, 455);
            this.pan2.TabIndex = 19;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.Location = new System.Drawing.Point(0, 0);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(1052, 451);
            this.ssView.TabIndex = 0;
            this.ssView.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView_CellDoubleClick);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 13;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "분류";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "누적";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "수가코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "품명코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "한글명칭";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "성분코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "한글수가";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "보험수가";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "자보수가";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "일반수가";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "적용일자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "삭제일자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "표준코드";
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Label = "분류";
            this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Width = 32F;
            this.ssView_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Label = "누적";
            this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Width = 31F;
            this.ssView_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Label = "수가코드";
            this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Width = 82F;
            this.ssView_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Label = "품명코드";
            this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Width = 81F;
            this.ssView_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).Label = "한글명칭";
            this.ssView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).Width = 494F;
            this.ssView_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Label = "성분코드";
            this.ssView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Width = 88F;
            this.ssView_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.ssView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Label = "한글수가";
            this.ssView_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).CellType = textCellType8;
            this.ssView_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Label = "보험수가";
            this.ssView_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).CellType = textCellType9;
            this.ssView_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).Label = "자보수가";
            this.ssView_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(9).CellType = textCellType10;
            this.ssView_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(9).Label = "일반수가";
            this.ssView_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            dateTimeCellType1.Calendar = new System.Globalization.GregorianCalendar(System.Globalization.GregorianCalendarTypes.Localized);
            dateTimeCellType1.CalendarSurroundingDaysColor = System.Drawing.SystemColors.GrayText;
            dateTimeCellType1.MaximumTime = System.TimeSpan.Parse("23:59:59.9999999");
            dateTimeCellType1.TimeDefault = new System.DateTime(2019, 10, 11, 18, 25, 13, 0);
            this.ssView_Sheet1.Columns.Get(10).CellType = dateTimeCellType1;
            this.ssView_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(10).Label = "적용일자";
            this.ssView_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(10).Width = 91F;
            dateTimeCellType2.Calendar = new System.Globalization.GregorianCalendar(System.Globalization.GregorianCalendarTypes.Localized);
            dateTimeCellType2.CalendarSurroundingDaysColor = System.Drawing.SystemColors.GrayText;
            dateTimeCellType2.MaximumTime = System.TimeSpan.Parse("23:59:59.9999999");
            dateTimeCellType2.TimeDefault = new System.DateTime(2019, 10, 11, 18, 25, 13, 0);
            this.ssView_Sheet1.Columns.Get(11).CellType = dateTimeCellType2;
            this.ssView_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(11).Label = "삭제일자";
            this.ssView_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(11).Width = 90F;
            this.ssView_Sheet1.Columns.Get(12).CellType = textCellType11;
            this.ssView_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(12).Label = "표준코드";
            this.ssView_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pan1
            // 
            this.pan1.BackColor = System.Drawing.Color.White;
            this.pan1.Controls.Add(this.btnPrint);
            this.pan1.Controls.Add(this.grbCheeke);
            this.pan1.Controls.Add(this.grbCode);
            this.pan1.Controls.Add(this.btnExit);
            this.pan1.Controls.Add(this.btnSearch);
            this.pan1.Controls.Add(this.grbGubun);
            this.pan1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan1.Location = new System.Drawing.Point(0, 0);
            this.pan1.Name = "pan1";
            this.pan1.Size = new System.Drawing.Size(1056, 46);
            this.pan1.TabIndex = 17;
            // 
            // grbCheeke
            // 
            this.grbCheeke.Controls.Add(this.optDelet);
            this.grbCheeke.Controls.Add(this.optActive);
            this.grbCheeke.Controls.Add(this.optAll);
            this.grbCheeke.Location = new System.Drawing.Point(428, 4);
            this.grbCheeke.Name = "grbCheeke";
            this.grbCheeke.Size = new System.Drawing.Size(149, 40);
            this.grbCheeke.TabIndex = 43;
            this.grbCheeke.TabStop = false;
            // 
            // optDelet
            // 
            this.optDelet.AutoSize = true;
            this.optDelet.Location = new System.Drawing.Point(100, 15);
            this.optDelet.Name = "optDelet";
            this.optDelet.Size = new System.Drawing.Size(47, 16);
            this.optDelet.TabIndex = 40;
            this.optDelet.TabStop = true;
            this.optDelet.Text = "삭제";
            this.optDelet.UseVisualStyleBackColor = true;
            // 
            // optActive
            // 
            this.optActive.AutoSize = true;
            this.optActive.Location = new System.Drawing.Point(53, 15);
            this.optActive.Name = "optActive";
            this.optActive.Size = new System.Drawing.Size(47, 16);
            this.optActive.TabIndex = 39;
            this.optActive.TabStop = true;
            this.optActive.Text = "사용";
            this.optActive.UseVisualStyleBackColor = true;
            // 
            // optAll
            // 
            this.optAll.AutoSize = true;
            this.optAll.Location = new System.Drawing.Point(6, 14);
            this.optAll.Name = "optAll";
            this.optAll.Size = new System.Drawing.Size(47, 16);
            this.optAll.TabIndex = 38;
            this.optAll.TabStop = true;
            this.optAll.Text = "전체";
            this.optAll.UseVisualStyleBackColor = true;
            // 
            // grbCode
            // 
            this.grbCode.Controls.Add(this.txtCode);
            this.grbCode.Location = new System.Drawing.Point(273, 4);
            this.grbCode.Name = "grbCode";
            this.grbCode.Size = new System.Drawing.Size(149, 40);
            this.grbCode.TabIndex = 42;
            this.grbCode.TabStop = false;
            this.grbCode.Text = "찾으실 코드?";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(10, 14);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(132, 21);
            this.txtCode.TabIndex = 31;
            this.txtCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode_KeyDown);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(981, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(835, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 27;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // grbGubun
            // 
            this.grbGubun.Controls.Add(this.optGanGaga);
            this.grbGubun.Controls.Add(this.optNameCode);
            this.grbGubun.Controls.Add(this.optSugaCode);
            this.grbGubun.Controls.Add(this.optName);
            this.grbGubun.Location = new System.Drawing.Point(3, 4);
            this.grbGubun.Name = "grbGubun";
            this.grbGubun.Size = new System.Drawing.Size(264, 40);
            this.grbGubun.TabIndex = 41;
            this.grbGubun.TabStop = false;
            this.grbGubun.Text = "찾기 구분";
            // 
            // optGanGaga
            // 
            this.optGanGaga.AutoSize = true;
            this.optGanGaga.Location = new System.Drawing.Point(191, 14);
            this.optGanGaga.Name = "optGanGaga";
            this.optGanGaga.Size = new System.Drawing.Size(71, 16);
            this.optGanGaga.TabIndex = 43;
            this.optGanGaga.TabStop = true;
            this.optGanGaga.Text = "표준코드";
            this.optGanGaga.UseVisualStyleBackColor = true;
            // 
            // optNameCode
            // 
            this.optNameCode.AutoSize = true;
            this.optNameCode.Location = new System.Drawing.Point(120, 14);
            this.optNameCode.Name = "optNameCode";
            this.optNameCode.Size = new System.Drawing.Size(71, 16);
            this.optNameCode.TabIndex = 42;
            this.optNameCode.TabStop = true;
            this.optNameCode.Text = "품명코드";
            this.optNameCode.UseVisualStyleBackColor = true;
            // 
            // optSugaCode
            // 
            this.optSugaCode.AutoSize = true;
            this.optSugaCode.Location = new System.Drawing.Point(49, 14);
            this.optSugaCode.Name = "optSugaCode";
            this.optSugaCode.Size = new System.Drawing.Size(71, 16);
            this.optSugaCode.TabIndex = 41;
            this.optSugaCode.TabStop = true;
            this.optSugaCode.Text = "수가코드";
            this.optSugaCode.UseVisualStyleBackColor = true;
            // 
            // optName
            // 
            this.optName.AutoSize = true;
            this.optName.Location = new System.Drawing.Point(2, 14);
            this.optName.Name = "optName";
            this.optName.Size = new System.Drawing.Size(47, 16);
            this.optName.TabIndex = 40;
            this.optName.TabStop = true;
            this.optName.Text = "명칭";
            this.optName.UseVisualStyleBackColor = true;
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1060, 28);
            this.panTitleSub0.TabIndex = 12;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(93, 12);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "수가 코드 찾기";
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1060, 34);
            this.panTitle.TabIndex = 11;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(122, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "수가 코드 찾기";
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Location = new System.Drawing.Point(908, 2);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 44;
            this.btnPrint.Text = "인쇄";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // FrmSugaSerch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 567);
            this.Controls.Add(this.pan0);
            this.Name = "FrmSugaSerch";
            this.Text = "수가 코드 찾기";
            this.Load += new System.EventHandler(this.FrmSugaSerch_Load);
            this.pan0.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pan2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.pan1.ResumeLayout(false);
            this.grbCheeke.ResumeLayout(false);
            this.grbCheeke.PerformLayout();
            this.grbCode.ResumeLayout(false);
            this.grbCode.PerformLayout();
            this.grbGubun.ResumeLayout(false);
            this.grbGubun.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan0;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pan2;
        private System.Windows.Forms.Panel pan1;
        private System.Windows.Forms.GroupBox grbCheeke;
        private System.Windows.Forms.RadioButton optDelet;
        private System.Windows.Forms.RadioButton optActive;
        private System.Windows.Forms.RadioButton optAll;
        private System.Windows.Forms.GroupBox grbCode;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.GroupBox grbGubun;
        private System.Windows.Forms.RadioButton optGanGaga;
        private System.Windows.Forms.RadioButton optNameCode;
        private System.Windows.Forms.RadioButton optSugaCode;
        private System.Windows.Forms.RadioButton optName;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.Button btnPrint;
    }
}