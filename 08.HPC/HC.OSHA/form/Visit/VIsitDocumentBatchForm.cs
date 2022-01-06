using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Utils;
using HC.Core.Dto;
using HC.Core.Repository;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Repository;
using HC_Core;
using HC_OSHA.form.Charge;
using HC_OSHA.Model.Schedule;
using HC_OSHA.Service.Schedule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;

namespace HC_OSHA.form.Visit
{
    /// <summary>
    /// 방문일정 공문
    /// </summary>
    public partial class VIsitDocumentBatchForm : CommonForm
    {
        private VisitDocument visitDocumentForm = null;
        private VisitDocumentService visitDocumentService;
        private HcSiteWorkerRepository hcSiteWorkerRepository;

        private HcUsersRepository hcUsersRepository;
        private HicMailRepository hicMailRepository;

        private Role USER_ROLE = Role.ENGINEER;
        private MailType MAIL_TYPE = MailType.VISIT;

        public VIsitDocumentBatchForm()
        {
            InitializeComponent();
            visitDocumentService = new VisitDocumentService();
            visitDocumentForm = new VisitDocument();
            hcSiteWorkerRepository = new HcSiteWorkerRepository();

            hcUsersRepository = new HcUsersRepository();
            hicMailRepository = new HicMailRepository();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            long nESTIMATE_ID = 0;
            SSList.ActiveSheet.RowCount = 0;
            string month = CboMonth.GetValue();
            List<VisitDocumentModel> list = visitDocumentService.VisitDocumentRepository.FindScheduleByMonth(month);
            VisitDocumentModel addedModel = null;
            int rowIndex = 0;
            int COlumnIndex = 0;
            for(int i=0; i<list.Count; i++)
            {
                if(i==0 || addedModel.SiteId != list[i].SiteId)
                {
                    rowIndex = SSList.ActiveSheet.RowCount;
                    SSList.ActiveSheet.Rows.Add(rowIndex, 1);
                    addedModel = list[i];
                    COlumnIndex = 0;
                }
                else
                {
                    COlumnIndex += 1;
                }

                SSList.ActiveSheet.Cells[rowIndex, 1].Text = list[i].SiteId.ToString();
                SSList.ActiveSheet.Cells[rowIndex, 2].Text = list[i].SiteName;

                SSList.ActiveSheet.Cells[rowIndex, 9].Text = list[i].SENDDATE;
                SSList.ActiveSheet.Cells[rowIndex, 10].Text = list[i].SENDNAME;
                SSList.ActiveSheet.Cells[rowIndex, 11].Value = list[i].ESTIMATE_ID;

                if (COlumnIndex == 0)
                {
                    SSList.ActiveSheet.Cells[rowIndex, 3].Text =  list[i].VisitReserveDate.Substring(5) + " " + list[i].visitUserName;
                    if (!list[i].visitUserName2.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[rowIndex, 3].Text += " , "+list[i].visitUserName2;
                    }
                }
                else if (COlumnIndex == 1)
                {
                    SSList.ActiveSheet.Cells[rowIndex, 4].Text = list[i].VisitReserveDate.Substring(5) + " " + list[i].visitUserName;
                    if (!list[i].visitUserName2.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[rowIndex, 4].Text += " , " + list[i].visitUserName2;
                    }
                    
                }
                else if (COlumnIndex == 2)
                {
                    SSList.ActiveSheet.Cells[rowIndex, 5].Text = list[i].VisitReserveDate.Substring(5) + " " + list[i].visitUserName;
                    if (!list[i].visitUserName2.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[rowIndex, 5].Text += " , " + list[i].visitUserName2;
                    }
                    
                }
                else if (COlumnIndex == 3)
                {
                    SSList.ActiveSheet.Cells[rowIndex, 6].Text = list[i].VisitReserveDate.Substring(5) + " " + list[i].visitUserName;
                    if (!list[i].visitUserName2.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[rowIndex, 6].Text += " , " + list[i].visitUserName2;
                    }
                }
            }

            long siteId = 0;

            List<HC_SITE_WORKER> workers = hcSiteWorkerRepository.FindWorker("HEALTH_ROLE");

            for (int i=0; i< SSList.ActiveSheet.RowCount; i++)
            {
                
                siteId = SSList.ActiveSheet.Cells[i, 1].Value.To<long>(0);
                nESTIMATE_ID = SSList.ActiveSheet.Cells[i, 11].Value.To<long>(0);

                //List<HC_SITE_WORKER> workers = hcSiteWorkerRepository.FindWorkerByRole(siteId, "HEALTH_ROLE");

                //foreach(HC_SITE_WORKER worker in workers)
                //{
                //    email += worker.NAME + ", mail: " + worker.EMAIL + "\n";
                //}
                //List<string> mailList = GetEmail(workers, siteId);
                List<string> mailList = GetEmail(nESTIMATE_ID);
                if (mailList.Count == 1)
                {
                    SSList.ActiveSheet.Cells[i, 7].Text = mailList[0];
                }
                else if (mailList.Count > 1)
                {
                    SSList.ActiveSheet.Cells[i, 7].Text = mailList[0];
                    SSList.ActiveSheet.Cells[i, 8].Text = mailList[1];
                }

                SSList.ActiveSheet.Rows[i].Height = SSList.ActiveSheet.Rows[i].GetPreferredHeight();
            }
        }

