namespace ComLibB
{
    partial class frmWonCodeEntry
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType3 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType4 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            this.pan0 = new System.Windows.Forms.Panel();
            this.pan2 = new System.Windows.Forms.Panel();
            this.pan4 = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pan3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboWongaList = new System.Windows.Forms.ComboBox();
            this.grb = new System.Windows.Forms.GroupBox();
            this.optWongaCode = new System.Windows.Forms.RadioButton();
            this.optSugaCode = new System.Windows.Forms.RadioButton();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pan0.SuspendLayout();
            this.pan2.SuspendLayout();
            this.pan4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.pan3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grb.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pan0
            // 
            this.pan0.Controls.Add(this.pan2);
            this.pan0.Controls.Add(this.panTitleSub0);
            this.pan0.Controls.Add(this.panTitle);
            this.pan0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan0.Location = new System.Drawing.Point(0, 0);
            this.pan0.Name = "pan0";
            this.pan0.Size = new System.Drawing.Size(657, 693);
            this.pan0.TabIndex = 1;
            // 
            // pan2
            // 
            this.pan2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pan2.Controls.Add(this.pan4);
            this.pan2.Controls.Add(this.pan3);
            this.pan2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan2.Location = new System.Drawing.Point(0, 62);
            this.pan2.Name = "pan2";
            this.pan2.Size = new System.Drawing.Size(657, 631);
            this.pan2.TabIndex = 13;
            // 
            // pan4
            // 
            this.pan4.BackColor = System.Drawing.Color.White;
            this.pan4.Controls.Add(this.ssView);
            this.pan4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan4.Location = new System.Drawing.Point(0, 59);
            this.pan4.Name = "pan4";
            this.pan4.Size = new System.Drawing.Size(653, 568);
            this.pan4.TabIndex = 18;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssView.Location = new System.Drawing.Point(0, 0);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(653, 568);
            this.ssView.TabIndex = 46;
            this.ssView.EditModeOff += new System.EventHandler(this.ssView_EditModeOff);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 8;
            this.ssView_Sheet1.RowCount = 50;
            this.ssView_Sheet1.Cells.Get(49, 7).Value = true;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "수가코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "품명코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "분류";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "수  가  명  칭";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "원가코드";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "원가항목명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "재료대\r\n여  부";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "조영제\r\n여  부";
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 43F;
            this.ssView_Sheet1.Columns.Get(0).CellType = textCellType7;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(0).Label = "수가코드";
            this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).CellType = textCellType8;
            this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(1).Label = "품명코드";
            this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).CellType = textCellType9;
            this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Label = "분류";
            this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Width = 43F;
            this.ssView_Sheet1.Columns.Get(3).CellType = textCellType10;
            this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(3).Label = "수  가  명  칭";
            this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Width = 200F;
            this.ssView_Sheet1.Columns.Get(4).CellType = textCellType11;
            this.ssView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).Label = "원가코드";
            this.ssView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).CellType = textCellType12;
            this.ssView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Label = "원가항목명";
            this.ssView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Width = 74F;
            this.ssView_Sheet1.Columns.Get(6).CellType = checkBoxCellType3;
            this.ssView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Label = "재료대\r\n여  부";
            this.ssView_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Width = 46F;
            this.ssView_Sheet1.Columns.Get(7).CellType = checkBoxCellType4;
            this.ssView_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Label = "조영제\r\n여  부";
            this.ssView_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Width = 46F;
            this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pan3
            // 
            this.pan3.BackColor = System.Drawing.Color.White;
            this.pan3.Controls.Add(this.groupBox1);
            this.pan3.Controls.Add(this.grb);
            this.pan3.Controls.Add(this.btnExit);
            this.pan3.Controls.Add(this.btnSearch);
            this.pan3.Controls.Add(this.btnPrint);
            this.pan3.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan3.Location = new System.Drawing.Point(0, 0);
            this.pan3.Name = "pan3";
            this.pan3.Size = new System.Drawing.Size(653, 59);
            this.pan3.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboWongaList);
            this.groupBox1.Location = new System.Drawing.Point(108, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(229, 49);
            this.groupBox1.TabIndex = 43;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "조회할 원가항목은?";
            // 
            // cboWongaList
            // 
            this.cboWongaList.FormattingEnabled = true;
            this.cboWongaList.Location = new System.Drawing.Point(8, 19);
            this.cboWongaList.Name = "cboWongaList";
            this.cboWongaList.Size = new System.Drawing.Size(213, 20);
            this.cboWongaList.TabIndex = 0;
            this.cboWongaList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboWongaList_KeyDown);
            // 
            // grb
            // 
            this.grb.Controls.Add(this.optWongaCode);
            this.grb.Controls.Add(this.optSugaCode);
            this.grb.Location = new System.Drawing.Point(8, 5);
            this.grb.Name = "grb";
            this.grb.Size = new System.Drawing.Size(94, 49);
            this.grb.TabIndex = 42;
            this.grb.TabStop = false;
            this.grb.Text = "SORT";
            // 
            // optWongaCode
            // 
            this.optWongaCode.AutoSize = true;
            this.optWongaCode.Checked = true;
            this.optWongaCode.Location = new System.Drawing.Point(12, 29);
            this.optWongaCode.Name = "optWongaCode";
            this.optWongaCode.Size = new System.Drawing.Size(71, 16);
            this.optWongaCode.TabIndex = 40;
            this.optWongaCode.TabStop = true;
            this.optWongaCode.Text = "원가코드";
            this.optWongaCode.UseVisualStyleBackColor = true;
            this.optWongaCode.CheckedChanged += new System.EventHandler(this.optSCheckedChanged);
            // 
            // optSugaCode
            // 
            this.optSugaCode.AutoSize = true;
            this.optSugaCode.Location = new System.Drawing.Point(12, 13);
            this.optSugaCode.Name = "optSugaCode";
            this.optSugaCode.Size = new System.Drawing.Size(71, 16);
            this.optSugaCode.TabIndex = 39;
            this.optSugaCode.TabStop = true;
            this.optSugaCode.Text = "수가코드";
            this.optSugaCode.UseVisualStyleBackColor = true;
            this.optSugaCode.CheckedChanged += new System.EventHandler(this.optSCheckedChanged);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(578, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Location = new System.Drawing.Point(506, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 27;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Location = new System.Drawing.Point(434, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 30);
            this.btnPrint.TabIndex = 22;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(657, 28);
            this.panTitleSub0.TabIndex = 12;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(300, 12);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "수입항목대 수가코드 변환정보 조회, 수정 및 출력";
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(657, 34);
            this.panTitle.TabIndex = 11;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(390, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "수입항목대 수가코드 변환정보 조회, 수정 및 출력";
            // 
            // frmWonCodeEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 693);
            this.Controls.Add(this.pan0);
            this.Name = "frmWonCodeEntry";
            this.Text = "수입항목대 수가코드 변환정보 조회, 수정 및 출력";
            this.Load += new System.EventHandler(this.frmWonCodeEntry_Load);
            this.pan0.ResumeLayout(false);
            this.pan2.ResumeLayout(false);
            this.pan4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.pan3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.grb.ResumeLayout(false);
            this.grb.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan0;
        private System.Windows.Forms.Panel pan2;
        private System.Windows.Forms.Panel pan4;
        private System.Windows.Forms.Panel pan3;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.ComboBox cboWongaList;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox grb;
        private System.Windows.Forms.RadioButton optWongaCode;
        private System.Windows.Forms.RadioButton optSugaCode;
    }
}