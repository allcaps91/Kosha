namespace ComPmpaLibB
{
    partial class frmPmPaAdmistion1STS
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color341636409395090809444", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text453636409395090999519", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static557636409395091109541");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static701636409395091469696");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panmain = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.label1 = new System.Windows.Forms.Label();
            this.panSSmain = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.panTitleSub1 = new System.Windows.Forms.Panel();
            this.panSub = new System.Windows.Forms.Panel();
            this.chkBun = new System.Windows.Forms.CheckBox();
            this.lblttdd = new System.Windows.Forms.Label();
            this.dtpTdate = new System.Windows.Forms.DateTimePicker();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.cboJohap = new System.Windows.Forms.ComboBox();
            this.cbobi = new System.Windows.Forms.Label();
            this.lbldoc = new System.Windows.Forms.Label();
            this.btnView = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblyyyy = new System.Windows.Forms.Panel();
            this.btncancel = new System.Windows.Forms.Button();
            this.panmain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.panSSmain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            this.panTitleSub1.SuspendLayout();
            this.panSub.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.lblyyyy.SuspendLayout();
            this.SuspendLayout();
            // 
            // panmain
            // 
            this.panmain.BackColor = System.Drawing.Color.RoyalBlue;
            this.panmain.Controls.Add(this.lblTitleSub0);
            this.panmain.Dock = System.Windows.Forms.DockStyle.Top;
            this.panmain.Location = new System.Drawing.Point(0, 34);
            this.panmain.Name = "panmain";
            this.panmain.Size = new System.Drawing.Size(1012, 28);
            this.panmain.TabIndex = 196;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(4, 6);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(34, 17);
            this.lblTitleSub0.TabIndex = 22;
            this.lblTitleSub0.Text = "조건";
            this.lblTitleSub0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 11;
            this.ssView_Sheet1.RowCount = 0;
            this.ssView_Sheet1.ActiveColumnIndex = -1;
            this.ssView_Sheet1.ActiveRowIndex = -1;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.Width = 72F;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "피보험자명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "관계";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "수진자명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "주민번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "상   병   명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "진료과";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "입원일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "퇴원일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "병실";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "전화번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "비     고";
            this.ssView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Default.Width = 72F;
            this.ssView_Sheet1.Columns.Get(0).Label = "피보험자명";
            this.ssView_Sheet1.Columns.Get(0).StyleName = "Static557636409395091109541";
            this.ssView_Sheet1.Columns.Get(0).Width = 81F;
            this.ssView_Sheet1.Columns.Get(1).Label = "관계";
            this.ssView_Sheet1.Columns.Get(1).StyleName = "Static557636409395091109541";
            this.ssView_Sheet1.Columns.Get(1).Width = 46F;
            this.ssView_Sheet1.Columns.Get(2).Label = "수진자명";
            this.ssView_Sheet1.Columns.Get(2).StyleName = "Static557636409395091109541";
            this.ssView_Sheet1.Columns.Get(2).Width = 75F;
            this.ssView_Sheet1.Columns.Get(3).Label = "주민번호";
            this.ssView_Sheet1.Columns.Get(3).StyleName = "Static557636409395091109541";
            this.ssView_Sheet1.Columns.Get(3).Width = 117F;
            this.ssView_Sheet1.Columns.Get(4).Label = "상   병   명";
            this.ssView_Sheet1.Columns.Get(4).StyleName = "Static701636409395091469696";
            this.ssView_Sheet1.Columns.Get(4).Width = 312F;
            this.ssView_Sheet1.Columns.Get(5).Label = "진료과";
            this.ssView_Sheet1.Columns.Get(5).StyleName = "Static557636409395091109541";
            this.ssView_Sheet1.Columns.Get(5).Width = 75F;
            this.ssView_Sheet1.Columns.Get(6).Label = "입원일";
            this.ssView_Sheet1.Columns.Get(6).StyleName = "Static557636409395091109541";
            this.ssView_Sheet1.Columns.Get(6).Width = 80F;
            this.ssView_Sheet1.Columns.Get(7).Label = "퇴원일";
            this.ssView_Sheet1.Columns.Get(7).StyleName = "Static557636409395091109541";
            this.ssView_Sheet1.Columns.Get(7).Width = 80F;
            this.ssView_Sheet1.Columns.Get(8).Label = "병실";
            this.ssView_Sheet1.Columns.Get(8).StyleName = "Static557636409395091109541";
            this.ssView_Sheet1.Columns.Get(8).Width = 42F;
            this.ssView_Sheet1.Columns.Get(9).Label = "전화번호";
            this.ssView_Sheet1.Columns.Get(9).Width = 85F;
            this.ssView_Sheet1.Columns.Get(10).Label = "비     고";
            this.ssView_Sheet1.Columns.Get(10).StyleName = "Static701636409395091469696";
            this.ssView_Sheet1.Columns.Get(10).Width = 158F;
            this.ssView_Sheet1.DefaultStyleName = "Text453636409395090999519";
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.RowHeader.DefaultStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ssView_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssView_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.Rows.Default.Height = 19F;
            this.ssView_Sheet1.SheetCornerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ssView_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssView_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssView_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(4, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 17);
            this.label1.TabIndex = 22;
            this.label1.Text = "결과";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panSSmain
            // 
            this.panSSmain.Controls.Add(this.ssView);
            this.panSSmain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSSmain.Location = new System.Drawing.Point(0, 122);
            this.panSSmain.Name = "panSSmain";
            this.panSSmain.Size = new System.Drawing.Size(1012, 403);
            this.panSSmain.TabIndex = 199;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssView.Location = new System.Drawing.Point(0, 0);
            this.ssView.Name = "ssView";
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 60;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType2.Static = true;
            namedStyle3.CellType = textCellType2;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = textCellType2;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.Static = true;
            namedStyle4.CellType = textCellType3;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType3;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4});
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(1012, 403);
            this.ssView.TabIndex = 166;
            this.ssView.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssView.TextTipAppearance = tipAppearance1;
            // 
            // panTitleSub1
            // 
            this.panTitleSub1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub1.Controls.Add(this.label1);
            this.panTitleSub1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub1.Location = new System.Drawing.Point(0, 94);
            this.panTitleSub1.Name = "panTitleSub1";
            this.panTitleSub1.Size = new System.Drawing.Size(1012, 28);
            this.panTitleSub1.TabIndex = 198;
            // 
            // panSub
            // 
            this.panSub.Controls.Add(this.chkBun);
            this.panSub.Controls.Add(this.lblttdd);
            this.panSub.Controls.Add(this.dtpTdate);
            this.panSub.Controls.Add(this.dtpFDate);
            this.panSub.Controls.Add(this.cboJohap);
            this.panSub.Controls.Add(this.cbobi);
            this.panSub.Controls.Add(this.lbldoc);
            this.panSub.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub.Location = new System.Drawing.Point(0, 0);
            this.panSub.Name = "panSub";
            this.panSub.Size = new System.Drawing.Size(693, 32);
            this.panSub.TabIndex = 90;
            // 
            // chkBun
            // 
            this.chkBun.AutoSize = true;
            this.chkBun.Location = new System.Drawing.Point(430, 8);
            this.chkBun.Name = "chkBun";
            this.chkBun.Size = new System.Drawing.Size(88, 16);
            this.chkBun.TabIndex = 176;
            this.chkBun.Text = "퇴원자 제외";
            this.chkBun.UseVisualStyleBackColor = true;
            // 
            // lblttdd
            // 
            this.lblttdd.AutoSize = true;
            this.lblttdd.Location = new System.Drawing.Point(155, 10);
            this.lblttdd.Name = "lblttdd";
            this.lblttdd.Size = new System.Drawing.Size(11, 12);
            this.lblttdd.TabIndex = 175;
            this.lblttdd.Text = "-";
            // 
            // dtpTdate
            // 
            this.dtpTdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTdate.Location = new System.Drawing.Point(166, 6);
            this.dtpTdate.Name = "dtpTdate";
            this.dtpTdate.Size = new System.Drawing.Size(85, 21);
            this.dtpTdate.TabIndex = 174;
            // 
            // dtpFDate
            // 
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(70, 6);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(85, 21);
            this.dtpFDate.TabIndex = 173;
            // 
            // cboJohap
            // 
            this.cboJohap.FormattingEnabled = true;
            this.cboJohap.Location = new System.Drawing.Point(315, 6);
            this.cboJohap.Name = "cboJohap";
            this.cboJohap.Size = new System.Drawing.Size(99, 20);
            this.cboJohap.TabIndex = 170;
            // 
            // cbobi
            // 
            this.cbobi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cbobi.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbobi.ForeColor = System.Drawing.Color.Black;
            this.cbobi.Location = new System.Drawing.Point(257, 5);
            this.cbobi.Name = "cbobi";
            this.cbobi.Size = new System.Drawing.Size(57, 23);
            this.cbobi.TabIndex = 169;
            this.cbobi.Text = "조합구분";
            this.cbobi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbldoc
            // 
            this.lbldoc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbldoc.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbldoc.ForeColor = System.Drawing.Color.Black;
            this.lbldoc.Location = new System.Drawing.Point(12, 5);
            this.lbldoc.Name = "lbldoc";
            this.lbldoc.Size = new System.Drawing.Size(57, 23);
            this.lbldoc.TabIndex = 96;
            this.lbldoc.Text = "작업기간";
            this.lbldoc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnView
            // 
            this.btnView.AutoSize = true;
            this.btnView.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnView.Location = new System.Drawing.Point(796, 0);
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
            this.btnPrint.Location = new System.Drawing.Point(940, 0);
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
            this.btnExit.Location = new System.Drawing.Point(936, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btncansel_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(3, 5);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(258, 21);
            this.label15.TabIndex = 83;
            this.label15.Text = "조합용 수진자 현황(지역코드기준)";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.panTitle.Size = new System.Drawing.Size(1012, 34);
            this.panTitle.TabIndex = 195;
            // 
            // lblyyyy
            // 
            this.lblyyyy.BackColor = System.Drawing.SystemColors.Window;
            this.lblyyyy.Controls.Add(this.btnView);
            this.lblyyyy.Controls.Add(this.btncancel);
            this.lblyyyy.Controls.Add(this.panSub);
            this.lblyyyy.Controls.Add(this.btnPrint);
            this.lblyyyy.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblyyyy.Location = new System.Drawing.Point(0, 62);
            this.lblyyyy.Name = "lblyyyy";
            this.lblyyyy.Size = new System.Drawing.Size(1012, 32);
            this.lblyyyy.TabIndex = 197;
            // 
            // btncancel
            // 
            this.btncancel.AutoSize = true;
            this.btncancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btncancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btncancel.Location = new System.Drawing.Point(868, 0);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(72, 32);
            this.btncancel.TabIndex = 91;
            this.btncancel.Text = "취소";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // frmPmPaAdmistion1STS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 525);
            this.Controls.Add(this.panSSmain);
            this.Controls.Add(this.panTitleSub1);
            this.Controls.Add(this.lblyyyy);
            this.Controls.Add(this.panmain);
            this.Controls.Add(this.panTitle);
            this.Name = "frmPmPaAdmistion1STS";
            this.Text = "조합용 수진자 현황(지역코드기준)";
            this.Load += new System.EventHandler(this.frmPmPaAdmistion1STS_Load);
            this.panmain.ResumeLayout(false);
            this.panmain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.panSSmain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            this.panTitleSub1.ResumeLayout(false);
            this.panTitleSub1.PerformLayout();
            this.panSub.ResumeLayout(false);
            this.panSub.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.lblyyyy.ResumeLayout(false);
            this.lblyyyy.PerformLayout();
            this.ResumeLayout(false);

	}

	#endregion

	private System . Windows . Forms . Panel panmain;
	private System . Windows . Forms . Label lblTitleSub0;
	private FarPoint . Win . Spread . SheetView ssView_Sheet1;
	private System . Windows . Forms . Label label1;
	private System . Windows . Forms . Panel panSSmain;
	private FarPoint . Win . Spread . FpSpread ssView;
	private System . Windows . Forms . Panel panTitleSub1;
	private System . Windows . Forms . Panel panSub;
	private System . Windows . Forms . ComboBox cboJohap;
	private System . Windows . Forms . Label cbobi;
	private System . Windows . Forms . Label lbldoc;
	private System . Windows . Forms . Button btnView;
	private System . Windows . Forms . Button btnPrint;
	public System . Windows . Forms . Button btnExit;
	private System . Windows . Forms . Label label15;
	private System . Windows . Forms . Panel panTitle;
	private System . Windows . Forms . Panel lblyyyy;
	private System . Windows . Forms . Label lblttdd;
	private System . Windows . Forms . DateTimePicker dtpTdate;
	private System . Windows . Forms . DateTimePicker dtpFDate;
	private System . Windows . Forms . CheckBox chkBun;
	private System . Windows . Forms . Button btncancel;
    }
}