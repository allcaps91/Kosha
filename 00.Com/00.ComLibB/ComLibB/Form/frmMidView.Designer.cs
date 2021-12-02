namespace ComLibB
{
    partial class frmMidView
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
            FarPoint.Win.Spread.NamedStyle namedStyle13 = new FarPoint.Win.Spread.NamedStyle("Color341636355813172094304", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle14 = new FarPoint.Win.Spread.NamedStyle("Text419636355813172114327", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle15 = new FarPoint.Win.Spread.NamedStyle("Static505636355813172124306");
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle16 = new FarPoint.Win.Spread.NamedStyle("Static865636355813172204301");
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance4 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.grbMsym = new System.Windows.Forms.GroupBox();
            this.chkMsym = new System.Windows.Forms.CheckBox();
            this.txtCode1 = new System.Windows.Forms.TextBox();
            this.txtCode2 = new System.Windows.Forms.TextBox();
            this.txtCode3 = new System.Windows.Forms.TextBox();
            this.grbDept = new System.Windows.Forms.GroupBox();
            this.cboDept = new System.Windows.Forms.ComboBox();
            this.grbDate = new System.Windows.Forms.GroupBox();
            this.lblItem0 = new System.Windows.Forms.Label();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.grbKind = new System.Windows.Forms.GroupBox();
            this.cboJong = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panTitleSub1 = new System.Windows.Forms.Panel();
            this.lblTitleSub1 = new System.Windows.Forms.Label();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.lblMsg = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panel3.SuspendLayout();
            this.grbMsym.SuspendLayout();
            this.grbDept.SuspendLayout();
            this.grbDate.SuspendLayout();
            this.grbKind.SuspendLayout();
            this.panTitleSub1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(824, 34);
            this.panTitle.TabIndex = 12;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(150, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "의무기록 조건검색";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(745, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 3;
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
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(824, 28);
            this.panTitleSub0.TabIndex = 13;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(62, 12);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "조회 조건";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.grbMsym);
            this.panel3.Controls.Add(this.grbDept);
            this.panel3.Controls.Add(this.grbDate);
            this.panel3.Controls.Add(this.grbKind);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 62);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(824, 87);
            this.panel3.TabIndex = 18;
            // 
            // grbMsym
            // 
            this.grbMsym.Controls.Add(this.chkMsym);
            this.grbMsym.Controls.Add(this.txtCode1);
            this.grbMsym.Controls.Add(this.txtCode2);
            this.grbMsym.Controls.Add(this.txtCode3);
            this.grbMsym.Location = new System.Drawing.Point(534, 8);
            this.grbMsym.Name = "grbMsym";
            this.grbMsym.Size = new System.Drawing.Size(198, 70);
            this.grbMsym.TabIndex = 48;
            this.grbMsym.TabStop = false;
            this.grbMsym.Text = "상병코드";
            // 
            // chkMsym
            // 
            this.chkMsym.AutoSize = true;
            this.chkMsym.Location = new System.Drawing.Point(9, 18);
            this.chkMsym.Name = "chkMsym";
            this.chkMsym.Size = new System.Drawing.Size(100, 16);
            this.chkMsym.TabIndex = 51;
            this.chkMsym.Text = "주상병만 찾기";
            this.chkMsym.UseVisualStyleBackColor = true;
            // 
            // txtCode1
            // 
            this.txtCode1.Location = new System.Drawing.Point(8, 37);
            this.txtCode1.Name = "txtCode1";
            this.txtCode1.Size = new System.Drawing.Size(60, 21);
            this.txtCode1.TabIndex = 31;
            this.txtCode1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode1_KeyDown);
            this.txtCode1.Leave += new System.EventHandler(this.txtCode1_Leave);
            // 
            // txtCode2
            // 
            this.txtCode2.Location = new System.Drawing.Point(69, 37);
            this.txtCode2.Name = "txtCode2";
            this.txtCode2.Size = new System.Drawing.Size(60, 21);
            this.txtCode2.TabIndex = 32;
            this.txtCode2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode2_KeyDown);
            this.txtCode2.Leave += new System.EventHandler(this.txtCode2_Leave);
            // 
            // txtCode3
            // 
            this.txtCode3.Location = new System.Drawing.Point(130, 37);
            this.txtCode3.Name = "txtCode3";
            this.txtCode3.Size = new System.Drawing.Size(60, 21);
            this.txtCode3.TabIndex = 48;
            this.txtCode3.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCode3_KeyDown);
            this.txtCode3.Leave += new System.EventHandler(this.txtCode3_Leave);
            // 
            // grbDept
            // 
            this.grbDept.Controls.Add(this.cboDept);
            this.grbDept.Location = new System.Drawing.Point(390, 8);
            this.grbDept.Name = "grbDept";
            this.grbDept.Size = new System.Drawing.Size(138, 70);
            this.grbDept.TabIndex = 42;
            this.grbDept.TabStop = false;
            this.grbDept.Text = "퇴원과";
            // 
            // cboDept
            // 
            this.cboDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDept.FormattingEnabled = true;
            this.cboDept.Location = new System.Drawing.Point(9, 28);
            this.cboDept.Name = "cboDept";
            this.cboDept.Size = new System.Drawing.Size(121, 20);
            this.cboDept.TabIndex = 50;
            // 
            // grbDate
            // 
            this.grbDate.Controls.Add(this.lblItem0);
            this.grbDate.Controls.Add(this.dtpFDate);
            this.grbDate.Controls.Add(this.dtpTDate);
            this.grbDate.Location = new System.Drawing.Point(155, 8);
            this.grbDate.Name = "grbDate";
            this.grbDate.Size = new System.Drawing.Size(229, 70);
            this.grbDate.TabIndex = 49;
            this.grbDate.TabStop = false;
            this.grbDate.Text = "작업기간";
            // 
            // lblItem0
            // 
            this.lblItem0.AutoSize = true;
            this.lblItem0.Location = new System.Drawing.Point(107, 32);
            this.lblItem0.Name = "lblItem0";
            this.lblItem0.Size = new System.Drawing.Size(14, 12);
            this.lblItem0.TabIndex = 24;
            this.lblItem0.Text = "~";
            // 
            // dtpFDate
            // 
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(7, 28);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(100, 21);
            this.dtpFDate.TabIndex = 47;
            this.dtpFDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtpFDate_KeyDown);
            // 
            // dtpTDate
            // 
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(121, 28);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(100, 21);
            this.dtpTDate.TabIndex = 46;
            this.dtpTDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtpTDate_KeyDown);
            // 
            // grbKind
            // 
            this.grbKind.Controls.Add(this.cboJong);
            this.grbKind.Location = new System.Drawing.Point(12, 8);
            this.grbKind.Name = "grbKind";
            this.grbKind.Size = new System.Drawing.Size(137, 70);
            this.grbKind.TabIndex = 50;
            this.grbKind.TabStop = false;
            this.grbKind.Text = "작업종류";
            // 
            // cboJong
            // 
            this.cboJong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboJong.FormattingEnabled = true;
            this.cboJong.Location = new System.Drawing.Point(8, 28);
            this.cboJong.Name = "cboJong";
            this.cboJong.Size = new System.Drawing.Size(121, 20);
            this.cboJong.TabIndex = 49;
            this.cboJong.SelectedIndexChanged += new System.EventHandler(this.cboJong_SelectedIndexChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(743, 28);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 27;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panTitleSub1
            // 
            this.panTitleSub1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub1.Controls.Add(this.lblTitleSub1);
            this.panTitleSub1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub1.Location = new System.Drawing.Point(0, 149);
            this.panTitleSub1.Name = "panTitleSub1";
            this.panTitleSub1.Size = new System.Drawing.Size(824, 28);
            this.panTitleSub1.TabIndex = 19;
            // 
            // lblTitleSub1
            // 
            this.lblTitleSub1.AutoSize = true;
            this.lblTitleSub1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub1.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub1.Location = new System.Drawing.Point(8, 6);
            this.lblTitleSub1.Name = "lblTitleSub1";
            this.lblTitleSub1.Size = new System.Drawing.Size(62, 12);
            this.lblTitleSub1.TabIndex = 1;
            this.lblTitleSub1.Text = "조회 결과";
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssView.Location = new System.Drawing.Point(0, 177);
            this.ssView.Name = "ssView";
            namedStyle13.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle13.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle13.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle13.Parent = "DataAreaDefault";
            namedStyle13.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType10.MaxLength = 60;
            namedStyle14.CellType = textCellType10;
            namedStyle14.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle14.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle14.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle14.Parent = "DataAreaDefault";
            namedStyle14.Renderer = textCellType10;
            namedStyle14.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType11.Static = true;
            namedStyle15.CellType = textCellType11;
            namedStyle15.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle15.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle15.Renderer = textCellType11;
            namedStyle15.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType12.Static = true;
            namedStyle16.CellType = textCellType12;
            namedStyle16.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle16.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle16.Renderer = textCellType12;
            namedStyle16.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle13,
            namedStyle14,
            namedStyle15,
            namedStyle16});
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(824, 471);
            this.ssView.TabIndex = 47;
            this.ssView.TabStripRatio = 0.6D;
            tipAppearance4.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance4.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssView.TextTipAppearance = tipAppearance4;
            this.ssView.LeaveCell += new FarPoint.Win.Spread.LeaveCellEventHandler(this.ssView_LeaveCell);
            this.ssView.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView_CellDoubleClick);
            this.ssView.SetViewportLeftColumn(0, 0, 3);
            this.ssView.SetActiveViewport(0, 0, -1);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 28;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "퇴원일자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "성 명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "입원과";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "퇴원과";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "퇴원의사";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "입원일자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "재원일수";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "성별";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "나이";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "종류";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "퇴원";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "결과";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "상병1";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "상병2";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "상병3";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "상병4";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 17).Value = "수술1";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 18).Value = "수술2";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 19).Value = "수술3";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 20).Value = "사망";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 21).Value = "전신마취";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 22).Value = "부분마취";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 23).Value = "국소마취";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 24).Value = "신생아";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 25).Value = "생검1";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 26).Value = "생검2";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 27).Value = "산모";
            this.ssView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 32F;
            this.ssView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Get(0).Label = "등록번호";
            this.ssView_Sheet1.Columns.Get(0).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(1).Label = "퇴원일자";
            this.ssView_Sheet1.Columns.Get(1).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(1).Width = 80F;
            this.ssView_Sheet1.Columns.Get(2).Label = "성 명";
            this.ssView_Sheet1.Columns.Get(2).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(2).Width = 80F;
            this.ssView_Sheet1.Columns.Get(3).Label = "입원과";
            this.ssView_Sheet1.Columns.Get(3).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(3).Width = 45F;
            this.ssView_Sheet1.Columns.Get(4).Label = "퇴원과";
            this.ssView_Sheet1.Columns.Get(4).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(4).Width = 45F;
            this.ssView_Sheet1.Columns.Get(5).Label = "퇴원의사";
            this.ssView_Sheet1.Columns.Get(5).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(5).Width = 61F;
            this.ssView_Sheet1.Columns.Get(6).Label = "입원일자";
            this.ssView_Sheet1.Columns.Get(6).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(6).Width = 80F;
            this.ssView_Sheet1.Columns.Get(7).Label = "재원일수";
            this.ssView_Sheet1.Columns.Get(7).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(7).Width = 34F;
            this.ssView_Sheet1.Columns.Get(8).Label = "성별";
            this.ssView_Sheet1.Columns.Get(8).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(8).Width = 23F;
            this.ssView_Sheet1.Columns.Get(9).Label = "나이";
            this.ssView_Sheet1.Columns.Get(9).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(9).Width = 35F;
            this.ssView_Sheet1.Columns.Get(10).Label = "종류";
            this.ssView_Sheet1.Columns.Get(10).StyleName = "Static865636355813172204301";
            this.ssView_Sheet1.Columns.Get(10).Width = 69F;
            this.ssView_Sheet1.Columns.Get(11).Label = "퇴원";
            this.ssView_Sheet1.Columns.Get(11).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(11).Width = 32F;
            this.ssView_Sheet1.Columns.Get(12).Label = "결과";
            this.ssView_Sheet1.Columns.Get(12).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(12).Width = 38F;
            this.ssView_Sheet1.Columns.Get(13).Label = "상병1";
            this.ssView_Sheet1.Columns.Get(13).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(14).Label = "상병2";
            this.ssView_Sheet1.Columns.Get(14).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(15).Label = "상병3";
            this.ssView_Sheet1.Columns.Get(15).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(16).Label = "상병4";
            this.ssView_Sheet1.Columns.Get(16).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(17).Label = "수술1";
            this.ssView_Sheet1.Columns.Get(17).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(18).Label = "수술2";
            this.ssView_Sheet1.Columns.Get(18).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(19).Label = "수술3";
            this.ssView_Sheet1.Columns.Get(19).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(20).Label = "사망";
            this.ssView_Sheet1.Columns.Get(20).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(20).Width = 36F;
            this.ssView_Sheet1.Columns.Get(21).Label = "전신마취";
            this.ssView_Sheet1.Columns.Get(21).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(21).Width = 35F;
            this.ssView_Sheet1.Columns.Get(22).Label = "부분마취";
            this.ssView_Sheet1.Columns.Get(22).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(22).Width = 37F;
            this.ssView_Sheet1.Columns.Get(23).Label = "국소마취";
            this.ssView_Sheet1.Columns.Get(23).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(23).Width = 35F;
            this.ssView_Sheet1.Columns.Get(24).Label = "신생아";
            this.ssView_Sheet1.Columns.Get(24).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(24).Width = 49F;
            this.ssView_Sheet1.Columns.Get(25).Label = "생검1";
            this.ssView_Sheet1.Columns.Get(25).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(25).Width = 42F;
            this.ssView_Sheet1.Columns.Get(26).Label = "생검2";
            this.ssView_Sheet1.Columns.Get(26).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(26).Width = 42F;
            this.ssView_Sheet1.Columns.Get(27).Label = "산모";
            this.ssView_Sheet1.Columns.Get(27).StyleName = "Static505636355813172124306";
            this.ssView_Sheet1.Columns.Get(27).Width = 38F;
            this.ssView_Sheet1.DefaultStyleName = "Text419636355813172114327";
            this.ssView_Sheet1.FrozenColumnCount = 3;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.Rows.Get(0).Height = 17F;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // lblMsg
            // 
            this.lblMsg.BackColor = System.Drawing.Color.White;
            this.lblMsg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMsg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblMsg.Location = new System.Drawing.Point(0, 648);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(824, 35);
            this.lblMsg.TabIndex = 52;
            this.lblMsg.Text = " ";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmMidView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 683);
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.panTitleSub1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmMidView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "의무기록 조건검색";
            this.Load += new System.EventHandler(this.frmMidView_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.grbMsym.ResumeLayout(false);
            this.grbMsym.PerformLayout();
            this.grbDept.ResumeLayout(false);
            this.grbDate.ResumeLayout(false);
            this.grbDate.PerformLayout();
            this.grbKind.ResumeLayout(false);
            this.panTitleSub1.ResumeLayout(false);
            this.panTitleSub1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtCode2;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.TextBox txtCode1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblItem0;
        private System.Windows.Forms.Panel panTitleSub1;
        private System.Windows.Forms.Label lblTitleSub1;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.GroupBox grbMsym;
        private System.Windows.Forms.GroupBox grbDept;
        private System.Windows.Forms.GroupBox grbDate;
        private System.Windows.Forms.GroupBox grbKind;
        private System.Windows.Forms.TextBox txtCode3;
        private System.Windows.Forms.ComboBox cboDept;
        private System.Windows.Forms.ComboBox cboJong;
        private System.Windows.Forms.CheckBox chkMsym;
        private System.Windows.Forms.Label lblMsg;
    }
}