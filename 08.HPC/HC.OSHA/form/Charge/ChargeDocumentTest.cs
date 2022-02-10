using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using ComHpcLibB;
using HC.Core.Common.Interface;
using HC.Core.Common.Util;
using HC.Core.Dto;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC.OSHA.Service;
using HC_Core;
using HC_OSHA.form.Charge;
using HC_OSHA.form.Etc;
using HC_OSHA.form.Visit;
using HC_OSHA.Model.Schedule;
using HC_OSHA.StatusReport;
using HC_OSHA.Visit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace HC_OSHA
{

    public partial class ChargeDocumentTest : CommonForm
    {
        private HC_CODE pdfPath = null;
        SpreadPrint print = null;
        public ChargeDocumentTest()
        {
            InitializeComponent();
            HcCodeService codeService = new HcCodeService();
            pdfPath = codeService.FindActiveCodeByGroupAndCode("PDF_PATH", "OSHA_ESTIMATE", "OSHA");
        }


        public void clear()
        {
            //  ssDoc.ActiveSheet.Cells[28, 2].Value = "1";

            ssDoc.ActiveSheet.Cells[29, 20].Value = "";//수수료
            ssDoc.ActiveSheet.Cells[8, 5].Value = "";//시행일자


        }
        public ChargeEmailModel Print(string slipAmt, string yearAndMonth, string executeDate, string siteName, bool isPdf, string docNumber, string exportPath = "")
        {
            clear();
            ChargeEmailModel chargeEmailModel = new ChargeEmailModel();
            string year = yearAndMonth.Substring(0, 4);
            string month = yearAndMonth.Substring(5, 2);
            ssDoc.ActiveSheet.Cells[6, 5].Value = "포성건 " + year + " - " + docNumber + "호";
            ssDoc.ActiveSheet.Cells[8, 5].Value = executeDate.Replace("-",".");

            ssDoc.ActiveSheet.Cells[10, 5].Value = siteName +" 대표이사";
            ssDoc.ActiveSheet.Cells[14, 5].Value = year + "년 " + month + "월 " + "보건관리전문기관 위탁 수수료 청구";
            
            chargeEmailModel.Title = "포항성모병원 " + year + "년 " + month + "월 위탁 수수료 청구 공문 안내";

            ssDoc.ActiveSheet.Cells[21, 4].Text = "3. 위 관련으로 당원에서 실시한 " + year + "년 " + month + "월 보건관리 위탁수수료를 아래와";

            chargeEmailModel.Content = "<html><body>1. 귀사의 무궁한 발전과 무재해를 기원합니다. <br>";
            chargeEmailModel.Content = chargeEmailModel.Content + "2. 관련근거 : 산업안전보건법 제18조, 동법 시행령 제23조, 고용노동부 예규 제163호 제4조. <br>";
            chargeEmailModel.Content = chargeEmailModel.Content + "3. 위 관련으로 당원에서 실시한 " + year + "년 " + month + "월 보건관리 위탁수수료를 <br> 첨부와 같이 청구하오니 확인하여 주시기 바랍니다.";
            chargeEmailModel.Content = chargeEmailModel.Content + "</body></html>";

            ssDoc.ActiveSheet.Cells[29, 20].Text = slipAmt.Trim() + "원";

            ssDoc.ActiveSheet.Cells[29, 20].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssDoc.ActiveSheet.Columns[20].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;

           FarPoint.Win.Spread.CellType.ImageCellType dojang = new FarPoint.Win.Spread.CellType.ImageCellType();
            dojang.Style = FarPoint.Win.RenderStyle.Normal;
            ssDoc.ActiveSheet.Cells[37, 19].CellType = dojang;
            ssDoc.ActiveSheet.Cells[37, 19].Value = Image.FromFile(@"C:\HealthSoft\exenet\Resources\dojang.png");


            FarPoint.Win.Spread.CellType.ImageCellType logo = new FarPoint.Win.Spread.CellType.ImageCellType();
            logo.Style = FarPoint.Win.RenderStyle.Normal;
            ssDoc.ActiveSheet.Cells[1, 1].CellType = logo;
            ssDoc.ActiveSheet.Cells[1, 1].Value = Image.FromFile(@"C:\HealthSoft\exenet\Resources\logo.png");

            print = new SpreadPrint(ssDoc, PrintStyle.STANDARD_APPROVAL);
            if (isPdf)
            {

                string pdfFileName = "";
                if (exportPath.NotEmpty())
                {
                    pdfFileName = exportPath + "\\" + yearAndMonth + "_청구공문_" + siteName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                }
                else
                {
                    pdfFileName = pdfPath.CodeName + "\\" + yearAndMonth + "_청구공문_" + siteName + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
                }

                ComFunc.Delay(1000);
                print.ExportPDF(pdfFileName);
                ComFunc.Delay(1000);

                chargeEmailModel.PdfFileName = pdfFileName;
                ComFunc.Delay(1000);

            }
            else
            {
                print.Execute();
            }
            return chargeEmailModel;
        }

        private void ChargeDocumentTest_Load(object sender, EventArgs e)
        {
       
        }
    }
}
