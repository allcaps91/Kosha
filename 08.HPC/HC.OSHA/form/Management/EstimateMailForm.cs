using ComBase.Mvc.UserControls;
using ComBase.Mvc.Utils;
using HC.Core.Dto;
using HC.OSHA.Dto;
using HC_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_OSHA
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
            mailForm.SMTP_USERID = codeService.FindActiveCodeByGroupAndCode("OSHA_MANAGER", "mail_user", "OSHA").CodeName;
            mailForm.SMTP_PASSWORD = codeService.FindActiveCodeByGroupAndCode("OSHA_MANAGER", "mail_password", "OSHA").CodeName;
        }
        

        private void MailForm_SendMailClick(object sender, EventArgs e)
        {
            if(sender is Exception)
            {
                Exception ex = sender as Exception;
                MessageUtil.Info("메일 전송 실패하였습니다 \n" + ex.Message);
                
            }
            else
            {
                MessageUtil.Info("메일을 전송하였습니다");
                this.DialogResult = DialogResult.OK;
            }
            

        }

        private void mailForm_Load(object sender, EventArgs e)
        {
      
        }

        private void mailForm_ReceiverListClick(object sender, EventArgs e)
        {
       
            SiteWorkerPopupForm form = new SiteWorkerPopupForm();
            form.SelectedSite = base.SelectedSite;
            form.ShowDialog();


            this.mailForm.ReciverMailSddress.Clear();
            foreach (HC_SITE_WORKER worker in form.GetWorker())
            {
               this.mailForm.ReciverMailSddress.Add(worker.EMAIL);
            }
            this.mailForm.RefreshReceiver();
        }
    }
}

