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
/// Class Name      : HC_Sangdam
/// File Name       : frmHcSangdamMain.cs
/// Description     : 검사결과 등록 / 변경
/// Author          : 이상훈
/// Create Date     : 2019-08-20
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHaActMain.frm(FrmHaActMain)" />

namespace HC_Sangdam
{
    public partial class frmHcSangdamMain : Form, MainFormMessage
    {
        string mPara1 = "";
        HeaJepsuResultService heaJepsuResultService = null;
        ComHpcLibBService comHpcLibBService = null;
        HeaSangdamWaitService heaSangdamWaitService = null;
        HicBcodeService hicBcodeService = null;
        HeaResultService heaResultService = null;
        HicXrayResultService hicXrayResultService = null;
        HeaResvExamService heaResvExamService = null;
        HeaJepsuService heaJepsuService = null;
        BasPcconfigService basPcconfigService = null;

        frmHcSangTotalCounsel FrmHcSangTotalCounsel = null;
        frmHcSangTeeth FrmHcSangTeeth = null;
        frmHcSangStudentTeeth FrmHcSangStudentTeeth = null;
        frmHcSangHyangjengApproval FrmHcSangHyangjengApproval = null;
        frmHcPanInternetMunjin_New FrmHcPanInternetMunjin_New = null;
        frmHcSangLivingHabitPrescriptionModify FrmHcSangLivingHabitPrescriptionModify = null;
        frmHaTeethCounsel FrmHaTeethCounsel = null;
        frmHcSangEndoOrderClose FrmHcSangEndoOrderClose = null;

        ComFunc cF = null;
        clsHcMain hm = null;
        clsHcFunc hc = null;
        clsHaBase hb = null;
        clsHcAct ha = null;
        clsHcVariable hv = null;
        clsHcFunc cHF = null;

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

        public frmHcSangdamMain()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcSangdamMain(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            SetEvent();
            SetControl();
        }

