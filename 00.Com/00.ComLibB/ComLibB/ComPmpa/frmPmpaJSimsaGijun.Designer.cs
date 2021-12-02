namespace ComLibB
{
    partial class frmPmpaJSimsaGijun
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
            this.pnlHead = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.panHead = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.panSuName = new System.Windows.Forms.Panel();
            this.lblSuName = new System.Windows.Forms.Label();
            this.txtSuCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtChkMsg = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlHead.SuspendLayout();
            this.panHead.SuspendLayout();
            this.panSuName.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHead
            // 
            this.pnlHead.BackColor = System.Drawing.Color.RoyalBlue;
            this.pnlHead.Controls.Add(this.label7);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.ForeColor = System.Drawing.Color.White;
            this.pnlHead.Location = new System.Drawing.Point(0, 0);
            this.pnlHead.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.pnlHead.Name = "pnlHead";
            this.pnlHead.Size = new System.Drawing.Size(424, 42);
            this.pnlHead.TabIndex = 183;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(14, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "재원 심사 기준";
            // 
            // panHead
            // 
            this.panHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panHead.Controls.Add(this.btnExit);
            this.panHead.Controls.Add(this.panSuName);
            this.panHead.Controls.Add(this.txtSuCode);
            this.panHead.Controls.Add(this.label1);
            this.panHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHead.Location = new System.Drawing.Point(0, 42);
            this.panHead.Name = "panHead";
            this.panHead.Padding = new System.Windows.Forms.Padding(5);
            this.panHead.Size = new System.Drawing.Size(424, 39);
            this.panHead.TabIndex = 184;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnExit.Location = new System.Drawing.Point(357, 5);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(60, 27);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panSuName
            // 
            this.panSuName.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.panSuName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panSuName.Controls.Add(this.lblSuName);
            this.panSuName.Location = new System.Drawing.Point(158, 8);
            this.panSuName.Name = "panSuName";
            this.panSuName.Padding = new System.Windows.Forms.Padding(3);
            this.panSuName.Size = new System.Drawing.Size(193, 23);
            this.panSuName.TabIndex = 2;
            // 
            // lblSuName
            // 
            this.lblSuName.AutoSize = true;
            this.lblSuName.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSuName.Location = new System.Drawing.Point(3, 3);
            this.lblSuName.Name = "lblSuName";
            this.lblSuName.Size = new System.Drawing.Size(55, 15);
            this.lblSuName.TabIndex = 1;
            this.lblSuName.Text = "수가코드";
            // 
            // txtSuCode
            // 
            this.txtSuCode.Location = new System.Drawing.Point(69, 8);
            this.txtSuCode.Name = "txtSuCode";
            this.txtSuCode.Size = new System.Drawing.Size(83, 23);
            this.txtSuCode.TabIndex = 1;
            this.txtSuCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSuCode_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "수가코드";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtRemark);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.txtChkMsg);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 81);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(424, 372);
            this.panel1.TabIndex = 185;
            // 
            // txtRemark
            // 
            this.txtRemark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemark.Location = new System.Drawing.Point(5, 63);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRemark.Size = new System.Drawing.Size(412, 302);
            this.txtRemark.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 53);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(412, 10);
            this.panel3.TabIndex = 4;
            // 
            // txtChkMsg
            // 
            this.txtChkMsg.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtChkMsg.Location = new System.Drawing.Point(5, 30);
            this.txtChkMsg.Name = "txtChkMsg";
            this.txtChkMsg.Size = new System.Drawing.Size(412, 23);
            this.txtChkMsg.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(5, 20);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(412, 10);
            this.panel2.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(5, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(147, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "재원심사 체크리스트 설명";
            // 
            // frmPmpaJSimsaGijun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 453);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panHead);
            this.Controls.Add(this.pnlHead);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmPmpaJSimsaGijun";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmPmpaJSimsaGijun_Load);
            this.pnlHead.ResumeLayout(false);
            this.pnlHead.PerformLayout();
            this.panHead.ResumeLayout(false);
            this.panHead.PerformLayout();
            this.panSuName.ResumeLayout(false);
            this.panSuName.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHead;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panHead;
        public System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panSuName;
        private System.Windows.Forms.Label lblSuName;
        private System.Windows.Forms.TextBox txtSuCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtChkMsg;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
    }
}