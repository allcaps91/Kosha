using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.UserControls;
using ComBase.Mvc.Utils;
using HC.Core.Common.Util;
using HC.Core.Dto;
using HC.Core.Service;
using HC.OSHA.Repository;
using HC_OSHA.form.Charge;
using HC_OSHA.Model.Schedule;

namespace HC_OSHA.form.Visit
{
    /// <summary>
    /// 방문공문 - 방문전, 방문일정 기반으로ㅈㅈㅈㅈ 공문을 발송한다.
    /// </summary>
    public partial class VisitDocument : Form
    {
        private HC_CODE pdfPath = null;
        SpreadPrint print = null;
        public VisitDocument()
        {
            InitializeComponent();
            HcCodeService codeService = new HcCodeService();
            pdfPath = codeService.FindActiveCodeByGroupAndCode("PDF_PATH", "OSHA_ESTIMATE", "OSHA");
           
        }
        public void clear()
        {
            ssDoc.ActiveSheet.Cells[28, 2].Value = "-";
            ssDoc.ActiveSheet.Cells[29, 2].Value = "-";
            //간호사
            ssDoc.ActiveSheet.Cells[28, 8].Value = "-";
            ssDoc.ActiveSheet.Cells[29, 8].Value = "-";

            //산업기사ㅣ
            ssDoc.ActiveSheet.Cells[28, 13].Value = "-";
            ssDoc.ActiveSheet.Cells[29, 13].Value = "-";
        }
        public ChargeEmailModel Print(List<VisitDocumentModel> list, string yearAndMonth, string executeDate, long siteId, string siteName, bool isPdf, string docNumber, string exportPath="" )
        {
            ChargeEmailModel chargeEmailModel = new ChargeEmailModel();
            try
            {
                clear();
            
                string year = yearAndMonth.Substring(0, 4);
                string month = yearAndMonth.Substring(5, 2);
                //ssDoc.ActiveSheet.Cells[6, 5].Value = "포성건 " + year + " - " + docNumber + "호";
                ssDoc.ActiveSheet.Cells[6, 5].Value = "대보연 " + year + " - " + docNumber + "호";
                ssDoc.ActiveSheet.Cells[8, 5].Value = executeDate;
                ssDoc.ActiveSheet.Cells[10, 5].Value = siteName + " 대표이사";
                ssDoc.ActiveSheet.Cells[14, 5].Value = year + "년 " + month + "월 " + "보건관리전문기관 방문일정 안내";
                ssDoc.ActiveSheet.Cells[21, 5].Value = "3. 위 관련으로 " + month + "월 일정을 아래와 같이 보내드리오니 직원 여러분의 많은 참여";

                //chargeEmailModel.Title = "포항성모병원 " + year + "년 " + month + "월 보건관리전문기관 방문일정 안내";
                chargeEmailModel.Title = "대한보건환경연구소 " + year + "년 " + month + "월 보건관리전문기관 방문일정 안내";

                chargeEmailModel.Content = "<html><body> 1. 귀사의 무궁한 발전과 무재해를 기원합니다 <br>";
                chargeEmailModel.Content = chargeEmailModel.Content + "2. 관련근거 : 고용노동부 예규 제439호 4조 2항 <br>";
                chargeEmailModel.Content = chargeEmailModel.Content + "3. "+ month + "월 일정을 첨부와 같이 보내드리오니 직원 여러분의 많은 참여 부탁드립니다 ";
                chargeEmailModel.Content = chargeEmailModel.Content + "</body></html>";

                FarPoint.Win.Spread.CellType.ImageCellType dojang = new FarPoint.Win.Spread.CellType.ImageCellType();
                dojang.Style = FarPoint.Win.RenderStyle.Normal;
                ssDoc.ActiveSheet.Cells[35, 19].CellType = dojang;
                ssDoc.ActiveSheet.Cells[35, 19].Value = Image.FromFile(@"C:\PSMHEXE\exenet\Resources\dojang.png");


                FarPoint.Win.Spread.CellType.ImageCellType logo = new FarPoint.Win.Spread.CellType.ImageCellType();
                logo.Style = FarPoint.Win.RenderStyle.Normal;

                ssDoc.ActiveSheet.Cells[1, 1].CellType = logo;
                ssDoc.ActiveSheet.Cells[1, 1].Value = Image.FromFile(@"C:\PSMHEXE\exenet\Resources\logo.png");

                Log.Error(exportPath);
                for (int j = 0; j < list.Count; j++)
                {
                    //string date = DateUtil.stringToDateTime(list[j].VisitReserveDate, ComBase.Controls.DateTimeType.YYYY_MM_DD).ToString("M/d");
                    string date = list[j].VisitReserveDate.Substring(5, 2) + "/" + list[j].VisitReserveDate.Substring(8, 2);
                    string dayOfWeek = DateUtil.ToDayOfWeek(list[j].VisitReserveDate, ComBase.Controls.DateTimeType.YYYY_MM_DD);
                    string day = date + "(" + dayOfWeek.Substring(0, 1) + ")";
                    if (list[j].visitUserRole == "DOCTOR")
                    {
                        if (ssDoc.ActiveSheet.Cells[28, 2].Value.Equals("-"))
                        {
                            ssDoc.ActiveSheet.Cells[28, 2].Value = day + list[j].visitUserName;
                        }
                        else
                        {
                            ssDoc.ActiveSheet.Cells[29, 2].Value = day + list[j].visitUserName;
                        }

                    }
                    if (list[j].visitUserRole == "NURSE")
                    {
                        if (ssDoc.ActiveSheet.Cells[28, 8].Value.Equals("-"))
                        {
                            ssDoc.ActiveSheet.Cells[28, 8].Value = day + list[j].visitUserName;
                        }
                        else
                        {
                            ssDoc.ActiveSheet.Cells[29, 8].Value = day + list[j].visitUserName;

                        }

                    }
                    if (list[j].visitUserRole == "ENGINEER")
                    {
                        if (ssDoc.ActiveSheet.Cells[28, 13].Value.Equals("-"))
                        {
                            ssDoc.ActiveSheet.Cells[28, 13].Value = day + list[j].visitUserName;
                        }
                        else
                        {
                            ssDoc.ActiveSheet.Cells[29, 13].Value = day + list[j].visitUserName;
                        }

                    }
                }
                Log.Error(4);
                print = new SpreadPrint(ssDoc, PrintStyle.STANDARD_APPROVAL);
                if (isPdf)
                {

                    string pdfFileName = "";
                    if (exportPath.NotEmpty())
                    {
                        pdfFileName = exportPath + "\\" + yearAndMonth + "_방문공문_" + siteName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                    }
                    else
                    {
                        pdfFileName = pdfPath.CodeName + "\\" + yearAndMonth + "_방문공문_" + siteName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                    }

                    print.ExportPDF(pdfFileName);
                    chargeEmailModel.PdfFileName = pdfFileName;

                }
                else
                {
                    print.Execute();
                  
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex);

            }


            return chargeEmailModel;

        }
        //public void SendMail(Dictionary<string, string> receiverMailList = null)
        //{
        //    foreach(KeyValuePair<string, string> item in receiverMailList)
        //    {
        //        try
        //        {
        //            string emailAddress = receiverMailList[item.Key];
        //            string pdfFileName;
        //            exportPdfList.TryGetValue(item.Key, out pdfFileName);
        //            if (pdfFileName != null)
        //            {
        //                List<string> recever = new List<string>();
        //                recever.Add(emailAddress);

