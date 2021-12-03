namespace ComLibB
{
    partial class frmBCode
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
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.ssGichoCode = new FarPoint.Win.Spread.FpSpread();
            this.ssGichoCode_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblName = new System.Windows.Forms.Label();
            this.cboCode = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssGichoCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssGichoCode_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(778, 34);
            this.panTitle.TabIndex = 82;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(701, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫 기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(9, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(116, 16);
            this.lblTitle.TabIndex = 15;
            this.lblTitle.Text = "기초코드 관리";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(486, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 26);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.Text = "조 회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Location = new System.Drawing.Point(558, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 26);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "저 장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(702, 1);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(72, 26);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "삭 제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // ssGichoCode
            // 
            this.ssGichoCode.AccessibleDescription = "ssGichoCode, Sheet1, Row 0, Column 0, ";
            this.ssGichoCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssGichoCode.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssGichoCode.Location = new System.Drawing.Point(0, 102);
            this.ssGichoCode.Name = "ssGichoCode";
            this.ssGichoCode.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssGichoCode_Sheet1});
            this.ssGichoCode.Size = new System.Drawing.Size(778, 334);
            this.ssGichoCode.TabIndex = 2;
            // 
            // ssGichoCode_Sheet1
            // 
            this.ssGichoCode_Sheet1.Reset();
            this.ssGichoCode_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssGichoCode_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssGichoCode_Sheet1.ColumnCount = 18;
            this.ssGichoCode_Sheet1.RowCount = 1;
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "CODE";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "NAME";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "JDATE";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "DELDATE";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "ENTSABUN";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "ENTDATE";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "SORT";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "PART";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "CNT";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "GUBUN2";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "GUBUN3";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "GUBUN4";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "GUBUN5";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "GUNUM1";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "GUNUM2";
            this.ssGichoCode_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "GUNUM3";
            this.ssGichoCode_Sheet1.ColumnHeader.Rows.Get(0).Height = 24F;
            this.ssGichoCode_Sheet1.Columns.Get(0).CellType = checkBoxCellType1;
            this.ssGichoCode_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(0).Width = 30F;
            this.ssGichoCode_Sheet1.Columns.Get(1).CellType = textCellType1;
            this.ssGichoCode_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(1).Label = "CODE";
            this.ssGichoCode_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(1).Width = 150F;
            this.ssGichoCode_Sheet1.Columns.Get(2).CellType = textCellType2;
            this.ssGichoCode_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(2).Label = "NAME";
            this.ssGichoCode_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(2).Width = 250F;
            this.ssGichoCode_Sheet1.Columns.Get(3).CellType = textCellType3;
            this.ssGichoCode_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(3).Label = "JDATE";
            this.ssGichoCode_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssGichoCode_Sheet1.Columns.Get(3).Width = 250F;
            this.ssGichoCode_Sheet1.Columns.Get(4).CellType = textCellType4;
            this.ssGichoCode_Sheet1.Columns.Get(4).Label = "DELDATE";
            this.ssGichoCode_Sheet1.Columns.Get(4).Width = 62F;
            this.ssGichoCode_Sheet1.Columns.Get(5).CellType = textCellType5;
            this.ssGichoCode_Sheet1.Columns.Get(5).Label = "ENTSABUN";
            this.ssGichoCode_Sheet1.Columns.Get(5).Width = 73F;
            this.ssGichoCode_Sheet1.Columns.Get(6).CellType = textCellType6;
            this.ssGichoCode_Sheet1.Columns.Get(6).Label = "ENTDATE";
            this.ssGichoCode_Sheet1.Columns.Get(7).CellType = textCellType7;
            this.ssGichoCode_Sheet1.Columns.Get(7).Label = "SORT";
            this.ssGichoCode_Sheet1.Columns.Get(8).CellType = textCellType8;
            this.ssGichoCode_Sheet1.Columns.Get(8).Label = "PART";
            this.ssGichoCode_Sheet1.Columns.Get(9).CellType = textCellType9;
            this.ssGichoCode_Sheet1.Columns.Get(9).Label = "CNT";
            this.ssGichoCode_Sheet1.Columns.Get(10).CellType = textCellType10;
            this.ssGichoCode_Sheet1.Columns.Get(10).Label = "GUBUN2";
            this.ssGichoCode_Sheet1.Columns.Get(11).CellType = textCellType11;
            this.ssGichoCode_Sheet1.Columns.Get(11).Label = "GUBUN3";
            this.ssGichoCode_Sheet1.Columns.Get(12).CellType = textCellType12;
            this.ssGichoCode_Sheet1.Columns.Get(12).Label = "GUBUN4";
            this.ssGichoCode_Sheet1.Columns.Get(13).CellType = textCellType13;
            this.ssGichoCode_Sheet1.Columns.Get(13).Label = "GUBUN5";
            this.ssGichoCode_Sheet1.Columns.Get(14).CellType = textCellType14;
            this.ssGichoCode_Sheet1.Columns.Get(14).Label = "GUNUM1";
            this.ssGichoCode_Sheet1.Columns.Get(15).CellType = textCellType15;
            this.ssGichoCode_Sheet1.Columns.Get(15).Label = "GUNUM2";
            this.ssGichoCode_Sheet1.Columns.Get(16).CellType = textCellType16;
            this.ssGichoCode_Sheet1.Columns.Get(16).Label = "GUNUM3";
            this.ssGichoCode_Sheet1.Columns.Get(17).CellType = textCellType17;
            this.ssGichoCode_Sheet1.Columns.Get(17).Visible = false;
            this.ssGichoCode_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssGichoCode_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssGichoCode_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblName);
            this.panel1.Controls.Add(this.cboCode);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(778, 40);
            this.panel1.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblName.Location = new System.Drawing.Point(272, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(250, 23);
            this.lblName.TabIndex = 2;
            this.lblName.Text = "label1";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboCode
            // 
            this.cboCode.FormattingEnabled = true;
            this.cboCode.Location = new System.Drawing.Point(11, 10);
            this.cboCode.Name = "cboCode";
            this.cboCode.Size = new System.Drawing.Size(257, 20);
            this.cboCode.TabIndex = 0;
            this.cboCode.SelectedIndexChanged += new System.EventHandler(this.cboCode_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 34);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(778, 28);
            this.panel2.TabIndex = 83;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAdd.ForeColor = System.Drawing.Color.Black;
            this.btnAdd.Location = new System.Drawing.Point(630, 1);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 26);
            this.btnAdd.TabIndex = 16;
            this.btnAdd.Text = "추 가";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // frmBCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(778, 436);
            this.ControlBox = false;
            this.Controls.Add(this.ssGichoCode);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panTitle);
            this.Name = "frmBCode";
            this.Text = "기초코드 관리";
            this.Load += new System.EventHandler(this.frmBCode_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssGichoCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssGichoCode_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private FarPoint.Win.Spread.FpSpread ssGichoCode;
        private FarPoint.Win.Spread.SheetView ssGichoCode_Sheet1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cboCode;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblName;
    }
}