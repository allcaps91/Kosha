using CefSharp;
using CefSharp.WinForms;
using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Service;
using System;
using System.Windows.Forms;
/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcIEMunjinVIew.cs
/// Description     : 종검수검자 인터넷 문진표 보기
/// Author          : 김민철
/// Create Date     : 2020-05-12
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm인터넷문진표_New(Frm인터넷문진표_New.frm)" />
namespace ComHpcLibB
{
    public partial class frmHcIEMunjinVIew : Form
    {
        ChromiumWebBrowser browser = null;

        ComHpcLibBService comHpcLibBService = null;
        HicJepsuService hicJepsuService = null;

        long FnMunID = 0;
        string FstrForm = string.Empty;
        string FstrPtno = string.Empty;

        public frmHcIEMunjinVIew()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcIEMunjinVIew(long nMunID, string argForm, string argPtno)
        {
            FnMunID = nMunID;
            FstrForm = argForm;
            FstrPtno = argPtno;

            InitializeComponent();
            SetControl();
            SetEvent();
            
            this.Text = "인터넷 문진표 (" + FnMunID.To<string>() + ")";
            if (FstrPtno != "")
            {
                this.btnRun.Enabled = true;
            }
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click  += new EventHandler(eBtnClick);
            this.btnSave.Click  += new EventHandler(eBtnClick);
            this.btnRun.Click += new EventHandler(eBtnClick);
            this.FormClosing += new FormClosingEventHandler(eFormClosing);
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            //처음부터 단일모니터가 아닐경우와
            if (clsHcVariable.singmon != 1)
            {
                //단일이면 1 아니면 (듀얼이면)...(메뉴에서 조정했을경우)
                if (clsHcVariable.selmon == 1)
                {
                    this.Left = 0;
                    this.Top = 0;
                }
                else
                {
                    this.Left = (int)clsHcVariable.slavecoodinate * 15;
                    this.Top = 0;
                }
            }

            string strYear = comHpcLibBService.GetIEMunjinDateByPtno(FstrPtno);

            if (string.Compare(VB.Left(strYear, 4), "2019") < 0)
            {
                browser = new ChromiumWebBrowser("http://www.pohangsmh.co.kr/web_question/general/g_view_2018.html?m_id=" + FnMunID.To<string>());
            }
            else
            {
                browser = new ChromiumWebBrowser("http://www.pohangsmh.co.kr/web_question/general/g_view.html?m_id=" + FnMunID.To<string>());
            }

            this.panMain.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;

            JsDialogHandler jss = new JsDialogHandler();
            browser.JsDialogHandler = jss;
        }

        private void eFormClosing(object sender, FormClosingEventArgs e)
        {
            //Cef.Shutdown();
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                //수정
                string strRowid = hicJepsuService.GetRowidByPtno(FstrPtno);

                if (strRowid != "")
                {
                    string strURL = "http://www.pohangsmh.co.kr/web_question/general/g_modify.html?m_id=" + FnMunID.To<string>();
                    this.browser.Load(strURL);
                }
                else
                {
                    MessageBox.Show("검진 당일만 문진표 수정이 가능합니다.", "에러");
                }
            }
            else if (sender == btnRun)
            {
                if (FstrForm.IsNullOrEmpty())
                {
                    return;
                }

                int result = comHpcLibBService.InsertHicIEMunjinSendReq(FstrPtno, FstrForm , FnMunID);
                if (result <= 0)
                {
                    MessageBox.Show("저장실패");
                }
                else
                {
                    Cef.Shutdown();
                    this.Close();
                    return;
                }
            }
        }

        private void SetControl()
        {
            comHpcLibBService = new ComHpcLibBService();
            hicJepsuService = new HicJepsuService();

            //Cef.Initialize(new CefSettings());

            if (!Cef.IsInitialized)
            {
                //localstorage 재사용가능해짐..
                CefSettings settings = new CefSettings()
                {
                    //CachePath = "cache",
                };

                settings.CefCommandLineArgs.Add("disable-web-security", "true"); //google 달력 cors 문제해결


                Cef.Initialize(settings);
            }

            btnRun.Enabled = false;
        }
    }
}
