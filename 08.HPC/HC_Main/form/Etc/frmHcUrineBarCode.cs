using ComBase;
using ComHpcLibB;
using ComHpcLibB.Service;
using ComHpcLibB.Dto;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using ComBase.Controls;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcUrineBarCode.cs
/// Description     : 일반건진 소변컴사 바코드
/// Author          : 이상훈
/// Create Date     : 2020-06-22
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm건진소변검사.frm(Frm건진소변검사)" />

namespace HC_Main
{
    public partial class frmHcUrineBarCode : Form
    {
        HicJepsuService hicJepsuService = null;
        HicResultExCodeService hicResultExCodeService = null;
        ComHpcLibBService comHpcLibBService = null;

        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();

        int Siz;
        int Opt;
        string Font_Name;
        long chkcolor;
        int mo_flag;

        string strPrinterName;
        string FstrWrtNo;

        //string GstrLabelPrint1 = "";
        //string GstrLabelPrint2 = "";
        //string GstrLabelPrint3 = "";
        //string GstrLabelPrint4 = "";
        //string GstrLabelPrint5 = "";

        string FstringToPrint;

        private Font verdana10Font;  
        private StreamReader streamToPrint;
        private Font printFont;

        string ls_PrintSpeed = "";  //인쇄속도

        [DllImport("kernel32.dll", EntryPoint = "Sleep")]
        private static extern long Sleep(long dwMilliseconds);

        // 스트럭쳐 api 호출.
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }

        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter([In]System.IntPtr hPrinter, [In, Out]string pBuf, [In]int cbBug, ref int pcWritten);

