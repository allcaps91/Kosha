using ComBase;
using ComHpcLibB;
using System;
using System.IO;
using System.Windows.Forms;

namespace HC_IF
{
    public partial class frmHcKiosk_Main :Form
    {
        clsHcFunc cHF = null;
        frmHcKiosk fHcKsk = null;
        frmHaKiosk fHaKsk = null;
        frmHcCall_DID fHcDID = null;

        public frmHcKiosk_Main()
        {
            InitializeComponent();
            SetInit();
            SetEvent();
            SerControl();
        }

        private void SetInit()
        {
            clsAuto.GnDBOPEN = 1;

            clsPublic.GstrPassProgramID = "";

            //자동수납용 사번 및 이름 세팅
            clsPublic.GnJobSabun = 111;
            clsPublic.GstrJobName = "키오스크";
            clsPublic.GstrJobPart = "111"; 
        }

        private void SerControl()
        {
            cHF = new clsHcFunc();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnHaKiosk.Click += new EventHandler(eBtnClick);
            this.btnHcKiosk.Click += new EventHandler(eBtnClick);
            this.btnWait_TV.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.timer1.Tick += new EventHandler(eTimer_Tick);
        }

        private void eTimer_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            //대기순번용 PC번호 읽기(예:접수PC번호=5번"
            FileInfo nFILE = null;

            string strFIleNm = @"c:\HIC_WAIT.ini";
            nFILE = new FileInfo(strFIleNm);

            if (nFILE.Exists == false) { return; }

            StreamReader SR = new StreamReader(strFIleNm, System.Text.Encoding.Default);

            string strWaitPcNo = SR.ReadToEnd();

            SR.Close();

            strWaitPcNo = VB.Pstr(strWaitPcNo, "{}", 1);

            switch (strWaitPcNo)
            {
                case "건진키오스크":
                    if (cHF.OpenForm_Check("frmHcKiosk") == true) { return; }

                    fHcKsk = new frmHcKiosk();
                    fHcKsk.StartPosition = FormStartPosition.CenterScreen;
                    fHcKsk.Show();
                    //cHF.fn_ClearMemory(fHcKsk);
                    break;
                case "종검키오스크":
                    if (cHF.OpenForm_Check("frmHaKiosk") == true) { return; }

                    fHaKsk = new frmHaKiosk();
                    fHaKsk.StartPosition = FormStartPosition.CenterScreen;
                    fHaKsk.Show();
                    //cHF.fn_ClearMemory(fHaKsk);
                    break;
                //case "상담대기PC":    frmHcKiosk fHcKsk = new frmHcKiosk(); fHcKsk.ShowDialog(); break;   //사용안함
                case "대기순번TV":
                    if (cHF.OpenForm_Check("frmHcCall_DID") == true) { return; }

                    fHcDID = new frmHcCall_DID();
                    fHcDID.StartPosition = FormStartPosition.CenterScreen;
                    fHcDID.ShowDialog();
                    cHF.fn_ClearMemory(fHcDID);
                    break;
                //case "대형TV":        frmHcKiosk fHcKsk = new frmHcKiosk(); fHcKsk.ShowDialog(); break;
                default: break;
            }

        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnHaKiosk)
            {
                if (cHF.OpenForm_Check("frmHaKiosk") == true) { return; }

                fHaKsk = new frmHaKiosk();
                fHaKsk.StartPosition = FormStartPosition.CenterScreen;
                fHaKsk.ShowDialog();
                cHF.fn_ClearMemory(fHaKsk);
            }
            else if (sender == btnHcKiosk)
            {
                if (cHF.OpenForm_Check("frmHcKiosk") == true) { return; }

                fHcKsk = new frmHcKiosk();
                fHcKsk.StartPosition = FormStartPosition.CenterScreen;
                fHcKsk.ShowDialog();
                cHF.fn_ClearMemory(fHcKsk);
            }
            else if (sender == btnWait_TV)
            {
                if (cHF.OpenForm_Check("frmHcCall_DID") == true) { return; }

                fHcDID = new frmHcCall_DID();
                fHcDID.StartPosition = FormStartPosition.CenterScreen;
                fHcDID.ShowDialog();
                cHF.fn_ClearMemory(fHcDID);
            }

        }

        private void eFormLoad(object sender, EventArgs e)
        {
            cHF.SET_자료사전_VALUE();
        }
    }
}
