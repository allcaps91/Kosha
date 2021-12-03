namespace ComLibB
{
    partial class frmOcsCpSet
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.panTitleSub0 = new System.Windows.Forms.Panel();
            this.lblTitleSub0 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblPatInfo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboCpCode = new System.Windows.Forms.ComboBox();
            this.btnCPActive = new System.Windows.Forms.Button();
            this.btnCPWarm = new System.Windows.Forms.Button();
            this.btnCPDeActive = new System.Windows.Forms.Button();
            this.btnCPAct = new System.Windows.Forms.Button();
            this.lblWarm = new System.Windows.Forms.Label();
            this.lblActive = new System.Windows.Forms.Label();
            this.lblDeActive = new System.Windows.Forms.Label();
            this.lblAct = new System.Windows.Forms.Label();
            this.btnCPNew = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cboCpSayu = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panTitle.SuspendLayout();
            this.panTitleSub0.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.Color.White;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(710, 34);
            this.panTitle.TabIndex = 12;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(5, 5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(186, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "응급실 CP 대상등록관리";
            // 
            // panTitleSub0
            // 
            this.panTitleSub0.BackColor = System.Drawing.Color.RoyalBlue;
            this.panTitleSub0.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTitleSub0.Controls.Add(this.lblTitleSub0);
            this.panTitleSub0.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitleSub0.Location = new System.Drawing.Point(0, 34);
            this.panTitleSub0.Name = "panTitleSub0";
            this.panTitleSub0.Size = new System.Drawing.Size(710, 28);
            this.panTitleSub0.TabIndex = 13;
            // 
            // lblTitleSub0
            // 
            this.lblTitleSub0.AutoSize = true;
            this.lblTitleSub0.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitleSub0.ForeColor = System.Drawing.Color.White;
            this.lblTitleSub0.Location = new System.Drawing.Point(8, 4);
            this.lblTitleSub0.Name = "lblTitleSub0";
            this.lblTitleSub0.Size = new System.Drawing.Size(55, 15);
            this.lblTitleSub0.TabIndex = 0;
            this.lblTitleSub0.Text = "환자정보";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblPatInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(2);
            this.panel1.Size = new System.Drawing.Size(710, 34);
            this.panel1.TabIndex = 14;
            // 
            // lblPatInfo
            // 
            this.lblPatInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblPatInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPatInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPatInfo.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.lblPatInfo.Location = new System.Drawing.Point(2, 2);
            this.lblPatInfo.Name = "lblPatInfo";
            this.lblPatInfo.Size = new System.Drawing.Size(706, 30);
            this.lblPatInfo.TabIndex = 0;
            this.lblPatInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 19);
            this.label1.TabIndex = 15;
            this.label1.Text = "CP 명칭";
            // 
            // cboCpCode
            // 
            this.cboCpCode.FormattingEnabled = true;
            this.cboCpCode.Location = new System.Drawing.Point(64, 7);
            this.cboCpCode.Name = "cboCpCode";
            this.cboCpCode.Size = new System.Drawing.Size(250, 25);
            this.cboCpCode.TabIndex = 16;
            // 
            // btnCPActive
            // 
            this.btnCPActive.Location = new System.Drawing.Point(320, 40);
            this.btnCPActive.Name = "btnCPActive";
            this.btnCPActive.Size = new System.Drawing.Size(130, 30);
            this.btnCPActive.TabIndex = 17;
            this.btnCPActive.Text = "CP activation";
            this.btnCPActive.UseVisualStyleBackColor = true;
            this.btnCPActive.Click += new System.EventHandler(this.btnCPActive_Click);
            // 
            // btnCPWarm
            // 
            this.btnCPWarm.Location = new System.Drawing.Point(320, 4);
            this.btnCPWarm.Name = "btnCPWarm";
            this.btnCPWarm.Size = new System.Drawing.Size(130, 30);
            this.btnCPWarm.TabIndex = 17;
            this.btnCPWarm.Text = "예비 CP";
            this.btnCPWarm.UseVisualStyleBackColor = true;
            this.btnCPWarm.Click += new System.EventHandler(this.btnCPWarm_Click);
            // 
            // btnCPDeActive
            // 
            this.btnCPDeActive.Location = new System.Drawing.Point(320, 76);
            this.btnCPDeActive.Name = "btnCPDeActive";
            this.btnCPDeActive.Size = new System.Drawing.Size(130, 30);
            this.btnCPDeActive.TabIndex = 17;
            this.btnCPDeActive.Text = "CP deactivation";
            this.btnCPDeActive.UseVisualStyleBackColor = true;
            this.btnCPDeActive.Click += new System.EventHandler(this.btnCPDeActive_Click);
            // 
            // btnCPAct
            // 
            this.btnCPAct.Location = new System.Drawing.Point(320, 112);
            this.btnCPAct.Name = "btnCPAct";
            this.btnCPAct.Size = new System.Drawing.Size(130, 30);
            this.btnCPAct.TabIndex = 17;
            this.btnCPAct.Text = "시술";
            this.btnCPAct.UseVisualStyleBackColor = true;
            this.btnCPAct.Click += new System.EventHandler(this.btnCPAct_Click);
            // 
            // lblWarm
            // 
            this.lblWarm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWarm.Location = new System.Drawing.Point(456, 4);
            this.lblWarm.Name = "lblWarm";
            this.lblWarm.Size = new System.Drawing.Size(250, 30);
            this.lblWarm.TabIndex = 18;
            this.lblWarm.Text = "2018-01-01 00:00 (가나다라마)";
            this.lblWarm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblActive
            // 
            this.lblActive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblActive.Location = new System.Drawing.Point(456, 40);
            this.lblActive.Name = "lblActive";
            this.lblActive.Size = new System.Drawing.Size(250, 30);
            this.lblActive.TabIndex = 18;
            this.lblActive.Text = "2018-01-01 00:00 (가나다라마)";
            this.lblActive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDeActive
            // 
            this.lblDeActive.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDeActive.Location = new System.Drawing.Point(456, 76);
            this.lblDeActive.Name = "lblDeActive";
            this.lblDeActive.Size = new System.Drawing.Size(250, 30);
            this.lblDeActive.TabIndex = 18;
            this.lblDeActive.Text = "2018-01-01 00:00 (가나다라마)";
            this.lblDeActive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAct
            // 
            this.lblAct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAct.Location = new System.Drawing.Point(456, 112);
            this.lblAct.Name = "lblAct";
            this.lblAct.Size = new System.Drawing.Size(250, 30);
            this.lblAct.TabIndex = 18;
            this.lblAct.Text = "2018-01-01 00:00 (가나다라마)";
            this.lblAct.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCPNew
            // 
            this.btnCPNew.Location = new System.Drawing.Point(64, 40);
            this.btnCPNew.Name = "btnCPNew";
            this.btnCPNew.Size = new System.Drawing.Size(90, 30);
            this.btnCPNew.TabIndex = 17;
            this.btnCPNew.Text = "CP 추가";
            this.btnCPNew.UseVisualStyleBackColor = true;
            this.btnCPNew.Visible = false;
            this.btnCPNew.Click += new System.EventHandler(this.btnCPNew_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 19);
            this.label2.TabIndex = 15;
            this.label2.Text = "deactivation 사유";
            // 
            // cboCpSayu
            // 
            this.cboCpSayu.FormattingEnabled = true;
            this.cboCpSayu.Location = new System.Drawing.Point(123, 115);
            this.cboCpSayu.Name = "cboCpSayu";
            this.cboCpSayu.Size = new System.Drawing.Size(191, 25);
            this.cboCpSayu.TabIndex = 16;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lblAct);
            this.panel2.Controls.Add(this.cboCpCode);
            this.panel2.Controls.Add(this.lblDeActive);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.lblActive);
            this.panel2.Controls.Add(this.cboCpSayu);
            this.panel2.Controls.Add(this.lblWarm);
            this.panel2.Controls.Add(this.btnCPActive);
            this.panel2.Controls.Add(this.btnCPAct);
            this.panel2.Controls.Add(this.btnCPWarm);
            this.panel2.Controls.Add(this.btnCPDeActive);
            this.panel2.Controls.Add(this.btnCPNew);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 124);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(710, 146);
            this.panel2.TabIndex = 19;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 96);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(710, 28);
            this.panel3.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(8, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "CP정보";
            // 
            // frmOcsCpSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(710, 270);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panTitleSub0);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmOcsCpSet";
            this.Text = "frmOcsCpSet";
            this.Load += new System.EventHandler(this.frmOcsCpSet_Load);
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panTitleSub0.ResumeLayout(false);
            this.panTitleSub0.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panTitleSub0;
        private System.Windows.Forms.Label lblTitleSub0;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblPatInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboCpCode;
        private System.Windows.Forms.Button btnCPActive;
        private System.Windows.Forms.Button btnCPWarm;
        private System.Windows.Forms.Button btnCPDeActive;
        private System.Windows.Forms.Button btnCPAct;
        private System.Windows.Forms.Label lblWarm;
        private System.Windows.Forms.Label lblActive;
        private System.Windows.Forms.Label lblDeActive;
        private System.Windows.Forms.Label lblAct;
        private System.Windows.Forms.Button btnCPNew;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboCpSayu;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
    }
}