namespace ComEmrBase
{
    partial class frmGFRCalc
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
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.txtMDRD = new System.Windows.Forms.TextBox();
            this.txtCockcroft = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.txtCreatinine = new System.Windows.Forms.TextBox();
            this.txtIBW = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdoSex2 = new System.Windows.Forms.RadioButton();
            this.rdoSex1 = new System.Windows.Forms.RadioButton();
            this.txtAge = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.btnCal = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label28);
            this.panel1.Controls.Add(this.label27);
            this.panel1.Controls.Add(this.label26);
            this.panel1.Controls.Add(this.txtMDRD);
            this.panel1.Controls.Add(this.txtCockcroft);
            this.panel1.Controls.Add(this.label25);
            this.panel1.Controls.Add(this.label24);
            this.panel1.Controls.Add(this.txtCreatinine);
            this.panel1.Controls.Add(this.txtIBW);
            this.panel1.Controls.Add(this.label23);
            this.panel1.Controls.Add(this.txtWeight);
            this.panel1.Controls.Add(this.label22);
            this.panel1.Controls.Add(this.txtHeight);
            this.panel1.Controls.Add(this.label21);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.txtAge);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.btnCal);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(449, 322);
            this.panel1.TabIndex = 0;
            // 
            // label28
            // 
            this.label28.BackColor = System.Drawing.Color.White;
            this.label28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label28.Location = new System.Drawing.Point(7, 288);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(435, 22);
            this.label28.TabIndex = 78;
            this.label28.Text = "공식:((140 - 나이) * IBW(몸무게)) / (72 * 크레이티닌) 여자면 * 0.85";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.BackColor = System.Drawing.Color.White;
            this.label27.Location = new System.Drawing.Point(145, 248);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(46, 12);
            this.label27.TabIndex = 77;
            this.label27.Text = "ml/min";
            this.label27.Visible = false;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.BackColor = System.Drawing.Color.White;
            this.label26.Location = new System.Drawing.Point(145, 212);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(46, 12);
            this.label26.TabIndex = 76;
            this.label26.Text = "ml/min";
            // 
            // txtMDRD
            // 
            this.txtMDRD.Location = new System.Drawing.Point(78, 244);
            this.txtMDRD.Name = "txtMDRD";
            this.txtMDRD.Size = new System.Drawing.Size(61, 21);
            this.txtMDRD.TabIndex = 75;
            this.txtMDRD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMDRD.Visible = false;
            // 
            // txtCockcroft
            // 
            this.txtCockcroft.Location = new System.Drawing.Point(78, 208);
            this.txtCockcroft.Name = "txtCockcroft";
            this.txtCockcroft.Size = new System.Drawing.Size(61, 21);
            this.txtCockcroft.TabIndex = 74;
            this.txtCockcroft.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.White;
            this.label25.Location = new System.Drawing.Point(26, 253);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(40, 12);
            this.label25.TabIndex = 73;
            this.label25.Text = "MDRD";
            this.label25.Visible = false;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.White;
            this.label24.Location = new System.Drawing.Point(8, 211);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(58, 12);
            this.label24.TabIndex = 72;
            this.label24.Text = "Cockcroft";
            // 
            // txtCreatinine
            // 
            this.txtCreatinine.Location = new System.Drawing.Point(75, 94);
            this.txtCreatinine.Name = "txtCreatinine";
            this.txtCreatinine.Size = new System.Drawing.Size(43, 21);
            this.txtCreatinine.TabIndex = 71;
            this.txtCreatinine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtIBW
            // 
            this.txtIBW.Location = new System.Drawing.Point(75, 59);
            this.txtIBW.Name = "txtIBW";
            this.txtIBW.Size = new System.Drawing.Size(43, 21);
            this.txtIBW.TabIndex = 70;
            this.txtIBW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.White;
            this.label23.Location = new System.Drawing.Point(122, 64);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(20, 12);
            this.label23.TabIndex = 69;
            this.label23.Text = "Kg";
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(211, 32);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(43, 21);
            this.txtWeight.TabIndex = 68;
            this.txtWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.White;
            this.label22.Location = new System.Drawing.Point(258, 37);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(20, 12);
            this.label22.TabIndex = 67;
            this.label22.Text = "Kg";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(75, 32);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(43, 21);
            this.txtHeight.TabIndex = 66;
            this.txtHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.BackColor = System.Drawing.Color.White;
            this.label21.Location = new System.Drawing.Point(123, 11);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(11, 12);
            this.label21.TabIndex = 65;
            this.label21.Text = "/";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.rdoSex2);
            this.panel2.Controls.Add(this.rdoSex1);
            this.panel2.Location = new System.Drawing.Point(140, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(140, 24);
            this.panel2.TabIndex = 64;
            // 
            // rdoSex2
            // 
            this.rdoSex2.AutoSize = true;
            this.rdoSex2.BackColor = System.Drawing.Color.White;
            this.rdoSex2.Location = new System.Drawing.Point(68, 4);
            this.rdoSex2.Name = "rdoSex2";
            this.rdoSex2.Size = new System.Drawing.Size(65, 16);
            this.rdoSex2.TabIndex = 1;
            this.rdoSex2.TabStop = true;
            this.rdoSex2.Text = "Female";
            this.rdoSex2.UseVisualStyleBackColor = false;
            // 
            // rdoSex1
            // 
            this.rdoSex1.AutoSize = true;
            this.rdoSex1.BackColor = System.Drawing.Color.White;
            this.rdoSex1.Location = new System.Drawing.Point(6, 4);
            this.rdoSex1.Name = "rdoSex1";
            this.rdoSex1.Size = new System.Drawing.Size(51, 16);
            this.rdoSex1.TabIndex = 0;
            this.rdoSex1.TabStop = true;
            this.rdoSex1.Text = "Male";
            this.rdoSex1.UseVisualStyleBackColor = false;
            // 
            // txtAge
            // 
            this.txtAge.Location = new System.Drawing.Point(75, 6);
            this.txtAge.Name = "txtAge";
            this.txtAge.Size = new System.Drawing.Size(43, 21);
            this.txtAge.TabIndex = 63;
            this.txtAge.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.RoyalBlue;
            this.label18.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label18.ForeColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(7, 170);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(277, 21);
            this.label18.TabIndex = 60;
            this.label18.Text = "계산 결과";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(152, 37);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(53, 12);
            this.label17.TabIndex = 59;
            this.label17.Text = "실제체중";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(122, 37);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(25, 12);
            this.label16.TabIndex = 58;
            this.label16.Text = "Cm";
            // 
            // btnCal
            // 
            this.btnCal.BackColor = System.Drawing.Color.White;
            this.btnCal.Location = new System.Drawing.Point(7, 121);
            this.btnCal.Name = "btnCal";
            this.btnCal.Size = new System.Drawing.Size(277, 36);
            this.btnCal.TabIndex = 57;
            this.btnCal.Text = "계산";
            this.btnCal.UseVisualStyleBackColor = false;
            this.btnCal.Click += new System.EventHandler(this.btnCal_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(9, 99);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(62, 12);
            this.label15.TabIndex = 56;
            this.label15.Text = "Creatinine";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(45, 66);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(26, 12);
            this.label14.TabIndex = 55;
            this.label14.Text = "IBW";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(42, 37);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 54;
            this.label13.Text = "신장";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(12, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 12);
            this.label12.TabIndex = 53;
            this.label12.Text = "나이/성별";
            // 
            // frmGFRCalc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 322);
            this.Controls.Add(this.panel1);
            this.Name = "frmGFRCalc";
            this.Text = "frmGFRCalc";
            this.Load += new System.EventHandler(this.frmGFRCalc_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox txtMDRD;
        private System.Windows.Forms.TextBox txtCockcroft;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox txtCreatinine;
        private System.Windows.Forms.TextBox txtIBW;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rdoSex2;
        private System.Windows.Forms.RadioButton rdoSex1;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnCal;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
    }
}