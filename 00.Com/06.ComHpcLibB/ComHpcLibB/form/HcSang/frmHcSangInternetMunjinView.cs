using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB\HC_Sang
/// File Name       : frmHcSangInternetMunjinView.cs
/// Description     : 인터넷문진표 보기
/// Author          : 이상훈
/// Create Date     : 2020-02-17
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm인터넷문진보기.frm(Frm인터넷문진보기)" />

namespace ComHpcLibB
{
    public partial class frmHcSangInternetMunjinView : Form
    {
        HicIeMunjinNewService hicIeMunjinNewService = null;
        HicIeMunjinViewService hiciemunjinViewService = null;
        HicIeMunjinViewService hicIeMunjinViewService = null;

        frmHcPanInternetMunjin_New FrmHcPanInternetMunjin_New = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();

        const int hNULL = 0;
        long fnLicense = 0;
        long FnCnt;
        long FnMunID;
        long FnWRTNO;
        string FstrJepDate;
        string FstrPtno;
        string FstrGjJong;
        string FstrROWID;
        string FstrOSVer;
        string FstrForm;

        string FstrViewKey;

        public frmHcSangInternetMunjinView()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcSangInternetMunjinView(long nWrtNo, string strJepDate, string strPtNo, string strGjJong, string strViewKey, long nLicense = 0)
        {
            InitializeComponent();

            FnWRTNO = nWrtNo;
            FstrJepDate = strJepDate;
            FstrPtno = strPtNo;
            FstrGjJong = strGjJong;
            FstrROWID = "";
            FstrViewKey = strViewKey;
            fnLicense = nLicense;

            SetEvent();
        }

        void SetEvent()
        {
            hicIeMunjinNewService = new HicIeMunjinNewService();
            hiciemunjinViewService = new HicIeMunjinViewService();
            hicIeMunjinViewService = new HicIeMunjinViewService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.timer1.Tick += new EventHandler(eTimerTick);
            this.timer2.Tick += new EventHandler(eTimerTick);
            this.FormClosing += new FormClosingEventHandler(eFormClosing);

        }

        void eFormClosing(object sender, FormClosingEventArgs e)
        {
            if (hf.OpenForm_Check("frmHcPanInternetMunjin_New") == true)
            {
                FrmHcPanInternetMunjin_New.Close();
                FrmHcPanInternetMunjin_New.Dispose();
                FrmHcPanInternetMunjin_New = null;
            }
        }

        bool FormIsExist(Type tp)
        {
            foreach (Form ff in this.MdiChildren)
            {
                if (ff.GetType() == tp)
                {
                    ff.Focus();
                    ff.BringToFront();
                    return true;
                }
            }
            return false;
        }

        void eFormLoad(object sender, EventArgs e)
        {
            FnCnt = 0;
            FnMunID = 0;

            this.Hide();

            timer1.Enabled = true;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                if (hf.OpenForm_Check("frmHcPanInternetMunjin_New") == true)
                {
                    FrmHcPanInternetMunjin_New.Close();
                    FrmHcPanInternetMunjin_New.Dispose();
                    FrmHcPanInternetMunjin_New = null;
                }

                this.Close();
                return;
            }
        }

