using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Data;
using System.Drawing;

namespace ComPmpaLibB
{
    public class clsPmpaPrint
{
        public const string CARD_PRINTER_NAME = "신용카드";
        public const string JUP_PRINTER_NAME = "접수증";
        public const string RES_PRINTER_NAME = "예약증";
        public const string RECEIPT_PRINTER_NAME = "영수증";
        public const string DRUG_PRINTER_NAME = "원외처방전";
        public const string ARTICLE_PRINTER_NAME = "혈액환자정보";
        public const string MCRT_PRINTER_NAME = "증명서";

        public static string GstrPrtBand = string.Empty;      //환자용 인식밴드 출력(입력) 위치
        string FstrPtno = "";
        string FstrSname = "";
        string FstrDept = "";
        string FstrDeptName = "";
        string FstrDr = "";
        string FstrRdate = "";
        string[] FstrSite = new string[10];
        int FnPo = 0;

        long FBAmt = 0;
        long FJAmt = 0;
        int FnTmp = 0;

        clsDrugPrint cDP = new clsDrugPrint();

        public struct Argument_Table
        {
            public string Ptno;
            public string DrCode;
            public string Date;
            public string Dept;
            public string Sex;
            public string GbSpc;
            public string GbGamek;
            public string GbGamekC;
            public int Retn;
            public int Bi;
            public int Bi1;
            public int Age;
            public int AgeIlsu;
            public string GbIlban2;
        }

        public struct Report_Add_Amt_Table
        {
            public long Amt1; //급여금액-합
            public long Amt2; //비급여금액-합
            public long Amt3; //특진료-합
            public long Amt4; //본인총액-합
            public long Amt5; //본인
            public long Amt6; //공단
            public long Amt7; //선별급여 총액
            public long Amt8; //선별급여 공단
            public long Amt9; //선별급여 본인
        }

        public struct Slip_ReportPrint_Table
        {
            public string SuCode;
            public string SuNext;
            public string Bi;
            public string BDate;
            public string Bun;
            public string Nu;
            public double Qty;
            public int Nal;
            public long BaseAmt;
            public string GbSpc;
            public string GbNgt;
            public string GbGisul;
            public string GbSelf;
            public string GbChild;
            public string DrCode;
            public string DeptCode;
            public string WardCode;
            public string GbSlip;
            public string GbHost;
            public long Amt1;
            public long Amt2;
            public double OrderNo;
            public string GbImiv;
            public string DosCode;
            public string GbBunup;
            public string GbIPD;
            public int Div;
            public string KsJin;
            public long DanAmt;
            public string GbSpc_No;
            public double Amt4;
            public string SugbS;
        }

        /// <summary>
        /// Description : 로컬 프린터집합에서 해당 프린터를 검색
        /// Author : 박병규
        /// Create Date : 2017.08.11
        /// </summary>
        public void Printer_Setting()
        {
            clsPmpaPb.GnPrtIpdNo = 0;

            try
            {
                for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
                {
                    if (PrinterSettings.InstalledPrinters[i].ToString().Trim() == "영수증")
                        clsPmpaPb.GnPrtIpdNo = i;
                    else if (PrinterSettings.InstalledPrinters[i].ToString().Trim() == "원외처방전")
                        clsPmpaPb.GnPrtIpdNo5 = i;
                    else if (PrinterSettings.InstalledPrinters[i].ToString().Trim() == "신용카드" ||
                             VB.Right(PrinterSettings.InstalledPrinters[i].ToString().Trim(), 8) == "신용카드" ||
                             PrinterSettings.InstalledPrinters[i].ToString().Trim() == "카드영수증" ||
                             VB.Right(PrinterSettings.InstalledPrinters[i].ToString().Trim(), 10) == "카드영수증")
                        clsPmpaPb.GnPrtIpdNo3 = i;

                    //종검스테이션 셋팅용
                    if (VB.Left(clsPmpaPb.GstrPrtBun, 1) == "5" || VB.Left(clsPmpaPb.GstrPrtBun, 1) == "6" ||
                        (clsPmpaPb.GstrZoneID == "14" && clsPmpaPb.GstrDeskID == "2"))
                    {
                        if (PrinterSettings.InstalledPrinters[i].ToString().Trim() == "영수증" ||
                             VB.Right(PrinterSettings.InstalledPrinters[i].ToString().Trim(), 6) == "영수증")
                            clsPmpaPb.GnPrtIpdNo = i;

                        if (PrinterSettings.InstalledPrinters[i].ToString().Trim() == "센터영수증" ||
                             VB.Right(PrinterSettings.InstalledPrinters[i].ToString().Trim(), 10) == "센터영수증")
                            clsPmpaPb.GnPrtIpdNo = i;

                        if (PrinterSettings.InstalledPrinters[i].ToString().Trim() == "원외처방전" ||
                             VB.Right(PrinterSettings.InstalledPrinters[i].ToString().Trim(), 10) == "원외처방전")
                            clsPmpaPb.GnPrtIpdNo5 = i;

                        if (PrinterSettings.InstalledPrinters[i].ToString().Trim() == "신용카드" ||
                             VB.Right(PrinterSettings.InstalledPrinters[i].ToString().Trim(), 8) == "신용카드")
                            clsPmpaPb.GnPrtIpdNo3 = i;

                        if (PrinterSettings.InstalledPrinters[i].ToString().Trim() == "카드영수증" ||
                             VB.Right(PrinterSettings.InstalledPrinters[i].ToString().Trim(), 10) == "카드영수증")
                            clsPmpaPb.GnPrtIpdNo3 = i;
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.ToString());
            }
        }
        
        /// <summary>
        /// Description : 예약검사 대체영수증 출력
        /// Author : 박병규
        /// Create Date : 2017.07.25
        /// <param name="ArgPtno">등록번호</param>
        /// <param name="ArgGwa">진료과목명</param>
        /// <param name="ArgName">환자명</param>
        /// <param name="ArgDr">의사코드</param>
        /// <param name="ArgBi">보험종류</param>
        /// <param name="ArgBDate">처방일자</param>
        /// <param name="ArgDept">진료과목코드</param>
        /// <param name="ArgPicture_Sign"></param>
        /// <param name="ArgSpc">특진여부</param>
        /// <param name="ArgJeaPrt"></param>
        /// <param name="ArgAmt6">대체금액</param>
        /// <param name="ArgWRTNO">고유번호</param>
        /// <history>사용안함 >> Report_Print_ResvExam 사용 </history>
        /// <seealso cref="OUMSAD2.bas : Report_Print_ResvExam"/>
        public void Report_Print_ResvExam_old(string ArgPtno, string ArgGwa, string ArgName, string ArgDr, string ArgBi, string ArgBDate, string ArgDept, PictureBox o, string ArgSpc, string ArgJeaPrt, long ArgAmt6, long ArgWRTNO)
        {



        }

        /// <summary>
        /// Description : 예약검사 대체영수증 출력
        /// Author : 박병규
        /// Create Date : 2017.07.25
        /// <param name="ArgPtno">등록번호</param>
        /// <param name="ArgGwa">진료과목명</param>
        /// <param name="ArgName">환자명</param>
        /// <param name="ArgDr">의사코드</param>
        /// <param name="ArgBi">보험종류</param>
        /// <param name="ArgBDate">처방일자</param>
        /// <param name="ArgDept">진료과목코드</param>
        /// <param name="ArgPicture_Sign"></param>
        /// <param name="ArgSpc">특진여부</param>
        /// <param name="ArgJeaPrt"></param>
        /// <param name="ArgAmt6">대체금액</param>
        /// <param name="ArgWRTNO">고유번호</param>
        /// <seealso cref="OUMSAD2.bas : Report_Print_ResvExam_2012"/>
        public void Report_Print_ResvReceipt(PsmhDb pDbCon, string ArgPtno, string ArgGwa, string ArgName, string ArgDr, string ArgBi, string ArgBDate, string ArgDept, PictureBox o, string ArgSpc, string ArgJeaPrt, long ArgAmt6, long ArgWRTNO)
        {
            FarPoint.Win.Spread.FpSpread ssSpread = null;
            FarPoint.Win.Spread.SheetView ssSpread_Sheet = null;

            ComFunc CF = new ComFunc();
            clsOumsadChk COC = new ComPmpaLibB.clsOumsadChk();
            clsDrugPrint clsDP = new clsDrugPrint();
            PrintDocument pd = new PrintDocument();
            PageSettings ps = new PageSettings();
            PrintController pc = new StandardPrintController();

            DataTable DtP = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            clsDP.SetReciptPrint(ref ssSpread, ref ssSpread_Sheet);
            clsDP.ClearReciptPrint(ssSpread, ssSpread_Sheet);

            //영수증출력
            clsPrint CP = new clsPrint();
            Card CC = new Card();

            string strPrintName = CP.getPmpaBarCodePrinter(RECEIPT_PRINTER_NAME);

            ssSpread_Sheet.Cells[0, 4].Text = "외래";
            ssSpread_Sheet.Cells[0, 12].Text = ArgPtno + VB.Asc(VB.Left(ArgDept, 1)) + VB.Asc(VB.Right(ArgDept, 1)); //바코드

            //공급받는자보관용
            ssSpread_Sheet.Cells[3, 0].Text = ArgPtno;//등록번호
            ssSpread_Sheet.Cells[3, 4].Text = ArgName;//수진자명

            if (clsPmpaPb.GstrJeaPrint == "OK") //진료일자
                ssSpread_Sheet.Cells[3, 8].Text = ArgBDate;
            else
                ssSpread_Sheet.Cells[3, 8].Text = clsPublic.GstrSysDate;

            ssSpread_Sheet.Cells[5, 0].Text = ArgGwa;//진료과목
            ssSpread_Sheet.Cells[5, 11].Text = clsPmpaPb.GstrSetBis[Convert.ToInt32(VB.Val(ArgBi))];//환자구분
            ssSpread_Sheet.Cells[5, 12].Text = clsType.User.JobName + "(" + clsType.User.IdNumber + ") " + ArgWRTNO;

            //예약검사비 - 선수금
            if (ArgAmt6 != 0)
            {
                if (ArgAmt6 > 0)
                {
                    if (ArgJeaPrt == "2")
                        ssSpread_Sheet.Cells[11, 11].Text = "예약선수금대체";
                    else
                        ssSpread_Sheet.Cells[11, 11].Text = "예약선수금";
                }
                else
                    ssSpread_Sheet.Cells[11, 11].Text = "예약환불비";

                ssSpread_Sheet.Cells[11, 12].Text = string.Format("{0:#,###}", ArgAmt6);
            }

            //납부할금액
            ssSpread_Sheet.Cells[13, 13].Text = string.Format("{0:#,###}", ArgAmt6);

            //납부한금액
            if (clsPmpaType.RSD.Gubun == "1")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {
                    ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//카드
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (ArgAmt6 - (clsPmpaType.RSD.TotAmt * -1)));//현금
                }
                else
                {
                    ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//카드
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (ArgAmt6 - clsPmpaType.RSD.TotAmt));//현금
                }
            }
            else if (clsPmpaType.RSD.Gubun == "2")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {
                    ssSpread_Sheet.Cells[16, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//현금영수증
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (ArgAmt6 - (clsPmpaType.RSD.TotAmt * -1)));//현금

                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[28, 12].Text = "현금승인취소";
                        ssSpread_Sheet.Cells[29, 11].Text = VB.Left(Card.GstrCashCard.Trim(), Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[32, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
                else
                {
                    ssSpread_Sheet.Cells[16, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//현금영수증
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (ArgAmt6 - clsPmpaType.RSD.TotAmt));//현금

                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[28, 12].Text = "현금승인";
                        ssSpread_Sheet.Cells[29, 11].Text = VB.Left(Card.GstrCashCard.Trim(), Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[32, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
            }
            else
            {
                ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", ArgAmt6);//현금
            }

            ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", ArgAmt6);//합계

            if (ArgJeaPrt == "1")
            {
                //2018.06.20 박병규 : 출력위치변경
                //ssSpread_Sheet.Cells[22, 12].ColumnSpan = 4;
                //ssSpread_Sheet.Cells[22, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                //ssSpread_Sheet.Cells[22, 12].Text = "사 본";
                ssSpread_Sheet.Cells[0, 4].Text += " [사본]";

                ssSpread_Sheet.Cells[23, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[23, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[23, 12].Text = "예약선수금은";
                ssSpread_Sheet.Cells[24, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[24, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[24, 12].Text = "영수증/연말정산용으로 사용불가";
                ssSpread_Sheet.Cells[25, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[25, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[25, 12].Text = "※ 검사 당일 영수증 발행됨.";
            }
            else
            {
                ssSpread_Sheet.Cells[22, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[22, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[22, 12].Text = "예약선수금은";
                ssSpread_Sheet.Cells[23, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[23, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[23, 12].Text = "영수증/연말정산용으로 사용불가";
                ssSpread_Sheet.Cells[24, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[24, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[24, 12].Text = "※ 검사 당일 영수증 발행됨.";
            }


            ssSpread_Sheet.Cells[40, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime;

            //카드영수증 통합
            if (clsPmpaType.RSD.CardSeqNo > 0)
            {
                Card.GstrCardApprov_Info = CC.Card_Sign_Info_Set(pDbCon, clsPmpaType.RSD.CardSeqNo, ArgPtno);

                if (Card.GstrCardApprov_Info != "")
                {
                    ssSpread_Sheet.Cells[29, 13].ColumnSpan = 3;
                    ssSpread_Sheet.Cells[29, 13].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[29, 13].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 24); //거래구분

                    ssSpread_Sheet.Cells[30, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[30, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[30, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 4) + " " + VB.Pstr(Card.GstrCardApprov_Info, "{}", 2); //카드종류/카드번호
                    ssSpread_Sheet.Cells[31, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[31, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[31, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 20); //승인금액
                    ssSpread_Sheet.Cells[32, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[32, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[32, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 12); //승인번호
                    ssSpread_Sheet.Cells[33, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[33, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[33, 12].Text = "72117503"; //가맹점번호
                    ssSpread_Sheet.Cells[34, 12].ColumnSpan = 2;
                    ssSpread_Sheet.Cells[34, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[34, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 14); //거래일시
                    ssSpread_Sheet.Cells[34, 15].Text = VB.Pstr(VB.Pstr(Card.GstrCardApprov_Info, "{}", 8), "개월", 1);
                }

                Card.GstrCardApprov_Info = "";
            }

            clsDP.PrintReciptPrint(ssSpread, ssSpread_Sheet, strPrintName);
        }
        public void Report_Print_ResvReceipt_New(PsmhDb pDbCon, string ArgPtno, string ArgGwa, string ArgName, string ArgDr, string ArgBi, string ArgBDate, string ArgDept, PictureBox o, string ArgSpc, string ArgJeaPrt, long ArgAmt6, long ArgWRTNO)
        {
            FarPoint.Win.Spread.FpSpread ssSpread = null;
            FarPoint.Win.Spread.SheetView ssSpread_Sheet = null;

            ComFunc CF = new ComFunc();
            clsOumsadChk COC = new ComPmpaLibB.clsOumsadChk();
            clsDrugPrint clsDP = new clsDrugPrint();
            PrintDocument pd = new PrintDocument();
            PageSettings ps = new PageSettings();
            PrintController pc = new StandardPrintController();

            DataTable DtP = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            clsDP.SetReciptPrint_New(ref ssSpread, ref ssSpread_Sheet);
            clsDP.ClearReciptPrint_New(ssSpread, ssSpread_Sheet);

            //영수증출력
            clsPrint CP = new clsPrint();
            Card CC = new Card();

            string strPrintName = CP.getPmpaBarCodePrinter(RECEIPT_PRINTER_NAME);

            ssSpread_Sheet.Cells[0, 4].Text = "외래";
            ssSpread_Sheet.Cells[0, 12].Text = ArgPtno + VB.Asc(VB.Left(ArgDept, 1)) + VB.Asc(VB.Right(ArgDept, 1)); //바코드

            //공급받는자보관용
            ssSpread_Sheet.Cells[3, 0].Text = ArgPtno;//등록번호
            ssSpread_Sheet.Cells[3, 4].Text = ArgName;//수진자명

            if (clsPmpaPb.GstrJeaPrint == "OK") //진료일자
                ssSpread_Sheet.Cells[3, 8].Text = ArgBDate;
            else
                ssSpread_Sheet.Cells[3, 8].Text = clsPublic.GstrSysDate;

            ssSpread_Sheet.Cells[5, 0].Text = ArgGwa;//진료과목
            ssSpread_Sheet.Cells[5, 11].Text = clsPmpaPb.GstrSetBis[Convert.ToInt32(VB.Val(ArgBi))];//환자구분
            ssSpread_Sheet.Cells[5, 12].Text = clsType.User.JobName + "(" + clsType.User.IdNumber + ") " + ArgWRTNO;

            //예약검사비 - 선수금
            if (ArgAmt6 != 0)
            {
                if (ArgAmt6 > 0)
                {
                    if (ArgJeaPrt == "2")
                        ssSpread_Sheet.Cells[13, 11].Text = "예약선수금대체";
                    else
                        ssSpread_Sheet.Cells[13, 11].Text = "예약선수금";
                }
                else
                    ssSpread_Sheet.Cells[13, 11].Text = "예약환불비";

                ssSpread_Sheet.Cells[13, 12].Text = string.Format("{0:#,###}", ArgAmt6);
            }

            //납부할금액
            ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", ArgAmt6);

            //납부한금액
            if (clsPmpaType.RSD.Gubun == "1")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//카드
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (ArgAmt6 - (clsPmpaType.RSD.TotAmt * -1)));//현금
                }
                else
                {
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//카드
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (ArgAmt6 - clsPmpaType.RSD.TotAmt));//현금
                }
            }
            else if (clsPmpaType.RSD.Gubun == "2")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {
                    ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//현금영수증
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (ArgAmt6 - (clsPmpaType.RSD.TotAmt * -1)));//현금

                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[30, 12].Text = "현금승인취소";
                        ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard.Trim(), Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
                else
                {
                    ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//현금영수증
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (ArgAmt6 - clsPmpaType.RSD.TotAmt));//현금

                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[30, 12].Text = "현금승인";
                        ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard.Trim(), Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
            }
            else
            {
                ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", ArgAmt6);//현금
            }

            ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", ArgAmt6);//합계

            if (ArgJeaPrt == "1")
            {
                //2018.06.20 박병규 : 출력위치변경
                //ssSpread_Sheet.Cells[22, 12].ColumnSpan = 4;
                //ssSpread_Sheet.Cells[22, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                //ssSpread_Sheet.Cells[22, 12].Text = "사 본";
                ssSpread_Sheet.Cells[0, 4].Text += " [사본]";

                ssSpread_Sheet.Cells[25, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[25, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[25, 12].Text = "예약선수금은";
                ssSpread_Sheet.Cells[26, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[26, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[26, 12].Text = "영수증/연말정산용으로 사용불가";
                ssSpread_Sheet.Cells[27, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[27, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[27, 12].Text = "※ 검사 당일 영수증 발행됨.";
            }
            else
            {
                ssSpread_Sheet.Cells[24, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[24, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[24, 12].Text = "예약선수금은";
                ssSpread_Sheet.Cells[25, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[25, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[25, 12].Text = "영수증/연말정산용으로 사용불가";
                ssSpread_Sheet.Cells[26, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[26, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[26, 12].Text = "※ 검사 당일 영수증 발행됨.";
            }


            ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime;

            //카드영수증 통합
            if (clsPmpaType.RSD.CardSeqNo > 0)
            {
                Card.GstrCardApprov_Info = CC.Card_Sign_Info_Set(pDbCon, clsPmpaType.RSD.CardSeqNo, ArgPtno);

                if (Card.GstrCardApprov_Info != "")
                {
                    ssSpread_Sheet.Cells[31, 13].ColumnSpan = 3;
                    ssSpread_Sheet.Cells[31, 13].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[31, 13].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 24); //거래구분

                    ssSpread_Sheet.Cells[32, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[32, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[32, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 4) + " " + VB.Pstr(Card.GstrCardApprov_Info, "{}", 2); //카드종류/카드번호
                    ssSpread_Sheet.Cells[33, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[33, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[33, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 20); //승인금액
                    ssSpread_Sheet.Cells[34, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[34, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[34, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 12); //승인번호
                    ssSpread_Sheet.Cells[35, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[35, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[35, 12].Text = "72117503"; //가맹점번호
                    ssSpread_Sheet.Cells[36, 12].ColumnSpan = 2;
                    ssSpread_Sheet.Cells[36, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[36, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 14); //거래일시
                    ssSpread_Sheet.Cells[36, 15].Text = VB.Pstr(VB.Pstr(Card.GstrCardApprov_Info, "{}", 8), "개월", 1);
                }

                Card.GstrCardApprov_Info = "";
            }
            strPrintName = VB.Replace(strPrintName, "(리디렉션)", "");
            strPrintName = VB.Replace(strPrintName, "(리디렉션 1)", "");
            strPrintName = VB.Replace(strPrintName, "(리디렉션 2)", "");
            clsDP.PrintReciptPrint_New(ssSpread, ssSpread_Sheet, strPrintName);
        }

        /// <summary>
        /// Description : 예약검사증 출력
        /// Author : 박병규
        /// Create Date : 2017.07.25
        /// <param name="ArgPtno">등록번호</param>
        /// <param name="ArgGwa">진료과목명</param>
        /// <param name="ArgName">환자명</param>
        /// <param name="ArgDr">의사코드</param>
        /// <param name="ArgBi">보험종류</param>
        /// <param name="ArgBDate">처방일자</param>
        /// <param name="ArgDept">진료과목코드</param>
        /// <param name="ArgPicture_Sign"></param>
        /// <param name="ArgSpc">특진여부</param>
        /// <param name="ArgJeaPrt"></param>
        /// <param name="ArgAmt6">대체금액</param>
        /// <param name="ArgWRTNO">고유번호</param>
        /// <param name="SPREAD"></param>
        /// <seealso cref="OUMSAD2.bas : Report_Print_ResvExam2"/>
        public void Report_Print_ResvExam(string ArgPtno, string ArgGwa, string ArgName, string ArgDr, string ArgDrName, string ArgBi, string ArgBDate, string ArgDept, PictureBox Pb, string ArgSpc, string ArgJeaPrt, long ArgAmt6, long ArgWRTNO, FpSpread o)
        {
            clsPrint CP = new clsPrint();
            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strPrintName = CP.getPmpaBarCodePrinter(CARD_PRINTER_NAME);

            //프린트이름이 없을시 다른 이름으로 설정(프린트명 : 접수증, 예약증(원무팀 사무실 설정이름))
            if (strPrintName == "")
                strPrintName = CP.getPmpaBarCodePrinter(JUP_PRINTER_NAME);

            if (strPrintName == "")
                strPrintName = CP.getPmpaBarCodePrinter(RES_PRINTER_NAME);

            //접수증 프린터 설정
            if (strPrintName != "")
            {
                o.ActiveSheet.Cells[0, 1].Text = ArgPtno + VB.Asc(VB.Left(ArgDept.Trim(), 1)) + VB.Asc(VB.Right(ArgDept.Trim(), 1));  //바코드
                o.ActiveSheet.Cells[1, 1].Text = ArgGwa + "(" + ArgDrName + ")";                                        //진료과/의사명
                o.ActiveSheet.Cells[2, 1].Text = string.Format("{0:#,##0}", ArgAmt6) ;                                  //예약검사비
                o.ActiveSheet.Cells[3, 1].Text = ArgPtno + "/" + ArgName;                                               //등록번호/성명
                o.ActiveSheet.Cells[4, 1].Text = Convert.ToString(ArgWRTNO);                                            //예약번호
                o.ActiveSheet.Cells[5, 1].Text = VB.Right(clsPublic.GstrSysDate + clsPublic.GstrSysTime, 14) + "/" + clsType.User.JobName ;  //예약수납일/담당자

                string strHeader = "";
                string strFooter = "";
                bool PrePrint = false;

                strHeader = "";
                strFooter = "";

                setMargin = new clsSpread.SpdPrint_Margin(2, 10, 5, 10, 5, 10);
                setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

                CS.setSpdPrint(o, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
                ComFunc.Delay(200);
            }

        }
        

        /// <summary>
        /// Description : 현금영수증
        /// Author : 박병규
        /// Create Date : 2017.09.21
        /// <param name="ArgTitle">타이틀</param>
        /// <param name="ArgCardNo">식별정보</param>
        /// <param name="ArgAppNo">승인번호</param>
        /// <param name="ArgDate">승인일자</param>
        /// <param name="ArgName">성명</param>
        /// <param name="ArgPtno">등록번호</param>
        /// <param name="ArgAmt">현금</param>
        /// <param name="SPREAD"></param>
        public void Report_Print_ReceiptCash(string ArgTitle, string ArgCardNo, string ArgAppNo, string ArgDate, string ArgName, string ArgPtno, long ArgAmt, FpSpread o)
        {
            clsPrint CP = new clsPrint();
            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strPrintName = CP.getPmpaBarCodePrinter(CARD_PRINTER_NAME);

            //접수증 프린터 설정
            if (CP.isPmpaBarCodePrinter(strPrintName) == true)
            {
                o.ActiveSheet.Cells[0, 0].Text = ArgTitle; //타이틀
                o.ActiveSheet.Cells[8, 1].Text = " " + ArgCardNo; //식별정보
                o.ActiveSheet.Cells[9, 1].Text = " " + ArgAppNo; //승인번호
                o.ActiveSheet.Cells[10, 1].Text = " " + string.Format("{0:yyyy-MM-dd HH:MM}", ArgDate); //승인일자
                o.ActiveSheet.Cells[11, 1].Text = " " + ArgName + " (" + ArgPtno + ")"; //성명 및 등록번호
                o.ActiveSheet.Cells[12, 1].Text = " " + string.Format("{0:#,###}", ArgAmt);

                string strHeader = "";
                string strFooter = "";
                bool PrePrint = false;

                strHeader = "";
                strFooter = "";

                setMargin = new clsSpread.SpdPrint_Margin(2, 10, 5, 10, 5, 10);
                setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

                CS.setSpdPrint(o, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
                ComFunc.Delay(200);

            }

        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.02.13
        /// <param name="ArgOpdNo">외래접수번호</param>
        /// <param name="ArgPtno">등록번호</param>
        /// <param name="ArgName">환자명</param>
        /// <param name="ArgGwa">진료과목명</param>
        /// <param name="ArgRdate">예약일자</param>
        /// <param name="ArgDr">진료의사코드</param>
        /// <param name="ArgBi">환자구분코드</param>
        /// <param name="ArgTel">전화예약시간</param>
        /// <param name="ArgBdate">실진료일자</param>
        /// <param name="ArgCard">카드작업구분</param>
        /// <param name="ArgSunap">수납구분</param>
        /// <param name="ArgDept">진료과목코드</param>
        /// <param name="ArgPicture_Sign"></param>
        /// <param name="ArgGubunX">플루예방접종여부</param>
        /// <param name="ArgSpc">특진여부</param>
        /// <param name="ArgGelCode">거래처코드</param>
        /// <param name="ArgMcode">MCODE</param>
        /// <param name="ArgVcode">VCODE</param>
        /// <param name="FpSpread">예약접수증 스프레드</param>
        /// <seealso cref="Report_Print_A4.bas : Report_Print_Jupsu_A4_Size"/>
        /// </summary>
        public void Report_Print_Jupsu_A4(PsmhDb pDbCon, long ArgOpdNo, string ArgPtno, string ArgName, string ArgGwa, string ArgRdate, string ArgDr, string ArgBi, string ArgTel, string ArgBdate, string ArgCard, string ArgSunap, string ArgDept, PictureBox Pb, bool ArgGubunX, string ArgSpc, string ArgGelcode, string ArgMcode, string ArgVcode, FpSpread o)
        {
            FarPoint.Win.Spread.FpSpread ssSpread = null;
            FarPoint.Win.Spread.SheetView ssSpread_Sheet = null;

            ComFunc CF = new ComFunc();
            clsOumsadChk COC = new ComPmpaLibB.clsOumsadChk();
            clsDrugPrint clsDP = new clsDrugPrint();
            PrintDocument pd = new PrintDocument();
            PageSettings ps = new PageSettings();
            PrintController pc = new StandardPrintController();
            clsPrint CP = new clsPrint();
            Card CC = new Card();

            DataTable DtP = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            int nSeqNo = 0;
            int nFlu_Cnt = 0;
            long nX = 0;
            long nY = 0;
            string strYdate = "";
            string strBi = "";
            string strTimeSS = "";
            string strPrintName = "";

            FstrPtno = ArgPtno;
            FstrSname = ArgName;
            FstrDept = ArgDept;
            FstrDr = ArgDr;
            FstrRdate = ArgRdate;
            
            clsDP.SetReciptPrint(ref ssSpread, ref ssSpread_Sheet);
            clsDP.ClearReciptPrint(ssSpread, ssSpread_Sheet);

            nX = 0 + clsPmpaPb.GnPrintXSet;
            nY = 620 + clsPmpaPb.GnPrintYSet;

            ComFunc.ReadSysDate(pDbCon);
            strBi = CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", ArgBi, "");

            //예약조정 대상자 체크
            strYdate = COC.Check_Reserved_Adjust(pDbCon, ArgPtno, ArgBdate, ArgDept);

            #region //수납순번 가져오기
            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(MAX(SEQNO), 0) SEQNO FROM " + ComNum.DB_PMPA + "OPD_SUNAP ";
            SQL += ComNum.VBLF + "  WHERE ACTDATE   = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND PART      = '" + clsType.User.JobPart + "' ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            if (DtP.Rows.Count > 0)
                nSeqNo = Convert.ToInt32(DtP.Rows[0]["SEQNO"].ToString());

            if (nSeqNo == 0)
                nSeqNo = 1;
            else
                nSeqNo += 1;

            DtP.Dispose();
            DtP = null;
            #endregion

            #region //플루예방접종여부
            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(COUNT(PANO), 0) CNT FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE ACTDATE   = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND GbFlu_Vac = 'Y' ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            if (DtP.Rows.Count > 0)
                nFlu_Cnt = Convert.ToInt32(DtP.Rows[0]["CNT"].ToString());

            DtP.Dispose();
            DtP = null;
            #endregion

            #region //수납시간 초
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS') Sdate FROM DUAL ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            if (DtP.Rows.Count > 0)
                strTimeSS = VB.Right(DtP.Rows[0]["Sdate"].ToString(), 2);

            DtP.Dispose();
            DtP = null;
            #endregion

            #region //INSERT/UPDATE
            if (DialogResult.No == ComFunc.MsgBoxQ("영수증을 출력하겠습니까?", "선택", MessageBoxDefaultButton.Button1))
            {
                if (clsPmpaPb.GstrJeaSunap == "YES")
                {
                    //조별로 수납시 순차적으로 INSERT 함. 마감을 점검 하기위한 문
                    //부가세 추가(Amt7)
                    //진찰료는 부가세 대상에서 제외함

                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP ";
                    SQL += ComNum.VBLF + "        (ACTDATE, OPDNO, PANO, AMT, ";
                    SQL += ComNum.VBLF + "         SEQNO, STIME, BIGO, ";
                    SQL += ComNum.VBLF + "         REMARK, AMT1, AMT2, ";
                    SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
                    SQL += ComNum.VBLF + "         SEQNO2, DEPTCODE, BI, ";
                    SQL += ComNum.VBLF + "         CARDGB, CardAmt, GbSPC, ";
                    SQL += ComNum.VBLF + "         GelCode, MCode, VCode, ";
                    SQL += ComNum.VBLF + "         BDan, CDan, EntDate, ";
                    SQL += ComNum.VBLF + "         BDate, PtAmt, PART, Part2) ";
                    SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "          " + ArgOpdNo + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgPtno + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT7 + ", "; //진찰료 본인부담액
                    SQL += ComNum.VBLF + "          " + nSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                    SQL += ComNum.VBLF + "         '출력안함', ";
                    SQL += ComNum.VBLF + "         '" + ArgSunap + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT3 + ", "; //진찰료 총액
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT4 + ", "; //진찰료 조합부담
                    SQL += ComNum.VBLF + "          " + (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) + ", "; 
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT5 + ", "; //진찰료 감액
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT6 + ", "; //진찰료 미수
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GDrSeqNo + ", ";  //의사별 접수번호
                    SQL += ComNum.VBLF + "         '" + ArgDept.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgBi.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.RSD.Gubun + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.RSD.TotAmt + ", ";
                    SQL += ComNum.VBLF + "         '0', ";
                    SQL += ComNum.VBLF + "         '" + ArgGelcode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgMcode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgVcode + "', ";
                    SQL += ComNum.VBLF + "         0, ";
                    SQL += ComNum.VBLF + "         0, ";
                    SQL += ComNum.VBLF + "         SYSDATE,";
                    SQL += ComNum.VBLF + "         TO_DATE('" + ArgBdate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "         0 , ";
                    
                    if (ArgSunap == "접수취소" && clsPmpaType.TOM.Part != clsType.User.JobPart)
                    {
                        SQL += ComNum.VBLF + "     '" + clsType.User.JobPart + "', ";
                        SQL += ComNum.VBLF + "     '" + clsPmpaType.TOM.Part + "' ) ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "     '" + clsType.User.JobPart + "', ";
                        SQL += ComNum.VBLF + "     '') ";
                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("OPD_SUNAP INSERT Error !!");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        return;
                    }
                }

                clsPmpaPb.GnPatAmt = 0; //환자에게 돈을 받는 금액
                clsPmpaPb.GnJanAmt = 0; //잔액

                //예방접종신플접수증
                if (ArgGubunX == true)
                {
                    //pd.PrinterSettings.PrinterName = PrinterSettings.InstalledPrinters[clsPmpaPb.GnPrtIpdNo3].ToString().Trim(); //신용카드
                    strPrintName = CP.getPmpaBarCodePrinter(CARD_PRINTER_NAME); //Default :신용카드

                    //프린트이름이 없을시 다른 이름으로 설정(프린트명 : 접수증, 예약증)
                    if (strPrintName == "")
                        strPrintName = CP.getPmpaBarCodePrinter(JUP_PRINTER_NAME);

                    if (strPrintName == "")
                        strPrintName = CP.getPmpaBarCodePrinter(RES_PRINTER_NAME);
                }

                //예약접수증
                if (ArgRdate.Trim() != "" && ArgSunap != "예약취소")
                {
                    //2018.06.18 박병규 : 진료의사코드 변경
                    //ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, clsPmpaType.TOM.DrCode, ArgRdate, o);
                    ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, ArgDr, ArgRdate,"", o);
                }

                return;
            }
            else
            {
                if (clsPmpaPb.GstrJeaSunap == "YES")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP ";
                    SQL += ComNum.VBLF + "        (ACTDATE, OPDNO, PANO, AMT, ";
                    SQL += ComNum.VBLF + "         SEQNO, STIME, BIGO, ";
                    SQL += ComNum.VBLF + "         REMARK, AMT1, AMT2, ";
                    SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
                    SQL += ComNum.VBLF + "         SEQNO2, DEPTCODE, BI, ";
                    SQL += ComNum.VBLF + "         CARDGB, CardAmt, GbSPC, ";
                    SQL += ComNum.VBLF + "         GelCode,MCode,VCode, ";
                    SQL += ComNum.VBLF + "         BDan,CDan,EntDate, ";
                    SQL += ComNum.VBLF + "         BDate,PtAmt,PART,PART2) ";
                    SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "          " + ArgOpdNo + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgPtno + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT7 + ", ";
                    SQL += ComNum.VBLF + "          " + nSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                    SQL += ComNum.VBLF + "         ' ', ";
                    SQL += ComNum.VBLF + "         '" + ArgSunap + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT3 + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT4 + ", ";
                    SQL += ComNum.VBLF + "          " + (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT5 + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT6 + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GDrSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgDept.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgBi.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.RSD.Gubun + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.RSD.TotAmt + ", ";
                    SQL += ComNum.VBLF + "         '0', ";
                    SQL += ComNum.VBLF + "         '" + ArgGelcode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgMcode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgVcode + "', ";
                    SQL += ComNum.VBLF + "         0, ";
                    SQL += ComNum.VBLF + "         0, ";
                    SQL += ComNum.VBLF + "         SYSDATE,";
                    SQL += ComNum.VBLF + "         TO_DATE('" + ArgBdate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "         0, ";

                    if (ArgSunap == "접수취소" && clsPmpaType.TOM.Part != clsType.User.JobPart)
                    {
                        SQL += ComNum.VBLF + "     '" + clsType.User.JobPart + "', ";
                        SQL += ComNum.VBLF + "     '" + clsPmpaType.TOM.Part + "' ) ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "     '" + clsType.User.JobPart + "', ";
                        SQL += ComNum.VBLF + "     '') ";
                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("OPD_SUNAP INSERT Error !!");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        return;
                    }

                    clsPmpaPb.GnPatAmt = 0; //환자에게 돈을 받는 금액
                    clsPmpaPb.GnJanAmt = 0; //잔액

                    //예방접종신플접수증
                    if (ArgGubunX == true)
                    {
                        //pd.PrinterSettings.PrinterName = PrinterSettings.InstalledPrinters[clsPmpaPb.GnPrtIpdNo3].ToString().Trim(); //신용카드
                        strPrintName = CP.getPmpaBarCodePrinter(CARD_PRINTER_NAME); //Default :신용카드

                        //프린트이름이 없을시 다른 이름으로 설정(프린트명 : 접수증, 예약증)
                        if (strPrintName == "")
                            strPrintName = CP.getPmpaBarCodePrinter(JUP_PRINTER_NAME);

                        if (strPrintName == "")
                            strPrintName = CP.getPmpaBarCodePrinter(RES_PRINTER_NAME);
                    }
                }
            }
            #endregion

            //예약접수증
            if (ArgRdate.Trim() != "" && ArgSunap != "예약취소")
            {
                //2018.06.18 박병규 : 진료의사코드 변경
                //ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, clsPmpaType.TOM.DrCode, ArgRdate, o);
                ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, ArgDr, ArgRdate,"", o);
            }

            //영수증출력
            CP.getPrinter_Chk(RECEIPT_PRINTER_NAME);
            strPrintName = CP.getPmpaBarCodePrinter(RECEIPT_PRINTER_NAME);

            if (strPrintName.Trim() == "") { return; }

            if (ArgDr == "1107" || ArgDr == "1125")
                ArgGwa = "류마티스내과";

            ssSpread_Sheet.Cells[0, 4].Text = "외래";

            if (ArgTel == "OK")
            {
                if (ArgGwa == "PD")
                    ssSpread_Sheet.Cells[0, 4].Text = "외래" + "(전화접수)";
                else
                    ssSpread_Sheet.Cells[0, 4].Text = "외래" + "(전화예약)";
            }

            ssSpread_Sheet.Cells[0, 12].Text = ArgPtno + VB.Asc(VB.Left(ArgDept, 1)) + VB.Asc(VB.Right(ArgDept, 1)); //바코드

            if (clsPmpaPb.GstrErJobFlag == "OK")
            {
                if (clsPmpaPb.GnNight1 == "2")
                    ssSpread_Sheet.Cells[3, 13].Text = "√";
                else if (clsPmpaPb.GnNight1 == "1")
                    ssSpread_Sheet.Cells[3, 13].Text = VB.Space(11) + "√";
            }

            if (clsPmpaPb.GstrCopyPrint == "OK")
            {
                //2018.06.20 박병규 : 출력위치변경
                //ssSpread_Sheet.Cells[22, 12].Text = "사 본";
                ssSpread_Sheet.Cells[0, 4].Text += " [사본]";
            }

            //if (strYdate != "")  //원무 이채현 책임 후불제 관련 내용과 관계없어 삭제요청 2019.04.08
            //{
            //    ssSpread_Sheet.Cells[23, 12].ColumnSpan = 4;
            //    ssSpread_Sheet.Cells[23, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
            //    ssSpread_Sheet.Cells[24, 12].ColumnSpan = 4;
            //    ssSpread_Sheet.Cells[24, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
            //    ssSpread_Sheet.Cells[23, 12].Text = "금일 예약비는 " + strYdate + " 에";
            //    ssSpread_Sheet.Cells[24, 12].Text = "이미 대체되었습니다.";
            //}

            //공급받는자보관용
            ssSpread_Sheet.Cells[3, 0].Text = ArgPtno;
            ssSpread_Sheet.Cells[3, 4].Text = ArgName;
            ssSpread_Sheet.Cells[3, 8].Text = clsPublic.GstrSysDate;
            ssSpread_Sheet.Cells[5, 0].Text = ArgGwa;
            ssSpread_Sheet.Cells[5, 11].Text = CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", ArgBi);
            ssSpread_Sheet.Cells[5, 12].Text = clsType.User.JobName + "(" + clsType.User.IdNumber + ") " + clsPmpaPb.GDrSeqNo;

            if (ArgDept != "ER")
            {
                ssSpread_Sheet.Cells[26, 5].ColumnSpan = 6;
                ssSpread_Sheet.Cells[26, 5].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[27, 5].ColumnSpan = 6;
                ssSpread_Sheet.Cells[27, 5].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[28, 5].ColumnSpan = 6;
                ssSpread_Sheet.Cells[28, 5].HorizontalAlignment = CellHorizontalAlignment.Left;

                ssSpread_Sheet.Cells[26, 5].Text = " ★★ 진료는 예약자 우선이며, ";
                ssSpread_Sheet.Cells[27, 5].Text = " 진료과에 환자 도착순으로 진료가 실시됩니다. ";
                ssSpread_Sheet.Cells[28, 5].Text = " 도착후 환자의 성함을 알려주시기 바랍니다. ★★";

                if (clsPmpaType.TOM.Chojae == "1")
                {
                    ssSpread_Sheet.Cells[29, 5].ColumnSpan = 6;
                    ssSpread_Sheet.Cells[29, 5].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[30, 5].ColumnSpan = 6;
                    ssSpread_Sheet.Cells[30, 5].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[29, 5].Text = " ★★ 방문해 주셔서 감사합니다. ★★";
                    ssSpread_Sheet.Cells[30, 5].Text = " ★★ 최상의 서비스제공을 위해 노력하겠습니다. ★★";
                }
            }

            if (ArgSunap != "예약비")
            {
                //진찰료 급여본인부담금
                if (clsPmpaPb.gnJinAMT5 != 0)
                    ssSpread_Sheet.Cells[9, 3].Text = string.Format("{0:#,###}", ((clsPmpaPb.gnJinAMT7 + clsPmpaPb.gnJinAMT5) - clsPmpaPb.gnJinAMT2));
                else
                    ssSpread_Sheet.Cells[9, 3].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaPb.gnJinAMT2));

                //진찰료 급여공단부담금
                ssSpread_Sheet.Cells[9, 5].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT4);
                //진찰료 선택진료료
                ssSpread_Sheet.Cells[9, 8].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT2); 

            }
            else if (ArgSunap == "예약비")
            {
                //진찰료 급여본인부담금
                ssSpread_Sheet.Cells[29, 3].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaPb.gnJinAMT2));
                //진찰료 급여공단부담금
                ssSpread_Sheet.Cells[29, 5].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT4);
                //진찰료 선택진료료
                ssSpread_Sheet.Cells[29, 8].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT2); 
            }

            //진찰료 총액에 선택진료비 포함되어었음
            clsPmpaPb.gnJinAMT3 = clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT2;

            //합계 급여본인부담금
            if (clsPmpaPb.gnJinAMT5 != 0)
                ssSpread_Sheet.Cells[35, 3].Text = string.Format("{0:#,###}", ((clsPmpaPb.gnJinAMT7 + clsPmpaPb.gnJinAMT5) - clsPmpaPb.gnJinAMT2)); 
            else
                ssSpread_Sheet.Cells[35, 3].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaPb.gnJinAMT2));

            //합계 급여공단부담금
            ssSpread_Sheet.Cells[35, 5].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT4);
            //합계 선택진료료
            ssSpread_Sheet.Cells[35, 8].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT2);

            //<금액산정내용>***************************************************************************************************************
            //진료비총액
            ssSpread_Sheet.Cells[7, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT1 + clsPmpaPb.gnJinAMT2)); 

            //환자부담총액
            if (clsPmpaPb.gnJinAMT5 != 0)
                ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 + clsPmpaPb.gnJinAMT5));
            else
                ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);

            //납부할금액
            ssSpread_Sheet.Cells[13, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);

            //납부한금액
            if (clsPmpaType.RSD.Gubun == "1")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {
                    ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//카드
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - (clsPmpaType.RSD.TotAmt * -1)));//현금
                }
                else
                {
                    ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//카드
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaType.RSD.TotAmt));//현금
                }
            }
            else if (clsPmpaType.RSD.Gubun == "2")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {
                    ssSpread_Sheet.Cells[16, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//현금영수증
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - (clsPmpaType.RSD.TotAmt * -1)));//현금

                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[28, 12].Text = "현금승인취소";
                        ssSpread_Sheet.Cells[29, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[32, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
                else
                {
                    ssSpread_Sheet.Cells[16, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//현금영수증
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaType.RSD.TotAmt));//현금

                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[28, 12].Text = "현금승인";
                        ssSpread_Sheet.Cells[29, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[32, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
            }
            else
            {
                ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);//현금
            }

            ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);//합계
            //<금액산정내용>***************************************************************************************************************

            //감액
            if (clsPmpaPb.gnJinAMT5 > 0)
            {
                ssSpread_Sheet.Cells[11, 11].ColumnSpan = 2;
                ssSpread_Sheet.Cells[11, 11].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssSpread_Sheet.Cells[11, 11].Text = "감액";
                ssSpread_Sheet.Cells[11, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT5);//감액

                if (clsPmpaType.TOM.GbGameK != "00" && clsPmpaType.TOM.GbGameK != "")
                {
                    if (clsPmpaType.TOM.GbGameK == "11")
                    {
                        ssSpread_Sheet.Cells[12, 11].ColumnSpan = 4;
                        ssSpread_Sheet.Cells[12, 11].HorizontalAlignment = CellHorizontalAlignment.Left;

                        ssSpread_Sheet.Cells[12, 11].Text = "감액사유 : 직원본인할인";
                    }
                    else if (clsPmpaType.TOM.GbGameK == "13" || clsPmpaType.TOM.GbGameK == "14" || clsPmpaType.TOM.GbGameK == "22" || clsPmpaType.TOM.GbGameK == "23" || clsPmpaType.TOM.GbGameK == "24" || clsPmpaType.TOM.GbGameK == "27" || clsPmpaType.TOM.GbGameK == "32" || clsPmpaType.TOM.GbGameK == "33" || clsPmpaType.TOM.GbGameK == "34")
                    {
                        ssSpread_Sheet.Cells[12, 11].ColumnSpan = 4;
                        ssSpread_Sheet.Cells[12, 11].HorizontalAlignment = CellHorizontalAlignment.Left;

                        ssSpread_Sheet.Cells[12, 11].Text = "감액사유 : 직원가족할인";
                    }
                }
            }

            //미수
            if (clsPmpaPb.gnJinAMT6 > 0)
            {
                ssSpread_Sheet.Cells[12, 11].ColumnSpan = 2;
                ssSpread_Sheet.Cells[12, 11].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssSpread_Sheet.Cells[12, 11].Text = "미수금";
                ssSpread_Sheet.Cells[12, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT6);//미수
            }

            ssSpread_Sheet.Cells[40, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime;

            //카드영수증 통합
            if (clsPmpaType.RSD.CardSeqNo > 0 && ArgSunap != "대리접수")
            {
                Card.GstrCardApprov_Info = CC.Card_Sign_Info_Set(pDbCon, clsPmpaType.RSD.CardSeqNo, ArgPtno);

                if (Card.GstrCardApprov_Info != "")
                {
                    ssSpread_Sheet.Cells[29, 13].ColumnSpan = 3;
                    ssSpread_Sheet.Cells[29, 13].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[29, 13].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 24); //거래구분

                    ssSpread_Sheet.Cells[30, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[30, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[30, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 4) + " " + VB.Pstr(Card.GstrCardApprov_Info, "{}", 2); //카드종류/카드번호
                    ssSpread_Sheet.Cells[31, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[31, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[31, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 20); //승인금액
                    ssSpread_Sheet.Cells[32, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[32, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[32, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 12); //승인번호
                    ssSpread_Sheet.Cells[33, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[33, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[33, 12].Text = "72117503"; //가맹점번호
                    ssSpread_Sheet.Cells[34, 12].ColumnSpan = 2;
                    ssSpread_Sheet.Cells[34, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[34, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 14); //거래일시
                    ssSpread_Sheet.Cells[34, 15].Text = VB.Pstr(VB.Pstr(Card.GstrCardApprov_Info, "{}", 8), "개월", 1); 
                }

                Card.GstrCardApprov_Info = "";
            }

            clsDP.PrintReciptPrint(ssSpread, ssSpread_Sheet, strPrintName);
        }
        public void Report_Print_Jupsu_A4_New(PsmhDb pDbCon, long ArgOpdNo, string ArgPtno, string ArgName, string ArgGwa, string ArgRdate, string ArgDr, string ArgBi, string ArgTel, string ArgBdate, string ArgCard, string ArgSunap, string ArgDept, PictureBox Pb, bool ArgGubunX, string ArgSpc, string ArgGelcode, string ArgMcode, string ArgVcode, FpSpread o)
        {
            FarPoint.Win.Spread.FpSpread ssSpread = null;
            FarPoint.Win.Spread.SheetView ssSpread_Sheet = null;

            ComFunc CF = new ComFunc();
            clsOumsadChk COC = new ComPmpaLibB.clsOumsadChk();
            clsDrugPrint clsDP = new clsDrugPrint();
            PrintDocument pd = new PrintDocument();
            PageSettings ps = new PageSettings();
            PrintController pc = new StandardPrintController();
            clsPrint CP = new clsPrint();
            Card CC = new Card();

            DataTable DtP = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            int nSeqNo = 0;
            int nFlu_Cnt = 0;
            long nX = 0;
            long nY = 0;
            string strYdate = "";
            string strBi = "";
            string strTimeSS = "";
            string strPrintName = "";

            FstrPtno = ArgPtno;
            FstrSname = ArgName;
            FstrDept = ArgDept;
            FstrDr = ArgDr;
            FstrRdate = ArgRdate;

            clsDP.SetReciptPrint_New(ref ssSpread, ref ssSpread_Sheet);
            clsDP.ClearReciptPrint_New(ssSpread, ssSpread_Sheet);

            nX = 0 + clsPmpaPb.GnPrintXSet;
            nY = 620 + clsPmpaPb.GnPrintYSet;

            ComFunc.ReadSysDate(pDbCon);
            strBi = CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", ArgBi, "");

            //예약조정 대상자 체크
            strYdate = COC.Check_Reserved_Adjust(pDbCon, ArgPtno, ArgBdate, ArgDept);

            #region //수납순번 가져오기
            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(MAX(SEQNO), 0) SEQNO FROM " + ComNum.DB_PMPA + "OPD_SUNAP ";
            SQL += ComNum.VBLF + "  WHERE ACTDATE   = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND PART      = '" + clsType.User.JobPart + "' ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            if (DtP.Rows.Count > 0)
                nSeqNo = Convert.ToInt32(DtP.Rows[0]["SEQNO"].ToString());

            if (nSeqNo == 0)
                nSeqNo = 1;
            else
                nSeqNo += 1;

            DtP.Dispose();
            DtP = null;
            #endregion

            #region //플루예방접종여부
            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(COUNT(PANO), 0) CNT FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE ACTDATE   = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND GbFlu_Vac = 'Y' ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            if (DtP.Rows.Count > 0)
                nFlu_Cnt = Convert.ToInt32(DtP.Rows[0]["CNT"].ToString());

            DtP.Dispose();
            DtP = null;
            #endregion

            #region //수납시간 초
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS') Sdate FROM DUAL ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            if (DtP.Rows.Count > 0)
                strTimeSS = VB.Right(DtP.Rows[0]["Sdate"].ToString(), 2);

            DtP.Dispose();
            DtP = null;
            #endregion

            #region //INSERT/UPDATE
            if (DialogResult.No == ComFunc.MsgBoxQ("영수증을 출력하겠습니까?", "선택", MessageBoxDefaultButton.Button1))
            {
                if (clsPmpaPb.GstrJeaSunap == "YES")
                {
                    //조별로 수납시 순차적으로 INSERT 함. 마감을 점검 하기위한 문
                    //부가세 추가(Amt7)
                    //진찰료는 부가세 대상에서 제외함

                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP ";
                    SQL += ComNum.VBLF + "        (ACTDATE, OPDNO, PANO, AMT, ";
                    SQL += ComNum.VBLF + "         SEQNO, STIME, BIGO, ";
                    SQL += ComNum.VBLF + "         REMARK, AMT1, AMT2, ";
                    SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
                    SQL += ComNum.VBLF + "         SEQNO2, DEPTCODE, BI, ";
                    SQL += ComNum.VBLF + "         CARDGB, CardAmt, GbSPC, ";
                    SQL += ComNum.VBLF + "         GelCode, MCode, VCode, ";
                    SQL += ComNum.VBLF + "         BDan, CDan, EntDate, ";
                    SQL += ComNum.VBLF + "         BDate, PtAmt, PART, Part2) ";
                    SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "          " + ArgOpdNo + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgPtno + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT7 + ", "; //진찰료 본인부담액
                    SQL += ComNum.VBLF + "          " + nSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                    SQL += ComNum.VBLF + "         '출력안함', ";
                    SQL += ComNum.VBLF + "         '" + ArgSunap + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT3 + ", "; //진찰료 총액
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT4 + ", "; //진찰료 조합부담
                    SQL += ComNum.VBLF + "          " + (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT5 + ", "; //진찰료 감액
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT6 + ", "; //진찰료 미수
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GDrSeqNo + ", ";  //의사별 접수번호
                    SQL += ComNum.VBLF + "         '" + ArgDept.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgBi.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.RSD.Gubun + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.RSD.TotAmt + ", ";
                    SQL += ComNum.VBLF + "         '0', ";
                    SQL += ComNum.VBLF + "         '" + ArgGelcode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgMcode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgVcode + "', ";
                    SQL += ComNum.VBLF + "         0, ";
                    SQL += ComNum.VBLF + "         0, ";
                    SQL += ComNum.VBLF + "         SYSDATE,";
                    SQL += ComNum.VBLF + "         TO_DATE('" + ArgBdate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "         0 , ";

                    if (ArgSunap == "접수취소" && clsPmpaType.TOM.Part != clsType.User.JobPart)
                    {
                        SQL += ComNum.VBLF + "     '" + clsType.User.JobPart + "', ";
                        SQL += ComNum.VBLF + "     '" + clsPmpaType.TOM.Part + "' ) ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "     '" + clsType.User.JobPart + "', ";
                        SQL += ComNum.VBLF + "     '') ";
                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("OPD_SUNAP INSERT Error !!");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        return;
                    }
                }

                clsPmpaPb.GnPatAmt = 0; //환자에게 돈을 받는 금액
                clsPmpaPb.GnJanAmt = 0; //잔액

                //예방접종신플접수증
                if (ArgGubunX == true)
                {
                    //pd.PrinterSettings.PrinterName = PrinterSettings.InstalledPrinters[clsPmpaPb.GnPrtIpdNo3].ToString().Trim(); //신용카드
                    strPrintName = CP.getPmpaBarCodePrinter(CARD_PRINTER_NAME); //Default :신용카드

                    //프린트이름이 없을시 다른 이름으로 설정(프린트명 : 접수증, 예약증)
                    if (strPrintName == "")
                        strPrintName = CP.getPmpaBarCodePrinter(JUP_PRINTER_NAME);

                    if (strPrintName == "")
                        strPrintName = CP.getPmpaBarCodePrinter(RES_PRINTER_NAME);
                }

                //예약접수증
                if (ArgRdate.Trim() != "" && ArgSunap != "예약취소")
                {
                    //2018.06.18 박병규 : 진료의사코드 변경
                    //ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, clsPmpaType.TOM.DrCode, ArgRdate, o);
                    ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, ArgDr, ArgRdate,"", o);
                }

                return;
            }
            else
            {
                if (clsPmpaPb.GstrJeaSunap == "YES")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP ";
                    SQL += ComNum.VBLF + "        (ACTDATE, OPDNO, PANO, AMT, ";
                    SQL += ComNum.VBLF + "         SEQNO, STIME, BIGO, ";
                    SQL += ComNum.VBLF + "         REMARK, AMT1, AMT2, ";
                    SQL += ComNum.VBLF + "         AMT3, AMT4, AMT5, ";
                    SQL += ComNum.VBLF + "         SEQNO2, DEPTCODE, BI, ";
                    SQL += ComNum.VBLF + "         CARDGB, CardAmt, GbSPC, ";
                    SQL += ComNum.VBLF + "         GelCode,MCode,VCode, ";
                    SQL += ComNum.VBLF + "         BDan,CDan,EntDate, ";
                    SQL += ComNum.VBLF + "         BDate,PtAmt,PART,PART2) ";
                    SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "          " + ArgOpdNo + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgPtno + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT7 + ", ";
                    SQL += ComNum.VBLF + "          " + nSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                    SQL += ComNum.VBLF + "         ' ', ";
                    SQL += ComNum.VBLF + "         '" + ArgSunap + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT3 + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT4 + ", ";
                    SQL += ComNum.VBLF + "          " + (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT5 + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT6 + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GDrSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgDept.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgBi.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.RSD.Gubun + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.RSD.TotAmt + ", ";
                    SQL += ComNum.VBLF + "         '0', ";
                    SQL += ComNum.VBLF + "         '" + ArgGelcode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgMcode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgVcode + "', ";
                    SQL += ComNum.VBLF + "         0, ";
                    SQL += ComNum.VBLF + "         0, ";
                    SQL += ComNum.VBLF + "         SYSDATE,";
                    SQL += ComNum.VBLF + "         TO_DATE('" + ArgBdate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "         0, ";

                    if (ArgSunap == "접수취소" && clsPmpaType.TOM.Part != clsType.User.JobPart)
                    {
                        SQL += ComNum.VBLF + "     '" + clsType.User.JobPart + "', ";
                        SQL += ComNum.VBLF + "     '" + clsPmpaType.TOM.Part + "' ) ";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "     '" + clsType.User.JobPart + "', ";
                        SQL += ComNum.VBLF + "     '') ";
                    }
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("OPD_SUNAP INSERT Error !!");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        return;
                    }

                    clsPmpaPb.GnPatAmt = 0; //환자에게 돈을 받는 금액
                    clsPmpaPb.GnJanAmt = 0; //잔액

                    //예방접종신플접수증
                    if (ArgGubunX == true)
                    {
                        //pd.PrinterSettings.PrinterName = PrinterSettings.InstalledPrinters[clsPmpaPb.GnPrtIpdNo3].ToString().Trim(); //신용카드
                        strPrintName = CP.getPmpaBarCodePrinter(CARD_PRINTER_NAME); //Default :신용카드

                        //프린트이름이 없을시 다른 이름으로 설정(프린트명 : 접수증, 예약증)
                        if (strPrintName == "")
                            strPrintName = CP.getPmpaBarCodePrinter(JUP_PRINTER_NAME);

                        if (strPrintName == "")
                            strPrintName = CP.getPmpaBarCodePrinter(RES_PRINTER_NAME);
                    }
                }
            }
            #endregion

            //예약접수증
            if (ArgRdate.Trim() != "" && ArgSunap != "예약취소")
            {
                //2018.06.18 박병규 : 진료의사코드 변경
                //ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, clsPmpaType.TOM.DrCode, ArgRdate, o);
                ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, ArgDr, ArgRdate,"", o);
            }

            //영수증출력
            CP.getPrinter_Chk(RECEIPT_PRINTER_NAME);
            strPrintName = CP.getPmpaBarCodePrinter(RECEIPT_PRINTER_NAME);

            if (strPrintName.Trim() == "") { return; }

            if (ArgDr == "1107" || ArgDr == "1125")
                ArgGwa = "류마티스내과";

            ssSpread_Sheet.Cells[0, 4].Text = "외래";

            if (ArgTel == "OK")
            {
                if (ArgGwa == "PD")
                    ssSpread_Sheet.Cells[0, 4].Text = "외래" + "(전화접수)";
                else
                    ssSpread_Sheet.Cells[0, 4].Text = "외래" + "(전화예약)";
            }

            ssSpread_Sheet.Cells[0, 12].Text = ArgPtno + VB.Asc(VB.Left(ArgDept, 1)) + VB.Asc(VB.Right(ArgDept, 1)); //바코드

            if (clsPmpaPb.GstrErJobFlag == "OK")
            {
                if (clsPmpaPb.GnNight1 == "2")
                    ssSpread_Sheet.Cells[3, 13].Text = "√";
                else if (clsPmpaPb.GnNight1 == "1")
                    ssSpread_Sheet.Cells[3, 13].Text = VB.Space(11) + "√";
            }

            if (clsPmpaPb.GstrCopyPrint == "OK")
            {
                //2018.06.20 박병규 : 출력위치변경
                //ssSpread_Sheet.Cells[22, 12].Text = "사 본";
                ssSpread_Sheet.Cells[0, 4].Text += " [사본]";
            }

            //if (strYdate != "")
            //{
            //    ssSpread_Sheet.Cells[25, 12].ColumnSpan = 4;
            //    ssSpread_Sheet.Cells[25, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
            //    ssSpread_Sheet.Cells[26, 12].ColumnSpan = 4;
            //    ssSpread_Sheet.Cells[26, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
            //    ssSpread_Sheet.Cells[25, 12].Text = "금일 예약비는 " + strYdate + " 에";
            //    ssSpread_Sheet.Cells[26, 12].Text = "이미 대체되었습니다.";
            //}

            //공급받는자보관용
            ssSpread_Sheet.Cells[3, 0].Text = ArgPtno;
            ssSpread_Sheet.Cells[3, 4].Text = ArgName;
            ssSpread_Sheet.Cells[3, 8].Text = clsPublic.GstrSysDate;
           // ssSpread_Sheet.Cells[5, 0].Text = ArgGwa;
            ssSpread_Sheet.Cells[5, 0].Text = ArgGwa +  COC.GetClinicDept_Inf(pDbCon, ArgDept)  ;  
            ssSpread_Sheet.Cells[5, 11].Text = CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", ArgBi);
            ssSpread_Sheet.Cells[5, 12].Text = clsType.User.JobName + "(" + clsType.User.IdNumber + ") " + clsPmpaPb.GDrSeqNo;

            if (ArgDept != "ER")
            {
                ssSpread_Sheet.Cells[28, 5].ColumnSpan = 6;
                ssSpread_Sheet.Cells[28, 5].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[29, 5].ColumnSpan = 6;
                ssSpread_Sheet.Cells[29, 5].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[30, 5].ColumnSpan = 6;
                ssSpread_Sheet.Cells[30, 5].HorizontalAlignment = CellHorizontalAlignment.Left;

                ssSpread_Sheet.Cells[28, 5].Text = " ★★ 진료는 예약자 우선이며, ";
                ssSpread_Sheet.Cells[29, 5].Text = " 진료과에 환자 도착순으로 진료가 실시됩니다. ";
                ssSpread_Sheet.Cells[30, 5].Text = " 도착후 환자의 성함을 알려주시기 바랍니다. ★★";

                if (clsPmpaType.TOM.Chojae == "1")
                {
                    ssSpread_Sheet.Cells[31, 5].ColumnSpan = 6;
                    ssSpread_Sheet.Cells[31, 5].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[32, 5].ColumnSpan = 6;
                    ssSpread_Sheet.Cells[32, 5].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[31, 5].Text = " ★★ 방문해 주셔서 감사합니다. ★★";
                    ssSpread_Sheet.Cells[32, 5].Text = " ★★ 최상의 서비스제공을 위해 노력하겠습니다. ★★";
                }
            }

            if (ArgSunap != "예약비")
            {
                //진찰료 급여본인부담금
                if (clsPmpaPb.gnJinAMT5 != 0)
                    ssSpread_Sheet.Cells[9, 3].Text = string.Format("{0:#,###}", ((clsPmpaPb.gnJinAMT7 + clsPmpaPb.gnJinAMT5) - clsPmpaPb.gnJinAMT2));
                else
                    ssSpread_Sheet.Cells[9, 3].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaPb.gnJinAMT2));

                //진찰료 급여공단부담금
                ssSpread_Sheet.Cells[9, 5].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT4);
                //진찰료 선택진료료
                ssSpread_Sheet.Cells[9, 8].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT2);

            }
            else if (ArgSunap == "예약비")
            {
                //진찰료 급여본인부담금
                ssSpread_Sheet.Cells[31, 3].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaPb.gnJinAMT2));
                //진찰료 급여공단부담금
                ssSpread_Sheet.Cells[31, 5].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT4);
                //진찰료 선택진료료
                ssSpread_Sheet.Cells[31, 8].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT2);
            }

            //진찰료 총액에 선택진료비 포함되어었음
            clsPmpaPb.gnJinAMT3 = clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT2;

            //합계 급여본인부담금
            if (clsPmpaPb.gnJinAMT5 != 0)
                ssSpread_Sheet.Cells[37, 3].Text = string.Format("{0:#,###}", ((clsPmpaPb.gnJinAMT7 + clsPmpaPb.gnJinAMT5) - clsPmpaPb.gnJinAMT2));
            else
                ssSpread_Sheet.Cells[37, 3].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaPb.gnJinAMT2));

            //합계 급여공단부담금
            ssSpread_Sheet.Cells[37, 5].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT4);
            //합계 선택진료료
            ssSpread_Sheet.Cells[37, 8].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT2);

            //<금액산정내용>***************************************************************************************************************
            //진료비총액
            ssSpread_Sheet.Cells[7, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT1 + clsPmpaPb.gnJinAMT2));

            //환자부담총액
            if (clsPmpaPb.gnJinAMT5 != 0)
                ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 + clsPmpaPb.gnJinAMT5));
            else
                ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);

            //납부할금액
            ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);

            //납부한금액
            //(2021-07-15)
            if (clsPmpaType.RSD.Gubun == "1")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//카드
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - (clsPmpaType.RSD.TotAmt * -1)));//현금
                }
                else
                {
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//카드
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaType.RSD.TotAmt));//현금
                }
            }
            else if (clsPmpaType.RSD.Gubun == "2")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {
                    ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//현금영수증
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - (clsPmpaType.RSD.TotAmt * -1)));//현금

                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[30, 12].Text = "현금승인취소";
                        ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
                else
                {
                    ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//현금영수증
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaType.RSD.TotAmt));//현금

                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[30, 12].Text = "현금승인";
                        ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
            }
            else
            {
                ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);//현금
            }

            ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);//합계
            //<금액산정내용>***************************************************************************************************************

            //감액
            if (clsPmpaPb.gnJinAMT5 > 0)
            {
                ssSpread_Sheet.Cells[13, 11].ColumnSpan = 2;
                ssSpread_Sheet.Cells[13, 11].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssSpread_Sheet.Cells[13, 11].Text = "감액";
                ssSpread_Sheet.Cells[13, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT5);//감액

                if (clsPmpaType.TOM.GbGameK != "00" && clsPmpaType.TOM.GbGameK != "")
                {
                    if (clsPmpaType.TOM.GbGameK == "11")
                    {
                        ssSpread_Sheet.Cells[14, 11].ColumnSpan = 4;
                        ssSpread_Sheet.Cells[14, 11].HorizontalAlignment = CellHorizontalAlignment.Left;

                        ssSpread_Sheet.Cells[14, 11].Text = "감액사유 : 직원본인할인";
                    }
                    else if (clsPmpaType.TOM.GbGameK == "13" || clsPmpaType.TOM.GbGameK == "14" || clsPmpaType.TOM.GbGameK == "22" || clsPmpaType.TOM.GbGameK == "23" || clsPmpaType.TOM.GbGameK == "24" || clsPmpaType.TOM.GbGameK == "27" || clsPmpaType.TOM.GbGameK == "32" || clsPmpaType.TOM.GbGameK == "33" || clsPmpaType.TOM.GbGameK == "34")
                    {
                        ssSpread_Sheet.Cells[14, 11].ColumnSpan = 4;
                        ssSpread_Sheet.Cells[14, 11].HorizontalAlignment = CellHorizontalAlignment.Left;

                        ssSpread_Sheet.Cells[14, 11].Text = "감액사유 : 직원가족할인";
                    }
                }
            }

            //미수
            if (clsPmpaPb.gnJinAMT6 > 0)
            {
                ssSpread_Sheet.Cells[14, 11].ColumnSpan = 2;
                ssSpread_Sheet.Cells[14, 11].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssSpread_Sheet.Cells[14, 11].Text = "미수금";
                ssSpread_Sheet.Cells[14, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT6);//미수
            }

            ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime;

            //카드영수증 통합
            if (clsPmpaType.RSD.CardSeqNo > 0 && ArgSunap != "대리접수")
            {
                Card.GstrCardApprov_Info = CC.Card_Sign_Info_Set(pDbCon, clsPmpaType.RSD.CardSeqNo, ArgPtno);

                if (Card.GstrCardApprov_Info != "")
                {
                    ssSpread_Sheet.Cells[31, 13].ColumnSpan = 3;
                    ssSpread_Sheet.Cells[31, 13].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[31, 13].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 24); //거래구분

                    ssSpread_Sheet.Cells[32, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[32, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[32, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 4) + " " + VB.Pstr(Card.GstrCardApprov_Info, "{}", 2); //카드종류/카드번호
                    ssSpread_Sheet.Cells[33, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[33, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[33, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 20); //승인금액
                    ssSpread_Sheet.Cells[34, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[34, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[34, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 12); //승인번호
                    ssSpread_Sheet.Cells[35, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[35, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[35, 12].Text = "72117503"; //가맹점번호
                    ssSpread_Sheet.Cells[36, 12].ColumnSpan = 2;
                    ssSpread_Sheet.Cells[36, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[36, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 14); //거래일시
                    ssSpread_Sheet.Cells[36, 15].Text = VB.Pstr(VB.Pstr(Card.GstrCardApprov_Info, "{}", 8), "개월", 1);
                }

                Card.GstrCardApprov_Info = "";
            }
            strPrintName = VB.Replace(strPrintName, "(리디렉션)", "");
            strPrintName = VB.Replace(strPrintName, "(리디렉션 1)", "");
            strPrintName = VB.Replace(strPrintName, "(리디렉션 2)", "");
            clsDP.PrintReciptPrint_New(ssSpread, ssSpread_Sheet, strPrintName);
        }

        /// <summary>
        /// Description : 예약접수증 출력
        /// Author : 박병규
        /// Create Date : 2018.03.14
        /// <param name="pDbCon"></param>
        /// <param name="ArgRe">사본여부(1.사본)</param>
        /// <param name="ArgPtno">등록번호</param>
        /// <param name="ArgName">환자명</param>
        /// <param name="ArgDept">진료과목코드</param>
        /// <param name="ArgDeptName">진료과목명</param>
        /// <param name="ArgDr">예약의사코드</param>
        /// <param name="ArgRDate">예약일자</param>
        /// <param name="ArgPicture_Sign"></param>
        /// <param name="SPREAD"></param>
        public void ResJupsu_Print(PsmhDb pDbCon, string ArgRe, string ArgPtno, string ArgName, string ArgDept, string ArgDeptName, string ArgDr, string ArgRDate, string ArgActDate, FpSpread o)
        {
            ComFunc CF = new ComBase.ComFunc();
            clsPrint CP = new clsPrint();
            clsSpread CS = new clsSpread();
            clsPmpaFunc CPF = new ComPmpaLibB.clsPmpaFunc();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strPrintName = "";

            strPrintName = CP.getPmpaBarCodePrinter(CARD_PRINTER_NAME); //Default :신용카드(접수창구용 설정이름)

            //프린트이름이 없을시 다른 이름으로 설정(프린트명 : 접수증, 예약증(원무팀 사무실 설정이름))
            if (strPrintName == "")
                strPrintName = CP.getPmpaBarCodePrinter(JUP_PRINTER_NAME);

            if (strPrintName == "")
                strPrintName = CP.getPmpaBarCodePrinter(RES_PRINTER_NAME);


            //접수증 프린터 설정
            if (strPrintName != "")
            {
                if (ArgRe == "1")
                    o.ActiveSheet.Cells[0, 0].Text = "예약증 (사본)";
                else
                    o.ActiveSheet.Cells[0, 0].Text = "예약증 (후불)";

                o.ActiveSheet.Cells[0, 1].Text = ArgPtno + VB.Asc(VB.Left(ArgDept, 1)) + VB.Asc(VB.Right(ArgDept, 1)); //바코드
                o.ActiveSheet.Cells[1, 1].Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Bold);
                o.ActiveSheet.Cells[1, 1].Text = ArgDeptName + " / " + CF.READ_DrName(pDbCon, ArgDr); //진료과/의사명
                o.ActiveSheet.Cells[2, 1].Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Bold);
                o.ActiveSheet.Cells[2, 1].Text = ArgRDate; //예약일자
                o.ActiveSheet.Cells[3, 1].Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Bold);
                o.ActiveSheet.Cells[3, 1].Text = ArgPtno + " / " + ArgName; //등록번호/성명

                o.ActiveSheet.Cells[4, 1].ColumnSpan = 2;
                o.ActiveSheet.Cells[4, 1].HorizontalAlignment = CellHorizontalAlignment.Left;
                if (ArgActDate != "")
                {
                    o.ActiveSheet.Cells[4, 1].Text = ArgActDate + " " + "(" + clsType.User.JobName + ")"; //수납일자/수납자
                }
                else
                {
                    o.ActiveSheet.Cells[4, 1].Text = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "(" + clsType.User.JobName + ")"; //수납일자/수납자
                }
                

                o.ActiveSheet.Cells[5, 1].ColumnSpan = 2;
                o.ActiveSheet.Cells[5, 1].HorizontalAlignment = CellHorizontalAlignment.Left;
                o.ActiveSheet.Cells[5, 1].Text = CPF.Get_Dept_TelNo(pDbCon, ArgDept, ArgDr).Trim() + "(진료과)";

                string strHeader = "";
                string strFooter = "";
                bool PrePrint = false;

                strHeader = "";
                strFooter = "";

                setMargin = new clsSpread.SpdPrint_Margin(2, 10, 5, 10, 5, 10);
                setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);
                
                CS.setSpdPrint(o, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);

                ComFunc.Delay(200);
            }
        }

        /// <summary>
        /// 예약증 프린트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResPrint_PrintPage(object sender, PrintPageEventArgs e)
        {

        }

        /// <summary>
        /// Description : 수납영수증
        /// Author : 박병규
        /// Create Date : 2017.07.25
        /// <param name="pDbCon"></param>
        /// <param name="ArgOpdNo"></param>
        /// <param name="ArgPtno">등록번호</param>
        /// <param name="ArgGwa">진료과목명</param>
        /// <param name="ArgName">환자명</param>
        /// <param name="ArgRetn">취소여부</param>
        /// <param name="ArgSeq">영수증번호</param>
        /// <param name="ArgRdate">예약일자</param>
        /// <param name="ArgDr">예약의사</param>
        /// <param name="ArgBi">환자구분</param>
        /// <param name="ArgBDate">처방일자</param>
        /// <param name="ArgCardBun">카드구분</param>
        /// <param name="ArgSunap"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgPicture_Sign"></param>
        /// <param name="ArgIpdDtl">입원정밀</param>
        /// <param name="ArgSpc">특진여부</param>
        /// <param name="ArgGelCode">거래처코드</param>
        /// <param name="ArgMcode">MCODE</param>
        /// <param name="ArgVcode">VCODE</param>
        /// <param name="ArgJin"></param>
        /// <param name="ArgJinDtl"></param>
        /// <param name="ArgRe">재발행여부 : R.재발행</param>
        /// <param name="ArgReDate">재발행일자</param>
        /// <seealso cref="Report_Print_A4.bas : Report_Print_Sunap_A4_Size"/>
        /// </summary>
        public void Report_Print_Sunap_A4(PsmhDb pDbCon, long ArgOpdNo, string ArgPtno, string ArgGwa, string ArgName, string ArgRetn, int ArgSeq, string ArgRdate,  string ArgDr, string ArgBi, string ArgBDate, string ArgCardBun, string ArgSunap, string ArgDept, PictureBox Pb, string ArgIpdDtl, string ArgSpc, string ArgGelCode, string ArgMcode, string ArgVcode, string ArgJin, string ArgJinDtl, FpSpread o, string ArgRe = "", string ArgReDate = "")
        {
            FarPoint.Win.Spread.FpSpread ssSpread = null;
            FarPoint.Win.Spread.SheetView ssSpread_Sheet = null;

            ComFunc CF = new ComFunc();
            clsPmpaFunc CPF = new ComPmpaLibB.clsPmpaFunc();
            clsOumsadChk COC = new ComPmpaLibB.clsOumsadChk();
            clsDrugPrint clsDP = new clsDrugPrint();
            PrintDocument pd = new PrintDocument();
            PageSettings ps = new PageSettings();
            PrintController pc = new StandardPrintController();
            clsOumsad CPO = new clsOumsad();
            clsBasAcct CBA = new ComPmpaLibB.clsBasAcct();

            DataTable DtP = new DataTable();
            DataTable DtSub = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            string strPrintName = "";

            int nSelf = 0;
            int nSeqNo = 0;
            string strYdate = "";
            string strTimeSS = "";
            long[ , ] nPAmt = new long[29, 6];
            double nTaxAmt = 0;
            long nTaxDanAmt = 0;
            long nAmt8 = 0;
            string strMsgChk = "";
            int nCntEtc1 = 0;//계정별기타
            int nCntEtc2 = 0;//정산별기타
            int j = 0;

            Argument_Table RPA = new Argument_Table();
            Report_Add_Amt_Table[] RPG = new ComPmpaLibB.clsPmpaPrint.Report_Add_Amt_Table[50];
            Slip_ReportPrint_Table[] RP = new ComPmpaLibB.clsPmpaPrint.Slip_ReportPrint_Table[999];

            clsDP.SetReciptPrint(ref ssSpread, ref ssSpread_Sheet);
            clsDP.ClearReciptPrint(ssSpread, ssSpread_Sheet);

            //예약조정 대상자 체크
            strYdate = COC.Check_Reserved_Adjust(pDbCon, ArgPtno, ArgBDate, ArgDept);

            #region //수납순번 가져오기
            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(MAX(SEQNO), 0) SEQNO FROM " + ComNum.DB_PMPA + "OPD_SUNAP ";
            SQL += ComNum.VBLF + "  WHERE ACTDATE   = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND PART      = '" + clsType.User.IdNumber + "' ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            if (DtP.Rows.Count > 0)
                nSeqNo = Convert.ToInt32(DtP.Rows[0]["SEQNO"].ToString());

            if (nSeqNo == 0)
                nSeqNo = 1;
            else
                nSeqNo += 1;

            DtP.Dispose();
            DtP = null;
            #endregion

            #region //수납시간 초
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS') Sdate FROM DUAL ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            if (DtP.Rows.Count > 0)
                strTimeSS = VB.Right(DtP.Rows[0]["Sdate"].ToString(), 2);

            DtP.Dispose();
            DtP = null;
            #endregion

            #region //Read_Opd_Slip

            for (int i = 0; i <= 28; i++)
            {
                for (j = 0; j <= 5; j++)
                    nPAmt[i, j] = 0;
            }

            Card.GnOgAmt = 0;

            //환자정보읽기
            CPO.READ_BAS_PATIENT(pDbCon, ArgPtno);
            CPO.READ_OPD_MASTER(pDbCon, ArgPtno, ArgDept, "", ArgBDate);
            
            //변수 clear
            Report_Print_Clear(ref RPA, ref RPG, ref RP);

            //영수증 재발행
            if (clsPmpaPb.GstrJeaPrint == "OK")
            {
                if (clsPmpaPb.GstrJeaEtcPrint != "OK") //기타수납 재발행 아닐경우
                {
                    clsPmpaPb.GstrERStat = "";

                    if (COC.Check_ER_AC101_Slip(pDbCon, ArgPtno, clsPmpaPb.GstrJeaDate, clsPmpaPb.GstrJeaDept, clsPmpaPb.GnJeaSeqNo, clsPmpaPb.GstrJeaBi))
                        clsPmpaPb.GstrERStat = "OK";

                    //환의수납여부
                    clsPmpaPb.GstrPCLR = "";
                    if (clsPmpaPb.GstrJeaDept == "ER")
                    {
                        if (COC.Check_ER_PCLR_Slip(pDbCon, ArgPtno, clsPmpaPb.GstrJeaDate, clsPmpaPb.GstrJeaDept, clsPmpaPb.GnJeaSeqNo, clsPmpaPb.GstrJeaBi, "Y", ""))
                            clsPmpaPb.GstrPCLR = "OK";
                    }

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT BUN, GBSELF, SUM(AMT1) NAMT, ";
                    SQL += ComNum.VBLF + "        SUM(AMT2) NAMT2, SUM(OGAMT) OGAMT, SUM(AMT4) NAMT4, ";
                    SQL += ComNum.VBLF + "        SUM(DanAmt) NDan ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND ActDate   = TO_DATE('" + clsPmpaPb.GstrJeaDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
                    SQL += ComNum.VBLF + "    AND Bi        = '" + clsPmpaPb.GstrJeaBi + "' ";
                    SQL += ComNum.VBLF + "    AND DeptCode  = '" + clsPmpaPb.GstrJeaDept + "' ";
                    SQL += ComNum.VBLF + "    AND SeqNo     =  " + clsPmpaPb.GnJeaSeqNo + " ";
                    SQL += ComNum.VBLF + "    AND PART      = '" + clsPmpaPb.GstrJeaPart + "' ";
                    SQL += ComNum.VBLF + "    AND TRIM(SUNEXT) NOT IN (SELECT CODE ";
                    SQL += ComNum.VBLF + "                               FROM ADMIN.BAS_BCODE ";
                    SQL += ComNum.VBLF + "                              WHERE GUBUN ='원무영수제외코드') "; //저가약제 제외코드 2010-11-22
                    SQL += ComNum.VBLF + "  GROUP BY BUN, GBSELF ";
                    SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtP.Dispose();
                        DtP = null;
                        return;
                    }
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT BUN, GBSELF, SUM(AMT) NAMT, ";
                    SQL += ComNum.VBLF + "        0 NAMT2, 0 OGAMT, 0 NAMT4, 0 NDan ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_ETCSLIP ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND ActDate   = TO_DATE('" + clsPmpaPb.GstrJeaDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
                    SQL += ComNum.VBLF + "    AND Bi        = '" + clsPmpaPb.GstrJeaBi + "' ";
                    SQL += ComNum.VBLF + "    AND Sunext NOT IN  ( 'Y96C' )  ";
                    SQL += ComNum.VBLF + "    AND SeqNo     =  " + clsPmpaPb.GnJeaSeqNo + " ";
                    SQL += ComNum.VBLF + "  GROUP BY BUN, GBSELF ";
                    SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtP.Dispose();
                        DtP = null;
                        return;
                    }
                }
            }
            else
            {
                //수탁검사
                if (ArgPtno.Trim().Length == 13)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT BUN, GBSELF, SUM(AMT) NAMT, ";
                    SQL += ComNum.VBLF + "        0 NAMT2, 0 OGAMT, 0 NAMT4, 0 NDan  ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_ETCSLIP ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND ACTDATE   = TRUNC(SYSDATE)   ";
                    SQL += ComNum.VBLF + "    AND JUMIN1    = '" + VB.Left(ArgPtno.Trim(), 6) + "' ";
                    //2018.06.21 박병규 : OPD_ETCSLIP에 주민번호 뒷자리는 암화가 안되어있음.
                    //SQL += ComNum.VBLF + "    AND JUMIN3    = '" + VB.Right(clsAES.AES(ArgPtno.Trim()), 7) + "' ";  //주민암호화
                    SQL += ComNum.VBLF + "    AND SEQNO     = '" + ArgSeq + "' ";
                    SQL += ComNum.VBLF + "    AND PART      = '" + clsType.User.JobPart + "'";
                    SQL += ComNum.VBLF + "  GROUP BY BUN, GBSELF ";
                    SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtP.Dispose();
                        DtP = null;
                        return;
                    }
                }
                else if (ArgGwa.Trim() == "II" || ArgGwa.Trim() == "재원자외래수납")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT BUN, GBSELF, SUM(AMT1) NAMT, ";
                    SQL += ComNum.VBLF + "        SUM(AMT2) NAMT2, SUM(OGAMT) OGAMT, SUM(AMT4) NAMT4, ";
                    SQL += ComNum.VBLF + "        SUM(DanAmt) NDan ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND ACTDATE   = TRUNC(SYSDATE)   ";
                    SQL += ComNum.VBLF + "    AND WARDCODE  = 'II'  ";
                    SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "'   ";
                    SQL += ComNum.VBLF + "    AND SEQNO     = '" + ArgSeq + "' ";
                    SQL += ComNum.VBLF + "    AND PART      = '" + clsType.User.JobPart + "'";
                    SQL += ComNum.VBLF + "    AND TRIM(SUNEXT) NOT IN (SELECT CODE ";
                    SQL += ComNum.VBLF + "                               FROM ADMIN.BAS_BCODE ";
                    SQL += ComNum.VBLF + "                              WHERE GUBUN ='원무영수제외코드') "; // 저가약제 제외코드 2010-11-22
                    SQL += ComNum.VBLF + "  GROUP BY BUN, GBSELF ";
                    SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtP.Dispose();
                        DtP = null;
                        return;
                    }
                }
                else
                {
                    //환의수납여부
                    clsPmpaPb.GstrPCLR = "";
                    if (ArgDept == "ER")
                    {
                        if (COC.Check_ER_PCLR_Slip(pDbCon, ArgPtno, clsPublic.GstrSysDate, ArgDept.Trim(), ArgSeq, "", "", clsType.User.JobPart))
                            clsPmpaPb.GstrPCLR = "OK";
                    }

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT BUN, GBSELF, SUM(AMT1) NAMT, ";
                    SQL += ComNum.VBLF + "        SUM(AMT2) NAMT2,SUM(OGAMT) OGAMT, SUM(AMT4) NAMT4, ";
                    SQL += ComNum.VBLF + "        SUM(DanAmt) NDan ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND ACTDATE   = TRUNC(SYSDATE)   ";
                    SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "'   ";
                    SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept.Trim() + "' ";
                    SQL += ComNum.VBLF + "    AND SEQNO     = '" + ArgSeq + "' ";
                    SQL += ComNum.VBLF + "    AND PART      = '" + clsType.User.IdNumber + "'";
                    SQL += ComNum.VBLF + "    AND TRIM(SUNEXT) NOT IN (SELECT CODE ";
                    SQL += ComNum.VBLF + "                               FROM ADMIN.BAS_BCODE ";
                    SQL += ComNum.VBLF + "                              WHERE GUBUN ='원무영수제외코드') "; // 저가약제 제외코드 2010-11-22
                    SQL += ComNum.VBLF + "  GROUP BY BUN, GBSELF ";
                    SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtP.Dispose();
                        DtP = null;
                        return;
                    }
                }
            }

            nTaxAmt = 0;
            nTaxDanAmt = 0;

            string strBun = "";

            for (int i = 0; i < DtP.Rows.Count; i++)
            {
                if (DtP.Rows[i]["GBSELF"].ToString().Trim() == "0")
                    nSelf = 1;
                else
                    nSelf = 2;

                strBun = DtP.Rows[i]["BUN"].ToString().Trim();

                if (strBun == "97")
                {
                    nTaxAmt += Convert.ToDouble(VB.Val(DtP.Rows[i]["NAMT4"].ToString()));
                    nTaxDanAmt += Convert.ToInt64(VB.Val(DtP.Rows[i]["NDan"].ToString()));
                }

                if (strBun == "99" || strBun == "98" || strBun == "92" || strBun == "96")
                    nPAmt[CBA.Rtn_Reception_Report(strBun), 1] += Convert.ToInt64(VB.Val(DtP.Rows[i]["NAMT"].ToString()));
                else
                    nPAmt[CBA.Rtn_Reception_Report(strBun), nSelf] += Convert.ToInt64(VB.Val(DtP.Rows[i]["NAMT"].ToString()));

                if (string.Compare(strBun, "84") <= 0)
                    nPAmt[17, nSelf] += Convert.ToInt64(VB.Val(DtP.Rows[i]["NAMT"].ToString())); //합계

                //산전승인금액
                if (strBun == "99" && Convert.ToInt64(VB.Val(DtP.Rows[i]["OGAMT"].ToString())) != 0)
                    Card.GnOgAmt += Convert.ToInt64(VB.Val(DtP.Rows[i]["OGAMT"].ToString()));
            }

            //예약비처리
            if (ArgRdate.Trim() != "")
            {
                nAmt8 = 0;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT AMT1, AMT2, AMT4, ";
                SQL += ComNum.VBLF + "        AMT5, AMT6, AMT7, ";
                SQL += ComNum.VBLF + "        AMT8, GbSPC  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW  ";
                SQL += ComNum.VBLF + "  WHERE PANO      = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "    AND DATE1     = TRUNC(SYSDATE)  ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept.Trim() + "'  ";
                SQL += ComNum.VBLF + "    AND BI        = '" + ArgBi + "' ";
                SQL += ComNum.VBLF + "    AND DATE3     = TO_DATE('" + ArgRdate + "','YYYY-MM-DD HH24:MI')    ";
                SQL += ComNum.VBLF + "    AND RETDATE IS NULL ";
                SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtSub.Dispose();
                    DtSub = null;
                    DtP.Dispose();
                    DtP = null;
                    return;
                }

                if (DtSub.Rows.Count > 0)
                {
                    RPG[22].Amt1 = Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT1"].ToString()));
                    RPG[22].Amt3 = Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT2"].ToString()));
                    //2018-09-14 변경

                    //RPG[22].Amt5 = Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT7"].ToString())) + Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT6"].ToString()))  + Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT5"].ToString())) - Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT2"].ToString()));
                    RPG[22].Amt5 = Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT7"].ToString())) + Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT5"].ToString())) - Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT2"].ToString()));

                    RPG[22].Amt6 = Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT4"].ToString()));

                    nPAmt[1, 1] = Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT1"].ToString()));
                    nPAmt[17, 1] += Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT1"].ToString()));
                    nPAmt[19, 1] += Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT4"].ToString()));
                    nPAmt[24, 1] += Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT7"].ToString()));
                    nPAmt[22, 1] += Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT5"].ToString()));
                    nPAmt[23, 1] += Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT6"].ToString()));
                    nAmt8 = Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT8"].ToString()));
                }

                DtSub.Dispose();
                DtSub = null;
            }

            if (clsPmpaPb.GstrJinGubun == "E" || clsPmpaPb.GstrJinGubun == "5" || clsPmpaPb.GstrJinGubun == "8" || clsPmpaPb.GstrJinGubun == "G" || clsPmpaPb.GstrJinGubun == "U" || clsPmpaPb.GstrJinGubun == "T")
            {
                RPG[40].Amt1 += Convert.ToInt64(clsPmpaPb.gnJinAMT1T);
                RPG[40].Amt3 += Convert.ToInt64(clsPmpaPb.gnJinAMT2T);
                RPG[40].Amt5 += Convert.ToInt64(clsPmpaPb.gnJinAMT7T + clsPmpaPb.gnJinAMT5T - clsPmpaPb.gnJinAMT2T);
                RPG[40].Amt6 += Convert.ToInt64(clsPmpaPb.gnJinAMT4T);

                nPAmt[1, 1] += Convert.ToInt64(clsPmpaPb.gnJinAMT1T);
                nPAmt[17, 1] += Convert.ToInt64(clsPmpaPb.gnJinAMT1T);
                nPAmt[19, 1] += Convert.ToInt64(clsPmpaPb.gnJinAMT4T);
                nPAmt[24, 1] += Convert.ToInt64(clsPmpaPb.gnJinAMT7T);
                nPAmt[22, 1] += Convert.ToInt64(clsPmpaPb.gnJinAMT5T);
                nPAmt[23, 1] += Convert.ToInt64(clsPmpaPb.gnJinAMT6T);

                nPAmt[17, 2] += Convert.ToInt64(clsPmpaPb.gnJinAMT2T);
            }

            if (ArgBi == "52" && nPAmt[17, 2] > 0 && clsPmpaPb.GstrSelUse == "OK")
            {
                nPAmt[20, 1] = nPAmt[17, 1] + nPAmt[17, 2];                 //진료비총액
                nPAmt[18, 1] = nPAmt[17, 1] - nPAmt[19, 1] + nPAmt[17, 2];  //급여본인부담금
                nPAmt[21, 1] = nPAmt[18, 1];                                //환자부담총액
            }
            else
            {
                nPAmt[20, 1] = nPAmt[17, 1] + nPAmt[17, 2];                 //진료비총액
                nPAmt[18, 1] = nPAmt[17, 1] - nPAmt[19, 1];                 //급여본인부담금
                nPAmt[21, 1] = nPAmt[18, 1] + nPAmt[17, 2];                 //환자부담총액
            }

            nPAmt[25, 1] = nPAmt[21, 1] - nPAmt[22, 1] - nPAmt[23, 1];      //수납금액

            //영수증인쇄시 인공신장 급여본인부담금이 - 금액이면 0 으로 표시
            if ((nPAmt[18, 1] < 0 && clsPmpaPb.GstrJeaDept == "HD") || (nPAmt[18, 1] < 0 && nPAmt[18, 1].ToString().Length < 3))
            {
                nPAmt[18, 1] = nPAmt[18, 1] * -1;

                if (nPAmt[18, 1] < 10)
                    nPAmt[18, 1] = 0;
                else
                    nPAmt[18, 1] = nPAmt[18, 1] * -1;
            }

            DtP.Dispose();
            DtP = null;

            //마지막 절사액 읽기
            if (clsPmpaPb.GstrJeaPrint == "OK")
                CPF.OPD_SUNAP_Last_Info(pDbCon, ArgPtno, clsPmpaPb.GstrJeaDate, clsPmpaType.TOM.BDate, ArgDept, clsPmpaPb.GstrJeaPart.Trim(), ArgSeq, "", ArgBi, "", "");
            else
            {
                if (clsPmpaType.TOM.MCode == "E000" || clsPmpaType.TOM.MCode == "F000" || clsPmpaType.TOM.GbDementia == "Y" || (clsPmpaPb.GstrResExamCHK == "OK" && clsPmpaPb.GstrResExamFlag == "OK"))
                    clsPmpaPb.GnOpd_Sunap_LastDan = 0;
                else
                    clsPmpaPb.GnOpd_Sunap_LastDan = clsPmpaPb.GnBTruncLastAmt;

                clsPmpaPb.GOpd_Sunap_GelCode = clsPmpaType.TOM.GelCode;
                clsPmpaPb.GOpd_Sunap_MCode = clsPmpaType.TOM.MCode;
                clsPmpaPb.GOpd_Sunap_VCode = clsPmpaType.TOM.VCode;
                clsPmpaPb.GOpd_Sunap_Jin = clsPmpaType.TOM.Jin;
                clsPmpaPb.GOpd_Sunap_JinDtl = clsPmpaType.TOM.JinDtl;
                clsPmpaPb.GOpd_Sunap_II = ArgGwa;

                clsPmpaPb.GOpd_Sunap_Boamt = clsPmpaType.BAT.M3_GC_Amt;
                clsPmpaPb.GOpd_Sunap_EFamt = clsPmpaPb.GnBoninAmt_EF;
            }

            //II과이면서 자보는 일반수가를 적용함
            if (ArgDept == "II" && clsPmpaType.TOM.Bi == "52")
                clsPmpaType.TOM.Bi = "51";

            clsPmpaType.a.Date = ArgBDate;
            clsPmpaType.a.Dept = ArgDept;
            clsPmpaType.a.Sex = clsPmpaType.TOM.Sex;
            clsPmpaType.a.GbGameK = clsPmpaType.TOM.GbGameK;
            clsPmpaType.a.GbGameKC = clsPmpaType.TOM.GbGameKC;
            clsPmpaType.a.Retn = 0;
            clsPmpaType.a.Bi = Convert.ToInt32(VB.Val(clsPmpaType.TOM.Bi));

            if (clsPmpaType.a.Bi != 0)
                clsPmpaType.a.Bi1 = Convert.ToInt32(VB.Val(VB.Left(clsPmpaType.TOM.Bi, 1)));
            else
                clsPmpaType.a.Bi1 = 0;

            if (clsPmpaType.a.Bi == 52 || clsPmpaType.a.Bi == 55)
                clsPmpaType.a.Bi1 = 6; //자보

            if (COC.CHK_SANID_GASAN(clsDB.DbCon, clsPmpaType.TOM.Pano, clsPmpaType.TOM.Bi, clsPmpaType.TOM.DrCode, clsPmpaType.TOM.BDate) == true && clsPmpaPb.GstrSanAdd_NOT == "") { clsPmpaType.a.Bi1 = 7; } //자보

            clsPmpaType.a.Age = clsPmpaType.TOM.Age;
            clsPmpaType.a.AgeiLsu = 99;

            if (clsPmpaType.a.Age == 0)
                clsPmpaType.a.AgeiLsu = CF.DATE_ILSU(pDbCon, clsPmpaType.a.Date, "20" + VB.Left(clsPmpaType.TBP.Jumin1, 2) + "-" + VB.Mid(clsPmpaType.TBP.Jumin1, 3, 2) + VB.Right(clsPmpaType.TBP.Jumin1, 2));

            clsPmpaType.a.Gbilban2 = clsPmpaType.TOM.Gbilban2;
            clsPmpaType.a.Pano = clsPmpaType.TOM.Pano;
            clsPmpaType.a.DrCode = clsPmpaType.TOM.DrCode;
            clsPmpaType.a.GbSpc = clsPmpaType.TOM.GbSpc;

            if (clsPmpaPb.GOpd_Sunap_GelCode != "") { clsPmpaType.TOM.GelCode = clsPmpaPb.GOpd_Sunap_GelCode; }
            if (clsPmpaPb.GOpd_Sunap_MCode != "") { clsPmpaType.TOM.MCode = clsPmpaPb.GOpd_Sunap_MCode; }
            if (clsPmpaPb.GOpd_Sunap_VCode != "") { clsPmpaType.TOM.VCode = clsPmpaPb.GOpd_Sunap_VCode; }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.DeptCode,a.SuCode,a.SuNext, ";
            SQL += ComNum.VBLF + "        a.Bi,a.Bun,a.Nu, ";
            SQL += ComNum.VBLF + "        a.Qty,a.Nal,a.BaseAmt, ";
            SQL += ComNum.VBLF + "        a.GbSugbS, a.GbSpc,a.GbNgt, ";
            SQL += ComNum.VBLF + "        a.GbGisul,a.GbSelf,a.GbChild, ";
            SQL += ComNum.VBLF + "        a.DrCode,a.WardCode, a.GbSlip, ";
            SQL += ComNum.VBLF + "        a.GbHost,a.Amt1,a.Amt2, ";
            SQL += ComNum.VBLF + "        a.Jamt,a.Bamt,a.OrderNo, ";
            SQL += ComNum.VBLF + "        a.GbImiv,a.GbBunup, ";
            SQL += ComNum.VBLF + "        TO_CHAR(a.BDate,'YYYY-MM-DD') BDate, ";
            SQL += ComNum.VBLF + "        a.GbIpd, a.CardSeqNo,a.DosCode, ";
            SQL += ComNum.VBLF + "        a.ABCDATE,a.OPER_DEPT,a.OPER_DCT, ";
            SQL += ComNum.VBLF + "        a.CARDSEQNO,a.OgAmt,a.DanAmt, ";
            SQL += ComNum.VBLF + "        a.GbSpc_No, a.MULTI,a.MULTIREMARK, ";
            SQL += ComNum.VBLF + "        a.DIV,a.DUR,a.KSJIN, ";
            SQL += ComNum.VBLF + "        a.SCODESAYU,a.SCODEREMARK,a.Amt4 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP a, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND a.Sunext  = b.Sunext(+) ";
            SQL += ComNum.VBLF + "    AND a.Pano    = '" + ArgPtno + "' ";

            if (ArgDept == "II" && clsPmpaPb.GstrllDept != "")
                SQL += ComNum.VBLF + "AND a.DeptCode = '" + clsPmpaPb.GstrllDept + "' ";
            else
                SQL += ComNum.VBLF + "AND a.DeptCode = '" + ArgDept + "' ";
            
            if (clsPmpaPb.GstrJeaPrint == "OK")
            {
                SQL += ComNum.VBLF + "AND a.ActDate = TO_DATE('" + clsPmpaPb.GstrJeaDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "AND a.Part    = '" + clsPmpaPb.GstrJeaPart.Trim() + "' ";
            }
            else
            {
                SQL += ComNum.VBLF + "AND a.ActDate = TRUNC(SYSDATE) ";
                SQL += ComNum.VBLF + "AND a.Part    = '" + clsType.User.IdNumber + "'  ";
            }

            SQL += ComNum.VBLF + "    AND TRIM(a.SUNEXT) NOT IN (SELECT CODE ";
            SQL += ComNum.VBLF + "                                 FROM ADMIN.BAS_BCODE ";
            SQL += ComNum.VBLF + "                                WHERE GUBUN ='원무영수제외코드') "; // 저가약제 제외코드 2010-11-22
            SQL += ComNum.VBLF + "    AND a.SeqNo   = " + ArgSeq + " ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            for (int i = 0; i < DtP.Rows.Count; i++)
            {
                RP[i].SuCode = DtP.Rows[i]["SUCODE"].ToString().Trim();
                RP[i].SuNext = DtP.Rows[i]["Sunext"].ToString().Trim();
                RP[i].Bi = DtP.Rows[i]["Bi"].ToString().Trim();
                RP[i].BDate = DtP.Rows[i]["BDate"].ToString().Trim();
                RP[i].Bun = DtP.Rows[i]["BUN"].ToString().Trim();
                RP[i].Nu = DtP.Rows[i]["NU"].ToString().Trim();
                RP[i].Qty = Convert.ToDouble(DtP.Rows[i]["QTY"].ToString().Trim());
                RP[i].Nal = Convert.ToInt32(DtP.Rows[i]["NAL"].ToString().Trim());
                RP[i].BaseAmt = Convert.ToInt64(DtP.Rows[i]["BASEAMT"].ToString().Trim());
                RP[i].GbSpc = DtP.Rows[i]["GBSPC"].ToString().Trim();
                RP[i].GbNgt = DtP.Rows[i]["GBNGT"].ToString().Trim();
                RP[i].GbGisul = DtP.Rows[i]["GBGISUL"].ToString().Trim();
                RP[i].SugbS = DtP.Rows[i]["GBSUGBS"].ToString().Trim();
                RP[i].GbSelf = DtP.Rows[i]["GBSELF"].ToString().Trim();

                switch (RP[i].GbSelf)
                {
                    //SugbS : 1.100/100, 4.100/80, 5.100/50, 6.100/80
                    case "0":
                        if (RP[i].SugbS == "3" ||  RP[i].SugbS == "4" || RP[i].SugbS == "5" || RP[i].SugbS == "6" || RP[i].SugbS == "7" || RP[i].SugbS == "8" || RP[i].SugbS == "9") 
                            RP[i].GbSelf = "2";
                        break;

                    case "1":
                        if (RP[i].SugbS == "1" )
                            RP[i].GbSelf = "2";
                        break;

                    case "2":
                        if (RP[i].SugbS == "1")
                            RP[i].GbSelf = "2";
                        else
                            RP[i].GbSelf = "1";
                        break;
                }

                RP[i].GbChild = DtP.Rows[i]["GBCHILD"].ToString().Trim();
                RP[i].DrCode = DtP.Rows[i]["DRCODE"].ToString().Trim();
                RP[i].DeptCode = DtP.Rows[i]["DEPTCODE"].ToString().Trim();
                RP[i].WardCode = DtP.Rows[i]["WARDCODE"].ToString().Trim();
                RP[i].GbSlip = DtP.Rows[i]["GBSLIP"].ToString().Trim();
                RP[i].GbHost = DtP.Rows[i]["GBHOST"].ToString().Trim();
                RP[i].Amt1 = Convert.ToInt64(VB.Val(DtP.Rows[i]["AMT1"].ToString().Trim()));
                RP[i].Amt2 = Convert.ToInt64(VB.Val(DtP.Rows[i]["AMT2"].ToString().Trim()));
                RP[i].Amt4 = Convert.ToInt64(VB.Val(DtP.Rows[i]["AMT4"].ToString().Trim()));

                RP[i].OrderNo = Convert.ToInt64(VB.Val(DtP.Rows[i]["ORDERNO"].ToString().Trim()));
                RP[i].GbImiv = DtP.Rows[i]["GBIMIV"].ToString().Trim();
                RP[i].GbBunup = DtP.Rows[i]["GBBUNUP"].ToString().Trim();
                RP[i].DosCode = DtP.Rows[i]["DOSCODE"].ToString().Trim();
                RP[i].GbIPD = DtP.Rows[i]["GBIPD"].ToString().Trim();
                RP[i].KsJin = DtP.Rows[i]["KSJIN"].ToString().Trim();
                RP[i].DanAmt = Convert.ToInt64(VB.Val(DtP.Rows[i]["DANAMT"].ToString().Trim()));
                RP[i].GbSpc_No = DtP.Rows[i]["GbSpc_No"].ToString().Trim();

                if (RP[i].SuNext == "SPEECH1A")
                    RP[i].Bun = "47";
            }

            //영수 금액 읽기
            for (int i = 0; i < 999; i++)
            {
                if (RP[i].SuCode == null || RP[i].SuCode == "") { break; }

                if (RP[i].SuCode.Trim() != "")
                    Report_Slip_AmtAdd(i, ref RP, ref RPG);
            }

            //영수금액계산
            Last_Report_Amt_Gesan(ref RPG);

            DtP.Dispose();
            DtP = null;

            #endregion

            #region //INSERT/UPDATE
            if (clsPmpaPb.GstrPrintChk != "NO")
            {
                if (clsAuto.GstrAREquipUse != "OK")
                {
                    if (nPAmt[24, 1] == 0)
                        strMsgChk = ComFunc.MsgBoxQ("수납금액 없음. 영수증을 출력하시겠습니까?", "출력선택", MessageBoxDefaultButton.Button1).ToString();
                    else
                        strMsgChk =  ComFunc.MsgBoxQ("영수증을 출력하시겠습니까?", "출력선택", MessageBoxDefaultButton.Button1).ToString();
                }
                else if (clsAuto.GstrAREquipUse == "OK")
                {
                    strMsgChk = "Yes"; //멘트확인안하고 인쇄

                    if (clsAuto.GnAR_AMT == 0 && ArgRdate.Trim() == "" && clsPmpaType.TOM.GbGameK != "11")
                    {
                        if (clsPmpaPb.GstrMobileSunap != "OK")
                            strMsgChk = "No";
                    }
                }
            }
            else
            {
                strMsgChk = "Yes"; //멘트확인안하고 인쇄
            }

            if (clsPmpaPb.GstrMobilePrint == "OK") { strMsgChk = "Yes"; }

            if (strMsgChk == "No")
            {
                if (clsAuto.GstrAREquipUse != "OK")
                {
                    if (clsPmpaPb.GnOutTuyakNo > 0 && clsPmpaPb.GnTuyakNo > 0)
                        ComFunc.MsgBox("원외처방번호 : " + clsPmpaPb.GnOutTuyakNo + ", 투약번호 : " + clsPmpaPb.GnTuyakNo + " 입니다.", "확인");
                    else if (clsPmpaPb.GnOutTuyakNo > 0 && clsPmpaPb.GnTuyakNo <= 0)
                        ComFunc.MsgBox("원외처방번호 : " + clsPmpaPb.GnOutTuyakNo + " 입니다.", "확인");
                    else if (clsPmpaPb.GnOutTuyakNo <= 0 && clsPmpaPb.GnTuyakNo > 0)
                        ComFunc.MsgBox("투약번호 : " + clsPmpaPb.GnTuyakNo + " 입니다.", "확인");
                }

                if (clsPmpaPb.GstrJeaSunap == "YES")
                {
                    //조별로 수납시 순차적으로 INSERT 함. 마감을 점검 하기위한 문
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO OPD_SUNAP ";
                    SQL += ComNum.VBLF + "        (ACTDATE, OPDNO, PANO, AMT, ";
                    SQL += ComNum.VBLF + "         PART, SEQNO,STIME, ";
                    SQL += ComNum.VBLF + "         BIGO, REMARK, AMT1, ";
                    SQL += ComNum.VBLF + "         AMT2, AMT3, AMT4, ";
                    SQL += ComNum.VBLF + "         AMT5, SEQNO2, DEPTCODE, ";
                    SQL += ComNum.VBLF + "         BI, CARDGB,CardAmt, ";
                    SQL += ComNum.VBLF + "         WorkGbn,GbSPC,GelCode, ";
                    SQL += ComNum.VBLF + "         MCode,VCode,BDan, ";
                    SQL += ComNum.VBLF + "         CDan,Jin,JinDtl, ";
                    SQL += ComNum.VBLF + "         EtcAmt,EtcAmt2,EntDate, ";
                    SQL += ComNum.VBLF + "         BDate,PtAmt,AL200, ";
                    SQL += ComNum.VBLF + "         amt6,Amt7,TaxDan) ";
                    SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "         '" + ArgOpdNo + "',  ";
                    SQL += ComNum.VBLF + "         '" + ArgPtno + "',  ";
                    SQL += ComNum.VBLF + "          " + nPAmt[24, 1] + ", ";
                    SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                    SQL += ComNum.VBLF + "          " + nSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                    SQL += ComNum.VBLF + "         '출력안함', ";
                    SQL += ComNum.VBLF + "         '" + ArgSunap +"', ";
                    SQL += ComNum.VBLF + "          " + nPAmt[20, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + nPAmt[19, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + nPAmt[21, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + nPAmt[22, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + nPAmt[23, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + ArgSeq + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgDept.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgBi.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.RSD.Gubun + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.RSD.TotAmt + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaPb.GstrDrugJobGb + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgSpc + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgGelCode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgMcode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgVcode + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnBTruncLastAmt + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnHTruncLastAmt + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgJin + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgJinDtl + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.BAT.M3_GC_Amt + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnBoninAmt_EF + ", ";
                    SQL += ComNum.VBLF + "         SYSDATE, ";
                    SQL += ComNum.VBLF + "         TO_DATE('" + ArgBDate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnPtVoucherAmt + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaPb.GstrAL200_NOT + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT7T + ", ";
                    SQL += ComNum.VBLF + "          " + nTaxAmt + ", ";
                    SQL += ComNum.VBLF + "          " + nTaxDanAmt + ") ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("OPD_SUNAP INSERT Error !!");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        clsPmpaPb.strDataFlag = "NO";
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    //clsDB.setCommitTran(pDbCon);

                    clsPmpaPb.GnPatAmt = 0;
                    clsPmpaPb.GnJanAmt = 0;
                }

                //예약접수증
                if (clsPmpaPb.GblnResvPrint == false)
                {
                    if (ArgRdate.Trim() != "")
                    {
                        if (clsType.User.IdNumber == "4349")
                        {
                            if (DialogResult.Yes == ComFunc.MsgBoxQ("예약접수증을 출력하시겠습니까?", "알림"))
                                ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, ArgDr, ArgRdate,"", o);
                        }
                        else
                            ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, ArgDr, ArgRdate,"", o);
                    }
                }

                return;
            }
            else
            {
                if (clsPmpaPb.GstrJeaSunap == "YES")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO OPD_SUNAP ";
                    SQL += ComNum.VBLF + "        (ACTDATE, OPDNO, PANO, AMT, ";
                    SQL += ComNum.VBLF + "         PART, SEQNO,STIME, ";
                    SQL += ComNum.VBLF + "         BIGO, REMARK, AMT1, ";
                    SQL += ComNum.VBLF + "         AMT2, AMT3, AMT4, ";
                    SQL += ComNum.VBLF + "         AMT5, SEQNO2, DEPTCODE, ";
                    SQL += ComNum.VBLF + "         BI, CARDGB,CardAmt, ";
                    SQL += ComNum.VBLF + "         WorkGbn,GbSPC,GelCode, ";
                    SQL += ComNum.VBLF + "         MCode,VCode,BDan, ";
                    SQL += ComNum.VBLF + "         CDan,Jin,JinDtl, ";
                    SQL += ComNum.VBLF + "         EtcAmt,EtcAmt2,EntDate, ";
                    SQL += ComNum.VBLF + "         BDate,PtAmt,AL200, ";
                    SQL += ComNum.VBLF + "         amt6,Amt7,TaxDan) ";
                    SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "         '" + ArgOpdNo + "',  ";
                    SQL += ComNum.VBLF + "         '" + ArgPtno + "',  ";
                    SQL += ComNum.VBLF + "          " + nPAmt[24, 1] + ", ";
                    SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                    SQL += ComNum.VBLF + "          " + nSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                    SQL += ComNum.VBLF + "         '', ";
                    SQL += ComNum.VBLF + "         '" + ArgSunap + "', ";
                    SQL += ComNum.VBLF + "          " + nPAmt[20, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + nPAmt[19, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + nPAmt[21, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + nPAmt[22, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + nPAmt[23, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + ArgSeq + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgDept.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgBi.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.RSD.Gubun + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.RSD.TotAmt + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaPb.GstrDrugJobGb + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgSpc + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgGelCode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgMcode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgVcode + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnBTruncLastAmt + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnHTruncLastAmt + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgJin + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgJinDtl + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.BAT.M3_GC_Amt + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnBoninAmt_EF + ", ";
                    SQL += ComNum.VBLF + "         SYSDATE,";
                    SQL += ComNum.VBLF + "         TO_DATE('" + ArgBDate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnPtVoucherAmt + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaPb.GstrAL200_NOT + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT7T + ", ";
                    SQL += ComNum.VBLF + "          " + nTaxAmt + ", ";
                    SQL += ComNum.VBLF + "          " + nTaxDanAmt + ") ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("OPD_HOANBUL INSERT Error !!");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        clsPmpaPb.strDataFlag = "NO";
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsPmpaPb.GnPatAmt = 0;
                    clsPmpaPb.GnJanAmt = 0;

                }

                //예약접수증
                if (clsPmpaPb.GblnResvPrint == false)
                {
                    if (ArgRdate.Trim() != "")
                    {
                        if (clsType.User.IdNumber == "4349")
                        {
                            if (DialogResult.Yes == ComFunc.MsgBoxQ("예약접수증을 출력하시겠습니까?", "알림"))
                                ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, ArgDr, ArgRdate,"", o);
                        }
                        else
                            ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, ArgDr, ArgRdate,"", o);
                    }
                }
            }

            #endregion

            //영수증출력
            clsPrint CP = new clsPrint();
            Card CC = new Card();

            clsPmpaPb.GstrMobilePrint = "";
                        

            if (clsPmpaPb.GstrMobileSunap != "OK")
            {
                CP.getPrinter_Chk(RECEIPT_PRINTER_NAME);
                strPrintName = CP.getPmpaBarCodePrinter(RECEIPT_PRINTER_NAME);
                if (strPrintName.Trim() == "") { return; }

                if (ArgIpdDtl == "Y")
                    ssSpread_Sheet.Cells[0, 4].Text = "입원정밀검진";
                else
                {
                    if (clsPmpaPb.GstatEROVER != "*")
                    {
                        if (ArgRe == "R")
                        {
                            if (clsPmpaPb.GOpd_Sunap_II == "II" || clsPmpaPb.Gstr입원재출력 == "OK")
                                ssSpread_Sheet.Cells[0, 4].Text = "입원(재발행)";
                            else
                                ssSpread_Sheet.Cells[0, 4].Text = "외래(재발행)";
                        }
                        else
                        {
                            if (clsPmpaPb.GOpd_Sunap_II == "II" || clsPmpaPb.Gstr입원재출력 == "OK")
                                ssSpread_Sheet.Cells[0, 4].Text = "입원";
                            else
                                ssSpread_Sheet.Cells[0, 4].Text = "외래";
                        }
                    }
                    else
                        if (ArgRe == "R")
                            ssSpread_Sheet.Cells[0, 4].Text = "응급(재발행)";
                        else
                            ssSpread_Sheet.Cells[0, 4].Text = "응급";
                }

                if (clsPmpaPb.GstrErJobFlag == "OK")
                {
                    if (clsPmpaPb.GnNight1 == "2")
                        ssSpread_Sheet.Cells[3, 13].Text = "√";
                    else if (clsPmpaPb.GnNight1 == "1")
                        ssSpread_Sheet.Cells[3, 13].Text = VB.Space(11) + "√";
                }

                ssSpread_Sheet.Cells[0, 12].Text = ArgPtno + VB.Asc(VB.Left(ArgDept, 1)) + VB.Asc(VB.Right(ArgDept, 1)); //바코드
            }

            if (ArgGwa == "II") //수납항목을 인쇄(가셔야할곳 인쇄)
                PR_Site_Move_II(ArgDept, ArgRetn, ArgBDate, ArgIpdDtl,ref ssSpread, ref ssSpread_Sheet);
            else
                PR_Site_Move4(pDbCon, ArgDept, ArgRetn, ArgBDate, ArgIpdDtl, ref ssSpread, ref ssSpread_Sheet);

            if (clsPmpaPb.GstrMobileSunap == "OK") { return; }

            if (clsPmpaPb.GstrJeaPrint != "OK")
            {
                for (j = 0; j < FnPo; j++)
                {
                    ssSpread_Sheet.Cells[22 + j, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[22 + j, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[22 + j, 12].Text = FstrSite[j];
                }
            }

            //if (strYdate != "")
            //{
            //    j += 1;
            //    ssSpread_Sheet.Cells[22 + j, 12].ColumnSpan = 4;
            //    ssSpread_Sheet.Cells[22 + j, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
            //    ssSpread_Sheet.Cells[22 + j, 12].Text = "금일 예약비는 " + strYdate + " 에";
            //    j += 1;
            //    ssSpread_Sheet.Cells[22 + j, 12].ColumnSpan = 4;
            //    ssSpread_Sheet.Cells[22 + j, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
            //    ssSpread_Sheet.Cells[22 + j, 12].Text = "이미 대체되었습니다.";
            //}

            if (clsPmpaPb.GstrCopyPrint == "OK")
            {
                //2018.06.20 박병규 : 출력위치변경
                //ssSpread_Sheet.Cells[22 + FnPo, 12].ColumnSpan = 4;
                //ssSpread_Sheet.Cells[22 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                //ssSpread_Sheet.Cells[22 + FnPo, 12].Text = "사 본";

                ssSpread_Sheet.Cells[0, 4].Text += " [사본]";
            }

            //약 처방전번호
            if (ArgGwa != "II")
            {
                if (clsPmpaPb.GnTuyakNo > 0 && clsPmpaPb.GnOutDrugNo > 0)
                {
                    ssSpread_Sheet.Cells[20, 13].Text = clsPmpaPb.GnTuyakNo.ToString();
                    ssSpread_Sheet.Cells[21, 13].Text = clsPmpaPb.GnOutDrugNo.ToString();
                }
                else if (clsPmpaPb.GnTuyakNo > 0)
                    ssSpread_Sheet.Cells[20, 13].Text = clsPmpaPb.GnTuyakNo.ToString();
                else if (clsPmpaPb.GnOutDrugNo > 0)
                    ssSpread_Sheet.Cells[21, 13].Text = clsPmpaPb.GnOutDrugNo.ToString();
            }
            else if (ArgGwa == "II")
            {
                if (FnPo >= 9)
                    ssSpread_Sheet.Cells[21, 13].Text = "◈수납확인서◈" + " " + clsPmpaType.TOM.WardCode.Trim() + "-" + clsPmpaType.TOM.RoomCode;
            }

            //공급받는자보관용
            ssSpread_Sheet.Cells[3, 0].Text = ArgPtno;
            ssSpread_Sheet.Cells[3, 4].Text = ArgName;

            if (clsPmpaPb.GstrJeaPrint == "OK")
                ssSpread_Sheet.Cells[3, 8].Text = ArgBDate;
            else
                ssSpread_Sheet.Cells[3, 8].Text = clsPublic.GstrSysDate;

            ssSpread_Sheet.Cells[5, 0].Text = ArgGwa;

            if (ArgBi == "43" && ArgJinDtl == "18")
                ssSpread_Sheet.Cells[5, 11].Text = "보호100%";
            else
                ssSpread_Sheet.Cells[5, 11].Text = CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", ArgBi);

            //수납자 영수증번호
            if (clsPmpaPb.GstrJeaPrint == "OK")
                ssSpread_Sheet.Cells[5, 12].Text = clsPmpaPb.GstrJeaName + "(" + clsPmpaPb.GstrJeaPart.Trim() + ") " + ArgSeq;
            else
                ssSpread_Sheet.Cells[5, 12].Text = clsType.User.JobName + "(" + clsType.User.IdNumber + ") " + ArgSeq;

            //진료항목
            //진찰료
            ssSpread_Sheet.Cells[9, 3].Text = string.Format("{0:#,###}", RPG[0].Amt5 + RPG[40].Amt5); //급여 본인
            ssSpread_Sheet.Cells[9, 5].Text = string.Format("{0:#,###}", RPG[0].Amt6 + RPG[40].Amt6); //급여 공단
            ssSpread_Sheet.Cells[9, 6].Text = string.Format("{0:#,###}", RPG[0].Amt4 + RPG[40].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[9, 8].Text = string.Format("{0:#,###}", RPG[0].Amt3 + RPG[40].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[9, 10].Text = string.Format("{0:#,###}", RPG[0].Amt2 + RPG[40].Amt2); //비급여 선택진료이외
            //투약및조제료(행위료)
            ssSpread_Sheet.Cells[12, 3].Text = string.Format("{0:#,###}", RPG[3].Amt5); //급여 본인
            ssSpread_Sheet.Cells[12, 5].Text = string.Format("{0:#,###}", RPG[3].Amt6); //급여 공단
            ssSpread_Sheet.Cells[12, 6].Text = string.Format("{0:#,###}", RPG[3].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[12, 8].Text = string.Format("{0:#,###}", RPG[3].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[12, 10].Text = string.Format("{0:#,###}", RPG[3].Amt2); //비급여 선택진료이외
            //투약및조제료(약품비)
            ssSpread_Sheet.Cells[13, 3].Text = string.Format("{0:#,###}", RPG[4].Amt5); //급여 본인
            ssSpread_Sheet.Cells[13, 5].Text = string.Format("{0:#,###}", RPG[4].Amt6); //급여 공단
            ssSpread_Sheet.Cells[13, 6].Text = string.Format("{0:#,###}", RPG[4].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[13, 8].Text = string.Format("{0:#,###}", RPG[4].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[13, 10].Text = string.Format("{0:#,###}", RPG[4].Amt2); //비급여 선택진료이외
            //주사료(행위료)
            ssSpread_Sheet.Cells[14, 3].Text = string.Format("{0:#,###}", RPG[5].Amt5); //급여 본인
            ssSpread_Sheet.Cells[14, 5].Text = string.Format("{0:#,###}", RPG[5].Amt6); //급여 공단
            ssSpread_Sheet.Cells[14, 6].Text = string.Format("{0:#,###}", RPG[5].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[14, 8].Text = string.Format("{0:#,###}", RPG[5].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[14, 10].Text = string.Format("{0:#,###}", RPG[5].Amt2); //비급여 선택진료이외
            //주사료(약품비)
            ssSpread_Sheet.Cells[15, 3].Text = string.Format("{0:#,###}", RPG[6].Amt5); //급여 본인
            ssSpread_Sheet.Cells[15, 5].Text = string.Format("{0:#,###}", RPG[6].Amt6); //급여 공단
            ssSpread_Sheet.Cells[15, 6].Text = string.Format("{0:#,###}", RPG[6].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[15, 8].Text = string.Format("{0:#,###}", RPG[6].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[15, 10].Text = string.Format("{0:#,###}", RPG[6].Amt2); //비급여 선택진료이외
            //마취료
            ssSpread_Sheet.Cells[16, 3].Text = string.Format("{0:#,###}", RPG[7].Amt5); //급여 본인
            ssSpread_Sheet.Cells[16, 5].Text = string.Format("{0:#,###}", RPG[7].Amt6); //급여 공단
            ssSpread_Sheet.Cells[16, 6].Text = string.Format("{0:#,###}", RPG[7].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[16, 8].Text = string.Format("{0:#,###}", RPG[7].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[16, 10].Text = string.Format("{0:#,###}", RPG[7].Amt2); //비급여 선택진료이외
            //처치및수술료
            ssSpread_Sheet.Cells[17, 3].Text = string.Format("{0:#,###}", RPG[8].Amt5); //급여 본인
            ssSpread_Sheet.Cells[17, 5].Text = string.Format("{0:#,###}", RPG[8].Amt6); //급여 공단
            ssSpread_Sheet.Cells[17, 6].Text = string.Format("{0:#,###}", RPG[8].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[17, 8].Text = string.Format("{0:#,###}", RPG[8].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[17, 10].Text = string.Format("{0:#,###}", RPG[8].Amt2); //비급여 선택진료이외
            //검사료
            ssSpread_Sheet.Cells[18, 3].Text = string.Format("{0:#,###}", RPG[9].Amt5); //급여 본인
            ssSpread_Sheet.Cells[18, 5].Text = string.Format("{0:#,###}", RPG[9].Amt6); //급여 공단
            ssSpread_Sheet.Cells[18, 6].Text = string.Format("{0:#,###}", RPG[9].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[18, 8].Text = string.Format("{0:#,###}", RPG[9].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[18, 10].Text = string.Format("{0:#,###}", RPG[9].Amt2); //비급여 선택진료이외
            //영상진단료
            ssSpread_Sheet.Cells[19, 3].Text = string.Format("{0:#,###}", RPG[10].Amt5); //급여 본인
            ssSpread_Sheet.Cells[19, 5].Text = string.Format("{0:#,###}", RPG[10].Amt6); //급여 공단
            ssSpread_Sheet.Cells[19, 6].Text = string.Format("{0:#,###}", RPG[10].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[19, 8].Text = string.Format("{0:#,###}", RPG[10].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[19, 10].Text = string.Format("{0:#,###}", RPG[10].Amt2); //비급여 선택진료이외
            //방사선치료료
            ssSpread_Sheet.Cells[20, 3].Text = string.Format("{0:#,###}", RPG[11].Amt5); //급여 본인
            ssSpread_Sheet.Cells[20, 5].Text = string.Format("{0:#,###}", RPG[11].Amt6); //급여 공단
            ssSpread_Sheet.Cells[20, 6].Text = string.Format("{0:#,###}", RPG[11].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[20, 8].Text = string.Format("{0:#,###}", RPG[11].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[20, 10].Text = string.Format("{0:#,###}", RPG[11].Amt2); //비급여 선택진료이외
            //치료재료대
            ssSpread_Sheet.Cells[21, 3].Text = string.Format("{0:#,###}", RPG[12].Amt5); //급여 본인
            ssSpread_Sheet.Cells[21, 5].Text = string.Format("{0:#,###}", RPG[12].Amt6); //급여 공단
            ssSpread_Sheet.Cells[21, 6].Text = string.Format("{0:#,###}", RPG[12].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[21, 8].Text = string.Format("{0:#,###}", RPG[12].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[21, 10].Text = string.Format("{0:#,###}", RPG[12].Amt2); //비급여 선택진료이외
            //재활및물리치료료
            ssSpread_Sheet.Cells[22, 3].Text = string.Format("{0:#,###}", RPG[13].Amt5); //급여 본인
            ssSpread_Sheet.Cells[22, 5].Text = string.Format("{0:#,###}", RPG[13].Amt6); //급여 공단
            ssSpread_Sheet.Cells[22, 6].Text = string.Format("{0:#,###}", RPG[13].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[22, 8].Text = string.Format("{0:#,###}", RPG[13].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[22, 10].Text = string.Format("{0:#,###}", RPG[13].Amt2); //비급여 선택진료이외
            //정신요법료
            ssSpread_Sheet.Cells[23, 3].Text = string.Format("{0:#,###}", RPG[14].Amt5); //급여 본인
            ssSpread_Sheet.Cells[23, 5].Text = string.Format("{0:#,###}", RPG[14].Amt6); //급여 공단
            ssSpread_Sheet.Cells[23, 6].Text = string.Format("{0:#,###}", RPG[14].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[23, 8].Text = string.Format("{0:#,###}", RPG[14].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[23, 10].Text = string.Format("{0:#,###}", RPG[14].Amt2); //비급여 선택진료이외
            //수혈료
            ssSpread_Sheet.Cells[24, 3].Text = string.Format("{0:#,###}", RPG[15].Amt5); //급여 본인
            ssSpread_Sheet.Cells[24, 5].Text = string.Format("{0:#,###}", RPG[15].Amt6); //급여 공단
            ssSpread_Sheet.Cells[24, 6].Text = string.Format("{0:#,###}", RPG[15].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[24, 8].Text = string.Format("{0:#,###}", RPG[15].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[24, 10].Text = string.Format("{0:#,###}", RPG[15].Amt2); //비급여 선택진료이외
            //CT
            ssSpread_Sheet.Cells[25, 3].Text = string.Format("{0:#,###}", RPG[16].Amt5); //급여 본인
            ssSpread_Sheet.Cells[25, 5].Text = string.Format("{0:#,###}", RPG[16].Amt6); //급여 공단
            ssSpread_Sheet.Cells[25, 6].Text = string.Format("{0:#,###}", RPG[16].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[25, 8].Text = string.Format("{0:#,###}", RPG[16].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[25, 10].Text = string.Format("{0:#,###}", RPG[16].Amt2); //비급여 선택진료이외
            //MRI
            ssSpread_Sheet.Cells[26, 3].Text = string.Format("{0:#,###}", RPG[17].Amt5); //급여 본인
            ssSpread_Sheet.Cells[26, 5].Text = string.Format("{0:#,###}", RPG[17].Amt6); //급여 공단
            ssSpread_Sheet.Cells[26, 6].Text = string.Format("{0:#,###}", RPG[17].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[26, 8].Text = string.Format("{0:#,###}", RPG[17].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[26, 10].Text = string.Format("{0:#,###}", RPG[17].Amt2); //비급여 선택진료이외
            //초음파
            ssSpread_Sheet.Cells[27, 3].Text = string.Format("{0:#,###}", RPG[18].Amt5); //급여 본인
            ssSpread_Sheet.Cells[27, 5].Text = string.Format("{0:#,###}", RPG[18].Amt6); //급여 공단
            ssSpread_Sheet.Cells[27, 6].Text = string.Format("{0:#,###}", RPG[18].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[27, 8].Text = string.Format("{0:#,###}", RPG[18].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[27, 10].Text = string.Format("{0:#,###}", RPG[18].Amt2); //비급여 선택진료이외
            //보철/교정료
            ssSpread_Sheet.Cells[28, 3].Text = string.Format("{0:#,###}", RPG[19].Amt5); //급여 본인
            ssSpread_Sheet.Cells[28, 5].Text = string.Format("{0:#,###}", RPG[19].Amt6); //급여 공단
            ssSpread_Sheet.Cells[28, 6].Text = string.Format("{0:#,###}", RPG[19].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[28, 8].Text = string.Format("{0:#,###}", RPG[19].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[28, 10].Text = string.Format("{0:#,###}", RPG[19].Amt2); //비급여 선택진료이외
            //예약진찰료
            ssSpread_Sheet.Cells[29, 3].Text = string.Format("{0:#,###}", RPG[22].Amt5); //급여 본인
            ssSpread_Sheet.Cells[29, 5].Text = string.Format("{0:#,###}", RPG[22].Amt6); //급여 공단
            ssSpread_Sheet.Cells[29, 6].Text = string.Format("{0:#,###}", RPG[22].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[29, 8].Text = string.Format("{0:#,###}", RPG[22].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[29, 10].Text = string.Format("{0:#,###}", RPG[22].Amt2); //비급여 선택진료이외
            //증명료
            ssSpread_Sheet.Cells[30, 3].Text = string.Format("{0:#,###}", RPG[21].Amt5); //급여 본인
            ssSpread_Sheet.Cells[30, 5].Text = string.Format("{0:#,###}", RPG[21].Amt6); //급여 공단
            ssSpread_Sheet.Cells[30, 6].Text = string.Format("{0:#,###}", RPG[21].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[30, 8].Text = string.Format("{0:#,###}", RPG[21].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[30, 10].Text = string.Format("{0:#,###}", RPG[21].Amt2); //비급여 선택진료이외

            if (ArgGwa == "R6") { nPAmt[16, 2] = nPAmt[24, 1]; }

            if (RPG[29].Amt2 > 0 || RPG[29].Amt3 > 0 || RPG[29].Amt4 > 0 || RPG[29].Amt5 > 0 || RPG[29].Amt6 > 0)
            {
                if (nPAmt[16, 1] > 0)
                    ssSpread_Sheet.Cells[32 + nCntEtc1, 0].Text = "응급관리료/기타";
                else
                    ssSpread_Sheet.Cells[32 + nCntEtc1, 0].Text = "기           타";

                ssSpread_Sheet.Cells[32 + nCntEtc1, 3].Text = string.Format("{0:#,###}", RPG[29].Amt5); //급여 본인
                ssSpread_Sheet.Cells[32 + nCntEtc1, 5].Text = string.Format("{0:#,###}", RPG[29].Amt6); //급여 공단
                ssSpread_Sheet.Cells[32 + nCntEtc1, 6].Text = string.Format("{0:#,###}", RPG[29].Amt4); //급여 전액부담
                ssSpread_Sheet.Cells[32 + nCntEtc1, 8].Text = string.Format("{0:#,###}", RPG[29].Amt3); //비급여 선택진료
                ssSpread_Sheet.Cells[32 + nCntEtc1, 10].Text = string.Format("{0:#,###}", RPG[29].Amt2); //비급여 선택진료이외

                nCntEtc1 += 1;
            }

            //선별급여
            if (RPG[49].Amt7 > 0)
            {
                ssSpread_Sheet.Cells[32 + nCntEtc1, 0].Text = "선 별 급 여";

                ssSpread_Sheet.Cells[32 + nCntEtc1, 3].Text = string.Format("{0:#,###}", RPG[49].Amt9); //급여 본인
                ssSpread_Sheet.Cells[32 + nCntEtc1, 5].Text = string.Format("{0:#,###}", RPG[49].Amt8); //급여 공단
                ssSpread_Sheet.Cells[32 + nCntEtc1, 6].Text = ""; //급여 전액부담
                ssSpread_Sheet.Cells[32 + nCntEtc1, 8].Text = ""; //비급여 선택진료
                ssSpread_Sheet.Cells[32 + nCntEtc1, 10].Text = ""; //비급여 선택진료이외

                nCntEtc1 += 1;
            }
            
            if (VB.Left(ArgRdate.Trim(), 10) != "" && clsAuto.GstrAREquipUse == "OK")
            {
                ssSpread_Sheet.Cells[22 + FnPo, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[22 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[22 + FnPo, 12].Font = new Font("굴림", 11, FontStyle.Bold);
                ssSpread_Sheet.Cells[22 + FnPo, 12].Text = "*** 예 약 증 ***";
                ssSpread_Sheet.Cells[23 + FnPo, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[23 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[23 + FnPo, 12].Font = new Font("굴림", 11, FontStyle.Bold);
                ssSpread_Sheet.Cells[23 + FnPo, 12].Text = VB.Left(ArgRdate, 4) + "." + VB.Mid(ArgRdate, 6, 2) + "." + VB.Mid(ArgRdate, 9, 2) + "(" + VB.Left(CF.READ_YOIL(pDbCon, VB.Left(ArgRdate, 10)), 1) + ") " + VB.Right(ArgRdate.Trim(), 5);
                ssSpread_Sheet.Cells[24 + FnPo, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[24 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[24 + FnPo, 12].Font = new Font("굴림", 11, FontStyle.Bold);
                ssSpread_Sheet.Cells[24 + FnPo, 12].Text = ArgGwa + ":" + CF.READ_DrName(pDbCon, ArgDr);
                ssSpread_Sheet.Cells[25 + FnPo, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[25 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[25 + FnPo, 12].Font = new Font("굴림", 11, FontStyle.Bold);
                ssSpread_Sheet.Cells[25 + FnPo, 12].Text = "진료과목:" + CPF.Get_Dept_TelNo(pDbCon, ArgDept, clsPmpaType.TOM.DrCode);
                ssSpread_Sheet.Cells[26 + FnPo, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[26 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[26 + FnPo, 12].Font = new Font("굴림", 11, FontStyle.Bold);
                ssSpread_Sheet.Cells[26 + FnPo, 12].Text = "등록번호:" + ArgPtno;
                ssSpread_Sheet.Cells[27 + FnPo, 12].ColumnSpan = 4;
                ssSpread_Sheet.Cells[27 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[27 + FnPo, 12].Font = new Font("굴림", 11, FontStyle.Bold);
                ssSpread_Sheet.Cells[27 + FnPo, 12].Text = "수진자명:" + ArgName;
            }
            else
            {
                if (VB.Left(ArgRdate.Trim(), 10) != "")
                {
                    ssSpread_Sheet.Cells[34, 3].ColumnSpan = 8;
                    ssSpread_Sheet.Cells[34, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[34, 3].Text = "<예약>" + VB.Space(3) + string.Format("{0:yyyy년 MM월 dd일}", VB.Left(ArgRdate.Trim(), 10)) + VB.Space(1) + string.Format("{0:HH시 MM분}", VB.Mid(ArgRdate, 11, 5)) + VB.Space(3) + ArgGwa + VB.Space(3) + CF.READ_DrName(pDbCon, ArgDr);
                }
            }

            if (clsPmpaPb.GstrPCLR == "OK")
            {
                ssSpread_Sheet.Cells[22 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[22 + FnPo, 12].Text = "★환자복 반납시 보증금을 ";
                ssSpread_Sheet.Cells[23 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[23 + FnPo, 12].Text = "환불 받을 수 있습니다.";
            }

            //합계
            ssSpread_Sheet.Cells[35, 3].Text = string.Format("{0:#,###}", RPG[35].Amt1); //급여 본인
            ssSpread_Sheet.Cells[35, 5].Text = string.Format("{0:#,###}", RPG[36].Amt1); //급여 공단
            ssSpread_Sheet.Cells[35, 6].Text = string.Format("{0:#,###}", RPG[37].Amt1); //급여 전액부담
            ssSpread_Sheet.Cells[35, 8].Text = string.Format("{0:#,###}", RPG[38].Amt1); //비급여 선택진료
            ssSpread_Sheet.Cells[35, 10].Text = string.Format("{0:#,###}", RPG[39].Amt1); //비급여 선택진료이외

            //<금액산정내용>***************************************************************************************************************
            //진료비총액
            ssSpread_Sheet.Cells[7, 13].Text = string.Format("{0:#,###}", RPG[30].Amt1);

            //환자부담총액 = 환자부담총액 - 예약비
            if (Card.GnOgAmt == 0)
                ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", nPAmt[21, 1] - nAmt8); 
            else
            {
                if (nPAmt[21, 1] < Card.GnOgAmt)
                    //산전카드금액이 환자부담금보다 크면 같게함
                    ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", nPAmt[21, 1] - nAmt8 - nPAmt[21, 1]);
                else
                    //산전카드금액이 있으면
                    ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", nPAmt[21, 1] - nAmt8 - Card.GnOgAmt);
            }

            //부가세
            if (nTaxAmt != 0)
            {
                ssSpread_Sheet.Cells[11 + nCntEtc2, 11].ColumnSpan = 5;
                ssSpread_Sheet.Cells[11 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[11 + nCntEtc2, 11].Text = "부가세 : " + string.Format("{0:#,###}", nTaxAmt);
                nCntEtc2 += 1;
            }

            //납부할금액
            if (clsPmpaPb.GOpd_Sunap_Boamt != 0)
                ssSpread_Sheet.Cells[13, 13].Text = string.Format("{0:#,###}", nPAmt[24, 1] + nTaxAmt - clsPmpaPb.GOpd_Sunap_Boamt);
            else
            {
                if (clsPmpaPb.GnPtVoucherAmt != 0)
                    ssSpread_Sheet.Cells[13, 13].Text = string.Format("{0:#,###}", nPAmt[24, 1] + nTaxAmt - clsPmpaPb.GnPtVoucherAmt);
                else
                    ssSpread_Sheet.Cells[13, 13].Text = string.Format("{0:#,###}", nPAmt[24, 1] + nTaxAmt);
            }

            //금액산정부분 예외
            if (nPAmt[22, 1] > 0)
            {
                ssSpread_Sheet.Cells[11 + nCntEtc2, 11].ColumnSpan = 5;
                ssSpread_Sheet.Cells[11 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[11 + nCntEtc2, 11].Text = "감액 : " + string.Format("{0:#,###}", nPAmt[22, 1]);

                if ((clsPmpaType.TOM.GbGameK != "00" && clsPmpaType.TOM.GbGameK != "") || (clsPmpaType.TOM.GbGameKC != "50" && clsPmpaType.TOM.GbGameKC != ""))
                {
                    if (clsPmpaType.TOM.GbGameK != "00")
                    {
                        ssSpread_Sheet.Cells[11 + nCntEtc2, 11].ColumnSpan = 5;
                        ssSpread_Sheet.Cells[11 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                        ssSpread_Sheet.Cells[11 + nCntEtc2, 11].Text = "감액 : " + string.Format("{0:#,###}", nPAmt[22, 1]) + "(" + CF.Read_Bcode_Name(pDbCon, "BAS_감액코드명", clsPmpaType.TOM.GbGameK) + ")";
                    }
                    else if (clsPmpaType.TOM.GbGameK != "50")
                    {
                        ssSpread_Sheet.Cells[11 + nCntEtc2, 11].ColumnSpan = 5;
                        ssSpread_Sheet.Cells[11 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                        ssSpread_Sheet.Cells[11 + nCntEtc2, 11].Text = "감액 : " + string.Format("{0:#,###}", nPAmt[22, 1]) + "(" + CF.Read_Bcode_Name(pDbCon, "BAS_감액코드명", clsPmpaType.TOM.GbGameKC) + ")";
                    }
                }

                nCntEtc2 += 1;
            }

            if (Card.GnOgAmt != 0 && ArgDept == "OG")
            {
                ssSpread_Sheet.Cells[11 + nCntEtc2, 11].ColumnSpan = 5;
                ssSpread_Sheet.Cells[11 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[11 + nCntEtc2, 11].Text = "고운맘금액:" + string.Format("{0:#,###}", Card.GnOgAmt) + ", 잔액:" + string.Format("{0:#,###}", Card.GnOgJanAmt);
                nCntEtc2 += 1;
            }

            if (RPG[34].Amt1 > 0)
            {
                ssSpread_Sheet.Cells[11 + nCntEtc2, 11].ColumnSpan = 5;
                ssSpread_Sheet.Cells[11 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[11 + nCntEtc2, 11].Text = "미수금 : " + string.Format("{0:#,###}", RPG[34].Amt1);
                nCntEtc2 += 1;
            }

            if (clsPmpaType.TOM.Bi == "21")
            {
                if (clsPmpaType.BAT.M3_GC_Amt != 0)
                {
                    ssSpread_Sheet.Cells[11 + nCntEtc2, 11].ColumnSpan = 5;
                    ssSpread_Sheet.Cells[11 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[11 + nCntEtc2, 11].Text = "(가상계좌)" + string.Format("{0:#,###}", clsPmpaType.BAT.M3_GC_Amt);
                    nCntEtc2 += 1;
                }
                else if (clsPmpaPb.GOpd_Sunap_Boamt != 0)
                {
                    ssSpread_Sheet.Cells[11 + nCntEtc2, 11].ColumnSpan = 5;
                    ssSpread_Sheet.Cells[11 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[11 + nCntEtc2, 11].Text = "(가상계좌)" + string.Format("{0:#,###}", clsPmpaPb.GOpd_Sunap_Boamt);
                    nCntEtc2 += 1;
                }

                if (clsPmpaType.BAT.M3_PregDmndAmt != 0 && ArgDept == "OG")
                {
                    ssSpread_Sheet.Cells[11 + nCntEtc2, 11].ColumnSpan = 5;
                    ssSpread_Sheet.Cells[11 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[11 + nCntEtc2, 11].Text = "(산전가상계좌)" + string.Format("{0:#,###}", clsPmpaType.BAT.M3_PregDmndAmt);
                    nCntEtc2 += 1;
                }
            }

            //물리치료 바우처 금액있을경우
            if (clsPmpaPb.GnPtVoucherAmt != 0)
            {
                ssSpread_Sheet.Cells[11 + nCntEtc2, 11].ColumnSpan = 5;
                ssSpread_Sheet.Cells[11 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssSpread_Sheet.Cells[11 + nCntEtc2, 11].Text = "(바우처)" + string.Format("{0:#,###}", clsPmpaPb.GnPtVoucherAmt);
                nCntEtc2 += 1;
            }

            if (clsPmpaType.RSD.Gubun == "1")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {
                    ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//카드

                    if (clsPmpaPb.GnPtVoucherAmt != 0)
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - (clsPmpaType.RSD.TotAmt * -1) - clsPmpaPb.GnPtVoucherAmt);//현금
                    else
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - (clsPmpaType.RSD.TotAmt * -1));//현금
                }
                else
                {
                    ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//카드

                    if (clsPmpaPb.GnPtVoucherAmt != 0)
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - (clsPmpaType.RSD.TotAmt) - clsPmpaPb.GnPtVoucherAmt);//현금
                    else
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - (clsPmpaType.RSD.TotAmt));//현금
                }
            }
            else if (clsPmpaType.RSD.Gubun == "2")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {
                    ssSpread_Sheet.Cells[16, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//현금영수증

                    if (clsPmpaPb.GnPtVoucherAmt != 0)
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - (clsPmpaType.RSD.TotAmt * -1) - clsPmpaPb.GnPtVoucherAmt);//현금
                    else
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - (clsPmpaType.RSD.TotAmt * -1));//현금

                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[28, 12].Text = "현금승인취소";
                        ssSpread_Sheet.Cells[29, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[32, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
                else
                {
                    ssSpread_Sheet.Cells[16, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//현금영수증

                    if (clsPmpaPb.GnPtVoucherAmt != 0)
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - (clsPmpaType.RSD.TotAmt) - clsPmpaPb.GnPtVoucherAmt);//현금
                    else
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - (clsPmpaType.RSD.TotAmt));//현금

                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[28, 12].Text = "현금승인";
                        ssSpread_Sheet.Cells[29, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[32, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
            }
            else
            {
                if (clsPmpaPb.GnPtVoucherAmt != 0)
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - clsPmpaPb.GnPtVoucherAmt);//현금
                else
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0));//현금
            }

            if (clsPmpaPb.GnPtVoucherAmt != 0)
            {
                ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - clsPmpaPb.GnPtVoucherAmt);//합계
            }
            else
            {
                ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0));//합계
            }

            if (ArgRe == "R" && ArgReDate.Trim() != "")
            {
                ssSpread_Sheet.Cells[40, 0].Text = VB.Space(3) + VB.Left(ArgReDate, 4) + VB.Space(15) + VB.Mid(ArgReDate, 6, 2) + VB.Space(15) + VB.Mid(ArgReDate, 9, 2) + VB.Space(15);
            }
            else
            {
                if (strTimeSS != "")
                    ssSpread_Sheet.Cells[40, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime + ":" + strTimeSS;
                else
                    ssSpread_Sheet.Cells[40, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime;
            }

            //카드영수증 통합
            if (clsPmpaType.RSD.CardSeqNo > 0)
            {
                Card.GstrCardApprov_Info = CC.Card_Sign_Info_Set(pDbCon, clsPmpaType.RSD.CardSeqNo, ArgPtno);

                if (Card.GstrCardApprov_Info != "")
                {
                    ssSpread_Sheet.Cells[29, 13].ColumnSpan = 3;
                    ssSpread_Sheet.Cells[29, 13].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[29, 13].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 24); //거래구분

                    ssSpread_Sheet.Cells[30, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[30, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[30, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 4) + " " + VB.Pstr(Card.GstrCardApprov_Info, "{}", 2); //카드종류/카드번호
                    ssSpread_Sheet.Cells[31, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[31, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[31, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 20); //승인금액
                    ssSpread_Sheet.Cells[32, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[32, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[32, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 12); //승인번호
                    ssSpread_Sheet.Cells[33, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[33, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[33, 12].Text = "72117503"; //가맹점번호
                    ssSpread_Sheet.Cells[34, 12].ColumnSpan = 2;
                    ssSpread_Sheet.Cells[34, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[34, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 14); //거래일시
                    ssSpread_Sheet.Cells[34, 15].Text = VB.Pstr(VB.Pstr(Card.GstrCardApprov_Info, "{}", 8), "개월", 1);
                }

                Card.GstrCardApprov_Info = "";
            }

            clsDP.PrintReciptPrint(ssSpread, ssSpread_Sheet, strPrintName);
        }

        public void Print_IPD_Information_Set(ref SheetView spd, string strCode)
        {

            //case "Y85": strBunName = "가퇴원금"; break;
            //case "Y87": strBunName = "중 간 납"; break;

            if (strCode == "Y87")
            {
                //중간납 안내문구            
                spd.Cells[24, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[24, 3].ColumnSpan = 15;
                spd.Cells[24, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[24, 3].Text = "※ 중간진료비에는 수술 재료대.약제.검사 등";

                spd.Cells[25, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[25, 3].ColumnSpan = 15;
                spd.Cells[25, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[25, 3].Text = "   일부가 누락될 수 있습니다.";

                spd.Cells[26, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[26, 3].ColumnSpan = 15;
                spd.Cells[26, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[26, 3].Text = "※ 중간진료비는 입원한 날로부터 누적되어 발행됩니다.";

                spd.Cells[27, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[27, 3].ColumnSpan = 15;
                spd.Cells[27, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[27, 3].Text = "※ 중간진료비는 심사전 상태이며, 퇴원 시 심사과정을 거쳐";

                spd.Cells[28, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[28, 3].ColumnSpan = 15;
                spd.Cells[28, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[28, 3].Text = "   정확한 계산서가 발행됩니다.";

                spd.Cells[29, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[29, 3].ColumnSpan = 15;
                spd.Cells[29, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[29, 3].Text = "※ 주간 수납은 원무과 퇴원 창구(1층 3번)에서 수납하시고,";

                spd.Cells[30, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[30, 3].ColumnSpan = 15;
                spd.Cells[30, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[30, 3].Text = "   휴일 . 야간 수납은 응급의료센터 옆 응급 수납창구에";

                spd.Cells[31, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[31, 3].ColumnSpan = 15;
                spd.Cells[31, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[31, 3].Text = "   수납하시기 바랍니다.";
            }
            else if(strCode == "Y85")
            {
                //가퇴원 안내문구
                spd.Cells[24, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[24, 3].ColumnSpan = 15;
                spd.Cells[24, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[24, 3].Text = "◎ 퇴원일로부터 7일(공휴일 제외) 이내 ";

                spd.Cells[25, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[25, 3].ColumnSpan = 15;
                spd.Cells[25, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[25, 3].Text = "   원무팀 퇴원수납 3번창구에서 정산 받으시길 바랍니다.";

                spd.Cells[26, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[26, 3].ColumnSpan = 15;
                spd.Cells[26, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[26, 3].Text = "◎ 진료비를 정산하실 때에는 퇴원 당시 발급받으신 보관증을";

                spd.Cells[27, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[27, 3].ColumnSpan = 15;
                spd.Cells[27, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[27, 3].Text = "    지참하시기 바라며, ";

                spd.Cells[28, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[28, 3].ColumnSpan = 15;
                spd.Cells[28, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[28, 3].Text = "    환불금 발생 시는 정산 당일 환불하여 드리겠습니다.";

                spd.Cells[29, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[29, 3].ColumnSpan = 15;
                spd.Cells[29, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[29, 3].Text = "◎ 진료비 정산 시간은 다음과 같습니다.";

                spd.Cells[30, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[30, 3].ColumnSpan = 15;
                spd.Cells[30, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[30, 3].Text = "    평일 : 09:00~16:30 토요일: 09:00~12:00";

                spd.Cells[31, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[31, 3].ColumnSpan = 15;
                spd.Cells[31, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[31, 3].Text = "◎ 가퇴원에 대한 문의가 있을 경우";

                spd.Cells[32, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[32, 3].ColumnSpan = 15;
                spd.Cells[32, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[32, 3].Text = "    ☎ 054) 260 - 8107번으로 문의하십시오.";

                spd.Cells[33, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[33, 3].ColumnSpan = 15;
                spd.Cells[33, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[33, 3].Text = "◎ 카드로 결제하였을 시 카드 취소 후 재결제 해야 하므로";

                spd.Cells[34, 3].Font = new Font("맑은 고딕", 10, FontStyle.Bold);
                spd.Cells[34, 3].ColumnSpan = 15;
                spd.Cells[34, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                spd.Cells[34, 3].Text = "    해당 카드를 꼭 지참하십시오.";
            }

        }

        public void Report_Print_Sunap_A4_New(PsmhDb pDbCon, long ArgOpdNo, string ArgPtno, string ArgGwa, string ArgName, string ArgRetn, int ArgSeq, string ArgRdate, string ArgDr, string ArgBi, string ArgBDate, string ArgCardBun, string ArgSunap, string ArgDept, PictureBox Pb, string ArgIpdDtl, string ArgSpc, string ArgGelCode, string ArgMcode, string ArgVcode, string ArgJin, string ArgJinDtl, FpSpread o, string ArgRe = "", string ArgReDate = "")
        {
            FarPoint.Win.Spread.FpSpread ssSpread = null;
            FarPoint.Win.Spread.SheetView ssSpread_Sheet = null;

            ComFunc CF = new ComFunc();
            clsPmpaFunc CPF = new ComPmpaLibB.clsPmpaFunc();
            clsOumsadChk COC = new ComPmpaLibB.clsOumsadChk();
            clsDrugPrint clsDP = new clsDrugPrint();
            PrintDocument pd = new PrintDocument();
            PageSettings ps = new PageSettings();
            PrintController pc = new StandardPrintController();
            clsOumsad CPO = new clsOumsad();
            clsBasAcct CBA = new ComPmpaLibB.clsBasAcct();

            DataTable DtP = new DataTable();
            DataTable DtSub = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            

            string strPrintName = "";

            int nSelf = 0;
            int nSeqNo = 0;
            string strYdate = "";
            string strTimeSS = "";
            long[,] nPAmt = new long[29, 6];
            double nTaxAmt = 0;
            long nTaxDanAmt = 0;
            long nAmt8 = 0;
            string strMsgChk = "";
            int nCntEtc1 = 0;//계정별기타
            int nCntEtc2 = 0;//정산별기타
            int j = 0;

            Argument_Table RPA = new Argument_Table();
            Report_Add_Amt_Table[] RPG = new ComPmpaLibB.clsPmpaPrint.Report_Add_Amt_Table[50];
            Slip_ReportPrint_Table[] RP = new ComPmpaLibB.clsPmpaPrint.Slip_ReportPrint_Table[999];

            clsDP.SetReciptPrint_New(ref ssSpread, ref ssSpread_Sheet);
            clsDP.ClearReciptPrint_New(ssSpread, ssSpread_Sheet);

            //예약조정 대상자 체크
            strYdate = COC.Check_Reserved_Adjust(pDbCon, ArgPtno, ArgBDate, ArgDept);

            #region //수납순번 가져오기
            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(MAX(SEQNO), 0) SEQNO FROM " + ComNum.DB_PMPA + "OPD_SUNAP ";
            SQL += ComNum.VBLF + "  WHERE ACTDATE   = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND PART      = '" + clsType.User.IdNumber + "' ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            if (DtP.Rows.Count > 0)
                nSeqNo = Convert.ToInt32(DtP.Rows[0]["SEQNO"].ToString());

            if (nSeqNo == 0)
                nSeqNo = 1;
            else
                nSeqNo += 1;

            DtP.Dispose();
            DtP = null;
            #endregion

            #region //수납시간 초
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS') Sdate FROM DUAL ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            if (DtP.Rows.Count > 0)
                strTimeSS = VB.Right(DtP.Rows[0]["Sdate"].ToString(), 2);

            DtP.Dispose();
            DtP = null;
            #endregion

            #region //Read_Opd_Slip

            for (int i = 0; i <= 28; i++)
            {
                for (j = 0; j <= 5; j++)
                    nPAmt[i, j] = 0;
            }

            Card.GnOgAmt = 0;

            //환자정보읽기
            CPO.READ_BAS_PATIENT(pDbCon, ArgPtno);
            CPO.READ_OPD_MASTER(pDbCon, ArgPtno, ArgDept, "", ArgBDate);

            //변수 clear
            Report_Print_Clear(ref RPA, ref RPG, ref RP);

            //영수증 재발행
            if (clsPmpaPb.GstrJeaPrint == "OK")
            {
                if (clsPmpaPb.GstrJeaEtcPrint != "OK") //기타수납 재발행 아닐경우
                {
                    clsPmpaPb.GstrERStat = "";

                    if (COC.Check_ER_AC101_Slip(pDbCon, ArgPtno, clsPmpaPb.GstrJeaDate, clsPmpaPb.GstrJeaDept, clsPmpaPb.GnJeaSeqNo, clsPmpaPb.GstrJeaBi))
                        clsPmpaPb.GstrERStat = "OK";

                    //환의수납여부
                    clsPmpaPb.GstrPCLR = "";
                    if (clsPmpaPb.GstrJeaDept == "ER")
                    {
                        if (COC.Check_ER_PCLR_Slip(pDbCon, ArgPtno, clsPmpaPb.GstrJeaDate, clsPmpaPb.GstrJeaDept, clsPmpaPb.GnJeaSeqNo, clsPmpaPb.GstrJeaBi, "Y", ""))
                            clsPmpaPb.GstrPCLR = "OK";
                    }

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT BUN, GBSELF, SUM(AMT1) NAMT, ";
                    SQL += ComNum.VBLF + "        SUM(AMT2) NAMT2, SUM(OGAMT) OGAMT, SUM(AMT4) NAMT4, ";
                    SQL += ComNum.VBLF + "        SUM(DanAmt) NDan ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND ActDate   = TO_DATE('" + clsPmpaPb.GstrJeaDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
                    SQL += ComNum.VBLF + "    AND Bi        = '" + clsPmpaPb.GstrJeaBi + "' ";
                    SQL += ComNum.VBLF + "    AND DeptCode  = '" + clsPmpaPb.GstrJeaDept + "' ";
                    SQL += ComNum.VBLF + "    AND SeqNo     =  " + clsPmpaPb.GnJeaSeqNo + " ";
                    SQL += ComNum.VBLF + "    AND PART      = '" + clsPmpaPb.GstrJeaPart + "' ";
                    SQL += ComNum.VBLF + "    AND TRIM(SUNEXT) NOT IN (SELECT CODE ";
                    SQL += ComNum.VBLF + "                               FROM ADMIN.BAS_BCODE ";
                    SQL += ComNum.VBLF + "                              WHERE GUBUN ='원무영수제외코드') "; //저가약제 제외코드 2010-11-22
                    SQL += ComNum.VBLF + "  GROUP BY BUN, GBSELF ";
                    SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtP.Dispose();
                        DtP = null;
                        return;
                    }
                }
                else
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT BUN, GBSELF, SUM(AMT) NAMT, ";
                    SQL += ComNum.VBLF + "        0 NAMT2, 0 OGAMT, 0 NAMT4, 0 NDan ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_ETCSLIP ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND ActDate   = TO_DATE('" + clsPmpaPb.GstrJeaDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND Pano      = '" + ArgPtno + "' ";
                    SQL += ComNum.VBLF + "    AND Bi        = '" + clsPmpaPb.GstrJeaBi + "' ";
                    SQL += ComNum.VBLF + "    AND Sunext NOT IN  ( 'Y96C' )  ";
                    SQL += ComNum.VBLF + "    AND SeqNo     =  " + clsPmpaPb.GnJeaSeqNo + " ";
                    SQL += ComNum.VBLF + "  GROUP BY BUN, GBSELF ";
                    SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtP.Dispose();
                        DtP = null;
                        return;
                    }
                }
            }
            else
            {
                //수탁검사
                if (ArgPtno.Trim().Length == 13)
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT BUN, GBSELF, SUM(AMT) NAMT, ";
                    SQL += ComNum.VBLF + "        0 NAMT2, 0 OGAMT, 0 NAMT4, 0 NDan  ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_ETCSLIP ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND ACTDATE   = TRUNC(SYSDATE)   ";
                    SQL += ComNum.VBLF + "    AND JUMIN1    = '" + VB.Left(ArgPtno.Trim(), 6) + "' ";
                    //2018.06.21 박병규 : OPD_ETCSLIP에 주민번호 뒷자리는 암화가 안되어있음.
                    //SQL += ComNum.VBLF + "    AND JUMIN3    = '" + VB.Right(clsAES.AES(ArgPtno.Trim()), 7) + "' ";  //주민암호화
                    SQL += ComNum.VBLF + "    AND SEQNO     = '" + ArgSeq + "' ";
                    SQL += ComNum.VBLF + "    AND PART      = '" + clsType.User.JobPart + "'";
                    SQL += ComNum.VBLF + "  GROUP BY BUN, GBSELF ";
                    SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtP.Dispose();
                        DtP = null;
                        return;
                    }
                }
                else if (ArgGwa.Trim() == "II" || ArgGwa.Trim() == "재원자외래수납")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT BUN, GBSELF, SUM(AMT1) NAMT, ";
                    SQL += ComNum.VBLF + "        SUM(AMT2) NAMT2, SUM(OGAMT) OGAMT, SUM(AMT4) NAMT4, ";
                    SQL += ComNum.VBLF + "        SUM(DanAmt) NDan ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND ACTDATE   = TRUNC(SYSDATE)   ";
                    SQL += ComNum.VBLF + "    AND WARDCODE  = 'II'  ";
                    SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "'   ";
                    SQL += ComNum.VBLF + "    AND SEQNO     = '" + ArgSeq + "' ";
                    SQL += ComNum.VBLF + "    AND PART      = '" + clsType.User.JobPart + "'";
                    SQL += ComNum.VBLF + "    AND TRIM(SUNEXT) NOT IN (SELECT CODE ";
                    SQL += ComNum.VBLF + "                               FROM ADMIN.BAS_BCODE ";
                    SQL += ComNum.VBLF + "                              WHERE GUBUN ='원무영수제외코드') "; // 저가약제 제외코드 2010-11-22
                    SQL += ComNum.VBLF + "  GROUP BY BUN, GBSELF ";
                    SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtP.Dispose();
                        DtP = null;
                        return;
                    }
                }
                else
                {
                    //환의수납여부
                    clsPmpaPb.GstrPCLR = "";
                    if (ArgDept == "ER")
                    {
                        if (COC.Check_ER_PCLR_Slip(pDbCon, ArgPtno, clsPublic.GstrSysDate, ArgDept.Trim(), ArgSeq, "", "", clsType.User.JobPart))
                            clsPmpaPb.GstrPCLR = "OK";
                    }

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT BUN, GBSELF, SUM(AMT1) NAMT, ";
                    SQL += ComNum.VBLF + "        SUM(AMT2) NAMT2,SUM(OGAMT) OGAMT, SUM(AMT4) NAMT4, ";
                    SQL += ComNum.VBLF + "        SUM(DanAmt) NDan ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND ACTDATE   = TRUNC(SYSDATE)   ";
                    SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "'   ";
                    SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept.Trim() + "' ";
                    SQL += ComNum.VBLF + "    AND SEQNO     = '" + ArgSeq + "' ";
                    SQL += ComNum.VBLF + "    AND PART      = '" + clsType.User.IdNumber + "'";
                    SQL += ComNum.VBLF + "    AND TRIM(SUNEXT) NOT IN (SELECT CODE ";
                    SQL += ComNum.VBLF + "                               FROM ADMIN.BAS_BCODE ";
                    SQL += ComNum.VBLF + "                              WHERE GUBUN ='원무영수제외코드') "; // 저가약제 제외코드 2010-11-22
                    SQL += ComNum.VBLF + "  GROUP BY BUN, GBSELF ";
                    SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtP.Dispose();
                        DtP = null;
                        return;
                    }
                }
            }

            nTaxAmt = 0;
            nTaxDanAmt = 0;

            string strBun = "";

            for (int i = 0; i < DtP.Rows.Count; i++)
            {
                if (DtP.Rows[i]["GBSELF"].ToString().Trim() == "0")
                    nSelf = 1;
                else
                    nSelf = 2;

                strBun = DtP.Rows[i]["BUN"].ToString().Trim();

                if (strBun == "97")
                {
                    nTaxAmt += Convert.ToDouble(VB.Val(DtP.Rows[i]["NAMT4"].ToString()));
                    nTaxDanAmt += Convert.ToInt64(VB.Val(DtP.Rows[i]["NDan"].ToString()));
                }

                if (strBun == "99" || strBun == "98" || strBun == "92" || strBun == "96")
                    nPAmt[CBA.Rtn_Reception_Report(strBun), 1] += Convert.ToInt64(VB.Val(DtP.Rows[i]["NAMT"].ToString()));
                else
                    nPAmt[CBA.Rtn_Reception_Report(strBun), nSelf] += Convert.ToInt64(VB.Val(DtP.Rows[i]["NAMT"].ToString()));

                if (string.Compare(strBun, "84") <= 0)
                    nPAmt[17, nSelf] += Convert.ToInt64(VB.Val(DtP.Rows[i]["NAMT"].ToString())); //합계

                //산전승인금액
                if (strBun == "99" && Convert.ToInt64(VB.Val(DtP.Rows[i]["OGAMT"].ToString())) != 0)
                    Card.GnOgAmt += Convert.ToInt64(VB.Val(DtP.Rows[i]["OGAMT"].ToString()));
            }

            //예약비처리
            if (ArgRdate.Trim() != "")
            {
                nAmt8 = 0;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT AMT1, AMT2, AMT4, ";
                SQL += ComNum.VBLF + "        AMT5, AMT6, AMT7, ";
                SQL += ComNum.VBLF + "        AMT8, GbSPC  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_RESERVED_NEW  ";
                SQL += ComNum.VBLF + "  WHERE PANO      = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "    AND DATE1     = TRUNC(SYSDATE)  ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept.Trim() + "'  ";
                SQL += ComNum.VBLF + "    AND BI        = '" + ArgBi + "' ";
                SQL += ComNum.VBLF + "    AND DATE3     = TO_DATE('" + ArgRdate + "','YYYY-MM-DD HH24:MI')    ";
                SQL += ComNum.VBLF + "    AND RETDATE IS NULL ";
                SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtSub.Dispose();
                    DtSub = null;
                    DtP.Dispose();
                    DtP = null;
                    return;
                }

                if (DtSub.Rows.Count > 0)
                {
                    RPG[22].Amt1 = Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT1"].ToString()));
                    RPG[22].Amt3 = Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT2"].ToString()));
                    //2018-09-14 변경
                    //RPG[22].Amt5 = Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT7"].ToString())) + Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT6"].ToString()))  + Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT5"].ToString())) - Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT2"].ToString()));
                    RPG[22].Amt5 = Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT7"].ToString())) + Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT5"].ToString())) - Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT2"].ToString()));
                    RPG[22].Amt6 = Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT4"].ToString()));

                    nPAmt[1, 1] = Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT1"].ToString()));
                    nPAmt[17, 1] += Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT1"].ToString()));
                    nPAmt[19, 1] += Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT4"].ToString()));
                    nPAmt[24, 1] += Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT7"].ToString()));
                    nPAmt[22, 1] += Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT5"].ToString()));
                    nPAmt[23, 1] += Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT6"].ToString()));
                    nAmt8 = Convert.ToInt64(VB.Val(DtSub.Rows[0]["AMT8"].ToString()));
                }

                DtSub.Dispose();
                DtSub = null;
            }

            if (clsPmpaPb.GstrJinGubun == "E" || clsPmpaPb.GstrJinGubun == "5" || clsPmpaPb.GstrJinGubun == "8" || clsPmpaPb.GstrJinGubun == "G" || clsPmpaPb.GstrJinGubun == "U" || clsPmpaPb.GstrJinGubun == "T")
            {
                RPG[40].Amt1 += Convert.ToInt64(clsPmpaPb.gnJinAMT1T);
                RPG[40].Amt3 += Convert.ToInt64(clsPmpaPb.gnJinAMT2T);
                RPG[40].Amt5 += Convert.ToInt64(clsPmpaPb.gnJinAMT7T + clsPmpaPb.gnJinAMT5T - clsPmpaPb.gnJinAMT2T);
                RPG[40].Amt6 += Convert.ToInt64(clsPmpaPb.gnJinAMT4T);

                nPAmt[1, 1] += Convert.ToInt64(clsPmpaPb.gnJinAMT1T);
                nPAmt[17, 1] += Convert.ToInt64(clsPmpaPb.gnJinAMT1T);
                nPAmt[19, 1] += Convert.ToInt64(clsPmpaPb.gnJinAMT4T);
                nPAmt[24, 1] += Convert.ToInt64(clsPmpaPb.gnJinAMT7T);
                nPAmt[22, 1] += Convert.ToInt64(clsPmpaPb.gnJinAMT5T);
                nPAmt[23, 1] += Convert.ToInt64(clsPmpaPb.gnJinAMT6T);

                nPAmt[17, 2] += Convert.ToInt64(clsPmpaPb.gnJinAMT2T);
            }

            if (ArgBi == "52" && nPAmt[17, 2] > 0 && clsPmpaPb.GstrSelUse == "OK")
            {
                nPAmt[20, 1] = nPAmt[17, 1] + nPAmt[17, 2];                 //진료비총액
                nPAmt[18, 1] = nPAmt[17, 1] - nPAmt[19, 1] + nPAmt[17, 2];  //급여본인부담금
                nPAmt[21, 1] = nPAmt[18, 1];                                //환자부담총액
            }
            else
            {
                nPAmt[20, 1] = nPAmt[17, 1] + nPAmt[17, 2];                 //진료비총액
                nPAmt[18, 1] = nPAmt[17, 1] - nPAmt[19, 1];                 //급여본인부담금
                nPAmt[21, 1] = nPAmt[18, 1] + nPAmt[17, 2];                 //환자부담총액
            }

            nPAmt[25, 1] = nPAmt[21, 1] - nPAmt[22, 1] - nPAmt[23, 1];      //수납금액

            //영수증인쇄시 인공신장 급여본인부담금이 - 금액이면 0 으로 표시
            if ((nPAmt[18, 1] < 0 && clsPmpaPb.GstrJeaDept == "HD") || (nPAmt[18, 1] < 0 && nPAmt[18, 1].ToString().Length < 3))
            {
                nPAmt[18, 1] = nPAmt[18, 1] * -1;

                if (nPAmt[18, 1] < 10)
                    nPAmt[18, 1] = 0;
                else
                    nPAmt[18, 1] = nPAmt[18, 1] * -1;
            }

            DtP.Dispose();
            DtP = null;

            //마지막 절사액 읽기
            if (clsPmpaPb.GstrJeaPrint == "OK")
                CPF.OPD_SUNAP_Last_Info(pDbCon, ArgPtno, clsPmpaPb.GstrJeaDate, clsPmpaType.TOM.BDate, ArgDept, clsPmpaPb.GstrJeaPart.Trim(), ArgSeq, "", ArgBi, "", "");
            else
            {
                if (clsPmpaType.TOM.MCode == "E000" || clsPmpaType.TOM.MCode == "F000" || clsPmpaType.TOM.GbDementia == "Y" || (clsPmpaPb.GstrResExamCHK == "OK" && clsPmpaPb.GstrResExamFlag == "OK"))
                    clsPmpaPb.GnOpd_Sunap_LastDan = 0;
                else
                    clsPmpaPb.GnOpd_Sunap_LastDan = clsPmpaPb.GnBTruncLastAmt;

                clsPmpaPb.GOpd_Sunap_GelCode = clsPmpaType.TOM.GelCode;
                clsPmpaPb.GOpd_Sunap_MCode = clsPmpaType.TOM.MCode;
                clsPmpaPb.GOpd_Sunap_VCode = clsPmpaType.TOM.VCode;
                clsPmpaPb.GOpd_Sunap_Jin = clsPmpaType.TOM.Jin;
                clsPmpaPb.GOpd_Sunap_JinDtl = clsPmpaType.TOM.JinDtl;
                clsPmpaPb.GOpd_Sunap_II = ArgGwa;

                clsPmpaPb.GOpd_Sunap_Boamt = clsPmpaType.BAT.M3_GC_Amt;
                clsPmpaPb.GOpd_Sunap_EFamt = clsPmpaPb.GnBoninAmt_EF;
            }

            //II과이면서 자보는 일반수가를 적용함
            if (ArgDept == "II" && clsPmpaType.TOM.Bi == "52")
                clsPmpaType.TOM.Bi = "51";

            clsPmpaType.a.Date = ArgBDate;
            clsPmpaType.a.Dept = ArgDept;
            clsPmpaType.a.Sex = clsPmpaType.TOM.Sex;
            clsPmpaType.a.GbGameK = clsPmpaType.TOM.GbGameK;
            clsPmpaType.a.GbGameKC = clsPmpaType.TOM.GbGameKC;
            clsPmpaType.a.Retn = 0;
            clsPmpaType.a.Bi = Convert.ToInt32(VB.Val(clsPmpaType.TOM.Bi));

            if (clsPmpaType.a.Bi != 0)
                clsPmpaType.a.Bi1 = Convert.ToInt32(VB.Val(VB.Left(clsPmpaType.TOM.Bi, 1)));
            else
                clsPmpaType.a.Bi1 = 0;

            if (clsPmpaType.a.Bi == 52 || clsPmpaType.a.Bi == 55)
                clsPmpaType.a.Bi1 = 6; //자보

            if (COC.CHK_SANID_GASAN(clsDB.DbCon, clsPmpaType.TOM.Pano, clsPmpaType.TOM.Bi, clsPmpaType.TOM.DrCode, clsPmpaType.TOM.BDate) == true && clsPmpaPb.GstrSanAdd_NOT == "") { clsPmpaType.a.Bi1 = 7; } //자보

            clsPmpaType.a.Age = clsPmpaType.TOM.Age;
            clsPmpaType.a.AgeiLsu = 99;

            if (clsPmpaType.a.Age == 0)
                clsPmpaType.a.AgeiLsu = CF.DATE_ILSU(pDbCon, clsPmpaType.a.Date, "20" + VB.Left(clsPmpaType.TBP.Jumin1, 2) + "-" + VB.Mid(clsPmpaType.TBP.Jumin1, 3, 2) + VB.Right(clsPmpaType.TBP.Jumin1, 2));

            clsPmpaType.a.Gbilban2 = clsPmpaType.TOM.Gbilban2;
            clsPmpaType.a.Pano = clsPmpaType.TOM.Pano;
            clsPmpaType.a.DrCode = clsPmpaType.TOM.DrCode;
            clsPmpaType.a.GbSpc = clsPmpaType.TOM.GbSpc;

            if (clsPmpaPb.GOpd_Sunap_GelCode != "") { clsPmpaType.TOM.GelCode = clsPmpaPb.GOpd_Sunap_GelCode; }
            if (clsPmpaPb.GOpd_Sunap_MCode != "") { clsPmpaType.TOM.MCode = clsPmpaPb.GOpd_Sunap_MCode; }
            if (clsPmpaPb.GOpd_Sunap_VCode != "") { clsPmpaType.TOM.VCode = clsPmpaPb.GOpd_Sunap_VCode; }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.DeptCode,a.SuCode,a.SuNext, ";
            SQL += ComNum.VBLF + "        a.Bi,a.Bun,a.Nu, ";
            SQL += ComNum.VBLF + "        a.Qty,a.Nal,a.BaseAmt, ";
            SQL += ComNum.VBLF + "        a.GbSugbS, a.GbSpc,a.GbNgt, ";
            SQL += ComNum.VBLF + "        a.GbGisul,a.GbSelf,a.GbChild, ";
            SQL += ComNum.VBLF + "        a.DrCode,a.WardCode, a.GbSlip, ";
            SQL += ComNum.VBLF + "        a.GbHost,a.Amt1,a.Amt2, ";
            SQL += ComNum.VBLF + "        a.Jamt,a.Bamt,a.OrderNo, ";
            SQL += ComNum.VBLF + "        a.GbImiv,a.GbBunup, ";
            SQL += ComNum.VBLF + "        TO_CHAR(a.BDate,'YYYY-MM-DD') BDate, ";
            SQL += ComNum.VBLF + "        a.GbIpd, a.CardSeqNo,a.DosCode, ";
            SQL += ComNum.VBLF + "        a.ABCDATE,a.OPER_DEPT,a.OPER_DCT, ";
            SQL += ComNum.VBLF + "        a.CARDSEQNO,a.OgAmt,a.DanAmt, ";
            SQL += ComNum.VBLF + "        a.GbSpc_No, a.MULTI,a.MULTIREMARK, ";
            SQL += ComNum.VBLF + "        a.DIV,a.DUR,a.KSJIN, ";
            SQL += ComNum.VBLF + "        a.SCODESAYU,a.SCODEREMARK,a.Amt4 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP a, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_SUN b ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND a.Sunext  = b.Sunext(+) ";
            SQL += ComNum.VBLF + "    AND a.Pano    = '" + ArgPtno + "' ";

            if (ArgDept == "II" && clsPmpaPb.GstrllDept != "")
                SQL += ComNum.VBLF + "AND a.DeptCode = '" + clsPmpaPb.GstrllDept + "' ";
            else
                SQL += ComNum.VBLF + "AND a.DeptCode = '" + ArgDept + "' ";

            if (clsPmpaPb.GstrJeaPrint == "OK")
            {
                SQL += ComNum.VBLF + "AND a.ActDate = TO_DATE('" + clsPmpaPb.GstrJeaDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "AND a.Part    = '" + clsPmpaPb.GstrJeaPart.Trim() + "' ";
            }
            else
            {
                SQL += ComNum.VBLF + "AND a.ActDate = TRUNC(SYSDATE) ";
                SQL += ComNum.VBLF + "AND a.Part    = '" + clsType.User.IdNumber + "'  ";
            }

            SQL += ComNum.VBLF + "    AND TRIM(a.SUNEXT) NOT IN (SELECT CODE ";
            SQL += ComNum.VBLF + "                                 FROM ADMIN.BAS_BCODE ";
            SQL += ComNum.VBLF + "                                WHERE GUBUN ='원무영수제외코드') "; // 저가약제 제외코드 2010-11-22
            SQL += ComNum.VBLF + "    AND a.SeqNo   = " + ArgSeq + " ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            for (int i = 0; i < DtP.Rows.Count; i++)
            {
                RP[i].SuCode = DtP.Rows[i]["SUCODE"].ToString().Trim();
                RP[i].SuNext = DtP.Rows[i]["Sunext"].ToString().Trim();
                RP[i].Bi = DtP.Rows[i]["Bi"].ToString().Trim();
                RP[i].BDate = DtP.Rows[i]["BDate"].ToString().Trim();
                RP[i].Bun = DtP.Rows[i]["BUN"].ToString().Trim();
                RP[i].Nu = DtP.Rows[i]["NU"].ToString().Trim();
                RP[i].Qty = Convert.ToDouble(DtP.Rows[i]["QTY"].ToString().Trim());
                RP[i].Nal = Convert.ToInt32(DtP.Rows[i]["NAL"].ToString().Trim());
                RP[i].BaseAmt = Convert.ToInt64(DtP.Rows[i]["BASEAMT"].ToString().Trim());
                RP[i].GbSpc = DtP.Rows[i]["GBSPC"].ToString().Trim();
                RP[i].GbNgt = DtP.Rows[i]["GBNGT"].ToString().Trim();
                RP[i].GbGisul = DtP.Rows[i]["GBGISUL"].ToString().Trim();
                RP[i].SugbS = DtP.Rows[i]["GBSUGBS"].ToString().Trim();
                RP[i].GbSelf = DtP.Rows[i]["GBSELF"].ToString().Trim();

                switch (RP[i].GbSelf)
                {
                    //SugbS : 1.100/100, 4.100/80, 5.100/50, 6.100/80
                    case "0":
                        if (RP[i].SugbS == "2" ||  RP[i].SugbS == "3" ||  RP[i].SugbS == "4" || RP[i].SugbS == "5" || RP[i].SugbS == "6" || RP[i].SugbS == "7" || RP[i].SugbS == "8" || RP[i].SugbS == "9")
                            RP[i].GbSelf = "2";
                        break;

                    case "1":
                        if (RP[i].SugbS == "1")
                            RP[i].GbSelf = "2";
                        break;

                    case "2":
                        if (RP[i].SugbS == "1")
                            RP[i].GbSelf = "2";
                        else
                            RP[i].GbSelf = "1";
                        break;
                }

                RP[i].GbChild = DtP.Rows[i]["GBCHILD"].ToString().Trim();
                RP[i].DrCode = DtP.Rows[i]["DRCODE"].ToString().Trim();
                RP[i].DeptCode = DtP.Rows[i]["DEPTCODE"].ToString().Trim();
                RP[i].WardCode = DtP.Rows[i]["WARDCODE"].ToString().Trim();
                RP[i].GbSlip = DtP.Rows[i]["GBSLIP"].ToString().Trim();
                RP[i].GbHost = DtP.Rows[i]["GBHOST"].ToString().Trim();
                RP[i].Amt1 = Convert.ToInt64(VB.Val(DtP.Rows[i]["AMT1"].ToString().Trim()));
                RP[i].Amt2 = Convert.ToInt64(VB.Val(DtP.Rows[i]["AMT2"].ToString().Trim()));
                RP[i].Amt4 = Convert.ToInt64(VB.Val(DtP.Rows[i]["AMT4"].ToString().Trim()));

                RP[i].OrderNo = Convert.ToInt64(VB.Val(DtP.Rows[i]["ORDERNO"].ToString().Trim()));
                RP[i].GbImiv = DtP.Rows[i]["GBIMIV"].ToString().Trim();
                RP[i].GbBunup = DtP.Rows[i]["GBBUNUP"].ToString().Trim();
                RP[i].DosCode = DtP.Rows[i]["DOSCODE"].ToString().Trim();
                RP[i].GbIPD = DtP.Rows[i]["GBIPD"].ToString().Trim();
                RP[i].KsJin = DtP.Rows[i]["KSJIN"].ToString().Trim();
                RP[i].DanAmt = Convert.ToInt64(VB.Val(DtP.Rows[i]["DANAMT"].ToString().Trim()));
                RP[i].GbSpc_No = DtP.Rows[i]["GbSpc_No"].ToString().Trim();

                if (RP[i].SuNext == "SPEECH1A")
                    RP[i].Bun = "47";
            }

            //영수 금액 읽기
            for (int i = 0; i < 999; i++)
            {
                if (RP[i].SuCode == null || RP[i].SuCode == "") { break; }

                if (RP[i].SuCode.Trim() != "")
                    Report_Slip_AmtAdd(i, ref RP, ref RPG);
            }

            //영수금액계산
            Last_Report_Amt_Gesan(ref RPG);

            DtP.Dispose();
            DtP = null;

            #endregion

            #region //INSERT/UPDATE
            if (clsPmpaPb.GstrPrintChk != "NO")
            {
                if (clsAuto.GstrAREquipUse != "OK")
                {
                    if (nPAmt[24, 1] == 0)
                        strMsgChk = ComFunc.MsgBoxQ("수납금액 없음. 영수증을 출력하시겠습니까?", "출력선택", MessageBoxDefaultButton.Button1).ToString();
                    else
                        strMsgChk = ComFunc.MsgBoxQ("영수증을 출력하시겠습니까?", "출력선택", MessageBoxDefaultButton.Button1).ToString();
                }
                else if (clsAuto.GstrAREquipUse == "OK")
                {
                    strMsgChk = "Yes"; //멘트확인안하고 인쇄

                    if (clsAuto.GnAR_AMT == 0 && ArgRdate.Trim() == "" && clsPmpaType.TOM.GbGameK != "11")
                    {
                        if (clsPmpaPb.GstrMobileSunap != "OK")
                            strMsgChk = "No";
                    }
                }
            }
            else
            {
                strMsgChk = "Yes"; //멘트확인안하고 인쇄
            }

            if (clsPmpaPb.GstrMobilePrint == "OK") { strMsgChk = "Yes"; }

            if (strMsgChk == "No")
            {
                if (clsAuto.GstrAREquipUse != "OK")
                {
                    if (clsPmpaPb.GnOutTuyakNo > 0 && clsPmpaPb.GnTuyakNo > 0)
                        ComFunc.MsgBox("원외처방번호 : " + clsPmpaPb.GnOutTuyakNo + ", 투약번호 : " + clsPmpaPb.GnTuyakNo + " 입니다.", "확인");
                    else if (clsPmpaPb.GnOutTuyakNo > 0 && clsPmpaPb.GnTuyakNo <= 0)
                        ComFunc.MsgBox("원외처방번호 : " + clsPmpaPb.GnOutTuyakNo + " 입니다.", "확인");
                    else if (clsPmpaPb.GnOutTuyakNo <= 0 && clsPmpaPb.GnTuyakNo > 0)
                        ComFunc.MsgBox("투약번호 : " + clsPmpaPb.GnTuyakNo + " 입니다.", "확인");
                }

                if (clsPmpaPb.GstrJeaSunap == "YES")
                {
                    //조별로 수납시 순차적으로 INSERT 함. 마감을 점검 하기위한 문
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO OPD_SUNAP ";
                    SQL += ComNum.VBLF + "        (ACTDATE, OPDNO, PANO, AMT, ";
                    SQL += ComNum.VBLF + "         PART, SEQNO,STIME, ";
                    SQL += ComNum.VBLF + "         BIGO, REMARK, AMT1, ";
                    SQL += ComNum.VBLF + "         AMT2, AMT3, AMT4, ";
                    SQL += ComNum.VBLF + "         AMT5, SEQNO2, DEPTCODE, ";
                    SQL += ComNum.VBLF + "         BI, CARDGB,CardAmt, ";
                    SQL += ComNum.VBLF + "         WorkGbn,GbSPC,GelCode, ";
                    SQL += ComNum.VBLF + "         MCode,VCode,BDan, ";
                    SQL += ComNum.VBLF + "         CDan,Jin,JinDtl, ";
                    SQL += ComNum.VBLF + "         EtcAmt,EtcAmt2,EntDate, ";
                    SQL += ComNum.VBLF + "         BDate,PtAmt,AL200, ";
                    SQL += ComNum.VBLF + "         amt6,Amt7,TaxDan) ";
                    SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "         '" + ArgOpdNo + "',  ";
                    SQL += ComNum.VBLF + "         '" + ArgPtno + "',  ";
                    SQL += ComNum.VBLF + "          " + nPAmt[24, 1] + ", ";
                    SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                    SQL += ComNum.VBLF + "          " + nSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                    SQL += ComNum.VBLF + "         '출력안함', ";
                    SQL += ComNum.VBLF + "         '" + ArgSunap + "', ";
                    SQL += ComNum.VBLF + "          " + nPAmt[20, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + nPAmt[19, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + nPAmt[21, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + nPAmt[22, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + nPAmt[23, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + ArgSeq + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgDept.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgBi.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.RSD.Gubun + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.RSD.TotAmt + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaPb.GstrDrugJobGb + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgSpc + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgGelCode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgMcode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgVcode + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnBTruncLastAmt + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnHTruncLastAmt + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgJin + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgJinDtl + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.BAT.M3_GC_Amt + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnBoninAmt_EF + ", ";
                    SQL += ComNum.VBLF + "         SYSDATE, ";
                    SQL += ComNum.VBLF + "         TO_DATE('" + ArgBDate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnPtVoucherAmt + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaPb.GstrAL200_NOT + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT7T + ", ";
                    SQL += ComNum.VBLF + "          " + nTaxAmt + ", ";
                    SQL += ComNum.VBLF + "          " + nTaxDanAmt + ") ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("OPD_SUNAP INSERT Error !!");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        clsPmpaPb.strDataFlag = "NO";
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    //clsDB.setCommitTran(pDbCon);

                    clsPmpaPb.GnPatAmt = 0;
                    clsPmpaPb.GnJanAmt = 0;
                }

                //예약접수증
                if (clsPmpaPb.GblnResvPrint == false)
                {
                    if (ArgRdate.Trim() != "")
                    {
                        if (clsType.User.IdNumber == "4349")
                        {
                            if (DialogResult.Yes == ComFunc.MsgBoxQ("예약접수증을 출력하시겠습니까?", "알림"))
                                ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, ArgDr, ArgRdate,"", o);
                        }
                        else
                            ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, ArgDr, ArgRdate,"", o);
                    }
                }

                return;
            }
            else
            {
                if (clsPmpaPb.GstrJeaSunap == "YES")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO OPD_SUNAP ";
                    SQL += ComNum.VBLF + "        (ACTDATE, OPDNO, PANO, AMT, ";
                    SQL += ComNum.VBLF + "         PART, SEQNO,STIME, ";
                    SQL += ComNum.VBLF + "         BIGO, REMARK, AMT1, ";
                    SQL += ComNum.VBLF + "         AMT2, AMT3, AMT4, ";
                    SQL += ComNum.VBLF + "         AMT5, SEQNO2, DEPTCODE, ";
                    SQL += ComNum.VBLF + "         BI, CARDGB,CardAmt, ";
                    SQL += ComNum.VBLF + "         WorkGbn,GbSPC,GelCode, ";
                    SQL += ComNum.VBLF + "         MCode,VCode,BDan, ";
                    SQL += ComNum.VBLF + "         CDan,Jin,JinDtl, ";
                    SQL += ComNum.VBLF + "         EtcAmt,EtcAmt2,EntDate, ";
                    SQL += ComNum.VBLF + "         BDate,PtAmt,AL200, ";
                    SQL += ComNum.VBLF + "         amt6,Amt7,TaxDan) ";
                    SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "         '" + ArgOpdNo + "',  ";
                    SQL += ComNum.VBLF + "         '" + ArgPtno + "',  ";
                    SQL += ComNum.VBLF + "          " + nPAmt[24, 1] + ", ";
                    SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                    SQL += ComNum.VBLF + "          " + nSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                    SQL += ComNum.VBLF + "         '', ";
                    SQL += ComNum.VBLF + "         '" + ArgSunap + "', ";
                    SQL += ComNum.VBLF + "          " + nPAmt[20, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + nPAmt[19, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + nPAmt[21, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + nPAmt[22, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + nPAmt[23, 1] + ", ";
                    SQL += ComNum.VBLF + "          " + ArgSeq + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgDept.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgBi.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.RSD.Gubun + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.RSD.TotAmt + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaPb.GstrDrugJobGb + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgSpc + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgGelCode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgMcode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgVcode + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnBTruncLastAmt + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnHTruncLastAmt + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgJin + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgJinDtl + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.BAT.M3_GC_Amt + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnBoninAmt_EF + ", ";
                    SQL += ComNum.VBLF + "         SYSDATE,";
                    SQL += ComNum.VBLF + "         TO_DATE('" + ArgBDate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnPtVoucherAmt + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaPb.GstrAL200_NOT + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT7T + ", ";
                    SQL += ComNum.VBLF + "          " + nTaxAmt + ", ";
                    SQL += ComNum.VBLF + "          " + nTaxDanAmt + ") ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("OPD_HOANBUL INSERT Error !!");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        clsPmpaPb.strDataFlag = "NO";
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsPmpaPb.GnPatAmt = 0;
                    clsPmpaPb.GnJanAmt = 0;

                }

                //예약접수증
                if (clsPmpaPb.GblnResvPrint == false)
                {
                    if (ArgRdate.Trim() != "")
                    {
                        if (clsType.User.IdNumber == "4349")
                        {
                            if (DialogResult.Yes == ComFunc.MsgBoxQ("예약접수증을 출력하시겠습니까?", "알림"))
                                ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, ArgDr, ArgRdate,"", o);
                        }
                        else
                            ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, ArgDr, ArgRdate,"", o);
                    }
                }
            }

            #endregion

            //영수증출력
            clsPrint CP = new clsPrint();
            Card CC = new Card();

            clsPmpaPb.GstrMobilePrint = "";
                        
            if (clsPmpaPb.GstrMobileSunap != "OK")
            {
                CP.getPrinter_Chk(RECEIPT_PRINTER_NAME);
                strPrintName = CP.getPmpaBarCodePrinter(RECEIPT_PRINTER_NAME);
                if (strPrintName.Trim() == "") { return; }

                if (ArgIpdDtl == "Y")
                    ssSpread_Sheet.Cells[0, 4].Text = "입원정밀검진";
                else
                {
                    if (clsPmpaPb.GstatEROVER != "*")
                    {
                        if (ArgRe == "R")
                        {
                            if (clsPmpaPb.GOpd_Sunap_II == "II" || clsPmpaPb.Gstr입원재출력 == "OK")
                                ssSpread_Sheet.Cells[0, 4].Text = "입원(재발행)";
                            else
                                ssSpread_Sheet.Cells[0, 4].Text = "외래(재발행)";
                        }
                        else
                        {
                            if (clsPmpaPb.GOpd_Sunap_II == "II" || clsPmpaPb.Gstr입원재출력 == "OK")
                                ssSpread_Sheet.Cells[0, 4].Text = "입원";
                            else
                                ssSpread_Sheet.Cells[0, 4].Text = "외래";
                        }
                    }
                    else
                        if (ArgRe == "R")
                        ssSpread_Sheet.Cells[0, 4].Text = "응급(재발행)";
                    else
                        ssSpread_Sheet.Cells[0, 4].Text = "응급";
                }

                if (clsPmpaPb.GstrErJobFlag == "OK")
                {
                    if (clsPmpaPb.GnNight1 == "2")
                        ssSpread_Sheet.Cells[3, 13].Text = "√";
                    else if (clsPmpaPb.GnNight1 == "1")
                        ssSpread_Sheet.Cells[3, 13].Text = VB.Space(11) + "√";
                }

                ssSpread_Sheet.Cells[0, 12].Text = ArgPtno + VB.Asc(VB.Left(ArgDept, 1)) + VB.Asc(VB.Right(ArgDept, 1)); //바코드
            }

            if (ArgGwa == "II") //수납항목을 인쇄(가셔야할곳 인쇄)
                PR_Site_Move_II(ArgDept, ArgRetn, ArgBDate, ArgIpdDtl, ref ssSpread, ref ssSpread_Sheet);
            else
                PR_Site_Move4(pDbCon, ArgDept, ArgRetn, ArgBDate, ArgIpdDtl, ref ssSpread, ref ssSpread_Sheet);

            if (clsPmpaPb.GstrMobileSunap == "OK") { return; }

            if (clsPmpaPb.GstrJeaPrint != "OK")
            {
                for (j = 0; j < FnPo; j++)
                {
                    ssSpread_Sheet.Cells[24 + j, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[24 + j, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[24 + j, 12].Text = FstrSite[j];
                }
            }

            //if (strYdate != "")
            //{
            //    j += 1;
            //    ssSpread_Sheet.Cells[24 + j, 12].ColumnSpan = 4;
            //    ssSpread_Sheet.Cells[24 + j, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
            //    ssSpread_Sheet.Cells[24 + j, 12].Text = "금일 예약비는 " + strYdate + " 에";
            //    j += 1;
            //    ssSpread_Sheet.Cells[24 + j, 12].ColumnSpan = 4;
            //    ssSpread_Sheet.Cells[24 + j, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
            //    ssSpread_Sheet.Cells[24 + j, 12].Text = "이미 대체되었습니다.";
            //}

            if (clsPmpaPb.GstrCopyPrint == "OK")
            {
                //2018.06.20 박병규 : 출력위치변경
                //ssSpread_Sheet.Cells[22 + FnPo, 12].ColumnSpan = 4;
                //ssSpread_Sheet.Cells[22 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                //ssSpread_Sheet.Cells[22 + FnPo, 12].Text = "사 본";

                ssSpread_Sheet.Cells[0, 4].Text += " [사본]";
            }

            //약 처방전번호
            if (ArgGwa != "II")
            {
                string Strtemp = "";
                if (clsPmpaPb.GstrDrugJobGb =="02" )
                {
                    Strtemp = "조제불필요";
                }
               
                if (clsPmpaPb.GnTuyakNo > 0 && clsPmpaPb.GnOutDrugNo > 0)
                {
                    ssSpread_Sheet.Cells[22, 13].Text = clsPmpaPb.GnTuyakNo.ToString() + " " + Strtemp;
                    ssSpread_Sheet.Cells[23, 13].Text = clsPmpaPb.GnOutDrugNo.ToString();
                }
                else if (clsPmpaPb.GnTuyakNo > 0)
                    ssSpread_Sheet.Cells[22, 13].Text = clsPmpaPb.GnTuyakNo.ToString() +" " + Strtemp;
                else if (clsPmpaPb.GnOutDrugNo > 0)
                    ssSpread_Sheet.Cells[23, 13].Text = clsPmpaPb.GnOutDrugNo.ToString();
            }
            else if (ArgGwa == "II")
            {
                if (FnPo >= 9)
                    ssSpread_Sheet.Cells[23, 13].Text = "◈수납확인서◈" + " " + clsPmpaType.TOM.WardCode.Trim() + "-" + clsPmpaType.TOM.RoomCode;
            }

            //공급받는자보관용
            ssSpread_Sheet.Cells[3, 0].Text = ArgPtno;
            ssSpread_Sheet.Cells[3, 4].Text = ArgName;

            if (clsPmpaPb.GstrJeaPrint == "OK")
                ssSpread_Sheet.Cells[3, 8].Text = ArgBDate;
            else
                ssSpread_Sheet.Cells[3, 8].Text = clsPublic.GstrSysDate;

            //2019-03-15 무인수납 영수증 요청사항으로 수정함
            ssSpread_Sheet.Cells[5, 0].Text = clsVbfunc.GetBASClinicDeptNameK(pDbCon, ArgGwa) + "(" + ArgGwa + ")";
            ssSpread_Sheet.Cells[5, 0].Text = clsVbfunc.GetBASClinicDeptNameK(pDbCon, ArgDept);
            
            if (ArgBi == "43" && ArgJinDtl == "18")
                ssSpread_Sheet.Cells[5, 11].Text = "보호100%";
            else
                ssSpread_Sheet.Cells[5, 11].Text = CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", ArgBi);

            //수납자 영수증번호
            if (clsPmpaPb.GstrJeaPrint == "OK")
                ssSpread_Sheet.Cells[5, 12].Text = clsPmpaPb.GstrJeaName + "(" + clsPmpaPb.GstrJeaPart.Trim() + ") " + ArgSeq;
            else
                ssSpread_Sheet.Cells[5, 12].Text = clsType.User.JobName + "(" + clsType.User.IdNumber + ") " + ArgSeq;

            //진료항목
            //진찰료
            ssSpread_Sheet.Cells[9, 3].Text = string.Format("{0:#,###}", RPG[0].Amt5 + RPG[40].Amt5); //급여 본인
            ssSpread_Sheet.Cells[9, 5].Text = string.Format("{0:#,###}", RPG[0].Amt6 + RPG[40].Amt6); //급여 공단
            ssSpread_Sheet.Cells[9, 6].Text = string.Format("{0:#,###}", RPG[0].Amt4 + RPG[40].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[9, 8].Text = string.Format("{0:#,###}", RPG[0].Amt3 + RPG[40].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[9, 10].Text = string.Format("{0:#,###}", RPG[0].Amt2 + RPG[40].Amt2); //비급여 선택진료이외
            //투약및조제료(행위료)
            ssSpread_Sheet.Cells[14, 3].Text = string.Format("{0:#,###}", RPG[3].Amt5); //급여 본인
            ssSpread_Sheet.Cells[14, 5].Text = string.Format("{0:#,###}", RPG[3].Amt6); //급여 공단
            ssSpread_Sheet.Cells[14, 6].Text = string.Format("{0:#,###}", RPG[3].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[14, 8].Text = string.Format("{0:#,###}", RPG[3].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[14, 10].Text = string.Format("{0:#,###}", RPG[3].Amt2); //비급여 선택진료이외
            //투약및조제료(약품비)
            ssSpread_Sheet.Cells[15, 3].Text = string.Format("{0:#,###}", RPG[4].Amt5); //급여 본인
            ssSpread_Sheet.Cells[15, 5].Text = string.Format("{0:#,###}", RPG[4].Amt6); //급여 공단
            ssSpread_Sheet.Cells[15, 6].Text = string.Format("{0:#,###}", RPG[4].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[15, 8].Text = string.Format("{0:#,###}", RPG[4].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[15, 10].Text = string.Format("{0:#,###}", RPG[4].Amt2); //비급여 선택진료이외
            //주사료(행위료)
            ssSpread_Sheet.Cells[16, 3].Text = string.Format("{0:#,###}", RPG[5].Amt5); //급여 본인
            ssSpread_Sheet.Cells[16, 5].Text = string.Format("{0:#,###}", RPG[5].Amt6); //급여 공단
            ssSpread_Sheet.Cells[16, 6].Text = string.Format("{0:#,###}", RPG[5].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[16, 8].Text = string.Format("{0:#,###}", RPG[5].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[16, 10].Text = string.Format("{0:#,###}", RPG[5].Amt2); //비급여 선택진료이외
            //주사료(약품비)
            ssSpread_Sheet.Cells[17, 3].Text = string.Format("{0:#,###}", RPG[6].Amt5); //급여 본인
            ssSpread_Sheet.Cells[17, 5].Text = string.Format("{0:#,###}", RPG[6].Amt6); //급여 공단
            ssSpread_Sheet.Cells[17, 6].Text = string.Format("{0:#,###}", RPG[6].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[17, 8].Text = string.Format("{0:#,###}", RPG[6].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[17, 10].Text = string.Format("{0:#,###}", RPG[6].Amt2); //비급여 선택진료이외
            //마취료
            ssSpread_Sheet.Cells[18, 3].Text = string.Format("{0:#,###}", RPG[7].Amt5); //급여 본인
            ssSpread_Sheet.Cells[18, 5].Text = string.Format("{0:#,###}", RPG[7].Amt6); //급여 공단
            ssSpread_Sheet.Cells[18, 6].Text = string.Format("{0:#,###}", RPG[7].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[18, 8].Text = string.Format("{0:#,###}", RPG[7].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[18, 10].Text = string.Format("{0:#,###}", RPG[7].Amt2); //비급여 선택진료이외
            //처치및수술료
            ssSpread_Sheet.Cells[19, 3].Text = string.Format("{0:#,###}", RPG[8].Amt5); //급여 본인
            ssSpread_Sheet.Cells[19, 5].Text = string.Format("{0:#,###}", RPG[8].Amt6); //급여 공단
            ssSpread_Sheet.Cells[19, 6].Text = string.Format("{0:#,###}", RPG[8].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[19, 8].Text = string.Format("{0:#,###}", RPG[8].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[19, 10].Text = string.Format("{0:#,###}", RPG[8].Amt2); //비급여 선택진료이외
            //검사료
            ssSpread_Sheet.Cells[20, 3].Text = string.Format("{0:#,###}", RPG[9].Amt5); //급여 본인
            ssSpread_Sheet.Cells[20, 5].Text = string.Format("{0:#,###}", RPG[9].Amt6); //급여 공단
            ssSpread_Sheet.Cells[20, 6].Text = string.Format("{0:#,###}", RPG[9].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[20, 8].Text = string.Format("{0:#,###}", RPG[9].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[20, 10].Text = string.Format("{0:#,###}", RPG[9].Amt2); //비급여 선택진료이외
            //영상진단료
            ssSpread_Sheet.Cells[21, 3].Text = string.Format("{0:#,###}", RPG[10].Amt5); //급여 본인
            ssSpread_Sheet.Cells[21, 5].Text = string.Format("{0:#,###}", RPG[10].Amt6); //급여 공단
            ssSpread_Sheet.Cells[21, 6].Text = string.Format("{0:#,###}", RPG[10].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[21, 8].Text = string.Format("{0:#,###}", RPG[10].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[21, 10].Text = string.Format("{0:#,###}", RPG[10].Amt2); //비급여 선택진료이외
            //방사선치료료
            ssSpread_Sheet.Cells[22, 3].Text = string.Format("{0:#,###}", RPG[11].Amt5); //급여 본인
            ssSpread_Sheet.Cells[22, 5].Text = string.Format("{0:#,###}", RPG[11].Amt6); //급여 공단
            ssSpread_Sheet.Cells[22, 6].Text = string.Format("{0:#,###}", RPG[11].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[22, 8].Text = string.Format("{0:#,###}", RPG[11].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[22, 10].Text = string.Format("{0:#,###}", RPG[11].Amt2); //비급여 선택진료이외
            //치료재료대
            ssSpread_Sheet.Cells[23, 3].Text = string.Format("{0:#,###}", RPG[12].Amt5); //급여 본인
            ssSpread_Sheet.Cells[23, 5].Text = string.Format("{0:#,###}", RPG[12].Amt6); //급여 공단
            ssSpread_Sheet.Cells[23, 6].Text = string.Format("{0:#,###}", RPG[12].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[23, 8].Text = string.Format("{0:#,###}", RPG[12].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[23, 10].Text = string.Format("{0:#,###}", RPG[12].Amt2); //비급여 선택진료이외
            //재활및물리치료료
            ssSpread_Sheet.Cells[24, 3].Text = string.Format("{0:#,###}", RPG[13].Amt5); //급여 본인
            ssSpread_Sheet.Cells[24, 5].Text = string.Format("{0:#,###}", RPG[13].Amt6); //급여 공단
            ssSpread_Sheet.Cells[24, 6].Text = string.Format("{0:#,###}", RPG[13].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[24, 8].Text = string.Format("{0:#,###}", RPG[13].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[24, 10].Text = string.Format("{0:#,###}", RPG[13].Amt2); //비급여 선택진료이외
            //정신요법료
            ssSpread_Sheet.Cells[25, 3].Text = string.Format("{0:#,###}", RPG[14].Amt5); //급여 본인
            ssSpread_Sheet.Cells[25, 5].Text = string.Format("{0:#,###}", RPG[14].Amt6); //급여 공단
            ssSpread_Sheet.Cells[25, 6].Text = string.Format("{0:#,###}", RPG[14].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[25, 8].Text = string.Format("{0:#,###}", RPG[14].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[25, 10].Text = string.Format("{0:#,###}", RPG[14].Amt2); //비급여 선택진료이외
            //수혈료
            ssSpread_Sheet.Cells[26, 3].Text = string.Format("{0:#,###}", RPG[15].Amt5); //급여 본인
            ssSpread_Sheet.Cells[26, 5].Text = string.Format("{0:#,###}", RPG[15].Amt6); //급여 공단
            ssSpread_Sheet.Cells[26, 6].Text = string.Format("{0:#,###}", RPG[15].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[26, 8].Text = string.Format("{0:#,###}", RPG[15].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[26, 10].Text = string.Format("{0:#,###}", RPG[15].Amt2); //비급여 선택진료이외
            //CT
            ssSpread_Sheet.Cells[27, 3].Text = string.Format("{0:#,###}", RPG[16].Amt5); //급여 본인
            ssSpread_Sheet.Cells[27, 5].Text = string.Format("{0:#,###}", RPG[16].Amt6); //급여 공단
            ssSpread_Sheet.Cells[27, 6].Text = string.Format("{0:#,###}", RPG[16].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[27, 8].Text = string.Format("{0:#,###}", RPG[16].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[27, 10].Text = string.Format("{0:#,###}", RPG[16].Amt2); //비급여 선택진료이외
            //MRI
            ssSpread_Sheet.Cells[28, 3].Text = string.Format("{0:#,###}", RPG[17].Amt5); //급여 본인
            ssSpread_Sheet.Cells[28, 5].Text = string.Format("{0:#,###}", RPG[17].Amt6); //급여 공단
            ssSpread_Sheet.Cells[28, 6].Text = string.Format("{0:#,###}", RPG[17].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[28, 8].Text = string.Format("{0:#,###}", RPG[17].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[28, 10].Text = string.Format("{0:#,###}", RPG[17].Amt2); //비급여 선택진료이외
            //초음파
            ssSpread_Sheet.Cells[29, 3].Text = string.Format("{0:#,###}", RPG[18].Amt5); //급여 본인
            ssSpread_Sheet.Cells[29, 5].Text = string.Format("{0:#,###}", RPG[18].Amt6); //급여 공단
            ssSpread_Sheet.Cells[29, 6].Text = string.Format("{0:#,###}", RPG[18].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[29, 8].Text = string.Format("{0:#,###}", RPG[18].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[29, 10].Text = string.Format("{0:#,###}", RPG[18].Amt2); //비급여 선택진료이외
            //보철/교정료
            ssSpread_Sheet.Cells[30, 3].Text = string.Format("{0:#,###}", RPG[19].Amt5); //급여 본인
            ssSpread_Sheet.Cells[30, 5].Text = string.Format("{0:#,###}", RPG[19].Amt6); //급여 공단
            ssSpread_Sheet.Cells[30, 6].Text = string.Format("{0:#,###}", RPG[19].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[30, 8].Text = string.Format("{0:#,###}", RPG[19].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[30, 10].Text = string.Format("{0:#,###}", RPG[19].Amt2); //비급여 선택진료이외
            //예약진찰료
            ssSpread_Sheet.Cells[31, 3].Text = string.Format("{0:#,###}", RPG[22].Amt5); //급여 본인
            ssSpread_Sheet.Cells[31, 5].Text = string.Format("{0:#,###}", RPG[22].Amt6); //급여 공단
            ssSpread_Sheet.Cells[31, 6].Text = string.Format("{0:#,###}", RPG[22].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[31, 8].Text = string.Format("{0:#,###}", RPG[22].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[31, 10].Text = string.Format("{0:#,###}", RPG[22].Amt2); //비급여 선택진료이외
            //증명료
            ssSpread_Sheet.Cells[32, 3].Text = string.Format("{0:#,###}", RPG[21].Amt5); //급여 본인
            ssSpread_Sheet.Cells[32, 5].Text = string.Format("{0:#,###}", RPG[21].Amt6); //급여 공단
            ssSpread_Sheet.Cells[32, 6].Text = string.Format("{0:#,###}", RPG[21].Amt4); //급여 전액부담
            ssSpread_Sheet.Cells[32, 8].Text = string.Format("{0:#,###}", RPG[21].Amt3); //비급여 선택진료
            ssSpread_Sheet.Cells[32, 10].Text = string.Format("{0:#,###}", RPG[21].Amt2); //비급여 선택진료이외

            if (ArgGwa == "R6") { nPAmt[16, 2] = nPAmt[24, 1]; }

            if (RPG[29].Amt2 > 0 || RPG[29].Amt3 > 0 || RPG[29].Amt4 > 0 || RPG[29].Amt5 > 0 || RPG[29].Amt6 > 0)
            {
                if (nPAmt[16, 1] > 0)
                    ssSpread_Sheet.Cells[34 + nCntEtc1, 0].Text = "응급관리료/기타";
                else
                    ssSpread_Sheet.Cells[34 + nCntEtc1, 0].Text = "기           타";

                ssSpread_Sheet.Cells[34 + nCntEtc1, 3].Text = string.Format("{0:#,###}", RPG[29].Amt5); //급여 본인
                ssSpread_Sheet.Cells[34 + nCntEtc1, 5].Text = string.Format("{0:#,###}", RPG[29].Amt6); //급여 공단
                ssSpread_Sheet.Cells[34 + nCntEtc1, 6].Text = string.Format("{0:#,###}", RPG[29].Amt4); //급여 전액부담
                ssSpread_Sheet.Cells[34 + nCntEtc1, 8].Text = string.Format("{0:#,###}", RPG[29].Amt3); //비급여 선택진료
                ssSpread_Sheet.Cells[34 + nCntEtc1, 10].Text = string.Format("{0:#,###}", RPG[29].Amt2); //비급여 선택진료이외

                nCntEtc1 += 1;
            }

            //선별급여
            if (RPG[49].Amt7 > 0)
            {
                ssSpread_Sheet.Cells[34 + nCntEtc1, 0].Text = "선 별 급 여";

                ssSpread_Sheet.Cells[34 + nCntEtc1, 3].Text = string.Format("{0:#,###}", RPG[49].Amt9); //급여 본인
                ssSpread_Sheet.Cells[34 + nCntEtc1, 5].Text = string.Format("{0:#,###}", RPG[49].Amt8); //급여 공단
                ssSpread_Sheet.Cells[34 + nCntEtc1, 6].Text = ""; //급여 전액부담
                ssSpread_Sheet.Cells[34 + nCntEtc1, 8].Text = ""; //비급여 선택진료
                ssSpread_Sheet.Cells[34 + nCntEtc1, 10].Text = ""; //비급여 선택진료이외

                nCntEtc1 += 1;
            }

            if (VB.Left(ArgRdate.Trim(), 10) == "" && clsAuto.GstrAREquipUse == "OK")
            {
                ArgRdate = COC.DATE_RESERVED(pDbCon, ArgPtno, ArgRdate, ArgBDate, ArgDept.Trim());
            }

            // if (VB.Left(ArgRdate.Trim(), 10) != "" && clsAuto.GstrAREquipUse == "OK")
            if (VB.Left(ArgRdate.Trim(), 10) != "" )
            {
                //ssSpread_Sheet.Cells[24 + FnPo, 12].ColumnSpan = 4;
                //ssSpread_Sheet.Cells[24 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                //ssSpread_Sheet.Cells[24 + FnPo, 12].Font = new Font("굴림", 11, FontStyle.Bold);
                //ssSpread_Sheet.Cells[24 + FnPo, 12].Text = "*** 예 약 증 ***";
                //ssSpread_Sheet.Cells[25 + FnPo, 12].ColumnSpan = 4;
                //ssSpread_Sheet.Cells[25 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                //ssSpread_Sheet.Cells[25 + FnPo, 12].Font = new Font("굴림", 11, FontStyle.Bold);
                //ssSpread_Sheet.Cells[25 + FnPo, 12].Text = VB.Left(ArgRdate, 4) + "." + VB.Mid(ArgRdate, 6, 2) + "." + VB.Mid(ArgRdate, 9, 2) + "(" + VB.Left(CF.READ_YOIL(pDbCon, VB.Left(ArgRdate, 10)), 1) + ") " + VB.Right(ArgRdate.Trim(), 5);
                //ssSpread_Sheet.Cells[26 + FnPo, 12].ColumnSpan = 4;
                //ssSpread_Sheet.Cells[26 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                //ssSpread_Sheet.Cells[26 + FnPo, 12].Font = new Font("굴림", 11, FontStyle.Bold);
                //ssSpread_Sheet.Cells[26 + FnPo, 12].Text = clsVbfunc.GetBASClinicDeptNameK(pDbCon, ArgGwa) + "(" + ArgGwa + ")" + ":" + CF.READ_DrName(pDbCon, ArgDr);//clsVbfunc.GetBASClinicDeptNameK(pDbCon, ArgGwa) + "(" + ArgGwa + ")";
                //ssSpread_Sheet.Cells[27 + FnPo, 12].ColumnSpan = 4;
                //ssSpread_Sheet.Cells[27 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                //ssSpread_Sheet.Cells[27 + FnPo, 12].Font = new Font("굴림", 11, FontStyle.Bold);
                //ssSpread_Sheet.Cells[27 + FnPo, 12].Text = "전화번호:" +  CPF.Get_Dept_TelNo(pDbCon, ArgDept, clsPmpaType.TOM.DrCode);
                //ssSpread_Sheet.Cells[28 + FnPo, 12].ColumnSpan = 4;
                //ssSpread_Sheet.Cells[28 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                //ssSpread_Sheet.Cells[28 + FnPo, 12].Font = new Font("굴림", 11, FontStyle.Bold);
                //ssSpread_Sheet.Cells[28 + FnPo, 12].Text = "등록번호:" + ArgPtno;
                //ssSpread_Sheet.Cells[29 + FnPo, 12].ColumnSpan = 4;
                //ssSpread_Sheet.Cells[29 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                //ssSpread_Sheet.Cells[29 + FnPo, 12].Font = new Font("굴림", 11, FontStyle.Bold);
                //ssSpread_Sheet.Cells[29 + FnPo, 12].Text = "수진자명:" + ArgName;
                // ssSpread_Sheet.Cells[36, 3].Text = "<예약>" + VB.Space(3) + string.Format("{0:yyyy년 MM월 dd일}", VB.Left(ArgRdate.Trim(), 10)) + VB.Space(1) + string.Format("{0:HH시 MM분}", VB.Mid(ArgRdate, 11, 6)) + VB.Space(3) + ArgGwa + VB.Space(3) + CF.READ_DrName(pDbCon, ArgDr);
                ssSpread_Sheet.Cells[35, 3].Font = new Font("굴림", 11, FontStyle.Bold);
                ssSpread_Sheet.Cells[35, 3].ColumnSpan = 8;
                ssSpread_Sheet.Cells[35, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[35, 3].Text = "★예약 " + VB.Space(3) + VB.Left(ArgRdate, 4) + "." + VB.Mid(ArgRdate, 6, 2) + "." + VB.Mid(ArgRdate, 9, 2) + " (" + VB.Left(CF.READ_YOIL(pDbCon, VB.Left(ArgRdate, 10)), 1) + ") " + VB.Right(ArgRdate.Trim(), 5);
                ssSpread_Sheet.Cells[36, 3].Font = new Font("굴림", 11, FontStyle.Bold);
                ssSpread_Sheet.Cells[36, 3].ColumnSpan = 8;
                ssSpread_Sheet.Cells[36, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
                //ssSpread_Sheet.Cells[36, 3].Text = clsVbfunc.GetBASClinicDeptNameK(pDbCon, ArgGwa) + " (" + ArgGwa + ") " + ":" + CF.READ_DrName(pDbCon, ArgDr) + "(☎ " + CPF.Get_Dept_TelNo(pDbCon, ArgDept, clsPmpaType.TOM.DrCode) + ") " ;
                ssSpread_Sheet.Cells[36, 3].Text = clsVbfunc.GetBASClinicDeptNameK(pDbCon, ArgDept) + ":" + CF.READ_DrName(pDbCon, ArgDr) + "(☎ " + CPF.Get_Dept_TelNo(pDbCon, ArgDept, clsPmpaType.TOM.DrCode) + ") ";
            }
            //else
            //{
            //    if (VB.Left(ArgRdate.Trim(), 10) != "")
            //    {
            //        ssSpread_Sheet.Cells[36, 3].ColumnSpan = 8;
            //        ssSpread_Sheet.Cells[36, 3].HorizontalAlignment = CellHorizontalAlignment.Left;
            //        ssSpread_Sheet.Cells[36, 3].Text = "<예약>" + VB.Space(3) + string.Format("{0:yyyy년 MM월 dd일}", VB.Left(ArgRdate.Trim(), 10)) + VB.Space(1) + string.Format("{0:HH시 MM분}", VB.Mid(ArgRdate, 11, 5)) + VB.Space(3) + ArgGwa + VB.Space(3) + CF.READ_DrName(pDbCon, ArgDr);
            //    }
            //}

            if (clsPmpaPb.GstrPCLR == "OK")
            {
                ssSpread_Sheet.Cells[24 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[24 + FnPo, 12].Text = "★환자복 반납시 보증금을 ";
                ssSpread_Sheet.Cells[25 + FnPo, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[25 + FnPo, 12].Text = "환불 받을 수 있습니다.";
            }

            //합계
            ssSpread_Sheet.Cells[37, 3].Text = string.Format("{0:#,###}", RPG[35].Amt1); //급여 본인
            ssSpread_Sheet.Cells[37, 5].Text = string.Format("{0:#,###}", RPG[36].Amt1); //급여 공단
            ssSpread_Sheet.Cells[37, 6].Text = string.Format("{0:#,###}", RPG[37].Amt1); //급여 전액부담
            ssSpread_Sheet.Cells[37, 8].Text = string.Format("{0:#,###}", RPG[38].Amt1); //비급여 선택진료
            ssSpread_Sheet.Cells[37, 10].Text = string.Format("{0:#,###}", RPG[39].Amt1); //비급여 선택진료이외

            //<금액산정내용>***************************************************************************************************************
            //진료비총액
            ssSpread_Sheet.Cells[7, 13].Text = string.Format("{0:#,###}", RPG[30].Amt1);

            //환자부담총액 = 환자부담총액 - 예약비
            if (Card.GnOgAmt == 0)
                ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", nPAmt[21, 1] - nAmt8);
            else
            {
                if (nPAmt[21, 1] < Card.GnOgAmt)
                    //산전카드금액이 환자부담금보다 크면 같게함
                    ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", nPAmt[21, 1] - nAmt8 - nPAmt[21, 1]);
                else
                    //산전카드금액이 있으면
                    ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", nPAmt[21, 1] - nAmt8 - Card.GnOgAmt);
            }
            clsPmpaPb.GnPrintChaAmt = 0;
            clsPmpaPb.GnPrintChaAmt = (RPG[35].Amt1 + RPG[37].Amt1 + RPG[39].Amt1) - nPAmt[21, 1];

            //부가세
            if (nTaxAmt != 0)
            {
                ssSpread_Sheet.Cells[13 + nCntEtc2, 11].ColumnSpan = 5;
                ssSpread_Sheet.Cells[13 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[13 + nCntEtc2, 11].Text = "부가세 : " + string.Format("{0:#,###}", nTaxAmt);
                nCntEtc2 += 1;
            }

            //납부할금액
            if (clsPmpaPb.GOpd_Sunap_Boamt != 0)
                ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", nPAmt[24, 1] + nTaxAmt - clsPmpaPb.GOpd_Sunap_Boamt);
            else
            {
                if (clsPmpaPb.GnPtVoucherAmt != 0)
                    ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", nPAmt[24, 1] + nTaxAmt - clsPmpaPb.GnPtVoucherAmt);
                else
                    ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", nPAmt[24, 1] + nTaxAmt);
            }

            //금액산정부분 예외
            if (nPAmt[22, 1] > 0)
            {
                ssSpread_Sheet.Cells[13 + nCntEtc2, 11].ColumnSpan = 5;
                ssSpread_Sheet.Cells[13 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[13 + nCntEtc2, 11].Text = "감액 : " + string.Format("{0:#,###}", nPAmt[22, 1]);

                if ((clsPmpaType.TOM.GbGameK != "00" && clsPmpaType.TOM.GbGameK != "") || (clsPmpaType.TOM.GbGameKC != "50" && clsPmpaType.TOM.GbGameKC != ""))
                {
                    if (clsPmpaType.TOM.GbGameK != "00")
                    {
                        ssSpread_Sheet.Cells[13 + nCntEtc2, 11].ColumnSpan = 5;
                        ssSpread_Sheet.Cells[13 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                        ssSpread_Sheet.Cells[13 + nCntEtc2, 11].Text = "감액 : " + string.Format("{0:#,###}", nPAmt[22, 1]) + "(" + CF.Read_Bcode_Name(pDbCon, "BAS_감액코드명", clsPmpaType.TOM.GbGameK) + ")";
                    }
                    else if (clsPmpaType.TOM.GbGameK != "50")
                    {
                        ssSpread_Sheet.Cells[13 + nCntEtc2, 11].ColumnSpan = 5;
                        ssSpread_Sheet.Cells[13 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                        ssSpread_Sheet.Cells[13 + nCntEtc2, 11].Text = "감액 : " + string.Format("{0:#,###}", nPAmt[22, 1]) + "(" + CF.Read_Bcode_Name(pDbCon, "BAS_감액코드명", clsPmpaType.TOM.GbGameKC) + ")";
                    }
                }

                nCntEtc2 += 1;
            }

            if (Card.GnOgAmt != 0 && ArgDept == "OG")
            {
                ssSpread_Sheet.Cells[13 + nCntEtc2, 11].ColumnSpan = 5;
                ssSpread_Sheet.Cells[13 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[13 + nCntEtc2, 11].Text = "고운맘금액:" + string.Format("{0:#,###}", Card.GnOgAmt) + ", 잔액:" + string.Format("{0:#,###}", Card.GnOgJanAmt);
                nCntEtc2 += 1;
            }

            if (RPG[34].Amt1 > 0)
            {
                ssSpread_Sheet.Cells[13 + nCntEtc2, 11].ColumnSpan = 5;
                ssSpread_Sheet.Cells[13 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                ssSpread_Sheet.Cells[13 + nCntEtc2, 11].Text = "미수금 : " + string.Format("{0:#,###}", RPG[34].Amt1);
                nCntEtc2 += 1;
            }

            if (clsPmpaType.TOM.Bi == "21")
            {
                if (clsPmpaType.BAT.M3_GC_Amt != 0)
                {
                    ssSpread_Sheet.Cells[13 + nCntEtc2, 11].ColumnSpan = 5;
                    ssSpread_Sheet.Cells[13 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[13 + nCntEtc2, 11].Text = "(가상계좌)" + string.Format("{0:#,###}", clsPmpaType.BAT.M3_GC_Amt);
                    nCntEtc2 += 1;
                }
                else if (clsPmpaPb.GOpd_Sunap_Boamt != 0)
                {
                    ssSpread_Sheet.Cells[13 + nCntEtc2, 11].ColumnSpan = 5;
                    ssSpread_Sheet.Cells[13 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[13 + nCntEtc2, 11].Text = "(가상계좌)" + string.Format("{0:#,###}", clsPmpaPb.GOpd_Sunap_Boamt);
                    nCntEtc2 += 1;
                }

                if (clsPmpaType.BAT.M3_PregDmndAmt != 0 && ArgDept == "OG")
                {
                    ssSpread_Sheet.Cells[13 + nCntEtc2, 11].ColumnSpan = 5;
                    ssSpread_Sheet.Cells[13 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[13 + nCntEtc2, 11].Text = "(산전가상계좌)" + string.Format("{0:#,###}", clsPmpaType.BAT.M3_PregDmndAmt);
                    nCntEtc2 += 1;
                }
            }

            //물리치료 바우처 금액있을경우
            if (clsPmpaPb.GnPtVoucherAmt != 0)
            {
                ssSpread_Sheet.Cells[13 + nCntEtc2, 11].ColumnSpan = 5;
                ssSpread_Sheet.Cells[13 + nCntEtc2, 11].HorizontalAlignment = CellHorizontalAlignment.Center;
                ssSpread_Sheet.Cells[13 + nCntEtc2, 11].Text = "(바우처)" + string.Format("{0:#,###}", clsPmpaPb.GnPtVoucherAmt);
                nCntEtc2 += 1;
            }

            //납부할 금액(2021-08-03)
            if (clsPmpaPb.GstrCopyPrint == "OK")
            {
                //사본출력의 경우 신용카드,현금영수증,현금 구분 없이 총액만 표시되도록 함.
                //frmOrderList.cs 폼에서 사본출력할 경우에만 적용이 됩니다.
                //2021-08-03 김윤희 선생님과 협의함.
            }
            else
            {
                if (clsPmpaType.RSD.Gubun == "1")
                {
                    if (clsPmpaType.RD.OrderGb == "2")
                    {
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//카드

                        if (clsPmpaPb.GnPtVoucherAmt != 0)
                            ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - (clsPmpaType.RSD.TotAmt * -1) - clsPmpaPb.GnPtVoucherAmt);//현금
                        else
                            ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - (clsPmpaType.RSD.TotAmt * -1));//현금
                    }
                    else
                    {
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//카드

                        if (clsPmpaPb.GnPtVoucherAmt != 0)
                            ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - (clsPmpaType.RSD.TotAmt) - clsPmpaPb.GnPtVoucherAmt);//현금
                        else
                            ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - (clsPmpaType.RSD.TotAmt));//현금
                    }
                }
                else if (clsPmpaType.RSD.Gubun == "2")
                {
                    if (clsPmpaType.RD.OrderGb == "2")
                    {
                        ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//현금영수증

                        if (clsPmpaPb.GnPtVoucherAmt != 0)
                            ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - (clsPmpaType.RSD.TotAmt * -1) - clsPmpaPb.GnPtVoucherAmt);//현금
                        else
                            ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - (clsPmpaType.RSD.TotAmt * -1));//현금

                        if (clsPmpaType.RSD.CardSeqNo != 0)
                        {
                            ssSpread_Sheet.Cells[30, 12].Text = "현금승인취소";
                            ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                            ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                        }
                    }
                    else
                    {
                        ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//현금영수증

                        if (clsPmpaPb.GnPtVoucherAmt != 0)
                            ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - (clsPmpaType.RSD.TotAmt) - clsPmpaPb.GnPtVoucherAmt);//현금
                        else
                            ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - (clsPmpaType.RSD.TotAmt));//현금

                        if (clsPmpaType.RSD.CardSeqNo != 0)
                        {
                            ssSpread_Sheet.Cells[30, 12].Text = "현금승인";
                            ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                            ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                        }
                    }
                }
                else
                {
                    if (clsPmpaPb.GnPtVoucherAmt != 0)
                        ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - clsPmpaPb.GnPtVoucherAmt);//현금
                    else
                        ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0));//현금
                }
            }


            if (clsPmpaPb.GnPtVoucherAmt != 0)
            {
                ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0) - clsPmpaPb.GnPtVoucherAmt);//합계
            }
            else
            {
                ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", (long)Math.Truncate(((nPAmt[24, 1] + nTaxAmt) / 10) * 10.0));//합계
            }

            if (ArgRe == "R" && ArgReDate.Trim() != "")
            {
                ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + VB.Left(ArgReDate, 4) + VB.Space(15) + VB.Mid(ArgReDate, 6, 2) + VB.Space(15) + VB.Mid(ArgReDate, 9, 2) + VB.Space(15);
            }
            else
            {
                if (strTimeSS != "")
                    ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime + ":" + strTimeSS;
                else
                    ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime;
            }

            //카드영수증 통합
            if (clsPmpaType.RSD.CardSeqNo > 0)
            {
                Card.GstrCardApprov_Info = CC.Card_Sign_Info_Set(pDbCon, clsPmpaType.RSD.CardSeqNo, ArgPtno);

                if (Card.GstrCardApprov_Info != "")
                {
                    ssSpread_Sheet.Cells[31, 13].ColumnSpan = 3;
                    ssSpread_Sheet.Cells[31, 13].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[31, 13].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 24); //거래구분

                    ssSpread_Sheet.Cells[32, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[32, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[32, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 4) + " " + VB.Pstr(Card.GstrCardApprov_Info, "{}", 2); //카드종류/카드번호
                    ssSpread_Sheet.Cells[33, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[33, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[33, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 20); //승인금액
                    ssSpread_Sheet.Cells[34, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[34, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[34, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 12); //승인번호
                    ssSpread_Sheet.Cells[35, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[35, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[35, 12].Text = "72117503"; //가맹점번호
                    ssSpread_Sheet.Cells[36, 12].ColumnSpan = 2;
                    ssSpread_Sheet.Cells[36, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[36, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 14); //거래일시
                    ssSpread_Sheet.Cells[36, 15].Text = VB.Pstr(VB.Pstr(Card.GstrCardApprov_Info, "{}", 8), "개월", 1);
                }

                Card.GstrCardApprov_Info = "";
            }
            strPrintName = VB.Replace(strPrintName, "(리디렉션)", "");
            strPrintName = VB.Replace(strPrintName, "(리디렉션 1)", "");
            strPrintName = VB.Replace(strPrintName, "(리디렉션 2)", "");
            clsDP.PrintReciptPrint_New(ssSpread, ssSpread_Sheet, strPrintName );
        }

        private void PR_Site_Move4(PsmhDb pDbCon, string ArgDept, string ArgRetn, string ArgBDate, string ArgIpdDtl, ref FpSpread ssSpread, ref SheetView ssSpread_Sheet)
        {
            ComFunc CF = new ComFunc();
            DataTable DtP = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int nCnt = 0;

            ComFunc.ReadSysDate(pDbCon);

            FnPo = 0;
            for (int i = 0; i < 10; i++)
                FstrSite[i] = "";

            cDP.GstrGo = "";

            if (ArgRetn == "1") { return; }

            if (ArgDept != "ER")
            {
                if (clsPmpaPb.GstrCPStat == "OK")
                {
                    FstrSite[FnPo] = "본관1층 현미경실";
                    FnPo += 1;
                }

                if (clsPmpaPb.GstrXRAYStat2 == "OK")
                {
                    FstrSite[FnPo] = "신관1층 영상의학과(골밀도)";
                    FnPo += 1;
                }
                if (clsPmpaPb.GstrLABStat_Slide == "OK")
                {
                    FstrSite[FnPo] = "본관2층 진단검사(병리과)";
                    FnPo += 1;
                }

                if (clsPmpaPb.GstrLABStat == "OK")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT PANO FROM ADMIN.EXAM_ORDER ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgBDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND SPECNO IS NULL ";
                    SQL += ComNum.VBLF + "    AND IPDOPD    = 'O' ";
                    SQL += ComNum.VBLF + "  GROUP BY PANO  ";
                    SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtP.Dispose();
                        DtP = null;
                        return;
                    }

                    if (DtP.Rows.Count > 0)
                    {
                        FstrSite[FnPo] = VB.Left("신관 2층 외래채혈실" + VB.Space(14), 14) + "대기 " + VB.Right(VB.Space(2) + string.Format("{0:##}", DtP.Rows.Count), 2) + " 명";
                        FnPo += 1;
                    }

                    DtP.Dispose();
                    DtP = null;

                    if (clsPmpaPb.GstrLABStat_Res == "OK") { FstrSite[FnPo] += "[예약]"; }
                }
                // 1층 심전도실 2층통합으로 변경
                if (clsPmpaPb.GstrEKGStat == "OK" || clsPmpaPb.GstrEKGStat2 == "OK" )
                {
                    FstrSite[FnPo] = "신관 2층심전도실";
                    FnPo += 1;
                }

               // if ( clsPmpaPb.GstrEKGStat2 == "OK")
               // {
               //     FstrSite[FnPo] = "신관 1층심전도실";
               //     FnPo += 1;
               // }

                if (clsPmpaPb.GstrEKGEegStat == "OK")
                {
                    FstrSite[FnPo] = "신관2층 뇌파검사실";
                    FnPo += 1;
                }

                if (clsPmpaPb.GstrINJStat == "OK")
                {
                    FstrSite[FnPo] = "본관1층 외래주사실";
                    FnPo += 1;
                }
                if (clsPmpaPb.GstrINJStat2 == "OK")
                {
                    //2021-06-22 소아주사실 사라져서 외래주사실로 치환, 변수는 그대로 놔둠.
                    //FstrSite[FnPo] = "본관1층 소아외래주사실";
                    FstrSite[FnPo] = "본관1층 외래주사실";
                    FnPo += 1;
                }
                if (clsPmpaPb.GstrINJStat3 == "OK")
                {
                    FstrSite[FnPo] = "2층 항암주사실";
                    FnPo += 1;
                }
                if (clsPmpaPb.GstrEMGStat == "OK")
                {
                    FstrSite[FnPo] = "재활의학과내 근전도실";
                    FnPo += 1;
                }

                if (clsPmpaPb.GstrEMGNMStat == "OK")
                {
                    FstrSite[FnPo] = "심전도실내 근전도실";
                    FnPo += 1;
                }

                if (clsPmpaPb.GstrEMGStatF633 == "OK")
                {
                    FstrSite[FnPo] = "신관2층 어지럼검사실";
                    FnPo += 1;
                }

                if (clsPmpaPb.GstrPTStat == "OK")
                {
                    FstrSite[FnPo] = "본관3층 재활치료실(물리,작업,언어)";
                    FnPo += 1;
                }

                if (clsPmpaPb.GstrXRAYStat_49 == "OK")
                {
                    FstrSite[FnPo] = "신관1층 영상의학과 접수";
                    FnPo += 1;
                }

                if (clsPmpaPb.GstrXRAYStat2 == "OK" || clsPmpaPb.GstrXRAYStat == "OK")
                {
                    if (clsPmpaPb.GstrXRAYStat10 == "OK")
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT NVL(COUNT(*), 0) CNT From ADMIN.XRAY_DETAIL ";
                        SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
                        SQL += ComNum.VBLF + "    AND SeekDate      >= TO_DATE('" + ArgBDate + "', 'YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND SeekDate      <  TO_DATE('" + VB.DateAdd("D", 1, ArgBDate).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND GbReserved    >= '6' ";
                        SQL += ComNum.VBLF + "    AND XJong         = '1' ";
                        SQL += ComNum.VBLF + "    AND PacsNo IS NOT NULL ";
                        SQL += ComNum.VBLF + "    AND Pacs_End IS NULL ";
                        SQL += ComNum.VBLF + "    AND (GbEnd IS NULL OR GbEnd <> '1') ";
                        SQL += ComNum.VBLF + "    AND DEPTCODE NOT IN ('TO')";
                        SQL += ComNum.VBLF + "    AND (DEPTCODE NOT IN ('DT') OR IPDOPD <> 'O') ";
                        SQL += ComNum.VBLF + "    AND (DEPTCODE NOT IN ('RM') OR IPDOPD <> 'O') ";
                        SQL += ComNum.VBLF + "    AND (DEPTCODE NOT IN ('PD') OR IPDOPD <> 'O') ";
                        SQL += ComNum.VBLF + "    AND (DRCODE NOT IN ('1107','1125') OR IPDOPD <> 'O') ";
                        SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            DtP.Dispose();
                            DtP = null;
                            return;
                        }

                        nCnt = DtP.Rows.Count;

                        DtP.Dispose();
                        DtP = null;

                        SQL = "";
                        SQL += ComNum.VBLF + " SELECT PANO CNT FROM XRAY_DETAIL ";
                        SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
                        SQL += ComNum.VBLF + "    AND SeekDate      >= TO_DATE('" + ArgBDate + "', 'YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND SeekDate      <  TO_DATE('" + VB.DateAdd("D", 1, ArgBDate).ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "    AND ORDERNO       > 0 ";
                        SQL += ComNum.VBLF + "    AND (GbReserved = '1' OR GbReserved = '2') ";
                        SQL += ComNum.VBLF + "    AND IPDOPD        = 'O' ";
                        SQL += ComNum.VBLF + "    AND XJONG         = '1' ";
                        SQL += ComNum.VBLF + "  GROUP BY PANO ";
                        SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            DtP.Dispose();
                            DtP = null;
                            return;
                        }

                        if (DtP.Rows.Count > 0)
                        {
                            if (ArgDept == "MR")
                            {
                                FstrSite[FnPo] = "류마티스내과 전용촬영실";
                                FnPo += 1;
                            }
                            else
                            {
                                // FstrSite[FnPo] = VB.Left("신관 1층 영상의학과" + VB.Space(15), 15) + "대기 " + VB.Right(VB.Space(2) + DtP.Rows.Count + nCnt, 2) + " 명";
                                FstrSite[FnPo] = "신관 1층 영상의학과 접수";
                                FnPo += 1;
                            }
                        }
                        else
                        {
                            FstrSite[FnPo] = "신관 1층 영상의학과 접수";
                            FnPo += 1;
                        }

                        DtP.Dispose();
                        DtP = null;
                    }
                    else
                    {
                        FstrSite[FnPo] = "신관 1층 영상의학과 접수";
                        FnPo += 1;
                    }

                    if (ArgDept == "MR" && clsPmpaPb.GstrXRAYStat10 == "OK" && clsPmpaPb.GstrXRAYStat5 == "OK")
                    {
                        FstrSite[FnPo] = "신관 1층 영상의학과 접수";
                        FnPo += 1;
                    }

                    if (clsPmpaPb.GstrXRAYStat_Res == "OK") { FstrSite[FnPo] += "[예약]"; }
                }
                else if (clsPmpaPb.GstrXRAYStat5 == "OK")
                {
                    FstrSite[FnPo] = "신관 1층 영상의학과 접수";
                    FnPo += 1;

                    if (clsPmpaPb.GstrXRAYStat_Res == "OK") { FstrSite[FnPo] += "[예약]"; }
                }
                else if (clsPmpaPb.GstrXRAYStat11 == "OK")
                {
                    FstrSite[FnPo] = "동위원소검사";
                    FnPo += 1;
                }

                if (clsPmpaPb.GstrENTStat == "OK")
                {
                    FstrSite[FnPo] = "본관 2층이비인후과(청력실)";
                    FnPo += 1;
                }

                if (clsPmpaPb.GstrENTStat2 == "OK")
                {
                    FstrSite[FnPo] = "본관 2층이비인후과(진료실)";
                    FnPo += 1;
                }

                if (clsPmpaPb.GstrCASTStat == "OK")
                {
                    FstrSite[FnPo] = "본관 1층 치료실";
                    FnPo += 1;
                }
            }

            if (clsPmpaPb.GnTuyakNo > 0)
            {
                CF.DATE_HUIL_CHECK(pDbCon, ArgBDate);

                if (CF.READ_YOIL(pDbCon, ArgBDate) == "토요일" || CF.READ_YOIL(pDbCon, ArgBDate) == "일요일")
                {
                    FstrSite[FnPo] = "본관 지하1층 병원약국";
                    FnPo += 1;
                }

                else if (ArgBDate  == "2020-05-01" ) //임시공휴일 아닌 날인데 오전근무로인한 조정 근로자의날
                {
                    FstrSite[FnPo] = "본관 지하1층 병원약국";
                    FnPo += 1;
                }
                else
                {
                    if (clsPublic.GstrHoliday == "*" && clsPublic.GstrTempHoliday != "*")
                    {
                        FstrSite[FnPo] = "본관 지하1층 병원약국";
                        FnPo += 1;
                    }
                    else
                    {
                        if (string.Compare(clsPublic.GstrSysTime, "17:30") > 0 || string.Compare(clsPublic.GstrSysTime, "08:30") < 0)
                        {
                            FstrSite[FnPo] = "본관 지하1층 병원약국";
                            FnPo += 1;
                        }
                        else
                        {
                            FstrSite[FnPo] = "본관1층 약국 투약번호(" + clsPmpaPb.GnTuyakNo +")" ;
                            clsPmpaPb.GstrGo_TuNo = clsPmpaPb.GnTuyakNo;
                            FnPo += 1;
                        }
                    }
                }
            }

            if (clsPmpaPb.GnOutTuyakNo > 0)
            {
                FstrSite[FnPo] = "병원밖 외부(일반)약국";
                FnPo += 1;
            }

            if (ArgDept != "ER")
            {
                if (clsPmpaPb.GstrXRAYStat3 == "OK")
                {
                    FstrSite[FnPo] = "신관2층 뇌혈류검사실";
                    FnPo += 1;
                }

                if (clsPmpaPb.GstrXRAYStat4 == "OK")
                {
                    FstrSite[FnPo] = "본관 1층 비뇨기과";
                    FnPo += 1;
                }

                if (clsPmpaPb.GstrXRAYStat12 == "OK")
                {
                    FstrSite[FnPo] = "본관 1층 정형외과";
                    FnPo += 1;
                }

                if (clsPmpaPb.GstrECHOStat == "OK")
                {
                    FstrSite[FnPo] = "본관 2층 심장초음파실";
                    FnPo += 1;
                }

                if (clsPmpaPb.GstrCAGStat == "OK")
                {
                    FstrSite[FnPo] = "신관 2층 혈관조영실로 가세요";
                    FnPo += 1;
                }

                if (clsPmpaPb.GstrENDOStat == "OK")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT PTNO FROM ADMIN.ENDO_JUPMST ";
                    SQL += ComNum.VBLF + "  WHERE RDATE = TO_DATE('" + ArgBDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND RESULTDATE IS NULL ";
                    SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtP.Dispose();
                        DtP = null;
                        return;
                    }

                    if (DtP.Rows.Count > 0)
                    {
                        FstrSite[FnPo] = VB.Left("본관 2층 내시경실" + VB.Space(15), 15) + "대기 " + VB.Right(VB.Space(2) + DtP.Rows.Count, 2) + " 명";
                        FnPo += 1;
                    }

                    DtP.Dispose();
                    DtP = null;
                }

                if (clsPmpaPb.GstrDMStat == "OK")
                {
                    FstrSite[FnPo] = "본관 3층 피부과";
                    FnPo += 1;
                }

                if (clsPmpaPb.GstrPURStat == "OK")
                {
                    FstrSite[FnPo] = "본관 지하1층 구매과";
                    FnPo += 1;
                }
            }

            //모바일수납 가셔야할곳
            clsPmpaPb.GstrGo = "";

            for (int i = 0; i < 10; i++)
            {
                if (FstrSite[i].Trim() != "")
                {
                    cDP.GstrGo += FstrSite[i].Trim() + "|";
                    clsPmpaPb.GstrGo += FstrSite[i].Trim() + "|";
                }
            }
        }

        private void PR_Site_Move_II(string ArgDept, string ArgRetn, string ArgBDate, string ArgIpdDtl, ref FpSpread ssSpread, ref SheetView ssSpread_Sheet)
        {
            for (int i = 0; i < 10; i++)
                FstrSite[i] = "";

            FnPo = 0;

            for (int i = 0; i < 999; i++)
            {
                if (clsPmpaType.SA[i].SuCode.Trim() != "" && string.Compare(clsPmpaType.SA[i].Bun, "75") < 0)
                {
                    if (FnPo > 9) { return; }

                    FstrSite[FnPo] = clsPmpaType.SA[i].SuCode + VB.Right(VB.Space(8) + string.Format("{0:##0.0}", (clsPmpaType.SA[i].Qty * clsPmpaType.SA[i].Nal)), 8);
                    FnPo += 1;
                }
            }

            if (FnPo < 9)
            {
                FnPo += 1;

                if (ArgIpdDtl == "Y")
                    FstrSite[FnPo] = "◈[입원정밀수납] 영수증 보관요망";

                //2018.06.27 박병규 : 삭제요청
                //else
                //    FstrSite[FnPo] = "◈간호사실 제출요망" + " " + clsPmpaType.TOM.WardCode.Trim() + "-" + clsPmpaType.TOM.RoomCode;
            }

            if (clsPmpaPb.GstrJeaPrint != "OK")
            {
                for (int i = 0; i < 10; i++)
                    ssSpread_Sheet.Cells[22 + i, 12].Text = FstrSite[i];
            }
        }

        private void Last_Report_Amt_Gesan(ref Report_Add_Amt_Table[] RPG)
        {
            for (int i = 0; i < 30; i++)
            {
                FBAmt = 0;
                FJAmt = 0;
                FnTmp = 0;

                if (i != 22) //예약진찰제외
                {
                    Gesan_Johap(i, ref RPG); //As-is

                    if (i == 0 ) { FJAmt = FJAmt + clsPmpaPb.GnJinRP회신료;}

                    //2021-09-16 결핵관리료, 상담료 
                    if (i == 0)
                    {
                        FJAmt = FJAmt + clsPmpaPb.GnJinRP재택결핵;
                    }

                    if (i == 0 && clsPmpaPb.GnJinRP의뢰료 > 0 )
                    {
                        FBAmt = FBAmt + (long)Math.Truncate(clsPmpaPb.GnJinRP의뢰료 * 30 / 100.0);
                        FJAmt = FJAmt + (clsPmpaPb.GnJinRP의뢰료 - (long)Math.Truncate(clsPmpaPb.GnJinRP의뢰료 * 30 / 100.0));
                    }


                    if (i == 9)  { FJAmt = FJAmt + clsPmpaPb.GnJinRP검사료; }

                    if (i == 29 && clsPmpaPb.GnJinRP격리료 > 0 )
                    {
                        if (clsPmpaType.TOM.VCode == "" && clsPmpaType.TOM.MCode !="V000")
                        {
                            if (clsPmpaType.TOM.Bi == "22")
                            {
                                FBAmt = FBAmt + (long)Math.Truncate(clsPmpaPb.GnJinRP격리료 * 5 / 100.0);
                                FJAmt = FJAmt + (clsPmpaPb.GnJinRP격리료 - (long)Math.Truncate(clsPmpaPb.GnJinRP격리료 * 5 / 100.0));
                            }
                            else if(clsPmpaType.TOM.Bi == "11"|| clsPmpaType.TOM.Bi == "12"|| clsPmpaType.TOM.Bi == "13")
                            { 
                                if (clsPmpaType.TOM.MCode == "E000" || clsPmpaType.TOM.MCode == "F000")
                                {
                                    FBAmt = FBAmt + (long)Math.Truncate(clsPmpaPb.GnJinRP격리료 * 5 / 100.0);
                                    FJAmt = FJAmt + (clsPmpaPb.GnJinRP격리료 - (long)Math.Truncate(clsPmpaPb.GnJinRP격리료 * 5 / 100.0));
                                }
                                else
                                {
                                    FBAmt = FBAmt + (long)Math.Truncate(clsPmpaPb.GnJinRP격리료 * 10 / 100.0);
                                    FJAmt = FJAmt + (clsPmpaPb.GnJinRP격리료 - (long)Math.Truncate(clsPmpaPb.GnJinRP격리료 * 10 / 100.0));
                                }

                            }

                        }

                    }

                    RPG[i].Amt2 = RPG[i].Amt2;  //비급여
                    RPG[i].Amt3 = RPG[i].Amt3;  //특진
                    RPG[i].Amt4 = RPG[i].Amt4;  //본인총액
                    RPG[i].Amt7 = RPG[i].Amt7;  //선별급여 총액
                    RPG[i].Amt8 = RPG[i].Amt8;  //선별급여 조합
                    RPG[i].Amt9 = RPG[i].Amt9;  //선별급여 본인

                    if ((clsPmpaPb.GOpd_Sunap_MCode == "H000" || clsPmpaPb.GOpd_Sunap_MCode == "F000") && clsPmpaPb.GOpd_Sunap_JinDtl != "02")
                    {
                        FJAmt += FBAmt;
                        FBAmt = 0;
                    }
                    else if ((clsPmpaType.TOM.Bi == "21" || clsPmpaType.TOM.Bi == "22" ) && (clsPmpaPb.GOpd_Sunap_JinDtl != "02" && clsPmpaPb.GOpd_Sunap_JinDtl != "07"))
                    {
                        if (clsPmpaType.TOM.Bi == "21" && (i == 16 || i == 17) && clsPmpaPb.GOpd_Sunap_MCode == "M000" || clsPmpaPb.GOpd_Sunap_JinDtl == "02")
                        {
                        }
                        else if (clsPmpaType.TOM.Bi == "22" && clsPmpaPb.GOpd_Sunap_JinDtl2 == "E")
                        {
                        }
                        else if(clsPmpaType.TOM.Bi == "22")
                        {
                        }
                        else
                        {
                            FJAmt += FBAmt;
                            FBAmt = 0;
                        }
                    }

                    RPG[i].Amt5 += FBAmt;//본인부담
                    RPG[i].Amt6 += FJAmt;//공단부담
                    RPG[i].Amt7 += RPG[i].Amt4; //본인총액

                    //Sub 합
                    RPG[35].Amt1 += RPG[i].Amt5; //본인총액
                    RPG[36].Amt1 += RPG[i].Amt6; //공단부담
                    RPG[37].Amt1 += RPG[i].Amt4; //전액본인
                    RPG[38].Amt1 += RPG[i].Amt3; //선택료
                    RPG[39].Amt1 += RPG[i].Amt2; //비급여
                }
            }

            RPG[30].Amt1 += RPG[22].Amt1 + RPG[22].Amt3 + RPG[40].Amt1 + RPG[40].Amt3; //기존금액 + 예약비(선택) + 전화예약비(선택)
            RPG[32].Amt1 += RPG[22].Amt5 + RPG[22].Amt3 + RPG[40].Amt5 + RPG[40].Amt3;

            RPG[35].Amt1 += RPG[22].Amt5 + RPG[40].Amt5; //본인총액
            RPG[36].Amt1 += RPG[22].Amt6 + RPG[40].Amt6; //공단부담
            RPG[37].Amt1 += RPG[22].Amt4 + RPG[40].Amt4; //전액본인
            RPG[38].Amt1 += RPG[22].Amt3 + RPG[40].Amt3; //선택료
            RPG[39].Amt1 += RPG[22].Amt2 + RPG[40].Amt2; //비급여

            //선별급여 영수증 표시
            RPG[35].Amt1 += RPG[49].Amt9;     //선별급여 본인
            RPG[36].Amt1 += RPG[49].Amt8;     //선별급여 조합

            //이전 절사액 포함여부
            string strChk = "";

            if (RPG[32].Amt1 != VB.Fix(Convert.ToInt32(RPG[35].Amt1) / 100) * 100 && RPG[32].Amt1 != 0)
            {
                for (int i = 0; i < 30; i++)
                {
                    RPG[i].Amt3 = RPG[i].Amt3;               //특진
                    RPG[i].Amt2 = RPG[i].Amt2;               //비급여
                    RPG[i].Amt4 = RPG[i].Amt4;               //본인총액
                    RPG[i].Amt5 = RPG[i].Amt5;               //본인부담
                    RPG[i].Amt6 = RPG[i].Amt6;               //공단부담
                    RPG[i].Amt7 = RPG[i].Amt4;               //본인총액


                    if ( i != 22 ) //예약진찰제외
                    {
                        if ( i > 0 && RPG[i].Amt5 > 0 && strChk != "OK" && clsPmpaPb.GnOpd_Sunap_LastDan > 0)
                        {
                            strChk = "OK";
                            RPG[i].Amt5 += clsPmpaPb.GnOpd_Sunap_LastDan; //본인부담
                            RPG[i].Amt6 = RPG[i].Amt6;

                            //Sub 합
                            RPG[35].Amt1 += clsPmpaPb.GnOpd_Sunap_LastDan; //본인총액
                            RPG[36].Amt1 = RPG[36].Amt1;   //공단부담
                            RPG[37].Amt1 = RPG[37].Amt1;   //전액본인
                            RPG[38].Amt1 = RPG[38].Amt1;   //선택료
                            RPG[39].Amt1 = RPG[39].Amt1;   //비급여

                            //진료비총액
                            RPG[30].Amt1 += clsPmpaPb.GnOpd_Sunap_LastDan;
                        }
                    }
                }

                //이전절사 있는데 진찰만 있을경우
                if (strChk != "OK" && RPG[0].Amt5 > 0 && clsPmpaPb.GnOpd_Sunap_LastDan > 0 )
                {
                    strChk = "OK";
                    RPG[0].Amt5 += clsPmpaPb.GnOpd_Sunap_LastDan; // 본인부담
                    RPG[0].Amt6 = RPG[0].Amt6;

                    //Sub 합
                    RPG[35].Amt1 += clsPmpaPb.GnOpd_Sunap_LastDan; // 본인총액
                    RPG[36].Amt1 = RPG[36].Amt1;   //공단부담
                    RPG[37].Amt1 = RPG[37].Amt1;   //전액본인
                    RPG[38].Amt1 = RPG[38].Amt1;   //선택료
                    RPG[39].Amt1 = RPG[39].Amt1;   //비급여

                    //진료비총액
                    RPG[30].Amt1 += clsPmpaPb.GnOpd_Sunap_LastDan;
                }
            }

            if (string.Compare(clsPmpaType.TOM.Bi, "14")  < 0 && clsPmpaPb.GOpd_Sunap_MCode == "H000") //건강보험 희귀난치 지원금
            {
                RPG[36].Amt1 += RPG[35].Amt1;
                RPG[35].Amt1 -= RPG[35].Amt1;
            }
            else if (string.Compare(clsPmpaType.TOM.Bi, "14") < 0 && clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000" && (clsPmpaType.TOM.Jin == "I" || clsPmpaType.TOM.Jin == "J")) //차상위2종 정액 처리
            {
                if (clsPmpaPb.GOpd_Sunap_MCode == "V206" || clsPmpaPb.GOpd_Sunap_MCode == "V231" || clsPmpaPb.GOpd_Sunap_MCode == "V246")
                {
                    if (clsPmpaPb.GOpd_Sunap_MCode == "E000")
                    {
                        if (clsPmpaType.TOM.Jin == "I")
                        {
                            RPG[35].Amt1 = (long)Math.Truncate((RPG[35].Amt1 + 1500) * 50 / 100.0);
                            RPG[36].Amt1 += RPG[35].Amt1 - 1500;
                        }
                        else if (clsPmpaType.TOM.Jin == "J")
                        {
                            RPG[35].Amt1 = (long)Math.Truncate((RPG[35].Amt1 + 1000) * 50 / 100.0);
                            RPG[36].Amt1 += RPG[35].Amt1 - 1000;
                        }
                    }
                }
                else if (clsPmpaPb.GOpd_Sunap_MCode == "E000")
                {
                    if (clsPmpaType.TOM.Jin == "I")
                    {
                        RPG[35].Amt1 = 1500;
                        RPG[36].Amt1 -= 1500;
                    }
                    else if (clsPmpaType.TOM.Jin == "J")
                    {
                        RPG[35].Amt1 = 1000;
                        RPG[36].Amt1 -= 1000;
                    }
                }
            }
            else if (clsPmpaType.TOM.Bi == "21")
            {
                RPG[32].Amt1 += RPG[34].Amt1;
                RPG[35].Amt1 += clsPmpaPb.GOpd_Sunap_Boamt + RPG[34].Amt1;
                RPG[36].Amt1 -= clsPmpaPb.GOpd_Sunap_Boamt;
            }
            else if (string.Compare(clsPmpaType.TOM.Bi, "14") < 0 && (clsPmpaPb.GOpd_Sunap_MCode == "V206" || clsPmpaPb.GOpd_Sunap_MCode == "V231" || clsPmpaPb.GOpd_Sunap_MCode == "V246"))
            {
                RPG[35].Amt1 -= clsPmpaPb.GnOpd_Sunap_LastDan;
                RPG[35].Amt1 -= (long)Math.Truncate(RPG[35].Amt1 * 50 / 100.0);
                RPG[36].Amt1 += RPG[35].Amt1;
            }
            else if (clsPmpaType.TOM.Bi == "22")
            {
                if (clsPmpaType.TOM.Bohun == "3")
                {
                    RPG[36].Amt1 += RPG[35].Amt1;
                    RPG[35].Amt1 -= RPG[35].Amt1;
                }
            }
        }

        private void Gesan_Johap(int ArgI, ref Report_Add_Amt_Table[] RPG)
        {
            clsBasAcct CBA = new clsBasAcct();

            if (clsPmpaType.TOM.DeptCode == "DT" && string.Compare(clsPmpaType.TOM.BDate, "2017-11-01") >= 0 && clsPmpaPb.GOpd_Sunap_JinDtl == "02" && string.Compare(clsPmpaType.TOM.Bi, "22") <= 0)
            {
                if (string.Compare(clsPmpaType.TOM.Bi, "13") <= 0)
                {
                    if (ArgI == 19)
                    {
                        if (clsPmpaPb.GOpd_Sunap_MCode == "C000")
                        {
                            FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                            FBAmt += (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 5 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                            FJAmt -= (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 5 / 100.0);
                        }
                        else if (clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000")
                        {
                            FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                            FBAmt += (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 15 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                            FJAmt -= (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 15 / 100.0);
                        }
                        else
                        {
                            FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                            FBAmt += (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 30 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                            FJAmt -= (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 30 / 100.0);
                        }
                    }
                    else
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                    }
                }
                else if (clsPmpaType.TOM.Bi == "21")
                {
                    if (ArgI == 19)
                    {
                        FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                        FBAmt += (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 5 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                        FJAmt -= (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 5 / 100.0);
                    }
                    else
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                    }
                }
                else if (string.Compare(clsPmpaType.TOM.Bi, "22") <= 0)
                {
                    if (ArgI == 19)
                    {
                        FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                        FBAmt += (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 15 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                        FJAmt -= (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 15 / 100.0);
                    }
                    else
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                    }
                }
            }
            else if (clsPmpaType.TOM.DeptCode == "DT" && string.Compare(clsPmpaType.TOM.BDate, "2018-07-01") >= 0 && clsPmpaPb.GOpd_Sunap_JinDtl == "07" && string.Compare(clsPmpaType.TOM.Bi, "22") <= 0)
            {
                if (string.Compare(clsPmpaType.TOM.Bi, "13") <= 0)
                {
                    if (ArgI == 19)
                    {
                        if (clsPmpaPb.GOpd_Sunap_MCode == "C000")
                        {
                            FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                            FBAmt += (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 10 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                            FJAmt -= (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 10 / 100.0);
                        }
                        else if (clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000")
                        {
                            FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                            FBAmt += (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 20 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                            FJAmt -= (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 20 / 100.0);
                        }
                        else
                        {
                            FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                            FBAmt += (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 30 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                            FJAmt -= (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 30 / 100.0);

                        }
                    }
                    else
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                    }
                }
                else if (clsPmpaType.TOM.Bi == "21")
                {
                    if (ArgI == 19)
                    {
                        FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                        FBAmt += (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 10 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                        FJAmt -= (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 10 / 100.0);
                    }
                    else
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                    }
                }
                else if (string.Compare(clsPmpaType.TOM.Bi, "22") <= 0)
                {
                    if (ArgI == 19)
                    {
                        FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                        FBAmt += (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 20 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnToothRpAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                        FJAmt -= (long)Math.Truncate(clsPmpaPb.GnToothRpAmt * 20 / 100.0);
                    }
                    else
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                    }
                }
            }
            else if (clsPmpaType.TOM.Bi == "22" && clsPmpaType.TOM.Jin == "9") //가정간호 보호2종일경우 외래본인부담 10%
            {
                if (clsPmpaPb.GOpd_Sunap_VCode == "V194" || clsPmpaPb.GOpd_Sunap_VCode == "V247" || clsPmpaPb.GOpd_Sunap_VCode == "V248" || clsPmpaPb.GOpd_Sunap_VCode == "V249" || clsPmpaPb.GOpd_Sunap_VCode == "V250")
                {
                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                }
                else
                {
                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                }
            }
            else if (clsPmpaType.TOM.Bi == "22" && (clsPmpaType.a.Dept == "HD" || clsPmpaType.TOM.Jin == "6"))
                FJAmt = RPG[ArgI].Amt1;
            else if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && (clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000"))//차상위2 만성질환(장애인) 및 만18세미만
            {
                if (ArgI == 16 || ArgI == 17) //MRI,CT
                {
                    if( (clsPmpaPb.GOpd_Sunap_VCode == "V000" || clsPmpaPb.GOpd_Sunap_VCode == "V010") && string.Compare(clsPmpaType.TOM.BDate, "2016-07-01") >= 0)//차상위 15세 미만 본부 3%...(copy&paste한듯., 결핵/잠복결핵 본인 0%)
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                    }
                    else if (clsPmpaType.TOM.Age < 6 &&  clsPmpaPb.GOpd_Sunap_MCode == "E000" && clsPmpaPb.GstatEROVER == "*")//차상위 15세 미만 본부 3%
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                    }
                    else if (clsPmpaType.TOM.Age <= 15 && (clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000") && clsPmpaPb.GstatEROVER == "*")//차상위 15세 미만 본부 3%
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 3 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 3 / 100.0);
                    }
                    else if (clsPmpaPb.GOpd_Sunap_VCode == "V247" || clsPmpaPb.GOpd_Sunap_VCode == "V248" || clsPmpaPb.GOpd_Sunap_VCode == "V249" || clsPmpaPb.GOpd_Sunap_VCode == "V250")//중증화상 5%
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                    }
                    else if (clsPmpaPb.GOpd_Sunap_VCode == "V191" || clsPmpaPb.GOpd_Sunap_VCode == "V192" || clsPmpaPb.GOpd_Sunap_VCode == "V193" || clsPmpaPb.GOpd_Sunap_VCode == "V194") //중증환자 5 %
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                    }
                    else if (clsPmpaPb.GOpd_Sunap_VCode == "V206" || clsPmpaPb.GOpd_Sunap_VCode == "V231" || clsPmpaPb.GOpd_Sunap_VCode == "V246") //차상위2이면서 결핵 V -> EV01 MR,CT 10%
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                    }
                    else if (clsPmpaPb.GOpd_Sunap_VCode == "V193" || clsPmpaPb.GOpd_Sunap_VCode == "EV00")
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                    }
                    else if (clsPmpaType.TOM.Age <= 5 && (clsPmpaType.TOM.Jin == "R" || clsPmpaType.TOM.Jin == "S" || clsPmpaType.TOM.Jin == "T" || clsPmpaType.TOM.Jin == "U") && clsPmpaPb.GOpd_Sunap_MCode == "E000" && clsPmpaPb.GstatEROVER == "*")
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                    }
                    else if (clsPmpaType.TOM.Age <= 15 &&  (clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000") && clsPmpaPb.GstatEROVER == "*")
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 3 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 3 / 100.0);
                    }
                    else
                    {
                        if ( clsPmpaType.TOM.JinDtl == "22" || clsPmpaType.TOM.JinDtl == "25")
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                        }
                        else if (clsPmpaType.TOM.Age == 0  && string.Compare(clsPmpaType.TOM.BDate, "2019-01-01") >= 0)
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                        }
                        else
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 14 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 14 / 100.0);
                        }
                    }
                }
                else
                {
                    if (clsPmpaType.TOM.Jin == "I" || clsPmpaType.TOM.Jin == "J")
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                    }
                    else
                    {
                        if ((clsPmpaPb.GOpd_Sunap_VCode == "V000" || clsPmpaPb.GOpd_Sunap_VCode == "V010") && string.Compare(clsPmpaType.TOM.BDate, "2016-07-01") >= 0 )
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                        }
                        else if (clsPmpaType.TOM.Age == 0 && string.Compare(clsPmpaType.TOM.BDate, "2019-01-01") >= 0 &&  clsPmpaPb.GstatEROVER == "")
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                        }
                        else if (clsPmpaPb.GOpd_Sunap_VCode == "EV00")
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                        }
                        else if (clsPmpaPb.GOpd_Sunap_VCode == "EV01")
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                        }
                        else if (clsPmpaType.TOM.Age <= 5 && (clsPmpaType.TOM.Jin == "R" || clsPmpaType.TOM.Jin == "S" || clsPmpaType.TOM.Jin == "T" || clsPmpaType.TOM.Jin == "U") && clsPmpaPb.GOpd_Sunap_MCode == "E000" && clsPmpaPb.GstatEROVER == "*")
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                        }
                        else if (clsPmpaType.TOM.Age <= 15 && (clsPmpaPb.GOpd_Sunap_MCode == "E000" || clsPmpaPb.GOpd_Sunap_MCode == "F000") && clsPmpaPb.GstatEROVER == "*")
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 3 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 3 / 100.0);
                        }
                        else
                        {
                            if (clsPmpaPb.GOpd_Sunap_MCode == "F000")
                                FJAmt = RPG[ArgI].Amt1;
                            else if ( clsPmpaType.TOM.JinDtl == "22" || clsPmpaType.TOM.JinDtl == "25")
                            {
                                FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                                FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                            }
                            else if (clsPmpaType.TOM.Age == 0 && string.Compare(clsPmpaType.TOM.BDate, "2019-01-01") >= 0)
                            {
                                FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                                FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                            }
                            else
                            {
                                FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 14 / 100.0);
                                FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 14 / 100.0);
                            }
                        }
                    }
                }
            }
            else if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && (clsPmpaType.a.Dept == "HD" || clsPmpaPb.GstrOtherHD == "*" || clsPmpaType.TOM.Jin == "6" || clsPmpaType.TOM.Jin == "9" || clsPmpaType.TOM.Jin == "A" || clsPmpaPb.GstatHULWOO == "*" || clsPmpaPb.GstatEROVER == "*"))
            {
                if (clsPmpaPb.GOpd_Sunap_MCode == "C000")
                {
                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                }
                else if ((clsPmpaPb.GOpd_Sunap_VCode == "V000" || clsPmpaPb.GOpd_Sunap_VCode == "V010") && string.Compare(clsPmpaType.TOM.BDate, "2016-07-01") >= 0 )
                {
                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                }
                    
                else if (clsPmpaType.TOM.Age == 0 && clsPmpaType.TOM.JinDtl == "24" && clsPmpaPb.GstatEROVER == "*")
                {
                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                }
                else if (clsPmpaType.TOM.Age <= 15 && clsPmpaPb.GstatEROVER == "*")
                {
                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                }
                else if (clsPmpaPb.GOpd_Sunap_MCode == "V001" && clsPmpaType.TOM.Jin == "9")
                {
                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                }
                else if ((clsPmpaPb.GOpd_Sunap_MCode == "V000" || clsPmpaPb.GOpd_Sunap_MCode == "H000") && clsPmpaType.TOM.Jin == "9")
                {
                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                }
                else if ((clsPmpaPb.GOpd_Sunap_MCode == "V000" || clsPmpaPb.GOpd_Sunap_MCode == "H000") && clsPmpaType.TOM.Jin == "6")
                {
                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                }
                else if (clsPmpaPb.GOpd_Sunap_MCode == "V000" && clsPmpaType.TOM.Jin == "F" && clsPmpaPb.GstatEROVER == "*")
                {
                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                }
                else if (clsPmpaPb.GOpd_Sunap_VCode == "V247" || clsPmpaPb.GOpd_Sunap_VCode == "V248" || clsPmpaPb.GOpd_Sunap_VCode == "V249" || clsPmpaPb.GOpd_Sunap_VCode == "V250")
                {
                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                }
                else if (clsPmpaPb.GOpd_Sunap_VCode == "V194")
                {
                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100.0);
                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100.0);
                }
                else if ((clsPmpaPb.GOpd_Sunap_VCode == "V191" || clsPmpaPb.GOpd_Sunap_VCode == "V192" || clsPmpaPb.GOpd_Sunap_VCode == "V193") && clsPmpaPb.GstatEROVER == "*")
                {
                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100.0);
                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100.0);
                }
                else if (clsPmpaPb.GOpd_Sunap_VCode == "V193")
                {
                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100.0);
                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100.0);
                }
                else if (clsPmpaType.TOM.Jin == "F" && clsPmpaPb.GstatEROVER == "*")
                {
                    if (string.Compare(clsPmpaType.TOM.Bi, "24") <= 0)
                    {
                        if (ArgI == 16 || ArgI == 17)
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 50 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 50 / 100.0);
                        }
                        else
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 20 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 20 / 100.0);
                        }
                    }
                    else
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                    }
                }
                else
                {
                    if (ArgI != 16 && ArgI != 17)
                    {
                        if (clsPmpaType.TOM.Age == 0 && clsPmpaType.TOM.JinDtl == "24" && clsPmpaPb.GstatEROVER == "*")
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                        }
                        else if (clsPmpaType.TOM.Age < 6)
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                        }
                        else
                        {
                            if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaType.TOM.DeptCode == "HD" && clsPmpaType.a.Dept == "HD" && clsPmpaPb.GOpd_Sunap_MCode == "")
                            {
                                FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                            }
                            else if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaType.TOM.Jin != "9" && clsPmpaPb.GstatEROVER.Trim() == "")
                            {
                                FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                            }
                            else
                            {
                                if (clsPmpaPb.GstrHighRiskMother == "OK")
                                {
                                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                }
                                else
                                {
                                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100.0);
                                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100.0);
                                }
                            }
                        }
                    }
                    else if (ArgI == 16 || ArgI == 17)
                    {
                        if (string.Compare(clsPmpaType.TOM.BDate, clsPmpaPb.OBON_DATE) >= 0)
                        {
                            if (clsPmpaType.TOM.Age == 0 && clsPmpaType.TOM.JinDtl == "24" && clsPmpaPb.GstatEROVER == "*")
                            {
                                FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                                FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                            }
                            else if (clsPmpaType.TOM.Age < 6)
                            {
                                FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                            }
                            else
                            {
                                if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaType.TOM.DeptCode == "HD" && clsPmpaType.a.Dept == "HD" && clsPmpaPb.GOpd_Sunap_MCode == "")
                                {
                                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                }
                                else if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaType.TOM.Jin != "9" && clsPmpaPb.GstatEROVER.Trim() == "")
                                {
                                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                }
                                else
                                {
                                    if (clsPmpaPb.GstrHighRiskMother == "OK")
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                    }
                                    else
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                    }
                                }
                            }
                        }
                        else
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OLD_OBON[clsPmpaType.a.Bi] / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OLD_OBON[clsPmpaType.a.Bi] / 100.0);
                        }
                    }
                }
            }
            else if (clsPmpaType.TOM.Bi == "22" && (clsPmpaType.TOM.Jin == "I" || clsPmpaType.TOM.Jin == "J"))
            {
                if (ArgI == 16 || ArgI == 17)
                {
                    if (clsPmpaPb.GOpd_Sunap_VCode == "V247" || clsPmpaPb.GOpd_Sunap_VCode == "V248" || clsPmpaPb.GOpd_Sunap_VCode == "V249" || clsPmpaPb.GOpd_Sunap_VCode == "V250")
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                    }
                    else
                    {
                        if (clsPmpaPb.GOpd_Sunap_VCode == "V191" || clsPmpaPb.GOpd_Sunap_VCode == "V192" || clsPmpaPb.GOpd_Sunap_VCode == "V193" || clsPmpaPb.GOpd_Sunap_VCode == "V194")
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                        }
                        else
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 15 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 15 / 100.0);
                        }
                    }
                }
                else
                {
                    FJAmt = RPG[ArgI].Amt1;
                }
            }
            else if (clsPmpaType.TOM.Bi == "22" && clsPmpaType.a.Dept == "NP")
            {
                if (ArgI == 6)
                {
                    if (clsPmpaPb.GstrSPR == "OK")
                    {
                        if (string.Compare(clsPmpaType.TOM.BDate, "2021-04-01") >= 0)
                        {
                            FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPInjAmt) * 5 / 100.0);
                            FBAmt += (long)Math.Truncate(clsPmpaPb.GnNPInjAmt * 5 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPInjAmt) * 5 / 100.0);
                            FJAmt -= (long)Math.Truncate(clsPmpaPb.GnNPInjAmt * 5 / 100.0);
                        }
                        else
                        {
                            FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPInjAmt) * 5 / 100.0);
                            FBAmt += (long)Math.Truncate(clsPmpaPb.GnNPInjAmt * 10 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPInjAmt) * 5 / 100.0);
                            FJAmt -= (long)Math.Truncate(clsPmpaPb.GnNPInjAmt * 10 / 100.0);
                        }
                        
                    }
                    else
                    {
                        if (string.Compare(clsPmpaType.TOM.BDate, "2021-04-01") >= 0)
                        {
                            FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPInjAmt) * 10 / 100.0);
                            FBAmt += (long)Math.Truncate(clsPmpaPb.GnNPInjAmt * 5 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPInjAmt) * 10 / 100.0);
                            FJAmt -= (long)Math.Truncate(clsPmpaPb.GnNPInjAmt * 5 / 100.0);
                        }
                        else
                        {
                            FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPInjAmt) * 10 / 100.0);
                            FBAmt += (long)Math.Truncate(clsPmpaPb.GnNPInjAmt * 10 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPInjAmt) * 10 / 100.0);
                            FJAmt -= (long)Math.Truncate(clsPmpaPb.GnNPInjAmt * 10 / 100.0);
                        }
                         
                    }
                }
                else if (ArgI == 16 || ArgI == 17)
                {
                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 15 / 100.0);
                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 15 / 100.0);
                }
                else
                {
                    if (clsPmpaPb.GstrSPR == "")
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 15 / 100.0);
                    }
                    else if (clsPmpaPb.GstrSPR == "OK")
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 15 / 100.0);
                    }
                }
            }
            else if (clsPmpaType.TOM.Bi == "22" && clsPmpaType.TOM.JinDtl == "18")
            {
                FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
            }
            else
            {
                if (string.Compare(clsPmpaType.TOM.BDate, clsPmpaPb.OBON_DATE) >= 0)
                {
                    if (clsPmpaPb.GOpd_Sunap_MCode == "C000")
                    {
                        if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1")
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                        }
                    }
                    else if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && (clsPmpaPb.GOpd_Sunap_VCode == "V247" || clsPmpaPb.GOpd_Sunap_VCode == "V248" || clsPmpaPb.GOpd_Sunap_VCode == "V249" || clsPmpaPb.GOpd_Sunap_VCode == "V250"))
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                    }
                    else if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && (clsPmpaPb.GOpd_Sunap_VCode == "V191" || clsPmpaPb.GOpd_Sunap_VCode == "V192" || clsPmpaPb.GOpd_Sunap_VCode == "V193" || clsPmpaPb.GOpd_Sunap_VCode == "V194"))
                    {
                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                    }
                    else if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaPb.GOpd_Sunap_MCode == "V000")
                    {
                        if ((clsPmpaPb.GOpd_Sunap_VCode == "V000" || clsPmpaPb.GOpd_Sunap_VCode == "V010" ) && string.Compare(clsPmpaType.TOM.BDate, "2016-07-01") >= 0)
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);  
                        }
                        else if (clsPmpaType.TOM.JinDtl == "22" && string.Compare(clsPmpaType.TOM.BDate, "2020-01-01") >= 0)
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                        }
                        else
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                        }
                        
                    }
                    else if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaType.TOM.Age >= 6 && clsPmpaType.TOM.Jin == "C" && clsPmpaPb.GOpd_Sunap_MCode == "H000")
                    {
                        if (ArgI == 16 || ArgI == 17)
                        {
                            if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaPb.GOpd_Sunap_MCode == "H000")
                            {
                                FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                            }
                            else
                            {
                                FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 50 / 100.0);
                                FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 50 / 100.0);
                            }
                        }
                        else
                        {
                            if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1"  && clsPmpaPb.GOpd_Sunap_MCode == "H000")
                            {
                                FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                            }
                            else
                            {
                                FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 20 / 100.0);
                                FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 20 / 100.0);
                            }
                        }
                    }
                    else if (clsPmpaType.TOM.Age >= 6 && (clsPmpaType.TOM.Jin == "F" || clsPmpaType.TOM.Jin == "G"))
                    {
                        if (string.Compare(clsPmpaType.TOM.Bi, "24") <= 0)
                        {
                            if (ArgI == 16 || ArgI == 17)
                            {
                                if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaPb.GOpd_Sunap_MCode == "H000")
                                {
                                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                }
                                else
                                {
                                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 50 / 100.0);
                                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 50 / 100.0);
                                }
                            }
                            else
                            {
                                if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaPb.GOpd_Sunap_MCode == "H000")
                                {
                                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                }
                                else
                                {
                                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 20 / 100.0);
                                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 20 / 100.0);
                                }
                            }
                        }
                        else
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                        }
                    }
                    else if (clsPmpaType.TOM.Age <= 5 && VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaType.a.Dept != "NP" &&(clsPmpaType.TOM.Jin == "R" || clsPmpaType.TOM.Jin == "S" || clsPmpaType.TOM.Jin == "T" || clsPmpaType.TOM.Jin == "U"))
                    {
                        if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaPb.GOpd_Sunap_MCode == "H000")
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                        }
                        else if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaType.TOM.JinDtl == "22" )
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                        }
                        else
                        {
                            if (clsPmpaType.TOM.Jin == "R" || clsPmpaType.TOM.Jin == "U")
                            {

                                if (clsPmpaType.TOM.JinDtl == "22"  && string.Compare(clsPmpaType.TOM.BDate, "2020-01-01") >= 0)
                                {
                                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                                }

                                else if( clsPmpaType.TOM.JinDtl == "22")
                                {
                                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                }
                                else if (clsPmpaType.TOM.Age == 0  && string.Compare(clsPmpaType.TOM.BDate, "2019-01-01") >= 0)
                                {
                                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 15 / 100.0);
                                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 15 / 100.0);
                                }
                                else if (clsPmpaType.TOM.VCode =="F003")
                                {
                                    if (ArgI == 4)//약품비만
                                    {
                                        FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnDrugRPAmt) * 35 / 100.0);
                                        FBAmt += (long)Math.Truncate(clsPmpaPb.GnDrugRPAmt * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnDrugRPAmt) * 35 / 100.0);
                                        FJAmt -= (long)Math.Truncate(clsPmpaPb.GnDrugRPAmt * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100.0);
                                    }
                                    else
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 35 / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 35 / 100.0);
                                    }
                 
                                }
                                else
                                {
                                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 35 / 100.0);
                                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 35 / 100.0);
                                }
                            }
                            else if (clsPmpaType.TOM.Jin == "S" || clsPmpaType.TOM.Jin == "T")
                            {
                                if (ArgI == 16 || ArgI == 17)
                                {
                                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 35 / 100.0);
                                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 35 / 100.0);
                                }
                                else
                                {
                                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 14 / 100.0);
                                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 14 / 100.0);
                                }
                            }
                        }
                    }
                    //2018.06.25 박병규 : 개인정신치료코드금액이 있을경우
                    else if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaType.a.Dept == "NP" && clsPmpaPb.GOpd_Sunap_VCode != "F003")
                    {
                        if (ArgI == 14)
                        {
                            if (clsPmpaType.TOM.Age < 6)
                            {
                                FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPNnAmt) * 35 / 100.0);
                                FBAmt += (long)Math.Truncate(clsPmpaPb.GnNPNnAmt * 21 / 100.0);
                                FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPNnAmt) * 35 / 100.0);
                                FJAmt -= (long)Math.Truncate(clsPmpaPb.GnNPNnAmt * 21 / 100.0);
                            }
                            else
                            {
                                FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPNnAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                FBAmt += (long)Math.Truncate(clsPmpaPb.GnNPNnAmt * 30 / 100.0);
                                FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPNnAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                FJAmt -= (long)Math.Truncate(clsPmpaPb.GnNPNnAmt * 30 / 100.0);
                            }
                        }
                        else
                        {
                            // FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                            // FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                            if (clsPmpaType.TOM.Age < 6)
                            {
                                FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 ) * 35 / 100.0);
                                FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 ) * 35 / 100.0);
                              
                            }
                            else
                            {
                                 FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                 FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                            }
                        }
                    }
                    else
                    {
                        if (clsPmpaType.TOM.Bi == "21" && (ArgI == 16 || ArgI == 17) && clsPmpaPb.GstatEROVER == "*"  )
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                        }
                        else  if (clsPmpaType.TOM.Bi == "21" && (ArgI == 16 || ArgI == 17) && clsPmpaPb.GOpd_Sunap_MCode == "M000")
                        {
                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                        }
                        else
                        {
                            switch (clsPmpaPb.GOpd_Sunap_VCode)
                            {
                                case "V191":
                                case "V192":
                                case "V193":
                                case "V194":

                                    if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaType.TOM.Bi == "22")
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100.0);
                                    }
                                    else if (clsPmpaType.TOM.Bi == "21")
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                    }

                                    break;

                                case "V247":
                                case "V248":
                                case "V249":
                                case "V250":

                                    if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaType.TOM.Bi == "22")
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                                    }
                                    else
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                    }

                                    break;

                                case "F003":

                                    if (ArgI == 4)//약품비만
                                    {
                                        FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnDrugRPAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi]  / 100.0);
                                        FBAmt += (long)Math.Truncate(clsPmpaPb.GnDrugRPAmt * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnDrugRPAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                        FJAmt -= (long)Math.Truncate(clsPmpaPb.GnDrugRPAmt * CBA.Read_Cancer_BonRate(clsPmpaType.TOM.Bi, clsPmpaType.TOM.BDate, clsPmpaPb.GOpd_Sunap_VCode) / 100.0);
                                    }
                                    else if (ArgI == 14 )
                                    {
                                        FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPNnAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                        FBAmt += (long)Math.Truncate(clsPmpaPb.GnNPNnAmt * 30 / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPNnAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                        FJAmt -= (long)Math.Truncate(clsPmpaPb.GnNPNnAmt * 30 / 100.0);

                                    }
                                    else
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                    }

                                    break;

                                default:

                                    if (clsPmpaType.TOM.Bi == "22" && clsPmpaType.TOM.Age <= 5 && (clsPmpaPb.GstatEROVER == "*" || clsPmpaPb.GOpd_Sunap_JinDtl2 == "E"))
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                                    }
                                    else if (clsPmpaType.TOM.Bi == "22" && clsPmpaType.TOM.Age <= 15 && (clsPmpaPb.GstatEROVER == "*" || clsPmpaPb.GOpd_Sunap_JinDtl2 == "E"))
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 3 / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 3 / 100.0);
                                    }
                                    else if (clsPmpaPb.GstatEROVER == "*" || clsPmpaPb.GOpd_Sunap_JinDtl2 == "E")
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100.0);
                                    }
                                    else if (clsPmpaType.TOM.Bi == "22" && (clsPmpaPb.GstatEROVER == "*" || clsPmpaPb.GOpd_Sunap_JinDtl2 == "E"))
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100.0);
                                    }
                                    else if (clsPmpaType.TOM.Bi == "22" && clsPmpaType.TOM.Age == 0 && string.Compare(clsPmpaType.TOM.BDate, "2019-01-01") >= 0)
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.IBON[clsPmpaType.a.Bi] / 100.0);
                                    }
                                    else if (clsPmpaType.TOM.Bi == "22" && ( clsPmpaType.TOM.JinDtl == "22" || clsPmpaType.TOM.JinDtl == "25"))
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                                    }
                                    else if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && (clsPmpaPb.GstrOpdMother == "OK" || clsPmpaType.TOM.JinDtl == "25"))
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 30 / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 30 / 100.0);
                                    }
                                    else if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && (clsPmpaType.TOM.JinDtl == "22") && string.Compare(clsPmpaType.TOM.BDate, "2020-01-01") >= 0 )
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 5 / 100.0);
                                    }
                                    else if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && ( clsPmpaType.TOM.JinDtl == "22"))
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 10 / 100.0);
                                    }
                                    else if (clsPmpaType.TOM.Bi == "21" && clsPmpaType.TOM.DeptCode == "NP")
                                    {
                                        if (ArgI == 6)
                                        {
                                            if (clsPmpaPb.GstrSPR == "OK")
                                            {
                                                if (string.Compare(clsPmpaType.TOM.BDate, "2021-04-01") >= 0)
                                                {
                                                    FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPInjAmt) * 0 / 100.0);
                                                    FBAmt += (long)Math.Truncate(clsPmpaPb.GnNPInjAmt * 5 / 100.0);
                                                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPInjAmt) * 0 / 100.0);
                                                    FJAmt -= (long)Math.Truncate(clsPmpaPb.GnNPInjAmt * 5 / 100.0);
                                                }
                                                else
                                                {
                                                    FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPInjAmt) * 0 / 100.0);
                                                    FBAmt += (long)Math.Truncate(clsPmpaPb.GnNPInjAmt * 10 / 100.0);
                                                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPInjAmt) * 0 / 100.0);
                                                    FJAmt -= (long)Math.Truncate(clsPmpaPb.GnNPInjAmt * 10 / 100.0);
                                                }
                                                
                                            }
                                            else
                                            {
                                                FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPInjAmt) * 0 / 100.0);
                                                FBAmt += (long)Math.Truncate(clsPmpaPb.GnNPInjAmt * 0 / 100.0);
                                                FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnNPInjAmt) * 0 / 100.0);
                                                FJAmt -= (long)Math.Truncate(clsPmpaPb.GnNPInjAmt * 0 / 100.0);
                                            }
                                        }
                                        else
                                        {
                                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * 0 / 100.0);
                                        }
                                    }
                                    else if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaType.TOM.JinDtl == "01")
                                    {
                                        if (ArgI == 8)
                                        {
                                            FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnSpecAmt) * 20 / 100.0);
                                            FBAmt += (long)Math.Truncate(clsPmpaPb.GnSpecAmt * 20 / 100.0);
                                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnSpecAmt) * 20 / 100.0);
                                            FJAmt -= (long)Math.Truncate(clsPmpaPb.GnSpecAmt * 20 / 100.0);
                                        }
                                        else
                                        {
                                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                        }
                                    }
                                    else if (VB.Left(clsPmpaType.TOM.Bi, 1) == "1" && clsPmpaType.TOM.DeptCode == "DT" && clsPmpaType.TOM.Age <= 18)
                                    {
                                        if (ArgI == 8)
                                        {
                                            FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnDtTrmAmt) * 50 / 100.0);
                                            FBAmt += (long)Math.Truncate(clsPmpaPb.GnDtTrmAmt * 10 / 100.0);
                                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnDtTrmAmt) * 50 / 100.0);
                                            FJAmt -= (long)Math.Truncate(clsPmpaPb.GnDtTrmAmt * 10 / 100.0);
                                        }
                                        else
                                        {
                                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                        }
                                    }
                                    else if (clsPmpaType.TOM.Bi == "22" && clsPmpaType.TOM.DeptCode == "DT" && clsPmpaType.TOM.Age <= 18)
                                    {
                                        if (ArgI == 8)
                                        {
                                            FBAmt = (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnDtTrmAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                            FBAmt += (long)Math.Truncate(clsPmpaPb.GnDtTrmAmt * 5 / 100.0);
                                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate((RPG[ArgI].Amt1 - clsPmpaPb.GnDtTrmAmt) * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                            FJAmt -= (long)Math.Truncate(clsPmpaPb.GnDtTrmAmt * 5 / 100.0);
                                        }
                                        else
                                        {
                                            FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                            FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                        }
                                    }
                                    else
                                    {
                                        FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                        FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OBON[clsPmpaType.a.Bi] / 100.0);
                                    }

                                    break;
                            }
                        }
                    }
                }
                else
                {
                    FBAmt = (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OLD_OBON[clsPmpaType.a.Bi] / 100.0);
                    FJAmt = RPG[ArgI].Amt1 - (long)Math.Truncate(RPG[ArgI].Amt1 * clsPmpaPb.OLD_OBON[clsPmpaType.a.Bi] / 100.0);
                }
            }
        }

        #region 외래 영수증 급여/비급여 분류
        private void Report_Slip_AmtAdd(int ArgCnt, ref Slip_ReportPrint_Table[] RP, ref Report_Add_Amt_Table[] RPG)
        {
            long n100BAmt = 0; //100/100본인부담금
            long n100JAmt = 0; //100/100조합부담금

            //진료비총액
            if (RP[ArgCnt].Bun == "99" || RP[ArgCnt].Bun == "98" || RP[ArgCnt].Bun == "96" || RP[ArgCnt].Bun == "92")
            {
                RPG[30].Amt1 += RP[ArgCnt].Amt1;
                RPG[30].Amt1 += RP[ArgCnt].Amt2;
            }

            //공단부담총액
            if (RP[ArgCnt].Bun == "98")
            {
                RPG[31].Amt1 += RP[ArgCnt].Amt1;
                RPG[31].Amt1 += RP[ArgCnt].Amt2;
            }

            //본인부담총액
            if (RP[ArgCnt].Bun == "99")
            {
                RPG[32].Amt1 += RP[ArgCnt].Amt1;
                RPG[32].Amt1 += RP[ArgCnt].Amt2;
            }

            //감액
            if (RP[ArgCnt].Bun == "92")
            {
                RPG[33].Amt1 += RP[ArgCnt].Amt1;
                RPG[33].Amt1 += RP[ArgCnt].Amt2;
            }
            //미수
            else if (RP[ArgCnt].Bun == "96")
            {
                RPG[34].Amt1 += RP[ArgCnt].Amt1;
                RPG[34].Amt1 += RP[ArgCnt].Amt2;
            }
            else if (string.Compare(RP[ArgCnt].Bun, "85") < 0)
            {
                n100BAmt = 0;
                n100JAmt = 0;

                //건강보험, 의료급여만 정산
                if (string.Compare(RP[ArgCnt].Bi, "30") < 0)
                {
                    if (RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "6")
                    {
                        n100BAmt = (long)Math.Truncate(RP[ArgCnt].Amt1 * (80 / 100.00));
                        n100JAmt = RP[ArgCnt].Amt1 - n100BAmt;
                        RPG[49].Amt7 += RP[ArgCnt].Amt1; //선별급여 총액
                        RPG[49].Amt8 += n100JAmt;        //선별급여 조합
                        RPG[49].Amt9 += n100BAmt;        //선별급여 본인
                    }
                    else if (RP[ArgCnt].SugbS == "3" )
                    {
                        n100BAmt = (long)Math.Truncate(RP[ArgCnt].Amt1 * (30 / 100.00));
                        n100JAmt = RP[ArgCnt].Amt1 - n100BAmt;
                        RPG[49].Amt7 += RP[ArgCnt].Amt1; //선별급여 총액
                        RPG[49].Amt8 += n100JAmt;        //선별급여 조합
                        RPG[49].Amt9 += n100BAmt;        //선별급여 본인
                    }

                    else if (RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9")
                    {
                        n100BAmt = (long)Math.Truncate(RP[ArgCnt].Amt1 * (90 / 100.00));
                        n100JAmt = RP[ArgCnt].Amt1 - n100BAmt;
                        RPG[49].Amt7 += RP[ArgCnt].Amt1; //선별급여 총액
                        RPG[49].Amt8 += n100JAmt;        //선별급여 조합
                        RPG[49].Amt9 += n100BAmt;        //선별급여 본인
                    }

                    else if (RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "7")
                    {
                        n100BAmt = (long)Math.Truncate(RP[ArgCnt].Amt1 * (50 / 100.00));
                        n100JAmt = RP[ArgCnt].Amt1 - n100BAmt;
                        RPG[49].Amt7 += RP[ArgCnt].Amt1; //선별급여 총액
                        RPG[49].Amt8 += n100JAmt;        //선별급여 조합
                        RPG[49].Amt9 += n100BAmt;        //선별급여 본인
                    }
                    else if (RP[ArgCnt].SugbS == "2" )
                    {
                        n100BAmt = (long)Math.Truncate(RP[ArgCnt].Amt1 * (20 / 100.00));
                        n100JAmt = RP[ArgCnt].Amt1 - n100BAmt;
                        RPG[49].Amt7 += RP[ArgCnt].Amt1; //선별급여 총액
                        RPG[49].Amt8 += n100JAmt;        //선별급여 조합
                        RPG[49].Amt9 += n100BAmt;        //선별급여 본인
                    }

                    else
                    {
                        n100BAmt = 0;
                        n100JAmt = 0;
                    }
                }


                switch (RP[ArgCnt].Bun)
                {
                    case "01"://진찰료
                    case "02":
                    case "03":
                    case "04":
                        R_Amt_Add_00(n100BAmt, n100JAmt, ArgCnt, ref RP, ref RPG);
                        break;

                    case "11"://약
                    case "12":
                    case "13":
                    case "14":
                    case "15":
                        R_Amt_Add_03(n100BAmt, n100JAmt, ArgCnt, ref RP, ref RPG);
                        break;

                    case "16"://주사
                    case "17":
                    case "18":
                    case "19":
                    case "20":
                    case "21":
                        R_Amt_Add_05(n100BAmt, n100JAmt, ArgCnt, ref RP, ref RPG);
                        break;

                    case "22"://마취
                    case "23":
                        R_Amt_Add_07(n100BAmt, n100JAmt, ArgCnt, ref RP, ref RPG);
                        break;

                    case "28"://처치수술
                    case "29":
                    case "30":
                    case "31":
                    case "32":
                    case "33":
                    case "34":
                    case "35":
                    case "36":
                    case "38":
                    case "39":
                        R_Amt_Add_08(n100BAmt, n100JAmt, ArgCnt, ref RP, ref RPG);
                        break;

                    case "41"://검사
                    case "42":
                    case "43":
                    case "44":
                    case "45":
                    case "46":
                    case "47":
                    case "48":
                    case "49":
                    case "50":
                    case "51":
                    case "52":
                    case "53":
                    case "54":
                    case "55":
                    case "56":
                    case "57":
                    case "58":
                    case "59":
                    case "60":
                    case "61":
                    case "62":
                    case "63":
                    case "64":
                        R_Amt_Add_09(n100BAmt, n100JAmt, ArgCnt, ref RP, ref RPG);
                        break;

                    case "65"://XRAY
                    case "66":
                    case "67":
                    case "68":
                    case "69":
                    case "70":
                        R_Amt_Add_10(n100BAmt, n100JAmt, ArgCnt, ref RP, ref RPG);
                        break;

                    case "24"://물리치료
                    case "25":
                        R_Amt_Add_13(n100BAmt, n100JAmt, ArgCnt, ref RP, ref RPG);
                        break;

                    case "26"://정신요법
                    case "27":
                        R_Amt_Add_14(n100BAmt, n100JAmt, ArgCnt, ref RP, ref RPG);
                        break;

                    case "37"://수혈
                        R_Amt_Add_15(n100BAmt, n100JAmt, ArgCnt, ref RP, ref RPG);
                        break;

                    case "72"://CT
                        R_Amt_Add_16(n100BAmt, n100JAmt, ArgCnt, ref RP, ref RPG);
                        break;

                    case "73"://MRI
                        R_Amt_Add_17(n100BAmt, n100JAmt, ArgCnt, ref RP, ref RPG);
                        break;

                    case "71"://초음파
                        R_Amt_Add_18(n100BAmt, n100JAmt, ArgCnt, ref RP, ref RPG);
                        break;

                    case "40"://보철료
                        R_Amt_Add_19(n100BAmt, n100JAmt, ArgCnt, ref RP, ref RPG);
                        break;

                    case "75"://증명료
                        R_Amt_Add_21(n100BAmt, n100JAmt, ArgCnt, ref RP, ref RPG);
                        break;

                    default://기타
                        R_Amt_Add_29(n100BAmt, n100JAmt, ArgCnt, ref RP, ref RPG);
                        break;
                }
            }
        }

        private void R_Amt_Add_29(long ArgB, long ArgJ, int ArgCnt, ref Slip_ReportPrint_Table[] RP, ref Report_Add_Amt_Table[] RPG)
        {
            if (clsPmpaType.TOM.Bi == "52")
            {
                if (RP[ArgCnt].GbSelf == "1")
                {
                    RPG[29].Amt2 += RP[ArgCnt].Amt1;   //비급여
                    RPG[29].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
                else if (RP[ArgCnt].GbSelf == "2")
                {
                    RPG[29].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    RPG[29].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                }
                else
                {
                    RPG[29].Amt1 += RP[ArgCnt].Amt1;   //보험합
                    RPG[29].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
            }
            else
            {
                if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" || RP[ArgCnt].SugbS == "3" ||  RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
                {
                    RPG[29].Amt7 += (ArgB + ArgJ); //총액
                    RPG[29].Amt8 += ArgJ; //조합부담
                    RPG[29].Amt9 += ArgB; //조합부담
                    RPG[29].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
                else
                {
                    if (RP[ArgCnt].GbSelf == "1")
                    {
                        RPG[29].Amt2 += RP[ArgCnt].Amt1;   //비급여
                        RPG[29].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    }
                    else if (RP[ArgCnt].GbSelf == "2")
                    {
                        RPG[29].Amt3 += RP[ArgCnt].Amt2;   //특진합
                        RPG[29].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                    }
                    else
                    {
                        RPG[29].Amt1 += RP[ArgCnt].Amt1;   //보험합
                        RPG[29].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    }
                   
                }
              
            }
            if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SuCode == "V6001" || RP[ArgCnt].SuCode == "V6002" || RP[ArgCnt].SuCode == "AH001" || RP[ArgCnt].SuCode == "AH002"))
            {
                if (clsPmpaType.TOM.VCode == "" && clsPmpaType.TOM.MCode != "V000")
                {
                    clsPmpaPb.GnJinRP격리료 = clsPmpaPb.GnJinRP격리료 + RP[ArgCnt].Amt1;
                    RPG[29].Amt1 = RPG[29].Amt1 - RP[ArgCnt].Amt1;   //보험합
                    RPG[29].Amt3 = RPG[29].Amt3 - RP[ArgCnt].Amt2;   //보험합
                }
                
  
            }
        }

        private void R_Amt_Add_21(long ArgB, long ArgJ, int ArgCnt, ref Slip_ReportPrint_Table[] RP, ref Report_Add_Amt_Table[] RPG)
        {
            if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" || RP[ArgCnt].SugbS == "3" ||  RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
            {
                RPG[21].Amt7 += (ArgB + ArgJ); //총액
                RPG[21].Amt8 += ArgJ; //조합부담
                RPG[21].Amt9 += ArgB; //조합부담
                RPG[21].Amt3 += RP[ArgCnt].Amt2;   //특진합
            }
            else
            {
                if (RP[ArgCnt].GbSelf == "1")
                {
                    RPG[21].Amt2 += RP[ArgCnt].Amt1;   //비급여
                    RPG[21].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
                else if (RP[ArgCnt].GbSelf == "2")
                {
                    RPG[21].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    RPG[21].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                }
                else
                {
                    RPG[21].Amt1 += RP[ArgCnt].Amt1;   //보험합
                    RPG[21].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
            }
        }

        private void R_Amt_Add_19(long ArgB, long ArgJ, int ArgCnt, ref Slip_ReportPrint_Table[] RP, ref Report_Add_Amt_Table[] RPG)
        {
            if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" || RP[ArgCnt].SugbS == "3" ||  RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
            {
                RPG[19].Amt7 += (ArgB + ArgJ); //총액
                RPG[19].Amt8 += ArgJ; //조합부담
                RPG[19].Amt9 += ArgB; //조합부담
                RPG[19].Amt3 += RP[ArgCnt].Amt2;   //특진합
            }
            else
            {
                if (RP[ArgCnt].GbSelf == "1")
                {
                    RPG[19].Amt2 += RP[ArgCnt].Amt1;   //비급여
                    RPG[19].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
                else if (RP[ArgCnt].GbSelf == "2")
                {
                    RPG[19].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    RPG[19].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                }
                else
                {
                    RPG[19].Amt1 += RP[ArgCnt].Amt1;   //보험합
                    RPG[19].Amt3 += RP[ArgCnt].Amt2;   //특진합

                    clsPmpaPb.GnToothRpAmt += RP[ArgCnt].Amt1; //노인틀니
                }
            }
        }

        private void R_Amt_Add_18(long ArgB, long ArgJ, int ArgCnt, ref Slip_ReportPrint_Table[] RP, ref Report_Add_Amt_Table[] RPG)
        {
            if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" || RP[ArgCnt].SugbS == "3" ||  RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
            {
                RPG[18].Amt7 += (ArgB + ArgJ); //총액
                RPG[18].Amt8 += ArgJ; //조합부담
                RPG[18].Amt9 += ArgB; //조합부담
                RPG[18].Amt3 += RP[ArgCnt].Amt2;   //특진합
            }
            else
            {
                if (RP[ArgCnt].GbSelf == "1")
                {
                    RPG[18].Amt2 += RP[ArgCnt].Amt1;   //비급여
                    RPG[18].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
                else if (RP[ArgCnt].GbSelf == "2")
                {
                    RPG[18].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    RPG[18].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                }
                else
                {
                    RPG[18].Amt1 += RP[ArgCnt].Amt1;   //보험합
                    RPG[18].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
            }
        }

        private void R_Amt_Add_17(long ArgB, long ArgJ, int ArgCnt, ref Slip_ReportPrint_Table[] RP, ref Report_Add_Amt_Table[] RPG)
        {
            if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" || RP[ArgCnt].SugbS == "3" ||  RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
            {
                RPG[17].Amt7 += (ArgB + ArgJ); //총액
                RPG[17].Amt8 += ArgJ; //조합부담
                RPG[17].Amt9 += ArgB; //조합부담
                RPG[17].Amt3 += RP[ArgCnt].Amt2;   //특진합
            }
            else
            {
                if (RP[ArgCnt].GbSelf == "1")
                {
                    RPG[17].Amt2 += RP[ArgCnt].Amt1;   //비급여
                    RPG[17].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
                else if (RP[ArgCnt].GbSelf == "2")
                {
                    RPG[17].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    RPG[17].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                }
                else
                {
                    RPG[17].Amt1 += RP[ArgCnt].Amt1;   //보험합
                    RPG[17].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
            }
        }

        private void R_Amt_Add_16(long ArgB, long ArgJ, int ArgCnt, ref Slip_ReportPrint_Table[] RP, ref Report_Add_Amt_Table[] RPG)
        {
            if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" || RP[ArgCnt].SugbS == "3" ||  RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
            {
                RPG[16].Amt7 += (ArgB + ArgJ); //총액
                RPG[16].Amt8 += ArgJ; //조합부담
                RPG[16].Amt9 += ArgB; //조합부담
                RPG[16].Amt3 += RP[ArgCnt].Amt2;   //특진합
            }
            else
            {
                if (RP[ArgCnt].GbSelf == "1")
                {
                    RPG[16].Amt2 += RP[ArgCnt].Amt1;   //비급여
                    RPG[16].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
                else if (RP[ArgCnt].GbSelf == "2")
                {
                    RPG[16].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    RPG[16].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                }
                else
                {
                    RPG[16].Amt1 += RP[ArgCnt].Amt1;   //보험합
                    RPG[16].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
            }
        }

        private void R_Amt_Add_15(long ArgB, long ArgJ, int ArgCnt, ref Slip_ReportPrint_Table[] RP, ref Report_Add_Amt_Table[] RPG)
        {
            if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" || RP[ArgCnt].SugbS == "3" ||  RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
            {
                RPG[15].Amt7 += (ArgB + ArgJ); //총액
                RPG[15].Amt8 += ArgJ; //조합부담
                RPG[15].Amt9 += ArgB; //조합부담
                RPG[15].Amt3 += RP[ArgCnt].Amt2;   //특진합
            }
            else
            {
                if (RP[ArgCnt].GbSelf == "1")
                {
                    RPG[15].Amt2 += RP[ArgCnt].Amt1;   //비급여
                    RPG[15].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
                else if (RP[ArgCnt].GbSelf == "2")
                {
                    RPG[15].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    RPG[15].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                }
                else
                {
                    RPG[15].Amt1 += RP[ArgCnt].Amt1;   //보험합
                    RPG[15].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
            }
        }

        private void R_Amt_Add_14(long ArgB, long ArgJ, int ArgCnt, ref Slip_ReportPrint_Table[] RP, ref Report_Add_Amt_Table[] RPG)
        {
            if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" || RP[ArgCnt].SugbS == "3" ||  RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
            {
                RPG[14].Amt7 += (ArgB + ArgJ); //총액
                RPG[14].Amt8 += ArgJ; //조합부담
                RPG[14].Amt9 += ArgB; //조합부담
                RPG[14].Amt3 += RP[ArgCnt].Amt2;   //특진합
            }
            else
            {
                if (RP[ArgCnt].GbSelf == "1")
                {
                    RPG[14].Amt2 += RP[ArgCnt].Amt1;   //비급여
                    RPG[14].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
                else if (RP[ArgCnt].GbSelf == "2")
                {
                    RPG[14].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    RPG[14].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                }
                else
                {
                    RPG[14].Amt1 += RP[ArgCnt].Amt1;   //보험합
                    RPG[14].Amt3 += RP[ArgCnt].Amt2;   //특진합

                    if (RP[ArgCnt].SuNext == "NN001" || RP[ArgCnt].SuNext == "NN002" || RP[ArgCnt].SuNext == "NN003" || RP[ArgCnt].SuNext == "NN004" || RP[ArgCnt].SuNext == "NN005")
                        clsPmpaPb.GnNPNnAmt += RP[ArgCnt].Amt1;
                }
            }
        }

        private void R_Amt_Add_13(long ArgB, long ArgJ, int ArgCnt, ref Slip_ReportPrint_Table[] RP, ref Report_Add_Amt_Table[] RPG)
        {
            if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" || RP[ArgCnt].SugbS == "3" ||  RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
            {
                RPG[13].Amt7 += (ArgB + ArgJ); //총액
                RPG[13].Amt8 += ArgJ; //조합부담
                RPG[13].Amt9 += ArgB; //조합부담
                RPG[13].Amt3 += RP[ArgCnt].Amt2;   //특진합
            }
            else
            {
                if (RP[ArgCnt].GbSelf == "1")
                {
                    RPG[13].Amt2 += RP[ArgCnt].Amt1;   //비급여
                    RPG[13].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
                else if (RP[ArgCnt].GbSelf == "2")
                {
                    RPG[13].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    RPG[13].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                }
                else
                {
                    RPG[13].Amt1 += RP[ArgCnt].Amt1;   //보험합
                    RPG[13].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
            }
        }

        private void R_Amt_Add_10(long ArgB, long ArgJ, int ArgCnt, ref Slip_ReportPrint_Table[] RP, ref Report_Add_Amt_Table[] RPG)
        {
            if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" ||  RP[ArgCnt].SugbS == "3" ||  RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
            {
                RPG[10].Amt7 += (ArgB + ArgJ); //총액
                RPG[10].Amt8 += ArgJ; //조합부담
                RPG[10].Amt9 += ArgB; //조합부담
                RPG[10].Amt3 += RP[ArgCnt].Amt2;   //특진합
            }
            else
            {
                if (RP[ArgCnt].GbSelf == "1")
                {
                    RPG[10].Amt2 += RP[ArgCnt].Amt1;   //비급여
                    RPG[10].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
                else if (RP[ArgCnt].GbSelf == "2")
                {
                    RPG[10].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    RPG[10].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                }
                else
                {
                    RPG[10].Amt1 += RP[ArgCnt].Amt1;   //보험합
                    RPG[10].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
            }
        }

        private void R_Amt_Add_09(long ArgB, long ArgJ, int ArgCnt, ref Slip_ReportPrint_Table[] RP, ref Report_Add_Amt_Table[] RPG)
        {
            if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" ||  RP[ArgCnt].SugbS == "3" ||  RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
            {
                RPG[9].Amt7 += (ArgB + ArgJ); //총액
                RPG[9].Amt8 += ArgJ; //조합부담
                RPG[9].Amt9 += ArgB; //조합부담
                RPG[9].Amt3 += RP[ArgCnt].Amt2;   //특진합
            }
            else
            {
                if (RP[ArgCnt].GbSelf == "1")
                {
                    RPG[9].Amt2 += RP[ArgCnt].Amt1;   //비급여
                    RPG[9].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
                else if (RP[ArgCnt].GbSelf == "2")
                {
                    RPG[9].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    RPG[9].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                }
                else
               


                {
                    RPG[9].Amt1 += RP[ArgCnt].Amt1;   //보험합
                    RPG[9].Amt3 += RP[ArgCnt].Amt2;   //특진합

                    if ( ( RP[ArgCnt].SuCode == "NCOV-1" || RP[ArgCnt].SuCode == "COV-FLU" ||  RP[ArgCnt].SuCode == "NCOV-P2")  && string.Compare(RP[ArgCnt].Bi, "30") < 0)
                    {
                        clsPmpaPb.GnJinRP검사료 = clsPmpaPb.GnJinRP검사료 + RP[ArgCnt].Amt1;
                        RPG[9].Amt1 -= RP[ArgCnt].Amt1;   //보험합
                        RPG[9].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    }

                    if ((RP[ArgCnt].SuCode == "F022-TB5" || RP[ArgCnt].SuCode == "FTB-6A1" || RP[ArgCnt].SuCode == "FTB-6B1" || RP[ArgCnt].SuCode == "F022-TB7") && clsPmpaType.TOM.JinDtl.Trim() =="29")
                    {
                        clsPmpaPb.GnJinRP검사료 = clsPmpaPb.GnJinRP검사료 + RP[ArgCnt].Amt1;
                        RPG[9].Amt1 -= RP[ArgCnt].Amt1;   //보험합
                        RPG[9].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    }




                }
            }
        }

        private void R_Amt_Add_08(long ArgB, long ArgJ, int ArgCnt, ref Slip_ReportPrint_Table[] RP, ref Report_Add_Amt_Table[] RPG)
        {
            if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" ||  RP[ArgCnt].SugbS == "3" ||  RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
            {
                RPG[8].Amt7 += (ArgB + ArgJ); //선별급여총액
                RPG[8].Amt8 += ArgJ; //선별조합부담
                RPG[8].Amt9 += ArgB; //선별조합부담
                RPG[8].Amt3 += RP[ArgCnt].Amt2;   //특진합
            }
            else
            {
                if (RP[ArgCnt].GbSelf == "1")
                {
                    RPG[8].Amt2 += RP[ArgCnt].Amt1;   //비급여
                    RPG[8].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
                else if (RP[ArgCnt].GbSelf == "2")
                {
                    RPG[8].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    RPG[8].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                }
                else
                {
                    RPG[8].Amt1 += RP[ArgCnt].Amt1;   //보험합
                    RPG[8].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
            }
        }

        private void R_Amt_Add_07(long ArgB, long ArgJ, int ArgCnt, ref Slip_ReportPrint_Table[] RP, ref Report_Add_Amt_Table[] RPG)
        {
            if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" ||  RP[ArgCnt].SugbS == "3" ||  RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
            {
                RPG[7].Amt7 += (ArgB + ArgJ); //총액
                RPG[7].Amt8 += ArgJ; //조합부담
                RPG[7].Amt9 += ArgB; //조합부담
                RPG[7].Amt3 += RP[ArgCnt].Amt2;   //특진합
            }
            else
            {
                if (RP[ArgCnt].GbSelf == "1")
                {
                    RPG[7].Amt2 += RP[ArgCnt].Amt1;   //비급여
                    RPG[7].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
                else if (RP[ArgCnt].GbSelf == "2")
                {
                    RPG[7].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    RPG[7].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                }
                else
                {
                    RPG[7].Amt1 += RP[ArgCnt].Amt1;   //보험합
                    RPG[7].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
            }
        }

        private void R_Amt_Add_05(long ArgB, long ArgJ, int ArgCnt, ref Slip_ReportPrint_Table[] RP, ref Report_Add_Amt_Table[] RPG)
        {
            if (RP[ArgCnt].GbGisul == "1")
            {
                if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" ||  RP[ArgCnt].SugbS == "3" || RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
                {
                    RPG[5].Amt7 += (ArgB + ArgJ); //총액
                    RPG[5].Amt8 += ArgJ; //조합부담
                    RPG[5].Amt9 += ArgB; //조합부담
                    RPG[5].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
                else
                {
                    if (RP[ArgCnt].GbSelf == "1")
                    {
                        RPG[5].Amt2 += RP[ArgCnt].Amt1;   //비급여
                        RPG[5].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    }
                    else if (RP[ArgCnt].GbSelf == "2")
                    {
                        RPG[5].Amt3 += RP[ArgCnt].Amt2;   //특진합
                        RPG[5].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                    }
                    else
                    {
                        RPG[5].Amt1 += RP[ArgCnt].Amt1;   //보험합
                        RPG[5].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    }
                }
            }
            else
            {
                if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" ||  RP[ArgCnt].SugbS == "3" ||  RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
                {
                    RPG[6].Amt7 += (ArgB + ArgJ); //총액
                    RPG[6].Amt8 += ArgJ; //조합부담
                    RPG[6].Amt9 += ArgB; //본인부담
                    RPG[6].Amt3 += RP[ArgCnt].Amt2; // 특진합
                }
                else
                {
                    if (RP[ArgCnt].GbSelf == "1")
                    {
                        RPG[6].Amt2 += RP[ArgCnt].Amt1;   //비급여
                        RPG[6].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    }
                    else if (RP[ArgCnt].GbSelf == "2")
                    {
                        RPG[6].Amt3 += RP[ArgCnt].Amt2;   //특진합
                        RPG[6].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                    }
                    else
                    {
                        RPG[6].Amt1 += RP[ArgCnt].Amt1;   //보험합
                        RPG[6].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    }
                }
            }
        }

        private void R_Amt_Add_03(long ArgB, long ArgJ, int ArgCnt, ref Slip_ReportPrint_Table[] RP, ref Report_Add_Amt_Table[] RPG)
        {
            if (RP[ArgCnt].GbGisul == "1")
            {
                if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" ||  RP[ArgCnt].SugbS == "3" ||  RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
                {
                    RPG[3].Amt7 += (ArgB + ArgJ); //총액
                    RPG[3].Amt8 += ArgJ; //조합부담
                    RPG[3].Amt9 += ArgB; //조합부담
                    RPG[3].Amt3 += RP[ArgCnt].Amt2;   //특진합
                }
                else
                {
                    if (RP[ArgCnt].GbSelf == "1")
                    {
                        RPG[3].Amt2 += RP[ArgCnt].Amt1;   //비급여
                        RPG[3].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    }
                    else if (RP[ArgCnt].GbSelf == "2")
                    {
                        RPG[3].Amt3 += RP[ArgCnt].Amt2;   //특진합
                        RPG[3].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                    }
                    else
                    {
                        RPG[3].Amt1 += RP[ArgCnt].Amt1;   //보험합
                        RPG[3].Amt3 += RP[ArgCnt].Amt2;   //특진합

                        if (RP[ArgCnt].Bun == "11" || RP[ArgCnt].Bun == "12")
                            clsPmpaPb.GnDrugRPAmt += RP[ArgCnt].Amt1;
                    }
                }
            }
            else
            {
                if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" ||  RP[ArgCnt].SugbS == "3" ||  RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
                {
                    RPG[4].Amt7 += (ArgB + ArgJ); //총액
                    RPG[4].Amt8 += ArgJ; //조합부담
                    RPG[4].Amt9 += ArgB; //본인부담
                    RPG[4].Amt3 += RP[ArgCnt].Amt2; // 특진합
                }
                else
                {
                    if (RP[ArgCnt].GbSelf == "1")
                    {
                        RPG[4].Amt2 += RP[ArgCnt].Amt1;   //비급여
                        RPG[4].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    }
                    else if (RP[ArgCnt].GbSelf == "2")
                    {
                        RPG[4].Amt3 += RP[ArgCnt].Amt2;   //특진합
                        RPG[4].Amt4 += RP[ArgCnt].Amt1;   //본인총액
                    }
                    else
                    {
                        RPG[4].Amt1 += RP[ArgCnt].Amt1;   //보험합
                        RPG[4].Amt3 += RP[ArgCnt].Amt2;   //특진합

                        if (RP[ArgCnt].Bun == "11" || RP[ArgCnt].Bun == "12")
                            clsPmpaPb.GnDrugRPAmt += RP[ArgCnt].Amt1;
                    }
                }
            }
        }

        private void R_Amt_Add_00(long ArgB, long ArgJ, int ArgCnt, ref Slip_ReportPrint_Table[] RP, ref Report_Add_Amt_Table[] RPG)
        {
            if (string.Compare(RP[ArgCnt].Bi, "30") < 0 && (RP[ArgCnt].SugbS == "2" ||  RP[ArgCnt].SugbS == "3" ||  RP[ArgCnt].SugbS == "4" || RP[ArgCnt].SugbS == "5" || RP[ArgCnt].SugbS == "6" || RP[ArgCnt].SugbS == "7" || RP[ArgCnt].SugbS == "8" || RP[ArgCnt].SugbS == "9"))
            {
                RPG[0].Amt7 += (ArgB + ArgJ);       //총액
                RPG[0].Amt8 += ArgJ;                //조합부담
                RPG[0].Amt9 += ArgB;                //조합부담
                RPG[0].Amt3 += RP[ArgCnt].Amt2;     //특진합

            }
            else
            {
                if (RP[ArgCnt].GbSelf == "1")
                {
                    RPG[0].Amt2 += RP[ArgCnt].Amt1; //비급여
                    RPG[0].Amt3 += RP[ArgCnt].Amt2; //특진합
                }
                else if (RP[ArgCnt].GbSelf == "2")
                {
                    RPG[0].Amt3 += RP[ArgCnt].Amt2; //특진합
                    RPG[0].Amt4 += RP[ArgCnt].Amt1; //본인총액
                }
                else
                {
                    RPG[0].Amt1 += RP[ArgCnt].Amt1;   //보험합
                    RPG[0].Amt3 += RP[ArgCnt].Amt2;   //특진합

                    if ((RP[ArgCnt].SuCode == "IA213" || RP[ArgCnt].SuCode == "IA313" || RP[ArgCnt].SuCode == "IA231") && string.Compare(clsPmpaType.TOM.BDate, "2020-11-01") < 0)
                    {
                            clsPmpaPb.GnJinRP회신료 = clsPmpaPb.GnJinRP회신료 + RP[ArgCnt].Amt1;
                            RPG[0].Amt1 -= RP[ArgCnt].Amt1;   //보험합
                            RPG[0].Amt3 += RP[ArgCnt].Amt2;   //특진합
                     
                    }
                    else if ((RP[ArgCnt].SuCode == "IA313" || RP[ArgCnt].SuCode == "IA231" || RP[ArgCnt].SuCode == "IA110" || RP[ArgCnt].SuCode == "IA120") && string.Compare(clsPmpaType.TOM.BDate, "2020-11-01") >= 0)
                    {
                        clsPmpaPb.GnJinRP회신료 = clsPmpaPb.GnJinRP회신료 + RP[ArgCnt].Amt1;
                        RPG[0].Amt1 -= RP[ArgCnt].Amt1;   //보험합
                        RPG[0].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    }
                    else if ((RP[ArgCnt].SuCode == "AA176" || RP[ArgCnt].SuCode == "AA276" || RP[ArgCnt].SuCode == "AU312" || RP[ArgCnt].SuCode == "AU214" || RP[ArgCnt].SuCode == "AU413") && clsPmpaType.TOM.JinDtl =="29")
                    {
                        clsPmpaPb.GnJinRP회신료 = clsPmpaPb.GnJinRP회신료 + RP[ArgCnt].Amt1;
                        RPG[0].Amt1 -= RP[ArgCnt].Amt1;   //보험합
                        RPG[0].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    }

                    if (RP[ArgCnt].SuCode == "IA213" &&  string.Compare(clsPmpaType.TOM.BDate, "2020-11-01") >= 0 && VB.Left(clsPmpaType.TOM.Bi,1)== "1" && clsPmpaType.TOM.MCode.Trim() == "" && clsPmpaType.TOM.VCode.Trim() == "" )
                    {
                            clsPmpaPb.GnJinRP의뢰료 = clsPmpaPb.GnJinRP의뢰료 + RP[ArgCnt].Amt1;
                            RPG[0].Amt1 -= RP[ArgCnt].Amt1;   //보험합
                            RPG[0].Amt3 += RP[ArgCnt].Amt2;   //특진합
                       
                    }
                    //주석해제!
                    ////2021-09-16 결핵상담료,관리료
                    if (RP[ArgCnt].SuCode == "ID110" || RP[ArgCnt].SuCode == "ID120" || RP[ArgCnt].SuCode == "ID130")
                    {
                        clsPmpaPb.GnJinRP재택결핵 = clsPmpaPb.GnJinRP재택결핵 + RP[ArgCnt].Amt1;
                        RPG[0].Amt1 -= RP[ArgCnt].Amt1;   //보험합
                        RPG[0].Amt3 += RP[ArgCnt].Amt2;   //특진합
                    }


                }
            }
        } 
        #endregion

        private void Report_Print_Clear(ref Argument_Table RPA, ref Report_Add_Amt_Table[] RPG, ref Slip_ReportPrint_Table[] RP)
        {
            clsPmpaPb.GnDrugRPAmt = 0;
            clsPmpaPb.GnToothRpAmt = 0;
            clsPmpaPb.GnNPNnAmt = 0;
            clsPmpaPb.GnJinRP회신료 = 0;
            clsPmpaPb.GnJinRP의뢰료 = 0;
            clsPmpaPb.GnJinRP검사료 = 0;
            clsPmpaPb.GnJinRP격리료 = 0;
            clsPmpaPb.GnJinRP재택결핵 = 0;

            RPA.Ptno = "";
            RPA.DrCode = "";
            RPA.Date = "";
            RPA.Dept = "";
            RPA.Sex = "";
            RPA.GbSpc = "";
            RPA.GbGamek = "";
            RPA.GbGamekC = "";
            RPA.Retn = 0;
            RPA.Bi = 0;
            RPA.Bi1 = 0;
            RPA.Age = 0;
            RPA.AgeIlsu = 0;
            RPA.GbIlban2 = "";

            for (int i = 0; i < 999; i++)
            {
                if (i < 50)
                {
                    RPG[i].Amt1 = 0;
                    RPG[i].Amt2 = 0;
                    RPG[i].Amt3 = 0;
                    RPG[i].Amt4 = 0;
                    RPG[i].Amt5 = 0;
                    RPG[i].Amt6 = 0;
                    RPG[i].Amt7 = 0;
                    RPG[i].Amt8 = 0;
                    RPG[i].Amt9 = 0;
                }

                RP[i].SuCode = "";
                RP[i].SuNext = "";
                RP[i].OrderNo = 0;
                RP[i].GbSlip = "";
                RP[i].GbImiv = "";
                RP[i].DosCode = "";
                RP[i].GbBunup = "";
                RP[i].GbIPD = "";
                RP[i].KsJin = "";
                RP[i].GbSpc_No = "0";

                RP[i].Amt1 = 0;
                RP[i].Amt2 = 0;
                RP[i].Amt4 = 0;
                RP[i].SugbS = "";
            }
        }


        /// <summary>
        /// Description : 주소확인증 출력
        /// Author : 박병규
        /// Create Date : 2017.11.07
        /// <param name="ArgPtno">등록번호</param>
        /// <param name="ArgName">환자명</param>
        /// <param name="ArgTel">전화번호</param>
        /// <param name="ArgHphone">휴대폰</param>
        /// <param name="ArgJuso1">주소1</param>
        /// <param name="ArgJuso2">주소2</param>
        /// <param name="SPREAD"></param>
        public void Report_Print_JUSO(string ArgPtno, string ArgName, string ArgTel, string ArgHphone, string ArgJuso1, string ArgJuso2, string ArgJumin, FpSpread o)
        {
            clsPrint CP = new clsPrint();
            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strPrintName = CP.getPmpaBarCodePrinter(CARD_PRINTER_NAME);

            //접수증 프린터 설정
            if (CP.isPmpaBarCodePrinter(strPrintName) == true)
            {
                o.ActiveSheet.Cells[1, 1].Text = ArgName + "(" + ArgPtno + ")";
                o.ActiveSheet.Cells[2, 1].Text = ArgJumin+"******";
                o.ActiveSheet.Cells[3, 1].Text = ArgTel;
                o.ActiveSheet.Cells[4, 1].Text = ArgHphone;
                o.ActiveSheet.Cells[5, 1].Text = ArgJuso1 + " " + ArgJuso2;

                string strHeader = "";
                string strFooter = "";
                bool PrePrint = false;

                strHeader = "";
                strFooter = "";

                setMargin = new clsSpread.SpdPrint_Margin(2, 10, 5, 10, 5, 10);
                setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

                CS.setSpdPrint(o, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
                ComFunc.Delay(200);

            }
        }


        /// <summary>
        /// Description : 산재 물리치료 접수확인증 출력
        /// Author : 박병규
        /// Create Date : 2018.02.14
        /// <param name="ArgGb">접수 확인증/수납 확인증</param>
        /// <param name="ArgPtno">등록번호</param>
        /// <param name="ArgBi">보험종류</param>
        /// <param name="ArgSname">수진자명</param>
        /// <param name="ArgJepDate">접수일자</param>
        /// <param name="ArgActDate">회계일자</param>
        /// <param name="ArgBirth">생년월일</param>
        /// <param name="ArgMailJuso">주소</param>
        /// <param name="ArgJuso">상세주소</param>
        /// <param name="ArgAge">나이</param>
        /// <param name="ArgSex">성별</param>
        /// <param name="ArgRemark"></param>
        /// <param name="SPREAD">SPREAD</param>
        /// <seealso cref="FrmSunapPrt.frm"/>
        public void Report_Print_Sanjae_Confirm(string ArgGb, string ArgPtno, string ArgBi, string ArgSname, string ArgJepDate, string ArgActDate, string ArgBirth, string ArgMailJuso, string ArgJuso, int ArgAge, string ArgSex, FpSpread o, string ArgRemark = "")
        {
            clsPrint CP = new clsPrint();
            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strPrintName = CP.getPmpaBarCodePrinter(CARD_PRINTER_NAME);

            //프린트이름이 없을시 다른 이름으로 설정(프린트명 : 접수증, 예약증(원무팀 사무실 설정이름))
            if (strPrintName == "")
                strPrintName = CP.getPmpaBarCodePrinter(JUP_PRINTER_NAME);

            if (strPrintName == "")
                strPrintName = CP.getPmpaBarCodePrinter(RES_PRINTER_NAME);

            //접수증 프린터 설정
            if (CP.isPmpaBarCodePrinter(strPrintName) == true)
            {
                o.ActiveSheet.Cells[0, 0].Text = ArgGb;
                o.ActiveSheet.Cells[0, 1].Text = ArgPtno; //바코드
                o.ActiveSheet.Cells[1, 1].Text = ArgBi;
                o.ActiveSheet.Cells[2, 1].Text = ArgPtno + "   " + ArgSname;
                o.ActiveSheet.Cells[3, 1].Text = ArgJepDate;
                o.ActiveSheet.Cells[4, 1].Text = ArgActDate;
                o.ActiveSheet.Cells[5, 1].Text = ArgBirth;
                o.ActiveSheet.Cells[6, 1].Text = ArgMailJuso + " " + ArgJuso;
                o.ActiveSheet.Cells[7, 1].Text = ArgAge + "세 (" + ArgSex + ")";
                o.ActiveSheet.Cells[8, 0].Text = ArgRemark;

                string strHeader = "";
                string strFooter = "";
                bool PrePrint = false;

                strHeader = "";
                strFooter = "";

                setMargin = new clsSpread.SpdPrint_Margin(2, 10, 5, 10, 5, 10);
                setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

                CS.setSpdPrint(o, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
                ComFunc.Delay(200);

            }

        }

        public void Report_Print_JungganAmt(string ArgGubun, string ArgPtno, string ArgSname, string ArgDept, string ArgWard, string ArgRoom,  string ArgAmt, string ArgIPDNO, string ArgInDate,  FpSpread o, string ArgRemark = "")
        {
          //  CPP.Report_Print_JungganAmt( strPano, strSname, strDept, strWard, strAmt, nIPDNO, strInDate, ssPrint, clsPublic.GstrMsgList);

            clsPrint CP = new clsPrint();
            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strPrintName = CP.getPmpaBarCodePrinter(CARD_PRINTER_NAME);

            //프린트이름이 없을시 다른 이름으로 설정(프린트명 : 접수증, 예약증(원무팀 사무실 설정이름))
            if (strPrintName == "")
                strPrintName = CP.getPmpaBarCodePrinter(JUP_PRINTER_NAME);

            if (strPrintName == "")
                strPrintName = CP.getPmpaBarCodePrinter(RES_PRINTER_NAME);

            //접수증 프린터 설정
            if (CP.isPmpaBarCodePrinter(strPrintName) == true)
            {
                o.ActiveSheet.Cells[1, 1].Text = ArgPtno;
                o.ActiveSheet.Cells[2, 1].Text = ArgSname;
                o.ActiveSheet.Cells[3, 1].Text = ArgInDate + " " + ArgDept + " " + ArgWard + " " ;
                o.ActiveSheet.Cells[4, 1].Text = ArgAmt;   
                o.ActiveSheet.Cells[5, 1].Text = clsPublic.GstrSysDate;
                o.ActiveSheet.Cells[8, 0].Text = ArgGubun;
                
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = false;

                strHeader = "";
                strFooter = "";

                setMargin = new clsSpread.SpdPrint_Margin(2, 10, 5, 10, 5, 10);
                setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

                CS.setSpdPrint(o, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
                ComFunc.Delay(200);

            }

        }


        public void Report_Print_IPD_Confirm(string dtpInDate, string cboDept, string txtPano, string txtSname, string txtAgeSex, string cboReligion, string txtJumin1, string txtJumin2, string txtAddress, string txtZipJuso, string txtTel, string txtHPhone, string cboBi, string txtKiho, string txtDr , FpSpread o)
        {                                 //(dtpInDate.Text, cboDept.Text, txtPano.Text, txtSname.Text, VB.Pstr(txtAgeSex.Text, "/", 1), cboReligion.Text, txtJumin1.Text, txtJumin2.Text, txtAddress.Text, txtZipJuso.Text, txtTel.Text,  txtHPhone.Text, cboBi.Text, txtKiho.Text, txtDr.Text);
            clsPrint CP = new clsPrint();
            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strPrintName = CP.getPmpaBarCodePrinter(DRUG_PRINTER_NAME);  //영수증

            //프린트이름이 없을시 다른 이름으로 설정(프린트명 : 접수증, 예약증(원무팀 사무실 설정이름))
            if (strPrintName == "")
                strPrintName = CP.getPmpaBarCodePrinter(JUP_PRINTER_NAME);

          

            o.ActiveSheet.Cells[2, 1].Text = "";
            o.ActiveSheet.Cells[2, 2].Text = "";
            o.ActiveSheet.Cells[2, 3].Text = "";
            o.ActiveSheet.Cells[3, 2].Text = "";
            o.ActiveSheet.Cells[5, 1].Text = "";
            o.ActiveSheet.Cells[7, 1].Text = "";
            o.ActiveSheet.Cells[8, 1].Text = "";
            o.ActiveSheet.Cells[9, 1].Text = "";
            o.ActiveSheet.Cells[10, 1].Text = "";
            o.ActiveSheet.Cells[10, 4].Text = "";
            o.ActiveSheet.Cells[7, 6].Text = "";
            o.ActiveSheet.Cells[7, 7].Text = "";
            o.ActiveSheet.Cells[7, 9].Text = "";
            o.ActiveSheet.Cells[7, 10].Text = "";
            o.ActiveSheet.Cells[7, 11].Text = "";
            o.ActiveSheet.Cells[8, 9].Text = "";
            o.ActiveSheet.Cells[8, 10].Text = "";
            o.ActiveSheet.Cells[8, 11].Text = "";
  

                o.ActiveSheet.Cells[5, 1].Text = txtPano;
                o.ActiveSheet.Cells[7, 1].Text = txtSname;
                o.ActiveSheet.Cells[8, 1].Text = txtJumin1 + "-" + txtJumin2;
                o.ActiveSheet.Cells[9, 1].Text = txtZipJuso  + " " + txtAddress ;
                o.ActiveSheet.Cells[10, 1].Text = txtTel;
                o.ActiveSheet.Cells[10, 4].Text = txtHPhone;
   

                string strHeader = "";
                string strFooter = "";
                bool PrePrint = false;

                strHeader = "";
                strFooter = "";

                setMargin = new clsSpread.SpdPrint_Margin(2, 10, 5, 10, 5, 10);
                setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, false, false, false, false, false, false, false);

                CS.setSpdPrint(o, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
                ComFunc.Delay(200);
             
        }


        /// <summary>
        /// Description : 재원/ 퇴원 영수증 출력
        /// Author : 김민철
        /// Create Date : 2018.03.27
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgTrsNo"></param>
        /// <param name="ArgPicture_Sign"></param>
        public void IPD_Sunap_Report_A4(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, long ArgTrsNo, string strTewon, bool chkSabon)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strTimeSS = string.Empty;
            string strAMTCard = string.Empty;   //카드금액
            string strAMTTot = string.Empty;    //총금액

            int nIlsu = 0, nRow = 0;
            long nSupAmt = 0;   //납부할 금액
            long nTaxAmt = 0;   //부가세 금액

            Card CARD           = new Card();
            ComFunc CF          = new ComFunc();
            clsPrint CP         = new clsPrint();
            clsIument cIMST     = new clsIument();
            clsDrugPrint clsDP  = new clsDrugPrint();

            FpSpread ssSpread   = null;
            SheetView ssSpread_Sheet = null;

            if (ArgIpdNo == 0)
            {
                ComFunc.MsgBox("환자를 선택하여주십시오.");
                return;
            }

            clsDP.SetReciptPrint(ref ssSpread, ref ssSpread_Sheet);
            clsDP.ClearReciptPrint(ssSpread, ssSpread_Sheet);

            ComFunc.ReadSysDate(pDbCon);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                cIMST.Read_Ipd_Mst_Trans(pDbCon, ArgPano, ArgTrsNo, "");

                if (strTewon == "OK")
                {
                    #region 퇴원금입력시 영수증 발행부분 주석처리
                    //cIMST.Read_Ipd_Mst_Trans(pDbCon, ArgPano, ArgTrsNo, "");

                    //if (clsPmpaType.TIT.GbDRG == "D")
                    //{
                    //    cIMST.Ipd_Trans_PrtAmt_Read_Drg(pDbCon, ArgTrsNo);
                    //    cIMST.Ipd_Tewon_PrtAmt_Gesan_Drg(pDbCon, null, null, ArgPano, ArgIpdNo, ArgTrsNo);
                    //}
                    //else
                    //{
                    //    cIMST.Ipd_Trans_PrtAmt_Read(pDbCon, ArgTrsNo, "");
                    //    cIMST.Ipd_Tewon_PrtAmt_Gesan(pDbCon, ArgPano, ArgIpdNo, ArgTrsNo, "", "");
                    //}

                    //이미납부한 금액

                    //SQL = "";
                    //SQL = SQL + ComNum.VBLF + "SELECT SUM(SUM(CASE WHEN SUNEXT = 'Y88' THEN AMT END)) Y88 ";
                    //SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH  ";
                    //SQL = SQL + ComNum.VBLF + " WHERE TRSNO =" + ArgTrsNo + " ";
                    //SQL = SQL + ComNum.VBLF + "   AND SuNext IN ('Y88') ";
                    //SQL = SQL + ComNum.VBLF + " GROUP BY SuNext ";

                    //SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    //if (SqlErr != "")
                    //{
                    //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    //    return;
                    //}

                    //if (dt.Rows.Count > 0)
                    //{
                    //    clsPmpaType.TIT.Amt[51] = (long)VB.Val(dt.Rows[0]["Y88"].ToString().Trim());
                    //}

                    //dt.Dispose();
                    //dt = null; 
                    #endregion

                }
                
                #region 입원일수 UPDATE
                //재원일수 재 계산함.
                nIlsu = CF.DATE_ILSU(pDbCon, VB.Left(clsPmpaType.TIT.OutDate, 10), VB.Left(clsPmpaType.TIT.InDate, 10)) + 1; //일수를 계산
                clsPmpaType.TIT.Ilsu = nIlsu;

                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS         \r\n";
                SQL += "    SET ILSU = " + nIlsu + "                    \r\n";
                SQL += "  WHERE PANO = '" + clsPmpaType.TIT.Pano + "'   \r\n";
                SQL += "    AND TRSNO = " + clsPmpaType.TIT.Trsno + "       ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (clsPmpaType.TIT.OutDate == "")
                    nIlsu = CF.DATE_ILSU(pDbCon, VB.Left(clsPublic.GstrSysDate, 10), VB.Left(clsPmpaType.TIT.InDate, 10)) + 1;     //일수를 계산
                else
                    nIlsu = CF.DATE_ILSU(pDbCon, VB.Left(clsPmpaType.TIT.OutDate, 10), VB.Left(clsPmpaType.TIT.InDate, 10)) + 1;  //일수를 계산

                clsPmpaType.TIT.Ilsu = nIlsu;

                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER    \r\n";
                SQL += "    SET ILSU = " + nIlsu + "                    \r\n";
                SQL += "  WHERE PANO = '" + clsPmpaType.TIT.Pano + "'  \r\n";
                SQL += "    AND IPDNO = " + clsPmpaType.TIT.Ipdno + "      ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                #endregion

                #region Title Print
                string strPrintName = CP.getPmpaBarCodePrinter(RECEIPT_PRINTER_NAME);

                if (CP.isPmpaBarCodePrinter(strPrintName) == false) { return; }

                ssSpread_Sheet.Cells[0, 4].Text = "입 원";
                ssSpread_Sheet.Cells[0, 12].Text = clsPmpaType.TIT.Pano + " " + VB.Asc(VB.Left(clsPmpaType.TIT.DeptCode, 1)) + VB.Asc(VB.Right(clsPmpaType.TIT.DeptCode, 1)); //바코드

                //공급받는자보관용
                ssSpread_Sheet.Cells[3, 0].Text = clsPmpaType.TIT.Pano;
                ssSpread_Sheet.Cells[3, 4].Text = clsPmpaType.TIT.Sname;

                if (ArgTrsNo == 0)
                    ssSpread_Sheet.Cells[3, 8].Text = clsPmpaType.IMST.InDate + "∼" + clsPmpaType.IMST.OutDate + "(" + clsPmpaType.IMST.Ilsu + ")";
                else
                    ssSpread_Sheet.Cells[3, 8].Text = clsPmpaType.TIT.InDate + "∼" + clsPmpaType.TIT.OutDate + "(" + clsPmpaType.TIT.Ilsu + ")";

                ssSpread_Sheet.Cells[5, 0].Text = CF.READ_DEPTNAMEK(pDbCon, clsPmpaType.TIT.DeptCode);
                ssSpread_Sheet.Cells[5, 4].Text = clsPmpaType.TIT.DrgCode;
                ssSpread_Sheet.Cells[5, 8].Text = clsPmpaType.TIT.RoomCode.ToString();
                ssSpread_Sheet.Cells[5, 11].Text = CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", clsPmpaType.TIT.Bi);

                //수납자 영수증번호
                ssSpread_Sheet.Cells[5, 12].Text = clsType.User.JobName + "(" + clsType.User.IdNumber + ") ";
                #endregion

                #region //진료항목
                //진찰료
                nRow = 9;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[1].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[1].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[1].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[1].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[1].ToString("#,### ");    //비급여

                //입원
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[2].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[2].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[2].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[2].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[2].ToString("#,### ");    //비급여

                //식대
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[3].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[3].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[3].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[3].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[3].ToString("#,### ");    //비급여

                //투약 조제 행위
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[4].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[4].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[4].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[4].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[4].ToString("#,### ");    //비급여

                //투약 조제 약품
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[5].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[5].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[5].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[5].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[5].ToString("#,### ");    //비급여

                //주사 행위
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[6].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[6].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[6].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[6].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[6].ToString("#,### ");    //비급여

                //주사 약품
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[7].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[7].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[7].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[7].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[7].ToString("#,### ");    //비급여

                //마취
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[8].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[8].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[8].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[8].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[8].ToString("#,### ");    //비급여

                //처치 수술
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[9].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[9].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[9].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[9].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[9].ToString("#,### ");    //비급여

                //검사료
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[10].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[10].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[10].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[10].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[10].ToString("#,### ");    //비급여

                //영상진단
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[11].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[11].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[11].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[11].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[11].ToString("#,### ");    //비급여

                //방사선치료
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[12].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[12].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[12].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[12].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[12].ToString("#,### ");    //비급여

                //치료재료대
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[13].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[13].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[13].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[13].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[13].ToString("#,### ");    //비급여

                //물리치료
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[14].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[14].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[14].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[14].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[14].ToString("#,### ");    //비급여

                //정신요법
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[15].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[15].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[15].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[15].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[15].ToString("#,### ");    //비급여

                //전혈
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[16].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[16].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[16].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[16].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[16].ToString("#,### ");    //비급여

                //CT
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[17].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[17].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[17].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[17].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[17].ToString("#,### ");    //비급여

                //MRI
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[18].ToString("#,### ");    ///본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[18].ToString("#,### ");    ///조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[18].ToString("#,### ");    ///전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[18].ToString("#,### ");    ///선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[18].ToString("#,### ");    ///비급여

                //초음파
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[19].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[19].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[19].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[19].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[19].ToString("#,### ");    //비급여

                //보철
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[20].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[20].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[20].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[20].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[20].ToString("#,### ");    //비급여

                //증명료
                nRow += 2;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[22].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[22].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[22].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[22].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[22].ToString("#,### ");    //비급여

                //병실차액
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[21].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[21].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[21].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[21].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[21].ToString("#,### ");    //비급여

                if (clsPmpaType.TIT.GbDRG == "D")
                {
                    //DRG
                    nRow += 1;
                    ssSpread_Sheet.Cells[nRow, 0].Text = "포괄수가진료비";
                    ssSpread_Sheet.Cells[nRow, 3].Text = DRG.GnDrgBonAmt.ToString("#,### ");            //본인부담(급여)
                    ssSpread_Sheet.Cells[nRow, 5].Text = DRG.GnDrgJohapAmt.ToString("#,### ");          //조합부담(급여)
                    ssSpread_Sheet.Cells[nRow, 6].Text = "";                                            //전액본인
                    ssSpread_Sheet.Cells[nRow, 8].Text = "";                                            //선택진료
                    ssSpread_Sheet.Cells[nRow, 10].Text = "";                                           //비급여

                    long nSunAmt_Tot = DRG.GnGs50Amt_T + DRG.GnGs80Amt_T + DRG.GnGs90Amt_T;
                    long nSunAmt_Jhp = DRG.GnGs50Amt_J + DRG.GnGs80Amt_J + DRG.GnGs90Amt_J;
                    long nSunAmt_BOn = DRG.GnGs50Amt_B + DRG.GnGs80Amt_B + DRG.GnGs90Amt_B;

                    //선별급여 표시
                    nRow += 1;
                    ssSpread_Sheet.Cells[nRow, 0].Text = "선별급여";
                    ssSpread_Sheet.Cells[nRow, 3].Text = nSunAmt_BOn.ToString("#,### ");    //본인부담(급여)
                    ssSpread_Sheet.Cells[nRow, 5].Text = nSunAmt_Jhp.ToString("#,### ");    //조합부담(급여)
                    ssSpread_Sheet.Cells[nRow, 6].Text = "";    //전액본인
                    ssSpread_Sheet.Cells[nRow, 8].Text = "";    //선택진료
                    ssSpread_Sheet.Cells[nRow, 10].Text = "";    //비급여

                    long nHapTot = DRG.GnDRG_Amt2 + DRG.GnDrgFoodAmt[0] + DRG.GnDrgFoodAmt[1] + DRG.GnDrgRoomAmt[0] + DRG.GnDrgRoomAmt[1];
                    long nHapJhp = DRG.GnDrgJohapAmt + DRG.GnDrgFoodAmt[1] + DRG.GnDrgRoomAmt[1];
                    long nHapBon = DRG.GnDrgBonAmt + DRG.GnDrgFoodAmt[0] + DRG.GnDrgRoomAmt[0]; //(nHapTot - nHapJhp);

                    nHapTot += nSunAmt_Tot;
                    nHapJhp += nSunAmt_Jhp;
                    nHapBon += nSunAmt_BOn;

                    //열외군금액
                    if (DRG.GnDrg열외군금액 > 0)
                    {
                        nRow += 1;
                        ssSpread_Sheet.Cells[nRow, 0].Text = "DRG열외군차액";
                        ssSpread_Sheet.Cells[nRow, 3].Text = DRG.GnDrg열외군금액_Bon.ToString("#,### ");    //본인부담(급여)
                        ssSpread_Sheet.Cells[nRow, 5].Text = "";    //조합부담(급여)
                        ssSpread_Sheet.Cells[nRow, 6].Text = "";    //전액본인
                        ssSpread_Sheet.Cells[nRow, 8].Text = "";    //선택진료
                        ssSpread_Sheet.Cells[nRow, 10].Text = "";    //비급여
                    }

                    //합계
                    ssSpread_Sheet.Cells[35, 3].Text = nHapBon.ToString("#,### ");                  //본인부담(급여)
                    ssSpread_Sheet.Cells[35, 5].Text = nHapJhp.ToString("#,### ");                  //조합부담(급여)
                    ssSpread_Sheet.Cells[35, 6].Text = DRG.GnGs100Amt.ToString("#,### ");           //전액본인
                    ssSpread_Sheet.Cells[35, 8].Text = DRG.GnDrgSelTAmt.ToString("#,### ");         //선택진료
                    ssSpread_Sheet.Cells[35, 10].Text = DRG.GnDrgBiFAmt.ToString("#,### ");          //비급여
                }
                else
                {
                    if (clsPmpaType.RPG.Amt1[24] > 0)
                    {
                        //선별급여
                        nRow += 1;
                        ssSpread_Sheet.Cells[nRow, 0].Text = "선별급여";
                        ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[24].ToString("#,### ");    //본인부담(급여)
                        ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[24].ToString("#,### ");    //조합부담(급여)
                        ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[24].ToString("#,### ");    //전액본인
                        ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[24].ToString("#,### ");    //선택진료
                        ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[24].ToString("#,### ");    //비급여
                    }

                    //합계
                    ssSpread_Sheet.Cells[35, 3].Text = (clsPmpaType.RPG.Amt5[50] - clsPmpaType.TIT.Amt[64]).ToString("#,### ");    //본인부담(급여)
                    ssSpread_Sheet.Cells[35, 5].Text = clsPmpaType.RPG.Amt6[50].ToString("#,### ");                                //조합부담(급여)
                    ssSpread_Sheet.Cells[35, 6].Text = clsPmpaType.RPG.Amt4[50].ToString("#,### ");                                //전액본인
                    ssSpread_Sheet.Cells[35, 8].Text = clsPmpaType.RPG.Amt3[50].ToString("#,### ");                                //선택진료
                    ssSpread_Sheet.Cells[35, 10].Text = clsPmpaType.RPG.Amt2[50].ToString("#,### ");                                //비급여

                }

                if (clsPmpaType.RPG.Amt1[49] > 0)
                {
                    //기타
                    nRow += 1;
                    ssSpread_Sheet.Cells[nRow, 0].Text = "기타";
                    ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[49].ToString("#,### ");    //본인부담(급여)
                    ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[49].ToString("#,### ");    //조합부담(급여)
                    ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[49].ToString("#,### ");    //전액본인
                    ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[49].ToString("#,### ");    //선택진료
                    ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[49].ToString("#,### ");    //비급여                   
                }

                //상한액 초과금
                ssSpread_Sheet.Cells[36, 3].Text = clsPmpaType.TIT.SangAmt.ToString("#,### ");

                #endregion

                #region 금액산정부분

                #region 납부할 금액 산정
                if (clsPmpaType.TIT.OgPdBun == "H")
                {
                    nSupAmt = (clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64] - clsPmpaType.TIT.Amt[53] - clsPmpaType.TIT.Amt[62] - (clsPmpaType.TIT.Amt[51] + clsPmpaType.TIT.Amt[54] + clsPmpaType.TIT.Amt[56]));
                    nSupAmt = (long)Math.Truncate(nSupAmt / 10.0) * 10; //원단위 절사
                }
                else if (clsPmpaType.TIT.VCode == "V206" || clsPmpaType.TIT.VCode == "V231")
                {
                    nSupAmt = (clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64] - clsPmpaType.TIT.Amt[53]);
                    nSupAmt = nSupAmt - (clsPmpaType.TIT.Amt[51] + clsPmpaType.TIT.Amt[54] + clsPmpaType.TIT.Amt[56]);

                    if (clsPmpaType.TIT.Amt[52] > 0)
                    {
                        nSupAmt = nSupAmt - clsPmpaType.TIT.Amt[52];
                        //원단위절사는 최종처리
                        nSupAmt = (long)Math.Truncate(nSupAmt / 10.0) * 10;
                    }
                    else
                    {
                        nSupAmt = (long)Math.Round((nSupAmt * 0.5), 1, MidpointRounding.AwayFromZero);
                    }
                }
                else if (clsPmpaType.TIT.FCode == "F010")
                {
                    nSupAmt = clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64] - clsPmpaType.TIT.Amt[53];
                    nSupAmt = nSupAmt - (clsPmpaType.TIT.Amt[51] + clsPmpaType.TIT.Amt[54] + clsPmpaType.TIT.Amt[56]);

                    if (clsPmpaType.TIT.Amt[62] > 0)
                    {
                        nSupAmt = nSupAmt - clsPmpaType.TIT.Amt[62];
                        //원단위절사는 최종처리
                        nSupAmt = (long)Math.Truncate(nSupAmt / 10.0) * 10;
                    }
                    else
                    {
                        nSupAmt = (long)Math.Round(nSupAmt / 10.0, 1, MidpointRounding.AwayFromZero) * 10;
                    }
                }
                else
                {
                    nSupAmt = clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64] - clsPmpaType.TIT.Amt[53];
                    nSupAmt = nSupAmt - (clsPmpaType.TIT.Amt[51] + clsPmpaType.TIT.Amt[54] + clsPmpaType.TIT.Amt[56]);
                    nSupAmt = (long)Math.Truncate(nSupAmt / 10.0) * 10;
                }

                nTaxAmt = clsPmpaType.TIT.Amt[65];  //부가세 금액

                #endregion

                long nTotAmt = clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64];
                long nBonAmt = clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64] - clsPmpaType.TIT.Amt[53];

                ssSpread_Sheet.Cells[7, 13].Text = nTotAmt.ToString("#,### ");                 //총진료비
                ssSpread_Sheet.Cells[9, 13].Text = nBonAmt.ToString("#,### ");                 //본인부담금
                ssSpread_Sheet.Cells[10, 13].Text = clsPmpaType.TIT.Amt[51].ToString("#,### "); //이미납부한 금액

                if (clsPmpaType.TIT.Amt[54] > 0)
                {
                    ssSpread_Sheet.Cells[11, 11].Text = "감   액";
                    ssSpread_Sheet.Cells[11, 13].Text = clsPmpaType.TIT.Amt[54].ToString("#,### "); //감액
                }

                if (clsPmpaType.TIT.Amt[56] > 0)
                {
                    ssSpread_Sheet.Cells[11, 11].Text = "미   수";
                    ssSpread_Sheet.Cells[11, 13].Text = clsPmpaType.TIT.Amt[56].ToString("#,### "); //미수
                }

                if (clsPmpaType.TIT.OgPdBun == "H")
                {
                    ssSpread_Sheet.Cells[12, 11].Text = "지 원 금"; //지원금
                    ssSpread_Sheet.Cells[12, 13].Text = clsPmpaType.TIT.Amt[62].ToString("#,### "); //지원금
                }
                else if (clsPmpaType.TIT.VCode == "V206" || clsPmpaType.TIT.VCode == "V231")
                {
                    if (clsPmpaType.TIT.Amt[62] != 0)
                    {
                        if (clsPmpaType.TIT.FCode == "F008")
                        {
                            ssSpread_Sheet.Cells[12, 13].Text = (Math.Truncate(clsPmpaType.TIT.Amt[62] / 10.0) * 10).ToString("#,### ");
                        }
                        else
                        {
                            ssSpread_Sheet.Cells[12, 13].Text = (Math.Round((clsPmpaType.TIT.Amt[62] * 0.5), 1, MidpointRounding.AwayFromZero)).ToString("#,### ");
                        }
                    }
                }
                else if (clsPmpaType.TIT.FCode == "F010")
                {
                    if (clsPmpaType.TIT.Amt[62] != 0)
                    {
                        ssSpread_Sheet.Cells[12, 13].Text = (Math.Truncate(clsPmpaType.TIT.Amt[62] / 10.0) * 10).ToString("#,### ");
                    }
                }
                
                nSupAmt = nSupAmt + nTaxAmt;

                ssSpread_Sheet.Cells[13, 13].Text = nSupAmt.ToString("#,### ");     //납부할 금액
                ssSpread_Sheet.Cells[18, 13].Text = nSupAmt.ToString("#,### ");     //수납금액 합계

                //급여 22종 이거나 건보자격중 OGPDBUN "F"인 경우의 장애인(BOHUN == "3")만 기금 지원
                //if (clsPmpaType.TIT.OgPdBun == "F")
                //if (clsPmpaType.TIT.Bohun == "3" && ((clsPmpaType.TIT.OgPdBun == "F" && VB.Left(clsPmpaType.TIT.Bi, 1) == "1") || clsPmpaType.TIT.Bi == "22"))
                //{
                //    ssSpread_Sheet.Cells[12, 11].Text = "장애인기금";
                //    ssSpread_Sheet.Cells[12, 13].Text = (clsPmpaType.TIT.Amt[55] - nSupAmt).ToString("#,### "); //지원금
                //}

                if (chkSabon == true)
                {
                    //ssSpread_Sheet.Cells[12, 11].Font.Size = 18;
                    ssSpread_Sheet.Cells[12, 13].Text += ComNum.VBLF + "사    본";
                }

                if (clsPmpaType.TIT.Amt[54] > 0)
                {
                    ssSpread_Sheet.Cells[22, 12].Text = "감액관계";
                    ssSpread_Sheet.Cells[22, 12].Text += ":" + CF.Read_Bcode_Name(pDbCon, "BAS_감액코드명", clsPmpaType.TIT.GbGameK) + "";
                }

                if (clsPmpaType.RSD.Gubun == "1")   //신용카드
                {
                    if (clsPmpaType.RD.OrderGb == "2")
                    {   //취소
                        ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);                //카드
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (nSupAmt - (clsPmpaType.RSD.TotAmt * -1)));  //현금
                    }
                    else
                    {   //승인
                        ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);                     //카드
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", nSupAmt - clsPmpaType.RSD.TotAmt);           //현금
                    }
                }
                else if (clsPmpaType.RSD.Gubun == "2")  //현금영수
                {
                    if (clsPmpaType.RD.OrderGb == "2")
                    {
                        ssSpread_Sheet.Cells[16, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);                    //현금영수증
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (nSupAmt - (clsPmpaType.RSD.TotAmt * -1)));      //현금

                        if (clsPmpaType.RSD.CardSeqNo != 0)
                        {
                            ssSpread_Sheet.Cells[28, 12].Text = "현금승인취소";

                            ssSpread_Sheet.Cells[29, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";  //신용카드매출전표
                            ssSpread_Sheet.Cells[32, 12].Text = clsPmpaType.RD.ApprovalNo;                                              //승인번호

                            ssSpread_Sheet.Cells[29, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                            ssSpread_Sheet.Cells[32, 12].Text = clsPmpaType.RD.ApprovalNo;  //승인번호

                            ssSpread_Sheet.Cells[29, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                            ssSpread_Sheet.Cells[32, 12].Text = clsPmpaType.RD.ApprovalNo;  //승인번호

                        }
                    }
                    else
                    {
                        ssSpread_Sheet.Cells[16, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);                         //현금영수증
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", nSupAmt - clsPmpaType.RSD.TotAmt);               //현금

                        if (clsPmpaType.RSD.CardSeqNo != 0)
                        {
                            ssSpread_Sheet.Cells[28, 12].Text = "현금승인";

                            ssSpread_Sheet.Cells[29, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";  //신용카드매출전표
                            ssSpread_Sheet.Cells[32, 12].Text = clsPmpaType.RD.ApprovalNo;                                              //승인번호

                            ssSpread_Sheet.Cells[29, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                            ssSpread_Sheet.Cells[32, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호

                            ssSpread_Sheet.Cells[29, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                            ssSpread_Sheet.Cells[32, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호

                        }
                    }
                }
                else
                {
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", nSupAmt);                                            //현금
                }

                ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", nSupAmt);                                                //합계

                if (strTimeSS != "")
                    ssSpread_Sheet.Cells[40, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime + ":" + strTimeSS;
                else
                    ssSpread_Sheet.Cells[40, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime;

                //카드영수증 통합
                if (clsPmpaType.RSD.CardSeqNo > 0)
                {
                    Card.GstrCardApprov_Info = CARD.Card_Sign_Info_Set(pDbCon, clsPmpaType.RSD.CardSeqNo, ArgPano);

                    if (Card.GstrCardApprov_Info != "")
                    {
                        ssSpread_Sheet.Cells[29, 13].ColumnSpan = 3;
                        ssSpread_Sheet.Cells[29, 13].HorizontalAlignment = CellHorizontalAlignment.Left;
                        ssSpread_Sheet.Cells[29, 13].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 24); //거래구분

                        ssSpread_Sheet.Cells[30, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 4) + " " + VB.Pstr(Card.GstrCardApprov_Info, "{}", 2); //카드종류/카드번호
                        ssSpread_Sheet.Cells[31, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 20); //승인금액
                        ssSpread_Sheet.Cells[32, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 12); //승인번호
                        ssSpread_Sheet.Cells[33, 12].Text = "72117503"; //가맹점번호
                        ssSpread_Sheet.Cells[34, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 14); //거래일시
                        ssSpread_Sheet.Cells[34, 15].Text = VB.Pstr(VB.Pstr(Card.GstrCardApprov_Info, "{}", 8), "개월", 1);
                    }

                    Card.GstrCardApprov_Info = "";
                }
                #endregion

                #region 영수증 출력일시
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS') Sdate FROM DUAL ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("영수증 출력일시 조회중 문제가 발생했습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (dt.Rows.Count > 0)
                    strTimeSS = VB.Right(dt.Rows[0]["Sdate"].ToString(), 2);

                if (strTimeSS != "")
                    ssSpread_Sheet.Cells[40, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime + ":" + strTimeSS;
                else
                    ssSpread_Sheet.Cells[40, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime;
                #endregion

                clsDP.PrintReciptPrint(ssSpread, ssSpread_Sheet, strPrintName);

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.ToString());
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

        }
        //2018-08-22 병실료 분류에따른 변경
        public void IPD_Sunap_Report_A4_New(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, long ArgTrsNo, string strTewon, bool chkSabon)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strTimeSS = string.Empty;
            string strAMTCard = string.Empty;   //카드금액
            string strAMTTot = string.Empty;    //총금액

            int nIlsu = 0, nRow = 0;
            long nSupAmt = 0;   //납부할 금액
            long nTaxAmt = 0;   //부가세 금액

            Card CARD = new Card();
            ComFunc CF = new ComFunc();
            clsPrint CP = new clsPrint();
            clsIument cIMST = new clsIument();
            clsDrugPrint clsDP = new clsDrugPrint();

            FpSpread ssSpread = null;
            SheetView ssSpread_Sheet = null;

            if (ArgIpdNo == 0)
            {
                ComFunc.MsgBox("환자를 선택하여주십시오.");
                return;
            }
            //Dim nHRoomAmt       As Long    '2인실 2018-07-01
            //Dim nHRoomBonin     As Long    '2인실 2018-07-01


            clsDP.SetReciptPrint_New(ref ssSpread, ref ssSpread_Sheet);   //변경
            clsDP.ClearReciptPrint_New(ssSpread, ssSpread_Sheet);         //변경   

            ComFunc.ReadSysDate(pDbCon);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                cIMST.Read_Ipd_Mst_Trans(pDbCon, ArgPano, ArgTrsNo, "");

                if (strTewon == "OK")
                {
                    #region 퇴원금입력시 영수증 발행부분 주석처리
                    //cIMST.Read_Ipd_Mst_Trans(pDbCon, ArgPano, ArgTrsNo, "");

                    //if (clsPmpaType.TIT.GbDRG == "D")
                    //{
                    //    cIMST.Ipd_Trans_PrtAmt_Read_Drg(pDbCon, ArgTrsNo);
                    //    cIMST.Ipd_Tewon_PrtAmt_Gesan_Drg(pDbCon, null, null, ArgPano, ArgIpdNo, ArgTrsNo);
                    //}
                    //else
                    //{
                    //    cIMST.Ipd_Trans_PrtAmt_Read(pDbCon, ArgTrsNo, "");
                    //    cIMST.Ipd_Tewon_PrtAmt_Gesan(pDbCon, ArgPano, ArgIpdNo, ArgTrsNo, "", "");
                    //}

                    //이미납부한 금액

                    //SQL = "";
                    //SQL = SQL + ComNum.VBLF + "SELECT SUM(SUM(CASE WHEN SUNEXT = 'Y88' THEN AMT END)) Y88 ";
                    //SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH  ";
                    //SQL = SQL + ComNum.VBLF + " WHERE TRSNO =" + ArgTrsNo + " ";
                    //SQL = SQL + ComNum.VBLF + "   AND SuNext IN ('Y88') ";
                    //SQL = SQL + ComNum.VBLF + " GROUP BY SuNext ";

                    //SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    //if (SqlErr != "")
                    //{
                    //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    //    return;
                    //}

                    //if (dt.Rows.Count > 0)
                    //{
                    //    clsPmpaType.TIT.Amt[51] = (long)VB.Val(dt.Rows[0]["Y88"].ToString().Trim());
                    //}

                    //dt.Dispose();
                    //dt = null; 
                    #endregion

                }

                #region 입원일수 UPDATE
                //재원일수 재 계산함.
                if (clsPmpaType.TIT.OutDate == "")
                    nIlsu = CF.DATE_ILSU(pDbCon, VB.Left(clsPublic.GstrSysDate, 10), VB.Left(clsPmpaType.TIT.InDate, 10)) + 1;     //일수를 계산
                else
                    nIlsu = CF.DATE_ILSU(pDbCon, VB.Left(clsPmpaType.TIT.OutDate, 10), VB.Left(clsPmpaType.TIT.InDate, 10)) + 1;  //일수를 계산

                //nIlsu = CF.DATE_ILSU(pDbCon, VB.Left(clsPmpaType.TIT.OutDate, 10), VB.Left(clsPmpaType.TIT.InDate, 10)) + 1; //일수를 계산
                clsPmpaType.TIT.Ilsu = nIlsu;

                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS         \r\n";
                SQL += "    SET ILSU = " + nIlsu + "                    \r\n";
                SQL += "  WHERE PANO = '" + clsPmpaType.TIT.Pano + "'   \r\n";
                SQL += "    AND TRSNO = " + clsPmpaType.TIT.Trsno + "       ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (clsPmpaType.TIT.M_OutDate == "")
                    nIlsu = CF.DATE_ILSU(pDbCon, VB.Left(clsPublic.GstrSysDate, 10), VB.Left(clsPmpaType.TIT.M_InDate, 10)) + 1;     //일수를 계산
                else
                    nIlsu = CF.DATE_ILSU(pDbCon, VB.Left(clsPmpaType.TIT.M_OutDate, 10), VB.Left(clsPmpaType.TIT.M_InDate, 10)) + 1;  //일수를 계산

                clsPmpaType.TIT.Ilsu = nIlsu;

                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER    \r\n";
                SQL += "    SET ILSU = " + nIlsu + "                    \r\n";
                SQL += "  WHERE PANO = '" + clsPmpaType.TIT.Pano + "'  \r\n";
                SQL += "    AND IPDNO = " + clsPmpaType.TIT.Ipdno + "      ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                #endregion

                #region Title Print
                string strPrintName = CP.getPmpaBarCodePrinter(RECEIPT_PRINTER_NAME);

                if (CP.isPmpaBarCodePrinter(strPrintName) == false) { return; }

                ssSpread_Sheet.Cells[0, 4].Text = "입 원";
                ssSpread_Sheet.Cells[0, 12].Text = clsPmpaType.TIT.Pano + " " + VB.Asc(VB.Left(clsPmpaType.TIT.DeptCode, 1)) + VB.Asc(VB.Right(clsPmpaType.TIT.DeptCode, 1)); //바코드

                //공급받는자보관용
                ssSpread_Sheet.Cells[3, 0].Text = clsPmpaType.TIT.Pano;
                ssSpread_Sheet.Cells[3, 4].Text = clsPmpaType.TIT.Sname;

                if (ArgTrsNo == 0)
                    ssSpread_Sheet.Cells[3, 8].Text = clsPmpaType.IMST.InDate + "∼" + clsPmpaType.IMST.OutDate + "(" + clsPmpaType.IMST.Ilsu + ")";
                else
                    ssSpread_Sheet.Cells[3, 8].Text = clsPmpaType.TIT.InDate + "∼" + clsPmpaType.TIT.OutDate + "(" + clsPmpaType.TIT.Ilsu + ")";

                ssSpread_Sheet.Cells[5, 0].Text = CF.READ_DEPTNAMEK(pDbCon, clsPmpaType.TIT.DeptCode);
                ssSpread_Sheet.Cells[5, 4].Text = clsPmpaType.TIT.DrgCode;
                ssSpread_Sheet.Cells[5, 8].Text = clsPmpaType.TIT.RoomCode.ToString();
                ssSpread_Sheet.Cells[5, 11].Text = CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", clsPmpaType.TIT.Bi);

                //수납자 영수증번호
                ssSpread_Sheet.Cells[5, 12].Text = clsType.User.JobName + "(" + clsType.User.IdNumber + ") ";
                #endregion

                //clsPmpaPb.GnHRoomAmt = nHRoomAmt;
                //clsPmpaPb.GnHRoomBonin = nHRoomBonin;
                #region //진료항목
                //진찰료
                nRow = 9;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[1].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[1].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[1].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[1].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[1].ToString("#,### ");    //비급여

                //입원 2,3,4 인실
                nRow += 1;
               // ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaPb.GnHRoomBonin.ToString("#,### ");    //본인부담(급여)
               // ssSpread_Sheet.Cells[nRow, 5].Text = (clsPmpaPb.GnHRoomAmt- clsPmpaPb.GnHRoomBonin).ToString("#,### ");    //조합부담(급여)
               // ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[2].ToString("#,### ");    //전액본인
               // ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[2].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaPb.GnH1RoomAmt.ToString("#,### ");    //비급여
                                                                                                     //입원
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaPb.GnHRoomBonin.ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = (clsPmpaPb.GnHRoomAmt- clsPmpaPb.GnHRoomBonin).ToString("#,### ");    //조합부담(급여)
                //ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[2].ToString("#,### ");    //전액본인
                //ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[2].ToString("#,### ");    //선택진료
                //ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[2].ToString("#,### ");    //비급여
                //입원
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = (clsPmpaType.RPG.Amt5[2]- clsPmpaPb.GnHRoomBonin - clsPmpaPb.GnHUSetBonin).ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = (clsPmpaType.RPG.Amt6[2]- (clsPmpaPb.GnHRoomAmt - clsPmpaPb.GnHRoomBonin) - (clsPmpaPb.GnHUSetAmt - clsPmpaPb.GnHUSetBonin) ).ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[2].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[2].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = (clsPmpaType.RPG.Amt2[2]- clsPmpaPb.GnH1RoomAmt).ToString("#,### ");    //비급여

                //식대
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[3].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[3].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[3].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[3].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[3].ToString("#,### ");    //비급여

                //투약 조제 행위
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[4].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[4].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[4].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[4].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[4].ToString("#,### ");    //비급여

                //투약 조제 약품
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[5].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[5].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[5].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[5].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[5].ToString("#,### ");    //비급여

                //주사 행위
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[6].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[6].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[6].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[6].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[6].ToString("#,### ");    //비급여

                //주사 약품
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[7].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[7].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[7].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[7].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[7].ToString("#,### ");    //비급여

                //마취
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[8].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[8].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[8].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[8].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[8].ToString("#,### ");    //비급여

                //처치 수술
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[9].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[9].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[9].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[9].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[9].ToString("#,### ");    //비급여

                //검사료
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[10].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[10].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[10].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[10].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[10].ToString("#,### ");    //비급여

                //영상진단
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[11].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[11].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[11].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[11].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[11].ToString("#,### ");    //비급여

                //방사선치료
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[12].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[12].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[12].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[12].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[12].ToString("#,### ");    //비급여

                //치료재료대
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[13].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[13].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[13].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[13].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[13].ToString("#,### ");    //비급여

                //물리치료
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[14].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[14].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[14].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[14].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[14].ToString("#,### ");    //비급여

                //정신요법
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[15].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[15].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[15].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[15].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[15].ToString("#,### ");    //비급여

                //전혈
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[16].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[16].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[16].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[16].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[16].ToString("#,### ");    //비급여

                //CT
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[17].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[17].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[17].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[17].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[17].ToString("#,### ");    //비급여

                //MRI
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[18].ToString("#,### ");    ///본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[18].ToString("#,### ");    ///조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[18].ToString("#,### ");    ///전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[18].ToString("#,### ");    ///선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[18].ToString("#,### ");    ///비급여

                //초음파
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[19].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[19].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[19].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[19].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[19].ToString("#,### ");    //비급여

                //보철
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[20].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[20].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[20].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[20].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[20].ToString("#,### ");    //비급여

                //증명료
                nRow += 2;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[22].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[22].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[22].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[22].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[22].ToString("#,### ");    //비급여

                //병실차액
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[21].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[21].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[21].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[21].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[21].ToString("#,### ");    //비급여
                if (clsPmpaType.TIT.GBHU == "Y")
                {
                    //호스피스 병실료
                    nRow += 1;
                    ssSpread_Sheet.Cells[nRow, 0].Text = "호스피스병실료(정액)";
                    ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaPb.GnHUSetBonin.ToString("#,### ");            //본인부담(급여)
                    ssSpread_Sheet.Cells[nRow, 5].Text = (clsPmpaPb.GnHUSetAmt - clsPmpaPb.GnHUSetBonin).ToString("#,### ");          //조합부담(급여)
                    ssSpread_Sheet.Cells[nRow, 6].Text = "";                                            //전액본인
                    ssSpread_Sheet.Cells[nRow, 8].Text = "";                                            //선택진료
                    ssSpread_Sheet.Cells[nRow, 10].Text = "";                                           //비급여
                }

                    if (clsPmpaType.TIT.GbDRG == "D")
                {
                    //DRG
                    nRow += 1;
                    ssSpread_Sheet.Cells[nRow, 0].Text = "포괄수가진료비";
                    ssSpread_Sheet.Cells[nRow, 3].Text = DRG.GnDrgBonAmt.ToString("#,### ");            //본인부담(급여)
                    ssSpread_Sheet.Cells[nRow, 5].Text = DRG.GnDrgJohapAmt.ToString("#,### ");          //조합부담(급여)
                    ssSpread_Sheet.Cells[nRow, 6].Text = "";                                            //전액본인
                    ssSpread_Sheet.Cells[nRow, 8].Text = "";                                            //선택진료
                    ssSpread_Sheet.Cells[nRow, 10].Text = "";                                           //비급여

                    long nSunAmt_Tot = DRG.GnGs50Amt_T + DRG.GnGs80Amt_T + DRG.GnGs90Amt_T;
                    long nSunAmt_Jhp = DRG.GnGs50Amt_J + DRG.GnGs80Amt_J + DRG.GnGs90Amt_J;
                    long nSunAmt_BOn = DRG.GnGs50Amt_B + DRG.GnGs80Amt_B + DRG.GnGs90Amt_B;

                    //선별급여 표시
                    nRow += 1;
                    ssSpread_Sheet.Cells[nRow, 0].Text = "선별급여";
                    ssSpread_Sheet.Cells[nRow, 3].Text = nSunAmt_BOn.ToString("#,### ");    //본인부담(급여)
                    ssSpread_Sheet.Cells[nRow, 5].Text = nSunAmt_Jhp.ToString("#,### ");    //조합부담(급여)
                    ssSpread_Sheet.Cells[nRow, 6].Text = "";    //전액본인
                    ssSpread_Sheet.Cells[nRow, 8].Text = "";    //선택진료
                    ssSpread_Sheet.Cells[nRow, 10].Text = "";    //비급여

                    long nHapTot = DRG.GnDRG_Amt2 + DRG.GnDrgFoodAmt[0] + DRG.GnDrgFoodAmt[1] + DRG.GnDrgRoomAmt[0] + DRG.GnDrgRoomAmt[1];
                    long nHapJhp = DRG.GnDrgJohapAmt + DRG.GnDrgFoodAmt[1] + DRG.GnDrgRoomAmt[1];
                    long nHapBon = DRG.GnDrgBonAmt + DRG.GnDrgFoodAmt[0] + DRG.GnDrgRoomAmt[0] + DRG.GnDrg열외군금액_Bon ; //(nHapTot - nHapJhp);

                    nHapTot += nSunAmt_Tot;
                    nHapJhp += nSunAmt_Jhp;
                    nHapBon += nSunAmt_BOn;

                    //열외군금액
                    if (DRG.GnDrg열외군금액 > 0)
                    {
                        nRow += 1;
                        ssSpread_Sheet.Cells[nRow, 0].Text = "DRG열외군차액";
                        ssSpread_Sheet.Cells[nRow, 3].Text = DRG.GnDrg열외군금액_Bon.ToString("#,### ");    //본인부담(급여)
                        ssSpread_Sheet.Cells[nRow, 5].Text = "";    //조합부담(급여)
                        ssSpread_Sheet.Cells[nRow, 6].Text = "";    //전액본인
                        ssSpread_Sheet.Cells[nRow, 8].Text = "";    //선택진료
                        ssSpread_Sheet.Cells[nRow, 10].Text = "";    //비급여
                    }

                    //합계
                    ssSpread_Sheet.Cells[37, 3].Text = nHapBon.ToString("#,### ");                  //본인부담(급여)
                    ssSpread_Sheet.Cells[37, 5].Text = nHapJhp.ToString("#,### ");                  //조합부담(급여)
                    ssSpread_Sheet.Cells[37, 6].Text = DRG.GnGs100Amt.ToString("#,### ");           //전액본인
                    ssSpread_Sheet.Cells[37, 8].Text = DRG.GnDrgSelTAmt.ToString("#,### ");         //선택진료
                    ssSpread_Sheet.Cells[37, 10].Text = DRG.GnDrgBiFAmt.ToString("#,### ");          //비급여
                }
                else
                {
                    if (clsPmpaType.RPG.Amt1[24] > 0)
                    {
                        //선별급여
                        nRow += 1;
                        ssSpread_Sheet.Cells[nRow, 0].Text = "선별급여";
                        ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[24].ToString("#,### ");    //본인부담(급여)
                        ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[24].ToString("#,### ");    //조합부담(급여)
                        ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[24].ToString("#,### ");    //전액본인
                        ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[24].ToString("#,### ");    //선택진료
                        ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[24].ToString("#,### ");    //비급여
                    }

                    //합계
                    ssSpread_Sheet.Cells[37, 3].Text = (clsPmpaType.RPG.Amt5[50] - clsPmpaType.TIT.Amt[64]).ToString("#,### ");    //본인부담(급여)
                    ssSpread_Sheet.Cells[37, 5].Text = clsPmpaType.RPG.Amt6[50].ToString("#,### ");                                //조합부담(급여)
                    ssSpread_Sheet.Cells[37, 6].Text = clsPmpaType.RPG.Amt4[50].ToString("#,### ");                                //전액본인
                    ssSpread_Sheet.Cells[37, 8].Text = clsPmpaType.RPG.Amt3[50].ToString("#,### ");                                //선택진료
                    ssSpread_Sheet.Cells[37, 10].Text = clsPmpaType.RPG.Amt2[50].ToString("#,### ");                                //비급여

                }

                if (clsPmpaType.RPG.Amt2[49] > 0 || clsPmpaType.RPG.Amt3[49] > 0|| clsPmpaType.RPG.Amt4[49] > 0|| clsPmpaType.RPG.Amt5[49] > 0|| clsPmpaType.RPG.Amt6[49] > 0)
                {
                    //기타
                    nRow += 1;
                    ssSpread_Sheet.Cells[nRow, 0].Text = "기타";
                    ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[49].ToString("#,### ");    //본인부담(급여)
                    ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[49].ToString("#,### ");    //조합부담(급여)
                    ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[49].ToString("#,### ");    //전액본인
                    ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[49].ToString("#,### ");    //선택진료
                    ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[49].ToString("#,### ");    //비급여                   
                }

                //상한액 초과금
                ssSpread_Sheet.Cells[38, 3].Text = clsPmpaType.TIT.SangAmt.ToString("#,### ");

                #endregion

                #region 금액산정부분

                #region 납부할 금액 산정
                if (clsPmpaType.TIT.OgPdBun == "H")
                {
                    nSupAmt = (clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64] - clsPmpaType.TIT.Amt[53] - clsPmpaType.TIT.Amt[62] - (clsPmpaType.TIT.Amt[51] + clsPmpaType.TIT.Amt[54] + clsPmpaType.TIT.Amt[56]));
                    nSupAmt = (long)Math.Truncate(nSupAmt / 10.0) * 10; //원단위 절사
                }
                else if (clsPmpaType.TIT.VCode == "V206" || clsPmpaType.TIT.VCode == "V231")
                {
                    nSupAmt = (clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64] - clsPmpaType.TIT.Amt[53]);
                    nSupAmt = nSupAmt - (clsPmpaType.TIT.Amt[51] + clsPmpaType.TIT.Amt[54] + clsPmpaType.TIT.Amt[56]);

                    if (clsPmpaType.TIT.Amt[52] > 0)
                    {
                        nSupAmt = nSupAmt - clsPmpaType.TIT.Amt[52];
                        //원단위절사는 최종처리
                        nSupAmt = (long)Math.Truncate(nSupAmt / 10.0) * 10;
                    }
                    else
                    {
                        nSupAmt = (long)Math.Round((nSupAmt * 0.5), 1, MidpointRounding.AwayFromZero);
                    }
                }
                else if (clsPmpaType.TIT.FCode == "F010")
                {
                    nSupAmt = clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64] - clsPmpaType.TIT.Amt[53];
                    nSupAmt = nSupAmt - (clsPmpaType.TIT.Amt[51] + clsPmpaType.TIT.Amt[54] + clsPmpaType.TIT.Amt[56]);

                    if (clsPmpaType.TIT.Amt[62] > 0)
                    {
                        nSupAmt = nSupAmt - clsPmpaType.TIT.Amt[62];
                        //원단위절사는 최종처리
                        nSupAmt = (long)Math.Truncate(nSupAmt / 10.0) * 10;
                    }
                    else
                    {
                        nSupAmt = (long)Math.Round(nSupAmt / 10.0, 1, MidpointRounding.AwayFromZero) * 10;
                    }
                }
                else
                {
                    nSupAmt = clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64] - clsPmpaType.TIT.Amt[53];
                    nSupAmt = nSupAmt - (clsPmpaType.TIT.Amt[51] + clsPmpaType.TIT.Amt[54] + clsPmpaType.TIT.Amt[56]);
                    nSupAmt = (long)Math.Truncate(nSupAmt / 10.0) * 10;
                }

                nTaxAmt = clsPmpaType.TIT.Amt[67];  //부가세 금액

                #endregion

                long nTotAmt = clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64];  
                long nBonAmt = clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64] - clsPmpaType.TIT.Amt[53];

                ssSpread_Sheet.Cells[7, 13].Text = nTotAmt.ToString("#,### ");                 //총진료비
                ssSpread_Sheet.Cells[9, 13].Text = nBonAmt.ToString("#,### ");                 //본인부담금
                ssSpread_Sheet.Cells[12, 13].Text = clsPmpaType.TIT.Amt[51].ToString("#,### "); //이미납부한 금액

                if (clsPmpaType.TIT.Amt[54] > 0 && clsPmpaType.TIT.Amt[56] > 0)
                {
                    ssSpread_Sheet.Cells[13, 11].Text = "감액 ";
                    ssSpread_Sheet.Cells[13, 11].Text += clsPmpaType.TIT.Amt[54].ToString("#,### "); //감액
                    ssSpread_Sheet.Cells[13, 13].Text = "미수 ";
                    ssSpread_Sheet.Cells[13, 13].Text += clsPmpaType.TIT.Amt[56].ToString("#,### "); //미수
                }
                else if (clsPmpaType.TIT.Amt[54] > 0)
                {
                    ssSpread_Sheet.Cells[13, 11].Text = "감   액";
                    ssSpread_Sheet.Cells[13, 13].Text = clsPmpaType.TIT.Amt[54].ToString("#,### "); //감액
                }
                else if (clsPmpaType.TIT.Amt[56] > 0)
                {
                    ssSpread_Sheet.Cells[13, 11].Text = "미   수";
                    ssSpread_Sheet.Cells[13, 13].Text = clsPmpaType.TIT.Amt[56].ToString("#,### "); //미수
                }
                

                if (clsPmpaType.TIT.OgPdBun == "H")
                {
                    ssSpread_Sheet.Cells[14, 11].Text = "지 원 금"; //지원금
                    ssSpread_Sheet.Cells[14, 13].Text = clsPmpaType.TIT.Amt[62].ToString("#,### "); //지원금
                }
                else if (clsPmpaType.TIT.VCode == "V206" || clsPmpaType.TIT.VCode == "V231")
                {
                    if (clsPmpaType.TIT.Amt[62] != 0)
                    {
                        ssSpread_Sheet.Cells[14, 11].Text = "지 원 금"; //지원금

                        if (clsPmpaType.TIT.FCode == "F008")
                        {
                            ssSpread_Sheet.Cells[14, 13].Text = (Math.Truncate(clsPmpaType.TIT.Amt[62] / 10.0) * 10).ToString("#,### ");
                        }
                        else
                        {
                            ssSpread_Sheet.Cells[14, 13].Text = (Math.Round((clsPmpaType.TIT.Amt[62] * 0.5), 1, MidpointRounding.AwayFromZero)).ToString("#,### ");
                        }
                    }
                }
                else if (clsPmpaType.TIT.FCode == "F010")
                {
                    if (clsPmpaType.TIT.Amt[62] != 0)
                    {
                        ssSpread_Sheet.Cells[14, 11].Text = "지 원 금"; //지원금
                        ssSpread_Sheet.Cells[14, 13].Text = (Math.Truncate(clsPmpaType.TIT.Amt[62] / 10.0) * 10).ToString("#,### ");
                    }
                }

                nSupAmt = nSupAmt + nTaxAmt;

                ssSpread_Sheet.Cells[15, 13].Text = nSupAmt.ToString("#,### ");     //납부할 금액
                ssSpread_Sheet.Cells[20, 13].Text = nSupAmt.ToString("#,### ");     //수납금액 합계

                //급여 22종 이거나 건보자격중 OGPDBUN "F"인 경우의 장애인(BOHUN == "3")만 기금 지원
                //if (clsPmpaType.TIT.OgPdBun == "F")
                //if (clsPmpaType.TIT.Bohun == "3" && ((clsPmpaType.TIT.OgPdBun == "F" && VB.Left(clsPmpaType.TIT.Bi, 1) == "1") || clsPmpaType.TIT.Bi == "22"))
                //{
                //    ssSpread_Sheet.Cells[12, 11].Text = "장애인기금";
                //    ssSpread_Sheet.Cells[12, 13].Text = (clsPmpaType.TIT.Amt[55] - nSupAmt).ToString("#,### "); //지원금
                //}

                if (chkSabon == true)
                {
                    //ssSpread_Sheet.Cells[12, 11].Font.Size = 18;
                    ssSpread_Sheet.Cells[14, 13].Text += ComNum.VBLF + "사    본";
                }

                if (clsPmpaType.TIT.Amt[54] > 0)
                {
                    ssSpread_Sheet.Cells[24, 12].Text = "감액관계";
                    ssSpread_Sheet.Cells[24, 12].Text += ":" + CF.Read_Bcode_Name(pDbCon, "BAS_감액코드명", clsPmpaType.TIT.GbGameK) + "";
                }

                //납부할 금액
                if (chkSabon == true)
                {
                    //사본출력의 경우 신용카드,현금영수증,현금 구분 없이 총액만 표시되도록 함.
                    //입원진료비 조에서 영수증 사본 표시 체크했을 경우에만 적용이 됩니다.
                    //2021-08-03 김윤희 선생님과 협의함.
                }
                else
                {

                    if (clsPmpaType.RSD.Gubun == "1")   //신용카드
                    {
                        if (clsPmpaType.RD.OrderGb == "2")
                        {   //취소
                            ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);                //카드
                            ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (nSupAmt - (clsPmpaType.RSD.TotAmt * -1)));  //현금
                        }
                        else
                        {   //승인
                            ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);                     //카드
                            ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", nSupAmt - clsPmpaType.RSD.TotAmt);           //현금
                        }
                    }
                    else if (clsPmpaType.RSD.Gubun == "2")  //현금영수
                    {
                        if (clsPmpaType.RD.OrderGb == "2")
                        {
                            ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);                    //현금영수증
                            ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (nSupAmt - (clsPmpaType.RSD.TotAmt * -1)));      //현금

                            if (clsPmpaType.RSD.CardSeqNo != 0)
                            {
                                ssSpread_Sheet.Cells[30, 12].Text = "현금승인취소";

                                ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";  //신용카드매출전표
                                ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;                                              //승인번호

                                ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                                ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;  //승인번호

                                ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                                ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;  //승인번호

                            }
                        }
                        else
                        {
                            ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);                         //현금영수증
                            ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", nSupAmt - clsPmpaType.RSD.TotAmt);               //현금

                            if (clsPmpaType.RSD.CardSeqNo != 0)
                            {
                                ssSpread_Sheet.Cells[30, 12].Text = "현금승인";

                                ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";  //신용카드매출전표
                                ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;                                              //승인번호

                                ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                                ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호

                                ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                                ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호

                            }
                        }
                    }
                    else
                    {
                        ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", nSupAmt);                                            //현금
                    }
                }

                ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", nSupAmt);                                                //합계

                if (strTimeSS != "")
                    ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime + ":" + strTimeSS;
                else
                    ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime;

                //카드영수증 통합
                if (clsPmpaType.RSD.CardSeqNo > 0)
                {
                    Card.GstrCardApprov_Info = CARD.Card_Sign_Info_Set(pDbCon, clsPmpaType.RSD.CardSeqNo, ArgPano);

                    if (Card.GstrCardApprov_Info != "")
                    {
                        ssSpread_Sheet.Cells[31, 13].ColumnSpan = 3;
                        ssSpread_Sheet.Cells[31, 13].HorizontalAlignment = CellHorizontalAlignment.Left;
                        ssSpread_Sheet.Cells[31, 13].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 24); //거래구분

                        ssSpread_Sheet.Cells[32, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 4) + " " + VB.Pstr(Card.GstrCardApprov_Info, "{}", 2); //카드종류/카드번호
                        ssSpread_Sheet.Cells[33, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 20); //승인금액
                        ssSpread_Sheet.Cells[34, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 12); //승인번호
                        ssSpread_Sheet.Cells[35, 12].Text = "72117503"; //가맹점번호
                        ssSpread_Sheet.Cells[36, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 14); //거래일시
                        ssSpread_Sheet.Cells[36, 15].Text = VB.Pstr(VB.Pstr(Card.GstrCardApprov_Info, "{}", 8), "개월", 1);
                    }

                    Card.GstrCardApprov_Info = "";
                }
                #endregion

                #region 영수증 출력일시
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS') Sdate FROM DUAL ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("영수증 출력일시 조회중 문제가 발생했습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (dt.Rows.Count > 0)
                    strTimeSS = VB.Right(dt.Rows[0]["Sdate"].ToString(), 2);

                if (strTimeSS != "")
                    ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime + ":" + strTimeSS;
                else
                    ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime;
                #endregion

                strPrintName = VB.Replace(strPrintName, "(리디렉션)", "");
                strPrintName = VB.Replace(strPrintName, "(리디렉션 1)", "");
                strPrintName = VB.Replace(strPrintName, "(리디렉션 2)", "");
                clsDP.PrintReciptPrint_New(ssSpread, ssSpread_Sheet, strPrintName); //변경 
                

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.ToString());
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

        }


        public void IPD_Sunap_Report_A4_Pay(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, long ArgTrsNo, string strTewon, bool chkSabon)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strTimeSS = string.Empty;
            string strAMTCard = string.Empty;   //카드금액
            string strAMTTot = string.Empty;    //총금액

            int nIlsu = 0, nRow = 0;
            long nSupAmt = 0;   //납부할 금액
            long nTaxAmt = 0;   //부가세 금액

            Card CARD = new Card();
            ComFunc CF = new ComFunc();
            clsPrint CP = new clsPrint();
            clsIument cIMST = new clsIument();
            clsDrugPrint clsDP = new clsDrugPrint();

            FpSpread ssSpread = null;
            SheetView ssSpread_Sheet = null;

            if (ArgIpdNo == 0)
            {
                ComFunc.MsgBox("환자를 선택하여주십시오.");
                return;
            }
            //Dim nHRoomAmt       As Long    '2인실 2018-07-01
            //Dim nHRoomBonin     As Long    '2인실 2018-07-01


            clsDP.SetReciptPrint_New(ref ssSpread, ref ssSpread_Sheet);   //변경
            clsDP.ClearReciptPrint_New(ssSpread, ssSpread_Sheet);         //변경   

            ComFunc.ReadSysDate(pDbCon);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                cIMST.Read_Ipd_Mst_Trans(pDbCon, ArgPano, ArgTrsNo, "");

                if (strTewon == "OK")
                {
                    #region 퇴원금입력시 영수증 발행부분 주석처리
                    //cIMST.Read_Ipd_Mst_Trans(pDbCon, ArgPano, ArgTrsNo, "");

                    //if (clsPmpaType.TIT.GbDRG == "D")
                    //{
                    //    cIMST.Ipd_Trans_PrtAmt_Read_Drg(pDbCon, ArgTrsNo);
                    //    cIMST.Ipd_Tewon_PrtAmt_Gesan_Drg(pDbCon, null, null, ArgPano, ArgIpdNo, ArgTrsNo);
                    //}
                    //else
                    //{
                    //    cIMST.Ipd_Trans_PrtAmt_Read(pDbCon, ArgTrsNo, "");
                    //    cIMST.Ipd_Tewon_PrtAmt_Gesan(pDbCon, ArgPano, ArgIpdNo, ArgTrsNo, "", "");
                    //}

                    //이미납부한 금액

                    //SQL = "";
                    //SQL = SQL + ComNum.VBLF + "SELECT SUM(SUM(CASE WHEN SUNEXT = 'Y88' THEN AMT END)) Y88 ";
                    //SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH  ";
                    //SQL = SQL + ComNum.VBLF + " WHERE TRSNO =" + ArgTrsNo + " ";
                    //SQL = SQL + ComNum.VBLF + "   AND SuNext IN ('Y88') ";
                    //SQL = SQL + ComNum.VBLF + " GROUP BY SuNext ";

                    //SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    //if (SqlErr != "")
                    //{
                    //    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    //    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    //    return;
                    //}

                    //if (dt.Rows.Count > 0)
                    //{
                    //    clsPmpaType.TIT.Amt[51] = (long)VB.Val(dt.Rows[0]["Y88"].ToString().Trim());
                    //}

                    //dt.Dispose();
                    //dt = null; 
                    #endregion

                }

                #region 입원일수 UPDATE
                //재원일수 재 계산함.
                if (clsPmpaType.TIT.OutDate == "")
                    nIlsu = CF.DATE_ILSU(pDbCon, VB.Left(clsPublic.GstrSysDate, 10), VB.Left(clsPmpaType.TIT.InDate, 10)) + 1;     //일수를 계산
                else
                    nIlsu = CF.DATE_ILSU(pDbCon, VB.Left(clsPmpaType.TIT.OutDate, 10), VB.Left(clsPmpaType.TIT.InDate, 10)) + 1;  //일수를 계산

                //nIlsu = CF.DATE_ILSU(pDbCon, VB.Left(clsPmpaType.TIT.OutDate, 10), VB.Left(clsPmpaType.TIT.InDate, 10)) + 1; //일수를 계산
                clsPmpaType.TIT.Ilsu = nIlsu;

                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS         \r\n";
                SQL += "    SET ILSU = " + nIlsu + "                    \r\n";
                SQL += "  WHERE PANO = '" + clsPmpaType.TIT.Pano + "'   \r\n";
                SQL += "    AND TRSNO = " + clsPmpaType.TIT.Trsno + "       ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (clsPmpaType.TIT.M_OutDate == "")
                    nIlsu = CF.DATE_ILSU(pDbCon, VB.Left(clsPublic.GstrSysDate, 10), VB.Left(clsPmpaType.TIT.M_InDate, 10)) + 1;     //일수를 계산
                else
                    nIlsu = CF.DATE_ILSU(pDbCon, VB.Left(clsPmpaType.TIT.M_OutDate, 10), VB.Left(clsPmpaType.TIT.M_InDate, 10)) + 1;  //일수를 계산

                clsPmpaType.TIT.Ilsu = nIlsu;

                SQL = "";
                SQL += " UPDATE " + ComNum.DB_PMPA + "IPD_NEW_MASTER    \r\n";
                SQL += "    SET ILSU = " + nIlsu + "                    \r\n";
                SQL += "  WHERE PANO = '" + clsPmpaType.TIT.Pano + "'  \r\n";
                SQL += "    AND IPDNO = " + clsPmpaType.TIT.Ipdno + "      ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                #endregion

                #region Title Print
                string strPrintName = CP.getPmpaBarCodePrinter(RECEIPT_PRINTER_NAME);

                if (CP.isPmpaBarCodePrinter(strPrintName) == false) { return; }

                ssSpread_Sheet.Cells[0, 4].Text = "입 원";
                ssSpread_Sheet.Cells[0, 12].Text = clsPmpaType.TIT.Pano + " " + VB.Asc(VB.Left(clsPmpaType.TIT.DeptCode, 1)) + VB.Asc(VB.Right(clsPmpaType.TIT.DeptCode, 1)); //바코드

                //공급받는자보관용
                ssSpread_Sheet.Cells[3, 0].Text = clsPmpaType.TIT.Pano;
                ssSpread_Sheet.Cells[3, 4].Text = clsPmpaType.TIT.Sname;

                if (ArgTrsNo == 0)
                    ssSpread_Sheet.Cells[3, 8].Text = clsPmpaType.IMST.InDate + "∼" + clsPmpaType.IMST.OutDate + "(" + clsPmpaType.IMST.Ilsu + ")";
                else
                    ssSpread_Sheet.Cells[3, 8].Text = clsPmpaType.TIT.InDate + "∼" + clsPmpaType.TIT.OutDate + "(" + clsPmpaType.TIT.Ilsu + ")";

                ssSpread_Sheet.Cells[5, 0].Text = CF.READ_DEPTNAMEK(pDbCon, clsPmpaType.TIT.DeptCode);
                ssSpread_Sheet.Cells[5, 4].Text = clsPmpaType.TIT.DrgCode;
                ssSpread_Sheet.Cells[5, 8].Text = clsPmpaType.TIT.RoomCode.ToString();
                ssSpread_Sheet.Cells[5, 11].Text = CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", clsPmpaType.TIT.Bi);

                //수납자 영수증번호
                ssSpread_Sheet.Cells[5, 12].Text = clsType.User.JobName + "(" + clsType.User.IdNumber + ") ";
                #endregion

                //clsPmpaPb.GnHRoomAmt = nHRoomAmt;
                //clsPmpaPb.GnHRoomBonin = nHRoomBonin;
                #region //진료항목
                //진찰료
                nRow = 9;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[1].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[1].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[1].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[1].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[1].ToString("#,### ");    //비급여

                //입원 2,3,4 인실
                nRow += 1;
                // ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaPb.GnHRoomBonin.ToString("#,### ");    //본인부담(급여)
                // ssSpread_Sheet.Cells[nRow, 5].Text = (clsPmpaPb.GnHRoomAmt- clsPmpaPb.GnHRoomBonin).ToString("#,### ");    //조합부담(급여)
                // ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[2].ToString("#,### ");    //전액본인
                // ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[2].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaPb.GnH1RoomAmt.ToString("#,### ");    //비급여
                                                                                                   //입원
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaPb.GnHRoomBonin.ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = (clsPmpaPb.GnHRoomAmt - clsPmpaPb.GnHRoomBonin).ToString("#,### ");    //조합부담(급여)
                //ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[2].ToString("#,### ");    //전액본인
                //ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[2].ToString("#,### ");    //선택진료
                //ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[2].ToString("#,### ");    //비급여
                //입원
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = (clsPmpaType.RPG.Amt5[2] - clsPmpaPb.GnHRoomBonin - clsPmpaPb.GnHUSetBonin).ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = (clsPmpaType.RPG.Amt6[2] - (clsPmpaPb.GnHRoomAmt - clsPmpaPb.GnHRoomBonin) - (clsPmpaPb.GnHUSetAmt - clsPmpaPb.GnHUSetBonin)).ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[2].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[2].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = (clsPmpaType.RPG.Amt2[2] - clsPmpaPb.GnH1RoomAmt).ToString("#,### ");    //비급여

                //식대
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[3].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[3].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[3].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[3].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[3].ToString("#,### ");    //비급여

                //투약 조제 행위
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[4].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[4].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[4].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[4].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[4].ToString("#,### ");    //비급여

                //투약 조제 약품
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[5].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[5].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[5].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[5].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[5].ToString("#,### ");    //비급여

                //주사 행위
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[6].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[6].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[6].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[6].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[6].ToString("#,### ");    //비급여

                //주사 약품
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[7].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[7].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[7].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[7].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[7].ToString("#,### ");    //비급여

                //마취
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[8].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[8].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[8].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[8].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[8].ToString("#,### ");    //비급여

                //처치 수술
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[9].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[9].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[9].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[9].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[9].ToString("#,### ");    //비급여

                //검사료
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[10].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[10].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[10].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[10].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[10].ToString("#,### ");    //비급여

                //영상진단
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[11].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[11].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[11].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[11].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[11].ToString("#,### ");    //비급여

                //방사선치료
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[12].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[12].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[12].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[12].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[12].ToString("#,### ");    //비급여

                //치료재료대
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[13].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[13].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[13].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[13].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[13].ToString("#,### ");    //비급여

                //물리치료
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[14].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[14].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[14].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[14].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[14].ToString("#,### ");    //비급여

                //정신요법
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[15].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[15].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[15].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[15].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[15].ToString("#,### ");    //비급여

                //전혈
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[16].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[16].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[16].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[16].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[16].ToString("#,### ");    //비급여

                //CT
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[17].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[17].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[17].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[17].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[17].ToString("#,### ");    //비급여

                //MRI
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[18].ToString("#,### ");    ///본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[18].ToString("#,### ");    ///조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[18].ToString("#,### ");    ///전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[18].ToString("#,### ");    ///선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[18].ToString("#,### ");    ///비급여

                //초음파
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[19].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[19].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[19].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[19].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[19].ToString("#,### ");    //비급여

                //보철
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[20].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[20].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[20].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[20].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[20].ToString("#,### ");    //비급여

                //증명료
                nRow += 2;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[22].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[22].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[22].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[22].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[22].ToString("#,### ");    //비급여

                //병실차액
                nRow += 1;
                ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[21].ToString("#,### ");    //본인부담(급여)
                ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[21].ToString("#,### ");    //조합부담(급여)
                ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[21].ToString("#,### ");    //전액본인
                ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[21].ToString("#,### ");    //선택진료
                ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[21].ToString("#,### ");    //비급여
                if (clsPmpaType.TIT.GBHU == "Y")
                {
                    //호스피스 병실료
                    nRow += 1;
                    ssSpread_Sheet.Cells[nRow, 0].Text = "호스피스병실료(정액)";
                    ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaPb.GnHUSetBonin.ToString("#,### ");            //본인부담(급여)
                    ssSpread_Sheet.Cells[nRow, 5].Text = (clsPmpaPb.GnHUSetAmt - clsPmpaPb.GnHUSetBonin).ToString("#,### ");          //조합부담(급여)
                    ssSpread_Sheet.Cells[nRow, 6].Text = "";                                            //전액본인
                    ssSpread_Sheet.Cells[nRow, 8].Text = "";                                            //선택진료
                    ssSpread_Sheet.Cells[nRow, 10].Text = "";                                           //비급여
                }

                if (clsPmpaType.TIT.GbDRG == "D")
                {
                    //DRG
                    nRow += 1;
                    ssSpread_Sheet.Cells[nRow, 0].Text = "포괄수가진료비";
                    ssSpread_Sheet.Cells[nRow, 3].Text = DRG.GnDrgBonAmt.ToString("#,### ");            //본인부담(급여)
                    ssSpread_Sheet.Cells[nRow, 5].Text = DRG.GnDrgJohapAmt.ToString("#,### ");          //조합부담(급여)
                    ssSpread_Sheet.Cells[nRow, 6].Text = "";                                            //전액본인
                    ssSpread_Sheet.Cells[nRow, 8].Text = "";                                            //선택진료
                    ssSpread_Sheet.Cells[nRow, 10].Text = "";                                           //비급여

                    long nSunAmt_Tot = DRG.GnGs50Amt_T + DRG.GnGs80Amt_T + DRG.GnGs90Amt_T;
                    long nSunAmt_Jhp = DRG.GnGs50Amt_J + DRG.GnGs80Amt_J + DRG.GnGs90Amt_J;
                    long nSunAmt_BOn = DRG.GnGs50Amt_B + DRG.GnGs80Amt_B + DRG.GnGs90Amt_B;

                    //선별급여 표시
                    nRow += 1;
                    ssSpread_Sheet.Cells[nRow, 0].Text = "선별급여";
                    ssSpread_Sheet.Cells[nRow, 3].Text = nSunAmt_BOn.ToString("#,### ");    //본인부담(급여)
                    ssSpread_Sheet.Cells[nRow, 5].Text = nSunAmt_Jhp.ToString("#,### ");    //조합부담(급여)
                    ssSpread_Sheet.Cells[nRow, 6].Text = "";    //전액본인
                    ssSpread_Sheet.Cells[nRow, 8].Text = "";    //선택진료
                    ssSpread_Sheet.Cells[nRow, 10].Text = "";    //비급여

                    long nHapTot = DRG.GnDRG_Amt2 + DRG.GnDrgFoodAmt[0] + DRG.GnDrgFoodAmt[1] + DRG.GnDrgRoomAmt[0] + DRG.GnDrgRoomAmt[1];
                    long nHapJhp = DRG.GnDrgJohapAmt + DRG.GnDrgFoodAmt[1] + DRG.GnDrgRoomAmt[1];
                    long nHapBon = DRG.GnDrgBonAmt + DRG.GnDrgFoodAmt[0] + DRG.GnDrgRoomAmt[0] + DRG.GnDrg열외군금액_Bon; //(nHapTot - nHapJhp);

                    nHapTot += nSunAmt_Tot;
                    nHapJhp += nSunAmt_Jhp;
                    nHapBon += nSunAmt_BOn;

                    //열외군금액
                    if (DRG.GnDrg열외군금액 > 0)
                    {
                        nRow += 1;
                        ssSpread_Sheet.Cells[nRow, 0].Text = "DRG열외군차액";
                        ssSpread_Sheet.Cells[nRow, 3].Text = DRG.GnDrg열외군금액_Bon.ToString("#,### ");    //본인부담(급여)
                        ssSpread_Sheet.Cells[nRow, 5].Text = "";    //조합부담(급여)
                        ssSpread_Sheet.Cells[nRow, 6].Text = "";    //전액본인
                        ssSpread_Sheet.Cells[nRow, 8].Text = "";    //선택진료
                        ssSpread_Sheet.Cells[nRow, 10].Text = "";    //비급여
                    }

                    //합계
                    ssSpread_Sheet.Cells[37, 3].Text = nHapBon.ToString("#,### ");                  //본인부담(급여)
                    ssSpread_Sheet.Cells[37, 5].Text = nHapJhp.ToString("#,### ");                  //조합부담(급여)
                    ssSpread_Sheet.Cells[37, 6].Text = DRG.GnGs100Amt.ToString("#,### ");           //전액본인
                    ssSpread_Sheet.Cells[37, 8].Text = DRG.GnDrgSelTAmt.ToString("#,### ");         //선택진료
                    ssSpread_Sheet.Cells[37, 10].Text = DRG.GnDrgBiFAmt.ToString("#,### ");          //비급여
                }
                else
                {
                    if (clsPmpaType.RPG.Amt1[24] > 0)
                    {
                        //선별급여
                        nRow += 1;
                        ssSpread_Sheet.Cells[nRow, 0].Text = "선별급여";
                        ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[24].ToString("#,### ");    //본인부담(급여)
                        ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[24].ToString("#,### ");    //조합부담(급여)
                        ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[24].ToString("#,### ");    //전액본인
                        ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[24].ToString("#,### ");    //선택진료
                        ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[24].ToString("#,### ");    //비급여
                    }

                    //합계
                    ssSpread_Sheet.Cells[37, 3].Text = (clsPmpaType.RPG.Amt5[50] - clsPmpaType.TIT.Amt[64]).ToString("#,### ");    //본인부담(급여)
                    ssSpread_Sheet.Cells[37, 5].Text = clsPmpaType.RPG.Amt6[50].ToString("#,### ");                                //조합부담(급여)
                    ssSpread_Sheet.Cells[37, 6].Text = clsPmpaType.RPG.Amt4[50].ToString("#,### ");                                //전액본인
                    ssSpread_Sheet.Cells[37, 8].Text = clsPmpaType.RPG.Amt3[50].ToString("#,### ");                                //선택진료
                    ssSpread_Sheet.Cells[37, 10].Text = clsPmpaType.RPG.Amt2[50].ToString("#,### ");                                //비급여

                }

                if (clsPmpaType.RPG.Amt2[49] > 0 || clsPmpaType.RPG.Amt3[49] > 0 || clsPmpaType.RPG.Amt4[49] > 0 || clsPmpaType.RPG.Amt5[49] > 0 || clsPmpaType.RPG.Amt6[49] > 0)
                {
                    //기타
                    nRow += 1;
                    ssSpread_Sheet.Cells[nRow, 0].Text = "기타";
                    ssSpread_Sheet.Cells[nRow, 3].Text = clsPmpaType.RPG.Amt5[49].ToString("#,### ");    //본인부담(급여)
                    ssSpread_Sheet.Cells[nRow, 5].Text = clsPmpaType.RPG.Amt6[49].ToString("#,### ");    //조합부담(급여)
                    ssSpread_Sheet.Cells[nRow, 6].Text = clsPmpaType.RPG.Amt4[49].ToString("#,### ");    //전액본인
                    ssSpread_Sheet.Cells[nRow, 8].Text = clsPmpaType.RPG.Amt3[49].ToString("#,### ");    //선택진료
                    ssSpread_Sheet.Cells[nRow, 10].Text = clsPmpaType.RPG.Amt2[49].ToString("#,### ");    //비급여                   
                }

                //상한액 초과금
                ssSpread_Sheet.Cells[38, 3].Text = clsPmpaType.TIT.SangAmt.ToString("#,### ");

                #endregion

                #region 금액산정부분

                #region 납부할 금액 산정
                if (clsPmpaType.TIT.OgPdBun == "H")
                {
                    nSupAmt = (clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64] - clsPmpaType.TIT.Amt[53] - clsPmpaType.TIT.Amt[62] - (clsPmpaType.TIT.Amt[51] + clsPmpaType.TIT.Amt[54] + clsPmpaType.TIT.Amt[56]));
                    nSupAmt = (long)Math.Truncate(nSupAmt / 10.0) * 10; //원단위 절사
                }
                else if (clsPmpaType.TIT.VCode == "V206" || clsPmpaType.TIT.VCode == "V231")
                {
                    nSupAmt = (clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64] - clsPmpaType.TIT.Amt[53]);
                    nSupAmt = nSupAmt - (clsPmpaType.TIT.Amt[51] + clsPmpaType.TIT.Amt[54] + clsPmpaType.TIT.Amt[56]);

                    if (clsPmpaType.TIT.Amt[52] > 0)
                    {
                        nSupAmt = nSupAmt - clsPmpaType.TIT.Amt[52];
                        //원단위절사는 최종처리
                        nSupAmt = (long)Math.Truncate(nSupAmt / 10.0) * 10;
                    }
                    else
                    {
                        nSupAmt = (long)Math.Round((nSupAmt * 0.5), 1, MidpointRounding.AwayFromZero);
                    }
                }
                else if (clsPmpaType.TIT.FCode == "F010")
                {
                    nSupAmt = clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64] - clsPmpaType.TIT.Amt[53];
                    nSupAmt = nSupAmt - (clsPmpaType.TIT.Amt[51] + clsPmpaType.TIT.Amt[54] + clsPmpaType.TIT.Amt[56]);

                    if (clsPmpaType.TIT.Amt[62] > 0)
                    {
                        nSupAmt = nSupAmt - clsPmpaType.TIT.Amt[62];
                        //원단위절사는 최종처리
                        nSupAmt = (long)Math.Truncate(nSupAmt / 10.0) * 10;
                    }
                    else
                    {
                        nSupAmt = (long)Math.Round(nSupAmt / 10.0, 1, MidpointRounding.AwayFromZero) * 10;
                    }
                }
                else
                {
                    nSupAmt = clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64] - clsPmpaType.TIT.Amt[53];
                    nSupAmt = nSupAmt - (clsPmpaType.TIT.Amt[51] + clsPmpaType.TIT.Amt[54] + clsPmpaType.TIT.Amt[56]);
                    nSupAmt = (long)Math.Truncate(nSupAmt / 10.0) * 10;
                }

                nTaxAmt = clsPmpaType.TIT.Amt[67];  //부가세 금액

                #endregion

                long nTotAmt = clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64];
                long nBonAmt = clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[64] - clsPmpaType.TIT.Amt[53];

                ssSpread_Sheet.Cells[7, 13].Text = nTotAmt.ToString("#,### ");                 //총진료비
                ssSpread_Sheet.Cells[9, 13].Text = nBonAmt.ToString("#,### ");                 //본인부담금
                ssSpread_Sheet.Cells[12, 13].Text = clsPmpaType.TIT.Amt[51].ToString("#,### "); //이미납부한 금액

                if (clsPmpaType.TIT.Amt[54] > 0 && clsPmpaType.TIT.Amt[56] > 0)
                {
                    ssSpread_Sheet.Cells[13, 11].Text = "감액 ";
                    ssSpread_Sheet.Cells[13, 11].Text += clsPmpaType.TIT.Amt[54].ToString("#,### "); //감액
                    ssSpread_Sheet.Cells[13, 13].Text = "미수 ";
                    ssSpread_Sheet.Cells[13, 13].Text += clsPmpaType.TIT.Amt[56].ToString("#,### "); //미수
                }
                else if (clsPmpaType.TIT.Amt[54] > 0)
                {
                    ssSpread_Sheet.Cells[13, 11].Text = "감   액";
                    ssSpread_Sheet.Cells[13, 13].Text = clsPmpaType.TIT.Amt[54].ToString("#,### "); //감액
                }
                else if (clsPmpaType.TIT.Amt[56] > 0)
                {
                    ssSpread_Sheet.Cells[13, 11].Text = "미   수";
                    ssSpread_Sheet.Cells[13, 13].Text = clsPmpaType.TIT.Amt[56].ToString("#,### "); //미수
                }


                if (clsPmpaType.TIT.OgPdBun == "H")
                {
                    ssSpread_Sheet.Cells[14, 11].Text = "지 원 금"; //지원금
                    ssSpread_Sheet.Cells[14, 13].Text = clsPmpaType.TIT.Amt[62].ToString("#,### "); //지원금
                }
                else if (clsPmpaType.TIT.VCode == "V206" || clsPmpaType.TIT.VCode == "V231")
                {
                    if (clsPmpaType.TIT.Amt[62] != 0)
                    {
                        ssSpread_Sheet.Cells[14, 11].Text = "지 원 금"; //지원금

                        if (clsPmpaType.TIT.FCode == "F008")
                        {
                            ssSpread_Sheet.Cells[14, 13].Text = (Math.Truncate(clsPmpaType.TIT.Amt[62] / 10.0) * 10).ToString("#,### ");
                        }
                        else
                        {
                            ssSpread_Sheet.Cells[14, 13].Text = (Math.Round((clsPmpaType.TIT.Amt[62] * 0.5), 1, MidpointRounding.AwayFromZero)).ToString("#,### ");
                        }
                    }
                }
                else if (clsPmpaType.TIT.FCode == "F010")
                {
                    if (clsPmpaType.TIT.Amt[62] != 0)
                    {
                        ssSpread_Sheet.Cells[14, 11].Text = "지 원 금"; //지원금
                        ssSpread_Sheet.Cells[14, 13].Text = (Math.Truncate(clsPmpaType.TIT.Amt[62] / 10.0) * 10).ToString("#,### ");
                    }
                }

                nSupAmt = nSupAmt + nTaxAmt;

                ssSpread_Sheet.Cells[15, 13].Text = nSupAmt.ToString("#,### ");     //납부할 금액
                ssSpread_Sheet.Cells[20, 13].Text = nSupAmt.ToString("#,### ");     //수납금액 합계

                //급여 22종 이거나 건보자격중 OGPDBUN "F"인 경우의 장애인(BOHUN == "3")만 기금 지원
                //if (clsPmpaType.TIT.OgPdBun == "F")
                //if (clsPmpaType.TIT.Bohun == "3" && ((clsPmpaType.TIT.OgPdBun == "F" && VB.Left(clsPmpaType.TIT.Bi, 1) == "1") || clsPmpaType.TIT.Bi == "22"))
                //{
                //    ssSpread_Sheet.Cells[12, 11].Text = "장애인기금";
                //    ssSpread_Sheet.Cells[12, 13].Text = (clsPmpaType.TIT.Amt[55] - nSupAmt).ToString("#,### "); //지원금
                //}

                if (chkSabon == true)
                {
                    //ssSpread_Sheet.Cells[12, 11].Font.Size = 18;
                    ssSpread_Sheet.Cells[14, 13].Text += ComNum.VBLF + "사    본";
                }

                if (clsPmpaType.TIT.Amt[54] > 0)
                {
                    ssSpread_Sheet.Cells[24, 12].Text = "감액관계";
                    ssSpread_Sheet.Cells[24, 12].Text += ":" + CF.Read_Bcode_Name(pDbCon, "BAS_감액코드명", clsPmpaType.TIT.GbGameK) + "";
                }

                if (clsPmpaType.RSD.Gubun == "1")   //신용카드
                {
                    if (clsPmpaType.RD.OrderGb == "2")
                    {   //취소
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);                //카드
                        ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (nSupAmt - (clsPmpaType.RSD.TotAmt * -1)));  //현금
                    }
                    else
                    {   //승인
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);                     //카드
                        ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", nSupAmt - clsPmpaType.RSD.TotAmt);           //현금
                    }
                }
                else if (clsPmpaType.RSD.Gubun == "2")  //현금영수
                {
                    if (clsPmpaType.RD.OrderGb == "2")
                    {
                        ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);                    //현금영수증
                        ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (nSupAmt - (clsPmpaType.RSD.TotAmt * -1)));      //현금

                        if (clsPmpaType.RSD.CardSeqNo != 0)
                        {
                            ssSpread_Sheet.Cells[30, 12].Text = "현금승인취소";

                            ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";  //신용카드매출전표
                            ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;                                              //승인번호

                            ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                            ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;  //승인번호

                            ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                            ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;  //승인번호

                        }
                    }
                    else
                    {
                        ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);                         //현금영수증
                        ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", nSupAmt - clsPmpaType.RSD.TotAmt);               //현금

                        if (clsPmpaType.RSD.CardSeqNo != 0)
                        {
                            ssSpread_Sheet.Cells[30, 12].Text = "현금승인";

                            ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";  //신용카드매출전표
                            ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;                                              //승인번호

                            ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                            ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호

                            ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                            ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호

                        }
                    }
                }
                else
                {
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", nSupAmt);                                            //현금
                }

                ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", nSupAmt);                                                //합계

                if (strTimeSS != "")
                    ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime + ":" + strTimeSS;
                else
                    ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime;

                //카드영수증 통합
                if (clsPmpaType.RSD.CardSeqNo > 0)
                {
                    Card.GstrCardApprov_Info = CARD.Card_Sign_Info_Set(pDbCon, clsPmpaType.RSD.CardSeqNo, ArgPano);

                    if (Card.GstrCardApprov_Info != "")
                    {
                        ssSpread_Sheet.Cells[31, 13].ColumnSpan = 3;
                        ssSpread_Sheet.Cells[31, 13].HorizontalAlignment = CellHorizontalAlignment.Left;
                        ssSpread_Sheet.Cells[31, 13].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 24); //거래구분

                        ssSpread_Sheet.Cells[32, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 4) + " " + VB.Pstr(Card.GstrCardApprov_Info, "{}", 2); //카드종류/카드번호
                        ssSpread_Sheet.Cells[33, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 20); //승인금액
                        ssSpread_Sheet.Cells[34, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 12); //승인번호
                        ssSpread_Sheet.Cells[35, 12].Text = "72117503"; //가맹점번호
                        ssSpread_Sheet.Cells[36, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 14); //거래일시
                        ssSpread_Sheet.Cells[36, 15].Text = VB.Pstr(VB.Pstr(Card.GstrCardApprov_Info, "{}", 8), "개월", 1);
                    }

                    Card.GstrCardApprov_Info = "";
                }
                #endregion

                #region 영수증 출력일시
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS') Sdate FROM DUAL ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("영수증 출력일시 조회중 문제가 발생했습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (dt.Rows.Count > 0)
                    strTimeSS = VB.Right(dt.Rows[0]["Sdate"].ToString(), 2);

                if (strTimeSS != "")
                    ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime + ":" + strTimeSS;
                else
                    ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime;
                #endregion

                //strPrintName = VB.Replace(strPrintName, "(리디렉션)", "");
                //strPrintName = VB.Replace(strPrintName, "(리디렉션 1)", "");
                //strPrintName = VB.Replace(strPrintName, "(리디렉션 2)", "");
                //clsDP.PrintReciptPrint_New(ssSpread, ssSpread_Sheet, strPrintName); //변경 

                ssSpread_Sheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewResult));
                ssSpread_Sheet.PrintInfo.PaperSize = ((System.Drawing.Printing.PaperSize)(resources.GetObject("resource.PaperSize")));
                ssSpread_Sheet.PrintInfo.Margin.Top = 12;
                ssSpread_Sheet.PrintInfo.Margin.Bottom = 20;
                ssSpread_Sheet.PrintInfo.ShowColor = false;
                ssSpread_Sheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
                ssSpread_Sheet.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
                ssSpread_Sheet.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
                ssSpread_Sheet.PrintInfo.ShowBorder = false;
                ssSpread_Sheet.PrintInfo.ShowGrid = false;
                ssSpread_Sheet.PrintInfo.ShowShadows = false;
                ssSpread_Sheet.PrintInfo.UseMax = true;
                ssSpread_Sheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
                ssSpread_Sheet.PrintInfo.UseSmartPrint = false;
                ssSpread_Sheet.PrintInfo.ShowPrintDialog = false;
                ssSpread_Sheet.PrintInfo.Opacity = 0;


                //Bitmap bmp = new Bitmap(2480, 3508);
                Bitmap bmp = new Bitmap(840, 1177);
                // ArgPano, ArgTrsNo
                string file_name = "";  //생성할 전자처방전 png파일명
                string file_path = "C:\\CMC\\exe\\Image\\";
                file_name = string.Format("{0}{1}{2}.Png", ArgPano, "-" + ArgTrsNo,"");
                file_path = string.Format("{0}{1}", file_path, file_name);

                Image bg = Image.FromFile(@"C:\CMC\exe\Image\영수증.JPG");
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // 배경이미지 그리기
                    g.Clear(Color.White);
                    //g.DrawImage(Image.FromFile(@"C:\Temp\bg.png"), new Point(0, 0));
                    g.DrawImage(bg, new Rectangle(0, 0, bmp.Width, bmp.Height), new Rectangle(0, 0, bg.Width, bg.Height), GraphicsUnit.Pixel);

                    // 데이터 표시하기
                    ssSpread.OwnerPrintDraw(g, new Rectangle(20, 20, 820, 1400), ssSpread.ActiveSheetIndex, 1);
                    g.Dispose();
                }
                bg.Dispose();

                //// 최종목적의 이미지를 파일로 저장
                bmp.Save(@file_path, System.Drawing.Imaging.ImageFormat.Png);

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.ToString());
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        /// <summary>
        /// 중간납, 환불, 퇴원금, 가퇴원시 출력 영수증
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgTrsNo"></param>
        /// <param name="strCode"></param>
        /// <param name="strAmt"></param>
        public void IPD_Sunap_Report_One(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, long ArgTrsNo, string strCode, string strAmt)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            string strTimeSS = string.Empty;
            string strAMTCard = string.Empty;   //카드금액
            string strAMTTot = string.Empty;    //총금액
            string strBunName = string.Empty;
            
            long nSupAmt = 0;   //납부할 금액
            
            Card CARD = new Card();
            ComFunc CF = new ComFunc();
            clsPrint CP = new clsPrint();
            clsIument cIMST = new clsIument();
            clsDrugPrint clsDP = new clsDrugPrint();

            FpSpread ssSpread = null;
            SheetView ssSpread_Sheet = null;

            if (ArgIpdNo == 0)
            {
                ComFunc.MsgBox("환자를 선택하여주십시오.");
                return;
            }

            clsDP.SetReciptPrint(ref ssSpread, ref ssSpread_Sheet);
            clsDP.ClearReciptPrint(ssSpread, ssSpread_Sheet);

            ComFunc.ReadSysDate(pDbCon);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                #region Title Print
                string strPrintName = CP.getPmpaBarCodePrinter(RECEIPT_PRINTER_NAME);

                if (CP.isPmpaBarCodePrinter(strPrintName) == false) { return; }

                ssSpread_Sheet.Cells[0, 4].Text = "입 원";

                //공급받는자보관용
                ssSpread_Sheet.Cells[3, 0].Text = clsPmpaType.TIT.Pano;
                ssSpread_Sheet.Cells[3, 4].Text = clsPmpaType.TIT.Sname;

                if (ArgTrsNo == 0)
                    ssSpread_Sheet.Cells[3, 8].Text = clsPmpaType.IMST.InDate + "∼" + clsPmpaType.IMST.OutDate + "(" + clsPmpaType.IMST.Ilsu + ")";
                else
                    ssSpread_Sheet.Cells[3, 8].Text = clsPmpaType.TIT.InDate + "∼" + clsPmpaType.TIT.OutDate + "(" + clsPmpaType.TIT.Ilsu + ")";

                ssSpread_Sheet.Cells[5, 0].Text = CF.READ_DEPTNAMEK(pDbCon, clsPmpaType.TIT.DeptCode);
                ssSpread_Sheet.Cells[5, 4].Text = clsPmpaType.TIT.DrgCode;
                ssSpread_Sheet.Cells[5, 8].Text = clsPmpaType.TIT.RoomCode.ToString();
                ssSpread_Sheet.Cells[5, 11].Text = CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", clsPmpaType.TIT.Bi);

                //수납자 영수증번호
                ssSpread_Sheet.Cells[5, 12].Text = clsType.User.JobName + "(" + clsType.User.IdNumber + ") ";

                switch (strCode)
                {
                    case "Y85": strBunName = "가퇴원금"; break;
                    case "Y87": strBunName = "중 간 납"; break;
                    case "Y89": strBunName = "퇴 원 금"; break;
                    case "Y91": strBunName = "환 불 금"; break;
                    default:
                        break;
                }
                ssSpread_Sheet.Cells[11, 11].Text = strBunName;
                ssSpread_Sheet.Cells[11, 13].Text = string.Format("{0:#,###}", strAmt);

                #endregion
                nSupAmt = (long)VB.Val(VB.Replace(strAmt, ",", ""));

                #region 금액산정부분                
                if (clsPmpaType.RSD.Gubun == "1")
                {
                    if (clsPmpaType.RD.OrderGb == "2")
                    {
                        ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);    //카드
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (nSupAmt - (clsPmpaType.RSD.TotAmt * -1)));    //현금
                    }
                    else
                    {
                        ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//카드
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", nSupAmt - clsPmpaType.RSD.TotAmt);//현금
                    }
                }
                else if (clsPmpaType.RSD.Gubun == "2")
                {
                    if (clsPmpaType.RD.OrderGb == "2")
                    {
                        ssSpread_Sheet.Cells[16, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//현금영수증
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//현금

                        if (clsPmpaType.RSD.CardSeqNo != 0)
                        {
                            ssSpread_Sheet.Cells[28, 12].Text = "현금승인취소";
                            ssSpread_Sheet.Cells[29, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                            ssSpread_Sheet.Cells[32, 12].Text = clsPmpaType.RD.ApprovalNo;  //승인번호
                        }
                    }
                    else
                    {
                        ssSpread_Sheet.Cells[16, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt); //현금영수증
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt); //현금

                        if (clsPmpaType.RSD.CardSeqNo != 0)
                        {
                            ssSpread_Sheet.Cells[28, 12].Text = "현금승인";
                            ssSpread_Sheet.Cells[29, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                            ssSpread_Sheet.Cells[32, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                        }
                    }
                }
                else
                {
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", nSupAmt);//현금
                }

                if (strTimeSS != "")
                    ssSpread_Sheet.Cells[40, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime + ":" + strTimeSS;
                else
                    ssSpread_Sheet.Cells[40, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime;

                //카드영수증 통합
                if (clsPmpaType.RSD.CardSeqNo > 0)
                {
                    Card.GstrCardApprov_Info = CARD.Card_Sign_Info_Set(pDbCon, clsPmpaType.RSD.CardSeqNo, ArgPano);

                    if (Card.GstrCardApprov_Info != "")
                    {
                        ssSpread_Sheet.Cells[29, 13].ColumnSpan = 3;
                        ssSpread_Sheet.Cells[29, 13].HorizontalAlignment = CellHorizontalAlignment.Left;
                        ssSpread_Sheet.Cells[29, 13].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 24); //거래구분

                        ssSpread_Sheet.Cells[30, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 4) + " " + VB.Pstr(Card.GstrCardApprov_Info, "{}", 2); //카드종류/카드번호
                        ssSpread_Sheet.Cells[31, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 20); //승인금액
                        ssSpread_Sheet.Cells[32, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 12); //승인번호
                        ssSpread_Sheet.Cells[33, 12].Text = "72117503"; //가맹점번호
                        ssSpread_Sheet.Cells[34, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 14); //거래일시
                        ssSpread_Sheet.Cells[34, 15].Text = VB.Pstr(VB.Pstr(Card.GstrCardApprov_Info, "{}", 8), "개월", 1);
                    }

                    Card.GstrCardApprov_Info = "";
                }
                #endregion

                #region 영수증 출력일시
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS') Sdate FROM DUAL ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("영수증 출력일시 조회중 문제가 발생했습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (dt.Rows.Count > 0)
                    strTimeSS = VB.Right(dt.Rows[0]["Sdate"].ToString(), 2);

                if (strTimeSS != "")
                    ssSpread_Sheet.Cells[40, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime + ":" + strTimeSS;
                else
                    ssSpread_Sheet.Cells[40, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime;
                #endregion

                clsDP.PrintReciptPrint(ssSpread, ssSpread_Sheet, strPrintName);

                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.ToString());
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

        }
        // 병실료 2인실 분류에따른 추가
        public void IPD_Sunap_Report_One_New(PsmhDb pDbCon, string ArgPano, long ArgIpdNo, long ArgTrsNo, string strCode, string strAmt, string argSetInfo = "")
        {

            //argSetInfo는 사용 중 문제 없을 경우 제거할 예정
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            string strTimeSS = string.Empty;
            string strAMTCard = string.Empty;   //카드금액
            string strAMTTot = string.Empty;    //총금액
            string strBunName = string.Empty;

            long nSupAmt = 0;   //납부할 금액

            Card CARD = new Card();
            ComFunc CF = new ComFunc();
            clsPrint CP = new clsPrint();
            clsIument cIMST = new clsIument();
            clsDrugPrint clsDP = new clsDrugPrint();

            FpSpread ssSpread = null;
            SheetView ssSpread_Sheet = null;

            if (ArgIpdNo == 0)
            {
                ComFunc.MsgBox("환자를 선택하여주십시오.");
                return;
            }

            clsDP.SetReciptPrint_New(ref ssSpread, ref ssSpread_Sheet);
            clsDP.ClearReciptPrint_New(ssSpread, ssSpread_Sheet);

            ComFunc.ReadSysDate(pDbCon);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                #region Title Print
                string strPrintName = CP.getPmpaBarCodePrinter(RECEIPT_PRINTER_NAME);

                if (CP.isPmpaBarCodePrinter(strPrintName) == false) { return; }

                ssSpread_Sheet.Cells[0, 4].Text = "입 원";

                //공급받는자보관용
                ssSpread_Sheet.Cells[3, 0].Text = clsPmpaType.TIT.Pano;
                ssSpread_Sheet.Cells[3, 4].Text = clsPmpaType.TIT.Sname;

                if (ArgTrsNo == 0)
                    ssSpread_Sheet.Cells[3, 8].Text = clsPmpaType.IMST.InDate + "∼" + clsPmpaType.IMST.OutDate + "(" + clsPmpaType.IMST.Ilsu + ")";
                else
                    ssSpread_Sheet.Cells[3, 8].Text = clsPmpaType.TIT.InDate + "∼" + clsPmpaType.TIT.OutDate + "(" + clsPmpaType.TIT.Ilsu + ")";

                ssSpread_Sheet.Cells[5, 0].Text = CF.READ_DEPTNAMEK(pDbCon, clsPmpaType.TIT.DeptCode);
                ssSpread_Sheet.Cells[5, 4].Text = clsPmpaType.TIT.DrgCode;
                ssSpread_Sheet.Cells[5, 8].Text = clsPmpaType.TIT.RoomCode.ToString();
                ssSpread_Sheet.Cells[5, 11].Text = CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", clsPmpaType.TIT.Bi);

                //수납자 영수증번호
                ssSpread_Sheet.Cells[5, 12].Text = clsType.User.JobName + "(" + clsType.User.IdNumber + ") ";

                switch (strCode)
                {
                    case "Y85": strBunName = "가퇴원금"; break;
                    case "Y87": strBunName = "중 간 납"; break;
                    case "Y89": strBunName = "퇴 원 금"; break;
                    case "Y91": strBunName = "환 불 금"; break;
                    default:
                        break;
                }
                ssSpread_Sheet.Cells[13, 11].Text = strBunName;
                ssSpread_Sheet.Cells[13, 13].Text = string.Format("{0:#,###}", strAmt);

                if (argSetInfo == "Y")
                {

                    //가퇴원, 중간납 인쇄 시 스프레드 안내문구 표시(2021-09-13)
                    Print_IPD_Information_Set(ref ssSpread_Sheet, strCode);
                }

                #endregion
                nSupAmt = (long)VB.Val(VB.Replace(strAmt, ",", ""));

                #region 금액산정부분                
                if (clsPmpaType.RSD.Gubun == "1")
                {
                    if (clsPmpaType.RD.OrderGb == "2")
                    {
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);    //카드
                        ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (nSupAmt - (clsPmpaType.RSD.TotAmt * -1)));    //현금
                    }
                    else
                    {
                        ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//카드
                        ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", nSupAmt - clsPmpaType.RSD.TotAmt);//현금
                    }
                }
                else if (clsPmpaType.RSD.Gubun == "2")
                {
                    if (clsPmpaType.RD.OrderGb == "2")
                    {
                        ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//현금영수증
                        ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//현금

                        if (clsPmpaType.RSD.CardSeqNo != 0)
                        {
                            ssSpread_Sheet.Cells[30, 12].Text = "현금승인취소";
                            ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                            ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;  //승인번호
                        }
                    }
                    else
                    {
                        ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt); //현금영수증
                        ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt); //현금

                        if (clsPmpaType.RSD.CardSeqNo != 0)
                        {
                            ssSpread_Sheet.Cells[30, 12].Text = "현금승인";
                            ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                            ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                        }
                    }
                }
                else
                {
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", nSupAmt);//현금
                }

                if (strTimeSS != "")
                    ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime + ":" + strTimeSS;
                else
                    ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime;

                //카드영수증 통합
                if (clsPmpaType.RSD.CardSeqNo > 0)
                {
                    Card.GstrCardApprov_Info = CARD.Card_Sign_Info_Set(pDbCon, clsPmpaType.RSD.CardSeqNo, ArgPano);

                    if (Card.GstrCardApprov_Info != "")
                    {
                        ssSpread_Sheet.Cells[31, 13].ColumnSpan = 3;
                        ssSpread_Sheet.Cells[31, 13].HorizontalAlignment = CellHorizontalAlignment.Left;
                        ssSpread_Sheet.Cells[31, 13].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 24); //거래구분

                        ssSpread_Sheet.Cells[32, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 4) + " " + VB.Pstr(Card.GstrCardApprov_Info, "{}", 2); //카드종류/카드번호
                        ssSpread_Sheet.Cells[33, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 20); //승인금액
                        ssSpread_Sheet.Cells[34, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 12); //승인번호
                        ssSpread_Sheet.Cells[35, 12].Text = "72117503"; //가맹점번호
                        ssSpread_Sheet.Cells[36, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 14); //거래일시
                        ssSpread_Sheet.Cells[36, 15].Text = VB.Pstr(VB.Pstr(Card.GstrCardApprov_Info, "{}", 8), "개월", 1);
                    }

                    Card.GstrCardApprov_Info = "";
                }
                #endregion

                #region 영수증 출력일시
                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS') Sdate FROM DUAL ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("영수증 출력일시 조회중 문제가 발생했습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (dt.Rows.Count > 0)
                    strTimeSS = VB.Right(dt.Rows[0]["Sdate"].ToString(), 2);

                if (strTimeSS != "")
                    ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime + ":" + strTimeSS;
                else
                    ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime;
                #endregion

                strPrintName = VB.Replace(strPrintName, "(리디렉션)", "");
                strPrintName = VB.Replace(strPrintName, "(리디렉션 1)", "");
                strPrintName = VB.Replace(strPrintName, "(리디렉션 2)", "");

                clsDP.PrintReciptPrint_New(ssSpread, ssSpread_Sheet, strPrintName);


                return;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.ToString());
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        public void Report_Print_Misu(PsmhDb pDbCon, string ArgPano, string ArgSName, string ArgDept, string ArgBi, string ArgAmt, bool ArgPrt)
        {
            int nSeqNo = 0;
            string strTimeSS = string.Empty;
            
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            
            ComFunc CF          = new ComFunc();
            clsPrint CP         = new clsPrint();
            clsIument cIMST     = new clsIument();
            clsDrugPrint clsDP  = new clsDrugPrint();
            clsPmpaQuery cPQ    = new clsPmpaQuery();

            FpSpread ssSpread = null;
            SheetView ssSpread_Sheet = null;

            if (ArgPano == "")
            {
                ComFunc.MsgBox("환자를 선택하여주십시오.");
                return;
            }

            clsDP.SetReciptPrint(ref ssSpread, ref ssSpread_Sheet);
            clsDP.ClearReciptPrint(ssSpread, ssSpread_Sheet);

            ComFunc.ReadSysDate(pDbCon);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                #region  마지막 영수번호 읽기
                SQL = "";
                SQL += " SELECT MAX(SEQNO) SEQNO                        ";
                SQL += "   FROM " + ComNum.DB_PMPA + "OPD_SUNAP         ";
                SQL += "  WHERE ACTDATE = TRUNC(SYSDATE)                ";
                SQL += "    AND PART = '" + clsType.User.IdNumber + "'  ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nSeqNo = Convert.ToInt16(VB.Val(dt.Rows[0]["SEQNO"].ToString()));
                }

                if (nSeqNo == 0)
                {
                    nSeqNo = 1;
                }
                else
                {
                    nSeqNo += 1;
                }

                dt.Dispose();
                dt = null;
                #endregion
                
                #region 외래수납 Data Insert
                if (ArgPrt == false)
                {
                    cPQ.Ins_Opd_Sunap(pDbCon, ArgPano, ArgAmt, nSeqNo, ArgDept, ArgBi, clsType.User.IdNumber, "미수입금", "출력안함", clsPmpaType.RSD.Gubun);
                }
                else
                {
                    if (ComFunc.MsgBoxQ("영수증을 출력하겠습니까?", "선택", MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        cPQ.Ins_Opd_Sunap(pDbCon, ArgPano, ArgAmt, nSeqNo, ArgDept, ArgBi, clsType.User.IdNumber, "미수입금", "출력안함", clsPmpaType.RSD.Gubun);
                        return;
                    }
                    else
                    {
                        cPQ.Ins_Opd_Sunap(pDbCon, ArgPano, ArgAmt, nSeqNo, ArgDept, ArgBi, clsType.User.IdNumber, "미수입금", "", clsPmpaType.RSD.Gubun);
                    }
                }
                #endregion

                #region Title Print
                //영수증출력
                CP.getPrinter_Chk(RECEIPT_PRINTER_NAME);
                string strPrintName = CP.getPmpaBarCodePrinter(RECEIPT_PRINTER_NAME);

                if (strPrintName.Trim() == "") { return; }

                ssSpread_Sheet.Cells[0, 4].Text = "외 래";
                ssSpread_Sheet.Cells[0, 12].Text = ArgPano + VB.Asc(VB.Left(ArgDept, 1)) + VB.Asc(VB.Right(ArgDept, 1)); //바코드

                //공급받는자보관용
                ssSpread_Sheet.Cells[3, 0].Text = ArgPano;
                ssSpread_Sheet.Cells[3, 4].Text = ArgSName;
                ssSpread_Sheet.Cells[5, 0].Text = CF.READ_DEPTNAMEK(pDbCon, ArgDept);
                ssSpread_Sheet.Cells[5, 4].Text = "";
                ssSpread_Sheet.Cells[5, 8].Text = "";
                ssSpread_Sheet.Cells[5, 11].Text = CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", ArgBi);

                //수납자 영수증번호
                ssSpread_Sheet.Cells[5, 12].Text = clsType.User.JobName + "(" + clsType.User.IdNumber + ") ";

                #endregion

                #region 금액산정부분
                ssSpread_Sheet.Cells[11, 11].Text = "미수입금";
                ssSpread_Sheet.Cells[11, 13].Text = string.Format("{0:#,###}", ArgAmt);
                ssSpread_Sheet.Cells[13, 13].Text = string.Format("{0:#,###}", ArgAmt);
                #endregion

                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS') Sdate FROM DUAL ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (dt.Rows.Count > 0)
                    strTimeSS = VB.Right(dt.Rows[0]["Sdate"].ToString(), 2);

                if (strTimeSS != "")
                    ssSpread_Sheet.Cells[40, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime + ":" + strTimeSS;
                else
                    ssSpread_Sheet.Cells[40, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime;

                clsDP.PrintReciptPrint(ssSpread, ssSpread_Sheet, strPrintName);

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.ToString());
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

        }
        public void Report_Print_Misu_New(PsmhDb pDbCon, string ArgPano, string ArgSName, string ArgDept, string ArgBi, string ArgAmt, bool ArgPrt)
        {
            int nSeqNo = 0;
            string strTimeSS = string.Empty;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            ComFunc CF = new ComFunc();
            clsPrint CP = new clsPrint();
            clsIument cIMST = new clsIument();
            clsDrugPrint clsDP = new clsDrugPrint();
            clsPmpaQuery cPQ = new clsPmpaQuery();

            FpSpread ssSpread = null;
            SheetView ssSpread_Sheet = null;

            if (ArgPano == "")
            {
                ComFunc.MsgBox("환자를 선택하여주십시오.");
                return;
            }

            clsDP.SetReciptPrint_New(ref ssSpread, ref ssSpread_Sheet);
            clsDP.ClearReciptPrint_New(ssSpread, ssSpread_Sheet);

            ComFunc.ReadSysDate(pDbCon);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                #region  마지막 영수번호 읽기
                SQL = "";
                SQL += " SELECT MAX(SEQNO) SEQNO                        ";
                SQL += "   FROM " + ComNum.DB_PMPA + "OPD_SUNAP         ";
                SQL += "  WHERE ACTDATE = TRUNC(SYSDATE)                ";
                SQL += "    AND PART = '" + clsType.User.IdNumber + "'  ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nSeqNo = Convert.ToInt16(VB.Val(dt.Rows[0]["SEQNO"].ToString()));
                }

                if (nSeqNo == 0)
                {
                    nSeqNo = 1;
                }
                else
                {
                    nSeqNo += 1;
                }

                dt.Dispose();
                dt = null;
                #endregion

                #region 외래수납 Data Insert
                if (ArgPrt == true)
                {
                    cPQ.Ins_Opd_Sunap(pDbCon, ArgPano, ArgAmt, nSeqNo, ArgDept, ArgBi, clsType.User.IdNumber, "미수입금", "출력안함", clsPmpaType.RSD.Gubun);
                    return;
                }
                else
                {
                    if (ComFunc.MsgBoxQ("영수증을 출력하겠습니까?", "선택", MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        cPQ.Ins_Opd_Sunap(pDbCon, ArgPano, ArgAmt, nSeqNo, ArgDept, ArgBi, clsType.User.IdNumber, "미수입금", "출력안함", clsPmpaType.RSD.Gubun);
                        return;
                    }
                    else
                    {
                        cPQ.Ins_Opd_Sunap(pDbCon, ArgPano, ArgAmt, nSeqNo, ArgDept, ArgBi, clsType.User.IdNumber, "미수입금", "", clsPmpaType.RSD.Gubun);
                    }
                }
                #endregion

                #region Title Print
                //영수증출력
                CP.getPrinter_Chk(RECEIPT_PRINTER_NAME);
                string strPrintName = CP.getPmpaBarCodePrinter(RECEIPT_PRINTER_NAME);

                if (strPrintName.Trim() == "") { return; }

                ssSpread_Sheet.Cells[0, 4].Text = "외 래";
                ssSpread_Sheet.Cells[0, 12].Text = ArgPano + VB.Asc(VB.Left(ArgDept, 1)) + VB.Asc(VB.Right(ArgDept, 1)); //바코드

                //공급받는자보관용
                ssSpread_Sheet.Cells[3, 0].Text = ArgPano;
                ssSpread_Sheet.Cells[3, 4].Text = ArgSName;
                ssSpread_Sheet.Cells[5, 0].Text = CF.READ_DEPTNAMEK(pDbCon, ArgDept);
                ssSpread_Sheet.Cells[5, 4].Text = "";
                ssSpread_Sheet.Cells[5, 8].Text = "";
                ssSpread_Sheet.Cells[5, 11].Text = CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", ArgBi);

                //수납자 영수증번호
                ssSpread_Sheet.Cells[5, 12].Text = clsType.User.JobName + "(" + clsType.User.IdNumber + ") ";

                #endregion

                #region 금액산정부분
                ssSpread_Sheet.Cells[13, 11].Text = "미수입금";
                ssSpread_Sheet.Cells[13, 13].Text = string.Format("{0:#,###}", ArgAmt);
                ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", ArgAmt);
                #endregion

                SQL = "";
                SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS') Sdate FROM DUAL ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                strPrintName = VB.Replace(strPrintName, "(리디렉션)", "");
                strPrintName = VB.Replace(strPrintName, "(리디렉션 1)", "");
                strPrintName = VB.Replace(strPrintName, "(리디렉션 2)", "");

                if (dt.Rows.Count > 0)
                    strTimeSS = VB.Right(dt.Rows[0]["Sdate"].ToString(), 2);

                if (strTimeSS != "")
                    ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime + ":" + strTimeSS;
                else
                    ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + clsPublic.GstrSysDate.Substring(0, 4) + VB.Space(15) + clsPublic.GstrSysDate.Substring(5, 2) + VB.Space(15) + clsPublic.GstrSysDate.Substring(8, 2) + VB.Space(15) + clsPublic.GstrSysTime;

                clsDP.PrintReciptPrint_New(ssSpread, ssSpread_Sheet, strPrintName);

                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.ToString());
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgOpdNo">외래관리번호</param>
        /// <param name="ArgPtno">등록번호</param>
        /// <param name="ArgName">수진자명</param>
        /// <param name="ArgGwa"><진료과목코드</param>
        /// <param name="ArgResv">예약일자</param>
        /// <param name="ArgDr">예약의사</param>
        /// <param name="ArgBi">환자구분코드</param>
        /// <param name="ArgTel"></param>
        /// <param name="ArgBdate"></param>
        /// <param name="ArgCard"></param>
        /// <param name="ArgSunap"></param>
        /// <param name="ArgDept"></param>
        /// <param name="pic_Sign"></param>
        /// <param name="ArgGubunX"></param>
        /// <param name="ArgGelCode"></param>
        /// <param name="ArgMcode"></param>
        /// <param name="ArgVcode"></param>
        public void Report_Print_Jupsu_Sign(PsmhDb pDbCon, long ArgOpdNo, string ArgPtno, string ArgName, string ArgGwa, string ArgResv, string ArgDr, string ArgBi, string ArgTel, string ArgBdate, string ArgCard, string ArgSunap, string ArgDept, PictureBox pic_Sign, bool ArgGubunX, string ArgGelCode, string ArgMcode, string ArgVcode, FpSpread o)
        {
            FarPoint.Win.Spread.FpSpread ssSpread = null;
            FarPoint.Win.Spread.SheetView ssSpread_Sheet = null;

            ComFunc CF = new ComFunc();
            clsOumsadChk COC = new ComPmpaLibB.clsOumsadChk();
            clsDrugPrint clsDP = new clsDrugPrint();
            PrintDocument pd = new PrintDocument();
            PageSettings ps = new PageSettings();
            PrintController pc = new StandardPrintController();

            DataTable DtP = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            long nX = 0;
            long nY = 0;
            string strBi = "";
            int nSeqNo = 0;
            int nFlu_Cnt = 0;
            string strTimeSS = "";


            clsDP.SetReciptPrint(ref ssSpread, ref ssSpread_Sheet);
            clsDP.ClearReciptPrint(ssSpread, ssSpread_Sheet);

            nX = 0 + clsPmpaPb.GnPrintXSet;
            nY = 620 + clsPmpaPb.GnPrintYSet;

            ComFunc.ReadSysDate(pDbCon);
            //strBi = CF.Read_Bcode_Name(pDbCon, "", ArgBi, "");

            #region //수납순번 가져오기
            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(MAX(SEQNO), 0) SEQNO FROM " + ComNum.DB_PMPA + "OPD_SUNAP ";
            SQL += ComNum.VBLF + "  WHERE ACTDATE   = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND PART      = '" + clsType.User.JobPart + "' ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            if (DtP.Rows.Count > 0)
                nSeqNo = Convert.ToInt32(DtP.Rows[0]["SEQNO"].ToString());

            if (nSeqNo == 0)
                nSeqNo = 1;
            else
                nSeqNo += 1;

            DtP.Dispose();
            DtP = null;
            #endregion

            #region //플루예방접종여부
            SQL = "";
            SQL += ComNum.VBLF + " SELECT COUNT(PANO) CNT FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE ACTDATE   = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND GbFlu_Vac = 'Y' ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            if (DtP.Rows.Count > 0)
                nFlu_Cnt = Convert.ToInt32(DtP.Rows[0]["CNT"].ToString());

            DtP.Dispose();
            DtP = null;
            #endregion

            #region //수납시간 초
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS') Sdate FROM DUAL ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            if (DtP.Rows.Count > 0)
                strTimeSS = VB.Right(DtP.Rows[0]["Sdate"].ToString(), 2);

            DtP.Dispose();
            DtP = null;
            #endregion

            #region //INSERT/UPDATE
            if (DialogResult.No == ComFunc.MsgBoxQ("영수증을 출력하겠습니까?", "선택"))
            {
                if (clsPmpaPb.GstrJeaSunap == "YES")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP ";
                    SQL += ComNum.VBLF + "        (ACTDATE, OPDNO, PANO, AMT, ";
                    SQL += ComNum.VBLF + "         PART, SEQNO, STIME, ";
                    SQL += ComNum.VBLF + "         BIGO, REMARK, AMT1, ";
                    SQL += ComNum.VBLF + "         AMT2, AMT3, AMT4, ";
                    SQL += ComNum.VBLF + "         AMT5, SEQNO2, DEPTCODE, ";
                    SQL += ComNum.VBLF + "         BI, CARDGB, CardAmt, ";
                    SQL += ComNum.VBLF + "         GbSPC, GelCode, MCode, ";
                    SQL += ComNum.VBLF + "         VCode,BDan,CDan) ";
                    SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "          " + ArgOpdNo + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgPtno + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT7 + ", "; //진찰료 본인부담액
                    SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                    SQL += ComNum.VBLF + "          " + nSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                    SQL += ComNum.VBLF + "         '출력안함', ";
                    SQL += ComNum.VBLF + "         '" + ArgSunap + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT3 + ", "; //진찰료 총액
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT4 + ", "; //진찰료 조합부담
                    SQL += ComNum.VBLF + "          " + (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT5 + ", "; //진찰료 감액
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT6 + ", "; //진찰료 미수
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GDrSeqNo + ", ";  //의사별 접수번호
                    SQL += ComNum.VBLF + "         '" + ArgDept.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgBi.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.RSD.Gubun + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.RSD.TotAmt + ", ";
                    SQL += ComNum.VBLF + "         '0', ";
                    SQL += ComNum.VBLF + "         '" + ArgGelCode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgMcode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgVcode + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnBTruncLastAmt + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnHTruncLastAmt + " ) ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("OPD_SUNAP INSERT Error !!");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        return;
                    }
                }

                clsPmpaPb.GnPatAmt = 0; //환자에게 돈을 받는 금액
                clsPmpaPb.GnJanAmt = 0; //잔액

                //예방접종신플접수증
                if (ArgGubunX == true)
                    pd.PrinterSettings.PrinterName = PrinterSettings.InstalledPrinters[clsPmpaPb.GnPrtIpdNo3].ToString().Trim(); //신용카드

                return;
            }
            else
            {
                if (clsPmpaPb.GstrJeaSunap == "YES")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP ";
                    SQL += ComNum.VBLF + "        (ACTDATE, OPDNO, PANO, AMT, ";
                    SQL += ComNum.VBLF + "         PART, SEQNO, STIME, ";
                    SQL += ComNum.VBLF + "         BIGO, REMARK, AMT1, ";
                    SQL += ComNum.VBLF + "         AMT2, AMT3, AMT4, ";
                    SQL += ComNum.VBLF + "         AMT5, SEQNO2, DEPTCODE, ";
                    SQL += ComNum.VBLF + "         BI,CARDGB, CardAmt, ";
                    SQL += ComNum.VBLF + "         GbSPC, GelCode,MCode, ";
                    SQL += ComNum.VBLF + "         VCode,BDan,CDan) ";
                    SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "          " + ArgOpdNo + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgPtno + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT7 + ", ";
                    SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                    SQL += ComNum.VBLF + "          " + nSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                    SQL += ComNum.VBLF + "         ' ', ";
                    SQL += ComNum.VBLF + "         '" + ArgSunap + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT3 + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT4 + ", ";
                    SQL += ComNum.VBLF + "          " + (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT5 + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT6 + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GDrSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgDept.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgBi.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.RSD.Gubun + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.RSD.TotAmt + ", ";
                    SQL += ComNum.VBLF + "         '0', ";
                    SQL += ComNum.VBLF + "         '" + ArgGelCode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgMcode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgVcode + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnBTruncLastAmt + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnHTruncLastAmt + " ) ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("OPD_SUNAP INSERT Error !!");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        return;
                    }

                    clsPmpaPb.GnPatAmt = 0; //환자에게 돈을 받는 금액
                    clsPmpaPb.GnJanAmt = 0; //잔액
                }
            }

            #endregion

            //예약접수증
            if (ArgResv.Trim() != "" && ArgSunap != "예약취소")
            {
                //2018.06.18 박병규 : 파라미터 변경
                //ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, clsPmpaType.TOM.DrCode, ArgResv, o);
                ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, ArgGwa, CF.READ_DEPTNAMEK(pDbCon, ArgGwa) ,  ArgDr, ArgResv,"", o);
            }

            //영수증출력
            clsPrint CP = new clsPrint();
            Card CC = new Card();

            //영수증출력
            CP.getPrinter_Chk(RECEIPT_PRINTER_NAME);
            string strPrintName = CP.getPmpaBarCodePrinter(RECEIPT_PRINTER_NAME);

            if (strPrintName.Trim() == "") { return; }

            if (ArgDr == "1107" || ArgDr == "1125")
                ArgGwa = "류마티스내과";

            ssSpread_Sheet.Cells[0, 4].Text = "외래";
            if (ArgTel != "NO" || ArgTel.Trim() != "")
            {
                if (ArgGwa == "PD")
                    ssSpread_Sheet.Cells[0, 4].Text = "외래" + "(전화접수)";
                else
                    ssSpread_Sheet.Cells[0, 4].Text = "외래" + "(전화예약)";
            }

            ssSpread_Sheet.Cells[0, 12].Text = ArgPtno + VB.Asc(VB.Left(ArgDept, 1)) + VB.Asc(VB.Right(ArgDept, 1)); //바코드

            if (clsPmpaPb.GstrErJobFlag == "OK")
            {
                if (clsPmpaPb.GnNight1 == "2")
                    ssSpread_Sheet.Cells[3, 13].Text = "√";
                else if (clsPmpaPb.GnNight1 == "1")
                    ssSpread_Sheet.Cells[3, 13].Text = VB.Space(11) + "√";
            }

            if (clsPmpaPb.GstrCopyPrint == "OK")
            {
                //2018.06.20 박병규 : 출력위치변경
                //ssSpread_Sheet.Cells[22, 12].Text = "사 본";
                ssSpread_Sheet.Cells[0, 4].Text += " [사본]";
            }

            //공급받는자보관용
            ssSpread_Sheet.Cells[3, 0].Text = ArgPtno;
            ssSpread_Sheet.Cells[3, 4].Text = ArgName;
            ssSpread_Sheet.Cells[3, 8].Text = clsPublic.GstrSysDate;
            ssSpread_Sheet.Cells[5, 0].Text = CF.READ_DEPTNAMEK(pDbCon, ArgGwa);
            ssSpread_Sheet.Cells[5, 11].Text = CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", ArgBi);

            //진찰료 급여본인부담금
            if (ArgSunap != "예약비")
                ssSpread_Sheet.Cells[9, 3].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT1);
            else if (ArgSunap == "예약비")
                ssSpread_Sheet.Cells[29, 3].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT1);

            //진찰료 총액에 선택진료비 포함되어었음
            clsPmpaPb.gnJinAMT3 = clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT2;

            //합계 급여본인부담금
            ssSpread_Sheet.Cells[35, 3].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4));

            //합계 급여공단부담금
            ssSpread_Sheet.Cells[35, 5].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT4);

            //<금액산정내용>***************************************************************************************************************
            //진료비총액
            ssSpread_Sheet.Cells[7, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT3 + clsPmpaPb.gnJinAMT2));

            //환자부담총액
            ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);

            //납부한금액
            if (clsPmpaType.RSD.Gubun == "1")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {
                    ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//카드
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - (clsPmpaType.RSD.TotAmt * -1)));//현금
                }
                else
                {
                    ssSpread_Sheet.Cells[15, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//카드
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaType.RSD.TotAmt));//현금
                }
            }
            else if (clsPmpaType.RSD.Gubun == "2")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {
                    ssSpread_Sheet.Cells[16, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//현금영수증
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - (clsPmpaType.RSD.TotAmt * -1)));//현금

                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[28, 12].Text = "현금승인취소";
                        ssSpread_Sheet.Cells[29, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[32, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
                else
                {
                    ssSpread_Sheet.Cells[16, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//현금영수증
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaType.RSD.TotAmt));//현금

                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[28, 12].Text = "현금승인";
                        ssSpread_Sheet.Cells[29, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[32, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
            }
            else
            {
                ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);//현금
            }

            ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);//합계
                                                                                                //<금액산정내용>***************************************************************************************************************

            //감액
            if (clsPmpaPb.gnJinAMT5 > 0)
            {
                ssSpread_Sheet.Cells[11, 12].Text = "감액";
                ssSpread_Sheet.Cells[11, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT5);//감액
            }

            //미수
            if (clsPmpaPb.gnJinAMT6 > 0)
            {
                ssSpread_Sheet.Cells[12, 12].Text = "미수금";
                ssSpread_Sheet.Cells[12, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT6);//미수
            }

            ssSpread_Sheet.Cells[40, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime;

            //카드영수증 통합
            if (clsPmpaType.RSD.CardSeqNo > 0 && ArgSunap != "대리접수")
            {
                Card.GstrCardApprov_Info = CC.Card_Sign_Info_Set(pDbCon, clsPmpaType.RSD.CardSeqNo, ArgPtno);

                if (Card.GstrCardApprov_Info != "")
                {
                    ssSpread_Sheet.Cells[29, 13].ColumnSpan = 3;
                    ssSpread_Sheet.Cells[29, 13].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[29, 13].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 24); //거래구분

                    ssSpread_Sheet.Cells[30, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[30, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[30, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 4) + " " + VB.Pstr(Card.GstrCardApprov_Info, "{}", 2); //카드종류/카드번호
                    ssSpread_Sheet.Cells[31, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[31, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[31, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 20); //승인금액
                    ssSpread_Sheet.Cells[32, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[32, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[32, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 12); //승인번호
                    ssSpread_Sheet.Cells[33, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[33, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[33, 12].Text = "72117503"; //가맹점번호
                    ssSpread_Sheet.Cells[34, 12].ColumnSpan = 2;
                    ssSpread_Sheet.Cells[34, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[34, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 14); //거래일시
                    ssSpread_Sheet.Cells[34, 15].Text = VB.Pstr(VB.Pstr(Card.GstrCardApprov_Info, "{}", 8), "개월", 1);
                }

                Card.GstrCardApprov_Info = "";
            }

            clsDP.PrintReciptPrint(ssSpread, ssSpread_Sheet, strPrintName);
        }

        public void Report_Print_Jupsu_Sign_New(PsmhDb pDbCon, long ArgOpdNo, string ArgPtno, string ArgName, string ArgGwa, string ArgResv, string ArgDr, string ArgBi, string ArgTel, string ArgBdate, string ArgCard, string ArgSunap, string ArgDept, PictureBox pic_Sign, bool ArgGubunX, string ArgGelCode, string ArgMcode, string ArgVcode, FpSpread o)
        {
            FarPoint.Win.Spread.FpSpread ssSpread = null;
            FarPoint.Win.Spread.SheetView ssSpread_Sheet = null;

            ComFunc CF = new ComFunc();
            clsOumsadChk COC = new ComPmpaLibB.clsOumsadChk();
            clsDrugPrint clsDP = new clsDrugPrint();
            PrintDocument pd = new PrintDocument();
            PageSettings ps = new PageSettings();
            PrintController pc = new StandardPrintController();

            DataTable DtP = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            long nX = 0;
            long nY = 0;
            string strBi = "";
            int nSeqNo = 0;
            int nFlu_Cnt = 0;
            string strTimeSS = "";


            clsDP.SetReciptPrint_New(ref ssSpread, ref ssSpread_Sheet);
            clsDP.ClearReciptPrint_New(ssSpread, ssSpread_Sheet);

            nX = 0 + clsPmpaPb.GnPrintXSet;
            nY = 620 + clsPmpaPb.GnPrintYSet;

            ComFunc.ReadSysDate(pDbCon);
            //strBi = CF.Read_Bcode_Name(pDbCon, "", ArgBi, "");

            #region //수납순번 가져오기
            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(MAX(SEQNO), 0) SEQNO FROM " + ComNum.DB_PMPA + "OPD_SUNAP ";
            SQL += ComNum.VBLF + "  WHERE ACTDATE   = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND PART      = '" + clsType.User.JobPart + "' ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            if (DtP.Rows.Count > 0)
                nSeqNo = Convert.ToInt32(DtP.Rows[0]["SEQNO"].ToString());

            if (nSeqNo == 0)
                nSeqNo = 1;
            else
                nSeqNo += 1;

            DtP.Dispose();
            DtP = null;
            #endregion

            #region //플루예방접종여부
            SQL = "";
            SQL += ComNum.VBLF + " SELECT COUNT(PANO) CNT FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE ACTDATE   = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND GbFlu_Vac = 'Y' ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            if (DtP.Rows.Count > 0)
                nFlu_Cnt = Convert.ToInt32(DtP.Rows[0]["CNT"].ToString());

            DtP.Dispose();
            DtP = null;
            #endregion

            #region //수납시간 초
            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD HH24:MI:SS') Sdate FROM DUAL ";
            SqlErr = clsDB.GetDataTable(ref DtP, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtP.Dispose();
                DtP = null;
                return;
            }

            if (DtP.Rows.Count > 0)
                strTimeSS = VB.Right(DtP.Rows[0]["Sdate"].ToString(), 2);

            DtP.Dispose();
            DtP = null;
            #endregion

            #region //INSERT/UPDATE
            if (DialogResult.No == ComFunc.MsgBoxQ("영수증을 출력하겠습니까?", "선택"))
            {
                if (clsPmpaPb.GstrJeaSunap == "YES")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP ";
                    SQL += ComNum.VBLF + "        (ACTDATE, OPDNO, PANO, AMT, ";
                    SQL += ComNum.VBLF + "         PART, SEQNO, STIME, ";
                    SQL += ComNum.VBLF + "         BIGO, REMARK, AMT1, ";
                    SQL += ComNum.VBLF + "         AMT2, AMT3, AMT4, ";
                    SQL += ComNum.VBLF + "         AMT5, SEQNO2, DEPTCODE, ";
                    SQL += ComNum.VBLF + "         BI, CARDGB, CardAmt, ";
                    SQL += ComNum.VBLF + "         GbSPC, GelCode, MCode, ";
                    SQL += ComNum.VBLF + "         VCode,BDan,CDan) ";
                    SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "          " + ArgOpdNo + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgPtno + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT7 + ", "; //진찰료 본인부담액
                    SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                    SQL += ComNum.VBLF + "          " + nSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                    SQL += ComNum.VBLF + "         '출력안함', ";
                    SQL += ComNum.VBLF + "         '" + ArgSunap + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT3 + ", "; //진찰료 총액
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT4 + ", "; //진찰료 조합부담
                    SQL += ComNum.VBLF + "          " + (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT5 + ", "; //진찰료 감액
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT6 + ", "; //진찰료 미수
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GDrSeqNo + ", ";  //의사별 접수번호
                    SQL += ComNum.VBLF + "         '" + ArgDept.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgBi.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.RSD.Gubun + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.RSD.TotAmt + ", ";
                    SQL += ComNum.VBLF + "         '0', ";
                    SQL += ComNum.VBLF + "         '" + ArgGelCode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgMcode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgVcode + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnBTruncLastAmt + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnHTruncLastAmt + " ) ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("OPD_SUNAP INSERT Error !!");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        return;
                    }
                }

                clsPmpaPb.GnPatAmt = 0; //환자에게 돈을 받는 금액
                clsPmpaPb.GnJanAmt = 0; //잔액

                //예방접종신플접수증
                if (ArgGubunX == true)
                    pd.PrinterSettings.PrinterName = PrinterSettings.InstalledPrinters[clsPmpaPb.GnPrtIpdNo3].ToString().Trim(); //신용카드

                return;
            }
            else
            {
                if (clsPmpaPb.GstrJeaSunap == "YES")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP ";
                    SQL += ComNum.VBLF + "        (ACTDATE, OPDNO, PANO, AMT, ";
                    SQL += ComNum.VBLF + "         PART, SEQNO, STIME, ";
                    SQL += ComNum.VBLF + "         BIGO, REMARK, AMT1, ";
                    SQL += ComNum.VBLF + "         AMT2, AMT3, AMT4, ";
                    SQL += ComNum.VBLF + "         AMT5, SEQNO2, DEPTCODE, ";
                    SQL += ComNum.VBLF + "         BI,CARDGB, CardAmt, ";
                    SQL += ComNum.VBLF + "         GbSPC, GelCode,MCode, ";
                    SQL += ComNum.VBLF + "         VCode,BDan,CDan) ";
                    SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD'), ";
                    SQL += ComNum.VBLF + "          " + ArgOpdNo + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgPtno + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT7 + ", ";
                    SQL += ComNum.VBLF + "         '" + clsType.User.IdNumber + "', ";
                    SQL += ComNum.VBLF + "          " + nSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + clsPublic.GstrSysTime + "', ";
                    SQL += ComNum.VBLF + "         ' ', ";
                    SQL += ComNum.VBLF + "         '" + ArgSunap + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT3 + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT4 + ", ";
                    SQL += ComNum.VBLF + "          " + (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4) + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT5 + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.gnJinAMT6 + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GDrSeqNo + ", ";
                    SQL += ComNum.VBLF + "         '" + ArgDept.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgBi.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + clsPmpaType.RSD.Gubun + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaType.RSD.TotAmt + ", ";
                    SQL += ComNum.VBLF + "         '0', ";
                    SQL += ComNum.VBLF + "         '" + ArgGelCode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgMcode + "', ";
                    SQL += ComNum.VBLF + "         '" + ArgVcode + "', ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnBTruncLastAmt + ", ";
                    SQL += ComNum.VBLF + "          " + clsPmpaPb.GnHTruncLastAmt + " ) ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox("OPD_SUNAP INSERT Error !!");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        return;
                    }

                    clsPmpaPb.GnPatAmt = 0; //환자에게 돈을 받는 금액
                    clsPmpaPb.GnJanAmt = 0; //잔액
                }
            }

            #endregion

            //예약접수증
            if (ArgResv.Trim() != "" && ArgSunap != "예약취소")
            {
                //2018.06.18 박병규 : 파라미터 변경
                //ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, clsPmpaType.TOM.DeptCode, ArgGwa, clsPmpaType.TOM.DrCode, ArgResv, o);
                ResJupsu_Print(pDbCon, "0", ArgPtno, ArgName, ArgGwa, CF.READ_DEPTNAMEK(pDbCon, ArgGwa), ArgDr, ArgResv,"", o);
            }

            //영수증출력
            clsPrint CP = new clsPrint();
            Card CC = new Card();

            //영수증출력
            CP.getPrinter_Chk(RECEIPT_PRINTER_NAME);
            string strPrintName = CP.getPmpaBarCodePrinter(RECEIPT_PRINTER_NAME);

            if (strPrintName.Trim() == "") { return; }

            if (ArgDr == "1107" || ArgDr == "1125")
                ArgGwa = "류마티스내과";

            ssSpread_Sheet.Cells[0, 4].Text = "외래";
            if (ArgTel != "NO" || ArgTel.Trim() != "")
            {
                if (ArgGwa == "PD")
                    ssSpread_Sheet.Cells[0, 4].Text = "외래" + "(전화접수)";
                else
                    ssSpread_Sheet.Cells[0, 4].Text = "외래" + "(전화예약)";
            }

            ssSpread_Sheet.Cells[0, 12].Text = ArgPtno + VB.Asc(VB.Left(ArgDept, 1)) + VB.Asc(VB.Right(ArgDept, 1)); //바코드

            if (clsPmpaPb.GstrErJobFlag == "OK")
            {
                if (clsPmpaPb.GnNight1 == "2")
                    ssSpread_Sheet.Cells[3, 13].Text = "√";
                else if (clsPmpaPb.GnNight1 == "1")
                    ssSpread_Sheet.Cells[3, 13].Text = VB.Space(11) + "√";
            }

            if (clsPmpaPb.GstrCopyPrint == "OK")
            {
                //2018.06.20 박병규 : 출력위치변경
                //ssSpread_Sheet.Cells[22, 12].Text = "사 본";
                ssSpread_Sheet.Cells[0, 4].Text += " [사본]";
            }

            //공급받는자보관용
            ssSpread_Sheet.Cells[3, 0].Text = ArgPtno;
            ssSpread_Sheet.Cells[3, 4].Text = ArgName;
            ssSpread_Sheet.Cells[3, 8].Text = clsPublic.GstrSysDate;
            ssSpread_Sheet.Cells[5, 0].Text = CF.READ_DEPTNAMEK(pDbCon, ArgGwa);
            ssSpread_Sheet.Cells[5, 11].Text = CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", ArgBi);

            //진찰료 급여본인부담금
            if (ArgSunap != "예약비")
                ssSpread_Sheet.Cells[9, 3].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT1);
            else if (ArgSunap == "예약비")
                ssSpread_Sheet.Cells[31, 3].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT1);

            //진찰료 총액에 선택진료비 포함되어었음
            clsPmpaPb.gnJinAMT3 = clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT2;

            //합계 급여본인부담금
            ssSpread_Sheet.Cells[37, 3].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT3 - clsPmpaPb.gnJinAMT4));

            //합계 급여공단부담금
            ssSpread_Sheet.Cells[37, 5].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT4);

            //<금액산정내용>***************************************************************************************************************
            //진료비총액
            ssSpread_Sheet.Cells[7, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT3 + clsPmpaPb.gnJinAMT2));

            //환자부담총액
            ssSpread_Sheet.Cells[9, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);

            //납부한금액
            if (clsPmpaType.RSD.Gubun == "1")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//카드
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - (clsPmpaType.RSD.TotAmt * -1)));//현금
                }
                else
                {
                    ssSpread_Sheet.Cells[17, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//카드
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaType.RSD.TotAmt));//현금
                }
            }
            else if (clsPmpaType.RSD.Gubun == "2")
            {
                if (clsPmpaType.RD.OrderGb == "2")
                {
                    ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt * -1);//현금영수증
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - (clsPmpaType.RSD.TotAmt * -1)));//현금

                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[30, 12].Text = "현금승인취소";
                        ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
                else
                {
                    ssSpread_Sheet.Cells[18, 13].Text = string.Format("{0:#,###}", clsPmpaType.RSD.TotAmt);//현금영수증
                    ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", (clsPmpaPb.gnJinAMT7 - clsPmpaType.RSD.TotAmt));//현금

                    if (clsPmpaType.RSD.CardSeqNo != 0)
                    {
                        ssSpread_Sheet.Cells[30, 12].Text = "현금승인";
                        ssSpread_Sheet.Cells[31, 11].Text = VB.Left(Card.GstrCashCard, Card.GstrCashCard.Length - 4) + "****";//신용카드매출전표
                        ssSpread_Sheet.Cells[34, 12].Text = clsPmpaType.RD.ApprovalNo;//승인번호
                    }
                }
            }
            else
            {
                ssSpread_Sheet.Cells[19, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);//현금
            }

            ssSpread_Sheet.Cells[20, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT7);//합계
                                                                                                //<금액산정내용>***************************************************************************************************************

            //감액
            if (clsPmpaPb.gnJinAMT5 > 0)
            {
                ssSpread_Sheet.Cells[13, 12].Text = "감액";
                ssSpread_Sheet.Cells[13, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT5);//감액
            }

            //미수
            if (clsPmpaPb.gnJinAMT6 > 0)
            {
                ssSpread_Sheet.Cells[14, 12].Text = "미수금";
                ssSpread_Sheet.Cells[14, 13].Text = string.Format("{0:#,###}", clsPmpaPb.gnJinAMT6);//미수
            }

            ssSpread_Sheet.Cells[42, 0].Text = VB.Space(3) + VB.Left(clsPublic.GstrSysDate, 4) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Space(15) + VB.Mid(clsPublic.GstrSysDate, 9, 2) + VB.Space(15) + clsPublic.GstrSysTime;

            //카드영수증 통합
            if (clsPmpaType.RSD.CardSeqNo > 0 && ArgSunap != "대리접수")
            {
                Card.GstrCardApprov_Info = CC.Card_Sign_Info_Set(pDbCon, clsPmpaType.RSD.CardSeqNo, ArgPtno);

                if (Card.GstrCardApprov_Info != "")
                {
                    ssSpread_Sheet.Cells[31, 13].ColumnSpan = 3;
                    ssSpread_Sheet.Cells[31, 13].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[31, 13].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 24); //거래구분

                    ssSpread_Sheet.Cells[32, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[32, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[32, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 4) + " " + VB.Pstr(Card.GstrCardApprov_Info, "{}", 2); //카드종류/카드번호
                    ssSpread_Sheet.Cells[33, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[33, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[33, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 20); //승인금액
                    ssSpread_Sheet.Cells[34, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[34, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[34, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 12); //승인번호
                    ssSpread_Sheet.Cells[35, 12].ColumnSpan = 4;
                    ssSpread_Sheet.Cells[35, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[35, 12].Text = "72117503"; //가맹점번호
                    ssSpread_Sheet.Cells[36, 12].ColumnSpan = 2;
                    ssSpread_Sheet.Cells[36, 12].HorizontalAlignment = CellHorizontalAlignment.Left;
                    ssSpread_Sheet.Cells[36, 12].Text = VB.Pstr(Card.GstrCardApprov_Info, "{}", 14); //거래일시
                    ssSpread_Sheet.Cells[36, 15].Text = VB.Pstr(VB.Pstr(Card.GstrCardApprov_Info, "{}", 8), "개월", 1);
                }

                Card.GstrCardApprov_Info = "";
            }
            strPrintName = VB.Replace(strPrintName, "(리디렉션)", "");
            strPrintName = VB.Replace(strPrintName, "(리디렉션 1)", "");
            strPrintName = VB.Replace(strPrintName, "(리디렉션 2)", "");

            clsDP.PrintReciptPrint_New(ssSpread, ssSpread_Sheet, strPrintName);
        }
    }
}
