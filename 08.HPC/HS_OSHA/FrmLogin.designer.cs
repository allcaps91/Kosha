namespace HS_OSHA
{
    partial class FrmLogin
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
            this.lblLicno = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtIdNumber = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtGuide = new System.Windows.Forms.TextBox();
            this.lblSangho = new System.Windows.Forms.Label();
            this.lblLTD02 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblLicno
            // 
            this.lblLicno.BackColor = System.Drawing.Color.Transparent;
            this.lblLicno.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLicno.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblLicno.Location = new System.Drawing.Point(187, 10);
            this.lblLicno.Name = "lblLicno";
            this.lblLicno.Size = new System.Drawing.Size(135, 18);
            this.lblLicno.TabIndex = 5;
            this.lblLicno.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Location = new System.Drawing.Point(369, 70);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 26);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "종  료";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Location = new System.Drawing.Point(455, 70);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(112, 26);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "로  그  인";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtIdNumber
            // 
            this.txtIdNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIdNumber.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtIdNumber.Location = new System.Drawing.Point(455, 9);
            this.txtIdNumber.Name = "txtIdNumber";
            this.txtIdNumber.Size = new System.Drawing.Size(112, 25);
            this.txtIdNumber.TabIndex = 0;
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPassword.Location = new System.Drawing.Point(455, 38);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(112, 25);
            this.txtPassword.TabIndex = 1;
            // 
            // txtGuide
            // 
            this.txtGuide.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGuide.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtGuide.Location = new System.Drawing.Point(4, 323);
            this.txtGuide.Multiline = true;
            this.txtGuide.Name = "txtGuide";
            this.txtGuide.ReadOnly = true;
            this.txtGuide.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtGuide.Size = new System.Drawing.Size(571, 110);
            this.txtGuide.TabIndex = 10;
            // 
            // lblSangho
            // 
            this.lblSangho.BackColor = System.Drawing.Color.Transparent;
            this.lblSangho.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSangho.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblSangho.Location = new System.Drawing.Point(11, 10);
            this.lblSangho.Name = "lblSangho";
            this.lblSangho.Size = new System.Drawing.Size(180, 18);
            this.lblSangho.TabIndex = 11;
            this.lblSangho.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLTD02
            // 
            this.lblLTD02.BackColor = System.Drawing.Color.LightBlue;
            this.lblLTD02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLTD02.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLTD02.Location = new System.Drawing.Point(369, 9);
            this.lblLTD02.Name = "lblLTD02";
            this.lblLTD02.Size = new System.Drawing.Size(80, 25);
            this.lblLTD02.TabIndex = 81;
            this.lblLTD02.Text = "아이디";
            this.lblLTD02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightBlue;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(369, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 25);
            this.label1.TabIndex = 82;
            this.label1.Text = "비밀번호";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::HS_OSHA.Properties.Resources.login_bakImg1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(579, 435);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblLTD02);
            this.Controls.Add(this.lblSangho);
            this.Controls.Add(this.txtGuide);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtIdNumber);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.lblLicno);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "보건관리전문 프로그램";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLicno;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtIdNumber;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtGuide;
        private System.Windows.Forms.Label lblSangho;
        private System.Windows.Forms.Label lblLTD02;
        private System.Windows.Forms.Label label1;
    }
}