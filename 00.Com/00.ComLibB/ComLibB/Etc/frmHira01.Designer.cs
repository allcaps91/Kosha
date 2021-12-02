namespace ComLibB
{
    partial class frmHira01
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color333636626679595283382", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text405636626679595283382", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Color495636626679595283382");
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Color333636626680001724784", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Text405636626680001724784", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ss1 = new FarPoint.Win.Spread.FpSpread();
            this.ss1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ss2 = new FarPoint.Win.Spread.FpSpread();
            this.ss2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rdoGB2 = new System.Windows.Forms.RadioButton();
            this.rdoGB1 = new System.Windows.Forms.RadioButton();
            this.rdoGB0 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCopy = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).BeginInit();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss2_Sheet1)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1088, 34);
            this.panTitle.TabIndex = 13;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(1012, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(161, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "심평원자료-병용금기";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(447, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 17;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Location = new System.Drawing.Point(519, -1);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 17;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ss1);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1088, 525);
            this.panel1.TabIndex = 15;
            // 
            // ss1
            // 
            this.ss1.AccessibleDescription = "ss1, Sheet1, Row 0, Column 0, ";
            this.ss1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ss1.Location = new System.Drawing.Point(0, 30);
            this.ss1.Name = "ss1";
            namedStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 32000;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            namedStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3});
            this.ss1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss1_Sheet1});
            this.ss1.Size = new System.Drawing.Size(826, 465);
            this.ss1.TabIndex = 3;
            this.ss1.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ss1.TextTipAppearance = tipAppearance1;
            this.ss1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ss1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ss1_CellDoubleClick);
            // 
            // ss1_Sheet1
            // 
            this.ss1_Sheet1.Reset();
            this.ss1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss1_Sheet1.ColumnCount = 9;
            this.ss1_Sheet1.RowCount = 30;
            this.ss1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "고지일자";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성분코드";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "수가코드";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "수가명칭";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = " ";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "성분코드";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "수가코드";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "수가명칭";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "부작용";
            this.ss1_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Rows.Get(0).Height = 27F;
            this.ss1_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.Columns.Get(0).Label = "고지일자";
            this.ss1_Sheet1.Columns.Get(0).StyleName = "Color495636626679595283382";
            this.ss1_Sheet1.Columns.Get(0).Width = 62F;
            this.ss1_Sheet1.Columns.Get(1).Label = "성분코드";
            this.ss1_Sheet1.Columns.Get(1).Width = 62F;
            this.ss1_Sheet1.Columns.Get(2).Label = "수가코드";
            this.ss1_Sheet1.Columns.Get(2).Width = 62F;
            this.ss1_Sheet1.Columns.Get(3).Label = "수가명칭";
            this.ss1_Sheet1.Columns.Get(3).Width = 162F;
            this.ss1_Sheet1.Columns.Get(4).Label = " ";
            this.ss1_Sheet1.Columns.Get(4).Width = 4F;
            this.ss1_Sheet1.Columns.Get(5).Label = "성분코드";
            this.ss1_Sheet1.Columns.Get(5).Width = 62F;
            this.ss1_Sheet1.Columns.Get(6).Label = "수가코드";
            this.ss1_Sheet1.Columns.Get(6).Width = 62F;
            this.ss1_Sheet1.Columns.Get(7).Label = "수가명칭";
            this.ss1_Sheet1.Columns.Get(7).Width = 173F;
            this.ss1_Sheet1.Columns.Get(8).Label = "부작용";
            this.ss1_Sheet1.Columns.Get(8).Width = 295F;
            this.ss1_Sheet1.DefaultStyleName = "Text405636626679595283382";
            this.ss1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss1_Sheet1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnDelete);
            this.panel5.Controls.Add(this.btnSend);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(826, 30);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(61, 465);
            this.panel5.TabIndex = 2;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.Font = new System.Drawing.Font("맑은 고딕", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.Location = new System.Drawing.Point(6, 232);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(50, 110);
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "☜";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.Transparent;
            this.btnSend.Font = new System.Drawing.Font("맑은 고딕", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSend.Location = new System.Drawing.Point(6, 90);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(50, 110);
            this.btnSend.TabIndex = 17;
            this.btnSend.Text = "☞";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.ss2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(887, 30);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(201, 465);
            this.panel4.TabIndex = 1;
            // 
            // ss2
            // 
            this.ss2.AccessibleDescription = "";
            this.ss2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss2.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ss2.Location = new System.Drawing.Point(0, 0);
            this.ss2.Name = "ss2";
            namedStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Parent = "DataAreaDefault";
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType2.MaxLength = 32000;
            namedStyle5.CellType = textCellType2;
            namedStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Parent = "DataAreaDefault";
            namedStyle5.Renderer = textCellType2;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss2.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle4,
            namedStyle5});
            this.ss2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss2_Sheet1});
            this.ss2.Size = new System.Drawing.Size(201, 465);
            this.ss2.TabIndex = 4;
            this.ss2.TabStripRatio = 0.6D;
            tipAppearance2.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ss2.TextTipAppearance = tipAppearance2;
            this.ss2.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ss2.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ss2_CellDoubleClick);
            // 
            // ss2_Sheet1
            // 
            this.ss2_Sheet1.Reset();
            this.ss2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss2_Sheet1.ColumnCount = 2;
            this.ss2_Sheet1.RowCount = 30;
            this.ss2_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss2_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "성분코드A";
            this.ss2_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성분코드B";
            this.ss2_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss2_Sheet1.ColumnHeader.Rows.Get(0).Height = 21F;
            this.ss2_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss2_Sheet1.Columns.Get(0).Label = "성분코드A";
            this.ss2_Sheet1.Columns.Get(0).Width = 71F;
            this.ss2_Sheet1.Columns.Get(1).Label = "성분코드B";
            this.ss2_Sheet1.Columns.Get(1).Width = 70F;
            this.ss2_Sheet1.DefaultStyleName = "Text405636626680001724784";
            this.ss2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss2_Sheet1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ss2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label3);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 495);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1088, 30);
            this.panel6.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1088, 30);
            this.label3.TabIndex = 1;
            this.label3.Text = "이자료는 심평원자료와 자동 연동하여 본원 수가로 조회 됩니다.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.btnCopy);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.btnPrint);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1088, 30);
            this.panel2.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.rdoGB2);
            this.panel3.Controls.Add(this.rdoGB1);
            this.panel3.Controls.Add(this.rdoGB0);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(4, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(441, 30);
            this.panel3.TabIndex = 0;
            // 
            // rdoGB2
            // 
            this.rdoGB2.AutoSize = true;
            this.rdoGB2.Location = new System.Drawing.Point(347, 4);
            this.rdoGB2.Name = "rdoGB2";
            this.rdoGB2.Size = new System.Drawing.Size(83, 21);
            this.rdoGB2.TabIndex = 1;
            this.rdoGB2.TabStop = true;
            this.rdoGB2.Text = "삭제 코드";
            this.rdoGB2.UseVisualStyleBackColor = true;
            // 
            // rdoGB1
            // 
            this.rdoGB1.AutoSize = true;
            this.rdoGB1.Location = new System.Drawing.Point(227, 3);
            this.rdoGB1.Name = "rdoGB1";
            this.rdoGB1.Size = new System.Drawing.Size(114, 21);
            this.rdoGB1.TabIndex = 1;
            this.rdoGB1.TabStop = true;
            this.rdoGB1.Text = "삭제 제외 코드";
            this.rdoGB1.UseVisualStyleBackColor = true;
            // 
            // rdoGB0
            // 
            this.rdoGB0.AutoSize = true;
            this.rdoGB0.Location = new System.Drawing.Point(138, 3);
            this.rdoGB0.Name = "rdoGB0";
            this.rdoGB0.Size = new System.Drawing.Size(83, 21);
            this.rdoGB0.TabIndex = 1;
            this.rdoGB0.TabStop = true;
            this.rdoGB0.Text = "전체 코드";
            this.rdoGB0.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "삭제 수가 조회 구분";
            // 
            // btnCopy
            // 
            this.btnCopy.BackColor = System.Drawing.Color.Transparent;
            this.btnCopy.Location = new System.Drawing.Point(591, -1);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(283, 30);
            this.btnCopy.TabIndex = 17;
            this.btnCopy.Text = "외래제한사항( 심사과, 약제과용) 동시에 복사";
            this.btnCopy.UseVisualStyleBackColor = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(914, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "복사 제외 성분";
            // 
            // frmHira01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1088, 559);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHira01";
            this.Text = "frmHira01";
            this.Load += new System.EventHandler(this.frmHira01_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ss2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss2_Sheet1)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rdoGB2;
        private System.Windows.Forms.RadioButton rdoGB1;
        private System.Windows.Forms.RadioButton rdoGB0;
        private System.Windows.Forms.Label label1;
        private FarPoint.Win.Spread.FpSpread ss1;
        private FarPoint.Win.Spread.SheetView ss1_Sheet1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Panel panel4;
        private FarPoint.Win.Spread.FpSpread ss2;
        private FarPoint.Win.Spread.SheetView ss2_Sheet1;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label3;
    }
}