using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC.OSHA.Repository;
using HC.OSHA.Service;
using HC.Core.Dto;
using HC.Core.Repository;
using HC.Core.Service;
using HC_OSHA.form.Charge;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using HC_Core;

namespace HC_OSHA
{
    public partial class ChargeDocumentBatchForm : CommonForm
    {
        private HcOshaChargeService hcOshaChargeService;
        private HcSiteWorkerRepository hcSiteWorkerRepository;

        private HicMailRepository hicMailRepository;

        private MailType MAIL_TYPE = MailType.CHARGE;

        public ChargeDocumentBatchForm()
        {
            InitializeComponent();
            hcOshaChargeService = new HcOshaChargeService();
            hicMailRepository = new HicMailRepository();

            hcSiteWorkerRepository = new HcSiteWorkerRepository();
        }

        private void ChargeDocumentBatchForm_Load(object sender, EventArgs e)
        {
            DateTime dateTime = codeService.CurrentDate;
            for (int i = 0; i <= 24; i++)
            {
                CboMonth.Items.Add(dateTime.AddMonths(-i).ToString("yyyy-MM"));
            }
            CboMonth.SelectedIndex = 0;


            SSList.Initialize(new SpreadOption() { IsRowSelectColor = false });
            SSList.AddColumnCheckBox("", "", 30, new CheckBoxBooleanCellType());
            SSList.AddColumnText("발생일자", nameof(ChargeDocumentModel.BDATE), 94, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Center });
            SSList.AddColumnText("미수번호", nameof(ChargeDocumentModel.WRTNO), 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Center });
            SSList.AddColumnText("회사코드", nameof(ChargeDocumentModel.LTDCODE), 85, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true });
            SSList.AddColumnText("회사명", nameof(ChargeDocumentModel.SITENAME), 171, IsReadOnly.N, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SSList.AddColumnText("계정", nameof(ChargeDocumentModel.GEACODE), 47, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("변동계정", nameof(ChargeDocumentModel.), 100, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("금액", nameof(ChargeDocumentModel.SLIPAMT), 98, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            //SSList.AddColumnText("작업자", nameof(ChargeDocumentModel.NAME), 71, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("참고사항", nameof(ChargeDocumentModel.REMARK), 250, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("담당자", nameof(ChargeDocumentModel.DAMNAME), 66, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("이메일", nameof(ChargeDocumentModel.EMAIL), 225, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("이메일2", nameof(ChargeDocumentModel.EMAIL2), 225, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("메일 방송일", nameof(ChargeDocumentModel.SENDDATE), 120, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });
            SSList.AddColumnText("발송자", nameof(ChargeDocumentModel.SENDNAME), 80, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false });

            //            SSList.AddColumnText("담당자", "", 250, IsReadOnly.N, new SpreadCellTypeOption { IsSort = false, WordWrap=true, IsMulti=true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            Search();
        }

        private void Search()
        {
            string month = CboMonth.GetValue();
            string startDate = CboMonth.GetValue() + "-01";
            string endDate = CboMonth.GetValue() + "-" + DateTime.DaysInMonth(CboMonth.Text.Substring(0, 4).To<int>(0), CboMonth.Text.Substring(5, 2).To<int>(0));
            List<ChargeDocumentModel> list = hcOshaChargeService.HcOshaChargeRepository.FindChargeDocument(startDate, endDate);
            SSList.SetDataSource(list);

           // List<HC_SITE_WORKER> workers = hcSiteWorkerRepository.FindWorker("HEALTH_ROLE");

            long siteId = 0;
            for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
            {
                ChargeDocumentModel model = SSList.GetRowData(i) as ChargeDocumentModel;
                siteId = SSList.ActiveSheet.Cells[i, 1].Value.To<long>(0);
              
                //List<HC_SITE_WORKER> workers = hcSiteWorkerRepository.FindWorkerByRole(siteId, "HEALTH_ROLE");

                //foreach(HC_SITE_WORKER worker in workers)
                //{
                //    email += worker.NAME + ", mail: " + worker.EMAIL + "\n";
                //}
              //  SSList.ActiveSheet.Cells[i, 9].Text =  list[i].DAMNAME + " ( " + list[i].MAIL + " )";   //GetEmail(workers, model.LTDCODE);
                SSList.ActiveSheet.Rows[i].Height = SSList.ActiveSheet.Rows[i].GetPreferredHeight();
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void btnPRint_Click(object sender, EventArgs e)
        {
            if (TxtDocNumber.GetValue().IsNullOrEmpty())
            {
                MessageUtil.Alert("문서번호를 입력하세요");
                return;
            }
            Print(false);
        }
        private Dictionary<long, ChargeEmailModel> Print(bool isPdf, string exportPath = "")
        {
            Dictionary<long, ChargeEmailModel> exportPdfList = new Dictionary<long, ChargeEmailModel>();
            ChargeDocumentTest chargeDocumentTest = new ChargeDocumentTest();
            string month = CboMonth.GetValue();
            int checkedCount = 0;
            for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
            {
                if (Convert.ToBoolean(SSList.ActiveSheet.Cells[i, 0].Value))
                {
                    checkedCount += 1;

                    ChargeDocumentModel model =  SSList.GetRowData(i) as ChargeDocumentModel;

                    ChargeEmailModel chargeEmailModel = chargeDocumentTest.Print(model.SLIPAMT, month, DateUtil.DateTimeToStrig(DtpDate.Value, DateTimeType.YYYY_MM_DD), model.SITENAME, isPdf, TxtDocNumber.Text, exportPath);
                    exportPdfList.Add(model.LTDCODE, chargeEmailModel);

                    //  chargeDocument.Show();
                }
            }
            if (checkedCount == 0)
            {
                MessageUtil.Alert(" 사업장을 선택하세요 ");
            }

            return exportPdfList;
        }

        Thread mailThread = null;
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
                        ChargeDocumentModel model = SSList.GetRowData(i) as ChargeDocumentModel;
                        //  int mailCount = 0;
                        // List<HC_SITE_WORKER> list = hcSiteWorkerRepository.FindWorkerByRole(model.LTDCODE, "HEALTH_ROLE");
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
                        //    receiverMailList.Add(model.LTDCODE, mailList);
                        //}
                      
                        string email = model.EMAIL;
                        if (email.NotEmpty())
                        {
                            List<string> mailList = new List<string>();

                            if (IsValidEmail(email))
                            {
                                mailList.Add(email);
                            }

                            string email2 = model.EMAIL2;
                            if (email2.NotEmpty())
                            {
                                if (IsValidEmail(email2))
                                {
                                    mailList.Add(email2);
                                }
                            }
                           
                            receiverMailList.Add(model.LTDCODE, mailList);
                        }
                        else
                        {
                            isStop = true;
                            MessageUtil.Alert(model.SITENAME + "보건 담당자 이메일이 없습니다.  메일발송이 취소되었습니다. ");
                            break;
                            //사업장 메일이 없을경우 중단
                            //if (mailCount == 0)
                            //{
                            //    isStop = true;
                            //    MessageUtil.Alert(model.SITENAME + "보건 담당자 이메일이 없습니다.  메일발송이 취소되었습니다. 근로자 등록에서 보건담당자를 지정하세요");
                            //    break;
                            //}
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

                    PnlMail.Visible = true;
                    LblTotal.Text = string.Concat(0, " / ", receiverMailList.Count);
                    progressBar1.Maximum = receiverMailList.Count;
                    progressBar1.Value = 0;

                    mailThread = new Thread(new ThreadStart(() =>
                    {
                        //ThreadMailCall(receiverMailList, exportPdfList);
                        SendMail(receiverMailList, exportPdfList);
                    }));
                    mailThread.IsBackground = true;
                    mailThread.Start();

                    //for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
                    //{
                    //    if (Convert.ToBoolean(SSList.ActiveSheet.Cells[i, 0].Value))
                    //    {
                    //        ChargeDocumentModel model = SSList.GetRowData(i) as ChargeDocumentModel;

                    //        HIC_OSHA_MAIL_SEND sendMail = new HIC_OSHA_MAIL_SEND
                    //        {
                    //            SITE_ID = model.LTDCODE,
                    //            SEND_TYPE = MAIL_TYPE.ToString(),
                    //            SEND_USER = clsType.User.Sabun,
                    //            WRTNO = model.WRTNO
                    //        };

                    //        int res = hicMailRepository.Insert(sendMail);
                    //    }
                    //}

                    //GetMailDate();
                }
            }
            catch(Exception ex)
            {
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

            //mail = "kayakiller@gmail.com";
            //password = "cldnql1!@";
            MailUtil mailUtil = new MailUtil(mail, password);
            //string month = CboMonth.GetValue().Substring(5, 2);
            //if (month.Substring(0, 1) == "0")
            //{
            //    month = month.Substring(1, 1);
            //}
            //month = month + "월";
            // string title = "포항성모병원 보건관리전문 " + month + " 안내";
            //string body = title + " 첨부파일을 확인해주세요";

            int i = 0;
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
                            //recever.Add("kil0207@naver.com");

                            List<string> attach = new List<string>();
                            attach.Add(chargeEmailModel.PdfFileName);

                            mailUtil.Subject = chargeEmailModel.Title;
                            mailUtil.Body = chargeEmailModel.Content;
                            mailUtil.Attachments = attach;
                            mailUtil.ReciverMailSddress = recever;
                            mailUtil.SenderMailAddress = mail;
                            mailUtil.IsBodyHtml = true;
                            mailUtil.SendMaill();
                        }
                    }
                }
                catch (Exception ex)
                {
                    mailThread.Abort();
                    mailThread.Join();
                    PnlMail.Visible = false;
                    Log.Error(ex);
                    return;
                }

                i++;
                progressBar1.InvokeUIRequired(() => { progressBar1.Value = i; });
                LblTotal.InvokeUIRequired(() => { LblTotal.Text = string.Concat(i, " / ", receiverMailList.Count);});
            }

            try
            {
                mailUtil.Dispose();
                this.InvokeUIRequired(() => { MessageUtil.Info("메일을 발송하였습니다"); });
                PnlMail.InvokeUIRequired(() => { PnlMail.Visible = false; });

                for (i = 0; i < SSList.ActiveSheet.RowCount; i++)
                {
                    if (Convert.ToBoolean(SSList.ActiveSheet.Cells[i, 0].Value))
                    {
                        ChargeDocumentModel model = SSList.GetRowData(i) as ChargeDocumentModel;

                        HIC_OSHA_MAIL_SEND sendMail = new HIC_OSHA_MAIL_SEND
                        {
                            SITE_ID = model.LTDCODE,
                            SEND_TYPE = MAIL_TYPE.ToString(),
                            SEND_USER = clsType.User.Sabun,
                            WRTNO = model.WRTNO
                        };

                        int res = hicMailRepository.Insert(sendMail);
                    }
                }
            }
            catch (Exception ex)
            {
                mailThread.Abort();
                mailThread.Join();
                PnlMail.Visible = false;
                Log.Error(ex);
            }
        }

        private void BtnExportPdf_Click(object sender, EventArgs e)
        {

            if (TxtDocNumber.GetValue().IsNullOrEmpty())
            {
                MessageUtil.Alert("문서번호를 입력하세요");
                return;
            }
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string path = dialog.SelectedPath;
                    Print(true, path);
                }

            }
            catch(Exception ex)
            {
                Log.Error(ex);
                Cursor.Current = Cursors.Default;
                MessageUtil.Alert(ex.Message);
            }
            
        }
    }


    static class InvokeExtensions
    {
        public static void InvokeUIRequired(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }
    }
}
