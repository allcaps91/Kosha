using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public partial class frmHcEmrPermission : Form
    {

        Ftpedt ftp = new Ftpedt();

        clsHcMain cHM = null;
        clsHaBase hb = null;
        HIC_PATIENT hPT = null;

        HicPatientService hicPatientService = null;
        HicConsentService hicConsentService = null;
        HicPrivacyAcceptService hicPrivacyAcceptService = null;
        HicPrivacyAcceptNewService hicPrivacyAcceptNewService = null;
        HicJepsuService hicJepsuService = null;
        HeaJepsuService heaJepsuService = null;

        HicResDentalService hicResDentalService = null;
        HicIeMunjinNewService hicIeMunjinNewService = null;
        HicSangdamNewService hicSangdamNewService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicJepsuHeaExjongService hicJepsuHeaExjongService = null;

        #region 동의서작성 추가
        string fstrYear = "";
        string fstrPtno = "";
        string fstrFileName1 = ""; //정보활용동의서
        string fstrFileName2 = ""; //검진동시동의서
        string fstrJepDate = "";

        string FstrFileName;
        int FnFileCnt;
        string FstrDrno;
        string FstrGubun;
        string FstrPtno;
        string FstrSName;
        string FstrDept;
        long FnWRTNO;
        string FstrFormList;
        string[] FstrFilePath = new string[21];
        string FstrCmd = "";
        #endregion

        public frmHcEmrPermission()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        public frmHcEmrPermission(string argGubun)
        {
            InitializeComponent();
            SetControl();
            SetEvent();
            FstrGubun = argGubun;
        }
        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
            //this.ssConsent.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(eSpdBtnClicked);
            this.ssConsent.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void SetControl()
        {
            cHM = new clsHcMain();
            hPT = new HIC_PATIENT();
            hb = new clsHaBase();

        }


        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            long nwrtno = 1200072;

            if (e.Row == 0)
            {
                //개인정보동의서
                frmHcEmrConset_Rec frm = new frmHcEmrConset_Rec(nwrtno, "NUR", "D50");
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.Show();
            }
            else if(e.Row == 1)
            {
                //내시경동의서
            }
            else if (e.Row == 2)
            {
                //정보활용동의서
                frmHcEmrConset_Rec frm = new frmHcEmrConset_Rec(nwrtno, "NUR", "D52");
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.Show();
            }
            else if (e.Row == 3)
            {
                //검진동시진행동의서
                frmHcEmrConset_Rec frm = new frmHcEmrConset_Rec(nwrtno, "NUR", "D53");
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.Show();
            }
            else if (e.Row == 4)
            {
                //건강진단표
                frmHcEmrConset_Rec frm = new frmHcEmrConset_Rec(nwrtno, "NUR", "D54");
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.Show();
            }
        }

        

        private void eFormload(object sender, EventArgs e)
        {
            Screen_Clear();
        }

        public void Screen_Clear()
        {
            for (int i = 0; i < ssConsent.ActiveSheet.RowCount; i++)
            {
                ssConsent.ActiveSheet.Cells[i, 1].Text = "";
            }
        }

        public void SetDisplay(string argPtno, string argYear, string argJepDate, string[] argDept)
        {
            Screen_Clear();
            fstrPtno = argPtno;
            fstrYear = argYear;
            fstrJepDate = argJepDate;

            hPT = hicPatientService.GetPatInfoByPtno(argPtno);

            //개인정보 동의서
            ssConsent.ActiveSheet.Cells[0, 1].Text = hicPatientService.GetPrivacyNewByPtno(argPtno);

            //내시경 동의서
            HIC_CONSENT item = hicConsentService.GetIetmByPtnoSdateDeptForm(argPtno, argJepDate, argDept, "D10");
            if (!item.IsNullOrEmpty())
            {
                ssConsent.ActiveSheet.Cells[1, 1].Text = VB.Left(item.DOCTSIGN.ToString(), 10);
            }

            //정보활용 동의서
            HIC_PRIVACY_ACCEPT item1 = hicPrivacyAcceptService.GetIetmByPtnoYear(argPtno, argYear);
            if (!item1.IsNullOrEmpty())
            {
                fstrFileName1 = item1.FILENAME;
                ssConsent.ActiveSheet.Cells[2, 1].Text = item1.ENTDATE;
            }

            //검진동시 동의서
            HIC_PRIVACY_ACCEPT_NEW item2 = hicPrivacyAcceptNewService.GetIetmByPtnoYear(argPtno, argYear);
            if (!item2.IsNullOrEmpty())
            {
                fstrFileName2 = item2.FILENAME;
                ssConsent.ActiveSheet.Cells[3, 1].Text = item2.ENTDATE;
            }

            //건강진단 개인표
            ssConsent.ActiveSheet.Cells[4, 1].Text = "";

        }
    }
}
