using ComBase;
using ComBase.Controls;
using FarPoint.Win.Spread;
using HC_Core.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HC_Core.Model;

namespace HC.Core.Common.Util
{
    public enum PrintStyle
    {
        /// <summary>
        /// 형식이 있는 스프레드
        /// </summary>
        FORM,
        /// <summary>
        /// 일반 리포트
        /// </summary>
        STANDARD_REPORT,
        /// <summary>
        /// 
        /// </summary>
        STANDARD_APPROVAL,

        /// <summary>
        /// 장수마다 서명 추가
        /// </summary>
        STANDARD_APPROVAL_SIGN,
        /// <summary>
        /// 헤더결재란
        /// </summary>         
        HEAD_APPROVAL
    }
    /// <summary>
    /// https://help.grapecity.com/spread/SpreadNet10/WF/webframe.html#spwin-printlayout.html
    /// </summary>
    public class SpreadPrint
    {
        public delegate void PrintCompletedEventHandler(object sender, PrintMessageBoxEventArgs e);
        public event PrintCompletedEventHandler printCompletedEventHandler;
        //  public delegate void OnPrintCompleted();
        // public OnPrintCompleted onPrintCompleted = null;
        ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        /// <summary>
        /// 제목
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 기간
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// 사업장명 표시
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// 사업장 서명/담당자 , 작성자 이름/서명
        /// </summary>
        public PrintSignModel PrintSignModel { get; set; }
        /// <summary>
        /// 워터마크 사용여부
        /// </summary>
        public bool IsWarterMark {get;set; }

        /// <summary>
        /// 여백
        /// </summary>
        public PrintMargin margin { get; set; }

        /// <summary>
        /// 프린터 출력 방향
        /// </summary>
        public PrintOrientation orientation {get;set;}
        private FpSpread spread;
        private PrintInfo printInfo;
        private Font regularFont;
        private Font footerFont;
        private Font titleFont;
        private PrintStyle printStyle;
        private clsSpread CS = new clsSpread();
        private PrinterSettings settings;

        int TOTAL_PAGE = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spread"></param>
        /// <param name="printStyle"></param>
        /// <param name="isPreview"></param>
        public SpreadPrint(FpSpread spread, PrintStyle printStyle = PrintStyle.STANDARD_REPORT,  bool isPreview = false)
        {
            settings = new PrinterSettings();
            this.IsWarterMark = false;            
            this.Title = "";
            this.spread = spread;
            this.regularFont = new Font("맑은 고딕", 11, FontStyle.Regular);
            footerFont = new Font("맑은 고딕", 8, FontStyle.Regular);
            this.titleFont = new Font("맑은 고딕", 16, FontStyle.Bold);
            this.printInfo = new PrintInfo();
            this.printInfo.Preview = isPreview;
            this.printStyle = printStyle;
            this.printInfo.ShowBorder = false;
           // this.printInfo.UseSmartPrint = true;
            
            this.spread.ActiveSheet.PrintInfo = printInfo;
            
            if (IsWarterMark)
            {
                this.spread.PrintBackground += fpSpread_PrintBackground;
            }
            this.spread.PrintAbort += Spread_PrintAbort;
            this.spread.PrintMessageBox += Spread_PrintMessageBox;
            this.spread.PrintDocument += Spread_PrintDocument;
            spread.PrintHeaderFooterArea += Spread_PrintHeaderFooterArea;

            margin = new PrintMargin();

            int[] intPage = null;
            intPage = spread.GetRowPageBreaks(0);
            TOTAL_PAGE = intPage.Length + 1;

        }

