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
        private HcUserService hcUsersService;
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
            hcUsersService = new HcUserService();
            visitDocumentService = new VisitDocumentService();
            visitDocumentForm = new VisitDocument();
            hcSiteWorkerRepository = new HcSiteWorkerRepository();

            hcUsersRepository = new HcUsersRepository();
            hicMailRepository = new HicMailRepository();

            List<HC_USER> OSHAUsers = hcUsersService.GetOsha();
            CboManager.SetItems(OSHAUsers, "Name", "UserId", "전체", "", AddComboBoxPosition.Top);
            CboManager.SetValue(CommonService.Instance.Session.UserId);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            long nESTIMATE_ID = 0;
            SSList.ActiveSheet.RowCount = 0;
            string month = CboMonth.GetValue();
            string visitUser = CboManager.GetValue();
            List<VisitDocumentModel> list = visitDocumentService.VisitDocumentRepository.FindScheduleByMonth(month);
            VisitDocumentModel addedModel = null;
            int rowIndex = 0;

            int k = 0;
            long nOldSiteId = -1;
            string strList = "";
            string strIDs = "";
            int nMailNotSend = 0;
            bool bOK = false;
            for(int i=0; i<list.Count; i++)
            {
                if (nOldSiteId == -1 || nOldSiteId != list[i].SiteId)
                {
                    // 메일 미전송만 표시할때 모두 전송을 하였으면 표시 안함
                    if (chkMailSend.Checked == true && nMailNotSend == 0) bOK = false;

                    if (bOK == true)
                    {
                        rowIndex = SSList.ActiveSheet.RowCount;
                        SSList.ActiveSheet.Rows.Add(rowIndex, 1);

                        k = Int32.Parse(VB.Pstr(strList, ",", 1));
                        SSList.ActiveSheet.Cells[rowIndex, 1].Text = list[k].SiteId.ToString();
                        SSList.ActiveSheet.Cells[rowIndex, 2].Text = list[k].SiteName;

                        SSList.ActiveSheet.Cells[rowIndex, 12].Text = VB.Pstr(list[k].SENDMAIL, "/", 1);
                        SSList.ActiveSheet.Cells[rowIndex, 13].Text = VB.Pstr(list[k].SENDMAIL, "/", 2);
                        SSList.ActiveSheet.Cells[rowIndex, 14].Value = list[k].ESTIMATE_ID;
                        SSList.ActiveSheet.Cells[rowIndex, 15].Value = strIDs;

                        SSList.ActiveSheet.Cells[rowIndex, 3].Text = list[k].VisitReserveDate.Substring(5) + " " + list[k].visitUserName;
                        if (!list[k].visitUserName2.IsNullOrEmpty())
                        {
                            SSList.ActiveSheet.Cells[rowIndex, 3].Text += " , " + list[k].visitUserName2;
                        }

                        if (VB.Pstr(strList, ",", 2) != "")
                        {
                            k = Int32.Parse(VB.Pstr(strList, ",", 2));
                            SSList.ActiveSheet.Cells[rowIndex, 4].Text = list[k].VisitReserveDate.Substring(5) + " " + list[k].visitUserName;
                            if (!list[k].visitUserName2.IsNullOrEmpty())
                            {
                                SSList.ActiveSheet.Cells[rowIndex, 4].Text += " , " + list[k].visitUserName2;
                            }
                        }

                        if (VB.Pstr(strList, ",", 3) != "")
                        {
                            k = Int32.Parse(VB.Pstr(strList, ",", 3));
                            SSList.ActiveSheet.Cells[rowIndex, 5].Text = list[k].VisitReserveDate.Substring(5) + " " + list[k].visitUserName;
                            if (!list[k].visitUserName2.IsNullOrEmpty())
                            {
                                SSList.ActiveSheet.Cells[rowIndex, 5].Text += " , " + list[k].visitUserName2;
                            }
                        }

                        if (VB.Pstr(strList, ",", 4) != "")
                        {
                            k = Int32.Parse(VB.Pstr(strList, ",", 4));
                            SSList.ActiveSheet.Cells[rowIndex, 6].Text = list[k].VisitReserveDate.Substring(5) + " " + list[k].visitUserName;
                            if (!list[k].visitUserName2.IsNullOrEmpty())
                            {
                                SSList.ActiveSheet.Cells[rowIndex, 6].Text += " , " + list[k].visitUserName2;
                            }
                        }

                        if (VB.Pstr(strList, ",", 5) != "")
                        {
                            k = Int32.Parse(VB.Pstr(strList, ",", 5));
                            SSList.ActiveSheet.Cells[rowIndex, 7].Text = list[k].VisitReserveDate.Substring(5) + " " + list[k].visitUserName;
                            if (!list[k].visitUserName2.IsNullOrEmpty())
                            {
                                SSList.ActiveSheet.Cells[rowIndex, 7].Text += " , " + list[k].visitUserName2;
                            }
                        }

                    }

                    strList = "";
                    bOK = false;
                    nOldSiteId = list[i].SiteId;
                    strIDs = "";
                    nMailNotSend = 0;
                }

                strList = strList + i.ToString() + ",";
                if (visitUser=="" || visitUser == list[i].visitUserId) bOK = true;

                //메일 미전송 건수를 누적
                if (list[i].SENDMAIL.IsNullOrEmpty()) nMailNotSend++;

                if (strIDs=="")
                {
                    strIDs = list[i].Id.ToString();
                }
                else
                {
                    strIDs += "," + list[i].Id.ToString();
                }
            }

            // 메일 미전송만 표시할때 모두 전송을 하였으면 표시 안함
            if (chkMailSend.Checked == true && nMailNotSend == 0) bOK = false;

            if (bOK == true)
            {
                rowIndex = SSList.ActiveSheet.RowCount;
                SSList.ActiveSheet.Rows.Add(rowIndex, 1);

                k = Int32.Parse(VB.Pstr(strList, ",", 1));
                SSList.ActiveSheet.Cells[rowIndex, 1].Text = list[k].SiteId.ToString();
                SSList.ActiveSheet.Cells[rowIndex, 2].Text = list[k].SiteName;

                SSList.ActiveSheet.Cells[rowIndex, 12].Text = VB.Pstr(list[k].SENDMAIL, "/", 1);
                SSList.ActiveSheet.Cells[rowIndex, 13].Text = VB.Pstr(list[k].SENDMAIL, "/", 2);
                SSList.ActiveSheet.Cells[rowIndex, 14].Value = list[k].ESTIMATE_ID;
                SSList.ActiveSheet.Cells[rowIndex, 15].Value = strIDs;

                SSList.ActiveSheet.Cells[rowIndex, 3].Text = list[k].VisitReserveDate.Substring(5) + " " + list[k].visitUserName;
                if (!list[k].visitUserName2.IsNullOrEmpty())
                {
                    SSList.ActiveSheet.Cells[rowIndex, 3].Text += " , " + list[k].visitUserName2;
                }

                if (VB.Pstr(strList, ",", 2) != "")
                {
                    k = Int32.Parse(VB.Pstr(strList, ",", 2));
                    SSList.ActiveSheet.Cells[rowIndex, 4].Text = list[k].VisitReserveDate.Substring(5) + " " + list[k].visitUserName;
                    if (!list[k].visitUserName2.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[rowIndex, 4].Text += " , " + list[k].visitUserName2;
                    }
                }

                if (VB.Pstr(strList, ",", 3) != "")
                {
                    k = Int32.Parse(VB.Pstr(strList, ",", 3));
                    SSList.ActiveSheet.Cells[rowIndex, 5].Text = list[k].VisitReserveDate.Substring(5) + " " + list[k].visitUserName;
                    if (!list[k].visitUserName2.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[rowIndex, 5].Text += " , " + list[k].visitUserName2;
                    }
                }

                if (VB.Pstr(strList, ",", 4) != "")
                {
                    k = Int32.Parse(VB.Pstr(strList, ",", 4));
                    SSList.ActiveSheet.Cells[rowIndex, 6].Text = list[k].VisitReserveDate.Substring(5) + " " + list[k].visitUserName;
                    if (!list[k].visitUserName2.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[rowIndex, 6].Text += " , " + list[k].visitUserName2;
                    }
                }

                if (VB.Pstr(strList, ",", 5) != "")
                {
                    k = Int32.Parse(VB.Pstr(strList, ",", 5));
                    SSList.ActiveSheet.Cells[rowIndex, 7].Text = list[k].VisitReserveDate.Substring(5) + " " + list[k].visitUserName;
                    if (!list[k].visitUserName2.IsNullOrEmpty())
                    {
                        SSList.ActiveSheet.Cells[rowIndex, 7].Text += " , " + list[k].visitUserName2;
                    }
                }

            }

            long siteId = 0;

            List<HC_SITE_WORKER> workers = hcSiteWorkerRepository.FindWorker("HEALTH_ROLE");

            for (int i=0; i< SSList.ActiveSheet.RowCount; i++)
            {
                
                siteId = SSList.ActiveSheet.Cells[i, 1].Value.To<long>(0);
                nESTIMATE_ID = SSList.ActiveSheet.Cells[i, 14].Value.To<long>(0);

                List<string> mailList = GetEmail(nESTIMATE_ID);
                if (mailList.Count > 0) SSList.ActiveSheet.Cells[i, 8].Text = mailList[0];
                if (mailList.Count > 1) SSList.ActiveSheet.Cells[i, 9].Text = mailList[1];
                if (mailList.Count > 2) SSList.ActiveSheet.Cells[i, 10].Text = mailList[2];
                if (mailList.Count > 3) SSList.ActiveSheet.Cells[i, 11].Text = mailList[3];

                //SSList.ActiveSheet.Rows[i].Height = SSList.ActiveSheet.Rows[i].GetPreferredHeight();
            }
            BtnSearch.Text = "검색";
            if (SSList.ActiveSheet.RowCount > 0) BtnSearch.Text = "검색(" + SSList.ActiveSheet.RowCount + "건)";
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
            SQL = SQL + ComNum.VBLF + "  AND (EMAILSEND='Y' OR EMAILSEND='y') ";
            SQL = SQL + ComNum.VBLF + "  AND EMAIL IS NOT NULL ";
            SQL = SQL + ComNum.VBLF + "  AND SWLicense = '" + clsType.HosInfo.SwLicense + "' ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                for (int i=0; i<dt.Rows.Count; i++)
                {
                    strEMail = dt.Rows[i]["EMAIL"].ToString().Trim();
                    if (strEMail!="")
                    {
                        email = dt.Rows[i]["NAME"].ToString().Trim() + "," + strEMail + "\n";
                        list.Add(email);
                    }
                }
            }
            dt.Dispose();
            dt = null;

            return list;
        }

        private void UPDATE_MAILSEND(string strIDs, string strMailSend)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE HIC_OSHA_SCHEDULE  SET ";
                SQL = SQL + ComNum.VBLF + "        SENDMAIL = '" + strMailSend + "' ";
                SQL = SQL + ComNum.VBLF + "WHERE ID IN (" + strIDs + ") ";
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                return;
            }
            catch (Exception ex)
            {
                MessageUtil.Alert("메일 전송완료 저장 오류");
            }
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
                    Thread.Sleep(2000);
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
                int mailCount = 0;
                int mailTot = 0;
                long siteId = 0;
                string siteName = "";
                string email = "";

                Cursor.Current = Cursors.WaitCursor;
                HcSiteWorkerRepository hcSiteWorkerRepository = new HcSiteWorkerRepository();
                Dictionary<long, List<string>> receiverMailList = new Dictionary<long, List<string>>();
                for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    if (Convert.ToBoolean(SSList.ActiveSheet.Cells[i, 0].Value))
                    {
                        mailCount = 0;
                        mailTot = 0;
                        siteId = SSList.ActiveSheet.Cells[i, 1].Value.To<long>(0);
                        siteName = SSList.ActiveSheet.Cells[i, 2].Value.ToString();
                        List<string> mailList = new List<string>();

                        // 전송할 메일 목록을 보관(최대 4개)
                        for (int j = 0; j < 4; j++)
                        {
                            email = SSList.ActiveSheet.Cells[i, 7 + j].Text.Trim();
                            if (email != "")
                            {
                                mailTot++;
                                if (VB.Pstr(email, ",", 2) != "")
                                {
                                    if (IsValidEmail(VB.Pstr(email, ",", 2)))
                                    {
                                        mailList.Add(VB.Pstr(email, ",", 2));
                                        mailCount++;
                                    }
                                }
                            }
                        }

                        //정상적인 메일계정이면 전송목록에 추가함
                        if (mailTot==mailCount && mailCount>0)
                        {
                            receiverMailList.Add(siteId, mailList);
                        }
                    }
                }

                if (receiverMailList.Count > 0)
                {
                    Dictionary<long, ChargeEmailModel> exportPdfList = new Dictionary<long, ChargeEmailModel>();
                    exportPdfList = Print(true);
                    Thread.Sleep(2000);
                    SendMail(receiverMailList, exportPdfList);
                }
                else
                {
                    MessageUtil.Alert("전송할 메일이 없습니다.");
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
            long siteId = 0;
            string strIDs = "";
            string strMailSend = "";

            if (month.Substring(0,1) == "0")
            {
                month = month.Substring(1, 1);
            }
            month = month + "월";
            strMailSend = DateTime.Now.ToString("yyyy-MM-dd") + "/" + clsType.User.JobName;

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
                            //교육자료등 첨부
                            txtAttach.Text = txtAttach.Text.Trim();
                            if (txtAttach.Text != "")
                            {
                                if (VB.Pstr(txtAttach.Text, ";", 1).Trim() != "") attach.Add(VB.Pstr(txtAttach.Text, ";", 1).Trim());
                                if (VB.Pstr(txtAttach.Text, ";", 2).Trim() != "") attach.Add(VB.Pstr(txtAttach.Text, ";", 2).Trim());
                                if (VB.Pstr(txtAttach.Text, ";", 3).Trim() != "") attach.Add(VB.Pstr(txtAttach.Text, ";", 3).Trim());
                                if (VB.Pstr(txtAttach.Text, ";", 4).Trim() != "") attach.Add(VB.Pstr(txtAttach.Text, ";", 4).Trim());
                                if (VB.Pstr(txtAttach.Text, ";", 5).Trim() != "") attach.Add(VB.Pstr(txtAttach.Text, ";", 5).Trim());
                            }
                            mailUtil.Subject = chargeEmailModel.Title;
                            mailUtil.Body = chargeEmailModel.Content;
                            mailUtil.Attachments = attach;
                            mailUtil.ReciverMailSddress = recever;
                            mailUtil.SenderMailAddress = mail;
                            mailUtil.SenderMail = VB.Pstr(clsType.HosInfo.SwLicInfo, "{}", 2);
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
                        // 전송완료 UPDATE
                        for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                        {
                            if (Convert.ToBoolean(SSList.ActiveSheet.Cells[i, 0].Value))
                            {
                                siteId = SSList.ActiveSheet.Cells[i, 1].Value.To<long>(0);
                                if (siteId == item.Key)
                                {
                                    strIDs = SSList.ActiveSheet.Cells[i, 15].Text.Trim();
                                    SSList.ActiveSheet.Cells[i, 12].Value = VB.Pstr(strMailSend, "/", 1); //발송일자
                                    SSList.ActiveSheet.Cells[i, 13].Value = VB.Pstr(strMailSend, "/", 2); //발송자
                                    UPDATE_MAILSEND(strIDs, strMailSend);
                                    break;
                                }
                            }
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
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string path = dialog.SelectedPath;
                    Print(true, path);
                    Thread.Sleep(2000);
                    MessageUtil.Info("PDF 저장 완료");
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                Cursor.Current = Cursors.Default;
               
                MessageUtil.Alert(ex.Message);
            }
        }

        private void btnAttach_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtAttach.Text += dialog.FileName + ";";
            }
        }
    }
}
