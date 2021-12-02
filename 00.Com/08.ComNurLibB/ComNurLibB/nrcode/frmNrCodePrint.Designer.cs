namespace ComNurLibB
{
    partial class frmNrCodePrint
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color411636280470268193802", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("BorderEx605636280470268350237");
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.EmptyBorder emptyBorder1 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder2 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder3 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder4 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder5 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder6 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder7 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder8 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder9 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder10 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder11 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssNrCodePrint = new FarPoint.Win.Spread.FpSpread();
            this.ssNrCodePrint_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssNrCodePrint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssNrCodePrint_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnSearch);
            this.panTitle.Controls.Add(this.btnPrint);
            this.panTitle.Controls.Add(this.btnCancel);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(705, 36);
            this.panTitle.TabIndex = 12;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(617, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 28;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(190, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "간호부 기초코드집 인쇄";
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(401, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 31;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Location = new System.Drawing.Point(545, 2);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 29;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Location = new System.Drawing.Point(473, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 30);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssNrCodePrint);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(705, 527);
            this.panel1.TabIndex = 14;
            // 
            // ssNrCodePrint
            // 
            this.ssNrCodePrint.AccessibleDescription = "ssNrCodePrint, Sheet1, Row 0, Column 0, ";
            this.ssNrCodePrint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssNrCodePrint.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssNrCodePrint.Location = new System.Drawing.Point(0, 0);
            this.ssNrCodePrint.Name = "ssNrCodePrint";
            namedStyle1.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle2.Border = complexBorder1;
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssNrCodePrint.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            new FarPoint.Win.Spread.NamedStyle("Static499636280470268350237", "DataAreaDefault"),
            namedStyle2});
            this.ssNrCodePrint.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssNrCodePrint_Sheet1});
            this.ssNrCodePrint.Size = new System.Drawing.Size(705, 527);
            this.ssNrCodePrint.TabIndex = 0;
            this.ssNrCodePrint.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssNrCodePrint.TextTipAppearance = tipAppearance1;
            // 
            // ssNrCodePrint_Sheet1
            // 
            this.ssNrCodePrint_Sheet1.Reset();
            this.ssNrCodePrint_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssNrCodePrint_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssNrCodePrint_Sheet1.ColumnCount = 11;
            this.ssNrCodePrint_Sheet1.RowCount = 1;
            this.ssNrCodePrint_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssNrCodePrint_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssNrCodePrint_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssNrCodePrint_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssNrCodePrint_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssNrCodePrint_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssNrCodePrint_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssNrCodePrint_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssNrCodePrint_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssNrCodePrint_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "코드";
            this.ssNrCodePrint_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "직책명칭";
            this.ssNrCodePrint_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "코드";
            this.ssNrCodePrint_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "병동명칭";
            this.ssNrCodePrint_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "코드";
            this.ssNrCodePrint_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "주사분류명";
            this.ssNrCodePrint_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "코드";
            this.ssNrCodePrint_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "근무형태";
            this.ssNrCodePrint_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssNrCodePrint_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssNrCodePrint_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssNrCodePrint_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssNrCodePrint_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssNrCodePrint_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssNrCodePrint_Sheet1.Columns.Get(0).Border = emptyBorder1;
            this.ssNrCodePrint_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssNrCodePrint_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(0).Label = "코드";
            this.ssNrCodePrint_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(0).Width = 43F;
            this.ssNrCodePrint_Sheet1.Columns.Get(1).Border = emptyBorder2;
            this.ssNrCodePrint_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssNrCodePrint_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(1).Label = "직책명칭";
            this.ssNrCodePrint_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(1).Width = 74F;
            this.ssNrCodePrint_Sheet1.Columns.Get(2).Border = emptyBorder3;
            this.ssNrCodePrint_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssNrCodePrint_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(2).Width = 5F;
            this.ssNrCodePrint_Sheet1.Columns.Get(3).Border = emptyBorder4;
            this.ssNrCodePrint_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssNrCodePrint_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(3).Label = "코드";
            this.ssNrCodePrint_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(3).Width = 46F;
            this.ssNrCodePrint_Sheet1.Columns.Get(4).Border = emptyBorder5;
            this.ssNrCodePrint_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssNrCodePrint_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(4).Label = "병동명칭";
            this.ssNrCodePrint_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(4).Width = 73F;
            this.ssNrCodePrint_Sheet1.Columns.Get(5).Border = emptyBorder6;
            this.ssNrCodePrint_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssNrCodePrint_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(5).Width = 4F;
            this.ssNrCodePrint_Sheet1.Columns.Get(6).Border = emptyBorder7;
            this.ssNrCodePrint_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.ssNrCodePrint_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(6).Label = "코드";
            this.ssNrCodePrint_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(6).Width = 46F;
            this.ssNrCodePrint_Sheet1.Columns.Get(7).Border = emptyBorder8;
            this.ssNrCodePrint_Sheet1.Columns.Get(7).CellType = textCellType8;
            this.ssNrCodePrint_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(7).Label = "주사분류명";
            this.ssNrCodePrint_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(7).Width = 129F;
            this.ssNrCodePrint_Sheet1.Columns.Get(8).Border = emptyBorder9;
            this.ssNrCodePrint_Sheet1.Columns.Get(8).CellType = textCellType9;
            this.ssNrCodePrint_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(8).Width = 4F;
            this.ssNrCodePrint_Sheet1.Columns.Get(9).Border = emptyBorder10;
            this.ssNrCodePrint_Sheet1.Columns.Get(9).CellType = textCellType10;
            this.ssNrCodePrint_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(9).Label = "코드";
            this.ssNrCodePrint_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(9).Width = 51F;
            this.ssNrCodePrint_Sheet1.Columns.Get(10).Border = emptyBorder11;
            this.ssNrCodePrint_Sheet1.Columns.Get(10).CellType = textCellType11;
            this.ssNrCodePrint_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(10).Label = "근무형태";
            this.ssNrCodePrint_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssNrCodePrint_Sheet1.Columns.Get(10).Width = 180F;
            this.ssNrCodePrint_Sheet1.DefaultStyleName = "Static499636280470268350237";
            this.ssNrCodePrint_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssNrCodePrint_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssNrCodePrint_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssNrCodePrint_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssNrCodePrint_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssNrCodePrint_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssNrCodePrint_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssNrCodePrint_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssNrCodePrint_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssNrCodePrint_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.ssNrCodePrint_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssNrCodePrint_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssNrCodePrint_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssNrCodePrint_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssNrCodePrint_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssNrCodePrint_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssNrCodePrint_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssNrCodePrint_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssNrCodePrint_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssNrCodePrint_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.ssNrCodePrint_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmNrCodePrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 563);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmNrCodePrint";
            this.Text = "간호부 기초코드집 인쇄";
            this.Load += new System.EventHandler(this.frmNrCodePrint_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssNrCodePrint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssNrCodePrint_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssNrCodePrint;
        private FarPoint.Win.Spread.SheetView ssNrCodePrint_Sheet1;
    }
}