        private void Spread_PrintHeaderFooterArea(object sender, PrintHeaderFooterAreaEventArgs e)
        {
            if (this.printStyle == PrintStyle.HEAD_APPROVAL)
            {
                Pen cPen = new Pen(Color.Black);
                cPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                cPen.Width = 3;
                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Far;

                if (e.IsHeader == true)
                {
                    #region 칸 그리기
                    e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 555, 90, 220, 70);
                    e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 555, 90, 30, 70);
                    e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 585, 115, 190, 45);
                    e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), 650, 90, 65, 70);
                    #endregion

                    #region 칸안에 글
                    e.Graphics.DrawString("결", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 580, 93, drawFormat);
                    e.Graphics.DrawString("재", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 580, 135, drawFormat);
                    e.Graphics.DrawString("담  당", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 640, 93, drawFormat);
                    //e.Graphics.DrawString("계  장", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 705, 93, drawFormat);
                    e.Graphics.DrawString("팀  장", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 705, 93, drawFormat);
                    e.Graphics.DrawString("부  장", new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 770, 93, drawFormat);
                    #endregion

                    #region  
                    drawFormat.Alignment = StringAlignment.Far;
                    if (Period.NotEmpty())
                    {
                        e.Graphics.DrawString("작업기간 : " + Period, new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 280, 122, drawFormat);
                    }
                    e.Graphics.DrawString("출력시간 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 220, 135, drawFormat);
                    //e.Graphics.DrawString("Page : " + e.PageNumber, new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 135, 143, drawFormat);
                    #endregion
                }
            }
            else if (this.printStyle == PrintStyle.STANDARD_APPROVAL_SIGN)
            {
                Pen cPen = new Pen(Color.Black);
                cPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                cPen.Width = 3;
                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Far;

                if (e.IsHeader == true)
                {

                    //if (PrintSignModel != null)
                    //{
                    //    e.Graphics.DrawString(PrintSignModel.SiteManagerName, new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 160, 1110, drawFormat);
                    //    if (PrintSignModel.SiteManagerSign != null)
                    //    {
                    //        Rectangle imageRect = new Rectangle(180, 1090, 50, 50);
                    //        e.Graphics.DrawImage(PrintSignModel.SiteManagerSign, imageRect);
                    //    }


                    //   e.Graphics.DrawString(PrintSignModel.OshaName, new Font("맑은 고딕", 11, FontStyle.Regular), Brushes.Black, 690, 1110, drawFormat);
                    //    if (PrintSignModel.OshaSign != null)
                    //    {
                    //        Rectangle imageRect = new Rectangle(710, 1090, 50, 50);
                    //        e.Graphics.DrawImage(PrintSignModel.OshaSign, imageRect);
                    //    }
                    //}
                }
                else
                {
                    if (this.SiteName.NotEmpty())
                    {
                        StringFormat stringFormat = new StringFormat();
                        stringFormat.Alignment = StringAlignment.Center;

                        //  페이지 번호 출력 변경
                        //  총갯수 - 현재페이지
                        Rectangle drawRect = new Rectangle(0, 1120, e.Rectangle.Width, 20);
                        e.Graphics.DrawString(SiteName + "  " + TOTAL_PAGE + " - " + e.PageNumber, new Font("맑은 고딕", 7, FontStyle.Regular), Brushes.Black, drawRect, stringFormat);
                      
                    }
                  
                }
               
            }


        }

        private void Spread_PrintDocument(object sender, PrintDocumentEventArgs e)
        {
           
        }

        private void Spread_PrintAbort(object sender, PrintAbortEventArgs e)
        {
            manualResetEvent.Set();
        }

        /// <summary>
        /// 프린트 시작과 끝에 호출됨.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spread_PrintMessageBox(object sender, PrintMessageBoxEventArgs e)
        {
            if(e.IsPreview == false)
            {
                bool result = e.BeginPrinting;
            }
            
            if(e.BeginPrinting == false)
            {
                printCompletedEventHandler?.Invoke(sender, e);
                manualResetEvent.Set();
            }
        }

    

        /// <summary>
        /// 워터마크
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpSpread_PrintBackground(object sender, FarPoint.Win.Spread.PrintBackgroundEventArgs e)
        {
            FarPoint.Win.Picture pic = new FarPoint.Win.Picture(Resources.logo, FarPoint.Win.RenderStyle.Normal);
            pic.AlignHorz = FarPoint.Win.HorizontalAlignment.Left;
            pic.AlignVert = FarPoint.Win.VerticalAlignment.Top;
            pic.Paint(e.Graphics, e.SheetRectangle);
        }
        public PrintMargin MakeMargin()
        {
         
            if(this.printStyle == PrintStyle.FORM)
            {
                margin.Top = 30; // 끝과 헤더의 간격
                margin.Header = 30; // 헤더와 스프레드 간격
                margin.Footer = 30; // 푸터와 스프레드 간격
                margin.Bottom = 30; //푸터와 끝 간격
            }
            else if (this.printStyle == PrintStyle.STANDARD_APPROVAL)
            {
                margin.Top = 60; // 끝과 헤더의 간격
                margin.Header = 30; // 헤더와 스프레드 간격
                margin.Footer = 30; // 푸터와 스프레드 간격
                margin.Bottom = 30; //푸터와 끝 간격
             
            }
            else if (this.printStyle == PrintStyle.STANDARD_APPROVAL_SIGN)
            {
                margin.Top = 30; // 끝과 헤더의 간격
                margin.Header = 40; // 헤더와 스프레드 간격
                margin.Footer = 30; // 푸터와 스프레드 간격
                margin.Bottom = 30; //푸터와 끝 간격
              
            }
            else if (this.printStyle == PrintStyle.HEAD_APPROVAL)
            {
                margin.Top = 30; // 끝과 헤더의 간격
                margin.Header = 125; // 헤더와 스프레드 간격
                margin.Footer = 30; // 푸터와 스프레드 간격
                margin.Bottom = 30; //푸터와 끝 간격
          
            }
            else
            {
                margin.Top = 10; // 끝과 헤더의 간격
                margin.Header = 10; // 헤더와 스프레드 간격
                margin.Footer = 10; // 푸터와 스프레드 간격
                margin.Bottom = 10; //푸터와 끝 간격
            }
            
            return margin;
        }
        public string MakeHeader()
        {
            string header = string.Empty;
            header += CS.setSpdPrint_String(Title, titleFont, clsSpread.enmSpdHAlign.Center, false, true);
            return header;
        }
        public string MakeFooter()
        {
            string footer = "";// " / g\"1\"/r/cl\"4\" /p of /pc";
            //if (SiteName.NotEmpty())
            //{
            //    footer += CS.setSpdPrint_String( SiteName, footerFont, clsSpread.enmSpdHAlign.Right, false, false) ;
            //}
            //footer += " Page /p";
            
            
            return footer;
        }
        
        /// <summary>
        /// 인쇄 실행
        /// </summary>
        /// <param name="printSheet"></param>
        public void Execute(SheetView printSheet = null)
        {
            string printerName = settings.PrinterName;

            //this.printInfo.ColStart = printSheet.Models.Selection.AnchorColumn;
            //this.printInfo.ColEnd = printSheet.Models.Selection.LeadColumn;
            //this.printInfo.RowStart = printSheet.Models.Selection.AnchorRow;
            //this.printInfo.RowEnd = printSheet.Models.Selection.LeadRow;

            this.printInfo.Orientation = this.orientation;
            this.printInfo.Centering = Centering.Horizontal;
            
            if (this.Title.NotEmpty())
            {
                this.printInfo.Header = MakeHeader();
            }
            
            this.printInfo.Footer = MakeFooter();
            this.printInfo.Margin = MakeMargin();
            this.printInfo.ShowColor = true;
            this.printInfo.PrintType = PrintType.All;
            this.printInfo.PdfWriteMode = PdfWriteMode.New;
            if (printSheet == null)
            {
                spread.PrintSheet(0, true);

                if (this.printInfo.PrintToPdf == false)
                {
                    if (printerName == "Microsoft Print to PDF" || printerName =="OneNote for Windows 10")
                    {
                        manualResetEvent.WaitOne(5000);//microsoft pdf 프린터에서는 작동안함
                    }
                    else
                    {
                        manualResetEvent.WaitOne(1000);
                    }

                }
                else
                {
                    if (printerName == "Microsoft Print to PDF" || printerName == "OneNote for Windows 10")
                    {
                        manualResetEvent.WaitOne(5000 );//microsoft pdf 프린터에서는 작동안함
                    }
                    else
                    {
                        manualResetEvent.WaitOne(1000);
                    }
                }
                
            }
            else
            {
            
                spread.PrintSheet(printSheet);
            
            }
            

          


        }
     
        public void ExportPDF(string fileName, SheetView printSheet = null)
        {
            this.printInfo.PrintToPdf = true;
            this.printInfo.PdfWriteTo = PdfWriteTo.File;
            this.printInfo.PdfWriteMode = PdfWriteMode.New;
            this.printInfo.PdfFileName = @fileName;
            this.printInfo.Preview = false;
            //this.printInfo.UseSmartPrint = true;
            this.printInfo.Opacity = 150;

            Execute(printSheet);
        }
        public void ExportPDFNoWait(string fileName, SheetView printSheet)
        {
            this.printInfo.PrintToPdf = true;
            this.printInfo.PdfWriteTo = PdfWriteTo.File;
            this.printInfo.PdfWriteMode = PdfWriteMode.New;
            this.printInfo.PdfFileName = @fileName;
            this.printInfo.Preview = false;
            Execute(printSheet);
        }
    }
}
