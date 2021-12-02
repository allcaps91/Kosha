using CefSharp;
using CefSharp.WinForms;
using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public partial class frmHcNhic : Form, MainFormMessage
    {
        #region Variable Define
        ChromiumWebBrowser browser = null;
        TextBox txtbox = null;

        WorkNhicService workNhicService = null;
        HicJepsuPatientService HicJepsuPatientService = null;
        HicResultService hicResultService = null;
        HicJepsuService hicJepsuService = null;

        WORK_NHIC var = null;
        clsSpread cSpd = null;

        string FstrHtml = string.Empty;
        string FstrQuery = string.Empty;
        string FstrJob = "조회";

        long FnWRTNO = 0;
        string FstrJepDate  = string.Empty;        
        string FstrJumin    = string.Empty;
        string FstrSName    = string.Empty;
        string FstrChul     = string.Empty;
        string FstrFirst    = string.Empty;
        string FstrDent     = string.Empty;
        string FstrSecond   = string.Empty;
        string FstrStomach1 = string.Empty;
        string FstrStomach2 = string.Empty;
        string FstrColon1   = string.Empty;
        string FstrColon2   = string.Empty;
        string FstrColon3   = string.Empty;
        string FstrBreast   = string.Empty;
        string FstrWomb     = string.Empty;
        string FstrLiver1   = string.Empty;
        string FstrLiver2   = string.Empty;
        string FstrLungs    = string.Empty;
        string FstrYear     = string.Empty;
        #endregion

        string mPara1 = "";

        #region //MainFormMessage

        public MainFormMessage mCallForm = null;

        public void MsgActivedForm(Form frm)
        {
        }
        public void MsgUnloadForm(Form frm)
        {
        }
        public void MsgFormClear()
        {
        }
        public void MsgSendPara(string strPara)
        {
        }
        #endregion //MainFormMessage

        public frmHcNhic()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcNhic(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            SetControl();
            SetEvent();
        }

        public frmHcNhic(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
            SetControl();
            SetEvent();
        }

        private void SetEvent()
        {
            this.Load               += new EventHandler(eFormLoad);
            this.FormClosing        += new FormClosingEventHandler(eFormClosing);
            this.FormClosed         += new FormClosedEventHandler(eFormClosed);

            this.btnView.Click      += new EventHandler(eBtnClick);
            this.btnSearch.Click    += new EventHandler(eBtnClick);
            this.btnStart.Click     += new EventHandler(eBtnClick);
            this.btnStop.Click      += new EventHandler(eBtnClick);
            this.btnExit.Click      += new EventHandler(eBtnClick);

            this.timer1.Tick        += new EventHandler(eTimerTick1);
            this.timer3.Tick        += new EventHandler(eTimerTick3);
            this.timer4.Tick        += new EventHandler(eTimerTick4);
            this.timer5.Tick        += new EventHandler(eTimerTick5);

            this.rdoJob1.CheckedChanged += new EventHandler(eRdoChanged);
            this.rdoJob2.CheckedChanged += new EventHandler(eRdoChanged);

        }

        private void eRdoChanged(object sender, EventArgs e)
        {
            if (sender == rdoJob1)
            {
                if (rdoJob1.Checked)
                {
                    Work_Stop();
                    btnSearch.Visible = false;
                    panDate.Visible = false;
                    this.collapsibleSplitContainer1.Panel1Collapsed = true;
                    btnStart.Text = "조회시작";
                    btnStop.Text = "조회중지";
                    FstrJob = "조회";
                }
            }
            else if (sender == rdoJob2)
            {
                if (rdoJob2.Checked)
                {
                    Work_Stop();
                    btnSearch.Visible = true;
                    panDate.Visible = true;
                    this.collapsibleSplitContainer1.Panel1Collapsed = false;
                    btnStart.Text = "작업시작";
                    btnStop.Text = "작업중지";
                    FstrJob = "등록";
                }
            }

            browser.Load("http://sis.nhis.or.kr/ggob011_r01.do?reqUrl=ggob011m01");
        }

        private void eTimerTick1(object sender, EventArgs e)
        {
            string strOK = string.Empty;

            txtbox.Text = "";

            timer1.Enabled = false;

            FstrHtml = GetHTMLFromWebBrowser();
            FstrHtml = VB.Replace(FstrHtml, VB.Chr(34).To<string>(), "");

            if (VB.InStr(FstrHtml, "오류메세지:오류코드가 -3인 대상자는") > 0 || VB.InStr(FstrHtml, "오류코드:-3") > 0)
            {
                timer1.Enabled = false;
                timer3.Enabled = false;
                timer4.Enabled = false;
                Work_Start();
                lblMsg.Text = "Timer1: 오류코드가 -3";
                Application.DoEvents();
                Thread.Sleep(2000);
                return;
            }

            lblMsg.Text = DateTime.Now.ToShortTimeString() + " Timer1 Start";
            Application.DoEvents();

            // 대기자 1건씩 읽어 자격조회 함
            WORK_NHIC item = workNhicService.GetOneItemByNewData("H");
            if (item.IsNullOrEmpty())
            {
                //증번호, 지사기호, 자격구분 누락 Data UpDate 로직은 제외함
                #region 기존 원본 소스
                //If chkGkiho.Value = True Then
                //    SQL = "   SELECT  JUMIN,JUMIN2, SNAME,Year, Rowid  "
                //    SQL = SQL & "  FROM  WORK_NHIC "
                //    SQL = SQL & "WHERE RTime>=TRUNC(SYSDATE-3) "
                //    SQL = SQL & "  AND GKiho IS NULL "
                //    SQL = SQL & "  AND GBSTS  ='1' "
                //    'SQL = SQL & "  AND SName='이국희' AND CTime IS NOT NULL AND GbSTS='1' "
                //    SQL = SQL & "  AND ROWNUM = 1 "
                //    Result = AdoOpenSet(Rs, SQL)
                //End If 
                #endregion    
            }

            var = new WORK_NHIC();

            if (!item.IsNullOrEmpty())
            {
                var.RID = item.RID;
                var.JUMIN = clsAES.DeAES(item.JUMIN2);
                var.SNAME = item.SNAME;
                if (item.YEAR.IsNullOrEmpty())
                {
                    var.YEAR = cboYear.Text.Trim();
                }
                else
                {
                    var.YEAR = item.YEAR;
                }

                if (var.JUMIN == "" || var.SNAME == "")
                {
                    //자격조회 대상 Data 오류로 자격조회 실패처리 
                    workNhicService.UpDateError(var.RID);
                    FstrQuery = "1";
                    timer1.Enabled = true;
                    timer3.Enabled = false;
                    timer4.Enabled = false;
                }
                else
                {
                    strOK = "";
                    for (int i = 0; i < 10; i++)
                    {
                        if (SetInputHTML(var.JUMIN, var.SNAME, var.YEAR)) { strOK = "OK"; break; }
                        Thread.Sleep(500);
                        Application.DoEvents();
                    }

                    if (strOK == "OK")
                    {
                        timer1.Enabled = false;
                        timer3.Enabled = true;
                        timer4.Enabled = false;
                    }
                    else
                    {
                        //웹페이지 컨트롤 에러로 자격조회 실패처리
                        workNhicService.UpDateError(var.RID);
                        FstrQuery = "1";
                        timer1.Enabled = true;
                        timer3.Enabled = false;
                        timer4.Enabled = false;
                    }
                }
            }
            else
            {
                FstrQuery = "1";
                timer1.Enabled = true;
                timer3.Enabled = false;
                timer4.Enabled = false;
            }

            lblMsg.Text = DateTime.Now.ToShortTimeString() + " Timer1 End";
            Application.DoEvents();
        }

        private void eTimerTick3(object sender, EventArgs e)
        {
            string strJumin = string.Empty;

            timer3.Enabled = false;

            lblMsg.Text = DateTime.Now.ToShortTimeString() + " Timer3 Start";
            Application.DoEvents();

            FstrHtml = GetHTMLFromWebBrowser();
            FstrHtml = VB.Replace(FstrHtml, VB.Chr(34).To<string>(), "");

            strJumin = VB.Pstr(FstrHtml, "<th scope=row class=first>*주민등록번호</th>", 1);
            strJumin = VB.Pstr(strJumin, "</td>", 3);
            strJumin = VB.Pstr(strJumin, "value=", 2);
            strJumin = strJumin.Replace(">", "").Trim();

            if (strJumin.IsNullOrEmpty())
            {
                timer1.Enabled = false;
                timer3.Enabled = false;
                timer3.Enabled = false;

                //자격조회 대상으로 변경
                workNhicService.UpdateNewTarget(var.RID);

                Thread.Sleep(500);

                FstrQuery = "1";
                timer1.Enabled = true;
                timer3.Enabled = false;
                timer4.Enabled = false;

                return;
            }

            strJumin = "";

            FstrQuery = "4";
            browser.ExecuteScriptAsync("f_inquiry()");

            Thread.Sleep(300);

            timer1.Enabled = false;
            timer3.Enabled = false;
            timer4.Enabled = true;
        }

        private void eTimerTick4(object sender, EventArgs e)
        {
            timer4.Enabled = false;

            lblMsg.Text = DateTime.Now.ToShortTimeString() + ":Timer4 Start";
            Application.DoEvents();

            //Thread.Sleep(2000);

            HTML_Parse_HIC_NEW();

            lblMsg.Text = DateTime.Now.ToShortTimeString() + ":HTML_Parse End";
            Application.DoEvents();

            timer1.Enabled = true;
            timer3.Enabled = false;
            timer4.Enabled = false;
        }

        private void eTimerTick5(object sender, EventArgs e)
        {
            int nRow = 0;

            if (SS1.ActiveSheet.RowCount == 0)
            {
                Work_Stop();
                MessageBox.Show("작업건수가 없습니다.", "Data 필요");
                return;
            }

            timer5.Enabled = false;

            lblMsg.Text = DateTime.Now.ToShortTimeString() + " Timer5 Start";

            FstrHtml = GetHTMLFromWebBrowser();
            FstrHtml = VB.Replace(FstrHtml, VB.Chr(34).To<string>(), "");

            if (VB.InStr(FstrHtml, "오류메세지:오류코드가 -3인 대상자는") > 0 || VB.InStr(FstrHtml, "오류코드:-3") > 0)
            {
                timer5.Enabled = false;
                Work_Start();
                lblMsg.Text = "Timer5: 오류코드가 -3";
                Application.DoEvents();
                Thread.Sleep(2000);
                return;
            }

            nRow = SS1.ActiveSheet.RowCount;

            for (int i = 0; i < nRow; i++)
            {
                if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    //검진내역 읽기
                    FnWRTNO      = SS1.ActiveSheet.Cells[i, 1].Text.To<long>();
                    FstrJepDate  = SS1.ActiveSheet.Cells[i, 2].Text;
                    FstrSName    = SS1.ActiveSheet.Cells[i, 4].Text;
                    FstrJumin    = SS1.ActiveSheet.Cells[i, 5].Text;
                    FstrChul     = SS1.ActiveSheet.Cells[i, 6].Text;
                    FstrFirst    = SS1.ActiveSheet.Cells[i, 7].Text;
                    FstrDent     = SS1.ActiveSheet.Cells[i, 8].Text;
                    FstrSecond   = SS1.ActiveSheet.Cells[i, 9].Text;
                    FstrStomach1 = SS1.ActiveSheet.Cells[i, 10].Text;
                    FstrStomach2 = SS1.ActiveSheet.Cells[i, 11].Text;
                    FstrColon1   = SS1.ActiveSheet.Cells[i, 12].Text;
                    FstrColon2   = SS1.ActiveSheet.Cells[i, 13].Text;
                    FstrColon3   = SS1.ActiveSheet.Cells[i, 14].Text;
                    FstrBreast   = SS1.ActiveSheet.Cells[i, 15].Text;
                    FstrWomb     = SS1.ActiveSheet.Cells[i, 16].Text;
                    FstrLiver1   = SS1.ActiveSheet.Cells[i, 17].Text;
                    FstrLiver2   = SS1.ActiveSheet.Cells[i, 18].Text;
                    FstrLungs    = SS1.ActiveSheet.Cells[i, 19].Text;
                    FstrYear     = SS1.ActiveSheet.Cells[i, 20].Text;

                    if (!eTimerTick6())
                    {
                        SS1.ActiveSheet.Cells[i, 21].Text = "에러";
                    }       
                }
            }
        }

        private bool eTimerTick6()
        {
            string strOK = string.Empty;
            
            lblMsg.Text = DateTime.Now.ToShortTimeString() + " Timer6 Start";

            for (int i = 0; i < 10; i++)
            {
                if (SetInputHTML(FstrJumin, FstrSName, FstrYear)) { strOK = "OK"; break; }
                Thread.Sleep(500);
                Application.DoEvents();
            }

            lblMsg.Text = DateTime.Now.ToShortTimeString() + " Timer6 End";

            return eTimerTick7();
        }

        private bool eTimerTick7()
        {
            string strJumin = string.Empty;
            string strTemp = string.Empty;
            string strSName = string.Empty;
            string strOK = string.Empty;

            lblMsg.Text = DateTime.Now.ToShortTimeString() + " Timer7 Start";

            FstrHtml = GetHTMLFromWebBrowser();
            FstrHtml = VB.Replace(FstrHtml, VB.Chr(34).To<string>(), "");

            strJumin = VB.Pstr(FstrHtml, "<th scope=row class=first>*주민등록번호</th>", 1);
            strJumin = VB.Pstr(strJumin, "</td>", 3);
            strJumin = VB.Pstr(strJumin, "value=", 2);
            strJumin = strJumin.Replace(">", "").Trim();

            if (strJumin.IsNullOrEmpty())
            {
                return false;
            }

            strJumin = "";

            browser.ExecuteScriptAsync("f_inquiry()");

            Thread.Sleep(300);

            Application.DoEvents();

            lblMsg.Text = DateTime.Now.ToShortTimeString() + " Timer7 End";

            //정상적으로 조회가 된 상태인지 확인
            FstrHtml = GetHTMLFromWebBrowser();
            FstrHtml = VB.Replace(FstrHtml, VB.Chr(34).To<string>(), "");
            strTemp = VB.Pstr(FstrHtml, "<div id=DivExmdobGubun1", 2);
            strTemp = VB.Pstr(strTemp, "<th scope=row class=first>성명 </th>", 2);
            strTemp = VB.Pstr(strTemp, "<td class=left>", 2);
            strTemp = VB.Pstr(strTemp, "</td>", 1);
            if (strTemp == "") { strOK = "NO"; }

            //성명
            //<th scope=row class=first>성명 </th>
            strSName = VB.Pstr(FstrHtml, "<th scope=row class=first>성명 </th>", 2);
            strSName = VB.Pstr(strSName, "<td class=left>", 2);
            strSName = VB.Pstr(strSName, "</td>", 1);
            if (strSName != FstrSName) { strOK = "NO"; }

            if (strOK == "NO")
            {
                return false;
            }

            return eTimerTick8();
        }

        private bool eTimerTick8()
        {
            bool bSetup = false;

            lblMsg.Text = DateTime.Now.ToShortTimeString() + " Timer8 Start";

            //1차수검
            if (FstrFirst != "")
            {
                bSetup = true;
                browser.ExecuteScriptAsync("document.getElementById('CHECK1').checked= true");
                browser.ExecuteScriptAsync("document.getElementById('HCHK_DT1').value= '" + FstrJepDate + "'");
                if (FstrChul == "Y")
                {
                    string strChul = FstrChul == "Y" ? "true" : "false";
                    browser.ExecuteScriptAsync("document.getElementById('EXMD_PLC_TYPE1').checked= " + strChul + " ");
                }
                browser.ExecuteScriptAsync("f_clickHmeCheck(this,'HCHK_DT1','READ_ONLY_YN1')");
                Thread.Sleep(300);
            }

            //2차수검 (등록안함)
            if (FstrSecond != "")
            {
                //2차는 수검등록안함
                //browser.ExecuteScriptAsync("document.getElementById('CHECK2').checked= true");
                //browser.ExecuteScriptAsync("document.getElementById('HCHK_DT2').value= '" + FstrJepDate + "'");
                //if (FstrChul == "Y")
                //{
                //    string strChul = FstrChul == "Y" ? "true" : "false";
                //    browser.ExecuteScriptAsync("document.getElementById('EXMD_PLC_TYPE2').checked= " + strChul + " ");
                //}
                //browser.ExecuteScriptAsync("f_clickHmeCheck(this,'HCHK_DT2','READ_ONLY_YN2')");
                //Thread.Sleep(100);
            }

            //구강
            if (FstrDent != "")
            {
                bSetup = true;
                browser.ExecuteScriptAsync("document.getElementById('CHECK3').checked= true");
                browser.ExecuteScriptAsync("document.getElementById('HCHK_DT3').value= '" + FstrJepDate + "'");
                if (FstrChul == "Y")
                {
                    string strChul = FstrChul == "Y" ? "true" : "false";
                    browser.ExecuteScriptAsync("document.getElementById('EXMD_PLC_TYPE3').checked= " + strChul + " ");
                }
                browser.ExecuteScriptAsync("f_clickHmeCheck(this,'HCHK_DT3','READ_ONLY_YN3')");
                Thread.Sleep(300);
            }

            //위장조영
            if (FstrStomach1 != "")
            {
                bSetup = true;
                browser.ExecuteScriptAsync("document.getElementById('CHECK4').checked= true");
                browser.ExecuteScriptAsync("document.getElementById('HCHK_DT4').value= '" + FstrJepDate + "'");
                if (FstrChul == "Y")
                {
                    string strChul = FstrChul == "Y" ? "true" : "false";
                    browser.ExecuteScriptAsync("document.getElementById('EXMD_PLC_TYPE4').checked= " + strChul + " ");
                }
                browser.ExecuteScriptAsync("f_clickHmeCheck(this,'HCHK_DT4','READ_ONLY_YN4')");
                Thread.Sleep(300);
            }

            //위장내시경
            if (FstrStomach2 != "")
            {
                bSetup = true;
                browser.ExecuteScriptAsync("document.getElementById('CHECK15').checked= true");
                browser.ExecuteScriptAsync("document.getElementById('HCHK_DT15').value= '" + FstrJepDate + "'");
                if (FstrChul == "Y")
                {
                    string strChul = FstrChul == "Y" ? "true" : "false";
                    browser.ExecuteScriptAsync("document.getElementById('EXMD_PLC_TYPE15').checked= " + strChul + " ");
                }
                browser.ExecuteScriptAsync("f_clickHmeCheck(this,'HCHK_DT15','READ_ONLY_YN15')");
                Thread.Sleep(300);
            }

            //대장잠혈
            if (FstrColon1 != "")
            {
                bSetup = true;
                browser.ExecuteScriptAsync("document.getElementById('CHECK5').checked= true");
                browser.ExecuteScriptAsync("document.getElementById('HCHK_DT5').value= '" + FstrJepDate + "'");
                if (FstrChul == "Y")
                {
                    string strChul = FstrChul == "Y" ? "true" : "false";
                    browser.ExecuteScriptAsync("document.getElementById('EXMD_PLC_TYPE5').checked= " + strChul + " ");
                }
                browser.ExecuteScriptAsync("f_clickHmeCheck(this,'HCHK_DT5','READ_ONLY_YN5')");
                Thread.Sleep(300);
            }

            //대장조영
            if (FstrColon2 != "")
            {
                bSetup = true;
                browser.ExecuteScriptAsync("document.getElementById('CHECK16').checked= true");
                browser.ExecuteScriptAsync("document.getElementById('HCHK_DT16').value= '" + FstrJepDate + "'");
                if (FstrChul == "Y")
                {
                    string strChul = FstrChul == "Y" ? "true" : "false";
                    browser.ExecuteScriptAsync("document.getElementById('EXMD_PLC_TYPE16').checked= " + strChul + " ");
                }
                browser.ExecuteScriptAsync("f_clickHmeCheck(this,'HCHK_DT16','READ_ONLY_YN16')");
                Thread.Sleep(300);
            }

            //대장내시경
            if (FstrColon3 != "")
            {
                bSetup = true;
                browser.ExecuteScriptAsync("document.getElementById('CHECK17').checked= true");
                browser.ExecuteScriptAsync("document.getElementById('HCHK_DT17').value= '" + FstrJepDate + "'");
                if (FstrChul == "Y")
                {
                    string strChul = FstrChul == "Y" ? "true" : "false";
                    browser.ExecuteScriptAsync("document.getElementById('EXMD_PLC_TYPE17').checked= " + strChul + " ");
                }
                browser.ExecuteScriptAsync("f_clickHmeCheck(this,'HCHK_DT17','READ_ONLY_YN17')");
                Thread.Sleep(300);
            }

            //간암상반기
            if (FstrLiver1 != "")
            {
                bSetup = true;
                browser.ExecuteScriptAsync("document.getElementById('CHECK8').checked= true");
                browser.ExecuteScriptAsync("document.getElementById('HCHK_DT8').value= '" + FstrJepDate + "'");
                if (FstrChul == "Y")
                {
                    string strChul = FstrChul == "Y" ? "true" : "false";
                    browser.ExecuteScriptAsync("document.getElementById('EXMD_PLC_TYPE8').checked= " + strChul + " ");
                }
                browser.ExecuteScriptAsync("f_clickHmeCheck(this,'HCHK_DT8','READ_ONLY_YN8')");
                Thread.Sleep(300);
            }

            //간암하반기
            if (FstrLiver2 != "")
            {
                bSetup = true;
                browser.ExecuteScriptAsync("document.getElementById('CHECK11').checked= true");
                browser.ExecuteScriptAsync("document.getElementById('HCHK_DT11').value= '" + FstrJepDate + "'");
                if (FstrChul == "Y")
                {
                    string strChul = FstrChul == "Y" ? "true" : "false";
                    browser.ExecuteScriptAsync("document.getElementById('EXMD_PLC_TYPE11').checked= " + strChul + " ");
                }
                browser.ExecuteScriptAsync("f_clickHmeCheck(this,'HCHK_DT11','READ_ONLY_YN11')");
                Thread.Sleep(300);
            }

            //유방암
            if (FstrBreast != "")
            {
                bSetup = true;
                browser.ExecuteScriptAsync("document.getElementById('CHECK6').checked= true");
                browser.ExecuteScriptAsync("document.getElementById('HCHK_DT6').value= '" + FstrJepDate + "'");
                if (FstrChul == "Y")
                {
                    string strChul = FstrChul == "Y" ? "true" : "false";
                    browser.ExecuteScriptAsync("document.getElementById('EXMD_PLC_TYPE6').checked= " + strChul + " ");
                }
                browser.ExecuteScriptAsync("f_clickHmeCheck(this,'HCHK_DT6','READ_ONLY_YN6')");
                Thread.Sleep(300);
            }

            //자궁경부암
            if (FstrWomb != "")
            {
                bSetup = true;
                browser.ExecuteScriptAsync("document.getElementById('CHECK7').checked= true");
                browser.ExecuteScriptAsync("document.getElementById('HCHK_DT7').value= '" + FstrJepDate + "'");
                if (FstrChul == "Y")
                {
                    string strChul = FstrChul == "Y" ? "true" : "false";
                    browser.ExecuteScriptAsync("document.getElementById('EXMD_PLC_TYPE7').checked= " + strChul + " ");
                }
                browser.ExecuteScriptAsync("f_clickHmeCheck(this,'HCHK_DT7','READ_ONLY_YN7')");
                Thread.Sleep(300);
            }

            //폐암
            if (FstrLungs != "")
            {
                bSetup = true;
                browser.ExecuteScriptAsync("document.getElementById('CHECK9').checked= true");
                browser.ExecuteScriptAsync("document.getElementById('HCHK_DT9').value= '" + FstrJepDate + "'");
                if (FstrChul == "Y")
                {
                    string strChul = FstrChul == "Y" ? "true" : "false";
                    browser.ExecuteScriptAsync("document.getElementById('EXMD_PLC_TYPE9').checked= " + strChul + " ");
                }
                browser.ExecuteScriptAsync("f_clickHmeCheck(this,'HCHK_DT9','READ_ONLY_YN9')");
                Thread.Sleep(300);
            }

            Application.DoEvents();

            if (bSetup)
            {
                browser.ExecuteScriptAsync("f_save()");
                Thread.Sleep(1000);
            }

            Application.DoEvents();

            lblMsg.Text = DateTime.Now.ToShortTimeString() + " Timer8 End";

            return eTimerTick9();

        }

        private bool eTimerTick9()
        {
            string strJumin = string.Empty;
            string strYear = string.Empty;
            string strSName = string.Empty;
            string strOK = string.Empty;
            string strChk1 = string.Empty;
            string strChk2 = string.Empty;
            string strChk3 = string.Empty;
            string strTemp = string.Empty;

            lblMsg.Text = DateTime.Now.ToShortTimeString() + " Timer9 Start";

            browser.ExecuteScriptAsync("f_inquiry()");

            Thread.Sleep(1000);

            //정상적으로 조회가 된 상태인지 확인
            FstrHtml = GetHTMLFromWebBrowser();
            FstrHtml = VB.Replace(FstrHtml, VB.Chr(34).To<string>(), "");
            strTemp = VB.Pstr(FstrHtml, "<div id=DivExmdobGubun1", 2);
            strTemp = VB.Pstr(strTemp, "<th scope=row class=first>성명 </th>", 2);
            strTemp = VB.Pstr(strTemp, "<td class=left>", 2);
            strTemp = VB.Pstr(strTemp, "</td>", 1);

            if (strTemp == "")
            {
                return false;
            }

            strOK = "OK";

            FstrHtml = GetHTMLFromWebBrowser();
            FstrHtml = VB.Replace(FstrHtml, VB.Chr(34).To<string>(), "");
            //<label for="mtm1" class="hide">*주민등록번호</label>
            strJumin = VB.Pstr(FstrHtml, "<label for=mtm1 class=hide>*주민등록번호</label>", 2);
            strJumin = VB.Pstr(strJumin, "</td>", 1);
            strJumin = VB.Pstr(strJumin, "name=S_JUMIN_NO", 3);
            strJumin = VB.Pstr(strJumin, "value=", 2);
            strJumin = VB.Pstr(strJumin, "onkeyup=", 1).Trim();

            //사업년도
            strYear = VB.Pstr(FstrHtml, "<th scope=row class=first>사업년도</th>", 2);
            strYear = VB.Pstr(strYear, "<td class=left>", 2);
            strYear = VB.Pstr(strYear, "</td>", 1);
            
            if (strYear != FstrYear) { strOK = "NO"; }

            //성명
            if (strOK == "OK")
            {
                //<th scope=row class=first>성명 </th>
                strSName = VB.Pstr(FstrHtml, "<th scope=row class=first>성명 </th>", 2);
                strSName = VB.Pstr(strSName, "<td class=left>", 2);
                strSName = VB.Pstr(strSName, "</td>", 1);
                if (strSName != FstrSName) { strOK = "NO"; }
            }

            //조회를 하지 못하였으면 완료처리 안하고 다음 수검자 처리함
            if (strOK == "NO" || strJumin.IsNullOrEmpty()) { return false; }
            
            //1차검진
            if (strOK == "OK" && FstrFirst != "")
            {
                //1차진단
                strChk1 = VB.Pstr(FstrHtml, "id=HCHK_DT1", 2);
                strChk1 = VB.Pstr(strChk1, "</td>", 1);
                strChk1 = VB.Left(VB.Pstr(strChk1, "value=", 2), 10);

                //1차 검진기관
                strChk3 = VB.Pstr(FstrHtml, "name=EXMD_PLC_TYPE1", 2);
                strChk3 = VB.Pstr(strChk3, "</tr>", 1);
                strChk3 = VB.Pstr(strChk3, "<td headers=me1 rs1>", 2);
                strChk3 = VB.Pstr(strChk3, "</td>", 1);

                if (strChk1 != FstrJepDate || strChk3 != "포항성모병원") { strOK = "NO"; }
            }

            //2차검진
            if (strOK == "OK" && FstrSecond != "")
            {
                //2차진단
                strChk1 = VB.Pstr(FstrHtml, "id=HCHK_DT2", 2);
                strChk1 = VB.Pstr(strChk1, "</td>", 1);
                strChk1 = VB.Left(VB.Pstr(strChk1, "value=", 2), 10);

                //2차 검진기관
                strChk3 = VB.Pstr(FstrHtml, "name=EXMD_PLC_TYPE2", 2);
                strChk3 = VB.Pstr(strChk3, "</tr>", 1);
                strChk3 = VB.Pstr(strChk3, "<TD headers=me1 rs1>", 2);
                strChk3 = VB.Pstr(strChk3, "</td>", 1);

                if (strChk3 != FstrJepDate || strChk3 != "포항성모병원") { strOK = "NO"; }
            }

            //구강검진
            if (strOK == "OK" && FstrDent != "")
            {
                //구강검사
                strChk1 = VB.Pstr(FstrHtml, "id=HCHK_DT3", 2);
                strChk1 = VB.Pstr(strChk1, "</td>", 1);
                strChk1 = VB.Left(VB.Pstr(strChk1, "value=", 2), 10);

                //구강검사 검진기관
                strChk3 = VB.Pstr(FstrHtml, "name=EXMD_PLC_TYPE3", 2);
                strChk3 = VB.Pstr(strChk3, "</tr>", 1);
                strChk3 = VB.Pstr(strChk3, "<td headers=me1 rs1>", 2);
                strChk3 = VB.Pstr(strChk3, "</td>", 1);

                if (strChk1 != FstrJepDate || strChk3 != "포항성모병원") { strOK = "NO"; }
            }

            //위암검진(조영)
            if (strOK == "OK" && FstrStomach1 != "")
            {
                //위암검사
                strChk1 = VB.Pstr(FstrHtml, "id=HCHK_DT4", 2);
                strChk1 = VB.Pstr(strChk1, "</td>", 1);
                strChk1 = VB.Left(VB.Pstr(strChk1, "value=", 2), 10);

                //위암검사 검진기관
                strChk3 = VB.Pstr(FstrHtml, "name=EXMD_PLC_TYPE4", 2);
                strChk3 = VB.Pstr(strChk3, "</tr>", 1);
                strChk3 = VB.Pstr(strChk3, "<TD headers=me1 rs1>", 2);
                strChk3 = VB.Pstr(strChk3, "</td>", 1);

                if (strChk1 != FstrJepDate || strChk3 != "포항성모병원") { strOK = "NO"; }
            }

            //위암검진(내시경)
            if (strOK == "OK" && FstrStomach2 != "")
            {
                //위암검사
                strChk1 = VB.Pstr(FstrHtml, "id=HCHK_DT15", 2);
                strChk1 = VB.Pstr(strChk1, "</td>", 1);
                strChk1 = VB.Left(VB.Pstr(strChk1, "value=", 2), 10);

                //위암검사 검진기관
                strChk3 = VB.Pstr(FstrHtml, "name=EXMD_PLC_TYPE15", 2);
                strChk3 = VB.Pstr(strChk3, "</tr>", 1);
                strChk3 = VB.Pstr(strChk3, "<TD headers=me1 rs1>", 2);
                strChk3 = VB.Pstr(strChk3, "</td>", 1);

                if (strChk1 != FstrJepDate || strChk3 != "포항성모병원") { strOK = "NO"; }
            }

            //대장검진(잠혈)
            if (strOK == "OK" && FstrColon1 != "")
            {
                //대장검사(잠혈)
                strChk1 = VB.Pstr(FstrHtml, "id=HCHK_DT5", 2);
                strChk1 = VB.Pstr(strChk1, "</td>", 1);
                strChk1 = VB.Left(VB.Pstr(strChk1, "value=", 2), 10);

                //대장검사(잠혈) 검진기관
                strChk3 = VB.Pstr(FstrHtml, "name=EXMD_PLC_TYPE5", 2);
                strChk3 = VB.Pstr(strChk3, "</tr>", 1);
                strChk3 = VB.Pstr(strChk3, "<TD headers=me1 rs1>", 2);
                strChk3 = VB.Pstr(strChk3, "</td>", 1);

                if (strChk1 != FstrJepDate || strChk3 != "포항성모병원") { strOK = "NO"; }
            }

            //대장검진(조영)
            if (strOK == "OK" && FstrColon2 != "")
            {
                //대장검사
                strChk1 = VB.Pstr(FstrHtml, "id=HCHK_DT16", 2);
                strChk1 = VB.Pstr(strChk1, "</td>", 1);
                strChk1 = VB.Left(VB.Pstr(strChk1, "value=", 2), 10);

                //대장검사 검진기관
                strChk3 = VB.Pstr(FstrHtml, "name=EXMD_PLC_TYPE16", 2);
                strChk3 = VB.Pstr(strChk3, "</tr>", 1);
                strChk3 = VB.Pstr(strChk3, "<TD headers=me1 rs1>", 2);
                strChk3 = VB.Pstr(strChk3, "</td>", 1);

                if (strChk1 != FstrJepDate || strChk3 != "포항성모병원") { strOK = "NO"; }
            }

            //대장검진(내시경)
            if (strOK == "OK" && FstrColon3 != "")
            {
                //대장검사
                strChk1 = VB.Pstr(FstrHtml, "id=HCHK_DT17", 2);
                strChk1 = VB.Pstr(strChk1, "</td>", 1);
                strChk1 = VB.Left(VB.Pstr(strChk1, "value=", 2), 10);

                //대장검사 검진기관
                strChk3 = VB.Pstr(FstrHtml, "name=EXMD_PLC_TYPE17", 2);
                strChk3 = VB.Pstr(strChk3, "</tr>", 1);
                strChk3 = VB.Pstr(strChk3, "<TD headers=me1 rs1>", 2);
                strChk3 = VB.Pstr(strChk3, "</td>", 1);

                if (strChk1 != FstrJepDate || strChk3 != "포항성모병원") { strOK = "NO"; }
            }

            //유방검진
            if (strOK == "OK" && FstrBreast != "")
            {
                //유방암검사
                strChk1 = VB.Pstr(FstrHtml, "id=HCHK_DT6", 2);
                strChk1 = VB.Pstr(strChk1, "</td>", 1);
                strChk1 = VB.Left(VB.Pstr(strChk1, "value=", 2), 10);

                //유방암검사 검진기관
                strChk3 = VB.Pstr(FstrHtml, "name=EXMD_PLC_TYPE6", 2);
                strChk3 = VB.Pstr(strChk3, "</tr>", 1);
                strChk3 = VB.Pstr(strChk3, "<TD headers=me1 rs1>", 2);
                strChk3 = VB.Pstr(strChk3, "</td>", 1);

                if (strChk1 != FstrJepDate || strChk3 != "포항성모병원") { strOK = "NO"; }
            }

            //자궁경부검진
            if (strOK == "OK" && FstrWomb != "")
            {
                //자궁경부암검사
                strChk1 = VB.Pstr(FstrHtml, "id=HCHK_DT7", 2);
                strChk1 = VB.Pstr(strChk1, "</td>", 1);
                strChk1 = VB.Left(VB.Pstr(strChk1, "value=", 2), 10);

                //자궁경부암검사 검진기관
                strChk3 = VB.Pstr(FstrHtml, "name=EXMD_PLC_TYPE7", 2);
                strChk3 = VB.Pstr(strChk3, "</tr>", 1);
                strChk3 = VB.Pstr(strChk3, "<TD headers=me1 rs1>", 2);
                strChk3 = VB.Pstr(strChk3, "</td>", 1);

                if (strChk1 != FstrJepDate || strChk3 != "포항성모병원") { strOK = "NO"; }
            }

            //간암검진(상반기)
            if (strOK == "OK" && FstrLiver1 != "")
            {
                //간암검진
                strChk1 = VB.Pstr(FstrHtml, "id=HCHK_DT8", 2);
                strChk1 = VB.Pstr(strChk1, "</td>", 1);
                strChk1 = VB.Left(VB.Pstr(strChk1, "value=", 2), 10);

                //간암검진 검진기관
                strChk3 = VB.Pstr(FstrHtml, "name=EXMD_PLC_TYPE8", 2);
                strChk3 = VB.Pstr(strChk3, "</tr>", 1);
                strChk3 = VB.Pstr(strChk3, "<TD headers=me1 rs1>", 2);
                strChk3 = VB.Pstr(strChk3, "</td>", 1);

                if (strChk1 != FstrJepDate || strChk3 != "포항성모병원") { strOK = "NO"; }
            }

            //간암검진(후반기)
            if (strOK == "OK" && FstrLiver2 != "")
            {
                //간암검진
                strChk1 = VB.Pstr(FstrHtml, "id=HCHK_DT11", 2);
                strChk1 = VB.Pstr(strChk1, "</td>", 1);
                strChk1 = VB.Left(VB.Pstr(strChk1, "value=", 2), 10);

                //간암검진 검진기관
                strChk3 = VB.Pstr(FstrHtml, "name=EXMD_PLC_TYPE11", 2);
                strChk3 = VB.Pstr(strChk3, "</tr>", 1);
                strChk3 = VB.Pstr(strChk3, "<TD headers=me1 rs1>", 2);
                strChk3 = VB.Pstr(strChk3, "</td>", 1);

                if (strChk1 != FstrJepDate || strChk3 != "포항성모병원") { strOK = "NO"; }
            }

            //폐암검사
            if (strOK == "OK" && FstrLungs != "")
            {
                //폐암검사
                strChk1 = VB.Pstr(FstrHtml, "id=HCHK_DT9", 2);
                strChk1 = VB.Pstr(strChk1, "</td>", 1);
                strChk1 = VB.Left(VB.Pstr(strChk1, "value=", 2), 10);

                //폐암 검진기관
                strChk3 = VB.Pstr(FstrHtml, "name=EXMD_PLC_TYPE9", 2);
                strChk3 = VB.Pstr(strChk3, "</tr>", 1);
                strChk3 = VB.Pstr(strChk3, "<TD headers=me1 rs1>", 2);
                strChk3 = VB.Pstr(strChk3, "</td>", 1);

                if (strChk1 != FstrJepDate || strChk3 != "포항성모병원") { strOK = "NO"; }
            }

            Application.DoEvents();

            if (strOK == "OK")
            {
                int result = hicJepsuService.UpdateGbSujinbyWrtNo(FnWRTNO);
                if (result <= 0)
                {
                    lblMsg.Text = "수진완료 여부 등록에러";
                    return false;
                }
            }
            else
            {
                return false;
            }

            lblMsg.Text = DateTime.Now.ToShortTimeString() + " Timer9 End";

            return true;
        }

        private void HTML_Parse_HIC_NEW()
        {
            #region Define Variable
            string strPANO     = string.Empty;  string strJumin    = string.Empty;  string strSName = string.Empty; string strROWID = string.Empty;  string strRel = string.Empty;
            string strBI       = string.Empty;  string strkiho     = string.Empty;  string strGkiho = string.Empty; string strPName = string.Empty;  string strBDate = string.Empty; 
            string strJisa     = string.Empty;  string strLiver2   = string.Empty;  
            string strYear     = string.Empty;  string strTrans    = string.Empty;  string strFirst = string.Empty; string strSecond = string.Empty; string strEKG = string.Empty;
            string strLiver    = string.Empty;  string strLiverC   = string.Empty;            
            string strDental   = string.Empty;  string str1차Add   = string.Empty;  string str2차Add = string.Empty;
            string strCancer11 = string.Empty;  string strCancer12 = string.Empty;
            string strCancer21 = string.Empty;  string strCancer22 = string.Empty;
            string strCancer31 = string.Empty;  string strCancer32 = string.Empty;
            string strCancer41 = string.Empty;  string strCancer42 = string.Empty;
            string strCancer51 = string.Empty;  string strCancer52 = string.Empty;
            string strCancer53 = string.Empty;                  
            string strCancer61 = string.Empty;  string strCancer62 = string.Empty;
            string strCancer71 = string.Empty;  string strCancer72 = string.Empty;

            //1차검진 일자,출장,기관
            string strChk01_1 = string.Empty;
            string strChk01_2 = string.Empty;
            string strChk01_3 = string.Empty;
            //2차검진
            string strChk02_1 = string.Empty;
            string strChk02_2 = string.Empty;
            string strChk02_3 = string.Empty;
            //구강
            string strChk03_1 = string.Empty;
            string strChk03_2 = string.Empty;
            string strChk03_3 = string.Empty;

            //위암(조영)
            string strChk04_1 = string.Empty;
            string strChk04_2 = string.Empty;
            string strChk04_3 = string.Empty;
            //위암(내시경)
            string strChk15_1 = string.Empty;
            string strChk15_2 = string.Empty;
            string strChk15_3 = string.Empty;

            //대장암(잠혈)
            string strChk05_1 = string.Empty;
            string strChk05_2 = string.Empty;
            string strChk05_3 = string.Empty;
            //대장암(조영)
            string strChk16_1 = string.Empty;
            string strChk16_2 = string.Empty;
            string strChk16_3 = string.Empty;
            //대장(내시경)
            string strChk17_1 = string.Empty;
            string strChk17_2 = string.Empty;
            string strChk17_3 = string.Empty;

            //유방암
            string strChk06_1 = string.Empty;
            string strChk06_2 = string.Empty;
            string strChk06_3 = string.Empty;
            //자궁경부
            string strChk07_1 = string.Empty;
            string strChk07_2 = string.Empty;
            string strChk07_3 = string.Empty;
            //간암(상반기)
            string strChk08_1 = string.Empty;
            string strChk08_2 = string.Empty;
            string strChk08_3 = string.Empty;
            //간암(하반기)
            string strChk09_1 = string.Empty;
            string strChk09_2 = string.Empty;
            string strChk09_3 = string.Empty;
            //폐암
            string strChk10_1 = string.Empty;
            string strChk10_2 = string.Empty;
            string strChk10_3 = string.Empty;

            string strOK         = string.Empty;
            string strGubun      = string.Empty;
            string strLifeGubun  = string.Empty;

            string strExamA = string.Empty; //이상지질혈증
            string strExamD = string.Empty; //골밀도
            string strExamE = string.Empty; //인지기능장애
            string strExamF = string.Empty; //정신건강검사
            string strExamG = string.Empty; //생활습관평가
            string strExamH = string.Empty; //노인신체기능
            string strExamI = string.Empty; //치면세균막

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            #endregion

            strOK = "OK"; strGubun = "일반";

            if (var.RID.IsNullOrEmpty()) { return; }

            FstrHtml = GetHTMLFromWebBrowser();
            FstrHtml = VB.Replace(FstrHtml, @"""", "");
            FstrHtml = FstrHtml.Replace("\n", "");
            FstrHtml = FstrHtml.Replace(" style=background-color:yellow;", "");
            //사업년도            
            strYear = VB.Pstr(FstrHtml, "<th scope=row class=first>사업년도</th>", 2);
            strYear = VB.Pstr(strYear, "<td class=left>", 2);
            strYear = VB.Pstr(strYear, "</td>", 1);
            if (strYear != var.YEAR) { strOK = "NO"; }

            //성명
            if (strOK == "OK")
            {
                strSName = VB.Pstr(FstrHtml, "<th scope=row class=first>성명</th>", 2);
                strSName = VB.Pstr(strSName, "<td class=left>", 2);
                strSName = VB.Pstr(strSName, "</td>", 1);
                if (strSName != var.SNAME) { strOK = "NO"; }
            }

            if (strOK == "OK")
            {
                //증번호
                strGkiho = VB.Pstr(FstrHtml, "<th scope=row class=first>증번호</th>", 2);
                strGkiho = VB.Pstr(strGkiho, "<td class=left>", 2);
                strGkiho = VB.Pstr(strGkiho, "</td>", 1);

                //소속지사
                strJisa = VB.Pstr(FstrHtml, "<th scope=row class=first>소속지사</th>", 2);
                strJisa = VB.Pstr(strJisa, "<td class=left>", 2);
                strJisa = VB.Pstr(strJisa, "</td>", 1);

                //진단대상
                //strRel = VB.Pstr(FstrHtml, "<th scope=row class=first>진단대상</th>", 2);
                //strRel = VB.Pstr(strRel, "id=EXMD_TGT_TYPE_NM value=", 2);
                //strRel = VB.Pstr(strRel, "</td>", 1);
                //strRel = strRel.Replace(">", "");

                strRel = VB.Pstr(FstrHtml, "<th scope=row class=first>직역구분</th>", 2);
                strRel = VB.Pstr(strRel, "<td class=left>", 2);
                strRel = VB.Pstr(strRel, "</td>", 1);

                //사업장관리번호
                strkiho = VB.Pstr(FstrHtml, "<th scope=row class=first>사업장번호</th>", 2);
                strkiho = VB.Pstr(strkiho, "<td class=left>", 2);
                strkiho = VB.Pstr(strkiho, "</td>", 1);

                //국가암 통보처
                strCancer53 = VB.Pstr(FstrHtml, "<th scope=row class=first>국가암 통보처</th>", 2);
                strCancer53 = VB.Pstr(strCancer53, "<td class=left colspan=3>", 2);
                strCancer53 = VB.Pstr(strCancer53, "</td>", 1);
                strCancer53 = VB.Pstr(strCancer53, "[", 2);
                strCancer53 = VB.Pstr(strCancer53, "]", 1);

                //1차진단
                if (VB.InStr(FstrHtml, "<th scope=row colspan=4 class=first>건강검진</th>") > 0)
                {
                    strFirst = VB.Pstr(FstrHtml, "<th scope=row colspan=4 class=first>건강검진</th>", 2);
                }
                else if (VB.InStr(FstrHtml, "건강검진[일반]") > 0)
                {
                    strFirst = VB.Pstr(FstrHtml, "건강검진[일반]", 2);
                }
                //2020-04-02(의료급여생애 대상자 조회 누락으로 추가)
                else if (VB.InStr(FstrHtml, "건강검진[의료급여생애]") > 0)
                {
                    strFirst = VB.Pstr(FstrHtml, "건강검진[의료급여생애]", 2);
                }
                //2020-07-03(의료급여 대상자 조회 누락으로 추가)
                else if (VB.InStr(FstrHtml, "건강검진[의료급여]") > 0)
                {
                    strFirst = VB.Pstr(FstrHtml, "건강검진[의료급여]", 2); 
                }

                strFirst = VB.Pstr(strFirst, "<td class=left>", 2);
                strFirst = VB.Pstr(strFirst, "</td>", 1);

                //구강검진
                strDental = VB.Pstr(FstrHtml, "<th scope=row class=first>구강검진</th>", 2);
                strDental = VB.Pstr(strDental, "<td class=left>", 2);
                strDental = VB.Pstr(strDental, "</td>", 1); 


                //연령별 세부검사항목-----------------------------------start---------------
                //이상지질혈증
                strExamA = VB.Pstr(FstrHtml, "<th scope=row colspan=9 class=first>연령별 세부 검사 항목</th>", 2);
                strExamA = VB.Pstr(strExamA, "<td class=cen>", 2);
                strExamA = VB.Pstr(strExamA, "</td>", 1);

                //2020-02-04
                if (VB.InStr(strExamA, "비대상") > 0) { strExamA = "비대상"; }
                else if (VB.InStr(strExamA, "대상") > 0) { strExamA = "대상"; }

                //B형간염
                strLiver = VB.Pstr(FstrHtml, "<th scope=row colspan=9 class=first>연령별 세부 검사 항목</th>", 2);
                strLiver = VB.Pstr(strLiver, "<td class=cen>", 3);
                strLiver = VB.Pstr(strLiver, "</td>", 1);

                //2020-02-04
                if (VB.InStr(strLiver, "비대상") > 0) { strLiver = "비대상"; }
                else if (VB.InStr(strLiver, "대상") > 0) { strLiver = "대상"; }

                //C형간염(2020-09-01)
                strLiverC = VB.Pstr(FstrHtml, "<th scope=row colspan=9 class=first>연령별 세부 검사 항목</th>", 2);
                strLiverC = VB.Pstr(strLiverC, "<td class=cen>", 4);
                strLiverC = VB.Pstr(strLiverC, "</td>", 1);

                if (VB.InStr(strLiverC, "비대상") > 0) { strLiverC = "비대상"; }
                else if (VB.InStr(strLiverC, "대상") > 0) { strLiverC = "대상"; }

                //골밀도
                strExamD = VB.Pstr(FstrHtml, "<th scope=row colspan=9 class=first>연령별 세부 검사 항목</th>", 2);
                strExamD = VB.Pstr(strExamD, "<td class=cen>", 5);
                strExamD = VB.Pstr(strExamD, "</td>", 1);

                //2020-02-04
                if (VB.InStr(strExamD, "비대상") > 0) { strExamD = "비대상"; }
                else if (VB.InStr(strExamD, "대상") > 0) { strExamD = "대상"; }

                //인지기능장애
                strExamE = VB.Pstr(FstrHtml, "<th scope=row colspan=9 class=first>연령별 세부 검사 항목</th>", 2);
                strExamE = VB.Pstr(strExamE, "<td class=cen>", 6);
                strExamE = VB.Pstr(strExamE, "</td>", 1);

                //2020-02-04
                if (VB.InStr(strExamE, "비대상") > 0) { strExamE = "비대상"; }
                else if (VB.InStr(strExamE, "대상") > 0) { strExamE = "대상"; }

                //정신건강검사
                strExamF = VB.Pstr(FstrHtml, "<th scope=row colspan=9 class=first>연령별 세부 검사 항목</th>", 2);
                strExamF = VB.Pstr(strExamF, "<td class=cen>", 7);
                strExamF = VB.Pstr(strExamF, "</td>", 1);

                //2020-02-04
                if (VB.InStr(strExamF, "비대상") > 0) { strExamF = "비대상"; }
                else if (VB.InStr(strExamF, "대상") > 0) { strExamF = "대상"; }

                //생활습관평가
                strExamG = VB.Pstr(FstrHtml, "<th scope=row colspan=9 class=first>연령별 세부 검사 항목</th>", 2);
                strExamG = VB.Pstr(strExamG, "<td class=cen>", 8);
                strExamG = VB.Pstr(strExamG, "</td>", 1);

                //2020-02-04
                if (VB.InStr(strExamG, "비대상") > 0) { strExamG = "비대상"; }
                else if (VB.InStr(strExamG, "대상") > 0) { strExamG = "대상"; }

                //노인신체기능
                strExamH = VB.Pstr(FstrHtml, "<th scope=row colspan=9 class=first>연령별 세부 검사 항목</th>", 2);
                strExamH = VB.Pstr(strExamH, "<td class=cen>", 9);
                strExamH = VB.Pstr(strExamH, "</td>", 1);

                //2020-02-04
                if (VB.InStr(strExamH, "비대상") > 0) { strExamH = "비대상"; }
                else if (VB.InStr(strExamH, "대상") > 0) { strExamH = "대상"; }

                //치면세균막
                strExamI = VB.Pstr(FstrHtml, "<th scope=row colspan=9 class=first>연령별 세부 검사 항목</th>", 2);
                strExamI = VB.Pstr(strExamI, "<td class=cen>", 10);
                strExamI = VB.Pstr(strExamI, "</td>", 1);

                //2020-02-04
                if (VB.InStr(strExamI, "비대상") > 0) { strExamI = "비대상"; }
                else if (VB.InStr(strExamI, "대상") > 0) { strExamI = "대상"; }
               
                // 연령별 세부검사 항목 -------------------------End ---------------------------

                //위암 부담율
                strCancer11 = VB.Pstr(FstrHtml, "<th scope=row class=first>위암</th>", 2);
                strCancer11 = VB.Pstr(strCancer11, "<td class=left>", 2);
                strCancer11 = VB.Pstr(strCancer11, "</td>", 1);

                //위암 치료비지원
                strCancer12 = VB.Pstr(FstrHtml, "<th scope=row class=first>위암</th>", 2);
                strCancer12 = VB.Pstr(strCancer12, "<td class=left>", 2);
                strCancer12 = VB.Pstr(strCancer12, "</td>", 1);

                //대장암 부담율
                strCancer31 = VB.Pstr(FstrHtml, "<th scope=row class=first>대장암</th>", 2);
                strCancer31 = VB.Pstr(strCancer31, "<td class=left>", 2);
                strCancer31 = VB.Pstr(strCancer31, "</td>", 1);

                //대장암 치료비지원
                strCancer32 = VB.Pstr(FstrHtml, "<th scope=row class=first>대장암</th>", 2);
                strCancer32 = VB.Pstr(strCancer32, "<td class=left>", 2);
                strCancer32 = VB.Pstr(strCancer32, "</td>", 1);

                //간암(상반기)
                strCancer41 = VB.Pstr(FstrHtml, "<th scope=row class=first>간암 상반기</th>", 2);
                strCancer41 = VB.Pstr(strCancer41, "<td class=left>", 2);
                strCancer41 = VB.Pstr(strCancer41, "</td>", 1);

                //간암 치료비지원
                strCancer42 = VB.Pstr(FstrHtml, "<th scope=row class=first>간암 상반기</th>", 2);
                strCancer42 = VB.Pstr(strCancer42, "<td class=left>", 2);
                strCancer42 = VB.Pstr(strCancer42, "</td>", 1);

                //간암(하반기)
                strCancer61 = VB.Pstr(FstrHtml, "<th scope=row class=first>간암 하반기</th>", 2);
                strCancer61 = VB.Pstr(strCancer61, "<td class=left>", 2);
                strCancer61 = VB.Pstr(strCancer61, "</td>", 1);

                //간암 치료비지원
                strCancer62 = VB.Pstr(FstrHtml, "<th scope=row class=first>간암 하반기</th>", 2);
                strCancer62 = VB.Pstr(strCancer62, "<td class=left>", 2);
                strCancer62 = VB.Pstr(strCancer62, "</td>", 1);

                //유방암
                strCancer21 = VB.Pstr(FstrHtml, "<th scope=row class=first>유방암</th>", 2);
                strCancer21 = VB.Pstr(strCancer21, "<td class=left>", 2);
                strCancer21 = VB.Pstr(strCancer21, "</td>", 1);

                //유방암 치료비지원
                strCancer22 = VB.Pstr(FstrHtml, "<th scope=row class=first>유방암</th>", 2);
                strCancer22 = VB.Pstr(strCancer22, "<td class=left>", 2);
                strCancer22 = VB.Pstr(strCancer22, "</td>", 1);

                //자궁경부암
                strCancer51 = VB.Pstr(FstrHtml, "<th scope=row class=first>자궁 경부암</th>", 2);
                strCancer51 = VB.Pstr(strCancer51, "<td class=left>", 2);
                strCancer51 = VB.Pstr(strCancer51, "</td>", 1);

                //자궁경부암 치료비지원
                strCancer52 = VB.Pstr(FstrHtml, "<th scope=row class=first>자궁 경부암</th>", 2);
                strCancer52 = VB.Pstr(strCancer52, "<td class=left>", 2);
                strCancer52 = VB.Pstr(strCancer52, "</td>", 1);

                //폐암
                strCancer71 = VB.Pstr(FstrHtml, "<th scope=row class=first>폐암</th>", 2);
                strCancer71 = VB.Pstr(strCancer71, "<td class=left>", 2);
                strCancer71 = VB.Pstr(strCancer71, "</td>", 1);

                //폐암 치료비지원
                strCancer72 = VB.Pstr(FstrHtml, "<th scope=row class=first>폐암</th>", 2);
                strCancer72 = VB.Pstr(strCancer72, "<td class=left>", 2);
                strCancer72 = VB.Pstr(strCancer72, "</td>", 1);

                //검진 정보 --------------------------------------------------------------------------------------
                string strHtml = FstrHtml.Replace("\n", "").Replace("\t", "").Replace("readonly=readonly", "");

                //1차진단
                strChk01_1 = VB.Pstr(strHtml, "id=HCHK_DT1", 2);
                strChk01_1 = VB.Pstr(strChk01_1, "</td>", 1);
                strChk01_1 = VB.Pstr(strChk01_1, "value=", 2);
                strChk01_1 = VB.Pstr(strChk01_1, ">", 1);
                //1차진단 출장여부
                strChk01_2 = VB.Pstr(strHtml, "id=EXMD_PLC_TYPE1 value=1", 2);
                strChk01_2 = VB.Pstr(strChk01_2, "</TD>", 1);
                strChk01_2 = strChk01_2.Length != strChk01_2.Replace("CHECKED", "").Length ? "Y" : "";
                //1차 검진기관
                strChk01_3 = VB.Pstr(strHtml, "id=HCHK_DT1", 2);
                strChk01_3 = VB.Pstr(strChk01_3, "<td headers=me1 rs1>", 2);
                strChk01_3 = VB.Pstr(strChk01_3, "</td>", 1);

                //구강검사
                strChk03_1 = VB.Pstr(strHtml, "id=HCHK_DT3", 2);
                strChk03_1 = VB.Pstr(strChk03_1, "</td>", 1);
                strChk03_1 = VB.Pstr(strChk03_1, "value=", 2);
                strChk03_1 = VB.Pstr(strChk03_1, ">", 1);
                //구강검사 출장여부
                strChk03_2 = VB.Pstr(strHtml, "id=EXMD_PLC_TYPE3 value=1", 2);
                strChk03_2 = VB.Pstr(strChk03_2, "</TD>", 1);
                strChk03_2 = strChk03_2.Length != strChk03_2.Replace("CHECKED", "").Length ? "Y" : "";
                //구강검사 검진기관
                strChk03_3 = VB.Pstr(strHtml, "id=HCHK_DT3", 2);
                strChk03_3 = VB.Pstr(strChk03_3, "<td headers=me1 rs1>", 2);
                strChk03_3 = VB.Pstr(strChk03_3, "</td>", 1);

                //위암검사
                strChk04_1 = VB.Pstr(strHtml, "id=HCHK_DT4", 2);
                strChk04_1 = VB.Pstr(strChk04_1, "</TD>", 1);
                strChk04_1 = VB.Pstr(strChk04_1, "value=", 2);
                strChk04_1 = VB.Pstr(strChk04_1, ">", 1);
                //위암검사 검진기관
                strChk04_3 = VB.Pstr(strHtml, "id=HCHK_DT4", 2);
                strChk04_3 = VB.Pstr(strChk04_3, "<td headers=me1 rs1>", 2);
                strChk04_3 = VB.Pstr(strChk04_3, "</td>", 1);

                //2020-04-01
                //위암 (내시경)
                strChk15_1 = VB.Pstr(strHtml, "id=HCHK_DT15", 2);
                strChk15_1 = VB.Pstr(strChk15_1, "</TD>", 1);
                strChk15_1 = VB.Pstr(strChk15_1, "value=", 2);
                strChk15_1 = VB.Pstr(strChk15_1, ">", 1);
                //위암검사 검진기관
                strChk15_3 = VB.Pstr(strHtml, "id=HCHK_DT15", 2);
                strChk15_3 = VB.Pstr(strChk15_3, "<td headers=me1 rs1>", 2);
                strChk15_3 = VB.Pstr(strChk15_3, "</td>", 1);

                //대장검사(잠혈)
                strChk05_1 = VB.Pstr(strHtml, "id=HCHK_DT5", 2);
                strChk05_1 = VB.Pstr(strChk05_1, "</TD>", 1);
                strChk05_1 = VB.Pstr(strChk05_1, "value=", 2);
                strChk05_1 = VB.Pstr(strChk05_1, ">", 1);
                //대장검사(잠혈) 검진기관
                strChk05_3 = VB.Pstr(strHtml, "id=HCHK_DT5", 2);
                strChk05_3 = VB.Pstr(strChk05_3, "<td headers=me1 rs1>", 2);
                strChk05_3 = VB.Pstr(strChk05_3, "</td>", 1);

                //2020-04-01
                //대장검사(조영)
                strChk16_1 = VB.Pstr(strHtml, "id=HCHK_DT16", 2);
                strChk16_1 = VB.Pstr(strChk16_1, "</TD>", 1);
                strChk16_1 = VB.Pstr(strChk16_1, "value=", 2);
                strChk16_1 = VB.Pstr(strChk16_1, ">", 1);
                //대장검사(조영) 검진기관
                strChk16_3 = VB.Pstr(strHtml, "id=HCHK_DT16", 2);
                strChk16_3 = VB.Pstr(strChk16_3, "<td headers=me1 rs1>", 2);
                strChk16_3 = VB.Pstr(strChk16_3, "</td>", 1);
                //대장검사(내시경)
                strChk17_1 = VB.Pstr(strHtml, "id=HCHK_DT17", 2);
                strChk17_1 = VB.Pstr(strChk17_1, "</TD>", 1);
                strChk17_1 = VB.Pstr(strChk17_1, "value=", 2);
                strChk17_1 = VB.Pstr(strChk17_1, ">", 1);
                //대장검사(내시경) 검진기관
                strChk17_3 = VB.Pstr(strHtml, "id=HCHK_DT17", 2);
                strChk17_3 = VB.Pstr(strChk17_3, "<td headers=me1 rs1>", 2);
                strChk17_3 = VB.Pstr(strChk17_3, "</td>", 1);

                //유방암검사
                strChk06_1 = VB.Pstr(strHtml, "id=HCHK_DT6", 2);
                strChk06_1 = VB.Pstr(strChk06_1, "</TD>", 1);
                strChk06_1 = VB.Pstr(strChk06_1, "value=", 2);
                strChk06_1 = VB.Pstr(strChk06_1, ">", 1);
                //유방암검사 검진기관
                strChk06_3 = VB.Pstr(strHtml, "id=HCHK_DT6", 2);
                strChk06_3 = VB.Pstr(strChk06_3, "<td headers=me1 rs1>", 2);
                strChk06_3 = VB.Pstr(strChk06_3, "</td>", 1);

                //자궁경부암검사
                strChk07_1 = VB.Pstr(strHtml, "id=HCHK_DT7", 2);
                strChk07_1 = VB.Pstr(strChk07_1, "</TD>", 1);
                strChk07_1 = VB.Pstr(strChk07_1, "value=", 2);
                strChk07_1 = VB.Pstr(strChk07_1, ">", 1);
                //자궁경부암검사 검진기관
                strChk07_3 = VB.Pstr(strHtml, "id=HCHK_DT7", 2);
                strChk07_3 = VB.Pstr(strChk07_3, "<td headers=me1 rs1>", 2);
                strChk07_3 = VB.Pstr(strChk07_3, "</td>", 1);

                //간암검사(상반기)
                strChk08_1 = VB.Pstr(strHtml, "id=HCHK_DT8", 2);
                strChk08_1 = VB.Pstr(strChk08_1, "</TD>", 1);
                strChk08_1 = VB.Pstr(strChk08_1, "value=", 2);
                strChk08_1 = VB.Pstr(strChk08_1, ">", 1);
                //간암검사 검진기관
                strChk08_3 = VB.Pstr(strHtml, "id=HCHK_DT8", 2);
                strChk08_3 = VB.Pstr(strChk08_3, "<td headers=me1 rs1>", 2);
                strChk08_3 = VB.Pstr(strChk08_3, "</td>", 1);

                //간암검사(하반기)
                strChk09_1 = VB.Pstr(strHtml, "id=HCHK_DT11", 2);
                strChk09_1 = VB.Pstr(strChk09_1, "</TD>", 1);
                strChk09_1 = VB.Pstr(strChk09_1, "value=", 2);
                strChk09_1 = VB.Pstr(strChk09_1, ">", 1);
                //간암검사 검진기관
                strChk09_3 = VB.Pstr(strHtml, "id=HCHK_DT11", 2);
                strChk09_3 = VB.Pstr(strChk09_3, "<td headers=me1 rs1>", 2);
                strChk09_3 = VB.Pstr(strChk09_3, "</td>", 1);

                //폐암
                strChk10_1 = VB.Pstr(strHtml, "id=HCHK_DT9", 2);
                strChk10_1 = VB.Pstr(strChk10_1, "</TD>", 1);
                strChk10_1 = VB.Pstr(strChk10_1, "value=", 2);
                strChk10_1 = VB.Pstr(strChk10_1, ">", 1);
                //폐암검사 검진기관
                strChk10_3 = VB.Pstr(strHtml, "id=HCHK_DT9", 2);
                strChk10_3 = VB.Pstr(strChk10_3, "<td headers=me1 rs1>", 2);
                strChk10_3 = VB.Pstr(strChk10_3, "</td>", 1);
            }

            //자격조회 완료 및 정보 저장
            if (strOK =="OK")
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "WORK_NHIC ";
                SQL += ComNum.VBLF + "    SET PNAME         ='" + strPName.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,BI            ='" + strBI.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,KIHO          ='" + strkiho.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GKIHO         ='" + strGkiho.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,BDATE         ='" + strBDate.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,REL           ='" + strRel.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,JISA          ='" + strJisa.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,YEAR          ='" + strYear.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,TRANS         ='" + strTrans.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,EKG           ='" + strEKG.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,FIRST         ='" + strFirst.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,DENTAL        ='" + strDental.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,LIVER         ='" + strLiver.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,LIVER2        ='" + strLiver2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,LIVERC        ='" + strLiverC.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,SECOND        ='" + strSecond.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,FIRSTADD      ='" + str1차Add.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,SECONDADD     ='" + str2차Add.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER11      ='" + strCancer11.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER12      ='" + strCancer12.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER21      ='" + strCancer21.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER22      ='" + strCancer22.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER31      ='" + strCancer31.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER32      ='" + strCancer32.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER41      ='" + strCancer41.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER42      ='" + strCancer42.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER51      ='" + strCancer51.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER52      ='" + strCancer52.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER53      ='" + strCancer53.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER61      ='" + strCancer61.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER62      ='" + strCancer62.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER71      ='" + strCancer71.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,CANCER72      ='" + strCancer72.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK01       ='" + strChk01_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK01_CHUL  ='" + strChk01_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK01_NAME  ='" + strChk01_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK02       ='" + strChk02_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK02_CHUL  ='" + strChk02_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK02_NAME  ='" + strChk02_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK03       ='" + strChk03_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK03_CHUL  ='" + strChk03_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK03_NAME  ='" + strChk03_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK04       ='" + strChk04_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK04_CHUL  ='" + strChk04_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK04_NAME  ='" + strChk04_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK05       ='" + strChk05_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK05_CHUL  ='" + strChk05_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK05_NAME  ='" + strChk05_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK06       ='" + strChk06_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK06_CHUL  ='" + strChk06_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK06_NAME  ='" + strChk06_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK07       ='" + strChk07_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK07_CHUL  ='" + strChk07_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK07_NAME  ='" + strChk07_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK08       ='" + strChk08_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK08_CHUL  ='" + strChk08_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK08_NAME  ='" + strChk08_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK09       ='" + strChk09_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK09_CHUL  ='" + strChk09_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK09_NAME  ='" + strChk09_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK10       ='" + strChk10_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK10_CHUL  ='" + strChk10_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK10_NAME  ='" + strChk10_3.Trim() + "' ";
                //2020-04-01(위, 대장 구분)
                SQL += ComNum.VBLF + "       ,GBCHK15       ='" + strChk15_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK15_CHUL  ='" + strChk15_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK15_NAME  ='" + strChk15_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK16       ='" + strChk16_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK16_CHUL  ='" + strChk16_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK16_NAME  ='" + strChk16_3.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK17       ='" + strChk17_1.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK17_CHUL  ='" + strChk17_2.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBCHK17_NAME  ='" + strChk17_3.Trim() + "' ";
                //2018-01-01(추가항목)
                SQL += ComNum.VBLF + "       ,EXAMA         ='" + strExamA.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,EXAMD         ='" + strExamD.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,EXAME         ='" + strExamE.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,EXAMF         ='" + strExamF.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,EXAMG         ='" + strExamG.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,EXAMH         ='" + strExamH.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,EXAMI         ='" + strExamI.Trim() + "' ";
                SQL += ComNum.VBLF + "       ,GBSTS         ='1' ";
                SQL += ComNum.VBLF + "       ,GBERROR       ='N' ";
                SQL += ComNum.VBLF + "       ,CTIME         =SYSDATE ";
                SQL += ComNum.VBLF + "  WHERE ROWID         = '" + var.RID.Trim() + "'";
            }
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA +  "WORK_NHIC          ";
                SQL += ComNum.VBLF + "    SET CTIME=SYSDATE, GBERROR='Y', GBSTS ='2'    ";
                SQL += ComNum.VBLF + "  WHERE ROWID = '" + var.RID + "'                 ";
            }

            clsDB.setBeginTran(clsDB.DbCon);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
            //ComFunc.MsgBox("저장되었습니다.", "알림");
            Cursor.Current = Cursors.Default;

        }

        private bool SetInputHTML(string jUMIN, string sNAME, string yEAR)
        {
            bool rtnVal = false;

            try
            {
                browser.ExecuteScriptAsync("document.getElementById('S_JUMIN_NO').value= '" + jUMIN + "'");
                browser.ExecuteScriptAsync("document.getElementById('S_NM').value= '" + sNAME + "'");
                browser.ExecuteScriptAsync("document.getElementById('EXMD_BZ_YYYY').value= '" + yEAR + "'");

                rtnVal = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "웹 컨트롤 실패");
                rtnVal = false;
            }
            
            return rtnVal;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            cboYear.SelectedIndex = 0;
            dtpFDate.Text = DateTime.Now.ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();
            cSpd.Spread_Clear_Simple(SS1);
            this.collapsibleSplitContainer1.Panel1Collapsed = true;
            timer1.Enabled = false;
        }

        private void eFormClosing(object sender, FormClosingEventArgs e)
        {
            //Cef.Shutdown();
        }

        private void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        public void SetControl()
        {
            workNhicService = new WorkNhicService();
            HicJepsuPatientService = new HicJepsuPatientService();
            hicResultService = new HicResultService();
            hicJepsuService = new HicJepsuService();
            cSpd = new clsSpread();

            Cef.Initialize(new CefSettings());
            browser = new ChromiumWebBrowser("http://sis.nhis.or.kr/ggob011_r01.do?reqUrl=ggob011m01");
            this.collapsibleSplitContainer1.Panel2.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;

            JsDialogHandler jss = new JsDialogHandler();
            browser.JsDialogHandler = jss;


            txtbox = new TextBox();
            this.Controls.Add(txtbox);
            txtbox.Dock = DockStyle.Right;
            txtbox.Width = 500;
            txtbox.Multiline = true;
            txtbox.ScrollBars = ScrollBars.Both;
            //디버깅시 사용할것
            txtbox.Visible = false;

            int nYear = VB.Left(DateTime.Now.ToShortDateString(), 4).To<int>();

            cboYear.Items.Clear();

            for (int i = 0; i < 2; i++)
            {
                cboYear.Items.Add(nYear);
                nYear = nYear - 1;
            }

        }

        private string GetHTMLFromWebBrowser()
        {
            Task<String> taskHtml = browser.GetBrowser().MainFrame.GetSourceAsync();

            string response = taskHtml.Result;
            return response;
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnView)
            {
                txtbox.Text = GetHTMLFromWebBrowser();
            }
            else if (sender == btnSearch)
            {
                Display_Pat_List();
            }
            else if (sender == btnStart)
            {
                Work_Start();
            }
            else if (sender == btnStop)
            {
                Work_Stop();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        private void Data_Save()
        {
            
            

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                browser.ExecuteScriptAsync("document.getElementById('CHECK1').checked= true");
                browser.ExecuteScriptAsync("document.getElementById('HCHK_DT1').value= '20200204'");
                browser.ExecuteScriptAsync("f_clickHmeCheck(this,'HCHK_DT1','READ_ONLY_YN1')");
                Thread.Sleep(100);

                browser.ExecuteScriptAsync("document.getElementById('CHECK3').checked= true");
                browser.ExecuteScriptAsync("document.getElementById('HCHK_DT3').value= '20200204'");
                browser.ExecuteScriptAsync("f_clickHmeCheck(this,'HCHK_DT3','READ_ONLY_YN3')");
                Thread.Sleep(100);

                browser.ExecuteScriptAsync("f_save()");
            }   
        }

        private void Display_Pat_List()
        {
            int nRow = 0;

            cSpd.Spread_Clear_Simple(SS1);

            IList<HIC_JEPSU_PATIENT> lists = HicJepsuPatientService.GetNhicListByDate(dtpFDate.Text, dtpTDate.Text);

            if (lists.Count > 0)
            {
                SS1.ActiveSheet.RowCount = lists.Count;

                for (int i = 0; i < lists.Count; i++)
                {
                    if (lists[i].WRTNO == 1013018)
                    {
                        i = i;
                    }
                    SS1.ActiveSheet.Rows[nRow].Height = 24;

                    SS1.ActiveSheet.Cells[nRow, 0].Text = "True";
                    SS1.ActiveSheet.Cells[nRow, 1].Text = lists[i].WRTNO.To<string>();
                    SS1.ActiveSheet.Cells[nRow, 2].Text = lists[i].JEPDATE_STR;
                    SS1.ActiveSheet.Cells[nRow, 3].Text = lists[i].GJJONG;
                    SS1.ActiveSheet.Cells[nRow, 4].Text = lists[i].SNAME;
                    SS1.ActiveSheet.Cells[nRow, 5].Text = clsAES.DeAES(lists[i].JUMIN2);
                    if (lists[i].GBCHUL == "Y")
                    {
                        if (lists[i].GBCHUL2 != "Y")
                        {
                            SS1.ActiveSheet.Cells[nRow, 6].Text = "Y";
                        }
                    }
                    SS1.ActiveSheet.Cells[nRow, 8].Text = lists[i].GBDENTAL == "Y" ? "Y" : "";
                    switch (lists[i].GJJONG)
                    {
                        case "11": SS1.ActiveSheet.Cells[nRow, 7].Text = "Y"; break;
                        case "16": SS1.ActiveSheet.Cells[nRow, 9].Text = "Y"; break;
                        default: break;
                    }
                    SS1.ActiveSheet.Cells[nRow, 20].Text = lists[i].GJYEAR;

                    SS1.ActiveSheet.Cells[nRow, 10].Text = hicResultService.GetRowidByOneExcodeWrtno("TX22", lists[i].WRTNO)   != null ? "Y" : "";  //위암(조영)
                    SS1.ActiveSheet.Cells[nRow, 11].Text = hicResultService.GetRowidStomachByWrtno(lists[i].WRTNO) != null ? "Y" : "";              //위암(내시경)
                    SS1.ActiveSheet.Cells[nRow, 12].Text = hicResultService.GetRowidByOneExcodeWrtno("TX26", lists[i].WRTNO)   != null ? "Y" : "";  //대장암(잠혈)
                    SS1.ActiveSheet.Cells[nRow, 13].Text = hicResultService.GetRowidByOneExcodeWrtno("TX31", lists[i].WRTNO)   != null ? "Y" : "";  //대장암(조영)
                    SS1.ActiveSheet.Cells[nRow, 14].Text = hicResultService.GetRowidColonByWrtno(lists[i].WRTNO)   != null ? "Y" : "";              //대장암(내시경)
                    SS1.ActiveSheet.Cells[nRow, 15].Text = VB.Mid(lists[i].GBAM, 9, 1) == "1" ? "Y" : "";                                           //유방
                    SS1.ActiveSheet.Cells[nRow, 16].Text = VB.Mid(lists[i].GBAM, 11, 1) == "1" ? "Y" : "";                                          //자궁경부
                    if (VB.Mid(lists[i].GBAM, 7, 1) == "1")
                    {
                        if (string.Compare(VB.Mid(lists[i].JEPDATE_STR, 6, 2), "07") < 0)
                        {
                            SS1.ActiveSheet.Cells[nRow, 17].Text = "Y";      //간암(상반기)
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRow, 18].Text = "Y";      //간암(하반기)
                        }
                    }
                    SS1.ActiveSheet.Cells[nRow, 19].Text = VB.Mid(lists[i].GBAM, 13, 1) == "1" ? "Y" : "";      //폐암

                    nRow = nRow + 1;
                }
            }
        }

        private void Work_Start()
        {
            if (rdoJob1.Checked)
            {
                lblMsg.Text = "자격조회 시작 .. ";
                timer1.Enabled = true;
            }
            else
            {
                lblMsg.Text = "수검등록 시작 .. ";
                timer5.Enabled = true;
            }
            
            FstrQuery = "1";
            browser.Load("http://sis.nhis.or.kr/ggob011_r01.do?reqUrl=ggob011m01");
            
        }

        private void Work_Stop()
        {
            lblMsg.Text = "! 작업 중지 !";
            FstrQuery = "";
            browser.Stop();
            timer1.Enabled = false;
            timer3.Enabled = false;
            timer4.Enabled = false;
            timer5.Enabled = false;
        }

    }
}
