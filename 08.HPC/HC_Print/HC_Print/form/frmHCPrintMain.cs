using ComBase;
using HC_Print.form;
using System;
using System.Windows.Forms;

namespace HC_Print
{
    public partial class frmHCPrintMain : Form, MainFormMessage
    {

        string mPara1 = "";



        frmHCPrint_Bohum1 FrmHCPrint_Bohum1 = null;
        frmHcPrint_Spc FrmHcPrint_Spc = null;
        frmHcPrint_Cancer FrmHcPrint_Cancer = null;
        frmHcPrint_Dental FrmHcPrint_Dental = null;
        frmHcPrint_Gongmu FrmHcPrint_Gongmu = null;
        frmHcPrint_Xray FrmHcPrint_Xray = null;
        frmHcBloodExam FrmHcBloodExam = null;
        frmHcPrint_Add FrmHcPrint_Add = null;
        frmHcPrint_Foreign_Main FrmHcPrint_Foreign_Main = null;
        frmHcPrint_JinDan_Main FrmHcPrint_JinDan_Main = null;
        frmHcPrint_Certificate FrmHcPrint_Certificate = null;
        frmHcPrint_Job FrmHcPrint_Job = null;
        frmHcPrint_Talk_ReSend FrmHcPrint_Talk_ReSend = null;
        frmHcPrint_BalDate FrmHcPrint_BalDate = null;
        frmHicXrayPrint FrmHicXrayPrint = null;
        frmHicXrayView FrmHicXrayView = null;
        frmHCXrayList FrmHCXrayList = null;
        frmHcXrayView FrmHcXrayView = null;

        public frmHCPrintMain()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHCPrintMain(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            SetEvent();
            SetControl();
        }

        public frmHCPrintMain(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
            SetEvent();
            SetControl();
        }

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


        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
            this.FormClosing += new FormClosingEventHandler(eFormClosing);

            //결과지출력1
            this.menuPRINT101.Click += new EventHandler(eMenuClick);
            this.menuPRINT102.Click += new EventHandler(eMenuClick);
            this.menuPRINT103.Click += new EventHandler(eMenuClick);
            this.menuPRINT104.Click += new EventHandler(eMenuClick);
            this.menuPRINT105.Click += new EventHandler(eMenuClick);
            this.menuPRINT106.Click += new EventHandler(eMenuClick);

            //결과지출력2
            this.menuPRINT111.Click += new EventHandler(eMenuClick);
            this.menuPRINT112.Click += new EventHandler(eMenuClick);
            this.menuPRINT113.Click += new EventHandler(eMenuClick);
            this.menuPRINT114.Click += new EventHandler(eMenuClick);


            //결과지출력3
            this.menuPRINT121.Click += new EventHandler(eMenuClick);
            this.menuPRINT122.Click += new EventHandler(eMenuClick);
            this.menuPRINT123.Click += new EventHandler(eMenuClick);
            this.menuPRINT124.Click += new EventHandler(eMenuClick);

            //결과지출력4
            this.menuPRINT131.Click += new EventHandler(eMenuClick);
            this.menuPRINT132.Click += new EventHandler(eMenuClick);

            //방사선판독결과
            this.menuPRINT201.Click += new EventHandler(eMenuClick);
            this.menuPRINT202.Click += new EventHandler(eMenuClick);
            this.menuPRINT203.Click += new EventHandler(eMenuClick);
            this.menuPRINT204.Click += new EventHandler(eMenuClick);

            
            this.menuExit.Click += new EventHandler(eMenuClick);
        }

        private void SetControl()
        {
            
        }

        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        private void eFormClosing(object sender, FormClosingEventArgs e)
        {

        }

        void eFormClosed(object sender, FormClosedEventArgs e)
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

