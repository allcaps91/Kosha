namespace ComLibB
{
    partial class frmbugamf2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType4 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType5 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType6 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType7 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.pan0 = new System.Windows.Forms.Panel();
            this.pan1 = new System.Windows.Forms.Panel();
            this.pan3 = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pan2 = new System.Windows.Forms.Panel();
            this.grbin = new System.Windows.Forms.GroupBox();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.lblyyyy = new System.Windows.Forms.Label();
            this.grbSearch = new System.Windows.Forms.GroupBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.grbGubun = new System.Windows.Forms.GroupBox();
            this.optin = new System.Windows.Forms.RadioButton();
            this.optJumin = new System.Windows.Forms.RadioButton();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pan0.SuspendLayout();
            this.pan1.SuspendLayout();
            this.pan3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.pan2.SuspendLayout();
            this.grbin.SuspendLayout();
            this.grbSearch.SuspendLayout();
            this.grbGubun.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // pan0
            // 
            this.pan0.Controls.Add(this.pan1);
            this.pan0.Controls.Add(this.panTitleSub0);
            this.pan0.Controls.Add(this.panTitle);
            this.pan0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan0.Location = new System.Drawing.Point(0, 0);
            this.pan0.Name = "pan0";
            this.pan0.Size = new System.Drawing.Size(834, 583);
            this.pan0.TabIndex = 1;
            // 
            // pan1
            // 
            this.pan1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pan1.Controls.Add(this.pan3);
            this.pan1.Controls.Add(this.pan2);
            this.pan1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan1.Location = new System.Drawing.Point(0, 62);
            this.pan1.Name = "pan1";
            this.pan1.Size = new System.Drawing.Size(834, 521);
            this.pan1.TabIndex = 13;
            // 
            // pan3
            // 
            this.pan3.BackColor = System.Drawing.Color.White;
            this.pan3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pan3.Controls.Add(this.ssView);
            this.pan3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan3.Location = new System.Drawing.Point(0, 71);
            this.pan3.Name = "pan3";
            this.pan3.Size = new System.Drawing.Size(830, 446);
            this.pan3.TabIndex = 19;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.Location = new System.Drawing.Point(0, 0);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(826, 442);
            this.ssView.TabIndex = 46;
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 7;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "주민번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "감액부분";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "성명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "감액내용";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "등록번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "등록일자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "직업사번";
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(0).Label = "주민번호";
            this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Width = 160F;
            this.ssView_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Label = "감액부분";
            this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Width = 142F;
            this.ssView_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Label = "성명";
            this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Label = "감액내용";
            this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Width = 159F;
            this.ssView_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(4).Label = "등록번호";
            this.ssView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Label = "등록일자";
            this.ssView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Width = 122F;
            this.ssView_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.ssView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(6).Label = "직업사번";
            this.ssView_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pan2
            // 
            this.pan2.BackColor = System.Drawing.Color.White;
            this.pan2.Controls.Add(this.grbin);
            this.pan2.Controls.Add(this.grbSearch);
            this.pan2.Controls.Add(this.grbGubun);
            this.pan2.Controls.Add(this.btnExit);
            this.pan2.Controls.Add(this.btnSearch);
            this.pan2.Controls.Add(this.btnPrint);
            this.pan2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan2.Location = new System.Drawing.Point(0, 0);
            this.pan2.Name = "pan2";
            this.pan2.Size = new System.Drawing.Size(830, 71);
            this.pan2.TabIndex = 17;
            // 
            // grbin
            // 
            this.grbin.Controls.Add(this.dtpEnd);
            this.grbin.Controls.Add(this.dtpStart);
            this.grbin.Controls.Add(this.lblyyyy);
            this.grbin.Location = new System.Drawing.Point(218, 23);
            this.grbin.Name = "grbin";
            this.grbin.Size = new System.Drawing.Size(273, 43);
            this.grbin.TabIndex = 42;
            this.grbin.TabStop = false;
            this.grbin.Text = "등록일자";
            // 
            // dtpEnd
            // 
            this.dtpEnd.Location = new System.Drawing.Point(151, 17);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(116, 21);
            this.dtpEnd.TabIndex = 36;
            // 
            // dtpStart
            // 
            this.dtpStart.Location = new System.Drawing.Point(9, 17);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(116, 21);
            this.dtpStart.TabIndex = 35;
            // 
            // lblyyyy
            // 
            this.lblyyyy.AutoSize = true;
            this.lblyyyy.Location = new System.Drawing.Point(131, 22);
            this.lblyyyy.Name = "lblyyyy";
            this.lblyyyy.Size = new System.Drawing.Size(14, 12);
            this.lblyyyy.TabIndex = 34;
            this.lblyyyy.Text = "~";
            // 
            // grbSearch
            // 
            this.grbSearch.Controls.Add(this.txtSearch);
            this.grbSearch.Location = new System.Drawing.Point(96, 23);
            this.grbSearch.Name = "grbSearch";
            this.grbSearch.Size = new System.Drawing.Size(116, 43);
            this.grbSearch.TabIndex = 42;
            this.grbSearch.TabStop = false;
            this.grbSearch.Text = "검색";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(6, 17);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(100, 21);
            this.txtSearch.TabIndex = 32;
            this.txtSearch.EnabledChanged += new System.EventHandler(this.txtSearch_EnabledChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            this.txtSearch.Leave += new System.EventHandler(this.txtSearch_Leave);
            // 
            // grbGubun
            // 
            this.grbGubun.Controls.Add(this.optin);
            this.grbGubun.Controls.Add(this.optJumin);
            this.grbGubun.Location = new System.Drawing.Point(4, 4);
            this.grbGubun.Name = "grbGubun";
            this.grbGubun.Size = new System.Drawing.Size(86, 62);
            this.grbGubun.TabIndex = 42;
            this.grbGubun.TabStop = false;
            this.grbGubun.Text = "구분";
            // 
            // optin
            // 
            this.optin.AutoSize = true;
            this.optin.Location = new System.Drawing.Point(6, 39);
            this.optin.Name = "optin";
            this.optin.Size = new System.Drawing.Size(71, 16);
            this.optin.TabIndex = 39;
            this.optin.TabStop = true;
            this.optin.Text = "등록번호";
            this.optin.UseVisualStyleBackColor = true;
            // 
            // optJumin
            // 
            this.optJumin.AutoSize = true;
            this.optJumin.Location = new System.Drawing.Point(6, 17);
            this.optJumin.Name = "optJumin";
            this.optJumin.Size = new System.Drawing.Size(71, 16);
            this.optJumin.TabIndex = 38;
            this.optJumin.TabStop = true;
            this.optJumin.Text = "주민번호";
            this.optJumin.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(755, 3);
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
            this.btnSearch.Location = new System.Drawing.Point(683, 3);
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
            this.btnPrint.Location = new System.Drawing.Point(611, 3);
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
            this.panTitleSub0.Size = new System.Drawing.Size(834, 28);
            this.panTitleSub0.TabIndex = 12;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(127, 12);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "성직자감액등록 내역";
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(834, 34);
            this.panTitle.TabIndex = 11;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(167, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "성직자감액등록 내역";
            // 
            // frmbugamf2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 583);
            this.Controls.Add(this.pan0);
            this.Name = "frmbugamf2";
            this.Text = "성직자감액등록 내역";
            this.Load += new System.EventHandler(this.frmbugamf2_Load);
            this.pan0.ResumeLayout(false);
            this.pan1.ResumeLayout(false);
            this.pan3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.pan2.ResumeLayout(false);
            this.grbin.ResumeLayout(false);
            this.grbin.PerformLayout();
            this.grbSearch.ResumeLayout(false);
            this.grbSearch.PerformLayout();
            this.grbGubun.ResumeLayout(false);
            this.grbGubun.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan0;
        private System.Windows.Forms.Panel pan1;
        private System.Windows.Forms.Panel pan3;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.Panel pan2;
        private System.Windows.Forms.GroupBox grbin;
        private System.Windows.Forms.Label lblyyyy;
        private System.Windows.Forms.GroupBox grbSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.GroupBox grbGubun;
        private System.Windows.Forms.RadioButton optin;
        private System.Windows.Forms.RadioButton optJumin;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.DateTimePicker dtpStart;
    }
}