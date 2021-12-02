namespace ComPmpaLibB
{
    partial class frmPmPaMISUM405STS
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
            FarPoint.Win.Spread.NamedStyle namedStyle1 = new FarPoint.Win.Spread.NamedStyle("Color343636409382849783716", "DataAreaDefault");
            FarPoint.Win.Spread.NamedStyle namedStyle2 = new FarPoint.Win.Spread.NamedStyle("Text437636409382849983764", "DataAreaDefault");
            FarPoint.Win.Spread.CellType.TextCellType textCellType1 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle3 = new FarPoint.Win.Spread.NamedStyle("Static541636409382850213860");
            FarPoint.Win.Spread.CellType.TextCellType textCellType2 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.NamedStyle namedStyle4 = new FarPoint.Win.Spread.NamedStyle("Static721636409382850573969");
            FarPoint.Win.Spread.CellType.TextCellType textCellType3 = new FarPoint.Win.Spread.CellType.TextCellType();
            FarPoint.Win.Spread.TipAppearance tipAppearance1 = new FarPoint.Win.Spread.TipAppearance();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panTitle = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.lblyyyy = new System.Windows.Forms.Panel();
            this.panSub = new System.Windows.Forms.Panel();
            this.cboyyyy = new System.Windows.Forms.ComboBox();
            this.rdoJob1 = new System.Windows.Forms.RadioButton();
            this.cboJong = new System.Windows.Forms.ComboBox();
            this.cbobi = new System.Windows.Forms.Label();
            this.rdoJob2 = new System.Windows.Forms.RadioButton();
            this.rdoJob0 = new System.Windows.Forms.RadioButton();
            this.lbldoc = new System.Windows.Forms.Label();
            this.lblsort = new System.Windows.Forms.Label();
            this.panTitleSub1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.ssView = new FarPoint.Win.Spread.FpSpread();
            this.panSSmain = new System.Windows.Forms.Panel();
            this.panmain = new System.Windows.Forms.Panel();
            this.panTitle.SuspendLayout();
            this.lblyyyy.SuspendLayout();
            this.panSub.SuspendLayout();
            this.panTitleSub1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
            this.panSSmain.SuspendLayout();
            this.panmain.SuspendLayout();
            this.SuspendLayout();
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
            this.panTitle.Controls.Add(this.label15);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.ForeColor = System.Drawing.Color.White;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(928, 34);
            this.panTitle.TabIndex = 190;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(3, 5);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(252, 21);
            this.label15.TabIndex = 83;
            this.label15.Text = "조합청구 예상액과 실청구액 점검";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = true;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExit.Location = new System.Drawing.Point(852, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 30);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnView
            // 
            this.btnView.AutoSize = true;
            this.btnView.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnView.Location = new System.Drawing.Point(784, 0);
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
            this.btnPrint.Location = new System.Drawing.Point(856, 0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 32);
            this.btnPrint.TabIndex = 80;
            this.btnPrint.Text = "인쇄";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lblyyyy
            // 
            this.lblyyyy.BackColor = System.Drawing.SystemColors.Window;
            this.lblyyyy.Controls.Add(this.panSub);
            this.lblyyyy.Controls.Add(this.btnView);
            this.lblyyyy.Controls.Add(this.btnPrint);
            this.lblyyyy.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblyyyy.Location = new System.Drawing.Point(0, 62);
            this.lblyyyy.Name = "lblyyyy";
            this.lblyyyy.Size = new System.Drawing.Size(928, 32);
            this.lblyyyy.TabIndex = 192;
            // 
            // panSub
            // 
            this.panSub.Controls.Add(this.cboyyyy);
            this.panSub.Controls.Add(this.rdoJob1);
            this.panSub.Controls.Add(this.cboJong);
            this.panSub.Controls.Add(this.cbobi);
            this.panSub.Controls.Add(this.rdoJob2);
            this.panSub.Controls.Add(this.rdoJob0);
            this.panSub.Controls.Add(this.lbldoc);
            this.panSub.Controls.Add(this.lblsort);
            this.panSub.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub.Location = new System.Drawing.Point(0, 0);
            this.panSub.Name = "panSub";
            this.panSub.Size = new System.Drawing.Size(685, 32);
            this.panSub.TabIndex = 90;
            // 
            // cboyyyy
            // 
            this.cboyyyy.FormattingEnabled = true;
            this.cboyyyy.Location = new System.Drawing.Point(59, 6);
            this.cboyyyy.Name = "cboyyyy";
            this.cboyyyy.Size = new System.Drawing.Size(99, 20);
            this.cboyyyy.TabIndex = 172;
            // 
            // rdoJob1
            // 
            this.rdoJob1.AutoSize = true;
            this.rdoJob1.Checked = true;
            this.rdoJob1.Location = new System.Drawing.Point(509, 8);
            this.rdoJob1.Name = "rdoJob1";
            this.rdoJob1.Size = new System.Drawing.Size(109, 16);
            this.rdoJob1.TabIndex = 171;
            this.rdoJob1.TabStop = true;
            this.rdoJob1.Text = "차액 10,000이상";
            this.rdoJob1.UseVisualStyleBackColor = true;
            // 
            // cboJong
            // 
            this.cboJong.FormattingEnabled = true;
            this.cboJong.Location = new System.Drawing.Point(220, 6);
            this.cboJong.Name = "cboJong";
            this.cboJong.Size = new System.Drawing.Size(99, 20);
            this.cboJong.TabIndex = 170;
            // 
            // cbobi
            // 
            this.cbobi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cbobi.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cbobi.ForeColor = System.Drawing.Color.Black;
            this.cbobi.Location = new System.Drawing.Point(162, 5);
            this.cbobi.Name = "cbobi";
            this.cbobi.Size = new System.Drawing.Size(57, 23);
            this.cbobi.TabIndex = 169;
            this.cbobi.Text = "환자종류";
            this.cbobi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rdoJob2
            // 
            this.rdoJob2.AutoSize = true;
            this.rdoJob2.Location = new System.Drawing.Point(444, 8);
            this.rdoJob2.Name = "rdoJob2";
            this.rdoJob2.Size = new System.Drawing.Size(65, 16);
            this.rdoJob2.TabIndex = 106;
            this.rdoJob2.Text = "금액0원";
            this.rdoJob2.UseVisualStyleBackColor = true;
            // 
            // rdoJob0
            // 
            this.rdoJob0.AutoSize = true;
            this.rdoJob0.Location = new System.Drawing.Point(397, 8);
            this.rdoJob0.Name = "rdoJob0";
            this.rdoJob0.Size = new System.Drawing.Size(47, 16);
            this.rdoJob0.TabIndex = 105;
            this.rdoJob0.Text = "전체";
            this.rdoJob0.UseVisualStyleBackColor = true;
            // 
            // lbldoc
            // 
            this.lbldoc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbldoc.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbldoc.ForeColor = System.Drawing.Color.Black;
            this.lbldoc.Location = new System.Drawing.Point(12, 5);
            this.lbldoc.Name = "lbldoc";
            this.lbldoc.Size = new System.Drawing.Size(46, 23);
            this.lbldoc.TabIndex = 96;
            this.lbldoc.Text = "작업월";
            this.lbldoc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblsort
            // 
            this.lblsort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblsort.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblsort.ForeColor = System.Drawing.Color.Black;
            this.lblsort.Location = new System.Drawing.Point(331, 5);
            this.lblsort.Name = "lblsort";
            this.lblsort.Size = new System.Drawing.Size(63, 23);
            this.lblsort.TabIndex = 90;
            this.lblsort.Text = "작업선택";
            this.lblsort.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panTitleSub1
            // 
            this.panTitleSub1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub1.Controls.Add(this.label1);
            this.panTitleSub1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub1.Location = new System.Drawing.Point(0, 94);
            this.panTitleSub1.Name = "panTitleSub1";
            this.panTitleSub1.Size = new System.Drawing.Size(928, 28);
            this.panTitleSub1.TabIndex = 193;
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
            this.ssView_Sheet1.ColumnCount = 9;
            this.ssView_Sheet1.RowCount = 0;
            this.ssView_Sheet1.ActiveColumnIndex = -1;
            this.ssView_Sheet1.ActiveRowIndex = -1;
            this.ssView_Sheet1.ColumnFooter.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "종류";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "등록번호";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "성 명";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "과";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "퇴원일자";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "청구예상액";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "실청구액";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "청구차액";
            this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "비 고";
            this.ssView_Sheet1.ColumnHeader.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.ssView_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.ssView_Sheet1.Columns.Default.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            this.ssView_Sheet1.Columns.Get(0).Label = "종류";
            this.ssView_Sheet1.Columns.Get(0).StyleName = "Static541636409382850213860";
            this.ssView_Sheet1.Columns.Get(0).Width = 35F;
            this.ssView_Sheet1.Columns.Get(1).Label = "등록번호";
            this.ssView_Sheet1.Columns.Get(1).StyleName = "Static541636409382850213860";
            this.ssView_Sheet1.Columns.Get(1).Width = 65F;
            this.ssView_Sheet1.Columns.Get(2).Label = "성 명";
            this.ssView_Sheet1.Columns.Get(2).StyleName = "Static541636409382850213860";
            this.ssView_Sheet1.Columns.Get(2).Width = 75F;
            this.ssView_Sheet1.Columns.Get(3).Label = "과";
            this.ssView_Sheet1.Columns.Get(3).StyleName = "Static541636409382850213860";
            this.ssView_Sheet1.Columns.Get(3).Width = 33F;
            this.ssView_Sheet1.Columns.Get(4).Label = "퇴원일자";
            this.ssView_Sheet1.Columns.Get(4).StyleName = "Static541636409382850213860";
            this.ssView_Sheet1.Columns.Get(4).Width = 77F;
            this.ssView_Sheet1.Columns.Get(5).Label = "청구예상액";
            this.ssView_Sheet1.Columns.Get(5).StyleName = "Static721636409382850573969";
            this.ssView_Sheet1.Columns.Get(5).Width = 92F;
            this.ssView_Sheet1.Columns.Get(6).Label = "실청구액";
            this.ssView_Sheet1.Columns.Get(6).StyleName = "Static721636409382850573969";
            this.ssView_Sheet1.Columns.Get(6).Width = 89F;
            this.ssView_Sheet1.Columns.Get(7).Label = "청구차액";
            this.ssView_Sheet1.Columns.Get(7).StyleName = "Static721636409382850573969";
            this.ssView_Sheet1.Columns.Get(7).Width = 91F;
            this.ssView_Sheet1.Columns.Get(8).Label = "비 고";
            this.ssView_Sheet1.Columns.Get(8).StyleName = "Static541636409382850213860";
            this.ssView_Sheet1.Columns.Get(8).Width = 162F;
            this.ssView_Sheet1.DefaultStyleName = "Text437636409382849983764";
            this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssView_Sheet1.RowHeader.DefaultStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssView_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.ssView_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.SheetCornerStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ssView_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssView_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssView_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.ssView_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // ssView
            // 
            this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, ";
            this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssView.Location = new System.Drawing.Point(0, 0);
            this.ssView.Name = "ssView";
            namedStyle1.Font = new System.Drawing.Font("굴림체", 9F);
            namedStyle1.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            namedStyle1.NoteIndicatorColor = System.Drawing.Color.Red;
            namedStyle1.Parent = "DataAreaDefault";
            namedStyle1.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            textCellType1.MaxLength = 60;
            namedStyle2.CellType = textCellType1;
            namedStyle2.Font = new System.Drawing.Font("굴림체", 9F);
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
            namedStyle4.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
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
            this.ssView.Size = new System.Drawing.Size(928, 408);
            this.ssView.TabIndex = 166;
            this.ssView.TabStripRatio = 0.6D;
            tipAppearance1.BackColor = System.Drawing.SystemColors.Info;
            tipAppearance1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            tipAppearance1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.ssView.TextTipAppearance = tipAppearance1;
            this.ssView.SetActiveViewport(0, -1, -1);
            // 
            // panSSmain
            // 
            this.panSSmain.Controls.Add(this.ssView);
            this.panSSmain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panSSmain.Location = new System.Drawing.Point(0, 122);
            this.panSSmain.Name = "panSSmain";
            this.panSSmain.Size = new System.Drawing.Size(928, 408);
            this.panSSmain.TabIndex = 194;
            // 
            // panmain
            // 
            this.panmain.BackColor = System.Drawing.Color.RoyalBlue;
            this.panmain.Controls.Add(this.lblTitleSub0);
            this.panmain.Dock = System.Windows.Forms.DockStyle.Top;
            this.panmain.Location = new System.Drawing.Point(0, 34);
            this.panmain.Name = "panmain";
            this.panmain.Size = new System.Drawing.Size(928, 28);
            this.panmain.TabIndex = 191;
            // 
            // frmPmPaMISUM405STS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 530);
            this.Controls.Add(this.panSSmain);
            this.Controls.Add(this.panTitleSub1);
            this.Controls.Add(this.lblyyyy);
            this.Controls.Add(this.panmain);
            this.Controls.Add(this.panTitle);
            this.Name = "frmPmPaMISUM405STS";
            this.Text = "조합청구 예상액과 실청구액 점검";
            this.Load += new System.EventHandler(this.frmPmPaMISUM405STS_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.lblyyyy.ResumeLayout(false);
            this.lblyyyy.PerformLayout();
            this.panSub.ResumeLayout(false);
            this.panSub.PerformLayout();
            this.panTitleSub1.ResumeLayout(false);
            this.panTitleSub1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
            this.panSSmain.ResumeLayout(false);
            this.panmain.ResumeLayout(false);
            this.panmain.PerformLayout();
            this.ResumeLayout(false);

	}

	#endregion
	private System . Windows . Forms . Label lblTitleSub0;
	private System . Windows . Forms . Panel panTitle;
	private System . Windows . Forms . Label label15;
	public System . Windows . Forms . Button btnExit;
	private System . Windows . Forms . Button btnView;
	private System . Windows . Forms . Button btnPrint;
	private System . Windows . Forms . Panel lblyyyy;
	private System . Windows . Forms . Panel panSub;
	private System . Windows . Forms . RadioButton rdoJob2;
	private System . Windows . Forms . RadioButton rdoJob0;
	private System . Windows . Forms . Label lbldoc;
	private System . Windows . Forms . Label lblsort;
	private System . Windows . Forms . Panel panTitleSub1;
	private System . Windows . Forms . Label label1;
	private FarPoint . Win . Spread . SheetView ssView_Sheet1;
	private FarPoint . Win . Spread . FpSpread ssView;
	private System . Windows . Forms . Panel panSSmain;
	private System . Windows . Forms . Panel panmain;
	private System . Windows . Forms . RadioButton rdoJob1;
	private System . Windows . Forms . ComboBox cboJong;
	private System . Windows . Forms . Label cbobi;
	private System . Windows . Forms . ComboBox cboyyyy;
    }
}