        //                List<string> attach = new List<string>();
        //                attach.Add(pdfFileName);

        //                mailUtil.Subject = pdfFileName;
        //                mailUtil.Body = "ddd";
        //                mailUtil.Attachments = attach;
        //                mailUtil.ReciverMailSddress = recever;
        //                mailUtil.SenderMailAddress = "faye12005@gmail.com";
        //                mailUtil.SendMaill();
        //            }
                

        //        }
        //        catch(Exception ex)
        //        {
        //            Log.Debug(ex);
        //        }
                
        //    }
        //}
        private void btnPrint_Click(object sender, EventArgs e)
        {
            for(int i=0; i<2; i++)
            {
                ssDoc.ActiveSheet.Cells[21, 5].Value = "3. 위 관련으로 44월 일정을 아래와 같이 보내드리오니 직원 여러분의 많은 참여";

                string siteName = "dd" + i;
                ssDoc.ActiveSheet.Cells[6, 5].Value = siteName;
                ssDoc.ActiveSheet.Cells[28, 2].Value = "1";
                ssDoc.ActiveSheet.Cells[29, 2].Value = "2";
                //간호사
                ssDoc.ActiveSheet.Cells[28, 8].Value = siteName;
                ssDoc.ActiveSheet.Cells[29, 8].Value = siteName;

                //산업기사ㅣ
                ssDoc.ActiveSheet.Cells[28, 13].Value = "1111";
                ssDoc.ActiveSheet.Cells[29, 13].Value = siteName;
                //    ssDoc.ActiveSheet.Cells[7, 3].Text = "22포성건 -  1호";

                SpreadPrint print = new SpreadPrint(ssDoc, PrintStyle.STANDARD_APPROVAL);
                print.Execute();
            }
      
        }


        //private void VisitDocument_Load(object sender, EventArgs e)
        //{
        //    HcCodeService codeService = new HcCodeService();
        //    pdfPath = codeService.FindActiveCodeByGroupAndCode("PDF_PATH", "OSHA_ESTIMATE");
        //    codeService.FindActiveCodeByGroupAndCode("PDF_PATH", "OSHA_ESTIMATE");

        //    string mail = codeService.FindActiveCodeByGroupAndCode("OSHA_MANAGER", "mail").CodeName;
        //    string password = codeService.FindActiveCodeByGroupAndCode("OSHA_MANAGER", "mail_password").CodeName;

        //     mailUtil = new MailUtil(mail, password);
        //}
    }
}
