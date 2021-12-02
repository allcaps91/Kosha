using ComBase;
using ComBase.Controls;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using HC_IF.Dto;
using System;
using System.Windows.Forms;

namespace HC_IF
{
    public partial class frmHcKiosk_Verify :Form
    {
        ComHpcLibBService comHpcLibBService = null;

        string FstrGbn = string.Empty;
        string FstrKey = string.Empty;
        string FstrJumin = string.Empty;

        public delegate void SetGstrValue(string strJumin);
        public static event SetGstrValue rSetGstrValue;

        public delegate void SetHaGstrValue(HEA_KIOSK haKIOSK);
        public static event SetHaGstrValue rSetHaGstrValue;

        HEA_KIOSK haKiosk = null;
        HEA_KIOSK rtnHaKiosk = null;

        public frmHcKiosk_Verify()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcKiosk_Verify(string argValue1, string argValue2)
        {
            InitializeComponent();
            SetEvent();
            FstrGbn = argValue1;
            FstrKey = argValue2;

            lblJuso.Visible = false;
            lblHphone.Visible = false;
        }

        public frmHcKiosk_Verify(HEA_KIOSK argHaKiosk)
        {
            InitializeComponent();
            SetEvent();
            haKiosk = new HEA_KIOSK();
            haKiosk = argHaKiosk;   
        }


        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnYes.Click += new EventHandler(eBtnClick);
            this.btnNo.Click += new EventHandler(eBtnClick);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnYes)
            {
                panWait.Visible = true;
                Progress.IsRunning = true;

                if (!rSetGstrValue.IsNullOrEmpty())
                {
                    rSetGstrValue(FstrJumin);
                }

                if (!rSetHaGstrValue.IsNullOrEmpty())
                {
                    rSetHaGstrValue(haKiosk);
                }

                panWait.Visible = false;
                Progress.IsRunning = false;

                Application.DoEvents();

            }
            else if (sender == btnNo)
            {
                if (!rSetGstrValue.IsNullOrEmpty())
                {
                    rSetGstrValue("");
                }

                if (!rSetHaGstrValue.IsNullOrEmpty())
                {
                    rtnHaKiosk = new HEA_KIOSK();
                    rtnHaKiosk.INDX = haKiosk.INDX;
                    rSetHaGstrValue(rtnHaKiosk);
                }

            }

            this.Close();
            return;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            panWait.Visible = false;
            Progress.IsRunning = false;

            if (haKiosk.IsNullOrEmpty())
            {
                Display_Confirm_HicKiosk();
            }
            else
            {
                Display_Confirm_HeaKiosk();
            }
            
        }

        private void Display_Confirm_HeaKiosk()
        {
            lblInfo1.Text = haKiosk.SNAME.To<string>("") + "님이 맞습니까?";
            lblTel.Text = haKiosk.LTDNAME;
            lblInfo2.Text = VB.Left(haKiosk.JUMIN, 6) + "-" + VB.Mid(haKiosk.JUMIN, 7, 1) + "******";
            lblJuso.Text = haKiosk.JUSO;
            lblHphone.Text = haKiosk.HPHONE;
        }

        private void Display_Confirm_HicKiosk()
        {
            comHpcLibBService = new ComHpcLibBService();
            COMHPC item = new COMHPC();

            string strTel =string.Empty;

            if (FstrGbn == "예약")
            {
                item = comHpcLibBService.GetPatientInfoByGubunTable("HIC_CANCER_RESV2", FstrKey);
            }
            else if (FstrGbn == "종검")
            {
                item = comHpcLibBService.GetPatientInfoByGubunTable("HIC_PATIENT", FstrKey);
            }
            else
            {
                item = comHpcLibBService.GetPatientInfoByGubunTable("BAS_PATIENT", FstrKey);
            }

            if (!item.IsNullOrEmpty())
            {
                strTel = "";

                if (!item.HPHONE.IsNullOrEmpty())
                {
                    strTel = item.HPHONE + ",";
                }

                if (!item.TEL.IsNullOrEmpty())
                {
                    if (item.TEL.To<string>("").Trim() == "000-0000" || item.TEL.To<string>("").Trim() == "054-000-0000")
                    {

                    }
                    else
                    {
                        strTel += item.TEL;
                    }
                }

                if (VB.Right(strTel, 1) == ",")
                {
                    strTel = VB.Left(strTel, strTel.Length - 1);
                }

                lblInfo1.Text = item.SNAME.To<string>("") + "님이 맞습니까?";
                lblTel.Text = strTel;

                if (FstrGbn == "예약")
                {
                    FstrJumin = clsAES.DeAES(item.JUMIN2.To<string>(""));
                }
                else
                {
                    FstrJumin = item.JUMIN1.To<string>("") + clsAES.DeAES(item.JUMIN3.To<string>(""));
                }

                lblInfo2.Text = VB.Left(FstrJumin, 6) + "-" + VB.Mid(FstrJumin, 7, 1) + "******";

            }
            else
            {
                lblInfo1.Text = "자료가 없습니다.";
                lblTel.Text = "";
                lblInfo2.Text = "";
                btnYes.Enabled = false;
            }
        }
    }
}
