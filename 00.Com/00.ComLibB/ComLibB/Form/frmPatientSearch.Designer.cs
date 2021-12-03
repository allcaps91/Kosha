namespace ComLibB
{
    partial class frmPatientSearch
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType23 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType24 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType25 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType26 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType27 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType28 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType29 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType30 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType31 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType32 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType33 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.panTitle = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnEMR = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panName = new System.Windows.Forms.Panel();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panJumin = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtJumin2 = new System.Windows.Forms.TextBox();
            this.txtJumin1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.optSearch2 = new System.Windows.Forms.RadioButton();
            this.optSearch1 = new System.Windows.Forms.RadioButton();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panNumber = new System.Windows.Forms.Panel();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.optSearch3 = new System.Windows.Forms.RadioButton();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panName.SuspendLayout();
            this.panJumin.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panNumber.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.label5);
            this.panTitle.Controls.Add(this.label3);
            this.panTitle.Controls.Add(this.btnEMR);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1179, 34);
            this.panTitle.TabIndex = 74;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.LightGreen;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(430, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 20);
            this.label5.TabIndex = 17;
            this.label5.Text = "감액대상 (협력업체제외)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Pink;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(271, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 20);
            this.label3.TabIndex = 16;
            this.label3.Text = "최근 30일 진료환자";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnEMR
            // 
            this.btnEMR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEMR.AutoSize = true;
            this.btnEMR.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnEMR.Location = new System.Drawing.Point(1001, 1);
            this.btnEMR.Name = "btnEMR";
            this.btnEMR.Size = new System.Drawing.Size(86, 30);
            this.btnEMR.TabIndex = 15;
            this.btnEMR.Text = "EMR 실행";
            this.btnEMR.UseVisualStyleBackColor = true;
            this.btnEMR.Click += new System.EventHandler(this.btnEMR_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSize = true;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(1093, 1);
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
            this.lblTitle.Size = new System.Drawing.Size(173, 16);
            this.lblTitle.TabIndex = 14;
            this.lblTitle.Text = "구환자 병록번호 조회";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panTitleSub0);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1179, 524);
            this.panel1.TabIndex = 75;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.ssView);
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 28);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1179, 496);
            this.panel4.TabIndex = 1;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, 99999999";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssView.Location = new System.Drawing.Point(0, 34);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(1179, 462);
            this.ssView.TabIndex = 1;
            this.ssView.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView_CellDoubleClick);
            this.ssView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ssView_KeyDown);
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 11;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.Cells.Get(0, 0).Value = "99999999";
            this.ssView_Sheet1.Cells.Get(0, 3).Value = "999999-9999999";
            this.ssView_Sheet1.Cells.Get(0, 6).Value = "9999-999-9999";
            this.ssView_Sheet1.Cells.Get(0, 7).Value = "9999-99-99";
            this.ssView_Sheet1.Cells.Get(0, 9).Value = "999-9999-9999";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "등록번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "수진자명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "성별";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "주민번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "보호자명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "환자구분";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "전화번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "최종내원";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "최종과";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "휴대폰";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "건강보험증번호";
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Get(0).CellType = textCellType23;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Label = "등록번호";
            this.ssView_Sheet1.Columns.Get(0).Locked = true;
            this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Width = 66F;
            this.ssView_Sheet1.Columns.Get(1).CellType = textCellType24;
            this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(1).Label = "수진자명";
            this.ssView_Sheet1.Columns.Get(1).Locked = true;
            this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Width = 73F;
            this.ssView_Sheet1.Columns.Get(2).CellType = textCellType25;
            this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Label = "성별";
            this.ssView_Sheet1.Columns.Get(2).Locked = true;
            this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Width = 42F;
            this.ssView_Sheet1.Columns.Get(3).CellType = textCellType26;
            this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Label = "주민번호";
            this.ssView_Sheet1.Columns.Get(3).Locked = true;
            this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Width = 101F;
            this.ssView_Sheet1.Columns.Get(4).CellType = textCellType27;
            this.ssView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(4).Label = "보호자명";
            this.ssView_Sheet1.Columns.Get(4).Locked = true;
            this.ssView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).Width = 68F;
            this.ssView_Sheet1.Columns.Get(5).CellType = textCellType28;
            this.ssView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            this.ssView_Sheet1.Columns.Get(5).Label = "환자구분";
            this.ssView_Sheet1.Columns.Get(5).Locked = true;
            this.ssView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).Width = 63F;
            this.ssView_Sheet1.Columns.Get(6).CellType = textCellType29;
            this.ssView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Label = "전화번호";
            this.ssView_Sheet1.Columns.Get(6).Locked = true;
            this.ssView_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Width = 96F;
            this.ssView_Sheet1.Columns.Get(7).AllowAutoSort = true;
            this.ssView_Sheet1.Columns.Get(7).CellType = textCellType30;
            this.ssView_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Label = "최종내원";
            this.ssView_Sheet1.Columns.Get(7).Locked = true;
            this.ssView_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Width = 82F;
            this.ssView_Sheet1.Columns.Get(8).CellType = textCellType31;
            this.ssView_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).Label = "최종과";
            this.ssView_Sheet1.Columns.Get(8).Locked = true;
            this.ssView_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).Width = 50F;
            this.ssView_Sheet1.Columns.Get(9).CellType = textCellType32;
            this.ssView_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(9).Label = "휴대폰";
            this.ssView_Sheet1.Columns.Get(9).Locked = true;
            this.ssView_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(9).Width = 94F;
            this.ssView_Sheet1.Columns.Get(10).CellType = textCellType33;
            this.ssView_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(10).Label = "건강보험증번호";
            this.ssView_Sheet1.Columns.Get(10).Locked = true;
            this.ssView_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(10).Width = 128F;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.optSearch3);
            this.panel2.Controls.Add(this.panNumber);
            this.panel2.Controls.Add(this.panName);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.panJumin);
            this.panel2.Controls.Add(this.optSearch2);
            this.panel2.Controls.Add(this.optSearch1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1179, 34);
            this.panel2.TabIndex = 0;
            // 
            // panName
            // 
            this.panName.Controls.Add(this.txtName);
            this.panName.Controls.Add(this.label4);
            this.panName.Location = new System.Drawing.Point(783, 4);
            this.panName.Name = "panName";
            this.panName.Size = new System.Drawing.Size(162, 27);
            this.panName.TabIndex = 17;
            // 
            // txtName
            // 
            this.txtName.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.txtName.Location = new System.Drawing.Point(69, 4);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(80, 21);
            this.txtName.TabIndex = 1;
            this.txtName.Enter += new System.EventHandler(this.txtName_Enter);
            this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "수진자명";
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.AutoSize = true;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearch.Location = new System.Drawing.Point(1095, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 30);
            this.btnSearch.TabIndex = 16;
            this.btnSearch.Text = "조  회";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panJumin
            // 
            this.panJumin.Controls.Add(this.label2);
            this.panJumin.Controls.Add(this.txtJumin2);
            this.panJumin.Controls.Add(this.txtJumin1);
            this.panJumin.Controls.Add(this.label1);
            this.panJumin.Location = new System.Drawing.Point(328, 2);
            this.panJumin.Name = "panJumin";
            this.panJumin.Size = new System.Drawing.Size(579, 27);
            this.panJumin.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(250, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(301, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "정확한 접수를 위해 주민번호를 끝까지 입력하십시오 !!";
            // 
            // txtJumin2
            // 
            this.txtJumin2.Location = new System.Drawing.Point(155, 4);
            this.txtJumin2.MaxLength = 7;
            this.txtJumin2.Name = "txtJumin2";
            this.txtJumin2.Size = new System.Drawing.Size(80, 21);
            this.txtJumin2.TabIndex = 2;
            this.txtJumin2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtJumin2_KeyDown);
            // 
            // txtJumin1
            // 
            this.txtJumin1.Location = new System.Drawing.Point(69, 4);
            this.txtJumin1.MaxLength = 6;
            this.txtJumin1.Name = "txtJumin1";
            this.txtJumin1.Size = new System.Drawing.Size(80, 21);
            this.txtJumin1.TabIndex = 1;
            this.txtJumin1.TextChanged += new System.EventHandler(this.txtJumin1_TextChanged);
            this.txtJumin1.Enter += new System.EventHandler(this.txtJumin1_Enter);
            this.txtJumin1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtJumin1_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "주민번호";
            // 
            // optSearch2
            // 
            this.optSearch2.AutoSize = true;
            this.optSearch2.Location = new System.Drawing.Point(94, 8);
            this.optSearch2.Name = "optSearch2";
            this.optSearch2.Size = new System.Drawing.Size(67, 16);
            this.optSearch2.TabIndex = 1;
            this.optSearch2.TabStop = true;
            this.optSearch2.Text = "성 명(&2)";
            this.optSearch2.UseVisualStyleBackColor = true;
            this.optSearch2.CheckedChanged += new System.EventHandler(this.optSearch2_CheckedChanged);
            // 
            // optSearch1
            // 
            this.optSearch1.AutoSize = true;
            this.optSearch1.Location = new System.Drawing.Point(8, 8);
            this.optSearch1.Name = "optSearch1";
            this.optSearch1.Size = new System.Drawing.Size(87, 16);
            this.optSearch1.TabIndex = 0;
            this.optSearch1.TabStop = true;
            this.optSearch1.Text = "주민번호(&1)";
            this.optSearch1.UseVisualStyleBackColor = true;
            this.optSearch1.CheckedChanged += new System.EventHandler(this.optSearch1_CheckedChanged);
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1179, 28);
            this.panTitleSub0.TabIndex = 0;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(12, 9);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(88, 12);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "병록번호 조회";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panNumber
            // 
            this.panNumber.Controls.Add(this.txtNumber);
            this.panNumber.Controls.Add(this.label6);
            this.panNumber.Location = new System.Drawing.Point(927, 6);
            this.panNumber.Name = "panNumber";
            this.panNumber.Size = new System.Drawing.Size(162, 27);
            this.panNumber.TabIndex = 18;
            // 
            // txtNumber
            // 
            this.txtNumber.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.txtNumber.Location = new System.Drawing.Point(69, 4);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(89, 21);
            this.txtNumber.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "전화번호";
            // 
            // optSearch3
            // 
            this.optSearch3.AutoSize = true;
            this.optSearch3.Location = new System.Drawing.Point(167, 7);
            this.optSearch3.Name = "optSearch3";
            this.optSearch3.Size = new System.Drawing.Size(87, 16);
            this.optSearch3.TabIndex = 19;
            this.optSearch3.TabStop = true;
            this.optSearch3.Text = "전화번호(&3)";
            this.optSearch3.UseVisualStyleBackColor = true;
            this.optSearch3.CheckedChanged += new System.EventHandler(this.optSearch3_CheckedChanged);
            // 
            // frmPatientSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1179, 558);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Name = "frmPatientSearch";
            this.Text = "구환자 병록번호 조회";
            this.Load += new System.EventHandler(this.frmPatientSearch_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panName.ResumeLayout(false);
            this.panName.PerformLayout();
            this.panJumin.ResumeLayout(false);
            this.panJumin.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panNumber.ResumeLayout(false);
            this.panNumber.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panJumin;
        private System.Windows.Forms.RadioButton optSearch2;
        private System.Windows.Forms.RadioButton optSearch1;
        private FarPoint.Win.Spread.FpSpread ssView;
        private FarPoint.Win.Spread.SheetView ssView_Sheet1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtJumin2;
        private System.Windows.Forms.TextBox txtJumin1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnEMR;
        private System.Windows.Forms.RadioButton optSearch3;
        private System.Windows.Forms.Panel panNumber;
        private System.Windows.Forms.TextBox txtNumber;
        private System.Windows.Forms.Label label6;
    }
}