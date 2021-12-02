using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcExamBarCode.cs
/// Description     : 검체번호 Barcode 인쇄
/// Author          : 이상훈
/// Create Date     : 2020-07-01
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmExmBarCode.frm(ExamBarCode)" />

namespace ComHpcLibB
{
    public partial class frmHcExamBarCode : Form
    {
        ExamResultcService examResultcService = null;
        ExamSpecmstService examSpecmstService = null;
        HicJepsuService hicJepsuService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicJepsuLtdService hicJepsuLtdService = null;
        ExamSpecodeService examSpecodeService = null;

        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        clsHcExam he = new clsHcExam();

        [DllImport("kernel32.dll", EntryPoint = "Sleep")]
        private static extern long Sleep(long dwMilliseconds);

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

        int Siz;
        int Opt;
        string Font_Name;
        long chkcolor;
        int mo_flag;

        string FstrWRTNO;
        string FsOldBarName = "";
        int FnBarCnt;

        string rtnVal = "";
        string strPrtName = "";
        string strPrintName = "";
        object ob;
        int nPrint = 0;
        string PatName = "";    //환자성명
        string strDrno = "";    //등록번호
        string strAge = "";
        string strSex = "";
        string strDep = "";
        string strDoctNo = "";
        string strWS = "";
        string strSTRT = "";
        string strSpno = "";
        string strTime = "";
        string strSpecimen = "";
        string strSpecCode = "";
        string strTube = "";
        string strVolume = "";
        string strRoom = "";
        string strIO = "";
        string strBI = "";

        string strAbo = "";

        string File_Name = "";
        int FontSize = 0;
        string angle = "";
        string Prdata = "";
        string sBCodeName = "";
        string sBCodeName1 = "";
        int iBCodeCount = 0;

        int iItem = 0;
        double iVolumeMax = 0;
        string sVW = "";
        string sVolumeCode = "";

        string strWon = ""; //원거리 표시
        string strInfect = ""; //혈액감염여부

        string strEDTA = ""; //EDTA
        string strSodium = "";
        string strHeparin = "";
        string strEDTA_R = "";

        string strCodes = "";
        string strCodes1 = "";
        string strCodes2 = "";
        string strCodes3 = "";

        string strLTDNAME = "";
        string strLTDOK = "";

        string strBarCodeAdd = "";
        string strHicSTRT = "";     //일반건진 응급여부
        string ls_PrintSpeed = "";  //인쇄속도
        string strJung = "";
        string strBDate = "";

        string strExName = "";

        private Font verdana10Font;
        private StreamReader reader;

        string FstringToPrint;

        int nRead = 0;

        public frmHcExamBarCode()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcExamBarCode(string sOldBarName)
        {
            InitializeComponent();
            FsOldBarName = sOldBarName;
            SetEvent();
        }

