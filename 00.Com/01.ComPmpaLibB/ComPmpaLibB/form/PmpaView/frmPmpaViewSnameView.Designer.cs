namespace ComPmpaLibB
{
    partial class frmPmpaViewSnameView
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color341636804127316976146", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text405636804127316986122", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static509636804127317006069");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static671636804127317056069");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Static743636804127317085845");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ss1 = new FarPoint.Win.Spread.FpSpread();
            this.ss1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).BeginInit();
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
            this.panTitle.Size = new System.Drawing.Size(716, 34);
            this.panTitle.TabIndex = 20;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(640, 0);
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
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(89, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "수신자 List";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ss1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(716, 325);
            this.panel1.TabIndex = 21;
            // 
            // ss1
            // 
            this.ss1.AccessibleDescription = "ss1, Sheet1, Row 0, Column 0, ";
            this.ss1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss1.Location = new System.Drawing.Point(0, 0);
            this.ss1.Name = "ss1";
            namedStyle1.Font = new System.Drawing.Font("굴림", 11F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 60;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 11F);
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
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType3;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            textCellType4.Static = true;
            namedStyle5.CellType = textCellType4;
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Renderer = textCellType4;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5});
            this.ss1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss1_Sheet1});
            this.ss1.Size = new System.Drawing.Size(716, 325);
            this.ss1.TabIndex = 0;
            this.ss1.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ss1.TextTipAppearance = tipAppearance1;
            this.ss1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ss1_CellDoubleClick);
            // 
            // ss1_Sheet1
            // 
            this.ss1_Sheet1.Reset();
            this.ss1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss1_Sheet1.ColumnCount = 11;
            this.ss1_Sheet1.RowCount = 1;
            this.ss1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "수신자";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "성별";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "주민등록번호";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "최종내원일";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "지역";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "전화번호";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "주소";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "최초내원일";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "우편번호";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "주소1";
            this.ss1_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ss1_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ss1_Sheet1.Columns.Get(0).Label = "등록번호";
            this.ss1_Sheet1.Columns.Get(0).StyleName = "Static509636804127317006069";
            this.ss1_Sheet1.Columns.Get(0).Width = 76F;
            this.ss1_Sheet1.Columns.Get(1).Label = "수신자";
            this.ss1_Sheet1.Columns.Get(1).StyleName = "Static509636804127317006069";
            this.ss1_Sheet1.Columns.Get(2).Label = "성별";
            this.ss1_Sheet1.Columns.Get(2).StyleName = "Static509636804127317006069";
            this.ss1_Sheet1.Columns.Get(2).Width = 37F;
            this.ss1_Sheet1.Columns.Get(3).Label = "주민등록번호";
            this.ss1_Sheet1.Columns.Get(3).StyleName = "Static509636804127317006069";
            this.ss1_Sheet1.Columns.Get(3).Width = 128F;
            this.ss1_Sheet1.Columns.Get(4).Label = "최종내원일";
            this.ss1_Sheet1.Columns.Get(4).StyleName = "Static509636804127317006069";
            this.ss1_Sheet1.Columns.Get(4).Width = 92F;
            this.ss1_Sheet1.Columns.Get(5).Label = "지역";
            this.ss1_Sheet1.Columns.Get(5).StyleName = "Static671636804127317056069";
            this.ss1_Sheet1.Columns.Get(5).Width = 169F;
            this.ss1_Sheet1.Columns.Get(6).Label = "전화번호";
            this.ss1_Sheet1.Columns.Get(6).StyleName = "Static509636804127317006069";
            this.ss1_Sheet1.Columns.Get(6).Width = 95F;
            this.ss1_Sheet1.Columns.Get(7).Label = "주소";
            this.ss1_Sheet1.Columns.Get(7).StyleName = "Static743636804127317085845";
            this.ss1_Sheet1.Columns.Get(7).Width = 111F;
            this.ss1_Sheet1.Columns.Get(8).Label = "최초내원일";
            this.ss1_Sheet1.Columns.Get(8).StyleName = "Static509636804127317006069";
            this.ss1_Sheet1.Columns.Get(8).Width = 81F;
            this.ss1_Sheet1.Columns.Get(9).Label = "우편번호";
            this.ss1_Sheet1.Columns.Get(9).StyleName = "Static509636804127317006069";
            this.ss1_Sheet1.Columns.Get(9).Width = 85F;
            this.ss1_Sheet1.DefaultStyleName = "Text405636804127316986122";
            this.ss1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmPmpaViewSnameView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 359);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmPmpaViewSnameView";
            this.Text = "frmPmpaViewSnameView";
            this.Load += new System.EventHandler(this.frmPmpaViewSnameView_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExit;
        private FarPoint.Win.Spread.FpSpread ss1;
        private FarPoint.Win.Spread.SheetView ss1_Sheet1;
    }
}