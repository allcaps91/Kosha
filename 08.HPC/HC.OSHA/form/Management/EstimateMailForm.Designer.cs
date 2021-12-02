namespace HC_OSHA
{
    partial class EstimateMailForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EstimateMailForm));
            this.mailForm = new ComBase.Mvc.UserControls.MailForm();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // mailForm
            // 
            this.mailForm.AttachmentsList = ((System.Collections.Generic.List<string>)(resources.GetObject("mailForm.AttachmentsList")));
            this.mailForm.Body = null;
            this.mailForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mailForm.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.mailForm.Location = new System.Drawing.Point(0, 0);
            this.mailForm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.mailForm.Name = "mailForm";
            this.mailForm.ReciverMailSddress = ((System.Collections.Generic.List<string>)(resources.GetObject("mailForm.ReciverMailSddress")));
            this.mailForm.SenderMailAddress = null;
            this.mailForm.Size = new System.Drawing.Size(565, 583);
            this.mailForm.SMTP_PASSWORD = null;
            this.mailForm.SMTP_USERID = null;
            this.mailForm.Subject = null;
            this.mailForm.TabIndex = 0;
            this.mailForm.SendMailClick += new ComBase.Mvc.UserControls.MailForm.SendMailClickEventHandler(this.MailForm_SendMailClick);
            this.mailForm.ReceiverListClick += new ComBase.Mvc.UserControls.MailForm.ReceiverListClickEventHandler(this.mailForm_ReceiverListClick);
            this.mailForm.Load += new System.EventHandler(this.mailForm_Load);
            // 
            // EstimateMailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 583);
            this.Controls.Add(this.mailForm);
            this.Name = "EstimateMailForm";
            this.Text = "메일 발송";
            this.Load += new System.EventHandler(this.mailForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ComBase.Mvc.UserControls.MailForm mailForm;
    }
}