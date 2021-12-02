namespace ComLibB
{
    partial class frmDocu_03
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
            this.BtnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ComboToBuse = new System.Windows.Forms.ComboBox();
            this.ToDate = new System.Windows.Forms.DateTimePicker();
            this.ComboFrBuse = new System.Windows.Forms.ComboBox();
            this.BtnPrint = new System.Windows.Forms.Button();
            this.FrDate = new System.Windows.Forms.DateTimePicker();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.ss1 = new FarPoint.Win.Spread.FpSpread();
            this.ss1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).BeginInit();
            this.SuspendLayout();
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
            this.panTitle.Size = new System.Drawing.Size(966, 34);
            this.panTitle.TabIndex = 15;
            // 
            // BtnExit
            // 
            this.BtnExit.AutoSize = true;
            this.BtnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.BtnExit.Location = new System.Drawing.Point(883, 0);
            this.BtnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(79, 30);
            this.BtnExit.TabIndex = 19;
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
            this.lblTitle.Size = new System.Drawing.Size(106, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "문서발송현황";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ComboToBuse);
            this.panel1.Controls.Add(this.ToDate);
            this.panel1.Controls.Add(this.ComboFrBuse);
            this.panel1.Controls.Add(this.BtnPrint);
            this.panel1.Controls.Add(this.FrDate);
            this.panel1.Controls.Add(this.BtnSearch);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(966, 33);
            this.panel1.TabIndex = 16;
            // 
            // ComboToBuse
            // 
            this.ComboToBuse.FormattingEnabled = true;
            this.ComboToBuse.Location = new System.Drawing.Point(602, 4);
            this.ComboToBuse.Name = "ComboToBuse";
            this.ComboToBuse.Size = new System.Drawing.Size(196, 25);
            this.ComboToBuse.TabIndex = 17;
            // 
            // ToDate
            // 
            this.ToDate.CustomFormat = "yyyy-MM-dd";
            this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ToDate.Location = new System.Drawing.Point(204, 4);
            this.ToDate.Name = "ToDate";
            this.ToDate.Size = new System.Drawing.Size(113, 25);
            this.ToDate.TabIndex = 17;
            // 
            // ComboFrBuse
            // 
            this.ComboFrBuse.FormattingEnabled = true;
            this.ComboFrBuse.Location = new System.Drawing.Point(405, 4);
            this.ComboFrBuse.Name = "ComboFrBuse";
            this.ComboFrBuse.Size = new System.Drawing.Size(196, 25);
            this.ComboFrBuse.TabIndex = 16;
            // 
            // BtnPrint
            // 
            this.BtnPrint.Location = new System.Drawing.Point(881, 3);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(81, 27);
            this.BtnPrint.TabIndex = 5;
            this.BtnPrint.Text = "인쇄";
            this.BtnPrint.UseVisualStyleBackColor = true;
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // FrDate
            // 
            this.FrDate.CustomFormat = "yyyy-MM-dd";
            this.FrDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.FrDate.Location = new System.Drawing.Point(91, 4);
            this.FrDate.Name = "FrDate";
            this.FrDate.Size = new System.Drawing.Size(113, 25);
            this.FrDate.TabIndex = 16;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Location = new System.Drawing.Point(800, 3);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(81, 27);
            this.BtnSearch.TabIndex = 4;
            this.BtnSearch.Text = "조회";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Gainsboro;
            this.label2.Location = new System.Drawing.Point(322, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "구분";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Gainsboro;
            this.label1.Location = new System.Drawing.Point(8, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "작업년도";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 67);
            this.panTitleSub0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(966, 28);
            this.panTitleSub0.TabIndex = 18;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(43, 15);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "리스트";
            // 
            // ss1
            // 
            this.ss1.AccessibleDescription = "";
            this.ss1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss1.Location = new System.Drawing.Point(0, 95);
            this.ss1.Name = "ss1";
            this.ss1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss1_Sheet1});
            this.ss1.Size = new System.Drawing.Size(966, 500);
            this.ss1.TabIndex = 19;
            // 
            // ss1_Sheet1
            // 
            this.ss1_Sheet1.Reset();
            this.ss1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss1_Sheet1.ColumnCount = 8;
            this.ss1_Sheet1.RowCount = 1;
            this.ss1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ss1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "번호";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "년월일";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "문서번호";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "수신기관명";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "공문명";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "담당부서";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "담당자";
            this.ss1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "시간";
            this.ss1_Sheet1.ColumnHeader.Rows.Get(0).Height = 41F;
            this.ss1_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ss1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(0).Label = "번호";
            this.ss1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(0).Width = 62F;
            this.ss1_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ss1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(1).Label = "년월일";
            this.ss1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(1).Width = 100F;
            this.ss1_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ss1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ss1_Sheet1.Columns.Get(2).Label = "문서번호";
            this.ss1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(2).Width = 220F;
            this.ss1_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ss1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ss1_Sheet1.Columns.Get(3).Label = "수신기관명";
            this.ss1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(3).Width = 220F;
            this.ss1_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ss1_Sheet1.Columns.Get(4).Label = "공문명";
            this.ss1_Sheet1.Columns.Get(4).Width = 300F;
            this.ss1_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ss1_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(5).Label = "담당부서";
            this.ss1_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(5).Width = 120F;
            this.ss1_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.ss1_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(6).Label = "담당자";
            this.ss1_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(6).Width = 80F;
            this.ss1_Sheet1.Columns.Get(7).CellType = textCellType8;
            this.ss1_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(7).Label = "시간";
            this.ss1_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ss1_Sheet1.Columns.Get(7).Visible = false;
            this.ss1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss1_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ss1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.ss1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmDocu_03
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(966, 595);
            this.Controls.Add(this.ss1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmDocu_03";
            this.Text = "frmDocu_03";
            this.Activated += new System.EventHandler(this.frmDocu_03_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDocu_03_FormClosed);
            this.Load += new System.EventHandler(this.frmDocu_03_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox ComboToBuse;
        private System.Windows.Forms.DateTimePicker ToDate;
        private System.Windows.Forms.ComboBox ComboFrBuse;
        private System.Windows.Forms.Button BtnPrint;
        private System.Windows.Forms.DateTimePicker FrDate;
        private System.Windows.Forms.Button BtnSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private FarPoint.Win.Spread.FpSpread ss1;
        private FarPoint.Win.Spread.SheetView ss1_Sheet1;
        private System.Windows.Forms.Button BtnExit;
    }
}