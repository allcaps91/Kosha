using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace HC_Bill
{
    public partial class frmHcBillMain : Form
    {
        frmHcBillExpenses FrmHcBillExpenses = null;
        frmHcBillOralExamExpenses FrmHcBillOralExamExpenses = null;
        frmHcBillCancerExpenses FrmHcBillCancerExpenses = null;

        frmHcBillListCreate FrmHcBillListCreate = null;
        frmHcBillExpenseFileCreate FrmHcBillExpenseFileCreate = null;
        frmHcBillChkList FrmHcBillChkList = null;
        frmHcBillSunapList FrmHcBillSunapList = null;
        frmMirErrorList FrmMirErrorList = null;
        frmCltd FrmCltd = null;
        frmHcBillResultNotiSendList FrmHcBillResultNotiSendList = null;
        frmHcBillDailyExpenseView FrmHcBillDailyExpenseView = null;
        frmHcBillMIrLtdList FrmHcBillMIrLtdList = null;
        frmHcBillMirTotList FrmHcBillMirTotList = null;
        frmHcBillCofirmedListView FrmHcBillCofirmedListView = null;

        frmHcBillGView FrmHcBillGView = null;
        frmHcBillCancerCountInpect FrmHcBillCancerCountInpect = null;

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

        public frmHcBillMain()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcBillMain(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            SetEvent();
        }

        public frmHcBillMain(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
            SetEvent();
        }

        void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
            this.menuExit.Click += new EventHandler(eMenuClick);
            this.menuMainMir.Click += new EventHandler(eMenuClick);
            this.menuEtc.Click += new EventHandler(eMenuClick);
            this.menuSearch.Click += new EventHandler(eMenuClick);
            this.menuOldMir.Click += new EventHandler(eMenuClick);

            this.Menu01_01.Click += new EventHandler(eMenuClick);   //1.청구자 제외명단 관리(통합)
            this.Menu01_02.Click += new EventHandler(eMenuClick);   //2.청구대상자 명단작성
            this.Menu01_03.Click += new EventHandler(eMenuClick);   //3.청구대상자 점검 및 파일작성
            this.Menu01_04.Click += new EventHandler(eMenuClick);   //4.검진비용청구서
            this.Menu01_05.Click += new EventHandler(eMenuClick);   //5.구강비용청구서
            this.Menu01_06.Click += new EventHandler(eMenuClick);   //6.암검진비용청구서
            this.Menu01_07.Click += new EventHandler(eMenuClick);   //7.수납집계표
            this.Menu01_08.Click += new EventHandler(eMenuClick);   //8.건진청구 오류 수정의뢰 내역
            this.Menu01_09.Click += new EventHandler(eMenuClick);   //9.암검진 건수 점검

            this.Menu02_01.Click += new EventHandler(eMenuClick);   //1.공무원 인원조회
            this.Menu02_02.Click += new EventHandler(eMenuClick);   //2.암검진 회사청구 관리
            this.Menu02_03.Click += new EventHandler(eMenuClick);   //3.결과통보서 발송대장

            this.Menu03_01.Click += new EventHandler(eMenuClick);   //1.청구금액조회(&V)
            this.Menu03_02.Click += new EventHandler(eMenuClick);   //2.회사청구조회 및 청구작업
            this.Menu03_03.Click += new EventHandler(eMenuClick);   //3.종검 및 건진 청구금액 조회
            this.Menu03_04.Click += new EventHandler(eMenuClick);   //4.확진대상자 조회            
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

        void eFormLoad(object sender, EventArgs e)
        {
            this.Text += " " + "☞작업자: " + clsType.User.UserName;
            ComFunc.ReadSysDate(clsDB.DbCon);
            clsCompuInfo.SetComputerInfo();
        }

        void eMenuClick(object sender, EventArgs e)
        {
            if (sender == menuExit)    //종료
            {
                this.Close();
                return;
            }
            else if (sender == Menu01_01)
            {   
                FrmHcBillChkList = new frmHcBillChkList();
                FrmHcBillChkList.StartPosition = FormStartPosition.CenterScreen;
                FrmHcBillChkList.ShowDialog(this);
            }
            else if (sender == Menu01_02)
            {
                FrmHcBillListCreate = new frmHcBillListCreate();
                FrmHcBillListCreate.StartPosition = FormStartPosition.CenterScreen;
                FrmHcBillListCreate.ShowDialog(this);
            }
            else if (sender == Menu01_03)
            {
                FrmHcBillExpenseFileCreate = new frmHcBillExpenseFileCreate();
                FrmHcBillExpenseFileCreate.StartPosition = FormStartPosition.CenterScreen;
                FrmHcBillExpenseFileCreate.ShowDialog(this);
            }
            else if (sender == Menu01_04)
            {
                FrmHcBillExpenses = new frmHcBillExpenses();
                FrmHcBillExpenses.StartPosition = FormStartPosition.CenterScreen;
                FrmHcBillExpenses.ShowDialog(this);
            }
            else if (sender == Menu01_05)
            {
                FrmHcBillOralExamExpenses = new frmHcBillOralExamExpenses();
                FrmHcBillOralExamExpenses.StartPosition = FormStartPosition.CenterScreen;
                FrmHcBillOralExamExpenses.ShowDialog(this);                
            }
            else if (sender == Menu01_06)
            {
                FrmHcBillCancerExpenses = new frmHcBillCancerExpenses();
                FrmHcBillCancerExpenses.StartPosition = FormStartPosition.CenterScreen;
                FrmHcBillCancerExpenses.ShowDialog(this);
            }
            else if (sender == Menu01_07)
            {
                //수납집계표
                FrmHcBillSunapList = new frmHcBillSunapList();
                FrmHcBillSunapList.StartPosition = FormStartPosition.CenterScreen;
                FrmHcBillSunapList.ShowDialog(this);
            }
            else if (sender == Menu01_08)
            {
                FrmMirErrorList = new frmMirErrorList();
                FrmMirErrorList.StartPosition = FormStartPosition.CenterScreen;
                FrmMirErrorList.ShowDialog(this);
            }
            else if (sender == Menu01_09)
            {
                FrmHcBillCancerCountInpect = new frmHcBillCancerCountInpect();
                FrmHcBillCancerCountInpect.StartPosition = FormStartPosition.CenterScreen;
                FrmHcBillCancerCountInpect.ShowDialog(this);
            }
            else if (sender == Menu02_01)
            {
                FrmHcBillGView = new frmHcBillGView();
                FrmHcBillGView.StartPosition = FormStartPosition.CenterScreen;
                FrmHcBillGView.ShowDialog(this);
            }
            else if (sender == Menu02_02)
            {
                FrmCltd = new frmCltd();
                FrmCltd.StartPosition = FormStartPosition.CenterScreen;
                FrmCltd.ShowDialog(this);
            }
            else if (sender == Menu02_03)
            {
                FrmHcBillResultNotiSendList = new frmHcBillResultNotiSendList();
                FrmHcBillResultNotiSendList.StartPosition = FormStartPosition.CenterParent;
                FrmHcBillResultNotiSendList.ShowDialog(this);
            }
            else if (sender == Menu03_01)
            {
                FrmHcBillDailyExpenseView = new frmHcBillDailyExpenseView();
                FrmHcBillDailyExpenseView.StartPosition = FormStartPosition.CenterParent;
                FrmHcBillDailyExpenseView.ShowDialog(this);
                
            }
            else if (sender == Menu03_02)
            {
                FrmHcBillMIrLtdList = new frmHcBillMIrLtdList();
                FrmHcBillMIrLtdList.StartPosition = FormStartPosition.CenterParent;
                FrmHcBillMIrLtdList.ShowDialog(this);                
            }
            else if (sender == Menu03_03)
            {
                FrmHcBillMirTotList = new frmHcBillMirTotList();
                FrmHcBillMirTotList.StartPosition = FormStartPosition.CenterParent;
                FrmHcBillMirTotList.ShowDialog(this);                
            }
            else if (sender == Menu03_04)
            {
                FrmHcBillCofirmedListView = new frmHcBillCofirmedListView();
                FrmHcBillCofirmedListView.StartPosition = FormStartPosition.CenterParent;
                FrmHcBillCofirmedListView.ShowDialog(this);
            }
        }
    }
}
