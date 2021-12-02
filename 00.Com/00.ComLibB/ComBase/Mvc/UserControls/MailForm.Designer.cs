namespace ComBase.Mvc.UserControls
{
    partial class MailForm
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnReceiver = new System.Windows.Forms.Button();
            this.BtnSend = new System.Windows.Forms.Button();
            this.BtnOpenFile = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.TxtBody = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtAttachment = new System.Windows.Forms.TextBox();
            this.TxtSubject = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtReceiveMailAddress = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TxtSendMailAddress = new System.Windows.Forms.TextBox();
            this.contentTitle2 = new ComBase.Mvc.UserControls.ContentTitle();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.BtnReceiver);
            this.panel1.Controls.Add(this.BtnSend);
            this.panel1.Controls.Add(this.BtnOpenFile);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.TxtBody);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.TxtAttachment);
            this.panel1.Controls.Add(this.TxtSubject);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.TxtReceiveMailAddress);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.TxtSendMailAddress);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(591, 642);
            this.panel1.TabIndex = 2;
            // 
            // BtnReceiver
            // 
            this.BtnReceiver.Location = new System.Drawing.Point(469, 99);
            this.BtnReceiver.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnReceiver.Name = "BtnReceiver";
            this.BtnReceiver.Size = new System.Drawing.Size(75, 28);
            this.BtnReceiver.TabIndex = 90;
            this.BtnReceiver.Text = "목록";
            this.BtnReceiver.UseVisualStyleBackColor = true;
            this.BtnReceiver.Click += new System.EventHandler(this.BtnReceiver_Click);
            // 
            // BtnSend
            // 
            this.BtnSend.Location = new System.Drawing.Point(469, 38);
            this.BtnSend.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnSend.Name = "BtnSend";
            this.BtnSend.Size = new System.Drawing.Size(75, 28);
            this.BtnSend.TabIndex = 5;
            this.BtnSend.Text = "보내기";
            this.BtnSend.UseVisualStyleBackColor = true;
            this.BtnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // BtnOpenFile
            // 
            this.BtnOpenFile.Location = new System.Drawing.Point(469, 446);
            this.BtnOpenFile.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.BtnOpenFile.Name = "BtnOpenFile";
            this.BtnOpenFile.Size = new System.Drawing.Size(75, 28);
            this.BtnOpenFile.TabIndex = 88;
            this.BtnOpenFile.Text = "파일찾기";
            this.BtnOpenFile.UseVisualStyleBackColor = true;
            this.BtnOpenFile.Click += new System.EventHandler(this.BtnOpenFile_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(14, 448);
            this.label5.Name = "label5";
            this.label5.Padding = new System.Windows.Forms.Padding(4);
            this.label5.Size = new System.Drawing.Size(123, 25);
            this.label5.TabIndex = 78;
            this.label5.Text = "첨부파일";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtBody
            // 
            this.TxtBody.Location = new System.Drawing.Point(141, 177);
            this.TxtBody.Multiline = true;
            this.TxtBody.Name = "TxtBody";
            this.TxtBody.Size = new System.Drawing.Size(403, 233);
            this.TxtBody.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(14, 176);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(4);
            this.label4.Size = new System.Drawing.Size(123, 25);
            this.label4.TabIndex = 76;
            this.label4.Text = "내용";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(14, 141);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(4);
            this.label3.Size = new System.Drawing.Size(123, 25);
            this.label3.TabIndex = 75;
            this.label3.Text = "제목";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtAttachment
            // 
            this.TxtAttachment.Location = new System.Drawing.Point(143, 448);
            this.TxtAttachment.Name = "TxtAttachment";
            this.TxtAttachment.ReadOnly = true;
            this.TxtAttachment.Size = new System.Drawing.Size(307, 25);
            this.TxtAttachment.TabIndex = 74;
            // 
            // TxtSubject
            // 
            this.TxtSubject.Location = new System.Drawing.Point(141, 141);
            this.TxtSubject.Name = "TxtSubject";
            this.TxtSubject.Size = new System.Drawing.Size(403, 25);
            this.TxtSubject.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(255, 17);
            this.label2.TabIndex = 73;
            this.label2.Text = "받는사람 메일주소는 콤마(,)로 구분합니다";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(14, 102);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(4);
            this.label1.Size = new System.Drawing.Size(123, 25);
            this.label1.TabIndex = 72;
            this.label1.Text = "받는사람";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtReceiveMailAddress
            // 
            this.TxtReceiveMailAddress.Location = new System.Drawing.Point(141, 102);
            this.TxtReceiveMailAddress.Name = "TxtReceiveMailAddress";
            this.TxtReceiveMailAddress.Size = new System.Drawing.Size(293, 25);
            this.TxtReceiveMailAddress.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(14, 40);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(4);
            this.label8.Size = new System.Drawing.Size(123, 25);
            this.label8.TabIndex = 70;
            this.label8.Text = "보내는사람";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TxtSendMailAddress
            // 
            this.TxtSendMailAddress.Location = new System.Drawing.Point(141, 40);
            this.TxtSendMailAddress.Name = "TxtSendMailAddress";
            this.TxtSendMailAddress.Size = new System.Drawing.Size(293, 25);
            this.TxtSendMailAddress.TabIndex = 1;
            // 
            // contentTitle2
            // 
            this.contentTitle2.Dock = System.Windows.Forms.DockStyle.Top;
            this.contentTitle2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.contentTitle2.Location = new System.Drawing.Point(0, 0);
            this.contentTitle2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.contentTitle2.Name = "contentTitle2";
            this.contentTitle2.Size = new System.Drawing.Size(591, 38);
            this.contentTitle2.TabIndex = 1;
            this.contentTitle2.TitleText = "메일전송";
            // 
            // MailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.contentTitle2);
            this.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MailForm";
            this.Size = new System.Drawing.Size(591, 680);
            this.Load += new System.EventHandler(this.MailForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ContentTitle contentTitle2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TxtBody;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtAttachment;
        private System.Windows.Forms.TextBox TxtSubject;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtReceiveMailAddress;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TxtSendMailAddress;
        private System.Windows.Forms.Button BtnOpenFile;
        private System.Windows.Forms.Button BtnSend;
        private System.Windows.Forms.Button BtnReceiver;
    }
}
