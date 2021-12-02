namespace ComLibB
{
    partial class frmJemsuSuga2
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblJob = new System.Windows.Forms.Label();
            this.btnCancle = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.cboBun = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ssSuga = new FarPoint.Win.Spread.FpSpread();
            this.ssSuga_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssSuga)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSuga_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.btnExit);
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(558, 34);
            this.panTitleSub0.TabIndex = 13;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(482, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 5);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(242, 21);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "분기별신고 약가 수가 일괄 변경";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.lblJob);
            this.panel3.Controls.Add(this.btnCancle);
            this.panel3.Controls.Add(this.btnStart);
            this.panel3.Controls.Add(this.cboBun);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 34);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(558, 34);
            this.panel3.TabIndex = 26;
            // 
            // lblJob
            // 
            this.lblJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblJob.Location = new System.Drawing.Point(412, 6);
            this.lblJob.Name = "lblJob";
            this.lblJob.Size = new System.Drawing.Size(143, 23);
            this.lblJob.TabIndex = 26;
            this.lblJob.Text = "Job";
            this.lblJob.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancle
            // 
            this.btnCancle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancle.BackColor = System.Drawing.Color.Transparent;
            this.btnCancle.Location = new System.Drawing.Point(333, 2);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(72, 30);
            this.btnCancle.TabIndex = 4;
            this.btnCancle.Text = "작업취소";
            this.btnCancle.UseVisualStyleBackColor = false;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.BackColor = System.Drawing.Color.Transparent;
            this.btnStart.Location = new System.Drawing.Point(232, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 30);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "수가변경 시작";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // cboBun
            // 
            this.cboBun.FormattingEnabled = true;
            this.cboBun.Location = new System.Drawing.Point(102, 7);
            this.cboBun.Name = "cboBun";
            this.cboBun.Size = new System.Drawing.Size(121, 20);
            this.cboBun.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 12);
            this.label1.TabIndex = 24;
            this.label1.Text = "약제 신고 분기 : ";
            // 
            // ssSuga
            // 
            this.ssSuga.AccessibleDescription = "";
            this.ssSuga.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssSuga.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssSuga.Location = new System.Drawing.Point(0, 68);
            this.ssSuga.Name = "ssSuga";
            this.ssSuga.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssSuga_Sheet1});
            this.ssSuga.Size = new System.Drawing.Size(558, 355);
            this.ssSuga.TabIndex = 27;
            // 
            // ssSuga_Sheet1
            // 
            this.ssSuga_Sheet1.Reset();
            this.ssSuga_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssSuga_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssSuga_Sheet1.ColumnCount = 5;
            this.ssSuga_Sheet1.RowCount = 1;
            this.ssSuga_Sheet1.Cells.Get(0, 0).CellType = textCellType1;
            this.ssSuga_Sheet1.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga_Sheet1.Cells.Get(0, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga_Sheet1.Cells.Get(0, 1).CellType = textCellType2;
            this.ssSuga_Sheet1.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga_Sheet1.Cells.Get(0, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga_Sheet1.Cells.Get(0, 2).CellType = textCellType3;
            this.ssSuga_Sheet1.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga_Sheet1.Cells.Get(0, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga_Sheet1.Cells.Get(0, 3).CellType = textCellType4;
            this.ssSuga_Sheet1.Cells.Get(0, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga_Sheet1.Cells.Get(0, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga_Sheet1.Cells.Get(0, 4).CellType = textCellType5;
            this.ssSuga_Sheet1.Cells.Get(0, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga_Sheet1.Cells.Get(0, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "표준수가";
            this.ssSuga_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "상대가치점수";
            this.ssSuga_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "보험수가";
            this.ssSuga_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "변경건수";
            this.ssSuga_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "비고";
            this.ssSuga_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssSuga_Sheet1.Columns.Get(0).CellType = textCellType6;
            this.ssSuga_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga_Sheet1.Columns.Get(0).Label = "표준수가";
            this.ssSuga_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga_Sheet1.Columns.Get(0).Width = 100F;
            this.ssSuga_Sheet1.Columns.Get(1).CellType = textCellType7;
            this.ssSuga_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga_Sheet1.Columns.Get(1).Label = "상대가치점수";
            this.ssSuga_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga_Sheet1.Columns.Get(1).Width = 100F;
            this.ssSuga_Sheet1.Columns.Get(2).CellType = textCellType8;
            this.ssSuga_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga_Sheet1.Columns.Get(2).Label = "보험수가";
            this.ssSuga_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga_Sheet1.Columns.Get(2).Width = 100F;
            this.ssSuga_Sheet1.Columns.Get(3).CellType = textCellType9;
            this.ssSuga_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga_Sheet1.Columns.Get(3).Label = "변경건수";
            this.ssSuga_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga_Sheet1.Columns.Get(3).Width = 100F;
            this.ssSuga_Sheet1.Columns.Get(4).CellType = textCellType10;
            this.ssSuga_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga_Sheet1.Columns.Get(4).Label = "비고";
            this.ssSuga_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga_Sheet1.Columns.Get(4).Width = 100F;
            this.ssSuga_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssSuga_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmJemsuSuga2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 423);
            this.Controls.Add(this.ssSuga);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panTitleSub0);
            this.Name = "frmJemsuSuga2";
            this.Text = "분기별신고 약가 수가 일괄변경";
            this.Load += new System.EventHandler(this.frmJemsuSuga2_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssSuga)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSuga_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboBun;
        private System.Windows.Forms.Label lblJob;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Button btnStart;
        private FarPoint.Win.Spread.FpSpread ssSuga;
        private FarPoint.Win.Spread.SheetView ssSuga_Sheet1;
    }
}