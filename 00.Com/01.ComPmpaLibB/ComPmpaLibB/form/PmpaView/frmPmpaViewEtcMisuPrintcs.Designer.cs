namespace ComPmpaLibB
{
    partial class frmPmpaViewEtcMisuPrintcs
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color343636432453327630453", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text465636432453327786353", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static598636432453327786353");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Currency877636432453327786353");
            FarPoint.Win.Spread.CellType.CurrencyCellType currencyCellType1 = new FarPoint.Win.Spread.CellType.CurrencyCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType1 = new FarPoint.Win.Spread.CellType.NumberCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.lblItem1 = new System.Windows.Forms.Label();
            this.lblItem0 = new System.Windows.Forms.Label();
            this.panTitleSub1 = new System.Windows.Forms.Panel();
            this.lblTitleSub1 = new System.Windows.Forms.Label();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.cboYYMM = new System.Windows.Forms.ComboBox();
            this.cboClass = new System.Windows.Forms.ComboBox();
            this.rdoSort0 = new System.Windows.Forms.RadioButton();
            this.rdoSort1 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panel3.SuspendLayout();
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
            this.panTitle.Size = new System.Drawing.Size(1010, 34);
            this.panTitle.TabIndex = 12;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(198, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "기타미수 종류별 상세내역";
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1010, 28);
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
            this.panel3.Controls.Add(this.rdoSort0);
            this.panel3.Controls.Add(this.rdoSort1);
            this.panel3.Controls.Add(this.cboClass);
            this.panel3.Controls.Add(this.cboYYMM);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.btnPrint);
            this.panel3.Controls.Add(this.lblItem1);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.lblItem0);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 62);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1010, 37);
            this.panel3.TabIndex = 18;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(931, 0);
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
            this.btnSearch.Location = new System.Drawing.Point(861, 3);
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
            this.btnPrint.Location = new System.Drawing.Point(933, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 22;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lblItem1
            // 
            this.lblItem1.AutoSize = true;
            this.lblItem1.Location = new System.Drawing.Point(11, 12);
            this.lblItem1.Name = "lblItem1";
            this.lblItem1.Size = new System.Drawing.Size(29, 12);
            this.lblItem1.TabIndex = 25;
            this.lblItem1.Text = "정렬";
            // 
            // lblItem0
            // 
            this.lblItem0.AutoSize = true;
            this.lblItem0.Location = new System.Drawing.Point(421, 12);
            this.lblItem0.Name = "lblItem0";
            this.lblItem0.Size = new System.Drawing.Size(53, 12);
            this.lblItem0.TabIndex = 24;
            this.lblItem0.Text = "미수종류";
            // 
            // panTitleSub1
            // 
            this.panTitleSub1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub1.Controls.Add(this.lblTitleSub1);
            this.panTitleSub1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub1.Location = new System.Drawing.Point(0, 99);
            this.panTitleSub1.Name = "panTitleSub1";
            this.panTitleSub1.Size = new System.Drawing.Size(1010, 28);
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
            namedStyle1.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 60;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림체", 9F);
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
            currencyCellType1.DecimalPlaces = 0;
            currencyCellType1.MaximumValue = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            currencyCellType1.MinimumValue = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            currencyCellType1.ShowCurrencySymbol = false;
            currencyCellType1.ShowSeparator = true;
            namedStyle4.CellType = currencyCellType1;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = currencyCellType1;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssView.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4});
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(1010, 497);
            this.ssView.TabIndex = 47;
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
            this.ssView_Sheet1.ColumnCount = 11;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.Resizable = true;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "계 약 처";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "순번";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "청구일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "병록번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "성 명 ";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "진료기간";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "구분";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "과목";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "현잔액";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "미수일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "비고";
            this.ssView_Sheet1.ColumnHeader.Columns.Default.Resizable = true;
            this.ssView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Default.Resizable = true;
            this.ssView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Get(0).Label = "계 약 처";
            this.ssView_Sheet1.Columns.Get(0).Width = 192F;
            this.ssView_Sheet1.Columns.Get(1).Label = "순번";
            this.ssView_Sheet1.Columns.Get(1).StyleName = "Static598636432453327786353";
            this.ssView_Sheet1.Columns.Get(1).Width = 35F;
            this.ssView_Sheet1.Columns.Get(2).Label = "청구일";
            this.ssView_Sheet1.Columns.Get(2).StyleName = "Static598636432453327786353";
            this.ssView_Sheet1.Columns.Get(2).Width = 91F;
            this.ssView_Sheet1.Columns.Get(3).Label = "병록번호";
            this.ssView_Sheet1.Columns.Get(3).StyleName = "Static598636432453327786353";
            this.ssView_Sheet1.Columns.Get(3).Width = 78F;
            this.ssView_Sheet1.Columns.Get(4).Label = "성 명 ";
            this.ssView_Sheet1.Columns.Get(4).StyleName = "Static598636432453327786353";
            this.ssView_Sheet1.Columns.Get(4).Width = 78F;
            this.ssView_Sheet1.Columns.Get(5).Label = "진료기간";
            this.ssView_Sheet1.Columns.Get(5).StyleName = "Static598636432453327786353";
            this.ssView_Sheet1.Columns.Get(5).Width = 191F;
            this.ssView_Sheet1.Columns.Get(6).Label = "구분";
            this.ssView_Sheet1.Columns.Get(6).StyleName = "Static598636432453327786353";
            this.ssView_Sheet1.Columns.Get(6).Width = 43F;
            this.ssView_Sheet1.Columns.Get(7).Label = "과목";
            this.ssView_Sheet1.Columns.Get(7).StyleName = "Static598636432453327786353";
            this.ssView_Sheet1.Columns.Get(7).Width = 42F;
            numberCellType1.DecimalPlaces = 0;
            numberCellType1.MaximumValue = 999999999.99D;
            numberCellType1.ShowSeparator = true;
            this.ssView_Sheet1.Columns.Get(8).CellType = numberCellType1;
            this.ssView_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssView_Sheet1.Columns.Get(8).Label = "현잔액";
            this.ssView_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.ssView_Sheet1.Columns.Get(8).Width = 108F;
            this.ssView_Sheet1.Columns.Get(9).Label = "미수일";
            this.ssView_Sheet1.Columns.Get(9).StyleName = "Static598636432453327786353";
            this.ssView_Sheet1.Columns.Get(9).Width = 66F;
            this.ssView_Sheet1.Columns.Get(10).Label = "비고";
            this.ssView_Sheet1.Columns.Get(10).Width = 65F;
            this.ssView_Sheet1.DefaultStyleName = "Text465636432453327786353";
            this.ssView_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.None);
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssView_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.Rows.Default.Resizable = false;
            this.ssView_Sheet1.RowHeader.Visible = false;
            this.ssView_Sheet1.Rows.Default.Height = 23F;
            this.ssView_Sheet1.Rows.Default.Resizable = false;
            this.ssView_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssView_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssView_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192))))));
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // cboYYMM
            // 
            this.cboYYMM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboYYMM.FormattingEnabled = true;
            this.cboYYMM.Location = new System.Drawing.Point(263, 8);
            this.cboYYMM.Name = "cboYYMM";
            this.cboYYMM.Size = new System.Drawing.Size(121, 20);
            this.cboYYMM.TabIndex = 33;
            // 
            // cboClass
            // 
            this.cboClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClass.FormattingEnabled = true;
            this.cboClass.Location = new System.Drawing.Point(474, 8);
            this.cboClass.Name = "cboClass";
            this.cboClass.Size = new System.Drawing.Size(121, 20);
            this.cboClass.TabIndex = 33;
            // 
            // rdoSort0
            // 
            this.rdoSort0.AutoSize = true;
            this.rdoSort0.Location = new System.Drawing.Point(48, 10);
            this.rdoSort0.Name = "rdoSort0";
            this.rdoSort0.Size = new System.Drawing.Size(71, 16);
            this.rdoSort0.TabIndex = 40;
            this.rdoSort0.Text = "등록번호";
            this.rdoSort0.UseVisualStyleBackColor = true;
            // 
            // rdoSort1
            // 
            this.rdoSort1.AutoSize = true;
            this.rdoSort1.Checked = true;
            this.rdoSort1.Location = new System.Drawing.Point(119, 10);
            this.rdoSort1.Name = "rdoSort1";
            this.rdoSort1.Size = new System.Drawing.Size(59, 16);
            this.rdoSort1.TabIndex = 39;
            this.rdoSort1.TabStop = true;
            this.rdoSort1.Text = "청구일";
            this.rdoSort1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(210, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 24;
            this.label1.Text = "작업년월";
            // 
            // frmPmpaViewEtcMisuPrintcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 624);
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.panTitleSub1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmPmpaViewEtcMisuPrintcs";
            this.Text = "기타미수 종류별 상세내역";
            this.Load += new System.EventHandler(this.frmPmpaViewEtcMisuPrintcs_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
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
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblItem1;
        private System.Windows.Forms.Label lblItem0;
        private System.Windows.Forms.Panel panTitleSub1;
        private System.Windows.Forms.Label lblTitleSub1;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.ComboBox cboClass;
        private System.Windows.Forms.ComboBox cboYYMM;
        private System.Windows.Forms.RadioButton rdoSort0;
        private System.Windows.Forms.RadioButton rdoSort1;
        private System.Windows.Forms.Label label1;
    }
}