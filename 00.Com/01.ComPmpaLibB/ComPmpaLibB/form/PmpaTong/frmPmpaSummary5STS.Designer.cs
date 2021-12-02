namespace ComPmpaLibB 
{
    partial class frmPmpaSummary5STS
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
	    this.panel2 = new System.Windows.Forms.Panel();
	    this.label1 = new System.Windows.Forms.Label();
	    this.label15 = new System.Windows.Forms.Label();
	    this.btnExit = new System.Windows.Forms.Button();
	    this.panTitleSub0 = new System.Windows.Forms.Panel();
	    this.lblTitleSub0 = new System.Windows.Forms.Label();
	    this.panTitle = new System.Windows.Forms.Panel();
	    this.lblyyyymm = new System.Windows.Forms.Label();
	    this.btnView = new System.Windows.Forms.Button();
	    this.btnPrint = new System.Windows.Forms.Button();
	    this.panel3 = new System.Windows.Forms.Panel();
	    this.cboYYYY = new System.Windows.Forms.ComboBox();
	    this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
	    this.ssView = new FarPoint.Win.Spread.FpSpread();
	    this.lblyyyy = new System.Windows.Forms.Panel();
	    this.panel2.SuspendLayout();
	    this.panTitleSub0.SuspendLayout();
	    this.panTitle.SuspendLayout();
	    this.panel3.SuspendLayout();
	    ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
	    ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
	    this.lblyyyy.SuspendLayout();
	    this.SuspendLayout();
	    // 
	    // panel2
	    // 
	    this.panel2.BackColor = System.Drawing.Color.RoyalBlue;
	    this.panel2.Controls.Add(this.label1);
	    this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
	    this.panel2.Location = new System.Drawing.Point(0, 94);
	    this.panel2.Name = "panel2";
	    this.panel2.Size = new System.Drawing.Size(643, 28);
	    this.panel2.TabIndex = 122;
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
	    // label15
	    // 
	    this.label15.AutoSize = true;
	    this.label15.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
	    this.label15.ForeColor = System.Drawing.Color.Black;
	    this.label15.Location = new System.Drawing.Point(3, 5);
	    this.label15.Name = "label15";
	    this.label15.Size = new System.Drawing.Size(260, 21);
	    this.label15.TabIndex = 83;
	    this.label15.Text = "청구 미수 발생 통계 (처방전 기준)";
	    this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
	    // 
	    // btnExit
	    // 
	    this.btnExit.AutoSize = true;
	    this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
	    this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
	    this.btnExit.Location = new System.Drawing.Point(567, 0);
	    this.btnExit.Name = "btnExit";
	    this.btnExit.Size = new System.Drawing.Size(72, 30);
	    this.btnExit.TabIndex = 15;
	    this.btnExit.Text = "닫기";
	    this.btnExit.UseVisualStyleBackColor = true;
	    this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
	    // 
	    // panTitleSub0
	    // 
	    this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
	    this.panTitleSub0.Controls.Add(this.lblTitleSub0);
	    this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
	    this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
	    this.panTitleSub0.Name = "panTitleSub0";
	    this.panTitleSub0.Size = new System.Drawing.Size(643, 28);
	    this.panTitleSub0.TabIndex = 120;
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
	    this.panTitle.Size = new System.Drawing.Size(643, 34);
	    this.panTitle.TabIndex = 119;
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
	    this.lblyyyymm.Text = "작업년월";
	    this.lblyyyymm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
	    // 
	    // btnView
	    // 
	    this.btnView.AutoSize = true;
	    this.btnView.Dock = System.Windows.Forms.DockStyle.Right;
	    this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
	    this.btnView.Location = new System.Drawing.Point(499, 0);
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
	    this.btnPrint.Location = new System.Drawing.Point(571, 0);
	    this.btnPrint.Name = "btnPrint";
	    this.btnPrint.Size = new System.Drawing.Size(72, 32);
	    this.btnPrint.TabIndex = 80;
	    this.btnPrint.Text = "인쇄";
	    this.btnPrint.UseVisualStyleBackColor = true;
	    this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
	    // 
	    // panel3
	    // 
	    this.panel3.Controls.Add(this.cboYYYY);
	    this.panel3.Controls.Add(this.lblyyyymm);
	    this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
	    this.panel3.Location = new System.Drawing.Point(0, 0);
	    this.panel3.Name = "panel3";
	    this.panel3.Size = new System.Drawing.Size(212, 32);
	    this.panel3.TabIndex = 88;
	    // 
	    // cboYYYY
	    // 
	    this.cboYYYY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
	    this.cboYYYY.FormattingEnabled = true;
	    this.cboYYYY.Location = new System.Drawing.Point(81, 6);
	    this.cboYYYY.Name = "cboYYYY";
	    this.cboYYYY.Size = new System.Drawing.Size(121, 20);
	    this.cboYYYY.TabIndex = 88;
	    // 
	    // ssView_Sheet1
	    // 
	    this.ssView_Sheet1.Reset();
	    this.ssView_Sheet1.SheetName = "Sheet1";
	    // Formulas and custom names must be loaded with R1C1 reference style
	    this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
	    this.ssView_Sheet1.ColumnCount = 5;
	    this.ssView_Sheet1.RowCount = 6;
	    this.ssView_Sheet1.Cells.Get(0, 0).CellType = textCellType1;
	    this.ssView_Sheet1.Cells.Get(0, 0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Cells.Get(0, 0).Value = "의료보험";
	    this.ssView_Sheet1.Cells.Get(0, 0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    textCellType2.Multiline = true;
	    textCellType2.WordWrap = true;
	    this.ssView_Sheet1.Cells.Get(0, 1).CellType = textCellType2;
	    this.ssView_Sheet1.Cells.Get(0, 1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Cells.Get(0, 1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    textCellType3.Multiline = true;
	    textCellType3.WordWrap = true;
	    this.ssView_Sheet1.Cells.Get(0, 2).CellType = textCellType3;
	    this.ssView_Sheet1.Cells.Get(0, 2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Cells.Get(0, 2).Locked = false;
	    this.ssView_Sheet1.Cells.Get(0, 2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    textCellType4.Multiline = true;
	    textCellType4.WordWrap = true;
	    this.ssView_Sheet1.Cells.Get(0, 3).CellType = textCellType4;
	    this.ssView_Sheet1.Cells.Get(0, 3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Cells.Get(0, 3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    textCellType5.Multiline = true;
	    textCellType5.WordWrap = true;
	    this.ssView_Sheet1.Cells.Get(0, 4).CellType = textCellType5;
	    this.ssView_Sheet1.Cells.Get(0, 4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Cells.Get(0, 4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Cells.Get(1, 0).Value = "의료보호";
	    this.ssView_Sheet1.Cells.Get(2, 0).Value = "산  재";
	    this.ssView_Sheet1.Cells.Get(3, 0).Value = "자  보";
	    this.ssView_Sheet1.Cells.Get(4, 0).Value = "기  타";
	    this.ssView_Sheet1.Cells.Get(5, 0).Value = "합  계";
	    this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
	    this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
	    this.ssView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
	    this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "구  분";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "외  래";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "입  원";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "합  계";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "비  고";
	    this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
	    this.ssView_Sheet1.Columns.Get(0).CellType = textCellType6;
	    this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(0).Label = "구  분";
	    this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(0).Width = 100F;
	    textCellType7.Multiline = true;
	    textCellType7.WordWrap = true;
	    this.ssView_Sheet1.Columns.Get(1).CellType = textCellType7;
	    this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(1).Label = "외  래";
	    this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(1).Width = 140F;
	    textCellType8.Multiline = true;
	    textCellType8.WordWrap = true;
	    this.ssView_Sheet1.Columns.Get(2).CellType = textCellType8;
	    this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(2).Label = "입  원";
	    this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(2).Width = 140F;
	    textCellType9.Multiline = true;
	    textCellType9.WordWrap = true;
	    this.ssView_Sheet1.Columns.Get(3).CellType = textCellType9;
	    this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(3).Label = "합  계";
	    this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(3).Width = 140F;
	    textCellType10.Multiline = true;
	    this.ssView_Sheet1.Columns.Get(4).CellType = textCellType10;
	    this.ssView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(4).Label = "비  고";
	    this.ssView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(4).Width = 100F;
	    this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
	    this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
	    this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
	    this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
	    this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
	    this.ssView_Sheet1.RowHeader.Visible = false;
	    this.ssView_Sheet1.Rows.Get(0).Height = 50F;
	    this.ssView_Sheet1.Rows.Get(1).Height = 50F;
	    this.ssView_Sheet1.Rows.Get(2).Height = 50F;
	    this.ssView_Sheet1.Rows.Get(3).Height = 50F;
	    this.ssView_Sheet1.Rows.Get(4).Height = 50F;
	    this.ssView_Sheet1.Rows.Get(5).Height = 50F;
	    this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
	    // 
	    // ssView
	    // 
	    this.ssView.AccessibleDescription = "ssView, Sheet1, Row 0, Column 0, 의료보험";
	    this.ssView.Dock = System.Windows.Forms.DockStyle.Fill;
	    this.ssView.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
	    this.ssView.Location = new System.Drawing.Point(0, 122);
	    this.ssView.Name = "ssView";
	    this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
	    this.ssView.Size = new System.Drawing.Size(643, 335);
	    this.ssView.TabIndex = 123;
	    // 
	    // lblyyyy
	    // 
	    this.lblyyyy.BackColor = System.Drawing.SystemColors.Window;
	    this.lblyyyy.Controls.Add(this.btnView);
	    this.lblyyyy.Controls.Add(this.panel3);
	    this.lblyyyy.Controls.Add(this.btnPrint);
	    this.lblyyyy.Dock = System.Windows.Forms.DockStyle.Top;
	    this.lblyyyy.Location = new System.Drawing.Point(0, 62);
	    this.lblyyyy.Name = "lblyyyy";
	    this.lblyyyy.Size = new System.Drawing.Size(643, 32);
	    this.lblyyyy.TabIndex = 121;
	    // 
	    // frmPmpaSummary5STS
	    // 
	    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
	    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	    this.ClientSize = new System.Drawing.Size(643, 457);
	    this.Controls.Add(this.ssView);
	    this.Controls.Add(this.panel2);
	    this.Controls.Add(this.lblyyyy);
	    this.Controls.Add(this.panTitleSub0);
	    this.Controls.Add(this.panTitle);
	    this.Name = "frmPmpaSummary5STS";
	    this.Text = "청구 미수 발생 통계 (처방전 기준)";
	    this.Load += new System.EventHandler(this.frmPmpaSummary5STS_Load);
	    this.panel2.ResumeLayout(false);
	    this.panel2.PerformLayout();
	    this.panTitleSub0.ResumeLayout(false);
	    this.panTitleSub0.PerformLayout();
	    this.panTitle.ResumeLayout(false);
	    this.panTitle.PerformLayout();
	    this.panel3.ResumeLayout(false);
	    ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
	    ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
	    this.lblyyyy.ResumeLayout(false);
	    this.lblyyyy.PerformLayout();
	    this.ResumeLayout(false);

	}

	#endregion

	private System . Windows . Forms . Panel panel2;
	private System . Windows . Forms . Label label1;
	private System . Windows . Forms . Label label15;
	public System . Windows . Forms . Button btnExit;
	private System . Windows . Forms . Panel panTitleSub0;
	private System . Windows . Forms . Label lblTitleSub0;
	private System . Windows . Forms . Panel panTitle;
	private System . Windows . Forms . Label lblyyyymm;
	private System . Windows . Forms . Button btnView;
	private System . Windows . Forms . Button btnPrint;
	private System . Windows . Forms . Panel panel3;
	private FarPoint . Win . Spread . SheetView ssView_Sheet1;
	private FarPoint . Win . Spread . FpSpread ssView;
	private System . Windows . Forms . Panel lblyyyy;
	private System . Windows . Forms . ComboBox cboYYYY;
    }
}