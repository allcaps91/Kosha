namespace ComPmpaLibB
{
    partial class frmPmPaVIEWJewonlistNew
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("BorderEx360636410922285223784", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text468636410922285263796", "DataAreaDefault");
            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("CheckBox648636410922285805025");
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType1 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static684636410922285825005");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle5 = new FarPoint.Win.Spread.NamedStyle("Static1080636410922286015071");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.panmain = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblsubTile = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.cboWard = new System.Windows.Forms.ComboBox();
            this.btnView = new System.Windows.Forms.Button();
            this.lblyyyy = new System.Windows.Forms.Panel();
            this.btnSelPrnt = new System.Windows.Forms.Button();
            this.panSub = new System.Windows.Forms.Panel();
            this.cbodept = new System.Windows.Forms.ComboBox();
            this.lblward = new System.Windows.Forms.Label();
            this.rdo2 = new System.Windows.Forms.RadioButton();
            this.rdo1 = new System.Windows.Forms.RadioButton();
            this.rdo0 = new System.Windows.Forms.RadioButton();
            this.lbldept = new System.Windows.Forms.Label();
            this.lbldoc = new System.Windows.Forms.Label();
            this.panTitleSub1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSSmain = new System.Windows.Forms.Panel();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.panmain.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.lblyyyy.SuspendLayout();
            this.panSub.SuspendLayout();
            this.panTitleSub1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            this.panSSmain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            this.SuspendLayout();
            // 
            // panmain
            // 
            this.panmain.BackColor = System.Drawing.Color.RoyalBlue;
            this.panmain.Controls.Add(this.lblTitleSub0);
            this.panmain.Dock = System.Windows.Forms.DockStyle.Top;
            this.panmain.Location = new System.Drawing.Point(0, 34);
            this.panmain.Name = "panmain";
            this.panmain.Size = new System.Drawing.Size(1014, 28);
            this.panmain.TabIndex = 201;
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
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblsubTile);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1014, 34);
            this.panTitle.TabIndex = 200;
            // 
            // lblsubTile
            // 
            this.lblsubTile.AutoSize = true;
            this.lblsubTile.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblsubTile.ForeColor = System.Drawing.Color.Black;
            this.lblsubTile.Location = new System.Drawing.Point(3, 5);
            this.lblsubTile.Name = "lblsubTile";
            this.lblsubTile.Size = new System.Drawing.Size(118, 21);
            this.lblsubTile.TabIndex = 83;
            this.lblsubTile.Text = "재원 환자 명단";
            this.lblsubTile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = true;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(938, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btncansel_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.AutoSize = true;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrint.Location = new System.Drawing.Point(942, 0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 32);
            this.btnPrint.TabIndex = 80;
            this.btnPrint.Text = "인쇄";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // cboWard
            // 
            this.cboWard.FormattingEnabled = true;
            this.cboWard.Location = new System.Drawing.Point(226, 6);
            this.cboWard.Name = "cboWard";
            this.cboWard.Size = new System.Drawing.Size(102, 20);
            this.cboWard.TabIndex = 169;
            this.cboWard.Text = "ComboWard";
            // 
            // btnView
            // 
            this.btnView.AutoSize = true;
            this.btnView.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnView.Location = new System.Drawing.Point(798, 0);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(72, 32);
            this.btnView.TabIndex = 77;
            this.btnView.Text = "조회";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // lblyyyy
            // 
            this.lblyyyy.BackColor = System.Drawing.SystemColors.Window;
            this.lblyyyy.Controls.Add(this.btnView);
            this.lblyyyy.Controls.Add(this.btnSelPrnt);
            this.lblyyyy.Controls.Add(this.panSub);
            this.lblyyyy.Controls.Add(this.btnPrint);
            this.lblyyyy.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblyyyy.Location = new System.Drawing.Point(0, 62);
            this.lblyyyy.Name = "lblyyyy";
            this.lblyyyy.Size = new System.Drawing.Size(1014, 32);
            this.lblyyyy.TabIndex = 202;
            // 
            // btnSelPrnt
            // 
            this.btnSelPrnt.AutoSize = true;
            this.btnSelPrnt.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSelPrnt.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSelPrnt.Location = new System.Drawing.Point(870, 0);
            this.btnSelPrnt.Name = "btnSelPrnt";
            this.btnSelPrnt.Size = new System.Drawing.Size(72, 32);
            this.btnSelPrnt.TabIndex = 91;
            this.btnSelPrnt.Text = "선택 인쇄";
            this.btnSelPrnt.UseVisualStyleBackColor = true;
            this.btnSelPrnt.Click += new System.EventHandler(this.btnSelPrnt_Click);
            // 
            // panSub
            // 
            this.panSub.Controls.Add(this.cbodept);
            this.panSub.Controls.Add(this.lblward);
            this.panSub.Controls.Add(this.rdo2);
            this.panSub.Controls.Add(this.rdo1);
            this.panSub.Controls.Add(this.rdo0);
            this.panSub.Controls.Add(this.cboWard);
            this.panSub.Controls.Add(this.lbldept);
            this.panSub.Controls.Add(this.lbldoc);
            this.panSub.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub.Location = new System.Drawing.Point(0, 0);
            this.panSub.Name = "panSub";
            this.panSub.Size = new System.Drawing.Size(605, 32);
            this.panSub.TabIndex = 90;
            // 
            // cbodept
            // 
            this.cbodept.FormattingEnabled = true;
            this.cbodept.Location = new System.Drawing.Point(419, 6);
            this.cbodept.Name = "cbodept";
            this.cbodept.Size = new System.Drawing.Size(106, 20);
            this.cbodept.TabIndex = 174;
            this.cbodept.Text = "Combodept";
            // 
            // lblward
            // 
            this.lblward.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblward.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblward.ForeColor = System.Drawing.Color.Black;
            this.lblward.Location = new System.Drawing.Point(192, 5);
            this.lblward.Name = "lblward";
            this.lblward.Size = new System.Drawing.Size(33, 23);
            this.lblward.TabIndex = 173;
            this.lblward.Text = "병동";
            this.lblward.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rdo2
            // 
            this.rdo2.AutoSize = true;
            this.rdo2.Location = new System.Drawing.Point(139, 8);
            this.rdo2.Name = "rdo2";
            this.rdo2.Size = new System.Drawing.Size(47, 16);
            this.rdo2.TabIndex = 172;
            this.rdo2.Text = "의사";
            this.rdo2.UseVisualStyleBackColor = true;
            // 
            // rdo1
            // 
            this.rdo1.AutoSize = true;
            this.rdo1.Location = new System.Drawing.Point(92, 8);
            this.rdo1.Name = "rdo1";
            this.rdo1.Size = new System.Drawing.Size(47, 16);
            this.rdo1.TabIndex = 171;
            this.rdo1.Text = "성명";
            this.rdo1.UseVisualStyleBackColor = true;
            // 
            // rdo0
            // 
            this.rdo0.AutoSize = true;
            this.rdo0.Checked = true;
            this.rdo0.Location = new System.Drawing.Point(45, 8);
            this.rdo0.Name = "rdo0";
            this.rdo0.Size = new System.Drawing.Size(47, 16);
            this.rdo0.TabIndex = 170;
            this.rdo0.TabStop = true;
            this.rdo0.Text = "호실";
            this.rdo0.UseVisualStyleBackColor = true;
            // 
            // lbldept
            // 
            this.lbldept.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbldept.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbldept.ForeColor = System.Drawing.Color.Black;
            this.lbldept.Location = new System.Drawing.Point(344, 5);
            this.lbldept.Name = "lbldept";
            this.lbldept.Size = new System.Drawing.Size(74, 23);
            this.lbldept.TabIndex = 90;
            this.lbldept.Text = "진료과";
            this.lbldept.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbldoc
            // 
            this.lbldoc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbldoc.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbldoc.ForeColor = System.Drawing.Color.Black;
            this.lbldoc.Location = new System.Drawing.Point(4, 5);
            this.lbldoc.Name = "lbldoc";
            this.lbldoc.Size = new System.Drawing.Size(38, 23);
            this.lbldoc.TabIndex = 96;
            this.lbldoc.Text = "SORT";
            this.lbldoc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitleSub1
            // 
            this.panTitleSub1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub1.Controls.Add(this.label1);
            this.panTitleSub1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub1.Location = new System.Drawing.Point(0, 94);
            this.panTitleSub1.Name = "panTitleSub1";
            this.panTitleSub1.Size = new System.Drawing.Size(1014, 28);
            this.panTitleSub1.TabIndex = 203;
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
            // ssView_Sheet1
            // 
            this.ssView_Sheet1.Reset();
            this.ssView_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssView_Sheet1.ColumnCount = 16;
            this.ssView_Sheet1.RowCount = 0;
            this.ssView_Sheet1.ActiveColumnIndex = -1;
            this.ssView_Sheet1.ActiveRowIndex = -1;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnFooter.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnFooter.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = " ";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "병동";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "호실";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "등록번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "성명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "나이";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "성별";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "과";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "의사";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "입원일";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "보험";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "재원일수";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "주소";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "지역";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "경로";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "우편번호";
            this.ssView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 35F;
            this.ssView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Get(0).Label = " ";
            this.ssView_Sheet1.Columns.Get(0).StyleName = "CheckBox648636410922285805025";
            this.ssView_Sheet1.Columns.Get(0).Width = 25F;
            this.ssView_Sheet1.Columns.Get(1).Label = "병동";
            this.ssView_Sheet1.Columns.Get(1).StyleName = "Static684636410922285825005";
            this.ssView_Sheet1.Columns.Get(1).Width = 38F;
            this.ssView_Sheet1.Columns.Get(2).Label = "호실";
            this.ssView_Sheet1.Columns.Get(2).StyleName = "Static684636410922285825005";
            this.ssView_Sheet1.Columns.Get(2).Width = 40F;
            this.ssView_Sheet1.Columns.Get(3).Label = "등록번호";
            this.ssView_Sheet1.Columns.Get(3).StyleName = "Static684636410922285825005";
            this.ssView_Sheet1.Columns.Get(3).Width = 67F;
            this.ssView_Sheet1.Columns.Get(4).Label = "성명";
            this.ssView_Sheet1.Columns.Get(4).StyleName = "Static684636410922285825005";
            this.ssView_Sheet1.Columns.Get(4).Width = 76F;
            this.ssView_Sheet1.Columns.Get(5).Label = "나이";
            this.ssView_Sheet1.Columns.Get(5).StyleName = "Static684636410922285825005";
            this.ssView_Sheet1.Columns.Get(5).Width = 35F;
            this.ssView_Sheet1.Columns.Get(6).Label = "성별";
            this.ssView_Sheet1.Columns.Get(6).StyleName = "Static684636410922285825005";
            this.ssView_Sheet1.Columns.Get(6).Width = 40F;
            this.ssView_Sheet1.Columns.Get(7).Label = "과";
            this.ssView_Sheet1.Columns.Get(7).StyleName = "Static684636410922285825005";
            this.ssView_Sheet1.Columns.Get(7).Width = 35F;
            this.ssView_Sheet1.Columns.Get(8).Label = "의사";
            this.ssView_Sheet1.Columns.Get(8).StyleName = "Static684636410922285825005";
            this.ssView_Sheet1.Columns.Get(8).Width = 57F;
            this.ssView_Sheet1.Columns.Get(9).Label = "입원일";
            this.ssView_Sheet1.Columns.Get(9).StyleName = "Static684636410922285825005";
            this.ssView_Sheet1.Columns.Get(9).Width = 87F;
            this.ssView_Sheet1.Columns.Get(10).Label = "보험";
            this.ssView_Sheet1.Columns.Get(10).StyleName = "Static684636410922285825005";
            this.ssView_Sheet1.Columns.Get(10).Width = 40F;
            this.ssView_Sheet1.Columns.Get(11).Label = "재원일수";
            this.ssView_Sheet1.Columns.Get(11).StyleName = "Static684636410922285825005";
            this.ssView_Sheet1.Columns.Get(11).Width = 40F;
            this.ssView_Sheet1.Columns.Get(12).Label = "주소";
            this.ssView_Sheet1.Columns.Get(12).StyleName = "Static1080636410922286015071";
            this.ssView_Sheet1.Columns.Get(12).Width = 277F;
            this.ssView_Sheet1.Columns.Get(13).Label = "지역";
            this.ssView_Sheet1.Columns.Get(13).StyleName = "Static684636410922285825005";
            this.ssView_Sheet1.Columns.Get(13).Width = 40F;
            this.ssView_Sheet1.Columns.Get(14).Label = "경로";
            this.ssView_Sheet1.Columns.Get(14).StyleName = "Static684636410922285825005";
            this.ssView_Sheet1.Columns.Get(14).Width = 40F;
            this.ssView_Sheet1.Columns.Get(15).Label = "우편번호";
            this.ssView_Sheet1.Columns.Get(15).StyleName = "Static684636410922285825005";
            this.ssView_Sheet1.Columns.Get(15).Width = 80F;
            this.ssView_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.ssView_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.FilterBar.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.FrozenColumnCount = 3;
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.RowHeader.Columns.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.RowHeader.Columns.Get(0).Width = 41F;
            this.ssView_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.RowHeader.DefaultStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.RowHeader.Rows.Default.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.RowHeader.Visible = false;
            this.ssView_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssView_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.SheetCornerStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSSmain
            // 
            this.panSSmain.Controls.Add(this.ssView);
            this.panSSmain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSSmain.Location = new System.Drawing.Point(0, 122);
            this.panSSmain.Name = "panSSmain";
            this.panSSmain.Size = new System.Drawing.Size(1014, 478);
            this.panSSmain.TabIndex = 204;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Never;
            this.ssView.Location = new System.Drawing.Point(0, 0);
            this.ssView.Name = "ssView";
            namedStyle1.Border = complexBorder1;
            namedStyle1.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle2.Border = complexBorder2;
            textCellType1.MaxLength = 60;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림", 9F);
            namedStyle2.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle2.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle2.Parent = "DataAreaDefault";
            namedStyle2.Renderer = textCellType1;
            namedStyle2.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            namedStyle3.CellType = checkBoxCellType1;
            namedStyle3.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle3.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle3.Renderer = checkBoxCellType1;
            namedStyle3.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType2.Static = true;
            namedStyle4.CellType = textCellType2;
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            namedStyle4.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle4.Renderer = textCellType2;
            namedStyle4.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            textCellType3.Static = true;
            textCellType3.WordWrap = true;
            namedStyle5.CellType = textCellType3;
            namedStyle5.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
            namedStyle5.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle5.Renderer = textCellType3;
            namedStyle5.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssView.NamedStyles.AddRange(new FarPoint.Win.Spread.NamedStyle[] {
            namedStyle1,
            namedStyle2,
            namedStyle3,
            namedStyle4,
            namedStyle5});
            this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
            this.ssView.Size = new System.Drawing.Size(1014, 478);
            this.ssView.TabIndex = 166;
            this.ssView.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssView.TextTipAppearance = tipAppearance1;
            this.ssView.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.ssView.SetViewportLeftColumn(0, 0, 3);
            this.ssView.SetActiveViewport(0, -1, -1);
            // 
            // frmPmPaVIEWJewonlistNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 600);
            this.Controls.Add(this.panSSmain);
            this.Controls.Add(this.panTitleSub1);
            this.Controls.Add(this.lblyyyy);
            this.Controls.Add(this.panmain);
            this.Controls.Add(this.panTitle);
            this.Name = "frmPmPaVIEWJewonlistNew";
            this.Text = "재원 환자 명단";
            this.Load += new System.EventHandler(this.frmPmPaVIEWJewonlistNew_Load);
            this.panmain.ResumeLayout(false);
            this.panmain.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.lblyyyy.ResumeLayout(false);
            this.lblyyyy.PerformLayout();
            this.panSub.ResumeLayout(false);
            this.panSub.PerformLayout();
            this.panTitleSub1.ResumeLayout(false);
            this.panTitleSub1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            this.panSSmain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            this.ResumeLayout(false);

	}

	#endregion

	private System . Windows . Forms . Panel panmain;
	private System . Windows . Forms . Label lblTitleSub0;
	private System . Windows . Forms . Panel panTitle;
	private System . Windows . Forms . Label lblsubTile;
	public System . Windows . Forms . Button btnExit;
	private System . Windows . Forms . Button btnPrint;
	private System . Windows . Forms . ComboBox cboWard;
	private System . Windows . Forms . Button btnView;
	private System . Windows . Forms . Panel lblyyyy;
	private System . Windows . Forms . Panel panSub;
	private System . Windows . Forms . Label lbldept;
	private System . Windows . Forms . Label lbldoc;
	private System . Windows . Forms . Panel panTitleSub1;
	private System . Windows . Forms . Label label1;
	private FarPoint . Win . Spread . SheetView ssView_Sheet1;
	private System . Windows . Forms . Panel panSSmain;
	private FarPoint . Win . Spread . FpSpread ssView;
	private System . Windows . Forms . Label lblward;
	private System . Windows . Forms . RadioButton rdo2;
	private System . Windows . Forms . RadioButton rdo1;
	private System . Windows . Forms . RadioButton rdo0;
	private System . Windows . Forms . ComboBox cbodept;
	private System . Windows . Forms . Button btnSelPrnt;
    }
}