namespace ComLibB
{
    partial class frmSlipView2
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssSlipView2 = new FarPoint.Win.Spread.FpSpread();
            this.ssSlipView2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssSlipView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSlipView2_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(708, 28);
            this.panTitleSub0.TabIndex = 80;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 9);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(57, 12);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "조회결과";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnPrint);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(708, 34);
            this.panTitle.TabIndex = 79;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.AutoSize = true;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrint.Location = new System.Drawing.Point(551, 1);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 16;
            this.btnPrint.Text = "인 쇄";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(629, 1);
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
            this.lblTitle.Size = new System.Drawing.Size(278, 16);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "구매과, 공급실 OCS전달 판넬 조회";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssSlipView2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(708, 443);
            this.panel1.TabIndex = 81;
            // 
            // ssSlipView2
            // 
            this.ssSlipView2.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.ssSlipView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssSlipView2.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssSlipView2.Location = new System.Drawing.Point(0, 0);
            this.ssSlipView2.Name = "ssSlipView2";
            this.ssSlipView2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssSlipView2_Sheet1});
            this.ssSlipView2.Size = new System.Drawing.Size(708, 443);
            this.ssSlipView2.TabIndex = 0;
            this.ssSlipView2.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssSlipView2.Change += new FarPoint.Win.Spread.ChangeEventHandler(this.ssSlipView2_Change);
            // 
            // ssSlipView2_Sheet1
            // 
            this.ssSlipView2_Sheet1.Reset();
            this.ssSlipView2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssSlipView2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssSlipView2_Sheet1.ColumnCount = 9;
            this.ssSlipView2_Sheet1.RowCount = 1;
            this.ssSlipView2_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "Slip명칭";
            this.ssSlipView2_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "오더코드";
            this.ssSlipView2_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "수가코드";
            this.ssSlipView2_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "오더명칭";
            this.ssSlipView2_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "구분";
            this.ssSlipView2_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "물품코드";
            this.ssSlipView2_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "물품명칭";
            this.ssSlipView2_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "ROWID";
            this.ssSlipView2_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "구분";
            this.ssSlipView2_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssSlipView2_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(0).Label = "Slip명칭";
            this.ssSlipView2_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssSlipView2_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(1).Label = "오더코드";
            this.ssSlipView2_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssSlipView2_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(2).Label = "수가코드";
            this.ssSlipView2_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssSlipView2_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(3).Label = "오더명칭";
            this.ssSlipView2_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(3).Width = 200F;
            this.ssSlipView2_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssSlipView2_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(4).Label = "구분";
            this.ssSlipView2_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(4).Width = 40F;
            this.ssSlipView2_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssSlipView2_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(5).Label = "물품코드";
            this.ssSlipView2_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.ssSlipView2_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(6).Label = "물품명칭";
            this.ssSlipView2_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(6).Width = 170F;
            this.ssSlipView2_Sheet1.Columns.Get(7).CellType = textCellType8;
            this.ssSlipView2_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(7).Label = "ROWID";
            this.ssSlipView2_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(8).CellType = textCellType9;
            this.ssSlipView2_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(8).Label = "구분";
            this.ssSlipView2_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSlipView2_Sheet1.Columns.Get(8).Width = 40F;
            this.ssSlipView2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssSlipView2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmSlipView2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(708, 505);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmSlipView2";
            this.Text = "구매과, 공급실 OCS전달 판넬 조회";
            this.Load += new System.EventHandler(this.frmSlipView2_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssSlipView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSlipView2_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssSlipView2;
        private FarPoint.Win.Spread.SheetView ssSlipView2_Sheet1;
    }
}