        void SetEvent()
        {
            examResultcService = new ExamResultcService();
            examSpecmstService = new ExamSpecmstService();
            hicJepsuService = new HicJepsuService();
            comHpcLibBService = new ComHpcLibBService();
            hicJepsuLtdService = new HicJepsuLtdService();
            examSpecodeService = new ExamSpecodeService();

            this.Load += new EventHandler(eFormLoad);
            //혈액종양검사
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnBarPrint.Click += new EventHandler(eBtnClick);
            this.btnUSB.Click += new EventHandler(eBtnClick);
            this.btnUSBT.Click += new EventHandler(eBtnClick);
            this.btnBarMini.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            double t = 0;
            string sSpecNo = "";
            int nCnt = 0;
            string strMasterCode = "";
            string strTubeMsg = "";

            this.Location = new Point(10, 10);

            //Me.ScaleMode = 3     ' 필셀로 해야 바코드에 한글이 인쇄된다.

            ComFunc.ReadSysDate(clsDB.DbCon);

            //gsOldBarName = "";  GnWBVolume = 0;
            //GstrTubeMsg = "";
            for (int i = 0; i < VB.L(FsOldBarName, ","); i++)
            {
                strMasterCode = "";
                sSpecNo = VB.Pstr(FsOldBarName, ",", i);
                txtSpecNo.Text = sSpecNo;
                if (!sSpecNo.IsNullOrEmpty())
                {
                    if (!FsOldBarName.IsNullOrEmpty())
                    {
                        strTubeMsg += VB.Space(2);

                        switch (i)
                        {
                            case 0:
                                strTubeMsg += "①";
                                break;
                            case 1:
                                strTubeMsg += "②";
                                break;
                            case 2:
                                strTubeMsg += "③";
                                break;
                            case 3:
                                strTubeMsg += "④";
                                break;
                            case 4:
                                strTubeMsg += "\r\n" + "⑤";
                                break;
                            case 5:
                                strTubeMsg += "⑥";
                                break;
                            case 6:
                                strTubeMsg += "⑦";
                                break;
                            case 7:
                                strTubeMsg += "⑧";
                                break;
                            case 8:
                                strTubeMsg += "\r\n" + "⑨";
                                break;
                            case 9:
                                strTubeMsg += "⑩";
                                break;
                            case 10:
                                strTubeMsg += "⑪";
                                break;
                            case 11:
                                strTubeMsg += "⑫";
                                break;
                            case 12:
                                strTubeMsg += "⑬";
                                break;
                            default:
                                strTubeMsg += string.Format("{0:00}", i) + "]";
                                break;
                        }

                        eBtnClick(btnUSBT, new EventArgs());

                        //혈액학PB는 바코드2번 출력함.
                        List<EXAM_RESULTC> list = examResultcService.GetMasterCodebySpecNoMasterCode(sSpecNo, "HR10");

                        nCnt = list.Count;
                        if (nCnt > 0)
                        {
                            eBtnClick(btnUSBT, new EventArgs());
                        }
                    }
                }
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnBarPrint)
            {

            }
            else if (sender == btnUSB)
            {

            }
            else if (sender == btnUSBT)
            {
                //awprint 는 Z:\EDPSDATA\자료실\자료(바코드)\zabra프린터 드라이버\rawprn.exe
                if (txtSpecNo.Text.IsNullOrEmpty())
                {
                    return;
                }

                clsPrint CP = new clsPrint();

                strPrtName = "혈액환자정보";
                strPrintName = CP.getPrinter_Chk(strPrtName.ToUpper());

                if (strPrintName.IsNullOrEmpty())
                {
                    ComFunc.MsgBox("프린터 설정 오류입니다. 전산정보팀(☏29047)에 연락바랍니다.");
                    return;
                }

                List<EXAM_SPECMST> list = examSpecmstService.GetItembySpecNo(txtSpecNo.Text);

                strIO = "";
                strHicSTRT = "";

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        PatName = list[i].SNAME;
                        strDrno = list[i].PANO;

                        if (list[i].HICNO > 0)
                        {
                            FstrWRTNO = string.Format("{0:#0}", list[i].HICNO);
                        }
                        else
                        {
                            FstrWRTNO = list[i].PANO;
                        }
                        if (strDrno == "81000013")  //정도관리
                        {
                            strAge = "";
                            strSex = "";
                            strDep = "";
                            strDoctNo = "";
                            strBI = "";
                        }
                        else
                        {
                            strAge = list[i].AGE.ToString();
                            strSex = list[i].SEX;
                            strDep = list[i].DEPTCODE;
                            strDoctNo = list[i].DRCODE;
                            strRoom = list[i].ROOM.ToString();
                            strIO = list[i].IPDOPD;
                            strBI = list[i].BI;
                        }

                        strWS = list[i].WORKSTS.Replace(",", "");
                        strSTRT = list[i].STRT;
                        strSpno = list[i].SPECNO;
                        strTime = list[i].BDATE;
                        strSpecimen = he.GetBasCode("검체약어", list[i].SPECCODE);
                        strTube = he.GetBasCode("용기약어", list[i].TUBE);
                        strSpecCode = list[i].SPECCODE;
                        strBDate = list[i].BBDATE;

                        if (VB.Left(clsVbfunc.GetIpAddressOeacle(clsDB.DbCon), 10) == "192.168.41" ||
                            VB.Left(clsVbfunc.GetIpAddressOeacle(clsDB.DbCon), 9) == "192.168.2")
                        {
                            if (list[i].HICNO > 0)
                            {
                                FstrWRTNO = string.Format("{0:#0}", list[i].HICNO);
                            }
                            else
                            {
                                FstrWRTNO = list[i].PANO;
                            }
                        }
                        else
                        {
                            FstrWRTNO = list[i].PANO;
                        }

                        //건진 채용신검은 객담검사(긴급) 별표 표시
                        strHicSTRT = "";
                        if (list[i].HICNO > 0)
                        {
                            //건진 객담검사
                            if (strWS == "A" && strTube == "Ste_C" && strSpecimen == "SP")
                            {
                                switch (hicJepsuService.GetGjJongbyWrtNo(long.Parse(FstrWRTNO)))
                                {
                                    case "21":
                                    case "22":
                                    case "24":
                                    case "29":
                                    case "33":
                                    case "69":
                                        strHicSTRT = "E";
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        if (hicJepsuService.GetGjJOngbyPtnoJepDate(strDrno, strBDate) == 0)
                        {
                            strHicSTRT = "";
                        }

                        //입원환자는 진료과 대신에 병동을 바코드에 인쇄함
                        if (list[i].IPDOPD == "I")
                        {
                            strDep = list[i].WARD;
                            //중환자실 SICU,MICU분리
                            if (strDep == "IU")
                            {
                                if (list[i].ROOM == 233)
                                {
                                    strDep = "SICU";
                                }
                                else if (list[i].ROOM == 234)
                                {
                                    strDep = "MICU";
                                }
                            }

                            //icu 격리실 정보 추가 kyo
                            COMHPC list2 = comHpcLibBService.GetIpdMasterBCodebyPaNo(strDrno);

                            if (!list2.IsNullOrEmpty() && !list2.NAME.IsNullOrEmpty())
                            {
                                strRoom = strRoom + "/" + list2.NAME;
                            }
                        }
                    }
                }

                //건강증진센터 회사명 표시
                if (strDep == "HR")
                {
                    List<HIC_JEPSU_LTD> list3 = hicJepsuLtdService.GetNamebyWrtNo(FstrWRTNO);

                    if (list3.Count == 1)
                    {
                        strLTDNAME = list3[0].NAME;
                    }
                }

                //혈액형 조회
                strAbo = "";
                strAbo = comHpcLibBService.GetABObyPaNo(strDrno);

                //EDTA 읽기
                strEDTA = "";
                if (VB.InStr(strWS, "H") > 0)   //혈액학만
                {
                    COMHPC list4 = comHpcLibBService.GetExamPatientbyPaNo(strDrno);

                    if (!list4.IsNullOrEmpty())
                    {
                        strEDTA = "*";
                        if (list4.SODIUM == "*")
                        {
                            strSodium = "*";
                        }
                        if (list4.HEPARIN == "*")
                        {
                            strHeparin = "*";
                        }
                    }
                }

                //원거리
                strWon = "";

                if (strDrno == "81000004")
                {
                    strWon = "#";
                }

                if (strIO == "O" && strDoctNo != "1107" && strDoctNo != "0901" && strDoctNo != "0902")  //오동호과장님 # 표시 않되도록함(임상병리요청)
                {
                    if (strBI != "61" && strBI != "62")
                    {
                        if (!comHpcLibBService.GetPatientAreabyPaNo(strDrno).IsNullOrEmpty())
                        {
                            strWon = "#";
                        }
                    }
                }

                //중증도표시 
                strJung = "";

                COMHPC list5 = comHpcLibBService.GetOpdMasterbyPaNoDeptCodeBDate(strDrno, strDep, strBDate);

                if (!list5.IsNullOrEmpty())
                {
                    switch (list5.ERPAT)
                    {
                        case "T":
                            strJung = "TRA";
                            break;
                        case "C":
                            strJung = "CVA";
                            break;
                        case "A":
                            strJung = "ACS";
                            break;
                        default:
                            break;
                    }
                }

                strInfect = "";
                strExName = "VDRL";
                COMHPC list6 = comHpcLibBService.GetInfectMasterbyExNamePaNo(strExName, strDrno);   //등록번호조회

                if (!list6.IsNullOrEmpty())
                {
                    strInfect += "v";
                }

                strExName = "HIV";
                COMHPC list7 = comHpcLibBService.GetInfectMasterbyExNamePaNo(strExName, strDrno);   //등록번호조회

                if (!list7.IsNullOrEmpty())
                {
                    strInfect += "H";
                }

                strExName = "Hepatitis B";
                COMHPC list8 = comHpcLibBService.GetInfectMasterbyExNamePaNo(strExName, strDrno);   //등록번호조회

                if (!list8.IsNullOrEmpty())
                {
                    strInfect += "b";
                }

                strExName = "Hepatitis C";
                COMHPC list9 = comHpcLibBService.GetInfectMasterbyExNamePaNo(strExName, strDrno);   //등록번호조회

                if (!list9.IsNullOrEmpty())
                {
                    strInfect += "c";
                }

                if (hf.GetLength(PatName) > 6)
                {
                    if (VB.Right(PatName, 4) == "애기")
                    {
                        PatName = VB.Left(PatName, hf.GetLength(PatName) - 4) + "B";
                    }
                    else if (VB.Right(PatName, 2).ToUpper() == "B1")
                    {
                        PatName = VB.Left(PatName, hf.GetLength(PatName) - 2) + "1B";
                    }
                    else if (VB.Right(PatName, 2).ToUpper() == "B2")
                    {
                        PatName = VB.Right(PatName, hf.GetLength(PatName) - 2) + "2B";
                    }
                    else if (VB.Right(PatName, 3).ToUpper() == "B-1")
                    {
                        PatName = VB.Left(PatName, hf.GetLength(PatName) - 3) + "1B";
                    }
                    else if (VB.Right(PatName, 3).ToUpper() == "B-2")
                    {
                        PatName = VB.Left(PatName, hf.GetLength(PatName) - 3) +"2B";
                    }
                }

                sVolumeCode = "";
                sVW = "";
                iItem = 0;
                iBCodeCount = 0;

                List<COMHPC> list10 = comHpcLibBService.GetExamMasterResultCbySpecNo(txtSpecNo.Text);

                nRead = list10.Count;

                if (nRead > 0)
                {
                    for (int i = 0; i < nRead; i++)
                    {
                        sVolumeCode = list10[i].VOLUMECODE;

                        //종검에서는 무조건 한장씩만 발행 종검로직은 다른 조건
                        if (list10[i].BCODEPRINT > iBCodeCount)
                        {
                            iBCodeCount = list10[i].BCODEPRINT;
                        }

                        if (!sVolumeCode.Replace(" ", "").IsNullOrEmpty())
                        {
                            sVW = VB.Left(sVolumeCode, 1);
                        }

                        List<EXAM_SPECODE> list11 = examSpecodeService.GetNamebyCode(list10[i].VOLUMECODE);

                        iVolumeMax = 0;

                        if (list11.Count > 0)
                        {
                            for (int j = 0; j < list11.Count; j++)
                            {
                                if (int.Parse(list11[j].NAME.Replace("ml", "").Replace(" ", "")) > iVolumeMax)
                                {
                                    iVolumeMax = int.Parse(list11[j].NAME.Replace("ml", ""));
                                }
                                iItem += 1;
                            }
                        }

                        switch (list10[i].SUBCODE)
                        {
                            case "CR62D":
                            case "HM02":
                            case "HM14":
                            case "HM15":
                            case "HM16":
                            case "YU01":
                            case "DR371":
                                strLTDOK = "OK";
                                break;
                            default:
                                break;
                        }

                        switch (list10[i].SUBCODE)
                        {
                            case "HR014":
                            case "HR013":
                            case "HR012":
                            case "HR011":
                            case "HR01":
                            case "HR01C":
                            case "HR01I":
                                strBarCodeAdd = "Y";
                                break;
                            default:
                                break;
                        }
                    }
                }

                strVolume = "";
                if (iVolumeMax != 0)
                {
                    strVolume = iVolumeMax.ToString();
                    if (iItem > 8)
                    {
                        strVolume = (iVolumeMax + (iItem - 8) / 4).ToString();
                    }
                    else
                    {
                        strVolume = iVolumeMax.ToString();
                    }

                    //W/B의 총량을 계산(검체가 혈액인것만)
                    if (string.Compare(strSpecCode, "010") >= 0 && string.Compare(strSpecCode, "019") >= 0)
                    {
                        clsHcVariable.GnWBVolume += double.Parse(strVolume);
                    }

                    strVolume = string.Format("{0:###.0}", strVolume) + "ml";
                }

                List<COMHPC> list12 = comHpcLibBService.GetExamResultCMasterbySpecNo(txtSpecNo.Text);

                sBCodeName = "";

                for (int i = 0; i < list12.Count; i++)
                {
                    sBCodeName += list12[i].BCODENAME;
                    if (list12[i].CNT > 1)
                    {
                        sBCodeName += "*" + list12[i].CNT.ToString();
                    }
                    else
                    {
                        sBCodeName += ",";
                    }
                }
                
                if (strDep == "HR")
                {
                    //건진 바코드 항목이 없는것 (0) 건진에서 사용할것
                    List<COMHPC> list13 = comHpcLibBService.GetExamResultCMasterNotBarCodebySpecNo(txtSpecNo.Text);

                    sBCodeName1 = "";
                    for (int i = 0; i < list13.Count; i++)
                    {
                        sBCodeName1 += list13[i].MASTERCODE + ",";
                    }

                    if (sBCodeName1 == "HR02F,HR02G,HR02J,HR02K,HR02L,")
                    {
                        sBCodeName1 = "Diff,";
                    }
                    else if (sBCodeName1 == "HR02E,HR02F,HR02G,HR02J,HR02K,HR02L,")
                    {
                        sBCodeName1 = "Diff,";
                    }
                    else
                    {
                        sBCodeName1 = "";
                    }
                }

                if (sBCodeName.Replace(",", "") == "")
                {
                    return;
                }
                if (!sBCodeName1.IsNullOrEmpty())
                {
                    sBCodeName += sBCodeName1;
                }
                if (!sBCodeName.IsNullOrEmpty())
                {
                    sBCodeName = VB.Left(sBCodeName, hf.GetLength(sBCodeName) - 1);
                }

                clsHcVariable.GstrTubeMsg += strSpecimen + "/" + strTube + "/" + strVolume;

                strCodes = sBCodeName;
                if (hf.GetLength(strCodes) < 34)
                {
                    strCodes1 = strCodes;
                    strCodes2 = "";
                    strCodes3 = "";
                }
                else if (hf.GetLength(strCodes) < 67)
                {
                    strCodes1 = VB.Left(strCodes, 33);
                    strCodes2 = VB.Mid(strCodes, 34, hf.GetLength(strCodes) - 33);
                    strCodes3 = "";
                }
                else if (hf.GetLength(strCodes) < 99)
                {
                    strCodes1 = VB.Left(strCodes, 33);
                    strCodes2 = VB.Mid(strCodes, 34, 33);
                    strCodes3 = VB.Mid(strCodes, 67, hf.GetLength(strCodes) - 66);
                }
                else 
                {
                    strCodes1 = VB.Left(strCodes, 33);
                    strCodes2 = VB.Mid(strCodes, 34, 33);
                    strCodes3 = VB.Mid(strCodes, 67, 33) + "(..)";
                }

                if (strHicSTRT == "E") strSTRT = "E"; //건진 채용 객담검사(응급)
                if (strSTRT == "S") strSTRT = "E";
                if (strSTRT != "E") strSTRT = "";

                for (int i = 0; i < iBCodeCount; i++)
                {
                    fn_BarCode_Print();
                }

                if (strBarCodeAdd == "Y")
                {
                    if (strSodium == "*")
                    {
                        strEDTA_R = "P / C / 2.7 ml";
                        fn_BarCode_Print();
                    }
                    if (strHeparin == "*")
                    {
                        strEDTA_R = "heparin";
                        fn_BarCode_Print();
                    }
                }

            }
            else if (sender == btnBarMini)
            {
                
            }
        }

        void fn_BarCode_Print()
        {
            int nx = 0;
            int result = 0;

            Prdata = "";
            angle = "N";

            Opt = 1; //보통
            FsOldBarName = PatName;

            Prdata = "";
            nx = clsType.PC_CONFIG.GX420D_X;

            if (nx < 0)
            {
                nx = 0;
            }

            ls_PrintSpeed = "5";         //인쇄속도
            if (clsType.PC_CONFIG.GX420D == "1")
            {
                Prdata = "^XA^FWN" + "^PR" + ls_PrintSpeed + "^LH" + nx.ToString() + ",0^FS";
            }
            else
            {
                Prdata = "^XA^FWN" + "^PR" + ls_PrintSpeed + "^LH0,0^FS";
            }

            Prdata += "^SEE:UHANGUL.DAT^FS";
            Prdata += "^CW1,E:KFONT3.FNT^FS";
            Prdata += "^FO20,15^CI26^A1N,30,30^FD" + PatName + "^FS";

            if (strDep == "HR")
            {
                strDrno = FstrWRTNO;
            }

            if (strDrno == "81000013") //정도관리
            {
                Prdata += "^FO160,20^A0N,30,25^FD" + strDrno + "^FS";
            }
            else
            {
                if (strIO == "O")   //외래
                {
                    //중증도 표시 컨폼후 처리
                    if (!strJung.IsNullOrEmpty() && strDep == "ER")
                    {
                        Prdata += "^FO115,20^GB260,25,20^FS";   //혈액감염내역
                        Prdata += "^FO120,20^A0N,30,25^FR^FD" + strDrno + "  " + strAge + "/" + strSex + "  " + strDep + " " + strJung + "^FS";
                    }
                    else
                    {
                        if (hf.GetLength(PatName) <= 6)
                        {
                            Prdata += "^FO120,20^A0N,30,25^FD" + strDrno + "  " + strAge + "/" + strSex + "  " + strDep + " " + strDoctNo + "^FS";
                        }
                        else if (hf.GetLength(PatName) <= 8)
                        {
                            Prdata += "^FO160,20^A0N,30,25^FD" + strDrno + "  " + strAge + "/" + strSex + "  " + strDep + "^FS";
                        }
                        else
                        {
                            Prdata = Prdata + "^FO190,20^A0N,30,25^FD" + strDrno + "  " + strAge + "/" + strSex + "  " + strDep + "^FS";
                        }
                    }
                }
                else //입원
                {
                    if (hf.GetLength(PatName) <= 6)
                    {
                        Prdata += "^FO120,20^A0N,30,20^FD" + strDrno + "  " + strAge + "/" + strSex + "  " + strDep + "/" + strRoom + "  " + strDoctNo + "^FS";
                    }
                    else if (hf.GetLength(PatName) <= 8)
                    {
                        Prdata += "^FO160,20^A0N,30,20^FD" + strDrno + "  " + strAge + "/" + strSex + "  " + strDep + "/" + strRoom + "  " + strDoctNo + "^FS";
                    }
                    else
                    {
                        Prdata += "^FO190,20^A0N,30,25^FD" + strDrno + "  " + strAge + "/" + strSex + "  " + strDep + " " + strRoom + "^FS";
                    }
                }
            }
            Prdata += "^FO20,60^A0N,45,45^FD" + VB.Mid(strWS, 1, 1) + "^FS";    //첫째 ws
            Prdata += "^FO20,105^A0N,35,35^FD" + VB.Mid(strWS, 2, 2) + "^FS";   //두째,세째 ws
            Prdata += "^FO20,145^A0N,45,45^FD" + strSTRT + "^FS";               //응급(E)

            //------------< 바코드시작 > tla 바코드
            if (clsType.PC_CONFIG.GX420D == "1")
            {
                Prdata += "^FO57,50^BY2,2:1";                                           // 바코드 인쇄 (10자리)
            }
            else
            {
                Prdata += "^FO60,50^BY2,2:1";                                           // 바코드 인쇄 (10자리)
            }
            Prdata += "^B3N,N,80,N,N";   //Barcode Type: Code 39 (SubSets A,B and C)
            Prdata += "^FD" + strSpno + "^FS";
            //------------<바코드끝>

            if (strWon.IsNullOrEmpty())//원거리              
            {
                Prdata += "^FO390,50^GB70,30,20^FS";                    //혈액감염내역
                Prdata += "^FO390,50^A0N,32,27^FR^FD" + strWon + "^FS"; //혈액감염내
            }

            if (!strInfect.IsNullOrEmpty()) //혈액감염 내역
            {
                Prdata += "^FO380,70^GB70,30,20^FS";                        //혈액감염내역
                Prdata += "^FO380,70^A0N,32,27^FR^FD" + strInfect + "^FS";  //혈액감염내역
                Prdata += "^FO90,140^A0N,25,20^FD" + VB.Left(strSpno, 6) + "-" + VB.Right(strSpno, 4) + " " + strTime;
            }
            else
            {
                Prdata = Prdata + "^FO70,140^A0N,25,20^FD" + VB.Left(strSpno, 6) + "-" + VB.Right(strSpno, 4) + " " + strTime;
            }

            if (!strEDTA_R.IsNullOrEmpty())
            {
                Prdata += "  " + strEDTA_R + "^FS";
            }
            else
            {
                Prdata += "  " + strSpecimen + " / " + strTube + " / " + strVolume + "^FS";
            }

            if (strEDTA == "*")
            {
                Prdata += "^FO7,136^GB55,25,20^FS";
                Prdata += "^FO15,160^A0N,20,20^FR^FD" + "EDTA" + "^FS";   //EDTA 표시
            }

            if (strLTDNAME.IsNullOrEmpty() && strLTDOK == "OK")
            {
                Prdata += "^FO65,160^CI26^A1N,20,20^FD" + strCodes1 + strLTDNAME + "^fs";
            }
            else
            {
                Prdata += "^FO65,160^A0N,20,20^FD" + strCodes1 + "^FS";
            }
            Prdata += "^FO65,180^A0N,20,20^FD" + strCodes2 + "^FS";
            Prdata += "^FO65,280^A0N,20,20^FD" + strCodes3 + "^FS";

            //ws가B 인 경우  혈액형 표시
            if (hf.GetLength(strWS.Replace("B", "")) != hf.GetLength(strWS))
            {
                Prdata += "^FO300,150^A0N,45,45^FD" + strAbo + "^FS";
            }

            Prdata += "^XZ";
            FnBarCnt += 1; //바코드 출력 누적

            //출장검진은 직접 인쇄하지 않고 WORK DB를 이용하여 바코드를 인쇄함
            if (clsHcVariable.GbBarcodeDbSend == true)
            {
                clsDB.setBeginTran(clsDB.DbCon);

                result = comHpcLibBService.InsertHicSpecmstWork(Prdata);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }
            else
            {
                fn_RawPrint_cls(Prdata);
            }

            if (clsHcVariable.GbHicChul == true)
            {
                clsDB.setBeginTran(clsDB.DbCon);

                result = comHpcLibBService.InsertHicSpecmstWork(Prdata, "N");
                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                }
                clsDB.setCommitTran(clsDB.DbCon);
            }
        }

