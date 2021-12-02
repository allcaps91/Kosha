namespace HC_IF
{
    partial class frmHcKiosk_Main
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
            this.components = new System.ComponentModel.Container();
            this.panTitle = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panMain = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnTV_Call = new System.Windows.Forms.Button();
            this.btnWait_TV = new System.Windows.Forms.Button();
            this.btnHcKiosk = new System.Windows.Forms.Button();
            this.btnHaKiosk = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panTitle.SuspendLayout();
            this.panMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panTitle
            // 
            this.panTitle.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panTitle.Controls.Add(this.lblTitle);
            this.panTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panTitle.Location = new System.Drawing.Point(0, 0);
            this.panTitle.Name = "panTitle";
            this.panTitle.Size = new System.Drawing.Size(378, 40);
            this.panTitle.TabIndex = 14;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.Location = new System.Drawing.Point(3, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(176, 21);
            this.lblTitle.TabIndex = 4;
            this.lblTitle.Text = "건강증진센터 대기순번";
            // 
            // panMain
            // 
            this.panMain.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panMain.Controls.Add(this.btnExit);
            this.panMain.Controls.Add(this.btnTV_Call);
            this.panMain.Controls.Add(this.btnWait_TV);
            this.panMain.Controls.Add(this.btnHcKiosk);
            this.panMain.Controls.Add(this.btnHaKiosk);
            this.panMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panMain.Location = new System.Drawing.Point(0, 40);
            this.panMain.Name = "panMain";
            this.panMain.Size = new System.Drawing.Size(378, 438);
            this.panMain.TabIndex = 15;
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnExit.Location = new System.Drawing.Point(45, 348);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(288, 67);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "작업종료";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnTV_Call
            // 
            this.btnTV_Call.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnTV_Call.Location = new System.Drawing.Point(45, 265);
            this.btnTV_Call.Name = "btnTV_Call";
            this.btnTV_Call.Size = new System.Drawing.Size(288, 67);
            this.btnTV_Call.TabIndex = 3;
            this.btnTV_Call.Text = "TV호출번호";
            this.btnTV_Call.UseVisualStyleBackColor = true;
            // 
            // btnWait_TV
            // 
            this.btnWait_TV.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnWait_TV.Location = new System.Drawing.Point(45, 182);
            this.btnWait_TV.Name = "btnWait_TV";
            this.btnWait_TV.Size = new System.Drawing.Size(288, 67);
            this.btnWait_TV.TabIndex = 2;
            this.btnWait_TV.Text = "대기순번 안내TV";
            this.btnWait_TV.UseVisualStyleBackColor = true;
            // 
            // btnHcKiosk
            // 
            this.btnHcKiosk.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnHcKiosk.Location = new System.Drawing.Point(45, 99);
            this.btnHcKiosk.Name = "btnHcKiosk";
            this.btnHcKiosk.Size = new System.Drawing.Size(288, 67);
            this.btnHcKiosk.TabIndex = 1;
            this.btnHcKiosk.Text = "일반검진 키오스크";
            this.btnHcKiosk.UseVisualStyleBackColor = true;
            // 
            // btnHaKiosk
            // 
            this.btnHaKiosk.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnHaKiosk.Location = new System.Drawing.Point(45, 16);
            this.btnHaKiosk.Name = "btnHaKiosk";
            this.btnHaKiosk.Size = new System.Drawing.Size(288, 67);
            this.btnHaKiosk.TabIndex = 0;
            this.btnHaKiosk.Text = "종합검진 키오스크";
            this.btnHaKiosk.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 2000;
            // 
            // frmHcKiosk_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(378, 478);
            this.Controls.Add(this.panMain);
            this.Controls.Add(this.panTitle);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHcKiosk_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "건강증진센터 대기순번";
            this.panTitle.ResumeLayout(false);
            this.panTitle.PerformLayout();
            this.panMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panMain;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnHcKiosk;
        private System.Windows.Forms.Button btnHaKiosk;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnTV_Call;
        private System.Windows.Forms.Button btnWait_TV;
    }
}