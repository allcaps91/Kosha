namespace ComMirLibB.Com
{
    partial class frmComMirDtlViewNEW
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color392636486969986966340", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text456636486969986986389", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static542636486969986996421");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static614636486969987036524");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Static632636486969987036524");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.ssMain = new FarPoint.Win.Spread.FpSpread();
            this.ssMain_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.Controls.Add(this.btnSearch);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Padding = new System.Windows.Forms.Padding(1);
            this.panTitle.Size = new System.Drawing.Size(875, 34);
            this.panTitle.TabIndex = 95;
            // 
            // btnSearch
            // 
            this.btnSearch.AutoSize = true;
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnSearch.Location = new System.Drawing.Point(714, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 32);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "다시조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnExit.Location = new System.Drawing.Point(802, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 32);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(6, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(205, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "청구 내역 VIEW(MIR_DTL)";
            // 
            // ssMain
            // 
            this.ssMain.AccessibleDescription = "";
            this.ssMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssMain.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.ssMain.Location = new System.Drawing.Point(0, 34);
            this.ssMain.Name = "ssMain";
            namedStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 32000;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
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
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType4.Static = true;
            namedStyle5.CellType = textCellType4;
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Renderer = textCellType4;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMain.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5});
            this.ssMain.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssMain_Sheet1});
            this.ssMain.Size = new System.Drawing.Size(875, 578);
            this.ssMain.TabIndex = 96;
            this.ssMain.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssMain.TextTipAppearance = tipAppearance1;
            // 
            // ssMain_Sheet1
            // 
            this.ssMain_Sheet1.Reset();
            this.ssMain_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssMain_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssMain_Sheet1.ColumnCount = 9;
            this.ssMain_Sheet1.RowCount = 1;
            this.ssMain_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "청구번호";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "청구년월";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "진료과";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "수가";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "수가명";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "단가";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "수량";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "날수";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "금액";
            this.ssMain_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMain_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssMain_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMain_Sheet1.Columns.Get(0).Label = "청구번호";
            this.ssMain_Sheet1.Columns.Get(0).StyleName = "Static542636486969986996421";
            this.ssMain_Sheet1.Columns.Get(0).Width = 81F;
            this.ssMain_Sheet1.Columns.Get(1).Label = "청구년월";
            this.ssMain_Sheet1.Columns.Get(1).StyleName = "Static542636486969986996421";
            this.ssMain_Sheet1.Columns.Get(1).Width = 67F;
            this.ssMain_Sheet1.Columns.Get(2).Label = "진료과";
            this.ssMain_Sheet1.Columns.Get(2).StyleName = "Static542636486969986996421";
            this.ssMain_Sheet1.Columns.Get(2).Width = 75F;
            this.ssMain_Sheet1.Columns.Get(3).Label = "수가";
            this.ssMain_Sheet1.Columns.Get(3).Width = 74F;
            this.ssMain_Sheet1.Columns.Get(4).Label = "수가명";
            this.ssMain_Sheet1.Columns.Get(4).StyleName = "Static614636486969987036524";
            this.ssMain_Sheet1.Columns.Get(4).Width = 247F;
            this.ssMain_Sheet1.Columns.Get(5).Label = "단가";
            this.ssMain_Sheet1.Columns.Get(5).StyleName = "Static632636486969987036524";
            this.ssMain_Sheet1.Columns.Get(6).Label = "수량";
            this.ssMain_Sheet1.Columns.Get(6).StyleName = "Static632636486969987036524";
            this.ssMain_Sheet1.Columns.Get(7).Label = "날수";
            this.ssMain_Sheet1.Columns.Get(7).StyleName = "Static632636486969987036524";
            this.ssMain_Sheet1.Columns.Get(7).Width = 59F;
            this.ssMain_Sheet1.Columns.Get(8).Label = "금액";
            this.ssMain_Sheet1.Columns.Get(8).StyleName = "Static632636486969987036524";
            this.ssMain_Sheet1.Columns.Get(8).Width = 92F;
            this.ssMain_Sheet1.DefaultStyleName = "Text456636486969986986389";
            this.ssMain_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssMain_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmComMirDtlViewNEW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 612);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.panTitle);
            this.Name = "frmComMirDtlViewNEW";
            this.Text = "frmComMirDtlViewNEW";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private FarPoint.Win.Spread.FpSpread ssMain;
        private FarPoint.Win.Spread.SheetView ssMain_Sheet1;
    }
}