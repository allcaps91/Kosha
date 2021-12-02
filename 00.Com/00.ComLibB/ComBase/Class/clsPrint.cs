using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ComBase
{
    public class clsPrint : MTSDisposable
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        // 펜 : Pen penBlack1 = new Pen(Color.Black, (float)0.5); //펜색, 펜굵기
        // 선 : e.Graphics.DrawLine(penBlack1, 1, 2, 3, 4); //1:시작 x좌표, 2:시작 y좌표, 3:끝 x좌표, 4:끝 y좌표
        // 문자 : e.Graphics.DrawString("A",Font8, Brushes.Black, 1,2);// A:출력 문자열 , 1:시작 x좌표, 2:시작 y좌표
        // 박스 : e.Graphics.DrawRectangle(penBlack1, x 좌표, y 좌표 , 너비, 높이,);
        // 그림 : e.Graphics.DrawImage(이미지, x 좌표, y 좌표, 너비, 높이);

        //글상자를 이용한 문자 출력 :
        //Rectangle recString;
        //StringFormat strFormat = new StringFormat();
        //strig imStr = "출력할 문자";
        //recString = new Rectangle(x좌표, y좌표, 가로너비,세로높이);
        //strFormat.Alignment = StringAlignment.Center; //가로 정렬
        //strFormat.LineAlignment = StringAlignment.Center;//세로 정렬
        //e.Graphics.DrawString(imStr, Font11B, Brushes.Black, recString, strFormat);
        //                                출력문자를 폰트11 Bole로 검은색으로 출력하는 것이다. 

        //System.Windows.Forms.PrintDialog PD = new PrintDialog();
        //System.Drawing.Printing.PrinterSettings PS = new System.Drawing.Printing.PrinterSettings();

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);

        public const string pOutPrintName = "처방전"; //원외프린터명
        public const string pInPrintName = "처방전"; //원내프린터명
        public const string pAdmPrintName = "처방전"; //입원프린터명
        private static string pstrCAcpNo;
        private static string pstrOrdDate;
        private static int pintDrugCnt;
        private static int pintInjectCnt;
        private static int pintDrugSlipCnt;
        private static int pintInjectSlipCnt;
        private static int pintPageCnt = 1;
        //private static modType.gtDrugOutOrderSlipInfo[] ptDrugSlip;
        //private static modType.gtDrugOutOrderSlipInfo[] ptInjectSlip;
        private static short pintDataIndex = (short)0;
        private static string pstrPrtDate; //출력일자
        private static string pstrPrtTime; //출력일자

        //포스코통보서 인쇄
        public static string GstrPoscoPrtJDate;     //접수일자
        public static string GstrPoscoPrtName;      //성명
        public static string GstrPoscoPrtJumin;     //주민등록번호
        public static string GstrPoscoPrtPtno;      //등록번호
        public static string GstrPoscoPrtJikbun;    //직번
        public static string GstrPoscoPrtBuse;      //부서



        /// <summary>
        /// 기본프린터 이름을 가져온다
        /// </summary>
        /// <returns>기본 프린터 이름</returns>
        public static string gGetDefaultPrinter()
        {
            using (PrintDocument PD = new PrintDocument())
            {
                return PD.PrinterSettings.PrinterName;
            }
        }

        /// <summary>
        /// 기본프린터를 설정을 한다
        /// </summary>
        /// <param name="strPrintName">프린터 이름</param>
        /// <returns>true,false</returns>
        public static bool gSetDefaultPrinter(string strPrintName)
        {
            try
            {
                SetDefaultPrinter(strPrintName);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new Form() { TopMost = true }, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 설치된 프린터의 리스트를 출력한다.
        /// </summary>
        public static void gGetPrinterList()
        {
            int i = 0;
            for (i = 0; i <= System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count - 1; i++)
            {
                //if (System.Drawing.Printing.PrinterSettings.InstalledPrinters[i].ToString()  == strDriverName)
                //{
                //}
            }
        }

        /// <summary>
        /// 지정된 프린트가 존재 하는지 체크한다ㅏ.
        /// </summary>
        /// <param name="strPrintName"></param>
        /// <returns></returns>
        public static bool gGetPrinterFind(string strPrintName)
        {
            bool rtnVal = false;
            int i = 0;

            for (i = 0; i <= System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count - 1; i++)
            {
                if (System.Drawing.Printing.PrinterSettings.InstalledPrinters[i].ToString() == strPrintName)
                {
                    rtnVal = true;
                    break;
                }
            }
            return rtnVal;
        }

        /// <summary>
        /// 텍스트를 출력한다.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="strPrtText"></param>
        /// <param name="FontX"></param>
        /// <param name="BrushX"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public static void printString(PrintPageEventArgs e, string strPrtText, float X, float Y, Font FontX, Brush BrushX)
        {
            //이전버전 : 문자열, dCurrentX, dCurrentY, strFont, iFontSize, Alignment, bFontBold
            //e.Graphics.DrawString("2423423432234432", Font11, Brushes.Black, 20, 20);
            e.Graphics.DrawString(strPrtText, FontX, BrushX, X, Y);
        }

        /// <summary>피씨내 존재하는 프린터 리스트</summary>
        /// <author>김홍록</author>
        /// <date>2017.06.08</date>
        /// <returns></returns>
        public string[] getPrinter()
        {
            string[] s = new string[PrinterSettings.InstalledPrinters.Count];

            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                s[i] = PrinterSettings.InstalledPrinters[i];
            }

            return s;
        }

        /// <summary>진단검사 바코드</summary>
        /// <author>김홍록</author>
        /// <date>2017.06.08</date>
        /// <param name="strName"></param>
        /// <returns></returns>
        public bool isLabBarCodePrinter(string strName)
        {
            bool b = false;
            string[] print = getPrinter();

            if (print.Length == 0)
            {
                ComFunc.MsgBox("컴퓨터에 프린트 드라이버가 없습니다.", "프린터 설정오류");
            }

            for (int i = 0; i < print.Length; i++)
            {
                if (print[i].ToString().ToUpper().IndexOf(strName) >= 0)
                {
                    return true;
                }
            }

            ComFunc.MsgBox("컴퓨터에 혈액 바코드 프린터가 설정되어 있지 않습니다.", "프린터 설정오류");
            return b;
        }

        /// <summary>진단검사 바코드</summary>
        /// <author>김홍록</author>
        /// <date>2017.06.08</date>
        /// <param name="strName"></param>
        /// <returns></returns>
        public string getLabBarCodePrinter(string strName)
        {
            string s = "";
            string[] print = getPrinter();

            if (print.Length == 0)
            {
                ComFunc.MsgBox("컴퓨터에 프린트 드라이버가 없습니다.", "프린터 설정오류");
            }

            for (int i = 0; i < print.Length; i++)
            {
                if (print[i].ToString().ToUpper().IndexOf(strName) >= 0)
                {
                    return print[i];
                }
            }

            ComFunc.MsgBox("컴퓨터에 혈액 바코드 프린터가 설정되어 있지 않습니다.", "프린터 설정오류");
            return "";
        }

        /// <summary>
        /// 프린트 이름으로 설치유무체크
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public string getPrinter_Chk(string strName)
        {
            string[] print = getPrinter();

            try
            {
                if (print.Length == 0)
                {
                    ComFunc.MsgBox("컴퓨터에 프린트 드라이버가 없습니다.", "프린터 설정오류");
                }

                for (int i = 0; i < print.Length; i++)
                {
                    if (print[i].ToString().ToUpper().IndexOf(strName) >= 0)
                    {
                        return print[i];
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox("컴퓨터에 [" + strName + "] 프린터가 설정되어 있지 않습니다.", "프린터 설정오류"
                + ComNum.VBLF + ComNum.VBLF + ex.Message);
            }

            return "";
        }


        /// <summary>원무 바코드</summary>
        /// <author>박병규</author>
        /// <date>2018.04.18</date>
        /// <param name="strName"></param>
        /// <returns></returns>
        public string getPmpaBarCodePrinter(string strName)
        {
            string s = "";
            string[] print = getPrinter();

            if (print.Length == 0)
            {
                ComFunc.MsgBox("컴퓨터에 프린트 드라이버가 없습니다.", "프린터 설정오류");
            }

            for (int i = 0; i < print.Length; i++)
            {
                if (print[i].ToString().ToUpper().IndexOf(strName) >= 0)
                {
                    return print[i];
                }
            }

            //ComFunc.MsgBox("컴퓨터에 프린터가 설정되어 있지 않습니다.", "프린터 설정오류");
            return "";
        }

        /// <summary>원무 바코드</summary>
        /// <author>박병규</author>
        /// <date>2018.04.18</date>
        /// <param name="strName"></param>
        /// <returns></returns>
        public bool isPmpaBarCodePrinter(string strName)
        {
            bool b = false;
            string[] print = getPrinter();

            if (print.Length == 0)
            {
                ComFunc.MsgBox("컴퓨터에 프린트 드라이버가 없습니다.", "프린터 설정오류");
            }

            for (int i = 0; i < print.Length; i++)
            {
                if (print[i].ToString().ToUpper().IndexOf(strName) >= 0)
                {
                    return true;
                }
            }

            ComFunc.MsgBox("컴퓨터에 프린터가 설정되어 있지 않습니다.", "프린터 설정오류");
            return b;
        }

        /// <summary>프린터 연결 상태</summary>
        /// <author>김홍록</author>
        /// <date>2018.04.23</date>

        /// <param name="pStrPrinterNm">프린트이름</param>
        /// <returns></returns>
        public bool isPrinterOffLine(string pStrPrinterNm)
        {
            bool b = true;

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

                string printerName = "";
                foreach (ManagementObject printer in searcher.Get())
                {
                    printerName = printer["Name"].ToString().ToLower();
                    if (printerName.ToString().ToUpper().IndexOf(pStrPrinterNm) >= 0)
                    {
                        if (printer["WorkOffline"].ToString().ToLower().Equals("true"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                        break;
                    }
                }
            }
            catch
            {

                return false;
            }

            return false;
        }

    }
}