        public frmHcUrineBarCode()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcUrineBarCode(string strWrtNo)
        {
            InitializeComponent();
            FstrWrtNo = strWrtNo;

            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicResultExCodeService = new HicResultExCodeService();
            comHpcLibBService = new ComHpcLibBService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnUSBT.Click += new EventHandler(eBtnClick);
            this.timer1.Tick += new EventHandler(etimerTick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            clsQuery.READ_PC_CONFIG(clsDB.DbCon);

            //Me.ScaleMode = 3     ' 픽셀로 해야 바코드에 한글이 인쇄된다.

            txtSpecNo.Text = FstrWrtNo;
            timer1.Enabled = true;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            string sMsg = "";

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnUSBT)
            {
                string rtnVal = "";
                string File_Name = "";
                int FontSize = 0;
                //string strPrinterName = "";
                string strSName = "";
                string strAge = "";
                string strSex = "";
                int nPrint = 0;
                int nREAD = 0;
                string strExCode = "";
                string strBarCodeAdd = "";
                string strBDATE = "";
                string strGbn1 = "";
                string strGbn2 = "";
                string strGbn3 = "";
                string strGbn4 = "";
                string strGbn5 = "";
                string Prdata = "";
                string strCodes = "";

                string strPrintName1 = "";
                string strPrintName2 = "";

                int result = 0;

                clsPrint CP = new clsPrint();

                if (txtSpecNo.Text == "")
                {
                    return;
                }

                if (clsHcVariable.GbBarcodeDbSend == false)
                {
                    strPrinterName = "혈액환자정보";
                    strPrintName1 = clsPrint.gGetDefaultPrinter();
                    strPrintName2 = CP.getPrinter_Chk(strPrinterName.ToUpper());

                    if (strPrintName2.IsNullOrEmpty())
                    {
                        ComFunc.MsgBox("프린터 설정 오류입니다. 전산정보팀(☏29047)에 연락바랍니다.");
                        return;
                    }

                    strPrinterName = strPrintName2;
                }

                HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(long.Parse(FstrWrtNo));

                if (list.IsNullOrEmpty())
                {
                    return;
                }

                strSName = list.SNAME;
                strAge = list.AGE.ToString();
                strSex = list.SEX;
                strBDATE = list.JEPDATE;

                //뇨단백 소변컵 바코드 인쇄
                List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetUrineItembyWrtNo(long.Parse(FstrWrtNo));

                nREAD = list2.Count;
                strGbn1 = "";
                strGbn2 = "";
                strGbn3 = "";
                strGbn4 = "";
                strGbn5 = "";

                for (int i = 0; i < nREAD; i++)
                {
                    strExCode = list2[i].EXCODE;
                    switch (strExCode)
                    {
                        case "A112":
                            strGbn1 = "Y";                  //단백뇨
                            break;
                        case "A111":
                        case "A113":
                        case "A114":
                            strGbn2 = "Y";                  //소변검사4종
                            break;
                        case "MU11":
                        case "MU12":
                        case "MU13":
                        case "MU15":
                        case "MU27":
                        case "MU28":
                        case "MU74":
                        case "MU79":
                            strGbn4 += strExCode + ",";     //작업종료소변 별도 라벨
                            break;
                        case "LM11":
                            strGbn5 += strExCode + ",";     //아침첫소변 별도 라벨
                            break;
                        default:
                            strGbn3 += strExCode + ",";     //검사실 또는 분석실 검사
                            break;
                    }
                }

                //작업종료소변이 있으면 별도 라벨 인쇄
                if (!strGbn4.IsNullOrEmpty())
                {
                    Prdata = "";
                    ls_PrintSpeed = "5";         //인쇄속도

                    if (clsHcVariable.GbHicChul == true || clsHcVariable.GstrChul == "Y")
                    {
                        Prdata = "^XA^FWN^PR5^LH100,0^FS";
                    }
                    else
                    {
                        if (clsType.PC_CONFIG.GX420D == "1" && string.Compare(clsType.PC_CONFIG.GX420D, "0") > 0)
                        {
                            Prdata = "^XA^FWN^PR5^LH" + clsType.PC_CONFIG.GX420D_X.ToString() + ",0^FS";
                        }
                        else
                        {
                            Prdata = "^XA^FWN^PR5^LH0,0^FS"; //인쇄속도
                        }
                    }
                    Prdata += "^SEE:UHANGUL.DAT^FS";
                    Prdata += "^CW1,E:KFONT3.FNT^FS";
                    Prdata += "^FO20,35^CI26^A1N,30,30^FD" + strSName + "^FS";
                    if (hf.GetLength(strSName) == 6)
                    {
                        Prdata += "^FO130,40^A0N,30,25^FD" + FstrWrtNo + "  " + strAge + "/" + strSex + " HR" + "^FS";
                    }
                    else if (hf.GetLength(strSName) == 8)
                    {
                        Prdata += "^FO160,40^A0N,30,25^FD" + FstrWrtNo + "  " + strAge + "/" + strSex + " HR" + "^FS";
                    }
                    else if (hf.GetLength(strSName) == 10)
                    {
                        Prdata += "^FO190,40^A0N,30,25^FD" + FstrWrtNo + "  " + strAge + "/" + strSex + " HR" + "^FS";
                    }
                    else
                    {
                        Prdata += "^FO220,40^A0N,30,25^FD" + FstrWrtNo + "  " + strAge + "/" + strSex + " HR" + "^FS";
                    }

                    //------------<바코드시작>  tla 바코드
                    Prdata += "^FO57,70^BY2,2:1";                                           //바코드 인쇄 (10자리)
                    Prdata += "^B3N,N,80,N,N";   //Barcode Type: Code 39 (SubSets A,B and C)
                    Prdata += "^FD" + FstrWrtNo.Trim() + "^FS";
                    //------------<바코드끝>

                    strCodes = "(" + VB.Left(strGbn4, hf.GetLength(strGbn4) - 1) + ")";
                    Prdata += "^FO57,160^A0N,25,25^FD" + strCodes + "^FS";

                    Prdata += "^XZ";

                    //출장검진은 직접 인쇄하지 않고 WORK DB를 이용하여 바코드를 인쇄함
                    if (clsHcVariable.GbBarcodeDbSend == true || clsHcVariable.GbHicChul == true || clsHcVariable.GstrChul == "Y")
                    {
                        result = comHpcLibBService.InsertHicSpecmstWork(Prdata);
                    }
                    else
                    {
                        fn_RawPrint_cls(Prdata);
                        Thread.Sleep(100);
                    }
                }

                //아침첫소변 있으면 별도 라벨 인쇄
                if (!strGbn5.IsNullOrEmpty())
                {
                    Prdata = "";
                    ls_PrintSpeed = "5";         //인쇄속도
                    if (clsHcVariable.GbHicChul == true || clsHcVariable.GstrChul == "Y")
                    {
                        Prdata = "^XA^FWN^PR5^LH100,0^FS";
                    }
                    else
                    {
                        if (clsType.PC_CONFIG.GX420D == "1" && string.Compare(clsType.PC_CONFIG.GX420D, "0") > 0)
                        {
                            Prdata = "^XA^FWN^PR5^LH" + clsType.PC_CONFIG.GX420D_X.ToString() + ",0^FS";
                        }
                        else
                        {
                            Prdata = "^XA^FWN^PR5^LH0,0^FS"; //인쇄속도
                        }
                    }

                    Prdata += "^SEE:UHANGUL.DAT^FS";
                    Prdata += "^CW1,E:KFONT3.FNT^FS";
                    Prdata += "^FO20,35^CI26^A1N,30,30^FD" + strSName + "^FS";
                    if (hf.GetLength(strSName) == 6)
                    {
                        Prdata += "^FO130,40^A0N,30,25^FD" + FstrWrtNo + "  " + strAge + "/" + strSex + " HR" + "^FS";
                    }
                    else if (hf.GetLength(strSName) == 8)
                    {
                        Prdata += "^FO160,40^A0N,30,25^FD" + FstrWrtNo + "  " + strAge + "/" + strSex + " HR" + "^FS";
                    }
                    else if (hf.GetLength(strSName) == 10)
                    {
                        Prdata += "^FO190,40^A0N,30,25^FD" + FstrWrtNo + "  " + strAge + "/" + strSex + " HR" + "^FS";
                    }
                    else
                    {
                        Prdata += "^FO220,40^A0N,30,25^FD" + FstrWrtNo + "  " + strAge + "/" + strSex + " HR" + "^FS";
                    }

                    //------------<바코드시작>  tla 바코드
                    Prdata += "^FO57,70^BY2,2:1";                                           //바코드 인쇄 (10자리)
                    Prdata += "^B3N,N,80,N,N";   //Barcode Type: Code 39 (SubSets A,B and C)
                    Prdata += "^FD" + FstrWrtNo.Trim() + "^FS";
                    //------------<바코드끝>

                    strCodes = "(" + VB.Left(strGbn5, hf.GetLength(strGbn5) - 1) + ")";
                    Prdata += "^FO57,160^A0N,25,25^FD" + strCodes + "^FS";

                    Prdata += "^XZ";

                    //출장검진은 직접 인쇄하지 않고 WORK DB를 이용하여 바코드를 인쇄함
                    if (clsHcVariable.GbBarcodeDbSend == true || clsHcVariable.GbHicChul == true || clsHcVariable.GstrChul == "Y")
                    {
                        result = comHpcLibBService.InsertHicSpecmstWork(Prdata);
                    }
                    else
                    {
                        fn_RawPrint_cls(Prdata);
                        Thread.Sleep(100);
                    }
                }

                //당일 소변컵 인쇄
                if (!strGbn1.IsNullOrEmpty() || !strGbn2.IsNullOrEmpty() || !strGbn3.IsNullOrEmpty())
                {
                    Prdata = "";
                    ls_PrintSpeed = "5";         //인쇄속도
                    if (clsHcVariable.GbHicChul == true || clsHcVariable.GstrChul == "Y")
                    {
                        Prdata = "^XA^FWN^PR5^LH100,0^FS";
                    }
                    else
                    {
                        if (clsType.PC_CONFIG.GX420D == "1" && string.Compare(clsType.PC_CONFIG.GX420D, "0") > 0)
                        {
                            Prdata = "^XA^FWN^PR5^LH" + clsType.PC_CONFIG.GX420D_X.ToString() + ",0^FS";
                        }
                        else
                        {
                            Prdata = "^XA^FWN^PR5^LH0,0^FS"; //인쇄속도
                        }
                    }

                    Prdata += "^SEE:UHANGUL.DAT^FS";
                    Prdata += "^CW1,E:KFONT3.FNT^FS";
                    Prdata += "^FO20,35^CI26^A1N,30,30^FD" + strSName + "^FS";
                    if (hf.GetLength(strSName) == 6)
                    {
                        Prdata += "^FO130,40^A0N,30,25^FD" + FstrWrtNo + "  " + strAge + "/" + strSex + " HR" + "^FS";
                    }
                    else if (hf.GetLength(strSName) == 8)
                    {
                        Prdata += "^FO160,40^A0N,30,25^FD" + FstrWrtNo + "  " + strAge + "/" + strSex + " HR" + "^FS";
                    }
                    else if (hf.GetLength(strSName) == 10)
                    {
                        Prdata += "^FO190,40^A0N,30,25^FD" + FstrWrtNo + "  " + strAge + "/" + strSex + " HR" + "^FS";
                    }
                    else
                    {
                        Prdata += "^FO220,40^A0N,30,25^FD" + FstrWrtNo + "  " + strAge + "/" + strSex + " HR" + "^FS";
                    }

                    //------------<바코드시작>  tla 바코드
                    Prdata += "^FO57,70^BY2,2:1";                                           //바코드 인쇄 (10자리)
                    Prdata += "^B3N,N,80,N,N";   //Barcode Type: Code 39 (SubSets A,B and C)
                    Prdata += "^FD" + FstrWrtNo.Trim() + "^FS";
                    //------------<바코드끝>

                    if (!strGbn3.IsNullOrEmpty())
                    {
                        strGbn3 = VB.Left(strGbn3, strGbn3.Length - 1);
                    }

                    strCodes = "";
                    if (!strGbn2.IsNullOrEmpty())
                    {
                        strCodes = "UA4(**)";
                        if (!strGbn3.IsNullOrEmpty())
                        {
                            strCodes += ",($$)";
                        }
                    }
                    else if (!strGbn1.IsNullOrEmpty())
                    {
                        strCodes = "UA1(*)";
                        if (!strGbn3.IsNullOrEmpty())
                        {
                            strCodes += ",($$)";
                        }
                    }
                    else
                    {
                        strCodes = "($$)";
                    }

                    Prdata += "^FO57,160^A0N,25,25^FD" + strCodes + "^FS";

                    Prdata += "^XZ";

                    //출장검진은 직접 인쇄하지 않고 WORK DB를 이용하여 바코드를 인쇄함
                    if (clsHcVariable.GbBarcodeDbSend == true || clsHcVariable.GbHicChul == true || clsHcVariable.GstrChul == "Y")
                    {
                        result = comHpcLibBService.InsertHicSpecmstWork(Prdata);
                    }
                    else
                    {
                        fn_RawPrint_cls(Prdata);
                        Thread.Sleep(100);
                    }
                }
            }
        }

