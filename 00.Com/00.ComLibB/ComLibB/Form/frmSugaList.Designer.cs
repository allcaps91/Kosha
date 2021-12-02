namespace ComLibB
{
    partial class frmSugaList
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color352636452396996672255", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text416636452396996682516", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static520636452396996692543");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Color542636452396996697560");
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Static578636452396996707347");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Color600636452396996712362");
            FarPoint.Win.Spread.NamedStyle namedStyle7 = new FarPoint.Win.Spread.NamedStyle("Static636636452396996722386");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle8 = new FarPoint.Win.Spread.NamedStyle("Static708636452396996742454");
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle9 = new FarPoint.Win.Spread.NamedStyle("Color962636452396996839085");
            FarPoint.Win.Spread.NamedStyle namedStyle10 = new FarPoint.Win.Spread.NamedStyle("Static998636452396996849109");
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle11 = new FarPoint.Win.Spread.NamedStyle("Static1056636452396996899248");
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle12 = new FarPoint.Win.Spread.NamedStyle("Static1114636452396996914558");
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle13 = new FarPoint.Win.Spread.NamedStyle("Static1132636452396996919567");
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panSub01 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSuNameK = new System.Windows.Forms.TextBox();
            this.chkGroup = new System.Windows.Forms.CheckBox();
            this.txtCDate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSearchView = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.ss1 = new FarPoint.Win.Spread.FpSpread();
            this.ss1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panSub01.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1314, 41);
            this.panTitle.TabIndex = 13;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(184, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "수가코드입력 목록조회";
            // 
            // panSub01
            // 
            this.panSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub01.Controls.Add(this.groupBox1);
            this.panSub01.Controls.Add(this.chkGroup);
            this.panSub01.Controls.Add(this.txtCDate);
            this.panSub01.Controls.Add(this.label5);
            this.panSub01.Controls.Add(this.btnPrint);
            this.panSub01.Controls.Add(this.btnSearchView);
            this.panSub01.Controls.Add(this.btnExit);
            this.panSub01.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub01.Location = new System.Drawing.Point(0, 41);
            this.panSub01.Name = "panSub01";
            this.panSub01.Size = new System.Drawing.Size(1314, 45);
            this.panSub01.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.txtSuNameK);
            this.groupBox1.Location = new System.Drawing.Point(618, -10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(237, 60);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(124, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(111, 43);
            this.btnSearch.TabIndex = 19;
            this.btnSearch.Text = "명칭으로 조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSuNameK
            // 
            this.txtSuNameK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.txtSuNameK.Location = new System.Drawing.Point(6, 20);
            this.txtSuNameK.Name = "txtSuNameK";
            this.txtSuNameK.Size = new System.Drawing.Size(112, 21);
            this.txtSuNameK.TabIndex = 18;
            this.txtSuNameK.Text = "txtSuNameK";
            this.txtSuNameK.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkGroup
            // 
            this.chkGroup.AutoSize = true;
            this.chkGroup.Location = new System.Drawing.Point(341, 13);
            this.chkGroup.Name = "chkGroup";
            this.chkGroup.Size = new System.Drawing.Size(86, 16);
            this.chkGroup.TabIndex = 16;
            this.chkGroup.Text = "Group 풀기";
            this.chkGroup.UseVisualStyleBackColor = true;
            // 
            // txtCDate
            // 
            this.txtCDate.Location = new System.Drawing.Point(501, 11);
            this.txtCDate.Name = "txtCDate";
            this.txtCDate.Size = new System.Drawing.Size(108, 21);
            this.txtCDate.TabIndex = 14;
            this.txtCDate.Text = "txtCDate";
            this.txtCDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCDate.DoubleClick += new System.EventHandler(this.txtCDate_DoubleClick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(435, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "기준일자";
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPrint.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.Location = new System.Drawing.Point(213, 0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(111, 43);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "목록인쇄";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSearchView
            // 
            this.btnSearchView.BackColor = System.Drawing.Color.Transparent;
            this.btnSearchView.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSearchView.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearchView.Location = new System.Drawing.Point(0, 0);
            this.btnSearchView.Name = "btnSearchView";
            this.btnSearchView.Size = new System.Drawing.Size(213, 43);
            this.btnSearchView.TabIndex = 7;
            this.btnSearchView.Text = "검색조건설정 및 자료조회";
            this.btnSearchView.UseVisualStyleBackColor = false;
            this.btnSearchView.Click += new System.EventHandler(this.btnSearchView_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1201, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(111, 43);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 86);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1314, 28);
            this.panTitleSub0.TabIndex = 20;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(5, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(101, 12);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "수가코드 리스트";
            // 
            // ss1
            // 
            this.ss1.AccessibleDescription = "ss1, Sheet1, Row 0, Column 0, ";
            this.ss1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ss1.Location = new System.Drawing.Point(0, 114);
            this.ss1.Name = "ss1";
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 60;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType2.Static = true;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            namedStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            textCellType3.Static = true;
            namedStyle5.CellType = textCellType3;
            namedStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Renderer = textCellType3;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(208)))));
            namedStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle6.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle6.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(208)))));
            textCellType4.Static = true;
            namedStyle7.CellType = textCellType4;
            namedStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle7.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle7.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle7.Renderer = textCellType4;
            namedStyle7.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType5.Static = true;
            namedStyle8.CellType = textCellType5;
            namedStyle8.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle8.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle8.Renderer = textCellType5;
            namedStyle8.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            namedStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle9.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle9.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle9.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            textCellType6.Static = true;
            namedStyle10.CellType = textCellType6;
            namedStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle10.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle10.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle10.Renderer = textCellType6;
            namedStyle10.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            textCellType7.Static = true;
            namedStyle11.CellType = textCellType7;
            namedStyle11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle11.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle11.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle11.Renderer = textCellType7;
            namedStyle11.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            textCellType8.Static = true;
            namedStyle12.CellType = textCellType8;
            namedStyle12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle12.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle12.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle12.Renderer = textCellType8;
            namedStyle12.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType9.Static = true;
            namedStyle13.CellType = textCellType9;
            namedStyle13.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle13.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle13.Renderer = textCellType9;
            namedStyle13.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5,
            namedStyle6,
            namedStyle7,
            namedStyle8,
            namedStyle9,
            namedStyle10,
            namedStyle11,
            namedStyle12,
            namedStyle13});
            this.ss1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss1_Sheet1});
            this.ss1.Size = new System.Drawing.Size(1314, 703);
            this.ss1.TabIndex = 21;
            this.ss1.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ss1.TextTipAppearance = tipAppearance1;
            this.ss1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ss1.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ss1_CellClick);
            this.ss1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ss1_CellDoubleClick);
            // 
            // ss1_Sheet1
            // 
            this.ss1_Sheet1.Reset();
            this.ss1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss1_Sheet1.ColumnCount = 20;
            this.ss1_Sheet1.RowCount = 1;
            this.ss1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "수가코드";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "품목코드";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "A항";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "보험수가";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "수가코드 명칭";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "변경일자";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "E항";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "F항";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "P항";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "S항";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "U항";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "표준코드";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "표준단가";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "표준일자";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "의원수가";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "구표준코드";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "구분";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 17).Value = "보험수가2";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 18).Value = "표준계수";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 19).Value = "비급여고지";
            this.ss1_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.Columns.Get(0).Label = "수가코드";
            this.ss1_Sheet1.Columns.Get(0).StyleName = "Static520636452396996692543";
            this.ss1_Sheet1.Columns.Get(0).Width = 66F;
            this.ss1_Sheet1.Columns.Get(1).Label = "품목코드";
            this.ss1_Sheet1.Columns.Get(1).Visible = false;
            this.ss1_Sheet1.Columns.Get(2).Label = "A항";
            this.ss1_Sheet1.Columns.Get(2).StyleName = "Static578636452396996707347";
            this.ss1_Sheet1.Columns.Get(2).Width = 32F;
            this.ss1_Sheet1.Columns.Get(3).Label = "보험수가";
            this.ss1_Sheet1.Columns.Get(3).StyleName = "Static636636452396996722386";
            this.ss1_Sheet1.Columns.Get(3).Width = 66F;
            this.ss1_Sheet1.Columns.Get(4).Label = "수가코드 명칭";
            this.ss1_Sheet1.Columns.Get(4).StyleName = "Static520636452396996692543";
            this.ss1_Sheet1.Columns.Get(4).Width = 236F;
            this.ss1_Sheet1.Columns.Get(5).Label = "변경일자";
            this.ss1_Sheet1.Columns.Get(5).StyleName = "Static708636452396996742454";
            this.ss1_Sheet1.Columns.Get(5).Width = 78F;
            this.ss1_Sheet1.Columns.Get(6).Label = "E항";
            this.ss1_Sheet1.Columns.Get(6).StyleName = "Static578636452396996707347";
            this.ss1_Sheet1.Columns.Get(6).Width = 27F;
            this.ss1_Sheet1.Columns.Get(7).Label = "F항";
            this.ss1_Sheet1.Columns.Get(7).StyleName = "Static578636452396996707347";
            this.ss1_Sheet1.Columns.Get(7).Width = 26F;
            this.ss1_Sheet1.Columns.Get(8).Label = "P항";
            this.ss1_Sheet1.Columns.Get(8).StyleName = "Static578636452396996707347";
            this.ss1_Sheet1.Columns.Get(8).Width = 28F;
            this.ss1_Sheet1.Columns.Get(9).Label = "S항";
            this.ss1_Sheet1.Columns.Get(9).StyleName = "Static578636452396996707347";
            this.ss1_Sheet1.Columns.Get(9).Width = 28F;
            this.ss1_Sheet1.Columns.Get(10).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.ss1_Sheet1.Columns.Get(10).CellType = textCellType10;
            this.ss1_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(10).Label = "U항";
            this.ss1_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(10).Width = 28F;
            this.ss1_Sheet1.Columns.Get(11).Label = "표준코드";
            this.ss1_Sheet1.Columns.Get(11).StyleName = "Static998636452396996849109";
            this.ss1_Sheet1.Columns.Get(11).Width = 76F;
            this.ss1_Sheet1.Columns.Get(12).Label = "표준단가";
            this.ss1_Sheet1.Columns.Get(12).StyleName = "Static1056636452396996899248";
            this.ss1_Sheet1.Columns.Get(12).Width = 70F;
            this.ss1_Sheet1.Columns.Get(13).Label = "표준일자";
            this.ss1_Sheet1.Columns.Get(13).StyleName = "Static1114636452396996914558";
            this.ss1_Sheet1.Columns.Get(13).Width = 78F;
            this.ss1_Sheet1.Columns.Get(14).Label = "의원수가";
            this.ss1_Sheet1.Columns.Get(14).StyleName = "Static1132636452396996919567";
            this.ss1_Sheet1.Columns.Get(15).Label = "구표준코드";
            this.ss1_Sheet1.Columns.Get(15).Width = 73F;
            this.ss1_Sheet1.Columns.Get(16).Label = "구분";
            this.ss1_Sheet1.Columns.Get(16).Width = 73F;
            this.ss1_Sheet1.Columns.Get(17).Label = "보험수가2";
            this.ss1_Sheet1.Columns.Get(17).StyleName = "Static1132636452396996919567";
            this.ss1_Sheet1.Columns.Get(17).Width = 70F;
            this.ss1_Sheet1.Columns.Get(18).Label = "표준계수";
            this.ss1_Sheet1.Columns.Get(18).StyleName = "Static1132636452396996919567";
            this.ss1_Sheet1.Columns.Get(18).Width = 70F;
            this.ss1_Sheet1.Columns.Get(19).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(19).Label = "비급여고지";
            this.ss1_Sheet1.Columns.Get(19).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(19).Width = 67F;
            this.ss1_Sheet1.DefaultStyleName = "Text416636452396996682516";
            this.ss1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss1_Sheet1.RowHeader.Columns.Get(0).Width = 44F;
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmSugaList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1314, 817);
            this.Controls.Add(this.ss1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panSub01);
            this.Controls.Add(this.panTitle);
            this.Name = "frmSugaList";
            this.Text = "frmSugaList";
            this.Load += new System.EventHandler(this.frmSugaList_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panSub01.ResumeLayout(false);
            this.panSub01.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panSub01;
        private System.Windows.Forms.TextBox txtCDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSearchView;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSuNameK;
        private System.Windows.Forms.CheckBox chkGroup;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private FarPoint.Win.Spread.FpSpread ss1;
        private FarPoint.Win.Spread.SheetView ss1_Sheet1;
    }
}