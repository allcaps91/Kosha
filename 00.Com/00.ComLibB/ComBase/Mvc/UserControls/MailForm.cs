using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase.Mvc.Utils;
using ComBase.Controls;

namespace ComBase.Mvc.UserControls
{
    [Description("메일전송 폼")]
    public partial class MailForm : UserControl
    {
        public delegate void SendMailClickEventHandler(object sender, EventArgs e);

        public delegate void ReceiverListClickEventHandler(object sender, EventArgs e);

        [Category("A-MTS-Framework-Event")]
        [Description("메일전송 이벤트")]
        public event SendMailClickEventHandler SendMailClick;

        [Category("A-MTS-Framework-Event")]
        [Description("메일받는사람 목록 이벤트")]
        public event ReceiverListClickEventHandler ReceiverListClick;

        private MailUtil mailUtil;

        public string SMTP_USERID { get; set; }
        public string SMTP_PASSWORD { get; set; }

        /// <summary>
        /// 보내는사람
        /// </summary>
        public string SenderMailAddress { get; set; }
        /// <summary>
        /// 제목
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 내용
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 받는사람목록
        /// </summary>
        public List<string> ReciverMailSddress { get; set; }


        /// <summary>
        /// 첨부파일 목록
        /// </summary>
        public List<string> AttachmentsList { get; set; }

        public MailForm()
        {
            InitializeComponent();
            ReciverMailSddress = new List<string>();
            AttachmentsList = new List<string>();
        }
        public MailForm(string SMTP_USERID, string SMTP_PASSWORD)
        {
            InitializeComponent();
            this.SMTP_PASSWORD = SMTP_PASSWORD;
            this.SMTP_USERID = SMTP_USERID;
            ReciverMailSddress = new List<string>();
            AttachmentsList = new List<string>(); 

        }
        private void MailForm_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {

                mailUtil = new MailUtil(SMTP_USERID, SMTP_PASSWORD);
                if (SenderMailAddress.NotEmpty())
                {
                    TxtSendMailAddress.Text = SenderMailAddress;
                }

                TxtReceiveMailAddress.Text = string.Join(",", ReciverMailSddress.ToArray());

                if (Subject.NotEmpty())
                {
                    TxtSubject.Text = Subject;
                }
                if (Body.NotEmpty())
                {
                    TxtBody.Text = Body;
                }
                if (AttachmentsList.Count > 0)
                {
                    TxtAttachment.Text = AttachmentsList[0];
                }
                
            }
            
        }

        public void RefreshReceiver()
        {
            TxtReceiveMailAddress.Text = "";
            TxtReceiveMailAddress.Text = string.Join(",", ReciverMailSddress.ToArray());
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (TxtReceiveMailAddress.Text.IsNullOrEmpty())
                {
                    MessageUtil.Alert("받는 사람 주소가 없습니다");
                    return;
                }
                if (TxtSubject.Text.IsNullOrEmpty())
                {
                    MessageUtil.Alert("메일 제목이 없습니다");
                    return;
                }
                if (TxtBody.Text.IsNullOrEmpty())
                {
                    MessageUtil.Alert("메일내용이 없습니다");
                    return;
                }

                mailUtil.SenderMailAddress = TxtSendMailAddress.Text;

                string[] address = TxtReceiveMailAddress.Text.Split(',');

                mailUtil.ReciverMailSddress.Clear();
                foreach (string mailAddress in address)
                {
                    if (mailAddress.NotEmpty())
                    {
                        mailUtil.ReciverMailSddress.Add(mailAddress.Trim());
                    }
                    
                }

                mailUtil.Subject = TxtSubject.Text;

                mailUtil.Body = TxtBody.Text;

                mailUtil.Attachments.Add(TxtAttachment.Text);

                mailUtil.SendMaill();

                SendMailClick?.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
               SendMailClick?.Invoke(ex, e);
            }
        }

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            String fileName = dialog.FileName;

            AttachmentsList.Add(fileName);
            TxtAttachment.Text = fileName;
        }

        private void BtnReceiver_Click(object sender, EventArgs e)
        {
            ReceiverListClick?.Invoke(sender, e);
        }
    }
}
