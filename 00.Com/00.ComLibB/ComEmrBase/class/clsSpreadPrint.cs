using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using ComBase; //기본 클래스
using ComDbB; //DB연결

namespace ComEmrBase
{
    public class clsSpreadPrint
    {
        /// <summary>
        /// 작성일자
        /// </summary>
        public static string mstrWRITEDATE= string.Empty;    //
        /// <summary>
        /// 작성자
        /// </summary>
        public static string mstrWRITENAME= string.Empty;    //
        /// <summary>
        /// 출력일자
        /// </summary>
        public static string mstrPRINTDATE= string.Empty;    //
        /// <summary>
        /// 출력자
        /// </summary>
        public static string mstrPRINTNAME= string.Empty;    //

        private static string mstrFormNo = "0";
        private static string mstrUpdateNo = "0";

        private static EmrPatient pPrint = null;
        private static EmrForm fPrint = null;

        private static FarPoint.Win.Spread.FpSpread mSpd;

        private static int pnTotPage = 0;
        private static int pnPage = 0;

        /// <summary>
        /// 스프래드 형식의 기록지를 출력한다.
        /// </summary>
        /// <param name="strFormNo">기록지번호</param>
        /// <param name="strUpdateNo">업데이트번호</param>
        /// <param name="po">환자정보</param>
        /// <param name="blnAcp">내원내역별 출력(true)</param>
        /// <param name="strFrDate">조회시작일자</param>
        /// <param name="strEndDate">조회 마지막일자</param>
        /// <param name="spd">출력할 스프레드</param>
        /// <param name="PrintType">미리보기 여부</param>
        /// <param name="MarginTop">마진</param>
        /// <param name="MarginBottom">마진</param>
        /// <param name="MarginLeft">마진</param>
        /// <param name="MarginRight">마진</param>
        /// <param name="UseSmartPrint">스마트프린터</param>
        /// <param name="Orientation">용지방향</param>
        /// <param name="strCurDateTime">출력일자시간</param>
        public static void PrintSpdFormEx(PsmhDb pDbCon, string strFormNo, string strUpdateNo, EmrPatient po, bool blnAcp, string strFrDate, string strEndDate,
                                        FarPoint.Win.Spread.FpSpread spd, string PrintType,
                                        int MarginTop, int MarginBottom, int MarginLeft, int MarginRight, bool UseSmartPrint,
                                        FarPoint.Win.Spread.PrintOrientation Orientation, string strCurDateTime)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;

            pPrint = clsEmrChart.ClearPatient();
            fPrint = clsEmrChart.ClearEmrForm();

            pPrint = po;
            mSpd = spd;

            //if (clsPrint.gGetPrinterFind("기록지") == false)
            //{
            //    ComFunc.MsgBox("기록지 프린터를 찾을 수 없습니다." + ComNum.VBLF + "프린터를 추가해 주십시오.");
            //}

            string strPRINTDATE = ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D");
            mstrPRINTNAME = clsType.User.UserName;

            //기록지 정보를 세팅
            fPrint = clsEmrChart.SerEmrFormInfo(pDbCon, mstrFormNo, mstrUpdateNo);

            //clsSpread.setPrintHeadWhite(spd.Sheets[0]);

            PrintDocument prtFlow = new PrintDocument();
            PageSettings ps = new PageSettings();
            //ps.PrinterSettings.PrinterName = "기록지";
            ps.Margins = new Margins(10, 10, 10, 10);
            PrintController printController = new StandardPrintController();
            prtFlow.PrintController = printController;  //기본인쇄창 없애기
            prtFlow.DocumentName = fPrint.FmFORMNAMEPRINT;

            prtFlow.DefaultPageSettings.Margins = ps.Margins;

            if (Orientation == FarPoint.Win.Spread.PrintOrientation.Landscape)
            {
                prtFlow.DefaultPageSettings.Landscape = true;
            }
            else
            {
                prtFlow.DefaultPageSettings.Landscape = false;
            }
            prtFlow.PrintPage += new PrintPageEventHandler(pd_FlowPrintPage);


            spd.Sheets[0].PrintInfo.Orientation = Orientation;
            spd.Sheets[0].PrintInfo.Margin.Top = MarginTop;
            spd.Sheets[0].PrintInfo.Margin.Bottom = MarginBottom;
            spd.Sheets[0].PrintInfo.Margin.Left = MarginLeft;
            spd.Sheets[0].PrintInfo.Margin.Right = MarginRight;
            spd.Sheets[0].PrintInfo.HeaderHeight = 120;
            //spd.Sheets[0].PrintInfo.FooterHeight = 50;
            spd.Sheets[0].PrintInfo.ShowColor = false;
            spd.Sheets[0].PrintInfo.ShowBorder = true;
            spd.Sheets[0].PrintInfo.ShowGrid = true;
            spd.Sheets[0].PrintInfo.ShowShadows = false;
            spd.Sheets[0].PrintInfo.UseMax = true;
            spd.Sheets[0].PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            spd.Sheets[0].PrintInfo.UseSmartPrint = true;

            int i = 0;
            for (i = 1; i <= spd.GetPrintPageCount(0); i++)
            {
                if (PrintType == "P")
                {
                    prtFlow.Print();
                }
                pnPage += 1;
            }
            prtFlow.Print();
            //clsSpread.setPrintHeadWhiteBack(spd.Sheets[0], Color.White);
        }

