namespace ComPmpaLibB
{
    partial class frmMisuEdiView
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
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ko-KR", false);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMisuEdiView));
            FarPoint.Win.Spread.CellType.CurrencyCellType currencyCellType2 = new FarPoint.Win.Spread.CellType.CurrencyCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType10 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType11 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType12 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType13 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType14 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType15 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType16 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType17 = new FarPoint.Win.Spread.CellType.NumberCellType();
            FarPoint.Win.Spread.CellType.NumberCellType numberCellType18 = new FarPoint.Win.Spread.CellType.NumberCellType();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.grbMir = new System.Windows.Forms.GroupBox();
            this.cboB = new System.Windows.Forms.ComboBox();
            this.grbWeek = new System.Windows.Forms.GroupBox();
            this.cboA = new System.Windows.Forms.ComboBox();
            this.grbJong = new System.Windows.Forms.GroupBox();
            this.cboJong = new System.Windows.Forms.ComboBox();
            this.grbYYMM = new System.Windows.Forms.GroupBox();
            this.cboYYMM = new System.Windows.Forms.ComboBox();
            this.grbIO = new System.Windows.Forms.GroupBox();
            this.rdoIO3 = new System.Windows.Forms.RadioButton();
            this.rdoIO2 = new System.Windows.Forms.RadioButton();
            this.rdoIO1 = new System.Windows.Forms.RadioButton();
            this.grbJob = new System.Windows.Forms.GroupBox();
            this.rdoJob2 = new System.Windows.Forms.RadioButton();
            this.rdoJob1 = new System.Windows.Forms.RadioButton();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panBottom = new System.Windows.Forms.Panel();
            this.lblMsg = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitleSub0.SuspendLayout();
            this.grbMir.SuspendLayout();
            this.grbWeek.SuspendLayout();
            this.grbJong.SuspendLayout();
            this.grbYYMM.SuspendLayout();
            this.grbIO.SuspendLayout();
            this.grbJob.SuspendLayout();
            this.panTitle.SuspendLayout();
            this.panBottom.SuspendLayout();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.Transparent;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitleSub0.Controls.Add(this.grbMir);
            this.panTitleSub0.Controls.Add(this.grbWeek);
            this.panTitleSub0.Controls.Add(this.grbJong);
            this.panTitleSub0.Controls.Add(this.grbYYMM);
            this.panTitleSub0.Controls.Add(this.grbIO);
            this.panTitleSub0.Controls.Add(this.grbJob);
            this.panTitleSub0.Controls.Add(this.btnSearch);
            this.panTitleSub0.Controls.Add(this.btnClear);
            this.panTitleSub0.Controls.Add(this.btnPrint);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 36);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Padding = new System.Windows.Forms.Padding(3);
            this.panTitleSub0.Size = new System.Drawing.Size(1264, 56);
            this.panTitleSub0.TabIndex = 21;
            // 
            // grbMir
            // 
            this.grbMir.AutoSize = true;
            this.grbMir.Controls.Add(this.cboB);
            this.grbMir.Dock = System.Windows.Forms.DockStyle.Left;
            this.grbMir.Location = new System.Drawing.Point(654, 3);
            this.grbMir.Name = "grbMir";
            this.grbMir.Size = new System.Drawing.Size(122, 48);
            this.grbMir.TabIndex = 17;
            this.grbMir.TabStop = false;
            this.grbMir.Text = "청구구분";
            // 
            // cboB
            // 
            this.cboB.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboB.FormattingEnabled = true;
            this.cboB.Items.AddRange(new object[] {
            "*.전체",
            "0.일반청구",
            "1.보완(재)청구",
            "2.추가청구",
            "4.NP정액"});
            this.cboB.Location = new System.Drawing.Point(3, 21);
            this.cboB.Name = "cboB";
            this.cboB.Size = new System.Drawing.Size(116, 25);
            this.cboB.TabIndex = 0;
            this.cboB.Text = "*.전체";
            // 
            // grbWeek
            // 
            this.grbWeek.AutoSize = true;
            this.grbWeek.Controls.Add(this.cboA);
            this.grbWeek.Dock = System.Windows.Forms.DockStyle.Left;
            this.grbWeek.Location = new System.Drawing.Point(532, 3);
            this.grbWeek.Name = "grbWeek";
            this.grbWeek.Size = new System.Drawing.Size(122, 48);
            this.grbWeek.TabIndex = 16;
            this.grbWeek.TabStop = false;
            this.grbWeek.Text = "주별구분";
            // 
            // cboA
            // 
            this.cboA.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboA.FormattingEnabled = true;
            this.cboA.Items.AddRange(new object[] {
            "*.전체",
            "A.중간청구제외",
            "1.1주차",
            "2.2주차",
            "3.3주차",
            "4.4주차",
            "5.5주차",
            "6.외래및퇴원청구",
            "7.중간청구"});
            this.cboA.Location = new System.Drawing.Point(3, 21);
            this.cboA.Name = "cboA";
            this.cboA.Size = new System.Drawing.Size(116, 25);
            this.cboA.TabIndex = 0;
            this.cboA.Text = "*.전체";
            // 
            // grbJong
            // 
            this.grbJong.AutoSize = true;
            this.grbJong.Controls.Add(this.cboJong);
            this.grbJong.Dock = System.Windows.Forms.DockStyle.Left;
            this.grbJong.Location = new System.Drawing.Point(427, 3);
            this.grbJong.Name = "grbJong";
            this.grbJong.Size = new System.Drawing.Size(105, 48);
            this.grbJong.TabIndex = 15;
            this.grbJong.TabStop = false;
            this.grbJong.Text = "미수종류";
            // 
            // cboJong
            // 
            this.cboJong.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboJong.FormattingEnabled = true;
            this.cboJong.Items.AddRange(new object[] {
            "1.건강보험",
            "2.의료급여",
            "3.산재"});
            this.cboJong.Location = new System.Drawing.Point(3, 21);
            this.cboJong.Name = "cboJong";
            this.cboJong.Size = new System.Drawing.Size(99, 25);
            this.cboJong.TabIndex = 0;
            this.cboJong.Text = "1.건강보험";
            // 
            // grbYYMM
            // 
            this.grbYYMM.AutoSize = true;
            this.grbYYMM.Controls.Add(this.cboYYMM);
            this.grbYYMM.Dock = System.Windows.Forms.DockStyle.Left;
            this.grbYYMM.Location = new System.Drawing.Point(327, 3);
            this.grbYYMM.Name = "grbYYMM";
            this.grbYYMM.Size = new System.Drawing.Size(100, 48);
            this.grbYYMM.TabIndex = 14;
            this.grbYYMM.TabStop = false;
            this.grbYYMM.Text = "진료월구분";
            // 
            // cboYYMM
            // 
            this.cboYYMM.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboYYMM.FormattingEnabled = true;
            this.cboYYMM.Location = new System.Drawing.Point(3, 21);
            this.cboYYMM.Name = "cboYYMM";
            this.cboYYMM.Size = new System.Drawing.Size(94, 25);
            this.cboYYMM.TabIndex = 0;
            this.cboYYMM.Text = "299912";
            // 
            // grbIO
            // 
            this.grbIO.AutoSize = true;
            this.grbIO.Controls.Add(this.rdoIO3);
            this.grbIO.Controls.Add(this.rdoIO2);
            this.grbIO.Controls.Add(this.rdoIO1);
            this.grbIO.Dock = System.Windows.Forms.DockStyle.Left;
            this.grbIO.Location = new System.Drawing.Point(165, 3);
            this.grbIO.Name = "grbIO";
            this.grbIO.Size = new System.Drawing.Size(162, 48);
            this.grbIO.TabIndex = 13;
            this.grbIO.TabStop = false;
            this.grbIO.Text = "외래/입원";
            // 
            // rdoIO3
            // 
            this.rdoIO3.AutoSize = true;
            this.rdoIO3.Checked = true;
            this.rdoIO3.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoIO3.Location = new System.Drawing.Point(107, 21);
            this.rdoIO3.Name = "rdoIO3";
            this.rdoIO3.Size = new System.Drawing.Size(52, 24);
            this.rdoIO3.TabIndex = 5;
            this.rdoIO3.TabStop = true;
            this.rdoIO3.Text = "전체";
            this.rdoIO3.UseVisualStyleBackColor = true;
            // 
            // rdoIO2
            // 
            this.rdoIO2.AutoSize = true;
            this.rdoIO2.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoIO2.Location = new System.Drawing.Point(55, 21);
            this.rdoIO2.Name = "rdoIO2";
            this.rdoIO2.Size = new System.Drawing.Size(52, 24);
            this.rdoIO2.TabIndex = 4;
            this.rdoIO2.Text = "입원";
            this.rdoIO2.UseVisualStyleBackColor = true;
            // 
            // rdoIO1
            // 
            this.rdoIO1.AutoSize = true;
            this.rdoIO1.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoIO1.Location = new System.Drawing.Point(3, 21);
            this.rdoIO1.Name = "rdoIO1";
            this.rdoIO1.Size = new System.Drawing.Size(52, 24);
            this.rdoIO1.TabIndex = 3;
            this.rdoIO1.Text = "외래";
            this.rdoIO1.UseVisualStyleBackColor = true;
            // 
            // grbJob
            // 
            this.grbJob.AutoSize = true;
            this.grbJob.Controls.Add(this.rdoJob2);
            this.grbJob.Controls.Add(this.rdoJob1);
            this.grbJob.Dock = System.Windows.Forms.DockStyle.Left;
            this.grbJob.Location = new System.Drawing.Point(3, 3);
            this.grbJob.Name = "grbJob";
            this.grbJob.Size = new System.Drawing.Size(162, 48);
            this.grbJob.TabIndex = 12;
            this.grbJob.TabStop = false;
            this.grbJob.Text = "작업구분";
            // 
            // rdoJob2
            // 
            this.rdoJob2.AutoSize = true;
            this.rdoJob2.Checked = true;
            this.rdoJob2.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoJob2.Location = new System.Drawing.Point(81, 21);
            this.rdoJob2.Name = "rdoJob2";
            this.rdoJob2.Size = new System.Drawing.Size(78, 24);
            this.rdoJob2.TabIndex = 4;
            this.rdoJob2.TabStop = true;
            this.rdoJob2.Text = "진료월별";
            this.rdoJob2.UseVisualStyleBackColor = true;
            this.rdoJob2.CheckedChanged += new System.EventHandler(this.rdoJob2_CheckedChanged);
            // 
            // rdoJob1
            // 
            this.rdoJob1.AutoSize = true;
            this.rdoJob1.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoJob1.Location = new System.Drawing.Point(3, 21);
            this.rdoJob1.Name = "rdoJob1";
            this.rdoJob1.Size = new System.Drawing.Size(78, 24);
            this.rdoJob1.TabIndex = 3;
            this.rdoJob1.Text = "접수일자";
            this.rdoJob1.UseVisualStyleBackColor = true;
            this.rdoJob1.CheckedChanged += new System.EventHandler(this.rdoJob1_CheckedChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(926, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(111, 48);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Transparent;
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClear.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClear.Location = new System.Drawing.Point(1037, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(111, 48);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "취소";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.Location = new System.Drawing.Point(1148, 3);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(111, 48);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Padding = new System.Windows.Forms.Padding(1);
            this.panTitle.Size = new System.Drawing.Size(1264, 36);
            this.panTitle.TabIndex = 20;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1148, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(111, 30);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(4, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(220, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "EDI 접수증 조회 및 자료변경";
            // 
            // panBottom
            // 
            this.panBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(223)))), ((int)(((byte)(247)))));
            this.panBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panBottom.Controls.Add(this.lblMsg);
            this.panBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panBottom.Location = new System.Drawing.Point(0, 628);
            this.panBottom.Name = "panBottom";
            this.panBottom.Padding = new System.Windows.Forms.Padding(5);
            this.panBottom.Size = new System.Drawing.Size(1264, 31);
            this.panBottom.TabIndex = 23;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblMsg.Location = new System.Drawing.Point(5, 5);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(155, 17);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Text = "EDI_VIEW Help Message";
            // 
            // panMain
            // 
            this.panMain.BackColor = System.Drawing.Color.Transparent;
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMain.Controls.Add(this.SS1);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 92);
            this.panMain.Name = "panMain";
            this.panMain.Padding = new System.Windows.Forms.Padding(1);
            this.panMain.Size = new System.Drawing.Size(1264, 536);
            this.panMain.TabIndex = 24;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "SS1, Sheet1, Row 0, Column 0, ";
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.Location = new System.Drawing.Point(1, 1);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(1260, 532);
            this.SS1.TabIndex = 0;
            this.SS1.EditModeOff += new System.EventHandler(this.SS1_EditModeOff);
            this.SS1.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SS1_CellClick);
            this.SS1.CellDoubleClick += new FarPoint.Win.Spread.CellClickEventHandler(this.SS1_CellDoubleClick);
            // 
            // SS1_Sheet1
            // 
            this.SS1_Sheet1.Reset();
            this.SS1_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SS1_Sheet1.ColumnCount = 20;
            this.SS1_Sheet1.RowCount = 50;
            this.SS1_Sheet1.Cells.Get(0, 1).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Cells.Get(0, 1).ParseFormatInfo = ((System.Globalization.DateTimeFormatInfo)(cultureInfo.DateTimeFormat.Clone()));
            this.SS1_Sheet1.Cells.Get(0, 1).ParseFormatString = "yyyy-MM-dd";
            this.SS1_Sheet1.Cells.Get(0, 1).Value = new System.DateTime(2999, 12, 31, 0, 0, 0, 0);
            this.SS1_Sheet1.Cells.Get(0, 5).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Cells.Get(0, 5).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 5).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 5).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SS1_Sheet1.Cells.Get(0, 5).ParseFormatString = "n";
            this.SS1_Sheet1.Cells.Get(0, 5).Value = 201705;
            this.SS1_Sheet1.Cells.Get(0, 8).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Cells.Get(0, 8).Value = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.SS1_Sheet1.Cells.Get(0, 9).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Cells.Get(0, 9).Value = 999999999999D;
            this.SS1_Sheet1.Cells.Get(0, 10).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Cells.Get(0, 10).Value = 999999999999D;
            this.SS1_Sheet1.Cells.Get(0, 11).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Cells.Get(0, 11).Value = 999999999999D;
            this.SS1_Sheet1.Cells.Get(0, 12).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Cells.Get(0, 12).Value = 999999999999D;
            this.SS1_Sheet1.Cells.Get(0, 13).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Cells.Get(0, 13).Value = 999999999999D;
            this.SS1_Sheet1.Cells.Get(0, 14).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Cells.Get(0, 14).Value = 999999999999D;
            this.SS1_Sheet1.Cells.Get(0, 15).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Cells.Get(0, 15).Value = 999999999999D;
            this.SS1_Sheet1.Cells.Get(0, 16).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Cells.Get(0, 16).Value = 999999999999D;
            this.SS1_Sheet1.Cells.Get(0, 17).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Cells.Get(0, 17).Value = 999999999999D;
            this.SS1_Sheet1.Cells.Get(0, 18).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Cells.Get(0, 18).ParseFormatInfo = ((System.Globalization.NumberFormatInfo)(cultureInfo.NumberFormat.Clone()));
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 18).ParseFormatInfo)).NumberDecimalDigits = 0;
            ((System.Globalization.NumberFormatInfo)(this.SS1_Sheet1.Cells.Get(0, 18).ParseFormatInfo)).NumberGroupSizes = new int[] {
        0};
            this.SS1_Sheet1.Cells.Get(0, 18).ParseFormatString = "n";
            this.SS1_Sheet1.Cells.Get(0, 18).Value = 2017051024;
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 0).Value = "종류";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "접수일자";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "접수번호";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "외/입";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "분야";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "진료월";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "주별";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "구분";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "건수";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "접수총진료비";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "접수조합부담";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "상한대불";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "장애인기금";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "희귀지원금";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "결핵지원금";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 15).Value = "약제상한액";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 16).Value = "100/100미만";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 17).Value = "국가재난지원(본인)";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 18).Value = "청구번호";
            this.SS1_Sheet1.ColumnHeader.Cells.Get(0, 19).Value = "ROWID";
            this.SS1_Sheet1.ColumnHeader.Rows.Get(0).Height = 26F;
            this.SS1_Sheet1.Columns.Get(0).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Label = "종류";
            this.SS1_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(0).Width = 45F;
            this.SS1_Sheet1.Columns.Get(1).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(1).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(1).Label = "접수일자";
            this.SS1_Sheet1.Columns.Get(1).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(1).Width = 74F;
            this.SS1_Sheet1.Columns.Get(2).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(2).Label = "접수번호";
            this.SS1_Sheet1.Columns.Get(2).Width = 62F;
            this.SS1_Sheet1.Columns.Get(3).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(3).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(3).Label = "외/입";
            this.SS1_Sheet1.Columns.Get(3).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(3).Width = 41F;
            this.SS1_Sheet1.Columns.Get(4).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(4).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(4).Label = "분야";
            this.SS1_Sheet1.Columns.Get(4).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(4).Width = 36F;
            this.SS1_Sheet1.Columns.Get(5).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(5).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(5).Label = "진료월";
            this.SS1_Sheet1.Columns.Get(5).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(5).Width = 50F;
            this.SS1_Sheet1.Columns.Get(6).BackColor = System.Drawing.Color.PaleTurquoise;
            this.SS1_Sheet1.Columns.Get(6).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(6).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(6).Label = "주별";
            this.SS1_Sheet1.Columns.Get(6).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(6).Width = 36F;
            this.SS1_Sheet1.Columns.Get(7).BackColor = System.Drawing.Color.PaleTurquoise;
            this.SS1_Sheet1.Columns.Get(7).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(7).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(7).Label = "구분";
            this.SS1_Sheet1.Columns.Get(7).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(7).Width = 36F;
            currencyCellType2.DecimalPlaces = 0;
            currencyCellType2.MaximumValue = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            currencyCellType2.MinimumValue = new decimal(new int[] {
            999999999,
            0,
            0,
            -2147483648});
            currencyCellType2.Separator = ",";
            currencyCellType2.ShowCurrencySymbol = false;
            currencyCellType2.ShowSeparator = true;
            this.SS1_Sheet1.Columns.Get(8).CellType = currencyCellType2;
            this.SS1_Sheet1.Columns.Get(8).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(8).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS1_Sheet1.Columns.Get(8).Label = "건수";
            this.SS1_Sheet1.Columns.Get(8).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(8).Width = 39F;
            numberCellType10.DecimalPlaces = 0;
            numberCellType10.MaximumValue = 999999999999D;
            numberCellType10.MinimumValue = -999999999999D;
            numberCellType10.NegativeRed = true;
            numberCellType10.Separator = ",";
            numberCellType10.ShowSeparator = true;
            this.SS1_Sheet1.Columns.Get(9).CellType = numberCellType10;
            this.SS1_Sheet1.Columns.Get(9).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(9).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS1_Sheet1.Columns.Get(9).Label = "접수총진료비";
            this.SS1_Sheet1.Columns.Get(9).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(9).Width = 101F;
            numberCellType11.DecimalPlaces = 0;
            numberCellType11.MaximumValue = 999999999999D;
            numberCellType11.MinimumValue = -999999999999D;
            numberCellType11.NegativeRed = true;
            numberCellType11.Separator = ",";
            numberCellType11.ShowSeparator = true;
            this.SS1_Sheet1.Columns.Get(10).CellType = numberCellType11;
            this.SS1_Sheet1.Columns.Get(10).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(10).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS1_Sheet1.Columns.Get(10).Label = "접수조합부담";
            this.SS1_Sheet1.Columns.Get(10).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(10).Width = 101F;
            numberCellType12.DecimalPlaces = 0;
            numberCellType12.MaximumValue = 999999999999D;
            numberCellType12.MinimumValue = -999999999999D;
            numberCellType12.NegativeRed = true;
            numberCellType12.Separator = ",";
            numberCellType12.ShowSeparator = true;
            this.SS1_Sheet1.Columns.Get(11).CellType = numberCellType12;
            this.SS1_Sheet1.Columns.Get(11).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(11).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS1_Sheet1.Columns.Get(11).Label = "상한대불";
            this.SS1_Sheet1.Columns.Get(11).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(11).Width = 101F;
            numberCellType13.DecimalPlaces = 0;
            numberCellType13.MaximumValue = 999999999999D;
            numberCellType13.MinimumValue = -999999999999D;
            numberCellType13.NegativeRed = true;
            numberCellType13.Separator = ",";
            numberCellType13.ShowSeparator = true;
            this.SS1_Sheet1.Columns.Get(12).CellType = numberCellType13;
            this.SS1_Sheet1.Columns.Get(12).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(12).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS1_Sheet1.Columns.Get(12).Label = "장애인기금";
            this.SS1_Sheet1.Columns.Get(12).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(12).Width = 101F;
            numberCellType14.DecimalPlaces = 0;
            numberCellType14.MaximumValue = 999999999999D;
            numberCellType14.MinimumValue = -999999999999D;
            numberCellType14.NegativeRed = true;
            numberCellType14.Separator = ",";
            numberCellType14.ShowSeparator = true;
            this.SS1_Sheet1.Columns.Get(13).CellType = numberCellType14;
            this.SS1_Sheet1.Columns.Get(13).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(13).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS1_Sheet1.Columns.Get(13).Label = "희귀지원금";
            this.SS1_Sheet1.Columns.Get(13).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(13).Width = 101F;
            numberCellType15.DecimalPlaces = 0;
            numberCellType15.MaximumValue = 999999999999D;
            numberCellType15.MinimumValue = -999999999999D;
            numberCellType15.NegativeRed = true;
            numberCellType15.Separator = ",";
            numberCellType15.ShowSeparator = true;
            this.SS1_Sheet1.Columns.Get(14).CellType = numberCellType15;
            this.SS1_Sheet1.Columns.Get(14).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(14).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS1_Sheet1.Columns.Get(14).Label = "결핵지원금";
            this.SS1_Sheet1.Columns.Get(14).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(14).Width = 101F;
            numberCellType16.DecimalPlaces = 0;
            numberCellType16.MaximumValue = 999999999999D;
            numberCellType16.MinimumValue = -999999999999D;
            numberCellType16.NegativeRed = true;
            numberCellType16.Separator = ",";
            numberCellType16.ShowSeparator = true;
            this.SS1_Sheet1.Columns.Get(15).CellType = numberCellType16;
            this.SS1_Sheet1.Columns.Get(15).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS1_Sheet1.Columns.Get(15).Label = "약제상한액";
            this.SS1_Sheet1.Columns.Get(15).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(15).Width = 101F;
            numberCellType17.DecimalPlaces = 0;
            numberCellType17.MaximumValue = 999999999999D;
            numberCellType17.MinimumValue = -999999999999D;
            numberCellType17.NegativeRed = true;
            numberCellType17.Separator = ",";
            numberCellType17.ShowSeparator = true;
            this.SS1_Sheet1.Columns.Get(16).CellType = numberCellType17;
            this.SS1_Sheet1.Columns.Get(16).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(16).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS1_Sheet1.Columns.Get(16).Label = "100/100미만";
            this.SS1_Sheet1.Columns.Get(16).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(16).Width = 101F;
            numberCellType18.DecimalPlaces = 0;
            numberCellType18.MaximumValue = 999999999999D;
            numberCellType18.MinimumValue = -999999999999D;
            numberCellType18.NegativeRed = true;
            numberCellType18.Separator = ",";
            numberCellType18.ShowSeparator = true;
            this.SS1_Sheet1.Columns.Get(17).CellType = numberCellType18;
            this.SS1_Sheet1.Columns.Get(17).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(17).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
            this.SS1_Sheet1.Columns.Get(17).Label = "국가재난지원(본인)";
            this.SS1_Sheet1.Columns.Get(17).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(17).Width = 122F;
            this.SS1_Sheet1.Columns.Get(18).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(18).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(18).Label = "청구번호";
            this.SS1_Sheet1.Columns.Get(18).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SS1_Sheet1.Columns.Get(18).Width = 78F;
            this.SS1_Sheet1.Columns.Get(19).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.Columns.Get(19).Label = "ROWID";
            this.SS1_Sheet1.Columns.Get(19).Width = 82F;
            this.SS1_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS1_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SS1_Sheet1.Rows.Get(0).Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.SS1_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SS1_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmMisuEdiView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 659);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panBottom);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmMisuEdiView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EDI접수증자료관리";
            this.Load += new System.EventHandler(this.frmMisuEdiView_Load);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.grbMir.ResumeLayout(false);
            this.grbWeek.ResumeLayout(false);
            this.grbJong.ResumeLayout(false);
            this.grbYYMM.ResumeLayout(false);
            this.grbIO.ResumeLayout(false);
            this.grbIO.PerformLayout();
            this.grbJob.ResumeLayout(false);
            this.grbJob.PerformLayout();
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panBottom.ResumeLayout(false);
            this.panBottom.PerformLayout();
            this.panMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panBottom;
        private System.Windows.Forms.Panel panMain;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.GroupBox grbYYMM;
        private System.Windows.Forms.ComboBox cboYYMM;
        private System.Windows.Forms.GroupBox grbIO;
        private System.Windows.Forms.RadioButton rdoIO3;
        private System.Windows.Forms.RadioButton rdoIO2;
        private System.Windows.Forms.RadioButton rdoIO1;
        private System.Windows.Forms.GroupBox grbJob;
        private System.Windows.Forms.RadioButton rdoJob2;
        private System.Windows.Forms.RadioButton rdoJob1;
        private System.Windows.Forms.GroupBox grbJong;
        private System.Windows.Forms.ComboBox cboJong;
        private System.Windows.Forms.GroupBox grbMir;
        private System.Windows.Forms.ComboBox cboB;
        private System.Windows.Forms.GroupBox grbWeek;
        private System.Windows.Forms.ComboBox cboA;
        private System.Windows.Forms.Label lblMsg;
    }
}