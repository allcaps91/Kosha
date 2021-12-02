namespace ComPmpaLibB
{
    partial class frmPmpaSpecInq
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
            FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer enhancedFocusIndicatorRenderer1 = new FarPoint.Win.Spread.EnhancedFocusIndicatorRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer1 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            FarPoint.Win.Spread.EnhancedScrollBarRenderer enhancedScrollBarRenderer2 = new FarPoint.Win.Spread.EnhancedScrollBarRenderer();
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panJob = new System.Windows.Forms.Panel();
            this.panJob04 = new System.Windows.Forms.Panel();
            this.txt_SNAME = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panSub01 = new System.Windows.Forms.Panel();
            this.txt_PANO = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panJob05 = new System.Windows.Forms.Panel();
            this.dtp_TDATE = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtp_FDATE = new System.Windows.Forms.DateTimePicker();
            this.panSub02 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panJob06 = new System.Windows.Forms.Panel();
            this.rdoIO3 = new System.Windows.Forms.RadioButton();
            this.rdoIO2 = new System.Windows.Forms.RadioButton();
            this.rdoIO1 = new System.Windows.Forms.RadioButton();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.panJob03 = new System.Windows.Forms.Panel();
            this.rdo_EXP = new System.Windows.Forms.RadioButton();
            this.rdo_NOTEXP = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.grpBox = new System.Windows.Forms.GroupBox();
            this.cboWS = new System.Windows.Forms.ComboBox();
            this.panMain = new System.Windows.Forms.Panel();
            this.barProgress = new DevComponents.DotNetBar.Controls.CircularProgress();
            this.ss_EXAM_SPECMST = new FarPoint.Win.Spread.FpSpread();
            this.ss_EXAM_SPECMST_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panJob.SuspendLayout();
            this.panJob04.SuspendLayout();
            this.panSub01.SuspendLayout();
            this.panJob05.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panSub02.SuspendLayout();
            this.panJob06.SuspendLayout();
            this.panJob03.SuspendLayout();
            this.grpBox.SuspendLayout();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ss_EXAM_SPECMST)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss_EXAM_SPECMST_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(2);
            this.panTitle.Name = "panTitle";
            this.panTitle.Padding = new System.Windows.Forms.Padding(1);
            this.panTitle.Size = new System.Drawing.Size(1184, 41);
            this.panTitle.TabIndex = 21;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1068, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(111, 35);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(4, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(112, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "검체번호 현황";
            // 
            // panJob
            // 
            this.panJob.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panJob.Controls.Add(this.panJob04);
            this.panJob.Controls.Add(this.panJob05);
            this.panJob.Controls.Add(this.panJob06);
            this.panJob.Controls.Add(this.btnSearch);
            this.panJob.Controls.Add(this.btnClear);
            this.panJob.Controls.Add(this.panJob03);
            this.panJob.Controls.Add(this.grpBox);
            this.panJob.Dock = System.Windows.Forms.DockStyle.Top;
            this.panJob.Location = new System.Drawing.Point(0, 41);
            this.panJob.Name = "panJob";
            this.panJob.Padding = new System.Windows.Forms.Padding(3);
            this.panJob.Size = new System.Drawing.Size(1184, 58);
            this.panJob.TabIndex = 22;
            // 
            // panJob04
            // 
            this.panJob04.Controls.Add(this.txt_SNAME);
            this.panJob04.Controls.Add(this.label3);
            this.panJob04.Controls.Add(this.panSub01);
            this.panJob04.Dock = System.Windows.Forms.DockStyle.Left;
            this.panJob04.Location = new System.Drawing.Point(711, 3);
            this.panJob04.Name = "panJob04";
            this.panJob04.Size = new System.Drawing.Size(180, 50);
            this.panJob04.TabIndex = 9;
            // 
            // txt_SNAME
            // 
            this.txt_SNAME.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_SNAME.Location = new System.Drawing.Point(74, 25);
            this.txt_SNAME.Name = "txt_SNAME";
            this.txt_SNAME.Size = new System.Drawing.Size(106, 25);
            this.txt_SNAME.TabIndex = 8;
            this.txt_SNAME.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(0, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "환자성명";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panSub01
            // 
            this.panSub01.Controls.Add(this.txt_PANO);
            this.panSub01.Controls.Add(this.label2);
            this.panSub01.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub01.Location = new System.Drawing.Point(0, 0);
            this.panSub01.Name = "panSub01";
            this.panSub01.Size = new System.Drawing.Size(180, 25);
            this.panSub01.TabIndex = 0;
            // 
            // txt_PANO
            // 
            this.txt_PANO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_PANO.Location = new System.Drawing.Point(74, 0);
            this.txt_PANO.Name = "txt_PANO";
            this.txt_PANO.Size = new System.Drawing.Size(106, 25);
            this.txt_PANO.TabIndex = 7;
            this.txt_PANO.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "등록번호";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panJob05
            // 
            this.panJob05.Controls.Add(this.dtp_TDATE);
            this.panJob05.Controls.Add(this.panel1);
            this.panJob05.Controls.Add(this.panSub02);
            this.panJob05.Dock = System.Windows.Forms.DockStyle.Left;
            this.panJob05.Location = new System.Drawing.Point(539, 3);
            this.panJob05.Name = "panJob05";
            this.panJob05.Size = new System.Drawing.Size(172, 50);
            this.panJob05.TabIndex = 8;
            // 
            // dtp_TDATE
            // 
            this.dtp_TDATE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtp_TDATE.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_TDATE.Location = new System.Drawing.Point(63, 25);
            this.dtp_TDATE.Name = "dtp_TDATE";
            this.dtp_TDATE.Size = new System.Drawing.Size(109, 25);
            this.dtp_TDATE.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtp_FDATE);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(63, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(109, 25);
            this.panel1.TabIndex = 1;
            // 
            // dtp_FDATE
            // 
            this.dtp_FDATE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtp_FDATE.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_FDATE.Location = new System.Drawing.Point(0, 0);
            this.dtp_FDATE.Name = "dtp_FDATE";
            this.dtp_FDATE.Size = new System.Drawing.Size(109, 25);
            this.dtp_FDATE.TabIndex = 0;
            // 
            // panSub02
            // 
            this.panSub02.Controls.Add(this.label4);
            this.panSub02.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub02.Location = new System.Drawing.Point(0, 0);
            this.panSub02.Name = "panSub02";
            this.panSub02.Padding = new System.Windows.Forms.Padding(1);
            this.panSub02.Size = new System.Drawing.Size(63, 50);
            this.panSub02.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(1, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 48);
            this.label4.TabIndex = 4;
            this.label4.Text = "체혈일자";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panJob06
            // 
            this.panJob06.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panJob06.Controls.Add(this.rdoIO3);
            this.panJob06.Controls.Add(this.rdoIO2);
            this.panJob06.Controls.Add(this.rdoIO1);
            this.panJob06.Dock = System.Windows.Forms.DockStyle.Left;
            this.panJob06.Location = new System.Drawing.Point(374, 3);
            this.panJob06.Name = "panJob06";
            this.panJob06.Padding = new System.Windows.Forms.Padding(3);
            this.panJob06.Size = new System.Drawing.Size(165, 50);
            this.panJob06.TabIndex = 5;
            // 
            // rdoIO3
            // 
            this.rdoIO3.AutoSize = true;
            this.rdoIO3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoIO3.Location = new System.Drawing.Point(107, 3);
            this.rdoIO3.Name = "rdoIO3";
            this.rdoIO3.Size = new System.Drawing.Size(53, 42);
            this.rdoIO3.TabIndex = 5;
            this.rdoIO3.Text = "입원";
            this.rdoIO3.UseVisualStyleBackColor = true;
            // 
            // rdoIO2
            // 
            this.rdoIO2.AutoSize = true;
            this.rdoIO2.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoIO2.Location = new System.Drawing.Point(55, 3);
            this.rdoIO2.Name = "rdoIO2";
            this.rdoIO2.Size = new System.Drawing.Size(52, 42);
            this.rdoIO2.TabIndex = 4;
            this.rdoIO2.Text = "외래";
            this.rdoIO2.UseVisualStyleBackColor = true;
            // 
            // rdoIO1
            // 
            this.rdoIO1.AutoSize = true;
            this.rdoIO1.Checked = true;
            this.rdoIO1.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoIO1.Location = new System.Drawing.Point(3, 3);
            this.rdoIO1.Name = "rdoIO1";
            this.rdoIO1.Size = new System.Drawing.Size(52, 42);
            this.rdoIO1.TabIndex = 3;
            this.rdoIO1.TabStop = true;
            this.rdoIO1.Text = "전체";
            this.rdoIO1.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(1061, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(59, 50);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClear.Location = new System.Drawing.Point(1120, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(59, 50);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "취소";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // panJob03
            // 
            this.panJob03.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panJob03.Controls.Add(this.rdo_EXP);
            this.panJob03.Controls.Add(this.rdo_NOTEXP);
            this.panJob03.Controls.Add(this.label1);
            this.panJob03.Dock = System.Windows.Forms.DockStyle.Left;
            this.panJob03.Location = new System.Drawing.Point(203, 3);
            this.panJob03.Name = "panJob03";
            this.panJob03.Padding = new System.Windows.Forms.Padding(3);
            this.panJob03.Size = new System.Drawing.Size(171, 50);
            this.panJob03.TabIndex = 2;
            // 
            // rdo_EXP
            // 
            this.rdo_EXP.AutoSize = true;
            this.rdo_EXP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdo_EXP.Location = new System.Drawing.Point(112, 3);
            this.rdo_EXP.Name = "rdo_EXP";
            this.rdo_EXP.Size = new System.Drawing.Size(54, 42);
            this.rdo_EXP.TabIndex = 3;
            this.rdo_EXP.Text = "않함";
            this.rdo_EXP.UseVisualStyleBackColor = true;
            // 
            // rdo_NOTEXP
            // 
            this.rdo_NOTEXP.AutoSize = true;
            this.rdo_NOTEXP.Checked = true;
            this.rdo_NOTEXP.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdo_NOTEXP.Location = new System.Drawing.Point(60, 3);
            this.rdo_NOTEXP.Name = "rdo_NOTEXP";
            this.rdo_NOTEXP.Size = new System.Drawing.Size(52, 42);
            this.rdo_NOTEXP.TabIndex = 2;
            this.rdo_NOTEXP.TabStop = true;
            this.rdo_NOTEXP.Text = "표시";
            this.rdo_NOTEXP.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 42);
            this.label1.TabIndex = 0;
            this.label1.Text = "미접수,인쇄";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpBox
            // 
            this.grpBox.Controls.Add(this.cboWS);
            this.grpBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpBox.Location = new System.Drawing.Point(3, 3);
            this.grpBox.Name = "grpBox";
            this.grpBox.Size = new System.Drawing.Size(200, 50);
            this.grpBox.TabIndex = 1;
            this.grpBox.TabStop = false;
            this.grpBox.Text = "WS 구분";
            // 
            // cboWS
            // 
            this.cboWS.BackColor = System.Drawing.Color.White;
            this.cboWS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboWS.FormattingEnabled = true;
            this.cboWS.Location = new System.Drawing.Point(3, 21);
            this.cboWS.Name = "cboWS";
            this.cboWS.Size = new System.Drawing.Size(194, 25);
            this.cboWS.TabIndex = 1;
            // 
            // panMain
            // 
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMain.Controls.Add(this.barProgress);
            this.panMain.Controls.Add(this.ss_EXAM_SPECMST);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 99);
            this.panMain.Name = "panMain";
            this.panMain.Padding = new System.Windows.Forms.Padding(3);
            this.panMain.Size = new System.Drawing.Size(1184, 662);
            this.panMain.TabIndex = 23;
            // 
            // barProgress
            // 
            this.barProgress.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.barProgress.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.barProgress.Location = new System.Drawing.Point(457, 124);
            this.barProgress.Name = "barProgress";
            this.barProgress.Size = new System.Drawing.Size(269, 249);
            this.barProgress.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.barProgress.TabIndex = 141;
            // 
            // ss_EXAM_SPECMST
            // 
            this.ss_EXAM_SPECMST.AccessibleDescription = "";
            this.ss_EXAM_SPECMST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ss_EXAM_SPECMST.FocusRenderer = enhancedFocusIndicatorRenderer1;
            this.ss_EXAM_SPECMST.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ss_EXAM_SPECMST.HorizontalScrollBar.Name = "";
            this.ss_EXAM_SPECMST.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.ss_EXAM_SPECMST.HorizontalScrollBar.TabIndex = 3;
            this.ss_EXAM_SPECMST.Location = new System.Drawing.Point(3, 3);
            this.ss_EXAM_SPECMST.Name = "ss_EXAM_SPECMST";
            this.ss_EXAM_SPECMST.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ss_EXAM_SPECMST_Sheet1});
            this.ss_EXAM_SPECMST.Size = new System.Drawing.Size(1176, 654);
            this.ss_EXAM_SPECMST.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;
            this.ss_EXAM_SPECMST.TabIndex = 140;
            this.ss_EXAM_SPECMST.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ss_EXAM_SPECMST.VerticalScrollBar.Name = "";
            this.ss_EXAM_SPECMST.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.ss_EXAM_SPECMST.VerticalScrollBar.TabIndex = 4;
            // 
            // ss_EXAM_SPECMST_Sheet1
            // 
            this.ss_EXAM_SPECMST_Sheet1.Reset();
            this.ss_EXAM_SPECMST_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ss_EXAM_SPECMST_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ss_EXAM_SPECMST_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_EXAM_SPECMST_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_EXAM_SPECMST_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.ss_EXAM_SPECMST_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_EXAM_SPECMST_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_EXAM_SPECMST_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_EXAM_SPECMST_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.ss_EXAM_SPECMST_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_EXAM_SPECMST_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_EXAM_SPECMST_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_EXAM_SPECMST_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.ss_EXAM_SPECMST_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_EXAM_SPECMST_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_EXAM_SPECMST_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_EXAM_SPECMST_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.ss_EXAM_SPECMST_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_EXAM_SPECMST_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_EXAM_SPECMST_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_EXAM_SPECMST_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarEnhanced";
            this.ss_EXAM_SPECMST_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_EXAM_SPECMST_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_EXAM_SPECMST_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_EXAM_SPECMST_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderEnhanced";
            this.ss_EXAM_SPECMST_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_EXAM_SPECMST_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ss_EXAM_SPECMST_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ss_EXAM_SPECMST_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_EXAM_SPECMST_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_EXAM_SPECMST_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.ss_EXAM_SPECMST_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_EXAM_SPECMST_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ss_EXAM_SPECMST_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ss_EXAM_SPECMST_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.ss_EXAM_SPECMST_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ss_EXAM_SPECMST_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmPmpaSpecInq
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1184, 761);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panJob);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmPmpaSpecInq";
            this.Text = "검체번호 현황";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panJob.ResumeLayout(false);
            this.panJob04.ResumeLayout(false);
            this.panJob04.PerformLayout();
            this.panSub01.ResumeLayout(false);
            this.panSub01.PerformLayout();
            this.panJob05.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panSub02.ResumeLayout(false);
            this.panJob06.ResumeLayout(false);
            this.panJob06.PerformLayout();
            this.panJob03.ResumeLayout(false);
            this.panJob03.PerformLayout();
            this.grpBox.ResumeLayout(false);
            this.panMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ss_EXAM_SPECMST)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ss_EXAM_SPECMST_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panJob;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Panel panJob03;
        private System.Windows.Forms.RadioButton rdo_EXP;
        private System.Windows.Forms.RadioButton rdo_NOTEXP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpBox;
        private System.Windows.Forms.ComboBox cboWS;
        private FarPoint.Win.Spread.FpSpread ss_EXAM_SPECMST;
        private FarPoint.Win.Spread.SheetView ss_EXAM_SPECMST_Sheet1;
        private DevComponents.DotNetBar.Controls.CircularProgress barProgress;
        private System.Windows.Forms.Panel panJob04;
        private System.Windows.Forms.TextBox txt_SNAME;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panSub01;
        private System.Windows.Forms.TextBox txt_PANO;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panJob05;
        private System.Windows.Forms.DateTimePicker dtp_TDATE;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtp_FDATE;
        private System.Windows.Forms.Panel panSub02;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panJob06;
        private System.Windows.Forms.RadioButton rdoIO3;
        private System.Windows.Forms.RadioButton rdoIO2;
        private System.Windows.Forms.RadioButton rdoIO1;
    }
}