namespace HC_Pan
{
    partial class frmHcPanYearlungcapacityExamCnt
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
            FarPoint.Win.Spread.FlatFocusIndicatorRenderer flatFocusIndicatorRenderer1 = new FarPoint.Win.Spread.FlatFocusIndicatorRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer1 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer2 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
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
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblRate = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(972, 35);
            this.panTitle.TabIndex = 19;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(888, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 33);
            this.btnExit.TabIndex = 20;
            this.btnExit.Text = "닫 기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(3, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(398, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "연도별 폐활량검사 대상자수 및 폐기능 이상자수 현황";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblRate);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.cboYear);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(972, 40);
            this.panel1.TabIndex = 20;
            // 
            // lblRate
            // 
            this.lblRate.AutoSize = true;
            this.lblRate.Location = new System.Drawing.Point(915, 10);
            this.lblRate.Name = "lblRate";
            this.lblRate.Size = new System.Drawing.Size(40, 17);
            this.lblRate.TabIndex = 28;
            this.lblRate.Text = "100%";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.progressBar1);
            this.panel3.Location = new System.Drawing.Point(602, 9);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(305, 22);
            this.panel3.TabIndex = 27;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(0, 0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(303, 20);
            this.progressBar1.TabIndex = 1;
            // 
            // cboYear
            // 
            this.cboYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Location = new System.Drawing.Point(74, 6);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(75, 25);
            this.cboYear.TabIndex = 26;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.LightBlue;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(7, 6);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 25);
            this.label10.TabIndex = 25;
            this.label10.Text = "작업년도";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(153, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(82, 29);
            this.btnSearch.TabIndex = 24;
            this.btnSearch.Text = "조회(&V)";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.White;
            this.btnPrint.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.Location = new System.Drawing.Point(238, 5);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(82, 29);
            this.btnPrint.TabIndex = 23;
            this.btnPrint.Text = "인쇄(&P)";
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.SS1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 75);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(972, 131);
            this.panel2.TabIndex = 21;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "SS1, Sheet1, Row 0, Column 0, ";
            this.SS1.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SS1.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.SS1.HorizontalScrollBar.TabIndex = 15;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS1.Location = new System.Drawing.Point(6, 5);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(959, 118);
            this.SS1.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SS1.TabIndex = 0;
            this.SS1.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.SS1.VerticalScrollBar.TabIndex = 16;
            this.SS1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 14;
            this.SS1_Sheet1.RowCount = 3;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "년도";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "대상자수";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "이상자수";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "비율(%)";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "제한성";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "비율(%)";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "폐쇄성";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "비율(%)";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "혼합성";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "비율(%)";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "대상자BMI30";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "이상자BMI30";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "G/F(%)";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "F/B(%)";
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.Rows.Get(0).Height = 27F;
            this.SS1_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.SS1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Label = "년도";
            this.SS1_Sheet1.Columns.Get(0).Locked = true;
            this.SS1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Width = 61F;
            this.SS1_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.SS1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(1).Label = "대상자수";
            this.SS1_Sheet1.Columns.Get(1).Locked = true;
            this.SS1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(1).Width = 65F;
            this.SS1_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.SS1_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Label = "이상자수";
            this.SS1_Sheet1.Columns.Get(2).Locked = true;
            this.SS1_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(2).Width = 65F;
            this.SS1_Sheet1.Columns.Get(3).CellType = textCellType4;
            this.SS1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(3).Label = "비율(%)";
            this.SS1_Sheet1.Columns.Get(3).Locked = true;
            this.SS1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(3).Width = 65F;
            this.SS1_Sheet1.Columns.Get(4).CellType = textCellType5;
            this.SS1_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(4).Label = "제한성";
            this.SS1_Sheet1.Columns.Get(4).Locked = true;
            this.SS1_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(4).Width = 65F;
            this.SS1_Sheet1.Columns.Get(5).CellType = textCellType6;
            this.SS1_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(5).Label = "비율(%)";
            this.SS1_Sheet1.Columns.Get(5).Locked = true;
            this.SS1_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(5).Width = 65F;
            this.SS1_Sheet1.Columns.Get(6).CellType = textCellType7;
            this.SS1_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(6).Label = "폐쇄성";
            this.SS1_Sheet1.Columns.Get(6).Locked = true;
            this.SS1_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(6).Width = 65F;
            this.SS1_Sheet1.Columns.Get(7).CellType = textCellType8;
            this.SS1_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(7).Label = "비율(%)";
            this.SS1_Sheet1.Columns.Get(7).Locked = true;
            this.SS1_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(7).Width = 65F;
            this.SS1_Sheet1.Columns.Get(8).CellType = textCellType9;
            this.SS1_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(8).Label = "혼합성";
            this.SS1_Sheet1.Columns.Get(8).Locked = true;
            this.SS1_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(8).Width = 65F;
            this.SS1_Sheet1.Columns.Get(9).CellType = textCellType10;
            this.SS1_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(9).Label = "비율(%)";
            this.SS1_Sheet1.Columns.Get(9).Locked = true;
            this.SS1_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(9).Width = 65F;
            this.SS1_Sheet1.Columns.Get(10).CellType = textCellType11;
            this.SS1_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(10).Label = "대상자BMI30";
            this.SS1_Sheet1.Columns.Get(10).Locked = true;
            this.SS1_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(10).Width = 86F;
            this.SS1_Sheet1.Columns.Get(11).CellType = textCellType12;
            this.SS1_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(11).Label = "이상자BMI30";
            this.SS1_Sheet1.Columns.Get(11).Locked = true;
            this.SS1_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(11).Width = 86F;
            this.SS1_Sheet1.Columns.Get(12).CellType = textCellType13;
            this.SS1_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(12).Label = "G/F(%)";
            this.SS1_Sheet1.Columns.Get(12).Locked = true;
            this.SS1_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(12).Width = 51F;
            this.SS1_Sheet1.Columns.Get(13).CellType = textCellType14;
            this.SS1_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(13).Label = "F/B(%)";
            this.SS1_Sheet1.Columns.Get(13).Locked = true;
            this.SS1_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(13).Width = 51F;
            this.SS1_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.SS1_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.SS1_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.Rows.Get(0).Height = 29F;
            this.SS1_Sheet1.Rows.Get(1).Height = 29F;
            this.SS1_Sheet1.Rows.Get(2).Height = 29F;
            this.SS1_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SS1_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmHcPanYearlungcapacityExamCnt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(972, 206);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcPanYearlungcapacityExamCnt";
            this.Text = "연도별 폐활량검사 대상자수 및 폐기능 이상자수 현황";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.Label label10;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblRate;
    }
}