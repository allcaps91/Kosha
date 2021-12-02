namespace ComPmpaLibB
{
    partial class frmPmPaViewmisubs55
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
	    FarPoint.Win.Spread.CellType.TextCellType textCellType11 = new FarPoint.Win.Spread.CellType.TextCellType();
	    FarPoint.Win.Spread.CellType.TextCellType textCellType12 = new FarPoint.Win.Spread.CellType.TextCellType();
	    FarPoint.Win.Spread.CellType.TextCellType textCellType13 = new FarPoint.Win.Spread.CellType.TextCellType();
	    FarPoint.Win.Spread.CellType.TextCellType textCellType14 = new FarPoint.Win.Spread.CellType.TextCellType();
	    FarPoint.Win.Spread.CellType.TextCellType textCellType15 = new FarPoint.Win.Spread.CellType.TextCellType();
	    FarPoint.Win.Spread.CellType.TextCellType textCellType16 = new FarPoint.Win.Spread.CellType.TextCellType();
	    FarPoint.Win.Spread.CellType.TextCellType textCellType17 = new FarPoint.Win.Spread.CellType.TextCellType();
	    FarPoint.Win.Spread.CellType.TextCellType textCellType18 = new FarPoint.Win.Spread.CellType.TextCellType();
	    FarPoint.Win.Spread.CellType.TextCellType textCellType19 = new FarPoint.Win.Spread.CellType.TextCellType();
	    FarPoint.Win.Spread.CellType.TextCellType textCellType20 = new FarPoint.Win.Spread.CellType.TextCellType();
	    this.ssView_Sheet1 = new FarPoint.Win.Spread.SheetView();
	    this.panTitle = new System.Windows.Forms.Panel();
	    this.btnView = new System.Windows.Forms.Button();
	    this.btnPrint = new System.Windows.Forms.Button();
	    this.label15 = new System.Windows.Forms.Label();
	    this.btnExit = new System.Windows.Forms.Button();
	    this.label1 = new System.Windows.Forms.Label();
	    this.ssView = new FarPoint.Win.Spread.FpSpread();
	    this.panel2 = new System.Windows.Forms.Panel();
	    this.lblmsg = new System.Windows.Forms.Label();
	    this.pan4 = new System.Windows.Forms.Panel();
	    ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).BeginInit();
	    this.panTitle.SuspendLayout();
	    ((System.ComponentModel.ISupportInitialize)(this.ssView)).BeginInit();
	    this.panel2.SuspendLayout();
	    this.pan4.SuspendLayout();
	    this.SuspendLayout();
	    // 
	    // ssView_Sheet1
	    // 
	    this.ssView_Sheet1.Reset();
	    this.ssView_Sheet1.SheetName = "Sheet1";
	    // Formulas and custom names must be loaded with R1C1 reference style
	    this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
	    this.ssView_Sheet1.ColumnCount = 10;
	    this.ssView_Sheet1.RowCount = 1;
	    this.ssView_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
	    this.ssView_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
	    this.ssView_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
	    this.ssView_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "처방일자";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "수가코드";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "수량";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "날수";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "단가";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "비급여";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "조정액";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "작업자";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "부서";
	    this.ssView_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "수가명칭";
	    this.ssView_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
	    this.ssView_Sheet1.Columns.Get(0).CellType = textCellType11;
	    this.ssView_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(0).Label = "처방일자";
	    this.ssView_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(0).Width = 100F;
	    this.ssView_Sheet1.Columns.Get(1).CellType = textCellType12;
	    this.ssView_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(1).Label = "수가코드";
	    this.ssView_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(1).Width = 80F;
	    this.ssView_Sheet1.Columns.Get(2).CellType = textCellType13;
	    this.ssView_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(2).Label = "수량";
	    this.ssView_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(2).Width = 40F;
	    this.ssView_Sheet1.Columns.Get(3).CellType = textCellType14;
	    this.ssView_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(3).Label = "날수";
	    this.ssView_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(3).Width = 40F;
	    this.ssView_Sheet1.Columns.Get(4).CellType = textCellType15;
	    this.ssView_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(4).Label = "단가";
	    this.ssView_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(4).Width = 80F;
	    this.ssView_Sheet1.Columns.Get(5).CellType = textCellType16;
	    this.ssView_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
	    this.ssView_Sheet1.Columns.Get(5).Label = "비급여";
	    this.ssView_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(5).Width = 50F;
	    this.ssView_Sheet1.Columns.Get(6).CellType = textCellType17;
	    this.ssView_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(6).Label = "조정액";
	    this.ssView_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(6).Width = 80F;
	    this.ssView_Sheet1.Columns.Get(7).CellType = textCellType18;
	    this.ssView_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(7).Label = "작업자";
	    this.ssView_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(8).CellType = textCellType19;
	    this.ssView_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(8).Label = "부서";
	    this.ssView_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(8).Width = 80F;
	    this.ssView_Sheet1.Columns.Get(9).CellType = textCellType20;
	    this.ssView_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
	    this.ssView_Sheet1.Columns.Get(9).Label = "수가명칭";
	    this.ssView_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
	    this.ssView_Sheet1.Columns.Get(9).Width = 260F;
	    this.ssView_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
	    this.ssView_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
	    this.ssView_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
	    this.ssView_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
	    this.ssView_Sheet1.RowHeader.Columns.Default.Resizable = false;
	    this.ssView_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
	    // 
	    // panTitle
	    // 
	    this.panTitle.BackColor = System.Drawing.Color.White;
	    this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
	    this.panTitle.Controls.Add(this.btnView);
	    this.panTitle.Controls.Add(this.btnPrint);
	    this.panTitle.Controls.Add(this.label15);
	    this.panTitle.Controls.Add(this.btnExit);
	    this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
	    this.panTitle.ForeColor = System.Drawing.Color.White;
	    this.panTitle.Location = new System.Drawing.Point(0, 0);
	    this.panTitle.Name = "panTitle";
	    this.panTitle.Size = new System.Drawing.Size(929, 34);
	    this.panTitle.TabIndex = 104;
	    // 
	    // btnView
	    // 
	    this.btnView.AutoSize = true;
	    this.btnView.Dock = System.Windows.Forms.DockStyle.Right;
	    this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
	    this.btnView.Location = new System.Drawing.Point(709, 0);
	    this.btnView.Name = "btnView";
	    this.btnView.Size = new System.Drawing.Size(72, 30);
	    this.btnView.TabIndex = 84;
	    this.btnView.Text = "조회";
	    this.btnView.UseVisualStyleBackColor = true;
	    this.btnView.Click += new System.EventHandler(this.btnView_Click);
	    // 
	    // btnPrint
	    // 
	    this.btnPrint.AutoSize = true;
	    this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
	    this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
	    this.btnPrint.Location = new System.Drawing.Point(781, 0);
	    this.btnPrint.Name = "btnPrint";
	    this.btnPrint.Size = new System.Drawing.Size(72, 30);
	    this.btnPrint.TabIndex = 85;
	    this.btnPrint.Text = "인쇄";
	    this.btnPrint.UseVisualStyleBackColor = true;
	    this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
	    // 
	    // label15
	    // 
	    this.label15.AutoSize = true;
	    this.label15.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
	    this.label15.ForeColor = System.Drawing.Color.Black;
	    this.label15.Location = new System.Drawing.Point(3, 5);
	    this.label15.Name = "label15";
	    this.label15.Size = new System.Drawing.Size(160, 21);
	    this.label15.TabIndex = 83;
	    this.label15.Text = "퇴원자 심사조정내역";
	    this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
	    // 
	    // btnExit
	    // 
	    this.btnExit.AutoSize = true;
	    this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
	    this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.System;
	    this.btnExit.Location = new System.Drawing.Point(853, 0);
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
	    this.ssView.Location = new System.Drawing.Point(0, 88);
	    this.ssView.Name = "ssView";
	    this.ssView.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssView_Sheet1});
	    this.ssView.Size = new System.Drawing.Size(929, 514);
	    this.ssView.TabIndex = 108;
	    // 
	    // panel2
	    // 
	    this.panel2.BackColor = System.Drawing.Color.RoyalBlue;
	    this.panel2.Controls.Add(this.label1);
	    this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
	    this.panel2.Location = new System.Drawing.Point(0, 34);
	    this.panel2.Name = "panel2";
	    this.panel2.Size = new System.Drawing.Size(929, 28);
	    this.panel2.TabIndex = 107;
	    // 
	    // lblmsg
	    // 
	    this.lblmsg.Dock = System.Windows.Forms.DockStyle.Fill;
	    this.lblmsg.Location = new System.Drawing.Point(0, 0);
	    this.lblmsg.Name = "lblmsg";
	    this.lblmsg.Size = new System.Drawing.Size(929, 26);
	    this.lblmsg.TabIndex = 109;
	    this.lblmsg.Text = "label2";
	    this.lblmsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
	    // 
	    // pan4
	    // 
	    this.pan4.BackColor = System.Drawing.SystemColors.Window;
	    this.pan4.Controls.Add(this.lblmsg);
	    this.pan4.Dock = System.Windows.Forms.DockStyle.Top;
	    this.pan4.Location = new System.Drawing.Point(0, 62);
	    this.pan4.Name = "pan4";
	    this.pan4.Size = new System.Drawing.Size(929, 26);
	    this.pan4.TabIndex = 110;
	    // 
	    // frmPmPaViewmisubs55
	    // 
	    this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
	    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	    this.ClientSize = new System.Drawing.Size(929, 602);
	    this.Controls.Add(this.ssView);
	    this.Controls.Add(this.pan4);
	    this.Controls.Add(this.panel2);
	    this.Controls.Add(this.panTitle);
	    this.Name = "frmPmPaViewmisubs55";
	    this.Text = "퇴원자 심사조정내역";
	    this.Load += new System.EventHandler(this.frmPmPaViewmisubs55_Load);
	    ((System.ComponentModel.ISupportInitialize)(this.ssView_Sheet1)).EndInit();
	    this.panTitle.ResumeLayout(false);
	    this.panTitle.PerformLayout();
	    ((System.ComponentModel.ISupportInitialize)(this.ssView)).EndInit();
	    this.panel2.ResumeLayout(false);
	    this.panel2.PerformLayout();
	    this.pan4.ResumeLayout(false);
	    this.ResumeLayout(false);

	}

	#endregion

	private FarPoint . Win . Spread . SheetView ssView_Sheet1;
	private System . Windows . Forms . Panel panTitle;
	private System . Windows . Forms . Label label15;
	public System . Windows . Forms . Button btnExit;
	private System . Windows . Forms . Label label1;
	private FarPoint . Win . Spread . FpSpread ssView;
	private System . Windows . Forms . Panel panel2;
	private System . Windows . Forms . Button btnView;
	private System . Windows . Forms . Button btnPrint;
	private System . Windows . Forms . Label lblmsg;
	private System . Windows . Forms . Panel pan4;
    }
}