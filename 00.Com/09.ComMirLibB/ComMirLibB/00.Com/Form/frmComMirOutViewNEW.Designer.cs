namespace ComMirLibB.Com
{
    partial class frmComMirOutViewNEW
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color383636487012500575972", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text491636487012500732094", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static606636487012500732094");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static624636487012500732094");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Static660636487012500732094");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Text974636487012500732094");
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
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
            this.panTitle.Size = new System.Drawing.Size(1225, 34);
            this.panTitle.TabIndex = 96;
            // 
            // btnSearch
            // 
            this.btnSearch.AutoSize = true;
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnSearch.Location = new System.Drawing.Point(1064, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(88, 32);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "다시조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.btnExit.Location = new System.Drawing.Point(1152, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 32);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(6, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(279, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "청구 원외처방 내역 VIEW(MIR_OUT)";
            // 
            // ssMain
            // 
            this.ssMain.AccessibleDescription = "ssMain, Sheet1, Row 0, Column 0, ";
            this.ssMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssMain.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.ssMain.Location = new System.Drawing.Point(0, 34);
            this.ssMain.Name = "ssMain";
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
            textCellType5.MaxLength = 1;
            namedStyle6.CellType = textCellType5;
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle6.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle6.Renderer = textCellType5;
            namedStyle6.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMain.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5,
            namedStyle6});
            this.ssMain.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssMain_Sheet1});
            this.ssMain.Size = new System.Drawing.Size(1225, 572);
            this.ssMain.TabIndex = 97;
            this.ssMain.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssMain.TextTipAppearance = tipAppearance1;
            this.ssMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ssMain_KeyPress);
            this.ssMain.SetViewportLeftColumn(0, 0, 2);
            this.ssMain.SetActiveViewport(0, 0, -1);
            // 
            // ssMain_Sheet1
            // 
            this.ssMain_Sheet1.Reset();
            this.ssMain_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssMain_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssMain_Sheet1.ColumnCount = 22;
            this.ssMain_Sheet1.RowCount = 1;
            this.ssMain_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "청구번호";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "청구년월";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "진료과";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "처방번호";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "수가코드";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "수량";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "1회투여";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "#";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "날수";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "수가명칭";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "표준코드";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "표준코드명";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "약품분류";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "약품분류명";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "저함량사유";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "저함량사유(참고사항)";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "동일성분";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 17).Value = "동일성분(참고)";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 18).Value = "rowid";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 19).Value = "WRTNOS";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 20).Value = "원외처방번호";
            this.ssMain_Sheet1.ColumnHeader.Cells.Get(0, 21).Value = "V252";
            this.ssMain_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMain_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssMain_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssMain_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.ColumnHeader.Rows.Get(0).Height = 35F;
            this.ssMain_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMain_Sheet1.Columns.Get(0).Label = "청구번호";
            this.ssMain_Sheet1.Columns.Get(0).Width = 67F;
            this.ssMain_Sheet1.Columns.Get(1).Label = "청구년월";
            this.ssMain_Sheet1.Columns.Get(1).Width = 67F;
            this.ssMain_Sheet1.Columns.Get(3).Label = "처방번호";
            this.ssMain_Sheet1.Columns.Get(3).StyleName = "Static606636487012500732094";
            this.ssMain_Sheet1.Columns.Get(3).Width = 105F;
            this.ssMain_Sheet1.Columns.Get(4).Label = "수가코드";
            this.ssMain_Sheet1.Columns.Get(4).StyleName = "Static624636487012500732094";
            this.ssMain_Sheet1.Columns.Get(4).Width = 67F;
            this.ssMain_Sheet1.Columns.Get(5).Label = "수량";
            this.ssMain_Sheet1.Columns.Get(5).StyleName = "Static660636487012500732094";
            this.ssMain_Sheet1.Columns.Get(5).Width = 49F;
            this.ssMain_Sheet1.Columns.Get(6).Label = "1회투여";
            this.ssMain_Sheet1.Columns.Get(6).StyleName = "Static660636487012500732094";
            this.ssMain_Sheet1.Columns.Get(6).Width = 61F;
            this.ssMain_Sheet1.Columns.Get(7).Label = "#";
            this.ssMain_Sheet1.Columns.Get(7).StyleName = "Static606636487012500732094";
            this.ssMain_Sheet1.Columns.Get(7).Width = 17F;
            this.ssMain_Sheet1.Columns.Get(8).Label = "날수";
            this.ssMain_Sheet1.Columns.Get(8).StyleName = "Static606636487012500732094";
            this.ssMain_Sheet1.Columns.Get(8).Width = 39F;
            this.ssMain_Sheet1.Columns.Get(9).Label = "수가명칭";
            this.ssMain_Sheet1.Columns.Get(9).StyleName = "Static624636487012500732094";
            this.ssMain_Sheet1.Columns.Get(9).Width = 196F;
            this.ssMain_Sheet1.Columns.Get(10).Label = "표준코드";
            this.ssMain_Sheet1.Columns.Get(10).Width = 82F;
            this.ssMain_Sheet1.Columns.Get(11).Label = "표준코드명";
            this.ssMain_Sheet1.Columns.Get(11).StyleName = "Static624636487012500732094";
            this.ssMain_Sheet1.Columns.Get(11).Width = 189F;
            this.ssMain_Sheet1.Columns.Get(12).Label = "약품분류";
            this.ssMain_Sheet1.Columns.Get(12).Width = 47F;
            this.ssMain_Sheet1.Columns.Get(13).Label = "약품분류명";
            this.ssMain_Sheet1.Columns.Get(13).Width = 139F;
            this.ssMain_Sheet1.Columns.Get(14).Label = "저함량사유";
            this.ssMain_Sheet1.Columns.Get(14).Width = 81F;
            this.ssMain_Sheet1.Columns.Get(15).Label = "저함량사유(참고사항)";
            this.ssMain_Sheet1.Columns.Get(15).Width = 221F;
            this.ssMain_Sheet1.Columns.Get(16).Label = "동일성분";
            this.ssMain_Sheet1.Columns.Get(16).StyleName = "Text974636487012500732094";
            this.ssMain_Sheet1.Columns.Get(16).Width = 67F;
            this.ssMain_Sheet1.Columns.Get(17).Label = "동일성분(참고)";
            this.ssMain_Sheet1.Columns.Get(17).Width = 291F;
            this.ssMain_Sheet1.Columns.Get(19).Label = "WRTNOS";
            this.ssMain_Sheet1.Columns.Get(19).Visible = false;
            this.ssMain_Sheet1.Columns.Get(20).Label = "원외처방번호";
            this.ssMain_Sheet1.Columns.Get(20).Visible = false;
            this.ssMain_Sheet1.Columns.Get(21).Label = "V252";
            this.ssMain_Sheet1.Columns.Get(21).Width = 32F;
            this.ssMain_Sheet1.DefaultStyleName = "Text491636487012500732094";
            this.ssMain_Sheet1.FrozenColumnCount = 2;
            this.ssMain_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssMain_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssMain_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssMain_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.RowHeader.Visible = false;
            this.ssMain_Sheet1.Rows.Default.Height = 16F;
            this.ssMain_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssMain_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssMain_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmComMirOutViewNEW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1225, 606);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.panTitle);
            this.Name = "frmComMirOutViewNEW";
            this.Text = "frmComMirOutViewNEW";
            this.Load += new System.EventHandler(this.frmComMirOutViewNEW_Load);
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