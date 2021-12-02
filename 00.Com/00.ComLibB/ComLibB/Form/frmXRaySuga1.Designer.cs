namespace ComLibB
{
    partial class frmXRaySuga1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.dtpSuga = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ssSuga1 = new FarPoint.Win.Spread.FpSpread();
            this.ssSuga1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ssSuga2 = new FarPoint.Win.Spread.FpSpread();
            this.ssSuga2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssSuga1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSuga1_Sheet1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssSuga2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSuga2_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.Controls.Add(this.panel1);
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(671, 28);
            this.panTitleSub0.TabIndex = 84;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(0, 31);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 85;
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
            this.lblTitleSub0.Text = "수가정보";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.dtpSuga);
            this.panTitle.Controls.Add(this.label1);
            this.panTitle.Controls.Add(this.btnCancel);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.btnStart);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(671, 34);
            this.panTitle.TabIndex = 83;
            // 
            // dtpSuga
            // 
            this.dtpSuga.CustomFormat = "yyyy-MM-dd";
            this.dtpSuga.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSuga.Location = new System.Drawing.Point(321, 6);
            this.dtpSuga.Name = "dtpSuga";
            this.dtpSuga.Size = new System.Drawing.Size(100, 21);
            this.dtpSuga.TabIndex = 86;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(250, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 85;
            this.label1.Text = "수가적용일";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSize = true;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(514, 1);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 30);
            this.btnCancel.TabIndex = 85;
            this.btnCancel.Text = "작업취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
            this.lblTitle.Text = "수가 일괄변경";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(592, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 83;
            this.btnExit.Text = "작업종료";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.AutoSize = true;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStart.Location = new System.Drawing.Point(436, 1);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(72, 30);
            this.btnStart.TabIndex = 84;
            this.btnStart.Text = "수가변경";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ssSuga1);
            this.panel2.Location = new System.Drawing.Point(1, 63);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(747, 144);
            this.panel2.TabIndex = 85;
            // 
            // ssSuga1
            // 
            this.ssSuga1.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.ssSuga1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssSuga1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssSuga1.Location = new System.Drawing.Point(0, 0);
            this.ssSuga1.Name = "ssSuga1";
            this.ssSuga1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssSuga1_Sheet1});
            this.ssSuga1.Size = new System.Drawing.Size(747, 144);
            this.ssSuga1.TabIndex = 0;
            this.ssSuga1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssSuga1_Sheet1
            // 
            this.ssSuga1_Sheet1.Reset();
            this.ssSuga1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssSuga1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssSuga1_Sheet1.ColumnCount = 4;
            this.ssSuga1_Sheet1.RowCount = 1;
            this.ssSuga1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "표준코드";
            this.ssSuga1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "변경수가";
            this.ssSuga1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "표준단위";
            this.ssSuga1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "표준코드명";
            this.ssSuga1_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssSuga1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga1_Sheet1.Columns.Get(0).Label = "표준코드";
            this.ssSuga1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga1_Sheet1.Columns.Get(0).Width = 70F;
            this.ssSuga1_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssSuga1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga1_Sheet1.Columns.Get(1).Label = "변경수가";
            this.ssSuga1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga1_Sheet1.Columns.Get(1).Width = 70F;
            this.ssSuga1_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssSuga1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga1_Sheet1.Columns.Get(2).Label = "표준단위";
            this.ssSuga1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga1_Sheet1.Columns.Get(2).Width = 70F;
            this.ssSuga1_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssSuga1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga1_Sheet1.Columns.Get(3).Label = "표준코드명";
            this.ssSuga1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga1_Sheet1.Columns.Get(3).Width = 400F;
            this.ssSuga1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssSuga1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(4, 204);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(495, 100);
            this.groupBox1.TabIndex = 86;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "작업 설명";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(404, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "3.일반수가,자보수가는 기준에 의해 자동 변경 됩니다. 4.F항=1인것은 제외";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(257, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "변경을 됩니다. (변경수가가 0원인것은 제외됨)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(438, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "2.\"수가변경시작(O)\"를 클릭하면 수가코드중 표준코드가 동일한 것은 변경수가로";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(363, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "1.수가변경일을 입력후, 위의 Sheet에 변경하실 변경수가를 입력후";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.ssSuga2);
            this.panel3.Location = new System.Drawing.Point(1, 302);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(747, 186);
            this.panel3.TabIndex = 87;
            // 
            // ssSuga2
            // 
            this.ssSuga2.AccessibleDescription = "fpSpread2, Sheet1, Row 0, Column 0, ";
            this.ssSuga2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssSuga2.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssSuga2.Location = new System.Drawing.Point(0, 0);
            this.ssSuga2.Name = "ssSuga2";
            this.ssSuga2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssSuga2_Sheet1});
            this.ssSuga2.Size = new System.Drawing.Size(747, 186);
            this.ssSuga2.TabIndex = 0;
            this.ssSuga2.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssSuga2_Sheet1
            // 
            this.ssSuga2_Sheet1.Reset();
            this.ssSuga2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssSuga2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssSuga2_Sheet1.ColumnCount = 5;
            this.ssSuga2_Sheet1.RowCount = 1;
            this.ssSuga2_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "표준코드";
            this.ssSuga2_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "보험수가";
            this.ssSuga2_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "변경건수";
            this.ssSuga2_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "표준코드명";
            this.ssSuga2_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "비고";
            this.ssSuga2_Sheet1.Columns.Get(0).CellType = textCellType5;
            this.ssSuga2_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga2_Sheet1.Columns.Get(0).Label = "표준코드";
            this.ssSuga2_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga2_Sheet1.Columns.Get(0).Width = 80F;
            this.ssSuga2_Sheet1.Columns.Get(1).CellType = textCellType6;
            this.ssSuga2_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga2_Sheet1.Columns.Get(1).Label = "보험수가";
            this.ssSuga2_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga2_Sheet1.Columns.Get(1).Width = 70F;
            this.ssSuga2_Sheet1.Columns.Get(2).CellType = textCellType7;
            this.ssSuga2_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga2_Sheet1.Columns.Get(2).Label = "변경건수";
            this.ssSuga2_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga2_Sheet1.Columns.Get(2).Width = 70F;
            this.ssSuga2_Sheet1.Columns.Get(3).CellType = textCellType8;
            this.ssSuga2_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga2_Sheet1.Columns.Get(3).Label = "표준코드명";
            this.ssSuga2_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga2_Sheet1.Columns.Get(3).Width = 300F;
            this.ssSuga2_Sheet1.Columns.Get(4).CellType = textCellType9;
            this.ssSuga2_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssSuga2_Sheet1.Columns.Get(4).Label = "비고";
            this.ssSuga2_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssSuga2_Sheet1.Columns.Get(4).Width = 90F;
            this.ssSuga2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssSuga2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmXRaySuga1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(671, 490);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmXRaySuga1";
            this.Text = "방사선 재료대 수가 일괄변경";
            this.Load += new System.EventHandler(this.frmXRaySuga1_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssSuga1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSuga1_Sheet1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssSuga2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssSuga2_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.DateTimePicker dtpSuga;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Panel panel2;
        private FarPoint.Win.Spread.FpSpread ssSuga1;
        private FarPoint.Win.Spread.SheetView ssSuga1_Sheet1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private FarPoint.Win.Spread.FpSpread ssSuga2;
        private FarPoint.Win.Spread.SheetView ssSuga2_Sheet1;
    }
}