namespace ComPmpaLibB
{
    partial class frmPmpaGyeID
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
            this.panTitle = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panList = new System.Windows.Forms.Panel();
            this.panListMain = new System.Windows.Forms.Panel();
            this.lstbox = new DevComponents.DotNetBar.ListBoxAdv();
            this.listBoxItem1 = new DevComponents.DotNetBar.ListBoxItem();
            this.panListSub = new System.Windows.Forms.Panel();
            this.cboLGel = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panJiByung = new System.Windows.Forms.Panel();
            this.panJiByungMain = new System.Windows.Forms.Panel();
            this.panJob = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblLen = new System.Windows.Forms.Label();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.lblBi = new System.Windows.Forms.Label();
            this.cboBi3 = new System.Windows.Forms.ComboBox();
            this.cboBi2 = new System.Windows.Forms.ComboBox();
            this.cboBi1 = new System.Windows.Forms.ComboBox();
            this.lblDeptName = new System.Windows.Forms.Label();
            this.cboDept3 = new System.Windows.Forms.ComboBox();
            this.cboDept2 = new System.Windows.Forms.ComboBox();
            this.cboDept1 = new System.Windows.Forms.ComboBox();
            this.cboGel = new System.Windows.Forms.ComboBox();
            this.lblJumin = new System.Windows.Forms.Label();
            this.lblSName = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panJiByungSub = new System.Windows.Forms.Panel();
            this.txtPano = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panTitleSub1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panList.SuspendLayout();
            this.panListMain.SuspendLayout();
            this.panListSub.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panJiByung.SuspendLayout();
            this.panJiByungMain.SuspendLayout();
            this.panJob.SuspendLayout();
            this.panJiByungSub.SuspendLayout();
            this.panTitleSub1.SuspendLayout();
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
            this.panTitle.Name = "panTitle";
            this.panTitle.Padding = new System.Windows.Forms.Padding(1);
            this.panTitle.Size = new System.Drawing.Size(645, 36);
            this.panTitle.TabIndex = 22;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(529, 1);
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
            this.lblTitle.Size = new System.Drawing.Size(188, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "계약처 미수 발생자 정보";
            // 
            // panList
            // 
            this.panList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panList.Controls.Add(this.panListMain);
            this.panList.Controls.Add(this.panListSub);
            this.panList.Controls.Add(this.panTitleSub0);
            this.panList.Dock = System.Windows.Forms.DockStyle.Left;
            this.panList.Location = new System.Drawing.Point(0, 36);
            this.panList.Name = "panList";
            this.panList.Padding = new System.Windows.Forms.Padding(3);
            this.panList.Size = new System.Drawing.Size(268, 448);
            this.panList.TabIndex = 23;
            // 
            // panListMain
            // 
            this.panListMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panListMain.Controls.Add(this.lstbox);
            this.panListMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panListMain.Location = new System.Drawing.Point(3, 81);
            this.panListMain.Name = "panListMain";
            this.panListMain.Size = new System.Drawing.Size(260, 362);
            this.panListMain.TabIndex = 22;
            // 
            // lstbox
            // 
            this.lstbox.AutoScroll = true;
            // 
            // 
            // 
            this.lstbox.BackgroundStyle.Class = "ListBoxAdv";
            this.lstbox.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lstbox.ContainerControlProcessDialogKey = true;
            this.lstbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstbox.DragDropSupport = true;
            this.lstbox.Items.Add(this.listBoxItem1);
            this.lstbox.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.lstbox.Location = new System.Drawing.Point(0, 0);
            this.lstbox.Name = "lstbox";
            this.lstbox.Size = new System.Drawing.Size(258, 360);
            this.lstbox.TabIndex = 0;
            this.lstbox.Text = "listBoxAdv1";
            this.lstbox.ItemDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstbox_ItemDoubleClick);
            // 
            // listBoxItem1
            // 
            this.listBoxItem1.IsSelected = true;
            this.listBoxItem1.Name = "listBoxItem1";
            this.listBoxItem1.Text = "81000004  김수한무거  H911  MD  51";
            // 
            // panListSub
            // 
            this.panListSub.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panListSub.Controls.Add(this.cboLGel);
            this.panListSub.Controls.Add(this.btnSearch);
            this.panListSub.Dock = System.Windows.Forms.DockStyle.Top;
            this.panListSub.Location = new System.Drawing.Point(3, 41);
            this.panListSub.Name = "panListSub";
            this.panListSub.Size = new System.Drawing.Size(260, 40);
            this.panListSub.TabIndex = 21;
            // 
            // cboLGel
            // 
            this.cboLGel.FormattingEnabled = true;
            this.cboLGel.Location = new System.Drawing.Point(7, 5);
            this.cboLGel.Name = "cboLGel";
            this.cboLGel.Size = new System.Drawing.Size(179, 25);
            this.cboLGel.TabIndex = 7;
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Location = new System.Drawing.Point(192, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(66, 38);
            this.btnSearch.TabIndex = 6;
            this.btnSearch.Text = "찾기";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(3, 3);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Padding = new System.Windows.Forms.Padding(1);
            this.panTitleSub0.Size = new System.Drawing.Size(260, 38);
            this.panTitleSub0.TabIndex = 20;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 10);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(63, 13);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "환자명단";
            // 
            // panJiByung
            // 
            this.panJiByung.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panJiByung.Controls.Add(this.panJiByungMain);
            this.panJiByung.Controls.Add(this.panJiByungSub);
            this.panJiByung.Controls.Add(this.panTitleSub1);
            this.panJiByung.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panJiByung.Location = new System.Drawing.Point(268, 36);
            this.panJiByung.Name = "panJiByung";
            this.panJiByung.Padding = new System.Windows.Forms.Padding(3);
            this.panJiByung.Size = new System.Drawing.Size(377, 448);
            this.panJiByung.TabIndex = 24;
            // 
            // panJiByungMain
            // 
            this.panJiByungMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panJiByungMain.Controls.Add(this.panJob);
            this.panJiByungMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panJiByungMain.Location = new System.Drawing.Point(3, 81);
            this.panJiByungMain.Name = "panJiByungMain";
            this.panJiByungMain.Size = new System.Drawing.Size(369, 362);
            this.panJiByungMain.TabIndex = 23;
            // 
            // panJob
            // 
            this.panJob.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panJob.Controls.Add(this.label2);
            this.panJob.Controls.Add(this.lblLen);
            this.panJob.Controls.Add(this.dtpTDate);
            this.panJob.Controls.Add(this.dtpFDate);
            this.panJob.Controls.Add(this.lblBi);
            this.panJob.Controls.Add(this.cboBi3);
            this.panJob.Controls.Add(this.cboBi2);
            this.panJob.Controls.Add(this.cboBi1);
            this.panJob.Controls.Add(this.lblDeptName);
            this.panJob.Controls.Add(this.cboDept3);
            this.panJob.Controls.Add(this.cboDept2);
            this.panJob.Controls.Add(this.cboDept1);
            this.panJob.Controls.Add(this.cboGel);
            this.panJob.Controls.Add(this.lblJumin);
            this.panJob.Controls.Add(this.lblSName);
            this.panJob.Controls.Add(this.txtRemark);
            this.panJob.Controls.Add(this.label8);
            this.panJob.Controls.Add(this.label7);
            this.panJob.Controls.Add(this.label6);
            this.panJob.Controls.Add(this.label5);
            this.panJob.Controls.Add(this.label4);
            this.panJob.Controls.Add(this.label3);
            this.panJob.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panJob.Location = new System.Drawing.Point(0, 0);
            this.panJob.Name = "panJob";
            this.panJob.Size = new System.Drawing.Size(367, 360);
            this.panJob.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 17);
            this.label2.TabIndex = 24;
            this.label2.Text = "환자성명";
            // 
            // lblLen
            // 
            this.lblLen.AutoSize = true;
            this.lblLen.Location = new System.Drawing.Point(304, 242);
            this.lblLen.Name = "lblLen";
            this.lblLen.Size = new System.Drawing.Size(55, 17);
            this.lblLen.TabIndex = 23;
            this.lblLen.Text = "500/500";
            // 
            // dtpTDate
            // 
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(85, 220);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(147, 25);
            this.dtpTDate.TabIndex = 22;
            // 
            // dtpFDate
            // 
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(85, 185);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(147, 25);
            this.dtpFDate.TabIndex = 21;
            // 
            // lblBi
            // 
            this.lblBi.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lblBi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBi.Location = new System.Drawing.Point(238, 152);
            this.lblBi.Name = "lblBi";
            this.lblBi.Size = new System.Drawing.Size(106, 25);
            this.lblBi.TabIndex = 20;
            this.lblBi.Text = "자동차보험";
            this.lblBi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboBi3
            // 
            this.cboBi3.FormattingEnabled = true;
            this.cboBi3.Location = new System.Drawing.Point(187, 152);
            this.cboBi3.Name = "cboBi3";
            this.cboBi3.Size = new System.Drawing.Size(45, 25);
            this.cboBi3.TabIndex = 19;
            this.cboBi3.Text = "52";
            this.cboBi3.SelectedIndexChanged += new System.EventHandler(this.cboBi3_SelectedIndexChanged);
            // 
            // cboBi2
            // 
            this.cboBi2.FormattingEnabled = true;
            this.cboBi2.Location = new System.Drawing.Point(136, 152);
            this.cboBi2.Name = "cboBi2";
            this.cboBi2.Size = new System.Drawing.Size(45, 25);
            this.cboBi2.TabIndex = 18;
            this.cboBi2.Text = "12";
            this.cboBi2.SelectedIndexChanged += new System.EventHandler(this.cboBi2_SelectedIndexChanged);
            this.cboBi2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboBi2_KeyPress);
            // 
            // cboBi1
            // 
            this.cboBi1.FormattingEnabled = true;
            this.cboBi1.Location = new System.Drawing.Point(85, 152);
            this.cboBi1.Name = "cboBi1";
            this.cboBi1.Size = new System.Drawing.Size(45, 25);
            this.cboBi1.TabIndex = 17;
            this.cboBi1.Text = "11";
            this.cboBi1.SelectedIndexChanged += new System.EventHandler(this.cboBi1_SelectedIndexChanged);
            this.cboBi1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboBi1_KeyPress);
            // 
            // lblDeptName
            // 
            this.lblDeptName.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lblDeptName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDeptName.Location = new System.Drawing.Point(238, 118);
            this.lblDeptName.Name = "lblDeptName";
            this.lblDeptName.Size = new System.Drawing.Size(106, 25);
            this.lblDeptName.TabIndex = 16;
            this.lblDeptName.Text = "정신건강의학과";
            this.lblDeptName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboDept3
            // 
            this.cboDept3.FormattingEnabled = true;
            this.cboDept3.Location = new System.Drawing.Point(187, 118);
            this.cboDept3.Name = "cboDept3";
            this.cboDept3.Size = new System.Drawing.Size(45, 25);
            this.cboDept3.TabIndex = 15;
            this.cboDept3.Text = "MD";
            this.cboDept3.SelectedIndexChanged += new System.EventHandler(this.cboDept3_SelectedIndexChanged);
            this.cboDept3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboDept3_KeyPress);
            // 
            // cboDept2
            // 
            this.cboDept2.FormattingEnabled = true;
            this.cboDept2.Location = new System.Drawing.Point(136, 118);
            this.cboDept2.Name = "cboDept2";
            this.cboDept2.Size = new System.Drawing.Size(45, 25);
            this.cboDept2.TabIndex = 14;
            this.cboDept2.Text = "MD";
            this.cboDept2.SelectedIndexChanged += new System.EventHandler(this.cboDept2_SelectedIndexChanged);
            this.cboDept2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboDept2_KeyPress);
            // 
            // cboDept1
            // 
            this.cboDept1.FormattingEnabled = true;
            this.cboDept1.Location = new System.Drawing.Point(85, 118);
            this.cboDept1.Name = "cboDept1";
            this.cboDept1.Size = new System.Drawing.Size(45, 25);
            this.cboDept1.TabIndex = 13;
            this.cboDept1.Text = "MD";
            this.cboDept1.SelectedIndexChanged += new System.EventHandler(this.cboDept1_SelectedIndexChanged);
            this.cboDept1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboDept1_KeyPress);
            // 
            // cboGel
            // 
            this.cboGel.FormattingEnabled = true;
            this.cboGel.Location = new System.Drawing.Point(85, 84);
            this.cboGel.Name = "cboGel";
            this.cboGel.Size = new System.Drawing.Size(147, 25);
            this.cboGel.TabIndex = 12;
            this.cboGel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cboGel_KeyPress);
            // 
            // lblJumin
            // 
            this.lblJumin.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lblJumin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblJumin.Location = new System.Drawing.Point(85, 49);
            this.lblJumin.Name = "lblJumin";
            this.lblJumin.Size = new System.Drawing.Size(147, 25);
            this.lblJumin.TabIndex = 11;
            this.lblJumin.Text = "123456-7890123";
            this.lblJumin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSName
            // 
            this.lblSName.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.lblSName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSName.Location = new System.Drawing.Point(85, 16);
            this.lblSName.Name = "lblSName";
            this.lblSName.Size = new System.Drawing.Size(147, 25);
            this.lblSName.TabIndex = 10;
            this.lblSName.Text = "김수한무거";
            this.lblSName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtRemark
            // 
            this.txtRemark.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtRemark.Location = new System.Drawing.Point(0, 262);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRemark.Size = new System.Drawing.Size(365, 96);
            this.txtRemark.TabIndex = 7;
            this.txtRemark.TextChanged += new System.EventHandler(this.txtRemark_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 223);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 17);
            this.label8.TabIndex = 6;
            this.label8.Text = "종결일자";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 189);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 17);
            this.label7.TabIndex = 5;
            this.label7.Text = "시작일자";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 17);
            this.label6.TabIndex = 4;
            this.label6.Text = "환자종류";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "진 료 과";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "계 약 처";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "주민등록";
            // 
            // panJiByungSub
            // 
            this.panJiByungSub.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panJiByungSub.Controls.Add(this.txtPano);
            this.panJiByungSub.Controls.Add(this.label9);
            this.panJiByungSub.Controls.Add(this.btnSave);
            this.panJiByungSub.Controls.Add(this.btnDelete);
            this.panJiByungSub.Controls.Add(this.btnCancel);
            this.panJiByungSub.Dock = System.Windows.Forms.DockStyle.Top;
            this.panJiByungSub.Location = new System.Drawing.Point(3, 41);
            this.panJiByungSub.Name = "panJiByungSub";
            this.panJiByungSub.Size = new System.Drawing.Size(369, 40);
            this.panJiByungSub.TabIndex = 22;
            // 
            // txtPano
            // 
            this.txtPano.Location = new System.Drawing.Point(75, 6);
            this.txtPano.Name = "txtPano";
            this.txtPano.Size = new System.Drawing.Size(96, 25);
            this.txtPano.TabIndex = 11;
            this.txtPano.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPano.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPano_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(60, 17);
            this.label9.TabIndex = 10;
            this.label9.Text = "등록번호";
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(187, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 38);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "저장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDelete.Location = new System.Drawing.Point(247, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(60, 38);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "삭제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(307, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 38);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "취소";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panTitleSub1
            // 
            this.panTitleSub1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub1.Controls.Add(this.label1);
            this.panTitleSub1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub1.Location = new System.Drawing.Point(3, 3);
            this.panTitleSub1.Name = "panTitleSub1";
            this.panTitleSub1.Padding = new System.Windows.Forms.Padding(1);
            this.panTitleSub1.Size = new System.Drawing.Size(369, 38);
            this.panTitleSub1.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "미수발생 구분 등록";
            // 
            // frmPmpaGyeID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 484);
            this.Controls.Add(this.panJiByung);
            this.Controls.Add(this.panList);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmPmpaGyeID";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmPmpaGyeID_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panList.ResumeLayout(false);
            this.panListMain.ResumeLayout(false);
            this.panListSub.ResumeLayout(false);
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panJiByung.ResumeLayout(false);
            this.panJiByungMain.ResumeLayout(false);
            this.panJob.ResumeLayout(false);
            this.panJob.PerformLayout();
            this.panJiByungSub.ResumeLayout(false);
            this.panJiByungSub.PerformLayout();
            this.panTitleSub1.ResumeLayout(false);
            this.panTitleSub1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panList;
        private System.Windows.Forms.Panel panListMain;
        private System.Windows.Forms.Panel panListSub;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private DevComponents.DotNetBar.ListBoxAdv lstbox;
        private DevComponents.DotNetBar.ListBoxItem listBoxItem1;
        private System.Windows.Forms.ComboBox cboLGel;
        private System.Windows.Forms.Panel panJiByung;
        private System.Windows.Forms.Panel panJiByungMain;
        private System.Windows.Forms.Panel panJob;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panJiByungSub;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panTitleSub1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.Label lblBi;
        private System.Windows.Forms.ComboBox cboBi3;
        private System.Windows.Forms.ComboBox cboBi2;
        private System.Windows.Forms.ComboBox cboBi1;
        private System.Windows.Forms.Label lblDeptName;
        private System.Windows.Forms.ComboBox cboDept3;
        private System.Windows.Forms.ComboBox cboDept2;
        private System.Windows.Forms.ComboBox cboDept1;
        private System.Windows.Forms.ComboBox cboGel;
        private System.Windows.Forms.Label lblJumin;
        private System.Windows.Forms.Label lblSName;
        private System.Windows.Forms.Label lblLen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPano;
        private System.Windows.Forms.Label label9;
    }
}