namespace HcAdmin
{
    partial class FrmMainform
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.닫기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.라이선스ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.안내문등록ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.서버업로드ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.설치파일만들기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.싸인복사ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.테스트ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.특정폴더삭제ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panLogin = new System.Windows.Forms.Panel();
            this.CmdExit = new System.Windows.Forms.Button();
            this.CmdLogin = new System.Windows.Forms.Button();
            this.TxtPass = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.서버업로드ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.엑셀파일업로드ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.panLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.닫기ToolStripMenuItem,
            this.라이선스ToolStripMenuItem,
            this.안내문등록ToolStripMenuItem,
            this.서버업로드ToolStripMenuItem,
            this.설치파일만들기ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.테스트ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1067, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 닫기ToolStripMenuItem
            // 
            this.닫기ToolStripMenuItem.Name = "닫기ToolStripMenuItem";
            this.닫기ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.닫기ToolStripMenuItem.Text = "닫기";
            this.닫기ToolStripMenuItem.Click += new System.EventHandler(this.닫기ToolStripMenuItem_Click);
            // 
            // 라이선스ToolStripMenuItem
            // 
            this.라이선스ToolStripMenuItem.Name = "라이선스ToolStripMenuItem";
            this.라이선스ToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.라이선스ToolStripMenuItem.Text = "라이선스";
            this.라이선스ToolStripMenuItem.Click += new System.EventHandler(this.라이선스ToolStripMenuItem_Click);
            // 
            // 안내문등록ToolStripMenuItem
            // 
            this.안내문등록ToolStripMenuItem.Name = "안내문등록ToolStripMenuItem";
            this.안내문등록ToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.안내문등록ToolStripMenuItem.Text = "안내문등록";
            this.안내문등록ToolStripMenuItem.Click += new System.EventHandler(this.안내문등록ToolStripMenuItem_Click);
            // 
            // 서버업로드ToolStripMenuItem
            // 
            this.서버업로드ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.서버업로드ToolStripMenuItem1,
            this.엑셀파일업로드ToolStripMenuItem});
            this.서버업로드ToolStripMenuItem.Name = "서버업로드ToolStripMenuItem";
            this.서버업로드ToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.서버업로드ToolStripMenuItem.Text = "서버업로드";
            this.서버업로드ToolStripMenuItem.Click += new System.EventHandler(this.서버업로드ToolStripMenuItem_Click);
            // 
            // 설치파일만들기ToolStripMenuItem
            // 
            this.설치파일만들기ToolStripMenuItem.Name = "설치파일만들기ToolStripMenuItem";
            this.설치파일만들기ToolStripMenuItem.Size = new System.Drawing.Size(103, 20);
            this.설치파일만들기ToolStripMenuItem.Text = "설치파일만들기";
            this.설치파일만들기ToolStripMenuItem.Click += new System.EventHandler(this.설치파일만들기ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.싸인복사ToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(59, 20);
            this.toolStripMenuItem1.Text = "DB관리";
            // 
            // 싸인복사ToolStripMenuItem
            // 
            this.싸인복사ToolStripMenuItem.Name = "싸인복사ToolStripMenuItem";
            this.싸인복사ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.싸인복사ToolStripMenuItem.Text = "1.싸인 복사";
            this.싸인복사ToolStripMenuItem.Click += new System.EventHandler(this.싸인복사ToolStripMenuItem_Click);
            // 
            // 테스트ToolStripMenuItem
            // 
            this.테스트ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.특정폴더삭제ToolStripMenuItem});
            this.테스트ToolStripMenuItem.Name = "테스트ToolStripMenuItem";
            this.테스트ToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.테스트ToolStripMenuItem.Text = "테스트";
            // 
            // 특정폴더삭제ToolStripMenuItem
            // 
            this.특정폴더삭제ToolStripMenuItem.Name = "특정폴더삭제ToolStripMenuItem";
            this.특정폴더삭제ToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.특정폴더삭제ToolStripMenuItem.Text = "특정폴더 삭제";
            this.특정폴더삭제ToolStripMenuItem.Click += new System.EventHandler(this.특정폴더삭제ToolStripMenuItem_Click);
            // 
            // panLogin
            // 
            this.panLogin.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panLogin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panLogin.Controls.Add(this.CmdExit);
            this.panLogin.Controls.Add(this.CmdLogin);
            this.panLogin.Controls.Add(this.TxtPass);
            this.panLogin.Controls.Add(this.label1);
            this.panLogin.Location = new System.Drawing.Point(349, 95);
            this.panLogin.Name = "panLogin";
            this.panLogin.Size = new System.Drawing.Size(301, 175);
            this.panLogin.TabIndex = 2;
            // 
            // CmdExit
            // 
            this.CmdExit.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CmdExit.Location = new System.Drawing.Point(170, 111);
            this.CmdExit.Name = "CmdExit";
            this.CmdExit.Size = new System.Drawing.Size(84, 33);
            this.CmdExit.TabIndex = 6;
            this.CmdExit.Text = "종료";
            this.CmdExit.UseVisualStyleBackColor = true;
            this.CmdExit.Click += new System.EventHandler(this.CmdExit_Click_1);
            // 
            // CmdLogin
            // 
            this.CmdLogin.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CmdLogin.Location = new System.Drawing.Point(52, 111);
            this.CmdLogin.Name = "CmdLogin";
            this.CmdLogin.Size = new System.Drawing.Size(91, 33);
            this.CmdLogin.TabIndex = 5;
            this.CmdLogin.Text = "로그인";
            this.CmdLogin.UseVisualStyleBackColor = true;
            this.CmdLogin.Click += new System.EventHandler(this.CmdLogin_Click_1);
            // 
            // TxtPass
            // 
            this.TxtPass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtPass.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TxtPass.Location = new System.Drawing.Point(52, 56);
            this.TxtPass.Name = "TxtPass";
            this.TxtPass.PasswordChar = '*';
            this.TxtPass.Size = new System.Drawing.Size(202, 25);
            this.TxtPass.TabIndex = 0;
            this.TxtPass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(40, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "관리자의 비밀번호를 입력하세요";
            // 
            // 서버업로드ToolStripMenuItem1
            // 
            this.서버업로드ToolStripMenuItem1.Name = "서버업로드ToolStripMenuItem1";
            this.서버업로드ToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.서버업로드ToolStripMenuItem1.Text = "서버업로드";
            this.서버업로드ToolStripMenuItem1.Click += new System.EventHandler(this.서버업로드ToolStripMenuItem1_Click);
            // 
            // 엑셀파일업로드ToolStripMenuItem
            // 
            this.엑셀파일업로드ToolStripMenuItem.Name = "엑셀파일업로드ToolStripMenuItem";
            this.엑셀파일업로드ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.엑셀파일업로드ToolStripMenuItem.Text = "엑셀파일업로드";
            this.엑셀파일업로드ToolStripMenuItem.Click += new System.EventHandler(this.엑셀파일업로드ToolStripMenuItem_Click);
            // 
            // FrmMainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 658);
            this.Controls.Add(this.panLogin);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMainform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "보건관리 관리자";
            this.Load += new System.EventHandler(this.FrmMainform_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panLogin.ResumeLayout(false);
            this.panLogin.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 닫기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 라이선스ToolStripMenuItem;
        private System.Windows.Forms.Panel panLogin;
        private System.Windows.Forms.Button CmdExit;
        private System.Windows.Forms.Button CmdLogin;
        private System.Windows.Forms.TextBox TxtPass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem 안내문등록ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 서버업로드ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 설치파일만들기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 테스트ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 특정폴더삭제ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 싸인복사ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 서버업로드ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 엑셀파일업로드ToolStripMenuItem;
    }
}

