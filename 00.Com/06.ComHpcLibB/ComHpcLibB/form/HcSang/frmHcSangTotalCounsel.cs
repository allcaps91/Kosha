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
using System.Diagnostics;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcSangTotalCounsel.cs
/// Description     : 일반검진 통합상담 프로그램
/// Author          : 이상훈
/// Create Date     : 2020-02-13
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmHcSang_통합.frm(FrmHcSang_통합)" />

namespace ComHpcLibB
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
        HicResultExcodeJepsuService hicResultExcodeJepsuService = null;
        HicPrivacyAcceptService hicPrivacyAcceptService = null;
        HicJinGbnService hicJinGbnService = null;


        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();
        clsVbfunc vf = new clsVbfunc();
        clsHcFunc cHF = new clsHcFunc();
        clsOrdFunction OF = new clsOrdFunction();

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
        frmHcResultView FrmHcResultView = null;
        frmHaHyangjoengApproval FrmHaHyangjoengApproval = null;
        frmHcPermList_Rec FrmHcPermList_Rec = null;
        frmHcEmrConset_Rec FrmHcEmrConset_Rec = null;
        frmHcSangHyangjengApproval FrmHcSangHyangjengApproval = null;

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        long fnLicense = 0;
        long FnWRTNO;
        long FnWrtno2;          //2차 검진시 이전 1차 접수번호 
        long FnWRTNO3;
        long FnPano;
        long FnAge;
        string FstrPtno;
        string FstrSName;
        string FstrSex;
        string FstrJepDate;
        string FstrJumin;
        string FstrMunjin;
        string FstrChasu;
        string FstrUCodes;
        string[] FstrHabit = new string[19];
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

        string FstrFileName;
        string FstrDept;
        int FnFileCnt;
        string FstrDrno;
        string FstrFormList;
        string FstrFilePath;
        string FstrCmd = "";

        bool FbInsomniaMun = false;
        bool FbinsomniaMun2 = false;
        bool FbStomachMun = false;
        bool FbBreastCancerMun = false;
        bool FbMunJin = false;
        bool boolSort = false;

        string FstrCode;
        string FstrName;

        /// <summary>
        /// 환자 변경시 저장 메시지 여부
        /// </summary>
        bool FblnPatChangeSaveFlag = false; 

        public frmHcSangTotalCounsel()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcSangTotalCounsel(long argWrtno, bool bMunjin = false)
        {
            InitializeComponent();
            SetEvent();
            SetControl();
            FbMunJin = bMunjin;
            FnWRTNO3 = argWrtno;
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
            hicResultExcodeJepsuService = new HicResultExcodeJepsuService();
            hicPrivacyAcceptService = new HicPrivacyAcceptService();
            hicJinGbnService = new HicJinGbnService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnMenuCall.Click += new EventHandler(eBtnClick);
            this.btnMenuNull.Click += new EventHandler(eBtnClick);
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
            this.btnJengSang.Click += new EventHandler(eBtnClick); 
            this.btnWards.Click += new EventHandler(eBtnClick);
            this.rdoJob1.Click += new EventHandler(eRdoClick);
            this.rdoJob2.Click += new EventHandler(eRdoClick);
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
            //this.txtLastMedHis.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtBodySymptom.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtCancerHis.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtStomachBowlLiver.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtChestCervical.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtSName.GotFocus += new EventHandler(eTxtGotFocus);
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
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtPanDrNo.KeyPress += new KeyPressEventHandler(eKeyPress); 
            this.FormClosing += new FormClosingEventHandler(eFormClosing);

            this.cboXYear.TextChanged += new EventHandler(eCboTxtChanged);
            this.cboXMonth.TextChanged += new EventHandler(eCboTxtChanged);

            this.chkRoom0.CheckedChanged += new EventHandler(eChkBoxChanged);
            this.chkRoom1.CheckedChanged += new EventHandler(eChkBoxChanged);
            this.chkRoom2.CheckedChanged += new EventHandler(eChkBoxChanged);
            this.chkRoom3.CheckedChanged += new EventHandler(eChkBoxChanged);
            this.chkRoom4.CheckedChanged += new EventHandler(eChkBoxChanged);
            this.chkRoom5.CheckedChanged += new EventHandler(eChkBoxChanged);

            this.txtLastMedHis.Click += new EventHandler(eTxtClick);
            this.txtGajok.Click += new EventHandler(eTxtClick);
            this.txtGiinsung.Click += new EventHandler(eTxtClick);

            KeyPreview = true;
            KeyUp += Form_KeyUp;    //전현준과장 단축키 요청
        }

        /// <summary>
        /// 폼 키 UP 처리하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.A)
            {
                bool[] arrTabIdx = new bool[7];

                for (int i = 0; i < TabMain.Tabs.Count; i++)
                {
                    arrTabIdx[i] = TabMain.Tabs[i].Visible;
                }

                int nCNT = TabMain.Tabs.Count - 1;
                int nCurIdx = TabMain.SelectedTabIndex;

                if (nCNT < nCurIdx + 1)
                {
                    for (int i = 0; i < TabMain.Tabs.Count; i++)
                    {
                        if (arrTabIdx[i])
                        {
                            TabMain.SelectedTabIndex = i;
                            return;
                        }
                    }
                }
                else
                {
                    for (int i = nCurIdx + 1; i < TabMain.Tabs.Count; i++)
                    {
                        if (arrTabIdx[i])
                        {
                            TabMain.SelectedTabIndex = i;
                            return;
                        }
                    }

                    for (int i = 0; i < TabMain.Tabs.Count; i++)
                    {
                        if (arrTabIdx[i])
                        {
                            TabMain.SelectedTabIndex = i;
                            return;
                        }
                    }
                }
            }
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormClosing(object sender, FormClosingEventArgs e)
        {
            if (hf.OpenForm_Check("frmHcSangInternetMunjinView") == true)
            {
                //FrmHcSangInternetMunjinView.Close();
                FrmHcSangInternetMunjinView.Dispose();
                FrmHcSangInternetMunjinView = null;
            }

            ComFunc.KillProc("friendly omr.exe");
            ComFunc.KillProc("hcscript.exe");
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strData = "";

            FblnPatChangeSaveFlag = true;

            hf.SET_자료사전_VALUE();

            dtpFrDate.Text = clsPublic.GstrSysDate;
            dtpToDate.Text = clsPublic.GstrSysDate;

            chkMunjin.Checked = FbMunJin;

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
            hb.READ_HIC_Doctor(clsType.User.IdNumber.To<long>());

            this.Text += "(" + clsHcVariable.GstrHicDrName + ") (면허:" + clsHcVariable.GnHicLicense + ")";
            fnLicense = clsHcVariable.GnHicLicense;

            fn_ComboBox_Set();
            fn_Screen_Clear();

            List<HIC_EXJONG> list = hicExjongService.Read_ExJong_Add(true);

            if (list.Count > 0)
            {
                cboJong.Items.Clear();
                cboJong.Items.Add("**.전체");
                for (int i = 0; i < list.Count; i++)
                {
                    cboJong.Items.Add(list[i].CODE.Trim() + "." + list[i].NAME);
                }
                cboJong.SelectedIndex = 0;
            }

            cboASA.Items.Clear();
            cboASA.Items.Add("");
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

            if (FnWRTNO3 != 0)
            {
                fn_Screen_Clear();
                txtWrtNo.Text = FnWRTNO3.To<string>();

                fn_Screen_Display();
            }
        }

        void fn_ComboBox_Set()
        {
            //학생상담 Set=============================================

            //근골격 및 척추
            cboPRes.Items.Clear();
            cboPRes.Items.Add("");
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
            cboPRes.SelectedIndex = 0;

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
                cboEJ.SelectedIndex = 0;
            }

            //콧병
            cboM.Items.Clear();
            cboM.Items.Add(" ");
            cboM.Items.Add("1.없음");
            cboM.Items.Add("2.부비동염");
            cboM.Items.Add("3.비염");
            cboM.Items.Add("4.기타");
            cboM.SelectedIndex = 0;

            //목병
            cboN.Items.Clear();
            cboN.Items.Add(" ");
            cboN.Items.Add("1.없음");
            cboN.Items.Add("2.편도비대");
            cboN.Items.Add("3.임파절증대");
            cboN.Items.Add("4.갑상선비대");
            cboN.Items.Add("5.기타");
            cboN.SelectedIndex = 0;

            //피부병
            cboS.Items.Clear();
            cboS.Items.Add(" ");
            cboS.Items.Add("1.없음");
            cboS.Items.Add("2.아토피성피부염");
            cboS.Items.Add("3.전염성피부염");
            cboS.Items.Add("4.기타");
            cboS.SelectedIndex = 0;

            //귓병
            cboHJ.Items.Clear();
            cboHJ.Items.Add(" ");
            cboHJ.Items.Add("1.없음");
            cboHJ.Items.Add("2.중이염");
            cboHJ.Items.Add("3.외이도염");
            cboHJ.Items.Add("4.기타");
            cboHJ.SelectedIndex = 0;

            //진찰 및 상담
            cboJ.Items.Clear();
            cboJ.Items.Add(" ");
            cboJ.Items.Add("1.무");
            cboJ.Items.Add("2.유");
            cboJ.SelectedIndex = 0;


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
                cboOrgan.SelectedIndex = 0;
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
                cboXYear.Items.Add(string.Format("{0:00}", i));
            }
            cboXYear.SelectedIndex = 0;

            cboXMonth.Items.Clear();
            for (int i = 0; i <= 12; i++)
            {
                cboXMonth.Items.Add(string.Format("{0:00}", i));
            }
            cboXMonth.SelectedIndex = 0;
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

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                Form frmExit = hf.OpenForm_Check_Return("frmHcSangInternetMunjinView");

                if (!frmExit.IsNullOrEmpty())
                {
                    frmExit.Close();
                    frmExit.Dispose();
                    frmExit = null;
                }

                ComFunc.KillProc("friendly omr.exe");
                ComFunc.KillProc("hcscript.exe");

                this.Close();
                return;
            }
            else if (sender == btnCancel)
            {
                FblnPatChangeSaveFlag = true;
                fn_Screen_Clear();
                txtWrtNo.Focus();
            }
            else if (sender == btnLtdCode)
            {
                string strLtdCode = "";

                if (txtLtdCode.Text.IndexOf(".") > 0)
                {
                    strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 2);
                }
                else
                {
                    strLtdCode = txtLtdCode.Text;
                }

                FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
                FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                FrmHcLtdHelp.ShowDialog();
                FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

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
                FrmViewResult.ShowDialog(this);
            }
            else if (sender == btnAudio)
            {
                string strRowId = "";

                List<ETC_JUPMST> list = etcJupmstService.GetRowIdbyPtNoBDate(FstrPtno, FstrJepDate, "", "6");

                if (list.Count > 0)
                {
                    strRowId = list[0].ROWID;
                }

                if (!strRowId.IsNullOrEmpty())
                {
                    hf.AudioFILE_DBToFile(strRowId, "1");
                }
            }
            else if (sender == btnAudio1)
            {
                string strRowId = "";
                string strFileName = @"c:\CMC\팀파노이미지_0.jpg";
                FileInfo f = new FileInfo(strFileName);

                DirectoryInfo dir = new DirectoryInfo(@"c:\CMC\");
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
                if (hicSangdamWaitService.GetCountbyWrtNoGubun(clsHcVariable.GstrDrRoom, txtWrtNo.Text.To<long>()) > 0)
                {
                    result = hicSangdamWaitService.UpdateCallTimeDisplaybyOnlyWrtNo(txtWrtNo.Text.To<long>());

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

                if (!FstrCode.IsNullOrEmpty())
                {
                    txtJengSang.Text = FstrCode.Trim() + "." + FstrName.Trim();
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
                string sMsg = "";

                FblnPatChangeSaveFlag = true;

                if (txtWrtNo.Text.To<long>() == 0)
                {
                    return;
                }

                if (txtPanDrNo.Text.To<long>() == 0)
                {
                    MessageBox.Show("상담의사 면허번호가 누락되었습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (clsType.User.IdNumber != "28048")
                {
                    if (hicJepsuService.GetCountbyWrtNo(txtWrtNo.Text.To<long>()) > 0)
                    {
                        MessageBox.Show("판정완료 수검자 입니다. 상담수정을 하시려면 판정을 풀어주세요", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //야간작업 불면증 증상문진 점검
                if (!lblPan1.Text.IsNullOrEmpty() && txtSum1.Text.To<long>() > 0)
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
                }

                //ASA등급
                strASA = VB.Pstr(cboASA.Text, ".", 1).Trim();
                if (SSHyang.ActiveSheet.RowCount > 0 && strASA.IsNullOrEmpty())
                {
                    MessageBox.Show("신체등급(ASA)을 선택하십시오.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                #region 필수검사 체크 로직(2020.10.18)
                if (tab6.Visible == true)   //야간대상일 경우
                {
                    HIC_MUNJIN_NIGHT listItemData = hicMunjinNightService.GetItembyWrtNo(txtWrtNo.Text.To<long>());

                    if (FstrJong == "16" || FstrJong == "28")   //야간 2차(2020.11.30) 야간 1차/2차 분리 체크로직 추가
                    {
                        if (listItemData.ITEM2_DATA.IsNullOrEmpty())
                        {
                            MessageBox.Show("야간작업 문진표 작성이 누락 되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    else
                    {
                        if (listItemData.ITEM1_DATA.IsNullOrEmpty())
                        {
                            MessageBox.Show("야간작업 문진표 작성이 누락 되었습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }

                //A810(팀파노), A899(폐활량), A151(심전도), TZ08(악력(좌)), TZ09(악력(우))
                for (int i = 0; i < SS2.ActiveSheet.NonEmptyRowCount; i++)
                {
                    switch (SS2.ActiveSheet.Cells[i, 0].Text.Trim())
                    {
                        case "TH01":    //팀파노(좌)
                            if (SS2.ActiveSheet.Cells[i, 2].Text.Trim() == "")
                            {
                                sMsg += "팀파노(좌),";
                            }
                            break;
                        case "TH02":    //팀파노(우)
                            if (SS2.ActiveSheet.Cells[i, 2].Text.Trim() == "")
                            {
                                sMsg += "팀파노(우),";
                            }
                            break;
                        case "A151":    //심전도
                        case "A153":
                            if (SS2.ActiveSheet.Cells[i, 2].Text.Trim() == "")
                            {
                                sMsg += "심전도,";
                            }
                            break;
                        case "A899":    //폐활량
                            if (SS2.ActiveSheet.Cells[i, 2].Text.Trim() == "")
                            {
                                sMsg += "폐활량,";
                            }
                            break;
                        case "TZ08":    //악력(좌)
                            if (SS2.ActiveSheet.Cells[i, 2].Text.Trim() == "")
                            {
                                sMsg += "악력(좌),";
                            }
                            break;
                        case "TZ09":    //악력(우)
                            if (SS2.ActiveSheet.Cells[i, 2].Text.Trim() == "")
                            {
                                sMsg += "악력(우),";
                            }
                            break;
                        default:
                            break;
                    }
                }

                if (!sMsg.IsNullOrEmpty())
                {
                    sMsg = VB.Left(sMsg, sMsg.Length - 1);
                    MessageBox.Show(sMsg + " 결과가 누락 되었습니다! 저장 할 수 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                #endregion

                FnDrno = txtPanDrNo.Text.To<long>();
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

                //clsDB.setCommitTran(clsDB.DbCon);

                FstrCOMMIT = "OK";

                //상담대기순번 완료 및 다음검사실 지정
                fn_WAIT_NextRoom_SET();

                nIdx = TabMain.SelectedTabIndex;

                switch (nIdx)
                {
                    case 0:
                        fn_Save1(); //CmdSave1_Click(1차 일반상담 저장)
                        if (FstrCOMMIT != "OK")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
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

                //사전점검에 걸리면 저장로직 빠져나감
                if (FstrCOMMIT != "OK")
                {
                    MessageBox.Show("사전점검 체크로직 오류입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                ////준종합검진 상병코드입력
                //if (FstrJong != "31")
                //{
                //    fn_OILLS_INSERT();
                //}

                if (FstrCOMMIT != "OK")
                {
                    MessageBox.Show("OILLS_INSERT 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                //진단서의사 UPDATE
                if (FstrJong == "32")
                {
                    fn_JIN_GBN_UPDATE();
                }

                if (FstrCOMMIT != "OK")
                {
                    MessageBox.Show("진단서의사 업데이트 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                //62종, 69종 상담저장
                if (FstrJong == "11")
                {
                    fn_SANGDAM_ADD_UPDATE();
                }

                if (FstrCOMMIT != "OK")
                {
                    MessageBox.Show("62,69상담 체크로직오류.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                fn_Update_Result_Data();

                if (FstrCOMMIT != "OK")
                {
                    MessageBox.Show("결과업데이트 오류.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                //당일수검 종류와 상태를 체크
                fn_Read_Jepsu_Display();

                if (FstrCOMMIT == "OK")
                {
                    fn_UseCheck();
                    clsDB.setCommitTran(clsDB.DbCon);
                }
                else
                {
                    MessageBox.Show("상담최종저장 오류발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
               
                //일반검진상담내용 종합검진상담내역UPDATE(2021-03-17)
                //HEA_JEPSU item1 = heaJepsuService.Read_Jepsu3(FstrPtno, FstrJepDate);
                //if (!item1.IsNullOrEmpty() && FstrGjJong == "11")
                //{
                //    if (item1.SANGDAM.IsNullOrEmpty())
                //    {
                //        heaJepsuService.UpdateSangdamByWrtno(txtRemark.Text, item1.WRTNO);
                //    }
                //    else
                //    {
                //        if (MessageBox.Show(" 종합검진상담 내용이 있습니다." + "\r\n\r\n" + " 그래도 저장 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //        {
                //            heaJepsuService.UpdateSangdamByWrtno(txtRemark.Text, item1.WRTNO);
                //        }
                //    }
                //}

                //상담완료 되지않은것을 찾아 자동으로 Display
                for (int i = 0; i <= 4; i++)
                {
                    if (!SSJong.ActiveSheet.Cells[0, i].Text.IsNullOrEmpty() && SSJong.ActiveSheet.Cells[0, i].BackColor == ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HBAFEFC")))
                    {
                        if (nIdx != i)    //현재 상담위치를 제외함
                        {
                            nWRTNO = SSJong.ActiveSheet.Cells[1, i].Text.To<long>();
                            break;
                        }
                    }
                }

                fn_Screen_Clear();

                if (nWRTNO == 0)
                {
                    txtWrtNo.Focus();
                    txtWrtNo.Select();
                    eBtnClick(btnSearch, new EventArgs());
                }
                else
                {
                    txtWrtNo.Text = nWRTNO.To<string>();
                    FnWRTNO = nWRTNO;
                    fn_Screen_Display();
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

                Cursor.Current = Cursors.WaitCursor;

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
                                continue;
                            case 1:
                                strGubun.Add("17");
                                continue;
                            case 2:
                                strGubun.Add("16");
                                continue;
                            case 3:
                                strGubun.Add("15");
                                continue;
                            case 5:
                                strGubun.Add("19");
                                continue;
                            case 4:
                                strGubun.Add("");
                                continue;
                            default:
                                continue;
                        }
                    }
                }

                strDrList.Clear();
                if (!strGubun.IsNullOrEmpty() && rdoJob2.Checked == true)
                {
                    if (strGubun.Count > 0)
                    {
                        if (!strGubun[0].IsNullOrEmpty())
                        {
                            List<HIC_DOCTOR> list = hicDoctorService.GetSabunbyRoom(strGubun);

                            nREAD = list.Count;
                            for (int i = 0; i < nREAD; i++)
                            {
                                strDrList.Add(list[i].SABUN.To<string>());
                            }
                        }
                    }
                }

                for (int i = 0; i <= 3; i++)
                {
                    RadioButton rdoGbn = (Controls.Find("rdoGbn" + i.ToString(), true)[0] as RadioButton);
                    if (rdoGbn.Checked == true)
                    {
                        strGbn = i.To<string>();
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
                    strJob = "0";
                }
                else if (rdoJob2.Checked == true)
                {
                    strJob = "1";
                }

                strSName = txtSName.Text.Trim();
                nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                strJong = VB.Left(cboJong.Text, 2);

                sp.Spread_All_Clear(SSList);

                if (hicSangdamWaitService.GetCountbyGubunSName(clsHcVariable.GstrDrRoom, "{자리비움}")> 0)
                {
                    MessageBox.Show("자리비움입니다. 자리비움상태를 취소해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //상담대기자를 읽음
                List<HIC_JEPSU_SANGDAM_NEW_EXJONG> list2 = hicJepsuSangdamNewExjongService.GetItembyJepDateGjJong(strFrDate, strToDate, strChul, strGbn, strJob, strRoom, strDrList, strGubun, strSName, strJong, nLtdCode);

                nREAD = list2.Count;
                SSList.ActiveSheet.RowCount = 0;

                nRow = 0;
                if (nREAD > 0)
                {
                    progressBar1.Maximum = nREAD;
                    for (int i = 0; i < nREAD; i++)
                    {
                        strOK = "OK";
                        nPano = list2[i].PANO;
                        strTemp = list2[i].SANGDAMDRNO.To<string>();
                        if (strTemp == "0") strTemp = "";
                        if (strJob == "0")  //대기자
                        {
                            if (!strTemp.IsNullOrEmpty())
                            {
                                strOK = "";
                            }
                        }
                        else
                        {
                            if (strTemp.IsNullOrEmpty())
                            {
                                strOK = "";
                            }
                        }

                        if (strOK == "OK")
                        {
                            if (strRoom == "1")
                            {
                                if (hicSangdamWaitService.GetCountbyOnlyWrtNo(list2[i].WRTNO) > 0)
                                {
                                    strOK = "";
                                }
                            }
                        }

                        if (strOK == "OK")
                        {
                            //상담 Acting 코드가 있는 사람만 표시함...
                            if (hb.READ_SangDam_Acting(list2[i].WRTNO).IsNullOrEmpty())
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
                            nRow += 1;
                            if (SSList.ActiveSheet.RowCount < nRow)
                            {
                                SSList.ActiveSheet.RowCount = nRow;
                            }
                            SSList.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                            SSList.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                            SSList.ActiveSheet.Cells[nRow - 1, 0].Text = list2[i].WRTNO.To<string>();
                            SSList.ActiveSheet.Cells[nRow - 1, 1].Text = list2[i].SNAME;
                            SSList.ActiveSheet.Cells[nRow - 1, 2].Text = list2[i].JEPDATE;
                            strJong = list2[i].GJJONG;
                            //SSList.ActiveSheet.Cells[nRow - 1, 3].Text = hb.READ_GjJong_Name(list2[i].GJJONG
                            SSList.ActiveSheet.Cells[nRow - 1, 3].Text = list2[i].GJJONG;
                            SSList.ActiveSheet.Cells[nRow - 1, 5].Text = list2[i].GJYEAR;

                            //일특 체크
                            if (strJong == "11" || strJong == "16" || strJong == "41" || strJong == "44")
                            {
                                if (!list2[i].UCODES.IsNullOrEmpty())
                                {
                                    SSList.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                                    SSList.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(200, 255, 200);
                                }
                            }

                            if (strTemp == clsType.User.IdNumber)
                            {
                                SSList.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
                                SSList.ActiveSheet.Cells[nRow - 1, 0, nRow - 1, SSList.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFC0FF"));
                            }

                            if (strJong == "11" || strJong == "12" || strJong == "13" || strJong == "14" || strJong == "41" || strJong == "42" || strJong == "43")
                            {
                                if (fn_Read_PubCorpCancerScreeningYN(nPano, list2[i].JEPDATE) > 0)
                                {
                                    SSList.ActiveSheet.Cells[nRow - 1, 3].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HFFC0C0"));
                                }
                            }
                            SSList.ActiveSheet.Cells[nRow - 1, 4].Text = fn_Read_SangDam_Check(nPano, list2[i].JEPDATE);
                        }
                        progressBar1.Value = i + 1;
                    }

                    SSList.ActiveSheet.RowCount = nRow;
                    if (SSList.ActiveSheet.RowCount > 0)
                    {
                        if (rdoJob1.Checked == true)
                        {
                            SSList.ActiveSheet.Cells[0, 0, 0, SSList.ActiveSheet.ColumnCount - 1].BackColor = ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString("&HC0E0FF"));
                        }
                    }
                }

                sp.SetfpsRowHeight(SSList, 22);

                //상담인원 및 대기인원 DISPLAY
                HIC_SANGDAM_NEW_JEPSU_EXJONG list3 = hicSangdamNewJepsuExjongService.GetCntCnt2(clsHcVariable.GnHicLicense.To<long>());

                lblCounter.Text = "총 대기인원: ";
                lblCounter.Text += list3.CNT2 + "명 / ";
                lblCounter.Text += "상담인원 : ";
                lblCounter.Text += list3.CNT + " 명";

                Cursor.Current = Cursors.Default;
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
                FrmHcSangLivingHabitPrescription = new frmHcSangLivingHabitPrescription(txtWrtNo.Text.To<long>());
                FrmHcSangLivingHabitPrescription.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSangLivingHabitPrescription.ShowDialog(this);
            }
            else if (sender == btnMenuEndoConsent)
            {
                if (clsType.User.IdNumber.To<long>() != 28048 && clsType.User.IdNumber.To<long>() != 34902 && clsType.User.IdNumber.To<long>() != 39604 && clsType.User.IdNumber.To<long>() != 41326)
                //if (clsType.User.IdNumber.To<long>() != 28048 && clsType.User.IdNumber.To<long>() != 36540 )
                {
                    //더블클릭 방지
                    btnMenuEndoConsent.Enabled = false;
                    FrmHcPermList_Rec = new frmHcPermList_Rec(FnWRTNO);
                    FrmHcPermList_Rec.Show();
                    btnMenuEndoConsent.Enabled = true;
                }
                else
                {
                    //전자동의서
                    btnMenuEndoConsent.Enabled = false;
                    FrmHcEmrConset_Rec = new frmHcEmrConset_Rec(FnWRTNO, "DOCTOR");
                    FrmHcEmrConset_Rec.Show();
                    btnMenuEndoConsent.Enabled = true;
                }

            }
            else if (sender == btnMenuUnSave)
            {
                fn_Screen_Clear();
                FblnPatChangeSaveFlag = true;
            }
            else if (sender == btnMenuWard)
            {
                FrmHcSchoolCommonDistrictRegView = new frmHcSchoolCommonDistrictRegView();
                FrmHcSchoolCommonDistrictRegView.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSchoolCommonDistrictRegView.ShowDialog(this);
            }
            else if (sender == btnMenuWait)
            {
                int nRead = 0;
                int nWaitNo = 0;
                string strFrDate = "";
                string strToDate = "";
                int result = 0;

                if (FnWRTNO == 0) return;

                if (clsHcVariable.GstrDrRoom.IsNullOrEmpty()) return;

                if (MessageBox.Show("상담실 방번호를 변경하시겠습니까?", "상담실 구분변경", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                clsHcVariable.GnWRTNO = FnWRTNO;
                nWaitNo = 2;

                strFrDate = FstrJepDate;
                strToDate = DateTime.Parse(FstrJepDate).AddDays(1).ToShortDateString();

                if (clsHcVariable.GstrDrRoom == hicSangdamWaitService.GetGubunbyWrtNo(FnWRTNO))
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

                result = hicSangdamWaitService.UpdateWaitNoGubunbyWrtNo(clsHcVariable.GstrDrRoom, FnWRTNO);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담순번 변경 시 오류 발생!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                eBtnClick(btnSearch, new EventArgs());
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

                clsDB.setCommitTran(clsDB.DbCon);
                //clsDB.setRollbackTran(clsDB.DbCon);
            }
            else if (sender == btnMenuNull)
            {
                int result = 0;
                string strMsg = "";

                strMsg = "자리비움 상태를 변경하시겠습니까?" + ComNum.VBLF;
                strMsg = strMsg + "자리비움은 예(Y), 자리비움취소는 아니오(N)를 누르세요." + ComNum.VBLF;

                if (MessageBox.Show(strMsg, "확인사항", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {

                    clsDB.setBeginTran(clsDB.DbCon);

                    result = hicSangdamWaitService.DeletebyGubun(clsHcVariable.GstrDrRoom);

                    HIC_SANGDAM_WAIT item = new HIC_SANGDAM_WAIT();

                    item.WRTNO = 0;
                    item.SNAME = "{자리비움}";
                    item.GUBUN = clsHcVariable.GstrDrRoom;

                    result = hicSangdamWaitService.InserWrtNoSNameGubunt(item);

                    if (result < 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("자리비움 저장 실패", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    //clsDB.setRollbackTran(clsDB.DbCon);
                }
                else
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    result = hicSangdamWaitService.DeletebyGubun(clsHcVariable.GstrDrRoom);
                    clsDB.setCommitTran(clsDB.DbCon);
                }
            }
            else if (sender == btnMenuLungMun)
            {
                FrmHcActPFTMunjin = new frmHcActPFTMunjin("HCSang", "SANG", FnWRTNO, FstrPtno, txtWrtNo.Text.To<long>());
                FrmHcActPFTMunjin.StartPosition = FormStartPosition.CenterScreen;
                FrmHcActPFTMunjin.ShowDialog(this);
            }
            else if (sender == btnMenuPsychotropicDrugsApproval)
            {
                FrmHcSangHyangjengApproval = new frmHcSangHyangjengApproval();
                FrmHcSangHyangjengApproval.ShowDialog(this);

                //FrmHaHyangjoengApproval = new frmHaHyangjoengApproval();
                //FrmHaHyangjoengApproval.ShowDialog(this);
            }
        }

        void eCboTxtChanged(object sender, EventArgs e)
        {
            if (sender == cboXYear)
            {
                txtXTerm.Text = string.Format("{0:#0}", cboXYear.Text.To<int>()) + "년 " + string.Format("{0:#0}", cboXMonth.Text.To<int>()) + "개월";
            }
            else if (sender == cboXMonth)
            {
                txtXTerm.Text = string.Format("{0:#0}", cboXYear.Text.To<int>()) + "년 " + string.Format("{0:#0}", cboXMonth.Text.To<int>()) + "개월";
            }
        }

        void eChkBoxChanged(object sender, EventArgs e)
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
        }

        void eTimerTick(object sender, EventArgs e)
        {
            //동의서 FTP전송
            long nSeq = 0;
            long nDrSabun = 0;
            string strFile = "";
            string strServer = "";
            string strOK = "";
            string strYear = "";
            string strOLD = "";
            string strNew = "";

            FileInfo Dir = null;

            //동의서 파일이 저장되었는지 점검
            strOK = "OK";
            strFile = "c:\\_spool\\Save\\" + FstrFilePath + "_" + FstrDrno + "\\" + FstrFileName;

            Dir = new FileInfo(strFile);

            if (Dir.Exists == false || strFile == "") { strOK = ""; }

            
            if (strOK == "") { timer1.Enabled = true; return; }

            //파일을 서버에 저장함
            nSeq = 0; strOLD = "";

            strFile = "c:\\_spool\\Save\\" + FstrFilePath + "_" + FstrDrno + "\\" + FstrFileName;
            

            strNew = FstrFilePath;
            if (strNew != strOLD)
            {
                strOLD = strNew;
                nSeq = 1;
            }
            else
            {
                nSeq = nSeq + 1;
            }
            strServer = VB.Format(FnWRTNO, "#0") + "_" + FstrDept + "_" + FstrFilePath + "_" + VB.Format(nSeq, "#0") + ".jpg";

            //Sleep 100

            strYear = VB.Left(clsPublic.GstrSysDate.Replace("-", ""), 4);
            Ftpedt FtpedtX = new Ftpedt();
            
            strFile = "c:\\_spool\\Save\\" + FstrFilePath + "_" + FstrDrno + "\\" + FstrFileName;
            

            Dir = new FileInfo(strFile);
            if (Dir.Exists == false)
            {
                strOK = ""; return;
            }

            //PC의 파일을 삭제함
            strFile = "c:\\_spool\\Save\\" + FstrFilePath + "_" + FstrDrno + "\\" + FstrFileName;                

            Dir = new FileInfo(strFile);
            if (Dir.Exists == true)
            {
                Dir.Delete();
            }
            
            timer1.Enabled = false;
        }

        /// <summary>
        /// 사용자 저장 로그(2020.11.12)
        /// 오픈 후 안정화 이후 로직 삭제 할것(table : ADMIN.HIC_PROGRAM_USE_CNT)
        /// </summary>
        void fn_UseCheck()
        {
            int result = 0;

            try
            {
                result = comHpcLibBService.InsertSangdamUseLog(clsType.User.IdNumber, this.Name, clsCompuInfo.gstrCOMIP, FstrPtno, FnWRTNO);

                if (result < 0)
                {
                    return;
                }
            }
            catch (Exception ex)
            {   
                MessageBox.Show(ex.Message + "\r\n프로그램 사용 이력 저장 중 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void fn_OILLS_INSERT()
        {
            int nEndo1 = 0;
            int nEndo2 = 0;
            int nEndo3 = 0;
            int nEndo4 = 0;

            string strPtNo = "";
            int nREAD = 0;
            string strJepDate = "";

            int result = 0;

            string strEGD1 = "";
            string strEGD2 = "";
            string strCFS1 = "";
            string strCFS2 = "";

            nEndo1 = 0;
            nEndo2 = 0;
            nEndo3 = 0;
            nEndo4 = 0;
            strPtNo = "";

            List<HIC_RESULT_EXCODE_JEPSU> list = hicResultExcodeJepsuService.GetItembyOnlyWrtNo(FnWRTNO);

            nREAD = list.Count;
            if (nREAD > 0)
            {
                for (int i = 0; i < nREAD; i++)
                {
                    strPtNo = list[i].PTNO;
                    strJepDate = list[i].JEPDATE;
                    if (list[i].ENDOGUBUN2 == "Y")
                    {
                        nEndo1 += 1;    //위내시경
                        strEGD1 = "1";
                    }
                    if (list[i].ENDOGUBUN3 == "Y")
                    {
                        nEndo2 += 1;    //위수면내시경
                        strEGD2 = "1";
                    }
                    if (list[i].ENDOGUBUN4 == "Y")
                    {
                        nEndo3 += 1;    //대장내시경
                        strCFS1 = "1";
                    }
                    if (list[i].ENDOGUBUN5 == "Y")
                    {
                        nEndo4 += 1;    //대장수면내시경
                        strCFS2 = "1";
                    }
                }
            }

            if (nEndo1 > 0 || nEndo2 > 0 || nEndo3 > 0 || nEndo4 > 0)
            {
                if (comHpcLibBService.GetCountOcsOillsbyPtnoBDate(strPtNo, strJepDate) == 0)
                {  
                    result = comHpcLibBService.InsertOcsOills(strPtNo, strJepDate, "HR", 1, "Z018");

                    if (result < 0)
                    {                 
                        FstrCOMMIT = "NO";
                        MessageBox.Show("외래 상병 발생중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            fn_ENDO_EMR_INSERT(FnWRTNO, strEGD1, strEGD2, strCFS1, strCFS2);
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
                if (list[i].GJJONG == "50" || list[i].GJJONG == "51")
                {
                    if (hicXMunjinService.GetJinGbnbyWrtNo(list[i].WRTNO).IsNullOrEmpty())
                    {
                        rtnVal = "";
                    }
                }
                else
                {
                    if (list[i].GBSTS != "Y")
                    {
                        rtnVal = "";
                    }
                }
            }

            return rtnVal;
        }

        void fn_JIN_GBN_UPDATE()
        {
            int result = 0;
            string strRowId = "";

            strRowId = hicJinGbnService.GetItembyWrtNo(long.Parse(txtWrtNo.Text));

            HIC_JIN_GBN item = new HIC_JIN_GBN();

            item.ROWID = strRowId;
            item.PANJENGDRNO = txtPanDrNo.Text.To<long>();

            if (!strRowId.IsNullOrEmpty())
            {
                result = hicJinGbnService.PanJengDrNoUpdate(item);
            }

            if (result < 0)
            {
                FstrCOMMIT = "NO";
            }

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

            //clsDB.setBeginTran(clsDB.DbCon);

            //다음 검사실이 없으면
            if (strNextRoom.IsNullOrEmpty())
            {
                result = hicSangdamWaitService.UpdateGbCallbyWrtNo(clsHcVariable.GstrDrRoom, FnWRTNO);

                if (result < 0)
                {
                    //clsDB.setRollbackTran(clsDB.DbCon);
                    FstrCOMMIT = "NO";
                    rtnVal = false;
                }
                else
                {   
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
                result = hicSangdamWaitService.DeletebyPaNo(FnPano);

                if (result < 0)
                {
                    FstrCOMMIT = "NO";
                    rtnVal = false;
                    return rtnVal;
                }
                FstrCOMMIT = "OK";
                rtnVal = true;
                return rtnVal;
            }

            nWait = hicSangdamWaitService.GetMaxWaitNobyGubun(string.Format("{0:00}", strRoom), "");

            //기존 등록된 대기순번을 삭제함
            result = hicSangdamWaitService.DeletebyPaNo(FnPano);

            if (result < 0)
            {
                FstrCOMMIT = "NO";
                rtnVal = false;
                return rtnVal;
            }

            List<HIC_JEPSU> list2 = hicJepsuService.GetItembyPaNo(FnPano);

            if (list2.Count > 0)
            {
                for (int i = 0; i < list2.Count; i++)
                {
                    nWRTNO = list2[i].WRTNO;
                    strGjJong = list2[i].GJJONG;
                    strSname = list2[i].SNAME;
                    strSex = list2[i].SEX;
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
                        FstrCOMMIT = "NO";
                        MessageBox.Show("상담대기 순번등록 중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        rtnVal = false;
                        return rtnVal;
                    }
                }
            }

            FstrCOMMIT = "OK";
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
            //string strGenStatusEtc = "";    //일반상태 기타
            //string strExtInjury = "";       //외상 기타
            string strSiksa = "";
            string strRemark = "";
            string strGbHabit = "";
            string strGbOldByeng = "";
            string stroldByengName = "";
            string[] strSick1 = new string[3];
            string[] strSick2 = new string[3];
            string[] strSick3 = new string[3];
            string[] strSick4 = new string[3];
            string[] strSick5 = new string[3];
            string[] strSick6 = new string[3];
            string[] strSick7 = new string[3];
            string[] strSick8 = new string[3];
            string[] strHabit = new string[6];
            string[] strSpcHabit = new string[6];
            string strSpcJinchal1 = "";
            string strSpcJinchal2 = "";
            string[] strOldByeng = new string[8];
            string[] strOLD = new string[15];
            string[] strDrug = new string[8];
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

            if (!hm.HIC_NEW_MUNITEM_INSERT(FnWRTNO, FstrJong, "", FstrUCodes).IsNullOrEmpty())
            {
                MessageBox.Show("문진 Table 생성 중 ERROR 발생!!", "상담불가", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                FstrCOMMIT = "NO";
                return;
            }

            //표적장기별 상담
            strPjSangdam = "";
            if (!FstrUCodes.IsNullOrEmpty())
            {
                for (int i = 0; i < SS9.ActiveSheet.RowCount; i++)
                {
                    strPjSangdam += SS9.ActiveSheet.Cells[i, 2].Text.Trim() + "{}";
                    strPjSangdam += SS9.ActiveSheet.Cells[i, 1].Text.Trim() + "{$}";
                }
            }

            //clsDB.setBeginTran(clsDB.DbCon);

            //GoSub INPUT_DATA_BUILD      'Data Check
            //진단여부 및 약물복용 여부
            strSick1[1] = SS_JinDan.ActiveSheet.Cells[0, 1].Text == "True" ? "1" : "2";
            strSick1[2] = SS_JinDan.ActiveSheet.Cells[0, 2].Text == "True" ? "1" : "2";

            strSick2[1] = SS_JinDan.ActiveSheet.Cells[1, 1].Text == "True" ? "1" : "2";
            strSick2[2] = SS_JinDan.ActiveSheet.Cells[1, 2].Text == "True" ? "1" : "2";

            strSick3[1] = SS_JinDan.ActiveSheet.Cells[2, 1].Text == "True" ? "1" : "2";
            strSick3[2] = SS_JinDan.ActiveSheet.Cells[2, 2].Text == "True" ? "1" : "2";

            strSick4[1] = SS_JinDan.ActiveSheet.Cells[3, 1].Text == "True" ? "1" : "2";
            strSick4[2] = SS_JinDan.ActiveSheet.Cells[3, 2].Text == "True" ? "1" : "2";

            strSick5[1] = SS_JinDan.ActiveSheet.Cells[4, 1].Text == "True" ? "1" : "2";
            strSick5[2] = SS_JinDan.ActiveSheet.Cells[4, 2].Text == "True" ? "1" : "2";

            strSick6[1] = SS_JinDan.ActiveSheet.Cells[7, 1].Text == "True" ? "1" : "2";
            strSick6[2] = SS_JinDan.ActiveSheet.Cells[7, 2].Text == "True" ? "1" : "2";

            strSick7[1] = SS_JinDan.ActiveSheet.Cells[5, 1].Text == "True" ? "1" : "2";
            strSick7[2] = SS_JinDan.ActiveSheet.Cells[5, 2].Text == "True" ? "1" : "2";

            strSick8[1] = SS_JinDan.ActiveSheet.Cells[6, 1].Text == "True" ? "1" : "2";
            strSick8[2] = SS_JinDan.ActiveSheet.Cells[6, 2].Text == "True" ? "1" : "2";

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
            //일반상태 기타
            //strGenStatusEtc = txtGenStatusEtc.Text;

            //외상 및 후유증
            if (rdoJinchal10.Checked == true)
            {
                strJinchal1 = "1";
            }

            if (rdoJinchal11.Checked == true)
            {
                strJinchal1 = "2";
            }

            //외상 기타
            //strExtInjury = txtExtInjury.Text;

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
            if (!stroldByengName.IsNullOrEmpty())
            {
                strOldByeng[6] = "1";
            }

            if (txtRemark.Text.IsNullOrEmpty())
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
            item.T_STAT01 = strSick1[1];
            item.T_STAT02 = strSick1[2];
            item.T_STAT11 = strSick2[1];
            item.T_STAT12 = strSick2[2];
            item.T_STAT21 = strSick3[1];
            item.T_STAT22 = strSick3[2];
            item.T_STAT31 = strSick4[1];
            item.T_STAT32 = strSick4[2];
            item.T_STAT41 = strSick5[1];
            item.T_STAT42 = strSick5[2];
            item.T_STAT51 = strSick6[1];
            item.T_STAT52 = strSick6[2];
            item.T_STAT52_TEC = stroldByengName;
            item.T_STAT61 = strSick7[1];
            item.T_STAT62 = strSick7[2];
            item.T_STAT71 = strSick8[1];
            item.T_STAT72 = strSick8[2];
            item.GBSIKSA = strSiksa;
            item.MUN_OLDMSYM = txtLastMedHis.Text.Trim();
            item.MUN_GAJOK = txtGajok.Text.Trim();
            item.MUN_GIINSUNG = txtGiinsung.Text.Trim();
            item.JIN_01 = txtJinChal0.Text.Trim();
            item.JIN_02 = txtJinChal1.Text.Trim();
            item.JIN_03 = txtJinChal2.Text.Trim();
            item.JIN_04 = txtJinChal3.Text.Trim();
            item.JENGSANG = VB.Pstr(txtJengSang.Text.Trim(), ".", 1);
            item.PJSANGDAM = strPjSangdam;
            item.REMARK = strRemark;
            item.SANGDAMDRNO = FnDrno;
            item.GBSTS = "Y";
            item.ENTSABUN = clsType.User.IdNumber.To<long>();
            item.WRTNO = FnWRTNO;

            result = hicSangdamNewService.UpdatebyWrtNo(item);

            if (result < 0)
            {
                //clsDB.setRollbackTran(clsDB.DbCon);
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
                //clsDB.setRollbackTran(clsDB.DbCon);
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
            item2.JENGSANG = VB.Pstr(txtJengSang.Text.Trim(), ".", 1);
            item2.JINDRNO = FnDrno;
            item2.WRTNO = FnWRTNO;

            result = hicResSpecialService.UpdateAllbyWrtNo(item2);

            if (result < 0)
            {
                //clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("상담내역 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }

            //GoSub UPDATE_NIGHT_RESULT   '야간작업 증상문진 저장
            //야간작업 대상이면
            if (tab6.Visible == true)
            {
                //if (cboInsomniaMun.Visible == true) //불면증증상문진
                if (FbInsomniaMun)
                {

                    if(cboInsomniaMun.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("불면증증상문진 값이 공백입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FstrCOMMIT = "NO";
                        return;
                    }

                    strExCode = "TZ72";
                    result = hicResultService.UpdateResultActivebyWrtNoExCode(FnWRTNO, cboInsomniaMun.Text.Trim(), clsType.User.IdNumber, strExCode);

                    if (result < 0)
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("상담내역 저장시 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FstrCOMMIT = "NO";
                        return;
                    }

                }

                //if (cboStomachMun.Visible == true)  //위장관계증상문진
                if (FbStomachMun)  //위장관계증상문진
                {
                    if (cboStomachMun.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("위장관계증상문진 값이 공백입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FstrCOMMIT = "NO";
                        return;
                    }

                    strExCode = "TZ85";
                    result = hicResultService.UpdateResultActivebyWrtNoExCode(FnWRTNO, cboStomachMun.Text.Trim(), clsType.User.IdNumber, strExCode);

                    if (result < 0)
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("상담내역 저장시 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FstrCOMMIT = "NO";
                        return;
                    }
                }

                //if (cboBreastCancerMun.Visible == true) //유방암증상문진
                if (FbBreastCancerMun) //유방암증상문진
                {
                    if (cboBreastCancerMun.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("유방암증상문진 값이 공백입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FstrCOMMIT = "NO";
                        return;
                    }

                    strExCode = "TZ86";
                    result = hicResultService.UpdateResultActivebyWrtNoExCode(FnWRTNO, cboBreastCancerMun.Text.Trim(), clsType.User.IdNumber, strExCode);

                    if (result < 0)
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("상담내역 저장시 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FstrCOMMIT = "NO";
                        return;
                    }
                }

                //if (cboinsomniaMun2.Visible == true) //불면증증상문진2차
                if (FbinsomniaMun2) //불면증증상문진2차
                {
                    if (cboinsomniaMun2.Text.IsNullOrEmpty())
                    {
                        MessageBox.Show("불면증증상문진2차 값이 공백입니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FstrCOMMIT = "NO";
                        return;
                    }

                    strExCode = "TZ87";
                    result = hicResultService.UpdateResultActivebyWrtNoExCode(FnWRTNO, VB.Pstr(cboinsomniaMun2.Text.Trim(), ".", 1), clsType.User.IdNumber, strExCode);

                    if (result < 0)
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("상담내역 저장시 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FstrCOMMIT = "NO";
                        return;
                    }
                }


                //위장관계증상문진,유방암증상문진 PANJENG UPDATE
                string strPan1 = "";
                string strPan2 = "";
                if (cboStomachMun.Text.Trim() == "정상") { strPan1 = "1"; }
                if (cboStomachMun.Text.Trim() == "비정상") { strPan1 = "2"; }
                if (FbBreastCancerMun)
                {
                    if (cboBreastCancerMun.Text.Trim() == "정상") { strPan2 = "1"; }
                    if (cboBreastCancerMun.Text.Trim() == "비정상") { strPan2 = "2"; }
                }
                HIC_MUNJIN_NIGHT itemSub01 = new HIC_MUNJIN_NIGHT();
                itemSub01.WRTNO = FnWRTNO;
                itemSub01.ITEM4_PANJENG = strPan1;
                itemSub01.ITEM5_PANJENG = strPan2;

                hicMunjinNightService.UpDate(itemSub01);

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
                    //clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("문진의사 면허번호 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FstrCOMMIT = "NO";
                    return;
                }
            }

            //clsDB.setCommitTran(clsDB.DbCon);
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
            string[] strSick = new string[3];
            string[] strHabit = new string[3];

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
            string[] strDiet1 = new string[4];
            string[] strDiet2 = new string[4];
            string[] strDiet3 = new string[3];
            string strDiet4 = "";
            //비만
            string strBimanCmb1 = "";
            string strBimanCmb2 = "";
            string[] strBiman = new string[7];
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

            //clsDB.setBeginTran(clsDB.DbCon);

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

            if (txtRemark.Text.IsNullOrEmpty())
            {
                strRemark = "특이사항 없음";
            }
            else
            {
                strRemark = txtRemark.Text.Trim();
            }

            if (!clsPublic.GstrMsgList.IsNullOrEmpty())
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
            item.ENTSABUN = clsType.User.IdNumber.To<long>();
            item.WRTNO       = FnWRTNO;

            result = hicSangdamNewService.UpdateDiabetesbyWrtNo(item);

            if (result < 0)
            {
                //clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("상담내역 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }

            //상담완료 SET이 제대로 들어갔는지 체크
            if (hicResBohum2Service.GetT_SangdambyWrtNo(FnWRTNO).IsNullOrEmpty())
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
                    //clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("상담내역 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FstrCOMMIT = "NO";
                    return;
                }
            }
            //clsDB.setCommitTran(clsDB.DbCon);
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
            string[] strOLD = new string[16];
            string[] strDrug = new string[9];
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
                    strSTS = (i + 1).To<string>();
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
            if (chkMedHis14.Checked == true) strOLD[14] = "1";
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
            if (strBigo.IsNullOrEmpty()) strBigo = "특이사항 없음";

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
                            break;
                        }
                    }

                    if (!txtMedHisEtc.Text.Trim().IsNullOrEmpty())
                    {
                        strOK = "OK";
                    }

                    if (strOK.IsNullOrEmpty())
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
                            break;
                        }
                    }
                    if (!txtMedHisEtc.Text.IsNullOrEmpty())
                    {
                        strOK = "";
                    }
                    if (strOK.IsNullOrEmpty())
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
                            break;
                        }
                    }
                    if (!txtMedcineEtc.Text.IsNullOrEmpty())
                    {
                        strOK = "OK";
                    }

                    if (strOK.IsNullOrEmpty())
                    {
                        strMSG += "복용중인 약물상태를 체크하십시오." + "\r\n";
                    }
                }
                else
                {
                    for (int i = 1; i <= 7; i++)
                    {
                        CheckBox chkMedcine = (Controls.Find("chkMedcine" + i.ToString(), true)[0] as CheckBox);
                        if (chkMedcine.Checked == true)
                        {
                            strOK = "";
                            break;
                        }
                    }
                    if (!txtMedcineEtc.Text.IsNullOrEmpty())
                    {
                        strOK = "";
                    }
                    if (strOK.IsNullOrEmpty())
                    {
                        strMSG += "복용중인 약물상태를 체크하십시오." + "\r\n";
                    }
                }

                strOK = "OK";
                if (chkPreTreatment0.Checked == false && chkPreTreatment1.Checked == false)
                {
                    strOK = "";
                }
                if (strOK.IsNullOrEmpty())
                {
                    strMSG += "전처치 약제를 체크하십시오." + "\r\n";
                }
            }

            if (!strMSG.IsNullOrEmpty())
            {
                MessageBox.Show(strMSG, "저장불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FstrCOMMIT = "NO";
                return;
            }

            //clsDB.setBeginTran(clsDB.DbCon);

            //GoSub Endo_Chart_Save
            if (!FstrEndoRowID.IsNullOrEmpty())
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

                result = endoChartService.UpdatebyRowId(item3);

                if (result < 0)
                {
                    //clsDB.setRollbackTran(clsDB.DbCon);
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
            if (strRemark.IsNullOrEmpty()) strRemark = "특이사항 없음";

            HIC_SANGDAM_NEW item = new HIC_SANGDAM_NEW();

            item.JINCHAL2 = strJinchal2;
            item.GBSIKSA = strSiksa;
            item.AMSANGDAM = strAmSangdam;
            item.REMARK = strRemark;
            item.SANGDAMDRNO = FnDrno;
            item.GBSTS = "Y";
            item.ENTSABUN = clsType.User.IdNumber.To<long>();
            item.WRTNO = FnWRTNO;

            result = hicSangdamNewService.UpdateJinchal2byWrtNo(item);

            if (result < 0)
            {
                //clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("상담내역 저장시 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }

            //clsDB.setCommitTran(clsDB.DbCon);

            //항정마약 승인(20211115)
            //fn_HIC_HYANG_Approve(FnWRTNO);
            //if (FstrCOMMIT == "NO")
            //{
            //    return;
            //}

            fn_ENDO_EMR_INSERT(FnWRTNO, strEGD1, strEGD2, strCFS1, strCFS2);
            if (FstrCOMMIT == "NO")
            {
                return;
            }
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
            string[] strSick = new string[3];
            string[] strHabit = new string[3];

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
            string[] strDiet1 = new string[4];
            string[] strDiet2 = new string[4];
            string[] strDiet3 = new string[3];
            string strDiet4 = "";
            //비만
            string strBimanCmb1  = "";
            string strBimanCmb2 = "";
            string[] strBiman = new string[7];
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

            //clsDB.setBeginTran(clsDB.DbCon);

            //판정테이블 생성
            if (strROWID.IsNullOrEmpty())
            {
                result = hicResBohum1Service.Insert(FnWRTNO);

                if (result < 0)
                {
                    //clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("판정 테이블 생성 시 오류 발생(HIC_RES_BOHUM1)", "전산실 연락요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    FstrCOMMIT = "NO";
                    return;
                }
            }

            //clsDB.setCommitTran(clsDB.DbCon);

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

            if (txtRemark.Text.IsNullOrEmpty())
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
            item.ENTSABUN = clsType.User.IdNumber.To<long>();
            item.WRTNO = FnWRTNO;

            result = hicSangdamNewService.UpdateDiabetesbyWrtNo(item);

            if (result < 0)
            {
                //clsDB.setRollbackTran(clsDB.DbCon);
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
            if (txtPanDrNo.Text.To<long>() == 0)
            {
                MessageBox.Show("판정의사 면허번호 누락", "확인요망");
                return;
            }
            if (cboPRes.Text.IsNullOrEmpty()) strFlag = "OK";
            if (cboEJ0.Text.IsNullOrEmpty()) strFlag = "OK";
            if (cboEJ1.Text.IsNullOrEmpty()) strFlag = "OK";
            if (cboM.Text.IsNullOrEmpty()) strFlag = "OK";
            if (cboN.Text.IsNullOrEmpty()) strFlag = "OK";
            if (cboS.Text.IsNullOrEmpty()) strFlag = "OK";
            if (cboHJ.Text.IsNullOrEmpty()) strFlag = "OK";
            if (cboJ.Text.IsNullOrEmpty()) strFlag = "OK";

            if (strFlag == "OK")
            {
                MessageBox.Show("판정항목 누락.", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }

            if (VB.Left(cboPRes.Text, 2).To<long>() > 9)
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
                    if (strPPanK[1].IsNullOrEmpty())
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

            if (txtRemark.Text.IsNullOrEmpty())
            {
                strRemark = "특이사항 없음";
            }
            else
            {
                strRemark = txtRemark.Text.Trim();
            }

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

            if (result < 0)
            {
                FstrCOMMIT = "NO";
                return;
            }

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
            item2.ENTSABUN = clsType.User.IdNumber.To<long>();
            item2.WRTNO = FnWRTNO;

            result = hicSangdamNewService.UpdatSchPanbyWrtNo(item2);

            if (result < 0)
            {
                FstrCOMMIT = "NO";
                return;
            }
        }

        /// <summary>
        ///방사선종사자 상담 저장 - XMunjin_CmdSave_Click
        /// </summary>
        void fn_Xmunjin_Save()
        {
            string strYN = "";
            string strGbn = "";
            string strJilByung = "";
            string[] strBlood = new string[3];
            string[] strSkin = new string[3];
            string strNerv1= "";
            string strNerv2= "";
            string strEye = "";
            string strEye_Etc = "";
            string strCancer = "";
            string strGajok = "";
            string[] strJikJong = new string[3];
            int result = 0;

            strYN = "";
            strGbn = "";

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

            if (txtRemark.Text.IsNullOrEmpty())
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

            strBlood[2] = txtX1_1.Text.Trim();                //혈액관련질환_기타
            strSkin[2] = txtX1_2.Text.Trim();                 //피부질환_기타
            strNerv1 = txtX1_3.Text.Trim();                   //신경계질환명
            strEye = chkX1_40.Checked == true ? "1" : "0";    //눈질환_백내장
            strEye_Etc = txtX1_4.Text.Trim();                 //눈질환_기타
            strCancer = txtX1_5.Text.Trim();                  //암_기타
            strJikJong[2] = txtJikjong.Text.Trim();           //현재직종

            strGajok = rdoX20.Checked == true ? "0" : "1";

            HIC_X_MUNJIN item = new HIC_X_MUNJIN();

            item.JINGBN = strGbn;
            item.XP1 = strYN;
            item.XPJONG = txtXJong.Text.Trim();
            item.XPLACE = txtPlace.Text.Trim();
            item.XREMARK = txtXRemark.Text.Trim();
            item.XMUCH = txtMuch.Text.Trim();
            item.XTERM = txtTerm.Text.Trim();
            if (txtXTerm.Text.IsNullOrEmpty())
            {
                txtXTerm.Text = string.Format("{0:#0}", cboXYear.Text) + "년 " + string.Format("{0:#0}", cboXMonth.Text) + "개월";
            }
            item.XTERM1 = txtXTerm.Text.Trim();
            item.XJUNGSAN = txtJung.Text.Trim();
            item.MUN1 = txtMun1.Text.Trim();
            item.JUNGSAN1 = txtEye.Text.Trim();
            item.JUNGSAN2 = txtSkin.Text.Trim();
            item.JUNGSAN3 = txtEtc.Text.Trim();
            item.SANGDAM = txtRemark.Text.Trim();
            item.MUNDRNO = txtPanDrNo.Text.To<long>();
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
                MessageBox.Show("자료를 등록중 오류가 발생함!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }

            HIC_SANGDAM_NEW item2 = new HIC_SANGDAM_NEW();

            item2.REMARK = txtRemark.Text.Trim();
            item2.SANGDAMDRNO = FnDrno;
            item2.GBSTS = "Y";
            item2.ENTSABUN = clsType.User.IdNumber.To<long>();
            item2.WRTNO = FnWRTNO;

            result = hicSangdamNewService.UpdateRemarkbyWrtNo(item2);

            if (result < 0)
            {
                MessageBox.Show("상담내역 저장시 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }
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

            if(strResult1.IsNullOrEmpty() || strResult2.IsNullOrEmpty())
            {
                MessageBox.Show("폐암사후상담 및 금연상담내역이 공백입니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }

            result = hicCancerNewService.UpdateLungSangdambyWrtNo(strResult1, strResult2, clsPublic.GstrSysDate, FnDrno, FnWRTNO);

            if (result < 0)
            {
                MessageBox.Show("폐암사후상담내역 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }

            result = hicJepsuService.UpdatePanjengDrNoPanjengDateTongboDatebyWrtNo(FnDrno, FnWRTNO);

            if (result < 0)
            {
                MessageBox.Show("폐암사후상담내역 저장시 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FstrCOMMIT = "NO";
                return;
            }
        }

        void fn_SANGDAM_ADD_UPDATE()
        {
            string strROWID = "";
            int result = 0;

            List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDatePtno(FstrJepDate, FstrPtno);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strROWID = list[i].RID;

                    result = hicJepsuService.UpdateSangdamDrnobyRowId(clsType.User.IdNumber, strROWID);

                    if (result < 0)
                    {
                        FstrCOMMIT = "NO";
                        MessageBox.Show("62,69종 상담의사 저장시 오류 발생(HIC_JEPSU)", "전산실 연락요망", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }                    
                }
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

            //clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                strCODE = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                strResult = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                strResCode = SS2.ActiveSheet.Cells[i, 5].Text.Trim();
                strChange = SS2.ActiveSheet.Cells[i, 6].Text.Trim();
                strROWID = SS2.ActiveSheet.Cells[i, 7].Text.Trim();

                if (!strResCode.IsNullOrEmpty())
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
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("검사결과 등록중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FstrCOMMIT = "NO";
                        return;
                    }

                    result = hicResultService.UpdateResultPanjengbyRowId(strResult, strNewPan, strResCode, clsType.User.IdNumber, strROWID);

                    if (result < 0)
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("검사결과 등록중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FstrCOMMIT = "NO";
                        return;
                    }
                }
            }
            //clsDB.setCommitTran(clsDB.DbCon);
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

            FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();

            btnPACS.Enabled = true;
            TabMain.Enabled = true;

            FstrExamFlag = "N";

            FnWRTNO = txtWrtNo.Text.To<long>();
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
            strGjJong = hb.READ_GJJONG_CODE(txtWrtNo.Text.To<long>());
            if (hicExjongService.GetGbSangdambyCode(strGjJong) != "Y")
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

            FstrJong = list.GJJONG;                                          //검진종류
            FstrChasu = list.GJCHASU;                                        //검진차수
            FstrPtno = list.PTNO;                                            //등록번호
            FstrSName = list.SNAME;
            FnPano = list.PANO;                                              //검진번호
            FnAge = (long)hb.READ_HIC_AGE_GESAN(clsAES.DeAES(list.JUMIN2));  //나이
            FstrSex = list.SEX;                                              //성별
            FstrJepDate = list.JEPDATE;                                      //접수일자
            FstrJumin = clsAES.DeAES(list.JUMIN2);                           //주민번호
            FstrYear = list.GJYEAR;                                          //검진년도
            FstrUCodes = list.UCODES;                                        //유해물질
            FstrChul = list.GBCHUL;                                          //출장여부

            //검진자 기본정보 표시---------------
            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PTNO;
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = FnAge + "/" + list.SEX;
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = hb.READ_Ltd_Name(list.LTDCODE.To<string>());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = list.JEPDATE;
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);
            //일특표시추가
            if (FstrJong  == "11" && !FstrUCodes.IsNullOrEmpty())
            {
                ssPatInfo.ActiveSheet.Cells[0, 5].Text = "일반+특수";
            }

            //2차일특여부 체크
            if (FstrChasu == "2" && (FstrJong == "16" || FstrJong == "17" || FstrJong == "19" || FstrJong == "45" || FstrJong == "44" || FstrJong == "46"))
            {
                //2차 검진자는 이전 1차 접수번호를 읽음(FnWRTNO2: 1차접수번호)
                fn_READ_First_Wrtno(FnWRTNO, FstrJong);

                if (FnWrtno2 > 0)
                {
                    FstrUCodes = hicJepsuService.GetUcodesbyWrtNo(FnWrtno2).Trim(); //유해물질
                }
            }

            if (!FstrUCodes.IsNullOrEmpty())
            {
                for (int i = 0; i < FstrUCodes.Length; i++)
                {
                    if (VB.Pstr(FstrUCodes, ",", i + 1).IsNullOrEmpty())
                    {
                        break;
                    }
                    else
                    {
                        lblSpc.Text += hm.UCode_Names_Display(VB.Pstr(FstrUCodes, ",", i + 1)) + ", ";
                    }
                }
            }

            if (!lblSpc.Text.IsNullOrEmpty())
            {
                lblSpc.Text = VB.Left(lblSpc.Text, lblSpc.Text.Length - 1);
            }

            if (FstrJong == "11" || FstrJong == "16" || FstrJong == "41" || FstrJong == "44")
            {
                if (!FstrUCodes.IsNullOrEmpty())
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

            if (FstrGjJong.IsNullOrEmpty())
            {
                FstrGjJong = list.GJJONG;
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
            if (hicSunapdtlService.GetCountbyWrtNoCode(FnWRTNO, sCode) > 0)
            {
                //탭
                tab1.Visible = true;
                tabControlPanel14.Visible = true;
                TabMain.SelectedTab = tab7;
                tab7.Visible = true;
                txtLung_Sang1.Text = fn_READ_LUNG_PAN(FnPano,FstrYear);
            }

            if (hicConsentService.GetCountbyWrtNoSDate(FnWRTNO , FstrJepDate) > 0)
            {
                btnMenuEndoConsent.Enabled = true;
            }

            //상담화면 세팅
            if (FstrJong == "31" || FstrJong == "35")
            {
                //탭
                //tab1.Visible = true;
                tab1.Visible = false;
                TabMain.SelectedTab = tab3;
                tab3.Visible = true;
                lblTitle0.Text = "신체증상";
                lblTitle1.Text = "암병력(본인,가족)";
                lblTitle2.Text = "위대장간질환";
                lblTitleBreast.Text = "유방자궁질환";

                lblStatus1.Visible = false;
                lblPTSD.Visible = false;
                lblMealState.Visible = false;
                lblLivingHabit.Visible = false;

                pnlBodySymptom.Visible = true;
                txtStomachBowlLiver.Visible = true;
                //pnlCancerHis.Visible = true;
                //pnlStomachBowlLiver.Visible = true;
                pnlChestCervical.Visible = true;

                txtBodySymptom.Text = "특이소견없음";
                txtBodySymptom.Visible = true;
                txtBodySymptom.BringToFront();
                txtCancerHis.Text = "특이소견없음";
                txtCancerHis.Visible = true;
                txtCancerHis.BringToFront();
                txtStomachBowlLiver.Text = "특이소견없음";
                txtStomachBowlLiver.Visible = true;
                txtStomachBowlLiver.BringToFront();
                txtChestCervical.Text = "특이소견없음";
                txtChestCervical.Visible = true;
                txtChestCervical.BringToFront();
                Application.DoEvents();
            }
            else if (FstrJong == "56" || FstrJong == "59")
            {
                //탭
                tab1.Visible = true;
                tabControlPanel14.Visible = true;
                TabMain.SelectedTab = tab4;
                tab4.Visible = true;
                lblStatus1.Visible = false;
                lblPTSD.Visible = false;
                lblMealState.Visible = false;
                lblLivingHabit.Visible = false;

                txtBodySymptom.Visible = false;
                txtCancerHis.Visible = false;
                txtStomachBowlLiver.Visible = false;
                txtChestCervical.Visible = false;
            }
            else if (FstrJong == "51" || FstrJong == "50")
            {
                //탭
                tab1.Visible = true;
                tabControlPanel14.Visible = true;
                TabMain.SelectedTab = tab5;
                tab5.Visible = true;
                tabControlX.SelectedTab = tabX2;
            }
            else
            {
                if (FstrChasu == "2")
                {
                    //탭
                    tab1.Visible = true;
                    tabControlPanel14.Visible = true;
                    TabMain.SelectedTab = tab2;
                    tab2.Visible = true;
                    lblMealState.Visible = false;
                }
                else
                {
                    //탭
                    tab1.Visible = true;
                    tabControlPanel14.Visible = true;
                    TabMain.SelectedTab = tab1;
                    if ((string.Compare(FstrJong, "11") >= 0 && string.Compare(FstrJong, "19") <= 0) || (string.Compare(FstrJong, "41") >= 0 && string.Compare(FstrJong, "46") <= 0))
                    {
                        lblGenral1.Visible = false;
                    }
                    if (!FstrUCodes.IsNullOrEmpty())
                    {
                        lblSpc1.Visible = false;
                        txtLastMedHis.Visible = true;
                        SS9.Visible = true;
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
                FstrROWID = list2.RID;
                strGbSTS = list2.GBSTS;
                txtRemark.Text = list2.REMARK;
                strPjSangdam = list2.PJSANGDAM;
            }
            else
            {
                //상담테이블 없을 시 상담테이블 생성함
                if (!fn_HIC_NEW_SANGDAM_INSERT(FnWRTNO, FstrJong).IsNullOrEmpty())                    
                {
                    MessageBox.Show("접수번호 " + FnWRTNO + " 신규상담항목 자동발생시 오류가 발생함..전산실에 연락바람..", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            List<string> sExCode = new List<string>();

            sExCode.Clear();
            sExCode.Add("A118");
            sExCode.Add("A129");
            sExCode.Add("A130");
            sExCode.Add("A143");
            sExCode.Add("A144");
            sExCode.Add("A145");
            sExCode.Add("A146");
            sExCode.Add("A147");

            //생활습관도구 처방전 TabVisible
            List<HIC_RESULT> list3 = hicResultService.GetResultbyWrtNoExCode(FnWRTNO, sExCode);

            SS3.ActiveSheet.RowCount = list3.Count;
            if (list3.Count > 0)
            {
                for (int i = 0; i < list3.Count; i++)
                {
                    SS3.ActiveSheet.Cells[i, 1].Text = list3[i].EXCODE;
                    SS3.ActiveSheet.Cells[i, 0].Text = "";
                    switch (list3[i].EXCODE)
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
            }

            btnLivingHabit.Enabled = false;
            if (SS3.ActiveSheet.RowCount > 0 && clsHcVariable.GnHicLicense > 0)
            {
                lblLivingHabit1.Visible = false;
                btnLivingHabit.Enabled = true;
                FstrTFlag = "Y";
                fn_LivingHabitReport(FnWRTNO);  //Call 생활습관도구표_READ(FnWRTNO)
            }

            //표적장기별 상담
            if (!FstrUCodes.IsNullOrEmpty())
            {
                fn_PJ_Display(FnWRTNO, strPjSangdam);
            }

            //GoSub Screen_Exam_Items_display     '검사항목을 Display
            sp.Spread_All_Clear(SS2);
            Application.DoEvents();

            TabMain.Refresh();

            List<HIC_RESULT_EXCODE> list4 = hicResultExCodeService.GetItemCounselbyWrtNo(FnWRTNO);

            nRead = list4.Count;
            SS2.ActiveSheet.RowCount = nRead;
            nRow = 0;
            for (int i = 0; i < nRead; i++)
            {
                strExCode = list4[i].EXCODE;
                strExCode = strExCode.Trim();
                strResult = list4[i].RESULT;
                strResCode = list4[i].RESCODE;
                strResultType = list4[i].RESULTTYPE;
                strGbCodeUse = list4[i].GBCODEUSE;

                strFlag = "";
                switch (strExCode)
                {
                    case "TI01":
                    case "TI02":
                    case "TR11":
                    case "TR13":
                    case "TH01":
                    case "TH02":
                    case "A151":
                    case "A153":
                    case "A294":
                    case "TZ01":
                    case "TZ12":
                    case "ZE76":
                    case "ZE77":
                        strFlag = "OK";
                        break;
                    default:
                        break;
                }

                SS2.ActiveSheet.Cells[i, 0].Text = strExCode;
                SS2.ActiveSheet.Cells[i, 1].Text = list4[i].HNAME;
                SS2.ActiveSheet.Cells[i, 2].Text = strResult;

                SS2.ActiveSheet.Cells[i, 2].CellType = txt;


                //2021-10-11
                //SS2.ActiveSheet.Cells[i, 2].Locked = true;
                HIC_BCODE item = hicBcodeService.Read_Hic_BCode("HIC_상담시결과입력대상코드", strExCode);
                if (item.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[i, 2].Locked = true;
                }
                else
                {
                    SS2.ActiveSheet.Cells[i, 2].Locked = false;
                }

                if (strFlag == "OK")
                {
                    ufn_Combo_Set(strResCode, i);
                }

                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[i, 2].Text += "." + hb.READ_ResultName(strResCode, strResult);
                    }
                }

                SS2.ActiveSheet.Cells[i, 3].Text = list4[i].PANJENG;

                //PFT 자동판정
                SS2.ActiveSheet.Cells[i, 6].Text = "";
                if (strExCode == "TR11" && strResult.IsNullOrEmpty())
                {
                    strResult = hm.PFT_Auto_Panjeng(FnWRTNO);
                    if (strResult == "01")
                    {
                        SS2.ActiveSheet.Cells[i, 2].Text = "01.정상";
                        SS2.ActiveSheet.Cells[i, 6].Text = "Y";
                    }
                }

                //이경검사는 자동으로 정상으로 처리
                if ((strExCode == "TI01" || strExCode == "TI02" || strExCode == "TZ12" || strExCode == "ZE76" || strExCode == "ZE77") && strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[i, 2].Text = "01.정상";
                    SS2.ActiveSheet.Cells[i, 6].Text = "Y";
                }

                if (strExCode == "TZ45" && strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[i, 2].Text = ".";
                    SS2.ActiveSheet.Cells[i, 6].Text = "Y";
                }

                if (strExCode == "TZ98" && strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[i, 2].Text = ".";
                    SS2.ActiveSheet.Cells[i, 6].Text = "Y";
                }

                //2020.12.16 10:20 전현준과장 요청 공백일때 . 자동입력
                //if (strCODE == "TZ98")
                //{
                //    if (strResult.IsNullOrEmpty())
                //    {
                //        strResult = ".";
                //        strChange = "Y";
                //    }
                //}


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

                //2020.10.21 결과 핸들링 불가
                if (list4[i].GBAUTOSEND == "Y")
                {
                    SS2.ActiveSheet.Cells[i, 2].Locked = true;
                }

                //참고치를 Dispaly
                strNomal = hm.EXAM_NomalValue_SET(strExCode, FstrJepDate, FstrSex, list4[i].MIN_M, list4[i].MAX_M, list4[i].MIN_F, list4[i].MAX_F);

                SS2.ActiveSheet.Cells[i, 4].Text = strNomal;
                SS2.ActiveSheet.Cells[i, 5].Text = strResCode;
                if (list4[i].EXCODE.Trim() == "A151")
                {
                    SS2.ActiveSheet.Cells[i, 5].Text = "007";
                }

                if (strExCode == "TH01" || strExCode == "TH02")
                {
                    SS2.ActiveSheet.Cells[i, 5].Text = "022";
                }
                SS2.ActiveSheet.Cells[i, 7].Text = list4[i].RID;
                SS2.ActiveSheet.Cells[i, 8].Text = list4[i].RESULTTYPE;

                strExPan = list4[i].PANJENG;
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
                if (!strResult.IsNullOrEmpty())
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
                if (strExCode == "TI01" || strExCode == "TI02")
                {
                    SS2.ActiveSheet.Cells[i, 9].Text = "1"; //이경검사
                }

                if (strExCode == "A151" || strExCode == "A153")
                {
                    SS2.ActiveSheet.Cells[i, 9].Text = "2"; //심전도검사
                }

                if (strExCode == "TR11")
                {
                    SS2.ActiveSheet.Cells[i, 9].Text = "3"; //폐활량
                }

                if (strExCode == "ZE76" || strExCode == "ZE77") SS2.ActiveSheet.Cells[i, 9].Text = "4"; //진동각,통각

                //2020.11.26 전현준 과장 요청 
                //Default 값
                //폐활량검사(TR11)   : 99.미실시
                //진동각(ZE76)       : 01.정상
                //통각(ZE77)         : 01.정상
                //손톱압박검사(TZ12) : 01.정상
                if (strExCode == "TR11" && strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[i, 2].Text = "99.미실시";
                    SS2.ActiveSheet.Cells[i, 6].Text = "Y";
                }

                if (strExCode == "ZE76" && strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[i, 2].Text = "01.정상";
                    SS2.ActiveSheet.Cells[i, 6].Text = "Y";
                }

                if (strExCode == "ZE77" && strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[i, 2].Text = "01.정상";
                    SS2.ActiveSheet.Cells[i, 6].Text = "Y";
                }

                if (strExCode == "TZ12" && strResult.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[i, 2].Text = "01.정상";
                    SS2.ActiveSheet.Cells[i, 6].Text = "Y";
                }

                Application.DoEvents();
            }

            #region 강제 Sort
            //검사결과의 판정값이 R,B인것을 위에 표시 
            FarPoint.Win.Spread.SortInfo[] si = new FarPoint.Win.Spread.SortInfo[] {
               new FarPoint.Win.Spread.SortInfo(9,true)
            ,  new FarPoint.Win.Spread.SortInfo(3,false)
            ,  new FarPoint.Win.Spread.SortInfo(0,true)};

            SS2_Sheet1.SortRows(0, SS2_Sheet1.RowCount, si);
            #endregion

            fn_Screen_SangDam_display2(FnWRTNO); //2차 상담내역 DISPLAY

            Application.DoEvents();

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
                    txtLung_Sang1.Text = list6.LUNG_SANGDAM1;
                    txtLung_Sang2.Text = list6.LUNG_SANGDAM2;
                }
            }

            if (txtPanDrNo.Text.To<long>() == 0)
            {
                txtPanDrNo.Text = clsHcVariable.GnHicLicense.To<string>();
            }

            //상담한 의사만 수정이 가능함
            if (txtPanDrNo.Text.To<long>() != clsHcVariable.GnHicLicense)
            {
                btnSave.Enabled = false;
                //강제 재판정 수검자는 수정이 가능함
                if (hm.RePanjeng_WRTNO_Check(FnWRTNO) == true)
                {
                    btnSave.Enabled = true;
                }
            }
            else
            {
                btnSave.Enabled = true;
            }

            lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());

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

            fn_Genjin_History_SET();    //검진 HISTORY

            //문진뷰어
            if (clsPublic.GstrRetValue != "1")
            {
                if (chkMunjin.Checked == false)
                {
                    //검진문진뷰어
                    //DirectoryInfo dir = new DirectoryInfo(clsHcVariable.Hic_Mun_EXE_PATH);
                    DirectoryInfo dir = new DirectoryInfo(@"C:\Program Files\SamOmr\");
                    if (dir.Exists == true)
                    {
                        //Process.Start(clsHcVariable.Hic_Mun_EXE_PATH, FstrPtno); 
                        VB.Shell(clsHcVariable.Hic_Mun_EXE_PATH + " " + FstrPtno, "NormalFocus");
                    }
                    else
                    {
                        //DirectoryInfo dir1 = new DirectoryInfo(clsHcVariable.Hic_Mun_EXE_PATH_64);
                        DirectoryInfo dir1 = new DirectoryInfo(@"C:\Program Files (x86)\SamOmr\");
                        if (dir1.Exists == true)
                        {
                            //Process.Start(clsHcVariable.Hic_Mun_EXE_PATH_64, FstrPtno);
                            VB.Shell(clsHcVariable.Hic_Mun_EXE_PATH_64 + " " + FstrPtno, "NormalFocus");
                        }
                    }


                    if(Screen.AllScreens.Length > 1)
                    {
                        //int nMain = Screen.AllScreens[0].Primary == true ? 1 : 0;     //주   모니터
                        //int nSub1 = Screen.AllScreens[1].Primary == true ? 1 : 0;     //서브 모니터
                        //int nSub2 = Screen.AllScreens[2].Primary == true ? 1 : 0;     //서브 모니터

                        //int screenWidth1 = Screen.AllScreens[nSub1].Bounds.Width;      //서브 모니터1 화면 넓이
                        //int screenHeight1 = Screen.AllScreens[nSub1].Bounds.Height;    //서브 모니터1 화면 높이
                        //int screenWidth2 = Screen.AllScreens[nSub1].Bounds.Width;      //서브 모니터2 화면 넓이
                        //int screenHeight2 = Screen.AllScreens[nSub1].Bounds.Height;    //서브 모니터2 화면 높이

                    }



                    //인터넷문진표(New)
                    Form frm = cHF.OpenForm_Check_Return("frmHcSangInternetMunjinView");
                    if (!frm.IsNullOrEmpty())
                    {
                        
                        frm.Dispose();
                    }
                    
                    //if (cHF.OpenForm_Check("frmHcSangInternetMunjinView") == true)
                    //{
                    //    FrmHcSangInternetMunjinView.Close();
                    //    FrmHcSangInternetMunjinView.Dispose();
                    //    FrmHcSangInternetMunjinView = null;
                    //}
                    FrmHcSangInternetMunjinView = new frmHcSangInternetMunjinView(FnWRTNO, FstrJepDate, FstrPtno, FstrGjJong, FstrROWID, fnLicense);
                    FrmHcSangInternetMunjinView.Show();
                    FrmHcSangInternetMunjinView.WindowState = FormWindowState.Minimized;
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

            List<string> strGubun = new List<string>();

            strGubun.Clear();
            strGubun.Add("15");
            strGubun.Add("16");
            strGubun.Add("17");
            strGubun.Add("18");
            strGubun.Add("19");

            List<HIC_SANGDAM_WAIT> list7 = hicSangdamWaitService.GetNextRoombyWrtNoInGubun(FnWRTNO, strGubun);

            if (list7.Count >= 1)
            {
                strNextRoom = list7[0].NEXTROOM;
                if (!strNextRoom.IsNullOrEmpty())
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

            for (int i = 0; i < TabMain.Tabs.Count; i++)
            {
                if (TabMain.Tabs[i].Visible == true)
                {
                    TabMain.SelectedTabIndex = i;
                    break;
                }
            }

            Application.DoEvents();

            clsPublic.GstrRetValue = "";
        }

        void ufn_Combo_Set(string strResCode, int nRow)
        {
            string strList = "";

            FarPoint.Win.Spread.CellType.ComboBoxCellType combo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();

            //자료를 READ
            List<HIC_RESCODE> ResCodeList = hicRescodeService.Read_ResCode(strResCode);

            string[] strComboCode = new string[ResCodeList.Count];

            for (int i = 0; i < ResCodeList.Count; i++)
            {
                strComboCode[i] = ResCodeList[i].CODENAME;
            }

            if (ResCodeList.Count > 1)
            {
                combo.Items = strComboCode;
                combo.AutoSearch = FarPoint.Win.AutoSearch.SingleCharacter;
                combo.MaxDrop = ResCodeList.Count;
                combo.MaxLength = 150;
                combo.ListWidth = 200;
                combo.Editable = true;
                SS2.ActiveSheet.Cells[nRow, 2].CellType = combo;
                SS2.ActiveSheet.Cells[nRow, 2].Locked = false;
            }
            else
            {
                SS2.ActiveSheet.Cells[nRow, 2].CellType = txt;
                SS2.ActiveSheet.Cells[nRow, 2].Locked = false;
            }
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

            //2021-04-19
            string strDAT4 = "";
            string strDAT5 = "";

            List<string> strExCode = new List<string>();

            //야간작업 문진표 대상자인지 점검
            if (hicSunapdtlService.GetCountbyWrtNOInCode(argWrtNo, clsHcVariable.G36_NIGHT_CODE) == 0)
            {
                //탭
                tab6.Visible = false;
                tabNight.Visible = false;
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
            TabMain.SelectedTab = tab6;
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
                tabNgt2.Visible = true;
                tabNgt3.Visible = false;
                tabNgt4.Visible = false;
                tabNgt5.Visible = false;
                tabNight.SelectedTab = tabNgt1;
            }
            else
            {
                tabNgt1.Visible = false;
                tabNgt2.Visible = false;
                tabNgt3.Visible = false;
                tabNgt4.Visible = true;
                tabNgt5.Visible = true;
                tabNight.SelectedTab = tabNgt4;
            }
            
            if (strChasu == "1")
            {
                strDAT1 = list.ITEM1_DATA;
                nJemsu1 = list.ITEM1_JEMSU;
                strPAN1 = list.ITEM1_PANJENG;
                for (int i = 0; i <= 6; i++)
                {
                    strChar = VB.Mid(strDAT1, i + 1, 1);
                    SS10.ActiveSheet.Cells[i, 1].Text = strChar;
                    if (string.Compare(strChar, "0") > 0)
                    {
                        SS10.ActiveSheet.Cells[i, 2].Text = (strChar.To<long>() - 1).To<string>();
                        SS10.ActiveSheet.Cells[i, 3].Text = hf.Munjin_Night_Value1(i + 1, strChar);
                    }
                }
                txtSum1.Text = nJemsu1.To<string>();
                lblPan1.Text = hf.Munjin_Night_Panjeng("1", strPAN1);

                //위장관질환
                strDAT4 = list.ITEM4_DATA;
                for (int i = 0; i <= 5; i++)
                {
                    strChar = VB.Mid(strDAT4, i + 1, 1);
                    SS13.ActiveSheet.Cells[i, 1].Text = strChar;
                    if (string.Compare(strChar, "0") > 0)
                    {
                        SS13.ActiveSheet.Cells[i, 2].Text = hf.Munjin_Night_Value4(i + 1, strChar);
                    }
                }

                //유방암
                strDAT5 = list.ITEM5_DATA;
                for (int i = 0; i <= 5; i++)
                {
                    strChar = VB.Mid(strDAT5, i + 1, 1);
                    SS14.ActiveSheet.Cells[i, 1].Text = strChar;
                    if (string.Compare(strChar, "0") > 0)
                    {
                        SS14.ActiveSheet.Cells[i, 2].Text = hf.Munjin_Night_Value5(i + 1, strChar);
                    }
                }



            }
            else
            {
                //수면의 질
                strDAT2 = list.ITEM2_DATA;
                nJemsu2 = list.ITEM2_JEMSU;
                strPAN2 = list.ITEM2_PANJENG;
                for (int i = 0; i < 18; i++)
                {
                    strChar = VB.Pstr(strDAT2, ",", i + 1);
                    SS11.ActiveSheet.Cells[i, 1].Text = strChar;
                    if (string.Compare(strChar, "0") > 0)
                    {
                        SS11.ActiveSheet.Cells[i, 2].Text = (strChar.To<long>() - 1).To<string>();
                        SS11.ActiveSheet.Cells[i, 3].Text = hf.Munjin_Night_Value2(i + 1, strChar);
                    }
                }
                txtSum2.Text = nJemsu2.To<string>();
                lblPan2.Text = hf.Munjin_Night_Panjeng("2", strPAN2);
                //주간졸림증
                strDAT3 = list.ITEM3_DATA;
                nJemsu3 = list.ITEM3_JEMSU;
                strPAN3 = list.ITEM3_PANJENG;
                for (int i = 0; i <= 7; i++)
                {
                    strChar = VB.Mid(strDAT3, i+1, 1);
                    SS12.ActiveSheet.Cells[i, 1].Text = strChar;
                    if (string.Compare(strChar, "0") > 0)
                    {
                        SS12.ActiveSheet.Cells[i, 2].Text = (strChar.To<long>() - 1).To<string>();
                        SS12.ActiveSheet.Cells[i, 3].Text = hf.Munjin_Night_Value3(i+1, strChar);
                    }
                }
                txtSum3.Text = nJemsu3.To<string>();
                lblPan3.Text = hf.Munjin_Night_Panjeng("3", strPAN3);
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

            FbBreastCancerMun = false;
            FbInsomniaMun = false;
            FbinsomniaMun2 = false;
            FbStomachMun = false;

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
                    FbInsomniaMun = true;
                    cboInsomniaMun.SelectedIndex = 0;
                    if (list2[i].RESULT == "정상")
                    {
                        cboInsomniaMun.SelectedIndex = 1;
                    }
                    else if (list2[i].RESULT == "비정상")
                    {
                        cboInsomniaMun.SelectedIndex = 2;
                    }
                }
                if (list2[i].EXCODE.Trim() == "TZ85")
                {
                    lblStomachSymptom.Visible = true;
                    cboStomachMun.Visible = true;
                    FbStomachMun = true;
                    cboStomachMun.SelectedIndex = 0;
                    if (list2[i].RESULT == "정상")
                    {
                        cboStomachMun.SelectedIndex = 1;
                    }
                    else if (list2[i].RESULT == "비정상")
                    {
                        cboStomachMun.SelectedIndex = 2;
                    }
                }
                if (list2[i].EXCODE.Trim() == "TZ86")
                {
                    lblChestCancelMun.Visible = true;
                    cboBreastCancerMun.Visible = true;
                    tabNgt3.Visible = true;
                    FbBreastCancerMun = true;
                    cboBreastCancerMun.SelectedIndex = 0;
                    if (list2[i].RESULT == "정상")
                    {
                        cboBreastCancerMun.SelectedIndex = 1;
                    }
                    else if (list2[i].RESULT == "비정상")
                    {
                        cboBreastCancerMun.SelectedIndex = 2;
                    }
                }
                if (list2[i].EXCODE.Trim() == "TZ87")
                {
                    lblinsomniaMun2.Visible = true;
                    cboinsomniaMun2.Visible = true;
                    FbinsomniaMun2 = true;
                    cboinsomniaMun2.SelectedIndex = 0;
                    for (int j = 0; j < cboinsomniaMun2.Items.Count; j++)
                    {
                        if (VB.Left(cboinsomniaMun2.Items[j].To<string>(), 2) == list2[i].RESULT)
                        {
                            cboinsomniaMun2.SelectedIndex = j;
                            break;
                        }
                    }
                }

                Application.DoEvents();
            }

            strSANGOK = "";

            if (hicSangdamNewService.GetCountbyWrtNoSangdamDrNo(argWrtNo) == 0)
            {
                strSANGOK = "OK";
            }

            if (strSANGOK == "OK")
            {
                //불면증, 위장관계, 유방암 증상문진
                strOMR = "";
                strTemp = "";

                cboInsomniaMun.SelectedIndex = 1;
                cboStomachMun.SelectedIndex = 1;
                cboBreastCancerMun.SelectedIndex = 1;
                HIC_MUNJIN_NIGHT item = hicMunjinNightService.GetAllbyWrtNo(argWrtNo);

                if (!item.IsNullOrEmpty())
                {
                    //수면장애
                    if (nJemsu1 >= 15)
                    {
                        cboInsomniaMun.SelectedIndex = 2;
                    }

                    //위장관질환 문진표 항목(1,2번)
                    if ((VB.Mid(strDAT4,1,1) =="6" || VB.Mid(strDAT4, 1, 1) == "7") && VB.Mid(strDAT4, 2, 1) == "2")
                    {
                        cboStomachMun.SelectedIndex = 2;
                    }

                    //위장관질환 문진표 항목(3,4번)
                    if ((VB.Mid(strDAT4, 3, 1) == "6" || VB.Mid(strDAT4, 3, 1) == "7") && VB.Mid(strDAT4, 4, 1) == "2")
                    {
                        cboStomachMun.SelectedIndex = 2;
                    }

                    //위장관질환 문진표 항목(5,6번)
                    if ((VB.Mid(strDAT4, 5, 1) == "5" || VB.Mid(strDAT4, 5, 1) == "6" || VB.Mid(strDAT4, 5, 1) == "7") && VB.Mid(strDAT4, 6, 1) == "2")
                    {
                        cboStomachMun.SelectedIndex = 2;
                    }

                    //위장관질환 문진표 항목(5,6번)
                    if (VB.Mid(strDAT5, 2, 1) == "2" || VB.Mid(strDAT5, 3, 1) == "2" || VB.Mid(strDAT5, 4, 1) == "2")
                    {
                        cboBreastCancerMun.SelectedIndex = 2;
                    }
                }

                //HIC_IE_MUNJIN_NEW list3 = hicIeMunjinNewService.GetMunjinResbyWrtNo1(argWrtNo);

                //if (!list3.IsNullOrEmpty())
                //{
                //    //if (string.Compare(txtSum1.Text, "15") >= 0)
                //    if (Convert.ToInt32(txtSum1.Text) >= 15)
                //    {
                //        cboInsomniaMun.SelectedIndex = 2;
                //    }

                //    strOMR = VB.Pstr(VB.Pstr(VB.Pstr(list3.MUNJINRES.Trim(), "{<*>}tbl_night{*}", 2), "{<*>}", 1), "{*}", 2);

                //    //위장관질환 문진표 항목(1,2번)
                //    if ((VB.Pstr(VB.Pstr(strOMR, "{}", 22), ",", 2) == "6" || VB.Pstr(VB.Pstr(strOMR, "{}", 22), ",", 2) == "7") &&
                //        VB.Pstr(VB.Pstr(strOMR, "{}", 23), ",", 2) == "2")
                //    {
                //        cboStomachMun.SelectedIndex = 2;
                //    }
                //    //위장관질환 문진표 항목(3,4번)
                //    if ((VB.Pstr(VB.Pstr(strOMR, "{}", 24), ",", 2) == "6" || VB.Pstr(VB.Pstr(strOMR, "{}", 24), ",", 2) == "7") &&
                //        VB.Pstr(VB.Pstr(strOMR, "{}", 25), ",", 2) == "2")
                //    {
                //        cboStomachMun.SelectedIndex = 2;
                //    }
                //    //위장관질환 문진표 항목(5,6번)
                //    if ((VB.Pstr(VB.Pstr(strOMR, "{}", 26), ",", 2) == "5" || VB.Pstr(VB.Pstr(strOMR, "{}", 26), ",", 2) == "6" ||
                //        VB.Pstr(VB.Pstr(strOMR, "{}", 26), ",", 2) == "7") && VB.Pstr(VB.Pstr(strOMR, "{}", 27), ",", 2) == "2")
                //    {
                //        cboStomachMun.SelectedIndex = 2;
                //    }
                //    //유방암문진표 항목(2번)
                //    strTemp = VB.Pstr(VB.Pstr(strOMR, "{}", 29), ",,", 2);
                //    if (VB.Pstr(strTemp, "|", 1) == "Y" || VB.Pstr(strTemp, "|", 2) == "Y" || VB.Pstr(strTemp, "|", 3) == "Y")
                //    {
                //        cboBreastCancerMun.SelectedIndex = 2;
                //    }
                //}
            }
        }

        void fn_Genjin_History_SET()
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
                SSHistory.ActiveSheet.Cells[i, 2].Text = list[i].WRTNO.To<string>();
                SSHistory.ActiveSheet.Cells[i, 3].Text = list[i].GJCHASU;
                SSHistory.ActiveSheet.Cells[i, 4].Text = list[i].GJJONG.Trim();                
            }
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

            if (!FstrUCodes.IsNullOrEmpty())
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
                        if (!hm.HIC_NEW_MUNITEM_INSERT(argWrtNo, strJong, "", "").IsNullOrEmpty())
                        {
                            MessageBox.Show("문진내역이 없습니다(상담불가)", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    break;
                case "3":   //특수문진
                    if (hicResSpecialService.GetCountbyWrtNo(argWrtNo) == 0)
                    {
                        if (!hm.HIC_NEW_MUNITEM_INSERT(argWrtNo, strJong, "", "").IsNullOrEmpty())
                        {
                            MessageBox.Show("문진내역이 없습니다(상담불가)", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    break;
                case "91":
                    strRowID1 = hicResBohum1Service.GetRowIdbyWrtNo(argWrtNo).To<string>();   //일반
                    strRowid2 = hicResSpecialService.GetRowIdbyWrtNo(argWrtNo).To<string>();  //특수
                    if (strRowID1.IsNullOrEmpty())
                    {
                        if (!hm.HIC_NEW_MUNITEM_INSERT(argWrtNo, strJong, "", "").IsNullOrEmpty())
                        {
                            MessageBox.Show("문진내역이 없습니다(상담불가)", "확인요망", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    if (strRowid2.IsNullOrEmpty())
                    {
                        if (!hm.HIC_NEW_MUNITEM_INSERT(argWrtNo, strJong, "", "").IsNullOrEmpty())
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
                if (txtLastMedHis.Text.IsNullOrEmpty()) txtLastMedHis.Text = "특이병력 없음";
                if (txtGajok.Text.IsNullOrEmpty()) txtGajok.Text = "특이병력 없음";
                if (txtGiinsung.Text.IsNullOrEmpty()) txtGiinsung.Text = "특이사항 없음";
                if (txtJinChal0.Text.IsNullOrEmpty()) txtJinChal0.Text = "특이사항 없음";
                if (txtJinChal1.Text.IsNullOrEmpty()) txtJinChal1.Text = "특이사항 없음";
                if (txtJinChal2.Text.IsNullOrEmpty()) txtJinChal2.Text = "특이사항 없음";
                if (txtJinChal3.Text.IsNullOrEmpty()) txtJinChal3.Text = "특이사항 없음";
                if (txtJengSang.Text.IsNullOrEmpty())
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

                if (!list.IsNullOrEmpty())
                {
                    //현재상태 및 약물복용 여부
                    if (list.T_STAT01 == "1") SS_JinDan.ActiveSheet.Cells[0, 1].Text = "True";  //뇌졸중 진단여부
                    if (list.T_STAT02 == "1") SS_JinDan.ActiveSheet.Cells[0, 2].Text = "True";  //뇌졸중 약물여부

                    if (list.T_STAT11 == "1") SS_JinDan.ActiveSheet.Cells[1, 1].Text = "True";  //심장병 진단여부
                    if (list.T_STAT12 == "1") SS_JinDan.ActiveSheet.Cells[1, 2].Text = "True";  //심장병 약물여부

                    if (list.T_STAT21 == "1") SS_JinDan.ActiveSheet.Cells[2, 1].Text = "True";  //고혈압 진단여부
                    if (list.T_STAT22 == "1") SS_JinDan.ActiveSheet.Cells[2, 2].Text = "True";  //고혈압 약물여부

                    if (list.T_STAT31 == "1") SS_JinDan.ActiveSheet.Cells[3, 1].Text = "True";  //당뇨병 진단여부
                    if (list.T_STAT32 == "1") SS_JinDan.ActiveSheet.Cells[3, 2].Text = "True";  //당뇨병 약물여부

                    if (list.T_STAT41 == "1") SS_JinDan.ActiveSheet.Cells[4, 1].Text = "True";  //고지혈증 진단여부
                    if (list.T_STAT42 == "1") SS_JinDan.ActiveSheet.Cells[4, 2].Text = "True";  //고지혈증 약물 여부

                    if (list.T_STAT51 == "1") SS_JinDan.ActiveSheet.Cells[7, 1].Text = "True";  //기타 진단여부
                    if (list.T_STAT52 == "1") SS_JinDan.ActiveSheet.Cells[7, 2].Text = "True";  //기타 약물여부

                    if (list.T_STAT71 == "1") SS_JinDan.ActiveSheet.Cells[5, 1].Text = "True";  //폐결핵 진단여부
                    if (list.T_STAT72 == "1") SS_JinDan.ActiveSheet.Cells[5, 2].Text = "True";  //폐결핵 약물여부

                    if (list.T_STAT61 == "1") SS_JinDan.ActiveSheet.Cells[6, 1].Text = "True";  //간장질환 진단여부
                    if (list.T_STAT62 == "1") SS_JinDan.ActiveSheet.Cells[6, 2].Text = "True";  //간장질환 약물여부

                    txtOldByengName.Text = list.OLDBYENGNAME;    //기타병명

                    if (list.JINCHAL2 == "1") rdoJinchal20.Checked = true;
                    if (list.JINCHAL2 == "2") rdoJinchal21.Checked = true;
                    if (list.JINCHAL2 == "3") rdoJinchal22.Checked = true;

                    if (list.JINCHAL1 == "1") rdoJinchal10.Checked = true;
                    if (list.JINCHAL1 == "2") rdoJinchal11.Checked = true;

                    if (list.GBSIKSA == "Y")
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
                        str_T40_Feel[0] = list.T40_FEEL3;
                        str_T40_Feel[1] = list.T40_FEEL4;
                        str_T66_Feel[0] = list.T66_FEEL1;
                        str_T66_Feel[1] = list.T66_FEEL2;
                        str_T66_Feel[2] = list.T66_FEEL3;
                        str_T66_Memory[0] = list.T66_MEMORY1;
                        str_T66_Memory[1] = list.T66_MEMORY2;
                        str_T66_Memory[2] = list.T66_MEMORY3;
                        str_T66_Memory[3] = list.T66_MEMORY4;
                        str_T66_Memory[4] = list.T66_MEMORY5;
                    }

                    txtRemark.Text = list.SANGDAM;

                    for (int i = 0; i <= 7; i++)
                    {
                        strJilByung[i] = SS_JinDan.ActiveSheet.Cells[i, 2].Text.Trim();
                    }

                    if (strJilByung[0] == "True") txtRemark.Text += "\r\n" + "뇌졸중 약물치료중()년";
                    if (strJilByung[1] == "True") txtRemark.Text += "\r\n" + "심장병 약물치료중()년";
                    if (strJilByung[2] == "True") txtRemark.Text += "\r\n" + "고혈압 약물치료중()년";
                    if (strJilByung[3] == "True") txtRemark.Text += "\r\n" + "당뇨병 약물치료중()년";
                    if (strJilByung[4] == "True") txtRemark.Text += "\r\n" + "이상지질혈증 약물치료중()년";
                    if (strJilByung[5] == "True") txtRemark.Text += "\r\n" + "간장질환 약물치료중()년";
                    if (strJilByung[6] == "True") txtRemark.Text += "\r\n" + "폐결핵 약물치료중()년";
                    if (strJilByung[7] == "True") txtRemark.Text += "\r\n" + "기타 약물치료중()년";
                }
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
            if (list.GBSIKSA == "Y")
            {
                rdoMealState0.Checked = true;
            }
            else
            {
                rdoMealState1.Checked = true;
            }

            //뇌졸중
            if (list.T_STAT01 == "1") SS_JinDan.ActiveSheet.Cells[0, 1].Text = "True";
            if (list.T_STAT02 == "1") SS_JinDan.ActiveSheet.Cells[0, 2].Text = "True";
            //심장병
            if (list.T_STAT11 == "1") SS_JinDan.ActiveSheet.Cells[1, 1].Text = "True";
            if (list.T_STAT12 == "1") SS_JinDan.ActiveSheet.Cells[1, 2].Text = "True";
            //고혈압
            if (list.T_STAT21 == "1") SS_JinDan.ActiveSheet.Cells[2, 1].Text = "True";
            if (list.T_STAT22 == "1") SS_JinDan.ActiveSheet.Cells[2, 2].Text = "True";
            //당뇨
            if (list.T_STAT31 == "1") SS_JinDan.ActiveSheet.Cells[3, 1].Text = "True";
            if (list.T_STAT32 == "1") SS_JinDan.ActiveSheet.Cells[3, 2].Text = "True";
            //이상지질혈증
            if (list.T_STAT41 == "1") SS_JinDan.ActiveSheet.Cells[4, 1].Text = "True";
            if (list.T_STAT42 == "1") SS_JinDan.ActiveSheet.Cells[4, 2].Text = "True";            
            //간장질환
            if (list.T_STAT61 == "1") SS_JinDan.ActiveSheet.Cells[5, 1].Text = "True";
            if (list.T_STAT62 == "1") SS_JinDan.ActiveSheet.Cells[5, 2].Text = "True";
            //폐결핵
            if (list.T_STAT71 == "1") SS_JinDan.ActiveSheet.Cells[6, 1].Text = "True";
            if (list.T_STAT72 == "1") SS_JinDan.ActiveSheet.Cells[6, 2].Text = "True";
            //기타질환
            if (list.T_STAT51 == "1") SS_JinDan.ActiveSheet.Cells[7, 1].Text = "True";
            if (list.T_STAT52 == "1") SS_JinDan.ActiveSheet.Cells[7, 2].Text = "True";

            //생활습관 개선필요
            if (list.HABIT1 == "1")
            {
                chkHabit0.Checked = true;
            }
            if (list.HABIT2 == "1")
            {
                chkHabit1.Checked = true;
            }
            if (list.HABIT3 == "1")
            {
                chkHabit2.Checked = true;
            }
            if (list.HABIT4 == "1")
            {
                chkHabit3.Checked = true;
            }
            if (list.HABIT5 == "1")
            {
                chkHabit4.Checked = true;
            }

            txtOldByengName.Text = list.T_STAT52_TEC;    //기타병명
            txtRemark.Text = list.REMARK;                //상담내역

            txtLastMedHis.Text = list.MUN_OLDMSYM;       //과거병력
            txtGajok.Text = list.MUN_GAJOK;              //가족력
            txtGiinsung.Text = list.MUN_GIINSUNG;        //업무기인성

            //자타각증상
            txtJengSang.Text = list.JENGSANG + "." + hb.READ_HIC_CODE("11", list.JENGSANG);

            //임상진찰
            txtJinChal0.Text = list.JIN_01;
            txtJinChal1.Text = list.JIN_02;
            txtJinChal2.Text = list.JIN_03;
            txtJinChal3.Text = list.JIN_04;

            txtPanDrNo.Text = list.SANGDAMDRNO.To<string>();
        }

        void fn_Screen_School_Display(long argWrtNo)
        {
            string strGbSTS = "";
            string strROWID = "";

            //상담내역이 있는지 점검(판정)
            strROWID = hicSchoolNewService.GetRowIdbyWrtNo(argWrtNo);

            //판정테이블 없을 시 판정테이블 생성함
            if (strROWID.IsNullOrEmpty())
            {
                if (!fn_HIC_NEW_SCHOOL_INSERT(argWrtNo, "56").IsNullOrEmpty())
                {
                    MessageBox.Show("접수번호 " + argWrtNo + " 신규판정테이블 자동발생시 오류가 발생함..전산실에 연락바람..", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            HIC_SANGDAM_NEW list = hicSangdamNewService.GetSangdamDrNoAmSangdambyWrtNo(FnWRTNO);

            if (!list.IsNullOrEmpty())
            {
                txtPanDrNo.Text = list.SANGDAMDRNO.To<string>();
                lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
                strGbSTS = list.GBSTS;
            }

            if (strGbSTS == "Y")
            {
                List<HIC_SCHOOL_NEW> list2 = hicSchoolNewService.GetItembyWrtNo(argWrtNo);

                if (list2.Count > 0)
                {
                    cboPRes.SelectedIndex = list2[0].PPANB1.To<int>();
                    txtPResEtc.Text = list2[0].PPANB2;
                    cboEJ0.SelectedIndex = VB.Pstr(list2[0].PPANC4, "^^", 2).To<int>();
                    cboEJ1.SelectedIndex = VB.Pstr(list2[0].PPANC4, "^^", 1).To<int>();
                    txtEJEtc.Text = VB.Pstr(list2[0].PPANC4, "^^", 3);
                    cboM.SelectedIndex = VB.Pstr(list2[0].PPANC7, "^^", 1).To<int>();
                    txtMEtc.Text = VB.Pstr(list2[0].PPANC7, "^^", 2);
                    cboN.SelectedIndex = VB.Pstr(list2[0].PPANC8, "^^", 1).To<int>();
                    txtNEtc.Text = VB.Pstr(list2[0].PPANC8, "^^", 2);
                    cboS.SelectedIndex = VB.Pstr(list2[0].PPANC9, "^^", 1).To<int>();
                    txtSEtc.Text = VB.Pstr(list2[0].PPANC9, "^^", 2);
                    cboHJ.SelectedIndex = VB.Pstr(list2[0].PPANC6, "^^", 1).To<int>();
                    txtHJEtc.Text = VB.Pstr(list2[0].PPANC6, "^^", 2);
                    //기관능력
                    cboOrgan0.SelectedIndex = VB.Pstr(list2[0].PPAND1, "^^", 1).To<int>();
                    txtOrgan0Etc.Text = VB.Pstr(list2[0].PPAND1, "^^", 2);
                    cboOrgan1.SelectedIndex = VB.Pstr(list2[0].PPAND2, "^^", 1).To<int>();
                    txtOrgan1Etc.Text = VB.Pstr(list2[0].PPAND2, "^^", 2);
                    cboOrgan2.SelectedIndex = VB.Pstr(list2[0].PPAND3, "^^", 1).To<int>();
                    txtOrgan2Etc.Text = VB.Pstr(list2[0].PPAND3, "^^", 2);
                    cboOrgan3.SelectedIndex = VB.Pstr(list2[0].PPAND4, "^^", 1).To<int>();
                    txtOrgan3Etc.Text = VB.Pstr(list2[0].PPAND4, "^^", 2);
                    cboOrgan4.SelectedIndex = VB.Pstr(list2[0].PPAND5, "^^", 1).To<int>();
                    txtOrgan4Etc.Text = VB.Pstr(list2[0].PPAND5, "^^", 2);
                    cboOrgan5.SelectedIndex = VB.Pstr(list2[0].PPAND6, "^^", 1).To<int>();
                    txtOrgan5Etc.Text = VB.Pstr(list2[0].PPAND6, "^^", 2);
                    //진촬및상담
                    cboJ.SelectedIndex = VB.Pstr(list2[0].PPANK1, "^^", 1).To<int>();
                    txtJEtc.Text = VB.Pstr(list2[0].PPANK1, "^^", 2);
                    //생활습관개선
                    HIC_SANGDAM_NEW list3 = hicSangdamNewService.GetHabitbyWrtNo(argWrtNo);

                    if (list3.HABIT1 == "1")
                    {
                        chkHabit0.Checked = true;
                    }
                    else
                    {
                        chkHabit0.Checked = false;
                    }

                    if (list3.HABIT2 == "1")
                    {
                        chkHabit1.Checked = true;
                    }
                    else
                    {
                        chkHabit1.Checked = false;
                    }

                    if (list3.HABIT3 == "1")
                    {
                        chkHabit2.Checked = true;
                    }
                    else
                    {
                        chkHabit2.Checked = false;
                    }

                    if (list3.HABIT4 == "1")
                    {
                        chkHabit3.Checked = true;
                    }
                    else
                    {
                        chkHabit3.Checked = false;
                    }

                    if (list3.HABIT5 == "1")
                    {
                        chkHabit4.Checked = true;
                    }
                    else
                    {
                        chkHabit4.Checked = false;
                    }

                    //외상후유증
                    if (VB.Pstr(list2[0].PPANK4, "^^", 1).To<int>() == 1)
                    {
                        rdoJinchal10.Checked = true;
                    }
                    else
                    {
                        rdoJinchal11.Checked = true;
                    }
                    txtExtInjury.Text = VB.Pstr(list2[0].PPANK3, "^^", 2);

                    //외상후유증
                    if (VB.Pstr(list2[0].PPANK4, "^^", 1).To<long>() == 1)
                    {
                        rdoJinchal20.Checked = true;
                    }
                    else if (VB.Pstr(list2[0].PPANK4, "^^", 1).To<long>() == 1)
                    {
                        rdoJinchal21.Checked = true;
                    }
                    else if (VB.Pstr(list2[0].PPANK4, "^^", 1).To<long>() == 2)
                    {
                        rdoJinchal22.Checked = true;
                    }
                    txtGenStatusEtc.Text = VB.Pstr(list2[0].PPANK4, "^^", 2);
                    txtRemark.Text = list2[0].SANGDAM;
                }
            }
        }

        string fn_HIC_NEW_SCHOOL_INSERT(long argWrtNo, string ArgJong)
        {
            string rtnVal = "";
            string strRowId = "";
            int result = 0;

            strRowId = hicSchoolNewService.GetRowIdbyWrtNo(argWrtNo).Trim();

            if (strRowId.IsNullOrEmpty())
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
                if (list.XP1 == "Y")
                {
                    rdo10.Checked = true;
                }
                else if (list.XP1 == "N")
                {
                    rdo11.Checked = true;
                }

                if (list.JINGBN == "Y")
                {
                    rdoGubun1.Checked = true;
                }
                else
                {
                    rdoGubun2.Checked = true;
                }

                txtXJong.Text = list.XPJONG;
                txtPlace.Text = list.XPLACE;
                txtXRemark.Text = list.XREMARK;
                txtTerm.Text = list.XTERM;
                
                txtMuch.Text = list.XMUCH;
                txtJung.Text = list.XJUNGSAN;
                txtMun1.Text = list.MUN1;
                txtEye.Text = list.JUNGSAN1;
                txtSkin.Text = list.JUNGSAN2;
                txtEtc.Text = list.JUNGSAN3;
                txtRemark.Text = list.SANGDAM;
                txtPanDrNo.Text = list.MUNDRNO.To<string>();
                cboXYear.SelectedIndex = 0;
                cboXMonth.SelectedIndex = 0;
                txtXTerm.Text = list.XTERM1;

                if (!txtXTerm.Text.IsNullOrEmpty())
                {
                    if (VB.InStr(list.XTERM1, "년") > 0)
                    {
                        cboXYear.Text = string.Format("{0:00}", VB.Pstr(list.XTERM1, "년", 1).To<int>());
                    }
                    if (VB.InStr(list.XTERM1, "년") > 0)
                    {
                        if (VB.InStr(list.XTERM1, "개월") > 0)
                        {
                            cboXMonth.Text = string.Format("{0:00}", VB.Pstr(VB.Pstr(list.XTERM1, "년", 2), "개월", 1).To<int>());
                        }
                        else if (VB.InStr(list.XTERM1, "월") > 0)
                        {
                            cboXMonth.Text = string.Format("{0:00}", VB.Pstr(VB.Pstr(list.XTERM1, "년", 2), "월", 1).To<int>());
                        }
                    }
                    else
                    {
                        cboXYear.Text = "";
                        if (VB.InStr(list.XTERM1, "개월") > 0)
                        {
                            cboXMonth.Text = string.Format("{0:00}", VB.Pstr(VB.Pstr(list.XTERM1, "년", 1), "개월", 1).To<int>());
                        }
                        else if (VB.InStr(txtXTerm.Text, "월") > 0)
                        {
                            cboXMonth.Text = string.Format("{0:00}", VB.Pstr(VB.Pstr(list.XTERM1, "년", 1), "월", 1).To<int>());
                        }
                    }
                }

                //방사선종사자 컬럼 추가
                nJil = list.JILBYUNG.To<int>();    //과거 질병력
                if (nJil == 0)
                {
                    rdoX10.Checked = true;
                }
                else if (nJil == 1)
                {
                    rdoX11.Checked = true;
                }

                chkX1_10.Checked = list.BLOOD1 == "1" ? true : false;   //혈액관련질환(빈혈)
                chkX1_11.Checked = list.BLOOD2 == "1" ? true : false;   //혈액관련질환(백혈병)
                if (!list.BLOOD3.IsNullOrEmpty())   //혈액관련질환(기타)
                {
                    chkX1_12.Checked = true;
                    txtX1_1.Text = list.BLOOD3;
                }
                chkX1_20.Checked = list.SKIN1 == "1" ? true : false;    //피부질환(아토피)
                chkX1_21.Checked = list.SKIN2 == "1" ? true : false;    //피부질환(습진)
                if (!list.SKIN3.IsNullOrEmpty())    //피부질환(기타)txtX2_1
                {
                    chkX1_22.Checked = true;
                    txtX1_2.Text = list.SKIN3;
                }
                txtX1_3.Text = list.NERVOUS1;    //신경계질환명
                chkX1_40.Checked = list.EYE1 == "1" ? true : false;   //눈 질환(백내장)
                if (!list.EYE2.IsNullOrEmpty())         //눈 질환(기타)
                {
                    chkX1_41.Checked = true;
                    txtX1_4.Text = list.EYE2;
                }
                txtX1_5.Text = list.CANCER1; //암 질환명

                nGajok = list.GAJOK.To<int>();              //가족력
                RadioButton rdoX2 = (Controls.Find("rdoX2" + nGajok.ToString(), true)[0] as RadioButton);
                rdoX2.Checked = true;

                txtX2_1.Text = list.BLOOD;           //혈액관련질환명
                txtX2_2.Text = list.NERVOUS2;        //신경계질환명txtXSymptom
                txtX2_3.Text = list.CANCER2;         //암 질환명
                txtXSymptom.Text = list.SYMPTON;     //최근 특이증상

                chkX40.Checked = list.JIKJONG1 == "1" ? true : false;  //현재 직종(비파괴검사)
                chkX41.Checked = list.JIKJONG2 == "1" ? true : false;  //현재 직종(방사선사)
                if (!list.JIKJONG3.IsNullOrEmpty())         //눈 질환(기타)
                {
                    chkX42.Checked = true;
                    txtJikjong.Text = list.JIKJONG3;
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
            nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();

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

            if (!strAm.IsNullOrEmpty())
            {
                for (int i = 1; i < VB.L(strAm, ","); i++)
                {
                    if (VB.Pstr(strAm, ",", i) == "1")
                    {
                        CheckBox chkJong = (Controls.Find("chkJong" + (i - 1).ToString(), true)[0] as CheckBox);
                        chkJong.Checked = true;
                    }
                }
            }

            //GoSub Screen_Endo_Exam_Check        '내시경이 있는지 Check
            Fstr내시경대상 = "";

            List<HIC_RESULT> list = hicResultService.GetExCodeLoopbyWrtNo(argWrtNo);

            for (int i = 0; i < list.Count; i++)
            {
                strExCode = list[i].EXCODE;
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
                    if (list2.ENDOGUBUN3 == "Y")
                    {
                        chkEGD1.Checked = true;
                    }
                }
            }

            SSHyang.ActiveSheet.RowCount = 0;
            if (Fstr내시경대상 == "Y")
            {
                //내시경 대상자 이면 내시경챠트 테이블 생성함
                if (!fn_Read_Endo_Chart(FstrPtno, FstrJepDate).IsNullOrEmpty())
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
                strAmSangdam = list3.AMSANGDAM;
                txtPanDrNo.Text = list3.SANGDAMDRNO.To<string>();
                lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
            }

            if (strAmSangdam.IsNullOrEmpty())
            {
                if (!VB.Pstr(strAmSangdam, "{}", 1).IsNullOrEmpty()) txtBodySymptom.Text = VB.Pstr(strAmSangdam, "{}", 1);
                if (!VB.Pstr(strAmSangdam, "{}", 2).IsNullOrEmpty()) txtCancerHis.Text = VB.Pstr(strAmSangdam, "{}", 2);
                if (!VB.Pstr(strAmSangdam, "{}", 3).IsNullOrEmpty()) txtStomachBowlLiver.Text = VB.Pstr(strAmSangdam, "{}", 3);
                if (!VB.Pstr(strAmSangdam, "{}", 4).IsNullOrEmpty()) txtChestCervical.Text = VB.Pstr(strAmSangdam, "{}", 4);
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
                if (list.GB_CFS1 == "1") chkCFS0.Checked = true;
                if (list.GB_CFS2 == "1") chkCFS1.Checked = true;

                //파트1
                if (list.GB_DIET == "1") rdoMealState1.Checked = true;

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
                if (list.GB_OLD == "1") chkMedHis0.Checked = true;
                if (list.GB_OLD1 == "1") chkMedHis1.Checked = true;
                if (list.GB_OLD2 == "1") chkMedHis2.Checked = true;
                if (list.GB_OLD3 == "1") chkMedHis3.Checked = true;
                if (list.GB_OLD4 == "1") chkMedHis4.Checked = true;
                if (list.GB_OLD5 == "1") chkMedHis5.Checked = true;
                if (list.GB_OLD6 == "1") chkMedHis6.Checked = true;
                if (list.GB_OLD7 == "1") chkMedHis7.Checked = true;
                if (list.GB_OLD8 == "1") chkMedHis8.Checked = true;
                if (list.GB_OLD9 == "1") chkMedHis9.Checked = true;
                if (list.GB_OLD10 == "1") chkMedHis10.Checked = true;
                if (list.GB_OLD11 == "1") chkMedHis11.Checked = true;
                if (list.GB_OLD12 == "1") chkMedHis12.Checked = true;
                if (list.GB_OLD13 == "1") chkMedHis13.Checked = true;
                if (list.GB_OLD14 == "1") chkMedHis14.Checked = true;
                txtMedHis13.Text = list.GB_OLD13_1;
                txtMedHisEtc.Text = list.GB_OLD15_1;

                //현재복용약물
                if (list.GB_DRUG == "1")  chkMedcine0.Checked = true;
                if (list.GB_DRUG1 == "1") chkMedcine1.Checked = true;
                if (list.GB_DRUG2 == "1") chkMedcine2.Checked = true;
                if (list.GB_DRUG3 == "1") chkMedcine3.Checked = true;
                if (list.GB_DRUG4 == "1") chkMedcine4.Checked = true;
                if (list.GB_DRUG5 == "1") chkMedcine5.Checked = true;
                if (list.GB_DRUG6 == "1") chkMedcine6.Checked = true;
                if (list.GB_DRUG7 == "1") chkMedcine7.Checked = true;

                txtMedcineEtc.Text = list.GB_DRUG8_1;
                txtMedAspirin.Text = list.GB_DRUG_STOP1;
                txtAntiCoagulant.Text = list.GB_DRUG_STOP2;

                //전처치약제
                chkPreTreatment0.Checked = list.GB_B_DRUG == "1" ? true : false;
                chkPreTreatment1.Checked = list.GB_B_DRUG1 == "1" ? true : false;
                txtJinjengUse.Text = list.GB_B_DRUG1_1;
                txtRemark.Text = list.GB_BIGO;
            }

            //향정주사 승인요청
            List<HIC_HYANG_APPROVE> list2 = hicHyangApproveService.GetSucodeDrSabunQtybyWrtNo(txtWrtNo.Text.To<long>());

            nRead = list2.Count;
            SSHyang.ActiveSheet.RowCount = nRead;
            for (int i = 0; i < nRead; i++)
            {
                strSuCode = list2[i].SUCODE;
                SSHyang.ActiveSheet.Cells[i, 0].Text = strSuCode;
                SSHyang.ActiveSheet.Cells[i, 1].Text = fn_Read_Drug_Jep_Name(strSuCode);
                SSHyang.ActiveSheet.Cells[i, 2].Text = fn_Read_Drug_Unit(strSuCode);
                SSHyang.ActiveSheet.Cells[i, 3].Text = list2[i].QTY.To<string>();
            }

            cboASA.Text = "";
            if (SSHyang.ActiveSheet.RowCount > 0)
            {
                strASA = endoJupmstService.GetASAbyPtNoJepDate(FstrPtno, FstrJepDate);
                if (!strASA.IsNullOrEmpty())
                {   
                    for (int i = 0; i < cboASA.Items.Count; i++)
                    {
                        if (VB.Pstr(cboASA.Items[i].To<string>(), ".", 1) == strASA)
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
                strUnit1 = list.UNITNEW1.To<string>();
                strUnit2 = list.UNITNEW2.To<string>();
                strUnit3 = list.UNITNEW3.To<string>();
                strUnit4 = list.UNITNEW4.To<string>();
                strUnit = strUnit1 + strUnit2 + "/" + (strUnit4.To<long>() > 0 ? strUnit4 + "㎖/ " : "" + strUnit3);
            }

            rtnVal = strUnit;

            return rtnVal;
        }

        string fn_Read_Endo_Chart(string argPtno, string argJepDate)
        {
            string rtnVal = "";
            int result = 0;

            if (endoJupmstService.GetCountbyPtNoJDate(argPtno, argJepDate) > 0)
            {
                if (endoChartService.GetCountbyPtnoRDate(argPtno, argJepDate) == 0)
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    ENDO_CHART item = new ENDO_CHART();

                    item.PTNO = argPtno;
                    item.BDATE = argJepDate;
                    item.RDATE = argJepDate;
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
            if (!list.DIABETES_1.IsNullOrEmpty())
            {
                nValue = list.DIABETES_1.To<int>() - 1;
                RadioButton rdoDang = (Controls.Find("rdoDang" + nValue.ToString(), true)[0] as RadioButton);
                rdoDang.Checked = true;
            }

            //당뇨병(치료계획)
            if (!list.DIABETES_2.IsNullOrEmpty())
            {
                nValue = list.DIABETES_2.To<int>() - 1;
                RadioButton rdoDangJob = (Controls.Find("rdoDangJob" + nValue.ToString(), true)[0] as RadioButton);
                rdoDangJob.Checked = true;
            }

            //고혈압(결과상태)
            if (!list.CYCLE_1.IsNullOrEmpty())
            {
                nValue = list.CYCLE_1.To<int>() - 1;
                RadioButton rdoGohyul = (Controls.Find("rdoGohyul" + nValue.ToString(), true)[0] as RadioButton);
                rdoGohyul.Checked = true;
            }

            //고혈압(치료계획)
            if (!list.CYCLE_2.IsNullOrEmpty())
            {
                nValue = list.CYCLE_2.To<int>() - 1;
                RadioButton rdoGohyulJob = (Controls.Find("rdoGohyulJob" + nValue.ToString(), true)[0] as RadioButton);
                rdoGohyulJob.Checked = true;
            }

            //식사여부
            if (list.GBSIKSA == "Y")
            {
                rdoMealState0.Checked = true;
            }
            else
            {
                rdoMealState1.Checked = true;
            }

            txtRemark.Text = list.REMARK;    //상담내역
            txtPanDrNo.Text = list.SANGDAMDRNO.To<string>();
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
            int[] nGbSangdam = new int[21];
            string[] strPjSangdam = new string[21];
            string strName = "";

            for (int i = 1; i <= 20; i++)
            {
                nGbSangdam[i] = 0;
                strPjSangdam[i] = "";
            }

            //if (!argPjSangdam.IsNullOrEmpty())
            //{
            //    nCNT = VB.L(argPjSangdam, "{$}") - 1;
            //    for (int i = 1; i < nCNT; i++)
            //    {
            //        strData = VB.Pstr(argPjSangdam, "{$}", i);
            //        j = VB.Pstr(argPjSangdam, "{$}", i).To<int>();
            //        strPjSangdam[j] = VB.Pstr(strData, "{}", 2);
            //    }
            //}

            if (!argPjSangdam.IsNullOrEmpty())
            {
                nCNT = VB.L(argPjSangdam, "{$}") - 1;
                for (int i = 1; i <= nCNT; i++)
                {
                    strData = VB.Pstr(argPjSangdam, "{$}", i);
                    j = Convert.ToInt32(VB.Pstr(strData ,"{}",1));
                    strPjSangdam[j] = VB.Pstr(strData, "{}", 2);
                }
            }


            SS9.ActiveSheet.RowCount = 0;

            //상담해야할 표적장기 찾기
            List<HIC_SUNAPDTL_GROUPCODE> list = hicSunapdtlGroupcodeService.GetCodeGbSangdambyWrtNo(argWRTNO);

            nREAD = list.Count;
            for (int i = 0; i < nREAD; i++)
            {
                strData = list[i].GBSANGDAM;
                for (int k = 1; k <= strData.Length; k++)
                {
                    if (VB.Mid(strData, k, 1) == "1")
                    {
                        nGbSangdam[k] = 1;
                    }
                }
            }

            //표적장기명칭 및 상담결과를 표시
            nRow = 0;
            for (int i = 1; i <= 20; i++)
            {
                if (nGbSangdam[i] == 1)
                {
                    strName = hicBcodeService.GetCodeNamebyGubunCode("HIC_표적장기별특수상담", string.Format("{0:00}", i));

                    nRow += 1;
                    SS9.ActiveSheet.RowCount = nRow;
                    SS9.ActiveSheet.Cells[nRow - 1, 0].Text = strName;
                    if (!strPjSangdam[i].IsNullOrEmpty())
                    {
                        SS9.ActiveSheet.Cells[nRow - 1, 1].Text = strPjSangdam[i];
                    }
                    else
                    {
                        SS9.ActiveSheet.Cells[nRow - 1, 1].Text = "특이소견 없음";
                    }
                    SS9.ActiveSheet.Cells[nRow - 1, 2].Text = string.Format("{0:00}", i);
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
            long[] nJumsu = new long[20];
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
            FstrHabit[11] = "N";
            FstrHabit[12] = "N";
            FstrHabit[13] = "N";
            FstrHabit[14] = "N";
            FstrHabit[15] = "N";
            FstrHabit[16] = "N";
            FstrHabit[17] = "N";
            FstrHabit[18] = "N";

            //접수내역을 보고 항목을 세팅
            List<HIC_RESULT> list = hicResultService.GetExCodeResultbyOnlyWrtNo(nWRTNO);

            for (int i = 0; i < list.Count; i++)
            {
                switch (list[i].EXCODE)
                {
                    case "A143":
                        FstrHabit[11] = "Y";    //흡연
                        break;
                    case "A144":
                        FstrHabit[12] = "Y";    //음주
                        break;
                    case "A145":
                        FstrHabit[13] = "Y";    //운동
                        break;
                    case "A146":
                        FstrHabit[14] = "Y";    //영양
                        break;
                    case "A147":
                        FstrHabit[15] = "Y";    //비만
                        break;
                    case "A127":
                    case "A128":
                        FstrHabit[16] = "Y";    //우울증
                        break;
                    case "A129":
                        FstrHabit[17] = "Y";    //인지
                        break;
                    case "A118":
                    case "A119":
                    case "A120":
                    case "A130":
                        FstrHabit[18] = "Y";    //노인신체기능
                        break;
                    default:
                        break;
                }
            }

            if (FstrHabit[11] == "N" && FstrHabit[12] == "N" && FstrHabit[13] == "N" && FstrHabit[14] == "N" && 
                FstrHabit[15] == "N" && FstrHabit[16] == "N" && FstrHabit[17] == "N" && FstrHabit[18] == "N")
            {
                FstrTFlag = "";
                return;
            }

            FstrTFlag = "Y";

            //점수표 계산 및 DB업데이트
            for (int i = 11; i <= 17; i++)
            {
                nJumsu[i] = 0;
                if (i >= 16)
                {
                    i += 2;
                }
                nJumsu[i] = hicTitemService.GetTotalbyGubunWrtNo(i, nWRTNO);

                if (i == 18)
                {
                    i -= 2;
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
                strResult = list2[i].RESULT;
                switch (list2[i].EXCODE)
                {
                    case "A143":    //흡연
                        result = hicResultService.UpdateResultbyWrtNoExCode(nJumsu[11].To<string>(), nWRTNO, clsType.User.IdNumber.To<long>(), list2[i].EXCODE);
                        break;
                    case "A144":    //음주
                        result = hicResultService.UpdateResultbyWrtNoExCode(nJumsu[12].To<string>(), nWRTNO, clsType.User.IdNumber.To<long>(), list2[i].EXCODE);
                        break;
                    case "A145":    //운동
                        result = hicResultService.UpdateResultbyWrtNoExCode("0", nWRTNO, clsType.User.IdNumber.To<long>(), list2[i].EXCODE);
                        break;
                    case "A146":    //영양
                        result = hicResultService.UpdateResultbyWrtNoExCode(nJumsu[14].To<string>(), nWRTNO, clsType.User.IdNumber.To<long>(), list2[i].EXCODE);
                        break;
                    case "A130":    //우울증
                        result = hicResultService.UpdateResultbyWrtNoExCode(nJumsu[18].To<string>(), nWRTNO, clsType.User.IdNumber.To<long>(), list2[i].EXCODE);
                        break;
                    case "A129":    //인지기능
                        result = hicResultService.UpdateResultbyWrtNoExCode(nJumsu[19].To<string>(), nWRTNO, clsType.User.IdNumber.To<long>(), list2[i].EXCODE);
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

            nJumsu[13] = hm.Read_Auto_WORK(nWRTNO);

            nBiman1 = 0;
            nBiman2 = 0;

            List<string> sExCode = new List<string>();

            sExCode.Clear();
            sExCode.Add("A115");
            sExCode.Add("A117");

            List<HIC_RESULT> list3 = hicResultService.GetResultbyWrtNoExCodeList(nWRTNO, sExCode);

            nBiman1 = list3[0].RESULT.To<double>();
            nBiman2 = list3[1].RESULT.To<double>();

            //비만도체크
            //if (nBiman1 < 18.5)
            //{
            //    strBiman = "저체중";
            //}
            //else if (nBiman1 >= 18.5 && nBiman1 <= 24.9)
            //{
            //    strBiman = "정상체중";
            //}
            //else if (nBiman1 >= 25 && nBiman1 <= 29.9)
            //{
            //    strBiman = "비만";
            //}
            //else if (nBiman1 >= 30)
            //{
            //    strBiman = "고도비만";
            //}

            //비만도체크(2021-05-04 비만기준 변경)
            if (nBiman1 <= 24.9)
            {
                strBiman = "정상체중";
            }
            else if (nBiman1 >= 25 && nBiman1 <= 29.9)
            {
                strBiman = "과체중";
            }
            else if (nBiman1 >= 30)
            {
                strBiman = "비만";
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
            fn_Result_EntryEnd_Check(txtWrtNo.Text.To<long>());
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

            strRowId = hicSangdamNewService.GetRowIdbyWrtNo(FnWRTNO);

            if (strRowId.IsNullOrEmpty())
            {
                clsDB.setBeginTran(clsDB.DbCon);

                if (fn_HIC_ExJong_CHECK2(argJong) == "Y")
                {
                    HIC_SANGDAM_NEW item = new HIC_SANGDAM_NEW();

                    item.WRTNO = argWrtNo;
                    item.GJJONG = argJong;
                    item.GJCHASU = FstrChasu;
                    item.JEPDATE = FstrJepDate;
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
        string fn_READ_LUNG_PAN(long argPano, string argYear)
        {
            string rtnVal = "";
            string strYear = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            //strYear = VB.Left(clsPublic.GstrSysDate, 4);

            HIC_JEPSU_CANCER_NEW list = hicJepsuCancerNewService.GetItembyPaNoGjYear(argPano, argYear);

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
            List<HIC_SANGDAM_WAIT> list = hicSangdamWaitService.GetWrtNobyGubunNotWrtNo(clsHcVariable.GstrDrRoom, txtWrtNo.Text.To<long>());

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strWRTNO.Add(list[i].WRTNO.To<string>());
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

            result = hicSangdamWaitService.UpdateCallTimebyWrtNoGubun(txtWrtNo.Text.To<long>(), clsHcVariable.GstrDrRoom);

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
                strGjJong = list[i].GJJONG;
                strWRTNO = list[i].WRTNO.To<string>();

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

                if (list[i].GBSTS == "Y")
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
                    if (!FstrUCodes.IsNullOrEmpty())
                    {
                        SSJong.ActiveSheet.Cells[0, 1].Text = "○";
                        if (list[i].GBSTS == "Y")
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
                    if (!FstrUCodes.IsNullOrEmpty())
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
            double nOldQty = 0;
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
            string strApproveTime = "";

            //승인한것 수가코드 목록을 만듬
            strList = ",";
            for (int i = 0; i < SSHyang.ActiveSheet.RowCount; i++)
            {
                strSuCode = SSHyang.ActiveSheet.Cells[i, 0].Text.Trim();
                strList += strSuCode + ",";
            }

            //clsDB.setBeginTran(clsDB.DbCon);

            //HIC_HYANG_Approve에 삭제된것 등록
            List<HIC_HYANG_APPROVE> list = hicHyangApproveService.GetItembyWrtNoDeptCode(argWrtNo, "HR");

            for (int i = 0; i < list.Count; i++)
            {
                strSuCode = list[i].SUCODE.Trim();
                //승인한 수가코드 목록에 없으면 삭제함
                if (VB.InStr(strList, "," + strSuCode + ",") == 0)
                {
                    strBDATE = list[i].BDATE;
                    strPtNo = list[i].PTNO;

                    result = hicHyangApproveService.UpdateDelDatebyRowId(list[i].ROWID);

                    if (result < 0)
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("향정승인 오류1", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FstrCOMMIT = "NO";
                        return;
                    }

                    result = hicHyangService.UpdateDelDatebyWrtNo(argWrtNo, strSuCode);

                    if (result < 0)
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("향정승인 오류2", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FstrCOMMIT = "NO";
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
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("향정승인 오류3", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FstrCOMMIT = "NO";
                        return;
                    }

                    //OCS_OORDER 전송
                    result = comHpcLibBService.DeleteOcsOorder(strPtNo, strSuCode, strBDATE);

                    if (result < 0)
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("향정승인 오류4", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FstrCOMMIT = "NO";
                        return;
                    }
                }
            }


            HIC_JEPSU list3 = hicJepsuService.GetJuso1Juso2byWrtNo(argWrtNo);

            strJuso = list3.JUSO1 + list3.JUSO2;

            //승인한것 업데이트
            for (int k = 0; k < SSHyang.ActiveSheet.RowCount; k++)
            {
                strSuCode = SSHyang.ActiveSheet.Cells[k, 0].Text.Trim();
                nQty = SSHyang.ActiveSheet.Cells[k, 3].Text.Trim().To<long>();

                if (!strSuCode.IsNullOrEmpty())
                {
                    HIC_HYANG_APPROVE list4 = hicHyangApproveService.GetItembyWrtNoSucode(argWrtNo, strSuCode);

                    if (!list4.IsNullOrEmpty())
                    {
                        strROWID = list4.RID;
                        strBDATE = list4.BDATE;
                        strPtNo = list4.PTNO;
                        strSname = list4.SNAME;
                        strGbSite = list4.GBSITE;
                        strSex = list4.SEX;
                        nOldQty = list4.QTY.To<double>();
                        nAge = list4.AGE;
                        strApproveTime = list4.APPROVETIME;
                    }

                    //승인한 약품의 수량이 변경되었으면 History를 저장함
                    if (nQty != nOldQty && !strApproveTime.IsNullOrEmpty())
                    {
                        result = hicHyangApproveService.InsertSelectbyRowId(strROWID);

                        if (result < 0)
                        {
                            //clsDB.setRollbackTran(clsDB.DbCon);
                            FstrCOMMIT = "NO";
                            MessageBox.Show("승인의뢰용 처방을 등록 중 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    if (nQty != nOldQty)
                    {
                        strGubun = "1";
                    }
                    else if (strApproveTime.IsNullOrEmpty())
                    {
                        strAppTime = "";
                    }

                    result = hicHyangApproveService.UpdateAppTimebyRowId(strGubun, strAppTime, nQty, clsType.User.IdNumber, strROWID);

                    if (result < 0)
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        FstrCOMMIT = "NO";
                        MessageBox.Show("승인의뢰용 처방을 등록 중 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                            
                    //SEQNO를 읽음
                    strSysdate = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";
                    nSeqNo = hicHyangService.GetSeqNobyBDate(strSysdate);

                    //HIC_HYANG Update

                    HIC_HYANG listHang = hicHyangService.GetRowIdbyWrtNoSuCode(argWrtNo, strSuCode);

                    //if (!listHang.IsNullOrEmpty())
                    //{
                    //    strROWID = listHang.RID;
                    //}
                    //else
                    //{
                    //    strROWID = "";
                    //}

                    if (!listHang.IsNullOrEmpty())
                    {
                        result = hicHyangService.UpdateQtybyRowId(nQty, listHang.ROWID);
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


                    //if (!strROWID.IsNullOrEmpty())
                    //{
                    //    result = hicHyangService.UpdateQtybyRowId(nQty, strROWID);
                    //}
                    //else
                    //{
                    //    HIC_HYANG item = new HIC_HYANG();

                    //    item.IO = "O";
                    //    item.NAL = 1;
                    //    item.DOSCODE = "920103";
                    //    item.REMARK1 = "검사용";
                    //    item.REMARK2 = "Pain";
                    //    item.SEQNO = nSeqNo;
                    //    item.ORDERNO = 0;
                    //    item.JUSO = strJuso;
                    //    item.ROWID = strROWID;

                    //    result = hicHyangService.InsertSelectbyWorId(item);
                    //}

                    if (result < 0)
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        FstrCOMMIT = "NO";
                        MessageBox.Show("향정약품 등록 중 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //외래접수
                    if (comHpcLibBService.GetCountbyPaNoBDate(strPtNo, strBDATE) == 0)
                    {
                        COMHPC item = new COMHPC();

                        item.PTNO = strPtNo;
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
                            //clsDB.setRollbackTran(clsDB.DbCon);
                            FstrCOMMIT = "NO";
                            MessageBox.Show("외래접수 등록 중 오류 발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }


                    //OCS_OORDER 전송
                    COMHPC list5 = comHpcLibBService.GetRowIdQtybyOcsOrder(strPtNo, strBDATE, strSuCode);

                    if (list5.IsNullOrEmpty())
                    {
                        strDosCode = "920103";
                        //HD실 요청으로 용법코드를 920103을 사용함
                        strDrCode = hb.READ_HIC_OcsDrcode(clsType.User.IdNumber.To<long>());
                        //오더코드,SlipNo 설정
                        switch (strSuCode.Trim())
                        {
                            case "A-ANE12G":
                                strORDERCODE = "A-ANE12G";
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
                        item2.CBUN = "20";
                        item2.IP = clsPublic.GstrIpAddress;

                        result = comHpcLibBService.InsertOcsOrder(item2);

                        if (result < 0)
                        {
                            //clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("오더전송 오류1", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            FstrCOMMIT = "NO";
                            return;
                        }
                    }
                    else if (list5.QTY != nQty)
                    {
                        result = comHpcLibBService.UpdateOcsOrder(nQty, list5.ROWID);

                        if (result < 0)
                        {
                            //clsDB.setRollbackTran(clsDB.DbCon);
                            MessageBox.Show("오더전송 오류2", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            FstrCOMMIT = "NO";
                            return;
                        }
                    }

                    //OCS 오더번호를 찾음
                    nOrderno = comHpcLibBService.GetOrderNoOcsOrderbyPtno(strPtNo, strBDATE, strSuCode,"HR");

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
                            item3.DRSABUN = clsType.User.IdNumber.To<long>();
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
                                //clsDB.setRollbackTran(clsDB.DbCon);
                                FstrCOMMIT = "NO";
                                MessageBox.Show("마약 저장 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
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
                            item4.DRSABUN = clsType.User.IdNumber.To<long>();
                            item4.CERTNO = "";
                            item4.RID = strROWID;

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
                                //clsDB.setRollbackTran(clsDB.DbCon);
                                FstrCOMMIT = "NO";
                                MessageBox.Show("향정 저장 중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
            }
                    //clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// EMR 데이터 등록
        /// </summary>
        /// <param name="argWrtNo"></param>
        /// <param name="argEGD1"></param>
        /// <param name="argEGD2"></param>
        void fn_ENDO_EMR_INSERT(long argWrtNo, string argEGD1, string argEGD2, string argCFS1, string agrCFS2)
        {
            string strJONGGUM = "";
            string strHEAENDO = "";
            string strPtNo = "";
            string strPano = "";
            string strDrCode = "";
            string strJepDate = "";
            string strOK = "OK";
            string strDeptCode  = "";
            string strOK1 = "";
            string strFrDate = "";
            string strToDate = "";
            int result = 0;

            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(argWrtNo);

            if (!list.IsNullOrEmpty())
            {
                strJONGGUM = list.JONGGUMYN;
                strHEAENDO = list.GBHEAENDO;
                strPtNo = list.PTNO;
                strPano = list.PANO.To<string>();
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
                        item.BUN = "48";
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
                        item.CBUN = "480";
                        item.IP = clsPublic.GstrIpAddress;

                        //2021-08-16
                        item.SUCODE = OF.Mapping_SuCode(clsDB.DbCon, item.ORDERCODE, item.SUCODE,"", item.BDATE, item.DEPTCODE);

                        result = comHpcLibBService.InsertOcsOrder(item);
                        if (result < 0)
                        {
                            FstrCOMMIT = "NO";
                            return;
                        }

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

                            //2021-08-16
                            item2.SUCODE = OF.Mapping_SuCode(clsDB.DbCon, item2.ORDERCODE, item2.SUCODE, "", item2.BDATE, item2.DEPTCODE);

                            result = comHpcLibBService.InsertOcsOrder(item2);

                            if (result < 0)
                            {
                                FstrCOMMIT = "NO";
                                return;
                            }
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

                            //2021-08-16
                            item3.SUCODE = OF.Mapping_SuCode(clsDB.DbCon, item3.ORDERCODE, item3.SUCODE, "", item3.BDATE, item3.DEPTCODE);

                            result = comHpcLibBService.InsertOcsOrder(item3);

                            if (result < 0)
                            {
                                FstrCOMMIT = "NO";
                                return;
                            }
                        }
                    }
                    else if (argEGD1 == "1" && argEGD2.IsNullOrEmpty())
                    {
                        COMHPC item4 = new COMHPC();

                        item4.PTNO = strPtNo;
                        item4.BDATE = clsPublic.GstrSysDate;
                        item4.DEPTCODE = "HR";
                        item4.SEQNO = 99;
                        item4.ORDERCODE = "00440110";
                        item4.SUCODE = "E7630";
                        item4.BUN = "48";
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
                        item4.CBUN = "480";
                        item4.IP = clsPublic.GstrIpAddress;

                        //2021-08-16
                        item4.SUCODE = OF.Mapping_SuCode(clsDB.DbCon, item4.ORDERCODE, item4.SUCODE, "", item4.BDATE, item4.DEPTCODE);

                        result = comHpcLibBService.InsertOcsOrder(item4);

                        if (result < 0)
                        {
                            FstrCOMMIT = "NO";
                            return;
                        }
                    }
                }
                else if (strJONGGUM == "1" && strOK == "OK" && strOK1.IsNullOrEmpty())
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
                        item5.BUN = "48";
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
                        item5.CBUN = "480";
                        item5.IP = clsPublic.GstrIpAddress;

                        //2021-08-16
                        item5.SUCODE = OF.Mapping_SuCode(clsDB.DbCon, item5.ORDERCODE, item5.SUCODE, "", item5.BDATE, item5.DEPTCODE);

                        result = comHpcLibBService.InsertOcsOrder(item5);

                        if (result < 0)
                        {
                            FstrCOMMIT = "NO";
                            return;
                        }

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

                            //2021-08-16
                            item6.SUCODE = OF.Mapping_SuCode(clsDB.DbCon, item6.ORDERCODE, item6.SUCODE, "", item6.BDATE, item6.DEPTCODE);

                            result = comHpcLibBService.InsertOcsOrder(item6);

                            if (result < 0)
                            {
                                FstrCOMMIT = "NO";
                                return;
                            }
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

                            //2021-08-16
                            item7.SUCODE = OF.Mapping_SuCode(clsDB.DbCon, item7.ORDERCODE, item7.SUCODE, "", item7.BDATE, item7.DEPTCODE);

                            result = comHpcLibBService.InsertOcsOrder(item7);

                            if (result < 0)
                            {
                                FstrCOMMIT = "NO";
                                return;
                            }
                        }
                    }
                    else if (argEGD1 == "1" && argEGD2.IsNullOrEmpty())
                    {
                        COMHPC item8 = new COMHPC();

                        item8.PTNO = strPtNo;
                        item8.BDATE = clsPublic.GstrSysDate;
                        item8.DEPTCODE = "HR";
                        item8.SEQNO = 99;
                        item8.ORDERCODE = "00440110";
                        item8.SUCODE = "E7630";
                        item8.BUN = "48";
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
                        item8.CBUN = "480";
                        item8.IP = clsPublic.GstrIpAddress;

                        //2021-08-16
                        item8.SUCODE = OF.Mapping_SuCode(clsDB.DbCon, item8.ORDERCODE, item8.SUCODE, "", item8.BDATE, item8.DEPTCODE);

                        result = comHpcLibBService.InsertOcsOrder(item8);

                        if (result < 0)
                        {
                            FstrCOMMIT = "NO";
                            return;
                        }
                    }
                }
            }

            //OILLS
            if (argEGD1 == "1" || argCFS1 == "1")
            {
                if (comHpcLibBService.GetCountbyPtNoBDate(strPtNo, strJepDate) == 0)
                {
                    COMHPC item = new COMHPC();

                    item.PTNO = strPtNo;
                    item.BDATE = strJepDate;
                    item.DEPTCODE = "HR";
                    item.SEQNO = 1;
                    item.ILLCODE = "Z018";

                    result = comHpcLibBService.InsertOIlls(item);

                    if (result < 0)
                    {
                        FstrCOMMIT = "NO";
                        return;
                    }
                }
            }
            
        }

        void eCode_value(string strCode, string strName)
        {
            FstrCode = strCode;
            FstrName = strName;
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            string strValue = "";

            if (sender == SS_JinDan)
            {
                if (e.Column == 1)
                {
                    
                    if (SS_JinDan.ActiveSheet.Cells[e.Row, 1].Text == "True")
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
                    else
                    {
                        switch (e.Row)
                        {
                            case 0:
                                txtRemark.Text = txtRemark.Text.Replace("뇌졸증 병력( 년)" + "\r\n", "");
                                break;
                            case 1:
                                txtRemark.Text = txtRemark.Text.Replace("심장병 병력( 년)" + "\r\n", "");
                                break;
                            case 2:
                                txtRemark.Text = txtRemark.Text.Replace("고혈압 병력( 년)" + "\r\n", "");
                                break;
                            case 3:
                                txtRemark.Text = txtRemark.Text.Replace("당뇨병 병력( 년)" + "\r\n", "");
                                break;
                            case 4:
                                txtRemark.Text = txtRemark.Text.Replace("이상지질혈증 병력( 년)" + "\r\n", "");
                                break;
                            case 5:
                                txtRemark.Text = txtRemark.Text.Replace("간장질환 병력( 년)" + "\r\n", "");
                                break;
                            case 6:
                                txtRemark.Text = txtRemark.Text.Replace("폐결핵 병력( 년)" + "\r\n", "");
                                break;
                            case 7:
                                txtRemark.Text = txtRemark.Text.Replace("기타 병력( 년)" + "\r\n", "");
                                break;
                            default:
                                break;
                        }
                    }
                }
                else if (e.Column == 2)
                {
                    if (SS_JinDan.ActiveSheet.Cells[e.Row, 2].Text == "True")
                    {
                        SS_JinDan.ActiveSheet.Cells[e.Row, 1].Text = "True";
                    }
                    else
                    {
                        SS_JinDan.ActiveSheet.Cells[e.Row, 1].Text = "";
                    }

                    if (SS_JinDan.ActiveSheet.Cells[e.Row, 2].Text == "True")
                    {
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
                    else
                    {
                        switch (e.Row)
                        {
                            case 0:
                                txtRemark.Text = txtRemark.Text.Replace("뇌졸중 약물치료중( 년)" + "\r\n", "");
                                break;
                            case 1:
                                txtRemark.Text = txtRemark.Text.Replace("심장병 약물치료중( 년)" + "\r\n", "");
                                break;
                            case 2:
                                txtRemark.Text = txtRemark.Text.Replace("고혈압 약물치료중( 년)" + "\r\n", "");
                                break;
                            case 3:
                                txtRemark.Text = txtRemark.Text.Replace("당뇨병 약물치료중( 년)" + "\r\n", "");
                                break;
                            case 4:
                                txtRemark.Text = txtRemark.Text.Replace("이상지질혈증 약물치료중( 년)" + "\r\n", "");
                                break;
                            case 5:
                                txtRemark.Text = txtRemark.Text.Replace("간장질환 약물치료중( 년)" + "\r\n", "");
                                break;
                            case 6:
                                txtRemark.Text = txtRemark.Text.Replace("폐결핵 약물치료중( 년)" + "\r\n", "");
                                break;
                            case 7:
                                txtRemark.Text = txtRemark.Text.Replace("기타 약물치료중( 년)" + "\r\n", "");
                                break;
                            default:
                                break;
                        }
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

            if (!strResult.IsNullOrEmpty())
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

            if (strResCode.IsNullOrEmpty())
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
            else if (sender == SSList)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(SSList, e.Column, ref boolSort, true);
                    return;
                }
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

                if (btnSave.Enabled == true)    //저장버튼이 활성화 되어 있을때 (의사일때)
                {
                    if (FblnPatChangeSaveFlag == false)
                    {
                        string strPtNo = "";
                        string strSName = "";

                        strPtNo = ssPatInfo.ActiveSheet.Cells[0, 0].Text;
                        strSName = ssPatInfo.ActiveSheet.Cells[0, 1].Text;

                        if (MessageBox.Show(strSName + "[" + strPtNo + "] 님의 상담 내용이 저장 되지 않았습니다" + "\r\n\r\n" + "저장 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            btnSave.Focus();
                            eBtnClick(btnSave, new EventArgs());
                        }
                    }
                }

                //FnRow = e.Row;
                fn_Screen_Clear();

                txtWrtNo.Text = SSList.ActiveSheet.Cells[e.Row, 0].Text.Trim();

                TabMain.Visible = false;
                TabMain.Visible = true;

                fn_Screen_Display();

                FblnPatChangeSaveFlag = false;
            }
            else if (sender == SS2)
            {
                if (e.Column != 2)
                {
                    return;
                }
                SS2.ActiveSheet.SetActiveCell(e.Row, 2);
            }
            else if (sender == SSHistory)
            {
                //2021-02-01(종검 제외처리)
                if (string.Compare(SSHistory.ActiveSheet.Cells[e.Row, 3].Text, "2") <= 0  && SSHistory.ActiveSheet.Cells[e.Row, 1].Text != "종검")
                {
                    FrmHcPanView = new frmHcPanView(SSHistory.ActiveSheet.Cells[e.Row, 2].Text.Trim().To<long>()); //WrtNo
                    FrmHcPanView.StartPosition = FormStartPosition.CenterScreen;
                    FrmHcPanView.ShowDialog(this);
                }
                else if (string.Compare(SSHistory.ActiveSheet.Cells[e.Row, 3].Text, "3") <= 0)
                {
                    //clsPublic.GstrRetValue = SSHistory.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                    FrmHcResultView = new frmHcResultView("HEA",SSHistory.ActiveSheet.Cells[e.Row, 2].Text.Trim().To<long>()); //WrtNo
                    FrmHcResultView.StartPosition = FormStartPosition.CenterScreen;
                    FrmHcResultView.ShowDialog(this);

                }
            }
            else if (sender == SSJong)
            {
                long nWRTNO = 0;

                nWRTNO = SSJong.ActiveSheet.Cells[e.Row, e.Column].Text.To<long>();

                if (nWRTNO == 0)
                {
                    return;
                }

                if (FnWRTNO == nWRTNO)
                {
                    return;
                }

                fn_Screen_Clear();
                txtWrtNo.Text = nWRTNO.To<string>();
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

                nWRTNO = SSList.ActiveSheet.Cells[e.Row, 0].Text.To<long>();
                strYear = SSList.ActiveSheet.Cells[e.Row, 5].Text;
                strGjJong = SSList.ActiveSheet.Cells[e.Row, 3].Text.Trim();

                fn_Screen_Clear();

                txtWrtNo.Text = nWRTNO.To<string>();
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
                    lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
                    SendKeys.Send("{TAB}");
                }
                else if (sender == txtWrtNo)
                {
                    if (txtWrtNo.Text.IsNullOrEmpty()) return;

                    nWrtNo = txtWrtNo.Text.To<long>();
                    fn_Screen_Clear();
                    txtWrtNo.Text = nWrtNo.To<string>();
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
            //if (sender != chkRoom4)
            //{
            //    if (chkRoom0.Checked == true || chkRoom1.Checked == true || chkRoom2.Checked == true || chkRoom3.Checked == true || chkRoom5.Checked == true)
            //    {
            //        chkRoom4.Checked = false;
            //    }
            //}
            //else if (sender == chkRoom4)
            //{
            //    if (chkRoom4.Checked == true)
            //    {
            //        chkRoom0.Checked = false;
            //        chkRoom1.Checked = false;
            //        chkRoom2.Checked = false;
            //        chkRoom3.Checked = false;
            //        chkRoom5.Checked = false;
            //        return;
            //    }
            //}
            if (sender == chkX1_12)
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
            else if (sender == txtLastMedHis)
            {
                txtLastMedHis.SelectionStart = 0;
                txtLastMedHis.SelectionLength = txtLastMedHis.Text.Length;
            }
            else if (sender == txtGajok)
            {
                txtGajok.SelectionStart = 0;
                txtGajok.SelectionLength = txtGajok.Text.Length;
            }
            else if (sender == txtGiinsung)
            {
                txtGiinsung.SelectionStart = 0;
                txtGiinsung.SelectionLength = txtGiinsung.Text.Length;
            }

        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (btnLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                    }
                }
            }
            else if (sender == txtPanDrNo)
            {
                if (e.KeyChar == (char)13)
                {
                    lblDrName.Text = hb.READ_License_DrName(txtPanDrNo.Text.To<long>());
                    SendKeys.Send("{Tab}");
                }
            }
            else if (sender == txtWrtNo)
            {
                long nWrtNo = 0;

                if (e.KeyChar == 13)
                {
                    if (txtWrtNo.Text.IsNullOrEmpty()) return;

                    nWrtNo = txtWrtNo.Text.To<long>();

                    fn_Screen_Clear();
                    FnWRTNO = nWrtNo;
                    txtWrtNo.Text = FnWRTNO.To<string>();

                    fn_Screen_Display();
                }
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
            ssPatInfo.ActiveSheet.RowCount = 1;
            sp.SetfpsRowHeight(ssPatInfo, 26);
            sp.Spread_All_Clear(SS2);
            sp.Spread_All_Clear(SS3);
            sp.Spread_All_Clear(SSJong);
            SSJong.ActiveSheet.RowCount = 2;
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

            for (int i = 0; i <= 4; i++)
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
            lblSpc1.Visible = true;
            lblLivingHabit1.Visible = true;
            //lblSpc1.Text = "";
            //lblLivingHabit1.Text = "";
            lblGen20.Visible = true;
            lblGen21.Visible = true;
            lblEndo.Visible = true;
            txtLastMedHis.Visible = false;
            lblLivingHabit.Visible = true;
            lblTitle0.Text = "일반상태";
            lblTitle1.Text = "외상후유증";
            lblTitle2.Text = "식사여부";
            lblTitleBreast.Text = "생활습관개선";

            //Panel일반상태 => lblGeneral => pnlBodySymptom
            //SSPanel6 => pnlStatus1 => lblPTSD
            //SSPanel7 => pnlStatus2 => lblMealState
            //SSPanel8 => pnlStatus3 => lblLivingHabit

            pnlBodySymptom.Visible = false;
            txtCancerHis.Visible = false;
            txtStomachBowlLiver.Visible = false;
            //pnlCancerHis.Visible = false;
            //pnlStomachBowlLiver.Visible = false;
            pnlChestCervical.Visible = false;

            lblStatus1.Visible = true;      //일반상태
            lblPTSD.Visible = true;         //외상후유증
            lblMealState.Visible = true;    //식사여부

            //tab1.Visible = false;
            tab2.Visible = false;
            tab3.Visible = false;
            tab4.Visible = false;
            tab5.Visible = false;
            tab6.Visible = false;
            tab7.Visible = false;

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
                CheckBox chkMedcine = (Controls.Find("chkMedcine" + i.ToString(), true)[0] as CheckBox);
                chkMedcine.Checked = false;
            }

            txtMedcineEtc.Text = "";
            txtMedAspirin.Text = "";
            txtAntiCoagulant.Text = "";

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

            for (int i = 0; i <= 5; i++)
            {
                SS13.ActiveSheet.Cells[i, 1].Text = "";
                SS13.ActiveSheet.Cells[i, 2].Text = "";
                SS14.ActiveSheet.Cells[i, 1].Text = "";
                SS14.ActiveSheet.Cells[i, 2].Text = "";
            }

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

            tabControlX.SelectedTab = tabX1;

            for (int i = 1; i <= 5; i++)
            {
                TextBox txtX1_ = (Controls.Find("txtX1_" + i.ToString(), true)[0] as TextBox);
                txtX1_.Text = "";
            }

            txtX1_1.Enabled = false;
            txtX1_2.Enabled = false;
            txtX1_4.Enabled = false;
            txtJikjong.Enabled = false;

            chkX1_10.Checked = false;
            chkX1_11.Checked = false;
            chkX1_12.Checked = false;

            chkX1_20.Checked = false;
            chkX1_21.Checked = false;
            chkX1_22.Checked = false;

            chkX1_40.Checked = false;
            chkX1_41.Checked = false;

            chkX40.Checked = false;
            chkX41.Checked = false;
            chkX42.Checked = false;

            for (int i = 1; i <= 3; i++)
            {
                TextBox txtX2_ = (Controls.Find("txtX2_" + i.ToString(), true)[0] as TextBox);
                txtX2_.Text = "";
            }

            //폐암사후상담
            txtLung_Sang1.Text = "";
            txtLung_Sang2.Text = "";

            FbInsomniaMun = false;
            FbinsomniaMun2 = false;
            FbBreastCancerMun = false;
            FbStomachMun = false;
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
