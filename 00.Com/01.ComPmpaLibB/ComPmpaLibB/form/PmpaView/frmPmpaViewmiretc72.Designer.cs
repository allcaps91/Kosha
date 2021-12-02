namespace ComPmpaLibB
{
    partial class frmPmpaViewmiretc72
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
	    this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
	    this.label3 = new System.Windows.Forms.Label();
	    this.lblyyyymm = new System.Windows.Forms.Label();
	    this.panel3 = new System.Windows.Forms.Panel();
	    this.lbl = new System.Windows.Forms.Label();
	    this.dtpTdate = new System.Windows.Forms.DateTimePicker();
	    this.cboDrCode = new System.Windows.Forms.ComboBox();
	    this.lblDept = new System.Windows.Forms.Label();
	    this.cboDept = new System.Windows.Forms.ComboBox();
	    this.lblDrCode = new System.Windows.Forms.Label();
	    this.cboBi = new System.Windows.Forms.ComboBox();
	    this.dtpFdate = new System.Windows.Forms.DateTimePicker();
	    this.panTitleSub0 = new System.Windows.Forms.Panel();
	    this.lblTitleSub0 = new System.Windows.Forms.Label();
	    this.lblyyyy = new System.Windows.Forms.Panel();
	    this.btnView = new System.Windows.Forms.Button();
	    this.btnPrint = new System.Windows.Forms.Button();
	    this.panTitle = new System.Windows.Forms.Panel();
	    this.label15 = new System.Windows.Forms.Label();
	    this.btnExit = new System.Windows.Forms.Button();
	    this.label1 = new System.Windows.Forms.Label();
	    this.ssView = new FarPoint.Win.Spread.FpSpread();
	    this.panel2 = new System.Windows.Forms.Panel();
	    ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
	    this.panel3.SuspendLayout();
	    this.panTitleSub0.SuspendLayout();
	    this.lblyyyy.SuspendLayout();
	    this.panTitle.SuspendLayout();
	    ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
	    this.panel2.SuspendLayout();
	    this.SuspendLayout();
	    // 
	    // ssView_Sheet1
	    // 
	    this.ssView_Sheet1.Reset();
	    this.ssView_Sheet1.SheetName = "Sheet1";
	    // Formulas and custom names must be loaded with R1C1 reference style
	    this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
	    this.ssView_Sheet1.ColumnCount = 12;
	    this.ssView_Sheet1.RowCount = 1;
	    this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
	    this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
	    this.ssView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
	    this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "전산퇴원일";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "등록번호";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "성명";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "자격";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "입원일";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "퇴원일";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "진료기간";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "진료과";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "의사";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "병동";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "호실";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "총진료비";
	    this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
	    this.ssView_Sheet1.Columns.Get(0).CellType = textCellType1;
	    this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(0).Label = "전산퇴원일";
	    this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(0).Width = 100F;
	    this.ssView_Sheet1.Columns.Get(1).CellType = textCellType2;
	    this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(1).Label = "등록번호";
	    this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(1).Width = 100F;
	    this.ssView_Sheet1.Columns.Get(2).CellType = textCellType3;
	    this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(2).Label = "성명";
	    this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(2).Width = 80F;
	    this.ssView_Sheet1.Columns.Get(3).CellType = textCellType4;
	    this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(3).Label = "자격";
	    this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(3).Width = 40F;
	    this.ssView_Sheet1.Columns.Get(4).CellType = textCellType5;
	    this.ssView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(4).Label = "입원일";
	    this.ssView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(4).Width = 100F;
	    this.ssView_Sheet1.Columns.Get(5).CellType = textCellType6;
	    this.ssView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(5).Label = "퇴원일";
	    this.ssView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(5).Width = 100F;
	    this.ssView_Sheet1.Columns.Get(6).CellType = textCellType7;
	    this.ssView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
	    this.ssView_Sheet1.Columns.Get(6).Label = "진료기간";
	    this.ssView_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(6).Width = 40F;
	    this.ssView_Sheet1.Columns.Get(7).CellType = textCellType8;
	    this.ssView_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(7).Label = "진료과";
	    this.ssView_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(7).Width = 50F;
	    this.ssView_Sheet1.Columns.Get(8).CellType = textCellType9;
	    this.ssView_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(8).Label = "의사";
	    this.ssView_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(9).CellType = textCellType10;
	    this.ssView_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(9).Label = "병동";
	    this.ssView_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(9).Width = 40F;
	    this.ssView_Sheet1.Columns.Get(10).CellType = textCellType11;
	    this.ssView_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(10).Label = "호실";
	    this.ssView_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(10).Width = 40F;
	    this.ssView_Sheet1.Columns.Get(11).CellType = textCellType12;
	    this.ssView_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
	    this.ssView_Sheet1.Columns.Get(11).Label = "총진료비";
	    this.ssView_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(11).Width = 100F;
	    this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
	    this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
	    this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
	    this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
	    this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
	    this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
	    // 
	    // label3
	    // 
	    this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
	    this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
	    this.label3.ForeColor = System.Drawing.Color.Black;
	    this.label3.Location = new System.Drawing.Point(262, 5);
	    this.label3.Name = "label3";
	    this.label3.Size = new System.Drawing.Size(71, 23);
	    this.label3.TabIndex = 88;
	    this.label3.Text = "보험 자격";
	    this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
	    // 
	    // lblyyyymm
	    // 
	    this.lblyyyymm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
	    this.lblyyyymm.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
	    this.lblyyyymm.ForeColor = System.Drawing.Color.Black;
	    this.lblyyyymm.Location = new System.Drawing.Point(3, 5);
	    this.lblyyyymm.Name = "lblyyyymm";
	    this.lblyyyymm.Size = new System.Drawing.Size(71, 23);
	    this.lblyyyymm.TabIndex = 87;
	    this.lblyyyymm.Text = "퇴원 일자";
	    this.lblyyyymm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
	    // 
	    // panel3
	    // 
	    this.panel3.Controls.Add(this.lbl);
	    this.panel3.Controls.Add(this.dtpTdate);
	    this.panel3.Controls.Add(this.cboDrCode);
	    this.panel3.Controls.Add(this.lblDept);
	    this.panel3.Controls.Add(this.cboDept);
	    this.panel3.Controls.Add(this.lblDrCode);
	    this.panel3.Controls.Add(this.cboBi);
	    this.panel3.Controls.Add(this.label3);
	    this.panel3.Controls.Add(this.lblyyyymm);
	    this.panel3.Controls.Add(this.dtpFdate);
	    this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
	    this.panel3.Location = new System.Drawing.Point(0, 0);
	    this.panel3.Name = "panel3";
	    this.panel3.Size = new System.Drawing.Size(872, 32);
	    this.panel3.TabIndex = 88;
	    // 
	    // lbl
	    // 
	    this.lbl.AutoSize = true;
	    this.lbl.Location = new System.Drawing.Point(162, 10);
	    this.lbl.Name = "lbl";
	    this.lbl.Size = new System.Drawing.Size(14, 12);
	    this.lbl.TabIndex = 98;
	    this.lbl.Text = "~";
	    // 
	    // dtpTdate
	    // 
	    this.dtpTdate.CustomFormat = "yyyy-MM-dd";
	    this.dtpTdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
	    this.dtpTdate.Location = new System.Drawing.Point(176, 6);
	    this.dtpTdate.Name = "dtpTdate";
	    this.dtpTdate.Size = new System.Drawing.Size(80, 21);
	    this.dtpTdate.TabIndex = 97;
	    // 
	    // cboDrCode
	    // 
	    this.cboDrCode.FormattingEnabled = true;
	    this.cboDrCode.Location = new System.Drawing.Point(747, 6);
	    this.cboDrCode.Name = "cboDrCode";
	    this.cboDrCode.Size = new System.Drawing.Size(121, 20);
	    this.cboDrCode.TabIndex = 96;
	    // 
	    // lblDept
	    // 
	    this.lblDept.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
	    this.lblDept.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
	    this.lblDept.ForeColor = System.Drawing.Color.Black;
	    this.lblDept.Location = new System.Drawing.Point(466, 5);
	    this.lblDept.Name = "lblDept";
	    this.lblDept.Size = new System.Drawing.Size(71, 23);
	    this.lblDept.TabIndex = 95;
	    this.lblDept.Text = "진료과";
	    this.lblDept.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
	    // 
	    // cboDept
	    // 
	    this.cboDept.FormattingEnabled = true;
	    this.cboDept.Location = new System.Drawing.Point(543, 6);
	    this.cboDept.Name = "cboDept";
	    this.cboDept.Size = new System.Drawing.Size(121, 20);
	    this.cboDept.TabIndex = 94;
	    this.cboDept.Click += new System.EventHandler(this.cboDept_Click);
	    // 
	    // lblDrCode
	    // 
	    this.lblDrCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
	    this.lblDrCode.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
	    this.lblDrCode.ForeColor = System.Drawing.Color.Black;
	    this.lblDrCode.Location = new System.Drawing.Point(670, 5);
	    this.lblDrCode.Name = "lblDrCode";
	    this.lblDrCode.Size = new System.Drawing.Size(71, 23);
	    this.lblDrCode.TabIndex = 93;
	    this.lblDrCode.Text = "진료 의사";
	    this.lblDrCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
	    // 
	    // cboBi
	    // 
	    this.cboBi.FormattingEnabled = true;
	    this.cboBi.Location = new System.Drawing.Point(339, 6);
	    this.cboBi.Name = "cboBi";
	    this.cboBi.Size = new System.Drawing.Size(121, 20);
	    this.cboBi.TabIndex = 91;
	    // 
	    // dtpFdate
	    // 
	    this.dtpFdate.CustomFormat = "yyyy-MM-dd";
	    this.dtpFdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
	    this.dtpFdate.Location = new System.Drawing.Point(82, 6);
	    this.dtpFdate.Name = "dtpFdate";
	    this.dtpFdate.Size = new System.Drawing.Size(80, 21);
	    this.dtpFdate.TabIndex = 86;
	    // 
	    // panTitleSub0
	    // 
	    this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
	    this.panTitleSub0.Controls.Add(this.lblTitleSub0);
	    this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
	    this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
	    this.panTitleSub0.Name = "panTitleSub0";
	    this.panTitleSub0.Size = new System.Drawing.Size(1019, 28);
	    this.panTitleSub0.TabIndex = 105;
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
	    // lblyyyy
	    // 
	    this.lblyyyy.BackColor = System.Drawing.SystemColors.Window;
	    this.lblyyyy.Controls.Add(this.panel3);
	    this.lblyyyy.Controls.Add(this.btnView);
	    this.lblyyyy.Controls.Add(this.btnPrint);
	    this.lblyyyy.Dock = System.Windows.Forms.DockStyle.Top;
	    this.lblyyyy.Location = new System.Drawing.Point(0, 62);
	    this.lblyyyy.Name = "lblyyyy";
	    this.lblyyyy.Size = new System.Drawing.Size(1019, 32);
	    this.lblyyyy.TabIndex = 106;
	    // 
	    // btnView
	    // 
	    this.btnView.AutoSize = true;
	    this.btnView.Dock = System.Windows.Forms.DockStyle.Right;
	    this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
	    this.btnView.Location = new System.Drawing.Point(875, 0);
	    this.btnView.Name = "btnView";
	    this.btnView.Size = new System.Drawing.Size(72, 32);
	    this.btnView.TabIndex = 77;
	    this.btnView.Text = "조회";
	    this.btnView.UseVisualStyleBackColor = true;
	    this.btnView.Click += new System.EventHandler(this.btnView_Click);
	    // 
	    // btnPrint
	    // 
	    this.btnPrint.AutoSize = true;
	    this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
	    this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
	    this.btnPrint.Location = new System.Drawing.Point(947, 0);
	    this.btnPrint.Name = "btnPrint";
	    this.btnPrint.Size = new System.Drawing.Size(72, 32);
	    this.btnPrint.TabIndex = 80;
	    this.btnPrint.Text = "인쇄";
	    this.btnPrint.UseVisualStyleBackColor = true;
	    this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
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
	    this.panTitle.Size = new System.Drawing.Size(1019, 34);
	    this.panTitle.TabIndex = 104;
	    // 
	    // label15
	    // 
	    this.label15.AutoSize = true;
	    this.label15.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
	    this.label15.ForeColor = System.Drawing.Color.Black;
	    this.label15.Location = new System.Drawing.Point(3, 5);
	    this.label15.Name = "label15";
	    this.label15.Size = new System.Drawing.Size(198, 21);
	    this.label15.TabIndex = 83;
	    this.label15.Text = "퇴원자명단조회(총진료비)";
	    this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
	    // 
	    // btnExit
	    // 
	    this.btnExit.AutoSize = true;
	    this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
	    this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
	    this.btnExit.Location = new System.Drawing.Point(943, 0);
	    this.btnExit.Name = "btnExit";
	    this.btnExit.Size = new System.Drawing.Size(72, 30);
	    this.btnExit.TabIndex = 15;
	    this.btnExit.Text = "닫기";
	    this.btnExit.UseVisualStyleBackColor = true;
	    this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
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
	    // ssView
	    // 
	    this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
	    this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
	    this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
	    this.ssView.Location = new System.Drawing.Point(0, 122);
	    this.ssView.Name = "ssView";
	    this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
	    this.ssView.Size = new System.Drawing.Size(1019, 455);
	    this.ssView.TabIndex = 108;
	    this.ssView.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.ssView_CellDoubleClick);
	    // 
	    // panel2
	    // 
	    this.panel2.BackColor = System.Drawing.Color.RoyalBlue;
	    this.panel2.Controls.Add(this.label1);
	    this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
	    this.panel2.Location = new System.Drawing.Point(0, 94);
	    this.panel2.Name = "panel2";
	    this.panel2.Size = new System.Drawing.Size(1019, 28);
	    this.panel2.TabIndex = 107;
	    // 
	    // frmPmpaViewmiretc72
	    // 
	    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
	    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	    this.ClientSize = new System.Drawing.Size(1019, 577);
	    this.Controls.Add(this.ssView);
	    this.Controls.Add(this.panel2);
	    this.Controls.Add(this.lblyyyy);
	    this.Controls.Add(this.panTitleSub0);
	    this.Controls.Add(this.panTitle);
	    this.Name = "frmPmpaViewmiretc72";
	    this.Text = "퇴원자명단조회(총진료비)";
	    this.Load += new System.EventHandler(this.frmPmpaViewmiretc72_Load);
	    ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
	    this.panel3.ResumeLayout(false);
	    this.panel3.PerformLayout();
	    this.panTitleSub0.ResumeLayout(false);
	    this.panTitleSub0.PerformLayout();
	    this.lblyyyy.ResumeLayout(false);
	    this.lblyyyy.PerformLayout();
	    this.panTitle.ResumeLayout(false);
	    this.panTitle.PerformLayout();
	    ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
	    this.panel2.ResumeLayout(false);
	    this.panel2.PerformLayout();
	    this.ResumeLayout(false);

	}

	#endregion

	private FarPoint . Win . Spread . SheetView ssView_Sheet1;
	private System . Windows . Forms . Label label3;
	private System . Windows . Forms . Label lblyyyymm;
	private System . Windows . Forms . Panel panel3;
	private System . Windows . Forms . DateTimePicker dtpFdate;
	private System . Windows . Forms . Panel panTitleSub0;
	private System . Windows . Forms . Label lblTitleSub0;
	private System . Windows . Forms . Panel lblyyyy;
	private System . Windows . Forms . Button btnView;
	private System . Windows . Forms . Button btnPrint;
	private System . Windows . Forms . Panel panTitle;
	private System . Windows . Forms . Label label15;
	public System . Windows . Forms . Button btnExit;
	private System . Windows . Forms . Label label1;
	private FarPoint . Win . Spread . FpSpread ssView;
	private System . Windows . Forms . Panel panel2;
	private System . Windows . Forms . ComboBox cboBi;
	private System . Windows . Forms . Label lbl;
	private System . Windows . Forms . DateTimePicker dtpTdate;
	private System . Windows . Forms . ComboBox cboDrCode;
	private System . Windows . Forms . Label lblDept;
	private System . Windows . Forms . ComboBox cboDept;
	private System . Windows . Forms . Label lblDrCode;
    }
}