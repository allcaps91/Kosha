namespace ComHpcLibB
{
    partial class frmHcSuga
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
            FarPoint.Win.Spread.FlatFocusIndicatorRenderer flatFocusIndicatorRenderer2 = new FarPoint.Win.Spread.FlatFocusIndicatorRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer3 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            FarPoint.Win.Spread.FlatScrollBarRenderer flatScrollBarRenderer4 = new FarPoint.Win.Spread.FlatScrollBarRenderer();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panSub01 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdoJob3 = new System.Windows.Forms.RadioButton();
            this.rdoJob2 = new System.Windows.Forms.RadioButton();
            this.rdoJob1 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panSub03 = new System.Windows.Forms.Panel();
            this.rdoSuga5 = new System.Windows.Forms.RadioButton();
            this.rdoSuga4 = new System.Windows.Forms.RadioButton();
            this.rdoSuga3 = new System.Windows.Forms.RadioButton();
            this.rdoSuga2 = new System.Windows.Forms.RadioButton();
            this.rdoSuga1 = new System.Windows.Forms.RadioButton();
            this.rdoSuga6 = new System.Windows.Forms.RadioButton();
            this.panel10 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panSub02 = new System.Windows.Forms.Panel();
            this.dtpBDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.prgBar = new System.Windows.Forms.ProgressBar();
            this.panMain = new System.Windows.Forms.Panel();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.chkSuDate = new System.Windows.Forms.CheckBox();
            this.chkDel = new System.Windows.Forms.CheckBox();
            this.chkExamSuga = new System.Windows.Forms.CheckBox();
            this.chkSuCode = new System.Windows.Forms.CheckBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panSub01.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panSub03.SuspendLayout();
            this.panSub02.SuspendLayout();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(1264, 40);
            this.panTitle.TabIndex = 15;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1189, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(73, 38);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "닫기(&X)";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(294, 38);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "일괄수가변경 (종검수가만 일괄변경)";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panSub01
            // 
            this.panSub01.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub01.Controls.Add(this.btnSearch);
            this.panSub01.Controls.Add(this.btnSave);
            this.panSub01.Controls.Add(this.btnRun);
            this.panSub01.Controls.Add(this.panel1);
            this.panSub01.Controls.Add(this.panSub03);
            this.panSub01.Controls.Add(this.panSub02);
            this.panSub01.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub01.Location = new System.Drawing.Point(0, 40);
            this.panSub01.Name = "panSub01";
            this.panSub01.Size = new System.Drawing.Size(1264, 35);
            this.panSub01.TabIndex = 16;
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(1091, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(83, 33);
            this.btnSearch.TabIndex = 16;
            this.btnSearch.Text = "조회(&V)";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(1174, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 33);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "저장(&S)";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnRun
            // 
            this.btnRun.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnRun.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRun.Location = new System.Drawing.Point(853, 0);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(98, 33);
            this.btnRun.TabIndex = 14;
            this.btnRun.Text = "수가일괄적용";
            this.btnRun.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdoJob3);
            this.panel1.Controls.Add(this.rdoJob2);
            this.panel1.Controls.Add(this.rdoJob1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(571, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(282, 33);
            this.panel1.TabIndex = 6;
            // 
            // rdoJob3
            // 
            this.rdoJob3.AutoSize = true;
            this.rdoJob3.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoJob3.Location = new System.Drawing.Point(204, 0);
            this.rdoJob3.Name = "rdoJob3";
            this.rdoJob3.Size = new System.Drawing.Size(78, 33);
            this.rdoJob3.TabIndex = 67;
            this.rdoJob3.Text = "선택제외";
            this.rdoJob3.UseVisualStyleBackColor = true;
            // 
            // rdoJob2
            // 
            this.rdoJob2.AutoSize = true;
            this.rdoJob2.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoJob2.Location = new System.Drawing.Point(126, 0);
            this.rdoJob2.Name = "rdoJob2";
            this.rdoJob2.Size = new System.Drawing.Size(78, 33);
            this.rdoJob2.TabIndex = 66;
            this.rdoJob2.Text = "선택한것";
            this.rdoJob2.UseVisualStyleBackColor = true;
            // 
            // rdoJob1
            // 
            this.rdoJob1.AutoSize = true;
            this.rdoJob1.Checked = true;
            this.rdoJob1.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoJob1.Location = new System.Drawing.Point(74, 0);
            this.rdoJob1.Name = "rdoJob1";
            this.rdoJob1.Size = new System.Drawing.Size(52, 33);
            this.rdoJob1.TabIndex = 65;
            this.rdoJob1.TabStop = true;
            this.rdoJob1.Text = "전체";
            this.rdoJob1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(69, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 33);
            this.panel2.TabIndex = 64;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(3);
            this.label2.Size = new System.Drawing.Size(69, 33);
            this.label2.TabIndex = 4;
            this.label2.Text = "적용범위";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panSub03
            // 
            this.panSub03.Controls.Add(this.rdoSuga5);
            this.panSub03.Controls.Add(this.rdoSuga4);
            this.panSub03.Controls.Add(this.rdoSuga3);
            this.panSub03.Controls.Add(this.rdoSuga2);
            this.panSub03.Controls.Add(this.rdoSuga1);
            this.panSub03.Controls.Add(this.rdoSuga6);
            this.panSub03.Controls.Add(this.panel10);
            this.panSub03.Controls.Add(this.label1);
            this.panSub03.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub03.Location = new System.Drawing.Point(181, 0);
            this.panSub03.Name = "panSub03";
            this.panSub03.Size = new System.Drawing.Size(390, 33);
            this.panSub03.TabIndex = 5;
            // 
            // rdoSuga5
            // 
            this.rdoSuga5.AutoSize = true;
            this.rdoSuga5.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoSuga5.Location = new System.Drawing.Point(334, 0);
            this.rdoSuga5.Name = "rdoSuga5";
            this.rdoSuga5.Size = new System.Drawing.Size(52, 33);
            this.rdoSuga5.TabIndex = 74;
            this.rdoSuga5.Text = "임의";
            this.rdoSuga5.UseVisualStyleBackColor = true;
            // 
            // rdoSuga4
            // 
            this.rdoSuga4.AutoSize = true;
            this.rdoSuga4.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoSuga4.Location = new System.Drawing.Point(282, 0);
            this.rdoSuga4.Name = "rdoSuga4";
            this.rdoSuga4.Size = new System.Drawing.Size(52, 33);
            this.rdoSuga4.TabIndex = 73;
            this.rdoSuga4.Text = "조정";
            this.rdoSuga4.UseVisualStyleBackColor = true;
            // 
            // rdoSuga3
            // 
            this.rdoSuga3.AutoSize = true;
            this.rdoSuga3.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoSuga3.Location = new System.Drawing.Point(230, 0);
            this.rdoSuga3.Name = "rdoSuga3";
            this.rdoSuga3.Size = new System.Drawing.Size(52, 33);
            this.rdoSuga3.TabIndex = 72;
            this.rdoSuga3.Text = "특수";
            this.rdoSuga3.UseVisualStyleBackColor = true;
            // 
            // rdoSuga2
            // 
            this.rdoSuga2.AutoSize = true;
            this.rdoSuga2.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoSuga2.Location = new System.Drawing.Point(178, 0);
            this.rdoSuga2.Name = "rdoSuga2";
            this.rdoSuga2.Size = new System.Drawing.Size(52, 33);
            this.rdoSuga2.TabIndex = 71;
            this.rdoSuga2.Text = "공단";
            this.rdoSuga2.UseVisualStyleBackColor = true;
            // 
            // rdoSuga1
            // 
            this.rdoSuga1.AutoSize = true;
            this.rdoSuga1.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoSuga1.Location = new System.Drawing.Point(126, 0);
            this.rdoSuga1.Name = "rdoSuga1";
            this.rdoSuga1.Size = new System.Drawing.Size(52, 33);
            this.rdoSuga1.TabIndex = 70;
            this.rdoSuga1.Text = "종검";
            this.rdoSuga1.UseVisualStyleBackColor = true;
            // 
            // rdoSuga6
            // 
            this.rdoSuga6.AutoSize = true;
            this.rdoSuga6.Checked = true;
            this.rdoSuga6.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoSuga6.Location = new System.Drawing.Point(74, 0);
            this.rdoSuga6.Name = "rdoSuga6";
            this.rdoSuga6.Size = new System.Drawing.Size(52, 33);
            this.rdoSuga6.TabIndex = 69;
            this.rdoSuga6.TabStop = true;
            this.rdoSuga6.Text = "전체";
            this.rdoSuga6.UseVisualStyleBackColor = true;
            // 
            // panel10
            // 
            this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel10.Location = new System.Drawing.Point(69, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(5, 33);
            this.panel10.TabIndex = 63;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(3);
            this.label1.Size = new System.Drawing.Size(69, 33);
            this.label1.TabIndex = 4;
            this.label1.Text = "조회구분";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panSub02
            // 
            this.panSub02.Controls.Add(this.dtpBDate);
            this.panSub02.Controls.Add(this.label3);
            this.panSub02.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub02.Location = new System.Drawing.Point(0, 0);
            this.panSub02.Name = "panSub02";
            this.panSub02.Size = new System.Drawing.Size(181, 33);
            this.panSub02.TabIndex = 4;
            // 
            // dtpBDate
            // 
            this.dtpBDate.CalendarMonthBackground = System.Drawing.Color.White;
            this.dtpBDate.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpBDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpBDate.Location = new System.Drawing.Point(72, 4);
            this.dtpBDate.Name = "dtpBDate";
            this.dtpBDate.Size = new System.Drawing.Size(106, 25);
            this.dtpBDate.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(3);
            this.label3.Size = new System.Drawing.Size(69, 33);
            this.label3.TabIndex = 3;
            this.label3.Text = "적용일자";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // prgBar
            // 
            this.prgBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.prgBar.Location = new System.Drawing.Point(0, 720);
            this.prgBar.Name = "prgBar";
            this.prgBar.Size = new System.Drawing.Size(1264, 22);
            this.prgBar.TabIndex = 19;
            this.prgBar.Value = 20;
            // 
            // panMain
            // 
            this.panMain.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMain.Controls.Add(this.SS1);
            this.panMain.Controls.Add(this.panel4);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 75);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(1264, 645);
            this.panMain.TabIndex = 20;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "";
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.FocusRenderer = flatFocusIndicatorRenderer2;
            this.SS1.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.HorizontalScrollBar.Renderer = flatScrollBarRenderer3;
            this.SS1.HorizontalScrollBar.TabIndex = 110;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS1.Location = new System.Drawing.Point(0, 35);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(1262, 608);
            this.SS1.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SS1.TabIndex = 18;
            this.SS1.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.VerticalScrollBar.Name = "";
            flatScrollBarRenderer4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.VerticalScrollBar.Renderer = flatScrollBarRenderer4;
            this.SS1.VerticalScrollBar.TabIndex = 111;
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
            this.SS1_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SS1_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SS1_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SS1_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SS1_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.panel3);
            this.panel4.Controls.Add(this.panel8);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1262, 35);
            this.panel4.TabIndex = 17;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtSearch);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(570, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(214, 33);
            this.panel3.TabIndex = 7;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(93, 4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(116, 25);
            this.txtSearch.TabIndex = 4;
            this.txtSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(3);
            this.label4.Size = new System.Drawing.Size(89, 33);
            this.label4.TabIndex = 3;
            this.label4.Text = "조회수가명";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.chkSuDate);
            this.panel8.Controls.Add(this.chkDel);
            this.panel8.Controls.Add(this.chkExamSuga);
            this.panel8.Controls.Add(this.chkSuCode);
            this.panel8.Controls.Add(this.panel9);
            this.panel8.Controls.Add(this.label7);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(570, 33);
            this.panel8.TabIndex = 5;
            // 
            // chkSuDate
            // 
            this.chkSuDate.AutoSize = true;
            this.chkSuDate.Location = new System.Drawing.Point(306, 6);
            this.chkSuDate.Name = "chkSuDate";
            this.chkSuDate.Size = new System.Drawing.Size(144, 21);
            this.chkSuDate.TabIndex = 67;
            this.chkSuDate.Text = "적용일자이전변경건";
            this.chkSuDate.UseVisualStyleBackColor = true;
            // 
            // chkDel
            // 
            this.chkDel.AutoSize = true;
            this.chkDel.ForeColor = System.Drawing.Color.DarkRed;
            this.chkDel.Location = new System.Drawing.Point(456, 6);
            this.chkDel.Name = "chkDel";
            this.chkDel.Size = new System.Drawing.Size(105, 21);
            this.chkDel.TabIndex = 66;
            this.chkDel.Text = "삭제코드포함";
            this.chkDel.UseVisualStyleBackColor = true;
            // 
            // chkExamSuga
            // 
            this.chkExamSuga.AutoSize = true;
            this.chkExamSuga.Location = new System.Drawing.Point(186, 6);
            this.chkExamSuga.Name = "chkExamSuga";
            this.chkExamSuga.Size = new System.Drawing.Size(118, 21);
            this.chkExamSuga.TabIndex = 65;
            this.chkExamSuga.Text = "검사실코드매칭";
            this.chkExamSuga.UseVisualStyleBackColor = true;
            // 
            // chkSuCode
            // 
            this.chkSuCode.AutoSize = true;
            this.chkSuCode.Checked = true;
            this.chkSuCode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSuCode.Location = new System.Drawing.Point(75, 6);
            this.chkSuCode.Name = "chkSuCode";
            this.chkSuCode.Size = new System.Drawing.Size(105, 21);
            this.chkSuCode.TabIndex = 64;
            this.chkSuCode.Text = "수가코드매칭";
            this.chkSuCode.UseVisualStyleBackColor = true;
            // 
            // panel9
            // 
            this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel9.Location = new System.Drawing.Point(69, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(5, 33);
            this.panel9.TabIndex = 63;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Dock = System.Windows.Forms.DockStyle.Left;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(3);
            this.label7.Size = new System.Drawing.Size(69, 33);
            this.label7.TabIndex = 4;
            this.label7.Text = "조회구분";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmHcSuga
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 742);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.prgBar);
            this.Controls.Add(this.panSub01);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcSuga";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "일괄수가변경";
            this.panTitle.ResumeLayout(false);
            this.panSub01.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panSub03.ResumeLayout(false);
            this.panSub03.PerformLayout();
            this.panSub02.ResumeLayout(false);
            this.panMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panSub01;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panSub03;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panSub02;
        private System.Windows.Forms.DateTimePicker dtpBDate;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ProgressBar prgBar;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.RadioButton rdoJob3;
        private System.Windows.Forms.RadioButton rdoJob2;
        private System.Windows.Forms.RadioButton rdoJob1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.RadioButton rdoSuga5;
        private System.Windows.Forms.RadioButton rdoSuga4;
        private System.Windows.Forms.RadioButton rdoSuga3;
        private System.Windows.Forms.RadioButton rdoSuga2;
        private System.Windows.Forms.RadioButton rdoSuga1;
        private System.Windows.Forms.RadioButton rdoSuga6;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkDel;
        private System.Windows.Forms.CheckBox chkExamSuga;
        private System.Windows.Forms.CheckBox chkSuCode;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkSuDate;
    }
}