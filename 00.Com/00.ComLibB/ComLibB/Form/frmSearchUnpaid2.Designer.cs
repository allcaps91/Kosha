namespace ComLibB
{
    partial class frmSearchUnpaid2
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color354636312374345296891", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text436636312374345453085", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Color526636312374345453085");
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static562636312374345453085");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Static598636312374345453085");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Static652636312374345453085");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType1 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType2 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType3 = new FarPoint.Win.Spread.CellType.NumberCellType();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSelfInsert = new System.Windows.Forms.Button();
            this.optAll = new System.Windows.Forms.RadioButton();
            this.btnSearch = new System.Windows.Forms.Button();
            this.optEtc = new System.Windows.Forms.RadioButton();
            this.optGumsa = new System.Windows.Forms.RadioButton();
            this.optJusa = new System.Windows.Forms.RadioButton();
            this.optYak = new System.Windows.Forms.RadioButton();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssView.Location = new System.Drawing.Point(0, 70);
            this.ssView.Name = "ssView";
            namedStyle1.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 32000;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            namedStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            textCellType2.Static = true;
            namedStyle4.CellType = textCellType2;
            namedStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType2;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.Static = true;
            namedStyle5.CellType = textCellType3;
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Renderer = textCellType3;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType4.Static = true;
            namedStyle6.CellType = textCellType4;
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
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
            this.ssView.Size = new System.Drawing.Size(808, 496);
            this.ssView.TabIndex = 88;
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
            this.ssView_Sheet1.ColumnCount = 9;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "비급여  고지";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "수가코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "수가명칭";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "rowid";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "보험수가";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "안내할금액";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "일반수가";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "표준코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "E항";
            this.ssView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 31F;
            this.ssView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Get(0).Label = "비급여  고지";
            this.ssView_Sheet1.Columns.Get(0).StyleName = "Static562636312374345453085";
            this.ssView_Sheet1.Columns.Get(1).Label = "수가코드";
            this.ssView_Sheet1.Columns.Get(1).StyleName = "Static598636312374345453085";
            this.ssView_Sheet1.Columns.Get(1).Width = 90F;
            this.ssView_Sheet1.Columns.Get(2).Label = "수가명칭";
            this.ssView_Sheet1.Columns.Get(2).StyleName = "Static598636312374345453085";
            this.ssView_Sheet1.Columns.Get(2).Width = 240F;
            this.ssView_Sheet1.Columns.Get(3).Label = "rowid";
            this.ssView_Sheet1.Columns.Get(3).StyleName = "Static652636312374345453085";
            this.ssView_Sheet1.Columns.Get(3).Visible = false;
            this.ssView_Sheet1.Columns.Get(3).Width = 80F;
            numberCellType1.DecimalPlaces = 0;
            numberCellType1.MaximumValue = 99999999D;
            numberCellType1.MinimumValue = -99999999D;
            numberCellType1.Separator = ",";
            numberCellType1.ShowSeparator = true;
            this.ssView_Sheet1.Columns.Get(4).CellType = numberCellType1;
            this.ssView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).Label = "보험수가";
            this.ssView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).Width = 80F;
            numberCellType2.DecimalPlaces = 0;
            numberCellType2.MaximumValue = 99999999D;
            numberCellType2.MinimumValue = -99999999D;
            numberCellType2.Separator = ",";
            numberCellType2.ShowSeparator = true;
            this.ssView_Sheet1.Columns.Get(5).CellType = numberCellType2;
            this.ssView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Label = "안내할금액";
            this.ssView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Width = 89F;
            numberCellType3.DecimalPlaces = 0;
            numberCellType3.MaximumValue = 99999999D;
            numberCellType3.MinimumValue = -99999999D;
            numberCellType3.Separator = ",";
            numberCellType3.ShowSeparator = true;
            this.ssView_Sheet1.Columns.Get(6).CellType = numberCellType3;
            this.ssView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Label = "일반수가";
            this.ssView_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Width = 80F;
            this.ssView_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Label = "표준코드";
            this.ssView_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Width = 78F;
            this.ssView_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).Label = "E항";
            this.ssView_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).Width = 27F;
            this.ssView_Sheet1.DefaultStyleName = "Text436636312374345453085";
            this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.Rows.Default.Height = 18F;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnSelfInsert);
            this.panel1.Controls.Add(this.optAll);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.optEtc);
            this.panel1.Controls.Add(this.optGumsa);
            this.panel1.Controls.Add(this.optJusa);
            this.panel1.Controls.Add(this.optYak);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(808, 36);
            this.panel1.TabIndex = 89;
            // 
            // btnSelfInsert
            // 
            this.btnSelfInsert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelfInsert.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSelfInsert.ForeColor = System.Drawing.Color.Black;
            this.btnSelfInsert.Location = new System.Drawing.Point(513, 3);
            this.btnSelfInsert.Name = "btnSelfInsert";
            this.btnSelfInsert.Size = new System.Drawing.Size(94, 26);
            this.btnSelfInsert.TabIndex = 20;
            this.btnSelfInsert.Text = "수가 수동등록";
            this.btnSelfInsert.UseVisualStyleBackColor = true;
            this.btnSelfInsert.Click += new System.EventHandler(this.btnSelfInsert_Click);
            // 
            // optAll
            // 
            this.optAll.AutoSize = true;
            this.optAll.Checked = true;
            this.optAll.Location = new System.Drawing.Point(12, 8);
            this.optAll.Name = "optAll";
            this.optAll.Size = new System.Drawing.Size(49, 19);
            this.optAll.TabIndex = 19;
            this.optAll.TabStop = true;
            this.optAll.Text = "전체";
            this.optAll.UseVisualStyleBackColor = true;
            this.optAll.CheckedChanged += new System.EventHandler(this.opt_CheckedChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(729, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 18;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // optEtc
            // 
            this.optEtc.AutoSize = true;
            this.optEtc.Location = new System.Drawing.Point(252, 8);
            this.optEtc.Name = "optEtc";
            this.optEtc.Size = new System.Drawing.Size(49, 19);
            this.optEtc.TabIndex = 3;
            this.optEtc.TabStop = true;
            this.optEtc.Text = "기타";
            this.optEtc.UseVisualStyleBackColor = true;
            this.optEtc.CheckedChanged += new System.EventHandler(this.opt_CheckedChanged);
            // 
            // optGumsa
            // 
            this.optGumsa.AutoSize = true;
            this.optGumsa.Location = new System.Drawing.Point(189, 8);
            this.optGumsa.Name = "optGumsa";
            this.optGumsa.Size = new System.Drawing.Size(49, 19);
            this.optGumsa.TabIndex = 2;
            this.optGumsa.TabStop = true;
            this.optGumsa.Text = "검사";
            this.optGumsa.UseVisualStyleBackColor = true;
            this.optGumsa.CheckedChanged += new System.EventHandler(this.opt_CheckedChanged);
            // 
            // optJusa
            // 
            this.optJusa.AutoSize = true;
            this.optJusa.Location = new System.Drawing.Point(126, 8);
            this.optJusa.Name = "optJusa";
            this.optJusa.Size = new System.Drawing.Size(49, 19);
            this.optJusa.TabIndex = 1;
            this.optJusa.TabStop = true;
            this.optJusa.Text = "주사";
            this.optJusa.UseVisualStyleBackColor = true;
            this.optJusa.CheckedChanged += new System.EventHandler(this.opt_CheckedChanged);
            // 
            // optYak
            // 
            this.optYak.AutoSize = true;
            this.optYak.Location = new System.Drawing.Point(75, 8);
            this.optYak.Name = "optYak";
            this.optYak.Size = new System.Drawing.Size(37, 19);
            this.optYak.TabIndex = 0;
            this.optYak.TabStop = true;
            this.optYak.Text = "약";
            this.optYak.UseVisualStyleBackColor = true;
            this.optYak.CheckedChanged += new System.EventHandler(this.opt_CheckedChanged);
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(808, 34);
            this.panTitle.TabIndex = 87;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(729, 1);
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
            this.lblTitle.Size = new System.Drawing.Size(167, 16);
            this.lblTitle.TabIndex = 15;
            this.lblTitle.Text = "비급여고지항목 조정";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmSearchUnpaid2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(808, 566);
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmSearchUnpaid2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "비급여고지항목 조정";
            this.Load += new System.EventHandler(this.frmSearchUnpaid_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton optAll;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.RadioButton optEtc;
        private System.Windows.Forms.RadioButton optGumsa;
        private System.Windows.Forms.RadioButton optJusa;
        private System.Windows.Forms.RadioButton optYak;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnSelfInsert;
    }
}