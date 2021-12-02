namespace ComPmpaLibB
{
    partial class frmPmpaViewWardOutList2
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
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Color354636452321562302179", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Text418636452321562322178", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Static522636452321562472221");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rdoOptgb1 = new System.Windows.Forms.RadioButton();
            this.rdoOptgb2 = new System.Windows.Forms.RadioButton();
            this.rdoOptgb3 = new System.Windows.Forms.RadioButton();
            this.rdoOptgb0 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dtpFdate = new System.Windows.Forms.DateTimePicker();
            this.lblyyyy = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panhedden1 = new System.Windows.Forms.Panel();
            this.cboWard = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.panTitle.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panhedden1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
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
            this.panTitle.Size = new System.Drawing.Size(1234, 34);
            this.panTitle.TabIndex = 28;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(128, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "퇴원예고자 명단";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1158, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel5.Controls.Add(this.label1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 34);
            this.panel5.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1234, 28);
            this.panel5.TabIndex = 51;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "조회 조건";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.rdoOptgb1);
            this.panel3.Controls.Add(this.rdoOptgb2);
            this.panel3.Controls.Add(this.rdoOptgb3);
            this.panel3.Controls.Add(this.rdoOptgb0);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.btnSearch);
            this.panel3.Controls.Add(this.btnPrint);
            this.panel3.Controls.Add(this.panhedden1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 62);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1234, 32);
            this.panel3.TabIndex = 52;
            // 
            // rdoOptgb1
            // 
            this.rdoOptgb1.AutoSize = true;
            this.rdoOptgb1.Checked = true;
            this.rdoOptgb1.Location = new System.Drawing.Point(562, 8);
            this.rdoOptgb1.Name = "rdoOptgb1";
            this.rdoOptgb1.Size = new System.Drawing.Size(47, 16);
            this.rdoOptgb1.TabIndex = 70;
            this.rdoOptgb1.TabStop = true;
            this.rdoOptgb1.Text = "전체";
            this.rdoOptgb1.UseVisualStyleBackColor = true;
            // 
            // rdoOptgb2
            // 
            this.rdoOptgb2.AutoSize = true;
            this.rdoOptgb2.Location = new System.Drawing.Point(483, 8);
            this.rdoOptgb2.Name = "rdoOptgb2";
            this.rdoOptgb2.Size = new System.Drawing.Size(79, 16);
            this.rdoOptgb2.TabIndex = 69;
            this.rdoOptgb2.Text = "18:00 이전";
            this.rdoOptgb2.UseVisualStyleBackColor = true;
            // 
            // rdoOptgb3
            // 
            this.rdoOptgb3.AutoSize = true;
            this.rdoOptgb3.Location = new System.Drawing.Point(404, 8);
            this.rdoOptgb3.Name = "rdoOptgb3";
            this.rdoOptgb3.Size = new System.Drawing.Size(79, 16);
            this.rdoOptgb3.TabIndex = 68;
            this.rdoOptgb3.Text = "17:30 이전";
            this.rdoOptgb3.UseVisualStyleBackColor = true;
            // 
            // rdoOptgb0
            // 
            this.rdoOptgb0.AutoSize = true;
            this.rdoOptgb0.Location = new System.Drawing.Point(325, 8);
            this.rdoOptgb0.Name = "rdoOptgb0";
            this.rdoOptgb0.Size = new System.Drawing.Size(79, 16);
            this.rdoOptgb0.TabIndex = 67;
            this.rdoOptgb0.Text = "17:00 이전";
            this.rdoOptgb0.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.dtpTDate);
            this.panel2.Controls.Add(this.dtpFdate);
            this.panel2.Controls.Add(this.lblyyyy);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(160, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(154, 32);
            this.panel2.TabIndex = 61;
            // 
            // dtpFdate
            // 
            this.dtpFdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFdate.Location = new System.Drawing.Point(64, 6);
            this.dtpFdate.Name = "dtpFdate";
            this.dtpFdate.Size = new System.Drawing.Size(86, 21);
            this.dtpFdate.TabIndex = 57;
            // 
            // lblyyyy
            // 
            this.lblyyyy.AutoSize = true;
            this.lblyyyy.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.lblyyyy.Location = new System.Drawing.Point(4, 8);
            this.lblyyyy.Name = "lblyyyy";
            this.lblyyyy.Size = new System.Drawing.Size(60, 17);
            this.lblyyyy.TabIndex = 56;
            this.lblyyyy.Text = "퇴원년월";
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(1090, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 32);
            this.btnSearch.TabIndex = 27;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.Location = new System.Drawing.Point(1162, 0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 32);
            this.btnPrint.TabIndex = 22;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // panhedden1
            // 
            this.panhedden1.Controls.Add(this.cboWard);
            this.panhedden1.Controls.Add(this.label3);
            this.panhedden1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panhedden1.Location = new System.Drawing.Point(0, 0);
            this.panhedden1.Name = "panhedden1";
            this.panhedden1.Size = new System.Drawing.Size(160, 32);
            this.panhedden1.TabIndex = 64;
            this.panhedden1.Visible = false;
            // 
            // cboWard
            // 
            this.cboWard.FormattingEnabled = true;
            this.cboWard.Location = new System.Drawing.Point(33, 6);
            this.cboWard.Name = "cboWard";
            this.cboWard.Size = new System.Drawing.Size(121, 20);
            this.cboWard.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "병동";
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "SS1, Sheet1, Row 0, Column 0, ";
            this.SS1.AllowCellOverflow = true;
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.SS1.Location = new System.Drawing.Point(0, 94);
            this.SS1.Name = "SS1";
            namedStyle4.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Parent = "DataAreaDefault";
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType3.MaxLength = 60;
            namedStyle5.CellType = textCellType3;
            namedStyle5.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Parent = "DataAreaDefault";
            namedStyle5.Renderer = textCellType3;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType4.Static = true;
            namedStyle6.CellType = textCellType4;
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle6.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle6.Renderer = textCellType4;
            namedStyle6.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle4,
            namedStyle5,
            namedStyle6});
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(1234, 617);
            this.SS1.TabIndex = 53;
            this.SS1.TabStripRatio = 0.6D;
            tipAppearance2.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.SS1.TextTipAppearance = tipAppearance2;
            this.SS1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 12;
            this.SS1_Sheet1.RowCount = 1;
            this.SS1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "퇴원예고일";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "등록일시";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "등록번호";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "성명";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "보험";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "나이";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "성별";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "호실";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "과";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "과장";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "퇴원일";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "입원일";
            this.SS1_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet1.ColumnHeader.Rows.Get(0).Height = 32F;
            this.SS1_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet1.Columns.Get(0).Label = "퇴원예고일";
            this.SS1_Sheet1.Columns.Get(0).StyleName = "Static522636452321562472221";
            this.SS1_Sheet1.Columns.Get(0).Width = 85F;
            this.SS1_Sheet1.Columns.Get(1).Label = "등록일시";
            this.SS1_Sheet1.Columns.Get(1).SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.Ascending;
            this.SS1_Sheet1.Columns.Get(1).Width = 113F;
            this.SS1_Sheet1.Columns.Get(2).Label = "등록번호";
            this.SS1_Sheet1.Columns.Get(2).StyleName = "Static522636452321562472221";
            this.SS1_Sheet1.Columns.Get(2).Width = 75F;
            this.SS1_Sheet1.Columns.Get(3).Label = "성명";
            this.SS1_Sheet1.Columns.Get(3).StyleName = "Static522636452321562472221";
            this.SS1_Sheet1.Columns.Get(3).Width = 69F;
            this.SS1_Sheet1.Columns.Get(4).Label = "보험";
            this.SS1_Sheet1.Columns.Get(4).StyleName = "Static522636452321562472221";
            this.SS1_Sheet1.Columns.Get(5).Label = "나이";
            this.SS1_Sheet1.Columns.Get(5).StyleName = "Static522636452321562472221";
            this.SS1_Sheet1.Columns.Get(5).Width = 36F;
            this.SS1_Sheet1.Columns.Get(6).Label = "성별";
            this.SS1_Sheet1.Columns.Get(6).StyleName = "Static522636452321562472221";
            this.SS1_Sheet1.Columns.Get(6).Width = 36F;
            this.SS1_Sheet1.Columns.Get(7).Label = "호실";
            this.SS1_Sheet1.Columns.Get(7).StyleName = "Static522636452321562472221";
            this.SS1_Sheet1.Columns.Get(7).Width = 36F;
            this.SS1_Sheet1.Columns.Get(8).Label = "과";
            this.SS1_Sheet1.Columns.Get(8).StyleName = "Static522636452321562472221";
            this.SS1_Sheet1.Columns.Get(8).Width = 36F;
            this.SS1_Sheet1.Columns.Get(9).Label = "과장";
            this.SS1_Sheet1.Columns.Get(9).Width = 45F;
            this.SS1_Sheet1.Columns.Get(10).Label = "퇴원일";
            this.SS1_Sheet1.Columns.Get(10).StyleName = "Static522636452321562472221";
            this.SS1_Sheet1.Columns.Get(10).Width = 97F;
            this.SS1_Sheet1.Columns.Get(11).Label = "입원일";
            this.SS1_Sheet1.Columns.Get(11).StyleName = "Static522636452321562472221";
            this.SS1_Sheet1.Columns.Get(11).Width = 97F;
            this.SS1_Sheet1.DefaultStyleName = "Text418636452321562322178";
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(152, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 59;
            this.label2.Text = "-";
            // 
            // dtpTDate
            // 
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(163, 6);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(86, 21);
            this.dtpTDate.TabIndex = 58;
            // 
            // frmPmpaViewWardOutList2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 711);
            this.Controls.Add(this.SS1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panTitle);
            this.Name = "frmPmpaViewWardOutList2";
            this.Text = "퇴원예고자 명단";
            this.Load += new System.EventHandler(this.frmPmpaViewWardOutList2_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panhedden1.ResumeLayout(false);
            this.panhedden1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rdoOptgb1;
        private System.Windows.Forms.RadioButton rdoOptgb2;
        private System.Windows.Forms.RadioButton rdoOptgb3;
        private System.Windows.Forms.RadioButton rdoOptgb0;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dtpFdate;
        private System.Windows.Forms.Label lblyyyy;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panhedden1;
        private System.Windows.Forms.ComboBox cboWard;
        private System.Windows.Forms.Label label3;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpTDate;
    }
}