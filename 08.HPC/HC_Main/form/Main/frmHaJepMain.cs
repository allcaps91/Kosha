using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using ComMedLibB;
using ComPmpaLibB;
using ComSupLibB.Com;
using DevComponents.DotNetBar;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using HC_Main.Dto;
using HC_Main.Model;
using HC_Main.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

/// <summary>
/// Group_Exams_Display(); - > 신규 검사항목 Display
/// Display_Jepsu_Detail -> 접수 Display
/// 
/// 코드 추가 및 정리
/// frmHaGroupCode_ssDblclick
/// 
/// 저장 : Data_Save_Process();
/// 저장 체크로직 : Jepsu_Check_Logic(); 검사일정 체크로직
/// </summary>
namespace HC_Main
{
    public partial class frmHaJepMain :Form
    {
        public delegate void SetJepsuView(object sender, EventArgs e);
        public static event SetJepsuView rSetJepsuView;

        private string[] FstrChkExam = new string[12];
        private string[] FstrChkExamName = new string[12];
        private string[] FstrContrast = new string[12];
        private string[] strMsg = new string[10];

        string fstrNew = string.Empty;
        string FstrPtnoOld = string.Empty;
        string FstrOldSdate = string.Empty;
        string FstrPtno = string.Empty;
        string FstrBuildNo = string.Empty;
        string FstrJong = string.Empty;
        string FstrBuRate = string.Empty;
        string FstrGBSTS  = string.Empty;
        string FstrAddExamYN = string.Empty;
        string[,] FstrSTime = new string[16, 2];
        #region 개인별 검진 History 연계
        string FsHisJepDate;
        string FsHisPtNo;
        #endregion

        long FnPano = 0;
        long FnIEMunNo = 0;
        long FnWRTNO = 0;
        long GnWRTNO = 0;

        long FnTotAmt = 0,   FnLtdAmt = 0;
        long FnBoninAmt = 0, FnCardAmt = 0;
        long FnIpGumAmt = 0, FnHalinAmt = 0;
        long FnGongdanAmt = 0;
        long FnHisWRTNO = 0;

        bool FbCall = false;
        bool bolSort = false;
        int FnSMS_CNT = 0;

        List<HEA_EXJONG> haExjong = null;
        List<HEA_GAMCODE> haGamCode = null;

        List<string> lstOldExams = null;
        List<string> lstGroupExams = null;
        List<long> lstGaResvLtdCodes = null;
        List<string> lstTicketSMS = null;

        List<string> lstDelExam = null;

        List<READ_SUNAP_ITEM> suInfo = new List<READ_SUNAP_ITEM>();
        List<GROUPCODE_EXAM_DISPLAY> grpExam  = new List<GROUPCODE_EXAM_DISPLAY>();    //검사코드 항목
        HIC_SUNAP sunap  = new HIC_SUNAP();         //검진1
        HEA_JEPSU HaJEPSU = new HEA_JEPSU();

        clsHcFunc cHF = null;
        clsSpread cSpd = null;
        clsHaBase cHB = null;
        clsHcMain cHcMain = null;
        clsHcMainFunc cHMF = null;
        clsHcOrderSend cHOS = null;
        clsHcPrint cHPrt = null;
        clsAlimTalk cATK = null;
        clsComSup sup = null;

        HIC_LTD LtdHelpItem = null;
        HEA_CODE CodeHelpItem = null;
        HIC_PATIENT Hpatient = null;
        HEA_GROUPCODE HaGrpCD = null;
        System.Windows.Forms.ToolTip toolTip = null;

        HicLtdService hicLtdService = null;
        HeaExjongService heaExjongService = null;
        HeaGamcodeService heaGamcodeService = null;
        HicMemoService hicMemoService = null;
        HicPatientService hicPatientService = null;
        ComHpcLibBService comHpcLibBService = null;
        BasPatientService basPatientService = null;
        HeaExcelService heaExcelService = null;
        HicJepsuHeaExjongService hicJepsuHeaExjongService = null;
        HicIeMunjinNewService hicIeMunjinNewService = null;
        HeaJepsuService heaJepsuService = null;
        HicJepsuWorkService hicJepsuWorkService = null;
        HicJepsuService hicJepsuService = null;
        HicCancerResv2Service hicCancerResv2Service = null;
        HeaSunapdtlService heaSunapdtlService = null;
        ReadSunapItemService readSunapItemService = null;
        GroupCodeExamDisplayService groupCodeExamDisplayService = null;
        HicSunapService hicSunapService = null;
        HicGroupexamGroupcodeExcodeService hicGroupexamGroupcodeExcodeService = null;
        HicWaitService hicWaitService = null;
        HicBcodeService hicBcodeService = null;
        HeaResvExamService heaResvExamService = null;
        HeaGroupexamExcodeService heaGroupexamExcodeService = null;
        HeaResultService heaResultService = null;
        HicExcodeService hicExcodeService = null;
        HicResultService hicResultService = null;
        HicResultExCodeService hicResultExCodeService = null;
        CardApprovCenterService cardApprovCenterService = null;
        HeaTicketService heaTicketService = null;
        EtcSmsService etcSmsService = null;
        WorkNhicService workNhicService = null;
        HicPrivacyAcceptNewService hicPrivacyAcceptNewService = null;
        HicConsentService hicConsentService = null;
        HeaSunapService heaSunapService = null;
        EtcAlimTalkService etcAlimTalkService = null;
        HeaGroupcodeService heaGroupcodeService = null;
        HicCodeService hicCodeService = null;
        HeaCodeService heaCodeService = null;
        HeaJepsuPatientService heaJepsuPatientService = null;
        EndoJupmstService endoJupmstService = null;
        XrayDetailService xrayDetailService = null;
        ExamSpecmstService examSpecmstService = null;

        frmHcPermission frmHcPermission = null;         //동의서 폼
        frmHcEmrPermission frmHcEmrPermission = null;   //전자동의서 폼
        frmViewResult FrmViewResult = null;             //OCS 검사 폼