        private static void pd_FlowPrintPage(object sender, PrintPageEventArgs e)
        {

            Pen cPen = new Pen(Color.Black);
            cPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            cPen.Width = 2;
            e.Graphics.DrawLine(cPen, e.MarginBounds.X, e.MarginBounds.Y, e.MarginBounds.Width - 10, e.MarginBounds.Y);
            e.Graphics.DrawString(fPrint.FmFORMNAMEPRINT, new Font("굴림", 18), Brushes.Black, 50, e.PageBounds.Y + 25);
            if (clsType.HosInfo.strIMAGEUSE == "1")
            {
                if (File.Exists(@"C:\Mentorsoft\exenet\hsplog.png"))
                {

                    Image ImagesX = new Bitmap(System.Drawing.Image.FromFile(@"C:\Mentorsoft\exenet\hsplog.png"));   //'병원 로고
                    Image pThumbnail = ImagesX.GetThumbnailImage(160, 35, delegate { return false; }, new IntPtr());

                    e.Graphics.DrawImage(pThumbnail, 50, e.PageBounds.Y + 105 - 22, 124, 22);
                }
                else
                {
                }
            }
            else
            {
            }
            e.Graphics.DrawRectangle(new Pen(Brushes.Black, 1), e.MarginBounds.Width - 270, e.PageBounds.Y + 5, 230, 100);
            e.Graphics.DrawLine(cPen, e.MarginBounds.X, e.PageBounds.Height - 80, e.MarginBounds.Width - 10, e.PageBounds.Height - 80);

            Rectangle rect;
            rect = new Rectangle(e.MarginBounds.X + 10, e.MarginBounds.Y + 17, e.MarginBounds.Width - 20, e.MarginBounds.Height - 5);
            int cnt1 = mSpd.GetOwnerPrintPageCount(e.Graphics, rect, 0);
            pnTotPage = pnTotPage + 1;
            if (cnt1 > 0)
            {
                mSpd.OwnerPrintDraw(e.Graphics, rect, 0, pnPage);
                e.HasMorePages = false;
            }
        }