        private void eFormLoad(object sender, EventArgs e)
        {

        }
        private void eMenuClick(object sender, EventArgs e)
        {
            if (sender == menuExit)
            {
                this.Close();
                return;
            }
            //1차
            else if (sender == menuPRINT101)
            {
                FrmHCPrint_Bohum1 = new frmHCPrint_Bohum1();
                FrmHCPrint_Bohum1.ShowDialog(this);
            }
            //특수검진
            else if (sender == menuPRINT103)
            {
                FrmHcPrint_Spc = new frmHcPrint_Spc();
                FrmHcPrint_Spc.ShowDialog(this);
            }
            //암검진
            else if (sender == menuPRINT104)
            {
                FrmHcPrint_Cancer = new frmHcPrint_Cancer();
                FrmHcPrint_Cancer.ShowDialog(this);

            }
            //구강검진
            else if (sender == menuPRINT105)
            {
                FrmHcPrint_Dental = new frmHcPrint_Dental();
                FrmHcPrint_Dental.ShowDialog(this);
            }

            //채용(배치전)
            else if (sender == menuPRINT102)
            {

            }
            //공무원
            
            else if (sender == menuPRINT111)
            {
                FrmHcPrint_Gongmu = new frmHcPrint_Gongmu();
                FrmHcPrint_Gongmu.ShowDialog(this);
            }

            //방사선
            else if (sender == menuPRINT114)
            {
                FrmHcPrint_Xray = new frmHcPrint_Xray();
                FrmHcPrint_Xray.ShowDialog(this);
            }
            //혈액
            else if (sender == menuPRINT112)
            {
                FrmHcBloodExam = new frmHcBloodExam();
                FrmHcBloodExam.ShowDialog(this);
            }
            //회사추가검진
            else if (sender == menuPRINT113)
            {
                FrmHcPrint_Add = new frmHcPrint_Add();
                FrmHcPrint_Add.ShowDialog(this);
            }

            //외국인채용
            else if (sender == menuPRINT121)
            {
                FrmHcPrint_Foreign_Main = new frmHcPrint_Foreign_Main();
                FrmHcPrint_Foreign_Main.ShowDialog(this);
            }
            //건장진단서
            else if (sender == menuPRINT122)
            {
                FrmHcPrint_JinDan_Main = new frmHcPrint_JinDan_Main();
                FrmHcPrint_JinDan_Main.ShowDialog(this);
            }
            //영문건강진단서
            else if (sender == menuPRINT123)
            {
                FrmHcPrint_Certificate = new frmHcPrint_Certificate();
                FrmHcPrint_Certificate.ShowDialog(this);
            }
            else if (sender == menuPRINT124)
            {
                FrmHcPrint_Job = new frmHcPrint_Job();
                FrmHcPrint_Job.ShowDialog(this);
            }

            //알림톡재전송
            else if (sender == menuPRINT132)
            {
                FrmHcPrint_Talk_ReSend = new frmHcPrint_Talk_ReSend();
                FrmHcPrint_Talk_ReSend.ShowDialog(this);
            }
            //PDF결과다운로드
            else if (sender == menuPRINT106)
            {

            }
            //결과지발송관리
            else if (sender == menuPRINT131)
            {
                FrmHcPrint_BalDate = new frmHcPrint_BalDate();
                FrmHcPrint_BalDate.ShowDialog(this);
            }

            //방사선판독결과
            else if (sender == menuPRINT201)
            {
                FrmHicXrayPrint = new frmHicXrayPrint();
                FrmHicXrayPrint.ShowDialog(this);
            }
            else if (sender == menuPRINT202)
            {
                FrmHicXrayView = new frmHicXrayView();
                FrmHicXrayView.ShowDialog(this);
            }
            else if (sender == menuPRINT203)
            {
                FrmHCXrayList = new frmHCXrayList();
                FrmHCXrayList.ShowDialog(this);
            }
            else if (sender == menuPRINT204)
            {
                FrmHcXrayView = new frmHcXrayView();
                FrmHcXrayView.ShowDialog(this);
            }

        }

    }

}
