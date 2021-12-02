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
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("BorderEx434636281051621474363", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder3 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Static564636281051621474363", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder4 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))))), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle6 = new FarPoint.Win.Spread.NamedStyle("Text877636281051621474363");
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance2 = new FarPoint.Win.Spread.TipAppearance();
            FarPoint.Win.EmptyBorder emptyBorder4 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder5 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.EmptyBorder emptyBorder6 = new FarPoint.Win.EmptyBorder();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grbSabun = new System.Windows.Forms.GroupBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtSabun = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssSignYak = new FarPoint.Win.Spread.FpSpread();
            this.ssSignYak_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblBottom2 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grbSabun.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssSignYak)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSignYak_Sheet1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnNew);
            this.panTitle.Controls.Add(this.btnSearch);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.btnDelete);
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(630, 34);
            this.panTitle.TabIndex = 36;
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.BackColor = System.Drawing.Color.Transparent;
            this.btnNew.Location = new System.Drawing.Point(263, 1);
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
            this.btnSearch.Location = new System.Drawing.Point(335, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 40;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(551, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 37;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.Location = new System.Drawing.Point(479, 1);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(72, 30);
            this.btnDelete.TabIndex = 38;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Location = new System.Drawing.Point(407, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 39;
            this.btnSave.Text = "등록";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grbSabun);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(630, 57);
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
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 91);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(630, 444);
            this.panel2.TabIndex = 38;
            // 
            // ssSignYak
            // 
            this.ssSignYak.AccessibleDescription = "ssSignYak, Sheet1, Row 0, Column 0, ";
            this.ssSignYak.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssSignYak.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssSignYak.Location = new System.Drawing.Point(0, 0);
            this.ssSignYak.Name = "ssSignYak";
            namedStyle4.Border = complexBorder3;
            namedStyle4.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Parent = "DataAreaDefault";
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle5.Border = complexBorder4;
            textCellType6.Static = true;
            namedStyle5.CellType = textCellType6;
            namedStyle5.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Parent = "DataAreaDefault";
            namedStyle5.Renderer = textCellType6;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType7.MaxLength = 32000;
            namedStyle6.CellType = textCellType7;
            namedStyle6.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle6.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle6.Renderer = textCellType7;
            namedStyle6.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSignYak.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle4,
            namedStyle5,
            namedStyle6});
            this.ssSignYak.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssSignYak_Sheet1});
            this.ssSignYak.Size = new System.Drawing.Size(630, 444);
            this.ssSignYak.TabIndex = 0;
            this.ssSignYak.TabStripRatio = 0.6D;
            tipAppearance2.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance2.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssSignYak.TextTipAppearance = tipAppearance2;
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
            this.ssSignYak_Sheet1.Columns.Get(0).Border = emptyBorder4;
            this.ssSignYak_Sheet1.Columns.Get(0).CellType = textCellType8;
            this.ssSignYak_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSignYak_Sheet1.Columns.Get(0).Label = "사번";
            this.ssSignYak_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSignYak_Sheet1.Columns.Get(0).Width = 100F;
            this.ssSignYak_Sheet1.Columns.Get(1).Border = emptyBorder5;
            this.ssSignYak_Sheet1.Columns.Get(1).CellType = textCellType9;
            this.ssSignYak_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSignYak_Sheet1.Columns.Get(1).Label = "이름";
            this.ssSignYak_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSignYak_Sheet1.Columns.Get(1).Width = 100F;
            this.ssSignYak_Sheet1.Columns.Get(2).Border = emptyBorder6;
            this.ssSignYak_Sheet1.Columns.Get(2).CellType = textCellType10;
            this.ssSignYak_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSignYak_Sheet1.Columns.Get(2).Label = "핸드폰번호";
            this.ssSignYak_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSignYak_Sheet1.Columns.Get(2).Width = 140F;
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
            // panel3
            // 
            this.panel3.Controls.Add(this.lblBottom2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 535);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(630, 39);
            this.panel3.TabIndex = 49;
            // 
            // lblBottom2
            // 
            this.lblBottom2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.lblBottom2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBottom2.Location = new System.Drawing.Point(0, 0);
            this.lblBottom2.Name = "lblBottom2";
            this.lblBottom2.Size = new System.Drawing.Size(630, 39);
            this.lblBottom2.TabIndex = 49;
            this.lblBottom2.Text = "약제팀 간호부 공지사항 결재자 정보입니다.";
            this.lblBottom2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(3, 7);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(253, 16);
            this.lblTitle.TabIndex = 42;
            this.lblTitle.Text = "약제과 간호부 공지 결재자 설정";
            // 
            // frmNrCodeSignYak
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(630, 573);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmNrCodeSignYak";
            this.Text = "약제과 간호부 공지 결재자 설정";
            this.Load += new System.EventHandler(this.frmNrCodeSignYak_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.grbSabun.ResumeLayout(false);
            this.grbSabun.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssSignYak)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSignYak_Sheet1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox grbSabun;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtSabun;
        private System.Windows.Forms.Panel panel2;
        private FarPoint.Win.Spread.FpSpread ssSignYak;
        private FarPoint.Win.Spread.SheetView ssSignYak_Sheet1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblBottom2;
        private System.Windows.Forms.Label lblTitle;
    }
}