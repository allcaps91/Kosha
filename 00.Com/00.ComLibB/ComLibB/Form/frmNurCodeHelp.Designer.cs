namespace ComLibB
{
    partial class frmNurCodeHelp
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
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssNurCodeHelp = new FarPoint.Win.Spread.FpSpread();
            this.ssNurCodeHelp_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssNurCodeHelp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssNurCodeHelp_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(433, 28);
            this.panTitleSub0.TabIndex = 82;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 9);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(88, 12);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "간호활동 찾기";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(433, 34);
            this.panTitle.TabIndex = 81;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(353, 1);
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
            this.lblTitle.Size = new System.Drawing.Size(116, 16);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "간호활동 찾기";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssNurCodeHelp);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(433, 455);
            this.panel1.TabIndex = 83;
            // 
            // ssNurCodeHelp
            // 
            this.ssNurCodeHelp.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.ssNurCodeHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssNurCodeHelp.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssNurCodeHelp.Location = new System.Drawing.Point(0, 0);
            this.ssNurCodeHelp.Name = "ssNurCodeHelp";
            this.ssNurCodeHelp.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssNurCodeHelp_Sheet1});
            this.ssNurCodeHelp.Size = new System.Drawing.Size(433, 455);
            this.ssNurCodeHelp.TabIndex = 0;
            this.ssNurCodeHelp.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssNurCodeHelp.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssNurCodeHelp_CellDoubleClick);
            // 
            // ssNurCodeHelp_Sheet1
            // 
            this.ssNurCodeHelp_Sheet1.Reset();
            this.ssNurCodeHelp_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssNurCodeHelp_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssNurCodeHelp_Sheet1.ColumnCount = 3;
            this.ssNurCodeHelp_Sheet1.RowCount = 1;
            this.ssNurCodeHelp_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "코드";
            this.ssNurCodeHelp_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "간호활동분류";
            this.ssNurCodeHelp_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "간호활동";
            this.ssNurCodeHelp_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssNurCodeHelp_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssNurCodeHelp_Sheet1.Columns.Get(0).Label = "코드";
            this.ssNurCodeHelp_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssNurCodeHelp_Sheet1.Columns.Get(0).Width = 70F;
            this.ssNurCodeHelp_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssNurCodeHelp_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssNurCodeHelp_Sheet1.Columns.Get(1).Label = "간호활동분류";
            this.ssNurCodeHelp_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssNurCodeHelp_Sheet1.Columns.Get(1).Width = 140F;
            this.ssNurCodeHelp_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssNurCodeHelp_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssNurCodeHelp_Sheet1.Columns.Get(2).Label = "간호활동";
            this.ssNurCodeHelp_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssNurCodeHelp_Sheet1.Columns.Get(2).Width = 180F;
            this.ssNurCodeHelp_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssNurCodeHelp_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmNurCodeHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(433, 517);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmNurCodeHelp";
            this.Text = "간호활동 찾기";
            this.Load += new System.EventHandler(this.frmNurCodeHelp_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssNurCodeHelp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssNurCodeHelp_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssNurCodeHelp;
        private FarPoint.Win.Spread.SheetView ssNurCodeHelp_Sheet1;
    }
}