namespace ComLibB
{
    partial class frmDocuListSearch
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.OptSort1 = new System.Windows.Forms.RadioButton();
            this.OptSort0 = new System.Windows.Forms.RadioButton();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.CboBuseName = new System.Windows.Forms.ComboBox();
            this.DtpToDate = new System.Windows.Forms.DateTimePicker();
            this.DtpFrDate = new System.Windows.Forms.DateTimePicker();
            this.BtnPrint = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.BtnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SsView = new FarPoint.Win.Spread.FpSpread();
            this.SsView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SsView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SsView_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.BtnSearch);
            this.panel1.Controls.Add(this.CboBuseName);
            this.panel1.Controls.Add(this.DtpToDate);
            this.panel1.Controls.Add(this.DtpFrDate);
            this.panel1.Controls.Add(this.BtnPrint);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1254, 33);
            this.panel1.TabIndex = 18;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.OptSort1);
            this.panel2.Controls.Add(this.OptSort0);
            this.panel2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panel2.Location = new System.Drawing.Point(103, 4);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel2.Size = new System.Drawing.Size(124, 25);
            this.panel2.TabIndex = 31;
            // 
            // OptSort1
            // 
            this.OptSort1.AutoSize = true;
            this.OptSort1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.OptSort1.Location = new System.Drawing.Point(65, 1);
            this.OptSort1.Name = "OptSort1";
            this.OptSort1.Size = new System.Drawing.Size(52, 21);
            this.OptSort1.TabIndex = 0;
            this.OptSort1.Text = "발송";
            this.OptSort1.UseVisualStyleBackColor = true;
            // 
            // OptSort0
            // 
            this.OptSort0.AutoSize = true;
            this.OptSort0.Checked = true;
            this.OptSort0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.OptSort0.Location = new System.Drawing.Point(5, 1);
            this.OptSort0.Name = "OptSort0";
            this.OptSort0.Size = new System.Drawing.Size(52, 21);
            this.OptSort0.TabIndex = 0;
            this.OptSort0.TabStop = true;
            this.OptSort0.Text = "접수";
            this.OptSort0.UseVisualStyleBackColor = true;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnSearch.Location = new System.Drawing.Point(1106, 0);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(74, 33);
            this.BtnSearch.TabIndex = 5;
            this.BtnSearch.Text = "조회";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // CboBuseName
            // 
            this.CboBuseName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboBuseName.FormattingEnabled = true;
            this.CboBuseName.Location = new System.Drawing.Point(615, 6);
            this.CboBuseName.Name = "CboBuseName";
            this.CboBuseName.Size = new System.Drawing.Size(144, 20);
            this.CboBuseName.TabIndex = 17;
            // 
            // DtpToDate
            // 
            this.DtpToDate.CustomFormat = "yyyy-MM-dd";
            this.DtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpToDate.Location = new System.Drawing.Point(420, 6);
            this.DtpToDate.Name = "DtpToDate";
            this.DtpToDate.Size = new System.Drawing.Size(101, 21);
            this.DtpToDate.TabIndex = 18;
            // 
            // DtpFrDate
            // 
            this.DtpFrDate.CustomFormat = "yyyy-MM-dd";
            this.DtpFrDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpFrDate.Location = new System.Drawing.Point(321, 6);
            this.DtpFrDate.Name = "DtpFrDate";
            this.DtpFrDate.Size = new System.Drawing.Size(99, 21);
            this.DtpFrDate.TabIndex = 17;
            // 
            // BtnPrint
            // 
            this.BtnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnPrint.Location = new System.Drawing.Point(1180, 0);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(74, 33);
            this.BtnPrint.TabIndex = 6;
            this.BtnPrint.Text = "인쇄";
            this.BtnPrint.UseVisualStyleBackColor = true;
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Gainsboro;
            this.label2.Location = new System.Drawing.Point(527, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "부서별 조회";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Gainsboro;
            this.label3.Location = new System.Drawing.Point(9, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "공문 조회구분";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Gainsboro;
            this.label1.Location = new System.Drawing.Point(233, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "작업년도";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.BtnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1254, 34);
            this.panTitle.TabIndex = 17;
            // 
            // BtnExit
            // 
            this.BtnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnExit.Location = new System.Drawing.Point(1176, 0);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(74, 30);
            this.BtnExit.TabIndex = 20;
            this.BtnExit.Text = "닫기";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(74, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "공문대장";
            // 
            // SsView
            // 
            this.SsView.AccessibleDescription = "SsView, Sheet1, Row 0, Column 0, ";
            this.SsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SsView.Location = new System.Drawing.Point(0, 67);
            this.SsView.Name = "SsView";
            this.SsView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SsView_Sheet1});
            this.SsView.Size = new System.Drawing.Size(1254, 648);
            this.SsView.TabIndex = 19;
            // 
            // SsView_Sheet1
            // 
            this.SsView_Sheet1.Reset();
            this.SsView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SsView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SsView_Sheet1.ColumnCount = 7;
            this.SsView_Sheet1.RowCount = 1;
            this.SsView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "No.";
            this.SsView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "일자";
            this.SsView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "문서번호";
            this.SsView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "기관명";
            this.SsView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "공문명";
            this.SsView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "담당부서";
            this.SsView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "비고";
            this.SsView_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.SsView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(0).Label = "No.";
            this.SsView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.SsView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(1).Label = "일자";
            this.SsView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(1).Width = 108F;
            this.SsView_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.SsView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(2).Label = "문서번호";
            this.SsView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(2).Width = 151F;
            this.SsView_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.SsView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SsView_Sheet1.Columns.Get(3).Label = "기관명";
            this.SsView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(3).Width = 268F;
            this.SsView_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.SsView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SsView_Sheet1.Columns.Get(4).Label = "공문명";
            this.SsView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(4).Width = 442F;
            this.SsView_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.SsView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(5).Label = "담당부서";
            this.SsView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(5).Width = 104F;
            this.SsView_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.SsView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.SsView_Sheet1.Columns.Get(6).Label = "비고";
            this.SsView_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SsView_Sheet1.Columns.Get(6).Width = 72F;
            this.SsView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SsView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmDocuListSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1254, 715);
            this.Controls.Add(this.SsView);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmDocuListSearch";
            this.Text = "frmDocuListSearch";
            this.Activated += new System.EventHandler(this.frmDocuListSearch_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDocuListSearch_FormClosed);
            this.Load += new System.EventHandler(this.frmDocuListSearch_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SsView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SsView_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox CboBuseName;
        private System.Windows.Forms.DateTimePicker DtpToDate;
        private System.Windows.Forms.DateTimePicker DtpFrDate;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton OptSort1;
        private System.Windows.Forms.RadioButton OptSort0;
        private System.Windows.Forms.Label label3;
        private FarPoint.Win.Spread.FpSpread SsView;
        private FarPoint.Win.Spread.SheetView SsView_Sheet1;
    }
}