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

    public partial class ChargeDocument : CommonForm
    {
        private HC_CODE pdfPath = null;
        SpreadPrint print = null;
        public ChargeDocument()
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
        public string Print(string slipAmt, string yearAndMonth, string executeDate,  string siteName, bool isPdf, string docNumber, string exportPath = "")
        {
            clear();
            string year = yearAndMonth.Substring(0, 4);
            string month = yearAndMonth.Substring(5, 2);
            ssDoc.ActiveSheet.Cells[6, 5].Value = "포성건 " + year + " - " + docNumber + "호";
            ssDoc.ActiveSheet.Cells[8, 5].Value = executeDate;

            ssDoc.ActiveSheet.Cells[10, 5].Value = siteName;
            ssDoc.ActiveSheet.Cells[14, 5].Value = year + "년 " + month + "월 " + "보건관리전문기관 위탁 수수료 청구";
            ssDoc.ActiveSheet.Cells[21, 5].Value = "3. 위 관련으로 " + month + "월 일정을 아래와 같이 보내드리오니 직원 여러분의 많은 참여";

          
            ssDoc.ActiveSheet.Cells[29, 20].Text = slipAmt + "원3";
        


            FarPoint.Win.Spread.CellType.ImageCellType t = new FarPoint.Win.Spread.CellType.ImageCellType();
            //// Load an image file and set it to BackgroundImage property.
            FarPoint.Win.Picture p = new FarPoint.Win.Picture(Image.FromFile("c:\\psmhexe\\test.bmp"), FarPoint.Win.RenderStyle.Stretch);
           

            ssDoc.ActiveSheet.Cells[6, 5].CellType = t;
            ssDoc.ActiveSheet.Cells[6, 5].Value = p;
            //ssDoc.ActiveSheet.Rows[6].Height = 50;
            //ssDoc.ActiveSheet.Columns[5].Width = 150;

            print = new SpreadPrint(ssDoc, PrintStyle.STANDARD_APPROVAL);
            if (isPdf)
            {

                string pdfFileName = "";
                if (exportPath.NotEmpty())
                {
                    pdfFileName = exportPath + "\\" + yearAndMonth + "_미수공문_" + siteName + ".pdf";
                }
                else
                {
                    pdfFileName = pdfPath.CodeName + "\\" + yearAndMonth + "_미수공문_" + siteName + ".pdf";
                }


                print.ExportPDF(pdfFileName);

                return pdfFileName;

            }
            else
            {
                print.Execute();
                return string.Empty;
            }
        }
    }
}
