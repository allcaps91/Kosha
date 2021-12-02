namespace HC_Print
{
    partial class frmHcPrint_Cancer
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
            FarPoint.Win.Spread.CellType.CheckBoxCellType checkBoxCellType2 = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTongbo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUnTongbo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBal = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpFDate = new System.Windows.Forms.DateTimePicker();
            this.dtpTDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpTongbo = new System.Windows.Forms.DateTimePicker();
            this.panel11 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtWrtno = new System.Windows.Forms.TextBox();
            this.panel20 = new System.Windows.Forms.Panel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.panel13 = new System.Windows.Forms.Panel();
            this.panel14 = new System.Windows.Forms.Panel();
            this.panel16 = new System.Windows.Forms.Panel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.ChkGbRe = new System.Windows.Forms.CheckBox();
            this.panel21 = new System.Windows.Forms.Panel();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.txtLtdCode = new System.Windows.Forms.TextBox();
            this.btnLtdHelp = new System.Windows.Forms.Button();
            this.lblLtdName = new System.Windows.Forms.Label();
            this.panel15 = new System.Windows.Forms.Panel();
            this.panel18 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdoCNT2 = new System.Windows.Forms.RadioButton();
            this.rdoCNT1 = new System.Windows.Forms.RadioButton();
            this.panel19 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnWebPrint = new System.Windows.Forms.Button();
            this.btnWebSearch = new System.Windows.Forms.Button();
            this.panel17 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ChkAll = new System.Windows.Forms.CheckBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.SSList_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.SSList = new FarPoint.Win.Spread.FpSpread();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel10.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel7.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel20.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.panel14.SuspendLayout();
            this.panel16.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panel21.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.panel18.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuExit,
            this.menuPrint,
            this.menuTongbo,
            this.menuUnTongbo,
            this.menuBal});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(6, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1371, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(43, 19);
            this.menuExit.Text = "종료";
            // 
            // menuPrint
            // 
            this.menuPrint.Name = "menuPrint";
            this.menuPrint.Size = new System.Drawing.Size(43, 19);
            this.menuPrint.Text = "인쇄";
            // 
            // menuTongbo
            // 
            this.menuTongbo.Name = "menuTongbo";
            this.menuTongbo.Size = new System.Drawing.Size(91, 19);
            this.menuTongbo.Text = "통보일자세팅";
            // 
            // menuUnTongbo
            // 
            this.menuUnTongbo.Name = "menuUnTongbo";
            this.menuUnTongbo.Size = new System.Drawing.Size(91, 19);
            this.menuUnTongbo.Text = "결과통보해제";
            // 
            // menuBal
            // 
            this.menuBal.Name = "menuBal";
            this.menuBal.Size = new System.Drawing.Size(91, 19);
            this.menuBal.Text = "발송일자세팅";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel17);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel19);
            this.panel2.Controls.Add(this.panel18);
            this.panel2.Controls.Add(this.panel15);
            this.panel2.Controls.Add(this.panel14);
            this.panel2.Controls.Add(this.panel13);
            this.panel2.Controls.Add(this.panel12);
            this.panel2.Controls.Add(this.panel11);
            this.panel2.Controls.Add(this.panel10);
            this.panel2.Controls.Add(this.panel8);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1371, 100);
            this.panel2.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(10, 100);
            this.panel8.TabIndex = 4;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.groupBox6);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel10.Location = new System.Drawing.Point(10, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(261, 100);
            this.panel10.TabIndex = 5;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.dtpTongbo);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.dtpTDate);
            this.groupBox6.Controls.Add(this.dtpFDate);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(261, 100);
            this.groupBox6.TabIndex = 57;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "접수일자 및 통보일자";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightBlue;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(3, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 31);
            this.label1.TabIndex = 58;
            this.label1.Text = "검진일자";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpFDate
            // 
            this.dtpFDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFDate.Location = new System.Drawing.Point(71, 20);
            this.dtpFDate.Name = "dtpFDate";
            this.dtpFDate.Size = new System.Drawing.Size(88, 25);
            this.dtpFDate.TabIndex = 56;
            // 
            // dtpTDate
            // 
            this.dtpTDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTDate.Location = new System.Drawing.Point(165, 20);
            this.dtpTDate.Name = "dtpTDate";
            this.dtpTDate.Size = new System.Drawing.Size(88, 25);
            this.dtpTDate.TabIndex = 57;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightBlue;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(3, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 31);
            this.label2.TabIndex = 59;
            this.label2.Text = "통보일자";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpTongbo
            // 
            this.dtpTongbo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTongbo.Location = new System.Drawing.Point(71, 60);
            this.dtpTongbo.Name = "dtpTongbo";
            this.dtpTongbo.Size = new System.Drawing.Size(88, 25);
            this.dtpTongbo.TabIndex = 60;
            // 
            // panel11
            // 
            this.panel11.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel11.Location = new System.Drawing.Point(271, 0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(10, 100);
            this.panel11.TabIndex = 6;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.panel20);
            this.panel12.Controls.Add(this.panel7);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel12.Location = new System.Drawing.Point(281, 0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(229, 100);
            this.panel12.TabIndex = 7;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.groupBox2);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(229, 53);
            this.panel7.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtWrtno);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(229, 53);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "접수번호";
            // 
            // txtWrtno
            // 
            this.txtWrtno.Location = new System.Drawing.Point(6, 18);
            this.txtWrtno.Name = "txtWrtno";
            this.txtWrtno.Size = new System.Drawing.Size(216, 25);
            this.txtWrtno.TabIndex = 98;
            this.txtWrtno.Tag = "";
            this.txtWrtno.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel20
            // 
            this.panel20.Controls.Add(this.groupBox7);
            this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel20.Location = new System.Drawing.Point(0, 53);
            this.panel20.Name = "panel20";
            this.panel20.Size = new System.Drawing.Size(229, 46);
            this.panel20.TabIndex = 1;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.txtName);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox7.Location = new System.Drawing.Point(0, 0);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(229, 46);
            this.groupBox7.TabIndex = 17;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "성명";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(6, 18);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(216, 25);
            this.txtName.TabIndex = 97;
            this.txtName.Tag = "";
            this.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel13
            // 
            this.panel13.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel13.Location = new System.Drawing.Point(510, 0);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(10, 100);
            this.panel13.TabIndex = 8;
            // 
            // panel14
            // 
            this.panel14.Controls.Add(this.panel16);
            this.panel14.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel14.Location = new System.Drawing.Point(520, 0);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(228, 100);
            this.panel14.TabIndex = 9;
            // 
            // panel16
            // 
            this.panel16.Controls.Add(this.panel21);
            this.panel16.Controls.Add(this.groupBox5);
            this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel16.Location = new System.Drawing.Point(0, 0);
            this.panel16.Name = "panel16";
            this.panel16.Size = new System.Drawing.Size(228, 100);
            this.panel16.TabIndex = 11;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.ChkGbRe);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(228, 50);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "구분";
            // 
            // ChkGbRe
            // 
            this.ChkGbRe.AutoSize = true;
            this.ChkGbRe.BackColor = System.Drawing.Color.Transparent;
            this.ChkGbRe.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ChkGbRe.ForeColor = System.Drawing.Color.Black;
            this.ChkGbRe.Location = new System.Drawing.Point(6, 20);
            this.ChkGbRe.Name = "ChkGbRe";
            this.ChkGbRe.Size = new System.Drawing.Size(62, 19);
            this.ChkGbRe.TabIndex = 161;
            this.ChkGbRe.Text = "재발행";
            this.ChkGbRe.UseVisualStyleBackColor = false;
            // 
            // panel21
            // 
            this.panel21.Controls.Add(this.groupBox8);
            this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel21.Location = new System.Drawing.Point(0, 50);
            this.panel21.Name = "panel21";
            this.panel21.Size = new System.Drawing.Size(228, 53);
            this.panel21.TabIndex = 4;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.lblLtdName);
            this.groupBox8.Controls.Add(this.btnLtdHelp);
            this.groupBox8.Controls.Add(this.txtLtdCode);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox8.Location = new System.Drawing.Point(0, 0);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(228, 53);
            this.groupBox8.TabIndex = 15;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "사업자명";
            // 
            // txtLtdCode
            // 
            this.txtLtdCode.Location = new System.Drawing.Point(5, 20);
            this.txtLtdCode.Name = "txtLtdCode";
            this.txtLtdCode.Size = new System.Drawing.Size(49, 25);
            this.txtLtdCode.TabIndex = 98;
            this.txtLtdCode.Tag = "LTDCODE";
            this.txtLtdCode.Text = "01234";
            this.txtLtdCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnLtdHelp
            // 
            this.btnLtdHelp.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLtdHelp.Location = new System.Drawing.Point(56, 20);
            this.btnLtdHelp.Name = "btnLtdHelp";
            this.btnLtdHelp.Size = new System.Drawing.Size(25, 25);
            this.btnLtdHelp.TabIndex = 99;
            this.btnLtdHelp.Text = "&H";
            this.btnLtdHelp.UseVisualStyleBackColor = true;
            // 
            // lblLtdName
            // 
            this.lblLtdName.BackColor = System.Drawing.Color.LightGray;
            this.lblLtdName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLtdName.Location = new System.Drawing.Point(87, 20);
            this.lblLtdName.Name = "lblLtdName";
            this.lblLtdName.Size = new System.Drawing.Size(137, 23);
            this.lblLtdName.TabIndex = 100;
            this.lblLtdName.Text = "현대제철";
            this.lblLtdName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel15
            // 
            this.panel15.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel15.Location = new System.Drawing.Point(748, 0);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(10, 100);
            this.panel15.TabIndex = 10;
            // 
            // panel18
            // 
            this.panel18.Controls.Add(this.groupBox3);
            this.panel18.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel18.Location = new System.Drawing.Point(758, 0);
            this.panel18.Name = "panel18";
            this.panel18.Size = new System.Drawing.Size(57, 100);
            this.panel18.TabIndex = 13;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdoCNT1);
            this.groupBox3.Controls.Add(this.rdoCNT2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(57, 100);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "매수";
            // 
            // rdoCNT2
            // 
            this.rdoCNT2.AutoSize = true;
            this.rdoCNT2.Location = new System.Drawing.Point(9, 49);
            this.rdoCNT2.Name = "rdoCNT2";
            this.rdoCNT2.Size = new System.Drawing.Size(46, 21);
            this.rdoCNT2.TabIndex = 3;
            this.rdoCNT2.Text = "2장";
            this.rdoCNT2.UseVisualStyleBackColor = true;
            // 
            // rdoCNT1
            // 
            this.rdoCNT1.AutoSize = true;
            this.rdoCNT1.Checked = true;
            this.rdoCNT1.Location = new System.Drawing.Point(9, 22);
            this.rdoCNT1.Name = "rdoCNT1";
            this.rdoCNT1.Size = new System.Drawing.Size(46, 21);
            this.rdoCNT1.TabIndex = 5;
            this.rdoCNT1.TabStop = true;
            this.rdoCNT1.Text = "1장";
            this.rdoCNT1.UseVisualStyleBackColor = true;
            // 
            // panel19
            // 
            this.panel19.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel19.Location = new System.Drawing.Point(815, 0);
            this.panel19.Name = "panel19";
            this.panel19.Size = new System.Drawing.Size(10, 100);
            this.panel19.TabIndex = 14;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(825, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(98, 100);
            this.panel4.TabIndex = 15;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnSearch);
            this.groupBox4.Controls.Add(this.btnPrint);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(98, 100);
            this.groupBox4.TabIndex = 20;
            this.groupBox4.TabStop = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(7, 59);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(91, 34);
            this.btnPrint.TabIndex = 14;
            this.btnPrint.Text = "인쇄";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(6, 19);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(91, 34);
            this.btnSearch.TabIndex = 15;
            this.btnSearch.Text = "조회";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(923, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(10, 100);
            this.panel5.TabIndex = 16;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.groupBox1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(933, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(105, 100);
            this.panel6.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnWebSearch);
            this.groupBox1.Controls.Add(this.btnWebPrint);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(105, 100);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "웹결과지";
            // 
            // btnWebPrint
            // 
            this.btnWebPrint.Location = new System.Drawing.Point(10, 59);
            this.btnWebPrint.Name = "btnWebPrint";
            this.btnWebPrint.Size = new System.Drawing.Size(88, 34);
            this.btnWebPrint.TabIndex = 7;
            this.btnWebPrint.Text = "전송";
            this.btnWebPrint.UseVisualStyleBackColor = true;
            // 
            // btnWebSearch
            // 
            this.btnWebSearch.Location = new System.Drawing.Point(10, 20);
            this.btnWebSearch.Name = "btnWebSearch";
            this.btnWebSearch.Size = new System.Drawing.Size(88, 34);
            this.btnWebSearch.TabIndex = 8;
            this.btnWebSearch.Text = "대상조회";
            this.btnWebSearch.UseVisualStyleBackColor = true;
            // 
            // panel17
            // 
            this.panel17.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel17.Location = new System.Drawing.Point(1038, 0);
            this.panel17.Name = "panel17";
            this.panel17.Size = new System.Drawing.Size(10, 100);
            this.panel17.TabIndex = 20;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 100);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1371, 10);
            this.panel3.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel9);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1371, 612);
            this.panel1.TabIndex = 3;
            // 
            // ChkAll
            // 
            this.ChkAll.AutoSize = true;
            this.ChkAll.BackColor = System.Drawing.Color.Transparent;
            this.ChkAll.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ChkAll.ForeColor = System.Drawing.Color.Black;
            this.ChkAll.Location = new System.Drawing.Point(44, 15);
            this.ChkAll.Name = "ChkAll";
            this.ChkAll.Size = new System.Drawing.Size(15, 14);
            this.ChkAll.TabIndex = 164;
            this.ChkAll.UseVisualStyleBackColor = false;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.ChkAll);
            this.panel9.Controls.Add(this.SSList);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(0, 110);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1371, 502);
            this.panel9.TabIndex = 2;
            // 
            // SSList_Sheet1
            // 
            this.SSList_Sheet1.Reset();
            this.SSList_Sheet1.SheetName = "Sheet1";
            // Formulas and custom names must be loaded with R1C1 reference style
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.R1C1;
            this.SSList_Sheet1.ColumnCount = 16;
            this.SSList_Sheet1.RowCount = 1;
            this.SSList_Sheet1.Cells.Get(0, 0).TextIndent = 4;
            this.SSList_Sheet1.Cells.Get(0, 1).TextIndent = 4;
            this.SSList_Sheet1.Cells.Get(0, 2).TextIndent = 4;
            this.SSList_Sheet1.Cells.Get(0, 3).TextIndent = 4;
            this.SSList_Sheet1.Cells.Get(0, 4).TextIndent = 4;
            this.SSList_Sheet1.Cells.Get(0, 5).TextIndent = 4;
            this.SSList_Sheet1.Cells.Get(0, 6).TextIndent = 4;
            this.SSList_Sheet1.Cells.Get(0, 7).TextIndent = 4;
            this.SSList_Sheet1.Cells.Get(0, 8).TextIndent = 4;
            this.SSList_Sheet1.Cells.Get(0, 9).TextIndent = 4;
            this.SSList_Sheet1.Cells.Get(0, 10).TextIndent = 4;
            this.SSList_Sheet1.Cells.Get(0, 11).TextIndent = 4;
            this.SSList_Sheet1.Cells.Get(0, 12).TextIndent = 4;
            this.SSList_Sheet1.Cells.Get(0, 13).TextIndent = 4;
            this.SSList_Sheet1.Cells.Get(0, 14).TextIndent = 4;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooter.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.Parent = "CornerFooterDefaultEnhanced";
            this.SSList_Sheet1.ColumnFooterSheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 1).Value = "성명";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 2).Value = "검진종류";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 3).Value = "회사명";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 4).Value = "검진일자";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 5).Value = "주민번호";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 6).Value = "웹결과지";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 7).Value = "주소";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 8).Value = "취급물질";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 9).Value = "접수번호";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 10).Value = "공단통보일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "통보일";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "판정일자";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 13).Value = "휴대폰";
            this.SSList_Sheet1.ColumnHeader.Cells.Get(0, 14).Value = "메일";
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.Parent = "ColumnHeaderDefaultEnhanced";
            this.SSList_Sheet1.ColumnHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ColumnHeader.Rows.Get(0).Height = 36F;
            this.SSList_Sheet1.Columns.Get(0).CellType = checkBoxCellType2;
            this.SSList_Sheet1.Columns.Get(0).HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(0).VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            this.SSList_Sheet1.Columns.Get(0).Width = 29F;
            this.SSList_Sheet1.Columns.Get(2).Label = "검진종류";
            this.SSList_Sheet1.Columns.Get(2).Width = 79F;
            this.SSList_Sheet1.Columns.Get(3).Label = "회사명";
            this.SSList_Sheet1.Columns.Get(3).Width = 96F;
            this.SSList_Sheet1.Columns.Get(4).Label = "검진일자";
            this.SSList_Sheet1.Columns.Get(4).Width = 100F;
            this.SSList_Sheet1.Columns.Get(5).Label = "주민번호";
            this.SSList_Sheet1.Columns.Get(5).Width = 146F;
            this.SSList_Sheet1.Columns.Get(6).Label = "웹결과지";
            this.SSList_Sheet1.Columns.Get(6).Width = 46F;
            this.SSList_Sheet1.Columns.Get(7).Label = "주소";
            this.SSList_Sheet1.Columns.Get(7).Width = 234F;
            this.SSList_Sheet1.Columns.Get(8).Label = "취급물질";
            this.SSList_Sheet1.Columns.Get(8).Width = 75F;
            this.SSList_Sheet1.Columns.Get(9).Label = "접수번호";
            this.SSList_Sheet1.Columns.Get(9).Width = 70F;
            this.SSList_Sheet1.Columns.Get(10).Label = "공단통보일";
            this.SSList_Sheet1.Columns.Get(10).Width = 75F;
            this.SSList_Sheet1.Columns.Get(11).Label = "통보일";
            this.SSList_Sheet1.Columns.Get(11).Width = 75F;
            this.SSList_Sheet1.Columns.Get(12).Label = "판정일자";
            this.SSList_Sheet1.Columns.Get(12).Width = 75F;
            this.SSList_Sheet1.Columns.Get(13).Label = "휴대폰";
            this.SSList_Sheet1.Columns.Get(13).Width = 100F;
            this.SSList_Sheet1.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.DefaultStyle.Parent = "DataAreaDefault";
            this.SSList_Sheet1.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FilterBar.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBar.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBar.DefaultStyle.Parent = "FilterBarDefaultEnhanced";
            this.SSList_Sheet1.FilterBar.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.FilterBarHeaderStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.FilterBarHeaderStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList_Sheet1.FilterBarHeaderStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.RowHeader.Columns.Default.Resizable = false;
            this.SSList_Sheet1.RowHeader.DefaultStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.RowHeader.DefaultStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.RowHeader.DefaultStyle.Parent = "RowHeaderDefaultEnhanced";
            this.SSList_Sheet1.RowHeader.DefaultStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.SheetCornerStyle.HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.General;
            this.SSList_Sheet1.SheetCornerStyle.NoteIndicatorColor = System.Drawing.Color.Red;
            this.SSList_Sheet1.SheetCornerStyle.Parent = "CornerDefaultEnhanced";
            this.SSList_Sheet1.SheetCornerStyle.VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.General;
            this.SSList_Sheet1.ReferenceStyle = FarPoint.Win.Spread.Model.ReferenceStyle.A1;
            // 
            // SSList
            // 
            this.SSList.AccessibleDescription = "SSList, Sheet1, Row 0, Column 0, ";
            this.SSList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SSList.Location = new System.Drawing.Point(0, 0);
            this.SSList.Name = "SSList";
            this.SSList.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.SSList_Sheet1});
            this.SSList.Size = new System.Drawing.Size(1371, 502);
            this.SSList.TabIndex = 1;
            // 
            // frmHcPrint_Cancer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1371, 637);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcPrint_Cancer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "암검진 결과통보서 인쇄(2021년)";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel20.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.panel14.ResumeLayout(false);
            this.panel16.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.panel21.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.panel18.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SSList_Sheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SSList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem menuPrint;
        private System.Windows.Forms.ToolStripMenuItem menuTongbo;
        private System.Windows.Forms.ToolStripMenuItem menuUnTongbo;
        private System.Windows.Forms.ToolStripMenuItem menuBal;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel17;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnWebSearch;
        private System.Windows.Forms.Button btnWebPrint;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panel19;
        private System.Windows.Forms.Panel panel18;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdoCNT1;
        private System.Windows.Forms.RadioButton rdoCNT2;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Panel panel16;
        private System.Windows.Forms.Panel panel21;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label lblLtdName;
        private System.Windows.Forms.Button btnLtdHelp;
        private System.Windows.Forms.TextBox txtLtdCode;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox ChkGbRe;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel20;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtWrtno;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.DateTimePicker dtpTongbo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpTDate;
        private System.Windows.Forms.DateTimePicker dtpFDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.CheckBox ChkAll;
        private FarPoint.Win.Spread.FpSpread SSList;
        private FarPoint.Win.Spread.SheetView SSList_Sheet1;
    }
}