using ComBase;
using ComBase.Mvc;
using ComDbB;
using ComHpcLibB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_Bill
{
    public partial class frmMDI_Admin :BaseForm, MainFormMessage
    {
        string mPara1 = "";

        clsHcFunc cHF = null;

        #region Form List Declare to frmMDI_Admin
        frmHcCode pfrmHcCode = null;            //검진기초코드
        frmHcExCode pfrmHcExCode = null;        //검진검사항목코드
        frmHcSuga pfrmHcSuga = null;            //검진수가 일괄변경
        frmHcResCode pfrmHcResCode = null;      //결과값 코드
        frmHcLtdCode pfrmHcLtdCode = null;      //사업장 코드
        frmHcGroupCode pfrmHcGroupCode = null;  //공단검진 그룹코드
        frmHaGroupCode pfrmHaGroupCode = null;  //종합검진 그룹코드
        frmHcGroupExam pfrmHcGroupExam = null;  //공단검진 그룹코드 검사항목
        frmHaGroupExam pfrmHaGroupExam = null;  //종합검진 그룹코드 검사항목
        frmHcMCodeExam pfrmHcMCodeExam = null;  //취급물질별 그룹코드 검사항목
        frmHcGJong pfrmHcGJong = null;          //일반검진 종류
        frmHaGJong pfrmHaGJong = null;          //종합검진 종류
        frmHaGamCode pfrmHaGamCode = null;      //종검감액 코드
        frmHcMatterEntry pfrmHcMatterEntry = null; //취급물질 코드
        frmHcLtdMCode pfrmHcLtdMCode = null;    //사업장별 취급물질 등록
        frmHcDoctor pfrmHcDoctor = null;        //검진의사 코드등록
        frmHcMisuEntry frmHcMisuEntry = null;   // 미수 자료 등록(기타)
        frmHcPhcMisuAutoFormation frmHcPhcMisuAutoFormation = null;     // 보건소 미수 자동형성
        frmHcIndustrialMisuAutoFormation frmHcIndustrialMisuAutoFormation = null; // 공단 미수 자동형성
        frmHcCheckBill frmHcCheckBill = null;       // 계산서 조회
        frmHcDigitalBill frmHcDigitalBill = null;   // 전자계산서
        #endregion


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

        public frmMDI_Admin()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmMDI_Admin(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            SetEvent();
            SetControl();
        }

        public frmMDI_Admin(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
            SetEvent();
            SetControl();
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

        private void SetControl()
        {
            cHF = new clsHcFunc();
            pfrmHcExCode = new frmHcExCode();
            pfrmHcLtdCode = new frmHcLtdCode();
            frmHcMisuEntry = new frmHcMisuEntry();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
            this.Job1_1.Click += new EventHandler(eMenuClick);
            this.Job1_2.Click += new EventHandler(eMenuClick);
            this.Job1_3.Click += new EventHandler(eMenuClick);
            this.Job1_4.Click += new EventHandler(eMenuClick);
            this.Job1_5.Click += new EventHandler(eMenuClick);
            this.Job1_6_1.Click += new EventHandler(eMenuClick);
            this.Job1_6_2.Click += new EventHandler(eMenuClick);
            this.Job1_7_1.Click += new EventHandler(eMenuClick);
            this.Job1_7_2.Click += new EventHandler(eMenuClick);
            this.Job1_7_3.Click += new EventHandler(eMenuClick);
            this.Job1_8_1.Click += new EventHandler(eMenuClick);
            this.Job1_8_2.Click += new EventHandler(eMenuClick);
            this.Job1_9.Click += new EventHandler(eMenuClick);
            this.Job1_10.Click += new EventHandler(eMenuClick);
            this.Job1_11.Click += new EventHandler(eMenuClick);
            this.Job1_12.Click += new EventHandler(eMenuClick);
            this.Job4_1.Click += new EventHandler(eMenuClick);
            this.Job4_2.Click += new EventHandler(eMenuClick);
            this.Job4_3.Click += new EventHandler(eMenuClick);

            this.Job5_1.Click += new EventHandler(eMenuClick);
            this.Job5_2.Click += new EventHandler(eMenuClick);

            this.Job_Exit.Click += new EventHandler(eMenuClick);
        }

        private void eFormload(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
            cHF.SET_자료사전_VALUE();
        }

        private void eMenuClick(object sender, EventArgs e)
        {
            if (sender == Job1_1)
            {
                if (pfrmHcCode == null)
                {
                    pfrmHcCode = new frmHcCode();
                    pfrmHcCode.Show();
                }
                else
                {
                    if (chkOpenedForm(pfrmHcCode.Name))
                    {
                        FormVisiable(pfrmHcCode);
                    }
                    else
                    {
                        pfrmHcCode = new frmHcCode();
                        pfrmHcCode.Show();
                    }
                }
            }
            else if (sender == Job1_2)
            {
                if (pfrmHcExCode == null)
                {
                    themTabForm(pfrmHcExCode, this.panMain);
                }
                else
                {
                    if (FormIsExist(pfrmHcExCode) == true)
                    {
                        FormVisiable(pfrmHcExCode);
                    }
                    else
                    {
                        pfrmHcExCode = new frmHcExCode();
                        themTabForm(pfrmHcExCode, this.panMain);
                    }
                }
            }
            else if (sender == Job1_3)
            {
                if (pfrmHcSuga == null)
                {
                    pfrmHcSuga = new frmHcSuga();
                    pfrmHcSuga.Show();
                }
                else
                {
                    if (chkOpenedForm(pfrmHcSuga.Name))
                    {
                        FormVisiable(pfrmHcSuga);
                    }
                    else
                    {
                        pfrmHcSuga = new frmHcSuga();
                        pfrmHcSuga.Show();
                    }
                }
            }
            else if (sender == Job1_4)
            {
                if (pfrmHcResCode == null)
                {
                    pfrmHcResCode = new frmHcResCode();
                    pfrmHcResCode.Show();
                }
                else
                {
                    if (chkOpenedForm(pfrmHcResCode.Name))
                    {
                        FormVisiable(pfrmHcResCode);
                    }
                    else
                    {
                        pfrmHcResCode = new frmHcResCode();
                        pfrmHcResCode.Show();
                    }
                }
            }
            else if (sender == Job1_5)
            {
                if (pfrmHcLtdCode == null)
                {
                    themTabForm(pfrmHcLtdCode, this.panMain);
                }
                else
                {
                    if (FormIsExist(pfrmHcLtdCode) == true)
                    {
                        FormVisiable(pfrmHcLtdCode);
                    }
                    else
                    {
                        pfrmHcLtdCode = new frmHcLtdCode();
                        themTabForm(pfrmHcLtdCode, this.panMain);
                    }
                }
            }
            else if (sender == Job1_6_1)
            {
                if (pfrmHcGroupCode == null)
                {
                    pfrmHcGroupCode = new frmHcGroupCode();
                    pfrmHcGroupCode.Show();
                }
                else
                {
                    if (chkOpenedForm(pfrmHcGroupCode.Name))
                    {
                        FormVisiable(pfrmHcGroupCode);
                    }
                    else
                    {
                        pfrmHcGroupCode = new frmHcGroupCode();
                        pfrmHcGroupCode.Show();
                    }
                }
            }
            else if (sender == Job1_6_2)
            {
                if (pfrmHaGroupCode == null)
                {
                    pfrmHaGroupCode = new frmHaGroupCode();
                    pfrmHaGroupCode.Show();
                }
                else
                {
                    if (chkOpenedForm(pfrmHaGroupCode.Name))
                    {
                        FormVisiable(pfrmHaGroupCode);
                    }
                    else
                    {
                        pfrmHaGroupCode = new frmHaGroupCode();
                        pfrmHaGroupCode.Show();
                    }
                }
            }
            else if (sender == Job1_7_1)
            {
                if (pfrmHcGroupExam == null)
                {
                    pfrmHcGroupExam = new frmHcGroupExam();
                    pfrmHcGroupExam.Show();
                }
                else
                {
                    if (chkOpenedForm(pfrmHcGroupExam.Name))
                    {
                        FormVisiable(pfrmHcGroupExam);
                    }
                    else
                    {
                        pfrmHcGroupExam = new frmHcGroupExam();
                        pfrmHcGroupExam.Show();
                    }
                }
            }
            else if (sender == Job1_7_2)
            {
                if (pfrmHaGroupExam == null)
                {
                    pfrmHaGroupExam = new frmHaGroupExam();
                    pfrmHaGroupExam.Show();
                }
                else
                {
                    if (chkOpenedForm(pfrmHaGroupExam.Name))
                    {
                        FormVisiable(pfrmHaGroupExam);
                    }
                    else
                    {
                        pfrmHaGroupExam = new frmHaGroupExam();
                        pfrmHaGroupExam.Show();
                    }
                }
            }
            else if (sender == Job1_7_3)
            {
                if (pfrmHcMCodeExam == null)
                {
                    pfrmHcMCodeExam = new frmHcMCodeExam();
                    pfrmHcMCodeExam.Show();
                }
                else
                {
                    if (chkOpenedForm(pfrmHcMCodeExam.Name))
                    {
                        FormVisiable(pfrmHcMCodeExam);
                    }
                    else
                    {
                        pfrmHcMCodeExam = new frmHcMCodeExam();
                        pfrmHcMCodeExam.Show();
                    }
                }
            }
            else if (sender == Job1_8_1)
            {
                if (pfrmHcGJong == null)
                {
                    pfrmHcGJong = new frmHcGJong();
                    pfrmHcGJong.Show();
                }
                else
                {
                    if (chkOpenedForm(pfrmHcGJong.Name))
                    {
                        FormVisiable(pfrmHcGJong);
                    }
                    else
                    {
                        pfrmHcGJong = new frmHcGJong();
                        pfrmHcGJong.Show();
                    }
                }
            }
            else if (sender == Job1_8_2)
            {
                if (pfrmHaGJong == null)
                {
                    pfrmHaGJong = new frmHaGJong();
                    pfrmHaGJong.Show();
                }
                else
                {
                    if (chkOpenedForm(pfrmHaGJong.Name))
                    {
                        FormVisiable(pfrmHaGJong);
                    }
                    else
                    {
                        pfrmHaGJong = new frmHaGJong();
                        pfrmHaGJong.Show();
                    }
                }
            }
            else if (sender == Job1_9)
            {
                if (pfrmHaGamCode == null)
                {
                    pfrmHaGamCode = new frmHaGamCode();
                    pfrmHaGamCode.Show();
                }
                else
                {
                    if (chkOpenedForm(pfrmHaGamCode.Name))
                    {
                        FormVisiable(pfrmHaGamCode);
                    }
                    else
                    {
                        pfrmHaGamCode = new frmHaGamCode();
                        pfrmHaGamCode.Show();
                    }
                }
            }
            else if (sender == Job1_10)
            {
                if (pfrmHcMatterEntry == null)
                {
                    pfrmHcMatterEntry = new frmHcMatterEntry();
                    pfrmHcMatterEntry.Show();
                }
                else
                {
                    if (chkOpenedForm(pfrmHcMatterEntry.Name))
                    {
                        FormVisiable(pfrmHcMatterEntry);
                    }
                    else
                    {
                        pfrmHcMatterEntry = new frmHcMatterEntry();
                        pfrmHcMatterEntry.Show();
                    }
                }
            }
            else if (sender == Job1_11)
            {
                if (pfrmHcLtdMCode == null)
                {
                    pfrmHcLtdMCode = new frmHcLtdMCode();
                    pfrmHcLtdMCode.Show();
                }
                else
                {
                    if (chkOpenedForm(pfrmHcLtdMCode.Name))
                    {
                        FormVisiable(pfrmHcLtdMCode);
                    }
                    else
                    {
                        pfrmHcLtdMCode = new frmHcLtdMCode();
                        pfrmHcLtdMCode.Show();
                    }
                }
            }
            else if (sender == Job1_12)
            {
                if (pfrmHcDoctor == null)
                {
                    pfrmHcDoctor = new frmHcDoctor();
                    pfrmHcDoctor.Show();
                }
                else
                {
                    if (chkOpenedForm(pfrmHcDoctor.Name))
                    {
                        FormVisiable(pfrmHcDoctor);
                    }
                    else
                    {
                        pfrmHcDoctor = new frmHcDoctor();
                        pfrmHcDoctor.Show();
                    }
                }
            }
            

            else if (sender == Job4_1) 
            {
                if (frmHcMisuEntry == null)
                {
                    themTabForm(frmHcMisuEntry, this.panMain);
                }
                else
                {
                    if (FormIsExist(frmHcMisuEntry) == true)
                    {
                        FormVisiable(frmHcMisuEntry);
                    }
                    else
                    {
                        frmHcMisuEntry = new frmHcMisuEntry();
                        themTabForm(frmHcMisuEntry, this.panMain);
                    }
                }
            }

            else if (sender == Job4_2)
            {
                if (frmHcPhcMisuAutoFormation == null)
                {
                    frmHcPhcMisuAutoFormation = new frmHcPhcMisuAutoFormation();
                    frmHcPhcMisuAutoFormation.Show();
                }
                else
                {
                    if (chkOpenedForm(frmHcPhcMisuAutoFormation.Name))
                    {
                        FormVisiable(frmHcPhcMisuAutoFormation);
                    }
                    else
                    {
                        frmHcPhcMisuAutoFormation = new frmHcPhcMisuAutoFormation();
                        frmHcPhcMisuAutoFormation.Show();
                    }
                }
            }
            else if(sender == Job4_3)
            {
                if (frmHcIndustrialMisuAutoFormation == null)
                {
                    frmHcIndustrialMisuAutoFormation = new frmHcIndustrialMisuAutoFormation();
                    frmHcIndustrialMisuAutoFormation.Show();
                }
                else
                {
                    if (chkOpenedForm(frmHcIndustrialMisuAutoFormation.Name))
                    {
                        FormVisiable(frmHcIndustrialMisuAutoFormation);
                    }
                    else
                    {
                        frmHcIndustrialMisuAutoFormation = new frmHcIndustrialMisuAutoFormation();
                        frmHcIndustrialMisuAutoFormation.Show();
                    }
                }
            }
            else if (sender == Job5_1)
            {
                if (frmHcCheckBill == null)
                {
                    frmHcCheckBill = new frmHcCheckBill();
                    frmHcCheckBill.Show();
                }
                else
                {
                    if (chkOpenedForm(frmHcCheckBill.Name))
                    {
                        FormVisiable(frmHcCheckBill);
                    }
                    else
                    {
                        frmHcCheckBill = new frmHcCheckBill();
                        frmHcCheckBill.Show();
                    }
                }
            }
            else if (sender == Job5_2)
            {
                if (frmHcDigitalBill == null)
                {
                    frmHcDigitalBill = new frmHcDigitalBill();
                    frmHcDigitalBill.Show();
                }
                else
                {
                    if (chkOpenedForm(frmHcDigitalBill.Name))
                    {
                        FormVisiable(frmHcDigitalBill);
                    }
                    else
                    {
                        frmHcDigitalBill = new frmHcDigitalBill();
                        frmHcDigitalBill.Show();
                    }
                }
            }


            else if (sender == Job_Exit)
            {
                this.Close();
                return;
            }
        }

        private bool chkOpenedForm(string frmName)
        {
            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                if (frm.Name == frmName)
                {
                    return true;
                }
            }
            return false;
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
            frm.BringToFront();
            frm.Show();
        }

        private void FormVisiable_MDI(Form frm)
        {
            frm.Visible = false;
            frm.Visible = true;
            frm.BringToFront();
        }

        private void FormVisiable(Form frm)
        {
            frm.BringToFront();
            frm.Show();
            frm.Focus();
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

       
    }
}
