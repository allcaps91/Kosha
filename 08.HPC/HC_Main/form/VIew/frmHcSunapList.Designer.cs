namespace HC_Main
{
    partial class frmHcSunapList
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
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panSub01 = new System.Windows.Forms.Panel();
            this.panSub04 = new System.Windows.Forms.Panel();
            this.btnLtdHelp = new System.Windows.Forms.Button();
            this.txtLtdName = new System.Windows.Forms.TextBox();
            this.lblSub03 = new System.Windows.Forms.Label();
            this.panSub03 = new System.Windows.Forms.Panel();
            this.cboJong = new System.Windows.Forms.ComboBox();
            this.lblSub02 = new System.Windows.Forms.Label();
            this.panSub05 = new System.Windows.Forms.Panel();
            this.rdoJob3 = new System.Windows.Forms.RadioButton();
            this.rdoJob2 = new System.Windows.Forms.RadioButton();
            this.rdoJob1 = new System.Windows.Forms.RadioButton();
            this.lblSub04 = new System.Windows.Forms.Label();
            this.panSub02 = new System.Windows.Forms.Panel();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.lblSub01 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rdoSunap2 = new System.Windows.Forms.RadioButton();
            this.rdoSunap1 = new System.Windows.Forms.RadioButton();
            this.rdoSunap0 = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtSname = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cboHalinGye = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblChaAmt = new System.Windows.Forms.Label();
            this.panSub07 = new System.Windows.Forms.Panel();
            this.rdoAll2 = new System.Windows.Forms.RadioButton();
            this.rdoAll1 = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.chkZeroAmt = new System.Windows.Forms.CheckBox();
            this.panSub06 = new System.Windows.Forms.Panel();
            this.rdoHis2 = new System.Windows.Forms.RadioButton();
            this.rdoHis1 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panTitle.SuspendLayout();
            this.panSub01.SuspendLayout();
            this.panSub04.SuspendLayout();
            this.panSub03.SuspendLayout();
            this.panSub05.SuspendLayout();
            this.panSub02.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panSub07.SuspendLayout();
            this.panSub06.SuspendLayout();
            this.panMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).BeginInit();
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
            this.panTitle.Size = new System.Drawing.Size(1361, 40);
            this.panTitle.TabIndex = 15;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(3, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(150, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "검진비 수납자 명단";
            // 
            // panSub01
            // 
            this.panSub01.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSub01.Controls.Add(this.btnSearch);
            this.panSub01.Controls.Add(this.btnPrint);
            this.panSub01.Controls.Add(this.panSub04);
            this.panSub01.Controls.Add(this.lblSub03);
            this.panSub01.Controls.Add(this.panSub03);
            this.panSub01.Controls.Add(this.lblSub02);
            this.panSub01.Controls.Add(this.panSub05);
            this.panSub01.Controls.Add(this.lblSub04);
            this.panSub01.Controls.Add(this.panSub02);
            this.panSub01.Controls.Add(this.lblSub01);
            this.panSub01.Controls.Add(this.btnExit);
            this.panSub01.Dock = System.Windows.Forms.DockStyle.Top;
            this.panSub01.Location = new System.Drawing.Point(0, 40);
            this.panSub01.Name = "panSub01";
            this.panSub01.Padding = new System.Windows.Forms.Padding(1);
            this.panSub01.Size = new System.Drawing.Size(1361, 35);
            this.panSub01.TabIndex = 134;
            // 
            // panSub04
            // 
            this.panSub04.Controls.Add(this.btnLtdHelp);
            this.panSub04.Controls.Add(this.txtLtdName);
            this.panSub04.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub04.Location = new System.Drawing.Point(771, 1);
            this.panSub04.Name = "panSub04";
            this.panSub04.Size = new System.Drawing.Size(201, 31);
            this.panSub04.TabIndex = 54;
            // 
            // btnLtdHelp
            // 
            this.btnLtdHelp.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLtdHelp.Location = new System.Drawing.Point(173, 3);
            this.btnLtdHelp.Name = "btnLtdHelp";
            this.btnLtdHelp.Size = new System.Drawing.Size(24, 25);
            this.btnLtdHelp.TabIndex = 92;
            this.btnLtdHelp.Text = "&H";
            this.btnLtdHelp.UseVisualStyleBackColor = true;
            // 
            // txtLtdName
            // 
            this.txtLtdName.BackColor = System.Drawing.SystemColors.Window;
            this.txtLtdName.Location = new System.Drawing.Point(3, 3);
            this.txtLtdName.Name = "txtLtdName";
            this.txtLtdName.Size = new System.Drawing.Size(168, 25);
            this.txtLtdName.TabIndex = 91;
            this.txtLtdName.Tag = "";
            this.txtLtdName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblSub03
            // 
            this.lblSub03.BackColor = System.Drawing.Color.LightBlue;
            this.lblSub03.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSub03.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSub03.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSub03.Location = new System.Drawing.Point(708, 1);
            this.lblSub03.Name = "lblSub03";
            this.lblSub03.Size = new System.Drawing.Size(63, 31);
            this.lblSub03.TabIndex = 53;
            this.lblSub03.Text = "회사코드";
            this.lblSub03.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panSub03
            // 
            this.panSub03.Controls.Add(this.cboJong);
            this.panSub03.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub03.Location = new System.Drawing.Point(555, 1);
            this.panSub03.Name = "panSub03";
            this.panSub03.Size = new System.Drawing.Size(153, 31);
            this.panSub03.TabIndex = 52;
            // 
            // cboJong
            // 
            this.cboJong.FormattingEnabled = true;
            this.cboJong.Location = new System.Drawing.Point(3, 3);
            this.cboJong.Name = "cboJong";
            this.cboJong.Size = new System.Drawing.Size(147, 25);
            this.cboJong.TabIndex = 0;
            // 
            // lblSub02
            // 
            this.lblSub02.BackColor = System.Drawing.Color.LightBlue;
            this.lblSub02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSub02.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSub02.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSub02.Location = new System.Drawing.Point(492, 1);
            this.lblSub02.Name = "lblSub02";
            this.lblSub02.Size = new System.Drawing.Size(63, 31);
            this.lblSub02.TabIndex = 51;
            this.lblSub02.Text = "검진종류";
            this.lblSub02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panSub05
            // 
            this.panSub05.AutoSize = true;
            this.panSub05.Controls.Add(this.rdoJob3);
            this.panSub05.Controls.Add(this.rdoJob2);
            this.panSub05.Controls.Add(this.rdoJob1);
            this.panSub05.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub05.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.panSub05.Location = new System.Drawing.Point(317, 1);
            this.panSub05.Name = "panSub05";
            this.panSub05.Padding = new System.Windows.Forms.Padding(3);
            this.panSub05.Size = new System.Drawing.Size(175, 31);
            this.panSub05.TabIndex = 50;
            // 
            // rdoJob3
            // 
            this.rdoJob3.AutoSize = true;
            this.rdoJob3.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoJob3.Location = new System.Drawing.Point(120, 3);
            this.rdoJob3.Name = "rdoJob3";
            this.rdoJob3.Size = new System.Drawing.Size(52, 25);
            this.rdoJob3.TabIndex = 2;
            this.rdoJob3.Text = "청구";
            this.rdoJob3.UseVisualStyleBackColor = true;
            // 
            // rdoJob2
            // 
            this.rdoJob2.AutoSize = true;
            this.rdoJob2.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoJob2.Location = new System.Drawing.Point(55, 3);
            this.rdoJob2.Name = "rdoJob2";
            this.rdoJob2.Size = new System.Drawing.Size(65, 25);
            this.rdoJob2.TabIndex = 1;
            this.rdoJob2.Text = "미청구";
            this.rdoJob2.UseVisualStyleBackColor = true;
            // 
            // rdoJob1
            // 
            this.rdoJob1.AutoSize = true;
            this.rdoJob1.Checked = true;
            this.rdoJob1.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoJob1.Location = new System.Drawing.Point(3, 3);
            this.rdoJob1.Name = "rdoJob1";
            this.rdoJob1.Size = new System.Drawing.Size(52, 25);
            this.rdoJob1.TabIndex = 0;
            this.rdoJob1.TabStop = true;
            this.rdoJob1.Text = "전체";
            this.rdoJob1.UseVisualStyleBackColor = true;
            // 
            // lblSub04
            // 
            this.lblSub04.BackColor = System.Drawing.Color.LightBlue;
            this.lblSub04.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSub04.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSub04.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSub04.Location = new System.Drawing.Point(254, 1);
            this.lblSub04.Name = "lblSub04";
            this.lblSub04.Size = new System.Drawing.Size(63, 31);
            this.lblSub04.TabIndex = 49;
            this.lblSub04.Text = "청구구분";
            this.lblSub04.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panSub02
            // 
            this.panSub02.Controls.Add(this.dtpTDate);
            this.panSub02.Controls.Add(this.dtpFDate);
            this.panSub02.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub02.Location = new System.Drawing.Point(64, 1);
            this.panSub02.Name = "panSub02";
            this.panSub02.Size = new System.Drawing.Size(190, 31);
            this.panSub02.TabIndex = 44;
            // 
            // dtpTDate
            // 
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(97, 3);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(88, 25);
            this.dtpTDate.TabIndex = 1;
            // 
            // dtpFDate
            // 
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(3, 3);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(88, 25);
            this.dtpFDate.TabIndex = 0;
            // 
            // lblSub01
            // 
            this.lblSub01.BackColor = System.Drawing.Color.LightBlue;
            this.lblSub01.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSub01.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSub01.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSub01.Location = new System.Drawing.Point(1, 1);
            this.lblSub01.Name = "lblSub01";
            this.lblSub01.Size = new System.Drawing.Size(63, 31);
            this.lblSub01.TabIndex = 43;
            this.lblSub01.Text = "검진일자";
            this.lblSub01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(1276, 1);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(82, 31);
            this.btnExit.TabIndex = 19;
            this.btnExit.Text = "닫 기(&X)";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lblChaAmt);
            this.panel1.Controls.Add(this.panSub07);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.chkZeroAmt);
            this.panel1.Controls.Add(this.panSub06);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 75);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(1);
            this.panel1.Size = new System.Drawing.Size(1361, 35);
            this.panel1.TabIndex = 137;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.rdoSunap2);
            this.panel4.Controls.Add(this.rdoSunap1);
            this.panel4.Controls.Add(this.rdoSunap0);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(883, 1);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(224, 31);
            this.panel4.TabIndex = 67;
            // 
            // rdoSunap2
            // 
            this.rdoSunap2.AutoSize = true;
            this.rdoSunap2.Location = new System.Drawing.Point(153, 5);
            this.rdoSunap2.Name = "rdoSunap2";
            this.rdoSunap2.Size = new System.Drawing.Size(65, 21);
            this.rdoSunap2.TabIndex = 2;
            this.rdoSunap2.Text = "원무과";
            this.rdoSunap2.UseVisualStyleBackColor = true;
            // 
            // rdoSunap1
            // 
            this.rdoSunap1.AutoSize = true;
            this.rdoSunap1.Location = new System.Drawing.Point(70, 5);
            this.rdoSunap1.Name = "rdoSunap1";
            this.rdoSunap1.Size = new System.Drawing.Size(78, 21);
            this.rdoSunap1.TabIndex = 1;
            this.rdoSunap1.Text = "건진센터";
            this.rdoSunap1.UseVisualStyleBackColor = true;
            // 
            // rdoSunap0
            // 
            this.rdoSunap0.AutoSize = true;
            this.rdoSunap0.Checked = true;
            this.rdoSunap0.Location = new System.Drawing.Point(11, 5);
            this.rdoSunap0.Name = "rdoSunap0";
            this.rdoSunap0.Size = new System.Drawing.Size(52, 21);
            this.rdoSunap0.TabIndex = 0;
            this.rdoSunap0.TabStop = true;
            this.rdoSunap0.Text = "전체";
            this.rdoSunap0.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.LightBlue;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(820, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 31);
            this.label5.TabIndex = 66;
            this.label5.Text = "수납구분";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.Controls.Add(this.txtSname);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(719, 1);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(101, 31);
            this.panel3.TabIndex = 65;
            // 
            // txtSname
            // 
            this.txtSname.Location = new System.Drawing.Point(3, 3);
            this.txtSname.Name = "txtSname";
            this.txtSname.Size = new System.Drawing.Size(95, 25);
            this.txtSname.TabIndex = 91;
            this.txtSname.Tag = "";
            this.txtSname.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightBlue;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(656, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 31);
            this.label2.TabIndex = 64;
            this.label2.Text = "수검자명";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cboHalinGye);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(409, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(247, 31);
            this.panel2.TabIndex = 63;
            // 
            // cboHalinGye
            // 
            this.cboHalinGye.FormattingEnabled = true;
            this.cboHalinGye.Location = new System.Drawing.Point(3, 3);
            this.cboHalinGye.Name = "cboHalinGye";
            this.cboHalinGye.Size = new System.Drawing.Size(241, 25);
            this.cboHalinGye.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.LightBlue;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(347, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 31);
            this.label4.TabIndex = 62;
            this.label4.Text = "할인계정";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblChaAmt
            // 
            this.lblChaAmt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.lblChaAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblChaAmt.Location = new System.Drawing.Point(1148, 4);
            this.lblChaAmt.Name = "lblChaAmt";
            this.lblChaAmt.Size = new System.Drawing.Size(76, 26);
            this.lblChaAmt.TabIndex = 61;
            this.lblChaAmt.Text = "차액발생";
            this.lblChaAmt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panSub07
            // 
            this.panSub07.AutoSize = true;
            this.panSub07.Controls.Add(this.rdoAll2);
            this.panSub07.Controls.Add(this.rdoAll1);
            this.panSub07.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub07.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.panSub07.Location = new System.Drawing.Point(237, 1);
            this.panSub07.Name = "panSub07";
            this.panSub07.Padding = new System.Windows.Forms.Padding(3);
            this.panSub07.Size = new System.Drawing.Size(110, 31);
            this.panSub07.TabIndex = 58;
            // 
            // rdoAll2
            // 
            this.rdoAll2.AutoSize = true;
            this.rdoAll2.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoAll2.Location = new System.Drawing.Point(55, 3);
            this.rdoAll2.Name = "rdoAll2";
            this.rdoAll2.Size = new System.Drawing.Size(52, 25);
            this.rdoAll2.TabIndex = 3;
            this.rdoAll2.Text = "종검";
            this.rdoAll2.UseVisualStyleBackColor = true;
            // 
            // rdoAll1
            // 
            this.rdoAll1.AutoSize = true;
            this.rdoAll1.Checked = true;
            this.rdoAll1.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoAll1.Location = new System.Drawing.Point(3, 3);
            this.rdoAll1.Name = "rdoAll1";
            this.rdoAll1.Size = new System.Drawing.Size(52, 25);
            this.rdoAll1.TabIndex = 2;
            this.rdoAll1.TabStop = true;
            this.rdoAll1.Text = "일반";
            this.rdoAll1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LightBlue;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(174, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 31);
            this.label3.TabIndex = 57;
            this.label3.Text = "조회구분";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkZeroAmt
            // 
            this.chkZeroAmt.AutoSize = true;
            this.chkZeroAmt.Location = new System.Drawing.Point(1232, 7);
            this.chkZeroAmt.Name = "chkZeroAmt";
            this.chkZeroAmt.Size = new System.Drawing.Size(122, 21);
            this.chkZeroAmt.TabIndex = 56;
            this.chkZeroAmt.Text = "입금액 0원 제외";
            this.chkZeroAmt.UseVisualStyleBackColor = true;
            // 
            // panSub06
            // 
            this.panSub06.AutoSize = true;
            this.panSub06.Controls.Add(this.rdoHis2);
            this.panSub06.Controls.Add(this.rdoHis1);
            this.panSub06.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSub06.ImeMode = System.Windows.Forms.ImeMode.Hangul;
            this.panSub06.Location = new System.Drawing.Point(64, 1);
            this.panSub06.Name = "panSub06";
            this.panSub06.Padding = new System.Windows.Forms.Padding(3);
            this.panSub06.Size = new System.Drawing.Size(110, 31);
            this.panSub06.TabIndex = 51;
            // 
            // rdoHis2
            // 
            this.rdoHis2.AutoSize = true;
            this.rdoHis2.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoHis2.Location = new System.Drawing.Point(55, 3);
            this.rdoHis2.Name = "rdoHis2";
            this.rdoHis2.Size = new System.Drawing.Size(52, 25);
            this.rdoHis2.TabIndex = 2;
            this.rdoHis2.Text = "합계";
            this.rdoHis2.UseVisualStyleBackColor = true;
            // 
            // rdoHis1
            // 
            this.rdoHis1.AutoSize = true;
            this.rdoHis1.Checked = true;
            this.rdoHis1.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoHis1.Location = new System.Drawing.Point(3, 3);
            this.rdoHis1.Name = "rdoHis1";
            this.rdoHis1.Size = new System.Drawing.Size(52, 25);
            this.rdoHis1.TabIndex = 1;
            this.rdoHis1.TabStop = true;
            this.rdoHis1.Text = "상세";
            this.rdoHis1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightBlue;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 31);
            this.label1.TabIndex = 44;
            this.label1.Text = "집계구분";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panMain
            // 
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMain.Controls.Add(this.SSList);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 110);
            this.panMain.Name = "panMain";
            this.panMain.Padding = new System.Windows.Forms.Padding(1);
            this.panMain.Size = new System.Drawing.Size(1361, 951);
            this.panMain.TabIndex = 138;
            // 
            // SSList
            // 
            this.SSList.AccessibleDescription = "fpSpread1, Sheet1, Row 0, Column 0, ";
            this.SSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSList.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SSList.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SSList.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SSList.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.SSList.HorizontalScrollBar.TabIndex = 93;
            this.SSList.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SSList.Location = new System.Drawing.Point(1, 1);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(1357, 947);
            this.SSList.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SSList.TabIndex = 139;
            this.SSList.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SSList.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SSList.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.SSList.VerticalScrollBar.TabIndex = 94;
            this.SSList.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 5;
            this.SSList_Sheet1.RowCount = 1;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnFooterFlat";
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterFlat";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderFlat";
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.SSList_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarFlat";
            this.SSList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "FilterBarHeaderFlat";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.HorizontalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderFlat";
            this.SSList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.Rows.Get(0).Height = 24F;
            this.SSList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.SheetCornerStyle.Parent = "CornerHeaderFlat";
            this.SSList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.VerticalGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, System.Drawing.Color.Gray);
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.White;
            this.btnPrint.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnPrint.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPrint.Location = new System.Drawing.Point(1194, 1);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(82, 31);
            this.btnPrint.TabIndex = 55;
            this.btnPrint.Text = "출 력(&P)";
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.White;
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(1112, 1);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(82, 31);
            this.btnSearch.TabIndex = 56;
            this.btnSearch.Text = "조 회(&V)";
            this.btnSearch.UseVisualStyleBackColor = false;
            // 
            // frmHcSunapList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1361, 1061);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panSub01);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcSunapList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "검진비 수납자 명단";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panSub01.ResumeLayout(false);
            this.panSub01.PerformLayout();
            this.panSub04.ResumeLayout(false);
            this.panSub04.PerformLayout();
            this.panSub03.ResumeLayout(false);
            this.panSub05.ResumeLayout(false);
            this.panSub05.PerformLayout();
            this.panSub02.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panSub07.ResumeLayout(false);
            this.panSub07.PerformLayout();
            this.panSub06.ResumeLayout(false);
            this.panSub06.PerformLayout();
            this.panMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panSub01;
        private System.Windows.Forms.Panel panSub05;
        private System.Windows.Forms.Label lblSub04;
        private System.Windows.Forms.Panel panSub02;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.Label lblSub01;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panSub04;
        private System.Windows.Forms.Button btnLtdHelp;
        private System.Windows.Forms.TextBox txtLtdName;
        private System.Windows.Forms.Label lblSub03;
        private System.Windows.Forms.Panel panSub03;
        private System.Windows.Forms.ComboBox cboJong;
        private System.Windows.Forms.Label lblSub02;
        private System.Windows.Forms.RadioButton rdoJob3;
        private System.Windows.Forms.RadioButton rdoJob2;
        private System.Windows.Forms.RadioButton rdoJob1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblChaAmt;
        private System.Windows.Forms.Panel panSub07;
        private System.Windows.Forms.RadioButton rdoAll2;
        private System.Windows.Forms.RadioButton rdoAll1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkZeroAmt;
        private System.Windows.Forms.Panel panSub06;
        private System.Windows.Forms.RadioButton rdoHis2;
        private System.Windows.Forms.RadioButton rdoHis1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panMain;
        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtSname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cboHalinGye;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton rdoSunap2;
        private System.Windows.Forms.RadioButton rdoSunap1;
        private System.Windows.Forms.RadioButton rdoSunap0;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
    }
}