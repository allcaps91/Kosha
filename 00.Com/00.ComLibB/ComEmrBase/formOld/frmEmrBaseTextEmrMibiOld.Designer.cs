namespace ComEmrBase
{
    partial class frmEmrBaseTextEmrMibiOld
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("BorderEx374636622744557244183", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text501636622744557400184", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Text613636622744557400184");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Text1219636622744557556184");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Static1465636622744557556184");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.ssMiBi = new FarPoint.Win.Spread.FpSpread();
            this.ssMiBi_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssMiBi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMiBi_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.btnSearch);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1040, 34);
            this.panTitle.TabIndex = 14;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(960, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 30;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(888, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 28;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(178, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "미비 현황 통계(퇴원자)";
            // 
            // ssMiBi
            // 
            this.ssMiBi.AccessibleDescription = "ssStatisticsView1, Sheet1, Row 0, Column 0, 99999999";
            this.ssMiBi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssMiBi.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssMiBi.Location = new System.Drawing.Point(0, 34);
            this.ssMiBi.Name = "ssMiBi";
            namedStyle1.Border = complexBorder1;
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.Locked = true;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle2.Border = complexBorder2;
            textCellType1.MaxLength = 32000;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.Locked = true;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType2.MaxLength = 32000;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.MaxLength = 32000;
            textCellType3.Multiline = true;
            namedStyle4.CellType = textCellType3;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType3;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType4.Static = true;
            textCellType4.WordWrap = true;
            namedStyle5.CellType = textCellType4;
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Renderer = textCellType4;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMiBi.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5});
            this.ssMiBi.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssMiBi_Sheet1});
            this.ssMiBi.Size = new System.Drawing.Size(1040, 390);
            this.ssMiBi.TabIndex = 15;
            this.ssMiBi.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssMiBi.TextTipAppearance = tipAppearance1;
            this.ssMiBi.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssMiBi.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssMiBi_CellClick);
            this.ssMiBi.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssMiBi_CellDoubleClick);
            // 
            // ssMiBi_Sheet1
            // 
            this.ssMiBi_Sheet1.Reset();
            this.ssMiBi_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssMiBi_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssMiBi_Sheet1.ColumnCount = 19;
            this.ssMiBi_Sheet1.RowCount = 1;
            this.ssMiBi_Sheet1.Cells.Get(0, 0).Value = "99999999";
            this.ssMiBi_Sheet1.Cells.Get(0, 2).Value = "9999-99-99";
            this.ssMiBi_Sheet1.Cells.Get(0, 3).Value = "9999-99-99";
            this.ssMiBi_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성  명";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "입원일";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "퇴원일";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "진료과";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "진료의";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "MedFrTime";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "MedEndTime";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "MedDeptCd";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "MedDrCd";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "Sex";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "Age";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "입/퇴원요약지";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "동의서";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "입원기록지";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "경과기록지";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "수술기록지";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 17).Value = "전과기록지";
            this.ssMiBi_Sheet1.ColumnHeader.Cells.Get(0, 18).Value = "시술기록지";
            this.ssMiBi_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMiBi_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(242)))), ((int)(((byte)(248)))));
            this.ssMiBi_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMiBi_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMiBi_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssMiBi_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMiBi_Sheet1.ColumnHeader.Rows.Get(0).Height = 37F;
            this.ssMiBi_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssMiBi_Sheet1.Columns.Get(0).Label = "등록번호";
            this.ssMiBi_Sheet1.Columns.Get(0).StyleName = "Text613636622744557400184";
            this.ssMiBi_Sheet1.Columns.Get(0).Width = 72F;
            this.ssMiBi_Sheet1.Columns.Get(1).Label = "성  명";
            this.ssMiBi_Sheet1.Columns.Get(1).StyleName = "Text613636622744557400184";
            this.ssMiBi_Sheet1.Columns.Get(1).Width = 65F;
            this.ssMiBi_Sheet1.Columns.Get(2).Label = "입원일";
            this.ssMiBi_Sheet1.Columns.Get(2).StyleName = "Text613636622744557400184";
            this.ssMiBi_Sheet1.Columns.Get(2).Width = 81F;
            this.ssMiBi_Sheet1.Columns.Get(3).Label = "퇴원일";
            this.ssMiBi_Sheet1.Columns.Get(3).StyleName = "Text613636622744557400184";
            this.ssMiBi_Sheet1.Columns.Get(3).Width = 81F;
            this.ssMiBi_Sheet1.Columns.Get(4).Label = "진료과";
            this.ssMiBi_Sheet1.Columns.Get(4).StyleName = "Text613636622744557400184";
            this.ssMiBi_Sheet1.Columns.Get(4).Visible = false;
            this.ssMiBi_Sheet1.Columns.Get(4).Width = 102F;
            this.ssMiBi_Sheet1.Columns.Get(5).Label = "진료의";
            this.ssMiBi_Sheet1.Columns.Get(5).StyleName = "Text613636622744557400184";
            this.ssMiBi_Sheet1.Columns.Get(5).Visible = false;
            this.ssMiBi_Sheet1.Columns.Get(5).Width = 72F;
            this.ssMiBi_Sheet1.Columns.Get(6).Label = "MedFrTime";
            this.ssMiBi_Sheet1.Columns.Get(6).Visible = false;
            this.ssMiBi_Sheet1.Columns.Get(7).Label = "MedEndTime";
            this.ssMiBi_Sheet1.Columns.Get(7).Visible = false;
            this.ssMiBi_Sheet1.Columns.Get(8).Label = "MedDeptCd";
            this.ssMiBi_Sheet1.Columns.Get(8).Visible = false;
            this.ssMiBi_Sheet1.Columns.Get(9).Label = "MedDrCd";
            this.ssMiBi_Sheet1.Columns.Get(9).Visible = false;
            this.ssMiBi_Sheet1.Columns.Get(10).Label = "Sex";
            this.ssMiBi_Sheet1.Columns.Get(10).Visible = false;
            this.ssMiBi_Sheet1.Columns.Get(11).Label = "Age";
            this.ssMiBi_Sheet1.Columns.Get(11).Visible = false;
            this.ssMiBi_Sheet1.Columns.Get(12).Label = "입/퇴원요약지";
            this.ssMiBi_Sheet1.Columns.Get(12).StyleName = "Text1219636622744557556184";
            this.ssMiBi_Sheet1.Columns.Get(12).Width = 112F;
            this.ssMiBi_Sheet1.Columns.Get(13).Label = "동의서";
            this.ssMiBi_Sheet1.Columns.Get(13).StyleName = "Text1219636622744557556184";
            this.ssMiBi_Sheet1.Columns.Get(13).Width = 97F;
            this.ssMiBi_Sheet1.Columns.Get(14).Label = "입원기록지";
            this.ssMiBi_Sheet1.Columns.Get(14).StyleName = "Text1219636622744557556184";
            this.ssMiBi_Sheet1.Columns.Get(14).Width = 97F;
            this.ssMiBi_Sheet1.Columns.Get(15).Label = "경과기록지";
            this.ssMiBi_Sheet1.Columns.Get(15).StyleName = "Text1219636622744557556184";
            this.ssMiBi_Sheet1.Columns.Get(15).Width = 97F;
            this.ssMiBi_Sheet1.Columns.Get(16).Label = "수술기록지";
            this.ssMiBi_Sheet1.Columns.Get(16).StyleName = "Text1219636622744557556184";
            this.ssMiBi_Sheet1.Columns.Get(16).Width = 97F;
            this.ssMiBi_Sheet1.Columns.Get(17).Label = "전과기록지";
            this.ssMiBi_Sheet1.Columns.Get(17).StyleName = "Static1465636622744557556184";
            this.ssMiBi_Sheet1.Columns.Get(17).Width = 97F;
            this.ssMiBi_Sheet1.Columns.Get(18).Label = "시술기록지";
            this.ssMiBi_Sheet1.Columns.Get(18).StyleName = "Static1465636622744557556184";
            this.ssMiBi_Sheet1.Columns.Get(18).Width = 97F;
            this.ssMiBi_Sheet1.DefaultStyleName = "Text501636622744557400184";
            this.ssMiBi_Sheet1.GrayAreaBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ssMiBi_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.RowMode;
            this.ssMiBi_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssMiBi_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(242)))), ((int)(((byte)(248)))));
            this.ssMiBi_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMiBi_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMiBi_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssMiBi_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMiBi_Sheet1.Rows.Default.Height = 29F;
            this.ssMiBi_Sheet1.Rows.Get(0).Height = 48F;
            this.ssMiBi_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(242)))), ((int)(((byte)(248)))));
            this.ssMiBi_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMiBi_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMiBi_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssMiBi_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMiBi_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmEmrBaseTextEmrMibiOld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1040, 424);
            this.Controls.Add(this.ssMiBi);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmEmrBaseTextEmrMibiOld";
            this.Text = "frmEmrBaseTextEmrMibiOld";
            this.Load += new System.EventHandler(this.frmEmrBaseTextEmrMibiOld_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssMiBi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMiBi_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblTitle;
        private FarPoint.Win.Spread.FpSpread ssMiBi;
        private FarPoint.Win.Spread.SheetView ssMiBi_Sheet1;
    }
}