using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Drawing;
using FarPoint.Win.Spread;
using System.Text;
using System.ComponentModel;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcSchoolMain.cs
/// Description     : 학생신검 등록 및 출력
/// Author          : 이상훈
/// Create Date     : 2019-09-21
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "MDIForm1.frm(hschool00)" />

namespace HC_Pan
{
    public partial class frmHcSchoolMain : Form
    {
        

        ComFunc cF = null;
        clsHcMain hm = null;
        clsHcFunc hc = null;
        clsHaBase hb = null;
        clsHcAct ha = null;
        clsHcVariable hv = null;
        clsHcFunc cHF = null;

        frmHcSchoolExamPanjeng FrmHcSchoolExamPanjeng = null;
        frmHcSchoolTeethPrint FrmHcSchoolTeethPrint = null;
        frmHcSchoolResultMunjinPrint FrmHcSchoolResultMunjinPrint = null;
        frmHcSchoolExamResult FrmHcSchoolExamResult = null;
        frmHcSchoolStatic FrmHcSchoolStatic = null;
        frmHcSchoolStaticB FrmHcSchoolStaticB = null;
        frmHcSchoolStudentStatic FrmHcSchoolStudentStatic = null;
        frmHcSchoolStudentPhysicalDevStatic FrmHcSchoolStudentPhysicalDevStatic = null;
        frmHcSchoolOpinionAfterMgmtPrint FrmHcSchoolOpinionAfterMgmtPrint = null;
        frmHcSchoolChargeExpenses_New FrmHcSchoolChargeExpenses_New = null;
        frmHcSchoolChargeExpenses FrmHcSchoolChargeExpenses = null;
        frmHcSchoolJepsuViewPrint FrmHcSchoolJepsuViewPrint = null;
        //frmHcSFile FrmHcSFile = null; // 학생검진 결과 파일생성
        frmHcSchoolCommonDistrictRegView FrmHcSchoolCommonDistrictRegView = null;
        frmHcSchoolBmiCal FrmHcSchoolBmiCal = null;

        long FnWrtNo;
        string FstrIpAddress;
        string FstrPtno;

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

        public frmHcSchoolMain()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcSchoolMain(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            SetEvent();
            SetControl();
        }

        public frmHcSchoolMain(MainFormMessage pform, long nWrtNo)
        {
            InitializeComponent();
            mCallForm = pform;
            FnWrtNo = nWrtNo;
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
           

            this.Load += new EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.menuExit.Click += new EventHandler(eMenuClick);    //종료
            this.Menu01.Click += new EventHandler(eMenuClick);      //학생신검  =================================
            this.Menu01_01.Click += new EventHandler(eMenuClick);   //1.학생검진판정
            this.Menu01_02.Click += new EventHandler(eMenuClick);   //2.구강문진 및 결과 인쇄
            this.Menu01_03.Click += new EventHandler(eMenuClick);   //3.학생문진 및 결과 인쇄
            this.Menu01_03_1.Click += new EventHandler(eMenuClick); //3-1. 학생문진 및 결과 인쇄(통합)
            this.Menu01_04.Click += new EventHandler(eMenuClick);   //4.학생검진 통계표(A)
            this.Menu01_04_1.Click += new EventHandler(eMenuClick); //4-1.학생검진 통계표(B)
            this.Menu01_05.Click += new EventHandler(eMenuClick);   //5.학생검진 통계표(남녀별)
            this.Menu01_06.Click += new EventHandler(eMenuClick);   //6.학생신체발달상황 통계표
            this.Menu01_07.Click += new EventHandler(eMenuClick);   //7.학생 사후관리 인쇄
            this.Menu01_08.Click += new EventHandler(eMenuClick);   //8.학생검진 비용청구서
            this.Menu01_08_1.Click += new EventHandler(eMenuClick); //8-1.학생검진 비용청구서(구)
            this.Menu01_09.Click += new EventHandler(eMenuClick);   //접수자 조회

            this.Menu02.Click += new EventHandler(eMenuClick);      //파일생성 =================================
            this.Menu03.Click += new EventHandler(eMenuClick);      //상용구등록
            this.Menu04.Click += new EventHandler(eMenuClick);      //비만도

        }

        void SetControl()
        {
            cF = new ComFunc();
            hm = new clsHcMain();
            hc = new clsHcFunc();
            hb = new clsHaBase();
            ha = new clsHcAct();
            hv = new clsHcVariable();
            cHF = new clsHcFunc();
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
            string strTemp = "";

            ComFunc.ReadSysDate(clsDB.DbCon);
            clsCompuInfo.SetComputerInfo();

            FstrIpAddress = clsCompuInfo.gstrCOMIP;

            hb.READ_HIC_Doctor(clsType.User.IdNumber.To<long>());
            cHF.SET_자료사전_VALUE();

            if (FrmHcSchoolExamPanjeng == null)
            {
                FrmHcSchoolExamPanjeng = new frmHcSchoolExamPanjeng();
                themTabForm(FrmHcSchoolExamPanjeng, this.panMain);
            }
            else
            {
                if (FormIsExist(FrmHcSchoolExamPanjeng) == true)
                {
                    FormVisiable(FrmHcSchoolExamPanjeng);
                }
                else
                {
                    if (FrmHcSchoolExamPanjeng == null)
                    {
                        FrmHcSchoolExamPanjeng = new frmHcSchoolExamPanjeng();
                    }
                    themTabForm(FrmHcSchoolExamPanjeng, this.panMain);
                }
            }

            Menu04.Visible = false;
        }

