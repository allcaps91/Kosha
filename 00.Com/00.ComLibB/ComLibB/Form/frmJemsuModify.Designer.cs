namespace ComLibB
{
    partial class frmJemsuModify
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.chkZero = new System.Windows.Forms.CheckBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssJemsuModify = new FarPoint.Win.Spread.FpSpread();
            this.ssJemsuModify_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssJemsuModify)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssJemsuModify_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.Controls.Add(this.label1);
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(750, 28);
            this.panTitleSub0.TabIndex = 78;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 9);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(145, 12);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "상대가치 점수 일괄수정";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblCode);
            this.panTitle.Controls.Add(this.chkZero);
            this.panTitle.Controls.Add(this.txtCode);
            this.panTitle.Controls.Add(this.btnSearch);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(750, 34);
            this.panTitle.TabIndex = 77;
            // 
            // txtCode
            // 
            this.txtCode.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtCode.Location = new System.Drawing.Point(486, 6);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(100, 22);
            this.txtCode.TabIndex = 77;
            this.txtCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCode_KeyPress);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.AutoSize = true;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Location = new System.Drawing.Point(593, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 78;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(671, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫  기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(3, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(190, 16);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "상대가치 점수 일괄수정";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkZero
            // 
            this.chkZero.AutoSize = true;
            this.chkZero.ForeColor = System.Drawing.Color.Blue;
            this.chkZero.Location = new System.Drawing.Point(256, 9);
            this.chkZero.Name = "chkZero";
            this.chkZero.Size = new System.Drawing.Size(121, 16);
            this.chkZero.TabIndex = 79;
            this.chkZero.Text = "상대가치 ZERO만";
            this.chkZero.UseVisualStyleBackColor = true;
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.ForeColor = System.Drawing.Color.Black;
            this.lblCode.Location = new System.Drawing.Point(388, 11);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(87, 12);
            this.lblCode.TabIndex = 79;
            this.lblCode.Text = "찾으실 코드는?";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(415, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(261, 12);
            this.label1.TabIndex = 79;
            this.label1.Text = "변경을 하면 자동으로 DB에 저장이 됩니다.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssJemsuModify);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(750, 490);
            this.panel1.TabIndex = 79;
            // 
            // ssJemsuModify
            // 
            this.ssJemsuModify.AccessibleDescription = "ssJemsuModify, Sheet1, Row 0, Column 0, ";
            this.ssJemsuModify.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssJemsuModify.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssJemsuModify.Location = new System.Drawing.Point(0, 0);
            this.ssJemsuModify.Name = "ssJemsuModify";
            this.ssJemsuModify.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssJemsuModify_Sheet1});
            this.ssJemsuModify.Size = new System.Drawing.Size(750, 490);
            this.ssJemsuModify.TabIndex = 0;
            this.ssJemsuModify.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssJemsuModify.Change += new FarPoint.Win.Spread.ChangeEventHandler(this.ssJemsuModify_Change);
            // 
            // ssJemsuModify_Sheet1
            // 
            this.ssJemsuModify_Sheet1.Reset();
            this.ssJemsuModify_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssJemsuModify_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssJemsuModify_Sheet1.ColumnCount = 7;
            this.ssJemsuModify_Sheet1.RowCount = 1;
            this.ssJemsuModify_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "표준코드";
            this.ssJemsuModify_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "적용일자";
            this.ssJemsuModify_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "상대가치  점수";
            this.ssJemsuModify_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "보험수가";
            this.ssJemsuModify_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "분류명칭";
            this.ssJemsuModify_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "ROWID";
            this.ssJemsuModify_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "수정여부";
            this.ssJemsuModify_Sheet1.ColumnHeader.Rows.Get(0).Height = 32F;
            this.ssJemsuModify_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssJemsuModify_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssJemsuModify_Sheet1.Columns.Get(0).Label = "표준코드";
            this.ssJemsuModify_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssJemsuModify_Sheet1.Columns.Get(0).Width = 70F;
            this.ssJemsuModify_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssJemsuModify_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssJemsuModify_Sheet1.Columns.Get(1).Label = "적용일자";
            this.ssJemsuModify_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssJemsuModify_Sheet1.Columns.Get(1).Width = 70F;
            this.ssJemsuModify_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssJemsuModify_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssJemsuModify_Sheet1.Columns.Get(2).Label = "상대가치  점수";
            this.ssJemsuModify_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssJemsuModify_Sheet1.Columns.Get(2).Width = 70F;
            this.ssJemsuModify_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssJemsuModify_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssJemsuModify_Sheet1.Columns.Get(3).Label = "보험수가";
            this.ssJemsuModify_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssJemsuModify_Sheet1.Columns.Get(3).Width = 70F;
            textCellType5.Multiline = true;
            textCellType5.WordWrap = true;
            this.ssJemsuModify_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssJemsuModify_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssJemsuModify_Sheet1.Columns.Get(4).Label = "분류명칭";
            this.ssJemsuModify_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssJemsuModify_Sheet1.Columns.Get(4).Width = 430F;
            this.ssJemsuModify_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssJemsuModify_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssJemsuModify_Sheet1.Columns.Get(5).Label = "ROWID";
            this.ssJemsuModify_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssJemsuModify_Sheet1.Columns.Get(5).Width = 70F;
            this.ssJemsuModify_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.ssJemsuModify_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssJemsuModify_Sheet1.Columns.Get(6).Label = "수정여부";
            this.ssJemsuModify_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssJemsuModify_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssJemsuModify_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmJemsuModify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(750, 552);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmJemsuModify";
            this.Text = "상대가치 점수 일괄수정";
            this.Load += new System.EventHandler(this.frmJemsuModify_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssJemsuModify)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssJemsuModify_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.CheckBox chkZero;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssJemsuModify;
        private FarPoint.Win.Spread.SheetView ssJemsuModify_Sheet1;
    }
}