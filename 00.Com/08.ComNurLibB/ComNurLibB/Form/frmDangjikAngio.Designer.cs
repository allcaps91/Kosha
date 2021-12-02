namespace ComNurLibB
{
    partial class frmDangjikAngio
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
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("BorderEx360636282841800973033", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder3 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))));
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Text454636282841801063038", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder4 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))))));
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Text1003636282841801203046");
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.EmptyBorder emptyBorder8 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder9 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder10 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder11 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder12 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder13 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.EmptyBorder emptyBorder14 = new FarPoint.Win.EmptyBorder();
            this.panheader4 = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.cboGubun = new System.Windows.Forms.ComboBox();
            this.cboYYMM = new System.Windows.Forms.ComboBox();
            this.btnSearch3 = new System.Windows.Forms.Button();
            this.btnSearch2 = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.txtMemo = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panheader4.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panheader4
            // 
            this.panheader4.BackColor = System.Drawing.Color.RoyalBlue;
            this.panheader4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panheader4.Controls.Add(this.lblTitle);
            this.panheader4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panheader4.Location = new System.Drawing.Point(0, 0);
            this.panheader4.Name = "panheader4";
            this.panheader4.Size = new System.Drawing.Size(1161, 41);
            this.panheader4.TabIndex = 122;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(4, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(273, 21);
            this.lblTitle.TabIndex = 41;
            this.lblTitle.Text = "Angio/운수실/전산실콜 당직자 조회";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.cboGubun);
            this.panel3.Controls.Add(this.cboYYMM);
            this.panel3.Controls.Add(this.btnSearch3);
            this.panel3.Controls.Add(this.btnSearch2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 41);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(2, 5, 3, 5);
            this.panel3.Size = new System.Drawing.Size(1161, 37);
            this.panel3.TabIndex = 123;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1088, 5);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(70, 27);
            this.btnExit.TabIndex = 179;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(198, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 17);
            this.label10.TabIndex = 178;
            this.label10.Text = "당직구분";
            // 
            // cboGubun
            // 
            this.cboGubun.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboGubun.FormattingEnabled = true;
            this.cboGubun.Location = new System.Drawing.Point(261, 6);
            this.cboGubun.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboGubun.Name = "cboGubun";
            this.cboGubun.Size = new System.Drawing.Size(115, 25);
            this.cboGubun.TabIndex = 177;
            // 
            // cboYYMM
            // 
            this.cboYYMM.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboYYMM.FormattingEnabled = true;
            this.cboYYMM.Location = new System.Drawing.Point(34, 7);
            this.cboYYMM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboYYMM.Name = "cboYYMM";
            this.cboYYMM.Size = new System.Drawing.Size(117, 25);
            this.cboYYMM.TabIndex = 174;
            // 
            // btnSearch3
            // 
            this.btnSearch3.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch3.Location = new System.Drawing.Point(154, 5);
            this.btnSearch3.Name = "btnSearch3";
            this.btnSearch3.Size = new System.Drawing.Size(25, 27);
            this.btnSearch3.TabIndex = 7;
            this.btnSearch3.Text = "▶";
            this.btnSearch3.UseVisualStyleBackColor = true;
            // 
            // btnSearch2
            // 
            this.btnSearch2.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnSearch2.Font = new System.Drawing.Font("굴림체", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch2.Location = new System.Drawing.Point(2, 5);
            this.btnSearch2.Name = "btnSearch2";
            this.btnSearch2.Size = new System.Drawing.Size(28, 27);
            this.btnSearch2.TabIndex = 6;
            this.btnSearch2.Text = "◀";
            this.btnSearch2.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 78);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.SS1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.txtMemo);
            this.splitContainer2.Panel2.Controls.Add(this.panel1);
            this.splitContainer2.Size = new System.Drawing.Size(1161, 622);
            this.splitContainer2.SplitterDistance = 863;
            this.splitContainer2.TabIndex = 126;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "SS1, Sheet1, Row 0, Column 0, ";
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SS1.Location = new System.Drawing.Point(0, 0);
            this.SS1.Name = "SS1";
            namedStyle4.Border = complexBorder3;
            namedStyle4.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Parent = "DataAreaDefault";
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle5.Border = complexBorder4;
            textCellType3.MaxLength = 120;
            textCellType3.Multiline = true;
            namedStyle5.CellType = textCellType3;
            namedStyle5.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Parent = "DataAreaDefault";
            namedStyle5.Renderer = textCellType3;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType4.MaxLength = 500;
            textCellType4.Multiline = true;
            namedStyle6.CellType = textCellType4;
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle6.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle6.Renderer = textCellType4;
            namedStyle6.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
            this.SS1.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle4,
            namedStyle5,
            namedStyle6});
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(863, 622);
            this.SS1.TabIndex = 94;
            this.SS1.TabStripRatio = 0.6D;
            tipAppearance2.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.SS1.TextTipAppearance = tipAppearance2;
            this.SS1.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Fixed;
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 7;
            this.SS1_Sheet1.RowCount = 5;
            this.SS1_Sheet1.Cells.Get(0, 1).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(0, 2).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(0, 3).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(0, 4).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(0, 5).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(0, 6).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(1, 1).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(1, 2).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(1, 3).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(1, 4).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(1, 5).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(1, 6).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(2, 1).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(2, 2).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(2, 3).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(2, 4).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(2, 5).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(2, 6).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(3, 1).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(3, 2).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(3, 3).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(3, 4).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(3, 5).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(3, 6).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(4, 1).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(4, 2).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(4, 3).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(4, 4).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(4, 5).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.Cells.Get(4, 6).StyleName = "Text1003636282841801203046";
            this.SS1_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "일";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "월";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "화";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "수";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "목";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "금";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "토";
            this.SS1_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.SS1_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.SS1_Sheet1.Columns.Get(0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(210)))), ((int)(((byte)(210)))));
            this.SS1_Sheet1.Columns.Get(0).Border = emptyBorder8;
            this.SS1_Sheet1.Columns.Get(0).Label = "일";
            this.SS1_Sheet1.Columns.Get(0).Width = 134F;
            this.SS1_Sheet1.Columns.Get(1).Border = emptyBorder9;
            this.SS1_Sheet1.Columns.Get(1).Label = "월";
            this.SS1_Sheet1.Columns.Get(1).Width = 134F;
            this.SS1_Sheet1.Columns.Get(2).Border = emptyBorder10;
            this.SS1_Sheet1.Columns.Get(2).Label = "화";
            this.SS1_Sheet1.Columns.Get(2).Width = 134F;
            this.SS1_Sheet1.Columns.Get(3).Border = emptyBorder11;
            this.SS1_Sheet1.Columns.Get(3).Label = "수";
            this.SS1_Sheet1.Columns.Get(3).Width = 134F;
            this.SS1_Sheet1.Columns.Get(4).Border = emptyBorder12;
            this.SS1_Sheet1.Columns.Get(4).Label = "목";
            this.SS1_Sheet1.Columns.Get(4).Width = 134F;
            this.SS1_Sheet1.Columns.Get(5).Border = emptyBorder13;
            this.SS1_Sheet1.Columns.Get(5).Label = "금";
            this.SS1_Sheet1.Columns.Get(5).Width = 134F;
            this.SS1_Sheet1.Columns.Get(6).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(244)))), ((int)(((byte)(253)))));
            this.SS1_Sheet1.Columns.Get(6).Border = emptyBorder14;
            this.SS1_Sheet1.Columns.Get(6).Label = "토";
            this.SS1_Sheet1.Columns.Get(6).Width = 134F;
            this.SS1_Sheet1.DefaultStyleName = "Text454636282841801063038";
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.SS1_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.RowHeader.Visible = false;
            this.SS1_Sheet1.Rows.Get(0).Height = 142F;
            this.SS1_Sheet1.Rows.Get(1).Height = 142F;
            this.SS1_Sheet1.Rows.Get(2).Height = 142F;
            this.SS1_Sheet1.Rows.Get(3).Height = 142F;
            this.SS1_Sheet1.Rows.Get(4).Height = 142F;
            this.SS1_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // txtMemo
            // 
            this.txtMemo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMemo.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtMemo.Location = new System.Drawing.Point(0, 37);
            this.txtMemo.Multiline = true;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Size = new System.Drawing.Size(294, 585);
            this.txtMemo.TabIndex = 124;
            this.txtMemo.Text = "gfsgfsddgf";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(294, 37);
            this.panel1.TabIndex = 123;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(116, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 21);
            this.label1.TabIndex = 41;
            this.label1.Text = "참고사항";
            // 
            // frmDangjikAngio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1161, 700);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panheader4);
            this.Name = "frmDangjikAngio";
            this.Text = "frmDangjikAngio";
            this.panheader4.ResumeLayout(false);
            this.panheader4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panheader4;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cboYYMM;
        private System.Windows.Forms.Button btnSearch3;
        private System.Windows.Forms.Button btnSearch2;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox txtMemo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cboGubun;
        private System.Windows.Forms.Button btnExit;
        public FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
    }
}