        void fn_RawPrint_cls(string argData)
        {
            long lhPrinter;
            long lReturn;
            long lpcWritten;
            long lDoc;
            string sWrittenData;
            DOCINFO MyDocInfo = new DOCINFO();
            int nPrint;
            object ob;

            string strPrtName = "혈액환자정보";
            string strPrintName1 = "";
            string strPrintName2 = "";

            clsPrint CP = new clsPrint();

            strPrintName1 = clsPrint.gGetDefaultPrinter();
            strPrintName2 = CP.getPrinter_Chk(strPrtName.ToUpper());

            if (strPrintName2.IsNullOrEmpty())
            {
                ComFunc.MsgBox("프린터 설정 오류입니다. 전산정보팀(☏29047)에 연락바랍니다.");
                return;
            }

            string SavePath = @"C:\CMC\barcode.txt";
            string txtValue = argData;

            File.WriteAllText(SavePath, argData, Encoding.Default);

            string filename = @"C:\CMC\barcode.txt";
            reader = new StreamReader(filename);
            verdana10Font = new Font("나눔고딕", 10);
            PrintDocument Pd = new PrintDocument();
            //if (clsHcVariable.GbBarcodeDbSend == false)
            //{
            //    Pd.PrinterSettingPrintTextFileHandler(object sender, PrintPageEventArgs e)s.PrinterName = strPrintName1;
            //}
            //else
            //{
                Pd.PrinterSettings.PrinterName = strPrintName2;
            //}
            Pd.PrintPage += new PrintPageEventHandler(this.PrintTextFileHandler);
            Pd.Print();
            if (reader != null)
            {
                reader.Close();
            }
        }

        void PrintTextFileHandler(object sender, PrintPageEventArgs e)
        {
            int charactersOnPage = 0;
            int linesPerPage = 0;

            using (Font font = new Font("나눔고딕", 10))
            {
                using (StringFormat string_format = new StringFormat())
                {
                    SizeF layout_area = new SizeF(e.MarginBounds.Width, e.MarginBounds.Height);

                    e.Graphics.MeasureString(FstringToPrint, this.Font, e.MarginBounds.Size,
                                         StringFormat.GenericTypographic,
                                         out charactersOnPage, out linesPerPage);

                    e.Graphics.DrawString(FstringToPrint.Substring(0, charactersOnPage), this.Font, Brushes.Black,
                                          e.MarginBounds, StringFormat.GenericTypographic);

                    FstringToPrint = FstringToPrint.Substring(0, charactersOnPage);
                }
            }
            //e.HasMorePages = FstringToPrint.Length > 0;
            e.HasMorePages = false;
        }
    }
}
