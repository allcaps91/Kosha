namespace ComSupLibB.SupLbEx
{
    partial class frmComSupLbExRSLT02
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
            this.btnClear = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ucPT_INFO = new ComSupLibB.UcSupComPtSearch();
            this.panel7 = new System.Windows.Forms.Panel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rdoPtInfo = new System.Windows.Forms.RadioButton();
            this.rdoSpecNo = new System.Windows.Forms.RadioButton();
            this.panel6 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoReUp = new System.Windows.Forms.RadioButton();
            this.rdoUpdate = new System.Windows.Forms.RadioButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboWS = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.lblDateTitle = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.circProgress = new DevComponents.DotNetBar.Controls.CircularProgress();
            this.ssMain = new FarPoint.Win.Spread.FpSpread();
            this.ssMain_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panTitle.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.Controls.Add(this.btnClear);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitle.Name = "panTitle";
            this.panTitle.Padding = new System.Windows.Forms.Padding(1);
            this.panTitle.Size = new System.Drawing.Size(1259, 48);
            this.panTitle.TabIndex = 13;
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClear.Location = new System.Drawing.Point(1114, 1);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(72, 46);
            this.btnClear.TabIndex = 41;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(1186, 1);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 46);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(6, 13);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(144, 16);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "검사결과수정내역";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.lblDateTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 48);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Size = new System.Drawing.Size(1259, 78);
            this.panel1.TabIndex = 88;
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(1030, 4);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(72, 70);
            this.btnSearch.TabIndex = 47;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(1102, 4);
            this.panel8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(5, 70);
            this.panel8.TabIndex = 46;
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(1107, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 70);
            this.btnSave.TabIndex = 45;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(1179, 4);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 70);
            this.panel2.TabIndex = 42;
            // 
            // btnPrint
            // 
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Location = new System.Drawing.Point(1184, 4);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(72, 70);
            this.btnPrint.TabIndex = 41;
            this.btnPrint.Text = "출력";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ucPT_INFO);
            this.groupBox4.Controls.Add(this.panel7);
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox4.Location = new System.Drawing.Point(511, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(487, 70);
            this.groupBox4.TabIndex = 35;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "환자조회";
            // 
            // ucPT_INFO
            // 
            this.ucPT_INFO.AutoSize = true;
            this.ucPT_INFO.BackColor = System.Drawing.Color.White;
            this.ucPT_INFO.Dock = System.Windows.Forms.DockStyle.Left;
            this.ucPT_INFO.Location = new System.Drawing.Point(131, 21);
            this.ucPT_INFO.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.ucPT_INFO.Name = "ucPT_INFO";
            this.ucPT_INFO.pPSMH_LPoint = new System.Drawing.Point(0, 0);
            this.ucPT_INFO.PSMH_TITLE_VISIBLE = true;
            this.ucPT_INFO.PSMH_TYPE = ComSupLibB.UcSupComPtSearch.enmType.SPECINFO;
            this.ucPT_INFO.PSMH_UNITKEY = false;
            this.ucPT_INFO.Size = new System.Drawing.Size(251, 46);
            this.ucPT_INFO.TabIndex = 41;
            // 
            // panel7
            // 
            this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel7.Location = new System.Drawing.Point(126, 21);
            this.panel7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(5, 46);
            this.panel7.TabIndex = 40;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rdoPtInfo);
            this.groupBox5.Controls.Add(this.rdoSpecNo);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox5.Location = new System.Drawing.Point(3, 21);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(1);
            this.groupBox5.Size = new System.Drawing.Size(123, 46);
            this.groupBox5.TabIndex = 39;
            this.groupBox5.TabStop = false;
            // 
            // rdoPtInfo
            // 
            this.rdoPtInfo.AutoSize = true;
            this.rdoPtInfo.Location = new System.Drawing.Point(63, 12);
            this.rdoPtInfo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoPtInfo.Name = "rdoPtInfo";
            this.rdoPtInfo.Size = new System.Drawing.Size(52, 21);
            this.rdoPtInfo.TabIndex = 1;
            this.rdoPtInfo.Text = "환자";
            this.rdoPtInfo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.rdoPtInfo.UseVisualStyleBackColor = true;
            // 
            // rdoSpecNo
            // 
            this.rdoSpecNo.AutoSize = true;
            this.rdoSpecNo.Checked = true;
            this.rdoSpecNo.Location = new System.Drawing.Point(4, 13);
            this.rdoSpecNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoSpecNo.Name = "rdoSpecNo";
            this.rdoSpecNo.Size = new System.Drawing.Size(52, 21);
            this.rdoSpecNo.TabIndex = 0;
            this.rdoSpecNo.TabStop = true;
            this.rdoSpecNo.Text = "검체";
            this.rdoSpecNo.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(506, 4);
            this.panel6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(5, 70);
            this.panel6.TabIndex = 34;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoReUp);
            this.groupBox1.Controls.Add(this.rdoUpdate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(376, 4);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(130, 70);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "작업구분";
            // 
            // rdoReUp
            // 
            this.rdoReUp.AutoSize = true;
            this.rdoReUp.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoReUp.Location = new System.Drawing.Point(55, 22);
            this.rdoReUp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoReUp.Name = "rdoReUp";
            this.rdoReUp.Size = new System.Drawing.Size(65, 44);
            this.rdoReUp.TabIndex = 24;
            this.rdoReUp.Text = "재전송";
            this.rdoReUp.UseVisualStyleBackColor = true;
            // 
            // rdoUpdate
            // 
            this.rdoUpdate.AutoSize = true;
            this.rdoUpdate.Checked = true;
            this.rdoUpdate.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoUpdate.Location = new System.Drawing.Point(3, 22);
            this.rdoUpdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoUpdate.Name = "rdoUpdate";
            this.rdoUpdate.Size = new System.Drawing.Size(52, 44);
            this.rdoUpdate.TabIndex = 23;
            this.rdoUpdate.TabStop = true;
            this.rdoUpdate.Text = "수정";
            this.rdoUpdate.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(371, 4);
            this.panel5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(5, 70);
            this.panel5.TabIndex = 31;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cboWS);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(257, 4);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Size = new System.Drawing.Size(114, 70);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "작업WS";
            // 
            // cboWS
            // 
            this.cboWS.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboWS.FormattingEnabled = true;
            this.cboWS.Location = new System.Drawing.Point(3, 22);
            this.cboWS.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboWS.Name = "cboWS";
            this.cboWS.Size = new System.Drawing.Size(100, 25);
            this.cboWS.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(252, 4);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(5, 70);
            this.panel4.TabIndex = 29;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dtpTDate);
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Controls.Add(this.dtpFDate);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(3, 4);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox2.Size = new System.Drawing.Size(249, 70);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "작업기간";
            // 
            // dtpTDate
            // 
            this.dtpTDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(119, 22);
            this.dtpTDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(106, 25);
            this.dtpTDate.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(103, 22);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Size = new System.Drawing.Size(16, 44);
            this.panel3.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "~";
            // 
            // dtpFDate
            // 
            this.dtpFDate.Dock = System.Windows.Forms.DockStyle.Left;
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(3, 22);
            this.dtpFDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(100, 25);
            this.dtpFDate.TabIndex = 1;
            // 
            // lblDateTitle
            // 
            this.lblDateTitle.AutoSize = true;
            this.lblDateTitle.Location = new System.Drawing.Point(13, 30);
            this.lblDateTitle.Name = "lblDateTitle";
            this.lblDateTitle.Size = new System.Drawing.Size(60, 17);
            this.lblDateTitle.TabIndex = 0;
            this.lblDateTitle.Text = "결과일자";
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 126);
            this.panTitleSub0.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(1259, 38);
            this.panTitleSub0.TabIndex = 89;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 8);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(158, 12);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "검사결과 수정내역 리스트";
            // 
            // panMain
            // 
            this.panMain.Controls.Add(this.circProgress);
            this.panMain.Controls.Add(this.ssMain);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 164);
            this.panMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panMain.Name = "panMain";
            this.panMain.Padding = new System.Windows.Forms.Padding(1);
            this.panMain.Size = new System.Drawing.Size(1259, 843);
            this.panMain.TabIndex = 90;
            // 
            // circProgress
            // 
            this.circProgress.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.circProgress.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.circProgress.Location = new System.Drawing.Point(536, 119);
            this.circProgress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.circProgress.Name = "circProgress";
            this.circProgress.Size = new System.Drawing.Size(259, 363);
            this.circProgress.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeXP;
            this.circProgress.TabIndex = 3;
            this.circProgress.Visible = false;
            // 
            // ssMain
            // 
            this.ssMain.AccessibleDescription = "ssMain, Sheet1, Row 0, Column 0, ";
            this.ssMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ssMain.FocusRenderer = enhancedFocusIndicatorRenderer1;
            this.ssMain.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssMain.HorizontalScrollBar.Name = "";
            this.ssMain.HorizontalScrollBar.Renderer = enhancedScrollBarRenderer1;
            this.ssMain.HorizontalScrollBar.TabIndex = 21;
            this.ssMain.Location = new System.Drawing.Point(1, 1);
            this.ssMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ssMain.Name = "ssMain";
            this.ssMain.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.ssMain_Sheet1});
            this.ssMain.Size = new System.Drawing.Size(1257, 841);
            this.ssMain.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;
            this.ssMain.TabIndex = 0;
            this.ssMain.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.ssMain.VerticalScrollBar.Name = "";
            this.ssMain.VerticalScrollBar.Renderer = enhancedScrollBarRenderer2;
            this.ssMain.VerticalScrollBar.TabIndex = 22;
            // 
            // ssMain_Sheet1
            // 
            this.ssMain_Sheet1.Reset();
            this.ssMain_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.ssMain_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.ssMain_Sheet1.ColumnCount = 6;
            this.ssMain_Sheet1.RowCount = 2;
            this.ssMain_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterEnhanced";
            this.ssMain_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerEnhanced";
            this.ssMain_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.ColumnHeader.AutoText = FarPoint.Win.Spread.HeaderAutoText.Numbers;
            this.ssMain_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderEnhanced";
            this.ssMain_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarEnhanced";
            this.ssMain_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.FilterBarHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(236)))), ((int)(((byte)(247)))));
            this.ssMain_Sheet1.FilterBarHeaderStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ssMain_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.ssMain_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderEnhanced";
            this.ssMain_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.ssMain_Sheet1.FilterBarHeaderStyle.VisualStyles = FarPoint.Win.VisualStyles.Auto;
            this.ssMain_Sheet1.GrayAreaBackColor = System.Drawing.Color.White;
            this.ssMain_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.ssMain_Sheet1.RowHeader.Columns.Get(0).Width = 40F;
            this.ssMain_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderEnhanced";
            this.ssMain_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.ssMain_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.ssMain_Sheet1.SheetCornerStyle.Parent = "CornerEnhanced";
            this.ssMain_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.ssMain_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // frmComSupLbExRSLT02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1259, 1007);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmComSupLbExRSLT02";
            this.Text = "frmComSupLbExSpecInfo";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ssMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ssMain_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblDateTitle;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panMain;
        private FarPoint.Win.Spread.FpSpread ssMain;
        private FarPoint.Win.Spread.SheetView ssMain_Sheet1;
        private DevComponents.DotNetBar.Controls.CircularProgress circProgress;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cboWS;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoReUp;
        private System.Windows.Forms.RadioButton rdoUpdate;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton rdoPtInfo;
        private System.Windows.Forms.RadioButton rdoSpecNo;
        private UcSupComPtSearch ucPT_INFO;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button btnSave;
    }
}