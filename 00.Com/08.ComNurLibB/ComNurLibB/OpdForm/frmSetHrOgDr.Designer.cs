namespace ComNurLibB
{
    partial class frmSetHrOgDr
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.lblExCode = new System.Windows.Forms.Label();
            this.lblWrtno = new System.Windows.Forms.Label();
            this.cboDrSabun = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panInfo = new System.Windows.Forms.Panel();
            this.lblinfo = new System.Windows.Forms.Label();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.ssView2 = new FarPoint.Win.Spread.FpSpread();
            this.ssView2_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView2_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.btnSearch);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(433, 34);
            this.panTitle.TabIndex = 25;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Location = new System.Drawing.Point(277, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 30);
            this.btnSave.TabIndex = 34;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(204, 2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 33;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(349, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 31;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTitle.Location = new System.Drawing.Point(9, 6);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(144, 21);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "검진환자 의사세팅";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.dtpDate);
            this.panel1.Controls.Add(this.lblExCode);
            this.panel1.Controls.Add(this.lblWrtno);
            this.panel1.Controls.Add(this.cboDrSabun);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(433, 67);
            this.panel1.TabIndex = 29;
            // 
            // dtpDate
            // 
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDate.Location = new System.Drawing.Point(74, 10);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(121, 21);
            this.dtpDate.TabIndex = 42;
            // 
            // lblExCode
            // 
            this.lblExCode.AutoSize = true;
            this.lblExCode.Location = new System.Drawing.Point(238, 40);
            this.lblExCode.Name = "lblExCode";
            this.lblExCode.Size = new System.Drawing.Size(0, 12);
            this.lblExCode.TabIndex = 41;
            // 
            // lblWrtno
            // 
            this.lblWrtno.AutoSize = true;
            this.lblWrtno.Location = new System.Drawing.Point(238, 14);
            this.lblWrtno.Name = "lblWrtno";
            this.lblWrtno.Size = new System.Drawing.Size(0, 12);
            this.lblWrtno.TabIndex = 40;
            // 
            // cboDrSabun
            // 
            this.cboDrSabun.FormattingEnabled = true;
            this.cboDrSabun.Location = new System.Drawing.Point(74, 37);
            this.cboDrSabun.Name = "cboDrSabun";
            this.cboDrSabun.Size = new System.Drawing.Size(121, 20);
            this.cboDrSabun.TabIndex = 38;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 36;
            this.label2.Text = "진료의";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 37;
            this.label1.Text = "접수일자";
            // 
            // panInfo
            // 
            this.panInfo.BackColor = System.Drawing.Color.CornflowerBlue;
            this.panInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panInfo.Controls.Add(this.lblinfo);
            this.panInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panInfo.Location = new System.Drawing.Point(0, 101);
            this.panInfo.Name = "panInfo";
            this.panInfo.Size = new System.Drawing.Size(433, 32);
            this.panInfo.TabIndex = 30;
            // 
            // lblinfo
            // 
            this.lblinfo.AutoSize = true;
            this.lblinfo.BackColor = System.Drawing.Color.CornflowerBlue;
            this.lblinfo.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblinfo.ForeColor = System.Drawing.Color.White;
            this.lblinfo.Location = new System.Drawing.Point(4, 7);
            this.lblinfo.Name = "lblinfo";
            this.lblinfo.Size = new System.Drawing.Size(57, 12);
            this.lblinfo.TabIndex = 0;
            this.lblinfo.Text = "의사세팅";
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssView.Location = new System.Drawing.Point(0, 133);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(433, 455);
            this.ssView.TabIndex = 31;
            this.ssView.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssView.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView_CellDoubleClick);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 8;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "성명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "등록번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "나이";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "시행의사";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "접수번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "건진번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "ROWID";
            this.ssView_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Label = "성명";
            this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Width = 80F;
            this.ssView_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Label = "등록번호";
            this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Width = 80F;
            this.ssView_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Label = "나이";
            this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Width = 40F;
            this.ssView_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Label = "시행의사";
            this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Width = 80F;
            this.ssView_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).Label = "접수번호";
            this.ssView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Label = "코드";
            this.ssView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.ssView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Label = "건진번호";
            this.ssView_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).CellType = textCellType8;
            this.ssView_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Label = "ROWID";
            this.ssView_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // ssView2
            // 
            this.ssView2.AccessibleDescription = "ssView2, Sheet1, Row 0, Column 0, ";
            this.ssView2.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssView2.Location = new System.Drawing.Point(22, 345);
            this.ssView2.Name = "ssView2";
            this.ssView2.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView2_Sheet1});
            this.ssView2.Size = new System.Drawing.Size(399, 43);
            this.ssView2.TabIndex = 32;
            this.ssView2.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssView2.Visible = false;
            // 
            // ssView2_Sheet1
            // 
            this.ssView2_Sheet1.Reset();
            this.ssView2_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView2_Sheet1.ColumnCount = 6;
            this.ssView2_Sheet1.RowCount = 1;
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "PANO";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "WRTNO";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "HPANO";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "EXCODE";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "GBN";
            this.ssView2_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "ROWID";
            this.ssView2_Sheet1.Columns.Get(0).CellType = textCellType9;
            this.ssView2_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(0).Label = "PANO";
            this.ssView2_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(1).CellType = textCellType10;
            this.ssView2_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(1).Label = "WRTNO";
            this.ssView2_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(2).CellType = textCellType11;
            this.ssView2_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(2).Label = "HPANO";
            this.ssView2_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(3).CellType = textCellType12;
            this.ssView2_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(3).Label = "EXCODE";
            this.ssView2_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(4).CellType = textCellType13;
            this.ssView2_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(4).Label = "GBN";
            this.ssView2_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(5).CellType = textCellType14;
            this.ssView2_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView2_Sheet1.Columns.Get(5).Label = "ROWID";
            this.ssView2_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView2_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView2_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmSetHrOgDr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 588);
            this.Controls.Add(this.ssView2);
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.panInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmSetHrOgDr";
            this.Text = "frmSetHrOgDr";
            this.Load += new System.EventHandler(this.frmSetHrOgDr_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panInfo.ResumeLayout(false);
            this.panInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView2_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cboDrSabun;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panInfo;
        private System.Windows.Forms.Label lblinfo;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private FarPoint.Win.Spread.FpSpread ssView2;
        private FarPoint.Win.Spread.SheetView ssView2_Sheet1;
        private System.Windows.Forms.Label lblExCode;
        private System.Windows.Forms.Label lblWrtno;
        private System.Windows.Forms.DateTimePicker dtpDate;
    }
}