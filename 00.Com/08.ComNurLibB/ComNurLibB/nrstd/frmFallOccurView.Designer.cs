namespace ComNurLibB
{
    partial class frmFallOccurView
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
            FarPoint.Win.Spread.NamedStyle namedStyle13 = new FarPoint.Win.Spread.NamedStyle("BorderEx360636529121414022924", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder7 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.NamedStyle namedStyle14 = new FarPoint.Win.Spread.NamedStyle("Static446636529121414179183", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder8 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle15 = new FarPoint.Win.Spread.NamedStyle("Color590636529121414179183");
            FarPoint.Win.Spread.NamedStyle namedStyle16 = new FarPoint.Win.Spread.NamedStyle("Static716636529121414179183");
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance4 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType19 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType20 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExcel = new System.Windows.Forms.Button();
            this.dtpDate1 = new System.Windows.Forms.DateTimePicker();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.cboWard = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblItem1 = new System.Windows.Forms.Label();
            this.lblItem0 = new System.Windows.Forms.Label();
            this.panTitleSub1 = new System.Windows.Forms.Panel();
            this.lblTitleSub1 = new System.Windows.Forms.Label();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdoGubun1 = new System.Windows.Forms.RadioButton();
            this.rdoGubun0 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panTitleSub1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
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
            this.panTitle.Size = new System.Drawing.Size(976, 34);
            this.panTitle.TabIndex = 12;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(122, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "낙상발생리스트";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(897, 0);
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
            this.panTitleSub0.Size = new System.Drawing.Size(976, 28);
            this.panTitleSub0.TabIndex = 13;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(59, 15);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "조회 조건";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Controls.Add(this.btnExcel);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.btnPrint);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 62);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(976, 37);
            this.panel3.TabIndex = 18;
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.BackColor = System.Drawing.Color.Transparent;
            this.btnExcel.Location = new System.Drawing.Point(891, 3);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(72, 30);
            this.btnExcel.TabIndex = 47;
            this.btnExcel.Text = "엑셀";
            this.btnExcel.UseVisualStyleBackColor = false;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // dtpDate1
            // 
            this.dtpDate1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate1.Location = new System.Drawing.Point(192, 6);
            this.dtpDate1.Name = "dtpDate1";
            this.dtpDate1.Size = new System.Drawing.Size(103, 25);
            this.dtpDate1.TabIndex = 46;
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(72, 6);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(103, 25);
            this.dtpDate.TabIndex = 46;
            // 
            // cboWard
            // 
            this.cboWard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWard.FormattingEnabled = true;
            this.cboWard.Location = new System.Drawing.Point(46, 6);
            this.cboWard.Name = "cboWard";
            this.cboWard.Size = new System.Drawing.Size(90, 25);
            this.cboWard.TabIndex = 33;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(747, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 27;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Location = new System.Drawing.Point(819, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 22;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(175, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 17);
            this.label1.TabIndex = 25;
            this.label1.Text = "~";
            // 
            // lblItem1
            // 
            this.lblItem1.AutoSize = true;
            this.lblItem1.Location = new System.Drawing.Point(12, 10);
            this.lblItem1.Name = "lblItem1";
            this.lblItem1.Size = new System.Drawing.Size(60, 17);
            this.lblItem1.TabIndex = 25;
            this.lblItem1.Text = "조회기간";
            // 
            // lblItem0
            // 
            this.lblItem0.AutoSize = true;
            this.lblItem0.Location = new System.Drawing.Point(12, 10);
            this.lblItem0.Name = "lblItem0";
            this.lblItem0.Size = new System.Drawing.Size(34, 17);
            this.lblItem0.TabIndex = 24;
            this.lblItem0.Text = "병동";
            // 
            // panTitleSub1
            // 
            this.panTitleSub1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub1.Controls.Add(this.lblTitleSub1);
            this.panTitleSub1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub1.Location = new System.Drawing.Point(0, 99);
            this.panTitleSub1.Name = "panTitleSub1";
            this.panTitleSub1.Size = new System.Drawing.Size(976, 28);
            this.panTitleSub1.TabIndex = 19;
            // 
            // lblTitleSub1
            // 
            this.lblTitleSub1.AutoSize = true;
            this.lblTitleSub1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub1.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub1.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub1.Name = "lblTitleSub1";
            this.lblTitleSub1.Size = new System.Drawing.Size(59, 15);
            this.lblTitleSub1.TabIndex = 1;
            this.lblTitleSub1.Text = "조회 결과";
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssView.Location = new System.Drawing.Point(0, 127);
            this.ssView.Name = "ssView";
            namedStyle13.Border = complexBorder7;
            namedStyle13.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle13.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle13.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle13.Parent = "DataAreaDefault";
            namedStyle13.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle14.Border = complexBorder8;
            textCellType16.Static = true;
            namedStyle14.CellType = textCellType16;
            namedStyle14.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle14.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle14.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle14.Parent = "DataAreaDefault";
            namedStyle14.Renderer = textCellType16;
            namedStyle14.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(255)))), ((int)(((byte)(238)))));
            namedStyle15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle15.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle15.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle15.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType17.Static = true;
            namedStyle16.CellType = textCellType17;
            namedStyle16.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle16.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle16.Renderer = textCellType17;
            namedStyle16.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle13,
            namedStyle14,
            namedStyle15,
            namedStyle16});
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(976, 584);
            this.ssView.TabIndex = 47;
            this.ssView.TabStripRatio = 0.6D;
            tipAppearance4.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance4.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssView.TextTipAppearance = tipAppearance4;
            this.ssView.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView_CellDoubleClick);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 15;
            this.ssView_Sheet1.RowCount = 50;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "발생일자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "확인일시";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "보고일시";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "성명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "나이/성별";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "진료과";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "낙상유형";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "낙상발생장소";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "IPDNO";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "보고자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "구분";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "등록번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "발생일자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "부서";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "rowid";
            this.ssView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 37F;
            this.ssView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Get(0).AllowAutoSort = true;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Label = "발생일자";
            this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Width = 115F;
            this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Label = "확인일시";
            this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Width = 115F;
            this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Label = "보고일시";
            this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Width = 115F;
            this.ssView_Sheet1.Columns.Get(3).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(255)))), ((int)(((byte)(238)))));
            this.ssView_Sheet1.Columns.Get(3).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Label = "성명";
            this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Width = 65F;
            this.ssView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).Label = "나이/성별";
            this.ssView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).Width = 40F;
            this.ssView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Label = "진료과";
            this.ssView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Width = 49F;
            this.ssView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Label = "낙상유형";
            this.ssView_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Width = 65F;
            this.ssView_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Label = "낙상발생장소";
            this.ssView_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Width = 88F;
            this.ssView_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).Label = "IPDNO";
            this.ssView_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).Visible = false;
            this.ssView_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(9).Label = "보고자";
            this.ssView_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType18.Static = true;
            this.ssView_Sheet1.Columns.Get(10).CellType = textCellType18;
            this.ssView_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(10).Label = "구분";
            this.ssView_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(10).Width = 35F;
            textCellType19.Static = true;
            this.ssView_Sheet1.Columns.Get(11).CellType = textCellType19;
            this.ssView_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(11).Label = "등록번호";
            this.ssView_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(11).Width = 62F;
            textCellType20.Static = true;
            this.ssView_Sheet1.Columns.Get(12).CellType = textCellType20;
            this.ssView_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(12).Label = "발생일자";
            this.ssView_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(12).Width = 62F;
            this.ssView_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(13).Label = "부서";
            this.ssView_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(13).Width = 44F;
            this.ssView_Sheet1.Columns.Get(14).Label = "rowid";
            this.ssView_Sheet1.Columns.Get(14).Visible = false;
            this.ssView_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssView_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.Rows.Get(0).Height = 28F;
            this.ssView_Sheet1.Rows.Get(1).Height = 28F;
            this.ssView_Sheet1.Rows.Get(2).Height = 28F;
            this.ssView_Sheet1.Rows.Get(3).Height = 28F;
            this.ssView_Sheet1.Rows.Get(4).Height = 28F;
            this.ssView_Sheet1.Rows.Get(5).Height = 28F;
            this.ssView_Sheet1.Rows.Get(6).Height = 28F;
            this.ssView_Sheet1.Rows.Get(7).Height = 28F;
            this.ssView_Sheet1.Rows.Get(8).Height = 28F;
            this.ssView_Sheet1.Rows.Get(9).Height = 28F;
            this.ssView_Sheet1.Rows.Get(10).Height = 28F;
            this.ssView_Sheet1.Rows.Get(11).Height = 28F;
            this.ssView_Sheet1.Rows.Get(12).Height = 28F;
            this.ssView_Sheet1.Rows.Get(13).Height = 28F;
            this.ssView_Sheet1.Rows.Get(14).Height = 28F;
            this.ssView_Sheet1.Rows.Get(15).Height = 28F;
            this.ssView_Sheet1.Rows.Get(16).Height = 28F;
            this.ssView_Sheet1.Rows.Get(17).Height = 28F;
            this.ssView_Sheet1.Rows.Get(18).Height = 28F;
            this.ssView_Sheet1.Rows.Get(19).Height = 28F;
            this.ssView_Sheet1.Rows.Get(20).Height = 28F;
            this.ssView_Sheet1.Rows.Get(21).Height = 28F;
            this.ssView_Sheet1.Rows.Get(22).Height = 28F;
            this.ssView_Sheet1.Rows.Get(23).Height = 28F;
            this.ssView_Sheet1.Rows.Get(24).Height = 28F;
            this.ssView_Sheet1.Rows.Get(25).Height = 28F;
            this.ssView_Sheet1.Rows.Get(26).Height = 28F;
            this.ssView_Sheet1.Rows.Get(27).Height = 28F;
            this.ssView_Sheet1.Rows.Get(28).Height = 28F;
            this.ssView_Sheet1.Rows.Get(29).Height = 28F;
            this.ssView_Sheet1.Rows.Get(30).Height = 28F;
            this.ssView_Sheet1.Rows.Get(31).Height = 28F;
            this.ssView_Sheet1.Rows.Get(32).Height = 28F;
            this.ssView_Sheet1.Rows.Get(33).Height = 28F;
            this.ssView_Sheet1.Rows.Get(34).Height = 28F;
            this.ssView_Sheet1.Rows.Get(35).Height = 28F;
            this.ssView_Sheet1.Rows.Get(36).Height = 28F;
            this.ssView_Sheet1.Rows.Get(37).Height = 28F;
            this.ssView_Sheet1.Rows.Get(38).Height = 28F;
            this.ssView_Sheet1.Rows.Get(39).Height = 28F;
            this.ssView_Sheet1.Rows.Get(40).Height = 28F;
            this.ssView_Sheet1.Rows.Get(41).Height = 28F;
            this.ssView_Sheet1.Rows.Get(42).Height = 28F;
            this.ssView_Sheet1.Rows.Get(43).Height = 28F;
            this.ssView_Sheet1.Rows.Get(44).Height = 28F;
            this.ssView_Sheet1.Rows.Get(45).Height = 28F;
            this.ssView_Sheet1.Rows.Get(46).Height = 28F;
            this.ssView_Sheet1.Rows.Get(47).Height = 28F;
            this.ssView_Sheet1.Rows.Get(48).Height = 28F;
            this.ssView_Sheet1.Rows.Get(49).Height = 28F;
            this.ssView_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssView_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblItem0);
            this.panel1.Controls.Add(this.cboWard);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(143, 37);
            this.panel1.TabIndex = 48;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rdoGubun1);
            this.panel2.Controls.Add(this.rdoGubun0);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.dtpDate1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lblItem1);
            this.panel2.Controls.Add(this.dtpDate);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(143, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(598, 37);
            this.panel2.TabIndex = 49;
            // 
            // rdoGubun1
            // 
            this.rdoGubun1.AutoSize = true;
            this.rdoGubun1.Checked = true;
            this.rdoGubun1.Location = new System.Drawing.Point(390, 8);
            this.rdoGubun1.Name = "rdoGubun1";
            this.rdoGubun1.Size = new System.Drawing.Size(65, 21);
            this.rdoGubun1.TabIndex = 49;
            this.rdoGubun1.TabStop = true;
            this.rdoGubun1.Text = "작성일";
            this.rdoGubun1.UseVisualStyleBackColor = true;
            // 
            // rdoGubun0
            // 
            this.rdoGubun0.AutoSize = true;
            this.rdoGubun0.Location = new System.Drawing.Point(455, 8);
            this.rdoGubun0.Name = "rdoGubun0";
            this.rdoGubun0.Size = new System.Drawing.Size(65, 21);
            this.rdoGubun0.TabIndex = 48;
            this.rdoGubun0.Text = "발생일";
            this.rdoGubun0.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.label2.Location = new System.Drawing.Point(306, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 17);
            this.label2.TabIndex = 47;
            this.label2.Text = "조회일 기준";
            // 
            // frmFallOccurView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 711);
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.panTitleSub1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmFallOccurView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "낙상발생리스트";
            this.Load += new System.EventHandler(this.frmFallOccurView_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panTitleSub1.ResumeLayout(false);
            this.panTitleSub1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblItem1;
        private System.Windows.Forms.Label lblItem0;
        private System.Windows.Forms.Panel panTitleSub1;
        private System.Windows.Forms.Label lblTitleSub1;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.ComboBox cboWard;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.DateTimePicker dtpDate1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rdoGubun1;
        private System.Windows.Forms.RadioButton rdoGubun0;
        private System.Windows.Forms.Label label2;
    }
}