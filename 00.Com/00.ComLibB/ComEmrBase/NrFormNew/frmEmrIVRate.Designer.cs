namespace ComEmrBase
{
    partial class frmEmrIVRate
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdoCalc2 = new System.Windows.Forms.RadioButton();
            this.rdoCalc1 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panDose = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtConcentration2_2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboConcentration2 = new System.Windows.Forms.ComboBox();
            this.txtDose2 = new System.Windows.Forms.TextBox();
            this.txtConcentration2_1 = new System.Windows.Forms.TextBox();
            this.txtWeight2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cboWeight2 = new System.Windows.Forms.ComboBox();
            this.btnCalc2 = new System.Windows.Forms.Button();
            this.chkWeight2 = new System.Windows.Forms.CheckBox();
            this.cboDoseTime2 = new System.Windows.Forms.ComboBox();
            this.panIV = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtConcentration1_2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboConcentration = new System.Windows.Forms.ComboBox();
            this.txtDose = new System.Windows.Forms.TextBox();
            this.txtConcentration1_1 = new System.Windows.Forms.TextBox();
            this.cboDose = new System.Windows.Forms.ComboBox();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.cboWeight = new System.Windows.Forms.ComboBox();
            this.btnCalc = new System.Windows.Forms.Button();
            this.lblDose = new System.Windows.Forms.Label();
            this.chkWeight = new System.Windows.Forms.CheckBox();
            this.cboDoseTime = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cboDoses = new System.Windows.Forms.ComboBox();
            this.lblKg = new System.Windows.Forms.Label();
            this.cboDoses2 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtIVRate = new System.Windows.Forms.TextBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panDose.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panIV.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.rdoCalc2);
            this.panel1.Controls.Add(this.rdoCalc1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(428, 32);
            this.panel1.TabIndex = 0;
            // 
            // rdoCalc2
            // 
            this.rdoCalc2.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoCalc2.AutoSize = true;
            this.rdoCalc2.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoCalc2.Location = new System.Drawing.Point(85, 0);
            this.rdoCalc2.Name = "rdoCalc2";
            this.rdoCalc2.Size = new System.Drawing.Size(74, 32);
            this.rdoCalc2.TabIndex = 1;
            this.rdoCalc2.TabStop = true;
            this.rdoCalc2.Text = "Dose Calc";
            this.rdoCalc2.UseVisualStyleBackColor = true;
            this.rdoCalc2.CheckedChanged += new System.EventHandler(this.rdoCalc1_CheckedChanged);
            // 
            // rdoCalc1
            // 
            this.rdoCalc1.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoCalc1.AutoSize = true;
            this.rdoCalc1.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoCalc1.Location = new System.Drawing.Point(0, 0);
            this.rdoCalc1.Name = "rdoCalc1";
            this.rdoCalc1.Size = new System.Drawing.Size(85, 32);
            this.rdoCalc1.TabIndex = 0;
            this.rdoCalc1.TabStop = true;
            this.rdoCalc1.Text = "IV Rate Calc";
            this.rdoCalc1.UseVisualStyleBackColor = true;
            this.rdoCalc1.CheckedChanged += new System.EventHandler(this.rdoCalc1_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.Controls.Add(this.panDose);
            this.panel2.Controls.Add(this.panIV);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 32);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(428, 169);
            this.panel2.TabIndex = 1;
            // 
            // panDose
            // 
            this.panDose.BackColor = System.Drawing.Color.Gainsboro;
            this.panDose.Controls.Add(this.panel5);
            this.panDose.Dock = System.Windows.Forms.DockStyle.Left;
            this.panDose.Location = new System.Drawing.Point(414, 0);
            this.panDose.Name = "panDose";
            this.panDose.Size = new System.Drawing.Size(431, 169);
            this.panDose.TabIndex = 16;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.Gainsboro;
            this.panel5.Controls.Add(this.label8);
            this.panel5.Controls.Add(this.label9);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.txtConcentration2_2);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.cboConcentration2);
            this.panel5.Controls.Add(this.txtDose2);
            this.panel5.Controls.Add(this.txtConcentration2_1);
            this.panel5.Controls.Add(this.txtWeight2);
            this.panel5.Controls.Add(this.button1);
            this.panel5.Controls.Add(this.cboWeight2);
            this.panel5.Controls.Add(this.btnCalc2);
            this.panel5.Controls.Add(this.chkWeight2);
            this.panel5.Controls.Add(this.cboDoseTime2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(409, 169);
            this.panel5.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림체", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(225, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 14);
            this.label3.TabIndex = 30;
            this.label3.Text = "cc";
            // 
            // txtConcentration2_2
            // 
            this.txtConcentration2_2.Location = new System.Drawing.Point(165, 103);
            this.txtConcentration2_2.Name = "txtConcentration2_2";
            this.txtConcentration2_2.Size = new System.Drawing.Size(57, 21);
            this.txtConcentration2_2.TabIndex = 29;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림체", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(55, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 14);
            this.label5.TabIndex = 16;
            this.label5.Text = "IV Rate :";
            // 
            // cboConcentration2
            // 
            this.cboConcentration2.BackColor = System.Drawing.Color.White;
            this.cboConcentration2.FormattingEnabled = true;
            this.cboConcentration2.Items.AddRange(new object[] {
            "mcg",
            "mg",
            "grams",
            "units",
            "nanograms"});
            this.cboConcentration2.Location = new System.Drawing.Point(228, 76);
            this.cboConcentration2.Name = "cboConcentration2";
            this.cboConcentration2.Size = new System.Drawing.Size(86, 20);
            this.cboConcentration2.TabIndex = 24;
            this.cboConcentration2.Text = "mg";
            this.cboConcentration2.SelectedIndexChanged += new System.EventHandler(this.cboDoses_SelectedIndexChanged);
            // 
            // txtDose2
            // 
            this.txtDose2.Location = new System.Drawing.Point(142, 8);
            this.txtDose2.Name = "txtDose2";
            this.txtDose2.Size = new System.Drawing.Size(80, 21);
            this.txtDose2.TabIndex = 17;
            // 
            // txtConcentration2_1
            // 
            this.txtConcentration2_1.Location = new System.Drawing.Point(142, 76);
            this.txtConcentration2_1.Name = "txtConcentration2_1";
            this.txtConcentration2_1.Size = new System.Drawing.Size(80, 21);
            this.txtConcentration2_1.TabIndex = 27;
            // 
            // txtWeight2
            // 
            this.txtWeight2.Location = new System.Drawing.Point(142, 43);
            this.txtWeight2.Name = "txtWeight2";
            this.txtWeight2.Size = new System.Drawing.Size(80, 21);
            this.txtWeight2.TabIndex = 23;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(212, 130);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 31);
            this.button1.TabIndex = 28;
            this.button1.Text = "초기화";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cboWeight2
            // 
            this.cboWeight2.Enabled = false;
            this.cboWeight2.FormattingEnabled = true;
            this.cboWeight2.Items.AddRange(new object[] {
            "grams",
            "kg",
            "lbs"});
            this.cboWeight2.Location = new System.Drawing.Point(228, 43);
            this.cboWeight2.Name = "cboWeight2";
            this.cboWeight2.Size = new System.Drawing.Size(86, 20);
            this.cboWeight2.TabIndex = 19;
            this.cboWeight2.Text = "kg";
            // 
            // btnCalc2
            // 
            this.btnCalc2.BackColor = System.Drawing.Color.White;
            this.btnCalc2.Location = new System.Drawing.Point(104, 130);
            this.btnCalc2.Name = "btnCalc2";
            this.btnCalc2.Size = new System.Drawing.Size(102, 31);
            this.btnCalc2.TabIndex = 26;
            this.btnCalc2.Text = "계산하기";
            this.btnCalc2.UseVisualStyleBackColor = false;
            this.btnCalc2.Click += new System.EventHandler(this.btnCalc2_Click);
            // 
            // chkWeight2
            // 
            this.chkWeight2.AutoSize = true;
            this.chkWeight2.Checked = true;
            this.chkWeight2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWeight2.Font = new System.Drawing.Font("굴림체", 10F, System.Drawing.FontStyle.Bold);
            this.chkWeight2.Location = new System.Drawing.Point(47, 43);
            this.chkWeight2.Name = "chkWeight2";
            this.chkWeight2.Size = new System.Drawing.Size(90, 18);
            this.chkWeight2.TabIndex = 22;
            this.chkWeight2.Text = "Weight :";
            this.chkWeight2.UseVisualStyleBackColor = true;
            this.chkWeight2.CheckedChanged += new System.EventHandler(this.chkWeight2_CheckedChanged);
            // 
            // cboDoseTime2
            // 
            this.cboDoseTime2.FormattingEnabled = true;
            this.cboDoseTime2.Items.AddRange(new object[] {
            "cc/min",
            "cc/hr",
            "cc/day"});
            this.cboDoseTime2.Location = new System.Drawing.Point(228, 9);
            this.cboDoseTime2.Name = "cboDoseTime2";
            this.cboDoseTime2.Size = new System.Drawing.Size(86, 20);
            this.cboDoseTime2.TabIndex = 21;
            this.cboDoseTime2.Text = "cc/hr";
            this.cboDoseTime2.SelectedIndexChanged += new System.EventHandler(this.cboDoses_SelectedIndexChanged);
            // 
            // panIV
            // 
            this.panIV.BackColor = System.Drawing.Color.Gainsboro;
            this.panIV.Controls.Add(this.panel4);
            this.panIV.Dock = System.Windows.Forms.DockStyle.Left;
            this.panIV.Location = new System.Drawing.Point(0, 0);
            this.panIV.Name = "panIV";
            this.panIV.Size = new System.Drawing.Size(414, 169);
            this.panIV.TabIndex = 15;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Gainsboro;
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.txtConcentration1_2);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.cboConcentration);
            this.panel4.Controls.Add(this.txtDose);
            this.panel4.Controls.Add(this.txtConcentration1_1);
            this.panel4.Controls.Add(this.cboDose);
            this.panel4.Controls.Add(this.txtWeight);
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.btnClear);
            this.panel4.Controls.Add(this.cboWeight);
            this.panel4.Controls.Add(this.btnCalc);
            this.panel4.Controls.Add(this.lblDose);
            this.panel4.Controls.Add(this.chkWeight);
            this.panel4.Controls.Add(this.cboDoseTime);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(414, 169);
            this.panel4.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림체", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(224, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 14);
            this.label2.TabIndex = 15;
            this.label2.Text = "cc";
            // 
            // txtConcentration1_2
            // 
            this.txtConcentration1_2.Location = new System.Drawing.Point(164, 103);
            this.txtConcentration1_2.Name = "txtConcentration1_2";
            this.txtConcentration1_2.Size = new System.Drawing.Size(57, 21);
            this.txtConcentration1_2.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림체", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(80, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Dose :";
            // 
            // cboConcentration
            // 
            this.cboConcentration.BackColor = System.Drawing.Color.White;
            this.cboConcentration.FormattingEnabled = true;
            this.cboConcentration.Items.AddRange(new object[] {
            "mcg",
            "mg",
            "grams",
            "units",
            "nanograms"});
            this.cboConcentration.Location = new System.Drawing.Point(227, 76);
            this.cboConcentration.Name = "cboConcentration";
            this.cboConcentration.Size = new System.Drawing.Size(86, 20);
            this.cboConcentration.TabIndex = 12;
            this.cboConcentration.Text = "mg";
            this.cboConcentration.SelectedIndexChanged += new System.EventHandler(this.cboDose_SelectedIndexChanged);
            // 
            // txtDose
            // 
            this.txtDose.Location = new System.Drawing.Point(141, 8);
            this.txtDose.Name = "txtDose";
            this.txtDose.Size = new System.Drawing.Size(80, 21);
            this.txtDose.TabIndex = 1;
            // 
            // txtConcentration1_1
            // 
            this.txtConcentration1_1.Location = new System.Drawing.Point(141, 76);
            this.txtConcentration1_1.Name = "txtConcentration1_1";
            this.txtConcentration1_1.Size = new System.Drawing.Size(80, 21);
            this.txtConcentration1_1.TabIndex = 13;
            // 
            // cboDose
            // 
            this.cboDose.FormattingEnabled = true;
            this.cboDose.Items.AddRange(new object[] {
            "ng",
            "mcg",
            "mg",
            "grams"});
            this.cboDose.Location = new System.Drawing.Point(227, 8);
            this.cboDose.Name = "cboDose";
            this.cboDose.Size = new System.Drawing.Size(80, 20);
            this.cboDose.TabIndex = 2;
            this.cboDose.Text = "mcg";
            this.cboDose.SelectedIndexChanged += new System.EventHandler(this.cboDose_SelectedIndexChanged);
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(141, 43);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(80, 21);
            this.txtWeight.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림체", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(82, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 14);
            this.label7.TabIndex = 12;
            this.label7.Text = "용량 :";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(211, 130);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(102, 31);
            this.btnClear.TabIndex = 13;
            this.btnClear.Text = "초기화";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cboWeight
            // 
            this.cboWeight.Enabled = false;
            this.cboWeight.FormattingEnabled = true;
            this.cboWeight.Items.AddRange(new object[] {
            "grams",
            "kg",
            "lbs"});
            this.cboWeight.Location = new System.Drawing.Point(227, 43);
            this.cboWeight.Name = "cboWeight";
            this.cboWeight.Size = new System.Drawing.Size(80, 20);
            this.cboWeight.TabIndex = 2;
            this.cboWeight.Text = "kg";
            this.cboWeight.SelectedIndexChanged += new System.EventHandler(this.cboDose_SelectedIndexChanged);
            // 
            // btnCalc
            // 
            this.btnCalc.BackColor = System.Drawing.Color.White;
            this.btnCalc.Location = new System.Drawing.Point(103, 130);
            this.btnCalc.Name = "btnCalc";
            this.btnCalc.Size = new System.Drawing.Size(102, 31);
            this.btnCalc.TabIndex = 12;
            this.btnCalc.Text = "계산하기";
            this.btnCalc.UseVisualStyleBackColor = false;
            this.btnCalc.Click += new System.EventHandler(this.btnCalc_Click);
            // 
            // lblDose
            // 
            this.lblDose.AutoSize = true;
            this.lblDose.Font = new System.Drawing.Font("굴림체", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDose.Location = new System.Drawing.Point(313, 11);
            this.lblDose.Name = "lblDose";
            this.lblDose.Size = new System.Drawing.Size(39, 14);
            this.lblDose.TabIndex = 3;
            this.lblDose.Text = "/kg/";
            // 
            // chkWeight
            // 
            this.chkWeight.AutoSize = true;
            this.chkWeight.Checked = true;
            this.chkWeight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWeight.Font = new System.Drawing.Font("굴림체", 10F, System.Drawing.FontStyle.Bold);
            this.chkWeight.Location = new System.Drawing.Point(48, 43);
            this.chkWeight.Name = "chkWeight";
            this.chkWeight.Size = new System.Drawing.Size(90, 18);
            this.chkWeight.TabIndex = 5;
            this.chkWeight.Text = "Weight :";
            this.chkWeight.UseVisualStyleBackColor = true;
            this.chkWeight.CheckedChanged += new System.EventHandler(this.chkWeight_CheckedChanged);
            // 
            // cboDoseTime
            // 
            this.cboDoseTime.FormattingEnabled = true;
            this.cboDoseTime.Items.AddRange(new object[] {
            "min",
            "hr"});
            this.cboDoseTime.Location = new System.Drawing.Point(358, 8);
            this.cboDoseTime.Name = "cboDoseTime";
            this.cboDoseTime.Size = new System.Drawing.Size(46, 20);
            this.cboDoseTime.TabIndex = 4;
            this.cboDoseTime.Text = "min";
            this.cboDoseTime.SelectedIndexChanged += new System.EventHandler(this.cboDose_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.cboDoses);
            this.panel3.Controls.Add(this.lblKg);
            this.panel3.Controls.Add(this.cboDoses2);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.txtIVRate);
            this.panel3.Controls.Add(this.lblInfo);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 201);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(428, 32);
            this.panel3.TabIndex = 2;
            // 
            // cboDoses
            // 
            this.cboDoses.FormattingEnabled = true;
            this.cboDoses.Items.AddRange(new object[] {
            "ng",
            "mcg",
            "mg",
            "grams"});
            this.cboDoses.Location = new System.Drawing.Point(244, 5);
            this.cboDoses.Name = "cboDoses";
            this.cboDoses.Size = new System.Drawing.Size(80, 20);
            this.cboDoses.TabIndex = 16;
            this.cboDoses.Text = "mcg";
            this.cboDoses.SelectedIndexChanged += new System.EventHandler(this.cboDoses_SelectedIndexChanged);
            // 
            // lblKg
            // 
            this.lblKg.AutoSize = true;
            this.lblKg.Font = new System.Drawing.Font("굴림체", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblKg.Location = new System.Drawing.Point(330, 8);
            this.lblKg.Name = "lblKg";
            this.lblKg.Size = new System.Drawing.Size(39, 14);
            this.lblKg.TabIndex = 17;
            this.lblKg.Text = "/kg/";
            // 
            // cboDoses2
            // 
            this.cboDoses2.FormattingEnabled = true;
            this.cboDoses2.Items.AddRange(new object[] {
            "min",
            "hr"});
            this.cboDoses2.Location = new System.Drawing.Point(375, 5);
            this.cboDoses2.Name = "cboDoses2";
            this.cboDoses2.Size = new System.Drawing.Size(46, 20);
            this.cboDoses2.TabIndex = 18;
            this.cboDoses2.Text = "min";
            this.cboDoses2.SelectedIndexChanged += new System.EventHandler(this.cboDoses_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림체", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(241, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 14);
            this.label6.TabIndex = 15;
            this.label6.Text = "cc/hr";
            // 
            // txtIVRate
            // 
            this.txtIVRate.BackColor = System.Drawing.Color.White;
            this.txtIVRate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtIVRate.Font = new System.Drawing.Font("굴림체", 10F, System.Drawing.FontStyle.Bold);
            this.txtIVRate.Location = new System.Drawing.Point(112, 6);
            this.txtIVRate.Name = "txtIVRate";
            this.txtIVRate.ReadOnly = true;
            this.txtIVRate.Size = new System.Drawing.Size(123, 16);
            this.txtIVRate.TabIndex = 13;
            this.txtIVRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("굴림체", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblInfo.Location = new System.Drawing.Point(27, 8);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(79, 14);
            this.lblInfo.TabIndex = 10;
            this.lblInfo.Text = "IV Rate :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림체", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(64, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 14);
            this.label4.TabIndex = 16;
            this.label4.Text = "Volume : ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림체", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(63, 105);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 14);
            this.label8.TabIndex = 32;
            this.label8.Text = "Volume : ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("굴림체", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(81, 78);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 14);
            this.label9.TabIndex = 31;
            this.label9.Text = "용량 :";
            // 
            // frmEmrIVRate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 233);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmEmrIVRate";
            this.Text = "frmEmrIVRate";
            this.Load += new System.EventHandler(this.frmEmrIVRate_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panDose.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panIV.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rdoCalc1;
        private System.Windows.Forms.RadioButton rdoCalc2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDose;
        private System.Windows.Forms.ComboBox cboDose;
        private System.Windows.Forms.Label lblDose;
        private System.Windows.Forms.ComboBox cboDoseTime;
        private System.Windows.Forms.CheckBox chkWeight;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.ComboBox cboWeight;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.TextBox txtIVRate;
        private System.Windows.Forms.ComboBox cboConcentration;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnCalc;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panIV;
        private System.Windows.Forms.Panel panDose;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtConcentration1_2;
        private System.Windows.Forms.TextBox txtConcentration1_1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtConcentration2_2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboConcentration2;
        private System.Windows.Forms.TextBox txtDose2;
        private System.Windows.Forms.TextBox txtConcentration2_1;
        private System.Windows.Forms.TextBox txtWeight2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cboWeight2;
        private System.Windows.Forms.Button btnCalc2;
        private System.Windows.Forms.CheckBox chkWeight2;
        private System.Windows.Forms.ComboBox cboDoseTime2;
        private System.Windows.Forms.ComboBox cboDoses;
        private System.Windows.Forms.Label lblKg;
        private System.Windows.Forms.ComboBox cboDoses2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
    }
}