        void eTimerTick(object sender, EventArgs e)
        {
            if (sender == timer1)
            {
                string strFrDate = "";
                string strToDate = "";
                string strViewId = "";
                int result = 0;

                //strFrDate = DateTime.Parse(FstrJepDate).AddDays(-180).ToShortDateString();
                strFrDate = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";
                strToDate = DateTime.Parse(FstrJepDate).AddDays(1).ToShortDateString();

                //2021년 1,2월 한시적으로 전년도 문진조회
                //if (string.Compare(FstrJepDate, "2021-03-01") < 0) { strFrDate = "2020-01-01";}

                FnCnt = 0;
                FnMunID = 0;
                timer1.Enabled = false;

                //인터넷문진표(New)
                HIC_IE_MUNJIN_NEW list = hicIeMunjinNewService.GetItembyPtNoJepDateGjJong(FstrPtno, strFrDate, strToDate, FstrGjJong);

                if (!list.IsNullOrEmpty())
                {
                    FstrROWID = list.ROWID.Trim();
                    FstrForm = list.RECVFORM.Trim();
                    if (VB.InStr(FstrForm, "12001") > 0)
                    {
                        FstrForm = FstrForm.Replace("12001", "12001,12005");
                    }

                    if (hiciemunjinViewService.GetAllbyViewKey(FstrROWID, clsPublic.GstrSysDate) == 0)
                    {
                        clsDB.setBeginTran(clsDB.DbCon);

                        result = hiciemunjinViewService.Insert(clsPublic.GstrSysDate, FstrROWID);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("인터넷 문진 저장 중 오류발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        clsDB.setCommitTran(clsDB.DbCon);
                    }

                    //2021-01-19 인터넷문진뷰어 스크립트 오류 자체 해결로 아래 로직 PASS
                    //fn_Run_HcScript();

                    FstrOSVer = hf.fn_Find_OS_version().ToUpper();
                    //hf.fn_ProcessKill("friendly omr.exe");
                    timer2.Enabled = true;
                    return;
                }

                HIC_IE_MUNJIN_NEW list2 = hicIeMunjinNewService.GetItembyPtNoJepDateGjJong(FstrPtno, strFrDate, strToDate, FstrGjJong);

                if (!list2.IsNullOrEmpty())
                {
                    FstrROWID = list2.ROWID.Trim();
                    FstrForm = list2.RECVFORM.Trim();
                    if (VB.InStr(FstrForm, "12001") > 0)
                    {
                        FstrForm = FstrForm.Replace("12001", "12001,12005");
                    }

                    strViewId = hicIeMunjinViewService.GetViewIdbyViewKey(FstrViewKey, clsPublic.GstrSysDate);

                    if (strViewId.IsNullOrEmpty())
                    {
                        clsDB.setBeginTran(clsDB.DbCon);

                        result = hicIeMunjinViewService.Insert(clsPublic.GstrSysDate, FstrViewKey);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("인터넷 문진 저장 중 오류발생!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        clsDB.setCommitTran(clsDB.DbCon);
                        Thread.Sleep(1000);
                    }
                    timer2.Enabled = true;
                    return;
                }

                //====인터넷 문진Data 조회====
                if (FnWRTNO > 0)
                {
                    if (hicIeMunjinNewService.GetCountbyWrtNo(FnWRTNO) > 0)
                    {
                        clsHcVariable.GstrJepDate = DateTime.Parse(FstrJepDate).AddDays(-60).ToShortDateString();
                        clsHcVariable.GnWRTNO = FnWRTNO;
                    }
                }
                this.Close();
            }
            else if (sender == timer2)
            {
                string strURL = "";
                string strMunDate = "";
                string strRecvForm = "";
                string strSendData = "";
                string strMunjinRes = "";
                string strJusoData = "";
                string strSite = "";
                string strYear = "";
                string strIEVer = "";

                timer2.Enabled = false;
                FnCnt += 1;
                if (FnCnt >= 10)
                {
                    MessageBox.Show("인터넷문진 연동 프로그램이 응답이 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //string strViewId = hicIeMunjinViewService.GetViewIdbyViewKey(FstrViewKey, clsPublic.GstrSysDate);
                string strViewId = hicIeMunjinViewService.GetViewIdbyViewKey(FstrROWID, clsPublic.GstrSysDate);

                if (!strViewId.IsNullOrEmpty())
                {
                    FnMunID = strViewId.To<long>();
                }

                Thread.Sleep(500);

                //아직 연동이 되지 않았으면 대기함
                if (FnMunID == 0)
                {
                    timer2.Enabled = true;
                    return;
                }

                var ieVersion = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Internet Explorer").GetValue("Version");

                //XP,VISTA는 IE Old버전으로 설정
                if (VB.InStr(FstrOSVer, "XP") > 0)
                {
                    strIEVer = "8.0";
                }
                if (VB.InStr(FstrOSVer, "VISTA") > 0)
                {
                    strIEVer = "8.0";
                }
                if (VB.InStr(FstrOSVer, "98") > 0)
                {
                    strIEVer = "8.0";
                }
                if (VB.InStr(FstrOSVer, "10") > 0)
                {
                    //strIEVer = "8.0";
                    strIEVer = "11.0";
                }
                else
                {
                    strIEVer = VB.Left(ieVersion.To<string>(), 1) + ".0";
                }

                //종검2층 Windows 7
                if (VB.Left(clsType.PC_CONFIG.IPAddress, 10) == "192.168.41.80")
                {
                    strIEVer = "8.0";
                }

                strYear = VB.Left(hicIeMunjinNewService.GetMunDatebyPtNo(FstrPtno), 4);

                if (fnLicense > 0)
                {

                    //IE 9.0이상 버전
                    if (VB.Pstr(strIEVer, ".", 1).To<long>() >= 9)
                    {
                        FrmHcPanInternetMunjin_New = new frmHcPanInternetMunjin_New(FnMunID, FstrForm, FstrPtno);
                        FrmHcPanInternetMunjin_New.Show();
                    }
                    //(1) Google
                    else if (Directory.Exists(@"C:\Program Files\Google\Chrome\Application"))
                    {
                        if (string.Compare(strYear, "2019") < 0)
                        {
                            VB.Shell(@"C:\Program Files\Google\Chrome\Application\chrome.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view_2018.html?m_id=" + FnMunID + "&Chk=" + FstrForm, "NormalFocus");
                        }
                        else
                        {
                            VB.Shell(@"C:\Program Files\Google\Chrome\Application\chrome.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view.html?m_id=" + FnMunID + "&Chk=" + FstrForm, "NormalFocus");
                        }
                    }
                    else if (Directory.Exists(@"C:\Program Files (x86)\Google\Chrome\Application"))
                    {
                        if (string.Compare(strYear, "2019") < 0)
                        {
                            VB.Shell(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view_2018.html?m_id=" + FnMunID + "&Chk=" + FstrForm, "NormalFocus");
                        }
                        else
                        {
                            VB.Shell(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view.html?m_id=" + FnMunID + "&Chk=" + FstrForm, "NormalFocus");
                        }
                    }
                    //(3) 스윙브라우저
                    else if (Directory.Exists(@"C:\Users\user\AppData\Local\SwingBrowser\Application"))
                    {
                        VB.Shell(@"C:\Users\user\AppData\Local\SwingBrowser\Application\swing.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view.html?m_id=" + FnMunID + "&Chk=" + FstrForm, "NormalFocus");
                    }
                    //(4) 기본 브라우저로 표시
                    else
                    {
                        strURL = @"C:\Program Files\Internet Explorer\iexplore.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view.html?m_id=" + FnMunID + "&Chk=" + FstrForm;
                        VB.Shell(strURL, "NormalFocus");
                    }
                }
                else
                {
                    //(1) Google
                    if (Directory.Exists(@"C:\Program Files\Google\Chrome\Application"))
                    {
                        if (string.Compare(strYear, "2019") < 0)
                        {
                            VB.Shell(@"C:\Program Files\Google\Chrome\Application\chrome.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view_2018.html?m_id=" + FnMunID + "&Chk=" + FstrForm, "NormalFocus");
                        }
                        else
                        {
                            VB.Shell(@"C:\Program Files\Google\Chrome\Application\chrome.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view.html?m_id=" + FnMunID + "&Chk=" + FstrForm, "NormalFocus");
                        }
                    }
                    else if (Directory.Exists(@"C:\Program Files (x86)\Google\Chrome\Application"))
                    {
                        if (string.Compare(strYear, "2019") < 0)
                        {
                            VB.Shell(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view_2018.html?m_id=" + FnMunID + "&Chk=" + FstrForm, "NormalFocus");
                        }
                        else
                        {
                            VB.Shell(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view.html?m_id=" + FnMunID + "&Chk=" + FstrForm, "NormalFocus");
                        }
                    }

                    //IE 9.0이상 버전
                    else if (VB.Pstr(strIEVer, ".", 1).To<long>() >= 9)
                    {
                        FrmHcPanInternetMunjin_New = new frmHcPanInternetMunjin_New(FnMunID, FstrForm, FstrPtno);
                        FrmHcPanInternetMunjin_New.Show();
                    }

                    //(3) 스윙브라우저
                    else if (Directory.Exists(@"C:\Users\user\AppData\Local\SwingBrowser\Application"))
                    {
                        VB.Shell(@"C:\Users\user\AppData\Local\SwingBrowser\Application\swing.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view.html?m_id=" + FnMunID + "&Chk=" + FstrForm, "NormalFocus");
                    }
                    //(4) 기본 브라우저로 표시
                    else
                    {
                        strURL = @"C:\Program Files\Internet Explorer\iexplore.exe " + "http://www.pohangsmh.co.kr/web_question/general/g_view.html?m_id=" + FnMunID + "&Chk=" + FstrForm;
                        VB.Shell(strURL, "NormalFocus");
                    }
                }

                

                //Application.DoEvents();
                //this.Close();
                ComFunc.KillProc("friendly omr.exe");
                

            }
        }

        /// <summary>
        /// 프로세스 목록 읽기 및 실행(자격조회 오류 차단 프로그램)
        /// </summary>
        void fn_Run_HcScript()
        {
            bool blnOK = false;
            bool bOK = false;
            string strPath = @"c:\cmc\exe\";
            string strFileName = "hcscript.exe";
            string HostIp = "192.168.100.33";
            string HostId = "pcnfs";
            string HostPass = "pcnfs1";
            string LocalPath = @"C:\CMC\EXE\hcscript.exe";
            string HostFileName = "/pcnfs/exe/hcscript.exe";
            string HostPath = "/pcnfs/exe/";

            Ftpedt FtpedtX = new Ftpedt();

            DirectoryInfo Dir = new DirectoryInfo(strPath);
            if (Dir.Exists == false)
            {
                Dir.Create();
                FtpedtX.FtpDownloadEx(HostIp, HostId, HostPass, LocalPath, HostFileName, HostPath);                
            }

            System.IO.FileInfo[] files = Dir.GetFiles(strFileName, SearchOption.AllDirectories);
            foreach (System.IO.FileInfo file in files)
            {
                blnOK = true;
            }

            if (blnOK == false)
            {
                return;
            }

            ComFunc.KillProc("hcscript.exe");
            hf.fn_Find_ProcessExecute(strPath, strFileName);
        }
    }
}
