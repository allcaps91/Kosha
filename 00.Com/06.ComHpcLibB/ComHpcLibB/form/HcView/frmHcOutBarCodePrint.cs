using ComBase;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcOutBarCodePrint.cs
/// Description     : 출장 바코드 인쇄
/// Author          : 이상훈
/// Create Date     : 2020-06-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm출장바코드인쇄.frm(Frm출장바코드인쇄)" />

namespace ComHpcLibB
{
    public partial class frmHcOutBarCodePrint : Form
    {
        ComHpcLibBService comHpcLibBService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsQuery cq = new clsQuery();

        int Siz;
        int Opt;
        string Font_Name;
        long chkcolor;
        int mo_flag;
        string strPrGinterName;
        string FstrWRTNO;

        private Font verdana10Font;
        private StreamReader reader;

        public class DOCINFO
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
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFO di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter([In]System.IntPtr hPrinter, [In, Out]string pBuf, [In]int cbBug, ref int pcWritten);
        
        public frmHcOutBarCodePrint()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            comHpcLibBService = new ComHpcLibBService();

            this.Load += new EventHandler(eFormLoad);
            //혈액종양검사
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnMenuStart.Click += new EventHandler(eBtnClick);
            this.btnMenuStop.Click += new EventHandler(eBtnClick);
            this.timer1.Tick += new EventHandler(eTimerTick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            btnMenuStart.Enabled = true;
            btnMenuStop.Enabled = false;

            clsQuery.READ_PC_CONFIG(clsDB.DbCon);

            sp.Spread_All_Clear(SS1);
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnMenuStart)
            {
                btnMenuStart.Enabled = false;
                btnMenuStop.Enabled = true;
                timer1.Enabled = true;
                Application.DoEvents();
            }
            else if (sender == btnMenuStop)
            {
                btnMenuStart.Enabled = true;
                btnMenuStop.Enabled = false;
                timer1.Enabled = false;
                Application.DoEvents();
            }
        }

        void fn_RawPrint(string argData)
        {
            string strPrtName = "혈액환자정보";
            string strPrintName = "";
            Int32 dwError = 0;
            IntPtr lhPrinter = new IntPtr(0);
            bool lReturn = false;
            DOCINFO MyDocInfo = new DOCINFO();

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

        void eTimerTick(object sender, EventArgs e)
        {
            int nRead = 0;
            string strPrtData = "";
            string strRowId = "";
            int result = 0;

            timer1.Enabled = false;

            ComFunc.ReadSysDate(clsDB.DbCon);

            List<COMHPC> list = comHpcLibBService.GetPrtDatabyHicSpecMstWork();

            nRead = list.Count;
            SS1.ActiveSheet.RowCount = nRead;
            if (nRead > 0)
            {
                clsDB.setBeginTran(clsDB.DbCon);
                for (int i = 0; i < nRead; i++)
                {
                    strRowId = list[i].ROWID;
                    strPrtData = list[i].PRTDATA;

                    fn_RawPrint(strPrtData);
                    Thread.Sleep(200);

                    result = comHpcLibBService.DeleteHicSpecMstWorkbyRowId(strRowId);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("SPECMST_WORK 삭제중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    SS1.ActiveSheet.Cells[0, 0].Text = clsPublic.GstrSysDate;
                    SS1.ActiveSheet.Cells[0, 0].Text = strPrtData;
                }
                clsDB.setCommitTran(clsDB.DbCon);
            }

            timer1.Enabled = true;
        }
    }
}
