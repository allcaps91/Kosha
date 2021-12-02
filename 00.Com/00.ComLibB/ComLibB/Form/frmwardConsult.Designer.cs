namespace ComLibB
{
    partial class frmwardConsult
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType8 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType9 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType10 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.pan0 = new System.Windows.Forms.Panel();
            this.pan2 = new System.Windows.Forms.Panel();
            this.pan4 = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.pan3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.optrdodoctor = new System.Windows.Forms.RadioButton();
            this.optrdomedical = new System.Windows.Forms.RadioButton();
            this.optrdoWard = new System.Windows.Forms.RadioButton();
            this.grbDD = new System.Windows.Forms.GroupBox();
            this.dtpdd = new System.Windows.Forms.DateTimePicker();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.grbyyyy = new System.Windows.Forms.GroupBox();
            this.dtpyyyy = new System.Windows.Forms.DateTimePicker();
            this.chkAuto = new System.Windows.Forms.CheckBox();
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
            this.grbDD.SuspendLayout();
            this.grbyyyy.SuspendLayout();
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
            this.pan0.Size = new System.Drawing.Size(894, 443);
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
            this.pan2.Size = new System.Drawing.Size(894, 381);
            this.pan2.TabIndex = 13;
            // 
            // pan4
            // 
            this.pan4.BackColor = System.Drawing.Color.White;
            this.pan4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pan4.Controls.Add(this.ssView);
            this.pan4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pan4.Location = new System.Drawing.Point(0, 58);
            this.pan4.Name = "pan4";
            this.pan4.Size = new System.Drawing.Size(890, 319);
            this.pan4.TabIndex = 19;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, 건수-IU";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssView.Location = new System.Drawing.Point(0, 0);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(886, 315);
            this.ssView.TabIndex = 46;
            this.ssView.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 14;
            this.ssView_Sheet1.RowCount = 14;
            this.ssView_Sheet1.Cells.Get(0, 0).Value = "건수-IU";
            this.ssView_Sheet1.Cells.Get(1, 0).Value = "당일이내-IU";
            this.ssView_Sheet1.Cells.Get(2, 0).Value = "└>평균";
            this.ssView_Sheet1.Cells.Get(3, 0).Value = "당일이후-IU";
            this.ssView_Sheet1.Cells.Get(4, 0).Value = "└>평균";
            this.ssView_Sheet1.Cells.Get(5, 0).Value = "건수-일반";
            this.ssView_Sheet1.Cells.Get(6, 0).Value = "24시간 이내";
            this.ssView_Sheet1.Cells.Get(7, 0).Value = "└>평균";
            this.ssView_Sheet1.Cells.Get(8, 0).Value = "48시간 이내";
            this.ssView_Sheet1.Cells.Get(9, 0).Value = "└>평균";
            this.ssView_Sheet1.Cells.Get(10, 0).Value = "72시간 이내";
            this.ssView_Sheet1.Cells.Get(11, 0).Value = "└>평균";
            this.ssView_Sheet1.Cells.Get(12, 0).Value = "72시간 이후";
            this.ssView_Sheet1.Cells.Get(13, 0).Value = "└>평균";
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "항목";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "1월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "2월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "3월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "4월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "5월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "6월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "7월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "8월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "9월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "10월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "11월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "12월";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "합계";
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Label = "항목";
            this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Width = 98F;
            this.ssView_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Label = "1월";
            this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Label = "2월";
            this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Label = "3월";
            this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).Label = "4월";
            this.ssView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Label = "5월";
            this.ssView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.ssView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Label = "6월";
            this.ssView_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).CellType = textCellType8;
            this.ssView_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Label = "7월";
            this.ssView_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).CellType = textCellType9;
            this.ssView_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).Label = "8월";
            this.ssView_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(9).CellType = textCellType10;
            this.ssView_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(9).Label = "9월";
            this.ssView_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(10).CellType = textCellType11;
            this.ssView_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(10).Label = "10월";
            this.ssView_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(11).CellType = textCellType12;
            this.ssView_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(11).Label = "11월";
            this.ssView_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(12).CellType = textCellType13;
            this.ssView_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(12).Label = "12월";
            this.ssView_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(13).CellType = textCellType14;
            this.ssView_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(13).Label = "합계";
            this.ssView_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.RowHeader.Visible = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // pan3
            // 
            this.pan3.BackColor = System.Drawing.Color.White;
            this.pan3.Controls.Add(this.groupBox1);
            this.pan3.Controls.Add(this.grbDD);
            this.pan3.Controls.Add(this.btnExit);
            this.pan3.Controls.Add(this.btnSearch);
            this.pan3.Controls.Add(this.grbyyyy);
            this.pan3.Controls.Add(this.chkAuto);
            this.pan3.Dock = System.Windows.Forms.DockStyle.Top;
            this.pan3.Location = new System.Drawing.Point(0, 0);
            this.pan3.Name = "pan3";
            this.pan3.Size = new System.Drawing.Size(890, 58);
            this.pan3.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.optrdodoctor);
            this.groupBox1.Controls.Add(this.optrdomedical);
            this.groupBox1.Controls.Add(this.optrdoWard);
            this.groupBox1.Location = new System.Drawing.Point(175, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(175, 52);
            this.groupBox1.TabIndex = 43;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "조회 구분";
            // 
            // optrdodoctor
            // 
            this.optrdodoctor.AutoSize = true;
            this.optrdodoctor.Location = new System.Drawing.Point(125, 24);
            this.optrdodoctor.Name = "optrdodoctor";
            this.optrdodoctor.Size = new System.Drawing.Size(47, 16);
            this.optrdodoctor.TabIndex = 2;
            this.optrdodoctor.TabStop = true;
            this.optrdodoctor.Text = "의사";
            this.optrdodoctor.UseVisualStyleBackColor = true;
            // 
            // optrdomedical
            // 
            this.optrdomedical.AutoSize = true;
            this.optrdomedical.Location = new System.Drawing.Point(60, 24);
            this.optrdomedical.Name = "optrdomedical";
            this.optrdomedical.Size = new System.Drawing.Size(59, 16);
            this.optrdomedical.TabIndex = 1;
            this.optrdomedical.TabStop = true;
            this.optrdomedical.Text = "진료과";
            this.optrdomedical.UseVisualStyleBackColor = true;
            // 
            // optrdoWard
            // 
            this.optrdoWard.AutoSize = true;
            this.optrdoWard.Location = new System.Drawing.Point(7, 24);
            this.optrdoWard.Name = "optrdoWard";
            this.optrdoWard.Size = new System.Drawing.Size(47, 16);
            this.optrdoWard.TabIndex = 0;
            this.optrdoWard.TabStop = true;
            this.optrdoWard.Text = "병동";
            this.optrdoWard.UseVisualStyleBackColor = true;
            // 
            // grbDD
            // 
            this.grbDD.Controls.Add(this.dtpdd);
            this.grbDD.Location = new System.Drawing.Point(97, 3);
            this.grbDD.Name = "grbDD";
            this.grbDD.Size = new System.Drawing.Size(72, 52);
            this.grbDD.TabIndex = 42;
            this.grbDD.TabStop = false;
            this.grbDD.Text = "조회 월";
            // 
            // dtpdd
            // 
            this.dtpdd.CustomFormat = "mm";
            this.dtpdd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpdd.Location = new System.Drawing.Point(8, 20);
            this.dtpdd.Name = "dtpdd";
            this.dtpdd.Size = new System.Drawing.Size(54, 21);
            this.dtpdd.TabIndex = 46;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(815, 14);
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
            this.btnSearch.Location = new System.Drawing.Point(743, 14);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 27;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // grbyyyy
            // 
            this.grbyyyy.Controls.Add(this.dtpyyyy);
            this.grbyyyy.Location = new System.Drawing.Point(3, 3);
            this.grbyyyy.Name = "grbyyyy";
            this.grbyyyy.Size = new System.Drawing.Size(88, 52);
            this.grbyyyy.TabIndex = 41;
            this.grbyyyy.TabStop = false;
            this.grbyyyy.Text = "조회 년도";
            // 
            // dtpyyyy
            // 
            this.dtpyyyy.CustomFormat = "yyyy";
            this.dtpyyyy.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpyyyy.Location = new System.Drawing.Point(8, 20);
            this.dtpyyyy.Name = "dtpyyyy";
            this.dtpyyyy.Size = new System.Drawing.Size(67, 21);
            this.dtpyyyy.TabIndex = 45;
            // 
            // chkAuto
            // 
            this.chkAuto.AutoSize = true;
            this.chkAuto.Location = new System.Drawing.Point(356, 27);
            this.chkAuto.Name = "chkAuto";
            this.chkAuto.Size = new System.Drawing.Size(96, 16);
            this.chkAuto.TabIndex = 34;
            this.chkAuto.Text = "지표자동생성";
            this.chkAuto.UseVisualStyleBackColor = true;
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(894, 28);
            this.panTitleSub0.TabIndex = 12;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(117, 12);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "병동 Consult 조회";
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(894, 34);
            this.panTitle.TabIndex = 11;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(151, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "병동 Consult 조회";
            // 
            // frmwardConsult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 443);
            this.Controls.Add(this.pan0);
            this.Name = "frmwardConsult";
            this.Text = "병동 Consult 조회";
            this.Load += new System.EventHandler(this.frmwardConsult_Load);
            this.pan0.ResumeLayout(false);
            this.pan2.ResumeLayout(false);
            this.pan4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.pan3.ResumeLayout(false);
            this.pan3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grbDD.ResumeLayout(false);
            this.grbyyyy.ResumeLayout(false);
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
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.DateTimePicker dtpyyyy;
        private System.Windows.Forms.GroupBox grbyyyy;
        private System.Windows.Forms.CheckBox chkAuto;
        private System.Windows.Forms.Panel pan3;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton optrdodoctor;
        private System.Windows.Forms.RadioButton optrdomedical;
        private System.Windows.Forms.RadioButton optrdoWard;
        private System.Windows.Forms.GroupBox grbDD;
        private System.Windows.Forms.DateTimePicker dtpdd;
    }
}