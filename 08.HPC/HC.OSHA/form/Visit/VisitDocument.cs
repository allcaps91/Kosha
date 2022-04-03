using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
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
    /// 방문공문 - 방문전, 방문일정 기반으로 공문을 발송한다.
    /// </summary>
    public partial class VisitDocument : Form
    {
        private HC_CODE pdfPath = null;
        int[] nP1 = new int[8] { 0, 0, 0, 0, 0, 0,0,0 };
        int[] nP2 = new int[8] { 0, 0, 0, 0, 0, 0,0,0 };
        private HC_CODE excelPath = null;
        SpreadPrint print = null;

        public VisitDocument()
        {
            InitializeComponent();
            HcCodeService codeService = new HcCodeService();
            pdfPath = codeService.FindActiveCodeByGroupAndCode("PDF_PATH", "OSHA_ESTIMATE", "OSHA");
            excelPath = codeService.FindActiveCodeByGroupAndCode("EXCEL_PATH", "OSHA_ESTIMATE", "OSHA");
            OpenExcelFile();
        }

        public bool OpenExcelFile()
        {
            string s = "";
            ssDoc.ActiveSheet.RowCount = 0;
            string fileName = @"C:\HealthSoft\엑셀서식\일정공문양식.xlsx";
            //FileInfo생성
            FileInfo fi = new FileInfo(fileName);
            //FileInfo.Exists로 파일 존재유무 확인 "
            if (fi.Exists)
            {
                ssDoc.ActiveSheet.OpenExcel(fileName, 0);
                Thread.Sleep(200);
            }
            else
            {
                return false;
            }

            string str = "";
            int nMaxRows = 0;
            int nMaxCols = 0;

            //RowCount 설정
            for (int i = 100; i > 5; i--)
            {
                str = "";
                for (int j = 0; j < 20; j++)
                {
                    str += ssDoc.ActiveSheet.Cells[i, j].Text.Trim();
                }
                if (str != "")
                {
                    nMaxRows = i + 1;
                    break;
                }
            }

            //ColumnCount 설정
            for (int i = 100; i > 5; i--)
            {
                str = "";
                for (int j = 0; j < nMaxRows; j++)
                {
                    str += ssDoc.ActiveSheet.Cells[j, i].Text.Trim();
                }
                if (str != "")
                {
                    nMaxCols = i + 1;
                    break;
                }
            }

            //구하지 못하였으면 오류
            if (nMaxRows == 0 || nMaxCols == 0) { return false; }

            ssDoc.ActiveSheet.RowCount = nMaxRows;
            ssDoc.ActiveSheet.ColumnCount = nMaxCols;
            ssDoc.ActiveSheet.ColumnHeader.RowCount = 0;
            ssDoc.ActiveSheet.RowHeader.ColumnCount = 0;

            //일정공문의 글자위치를 저장
            for (int i = 0; i < ssDoc.ActiveSheet.RowCount; i++)
            {
                for (int c = 0; c < ssDoc.ActiveSheet.ColumnCount; c++)
                {
                    if (ssDoc.ActiveSheet.Cells[i, c].Value != null)
                    {
                        if (ssDoc.ActiveSheet.Cells[i, c].Value.Equals("~{수신}")) { nP1[0] = i; nP2[0] = c; }
                        if (ssDoc.ActiveSheet.Cells[i, c].Value.Equals("~{제목}")) { nP1[1] = i; nP2[1] = c; }
                        if (ssDoc.ActiveSheet.Cells[i, c].Value.Equals("~{의사}")) { nP1[2] = i; nP2[2] = c; }
                        if (ssDoc.ActiveSheet.Cells[i, c].Value.Equals("~{간호사}")) { nP1[3] = i; nP2[3] = c; }
                        if (ssDoc.ActiveSheet.Cells[i, c].Value.Equals("~{산업위생}")) { nP1[4] = i; nP2[4] = c; }
                        if (ssDoc.ActiveSheet.Cells[i, c].Value.Equals("~{문서번호}")) { nP1[5] = i; nP2[5] = c; }
                        if (ssDoc.ActiveSheet.Cells[i, c].Value.Equals("~{방문장소}")) { nP1[6] = i; nP2[6] = c; }
                    }
                }
            }
            return true;
        }
        public void clear()
        {
            //의사
            ssDoc.ActiveSheet.Cells[nP1[2], nP2[2]].Value = "-";
            //간호사
            ssDoc.ActiveSheet.Cells[nP1[3], nP2[3]].Value = "-";
            //산업기사ㅣ
            ssDoc.ActiveSheet.Cells[nP1[4], nP2[4]].Value = "-";
            //방문장소
            ssDoc.ActiveSheet.Cells[nP1[6], nP2[6]].Value = "";
        }

        public ChargeEmailModel Print(List<VisitDocumentModel> list, string yearAndMonth, string executeDate, long siteId, string siteName, bool isPdf, string docNumber, string exportPath="" )
        {
            ChargeEmailModel chargeEmailModel = new ChargeEmailModel();
            try
            {
                clear();
            
                string year = yearAndMonth.Substring(0, 4);
                string month = yearAndMonth.Substring(5, 2);
                string strVisit = "";
                ssDoc.ActiveSheet.Cells[nP1[5], nP2[5]].Value = "대한 " + year + " - " + docNumber + "호" + "(" + executeDate + ")";
                ssDoc.ActiveSheet.Cells[nP1[0], nP2[0]].Value = siteName + " 대표";
                ssDoc.ActiveSheet.Cells[nP1[1], nP2[1]].Value = year + "년 " + month + "월 " + "보건관리지원(대행) 방문일정 안내";
                if (nP1[6] > 0) ssDoc.ActiveSheet.Cells[nP1[6], nP2[6]].Value = clsType.User.JobName; //담당

                chargeEmailModel.Title = year + "년 " + month + "월 보건관리지원(대행) 방문일정 안내";
                if (clsType.HosInfo.SwLicense == "9555-88P5-8P33") chargeEmailModel.Title = "대한보건환경연구소 " + year + "년 " + month + "월 보건관리지원(대행) 방문일정 안내";
                
                chargeEmailModel.Content = "<html><body> 1. 귀사의 무궁한 발전과 무재해를 기원합니다 <br>";
                chargeEmailModel.Content = chargeEmailModel.Content + "2. "+ month + "월 일정을 첨부와 같이 보내드리오니 직원 여러분의 많은 참여 부탁드립니다 ";
                chargeEmailModel.Content = chargeEmailModel.Content + "</body></html>";
                Log.Error(exportPath);
                for (int j = 0; j < list.Count; j++)
                {
                    string date = list[j].VisitReserveDate.Substring(5, 2) + "/" + list[j].VisitReserveDate.Substring(8, 2);
                    string dayOfWeek = DateUtil.ToDayOfWeek(list[j].VisitReserveDate, ComBase.Controls.DateTimeType.YYYY_MM_DD);
                    string day = date + "(" + dayOfWeek.Substring(0, 1) + ")";
                    if (list[j].visitUserRole == "DOCTOR")
                    {
                        if (ssDoc.ActiveSheet.Cells[nP1[2], nP2[2]].Value == "-")
                        {
                            ssDoc.ActiveSheet.Cells[nP1[2], nP2[2]].Value = day + list[j].visitUserName;
                        }
                        else
                        {
                            ssDoc.ActiveSheet.Cells[nP1[2], nP2[2]].Value += "\n" + day + list[j].visitUserName;
                        }
                    }
                    if (list[j].visitUserRole == "NURSE")
                    {
                        if (ssDoc.ActiveSheet.Cells[nP1[3], nP2[3]].Value=="-")
                        {
                            ssDoc.ActiveSheet.Cells[nP1[3], nP2[3]].Value = day + list[j].visitUserName;
                        }
                        else
                        {
                            ssDoc.ActiveSheet.Cells[nP1[3], nP2[3]].Value += "\n" + day + list[j].visitUserName;
                        }
                        
                    }
                    if (list[j].visitUserRole == "ENGINEER")
                    {
                        if (ssDoc.ActiveSheet.Cells[nP1[4], nP2[4]].Value=="-")
                        {
                            ssDoc.ActiveSheet.Cells[nP1[4], nP2[4]].Value = day + list[j].visitUserName;
                        }
                        else
                        {
                            ssDoc.ActiveSheet.Cells[nP1[4], nP2[4]].Value += "\n" + day + list[j].visitUserName;
                        }
                    }

                    //방문장소
                    if (!list[j].visitPlace.IsNullOrEmpty())
                    {
                        if (VB.InStr(strVisit, day) == 0)
                        {
                            if (strVisit == "")
                            {
                                strVisit = day + " " + list[j].visitPlace;
                            }
                            else
                            {
                                strVisit += ", " + day + " " + list[j].visitPlace;
                            }
                        }
                    }
                }
                ssDoc.ActiveSheet.Cells[nP1[6], nP2[6]].Value = strVisit;
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
                //ssDoc_old.ActiveSheet.Cells[21, 5].Value = "3. 위 관련으로 44월 일정을 아래와 같이 보내드리오니 직원 여러분의 많은 참여";

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

                SpreadPrint print = new SpreadPrint(ssDoc_old, PrintStyle.STANDARD_APPROVAL);
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
