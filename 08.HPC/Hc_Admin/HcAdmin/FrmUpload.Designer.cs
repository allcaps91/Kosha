namespace HcAdmin
{
    partial class FrmUpload
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
            this.txtOldVer = new System.Windows.Forms.TextBox();
            this.lblLTD02 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblMsg = new System.Windows.Forms.Label();
            this.txtNewVer = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnSetupFile = new System.Windows.Forms.Button();
            this.btnFileCopy = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtOldVer
            // 
            this.txtOldVer.Enabled = false;
            this.txtOldVer.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtOldVer.Location = new System.Drawing.Point(132, 8);
            this.txtOldVer.Name = "txtOldVer";
            this.txtOldVer.Size = new System.Drawing.Size(106, 25);
            this.txtOldVer.TabIndex = 1;
            this.txtOldVer.Tag = "";
            this.txtOldVer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblLTD02
            // 
            this.lblLTD02.BackColor = System.Drawing.Color.LightBlue;
            this.lblLTD02.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLTD02.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLTD02.Location = new System.Drawing.Point(12, 9);
            this.lblLTD02.Name = "lblLTD02";
            this.lblLTD02.Size = new System.Drawing.Size(114, 24);
            this.lblLTD02.TabIndex = 141;
            this.lblLTD02.Text = "변경 전 버전";
            this.lblLTD02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightBlue;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 24);
            this.label1.TabIndex = 143;
            this.label1.Text = "변경 후 버전";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(244, 8);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(63, 54);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblMsg
            // 
            this.lblMsg.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.lblMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMsg.Location = new System.Drawing.Point(12, 242);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(295, 28);
            this.lblMsg.TabIndex = 147;
            this.lblMsg.Text = "lblMsg";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMsg.Click += new System.EventHandler(this.lblMsg_Click);
            // 
            // txtNewVer
            // 
            this.txtNewVer.Location = new System.Drawing.Point(132, 41);
            this.txtNewVer.Name = "txtNewVer";
            this.txtNewVer.Size = new System.Drawing.Size(105, 25);
            this.txtNewVer.TabIndex = 2;
            this.txtNewVer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnSend);
            this.panel1.Controls.Add(this.btnSetupFile);
            this.panel1.Controls.Add(this.btnFileCopy);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(12, 72);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(294, 157);
            this.panel1.TabIndex = 148;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(15, 123);
            this.btnSend.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(261, 25);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "4. 업데이트 설치파일 서버전송";
            this.btnSend.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnSetupFile
            // 
            this.btnSetupFile.Location = new System.Drawing.Point(15, 85);
            this.btnSetupFile.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnSetupFile.Name = "btnSetupFile";
            this.btnSetupFile.Size = new System.Drawing.Size(261, 25);
            this.btnSetupFile.TabIndex = 3;
            this.btnSetupFile.Text = "3. 업데이트 설치파일 생성";
            this.btnSetupFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSetupFile.UseVisualStyleBackColor = true;
            this.btnSetupFile.Click += new System.EventHandler(this.btnSetupFile_Click);
            // 
            // btnFileCopy
            // 
            this.btnFileCopy.Location = new System.Drawing.Point(15, 51);
            this.btnFileCopy.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnFileCopy.Name = "btnFileCopy";
            this.btnFileCopy.Size = new System.Drawing.Size(261, 25);
            this.btnFileCopy.TabIndex = 2;
            this.btnFileCopy.Text = "2. 변경된 파일만 업데이트 폴더로 복사";
            this.btnFileCopy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFileCopy.UseVisualStyleBackColor = true;
            this.btnFileCopy.Click += new System.EventHandler(this.btnFileCopy_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(162, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "(빌드 -> 일괄빌드 해야됨)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "1. 소스를 수정 후 빌드를 한다.";
            // 
            // FrmUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(319, 282);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtNewVer);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOldVer);
            this.Controls.Add(this.lblLTD02);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmUpload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "업데이트 서버 업로드";
            this.Load += new System.EventHandler(this.FrmUpload_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOldVer;
        private System.Windows.Forms.Label lblLTD02;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.TextBox txtNewVer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSetupFile;
        private System.Windows.Forms.Button btnFileCopy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSend;
    }
}