namespace ComPmpaLibB
{
    partial class frmPmpaViewOpdDailyCheck
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
            FarPoint.Win.Spread.NamedStyle namedStyle17 = new FarPoint.Win.Spread.NamedStyle("BorderEx349636397736362401197", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder9 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.NamedStyle namedStyle18 = new FarPoint.Win.Spread.NamedStyle("Text443636397736362401197", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder10 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle19 = new FarPoint.Win.Spread.NamedStyle("Static529636397736362401197");
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle20 = new FarPoint.Win.Spread.NamedStyle("Color839636397736362401197");
            FarPoint.Win.Spread.TipAppearance tipAppearance5 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panTitleSub0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.panel1);
            this.panTitleSub0.Controls.Add(this.panel2);
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(809, 34);
            this.panTitleSub0.TabIndex = 13;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(166, 21);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "외래 일자별 발생점검";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(151, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 28;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(7, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 30;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Location = new System.Drawing.Point(79, 0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 29;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(9, 2);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(104, 25);
            this.dtpDate.TabIndex = 46;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.Location = new System.Drawing.Point(0, 34);
            this.ssView.Name = "ssView";
            namedStyle17.Border = complexBorder9;
            namedStyle17.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle17.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle17.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle17.Parent = "DataAreaDefault";
            namedStyle17.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle18.Border = complexBorder10;
            textCellType9.MaxLength = 60;
            namedStyle18.CellType = textCellType9;
            namedStyle18.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle18.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle18.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle18.Parent = "DataAreaDefault";
            namedStyle18.Renderer = textCellType9;
            namedStyle18.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType10.Static = true;
            namedStyle19.CellType = textCellType10;
            namedStyle19.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle19.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle19.Renderer = textCellType10;
            namedStyle19.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            namedStyle20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            namedStyle20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            namedStyle20.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle20.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle20.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle17,
            namedStyle18,
            namedStyle19,
            namedStyle20});
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(809, 579);
            this.ssView.TabIndex = 47;
            this.ssView.TabStripRatio = 0.6D;
            tipAppearance5.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance5.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssView.TextTipAppearance = tipAppearance5;
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 10;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "과";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "초진";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "JIN";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "접수비";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "진찰코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "수납액";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "예약수납일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "오류내역";
            this.ssView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Get(0).Label = "등록번호";
            this.ssView_Sheet1.Columns.Get(0).StyleName = "Static529636397736362401197";
            this.ssView_Sheet1.Columns.Get(0).Width = 80F;
            this.ssView_Sheet1.Columns.Get(1).Label = "성명";
            this.ssView_Sheet1.Columns.Get(1).StyleName = "Static529636397736362401197";
            this.ssView_Sheet1.Columns.Get(1).Width = 80F;
            this.ssView_Sheet1.Columns.Get(2).Label = "과";
            this.ssView_Sheet1.Columns.Get(2).StyleName = "Static529636397736362401197";
            this.ssView_Sheet1.Columns.Get(2).Width = 40F;
            this.ssView_Sheet1.Columns.Get(3).Label = "초진";
            this.ssView_Sheet1.Columns.Get(3).StyleName = "Static529636397736362401197";
            this.ssView_Sheet1.Columns.Get(3).Width = 50F;
            this.ssView_Sheet1.Columns.Get(4).Label = "JIN";
            this.ssView_Sheet1.Columns.Get(4).StyleName = "Static529636397736362401197";
            this.ssView_Sheet1.Columns.Get(4).Width = 40F;
            this.ssView_Sheet1.Columns.Get(5).Label = "접수비";
            this.ssView_Sheet1.Columns.Get(5).StyleName = "Static529636397736362401197";
            this.ssView_Sheet1.Columns.Get(5).Width = 70F;
            this.ssView_Sheet1.Columns.Get(6).Label = "진찰코드";
            this.ssView_Sheet1.Columns.Get(6).StyleName = "Static529636397736362401197";
            this.ssView_Sheet1.Columns.Get(6).Width = 4F;
            this.ssView_Sheet1.Columns.Get(7).Label = "수납액";
            this.ssView_Sheet1.Columns.Get(7).StyleName = "Static529636397736362401197";
            this.ssView_Sheet1.Columns.Get(7).Width = 4F;
            this.ssView_Sheet1.Columns.Get(8).Label = "예약수납일";
            this.ssView_Sheet1.Columns.Get(8).StyleName = "Static529636397736362401197";
            this.ssView_Sheet1.Columns.Get(8).Width = 3F;
            this.ssView_Sheet1.Columns.Get(9).Label = "오류내역";
            this.ssView_Sheet1.Columns.Get(9).StyleName = "Color839636397736362401197";
            this.ssView_Sheet1.Columns.Get(9).Width = 381F;
            this.ssView_Sheet1.DefaultStyleName = "Text443636397736362401197";
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtpDate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(457, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(122, 30);
            this.panel1.TabIndex = 47;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.btnPrint);
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(579, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(226, 30);
            this.panel2.TabIndex = 48;
            // 
            // frmPmpaViewOpdDailyCheck
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(809, 613);
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.panTitleSub0);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "frmPmpaViewOpdDailyCheck";
            this.Text = "외래 일자별 발생점검";
            this.Load += new System.EventHandler(this.frmPmpaViewOpdDailyCheck_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Button btnSearch;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
    }
}