namespace ComNurLibB
{
    partial class frmNrCodeSignYak
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("BorderEx434636281051621474363", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Static564636281051621474363", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Text877636281051621474363");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.EmptyBorder emptyBorder1 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder2 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder3 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRegi = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grbSabun = new System.Windows.Forms.GroupBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtSabun = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssSignYak = new FarPoint.Win.Spread.FpSpread();
            this.ssSignYak_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.lblBottom2 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grbSabun.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssSignYak)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSignYak_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnNew);
            this.panTitle.Controls.Add(this.btnSearch);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.btnDelete);
            this.panTitle.Controls.Add(this.btnRegi);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(393, 34);
            this.panTitle.TabIndex = 36;
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.BackColor = System.Drawing.Color.Transparent;
            this.btnNew.Location = new System.Drawing.Point(5, 1);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(72, 30);
            this.btnNew.TabIndex = 41;
            this.btnNew.Text = "신규";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(82, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 40;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(313, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 37;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.Location = new System.Drawing.Point(236, 1);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(72, 30);
            this.btnDelete.TabIndex = 38;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRegi
            // 
            this.btnRegi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegi.BackColor = System.Drawing.Color.Transparent;
            this.btnRegi.Location = new System.Drawing.Point(159, 1);
            this.btnRegi.Name = "btnRegi";
            this.btnRegi.Size = new System.Drawing.Size(72, 30);
            this.btnRegi.TabIndex = 39;
            this.btnRegi.Text = "등록";
            this.btnRegi.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grbSabun);
            this.panel1.Location = new System.Drawing.Point(3, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(385, 57);
            this.panel1.TabIndex = 37;
            // 
            // grbSabun
            // 
            this.grbSabun.Controls.Add(this.txtName);
            this.grbSabun.Controls.Add(this.txtSabun);
            this.grbSabun.Location = new System.Drawing.Point(5, 5);
            this.grbSabun.Name = "grbSabun";
            this.grbSabun.Size = new System.Drawing.Size(218, 43);
            this.grbSabun.TabIndex = 43;
            this.grbSabun.TabStop = false;
            this.grbSabun.Text = "(사번)";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(112, 16);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(100, 21);
            this.txtName.TabIndex = 44;
            // 
            // txtSabun
            // 
            this.txtSabun.Location = new System.Drawing.Point(6, 16);
            this.txtSabun.Name = "txtSabun";
            this.txtSabun.Size = new System.Drawing.Size(100, 21);
            this.txtSabun.TabIndex = 43;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ssSignYak);
            this.panel2.Location = new System.Drawing.Point(3, 95);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(283, 340);
            this.panel2.TabIndex = 38;
            // 
            // ssSignYak
            // 
            this.ssSignYak.AccessibleDescription = "ssSignYak, Sheet1, Row 0, Column 0, ";
            this.ssSignYak.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssSignYak.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssSignYak.Location = new System.Drawing.Point(0, 0);
            this.ssSignYak.Name = "ssSignYak";
            namedStyle1.Border = complexBorder1;
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle2.Border = complexBorder2;
            textCellType1.Static = true;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType2.MaxLength = 32000;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSignYak.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3});
            this.ssSignYak.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssSignYak_Sheet1});
            this.ssSignYak.Size = new System.Drawing.Size(283, 340);
            this.ssSignYak.TabIndex = 0;
            this.ssSignYak.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssSignYak.TextTipAppearance = tipAppearance1;
            this.ssSignYak.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssSignYak.VerticalScrollBarWidth = 10;
            // 
            // ssSignYak_Sheet1
            // 
            this.ssSignYak_Sheet1.Reset();
            this.ssSignYak_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssSignYak_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssSignYak_Sheet1.ColumnCount = 4;
            this.ssSignYak_Sheet1.RowCount = 1;
            this.ssSignYak_Sheet1.Cells.Get(0, 0).StyleName = "Text877636281051621474363";
            this.ssSignYak_Sheet1.Cells.Get(0, 1).StyleName = "Text877636281051621474363";
            this.ssSignYak_Sheet1.Cells.Get(0, 2).StyleName = "Text877636281051621474363";
            this.ssSignYak_Sheet1.Cells.Get(0, 3).StyleName = "Text877636281051621474363";
            this.ssSignYak_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssSignYak_Sheet1.ColumnFooter.Columns.Default.Width = 64F;
            this.ssSignYak_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "사번";
            this.ssSignYak_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "이름";
            this.ssSignYak_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "핸드폰번호";
            this.ssSignYak_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "rowid";
            this.ssSignYak_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssSignYak_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssSignYak_Sheet1.Columns.Default.Width = 64F;
            this.ssSignYak_Sheet1.Columns.Get(0).Border = emptyBorder1;
            this.ssSignYak_Sheet1.Columns.Get(0).CellType = textCellType3;
            this.ssSignYak_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSignYak_Sheet1.Columns.Get(0).Label = "사번";
            this.ssSignYak_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSignYak_Sheet1.Columns.Get(0).Width = 52F;
            this.ssSignYak_Sheet1.Columns.Get(1).Border = emptyBorder2;
            this.ssSignYak_Sheet1.Columns.Get(1).CellType = textCellType4;
            this.ssSignYak_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSignYak_Sheet1.Columns.Get(1).Label = "이름";
            this.ssSignYak_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSignYak_Sheet1.Columns.Get(1).Width = 63F;
            this.ssSignYak_Sheet1.Columns.Get(2).Border = emptyBorder3;
            this.ssSignYak_Sheet1.Columns.Get(2).CellType = textCellType5;
            this.ssSignYak_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSignYak_Sheet1.Columns.Get(2).Label = "핸드폰번호";
            this.ssSignYak_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSignYak_Sheet1.Columns.Get(2).Width = 117F;
            this.ssSignYak_Sheet1.Columns.Get(3).Label = "rowid";
            this.ssSignYak_Sheet1.Columns.Get(3).Visible = false;
            this.ssSignYak_Sheet1.DefaultStyleName = "Static564636281051621474363";
            this.ssSignYak_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssSignYak_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.ssSignYak_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssSignYak_Sheet1.Rows.Default.Height = 22F;
            this.ssSignYak_Sheet1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ssSignYak_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Black);
            this.ssSignYak_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // lblBottom2
            // 
            this.lblBottom2.AutoSize = true;
            this.lblBottom2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.lblBottom2.Location = new System.Drawing.Point(28, 449);
            this.lblBottom2.Name = "lblBottom2";
            this.lblBottom2.Size = new System.Drawing.Size(241, 12);
            this.lblBottom2.TabIndex = 48;
            this.lblBottom2.Text = "약제팀 간호부 공지사항 결재자 정보입니다.";
            this.lblBottom2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // frmNrCodeSignYak
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(393, 498);
            this.Controls.Add(this.lblBottom2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmNrCodeSignYak";
            this.Text = "약제과 간호부 공지 결재자 설정";
            this.panTitle.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.grbSabun.ResumeLayout(false);
            this.grbSabun.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssSignYak)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSignYak_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRegi;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox grbSabun;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtSabun;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblBottom2;
        private FarPoint.Win.Spread.FpSpread ssSignYak;
        private FarPoint.Win.Spread.SheetView ssSignYak_Sheet1;
    }
}