namespace ComEmrBase
{
    partial class frmEmrRecordView
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.cboGBN = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkAsc = new System.Windows.Forms.CheckBox();
            this.dtpEDate = new System.Windows.Forms.DateTimePicker();
            this.btnSearch2 = new System.Windows.Forms.Button();
            this.dtpSDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.ssList1 = new FarPoint.Win.Spread.FpSpread();
            this.ssList1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.cboGBN);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.chkAsc);
            this.panel2.Controls.Add(this.dtpEDate);
            this.panel2.Controls.Add(this.btnSearch2);
            this.panel2.Controls.Add(this.dtpSDate);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1064, 36);
            this.panel2.TabIndex = 6;
            // 
            // cboGBN
            // 
            this.cboGBN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGBN.FormattingEnabled = true;
            this.cboGBN.Items.AddRange(new object[] {
            "인공호흡기",
            "CRRT",
            "산소요법"});
            this.cboGBN.Location = new System.Drawing.Point(336, 7);
            this.cboGBN.Name = "cboGBN";
            this.cboGBN.Size = new System.Drawing.Size(92, 20);
            this.cboGBN.TabIndex = 102;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(294, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 17);
            this.label2.TabIndex = 101;
            this.label2.Text = "항목";
            // 
            // chkAsc
            // 
            this.chkAsc.AutoSize = true;
            this.chkAsc.Location = new System.Drawing.Point(218, 9);
            this.chkAsc.Name = "chkAsc";
            this.chkAsc.Size = new System.Drawing.Size(60, 16);
            this.chkAsc.TabIndex = 100;
            this.chkAsc.Text = "순정렬";
            this.chkAsc.UseVisualStyleBackColor = true;
            // 
            // dtpEDate
            // 
            this.dtpEDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEDate.Location = new System.Drawing.Point(115, 7);
            this.dtpEDate.Name = "dtpEDate";
            this.dtpEDate.Size = new System.Drawing.Size(97, 21);
            this.dtpEDate.TabIndex = 78;
            // 
            // btnSearch2
            // 
            this.btnSearch2.Location = new System.Drawing.Point(434, 2);
            this.btnSearch2.Name = "btnSearch2";
            this.btnSearch2.Size = new System.Drawing.Size(62, 30);
            this.btnSearch2.TabIndex = 79;
            this.btnSearch2.Text = "조회";
            this.btnSearch2.UseVisualStyleBackColor = true;
            this.btnSearch2.Click += new System.EventHandler(this.btnSearch2_Click);
            // 
            // dtpSDate
            // 
            this.dtpSDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSDate.Location = new System.Drawing.Point(4, 7);
            this.dtpSDate.Name = "dtpSDate";
            this.dtpSDate.Size = new System.Drawing.Size(97, 21);
            this.dtpSDate.TabIndex = 80;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(101, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 12);
            this.label1.TabIndex = 81;
            this.label1.Text = "~";
            // 
            // ssList1
            // 
            this.ssList1.AccessibleDescription = "ssList1, Sheet1, Row 0, Column 0, 2020-01-01";
            this.ssList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssList1.Location = new System.Drawing.Point(0, 36);
            this.ssList1.Name = "ssList1";
            this.ssList1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssList1_Sheet1});
            this.ssList1.Size = new System.Drawing.Size(1064, 584);
            this.ssList1.TabIndex = 7;
            // 
            // ssList1_Sheet1
            // 
            this.ssList1_Sheet1.Reset();
            this.ssList1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssList1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssList1_Sheet1.ColumnCount = 2;
            this.ssList1_Sheet1.RowCount = 1;
            this.ssList1_Sheet1.Cells.Get(0, 0).Value = "2020-01-01";
            this.ssList1_Sheet1.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList1_Sheet1.Cells.Get(0, 1).Value = "11:11";
            this.ssList1_Sheet1.Cells.Get(0, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssList1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "일자";
            this.ssList1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "시간";
            this.ssList1_Sheet1.ColumnHeader.Rows.Get(0).Height = 49F;
            this.ssList1_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssList1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(0).Label = "일자";
            this.ssList1_Sheet1.Columns.Get(0).Locked = true;
            this.ssList1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(0).Width = 100F;
            this.ssList1_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssList1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssList1_Sheet1.Columns.Get(1).Label = "시간";
            this.ssList1_Sheet1.Columns.Get(1).Locked = true;
            this.ssList1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssList1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssList1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssList1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssList1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssList1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssList1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmEmrRecordView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 620);
            this.Controls.Add(this.ssList1);
            this.Controls.Add(this.panel2);
            this.Name = "frmEmrRecordView";
            this.Text = "frmEmrRecordView";
            this.Load += new System.EventHandler(this.frmEmrRecordView_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssList1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dtpEDate;
        private System.Windows.Forms.Button btnSearch2;
        private System.Windows.Forms.DateTimePicker dtpSDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkAsc;
        private FarPoint.Win.Spread.FpSpread ssList1;
        private FarPoint.Win.Spread.SheetView ssList1_Sheet1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboGBN;
    }
}