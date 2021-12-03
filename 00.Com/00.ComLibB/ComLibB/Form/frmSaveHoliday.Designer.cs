namespace ComLibB
{
    partial class frmSaveHoliday
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("BorderEx360636307998787939469", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text466636307998788095739", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Color567636307998788095739");
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static603636307998788095739");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Static697636307998788095739");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Color863636307998788095739");
            FarPoint.Win.Spread.NamedStyle namedStyle7 = new FarPoint.Win.Spread.NamedStyle("Static899636307998788095739");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle8 = new FarPoint.Win.Spread.NamedStyle("Color1647636307998788252197");
            FarPoint.Win.Spread.NamedStyle namedStyle9 = new FarPoint.Win.Spread.NamedStyle("Color1691636307998788252197");
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(591, 34);
            this.panTitle.TabIndex = 84;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(514, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(9, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(99, 16);
            this.lblTitle.TabIndex = 15;
            this.lblTitle.Text = "공휴일 등록";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.dtpDate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(591, 28);
            this.panel1.TabIndex = 85;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(514, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 22);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(442, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 22);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(17, 3);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(96, 21);
            this.dtpDate.TabIndex = 0;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssView.Location = new System.Drawing.Point(0, 62);
            this.ssView.Name = "ssView";
            namedStyle1.Border = complexBorder1;
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle2.Border = complexBorder2;
            textCellType1.MaxLength = 60;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            namedStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            textCellType2.Static = true;
            namedStyle4.CellType = textCellType2;
            namedStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType2;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.Static = true;
            namedStyle5.CellType = textCellType3;
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Renderer = textCellType3;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            namedStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle6.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle6.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            textCellType4.Static = true;
            namedStyle7.CellType = textCellType4;
            namedStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle7.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle7.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle7.Renderer = textCellType4;
            namedStyle7.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            namedStyle8.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle8.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle8.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            namedStyle9.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle9.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle9.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5,
            namedStyle6,
            namedStyle7,
            namedStyle8,
            namedStyle9});
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(591, 349);
            this.ssView.TabIndex = 86;
            this.ssView.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssView.TextTipAppearance = tipAppearance1;
            this.ssView.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView_CellDoubleClick);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 17;
            this.ssView_Sheet1.RowCount = 16;
            this.ssView_Sheet1.Cells.Get(0, 0).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(0, 1).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(0, 7).StyleName = "Color1691636307998788252197";
            this.ssView_Sheet1.Cells.Get(0, 8).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(0, 9).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(1, 0).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(1, 1).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(1, 7).StyleName = "Color1691636307998788252197";
            this.ssView_Sheet1.Cells.Get(1, 8).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(1, 9).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(2, 0).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(2, 1).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(2, 7).StyleName = "Color1691636307998788252197";
            this.ssView_Sheet1.Cells.Get(2, 8).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(2, 9).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(3, 0).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(3, 1).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(3, 7).StyleName = "Color1691636307998788252197";
            this.ssView_Sheet1.Cells.Get(3, 8).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(3, 9).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(4, 0).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(4, 1).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(4, 7).StyleName = "Color1691636307998788252197";
            this.ssView_Sheet1.Cells.Get(4, 8).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(4, 9).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(5, 0).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(5, 1).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(5, 7).StyleName = "Color1691636307998788252197";
            this.ssView_Sheet1.Cells.Get(5, 8).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(5, 9).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(6, 0).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(6, 1).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(6, 7).StyleName = "Color1691636307998788252197";
            this.ssView_Sheet1.Cells.Get(6, 8).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(6, 9).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(7, 0).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(7, 1).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(7, 7).StyleName = "Color1691636307998788252197";
            this.ssView_Sheet1.Cells.Get(7, 8).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(7, 9).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(8, 0).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(8, 1).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(8, 7).StyleName = "Color1691636307998788252197";
            this.ssView_Sheet1.Cells.Get(8, 8).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(8, 9).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(9, 0).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(9, 1).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(9, 7).StyleName = "Color1691636307998788252197";
            this.ssView_Sheet1.Cells.Get(9, 8).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(9, 9).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(10, 0).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(10, 1).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(10, 7).StyleName = "Color1691636307998788252197";
            this.ssView_Sheet1.Cells.Get(10, 8).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(10, 9).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(11, 0).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(11, 1).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(11, 7).StyleName = "Color1691636307998788252197";
            this.ssView_Sheet1.Cells.Get(11, 8).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(11, 9).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(12, 0).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(12, 1).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(12, 7).StyleName = "Color1691636307998788252197";
            this.ssView_Sheet1.Cells.Get(12, 8).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(12, 9).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(13, 0).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(13, 1).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(13, 7).StyleName = "Color1691636307998788252197";
            this.ssView_Sheet1.Cells.Get(13, 8).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(13, 9).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(14, 0).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(14, 1).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(14, 7).StyleName = "Color1691636307998788252197";
            this.ssView_Sheet1.Cells.Get(14, 8).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(14, 9).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(15, 0).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(15, 1).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(15, 7).StyleName = "Color1691636307998788252197";
            this.ssView_Sheet1.Cells.Get(15, 8).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.Cells.Get(15, 9).StyleName = "Color1647636307998788252197";
            this.ssView_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "일자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "요일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "휴일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "외래";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "입원";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "ARC";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "임시";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = " ";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "일자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "요일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "휴일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "외래";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "입원";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "ARC";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "임시";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "ROWID1";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "ROWID2";
            this.ssView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 18F;
            this.ssView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Get(0).Label = "일자";
            this.ssView_Sheet1.Columns.Get(0).StyleName = "Static603636307998788095739";
            this.ssView_Sheet1.Columns.Get(0).Width = 40F;
            this.ssView_Sheet1.Columns.Get(1).Label = "요일";
            this.ssView_Sheet1.Columns.Get(1).StyleName = "Static603636307998788095739";
            this.ssView_Sheet1.Columns.Get(1).Width = 40F;
            this.ssView_Sheet1.Columns.Get(2).Label = "휴일";
            this.ssView_Sheet1.Columns.Get(2).StyleName = "Static697636307998788095739";
            this.ssView_Sheet1.Columns.Get(2).Width = 40F;
            this.ssView_Sheet1.Columns.Get(3).Label = "외래";
            this.ssView_Sheet1.Columns.Get(3).StyleName = "Static697636307998788095739";
            this.ssView_Sheet1.Columns.Get(3).Width = 40F;
            this.ssView_Sheet1.Columns.Get(4).Label = "입원";
            this.ssView_Sheet1.Columns.Get(4).StyleName = "Static697636307998788095739";
            this.ssView_Sheet1.Columns.Get(4).Width = 40F;
            this.ssView_Sheet1.Columns.Get(5).Label = "ARC";
            this.ssView_Sheet1.Columns.Get(5).StyleName = "Static697636307998788095739";
            this.ssView_Sheet1.Columns.Get(5).Width = 40F;
            this.ssView_Sheet1.Columns.Get(6).Label = "임시";
            this.ssView_Sheet1.Columns.Get(6).StyleName = "Static697636307998788095739";
            this.ssView_Sheet1.Columns.Get(6).Width = 40F;
            this.ssView_Sheet1.Columns.Get(7).Label = " ";
            this.ssView_Sheet1.Columns.Get(7).StyleName = "Static899636307998788095739";
            this.ssView_Sheet1.Columns.Get(7).Width = 4F;
            this.ssView_Sheet1.Columns.Get(8).Label = "일자";
            this.ssView_Sheet1.Columns.Get(8).StyleName = "Static603636307998788095739";
            this.ssView_Sheet1.Columns.Get(8).Width = 39F;
            this.ssView_Sheet1.Columns.Get(9).Label = "요일";
            this.ssView_Sheet1.Columns.Get(9).StyleName = "Static603636307998788095739";
            this.ssView_Sheet1.Columns.Get(9).Width = 39F;
            this.ssView_Sheet1.Columns.Get(10).Label = "휴일";
            this.ssView_Sheet1.Columns.Get(10).StyleName = "Static697636307998788095739";
            this.ssView_Sheet1.Columns.Get(10).Width = 39F;
            this.ssView_Sheet1.Columns.Get(11).Label = "외래";
            this.ssView_Sheet1.Columns.Get(11).StyleName = "Static697636307998788095739";
            this.ssView_Sheet1.Columns.Get(11).Width = 39F;
            this.ssView_Sheet1.Columns.Get(12).Label = "입원";
            this.ssView_Sheet1.Columns.Get(12).StyleName = "Static697636307998788095739";
            this.ssView_Sheet1.Columns.Get(12).Width = 39F;
            this.ssView_Sheet1.Columns.Get(13).Label = "ARC";
            this.ssView_Sheet1.Columns.Get(13).StyleName = "Static697636307998788095739";
            this.ssView_Sheet1.Columns.Get(13).Width = 39F;
            this.ssView_Sheet1.Columns.Get(14).Label = "임시";
            this.ssView_Sheet1.Columns.Get(14).StyleName = "Static697636307998788095739";
            this.ssView_Sheet1.Columns.Get(14).Width = 39F;
            this.ssView_Sheet1.Columns.Get(15).Label = "ROWID1";
            this.ssView_Sheet1.Columns.Get(15).StyleName = "Static697636307998788095739";
            this.ssView_Sheet1.Columns.Get(15).Width = 70F;
            this.ssView_Sheet1.Columns.Get(16).Label = "ROWID2";
            this.ssView_Sheet1.Columns.Get(16).StyleName = "Static697636307998788095739";
            this.ssView_Sheet1.Columns.Get(16).Width = 70F;
            this.ssView_Sheet1.DefaultStyleName = "Text466636307998788095739";
            this.ssView_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.None);
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.RowHeader.Visible = false;
            this.ssView_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.None);
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmSaveHoliday
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 411);
            this.ControlBox = false;
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmSaveHoliday";
            this.Text = "공휴일 등록";
            this.Load += new System.EventHandler(this.frmSaveHoliday_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
    }
}