using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HC_Pan
{
    public partial class frmHcPanjeng_Old : Form
    {
        public delegate void SetPanExamResultReg(long argWRTNO);
        public static event SetPanExamResultReg rSetPanExamResultReg;

        /// <summary>
        /// panMain에 판정창 Display 하는 부분 개선하기
        /// </summary>
        HicResultExCodeService hicResultExCodeService = null;
        HicRescodeService hicRescodeService = null;
        HicJepsuService hicJepsuService = null;
        HeaJepsuService heaJepsuService = null;
        HicResultService hicResultService = null;
        HicPatientService hicPatientService = null;
        EtcJupmstService etcJupmstService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicSangdamNewService hicSangdamNewService = null;
        
        List<HC_PANJENG_PATLIST> PatListItemList = null;

        HC_PANJENG_PATLIST PatListItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc   cf = new ComFunc();

        frmHcPanPatList FrmHcPanPatList = null;
        frmPanjeng_Etc FrmPanjeng_Etc = null;
        frmPanjeng_Hic FrmPanjeng_Hic = null;
        frmPanjeng_Spc FrmPanjeng_Spc = null;
        frmHcPanView FrmHcPanView = null;
        frmViewResult FrmViewResult = null;
        frmHeaResult FrmHeaResult = null;
        frmHcSangInternetMunjinView FrmHcSangInternetMunjinView = null;
        frmHcIEMunjinVIew FrmHcIEMunjinVIew = null;
        frmHcSangTotalCounsel FrmHcSangTotalCounsel = null;
        frmHcPanXrayResultInput FrmHcPanXrayResultInput = null;
        frmHcPanMunjin_2019 FrmHcPanMunjin_2019 = null;
        frmHcPanPanjengAdd FrmHcPanPanjengAdd = null;

        long FnWRTNO;
        long FnPano;
        string FstrSex;
        string FstrPano;
        string FstrJepDate;
        string FstrJong;
        string FstrTemp;
        string FstrJumin;
        string FstrSelTab;
        string FstrFrDate;
        string FstrToDate;

        string FstrGjJong;

        int FnSelRow;
        string FJob;

        string FstrUCodes;

        List<string> FstrKind = new List<string>();
        List<long> FstrFnWrtNo = new List<long>();
        List<string> lstEtcKind = new List<string>();
        List<long> lstEtcWrtNo = new List<long>();
        

        long FnSelWrtNo1 = 0;
        long FnSelWrtNo2 = 0;
        long FnSelWrtNo3 = 0;
        long FnSelWrtNo4 = 0;

        string FSelGjJong1 = "";
        string FSelGjJong2 = "";
        string FSelGjJong3 = "";
        string FSelGjJong4 = "";

        string FstrResult_A163 = "";
        string FstrROWID;

        public frmHcPanjeng_Old()
        {
            InitializeComponent();
            SetEvent();
            SetContol();
        }

        void SetEvent()
        {
            hicResultExCodeService = new HicResultExCodeService();
            hicRescodeService = new HicRescodeService();
            hicJepsuService = new HicJepsuService();
            hicResultService = new HicResultService();
            hicPatientService = new HicPatientService();
            etcJupmstService = new EtcJupmstService();
            hicSunapdtlService = new HicSunapdtlService();
            heaJepsuService = new HeaJepsuService();
            hicSangdamNewService = new HicSangdamNewService();

            this.Load += new EventHandler(eFormLoad);
            this.FormClosing += new FormClosingEventHandler(eFormClosing);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnPatList.Click += new EventHandler(eBtnClick);
            this.btnPacs.Click += new EventHandler(eBtnClick);
            this.btnMed.Click += new EventHandler(eBtnClick);
            this.btnSangdam.Click += new EventHandler(eBtnClick);
            this.btnMunjinView.Click += new EventHandler(eBtnClick);
            this.btnIEMunjin.Click += new EventHandler(eBtnClick);
            this.btnAudio.Click += new EventHandler(eBtnClick);
            this.btnAudio1.Click += new EventHandler(eBtnClick);
            this.btnPFT.Click += new EventHandler(eBtnClick);
            this.btnEMR.Click += new EventHandler(eBtnClick);
            this.btnXrayResult.Click += new EventHandler(eBtnClick);
            this.btnMunjin.Click += new EventHandler(eBtnClick);
            this.btnMemo.Click += new EventHandler(eBtnClick);

            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSHistory.CellDoubleClick += new CellClickEventHandler(eSpdDClick);

            this.btnHic1.Click += new EventHandler(eBtnClick);
            this.btnHic2.Click += new EventHandler(eBtnClick);
            this.btnHic3.Click += new EventHandler(eBtnClick);
            this.btnHic4.Click += new EventHandler(eBtnClick);
            this.btnHic5.Click += new EventHandler(eBtnClick);
            this.btnSave_UcodeAdd.Click += new EventHandler(eBtnClick);
            this.btnExamResultReg.Click += new EventHandler(ePanExamResultReg);
        }

        private void eAddPanResSave(long argWRTNO)
        {
            string strCODE = "";
            string strResult = "";
            string strROWID = "";
            int result = 0;

            if (argWRTNO.IsNullOrEmpty() || argWRTNO == 0) { return; }

            if (SS2.ActiveSheet.RowCount == 0) { return; }

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                strCODE = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                strResult = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                strROWID = SS2.ActiveSheet.Cells[i, 9].Text.Trim();

                if (strResult.IsNullOrEmpty() || strResult == "") { strResult = "."; }

                if (!strResult.IsNullOrEmpty())
                {
                    //결과를 저장
                    result = hicResultService.UpdateResultPanjengResCodeActivebyRowId(strResult, "", "", clsType.User.IdNumber, clsPublic.GstrSysDate + ' ' + clsPublic.GstrSysTime, strROWID);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show(i + 1 + "번줄 검사결과를 등록중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);

            //접수마스타의 상태를 변경
            hm.Result_EntryEnd_Check(argWRTNO);

        }

        private void ePanExamResultReg(object sender, EventArgs e)
        {
            rSetPanExamResultReg(clsHcVariable.GnWRTNO);
        }

        private void eFormClosing(object sender, FormClosingEventArgs e)
        {
            this.btnExamResultReg.Click -= new EventHandler(ePanExamResultReg);
            frmPanjeng_Etc.rSetEventResSave -= new frmPanjeng_Etc.SetEventResSave(eAddPanResSave);
        }

        void SetContol()
        {
            int nWidth = Screen.PrimaryScreen.Bounds.Width;     //모니터 스크린 가로크기
            int nHeight = Screen.PrimaryScreen.Bounds.Height;   //모니터 스크린 세로크기

            if (nWidth > 1280)
            {
                expSplit.Expanded = true;
            }
            else
            {
                expSplit.Expanded = false;
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSHistory)
            {
                if (string.Compare(SSHistory.ActiveSheet.Cells[e.Row, 3].Text, "2") <= 0)
                {
                    FrmHcPanView = new frmHcPanView(SSHistory.ActiveSheet.Cells[e.Row, 2].Text.Trim().To<long>()); //WrtNo
                    FrmHcPanView.StartPosition = FormStartPosition.CenterScreen;
                    FrmHcPanView.ShowDialog(this);
                }
                else if (string.Compare(SSHistory.ActiveSheet.Cells[e.Row, 3].Text, "3") <= 0)
                {
                    clsPublic.GstrRetValue = SSHistory.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                }
            }
        }

        private void rSaveEventClosed(string sRtn)
        {
            if (!sRtn.IsNullOrEmpty())
            {
                fn_Screen_Clear();

                fn_PatListLoad();
            }
        }

        void fn_Screen_Clear()
        {
            clsHcType.HcMain_First_AutoPanjeng_Clear();
            fn_TFA_Clear();
            sp.Spread_All_Clear(SSHistory);
            fn_ExamKind_Clear();
            btnHic_Color_Clear();
            conHcPatInfo1.SetDisPlay("25420", "O", clsPublic.GstrSysDate, "", "", "");
            FstrSelTab = "";

            panFuncBtn.Enabled = false;
            panJob.Enabled = false;
        }

        void btnHic_Color_Clear()
        {
            btnHic1.BackColor = Color.White;
            btnHic2.BackColor = Color.White;
            btnHic3.BackColor = Color.White;
            btnHic4.BackColor = Color.White;
            btnHic5.BackColor = Color.White;

            btnHic1.ForeColor = Color.Black;
            btnHic2.ForeColor = Color.Black;
            btnHic3.ForeColor = Color.Black;
            btnHic4.ForeColor = Color.Black;
            btnHic5.ForeColor = Color.Black;

            btnHic1.Enabled = false;
            btnHic2.Enabled = false;
            btnHic3.Enabled = false;
            btnHic4.Enabled = false;
            btnHic5.Enabled = false;
            btnSave_UcodeAdd.Enabled = false;
        }

        bool FormIsExist(Type tp)
        {
            foreach (Form ff in this.MdiChildren)
            {
                if (ff.GetType() == tp)
                {
                    ff.Focus();
                    ff.BringToFront();
                    return true;
                }

            }

            return false;
        }

        void fn_TFA_Clear()
        {
            clsHcType.TFA.Panjeng = "";
            for (int i = 0; i <= 9; i++)
            {
                clsHcType.TFA.PanB[i] = false;
            }
            for (int i = 0; i <= 4; i++)
            {
                clsHcType.TFA.PanC[i] = false;
            }
            for (int i = 0; i <= 11; i++)
            {
                clsHcType.TFA.PanR[i] = false;
            }
            for (int i = 0; i <= 3; i++)
            {
                clsHcType.TFA.PanU[i] = false;
            }

            clsHcType.TFA.Sogen = "";
            clsHcType.TFA.SogenB = "";
            clsHcType.TFA.SogenC = "";
            clsHcType.TFA.SogenD = "";
            clsHcType.TFA.Liver = 0;
        }

        void eFormLoad(object sender, EventArgs e)
        {
            clsHcVariable.GStrPanGjJong = "";
            clsHcVariable.GstrPanFrDate = "";
            clsHcVariable.GstrPanToDate = "";
            clsHcVariable.GstrPanDrNo = "";
            
            fn_Screen_Clear();

            if (clsHcVariable.GnHicLicense > 0)
            {
                btnMemo.Visible = true;
                btnExamResultReg.Visible = false;
                btnSave_UcodeAdd.Visible = true;
            }
            else 
            {
                btnMemo.Visible = false;
                btnExamResultReg.Visible = true;
                btnSave_UcodeAdd.Visible = false;
            }

            if (!PatListItem.IsNullOrEmpty())
            {
                conHcPatInfo1.SetDisPlay("25420", "O", PatListItem.JEPDATE, string.Format("{0:00000000}", PatListItem.PTNO.To<long>()), "HR", PatListItem.GJJONG);

                FstrJepDate = PatListItem.JEPDATE;
                FstrSex = PatListItem.SEX;
                FstrJong = PatListItem.GJJONG;
                FstrPano = PatListItem.PANO.To<string>();
                FstrJumin = clsAES.DeAES(PatListItem.JUMIN2);
                FJob = PatListItem.JOB;

                FstrUCodes = PatListItem.UCODES;
            }

            fn_PatListLoad();   //기본 수검자명단 PopUp

            //청력 결과 존재여부
            if (etcJupmstService.GetCountbyPtNoBDate(FstrPano, FstrJepDate, "6") > 0)
            {
                btnAudio.Enabled = true;
            }

            //PFT결과 존재여부
            if (etcJupmstService.GetCountbyPtNoBDate(FstrPano, FstrJepDate, "4") > 0)
            {
                btnPFT.Enabled = true;
            }
        }

        /// <summary>
        /// 수검자 명단 PopUp Main Logic
        /// </summary>
        void fn_PatListLoad()
        {
            sp.Spread_All_Clear(SS2);
            SS2.ActiveSheet.RowCount = 50;
            SS2.ActiveSheet.Cells[0, 0, SS2.ActiveSheet.RowCount - 1, SS2.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
            //panPanjeng = null;

            FrmHcPanPatList = new frmHcPanPatList();
            FrmHcPanPatList.rSetPatItem += new frmHcPanPatList.SetPatItem(PatInfo_Value);
            FrmHcPanPatList.ShowDialog();
            FrmHcPanPatList.rSetPatItem -= new frmHcPanPatList.SetPatItem(PatInfo_Value);

            if (!PatListItem.IsNullOrEmpty())
            {
                clsHcVariable.GnWRTNO = PatListItem.WRTNO;

                conHcPatInfo1.SetDisPlay("25420", "O", PatListItem.JEPDATE, string.Format("{0:00000000}", long.Parse(PatListItem.PTNO)), "HR", PatListItem.GJJONG); //인적사항을 Display
                hm.ExamResult_RePanjeng(PatListItem.WRTNO, PatListItem.SEX, PatListItem.JEPDATE, "");   //검사결과를 재판정
                fn_Exam_Result_Display(PatListItem.WRTNO, PatListItem.JEPDATE, string.Format("{0:00000000}", long.Parse(PatListItem.PTNO)), PatListItem.SEX);   //검사항목을 Display
                fn_OLD_Result_Display(PatListItem.SEX); //종전결과 3개를 Display
                //판정 Display Main Set
                fn_ExamKindSet(PatListItem.GWRTNO, PatListItem.JEPDATE, PatListItem.PTNO, PatListItem.UCODES, PatListItem.WRTNO);

                //문진표 뷰어 활성화라면 ...
                if (PatListItem.MUN == "Y")
                {
                    //상담내역이 있는지 점검
                    HIC_SANGDAM_NEW list2 = hicSangdamNewService.GetItembyWrtNo(clsHcVariable.GnWRTNO);

                    if (!list2.IsNullOrEmpty())
                    {
                        FstrROWID = list2.RID;
                    }
                    else
                    {
                        FstrROWID = "";
                    }

                    //검진문진뷰어
                    //DirectoryInfo dir = new DirectoryInfo(clsHcVariable.Hic_Mun_EXE_PATH);
                    DirectoryInfo dir = new DirectoryInfo(@"C:\Program Files\SamOmr\");
                    if (dir.Exists == true)
                    {
                        //Process.Start(clsHcVariable.Hic_Mun_EXE_PATH, FstrPtno); 
                        VB.Shell(clsHcVariable.Hic_Mun_EXE_PATH + " " + PatListItem.PTNO, "NormalFocus");
                    }
                    else
                    {
                        //DirectoryInfo dir1 = new DirectoryInfo(clsHcVariable.Hic_Mun_EXE_PATH_64);
                        DirectoryInfo dir1 = new DirectoryInfo(@"C:\Program Files (x86)\SamOmr\");
                        if (dir1.Exists == true)
                        {
                            //Process.Start(clsHcVariable.Hic_Mun_EXE_PATH_64, FstrPtno);
                            VB.Shell(clsHcVariable.Hic_Mun_EXE_PATH_64 + " " + PatListItem.PTNO, "NormalFocus");
                        }
                    }

                    //인터넷문진표(New)
                    hf.Diplay_IE_Munjin(FrmHcSangInternetMunjinView, "frmHcSangInternetMunjinView", clsHcVariable.GnWRTNO, FstrJepDate, PatListItem.PTNO, FstrGjJong, FstrROWID);
                    //hf.Diplay_IE_Munjin_New(FrmHcIEMunjinVIew, FstrJepDate, PatListItem.PTNO, FstrGjJong);
                }

                panFuncBtn.Enabled = true;
                panJob.Enabled = true;
            }

            fn_Genjin_History_SET();    //검진 HISTORY
        }

        void fn_ExamKind_Clear()
        {
            for (int i = 1; i < 4; i++)
            {
                Button btnHic = (Controls.Find("btnHic" + i.ToString(), true)[0] as Button);
                btnHic.Enabled = false;
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                this.Dispose();
                return;
            }
            else if (sender == btnPatList)
            {
                fn_Screen_Clear();
                fn_PatListLoad();
            }
            else if (sender == btnPacs)
            {
                FrmViewResult = new frmViewResult(PatListItem.PTNO);
                FrmViewResult.ShowDialog(this);
            }
            else if (sender == btnMed)
            {
                FrmHeaResult = new frmHeaResult(FstrJumin);
                FrmHeaResult.ShowDialog(this);
            }
            else if (sender == btnSangdam)
            {
                Form frm = hf.OpenForm_Check_Return("frmHcSangTotalCounsel");
                if (!frm.IsNullOrEmpty())
                {
                    frm.Dispose();
                }

                FrmHcSangTotalCounsel = new frmHcSangTotalCounsel(PatListItem.WRTNO, true);
                FrmHcSangTotalCounsel.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSangTotalCounsel.Show();
            }
            else if (sender == btnMunjinView)
            {
                DirectoryInfo dir = new DirectoryInfo(@"C:\Program Files\SamOmr\");
                if (dir.Exists == true)
                {
                    //Process.Start(clsHcVariable.Hic_Mun_EXE_PATH, FstrPtno);
                    VB.Shell(clsHcVariable.Hic_Mun_EXE_PATH + " " + PatListItem.PTNO, "NormalFocus");
                }
                else
                {
                    DirectoryInfo dir1 = new DirectoryInfo(@"C:\Program Files (x86)\SamOmr\");
                    if (dir1.Exists == true)
                    {
                        //Process.Start(clsHcVariable.Hic_Mun_EXE_PATH_64, FstrPtno);
                        VB.Shell(clsHcVariable.Hic_Mun_EXE_PATH_64 + " " + PatListItem.PTNO, "NormalFocus");
                    }
                }
            }
            else if (sender == btnIEMunjin)
            {
                //상담내역이 있는지 점검
                HIC_SANGDAM_NEW list2 = hicSangdamNewService.GetItembyWrtNo(PatListItem.WRTNO);

                if (!list2.IsNullOrEmpty())
                {
                    FstrROWID = list2.RID;
                }

                //인터넷문진표(New)
                hf.Diplay_IE_Munjin(FrmHcSangInternetMunjinView, "frmHcSangInternetMunjinView", clsHcVariable.GnWRTNO, FstrJepDate, PatListItem.PTNO, FstrGjJong, FstrROWID);
                //hf.Diplay_IE_Munjin_New(FrmHcIEMunjinVIew, FstrJepDate, PatListItem.PTNO, FstrGjJong);
            }
            else if (sender == btnAudio)
            {
                string strRowId = "";

                ETC_JUPMST list = etcJupmstService.GetRowIdbyPtNoBDate(PatListItem.PTNO, PatListItem.JEPDATE);

                if (!list.IsNullOrEmpty())
                {
                    strRowId = list.ROWID;
                    hf.AudioFILE_DBToFile1(strRowId, "1", 1);
                }
            }
            else if (sender == btnAudio1)
            {
                string strRowId = "";
                string strFileName = "";

                //기존화일 삭제
                strFileName = @"c:\cmc\팀파노이미지_0.jpg";

                DirectoryInfo dir = new DirectoryInfo(strFileName);
                if (dir.Exists == true)
                {
                    FileInfo[] files = dir.GetFiles();
                    foreach (FileInfo F in files)
                    {
                        if (F.Extension == ".jpg")
                        {
                            F.Delete();
                        }
                    }
                }

                ETC_JUPMST list = etcJupmstService.GetRowIdbyPtNoBDateOrerCode(PatListItem.PTNO, PatListItem.JEPDATE);

                if (!list.IsNullOrEmpty())
                {
                    strRowId = list.ROWID;
                    hf.AudioFILE_DBToFile1(strRowId, "1", 1);
                }
            }
            else if (sender == btnPFT)
            {
                string strFileName = "";
                string strRowId = "";
                //폐기능은 여러건을 볼수 있어록 처리 합니다.

                //기존화일 삭제
                strFileName = @"c:\cmc\팀파노이미지_0.jpg";

                DirectoryInfo dir = new DirectoryInfo(strFileName);
                if (dir.Exists == true)
                {
                    FileInfo[] files = dir.GetFiles();
                    foreach (FileInfo F in files)
                    {
                        if (F.Extension == ".jpg")
                        {
                            F.Delete();
                        }
                    }
                }

                List<ETC_JUPMST> list = etcJupmstService.GetRowIdbyPtNoBDateList(PatListItem.PTNO, PatListItem.JEPDATE);

                if (!list.IsNullOrEmpty())
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        strRowId = list[i].ROWID;
                        hf.AudioFILE_DBToFile1(strRowId, "1", i);
                    }
                }
            }
            else if (sender == btnEMR)
            {
                clsVbEmr.EXECUTE_NewTextEmrView(PatListItem.PTNO);
            }
            else if (sender == btnSave_UcodeAdd)
            {
                FrmHcPanPanjengAdd = new frmHcPanPanjengAdd(clsHcVariable.GnWRTNO);
                FrmHcPanPanjengAdd.ShowDialog(this);
                if (FstrSelTab == "SPC")
                {
                    eBtnClick(btnHic3, new EventArgs());
                }
                else if (FstrSelTab == "REC")
                {
                    eBtnClick(btnHic4, new EventArgs());
                }
                
            }
            else if (sender == btnHic1 || sender == btnHic2 || sender == btnHic3 || sender == btnHic4 || sender == btnHic5)
            {
                this.prgBar.BeginInvoke(new System.Action(() => this.prgBar.Visible = true));
                this.prgBar.BeginInvoke(new System.Action(() => this.prgBar.IsRunning = true));

                btnSave_UcodeAdd.Enabled = false;

                if (sender == btnHic1)
                {
                    #region 공단검진
                    if (!PatListItem.IsNullOrEmpty())
                    {
                        FstrSelTab = "HIC";
                        clsHcVariable.GnWRTNO = FnSelWrtNo1;
                        FrmPanjeng_Hic = new frmPanjeng_Hic(FnSelWrtNo1, PatListItem.SNAME, PatListItem.JOB, PatListItem.LTDCODE, PatListItem.JONGGUMYN);

                        if (FormIsExist(FrmPanjeng_Hic.GetType()))
                        {
                            FrmPanjeng_Hic.Dispose();
                            return;
                        }
                        else
                        {
                            hm.ExamResult_RePanjeng(FnSelWrtNo1, PatListItem.SEX, PatListItem.JEPDATE, "");   //검사결과를 재판정
                            fn_Exam_Result_Display(FnSelWrtNo1, PatListItem.JEPDATE, string.Format("{0:00000000}", long.Parse(PatListItem.PTNO)), PatListItem.SEX);   //검사항목을 Display
                            fn_OLD_Result_Display(PatListItem.SEX); //종전결과 3개를 Display

                            FrmPanjeng_Hic.Owner = this;
                            FrmPanjeng_Hic.TopLevel = false;
                            this.Controls.Add(FrmPanjeng_Hic);
                            FrmPanjeng_Hic.Parent = panPanjeng;
                            FrmPanjeng_Hic.Text = "";
                            FrmPanjeng_Hic.ControlBox = false;
                            FrmPanjeng_Hic.FormBorderStyle = FormBorderStyle.None;
                            FrmPanjeng_Hic.Top = 0;
                            FrmPanjeng_Hic.Left = 0;
                            FrmPanjeng_Hic.WindowState = FormWindowState.Normal;
                            FrmPanjeng_Hic.Height = panPanjeng.Height;
                            FrmPanjeng_Hic.Width = panPanjeng.Width;

                            FrmPanjeng_Hic.rSaveEventClosed += new frmPanjeng_Hic.SaveEventClosed(rSaveEventClosed);
                            FrmPanjeng_Hic.Show();
                            FrmPanjeng_Hic.BringToFront();
                            //FrmPanjeng_Hic.rSaveEventClosed -= new frmPanjeng_Hic.SaveEventClosed(rSaveEventClosed);
                        }
                    }
                    #endregion                    
                }
                else if (sender == btnHic2)
                {
                    #region 일특특수판정
                    if (!PatListItem.IsNullOrEmpty())
                    {
                        FstrSelTab = "SPC";
                        btnSave_UcodeAdd.Enabled = true;
                        clsHcVariable.GnWRTNO = FnSelWrtNo2;
                        FrmPanjeng_Spc = new frmPanjeng_Spc(FnSelWrtNo2, PatListItem, PatListItemList, SS2);

                        if (FormIsExist(FrmPanjeng_Spc.GetType()))
                        {
                            FrmPanjeng_Spc.Dispose();
                            return;
                        }
                        else
                        {
                            hm.ExamResult_RePanjeng(FnSelWrtNo2, PatListItem.SEX, PatListItem.JEPDATE, "");   //검사결과를 재판정
                            fn_Exam_Result_Display(FnSelWrtNo2, PatListItem.JEPDATE, string.Format("{0:00000000}", long.Parse(PatListItem.PTNO)), PatListItem.SEX);   //검사항목을 Display
                            fn_OLD_Result_Display(PatListItem.SEX); //종전결과 3개를 Display

                            FrmPanjeng_Spc.Owner = this;
                            FrmPanjeng_Spc.TopLevel = false;
                            this.Controls.Add(FrmPanjeng_Spc);
                            FrmPanjeng_Spc.Parent = panPanjeng;
                            FrmPanjeng_Spc.Text = "";
                            FrmPanjeng_Spc.ControlBox = false;
                            FrmPanjeng_Spc.FormBorderStyle = FormBorderStyle.None;
                            FrmPanjeng_Spc.Top = 0;
                            FrmPanjeng_Spc.Left = 0;
                            FrmPanjeng_Spc.WindowState = FormWindowState.Normal;
                            FrmPanjeng_Spc.Height = panPanjeng.Height;
                            FrmPanjeng_Spc.Width = panPanjeng.Width;

                            FrmPanjeng_Spc.rSaveEventClosed += new frmPanjeng_Spc.SaveEventClosed(rSaveEventClosed);
                            FrmPanjeng_Spc.Show();
                            FrmPanjeng_Spc.BringToFront();
                            //FrmPanjeng_Spc.rSaveEventClosed -= new frmPanjeng_Spc.SaveEventClosed(rSaveEventClosed);
                        }
                    }
                    #endregion                    
                }
                else if (sender == btnHic3)
                {
                    #region 특수판정
                    if (!PatListItem.IsNullOrEmpty())
                    {
                        FstrSelTab = "SPC";
                        btnSave_UcodeAdd.Enabled = true;
                        clsHcVariable.GnWRTNO = FnSelWrtNo3;
                        FrmPanjeng_Spc = new frmPanjeng_Spc(FnSelWrtNo3, PatListItem, PatListItemList, SS2);

                        if (FormIsExist(FrmPanjeng_Spc.GetType()))
                        {
                            FrmPanjeng_Spc.Dispose();
                            return;
                        }
                        else
                        {
                            hm.ExamResult_RePanjeng(FnSelWrtNo3, PatListItem.SEX, PatListItem.JEPDATE, "");   //검사결과를 재판정
                            fn_Exam_Result_Display(FnSelWrtNo3, PatListItem.JEPDATE, string.Format("{0:00000000}", long.Parse(PatListItem.PTNO)), PatListItem.SEX);   //검사항목을 Display
                            fn_OLD_Result_Display(PatListItem.SEX); //종전결과 3개를 Display

                            FrmPanjeng_Spc.Owner = this;
                            FrmPanjeng_Spc.TopLevel = false;
                            this.Controls.Add(FrmPanjeng_Spc);
                            FrmPanjeng_Spc.Parent = panPanjeng;
                            FrmPanjeng_Spc.Text = "";
                            FrmPanjeng_Spc.ControlBox = false;
                            FrmPanjeng_Spc.FormBorderStyle = FormBorderStyle.None;
                            FrmPanjeng_Spc.Top = 0;
                            FrmPanjeng_Spc.Left = 0;
                            FrmPanjeng_Spc.WindowState = FormWindowState.Normal;
                            FrmPanjeng_Spc.Height = panPanjeng.Height;
                            FrmPanjeng_Spc.Width = panPanjeng.Width;

                            FrmPanjeng_Spc.rSaveEventClosed += new frmPanjeng_Spc.SaveEventClosed(rSaveEventClosed);
                            FrmPanjeng_Spc.Show();
                            FrmPanjeng_Spc.BringToFront();
                            //FrmPanjeng_Spc.rSaveEventClosed -= new frmPanjeng_Spc.SaveEventClosed(rSaveEventClosed);
                        }
                    }
                    #endregion                    
                }
                else if (sender == btnHic4)
                {
                    #region 채용판정
                    if (!PatListItem.IsNullOrEmpty())
                    {
                        FstrSelTab = "REC";
                        btnSave_UcodeAdd.Enabled = true;
                        clsHcVariable.GnWRTNO = FnSelWrtNo4;
                        FrmPanjeng_Spc = new frmPanjeng_Spc(FnSelWrtNo4, PatListItem, PatListItemList, SS2);

                        if (FormIsExist(FrmPanjeng_Spc.GetType()))
                        {
                            FrmPanjeng_Spc.Dispose();
                            return;
                        }
                        else
                        {
                            hm.ExamResult_RePanjeng(FnSelWrtNo4, PatListItem.SEX, PatListItem.JEPDATE, "");   //검사결과를 재판정
                            fn_Exam_Result_Display(FnSelWrtNo4, PatListItem.JEPDATE, string.Format("{0:00000000}", long.Parse(PatListItem.PTNO)), PatListItem.SEX);   //검사항목을 Display
                            fn_OLD_Result_Display(PatListItem.SEX); //종전결과 3개를 Display

                            FrmPanjeng_Spc.Owner = this;
                            FrmPanjeng_Spc.TopLevel = false;
                            this.Controls.Add(FrmPanjeng_Spc);
                            FrmPanjeng_Spc.Parent = panPanjeng;
                            FrmPanjeng_Spc.Text = "";
                            FrmPanjeng_Spc.ControlBox = false;
                            FrmPanjeng_Spc.FormBorderStyle = FormBorderStyle.None;
                            FrmPanjeng_Spc.Top = 0;
                            FrmPanjeng_Spc.Left = 0;
                            FrmPanjeng_Spc.WindowState = FormWindowState.Normal;
                            FrmPanjeng_Spc.Height = panPanjeng.Height;
                            FrmPanjeng_Spc.Width = panPanjeng.Width;

                            FrmPanjeng_Spc.rSaveEventClosed += new frmPanjeng_Spc.SaveEventClosed(rSaveEventClosed);
                            FrmPanjeng_Spc.Show();
                            FrmPanjeng_Spc.BringToFront();
                            //FrmPanjeng_Spc.rSaveEventClosed -= new frmPanjeng_Spc.SaveEventClosed(rSaveEventClosed);
                        }
                    }
                    #endregion                    
                }
                else if (sender == btnHic5)
                {
                    #region 기타검진처방
                    if (!PatListItem.IsNullOrEmpty())
                    {
                        FstrSelTab = "ETC";
                        clsHcVariable.GnWRTNO = lstEtcWrtNo[0];
                        FrmPanjeng_Etc = new frmPanjeng_Etc(lstEtcWrtNo, PatListItem.PTNO, PatListItem.JUMIN2, PatListItem.PANO, PatListItem.JEPDATE, PatListItem.SEX, PatListItem.EXNAME, lstEtcKind, PatListItemList, PatListItem.JOB);

                        if (FormIsExist(FrmPanjeng_Etc.GetType()))
                        {
                            FrmPanjeng_Etc.Dispose();
                            return;
                        }
                        else
                        {
                            hm.ExamResult_RePanjeng(lstEtcWrtNo, PatListItem.SEX, PatListItem.JEPDATE, "");   //검사결과를 재판정
                            fn_Exam_Result_Display(lstEtcWrtNo, PatListItem.JEPDATE, string.Format("{0:00000000}", long.Parse(PatListItem.PTNO)), PatListItem.SEX);   //검사항목을 Display
                            fn_OLD_Result_Display(PatListItem.SEX); //종전결과 3개를 Display

                            FrmPanjeng_Etc.Owner = this;
                            FrmPanjeng_Etc.TopLevel = false;
                            this.Controls.Add(FrmPanjeng_Etc);
                            FrmPanjeng_Etc.Parent = panPanjeng;
                            FrmPanjeng_Etc.Text = "";
                            FrmPanjeng_Etc.ControlBox = false;
                            FrmPanjeng_Etc.FormBorderStyle = FormBorderStyle.None;
                            FrmPanjeng_Etc.Top = 0;
                            FrmPanjeng_Etc.Left = 0;
                            FrmPanjeng_Etc.WindowState = FormWindowState.Normal;
                            FrmPanjeng_Etc.Height = panPanjeng.Height;
                            FrmPanjeng_Etc.Width = panPanjeng.Width;

                            frmPanjeng_Etc.rSetEventResSave += new frmPanjeng_Etc.SetEventResSave(eAddPanResSave);
                            //FrmPanjeng_Etc.rSaveEventClosed += new frmPanjeng_Etc.SaveEventClosed(rSaveEventClosed);
                            FrmPanjeng_Etc.Show();
                            FrmPanjeng_Etc.BringToFront();
                        }
                    }
                    #endregion
                }

                this.prgBar.BeginInvoke(new System.Action(() => this.prgBar.Visible = false));
                this.prgBar.BeginInvoke(new System.Action(() => this.prgBar.IsRunning = false));
            }
            else if (sender == btnXrayResult)
            {
                if (clsHcVariable.GnWRTNO > 0)
                {
                    FrmHcPanXrayResultInput = new frmHcPanXrayResultInput(clsHcVariable.GnWRTNO);
                    FrmHcPanXrayResultInput.ShowDialog(this);
                }
            }
            else if (sender == btnMunjin)
            {
                FrmHcPanMunjin_2019 = new frmHcPanMunjin_2019(FnSelWrtNo1);
                FrmHcPanMunjin_2019.ShowDialog(this);
            }
            else if (sender == btnMemo)
            {
                frmHcMemo frm = new frmHcMemo(PatListItem.PTNO);
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// 검진 History
        /// </summary>
        void fn_Genjin_History_SET()
        {
            int nRead = 0;
            string strJong = "";
            long nHeaPano = 0;

            if (PatListItem.IsNullOrEmpty()) return;

            if (PatListItem.JUMIN2.IsNullOrEmpty()) return;

            //if (FstrJumin.IsNullOrEmpty()) return;

            //종검의 등록번호를 찾음
            nHeaPano = 0;

            nHeaPano = hicPatientService.GetPanobyJumin(PatListItem.JUMIN2);

            //일반건진, 종합검진의 접수내역을 Display
            List<HIC_JEPSU> list = hicJepsuService.GetItembyOnlyPaNo(FnPano, nHeaPano);

            nRead = list.Count;
            SSHistory.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                strJong = list[i].GJJONG.Trim();

                SSHistory.ActiveSheet.Cells[i, 0].Text = list[i].JEPDATE;
                if (strJong == "XX")
                {
                    SSHistory.ActiveSheet.Cells[i, 1].Text = "종검";
                }
                else
                {
                    SSHistory.ActiveSheet.Cells[i, 1].Text = hb.READ_GjJong_Name(strJong);
                }
                SSHistory.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.To<string>();
                SSHistory.ActiveSheet.Cells[i, 3].Text = list[i].GJCHASU;
            }
            Application.DoEvents();
        }

        #region 검진종류 Setting
        /// <summary>
        /// 검진종류 Setting
        /// </summary>
        /// <param name="argGWrtNo"></param>
        void fn_ExamKindSet(long argGWrtNo, string argJepDate, string argPtNo, string argUCodes, long argWrtNo)
        {
            bool bPanYN = false;

            List<string> strPtNo = new List<string>();
            List<long> nLifeWrtNo = new List<long>();
            List<HIC_JEPSU> list = new List<HIC_JEPSU>();
            List<HEA_JEPSU> listHea = new List<HEA_JEPSU>();

            strPtNo.Clear();
            nLifeWrtNo.Clear();
            FstrKind.Clear();
            FstrFnWrtNo.Clear();
            lstEtcWrtNo.Clear();
            lstEtcKind.Clear();

            clsHcVariable.GstrPanLifeTab0 = "";
            clsHcVariable.GstrPanLifeTab1 = "";
            clsHcVariable.GstrPanLifeTab2 = "";
            clsHcVariable.GstrPanLifeTab3 = "";
            clsHcVariable.GstrPanLifeTab4 = "";

            clsHcVariable.GnWRTNO = 0;

            FnWRTNO = 0;
            FnSelWrtNo1 = 0;
            FnSelWrtNo2 = 0;
            FnSelWrtNo3 = 0;
            FnSelWrtNo4 = 0;

            btnHic_Color_Clear();

            Application.DoEvents();

            if (!argGWrtNo.IsNullOrEmpty() && argGWrtNo != 0)
            {
                list = hicJepsuService.GetGjJongbyGWrtNo(argGWrtNo, argJepDate);
            }
            else
            {
                list = hicJepsuService.GetUnionGjJongbyWrtNo(argWrtNo, argJepDate);
            }

            if (!list.IsNullOrEmpty())
            {
                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        //기타검진 검사결과를 한번에 보여주기 위해 접수번호를 별도로 집계
                        if (list[i].GJJONG == "50" || list[i].GJJONG == "51" || list[i].GJJONG == "62" || list[i].GJJONG == "69")
                        {
                            if (list[i].GJJONG == "69")
                            {
                                if (clsHcVariable.GnHicLicense > 0)
                                {
                                    if (list[i].GBADDPAN == "Y")
                                    {
                                        FstrKind.Add(list[i].GJJONG);
                                        FstrFnWrtNo.Add(list[i].WRTNO);
                                    }
                                }
                                else
                                {
                                    FstrKind.Add(list[i].GJJONG);
                                    FstrFnWrtNo.Add(list[i].WRTNO);
                                }
                            }
                            else
                            {
                                FstrKind.Add(list[i].GJJONG);
                                FstrFnWrtNo.Add(list[i].WRTNO);
                            }
                        }
                        else
                        {
                            FstrKind.Add(list[i].GJJONG);
                            FstrFnWrtNo.Add(list[i].WRTNO);
                        }
                    }

                    for (int i = 0; i < FstrKind.Count; i++)
                    {
                        switch (FstrKind[i])
                        {
                            case "11":
                            case "14":
                                btnHic1.Enabled = true;//일반검진
                                btnHic1.BackColor = Color.Aqua;
                                List<HIC_JEPSU> listJepsu = hicJepsuService.GetWrtNobyJepDatePtNo(argPtNo, argJepDate);

                                if (!listJepsu.IsNullOrEmpty())
                                {
                                    for (int j = 0; j < listJepsu.Count; j++)
                                    {
                                        nLifeWrtNo.Add(listJepsu[j].WRTNO);
                                    }
                                }

                                if (!list[i].UCODES.IsNullOrEmpty())
                                {
                                    btnHic2.Enabled = true;//특수검진
                                    btnHic2.BackColor = Color.Aqua;
                                    FnSelWrtNo2 = FstrFnWrtNo[i];
                                    FSelGjJong2 = FstrKind[i];

                                    bPanYN = hm.fn_PanjengYN(FnSelWrtNo2);
                                    if (bPanYN) { btnHic2.BackColor = Color.DarkSeaGreen; }
                                    else { btnHic2.BackColor = Color.LightCoral; }
                                }

                                FnSelWrtNo1 = FstrFnWrtNo[i];
                                FSelGjJong1 = FstrKind[i];

                                bPanYN = hm.fn_PanjengYN(FnSelWrtNo1);
                                if (bPanYN) { btnHic1.BackColor = Color.DarkSeaGreen; }
                                else { btnHic1.BackColor = Color.LightCoral; }
                                break;
                            case "16":
                            case "23":
                            case "28":
                                btnHic3.Enabled = true;//특수검진
                                btnHic3.BackColor = Color.Aqua;
                                FnSelWrtNo3 = FstrFnWrtNo[i];
                                FSelGjJong3 = FstrKind[i];

                                bPanYN = hm.fn_PanjengYN(FnSelWrtNo3);
                                if (bPanYN) { btnHic3.BackColor = Color.DarkSeaGreen; }
                                else { btnHic3.BackColor = Color.LightCoral; }

                                break;
                            case "21":
                            case "22":
                            case "24":
                            case "25":
                            case "26":
                            case "27":
                            case "29":
                            case "30":
                            case "33":
                            case "49":
                                btnHic4.Enabled = true;//채용검진
                                btnHic4.BackColor = Color.Aqua;
                                //btnSave_UcodeAdd.Enabled = true;
                                FnSelWrtNo4 = FstrFnWrtNo[i];
                                FSelGjJong4 = FstrKind[i];

                                bPanYN = hm.fn_PanjengYN(FnSelWrtNo4);
                                if (bPanYN) { btnHic4.BackColor = Color.DarkSeaGreen; }
                                else { btnHic4.BackColor = Color.LightCoral; }
                                break;
                            case "62":  //혈액종합검진
                            case "69":  //회사추가검진
                            case "51":  //방사선종사자1차
                            case "50":  //방사선종사자2차
                                btnHic5.Enabled = true; //기타
                                btnHic5.BackColor = Color.Aqua;
                                lstEtcWrtNo.Add(FstrFnWrtNo[i]);
                                lstEtcKind.Add(FstrKind[i]);

                                bPanYN = hm.fn_PanjengYN(lstEtcWrtNo);
                                if (bPanYN) { btnHic5.BackColor = Color.DarkSeaGreen; }
                                else { btnHic5.BackColor = Color.LightCoral; }
                                break;
                            default:
                                break;
                        }
                    }

                    switch (PatListItem.SELECTTAB)
                    {
                        case "HIC":
                            eBtnClick(btnHic1, new EventArgs());
                            break;
                        case "REC":
                            switch (FstrKind[0])
                            {
                                case "11":
                                case "14":
                                    eBtnClick(btnHic1, new EventArgs());
                                    break;
                                case "16":
                                case "23":
                                case "28":
                                    eBtnClick(btnHic3, new EventArgs());
                                    break;
                                case "21":
                                case "22":
                                case "24":
                                case "25":
                                case "26":
                                case "27":
                                
                                case "29":
                                case "30":
                                case "33":
                                case "49":
                                    eBtnClick(btnHic4, new EventArgs());
                                    break;
                                case "62":  //혈액종합검진
                                case "69":  //회사추가검진
                                case "51":  //방사선종사자1차
                                case "50":  //방사선종사자2차
                                    eBtnClick(btnHic5, new EventArgs());
                                    break;
                                default:
                                    break;
                            }                            
                            break;
                        case "SPC":
                            eBtnClick(btnHic3, new EventArgs());
                            break;
                        case "ETC":
                            eBtnClick(btnHic5, new EventArgs());
                            break;
                        default:
                            //접수번호 입력후 Display 한 경우
                            switch (FstrKind[0])
                            {
                                case "11":
                                case "14":
                                    eBtnClick(btnHic1, new EventArgs());
                                    break;
                                case "16":
                                case "23":
                                case "28":
                                    eBtnClick(btnHic3, new EventArgs());
                                    break;
                                case "21":
                                case "22":
                                case "24":
                                case "25":
                                case "26":
                                case "27":
                                case "29":
                                case "33":
                                case "49":
                                    eBtnClick(btnHic4, new EventArgs());
                                    break;
                                case "62":  //혈액종합검진
                                case "69":  //회사추가검진
                                case "51":  //방사선종사자1차
                                case "50":  //방사선종사자2차
                                    eBtnClick(btnHic5, new EventArgs());
                                    break;
                                default:
                                    break;
                            }
                            break;
                    }
                }
            }
        }
        #endregion

        void fn_Exam_Result_Display(long argWrtNo, string argJepDate, string argPtNo, string argSex)
        {
            long nRead = 0;
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strExPan = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strNomal = "";
            List<string> strInExCode = new List<string>();    //위수면내시경
            string[] strList;
            string strResName = "";
            string strRemark = "";

            List<HIC_RESULT_EXCODE> list = new List<HIC_RESULT_EXCODE>();

            SS2_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "이전결과1";
            SS2_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "이전결과2";

            strInExCode.Clear();
            strInExCode.Add("ZE47");
            strInExCode.Add("TX20");

            sp.Spread_All_Clear(SS2);
            Application.DoEvents();

            list = hicResultExCodeService.GetItemCounselbyWrtNo(argWrtNo);

            nRead = list.Count;
            SS2.ActiveSheet.RowCount = list.Count;

            for (int i = 0; i < nRead; i++)
            {
                strExCode = list[i].EXCODE;
                strResult = list[i].RESULT;
                strResCode = list[i].RESCODE;
                strResultType = list[i].RESULTTYPE;
                strGbCodeUse = list[i].GBCODEUSE;

                if (strExCode == "A163")
                {
                    FstrResult_A163 = strResult;
                }

                SS2.ActiveSheet.Cells[i, 0].Text = list[i].EXCODE;
                SS2.ActiveSheet.Cells[i, 1].Text = list[i].HNAME;
                if (!strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[i, 2].Text = strResult.Replace("\n", "");
                }
                
                //비만도
                if (strExCode == "A103")
                {
                    strResCode = "065";
                }

                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        strResName = hb.READ_ResultName(strResCode, strResult);
                        if (strResName.IsNullOrEmpty())
                        {
                            strResName = strResult;
                        }

                        if (!strResName.IsNullOrEmpty())
                        {
                            if (strResName.Length > 7)
                            {
                                strRemark += "▷" + list[i].HNAME + ":";
                                strRemark += strResName + "\r\n";
                            }
                        }
                    }
                }
                else if (!strResult.IsNullOrEmpty())
                {
                    if (strResult.Length > 7)
                    {
                        strRemark += "▷" + list[i].HNAME + ":";
                        strRemark += strResName + "\r\n";
                    }
                }

                if (list[i].PANJENG == "2")
                {
                    SS2.ActiveSheet.Cells[i, 5].Text = "*";
                }

                //Combo_set
                //자료를 READ
                List<HIC_RESCODE> list2 = hicRescodeService.GetCodeNamebyBindGubun(strResCode);
                int nCnt = list2.Count;

                //Array.Resize(ref strList, nCnt);
                strList = new string[nCnt];

                //for (int k = 0; k < list2.Count; k++) { strList[k] = ""; }

                if (list2.Count > 0)
                {
                    FarPoint.Win.Spread.CellType.ComboBoxCellType combo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

                    combo.Clear();

                    for (int j = 0; j < list2.Count; j++)
                    {
                        strList[j] = list2[j].CODE + "." + list2[j].NAME;
                    }

                    SS2.ActiveSheet.Cells[i, 2].Text = "";
                    combo.Items = strList;
                    //combo.ItemData = strList;
                    combo.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
                    combo.MaxDrop = list2.Count;
                    combo.MaxLength = 100;
                    combo.ListWidth = 200;
                    combo.Editable = false;
                    SS2.ActiveSheet.Cells[i, 2].CellType = combo;
                    SS2.ActiveSheet.Cells[i, 2].Locked = false;
                    //SS2.ActiveSheet.Cells[i, 2].Text = "";

                    if (!strResCode.IsNullOrEmpty() && !strResult.IsNullOrEmpty())
                    {
                        HIC_RESCODE list3 = hicRescodeService.GetCodeNamebyGubunCode(strResCode, strResult);

                        if (!list3.IsNullOrEmpty())
                        {
                            for (int k = 0; k < combo.Items.Length; k++)
                            {
                                combo.ListOffset = k;
                                if (combo.Items[k].ToString() == (list3.CODE.Trim() + "." + list3.NAME.Trim()).ToString())
                                {
                                    SS2.ActiveSheet.Cells[i, 2].Text = combo.Items[k].ToString();
                                    //break;
                                }
                            }
                        }
                    }
                }

                if (strResCode.IsNullOrEmpty() || strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[i, 2].Text = strResult;
                }

                SS2.ActiveSheet.Cells[i, 2].Locked = true;

                Application.DoEvents();

                SS2.ActiveSheet.Cells[i, 2].ForeColor = Color.FromArgb(0, 0, 0);

                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[i, 5].Text = hb.READ_ResultName(strResCode, strResult);
                    }
                }

                strExPan = list[i].PANJENG;

                //야간작업 검사결과가 비정상이면 R로 처리
                if (strExCode == "TZ72" || strExCode == "TZ85" || strExCode == "TZ86")
                {
                    if (strResult == "비정상") { strExPan = "R"; }
                }

                //참고치를 Dispaly
                strNomal = hm.EXAM_NomalValue_SET(strExCode, argJepDate, argSex, list[i].MIN_M, list[i].MAX_M, list[i].MIN_F, list[i].MAX_F);
                SS2.ActiveSheet.Cells[i, 5].Text = strExPan;
                SS2.ActiveSheet.Cells[i, 6].Text = strNomal;

                SS2.ActiveSheet.Cells[i, 7].Text = strResCode;

                if (list[i].EXCODE.Trim() == "A151")
                {
                    SS2.ActiveSheet.Cells[i, 7].Text = "007";

                    string strResult1 = strResult;
                }

                if (list[i].EXCODE.Trim() == "TH01" || list[i].EXCODE.Trim() == "TH02")
                {
                    SS2.ActiveSheet.Cells[i, 7].Text = "022";
                }

                SS2.ActiveSheet.Cells[i, 8].Text = "";
                SS2.ActiveSheet.Cells[i, 9].Text = list[i].RID;
                SS2.ActiveSheet.Cells[i, 10].Text = list[i].RESULTTYPE;
                SS2.ActiveSheet.Cells[i, 13].Text = list[i].EXSORT;
                //판정결과별 바탕색상을 다르게 표시함
                switch (strExPan)
                {
                    case "B":
                        SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222); //정상B
                        break;
                    case "C":
                        SS2.ActiveSheet.Cells[i, 2].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));  //주의C
                        break;
                    case "R":
                        SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 170, 170); //질환의심(R)
                        break;
                    default:
                        SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(190, 250, 220); //정상A 또는 기타
                        break;
                }

                if (list[i].GROUPCODE.To<string>("").Trim() != "1160" && (strExCode == "A123" || strExCode == "A242" || strExCode == "A241" || strExCode == "C404"))
                {
                    //SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(250, 210, 222); //정상
                    SS2.ActiveSheet.Cells[i, 1].BackColor = Color.PaleGoldenrod;
                }
            }

            #region 강제 Sort
            //검사결과의 판정값이 R,B인것을 위에 표시 
            SortInfo[] si = new SortInfo[] 
            {
                new SortInfo(5, false)
              //, new SortInfo(0, true)
              //, new SortInfo(13, true)
            };
            
            SS2_Sheet1.SortRows(0, SS2_Sheet1.RowCount, si);
            #endregion
        }

        void fn_Exam_Result_Display(List<long> argWrtNo, string argJepDate, string argPtNo, string argSex)
        {
            long nRead = 0;
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strExPan = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strNomal = "";
            List<string> strInExCode = new List<string>();    //위수면내시경
            string[] strList;
            string strResName = "";
            string strRemark = "";

            List<HIC_RESULT_EXCODE> list = new List<HIC_RESULT_EXCODE>();

            SS2_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "이전결과1";
            SS2_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "이전결과2";

            strInExCode.Clear();
            strInExCode.Add("ZE47");
            strInExCode.Add("TX20");

            sp.Spread_All_Clear(SS2);
            Application.DoEvents();

            list = hicResultExCodeService.GetItemCounselbyWrtNo(argWrtNo);

            nRead = list.Count;
            SS2.ActiveSheet.RowCount = list.Count;

            for (int i = 0; i < nRead; i++)
            {
                strExCode = list[i].EXCODE;
                strResult = list[i].RESULT;
                strResCode = list[i].RESCODE;
                strResultType = list[i].RESULTTYPE;
                strGbCodeUse = list[i].GBCODEUSE;

                if (strExCode == "A163")
                {
                    FstrResult_A163 = strResult;
                }

                SS2.ActiveSheet.Cells[i, 0].Text = list[i].EXCODE;
                SS2.ActiveSheet.Cells[i, 1].Text = list[i].HNAME;
                if (!strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[i, 2].Text = strResult.Replace("\n", "");
                }

                //비만도
                if (strExCode == "A103")
                {
                    strResCode = "065";
                }

                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        strResName = hb.READ_ResultName(strResCode, strResult);
                        if (strResName.IsNullOrEmpty())
                        {
                            strResName = strResult;
                        }

                        if (!strResName.IsNullOrEmpty())
                        {
                            if (strResName.Length > 7)
                            {
                                strRemark += "▷" + list[i].HNAME + ":";
                                strRemark += strResName + "\r\n";
                            }
                        }
                    }
                }
                else if (!strResult.IsNullOrEmpty())
                {
                    if (strResult.Length > 7)
                    {
                        strRemark += "▷" + list[i].HNAME + ":";
                        strRemark += strResName + "\r\n";
                    }
                }

                if (list[i].PANJENG == "2")
                {
                    SS2.ActiveSheet.Cells[i, 5].Text = "*";
                }

                //Combo_set
                //자료를 READ
                List<HIC_RESCODE> list2 = hicRescodeService.GetCodeNamebyBindGubun(strResCode);
                int nCnt = list2.Count;

                //Array.Resize(ref strList, nCnt);
                strList = new string[nCnt];

                //for (int k = 0; k < list2.Count; k++) { strList[k] = ""; }

                if (list2.Count > 0)
                {
                    FarPoint.Win.Spread.CellType.ComboBoxCellType combo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

                    combo.Clear();

                    for (int j = 0; j < list2.Count; j++)
                    {
                        strList[j] = list2[j].CODE + "." + list2[j].NAME;
                    }

                    SS2.ActiveSheet.Cells[i, 2].Text = "";
                    combo.Items = strList;
                    //combo.ItemData = strList;
                    combo.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
                    combo.MaxDrop = list2.Count;
                    combo.MaxLength = 100;
                    combo.ListWidth = 200;
                    combo.Editable = false;
                    SS2.ActiveSheet.Cells[i, 2].CellType = combo;
                    SS2.ActiveSheet.Cells[i, 2].Locked = false;
                    //SS2.ActiveSheet.Cells[i, 2].Text = "";

                    if (!strResCode.IsNullOrEmpty() && !strResult.IsNullOrEmpty())
                    {
                        HIC_RESCODE list3 = hicRescodeService.GetCodeNamebyGubunCode(strResCode, strResult);

                        if (!list3.IsNullOrEmpty())
                        {
                            for (int k = 0; k < combo.Items.Length; k++)
                            {
                                combo.ListOffset = k;
                                if (combo.Items[k].ToString() == (list3.CODE.Trim() + "." + list3.NAME.Trim()).ToString())
                                {
                                    SS2.ActiveSheet.Cells[i, 2].Text = combo.Items[k].ToString();
                                    //break;
                                }
                            }
                        }
                    }
                }

                if (strResCode.IsNullOrEmpty() || strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[i, 2].Text = strResult;
                }

                SS2.ActiveSheet.Cells[i, 2].Locked = true;

                Application.DoEvents();

                SS2.ActiveSheet.Cells[i, 2].ForeColor = Color.FromArgb(0, 0, 0);

                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[i, 5].Text = hb.READ_ResultName(strResCode, strResult);
                    }
                }

                strExPan = list[i].PANJENG;

                //야간작업 검사결과가 비정상이면 R로 처리
                if (strExCode == "TZ72" || strExCode == "TZ85" || strExCode == "TZ86")
                {
                    if (strResult == "비정상") { strExPan = "R"; }
                }

                //참고치를 Dispaly
                strNomal = hm.EXAM_NomalValue_SET(strExCode, argJepDate, argSex, list[i].MIN_M, list[i].MAX_M, list[i].MIN_F, list[i].MAX_F);
                SS2.ActiveSheet.Cells[i, 5].Text = strExPan;
                SS2.ActiveSheet.Cells[i, 6].Text = strNomal;

                SS2.ActiveSheet.Cells[i, 7].Text = strResCode;

                if (list[i].EXCODE.Trim() == "A151")
                {
                    SS2.ActiveSheet.Cells[i, 7].Text = "007";

                    string strResult1 = strResult;
                }

                if (list[i].EXCODE.Trim() == "TH01" || list[i].EXCODE.Trim() == "TH02")
                {
                    SS2.ActiveSheet.Cells[i, 7].Text = "022";
                }

                SS2.ActiveSheet.Cells[i, 8].Text = "";
                SS2.ActiveSheet.Cells[i, 9].Text = list[i].RID;
                SS2.ActiveSheet.Cells[i, 10].Text = list[i].RESULTTYPE;
                SS2.ActiveSheet.Cells[i, 13].Text = list[i].EXSORT;
                //판정결과별 바탕색상을 다르게 표시함
                switch (strExPan)
                {
                    case "B":
                        SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222); //정상B
                        break;
                    case "C":
                        SS2.ActiveSheet.Cells[i, 2].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));  //주의C
                        break;
                    case "R":
                        SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 170, 170); //질환의심(R)
                        break;
                    default:
                        SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(190, 250, 220); //정상A 또는 기타
                        break;
                }

                if (list[i].GROUPCODE.To<string>("").Trim() != "1160" && (strExCode == "A123" || strExCode == "A242" || strExCode == "A241" || strExCode == "C404"))
                {
                    //SS2.ActiveSheet.Cells[i, 1].BackColor = Color.FromArgb(250, 210, 222); //정상
                    SS2.ActiveSheet.Cells[i, 1].BackColor = Color.PaleGoldenrod;
                }
            }

            #region 강제 Sort
            //검사결과의 판정값이 R,B인것을 위에 표시 
            SortInfo[] si = new SortInfo[]
            {
                new SortInfo(5, false)
              //, new SortInfo(0, true)
              //, new SortInfo(13, true)
            };

            SS2_Sheet1.SortRows(0, SS2_Sheet1.RowCount, si);
            #endregion
        }

        void fn_OLD_Result_Display(string argSex)
        {
            int nOldCNT = 0;
            List<string> strExamCode = new List<string>();
            string strAllWRTNO = "";
            string strJepDate = "";

            string[] strGjJong = { "31", "35" };

            SS2_Sheet1.ColumnHeader.Cells.Get(0, 11).Value = "이전결과1";
            SS2_Sheet1.ColumnHeader.Cells.Get(0, 12).Value = "이전결과2";

            List<HIC_JEPSU> list = new List<HIC_JEPSU>();

            //검사항목을 Setting
            strExamCode.Clear();
            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                if (!SS2.ActiveSheet.Cells[i, 0].Text.Trim().IsNullOrEmpty())
                {
                    strExamCode.Add(SS2.ActiveSheet.Cells[i, 0].Text.Trim());
                }
            }

            FnPano = PatListItem.PANO;
            FstrJepDate = PatListItem.JEPDATE;
            string strPtNo = PatListItem.PTNO;

            //1차검사 종전 접수번호를 읽음
            list = hicJepsuService.GetWrtNoJepDatePanjeng(FnPano, FstrJepDate, "HIC");

            nOldCNT = list.Count;
            strAllWRTNO = "";
            for (int i = 0; i < nOldCNT; i++)
            {
                strAllWRTNO += list[i].WRTNO.ToString() + ",";
                strJepDate = list[i].JEPDATE;
                if (SS2.ActiveSheet.ColumnCount < i + 11)
                {
                    SS2.ActiveSheet.ColumnCount += 1;
                }
                SS2_Sheet1.ColumnHeader.Cells.Get(0, i + 11).Value = VB.Left(strJepDate, 4) + VB.Mid(strJepDate, 6, 2) + VB.Right(strJepDate, 2);
                if (strExamCode.Count > 0)
                {
                    fn_OLD_Result_Display_SUB(i, list[i].WRTNO, strExamCode, argSex);
                }
                if (i >= 1)
                {
                    break;
                }
            }
        }

        void fn_OLD_Result_Display_SUB(int argCol, long argWrtNo, List<string> argExamCode, string argSex)
        {
            int nREAD = 0;
            int nRow = 0;
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strResName = "";
            string strRemark = "";
            string strExPan = "";
            string strSearch = "";
            string strGbUse = "";

            List<HIC_RESULT_EXCODE> list = new List<HIC_RESULT_EXCODE>();

            //검사항목 및 결과를 READ
            list = hicResultExCodeService.GetItembyWrtNoInExCodes(argWrtNo, argExamCode, "HIC");

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list[i].EXCODE.Trim();          //검사코드
                strResult = list[i].RESULT;                 //검사실 결과값
                strResCode = list[i].RESCODE;               //결과값 코드
                strResultType = list[i].RESULTTYPE;         //결과값 TYPE
                strGbCodeUse = list[i].GBCODEUSE;           //결과값코드 사용여부
                strGbUse = list[i].GBUSE;

                //해당검사코드가 있는 Row를 찾음
                for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                {
                    if (SS2.ActiveSheet.Cells[j, 0].Text.Trim() == strExCode)
                    {
                        nRow = j;
                        strSearch = "OK";
                        break;
                    }
                }

                //해당검사가 시트에 있으면 결과를 표시함
                //if (nRow > 0)
                if (strSearch == "OK")
                {
                    SS2.ActiveSheet.Cells[nRow, argCol + 11].Text = strResult;
                    if (strGbCodeUse == "Y")
                    {
                        if (!strResult.IsNullOrEmpty())
                        {
                            strResName = hb.READ_ResultName(strResCode, strResult);
                            SS2.ActiveSheet.Cells[nRow, argCol + 11].Text = strResName;
                            if (!strResName.IsNullOrEmpty())
                            {
                                if (strResName.Length > 7)
                                {
                                    strRemark += "▷" + list[i].HNAME + ": ";
                                    strRemark += strResName + "\r\n";
                                }
                            }
                        }
                    }
                    else if (!strResult.IsNullOrEmpty())
                    {
                        if (strResult.Length > 7)
                        {
                            strRemark += "▷" + list[i].HNAME + ":";
                            strRemark += strResult + "\r\n";
                        }
                    }

                    strExPan = hm.ExCode_Result_Panjeng(strExCode, strResult, argSex, FstrJepDate, "");
                    //판정결과별 바탕색상을 다르게 표시함
                    switch (strExPan)
                    {
                        case "B":
                            SS2.ActiveSheet.Cells[nRow, argCol + 11].BackColor = Color.FromArgb(250, 210, 222); //정상B
                            break;
                        case "R":
                            SS2.ActiveSheet.Cells[nRow, argCol + 11].BackColor = Color.FromArgb(250, 170, 170); //질환의심(R)
                            break;
                        default:
                            SS2.ActiveSheet.Cells[nRow, argCol + 11].BackColor = Color.FromArgb(190, 250, 220); //정상A 또는 기타
                            break;
                    }
                }
            }
        }

        private void PatInfo_Value(HC_PANJENG_PATLIST item)
        {
            PatListItem = item;
        }

    }
}
