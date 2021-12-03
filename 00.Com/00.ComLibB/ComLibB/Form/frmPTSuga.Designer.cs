namespace ComLibB
{
    partial class frmPTSuga
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
            this.txtFind = new System.Windows.Forms.Panel();
            this.txtData = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnView = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ssPTSuga = new FarPoint.Win.Spread.FpSpread();
            this.ssPTSuga_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0.SuspendLayout();
            this.txtFind.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssPTSuga)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssPTSuga_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(397, 28);
            this.panTitleSub0.TabIndex = 84;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 9);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(184, 12);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "재활치료수가 최초발병일 관리";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtFind
            // 
            this.txtFind.BackColor = System.Drawing.Color.White;
            this.txtFind.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtFind.Controls.Add(this.txtData);
            this.txtFind.Controls.Add(this.label1);
            this.txtFind.Controls.Add(this.btnView);
            this.txtFind.Controls.Add(this.btnExit);
            this.txtFind.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtFind.ForeColor = System.Drawing.Color.White;
            this.txtFind.Location = new System.Drawing.Point(0, 0);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new System.Drawing.Size(397, 34);
            this.txtFind.TabIndex = 83;
            // 
            // txtData
            // 
            this.txtData.Location = new System.Drawing.Point(132, 6);
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(100, 21);
            this.txtData.TabIndex = 1;
            this.txtData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtData_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(8, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "성명 or 등록번호";
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.AutoSize = true;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnView.Location = new System.Drawing.Point(240, 1);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(72, 30);
            this.btnView.TabIndex = 85;
            this.btnView.Text = "조 회";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(318, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 83;
            this.btnExit.Text = "닫 기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ssPTSuga);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(397, 369);
            this.panel1.TabIndex = 85;
            // 
            // ssPTSuga
            // 
            this.ssPTSuga.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.ssPTSuga.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssPTSuga.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssPTSuga.Location = new System.Drawing.Point(0, 0);
            this.ssPTSuga.Name = "ssPTSuga";
            this.ssPTSuga.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssPTSuga_Sheet1});
            this.ssPTSuga.Size = new System.Drawing.Size(397, 369);
            this.ssPTSuga.TabIndex = 0;
            this.ssPTSuga.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssPTSuga_Sheet1
            // 
            this.ssPTSuga_Sheet1.Reset();
            this.ssPTSuga_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssPTSuga_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssPTSuga_Sheet1.ColumnCount = 4;
            this.ssPTSuga_Sheet1.RowCount = 1;
            this.ssPTSuga_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.ssPTSuga_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성명";
            this.ssPTSuga_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "수가코드";
            this.ssPTSuga_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "최초발병일";
            this.ssPTSuga_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssPTSuga_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPTSuga_Sheet1.Columns.Get(0).Label = "등록번호";
            this.ssPTSuga_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPTSuga_Sheet1.Columns.Get(0).Width = 80F;
            this.ssPTSuga_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssPTSuga_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPTSuga_Sheet1.Columns.Get(1).Label = "성명";
            this.ssPTSuga_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPTSuga_Sheet1.Columns.Get(1).Width = 80F;
            this.ssPTSuga_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssPTSuga_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPTSuga_Sheet1.Columns.Get(2).Label = "수가코드";
            this.ssPTSuga_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPTSuga_Sheet1.Columns.Get(2).Width = 80F;
            this.ssPTSuga_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssPTSuga_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssPTSuga_Sheet1.Columns.Get(3).Label = "최초발병일";
            this.ssPTSuga_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssPTSuga_Sheet1.Columns.Get(3).Width = 110F;
            this.ssPTSuga_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssPTSuga_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmPTSuga
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(397, 431);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.txtFind);
            this.Name = "frmPTSuga";
            this.Text = "재활치료수가 최초발병일 관리";
            this.Load += new System.EventHandler(this.frmPTSuga_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.txtFind.ResumeLayout(false);
            this.txtFind.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssPTSuga)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssPTSuga_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel txtFind;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel1;
        private FarPoint.Win.Spread.FpSpread ssPTSuga;
        private FarPoint.Win.Spread.SheetView ssPTSuga_Sheet1;
        private System.Windows.Forms.TextBox txtData;
        private System.Windows.Forms.Label label1;
    }
}