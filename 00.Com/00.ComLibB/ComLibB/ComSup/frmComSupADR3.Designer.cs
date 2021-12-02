namespace ComLibB
{
    partial class frmComSupADR3
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
            this.panTitleSub = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.lblTemp = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSaveTemp = new System.Windows.Forms.Button();
            this.panADR1_1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panADR3_1 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnEqual = new System.Windows.Forms.Button();
            this.panADR2_1 = new System.Windows.Forms.Panel();
            this.panel13 = new System.Windows.Forms.Panel();
            this.dtpwdate = new System.Windows.Forms.DateTimePicker();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.txtwbuse = new System.Windows.Forms.TextBox();
            this.txtwname = new System.Windows.Forms.TextBox();
            this.txtwSabun = new System.Windows.Forms.TextBox();
            this.panTitleSub.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel13.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitleSub
            // 
            this.panTitleSub.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub.Controls.Add(this.lblTitleSub0);
            this.panTitleSub.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub.Location = new System.Drawing.Point(0, 0);
            this.panTitleSub.Name = "panTitleSub";
            this.panTitleSub.Size = new System.Drawing.Size(1084, 34);
            this.panTitleSub.TabIndex = 19;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(5, 5);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(274, 21);
            this.lblTitleSub0.TabIndex = 4;
            this.lblTitleSub0.Text = "약물이상반응(ADR) 인과성 평가 2차";
            // 
            // lblTemp
            // 
            this.lblTemp.BackColor = System.Drawing.Color.Red;
            this.lblTemp.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTemp.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.lblTemp.Location = new System.Drawing.Point(0, 68);
            this.lblTemp.Name = "lblTemp";
            this.lblTemp.Size = new System.Drawing.Size(1084, 20);
            this.lblTemp.TabIndex = 25;
            this.lblTemp.Text = "임시저장된 보고서 입니다. 보고서를 제출하시려면 \'저장\' 버튼을 클릭하시기 바랍니다!";
            this.lblTemp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTemp.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnSaveTemp);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1084, 34);
            this.panel1.TabIndex = 24;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(992, 2);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(90, 30);
            this.btnExit.TabIndex = 0;
            this.btnExit.Text = "닫  기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(903, 2);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(90, 30);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "출  력";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(814, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 30);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = "삭  제";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(725, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 30);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "저  장";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveTemp
            // 
            this.btnSaveTemp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveTemp.Location = new System.Drawing.Point(636, 2);
            this.btnSaveTemp.Name = "btnSaveTemp";
            this.btnSaveTemp.Size = new System.Drawing.Size(90, 30);
            this.btnSaveTemp.TabIndex = 0;
            this.btnSaveTemp.Text = "임시저장";
            this.btnSaveTemp.UseVisualStyleBackColor = true;
            this.btnSaveTemp.Click += new System.EventHandler(this.btnSaveTemp_Click);
            // 
            // panADR1_1
            // 
            this.panADR1_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panADR1_1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panADR1_1.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.panADR1_1.Location = new System.Drawing.Point(0, 88);
            this.panADR1_1.Name = "panADR1_1";
            this.panADR1_1.Size = new System.Drawing.Size(1084, 382);
            this.panADR1_1.TabIndex = 26;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 470);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1084, 5);
            this.panel2.TabIndex = 27;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panADR3_1);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.panADR2_1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 475);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1084, 172);
            this.panel3.TabIndex = 28;
            // 
            // panADR3_1
            // 
            this.panADR3_1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panADR3_1.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.panADR3_1.Location = new System.Drawing.Point(563, 0);
            this.panADR3_1.Name = "panADR3_1";
            this.panADR3_1.Size = new System.Drawing.Size(519, 172);
            this.panADR3_1.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnEqual);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.panel5.Location = new System.Drawing.Point(519, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(44, 172);
            this.panel5.TabIndex = 1;
            // 
            // btnEqual
            // 
            this.btnEqual.Location = new System.Drawing.Point(1, 1);
            this.btnEqual.Name = "btnEqual";
            this.btnEqual.Size = new System.Drawing.Size(42, 170);
            this.btnEqual.TabIndex = 0;
            this.btnEqual.Text = "1차\r\n평가\r\n와\r\n동일";
            this.btnEqual.UseVisualStyleBackColor = true;
            this.btnEqual.Click += new System.EventHandler(this.btnEqual_Click);
            // 
            // panADR2_1
            // 
            this.panADR2_1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panADR2_1.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.panADR2_1.Location = new System.Drawing.Point(0, 0);
            this.panADR2_1.Name = "panADR2_1";
            this.panADR2_1.Size = new System.Drawing.Size(519, 172);
            this.panADR2_1.TabIndex = 0;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.txtwSabun);
            this.panel13.Controls.Add(this.dtpwdate);
            this.panel13.Controls.Add(this.label27);
            this.panel13.Controls.Add(this.label26);
            this.panel13.Controls.Add(this.txtwbuse);
            this.panel13.Controls.Add(this.txtwname);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel13.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.panel13.Location = new System.Drawing.Point(0, 647);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(1084, 30);
            this.panel13.TabIndex = 40;
            // 
            // dtpwdate
            // 
            this.dtpwdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpwdate.Location = new System.Drawing.Point(578, 3);
            this.dtpwdate.Name = "dtpwdate";
            this.dtpwdate.Size = new System.Drawing.Size(105, 23);
            this.dtpwdate.TabIndex = 30;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(687, 7);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(67, 15);
            this.label27.TabIndex = 0;
            this.label27.Text = "작성자사번";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(523, 7);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(55, 15);
            this.label26.TabIndex = 0;
            this.label26.Text = "작성일자";
            // 
            // txtwbuse
            // 
            this.txtwbuse.Location = new System.Drawing.Point(909, 3);
            this.txtwbuse.Name = "txtwbuse";
            this.txtwbuse.Size = new System.Drawing.Size(150, 23);
            this.txtwbuse.TabIndex = 29;
            // 
            // txtwname
            // 
            this.txtwname.Enabled = false;
            this.txtwname.Location = new System.Drawing.Point(829, 3);
            this.txtwname.Name = "txtwname";
            this.txtwname.Size = new System.Drawing.Size(80, 23);
            this.txtwname.TabIndex = 29;
            this.txtwname.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtwname_KeyDown);
            this.txtwname.Leave += new System.EventHandler(this.txtwname_Leave);
            // 
            // txtwSabun
            // 
            this.txtwSabun.Location = new System.Drawing.Point(753, 3);
            this.txtwSabun.Name = "txtwSabun";
            this.txtwSabun.Size = new System.Drawing.Size(73, 23);
            this.txtwSabun.TabIndex = 136;
            this.txtwSabun.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtwSabun_KeyDown);
            // 
            // frmComSupADR3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1084, 677);
            this.ControlBox = false;
            this.Controls.Add(this.panADR1_1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.lblTemp);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub);
            this.Controls.Add(this.panel13);
            this.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmComSupADR3";
            this.Text = "약물이상반응(ADR) 인과성 평가 2차";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmComSupADR3_Load);
            this.panTitleSub.ResumeLayout(false);
            this.panTitleSub.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitleSub;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Label lblTemp;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSaveTemp;
        private System.Windows.Forms.Panel panADR1_1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.DateTimePicker dtpwdate;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox txtwbuse;
        private System.Windows.Forms.TextBox txtwname;
        private System.Windows.Forms.Panel panADR3_1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panADR2_1;
        private System.Windows.Forms.Button btnEqual;
        private System.Windows.Forms.TextBox txtwSabun;
    }
}