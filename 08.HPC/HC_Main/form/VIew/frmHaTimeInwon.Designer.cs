namespace HC_Main
{
    partial class frmHaTimeInwon
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
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer3 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer4 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panSub01 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboYYMM = new System.Windows.Forms.ComboBox();
            this.panMain = new System.Windows.Forms.Panel();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.prgBar = new System.Windows.Forms.ProgressBar();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.SS3_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panSub02 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.SS3 = new FarPoint.Win.Spread.FpSpread();
            this.panTitle.SuspendLayout();
            this.panSub01.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS3_Sheet1)).BeginInit();
            this.panSub02.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS3)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1385, 40);
            this.panTitle.TabIndex = 17;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(3, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(204, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "종검 검사별 예약정원 조회";
            // 
            // panSub01
            // 
            this.panSub01.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub01.Controls.Add(this.btnSearch);
            this.panSub01.Controls.Add(this.btnSetting);
            this.panSub01.Controls.Add(this.btnExit);
            this.panSub01.Controls.Add(this.groupBox1);
            this.panSub01.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub01.Location = new System.Drawing.Point(0, 40);
            this.panSub01.Name = "panSub01";
            this.panSub01.Size = new System.Drawing.Size(1385, 54);
            this.panSub01.TabIndex = 18;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboYYMM);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(143, 52);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "작업년월";
            // 
            // cboYYMM
            // 
            this.cboYYMM.FormattingEnabled = true;
            this.cboYYMM.Location = new System.Drawing.Point(5, 20);
            this.cboYYMM.Name = "cboYYMM";
            this.cboYYMM.Size = new System.Drawing.Size(132, 25);
            this.cboYYMM.TabIndex = 1;
            // 
            // panMain
            // 
            this.panMain.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMain.Controls.Add(this.SS1);
            this.panMain.Controls.Add(this.prgBar);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 94);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(1385, 543);
            this.panMain.TabIndex = 19;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "SS1, Sheet1, Row 0, Column 0, ";
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SS1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.SS1.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.SS1.HorizontalScrollBar.TabIndex = 106;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS1.Location = new System.Drawing.Point(0, 0);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(1383, 526);
            this.SS1.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SS1.TabIndex = 144;
            this.SS1.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.SS1.VerticalScrollBar.TabIndex = 107;
            this.SS1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 5;
            this.SS1_Sheet1.RowCount = 1;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SS1_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SS1_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SS1_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(3).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(4).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.SS1_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.SS1_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.SS1_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.Rows.Get(0).Height = 24F;
            this.SS1_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SS1_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // prgBar
            // 
            this.prgBar.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.prgBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.prgBar.Location = new System.Drawing.Point(0, 526);
            this.prgBar.Name = "prgBar";
            this.prgBar.Size = new System.Drawing.Size(1383, 15);
            this.prgBar.TabIndex = 143;
            this.prgBar.Value = 30;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1301, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 52);
            this.btnExit.TabIndex = 22;
            this.btnExit.Text = "닫 기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // btnSetting
            // 
            this.btnSetting.BackColor = System.Drawing.Color.White;
            this.btnSetting.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSetting.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSetting.Location = new System.Drawing.Point(1219, 0);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(82, 52);
            this.btnSetting.TabIndex = 24;
            this.btnSetting.Text = "인원세팅";
            this.btnSetting.UseVisualStyleBackColor = false;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(1137, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(82, 52);
            this.btnSearch.TabIndex = 25;
            this.btnSearch.Text = "조 회(&V)";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // SS3_Sheet1
            // 
            this.SS3_Sheet1.Reset();
            this.SS3_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS3_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS3_Sheet1.ColumnCount = 3;
            this.SS3_Sheet1.RowCount = 1;
            this.SS3_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS3_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS3_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SS3_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS3_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS3_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS3_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SS3_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "예약자명";
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "예약시간";
            this.SS3_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "구분";
            this.SS3_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS3_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS3_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SS3_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS3_Sheet1.ColumnHeader.Rows.Get(0).Height = 27F;
            this.SS3_Sheet1.Columns.Get(0).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(235)))), ((int)(((byte)(169)))));
            this.SS3_Sheet1.Columns.Get(0).CellType = textCellType1;
            this.SS3_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS3_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS3_Sheet1.Columns.Get(0).Label = "예약자명";
            this.SS3_Sheet1.Columns.Get(0).Locked = true;
            this.SS3_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS3_Sheet1.Columns.Get(0).Width = 113F;
            this.SS3_Sheet1.Columns.Get(1).CellType = textCellType2;
            this.SS3_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS3_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS3_Sheet1.Columns.Get(1).Label = "예약시간";
            this.SS3_Sheet1.Columns.Get(1).Locked = true;
            this.SS3_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS3_Sheet1.Columns.Get(1).Width = 76F;
            this.SS3_Sheet1.Columns.Get(2).CellType = textCellType3;
            this.SS3_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.SS3_Sheet1.Columns.Get(2).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS3_Sheet1.Columns.Get(2).Label = "구분";
            this.SS3_Sheet1.Columns.Get(2).Locked = true;
            this.SS3_Sheet1.Columns.Get(2).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS3_Sheet1.Columns.Get(2).Width = 56F;
            this.SS3_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS3_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS3_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.SS3_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS3_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS3_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS3_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.SS3_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS3_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS3_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS3_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS3_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS3_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.SS3_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS3_Sheet1.Rows.Get(0).Height = 24F;
            this.SS3_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS3_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS3_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SS3_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS3_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS3_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panSub02
            // 
            this.panSub02.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panSub02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub02.Controls.Add(this.panel1);
            this.panSub02.Controls.Add(this.SS3);
            this.panSub02.Location = new System.Drawing.Point(541, 10);
            this.panSub02.Name = "panSub02";
            this.panSub02.Size = new System.Drawing.Size(302, 732);
            this.panSub02.TabIndex = 146;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 30);
            this.panel1.TabIndex = 146;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDate.Location = new System.Drawing.Point(3, 3);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(96, 21);
            this.lblDate.TabIndex = 147;
            this.lblDate.Text = "2020-04-30";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(121, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 17);
            this.label1.TabIndex = 146;
            this.label1.Text = "드래그 이동가능";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClose.Location = new System.Drawing.Point(231, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(67, 28);
            this.btnClose.TabIndex = 145;
            this.btnClose.Text = "닫 기(&X)";
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // SS3
            // 
            this.SS3.AccessibleDescription = "SS3, Sheet1, Row 0, Column 0, ";
            this.SS3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SS3.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SS3.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS3.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS3.HorizontalScrollBar.Renderer = flatScrollBarRenderer3;
            this.SS3.HorizontalScrollBar.TabIndex = 117;
            this.SS3.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS3.Location = new System.Drawing.Point(0, 47);
            this.SS3.Name = "SS3";
            this.SS3.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS3_Sheet1});
            this.SS3.Size = new System.Drawing.Size(300, 683);
            this.SS3.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SS3.TabIndex = 142;
            this.SS3.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS3.VerticalScrollBar.Name = "";
            flatScrollBarRenderer4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS3.VerticalScrollBar.Renderer = flatScrollBarRenderer4;
            this.SS3.VerticalScrollBar.TabIndex = 118;
            this.SS3.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // frmHaTimeInwon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1385, 637);
            this.Controls.Add(this.panSub02);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panSub01);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHaTimeInwon";
            this.Text = "frmHaTimeInwon";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panSub01.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS3_Sheet1)).EndInit();
            this.panSub02.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panSub01;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboYYMM;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panMain;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.ProgressBar prgBar;
        private FarPoint.Win.Spread.SheetView SS3_Sheet1;
        private System.Windows.Forms.Panel panSub02;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose;
        private FarPoint.Win.Spread.FpSpread SS3;
    }
}