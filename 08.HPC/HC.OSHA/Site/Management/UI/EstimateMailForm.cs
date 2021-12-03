using ComBase.Mvc.UserControls;
using ComBase.Mvc.Utils;
using HC.Core.Common.UI;
using System;

namespace HC.OSHA.Site.Management.UI
{

    public partial class EstimateMailForm : CommonForm
    {
      
        public MailForm GetMailForm()
        {
            return this.mailForm;
        }
        public EstimateMailForm()
        {
            InitializeComponent();
            mailForm.SMTP_USERID = "faye12005@gmail.com";
            mailForm.SMTP_PASSWORD = "love!@0223";
        }

        private void MailForm_SendMailClick(object sender, EventArgs e)
        {
            MessageUtil.Info("메일을 전송합였습니다");
        }
    }
}

