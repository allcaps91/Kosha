namespace ComLibB
{
    partial class frmDrHujin
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
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.cboDoct = new System.Windows.Forms.ComboBox();
            this.btnVeiw = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnReView = new System.Windows.Forms.Button();
            this.ssHujin = new FarPoint.Win.Spread.FpSpread();
            this.ssHujin_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssHujin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssHujin_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(379, 28);
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
            this.lblTitleSub0.Size = new System.Drawing.Size(57, 12);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "휴진일정";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.cboDoct);
            this.panTitle.Controls.Add(this.btnVeiw);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.btnReView);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(379, 34);
            this.panTitle.TabIndex = 77;
            // 
            // cboDoct
            // 
            this.cboDoct.FormattingEnabled = true;
            this.cboDoct.Location = new System.Drawing.Point(8, 6);
            this.cboDoct.Name = "cboDoct";
            this.cboDoct.Size = new System.Drawing.Size(106, 20);
            this.cboDoct.TabIndex = 79;
            // 
            // btnVeiw
            // 
            this.btnVeiw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVeiw.AutoSize = true;
            this.btnVeiw.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnVeiw.Location = new System.Drawing.Point(144, 1);
            this.btnVeiw.Name = "btnVeiw";
            this.btnVeiw.Size = new System.Drawing.Size(72, 30);
            this.btnVeiw.TabIndex = 78;
            this.btnVeiw.Text = "조회";
            this.btnVeiw.UseVisualStyleBackColor = true;
            this.btnVeiw.Click += new System.EventHandler(this.btnVeiw_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(300, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫  기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnReView
            // 
            this.btnReView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReView.AutoSize = true;
            this.btnReView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnReView.Location = new System.Drawing.Point(222, 1);
            this.btnReView.Name = "btnReView";
            this.btnReView.Size = new System.Drawing.Size(72, 30);
            this.btnReView.TabIndex = 77;
            this.btnReView.Text = "다시조회";
            this.btnReView.UseVisualStyleBackColor = true;
            this.btnReView.Click += new System.EventHandler(this.btnReView_Click);
            // 
            // ssHujin
            // 
            this.ssHujin.AccessibleDescription = "ssHujin, Sheet1, Row 0, Column 0, ";
            this.ssHujin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssHujin.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssHujin.Location = new System.Drawing.Point(0, 62);
            this.ssHujin.Name = "ssHujin";
            this.ssHujin.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssHujin_Sheet1});
            this.ssHujin.Size = new System.Drawing.Size(379, 463);
            this.ssHujin.TabIndex = 79;
            this.ssHujin.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssHujin_Sheet1
            // 
            this.ssHujin_Sheet1.Reset();
            this.ssHujin_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssHujin_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssHujin_Sheet1.ColumnCount = 4;
            this.ssHujin_Sheet1.RowCount = 1;
            this.ssHujin_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "진료과장";
            this.ssHujin_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "진료일자";
            this.ssHujin_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "오전";
            this.ssHujin_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "휴가";
            this.ssHujin_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssHujin_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssHujin_Sheet1.Columns.Get(0).Label = "진료과장";
            this.ssHujin_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssHujin_Sheet1.Columns.Get(0).Width = 90F;
            this.ssHujin_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssHujin_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssHujin_Sheet1.Columns.Get(1).Label = "진료일자";
            this.ssHujin_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssHujin_Sheet1.Columns.Get(1).Width = 90F;
            this.ssHujin_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssHujin_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssHujin_Sheet1.Columns.Get(2).Label = "오전";
            this.ssHujin_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssHujin_Sheet1.Columns.Get(2).Width = 65F;
            this.ssHujin_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssHujin_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssHujin_Sheet1.Columns.Get(3).Label = "휴가";
            this.ssHujin_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssHujin_Sheet1.Columns.Get(3).Width = 65F;
            this.ssHujin_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssHujin_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmDrHujin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(379, 525);
            this.Controls.Add(this.ssHujin);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmDrHujin";
            this.Text = "진료과장 휴진일정";
            this.Load += new System.EventHandler(this.frmDrHujin_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssHujin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssHujin_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.ComboBox cboDoct;
        private System.Windows.Forms.Button btnVeiw;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnReView;
        private FarPoint.Win.Spread.FpSpread ssHujin;
        private FarPoint.Win.Spread.SheetView ssHujin_Sheet1;
    }
}