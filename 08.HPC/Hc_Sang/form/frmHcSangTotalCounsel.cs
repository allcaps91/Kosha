using ComBase;
using ComLibB;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using ComBase.Controls;
using System.IO;

/// <summary>
/// Class Name      : HC_Sang
/// File Name       : frmHcSangTotalCounsel.cs
/// Description     : 일반검진 통합상담 프로그램
/// Author          : 이상훈
/// Create Date     : 2020-02-13
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHcSang_통합.frm(FrmHcSang_통합)" />

namespace Hc_Sang
{
    public partial class frmHcSangTotalCounsel : Form
    {
        EtcJupmstService etcJupmstService = null;
        HicSangdamWaitService hicSangdamWaitService = null;
        HicJepsuService hicJepsuService = null;
        HicResultService hicResultService = null;
        EndoJupmstService endoJupmstService = null;
        HicConsentService hicConsentService = null;
        HicSangdamNewService hicSangdamNewService = null;
        HicResBohum1Service hicResBohum1Service = null;
        HicResSpecialService hicResSpecialService = null;
        HicResBohum2Service hicResBohum2Service = null;
        EndoChartService endoChartService = null;
        HicSchoolNewService hicSchoolNewService = null;
        HicXMunjinService hicXMunjinService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicRescodeService hicRescodeService = null;
        HicCancerNewService hicCancerNewService = null;
        HicPatientService hicPatientService = null;
        HeaJepsuService heaJepsuService = null;
        HicWaitRoomService hicWaitRoomService = null;
        HicTitemService hicTitemService = null;
        HicJepsuExjongPatientService hicJepsuExjongPatientService = null;
        HicExcodeService hicExcodeService = null;
        HicHyangApproveService hicHyangApproveService = null;
        ComHpcLibBService comHpcLibBService = null;
        BasSunService basSunService = null;
        HicDoctorService hicDoctorService = null;
        HicJepsuSangdamNewExjongService hicJepsuSangdamNewExjongService = null;
        HicSangdamNewJepsuExjongService hicSangdamNewJepsuExjongService = null;
        HicExjongService hicExjongService = null;
        HicJepsuSangdamNewService hicJepsuSangdamNewService = null;
        HicResultHisService hicResultHisService = null;
        HicMunjinNightService hicMunjinNightService = null;
        HicIeMunjinNewService hicIeMunjinNewService = null;
        HicHyangService hicHyangService = null;
        HicSunapdtlGroupcodeService hicSunapdtlGroupcodeService = null;
        HicBcodeService hicBcodeService = null;
        HicJepsuCancerNewService hicJepsuCancerNewService = null;
        HeaResvExamService heaResvExamService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsVbfunc vf = new clsVbfunc();

        HIC_CODE CodeHelpItem = null;

        frmHeaResult FrmHeaResult = null;
        frmViewResult FrmViewResult = null;

        frmHcCodeHelp FrmHcCodeHelp = null;

        frmHcSangInternetMunjinView FrmHcSangInternetMunjinView = null;
        frmHcSchoolCommonInput FrmHcSchoolCommonInput = null;
        frmHcSangLivingHabitPrescription FrmHcSangLivingHabitPrescription = null;
        frmHcSchoolCommonDistrictRegView FrmHcSchoolCommonDistrictRegView = null;
        frmHcActPFTMunjin FrmHcActPFTMunjin = null;
        frmHcPanView FrmHcPanView = null;
        frmHaHyangjoengApproval FrmHaHyangjoengApproval = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();
        FarPoint.Win.Spread.CellType.ComboBoxCellType combo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

        long FnWRTNO;
        long FnWrtno2;          //2차 검진시 이전 1차 접수번호
        long FnPano;
        long FnAge;
        string FstrPtno;
        string FstrSex;
        string FstrJepDate;
        string FstrJumin;
        string FstrMunjin;
        string FstrChasu;
        string FstrUCodes;
        string[] FstrHabit = new string[18];
        string FstrJong;
        string FstrChul;
        string FstrGjJong;
        long FnRowNo;           //메모리타자기 위치 저장용
        long FnClickRow;        //Help를 Click한 Row
        string FstrYear;
        string FstrExamFlag;
        string FstrTFlag;       //생애검진 플래그
        string FstrCOMMIT;
        string FstrROWID;
        string FstrEndoRowID;
        string Fstr내시경대상;
        long FnHeaWRTNO;        //종합검진 접수번호
        long FnDrno;            //상담의사 면허번호

        public frmHcSangTotalCounsel()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            etcJupmstService = new EtcJupmstService();
            hicSangdamWaitService = new HicSangdamWaitService();
            hicJepsuService = new HicJepsuService();
            hicResultService = new HicResultService();
            endoJupmstService = new EndoJupmstService();
            hicConsentService = new HicConsentService();
            hicSangdamNewService = new HicSangdamNewService();
            hicResBohum1Service = new HicResBohum1Service();
            hicResSpecialService = new HicResSpecialService();
            hicResBohum2Service = new HicResBohum2Service();
            endoChartService = new EndoChartService();
            hicSchoolNewService = new HicSchoolNewService();
            hicXMunjinService = new HicXMunjinService();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicSunapdtlService = new HicSunapdtlService();
            hicResultExCodeService = new HicResultExCodeService();
            hicRescodeService = new HicRescodeService();
            hicCancerNewService = new HicCancerNewService();
            hicPatientService = new HicPatientService();
            heaJepsuService = new HeaJepsuService();
            hicWaitRoomService = new HicWaitRoomService();
            hicTitemService = new HicTitemService();
            hicJepsuExjongPatientService = new HicJepsuExjongPatientService();
            hicExcodeService = new HicExcodeService();
            hicHyangApproveService = new HicHyangApproveService();
            comHpcLibBService = new ComHpcLibBService();
            basSunService = new BasSunService();
            hicDoctorService = new HicDoctorService();
            hicJepsuSangdamNewExjongService = new HicJepsuSangdamNewExjongService();
            hicSangdamNewJepsuExjongService = new HicSangdamNewJepsuExjongService();
            hicExjongService = new HicExjongService();
            hicJepsuSangdamNewService = new HicJepsuSangdamNewService();
            hicResultHisService = new HicResultHisService();
            hicMunjinNightService = new HicMunjinNightService();
            hicIeMunjinNewService = new HicIeMunjinNewService();
            hicHyangService = new HicHyangService();
            hicSunapdtlGroupcodeService = new HicSunapdtlGroupcodeService();
            hicBcodeService = new HicBcodeService();
            hicJepsuCancerNewService = new HicJepsuCancerNewService();
            heaResvExamService = new HeaResvExamService();

            this.Load += new EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnMenuCall.Click += new EventHandler(eBtnClick);
            this.btnPACS.Click += new EventHandler(eBtnClick);
            this.btnMed.Click += new EventHandler(eBtnClick);
            this.btnPFT.Click += new EventHandler(eBtnClick);
            this.btnAudio.Click += new EventHandler(eBtnClick);
            this.btnAudio1.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnMenuUnSave.Click += new EventHandler(eBtnClick);
            this.btnMenuWard.Click += new EventHandler(eBtnClick);
            this.btnMenuWait.Click += new EventHandler(eBtnClick);
            this.btnMenuLungMun.Click += new EventHandler(eBtnClick);
            this.btnMenuPsychotropicDrugsApproval.Click += new EventHandler(eBtnClick);
            this.btnMenuEndoConsent.Click += new EventHandler(eBtnClick);
            this.btnSang1.Click += new EventHandler(eBtnClick);
            this.btnSang2.Click += new EventHandler(eBtnClick);
            this.btnLivingHabit.Click += new EventHandler(eBtnClick);
            this.rdoJob1.Click += new EventHandler(eRdoClick);
            this.rdoBodyCondition0.Click += new EventHandler(eRdoClick);
            this.rdoBodyCondition1.Click += new EventHandler(eRdoClick);
            this.rdoBodyCondition2.Click += new EventHandler(eRdoClick);
            this.rdoBodyCondition3.Click += new EventHandler(eRdoClick);
            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS2.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSHistory.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SSJong.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.SS2.CellClick += new CellClickEventHandler(eSpdClick);
            this.SS_JinDan.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SS2.Change += new ChangeEventHandler(eSpdChange);
            this.SS2.EditModeOff += new EventHandler(eSpdEditModeOff);
            this.SS2.KeyDown += new KeyEventHandler(eSpdKeyDown);
            this.txtSName.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtLastMedHis.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtBodySymptom.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCancerHis.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtStomachBowlLiver.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtChestCervical.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSName.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtLastMedHis.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtSName.LostFocus += new EventHandler(eTxtLostFocus);
            this.chkRoom0.Click += new EventHandler(eCheckBoxClick);
            this.chkRoom1.Click += new EventHandler(eCheckBoxClick);
            this.chkRoom2.Click += new EventHandler(eCheckBoxClick);
            this.chkRoom3.Click += new EventHandler(eCheckBoxClick);
            this.chkRoom4.Click += new EventHandler(eCheckBoxClick);
            this.chkRoom5.Click += new EventHandler(eCheckBoxClick);
            this.chkX1_10.Click += new EventHandler(eCheckBoxClick);
            this.chkX1_11.Click += new EventHandler(eCheckBoxClick);
            this.chkX1_12.Click += new EventHandler(eCheckBoxClick);
            this.chkX1_20.Click += new EventHandler(eCheckBoxClick);
            this.chkX1_21.Click += new EventHandler(eCheckBoxClick);
            this.chkX1_22.Click += new EventHandler(eCheckBoxClick);
            this.chkX1_40.Click += new EventHandler(eCheckBoxClick);
            this.chkX1_41.Click += new EventHandler(eCheckBoxClick);
            this.chkX40.Click += new EventHandler(eCheckBoxClick);
            this.chkX41.Click += new EventHandler(eCheckBoxClick);
            this.chkX42.Click += new EventHandler(eCheckBoxClick);
            this.cboXMonth.KeyPress += new KeyPressEventHandler(eComboBoxKeyPress);
            this.cboXYear.KeyPress += new KeyPressEventHandler(eComboBoxKeyPress);
            this.cboXMonth.Click += new EventHandler(eComboBoxClick);
            this.cboXYear.Click += new EventHandler(eComboBoxClick);
            this.txtWrtNo.Click += new EventHandler(eTxtClick);

        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strData = "";

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;

            if (clsPublic.GstrRetValue == "1")
            {
                hf.Read_Amountindicator(this, lstMonitors);

                //처음부터 단일모니터가 아닐경우와
                if (clsHcVariable.singmon != 1)
                {
                    if (clsHcVariable.selmon == 1)
                    {
                        this.Left = 0;
                        this.Top = 0;
                    }
                    else
                    {
                        this.Left = (int)clsHcVariable.slavecoodinate * 15;
                        this.Top = 0;
                    }
                }

                pnl1.Enabled = false;
                grpList.Enabled = false;
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;
            lblHyangTitle.BackColor = Color.FromArgb(250, 170, 170);

            //판정의사 여부를 읽음
            hb.READ_HIC_Doctor(long.Parse(clsType.User.IdNumber));

            this.Text += "(" + clsHcVariable.GstrHicDrName + ") (면허:" + clsHcVariable.GnHicLicense + ")";

            fn_ComboBox_Set();
            fn_Screen_Clear();
            hb.ComboJong_Set(cboJong);
            cboJong.SelectedIndex = 0;

            cboASA.Items.Clear();
            cboASA.Items.Add("1.전신질환이 없는 건강한 환자");
            cboASA.Items.Add("2.경한 전신 질환이 있으나 생리적 기능 장애는 없는 환자");
            cboASA.Items.Add("3.신체기능의 장애를 초래하는 중한 전신질환을 가진 환자");
            cboASA.Items.Add("4.생명에 위협이 되는 전신질환을 가진 환자");
            cboASA.Items.Add("5.24시간 내에 사망율이 50 % 인 반사상태의 환자");
            cboASA.Items.Add("6.죽음이 선언되고 장기기증을 목적으로 수술 받는 환자");
            cboASA.Items.Add("E.응급수술 및 시술이 필요한 경우");
            cboASA.SelectedIndex = 0;

            for (int i = 0; i <= 5; i++)
            {
                CheckBox chkRoom = (Controls.Find("chkRoom" + i.ToString(), true)[0] as CheckBox);
                chkRoom.Checked = false;
            }

            chkRoom4.Checked = true;
            switch (clsHcVariable.GstrDrRoom)
            {
                case "18":
                    chkRoom0.Checked = true;
                    break;
                case "17":
                    chkRoom1.Checked = true;
                    break;
                case "16":
                    chkRoom2.Checked = true;
                    break;
                case "15":
                    chkRoom3.Checked = true;
                    break;
                case "19":
                    chkRoom5.Checked = true;
                    break;
                default:
                    break;
            }

            eBtnClick(btnSearch, new EventArgs());

            if (clsHcVariable.GnHicLicense == 0)
            {
                btnSave.Enabled = false;
            }
        }

        void fn_ComboBox_Set()
        {
            //학생상담 Set=============================================

            //근골격 및 척추
            cboPRes.Items.Clear();
            cboPRes.Items.Add("01.정상");
            cboPRes.Items.Add("02.흉부측만");
            cboPRes.Items.Add("03.흉부후만");
            cboPRes.Items.Add("04.흉부전만");
            cboPRes.Items.Add("05.오목가슴");
            cboPRes.Items.Add("06.새가슴");
            cboPRes.Items.Add("07.요통");
            cboPRes.Items.Add("08.어깨결림");
            cboPRes.Items.Add("09.발달장애");
            cboPRes.Items.Add("10.기타");
            cboPRes.SelectedIndex = 1;

            //안질환(좌/우)
            for (int i = 0; i <= 1; i++)
            {
                ComboBox cboEJ = (Controls.Find("cboEJ" + i.ToString(), true)[0] as ComboBox);
                cboEJ.Items.Clear();
                cboEJ.Items.Add(" ");
                cboEJ.Items.Add("1.없음");
                cboEJ.Items.Add("2.결막염");
                cboEJ.Items.Add("3.눈썹찔림증");
                cboEJ.Items.Add("4.사시");
                cboEJ.Items.Add("5.기타");
                cboEJ.SelectedIndex = 1;
            }

            //콧병
            cboM.Items.Clear();
            cboM.Items.Add(" ");
            cboM.Items.Add("1.없음");
            cboM.Items.Add("2.부비동염");
            cboM.Items.Add("3.비염");
            cboM.Items.Add("4.기타");
            cboM.SelectedIndex = 1;

            //목병
            cboN.Items.Clear();
            cboN.Items.Add(" ");
            cboN.Items.Add("1.없음");
            cboN.Items.Add("2.편도비대");
            cboN.Items.Add("3.임파절증대");
            cboN.Items.Add("4.갑상선비대");
            cboN.Items.Add("5.기타");
            cboN.SelectedIndex = 1;

            //피부병
            cboS.Items.Clear();
            cboS.Items.Add(" ");
            cboS.Items.Add("1.없음");
            cboS.Items.Add("2.아토피성피부염");
            cboS.Items.Add("3.전염성피부염");
            cboS.Items.Add("4.기타");
            cboS.SelectedIndex = 1;

            //귓병
            cboHJ.Items.Clear();
            cboHJ.Items.Add(" ");
            cboHJ.Items.Add("1.없음");
            cboHJ.Items.Add("2.중이염");
            cboHJ.Items.Add("3.외이도염");
            cboHJ.Items.Add("4.기타");
            cboHJ.SelectedIndex = 1;

            //진찰 및 상담
            cboJ.Items.Clear();
            cboJ.Items.Add(" ");
            cboJ.Items.Add("1.무");
            cboJ.Items.Add("2.유");
            cboJ.SelectedIndex = 1;


            //기관능력
            //For i = 0 To 5
            for (int i = 0; i <= 5; i++)
            {
                ComboBox cboOrgan = (Controls.Find("cboOrgan" + i.ToString(), true)[0] as ComboBox);
                cboOrgan.Items.Clear();
                cboOrgan.Items.Add(" ");
                cboOrgan.Items.Add("1.정상");
                cboOrgan.Items.Add("2.예방필요");
                cboOrgan.Items.Add("3.정밀검사");
                cboOrgan.SelectedIndex = 1;
            }

            //야간작업
            cboInsomniaMun.Items.Clear();
            cboInsomniaMun.Items.Add(" ");
            cboInsomniaMun.Items.Add("정상");
            cboInsomniaMun.Items.Add("비정상");
            cboInsomniaMun.SelectedIndex = 0;

            cboStomachMun.Items.Clear();
            cboStomachMun.Items.Add(" ");
            cboStomachMun.Items.Add("정상");
            cboStomachMun.Items.Add("비정상");
            cboStomachMun.SelectedIndex = 0;

            cboBreastCancerMun.Items.Clear();
            cboBreastCancerMun.Items.Add(" ");
            cboBreastCancerMun.Items.Add("정상");
            cboBreastCancerMun.Items.Add("비정상");
            cboBreastCancerMun.SelectedIndex = 0;

            cboinsomniaMun2.Items.Clear();
            cboinsomniaMun2.Items.Add(" ");
            cboinsomniaMun2.Items.Add("01.정상");
            cboinsomniaMun2.Items.Add("02.중증도 주간졸림증");
            cboinsomniaMun2.Items.Add("03.중증도 주간졸림증 및 수면의 질이 좋지 못함");
            cboinsomniaMun2.Items.Add("04.심한 주간졸림증");
            cboinsomniaMun2.Items.Add("05.심한 주간졸림증 및 수면의 질이 좋지 못함");
            cboinsomniaMun2.Items.Add("06.수면의 질이 좋지 않음");
            cboinsomniaMun2.SelectedIndex = 0;

            lblChestCancelMun.Visible = false;
            cboBreastCancerMun.Visible = false;

            //방사선 종사기간
            cboXYear.Items.Clear();
            for (int i = 0; i <= 60; i++)
            {
                ComboBox cboXYear = (Controls.Find("cboXYear" + i.ToString(), true)[0] as ComboBox);
                cboXYear.Items.Add(string.Format("{0:00}", i));
            }

            cboXMonth.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                ComboBox cboXMonth = (Controls.Find("cboXMonth" + i.ToString(), true)[0] as ComboBox);
                cboXMonth.Items.Add(string.Format("{0:00}", i));
            }
        }

