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
            this.txtNewVer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBuild = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnFileSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtOldVer
            // 
            this.txtOldVer.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtOldVer.Location = new System.Drawing.Point(132, 8);
            this.txtOldVer.Name = "txtOldVer";
            this.txtOldVer.Size = new System.Drawing.Size(106, 25);
            this.txtOldVer.TabIndex = 4;
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
            // txtNewVer
            // 
            this.txtNewVer.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtNewVer.Location = new System.Drawing.Point(132, 41);
            this.txtNewVer.Name = "txtNewVer";
            this.txtNewVer.Size = new System.Drawing.Size(106, 25);
            this.txtNewVer.TabIndex = 0;
            this.txtNewVer.Tag = "";
            this.txtNewVer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            // btnBuild
            // 
            this.btnBuild.Location = new System.Drawing.Point(12, 73);
            this.btnBuild.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(142, 31);
            this.btnBuild.TabIndex = 1;
            this.btnBuild.Text = "업데이트 파일 생성";
            this.btnBuild.UseVisualStyleBackColor = true;
            this.btnBuild.Click += new System.EventHandler(this.btnBuild_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(168, 73);
            this.btnExit.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(69, 31);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "닫기";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblMsg
            // 
            this.lblMsg.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.lblMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMsg.Location = new System.Drawing.Point(14, 111);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(223, 28);
            this.lblMsg.TabIndex = 147;
            this.lblMsg.Text = "lblMsg";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMsg.Click += new System.EventHandler(this.lblMsg_Click);
            // 
            // btnFileSend
            // 
            this.btnFileSend.Location = new System.Drawing.Point(52, 146);
            this.btnFileSend.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnFileSend.Name = "btnFileSend";
            this.btnFileSend.Size = new System.Drawing.Size(142, 31);
            this.btnFileSend.TabIndex = 3;
            this.btnFileSend.Text = "서버에 파일 전송";
            this.btnFileSend.UseVisualStyleBackColor = true;
            this.btnFileSend.Click += new System.EventHandler(this.btnFileSend_Click);
            // 
            // FrmUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 188);
            this.ControlBox = false;
            this.Controls.Add(this.btnFileSend);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnBuild);
            this.Controls.Add(this.txtNewVer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtOldVer);
            this.Controls.Add(this.lblLTD02);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmUpload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "업데이트 서버 업로드";
            this.Load += new System.EventHandler(this.FrmUpload_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOldVer;
        private System.Windows.Forms.Label lblLTD02;
        private System.Windows.Forms.TextBox txtNewVer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBuild;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button btnFileSend;
    }
}