        private List<string> GetEmail_OLD(List<HC_SITE_WORKER> workers, long siteId)
        {
            string strEMail = "";
            List<string> list = new List<string>();
            string email = string.Empty;
            foreach (HC_SITE_WORKER worker in workers)
            {
                if (worker.SITEID == siteId)
                {
                    strEMail = "";
                    if (strEMail!="")
                    {
                        email = worker.NAME + "," + strEMail + "\n";
                        list.Add(email);
                    }
                }

            }
            return list;
        }

        private List<string> GetEmail(long ESTIMATE_ID)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            string strEMail = "";
            List<string> list = new List<string>();
            string email = string.Empty;

            SQL = "";
            SQL = "SELECT NAME,EMAIL FROM HIC_OSHA_CONTRACT_MANAGER ";
            SQL = SQL + ComNum.VBLF + "WHERE ESTIMATE_ID=" + ESTIMATE_ID + " ";
            SQL = SQL + ComNum.VBLF + "  AND WORKER_ROLE='HEALTH_ROLE' ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                for (int i=0; i<dt.Rows.Count; i++)
                {
                    strEMail = dt.Rows[0]["EMAIL"].ToString().Trim();
                    if (strEMail!="")
                    {
                        email = dt.Rows[0]["NAME"].ToString().Trim() + "," + strEMail + "\n";
                        list.Add(email);
                    }
                }
            }
            dt.Dispose();
            dt = null;

