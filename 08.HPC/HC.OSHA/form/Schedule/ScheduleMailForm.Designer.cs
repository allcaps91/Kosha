namespace HC_OSHA
{
    partial class ScheduleMailForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScheduleMailForm));
            this.mailForm1 = new ComBase.Mvc.UserControls.MailForm();
            this.SuspendLayout();
            // 
            // mailForm1
            // 
            this.mailForm1.AttachmentsList = ((System.Collections.Generic.List<string>)(resources.GetObject("mailForm1.AttachmentsList")));
            this.mailForm1.Body = null;
            this.mailForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mailForm1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mailForm1.Location = new System.Drawing.Point(0, 0);
            this.mailForm1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mailForm1.Name = "mailForm1";
            this.mailForm1.ReciverMailSddress = ((System.Collections.Generic.List<string>)(resources.GetObject("mailForm1.ReciverMailSddress")));
            this.mailForm1.SenderMailAddress = null;
            this.mailForm1.Size = new System.Drawing.Size(564, 547);
            this.mailForm1.SMTP_PASSWORD = null;
            this.mailForm1.SMTP_USERID = null;
            this.mailForm1.Subject = null;
            this.mailForm1.TabIndex = 0;
            // 
            // ScheduleMailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 547);
            this.Controls.Add(this.mailForm1);
            this.Name = "ScheduleMailForm";
            this.Text = "일정 메일 발송";
            this.ResumeLayout(false);

        }

        #endregion

        private ComBase.Mvc.UserControls.MailForm mailForm1;
    }
}