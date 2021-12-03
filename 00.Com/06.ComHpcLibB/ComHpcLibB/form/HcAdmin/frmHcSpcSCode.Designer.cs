namespace ComHpcLibB
{
    partial class frmHcSpcSCode
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
            this.txtRowid = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.panPan = new System.Windows.Forms.Panel();
            this.txtSogen = new System.Windows.Forms.TextBox();
            this.txtReExam = new System.Windows.Forms.TextBox();
            this.txtSahu = new System.Windows.Forms.TextBox();
            this.cboUpmu = new System.Windows.Forms.ComboBox();
            this.txtJochi = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panCode = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.cboDBun = new System.Windows.Forms.ComboBox();
            this.cboMCode = new System.Windows.Forms.ComboBox();
            this.cboPanjeng = new System.Windows.Forms.ComboBox();
            this.dtpDelDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSort = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panList = new System.Windows.Forms.Panel();
            this.SS1 = new FarPoint.Win.Spread.FpSpread();
            this.SS1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkDel = new System.Windows.Forms.CheckBox();
            this.cboPanjeng2 = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rdoAuto3 = new System.Windows.Forms.RadioButton();
            this.rdoAuto2 = new System.Windows.Forms.RadioButton();
            this.rdoAuto1 = new System.Windows.Forms.RadioButton();
            this.btnReExam = new System.Windows.Forms.Button();
            this.btnSahu = new System.Windows.Forms.Button();
            this.panTitle.SuspendLayout();
            this.panMain.SuspendLayout();
            this.panPan.SuspendLayout();
            this.panCode.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.txtRowid);
            this.panTitle.Controls.Add(this.btnSave);
            this.panTitle.Controls.Add(this.btnDelete);
            this.panTitle.Controls.Add(this.btnCancel);
            this.panTitle.Controls.Add(this.btnExit);
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(895, 40);
            this.panTitle.TabIndex = 15;
            // 
            // txtRowid
            // 
            this.txtRowid.Location = new System.Drawing.Point(410, 7);
            this.txtRowid.MaxLength = 99999;
            this.txtRowid.Name = "txtRowid";
            this.txtRowid.Size = new System.Drawing.Size(72, 25);
            this.txtRowid.TabIndex = 16;
            this.txtRowid.Tag = "RID";
            this.txtRowid.Text = "txtRowid";
            this.txtRowid.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRowid.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.Location = new System.Drawing.Point(601, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(73, 38);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "저장(&O)";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDelete.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.Location = new System.Drawing.Point(674, 0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(73, 38);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "삭제(&D)";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.Location = new System.Drawing.Point(747, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 38);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "취소(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(820, 0);
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
            this.lblTitle.Size = new System.Drawing.Size(224, 38);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "특수검진 소견코드 등록관리";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panMain
            // 
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMain.Controls.Add(this.panPan);
            this.panMain.Controls.Add(this.panCode);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.panMain.Location = new System.Drawing.Point(0, 40);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(895, 247);
            this.panMain.TabIndex = 19;
            // 
            // panPan
            // 
            this.panPan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panPan.Controls.Add(this.btnSahu);
            this.panPan.Controls.Add(this.btnReExam);
            this.panPan.Controls.Add(this.txtSogen);
            this.panPan.Controls.Add(this.txtReExam);
            this.panPan.Controls.Add(this.txtSahu);
            this.panPan.Controls.Add(this.cboUpmu);
            this.panPan.Controls.Add(this.txtJochi);
            this.panPan.Controls.Add(this.label13);
            this.panPan.Controls.Add(this.label12);
            this.panPan.Controls.Add(this.label11);
            this.panPan.Controls.Add(this.label10);
            this.panPan.Controls.Add(this.label9);
            this.panPan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panPan.Location = new System.Drawing.Point(0, 114);
            this.panPan.Name = "panPan";
            this.panPan.Size = new System.Drawing.Size(893, 131);
            this.panPan.TabIndex = 19;
            // 
            // txtSogen
            // 
            this.txtSogen.Location = new System.Drawing.Point(117, 8);
            this.txtSogen.MaxLength = 99999;
            this.txtSogen.Name = "txtSogen";
            this.txtSogen.Size = new System.Drawing.Size(768, 25);
            this.txtSogen.TabIndex = 34;
            this.txtSogen.Tag = "SOGENREMARK";
            this.txtSogen.Text = "txtSogen";
            // 
            // txtReExam
            // 
            this.txtReExam.Location = new System.Drawing.Point(700, 98);
            this.txtReExam.MaxLength = 99999;
            this.txtReExam.Name = "txtReExam";
            this.txtReExam.Size = new System.Drawing.Size(154, 25);
            this.txtReExam.TabIndex = 33;
            this.txtReExam.Tag = "REEXAM";
            this.txtReExam.Text = "txtReExam";
            // 
            // txtSahu
            // 
            this.txtSahu.Location = new System.Drawing.Point(415, 98);
            this.txtSahu.MaxLength = 99999;
            this.txtSahu.Name = "txtSahu";
            this.txtSahu.Size = new System.Drawing.Size(147, 25);
            this.txtSahu.TabIndex = 32;
            this.txtSahu.Tag = "SAHUCODE";
            this.txtSahu.Text = "SAHUCODE";
            // 
            // cboUpmu
            // 
            this.cboUpmu.FormattingEnabled = true;
            this.cboUpmu.Location = new System.Drawing.Point(117, 98);
            this.cboUpmu.Name = "cboUpmu";
            this.cboUpmu.Size = new System.Drawing.Size(182, 25);
            this.cboUpmu.TabIndex = 31;
            this.cboUpmu.Tag = "WORKYN";
            this.cboUpmu.Text = "cboUpmu";
            // 
            // txtJochi
            // 
            this.txtJochi.Location = new System.Drawing.Point(117, 40);
            this.txtJochi.Multiline = true;
            this.txtJochi.Name = "txtJochi";
            this.txtJochi.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtJochi.Size = new System.Drawing.Size(768, 50);
            this.txtJochi.TabIndex = 30;
            this.txtJochi.Tag = "JOCHIREMARK";
            this.txtJochi.Text = "txtJochi";
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(600, 98);
            this.label13.Name = "label13";
            this.label13.Padding = new System.Windows.Forms.Padding(3);
            this.label13.Size = new System.Drawing.Size(94, 25);
            this.label13.TabIndex = 8;
            this.label13.Text = "추가검사";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.Location = new System.Drawing.Point(305, 98);
            this.label12.Name = "label12";
            this.label12.Padding = new System.Windows.Forms.Padding(3);
            this.label12.Size = new System.Drawing.Size(104, 25);
            this.label12.TabIndex = 7;
            this.label12.Text = "사후관리";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(7, 98);
            this.label11.Name = "label11";
            this.label11.Padding = new System.Windows.Forms.Padding(3);
            this.label11.Size = new System.Drawing.Size(104, 25);
            this.label11.TabIndex = 6;
            this.label11.Text = "업무적합";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(7, 40);
            this.label10.Name = "label10";
            this.label10.Padding = new System.Windows.Forms.Padding(3);
            this.label10.Size = new System.Drawing.Size(104, 50);
            this.label10.TabIndex = 5;
            this.label10.Text = "조치";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(7, 7);
            this.label9.Name = "label9";
            this.label9.Padding = new System.Windows.Forms.Padding(3);
            this.label9.Size = new System.Drawing.Size(104, 25);
            this.label9.TabIndex = 4;
            this.label9.Text = "소견";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panCode
            // 
            this.panCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panCode.Controls.Add(this.panel2);
            this.panCode.Controls.Add(this.label15);
            this.panCode.Controls.Add(this.cboDBun);
            this.panCode.Controls.Add(this.cboMCode);
            this.panCode.Controls.Add(this.cboPanjeng);
            this.panCode.Controls.Add(this.dtpDelDate);
            this.panCode.Controls.Add(this.label8);
            this.panCode.Controls.Add(this.txtSort);
            this.panCode.Controls.Add(this.label7);
            this.panCode.Controls.Add(this.label6);
            this.panCode.Controls.Add(this.txtGCode);
            this.panCode.Controls.Add(this.label5);
            this.panCode.Controls.Add(this.label4);
            this.panCode.Controls.Add(this.label2);
            this.panCode.Controls.Add(this.textBox1);
            this.panCode.Controls.Add(this.label1);
            this.panCode.Controls.Add(this.txtCode);
            this.panCode.Controls.Add(this.label3);
            this.panCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.panCode.Location = new System.Drawing.Point(0, 0);
            this.panCode.Name = "panCode";
            this.panCode.Size = new System.Drawing.Size(893, 114);
            this.panCode.TabIndex = 18;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.rdoAuto3);
            this.panel2.Controls.Add(this.rdoAuto2);
            this.panel2.Controls.Add(this.rdoAuto1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Location = new System.Drawing.Point(700, 76);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(185, 25);
            this.panel2.TabIndex = 32;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(600, 76);
            this.label15.Name = "label15";
            this.label15.Padding = new System.Windows.Forms.Padding(3);
            this.label15.Size = new System.Drawing.Size(94, 25);
            this.label15.TabIndex = 31;
            this.label15.Text = "자동판정기준";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboDBun
            // 
            this.cboDBun.FormattingEnabled = true;
            this.cboDBun.Location = new System.Drawing.Point(415, 75);
            this.cboDBun.Name = "cboDBun";
            this.cboDBun.Size = new System.Drawing.Size(178, 25);
            this.cboDBun.TabIndex = 29;
            this.cboDBun.Tag = "DBUN";
            this.cboDBun.Text = "cboDBun";
            // 
            // cboMCode
            // 
            this.cboMCode.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.cboMCode.FormattingEnabled = true;
            this.cboMCode.Location = new System.Drawing.Point(117, 76);
            this.cboMCode.Name = "cboMCode";
            this.cboMCode.Size = new System.Drawing.Size(182, 25);
            this.cboMCode.TabIndex = 28;
            this.cboMCode.Text = "cboMCode";
            // 
            // cboPanjeng
            // 
            this.cboPanjeng.BackColor = System.Drawing.Color.Wheat;
            this.cboPanjeng.FormattingEnabled = true;
            this.cboPanjeng.Location = new System.Drawing.Point(305, 41);
            this.cboPanjeng.Name = "cboPanjeng";
            this.cboPanjeng.Size = new System.Drawing.Size(288, 25);
            this.cboPanjeng.TabIndex = 27;
            this.cboPanjeng.Tag = "PANJENG2";
            this.cboPanjeng.Text = "cboPanjeng";
            // 
            // dtpDelDate
            // 
            this.dtpDelDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDelDate.Location = new System.Drawing.Point(701, 41);
            this.dtpDelDate.Name = "dtpDelDate";
            this.dtpDelDate.ShowCheckBox = true;
            this.dtpDelDate.Size = new System.Drawing.Size(184, 25);
            this.dtpDelDate.TabIndex = 26;
            this.dtpDelDate.Tag = "DELDATE";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(600, 41);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(3);
            this.label8.Size = new System.Drawing.Size(94, 25);
            this.label8.TabIndex = 25;
            this.label8.Text = "삭제일자";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSort
            // 
            this.txtSort.Location = new System.Drawing.Point(701, 6);
            this.txtSort.MaxLength = 99999;
            this.txtSort.Name = "txtSort";
            this.txtSort.Size = new System.Drawing.Size(184, 25);
            this.txtSort.TabIndex = 24;
            this.txtSort.Tag = "SORT";
            this.txtSort.Text = "txtSort";
            this.txtSort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(600, 6);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(3);
            this.label7.Size = new System.Drawing.Size(94, 25);
            this.label7.TabIndex = 23;
            this.label7.Text = "순위";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(7, 76);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(3);
            this.label6.Size = new System.Drawing.Size(104, 25);
            this.label6.TabIndex = 22;
            this.label6.Text = "취급물질";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtGCode
            // 
            this.txtGCode.Location = new System.Drawing.Point(117, 41);
            this.txtGCode.MaxLength = 5;
            this.txtGCode.Name = "txtGCode";
            this.txtGCode.Size = new System.Drawing.Size(72, 25);
            this.txtGCode.TabIndex = 21;
            this.txtGCode.Tag = "GCODE";
            this.txtGCode.Text = "txtGCode";
            this.txtGCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(7, 41);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(3);
            this.label5.Size = new System.Drawing.Size(104, 25);
            this.label5.TabIndex = 20;
            this.label5.Text = "공단소견코드";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(305, 76);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(3);
            this.label4.Size = new System.Drawing.Size(104, 25);
            this.label4.TabIndex = 19;
            this.label4.Text = "질병분류";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(195, 41);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(3);
            this.label2.Size = new System.Drawing.Size(104, 25);
            this.label2.TabIndex = 18;
            this.label2.Text = "판정종류";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(305, 6);
            this.textBox1.MaxLength = 99999;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(288, 25);
            this.textBox1.TabIndex = 17;
            this.textBox1.Tag = "NAME";
            this.textBox1.Text = "txtName";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(195, 6);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(3);
            this.label1.Size = new System.Drawing.Size(104, 25);
            this.label1.TabIndex = 16;
            this.label1.Text = "소견코드명";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.Wheat;
            this.txtCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCode.Location = new System.Drawing.Point(117, 6);
            this.txtCode.MaxLength = 5;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(72, 25);
            this.txtCode.TabIndex = 15;
            this.txtCode.Tag = "CODE";
            this.txtCode.Text = "TXTCODE";
            this.txtCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(7, 6);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(3);
            this.label3.Size = new System.Drawing.Size(104, 25);
            this.label3.TabIndex = 3;
            this.label3.Text = "소견코드";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panList
            // 
            this.panList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panList.Controls.Add(this.SS1);
            this.panList.Controls.Add(this.panel1);
            this.panList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panList.Location = new System.Drawing.Point(0, 287);
            this.panList.Name = "panList";
            this.panList.Size = new System.Drawing.Size(895, 341);
            this.panList.TabIndex = 22;
            // 
            // SS1
            // 
            this.SS1.AccessibleDescription = "";
            this.SS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SS1.FocusRenderer = flatFocusIndicatorRenderer1;
            this.SS1.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.HorizontalScrollBar.Name = "";
            flatScrollBarRenderer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.HorizontalScrollBar.Renderer = flatScrollBarRenderer1;
            this.SS1.HorizontalScrollBar.TabIndex = 100;
            this.SS1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            this.SS1.Location = new System.Drawing.Point(0, 36);
            this.SS1.Name = "SS1";
            this.SS1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SS1_Sheet1});
            this.SS1.Size = new System.Drawing.Size(893, 303);
            this.SS1.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2013;
            this.SS1.TabIndex = 21;
            this.SS1.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
            this.SS1.VerticalScrollBar.Name = "";
            flatScrollBarRenderer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SS1.VerticalScrollBar.Renderer = flatScrollBarRenderer2;
            this.SS1.VerticalScrollBar.TabIndex = 101;
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
            // panel1
            // 
            this.panel1.Controls.Add(this.chkDel);
            this.panel1.Controls.Add(this.cboPanjeng2);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(893, 36);
            this.panel1.TabIndex = 19;
            // 
            // chkDel
            // 
            this.chkDel.AutoSize = true;
            this.chkDel.ForeColor = System.Drawing.Color.DarkRed;
            this.chkDel.Location = new System.Drawing.Point(11, 8);
            this.chkDel.Name = "chkDel";
            this.chkDel.Size = new System.Drawing.Size(105, 21);
            this.chkDel.TabIndex = 33;
            this.chkDel.Tag = "";
            this.chkDel.Text = "삭제코드포함";
            this.chkDel.UseVisualStyleBackColor = true;
            // 
            // cboPanjeng2
            // 
            this.cboPanjeng2.FormattingEnabled = true;
            this.cboPanjeng2.Location = new System.Drawing.Point(510, 4);
            this.cboPanjeng2.Name = "cboPanjeng2";
            this.cboPanjeng2.Size = new System.Drawing.Size(221, 25);
            this.cboPanjeng2.TabIndex = 32;
            this.cboPanjeng2.Text = "cboPanjeng2";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(444, 9);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(60, 17);
            this.label14.TabIndex = 11;
            this.label14.Text = "판정구분";
            // 
            // btnSearch
            // 
            this.btnSearch.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSearch.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSearch.Location = new System.Drawing.Point(739, 0);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(154, 36);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "자료조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(6, 23);
            this.panel3.TabIndex = 3;
            // 
            // rdoAuto3
            // 
            this.rdoAuto3.AutoSize = true;
            this.rdoAuto3.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoAuto3.Location = new System.Drawing.Point(110, 0);
            this.rdoAuto3.Name = "rdoAuto3";
            this.rdoAuto3.Size = new System.Drawing.Size(52, 23);
            this.rdoAuto3.TabIndex = 6;
            this.rdoAuto3.Text = "소견";
            this.rdoAuto3.UseVisualStyleBackColor = true;
            // 
            // rdoAuto2
            // 
            this.rdoAuto2.AutoSize = true;
            this.rdoAuto2.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoAuto2.Location = new System.Drawing.Point(58, 0);
            this.rdoAuto2.Name = "rdoAuto2";
            this.rdoAuto2.Size = new System.Drawing.Size(52, 23);
            this.rdoAuto2.TabIndex = 5;
            this.rdoAuto2.Text = "판정";
            this.rdoAuto2.UseVisualStyleBackColor = true;
            // 
            // rdoAuto1
            // 
            this.rdoAuto1.AutoSize = true;
            this.rdoAuto1.Checked = true;
            this.rdoAuto1.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoAuto1.Location = new System.Drawing.Point(6, 0);
            this.rdoAuto1.Name = "rdoAuto1";
            this.rdoAuto1.Size = new System.Drawing.Size(52, 23);
            this.rdoAuto1.TabIndex = 4;
            this.rdoAuto1.TabStop = true;
            this.rdoAuto1.Text = "없음";
            this.rdoAuto1.UseVisualStyleBackColor = true;
            // 
            // btnReExam
            // 
            this.btnReExam.BackColor = System.Drawing.Color.White;
            this.btnReExam.Location = new System.Drawing.Point(858, 97);
            this.btnReExam.Name = "btnReExam";
            this.btnReExam.Size = new System.Drawing.Size(27, 27);
            this.btnReExam.TabIndex = 35;
            this.btnReExam.Text = "&H";
            this.btnReExam.UseVisualStyleBackColor = false;
            // 
            // btnSahu
            // 
            this.btnSahu.BackColor = System.Drawing.Color.White;
            this.btnSahu.Location = new System.Drawing.Point(566, 97);
            this.btnSahu.Name = "btnSahu";
            this.btnSahu.Size = new System.Drawing.Size(27, 27);
            this.btnSahu.TabIndex = 36;
            this.btnSahu.Text = "&H";
            this.btnSahu.UseVisualStyleBackColor = false;
            // 
            // frmHcSpcSCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 628);
            this.Controls.Add(this.panList);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcSpcSCode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "특수검진 소견코드 등록";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panMain.ResumeLayout(false);
            this.panPan.ResumeLayout(false);
            this.panPan.PerformLayout();
            this.panCode.ResumeLayout(false);
            this.panCode.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SS1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS1_Sheet1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.Panel panPan;
        private System.Windows.Forms.TextBox txtSogen;
        private System.Windows.Forms.TextBox txtReExam;
        private System.Windows.Forms.TextBox txtSahu;
        private System.Windows.Forms.ComboBox cboUpmu;
        private System.Windows.Forms.TextBox txtJochi;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panCode;
        private System.Windows.Forms.ComboBox cboDBun;
        private System.Windows.Forms.ComboBox cboMCode;
        private System.Windows.Forms.ComboBox cboPanjeng;
        private System.Windows.Forms.DateTimePicker dtpDelDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSort;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cboPanjeng2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtRowid;
        private System.Windows.Forms.CheckBox chkDel;
        private FarPoint.Win.Spread.FpSpread SS1;
        private FarPoint.Win.Spread.SheetView SS1_Sheet1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.RadioButton rdoAuto3;
        private System.Windows.Forms.RadioButton rdoAuto2;
        private System.Windows.Forms.RadioButton rdoAuto1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnReExam;
        private System.Windows.Forms.Button btnSahu;
    }
}