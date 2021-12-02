namespace ComPmpaLibB
{
    partial class frmPmpaJengSanPaperIssue
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("BorderEx349636704818188484664", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Static453636704818188734679", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static983636704818188794682");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dtpEDate = new System.Windows.Forms.DateTimePicker();
            this.dtpSDate = new System.Windows.Forms.DateTimePicker();
            this.lblSName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPano = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdoBun1 = new System.Windows.Forms.RadioButton();
            this.rdoBun0 = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoIO2 = new System.Windows.Forms.RadioButton();
            this.rdoIO1 = new System.Windows.Forms.RadioButton();
            this.rdoIO0 = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoGb2 = new System.Windows.Forms.RadioButton();
            this.rdoGb1 = new System.Windows.Forms.RadioButton();
            this.rdoGb0 = new System.Windows.Forms.RadioButton();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
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
            this.panTitle.Size = new System.Drawing.Size(715, 34);
            this.panTitle.TabIndex = 20;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(150, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "기타서류 발급 내역";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(715, 34);
            this.panel1.TabIndex = 21;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(623, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(90, 30);
            this.btnExit.TabIndex = 0;
            this.btnExit.Text = "닫  기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(534, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(90, 30);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "조  회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 68);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(715, 28);
            this.panTitleSub0.TabIndex = 22;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(55, 15);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "조회구분";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dtpEDate);
            this.panel2.Controls.Add(this.dtpSDate);
            this.panel2.Controls.Add(this.lblSName);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.txtPano);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.panel2.Location = new System.Drawing.Point(0, 96);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(2);
            this.panel2.Size = new System.Drawing.Size(715, 85);
            this.panel2.TabIndex = 23;
            // 
            // dtpEDate
            // 
            this.dtpEDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEDate.Location = new System.Drawing.Point(465, 46);
            this.dtpEDate.Name = "dtpEDate";
            this.dtpEDate.Size = new System.Drawing.Size(105, 25);
            this.dtpEDate.TabIndex = 9;
            // 
            // dtpSDate
            // 
            this.dtpSDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSDate.Location = new System.Drawing.Point(360, 46);
            this.dtpSDate.Name = "dtpSDate";
            this.dtpSDate.Size = new System.Drawing.Size(105, 25);
            this.dtpSDate.TabIndex = 9;
            // 
            // lblSName
            // 
            this.lblSName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSName.Location = new System.Drawing.Point(441, 15);
            this.lblSName.Name = "lblSName";
            this.lblSName.Size = new System.Drawing.Size(100, 25);
            this.lblSName.TabIndex = 8;
            this.lblSName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(295, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 19);
            this.label3.TabIndex = 5;
            this.label3.Text = "조회기간";
            // 
            // txtPano
            // 
            this.txtPano.Location = new System.Drawing.Point(360, 15);
            this.txtPano.Name = "txtPano";
            this.txtPano.Size = new System.Drawing.Size(80, 25);
            this.txtPano.TabIndex = 7;
            this.txtPano.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPano.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPano_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(295, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "환자정보";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdoBun1);
            this.groupBox3.Controls.Add(this.rdoBun0);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.groupBox3.Location = new System.Drawing.Point(197, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(85, 81);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "구분";
            // 
            // rdoBun1
            // 
            this.rdoBun1.AutoSize = true;
            this.rdoBun1.Location = new System.Drawing.Point(10, 47);
            this.rdoBun1.Name = "rdoBun1";
            this.rdoBun1.Size = new System.Drawing.Size(73, 19);
            this.rdoBun1.TabIndex = 0;
            this.rdoBun1.Text = "수신자명";
            this.rdoBun1.UseVisualStyleBackColor = true;
            // 
            // rdoBun0
            // 
            this.rdoBun0.AutoSize = true;
            this.rdoBun0.Checked = true;
            this.rdoBun0.Location = new System.Drawing.Point(10, 22);
            this.rdoBun0.Name = "rdoBun0";
            this.rdoBun0.Size = new System.Drawing.Size(73, 19);
            this.rdoBun0.TabIndex = 0;
            this.rdoBun0.TabStop = true;
            this.rdoBun0.Text = "등록번호";
            this.rdoBun0.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(192, 2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(5, 81);
            this.panel4.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdoIO2);
            this.groupBox2.Controls.Add(this.rdoIO1);
            this.groupBox2.Controls.Add(this.rdoIO0);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.groupBox2.Location = new System.Drawing.Point(117, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(75, 81);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "퇴원/외래";
            // 
            // rdoIO2
            // 
            this.rdoIO2.AutoSize = true;
            this.rdoIO2.Location = new System.Drawing.Point(10, 57);
            this.rdoIO2.Name = "rdoIO2";
            this.rdoIO2.Size = new System.Drawing.Size(49, 19);
            this.rdoIO2.TabIndex = 0;
            this.rdoIO2.Text = "외래";
            this.rdoIO2.UseVisualStyleBackColor = true;
            // 
            // rdoIO1
            // 
            this.rdoIO1.AutoSize = true;
            this.rdoIO1.Location = new System.Drawing.Point(10, 37);
            this.rdoIO1.Name = "rdoIO1";
            this.rdoIO1.Size = new System.Drawing.Size(49, 19);
            this.rdoIO1.TabIndex = 0;
            this.rdoIO1.Text = "입원";
            this.rdoIO1.UseVisualStyleBackColor = true;
            // 
            // rdoIO0
            // 
            this.rdoIO0.AutoSize = true;
            this.rdoIO0.Checked = true;
            this.rdoIO0.Location = new System.Drawing.Point(10, 17);
            this.rdoIO0.Name = "rdoIO0";
            this.rdoIO0.Size = new System.Drawing.Size(49, 19);
            this.rdoIO0.TabIndex = 0;
            this.rdoIO0.TabStop = true;
            this.rdoIO0.Text = "전체";
            this.rdoIO0.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(112, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(5, 81);
            this.panel3.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoGb2);
            this.groupBox1.Controls.Add(this.rdoGb1);
            this.groupBox1.Controls.Add(this.rdoGb0);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.groupBox1.Location = new System.Drawing.Point(2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(110, 81);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "발급구분";
            // 
            // rdoGb2
            // 
            this.rdoGb2.AutoSize = true;
            this.rdoGb2.Location = new System.Drawing.Point(10, 57);
            this.rdoGb2.Name = "rdoGb2";
            this.rdoGb2.Size = new System.Drawing.Size(85, 19);
            this.rdoGb2.TabIndex = 0;
            this.rdoGb2.Text = "수납내역서";
            this.rdoGb2.UseVisualStyleBackColor = true;
            // 
            // rdoGb1
            // 
            this.rdoGb1.AutoSize = true;
            this.rdoGb1.Location = new System.Drawing.Point(10, 37);
            this.rdoGb1.Name = "rdoGb1";
            this.rdoGb1.Size = new System.Drawing.Size(97, 19);
            this.rdoGb1.TabIndex = 0;
            this.rdoGb1.Text = "진료상세내역";
            this.rdoGb1.UseVisualStyleBackColor = true;
            // 
            // rdoGb0
            // 
            this.rdoGb0.AutoSize = true;
            this.rdoGb0.Checked = true;
            this.rdoGb0.Location = new System.Drawing.Point(10, 17);
            this.rdoGb0.Name = "rdoGb0";
            this.rdoGb0.Size = new System.Drawing.Size(49, 19);
            this.rdoGb0.TabIndex = 0;
            this.rdoGb0.TabStop = true;
            this.rdoGb0.Text = "전체";
            this.rdoGb0.UseVisualStyleBackColor = true;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssView.Location = new System.Drawing.Point(0, 181);
            this.ssView.Name = "ssView";
            namedStyle1.Border = complexBorder1;
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle2.Border = complexBorder2;
            textCellType1.Static = true;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType2.Static = true;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3});
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(715, 477);
            this.ssView.TabIndex = 24;
            this.ssView.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssView.TextTipAppearance = tipAppearance1;
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 8;
            this.ssView_Sheet1.RowCount = 1;
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
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "발행구분";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "발행일자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "작업자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "발행용도";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "환자명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "신청인";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "신청인연락처";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "관계";
            this.ssView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Get(0).Label = "발행구분";
            this.ssView_Sheet1.Columns.Get(0).Width = 94F;
            this.ssView_Sheet1.Columns.Get(1).Label = "발행일자";
            this.ssView_Sheet1.Columns.Get(1).Width = 86F;
            this.ssView_Sheet1.Columns.Get(2).Label = "작업자";
            this.ssView_Sheet1.Columns.Get(2).Width = 53F;
            this.ssView_Sheet1.Columns.Get(3).Label = "발행용도";
            this.ssView_Sheet1.Columns.Get(3).Width = 136F;
            this.ssView_Sheet1.Columns.Get(4).Label = "환자명";
            this.ssView_Sheet1.Columns.Get(4).Width = 53F;
            this.ssView_Sheet1.Columns.Get(5).Label = "신청인";
            this.ssView_Sheet1.Columns.Get(5).Width = 53F;
            this.ssView_Sheet1.Columns.Get(6).Label = "신청인연락처";
            this.ssView_Sheet1.Columns.Get(6).Width = 110F;
            this.ssView_Sheet1.Columns.Get(7).Label = "관계";
            this.ssView_Sheet1.Columns.Get(7).Width = 63F;
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
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.RowHeader.Columns.Get(0).Width = 42F;
            this.ssView_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssView_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmPmpaJengSanPaperIssue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(715, 658);
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmPmpaJengSanPaperIssue";
            this.Text = "frmPmpaJengSanPaperIssue";
            this.Load += new System.EventHandler(this.frmPmpaJengSanPaperIssue_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdoBun1;
        private System.Windows.Forms.RadioButton rdoBun0;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoIO2;
        private System.Windows.Forms.RadioButton rdoIO1;
        private System.Windows.Forms.RadioButton rdoIO0;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoGb2;
        private System.Windows.Forms.RadioButton rdoGb1;
        private System.Windows.Forms.RadioButton rdoGb0;
        private System.Windows.Forms.DateTimePicker dtpEDate;
        private System.Windows.Forms.DateTimePicker dtpSDate;
        private System.Windows.Forms.Label lblSName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPano;
        private System.Windows.Forms.Label label1;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
    }
}