        public frmHcSangdamMain(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            heaJepsuResultService = new HeaJepsuResultService();
            comHpcLibBService = new ComHpcLibBService();
            heaSangdamWaitService = new HeaSangdamWaitService();
            hicBcodeService = new HicBcodeService();
            heaResultService = new HeaResultService();
            hicXrayResultService = new HicXrayResultService();
            heaResvExamService = new HeaResvExamService();
            heaJepsuService = new HeaJepsuService();
            basPcconfigService = new BasPcconfigService();

            this.Load += new EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.menuExit.Click += new EventHandler(eMenuClick);
            this.Menu01.Click += new EventHandler(eMenuClick);      //건강검진상담
            this.Menu01_01.Click += new EventHandler(eMenuClick);   //통합상담프로그램
            this.Menu01_02.Click += new EventHandler(eMenuClick);   //구상상담(일반)
            this.Menu01_03.Click += new EventHandler(eMenuClick);   //구상상담(학생)
            this.Menu01_04.Click += new EventHandler(eMenuClick);   //종합검진 구상상담
            this.Menu02.Click += new EventHandler(eMenuClick);      //향정승인
            this.Menu03.Click += new EventHandler(eMenuClick);      //내시경처방전송
            this.Menu04.Click += new EventHandler(eMenuClick);      //인터넷문진표
            this.Menu05.Click += new EventHandler(eMenuClick);      //생활습관 개선 상담 수정
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

            if (clsHcVariable.GstrDrGbPan == "1")
            {
                if (FrmHcSangTotalCounsel == null)
                {
                    //FrmHcSangTotalCounsel = new frmHcSangTotalCounsel();
                    //FrmHcSangTotalCounsel.WindowState = FormWindowState.Maximized;
                    //FrmHcSangTotalCounsel.Location = new Point(1, 50);
                    //FrmHcSangTotalCounsel.ShowDialog(this);
                    //themTabForm(FrmHcSangTotalCounsel, this.panMain);

                    FrmHcSangTotalCounsel = new frmHcSangTotalCounsel();
                    themTabForm(FrmHcSangTotalCounsel, this.panMain);
                }
                else
                {
                    if (FormIsExist(FrmHcSangTotalCounsel) == true)
                    {
                        FormVisiable(FrmHcSangTotalCounsel);
                    }
                    else
                    {
                        if (FrmHcSangTotalCounsel == null)
                        {
                            FrmHcSangTotalCounsel = new frmHcSangTotalCounsel();
                        }
                        themTabForm(FrmHcSangTotalCounsel, this.panMain);
                    }
                }
            }

            if (clsHcVariable.GstrDrGbDent == "1")
            {
                if (FrmHcSangTeeth == null)
                {
                    FrmHcSangTeeth = new frmHcSangTeeth();
                    FrmHcSangTeeth.WindowState = FormWindowState.Maximized;
                    //FrmHcSangTotalCounsel.Location = new Point(1, 50);
                    FrmHcSangTeeth.ShowDialog(this);
                    //themTabForm(FrmHcSangTeeth, this.panMain);

                    //FrmHcSangTeeth = new frmHcSangTeeth();
                    //themTabForm(FrmHcSangTeeth, this.panMain);
                }
                else
                {
                    if (FormIsExist(FrmHcSangTeeth) == true)
                    {
                        FormVisiable(FrmHcSangTeeth);
                    }
                    else
                    {
                        if (FrmHcSangTeeth == null)
                        {
                            FrmHcSangTeeth = new frmHcSangTeeth();
                        }

                        themTabForm(FrmHcSangTeeth, this.panMain);

                        ////themTabForm(FrmHcSangTeeth, this.panMain);
                        //FrmHcSangTeeth.WindowState = FormWindowState.Maximized;
                        //FrmHcSangTeeth.StartPosition = FormStartPosition.CenterParent;
                        //FrmHcSangTeeth.ShowDialog(this);
                    }
                }
            }
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
            else if (sender == Menu01)
            {

            }
            else if (sender == Menu01_01)   //통합상담프로그램
            {
                if (cHF.OpenForm_Check("frmHcSangTotalCounsel") == true)
                {
                    return;
                }

                if (cHF.OpenForm_Check("frmHcSangStudentTeeth") == true)
                {
                    FrmHcSangStudentTeeth.Close();
                    FrmHcSangStudentTeeth.Dispose();
                    FrmHcSangStudentTeeth = null;
                }

                if (cHF.OpenForm_Check("frmHcSangTeeth") == true)
                {
                    FrmHcSangTeeth.Close();
                    FrmHcSangTeeth.Dispose();
                    FrmHcSangTeeth = null;
                }

                if (cHF.OpenForm_Check("frmHaTeethCounsel") == true)
                {
                    FrmHaTeethCounsel.Close();
                    FrmHaTeethCounsel.Dispose();
                    FrmHaTeethCounsel = null;
                }

                //FrmHcSangTotalCounsel = new frmHcSangTotalCounsel();
                //FrmHcSangTotalCounsel.WindowState = FormWindowState.Maximized;
                //FrmHcSangTotalCounsel.Location = new Point(1, 50);
                //FrmHcSangTotalCounsel.ShowDialog(this);
                //themTabForm(FrmHcSangTotalCounsel, this.panMain);  


                FrmHcSangTotalCounsel = new frmHcSangTotalCounsel();
                themTabForm(FrmHcSangTotalCounsel, this.panMain);

            }
            else if (sender == Menu01_02)       //구상상담(일반)
            {
                if (cHF.OpenForm_Check("frmHcSangTeeth") == true)
                {
                    return;
                }

                if (cHF.OpenForm_Check("frmHcSangTotalCounsel") == true)
                {
                    FrmHcSangTotalCounsel.Close();
                    FrmHcSangTotalCounsel.Dispose();
                    FrmHcSangTotalCounsel = null;
                }

                if (cHF.OpenForm_Check("frmHcSangStudentTeeth") == true)
                {
                    FrmHcSangStudentTeeth.Close();
                    FrmHcSangStudentTeeth.Dispose();
                    FrmHcSangStudentTeeth = null;
                }

                if (cHF.OpenForm_Check("frmHaTeethCounsel") == true)
                {
                    FrmHaTeethCounsel.Close();
                    FrmHaTeethCounsel.Dispose();
                    FrmHaTeethCounsel = null;
                }

                FrmHcSangTeeth = new frmHcSangTeeth();
                FrmHcSangTeeth.WindowState = FormWindowState.Maximized;
                FrmHcSangTeeth.StartPosition = FormStartPosition.CenterParent;
                FrmHcSangTeeth.ShowDialog(this);

                //FrmHcSangTeeth = new frmHcSangTeeth();
                //themTabForm(FrmHcSangTeeth, this.panMain);

            }
            else if (sender == Menu01_03)   //구상상담(학생)
            {
                if (cHF.OpenForm_Check("frmHcSangStudentTeeth") == true)
                {
                    return;
                }

                if (cHF.OpenForm_Check("frmHcSangTotalCounsel") == true)
                {
                    FrmHcSangTotalCounsel.Close();
                    FrmHcSangTotalCounsel.Dispose();
                    FrmHcSangTotalCounsel = null;
                }

                if (cHF.OpenForm_Check("frmHcSangTeeth") == true)
                {
                    FrmHcSangTeeth.Close();
                    FrmHcSangTeeth.Dispose();
                    FrmHcSangTeeth = null;
                }

                if (cHF.OpenForm_Check("frmHaTeethCounsel") == true)
                {
                    FrmHaTeethCounsel.Close();
                    FrmHaTeethCounsel.Dispose();
                    FrmHaTeethCounsel = null;
                }

                //FrmHcSangStudentTeeth = new frmHcSangStudentTeeth();
                //FrmHcSangStudentTeeth.WindowState = FormWindowState.Maximized;
                //FrmHcSangStudentTeeth.StartPosition = FormStartPosition.CenterParent;                
                //FrmHcSangStudentTeeth.ShowDialog(this);

                FrmHcSangStudentTeeth = new frmHcSangStudentTeeth();
                themTabForm(FrmHcSangStudentTeeth, this.panMain);
            }
            else if (sender == Menu01_04)   //종합검진 구강상담
            {
                if (cHF.OpenForm_Check("frmHaTeethCounsel") == true)
                {
                    return;
                }

                if (cHF.OpenForm_Check("frmHcSangTotalCounsel") == true)
                {
                    FrmHcSangTotalCounsel.Close();
                    FrmHcSangTotalCounsel.Dispose();
                    FrmHcSangTotalCounsel = null;
                }

                if (cHF.OpenForm_Check("frmHcSangStudentTeeth") == true)
                {
                    FrmHcSangStudentTeeth.Close();
                    FrmHcSangStudentTeeth.Dispose();
                    FrmHcSangStudentTeeth = null;
                }

                if (cHF.OpenForm_Check("frmHcSangTeeth") == true)
                {
                    FrmHcSangTeeth.Close();
                    FrmHcSangTeeth.Dispose();
                    FrmHcSangTeeth = null;
                }

                //FrmHaTeethCounsel = new frmHaTeethCounsel();
                //FrmHaTeethCounsel.WindowState = FormWindowState.Maximized;
                //FrmHaTeethCounsel.StartPosition = FormStartPosition.CenterParent;
                //FrmHaTeethCounsel.ShowDialog(this);

                FrmHaTeethCounsel = new frmHaTeethCounsel();
                themTabForm(FrmHaTeethCounsel, this.panMain);
            }
            else if (sender == Menu02)  //향정처방승인
            {
                //if (cHF.OpenForm_Check("frmHcSangHyangjengApproval") == true)
                //{
                //    return;
                //}

                //FrmHcSangHyangjengApproval = new frmHcSangHyangjengApproval();
                //FrmHcSangHyangjengApproval.StartPosition = FormStartPosition.CenterParent;
                //FrmHcSangHyangjengApproval.ShowDialog(this);
            }
            else if (sender == Menu03)      //내시경처방전송
            {
                FrmHcSangEndoOrderClose = new frmHcSangEndoOrderClose();
                FrmHcSangEndoOrderClose.StartPosition = FormStartPosition.CenterParent;
                FrmHcSangEndoOrderClose.ShowDialog(this);
            }
            else if (sender == Menu04)      //인터넷문진표
            {
                if (cHF.OpenForm_Check("frmHcPanInternetMunjin_New") == true)
                {
                    return;
                }
                FrmHcPanInternetMunjin_New = new frmHcPanInternetMunjin_New(0, "", FstrPtno);
                FrmHcPanInternetMunjin_New.StartPosition = FormStartPosition.CenterParent;
                FrmHcPanInternetMunjin_New.ShowDialog(this);
            }
            else if (sender == Menu05)      //생활습관 개선 상담 수정
            {
                if (cHF.OpenForm_Check("frmHcSangLivingHabitPrescriptionModify") == true)
                {
                    return;
                }
                FrmHcSangLivingHabitPrescriptionModify = new frmHcSangLivingHabitPrescriptionModify();
                FrmHcSangLivingHabitPrescriptionModify.StartPosition = FormStartPosition.CenterParent;
                FrmHcSangLivingHabitPrescriptionModify.ShowDialog(this);
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
