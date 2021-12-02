using ComBase;
using ComHpcLibB;
using SupEnds;
using System;
using System.Windows.Forms;
using ComSupLibB.SupInfc;
using ComLibB;

namespace HC_Main
{
    public partial class frmHaEndo_Main : Form, MainFormMessage
    {

        string mPara1 = "";

        clsHcFunc cHF = null;
        frmSupEndsSCH01 FrmSupEndsSCH01 = null;
        frmHaEmergencyNarcoticManageList FrmHaEmergencyNarcoticManageList = null;
        frmHaPsychotropicMedSend FrmHaPsychotropicMedSend = null;
        frmSupEndsVIEW02 FrmSupEndsVIEW02 = null;
        frmEnvironmentOrderView FrmEnvironmentOrderView = null;
        frmComSupADRReport FrmComSupADRReport = null;
        frmSupDrstInfoEntryNew FrmSupDrstInfoEntryNew = null;
        frmComSupMayakPrescription FrmComSupMayakPrescription = null;

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


        public frmHaEndo_Main()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHaEndo_Main(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            SetEvent();
            SetControl();
        }

        public frmHaEndo_Main(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {

            cHF = new clsHcFunc();

        }
        private void SetEvent()
        {

            this.Load += new EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.menu_00.Click += new EventHandler(eMenuClick);    //종료
            this.menu_01.Click += new EventHandler(eMenuClick);    //검진접수
            this.menu_02.Click += new EventHandler(eMenuClick);    //일괄가접수
            this.menu_03.Click += new EventHandler(eMenuClick);    //신규수검자 번호생성
            this.menu_04.Click += new EventHandler(eMenuClick);    //생활습관 도구표
            this.menu_05.Click += new EventHandler(eMenuClick);    //야간문진표 작성
            this.menu_06.Click += new EventHandler(eMenuClick);    //C형간염문진표 작성
            this.menu_07.Click += new EventHandler(eMenuClick);    //종검접수
            this.menu_08.Click += new EventHandler(eMenuClick);    //종검접수
        }

        private void eMenuClick(object sender, EventArgs e)
        {

            if (sender == menu_00)    //종료
            {
                this.Close();
                return;
            }

            else if (sender == menu_01)   //
            {
                if (FrmSupEndsSCH01 == null)
                {
                    themTabForm(FrmSupEndsSCH01, this.panMain);
                }
                else
                {
                    if (FormIsExist(FrmSupEndsSCH01) == true)
                    {
                        FormVisiable(FrmSupEndsSCH01);
                    }
                    else
                    {
                        FrmSupEndsSCH01 = new frmSupEndsSCH01();
                        themTabForm(FrmSupEndsSCH01, this.panMain);
                    }
                }
            }
            else if (sender == menu_02)   //
            {
                if (cHF.OpenForm_Check("frmHaEmergencyNarcoticManageList") == true)
                {
                    return;
                }

                FrmHaEmergencyNarcoticManageList = new frmHaEmergencyNarcoticManageList();
                FrmHaEmergencyNarcoticManageList.ShowDialog();
                cHF.fn_ClearMemory(FrmHaEmergencyNarcoticManageList);
            }
            else if (sender == menu_03)   //종검향정약품관리
            {
                if (cHF.OpenForm_Check("frmHaPsychotropicMedSend") == true)
                {
                    return;
                }

                FrmHaPsychotropicMedSend = new frmHaPsychotropicMedSend();
                FrmHaPsychotropicMedSend.ShowDialog();
                cHF.fn_ClearMemory(FrmHaPsychotropicMedSend);
            }
            else if (sender == menu_04)   //내시경 검체장부 관리
            {
                if (cHF.OpenForm_Check("frmSupEndsVIEW02") == true)
                {
                    return;
                }

                FrmSupEndsVIEW02 = new frmSupEndsVIEW02();
                FrmSupEndsVIEW02.ShowDialog();
                cHF.fn_ClearMemory(FrmSupEndsVIEW02);
            }
            else if (sender == menu_05)   //환경 미생물 오더확인
            {
                if (cHF.OpenForm_Check("frmEnvironmentOrderView	") == true)
                {
                    return;
                }

                FrmEnvironmentOrderView = new frmEnvironmentOrderView();
                FrmEnvironmentOrderView.ShowDialog();
                cHF.fn_ClearMemory(FrmEnvironmentOrderView);
            }
            else if (sender == menu_06)   //약물이상반응(ADR) 발생 보고서
            {
                if (cHF.OpenForm_Check("frmComSupADRReport") == true)
                {
                    return;
                }

                FrmComSupADRReport = new frmComSupADRReport();
                FrmComSupADRReport.ShowDialog();
                cHF.fn_ClearMemory(FrmComSupADRReport);
            }
            else if (sender == menu_07)   //약품 정보 등록
            {
                if (cHF.OpenForm_Check("frmSupDrstInfoEntryNew") == true)
                {
                    return;
                }

                FrmSupDrstInfoEntryNew = new frmSupDrstInfoEntryNew();
                FrmSupDrstInfoEntryNew.ShowDialog();
                cHF.fn_ClearMemory(FrmSupDrstInfoEntryNew);
            }
            else if (sender == menu_08)   //마약처방전 인쇄
            {
                if (cHF.OpenForm_Check("frmComSupMayakPrescription") == true)
                {
                    return;
                }

                FrmComSupMayakPrescription = new frmComSupMayakPrescription();
                FrmComSupMayakPrescription.ShowDialog();
                cHF.fn_ClearMemory(FrmComSupMayakPrescription);
            }

        }

        /// <summary>
        /// Main 폼에서 폼이 로드된 경우
        /// </summary>
        /// <param name="frm"></param>
        /// <returns></returns>
        private bool FormIsExist(Form frm)
        {
            foreach (Control ctl in this.panMain.Controls)
            {
                if (ctl is Form)
                {
                    if (ctl.Name == frm.Name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void FormVisiable(Form frm)
        {
            frm.Visible = false;
            frm.Visible = true;
            frm.BringToFront();
        }

        public void themTabForm(Form frm, Control pControl)
        {
            frm.Owner = this;
            frm.TopLevel = false;
            this.Controls.Add(frm);
            frm.Parent = pControl;
            frm.ControlBox = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Top = 0;
            frm.Left = 0;
            frm.WindowState = FormWindowState.Normal;
            frm.Height = pControl.Height;
            frm.Width = pControl.Width;
            frm.Dock = DockStyle.Fill;
            frm.Show();
        }

        private void eFormLoad(object sender, EventArgs e)
        {

            if (FrmSupEndsSCH01 == null)
            {
                FrmSupEndsSCH01 = new frmSupEndsSCH01();
                themTabForm(FrmSupEndsSCH01, this.panMain);
            }
            else
            {
                if (FormIsExist(FrmSupEndsSCH01) == true)
                {
                    FormVisiable(FrmSupEndsSCH01);
                }
                else
                {
                    if (FrmSupEndsSCH01 == null)
                    {
                        FrmSupEndsSCH01 = new frmSupEndsSCH01();
                    }
                    themTabForm(FrmSupEndsSCH01, this.panMain);
                }
            }
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
    }
}