        /// <summary>
        /// TODO : 이상훈 (2020.08.03) - 출장검진 사용시 파일처리 방식 해결 해야 함. 
        /// ComPrintApi.SendFileToPrinter(strPrintName2, filename)  참조
        /// </summary>
        /// <param name="argData"></param>
        void fn_RawPrint_cls(string argData)
        {
            string strPrtName = "혈액환자정보";
            string strPrintName = "";
            Int32 dwError = 0;
            IntPtr lhPrinter = new IntPtr(0);
            bool lReturn = false;
            DOCINFOA MyDocInfo = new DOCINFOA();

            // 기록된 바이트 수 
            int dwWritten = 0;
            int dwBytesOfText = System.Text.Encoding.GetEncoding(949).GetByteCount(argData);

            clsPrint CP = new clsPrint();
            strPrintName = CP.getPrinter_Chk(strPrtName.ToUpper());

            // 프린터 오픈.
            if (OpenPrinter(strPrintName.Normalize(), out lhPrinter, IntPtr.Zero))
            {
                MyDocInfo.pDocName = "EXAM";
                MyDocInfo.pOutputFile = null;
                MyDocInfo.pDataType = "RAW";

                if (StartDocPrinter(lhPrinter, 1, MyDocInfo))
                {
                    // 시작 페이지
                    if (StartPagePrinter(lhPrinter))
                    {
                        // 바이트 쓰기
                        lReturn = WritePrinter(lhPrinter, argData, dwBytesOfText, ref dwWritten);
                        EndPagePrinter(lhPrinter);
                    }
                    EndDocPrinter(lhPrinter);
                }
                ClosePrinter(lhPrinter);
            }
            else
            {
                MessageBox.Show("프린터 설정 오류입니다. 전산정보팀(☏29047)에 연락바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lReturn == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
        }

        //void fn_RawPrint_cls(string argData)
        //{
        //    string strPrtName = "혈액환자정보";
        //    string strPrintName1 = "";
        //    string strPrintName2 = "";

        //    clsPrint CP = new clsPrint();

        //    strPrintName1 = clsPrint.gGetDefaultPrinter();
        //    strPrintName2 = CP.getPrinter_Chk(strPrtName.ToUpper());

        //    if (strPrintName2.IsNullOrEmpty())
        //    {
        //        ComFunc.MsgBox("프린터 설정 오류입니다. 전산정보팀(☏29047)에 연락바랍니다.");
        //        return;
        //    }

        //    string SavePath = @"C:\CMC\barcode.txt";
        //    string txtValue = argData;

        //    File.WriteAllText(SavePath, argData, Encoding.Default);

        //    string filename = @"C:\CMC\barcode.txt";

        //    if (ComPrintApi.SendFileToPrinter(strPrintName2, filename) == true)
        //    {

        //    }

        //    //PrintDocument Pd = new PrintDocument();
        //    //PrintController pc = new StandardPrintController();
        //    //Pd.PrintController = pc;
        //    //Pd.DocumentName = filename;
        //    //using (FileStream stream = new FileStream(filename, FileMode.Open))
        //    //using (StreamReader reader = new StreamReader(stream, Encoding.Default))
        //    //{
        //    //    FstringToPrint = reader.ReadToEnd();
        //    //}
        //    //Pd.PrinterSettings.PrinterName = strPrintName2;
        //    //Pd.PrintPage += new PrintPageEventHandler(fn_PrintTextFileHandler);
        //    //Pd.Print();
        //    //if (reader != null)
        //    //{
        //    //    reader.Close();
        //    //}
        //}

        //void fn_PrintTextFileHandler(object sender, PrintPageEventArgs e)
        //{
        //    int charactersOnPage = 0;
        //    int linesPerPage = 0;

        //    using (Font font = new Font("나눔고딕", 10))
        //    {
        //        using (StringFormat string_format = new StringFormat())
        //        {
        //            SizeF layout_area = new SizeF(e.MarginBounds.Width, e.MarginBounds.Height);

        //            e.Graphics.MeasureString(FstringToPrint, this.Font, e.MarginBounds.Size,
        //                                 StringFormat.GenericTypographic,
        //                                 out charactersOnPage, out linesPerPage);

        //            e.Graphics.DrawString(FstringToPrint.Substring(0, charactersOnPage), this.Font, Brushes.Black,
        //                                  e.MarginBounds, StringFormat.GenericTypographic);

        //            FstringToPrint = FstringToPrint.Substring(0, charactersOnPage);
        //        }
        //    }
        //    //e.HasMorePages = FstringToPrint.Length > 0;
        //    e.HasMorePages = false;

        //}

        void etimerTick(object sender, EventArgs e)
        {
            eBtnClick(btnUSBT, new EventArgs());
            this.Close();
        }
    }
}