            return list;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SSList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader && e.Column == 0)
            {
                bool isCHecked = false;
                if(Convert.ToBoolean(SSList.ActiveSheet.ColumnHeader.Cells[0,0].Value) == true)
                {
              
                    SSList.ActiveSheet.ColumnHeader.Cells[0, 0].Value = false;
                }
                else
                {
                    isCHecked = true;
                    SSList.ActiveSheet.ColumnHeader.Cells[0, 0].Value = true;
                }
             

                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    SSList.ActiveSheet.Cells[i, 0].Value = isCHecked;
                }
            }
        }

        private void btnPRint_Click(object sender, EventArgs e)
        {
            if(TxtDocNumber.GetValue().IsNullOrEmpty())
            {
                MessageUtil.Alert("문서번호를 입력하세요");
                return;
            }
            Print(false);
        }
        private Dictionary<long, ChargeEmailModel> Print(bool isPdf, string exportPath = "")
        {
            Dictionary<long, ChargeEmailModel> exportPdfList = new Dictionary<long, ChargeEmailModel>();

            string month = CboMonth.GetValue();
            int checkedCount = 0;
            for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
            {
                if (Convert.ToBoolean(SSList.ActiveSheet.Cells[i, 0].Value))
                {
                    checkedCount += 1;
                    long siteId = SSList.ActiveSheet.Cells[i, 1].Value.To<long>(0);
                    string siteName = SSList.ActiveSheet.Cells[i, 2].Value.ToString();
                   
                    List<VisitDocumentModel> list = visitDocumentService.VisitDocumentRepository.FindScheduleByMonth(month, siteId);
                    ChargeEmailModel chargeEmailModel = visitDocumentForm.Print(list, month, DateUtil.DateTimeToStrig(DtpDate.Value, DateTimeType.YYYY_MM_DD), siteId, siteName, isPdf, TxtDocNumber.Text, exportPath);
                    exportPdfList.Add(siteId, chargeEmailModel);
                }
            }
            if(checkedCount == 0)
            {
                MessageUtil.Alert(" 사업장을 선택하세요 ");
            }

            return exportPdfList;
        }
        private void btnSendMail_Click(object sender, EventArgs e)
        {
            if (TxtDocNumber.GetValue().IsNullOrEmpty())
            {
                MessageUtil.Alert("문서번호를 입력하세요");
                return;
            }
            try
            {
                bool isStop = false;
                Cursor.Current = Cursors.WaitCursor;
                HcSiteWorkerRepository hcSiteWorkerRepository = new HcSiteWorkerRepository();
                Dictionary<long, List<string>> receiverMailList = new Dictionary<long, List<string>>();
                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    if (Convert.ToBoolean(SSList.ActiveSheet.Cells[i, 0].Value))
                    {
                        int mailCount = 0;
                        long siteId = SSList.ActiveSheet.Cells[i, 1].Value.To<long>(0);
                        string siteName = SSList.ActiveSheet.Cells[i, 2].Value.ToString();
                        //List<HC_SITE_WORKER> list = hcSiteWorkerRepository.FindWorkerByRole(siteId, "HEALTH_ROLE");
                        //if (list.Count > 0)
                        //{
                        //    List<string> mailList = new List<string>();
                        //    foreach (HC_SITE_WORKER worker in list)
                        //    {
                        //        if (worker.EMAIL.NotEmpty())
                        //        {
                        //            mailList.Add(worker.EMAIL);
                        //            mailCount++;
                        //        }
                        //    }
                        //    receiverMailList.Add(siteId, mailList);
                        //}
                        List<string> mailList = new List<string>();
                        string email = SSList.ActiveSheet.Cells[i, 7].Text;
                        var emailArray = email.Split(',');
                        if(emailArray.Length == 2)
                        {
                            if (emailArray[1].NotEmpty())
                            {
                                if (IsValidEmail(emailArray[1]))
                                {
                                    mailList.Add(emailArray[1]);
                                    mailCount++;
                                }
                               
                            }
                        }
                        string email2 = SSList.ActiveSheet.Cells[i, 8].Text;
                        emailArray = email2.Split(',');
                        if (emailArray.Length == 2)
                        {
                            if (emailArray[1].NotEmpty())
                            {
                                if (IsValidEmail(emailArray[1]))
                                {
                                    mailList.Add(emailArray[1]);
                                    mailCount++;
                                }

                            }
                        }
                        receiverMailList.Add(siteId, mailList);
                        //사업장 메일이 없을경우 중단
                        if (mailCount == 0)
                        {
                            isStop = true;
                            MessageUtil.Alert(siteName + "보건 담당자 이메일이 없습니다.  메일발송이 취소되었습니다. 근로자 등록에서 보건담당자를 지정하세요");
                            break;
                        }
                    }
                }//for
                if (isStop)
                {
                    return;
                }
                if (receiverMailList.Count > 0)
                {
                    Dictionary<long, ChargeEmailModel> exportPdfList = new Dictionary<long, ChargeEmailModel>();
                    exportPdfList = Print(true);
                    Thread.Sleep(3000);
                    SendMail(receiverMailList, exportPdfList);
                }
                else
                {
                    MessageUtil.Alert("사업장을 체크하세요");
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                MessageUtil.Alert(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
      
        }
        public bool IsValidEmail(string email)
        {
            bool valid = Regex.IsMatch(email, @"[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?");
            return valid;
        }
        public void SendMail(Dictionary<long, List<string>> receiverMailList, Dictionary<long, ChargeEmailModel> exportPdfList)
        {
            HcCodeService codeService = new HcCodeService();
            string mail = codeService.FindActiveCodeByGroupAndCode("OSHA_MANAGER", "mail", "OSHA").CodeName;
            string password = codeService.FindActiveCodeByGroupAndCode("OSHA_MANAGER", "mail_password", "OSHA").CodeName;
            string month = CboMonth.GetValue().Substring(5, 2);

            if(month.Substring(0,1) == "0")
            {
                month = month.Substring(1, 1);
            }
            month = month + "월";

            MailUtil mailUtil = new MailUtil(mail, password);
            foreach (KeyValuePair<long, List<string>> item in receiverMailList)
            {
                try
                {
                    List<string> mailList = receiverMailList[item.Key];
                    foreach (string emailAddress in mailList)
                    {
                        ChargeEmailModel chargeEmailModel;
                        exportPdfList.TryGetValue(item.Key, out chargeEmailModel);
                        if (chargeEmailModel != null)
                        {
                            List<string> recever = new List<string>();
                            recever.Add(emailAddress);

                            List<string> attach = new List<string>();
                            attach.Add(chargeEmailModel.PdfFileName);

                            mailUtil.Subject = chargeEmailModel.Title;
                            mailUtil.Body = chargeEmailModel.Content;
                            mailUtil.Attachments = attach;
                            mailUtil.ReciverMailSddress = recever;
                            mailUtil.SenderMailAddress = mail;
                            mailUtil.IsBodyHtml = true;
                            mailUtil.SendMaill();

                            //  메일발송 저장
                            HIC_OSHA_MAIL_SEND sendMail = new HIC_OSHA_MAIL_SEND
                            {
                                SITE_ID = item.Key,
                                SEND_TYPE = MAIL_TYPE.ToString(),
                                SEND_USER = clsType.User.Sabun
                            };

                            int res = hicMailRepository.Insert(sendMail);

                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }
            MessageUtil.Info("메일을 발송하였습니다");
            mailUtil.Dispose();
        }
        private void VIsitDocumentBatchForm_Load(object sender, EventArgs e)
        {
            DateTime dateTime = codeService.CurrentDate;
            dateTime = dateTime.AddMonths(1);
            for (int i = 0; i <= 24; i++)
            {
                CboMonth.Items.Add(dateTime.AddMonths(-i).ToString("yyyy-MM"));
            }
            CboMonth.SelectedIndex = 0;

            BtnSearch.PerformClick();
        }

        private void BtnExportPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (TxtDocNumber.GetValue().IsNullOrEmpty())
                {
                    MessageUtil.Alert("문서번호를 입력하세요");
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                //FolderBrowserDialog dialog = new FolderBrowserDialog();
                //DialogResult result = dialog.ShowDialog();
                //if (result == DialogResult.OK)
                //{
                // string path = dialog.SelectedPath;
                string path = @"C:\temp";
                Print(true, path);
                //}
                MessageUtil.Info("PDF 저장 완료");
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                Cursor.Current = Cursors.Default;
               
                MessageUtil.Alert(ex.Message);
            }
        }
    }
}