        void eFormActivated(object sender, EventArgs e)
        {
            long nWRTNO = 0;

            if (FnWRTNO != 0)
            {
                nWRTNO = FnWRTNO;

                fn_Screen_Clear();
                txtWrtNo.Text = nWRTNO.ToString();
                FnWRTNO = nWRTNO;

                fn_Screen_Display();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnCancel)
            {
                fn_Screen_Clear();
                txtWrtNo.Focus();
            }            
            else if (sender == btnLtdCode)
            {
                frmHcLtdHelp frm = new frmHcLtdHelp();
                frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                frm.ShowDialog();
                frm.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnMed)
            {
                FrmHeaResult = new frmHeaResult(FstrJumin);
                FrmHeaResult.ShowDialog(this);
            }
            else if (sender == btnPACS)
            {
                FrmViewResult = new frmViewResult(FstrPtno);
                FrmViewResult.ShowDialog();
            }
            else if (sender == btnAudio)
            {
                string strRowId = "";

                List<ETC_JUPMST> list = etcJupmstService.GetRowIdbyPtNoBDate(FstrPtno, FstrJepDate, "", "6");

                if (list.Count > 0)
                {
                    strRowId = list[0].ROWID;
                }

                if (strRowId != "")
                {
                    hf.AudioFILE_DBToFile(strRowId, "1");
                }
            }
            else if (sender == btnAudio1)
            {
                string strRowId = "";
                //string strFileName = @"c:\cmc\*.jpg";
                //FileInfo f = new FileInfo(strFileName);

                DirectoryInfo dir = new DirectoryInfo(@"c:\cmc\");
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo F in files)
                {
                    if (F.Extension == ".jpg")
                    {
                        F.Delete();
                    }
                }

                List<ETC_JUPMST> list = etcJupmstService.GetRowIdbyPtNoBDate(FstrPtno, FstrJepDate, "Y", "6");

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        strRowId = list[i].ROWID;
                        hf.AudioFILE_DBToFile1(strRowId, "1", i);
                    }
                }
            }
            else if (sender == btnPFT)
            {
                //폐기능은 여러건을 볼수 있도록 처리 합니다.
                string fs = "";
                string f = "";
                string S = "";
                string strFileName = "";

                string strHicPano = "";
                string strHeaPano = "";
                string StrJumin = "";

                //기존화일 삭제
                strFileName = @"c:\cmc\*.jpg";

                DirectoryInfo dir = new DirectoryInfo(@"c:\cmc\");
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo F in files)
                {
                    if (F.Extension == ".jpg")
                    {
                        F.Delete();
                    }
                }

                List<ETC_JUPMST> list = etcJupmstService.GetRowIdbyPtNoBDate(FstrPtno, FstrJepDate, "N", "4");

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        hf.PFTFILE_DBToFile(list[i].ROWID, "1", i);
                    }
                }
            }
            else if (sender == btnCancel)
            {
                int result = 0;

                clsDB.setBeginTran(clsDB.DbCon);

                //호출은 했으나 상담이 완료안된 접수번호 찾음
                if (hicSangdamWaitService.GetCountbyWrtNoGubun(clsHcVariable.GstrDrRoom, long.Parse(txtWrtNo.Text)) > 0)
                {
                    result = hicSangdamWaitService.UpdateCallTimeDisplaybyOnlyWrtNo(long.Parse(txtWrtNo.Text));

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("상담내역 취소 처리 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                fn_Screen_Clear();

                txtWrtNo.Focus();
            }
            else if (sender == btnJengSang)
            {
                clsPublic.GstrRetValue = "11"; //자타각증상
                FrmHcCodeHelp = new frmHcCodeHelp("11");
                FrmHcCodeHelp.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(eCode_value);
                FrmHcCodeHelp.ShowDialog();

                if (!CodeHelpItem.IsNullOrEmpty())
                {
                    txtJengSang.Text = CodeHelpItem.CODE.Trim() + "." + CodeHelpItem.NAME.Trim();
                }
                else
                {
                    txtJengSang.Text = "";
                }
                FrmHcCodeHelp.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(eCode_value);
            }
            else if (sender == btnSang1)
            {
                txtLung_Sang2.Text = "지금 즉시 금연하여야 폐암 발생 위험을 줄일수 있습니다. 금연프로그램의 지원을 받으실수 있습니다.";
            }
            else if (sender == btnSang2)
            {
                txtLung_Sang2.Text = "금연은 폐암발생과 사망을 줄이는데 필수 요소입니다. 금연을 지속하시기 바랍니다.";
            }
            else if (sender == btnSave)
            {
                long nWRTNO = 0;
                long nPano = 0;
                int nIdx = 0;
                string strGbSang = "";
                string strASA = "";
                int result = 0;
                int nTabSel = 0;

                if (long.Parse(txtWrtNo.Text) == 0)
                {
                    return;
                }

                if (long.Parse(txtPanDrNo.Text) == 0)
                {
                    MessageBox.Show("상담의사 면허번호가 누락되었습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (clsType.User.IdNumber != "28048")
                {
                    if (hicJepsuService.GetCountbyWrtNo(long.Parse(txtWrtNo.Text)) > 0)
                    {
                        MessageBox.Show("판정완료 수검자 입니다. 상담수정을 하시려면 판정을 풀어주세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //야간작업 불면증 증상문진 점검
                if (lblPan1.Text.Trim() != "" && long.Parse(txtSum1.Text) > 0)
                {
                    switch (VB.Left(lblPan1.Text, 1))
                    {
                        case "1":
                        case "2":
                            if (cboInsomniaMun.Text.Trim() != "정상")
                            {
                                MessageBox.Show("야간작업 불면증 증상문진 값이 오류입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            break;
                        default:
                            if (cboInsomniaMun.Text.Trim() != "비정상")
                            {
                                MessageBox.Show("야간작업 불면증 증상문진 값이 오류입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            break;
                    }

                    //ASA등급
                    strASA = VB.Pstr(cboASA.Text, ".", 1).Trim();
                    if (SSHyang.ActiveSheet.RowCount > 0 && strASA == "")
                    {
                        MessageBox.Show("신체등급(ASA)을 선택하십시오.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    FnDrno = long.Parse(txtPanDrNo.Text);
                    hb.READ_HIC_DrSabun(txtPanDrNo.Text.Trim());

                    //상담완료 Flag (Jepsu)
                    strGbSang = "N";
                    HIC_JEPSU list = hicJepsuService.GetSangdamDrNoPaNobyWrtNo(FnWRTNO);
                    if (!list.IsNullOrEmpty())
                    {
                        if (list.SANGDAMDRNO > 0)
                        {
                            strGbSang = "Y";
                        }
                        nPano = list.PANO;
                    }

                    clsDB.setBeginTran(clsDB.DbCon);

                    nTabSel = TabMain.SelectedTabIndex;

                    result = hicJepsuService.UpdateCounselbyWrtNo(clsType.User.IdNumber, FnWRTNO, nTabSel, strGbSang);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("상담Flag 저장시 오류 발생(HIC_JEPSU)", "전산실 연락요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //상담완료 Flag (Acting)
                    HIC_RESULT list2 = hicResultService.GetActivebyWrtNo(FnWRTNO);

                    if (!list2.IsNullOrEmpty())
                    {
                        if (list2.ACTIVE != "Y")
                        {
                            result = hicResultService.UpdateActiveResultbyWrtNo(clsType.User.IdNumber, FnWRTNO);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("상담Flag 저장시 오류 발생(HIC_RESULT)", "전산실 연락요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    //clsDB.setCommitTran(clsDB.DbCon);

                    //수면내시경 신체등급(ASA)등급
                    if (SSHyang.ActiveSheet.RowCount > 0)
                    {
                        strASA = VB.Pstr(cboASA.Text, ".", 1).Trim();

                        result = endoJupmstService.UPdateASAbyPtNoRDate(strASA, FstrPtno, FstrJepDate);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("신체등급(ASA)등급 Update 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        result = hicConsentService.UpdateASASabunbyPtNoSDate(strASA, FnDrno, FstrPtno, FstrJepDate);

                        if (result < 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("신체등급(ASA)등급 Update 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    clsDB.setCommitTran(clsDB.DbCon);

                    //상담대기순번 완료 및 다음검사실 지정
                    fn_WAIT_NextRoom_SET();

                    FstrCOMMIT = "OK";
                    //nIdx = SSTab_Main.Tab
                    nIdx = TabMain.SelectedTabIndex;

                    switch (nIdx)
                    {
                        case 0:
                            fn_Save1(); //CmdSave1_Click(1차 일반상담 저장)
                            fn_Save_New();
                            break;
                        case 1:
                            fn_Save2(); //CmdSave2_Click(2차 일반상담 저장)
                            break;
                        case 2:
                            fn_Save3(); //CmdSave3_Click(암검진 상담 저장)
                            break;
                        case 3:
                            fn_School_Save(); //School_CmdSave_Click(학생검진 상담 저장)
                            break;
                        case 4:
                            fn_Xmunjin_Save(); //XMunjin_CmdSave_Click(방사선종사자 상담 저장)
                            break;
                        case 5:
                            fn_Save1(); //CmdSave1_Click(야간작업)
                            break;
                        case 6:
                            fn_Lung_Save(); //CmdSAVE_LUNG
                            break;
                        default:
                            break;
                    }

                    //62종, 69종 상담저장
                    if (FstrJong == "11")
                    {
                        fn_SANGDAM_ADD_UPDATE();
                    }

                    fn_Update_Result_Data();

                    //당일수검 종류와 상태를 체크
                    fn_Read_Jepsu_Display();

                    //사전점검에 걸리면 저장로직 빠져나감
                    if (FstrCOMMIT != "OK")
                    {
                        return;
                    }

                    //상담완료 되지않은것을 찾아 자동으로 Display
                    for (int i = 0; i <= 4; i++)
                    {
                        if (SSJong.ActiveSheet.Cells[0, i].Text.Trim() != "" && SSJong.ActiveSheet.Cells[0, i].BackColor == ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HBAFEFC")))
                        {
                            if (nIdx != (i - 1))    //현재 상담위치를 제외함
                            {
                                nWRTNO = long.Parse(SSJong.ActiveSheet.Cells[1, i].Text);
                                return;
                            }
                        }
                    }

                    fn_Screen_Clear();

                    if (nWRTNO == 0)
                    {
                        txtWrtNo.Focus();
                        eBtnClick(btnSearch, new EventArgs());
                    }
                    else
                    {
                        txtWrtNo.Text = nWRTNO.ToString();
                        FnWRTNO = nWRTNO;
                        fn_Screen_Display();
                    }
                }
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                long nPano = 0;
                string strJong = "";
                string strTemp = "";
                string strOK = "";
                List<string> strGubun = new List<string>();
                string strGbn = "";
                List<string> strDrList = new List<string>();
                string strYear = "";
                string strFrDate = "";
                string strToDate = "";
                string strChul = "";
                string strJob = "";
                string strRoom = "";
                string strSName = "";
                long nLtdCode = 0;
                int result = 0;

                nRow = 0;
                strGubun.Clear();

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;


                for (int i = 0; i <= 5; i++)
                {
                    CheckBox chkRoom = (Controls.Find("chkRoom" + i.ToString(), true)[0] as CheckBox);
                    if (chkRoom.Checked == true)
                    {
                        switch (i)
                        {
                            case 0:
                                strGubun.Add("18");
                                break;
                            case 1:
                                strGubun.Add("17");
                                break;
                            case 2:
                                strGubun.Add("16");
                                break;
                            case 3:
                                strGubun.Add("15");
                                break;
                            case 5:
                                strGubun.Add("19");
                                break;
                            case 4:
                                strGubun.Add("");
                                break;
                            default:
                                break;
                        }
                    }
                }

                strDrList.Clear();
                if (strGubun != null && rdoJob1.Checked == true)
                {
                    List<HIC_DOCTOR> list = hicDoctorService.GetSabunbyRoom(strGubun);

                    nREAD = list.Count;
                    for (int i = 0; i < nREAD; i++)
                    {
                        strDrList.Add(list[i].SABUN.ToString());
                    }
                }

                for (int i = 0; i <= 3; i++)
                {
                    RadioButton rdoGbn = (Controls.Find("rdoGbn" + i.ToString(), true)[0] as RadioButton);
                    if (rdoGbn.Checked == true)
                    {
                        strGbn = i.ToString();
                    }
                }

                if (rdoChul0.Checked == true)
                {
                    strChul = "0";
                }
                else if (rdoChul1.Checked == true)
                {
                    strChul = "1";
                }

                if (chkRoom4.Checked == true)
                {
                    strRoom = "1";
                }
                else
                {
                    strRoom = "0";
                }

                if (rdoJob1.Checked == true)
                {
                    strJob = "1";
                }

                strSName = txtSName.Text.Trim();
                nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
                strJong = VB.Left(cboJong.Text, 2);

                sp.Spread_All_Clear(SSList);                

                //상담대기자를 읽음
                List<HIC_JEPSU_SANGDAM_NEW_EXJONG> list2 = hicJepsuSangdamNewExjongService.GetItembyJepDateGjJong(strFrDate, strToDate, strChul, strGbn, strJob, strRoom, strDrList, strGubun, strSName, strJong, nLtdCode);

                nREAD = list2.Count;
                SSList.ActiveSheet.RowCount = nREAD;
                nRow = 0;
                for (int i = 0; i < nREAD; i++)
                {
                    strOK = "OK";
                    nPano = list2[i].PANO;
                    strTemp = list2[i].SANGDAMDRNO.ToString();
                    if (strJob == "0")
                    {
                        if (strTemp != "")
                        {
                            strOK = "";
                        }
                    }
                    else
                    {
                        if (strTemp == "")
                        {
                            strOK = "";
                        }
                    }

                    if (strOK == "ok")
                    {
                        if (strRoom == "1")
                        {
                            if (hicSangdamWaitService.GetCountbyWrtNo(list2[i].WRTNO) > 0)
                            {
                                strOK = "";
                            }
                        }
                    }

                    if (strOK == "OK")
                    {
                        //상담 Acting 코드가 있는 사람만 표시함...
                        if (hb.READ_SangDam_Acting(list2[i].WRTNO) == "")
                        {
                            strOK = "";
                            //상담대기를 삭제함
                            result = hicSangdamWaitService.DeletebyWrtNo(list2[i].WRTNO);

                            if (result < 0)
                            {
                                MessageBox.Show("삭제 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    if (strOK == "OK")
                    {
                        SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                        SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                        SSList.ActiveSheet.Cells[i, 0].Text = list2[i].WRTNO.ToString();
                        SSList.ActiveSheet.Cells[i, 1].Text = list2[i].SNAME.Trim();
                        SSList.ActiveSheet.Cells[i, 2].Text = list2[i].JEPDATE.ToString();
                        strJong = list2[i].GJJONG.Trim();
                        SSList.ActiveSheet.Cells[i, 3].Text = hb.READ_GjJong_Name(list2[i].GJJONG.ToString());                        
                        SSList.ActiveSheet.Cells[i, 5].Text = list2[i].GJYEAR.Trim();

                        //일특 체크
                        if (strJong == "11" || strJong == "16" || strJong == "41" || strJong == "44")
                        {
                            if (list2[i].UCODES.Trim() != "")
                            {
                                SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                                SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(200, 255, 200);
                            }
                        }

                        if (strTemp == clsType.User.IdNumber)
                        {
                            SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                            SSList.ActiveSheet.Cells[i, 0, i, SSList.ActiveSheet.ColumnCount - 1].ForeColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFC0FF"));
                        }

                        if (strJong == "11" || strJong == "12" || strJong == "13" || strJong == "14" || strJong == "41" || strJong == "42" || strJong == "43")
                        {
                            if (fn_Read_PubCorpCancerScreeningYN(nPano, list2[i].JEPDATE.ToString()) > 0)
                            {
                                SSList.ActiveSheet.Cells[i, 3].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFC0C0"));
                            }
                        }
                        SSList.ActiveSheet.Cells[i, 4].Text = fn_Read_SangDam_Check(nPano, list2[i].JEPDATE.ToString());
                    }
                }

                if (rdoJob1.Checked == true)
                {
                    SSList.ActiveSheet.Cells[0, 0, 0, SSList.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                }

                //상담인원 및 대기인원 DISPLAY
                HIC_SANGDAM_NEW_JEPSU_EXJONG list3 = hicSangdamNewJepsuExjongService.GetCntCnt2(long.Parse(clsType.User.IdNumber));

                lblCounter.Text = "총 대기인원: ";
                lblCounter.Text += list3.CNT2 + "명 / ";
                lblCounter.Text += "상담인원 : ";
                lblCounter.Text += list3.CNT + " 명";
            }
            else if (sender == btnWards)
            {
                FrmHcSchoolCommonInput = new frmHcSchoolCommonInput("4");
                FrmHcSchoolCommonInput.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSchoolCommonInput.ssPanjengDblClick += new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick);
                FrmHcSchoolCommonInput.ShowDialog(this);
                FrmHcSchoolCommonInput.ssPanjengDblClick -= new frmHcSchoolCommonInput.Spread_DoubleClick(frmHcSchoolCommonInput_ssPanjengDblClick);
            }
            else if (sender == btnLivingHabit)
            {

                FrmHcSangLivingHabitPrescription = new frmHcSangLivingHabitPrescription(long.Parse(txtWrtNo.Text));
                FrmHcSangLivingHabitPrescription.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSangLivingHabitPrescription.ShowDialog(this);
            }
            else if (sender == btnMenuEndoConsent)
            {
                ///TODO : 이상훈(2020.02.24) 동의서개발은 별도진행
                // clsHcVariable.GstrRetValue1 = TxtWRTNO.Text;
                //Frm동의서.Show
            }
            else if (sender == btnMenuUnSave)
            {
                int nRead = 0;
                int nWaitNo = 0;
                string strFrDate = "";
                string strToDate = "";
                int result = 0;

                if (FnWRTNO == 0) return;

                if (clsHcVariable.GstrDrRoom == "") return;

                if (MessageBox.Show("상담실 방번호를 변경하시겠습니까?", "상담실 구분변경", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                clsHcVariable.GnWRTNO = FnWRTNO;
                nWaitNo = 2;

                strFrDate = FstrJepDate;
                strToDate = DateTime.Parse(FstrJepDate).AddDays(1).ToShortDateString();

                if (clsHcVariable.GstrDrRoom == hicSangdamWaitService.GetGubunbyWrtNo(clsHcVariable.GnWRTNO))
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                HIC_SANGDAM_WAIT list = hicSangdamWaitService.GetWaitNobyGubun(clsHcVariable.GstrDrRoom);

                if (!list.IsNullOrEmpty())
                {
                    List<HIC_SANGDAM_WAIT> list2 = hicSangdamWaitService.GetWrtNoWaitNo(clsHcVariable.GstrDrRoom, strFrDate, strToDate);

                    nRead = list2.Count;
                    if (nRead > 0)
                    {
                        for (int i = 0; i < nRead; i++)
                        {
                            nWaitNo += 1;
                            result = hicSangdamWaitService.UpdateWaitNobyWrtnoGubunEntTime(nWaitNo, list2[i].WRTNO, clsHcVariable.GstrDrRoom, strFrDate, strToDate);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("상담순번 변경 시 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }

                result = hicSangdamWaitService.UpdateWaitNoGubunbyWrtNo(clsHcVariable.GstrDrRoom, clsHcVariable.GnWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담순번 변경 시 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == btnMenuWard)
            {
                FrmHcSchoolCommonDistrictRegView = new frmHcSchoolCommonDistrictRegView();
                FrmHcSchoolCommonDistrictRegView.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSchoolCommonDistrictRegView.ShowDialog(this);
            }
            else if (sender == btnMenuCall)
            {
                int result = 0;

                clsDB.setBeginTran(clsDB.DbCon);

                result = hicSangdamWaitService.DeletebyGubun(clsHcVariable.GstrDrRoom);

                HIC_SANGDAM_WAIT item = new HIC_SANGDAM_WAIT();

                item.WRTNO = 0;
                item.SNAME = "{수검자호출}";
                item.GUBUN = clsHcVariable.GstrDrRoom;

                result = hicSangdamWaitService.InserWrtNoSNameGubunt(item);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("다음 수검자 호출 실패", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setRollbackTran(clsDB.DbCon);
            }
            else if (sender == btnMenuLungMun)
            {   
                FrmHcActPFTMunjin = new frmHcActPFTMunjin(FstrPtno, long.Parse(txtWrtNo.Text));
                FrmHcActPFTMunjin.StartPosition = FormStartPosition.CenterScreen;
                FrmHcActPFTMunjin.ShowDialog(this);
            }
            else if (sender == btnMenuPsychotropicDrugsApproval)
            {
                FrmHaHyangjoengApproval = new frmHaHyangjoengApproval();
                FrmHaHyangjoengApproval.ShowDialog(this);
            }
        }

        void frmHcSchoolCommonInput_ssPanjengDblClick(string argGubun, string strCommon)
        {
            txtRemark.Text += strCommon;
        }

        string fn_Read_SangDam_Check(long argPaNo, string argJepDate)
        {
            string rtnVal = "○";
            int nRead = 0;

            List<HIC_JEPSU_SANGDAM_NEW> list = hicJepsuSangdamNewService.GetItembyPaNoJepDate(argPaNo, argJepDate);

            nRead = list.Count;
            for (int i = 0; i < nRead; i++)
            {
                if (list[i].GJJONG.Trim() == "50" || list[i].GJJONG.Trim() == "51")
                {
                    if (hicXMunjinService.GetJinGbnbyWrtNo(list[i].WRTNO).Trim().IsNullOrEmpty())
                    {
                        rtnVal = "";
                    }
                }
                else
                {
                    if (list[i].GBSTS.Trim() != "Y")
                    {
                        rtnVal = "";
                    }
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 다음 검사실 설정
        /// </summary>
        bool fn_WAIT_NextRoom_SET()
        {
            bool rtnVal = false;
            long nWait = 0;
            string strNextRoom = "";
            string strRoom = "";
            string strTemp = "";
            long nWRTNO = 0;
            string strGjJong = "";
            string strSname = "";
            string strSex = "";
            long nAge = 0;
            string[] strGubun = { "15", "16", "17", "18", "19" };
            int result = 0;

            strNextRoom = hicSangdamWaitService.GetNextRoomByWrtNoInGubun(FnWRTNO, strGubun);

            //다음 검사실이 없으면
            if (strNextRoom == "")
            {
                clsDB.setBeginTran(clsDB.DbCon);

                result = hicSangdamWaitService.UpdateGbCallbyWrtNo(clsHcVariable.GstrDrRoom, FnWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    rtnVal = false;
                }
                else
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    rtnVal = true;
                }
                return rtnVal;
            }

            strRoom = VB.Pstr(strNextRoom, ",", 1);
            strTemp = VB.Pstr(strNextRoom, strRoom + ",", 2);
            strNextRoom = strTemp;

            //다음 가셔야할곳이 접수창구이면 등록 안함
            if (string.Compare(strRoom, "30") >= 0)
            {
                clsDB.setBeginTran(clsDB.DbCon);

                result = hicSangdamWaitService.DeletebyPaNo(FnPano);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    rtnVal = false;
                    return rtnVal;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                rtnVal = true;
                return rtnVal;
            }

            nWait = hicSangdamWaitService.GetMaxWaitNobyGubun(string.Format("{0:00}", strRoom), "");

            clsDB.setBeginTran(clsDB.DbCon);

            //기존 등록된 대기순번을 삭제함
            result = hicSangdamWaitService.DeletebyPaNo(FnPano);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                rtnVal = false;
                return rtnVal;
            }

            List<HIC_JEPSU> list2 = hicJepsuService.GetItembyPaNo(FnPano);

            if (list2.Count > 0)
            {
                for (int i = 0; i < list2.Count; i++)
                {
                    nWRTNO = list2[i].WRTNO;
                    strGjJong = list2[i].GJJONG.Trim();
                    strSname = list2[i].SNAME.Trim();
                    strSex = list2[i].SEX.Trim();
                    nAge = list2[i].AGE;

                    //상담대기 등록함
                    HIC_SANGDAM_WAIT item = new HIC_SANGDAM_WAIT();

                    item.WRTNO = nWRTNO;
                    item.SNAME = strSname;
                    item.SEX = strSex;
                    item.AGE = nAge;
                    item.GJJONG = strGjJong;
                    item.GUBUN = strRoom;
                    item.WAITNO = nWait;
                    item.PANO = FnPano;
                    item.NEXTROOM = strNextRoom;

                    //상담대기 등록함
                    result = hicSangdamWaitService.Insert(item);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("상담대기 순번등록 중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        rtnVal = false;
                        return rtnVal;
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
            rtnVal = true;
            return rtnVal;
        }

        /// <summary>
        /// READ_공단암검진수검여부
        /// </summary>
        /// <param name="argPaNo"></param>
        /// <param name="argJepDAte"></param>
        /// <returns></returns>
        long fn_Read_PubCorpCancerScreeningYN(long argPaNo, string argJepDate)
        {
            long rtnVal = 0;

            rtnVal = hicJepsuService.GetWrtNobyJepdatePano(argJepDate, argPaNo);

            return rtnVal;
        }

        /// <summary>
        /// 1차 일반상담 저장 - CmdSave1_Click
        /// </summary>
        void fn_Save1()
        {
            string strJinchal1 = "";
            string strJinchal2 = "";
            string strSiksa = "";
            string strRemark = "";
            string strGbHabit = "";
            string strGbOldByeng = "";
            string stroldByengName = "";
            string[] strSick1 = new string[2];
            string[] strSick2 = new string[2];
            string[] strSick3 = new string[2];
            string[] strSick4 = new string[2];
            string[] strSick5 = new string[2];
            string[] strSick6 = new string[2];
            string[] strSick7 = new string[2];
            string[] strSick8 = new string[2];
            string[] strHabit = new string[5];
            string[] strSpcHabit = new string[5];
            string strSpcJinchal1 = "";
            string strSpcJinchal2 = "";
            string[] strOldByeng = new string[7];
            string[] strOLD = new string[14];
            string[] strDrug = new string[7];
            string strMSG = "";
            long nMunDrno = 0;
            string strDiet = "";
            string strSTS = "";
            string strOld13 = "";
            string strOld_Etc = "";
            string strDrug_Etc = "";
            string strDrug_Stop1 = "";
            string strDrug_Stop2 = "";
            string str_B_Drug = "";
            string str_B_Drug1 = "";
            string str_B_Drug11 = "";
            string strBigo = "";
            string strPjSangdam = "";
            string strExCode = "";
            int result = 0;

            strGbHabit = "1";
            strGbOldByeng = "1";
            nMunDrno = 0;

            for (int i = 0; i <= 2; i++)
            {
                strSick1[i] = "2";
                strSick2[i] = "2";
                strSick3[i] = "2";
                strSick4[i] = "2";
                strSick5[i] = "2";
                strSick6[i] = "2";
                strSick7[i] = "2";
                strSick8[i] = "2";
            }

            if (hm.HIC_NEW_MUNITEM_INSERT(FnWRTNO, FstrJong, "", FstrUCodes) != "")
            {
                MessageBox.Show("문진 Table 생성 중 ERROR 발생!!", "상담불가", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                FstrCOMMIT = "NO";
                return;
            }

            //표적장기별 상담
            strPjSangdam = "";
            if (FstrUCodes != "")
            {
                for (int i = 0; i < SS9.ActiveSheet.RowCount; i++)
                {
                    strPjSangdam += SS9.ActiveSheet.Cells[i, 2].Text.Trim() + "{}";
                    strPjSangdam += SS9.ActiveSheet.Cells[i, 1].Text.Trim() + "{$}";
                }
            }

            clsDB.setBeginTran(clsDB.DbCon);

            //GoSub INPUT_DATA_BUILD      'Data Check
            //진단여부 및 약물복용 여부
            strSick1[1] = SS_JinDan.ActiveSheet.Cells[0, 1].Text.Trim() == "1" ? "1" : "2";
            strSick1[2] = SS_JinDan.ActiveSheet.Cells[0, 1].Text.Trim() == "1" ? "1" : "2";

            strSick1[1] = SS_JinDan.ActiveSheet.Cells[1, 1].Text.Trim() == "1" ? "1" : "2";
            strSick1[2] = SS_JinDan.ActiveSheet.Cells[1, 1].Text.Trim() == "1" ? "1" : "2";

            strSick1[1] = SS_JinDan.ActiveSheet.Cells[2, 1].Text.Trim() == "1" ? "1" : "2";
            strSick1[2] = SS_JinDan.ActiveSheet.Cells[2, 1].Text.Trim() == "1" ? "1" : "2";

            strSick1[1] = SS_JinDan.ActiveSheet.Cells[3, 1].Text.Trim() == "1" ? "1" : "2";
            strSick1[2] = SS_JinDan.ActiveSheet.Cells[3, 1].Text.Trim() == "1" ? "1" : "2";

            strSick1[1] = SS_JinDan.ActiveSheet.Cells[4, 1].Text.Trim() == "1" ? "1" : "2";
            strSick1[2] = SS_JinDan.ActiveSheet.Cells[4, 1].Text.Trim() == "1" ? "1" : "2";

            strSick1[1] = SS_JinDan.ActiveSheet.Cells[5, 1].Text.Trim() == "1" ? "1" : "2";
            strSick1[2] = SS_JinDan.ActiveSheet.Cells[5, 1].Text.Trim() == "1" ? "1" : "2";

            strSick1[1] = SS_JinDan.ActiveSheet.Cells[6, 1].Text.Trim() == "1" ? "1" : "2";
            strSick1[2] = SS_JinDan.ActiveSheet.Cells[6, 1].Text.Trim() == "1" ? "1" : "2";

            //일반상태
            if (rdoJinchal20.Checked == true)
            {
                strJinchal2 = "1";
            }

            if (rdoJinchal21.Checked == true)
            {
                strJinchal2 = "2";
            }

            if (rdoJinchal22.Checked == true)
            {
                strJinchal2 = "3";
            }

            //외상 및 후유증
            if (rdoJinchal10.Checked == true)
            {
                strSpcJinchal1 = "1";
            }

            if (rdoJinchal11.Checked == true)
            {
                strSpcJinchal1 = "2";
            }

            //식사여부
            strSiksa = "";
            if (rdoMealState0.Checked == true)
            {
                strSiksa = "Y";
            }
            if (rdoMealState1.Checked == true)
            {
                strSiksa = "N";
            }

            //생활습관 개선필요
            for (int i = 0; i <= 4; i++)
            {
                CheckBox chkHabit = (Controls.Find("chkHabit" + i.ToString(), true)[0] as CheckBox);
                if (chkHabit.Checked == false)
                {
                    strHabit[i] = "0";
                }
                if (chkHabit.Checked == true)
                {
                    strHabit[i] = "1";
                    strGbHabit = "2";
                }
            }

            //과거병력
            for (int i = 0; i <= 6; i++)
            {
                if (chkOldByeng0.Checked == true)
                {
                    strOldByeng[i] = "1";
                    strGbOldByeng = "2";
                }
                else
                {
                    strOldByeng[i] = "0";
                }
            }
            stroldByengName = txtOldByengName.Text.Replace("'", "`");
            if (stroldByengName != "")
            {
                strOldByeng[6] = "1";
            }

            if (txtRemark.Text.Trim() == "")
            {
                strRemark = "특이사항 없음";
            }
            else
            {
                strRemark = txtRemark.Text.Trim();
            }

            //GoSub UPDATE_SANGDAM_DATA   '상담내역 저장
            //자료저장 및 갱신 -------------------------------------
            HIC_SANGDAM_NEW item = new HIC_SANGDAM_NEW();

            item.HABIT1 = strHabit[0];
            item.HABIT2 = strHabit[1];
            item.HABIT3 = strHabit[2];
            item.HABIT4 = strHabit[3];
            item.HABIT5 = strHabit[4];
            item.JINCHAL1 = strJinchal1;
            item.JINCHAL2 = strJinchal2;
            item.T_STAT01 = strSick1[0];
            item.T_STAT02 = strSick1[1];
            item.T_STAT11 = strSick2[0];
            item.T_STAT12 = strSick2[1];
            item.T_STAT21 = strSick3[0];
            item.T_STAT22 = strSick3[1];
            item.T_STAT31 = strSick4[0];
            item.T_STAT32 = strSick4[1];
            item.T_STAT41 = strSick5[0];
            item.T_STAT42 = strSick5[1];
            item.T_STAT51 = strSick6[0];
            item.T_STAT52 = strSick6[1];
            item.T_STAT52_TEC = stroldByengName;
            item.T_STAT61 = strSick7[0];
            item.T_STAT62 = strSick7[1];
            item.T_STAT71 = strSick8[0];
            item.T_STAT72 = strSick8[1];
            item.GBSIKSA = strSiksa;
            item.MUN_OLDMSYM = txtLastMedHis.Text.Trim();
            item.MUN_GAJOK = txtGajok.Text.Trim();
            item.MUN_GIINSUNG = txtGiinsung.Text.Trim();
            item.JIN_01 = txtJinChal0.Text.Trim();
            item.JIN_02 = txtJinChal1.Text.Trim();
            item.JIN_03 = txtJinChal2.Text.Trim();
            item.JIN_04 = txtJinChal3.Text.Trim();
            item.JENGSANG = txtJengSang.Text.Trim();
            item.PJSANGDAM = strPjSangdam;
            item.REMARK = strRemark;
            item.SANGDAMDRNO = FnDrno;
            item.GBSTS = "Y";
            item.ENTSABUN = long.Parse(clsType.User.IdNumber);
            item.WRTNO = FnWRTNO;

            result = hicSangdamNewService.UpdatebyWrtNo(item);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("상담내역 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }

            //GoSub UPDATE_RES_BOHUM1     '일반문진내역 저장
            HIC_RES_BOHUM1 item1 = new HIC_RES_BOHUM1();

            item1.JINCHAL1 = strJinchal1;
            item1.JINCHAL2 = strJinchal2;
            item1.HABIT = strGbHabit;
            item1.HABIT1 = strHabit[0];
            item1.HABIT2 = strHabit[1];
            item1.HABIT3 = strHabit[2];
            item1.HABIT4 = strHabit[3];
            item1.HABIT5 = strHabit[4];
            item1.OLDBYENG = strGbOldByeng;
            item1.OLDBYENG1 = strOldByeng[0];
            item1.OLDBYENG2 = strOldByeng[1];
            item1.OLDBYENG3 = strOldByeng[2];
            item1.OLDBYENG4 = strOldByeng[3];
            item1.OLDBYENG5 = strOldByeng[4];
            item1.OLDBYENG6 = strOldByeng[5];
            item1.OLDBYENG7 = strOldByeng[6];
            item1.OLDBYENGNAME = stroldByengName;
            item1.MUNJINDRNO = FnDrno;
            item1.WRTNO = FnWRTNO;

            result = hicResBohum1Service.UpdateAllbyWrtNo(item1);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("상담내역 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }

            //GoSub UPDATE_RES_SPECIAL    '특수문진내역 저장
            //특수 DB LayOut에 맞게 Data 변환------------------------
            if (strJinchal1 == "1") strSpcJinchal1 = "N";
            if (strJinchal1 == "2") strSpcJinchal1 = "Y";
            if (strJinchal2 == "1") strSpcJinchal2 = "N";
            if (strJinchal2 == "2") strSpcJinchal2 = "Y";

            for (int i = 0; i <= 4; i++)
            {
                if (strHabit[i] == "1") strSpcHabit[i] = "Y";
                if (strHabit[i] == "0") strSpcHabit[i] = "N";
            }
            //---------------------------------------------------------
            HIC_RES_SPECIAL item2 = new HIC_RES_SPECIAL();

            item2.GBHUYU = strSpcJinchal1;
            item2.GBSANGTAE = strSpcJinchal2;
            item2.HABIT1 = strSpcHabit[0];
            item2.HABIT2 = strSpcHabit[1];
            item2.HABIT3 = strSpcHabit[2];
            item2.HABIT4 = strSpcHabit[3];
            item2.HABIT5 = strSpcHabit[4];
            item2.MUN_OLDMSYM = txtLastMedHis.Text.Trim();
            item2.MUN_GAJOK = txtGajok.Text.Trim();
            item2.MUN_GIINSUNG = txtGiinsung.Text.Trim();
            item2.JIN_NEURO = txtJinChal0.Text.Trim();
            item2.JIN_HEAD = txtJinChal1.Text.Trim();
            item2.JIN_SKIN = txtJinChal2.Text.Trim();
            item2.JIN_CHEST = txtJinChal3.Text.Trim();
            item2.JENGSANG = txtJengSang.Text.Trim();
            item2.JINDRNO = FnDrno;
            item2.WRTNO = FnWRTNO;

            result = hicResSpecialService.UpdateAllbyWrtNo(item2);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("상담내역 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }

            //GoSub UPDATE_NIGHT_RESULT   '야간작업 증상문진 저장
            //야간작업 대상이 아니면
            if (tab6.Visible == false)
            {
                return;
            }

            if (cboInsomniaMun.Visible == true) //불면증증상문진
            {
                strExCode = "TZ72";
                result = hicResultService.UpdateResultActivebyWrtNoExCode(FnWRTNO, cboInsomniaMun.Text.Trim(), clsType.User.IdNumber, strExCode);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담내역 저장시 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (cboStomachMun.Visible == true)  //위장관계증상문진
            {
                strExCode = "TZ85";
                result = hicResultService.UpdateResultActivebyWrtNoExCode(FnWRTNO, cboStomachMun.Text.Trim(), clsType.User.IdNumber, strExCode);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담내역 저장시 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (cboBreastCancerMun.Visible == true) //유방암증상문진
            {
                strExCode = "TZ86";
                result = hicResultService.UpdateResultActivebyWrtNoExCode(FnWRTNO, cboBreastCancerMun.Text.Trim(), clsType.User.IdNumber, strExCode);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담내역 저장시 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (cboinsomniaMun2.Visible == true) //불면증증상문진2차
            {
                strExCode = "TZ87";
                result = hicResultService.UpdateResultActivebyWrtNoExCode(FnWRTNO, cboinsomniaMun2.Text.Trim(), clsType.User.IdNumber, strExCode);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담내역 저장시 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //문진에 상담의사 면허번호가 제대로 들어갔는지 체크
            HIC_RES_BOHUM1 list = hicResBohum1Service.GetMunjinDrNobyWrtNo(FnWRTNO);

            if (!list.IsNullOrEmpty())
            {
                nMunDrno = list.MUNJINDRNO;

                HIC_RES_BOHUM1 item3 = new HIC_RES_BOHUM1();

                item3.JINCHAL1 = strJinchal1;

                item3.JINCHAL1 = strJinchal1;
                item3.JINCHAL2 = strJinchal2;
                item3.HABIT = strGbHabit;
                item3.HABIT1 = strHabit[0];
                item3.HABIT2 = strHabit[1];
                item3.HABIT3 = strHabit[2];
                item3.HABIT4 = strHabit[3];
                item3.HABIT5 = strHabit[4];
                item3.OLDBYENG = strGbOldByeng;
                item3.OLDBYENG1 = strOldByeng[0];
                item3.OLDBYENG2 = strOldByeng[1];
                item3.OLDBYENG3 = strOldByeng[2];
                item3.OLDBYENG4 = strOldByeng[3];
                item3.OLDBYENG5 = strOldByeng[4];
                item3.OLDBYENG6 = strOldByeng[5];
                item3.OLDBYENG7 = strOldByeng[6];
                item3.OLDBYENGNAME = stroldByengName;
                item3.MUNJINDRNO = FnDrno;
                item3.WRTNO = FnWRTNO;

                result = hicResBohum1Service.UpdateAllbyWrtNo(item3);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("문진의사 면허번호 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 2차 일반상담 저장 - CmdSave2_Click
        /// </summary>
        void fn_Save2()
        {
            string strSiksa2 = "";
            string strDang = "";
            string strDangJob = "";
            string strGohyul = "";
            string strGohyulJob = "";
            string strRemark = "";
            string[] strSick = new string[2];
            string[] strHabit = new string[2];

            //흡연
            int nSmkScr = 0;
            string strSmkCmb = "";
            string strSmk = "";
            //음주
            int nDrkScr = 0;
            string strDrkCmb = "";
            string strDrk = "";
            //운동
            int nHelScr = 0;
            string strHelCmb = "";
            string strHel2 = "";
            string strHel3 = "";
            string strHel4 = "";
            //영양
            int nDietScr = 0;
            string strDietCmb = "";
            string[] strDiet1 = new string[3];
            string[] strDiet2 = new string[3];
            string[] strDiet3 = new string[2];
            string strDiet4 = "";
            //비만
            string strBimanCmb1 = "";
            string strBimanCmb2 = "";
            string[] strBiman = new string[6];
            //우울증
            int nPHQScr = 0;
            string strPHQCmb = "";
            string strPHQ = "";
            //인지기능
            int nKDSQScr = 0;
            string strKDSQCmb = "";
            string strKDSQ = "";
            string strROWID = "";
            int result = 0;

            //변수 클리어
            //GoSub Input_Data_Clear
            for (int i = 0; i <= 6; i++)
            {
                if (i < 4)
                {
                    strDiet1[i] = "";
                    strDiet2[i] = "";
                }
                if (i < 3)
                {
                    strDiet3[i] = "";
                }
                strBiman[i] = "";
            }

            nSmkScr = 0; nDrkScr = 0; nHelScr = 0; nDietScr = 0;
            strSmkCmb = "";
            strSmk = "";
            strDrkCmb = "";
            strDrk = "";
            strHelCmb = "";
            strHel2 = "";
            strHel3 = "";
            strHel4 = "";
            strDietCmb = "";
            strDiet4 = "";
            strBimanCmb1 = "";
            strBimanCmb2 = "";
            strPHQCmb = "";
            strPHQ = "";
            strKDSQCmb = "";
            strKDSQ = "";

            clsDB.setBeginTran(clsDB.DbCon);

            //GoSub INPUT_DATA_BUILD      'Data Check
            strSmk = "";
            strDrk = "";
            strHel2 = "";
            strHel3 = "";
            strHel4 = "";
            strDang = "";
            strDangJob = "";
            strGohyul = "";
            strGohyulJob = "";

            //당뇨병
            if (lblGen21.Visible == false)
            {
                if (rdoDang0.Checked == true) strDang = "1";
                if (rdoDang1.Checked == true) strDang = "2";
                if (rdoDang2.Checked == true) strDang = "3";
                if (rdoDangJob0.Checked == true) strDangJob = "1";
                if (rdoDangJob1.Checked == true) strDangJob = "2";
                if (rdoDangJob2.Checked == true) strDangJob = "3";
            }

            //고혈압
            if (lblGen20.Visible == false)
            {
                if (rdoGohyul0.Checked == true) strGohyul = "1";
                if (rdoGohyul1.Checked == true) strGohyul = "2";
                if (rdoGohyul2.Checked == true) strGohyul = "3";
                if (rdoGohyulJob0.Checked == true) strGohyulJob = "1";
                if (rdoGohyulJob1.Checked == true) strGohyulJob = "2";
                if (rdoGohyulJob2.Checked == true) strGohyulJob = "3";
            }
            //식사여부
            strSiksa2 = "";
            if (rdoMealState0.Checked == true) strSiksa2 = "Y";
            if (rdoMealState1.Checked == true) strSiksa2 = "N";

            if (txtRemark.Text.Trim() == "")
            {
                strRemark = "특이사항 없음";
            }
            else
            {
                strRemark = txtRemark.Text.Trim();
            }

            if (clsPublic.GstrMsgList != "")
            {
                MessageBox.Show(clsPublic.GstrMsgList, "저장불가", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                FstrCOMMIT = "NO";
                return;
            }

            //GoSub UPDATE_SANGDAM_DATA   '상담내역 저장
            // 자료저장 및 갱신 -------------------------------------
            HIC_SANGDAM_NEW item = new HIC_SANGDAM_NEW();

            item.DIABETES_1 = strDang;
            item.DIABETES_2 = strDangJob;
            item.CYCLE_1 = strGohyul;
            item.CYCLE_2 = strGohyulJob;
            item.GBSIKSA = strSiksa2;
            item.REMARK = strRemark;
            item.GBSTS       = "Y";
            item.SANGDAMDRNO = FnDrno;
            item.ENTSABUN = long.Parse(clsType.User.IdNumber);
            item.WRTNO       = FnWRTNO;

            result = hicResBohum2Service.UpdateDiabetesbyWrtNo(item);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("상담내역 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }

            //상담완료 SET이 제대로 들어갔는지 체크
            if (hicResBohum2Service.GetT_SangdambyWrtNo(FnWRTNO) == "")
            {
                HIC_RES_BOHUM2 item2 = new HIC_RES_BOHUM2();

                item2.DIABETES_RES = strDang;
                item2.DIABETES_RES_CARE = strDangJob;
                item2.CYCLE_RES = strGohyul;
                item2.CYCLE_RES_CARE = strGohyulJob;
                item2.T_SANGDAM = "Y";
                item2.WRTNO = FnWRTNO;

                result = hicResBohum2Service.UpdatebyWrtNo(item2);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담내역 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FstrCOMMIT = "NO";
                    return;
                }
            }
            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 암검진 상담 저장 - CmdSave3_Click
        /// </summary>
        void fn_Save3()
        {
            string strEGD1 = "";
            string strEGD2 = "";
            string strCFS1 = "";
            string strCFS2 = "";
            string strDiet = "";
            string strSTS = "";
            string strOld13 = "";
            string strOld_Etc = "";
            string strDrug_Etc = "";
            string strDrug_Stop1 = "";
            string strDrug_Stop2 = "";
            string str_B_Drug = "";
            string str_B_Drug1 = "";
            string str_B_Drug11 = "";
            string strBigo = "";
            string[] strOLD = new string[14];
            string[] strDrug = new string[7];
            string strOK = "";
            string strMSG = "";
            string strSiksa = "";
            string strJinchal2 = "";
            string strAmSangdam = "";
            string strRemark = "";
            int result = 0;

            //GoSub Build_Insert_Data
            //검사종류
            strEGD1 = "";
            if (chkEGD0.Checked == true) strEGD1 = "1";
            strEGD2 = "";
            if (chkEGD1.Checked == true)strEGD2 = "1";

            strCFS1 = "";
            if (chkCFS0.Checked == true) strCFS1 = "1";
            strCFS2 = "";
            if (chkCFS1.Checked == true) strCFS2 = "1";

            //기록지
            strDiet = "0";
            if (rdoMealState1.Checked == true) strDiet = "1";

            strSTS = "";
            for (int i = 0; i <= 3; i++)
            {
                RadioButton rdoBodyCondition = (Controls.Find("rdoBodyCondition" + i.ToString(), true)[0] as RadioButton);
                if (rdoBodyCondition.Checked == true)
                {
                    strSTS = (i + 1).ToString();
                }
            }

            for (int i = 0; i <= 14; i++)
            {
                strOLD[i] = "";
            }

            if (chkMedHis0.Checked == true) strOLD[0] = "1";    //병력 없음
            if (chkMedHis1.Checked == true) strOLD[1] = "1";
            if (chkMedHis2.Checked == true) strOLD[2] = "1";
            if (chkMedHis3.Checked == true) strOLD[3] = "1";
            if (chkMedHis4.Checked == true) strOLD[4] = "1";
            if (chkMedHis5.Checked == true) strOLD[5] = "1";
            if (chkMedHis6.Checked == true) strOLD[6] = "1";
            if (chkMedHis7.Checked == true) strOLD[7] = "1";
            if (chkMedHis8.Checked == true) strOLD[8] = "1";
            if (chkMedHis9.Checked == true) strOLD[9] = "1";
            if (chkMedHis10.Checked == true) strOLD[10] = "1";
            if (chkMedHis11.Checked == true) strOLD[11] = "1";
            if (chkMedHis12.Checked == true) strOLD[12] = "1";
            if (chkMedHis13.Checked == true) strOLD[13] = "1";
            strOld13 = txtMedHis13.Text.Trim();
            strOld_Etc = txtMedHisEtc.Text.Trim();

            for (int i = 0; i <= 7; i++)
            {
                strDrug[i] = "";
            }

            if (chkMedcine0.Checked == true) strDrug[0] = "1";  //복용약 없음
            if (chkMedcine1.Checked == true) strDrug[1] = "1";
            if (chkMedcine2.Checked == true) strDrug[2] = "1";
            if (chkMedcine3.Checked == true) strDrug[3] = "1";
            if (chkMedcine4.Checked == true) strDrug[4] = "1";
            if (chkMedcine5.Checked == true) strDrug[5] = "1";
            if (chkMedcine6.Checked == true) strDrug[6] = "1";
            if (chkMedcine7.Checked == true) strDrug[7] = "1";

            strDrug_Etc = txtMedcineEtc.Text.Trim();
            strDrug_Stop1 = txtMedAspirin.Text.Trim();
            strDrug_Stop2 = txtAntiCoagulant.Text.Trim();

            str_B_Drug = "";
            if (chkPreTreatment0.Checked == true) str_B_Drug = "1";
            if (chkPreTreatment1.Checked == true) str_B_Drug1 = "1";
            str_B_Drug11 = txtJinjengUse.Text.Trim();

            strBigo = txtRemark.Text.Trim();
            if (strBigo == "") strBigo = "특이사항 없음";

            //GoSub DATA_ERROR_CHECK
            strMSG = "";

            if (Fstr내시경대상 == "Y")
            {
                strOK = "OK";
                if (chkMedHis0.Checked == false)
                {
                    strOK = "";
                    for (int i = 1; i <= 14; i++)
                    {
                        CheckBox chkMedHis = (Controls.Find("chkMedHis" + i.ToString(), true)[0] as CheckBox);
                        if (chkMedHis.Checked == true)
                        {
                            strOK = "OK";
                        }
                    }
                    if (txtMedHisEtc.Text.Trim() != "")
                    {
                        strOK = "OK";
                    }
                    if (strOK  == "")
                    {
                        strMSG += "병력상태를 체크하십시오." + "\r\n";
                    }
                }
                else
                {
                    for (int i = 1; i <= 14; i++)
                    {
                        CheckBox chkMedHis = (Controls.Find("chkMedHis" + i.ToString(), true)[0] as CheckBox);
                        if (chkMedHis.Checked == true)
                        {
                            strOK = "";
                        }
                    }
                    if (txtMedHisEtc.Text.Trim() != "")
                    {
                        strOK = "";
                    }
                    if (strOK == "")
                    {
                        strMSG += "병력상태를 체크하십시오." + "\r\n";
                    }
                }

                strOK = "OK";
                if (chkMedcine0.Checked == false)
                {
                    strOK = "";
                    for (int i = 1; i <= 7; i++)
                    {
                        CheckBox chkMedcine = (Controls.Find("chkMedcine" + i.ToString(), true)[0] as CheckBox);
                        if (chkMedcine.Checked == true)
                        {
                            strOK = "OK";
                        }
                    }
                    if (txtMedcineEtc.Text.Trim() != "")
                    {
                        strOK = "OK";
                    }
                    if (strOK == "")
                    {
                        strMSG += "복용중인 약물상태를 체크하십시오." + "\r\n";
                    }
                }
                else
                {
                    strOK = "";
                    for (int i = 1; i <= 7; i++)
                    {
                        CheckBox chkMedcine = (Controls.Find("chkMedcine" + i.ToString(), true)[0] as CheckBox);
                        if (chkMedcine.Checked == true)
                        {
                            strOK = "";
                        }
                    }
                    if (txtMedcineEtc.Text.Trim() != "")
                    {
                        strOK = "";
                    }
                    if (strOK == "")
                    {
                        strMSG += "복용중인 약물상태를 체크하십시오." + "\r\n";
                    }
                }

                strOK = "OK";
                if (chkPreTreatment0.Checked == false && chkPreTreatment1.Checked == false)
                {
                    strOK = "";
                }
                if (strOK == "")
                {
                    strMSG += "전처치 약제를 체크하십시오." + "\r\n";
                }
            }

            if (strMSG != "")
            {
                MessageBox.Show(strMSG, "저장불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            clsDB.setBeginTran(clsDB.DbCon);

            //GoSub Endo_Chart_Save
            if (FstrEndoRowID != "")
            {
                ENDO_CHART item3 = new ENDO_CHART();

                item3.GUBUN = "3";
                item3.GB_EGD1 = strEGD1;
                item3.GB_EGD2 = strEGD2;
                item3.GB_CFS1 = strCFS1;
                item3.GB_CFS2 = strCFS2;
                item3.GB_DIET = strDiet;
                item3.GB_STS = strSTS;
                item3.GB_OLD = strOLD[0];
                item3.GB_OLD1 = strOLD[1];
                item3.GB_OLD2 = strOLD[2];
                item3.GB_OLD3 = strOLD[3];
                item3.GB_OLD4 = strOLD[4];
                item3.GB_OLD5 = strOLD[5];
                item3.GB_OLD6 = strOLD[6];
                item3.GB_OLD7 = strOLD[7];
                item3.GB_OLD8 = strOLD[8];
                item3.GB_OLD9 = strOLD[9];
                item3.GB_OLD10 = strOLD[10];
                item3.GB_OLD11 = strOLD[11];
                item3.GB_OLD12 = strOLD[12];
                item3.GB_OLD13 = strOLD[13];
                item3.GB_OLD13_1 = strOld13;
                item3.GB_OLD14 = strOLD[14];
                item3.GB_OLD15_1 = strOld_Etc;
                item3.GB_DRUG = strDrug[0];
                item3.GB_DRUG1 = strDrug[1];
                item3.GB_DRUG2 = strDrug[2];
                item3.GB_DRUG3 = strDrug[3];
                item3.GB_DRUG4 = strDrug[4];
                item3.GB_DRUG5 = strDrug[5];
                item3.GB_DRUG6 = strDrug[6];
                item3.GB_DRUG7 = strDrug[7];
                item3.GB_DRUG8_1 = strDrug_Etc;
                item3.GB_DRUG_STOP1 = strDrug_Stop1;
                item3.GB_DRUG_STOP2 = strDrug_Stop2;
                item3.GB_B_DRUG = str_B_Drug;
                item3.GB_B_DRUG1 = str_B_Drug1;
                item3.GB_B_DRUG1_1 = str_B_Drug11;
                item3.GB_BIGO = strBigo;
                item3.ROWID = FstrEndoRowID;

                result = endoChartService.GetRowIdbyPtNo(item3);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("내시경기록지 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FstrCOMMIT = "NO";
                    return;
                }
            }

            strSiksa = "N";
            strAmSangdam = "";
            switch (txtBodySymptom.Text.Trim())
            {
                case "":
                case "특이소견없음":
                    strAmSangdam += "{}";
                    break;
                default:
                    strAmSangdam += txtBodySymptom.Text.Trim() + "{}";
                    break;
            }

            switch (txtCancerHis.Text.Trim())
            {
                case "":
                case "특이소견없음":
                    strAmSangdam += "{}";
                    break;
                default:
                    strAmSangdam += txtCancerHis.Text.Trim() + "{}";
                    break;
            }

            switch (txtStomachBowlLiver.Text.Trim())
            {
                case "":
                case "특이소견없음":
                    strAmSangdam += "{}";
                    break;
                default:
                    strAmSangdam += txtStomachBowlLiver.Text.Trim() + "{}";
                    break;
            }

            switch (txtChestCervical.Text.Trim())
            {
                case "":
                case "특이소견없음":
                    strAmSangdam += "{}";
                    break;
                default:
                    strAmSangdam += txtChestCervical.Text.Trim() + "{}";
                    break;
            }

            strJinchal2 = "3";
            if (rdoBodyCondition0.Checked == true) strJinchal2 = "1";

            strRemark = txtRemark.Text.Trim();
            if (strRemark == "") strRemark = "특이사항 없음";

            HIC_SANGDAM_NEW item = new HIC_SANGDAM_NEW();

            item.JINCHAL2 = strJinchal2;
            item.GBSIKSA = strSiksa;
            item.AMSANGDAM = strAmSangdam;
            item.REMARK = strRemark;
            item.SANGDAMDRNO = FnDrno;
            item.GBSTS = "Y";
            item.ENTSABUN = long.Parse(clsType.User.IdNumber);
            item.WRTNO = FnWRTNO;

            result = hicSangdamNewService.UpdateJinchal2byWrtNo(item);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("상담내역 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            //항정마약 승인
            fn_HIC_HYANG_Approve(FnWRTNO);
            fn_ENDO_EMR_INSERT(FnWRTNO, strEGD1, strEGD2);
        }

        /// <summary>
        ///CmdSave1_NEW_Click
        /// </summary>
        void fn_Save_New()
        {
            string strSiksa2 = "";
            string strDang = "";
            string strDangJob = "";
            string strGohyul = "";
            string strGohyulJob = "";
            string strRemark = "";
            string[] strSick = new string[2];
            string[] strHabit = new string[2];

            //흡연
            int nSmkScr = 0;
            string strSmkCmb = "";
            string strSmk = "";
            //음주
            int nDrkScr = 0;
            string strDrkCmb = "";
            string strDrk = "";
            //운동
            int nHelScr = 0;
            string strHelCmb = "";
            string strHel2 = "";
            string strHel3 = "";
            string strHel4 = "";
            //영양
            int nDietScr = 0;
            string strDietCmb = "";
            string[] strDiet1 = new string[3];
            string[] strDiet2 = new string[3];
            string[] strDiet3 = new string[2];
            string strDiet4 = "";
            //비만
            string strBimanCmb1  = "";
            string strBimanCmb2 = "";
            string[] strBiman = new string[6];
            //우울증
            int nPHQScr = 0;
            string strPHQCmb = "";
            string strPHQ = "";
            //인지기능
            int nKDSQScr = 0;
            string strKDSQCmb = "";
            string strKDSQ = "";
            string strROWID = "";
            int result = 0;

            //변수 클리어
            for (int i = 0; i <= 6; i++)
            {
                if (i < 4)
                {
                    strDiet1[i] = "";
                    strDiet2[i] = "";
                }
                if (i < 3)
                {
                    strDiet3[i] = "";
                }
                strBiman[i] = "";
            }

            nSmkScr = 0;
            nDrkScr = 0;
            nHelScr = 0;
            nDietScr = 0;
            strSmkCmb = "";
            strSmk = "";
            strDrkCmb = "";
            strDrk = "";
            strHelCmb = "";
            strHel2 = "";
            strHel3 = "";
            strHel4 = "";
            strDietCmb = "";
            strDiet4 = "";
            strBimanCmb1 = "";
            strBimanCmb2 = "";
            strPHQCmb = "";
            strPHQ = "";
            strKDSQCmb = "";
            strKDSQ = "";

            strROWID = "";
            strROWID = hicResBohum1Service.GetRowIdbyWrtNo(FnWRTNO);

            clsDB.setBeginTran(clsDB.DbCon);

            //판정테이블 생성
            if (strROWID == "")
            {
                result = hicResBohum1Service.Insert(FnWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("판정 테이블 생성 시 오류 발생(HIC_RES_BOHUM1)", "전산실 연락요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FstrCOMMIT = "NO";
                    return;
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);

            //GoSub INPUT_DATA_BUILD      'Data Check
            strSmk = "";
            strDrk = "";
            strHel2 = "";
            strHel3 = "";
            strHel4 = "";
            strDang = "";
            strDangJob = "";
            strGohyul = "";
            strGohyulJob = "";

            //당뇨병
            if (lblGen21.Visible == false)
            {
                if (rdoDang0.Checked == true) strDang = "1";
                if (rdoDang1.Checked == true) strDang = "2";
                if (rdoDang2.Checked == true) strDang = "3";
                if (rdoDangJob0.Checked == true) strDangJob = "1";
                if (rdoDangJob1.Checked == true) strDangJob = "2";
                if (rdoDangJob2.Checked == true) strDangJob = "3";
            }

            //고혈압
            if (lblGen20.Visible == false)
            {
                if (rdoGohyul0.Checked == true) strGohyul = "1";
                if (rdoGohyul1.Checked == true) strGohyul = "2";
                if (rdoGohyul2.Checked == true) strGohyul = "3";
                if (rdoGohyulJob0.Checked == true) strGohyulJob = "1";
                if (rdoGohyulJob1.Checked == true) strGohyulJob = "2";
                if (rdoGohyulJob2.Checked == true) strGohyulJob = "3";
            }
            //식사여부
            strSiksa2 = "";
            if (rdoMealState0.Checked == true) strSiksa2 = "Y";
            if (rdoMealState1.Checked == true) strSiksa2 = "N";

            if (txtRemark.Text.Trim() == "")
            {
                strRemark = "특이사항 없음";
            }
            else
            {
                strRemark = txtRemark.Text.Trim();
            }

            if (FstrTFlag == "Y")
            {
                //'''        nSmkScr = Val(TxtSmkScr.Text)
                //'''        If ComboSmk1.ListIndex > 0 Then strSmkCmb = Trim(str(ComboSmk1.ListIndex))
                //'''
                //'''        nDrkScr = Val(TxtDrkScr.Text)
                //'''        If ComboDrink1.ListIndex > 0 Then strDrkCmb = Trim(str(ComboDrink1.ListIndex))
                //'''
                //'''        nHelScr = Val(TxtHelthScr.Text)
                //'''        If ComboHelth1.ListIndex > 0 Then strHelCmb = Trim(str(ComboHelth1.ListIndex))
                //'''
                //'''        nDietScr = Val(TxtDietScr.Text)
                //'''        If ComboDiet.ListIndex > 0 Then strDietCmb = Trim(str(ComboDiet.ListIndex))
                //'''
                //'''        If ComboBiman1.ListIndex > 0 Then strBimanCmb1 = Trim(str(ComboBiman1.ListIndex))
                //'''        If ComboBiman2.ListIndex > 0 Then strBimanCmb2 = Trim(str(ComboBiman2.ListIndex))
                //'''
                //'''        '2018 - 01 - 01 start
                //'''        nPHQScr = Val(TxtPHQScr.Text)
                //'''        If ComboPHQ1.ListIndex > 0 Then strPHQCmb = Trim(str(ComboPHQ1.ListIndex))
                //'''
                //'''        nKDSQScr = Val(TxtKDSQScr.Text)
                //'''        If ComboKDSQ1.ListIndex > 0 Then strKDSQCmb = Trim(str(ComboKDSQ1.ListIndex))
                //'''        '2018 - 01 - 01 end
                //'''
                //'''        For i = 1 To 3
                //'''            If i < 3 Then
                //'''                If ChkDiet2(i - 1).Value = 1 Then strDiet3(i) = "1"
                //'''            End If
                //'''            If ChkDiet(i - 1).Value = 1 Then strDiet1(i) = "1"
                //'''            If ChkDiet3(i - 1).Value = 1 Then strDiet2(i) = "1"
                //'''        Next i
                //'''        If ChkDiet4.Value = 1 Then strDiet4 = "1"
                //'''        For i = 1 To 6
                //'''            If ChkBiman(i - 1).Value = 1 Then strBiman(i) = "1"
                //'''        Next i
                //'''
                //'''        For i = 0 To 1
                //'''            If OptKDSQ(i).Value = True Then strKDSQ = Trim(str(i))
                //'''        Next i
                //'''
                //'''        For i = 0 To 2
                //'''            If OptSmk(i).Value = True Then strSmk = Trim(str(i))
                //'''            If OptDrk(i).Value = True Then strDrk = Trim(str(i))
                //'''            If OptHelth4(i).Value = True Then strHel4 = Trim(str(i))
                //'''            If OptPHQ(i).Value = True Then strPHQ = Trim(str(i))
                //'''        Next i
                //'''
                //'''        For i = 0 To 3
                //'''            If OptHelth3(i).Value = True Then strHel3 = Trim(str(i))
                //'''        Next i
                //'''
                //'''        For i = 0 To 7
                //'''            If OptHelth2(i).Value = True Then strHel2 = Trim(str(i))
                //'''        Next i
                //'''
                //'''        '상담이 완료되지 않으면 저장불가
                //'''        GstrMsgList = ""
                //'''        For i = 0 To 4
                //'''            If SSTab_Life.TabVisible(i) = True Then
                //'''                Select Case SSTab_Life.TabCaption(i)
                //'''                    Case "흡연":
                //'''                        If ComboSmk1.ListIndex = 0 Then GstrMsgList = GstrMsgList & "[흡연]니코틴의존도누락,"
                //'''                        If strSmk = "" Then GstrMsgList = GstrMsgList & "[흡연]처방누락,"
                //'''                    Case "음주":
                //'''                        If ComboDrink1.ListIndex = 0 Then GstrMsgList = GstrMsgList & "[음주]평가누락,"
                //'''                        If strDrk = "" Then GstrMsgList = GstrMsgList & "[음주]처방누락,"
                //'''                    Case "운동":
                //'''                        If ComboHelth1.ListIndex = 0 Then GstrMsgList = GstrMsgList & "[운동]평가누락,"
                //'''                        If strHel3 = "" Then GstrMsgList = GstrMsgList & "[운동]처방(시간)누락,"
                //'''                        If strHel4 = "" Then GstrMsgList = GstrMsgList & "[운동]처방(횟수)누락,"
                //'''                        If strHel2 = "" Then GstrMsgList = GstrMsgList & "[운동]처방(종류)누락,"
                //'''                    Case "영양":
                //'''                        If ComboDiet.ListIndex = 0 Then GstrMsgList = GstrMsgList & "[영양]평가누락,"
                //'''                End Select
                //'''            End If
                //'''        Next i
                //'''
                //'''        If GstrMsgList <> "" Then
                //'''            MsgBox GstrMsgList, vbCritical, "저장불가!!"
                //'''            FstrCOMMIT = "NO"
                //'''            Exit Sub
                //'''        End If
            }

            //GoSub UPDATE_SANGDAM_DATA   '상담내역 저장
            // 자료저장 및 갱신 -------------------------------------
            HIC_SANGDAM_NEW item = new HIC_SANGDAM_NEW();

            item.DIABETES_1 = strDang;
            item.DIABETES_2 = strDangJob;
            item.CYCLE_1 = strGohyul;
            item.CYCLE_2 = strGohyulJob;
            item.GBSIKSA = strSiksa2;
            item.REMARK = strRemark;
            item.GBSTS = "Y";
            item.SANGDAMDRNO = FnDrno;
            item.ENTSABUN = long.Parse(clsType.User.IdNumber);
            item.WRTNO = FnWRTNO;

            result = hicSangdamNewService.UpdateDiabetesbyWrtNo(item);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("상담내역 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }
        }

        /// <summary>
        ///학생검진 상담 저장 - School_CmdSave_Click
        /// </summary>
        void fn_School_Save()
        {
            string strFlag = "";
            string strPRes = "";
            string strRemark = "";
            string[] strPPanC = new string[5];
            string[] strPPanK = new string[4];
            string strPPanK_ETC = "";
            string[] strPPanD = new string[6];
            int result = 0;

            strFlag = "";
            //GoSub DATA_ERROR_CHECK
            if (long.Parse(txtPanDrNo.Text) == 0)
            {
                MessageBox.Show("판정의사 면허번호 누락", "확인요망");
                return;
            }
            if (cboPRes.Text.Trim() == "") strFlag = "OK";
            if (cboEJ0.Text.Trim() == "") strFlag = "OK";
            if (cboEJ1.Text.Trim() == "") strFlag = "OK";
            if (cboM.Text.Trim() == "") strFlag = "OK";
            if (cboN.Text.Trim() == "") strFlag = "OK";
            if (cboS.Text.Trim() == "") strFlag = "OK";
            if (cboHJ.Text.Trim() == "") strFlag = "OK";
            if (cboJ.Text.Trim() == "") strFlag = "OK";

            if (strFlag == "OK")
            {
                MessageBox.Show("판정항목 누락.", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }

            if (long.Parse(VB.Left(cboPRes.Text, 2)) > 9)
            {
                strPRes = VB.Left(cboPRes.Text, 2);
            }
            else
            {
                strPRes = VB.Left(cboPRes.Text, 2);
            }

            strPPanC[0] = VB.Left(cboEJ1.Text.Trim(), 1) + "^^" + VB.Left(cboEJ0.Text.Trim(), 1) + "^^" + txtEJEtc.Text.Trim() + "^^";
            strPPanC[1] = VB.Left(cboM.Text.Trim(), 1) + "^^" + txtMEtc.Text.Trim() + "^^";
            strPPanC[2] = VB.Left(cboN.Text.Trim(), 1) + "^^" + txtNEtc.Text.Trim() + "^^";
            strPPanC[3] = VB.Left(cboS.Text.Trim(), 1) + "^^" + txtSEtc.Text.Trim() + "^^";
            strPPanC[4] = VB.Left(cboHJ.Text.Trim(), 1) + "^^" + txtHJEtc.Text.Trim() + "^^";
            //진찰상담
            strPPanK[0] = VB.Left(cboJ.Text.Trim(), 1) + "^^" + txtJEtc.Text.Trim() + "^^";         //진찰 및 상담

            //생활습관개선
            for (int i = 0; i <= 4; i++)
            {
                CheckBox chkHabit = (Controls.Find("chkHabit" + i.ToString(), true)[0] as CheckBox);
                if (chkHabit.Checked == true)
                {
                    if (strPPanK[1] == "")
                    {
                        strPPanK[1] = "2";
                        strPPanK_ETC += chkHabit.Text + ",";
                    }
                }
            }

            strPPanK[1] += strPPanK_ETC + "^^";
            //외상및후유증
            if (rdoJinchal10.Checked == true)
            {
                strPPanK[2] = "1^^" + txtExtInjury.Text + "^^";
            }
            else
            {
                strPPanK[2] = "2^^" + txtExtInjury.Text + "^^";
            }
            //일반상태
            if (rdoJinchal20.Checked == true)
            {
                strPPanK[3] = "1^^" + txtGenStatusEtc + "^^";
            }
            else if (rdoJinchal21.Checked == true)
            {
                strPPanK[3] = "2^^" + txtGenStatusEtc + "^^";
            }
            else
            {
                strPPanK[3] = "3^^" + txtGenStatusEtc + "^^";
            }
            //기관능력
            strPPanD[0] = VB.Left(cboOrgan0.Text.Trim(), 1) + "^^" + txtOrgan0Etc.Text.Trim() + "^^";
            strPPanD[1] = VB.Left(cboOrgan1.Text.Trim(), 1) + "^^" + txtOrgan1Etc.Text.Trim() + "^^";
            strPPanD[2] = VB.Left(cboOrgan2.Text.Trim(), 1) + "^^" + txtOrgan2Etc.Text.Trim() + "^^";
            strPPanD[3] = VB.Left(cboOrgan3.Text.Trim(), 1) + "^^" + txtOrgan3Etc.Text.Trim() + "^^";
            strPPanD[4] = VB.Left(cboOrgan4.Text.Trim(), 1) + "^^" + txtOrgan4Etc.Text.Trim() + "^^";
            strPPanD[5] = VB.Left(cboOrgan5.Text.Trim(), 1) + "^^" + txtOrgan5Etc.Text.Trim() + "^^";

            if (txtRemark.Text.Trim() == "")
            {
                strRemark = "특이사항 없음";
            }
            else
            {
                strRemark = txtRemark.Text.Trim();
            }

            clsDB.setBeginTran(clsDB.DbCon);

            //GoSub UPDATE_SCHOOL_NEW
            HIC_SCHOOL_NEW item = new HIC_SCHOOL_NEW();

            item.PPANB1 = strPRes;
            item.PPANB2 = txtPResEtc.Text.Trim();
            item.PPANC4 = strPPanC[0].Trim();
            item.PPANC6 = strPPanC[4].Trim();
            item.PPANC7 = strPPanC[1].Trim();
            item.PPANC8 = strPPanC[2].Trim();
            item.PPANC9 = strPPanC[3].Trim();
            item.PPAND1 = strPPanD[0].Trim();
            item.PPAND2 = strPPanD[1].Trim();
            item.PPAND3 = strPPanD[2].Trim();
            item.PPAND4 = strPPanD[3].Trim();
            item.PPAND5 = strPPanD[4].Trim();
            item.PPAND6 = strPPanD[5].Trim();            
            item.PPANK1 = strPPanK[0].Trim();
            item.PPANK2 = strPPanK[1].Trim();
            item.PPANK3 = strPPanK[2].Trim();
            item.PPANK4 = strPPanK[3].Trim();
            item.SANGDAM = strRemark; 
            item.WRTNO = FnWRTNO;

            result = hicSchoolNewService.UpdatebyPpanBWrtNo(item);

            //GoSub UPDATE_SANGDAM
            HIC_SANGDAM_NEW item2 = new HIC_SANGDAM_NEW();

            item2.SCHPAN1 = strPRes;
            item2.SCHPAN2 = txtPResEtc.Text.Trim();
            item2.SCHPAN3 = strPPanC[0].Trim();
            item2.SCHPAN4 = strPPanC[1].Trim();
            item2.SCHPAN5 = strPPanC[2].Trim();
            item2.SCHPAN6 = strPPanC[3].Trim();
            item2.SCHPAN7 = strPPanC[4].Trim();
            item2.SCHPAN8 = strPPanK[0].Trim();
            item2.SCHPAN9 = strPPanK[1].Trim();
            item2.SCHPAN10 = strPPanK[2].Trim();
            item2.SCHPAN11 = strPPanK[3].Trim();
            item2.REMARK = strRemark;
            item2.GBSTS = "Y";
            item2.SANGDAMDRNO = FnDrno;
            item2.ENTSABUN = long.Parse(clsType.User.IdNumber);
            item2.WRTNO = FnWRTNO;

            result = hicSangdamNewService.UpdatSchPanbyWrtNo(item2);
        }

        /// <summary>
        ///방사선종사자 상담 저장 - XMunjin_CmdSave_Click
        /// </summary>
        void fn_Xmunjin_Save()
        {
            string strYN = "";
            string strGbn = "";
            string strJilByung = "";
            string[] strBlood = new string[2];
            string[] strSkin = new string[2];
            string strNerv1= "";
            string strNerv2= "";
            string strEye = "";
            string strEye_Etc = "";
            string strCancer = "";
            string strGajok = "";
            string[] strJikJong = new string[2];
            int result = 0;

            strYN = "";
            strGbn = "";

            clsDB.setBeginTran(clsDB.DbCon);

            if (rdo10.Checked == true)
            {
                strYN = "Y";
            }
            else
            {
                strYN = "N";
            }

            if (rdoGubun1.Checked == true)
            {
                strGbn = "Y";
            }
            else
            {
                strGbn = "N";
            }

            if (txtRemark.Text.Trim() == "")
            {
                txtRemark.Text = "특이사항 없음";
            }

            strJilByung = rdoX10.Checked == true ? "0" : "1";

            for (int i = 0; i <= 1; i++)
            {
                CheckBox chkX1_1 = (Controls.Find("chkX1_1" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkX1_2 = (Controls.Find("chkX1_2" + i.ToString(), true)[0] as CheckBox);
                CheckBox chkX4 = (Controls.Find("chkX4" + i.ToString(), true)[0] as CheckBox);
                strBlood[i] = chkX1_1.Checked == true ? "1" : "0";  //혈액관련질환1,2
                strSkin[i] = chkX1_2.Checked == true ? "1" : "0";   //피부질환1,2
                strJikJong[i] = chkX4.Checked == true ? "1" : "0";  //현재직종
            }

            strBlood[1] = txtX1_1.Text.Trim();                //혈액관련질환_기타
            strSkin[1] = txtX1_2.Text.Trim();                 //피부질환_기타
            strNerv1 = txtX1_3.Text.Trim();                   //신경계질환명
            strEye = chkX1_40.Checked == true ? "1" : "0";    //눈질환_백내장
            strEye_Etc = txtX1_4.Text.Trim();                 //눈질환_기타
            strCancer = txtX1_5.Text.Trim();                  //암_기타
            strJikJong[1] = txtJikjong.Text.Trim();           //현재직종

            strGajok = rdoX20.Checked == true ? "0" : "1";

            clsDB.setBeginTran(clsDB.DbCon);

            HIC_X_MUNJIN item = new HIC_X_MUNJIN();

            item.JINGBN = strGbn;
            item.XP1 = strYN;
            item.XPJONG = txtXJong.Text.Trim();
            item.XPLACE = txtPlace.Text.Trim();
            item.XREMARK = txtXRemark.Text.Trim();
            item.XMUCH = txtMuch.Text.Trim();
            item.XTERM = txtTerm.Text.Trim();
            item.XTERM1 = txtXTerm.Text.Trim();
            item.XJUNGSAN = txtJung.Text.Trim();
            item.MUN1 = txtMun1.Text.Trim();
            item.JUNGSAN1 = txtEye.Text.Trim();
            item.JUNGSAN2 = txtSkin.Text.Trim();
            item.JUNGSAN3 = txtEtc.Text.Trim();
            item.SANGDAM = txtRemark.Text.Trim();
            item.MUNDRNO = long.Parse(txtPanDrNo.Text);
            item.JILBYUNG = strJilByung;
            item.BLOOD1 = strBlood[0];
            item.BLOOD2 = strBlood[1];
            item.BLOOD3 = strBlood[2];
            item.SKIN1 = strSkin[0];
            item.SKIN2 = strSkin[1];
            item.SKIN3 = strSkin[2];
            item.NERVOUS1 = txtX1_3.Text.Trim();
            item.EYE1 = strEye;
            item.EYE2 = strEye_Etc;
            item.CANCER1 = strCancer;
            item.GAJOK = strGajok;
            item.BLOOD = txtX2_1.Text.Trim();
            item.NERVOUS2 = txtX2_2.Text.Trim();
            item.CANCER2 = txtX2_3.Text.Trim();
            item.SYMPTON = txtXSymptom.Text.Trim();
            item.JIKJONG1 = strJikJong[0];
            item.JIKJONG2 = strJikJong[1];
            item.JIKJONG3 = strJikJong[2];
            item.WRTNO = FnWRTNO;

            result = hicXMunjinService.UpdatebyWrtNo(item);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("자료를 등록중 오류가 발생함!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }

            HIC_SANGDAM_NEW item2 = new HIC_SANGDAM_NEW();

            item2.REMARK = txtRemark.Text.Trim();
            item2.SANGDAMDRNO = FnDrno;
            item2.GBSTS = "Y";
            item2.ENTSABUN = long.Parse(clsType.User.IdNumber);
            item2.WRTNO = FnWRTNO;

            result = hicSangdamNewService.UpdateRemarkbyWrtNo(item2);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("상담내역 저장시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        ///CmdSAVE_LUNG
        /// </summary>
        void fn_Lung_Save()
        {
            string strResult1 = "";
            string strResult2 = "";
            int result = 0;

            strResult1 = txtLung_Sang1.Text;
            strResult2 = txtLung_Sang2.Text;

            clsDB.setBeginTran(clsDB.DbCon);

            result = hicCancerNewService.UpdateLungSangdambyWrtNo(strResult1, strResult2, clsPublic.GstrSysDate, long.Parse(clsType.User.IdNumber), FnWRTNO);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("폐암사후상담내역 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            result = hicJepsuService.UpdatePanjengDrNobyWrtNo(FnDrno, FnWRTNO);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("폐암사후상담내역 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void fn_SANGDAM_ADD_UPDATE()
        {
            string strROWID = "";
            int result = 0;

            List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDatePtno(FstrJepDate, FstrPtno);

            if (list.Count > 0)
            {
                clsDB.setBeginTran(clsDB.DbCon);
                for (int i = 0; i < list.Count; i++)
                {
                    strROWID = list[i].RID.Trim();

                    result = hicJepsuService.UpdateSangdamDrnobyRowId(clsType.User.IdNumber, strROWID);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("62,69종 상담의사 저장시 오류 발생(HIC_JEPSU)", "전산실 연락요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }                    
                }
                clsDB.setCommitTran(clsDB.DbCon);
            }
        }

        void fn_Update_Result_Data()
        {
            string strCODE = "";
            string strResult = "";
            string strResCode = "";
            string strChange = "";
            string strROWID = "";
            string strNewPan = "";
            int result = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                strCODE = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                strResult = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                strResCode = SS2.ActiveSheet.Cells[i, 5].Text.Trim();
                strChange = SS2.ActiveSheet.Cells[i, 6].Text.Trim();
                strROWID = SS2.ActiveSheet.Cells[i, 7].Text.Trim();

                if (strResCode != "")
                {
                    strResult = VB.Pstr(strResult, ".", 1);
                }

                if (strChange == "Y")
                {
                    strNewPan = hm.ExCode_Result_Panjeng(strCODE, strResult, FstrSex, FstrJepDate, "").Trim();

                    //History에 INSERT
                    result = hicResultHisService.Result_History_Insert(clsType.User.IdNumber, strResult, strROWID, "");

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("검사결과 등록중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FstrCOMMIT = "NO";
                        return;
                    }

                    result = hicResultService.UpdateResultPanjengbyRowId(strResult, strNewPan, strResCode, clsType.User.IdNumber, strROWID);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("검사결과 등록중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FstrCOMMIT = "NO";
                        return;
                    }
                }
            }
            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        ///==========================================================================================
        /// 로직흐름도
        ///   접수번호가 유효한지 체크 -> 상담대상인지 체크 -> 상담대기순번에서 환자 Call 등록
        ///   상담내역이 있는 체크 -> 인적사항 Display -> 상담테이블이 없다면 생성
        ///   2차검진자 1차접수번호 읽기 -> 생애검진자는 생애검진항목 활성화 -> 검사항목 Display
        ///   암검진대상자 체크후 내시경 대상자인지 체크 -> 1차 및 2차 정보 Diplay
        ///==========================================================================================
        /// </summary>
        void fn_Screen_Display()
        {
            int nRead = 0;
            int nRow = 0;
            string strRemark = "";
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strResName = "";
            string strSex = "";
            string strNomal = "";
            string strExPan = "";
            string strTemp = "";
            string strGbSTS = "";
            string strFlag = "";
            string strGjJong = "";
            string strNextRoom = "";
            string strPtNo = "";
            string strPjSangdam = "";
            string strSNAGDAM_LUNG = "";
            string strTemp1 = "";
            string strYear = "";

            string sCode = "";

            //List<string> strList = new List<string>();
            string[] strList = new string[10];

            btnPACS.Enabled = true;
            TabMain.Enabled = true;

            FstrExamFlag = "N";

            FnWRTNO = long.Parse(txtWrtNo.Text);
            if (FnWRTNO == 0)
            {
                return;
            }

            //삭제된것 체크
            if (hb.READ_JepsuSTS(FnWRTNO) == "D")
            {
                MessageBox.Show("접수번호 : " + FnWRTNO + " 는 삭제 된 것입니다. 확인하십시오!", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //상담여부 체크
            strGjJong = hb.READ_GJJONG_CODE(long.Parse(txtWrtNo.Text));
            if (hb.READ_SangDam_Gubun(strGjJong) != "Y")
            {
                MessageBox.Show("접수번호 : " + FnWRTNO + " 는 상담이 없는 검진종류 입니다! 확인하십시오!", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //상담대기 화면에서 환자 Call 등록
            if (clsPublic.GstrRetValue != "1")
            {
                fn_Update_Patient_GbCall();
            }

            //인적사항을 Display
            //GoSub Screen_Injek_display
            FstrUCodes = "";
            FstrGjJong = "";

            HIC_JEPSU_PATIENT list = hicJepsuPatientService.GetItembyWrtNo(FnWRTNO);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호 " + FnWRTNO + "번이 자료가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FstrJong = list.GJJONG.Trim();                                          //검진종류
            FstrChasu = list.GJCHASU.Trim();                                        //검진차수
            FstrPtno = list.PTNO.Trim();                                            //등록번호
            FnPano = list.PANO;                                                     //검진번호
            FnAge = (long)hb.READ_HIC_AGE_GESAN(clsAES.DeAES(list.JUMIN2.Trim()));  //나이
            FstrSex = list.SEX.Trim();                                              //성별
            FstrJepDate = list.JEPDATE.ToString();                                  //접수일자
            FstrJumin = clsAES.DeAES(list.JUMIN2.Trim());                           //주민번호
            FstrYear = list.GJYEAR.Trim();                                          //검진년도
            FstrUCodes = list.UCODES.Trim();                                        //유해물질
            FstrChul = list.GBCHUL.Trim();                                          //출장여부

            //검진자 기본정보 표시---------------
            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PTNO.Trim();
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME.Trim();
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = FnAge + "/" + list.SEX.Trim();
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = list.LTDCODE.ToString();
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = list.JEPDATE.ToString();
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = list.GJJONG.Trim();

            //2차일특여부 체크
            if (FstrChasu == "2" && (FstrJong == "16" || FstrJong == "17" || FstrJong == "19" || FstrJong == "45" || FstrJong == "44" || FstrJong == "46"))
            {
                //2차 검진자는 이전 1차 접수번호를 읽음(FnWRTNO2: 1차접수번호)
                fn_READ_First_Wrtno(FnWRTNO, FstrJong);

                FstrUCodes = hicJepsuService.GetUcodesbyWrtNo(FnWrtno2).Trim(); //유해물질
            }

            if (FstrUCodes != "")
            {
                for (int i = 0; i < FstrUCodes.Length; i++)
                {
                    if (VB.Pstr(FstrUCodes, ",", i) == "")
                    {
                        break;
                    }
                    else
                    {
                        lblSpc.Text += hm.UCode_Names_Display(VB.Pstr(FstrUCodes, ",", i)) + ", ";
                    }
                }
            }

            if (lblSpc.Text.Trim() != "")
            {
                lblSpc.Text = VB.Left(lblSpc.Text, lblSpc.Text.Length - 1);
            }

            if (FstrJong == "11" || FstrJong == "16" || FstrJong == "41" || FstrJong == "44")
            {
                if (FstrUCodes != "")
                {
                    switch (FstrJong)
                    {
                        case "11":
                            FstrGjJong = "91";  //일특 1차
                            break;
                        case "16":
                            FstrGjJong = "92";  //일특 2차
                            break;
                        case "41":
                            FstrGjJong = "93";  //일특 1차(생애)
                            break;
                        case "44":
                            FstrGjJong = "94";  //일특 1차(생애)
                            break;
                        default:
                            break;
                    }
                }
            }

            if (FstrGjJong == "")
            {
                FstrGjJong = list.GJJONG.Trim();
                lblGjJong.Text = hb.READ_GjJong_Name(FstrGjJong);
            }
            else if (FstrGjJong == "91" || FstrGjJong == "93")
            {
                lblGjJong.Text = "일반+특수 1차";
            }
            else if (FstrGjJong == "92" || FstrGjJong == "94")
            {
                lblGjJong.Text = "일반+특수 2차";
            }

            if (clsHcVariable.GbHicAdminSabun == true)
            {
                clsHcVariable.GstrDrRoom = hicSangdamWaitService.GetGubunbyWrtNo(FnWRTNO);
            }

            //당일수검 종류와 상태를 체크
            fn_Read_Jepsu_Display();

            sCode = "3170";
            if (hicSunapdtlService.GetRowIdbyWrtNo(FnWRTNO, sCode) > 0)
            {
                tab7.Visible = true;
                txtLung_Sang1.Text = fn_READ_LUNG_PAN(FnPano);
            }

            //상담화면 세팅
            if (FstrJong == "31" || FstrJong == "35")
            {
                tab3.Visible = true;
                TabMain.SelectedTab = tab3;
                lblTitle0.Text = "신체증상";
                lblTitle1.Text = "암병력(본인,가족)";
                lblTitle2.Text = "위대장간질환";
                lblTitle3.Text = "유방자궁질환";

                lblStatus1.Visible = false;
                lblPTSD.Visible = false;
                lblMealState.Visible = false;
                lblLivingHabit.Visible = false;

                pnlBodySymptom.Visible = true;
                pnlCancerHis.Visible = true;
                pnlStomachBowlLiver.Visible = true;
                pnlChestCervical.Visible = true;

                txtBodySymptom.Text = "특이소견없음";
                txtCancerHis.Text = "특이소견없음";
                txtStomachBowlLiver.Text = "특이소견없음";
                txtChestCervical.Text = "특이소견없음";

                if (hicConsentService.GetCountbyWrtNoSDate(FnWRTNO, FstrJepDate) > 0)
                {
                    btnMenuEndoConsent.Enabled = false;
                }
            }
            else if (FstrJong == "56" || FstrJong == "59")
            {
                tab4.Visible = true;
                TabMain.SelectedTab = tab4;
                lblStatus1.Visible = false;
                lblPTSD.Visible = false;
                lblMealState.Visible = false;
                lblLivingHabit.Visible = false;
            }
            else if (FstrJong == "51" || FstrJong == "50")
            {
                tab5.Visible = true;
                TabMain.SelectedTab = tab5;
            }
            else
            {
                if (FstrChasu == "2")
                {
                    tab2.Visible = true;
                    tab1.Visible = false;
                    lblMealState.Visible = false;
                }
                else
                {
                    tab1.Visible = true;
                    TabMain.SelectedTab = tab1;
                    if ((string.Compare(FstrJong, "11") >= 0 && string.Compare(FstrJong, "19") <= 0) || (string.Compare(FstrJong, "41") >= 0 && string.Compare(FstrJong, "46") <= 0))
                    {
                        lblGenral1.Visible = false;
                    }
                    if (FstrUCodes != "")
                    {
                        lblSpc1.Visible = false;
                        txtLastMedHis.Visible = true;
                        if (Convert.ToDateTime(FstrJepDate) >= Convert.ToDateTime("2017-10-13"))
                        {
                            SS9.Visible = true;
                        }
                        else
                        {
                            SS9.Visible = false;
                        }
                    }
                    lblStatus1.Visible = false;
                    lblPTSD.Visible = false;
                    lblMealState.Visible = false;
                    lblLivingHabit.Visible = false;
                }
            }
            //생활습관문진 항목 변경
            if (FstrJong == "56")
            {
                chkHabit0.Text = "음주";
                chkHabit1.Text = "흡연";
                chkHabit2.Text = "운동";
                chkHabit3.Text = "영양";
                chkHabit4.Text = "비만";
                chkHabit4.Visible = true;
            }
            else
            {
                chkHabit0.Text = "절주";
                chkHabit1.Text = "금연";
                chkHabit2.Text = "운동";
                chkHabit3.Text = "근력";
                chkHabit4.Text = "";
                chkHabit4.Visible = false;
            }
            FstrROWID = "";
            strGbSTS = "";
            strPjSangdam = "";

            //상담내역이 있는지 점검
            HIC_SANGDAM_NEW list2 = hicSangdamNewService.GetItembyWrtNo(FnWRTNO);

            if (!list2.IsNullOrEmpty())
            {
                FstrROWID = list2.RID.Trim();
                strGbSTS = list2.GBSTS.Trim();
                txtRemark.Text = list2.REMARK.Trim();
                strPjSangdam = list2.PJSANGDAM.Trim();
            }
            else
            {
                //상담테이블 없을 시 상담테이블 생성함
                if (fn_HIC_NEW_SANGDAM_INSERT(FnWRTNO, FstrJong) != "")
                {
                    MessageBox.Show("접수번호 " + FnWRTNO + " 신규상담항목 자동발생시 오류가 발생함..전산실에 연락바람..", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            string[] sExCode = { "A118", "A129", "A130", "A143", "A144", "A145", "A146", "A147" };
            //생활습관도구 처방전 TabVisible
            List<HIC_RESULT> list3 = hicResultService.GetResultbyWrtNoExCode(FnWRTNO, sExCode);

            SS3.ActiveSheet.RowCount = list3.Count;
            for (int i = 0; i < list3.Count; i++)
            {
                SS3.ActiveSheet.Cells[i, 1].Text = list3[i].EXCODE.Trim();
                SS3.ActiveSheet.Cells[i, 0].Text = "";
                switch (list3[i].EXCODE.Trim())
                {
                    case "A143":
                        SS3.ActiveSheet.Cells[i, 0].Text = " 흡연";
                        break;
                    case "A144":
                        SS3.ActiveSheet.Cells[i, 0].Text = " 음주";
                        break;
                    case "A145":
                        SS3.ActiveSheet.Cells[i, 0].Text = " 운동";
                        break;
                    case "A146":
                        SS3.ActiveSheet.Cells[i, 0].Text = " 영양";
                        break;
                    case "A147":
                        SS3.ActiveSheet.Cells[i, 0].Text = " 비만";
                        break;
                    case "A130":
                        SS3.ActiveSheet.Cells[i, 0].Text = " 우울증";
                        break;
                    case "A129":
                        SS3.ActiveSheet.Cells[i, 0].Text = " 인지기능";
                        break;
                    case "A118":
                        SS3.ActiveSheet.Cells[i, 0].Text = " 노인신체기능검사";
                        break;
                    default:
                        break;
                }
            }

            btnLivingHabit.Enabled = false;
            if (SS3.ActiveSheet.RowCount > 0)
            {
                lblLivingHabit1.Visible = false;
                btnLivingHabit.Enabled = true;
                FstrTFlag = "Y";
                fn_LivingHabitReport(FnWRTNO);  //Call 생활습관도구표_READ(FnWRTNO)
            }

            //표적장기별 상담
            if (FstrUCodes != "")
            {
                fn_PJ_Display(FnWRTNO, strPjSangdam);
            }

            //GoSub Screen_Exam_Items_display     '검사항목을 Display
            sp.Spread_All_Clear(SS2);

            List<HIC_RESULT_EXCODE> list4 = hicResultExCodeService.GetItemCounselbyWrtNo(FnWRTNO);

            nRead = list4.Count;
            SS2.ActiveSheet.RowCount = nRead;
            nRow = 0;
            for (int i = 0; i < nRead; i++)
            {
                strExCode = list4[i].EXCODE.Trim();
                strResult = list4[i].RESULT.Trim();
                strResCode = list4[i].RESCODE.Trim();
                strResultType = list4[i].RESULTTYPE.Trim();
                strGbCodeUse = list4[i].GBCODEUSE.Trim();

                strFlag = "";
                switch (strExCode)
                {
                    case "TI01":
                        strFlag = "OK";
                        break;
                    case "TI02":
                        strFlag = "OK";
                        break;
                    case "TR11":
                        strFlag = "OK";
                        break;
                    case "TR13":
                        strFlag = "OK";
                        break;
                    case "TH01":
                        strFlag = "OK";
                        break;
                    case "TH02":
                        strFlag = "OK";
                        break;
                    case "A151":
                        strFlag = "OK";
                        break;
                    case "A153":
                        strFlag = "OK";
                        break;
                    case "A294":
                        strFlag = "OK";
                        break;
                    case "TZ01":
                        strFlag = "OK";
                        break;
                    case "TZ12":
                        strFlag = "OK";
                        break;
                    case "ZE76":
                        strFlag = "OK";
                        break;
                    case "ZE77":
                        strFlag = "OK";
                        break;
                    default:
                        break;
                }

                SS2.ActiveSheet.Cells[i, 0].Text = list4[i].EXCODE.Trim();
                SS2.ActiveSheet.Cells[i, 1].Text = list4[i].HNAME.Trim();
                SS2.ActiveSheet.Cells[i, 2].Text = strResult;

                SS2.ActiveSheet.Cells[i, 2].CellType = txt;
                SS2.ActiveSheet.Cells[i, 2].Locked = true;

                if (strFlag == "OK")
                {
                    //Combo_set
                    //string strList = "";
                    int nRead1 = 0;

                    List<HIC_RESCODE> list5 = hicRescodeService.GetCodeNamebyResCode(strResCode);
                        
                    nRead1 = list5.Count;
                    Array.Resize(ref strList, nRead1);

                    if (nRead1 > 0)
                    {   
                        for (int k = 0; k < nRead1; k++)
                        {
                            strList[k] += VB.Left(list5[k].CODE + VB.Space(3), 3) + "." + list5[k].NAME;
                        }

                        combo.Items = strList;
                        combo.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
                        combo.MaxDrop = nRead1;
                        combo.MaxLength = 100;
                        combo.ListWidth = 150;
                        combo.Editable = false;
                        SS2.ActiveSheet.Cells[i, 2].CellType = combo;
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[i, 2].CellType = txt;
                    }
                }

                if (strGbCodeUse == "Y")
                {
                    if (strResult != "")
                    {
                        SS2.ActiveSheet.Cells[i, 2].Text += "." + hb.READ_ResultName(strResCode, strResult);
                    }
                }

                SS2.ActiveSheet.Cells[i, 3].Text = list4[i].PANJENG.Trim();

                //PFT 자동판정
                SS2.ActiveSheet.Cells[i, 6].Text = "";
                if (strExCode == "TR11" && strResult == "")
                {
                    strResult = hm.PFT_Auto_Panjeng(FnWRTNO);
                    if (strResult == "01")
                    {
                        SS2.ActiveSheet.Cells[i, 2].Text = "01.정상";
                        SS2.ActiveSheet.Cells[i, 6].Text = "Y";
                    }
                }

                //이경검사는 자동으로 정상으로 처리
                if ((strExCode == "TI01" || strExCode == "TI02") && strResult == "")
                {
                    SS2.ActiveSheet.Cells[i, 2].Text = "01.정상";
                    SS2.ActiveSheet.Cells[i, 6].Text = "Y";
                }

                //자동계산은 선택못함.
                switch (strExCode)
                {
                    //비만도
                    case "A103":
                    case "TH91":
                    case "TH90":
                    case "A117":
                    case "A116":
                        SS2.ActiveSheet.Cells[i, 2].Locked = true;
                        break;
                    default:
                        break;
                }

                ///TODO : 이상훈(2020.02.20) 참고치 관련 Method 재 구성 예정 (EXAM_NomalValue_SET)
                //참고치를 Dispaly
                //strNomal = hm.EXAM_NomalValue_SET(strExCode, FstrJepDate, FstrSex, list4[i].MIN_M.Trim(), list4[i].MAX_M.Trim(), list4[i].MIN_F.Trim(), list4[i].MAX_F.Trim())

                SS2.ActiveSheet.Cells[i, 4].Text = strNomal;
                SS2.ActiveSheet.Cells[i, 5].Text = strResCode;
                if (list4[i].EXCODE.Trim() == "A151")
                {
                    SS2.ActiveSheet.Cells[i, 5].Text = "007";
                }

                if (list4[i].EXCODE.Trim() == "TH01" || list4[i].EXCODE.Trim() == "TH02")
                {
                    SS2.ActiveSheet.Cells[i, 5].Text = "022";
                }
                SS2.ActiveSheet.Cells[i, 7].Text = list4[i].ROWID.Trim();
                SS2.ActiveSheet.Cells[i, 8].Text = list4[i].RESULTTYPE.Trim();

                strExPan = list4[i].PANJENG.Trim();
                SS2.ActiveSheet.Cells[i, 3].Text = strExPan;

                //야간작업 검사결과가 비정상이면 R로 처리
                if (strExCode == "TZ72" || strExCode == "TZ85" || strExCode == "TZ86")
                {
                    if (SS2.ActiveSheet.Cells[i, 2].Text.Trim() == "비정상")
                    {
                        strExPan = "R";
                        SS2.ActiveSheet.Cells[i, 3].Text = strExPan;
                    }
                }

                //판정결과별 바탕색상을 다르게 표시함
                if (strResult != "")
                {
                    switch (strExPan)
                    {
                        case "B":
                            SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 210, 222);  //정상B
                            break;
                        case "C":
                            SS2.ActiveSheet.Cells[i, 2].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF")); ;  //주의C
                            break;
                        case "R":
                            SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(250, 170, 170);  //질환의심(R)
                            break;
                        default:
                            SS2.ActiveSheet.Cells[i, 2].BackColor = Color.FromArgb(190, 250, 220);  //정상A 또는 기타
                            break;
                    }
                }

                //2차검진 시 상담내역 선택함
                switch (strExCode)
                {
                    case "A148":
                        lblGen21.Visible = false;
                        break;
                    case "A231":
                    case "A232":
                        lblGen20.Visible = false;
                        break;
                    default:
                        break;
                }

                //이경검사를 우선 표시
                //심전도검사를 2순위로 표시
                SS2.ActiveSheet.Cells[i, 9].Text = "9";
                if (strExCode == "TI01" || strExCode == "TI02") SS2.ActiveSheet.Cells[i, 9].Text = "1"; //이경검사
                if (strExCode == "A151" || strExCode == "A153") SS2.ActiveSheet.Cells[i, 9].Text = "2"; //심전도검사
                if (strExCode == "TR11") SS2.ActiveSheet.Cells[i, 9].Text = "3"; //폐활량
                if (strExCode == "ZE76" || strExCode == "ZE77") SS2.ActiveSheet.Cells[i, 9].Text = "4"; //진동각,통각
            }

            //검사결과의 판정값이 R,B인것을 위에 표시
            FarPoint.Win.Spread.SortInfo[] si = new FarPoint.Win.Spread.SortInfo[3];
            si[0] = new FarPoint.Win.Spread.SortInfo(9, true, System.Collections.Comparer.Default);
            si[1] = new FarPoint.Win.Spread.SortInfo(3, false, System.Collections.Comparer.Default);
            si[2] = new FarPoint.Win.Spread.SortInfo(0, false, System.Collections.Comparer.Default);
            SS2_Sheet1.SortRows(0, SS2_Sheet1.RowCount, si);

            fn_Screen_SangDam_display2(FnWRTNO); //2차 상담내역 DISPLAY

            if (FstrChasu == "2")
            {
                fn_Screen_SangDam_display2(FnWRTNO); //2차 상담내역 DISPLAY
            }
            else
            {
                if (FstrGjJong == "31" || FstrGjJong == "35")
                {
                    fn_Screen_Cancer_Display(FnWRTNO);
                }
                else if (FstrGjJong == "50" || FstrGjJong == "51")
                {
                    fn_Screen_XMunjin_Display(FnWRTNO);
                }
                else if (FstrGjJong == "56" || FstrGjJong == "59")
                {
                    fn_Screen_School_Display(FnWRTNO);
                }
                else
                {
                    if (strGbSTS == "Y")    //수정
                    {
                        fn_Screen_SangDam_display(FnWRTNO);
                    }
                    else
                    {
                        fn_Screen_Munjin_Display(FnWRTNO);
                    }
                }
            }

            if (tab7.Visible == true)
            {
                HIC_CANCER_NEW list6 = hicCancerNewService.GetLungbyWrtNo(FnWRTNO);

                if (!list6.IsNullOrEmpty())
                {
                    txtLung_Sang1.Text = list6.LUNG_SANGDAM1.Trim();
                    txtLung_Sang2.Text = list6.LUNG_SANGDAM2.Trim();
                }
            }

            if (long.Parse(txtPanDrNo.Text) == 0)
            {
                txtPanDrNo.Text = clsHcVariable.GnHicLicense.ToString();
            }

            //상담한 의사만 수정이 가능함
            if (long.Parse(txtPanDrNo.Text) != clsHcVariable.GnHicLicense)
            {
                btnSave.Enabled = false;
                //강제 재판정 수검자는 수정이 가능함
                if (fn_RePanjeng_WrtNo_Check(FnWRTNO) == true)
                {
                    btnSave.Enabled = true;
                }
            }
            else
            {
                btnSave.Enabled = true;
            }

            lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));

            //전산실, 건진부장 외 의사사번이 아니면 수정불가
            if (clsHcVariable.GbHicAdminSabun == true)
            {
                btnSave.Enabled = true;
                txtPanDrNo.Enabled = true;
            }
            else if (clsHcVariable.GnHicLicense == 0)
            {
                btnSave.Enabled = false;
                txtPanDrNo.Enabled = false;
            }

            fn_Genjin_HIstory_SET();    //검진 HISTORY

            //문진뷰어
            if (clsPublic.GstrRetValue != "1")
            {
                if (chkMunjin.Checked == false)
                {
                    //검진문진뷰어
                    DirectoryInfo dir = new DirectoryInfo(@"C:\Program Files\SamOmr\");
                    FileInfo[] files = dir.GetFiles();
                    foreach (FileInfo F in files)
                    {
                        if (F.Name == "friendly Omr.exe")
                        {
                            VB.Shell("C:\\Program Files\\SamOmr\\friendly Omr.exe " + FstrPtno + "\\");
                        }
                        //인터넷문진표(New)
                        FrmHcSangInternetMunjinView = new frmHcSangInternetMunjinView(FnWRTNO, FstrJepDate, FstrPtno, FstrGjJong);
                        FrmHcSangInternetMunjinView.Show();
                    }
                }
            }

            //청력 검사결과 유무
            if (etcJupmstService.GetCountbyPtNo(FstrPtno, FstrJepDate, "6", "") > 0)
            {
                btnAudio.Enabled = true;
            }

            //팀파노 검사결과 유무
            if (etcJupmstService.GetCountbyPtNo(FstrPtno, FstrJepDate, "6", "Y") > 0)
            {
                btnAudio1.Enabled = true;
            }

            //폐기능 검사결과 유무
            if (etcJupmstService.GetCountbyPtNo(FstrPtno, FstrJepDate, "4", "") > 0)
            {
                btnPFT.Enabled = true;
                btnMenuLungMun.Enabled = true;
            }

            //검진 당일에 종검수검자인지 점검
            if (heaJepsuService.GetCountbyPtNoJepDate(FstrPtno, FstrJepDate) > 0)
            {
                btnMed.Enabled = true;
            }

            //야간작업 문진표
            fn_screen_Munjin_Night(FnWRTNO);

            //다음 상담.검사실 표시
            this.Text = VB.Pstr(this.Text, "▶다음 검사실", 1).Trim();
            lblWait.Text = "";

            string[] strGubun = { "15", "16", "17", "18", "19" };

            List<HIC_SANGDAM_WAIT> list7 = hicSangdamWaitService.GetNextRoombyWrtNoInGubun(FnWRTNO, strGubun);

            if (list7.Count >= 1)
            {
                strNextRoom = list7[0].NEXTROOM.Trim();
                if (strNextRoom != "")
                {
                    if (strNextRoom == "30")
                    {
                        this.Text += VB.Space(15) + "▶1번:시력.소변실로 수검자를 보내 주십시오.";
                        lblWait.Text = " ▶수검자를 1번:소변.시력실로 보내 주십시오.";
                    }
                    else if (strNextRoom == "31")
                    {
                        this.Text += VB.Space(15) + "▶3번 혈압으로 수검자를 보내 주십시오.";
                        lblWait.Text = " ▶수검자를 3번:혈압으로 보내 주십시오.";
                    }
                    else if (strNextRoom == "32")
                    {
                        this.Text += VB.Space(15) + "▶4번:채혈실로 수검자를 보내 주십시오.";
                        lblWait.Text = " ▶수검자를 4번:채혈실로 보내 주십시오.";
                    }
                    else if (strNextRoom == "33")
                    {
                        this.Text += VB.Space(15) + "▶검사가 완료되었습니다. 접수창구에 제출하십시오.";
                        lblWait.Text = " ▶검사완료 접수창구에 제출하십시오.";
                    }
                    else
                    {
                        HIC_WAIT_ROOM list8 = hicWaitRoomService.GetRoomRoomNamebyRoom(VB.Pstr(strNextRoom, ",", 1));

                        if (!list8.IsNullOrEmpty())
                        {
                            this.Text += VB.Space(15) + "▶다음 검사실 : " + list8.ROOM + "번방(";
                            this.Text += list8.ROOMNAME + ") 입니다.";
                            lblWait.Text = "▶다음검사실: " + list8.ROOM + ".";
                            this.Text += list8.ROOMNAME;
                        }
                    }
                }
            }

            clsPublic.GstrRetValue = "";
        }

        void fn_screen_Munjin_Night(long argWrtNo)
        {
            int nREAD = 0;
            string strChasu = "";
            string strMSG = "";
            string strChar = "";
            string strDAT1="";
            long nJemsu1 = 0;
            string strPAN1 = "";
            string strDAT2="";
            long nJemsu2 = 0;
            string strPAN2 = "";
            string strDAT3 = "";
            long nJemsu3 = 0;
            string strPAN3 = "";

            string strOMR = "";
            string strTemp = "";
            string strSANGOK = "";

            List<string> strExCode = new List<string>();

            //야간작업 문진표 대상자인지 점검
            if (hicSunapdtlService.GetCountbyWrtNOInCode(argWrtNo, clsHcVariable.G36_NIGHT_CODE) == 0)
            {
                tabNight.Visible = false;
                tab6.Visible = false;
                SSJong.ActiveSheet.Cells[0, 5].Text = "";
                return;
            }

            //문진 자료를 Display
            HIC_MUNJIN_NIGHT list = hicMunjinNightService.GetAllbyWrtNo(argWrtNo);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("야간작업 문진표가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
                tabNight.Visible = false;
                tab6.Visible = false;
                SSJong.ActiveSheet.Cells[0, 5].Text = "";
                return;
            }

            tab6.Visible = true;
            tabNight.Visible = true;
            SSJong.ActiveSheet.Cells[0, 5].Text = "○";

            strChasu = FstrChasu;
            if (strChasu != "1" && strChasu != "2")
            {
                switch (FstrJong)
                {
                    case "16":
                    case "17":
                    case "19":
                    case "28":
                    case "29":
                    case "44":
                    case "45":
                        strChasu = "2";
                        break;
                    default:
                        strChasu = "1";
                        break;
                }
            }

            if (strChasu == "1")
            {
                tabNgt1.Visible = true;
                tabNgt2.Visible = false;
                tabNgt3.Visible = false;
                tabNight.SelectedTab = tabNgt1;
            }
            else
            {
                tabNgt1.Visible = false;
                tabNgt2.Visible = true;
                tabNgt3.Visible = true;
                tabNight.SelectedTab = tabNgt2;
            }
            
            if (strChar == "1")
            {
                strDAT1 = list.ITEM1_DATA.Trim();
                nJemsu1 = list.ITEM1_JEMSU;
                strDAT1 = list.ITEM1_PANJENG.Trim();
                for (int i = 0; i <= 6; i++)
                {
                    strChar = VB.Mid(strDAT1, i + 1, 1);
                    SS10.ActiveSheet.Cells[i, 1].Text = strChar;
                    if (string.Compare(strChar, "0") > 0)
                    {
                        SS10.ActiveSheet.Cells[i, 2].Text = (long.Parse(strChar) - 1).ToString();
                        ///TODO : 이상훈(2020.02.25) HcMunjinNight.bas (김민철) 컨버전 후 주석 해제
                        //SS10.ActiveSheet.Cells[i, 3].Text = hf.Munjin_Night_Value1(i, strChar);
                    }
                }
                txtSum1.Text = nJemsu1.ToString();
                ///TODO : 이상훈(2020.02.25) HcMunjinNight.bas (김민철) 컨버전 후 주석 해제
                //lblPan1.Text = hf.Munjin_Night_Panjeng("1", strPAN1);
            }
            else
            {
                //수면의 질
                strDAT2 = list.ITEM2_DATA.Trim();
                nJemsu2 = list.ITEM2_JEMSU;
                strPAN2 = list.ITEM2_PANJENG.Trim();
                for (int i = 1; i <= 18; i++)
                {
                    strChar = VB.Pstr(strDAT2, ",", i);
                    SS11.ActiveSheet.Cells[i, 1].Text = strChar;
                    if (string.Compare(strChar, "0") > 0)
                    {
                        SS11.ActiveSheet.Cells[i, 2].Text = (long.Parse(strChar) - 1).ToString();
                        ///TODO : 이상훈(2020.02.25) HcMunjinNight.bas (김민철) 컨버전 후 주석 해제
                        //SS11.ActiveSheet.Cells[i, 3].Text = hf.Munjin_Night_Value2(i, strChar);
                    }
                }
                txtSum2.Text = nJemsu2.ToString();
                ///TODO : 이상훈(2020.02.25) HcMunjinNight.bas (김민철) 컨버전 후 주석 해제
                //lblPan2.Text = hf.Munjin_Night_Panjeng("2", strPAN2);
                //주간졸림증
                strDAT3 = list.ITEM3_DATA.Trim();
                nJemsu3 = list.ITEM3_JEMSU;
                strPAN3 = list.ITEM3_PANJENG.Trim();
                for (int i = 0; i <= 7; i++)
                {
                    strChar = VB.Mid(strDAT3, i, 1);
                    SS12.ActiveSheet.Cells[i, 1].Text = strChar;
                    if (string.Compare(strChar, "0") > 0)
                    {
                        SS12.ActiveSheet.Cells[i, 2].Text = (long.Parse(strChar) - 1).ToString();
                        ///TODO : 이상훈(2020.02.25) HcMunjinNight.bas (김민철) 컨버전 후 주석 해제
                        //SS12.ActiveSheet.Cells[i, 3].Text = hf.Munjin_Night_Value3(i, strChar);
                    }
                }
                txtSum3.Text = nJemsu3.ToString();
                ///TODO : 이상훈(2020.02.25) HcMunjinNight.bas (김민철) 컨버전 후 주석 해제
                //lblPan3.Text = hf.Munjin_Night_Panjeng("3", strPAN3);
            }

            //야간작업 증상문진 표시
            lblInsomnia.Visible = false;            //불면증증상문진
            cboInsomniaMun.Visible = false;
            lblStomachSymptom.Visible = false;      //위장관계증상문진
            cboStomachMun.Visible = false;
            lblChestCancelMun.Visible = false;      //유방암증상문진
            cboBreastCancerMun.Visible = false;
            lblinsomniaMun2.Visible = false;        //불면증증상문진2차
            cboinsomniaMun2.Visible = false;

            strExCode.Clear();
            strExCode.Add("TZ72");
            strExCode.Add("TZ85");
            strExCode.Add("TZ86");
            strExCode.Add("TZ87");

            List<HIC_RESULT> list2 = hicResultService.GetExCodeResultbyWrtNoExCode(argWrtNo, strExCode);

            for (int i = 0; i < list2.Count; i++)
            {
                if (list2[i].EXCODE.Trim() == "TZ72")
                {
                    lblInsomnia.Visible = true;
                    cboInsomniaMun.Visible = true;
                    cboInsomniaMun.SelectedIndex = 1;
                    if (list2[i].RESULT.Trim() == "비정상")
                    {
                        cboInsomniaMun.SelectedIndex = 2;
                    }
                }
                if (list2[i].EXCODE.Trim() == "TZ85")
                {
                    lblStomachSymptom.Visible = true;
                    cboStomachMun.Visible = true;
                    cboStomachMun.SelectedIndex = 1;
                    if (list2[i].RESULT.Trim() == "비정상")
                    {
                        cboStomachMun.SelectedIndex = 2;
                    }
                }
                if (list2[i].EXCODE.Trim() == "TZ86")
                {
                    lblChestCancelMun.Visible = true;
                    cboBreastCancerMun.Visible = true;
                    cboBreastCancerMun.SelectedIndex = 1;
                    if (list2[i].RESULT.Trim() == "비정상")
                    {
                        cboBreastCancerMun.SelectedIndex = 2;
                    }
                }
                if (list2[i].EXCODE.Trim() == "TZ87")
                {
                    lblinsomniaMun2.Visible = true;
                    cboinsomniaMun2.Visible = true;
                    cboinsomniaMun2.SelectedIndex = 0;
                    for (int j = 0; j < cboinsomniaMun2.Items.Count; j++)
                    {
                        if (VB.Left(cboinsomniaMun2.Items[j].ToString(), 2) == list2[i].RESULT.Trim())
                        {
                            cboinsomniaMun2.SelectedIndex = j;
                            break;
                        }
                    }
                }
            }

            strSANGOK = "";

            if (hicSangdamNewService.GetCountbyWrtNo(argWrtNo) == 0)
            {
                strSANGOK = "OK";
            }

            if (strSANGOK == "OK")
            {
                //불면증, 위장관계, 유방암 증상문진
                strOMR = "";
                strTemp = "";

                HIC_IE_MUNJIN_NEW list3 = hicIeMunjinNewService.GetMunjinResbyWrtNo1(argWrtNo);

                if (!list3.IsNullOrEmpty())
                {
                    if (string.Compare(txtSum1.Text, "15") >= 0)
                    {
                        cboInsomniaMun.SelectedIndex = 2;
                    }

                    strOMR = VB.Pstr(VB.Pstr(VB.Pstr(list3.MUNJINRES.Trim(), "{<*>}tbl_night{*}", 2), "{<*>}", 1), "{*}", 2);

                    //위장관질환 문진표 항목(1,2번)
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 22), ",", 2) == "6" || VB.Pstr(VB.Pstr(strOMR, "{}", 22), ",", 2) == "7" ||
                        VB.Pstr(VB.Pstr(strOMR, "{}", 23), ",", 2) == "2")
                    {
                        cboStomachMun.SelectedIndex = 2;
                    }
                    //위장관질환 문진표 항목(3,4번)
                    if (VB.Pstr(VB.Pstr(strOMR, "{}", 24), ",", 2) == "6" || VB.Pstr(VB.Pstr(strOMR, "{}", 24), ",", 2) == "7" ||
                        VB.Pstr(VB.Pstr(strOMR, "{}", 25), ",", 2) == "2")
                    {
                        cboStomachMun.SelectedIndex = 2;
                    }
                    //위장관질환 문진표 항목(5,6번)
                    if ((VB.Pstr(VB.Pstr(strOMR, "{}", 26), ",", 2) == "5" || VB.Pstr(VB.Pstr(strOMR, "{}", 26), ",", 2) == "6" ||
                        VB.Pstr(VB.Pstr(strOMR, "{}", 26), ",", 2) == "7") && VB.Pstr(VB.Pstr(strOMR, "{}", 27), ",", 2) == "2")
                    {
                        cboStomachMun.SelectedIndex = 2;
                    }
                    //유방암문진표 항목(2번)
                    strTemp = VB.Pstr(VB.Pstr(strOMR, "{}", 29), ",,", 2);
                    if (VB.Pstr(strTemp, "|", 1) == "Y" || VB.Pstr(strTemp, "|", 2) == "Y" || VB.Pstr(strTemp, "|", 3) == "Y")
                    {
                        cboBreastCancerMun.SelectedIndex = 2;
                    }
                }
            }
        }

        void fn_Genjin_HIstory_SET()
        {
            int nRead = 0;
            string strData = "";
            string strJong = "";
            long nHeaPano = 0;

            //종검의 등록번호를 찾음
            nHeaPano = 0;

            nHeaPano = hicPatientService.GetPanobyJumin(clsAES.AES(FstrJumin));

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
                SSHistory.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.ToString();
                SSHistory.ActiveSheet.Cells[i, 3].Text = list[i].GJCHASU;
            }
        }

        bool fn_RePanjeng_WrtNo_Check(long argWrtNo)
        {
            bool rtnVal = false;

            return rtnVal;

        }

        void fn_Screen_Munjin_Display(long argWrtNo)
        {
            string strRowID1 = "";
            string strRowid2 = "";
            string strJong = "";

            long nTotal = 0;
            string[] str_T40_Feel = new string[2];
            string[] str_T66_Feel = new string[3];
            string[] str_T66_Memory = new string[5];
            string[] strJilByung = new string[8];

            FstrMunjin = hicJepsuExjongPatientService.GetGbMunjinbyWrtNo(argWrtNo);

            if (FstrUCodes != "" )
            {
                FstrMunjin = "91";
            }

            strJong = hicJepsuService.GetGjJongbyWrtNo(argWrtNo).Trim();

            strRowID1 = "";
            strRowid2 = "";

            //문진내역이 있는지 점검
            switch (FstrMunjin)
            {
                case "1":
                case "4":   //일반문진
                    if (hicResBohum1Service.GetCountbyWrtNo(argWrtNo) == 0)
                    {
                        if (hm.HIC_NEW_MUNITEM_INSERT(argWrtNo, strJong, "", "") != "")
                        {
                            MessageBox.Show("문진내역이 없습니다(상담불가)", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    break;
                case "3":   //특수문진
                    if (hicResSpecialService.GetCountbyWrtNo(argWrtNo) == 0)
                    {
                        if (hm.HIC_NEW_MUNITEM_INSERT(argWrtNo, strJong, "", "") != "")
                        {
                            MessageBox.Show("문진내역이 없습니다(상담불가)", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    break;
                case "91":
                    strRowID1 = hicResBohum1Service.GetRowIdbyWrtNo(argWrtNo).Trim();   //일반
                    strRowid2 = hicResSpecialService.GetRowIdbyWrtNo(argWrtNo);         //특수
                    if (strRowID1 == "")
                    {
                        if (hm.HIC_NEW_MUNITEM_INSERT(argWrtNo, strJong, "", "") != "")
                        {
                            MessageBox.Show("문진내역이 없습니다(상담불가)", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    if (strRowid2 == "")
                    {
                        if (hm.HIC_NEW_MUNITEM_INSERT(argWrtNo, strJong, "", "") != "")
                        {
                            MessageBox.Show("문진내역이 없습니다(상담불가)", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    break;
                default:
                    break;
            }

            //특수판정 임상진찰
            if (lblSpc1.Visible == false)
            {
                if (txtLastMedHis.Text.Trim() == "") txtLastMedHis.Text = "특이병력 없음";
                if (txtGajok.Text.Trim() == "") txtGajok.Text = "특이병력 없음";
                if (txtGiinsung.Text.Trim() == "") txtGiinsung.Text = "특이사항 없음";
                if (txtJinChal0.Text.Trim() == "") txtJinChal0.Text = "특이사항 없음";
                if (txtJinChal1.Text.Trim() == "") txtJinChal1.Text = "특이사항 없음";
                if (txtJinChal2.Text.Trim() == "") txtJinChal2.Text = "특이사항 없음";
                if (txtJinChal3.Text.Trim() == "") txtJinChal3.Text = "특이사항 없음";
                if (txtJengSang.Text.Trim() == "")
                {
                    txtJengSang.Text = "000";

                    txtJengSang.Text += "." + hb.READ_HIC_CODE("11", txtJengSang.Text.Trim());
                }
                txtLastMedHis.Visible = true;
            }

            if (FstrMunjin == "1" || FstrMunjin == "4" || FstrMunjin == "91")
            {
                //GoSub Munjin_Display   '일반문진
                HIC_RES_BOHUM1 list = hicResBohum1Service.GetItemByWrtno(argWrtNo);

                //현재상태 및 약물복용 여부
                if (list.T_STAT01.Trim() == "1") SS_JinDan.ActiveSheet.Cells[0, 1].Text = "1";
                if (list.T_STAT02.Trim() == "1") SS_JinDan.ActiveSheet.Cells[0, 2].Text = "1";

                if (list.T_STAT11.Trim() == "1") SS_JinDan.ActiveSheet.Cells[1, 1].Text = "1";
                if (list.T_STAT12.Trim() == "1") SS_JinDan.ActiveSheet.Cells[1, 2].Text = "1";

                if (list.T_STAT21.Trim() == "1") SS_JinDan.ActiveSheet.Cells[2, 1].Text = "1";
                if (list.T_STAT22.Trim() == "1") SS_JinDan.ActiveSheet.Cells[2, 2].Text = "1";

                if (list.T_STAT31.Trim() == "1") SS_JinDan.ActiveSheet.Cells[3, 1].Text = "1";
                if (list.T_STAT32.Trim() == "1") SS_JinDan.ActiveSheet.Cells[3, 2].Text = "1";

                if (list.T_STAT41.Trim() == "1") SS_JinDan.ActiveSheet.Cells[4, 1].Text = "1";
                if (list.T_STAT42.Trim() == "1") SS_JinDan.ActiveSheet.Cells[4, 2].Text = "1";

                if (list.T_STAT51.Trim() == "1") SS_JinDan.ActiveSheet.Cells[7, 1].Text = "1";
                if (list.T_STAT52.Trim() == "1") SS_JinDan.ActiveSheet.Cells[7, 2].Text = "1";

                if (list.T_STAT61.Trim() == "1") SS_JinDan.ActiveSheet.Cells[6, 1].Text = "1";
                if (list.T_STAT62.Trim() == "1") SS_JinDan.ActiveSheet.Cells[6, 2].Text = "1";

                if (list.T_STAT71.Trim() == "1") SS_JinDan.ActiveSheet.Cells[5, 1].Text = "1";
                if (list.T_STAT72.Trim() == "1") SS_JinDan.ActiveSheet.Cells[5, 2].Text = "1";

                txtOldByengName.Text = list.OLDBYENGNAME.Trim();    //기타병명

                if (list.JINCHAL2.Trim() == "1") rdoJinchal20.Checked = true;
                if (list.JINCHAL2.Trim() == "2") rdoJinchal21.Checked = true;
                if (list.JINCHAL2.Trim() == "3") rdoJinchal22.Checked = true;

                if (list.JINCHAL1.Trim() == "1") rdoJinchal10.Checked = true;
                if (list.JINCHAL1.Trim() == "2") rdoJinchal11.Checked = true;

                if (list.GBSIKSA.Trim() == "Y")
                {
                    rdoMealState0.Checked = true;
                }
                else
                {
                    rdoMealState1.Checked = true;
                }

                //생애검진
                if (FstrGjJong == "41" || FstrGjJong == "42" || FstrGjJong == "43" || FstrGjJong == "93" || FstrGjJong == "94")
                {
                    str_T40_Feel[0]   = list.T40_FEEL3.Trim();
                    str_T40_Feel[1]   = list.T40_FEEL4.Trim();
                    str_T66_Feel[0]   = list.T66_FEEL1.Trim();
                    str_T66_Feel[1]   = list.T66_FEEL2.Trim();
                    str_T66_Feel[2]   = list.T66_FEEL3.Trim();
                    str_T66_Memory[0] = list.T66_MEMORY1.Trim();
                    str_T66_Memory[1] = list.T66_MEMORY2.Trim();
                    str_T66_Memory[2] = list.T66_MEMORY3.Trim();
                    str_T66_Memory[3] = list.T66_MEMORY4.Trim();
                    str_T66_Memory[4] = list.T66_MEMORY5.Trim();
                }

                txtRemark.Text = list.SANGDAM.Trim();

                for (int i = 0; i <= 7; i++)
                {
                    strJilByung[i] = SS_JinDan.ActiveSheet.Cells[i, 2].Text.Trim();
                }

                if (strJilByung[0] == "1") txtRemark.Text += "\r\n" + "뇌졸중 약물치료중()년";
                if (strJilByung[1] == "1") txtRemark.Text += "\r\n" + "심장병 약물치료중()년";
                if (strJilByung[2] == "1") txtRemark.Text += "\r\n" + "고혈압 약물치료중()년";
                if (strJilByung[3] == "1") txtRemark.Text += "\r\n" + "당뇨병 약물치료중()년";
                if (strJilByung[4] == "1") txtRemark.Text += "\r\n" + "이상지질혈증 약물치료중()년";
                if (strJilByung[5] == "1") txtRemark.Text += "\r\n" + "간장질환 약물치료중()년";
                if (strJilByung[6] == "1") txtRemark.Text += "\r\n" + "폐결핵 약물치료중()년";
                if (strJilByung[7] == "1") txtRemark.Text += "\r\n" + "기타 약물치료중()년";
            }
        }

        /// <summary>
        /// 1차 상담 내역 DISPLAY
        /// </summary>
        /// <param name="argWrtNo"></param>
        void fn_Screen_SangDam_display(long argWrtNo)
        {
            HIC_SANGDAM_NEW list = hicSangdamNewService.GetAllbyWrtNo(argWrtNo);

            //외상,휴유증
            if (list.JINCHAL1 == "1")
            {
                rdoJinchal10.Checked = true;
            }
            else if (list.JINCHAL1 == "2")
            {
                rdoJinchal11.Checked = true;
            }

            //일반상태(양호,보통,불량)
            switch (list.JINCHAL2)
            {
                case "1":
                    rdoJinchal20.Checked = true;
                    break;
                case "2":
                    rdoJinchal21.Checked = true;
                    break;
                case "3":
                    rdoJinchal22.Checked = true;
                    break;
                default:
                    break;
            }

            //식사여부
            if (list.GBSIKSA.Trim() == "Y")
            {
                rdoMealState0.Checked = true;
            }
            else
            {
                rdoMealState1.Checked = true;
            }

            //뇌졸중
            if (list.T_STAT01.Trim() == "1") SS_JinDan.ActiveSheet.Cells[0, 1].Text = "1";
            if (list.T_STAT02.Trim() == "1") SS_JinDan.ActiveSheet.Cells[0, 2].Text = "1";
            //심장병
            if (list.T_STAT11.Trim() == "1") SS_JinDan.ActiveSheet.Cells[1, 1].Text = "1";
            if (list.T_STAT12.Trim() == "1") SS_JinDan.ActiveSheet.Cells[1, 2].Text = "1";
            //고혈압
            if (list.T_STAT21.Trim() == "1") SS_JinDan.ActiveSheet.Cells[2, 1].Text = "1";
            if (list.T_STAT22.Trim() == "1") SS_JinDan.ActiveSheet.Cells[2, 2].Text = "1";
            //당뇨
            if (list.T_STAT31.Trim() == "1") SS_JinDan.ActiveSheet.Cells[3, 1].Text = "1";
            if (list.T_STAT32.Trim() == "1") SS_JinDan.ActiveSheet.Cells[3, 2].Text = "1";
            //이상지질혈증
            if (list.T_STAT41.Trim() == "1") SS_JinDan.ActiveSheet.Cells[4, 1].Text = "1";
            if (list.T_STAT42.Trim() == "1") SS_JinDan.ActiveSheet.Cells[4, 2].Text = "1";
            //기타질환
            if (list.T_STAT51.Trim() == "1") SS_JinDan.ActiveSheet.Cells[7, 1].Text = "1";
            if (list.T_STAT52.Trim() == "1") SS_JinDan.ActiveSheet.Cells[7, 2].Text = "1";
            //간장질환
            if (list.T_STAT61.Trim() == "1") SS_JinDan.ActiveSheet.Cells[5, 1].Text = "1";
            if (list.T_STAT62.Trim() == "1") SS_JinDan.ActiveSheet.Cells[5, 2].Text = "1";
            //폐결핵
            if (list.T_STAT71.Trim() == "1") SS_JinDan.ActiveSheet.Cells[6, 1].Text = "1";
            if (list.T_STAT72.Trim() == "1") SS_JinDan.ActiveSheet.Cells[6, 2].Text = "1";

            //생활습관 개선필요
            if (list.HABIT1.Trim() == "1")
            {
                chkHabit0.Checked = true;
            }
            if (list.HABIT2.Trim() == "1")
            {
                chkHabit1.Checked = true;
            }
            if (list.HABIT3.Trim() == "1")
            {
                chkHabit2.Checked = true;
            }
            if (list.HABIT4.Trim() == "1")
            {
                chkHabit3.Checked = true;
            }
            if (list.HABIT5.Trim() == "1")
            {
                chkHabit4.Checked = true;
            }

            txtOldByengName.Text = list.T_STAT52_TEC.Trim();    //기타병명
            txtRemark.Text = list.REMARK.Trim();                //상담내역

            txtLastMedHis.Text = list.MUN_OLDMSYM.Trim();       //과거병력
            txtGajok.Text = list.MUN_GAJOK.Trim();              //가족력
            txtGiinsung.Text = list.MUN_GIINSUNG.Trim();        //업무기인성

            //자타각증상
            txtJengSang.Text = list.JENGSANG.Trim() + "." + hb.READ_HIC_CODE("11", txtJengSang.Text.Trim());

            //임상진찰
            txtJinChal0.Text = list.JIN_01;
            txtJinChal1.Text = list.JIN_02;
            txtJinChal2.Text = list.JIN_03;
            txtJinChal3.Text = list.JIN_04;

            txtPanDrNo.Text = list.SANGDAMDRNO.ToString();
        }

        void fn_Screen_School_Display(long argWrtNo)
        {
            string strGbSTS = "";
            string strROWID = "";

            //상담내역이 있는지 점검(판정)
            strROWID = hicSchoolNewService.GetRowIdbyWrtNo(argWrtNo);

            //판정테이블 없을 시 판정테이블 생성함
            if (strROWID == "")
            {
                if (fn_HIC_NEW_SCHOOL_INSERT(argWrtNo, "56") != "")
                {
                    MessageBox.Show("접수번호 " + argWrtNo + " 신규판정테이블 자동발생시 오류가 발생함..전산실에 연락바람..", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            HIC_SANGDAM_NEW list = hicSangdamNewService.GetSangdamDrNoAmSangdambyWrtNo(FnWRTNO);

            if (!list.IsNullOrEmpty())
            {
                txtPanDrNo.Text = list.SANGDAMDRNO.ToString();
                lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));
                strGbSTS = list.GBSTS.Trim();
            }

            if (strGbSTS == "Y")
            {
                List<HIC_SCHOOL_NEW> list2 = hicSchoolNewService.GetItembyWrtNo(argWrtNo);

                if (list2.Count > 0)
                {
                    cboPRes.SelectedIndex = int.Parse(list2[0].PPANB1);
                    txtPResEtc.Text = list2[0].PPANB2.Trim();
                    cboEJ0.SelectedIndex = int.Parse(VB.Pstr(list2[0].PPANC4.Trim(), "^^", 2));
                    cboEJ1.SelectedIndex = int.Parse(VB.Pstr(list2[0].PPANC4.Trim(), "^^", 1));
                    txtEJEtc.Text = VB.Pstr(list2[0].PPANC4.Trim(), "^^", 3);
                    cboM.SelectedIndex = int.Parse(VB.Pstr(list2[0].PPANC7.Trim(), "^^", 1));
                    txtMEtc.Text = VB.Pstr(list2[0].PPANC7.Trim(), "^^", 2);
                    cboN.SelectedIndex = int.Parse(VB.Pstr(list2[0].PPANC8.Trim(), "^^", 1));
                    txtNEtc.Text = VB.Pstr(list2[0].PPANC8.Trim(), "^^", 2);
                    cboS.SelectedIndex = int.Parse(VB.Pstr(list2[0].PPANC9.Trim(), "^^", 1));
                    txtSEtc.Text = VB.Pstr(list2[0].PPANC9.Trim(), "^^", 2);
                    cboHJ.SelectedIndex = int.Parse(VB.Pstr(list2[0].PPANC6.Trim(), "^^", 1));
                    txtHJEtc.Text = VB.Pstr(list2[0].PPANC6.Trim(), "^^", 2);
                    //기관능력
                    cboOrgan0.SelectedIndex = int.Parse(VB.Pstr(list2[0].PPAND1.Trim(), "^^", 1));
                    txtOrgan0Etc.Text = VB.Pstr(list2[0].PPAND1.Trim(), "^^", 2);
                    cboOrgan1.SelectedIndex = int.Parse(VB.Pstr(list2[0].PPAND2.Trim(), "^^", 1));
                    txtOrgan1Etc.Text = VB.Pstr(list2[0].PPAND2.Trim(), "^^", 2);
                    cboOrgan2.SelectedIndex = int.Parse(VB.Pstr(list2[0].PPAND3.Trim(), "^^", 1));
                    txtOrgan2Etc.Text = VB.Pstr(list2[0].PPAND3.Trim(), "^^", 2);
                    cboOrgan3.SelectedIndex = int.Parse(VB.Pstr(list2[0].PPAND4.Trim(), "^^", 1));
                    txtOrgan3Etc.Text = VB.Pstr(list2[0].PPAND4.Trim(), "^^", 2);
                    cboOrgan4.SelectedIndex = int.Parse(VB.Pstr(list2[0].PPAND5.Trim(), "^^", 1));
                    txtOrgan4Etc.Text = VB.Pstr(list2[0].PPAND5.Trim(), "^^", 2);
                    cboOrgan5.SelectedIndex = int.Parse(VB.Pstr(list2[0].PPAND6.Trim(), "^^", 1));
                    txtOrgan5Etc.Text = VB.Pstr(list2[0].PPAND6.Trim(), "^^", 2);
                    //진촬및상담
                    cboJ.SelectedIndex = int.Parse(VB.Pstr(list2[0].PPANK1.Trim(), "^^", 1));
                    txtJEtc.Text = VB.Pstr(list2[0].PPANK1.Trim(), "^^", 2);
                    //생활습관개선
                    HIC_SANGDAM_NEW list3 = hicSangdamNewService.GetHabitbyWrtNo(argWrtNo);

                    if (list3.HABIT1.Trim() == "1")
                    {
                        chkHabit0.Checked = true;
                    }
                    else
                    {
                        chkHabit0.Checked = false;
                    }

                    if (list3.HABIT2.Trim() == "1")
                    {
                        chkHabit1.Checked = true;
                    }
                    else
                    {
                        chkHabit1.Checked = false;
                    }

                    if (list3.HABIT3.Trim() == "1")
                    {
                        chkHabit2.Checked = true;
                    }
                    else
                    {
                        chkHabit2.Checked = false;
                    }

                    if (list3.HABIT4.Trim() == "1")
                    {
                        chkHabit3.Checked = true;
                    }
                    else
                    {
                        chkHabit3.Checked = false;
                    }

                    if (list3.HABIT5.Trim() == "1")
                    {
                        chkHabit4.Checked = true;
                    }
                    else
                    {
                        chkHabit4.Checked = false;
                    }

                    //외상후유증
                    if (long.Parse(VB.Pstr(list2[0].PPANK4.Trim(), "^^", 1)) == 1)
                    {
                        rdoJinchal10.Checked = true;
                    }
                    else
                    {
                        rdoJinchal11.Checked = true;
                    }
                    txtExtInjury.Text = VB.Pstr(list2[0].PPANK3.Trim(), "^^", 2);

                    //외상후유증
                    if (long.Parse(VB.Pstr(list2[0].PPANK4.Trim(), "^^", 1)) == 1)
                    {
                        rdoJinchal20.Checked = true;
                    }
                    else if (long.Parse(VB.Pstr(list2[0].PPANK4.Trim(), "^^", 1)) == 1)
                    {
                        rdoJinchal21.Checked = true;
                    }
                    else if (long.Parse(VB.Pstr(list2[0].PPANK4.Trim(), "^^", 1)) == 2)
                    {
                        rdoJinchal22.Checked = true;
                    }
                    txtGenStatusEtc.Text = VB.Pstr(list2[0].PPANK4.Trim(), "^^", 2);
                    txtRemark.Text = list2[0].SANGDAM.Trim();
                }
            }
        }

        string fn_HIC_NEW_SCHOOL_INSERT(long argWrtNo, string ArgJong)
        {
            string rtnVal = "";
            string strRowId = "";
            int result = 0;

            strRowId = hicSchoolNewService.GetRowIdbyWrtNo(argWrtNo).Trim();

            if (strRowId == "")
            {
                clsDB.setBeginTran(clsDB.DbCon);

                result = hicSchoolNewService.InsertWrtNo(argWrtNo);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    rtnVal = "암문진생성시 오류";
                    return rtnVal;
                }
                clsDB.setCommitTran(clsDB.DbCon);
            }
            return rtnVal;
        }

        void fn_Screen_XMunjin_Display(long argWrtNo)
        {
            int nJil = 0;
            int nGajok = 0;

            fn_HIC_X_MUNJIN_INSERT();

            //건강검진 문진표 및 결과를  READ
            HIC_X_MUNJIN list = hicXMunjinService.GetItembyWrtNo(argWrtNo);

            if (list.IsNullOrEmpty())
            {
                MessageBox.Show("접수번호 " + argWrtNo + "는 결과 및 판정이 등록 안됨", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                if (list.XP1.Trim() == "Y")
                {
                    rdo10.Checked = true;
                }
                else if (list.XP1.Trim() == "N")
                {
                    rdo11.Checked = true;
                }

                if (list.JINGBN.Trim() == "Y")
                {
                    rdoGubun1.Checked = true;
                }
                else
                {
                    rdoGubun2.Checked = true;
                }

                txtXJong.Text = list.XPJONG.Trim();
                txtPlace.Text = list.XPLACE.Trim();
                txtXRemark.Text = list.XREMARK.Trim();
                txtTerm.Text = list.XTERM.Trim();
                txtXTerm.Text = list.XTERM1.Trim();
                txtMuch.Text = list.XMUCH.Trim();
                txtJung.Text = list.XJUNGSAN.Trim();
                txtMun1.Text = list.MUN1.Trim();
                txtEye.Text = list.JUNGSAN1.Trim();
                txtSkin.Text = list.JUNGSAN2.Trim();
                txtEtc.Text = list.JUNGSAN3.Trim();
                txtRemark.Text = list.SANGDAM.Trim();
                txtPanDrNo.Text = list.MUNDRNO.ToString();
                cboXYear.Text = "";
                cboXMonth.Text = "";

                if (txtXTerm.Text.Trim() != "")
                {
                    if (VB.InStr(txtXTerm.Text, "년") > 0)
                    {
                        cboXYear.Text = string.Format("{0:00}", VB.Pstr(txtXTerm.Text, "년", 1));
                    }
                    if (VB.InStr(txtXTerm.Text, "년") > 0)
                    {
                        if (VB.InStr(txtXTerm.Text, "개월") > 0)
                        {
                            cboXMonth.Text = string.Format("{0:00}", VB.Pstr(VB.Pstr(txtXTerm.Text, "년", 2), "개월", 1));
                        }
                        else if (VB.InStr(txtXTerm.Text, "월") > 0)
                        {
                            cboXMonth.Text = string.Format("{0:00}", VB.Pstr(VB.Pstr(txtXTerm.Text, "년", 2), "월", 1));
                        }
                    }
                    else
                    {
                        cboXYear.Text = "";
                        if (VB.InStr(txtXTerm.Text, "개월") > 0)
                        {
                            cboXMonth.Text = string.Format("{0:00}", VB.Pstr(VB.Pstr(txtXTerm.Text, "년", 1), "개월", 1));
                        }
                        else if (VB.InStr(txtXTerm.Text, "월") > 0)
                        {
                            cboXMonth.Text = string.Format("{0:00}", VB.Pstr(VB.Pstr(txtXTerm.Text, "년", 1), "월", 1));
                        }
                    }
                }

                //방사선종사자 컬럼 추가
                nJil = int.Parse(list.JILBYUNG);    //과거 질병력
                rdoX10.Checked = true;

                chkX1_10.Checked = list.BLOOD1 == "1" ? true : false;   //혈액관련질환(빈혈)
                chkX1_11.Checked = list.BLOOD2 == "1" ? true : false;   //혈액관련질환(백혈병)
                if (list.BLOOD3.Trim() != "")   //혈액관련질환(기타)
                {
                    chkX1_12.Checked = true;
                    txtX1_1.Text = list.BLOOD3;
                }
                chkX1_20.Checked = list.SKIN1 == "1" ? true : false;    //피부질환(아토피)
                chkX1_21.Checked = list.SKIN2 == "1" ? true : false;    //피부질환(습진)
                if (list.SKIN3.Trim() != "")    //피부질환(기타)
                {
                    chkX1_22.Checked = true;
                    txtX1_2.Text = list.SKIN3.Trim();
                }
                txtX1_3.Text = list.NERVOUS1.Trim();    //신경계질환명
                chkX1_40.Checked = list.EYE1 == "1" ? true : false;   //눈 질환(백내장)
                if (list.EYE2.Trim() != "")         //눈 질환(기타)
                {
                    chkX1_41.Checked = true;
                    txtX1_4.Text = list.EYE2.Trim();
                }
                txtX1_5.Text = list.CANCER1.Trim(); //암 질환명

                nGajok = int.Parse(list.GAJOK);              //가족력
                RadioButton rdoX2 = (Controls.Find("rdoX2" + nGajok.ToString(), true)[0] as RadioButton);
                rdoX2.Checked = true;

                txtX2_1.Text = list.BLOOD.Trim();           //혈액관련질환명
                txtX2_2.Text = list.NERVOUS2.Trim();        //신경계질환명
                txtX2_3.Text = list.CANCER2.Trim();         //암 질환명
                txtXSymptom.Text = list.SYMPTON.Trim();     //최근 특이증상

                chkX40.Checked = list.JIKJONG1 == "1" ? true : false;  //현재 직종(비파괴검사)
                chkX41.Checked = list.JIKJONG2 == "1" ? true : false;  //현재 직종(방사선사)
                if (list.JIKJONG3.Trim() != "")         //눈 질환(기타)
                {
                    chkX42.Checked = true;
                    txtJikjong.Text = list.JIKJONG3.Trim();
                }
            }
        }

        void fn_HIC_X_MUNJIN_INSERT()
        {
            int nREAD = 0;
            long nWrtNo = 0;
            string strJepDate = "";
            string strFrDate = "";
            string strToDate = "";
            long nLtdCode = 0;
            int result = 0;

            strFrDate = dtpFrDate.Text;
            strToDate = dtpToDate.Text;
            nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));

            List<HIC_JEPSU> list = hicJepsuService.GetWrtNoJepDatebyJepDate(strFrDate, strToDate, nLtdCode);

            nREAD = list.Count;
            if (nREAD == 0) return;

            clsDB.setBeginTran(clsDB.DbCon);
            for (int i = 0; i < nREAD; i++)
            {
                nWrtNo = list[i].WRTNO;
                strJepDate = list[i].JEPDATE;

                if (hicXMunjinService.GetCountbyWrtNo(nWrtNo) == 1)
                {
                    //Update
                    result = hicXMunjinService.UpdateJepDatebyWrtNo(strJepDate, nWrtNo);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("자료를 등록중 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    //Insert
                    result = hicXMunjinService.Insert(strJepDate, nWrtNo);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("자료를 등록중 오류가 발생함", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        void fn_Screen_Cancer_Display(long argWrtNo)
        {
            string strAm = "";
            string strExCode = "";
            string strAmSangdam = "";

            strAm = hicJepsuService.GetGbAmbyWrtNo(argWrtNo).Trim();

            if (strAm != "")
            {
                for (int i = 0; i < VB.L(strAm, ",") - 1; i++)
                {
                    if (VB.Pstr(strAm, ",", i) == "1")
                    {
                        CheckBox chkJong = (Controls.Find("chkJong" + i.ToString(), true)[0] as CheckBox);
                        chkJong.Checked = true;
                    }
                }
            }

            //GoSub Screen_Endo_Exam_Check        '내시경이 있는지 Check
            Fstr내시경대상 = "";

            List<HIC_RESULT> list = hicResultService.GetExCodeLoopbyWrtNo(argWrtNo);

            for (int i = 0; i < list.Count; i++)
            {
                strExCode = list[i].EXCODE.Trim();
                if (strExCode == "TX20" || strExCode == "TX23" || strExCode == "TX32" || strExCode == "TX41" || strExCode == "TX64")
                {
                    Fstr내시경대상 = "Y";
                    lblEndo.Visible = false;
                    switch (strExCode)
                    {
                        case "TX20":
                            chkEGD0.Checked = true;
                            chkEGD1.Checked = true;
                            break;
                        case "TX23":
                            chkEGD0.Checked = true;
                            break;
                        case "TX32":
                            chkCFS0.Checked = true;
                            break;
                        case "TX41":
                            chkEGD0.Checked = true;
                            chkEGD1.Checked = true;
                            chkCFS0.Checked = true;
                            chkCFS1.Checked = true;
                            break;
                        case "TX64":
                            chkCFS0.Checked = true;
                            chkCFS1.Checked = true;
                            break;
                        default:
                            break;
                    }
                }

                //(위내시경 수면체크관련 추가)
                HIC_EXCODE list2 = hicExcodeService.GetCodeEndoGubun3byExCode(strExCode);

                if (!list2.IsNullOrEmpty())
                {
                    if (list2.ENDOGUBUN3.Trim() == "Y")
                    {
                        chkEGD1.Checked = true;
                    }
                }
            }

            SSHyang.ActiveSheet.RowCount = 0;
            if (Fstr내시경대상 == "Y")
            {
                //내시경 대상자 이면 내시경챠트 테이블 생성함
                if (fn_Read_Endo_Chart(FstrPtno, FstrJepDate) != "")
                {
                    MessageBox.Show("접수번호 : " + FnWRTNO + " 내시경기록지 자동발생시 오류가 발생함..전산실에 연락바람..", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                FstrEndoRowID = endoChartService.GetRowIdbyPtNoJepDate(FstrPtno, FstrJepDate);

                fn_Screen_EndoChart_Display(FstrEndoRowID);
            }

            HIC_SANGDAM_NEW list3 = hicSangdamNewService.GetSangdamDrNoAmSangdambyWrtNo(FnWRTNO);

            strAmSangdam = "";

            if (!list3.IsNullOrEmpty())
            {
                strAmSangdam = list3.AMSANGDAM.Trim();
                txtPanDrNo.Text = list3.SANGDAMDRNO.ToString();
                lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));
            }

            if (strAmSangdam != "")
            {
                if (VB.Pstr(strAmSangdam, "{}", 1) != "") txtBodySymptom.Text = VB.Pstr(strAmSangdam, "{}", 1);
                if (VB.Pstr(strAmSangdam, "{}", 2) != "") txtCancerHis.Text = VB.Pstr(strAmSangdam, "{}", 2);
                if (VB.Pstr(strAmSangdam, "{}", 3) != "") txtStomachBowlLiver.Text = VB.Pstr(strAmSangdam, "{}", 3);
                if (VB.Pstr(strAmSangdam, "{}", 4) != "") txtChestCervical.Text = VB.Pstr(strAmSangdam, "{}", 4);
            }
        }

        void fn_Screen_EndoChart_Display(string argEndoRowId)
        {
            int nRead = 0;
            string strSuCode = "";
            string strASA = "";

            ENDO_CHART list = endoChartService.GetItembyRowId(argEndoRowId);

            if (!list.IsNullOrEmpty())
            {
                if (list.GB_CFS1.Trim() == "1") chkCFS0.Checked = true;
                if (list.GB_CFS2.Trim() == "1") chkCFS1.Checked = true;

                //파트1
                if (list.GB_DIET.Trim() == "1") rdoMealState1.Checked = true;

                //전신상태
                switch (list.GB_STS)
                {
                    case "1":
                        rdoBodyCondition0.Checked = true;
                        break;
                    case "2":
                        rdoBodyCondition1.Checked = true;
                        break;
                    case "3":
                        rdoBodyCondition2.Checked = true;
                        break;
                    case "4":
                        rdoBodyCondition3.Checked = true;
                        break;
                    default:
                        break;
                }

                //병력
                if (list.GB_OLD.Trim() == "1") chkMedHis0.Checked = false;
                if (list.GB_OLD1.Trim() == "1") chkMedHis1.Checked = false;
                if (list.GB_OLD2.Trim() == "1") chkMedHis2.Checked = false;
                if (list.GB_OLD3.Trim() == "1") chkMedHis3.Checked = false;
                if (list.GB_OLD4.Trim() == "1") chkMedHis4.Checked = false;
                if (list.GB_OLD5.Trim() == "1") chkMedHis5.Checked = false;
                if (list.GB_OLD6.Trim() == "1") chkMedHis6.Checked = false;
                if (list.GB_OLD7.Trim() == "1") chkMedHis7.Checked = false;
                if (list.GB_OLD8.Trim() == "1") chkMedHis8.Checked = false;
                if (list.GB_OLD9.Trim() == "1") chkMedHis9.Checked = false;
                if (list.GB_OLD10.Trim() == "1") chkMedHis10.Checked = false;
                if (list.GB_OLD11.Trim() == "1") chkMedHis11.Checked = false;
                if (list.GB_OLD12.Trim() == "1") chkMedHis12.Checked = false;
                if (list.GB_OLD13.Trim() == "1") chkMedHis13.Checked = false;
                if (list.GB_OLD14.Trim() == "1") chkMedHis14.Checked = false;
                txtMedHis13.Text = list.GB_OLD13_1.Trim();
                txtMedHisEtc.Text = list.GB_OLD15_1.Trim();

                //현재복용약물
                if (list.GB_DRUG.Trim() == "1")  chkMedcine0.Checked = true;
                if (list.GB_DRUG1.Trim() == "1") chkMedcine1.Checked = true;
                if (list.GB_DRUG2.Trim() == "1") chkMedcine2.Checked = true;
                if (list.GB_DRUG3.Trim() == "1") chkMedcine3.Checked = true;
                if (list.GB_DRUG4.Trim() == "1") chkMedcine4.Checked = true;
                if (list.GB_DRUG5.Trim() == "1") chkMedcine5.Checked = true;
                if (list.GB_DRUG6.Trim() == "1") chkMedcine6.Checked = true;
                if (list.GB_DRUG7.Trim() == "1") chkMedcine7.Checked = true;

                txtMedcineEtc.Text = list.GB_DRUG8_1.Trim();
                txtMedAspirin.Text = list.GB_DRUG_STOP1.Trim();
                txtAntiCoagulant.Text = list.GB_DRUG_STOP2.Trim();

                //전처치약제
                chkPreTreatment0.Checked = list.GB_B_DRUG.Trim() == "1" ? true : false;
                chkPreTreatment1.Checked = list.GB_B_DRUG1.Trim() == "1" ? true : false;
                txtJinjengUse.Text = list.GB_B_DRUG1_1.Trim();
                txtRemark.Text = list.GB_BIGO.Trim();
            }

            //향정주사 승인요청
            List<HIC_HYANG_APPROVE> list2 = hicHyangApproveService.GetSucodeDrSabunQtybyWrtNo(long.Parse(txtWrtNo.Text));

            nRead = list2.Count;
            SSHyang.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                strSuCode = list2[i].SUCODE.Trim();
                SSHyang.ActiveSheet.Cells[i, 0].Text = strSuCode;
                SSHyang.ActiveSheet.Cells[i, 1].Text = fn_Read_Drug_Jep_Name(strSuCode);
                SSHyang.ActiveSheet.Cells[i, 2].Text = fn_Read_Drug_Unit(strSuCode);
                SSHyang.ActiveSheet.Cells[i, 3].Text = list2[i].QTY.ToString();
            }

            cboASA.Text = "";
            if (SSHyang.ActiveSheet.RowCount > 0)
            {
                strASA = endoJupmstService.GetASAbyPtNoJepDate(FstrPtno, FstrJepDate);
                if (strASA != "")
                {
                    for (int i = 0; i < cboASA.Items.Count; i++)
                    {
                        if (VB.Pstr(cboASA.Items[i].ToString(), ".", 1) == strASA)
                        {
                            cboASA.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        string fn_Read_Drug_Jep_Name(string argSucode)
        {
            string rtnVal = "";
            string strTemp = "";

            strTemp = comHpcLibBService.GetJepNamebySuCode(argSucode);

            //한글만 표현 특수문자및 영어는 삭제처리
            for (int i = 0; i < strTemp.Length; i++)
            {
                if (VB.Asc(VB.Mid(strTemp, i, 1)) < 33 || VB.Asc(VB.Mid(strTemp, i, 1)) > 126)
                {
                    rtnVal += VB.Mid(strTemp, i, 1);
                }
            }
            return rtnVal;
        }

        string fn_Read_Drug_Unit(string argSuCode)
        {
            string rtnVal = "";
            string strUnit1 = "";
            string strUnit2 = "";
            string strUnit3 = "";
            string strUnit4 = "";
            string strUnit = "";

            //용량
            BAS_SUN list = basSunService.GetItembySuCode(argSuCode);

            strUnit = "";

            if (!list.IsNullOrEmpty())
            {
                strUnit1 = list.UNITNEW1.ToString();
                strUnit2 = list.UNITNEW2.ToString();
                strUnit3 = list.UNITNEW3.ToString();
                strUnit4 = list.UNITNEW4.ToString();
                strUnit = strUnit1 + strUnit2 + "/" + (long.Parse(strUnit4) > 0 ? strUnit4 + "㎖/ " : "" + strUnit3);
            }

            rtnVal = strUnit;

            return rtnVal;
        }

        void fn_Genjin_Histroy_SET()
        {
            int nREAD = 0;
            string strData = "";
            string strJong = "";
            long nHeaPano = 0;

            //종검의 등록번호를 찾음
            nHeaPano = 0;

            nHeaPano = long.Parse(hicPatientService.GetPanobyJumin2(clsAES.AES(FstrJumin)));

            //일반건진, 종합검진의 접수내역을 Display
            List<HIC_JEPSU> list = hicJepsuService.GetItembyUnionPaNo(FnPano);

            nREAD = list.Count;
            SSHistory.ActiveSheet.RowCount = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                strJong = list[i].GJJONG.Trim();

                SSHistory.ActiveSheet.Cells[i, 0].Text = list[i].JEPDATE.Trim();
                if (strJong == "XX")
                {
                    SSHistory.ActiveSheet.Cells[i, 1].Text = "종검";
                }
                else
                {
                    SSHistory.ActiveSheet.Cells[i, 1].Text = hb.READ_GjJong_Name(strJong);
                }
                SSHistory.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.ToString();
                SSHistory.ActiveSheet.Cells[i, 3].Text = list[i].GJCHASU.Trim();
            }
        }

        string fn_Read_Endo_Chart(string argPtno, string argJepDate)
        {
            string rtnVal = "";
            int result = 0;

            if (endoJupmstService.GetCountbyPtNoJDate(argPtno, argJepDate) > 0)
            {
                if (endoChartService.GetCountbyPtnoRDate(argPtno, argJepDate) > 0)
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    ENDO_CHART item = new ENDO_CHART();

                    item.PTNO = argPtno;
                    item.BDATE = Convert.ToDateTime(argJepDate);
                    item.RDATE = Convert.ToDateTime(argJepDate);
                    item.GUBUN = "3";

                    result = endoChartService.Insert(item);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        rtnVal = "내시경기록지 생성시 오류";
                        return rtnVal;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                }
            }

            return rtnVal;
        }

        /// <summary>
        /// 2차 상담 내역 DISPLAY
        /// </summary>
        /// <param name="argWrtNo"></param>
        void fn_Screen_SangDam_display2(long argWrtNo)
        {
            int nValue = 0;

            HIC_SANGDAM_NEW list = hicSangdamNewService.GetAllbyWrtNo(argWrtNo);

            //당뇨병(결과상태)
            if (list.DIABETES_1.Trim() != "")
            {
                nValue = int.Parse(list.DIABETES_1) - 1;
                RadioButton rdoDang = (Controls.Find("rdoDang" + nValue.ToString(), true)[0] as RadioButton);
                rdoDang.Checked = true;
            }

            //당뇨병(치료계획)
            if (list.DIABETES_2.Trim() != "")
            {
                nValue = int.Parse(list.DIABETES_2) - 1;
                RadioButton rdoDangJob = (Controls.Find("rdoDangJob" + nValue.ToString(), true)[0] as RadioButton);
                rdoDangJob.Checked = true;
            }

            //고혈압(결과상태)
            if (list.CYCLE_1.Trim() != "")
            {
                nValue = int.Parse(list.CYCLE_1) - 1;
                RadioButton rdoGohyul = (Controls.Find("rdoGohyul" + nValue.ToString(), true)[0] as RadioButton);
                rdoGohyul.Checked = true;
            }

            //고혈압(치료계획)
            if (list.CYCLE_2.Trim() != "")
            {
                nValue = int.Parse(list.CYCLE_2) - 1;
                RadioButton rdoGohyulJob = (Controls.Find("rdoGohyulJob" + nValue.ToString(), true)[0] as RadioButton);
                rdoGohyulJob.Checked = true;
            }

            //식사여부
            if (list.GBSIKSA.Trim() == "Y")
            {
                rdoMealState0.Checked = true;
            }
            else
            {
                rdoMealState1.Checked = true;
            }

            txtRemark.Text = list.REMARK.Trim();    //상담내역
            txtPanDrNo.Text = list.SANGDAMDRNO.ToString();
        }

        /// <summary>
        /// 표적장기별 상담 Display
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="argPjSangdam"></param>
        void fn_PJ_Display(long argWRTNO, string argPjSangdam)
        {
            int j = 0;
            long nCNT = 0;
            int nRow = 0;
            int nREAD = 0;
            string strData = "";
            int[] nGbSangdam = new int[20];
            string[] strPjSangdam = new string[20];
            string strName = "";

            for (int i = 0; i < 20; i++)
            {
                nGbSangdam[i] = 0;
                strPjSangdam[i] = "";
            }

            if (argPjSangdam != "")
            {
                nCNT = VB.L(argPjSangdam, "{$}") - 1;
                for (int i = 0; i < nCNT; i++)
                {
                    strData = VB.Pstr(argPjSangdam, "{$}", i);
                    j = int.Parse(VB.Pstr(argPjSangdam, "{$}", i));
                    strPjSangdam[j] = VB.Pstr(strData, "{}", 2);
                }
            }

            SS9.ActiveSheet.RowCount = 0;

            //상담해야할 표적장기 찾기
            List<HIC_SUNAPDTL_GROUPCODE> list = hicSunapdtlGroupcodeService.GetCodeGbSangdambyWrtNo(argWRTNO);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strData = list[i].GBSANGDAM.Trim();
                for (int k = 0; k < strData.Length; k++)
                {
                    if (VB.Mid(strData, k, 1) == "1")
                    {
                        nGbSangdam[k] = 1;
                    }
                }
            }

            //표적장기명칭 및 상담결과를 표시
            nRow = 0;
            for (int i = 0; i < 20; i++)
            {
                if (nGbSangdam[i] == 1)
                {
                    strName = hicBcodeService.GetCodeNamebyGubunCode("HIC_표적장기별특수상담", string.Format("{0:00}", i));

                    nRow += 1;
                    SS9.ActiveSheet.RowCount = nRow;
                    SS9.ActiveSheet.Cells[nRow - 1, 0].Text = strName.Trim();
                    if (strPjSangdam[i] != "")
                    {
                        SS9.ActiveSheet.Cells[nRow - 1, 1].Text = strPjSangdam[i];
                    }
                    else
                    {
                        SS9.ActiveSheet.Cells[nRow - 1, 1].Text = "특이소견 없음";
                    }
                    SS9.ActiveSheet.Cells[nRow - 1, 1].Text = string.Format("{0:00}", i + 1);
                }
            }
        }

        /// <summary>
        /// Call 생활습관도구표_READ(FnWRTNO)
        /// </summary>
        /// <param name="argWrtNo"></param>
        void fn_LivingHabitReport(long argWrtNo)
        {
            int nREAD = 0;
            string strDAT = "";
            long[] nJumsu = new long[19];
            long nWRTNO = 0;
            double nBiman1 = 0;
            double nBiman2 = 0;
            string strResult = "";
            string strBiman = "";

            string strPHQ ="";
            string strOK = "";

            int result = 0;

            strOK = "";
            nWRTNO = argWrtNo;
            FstrHabit[10] = "N";
            FstrHabit[11] = "N";
            FstrHabit[12] = "N";
            FstrHabit[13] = "N";
            FstrHabit[14] = "N";
            FstrHabit[15] = "N";
            FstrHabit[16] = "N";
            FstrHabit[17] = "N";

            //접수내역을 보고 항목을 세팅
            List<HIC_RESULT> list = hicResultService.GetExCodeResultbyOnlyWrtNo(nWRTNO);

            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i].EXCODE.Trim())
                {
                    case "A143":
                        FstrHabit[10] = "Y";    //흡연
                        break;
                    case "A144":
                        FstrHabit[11] = "Y";    //음주
                        break;
                    case "A145":
                        FstrHabit[12] = "Y";    //운동
                        break;
                    case "A146":
                        FstrHabit[13] = "Y";    //영양
                        break;
                    case "A147":
                        FstrHabit[14] = "Y";    //비만
                        break;
                    case "A127":
                    case "A128":
                        FstrHabit[15] = "Y";    //우울증
                        break;
                    case "A129":
                        FstrHabit[16] = "Y";    //인지
                        break;
                    case "A118":
                    case "A119":
                    case "A120":
                    case "A130":
                        FstrHabit[17] = "Y";    //노인신체기능
                        break;
                    default:
                        break;
                }
            }

            if (FstrHabit[10] == "N" && FstrHabit[11] == "N" && FstrHabit[12] == "N" && FstrHabit[13] == "N" && FstrHabit[14] == "N" && FstrHabit[15] == "N" && FstrHabit[16] == "N" && FstrHabit[17] == "N")
            {
                FstrTFlag = "";
                return;
            }

            FstrTFlag = "Y";

            //점수표 계산 및 DB업데이트
            for (int i = 10; i <= 16; i++)
            {
                nJumsu[i] = 0;
                if (i >= 15)
                {
                    i += 2;
                    nJumsu[i] = hicTitemService.GetTotalbyGubunWrtNo(i, nWRTNO);
                    if (i == 17)
                    {
                        i -= 2;
                    }
                }
            }

            //우울증평가
            if (hicTitemService.GetCountbyWrtNo(nWRTNO) > 0)
            {
                strPHQ = "1";
            }

            //검사결과를 읽어 공란이면 점수 UPDATE .
            List<HIC_RESULT> list2 = hicResultService.GetExCodeResultbyOnlyWrtNo(nWRTNO);

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < list2.Count; i++)
            {
                strResult = list2[i].RESULT.Trim();
                switch (list2[i].EXCODE.Trim())
                {
                    case "A143":    //흡연
                        result = hicResultService.UpdateResultbyWrtNoExCode(nJumsu[10].ToString(), nWRTNO, long.Parse(clsType.User.IdNumber), list2[i].EXCODE.Trim());
                        break;
                    case "A144":    //음주
                        result = hicResultService.UpdateResultbyWrtNoExCode(nJumsu[11].ToString(), nWRTNO, long.Parse(clsType.User.IdNumber), list2[i].EXCODE.Trim());
                        break;
                    case "A145":    //운동
                        result = hicResultService.UpdateResultbyWrtNoExCode("0", nWRTNO, long.Parse(clsType.User.IdNumber), list2[i].EXCODE.Trim());
                        break;
                    case "A146":    //영양
                        result = hicResultService.UpdateResultbyWrtNoExCode(nJumsu[13].ToString(), nWRTNO, long.Parse(clsType.User.IdNumber), list2[i].EXCODE.Trim());
                        break;
                    case "A130":    //우울증
                        result = hicResultService.UpdateResultbyWrtNoExCode(nJumsu[17].ToString(), nWRTNO, long.Parse(clsType.User.IdNumber), list2[i].EXCODE.Trim());
                        break;
                    case "A129":    //인지기능
                        result = hicResultService.UpdateResultbyWrtNoExCode(nJumsu[18].ToString(), nWRTNO, long.Parse(clsType.User.IdNumber), list2[i].EXCODE.Trim());
                        break;
                    default:
                        break;
                }

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("검사결과 저장중 오류발생!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);

            ///TODO : HcMain_Exam.bas 파일(김민철) 컨버전 후 주석해제
            ///nJumsu[12] = hm.Read_Auto_WORK(nWRTNO);

            nBiman1 = 0;
            nBiman2 = 0;

            List<string> sExCode = new List<string>();

            sExCode.Clear();
            sExCode.Add("A115");
            sExCode.Add("A117");

            List<HIC_RESULT> list3 = hicResultService.GetResultbyWrtNoExCodeList(nWRTNO, sExCode);

            nBiman1 = double.Parse(list3[0].RESULT);
            nBiman2 = double.Parse(list3[1].RESULT);

            //비만도체크
            if (nBiman1 < 18.5)
            {
                strBiman = "저체중";
            }
            else if (nBiman1 >= 18.5 && nBiman1 <= 24.9)
            {
                strBiman = "정상체중";
            }
            else if (nBiman1 >= 25 && nBiman1 <= 29.9)
            {
                strBiman = "비만";
            }
            else if (nBiman1 >= 30)
            {
                strBiman = "고도비만";
            }

            //청구관련 비만처방 검사코드에 결과입력
            clsDB.setBeginTran(clsDB.DbCon);

            result = hicResultService.UpdateBimanbyWrtNoExCode(strBiman, clsType.User.IdNumber, nWRTNO, "A147");

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("비만처방 검사코드에 결과입력시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            //접수마스타의 상태를 변경
            fn_Result_EntryEnd_Check(long.Parse(txtWrtNo.Text));
        }

        /// <summary>
        /// 접수마스타의 상태를 변경
        /// </summary>
        /// <param name="nWrtNo"></param>
        void fn_Result_EntryEnd_Check(long nWrtNo)
        {

        }

        /// <summary>
        /// 상담테이블 생성함
        /// </summary>
        /// <param name="argWrtNo"></param>
        /// <param name="argJong"></param>
        /// <returns></returns>
        string fn_HIC_NEW_SANGDAM_INSERT(long argWrtNo, string argJong)
        {
            string rtnVal = "";
            string strRowId = "";
            int result = 0;

            strRowId = hicSangdamNewService.GetRowIdbyWrtNo(FnWRTNO).Trim();

            if (strRowId == "")
            {
                clsDB.setBeginTran(clsDB.DbCon);

                if (fn_HIC_ExJong_CHECK2(argJong) == "Y")
                {
                    HIC_SANGDAM_NEW item = new HIC_SANGDAM_NEW();

                    item.WRTNO = argWrtNo;
                    item.GJJONG = argJong;
                    item.GJCHASU = FstrChasu;
                    item.JEPDATE = Convert.ToDateTime(FstrJepDate);
                    item.PANO = FnPano;
                    item.PTNO = FstrPtno;
                    item.GBSTS = "";

                    result = hicSangdamNewService.Insert(item);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        rtnVal = "암문진 생성시 오류발생!!!";
                        return rtnVal;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
            }

            return rtnVal;
        }

        /// <summary>
        /// 상담여부 체크
        /// </summary>
        /// <param name="argJong"></param>
        /// <returns></returns>
        string fn_HIC_ExJong_CHECK2(string argJong)
        {
            string rtnVal = "";

            rtnVal = hicExjongService.GetGbSangdambyCode(argJong).Trim();

            return rtnVal;
        }

        //폐암 사후상담시 판정데이터 불러오기위한 부분
        string fn_READ_LUNG_PAN(long argPano)
        {
            string rtnVal = "";
            string strYear = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            strYear = VB.Left(clsPublic.GstrSysDate, 4);

            HIC_JEPSU_CANCER_NEW list = hicJepsuCancerNewService.GetItembyPaNoGjYear(argPano, strYear);

            if (!list.IsNullOrEmpty())
            {
                rtnVal = "●판정구분에 의한 권고사항" + "\r\n" + list.LUNG_RESULT070 + "\r\n" + "●폐결절 외 기타 권고사항" + "\r\n" + list.LUNG_RESULT071;
            }
            else
            {
                rtnVal = "";
            }

            return rtnVal;
        }

        void fn_READ_First_Wrtno(long argWrtNo, string argJong)
        {
            string strYear = "";

            ComFunc.ReadSysDate(clsDB.DbCon);
            strYear = VB.Left(clsPublic.GstrSysDate, 4);

            FnWrtno2 = hicJepsuService.GetWrtNobyWrtNoJepdateGjYear(argWrtNo, FstrJepDate, FstrYear, argJong);
        }

        void fn_Update_Patient_GbCall()
        {
            int result = 0;
            string strSysDate = "";
            List<string> strWRTNO = new List<string>();

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsDB.setBeginTran(clsDB.DbCon);

            strWRTNO.Clear();

            strSysDate = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;

            //호출은 했으나 상담이 완료안된 접수번호 찾음
            List<HIC_SANGDAM_WAIT> list = hicSangdamWaitService.GetWrtNobyGubunNotWrtNo(clsHcVariable.GstrDrRoom, long.Parse(txtWrtNo.Text));

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strWRTNO.Add(list[i].WRTNO.ToString());
                }

                if (!strWRTNO.IsNullOrEmpty())
                {
                    result = hicSangdamWaitService.UpdateCallTimeDisplaybyWrtNo(strWRTNO, "NOT");

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("호출은 했으나 상담이 미완료 환자 호출 취소중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            result = hicSangdamWaitService.UpdateCallTimebyWrtNoGubun(long.Parse(txtWrtNo.Text), clsHcVariable.GstrDrRoom, strSysDate);

            if (result < 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("환자 호출Flag 적용중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 당일수검 종류와 상태를 체크
        /// </summary>
        void fn_Read_Jepsu_Display()
        {
            string strGjJong = "";
            string strWRTNO = "";
            int nRead = 0;
            int nCol = 0;

            List<HIC_JEPSU_SANGDAM_NEW> list = hicJepsuSangdamNewService.GetItembyPaNoJepDate(FnPano, FstrJepDate);

            nRead = list.Count;
            for (int i = 0; i < nRead; i++)
            {
                strGjJong = list[i].GJJONG.Trim();
                strWRTNO = list[i].WRTNO.ToString();

                switch (strGjJong)
                {
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                    case "25":
                    case "26":
                    case "27":
                    case "28":
                    case "29":
                    case "30":
                        SSJong.ActiveSheet.Cells[0, 1].Text = "○";
                        nCol = 1;
                        break;
                    case "33":
                    case "49":
                        SSJong.ActiveSheet.Cells[0, 1].Text = "○";
                        nCol = 1;
                        break;
                    case "56":
                    case "59":
                        SSJong.ActiveSheet.Cells[0, 2].Text = "○";
                        nCol = 2;
                        break;
                    case "50":
                    case "51":
                        SSJong.ActiveSheet.Cells[0, 3].Text = "○";
                        nCol = 3;
                        break;
                    case "31":
                    case "35":
                        SSJong.ActiveSheet.Cells[0, 4].Text = "○";
                        nCol = 4;
                        break;
                    default:
                        SSJong.ActiveSheet.Cells[0, 0].Text = "○";
                        nCol = 0;
                        break;
                }

                if (list[i].GBSTS.Trim() == "Y")
                {
                    SSJong.ActiveSheet.Cells[0, nCol].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HDFDFFF"));
                }
                else
                {
                    SSJong.ActiveSheet.Cells[0, nCol].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HBAFEFC"));
                }

                //일특체크
                if (strGjJong == "11" || strGjJong == "12" || strGjJong == "16" || strGjJong == "17" || strGjJong == "19" ||
                    strGjJong == "41" || strGjJong == "42" || strGjJong == "44" || strGjJong == "45")
                {
                    if (FstrUCodes != "")
                    {
                        SSJong.ActiveSheet.Cells[0, 1].Text = "○";
                        if (list[i].GBSTS.Trim() == "Y")
                        {
                            SSJong.ActiveSheet.Cells[0, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HDFDFFF"));
                        }
                        else
                        {
                            SSJong.ActiveSheet.Cells[0, 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HBAFEFC"));
                        }
                    }
                }

                switch (strGjJong)
                {
                    case "21":
                    case "22":
                    case "23":
                    case "24":
                    case "25":
                    case "26":
                    case "27":
                    case "28":
                    case "29":
                    case "30":
                        nCol = 1;
                        break;
                    case "33":
                    case "49":
                        nCol = 1;
                        break;
                    case "56":
                    case "59":
                        nCol = 2;
                        break;
                    case "50":
                    case "51":
                        nCol = 3;
                        break;
                    case "31":
                    case "35":
                        nCol = 4;
                        break;
                    default:
                        nCol = 0;
                        break;
                }
                SSJong.ActiveSheet.Cells[1, nCol].Text = strWRTNO;

                //일특체크
                if (strGjJong == "11" || strGjJong == "12" || strGjJong == "16" || strGjJong == "17" || strGjJong == "19" ||
                    strGjJong == "41" || strGjJong == "42" || strGjJong == "44" || strGjJong == "45")
                {
                    if (FstrUCodes != "")
                    {
                        SSJong.ActiveSheet.Cells[1, 1].Text = strWRTNO;
                    }
                }
            }
        }

        void fn_HIC_HYANG_Approve(long argWrtNo)
        {
            int nREAD = 0;
            string strPtNo = "";
            string strBDATE = "";
            string strSuCode = "";
            string strORDERCODE = "";
            string strDeldate = "";
            string strROWID = "";
            long nQty = 0;
            long nOldQty = 0;
            string strSname = "";
            string strJuso = "";
            string strList = "";
            long nSeqNo = 0;
            string strGbSite = "";
            string strSex = "";
            long nAge = 0;
            string strDosCode = "";
            string strDrCode = "";
            string strSlipNo = "";
            double nOrderno = 0;
            int result = 0;

            string strGubun = "";
            string strAppTime = "";
            string strSysdate = "";
            string strRID = "";

            //승인한것 수가코드 목록을 만듬
            strList = " ";
            for (int i = 0; i < SSHyang.ActiveSheet.RowCount; i++)
            {
                strSuCode = SSHyang.ActiveSheet.Cells[i, 0].Text.Trim();
                strList += strSuCode + ",";
            }

            clsDB.setBeginTran(clsDB.DbCon);

            //HIC_HYANG_Approve에 삭제된것 등록
            List<HIC_HYANG_APPROVE> list = hicHyangApproveService.GetItembyWrtNoDeptCode(argWrtNo, "HR");

            for (int i = 0; i < list.Count; i++)
            {
                strSuCode = list[i].SUCODE.Trim();
                //승인한 수가코드 목록에 없으면 삭제함
                if (VB.InStr(strList, "," + strSuCode + ",") == 0)
                {
                    strBDATE = list[i].BDATE.ToString();
                    strPtNo = list[i].PTNO.Trim();

                    result = hicHyangApproveService.UpdateDelDatebyRowId(list[i].ROWID.Trim());

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    result = hicHyangService.UpdateDelDatebyWrtNo(argWrtNo, strSuCode);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    if (VB.Left(strSuCode, 2) == "N-")
                    {
                        result = comHpcLibBService.DeleteOcsMayak(strPtNo, strSuCode, strBDATE);
                    }
                    else
                    {
                        result = comHpcLibBService.DeleteOcsHyang(strPtNo, strSuCode, strBDATE);
                    }

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    //OCS_OORDER 전송
                    result = comHpcLibBService.DeleteOcsOorder(strPtNo, strSuCode, strBDATE);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    HIC_JEPSU list3 = hicJepsuService.GetJuso1Juso2byWrtNo(argWrtNo);

                    strJuso = list3.JUSO1 + list3.JUSO2;

                    //승인한것 업데이트
                    for (int k = 0; k < SSHyang.ActiveSheet.RowCount; k++)
                    {
                        strSuCode = SSHyang.ActiveSheet.Cells[i, 0].Text.Trim();
                        nQty = long.Parse(SSHyang.ActiveSheet.Cells[i, 3].Text.Trim());

                        if (strSuCode != "")
                        {
                            HIC_HYANG_APPROVE list4 = hicHyangApproveService.GetItembyWrtNoSucode(argWrtNo, strSuCode);

                            strROWID = list4.ROWID.Trim();
                            strBDATE = list4.BDATE.ToString();
                            strPtNo = list4.PTNO.Trim();
                            strSname = list4.SNAME.Trim();
                            strGbSite = list4.GBSITE.Trim();
                            strSex = list4.SEX.Trim();
                            nOldQty = list4.QTY;
                            nAge = list4.AGE;

                            //승인한 약품의 수량이 변경되었으면 History를 저장함
                            if (nQty != nOldQty && list4.APPROVETIME.ToString() != "")
                            {   
                                result = hicHyangApproveService.InsertSelectbyRowId(strROWID);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("승인의뢰용 처방을 등록 중 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                            if (nQty != nOldQty)
                            {
                                strGubun = "1";
                            }
                            else if (list4.APPROVETIME.ToString() == "")
                            {
                                strAppTime = "";
                            }

                            result = hicHyangApproveService.UpdateAppTimebyRowId(strGubun, strAppTime, nQty, clsType.User.IdNumber, strROWID);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("승인의뢰용 처방을 등록 중 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            //SEQNO를 읽음
                            strSysdate = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";
                            nSeqNo = hicHyangService.GetSeqNobyBDate(strSysdate);

                            //HIC_HYANG Update
                            strROWID = hicHyangService.GetRowIdbyWrtNoSuCode(argWrtNo, strSuCode);

                            if (strROWID.IsNullOrEmpty())
                            {
                                result = hicHyangService.UpdateQtybyRowId(nQty, strROWID);
                            }
                            else
                            {
                                HIC_HYANG item = new HIC_HYANG();

                                item.IO = "O";
                                item.NAL = 1;
                                item.DOSCODE = "920103";
                                item.REMARK1 = "검사용";
                                item.REMARK2 = "Pain";
                                item.SEQNO = nSeqNo;
                                item.ORDERNO = 0;
                                item.JUSO = strJuso;
                                item.ROWID = strROWID;

                                result = hicHyangService.InsertSelectbyWorId(item);
                            }

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("향정약품 등록 중 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            //외래접수
                            if (comHpcLibBService.GetCountbyPaNoBDate(strPtNo, strBDATE) == 0)
                            {
                                COMHPC item = new COMHPC();

                                item.PANO = long.Parse(strPtNo);
                                item.DEPTCODE = "HR";
                                item.BI = "51";
                                item.SNAME = strSname;
                                item.SEX = strSex;
                                item.AGE = nAge;
                                item.DRCODE = "7101";
                                item.RESERVED = "0";
                                item.CHOJAE = "1";
                                item.GBGAMEK = "00";
                                item.GBSPC = "0";
                                item.JIN = "D";
                                item.SINGU = "0";
                                item.PART = "111";
                                item.BDATE = strBDATE;
                                item.EMR = "0";
                                item.GBUSE = "Y";
                                item.MKSJIN = "D";

                                result = comHpcLibBService.InsertOpdMaster(item);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("외래접수 등록 중 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                //OCS_OORDER 전송
                                COMHPC list5 = comHpcLibBService.GetRowIdQtybyOcsOrder(strPtNo, strBDATE, strSuCode);

                                if (list5.IsNullOrEmpty())
                                {
                                    strDosCode = "920103";
                                    //HD실 요청으로 용법코드를 920103을 사용함
                                    strDrCode = hb.READ_HIC_OcsDrcode(long.Parse(clsType.User.IdNumber));
                                    //오더코드,SlipNo 설정
                                    switch (strSuCode.Trim())
                                    {
                                        case "A-PO12GA":
                                            strORDERCODE = "A-PO12GA";
                                            strSlipNo = "0005";
                                            break;
                                        case "A-BASCAM":
                                            strORDERCODE = "A-BASCAM";
                                            strSlipNo = "0005";
                                            break;
                                        case "N-PTD25":
                                            strORDERCODE = "N-PTD25";
                                            strSlipNo = "0005";
                                            break;
                                        case "A-POL8G":
                                            strORDERCODE = "A-POL8G";
                                            strSlipNo = "0044";
                                            break;
                                        case "A-POL2":
                                            strORDERCODE = "A-POL2";
                                            strSlipNo = "0044";
                                            break;
                                        default:
                                            strORDERCODE = strSuCode;
                                            strSlipNo = "0044";
                                            break;
                                    }

                                    COMHPC item2 = new COMHPC();

                                    item2.PTNO = strPtNo;
                                    item2.BDATE = strBDATE;
                                    item2.DEPTCODE = "HR";
                                    item2.SEQNO = 99;
                                    item2.ORDERCODE = strORDERCODE;
                                    item2.SUCODE = strSuCode;
                                    item2.BUN = "20";
                                    item2.SLIPNO = strSlipNo;
                                    item2.REALQTY = nQty;
                                    item2.QTY = nQty;
                                    item2.NAL = 1;
                                    item2.GBDIV = "1";
                                    item2.DOSCODE = strDosCode;
                                    item2.GBBOTH = "0";
                                    item2.GBINFO = "";
                                    item2.GBER = "";
                                    item2.GBSELF = "";
                                    item2.GBSPC = "";
                                    item2.BI = "51";
                                    item2.DRCODE = strDrCode;
                                    item2.REMARK = "검사용";
                                    item2.GBSUNAP = "1";
                                    item2.TUYAKNO = 0;
                                    item2.MULTI = "";
                                    item2.MULTIREMARK = "";
                                    item2.DUR = "";
                                    item2.RESV = "";
                                    item2.SCODESAYU = "";
                                    item2.SCODEREMARK = "";
                                    item2.GBSEND = "Y";
                                    item2.SABUN = clsType.User.IdNumber;
                                    item2.CORDERCODE = strORDERCODE;
                                    item2.CSUCODE = strDosCode;
                                    item2.CBUN = "200";
                                    item2.IP = clsPublic.GstrIpAddress;

                                    result = comHpcLibBService.InsertOcsOrder(item2);

                                    if (result < 0)
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        return;
                                    }
                                }
                                else if (list5.QTY != nQty)
                                {
                                    result = comHpcLibBService.UpdateOcsOrder(nQty, list5.ROWID.Trim());
                                }

                                //OCS 오더번호를 찾음
                                nOrderno = comHpcLibBService.GetOrderNoOcsOrderbyPtno(strPtNo, strBDATE, strSuCode);

                                //OCS_HYANG Update
                                //본관내시경실만 전송함
                                if (strGbSite == "2")
                                {
                                    if (VB.Left(strSuCode, 2) == "N-")
                                    {
                                        COMHPC list6 = comHpcLibBService.GetRowIdOcsMayakbyPtNo(strPtNo, strBDATE, strSuCode);

                                        COMHPC item3 = new COMHPC();

                                        item3.BI = "51";
                                        item3.WARDCODE = "EN";
                                        item3.IO = "O";
                                        item3.QTY = nQty;
                                        item3.REALQTY = nQty;
                                        item3.NAL = 1;
                                        item3.DOSCODE = "920103";
                                        item3.ORDERNO = nOrderno;
                                        item3.REMARK1 = "검사용";
                                        item3.REMARK2 = "Pain";
                                        item3.JUSO = strJuso;
                                        item3.DRSABUN = long.Parse(clsType.User.IdNumber);
                                        item3.CERTNO = "";
                                        item3.ROWID = strROWID;

                                        if (list6.IsNullOrEmpty())
                                        {
                                            result = comHpcLibBService.InsertSelectOcsMayak(item3);
                                        }
                                        else
                                        {
                                            result = comHpcLibBService.UpdateOcsMayak(item3);
                                        }

                                        if (result < 0)
                                        {
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            MessageBox.Show("마약 저장 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    COMHPC list6 = comHpcLibBService.GetRowIdOcsHyangbyPtNo(strPtNo, strBDATE, strSuCode);

                                    COMHPC item4 = new COMHPC();

                                    item4.BI = "51";
                                    item4.WARDCODE = "EN";
                                    item4.IO = "O";
                                    item4.QTY = nQty;
                                    item4.REALQTY = nQty;
                                    item4.NAL = 1;
                                    item4.DOSCODE = "920103";
                                    item4.ORDERNO = nOrderno;
                                    item4.REMARK1 = "검사용";
                                    item4.REMARK2 = "Pain";
                                    item4.JUSO = strJuso;
                                    item4.DRSABUN = long.Parse(clsType.User.IdNumber);
                                    item4.CERTNO = "";
                                    item4.ROWID = strROWID;

                                    if (list6.IsNullOrEmpty())
                                    {
                                        result = comHpcLibBService.InsertSelectOcsHyang(item4);
                                    }
                                    else if (nQty != nOldQty)
                                    {
                                        result = comHpcLibBService.UpdateOcsHyang(item4);
                                    }

                                    if (result < 0)
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        MessageBox.Show("향정 저장 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                    clsDB.setCommitTran(clsDB.DbCon);
                }
            }
        }

        /// <summary>
        /// EMR 데이터 등록
        /// </summary>
        /// <param name="argWrtNo"></param>
        /// <param name="argEGD1"></param>
        /// <param name="argEGD2"></param>
        void fn_ENDO_EMR_INSERT(long argWrtNo, string argEGD1, string argEGD2)
        {
            string strJONGGUM = "";
            string strHEAENDO = "";
            string strPtNo = "";
            string strPano = "";
            string strDrCode = "";
            string strJepDate = "";
            string strOK = "";
            string strDeptCode  = "";
            string strOK1 = "";
            string strFrDate = "";
            string strToDate = "";
            int result = 0;

            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(argWrtNo);

            if (!list.IsNullOrEmpty())
            {
                strJONGGUM = list.JONGGUMYN.Trim();
                strHEAENDO = list.GBHEAENDO.Trim();
                strPtNo = list.PTNO.Trim();
                strPano = list.PANO.ToString();
                strJepDate = list.JEPDATE;
                strDrCode = hb.READ_BILL_DRCODE(clsType.User.IdNumber);
                strDeptCode = "HR";

                if (argEGD1 == "1")
                {
                    if (heaResvExamService.GetCountbyPanoSDate(strPano, strJepDate) > 0)
                    {
                        strDeptCode = "TO";
                        strOK = "";
                    }
                }

                if (comHpcLibBService.GetCountOrderbyPtnoJepDate(strPtNo, strJepDate) > 0)
                {
                    strOK = "";
                }

                //시행부서 체크
                strFrDate = strJepDate;
                strToDate = DateTime.Parse(strJepDate).AddDays(1).ToShortDateString();
                if (endoJupmstService.GetCountbyPtNoRDate(strFrDate, strToDate, strPtNo) > 0)
                {
                    strHEAENDO = "Y";
                }

                if (strJONGGUM == "0" && strOK == "OK")
                {
                    if (argEGD1 == "1" && argEGD2 == "1")
                    {
                        COMHPC item = new COMHPC();

                        item.PTNO = strPtNo;
                        item.BDATE = clsPublic.GstrSysDate;
                        item.DEPTCODE = "HR";
                        item.SEQNO = 99;
                        item.ORDERCODE = "00440120";
                        item.SUCODE = "E7630S";
                        item.BUN = "20";
                        item.SLIPNO = "0044";
                        item.REALQTY = 1;
                        item.QTY = 1;
                        item.NAL = 1;
                        item.GBDIV = "1";
                        item.DOSCODE = "";
                        item.GBBOTH = "0";
                        item.GBINFO = "";
                        item.GBER = "";
                        item.GBSELF = "";
                        item.GBSPC = "";
                        item.BI = "51";
                        item.DRCODE = strDrCode;
                        item.REMARK = "검사용";
                        item.GBSUNAP = "1";
                        item.TUYAKNO = 0;
                        item.MULTI = "";
                        item.MULTIREMARK = "";
                        item.DUR = "";
                        item.RESV = "";
                        item.SCODESAYU = "";
                        item.SCODEREMARK = "";
                        item.GBSEND = "Y";
                        item.SABUN = clsType.User.IdNumber;
                        item.CORDERCODE = "00440120";
                        item.CSUCODE = "E7630S";
                        item.CBUN = "200";
                        item.IP = clsPublic.GstrIpAddress;

                        result = comHpcLibBService.InsertOcsOrder(item);

                        //내시경장소 체크
                        if (strHEAENDO == "Y")
                        {
                            //NSB
                            COMHPC item2 = new COMHPC();

                            item2.PTNO = strPtNo;
                            item2.BDATE = clsPublic.GstrSysDate;
                            item2.DEPTCODE = "HR";
                            item2.SEQNO = 99;
                            item2.ORDERCODE = "00440430";
                            item2.SUCODE = "NSB";
                            item2.BUN = "20";
                            item2.SLIPNO = "0044";
                            item2.REALQTY = 1;
                            item2.QTY = 1;
                            item2.NAL = 1;
                            item2.GBDIV = "1";
                            item2.DOSCODE = "920103";
                            item2.GBBOTH = "0";
                            item2.GBINFO = "";
                            item2.GBER = "";
                            item2.GBSELF = "";
                            item2.GBSPC = "";
                            item2.BI = "51";
                            item2.DRCODE = strDrCode;
                            item2.REMARK = "검사용";
                            item2.GBSUNAP = "1";
                            item2.TUYAKNO = 0;
                            item2.MULTI = "";
                            item2.MULTIREMARK = "";
                            item2.DUR = "";
                            item2.RESV = "";
                            item2.SCODESAYU = "";
                            item2.SCODEREMARK = "";
                            item2.GBSEND = "Y";
                            item2.SABUN = clsType.User.IdNumber;
                            item2.CORDERCODE = "00440430";
                            item2.CSUCODE = "NSB";
                            item2.CBUN = "200";
                            item2.IP = clsPublic.GstrIpAddress;

                            result = comHpcLibBService.InsertOcsOrder(item2);
                        }
                        else
                        {
                            //5DWA-S
                            COMHPC item3 = new COMHPC();

                            item3.PTNO = strPtNo;
                            item3.BDATE = clsPublic.GstrSysDate;
                            item3.DEPTCODE = "HR";
                            item3.SEQNO = 99;
                            item3.ORDERCODE = "00400311";
                            item3.SUCODE = "5DWA-S";
                            item3.BUN = "20";
                            item3.SLIPNO = "0044";
                            item3.REALQTY = 1;
                            item3.QTY = 1;
                            item3.NAL = 1;
                            item3.GBDIV = "1";
                            item3.DOSCODE = "920103";
                            item3.GBBOTH = "0";
                            item3.GBINFO = "";
                            item3.GBER = "";
                            item3.GBSELF = "";
                            item3.GBSPC = "";
                            item3.BI = "51";
                            item3.DRCODE = strDrCode;
                            item3.REMARK = "검사용";
                            item3.GBSUNAP = "1";
                            item3.TUYAKNO = 0;
                            item3.MULTI = "";
                            item3.MULTIREMARK = "";
                            item3.DUR = "";
                            item3.RESV = "";
                            item3.SCODESAYU = "";
                            item3.SCODEREMARK = "";
                            item3.GBSEND = "Y";
                            item3.SABUN = clsType.User.IdNumber;
                            item3.CORDERCODE = "00440430";
                            item3.CSUCODE = "5DWA";
                            item3.CBUN = "200";
                            item3.IP = clsPublic.GstrIpAddress;

                            result = comHpcLibBService.InsertOcsOrder(item3);
                        }
                    }
                    else if (argEGD1 == "1" && argEGD2 == "")
                    {
                        COMHPC item4 = new COMHPC();

                        item4.PTNO = strPtNo;
                        item4.BDATE = clsPublic.GstrSysDate;
                        item4.DEPTCODE = "HR";
                        item4.SEQNO = 99;
                        item4.ORDERCODE = "00440110";
                        item4.SUCODE = "E7630";
                        item4.BUN = "20";
                        item4.SLIPNO = "0044";
                        item4.REALQTY = 1;
                        item4.QTY = 1;
                        item4.NAL = 1;
                        item4.GBDIV = "1";
                        item4.DOSCODE = "";
                        item4.GBBOTH = "0";
                        item4.GBINFO = "";
                        item4.GBER = "";
                        item4.GBSELF = "";
                        item4.GBSPC = "";
                        item4.BI = "51";
                        item4.DRCODE = strDrCode;
                        item4.REMARK = "검사용";
                        item4.GBSUNAP = "1";
                        item4.TUYAKNO = 0;
                        item4.MULTI = "";
                        item4.MULTIREMARK = "";
                        item4.DUR = "";
                        item4.RESV = "";
                        item4.SCODESAYU = "";
                        item4.SCODEREMARK = "";
                        item4.GBSEND = "Y";
                        item4.SABUN = clsType.User.IdNumber;
                        item4.CORDERCODE = "00440110";
                        item4.CSUCODE = "E7630";
                        item4.CBUN = "200";
                        item4.IP = clsPublic.GstrIpAddress;

                        result = comHpcLibBService.InsertOcsOrder(item4);
                    }
                }
                else if (strJONGGUM == "1" && strOK == "OK" && strOK1 == "")
                {
                    if (argEGD1 == "1" && argEGD2 == "1")
                    {
                        COMHPC item5 = new COMHPC();

                        item5.PTNO = strPtNo;
                        item5.BDATE = clsPublic.GstrSysDate;
                        item5.DEPTCODE = "HR";
                        item5.SEQNO = 99;
                        item5.ORDERCODE = "00440120";
                        item5.SUCODE = "E7630S";
                        item5.BUN = "20";
                        item5.SLIPNO = "0044";
                        item5.REALQTY = 1;
                        item5.QTY = 1;
                        item5.NAL = 1;
                        item5.GBDIV = "1";
                        item5.DOSCODE = "";
                        item5.GBBOTH = "0";
                        item5.GBINFO = "";
                        item5.GBER = "";
                        item5.GBSELF = "";
                        item5.GBSPC = "";
                        item5.BI = "51";
                        item5.DRCODE = strDrCode;
                        item5.REMARK = "검사용";
                        item5.GBSUNAP = "1";
                        item5.TUYAKNO = 0;
                        item5.MULTI = "";
                        item5.MULTIREMARK = "";
                        item5.DUR = "";
                        item5.RESV = "";
                        item5.SCODESAYU = "";
                        item5.SCODEREMARK = "";
                        item5.GBSEND = "Y";
                        item5.SABUN = clsType.User.IdNumber;
                        item5.CORDERCODE = "00440120";
                        item5.CSUCODE = "E7630S";
                        item5.CBUN = "200";
                        item5.IP = clsPublic.GstrIpAddress;

                        result = comHpcLibBService.InsertOcsOrder(item5);

                        //내시경장소 체크
                        if (strHEAENDO == "Y")
                        {
                            //NSB
                            COMHPC item6 = new COMHPC();

                            item6.PTNO = strPtNo;
                            item6.BDATE = clsPublic.GstrSysDate;
                            item6.DEPTCODE = "HR";
                            item6.SEQNO = 99;
                            item6.ORDERCODE = "00440430";
                            item6.SUCODE = "NSB";
                            item6.BUN = "20";
                            item6.SLIPNO = "0044";
                            item6.REALQTY = 1;
                            item6.QTY = 1;
                            item6.NAL = 1;
                            item6.GBDIV = "1";
                            item6.DOSCODE = "920103";
                            item6.GBBOTH = "0";
                            item6.GBINFO = "";
                            item6.GBER = "";
                            item6.GBSELF = "";
                            item6.GBSPC = "";
                            item6.BI = "51";
                            item6.DRCODE = strDrCode;
                            item6.REMARK = "검사용";
                            item6.GBSUNAP = "1";
                            item6.TUYAKNO = 0;
                            item6.MULTI = "";
                            item6.MULTIREMARK = "";
                            item6.DUR = "";
                            item6.RESV = "";
                            item6.SCODESAYU = "";
                            item6.SCODEREMARK = "";
                            item6.GBSEND = "Y";
                            item6.SABUN = clsType.User.IdNumber;
                            item6.CORDERCODE = "00440430";
                            item6.CSUCODE = "NSB";
                            item6.CBUN = "200";
                            item6.IP = clsPublic.GstrIpAddress;

                            result = comHpcLibBService.InsertOcsOrder(item6);
                        }
                        else
                        {
                            //5DWA-S
                            COMHPC item7 = new COMHPC();

                            item7.PTNO = strPtNo;
                            item7.BDATE = clsPublic.GstrSysDate;
                            item7.DEPTCODE = "HR";
                            item7.SEQNO = 99;
                            item7.ORDERCODE = "00400311";
                            item7.SUCODE = "5DWA-S";
                            item7.BUN = "20";
                            item7.SLIPNO = "0044";
                            item7.REALQTY = 1;
                            item7.QTY = 1;
                            item7.NAL = 1;
                            item7.GBDIV = "1";
                            item7.DOSCODE = "920103";
                            item7.GBBOTH = "0";
                            item7.GBINFO = "";
                            item7.GBER = "";
                            item7.GBSELF = "";
                            item7.GBSPC = "";
                            item7.BI = "51";
                            item7.DRCODE = strDrCode;
                            item7.REMARK = "검사용";
                            item7.GBSUNAP = "1";
                            item7.TUYAKNO = 0;
                            item7.MULTI = "";
                            item7.MULTIREMARK = "";
                            item7.DUR = "";
                            item7.RESV = "";
                            item7.SCODESAYU = "";
                            item7.SCODEREMARK = "";
                            item7.GBSEND = "Y";
                            item7.SABUN = clsType.User.IdNumber;
                            item7.CORDERCODE = "00400311";
                            item7.CSUCODE = "5DWA";
                            item7.CBUN = "200";
                            item7.IP = clsPublic.GstrIpAddress;

                            result = comHpcLibBService.InsertOcsOrder(item7);
                        }
                    }
                    else if (argEGD1 == "1" && argEGD2 == "")
                    {
                        COMHPC item8 = new COMHPC();

                        item8.PTNO = strPtNo;
                        item8.BDATE = clsPublic.GstrSysDate;
                        item8.DEPTCODE = "HR";
                        item8.SEQNO = 99;
                        item8.ORDERCODE = "00440110";
                        item8.SUCODE = "E7630";
                        item8.BUN = "20";
                        item8.SLIPNO = "0044";
                        item8.REALQTY = 1;
                        item8.QTY = 1;
                        item8.NAL = 1;
                        item8.GBDIV = "1";
                        item8.DOSCODE = "";
                        item8.GBBOTH = "0";
                        item8.GBINFO = "";
                        item8.GBER = "";
                        item8.GBSELF = "";
                        item8.GBSPC = "";
                        item8.BI = "51";
                        item8.DRCODE = strDrCode;
                        item8.REMARK = "검사용";
                        item8.GBSUNAP = "1";
                        item8.TUYAKNO = 0;
                        item8.MULTI = "";
                        item8.MULTIREMARK = "";
                        item8.DUR = "";
                        item8.RESV = "";
                        item8.SCODESAYU = "";
                        item8.SCODEREMARK = "";
                        item8.GBSEND = "Y";
                        item8.SABUN = clsType.User.IdNumber;
                        item8.CORDERCODE = "00440110";
                        item8.CSUCODE = "E7630";
                        item8.CBUN = "200";
                        item8.IP = clsPublic.GstrIpAddress;

                        result = comHpcLibBService.InsertOcsOrder(item8);
                    }
                }
            }

            //OILLS
            if (comHpcLibBService.GetCountbyPtNoBDate(strPtNo, strJepDate) == 0)
            {
                COMHPC item = new COMHPC();

                item.PTNO = strPtNo;
                item.BDATE = strJepDate;
                item.DEPTCODE = "HR";
                item.SEQNO = 1;
                item.ILLCODE = "Z018";

                result = comHpcLibBService.InsertOIlls(item);
            }
        }

        void eCode_value(HIC_CODE item)
        {
            CodeHelpItem = item;
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            string strValue = "";

            if (sender == SS_JinDan)
            {
                if (e.Column == 1)
                {
                    if (SS_JinDan.ActiveSheet.Cells[e.Row, 2].Text == "")
                    {
                        switch (e.Row)
                        {
                            case 0:
                                txtRemark.Text += "뇌졸증 병력( 년)" + "\r\n";
                                break;
                            case 1:
                                txtRemark.Text += "심장병 병력( 년)" + "\r\n";
                                break;
                            case 2:
                                txtRemark.Text += "고혈압 병력( 년)" + "\r\n";
                                break;
                            case 3:
                                txtRemark.Text += "당뇨병 병력( 년)" + "\r\n";
                                break;
                            case 4:
                                txtRemark.Text += "이상지질혈증 병력( 년)" + "\r\n";
                                break;
                            case 5:
                                txtRemark.Text += "간장질환 병력( 년)" + "\r\n";
                                break;
                            case 6:
                                txtRemark.Text += "폐결핵 병력( 년)" + "\r\n";
                                break;
                            case 7:
                                txtRemark.Text += "기타 병력( 년)" + "\r\n";
                                break;
                            default:
                                break;
                        }
                    }
                }
                else if (e.Column == 2)
                {
                    if (SS_JinDan.ActiveSheet.Cells[e.Row, 2].Text == "")
                    {
                        SS_JinDan.ActiveSheet.Cells[e.Row, 2].Text = "True";
                    }
                    else
                    {
                        SS_JinDan.ActiveSheet.Cells[e.Row, 2].Text = "";
                    }

                    switch (e.Row)
                    {
                        case 0:
                            txtRemark.Text += "뇌졸중 약물치료중( 년)" + "\r\n";
                            break;
                        case 1:
                            txtRemark.Text += "심장병 약물치료중( 년)" + "\r\n";
                            break;
                        case 2:
                            txtRemark.Text += "고혈압 약물치료중( 년)" + "\r\n";
                            break;
                        case 3:
                            txtRemark.Text += "당뇨병 약물치료중( 년)" + "\r\n";
                            break;
                        case 4:
                            txtRemark.Text += "이상지질혈증 약물치료중( 년)" + "\r\n";
                            break;
                        case 5:
                            txtRemark.Text += "간장질환 약물치료중( 년)" + "\r\n";
                            break;
                        case 6:
                            txtRemark.Text += "폐결핵 약물치료중( 년)" + "\r\n";
                            break;
                        case 7:
                            txtRemark.Text += "기타 약물치료중( 년)" + "\r\n";
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        void eSpdKeyDown(object sender, KeyEventArgs e)
        {
            string strCODE = "";
            string strResult = "";
            string strResCode = "";
            string strResType = "";

            if (FnRowNo < 0 || FnRowNo > SS2.ActiveSheet.RowCount)
            {
                return;
            }

            FnRowNo = SS2.ActiveSheet.ActiveRowIndex;

            strCODE = SS2.ActiveSheet.Cells[(int)FnRowNo, 0].Text.Trim();
            strResCode = SS2.ActiveSheet.Cells[(int)FnRowNo, 5].Text.Trim();
            strResType = SS2.ActiveSheet.Cells[(int)FnRowNo, 8].Text.Trim();

            if (strResult != "")
            {
                SS2.ActiveSheet.Cells[(int)FnRowNo, 2].Text = strResult;
                SS2.ActiveSheet.Cells[(int)FnRowNo, 6].Text = "Y";
                if (FnRowNo > SS2.ActiveSheet.RowCount)
                {
                    FnRowNo = SS2.ActiveSheet.RowCount;
                    SS2.ActiveSheet.SetActiveCell((int)FnRowNo, 2);
                }
            }
        }

        void eSpdEditModeOff(object sender, EventArgs e)
        {
            string strResCode = "";

            if (sender == SS2)
            {
                if (SS2.ActiveSheet.ActiveColumnIndex != 2)
                {
                    return;
                }
            }

            strResCode = SS2.ActiveSheet.Cells[SS2.ActiveSheet.ActiveRowIndex, 5].Text.Trim();

            if (strResCode == "")
            {
                FnClickRow = 0;
                return;
            }
        }

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            if (sender == SS2)
            {
                SS2.ActiveSheet.Cells[e.Row, 6].Text = "Y";
            }
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS2)
            {
                if (e.Column != 3)
                {
                    return;
                }

                FnClickRow = e.Row;
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSList)
            {
                if (e.ColumnHeader == true)
                {
                    return;
                }

                //FnRow = e.Row;
                fn_Screen_Clear();

                txtWrtNo.Text = SSList.ActiveSheet.Cells[e.Row, 0].Text.Trim();

                fn_Screen_Display();
            }
            else if (sender == SS2)
            {
                if (e.Column != 2)
                {
                    return;
                }
                SS2.ActiveSheet.SetActiveCell(e.Row, 2);
                SS2.Focus();
            }
            else if (sender == SSHistory)
            {
                if (string.Compare(SSHistory.ActiveSheet.Cells[e.Row, 3].Text, "2") <= 0)
                {
                    FrmHcPanView = new frmHcPanView(long.Parse(SSHistory.ActiveSheet.Cells[e.Row, 2].Text.Trim())); //WrtNo
                    FrmHcPanView.StartPosition = FormStartPosition.CenterScreen;
                    FrmHcPanView.ShowDialog(this);
                }
                else if (string.Compare(SSHistory.ActiveSheet.Cells[e.Row, 3].Text, "3") <= 0)
                {
                    clsPublic.GstrRetValue = SSHistory.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                }
            }
            else if (sender == SSJong)
            {
                long nWRTNO = 0;

                nWRTNO = long.Parse(SSJong.ActiveSheet.Cells[e.Row, e.Column].Text);

                if (nWRTNO == 0)
                {
                    return;
                }

                if (FnWRTNO == nWRTNO)
                {
                    return;
                }

                fn_Screen_Clear();
                txtWrtNo.Text = nWRTNO.ToString();
                FnWRTNO = nWRTNO;
                fn_Screen_Display();
            }
            else if (sender == SSList)
            {
                long nWRTNO = 0;
                string strYear = "";
                string strGjJong = "";

                if (e.Row == 0)
                {
                    return;
                }

                txtWrtNo.Focus();

                nWRTNO = long.Parse(SSList.ActiveSheet.Cells[e.Row, 0].Text);
                strYear = SSList.ActiveSheet.Cells[e.Row, 5].Text;
                strGjJong = SSList.ActiveSheet.Cells[e.Row, 3].Text.Trim();

                fn_Screen_Clear();

                txtWrtNo.Text = nWRTNO.ToString();
                FnWRTNO = nWRTNO;

                fn_Screen_Display();

                btnSave.Enabled = true;
            }
        }

        void eRdoClick(object sender, EventArgs e)
        {
            if (sender == rdoJob1 || sender == rdoJob2)
            {
                eBtnClick(btnSearch, new EventArgs());
            }
            else if (sender == rdoX10)
            {
                if (rdoX10.Checked == true)
                {
                    pnlX1.Enabled = false;
                }
                else
                {
                    pnlX1.Enabled = true;
                }
            }
            else if (sender == rdoX11)
            {
                pnlX1.Enabled = true;
            }
            else if (sender == rdoX20)
            {
                if (rdoX20.Checked == true)
                {
                    pnlX21.Enabled = false;
                }
                else
                {
                    pnlX21.Enabled = true;
                }
            }
            else if (sender == rdoX21)
            {
                pnlX21.Enabled = true;
            }
            else if (sender == rdoBodyCondition0)
            {
                rdoJinchal20.Checked = true;
            }
            else if (sender == rdoBodyCondition1)
            {
                rdoJinchal22.Checked = true;
            }
            else if (sender == rdoBodyCondition2)
            {
                rdoJinchal22.Checked = true;
            }
            else if (sender == rdoBodyCondition3)
            {
                rdoJinchal22.Checked = true;
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            long nWrtNo = 0;

            if (e.KeyChar == 13)
            {
                if (sender == txtPanDrNo)
                {
                    lblDrName.Text = hb.READ_License_DrName(long.Parse(txtPanDrNo.Text));
                    SendKeys.Send("{TAB}");
                }
                else if (sender == txtWrtNo)
                {
                    if (txtWrtNo.Text.Trim() == "") return;

                    nWrtNo = long.Parse(txtWrtNo.Text);
                    fn_Screen_Clear();
                    txtWrtNo.Text = nWrtNo.ToString();
                    FnWRTNO = nWrtNo;
                    fn_Screen_Display();
                }
                else
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            if (sender == txtGajok)
            {
                txtGajok.SelectionStart = 0;
                txtGajok.SelectionLength = txtGajok.Text.Length;
            }
            else if (sender == txtGiinsung)
            {
                txtGiinsung.SelectionStart = 0;
                txtGiinsung.SelectionLength = txtGiinsung.Text.Length;
            }
            else if (sender == txtJinChal0)
            {
                txtJinChal0.SelectionStart = 0;
                txtJinChal0.SelectionLength = txtJinChal0.Text.Length;
            }
            else if (sender == txtJinChal1)
            {
                txtJinChal1.SelectionStart = 0;
                txtJinChal1.SelectionLength = txtJinChal1.Text.Length;
            }
            else if (sender == txtJinChal2)
            {
                txtJinChal2.SelectionStart = 0;
                txtJinChal2.SelectionLength = txtJinChal2.Text.Length;
            }
            else if (sender == txtJinChal3)
            {
                txtJinChal3.SelectionStart = 0;
                txtJinChal3.SelectionLength = txtJinChal3.Text.Length;
            }
            else if (sender == txtRemark)
            {
                txtRemark.ImeMode = ImeMode.Hangul;
            }
            else if (sender == txtSName)
            {
                txtSName.ImeMode = ImeMode.Hangul;
            }
        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            if (sender == txtRemark)
            {
                txtRemark.ImeMode = ImeMode.Hangul;
            }
            else if (sender == txtSName)
            {
                txtSName.ImeMode = ImeMode.Hangul;
            }
        }

        void eCheckBoxClick(object sender, EventArgs e)
        {
            if (sender != chkRoom4)
            {
                if (chkRoom0.Checked == true || chkRoom1.Checked == true || chkRoom2.Checked == true || chkRoom3.Checked == true || chkRoom5.Checked == true)
                {
                    chkRoom4.Checked = false;
                }
            }
            else if (sender == chkRoom4)
            {
                if (chkRoom4.Checked == true)
                {
                    chkRoom0.Checked = false;
                    chkRoom1.Checked = false;
                    chkRoom2.Checked = false;
                    chkRoom3.Checked = false;
                    chkRoom5.Checked = false;
                    return;
                }
            }
            else if (sender == chkX1_12)
            {
                if (chkX1_12.Checked == true)
                {
                    txtX1_1.Enabled = true;
                }
                else
                {
                    txtX1_1.Enabled = false;
                }
            }
            else if (sender == chkX1_22)
            {
                if (chkX1_22.Checked == true)
                {
                    txtX1_2.Enabled = true;
                }
                else
                {
                    txtX1_2.Enabled = false;
                }
            }
            else if (sender == chkX1_40)
            {
                
            }
            else if (sender == chkX1_41)
            {
                if (chkX1_41.Checked == true)
                {
                    txtX1_4.Enabled = true;
                }
                else
                {
                    txtX1_4.Enabled = false;
                }
            }
            else if (sender == chkX42)
            {
                if (chkX42.Checked == true)
                {
                    txtJikjong.Enabled = true;
                }
                else
                {
                    txtJikjong.Enabled = false;
                }
            }
        }

        void eComboBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == cboXMonth)
                {
                    SendKeys.Send("{TAB}");
                }
                else if (sender == cboXYear)
                {
                    SendKeys.Send("{TAB}");
                }
                else
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void eTxtClick(object sender, EventArgs e)
        {
            if (sender == txtWrtNo)
            {
                txtWrtNo.SelectionStart = 0;
                txtWrtNo.SelectionLength = txtWrtNo.Text.Length;
            }
        }

        void eComboBoxClick(object sender, EventArgs e)
        {
            if (sender == cboXYear)
            {
                txtXTerm.Text = string.Format("{0:#0}", cboXYear.Text) + "년 " + string.Format("{0:#0}", cboXMonth.Text) + "개월";
            }
            else if (sender == cboXMonth)
            {
                txtXTerm.Text = string.Format("{0:#0}", cboXYear.Text) + "년 " + string.Format("{0:#0}", cboXMonth.Text) + "개월";
            }
        }

        void fn_Screen_Clear()
        {
            //공통항목 Clear-----------------------------
            FnWRTNO = 0;
            FnWrtno2 = 0;
            txtLtdCode.Text = "";
            txtSName.Text = "";
            txtWrtNo.Text = "";

            sp.Spread_All_Clear(SSHistory);
            sp.Spread_All_Clear(ssPatInfo);
            sp.Spread_All_Clear(SS2);
            sp.Spread_All_Clear(SS3);
            sp.Spread_All_Clear(SSJong);
            sp.Spread_All_Clear(SSHyang);
            sp.Spread_All_Clear(SS9);

            SS2.ActiveSheet.RowCount = 50;
            lblGjJong.Text = "";
            lblWait.Text = "";

            TabMain.Enabled = false;
            tabNight.Visible = false;

            for (int i = 0; i <= 5; i++)
            {
                FstrHabit[i] = "";
            }

            for (int i = 1; i <= 4; i++)
            {
                CheckBox chkHabit = (Controls.Find("chkHabit" + i.ToString(), true)[0] as CheckBox);
                chkHabit.Checked = false;
            }

            btnPACS.Enabled = false;
            btnMed.Enabled = false;
            btnAudio.Enabled = false;
            btnAudio1.Enabled = false;
            btnPFT.Enabled = false;
            btnMenuLungMun.Enabled = false;

            //(일반검진 내시경 동의서)
            btnMenuEndoConsent.Enabled = false;
            txtPanDrNo.Enabled = false;

            label5.Enabled = true;
            lblSpc.Text = "";

            FstrTFlag = "";

            lblGenral1.Visible = true;
            lblSpc1.Text = "";
            lblLivingHabit1.Text = "";
            lblGen20.Visible = true;
            lblGen21.Visible = true;
            lblEndo.Visible = true;
            txtLastMedHis.Visible = false;
            lblLivingHabit.Visible = true;
            lblTitle0.Text = "일반상태";
            lblTitle1.Text = "외상후유증";
            lblTitle2.Text = "식사여부";
            lblTitle3.Text = "생활습관개선";

            //Panel일반상태 => lblGeneral => pnlBodySymptom
            //SSPanel6 => pnlStatus1 => lblPTSD
            //SSPanel7 => pnlStatus2 => lblMealState
            //SSPanel8 => pnlStatus3 => lblLivingHabit

            pnlBodySymptom.Visible = false;
            pnlCancerHis.Visible = false;
            pnlStomachBowlLiver.Visible = false;
            pnlChestCervical.Visible = false;

            lblStatus1.Visible = true;      //일반상태
            lblPTSD.Visible = true;         //외상후유증
            lblMealState.Visible = true;    //식사여부

            tab1.Visible = false;
            tab2.Visible = false;
            tab3.Visible = false;
            tab4.Visible = false;
            tab5.Visible = false;
            tab6.Visible = false;

            txtExtInjury.Text = "";
            txtGenStatusEtc.Text = "";

            txtBodySymptom.Text = "특이소견없음";
            txtCancerHis.Text = "특이소견없음";
            txtStomachBowlLiver.Text = "특이소견없음";
            txtChestCervical.Text = "특이소견없음";

            //==[ 1차 상담항목 ]=======================================================================
            //----------------------------
            //         일반검진
            //----------------------------
            for (int i = 0; i < 8; i++)
            {
                SS_JinDan.ActiveSheet.Cells[i, 1].Text = "";
                SS_JinDan.ActiveSheet.Cells[i, 2].Text = "";
            }

            //----------------------------
            //         특수검진
            //----------------------------
            txtLastMedHis.Text = "";
            txtGajok.Text = "";
            txtGiinsung.Text = "";
            txtJengSang.Text = "";
            //임상관찰
            txtJinChal0.Text = "";
            txtJinChal1.Text = "";
            txtJinChal2.Text = "";
            txtJinChal3.Text = "";

            txtLastMedHis.Visible = false;

            //=========================================================================================
            for (int i = 0; i <= 2; i++)
            {
                RadioButton rdoDang = (Controls.Find("rdoDang" + i.ToString(), true)[0] as RadioButton);
                RadioButton rdoDangJob = (Controls.Find("rdoDangJob" + i.ToString(), true)[0] as RadioButton);
                RadioButton rdoGohyul = (Controls.Find("rdoGohyul" + i.ToString(), true)[0] as RadioButton);
                RadioButton rdoGohyulJob = (Controls.Find("rdoGohyulJob" + i.ToString(), true)[0] as RadioButton);

                rdoDang.Checked = false;
                rdoDangJob.Checked = false;
                rdoGohyul.Checked = false;
                rdoGohyulJob.Checked = false;
            }
            //=========================================================================================

            //공통항목
            rdoJinchal10.Checked = true;
            rdoJinchal20.Checked = true;
            rdoMealState1.Checked = true;

            txtRemark.Text = "";
            txtPanDrNo.Text = "";
            lblDrName.Text = "";
            btnSave.Enabled = true;

            //내시경 기록지 ---------------------------------------------------------------------
            for (int i = 0; i <= 6; i++)
            {
                CheckBox chkJong = (Controls.Find("chkJong" + i.ToString(), true)[0] as CheckBox);
                chkJong.Checked = false;
            }

            chkEGD0.Checked = false;
            chkEGD1.Checked = false;
            chkCFS0.Checked = false;
            chkCFS1.Checked = false;

            rdoBodyCondition0.Checked = true;

            for (int i = 0; i <= 14; i++)
            {
                CheckBox chkMedHis = (Controls.Find("chkMedHis" + i.ToString(), true)[0] as CheckBox);
                chkMedHis.Checked = false;
            }
            txtMedHis13.Text = "";
            txtMedHisEtc.Text = "";

            for (int i = 0; i <= 7; i++)
            {   
                CheckBox chkMedHis = (Controls.Find("chkMedHis" + i.ToString(), true)[0] as CheckBox);
                chkMedHis.Checked = false;
            }

            for (int i = 0; i <= 1; i++)
            {
                CheckBox chkPreTreatment = (Controls.Find("chkPreTreatment" + i.ToString(), true)[0] as CheckBox);
                chkPreTreatment.Checked = false;
            }

            chkPreTreatment0.Checked = true;
            txtJinjengUse.Text = "";
            cboASA.Text = "";

            FstrEndoRowID = "";

            //학생상담 -----------------------------------------------------------------------------
            cboPRes.SelectedIndex = 1;
            txtPResEtc.Text = "";
            cboEJ0.SelectedIndex = 1;
            cboEJ1.SelectedIndex = 1;
            txtEJEtc.Text = "";
            cboM.SelectedIndex = 1;
            txtMEtc.Text = "";
            cboN.SelectedIndex = 1;
            txtNEtc.Text = "";
            cboS.SelectedIndex = 1;
            txtSEtc.Text = "";
            cboHJ.SelectedIndex = 1;
            txtHJEtc.Text = "";
            cboJ.SelectedIndex = 1;
            txtJEtc.Text = "";

            for (int i = 0; i <= 5; i++)
            {
                ComboBox cboOrgan = (Controls.Find("cboOrgan" + i.ToString(), true)[0] as ComboBox);
                cboOrgan.SelectedIndex = 1;
            }

            txtOrgan0Etc.Text = "";
            txtOrgan1Etc.Text = "";
            txtOrgan2Etc.Text = "";
            txtOrgan3Etc.Text = "";
            txtOrgan4Etc.Text = "";
            txtOrgan5Etc.Text = "";

            //방사선종사자상담 -----------------------------------------------------------------------------
            txtXTerm.Text = "";
            txtMun1.Text = "";
            txtXJong.Text = "";
            txtEye.Text = "";
            txtPlace.Text = "";
            txtSkin.Text = "";
            txtXRemark.Text = "";
            txtEtc.Text = "";
            txtTerm.Text = "";
            txtMuch.Text = "";
            txtJung.Text = "";
            cboXYear.Text = "";
            cboXMonth.Text = "";

            //방사선종사자 상담프로그램 변경사항 적용
            tabControlX.SelectedTab = tabX1;
            txtXSymptom.Text = "";
            chkX40.Checked = false;
            chkX41.Checked = false;
            chkX42.Checked = false;
            txtJikjong.Text = "";

            //야간작업
            txtSum1.Text = "";
            lblPan1.Text = "";
            txtSum2.Text = "";
            lblPan2.Text = "";
            txtSum3.Text = "";
            lblPan3.Text = "";
            for (int i = 0; i <= 6; i++)
            {
                SS10.ActiveSheet.Cells[i, 1].Text = "";
                SS10.ActiveSheet.Cells[i, 2].Text = "";
                SS10.ActiveSheet.Cells[i, 3].Text = "";
                SS11.ActiveSheet.Cells[i, 1].Text = "";
                SS11.ActiveSheet.Cells[i, 2].Text = "";
                SS11.ActiveSheet.Cells[i, 3].Text = "";
            }

            for (int i = 0; i <= 7; i++)
            {
                SS12.ActiveSheet.Cells[i, 1].Text = "";
                SS12.ActiveSheet.Cells[i, 2].Text = "";
                SS12.ActiveSheet.Cells[i, 3].Text = "";
            }

            //폐암사후상담
            txtLung_Sang1.Text = "";
            txtLung_Sang2.Text = "";
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }
    }
}
