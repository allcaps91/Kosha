namespace ComPmpaLibB
{
    partial class frmPmpaViewDischargePatientList
    {
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.IContainer components = null;

	/// <summary>
	/// Clean up any resources being used.
	/// </summary>
	/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	protected override void Dispose ( bool disposing )
	{
	    if ( disposing && ( components != null ) )
	    {
		components . Dispose ( );
	    }
	    base . Dispose ( disposing );
	}

	#region Windows Form Designer generated code

	/// <summary>
	/// Required method for Designer support - do not modify
	/// the contents of this method with the code editor.
	/// </summary>
	private void InitializeComponent ( )
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
            FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
            this.label15 = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.dtpTdate = new System.Windows.Forms.DateTimePicker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rdoSORT2 = new System.Windows.Forms.RadioButton();
            this.rdoSORT1 = new System.Windows.Forms.RadioButton();
            this.rdoSORT0 = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.cboGubun = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rdoJob1 = new System.Windows.Forms.RadioButton();
            this.rdoJob0 = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(3, 5);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(112, 21);
            this.label15.TabIndex = 83;
            this.label15.Text = "퇴원환자 명부";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPrint
            // 
            this.btnPrint.AutoSize = true;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrint.Location = new System.Drawing.Point(1086, 0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 32);
            this.btnPrint.TabIndex = 80;
            this.btnPrint.Text = "인쇄";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = true;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(1082, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnView
            // 
            this.btnView.AutoSize = true;
            this.btnView.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnView.Location = new System.Drawing.Point(1014, 0);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(72, 32);
            this.btnView.TabIndex = 77;
            this.btnView.Text = "조회";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(4, 8);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(62, 12);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "조회 조건";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(5, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 22;
            this.label1.Text = "결과";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.label15);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1158, 34);
            this.panTitle.TabIndex = 98;
            // 
            // dtpTdate
            // 
            this.dtpTdate.CustomFormat = "yyyy-MM-dd";
            this.dtpTdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTdate.Location = new System.Drawing.Point(82, 6);
            this.dtpTdate.Name = "dtpTdate";
            this.dtpTdate.Size = new System.Drawing.Size(80, 21);
            this.dtpTdate.TabIndex = 86;
            this.dtpTdate.Leave += new System.EventHandler(this.dtpTdate_Leave);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 94);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1158, 28);
            this.panel2.TabIndex = 101;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.btnView);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1158, 32);
            this.panel1.TabIndex = 100;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.rdoSORT2);
            this.panel4.Controls.Add(this.rdoSORT1);
            this.panel4.Controls.Add(this.rdoSORT0);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.cboGubun);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(432, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(536, 32);
            this.panel4.TabIndex = 89;
            // 
            // rdoSORT2
            // 
            this.rdoSORT2.AutoSize = true;
            this.rdoSORT2.Location = new System.Drawing.Point(225, 8);
            this.rdoSORT2.Name = "rdoSORT2";
            this.rdoSORT2.Size = new System.Drawing.Size(79, 16);
            this.rdoSORT2.TabIndex = 94;
            this.rdoSORT2.Text = "과,의사 순";
            this.rdoSORT2.UseVisualStyleBackColor = true;
            // 
            // rdoSORT1
            // 
            this.rdoSORT1.AutoSize = true;
            this.rdoSORT1.Location = new System.Drawing.Point(138, 8);
            this.rdoSORT1.Name = "rdoSORT1";
            this.rdoSORT1.Size = new System.Drawing.Size(87, 16);
            this.rdoSORT1.TabIndex = 93;
            this.rdoSORT1.Text = "병동병실 순";
            this.rdoSORT1.UseVisualStyleBackColor = true;
            // 
            // rdoSORT0
            // 
            this.rdoSORT0.AutoSize = true;
            this.rdoSORT0.Checked = true;
            this.rdoSORT0.Location = new System.Drawing.Point(79, 8);
            this.rdoSORT0.Name = "rdoSORT0";
            this.rdoSORT0.Size = new System.Drawing.Size(59, 16);
            this.rdoSORT0.TabIndex = 92;
            this.rdoSORT0.TabStop = true;
            this.rdoSORT0.Text = "성명순";
            this.rdoSORT0.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(6, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 23);
            this.label5.TabIndex = 91;
            this.label5.Text = "Sort 순서";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboGubun
            // 
            this.cboGubun.FormattingEnabled = true;
            this.cboGubun.Location = new System.Drawing.Point(394, 8);
            this.cboGubun.Name = "cboGubun";
            this.cboGubun.Size = new System.Drawing.Size(121, 20);
            this.cboGubun.TabIndex = 90;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(315, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 23);
            this.label4.TabIndex = 89;
            this.label4.Text = "환자 구분";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rdoJob1);
            this.panel3.Controls.Add(this.rdoJob0);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.dtpTdate);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(432, 32);
            this.panel3.TabIndex = 88;
            // 
            // rdoJob1
            // 
            this.rdoJob1.AutoSize = true;
            this.rdoJob1.Location = new System.Drawing.Point(353, 8);
            this.rdoJob1.Name = "rdoJob1";
            this.rdoJob1.Size = new System.Drawing.Size(71, 16);
            this.rdoJob1.TabIndex = 90;
            this.rdoJob1.Text = "퇴원일자";
            this.rdoJob1.UseVisualStyleBackColor = true;
            // 
            // rdoJob0
            // 
            this.rdoJob0.AutoSize = true;
            this.rdoJob0.Checked = true;
            this.rdoJob0.Location = new System.Drawing.Point(266, 8);
            this.rdoJob0.Name = "rdoJob0";
            this.rdoJob0.Size = new System.Drawing.Size(87, 16);
            this.rdoJob0.TabIndex = 89;
            this.rdoJob0.TabStop = true;
            this.rdoJob0.Text = "퇴원 수납일";
            this.rdoJob0.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(193, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 23);
            this.label3.TabIndex = 88;
            this.label3.Text = "퇴원 구분";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 23);
            this.label2.TabIndex = 87;
            this.label2.Text = "퇴원 일자";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1158, 28);
            this.panTitleSub0.TabIndex = 99;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssView.Location = new System.Drawing.Point(0, 122);
            this.ssView.Name = "ssView";
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(1158, 454);
            this.ssView.TabIndex = 103;
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 16;
            this.ssView_Sheet1.RowCount = 1;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "병록번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "환자구분";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "피보성명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "관계";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "수진자명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "기관기호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "주민등록번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "과";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "호실";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "입원일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "퇴원일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "일수";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "의사명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "전화번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "퇴원종류";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "비   고";
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Label = "병록번호";
            this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(0).Width = 80F;
            this.ssView_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(1).Label = "환자구분";
            this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(2).Label = "피보성명";
            this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(3).Label = "관계";
            this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.ssView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(4).Label = "수진자명";
            this.ssView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.ssView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssView_Sheet1.Columns.Get(5).Label = "기관기호";
            this.ssView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.ssView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Label = "주민등록번호";
            this.ssView_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(6).Width = 120F;
            this.ssView_Sheet1.Columns.Get(7).CellType = textCellType8;
            this.ssView_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Label = "과";
            this.ssView_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(7).Width = 40F;
            this.ssView_Sheet1.Columns.Get(8).CellType = textCellType9;
            this.ssView_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).Label = "호실";
            this.ssView_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(8).Width = 40F;
            this.ssView_Sheet1.Columns.Get(9).CellType = textCellType10;
            this.ssView_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(9).Label = "입원일";
            this.ssView_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(9).Width = 80F;
            this.ssView_Sheet1.Columns.Get(10).CellType = textCellType11;
            this.ssView_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(10).Label = "퇴원일";
            this.ssView_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(10).Width = 80F;
            this.ssView_Sheet1.Columns.Get(11).CellType = textCellType12;
            this.ssView_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.ssView_Sheet1.Columns.Get(11).Label = "일수";
            this.ssView_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(12).CellType = textCellType13;
            this.ssView_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(12).Label = "의사명";
            this.ssView_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(13).CellType = textCellType14;
            this.ssView_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(13).Label = "전화번호";
            this.ssView_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(14).CellType = textCellType15;
            this.ssView_Sheet1.Columns.Get(14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(14).Label = "퇴원종류";
            this.ssView_Sheet1.Columns.Get(14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(15).CellType = textCellType16;
            this.ssView_Sheet1.Columns.Get(15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(15).Label = "비   고";
            this.ssView_Sheet1.Columns.Get(15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView_Sheet1.Columns.Get(15).Width = 120F;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmPmpaViewDischargePatientList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1158, 576);
            this.Controls.Add(this.ssView);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Name = "frmPmpaViewDischargePatientList";
            this.Text = "퇴원환자 명부";
            this.Load += new System.EventHandler(this.frmPmpaViewDischargePatientList_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.ResumeLayout(false);

	}

	#endregion
	private System . Windows . Forms . Label label15;
	private System . Windows . Forms . Button btnPrint;
	public System . Windows . Forms . Button btnExit;
	private System . Windows . Forms . Button btnView;
	private System . Windows . Forms . Label lblTitleSub0;
	private System . Windows . Forms . Label label1;
	private System . Windows . Forms . Panel panTitle;
	private System . Windows . Forms . DateTimePicker dtpTdate;
	private System . Windows . Forms . Panel panel2;
	private System . Windows . Forms . Panel panel1;
	private System . Windows . Forms . Panel panTitleSub0;
	private System . Windows . Forms . Label label2;
	private System . Windows . Forms . Panel panel4;
	private System . Windows . Forms . ComboBox cboGubun;
	private System . Windows . Forms . Label label4;
	private System . Windows . Forms . Panel panel3;
	private System . Windows . Forms . RadioButton rdoJob1;
	private System . Windows . Forms . RadioButton rdoJob0;
	private System . Windows . Forms . Label label3;
	private System . Windows . Forms . RadioButton rdoSORT2;
	private System . Windows . Forms . RadioButton rdoSORT1;
	private System . Windows . Forms . RadioButton rdoSORT0;
	private System . Windows . Forms . Label label5;
	private FarPoint . Win . Spread . FpSpread ssView;
	private FarPoint . Win . Spread . SheetView ssView_Sheet1;
    }
}