        public frmHaJepMain()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        private void SetControl()
        {
            cHF = new clsHcFunc();
            cSpd = new clsSpread();
            cHB = new clsHaBase();
            cHcMain = new clsHcMain();
            cHMF = new clsHcMainFunc();
            cHOS = new clsHcOrderSend();
            cHPrt = new clsHcPrint();
            cATK = new clsAlimTalk();
            sup = new clsComSup();
            toolTip = new System.Windows.Forms.ToolTip();

            lstOldExams = new List<string>();
            lstGroupExams = new List<string>();
            lstGaResvLtdCodes = new List<long>();
            lstTicketSMS = new List<string>();
            lstDelExam = new List<string>();

            LtdHelpItem = new HIC_LTD();
            CodeHelpItem = new HEA_CODE();
            Hpatient = new HIC_PATIENT();
            HaGrpCD = new HEA_GROUPCODE();

            hicLtdService = new HicLtdService();
            heaExjongService = new HeaExjongService();
            heaGamcodeService = new HeaGamcodeService();
            hicMemoService = new HicMemoService();
            hicPatientService = new HicPatientService();
            comHpcLibBService = new ComHpcLibBService();
            basPatientService = new BasPatientService();
            heaExcelService = new HeaExcelService();
            hicJepsuHeaExjongService = new HicJepsuHeaExjongService();
            hicIeMunjinNewService = new HicIeMunjinNewService();
            heaJepsuService = new HeaJepsuService();
            hicJepsuWorkService = new HicJepsuWorkService();
            hicJepsuService = new HicJepsuService();
            hicCancerResv2Service = new HicCancerResv2Service();
            heaSunapdtlService = new HeaSunapdtlService();
            readSunapItemService = new ReadSunapItemService();
            groupCodeExamDisplayService = new GroupCodeExamDisplayService();
            hicSunapService = new HicSunapService();
            hicGroupexamGroupcodeExcodeService = new HicGroupexamGroupcodeExcodeService();
            hicWaitService = new HicWaitService();
            hicBcodeService = new HicBcodeService();
            heaResvExamService = new HeaResvExamService();
            heaGroupexamExcodeService = new HeaGroupexamExcodeService();
            heaResultService = new HeaResultService();
            hicExcodeService = new HicExcodeService();
            hicResultService = new HicResultService();
            hicResultExCodeService = new HicResultExCodeService();
            cardApprovCenterService = new CardApprovCenterService();
            heaTicketService = new HeaTicketService();
            etcSmsService = new EtcSmsService();
            workNhicService = new WorkNhicService();
            hicPrivacyAcceptNewService = new HicPrivacyAcceptNewService();
            hicConsentService = new HicConsentService();
            heaSunapService = new HeaSunapService();
            etcAlimTalkService = new EtcAlimTalkService();
            heaGroupcodeService = new HeaGroupcodeService();
            hicCodeService = new HicCodeService();
            heaCodeService = new HeaCodeService();
            heaJepsuPatientService = new HeaJepsuPatientService();
            endoJupmstService = new EndoJupmstService();
            xrayDetailService = new XrayDetailService();
            examSpecmstService = new ExamSpecmstService();

            frmHcPermission = new frmHcPermission("HEA");
            frmHcEmrPermission = new frmHcEmrPermission("HEA");
            

            haExjong = heaExjongService.FindAll();
            cboJONG.SetItems(haExjong, "NAME", "CODE", "", "");

            cboHalinGye.Items.Clear();
            cboHalinGye.Items.Add("");
            haGamCode = heaGamcodeService.GetListItems(false);
            cboHalinGye.SetItems(haGamCode, "NAME", "CODE", "", "");

            //성별
            cboSex.Items.Clear();
            cboSex.Items.Add("M");
            cboSex.Items.Add("F");
            cboSex.SelectedIndex = 0;

            #region 수검자 메모 Spread
            ssETC.Initialize();
            ssETC.AddColumn("삭제",       nameof(HIC_MEMO.GBDEL),     34, FpSpreadCellType.CheckBoxCellType);
            ssETC.AddColumn("구분",       nameof(HIC_MEMO.JOBGBN),    42, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = false });
            ssETC.AddColumn("입력시각",   nameof(HIC_MEMO.ENTTIME),   80, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, BackColor = Color.FromArgb(192, 255, 192) });
            ssETC.AddColumn("내용",       nameof(HIC_MEMO.MEMO),     360, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsMulti = true });
            ssETC.AddColumn("작업자사번", nameof(HIC_MEMO.JOBSABUN),  48, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            ssETC.AddColumn("작업자명",   nameof(HIC_MEMO.JOBNAME),   80, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { });
            ssETC.AddColumn("CHANGE",     "",                         30, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            ssETC.AddColumn("ROWID",      nameof(HIC_MEMO.RID),       30, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            ssETC.AddColumn("PANO",       nameof(HIC_MEMO.PANO),      30, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            ssETC.AddColumn("PTNO",       nameof(HIC_MEMO.PTNO),      30, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            #endregion

            #region 그룹코드 정보
            ssGroup.Initialize(new SpreadOption { ColumnHeaderHeight = 34 });
            ssGroup.AddColumnCheckBox("제외",     "",                               28, new CheckBoxFlagEnumCellType<IsDeleted>() { IsHeaderCheckBox = false }).ButtonClick += eSpdGroupCD_ButtonClick;
            ssGroup.AddColumn("묶음코드",         nameof(READ_SUNAP_ITEM.GRPCODE),  60, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, BackColor = Color.FromArgb(246, 234, 210) });
            ssGroup.AddColumn("묶음코드명",       nameof(READ_SUNAP_ITEM.GRPNAME), 160, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            ssGroup.AddColumn("금액",             nameof(READ_SUNAP_ITEM.AMT),      70, FpSpreadCellType.NumberCellType,  new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Right });
            ssGroup.AddColumn("부담율",           nameof(READ_SUNAP_ITEM.GBSELF),   40, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { TextMaxLength = 1, BackColor = Color.FromArgb(210, 222, 243) });
            ssGroup.AddColumnCheckBox("할인구분", nameof(READ_SUNAP_ITEM.GBHALIN),  32, new CheckBoxFlagEnumCellType<IsActive>() { IsHeaderCheckBox = false }).ButtonClick += eSpdGroupCD_ButtonClick;
            ssGroup.AddColumn("ROWID",            nameof(READ_SUNAP_ITEM.RID),      32, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsVisivle = false });
            ssGroup.AddColumn("본인금액", nameof(READ_SUNAP_ITEM.BONINAMT), 70, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Right });
            ssGroup.AddColumn("회사금액", nameof(READ_SUNAP_ITEM.LTDAMT), 70, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = true, Aligen = CellHorizontalAlignment.Right });
            #endregion

            #region 검진 검사항목 코드 Spread
            ssExam.Initialize();
            ssExam.AddColumnCheckBox("제외",     "", 28, new CheckBoxFlagEnumCellType<IsDeleted>() { IsHeaderCheckBox = false }).ButtonClick += eSpdExamCD_ButtonClick;
            ssExam.AddColumn("검사코드",   nameof(GROUPCODE_EXAM_DISPLAY.EXCODE),     74, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, isFilter = true, BackColor = Color.FromArgb(246, 234, 210) });
            ssExam.AddColumn("검사명",     nameof(GROUPCODE_EXAM_DISPLAY.EXNAME),    280, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, isFilter = true });
            ssExam.AddColumn("묶음코드",   nameof(GROUPCODE_EXAM_DISPLAY.GROUPCODE), 138, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsVisivle = false });
            ssExam.AddColumn("검사예약",   nameof(GROUPCODE_EXAM_DISPLAY.ETCEXAM),    44, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, IsVisivle = false });

            //ETCEXAM
            #endregion

            rdoEndo1.SetOptions(new RadioButtonOption { DataField = nameof(HEA_JEPSU.ENDOGBN), CheckValue = "1" });
            rdoEndo2.SetOptions(new RadioButtonOption { DataField = nameof(HEA_JEPSU.ENDOGBN), CheckValue = "2" });

            #region ComboBox Setting
            int nYY = int.Parse(VB.Left(DateTime.Now.ToShortDateString(), 4));

            for (int i = 0; i < 5; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYY));
                nYY -= 1;
            }

            cboYear.SelectedIndex = 0;

            cboSTime.Items.Clear();
            cboSTime.Items.Add("07:20");
            cboSTime.Items.Add("07:30");
            cboSTime.Items.Add("07:40");
            cboSTime.Items.Add("07:50");
            cboSTime.Items.Add("08:00");
            cboSTime.Items.Add("08:10");
            cboSTime.Items.Add("08:20");
            cboSTime.Items.Add("08:30");
            cboSTime.Items.Add("08:40");
            cboSTime.Items.Add("08:50");
            cboSTime.Items.Add("09:00");
            cboSTime.Items.Add("09:30");
            cboSTime.Items.Add("13:00");
            cboSTime.Items.Add("13:20");
            cboSTime.Items.Add("14:00");
            cboSTime.Items.Add("14:30");

            cboBuRate.Items.Clear();
            cboBuRate.Items.Add("1.본인100%");
            cboBuRate.Items.Add("2.회사100%");
            cboBuRate.Items.Add("3.본인,회사50%");
            cboBuRate.Items.Add("4.임의금액 부담");
            //cboBuRate.Items.Add("4.본인,회사 일부부담");
            //cboBuRate.Items.Add("5.임의금액 부담");
            #endregion

            #region 검진시간별 인원설정
            FstrSTime[0, 0] = "07:20";  FstrSTime[0, 1] = "3";
            FstrSTime[1, 0] = "07:30";  FstrSTime[1, 1] = "12";
            FstrSTime[2, 0] = "07:40";  FstrSTime[2, 1] = "0";
            FstrSTime[3, 0] = "07:50";  FstrSTime[3, 1] = "10";
            FstrSTime[4, 0] = "08:00";  FstrSTime[4, 1] = "5";
            FstrSTime[5, 0] = "08:10";  FstrSTime[5, 1] = "0";
            FstrSTime[6, 0] = "08:20";  FstrSTime[6, 1] = "0";
            FstrSTime[7, 0] = "08:30";  FstrSTime[7, 1] = "10";
            FstrSTime[8, 0] = "08:40";  FstrSTime[8, 1] = "0";
            FstrSTime[9, 0] = "08:50";  FstrSTime[9, 1] = "0";
            FstrSTime[10, 0] = "09:00"; FstrSTime[10, 1] = "10";
            FstrSTime[11, 0] = "09:30"; FstrSTime[11, 1] = "5";
            FstrSTime[12, 0] = "13:00"; FstrSTime[12, 1] = "0";
            FstrSTime[13, 0] = "13:20"; FstrSTime[13, 1] = "0";
            FstrSTime[14, 0] = "14:00"; FstrSTime[14, 1] = "0";
            FstrSTime[15, 0] = "14:30"; FstrSTime[15, 1] = "0";
            #endregion

        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.FormClosing += new FormClosingEventHandler(eDisevent);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnSave_Memo.Click += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click += new EventHandler(eBtnClick);
            this.btnHelp_Mail.Click += new EventHandler(eBtnClick);
            this.btnHelp_Mail1.Click += new EventHandler(eBtnClick);
            this.btnSabun_Help.Click += new EventHandler(eBtnClick);
            this.btnExamSearch.Click += new EventHandler(eBtnClick);
            this.btnAmt.Click += new EventHandler(eBtnClick);
            this.btnGrouCode.Click += new EventHandler(eBtnClick);
            this.btnGrouCode_Search.Click += new EventHandler(eBtnClick);
            this.btnGView.Click += new EventHandler(eBtnClick);
            this.btnLtdRemark.Click += new EventHandler(eBtnClick);
            this.btnSearch_Nhic.Click += new EventHandler(eBtnClick);
            this.btnNhic_New.Click += new EventHandler(eBtnClick);
            this.btnNhic_Suga.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnCall.Click += new EventHandler(eBtnClick);
            this.btnMenualCall.Click += new EventHandler(eBtnClick);
            this.btnReCall.Click += new EventHandler(eBtnClick);
            this.btnFamilly.Click += new EventHandler(eBtnClick);
            this.btnSearch_His.Click += new EventHandler(eBtnClick);
            this.btnOCS.Click += new EventHandler(eBtnClick);
            this.btnEMR.Click += new EventHandler(eBtnClick);
            this.btnDrug.Click += new EventHandler(eBtnClick);
            this.btnLtdExam.Click += new EventHandler(eBtnClick);
            this.btnCard.Click += new EventHandler(eBtnClick);
            this.btnCash.Click += new EventHandler(eBtnClick);
            this.btnResReciept.Click += new EventHandler(eBtnClick);
            this.btnLtdJuso.Click += new EventHandler(eBalloonBtn);
            this.btnDur.Click += new EventHandler(eBtnClick);
            this.btnReset.Click += new EventHandler(eBtnClick);
            this.btnReset1.Click += new EventHandler(eBtnClick);
            //기타출력버튼
            this.btnXConfirm.Click += new EventHandler(eBtnXClick);
            this.btnXRecReport.Click += new EventHandler(eBtnXClick);
            this.btnXBarCode.Click += new EventHandler(eBtnXClick);
            this.btnXJepsu.Click += new EventHandler(eBtnXClick);
            this.btnDietTicket.Click += new EventHandler(eBtnXClick);

            this.btnJepView.Click += new EventHandler(eJepsuView);

            this.chkCall.CheckedChanged += new EventHandler(eChkChange);
            this.chkRES12_1.CheckedChanged += new EventHandler(eChkChange);
            this.chkRES12_2.CheckedChanged += new EventHandler(eChkChange);
            this.chkRES12_3.CheckedChanged += new EventHandler(eChkChange);
            this.chkGongDan.CheckedChanged += new EventHandler(eChkChange);
            this.cboJONG.KeyDown += new KeyEventHandler(eCboKeyDown);
            this.cboHalinGye.KeyDown += new KeyEventHandler(eCboKeyDown);
            this.cboHalinGye.TextChanged += new EventHandler(eCboTxtChanged);
            this.cboSTime.TextChanged += new EventHandler(eCboTxtChanged);
            this.dtpSDate.TextChanged += new EventHandler(eCboTxtChanged);

            this.rdoSTS1.CheckedChanged += new EventHandler(eStsRdoCheckChange);
            this.rdoSTS2.CheckedChanged += new EventHandler(eStsRdoCheckChange);
            this.rdoEndo1.CheckedChanged += new EventHandler(eEndoRdoCheckChange);
            this.rdoEndo2.CheckedChanged += new EventHandler(eEndoRdoCheckChange);
            this.rdoMemo1.CheckedChanged += new EventHandler(eMemordoChkChange);
            this.rdoMemo2.CheckedChanged += new EventHandler(eMemordoChkChange);
            this.rdoMemo3.CheckedChanged += new EventHandler(eMemordoChkChange);
            this.rdoRES12.CheckedChanged += new EventHandler(eRdoCheckChanged);
            this.rdoRES14.CheckedChanged += new EventHandler(eRdoCheckChanged);
            this.rdoRES15.CheckedChanged += new EventHandler(eRdoCheckChanged);

            this.ssETC.EditModeOff += new EventHandler(eSpdEditOff);
            this.ssETC.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);

            this.ssGroup.EditModeOff += new EventHandler(eSpdEditOff);

            this.ssExam.CellClick += new CellClickEventHandler(eSpdCellClick);

            this.txtLtdCode.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtJikSabun.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtJumin1.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtJumin1.TextChanged += new EventHandler(eTxtChanged);
            this.txtJumin2.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtPtno.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtGroupCode_Search.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtCardAmt.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtCashAmt.KeyDown += new KeyEventHandler(eTxtKeyDown);
            this.txtKeyNo.KeyDown += new KeyEventHandler(eTxtKeyDown);

            this.timer1.Tick += new EventHandler(eTimerTick);
            this.lblIEMunjin.DoubleClick += new EventHandler(eLblDblClick);
            this.lblUCODES.MouseClick += new MouseEventHandler(eLblMouseClick);
            this.lblRES1.DoubleClick += new EventHandler(eLblDblClick);

            this.txtJuso11.MouseUp += new MouseEventHandler(elblMouseUp);

            frmHaJepsuView.rSetGstrValue += new frmHaJepsuView.SetGstrValue(eJepList_DblClick);
            frmHaJepsuView.rSetGstrFamValue += new frmHaJepsuView.SetGstrFamValue(eJepsuView_Fam);
            frmHcPanPersonResult.rSetHaJepsuGstrValue += new frmHcPanPersonResult.SetHaJepsuGstrValue(PatHis_Value);
            frmHcPanPersonResult.rSetHaJepsuBtnRef += new frmHcPanPersonResult.SetHaJepsuBtnRef(ePtno_Event);
            frmHcExcelList.rSetHaJepsuGstrValue += new frmHcExcelList.SetHaJepsuGstrValue(eHaExcelList_DblClick);
            frmHaResvCalendar.rSndMsg += new frmHaResvCalendar.rSendMsg(eHaCalenderList_DblClick);
        }

        private void eBalloonBtn(object sender, EventArgs e)
        {
            if (VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0) == 0)
            {
                return;
            }

            long nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0);

            string strLtdJuso = hicLtdService.GetJusoByCode(nLtdCode);

            if (strLtdJuso.IsNullOrEmpty())
            {
                strLtdJuso = "회사 주소 등록 안됨";
            }

            balloonTipHover.Enabled = true;

            Balloon b = new Balloon();
            b.Style = eBallonStyle.Office2007Alert;
            b.CaptionText = "회사 주소";
            b.Text = strLtdJuso;
            b.AlertAnimation = eAlertAnimation.TopToBottom;
            b.Width = 320;
            b.Height = 100;
            //b.AutoResize();

            b.AutoClose = true;
            b.AutoCloseTimeOut = 5;
            b.Owner = this;
            b.Show(btnLtdJuso, false);
        }

        private void eLblMouseClick(object sender, MouseEventArgs e)
        {
            if (lblUCODES.Text.Trim() == "")
            {
                return;
            }

            balloonTipHover.Enabled = true;

            Balloon b = new Balloon();
            b.Style = eBallonStyle.Office2007Alert;
            b.CaptionText = "유해인자 목록";
            b.Text = lblUCODES.Text;
            b.AlertAnimation = eAlertAnimation.TopToBottom;
            b.Width = 520;
            b.Height = 180;
            //b.AutoResize();

            b.AutoClose = true;
            b.AutoCloseTimeOut = 5;
            b.Owner = this;
            b.Show(lblUCODES, false);
        }

        private void eLblDblClick(object sender, EventArgs e)
        {
            if (sender == lblRES1)
            {
                Clear_Result_Reciept_Control();  
            }
            else if (sender == lblIEMunjin)
            {
                frmHcIEMunjin frm = new frmHcIEMunjin("", txtSName.Text, 0, FnIEMunNo, txtPtno.Text, "", "");
                frm.ShowDialog();
            }
        }

        private void Clear_Result_Reciept_Control()
        {
            chkRES12_1.Checked = false;
            chkRES12_2.Checked = false;
            chkRES12_3.Checked = false;
            rdoRES12.Checked = false;
            rdoRES14.Checked = false;
            rdoRES15.Checked = false;
        }

        private void eSpdCellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader)
            {
                clsSpread.gSpdSortRow((FpSpread)sender, e.Column, ref bolSort, true);
            }
        }

        private void eRdoCheckChanged(object sender, EventArgs e)
        {
            if (sender == rdoRES12)
            {
                if (rdoRES12.Checked)
                {
                    chkRES12_1.Checked = true;
                }
            }
            else if (sender == rdoRES14)
            {
                if (rdoRES14.Checked)
                {
                    chkRES12_1.Checked = false;
                    chkRES12_2.Checked = false;
                    chkRES12_3.Checked = false;
                }
            }
            else if (sender == rdoRES15)
            {
                if (rdoRES15.Checked)
                {
                    chkRES12_1.Checked = false;
                    chkRES12_2.Checked = false;
                    chkRES12_3.Checked = false;
                }
            }
        }

        private void ePtno_Event(string argPtno)
        {
            try
            {
                if (!argPtno.IsNullOrEmpty())
                {
                    Screen_Clear();
                    dtpJepDate.Text = DateTime.Now.ToShortDateString();
                    cboYear.Text = DateTime.Now.Year.To<string>("");
                    txtPtno.Text = argPtno;
                    eTxtKeyDown(txtPtno, new KeyEventArgs(Keys.Enter));
                    txtLtdCode.Focus();
                }

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void eHaCalenderList_DblClick(string strPano, string strSDate)
        {
            try
            {
                if (!strPano.IsNullOrEmpty())
                {
                    Screen_Clear();
                    txtPtno.Text = strPano;

                    HEA_JEPSU hEA_JEPSU = heaJepsuService.GetItembyPtNoBdate(strPano, strSDate);

                    dtpJepDate.Text = hEA_JEPSU.JEPDATE;
                    cboYear.Text = VB.Left(hEA_JEPSU.JEPDATE, 4);

                    //검진 수검자 정보
                    Display_HeaPatient_Info(strPano, cboYear.Text);

                    //검진 접수 Display Main
                    Display_Jepsu_Main(txtPtno.Text);
                }

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void eSpdExamCD_ButtonClick(object sender, EditorNotifyEventArgs e)
        {
            ssExam.DeleteRow(e.Row);
        }

        private void eSpdGroupCD_ButtonClick(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column == 0)
            {
                ssGroup.DeleteRow(e.Row);

                btnAmt.PerformClick();
                GroupExams_Set(suInfo);    //검사항목코드 재설정
            }
            else if (e.Column == 5)
            {
                if (ssGroup.ActiveSheet.Cells[e.Row, 5].Text == "Y")
                {
                    suInfo[e.Row].GBHALIN = "Y";
                }
                else
                {
                    suInfo[e.Row].GBHALIN = "N";
                }

                Gesan_HalinAmt(VB.Left(cboHalinGye.Text, 3), VB.Left(cboJONG.Text, 2), VB.Pstr(txtLtdCode.Text, ".", 1), cboSex.Text);

                btnAmt.PerformClick();
            }
        }

        private void GroupExams_Set(List<READ_SUNAP_ITEM> lstRSI)
        {
            List<GROUPCODE_EXAM_DISPLAY> lstExam = new List<GROUPCODE_EXAM_DISPLAY>();
            List<string> lstGrpCD = new List<string>();

            for (int i = 0; i < lstRSI.Count; i++)
            {
                if (lstRSI[i].RowStatus != RowStatus.Delete)
                {
                    //선택한 코드 적용
                    lstGrpCD.Add(lstRSI[i].GRPCODE);
                }
            }

            if (!lstGrpCD.IsNullOrEmpty() && lstGrpCD.Count > 0)
            {
                lstExam = groupCodeExamDisplayService.GetHeaListByGroupCode(lstGrpCD);
            }
            
            grpExam.Clear();
            grpExam.AddRange(lstExam);

            //분진검사, 수면내시경의 경우 중복검사 제외
            Check_Exam_Except(lstGrpCD, ref grpExam);

            ssExam.DataSource = null;
            ssExam.SetDataSource(grpExam);

            clsSpread.gSpdSortRow(ssExam, 0, ref bolSort, true);
            //clsHcVariable.LSTHaRESEXAM.Clear();
        }

        private void eHaExcelList_DblClick(string hEA_EXCEL)
        {
            long nLtdCode = 0;
            string strJumin1 = "";
            string strJumin2 = "";
            string strAESJumin = "";
            string strSName = "";
            string strRowid = "";
            string strFDate = "";
            string strTDate = "";
            string strYear = "";
            string strSabun = "";
            string strMessage = "";


            try
            {
                if (!hEA_EXCEL.IsNullOrEmpty())
                {
                    Screen_Clear();

                    txtJumin1.Text = VB.Pstr(hEA_EXCEL, "{}", 1);
                    txtJumin2.Text = VB.Pstr(hEA_EXCEL, "{}", 2);
                    txtSName.Text = VB.Pstr(hEA_EXCEL, "{}", 3);
                    txtLtdSabun.Text = VB.Pstr(hEA_EXCEL, "{}", 8);
                    strSName = VB.Pstr(hEA_EXCEL, "{}", 3);
                    nLtdCode = VB.Pstr(hEA_EXCEL, "{}", 4).To<long>(0);
                    strYear = VB.Pstr(hEA_EXCEL, "{}", 5);

                    if (txtJumin1.Text.Trim() != "" && txtJumin2.Text.Trim() == "")
                    {
                        strJumin2 = hicPatientService.GetJumin2BySnameJuminLikeLtdCode(strSName, txtJumin1.Text);

                        if (!strJumin2.IsNullOrEmpty())
                        {
                            txtJumin2.Text = VB.Right(clsAES.DeAES(strJumin2), 7);
                        }
                    }

                    if (nLtdCode == 2177 && txtJumin1.Text.Trim() == "" && txtJumin2.Text.Trim() == "") 
                    {
                        strSabun =VB.Pstr(hEA_EXCEL, "{}", 8);
                    }

                    if (!strSabun.IsNullOrEmpty())
                    {
                        List <HIC_PATIENT> list = hicPatientService.GetItembySabun(strSabun);
                        for(int i = 0; i<list.Count; i++)
                        {
                            if (!list.IsNullOrEmpty())
                            {
                                if ( list.Count == 1)
                                {
                                    txtJumin1.Text = VB.Left(list[i].JUMIN, 6);
                                    txtJumin2.Text = VB.Right(clsAES.DeAES(list[i].JUMIN2), 7);
                                }
                                else if (list.Count == 0)
                                {
                                    strMessage = "포스코직원이며, 회사사번이 없는대상입니다.";
                                }
                                else if ( list.Count >1)
                                {
                                    if (strMessage.IsNullOrEmpty())
                                    {
                                        strMessage = "이름: " + list[i].SNAME + " / " + "등록번호" + list[i].PTNO + " / " + "생년월일: " + VB.Left(list[i].JUMIN, 6) + ComNum.VBLF;
                                    }
                                    else
                                    {
                                        strMessage += strMessage = "이름: " + list[i].SNAME + " / " + "등록번호" + list[i].PTNO + " / " + "생년월일: " + VB.Left(list[i].JUMIN, 6)+ ComNum.VBLF;
                                    }
                                }
                            }
                        } 
                    }

                    if (!strMessage.IsNullOrEmpty())
                    {
                        MessageBox.Show(strMessage, "오류", MessageBoxButtons.OK);
                        return;
                    }
                    
                    //주민등록번호가 공란이면 접수/예약내역 안찾음
                    if (txtJumin1.Text.Trim() == "" && txtJumin2.Text.Trim() == "")
                    {
                        return;
                    }

                    //접수/예약이 되었으면 표시함
                    strFDate = strYear + "-01-01";
                    strTDate = strYear + "-12-31";
                    if (txtJumin2.Text.Trim() != "")
                    {
                        strAESJumin = clsAES.AES(txtJumin1.Text + txtJumin2.Text);
                    }
                    else
                    {
                        strJumin1 = txtJumin1.Text;
                    }
                    
                    HEA_JEPSU hEA_JEPSU = heaJepsuPatientService.GetWrtnoBySDateSNameJumin2(strFDate, strTDate, strSName, strAESJumin, strJumin1);

                    if (!hEA_JEPSU.IsNullOrEmpty())
                    {
                        FnHisWRTNO = hEA_JEPSU.WRTNO;
                        txtPtno.Text = hEA_JEPSU.PTNO;

                        dtpJepDate.Text = hEA_JEPSU.JEPDATE;
                        cboYear.Text = VB.Left(hEA_JEPSU.JEPDATE, 4);

                        //검진 수검자 정보
                        Display_HeaPatient_Info(txtPtno.Text.Trim(), cboYear.Text);

                        //검진 접수 Display Main
                        Display_Jepsu_Main(txtPtno.Text);
                    }
                    else
                    {
                        FnHisWRTNO = 0;
                        cboYear.Text = VB.Left(DateTime.Now.ToShortDateString(), 4);
                        dtpJepDate.Text = DateTime.Now.ToShortDateString();
                        eTxtKeyDown(txtJumin2, new KeyEventArgs(Keys.Enter));
                        if (VB.Pstr(hEA_EXCEL, "{}", 9).Trim() != "") { txtHphone.Text = VB.Pstr(hEA_EXCEL, "{}", 9); }
                        if (VB.Pstr(hEA_EXCEL, "{}", 10).Trim() != "") { txtTel.Text = VB.Pstr(hEA_EXCEL, "{}", 10); }
                        if (VB.Pstr(hEA_EXCEL, "{}", 6).Trim() != "" && VB.Pstr(hEA_EXCEL, "{}", 11).Trim() != "")
                        {
                            txtSosok.Text = VB.Pstr(hEA_EXCEL, "{}", 6);
                        }
                        txtLtdSabun.Text = VB.Pstr(hEA_EXCEL, "{}", 8);
                        txtLtdCode.Text = nLtdCode.To<string>("");
                        if (txtLtdCode.Text.Trim() != "")
                        {
                            txtLtdCode.Text = txtLtdCode.Text + "." + hicLtdService.READ_Ltd_One_Name(txtLtdCode.Text);
                            txtLtdRemark.Text = hicLtdService.GetHaRemarkbyLtdCode(nLtdCode);
                        }

                        txtLtdCode.Focus();
                    }

                    //엑셀파일 내용 초기화
                    for (int i = 0; i < 24; i++) { SS3.ActiveSheet.Cells[i, 1].Text = ""; }

                    strRowid = VB.Pstr(hEA_EXCEL, "{}", 12);

                    if (!strRowid.IsNullOrEmpty())
                    {
                        HEA_EXCEL item = heaExcelService.GetAllbyRowId(strRowid);

                        //엑셀파일의 내용을 별도 시트에 표시함
                        if (!item.IsNullOrEmpty())
                        {
                            SS3.ActiveSheet.Cells[0, 1].Text = strSName;
                            SS3.ActiveSheet.Cells[1, 1].Text = txtJumin1.Text + "-" + txtJumin2.Text;
                            SS3.ActiveSheet.Cells[2, 1].Text = item.HDATE + " " + item.AMPM;
                            SS3.ActiveSheet.Cells[3, 1].Text = item.GJTYPE;
                            SS3.ActiveSheet.Cells[4, 1].Text = item.LTDADDEXAM;
                            SS3.ActiveSheet.Cells[5, 1].Text = item.BONINADDEXAM;
                            SS3.ActiveSheet.Cells[6, 1].Text = item.GAJOKADDEXAM;
                            SS3.ActiveSheet.Cells[7, 1].Text = item.HPHONE;
                            SS3.ActiveSheet.Cells[8, 1].Text = item.JUSO;
                            SS3.ActiveSheet.Cells[9, 1].Text = item.REMARK;
                            SS3.ActiveSheet.Cells[10, 1].Text = item.JNAME;
                            SS3.ActiveSheet.Cells[11, 1].Text = item.REL;
                            SS3.ActiveSheet.Cells[12, 1].Text = item.TEL;
                            SS3.ActiveSheet.Cells[13, 1].Text = item.MCODES;
                            SS3.ActiveSheet.Cells[14, 1].Text = item.GBSAMU;
                            SS3.ActiveSheet.Cells[15, 1].Text = item.GBNIGHT;
                            SS3.ActiveSheet.Cells[16, 1].Text = item.GBNHIC;
                            SS3.ActiveSheet.Cells[17, 1].Text = item.HOSPITAL;
                            SS3.ActiveSheet.Cells[18, 1].Text = item.IPSADATE;
                            SS3.ActiveSheet.Cells[19, 1].Text = item.GKIHO;
                            SS3.ActiveSheet.Cells[20, 1].Text = item.LTDBUSE;
                            SS3.ActiveSheet.Cells[21, 1].Text = item.JIKNAME;
                            SS3.ActiveSheet.Cells[22, 1].Text = item.LTDSABUN;
                            SS3.ActiveSheet.Cells[23, 1].Text = item.LTDCODE.To<string>("0");
                        }
                    }
                }

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void PatHis_Value(long argWRTNO)
        {
            HEA_JEPSU item = heaJepsuService.GetItemByWrtno(argWRTNO);

            try
            {
                if (!item.IsNullOrEmpty())
                {
                    FnHisWRTNO = argWRTNO;
                    Screen_Clear();
                    txtPtno.Text = item.PTNO;
                    dtpJepDate.Text = item.JEPDATE;
                    cboYear.Text = VB.Left(item.JEPDATE, 4);

                    //검진 수검자 정보
                    Display_HeaPatient_Info(txtPtno.Text.Trim(), cboYear.Text);

                    //검진 접수 Display Main
                    Display_Jepsu_Main(txtPtno.Text);
                }

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void eTimerTick(object sender, EventArgs e)
        {
            int nRow = 0;
            long nSeqNo = 0;
            long nHeaWait = 0;
            string strYeyak = "";


            ssWait.ActiveSheet.ClearRange(0, 0, ssWait.ActiveSheet.Rows.Count, ssWait.ActiveSheet.ColumnCount, true);
            ssWait.ActiveSheet.Rows.Count = 0;

            btnMenualCall.Text = "수동Call";

            List<HIC_WAIT> lHW = hicWaitService.GetItembyJobDate("1");

            if (lHW.Count > 0)
            {

                for (int i = 0; i < lHW.Count; i++)
                {
                    nRow += 1;

                    if (ssWait.ActiveSheet.RowCount < nRow)
                    {
                        ssWait.ActiveSheet.RowCount = nRow;
                    }

                    nSeqNo = lHW[i].SEQNO;

                    if (nSeqNo != ssWait.ActiveSheet.Cells[nRow - 1, 0].Text.To<long>())
                    {
                        ssWait.ActiveSheet.Cells[nRow - 1, 0].Text = nSeqNo.To<string>("");
                        ssWait.ActiveSheet.Cells[nRow - 1, 1].Text = lHW[i].SNAME;
                        ssWait.ActiveSheet.Cells[nRow - 1, 2].Text = lHW[i].JEPTIME;
                        ssWait.ActiveSheet.Cells[nRow - 1, 3].Text = clsAES.DeAES(lHW[i].JUMIN2);

                        if (lHW[i].GBYEYAK == "1")
                        {
                            strYeyak = "1";
                        }
                        else if (lHW[i].GBYEYAK == "2")
                        {
                            if (strYeyak != "1") { strYeyak = "2"; }
                        }
                    }
                }
            }

            //암검진 예약자가 있는 점검
            if (strYeyak != "")
            {
                if (strYeyak == "1")
                {
                    btnMenualCall.Text = "암예약자";
                    btnMenualCall.ForeColor = Color.DarkRed;
                }
            }

            //일검 대기자가 있는지 점검
            if (hicWaitService.GetCountbyJobDate("2") > 0)
            {
                btnMenualCall.Text = "일검대기";
                btnMenualCall.ForeColor = Color.DarkRed;
            }


            HIC_WAIT item = hicWaitService.GetItembyJobDate2(clsPublic.GstrSysDate, "2");

            lblCallWait.Text = lHW.Count.To<string>() + " 명";
            lblCallWaitHic.Text = item.CNT + " 명";
        }

        private void eBtnXClick(object sender, EventArgs e)
        {

            string strJong = "";

            HEA_JEPSU nHJ = Jepsu_Data_Build();
            HIC_PATIENT pHP = hicPatientService.GetPatInfoByPtno(txtPtno.Text.Trim());
            //HEA_SUNAP hSP = heaSunapService.GetHeaSunapAmtByWRTNO(nHJ.WRTNO);
            HIC_SUNAP hSP = hicSunapService.GetHeaSunapAmtByWRTNO(nHJ.WRTNO);

            if (!HaJEPSU.WEBPRINTREQ.IsNullOrEmpty())
            {
                nHJ.WEBPRINTREQ = HaJEPSU.WEBPRINTREQ;
            }


            //수검확인서
            if (sender == btnXConfirm)
            {
                if (nHJ.LTDCODE.IsNullOrEmpty())
                {
                    MessageBox.Show("회사코드가 공란입니다.", "오류", MessageBoxButtons.OK);
                    return;
                }
                
                if (VB.InStr(VB.Pstr(txtLtdCode.Text, ".", 2),"교육청") > 0)
                {
                    strJong = "1";
                }

                if (strJong == "")
                {
                    switch (nHJ.LTDCODE)
                    {
                        case 753:
                        case 402:
                            strJong = "1"; break;
                        case 1119: strJong = "2"; break;
                        default: strJong = "3"; break;
                    }
                }
                
                if(strJong == "1" && pHP.SOSOK.IsNullOrEmpty())
                {
                    MessageBox.Show("시청/교육청 소속명이 공란입니다.", "오류", MessageBoxButtons.OK);
                    return;
                }

                frmHaConfirmPrint fHPP = new frmHaConfirmPrint(nHJ.WRTNO, strJong);
                fHPP.ShowDialog();

            }
            //영수증
            else if (sender == btnXRecReport)
            {

                //if (ComFunc.MsgBoxQ(nHJ.GJJONG + " 종 영수증을 발생하시겠습니까?", "", MessageBoxDefaultButton.Button1) == DialogResult.Yes
                if (ComFunc.MsgBoxQ("영수증을 발생하시겠습니까?", "", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    nHJ.JEPBUN = "5";
                    cHPrt.Receipt_Report_Print(nHJ, hSP);
                }

            }
            //바코드증
            else if (sender == btnXBarCode)
            {
                frmHaPrintBar fHPP = new frmHaPrintBar(nHJ, pHP);
                fHPP.ShowDialog();
            }
            //접수증
            else if (sender == btnXJepsu)
            {
                JepsuPrintSetting(nHJ);
                frmHaPrintPano fHPP = new frmHaPrintPano(nHJ, pHP, FstrChkExam, FstrChkExamName, FstrContrast, strMsg);
                fHPP.ShowDialog();
                //frmHaPrintPano
            }
            //식권발급
            else if (sender == btnDietTicket)
            {
                
                clsPublic.GstrMsgList = "[예]:재발행 [아니오]:보호자 [취소]:인쇄안함";
                clsPublic.GstrMsgList += ComNum.VBLF + "식권을 인쇄하시겠습니까?";
                DialogResult MsgResult = MessageBox.Show(clsPublic.GstrMsgList, "식권재발행/보호자식권 발행", MessageBoxButtons.YesNoCancel);
                if (MsgResult == DialogResult.Yes)
                {
                    Print_Food_Ticket(nHJ, "(재발행)");
                }
                else if (MsgResult == DialogResult.No)
                {
                    Print_Food_Ticket(nHJ, "(보호자)");
                }
                else if (MsgResult == DialogResult.Cancel)
                {
                    return;
                }
            }
        }

        public void eDisevent(object sender, FormClosingEventArgs e)
        {
            frmHaJepsuView.rSetGstrValue -= new frmHaJepsuView.SetGstrValue(eJepList_DblClick);
            frmHaJepsuView.rSetGstrFamValue -= new frmHaJepsuView.SetGstrFamValue(eJepsuView_Fam);
            frmHaLtdExamCompare.rSetGstrItem -= new frmHaLtdExamCompare.SetGstrItem(frmHaLtdExCompare_BtnSelClcik);
            frmHcPanPersonResult.rSetHaJepsuGstrValue -= new frmHcPanPersonResult.SetHaJepsuGstrValue(PatHis_Value);
            frmHcPanPersonResult.rSetHaJepsuBtnRef -= new frmHcPanPersonResult.SetHaJepsuBtnRef(ePtno_Event);
            frmHcExcelList.rSetHaJepsuGstrValue -= new frmHcExcelList.SetHaJepsuGstrValue(eHaExcelList_DblClick);
            frmHaResvCalendar.rSndMsg -= new frmHaResvCalendar.rSendMsg(eHaCalenderList_DblClick);
            this.btnJepView.Click -= new EventHandler(eJepsuView);
        }

        private void eCboTxtChanged(object sender, EventArgs e)
        {
            if (sender == cboHalinGye)
            {
                Gesan_HalinAmt(VB.Left(cboHalinGye.Text, 3), VB.Left(cboJONG.Text, 2), VB.Pstr(txtLtdCode.Text, ".", 1), cboSex.Text);
                btnAmt.PerformClick();
            }
            else if (sender == cboSTime)
            {
                if (cboSTime.Text == "" || dtpSDate.Text == "")
                {
                    return;
                }

                string strSDate = dtpSDate.Text;
                string strSTime = cboSTime.Text;
                string strSTimeCnt = "0";

                int nCNT = heaJepsuService.GetCountBySDateSTime(strSDate, strSTime);

                for (int i = 0; i < 16; i++)
                {
                    if (FstrSTime[i, 0] == strSTime)
                    {
                        strSTimeCnt = FstrSTime[i, 1];
                        break;
                    }
                }

                txtJepResvCnt.Text = strSTimeCnt + "/" + nCNT.To<string>("0");
            }   
        }

        private void eJepList_DblClick(HEA_JEPSU item)
        {
            try
            {
                if (!item.IsNullOrEmpty())
                {
                    Screen_Clear();
                    txtPtno.Text = item.PTNO;
                    dtpJepDate.Text = item.JEPDATE;
                    cboYear.Text = VB.Left(item.JEPDATE, 4);
                    if (FnHisWRTNO == 0) { FnHisWRTNO = item.WRTNO; }

                    //검진 수검자 정보
                    Display_HeaPatient_Info(txtPtno.Text.Trim(), cboYear.Text);

                    //검진 접수 Display Main
                    Display_Jepsu_Main(txtPtno.Text);
                }

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void eJepsuView_Fam(HEA_JEPSU item)
        {
            if (!item.IsNullOrEmpty())
            {
                txtFamilly.Text = item.SNAME + VB.Space(10) + "." + item.PANO.To<string>();
            }

            clsHcVariable.GbFamilly_PopUp = false;
        }

        private void eJepsuView(object sender, EventArgs e)
        {
            rSetJepsuView(sender, e);
        }

        private void frmHaLtdExCompare_BtnSelClcik(HEA_GROUPCODE argGrpCD)
        {
            try
            {
                if (!argGrpCD.IsNullOrEmpty())
                {
                    HaGrpCD = argGrpCD;

                    cboSex.Text = HaGrpCD.GBSEX;
                    cboJONG.SelectedIndex = cboJONG.FindString(HaGrpCD.JONG);
                    txtLtdCode.Text = HaGrpCD.LTDCODE.To<string>("");
                    if (txtLtdCode.Text.Trim() != "") { txtLtdCode.Text = txtLtdCode.Text + "." + hicLtdService.READ_Ltd_One_Name(txtLtdCode.Text); }

                    Group_Exams_Display();

                    //수납금액 계산하기
                    Gesan_Sunap_Amt(suInfo, "02", HaGrpCD.LTDCODE, txtHalinAmt.Text.Replace(",", "").To<long>(0), "");
                }

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void eChkChange(object sender, EventArgs e)
        {
            if (sender == chkCall)
            {
                ComFunc.ReadSysDate(clsDB.DbCon);

                if (chkCall.Checked)
                {
                    chkCall.Text = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + " " + clsType.User.JobName;
                }
                else
                {
                    chkCall.Text = "";
                }
            }
            else if (sender == chkRES12_1)
            {
                if (chkRES12_1.Checked)
                {
                    rdoRES12.Checked = true;
                    chkRES12_2.Checked = false;
                    chkRES12_3.Checked = false;
                }
            }
            else if (sender == chkRES12_2)
            {
                if (chkRES12_2.Checked)
                {
                    rdoRES12.Checked = true;
                    chkRES12_1.Checked = false;
                    chkRES12_3.Checked = false;
                }
            }
            else if (sender == chkRES12_3)
            {
                if (chkRES12_3.Checked)
                {
                    rdoRES12.Checked = true;
                    chkRES12_1.Checked = false;
                    chkRES12_2.Checked = false;
                }
            }
            else if (sender == chkGongDan)
            {
                if (!chkGongDan.Checked) { return; }

                if (chkGongDan.Checked && VB.Replace(txtTotAmt.Text, ",", "").To<long>(0) > 0 && FstrGBSTS != "0" && FstrGBSTS != "D")
                {
                    HIC_PRIVACY_ACCEPT_NEW item2 = hicPrivacyAcceptNewService.GetIetmByPtnoYear(txtPtno.Text, cboYear.Text);
                    if (item2.IsNullOrEmpty())
                    {
                        frmHcPermission.CellDblClicked(3);
                        //frmHcEmrPermission.CellDblClicked(3);
                    }
                }
            }
        }

        private void eTxtChanged(object sender, EventArgs e)
        {
            if (sender == txtJumin1)
            {
                if (txtJumin1.TextLength == 6)
                {
                    txtJumin2.Focus();
                }
            }
        }

        private void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            if (sender == txtLtdCode)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0) > 0 && VB.Pstr(txtLtdCode.Text, ".", 2) != "")
                    {
                        if (cboSex.Text == "M")
                        {
                            cboJONG.SelectedIndex = cboJONG.FindString("21");
                        }
                        else
                        {
                            cboJONG.SelectedIndex = cboJONG.FindString("22");
                        }
                        cboJONG.Focus();
                    }
                    else if (txtLtdCode.Text.Trim() == "")
                    {
                        if (cboSex.Text == "M")
                        {
                            cboJONG.SelectedIndex = cboJONG.FindString("11");
                        }
                        else
                        {
                            cboJONG.SelectedIndex = cboJONG.FindString("12");
                        }
                        cboJONG.Focus();
                    }
                    else
                    {
                        if (!txtLtdCode.Text.Trim().IsNullOrEmpty())
                        {
                            Ltd_Code_Help();
                        }
                    }
                }
            }
            else if (sender == txtJikSabun)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (!txtJikSabun.Text.Trim().IsNullOrEmpty())
                    {
                        Hic_Code_Help("SA", txtJikSabun); //추천직원
                    }
                }
            }
            else if (sender == txtJumin1)
            {
                if (e.KeyCode == Keys.Enter) { txtJumin2.Focus(); }
            }
            else if (sender == txtJumin2)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (txtJumin1.Text.IsNullOrEmpty() || txtJumin2.Text.IsNullOrEmpty()) { return; }

                    if (cHF.JuminNo_Check_New(txtJumin1.Text, txtJumin2.Text).Equals("ERROR"))
                    {
                        MessageBox.Show("주민번호가 올바르지 않습니다.", "확인");
                        txtJumin2.Focus();
                        return;
                    }

                    string strPtno = hicPatientService.GetPtnoByJuminNo(clsAES.AES(txtJumin1.Text + txtJumin2.Text));

                    List<HIC_PATIENT> lstJumin = hicPatientService.GetCountbyPtNo(strPtno);

                    if (lstJumin.Count > 1)
                    {
                        string strJumin = "";

                        clsPublic.GstrMsgList = "";

                        for (int i = 0; i < lstJumin.Count; i++)
                        {
                            strJumin = clsAES.DeAES(lstJumin[i].JUMIN2);

                            clsPublic.GstrMsgList += VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 7);
                            clsPublic.GstrMsgList += " " + lstJumin[i].SNAME + ComNum.VBLF + ComNum.VBLF;
                        }

                        clsPublic.GstrMsgList += "조회된 등록번호로 2건 이상 수검자 마스터 Data가 존재합니다." + ComNum.VBLF;
                        clsPublic.GstrMsgList += "계속 진행하시겠습니까? (이중챠트정리 후 사용요망)";

                        if (MessageBox.Show(clsPublic.GstrMsgList, "", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }

                    if (!strPtno.IsNullOrEmpty())
                    {
                        Display_HeaPatient_Info(strPtno, cboYear.Text);
                    }
                    else
                    {
                        New_Patient_Create();
                    }

                    //종검 접수 Display Main
                    Display_Jepsu_Main(txtPtno.Text);

                    txtLtdCode.Focus();

                    return;
                }
            }
            else if (sender == txtPtno)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    txtPtno.Text = VB.Format(VB.Val(txtPtno.Text), "00000000");
                    if (txtPtno.Text.Trim().Equals("00000000")) { txtPtno.Text = ""; return; }

                    List<HIC_PATIENT> lstJumin = hicPatientService.GetCountbyPtNo(txtPtno.Text);

                    if (lstJumin.Count > 1)
                    {
                        string strJumin = "";

                        clsPublic.GstrMsgList = "";

                        for (int i = 0; i < lstJumin.Count; i++)
                        {
                            strJumin = clsAES.DeAES(lstJumin[i].JUMIN2);

                            clsPublic.GstrMsgList += VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 7);
                            clsPublic.GstrMsgList += " " + lstJumin[i].SNAME + ComNum.VBLF + ComNum.VBLF;
                        }

                        clsPublic.GstrMsgList += "입력하신 등록번호로 2건 이상 수검자 마스터 Data가 존재합니다." + ComNum.VBLF;
                        clsPublic.GstrMsgList += "계속 진행하시겠습니까? (이중챠트정리 후 사용요망)";

                        if (MessageBox.Show(clsPublic.GstrMsgList, "", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }

                    HIC_PATIENT pHP = hicPatientService.GetPatInfoByPtno(txtPtno.Text.Trim());

                    if (pHP.IsNullOrEmpty())
                    {
                        BAS_PATIENT pBP = basPatientService.GetItembyPano(txtPtno.Text.Trim());

                        if (pBP.IsNullOrEmpty())
                        {
                            New_Patient_Create();
                            return;
                        }
                        else
                        {
                            txtJumin1.Text = pBP.JUMIN1.Trim();
                            txtJumin2.Text = clsAES.DeAES(pBP.JUMIN3);
                            txtSName.Text = pBP.SNAME;
                            cboSex.Text = pBP.SEX;
                            txtAge.Text = ComFunc.AgeCalcEx(txtJumin1.Text + txtJumin2.Text, dtpJepDate.Value.ToShortDateString()).To<string>();
                            New_Patient_Create();
                            return;
                        }
                    }

                    //검진 수검자 정보
                    Display_HeaPatient_Info(txtPtno.Text.Trim(), cboYear.Text);

                    //검진 접수 Display Main
                    Display_Jepsu_Main(txtPtno.Text);

                    txtLtdCode.Focus();
                    return;
                }
            }
            else if (sender == txtGroupCode_Search)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Hea_GroupCode_Help(txtGroupCode_Search.Text.Trim(), RtnCurrCodeList(suInfo));
                }
            }
            //카드입금 입력 (자동계산)
            else if (sender == txtCardAmt)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (txtBonAmt.Text == "0" || txtBonAmt.Text == "") { return; }
                    long nBonAmt = txtBonAmt.Text.Replace(",", "").To<long>();
                    long nCardAmt = txtCardAmt.Text.Replace(",", "").To<long>();

                    txtCashAmt.Text = ((nBonAmt - nCardAmt) < 0 ? 0 : (nBonAmt - nCardAmt)).ToString("#,##0");

                    long nCashAmt = txtCashAmt.Text.Replace(",", "").To<long>();

                    txtIpgumAmt.Text = (nCardAmt + nCashAmt).ToString("#,##0");
                    txtChaAmt.Text = "0";

                    sunap.SUNAPAMT1 = nCashAmt;
                    sunap.SUNAPAMT2 = nCardAmt;
                }
            }
            //현금영수증 입력 (자동계산)
            else if (sender == txtCashAmt)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (txtBonAmt.Text == "0" || txtBonAmt.Text == "") { return; }
                    long nBonAmt = txtBonAmt.Text.Replace(",", "").To<long>();
                    long nCashAmt = txtCashAmt.Text.Replace(",", "").To<long>();

                    txtCardAmt.Text = ((nBonAmt - nCashAmt) < 0 ? 0 : (nBonAmt - nCashAmt)).ToString("#,##0");

                    long nCardAmt = txtCardAmt.Text.Replace(",", "").To<long>();

                    txtIpgumAmt.Text = (nCardAmt + nCashAmt).ToString("#,##0");
                    txtChaAmt.Text = "0";

                    sunap.SUNAPAMT1 = nCashAmt;
                    sunap.SUNAPAMT2 = nCardAmt;
                }
            }
            else if (sender == txtKeyNo)
            {
                txtKeyNo.ImeMode = ImeMode.Alpha;

                if (e.KeyCode == Keys.Enter)
                {
                    txtKeyNo.Text = txtKeyNo.Text.ToUpper();
                }
            }
        }

        /// <summary>
        /// 선택검사 코드 검색창 연동
        /// </summary>
        /// <param name="strGB"></param>
        private void Hea_GroupCode_Help(string argWard, List<READ_SUNAP_ITEM> argSuInfo)
        {
            string strCodes = string.Empty;

            frmHaGroupCode.rSndMsg += new frmHaGroupCode.rSendMsg(frmHaGroupCode_ssDblClick);
            frmHaGroupCode fHaGrpCD = new frmHaGroupCode(argWard, argSuInfo);
            fHaGrpCD.ShowDialog();
            frmHaGroupCode.rSndMsg -= new frmHaGroupCode.rSendMsg(frmHaGroupCode_ssDblClick);
            //clsHcVariable.LSTHaRESEXAM.Clear();
        }

        private void frmHaGroupCode_ssDblClick(List<READ_SUNAP_ITEM> argCode)
        {
            if (argCode.Count == 0) { return; }

            List<string> lstgrpCode = new List<string>();

            List<READ_SUNAP_ITEM> vRSI = new List<READ_SUNAP_ITEM>();                   //스프레드 내용을 담은 코드
            READ_SUNAP_ITEM_Get_Spread(vRSI, ssGroup);

            //추가한 그룹코드가 있다면 기존 검사항목과 중복체크하여 알림
            string strMsg = Verify_Overlap_ExCode(argCode, grpExam);
            if (!strMsg.IsNullOrEmpty())
            {
                MessageBox.Show(strMsg, "! 중복검사 알림 !");
            }

            List<READ_SUNAP_ITEM> delRSI = new List<READ_SUNAP_ITEM>();                 //삭제코드
            List<READ_SUNAP_ITEM> insRSI = new List<READ_SUNAP_ITEM>();                 //추가코드
            List<READ_SUNAP_ITEM> rtnRSI = new List<READ_SUNAP_ITEM>();                 //정리된 코드

            List<GROUPCODE_EXAM_DISPLAY> dupGED = new List<GROUPCODE_EXAM_DISPLAY>();   //그룹코드내 포함시키지 않을 검사항목
            List<GROUPCODE_EXAM_DISPLAY> rtnGED = new List<GROUPCODE_EXAM_DISPLAY>();   //그룹코드를 조회 후 작성된 검사항목

            delRSI = vRSI.Except(argCode).ToList();    //스프레드에서 제외시킬 코드
            insRSI = argCode.Except(vRSI).ToList();    //스프레드에 추가시킬 코드

            rtnRSI = Remake_Overlap_GroupList(vRSI, delRSI, insRSI);    //선택 및 제외한 그룹코드 정리

            string strBurate = VB.Pstr(cboBuRate.Text, ".", 1).To<string>(FstrBuRate);

            for (int i = 0; i < rtnRSI.Count; i++)
            {
                //선택한 코드 적용
                READ_SUNAP_ITEM item = readSunapItemService.GetHeaItemByCode(rtnRSI[i].GRPCODE);

                if (!item.IsNullOrEmpty())
                {
                    //검사코드 읽어서 목록 추가(중복제외)
                    rtnGED = Read_ExamCode_GrpCode(rtnGED, item.GRPCODE.Trim(), dtpJepDate.Text, dupGED);

                    if (item.GBSELF.To<string>("").Trim() == "")
                    {
                        item.GBSELF = strBurate;
                    }
                    
                    //선택 코드 금액 산정 
                    rtnRSI[i].GRPNAME = item.GRPNAME;

                    //추가
                    //if (rtnRSI[i].GBSELF == item.GBSELF || rtnRSI[i].GBSELF.IsNullOrEmpty())
                    if (rtnRSI[i].GBSELF == item.GBSELF )
                    {
                        rtnRSI[i].GBSELF = item.GBSELF;
                    }

                    //TODO : 날짜읽어서 금액코드 적용하기
                    rtnRSI[i].AMT = item.AMT;

                    lstgrpCode.Add(rtnRSI[i].GRPCODE);

                    if (lstgrpCode.Count > 0)
                    {
                        dupGED = groupCodeExamDisplayService.GetHeaListExcodeByGrpCodeList(lstgrpCode);
                    }
                }   
            }

            //Spread에 표시
            suInfo.Clear();
            suInfo.AddRange(rtnRSI);
            ssGroup.DataSource = null;
            ssGroup.SetDataSource(suInfo);

            grpExam.Clear();
            grpExam.AddRange(rtnGED);

            //분진검사, 수면내시경의 경우 중복검사 제외
            Check_Exam_Except(lstgrpCode, ref grpExam);

            ssExam.DataSource = null;
            ssExam.SetDataSource(grpExam);

            clsSpread.gSpdSortRow(ssExam, 0, ref bolSort, true);

            //수납금액 계산하기
            string strHalinCode = VB.Pstr(cboHalinGye.Text, ".", 1).Trim();
            long nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0);

            Gesan_Sunap_Amt(suInfo, strBurate, nLtdCode, txtHalinAmt.Text.Replace(",", "").To<long>(), strHalinCode);

        }

        /// <summary>
        /// 분진검사, 수면내시경이 있는 경우 중복검사 항목 제외처리
        /// </summary>
        /// <param name="lstgrpCode">선택된 그룹코드</param>
        /// <param name="grpExam">조정 대상의 검사항목 List</param>
        private void Check_Exam_Except(List<string> lstgrpCode, ref List<GROUPCODE_EXAM_DISPLAY> grpExam)
        {
            int nIndex1 = 0;
            int nIndex2 = 0;
            int nIndex3 = 0;
            int nIndex4 = 0;

            if (lstgrpCode.IsNullOrEmpty() || lstgrpCode.Count == 0)
            {
                return;
            }
            //수면내시경인 경우 일반 내시경 코드 제외
            List<HEA_GROUPEXAM_EXCODE> lstHaGrExCD = heaGroupexamExcodeService.GetGbEndoCodeByCodeIN(lstgrpCode);
            //위조영 있는 경우 위내시경, 위수면내시경 제외
            if (!lstHaGrExCD.Find(x => x.ENDOGUBUN1 == "Y").IsNullOrEmpty())
            {
                nIndex1 = lstHaGrExCD.FindIndex    (y => y.ENDOGUBUN2 == "Y");
                nIndex2 = lstHaGrExCD.FindLastIndex(y => y.ENDOGUBUN2 == "Y");
                nIndex3 = lstHaGrExCD.FindIndex    (y => y.ENDOGUBUN3 == "Y");
                nIndex4 = lstHaGrExCD.FindLastIndex(y => y.ENDOGUBUN3 == "Y");

                if (nIndex1 > -1) { grpExam[grpExam.FindIndex(x => x.EXCODE == lstHaGrExCD[nIndex1].EXCODE)].RowStatus = RowStatus.Delete; }
                if (nIndex2 > -1) { grpExam[grpExam.FindIndex(x => x.EXCODE == lstHaGrExCD[nIndex2].EXCODE)].RowStatus = RowStatus.Delete; }
                if (nIndex3 > -1) { grpExam[grpExam.FindIndex(x => x.EXCODE == lstHaGrExCD[nIndex3].EXCODE)].RowStatus = RowStatus.Delete; }
                if (nIndex4 > -1) { grpExam[grpExam.FindIndex(x => x.EXCODE == lstHaGrExCD[nIndex4].EXCODE)].RowStatus = RowStatus.Delete; }
            }

            //위수면내시경 있는 경우 위내시경 제외            
            if (!lstHaGrExCD.Find(x => x.ENDOGUBUN3 == "Y").IsNullOrEmpty())
            {
                nIndex1 = lstHaGrExCD.FindIndex    (y => y.ENDOGUBUN2 == "Y");
                nIndex2 = lstHaGrExCD.FindLastIndex(y => y.ENDOGUBUN2 == "Y");

                if (nIndex1 > -1) { grpExam[grpExam.FindIndex(x => x.EXCODE == lstHaGrExCD[nIndex1].EXCODE)].RowStatus = RowStatus.Delete; }
                if (nIndex2 > -1) { grpExam[grpExam.FindIndex(x => x.EXCODE == lstHaGrExCD[nIndex2].EXCODE)].RowStatus = RowStatus.Delete; }
            }

            //대장수면내시경 있는 경우 대장내시경 제외
            if (!lstHaGrExCD.Find(x => x.ENDOGUBUN5 == "Y").IsNullOrEmpty())
            {
                nIndex1 = lstHaGrExCD.FindIndex    (y => y.ENDOGUBUN4 == "Y");
                nIndex2 = lstHaGrExCD.FindLastIndex(y => y.ENDOGUBUN4 == "Y");

                if (nIndex1 > -1) { grpExam[grpExam.FindIndex(x => x.EXCODE == lstHaGrExCD[nIndex1].EXCODE)].RowStatus = RowStatus.Delete; }
                if (nIndex2 > -1) { grpExam[grpExam.FindIndex(x => x.EXCODE == lstHaGrExCD[nIndex2].EXCODE)].RowStatus = RowStatus.Delete; }
            }

            //분진촬영이 있을 경우
            if (!grpExam.Find(x => x.EXCODE == "TZ47").IsNullOrEmpty())
            {
                nIndex1 = grpExam.FindIndex(x => x.EXCODE == "TX11");
                if (nIndex1 > -1) { grpExam[nIndex1].RowStatus = RowStatus.Delete; }
            }
        }

        private string Verify_Overlap_ExCode(List<READ_SUNAP_ITEM> argCode, List<GROUPCODE_EXAM_DISPLAY> grpExam)
        {
            List<GROUPCODE_EXAM_DISPLAY> rtnList = new List<GROUPCODE_EXAM_DISPLAY>();
            List<GROUPCODE_EXAM_DISPLAY> varList = null;
            string rtnVal = "";
            List<string> strExCode = new List<string>();
            List<string> strExName = new List<string>();

            try
            {
                for (int i = 0; i < argCode.Count; i++)
                {
                    if (argCode[i].ADDGBN == "Y")
                    {
                        strExCode = new List<string>();
                        strExName = new List<string>();

                        varList = new List<GROUPCODE_EXAM_DISPLAY>();
                        varList = groupCodeExamDisplayService.GetHeaExamListByGroupCode(argCode[i].GRPCODE);

                        var rtnTmp1 = grpExam.Select(x => x.EXCODE).Intersect(varList.Select(y => y.EXCODE)).ToList();
                        var rtnTmp2 = grpExam.Select(x => x.EXNAME).Intersect(varList.Select(y => y.EXNAME)).ToList();

                        foreach (var item in rtnTmp1) { strExCode.Add(item.ToString()); }
                        foreach (var item in rtnTmp2) { strExName.Add(item.ToString()); }

                        if (!strExCode.IsNullOrEmpty() && strExCode.Count > 0)
                        {
                            rtnVal += "그룹코드 : " + argCode[i].GRPCODE + " " + argCode[i].GRPNAME + ComNum.VBLF;

                            for (int j = 0; j < strExCode.Count; j++)
                            {
                                rtnVal += "검사항목 : " + strExCode[j] + " " + strExName[j] + ComNum.VBLF;
                            }

                            rtnVal += ComNum.VBLF;
                        }
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "";
            }
        }

        private List<GROUPCODE_EXAM_DISPLAY> Read_ExamCode_GrpCode(List<GROUPCODE_EXAM_DISPLAY> rtnGED, string argCode, string argJepDate, List<GROUPCODE_EXAM_DISPLAY> dupGED)
        {
            List<string> lstExtExCode = new List<string>(); //그룹코드 정보 조회시 중복을 방지 할 검사항목 코드들

            if (!dupGED.IsNullOrEmpty() && dupGED.Count > 0)
            {
                for (int i = 0; i < dupGED.Count; i++)
                {
                    lstExtExCode.Add(dupGED[i].EXCODE);
                }
            }

            //그룹코드 금액 집계 (종검은 그룹코드에 지정된 금액을 사용함)
            List<HIC_GROUPEXAM_GROUPCODE_EXCODE> lst = hicGroupexamGroupcodeExcodeService.GetHeaListByCode(argCode, lstExtExCode);

            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    GROUPCODE_EXAM_DISPLAY item = new GROUPCODE_EXAM_DISPLAY
                    {
                        GROUPCODE = lst[i].GROUPCODE,
                        GROUPCODENAME = lst[i].GROUPNAME,
                        EXCODE = lst[i].EXCODE,
                        EXNAME = lst[i].HNAME,
                        ETCEXAM = lst[i].ETCEXAM
                    };

                    rtnGED.Add(item);   //조회된 검사항목 정보를 INSERT
                }
            }

            return rtnGED;
        }

        /// <summary>
        /// 스프레드를 비교하려 스프레드를 List 변수로 as 로 받으면 DataSource 링크가 깨지므로 
        /// 비교하려는 스프레드를 아래와 같이 수동으로 복사하여서 사용함
        /// </summary>
        /// <param name="ssRSI">저장 변수</param>
        /// <param name="ssGroup">대상 스프레드</param>
        private void READ_SUNAP_ITEM_Get_Spread(List<READ_SUNAP_ITEM> ssRSI, FpSpread ssGroup)
        {
            READ_SUNAP_ITEM rRSI = new READ_SUNAP_ITEM();

            for (int i = 0; i < ssGroup.ActiveSheet.RowCount; i++)
            {
                if (ssGroup.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    rRSI = new READ_SUNAP_ITEM();

                    rRSI.GRPCODE = ssGroup.ActiveSheet.Cells[i, 1].Text;
                    rRSI.GRPNAME = ssGroup.ActiveSheet.Cells[i, 2].Text;
                    rRSI.AMT = ssGroup.ActiveSheet.Cells[i, 3].Text.Replace(",", "").To<long>(0);
                    rRSI.BURATE = ssGroup.ActiveSheet.Cells[i, 4].Text;
                    rRSI.GBHALIN = ssGroup.ActiveSheet.Cells[i, 5].Text;
                    rRSI.RID = ssGroup.ActiveSheet.Cells[i, 6].Text;
                    
                    ssRSI.Add(rRSI);
                }
            }
        }

        private List<READ_SUNAP_ITEM> Remake_Overlap_GroupList(List<READ_SUNAP_ITEM> ssRSI, List<READ_SUNAP_ITEM> delRSI, List<READ_SUNAP_ITEM> insRSI)
        {
            for (int i = 0; i < delRSI.Count; i++)
            {
                for (int j = 0; j < ssRSI.Count; j++)
                {
                    if (delRSI[i].GRPCODE == ssRSI[j].GRPCODE)
                    {
                        ssRSI[j].RowStatus = RowStatus.Delete;
                    }
                }
            }

            Delete_GroupCode_List(ref ssRSI);   //그룹코드 삭제

            //ssRSI.AddRange(insRSI);

            for (int i = 0; i < insRSI.Count; i++)
            {
                //insRSI[i].ADDGBN = "Y";     //추가된 코드임을 표시
                ssRSI.Add(insRSI[i]);
            }

            return ssRSI;
        }

        private void Delete_GroupCode_List(ref List<READ_SUNAP_ITEM> ssRSI)
        {
            for (int i = 0; i < ssRSI.Count; i++)
            {
                if (ssRSI[i].RowStatus == RowStatus.Delete)
                {
                    ssRSI.RemoveAt(i);
                    Delete_GroupCode_List(ref ssRSI);
                    break;
                }
            }
        }

        private void New_Patient_Create()
        {
            Hpatient = new HIC_PATIENT();
            frmHcNewPatient frm = new frmHcNewPatient(txtJumin1.Text, txtJumin2.Text);
            frm.rSetGstrValue += new frmHcNewPatient.SetGstrValue(ePost_value_HPAT);
            frm.ShowDialog();

            if (Hpatient.PANO > 0)
            {
                txtJumin1.Text  = Hpatient.CT_JUMIN1;
                txtJumin2.Text  = Hpatient.CT_JUMIN2;
                txtSName.Text   = Hpatient.SNAME;
                cboSex.Text     = Hpatient.SEX;
                txtAge.Text     = Hpatient.P_AGE.To<string>();
                txtPtno.Text    = Hpatient.PTNO;
                txtPano.Text    = Hpatient.PANO.To<string>(); ;
                txtTel.Text     = Hpatient.TEL;
                txtHphone.Text  = Hpatient.HPHONE;
                txtJuso1.Text   = Hpatient.JUSO1;
                txtJuso2.Text   = Hpatient.JUSO2;
                txtMail.Text    = Hpatient.MAILCODE;
                txtEmail.Text   = Hpatient.EMAIL;
                if (!Hpatient.LTDCODE.IsNullOrEmpty() && Hpatient.LTDCODE > 0)
                {
                    txtLtdCode.Text = Hpatient.LTDCODE.To<string>();
                    txtLtdCode.Text += "." + hicLtdService.READ_Ltd_One_Name(txtLtdCode.Text.Trim());
                }
                FstrBuildNo     = Hpatient.BUILDNO;

                if (Hpatient.TEL == "" || Hpatient.TEL.To<long>(0) == 0) { txtTel.Text = "054-000-0000"; }

                cboJONG.Enabled = true;
            }
        }

        /// <summary>
        /// 종검접수 Display Main 루틴 AS_IS 방식
        /// </summary>
        /// <param name="text"></param>
        /// <param name="v"></param>
        private void Display_Jepsu_Main(string argPtno)
        {
            lblSTS.Text = "신규";
            lblSTS.BackColor = Color.FromArgb(255, 255, 192);
            lblSTS2.Text = "";
            
            //최근 60일이내 접수건
            string strDate = DateTime.Now.AddDays(-90).ToShortDateString();

            //수검자 메모
            Hic_Memo_Screen(argPtno);


            //팝업
            string strAllegy = sup.READ_ALLERGY_POPUP(clsDB.DbCon, txtPtno.Text, txtSName.Text);
            if (strAllegy != "")
            {
                ComFunc.MsgBox(strAllegy, "환자의 알러지 정보");
            }

            //영상의학과 메트포르민제 처방내역 팝업
            string strXrayPop = sup.READ_XRAY_CAUTION_POPUP(clsDB.DbCon, "I", dtpSDate.Text, txtPtno.Text, txtSName.Text);
            if (strXrayPop != "")
            {
                ComFunc.MsgBox(strXrayPop, "영상촬영시 환자주의 정보");
            }

            string strXrayPop1 = sup.READ_XRAY_CAUTION_POPUP(clsDB.DbCon, "O", dtpSDate.Text, txtPtno.Text, txtSName.Text);
            if (strXrayPop1 != "")
            {
                ComFunc.MsgBox(strXrayPop1, "영상촬영시 환자주의 정보");
            }


            //종검접수내역 조회 (예약포함)
            HIC_JEPSU_HEA_EXJONG Jep = null;
            if (FnHisWRTNO > 0)
            {
                Jep = hicJepsuHeaExjongService.GetHeaJepInfoByWrtno(FnHisWRTNO);
            }
            else
            {
                Jep = hicJepsuHeaExjongService.GetHeaJepInfo(argPtno, strDate);
            }
            
            //접수내역(예약포함)
            if (!Jep.IsNullOrEmpty())
            {
                //최근 접수내역 상세 Detail
                Display_Jepsu_Detail(Jep);
                tbCtrl_Etc.SelectedTab = tbETCpage1;
            }
            //신규(예약)접수
            else
            {
                Display_Jepsu_New();
            }

            //회사 참고사항
            if (VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0) > 0)
            {
                txtLtdRemark.Text = hicLtdService.GetHaRemarkbyLtdCode(VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0));
            }

            FnHisWRTNO = 0;

        }

        private void Display_Jepsu_Detail(HIC_JEPSU_HEA_EXJONG jep)
        {
            string strDate =cboYear.Text + "-01-01";
            string strWJong = "";
            string strUCodes = "";
            string strCommitOK = "";
            string strLtdCode = "";

            panSelExam.Enabled = true;

            clsPublic.GstrMsgList = "";

            try
            {
                HaJEPSU = heaJepsuService.GetItemByWrtno(jep.WRTNO);

                HIC_IE_MUNJIN_NEW iHIMN = hicIeMunjinNewService.GetItembyPtNoMunDate(txtPtno.Text, strDate);

                if (!iHIMN.IsNullOrEmpty())
                {
                    lblIEMunjin.Visible = true;
                    lblIEMunjin.BackColor = Color.FromArgb(192, 192, 255);
                    lblIEMunjin.Text = "인터넷문진" + " " + iHIMN.MUNDATE;
                    IEMunjin_Name_Display(iHIMN.RECVFORM);
                    MUNJIN_CHK(jep);
                }

                if (!HaJEPSU.IsNullOrEmpty())
                {
                    clsHcVariable.LSTHaRESEXAM = heaResvExamService.GetListByPanoSDate(HaJEPSU.PANO, HaJEPSU.SDATE);

                    btnResReciept.Enabled = true;

                    FnWRTNO = HaJEPSU.WRTNO;
                    GnWRTNO = HaJEPSU.GWRTNO;
                    FstrGBSTS = HaJEPSU.GBSTS;
                    FnIEMunNo = HaJEPSU.IEMUNNO;
                    FstrOldSdate = HaJEPSU.SDATE;

                    cboJONG.SelectedIndex = cboJONG.FindString(HaJEPSU.GJJONG);

                    dtpJepDate.Text = HaJEPSU.JEPDATE;

                    if (!HaJEPSU.SABUN.IsNullOrEmpty() && HaJEPSU.SABUN > 0)
                    {
                        txtJikSabun.Text = HaJEPSU.SABUN.To<string>("");
                        txtJikSabun.Text += "." + clsVbfunc.GetInSaName(clsDB.DbCon, txtJikSabun.Text);
                    }

                    if (!HaJEPSU.EXAMCHANGE.IsNullOrEmpty())
                    {
                        txtRemark.Text = HaJEPSU.EXAMCHANGE;
                    }

                    txtJepList.Text = "";
                    List<HIC_JEPSU> lstHJ = hicJepsuService.GetListGjNameByPtnoJepDate(dtpSDate.Text, txtPtno.Text);
                    if (lstHJ.Count > 0)
                    {
                        for (int i = 0; i < lstHJ.Count; i++)
                        {
                            txtJepList.Text += lstHJ[i].NAME + ",";
                        }
                    }

                    txtGaJepList.Text = "";
                    List<HIC_JEPSU_WORK> lstHJW2 = hicJepsuWorkService.GetListGjNameByPtnoJepDate(VB.Left(dtpSDate.Text,4), txtPtno.Text);
                    if (lstHJW2.Count > 0)
                    {
                        for (int i = 0; i < lstHJW2.Count; i++)
                        {
                            txtGaJepList.Text += lstHJW2[i].NAME + ",";
                        }
                    }

                    List<HIC_CANCER_RESV2> list = hicCancerResv2Service.GetListByRTimeJumin(dtpJepDate.Text, clsAES.AES(txtJumin1.Text + txtJumin2.Text));
                    if (list.Count > 0)
                    {
                        txtGaJepList.Text += "암검진,";
                    }

                    panJepList.SetData(HaJEPSU);

                    //if (!HaJEPSU.MAILCODE.IsNullOrEmpty()) { txtMail.Text = HaJEPSU.MAILCODE.Trim(); }
                    //if (!HaJEPSU.JUSO1.IsNullOrEmpty()) { txtJuso1.Text = HaJEPSU.JUSO1.Trim(); }
                    //if (!HaJEPSU.JUSO2.IsNullOrEmpty())
                    //{
                    //    txtJuso2.Text = HaJEPSU.JUSO2.Trim();
                    //}
                    //else
                    //{
                    //    txtJuso2.Text = "";
                    //}

                    //2021-05-24(접수주소 표시)
                    if (!HaJEPSU.MAILCODE.IsNullOrEmpty()) { txtMail1.Text = HaJEPSU.MAILCODE.Trim(); }
                    if (!HaJEPSU.JUSO1.IsNullOrEmpty()) { txtJuso11.Text = HaJEPSU.JUSO1.Trim(); }
                    if (!HaJEPSU.JUSO2.IsNullOrEmpty())
                    {
                        txtJuso21.Text = HaJEPSU.JUSO2.Trim();
                    }
                    else
                    {
                        txtJuso21.Text = "";
                    }

                    chkGongDan.Checked = HaJEPSU.GONGDAN == "Y" ? true : false;
                    //panPAT.SetData(HaJEPSU);

                    if (HaJEPSU.LTDCODE > 0)
                    {
                        strLtdCode = HaJEPSU.LTDCODE.To<string>();
                        if (strLtdCode.Trim() != "") { txtLtdCode.Text = strLtdCode + "." + hicLtdService.READ_Ltd_One_Name(strLtdCode); }
                    }

                    switch (HaJEPSU.GBSTS)
                    {
                        case "0": lblSTS2.Text = "0.예약접수"; break;
                        case "1": lblSTS2.Text = "1.수진등록"; break;
                        case "2": lblSTS2.Text = "2.일부입력"; break;
                        case "3": lblSTS2.Text = "3.입력완료"; break;
                        case "5": lblSTS2.Text = "5.가판정완료"; break;
                        case "9": lblSTS2.Text = "9.판정완료"; break;
                        default: break;
                    }

                    if (HaJEPSU.GBSTS == "0")
                    {
                        cboJONG.Enabled = false;
                        panSTS.Enabled = true;
                        rdoSTS1.Checked = true;
                        dtpSDate.Enabled = true;
                        lblSTS.Text = "";

                        List<HIC_JEPSU_WORK> lstHJW = hicJepsuWorkService.GetItembyJuMin(clsAES.AES(txtJumin1.Text + txtJumin2.Text), cboYear.Text);

                        if (lstHJW.Count > 0)
                        {
                            for (int i = 0; i < lstHJW.Count; i++)
                            {
                                strWJong += lstHJW[i].GJJONG + ",";
                                strUCodes += lstHJW[i].UCODES + ",";
                            }

                            lblUCodeJong.Text = "유해인자" + ComNum.VBLF + strWJong;
                            lblUCODES.Text = cHcMain.UCode_Names_Display(strUCodes);
                        }

                        clsPublic.GstrMsgList = "이미 예약접수 상태입니다.";
                        clsPublic.GstrMsgList += "예약 수정은 예(Y), 수정안함은 아니오(N)를 누르세요.";
                    }
                    else
                    {
                        cboJONG.Enabled = false;    //검진종류
                        rdoSTS2.Checked = true;     //접수, 예약구분
                        panSTS.Enabled = false;
                        dtpSDate.Enabled = false;
                        //cboSTime.Enabled = false;
                        lblSTS.Text = "수정";
                        lblSTS.BackColor = Color.LightSalmon;

                        clsPublic.GstrMsgList = "이미 접수 상태입니다." + ComNum.VBLF;

                        if (HaJEPSU.SDATE != DateTime.Now.ToShortDateString())
                        {
                            clsPublic.GstrMsgList += "접수를 수정은 예(Y), 수정안함은 아니오(N), 신규로 접수는 취소를 누르세요.";
                        }
                        else
                        {
                            clsPublic.GstrMsgList += "접수를 수정은 예(Y), 수정안함은 아니오(N)를 누르세요.";
                        }

                        List<HIC_JEPSU> lstHJ2 = hicJepsuService.GetListByPtnoYearJepDate(txtPtno.Text, cboYear.Text, HaJEPSU.SDATE);

                        if (lstHJ2.Count > 0)
                        {
                            for (int i = 0; i < lstHJ2.Count; i++)
                            {
                                strWJong += lstHJ2[i].GJJONG + ",";
                                strUCodes += lstHJ2[i].UCODES + ",";
                            }

                            lblUCodeJong.Text = "유해인자" + ComNum.VBLF + strWJong;
                            lblUCODES.Text = cHcMain.UCode_Names_Display(strUCodes);
                        }
                    }

                    if (clsPublic.GstrMsgList != "")
                    {
                        DialogResult result;

                        if (HaJEPSU.GBSTS == "0")
                        {
                            if (MessageBox.Show(clsPublic.GstrMsgList, "확인사항", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                Screen_Clear();
                                return;
                            }
                        }
                        else
                        {
                            if (HaJEPSU.SDATE != DateTime.Now.ToShortDateString())
                            {
                                result = MessageBox.Show(clsPublic.GstrMsgList, "확인사항", MessageBoxButtons.YesNoCancel);
                            }
                            else
                            {
                                result = MessageBox.Show(clsPublic.GstrMsgList, "확인사항", MessageBoxButtons.YesNo);
                            }

                            switch (result)
                            {
                                case DialogResult.Yes:
                                    if (HaJEPSU.GBSTS == "0") { strCommitOK = "OK"; }
                                    break;
                                case DialogResult.No:
                                    if (HaJEPSU.GBSTS != "0")
                                    {
                                        Screen_Clear();
                                        return;
                                    }
                                    break;
                                default:
                                    if (Convert.ToInt32(HaJEPSU.GBSTS) > 0)
                                    {
                                        dtpJepDate.Text = DateTime.Now.ToShortDateString();
                                        dtpSDate.Text = DateTime.Now.ToShortDateString();
                                        dtpSDate.Enabled = true;
                                        //cboSTime.Enabled = true;
                                        cboJONG.Enabled = true;
                                        panSTS.Enabled = true;
                                        rdoSTS1.Checked = true;
                                        FnWRTNO = 0;
                                        lblSTS.Text = "신규";
                                        lblSTS.BackColor = Color.FromArgb(255, 255, 192);
                                        Group_Exams_Display();
                                        //Display_Hic_Jepsu();
                                        return;
                                    }
                                    break;
                            }
                        }
                    }

                    if (HaJEPSU.GBSTS == "1") { btnReset.Visible = true; }      //Reset 버튼
                    if (HaJEPSU.SUNAP == "N") { lblSunap.Visible = true; }      //미수납 대상 표시
                    if (HaJEPSU.GBDAILY == "Y") { lblSangDam.Visible = true; }  //당일상담 대상 표시

                    if (!HaJEPSU.GUIDETEL.IsNullOrEmpty() && HaJEPSU.GUIDETEL.To<string>("") != "")
                    {
                        chkCall.Checked = true;
                        chkCall.Text = HaJEPSU.GUIDETEL;
                    }

                    if (!HaJEPSU.WEBPRINTREQ.IsNullOrEmpty())
                    {
                        rdoRES14.Checked = true;
                    }
                    else
                    {
                        if (HaJEPSU.GBCHK3 == "Y")
                        {
                            rdoRES15.Checked = true;
                        }

                        if (HaJEPSU.GBJUSO.To<string>("").Trim() != "")
                        {
                            rdoRES12.Checked = true;

                            if (HaJEPSU.GBJUSO == "Y")
                            {
                                chkRES12_1.Checked = true;
                            }
                            else if (HaJEPSU.GBJUSO == "N")
                            {
                                chkRES12_2.Checked = true;
                            }
                            else if (HaJEPSU.GBJUSO == "E")
                            {
                                chkRES12_3.Checked = true;
                            }
                        }
                    }

                    if (cHF.GET_Naksang_Flag(HaJEPSU.AGE, HaJEPSU.SDATE, HaJEPSU.PTNO) == "Y")
                    {
                        chkFall.Checked = true;
                    }

                    cboBuRate.SelectedIndex = cboBuRate.FindString(HaJEPSU.BURATE, 0);
                    cboHalinGye.SelectedIndex = cboHalinGye.FindString(HaJEPSU.GAMCODE, 0);
                    txtHalinAmt.Text = HaJEPSU.GAMAMT.ToString("#,##0");

                    suInfo.AddRange(Read_Sunap_GroupCode(FnWRTNO)); //묶음코드 읽기
                    grpExam.AddRange(Read_ExamCode(FnWRTNO));       //검사코드 읽기
                    sunap = Account_Sunap(FnWRTNO);

                    Clear_Spread_GroupCode();
                    ssGroup.SetDataSource(suInfo);      //수납된 묶음코드 읽기
                    ssExam.SetDataSource(grpExam);      //수납된 검사항목 읽기

                    Display_Sunap_Amt(sunap);           //수납정보 
                    Hea_Spec_Screen();                  //2015-07-21 검체접수
                    Ticket_Name_Display();

                    if (HaJEPSU.GBSTS == "0")
                    {
                        cboHalinGye.SelectedIndex = cboHalinGye.FindString(HaJEPSU.GAMCODE, 0);
                    }

                    //가접수일 경우 수납내역이 없음으로 계산로직을 태워줌
                    //if (HaJEPSU.GBSTS == "0") { btnAmt.PerformClick(); }          //계산
                    btnAmt.PerformClick();

                    if (strCommitOK == "OK") { btnSave.PerformClick(); }

                    //가셔야할곳 표시
                    HEA_JEPSU nHJ = Jepsu_Data_Build();
                    JepsuPrintSetting(nHJ);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Hea_Spec_Screen()
        {
            chkStool.Enabled = false;
            chkSputum.Enabled = false;
            chkUrine.Enabled = false;
            chkStool.Checked = false;
            chkSputum.Checked = false;
            chkUrine.Checked = false;

            if (FnWRTNO == 0)
            {
                return;
            }

            List<string> lstExCD = new List<string> { "A992", "A902", "A799", "A903" };

            List<HEA_RESULT> lstHRS = heaResultService.GetListByWrtnoExCodeIN(FnWRTNO, lstExCD);

            if (lstHRS.Count > 0)
            {
                for (int i = 0; i < lstHRS.Count; i++)
                {
                    if (lstHRS[i].EXCODE.To<string>("").Trim() == "A992")   //분변검사
                    {
                        chkStool.Enabled = true;
                        if (lstHRS[i].RESULT.To<string>("").Trim() != "")
                        {
                            chkStool.Checked = true;
                        }
                    }
                    else if (lstHRS[i].EXCODE.To<string>("").Trim() == "A902" || lstHRS[i].EXCODE.To<string>("").Trim() == "A799")  //객담
                    {
                        chkSputum.Enabled = true;
                        if (lstHRS[i].RESULT.To<string>("").Trim() != "")
                        {
                            chkSputum.Checked = true;
                        }
                    }
                    else if (lstHRS[i].EXCODE.To<string>("").Trim() == "A903")  //특수소변
                    {
                        chkUrine.Enabled = true;
                        if (lstHRS[i].RESULT.To<string>("").Trim() != "")
                        {
                            chkUrine.Checked = true;
                        }
                    }
                }
            }
        }

        private void Display_Jepsu_New()
        {
            string strDate = cboYear.Text + "-01-01";

            panSelExam.Enabled = true;

            FnWRTNO = 0;
            FstrGBSTS = "";
            FstrPtno = txtPtno.Text.Trim();
            FnPano = txtPano.Text.To<long>(0);
            dtpJepDate.Text = DateTime.Now.ToShortDateString();
            dtpSDate.Text = DateTime.Now.ToShortDateString();
            rdoSTS1.Checked = true;
            dtpSDate.Enabled = true;
            //cboSTime.Enabled = true;

            cboSTime.Text = "07:30";
            dtpJepDate.Text = DateTime.Now.ToShortDateString();

            lblSTS.Text = "신규";
            lblSTS.BackColor = Color.FromArgb(255, 255, 192);

            Clear_Spread_GroupCode();
            Clear_Working_Variants();

            if (cboSex.Text == "M")
            { 
                if (VB.Left(cboJONG.Text, 2) == "12" || VB.Left(cboJONG.Text, 2) == "22")
                {
                    MessageBox.Show("검진종류가 성별과 맞지 않습니다.", "오류");
                    cboJONG.Focus();
                    return;
                }
            }
            else if (cboSex.Text == "F")
            {
                if (VB.Left(cboJONG.Text, 2) == "11" || VB.Left(cboJONG.Text, 2) == "21")
                {
                    MessageBox.Show("검진종류가 성별과 맞지 않습니다.", "오류");
                    cboJONG.Focus();
                    return;
                }
            }

            if (HaGrpCD.CODE.IsNullOrEmpty())
            {
                Group_Exams_Display();
            }

            if (VB.Left(cboJONG.Text, 1) == "2")
            {
                tbCtrl_Etc.SelectedTab = tbETCpage2;
            }
            else if (VB.Left(cboJONG.Text, 1) == "1")
            {
                tbCtrl_Etc.SelectedTab = tbETCpage1;
            }

            btnAmt.PerformClick();

            txtMail.Focus();
        }

        /// <summary>
        /// 접수번호별 그룹코드 목록 가져오기
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="argGbn"></param>
        /// <returns></returns>
        private List<READ_SUNAP_ITEM> Read_Sunap_GroupCode(long argWRTNO)
        {
            return readSunapItemService.GetHeaSunapInfoByWrtno(argWRTNO);
        }

        /// <summary>
        /// 접수번호별 검사코드 목록 가져오기
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="argGbn"></param>
        /// <returns></returns>
        private List<GROUPCODE_EXAM_DISPLAY> Read_ExamCode(long argWRTNO)
        {
            return groupCodeExamDisplayService.GetHeaListByWrtno(argWRTNO);
        }

        /// <summary>
        /// 접수번호별 수납 집계
        /// </summary>
        /// <param name="nGWRTNO"></param>
        private HIC_SUNAP Account_Sunap(long nWRTNO)
        {
            HIC_SUNAP item = new HIC_SUNAP();
            long nSeqNo = 0;

            item = hicSunapService.GetHeaSunapAmtByWRTNO(nWRTNO);

            if (!item.IsNullOrEmpty())
            {
                nSeqNo = hicSunapService.GetHeaMaxSeqbyWrtNo(nWRTNO);
                item.HALINGYE = hicSunapService.GetHeaHalinGyeByWrtnoSeqNo(nWRTNO, nSeqNo - 1);
            }
            
            return item;
        }

        /// <summary>
        /// 전체 접수 수납집계 Display
        /// </summary>
        /// <param name="nGWRTNO"></param>
        private void Display_Sunap_Amt(HIC_SUNAP item, HIC_SUNAP pSunap = null)
        {
            if (item.IsNullOrEmpty()) { panAMT.Initialize(); return; }

            txtTotAmt.Text = item.TOTAMT.ToString("#,##0");
            txtLtdAmt.Text = item.LTDAMT.ToString("#,##0");
            txtBonAmt.Text = item.BONINAMT.ToString("#,##0");
            txtMisuAmt.Text = item.MISUAMT.ToString("#,##0");
            txtHalinAmt.Text = item.HALINAMT.ToString("#,##0");

            for (int i = 0; i < cboHalinGye.Items.Count; i++)
            {
                if (VB.Pstr(cboHalinGye.Items[i].To<string>(""), ".", 1).Trim() == item.HALINGYE.To<string>("").Trim())
                {
                    cboHalinGye.SelectedIndex = i;
                    break;
                }
            }

            //본인부담은 있는데 수납된 금액이 없을때
            if (item.BONINAMT > 0 && item.SUNAPAMT1 == 0 && item.SUNAPAMT2 == 0)
            {   //수납전
                if (txtCardAmt.Text.Replace(",", "").To<long>(0) == 0 && txtCashAmt.Text.Replace(",", "").To<long>(0) == 0)
                {
                    txtCardAmt.Text = "0";  //수납금액없이 저장하는 경우도 있음
                    txtCashAmt.Text = "0";
                }
                else
                {
                    txtCashAmt.Text = (item.BONINAMT - txtCardAmt.Text.Replace(",", "").To<long>(0)).ToString("#,##0");
                }

                txtIpgumAmt.Text = item.BONINAMT.ToString("#,##0");
            }
            //2021-01-12 회사부담시 지정병원감액 적용
            else if(item.LTDAMT > 0 && item.SUNAPAMT1 == 0 && item.SUNAPAMT2 == 0)
            {
                if(item.BONINAMT == 0)
                {
                    txtLtdAmt.Text = (item.LTDAMT - item.HALINAMT).ToString("#,##0");
                }
            }
            
            else
            {
                //수납후
                txtCardAmt.Text = item.SUNAPAMT2.ToString("#,##0");
                txtCashAmt.Text = (item.BONINAMT - item.SUNAPAMT2).ToString("#,##0");
                txtIpgumAmt.Text = item.BONINAMT.ToString("#,##0");
            }

            if (pSunap != null)
            {
                txtChaAmt.Text = (item.BONINAMT - pSunap.SUNAPAMT1 - pSunap.SUNAPAMT2).ToString("#,##0");
            }
            else
            {
                txtChaAmt.Text = item.BONINAMT.ToString("#,##0");
            }

            item.SUNAPAMT = txtIpgumAmt.Text.Replace(",", "").To<long>(0);
            item.SUNAPAMT1 = txtCashAmt.Text.Replace(",", "").To<long>(0);
            item.SUNAPAMT2 = txtCardAmt.Text.Replace(",", "").To<long>(0);
        }

        private void Group_Exams_Display()
        {
            string strJong = VB.Pstr(cboJONG.Text, ".", 1);
            string strJepDate = dtpJepDate.Text;
            string strSex = cboSex.Text;
            List<string> lstGrpCD = new List<string>();

            long nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0);

            List<READ_SUNAP_ITEM> lstRSI = new List<READ_SUNAP_ITEM>();

            #region 신규 검진종류에 세팅된 묶음코드
            if (HaGrpCD.CODE.IsNullOrEmpty())
            {
                lstRSI = readSunapItemService.GetListHeaGrpCodeByJong(strJong, strJepDate, nLtdCode, strSex);
            }
            else
            {
                READ_SUNAP_ITEM rSI = readSunapItemService.GetHeaItemByCode(HaGrpCD.CODE);                
                lstRSI.Add(rSI);
            }
            
            if (lstRSI.Count > 0)
            {
                for (int i = 0; i < lstRSI.Count; i++)
                {
                    if (i != 0) 
                    {
                        //처음 1개만 남기고 모두 삭제로 체크
                        lstRSI[i].RowStatus = ComBase.Mvc.RowStatus.Delete;
                    }
                    else
                    {
                        lstGrpCD.Add(lstRSI[i].GRPCODE);
                    }

                    if (string.Compare(lstRSI[i].SUDATE, dtpJepDate.Text) > 0)
                    {
                        lstRSI[i].AMT = lstRSI[i].OLDAMT;
                    }

                    if (strJong == "11")
                    {
                        if (lstRSI[i].GRPCODE != "AM011")
                        {
                            lstRSI[i].RowStatus = ComBase.Mvc.RowStatus.Delete;
                        }
                    }

                    if (strJong == "12")
                    {
                        if (lstRSI[i].GRPCODE != "AF122")
                        {
                            lstRSI[i].RowStatus = ComBase.Mvc.RowStatus.Delete;
                        }
                    }
                }

                int nCNT = 0;
                int nLength = lstRSI.Count;
                for (int i = 0; i < nLength; i++)
                {
                    if (!lstRSI[i + nCNT].ENDOCODE.IsNullOrEmpty())
                    {
                        READ_SUNAP_ITEM iRSI = readSunapItemService.GetHeaItemByCode(lstRSI[i + nCNT].ENDOCODE);

                        if (!iRSI.IsNullOrEmpty())
                        {
                            nCNT += 1;
                            lstRSI.Insert(i + nCNT, iRSI);
                            lstRSI[i + nCNT].RowStatus = ComBase.Mvc.RowStatus.Delete;
                            //lstRSI.Add(iRSI);
                            //lstGrpCD.Add(iRSI.GRPCODE);
                        }
                    }
                }

                suInfo = lstRSI;
                ssGroup.SetDataSource(suInfo);
            }

            
            #endregion

            #region 묶음코드에 세팅된 검사코드
            if (lstGrpCD.Count > 0)
            {
                List<GROUPCODE_EXAM_DISPLAY> lstGED = groupCodeExamDisplayService.GetHeaListByGroupCode(lstGrpCD);

                if (lstGED.Count > 0)
                {

                }

                grpExam.Clear();
                grpExam.AddRange(lstGED);

                //분진검사, 수면내시경의 경우 중복검사 제외
                Check_Exam_Except(lstGrpCD, ref grpExam);

                ssExam.DataSource = null;
                ssExam.SetDataSource(grpExam);

                clsSpread.gSpdSortRow(ssExam, 0, ref bolSort, true);
            }
            
            #endregion

        }

        private void IEMunjin_Name_Display(string ArgRecvForm)
        {
            string strResult = string.Empty;

            if (VB.InStr(ArgRecvForm, "12001") > 0) { strResult += "공통,"; }
            if (VB.InStr(ArgRecvForm, "12003") > 0) { strResult += "암검진,"; }
            if (VB.InStr(ArgRecvForm, "12005") > 0) { strResult += "구강,"; }
            if (VB.InStr(ArgRecvForm, "12006") > 0) { strResult += "생애,"; }
            if (VB.InStr(ArgRecvForm, "12010") > 0) { strResult += "초등학생,"; }
            if (VB.InStr(ArgRecvForm, "12014") > 0) { strResult += "학생구강,"; }
            if (VB.InStr(ArgRecvForm, "12020") > 0) { strResult += "중고등학생,"; }
            if (VB.InStr(ArgRecvForm, "20002") > 0) { strResult += "특수,"; }
            if (VB.InStr(ArgRecvForm, "20003") > 0) { strResult += "폐활량,"; }
            if (VB.InStr(ArgRecvForm, "20004") > 0) { strResult += "흡연,"; }
            if (VB.InStr(ArgRecvForm, "20005") > 0) { strResult += "음주,"; }
            if (VB.InStr(ArgRecvForm, "20006") > 0) { strResult += "운동,"; }
            if (VB.InStr(ArgRecvForm, "20007") > 0) { strResult += "영양,"; }
            if (VB.InStr(ArgRecvForm, "20008") > 0) { strResult += "비만,"; }
            //if (VB.InStr(ArgRecvForm, "20009") > 0) { strResult += "만40우울,"; }
            //if (VB.InStr(ArgRecvForm, "20010") > 0) { strResult += "만66우울,"; }
            if (VB.InStr(ArgRecvForm, "20011") > 0) { strResult += "인지기능,"; }
            if (VB.InStr(ArgRecvForm, "20012") > 0) { strResult += "정신건강검사,"; }
            if (VB.InStr(ArgRecvForm, "30001") > 0) { strResult += "야간1차,"; }
            if (VB.InStr(ArgRecvForm, "30003") > 0) { strResult += "야간2차,"; }

            if (strResult != "")
            {
                lblMsg.Text = "▶인터넷문진: " + strResult;
                lblMsg.BackColor = Color.LightSalmon;
            }
        }

        private void MUNJIN_CHK(HIC_JEPSU_HEA_EXJONG item)
        {
            string strChk2 = "", strChk1 = "";
            string strDate = DateTime.Now.ToShortDateString();

            HIC_IE_MUNJIN_NEW iHIMN = hicIeMunjinNewService.GetItembySNamePtnoMunDate(item.SNAME, item.PTNO, strDate);

            if (!iHIMN.IsNullOrEmpty())
            {
                if (VB.InStr(iHIMN.RECVFORM, "12001") > 0) { strChk1 = "OK"; }
                if (VB.InStr(iHIMN.RECVFORM, "12003") > 0) { strChk2 = "OK"; }
            }
            if (strChk1 == "" && strChk2 == "") { MessageBox.Show("공통,암 작성여부를 확인해주세요", "확인"); }
            else if (strChk1 == "") { MessageBox.Show("공통문진표 작성여부를 확인해주세요", "확인"); }
            else if (strChk2 == "") { MessageBox.Show("암 문진표 작성여부를 확인해주세요", "확인"); }
        }

        private void ePost_value_HPAT(HIC_PATIENT item)
        {
            Hpatient = item;
        }

        /// <summary>
        /// 수검자 정보 Display
        /// </summary>
        /// <param name="argPtno">등록번호</param>
        /// <param name="argYear">검진년도</param>
        private void Display_HeaPatient_Info(string argPtno, string argYear)
        {
            HIC_PATIENT pat = hicPatientService.GetPatInfoByPtno(argPtno);

            if (!pat.IsNullOrEmpty())   //환자마스터 있음
            {
                FstrPtno = pat.PTNO;
                FnPano = pat.PANO;
                FstrBuildNo = pat.BUILDNO;
                
                panSub01.SetData(pat);
                panPAT.SetData(pat);
                
                string strJumin = clsAES.DeAES(pat.JUMIN2);

                txtJumin1.Text = VB.Left(strJumin, 6);
                txtJumin2.Text = VB.Right(strJumin, 7);
                txtPano.Text = pat.PANO.To<string>("0");
                txtAge.Text = cHB.READ_HIC_AGE_GESAN2(strJumin).ToString();

                string strCode = string.Empty;

                if (txtLtdCode.Text.Trim() == "0" || txtLtdCode.Text.Trim() == ".")
                {
                    txtLtdCode.Text = "";
                } //회사명
                else
                {
                    strCode = VB.Pstr(txtLtdCode.Text, ".", 1);
                    if (strCode.Trim() != "") { txtLtdCode.Text = strCode + "." + hicLtdService.READ_Ltd_One_Name(strCode); }
                }

                txtLastDate1.Text = comHpcLibBService.GetLastDay1ByPtno(argPtno);       //최종검진(일반)
                txtLastDate2.Text = comHpcLibBService.GetLastDay2ByPtno(argPtno);       //최종검진(암)
                txtLastDate3.Text = comHpcLibBService.GetLastDay3ByPtno(argPtno);       //최종검진(종검)
                txtJongCNT.Text   = comHpcLibBService.GetCountHeaByPtno(argPtno);       //종합검진 횟수
                txtFirstDate.Text = comHpcLibBService.GetLastDay4ByPtno(argPtno);       //최초검진(종검)

                //Excel 자료 읽기
                Excel_Data_Display(txtJumin1.Text + txtJumin2.Text, cboYear.Text, txtLtdSabun.Text);
                
                //외래감액 확인
                string strGamMsg = cHB.READ_GAM_OPD(clsAES.AES(txtJumin1.Text + txtJumin2.Text));
                if (!strGamMsg.IsNullOrEmpty())
                {
                    strGamMsg = txtSName.Text + " 고객님은 외래감액 등록되어있습니다." + ComNum.VBLF + "감액내역: " + strGamMsg;
                    MessageBox.Show(strGamMsg, "확인");
                }

                if (txtFamilly.Text.Trim() != "" && txtFamilly.Text.To<long>(0) > 0)
                {
                    txtFamilly.Text = hicPatientService.GetSNameByPano(txtFamilly.Text.To<long>(0)) + VB.Space(10) + "." + txtFamilly.Text;
                }

                //동의서 보기
                string[] strDeptCode = { "HR", "TO" };
                //frmHcPermission.SetDisplay(pat.PTNO, cboYear.Text, dtpSDate.Text, "TO");
                frmHcPermission.SetDisplay(pat.PTNO, VB.Left(dtpSDate.Text, 4), dtpSDate.Text, strDeptCode, txtSName.Text);
                //frmHcEmrPermission.SetDisplay(pat.PTNO, VB.Left(dtpSDate.Text, 4), dtpSDate.Text, strDeptCode);
            }

            panSub01.Enabled = false;
            cboJONG.Enabled = true;
        }

        private void Excel_Data_Display(string argJuminNo, string argYear, string argLtdSabun)
        {
            HEA_EXCEL item = heaExcelService.GetItemByJuminYear(clsAES.AES(argJuminNo), argYear);
            if (item.IsNullOrEmpty())
            {
                Screen_Clear_Excel();
            }
            else
            {
                SS3.ActiveSheet.Cells[0, 1].Text = txtSName.Text;
                SS3.ActiveSheet.Cells[1, 1].Text = VB.Left(argJuminNo, 6) + "-" + VB.Right(argJuminNo, 7);
                SS3.ActiveSheet.Cells[2, 1].Text = item.HDATE + " " + item.AMPM;
                SS3.ActiveSheet.Cells[3, 1].Text  = item.GJTYPE;
                SS3.ActiveSheet.Cells[4, 1].Text  = item.LTDADDEXAM;
                SS3.ActiveSheet.Cells[5, 1].Text  = item.BONINADDEXAM;
                SS3.ActiveSheet.Cells[6, 1].Text  = item.GAJOKADDEXAM;
                SS3.ActiveSheet.Cells[7, 1].Text  = item.HPHONE;
                SS3.ActiveSheet.Cells[8, 1].Text  = item.JUSO;
                SS3.ActiveSheet.Cells[9, 1].Text  = item.REMARK;
                SS3.ActiveSheet.Cells[10, 1].Text = item.JNAME;
                SS3.ActiveSheet.Cells[11, 1].Text = item.REL;
                SS3.ActiveSheet.Cells[12, 1].Text = item.TEL;
                SS3.ActiveSheet.Cells[13, 1].Text = item.MCODES;
                SS3.ActiveSheet.Cells[14, 1].Text = item.GBSAMU;
                SS3.ActiveSheet.Cells[15, 1].Text = item.GBNIGHT;
                SS3.ActiveSheet.Cells[16, 1].Text = item.GBNHIC;
                SS3.ActiveSheet.Cells[17, 1].Text = item.HOSPITAL;
                SS3.ActiveSheet.Cells[18, 1].Text = item.IPSADATE;
                SS3.ActiveSheet.Cells[19, 1].Text = item.GKIHO;
                SS3.ActiveSheet.Cells[20, 1].Text = item.LTDBUSE;
                SS3.ActiveSheet.Cells[21, 1].Text = item.JIKNAME;
                SS3.ActiveSheet.Cells[22, 1].Text = item.LTDSABUN;
                SS3.ActiveSheet.Cells[23, 1].Text = item.LTDCODE.To<string>("");
            }

            if (item.IsNullOrEmpty() && !argLtdSabun.IsNullOrEmpty())
            {
                HEA_EXCEL item1 = heaExcelService.GetItemByLtdsabunYear(argLtdSabun, argYear);
                if (!item1.IsNullOrEmpty())
                {
                    SS3.ActiveSheet.Cells[0, 1].Text = txtSName.Text;
                    SS3.ActiveSheet.Cells[1, 1].Text = VB.Left(argJuminNo, 6) + "-" + VB.Right(argJuminNo, 7);
                    //SS3.ActiveSheet.Cells[2, 1].Text = item1.HDATE + " " + item.AMPM;
                    SS3.ActiveSheet.Cells[3, 1].Text = item1.GJTYPE;
                    SS3.ActiveSheet.Cells[4, 1].Text = item1.LTDADDEXAM;
                    SS3.ActiveSheet.Cells[5, 1].Text = item1.BONINADDEXAM;
                    SS3.ActiveSheet.Cells[6, 1].Text = item1.GAJOKADDEXAM;
                    SS3.ActiveSheet.Cells[7, 1].Text = item1.HPHONE;
                    SS3.ActiveSheet.Cells[8, 1].Text = item1.JUSO;
                    SS3.ActiveSheet.Cells[9, 1].Text = item1.REMARK;
                    SS3.ActiveSheet.Cells[10, 1].Text = item1.JNAME;
                    SS3.ActiveSheet.Cells[11, 1].Text = item1.REL;
                    SS3.ActiveSheet.Cells[12, 1].Text = item1.TEL;
                    SS3.ActiveSheet.Cells[13, 1].Text = item1.MCODES;
                    SS3.ActiveSheet.Cells[14, 1].Text = item1.GBSAMU;
                    SS3.ActiveSheet.Cells[15, 1].Text = item1.GBNIGHT;
                    SS3.ActiveSheet.Cells[16, 1].Text = item1.GBNHIC;
                    SS3.ActiveSheet.Cells[17, 1].Text = item1.HOSPITAL;
                    SS3.ActiveSheet.Cells[18, 1].Text = item1.IPSADATE;
                    SS3.ActiveSheet.Cells[19, 1].Text = item1.GKIHO;
                    SS3.ActiveSheet.Cells[20, 1].Text = item1.LTDBUSE;
                    SS3.ActiveSheet.Cells[21, 1].Text = item1.JIKNAME;
                    SS3.ActiveSheet.Cells[22, 1].Text = item1.LTDSABUN;
                    SS3.ActiveSheet.Cells[23, 1].Text = item1.LTDCODE.To<string>("");
                }
            }
        }

        private void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (sender == ssETC)
            {
                if (e.Column == (int)ComHpcLibB.clsHcSpd.enmHcMemo.GBDEL)
                {
                    if (ssETC.ActiveSheet.Cells[e.Row, (int)ComHpcLibB.clsHcSpd.enmHcMemo.GBDEL].Text == "True")
                    {
                        cSpd.setSpdForeColor(ssETC, e.Row, 0, e.Row, ssETC_Sheet1.ColumnCount - 1, Color.Red);

                    }
                    else
                    {
                        if (ssETC.ActiveSheet.Cells[e.Row, (int)ComHpcLibB.clsHcSpd.enmHcMemo.CHANGE].Text == "Y")
                        {
                            cSpd.setSpdForeColor(ssETC, e.Row, 0, e.Row, ssETC_Sheet1.ColumnCount - 1, Color.Blue);
                        }
                        else
                        {
                            cSpd.setSpdForeColor(ssETC, e.Row, 0, e.Row, ssETC_Sheet1.ColumnCount - 1, Color.Black);
                        }
                    }
                }
            }
        }

        private void eSpdEditOff(object sender, EventArgs e)
        {
            int nRow = 0, nCol = 0;

            if (sender == ssETC)
            {
                nRow = ssETC.ActiveSheet.ActiveRowIndex;
                nCol = ssETC.ActiveSheet.ActiveColumnIndex;

                if (nCol == (int)ComHpcLibB.clsHcSpd.enmHcMemo.MEMO)
                {
                    Size size = ssETC.ActiveSheet.GetPreferredCellSize(nRow, nCol);
                    ssETC.ActiveSheet.Rows[nRow].Height = size.Height;

                    if (ssETC.ActiveSheet.Cells[nRow, (int)ComHpcLibB.clsHcSpd.enmHcMemo.CHANGE].Text == "")
                    {
                        ssETC.ActiveSheet.Cells[nRow, (int)ComHpcLibB.clsHcSpd.enmHcMemo.CHANGE].Text = "Y";
                        cSpd.setSpdForeColor(ssETC, nRow, 0, nRow, ssETC_Sheet1.ColumnCount - 1, Color.Blue);
                    }
                }
            }
            else if (sender == ssGroup)
            {
                nRow = ssGroup.ActiveSheet.ActiveRowIndex;
                nCol = ssGroup.ActiveSheet.ActiveColumnIndex;

                if (nCol == 3)
                {
                    suInfo[nRow].AMT = suInfo[nRow].AMT * -1;
                }

            }
        }

        private void eEndoRdoCheckChange(object sender, EventArgs e)
        {
            if (rdoEndo1.Checked)
            {
                rdoEndo1.BackColor = Color.DarkSeaGreen;
                rdoEndo2.BackColor = Color.FromArgb(224, 224, 224);
            }
            else
            {
                rdoEndo1.BackColor = Color.FromArgb(224, 224, 224);
                rdoEndo2.BackColor = Color.DarkSeaGreen;
            }
        }

        private void eMemordoChkChange(object sender, EventArgs e)
        {
            if (txtPtno.Text.Trim() == "") { return; }

            Hic_Memo_Screen(txtPtno.Text);
        }

        private void eStsRdoCheckChange(object sender, EventArgs e)
        {
            if (rdoSTS1.Checked)
            {
                rdoSTS1.BackColor = Color.DarkSeaGreen;
                rdoSTS2.BackColor = Color.FromArgb(224, 224, 224);
            }
            else
            {
                rdoSTS1.BackColor = Color.FromArgb(224, 224, 224); 
                rdoSTS2.BackColor = Color.DarkSeaGreen;
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                FstrPtnoOld = txtPtno.Text;
                Data_Save_Process();    //저장루틴
                return;
            }
            else if (sender == btnSave_Memo)
            {
                Hic_Memo_Save();
                return;
            }
            else if (sender == btnCancel)
            {
                Screen_Clear();
                return;
            }
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
                return;
            }
            else if (sender == btnHelp_Mail)
            {
                Post_Code_Help("");
                return;
            }
            else if (sender == btnHelp_Mail1)
            {
                Post_Code_Help("1");
                return;
            }
            else if (sender == btnSabun_Help)
            {
                Hic_Code_Help("SA", txtJikSabun); //추천직원
                return;
            }
            else if (sender == btnExamSearch)
            {
                if (txtSName.Text != "")
                {
                    //회사별 가예약이 가능한지 설정
                    lstGaResvLtdCodes.Clear();
                    lstGaResvLtdCodes = SET_GaResv_Ltd(VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0));

                    frmHaExamDaySet fHaExDaySet = new frmHaExamDaySet(txtSName.Text, txtPano.Text.To<long>(0), dtpSDate.Text, lstGaResvLtdCodes, cboSTime.Text, grpExam);
                    fHaExDaySet.ShowDialog();
                    cHF.fn_ClearMemory(fHaExDaySet);
                    return;
                }
            }
            else if (sender == btnAmt)
            {
                //저장된 검진정보에 ADD
                FstrBuRate = VB.Pstr(cboBuRate.Text, ".", 1).Trim();
                string strBurate = VB.Pstr(cboBuRate.Text, ".", 1).To<string>(FstrBuRate);
                string strHalinCode = VB.Pstr(cboHalinGye.Text, ".", 1).Trim();
                long nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0);

                //수납금액 계산하기
                Gesan_Sunap_Amt(suInfo, strBurate, nLtdCode, txtHalinAmt.Text.Replace(",", "").To<long>(0), strHalinCode);
                return;
            }
            else if (sender == btnGrouCode || sender == btnGrouCode_Search)
            {
                Hea_GroupCode_Help(txtGroupCode_Search.Text.Trim(), RtnCurrCodeList(suInfo));
            }
            else if (sender == btnGView)
            {
                long nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0);
                Display_Ltd_GView(nLtdCode);
            }
            else if (sender == btnLtdRemark)
            {
                //회사 참고사항
                if (VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0) > 0)
                {
                    tbCtrl_Etc.SelectedTab = tbETCpage3;
                    txtLtdRemark.Text = hicLtdService.GetHaRemarkbyLtdCode(VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0));
                }
                else
                {
                    txtLtdRemark.Text = "";
                }
            }
            //자격조회
            else if (sender == btnSearch_Nhic)
            {
                if (!txtSName.Text.IsNullOrEmpty())
                {
                    //Clear_Nhic_List();
                    Hic_Chk_Nhic("H", txtSName.Text, txtJumin1.Text + txtJumin2.Text, txtPtno.Text, cboYear.Text);
                }

                return;
            }
            //자격조회 초기화
            else if (sender == btnNhic_New)
            {
                Clear_Nhic_List();
            }
            //공단검진 수가적용
            else if (sender == btnNhic_Suga)
            {
                frmHa_HcAmtHalin fHcAmtHalin = new frmHa_HcAmtHalin(ssExam, dtpSDate.Text, txtSName.Text, txtPano.Text.To<long>());
                fHcAmtHalin.ShowDialog();
                cHF.fn_ClearMemory(fHcAmtHalin);
                return;
            }
            //접수취소
            else if (sender == btnDelete)
            {
                if (MessageBox.Show("정말로 접수취소를 하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Data_Save_Process("D");
                }

            }
            else if (sender == btnCall)
            {
                Call_Process();
                return;
            }
            else if (sender == btnMenualCall)
            {
                Menual_Call_Process();
                return;
            }
            else if (sender == btnReCall)
            {
                ReCall_Process();
                return;
            }
            else if (sender == btnFamilly)
            {
                clsHcVariable.GbFamilly_PopUp = true;
                rSetJepsuView(sender, e);
            }
            else if (sender == btnSearch_His)
            {
                frmHcPanPersonResult frm = new frmHcPanPersonResult("frmHaJepMain", txtPtno.Text, txtSName.Text);
                frm.ShowDialog();
            }
            else if (sender == btnOCS)
            {
                FrmViewResult = new frmViewResult(txtPtno.Text);
                FrmViewResult.ShowDialog();
            }
            else if (sender == btnEMR)
            {
                clsVbEmr.EXECUTE_NewTextEmrView(txtPtno.Text);
            }
            else if (sender == btnDrug)
            {
                FrmOrderDrugInfoView f = new FrmOrderDrugInfoView(txtPtno.Text);
                f.ShowDialog();
                sup.setClearMemory(f);
            }
            else if (sender == btnLtdExam)
            {
                frmHaLtdExamCompare.rSetGstrItem += new frmHaLtdExamCompare.SetGstrItem(frmHaLtdExCompare_BtnSelClcik);
                frmHaLtdExamCompare fHaLtdExamComp = new frmHaLtdExamCompare(txtLtdCode.Text, cboJONG.Text);
                fHaLtdExamComp.ShowDialog();
                frmHaLtdExamCompare.rSetGstrItem -= new frmHaLtdExamCompare.SetGstrItem(frmHaLtdExCompare_BtnSelClcik);
            }
            else if (sender == btnCard)
            {
                CardApprov_Amt("CARD");
                return;
            }
            else if (sender == btnCash)
            {
                CardApprov_Amt("CASH");
                return;
            }
            else if (sender == btnResReciept)
            {
                Save_Result_Reciept(FnWRTNO);
            }
            else if (sender == btnDur)
            {
                clsDur.DUR_CHECK_Mers(clsDB.DbCon, txtJumin1.Text + txtJumin2.Text, txtSName.Text.Trim(), DateTime.Now.ToShortDateString());
            }

            else if (sender == btnReset)
            {
                if (FnWRTNO == 0)
                {
                    return;
                }

                if (ComFunc.MsgBoxQ("접수 이전상태로 돌리시겠습니까?", "", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {

                    //접수상태 UPDATE
                    int result = heaJepsuService.UpdateGbstsCdateGbexamByWrtno("0", "N", FnWRTNO);
                    if (result < 0)
                    {
                        MessageBox.Show("HEA_JEPSU에 UPDATE 도중에 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //검체상태 UPDATE
                    int result1 = examSpecmstService.UpdateHicnoByPtno(FnWRTNO, FstrPtno, "TO");
                    if (result1 < 0)
                    {
                        MessageBox.Show("KOSMOS_OCS.EXAM_SPECMST에 UPDATE 도중에 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    //수납기록 삭제
                    List<HEA_SUNAP> list = heaSunapService.GetRowidByWrtno1(FnWRTNO);
                    if (!list.IsNullOrEmpty())
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            heaSunapService.DeleteByRowid(list[i].RID);
                        }
                    }

                    Screen_Clear();
                }
            }
            else if (sender == btnReset1)
            {
                if (ComFunc.MsgBoxQ("방금저장한 환자를 다시 불러올까요?", "", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    txtPtno.Text = FstrPtnoOld;
                    eTxtKeyDown(txtPtno, new KeyEventArgs(Keys.Enter));
                }
            }
        }
        void elblMouseUp(object sender, MouseEventArgs e)
        {
            toolTip.SetToolTip((Control)sender, ((Control)sender).Text.ToString());
        }

        private void Save_Result_Reciept(long fnWRTNO)
        {
            string strJusoGubun = "";
            if (FnWRTNO == 0) { return; }

            //결과지 수령방법
            if (rdoRES14.Checked)
            {
                if (HaJEPSU.WEBPRINTREQ.IsNullOrEmpty())
                {
                    if (!heaJepsuService.UpDateWebPrintReq(FnWRTNO, true))
                    {
                        MessageBox.Show("웹결과지 신청 등록 UPDATE 도중에 오류가 발생함", "오류");
                        return;
                    }

                    if (!comHpcLibBService.InsertWebPrintReqHea(FnWRTNO))
                    {
                        MessageBox.Show("웹결과지 신청 HISTORY INSERT 오류발생", "오류");
                        return;
                    }
                }
            }
            else
            {
                if (rdoRES12.Checked)
                {
                    // string strGbJUSO = chkRES12_1.Checked ? "Y" : "N"; //우편수령 (집, 회사)
                    string strGbJUSO = "";
                    if (chkRES12_1.Checked == true)
                    {
                        strGbJUSO = "Y";
                    }
                    else if (chkRES12_2.Checked == true)
                    {
                        strGbJUSO = "N";
                    }
                    else if (chkRES12_3.Checked == true)
                    {
                        strGbJUSO = "E";
                        //rHJ.MAILCODE = txtMail1.Text;
                        //rHJ.JUSO1 = txtJuso11.Text;
                        //rHJ.JUSO2 = txtJuso21.Text;
                    }

                    strJusoGubun = strGbJUSO;
                    if (!heaJepsuService.UpDateGbJusoByWrtno(FnWRTNO, strGbJUSO))
                    {
                        MessageBox.Show("결과지 수령방법 UPDATE시 오류발생", "오류");
                        return;
                    }

                    if (!heaJepsuService.UpDateGbChk3ByWrtno(FnWRTNO, "N"))
                    {
                        MessageBox.Show("결과지 수령방법 UPDATE시 오류발생", "오류");
                        return;
                    }
                }
                else
                {
                    if (rdoRES15.Checked)
                    {
                        string strGbChk3 = "Y";    //방문수령

                        if (!heaJepsuService.UpDateGbChk3ByWrtno(FnWRTNO, strGbChk3))
                        {
                            MessageBox.Show("결과지 수령방법 UPDATE시 오류발생", "오류");
                            return;
                        }
                    }
                }

                //웹결과지 신청 해제
                if (!heaJepsuService.UpDateWebPrintReq(FnWRTNO, false))
                {
                    MessageBox.Show("웹결과지 신청 등록 UPDATE 도중에 오류가 발생함", "오류");
                    return;
                }
            }

            HEA_JEPSU item = new HEA_JEPSU
            {
                WRTNO = FnWRTNO,
                MAILCODE = txtMail.Text.Trim(),
                JUSO1 = txtJuso1.Text.Trim(),
                JUSO2 = txtJuso2.Text.Trim()
            };


            if (strJusoGubun == "N")
            {
                HIC_LTD nHL = hicLtdService.GetMailCodebyCode(VB.Pstr(txtLtdCode.Text, ".", 1));

                if (!nHL.IsNullOrEmpty())
                {
                    item.MAILCODE = nHL.MAILCODE;
                    item.JUSO1 = nHL.JUSO;
                    if(nHL.JUSODETAIL.IsNullOrEmpty())
                    {
                        item.JUSO2 = "";
                    }
                    else
                    {
                        item.JUSO2 = nHL.JUSODETAIL;
                    }
                    
                }
            }


            if (!heaJepsuService.UpDateJusoMailCodeByItem(item))
            {
                MessageBox.Show("결과지 주소 UPDATE시 오류발생", "오류");
                return;
            }

            MessageBox.Show("결과지 수령방법 변경완료.", "확인");
            return;
        }

        private void CardApprov_Amt(string strJob)
        {
            Card CARD = new Card();
            clsPmpaFunc cPF = new clsPmpaFunc();

            string strAmt = string.Empty;

            if (strJob == "CARD")
            {
                CARD.GstrCardJob = "";
                if (txtCardAmt.Text.Trim() == "") { return; }
            }
            else
            {
                CARD.GstrCardJob = "Menual2";
                if (txtCashAmt.Text.Trim() == "") { return; }
            }

            CARD.CardVariable_Clear(ref clsPmpaType.RSD, ref clsPmpaType.RD);

            CARD.gstrCdPtno = txtPtno.Text.Trim();
            CARD.gstrCdSName = txtSName.Text.Trim();
            CARD.gstrCdDeptCode = "TO";
            CARD.gstrCdPart = clsType.User.IdNumber;
            CARD.gstrCdGbIo = "T";
            CARD.GnHPano = txtPano.Text.To<long>(0);
            CARD.GnHWRTNO = 5;

            if (strJob == "CARD")
            {
                strAmt = VB.Replace(txtCardAmt.Text, ",", "");
            }
            else
            {
                strAmt = VB.Replace(txtCashAmt.Text, ",", "");
            }

            //원단위절삭
            CARD.glngCdAmt = (long)Math.Round(VB.Val(strAmt), 0, MidpointRounding.AwayFromZero);
            //CARD.glngCdAmt = Convert.ToInt32(strAmt);

            CARD.gstrCdPCode = "SUP+";

            frmHcEntryCardDaou frm = new frmHcEntryCardDaou(CARD.gstrCdPtno, CARD.gstrCdSName, CARD.gstrCdDeptCode, CARD.gstrCdGbIo, CARD.glngCdAmt, strJob, clsPublic.GstrSysDate, CARD.GnHPano, CARD.GnHWRTNO);
            frm.ShowDialog();
            cPF.fn_ClearMemory(frm);

        }

        private List<READ_SUNAP_ITEM> RtnCurrCodeList(List<READ_SUNAP_ITEM> suInfo)
        {
            List<READ_SUNAP_ITEM> rtnLst = new List<READ_SUNAP_ITEM>();

            //현재 작업중인 그룹코드 List중 유해물질이거나 추가검사 코드인 경우만 List로 반환
            for (int i = 0; i < suInfo.Count; i++)
            {   //제외처리 한 코드는 제외
                if (suInfo[i].RowStatus != RowStatus.Delete)
                {
                    rtnLst.Add(suInfo[i]);
                }
            }

            return rtnLst;
        }

        private void Call_Process()
        {
            int nSeqNo = 0;
            string strPcNo = "";
            string strJumin = "";
            string strName = "";

            timer1.Enabled = false;

            FileInfo nFILE = null;

            string strFIleNm = @"c:\HIC_WAIT.ini";
            nFILE = new FileInfo(strFIleNm);

            if (nFILE.Exists == false) { return; }

            StreamReader SR = new StreamReader(strFIleNm, System.Text.Encoding.Default);

            strPcNo = SR.ReadToEnd();

            SR.Close();

            strPcNo = VB.Pstr(VB.Pstr(strPcNo, "{}", 1), "=", 2);
            strPcNo = VB.Pstr(strPcNo, "번", 1);

            if (strPcNo.IsNullOrEmpty())
            {
                MessageBox.Show("PC번호가 C:\\HIC_WAIT.INI에 설정이 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ssWait.ActiveSheet.RowCount == 0)
            {
                return;
            }

            nSeqNo = ssWait.ActiveSheet.Cells[0, 0].Text.To<int>();
            strName = ssWait.ActiveSheet.Cells[0, 1].Text;
            strJumin = ssWait.ActiveSheet.Cells[0, 3].Text;

            if (nSeqNo == 0) { timer1.Enabled = true; return; }

            Screen_Clear();

            //FstrGaJepFalg = ""

            txtJumin1.Text = VB.Left(strJumin, 6);
            txtJumin2.Text = VB.Right(strJumin, 7);

            if (strName.IsNullOrEmpty())
            {
                txtSName.Text = nSeqNo.To<string>() + "번";
            }

            if (!hicWaitService.UpDateCall(strPcNo, "2", nSeqNo))
            {
                MessageBox.Show("호출실패!", "오류");
            }

            if (strPcNo != "")
            {
                if (!hicWaitService.UpDateCallWaitPC(strPcNo, "1", "1", strName, nSeqNo))
                {
                    MessageBox.Show("호출대기등록 실패!", "오류");
                }
            }

            lblCall.Text = nSeqNo.To<string>("0");

            txtJumin2.Focus();

            eTxtKeyDown(txtJumin2, new KeyEventArgs(Keys.Enter));

            timer1.Enabled = true;
            btnCall.Enabled = true;

            FbCall = true;

        }

        private void ReCall_Process()
        {
            string strPcNo = "";

            FileInfo nFILE = null;

            string strFIleNm = @"c:\HIC_WAIT.ini";
            nFILE = new FileInfo(strFIleNm);

            if (nFILE.Exists == false) { return; }

            StreamReader SR = new StreamReader(strFIleNm, System.Text.Encoding.Default);

            strPcNo = SR.ReadToEnd();

            SR.Close();

            strPcNo = VB.Pstr(VB.Pstr(strPcNo, "{}", 1), "=", 2);
            strPcNo = VB.Pstr(strPcNo, "번", 1);

            if (strPcNo.IsNullOrEmpty())
            {
                MessageBox.Show("PC번호가 C:\\HIC_WAIT.INI에 설정이 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (strPcNo != "")
            {
                if (!hicWaitService.UpDateReCallWaitPC(strPcNo, "1", "1"))
                {
                    MessageBox.Show("호출대기등록 실패!", "오류");
                }
            }
        }

        private void Menual_Call_Process()
        {
            //실시간으로 대기원이 보이게 요청함
            //timer1.Enabled = false;

            //창띄움
            clsHcVariable.GstrTempValue = "";
            frmHcWaitList_Hea FrmHcWaitList_Hea = new frmHcWaitList_Hea();
            frmHcWaitList_Hea.rSetGstrValue += new frmHcWaitList_Hea.SetGstrValue(frmHcWaitList_Hea_ssDblclick);
            FrmHcWaitList_Hea.ShowDialog(this);
            frmHcWaitList_Hea.rSetGstrValue -= new frmHcWaitList_Hea.SetGstrValue(frmHcWaitList_Hea_ssDblclick);

            FbCall = true;
        }

        private void frmHcWaitList_Hea_ssDblclick(string argTemp1, string argTemp2, string argTemp3)
        {
            if (argTemp1.IsNullOrEmpty())
            {
                timer1.Enabled = true;
                return;
            }

            Screen_Clear();

            string strPcNo = "";

            FileInfo nFILE = null;

            string strFIleNm = @"c:\HIC_WAIT.ini";
            nFILE = new FileInfo(strFIleNm);

            if (nFILE.Exists == false) { return; }

            StreamReader SR = new StreamReader(strFIleNm, System.Text.Encoding.Default);

            strPcNo = SR.ReadToEnd();

            SR.Close();

            strPcNo = VB.Pstr(VB.Pstr(strPcNo, "{}", 1), "=", 2);
            strPcNo = VB.Pstr(strPcNo, "번", 1);

            if (strPcNo.IsNullOrEmpty())
            {
                MessageBox.Show("PC번호가 C:\\HIC_WAIT.INI에 설정이 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int nSeqNo = argTemp1.To<int>();
            string strName = argTemp2;
            string strJumin = argTemp3;

            txtJumin1.Text = VB.Left(strJumin, 6);
            txtJumin2.Text = VB.Right(strJumin, 7);

            if (strName.IsNullOrEmpty())
            {
                txtSName.Text = nSeqNo.To<string>() + "번";
            }

            if (!hicWaitService.UpDateCall(strPcNo, "2", nSeqNo))
            {
                MessageBox.Show("호출실패!", "오류");
            }

            if (strPcNo != "")
            {
                if (!hicWaitService.UpDateCallWaitPC(strPcNo, "1", "1", strName, nSeqNo))
                {
                    MessageBox.Show("호출대기등록 실패!", "오류");
                }
            }

            lblCall.Text = nSeqNo.To<string>("0");

            txtJumin2.Focus();

            eTxtKeyDown(txtJumin2, new KeyEventArgs(Keys.Enter));

            timer1.Enabled = true;
            btnMenualCall.Enabled = true;

        }

        /// <summary>
        /// 수검자 공단 자격조회 Main Rutine
        /// </summary>
        /// <param name="argGbn"></param>
        /// <param name="argSName"></param>
        /// <param name="argJuminNo"></param>
        /// <param name="argPtno"></param>
        /// <param name="argYear"></param>
        /// <param name="nIdx"></param>
        /// <param name="argWork"></param> 가접수대상여부
        private void Hic_Chk_Nhic(string argGbn, string argSName, string argJuminNo, string argPtno, string argYear, int nIdx = 0, string argWork = "")
        {
            panExamDtl.Expanded = true;

            //당일 조회된 자격내역이 있는지 점검
            WORK_NHIC item = workNhicService.GetNhicInfo(argGbn, clsAES.AES(argJuminNo), argSName, argYear, "1");

            if (item.IsNullOrEmpty())
            {
                //신규자격조회
                frmHcNhicSub frm = new frmHcNhicSub(argSName, argJuminNo, argYear, "H", argPtno);
                frm.ShowDialog();

                WORK_NHIC item2 = workNhicService.GetNhicInfo(argGbn, clsAES.AES(argJuminNo), argSName, argYear, "1");

                Screen_Clear_Nhic();
                //자격조회 정보 Spread Display
                SetDisPlay_Nhic(item2);

                //자격조회 정보 변수 대입(가접수시 제외)
                if (argWork != "OK")
                {
                    cHcMain.Display_Nhic_Info(item2);
                }

                Set_Exam_Select();
            }
            else
            {
                Screen_Clear_Nhic();
                //자격조회 정보 Spread Display
                SetDisPlay_Nhic(item);

                //자격조회 정보 변수 대입(가접수시 제외)
                if (argWork != "OK")
                {
                    cHcMain.Display_Nhic_Info(item);
                }

                Set_Exam_Select();
            }

            if (clsHcType.THNV.hJaGubun.IsNullOrEmpty())
            {
                MessageBox.Show("자격이 없습니다.", "확인요망");
                return;
            }

            //2020-09-07
            if (clsHcType.TEC.EXAMC.Contains("Y"))
            {
                MessageBox.Show("C형간염 대상자입니다.", "확인요망");
            }

        }

        /// <summary>
        /// 회사 제안서 파일 보기
        /// </summary>
        /// <param name="nLtdCode"></param>
        private void Display_Ltd_GView(long nLtdCode)
        {
            if (nLtdCode == 0) { return; }

            string strLtdCode = VB.Format(nLtdCode, "00000");
            string strYear = VB.Left(dtpJepDate.Text, 4);
            string strExtension = string.Empty;
            string strFileName = string.Empty;
            string strServerName = string.Empty;

            DirectoryInfo Dir = new DirectoryInfo(@"c:\HICEXCEL");
            if (!Dir.Exists)
            {
                Dir.Create();
            }

            COMHPC item = comHpcLibBService.GetGViewLtdByLtdCode(strLtdCode, strYear);

            if (item.IsNullOrEmpty() || item.DATA1.IsNullOrEmpty())
            {
                MessageBox.Show("첨부파일이 없습니다.", "확인");
                return;
            }

            if (VB.Mid(VB.Right(item.DATA1.Trim(), 4), 1, 1) == ".")
            {
                strExtension = "*" + VB.Right(item.DATA1.Trim(), 4);
            }
            else if (VB.Mid(VB.Right(item.DATA1.Trim(), 5), 1, 1) == ".")
            {
                strExtension = "*" + VB.Right(item.DATA1.Trim(), 5);
            }

            if (VB.Right(item.DATA1.Trim(), 4) == "xlsx")
            {
                strFileName = @"c:\HICEXCEL\AAA.xlsx";
            }
            else
            {
                strFileName = @"c:\HICEXCEL\AAA" + VB.Right(item.DATA1, 4);
            }

            //strServerName = "/data/DOCU_READING/" + item.DATA1.Trim();
            strServerName = "/data/DOCU_READING";

            Ftpedt FtpedtX = new Ftpedt();
            if (FtpedtX.FtpDownload("192.168.100.31", "oracle", FtpedtX.READ_FTP(clsDB.DbCon, "192.168.100.31", "oracle"), strFileName, item.DATA1.Trim(), strServerName) == true)
            {
                System.Diagnostics.Process.Start(strFileName);
            }

            FtpedtX = null;
        }

        private void Gesan_Sunap_Amt(List<READ_SUNAP_ITEM> suInfo, string argBuRate, long argLtdCode, long argHalinAmt, string argHalinCode = null)
        {
            HIC_SUNAP pSunap = null;

            //검사항목 코드 재계산
            HIC_SUNAP item = hicSunapService.GetSumHeaAmtByGroupCode(suInfo, argBuRate, argLtdCode, argHalinAmt, FstrJong, argHalinCode);

            item.PANO = FnPano;
            item.WRTNO = FnWRTNO;
            item.PTNO = FstrPtno;
            item.JEPDATE = dtpJepDate.Text;
            item.SNAME = txtSName.Text;
            item.DEPTCODE = "종합건진(TO)";
            item.HALINGYE = VB.Pstr(cboHalinGye.Text, ".", 1);

            //이전 수납정보
            if (FnWRTNO > 0)
            {
                pSunap = hicSunapService.GetHeaSunapAmtByWRTNO(FnWRTNO);
            }
            
            Display_Sunap_Amt(item, pSunap);

            sunap = item;

            Clear_Spread_GroupCode("Group");
            ssGroup.DataSource = null;
            ssGroup.SetDataSource(suInfo);

            for (int i = 0; i < ssGroup.ActiveSheet.RowCount; i++)
            {
                if (ssGroup.ActiveSheet.Cells[i, 1].Text.Trim() == "YY001")
                {
                    TextCellType cellType = new TextCellType();

                    ssGroup.ActiveSheet.Cells[i, 2].Locked = false;
                    ssGroup.ActiveSheet.Cells[i, 3].Locked = false;
                    
                    ssGroup.ActiveSheet.Cells[i, 5].CellType = cellType;
                    ssGroup.ActiveSheet.Cells[i, 5].Locked = true;

                    ssGroup.ActiveSheet.Cells[i, 0, i, ssGroup.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;
                }
            }
        }

        /// <summary>
        /// 회사별 가예약이 가능한지 설정
        /// </summary>
        /// <param name="nLtdCode"></param>
        /// <returns></returns>
        private List<long> SET_GaResv_Ltd(long nLtdCode)
        {
            List<long> rtnList = new List<long>();

            clsHcVariable.GbGaResvLtd = false;

            string strGbResv = hicLtdService.GetGbResvByCode(nLtdCode);

            if (strGbResv == "Y") { clsHcVariable.GbGaResvLtd = true; }

            List<HIC_LTD> lstLTD = hicLtdService.GetListGaResv();
            if (lstLTD.Count > 0)
            {
                for (int i = 0; i < lstLTD.Count; i++)
                {
                    rtnList.Add(lstLTD[i].CODE);
                }
            }

            return rtnList;
        }

        /// <summary>
        /// 검진 기초코드 검색창 연동
        /// </summary>
        /// <param name="strGB"></param>
        /// <param name="tx"></param>
        /// <param name="lb"></param>
        private void Hic_Code_Help(string strGB, TextBox tx)
        {
            string strFind = "";

            if (tx.Text.Contains("."))
            {
                strFind = VB.Pstr(tx.Text, ".", 2).Trim();
            }
            else
            {
                strFind = tx.Text.Trim();
            }

            frmHaCodeHelp frm = new frmHaCodeHelp(strGB, strFind);
            frm.rSetGstrValue += new frmHaCodeHelp.SetGstrValue(ePost_value_CODE);
            frm.ShowDialog();

            if (!CodeHelpItem.CODE.IsNullOrEmpty() && !CodeHelpItem.IsNullOrEmpty())
            {
                tx.Text = CodeHelpItem.CODE.Trim();
                tx.Text += "." + CodeHelpItem.NAME.Trim();
            }
            else
            {
                if (VB.Pstr(tx.Text, ".", 1).Trim() == "") { tx.Text = ""; }
            }
        }

        private void ePost_value_CODE(HEA_CODE item)
        {
            CodeHelpItem = item;
        }

        /// <summary>
        /// 도로명 주소 검색창 연동
        /// </summary>
        private void Post_Code_Help(string strGubun)
        {
            clsHcVariable.GstrValue = "";
            clsPublic.GstrMsgList = "";

            frmSearchRoadWeb frm = new frmSearchRoadWeb();
            frm.rSetGstrValue += new frmSearchRoadWeb.SetGstrValue(ePost_value);
            frm.ShowDialog();

            if (strGubun.IsNullOrEmpty())
            {
                if (!clsHcVariable.GstrValue.IsNullOrEmpty())
                {
                    txtMail.Text = VB.Left(VB.Pstr(clsHcVariable.GstrValue, "|", 1), 3);
                    txtMail.Text += VB.Mid(VB.Pstr(clsHcVariable.GstrValue, "|", 1), 4, 2);
                    txtJuso1.Text = VB.Pstr(clsHcVariable.GstrValue, "|", 2).Trim();
                    txtJuso2.Text = "";

                    FstrBuildNo = VB.Pstr(clsHcVariable.GstrValue, "|", 5).Trim();
                    txtJuso2.Focus();
                }
                else
                {
                    FstrBuildNo = "";
                    txtJuso2.Focus();
                }
            }
            else if (strGubun == "1")
            {
                if (!clsHcVariable.GstrValue.IsNullOrEmpty())
                {
                    txtMail1.Text = VB.Left(VB.Pstr(clsHcVariable.GstrValue, "|", 1), 3);
                    txtMail1.Text += VB.Mid(VB.Pstr(clsHcVariable.GstrValue, "|", 1), 4, 2);
                    txtJuso11.Text = VB.Pstr(clsHcVariable.GstrValue, "|", 2).Trim();
                    txtJuso21.Text = "";

                    FstrBuildNo = VB.Pstr(clsHcVariable.GstrValue, "|", 5).Trim();
                    txtJuso21.Focus();
                }
                else
                {
                    FstrBuildNo = "";
                    txtJuso21.Focus();
                }
            }     
        }

        private void ePost_value(string GstrValue)
        {
            clsHcVariable.GstrValue = GstrValue;
        }

        /// <summary>
        /// 사업장 코드 검색창 연동
        /// </summary>
        private void Ltd_Code_Help()
        {
            string strFind = "";

            if (txtLtdCode.Text.Contains("."))
            {
                strFind = VB.Pstr(txtLtdCode.Text, ".", 2).Trim();
            }
            else
            {
                strFind = txtLtdCode.Text.Trim();
            }

            frmHcLtdHelp frm = new frmHcLtdHelp(strFind);
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();
            frm.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);

            if (!LtdHelpItem.IsNullOrEmpty() && LtdHelpItem.CODE > 0)
            {
                txtLtdCode.Text = LtdHelpItem.CODE.To<string>();
                txtLtdCode.Text += "." + LtdHelpItem.SANGHO;
                eBtnClick(btnLtdRemark, new EventArgs());
            }
            else
            {
                if (VB.Pstr(txtLtdCode.Text, ",", 1).Trim() == "")
                {
                    txtLtdCode.Text = "";
                }
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void eCboKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (sender == cboJONG)
                {
                    FstrJong = VB.Pstr(cboJONG.Text, ".", 1);
                    if (FstrJong == "") { return; }

                    FstrBuRate = heaExjongService.GetBuRateByGjJong(FstrJong);
                    cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate, 0);

                    Display_Jepsu_New();
                    
                }
                else if (sender == cboHalinGye)
                {
                    Gesan_HalinAmt(VB.Left(cboHalinGye.Text, 3), VB.Left(cboJONG.Text, 2), VB.Pstr(txtLtdCode.Text, ".", 1), cboSex.Text);
                    btnAmt.PerformClick();
                }
            }
        }

        private void Gesan_HalinAmt(string argHalinGye, string argJong, string argLtdCode, string argSex)
        {
            long nGamAmt = 0;

            //회사코드가 있고 개인종검은 회사100%
            //10인미만 회사 단체는 918.지정병원감액 5만원기본할인 또는 임의감액 가능
            if (argLtdCode.To<long>() > 0 && argLtdCode != "42276" && (argJong == "11" || argJong == "12"))
            {
                if (argHalinGye == "943")
                {
                    if (VB.Replace(txtHalinAmt.Text, ",", "").To<long>() == 0)
                    {
                        txtHalinAmt.Text = "50,000";
                        nGamAmt = 50000;
                    }
                }
                else
                {
                    cboHalinGye.SelectedIndex = cboHalinGye.FindString("918");
                    txtHalinAmt.Text = "50,000";
                    nGamAmt = 50000;
                }
            }
            else if (clsHcType.HaGamFlag == false && argHalinGye == "942" && VB.Replace(txtHalinAmt.Text, ",", "").To<long>(0) == 0)
            {
                clsPublic.GstrMsgList = "일반검진 수가항목이 선택되어있지 않습니다." + ComNum.VBLF;
                clsPublic.GstrMsgList += "검진항목감액은 검진코드를 선택하여야 적용됩니다." + ComNum.VBLF;
                clsPublic.GstrMsgList += "일반검진항목을 선택하여 주십시오." + ComNum.VBLF;

                MessageBox.Show(clsPublic.GstrMsgList, "할인코드 직접선택 불가!");

                cboHalinGye.SelectedIndex = 0;
                txtHalinAmt.Text = "0";
                return;
            }
            else
            {
                HEA_GAMCODE haGAMCD = heaGamcodeService.GetItemByCode(argHalinGye);

                if (haGAMCD.IsNullOrEmpty())
                {
                    txtHalinAmt.Text = "0";
                    return;
                }

                if (!haGAMCD.IsNullOrEmpty())
                {
                    //할인금액 적용
                    if (haGAMCD.GUBUN != "3")
                    {
                        txtHalinAmt.Text = "0";
                    }
                }


                string strGbSelect =  "";

                for (int i = 0; i < suInfo.Count; i++)
                {
                    strGbSelect = heaGroupcodeService.GetGbSelectByCode(suInfo[i].GRPCODE);

                    //944.초대권+추가검사 50% 할인
                    if (argHalinGye == "944")
                    {
                        //기본검사는 100% 할인(초대권)
                        if (strGbSelect != "Y" && VB.Left(suInfo[i].GRPCODE, 1) == "A")
                        {
                            if (suInfo[i].AMT > 0)
                            {
                                if (argSex == "M")
                                {
                                    nGamAmt = nGamAmt + (suInfo[i].AMT - haGAMCD.AMT1);
                                }
                                else
                                {
                                    nGamAmt = nGamAmt + (suInfo[i].AMT - haGAMCD.AMT2);
                                }

                                if (nGamAmt < 0) { nGamAmt = suInfo[i].AMT; }
                            }
                        }
                        else
                        {
                            if (suInfo[i].GRPCODE != "YY001")
                            {
                                if (suInfo[i].ISDELTE != "Y" && suInfo[i].GBHALIN == "Y")
                                {
                                    if (suInfo[i].AMT > 0)
                                    {
                                        nGamAmt = nGamAmt + (long)Math.Truncate(suInfo[i].AMT * 0.5);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //기본코드 금액에서 감액코드 금액을 뺌
                        if (suInfo[i].ISDELTE != "Y" && suInfo[i].GBHALIN == "Y")
                        {
                            if (suInfo[i].GRPCODE.To<string>("").Trim() != "YY001" && suInfo[i].AMT > 0)
                            {
                                if (haGAMCD.RATE.To<long>(0) > 0)
                                {
                                    nGamAmt = nGamAmt + (long)Math.Truncate((suInfo[i].AMT * haGAMCD.RATE / 100.0));
                                }
                                else
                                {
                                    if (argSex == "M")
                                    {
                                        nGamAmt = nGamAmt + haGAMCD.AMT1;
                                    }
                                    else
                                    {
                                        nGamAmt = nGamAmt + haGAMCD.AMT2;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (nGamAmt < 0) { nGamAmt = 0; }

            long nSpcHalAmt = 0;
            //장기근속자 감액관련 강제세팅
            if (FnWRTNO > 0)
            {
                nSpcHalAmt = heaGroupcodeService.GetLongServiceWorkerTotalAmtByWrtno(FnWRTNO);
            }

            if (nSpcHalAmt > 0)
            {
                if (cboHalinGye.Text.Trim() == "")
                {
                    cboHalinGye.Text = "936.장기근속검진";
                    nGamAmt = nGamAmt + nSpcHalAmt;
                }
            }

            //할인금액
            txtHalinAmt.Text = nGamAmt.ToString("#,##0");
        }

        private void Gesan_HalinAmt_Old(string argHalinGye, string argJong, string argLtdCode, string argSex)
        {
            long nGamAmt = 0;

            //회사코드가 있고 개인종검은 회사100%
            //10인미만 회사 단체는 918.지정병원감액 5만원기본할인 또는 임의감액 가능
            if (argLtdCode.To<long>() > 0 && argLtdCode != "42276" && (argJong == "11" || argJong == "12"))
            {
                if (argHalinGye == "943")
                {
                    if (VB.Replace(txtHalinAmt.Text, ",", "").To<long>() == 0)
                    {
                        txtHalinAmt.Text = "50,000";
                    }
                }
                else
                {
                    cboHalinGye.SelectedIndex = cboHalinGye.FindString("918");
                    txtHalinAmt.Text = "50,000";
                }
            }
            else if (clsHcType.HaGamFlag == false && argHalinGye == "942" && VB.Replace(txtHalinAmt.Text, ",", "").To<long>(0) == 0)
            {
                clsPublic.GstrMsgList = "일반검진 수가항목이 선택되어있지 않습니다." + ComNum.VBLF;
                clsPublic.GstrMsgList += "검진항목감액은 검진코드를 선택하여야 적용됩니다." + ComNum.VBLF;
                clsPublic.GstrMsgList += "일반검진항목을 선택하여 주십시오." + ComNum.VBLF;

                MessageBox.Show(clsPublic.GstrMsgList, "할인코드 직접선택 불가!");

                cboHalinGye.SelectedIndex = 0;
                txtHalinAmt.Text = "0";
                return;
            }
            else if (txtLtdCode.Text.Trim() != "" && argHalinGye == "939")
            {
                clsPublic.GstrMsgList = "939.초대권+본인부담30% 할인은 개인종검만" + ComNum.VBLF;
                clsPublic.GstrMsgList += "가능합니다. 회사검진은 다른 할인계정을 사용하세요." + ComNum.VBLF;

                MessageBox.Show(clsPublic.GstrMsgList, "오류");

                cboHalinGye.SelectedIndex = 0;
                txtHalinAmt.Text = "0";
                return;
            }
            else if (argHalinGye == "925")
            {
                if (hicBcodeService.GetCountbyGubunCode("HEA_공단검진비회사부담감액", argLtdCode) == 0)
                {
                    clsPublic.GstrMsgList = "회사부담 감액 대상 회사가 아닙니다.";

                    MessageBox.Show(clsPublic.GstrMsgList, "오류");

                    cboHalinGye.SelectedIndex = 0;
                    txtHalinAmt.Text = "0";
                    return;
                }
                //TODO : 하드코딩 없애기
                txtHalinAmt.Text = "37,650";
            }
            else if (argHalinGye == "945")
            {
                if (hicBcodeService.GetCountbyGubunCode("HEA_공단검진비회사부담감액", argLtdCode) == 0)
                {
                    clsPublic.GstrMsgList = "회사부담 감액 대상 회사가 아닙니다.";

                    MessageBox.Show(clsPublic.GstrMsgList, "오류");

                    cboHalinGye.SelectedIndex = 0;
                    txtHalinAmt.Text = "0";
                    return;
                }
                //TODO : 하드코딩 없애기
                txtHalinAmt.Text = "30,770";
            }
            else
            {
                HEA_GAMCODE haGAMCD = heaGamcodeService.GetItemByCode(argHalinGye);

                if (!haGAMCD.IsNullOrEmpty())
                {
                    //할인금액 적용
                    if (haGAMCD.GUBUN != "3")
                    {
                        txtHalinAmt.Text = "";
                    }
                }

                
                string strGbSelect =  "";

                for (int i = 0; i < suInfo.Count; i++)
                {
                    strGbSelect = heaGroupcodeService.GetGbSelectByCode(suInfo[i].GRPCODE);

                    //939.초대권+추가검사 30% 할인
                    if (argHalinGye == "939")
                    {   
                        //기본검사는 100% 할인(초대권)
                        if (strGbSelect != "Y" && VB.Left(suInfo[i].GRPCODE, 1) == "A")
                        {
                            nGamAmt = nGamAmt + suInfo[i].AMT;
                        }
                        else
                        { 
                            if (suInfo[i].GRPCODE != "YY001")
                            { 
                                if (suInfo[i].ISDELTE != "Y" && suInfo[i].GBHALIN == "Y")
                                {
                                    nGamAmt = nGamAmt + (long)Math.Truncate(suInfo[i].AMT * 0.3);
                                }
                            }
                        }
                    }
                    //944.초대권+추가검사 50% 할인
                    else if (argHalinGye == "944")
                    {
                        //기본검사는 100% 할인(초대권)
                        if (strGbSelect != "Y" && VB.Left(suInfo[i].GRPCODE, 1) == "A")
                        {
                            nGamAmt = nGamAmt + suInfo[i].AMT;
                        }
                        else
                        {
                            if (suInfo[i].GRPCODE != "YY001")
                            {
                                if (suInfo[i].ISDELTE != "Y" && suInfo[i].GBHALIN == "Y")
                                {
                                    nGamAmt = nGamAmt + (long)Math.Truncate(suInfo[i].AMT * 0.5);
                                }
                            }
                        }
                    }
                    else
                    {
                        //기본코드 금액에서 감액코드 금액을 뺌
                        if (suInfo[i].ISDELTE != "Y" && suInfo[i].GBHALIN == "Y")
                        {
                            if (suInfo[i].GRPCODE.To<string>("").Trim() != "YY001")
                            {
                                if (haGAMCD.RATE > 0)
                                {
                                    nGamAmt = VB.Replace(txtHalinAmt.Text, ",", "").To<long>(0) + (suInfo[i].AMT * (long)Math.Truncate(haGAMCD.RATE / 100.0));
                                }
                                else
                                {
                                    if (argSex == "M")
                                    {
                                        nGamAmt = VB.Replace(txtHalinAmt.Text, ",", "").To<long>(0) + haGAMCD.AMT1;
                                    }
                                    else
                                    {
                                        nGamAmt = VB.Replace(txtHalinAmt.Text, ",", "").To<long>(0) + haGAMCD.AMT2;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            long nSpcHalAmt = 0;
            //장기근속자 감액관련 강제세팅
            if (FnWRTNO > 0)
            {
                nSpcHalAmt = heaGroupcodeService.GetLongServiceWorkerTotalAmtByWrtno(FnWRTNO);
            }

            if (nSpcHalAmt > 0)
            {
                if (cboHalinGye.Text.Trim() == "")
                {
                    cboHalinGye.Text = "936.장기근속검진";
                    nGamAmt = VB.Replace(txtHalinAmt.Text, ",", "").To<long>(0) + nSpcHalAmt;
                }
            }

            //할인금액
            txtHalinAmt.Text = nGamAmt.ToString("#,##0");
        }

        private void Data_Save_Process(string argJob = "")
        {
            List<string> strJob = new List<string>();

            bool bNEW = false;
            int result = 0;

            long nCOUNT = 0;
            string strCODE = "";
            string strXrayRowid = "";
            
            if (argJob == "")
            {
                #region 필수입력 사항 체크
                if (!panSub01.RequiredValidate())
                {
                    MessageBox.Show("접수상단 필수 입력항목이 누락되었습니다.");
                    return;
                }

                if (!panPAT.RequiredValidate())
                {
                    MessageBox.Show("수검자정보 필수 입력항목이 누락되었습니다.");
                    return;
                }

                if (!panBuRate.RequiredValidate())
                {
                    MessageBox.Show("부담율 필수 입력항목이 누락되었습니다.");
                    return;
                }

                if (!panJepList.RequiredValidate())
                {
                    MessageBox.Show("검진예약 시간이 공란입니다.");
                    return;
                }

                if (txtMail.Text.Length != 5)
                {
                    MessageBox.Show("우편번호는 5자리만 가능합니다.", "오류");
                    return;
                }
                if (ComFunc.LenH(txtJuso1.Text) > 70)
                {
                    MessageBox.Show("주소 길이를 확인해주세요.", "오류");
                    return;
                }

                //2021-01-26(메인묶음코드 2개시 접수불가)
                for (int i = 0; i < ssGroup.ActiveSheet.RowCount; i++)
                {
                    if (ssGroup.ActiveSheet.Cells[i, 0].Text != "Y")
                    {
                        strCODE = ssGroup.ActiveSheet.Cells[i, 1].Text;
                        if (!heaGroupcodeService.GetJongByCode(strCODE).IsNullOrEmpty())
                        {
                            nCOUNT = nCOUNT + 1;
                        }
                    }
                }

                if (nCOUNT > 1 )
                {
                    MessageBox.Show("기본코드가 2개이상 선택되었습니다.");
                    return;
                }

                #endregion
            }

            try
            {
                if (rdoSTS1.Checked)            //예약
                {
                    switch (FstrGBSTS)
                    {
                        case "": FstrGBSTS = "0"; bNEW = true; break;
                        case "0":  break;
                        default: bNEW = false; break;
                    }
                }
                else if (rdoSTS2.Checked)       //접수
                {
                    switch (FstrGBSTS)
                    {
                        case "": FstrGBSTS = "1"; break;
                        case "0": FstrGBSTS = "1"; break;
                        default: break;
                    }
                }

                if (argJob == "D")
                {
                    //접수취소
                    FstrGBSTS = "D";

                    if (Jepsu_Del_Check_Logic() == false) { return; }

                    clsDB.setBeginTran(clsDB.DbCon);

                    //선택검사일정 삭제
                    if (!heaResvExamService.UpDateDelDateByPanoSDate(txtPano.Text.To<long>(0), dtpSDate.Text))
                    {
                        MessageBox.Show("환자마스타에 UPDATE 도중에 오류가 발생함", "오류");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    //외래접수내역 삭제
                    if (!comHpcLibBService.DelOpdMasterByPanoBDateDept(txtPtno.Text, dtpSDate.Text, "TO"))
                    {
                        MessageBox.Show("외래접수 삭제도중에 오류가 발생함", "오류");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    //알림톡 문자 삭제
                    if (!etcAlimTalkService.UpDateDelDateByPanoRDateTmpCD(txtPano.Text.To<long>(0), dtpSDate.Text, "C_MJ_001_02_12446"))
                    {
                        MessageBox.Show("알림톡 문자 삭제시 오류가 발생함", "오류");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    //2020-07-27(종합검진 취소시 일반검진 당일검진접수건 JongGumYN:0 업데이트)
                    if (!hicJepsuService.UpDateJonggumYNByPtnoJepDate(txtPtno.Text, dtpSDate.Text, "0"))
                    {
                        MessageBox.Show("일반검진 종합검진여부 업데이트중에 오류가 발생함", "오류");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    //내시경예약취소(2020-12-23)
                    result = endoJupmstService.UpDateGbsunapByPtnoRDate(txtPtno.Text, "*", "TO");
                    if (result < 0)
                    {
                        MessageBox.Show("내시경접수취소 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsQuery.NEW_TextEMR_TreatInterface(clsDB.DbCon, txtPtno.Text, dtpSDate.Text, "TO", "TO", "취소", "99917");

                    clsDB.setCommitTran(clsDB.DbCon);
                }
                else
                {
                    if (Jepsu_Check_Logic() == false) { return; }
                }

                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(clsDB.DbCon);

                //선택검사일정 저장
                if (clsHcVariable.LSTHaRESEXAM.Count > 0)
                {
                    if (!heaResvExamService.Save(clsHcVariable.LSTHaRESEXAM))
                    {
                        return;
                    }
                }



                AMT_SETTING();

                #region 수검자 마스터 정보 갱신
                HIC_PATIENT nHP = Patient_Data_Build();

                if (hicPatientService.UpDate(nHP) <= 0)
                {
                    MessageBox.Show("환자마스타에 UPDATE 도중에 오류가 발생함", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }
                #endregion

                if (FstrGBSTS == "1")
                {

                    //HIC_JEPSU GWRTNO조회
                    HIC_JEPSU item = hicJepsuService.Read_Jepsu_GWRTNO(txtPtno.Text, dtpJepDate.Text);
                    if (!item.IsNullOrEmpty())
                    {
                        GnWRTNO = item.GWRTNO;
                    }

                    if (GnWRTNO == 0) { GnWRTNO = cHB.Read_New_JepsuGWrtNo(); }
                }

                #region HEA_JEPSU 접수마스터 DB UpDate
                HEA_JEPSU nHJ = Jepsu_Data_Build();

                if (!heaJepsuService.Save(nHJ, bNEW, fstrNew))
                {
                    MessageBox.Show("접수 Data Insert 시 오류가 발생함", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }
                #endregion

                string strGbNaksang = "N";

                if (chkFall.Checked)
                {
                    strGbNaksang = "Y";
                }
                else
                {
                    strGbNaksang = cHF.GET_Naksang_Flag(nHJ.AGE, nHJ.SDATE, nHJ.PTNO);
                }

                if (!Sunap_DTL_INSERT(FnWRTNO, suInfo, FstrBuRate))         //검진항목을 UPDATE
                {
                    MessageBox.Show("SUNAPDTL Insert 시 오류가 발생함", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (!Sunap_Result_UPDATE(FnWRTNO, grpExam))      //검진결과 항목을 UPDATE
                {
                    MessageBox.Show("HEA_RESULT Data 정리중 오류가 발생함", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //변경사항 변경(2021-08-05)
                EXAM_UPDATE();

                //Insert_Jepsu_History();    //접수내역 History

                //검진항목감액내역 저장
                if (clsHcType.HaGamFlag == true)
                {
                    if (!GamCode_Detail_UPDATE())
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                if (string.Compare(FstrGBSTS, "1") >= 0)
                {
                    if (!Hic_Spc_Acting_Insert())    //일특파트 항목 UPDATE
                    {
                        MessageBox.Show("일특(ACT) 신규등록 시 오류 발생", "오류");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (!Exam_Acting_Update())       //액팅파트 항목 UPDATE
                    {
                        MessageBox.Show("검사액팅코드 UPDATE 도중에 오류가 발생함", "오류");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                //2016-09-22 웹결과지인쇄 신청로그 업데이트
                if (rdoRES14.Checked)
                {
                    if (HaJEPSU.WEBPRINTREQ.IsNullOrEmpty())
                    {
                        if (!heaJepsuService.UpDateWebPrintReq(FnWRTNO, true))
                        {
                            MessageBox.Show("웹결과지 신청 등록 UPDATE 도중에 오류가 발생함", "오류");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (!comHpcLibBService.InsertWebPrintReqHea(FnWRTNO))
                        {
                            MessageBox.Show("웹결과지 신청 HISTORY INSERT 오류발생", "오류");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
                //공단검진 대상자는 가접수가 되어 있으면 일반검진 자동접수 함  2013-03-29
                if (FstrGBSTS != "D" && FstrGBSTS != "0" && chkGongDan.Checked && clsType.User.IdNumber.To<long>() != 28048)
                {
                    if (!Read_Hic_GaJepsu(txtPtno.Text, cboYear.Text))
                    {
                        MessageBox.Show("공단검진 Data 접수 오류!", "확인");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }

                if (FstrGBSTS != "D" && FstrGBSTS != "0" && !nHJ.PTNO.IsNullOrEmpty())
                {
                    //EXAM_ORDER에 TX25 위 미생물 , TX21 결직장조직 insert
                    COMHPC cHPC = new COMHPC
                    {
                        WRTNO = nHJ.WRTNO,
                        JEPDATE = nHJ.SDATE,
                        SDATE = nHJ.SDATE,
                        PANO = nHJ.PANO,
                        PTNO = nHJ.PTNO,
                        DEPTCODE = "TO",
                        BI = "61",
                        SEX = nHJ.SEX,
                        AGE = nHJ.AGE,
                        SNAME = nHJ.SNAME,
                        JOBSABUN = 222,
                        DRCODE = "7102",
                        JUMIN = txtJumin1.Text + txtJumin2.Text,
                        LTDCODE = nHJ.LTDCODE,
                    };

                    if (!cHOS.Hic_ExamBarCode_New(cHPC))
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    //방사선오더 자동 발생 - 최초수진등록일경우에만
                    if (!cHOS.EXAM_ORDER_SEND(cHPC, "TO", ""))
                    {
                        MessageBox.Show("Xray Order 전송중 오류가 발생함", "오류");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }

                    //내시경 간호기록지 및 외래접수 생성
                    if (!cHcMain.HIC_ENDOCHART_INSERT(cHPC.WRTNO, cHPC.SDATE, cHPC.PTNO, "TO", cHPC.PANO))
                    {
                        MessageBox.Show("내시경기록지 생성시 오류가 발생함.", "오류");
                        clsDB.setRollbackTran(clsDB.DbCon);
                        return;
                    }
                }


                #region endo_jupms 취소처리
                string strFRtime = "";
                string strTRtime = "";

                strJob = new List<string>();
                //내시경접수 00시 설정(취소) endo_jupmst 취소처리
                HEA_RESV_EXAM item3 = heaResvExamService.GetRTimebyPaNoGbExamSDate(txtPano.Text.To<long>(0), dtpSDate.Text, "01");
                if (!item3.IsNullOrEmpty())
                {
                    if (VB.Right(item3.RTIME, 5) == "00:00")
                    {
                        strFRtime = VB.Left(item3.RTIME, 10);
                        strTRtime = VB.Left(item3.RTIME, 10) + " 23:59";
                        strJob.Add("2");
                        result = endoJupmstService.UpDateGbsunapByPtnoRDate1(txtPtno.Text, "*", "TO", strJob, strFRtime, strTRtime);

                        if (result < 0)
                        {
                            MessageBox.Show("내시경접수취소 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                strFRtime = "";
                strTRtime = "";
                strJob = new List<string>();
                HEA_RESV_EXAM item4 = heaResvExamService.GetRTimebyPaNoGbExamSDate(txtPano.Text.To<long>(0), dtpSDate.Text, "02");
                if (!item4.IsNullOrEmpty())
                {
                    if (VB.Right(item4.RTIME, 5) == "00:00")
                    {
                        strFRtime = VB.Left(item4.RTIME, 10);
                        strTRtime = VB.Left(item4.RTIME, 10) + " 23:59";
                        strJob.Add("3");
                        result = endoJupmstService.UpDateGbsunapByPtnoRDate1(txtPtno.Text, "*", "TO", strJob, strFRtime, strTRtime);

                        if (result < 0)
                        {
                            MessageBox.Show("내시경접수취소 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
                #endregion

                #region 수납금액을 등록 및 영수증 인쇄  CmdOK_Sunap_Amt_Update
                string strSuUpDate = "NO";
                HIC_SUNAP bSunap = hicSunapService.GetHeaSunapAmtByWRTNO(FnWRTNO);

                if (FstrGBSTS == "1")
                {
                    if (sunap.MISUAMT > 0) //예약금액이 있다면
                    {
                        if (!Report_OldAmt_Cancel(nHJ))      //종전의 영수증을 취소
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (!Report_NewAmt_Print(nHJ, sunap, FstrGBSTS))     //신규등록
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                    else
                    {
                        if (bSunap.TOTAMT > 0) //예약금액이 있다면
                        {
                            if (!Report_OldAmt_Cancel(nHJ))      //종전의 영수증을 취소
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }

                        if (!Report_NewAmt_Print(nHJ, sunap, FstrGBSTS))     //신규등록
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
                else if (FstrGBSTS == "0" && cboMisuGye.Text.Trim() != "")
                {
                    if (sunap.TOTAMT != bSunap.TOTAMT) { strSuUpDate = "OK"; }
                    if (sunap.LTDAMT != bSunap.LTDAMT) { strSuUpDate = "OK"; }
                    if (sunap.BONINAMT != bSunap.BONINAMT) { strSuUpDate = "OK"; }
                    if (sunap.MISUGYE != bSunap.MISUGYE) { strSuUpDate = "OK"; }
                    if (sunap.MISUAMT != bSunap.MISUAMT) { strSuUpDate = "OK"; }
                    if (sunap.HALINAMT != bSunap.HALINAMT) { strSuUpDate = "OK"; }
                    if (sunap.SUNAPAMT1 != bSunap.SUNAPAMT1) { strSuUpDate = "OK"; }
                    if (sunap.SUNAPAMT2 != bSunap.SUNAPAMT2) { strSuUpDate = "OK"; }
                    if (sunap.HALINGYE.To<string>("") != bSunap.HALINGYE.To<string>("")) { strSuUpDate = "OK"; }

                    if (strSuUpDate == "OK")
                    {
                        if (!bNEW && bSunap.TOTAMT > 0)
                        {
                            if (!Report_OldAmt_Cancel(nHJ))      //종전의 영수증을 취소
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                        
                        if (!Report_NewAmt_Print(nHJ, sunap, FstrGBSTS))     //신규등록
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
                else if (FstrGBSTS == "D")
                {
                    if (bSunap.TOTAMT > 0)
                    {
                        if (!Report_OldAmt_Cancel(nHJ))      //종전의 영수증을 취소
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
                else if (FstrGBSTS != "0")
                {
                    if (sunap.TOTAMT != bSunap.TOTAMT) { strSuUpDate = "OK"; }
                    if (sunap.LTDAMT != bSunap.LTDAMT) { strSuUpDate = "OK"; }
                    if (sunap.BONINAMT != bSunap.BONINAMT) { strSuUpDate = "OK"; }
                    if (sunap.HALINAMT != bSunap.HALINAMT) { strSuUpDate = "OK"; }
                    if (sunap.HALINGYE.To<string>("") != bSunap.HALINGYE.To<string>("")) { strSuUpDate = "OK"; }

                    //예약선수금 관련
                    if (sunap.MISUAMT > 0)
                    {
                        //수진일에 예약선수금을 대체
                        if (nHJ.SDATE == DateTime.Now.ToShortDateString())
                        {
                            if (hicSunapService.GetHeaSumMisuAmtByWrtnoSuDate(FnWRTNO) > 0) { strSuUpDate = "OK"; }
                        }
                        else
                        {
                            if (sunap.MISUAMT != bSunap.MISUAMT) { strSuUpDate = "OK"; }
                        }
                    }
                    else if (sunap.MISUGYE == "01")
                    {
                        if (sunap.MISUAMT != bSunap.MISUAMT) { strSuUpDate = "OK"; }
                    }
                    else
                    {
                        if (sunap.MISUAMT != bSunap.MISUAMT) { strSuUpDate = "OK"; }
                        if (sunap.MISUGYE != bSunap.MISUGYE) { strSuUpDate = "OK"; }
                        if (sunap.SUNAPAMT1 != bSunap.SUNAPAMT1) { strSuUpDate = "OK"; }
                        if (sunap.SUNAPAMT2 != bSunap.SUNAPAMT2) { strSuUpDate = "OK"; }
                    }

                    if (strSuUpDate == "OK")
                    {
                        if (!Report_OldAmt_Cancel(nHJ))      //종전의 영수증을 취소
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (!Report_NewAmt_Print(nHJ, sunap, FstrGBSTS))     //신규등록
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
                #endregion

                //카드테이블에 접수번호 갱신
                if (nHJ.PANO != 999)
                {
                    if (clsPmpaType.RSD.CardSeqNo > 0 && (sunap.SUNAPAMT2 > 0 || sunap.SUNAPAMT1 > 0))
                    {
                        if (cardApprovCenterService.UpdateHWrtNobyPano(nHJ.WRTNO, nHJ.PTNO, clsPmpaType.RSD.CardSeqNo.To<long>(0)) <= 0)
                        {
                            MessageBox.Show("CARD_APPROV_CENTER에 UPDATE 도중에 오류가 발생함.", "오류");
                            //2020-09-10
                            clsDB.setRollbackTran(clsDB.DbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                string strEkgJob = "";

                //EKG항목전송 및 점검
                if (FstrGBSTS == "0")
                {
                    strEkgJob = "1";
                }
                else if (FstrGBSTS == "D")
                {
                    strEkgJob = "9";
                }
                else
                {
                    strEkgJob = "3";
                }

                if (FstrGBSTS != "9")
                {
                    if (FstrOldSdate.IsNullOrEmpty()) { FstrOldSdate = dtpSDate.Text; }
                    if (!cHMF.Jin_Support_Data_Send_TO(nHJ, strEkgJob, FstrOldSdate))
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                if (FstrGBSTS == "1" && dtpSDate.Text == DateTime.Now.ToShortDateString())
                {
                    //TextEmr 자동 EMR Table INSERT
                    clsQuery.NEW_TextEMR_TreatInterface(clsDB.DbCon, txtPtno.Text, dtpSDate.Text, "TO", "TO", "정상", "99917");
                }

                //2020-06-01(검사보류시 EMR생성)
                string strTmpChk = heaResvExamService.GetNotEqualResvExamByPanoSDate(txtPano.Text.To<long>(0), dtpSDate.Text);

                if (!strTmpChk.IsNullOrEmpty())
                {
                    clsQuery.NEW_TextEMR_TreatInterface(clsDB.DbCon, txtPtno.Text, dtpSDate.Text, "TO", "TO", "정상", "99917");
                }

                Hic_Memo_Save();

                //종합건진 초대권 마스타 업데이트
                if (txtTikcet.Text.Trim() != "")
                {
                    if (!Ticket_Update(txtTikcet.Text.To<long>(0)))
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                //회사별 가예약 업데이트
                if (clsHcVariable.GbGaResvLtd)
                {
                    cHcMain.Update_GaResvLtd(nHJ.LTDCODE, nHJ.SDATE);
                }

                //종합건진 동의서 업데이트
                if (!cHMF.Consent_DB_Update_Hea(nHJ))
                {
                    MessageBox.Show("내시경 동의서 DB 생성시 오류가 발생함.", "오류");
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //일반건진 접수내역이 있으면 주소를 업데이트함
                if (!Hic_Juso_Update(nHJ, nHP))
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return; 
                }

                //2015-05-12 대기자를 Call하여 접수를 하면 접수증 및 식권을 인쇄함
                if (FbCall)
                {
                    //접수증
                    JepsuPrintSetting(nHJ);
                    frmHaPrintPano fHPP = new frmHaPrintPano(nHJ, nHP, FstrChkExam, FstrChkExamName, FstrContrast, strMsg);
                    fHPP.ShowDialog();

                    //식권
                    Print_Food_Ticket(nHJ, "");

                }

                clsDB.setCommitTran(clsDB.DbCon);
                //clsDB.setRollbackTran(clsDB.DbCon);

                string strSDate = HaJEPSU.SDATE.To<string>("");
                string strTempCD = "";

                //예약변경 알림톡 전송
                if (FstrGBSTS == "0" && strSDate != "" && strSDate != dtpSDate.Text && VB.Right(dtpSDate.Text, 5) != "12-25")  //예약변경
                {
                    cATK.Clear_ATK_Varient();
                    strTempCD = "C_MJ_001_02_12448";

                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = dtpSDate.Text + " " + cboSTime.Text;
                    clsHcType.ATK.SendUID = nHP.HPHONE + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                    clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                    clsHcType.ATK.Pano = txtPtno.Text;
                    clsHcType.ATK.sName = txtSName.Text;
                    clsHcType.ATK.HPhone = txtHphone.Text;
                    clsHcType.ATK.RetTel = "054-260-8188";
                    clsHcType.ATK.SendType = "A";
                    clsHcType.ATK.TempCD = strTempCD;
                    clsHcType.ATK.Dept = "TO";
                    clsHcType.ATK.DrName = "";
                    clsHcType.ATK.LtdName = "";
                    clsHcType.ATK.JobSabun = clsType.User.IdNumber.To<long>();

                    clsHcType.ATK.ATMsg = cATK.READ_TEMPLATE_MESSAGE(strTempCD);
                    clsHcType.ATK.SmsMsg = cATK.READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", txtSName.Text);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{YYYY}", VB.Left(dtpSDate.Text, 4));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{MM}", VB.Mid(dtpSDate.Text, 6, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{DD}", VB.Mid(dtpSDate.Text, 9, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{HH}", cboSTime.Text);

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", txtSName.Text);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{YYYY}", VB.Left(dtpSDate.Text, 4));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(dtpSDate.Text, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(dtpSDate.Text, 9, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{HH}", cboSTime.Text);

                    if (cATK.INSERT_ALIMTALK_MESSAGE() == false)
                    {
                        MessageBox.Show("알림톡 요청시 오류가 발생함.", "오류");
                        return;
                    }
                }

                if (FstrGBSTS == "D")
                {
                    cATK.Clear_ATK_Varient();
                    strTempCD = "C_MJ_001_02_12472";

                    //------------( 자료를 DB에 INSERT )---------------------
                    clsHcType.ATK.RDate = dtpSDate.Text + " " + cboSTime.Text;
                    clsHcType.ATK.SendUID = nHP.HPHONE + clsPublic.GstrSysDate + DateTime.Now.ToString("HH:mm:ss");
                    clsHcType.ATK.SendUID = clsHcType.ATK.SendUID.Replace(":", "").Replace("-", "").Replace(" ", "").Replace(".", "");
                    clsHcType.ATK.Pano = txtPtno.Text;
                    clsHcType.ATK.sName = txtSName.Text;
                    clsHcType.ATK.HPhone = txtHphone.Text;
                    clsHcType.ATK.RetTel = "054-260-8188";
                    clsHcType.ATK.SendType = "A";
                    clsHcType.ATK.TempCD = strTempCD;
                    clsHcType.ATK.Dept = "TO";
                    clsHcType.ATK.DrName = "";
                    clsHcType.ATK.LtdName = "";
                    clsHcType.ATK.JobSabun = clsType.User.IdNumber.To<long>();

                    clsHcType.ATK.ATMsg = cATK.READ_TEMPLATE_MESSAGE(strTempCD);
                    clsHcType.ATK.SmsMsg = cATK.READ_TEMPLATE_SMS_MESSAGE(strTempCD);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{수검자명}", txtSName.Text);
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{YYYY}", VB.Left(dtpSDate.Text, 4));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{MM}", VB.Mid(dtpSDate.Text, 6, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{DD}", VB.Mid(dtpSDate.Text, 9, 2));
                    clsHcType.ATK.ATMsg = clsHcType.ATK.ATMsg.Replace("#{HH}", cboSTime.Text);

                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{수검자명}", txtSName.Text);
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{YYYY}", VB.Left(dtpSDate.Text, 4));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{MM}", VB.Mid(dtpSDate.Text, 6, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{DD}", VB.Mid(dtpSDate.Text, 9, 2));
                    clsHcType.ATK.SmsMsg = clsHcType.ATK.SmsMsg.Replace("#{HH}", cboSTime.Text);

                    if (cATK.INSERT_ALIMTALK_MESSAGE() == false)
                    {
                        MessageBox.Show("알림톡 요청시 오류가 발생함.", "오류");
                        return;
                    }
                }

                clsDB.setBeginTran(clsDB.DbCon);

                //2015-07-21 검체접수 업데이트
                if (FstrGBSTS != "0" && FstrGBSTS != "D")
                {
                    if (!CmdOK_Spec_Receipt_Update(chkStool, chkSputum, chkUrine))
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
                
                //2015-11-19 낙상예방 외래 등록
                if (strGbNaksang == "Y") { cHMF.Naksang_Opd_INSERT(nHJ.PTNO, "TO", clsType.User.IdNumber.To<long>()); }

                //2016-02-19 종검예약 엑셀파일에 예약날짜,수검날짜를 업데이트
                if (!HEA_Excel_Update(nHJ, nHP))
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                //clsDB.setRollbackTran(clsDB.DbCon);

                Cursor.Current = Cursors.Default;

                //동시진행동의서 변경
                //if (dtpSDate.Text == DateTime.Now.ToShortDateString() && FstrGBSTS != "0" && FstrGBSTS != "D" && chkGongDan.Checked)
                //{
                //    if (nHJ.WRTNO > 0)
                //    {
                //        if (comHpcLibBService.GetHicPrivacyAccept(nHJ.PTNO, "D53", "TO").IsNullOrEmpty())
                //        {
                //            HIC_CONSENT nHC = new HIC_CONSENT
                //            {
                //                SDATE = nHJ.SDATE,
                //                BDATE = nHJ.JEPDATE,
                //                PTNO = nHJ.PTNO,
                //                WRTNO = nHJ.WRTNO,
                //                PANO = nHJ.PANO,
                //                SNAME = nHJ.SNAME,
                //                DEPTCODE = "TO",
                //                FORMCODE = "D53",
                //                PAGECNT = 1,
                //                ENTSABUN = clsType.User.IdNumber.To<long>()
                //            };

                //            if (!hicConsentService.Insert(nHC))
                //            {
                //                MessageBox.Show("전자동의서 등록시 오류가 발생함.", "오류");
                //                Cursor.Current = Cursors.Default;
                //                return;
                //            }

                //            //frmHcEmrPermission.CellDblClicked(3);
                //        }
                //    }
                //}

                //건강검진 동시진행 동의서
                if (FstrGBSTS != "0" && chkGongDan.Checked)
                {
                    HIC_PRIVACY_ACCEPT_NEW item2 = hicPrivacyAcceptNewService.GetIetmByPtnoYear(txtPtno.Text, cboYear.Text);
                    if (item2.IsNullOrEmpty())
                    {
                        frmHcPermission.CellDblClicked(3);
                        //frmHcEmrPermission.CellDblClicked(3);
                    }
                }

                //건강검진 동시진행 동의서 프로그램을 기다린다.
                Process[] ProcessEx = Process.GetProcessesByName("PenToolController");

                if (ProcessEx.Length > 0)
                {
                    Process[] Pro1 = Process.GetProcessesByName("PenToolController");
                    Process CurPro = Process.GetCurrentProcess();
                    foreach (Process Proc in Pro1)
                    {
                        if (Proc.Id != CurPro.Id)
                        {
                            //동의서 프로그램이 종료될때까지 기다림.
                            Proc.WaitForExit();
                        }
                    }
                }


                ComFunc.MsgBox("저장 완료!", "작업완료");

                //2020년 전자동의서관련 추가
                if (dtpSDate.Text == DateTime.Now.ToShortDateString() && FstrGBSTS != "0" && FstrGBSTS != "D")
                {
                    if (nHJ.WRTNO > 0)
                    {
                        if (comHpcLibBService.GetHicPrivacyAccept(nHJ.PTNO, "D50", "TO").IsNullOrEmpty())
                        {
                            HIC_CONSENT nHC = new HIC_CONSENT
                            {
                                SDATE = nHJ.SDATE,
                                BDATE = nHJ.JEPDATE,
                                PTNO = nHJ.PTNO,
                                WRTNO = nHJ.WRTNO,
                                PANO = nHJ.PANO,
                                SNAME = nHJ.SNAME,
                                DEPTCODE = "TO",
                                FORMCODE = "D50",
                                PAGECNT = 1,
                                ENTSABUN = clsType.User.IdNumber.To<long>()
                            };

                            if (!hicConsentService.Insert(nHC))
                            {
                                MessageBox.Show("전자동의서 등록시 오류가 발생함.", "오류");
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            frmHcPermission.CellDblClicked(0);
                            //frmHcEmrPermission.CellDblClicked(0);
                        }
                    }
                }

                Screen_Clear();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }

        }

        private bool Jepsu_Del_Check_Logic()
        {
            try
            {
                if (clsHcVariable.B01_JONGGUM_SABUN == false)
                {
                    MessageBox.Show("종검 직원만 접수취소가 가능합니다.", "작업불가");
                    return false;
                }

                if (clsHcVariable.GbHeaAdminSabun == false)
                {
                    if (!heaResultService.IsActiveByWrtno(FnWRTNO).IsNullOrEmpty())
                    {
                        MessageBox.Show("이미 검사가 시행되어 삭제할 수 없습니다. 관리자에게 문의하십시오.", "작업불가");
                        return false;
                    }
                }

                //방사선검사가 있고 이미 촬영하였는지..
                if (!comHpcLibBService.ChkXrayExcuteByPtnoBDate(txtPtno.Text, dtpSDate.Text).IsNullOrEmpty())
                {
                    MessageBox.Show("이미 방사선검사가 시행되어 삭제할 수 없습니다. 관리자에게 문의하십시오.", "작업불가");
                    return false;
                }

                //내시경검사가 있고 이미 촬영하였는지
                if (!comHpcLibBService.ChkEndoExcuteByPtnoBDate(txtPtno.Text, dtpSDate.Text).IsNullOrEmpty())
                {
                    MessageBox.Show("이미 내시경검사가 시행되어 삭제할 수 없습니다. 관리자에게 문의하십시오.", "작업불가");
                    return false;
                }

                //2015-02-14 예약선수금이 있으면 접수취소 불가
                if (heaSunapService.GetMisuAmtByWrtno(FnWRTNO) != 0)
                {
                    MessageBox.Show("예약선수금이 있어 접수취소가 불가능합니다.", "작업불가");
                    return false;
                }


                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 2016-02-19 종검예약 엑셀파일에 예약날짜,수검날짜를 업데이트
        /// </summary>
        /// <param name="nHJ"></param>
        /// <param name="nHP"></param>
        /// <returns></returns>
        private bool HEA_Excel_Update(HEA_JEPSU nHJ, HIC_PATIENT nHP)
        {
            string strRowid = string.Empty;

            try
            {
                //주민등록번호로 엑셀파일을 찾음
                HEA_EXCEL hEA_EXCEL = heaExcelService.GetItemByJuminYear(nHP.JUMIN2, VB.Left(nHJ.SDATE, 4));

                //교육청,군청 등 주민등록번호 미제공 거래처
                if (hEA_EXCEL.IsNullOrEmpty())
                {
                    hEA_EXCEL = new HEA_EXCEL();
                    hEA_EXCEL.BIRTH = nHP.CT_JUMIN1;
                    hEA_EXCEL.SNAME = nHP.SNAME;
                    hEA_EXCEL.LTDCODE = nHJ.LTDCODE;
                    hEA_EXCEL.YEAR = cboYear.Text;

                    strRowid = heaExcelService.GetRowidbyItem(hEA_EXCEL);
                }
                else
                {
                    strRowid = hEA_EXCEL.RID;
                }

                //엑셀파일에서 수검자를 찾았으면 업데이트 함
                if (!strRowid.IsNullOrEmpty())
                {
                    hEA_EXCEL = new HEA_EXCEL();
                    hEA_EXCEL.PANO = nHJ.PANO;
                    hEA_EXCEL.PTNO = nHJ.PTNO;
                    hEA_EXCEL.RID = strRowid;
                    hEA_EXCEL.AES_JUMIN = clsAES.AES(nHP.CT_JUMIN1 + nHP.CT_JUMIN2);

                    if (FstrGBSTS == "D")
                    {
                        hEA_EXCEL.RDATE = "";
                        hEA_EXCEL.SDATE = "";
                    }
                    else
                    {
                        hEA_EXCEL.RDATE = nHJ.SDATE;
                        if (FstrGBSTS == "0")
                        {
                            hEA_EXCEL.SDATE = "";
                        }
                        else
                        {
                            hEA_EXCEL.SDATE = nHJ.SDATE;
                        }
                    }

                    if (!heaExcelService.UpdateExcel(hEA_EXCEL))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool CmdOK_Spec_Receipt_Update(CheckBox chkStool, CheckBox chkSputum, CheckBox chkUrine)
        {
            try
            {
                HEA_RESULT iHR = null;

                if (chkStool.Enabled)
                {
                    iHR = heaResultService.GetOneItemByExCodeWrtno("A992", FnWRTNO);
                    if (!iHR.IsNullOrEmpty())
                    {
                        if (chkStool.Checked && iHR.RESULT.To<string>("").Trim() == "")
                        {
                            if (!heaResultService.UpdateResultActivebyWrtNoExCode("01", "Y", FnWRTNO, "A992"))
                            {
                                MessageBox.Show("종합검진 결과 UPDATE 중 에러발생", "오류");
                                return false;
                            }
                        }
                        else if (!chkStool.Checked && iHR.RESULT.To<string>("").Trim() != "")
                        {
                            if (!heaResultService.UpdateResultActivebyWrtNoExCode("", "", FnWRTNO, "A992"))
                            {
                                MessageBox.Show("종합검진 결과 UPDATE 중 에러발생", "오류");
                                return false;
                            }
                        }
                    }
                }

                if (chkSputum.Enabled)
                {
                    iHR = heaResultService.GetOneItemByExCodeWrtno("A902", FnWRTNO);
                    if (!iHR.IsNullOrEmpty())
                    {
                        if (chkSputum.Checked && iHR.RESULT.To<string>("").Trim() == "")
                        {
                            if (!heaResultService.UpdateResultActivebyWrtNoExCode("01", "Y", FnWRTNO, "A902"))
                            {
                                MessageBox.Show("종합검진 결과 UPDATE 중 에러발생", "오류");
                                return false;
                            }
                        }
                        else if (!chkSputum.Checked && iHR.RESULT.To<string>("").Trim() != "")
                        {
                            if (!heaResultService.UpdateResultActivebyWrtNoExCode("", "", FnWRTNO, "A902"))
                            {
                                MessageBox.Show("종합검진 결과 UPDATE 중 에러발생", "오류");
                                return false;
                            }
                        }
                    }
                }

                if (chkUrine.Enabled)
                {
                    iHR = heaResultService.GetOneItemByExCodeWrtno("A903", FnWRTNO);
                    if (!iHR.IsNullOrEmpty())
                    {
                        if (chkUrine.Checked && iHR.RESULT.To<string>("").Trim() == "")
                        {
                            if (!heaResultService.UpdateResultActivebyWrtNoExCode("01", "Y", FnWRTNO, "A903"))
                            {
                                MessageBox.Show("종합검진 결과 UPDATE 중 에러발생", "오류");
                                return false;
                            }
                        }
                        else if (!chkUrine.Checked && iHR.RESULT.To<string>("").Trim() != "")
                        {
                            if (!heaResultService.UpdateResultActivebyWrtNoExCode("", "", FnWRTNO, "A903"))
                            {
                                MessageBox.Show("종합검진 결과 UPDATE 중 에러발생", "오류");
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool Hic_Juso_Update(HEA_JEPSU nHJ, HIC_PATIENT nHP)
        {
            try
            {
                List<long> lstHcWrtno  = hicJepsuService.GetListWrtnoByPtnoJepDate(nHJ.PTNO, nHJ.JEPDATE);

                if (lstHcWrtno.Count > 0)
                {
                    if (lstHcWrtno[0] > 0)
                    {
                        if (hicPatientService.UpDate(nHP) <= 0)
                        {
                            MessageBox.Show("환자마스타에 UPDATE 도중에 오류가 발생함", "오류");
                            return false;
                        }

                        //일반건진 주소,성명,성별을 업데이트
                        if (!hicJepsuService.UpDateJusoByWrtnoIN(nHP, lstHcWrtno))
                        {
                            MessageBox.Show("일반검진 접수에 주소 UPDATE 중 에러발생", "오류");
                            return false;
                        }
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool Ticket_Update(long argTicketNo)
        {
            try
            {
                HEA_TICKET item = heaTicketService.GetItemByTicketNo(argTicketNo);

                if (item.IsNullOrEmpty())
                {
                    return true;
                }

                //수검자 이름을 설정
                string strSname = txtSName.Text + "(" + cboSex.Text + "/" + VB.Format(txtAge.Text.To<long>(), "00") + ")";

                //배부자의 휴대폰번호를 찾기
                string strHtel = "";
                for (int i = 0; i < lstTicketSMS.Count; i++)
                {
                    if (item.BSAWON.Trim() == VB.Pstr(lstTicketSMS[i], ";", 1).Trim())
                    {
                        strHtel = VB.Replace(VB.Pstr(lstTicketSMS[i], ";", 3), "-", "").Trim();
                        break;
                    }
                }

                //검진 SMS 전송
                if (!strHtel.IsNullOrEmpty() && item.GBSMS2.To<string>() == "Y" && item.SMS_SEND2.To<string>() == "")
                {
                    if (dtpSDate.Text == DateTime.Now.ToShortDateString())
                    {
                        clsPublic.GstrMsgList = "■종합건진■초대권(" + argTicketNo + ") " + strSname + "님이금일건진을하셨습니다";

                        ETC_SMS eSMS = new ETC_SMS
                        {
                            JOBDATE = DateTime.Now,
                            HPHONE = strHtel,
                            GUBUN = "10",
                            RETTEL = "054-260-8290",
                            SENDMSG = clsPublic.GstrMsgList,
                            ENTSABUN = clsType.User.IdNumber.To<long>()
                        };

                        if (etcSmsService.Insert(eSMS) < 0)
                        {
                            MessageBox.Show("SMS 전송 의뢰 실패", "오류");
                            return false;
                        }
                    }
                }

                //예약 SMS 전송
                if (!strHtel.IsNullOrEmpty() && item.GBSMS1.To<string>() == "Y" && item.SMS_SEND1.To<string>() == "" && item.SMS_SEND2.To<string>() == "")
                {
                    if (dtpJepDate.Text == DateTime.Now.ToShortDateString())
                    {
                        clsPublic.GstrMsgList = "■종합건진■초대권(" + argTicketNo + ") " + strSname + "님이금일예약접수하셨습니다";

                        ETC_SMS eSMS = new ETC_SMS
                        {
                            JOBDATE = DateTime.Now,
                            HPHONE = strHtel,
                            GUBUN = "10",
                            RETTEL = "054-260-8290",
                            SENDMSG = clsPublic.GstrMsgList,
                            ENTSABUN = clsType.User.IdNumber.To<long>()
                        };

                        if (etcSmsService.Insert(eSMS) < 0)
                        {
                            MessageBox.Show("SMS 전송 의뢰 실패", "오류");
                            return false;
                        }
                    }
                }

                //종검 초대권관리 DB에 업데이트
                HEA_TICKET iHaTCK = new HEA_TICKET
                {
                    JEPDATE = dtpJepDate.Text,
                    SDATE = dtpSDate.Text,
                    JEPSUNO = FnWRTNO,
                    SNAME = item.SNAME,
                    SMS_SEND1 = item.SMS_SEND1,
                    SMS_SEND2 = item.SMS_SEND2,
                    RID = item.RID
                };

                if (!heaTicketService.UpDate(iHaTCK))
                {
                    MessageBox.Show("종합건진 초대권 마스타 자료수정 오류", "확인");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool Report_NewAmt_Print(HEA_JEPSU nHJ, HIC_SUNAP hSP, string strGbSTS)
        {
            try
            {
                hSP.SEQNO = hicSunapService.GetHeaMaxSeqbyWrtNo(nHJ.WRTNO);
                hSP.PANO = nHJ.PANO;
                hSP.WRTNO = nHJ.WRTNO;
                //hSP.SUNAPAMT = txtCashAmt.Text.Replace(",", "").To<long>();
                //hSP.SUNAPAMT2 = txtCardAmt.Text.Replace(",", "").To<long>();
                //if (hSP.SUNAPAMT2 > 0) { hSP.SUNAPAMT = hSP.SUNAPAMT2; }
                //if (hSP.HALINAMT > 0)
                //{
                //    hSP.HALINGYE = VB.Pstr(cboHalinGye.Text, ".", 1);
                //}
                if (hSP.MISUAMT > 0) { hSP.MISUGYE = "01"; }

                //hSP.HALINGYE = hicSunapService.GetHalinGyeByWrtnoSeqNo(nHJ.WRTNO, hSP.SEQNO - 1);
                //hSP.MISUGYE = hicSunapService.GetMisuGyeByWrtnoSeqNo(nHJ.WRTNO, hSP.SEQNO - 1);

                hicSunapService.InsertHea(hSP);

                if (strGbSTS == "1" && hSP.BONINAMT > 0)
                {
                    if (ComFunc.MsgBoxQ(nHJ.GJJONG + " 종 영수증을 발생하시겠습니까?", "", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        nHJ.JEPBUN = "5";
                        cHPrt.Receipt_Report_Print(nHJ, hSP);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool Report_OldAmt_Cancel(HEA_JEPSU nHJ)
        {
            try
            {
                HIC_SUNAP item = hicSunapService.GetHeaSunapAmtByWRTNO(nHJ.WRTNO);

                //수납영수증 취소내역을 INSERT
                item.SEQNO = hicSunapService.GetHeaMaxSeqbyWrtNo(nHJ.WRTNO);
                item.WRTNO = nHJ.WRTNO;
                item.PANO = nHJ.PANO;
                item.HALINGYE = hicSunapService.GetHeaHalinGyeByWrtnoSeqNo(nHJ.WRTNO, item.SEQNO - 1);
                item.MISUGYE = hicSunapService.GetHeaMisuGyeByWrtnoSeqNo(nHJ.WRTNO, item.SEQNO - 1);

                hicSunapService.InsertMinusSunapData_Hea(item);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool Read_Hic_GaJepsu(string argPtno, string argYear)
        {
            try
            {
                List<HIC_JEPSU_WORK> lstHJW = hicJepsuWorkService.GetItemByPtnoYear(argPtno, argYear);
                if (lstHJW.Count > 0)
                {
                    for (int i = 0; i < lstHJW.Count; i++)
                    {
                        if (!cHMF.INSERT_HIC_JEPSU(lstHJW[i]))    //일반검진 접수
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool Exam_Acting_Update()
        {
            try
            {
                List<HIC_RESULT_EXCODE> haResult = hicResultExCodeService.GetItemHicHeabyWrtNoOrderbyPanjengPartExCode("HEA", FnWRTNO);

                if (haResult.Count > 0)
                {
                    HEA_RESULT hRES = null;

                    for (int i = 0; i < haResult.Count; i++)
                    {
                        hRES = new HEA_RESULT
                        {
                            ACTPART = haResult[i].HEAPART,
                            PART = haResult[i].PART,
                            RESCODE = haResult[i].RESCODE.To<string>(""),
                            EXCODE = haResult[i].EXCODE,
                            WRTNO = FnWRTNO,
                            RESULT = ""
                        };

                        if (!heaResultService.UpDatePartResCodeByExCodeWrtno(hRES))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 2015-07-20 종합검진+일특 Acting코드 생성
        /// </summary>
        /// <returns></returns>
        private bool Hic_Spc_Acting_Insert()
        {
            try
            {
                string strSDate = dtpSDate.Text;
                string strPtno = txtPtno.Text;
                string[] strSpcExam = new string[9];
                string strExam = string.Empty;
                string strTmpChk = string.Empty;
                string strActPart = string.Empty;

                List<HIC_RESULT> lstHRES = hicResultService.GetSpcResByPtnoSDate(strPtno, strSDate);

                if (lstHRES.Count > 0)
                {
                    //검사여부
                    for (int i = 0; i < lstHRES.Count; i++)
                    {
                        strExam = lstHRES[i].EXCODE.To<string>("").Trim();

                        if (VB.Left(strExam, 2) == "MU") { strSpcExam[1] = "Y"; }   //소변(특수)
                        else if (strExam == "LM10") { strSpcExam[2] = "Y"; }        //객담
                        else if (strExam == "TR11") { strSpcExam[3] = "Y"; }        //폐활량3회
                        else if (strExam == "A992") { strSpcExam[4] = "Y"; }        //분변검사
                        else if (strExam == "A993") { strSpcExam[5] = "Y"; }        //유방촬영
                        else if (strExam == "A803") { strSpcExam[6] = "Y"; }        //PAP
                    }
                }

                //액팅코드가 없으면 등록함
                for (int i = 1; i < 7; i++)
                {
                    if (strSpcExam[i] == "Y")
                    {
                        strTmpChk = "";
                        strExam = "";

                        switch (i)
                        {
                            case 1: strExam = "A903"; strActPart = "1"; break;
                            case 2: strExam = "A902"; strActPart = "G"; break;
                            case 3: strExam = "A920"; strActPart = "7"; break;
                            case 4: strExam = "A992"; strActPart = "J"; break;
                            case 5: strExam = "A993"; strActPart = "K"; break;
                            case 6: strExam = "A803"; strActPart = "O"; break;
                            default: break;
                        }

                        strTmpChk = heaResultService.GetRowidByOneExcodeWrtno(strExam, FnWRTNO);

                        if (strTmpChk.IsNullOrEmpty())
                        {
                            HEA_RESULT dHR = new HEA_RESULT
                            {
                                WRTNO = FnWRTNO,
                                EXCODE = strExam,
                                PART = "5",
                                RESCODE = "X05",
                                PANJENG = "",
                                RESULT = "",
                                ACTPART = strActPart
                            };

                            if (!heaResultService.InSert(dHR))
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool GamCode_Detail_UPDATE()
        {
            try
            {
                string strSDate = dtpSDate.Text;
                string strTmpChk = comHpcLibBService.GetHeaGamcodeHicByPano(FnPano, strSDate);

                if (!strTmpChk.IsNullOrEmpty())
                {
                    //기존의 자료가 있으면 삭제함
                    if (!comHpcLibBService.DelHeaGamCodeHicByPanoSDate(FnPano, strSDate))
                    {
                        MessageBox.Show("검진항목할인 상세내역 삭제시 오류 발생", "오류");
                        return false;
                    }
                }

                for (int i = 0; i < clsHcType.HaGam.Length; i++)
                {
                    if (clsHcType.HaGam[i].Pano > 0)
                    {
                        if (!comHpcLibBService.InsertHeaGamCodeHic(clsHcType.HaGam[i]))
                        {
                            MessageBox.Show("검진항목할인 상세내역 등록시 오류 발생", "오류");
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 검사항목(HEA_SUNAPDTL)을 UPDATE
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="list"></param>
        /// <param name="argBuRate"></param>
        /// <returns></returns>
        private bool Sunap_DTL_INSERT(long argWrtno, List<READ_SUNAP_ITEM> list, string argBuRate)
        {
            try
            {
                //기존의 자료가 있으면 삭제함
                if (heaSunapdtlService.GetCountbyWrtNo(argWrtno) > 0)
                {
                    heaSunapdtlService.DeleteAllByWrtno(argWrtno);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].RowStatus != RowStatus.Delete)
                    {
                        if (list[i].GBSELF.To<string>("").Trim() == "")
                        {
                            list[i].GBSELF = argBuRate;
                        }

                        if (list[i].GBHALIN == "Y")
                        {
                            list[i].GBHALIN = "1";
                        }

                        heaSunapdtlService.InsertData(argWrtno, list[i]);
                    }

                    heaSunapdtlService.InsertDataHis(argWrtno, list[i]);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 검사항목(HEA_SUNAPDTL)을 UPDATE
        /// </summary>
        /// <param name="argWrtno"></param>
        /// <param name="lstExam"></param>
        /// <returns></returns>
        private bool Sunap_Result_UPDATE(long argWrtno, List<GROUPCODE_EXAM_DISPLAY> lstExam)
        {
            string strOK = string.Empty;
            string strTmpChk = string.Empty;

            try
            {
                //DB에는 자료가 있고 검사 상세내역에 없는 검사는 삭제함
                List<HEA_RESULT> list = heaResultService.GetAllByWrtNo(argWrtno);

                for (int i = 0; i < list.Count; i++)
                {
                    strOK = "OK";

                    for (int j = 0; j < lstExam.Count; j++)
                    {
                        if (!lstExam[j].EXCODE.IsNullOrEmpty())
                        {
                            if (list[i].EXCODE.Trim() == lstExam[j].EXCODE.Trim())
                            {
                                strOK = "NO"; break;
                            }
                        }
                    }

                    if (strOK == "OK")
                    {
                        if (!list[i].RESULT.IsNullOrEmpty() && list[i].RESULT.To<string>("") != ".")
                        {
                            heaResultService.InsertDelSelectbyRowid(list[i].RID);
                        }

                        heaResultService.DeleteByRowid(list[i].RID);
                        lstDelExam.Add(list[i].EXCODE);

                    }
                }

                //추가 접수된 검사항목을 INSERT
                for (int i = 0; i < lstExam.Count; i++)
                {
                    //검사항목 제외 체크 삭제함
                    if (lstExam[i].RowStatus == RowStatus.Delete)
                    {
                        HEA_RESULT dHR = heaResultService.GetOneItemByExCodeWrtno(lstExam[i].EXCODE, argWrtno);

                        if (!dHR.IsNullOrEmpty())
                        {
                            if (!dHR.RESULT.IsNullOrEmpty() && dHR.RESULT.To<string>("").Trim() != ".")
                            {
                                heaResultService.InsertDelSelectbyRowid(list[i].RID);
                            }

                            heaResultService.DeleteByRowid(dHR.RID);
                        }
                    }
                    else
                    {
                        if (!lstExam[i].EXCODE.IsNullOrEmpty())
                        {
                            strOK = "OK";

                            for (int j = 0; j < list.Count; j++)
                            {
                                if (list[j].EXCODE.Trim() == lstExam[i].EXCODE.Trim())
                                {
                                    strOK = "NO"; break;
                                }
                            }

                            //자료가 없으면 신규 등록
                            if (strOK == "OK")
                            {
                                HIC_EXCODE item = hicExcodeService.FindOne(lstExam[i].EXCODE);

                                if (!item.IsNullOrEmpty() && item.PART != "9") //금액코드는 등록 안함
                                {
                                    HEA_RESULT hRES = new HEA_RESULT
                                    {
                                        WRTNO = argWrtno,
                                        PART = item.PART,
                                        ACTPART = item.HEAPART,
                                        EXCODE = lstExam[i].EXCODE,
                                        RESCODE = item.RESCODE
                                    };

                                    if (!heaResultService.InSert(hRES))              //자료를 INSERT
                                    {
                                        MessageBox.Show("검사항목 INSERT 시 오류 발생", "오류");
                                        return false;
                                    }

                                    //heaJepsuService.UpDate_GbSTS("1", argWrtno);    //접수상태를 돌림
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private HIC_PATIENT Patient_Data_Build()
        {
            HIC_PATIENT rHP = new HIC_PATIENT
            {
                SNAME = txtSName.Text.Trim(),                       JUMIN = txtJumin1.Text + VB.Left(txtJumin2.Text, 1) + "******",
                SEX = cboSex.Text,                                  BIRTHDAY = ComFunc.GetBirthDate(txtJumin1.Text, txtJumin2.Text, "-"),
                MAILCODE = txtMail.Text,                            JUSO1 = txtJuso1.Text,
                JUSO2 = txtJuso2.Text,                              BUILDNO = FstrBuildNo,
                TEL = txtTel.Text,                                  HPHONE = txtHphone.Text,
                GBSMS = "Y",                                        LTDCODE = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0),
                SABUN = txtLtdSabun.Text,                           GAMCODE = VB.Pstr(cboHalinGye.Text, ".", 1),
                STARTDATE = txtFirstDate.Text,                      LASTDATE = dtpSDate.Text,
                JINCOUNT = txtJongCNT.Text.To<long>(0) + 1,         FAMILLY = VB.Pstr(txtFamilly.Text, ".", 2),
                SOSOK = txtSosok.Text,                              VIPREMARK = txtVipRemark.Text,
                GBFOREIGNER = chkForeign.Checked ? "Y" : "",        ENAME = txtEName.Text,
                FOREIGNERNUM = txtForeignerNum.Text,                PTNO = txtPtno.Text,
                PANO = txtPano.Text.To<long>(0),                    JUMIN2 = clsAES.AES(txtJumin1.Text + txtJumin2.Text),
                CT_JUMIN1 = txtJumin1.Text,                         CT_JUMIN2 = txtJumin2.Text
            };

            return rHP;
        }

        private HEA_JEPSU Jepsu_Data_Build()
        {
            string strDust = string.Empty;
            string strGbNaksang = "N";
            string strGbExam = "N";
            string strTmpChk = "";
            string strGbChk3 = "";
            string strGbJUSO = "";
            fstrNew = "";

            if (FnWRTNO == 0)
            {
                FnWRTNO = cHB.READ_NewHEA_JepsuNo();
                fstrNew = "OK";
            }
            else
            {
                strGbExam = heaJepsuService.GetGbExamByWrtno(FnWRTNO);
            }

            READ_SUNAP_ITEM rRSI1 = suInfo.Find(x => x.ADDGBN == "Y");
            if (!rRSI1.IsNullOrEmpty() && rRSI1.ADDGBN == "Y")
            {
                strTmpChk = heaGroupexamExcodeService.GetRowidByGroupCode(rRSI1.GRPCODE);

                if (!strTmpChk.IsNullOrEmpty()) { FstrAddExamYN = "OK"; }
                //이미 발행된 상태에서 추가검사가 있을경우
                if (strGbExam == "Y" && FstrAddExamYN == "OK") { strGbExam = "N"; }
            }

            //CLO TEST 일경우 발행으로
            //수검일자가 당일이전인 경우만 이미 발행으로 표시...
            READ_SUNAP_ITEM rRSI2 = suInfo.Find(x => x.GRPCODE == "Z5261");            
            if (DateTime.Now.ToShortDateString() != dtpSDate.Text && !rRSI2.IsNullOrEmpty() && rRSI2.GRPCODE == "Z5261")
            {
                strGbExam = "Y";
            }

            GROUPCODE_EXAM_DISPLAY rGED1 = grpExam.Find(x => x.EXCODE == "TZ47");       //Dust 촬영체크
            if (!rGED1.IsNullOrEmpty() && rGED1.EXCODE == "TZ47") { strDust = "Y"; }    //Dust 촬영체크

            if (chkFall.Checked)
            {
                strGbNaksang = "Y";
            }
            else
            {
                strGbNaksang = cHF.GET_Naksang_Flag(txtAge.Text.To<long>(), dtpSDate.Text, txtPtno.Text);
            }

            HEA_JEPSU rHJ = new HEA_JEPSU
            {
                WRTNO = FnWRTNO,                                            JEPDATE = dtpJepDate.Text,
                SDATE = dtpSDate.Text,                                      PANO = txtPano.Text.To<long>(),
                SNAME = txtSName.Text,                                      SEX = cboSex.Text,
                AGE = txtAge.Text.To<long>(),                               GJJONG = VB.Pstr(cboJONG.Text, ".", 1),
                LTDCODE = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0),     GBEXAM = strGbExam,
                GAMCODE = VB.Pstr(cboHalinGye.Text, ".", 1),                SABUN = VB.Pstr(txtJikSabun.Text, ".", 1).To<long>(),
                MAILCODE = txtMail.Text,                                    JUSO1 = txtJuso1.Text,
                JUSO2 = txtJuso2.Text,                                      PTNO = txtPtno.Text,
                BURATE = VB.Pstr(cboBuRate.Text, ".", 1),                   GBSTS = FstrGBSTS,
                CARDSEQNO = 0,                                              GBDUST = strDust,
                JOBSABUN = clsType.User.IdNumber.To<long>(),                ENDOGBN = rdoEndo1.Checked ? "1" : "2",
                EXAMREMARK = txtExRemark.Text.Trim(),                       SUNAP = VB.Replace(txtChaAmt.Text, ",", "").To<long>(0) > 0 ? "N" : "Y",
                GUIDETEL = chkCall.Text.Trim(),                             EXAMCHANGE = txtRemark.Text.Trim(),
                AMPM2 = string.Compare(cboSTime.Text, "12:00") > 0 ? "2" : "1", GAMAMT = VB.Replace(txtHalinAmt.Text, ",", "").To<long>(0),
                GONGDAN = chkGongDan.Checked ? "Y" : "N",                   GONGDANAMT = FnGongdanAmt,
                TICKET = txtTikcet.Text.To<long>(0),                        GBJIKWON = "N",
                STIME = cboSTime.Text.Trim(),                               GBNAKSANG = strGbNaksang,
                KEYNO = txtKeyNo.Text.Trim(),                               EMAIL = txtEmail.Text.Trim(),
                GBCHK3 = strGbChk3,                                         GBJUSO = strGbJUSO
            };

            if (FstrGBSTS == "0") { rHJ.CDATE = clsPublic.GstrSysTime; }
            //2021-08-04(검진예정일이 변경되면 접수일자를 변경함
            if (FstrOldSdate!= dtpSDate.Text)
            {
                rHJ.JEPDATE = clsPublic.GstrSysDate;
                Insert_Jepsu_History();
            }

            //결과지 수령방법(2021-05-24) 
            if (rdoRES12.Checked)
            {
                //strGbJUSO = chkRES12_1.Checked ? "Y" : "N"; //우편수령 (집, 회사)

                if (chkRES12_1.Checked == true)
                {
                    rHJ.GBJUSO = "Y";
                }
                else if (chkRES12_2.Checked == true)
                {
                    //2021-03-23 우편회사 선택시 회사주소 자동연동
                    rHJ.GBJUSO = "N";
                    HIC_LTD nHL = hicLtdService.GetMailCodebyCode(rHJ.LTDCODE.To<string>());

                    if (!nHL.IsNullOrEmpty())
                    {
                        rHJ.MAILCODE = nHL.MAILCODE;
                        rHJ.JUSO1 = nHL.JUSO;
                        rHJ.JUSO2 = nHL.JUSODETAIL;
                    }
                }
                else if (chkRES12_3.Checked == true)
                {
                    rHJ.GBJUSO = "E";
                    rHJ.MAILCODE = txtMail1.Text;
                    rHJ.JUSO1 = txtJuso11.Text;
                    rHJ.JUSO2 = txtJuso21.Text;
                }
            }
            else
            {
                if (rdoRES15.Checked)
                {
                    rHJ.GBCHK3 = "Y";
                    strGbChk3 = "Y";    //방문수령
                }
            }

            return rHJ;
        }

        private void AMT_SETTING()
        {
            FnTotAmt = 0;   FnLtdAmt = 0;
            FnBoninAmt = 0; FnCardAmt = 0;
            FnIpGumAmt = 0; FnHalinAmt = 0;

            FnTotAmt = VB.Replace(txtTotAmt.Text, ",", "").To<long>(0);
            FnLtdAmt = VB.Replace(txtLtdAmt.Text, ",", "").To<long>(0);
            FnBoninAmt = VB.Replace(txtBonAmt.Text, ",", "").To<long>(0);
            FnCardAmt = VB.Replace(txtCardAmt.Text, ",", "").To<long>(0);
            FnIpGumAmt = VB.Replace(txtCashAmt.Text, ",", "").To<long>(0);
            FnHalinAmt = VB.Replace(txtHalinAmt.Text, ",", "").To<long>(0);
        }

        private void Insert_Jepsu_History()
        {
            //접수History

            int result = 0;

            HEA_JEPSU item = new HEA_JEPSU
            {
                WRTNO = FnWRTNO,
                JOBSABUN = clsType.User.IdNumber.To<long>()
            };

            result = heaJepsuService.HisInsert(item);
            if (result < 0)
            {
                MessageBox.Show("HISTORY 저장 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }


        }

        private bool Jepsu_Check_Logic()
        {
            string strTmpChk = string.Empty;

            try
            {
                //2017-12-26 판정완료는 접수수정 금지(부장님만 가능함)
                if (FstrGBSTS == "9" && clsHcVariable.GbHeaAdminSabun == false)
                {
                    MessageBox.Show("판정이 완료되어 접수수정이 불가능합니다.", "오류");
                    return false;
                }

                if (clsHcVariable.B01_JONGGUM_SABUN == false)
                {
                    MessageBox.Show("판정이 완료되어 접수수정이 불가능합니다.", "오류");
                    return false;
                }

                //2017-09-26 병원직원은 자동으로 웹결과지를 선택함
                if (VB.Pstr(txtLtdCode.Text, ".", 1) == "483" && rdoRES14.Checked == false)
                {
                    rdoRES14.Checked = true;
                }

                if (rdoRES14.Checked == false)
                {
                    strTmpChk = hicJepsuService.GetWebPrintReqByJepDatePtno(dtpSDate.Text, txtPtno.Text);

                    if (!strTmpChk.IsNullOrEmpty())
                    {
                        if (MessageBox.Show("알림톡 대상제외로 접수 진행하시겠습니까?", "알림톡결과 확인", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            rdoRES14.Checked = true;
                        }
                    }
                }

                if (VB.Pstr(cboHalinGye.Text, ".", 1) == "925")
                {
                    //공단 1차검진비 회사부담에서 할인하는 회사 여부(True=대상)
                    string strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<string>().Trim();
                    string strLtdName = VB.Pstr(txtLtdCode.Text, ".", 2).To<string>().Trim();
                    strTmpChk = hicBcodeService.Read_Code_One("HEA_공단검진비회사부담감액", strLtdCode);
                    if (strTmpChk.IsNullOrEmpty())
                    {
                        MessageBox.Show(strLtdName + " 회사부담 감액 대상 회사가 아닙니다.", "오류");
                        return false;
                    }
                }

                if (VB.Pstr(txtFamilly.Text, ".", 2).To<long>() == txtPano.Text.To<long>())
                {
                    MessageBox.Show("가족 등록번호와 본인 등록번호가 동일함.", "오류");
                    return false;
                }

                if (FstrGBSTS != "0" && FstrGBSTS != "D")
                {
                    txtKeyNo.Text = txtKeyNo.Text.ToUpper();

                    if (txtKeyNo.Text.Trim() == "")
                    {
                        MessageBox.Show("옷장키 번호가 공란입니다.", "오류");
                        return false;
                    }

                    if (cboSex.Text == "M" && VB.Left(txtKeyNo.Text, 1) != "M")
                    {
                        MessageBox.Show("남자 옷장키 번호를 입력하세요.", "오류");
                        return false;
                    }

                    if (cboSex.Text == "F" && VB.Left(txtKeyNo.Text, 1) != "F")
                    {
                        MessageBox.Show("여자 옷장키 번호를 입력하세요.", "오류");
                        return false;
                    }
                }

                if (txtMail.Text.Length != 5)
                {
                    MessageBox.Show("우편번호는 5자리만 가능합니다.", "오류");
                    if(FstrGBSTS == "0") { FstrGBSTS = ""; }
                    return false;
                }

                if (FstrBuildNo.IsNullOrEmpty())
                {
                    MessageBox.Show("우편번호를 다시 검색해 주십시오.", "확인");
                    if (FstrGBSTS == "0") { FstrGBSTS = ""; }
                    return false;
                }

                //회사별 가예약이 가능한지 설정
                lstGaResvLtdCodes.Clear();
                lstGaResvLtdCodes = SET_GaResv_Ltd(VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0));

                //if (FnWRTNO == 0)
                //{
                //    strTmpChk = heaJepsuService.GetResvRowidByPtno(txtPtno.Text);
                //    if (!strTmpChk.IsNullOrEmpty())
                //    {
                //        clsPublic.GstrMsgList = "기존 예약건수가 존재합니다.";
                //        clsPublic.GstrMsgList += ComNum.VBLF + "예약을 수정하시거나, 예약접수건을 취소후 다시 작업하세요.";
                //        clsPublic.GstrMsgList += ComNum.VBLF + "접수불가!";

                //        MessageBox.Show(clsPublic.GstrMsgList, "예약수정");
                //        return false;
                //    }
                //}

                if (FstrGBSTS != "0")
                {
                    if (FstrGBSTS == "0" && dtpSDate.Text != DateTime.Now.ToShortDateString())
                    {
                        clsPublic.GstrMsgList = "당일은 검진예정일이 아닙니다. 접수불가!";
                        clsPublic.GstrMsgList += ComNum.VBLF + "당일접수는 검진예정일을 수정하여 주십시오.";

                        MessageBox.Show(clsPublic.GstrMsgList, "확인");
                        return false;
                    }

                    if (cboMisuGye.Text.Trim() != "")
                    {
                        clsPublic.GstrMsgList = "예약금을 접수비로 전환하여야 합니다.";
                        clsPublic.GstrMsgList += ComNum.VBLF + "미수계정(예약금선수납) 항목을 삭제하여 주십시오.";

                        MessageBox.Show(clsPublic.GstrMsgList, "확인");
                        return false;
                    }
                }

                //2013-10-28 외래 환자마스타의 성명과 입력한 성명이 틀리면 메세지 표시
                strTmpChk = comHpcLibBService.Read_SName(txtJumin1.Text, clsAES.AES(txtJumin2.Text));
                if (txtSName.Text.Trim() != strTmpChk)
                {
                    clsPublic.GstrMsgList = "외래성명과 이름이 다름니다 그래도 진행할까요?";
                    clsPublic.GstrMsgList += ComNum.VBLF + ComNum.VBLF + "입력하신 이름: " + txtSName.Text.Trim();
                    clsPublic.GstrMsgList += ComNum.VBLF + "외래 환자마스타 이름: " + strTmpChk;

                    if (MessageBox.Show(clsPublic.GstrMsgList, "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return false;
                    }
                }

                if (VB.Replace(txtLtdAmt.Text, ",", "").To<long>(0) > 0 && VB.Pstr(txtLtdCode.Text, ".", 1) == "")
                {
                    MessageBox.Show("회사부담금이 발생하였으나 사업장 코드가 공란입니다.", "오류");
                    return false;
                }

                switch (VB.Pstr(cboHalinGye.Text, ".", 1))
                {
                    case "007":
                    case "009":
                    case "010":
                    case "938":
                        if (txtJikSabun.Text.Trim() == "")
                        {
                            MessageBox.Show("추천직원사번을 입력하여 주십시오.", "감액계정 정보누락");
                            return false;
                        }
                        break;
                    default: break;
                }

                if (VB.Replace(txtTotAmt.Text, ",", "").To<long>() == 0)
                {
                    MessageBox.Show("총검진비가 0원 입니다.", "확인");
                    return false;
                }

                strTmpChk = VB.Pstr(cboJONG.Text, ".", 1);
                if (string.Compare(strTmpChk, "11") >= 0 && string.Compare(strTmpChk, "19") <= 0 && VB.Pstr(txtLtdCode.Text, ".", 1) == "")
                {
                    if (VB.Replace(txtLtdAmt.Text, ",", "").To<long>() > 0)
                    {
                        MessageBox.Show("개인종검에 회사미수액이 발생함.", "오류");
                        return false;
                    }
                }

                for (int i = 0; i < ssGroup.ActiveSheet.RowCount; i++)
                {
                    if (ssGroup.ActiveSheet.Cells[i, 1].Text.Trim() == "YY001")
                    {
                        if (cboHalinGye.Text != "" || VB.Replace(txtHalinAmt.Text, ",", "").To<long>() > 0)
                        {
                            MessageBox.Show("YY001 검사비용 단가할인의 경우 할인계정을 사용할 수 없음.");
                            return false;
                        }
                    }
                }

                strTmpChk = "";

                //검사일정 세팅 대상 선택
                List<GROUPCODE_EXAM_DISPLAY> varGED = new List<GROUPCODE_EXAM_DISPLAY>();
                varGED = grpExam.FindAll(x => x.ETCEXAM == "Y");

                //검사일정 정보가 없는 경우(신규예약) 
                if (clsHcVariable.LSTHaRESEXAM.Count == 0)
                {
                    if (cHB.Read_Exam_Schedule(txtPano.Text.To<long>(0), dtpSDate.Text, varGED) == "OK")
                    {
                        clsHcVariable.GbClose = false;
                        btnExamSearch.PerformClick();
                        clsHcVariable.GbClose = true;
                    }
                }
                else
                {
                    //검사일정 정보가 있거나(수정) 검사일정이 저장된 경우 (검사일정이 없는 경우 세팅창 팝업)
                    for (int i = 0; i < clsHcVariable.LSTHaRESEXAM.Count; i++)
                    {
                        if (cHB.Read_Exam_Schedule_One(txtPano.Text.To<long>(0), dtpSDate.Text, clsHcVariable.LSTHaRESEXAM[i].EXCODE) == "OK")
                        {
                            clsHcVariable.GbClose = false;
                            btnExamSearch.PerformClick();
                            clsHcVariable.GbClose = true;
                            strTmpChk = "OK";
                            break;
                        }
                    }

                    if (strTmpChk == "")
                    {
                        //선택된 검사항목들의 검사일정을 점검 (검사일정이 없는 경우 세팅창 팝업)
                        for (int i = 0; i < varGED.Count; i++)
                        {
                            if (varGED[i].RowStatus != RowStatus.Delete)
                            {
                                if (cHB.Read_Exam_Schedule_One(txtPano.Text.To<long>(0), dtpSDate.Text, varGED[i].EXCODE) == "OK")
                                {
                                    clsHcVariable.GbClose = false;
                                    btnExamSearch.PerformClick();
                                    clsHcVariable.GbClose = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                //수검일자 외 다른날짜 선택검사일정 전체 삭제
                if (!heaResvExamService.UpDateDelDateByPanoNotSDate(txtPano.Text.To<long>(0), dtpSDate.Text))
                {
                    MessageBox.Show("선택검사일정 삭제시 오류발생!", "오류");
                    return false;
                }

                //선택된 검사항목외 다른 선택검사일정 전체 삭제
                if (varGED.Count > 0)
                {
                    List<string> lstExCD = new List<string>();

                    for (int i = 0; i < varGED.Count; i++)
                    {
                        if (varGED[i].RowStatus != RowStatus.Delete)
                        {
                            lstExCD.Add(varGED[i].EXCODE);
                        }
                    }

                    if (!heaResvExamService.UpDateDelDateByPanoNotExam(txtPano.Text.To<long>(0), lstExCD))
                    {
                        MessageBox.Show("선택검사일정 삭제시 오류발생!", "오류");
                        return false;
                    }
                }
                

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Clear();

            panSub01.AddRequiredControl(txtJumin1);
            panSub01.AddRequiredControl(txtJumin2);
            panSub01.AddRequiredControl(txtSName);
            panSub01.AddRequiredControl(txtPtno);

            panJepList.AddRequiredControl(cboSTime);

            panBuRate.AddRequiredControl(cboBuRate);

            panPAT.AddRequiredControl(txtMail);
            panPAT.AddRequiredControl(txtJuso1);
            panPAT.AddRequiredControl(txtHphone);

            themTabForm(frmHcPermission, this.panPerm);
            //themTabForm(frmHcEmrPermission, this.panPerm);
            

            string strWaitPcNo = string.Empty;

            FileInfo nFILE = null;

            string strFIleNm = @"c:\HIC_WAIT.ini";
            nFILE = new FileInfo(strFIleNm);

            if (nFILE.Exists)
            {
                StreamReader SR = new StreamReader(strFIleNm, System.Text.Encoding.Default);

                strWaitPcNo = SR.ReadToEnd();

                SR.Close();

                strWaitPcNo = VB.Pstr(VB.Pstr(strWaitPcNo, "{}", 1), "=", 2);
                strWaitPcNo = VB.Pstr(strWaitPcNo, "번", 1);

                if (!strWaitPcNo.IsNullOrEmpty())
                {
                    if (hicWaitService.GetRowidByJobDatePcNo(strWaitPcNo).IsNullOrEmpty())
                    {
                        //CAll 작동시 소리 나게 해주는 베이스 Row
                        hicWaitService.InsertHicWaitPcRow(strWaitPcNo);
                    }
                }
            }

            lblCall.Text = "0";

            //초대권 SMS 전송용 휴대폰번호 설정
            string strHTel = "";
            string strSabun = "";
            List<HIC_BCODE> lstBCode = hicBcodeService.GetItembyGubun("HEA_종검초대권배부자");
            if (lstBCode.Count > 0)
            {
                for (int i = 0; i < lstBCode.Count; i++)
                {
                    strSabun = VB.Pstr(lstBCode[i].NAME, ";", 2).Trim();
                    //인사마스타에서 휴대폰번호를 읽음(퇴사자 제외)
                    strHTel = comHpcLibBService.GetInsaMstHTelBySabun(strSabun);

                    if (!strHTel.IsNullOrEmpty())
                    {
                        lstTicketSMS.Add(lstBCode[i].NAME + ";" + strHTel);
                    }
                }
            }

            timer1.Start();
        }

        /// <summary>
        /// 자격조회 초기화
        /// </summary>
        /// <param name="argJumin"></param>
        private void Clear_Nhic_List()
        {
            string strJumin = txtJumin1.Text + txtJumin2.Text;

            if (strJumin.IsNullOrEmpty()) { return; }

            int result = workNhicService.DeleteDataAllByJuminNo(clsAES.AES(strJumin));
            if (result <= 0)
            {
                MessageBox.Show("자격조회 초기화 실패", "에러");
                return;
            }

            Hic_Chk_Nhic("H", txtSName.Text, strJumin, txtPtno.Text, VB.Left(DateTime.Now.ToShortDateString(), 4));
        }

        private void SetDisPlay_Nhic(WORK_NHIC item)
        {
            if (item.IsNullOrEmpty())
            {
                return;
            }

            //인적정보 
            SS10.ActiveSheet.Cells[1, 1].Text = item.SNAME;                                   //성명
            SS10.ActiveSheet.Cells[2, 1].Text = clsAES.DeAES(item.JUMIN2);                    //주민등록
            SS10.ActiveSheet.Cells[3, 1].Text = item.REL;                                     //사업구분
            SS10.ActiveSheet.Cells[4, 1].Text = item.YEAR;                                    //사업년도
            SS10.ActiveSheet.Cells[5, 1].Text = item.TRANS;                                   //자격변동
            SS10.ActiveSheet.Cells[6, 1].Text = item.GKIHO;                                   //증번호
            SS10.ActiveSheet.Cells[7, 1].Text = item.JISA;                                    //소속지사
            SS10.ActiveSheet.Cells[8, 1].Text = item.BDATE;                                   //취득일자
            SS10.ActiveSheet.Cells[9, 1].Text = item.CANCER53;                                //관할보건소
            SS10.ActiveSheet.Cells[10, 1].Text = item.KIHO;                                   //회사기호

            //검사정보
            SS10.ActiveSheet.Cells[12, 1].Text = item.FIRST;                                  //1차검진
            SS10.ActiveSheet.Cells[13, 1].Text = item.DENTAL;                                 //구강검진
            SS10.ActiveSheet.Cells[14, 1].Text = item.LIVER;                                  //B형간염
            SS10.ActiveSheet.Cells[15, 1].Text = item.LIVERC;                                 //C형간염

            //SS10.ActiveSheet.Cells[16, 1].Text = item.SECOND;                                 //2차검진
            SS10.ActiveSheet.Cells[16, 1].Text = item.CANCER71 + "(" + item.CANCER72 + ")";   //폐암
            SS10.ActiveSheet.Cells[17, 1].Text = item.CANCER11 + "(" + item.CANCER12 + ")";   //위암
            SS10.ActiveSheet.Cells[18, 1].Text = item.CANCER21 + "(" + item.CANCER22 + ")";   //유방암
            SS10.ActiveSheet.Cells[19, 1].Text = item.CANCER31 + "(" + item.CANCER32 + ")";   //대장암
            if (string.Compare(VB.Mid(DateTime.Now.ToShortDateString(), 6, 5), "06-30") <= 0)
            {
                SS10.ActiveSheet.Cells[20, 1].Text = item.CANCER41 + "(" + item.CANCER42 + ")";      //간암(전반기)
            }
            else
            {
                SS10.ActiveSheet.Cells[20, 1].Text = item.CANCER61 + "(" + item.CANCER62 + ")";      //간암(후반기)
            }
            SS10.ActiveSheet.Cells[21, 1].Text = item.CANCER51 + "(" + item.CANCER52 + ")";   //자궁암
            SS10.ActiveSheet.Cells[12, 4].Text = item.EXAMA;                                  //이상지질혈증
            SS10.ActiveSheet.Cells[13, 4].Text = item.EXAMD;                                  //골밀도
            SS10.ActiveSheet.Cells[14, 4].Text = item.EXAME;                                  //인지기능
            SS10.ActiveSheet.Cells[15, 4].Text = item.EXAMF;                                  //정신건강
            SS10.ActiveSheet.Cells[16, 4].Text = item.EXAMG;                                  //생활습관
            SS10.ActiveSheet.Cells[17, 4].Text = item.EXAMH;                                  //노인신체
            SS10.ActiveSheet.Cells[18, 4].Text = item.EXAMI;                                  //치면세균
                                                                                              //SS10.ActiveSheet.Cells[21, 4].Text = item.CANCER71 + "(" + item.CANCER72 + ")";   //폐암
                                                                                              //수검정보
            SS10.ActiveSheet.Cells[24, 1].Text = item.GBCHK01 + " " + item.GBCHK01_NAME;      //1차검진
            SS10.ActiveSheet.Cells[25, 1].Text = item.GBCHK02 + " " + item.GBCHK02_NAME;      //2차검진
            SS10.ActiveSheet.Cells[26, 1].Text = item.GBCHK03 + " " + item.GBCHK03_NAME;      //구강검사
            SS10.ActiveSheet.Cells[27, 1].Text = item.GBCHK04 + " " + item.GBCHK04_NAME;      //위암
            SS10.ActiveSheet.Cells[28, 1].Text = item.GBCHK05 + " " + item.GBCHK05_NAME;      //대장암
            SS10.ActiveSheet.Cells[29, 1].Text = item.GBCHK06 + " " + item.GBCHK06_NAME;      //유방암
            SS10.ActiveSheet.Cells[30, 1].Text = item.GBCHK07 + " " + item.GBCHK07_NAME;      //자궁경부암
            SS10.ActiveSheet.Cells[31, 1].Text = item.GBCHK09 + " " + item.GBCHK09_NAME;      //간암
            SS10.ActiveSheet.Cells[32, 1].Text = item.GBCHK10 + " " + item.GBCHK10_NAME;      //폐암

            if (item.GBCHK04.IsNullOrEmpty())
            {
                SS10.ActiveSheet.Cells[27, 1].Text = item.GBCHK15 + " " + item.GBCHK15_NAME;      //위암
            }

            if (item.GBCHK05.IsNullOrEmpty())
            {
                SS10.ActiveSheet.Cells[28, 1].Text = item.GBCHK16 + " " + item.GBCHK16_NAME;      //대장암
            }

            if (item.GBCHK05.IsNullOrEmpty() && item.GBCHK16.IsNullOrEmpty())
            {
                SS10.ActiveSheet.Cells[28, 1].Text = item.GBCHK17 + " " + item.GBCHK17_NAME;      //대장암
            }

            //수검정보 글자 색깔표시
            Screen_Display_Spread_Nhic(SS10, item);
        }

        private void Screen_Display_Spread_Nhic(FpSpread ss10, WORK_NHIC item)
        {
            cSpd.setSpdForeColor(ss10, 0, 0, ss10.ActiveSheet.RowCount - 1, ss10.ActiveSheet.ColumnCount - 1, Color.Black);

            if (item.FIRST.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[12, 1].ForeColor = Color.Red; }
            if (item.DENTAL.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[13, 1].ForeColor = Color.Red; }
            if (item.LIVER.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[14, 1].ForeColor = Color.Red; }
            if (item.LIVERC.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[15, 1].ForeColor = Color.Red; }
            if (item.CANCER71.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[16, 1].ForeColor = Color.Red; }
            if (item.CANCER11.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[17, 1].ForeColor = Color.Red; }
            if (item.CANCER21.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[18, 1].ForeColor = Color.Red; }
            if (item.CANCER31.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[19, 1].ForeColor = Color.Red; }
            if (string.Compare(VB.Mid(DateTime.Now.ToShortDateString(), 6, 5), "06-30") <= 0)
            {
                if (item.CANCER41.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[20, 1].ForeColor = Color.Red; }
            }
            else
            {
                if (item.CANCER61.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[20, 1].ForeColor = Color.Red; }
            }

            if (item.CANCER51.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[21, 1].ForeColor = Color.Red; }
            if (item.EXAMA.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[12, 4].ForeColor = Color.Red; }
            if (item.EXAMD.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[13, 4].ForeColor = Color.Red; }
            if (item.EXAME.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[14, 4].ForeColor = Color.Red; }
            if (item.EXAMF.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[15, 4].ForeColor = Color.Red; }
            if (item.EXAMG.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[16, 4].ForeColor = Color.Red; }
            if (item.EXAMH.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[17, 4].ForeColor = Color.Red; }
            if (item.EXAMI.To<string>("").Contains("수검완료")) { ss10.ActiveSheet.Cells[18, 4].ForeColor = Color.Red; }
        }

        private void Set_Exam_Select()
        {
            clsHcType.TEC.JEPDATE = dtpSDate.Text;
            clsHcType.TEC.JONG = VB.Pstr(cboJONG.Text, ".", 1);
            clsHcType.TEC.SNAME = txtSName.Text;
            clsHcType.TEC.JUMINFULL = txtJumin1.Text + txtJumin2.Text;
            clsHcType.TEC.SEX = cboSex.Text;
            clsHcType.TEC.AGE = txtAge.Text.To<int>(0);
            clsHcType.TEC.GUBUN = "";

            if (clsHcType.TEC.NHICOK == "Y")
            {
                if (clsHcType.THNV.hJaGubun.To<string>("").Trim() != "의료급여")
                {
                    clsHcType.TEC.GUBUN = "A";
                }
                else if (clsHcType.THNV.hJaGubun.To<string>("").Trim() == "의료급여" && clsHcType.TEC.AGE >= 66)
                {
                    clsHcType.TEC.GUBUN = "B";
                }
                else if (clsHcType.THNV.hJaGubun.To<string>("").Trim() == "의료급여" && clsHcType.TEC.AGE < 66)
                {
                    clsHcType.TEC.GUBUN = "A";
                }
                else
                {
                    clsHcType.TEC.GUBUN = "";
                }
            }
            else
            {
                if (clsHcType.TEC.JONG == "11")
                {
                    clsHcType.TEC.GUBUN = "A";
                }
            }

        }

        private void Screen_Clear()
        {
            panMain.Initialize();
            panSub01.Initialize();
            panPAT.Initialize();
            panJepList.Initialize();
            panAMT.Initialize();
            panGoto.Initialize();
            panMain04.Initialize();

            rdoSTS1.Checked = true;
            rdoEndo1.Checked = true;
            rdoMemo2.Checked = true;

            Clear_Result_Reciept_Control();

            ssETC.SetDataSource(null);
            cSpd.Spread_Clear_Simple(ssETC, 5);
            
            clsHcType.THNV_CLEAR();
            Screen_Clear_Nhic();
            Screen_Clear_Excel();
            Clear_Spread_GroupCode();
            Clear_Working_Variants();

            cboJONG.Enabled = false;
            cboSTime.SelectedIndex = 0;
            cboYear.SelectedIndex = 0;
            cboBuRate.SelectedIndex = 0;
            dtpSDate.Enabled = true;

            chkCall.Text = "";
            chkStool.Enabled = false;
            chkSputum.Enabled = false;
            chkUrine.Enabled = false;
            chkStool.Checked = false;
            chkSputum.Checked = false;
            chkUrine.Checked = false;
            panExamDtl.Expanded = false;

            lblCall.Text = "0";                 //호출번호
            lblCurDate.Text = DateTime.Now.ToShortDateString();

            lblSunap.Visible = false;           //수납여부
            lblSangDam.Visible = false;         //당일상담여부
            lblIEMunjin.Visible = false;        //인터넷문진
            lblIEMunjin.Text = "";
            lblIEMunjin.BackColor = Color.White;
            lblSTS.Text = "";                   //수검상태
            lblSTS.BackColor = Color.LightGray;
            lblSTS2.Text = "";
            lblSunap.Visible = false;
            lblSangDam.Visible = false;
            lblMsg.Text = "";                   //폼 하단 메세지
            lblMsg.BackColor = Color.White;
            lblUCodeJong.Text = "유해인자";
            lblUCODES.Text = "";
            lblTicket.Text = "";

            FstrPtno = "";
            FstrBuildNo = "";
            FstrJong = "";
            FstrBuRate = "";
            FstrGBSTS = "";
            FstrAddExamYN = "";

            FnPano = 0;
            FnIEMunNo = 0;
            FnWRTNO = 0;
            FnTotAmt = 0;   FnLtdAmt = 0;
            FnBoninAmt = 0; FnCardAmt = 0;
            FnIpGumAmt = 0; FnHalinAmt = 0;
            FnGongdanAmt = 0;
            FbCall = false;

            btnReset.Visible = false;
            btnResReciept.Enabled = false;

            frmHcPermission.Screen_Clear();
            //frmHcEmrPermission.Screen_Clear();

            HaJEPSU = new HEA_JEPSU();
            HaGrpCD = new HEA_GROUPCODE();

            clsHcVariable.LSTHaRESEXAM.Clear(); //예약검사 정보
            clsHcVariable.GbGaResvLtd = false;  //회사 예약검사 가능여부

            panSub01.Enabled = true;
            panSelExam.Enabled = false;

            txtLtdRemark.Text = "";
            txtJepResvCnt.Text = "";

            txtJumin1.Focus();
        }

        private void Clear_Working_Variants()
        {
            if (!suInfo.IsNullOrEmpty()) { suInfo.Clear(); }
            else { suInfo = new List<READ_SUNAP_ITEM>(); }

            if (!grpExam.IsNullOrEmpty()) { grpExam.Clear(); }
            else { grpExam = new List<GROUPCODE_EXAM_DISPLAY>(); }

            if (!sunap.IsNullOrEmpty()) { sunap = null; }
            else { sunap = new HIC_SUNAP(); }
        }

        private void Screen_Clear_Excel()
        {
            for (int i = 0; i < SS3.ActiveSheet.RowCount; i++)
            {
                SS3.ActiveSheet.Cells[i, 1].Text = "";
            }
        }

        /// <summary>
        /// 수검자 메모 Display
        /// </summary>
        /// <param name="argPtno"></param>
        private void Hic_Memo_Screen(string argPtno)
        {
            string strGbn = rdoMemo3.Checked ? "" : rdoMemo1.Checked ? "HIC" : "HEA";

            if (argPtno.IsNullOrEmpty()) { return; }

            ComFunc CF = null;
            List<HIC_MEMO> list = null;

            CF = new ComFunc();

            if (strGbn == "HIC")
            {
                list = hicMemoService.GetHicItembyPaNo(argPtno);
            }
            else if (strGbn == "HEA")
            {
                list = hicMemoService.GetHeaItembyPaNo(argPtno);
            }
            else
            {
                list = hicMemoService.GetItembyPaNo(argPtno, "");
            }


            ssETC.SetDataSource(list);

            if (!list.IsNullOrEmpty() && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Size size = ssETC.ActiveSheet.GetPreferredCellSize(i, 3);
                    ssETC.ActiveSheet.Rows[i].Height = size.Height;
                    ssETC.ActiveSheet.Cells[i, 5].Text = list[i].JOBNAME;
                }    
            }

            ssETC.AddRows(5);

            ssETC.ActiveSheet.SetActiveCell(0, 3);
            ssETC.ShowActiveCell(VerticalPosition.Nearest, HorizontalPosition.Nearest);

            CF = null;
        }

        /// <summary>
        /// 수검자 메모 저장 및 삭제
        /// </summary>
        /// <param name="item"></param>
        private void Hic_Memo_Save()
        {
            int result = 1;

            for (int i = 0; i < ssETC.ActiveSheet.RowCount; i++)
            {
                if (ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.GBDEL].Text == "True")
                {
                    //Delete Data
                    string strGbn = ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.JOBGBN].Text;
                    string strRid = ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.ROWID].Text;

                    result = hicMemoService.DeleteData(ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.ROWID].Text, strGbn);
                }
                else if (ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.ROWID].Text == "")
                {
                    if (ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.MEMO].Text != "")
                    {
                        //Insert Data
                        HIC_MEMO item = new HIC_MEMO
                        {
                            WRTNO = FnWRTNO,
                            MEMO = ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.MEMO].Text,
                            JOBSABUN = clsType.User.IdNumber.To<long>(),
                            PTNO = FstrPtno,
                            PANO = FnPano,
                            JOBGBN = "종검"
                        };

                        result = hicMemoService.Insert(item);
                    }
                }
            }

            if (result <= 0)
            {
                MessageBox.Show("수검자 메모 저장 오류.", "ERROR");
                return;
            }

            ssETC.ActiveSheet.ClearRange(0, 0, ssETC.ActiveSheet.Rows.Count, ssETC.ActiveSheet.ColumnCount, true);
            ssETC.ActiveSheet.Rows.Count = 0;

            Hic_Memo_Screen(FstrPtno);
        }

        private void Screen_Clear_Nhic()
        {
            for (int i = 1; i < 11; i++) { SS10.ActiveSheet.Cells[i, 1].Text = ""; }
            for (int i = 12; i < 22; i++) { SS10.ActiveSheet.Cells[i, 1].Text = ""; SS10.ActiveSheet.Cells[i, 4].Text = ""; }
            for (int i = 24; i < 32; i++) { SS10.ActiveSheet.Cells[i, 1].Text = ""; }
        }

        /// <summary>
        /// 검사 그룹코드 Spread Clear    
        /// </summary>
        private void Clear_Spread_GroupCode(string argGbn = "")
        {
            ssGroup.DataSource = null;
            ssGroup.ActiveSheet.ClearRange(0, 0, ssGroup.ActiveSheet.Rows.Count, ssGroup.ActiveSheet.ColumnCount, true);
            ssGroup.ActiveSheet.Rows.Count = 0;

            if (argGbn != "Group")
            {
                ssExam.DataSource = null;
                ssExam.ActiveSheet.ClearRange(0, 0, ssExam.ActiveSheet.Rows.Count, ssExam.ActiveSheet.ColumnCount, true);
                ssExam.ActiveSheet.Rows.Count = 0;
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

        private void FormVisiable(Form frm)
        {
            frm.Visible = false;
            frm.Visible = true;
            frm.BringToFront();
        }

        private void Ticket_Name_Display()
        {
            long nTicket = txtTikcet.Text.To<long>(0);

            if (nTicket == 0) { return; }

            COMHPC item = comHpcLibBService.GetTicketInfoByTicketNo(nTicket);

            if (!item.IsNullOrEmpty())
            {
                lblTicket.Text = item.BSAWON;

                if (!item.BNAME.IsNullOrEmpty())
                {
                    lblTicket.Text += "," + item.BNAME;
                }

                if (!item.REMARK.IsNullOrEmpty())
                {
                    lblTicket.Text += "," + item.REMARK;
                }
            }
            else
            {
                lblTicket.Text = "";
            }
        }

        /// <summary>
        /// 식권출력
        /// </summary>
        /// <param name="list"></param>
        /// <seealso cref="Cmd식권_Click"/>
        private void Print_Food_Ticket(HEA_JEPSU nHJ, string argSikGbn)
        {
            frmHaPrintFoodTicket fHPP = new frmHaPrintFoodTicket(nHJ, argSikGbn);
            fHPP.ShowDialog();
        }

        private void EXAM_UPDATE()
        {
            for (int i = 0; i< lstDelExam.Count; i++)
            {
          
                string strXcode = "";
                string strRowid_Xray = "";

                strXcode = hicExcodeService.Read_XrayCode(lstDelExam[i]);
                if (!strXcode.IsNullOrEmpty())
                {
                    strRowid_Xray = xrayDetailService.GetRowidByPanoXCodeBDateDept(FstrPtno, strXcode.Trim(), dtpSDate.Text, "TO");
                    if (!strRowid_Xray.IsNullOrEmpty())
                    {
                        xrayDetailService.UpDate_XrayDetail_Del(strRowid_Xray);
                    }

                }
            }
        }

        /// <summary>
        /// 검사안내접수증세팅_New
        /// </summary>
        /// <param name="list"></param>
        /// <seealso cref="검사안내접수증세팅_New"/>
        private void JepsuPrintSetting(HEA_JEPSU nHJ)
        {
            int Index = 0;
            long nEndo1 = 0;
            long nEndo2 = 0;
            long nEndo3 = 0;
            long nEndo4 = 0;

            string strCode = "";
            string strGoto = "";
            string strName = "";
            string strCodeName = "";
            string strGBPrint = "";


            for (int i = 0; i < FstrChkExam.Length; i++) { FstrChkExam[i] = ""; }
            for (int i = 0; i < FstrChkExamName.Length; i++) { FstrChkExamName[i] = ""; }
            for (int i = 0; i < FstrContrast.Length; i++) { FstrContrast[i] = ""; }
            for (int i = 0; i < strMsg.Length; i++) { strMsg[i] = ""; }

            for (int i = 0; i < ssExam.ActiveSheet.RowCount; i++)
            {
                    
                if (ssExam.ActiveSheet.Cells[i, 1].Text  != "false")
                {
                    strCode = ssExam.ActiveSheet.Cells[i, 1].Text;
                    HIC_EXCODE item = hicExcodeService.GetEndoGubunbyCode(strCode);
                        
                    if(!item.IsNullOrEmpty())
                    {
                        if (!item.ENDOGUBUN2.IsNullOrEmpty())
                        {
                            if (item.ENDOGUBUN2.Trim() == "Y") { nEndo1 = nEndo1 + 1; } //위내시경
                        }
                        if (!item.ENDOGUBUN3.IsNullOrEmpty())
                        {
                            if (item.ENDOGUBUN3.Trim() == "Y") { nEndo2 = nEndo2 + 1; } //위수면내시경
                        }
                        if (!item.ENDOGUBUN4.IsNullOrEmpty())
                        {
                            if (item.ENDOGUBUN4.Trim() == "Y") { nEndo3 = nEndo3 + 1; } //대장내시경
                        }
                        if (!item.ENDOGUBUN5.IsNullOrEmpty())
                        {
                            if (item.ENDOGUBUN5.Trim() == "Y") { nEndo4 = nEndo4 + 1; } //대장수면내시경
                        }
                    }
                }
            }

            if (nEndo3 > 0 || nEndo4 > 0 )
            {
                chkPrt6.Checked = true;
                FstrChkExamName[5] = chkPrt6.Text;
                if (nEndo2 > 0 && nEndo4 > 0)
                {
                    FstrChkExam[5] = "위+대장내시경(수면)";
                }
                else
                {
                    if (nEndo2 > 0)
                    {
                        FstrChkExam[5] = "위내시경(수면),";
                    }
                    else if (nEndo1 > 0)
                    {
                        FstrChkExam[5] = "위내시경(일반),";
                    }


                    if (nEndo4 > 0)
                    {
                        FstrChkExam[5] = FstrChkExam[5] + "대장내시경(수면)";
                    }
                    else
                    {
                        FstrChkExam[5] = FstrChkExam[5] + "대장내시경(일반)";
                    }
                }
                
            }
            else if (nEndo1 > 0 || nEndo2 > 0)
            {
                if (nEndo2 > 0)
                {
                    FstrChkExam[5] = "위내시경(수면),";
                }
                else if (nEndo1 > 0)
                {
                    FstrChkExam[5] = "위내시경(일반),";
                }
            }

            for (int i = 0; i < ssGroup.ActiveSheet.RowCount; i++)
            {
                strCode = ssGroup.ActiveSheet.Cells[i, 1].Text;
                strCodeName = ssGroup.ActiveSheet.Cells[i, 2].Text;
                HEA_GROUPCODE item2 = heaGroupcodeService.GetItemByCodeDeldate(strCode);

                if (!item2.IsNullOrEmpty())
                {
                    if (!item2.GBPRINT.IsNullOrEmpty())
                    {
                        strGBPrint = item2.GBPRINT;
                        for (int j = 1; j < VB.L(strGBPrint, ","); j++)
                        {
                            if (VB.PP(strGBPrint, ",", j) == "1")
                            {
                                switch (j)
                                {
                                    //1,2,3,4, 5 모두 신관 1층영상의학과
                                    case 1:
                                        chkPrt1.Checked = true;
                                        FstrChkExamName[0] = chkPrt1.Text;
                                        break;
                                    case 2:
                                        chkPrt1.Checked = true;
                                        FstrChkExamName[1] = chkPrt1.Text;
                                        break;
                                    case 3:
                                        chkPrt1.Checked = true;
                                        FstrChkExamName[2] = chkPrt1.Text;
                                        break;
                                    case 4:
                                        chkPrt1.Checked = true;
                                        FstrChkExamName[3] = chkPrt1.Text;
                                        break;
                                    case 5:
                                        chkPrt1.Checked = true;
                                        FstrChkExamName[4] = chkPrt1.Text;
                                        break;
                                    case 6:
                                        chkPrt6.Checked = true;
                                        FstrChkExamName[5] = chkPrt6.Text;
                                        break;
                                    case 7:
                                        chkPrt7.Checked = true;
                                        FstrChkExamName[6] = chkPrt7.Text;
                                        break;
                                    case 8:
                                        chkPrt8.Checked = true;
                                        FstrChkExamName[7] = chkPrt8.Text;
                                        break;
                                    case 9:
                                        chkPrt9.Checked = true;
                                        FstrChkExamName[8] = chkPrt9.Text;
                                        break;
                                    case 10:
                                        chkPrt10.Checked = true;
                                        FstrChkExamName[9] = chkPrt10.Text;
                                        break;
                                    case 11:
                                        chkPrt11.Checked = true;
                                        FstrChkExamName[10] = chkPrt11.Text;
                                        break;
                                    case 12:
                                        chkPrt12.Checked = true;
                                        FstrChkExamName[11] = chkPrt12.Text;
                                        break;
                                    default: break;
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < ssExam.ActiveSheet.RowCount; i++)
            {

                if (ssExam.ActiveSheet.Cells[i, 1].Text !=  "false")
                {
                    strCode = ssExam.ActiveSheet.Cells[i, 1].Text;          //검사코드
                    strCodeName = ssExam.ActiveSheet.Cells[i, 2].Text;      //검사명

                    if (strCode == "TX67")
                    {
                        strCode = strCode;
                    }

                    //strMsg1~9 작업
                    switch (strCode)
                    {
                        case "A803":
                            strMsg[1] = " PAP(ACT)";
                            break;
                        case "TX98":
                            strMsg[2] = " 골반초음파";
                            break;
                        case "E914":
                            strMsg[3] = " U-Urealticum culture";
                            break;
                        case "E913":
                            strMsg[4] = " M-hominis culture";
                            break;
                        case "ZE39":
                            strMsg[5] = " HPV-DNA";
                            break;
                        case "A940":
                            strMsg[6] = " HPV-DNA(ACT)";
                            break;
                        case "A914":
                            strMsg[7] = " 마이코플라즈마균배양";
                            break;
                        case "A801":
                            strMsg[8] = " 골반초음파(ACT)";
                            break;
                        case "A161":
                            strMsg[9] = " 부인과세포검사(유형별)";
                            break;
                        default: break;
                    }



                    HIC_EXCODE item3 = hicExcodeService.GetGotobyCode(strCode);
                    if(!item3.GOTO.IsNullOrEmpty())
                    {
                        strGoto = item3.GOTO;
                        HIC_CODE item4 = hicCodeService.GetNameGcodebyGubunCode("A7", strGoto);
                        if(!item4.IsNullOrEmpty())
                        {
                            strName = item4.NAME;
                            if(!item4.GCODE.IsNullOrEmpty())
                            {
                                Index = Convert.ToInt32(item4.GCODE);
                                //일반내시경은 종검내시경실에서 함
                                if(Index >= 0 && Index <=11 && Index != 5)
                                {
                                    switch (Index+1)
                                    {
                                        case 1:
                                            chkPrt1.Checked = true;
                                            FstrChkExamName[0] = chkPrt1.Text;
                                            break;
                                        case 2:
                                            chkPrt1.Checked = true;
                                            FstrChkExamName[1] = chkPrt1.Text;
                                            break;
                                        case 3:
                                            chkPrt1.Checked = true;
                                            FstrChkExamName[2] = chkPrt1.Text;
                                            break;
                                        case 4:
                                            chkPrt1.Checked = true;
                                            FstrChkExamName[3] = chkPrt1.Text;
                                            break;
                                        case 5:
                                            chkPrt1.Checked = true;
                                            FstrChkExamName[4] = chkPrt1.Text;
                                            break;
                                        case 6:
                                            chkPrt6.Checked = true;
                                            FstrChkExamName[5] = chkPrt6.Text;
                                            break;
                                        case 7:
                                            chkPrt7.Checked = true;
                                            FstrChkExamName[6] = chkPrt7.Text;
                                            break;
                                        case 8:
                                            chkPrt8.Checked = true;
                                            FstrChkExamName[7] = chkPrt8.Text;
                                            break;
                                        case 9:
                                            chkPrt9.Checked = true;
                                            FstrChkExamName[8] = chkPrt9.Text;
                                            break;
                                        case 10:
                                            chkPrt10.Checked = true;
                                            FstrChkExamName[9] = chkPrt10.Text;
                                            break;
                                        case 11:
                                            chkPrt11.Checked = true;
                                            FstrChkExamName[10] = chkPrt11.Text;
                                            break;
                                        case 12:
                                            chkPrt12.Checked = true;
                                            FstrChkExamName[11] = chkPrt12.Text;
                                            break;
                                        default: break;
                                    }

                                    if(FstrChkExam[Index].IsNullOrEmpty())
                                    {
                                        FstrChkExam[Index] = strCodeName;
                                    }
                                    else
                                    {
                                        FstrChkExam[Index] = FstrChkExam[Index] + "," + strCodeName;
                                    }

                                    // 접수증에 3-way, Jellco 인쇄

                                    //B08_조영제사용검사

                                    if (!heaCodeService.GetNameByGubunCode("17", strCode).IsNullOrEmpty())
                                    {
                                        FstrContrast[Index] = "Y";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //BMD는 종검 자체에서 감사함
            if (chkPrt5.Checked) { chkPrt5.Checked = false; }
            if (chkPrt11.Checked) { chkPrt11.Checked = false; FstrChkExam[10] = ""; }

            if (rdoEndo1.Checked) { FstrChkExam[5] = ""; }
            

            //for (int i = 1; i < 13; i++)
            //{
            //    if (chkPrt1.Checked = true) { FstrChkExamName[0] = chkPrt1.Text; }
            //    if (chkPrt2.Checked = true) { FstrChkExamName[1] = chkPrt2.Text; }
            //    if (chkPrt3.Checked = true) { FstrChkExamName[2] = chkPrt3.Text; }
            //    if (chkPrt4.Checked = true) { FstrChkExamName[3] = chkPrt4.Text; }
            //    if (chkPrt5.Checked = true) { FstrChkExamName[4] = chkPrt5.Text; }
            //    if (chkPrt6.Checked = true) { FstrChkExamName[5] = chkPrt6.Text; }
            //    if (chkPrt7.Checked = true) { FstrChkExamName[6] = chkPrt7.Text; }
            //    if (chkPrt8.Checked = true) { FstrChkExamName[7] = chkPrt8.Text; }
            //    if (chkPrt9.Checked = true) { FstrChkExamName[8] = chkPrt9.Text; }
            //    if (chkPrt10.Checked = true) { FstrChkExamName[9] = chkPrt10.Text; }
            //    if (chkPrt11.Checked = true) { FstrChkExamName[10] = chkPrt11.Text; }
            //    if (chkPrt12.Checked = true) { FstrChkExamName[11] = chkPrt12.Text; }
            //} 
        }
    }
}
