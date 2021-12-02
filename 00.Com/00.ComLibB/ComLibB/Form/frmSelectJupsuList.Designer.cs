namespace ComLibB
{
    partial class frmSelectJupsuList
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.ss99 = new FarPoint.Win.Spread.FpSpread();
            this.ss99_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.panTitle.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss99)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss99_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.dtpDate);
            this.panTitle.Controls.Add(this.button1);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(608, 34);
            this.panTitle.TabIndex = 76;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(522, 1);
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
            this.lblTitle.Size = new System.Drawing.Size(196, 16);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "당일접수 및 재원자 내역";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(608, 28);
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
            this.lblTitleSub0.Text = "예약현황";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ss99
            // 
            this.ss99.AccessibleDescription = "ss99, Sheet1, Row 0, Column 0, ";
            this.ss99.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss99.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ss99.Location = new System.Drawing.Point(0, 62);
            this.ss99.Name = "ss99";
            this.ss99.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss99_Sheet1});
            this.ss99.Size = new System.Drawing.Size(608, 332);
            this.ss99.TabIndex = 81;
            this.ss99.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ss99_CellDoubleClick);
            // 
            // ss99_Sheet1
            // 
            this.ss99_Sheet1.Reset();
            this.ss99_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss99_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss99_Sheet1.ColumnCount = 7;
            this.ss99_Sheet1.RowCount = 10;
            this.ss99_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss99_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss99_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss99_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "접수일자";
            this.ss99_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "등록번호";
            this.ss99_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "환자성명";
            this.ss99_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "진료과";
            this.ss99_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "의사명";
            this.ss99_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "의사코드";
            this.ss99_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "외래/입원";
            this.ss99_Sheet1.ColumnHeader.Rows.Get(0).Height = 39F;
            this.ss99_Sheet1.Columns.Get(0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(234)))), ((int)(((byte)(210)))));
            this.ss99_Sheet1.Columns.Get(0).CellType = textCellType8;
            this.ss99_Sheet1.Columns.Get(0).Label = "접수일자";
            this.ss99_Sheet1.Columns.Get(0).Width = 122F;
            this.ss99_Sheet1.Columns.Get(1).CellType = textCellType9;
            this.ss99_Sheet1.Columns.Get(1).Label = "등록번호";
            this.ss99_Sheet1.Columns.Get(1).Width = 62F;
            this.ss99_Sheet1.Columns.Get(2).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(238)))), ((int)(((byte)(210)))));
            this.ss99_Sheet1.Columns.Get(2).CellType = textCellType10;
            this.ss99_Sheet1.Columns.Get(2).Label = "환자성명";
            this.ss99_Sheet1.Columns.Get(2).Width = 120F;
            this.ss99_Sheet1.Columns.Get(3).CellType = textCellType11;
            this.ss99_Sheet1.Columns.Get(3).Label = "진료과";
            this.ss99_Sheet1.Columns.Get(4).CellType = textCellType12;
            this.ss99_Sheet1.Columns.Get(4).Label = "의사명";
            this.ss99_Sheet1.Columns.Get(4).Width = 100F;
            this.ss99_Sheet1.Columns.Get(5).CellType = textCellType13;
            this.ss99_Sheet1.Columns.Get(5).Label = "의사코드";
            this.ss99_Sheet1.Columns.Get(5).Width = 84F;
            this.ss99_Sheet1.Columns.Get(6).CellType = textCellType14;
            this.ss99_Sheet1.Columns.Get(6).Label = "외래/입원";
            this.ss99_Sheet1.Columns.Get(6).Visible = false;
            this.ss99_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss99_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss99_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss99_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss99_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(320, 4);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(111, 25);
            this.dtpDate.TabIndex = 16;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.AutoSize = true;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(448, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 30);
            this.button1.TabIndex = 15;
            this.button1.Text = "조회";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmSelectJupsuList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 394);
            this.Controls.Add(this.ss99);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmSelectJupsuList";
            this.Text = "frmSelectJupsuList";
            this.Load += new System.EventHandler(this.frmSelectJupsuList_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss99)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss99_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Button button1;
        private FarPoint.Win.Spread.FpSpread ss99;
        private FarPoint.Win.Spread.SheetView ss99_Sheet1;
    }
}