        private void FormVisiable(Form frm)
        {
            frm.Visible = false;
            frm.Visible = true;
            frm.BringToFront();
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

        void eMenuClick(object sender, EventArgs e)
        {               
            if (sender == menuExit)    //종료
            {
                this.Close();
                return;
            }
            else if (sender == Menu01)      //학생신검
            {
            }

            else if (sender == Menu01_01)   //1.학생검진판정
            {   
                FrmHcSchoolExamPanjeng.Show();
                if (cHF.OpenForm_Check("frmHcSchoolExamPanjeng") == true)
                {
                    return;
                }

                FrmHcSchoolExamPanjeng = new frmHcSchoolExamPanjeng();
                themTabForm(FrmHcSchoolExamPanjeng, this.panMain);
            }
            else if (sender == Menu01_02)   //2.구강문진 및 결과 인쇄
            {
                FrmHcSchoolTeethPrint = new frmHcSchoolTeethPrint();
                FrmHcSchoolTeethPrint.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolTeethPrint.ShowDialog(this);

            }
            else if (sender == Menu01_03)   //3.학생문진 및 결과 인쇄
            {
                FrmHcSchoolResultMunjinPrint = new frmHcSchoolResultMunjinPrint();
                FrmHcSchoolResultMunjinPrint.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolResultMunjinPrint.ShowDialog(this);
            }
            else if (sender == Menu01_03_1) //3-1. 학생문진 및 결과 인쇄(통합)
            {
                FrmHcSchoolExamResult = new frmHcSchoolExamResult();
                FrmHcSchoolExamResult.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolExamResult.ShowDialog(this);
            }
            else if (sender == Menu01_04)   //4.학생검진 통계표(A)
            {
                FrmHcSchoolStatic = new frmHcSchoolStatic();
                FrmHcSchoolStatic.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolStatic.ShowDialog(this);                
            }
            else if (sender == Menu01_04_1) //4-1.학생검진 통계표(B)
            {
                FrmHcSchoolStaticB = new frmHcSchoolStaticB();
                FrmHcSchoolStaticB.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolStaticB.ShowDialog(this);
            }
            else if (sender == Menu01_05)   //5.학생검진 통계표(남녀별)
            {
                FrmHcSchoolStudentStatic = new frmHcSchoolStudentStatic();
                FrmHcSchoolStudentStatic.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolStudentStatic.ShowDialog(this);
            }
            else if (sender == Menu01_06)   //6.학생신체발달상황 통계표
            {
                FrmHcSchoolStudentPhysicalDevStatic = new frmHcSchoolStudentPhysicalDevStatic();
                FrmHcSchoolStudentPhysicalDevStatic.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolStudentPhysicalDevStatic.ShowDialog(this);
            }
            else if (sender == Menu01_07)   //7.학생 사후관리 인쇄
            {
                FrmHcSchoolOpinionAfterMgmtPrint = new frmHcSchoolOpinionAfterMgmtPrint();
                FrmHcSchoolOpinionAfterMgmtPrint.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolOpinionAfterMgmtPrint.ShowDialog(this);
            }
            else if (sender == Menu01_08)   //8.학생검진 비용청구서
            {
                FrmHcSchoolChargeExpenses_New = new frmHcSchoolChargeExpenses_New();
                FrmHcSchoolChargeExpenses_New.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolChargeExpenses_New.ShowDialog(this);
            }
            else if (sender == Menu01_08_1) //8-1.학생검진 비용청구서(구)
            {
                FrmHcSchoolChargeExpenses = new frmHcSchoolChargeExpenses();
                FrmHcSchoolChargeExpenses.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolChargeExpenses.ShowDialog(this);
            }
            else if (sender == Menu01_09)   //접수자 조회
            {
                FrmHcSchoolJepsuViewPrint = new frmHcSchoolJepsuViewPrint();
                FrmHcSchoolJepsuViewPrint.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolJepsuViewPrint.ShowDialog(this);
            }
            else if (sender == Menu02)      //파일생성
            {
                ///TODO : 이상훈 (2020.09.21) - 미개발
                //FrmHcSFile = new frmHcSFile();
                //FrmHcSFile.StartPosition = FormStartPosition.CenterParent;
                //FrmHcSFile.ShowDialog(this);
            }
            else if (sender == Menu03)      //상용구등록
            {
                FrmHcSchoolCommonDistrictRegView = new frmHcSchoolCommonDistrictRegView();
                FrmHcSchoolCommonDistrictRegView.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolCommonDistrictRegView.ShowDialog(this);
            }
            else if (sender == Menu04)      //비만도
            {
                FrmHcSchoolBmiCal = new frmHcSchoolBmiCal();
                FrmHcSchoolBmiCal.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolBmiCal.ShowDialog(this);
            }
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
    }
}
