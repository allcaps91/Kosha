using ComBase;
using ComHpcLibB.Service;
using System;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanInternetMunjin_New.cs
/// Description     : 인터넷 문진표
/// Author          : 이상훈
/// Create Date     : 2019-12-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm인터넷문진표_New.frm(Frm인터넷문진표_New)" />

namespace ComHpcLibB
{
    public partial class frmHcPanInternetMunjin_New : Form
    {
        HicJepsuService hicJepsuService = null;
        ComHpcLibBService comHpcLibBService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();

        long FnMunID;
        long FnCnt;
        string FstrForm;
        long FnMunID_OLD;
        string strRefresh;
        string strPtNo;
        string FsPtNo;

        public frmHcPanInternetMunjin_New(long nMunID, string strForm, string sPtNo)
        {
            InitializeComponent();

            FnMunID = nMunID;
            FstrForm = strForm;
            FsPtNo = sPtNo;

            SetEvent();
        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            comHpcLibBService = new ComHpcLibBService();

            this.Load += new EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnModify.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.timer1.Tick += new EventHandler(eTimerTick);
        }

        public void eFormLoad(object sender, EventArgs e)
        {
            //READ_금액표시기_SETTING(Frm인터넷문진표_New, lstMonitors)
            //hf.Read_Amountindicator(this, lstMonitors);

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

            timer1.Enabled = true;
        }

        public void eFormActivated(object sender, EventArgs e)
        {
            //FnMunID = long.Parse(VB.Pstr(clsPublic.GstrHelpCode, "{}", 1));            
            if (FnMunID_OLD != FnMunID)
            {
                timer1.Enabled = true;
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnModify)
            {
                if (hicJepsuService.GetCountbyPtNo(FsPtNo) > 0)
                {
                    WB1.Navigate("http://www.pohangsmh.co.kr/web_question/general/g_modify.html?m_id=" + FnMunID);
                    Application.DoEvents();
                }
                else
                {
                    MessageBox.Show("검진 당일만 문진표 수정이 가능합니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (sender == btnPrint)
            {
                if (MessageBox.Show("인쇄를 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
                //WB1.ShowPrintDialog();
                WB1.Print();
            }
            else if (sender == btnRun)
            {
                int result = comHpcLibBService.InsertHicIEMunjinSendReq(FsPtNo, FstrForm, FnMunID);
                if (result <= 0)
                {
                    MessageBox.Show("저장실패", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        void eTimerTick(object sender, EventArgs e)
        {
            string strYear = "";

            timer1.Enabled = false;

            strPtNo = comHpcLibBService.GetPtNobyViewId(FnMunID);

            strYear = comHpcLibBService.GetMunDatebyPtNo(strPtNo);

            System.Threading.Thread.Sleep(1000);
            FnMunID_OLD = FnMunID;

            if (string.Compare(strYear, "2019") < 0)
            {
                WB1.Navigate("http://www.pohangsmh.co.kr/web_question/general/g_view_2018.html?m_id=" + FnMunID);
                Application.DoEvents();
            }
            else
            {
                WB1.Navigate("http://www.pohangsmh.co.kr/web_question/general/g_view.html?m_id=" + FnMunID);
                Application.DoEvents();
            }
        }
    }
}
