namespace ComPmpaLibB
{
    partial class frmPmpaViewPanoMir
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color341636421267080204439", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text419636421267080204439", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static523636421267080204439");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static667636421267080204439");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Color977636421267080360667");
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Static1013636421267080360667");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rdoInsu3 = new System.Windows.Forms.RadioButton();
            this.rdoInsu0 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.rdoInsu2 = new System.Windows.Forms.RadioButton();
            this.rdoInsu1 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdoIO0 = new System.Windows.Forms.RadioButton();
            this.rdoIO1 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPano = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblSName = new System.Windows.Forms.Label();
            this.lblItem0 = new System.Windows.Forms.Label();
            this.panTitleSub1 = new System.Windows.Forms.Panel();
            this.lblTitleSub1 = new System.Windows.Forms.Label();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
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
            this.panTitle.Size = new System.Drawing.Size(959, 34);
            this.panTitle.TabIndex = 13;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(198, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "등록번호별 청구내역 조회";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(880, 0);
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
            this.panTitleSub0.Size = new System.Drawing.Size(959, 28);
            this.panTitleSub0.TabIndex = 14;
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
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.txtPano);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.lblSName);
            this.panel3.Controls.Add(this.lblItem0);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 62);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(959, 36);
            this.panel3.TabIndex = 19;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.rdoInsu3);
            this.panel4.Controls.Add(this.rdoInsu0);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.rdoInsu2);
            this.panel4.Controls.Add(this.rdoInsu1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(178, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(361, 36);
            this.panel4.TabIndex = 33;
            // 
            // rdoInsu3
            // 
            this.rdoInsu3.AutoSize = true;
            this.rdoInsu3.Location = new System.Drawing.Point(305, 8);
            this.rdoInsu3.Name = "rdoInsu3";
            this.rdoInsu3.Size = new System.Drawing.Size(47, 16);
            this.rdoInsu3.TabIndex = 42;
            this.rdoInsu3.Text = "전체";
            this.rdoInsu3.UseVisualStyleBackColor = true;
            // 
            // rdoInsu0
            // 
            this.rdoInsu0.AutoSize = true;
            this.rdoInsu0.Checked = true;
            this.rdoInsu0.Location = new System.Drawing.Point(63, 8);
            this.rdoInsu0.Name = "rdoInsu0";
            this.rdoInsu0.Size = new System.Drawing.Size(127, 16);
            this.rdoInsu0.TabIndex = 40;
            this.rdoInsu0.TabStop = true;
            this.rdoInsu0.Text = "건강보험, 의료급여";
            this.rdoInsu0.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 24;
            this.label2.Text = "보험종류";
            // 
            // rdoInsu2
            // 
            this.rdoInsu2.AutoSize = true;
            this.rdoInsu2.Location = new System.Drawing.Point(253, 8);
            this.rdoInsu2.Name = "rdoInsu2";
            this.rdoInsu2.Size = new System.Drawing.Size(47, 16);
            this.rdoInsu2.TabIndex = 42;
            this.rdoInsu2.Text = "자보";
            this.rdoInsu2.UseVisualStyleBackColor = true;
            // 
            // rdoInsu1
            // 
            this.rdoInsu1.AutoSize = true;
            this.rdoInsu1.Location = new System.Drawing.Point(201, 8);
            this.rdoInsu1.Name = "rdoInsu1";
            this.rdoInsu1.Size = new System.Drawing.Size(47, 16);
            this.rdoInsu1.TabIndex = 41;
            this.rdoInsu1.Text = "산재";
            this.rdoInsu1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.rdoIO0);
            this.panel2.Controls.Add(this.rdoIO1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(178, 36);
            this.panel2.TabIndex = 32;
            // 
            // rdoIO0
            // 
            this.rdoIO0.AutoSize = true;
            this.rdoIO0.Location = new System.Drawing.Point(70, 8);
            this.rdoIO0.Name = "rdoIO0";
            this.rdoIO0.Size = new System.Drawing.Size(47, 16);
            this.rdoIO0.TabIndex = 40;
            this.rdoIO0.Text = "입원";
            this.rdoIO0.UseVisualStyleBackColor = true;
            // 
            // rdoIO1
            // 
            this.rdoIO1.AutoSize = true;
            this.rdoIO1.Checked = true;
            this.rdoIO1.Location = new System.Drawing.Point(122, 8);
            this.rdoIO1.Name = "rdoIO1";
            this.rdoIO1.Size = new System.Drawing.Size(47, 16);
            this.rdoIO1.TabIndex = 41;
            this.rdoIO1.TabStop = true;
            this.rdoIO1.Text = "외래";
            this.rdoIO1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 24;
            this.label1.Text = "외래/입원";
            // 
            // txtPano
            // 
            this.txtPano.Location = new System.Drawing.Point(605, 6);
            this.txtPano.Name = "txtPano";
            this.txtPano.Size = new System.Drawing.Size(100, 21);
            this.txtPano.TabIndex = 31;
            this.txtPano.Enter += new System.EventHandler(this.txtPano_Enter);
            this.txtPano.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPano_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(882, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 27;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblSName
            // 
            this.lblSName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSName.Location = new System.Drawing.Point(705, 6);
            this.lblSName.Name = "lblSName";
            this.lblSName.Size = new System.Drawing.Size(100, 25);
            this.lblSName.TabIndex = 24;
            this.lblSName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblItem0
            // 
            this.lblItem0.AutoSize = true;
            this.lblItem0.Location = new System.Drawing.Point(545, 10);
            this.lblItem0.Name = "lblItem0";
            this.lblItem0.Size = new System.Drawing.Size(53, 12);
            this.lblItem0.TabIndex = 24;
            this.lblItem0.Text = "등록번호";
            // 
            // panTitleSub1
            // 
            this.panTitleSub1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub1.Controls.Add(this.lblTitleSub1);
            this.panTitleSub1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub1.Location = new System.Drawing.Point(0, 98);
            this.panTitleSub1.Name = "panTitleSub1";
            this.panTitleSub1.Size = new System.Drawing.Size(959, 28);
            this.panTitleSub1.TabIndex = 20;
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
            this.ssView.Location = new System.Drawing.Point(0, 126);
            this.ssView.Name = "ssView";
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
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.Static = true;
            namedStyle4.CellType = textCellType3;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType3;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            namedStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            textCellType4.Static = true;
            namedStyle6.CellType = textCellType4;
            namedStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle6.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle6.Renderer = textCellType4;
            namedStyle6.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5,
            namedStyle6});
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(959, 500);
            this.ssView.TabIndex = 48;
            this.ssView.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssView.TextTipAppearance = tipAppearance1;
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
            this.ssView_Sheet1.ColumnCount = 17;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "년월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "보험";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "과";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "진료개시일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "조합부담";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "진료기간";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "보류";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "구분";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "EDI접수일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "EDI접수번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "주별";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "WRTNO";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "청구번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "미수액";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "청구액";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "차액";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "청구(EDI 송신)된 본인부담액";
            this.ssView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Get(0).Label = "년월";
            this.ssView_Sheet1.Columns.Get(0).StyleName = "Static523636421267080204439";
            this.ssView_Sheet1.Columns.Get(0).Width = 49F;
            this.ssView_Sheet1.Columns.Get(1).Label = "보험";
            this.ssView_Sheet1.Columns.Get(1).StyleName = "Static523636421267080204439";
            this.ssView_Sheet1.Columns.Get(1).Width = 37F;
            this.ssView_Sheet1.Columns.Get(2).Label = "과";
            this.ssView_Sheet1.Columns.Get(2).StyleName = "Static523636421267080204439";
            this.ssView_Sheet1.Columns.Get(2).Width = 28F;
            this.ssView_Sheet1.Columns.Get(3).Label = "진료개시일";
            this.ssView_Sheet1.Columns.Get(3).StyleName = "Static523636421267080204439";
            this.ssView_Sheet1.Columns.Get(3).Width = 72F;
            this.ssView_Sheet1.Columns.Get(4).Label = "조합부담";
            this.ssView_Sheet1.Columns.Get(4).StyleName = "Static667636421267080204439";
            this.ssView_Sheet1.Columns.Get(4).Width = 76F;
            this.ssView_Sheet1.Columns.Get(5).Label = "진료기간";
            this.ssView_Sheet1.Columns.Get(5).StyleName = "Static523636421267080204439";
            this.ssView_Sheet1.Columns.Get(5).Width = 100F;
            this.ssView_Sheet1.Columns.Get(6).Label = "보류";
            this.ssView_Sheet1.Columns.Get(6).StyleName = "Static523636421267080204439";
            this.ssView_Sheet1.Columns.Get(6).Width = 34F;
            this.ssView_Sheet1.Columns.Get(7).Label = "구분";
            this.ssView_Sheet1.Columns.Get(7).StyleName = "Static523636421267080204439";
            this.ssView_Sheet1.Columns.Get(7).Width = 35F;
            this.ssView_Sheet1.Columns.Get(8).Label = "EDI접수일";
            this.ssView_Sheet1.Columns.Get(8).StyleName = "Static523636421267080204439";
            this.ssView_Sheet1.Columns.Get(8).Width = 76F;
            this.ssView_Sheet1.Columns.Get(9).Label = "EDI접수번호";
            this.ssView_Sheet1.Columns.Get(9).StyleName = "Static523636421267080204439";
            this.ssView_Sheet1.Columns.Get(9).Width = 80F;
            this.ssView_Sheet1.Columns.Get(10).Label = "주별";
            this.ssView_Sheet1.Columns.Get(10).StyleName = "Static523636421267080204439";
            this.ssView_Sheet1.Columns.Get(10).Width = 35F;
            this.ssView_Sheet1.Columns.Get(11).Label = "WRTNO";
            this.ssView_Sheet1.Columns.Get(11).StyleName = "Static523636421267080204439";
            this.ssView_Sheet1.Columns.Get(11).Width = 61F;
            this.ssView_Sheet1.Columns.Get(12).Label = "청구번호";
            this.ssView_Sheet1.Columns.Get(12).StyleName = "Static523636421267080204439";
            this.ssView_Sheet1.Columns.Get(12).Width = 76F;
            this.ssView_Sheet1.Columns.Get(13).Label = "미수액";
            this.ssView_Sheet1.Columns.Get(13).StyleName = "Static1013636421267080360667";
            this.ssView_Sheet1.Columns.Get(13).Width = 68F;
            this.ssView_Sheet1.Columns.Get(14).Label = "청구액";
            this.ssView_Sheet1.Columns.Get(14).StyleName = "Static1013636421267080360667";
            this.ssView_Sheet1.Columns.Get(14).Width = 68F;
            this.ssView_Sheet1.Columns.Get(15).Label = "차액";
            this.ssView_Sheet1.Columns.Get(15).StyleName = "Static1013636421267080360667";
            this.ssView_Sheet1.Columns.Get(15).Width = 68F;
            this.ssView_Sheet1.Columns.Get(16).Label = "청구(EDI 송신)된 본인부담액";
            this.ssView_Sheet1.Columns.Get(16).StyleName = "Static1013636421267080360667";
            this.ssView_Sheet1.Columns.Get(16).Width = 92F;
            this.ssView_Sheet1.DefaultStyleName = "Text419636421267080204439";
            this.ssView_Sheet1.FrozenColumnCount = 3;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.Rows.Get(0).Height = 22F;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmPmpaViewPanoMir
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 626);
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.panTitleSub1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmPmpaViewPanoMir";
            this.Text = "등록번호별 청구내역 조회";
            this.Load += new System.EventHandler(this.frmPmpaViewPanoMir_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panTitleSub1.ResumeLayout(false);
            this.panTitleSub1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton rdoInsu3;
        private System.Windows.Forms.RadioButton rdoInsu0;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdoInsu2;
        private System.Windows.Forms.RadioButton rdoInsu1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rdoIO0;
        private System.Windows.Forms.RadioButton rdoIO1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPano;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblSName;
        private System.Windows.Forms.Label lblItem0;
        private System.Windows.Forms.Panel panTitleSub1;
        private System.Windows.Forms.Label lblTitleSub1;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
    }
}