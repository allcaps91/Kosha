using System;
using System.Collections.Generic;

namespace ComBase
{
    public class clsMail
    {
        /// <summary>
        /// SMTP 주소
        /// </summary>
        public string smtpServer = "smtp.mailplug.co.kr";
        /// <summary>
        /// SMTP 포트
        /// </summary>
        public int smtpPort = 465;
        /// <summary>
        /// 보내는사람 USER ID
        /// </summary>
        public string smtpUser = "psmh@pohangsmh.co.kr";
        /// <summary>
        /// 보내는사람 USER PW
        /// </summary>
        public string smtpPW = "vhgkdtjdahquddnjs*";
        /// <summary>
        /// 보내는사람 주소
        /// </summary>
        public string FromEmailAddr = "psmh@pohangsmh.co.kr";
        /// <summary>
        /// 받는사람 주소
        /// </summary>
        public string ToEmailAddr = string.Empty;
        /// <summary>
        /// 메일 제목
        /// </summary>
        public string Subject = string.Empty;
        /// <summary>
        /// 메일 내용
        /// </summary>
        public string Body = string.Empty;
        /// <summary>
        /// 첨부파일 URL
        /// </summary>
        public List<string> Attachment = null;

        public clsMail()
        {
            Attachment = new List<string>();
        }

        public bool SendMail()
        {            
            try
            {
                CDO.Message oMsg = new CDO.Message();
                CDO.IConfiguration iConfg;
                iConfg = oMsg.Configuration;
                ADODB.Fields oFields;
                oFields = iConfg.Fields;

                ADODB.Field field = oFields["http://schemas.microsoft.com/cdo/configuration/smtpserver"];
                field.Value = smtpServer;

                field = oFields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"];
                field.Value = smtpPort;

                field = oFields["http://schemas.microsoft.com/cdo/configuration/sendusing"];
                field.Value = CDO.CdoSendUsing.cdoSendUsingPort;

                field = oFields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"];
                field.Value = CDO.CdoProtocolsAuthentication.cdoBasic;

                field = oFields["http://schemas.microsoft.com/cdo/configuration/sendusername"];
                field.Value = smtpUser;

                field = oFields["http://schemas.microsoft.com/cdo/configuration/sendpassword"];
                field.Value = smtpPW;

                field = oFields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"];
                field.Value = "true";

                oFields.Update();                
                oMsg.From = FromEmailAddr;
                oMsg.To = ToEmailAddr;
                oMsg.Subject = Subject;
                oMsg.TextBody = Body;
                foreach (string ss in Attachment)
                {
                    oMsg.AddAttachment(ss);
                }
                
                oMsg.Send();

                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }
    }
}
