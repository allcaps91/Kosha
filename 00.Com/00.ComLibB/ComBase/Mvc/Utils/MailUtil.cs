using ComBase.Controls;
using ComBase.Mvc.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComBase.Mvc.Utils
{

    //구글사용시 https://myaccount.google.com/lesssecureapps 허용으로 해야함ㄴ
    public class MailUtil
    {
        private string SMTP_URL = "smtp.gmail.com";
        private int SMTP_PORT = 587;
       // private string SMTP_URL = "smtp.mailplug.co.kr";
      //  private int SMTP_PORT = 465;

        /// <summary>
        /// 보내는사람
        /// </summary>
        public string SenderMailAddress { get; set; }
        /// <summary>
        /// 받는사람 목록
        /// </summary>
        public List<string> ReciverMailSddress { get; set; }
        /// <summary>
        /// 첨부파일 목록
        /// </summary>
        public List<string> Attachments { get; set; }
        /// <summary>
        /// 제목
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 내용
        /// </summary>
        public string Body { get; set; }

        public bool IsBodyHtml { get; set; }


        private SmtpClient smtpClient;
        public MailUtil(string SmtpuserId, string SmtpPassword)
        {
            ReciverMailSddress = new List<string>();
            Attachments = new List<string>();
            smtpClient = new SmtpClient();
            smtpClient.Host = this.SMTP_URL;
            smtpClient.Port = this.SMTP_PORT;
            smtpClient.EnableSsl = true;
            //      smtpClient.UseDefaultCredentials = true;
            //       smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //       
            if(SmtpuserId== null)
            {
                throw new MTSException(" 메일 계정이 없습니다");
            }
            smtpClient.Credentials = new NetworkCredential(SmtpuserId, SmtpPassword);
            //  smtpClient.Credentials = new NetworkCredential("psmh8193@pohangsmh.co.kr", "qhrjs123!");
              //smtpClient.Credentials = new NetworkCredential("psmh8193@gmail.com", "health8193");

        }
        public void SendMaill()
        {
           
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = IsBodyHtml;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            if(this.SenderMailAddress == null)
            {
                throw new MTSException("보내는 메일주소가 없습니다"); 
            }
            mail.From = new MailAddress(this.SenderMailAddress, "포항성모병원");

            foreach (string receiverMail in ReciverMailSddress)
            {
                mail.To.Add(receiverMail);
            }

            mail.Subject = Subject;

            mail.Body = Body;

            foreach (string fileName in Attachments)
            {
                mail.Attachments.Add(new Attachment(fileName));
            }
           
            smtpClient.Send(mail);
           // smtpClient.SendAsync(mail, true);




        }
        public void Dispose()
        {
            smtpClient.Dispose();
        }
    }
}