        /// <summary>
        /// 스프래드 형식 출력
        /// </summary>
        /// <param name="strFormNo"> 폼번호 </param>
        /// <param name="strUpdateNo"> 업데이트 번호</param>
        /// <param name="po">환자정보</param>
        /// <param name="blnAcp"></param>
        /// <param name="strFrDate">내원일자</param>
        /// <param name="strEndDate">퇴원일자</param>
        /// <param name="spd">스프래드</param>
        /// <param name="PrintType">출력타입(P: 출력, V: 미리보기)</param>
        /// <param name="MarginTop">마진</param>
        /// <param name="MarginBottom">마진</param>
        /// <param name="MarginLeft">마진</param>
        /// <param name="MarginRight">마진</param>
        /// <param name="UseSmartPrint">스카트 출력여부</param>
        /// <param name="Orientation">출력방향</param>
        /// <param name="strCurDateTime">출력시간</param>
        public static int PrintSpdForm(PsmhDb pDbCon, string strFormNo, string strUpdateNo, EmrPatient po, bool blnAcp, string strFrDate, string strEndDate,
                                        FarPoint.Win.Spread.FpSpread spd, string PrintType,
                                        int MarginTop, int MarginBottom, int MarginLeft, int MarginRight, bool UseSmartPrint,
                                        FarPoint.Win.Spread.PrintOrientation Orientation, string strCurDateTime)
        {
            int rtnVal = 0;
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;

            pPrint = clsEmrChart.ClearPatient();
            fPrint = clsEmrChart.ClearEmrForm();

            pPrint = po;

            string strFont1= string.Empty;
            string strFont2= string.Empty;
            string strHead1= string.Empty;
            string strHead2= string.Empty;
            string strFooter1= string.Empty;
            string strFooter2= string.Empty;

            string strPRINTDATE = ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D");
            mstrPRINTNAME = clsType.User.UserName;
            
            //기록지 정보를 세팅
            fPrint = clsEmrChart.SerEmrFormInfo(pDbCon, mstrFormNo, mstrUpdateNo);

            //Print Head 지정
            strFont1 = "/fn\"굴림체\"/fz\"14\"/fb1/fi0/fu0/fk0/fs1";
            strFont2 = "/fn\"굴림체\"/fz\"10\"/fb0/fi0/fu0/fk0/fs2";
            string strFont3 = "/fn\"굴림체\"/fz\"8\"/fb0/fi0/fu0/fk0/fs3";

            if (clsFormPrint.mstrPRINTFLAG == "1")
            {
                strHead1 = "/c/f1" + fPrint.FmFORMNAMEPRINT + " (원내용)" + "/f1/n/n";
            }
            else
            {
                strHead1 = "/c/f1" + fPrint.FmFORMNAMEPRINT + "/f1/n/n";
            }

            string strInOut = "내원일자";
            if (pPrint.inOutCls == "I")
            {
                strInOut = "입원일자";
            }

            strHead2 = "/l/f2" + "성명 : " + pPrint.ptName + " [" + pPrint.ptNo + "]   (" + pPrint.age + "/" + pPrint.sex + ")" + "/f2/n";
            if (clsFormPrint.mstrPRINTFLAG == "1" || clsFormPrint.mstrPRINTFLAG == "2")
            {
                strHead2 = strHead2 + "/l/f2" + "주민번호 : " + pPrint.ssno1 + "-" + (pPrint.ssno2.Length == 0 ? "X" : pPrint.ssno2.Substring(0, 1)) + "/f2/n";
            }
            else
            {
                strHead2 = strHead2 + "/l/f2" + "주민번호 : " + pPrint.ssno1 + "-" + VB.Left(pPrint.ssno2, 1) + "/f2/n";
            }

            if (blnAcp == true)
            {
                strHead2 = strHead2 + "/l/f2" + "진료과/주치의(" + strInOut + ") : " + pPrint.medDeptKorName + " / " + pPrint.medDrName + " (" + ComFunc.FormatStrToDate(pPrint.medFrDate, "D") + ")" + "/f2/n";
            }

            //if (clsCommon.gstrEXENAME != "MHEMRJOBDG.EXE")
            //{
            //    strHead2 = strHead2 + "/l/f2" + "작성일자 : " + ComFunc.FormatStrToDate(strFrDate, "D") + " ~ " + ComFunc.FormatStrToDate(strEndDate, "D") + "/f2/n";
            //}

            strHead2 = strHead2 + "/l/f2" + "출력자(출력일자) : " + mstrPRINTNAME + "(" + strPRINTDATE + ")" + "/f2/n/n";

            if (clsType.HosInfo.strIMAGEUSE == "1")
            {
                if (File.Exists(@"C:\Mentorsoft\exenet\hsplog.png"))
                {

                    Image pThumbnail;
                    using (Image ImagesX = new Bitmap(Image.FromFile(@"C:\Mentorsoft\exenet\hsplog.png")))   //'병원 로고
                    {
                        pThumbnail = ImagesX.GetThumbnailImage(160, 35, delegate { return false; }, new IntPtr());
                    }
                    

                    spd.Sheets[0].PrintInfo.Images = new Image[] { pThumbnail };   //'병원 로고
                    strFooter1 = "/l/g\"0\"";
                }
                else
                {
                    strFooter1 = "/l/f1" + clsType.HosInfo.strNameKor + "/f1";
                }
            }
            else
            {
                strFooter1 = "/l/f1" + clsType.HosInfo.strNameKor + "/f1";
            }

            if (clsFormPrint.mstrPRINTFLAG == "1")
            {
                strFooter2 = "/r/f3" + "본 의무기록지는 병원내부용으로만 사용가능합니다." + "/f3/n";
                //strFooter2 = strFooter2 + "/r/f3" + "외부유출시 법적인 효력을 인정받을 수 없습니다." + "/f3/n";
                strFooter2 = strFooter2 + "/c/f3" + "(/p" + " of " + "/pc)" + "/f3";
            }
            else
            {
                strFooter2 = "/r/f3" + "본 의무기록지는 작성자의 전자서명을 공인인증 받은 기록지입니다." + "/f3/n";
                //strFooter2 = strFooter2 + "/r/f2" + "This is an electronically authorized official medical record." + "/f2/n";
                strFooter2 = strFooter2 + "/c/f3" + "(/p" + " of " + "/pc)" + "/f3";
            }

            //clsSpread.setPrintHeadWhite(spd.Sheets[0]);

            //FarPoint.Win.Spread.PrintInfo pi = new FarPoint.Win.Spread.PrintInfo();
            //PrinterSettings ps = new PrinterSettings();

            //int i = 0;
            //for (i = 0; i <= ps.PaperSources.Count - 1; i++)
            //{ 
            //    comboBox1.Items.Add(ps.PaperSources[i].SourceName);
            //}
            //ps.PrinterName = "기록지";
            //pi.PaperSize = new System.Drawing.Printing.PaperSize("Letter", 600, 300);
            //pi.PaperSource = ps.PaperSources[0]; 
            //spd.Sheets[0].PrintInfo = pi;


            spd.Sheets[0].PrintInfo.Orientation = Orientation;
            spd.Sheets[0].PrintInfo.Margin.Top = MarginTop;
            spd.Sheets[0].PrintInfo.Margin.Bottom = MarginBottom;
            spd.Sheets[0].PrintInfo.Margin.Left = MarginLeft;
            spd.Sheets[0].PrintInfo.Margin.Right = MarginRight;
            spd.Sheets[0].PrintInfo.HeaderHeight = 120;
            spd.Sheets[0].PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            //spd.Sheets[0].PrintInfo.FooterHeight = 100;
            spd.Sheets[0].PrintInfo.Footer = strFont1 + strFooter1 + strFont3 + strFooter2;
            spd.Sheets[0].PrintInfo.ShowColor = true;
            spd.Sheets[0].PrintInfo.ShowBorder = true;
            spd.Sheets[0].PrintInfo.ShowGrid = true;
            spd.Sheets[0].PrintInfo.ShowShadows = false;
            spd.Sheets[0].PrintInfo.UseMax = false;
            spd.Sheets[0].PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            spd.Sheets[0].PrintInfo.UseSmartPrint = UseSmartPrint;

            if (PrintType == "P")
            {
                spd.PrintSheet(0);
                if (spd.Name.Length == 0)
                {
                    ComFunc.Delay(5000);
                }
            }
            else if (PrintType == "V")
            {
                //spd.Sheets[0].PrintInfo.ShowPrintDialog = true;
                spd.Sheets[0].PrintInfo.Preview = true;
                spd.PrintSheet(0);
            }

            int[] intPage = spd.GetRowPageBreaks(0);
            //if (Orientation == FarPoint.Win.Spread.PrintOrientation.Landscape)
            //{
            //    intPage = spd.GetColumnPageBreaks(0);
            //}
            //else
            //{
            //}
            rtnVal = intPage.Length + 1;

            return rtnVal;
            //clsSpread.setPrintHeadWhiteBack(spd.Sheets[0], Color.White);
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <param name="strFormNo"> 폼번호 </param>
        /// <param name="strUpdateNo"> 업데이트 번호</param>
        /// <param name="po">환자정보</param>
        /// <param name="blnAcp"></param>
        /// <param name="strFrDate">출력시작일자</param>
        /// <param name="strEndDate">출력종료일자</param>
        /// <param name="spd">스프래드</param>
        /// <param name="PrintType">P: 출력, V: 미리보기</param>
        /// <param name="MarginTop">마진</param>
        /// <param name="MarginBottom">마진</param>
        /// <param name="MarginLeft">마진</param>
        /// <param name="MarginRight">마진</param>
        /// <param name="UseSmartPrint">스마트 출력여부</param>
        /// <param name="Orientation">출력방향</param>
        /// <param name="strRowCol">방향</param>
        /// <param name="intChk">체크 Row or Col</param>
        /// <param name="intEmrNo">EMRNO Row or Col</param>
        /// <param name="intStartNo">data의 시작위치</param>
        /// <param name="strMode">출력모드 P: 그냥출력, A: 출력여부 묻기 </param>
        public static int PrintEmrSpread(PsmhDb pDbCon, string strFormNo, string strUpdateNo, EmrPatient po, bool blnAcp, string strFrDate, string strEndDate,
                                        FarPoint.Win.Spread.FpSpread spd, string PrintType,
                                        int MarginTop, int MarginBottom, int MarginLeft, int MarginRight, bool UseSmartPrint,
                                        FarPoint.Win.Spread.PrintOrientation Orientation,
                                        string strRowCol, int intChk, int intEmrNo, int intStartNo, string strMode)
        {
            int rtnVal = 0;

            
            //if (strRowCol == "COL")
            //{
            //    if (spd.Sheets[0].ColumnCount <= 0)
            //    {
            //        return;
            //    }

            //    if (intChk != -1)
            //    {
            //        for (i = 0; i < spd.Sheets[0].ColumnCount; i++)
            //        {
            //            if (Convert.ToBoolean(spd.Sheets[0].Cells[intChk, i].Value) == true)
            //            {
            //                intChkCnt = intChkCnt + 1;
            //            }
            //            else
            //            {
            //                spd.Sheets[0].Columns[i].Visible = false;
            //            }
            //        }

            //        if (intChkCnt <= 0)
            //        {
            //            for (i = 0; i < spd.Sheets[0].ColumnCount; i++)
            //            {
            //                spd.Sheets[0].Columns[i].Visible = true;
            //            }
            //            ComFunc.MsgBox("출력이 체크되지 않았습니다.");
            //            return;
            //        }
            //    }
            //}
            //else
            //{
            //    if (spd.Sheets[0].RowCount <= 0)
            //    {
            //        return;
            //    }

            //    if (intChk != -1)
            //    {
            //        for (i = 0; i < spd.Sheets[0].RowCount; i++)
            //        {
            //            if (Convert.ToBoolean(spd.Sheets[0].Cells[i, intChk].Value) == true)
            //            {
            //                intChkCnt = intChkCnt + 1;
            //            }
            //            else
            //            {
            //                spd.Sheets[0].Rows[i].Visible = false;
            //            }
            //        }

            //        if (intChkCnt <= 0)
            //        {
            //            for (i = 0; i < spd.Sheets[0].RowCount; i++)
            //            {
            //                spd.Sheets[0].Rows[i].Visible = true;
            //            }
            //            ComFunc.MsgBox("출력이 체크되지 않았습니다.");
            //            return;
            //        }
            //    }
            //}

            if (strMode != "P")
            {
                using (frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption())
                {
                    frmEmrPrintOptionX.StartPosition = FormStartPosition.CenterParent;
                    frmEmrPrintOptionX.ShowDialog();
                }
            }

            if (clsFormPrint.mstrPRINTFLAG == "-1")
            {
                return rtnVal;
            }

            Dictionary<int, Color> RowFontColor = new Dictionary<int, Color>();


            if (spd.Name.Length > 0)
            {
                if (strRowCol.Equals("ROW"))
                {
                    for (int i = 0; i < spd.ActiveSheet.ColumnCount; i++)
                    {
                        if (spd.ActiveSheet.Columns[i].ForeColor != Color.Black)
                        {
                            RowFontColor.Add(i, spd.ActiveSheet.Columns[i].ForeColor);
                        }
                    }

                    spd.ActiveSheet.Columns[0, spd.ActiveSheet.ColumnCount - 1].ForeColor = Color.Black;
                }
                else
                {
                    for (int i = 0; i < spd.ActiveSheet.RowCount; i++)
                    {
                        if (spd.ActiveSheet.Rows[i].ForeColor != Color.Black)
                        {
                            RowFontColor.Add(i, spd.ActiveSheet.Rows[i].ForeColor);
                        }
                    }

                    spd.ActiveSheet.Rows[0, spd.ActiveSheet.RowCount - 1].ForeColor = Color.Black;
                }
            }


            string strCurDateTime = ComQuery.CurrentDateTime(pDbCon, "A");

            //if (clsEmrQuery.SaveEmrXmlPrnYn(pDbCon, spd.Sheets[0], strRowCol, intChk, intEmrNo, intStartNo, strCurDateTime) == false)
            //{
            //    if (intChk != -1)
            //    {
            //        if (strRowCol == "COL")
            //        {
            //            for (i = 0; i < spd.Sheets[0].ColumnCount; i++)
            //            {
            //                spd.Sheets[0].Columns[i].Visible = true;
            //            }
            //        }
            //        else
            //        {
            //            for (i = 0; i < spd.Sheets[0].RowCount; i++)
            //            {
            //                spd.Sheets[0].Rows[i].Visible = true;
            //            }
            //        }
            //    }
            //    return;
            //}

            //if (intChk != -1)
            //{
            //    if (strRowCol == "COL")
            //    {
            //        spd.Sheets[0].Rows[intChk].Visible = false;
            //    }
            //    else
            //    {
            //        spd.Sheets[0].Columns[intChk].Visible = false;
            //    }
            //}

            //clsSpread.setPrintHeadWhite(spd.Sheets[0]);
            //spd.Sheets[0].Cells[0, 0, spd.Sheets[0].RowCount - 1, spd.Sheets[0].ColumnCount - 1].BackColor = Color.White; //BackColor

            rtnVal = clsSpreadPrint.PrintSpdForm(pDbCon, strFormNo, strUpdateNo, po, blnAcp, strFrDate, strEndDate,
                                         spd, PrintType, MarginTop, MarginBottom, MarginLeft, MarginRight, UseSmartPrint, Orientation, strCurDateTime);
            Application.DoEvents();

            if (spd.Name.Length > 0)
            {
                foreach (KeyValuePair<int, Color> keyValue in RowFontColor)
                {
                    if (strRowCol.Equals("ROW"))
                    {
                        spd.ActiveSheet.Columns[keyValue.Key].ForeColor = keyValue.Value;
                    }
                    else
                    {
                        spd.ActiveSheet.Rows[keyValue.Key].ForeColor = keyValue.Value;
                    }
                }
            }

      

            //if (intChk != -1)
            //{
            //    if (strRowCol == "COL")
            //    {
            //        for (i = 0; i < spd.Sheets[0].ColumnCount; i++)
            //        {
            //            spd.Sheets[0].Columns[i].Visible = true;
            //        }
            //        spd.Sheets[0].Rows[intChk].Visible = true;
            //    }
            //    else
            //    {
            //        for (i = 0; i < spd.Sheets[0].RowCount; i++)
            //        {
            //            spd.Sheets[0].Rows[i].Visible = true;
            //        }
            //        spd.Sheets[0].Columns[intChk].Visible = true;
            //    }
            //}

            //clsSpread.setPrintHeadWhiteBack(spd.Sheets[0], Color.White);

            return rtnVal;
        }

        public static int PrintSpdFormCnt(PsmhDb pDbCon, string strFormNo, string strUpdateNo, EmrPatient po, bool blnAcp, string strFrDate, string strEndDate,
                                        FarPoint.Win.Spread.FpSpread spd, string PrintType,
                                        int MarginTop, int MarginBottom, int MarginLeft, int MarginRight, bool UseSmartPrint,
                                        FarPoint.Win.Spread.PrintOrientation Orientation, string strCurDateTime)
        {
            int rtnVal = 0;
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;

            pPrint = clsEmrChart.ClearPatient();
            fPrint = clsEmrChart.ClearEmrForm();

            pPrint = po;

            string strFont1= string.Empty;
            string strFont2= string.Empty;
            string strHead1= string.Empty;
            string strHead2= string.Empty;
            string strFooter1= string.Empty;
            string strFooter2= string.Empty;

            string strPRINTDATE = ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D");
            mstrPRINTNAME = clsType.User.UserName;

            //기록지 정보를 세팅
            fPrint = clsEmrChart.SerEmrFormInfo(pDbCon, mstrFormNo, mstrUpdateNo);

            //Print Head 지정
            strFont1 = "/fn\"굴림체\" /fz\"14\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            string strFont3 = "/fn\"굴림체\" /fz\"8\" /fb0 /fi0 /fu0 /fk0 /fs3";

            if (clsFormPrint.mstrPRINTFLAG == "1")
            {
                strHead1 = "/c/f1" + fPrint.FmFORMNAMEPRINT + " (원내용)" + "/f1/n/n";
            }
            else
            {
                strHead1 = "/c/f1" + fPrint.FmFORMNAMEPRINT + "/f1/n/n";
            }

            string strInOut = "내원일자";
            if (pPrint.inOutCls == "I")
            {
                strInOut = "입원일자";
            }

            strHead2 = "/l/f2" + "성명 : " + pPrint.ptName + " [" + pPrint.ptNo + "]   (" + pPrint.age + "/" + pPrint.sex + ")" + "/f2/n";
            if (clsFormPrint.mstrPRINTFLAG == "1" || clsFormPrint.mstrPRINTFLAG == "2")
            {
                strHead2 = strHead2 + "/l/f2" + "주민번호 : " + pPrint.ssno1 + "-" + (pPrint.ssno2.Length == 0 ? "X" : pPrint.ssno2.Substring(0, 1)) + "/f2/n";
            }
            else
            {
                strHead2 = strHead2 + "/l/f2" + "주민번호 : " + pPrint.ssno1 + "-" + VB.Left(pPrint.ssno2, 1) + "/f2/n";
            }

            if (blnAcp == true)
            {
                strHead2 = strHead2 + "/l/f2" + "진료과/주치의(" + strInOut + ") : " + pPrint.medDeptKorName + " / " + pPrint.medDrName + " (" + ComFunc.FormatStrToDate(pPrint.medFrDate, "D") + ")" + "/f2/n";
            }

            //if (clsCommon.gstrEXENAME != "MHEMRJOBDG.EXE")
            //{
            //    strHead2 = strHead2 + "/l/f2" + "작성일자 : " + ComFunc.FormatStrToDate(strFrDate, "D") + " ~ " + ComFunc.FormatStrToDate(strEndDate, "D") + "/f2/n";
            //}

            strHead2 = strHead2 + "/l/f2" + "출력자(출력일자) : " + mstrPRINTNAME + "(" + strPRINTDATE + ")" + "/f2/n/n";

            if (clsType.HosInfo.strIMAGEUSE == "1")
            {
                if (File.Exists(@"C:\Mentorsoft\exenet\hsplog.png"))
                {

                    Image ImagesX = new Bitmap(System.Drawing.Image.FromFile(@"C:\Mentorsoft\exenet\hsplog.png"));   //'병원 로고
                    Image pThumbnail = ImagesX.GetThumbnailImage(160, 35, delegate { return false; }, new IntPtr());

                    spd.Sheets[0].PrintInfo.Images = new Image[] { pThumbnail };   //'병원 로고
                    strFooter1 = "/l/g\"0\"";
                }
                else
                {
                    strFooter1 = "/l/f1" + clsType.HosInfo.strNameKor + "/f1";
                }
            }
            else
            {
                strFooter1 = "/l/f1" + clsType.HosInfo.strNameKor + "/f1";
            }

            if (clsFormPrint.mstrPRINTFLAG == "1")
            {
                strFooter2 = "/r/f3" + "본 의무기록지는 병원내부용으로만 사용가능합니다." + "/f3/n";
                //strFooter2 = strFooter2 + "/r/f3" + "외부유출시 법적인 효력을 인정받을 수 없습니다." + "/f3/n";
                strFooter2 = strFooter2 + "/c/f3" + "(/p" + " of " + "/pc)" + "/f3";
            }
            else
            {
                strFooter2 = "/r/f3" + "본 의무기록지는 작성자의 전자서명을 공인인증 받은 기록지입니다." + "/f3/n";
                //strFooter2 = strFooter2 + "/r/f2" + "This is an electronically authorized official medical record." + "/f2/n";
                strFooter2 = strFooter2 + "/c/f3" + "(/p" + " of " + "/pc)" + "/f3";
            }

            //clsSpread.setPrintHeadWhite(spd.Sheets[0]);

            //FarPoint.Win.Spread.PrintInfo pi = new FarPoint.Win.Spread.PrintInfo();
            //PrinterSettings ps = new PrinterSettings();

            //int i = 0;
            //for (i = 0; i <= ps.PaperSources.Count - 1; i++)
            //{ 
            //    comboBox1.Items.Add(ps.PaperSources[i].SourceName);
            //}
            //ps.PrinterName = "기록지";
            //pi.PaperSize = new System.Drawing.Printing.PaperSize("Letter", 600, 300);
            //pi.PaperSource = ps.PaperSources[0]; 
            //spd.Sheets[0].PrintInfo = pi;

            spd.Sheets[0].PrintInfo.Orientation = Orientation;
            spd.Sheets[0].PrintInfo.Margin.Top = MarginTop;
            spd.Sheets[0].PrintInfo.Margin.Bottom = MarginBottom;
            spd.Sheets[0].PrintInfo.Margin.Left = MarginLeft;
            spd.Sheets[0].PrintInfo.Margin.Right = MarginRight;
            spd.Sheets[0].PrintInfo.HeaderHeight = 120;
            spd.Sheets[0].PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            //spd.Sheets[0].PrintInfo.FooterHeight = 100;
            spd.Sheets[0].PrintInfo.Footer = strFont1 + strFooter1 + strFont3 + strFooter2;
            spd.Sheets[0].PrintInfo.ShowColor = false;
            spd.Sheets[0].PrintInfo.ShowBorder = true;
            spd.Sheets[0].PrintInfo.ShowGrid = true;
            spd.Sheets[0].PrintInfo.ShowShadows = false;
            spd.Sheets[0].PrintInfo.UseMax = false;
            spd.Sheets[0].PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            spd.Sheets[0].PrintInfo.UseSmartPrint = UseSmartPrint;

            if (PrintType == "P")
            {
                spd.PrintSheet(0);
            }
            else if (PrintType == "V")
            {
                spd.Sheets[0].PrintInfo.ShowPrintDialog = true;
                spd.Sheets[0].PrintInfo.Preview = true;
                spd.PrintSheet(0);
            }


            return rtnVal;
            //clsSpread.setPrintHeadWhiteBack(spd.Sheets[0], Color.White);
        }

        public static int PrintSpdFormCntICU(PsmhDb pDbCon, string strFormNo, string strUpdateNo, EmrPatient po, bool blnAcp, string strFrDate, string strEndDate,
                                        FarPoint.Win.Spread.SheetView spd, string PrintType,
                                        int MarginTop, int MarginBottom, int MarginLeft, int MarginRight, bool UseSmartPrint,
                                        FarPoint.Win.Spread.PrintOrientation Orientation, string strCurDateTime, FarPoint.Win.Spread.FpSpread ssPrint, int intSheetSet)
        {
            int rtnVal = 0;
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;

            pPrint = clsEmrChart.ClearPatient();
            fPrint = clsEmrChart.ClearEmrForm();

            pPrint = po;

            string strFont1= string.Empty;
            string strFont2= string.Empty;
            string strHead1= string.Empty;
            string strHead2= string.Empty;
            string strFooter1= string.Empty;
            string strFooter2= string.Empty;

            string strPRINTDATE = ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D");
            mstrPRINTNAME = clsType.User.UserName;

            //기록지 정보를 세팅
            fPrint = clsEmrChart.SerEmrFormInfo(pDbCon, mstrFormNo, mstrUpdateNo);

            //Print Head 지정
            strFont1 = "/fn\"굴림체\" /fz\"14\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"10\" /fb0 /fi0 /fu0 /fk0 /fs2";
            string strFont3 = "/fn\"굴림체\" /fz\"8\" /fb0 /fi0 /fu0 /fk0 /fs3";

            if (clsFormPrint.mstrPRINTFLAG == "1")
            {
                strHead1 = "/c/f1" + fPrint.FmFORMNAMEPRINT + " (원내용)" + "/f1/n/n";
            }
            else
            {
                strHead1 = "/c/f1" + fPrint.FmFORMNAMEPRINT + "/f1/n/n";
            }

            string strInOut = "내원일자";
            if (pPrint.inOutCls == "I")
            {
                strInOut = "입원일자";
            }

            strHead2 = "/l/f2" + "성명 : " + pPrint.ptName + " [" + pPrint.ptNo + "]   (" + pPrint.age + "/" + pPrint.sex + ")" + "/f2/n";
            if (clsFormPrint.mstrPRINTFLAG == "1" || clsFormPrint.mstrPRINTFLAG == "2")
            {
                strHead2 = strHead2 + "/l/f2" + "주민번호 : " + pPrint.ssno1 + "-" + (pPrint.ssno2.Length == 0 ? "X" : pPrint.ssno2.Substring(0, 1)) + "/f2/n";
            }
            else
            {
                strHead2 = strHead2 + "/l/f2" + "주민번호 : " + pPrint.ssno1 + "-" + VB.Left(pPrint.ssno2, 1) + "/f2/n";
            }

            if (blnAcp == true)
            {
                strHead2 = strHead2 + "/l/f2" + "진료과/주치의(" + strInOut + ") : " + pPrint.medDeptKorName + " / " + pPrint.medDrName + " (" + ComFunc.FormatStrToDate(pPrint.medFrDate, "D") + ")" + "/f2/n";
            }

            //if (clsCommon.gstrEXENAME != "MHEMRJOBDG.EXE")
            //{
            //    strHead2 = strHead2 + "/l/f2" + "작성일자 : " + ComFunc.FormatStrToDate(strFrDate, "D") + " ~ " + ComFunc.FormatStrToDate(strEndDate, "D") + "/f2/n";
            //}

            strHead2 = strHead2 + "/l/f2" + "출력자(출력일자) : " + mstrPRINTNAME + "(" + strPRINTDATE + ")" + "/f2/n/n";

            if (clsType.HosInfo.strIMAGEUSE == "1")
            {
                if (File.Exists(@"C:\Mentorsoft\exenet\hsplog.png"))
                {

                    Image ImagesX = new Bitmap(System.Drawing.Image.FromFile(@"C:\Mentorsoft\exenet\hsplog.png"));   //'병원 로고
                    Image pThumbnail = ImagesX.GetThumbnailImage(160, 35, delegate { return false; }, new IntPtr());

                    spd.PrintInfo.Images = new Image[] { pThumbnail };   //'병원 로고
                    strFooter1 = "/l/g\"0\"";
                }
                else
                {
                    strFooter1 = "/l/f1" + clsType.HosInfo.strNameKor + "/f1";
                }
            }
            else
            {
                strFooter1 = "/l/f1" + clsType.HosInfo.strNameKor + "/f1";
            }

            if (clsFormPrint.mstrPRINTFLAG == "1")
            {
                strFooter2 = "/r/f3" + "본 의무기록지는 병원내부용으로만 사용가능합니다." + "/f3/n";
                strFooter2 = strFooter2 + "/c/f3" + "(/p" + " of " + "/pc)" + "/f3";
            }
            else
            {
                strFooter2 = "/r/f3" + "본 의무기록지는 작성자의 전자서명을 공인인증 받은 기록지입니다." + "/f3/n";
                strFooter2 = strFooter2 + "/c/f3" + "(/p" + " of " + "/pc)" + "/f3";
            }

            spd.PrintInfo.Orientation = Orientation;
            spd.PrintInfo.Margin.Top = MarginTop;
            spd.PrintInfo.Margin.Bottom = MarginBottom;
            spd.PrintInfo.Margin.Left = MarginLeft;
            spd.PrintInfo.Margin.Right = MarginRight;
            spd.PrintInfo.HeaderHeight = 120;
            spd.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            spd.PrintInfo.Footer = strFont1 + strFooter1 + strFont3 + strFooter2;
            spd.PrintInfo.ShowColor = false;
            spd.PrintInfo.ShowBorder = true;
            spd.PrintInfo.ShowGrid = true;
            spd.PrintInfo.ShowShadows = false;
            spd.PrintInfo.UseMax = false;
            spd.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            spd.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            spd.PrintInfo.UseSmartPrint = UseSmartPrint;

            if (PrintType == "P")
            {
                ssPrint.PrintSheet(intSheetSet);
            }
            else if (PrintType == "V")
            {
                spd.PrintInfo.ShowPrintDialog = true;
                spd.PrintInfo.Preview = true;
                ssPrint.PrintSheet(intSheetSet);
            }
            return rtnVal;
        }


        /// <summary>
        /// 마취기록지 출력
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="po"></param>
        /// <param name="blnAcp"></param>
        /// <param name="strFrDate"></param>
        /// <param name="strEndDate"></param>
        /// <param name="spd"></param>
        /// <param name="PrintType"></param>
        /// <param name="MarginTop"></param>
        /// <param name="MarginBottom"></param>
        /// <param name="MarginLeft"></param>
        /// <param name="MarginRight"></param>
        /// <param name="UseSmartPrint"></param>
        /// <param name="Orientation"></param>
        /// <param name="strCurDateTime"></param>
        /// <returns></returns>
        public static int PrintSpdAnForm(PsmhDb pDbCon, string strFormNo, string strUpdateNo, EmrPatient po, bool blnAcp, ref FarPoint.Win.Spread.FpSpread spd, string PrintType,
                                        int MarginTop, int MarginBottom, int MarginLeft, int MarginRight, bool UseSmartPrint,
                                        FarPoint.Win.Spread.PrintOrientation Orientation, string strCurDateTime, int Page, int totPage, float ZoomFactor)
        {
            int rtnVal = 0;
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;

            pPrint = clsEmrChart.ClearPatient();
            fPrint = clsEmrChart.ClearEmrForm();

            pPrint = po;

            string strFont1= string.Empty;
            string strFont2= string.Empty;
            string strHead1= string.Empty;
            string strHead2= string.Empty;
            string strFooter1= string.Empty;
            string strFooter2= string.Empty;

            string strPRINTDATE = ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D");
            mstrPRINTNAME = clsType.User.UserName;

            //기록지 정보를 세팅
            fPrint = clsEmrChart.SerEmrFormInfo(pDbCon, mstrFormNo, mstrUpdateNo);

            //Print Head 지정
            strFont1 = "/fn\"굴림체\"/fz\"14\"/fb1/fi0/fu0/fk0/fs1";
            strFont2 = "/fn\"굴림체\"/fz\"10\"/fb0/fi0/fu0/fk0/fs2";
            string strFont3 = "/fn\"굴림체\"/fz\"8\"/fb0/fi0/fu0/fk0/fs3";

            if (po == null )
            {
                if (clsFormPrint.mstrPRINTFLAG == "1")
                {
                    strHead1 = "/c/f1" + fPrint.FmFORMNAMEPRINT + " (원내용)" + "/f1/n/n";
                }
                else
                {
                    strHead1 = "/c/f1" + fPrint.FmFORMNAMEPRINT + "/f1/n/n";
                }

                string strInOut = "내원일자";
                

                strHead2 = "/l/f2" + "성명 :       나이 : " + "" + "/ 성별 : " + "" + "  " + "병동 : " + "" + "/f2/n";
                strHead2 = strHead2 + "/l/f2" + "주민번호 : ";

            }
            else
            {
                if (clsFormPrint.mstrPRINTFLAG == "1")
                {
                    strHead1 = "/c/f1" + fPrint.FmFORMNAMEPRINT + " (원내용)" + "/f1/n/n";
                }
                else
                {
                    strHead1 = "/c/f1" + fPrint.FmFORMNAMEPRINT + "/f1/n/n";
                }

                string strInOut = "내원일자";
                if (pPrint.inOutCls == "I")
                {
                    strInOut = "입원일자";
                }

                strHead2 = "/l/f2" + "성명 : " + pPrint.ptName + " [" + pPrint.ptNo + "]   나이 : " + pPrint.age + "/ 성별 : " + pPrint.sex + "  " + "병동 : " + pPrint.ward + "/f2/n";
                if (clsFormPrint.mstrPRINTFLAG == "1" || clsFormPrint.mstrPRINTFLAG == "2")
                {
                    strHead2 = strHead2 + "/l/f2" + "주민번호 : " + pPrint.ssno1 + "-" + (pPrint.ssno2.Length == 0 ? "X" : pPrint.ssno2.Substring(0, 1)) + "/f2/n";
                }
                else
                {
                    strHead2 = strHead2 + "/l/f2" + "주민번호 : " + pPrint.ssno1 + "-" + VB.Left(pPrint.ssno2, 1) + "/f2/n";
                }

                if (blnAcp == true)
                {
                    strHead2 = strHead2 + "/l/f2" + "진료과/주치의(" + strInOut + ") : " + pPrint.medDeptKorName + " / " + pPrint.medDrName + " (" + ComFunc.FormatStrToDate(pPrint.medFrDate, "D") + ")" + "/f2/n";
                }

                strHead2 = strHead2 + "/l/f2" + "수술일자: " + pPrint.opdate + ", " + "작성자(작성일자): " + pPrint.writeName + "(" + Convert.ToInt32(pPrint.writeDate).ToString("0000-00-00") + ")";

            }



            strHead2 = strHead2 + "/r" + "출력자(출력일자) : " + mstrPRINTNAME + "(" + strPRINTDATE + ")" + "/f2/n/n";

            if (clsType.HosInfo.strIMAGEUSE == "1")
            {
                if (File.Exists(@"C:\PSMHEXE\exenet\hsplog.png"))
                {

                    Image pThumbnail;
                    using (Image ImagesX = new Bitmap(Image.FromFile(@"C:\PSMHEXE\exenet\hsplog.png")))   //'병원 로고
                    {
                        pThumbnail = ImagesX.GetThumbnailImage(160, 35, delegate { return false; }, new IntPtr());
                    }


                    spd.Sheets[0].PrintInfo.Images = new Image[] { pThumbnail };   //'병원 로고
                    strFooter1 = "/l/g\"0\"";
                }
                else
                {
                    strFooter1 = "/l/f1" + clsType.HosInfo.strNameKor + "/f1";
                }
            }
            else
            {
                strFooter1 = "/l/f1" + clsType.HosInfo.strNameKor + "/f1";
            }

            if (clsFormPrint.mstrPRINTFLAG == "1")
            {
                strFooter2 = "/r/f3" + "본 의무기록지는 병원내부용으로만 사용가능합니다." + "/f3/n";
                //strFooter2 = strFooter2 + "/r/f3" + "외부유출시 법적인 효력을 인정받을 수 없습니다." + "/f3/n";
                strFooter2 = strFooter2 + "/c/f3" + "(" + Page + " of " + totPage + ")" + "/f3";
            }
            else
            {
                strFooter2 = "/r/f3" + "본 의무기록지는 작성자의 전자서명을 공인인증 받은 기록지입니다." + "/f3/n";
                //strFooter2 = strFooter2 + "/r/f2" + "This is an electronically authorized official medical record." + "/f2/n";
                strFooter2 = strFooter2 + "/c/f3" + "(" + Page + " of " + totPage + ")" + "/f3";
            }

            //clsSpread.setPrintHeadWhite(spd.Sheets[0]);

            //FarPoint.Win.Spread.PrintInfo pi = new FarPoint.Win.Spread.PrintInfo();
            //PrinterSettings ps = new PrinterSettings();

            //int i = 0;
            //for (i = 0; i <= ps.PaperSources.Count - 1; i++)
            //{ 
            //    comboBox1.Items.Add(ps.PaperSources[i].SourceName);
            //}
            //ps.PrinterName = "기록지";
            //pi.PaperSize = new System.Drawing.Printing.PaperSize("Letter", 600, 300);
            //pi.PaperSource = ps.PaperSources[0]; 
            //spd.Sheets[0].PrintInfo = pi;


            spd.Sheets[0].PrintInfo.Orientation = Orientation;
            spd.Sheets[0].PrintInfo.Margin.Top = MarginTop;
            spd.Sheets[0].PrintInfo.Margin.Bottom = MarginBottom;
            spd.Sheets[0].PrintInfo.Margin.Left = MarginLeft;
            spd.Sheets[0].PrintInfo.Margin.Right = MarginRight;
            spd.Sheets[0].PrintInfo.HeaderHeight = 100;
            spd.Sheets[0].PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            //spd.Sheets[0].PrintInfo.FooterHeight = 100;
            spd.Sheets[0].PrintInfo.Footer = strFont1 + strFooter1 + strFont3 + strFooter2;
            spd.Sheets[0].PrintInfo.ShowColor = false;
            spd.Sheets[0].PrintInfo.ShowBorder = false;
            spd.Sheets[0].PrintInfo.ShowGrid = false;
            spd.Sheets[0].PrintInfo.ShowShadows = false;
            spd.Sheets[0].PrintInfo.UseMax = false;
            spd.Sheets[0].PrintInfo.ZoomFactor = ZoomFactor;
            spd.Sheets[0].PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            spd.Sheets[0].PrintInfo.UseSmartPrint = UseSmartPrint;

            //if (PrintType == "P")
            //{
            //    spd.PrintSheet(0);
            //    ComFunc.Delay(500);
            //}
            //else if (PrintType == "V")
            //{
            //    spd.Sheets[0].PrintInfo.ShowPrintDialog = true;
            //    spd.Sheets[0].PrintInfo.Preview = true;
            //    spd.PrintSheet(0);
            //}

            if (PrintType == "V")
            {
                spd.Sheets[0].PrintInfo.ShowPrintDialog = true;
                spd.Sheets[0].PrintInfo.Preview = true;                
            }

            int[] intPage = spd.GetRowPageBreaks(0);
            //if (Orientation == FarPoint.Win.Spread.PrintOrientation.Landscape)
            //{
            //    intPage = spd.GetColumnPageBreaks(0);
            //}
            //else
            //{
            //}
            rtnVal = intPage.Length + 1;

            return rtnVal;
            //clsSpread.setPrintHeadWhiteBack(spd.Sheets[0], Color.White);
        }
    }
}
