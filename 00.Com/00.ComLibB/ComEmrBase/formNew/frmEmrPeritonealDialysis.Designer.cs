namespace ComEmrBase
{
    partial class frmEmrPeritonealDialysis
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
            this.panTitle = new System.Windows.Forms.Panel();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFrDate = new System.Windows.Forms.DateTimePicker();
            this.btnSearchAll = new System.Windows.Forms.Button();
            this.chkAsc = new System.Windows.Forms.CheckBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.ss1 = new FarPoint.Win.Spread.FpSpread();
            this.ss1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.dtpEndDate);
            this.panTitle.Controls.Add(this.dtpFrDate);
            this.panTitle.Controls.Add(this.btnSearchAll);
            this.panTitle.Controls.Add(this.chkAsc);
            this.panTitle.Controls.Add(this.Label9);
            this.panTitle.Controls.Add(this.Label8);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(598, 39);
            this.panTitle.TabIndex = 90;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(190, 7);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(103, 21);
            this.dtpEndDate.TabIndex = 95;
            // 
            // dtpFrDate
            // 
            this.dtpFrDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrDate.Location = new System.Drawing.Point(70, 7);
            this.dtpFrDate.Name = "dtpFrDate";
            this.dtpFrDate.Size = new System.Drawing.Size(103, 21);
            this.dtpFrDate.TabIndex = 94;
            // 
            // btnSearchAll
            // 
            this.btnSearchAll.BackColor = System.Drawing.SystemColors.Control;
            this.btnSearchAll.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSearchAll.Font = new System.Drawing.Font("굴림", 9F);
            this.btnSearchAll.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSearchAll.Location = new System.Drawing.Point(364, 2);
            this.btnSearchAll.Name = "btnSearchAll";
            this.btnSearchAll.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSearchAll.Size = new System.Drawing.Size(79, 30);
            this.btnSearchAll.TabIndex = 91;
            this.btnSearchAll.Text = "조회";
            this.btnSearchAll.UseVisualStyleBackColor = true;
            this.btnSearchAll.Click += new System.EventHandler(this.btnSearchAll_Click);
            // 
            // chkAsc
            // 
            this.chkAsc.AutoSize = true;
            this.chkAsc.BackColor = System.Drawing.Color.Transparent;
            this.chkAsc.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkAsc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkAsc.ForeColor = System.Drawing.SystemColors.WindowText;
            this.chkAsc.Location = new System.Drawing.Point(301, 9);
            this.chkAsc.Name = "chkAsc";
            this.chkAsc.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkAsc.Size = new System.Drawing.Size(57, 16);
            this.chkAsc.TabIndex = 90;
            this.chkAsc.Text = "순정렬";
            this.chkAsc.UseVisualStyleBackColor = false;
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.BackColor = System.Drawing.Color.Transparent;
            this.Label9.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label9.Location = new System.Drawing.Point(173, 11);
            this.Label9.Name = "Label9";
            this.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label9.Size = new System.Drawing.Size(14, 12);
            this.Label9.TabIndex = 93;
            this.Label9.Text = "~";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.BackColor = System.Drawing.Color.Transparent;
            this.Label8.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Label8.Location = new System.Drawing.Point(10, 11);
            this.Label8.Name = "Label8";
            this.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label8.Size = new System.Drawing.Size(53, 12);
            this.Label8.TabIndex = 92;
            this.Label8.Text = "조회기간";
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(443, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // ss1
            // 
            this.ss1.AccessibleDescription = "ss1, Sheet1, Row 0, Column 0, ";
            this.ss1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss1.Location = new System.Drawing.Point(0, 39);
            this.ss1.Name = "ss1";
            this.ss1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss1_Sheet1});
            this.ss1.Size = new System.Drawing.Size(598, 411);
            this.ss1.TabIndex = 162;
            // 
            // ss1_Sheet1
            // 
            this.ss1_Sheet1.Reset();
            this.ss1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss1_Sheet1.ColumnCount = 7;
            this.ss1_Sheet1.RowCount = 1;
            this.ss1_Sheet1.Cells.Get(0, 1).CellType = textCellType1;
            this.ss1_Sheet1.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Cells.Get(0, 1).Value = "00:00";
            this.ss1_Sheet1.Cells.Get(0, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "날짜";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "시간";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "투석액종류/농도";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "주입량";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "배액량";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "제수량";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "총제수량";
            this.ss1_Sheet1.ColumnHeader.Rows.Get(0).Height = 28F;
            this.ss1_Sheet1.Columns.Get(0).CellType = textCellType2;
            this.ss1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(0).InputScope = FarPoint.Win.InputScopeNameValue.Search;
            this.ss1_Sheet1.Columns.Get(0).Label = "날짜";
            this.ss1_Sheet1.Columns.Get(0).Locked = true;
            this.ss1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(0).Width = 100F;
            this.ss1_Sheet1.Columns.Get(1).CellType = textCellType3;
            this.ss1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(1).Label = "시간";
            this.ss1_Sheet1.Columns.Get(1).Locked = true;
            this.ss1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(2).CellType = textCellType4;
            this.ss1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ss1_Sheet1.Columns.Get(2).Label = "투석액종류/농도";
            this.ss1_Sheet1.Columns.Get(2).Locked = true;
            this.ss1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(2).Width = 118F;
            this.ss1_Sheet1.Columns.Get(3).CellType = textCellType5;
            this.ss1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(3).Label = "주입량";
            this.ss1_Sheet1.Columns.Get(3).Locked = true;
            this.ss1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(3).Width = 65F;
            this.ss1_Sheet1.Columns.Get(4).CellType = textCellType6;
            this.ss1_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(4).Label = "배액량";
            this.ss1_Sheet1.Columns.Get(4).Locked = true;
            this.ss1_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(4).Width = 65F;
            this.ss1_Sheet1.Columns.Get(5).CellType = textCellType7;
            this.ss1_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(5).Label = "제수량";
            this.ss1_Sheet1.Columns.Get(5).Locked = true;
            this.ss1_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(5).Width = 65F;
            this.ss1_Sheet1.Columns.Get(6).CellType = textCellType8;
            this.ss1_Sheet1.Columns.Get(6).Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.ss1_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(6).Label = "총제수량";
            this.ss1_Sheet1.Columns.Get(6).Locked = true;
            this.ss1_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(6).Width = 65F;
            this.ss1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmEmrPeritonealDialysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 450);
            this.Controls.Add(this.ss1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmEmrPeritonealDialysis";
            this.Text = "frmEmrPeritonealDialysis";
            this.Load += new System.EventHandler(this.frmEmrPeritonealDialysis_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.DateTimePicker dtpFrDate;
        public System.Windows.Forms.Button btnSearchAll;
        public System.Windows.Forms.CheckBox chkAsc;
        public System.Windows.Forms.Label Label9;
        public System.Windows.Forms.Label Label8;
        private System.Windows.Forms.Button btnExit;
        private FarPoint.Win.Spread.FpSpread ss1;
        private FarPoint.Win.Spread.SheetView ss1_Sheet1;
    }
}