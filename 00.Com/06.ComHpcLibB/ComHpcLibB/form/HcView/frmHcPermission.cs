using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public partial class frmHcPermission :Form
    {
        Ftpedt ftp = new Ftpedt();

        clsHcMain cHM = null;
        clsHaBase hb = null;
        HIC_PATIENT hPT = null;

        HicPatientService  hicPatientService = null;
        HicConsentService hicConsentService = null; 
        HicPrivacyAcceptService hicPrivacyAcceptService = null;
        HicPrivacyAcceptNewService hicPrivacyAcceptNewService = null;
        HicJepsuService hicJepsuService = null;
        HeaJepsuService heaJepsuService = null;

        HicResDentalService hicResDentalService = null;
        HicIeMunjinNewService hicIeMunjinNewService = null;
        HicSangdamNewService hicSangdamNewService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicJepsuHeaExjongService hicJepsuHeaExjongService = null;

        #region 동의서작성 추가
        string fstrYear = "";
        string fstrPtno = "";
        string fstrFileName1 = ""; //정보활용동의서
        string fstrFileName2 = ""; //검진동시동의서
        string fstrJepDate = "";
        string fstrSName = "";

        string FstrFileName;
        int FnFileCnt;
        string FstrDrno;
        string FstrGubun;
        string FstrPtno;
        string FstrDept;
        long FnWRTNO;
        string FstrFormList;
        string[] FstrFilePath = new string[21];
        string FstrCmd = "";
        #endregion

        public frmHcPermission()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcPermission(string argGubun)
        {
            InitializeComponent();
            SetControl();
            SetEvent();
            FstrGubun = argGubun;
        }


        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
            this.ssConsent.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(eSpdBtnClicked);
            this.ssConsent.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void SetControl()
        {
            cHM = new clsHcMain();
            hPT = new HIC_PATIENT();
            hb = new clsHaBase();

            this.timer1.Tick += new EventHandler(eTimerTick);

            hicPatientService = new HicPatientService();
            hicPrivacyAcceptService = new HicPrivacyAcceptService();
            hicPrivacyAcceptNewService = new HicPrivacyAcceptNewService();
            hicConsentService = new HicConsentService();
            hicJepsuService = new HicJepsuService();
            heaJepsuService = new HeaJepsuService();
            hicResDentalService = new HicResDentalService();
            hicIeMunjinNewService = new HicIeMunjinNewService();
            hicSangdamNewService = new HicSangdamNewService();
            hicSunapdtlService = new HicSunapdtlService();
            hicJepsuHeaExjongService = new HicJepsuHeaExjongService();
        }

        /// <summary>
        /// 다른 폼에서 셀 더블클릭 호출시 사용
        /// </summary>
        /// <param name="ssConsent"></param>
        /// <param name="nRow"></param>
        public void CellDblClicked(int Row)
        {
           
            string strPtNo = "";
            string strJepDate = "";
            string strDate = "";
            string strDept = "";
            string strForm = "";

            string str동의일자 = "";
            string str검사일자 = "";
            string strDeptName = "";
            string strSname = "";
            string strSex = "";
            long nAge = 0;
            long nWRTNO = 0;

            string strHPhone = "";
            string strJuso = "";
            string strJumin = "";

            string strGubun = "";


            string strLtdName = "";
            string strTel = "";
            int[] nGbSangdam = new int[21];
            string[] strPjSangdam = new string[21];

            string strUcodes = "";


            FstrFileName = "";
            FnFileCnt = 0;

            //-------------------------------
            //    동의서 받기
            //-------------------------------
          
            strPtNo = hPT.PTNO;
            strSname = hPT.SNAME;
            strJumin = VB.Left(clsAES.DeAES(hPT.JUMIN2), 6) + "-" + VB.Mid(clsAES.DeAES(hPT.JUMIN2), 7, 7);
            strGubun = FstrGubun;


            HIC_PATIENT item = hicPatientService.GetPatInfoByPtno(strPtNo);
            strHPhone = item.HPHONE;
            strJuso = item.JUSO1 + " " + item.JUSO2;

            //성별, 나이를 읽음
            if (FstrGubun == "HEA")
            {
                HEA_JEPSU list = heaJepsuService.GetItembyPtNoBdate(strPtNo, DateTime.Now.ToShortDateString());

                if (!list.IsNullOrEmpty())
                {
                    strSex = list.SEX;
                    nAge = list.AGE;
                    strJepDate = fstrJepDate;
                    strLtdName = hb.READ_Ltd_Name(list.LTDCODE.ToString());
                    strTel = "";
                    //strMUN3 = list.BUSEIPSA;
                    FstrDrno = "";
                    strUcodes = "";
                }
            }
            else
            {
                HIC_JEPSU list = hicJepsuService.GetItembyPtNo(strPtNo);

                if (!list.IsNullOrEmpty())
                {
                    strSex = list.SEX;
                    nAge = list.AGE;
                    strJepDate = fstrJepDate;
                    strLtdName = hb.READ_Ltd_Name(list.LTDCODE.ToString());
                    strTel = list.TEL;
                    //strMUN3 = list.BUSEIPSA;
                    FstrDrno = list.SANGDAMDRNO.ToString();
                    strUcodes = list.UCODES;
                }
            }


            //IList<HIC_JEPSU_HEA_EXJONG> Jep = hicJepsuHeaExjongService.GetListWrtnoByHic(strPtNo, strJepDate);
            IList<HIC_JEPSU_HEA_EXJONG> Jep = hicJepsuHeaExjongService.ValidJepsu(strPtNo, strJepDate);

            if (Jep.Count > 0)
            {
                nWRTNO = Jep[0].WRTNO;
            }
            else
            {
                nWRTNO = 0;
            }

            if (nWRTNO == 0)
            {
                return;
            }

            strForm = ssConsent.ActiveSheet.Cells[Row, 3].Text;
            strDate = clsPublic.GstrSysDate.Replace("-", "");

            //if (strForm == "D50")
            //{
            //    strGubun = "HIC";
            //}
            //else
            //{
            //    strGubun = "";
            //}

            if (strGubun == "HIC")
            {
                strDept = "HR";
                strDeptName = "일반검진";
            }
            else
            {
                strDept = "TO";
                strDeptName = "종합검진";
            }

            if (strForm == "D54")
            {
                strDept = "HR";
                strDeptName = "일반검진";
            }

            //2020-03-10
            FstrCmd = @"C:\_spool\PenToolController.exe ";
            FstrCmd += strDate + ";" + strPtNo + ";_" + string.Format("{0:#0}", nWRTNO) + "_" + strSname + "_;" + strSex + ";" + nAge + ";" + strDept + ";";

            FstrPtno = strPtNo;
            fstrSName = strSname;
            FstrDept = strDept;
            FnWRTNO = nWRTNO;
            FstrFormList = "";

            FstrFileName = strDate + "_" + string.Format("{0:#0}", nWRTNO) + "_" + strSname + "_" + string.Format("{0:#0}", nAge) + strSex + "O" + strDept + ".jpg";
            //strDeptName = "일반검진";

            //if (nCNT > 0)
            //{
            //GoSub SET_NeoPenDesk
            FnFileCnt = FnFileCnt + 1;
            timer1.Enabled = true;
            fn_SET_NeoPenDesk(strForm, FstrFormList, FnFileCnt, FstrCmd, FstrDrno, strPtNo, strSname, strSex, nAge, str동의일자, str검사일자, strDeptName, strHPhone, strJumin, strJuso);
            //}

            DirectoryInfo Dir = new DirectoryInfo("C:\\_spool");

            if (Dir.Exists == false)
            {
                MessageBox.Show("동의서 실행파일이 없습니다!(C:\\_spool\\PenToolController.exe)" + "\r\n" + "전산팀으로 문의 바랍니다!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                //VB.Shell(@"c:\_spool\PenToolController.exe 20200801;81000004;_1046282_전산실연습_;M;70;TO;P;@C@;D52.ini;D52;2020;08;01;전산실연습;500101;1111111;", "NormalFocus");
                VB.Shell(FstrCmd, "NormalFocus");
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            int nCNT = 0;
            string strOK = "";
            string strPtNo = "";
            string strJepDate = "";
            string strDate = "";
            string strDept = "";
            string strForm = "";
            string strMsg = "";

            string str동의일자 = "";
            string str검사일자 = "";
            string str검사일자1 = "";
            string strDeptName = "";
            string strExName = "";
            string strExName2 = "";
            string strRel = "";
            string strTemp = "";
            string strCT = "";
            string strSname = "";
            string strSex = "";
            long nAge = 0;
            long nWRTNO = 0;
            long nDrNO = 0;
            bool bASA = false;

            string strHPhone = "";
            string strJuso = "";
            string strJumin = "";
            string str동의일자1 = "";
            string str동의일자2 = "";
            string str동의일자3 = "";

            string strGubun = "";

            string strResult1 = "";
            string strResult2 = "";
            string strResult3 = "";
            string strResult4 = "";
            string strResult5 = "";
            string strResult6 = "";
            string strResult7 = "";
            string strResult8 = "";
            string strResult9 = "";
            string strResult10 = "";
            string strResult11 = "";
            string strResult12 = "";
            string strResult13 = "";
            string strResult14 = "";
            string strResult15 = "";
            string strResult16 = "";
            string strResult17 = "";
            string strResult18 = "";
            string strResult19 = "";
            string strResult20 = "";
            string strResult21 = "";
            string strResult22 = "";
            string strResult23 = "";
            string strResult24 = "";
            string strResult25 = "";
            string strResult26 = "";
            string strResult27 = "";
            string strResult28 = "";
            string strResult29 = "";
            string strResult30 = "";
            string strResult31 = "";
            string strResult32 = "";

            string strLtdName = "";
            string strTel = "";
            string strMunjinRes = "";

            string strMUN1 = "";
            string strMUN2 = "";
            string strMUN3 = "";
            string strMUN4 = "";
            string strMUN5 = "";
            string strMUN6 = "";
            string strMUN7 = "";
            string strMUN8 = "";
            string strMUN9 = "";
            string strMUN10 = "";
            string strOMR = "";

            int nRow = 0;
            int nREAD = 0;
            string strData = "";
            int[] nGbSangdam = new int[21];
            string[] strPjSangdam = new string[21];

            string strPjSangdam1 = "";
            string strSANGDAMNO = "";
            string strDentalDrno = "";
            string strSpcDent = "";
            string strUcodes = "";
            int k = 0;

            FstrFileName = "";
            FnFileCnt = 0;

            //-------------------------------
            //    동의서 받기
            //-------------------------------
            strOK = "";
            nCNT = 0;

            strPtNo = hPT.PTNO;
            strSname = hPT.SNAME;
            strHPhone = hPT.HPHONE;
            strJuso = hPT.JUSO1 + " " + hPT.JUSO2;
            strJumin = VB.Left(clsAES.DeAES(hPT.JUMIN2), 6) + "-" + VB.Mid(clsAES.DeAES(hPT.JUMIN2), 7, 7);
            //nWRTNO = hPT.WRTNO;
            strGubun = FstrGubun;

            //성별, 나이를 읽음
            if (FstrGubun == "HEA")
            {
                HEA_JEPSU list = heaJepsuService.GetItembyPtNoBdate(strPtNo, DateTime.Now.ToShortDateString());

                if (!list.IsNullOrEmpty())
                {
                    strSex = list.SEX;
                    nAge = list.AGE;
                    strJepDate = fstrJepDate;
                    strLtdName = hb.READ_Ltd_Name(list.LTDCODE.ToString());
                    strTel = "";
                    strMUN3 = "";
                    FstrDrno = "";
                    strUcodes = "";
                }
            }
            else
            {
                HIC_JEPSU list = hicJepsuService.GetItembyPtNo(strPtNo);

                if (!list.IsNullOrEmpty())
                {
                    strSex = list.SEX;
                    nAge = list.AGE;
                    strJepDate = fstrJepDate;
                    strLtdName = hb.READ_Ltd_Name(list.LTDCODE.ToString());
                    strTel = list.TEL;
                    strMUN3 = list.BUSEIPSA;
                    FstrDrno = list.SANGDAMDRNO.ToString();
                    strUcodes = list.UCODES;
                }
            }



            //IList<HIC_JEPSU_HEA_EXJONG> Jep = hicJepsuHeaExjongService.GetListWrtnoByHic(strPtNo, strJepDate);
            IList<HIC_JEPSU_HEA_EXJONG> Jep = hicJepsuHeaExjongService.ValidJepsu(strPtNo, strJepDate);
            if (Jep.Count > 0)
            {
                nWRTNO = Jep[0].WRTNO;
            }
            else
            {
                nWRTNO = 0;
            }

            if (nWRTNO == 0)
            {
                return;
            }

            strForm = ssConsent.ActiveSheet.Cells[e.Row, 3].Text;
            strDate = clsPublic.GstrSysDate.Replace("-", "");

            //if (strForm == "D50")
            //{
            //    strGubun = "HIC";
            //}
            //else
            //{
            //    strGubun = "";
            //}

            if (strGubun == "HIC")
            {
                strDept = "HR";
                strDeptName = "일반검진";
            }
            else
            {
                strDept = "TO";
                strDeptName = "종합검진";
            }

            if (strForm == "D54")
            {
                strDept = "HR";
                strDeptName = "일반검진";
            }

            //2020-03-10
            FstrCmd = @"C:\_spool\PenToolController.exe ";
            FstrCmd += strDate + ";" + strPtNo + ";_" + string.Format("{0:#0}", nWRTNO) + "_" + strSname + "_;" + strSex + ";" + nAge + ";" + strDept + ";";

            FstrPtno = strPtNo;
            fstrSName = strSname;
            FstrDept = strDept;
            FnWRTNO = nWRTNO;
            FstrFormList = "";

            FstrFileName = strDate + "_" + string.Format("{0:#0}", nWRTNO) + "_" + strSname + "_" + string.Format("{0:#0}", nAge) + strSex + "O" + strDept + ".jpg";
            //strDeptName = "일반검진";

            //if (nCNT > 0)
            //{
            //GoSub SET_NeoPenDesk
            FnFileCnt = FnFileCnt + 1;
            timer1.Enabled = true;
            fn_SET_NeoPenDesk(strForm, FstrFormList, FnFileCnt, FstrCmd, FstrDrno, strPtNo, strSname, strSex, nAge, str동의일자, str검사일자, strDeptName, strHPhone, strJumin, strJuso);
            //}

            DirectoryInfo Dir = new DirectoryInfo("C:\\_spool");

            if (Dir.Exists == false)
            {
                MessageBox.Show("동의서 실행파일이 없습니다!(C:\\_spool\\PenToolController.exe)" + "\r\n" + "전산팀으로 문의 바랍니다!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                //VB.Shell(@"c:\_spool\PenToolController.exe 20200801;81000004;_1046282_전산실연습_;M;70;TO;P;@C@;D52.ini;D52;2020;08;01;전산실연습;500101;1111111;", "NormalFocus");
                VB.Shell(FstrCmd, "NormalFocus");
            }
        }

        private void eSpdBtnClicked(object sender, EditorNotifyEventArgs e)
        {

            string strFile = "";
            string strPath = "";
            string strLocal = "";
            string strServer = "";
            string strHost = "";
            Ftpedt FtpedtX = new Ftpedt();

            if (e.Row == 0)
            {
                if (ssConsent.ActiveSheet.Cells[e.Row, 1].Text != "")
                {

                    HIC_CONSENT item = hicConsentService.GetIetmByPtnoForm(fstrPtno, "D50");
                    if (!item.IsNullOrEmpty())
                    {
                        strFile = item.FILENAME1;

                        strPath = "C:\\cmc";
                        strLocal = strPath + "\\" + "ETC.jpg";
                        strServer = "/data/hic_result/consent_temp/" + strFile;
                        strHost = "/data/hic_result/consent_temp/";

                        //서버에서 PC로 파일을 다운로드함
                        clsHcVariable.FTP_Pass = ftp.READ_FTP(clsDB.DbCon, clsHcVariable.FTP_IpAddr, clsHcVariable.FTP_User);
                        ftp.FtpDownload(clsHcVariable.FTP_IpAddr, clsHcVariable.FTP_User, clsHcVariable.FTP_Pass, strLocal, strServer, strHost);

                        strFile = "rundll32.exe shimgvw.dll, ImageView_Fullscreen " + strLocal;
                        VB.Shell(strFile, "MaximizedFocus");
                        Application.DoEvents();
                    }
                }
            }
            else if (e.Row == 1)
            {
                if (ssConsent.ActiveSheet.Cells[e.Row, 1].Text != "")
                {
                    frmHcConsentformView f = new frmHcConsentformView(fstrSName, fstrJepDate);
                    //frmHaConsentApproval f = new frmHaConsentApproval();
                    f.Show();
                }
            }
            else if (e.Row == 2)
            { 
                if (ssConsent.ActiveSheet.Cells[e.Row, 1].Text != "")
                {
                    if (fstrFileName1 != "")
                    {
                        strPath = "C:\\cmc";
                        strLocal = strPath + "\\" + "ETC.jpg";
                        strServer = "/data/hic_result/privacy_accept/" + fstrYear +"/"+ fstrFileName1;
                        strHost = "/data/hic_result/privacy_accept/";

                        //서버에서 PC로 파일을 다운로드함
                        clsHcVariable.FTP_Pass = ftp.READ_FTP(clsDB.DbCon, clsHcVariable.FTP_IpAddr, clsHcVariable.FTP_User);
                        ftp.FtpDownload(clsHcVariable.FTP_IpAddr, clsHcVariable.FTP_User, clsHcVariable.FTP_Pass, strLocal, strServer, strHost);

                        strFile = "rundll32.exe shimgvw.dll, ImageView_Fullscreen " + strLocal;
                        VB.Shell(strFile, "MaximizedFocus");
                        Application.DoEvents();
                    }
                }
            }
            else if (e.Row == 3)
            { 
                if (ssConsent.ActiveSheet.Cells[e.Row, 1].Text != "")
                {
                    if (fstrFileName2 != "")
                    {
                        strPath = "C:\\cmc";
                        strLocal = strPath + "\\" + "ETC.jpg";
                        strServer = "/data/hic_result/privacy_accept_new/" + fstrYear + "/" + fstrFileName2;
                        strHost = "/data/hic_result/privacy_accept_new/";

                        //서버에서 PC로 파일을 다운로드함
                        clsHcVariable.FTP_Pass = ftp.READ_FTP(clsDB.DbCon, clsHcVariable.FTP_IpAddr, clsHcVariable.FTP_User);
                        ftp.FtpDownload(clsHcVariable.FTP_IpAddr, clsHcVariable.FTP_User, clsHcVariable.FTP_Pass, strLocal, strServer, strHost);

                        strFile = "rundll32.exe shimgvw.dll, ImageView_Fullscreen " + strLocal;
                        VB.Shell(strFile, "MaximizedFocus");
                        Application.DoEvents();
                    }
                }
            }
        }

        private void eFormload(object sender, EventArgs e)
        {
            Screen_Clear();
        }

        public void Screen_Clear()
        {
            for (int i = 0; i < ssConsent.ActiveSheet.RowCount; i++)
            {
                ssConsent.ActiveSheet.Cells[i, 1].Text = "";
            }
        }

        public void SetDisplay(string argPtno, string argYear, string argJepDate, string[] argDept, string argSname)
        {
            Screen_Clear();
            fstrPtno = argPtno;
            fstrYear = argYear;
            fstrSName = argSname;
            fstrJepDate = argJepDate;

            hPT = hicPatientService.GetPatInfoByPtno(argPtno);

            //개인정보 동의서
            ssConsent.ActiveSheet.Cells[0, 1].Text = hicPatientService.GetPrivacyNewByPtno(argPtno);

            //내시경 동의서
            HIC_CONSENT item = hicConsentService.GetIetmByPtnoSdateDeptForm(argPtno, argJepDate, argDept, "D10");
            if (!item.IsNullOrEmpty())
            {
                ssConsent.ActiveSheet.Cells[1, 1].Text = VB.Left(item.DOCTSIGN.ToString(), 10);
            }
                
            //정보활용 동의서
            HIC_PRIVACY_ACCEPT item1 = hicPrivacyAcceptService.GetIetmByPtnoYear(argPtno, argYear);
            if (!item1.IsNullOrEmpty())
            {
                fstrFileName1 = item1.FILENAME;
                ssConsent.ActiveSheet.Cells[2, 1].Text = item1.ENTDATE;
            }
            
            //검진동시 동의서
            HIC_PRIVACY_ACCEPT_NEW item2 = hicPrivacyAcceptNewService.GetIetmByPtnoYear(argPtno, argYear);
            if (!item2.IsNullOrEmpty())
            {
                fstrFileName2 = item2.FILENAME;
                ssConsent.ActiveSheet.Cells[3, 1].Text = item2.ENTDATE;
            }

            //건강진단 개인표
            ssConsent.ActiveSheet.Cells[4, 1].Text = "";
        }

        void fn_SET_NeoPenDesk(string strForm, string FstrFormList, int FnFileCnt, string strCmd, string FstrDrno, string strPtNo, string strSname, string strSex, long nAge, string str동의일자, string str검사일자, string strDeptName, string strHPhone, string strJumin, string strJuso)
        {
            if (strForm == "D10")
            {
                FstrFormList += "'D10',";
                FnFileCnt += 1;
                FstrFilePath[FnFileCnt - 1] = "D10";
                FnFileCnt += 1;
                FnFileCnt += 1; FstrFilePath[FnFileCnt - 1] = "D11";
                strCmd += "P;@C@;D10_" + FstrDrno + ".ini;D10_" + FstrDrno + ";" + strPtNo + ";" + strSname + ";" + strSex + "/" + nAge.ToString() + ";";
                strCmd += str동의일자 + ";" + str검사일자 + ";" + strDeptName + ";";
                strCmd += "P;@C@;D11.ini;D11; ;";
            }
            else if (strForm == "D50")  //개인정보동의서
            {
                FnFileCnt += 1;
                FstrFilePath[FnFileCnt - 1] = "D50";
                FstrFormList = FstrFormList + "'D50'";
                strCmd += "P;@C@;D50.ini;D50" + ";" + strSname + ";" + strHPhone + ";" + strJumin + ";" + strJuso + ";";
                strCmd += VB.Left(clsPublic.GstrSysDate, 4) + ";" + VB.Mid(clsPublic.GstrSysDate, 6, 2) + ";" + VB.Right(clsPublic.GstrSysDate, 2) + ";" + strSname + ";";
            }
            else if (strForm == "D51")
            {

            }
            else if (strForm == "D52")
            {
                FnFileCnt += 1;
                FstrFilePath[FnFileCnt - 1] = "D52";

                FstrFormList += "'D52'";
                strCmd += "P;@C@;D52.ini;D52" + ";" + VB.Left(clsPublic.GstrSysDate, 4) + ";" + VB.Mid(clsPublic.GstrSysDate, 6, 2) + ";" + VB.Right(clsPublic.GstrSysDate, 2) + ";";
                strCmd = strCmd + strSname + ";" + VB.Left(strJumin, 6) + ";" + VB.Right(strJumin, 7) + ";";
            }
            else if (strForm == "D53")
            {
                FnFileCnt = FnFileCnt + 1;
                FstrFilePath[FnFileCnt - 1] = "D53";
                FstrFormList += "'D53'";
                strCmd += "P;@C@;D53.ini;D53" + ";" + VB.Left(clsPublic.GstrSysDate, 4) + ";" + VB.Mid(clsPublic.GstrSysDate, 6, 2) + ";" + VB.Right(clsPublic.GstrSysDate, 2) + ";" + strSname + ";";
            }
            else if (strForm == "D54")
            {

            }

            FstrCmd = strCmd;
        }

        private void eTimerTick(object sender, EventArgs e)
        {
            //동의서 FTP전송
            long nSeq = 0;
            long nDrSabun = 0;
            string strFile = "";
            string strServer = "";
            string strOK = "";
            string strYear = "";
            string strOLD = "";
            string strNew = "";


            FileInfo Dir = null;

            //동의서 파일이 저장되었는지 점검
            strOK = "OK";
            for (int i = 1; i <= FnFileCnt; i++)
            {
                if (FstrFilePath[i] == "D54")
                {
                    strFile = "c:\\_spool\\Save\\" + FstrFilePath[i] + "_" + FstrDrno + "\\" + FstrFileName;
                }
                else
                {
                    strFile = "c:\\_spool\\Save\\" + FstrFilePath[i] +  "\\" + FstrFileName;
                }
                
                Dir = new FileInfo(strFile);

                if (Dir.Exists == false || strFile == "" ) { strOK = "";} 

            }
            if (strOK == "") { timer1.Enabled = true;  return; }

            //파일을 서버에 저장함
            nSeq = 0; strOLD = "";

            for (int i = 1; i <= FnFileCnt; i++)
            {
                if (FstrFilePath[i] == "D54")
                {
                    strFile = "c:\\_spool\\Save\\" + FstrFilePath[i] + "_" + FstrDrno + "\\" + FstrFileName;
                }
                else
                {
                    strFile = "c:\\_spool\\Save\\" + FstrFilePath[i] + "\\" + FstrFileName;
                }


                strNew = FstrFilePath[i];
                if (strNew != strOLD)
                {
                    strOLD = strNew;
                    nSeq = 1;
                }
                else
                {
                    nSeq = nSeq + 1;
                }
                strServer = VB.Format(FnWRTNO, "#0") + "_" + FstrDept + "_" + FstrFilePath[i] + "_" + VB.Format(nSeq, "#0") + ".jpg";

                //Sleep 100

                strYear = VB.Left(clsPublic.GstrSysDate.Replace("-", ""), 4);
                Ftpedt FtpedtX = new Ftpedt();

                if (FstrFilePath[i] == "D50")
                {

                    //디렉토리 만듬
                    //Call FTP_CREATE_DIRECTORY_Process("/data/hic_result/consent_temp")

                    //서버에 FTP로 파일을 전송
                    if ((FtpedtX.FtpUpload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFile, strServer, "/data/hic_result/consent_temp/") == true))
                    {
                        int result = hicConsentService.UpdateItemByWrtno(FstrPtno, FnWRTNO, "OK");

                        if (result < 0)
                        {
                            MessageBox.Show("동의서를 서버로 전송을 못하였습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        int result = hicConsentService.UpdateItemByWrtno(FstrPtno, FnWRTNO, "");

                    }

                }
                //정보활용동의서
                else if (FstrFilePath[i] == "D52")
                {
                    if ((FtpedtX.FtpUpload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFile, strServer, "/data/hic_result/privacy_accept/"+ strYear) == true))
                    {
                        //int result = hicConsentService.UpdateItemByWrtno(FstrPtno, FnWRTNO, "OK");

                        HIC_PRIVACY_ACCEPT nHC52 = new HIC_PRIVACY_ACCEPT
                        {
                            GJYEAR = strYear,
                            PTNO = FstrPtno,
                            SNAME = fstrSName,
                            FILENAME = strServer

                        };

                        if (!hicPrivacyAcceptService.Insert(nHC52))
                        {
                            MessageBox.Show("정보활용동의서 오류가 발생함.", "오류");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                    }

                }
                //검진동시동의서
                else if (FstrFilePath[i] == "D53")
                {

                    if ((FtpedtX.FtpUpload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFile, strServer, "/data/hic_result/privacy_accept_new/"+ strYear) == true))
                    {
                        //int result = hicConsentService.UpdateItemByWrtno(FstrPtno, FnWRTNO, "OK");

                        HIC_PRIVACY_ACCEPT_NEW nHC53 = new HIC_PRIVACY_ACCEPT_NEW
                        {
                            GJYEAR = strYear,
                            PTNO = FstrPtno,
                            SNAME = fstrSName,
                            FILENAME = strServer

                        };

                        if (!hicPrivacyAcceptNewService.Insert(nHC53))
                        {
                            MessageBox.Show("정보활용동의서 오류가 발생함.", "오류");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                    }
                }
                //건강진단개인표(일반검진)
                else if (FstrFilePath[i] == "D54")
                {

                }

            }

            for (int i = 1; i <= FnFileCnt; i++)
            {
                if (FstrFilePath[i] == "D54")
                {
                    strFile = "c:\\_spool\\Save\\" + FstrFilePath[i] + "_" + FstrDrno + "\\" + FstrFileName;
                }
                else
                {
                    strFile = "c:\\_spool\\Save\\" + FstrFilePath[i] + "\\" + FstrFileName;
                }

                Dir = new FileInfo(strFile);
                if (Dir.Exists == false)
                {
                    strOK = ""; return;
                }

            }

            //PC의 파일을 삭제함
            for (int i = 1; i <= FnFileCnt; i++)
            {
                if (FstrFilePath[i] == "D54")
                {
                    strFile = "c:\\_spool\\Save\\" + FstrFilePath[i] + "_" + FstrDrno + "\\" + FstrFileName;
                }
                else
                {
                    strFile = "c:\\_spool\\Save\\" + FstrFilePath[i] + "\\" + FstrFileName;
                }


                Dir = new FileInfo(strFile);
                if (Dir.Exists == true)
                {
                    Dir.Delete();
                }

            }

            if( strNew == "D50")
            {
                if( VB.Trim(FstrPtno) != "")
                {

                    int result = hicConsentService.UpdateTimeByWrtnoForm(clsPublic.GstrSysDate, FnWRTNO, fstrPtno, strNew);

                    int result1 = hicPatientService.UpdatePrivacyByPtno(FstrPtno);
                    if (result1 < 0)
                    {
                        MessageBox.Show("인적사항 개인정보동의서 완료여부 UPDATE 오류!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            timer1.Enabled = false;

        }

        /// <summary>
        /// 문진 결과가 이중으로 발생한 것을 중복 제거 후 결과를 반환함
        /// </summary>
        /// <param name="argMunRes"></param>
        /// <returns></returns>
        string fn_Conv_Munjin_Res(string argMunRes)
        {
            string rtnVal = "";

            int nTblCnt = 0;
            int nDCnt = 0;
            int nColCnt = 0;
            string strTemp1 = "";
            string strTemp2 = "";
            string strTemp3 = "";
            string strTable = "";
            string strResult = "";
            string[] strCol = new string[101];
            long k = 0;

            nTblCnt = (int)VB.L(argMunRes, "{<*>}");
            strResult = "";
            for (int i = 0; i < nTblCnt; i++)
            {
                for (int j = 0; j <= 100; j++)
                {
                    strCol[j] = "";
                }

                strTemp1 = VB.Pstr(argMunRes, "{<*>}", i + 2);
                strTable = VB.Pstr(strTemp1, "{*}", 1);
                strTemp2 = VB.Pstr(strTemp1, "{*}", 3);
                nDCnt = (int)VB.L(strTemp2, "{}");

                for (int j = 0; j < nDCnt; j++)
                {
                    strTemp3 = VB.Pstr(strTemp2, "{}", j);
                    k = long.Parse(VB.Pstr(strTemp3, ",", 1));
                    if (k > 0)
                    {
                        nColCnt = (int)k;
                        strCol[nColCnt] = strTemp3;
                    }
                }

                strResult += "{<*>}" + strTable + "{*}" + string.Format("{0:#0}", nColCnt) + "{*}";
                for (int j = 0; j < nColCnt; j++)
                {
                    strResult += strCol[j] + "{}";
                }
            }

            rtnVal = strResult;

            return rtnVal;
        }
    }
}
