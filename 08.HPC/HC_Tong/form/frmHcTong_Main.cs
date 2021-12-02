using ComBase;
using ComBase.Mvc;
using System;
using System.Windows.Forms;

namespace HC_Tong
{
    public partial class frmHcTong_Main :BaseForm, MainFormMessage
    {

        #region Form List Declare to frmHcTong_Main
        frmHcDailySuipTong frmHcDailySuipTong = null;                                       // 건강검진 수납 집계표(경리과통보용)
        frmHcDailySummary frmHcDailySummary = null;                                         // 일별 검진종류별 수입통계
        frmHcCreditCardReport frmHcCreditCardReport = null;                                 // 카드사별 수입 집계표
        frmHcPersonnelTotal frmHcPersonnelTotal = null;                                     // 전년도 대비 인원통계
        frmHcCancerClassify frmHcCancerClassify = null;                                     // 암검사 분류별 인원 통계
        frmHcGeneralCheckupIntroduction frmHcGeneralCheckupIntroduction = null;             // 종합검진 직원소개 현황
        frmHcDailyilzi frmHcDailyilzi = null;                                               // 일별 업무일지(통계기준)
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

        public frmHcTong_Main()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcTong_Main(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            SetEvent();
            SetControl();
        }

        public frmHcTong_Main(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            frmHcDailySuipTong = new frmHcDailySuipTong();
            frmHcDailySummary = new frmHcDailySummary();
            frmHcCreditCardReport = new frmHcCreditCardReport();
            frmHcPersonnelTotal = new frmHcPersonnelTotal();
            frmHcCancerClassify = new frmHcCancerClassify();
            frmHcGeneralCheckupIntroduction = new frmHcGeneralCheckupIntroduction();
            frmHcDailyilzi = new frmHcDailyilzi();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);

            this.Job1_1.Click += new EventHandler(eMenuClick);
            this.Job1_2.Click += new EventHandler(eMenuClick);
            this.Job1_3.Click += new EventHandler(eMenuClick);

            this.Job2_1.Click += new EventHandler(eMenuClick);
            this.Job2_2.Click += new EventHandler(eMenuClick);
            this.Job2_3.Click += new EventHandler(eMenuClick);
            this.Job2_4.Click += new EventHandler(eMenuClick);
            this.Job2_5.Click += new EventHandler(eMenuClick);
            this.Job_Exit.Click += new EventHandler(eMenuClick);

            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
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

        private void eFormActivated(object sender, EventArgs e)
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

        private void eMenuClick(object sender, EventArgs e)
        {
            if (sender == Job_Exit)
            {
                this.Close();
                return;
            }
            // 1번
            else if(sender == Job1_1)
            {
                if (frmHcDailySuipTong == null)
                {
                    frmHcDailySuipTong = new frmHcDailySuipTong();
                    frmHcDailySuipTong.Show();
                }
                else
                {
                    if (chkOpenedForm(frmHcDailySuipTong.Name))
                    {
                        FormVisiable(frmHcDailySuipTong);
                    }
                    else
                    {
                        frmHcDailySuipTong = new frmHcDailySuipTong();
                        frmHcDailySuipTong.Show();
                    }
                }
            }
            // 2번
            else if (sender == Job1_2)
            {
                if (frmHcDailySummary == null)
                {
                    frmHcDailySummary = new frmHcDailySummary();
                    frmHcDailySummary.Show();
                }
                else
                {
                    if (chkOpenedForm(frmHcDailySummary.Name))
                    {
                        FormVisiable(frmHcDailySummary);
                    }
                    else
                    {
                        frmHcDailySummary = new frmHcDailySummary();
                        frmHcDailySummary.Show();
                    }
                }
            }
            // 3번
            else if (sender == Job1_3)
            {
                if (frmHcCreditCardReport == null)
                {
                    frmHcCreditCardReport = new frmHcCreditCardReport();
                    frmHcCreditCardReport.Show();
                }
                else
                {
                    if (chkOpenedForm(frmHcCreditCardReport.Name))
                    {
                        FormVisiable(frmHcCreditCardReport);
                    }
                    else
                    {
                        frmHcCreditCardReport = new frmHcCreditCardReport();
                        frmHcCreditCardReport.Show();
                    }
                }
            }
            // 4번
            else if (sender == Job2_1)
            {
                if (frmHcPersonnelTotal == null)
                {
                    frmHcPersonnelTotal = new frmHcPersonnelTotal();
                    frmHcPersonnelTotal.Show();
                }
                else
                {
                    if (chkOpenedForm(frmHcPersonnelTotal.Name))
                    {
                        FormVisiable(frmHcPersonnelTotal);
                    }
                    else
                    {
                        frmHcPersonnelTotal = new frmHcPersonnelTotal();
                        frmHcPersonnelTotal.Show();
                    }
                }
            }
            // 5번
            else if (sender == Job2_2)
            {
                if (frmHcCancerClassify == null)
                {
                    frmHcCancerClassify = new frmHcCancerClassify();
                    frmHcCancerClassify.Show();
                }
                else
                {
                    if (chkOpenedForm(frmHcCancerClassify.Name))
                    {
                        FormVisiable(frmHcCancerClassify);
                    }
                    else
                    {
                        frmHcCancerClassify = new frmHcCancerClassify();
                        frmHcCancerClassify.Show();
                    }
                }
            }
            // 6번
            else if (sender == Job2_3)
            {
                if (frmHcGeneralCheckupIntroduction == null)
                {
                    frmHcGeneralCheckupIntroduction = new frmHcGeneralCheckupIntroduction();
                    frmHcGeneralCheckupIntroduction.Show();
                }
                else
                {
                    if (chkOpenedForm(frmHcGeneralCheckupIntroduction.Name))
                    {
                        FormVisiable(frmHcGeneralCheckupIntroduction);
                    }
                    else
                    {
                        frmHcGeneralCheckupIntroduction = new frmHcGeneralCheckupIntroduction();
                        frmHcGeneralCheckupIntroduction.Show();
                    }
                }
            }
            // 6번
            else if (sender == Job2_4)
            {
                if (frmHcDailyilzi == null)
                {
                    frmHcDailyilzi = new frmHcDailyilzi();
                    frmHcDailyilzi.Show();
                }
                else
                {
                    if (chkOpenedForm(frmHcDailyilzi.Name))
                    {
                        FormVisiable(frmHcDailyilzi);
                    }
                    else
                    {
                        frmHcDailyilzi = new frmHcDailyilzi();
                        frmHcDailyilzi.Show();
                    }
                }
            }
            else if (sender == Job2_5)
            {
                if (frmHcDailyilzi == null)
                {
                    frmHcDailyilzi = new frmHcDailyilzi();
                    frmHcDailyilzi.Show();
                }
                else
                {
                    if (chkOpenedForm(frmHcDailyilzi.Name))
                    {
                        FormVisiable(frmHcDailyilzi);
                    }
                    else
                    {
                        frmHcDailyilzi = new frmHcDailyilzi();
                        frmHcDailyilzi.Show();
                    }
                }
            }
        }

        private void FormVisiable(Form frm)
        {
            frm.BringToFront();
            frm.Show();
            frm.Focus();
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

        private void eFormload(object sender, EventArgs e)
        {
            
        }
    }
}
