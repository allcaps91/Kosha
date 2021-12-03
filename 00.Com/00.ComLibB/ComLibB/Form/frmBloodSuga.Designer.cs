namespace ComLibB
{
    partial class frmBloodSuga
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType2 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssBlood = new FarPoint.Win.Spread.FpSpread();
            this.ssBlood_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssBloodCC = new FarPoint.Win.Spread.FpSpread();
            this.ssBloodCC_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.txtBlood = new System.Windows.Forms.TextBox();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssBlood)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssBlood_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssBloodCC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssBloodCC_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(647, 28);
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
            this.lblTitleSub0.Size = new System.Drawing.Size(114, 12);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "혈액은행 수가매핑";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(647, 34);
            this.panTitle.TabIndex = 77;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(568, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫  기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.AutoSize = true;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSave.Location = new System.Drawing.Point(490, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 77;
            this.btnSave.Text = "저 장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(3, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(317, 16);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "혈액수가매핑(혈액은행불출시 수가적용)";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssBlood);
            this.panel1.Location = new System.Drawing.Point(2, 64);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 322);
            this.panel1.TabIndex = 79;
            // 
            // ssBlood
            // 
            this.ssBlood.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.ssBlood.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssBlood.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssBlood.Location = new System.Drawing.Point(0, 0);
            this.ssBlood.Name = "ssBlood";
            this.ssBlood.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssBlood_Sheet1});
            this.ssBlood.Size = new System.Drawing.Size(284, 322);
            this.ssBlood.TabIndex = 0;
            this.ssBlood.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssBlood.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssBlood_CellClick);
            // 
            // ssBlood_Sheet1
            // 
            this.ssBlood_Sheet1.Reset();
            this.ssBlood_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssBlood_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssBlood_Sheet1.ColumnCount = 2;
            this.ssBlood_Sheet1.RowCount = 1;
            this.ssBlood_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "혈액코드";
            this.ssBlood_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "혈액종류";
            this.ssBlood_Sheet1.Columns.Get(0).CellType = textCellType7;
            this.ssBlood_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssBlood_Sheet1.Columns.Get(0).Label = "혈액코드";
            this.ssBlood_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssBlood_Sheet1.Columns.Get(0).Width = 70F;
            this.ssBlood_Sheet1.Columns.Get(1).CellType = textCellType8;
            this.ssBlood_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssBlood_Sheet1.Columns.Get(1).Label = "혈액종류";
            this.ssBlood_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssBlood_Sheet1.Columns.Get(1).Width = 175F;
            this.ssBlood_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssBlood_Sheet1.DefaultStyle.Locked = true;
            this.ssBlood_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssBlood_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ssBlood_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssBlood_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssBlood_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ssBloodCC);
            this.panel2.Location = new System.Drawing.Point(292, 112);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(349, 274);
            this.panel2.TabIndex = 80;
            // 
            // ssBloodCC
            // 
            this.ssBloodCC.AccessibleDescription = "ssBloodCC, Sheet1, Row 0, Column 0, ";
            this.ssBloodCC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssBloodCC.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssBloodCC.Location = new System.Drawing.Point(0, 0);
            this.ssBloodCC.Name = "ssBloodCC";
            this.ssBloodCC.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssBloodCC_Sheet1});
            this.ssBloodCC.Size = new System.Drawing.Size(349, 274);
            this.ssBloodCC.TabIndex = 0;
            this.ssBloodCC.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssBloodCC.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssBloodCC_CellDoubleClick);
            // 
            // ssBloodCC_Sheet1
            // 
            this.ssBloodCC_Sheet1.Reset();
            this.ssBloodCC_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssBloodCC_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssBloodCC_Sheet1.ColumnCount = 5;
            this.ssBloodCC_Sheet1.RowCount = 1;
            this.ssBloodCC_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "삭제";
            this.ssBloodCC_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "적용일자";
            this.ssBloodCC_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "400cc";
            this.ssBloodCC_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "320cc";
            this.ssBloodCC_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "rowid";
            this.ssBloodCC_Sheet1.Columns.Get(0).CellType = checkBoxCellType2;
            this.ssBloodCC_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssBloodCC_Sheet1.Columns.Get(0).Label = "삭제";
            this.ssBloodCC_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssBloodCC_Sheet1.Columns.Get(0).Width = 39F;
            this.ssBloodCC_Sheet1.Columns.Get(1).CellType = textCellType9;
            this.ssBloodCC_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssBloodCC_Sheet1.Columns.Get(1).Label = "적용일자";
            this.ssBloodCC_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssBloodCC_Sheet1.Columns.Get(1).Width = 100F;
            this.ssBloodCC_Sheet1.Columns.Get(2).CellType = textCellType10;
            this.ssBloodCC_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssBloodCC_Sheet1.Columns.Get(2).Label = "400cc";
            this.ssBloodCC_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssBloodCC_Sheet1.Columns.Get(2).Width = 85F;
            this.ssBloodCC_Sheet1.Columns.Get(3).CellType = textCellType11;
            this.ssBloodCC_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssBloodCC_Sheet1.Columns.Get(3).Label = "320cc";
            this.ssBloodCC_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssBloodCC_Sheet1.Columns.Get(3).Width = 85F;
            this.ssBloodCC_Sheet1.Columns.Get(4).CellType = textCellType12;
            this.ssBloodCC_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssBloodCC_Sheet1.Columns.Get(4).Label = "rowid";
            this.ssBloodCC_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssBloodCC_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssBloodCC_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // txtBlood
            // 
            this.txtBlood.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtBlood.ForeColor = System.Drawing.Color.Blue;
            this.txtBlood.Location = new System.Drawing.Point(292, 67);
            this.txtBlood.Name = "txtBlood";
            this.txtBlood.Size = new System.Drawing.Size(349, 32);
            this.txtBlood.TabIndex = 81;
            this.txtBlood.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // frmBloodSuga
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(647, 389);
            this.Controls.Add(this.txtBlood);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmBloodSuga";
            this.Text = "혈액은행 수가매핑";
            this.Load += new System.EventHandler(this.frmBloodSuga_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssBlood)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssBlood_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssBloodCC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssBloodCC_Sheet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtBlood;
        private FarPoint.Win.Spread.FpSpread ssBlood;
        private FarPoint.Win.Spread.SheetView ssBlood_Sheet1;
        private FarPoint.Win.Spread.FpSpread ssBloodCC;
        private FarPoint.Win.Spread.SheetView ssBloodCC_Sheet1;
    }
}