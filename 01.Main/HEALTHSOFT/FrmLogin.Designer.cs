namespace HEALTHSOFT
{
    partial class FrmLogin
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLTD02 = new System.Windows.Forms.Label();
            this.lblSangho = new System.Windows.Forms.Label();
            this.txtGuide = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtIdNumber = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblLicno = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightBlue;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(369, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 25);
            this.label1.TabIndex = 91;
            this.label1.Text = "비밀번호";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLTD02
            // 
            this.lblLTD02.BackColor = System.Drawing.Color.LightBlue;
            this.lblLTD02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLTD02.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLTD02.Location = new System.Drawing.Point(369, 5);
            this.lblLTD02.Name = "lblLTD02";
            this.lblLTD02.Size = new System.Drawing.Size(80, 25);
            this.lblLTD02.TabIndex = 90;
            this.lblLTD02.Text = "아이디";
            this.lblLTD02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSangho
            // 
            this.lblSangho.BackColor = System.Drawing.Color.Transparent;
            this.lblSangho.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSangho.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblSangho.Location = new System.Drawing.Point(11, 6);
            this.lblSangho.Name = "lblSangho";
            this.lblSangho.Size = new System.Drawing.Size(180, 18);
            this.lblSangho.TabIndex = 89;
            this.lblSangho.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtGuide
            // 
            this.txtGuide.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGuide.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtGuide.Location = new System.Drawing.Point(4, 319);
            this.txtGuide.Multiline = true;
            this.txtGuide.Name = "txtGuide";
            this.txtGuide.ReadOnly = true;
            this.txtGuide.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtGuide.Size = new System.Drawing.Size(571, 110);
            this.txtGuide.TabIndex = 88;
            // 
            // txtPassword
            // 
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtPassword.Location = new System.Drawing.Point(455, 34);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(112, 25);
            this.txtPassword.TabIndex = 84;
            // 
            // txtIdNumber
            // 
            this.txtIdNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIdNumber.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtIdNumber.Location = new System.Drawing.Point(455, 5);
            this.txtIdNumber.Name = "txtIdNumber";
            this.txtIdNumber.Size = new System.Drawing.Size(112, 25);
            this.txtIdNumber.TabIndex = 83;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Location = new System.Drawing.Point(369, 66);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 26);
            this.btnExit.TabIndex = 86;
            this.btnExit.Text = "종  료";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Location = new System.Drawing.Point(455, 66);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(112, 26);
            this.btnLogin.TabIndex = 85;
            this.btnLogin.Text = "로  그  인";
            this.btnLogin.UseVisualStyleBackColor = false;
            // 
            // lblLicno
            // 
            this.lblLicno.BackColor = System.Drawing.Color.Transparent;
            this.lblLicno.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLicno.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblLicno.Location = new System.Drawing.Point(187, 6);
            this.lblLicno.Name = "lblLicno";
            this.lblLicno.Size = new System.Drawing.Size(135, 18);
            this.lblLicno.TabIndex = 87;
            this.lblLicno.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::HEALTHSOFT.Properties.Resources.login_bakImg1;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(579, 435);
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
            this.Text = "(주)HealthSoft";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblLTD02;
        private System.Windows.Forms.Label lblSangho;
        private System.Windows.Forms.TextBox txtGuide;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtIdNumber;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblLicno;
        private System.Windows.Forms.Timer timer1;
    }
}

