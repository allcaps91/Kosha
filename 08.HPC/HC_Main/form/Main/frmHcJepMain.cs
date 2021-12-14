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
using ComPmpaLibB;
using DevComponents.DotNetBar; 
using FarPoint.Win.Spread;
using HC_Main.Model;
using HC_Main.Service;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using frmHcPanPersonResult = ComHpcLibB.frmHcPanPersonResult;

/// <summary>
/// 운영시 Key Point
/// 1. eCboKeyDown -> 신규
/// 2. Display_JepsuWork_Detail -> 가접수 (수정, 신규전환, 가접수 삭제 안됨)
/// 3. Display_Jepsu_Detail - > 접수 수정 (수정, 삭제)
/// 
/// 코드 조정 구간
/// frmHcGroupCode_ssDblclick -> 오버 로딩 (추가검사코드 연동, 검사코드 일괄 추가)
/// 
/// 저장 : Data_Save_Process();
/// </summary>
namespace HC_Main
{
    public partial class frmHcJepMain :BaseForm
    {
        #region Declare Variable Area
        HIC_LTD     LtdHelpItem     = null;
        HIC_CODE    CodeHelpItem    = null;
        HIC_PATIENT Hpatient        = null;
        HIC_JEPSU   hJepsu          = null;
        System.Windows.Forms.ToolTip toolTip = null;
        #endregion
        #region 검진종류별 수납 그룹코드
        List<READ_SUNAP_ITEM> suInfo0  = new List<READ_SUNAP_ITEM>();          //검진1
        List<READ_SUNAP_ITEM> suInfo1  = new List<READ_SUNAP_ITEM>();          //검진2
        List<READ_SUNAP_ITEM> suInfo2  = new List<READ_SUNAP_ITEM>();          //검진3
        List<READ_SUNAP_ITEM> suInfo3  = new List<READ_SUNAP_ITEM>();          //검진4
        List<READ_SUNAP_ITEM> suInfo4  = new List<READ_SUNAP_ITEM>();          //검진5
        #endregion

        #region 검진종류별 이전수납 그룹코드
        List<READ_SUNAP_ITEM> oldSuInfo0  = new List<READ_SUNAP_ITEM>();       //검진1
        List<READ_SUNAP_ITEM> oldSuInfo1  = new List<READ_SUNAP_ITEM>();       //검진2
        List<READ_SUNAP_ITEM> oldSuInfo2  = new List<READ_SUNAP_ITEM>();       //검진3
        List<READ_SUNAP_ITEM> oldSuInfo3  = new List<READ_SUNAP_ITEM>();       //검진4
        List<READ_SUNAP_ITEM> oldSuInfo4  = new List<READ_SUNAP_ITEM>();       //검진5
        #endregion

        #region 검진종류별 검사코드들
        List<GROUPCODE_EXAM_DISPLAY> grpExam0  = new List<GROUPCODE_EXAM_DISPLAY>();    //검진1
        List<GROUPCODE_EXAM_DISPLAY> grpExam1  = new List<GROUPCODE_EXAM_DISPLAY>();    //검진2
        List<GROUPCODE_EXAM_DISPLAY> grpExam2  = new List<GROUPCODE_EXAM_DISPLAY>();    //검진3
        List<GROUPCODE_EXAM_DISPLAY> grpExam3  = new List<GROUPCODE_EXAM_DISPLAY>();    //검진4
        List<GROUPCODE_EXAM_DISPLAY> grpExam4  = new List<GROUPCODE_EXAM_DISPLAY>();    //검진5
        #endregion

        #region 검진종류별 수납정보
        HIC_SUNAP sunap0  = new HIC_SUNAP();         //검진1
        HIC_SUNAP sunap1  = new HIC_SUNAP();         //검진2
        HIC_SUNAP sunap2  = new HIC_SUNAP();         //검진3
        HIC_SUNAP sunap3  = new HIC_SUNAP();         //검진4
        HIC_SUNAP sunap4  = new HIC_SUNAP();         //검진5
        #endregion

        #region Form 전역변수 
        List<HIC_BCODE> lstBlnfo = new List<HIC_BCODE>();    //채혈안내문
        List<HIC_BCODE> lstUrinfo = new List<HIC_BCODE>();   //특수소변안내문
        List<HIC_BCODE> lstHyang = new List<HIC_BCODE>();    //일반건진 내시경실에 향정약품 전송 설정값
        List<HIC_BCODE> lstJindan = new List<HIC_BCODE>();    //채혈안내문
        #endregion

        INIFile             INI          = null;
        clsHaBase           cHB          = null;
        clsHcMain           cHcMain      = null;
        clsHcFunc           cHF          = null;
        clsHcPrint          cHPrt        = null;
        clsHcMainFunc       cHMF         = null;
        clsSpread           cSpd         = null;
        clsHcMainSpd        cHMSpd       = null;
        clsHcOrderSend      cHOS         = null;

        #region Service Declare
        BasBcodeService             basBcodeService             = null;
        BasPatientService           basPatientService           = null;
        ComHpcLibBService           comHpcLibBService           = null;
        CardApprovCenterService     cardApprovCenterService     = null;
        HicWaitService              hicWaitService              = null;
        HeaExjongService            heaExjongService            = null;
        HeaGamcodeService           heaGamcodeService           = null;
        HeaJepsuService             heaJepsuService             = null;
        HicBcodeService             hicBcodeService             = null;
        HicCodeService              hicCodeService              = null;
        HicConsentService           hicConsentService           = null;
        HicCancerResv2Service       hicCancerResv2Service       = null;
        HicPatientService           hicPatientService           = null;
        HicExjongService            hicExjongService            = null;
        HicExcodeService            hicExcodeService            = null;
        HicGroupexamService         hicGroupexamService         = null;
        HicGroupcodeService         hicGroupcodeService         = null;
        HicGroupexamExcodeService   hicGroupexamExcodeService   = null;
        HicResultExCodeService      hicResultExCodeService      = null;
        HicLtdService               hicLtdService               = null;
        HicJepsuService             hicJepsuService             = null;
        HicResultService            hicResultService            = null;
        HicJepsuResultService       hicJepsuResultService       = null;
        HicJepsuWorkService         hicJepsuWorkService         = null;
        HicJepsuHeaExjongService    hicJepsuHeaExjongService    = null;
        HicIeMunjinNewService       hicIeMunjinNewService       = null;
        HicMunjinNightService       hicMunjinNightService       = null;
        HicResSpecialService        hicResSpecialService        = null;
        HicMemoService              hicMemoService              = null;
        HicSunapService             hicSunapService             = null;
        HicSunapWorkService         hicSunapWorkService         = null;
        HicSunapdtlService          hicSunapdtlService          = null;
        HicSunapdtlWorkService      hicSunapdtlWorkService      = null;
        WorkNhicService             workNhicService             = null;
        ReadSunapItemService        readSunapItemService        = null;
        HicXrayResultService        hicXrayResultService        = null;
        HicCharttransPrintService   hicCharttransPrintService   = null;
        GroupCodeExamDisplayService groupCodeExamDisplayService = null;
        HicGroupexamGroupcodeExcodeService hicGroupexamGroupcodeExcodeService = null;
        frmHcCharttrans_Insert FrmHcCharttrans_Insert = null;
        EndoJupmstService           endoJupmstService = null;
        HicCharttransService        hicCharttransService = null;
        HicSunapdtlGroupcodeService hicSunapdtlGroupcodeService = null;
        HicJinGbnService            hicJinGbnService = null;

        #endregion

        frmHcDaeSangView frmHJGaView = null;
        frmHcPermission frmHcPermission = null;
        frmHcEmrPermission frmHcEmrPermission = null;
        frmHcEmrConset_Rec frmHcEmrConset_Rec = null;

        public delegate void SetJepsuView(object sender, EventArgs e);
        public static event SetJepsuView rSetJepsuView;

        //public delegate void SetGaJepsuView(object sender, EventArgs e);
        //public static event SetGaJepsuView rSetGaJepsuView;

        private string FstrBuildNo = string.Empty;
        private string FstrPtno = string.Empty;
        private string FstrPtnoOld = string.Empty;
        private string FstrUCodes = string.Empty;
        private string FstrSExams = string.Empty;
        private string FstrJepsuGbn = string.Empty; 
        private string FstrJong = string.Empty;
        private string FstrEndo = string.Empty;
        private string FstrHyang = string.Empty;
        private string FstrRetValue = string.Empty;
        private string FstrChulPC = string.Empty;

        private string[] FstrBuRate = new string[6];
        private string[] FstrExamName = new string[12];
        private string[] FstrName = new string[12];

        private long FnPano = 0;
        private long FnWRTNO = 0;
        private long FnIEMunNo = 0;
        private bool FbHuilGasan = false;   //휴일여부
        private long GnWRTNO = 0;

        string FstrComCode;
        string FstrComName;

        string FstrD54 = "";
        string FstrD56 = "";
        string FstrD57 = "";

        #region 개인별 검진 History 연계
        string FsHisJepDate;
        string FsHisPtNo;
        string FsHisYear;
        #endregion

        public frmHcJepMain()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        private void SetControl()
        {
            #region Declare Variable Area
            INI     = new INIFile();
            cHB     = new clsHaBase();
            cHcMain = new clsHcMain();
            cHF     = new clsHcFunc();
            cHPrt   = new clsHcPrint();
            cHMF    = new clsHcMainFunc();
            cSpd    = new clsSpread();
            cHMSpd  = new clsHcMainSpd();
            cHOS    = new clsHcOrderSend();
            toolTip = new System.Windows.Forms.ToolTip();



            //toolTip.AutoPopDelay = 5000;
            //toolTip.InitialDelay = 1000;
            //toolTip.ReshowDelay = 500;
            //toolTip.ShowAlways = true;

            basBcodeService                     = new BasBcodeService();
            basPatientService                   = new BasPatientService();
            comHpcLibBService                   = new ComHpcLibBService();
            cardApprovCenterService             = new CardApprovCenterService();
            hicWaitService                      = new HicWaitService();
            hicBcodeService                     = new HicBcodeService();
            hicCodeService                      = new HicCodeService();
            hicConsentService                   = new HicConsentService();
            hicCancerResv2Service               = new HicCancerResv2Service();
            hicLtdService                       = new HicLtdService();
            hicExjongService                    = new HicExjongService();
            hicExcodeService                    = new HicExcodeService();
            hicGroupcodeService                 = new HicGroupcodeService();
            hicGroupexamService                 = new HicGroupexamService();
            hicGroupexamExcodeService           = new HicGroupexamExcodeService();
            hicResultExCodeService              = new HicResultExCodeService();
            hicPatientService                   = new HicPatientService();
            hicJepsuService                     = new HicJepsuService();
            hicResultService                    = new HicResultService();
            hicJepsuResultService               = new HicJepsuResultService();
            hicJepsuWorkService                 = new HicJepsuWorkService();
            hicJepsuHeaExjongService            = new HicJepsuHeaExjongService();
            hicIeMunjinNewService               = new HicIeMunjinNewService();
            hicMunjinNightService               = new HicMunjinNightService();
            hicMemoService                      = new HicMemoService();
            hicSunapService                     = new HicSunapService();
            hicSunapWorkService                 = new HicSunapWorkService();
            hicSunapdtlService                  = new HicSunapdtlService();
            hicSunapdtlWorkService              = new HicSunapdtlWorkService();
            hicResSpecialService                = new HicResSpecialService();
            heaGamcodeService                   = new HeaGamcodeService();
            heaExjongService                    = new HeaExjongService();
            heaJepsuService                     = new HeaJepsuService();
            workNhicService                     = new WorkNhicService();
            readSunapItemService                = new ReadSunapItemService();
            hicXrayResultService                = new HicXrayResultService();
            hicCharttransPrintService           = new HicCharttransPrintService();
            groupCodeExamDisplayService         = new GroupCodeExamDisplayService();
            hicGroupexamGroupcodeExcodeService  = new HicGroupexamGroupcodeExcodeService();
            endoJupmstService                   = new EndoJupmstService();
            hicCharttransService                = new HicCharttransService();
            hicSunapdtlGroupcodeService         = new HicSunapdtlGroupcodeService();
            hicJinGbnService                    = new HicJinGbnService();

            LtdHelpItem     = new HIC_LTD();
            CodeHelpItem    = new HIC_CODE();
            Hpatient        = new HIC_PATIENT();
            hJepsu          = new HIC_JEPSU();

            List<HIC_EXJONG> hcExjong = null;

            frmHJGaView = new frmHcDaeSangView();
            frmHcPermission = new frmHcPermission("HIC");
            frmHcEmrPermission = new frmHcEmrPermission("HIC");
            frmHcEmrConset_Rec = new frmHcEmrConset_Rec();
            #endregion

            //검진종류
            hcExjong = hicExjongService.GetItemList();
            cboJONG1.SetItems(hcExjong, "NAME", "CODE", "", "", AddComboBoxPosition.Top);
            cboJONG2.SetItems(hcExjong, "NAME", "CODE", "", "");
            cboJONG3.SetItems(hcExjong, "NAME", "CODE", "", "");
            cboJONG4.SetItems(hcExjong, "NAME", "CODE", "", "");
            cboJONG5.SetItems(hcExjong, "NAME", "CODE", "", "");

            //국가코드
            cHB.ComboNational_AddItem(cboNational);     

            //특검종류
            List<HIC_CODE> hcSpcGb = hicCodeService.FindOne("54");
            cboGbSpc.SetItems(hcSpcGb, "NAME", "CODE", "", "", AddComboBoxPosition.Top);    

            //성별
            cboSex.Items.Clear();
            cboSex.Items.Add("M");
            cboSex.Items.Add("F");
            cboSex.SelectedIndex = 0;

            //감액계정
            cboHalinGye.Items.Clear();
            cboHalinGye.Items.Add("");
            cboHalinGye.Items.Add("01.금액할인");
            cboHalinGye.Items.Add("02.종검중복할인");
            cboHalinGye.Items.Add("03.재단성직자 할인");
            cboHalinGye.Items.Add("04.본원채용검진");

            chkSMS.SetOptions(      new CheckBoxOption { DataField = nameof(HIC_PATIENT.GBSMS),        CheckValue = "Y", UnCheckValue = "N" });
            chkFall.SetOptions(     new CheckBoxOption { DataField = nameof(HIC_JEPSU.GBNAKSANG),      CheckValue = "Y", UnCheckValue = "N" });
            chkForeign.SetOptions(  new CheckBoxOption { DataField = nameof(HIC_PATIENT.GBFOREIGNER),  CheckValue = "Y", UnCheckValue = "N" });
            chkGubAm.SetOptions(    new CheckBoxOption { DataField = nameof(HIC_JEPSU.GUBDAESANG),     CheckValue = "Y", UnCheckValue = "" });
            chkHeaEndo.SetOptions(  new CheckBoxOption { DataField = nameof(HIC_JEPSU.GBHEAENDO),      CheckValue = "Y", UnCheckValue = "" });
            chkOHMS.SetOptions(     new CheckBoxOption { DataField = nameof(HIC_RES_SPECIAL.GBOHMS),   CheckValue = "Y", UnCheckValue = "N" });
            chkSuchup.SetOptions(   new CheckBoxOption { DataField = nameof(HIC_RES_SPECIAL.SUCHUPYN), CheckValue = "Y", UnCheckValue = "N" });
            chk69Pan1.SetOptions(   new CheckBoxOption { DataField = nameof(HIC_JEPSU.GBADDPAN),       CheckValue = "Y", UnCheckValue = "N" });
            chk69Pan2.SetOptions(   new CheckBoxOption { DataField = nameof(HIC_JEPSU.GBADDPAN),       CheckValue = "Y", UnCheckValue = "N" });
            chk69Pan3.SetOptions(   new CheckBoxOption { DataField = nameof(HIC_JEPSU.GBADDPAN),       CheckValue = "Y", UnCheckValue = "N" });
            chk69Pan4.SetOptions(   new CheckBoxOption { DataField = nameof(HIC_JEPSU.GBADDPAN),       CheckValue = "Y", UnCheckValue = "N" });
            chk69Pan5.SetOptions(   new CheckBoxOption { DataField = nameof(HIC_JEPSU.GBADDPAN),       CheckValue = "Y", UnCheckValue = "N" });

            #region 수검자 메모 Spread
            ssETC.Initialize();
            ssETC.AddColumn("삭제",       nameof(HIC_MEMO.GBDEL),     34, FpSpreadCellType.CheckBoxCellType);
            ssETC.AddColumn("구분",       nameof(HIC_MEMO.JOBGBN),    42, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = false });
            ssETC.AddColumn("입력시각",   nameof(HIC_MEMO.ENTTIME),  160, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left, BackColor = Color.FromArgb(192, 255, 192) });
            ssETC.AddColumn("내용",       nameof(HIC_MEMO.MEMO),     440, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsMulti = true });
            ssETC.AddColumn("작업자사번", nameof(HIC_MEMO.JOBSABUN),  48, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            ssETC.AddColumn("작업자명",   nameof(HIC_MEMO.JOBNAME),   90, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { });
            ssETC.AddColumn("CHANGE",     "",                         30, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            ssETC.AddColumn("ROWID",      nameof(HIC_MEMO.RID),       30, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            ssETC.AddColumn("PANO",       nameof(HIC_MEMO.PANO),      30, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            ssETC.AddColumn("PTNO",       nameof(HIC_MEMO.PTNO),      30, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            #endregion

            #region 그룹코드 정보
            ssGroup.Initialize(new SpreadOption { ColumnHeaderHeight = 34 });
            ssGroup.AddColumnCheckBox("제외", "", 28, new CheckBoxFlagEnumCellType<IsDeleted>() { IsHeaderCheckBox = false }).ButtonClick += frmJepMainGrpCD_ButtonClick;
            ssGroup.AddColumn("묶음코드",   nameof(READ_SUNAP_ITEM.GRPCODE),  60, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            ssGroup.AddColumn("묶음코드명", nameof(READ_SUNAP_ITEM.GRPNAME), 160, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            ssGroup.AddColumn("금액",       nameof(READ_SUNAP_ITEM.AMT),      70, FpSpreadCellType.NumberCellType,  new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Right });
            ssGroup.AddColumn("부담율",     nameof(READ_SUNAP_ITEM.GBSELF),   40, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { TextMaxLength = 2 });
            ssGroup.AddColumn("추가검사",   nameof(READ_SUNAP_ITEM.GBSELECT), 40, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsVisivle = false });
            ssGroup.AddColumnCheckBox("할인구분", "",                         32, new CheckBoxFlagEnumCellType<IsActive>() { }).ButtonClick += frmJepMainGrpCD_ButtonClick;
            ssGroup.AddColumn("취급물질",   nameof(READ_SUNAP_ITEM.UCODE),    48, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false });
            ssGroup.AddColumn("ROWID",      nameof(READ_SUNAP_ITEM.RID),      32, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsVisivle = false });

            #endregion

            #region 검진 검사항목 코드 Spread
            ssExam.Initialize();
            ssExam.AddColumn("제외",     "",                                              34, FpSpreadCellType.CheckBoxCellType);
            //ssExam.AddColumn("제외", nameof(GROUPCODE_EXAM_DISPLAY.GUBUN), 34, FpSpreadCellType.CheckBoxCellType);
            ssExam.AddColumn("묶음코드명", nameof(GROUPCODE_EXAM_DISPLAY.GROUPCODENAME), 160, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false, IsSort = true, Aligen = CellHorizontalAlignment.Left, BackColor = Color.FromArgb(192, 255, 192) });
            ssExam.AddColumn("검사코드",   nameof(GROUPCODE_EXAM_DISPLAY.EXCODE),         46, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false, IsSort = true, Aligen = CellHorizontalAlignment.Left });
            ssExam.AddColumn("금액",       nameof(GROUPCODE_EXAM_DISPLAY.AMT),            64, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Right });
            ssExam.AddColumn("검사명",     nameof(GROUPCODE_EXAM_DISPLAY.EXNAME),        138, FpSpreadCellType.TextCellType,   new SpreadCellTypeOption { IsEditble = false, IsSort = true, Aligen = CellHorizontalAlignment.Left });
            ssExam.AddColumn("묶음코드",     nameof(GROUPCODE_EXAM_DISPLAY.GROUPCODE), 138, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, IsSort = true, Aligen = CellHorizontalAlignment.Left });
            #endregion

            #region 이전수납기록
            SS_Sunap.Initialize();
            SS_Sunap.AddColumn("묶음코드",   nameof(READ_SUNAP_ITEM.GRPCODE),  60, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS_Sunap.AddColumn("묶음코드명", nameof(READ_SUNAP_ITEM.GRPNAME), 180, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });
            SS_Sunap.AddColumn("금액",       nameof(READ_SUNAP_ITEM.AMT),      70, FpSpreadCellType.NumberCellType,  new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Right });
            SS_Sunap.AddColumn("부담율",     nameof(READ_SUNAP_ITEM.GBSELF),   48, FpSpreadCellType.TextCellType,    new SpreadCellTypeOption { TextMaxLength = 2 });
            #endregion

            int nYY = int.Parse(VB.Left(DateTime.Now.ToShortDateString(), 4));

            for (int i = 0; i < 5; i++)
            {
                cboYear.Items.Add(string.Format("{0:0000}", nYY));
                nYY -= 1;
            }

            cboYear.SelectedIndex = 0;

            //부담율
            cboBuRate.SetOptions(new ComboBoxOption { DataField = nameof(HIC_GROUPCODE.GBSELF) });
            cboBuRate.SetItems(hicCodeService.FindOne("B4"), "NAME", "CODE", "", "");

            for (int i = 0; i < FstrBuRate.Length; i++)
            {
                FstrBuRate[i] = "00";
            }

            cboMuRyoAm.Items.Clear();
            cboMuRyoAm.Items.Add(" ");
            cboMuRyoAm.Items.Add("Y");
            cboMuRyoAm.Items.Add("N");

            //일반건진 내시경실에 향정약품 전송 설정값


            //여기를 풀면 무한루핑처럼 디버깅 멈춤
            //panMain.SetEnterKey();

            if(clsCompuInfo.gstrCOMIP =="192.168.2.202")
            {
                FstrChulPC = "Y";
            }

        }

        private void frmHcDeaSangView_ssDblClcik(HIC_JEPSU_WORK_PATIENT_HEA_JEPSU item)
        {
            if (item.IsNullOrEmpty())
            {
                return;
            }
            else
            {
                Screen_Clear();

                cboYear.Text = item.GJYEAR;
                txtPtno.Text = item.PTNO;

                txtPtno.Text = VB.Format(VB.Val(txtPtno.Text), "00000000");
                if (txtPtno.Text.Trim().Equals("00000000")) { txtPtno.Text = ""; return; }

                //검진 수검자 정보
                Display_HicPatient_Info(txtPtno.Text.Trim(), cboYear.Text);

                //검진 접수 Display Main
                Display_Jepsu_Main(txtPtno.Text, dtpJepDate.Value.ToShortDateString());
            }
        }

        private void eCancerResvList_DblClick(string strPtno, string strRemark, List<string> lstAmChk)
        {
            try
            {
                if (!strPtno.IsNullOrEmpty())
                {
                    Screen_Clear();
                    txtPtno.Text = strPtno;
                    dtpJepDate.Text = DateTime.Now.ToShortDateString();

                    //검진 수검자 정보
                    Display_HicPatient_Info(txtPtno.Text.Trim(), cboYear.Text);

                    txtRemark.Text = strRemark;

                    //검진 접수 Display Main
                    Display_Jepsu_Main(txtPtno.Text, dtpJepDate.Text, lstAmChk);
                }

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void eJochiList_DblClick(HIC_JEPSU_PATIENT item)
        {
            try
            {
                if (!item.IsNullOrEmpty())
                {
                    Screen_Clear();
                    txtPtno.Text = item.PTNO;
                    dtpJepDate.Text = item.JEPDATE;

                    //검진 수검자 정보
                    Display_HicPatient_Info(txtPtno.Text.Trim(), cboYear.Text);

                    //검진 접수 Display Main
                    Display_Jepsu_Main(txtPtno.Text, dtpJepDate.Text);
                }

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public void eJepList_DblClick(HIC_JEPSU item)
        {
            try
            {
                if (!item.IsNullOrEmpty())
                {
                    Screen_Clear();
                    txtPtno.Text = item.PTNO;
                    dtpJepDate.Text = item.JEPDATE;
                    cboYear.Text = item.GJYEAR;

                    //검진 수검자 정보
                    Display_HicPatient_Info(txtPtno.Text.Trim(), cboYear.Text);

                    //검진 접수 Display Main
                    Display_Jepsu_Main(txtPtno.Text, dtpJepDate.Text);
                }

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void frmJepMainGrpCD_ButtonClick(object sender, EditorNotifyEventArgs e)
        {
            if (e.Column == 0)
            {
                ssGroup.DeleteRow(e.Row);

                //저장된 검진정보에 ADD
                int nIdx = tbCtrl_Exam.SelectedTabIndex;

                if (clsHcType.THC[nIdx].GJJONG == "31")
                {
                    chkAm1.Checked = false; chkAm2.Checked = false; chkAm3.Checked = false; chkAm4.Checked = false;
                    chkAm5.Checked = false; chkAm6.Checked = false; chkAm7.Checked = false;
                    for (int i = 0; i < ssGroup.ActiveSheet.RowCount; i++)
                    {
                        if (ssGroup.ActiveSheet.Cells[i, 0].Text != "Y")
                        {
                            string strGbAm = hicGroupcodeService.GetGbAmByCode(ssGroup.ActiveSheet.Cells[i, 1].Text);
                            Display_GbAm2(strGbAm);
                        }
                    }
                }

                //부담율
                string strBurate = VB.Pstr(cboBuRate.Text, ".", 1).To<string>(FstrBuRate[nIdx]);

                Gesan_Sunap_Amt(nIdx, lstsuInfo(nIdx), strBurate, txtHalinAmt.Text.To<long>());
                GroupExams_Set(lstsuInfo(nIdx), nIdx);    //검사항목코드 재설정
                SetControl_Special(nIdx, lstsuInfo(nIdx), lstgrpExam(nIdx));    //컨트롤 자동세팅

            }
            else if (e.Column == 6)
            {
                long nHalinAmt = txtHalinAmt.Text.Replace(",", "").To<long>();
                
                if (ssGroup.ActiveSheet.Cells[e.Row, 6].Text == "Y")
                {
                    nHalinAmt += ssGroup.ActiveSheet.Cells[e.Row, 3].Text.Replace(",", "").To<long>();
                }
                else
                {
                    nHalinAmt -= ssGroup.ActiveSheet.Cells[e.Row, 3].Text.Replace(",", "").To<long>();
                }

                if (nHalinAmt < 0) { nHalinAmt = 0; }

                txtHalinAmt.Text = nHalinAmt.ToString("#,##0");

                btnAmt.PerformClick();
            }
            
        }

        private void GroupExams_Set(List<READ_SUNAP_ITEM> lstRSI, int nIdx)
        {
            List<GROUPCODE_EXAM_DISPLAY> lstExam = new List<GROUPCODE_EXAM_DISPLAY>();
            List<string> lstGrpCD = new List<string>();

            //암검진 진찰료 부담율 초기화
            for (int k = 0; k < lstRSI.Count; k++)
            {
                if (lstRSI[k].RowStatus != RowStatus.Delete)
                {
                    if (lstRSI[k].GRPCODE.To<string>("").Trim() == "3101")
                    {
                        lstRSI[k].GBSELF = "";
                        break;
                    }
                }
            }

            //분변, 자궁 검사 하나라도 있을 경우 진찰료는 01 공단부담으로
            for (int i = 0; i < lstRSI.Count; i++)
            {
                if (lstRSI[i].RowStatus != RowStatus.Delete)
                {
                    if (lstRSI[i].GRPCODE.To<string>("").Trim() == "3116" ||
                    lstRSI[i].GRPCODE.To<string>("").Trim() == "3132")
                    {
                        for (int k = 0; k < lstRSI.Count; k++)
                        {
                            if (lstRSI[k].GRPCODE.To<string>("").Trim() == "3101")
                            {
                                lstRSI[k].GBSELF = "01";

                                if (clsHcType.THNV.hJaGubun.To<string>("").Trim() == "의료급여") { lstRSI[k].GBSELF = "11"; }
                                break;
                            }
                        }
                    }
                } 
            }

            for (int i = 0; i < lstRSI.Count; i++)
            {
                if (lstRSI[i].RowStatus != RowStatus.Delete)
                {
                    //선택한 코드 적용
                    lstGrpCD.Add(lstRSI[i].GRPCODE);
                }
            }

            //lstExam = groupCodeExamDisplayService.GetListExcodeByGrpCodeList(lstGrpCD);           //TEST

            lstExam = groupCodeExamDisplayService.GetHicListByGroupCode(lstGrpCD);

            //기존 검사코드 비교 중복검사 제거(묶음코드 제외선택시 중복검사 제외)
            Remove_Overlap_ExamList2(ref lstExam);

            //부담율
            string strBurate = VB.Pstr(cboBuRate.Text, ".", 1).To<string>(FstrBuRate[nIdx]);
            //수납금액 계산하기
            Gesan_Sunap_Amt(nIdx, lstsuInfo(nIdx), strBurate, txtHalinAmt.Text.To<long>());

            switch (nIdx)
            {
                case 0:
                    grpExam0.Clear();
                    grpExam0 = lstExam; break;
                case 1:
                    grpExam1.Clear();
                    grpExam1 = lstExam; break;
                case 2:
                    grpExam2.Clear();
                    grpExam2 = lstExam; break;
                case 3:
                    grpExam3.Clear();
                    grpExam3 = lstExam; break;
                case 4:
                    grpExam4.Clear();
                    grpExam4 = lstExam; break;
                default:
                    break;
            }

            ssExam.DataSource = null;
            ssExam.SetDataSource(lstExam);
        }

        private List<READ_SUNAP_ITEM> lstsuInfo(int nIdx)
        {
            List<READ_SUNAP_ITEM> rtnLst = new List<READ_SUNAP_ITEM>();

            switch (nIdx)
            {
                case 0: rtnLst = suInfo0; break;
                case 1: rtnLst = suInfo1; break;
                case 2: rtnLst = suInfo2; break;
                case 3: rtnLst = suInfo3; break;
                case 4: rtnLst = suInfo4; break;
                default: break;
            }

            return rtnLst;
        }

        private List<READ_SUNAP_ITEM> lstoldSuInfo(int nIdx)
        {
            List<READ_SUNAP_ITEM> rtnLst = new List<READ_SUNAP_ITEM>();

            switch (nIdx)
            {
                case 0: rtnLst = oldSuInfo0; break;
                case 1: rtnLst = oldSuInfo1; break;
                case 2: rtnLst = oldSuInfo2; break;
                case 3: rtnLst = oldSuInfo3; break;
                case 4: rtnLst = oldSuInfo4; break;
                default: break;
            }

            return rtnLst;
        }

        private List<GROUPCODE_EXAM_DISPLAY> lstgrpExam(int nIdx)
        {
            List<GROUPCODE_EXAM_DISPLAY> rtnLst = new List<GROUPCODE_EXAM_DISPLAY>();

            switch (nIdx)
            {
                case 0: rtnLst = grpExam0; break;
                case 1: rtnLst = grpExam1; break;
                case 2: rtnLst = grpExam2; break;
                case 3: rtnLst = grpExam3; break;
                case 4: rtnLst = grpExam4; break;
                default: break;
            }

            return rtnLst;
        }

        private HIC_SUNAP dSunap(int nIdx)
        {
            HIC_SUNAP rtnItem = new HIC_SUNAP();

            switch (nIdx)
            {
                case 0: rtnItem = sunap0; break;
                case 1: rtnItem = sunap1; break;
                case 2: rtnItem = sunap2; break;
                case 3: rtnItem = sunap3; break;
                case 4: rtnItem = sunap4; break;
                default: break;
            }

            return rtnItem;
        }

        private void SetEvents()
        {
            this.Load                           += new EventHandler(eFormLoad);
            this.FormClosing                    += new FormClosingEventHandler(eDisevent);
            this.btnExit.Click                  += new EventHandler(eBtnClick);
            this.btnAmt.Click                   += new EventHandler(eBtnClick);
            this.btnCall.Click                  += new EventHandler(eBtnClick);
            this.btnReCall.Click                += new EventHandler(eBtnClick);
            this.btnMenualCall.Click            += new EventHandler(eBtnClick);
            this.btnSearch_Nhic.Click           += new EventHandler(eBtnClick);         //자격조회
            this.btnNhic_New.Click              += new EventHandler(eBtnClick);
            this.btnNhic_2020.Click             += new EventHandler(eBtnClick);     //2020년자격조회
            this.btnSearch_His.Click            += new EventHandler(eBtnClick);     //개인 History
            this.btnGroupCode_Spc.Click         += new EventHandler(eBtnClick);
            this.btnGroupCode_Sel.Click         += new EventHandler(eBtnClick);
            this.btnGroupCode_Ltd.Click         += new EventHandler(eBtnClick);
            this.btnHelp_Mail.Click             += new EventHandler(eBtnClick);
            this.btnHelp_Mail1.Click            += new EventHandler(eBtnClick);
            this.btnHelp_Mail2.Click            += new EventHandler(eBtnClick);
            this.btnHelp_Mail3.Click            += new EventHandler(eBtnClick);
            this.btnHelp_Mail4.Click            += new EventHandler(eBtnClick);
            this.btnHelp_Mail5.Click            += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click               += new EventHandler(eBtnClick);
            this.btnJikjong.Click               += new EventHandler(eBtnClick);
            this.btnGongjeng.Click              += new EventHandler(eBtnClick);
            this.btnCancel.Click                += new EventHandler(eBtnClick);
            this.btnCard.Click                  += new EventHandler(eBtnClick);
            this.btnCash.Click                  += new EventHandler(eBtnClick);
            this.btnLtdJuso.Click               += new EventHandler(eBalloonBtn);

            this.btnReset.Click                 += new EventHandler(eBtnClick);
            this.btnSave.Click                  += new EventHandler(eBtnClick);
            this.btnSave_Memo.Click             += new EventHandler(eBtnClick);         //수검자 메모저장
            this.btnJepView.Click               += new EventHandler(eJepsuView);
            this.btnGaJepList.Click             += new EventHandler(eGaJepView);
            this.btnJisa.Click                  += new EventHandler(eBtnClick);
            this.btnKiho.Click                  += new EventHandler(eBtnClick);
            this.btnBogenso.Click               += new EventHandler(eBtnClick);
            this.btnAgree.Click                 += new EventHandler(eBtnClick);
            this.btnLifeTool.Click              += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyDown             += new KeyEventHandler(eTxtKeyDown);
            this.txtJikJong.KeyDown             += new KeyEventHandler(eTxtKeyDown);
            this.txtGongjeng.KeyDown            += new KeyEventHandler(eTxtKeyDown);
            this.txtPtno.KeyDown                += new KeyEventHandler(eTxtKeyDown);
            //this.txtGWrtno.KeyDown              += new KeyEventHandler(eTxtKeyDown);
            this.txtJumin1.KeyDown              += new KeyEventHandler(eTxtKeyDown);
            this.txtJumin1.TextChanged          += new EventHandler(eTxtChanged);
            this.txtJumin2.KeyDown              += new KeyEventHandler(eTxtKeyDown);
            this.txtCardAmt.KeyDown             += new KeyEventHandler(eTxtKeyDown);
            this.txtCashAmt.KeyDown             += new KeyEventHandler(eTxtKeyDown);
            
            this.txtHeight.KeyPress             += new KeyPressEventHandler(eKeyPress);
            this.txtWeight.KeyPress             += new KeyPressEventHandler(eKeyPress);

            this.cboJONG1.KeyDown               += new KeyEventHandler(eCboKeyDown);
            this.cboJONG2.KeyDown               += new KeyEventHandler(eCboKeyDown);
            this.cboJONG3.KeyDown               += new KeyEventHandler(eCboKeyDown);
            this.cboJONG4.KeyDown               += new KeyEventHandler(eCboKeyDown);
            this.cboJONG5.KeyDown               += new KeyEventHandler(eCboKeyDown);

            this.rdoMemo1.CheckedChanged        += new EventHandler(rdoChkChange);
            this.rdoMemo2.CheckedChanged        += new EventHandler(rdoChkChange);
            this.rdoMemo3.CheckedChanged        += new EventHandler(rdoChkChange);

            this.ssETC.EditModeOff              += new EventHandler(eSpdEditOff);
            this.ssETC.ButtonClicked            += new EditorNotifyEventHandler(eSpdBtnClick);
            this.ssGroup.EditModeOff            += new EventHandler(eSpdEditOff);

            this.tbCtrl_Exam.SelectedTabChanging += new EventHandler<SuperTabStripSelectedTabChangingEventArgs>(eTabChanging);
            this.tbCtrl_Exam.SelectedTabChanged  += new EventHandler<SuperTabStripSelectedTabChangedEventArgs>(eTabChanged);
            this.tbCtrl_JONG.SelectedTabChanged  += new EventHandler<SuperTabStripSelectedTabChangedEventArgs>(eTabChanged);

            //신규 Tab 생성
            this.tbJongPage99.Click             += new EventHandler(eTabPageClick);

            this.btnWaitReg.Click               += new EventHandler(eBtnClick);

            this.btnJongClose01.Click           += new EventHandler(eBtnClick);
            this.btnJongClose02.Click           += new EventHandler(eBtnClick);
            this.btnJongClose03.Click           += new EventHandler(eBtnClick);
            this.btnJongClose04.Click           += new EventHandler(eBtnClick);
            this.btnJongClose05.Click           += new EventHandler(eBtnClick);
            
            this.chkDel1.CheckedChanged         += new EventHandler(chkChanged);
            this.chkDel2.CheckedChanged         += new EventHandler(chkChanged);
            this.chkDel3.CheckedChanged         += new EventHandler(chkChanged);
            this.chkDel4.CheckedChanged         += new EventHandler(chkChanged);
            this.chkDel5.CheckedChanged         += new EventHandler(chkChanged);

            this.chkRES12_1.CheckedChanged      += new EventHandler(chkChanged);    //검진1 결과지 집/회사 선택
            this.chkRES12_2.CheckedChanged      += new EventHandler(chkChanged);    //검진1 결과지 집/회사 선택
            this.chkRES12_3.CheckedChanged      += new EventHandler(chkChanged);    //검진1 결과지 별도주소 선택
            this.chkRES22_1.CheckedChanged      += new EventHandler(chkChanged);    //검진2 결과지 집/회사 선택
            this.chkRES22_2.CheckedChanged      += new EventHandler(chkChanged);    //검진2 결과지 집/회사 선택
            this.chkRES22_3.CheckedChanged      += new EventHandler(chkChanged);    //검진1 결과지 별도주소 선택
            this.chkRES32_1.CheckedChanged      += new EventHandler(chkChanged);    //검진3 결과지 집/회사 선택
            this.chkRES32_2.CheckedChanged      += new EventHandler(chkChanged);    //검진3 결과지 집/회사 선택
            this.chkRES32_3.CheckedChanged      += new EventHandler(chkChanged);    //검진1 결과지 별도주소 선택
            this.chkRES42_1.CheckedChanged      += new EventHandler(chkChanged);    //검진4 결과지 집/회사 선택
            this.chkRES42_2.CheckedChanged      += new EventHandler(chkChanged);    //검진4 결과지 집/회사 선택
            this.chkRES42_3.CheckedChanged      += new EventHandler(chkChanged);    //검진1 결과지 별도주소 선택
            this.chkRES52_1.CheckedChanged      += new EventHandler(chkChanged);    //검진5 결과지 집/회사 선택
            this.chkRES52_2.CheckedChanged      += new EventHandler(chkChanged);    //검진5 결과지 집/회사 선택
            this.chkRES52_3.CheckedChanged      += new EventHandler(chkChanged);    //검진1 결과지 별도주소 선택

            this.rdoRES14.CheckedChanged += new EventHandler(eRdoCheckChanged);    //검진1 결과지 알림톡 선택 
            this.rdoRES15.CheckedChanged += new EventHandler(eRdoCheckChanged);    //검진1 결과지 방문수령 선택
            this.rdoRES24.CheckedChanged += new EventHandler(eRdoCheckChanged);    //검진2 결과지 알림톡 선택 
            this.rdoRES25.CheckedChanged += new EventHandler(eRdoCheckChanged);    //검진2 결과지 방문수령 선택
            this.rdoRES34.CheckedChanged += new EventHandler(eRdoCheckChanged);    //검진3 결과지 알림톡 선택 
            this.rdoRES35.CheckedChanged += new EventHandler(eRdoCheckChanged);    //검진3 결과지 방문수령 선택
            this.rdoRES44.CheckedChanged += new EventHandler(eRdoCheckChanged);    //검진4 결과지 알림톡 선택 
            this.rdoRES45.CheckedChanged += new EventHandler(eRdoCheckChanged);    //검진4 결과지 방문수령 선택
            this.rdoRES54.CheckedChanged += new EventHandler(eRdoCheckChanged);    //검진5 결과지 알림톡 선택 
            this.rdoRES55.CheckedChanged += new EventHandler(eRdoCheckChanged);    //검진5 결과지 방문수령 선택


            this.chkInfoAgr.CheckedChanged      += new EventHandler(chkChanged);
            this.chkPrvAgr.CheckedChanged       += new EventHandler(chkChanged);
            this.chkPrt9.CheckedChanged         += new EventHandler(chkChanged);

            this.btnXConfirm.Click              += new EventHandler(eBtnClick);
            this.btnXBarCode.Click              += new EventHandler(eBtnClick);
            this.btnXJepsu.Click                += new EventHandler(eBtnClick);
            this.btnXJepsu_Add.Click            += new EventHandler(eBtnClick);
            this.btnXRecReport.Click            += new EventHandler(eBtnClick);
            this.lblIEMunjin.DoubleClick        += new EventHandler(eLblDblClick);
            this.timer1.Tick                    += new EventHandler(eTimerTick);

            this.lblUCODES.MouseClick += new MouseEventHandler(eLblMouseClick);

            this.txtJuso11.MouseUp += new MouseEventHandler(elblMouseUp);
            this.txtJuso12.MouseUp += new MouseEventHandler(elblMouseUp);
            this.txtJuso13.MouseUp += new MouseEventHandler(elblMouseUp);
            this.txtJuso14.MouseUp += new MouseEventHandler(elblMouseUp);
            this.txtJuso15.MouseUp += new MouseEventHandler(elblMouseUp);


            frmHcDaeSangView.rSetGstrValue          += new frmHcDaeSangView.SetGstrValue(frmHcDeaSangView_ssDblClcik);
            frmHcJepsuView.rSetGstrValue            += new frmHcJepsuView.SetGstrValue(eJepList_DblClick);
            frmHcJochiList.rSetJochiGstrValue       += new frmHcJochiList.SetJochiGstrValue(eJochiList_DblClick);
            frmHcAmReserve.rSetCancerGstrValue      += new frmHcAmReserve.SetCancerGstrValue(eCancerResvList_DblClick);
            frmHcPanPersonResult.rSetJepsuGstrValue += new frmHcPanPersonResult.SetJepsuGstrValue(PatHis_Value);
            frmHcPanPersonResult.rSetJepsuBtnRef    += new frmHcPanPersonResult.SetJepsuBtnRef(ePtno_Event);
            frmMirErrorList.rSetJepsuGstrValue += new frmMirErrorList.SetJepsuGstrValue(PatHis_Value);
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

        private void ePtno_Event(string strPtNo)
        {
            try
            {
                if (!strPtNo.IsNullOrEmpty())
                {
                    Screen_Clear();
                    dtpJepDate.Text = DateTime.Now.ToShortDateString();
                    cboYear.Text = DateTime.Now.Year.To<string>("");
                    txtPtno.Text = strPtNo;

                    eTxtKeyDown(txtPtno, new KeyEventArgs(Keys.Enter));

                }

                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
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
            if (FnIEMunNo == 0) { return; }

            //frmHcIEMunjinVIew frm = new frmHcIEMunjinVIew(FnIEMunNo, "", txtPtno.Text);
            frmHcIEMunjin frm = new frmHcIEMunjin("", txtSName.Text);
            frm.ShowDialog();

        }

        private void eCboKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string strJong = string.Empty;
                int nIdx = tbCtrl_JONG.SelectedTabIndex;

                switch (nIdx)
                {
                    case 0: strJong = VB.Pstr(cboJONG1.Text, ".", 1).Trim();
                        break;
                    case 1: strJong = VB.Pstr(cboJONG2.Text, ".", 1).Trim();
                        break;
                    case 2: strJong = VB.Pstr(cboJONG3.Text, ".", 1).Trim();
                        break;
                    case 3: strJong = VB.Pstr(cboJONG4.Text, ".", 1).Trim();
                        break;
                    case 4: strJong = VB.Pstr(cboJONG5.Text, ".", 1).Trim();
                        break;
                    default: break;
                }

                FstrJong = strJong;
                tbCtrl_JONG.Tabs[nIdx].Text = strJong;
                tbCtrl_Exam.Tabs[nIdx].Text = strJong;
                //검진종류 기본부담율
                FstrBuRate[nIdx] = hicExjongService.GetBuRateByGjJong(strJong).PadLeft(2, '0');

                if (strJong == "56"|| strJong == "59")
                {
                    panDockingYN(panJONG, panUCODES, false);
                    panDockingYN(panJONG, panSchool, true);
                }
                else if (string.Compare(strJong, "20") > 0 && string.Compare(strJong, "31") < 0)
                {
                    panDockingYN(panJONG, panUCODES, true);
                    panDockingYN(panJONG, panSchool, false);
                }

                Clear_Working_Variants(nIdx);
                
                clsHcType.THC[nIdx].GJJONG = strJong;

                switch (nIdx)
                {
                    case 0:
                        rdoSTS1B.Checked = true;        //접수구분 접수로 기본세팅
                        chkRES12_1.Checked = true;      //결과지 집으로 기본 세팅
                        suInfo0.AddRange(Read_Jong_GroupCode(strJong, dtpJepDate.Text));
                        grpExam0.AddRange(Read_ExamCode_Work(suInfo0));
                        SetControl_Special(nIdx, suInfo0, grpExam0);    //컨트롤 자동세팅
                        break;
                    case 1:
                        rdoSTS2B.Checked = true;
                        chkRES22_1.Checked = true;
                        suInfo1.AddRange(Read_Jong_GroupCode(strJong, dtpJepDate.Text));
                        grpExam1.AddRange(Read_ExamCode_Work(suInfo1));
                        SetControl_Special(nIdx, suInfo1, grpExam1);    //컨트롤 자동세팅
                        break;
                    case 2:
                        rdoSTS3B.Checked = true;
                        chkRES32_1.Checked = true;
                        suInfo2.AddRange(Read_Jong_GroupCode(strJong, dtpJepDate.Text));
                        grpExam2.AddRange(Read_ExamCode_Work(suInfo2));
                        SetControl_Special(nIdx, suInfo2, grpExam2);    //컨트롤 자동세팅
                        break;
                    case 3:
                        rdoSTS4B.Checked = true;
                        chkRES42_1.Checked = true;
                        suInfo3.AddRange(Read_Jong_GroupCode(strJong, dtpJepDate.Text));
                        grpExam3.AddRange(Read_ExamCode_Work(suInfo3));
                        SetControl_Special(nIdx, suInfo3, grpExam3);    //컨트롤 자동세팅
                        break;
                    case 4:
                        rdoSTS5B.Checked = true;
                        chkRES52_1.Checked = true;
                        suInfo4.AddRange(Read_Jong_GroupCode(strJong, dtpJepDate.Text));
                        grpExam4.AddRange(Read_ExamCode_Work(suInfo4));
                        SetControl_Special(nIdx, suInfo4, grpExam4);    //컨트롤 자동세팅
                        break;
                    default: break;
                }

                Clear_Spread_GroupCode();

                ssGroup.SetDataSource(lstsuInfo(nIdx));
                ssExam.SetDataSource(lstgrpExam(nIdx));

                if (!chkNoNhic.Checked)
                {
                    //자격조회 여부
                    string strGbNhic = hicExjongService.GetGbNhicByExJong(strJong);

                    //자격조회
                    if (!txtSName.Text.IsNullOrEmpty() && strGbNhic == "Y")
                    {
                        //Clear_Nhic_List();
                        Hic_Chk_Nhic("H", txtSName.Text, txtJumin1.Text + txtJumin2.Text, txtPtno.Text, cboYear.Text, nIdx);
                    }
                }

                //자격조회 점검 알림 메세지
                if (clsHcType.THC[nIdx].GJJONG == "11")
                {
                    if (!clsHcType.THNV.h1Cha.IsNullOrEmpty() && clsHcType.THNV.h1Cha.Contains("비대상"))
                    {
                        MessageBox.Show("1차검진 비대상자 입니다. 검진자격을 확인하세요.", "접수불가");
                    }

                    if (!clsHcType.THNV.h1ChaDate.IsNullOrEmpty())
                    {
                        MessageBox.Show("1차검진을 이미 실시하였습니다. 검진내역을 확인하세요.", "접수불가");
                    }
                }
                else if (clsHcType.THC[nIdx].GJJONG == "16")
                {
                    if (!clsHcType.THNV.h2Cha.IsNullOrEmpty() && clsHcType.THNV.h2Cha.Contains("비대상"))
                    {
                        MessageBox.Show("2차검진 비대상자 입니다. 검진자격을 확인하세요.", "접수불가");
                    }

                    if (!clsHcType.THNV.h2ChaDate.IsNullOrEmpty())
                    {
                        MessageBox.Show("2차검진을 이미 실시하였습니다. 검진내역을 확인하세요.", "접수불가");
                    }
                }

                //암검진 초기 부담율 세팅
                //분변, 자궁 검사 하나라도 있을 경우 진찰료는 01 공단부담으로...
                READ_SUNAP_ITEM sRSI = new READ_SUNAP_ITEM();

                for (int i = 0; i < lstsuInfo(nIdx).Count; i++)
                {
                    sRSI = lstsuInfo(nIdx)[i];

                    Cancer_GroupCode_BuRate_Set(nIdx, ref sRSI);

                    if (lstsuInfo(nIdx)[i].GRPCODE.To<string>("").Trim() == "3116" ||
                        lstsuInfo(nIdx)[i].GRPCODE.To<string>("").Trim() == "3132")
                    {
                        for (int j = 0; j < lstsuInfo(nIdx).Count; j++)
                        {
                            if (lstsuInfo(nIdx)[j].GRPCODE.To<string>("").Trim() == "3101")
                            {
                                lstsuInfo(nIdx)[j].GBSELF = "01";
                                if (clsHcType.THNV.hJaGubun.To<string>("").Trim() == "의료급여") { lstsuInfo(nIdx)[j].GBSELF = "11"; }
                                break;
                            }
                        }
                    }
                }

                //자격에 따른 부담율 세부 설정 
                if (strJong == "31")
                {
                    //자궁경부암 검사만 있을경우 부담율 조합100% 세팅
                    if (chkAm3.Checked || chkAm6.Checked)
                    {
                        if (!chkAm1.Checked && !chkAm2.Checked && !chkAm4.Checked && !chkAm5.Checked && !chkAm7.Checked)
                        {
                            FstrBuRate[nIdx] = "01";
                            cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[nIdx].PadLeft(2, '0'), 0);
                        }
                    }

                    //보건소암 있을경우 무료암대상으로 표시
                    if (clsHcType.THNV.hBogen.To<string>("").Trim() != "")
                    {
                        if (VB.Left(clsHcType.THNV.hGKiho, 1) == "9")
                        {
                            FstrBuRate[nIdx] = "11";    //보건소 100%
                            cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[nIdx].PadLeft(2, '0'), 0);
                            chkGubAm.Checked = true;
                            cboMuRyoAm.SelectedIndex = 1;  //N
                        }
                        else
                        {
                            FstrBuRate[nIdx] = "12";    //조합90%,보건소10%
                            cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[nIdx].PadLeft(2, '0'), 0);
                            cboMuRyoAm.SelectedIndex = 1;   //Y
                        }
                    }
                }

                //휴일가산
                if (FbHuilGasan)
                {
                    List<string> lstCodes = new List<string>();

                    if (strJong == "11")
                    {
                        lstCodes.Add("1116");
                        lstCodes.Add("1118");
                    }
                    else if (strJong == "16")
                    {
                        lstCodes.Add("1117");
                    }
                    else if (strJong == "31")
                    {
                        lstCodes.Add("1119");
                    }

                    frmHcGroupCode_ssDblclick(lstCodes, "");
                }

                cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[nIdx].PadLeft(2, '0'), 0);
                
                Clear_Spread_GroupCode();

                ssGroup.SetDataSource(lstsuInfo(nIdx));
                ssExam.SetDataSource(lstgrpExam(nIdx));

                Gesan_Sunap_Amt(nIdx, lstsuInfo(nIdx), FstrBuRate[nIdx], 0);

                Application.DoEvents();
            }
        }

        public void eDisevent(object sender, FormClosingEventArgs e)
        {
            frmHcDaeSangView.rSetGstrValue -= new frmHcDaeSangView.SetGstrValue(frmHcDeaSangView_ssDblClcik);
            frmHcJepsuView.rSetGstrValue -= new frmHcJepsuView.SetGstrValue(eJepList_DblClick);
            frmHcJochiList.rSetJochiGstrValue -= new frmHcJochiList.SetJochiGstrValue(eJochiList_DblClick);
            frmHcAmReserve.rSetCancerGstrValue -= new frmHcAmReserve.SetCancerGstrValue(eCancerResvList_DblClick);
            frmHcPanPersonResult.rSetJepsuGstrValue -= new frmHcPanPersonResult.SetJepsuGstrValue(PatHis_Value);
            frmHcPanPersonResult.rSetJepsuBtnRef -= new frmHcPanPersonResult.SetJepsuBtnRef(ePtno_Event);
            this.btnJepView.Click -= new EventHandler(eJepsuView);
        }

        private void eGaJepView(object sender, EventArgs e)
        {
            if (frmHJGaView == null)
            {
                frmHcDaeSangView frmHJGaView = new frmHcDaeSangView();
                frmHJGaView.StartPosition = FormStartPosition.CenterScreen;
                frmHJGaView.ShowDialog();
                cHF.fn_ClearMemory(frmHJGaView);
            }
            else
            {
                FormVisiable(frmHJGaView);
            }
        }

        private void eJepsuView(object sender, EventArgs e)
        {
            rSetJepsuView(sender, e);
        }

        private void FormVisiable(Form frm)
        {
            frm.Visible = false;
            frm.Visible = true;
            frm.BringToFront();
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

        private void rdoChkChange(object sender, EventArgs e)
        {
            if (txtPtno.Text.Trim() == "")
            {
                return;
            }

            Hic_Memo_Screen(txtPtno.Text);
        }

        private void eTimerTick(object sender, EventArgs e)
        {
            int nRow = 0;
            long nSeqNo = 0;
            string strYeyak = "";
            

            ssWait.ActiveSheet.ClearRange(0, 0, ssWait.ActiveSheet.Rows.Count, ssWait.ActiveSheet.ColumnCount, true);
            ssWait.ActiveSheet.Rows.Count = 0;

            btnMenualCall.Text = "수동Call";
            btnMenualCall.ForeColor = Color.Black;

            List<HIC_WAIT> lHW = hicWaitService.GetItembyJobDate("2");

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
                    //btnMenualCall.Text = "암예약자";
                    btnMenualCall.ForeColor = Color.DarkRed;
                }
            }

            //종검 대기자가 있는지 점검
            if (hicWaitService.GetCountbyJobDate("1") > 0)
            {
                btnMenualCall.Text = "종검대기";
                btnMenualCall.ForeColor = Color.DarkRed;
            }


            HIC_WAIT item = hicWaitService.GetItembyJobDate2(clsPublic.GstrSysDate, "1");

            lblCallWait.Text = lHW.Count.To<string>() + " 명";
            lblCallWaitHea.Text = item.CNT + " 명";
        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtHeight || sender == txtWeight)
            {
                if (VB.Left(cHcMain.CALC_Biman_Rate(txtWeight.Text.To<double>(0), txtHeight.Text.To<double>(0), cboSex.Text.Trim()), 1) == "1")
                {
                    lblBMI.Text = "정상";
                }
                else
                {
                    lblBMI.Text = "비만";
                }
            }
        }

        private void chkChanged(object sender, EventArgs e)
        {
            if (sender == chkDel1)
            {
                clsHcType.THC[0].GBUSE = chkDel1.Checked ? "D" : clsHcType.THC[0].GBUSE;
            }
            else if (sender == chkDel2)
            {
                clsHcType.THC[1].GBUSE = chkDel2.Checked ? "D" : clsHcType.THC[1].GBUSE;
            }
            else if (sender == chkDel3)
            {
                clsHcType.THC[2].GBUSE = chkDel3.Checked ? "D" : clsHcType.THC[2].GBUSE;
            }
            else if (sender == chkDel4)
            {
                clsHcType.THC[3].GBUSE = chkDel4.Checked ? "D" : clsHcType.THC[3].GBUSE;
            }
            else if (sender == chkDel5)
            {
                clsHcType.THC[4].GBUSE = chkDel5.Checked ? "D" : clsHcType.THC[4].GBUSE;
            }
            else if (sender == chkInfoAgr)
            {
                if (chkInfoAgr.Checked)
                {
                    if (lblInfoAgr.Text.Trim() == "") { lblInfoAgr.Text = DateTime.Now.ToShortDateString(); }
                }
                else
                {
                    lblInfoAgr.Text = "";
                }
            }
            else if (sender == chkPrvAgr)
            {
                if (chkPrvAgr.Checked)
                {
                    if (lblPrvAgr.Text.Trim() == "") { lblPrvAgr.Text = DateTime.Now.ToShortDateString(); }
                }
                else
                {
                    lblPrvAgr.Text = "";
                }
            }

            else if (sender == chkRES12_1) { if (chkRES12_1.Checked) { rdoRES12.Checked = true; chkRES12_2.Checked = false; chkRES12_3.Checked = false; } }
            else if (sender == chkRES12_2) { if (chkRES12_2.Checked) { rdoRES12.Checked = true; chkRES12_1.Checked = false; chkRES12_3.Checked = false; } }
            else if (sender == chkRES12_3) { if (chkRES12_3.Checked) { rdoRES12.Checked = true; chkRES12_1.Checked = false; chkRES12_2.Checked = false; } }
            else if (sender == chkRES22_1) { if (chkRES22_1.Checked) { rdoRES22.Checked = true; chkRES22_2.Checked = false; chkRES22_3.Checked = false; } }
            else if (sender == chkRES22_2) { if (chkRES22_2.Checked) { rdoRES22.Checked = true; chkRES22_1.Checked = false; chkRES22_3.Checked = false; } }
            else if (sender == chkRES22_3) { if (chkRES22_3.Checked) { rdoRES22.Checked = true; chkRES22_1.Checked = false; chkRES22_2.Checked = false; } }
            else if (sender == chkRES32_1) { if (chkRES32_1.Checked) { rdoRES32.Checked = true; chkRES32_2.Checked = false; chkRES32_3.Checked = false; } }
            else if (sender == chkRES32_2) { if (chkRES32_2.Checked) { rdoRES32.Checked = true; chkRES32_1.Checked = false; chkRES32_3.Checked = false; } }
            else if (sender == chkRES32_3) { if (chkRES32_3.Checked) { rdoRES32.Checked = true; chkRES32_1.Checked = false; chkRES32_2.Checked = false; } }
            else if (sender == chkRES42_1) { if (chkRES42_1.Checked) { rdoRES42.Checked = true; chkRES42_2.Checked = false; chkRES42_3.Checked = false; } }
            else if (sender == chkRES42_2) { if (chkRES42_2.Checked) { rdoRES42.Checked = true; chkRES42_1.Checked = false; chkRES42_3.Checked = false; } }
            else if (sender == chkRES42_3) { if (chkRES42_3.Checked) { rdoRES42.Checked = true; chkRES42_1.Checked = false; chkRES42_2.Checked = false; } }
            else if (sender == chkRES52_1) { if (chkRES52_1.Checked) { rdoRES52.Checked = true; chkRES52_2.Checked = false; chkRES52_3.Checked = false; } }
            else if (sender == chkRES52_2) { if (chkRES52_2.Checked) { rdoRES52.Checked = true; chkRES52_1.Checked = false; chkRES52_3.Checked = false; } }
            else if (sender == chkRES52_3) { if (chkRES52_3.Checked) { rdoRES52.Checked = true; chkRES52_1.Checked = false; chkRES52_2.Checked = false; } }

            else if (sender == chkPrt9)
            {
                if (chkPrt9.Checked)
                {
                    FstrName[8] = chkPrt9.Text + "^^";
                }
                else
                {
                    FstrName[8] = "";
                }
            }
        }

        private void eRdoCheckChanged(object sender, EventArgs e)
        {

            if (sender == rdoRES12) { if (rdoRES12.Checked) { chkRES12_1.Checked = true; } }
            else if (sender == rdoRES14) { if (rdoRES14.Checked) { chkRES12_1.Checked = false; chkRES12_2.Checked = false; chkRES12_3.Checked = false; } }
            else if (sender == rdoRES15) { if (rdoRES15.Checked) { chkRES12_1.Checked = false; chkRES12_2.Checked = false; chkRES12_3.Checked = false; } }

            else if (sender == rdoRES22) { if (rdoRES22.Checked) { chkRES22_1.Checked = true; } }
            else if (sender == rdoRES24) { if (rdoRES24.Checked) { chkRES22_1.Checked = false; chkRES22_2.Checked = false; chkRES22_3.Checked = false; } }
            else if (sender == rdoRES25) { if (rdoRES25.Checked) { chkRES22_1.Checked = false; chkRES22_2.Checked = false; chkRES22_3.Checked = false; } }

            else if (sender == rdoRES32) { if (rdoRES32.Checked) { chkRES32_1.Checked = true; } }
            else if (sender == rdoRES34) { if (rdoRES34.Checked) { chkRES32_1.Checked = false; chkRES32_2.Checked = false; chkRES32_3.Checked = false; } }
            else if (sender == rdoRES35) { if (rdoRES35.Checked) { chkRES32_1.Checked = false; chkRES32_2.Checked = false; chkRES32_3.Checked = false; } }

            else if (sender == rdoRES42) { if (rdoRES42.Checked) { chkRES42_1.Checked = true; } }
            else if (sender == rdoRES44) { if (rdoRES44.Checked) { chkRES42_1.Checked = false; chkRES42_2.Checked = false; chkRES42_3.Checked = false; } }
            else if (sender == rdoRES45) { if (rdoRES45.Checked) { chkRES42_1.Checked = false; chkRES42_2.Checked = false; chkRES42_3.Checked = false; } }

            else if (sender == rdoRES52) { if (rdoRES52.Checked) { chkRES52_1.Checked = true; } }
            else if (sender == rdoRES54) { if (rdoRES54.Checked) { chkRES52_1.Checked = false; chkRES52_2.Checked = false; chkRES52_3.Checked = false; } }
            else if (sender == rdoRES55) { if (rdoRES55.Checked) { chkRES52_1.Checked = false; chkRES52_2.Checked = false; chkRES52_3.Checked = false; } }
        }
        private void eTabPageClick(object sender, EventArgs e)
        {
            if (sender == tbJongPage99)
            {
                for (int i = 0; i < tbCtrl_JONG.Tabs.Count; i++)
                {
                    if (tbCtrl_JONG.Tabs[i].Visible == false)
                    {
                        clsHcType.THC[i].GBUSE = "";
                        tbCtrl_JONG.Tabs[i].Visible = true;
                        tbCtrl_JONG.Tabs[i].Text = "";
                        tbCtrl_JONG.SelectedTabIndex = i;
                        tbCtrl_Exam.Tabs[i].Visible = true;
                        tbCtrl_Exam.Tabs[i].Text = "";
                        tbCtrl_Exam.SelectedTabIndex = i;
                        return;
                    }
                }

                tbCtrl_JONG.SelectedTabIndex = 0;
                tbCtrl_Exam.SelectedTabIndex = 0;
            }
        }

        /// <summary>
        /// 신규접수시 선택한 검진종류의 기본 세팅 그룹코드 List
        /// </summary>
        /// <param name="argJong"></param>
        /// <param name="argGbn"></param>
        /// <param name="argDate"></param>
        /// <returns></returns>
        private List<READ_SUNAP_ITEM> Read_Jong_GroupCode(string argJong, string argDate)
        {
            string strCode = string.Empty;
            List<READ_SUNAP_ITEM> lst = new List<READ_SUNAP_ITEM>();
            List<GROUPCODE_EXAM_DISPLAY> exm = new List<GROUPCODE_EXAM_DISPLAY>();
            int nIdx = tbCtrl_JONG.SelectedTabIndex;

            lst = readSunapItemService.GetListHicGrpCodeByJong(argJong, argDate);

            for (int i = 0; i < lst.Count; i++)
            {
                strCode = lst[i].GRPCODE.Trim();
                //선택 코드 금액 산정
                lst[i].AMT = Read_GrpCode_Amt(exm, strCode, argDate, null);
            }

            return lst;
        }

        /// <summary>
        /// 수납작업중인 정보를 객체에 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eTabChanging(object sender, SuperTabStripSelectedTabChangingEventArgs e)
        {
            if (sender == tbCtrl_Exam)
            {
                if (tbCtrl_Exam.SelectedTab == tbExamPage1)
                {
                    suInfo0 = (List<READ_SUNAP_ITEM>)ssGroup.DataSource;
                }
                else if (tbCtrl_Exam.SelectedTab == tbExamPage2)
                {
                    suInfo1 = (List<READ_SUNAP_ITEM>)ssGroup.DataSource;
                }
                else if (tbCtrl_Exam.SelectedTab == tbExamPage3)
                {
                    suInfo2 = (List<READ_SUNAP_ITEM>)ssGroup.DataSource;
                }
                else if (tbCtrl_Exam.SelectedTab == tbExamPage4)
                {
                    suInfo3 = (List<READ_SUNAP_ITEM>)ssGroup.DataSource;
                }
                else if (tbCtrl_Exam.SelectedTab == tbExamPage5)
                {
                    suInfo4 = (List<READ_SUNAP_ITEM>)ssGroup.DataSource;
                }
            }
        }

        /// <summary>
        /// 검사정보 Tab Change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eTabChanged(object sender, SuperTabStripSelectedTabChangedEventArgs e)
        {
            if (sender == tbCtrl_Exam)
            {
                if (tbCtrl_Exam.SelectedTab == tbExamPage1)
                {
                    tbCtrl_JONG.SelectedTabIndex = ((SuperTabControl)sender).SelectedTabIndex;
                    FstrJong = VB.Pstr(cboJONG1.Text, ".", 1).Trim();
                    cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[0].PadLeft(2, '0'), 0);
                    Display_Sunap_Amt(sunap0);
                }
                else if (tbCtrl_Exam.SelectedTab == tbExamPage2)
                {
                    tbCtrl_JONG.SelectedTabIndex = ((SuperTabControl)sender).SelectedTabIndex;
                    FstrJong = VB.Pstr(cboJONG2.Text, ".", 1).Trim();
                    cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[1].PadLeft(2, '0'), 0);
                    Display_Sunap_Amt(sunap1);
                }
                else if (tbCtrl_Exam.SelectedTab == tbExamPage3)
                {
                    tbCtrl_JONG.SelectedTabIndex = ((SuperTabControl)sender).SelectedTabIndex;
                    FstrJong = VB.Pstr(cboJONG3.Text, ".", 1).Trim();
                    cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[2].PadLeft(2, '0'), 0);
                    Display_Sunap_Amt(sunap2);
                }
                else if (tbCtrl_Exam.SelectedTab == tbExamPage4)
                {
                    tbCtrl_JONG.SelectedTabIndex = ((SuperTabControl)sender).SelectedTabIndex;
                    FstrJong = VB.Pstr(cboJONG4.Text, ".", 1).Trim();
                    cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[3].PadLeft(2, '0'), 0);
                    Display_Sunap_Amt(sunap3);
                }
                else if (tbCtrl_Exam.SelectedTab == tbExamPage5)
                {
                    tbCtrl_JONG.SelectedTabIndex = ((SuperTabControl)sender).SelectedTabIndex;
                    FstrJong = VB.Pstr(cboJONG5.Text, ".", 1).Trim();
                    cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[4].PadLeft(2, '0'), 0);
                    Display_Sunap_Amt(sunap4);
                }
                else
                {
                    panAMT.Initialize();
                }

                Clear_Spread_GroupCode();

                ssGroup.SetDataSource(lstsuInfo(tbCtrl_Exam.SelectedTabIndex));
                ssExam.SetDataSource(lstgrpExam(tbCtrl_Exam.SelectedTabIndex));
                SS_Sunap.SetDataSource(lstoldSuInfo(tbCtrl_Exam.SelectedTabIndex));

            }
            else if (sender == tbCtrl_JONG)
            {
                tbCtrl_Exam.SelectedTabIndex = ((SuperTabControl)sender).SelectedTabIndex;
            }
        }

        private void eTxtKeyDown(object sender, KeyEventArgs e)
        {
            //병원번호
            if (sender == txtPtno)
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
                            clsPublic.GstrMsgList += "  이름 :" + lstJumin[i].SNAME + "  검진번호: " + lstJumin[i].PANO + "  외래번호 :" + lstJumin[i].PTNO +  ComNum.VBLF + ComNum.VBLF;
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
                            //txtAge.Text = ComFunc.AgeCalcEx(txtJumin1.Text + txtJumin2.Text, dtpJepDate.Value.ToShortDateString()).To<string>();
                            txtAge.Text = cHB.READ_HIC_AGE_GESAN2(txtJumin1.Text + txtJumin2.Text).To<string>();

                            New_Patient_Create();
                            return;
                        }
                    }

                    //검진 수검자 정보
                    Display_HicPatient_Info(txtPtno.Text.Trim(), cboYear.Text);

                    //검진 접수 Display Main
                    Display_Jepsu_Main(txtPtno.Text, dtpJepDate.Value.ToShortDateString());

                    txtTel.Focus();
                    return;
                } 
            }
            //주민번호 앞자리
            else if (sender == txtJumin1)
            {
                if (e.KeyCode == Keys.Enter) { txtJumin2.Focus(); }
            }
            //주민번호 뒷자리
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
                    string strPANO = hicPatientService.GetPanoByJuminNo(clsAES.AES(txtJumin1.Text + txtJumin2.Text));

                    if (hicPatientService.GetCountbyJumin2(clsAES.AES(txtJumin1.Text + txtJumin2.Text)) > 0 && strPtno.IsNullOrEmpty())
                    {
                        strPtno = basPatientService.GetPaNobyJumin1Jumin2(txtJumin1.Text, clsAES.AES(txtJumin2.Text)).ToString();
                        if (!strPtno.IsNullOrEmpty())
                        {
                            hicPatientService.UpdatePtNobyJumin2(strPtno, clsAES.AES(txtJumin1.Text + txtJumin2.Text));
                        }
                    }

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
                        Display_HicPatient_Info(strPtno, cboYear.Text);
                    }
                    else
                    {
                        New_Patient_Create();
                    }

                    //검진 접수 Display Main
                    Display_Jepsu_Main(txtPtno.Text, dtpJepDate.Value.ToShortDateString());

                    Hic_Memo_Screen(FstrPtno);

                    txtTel.Focus();

                    return;
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
                }
            }
            else if (sender == txtLtdCode && e.KeyCode == Keys.Enter)
            {
                if (!txtLtdCode.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(); }
            }
            else if (sender == txtGongjeng && e.KeyCode == Keys.Enter)
            {
                if (!txtGongjeng.Text.Trim().IsNullOrEmpty()) { Hic_Code_Help("A2", txtGongjeng); }
            }
            else if (sender == txtJikJong && e.KeyCode == Keys.Enter)
            {
                if (!txtJikJong.Text.Trim().IsNullOrEmpty()) { Hic_Code_Help("05", txtJikJong); }
            }
        }

        /// <summary>
        /// 검진접수 Display Main 루틴 AS_IS 방식
        /// </summary>
        /// <param name="text"></param>
        /// <param name="v"></param>
        private void Display_Jepsu_Main(string argPtno, string argDate, List<string> lstAmChk = null)
        {
            //휴일 여부 체크
            if (chkChul.Checked) { FbHuilGasan = false; }   //출장검진은 휴일가산 없음
            FbHuilGasan = cHB.HIC_Huil_GasanDay(argDate);

            //당일접수내역 접수번호 조회
            //IList<HIC_JEPSU_HEA_EXJONG> Jep = hicJepsuHeaExjongService.GetListWrtnoByHic(argPtno, argDate);

            //당일접수내역 접수번호 조회 + 검진년도 추가
            IList<HIC_JEPSU_HEA_EXJONG> Jep = hicJepsuHeaExjongService.GetListWrtnoByHicYear(argPtno, argDate, cboYear.Text);


            //접수내역 있으면 수정
            if (Jep.Count > 0)
            {
                //각 접수내역 상세 Detail
                Display_Jepsu_Detail(Jep);
            }
            //신규접수
            else
            {
                Display_Jepsu_New(lstAmChk);
            }

            //수검자 메모
            Hic_Memo_Screen(argPtno);

            if (VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0) > 0)
            {
                //tabControl1.SelectedTab = tbETCpage6;
                txtLtdRemark.Text = hicLtdService.GetRemarkbyLtdCode(VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0));
                txtLtdRemark1.Text = hicLtdService.GetHaRemarkbyLtdCode(VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0));
            }
            else
            {
                txtLtdRemark.Text = "";
            }


            //암예약 읽음
            HIC_READ_RESV_CANCER(ssCan, txtJumin1.Text + txtJumin2.Text, DateTime.Now.ToShortDateString());

        }

        private void HIC_READ_RESV_CANCER(FpSpread ssCan, string argJumin, string argDate)
        {
            bool bHdColorChg = false;       //헤더색깔 변경 여부

            if (argJumin.Length != 13)
            {
                return;
            }

            int nRow = 0;

            List<HIC_CANCER_RESV2> list = hicCancerResv2Service.GetListByRTimeJumin(argDate, clsAES.AES(argJumin));

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    nRow += 1;
                    if (nRow > ssCan.ActiveSheet.RowCount)
                    {
                        ssCan.ActiveSheet.RowCount = nRow;
                    }

                    ssCan.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].RTIME.To<string>();
                    if (VB.Left(list[i].RTIME.To<string>(), 10).Trim() == dtpJepDate.Text) { bHdColorChg = true; }
                    ssCan.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].GBUGI == "Y" ? "◎" : "";
                    ssCan.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].GBGFS == "Y" ? "◎" : "";
                    ssCan.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].GBMAMMO == "Y" ? "◎" : "";
                    ssCan.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].GBRECUTM == "Y" ? "◎" : "";
                    ssCan.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].GBSONO == "Y" ? "◎" : "";
                    ssCan.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].GBWOMB == "Y" ? "◎" : "";
                    ssCan.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].GBBOHUM == "Y" ? "◎" : "";
                    ssCan.ActiveSheet.Cells[nRow - 1, 8].Text = list[i].GBGFSH == "Y" ? "◎" : "";
                    ssCan.ActiveSheet.Cells[nRow - 1, 9].Text = list[i].GBCT == "Y" ? "◎" : "";
                    ssCan.ActiveSheet.Cells[nRow - 1, 10].Text = list[i].REMARK;
                }

                if (list[0].RDATE == argDate)
                {
                    ssCan.ActiveSheet.Cells[nRow - 1, 10].ForeColor = Color.DarkRed;
                }
                else
                {
                    ssCan.ActiveSheet.Cells[nRow - 1, 10].ForeColor = Color.Black;
                }

                if (bHdColorChg)
                {
                    ssCan.ActiveSheet.ColumnHeader.Cells[0, 0, 0, ssCan.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 128, 128);
                }
            }
        }

        private List<READ_SUNAP_ITEM> Read_Sunap_GroupCode_His(long argWRTNO)
        {
            List<READ_SUNAP_ITEM> item = readSunapItemService.GetHicSunapHisInfoByWrtno(argWRTNO);

            return item;
        }

        /// <summary>
        /// 단일접수번호 1개씩 Display
        /// </summary>
        /// <param name="nWRTNO"></param>
        private void Display_Jepsu_Detail(IList<HIC_JEPSU_HEA_EXJONG> argJep)
        {
            int nJepCont = 0;

            if (argJep.Count > 0)
            {
                txtJepList.Text = "";
                tbCtrl_JONG.Enabled = true;
                tbCtrl_Exam.Enabled = true;
                panSelExam.Enabled = true;

                //접수 공통정보
                Screen_Display_Common_Info(argJep);

                for (int i = 0; i < argJep.Count; i++)
                {
                    //일반검진 내역
                    HIC_JEPSU item = hicJepsuService.GetItemByWRTNO(argJep[i].WRTNO);

                    //검진년도 
                    cboYear.SelectedIndex = cboYear.FindStringExact(item.GJYEAR.Trim());

                    GnWRTNO = item.GWRTNO;

                    clsHcType.THC[nJepCont].GBUSE = "U";
                    clsHcType.THC[nJepCont].GWRTNO = item.GWRTNO;
                    clsHcType.THC[nJepCont].WRTNO = item.WRTNO;
                    clsHcType.THC[nJepCont].PANO = item.PANO;
                    clsHcType.THC[nJepCont].GJJONG = item.GJJONG;
                    clsHcType.THC[nJepCont].UCODES = item.UCODES;
                    clsHcType.THC[nJepCont].SEXAMS = item.SEXAMS;
                    clsHcType.THC[nJepCont].REMARK = item.REMARK;
                    clsHcType.THC[nJepCont].GBAM = item.GBAM;

                    Clear_Working_Variants(nJepCont);

                    FstrUCodes = item.UCODES;
                    FstrSExams = item.SEXAMS;
                    FstrBuRate[nJepCont] = item.BURATE.Trim();
                    
                    cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[nJepCont].PadLeft(2, '0'), 0);

                    if (item.GBHEAENDO == "Y") { chkHeaEndo.Checked = true; }
                    if (item.GBCHUL == "Y") { chkChul.Checked = true; }

                    //암구분 표시
                    if (item.GJJONG == "31")
                    {
                        Display_GbAm(item.GBAM);
                        if (item.MURYOAM == "Y")
                        {
                            cboMuRyoAm.SelectedIndex = 1;
                        }
                        else
                        {
                            cboMuRyoAm.SelectedIndex = 0;
                        }
                    }

                    switch (nJepCont)
                    {
                        case 0:
                            tbJongPage1.Visible = true; tbJongPage1.Text = item.GJJONG;
                            tbExamPage1.Visible = true; tbExamPage1.Text = item.GJJONG;
                            tbCtrl_JONG.SelectedTab = tbJongPage1;
                            tbCtrl_Exam.SelectedTab = tbExamPage1;
                            FstrJong = item.GJJONG;

                            tbPanel1.SetData(item);
                            if (!item.WEBPRINTREQ.IsNullOrEmpty()) { rdoRES14.Checked = true; }
                            if (item.GBCHK3 == "Y") { rdoRES15.Checked = true; }
                            cboJONG1.Enabled = false; rdoSTS1A.Enabled = false; rdoSTS1B.Checked = true; btnJongClose01.Enabled = false;
                            if (item.GBJUSO == "Y") { chkRES12_1.Checked = true; }      //결과지 주소 (집)
                            if (item.GBJUSO == "N") { chkRES12_2.Checked = true; }      //결과지 주소 (회사)
                            if (item.GBJUSO == "E") { chkRES12_3.Checked = true; }      //결과지 주소 (기타)
                            if (item.JONGGUMYN == "1") { chkJongGum1.Checked = true; }  //종검여부표시

                            txtMail1.Text = item.MAILCODE;
                            txtJuso11.Text = item.JUSO1;
                            txtJuso21.Text = item.JUSO2;

                            suInfo0.AddRange(Read_Sunap_GroupCode(argJep[i].WRTNO, "HIC"));
                            oldSuInfo0.AddRange(Read_Sunap_GroupCode_His(argJep[i].WRTNO));
                            grpExam0.AddRange(Read_ExamCode(argJep[i].WRTNO, "HIC"));
                            sunap0 = Account_Sunap(argJep[i].WRTNO, "HIC");

                            Clear_Spread_GroupCode();
                            ssGroup.SetDataSource(suInfo0);
                            SS_Sunap.SetDataSource(oldSuInfo0);
                            ssExam.SetDataSource(grpExam0);
                            Display_Sunap_Amt(sunap0);
                            break;
                        case 1:
                            tbJongPage2.Visible = true; tbJongPage2.Text = item.GJJONG;
                            tbExamPage2.Visible = true; tbExamPage2.Text = item.GJJONG;
                            tbCtrl_JONG.SelectedTab = tbJongPage2;
                            tbCtrl_Exam.SelectedTab = tbExamPage2;
                            FstrJong = item.GJJONG;

                            tbPanel2.SetData(item);
                            if (!item.WEBPRINTREQ.IsNullOrEmpty()) { rdoRES24.Checked = true; }
                            if (item.GBCHK3 == "Y") { rdoRES25.Checked = true; }
                            cboJONG2.Enabled = false; rdoSTS2A.Enabled = false; rdoSTS2B.Checked = true; btnJongClose02.Enabled = false;
                            if (item.GBJUSO == "Y") { chkRES22_1.Checked = true; }      //결과지 주소 (집)
                            if (item.GBJUSO == "N") { chkRES22_2.Checked = true; }      //결과지 주소 (회사)
                            if (item.GBJUSO == "E") { chkRES22_3.Checked = true; }      //결과지 주소 (기타)
                            if (item.JONGGUMYN == "1") { chkJongGum2.Checked = true; }  //종검여부표시

                            txtMail2.Text = item.MAILCODE;
                            txtJuso12.Text = item.JUSO1;
                            txtJuso22.Text = item.JUSO2;

                            suInfo1.AddRange(Read_Sunap_GroupCode(argJep[i].WRTNO, "HIC"));
                            oldSuInfo1.AddRange(Read_Sunap_GroupCode_His(argJep[i].WRTNO));
                            grpExam1.AddRange(Read_ExamCode(argJep[i].WRTNO, "HIC"));
                            sunap1 = Account_Sunap(argJep[i].WRTNO, "HIC");

                            Clear_Spread_GroupCode();
                            ssGroup.SetDataSource(suInfo1);
                            SS_Sunap.SetDataSource(oldSuInfo1);
                            ssExam.SetDataSource(grpExam1);
                            Display_Sunap_Amt(sunap1);
                            break;
                        case 2:
                            tbJongPage3.Visible = true; tbJongPage3.Text = item.GJJONG;
                            tbExamPage3.Visible = true; tbExamPage3.Text = item.GJJONG;
                            tbCtrl_JONG.SelectedTab = tbJongPage3;
                            tbCtrl_Exam.SelectedTab = tbExamPage3;
                            FstrJong = item.GJJONG;

                            tbPanel3.SetData(item);
                            if (!item.WEBPRINTREQ.IsNullOrEmpty()) { rdoRES34.Checked = true; }
                            if (item.GBCHK3 == "Y") { rdoRES35.Checked = true; }
                            cboJONG3.Enabled = false; rdoSTS3A.Enabled = false; rdoSTS3B.Checked = true; btnJongClose03.Enabled = false;
                            if (item.GBJUSO == "Y") { chkRES32_1.Checked = true; }      //결과지 주소 (집)
                            if (item.GBJUSO == "N") { chkRES32_2.Checked = true; }      //결과지 주소 (회사)
                            if (item.GBJUSO == "E") { chkRES32_3.Checked = true; }      //결과지 주소 (기타)
                            if (item.JONGGUMYN == "1") { chkJongGum3.Checked = true; }  //종검여부표시

                            txtMail3.Text = item.MAILCODE;
                            txtJuso13.Text = item.JUSO1;
                            txtJuso23.Text = item.JUSO2;

                            suInfo2.AddRange(Read_Sunap_GroupCode(argJep[i].WRTNO, "HIC"));
                            oldSuInfo2.AddRange(Read_Sunap_GroupCode_His(argJep[i].WRTNO));
                            grpExam2.AddRange(Read_ExamCode(argJep[i].WRTNO, "HIC"));
                            sunap2 = Account_Sunap(argJep[i].WRTNO, "HIC");

                            Clear_Spread_GroupCode();
                            ssGroup.SetDataSource(suInfo2);
                            SS_Sunap.SetDataSource(oldSuInfo2);
                            ssExam.SetDataSource(grpExam2);
                            Display_Sunap_Amt(sunap2);
                            break;
                        case 3:
                            tbJongPage4.Visible = true; tbJongPage4.Text = item.GJJONG;
                            tbExamPage4.Visible = true; tbExamPage4.Text = item.GJJONG;
                            tbCtrl_JONG.SelectedTab = tbJongPage4;
                            tbCtrl_Exam.SelectedTab = tbExamPage4;
                            FstrJong = item.GJJONG;

                            tbPanel4.SetData(item);
                            if (!item.WEBPRINTREQ.IsNullOrEmpty()) { rdoRES44.Checked = true; }
                            if (item.GBCHK3 == "Y") { rdoRES45.Checked = true; }
                            cboJONG4.Enabled = false; rdoSTS4A.Enabled = false; rdoSTS4B.Checked = true; btnJongClose04.Enabled = false;
                            if (item.GBJUSO == "Y") { chkRES42_1.Checked = true; }      //결과지 주소 (집)
                            if (item.GBJUSO == "N") { chkRES42_2.Checked = true; }      //결과지 주소 (회사)
                            if (item.GBJUSO == "E") { chkRES42_3.Checked = true; }      //결과지 주소 (기타)
                            if (item.JONGGUMYN == "1") { chkJongGum4.Checked = true; }  //종검여부표시

                            txtMail4.Text = item.MAILCODE;
                            txtJuso14.Text = item.JUSO1;
                            txtJuso24.Text = item.JUSO2;

                            suInfo3.AddRange(Read_Sunap_GroupCode(argJep[i].WRTNO, "HIC"));
                            oldSuInfo3.AddRange(Read_Sunap_GroupCode_His(argJep[i].WRTNO));
                            grpExam3.AddRange(Read_ExamCode(argJep[i].WRTNO, "HIC"));
                            sunap3 = Account_Sunap(argJep[i].WRTNO, "HIC");

                            Clear_Spread_GroupCode();
                            ssGroup.SetDataSource(suInfo3);
                            SS_Sunap.SetDataSource(oldSuInfo3);
                            ssExam.SetDataSource(grpExam3);
                            Display_Sunap_Amt(sunap3);
                            break;
                        case 4:
                            tbJongPage5.Visible = true; tbJongPage5.Text = item.GJJONG;
                            tbExamPage5.Visible = true; tbExamPage5.Text = item.GJJONG;
                            tbCtrl_JONG.SelectedTab = tbJongPage5;
                            tbCtrl_Exam.SelectedTab = tbExamPage5;
                            FstrJong = item.GJJONG;

                            tbPanel5.SetData(item);
                            if (!item.WEBPRINTREQ.IsNullOrEmpty()) { rdoRES54.Checked = true; }
                            if (item.GBCHK3 == "Y") { rdoRES55.Checked = true; }
                            cboJONG5.Enabled = false; rdoSTS5A.Enabled = false; rdoSTS5B.Checked = true; btnJongClose05.Enabled = false;
                            if (item.GBJUSO == "Y") { chkRES52_1.Checked = true; }      //결과지 주소 (집)
                            if (item.GBJUSO == "N") { chkRES52_2.Checked = true; }      //결과지 주소 (회사)
                            if (item.GBJUSO == "E") { chkRES52_3.Checked = true; }      //결과지 주소 (기타)
                            if (item.JONGGUMYN == "1") { chkJongGum5.Checked = true; }  //종검여부표시

                            txtMail5.Text = item.MAILCODE;
                            txtJuso15.Text = item.JUSO1;
                            txtJuso25.Text = item.JUSO2;

                            suInfo4.AddRange(Read_Sunap_GroupCode(argJep[i].WRTNO, "HIC"));
                            oldSuInfo4.AddRange(Read_Sunap_GroupCode_His(argJep[i].WRTNO));
                            grpExam4.AddRange(Read_ExamCode(argJep[i].WRTNO, "HIC"));
                            sunap4 = Account_Sunap(argJep[i].WRTNO, "HIC");

                            Clear_Spread_GroupCode();
                            ssGroup.SetDataSource(suInfo4);
                            SS_Sunap.SetDataSource(oldSuInfo4);
                            ssExam.SetDataSource(grpExam4);
                            Display_Sunap_Amt(sunap4);
                            break;
                        default: break;
                    }

                    nJepCont++;

                    //특수검진 문진내역 
                    Display_Hic_Special(item.WRTNO);    //일특 특검정보

                    if (!FstrUCodes.IsNullOrEmpty())
                    {
                        panDockingYN(panJONG, panSchool, false);
                        panDockingYN(panJONG, panUCODES, true);
                        lblUCODES.Text = cHcMain.UCode_Names_Display(FstrUCodes);
                    }

                    if (item.GJJONG == "56"|| item.GJJONG == "59")
                    {
                        panDockingYN(panJONG, panUCODES, false);
                        panDockingYN(panJONG, panSchool, true);
                    }

                    //당일 접수정보(상단노출)
                    txtJepList.Text += comHpcLibBService.GetJongNameByCode("HIC", item.GJJONG) + ", ";

                    //if (!strGbNhic.IsNullOrEmpty())
                    //{
                    //    strGbNhic = hicExjongService.GetGbNhicByExJong(item.GJJONG);
                    //}
                }

                //자격조회 여부
                //if (!chkNoNhic.Checked)
                //{
                //    //자격조회
                //    if (!txtSName.Text.IsNullOrEmpty() && strGbNhic == "Y")
                //    {
                //        //Clear_Nhic_List();
                //        Hic_Chk_Nhic("H", txtSName.Text, txtJumin1.Text + txtJumin2.Text, txtPtno.Text, cboYear.Text);
                //    }
                //}
            }
        }

        private void Display_JepsuWork_Detail(IList<HIC_JEPSU_HEA_EXJONG> argJep)
        {
            int nJepCont = 0;
            string strGbNhic = string.Empty;

            if (argJep.Count > 0)
            {
                tbCtrl_JONG.Enabled = true;
                tbCtrl_Exam.Enabled = true;
                panSelExam.Enabled = true;
                                
                for (int i = 0; i < argJep.Count; i++)
                {
                    //가접수 내역 조회
                    HIC_JEPSU_WORK item = hicJepsuWorkService.GetItemByRid(argJep[i].RID);

                    //검진년도 
                    cboYear.SelectedIndex = cboYear.FindStringExact(item.GJYEAR.Trim());

                    clsHcType.THC[nJepCont].GJJONG = item.GJJONG;
                    clsHcType.THC[nJepCont].UCODES = item.UCODES;
                    clsHcType.THC[nJepCont].SEXAMS = item.SEXAMS;
                    clsHcType.THC[nJepCont].PANO = item.PANO;
                    clsHcType.THC[nJepCont].GBUSE = "G";
                    clsHcType.THC[nJepCont].REMARK = item.REMARK;
                    clsHcType.THC[nJepCont].RID = item.RID;
                    clsHcType.THC[nJepCont].GBAM = item.GBAM;
                    txtPano.Text = item.PANO.ToString();

                    FstrUCodes = item.UCODES;
                    FstrSExams = item.SEXAMS;

                    //2020-08-25(가접수회사 코드를 기준으로 불러옴)
                    if (!item.LTDCODE.IsNullOrEmpty() && item.LTDCODE > 0)
                    {
                        txtLtdCode.Text = txtLtdCode.Text = item.LTDCODE.ToString() + "." + hicLtdService.READ_Ltd_One_Name(item.LTDCODE.ToString());
                    }
                    else
                    {
                        txtLtdCode.Text = "";
                    }


                    clsHcType.THC[nJepCont].IPSADATE = item.IPSADATE;
                    clsHcType.THC[nJepCont].BUSEIPSA = item.BUSEIPSA;

                    //2020-08-19추가
                    if (!item.IPSADATE.IsNullOrEmpty())
                    {
                        dtpIpsaDate.Text = item.IPSADATE;
                    }

                    if (!item.BUSEIPSA.IsNullOrEmpty())
                    {
                        dtpJenipDate.Text = item.BUSEIPSA;
                    }
                    else if (item.BUSEIPSA.IsNullOrEmpty() && !item.IPSADATE.IsNullOrEmpty())
                    {
                        dtpJenipDate.Text = item.IPSADATE;
                    }

                    //암구분 표시
                    if (item.GJJONG == "31")
                    {
                        Display_GbAm(item.GBAM);
                        if (item.MURYOAM == "Y")
                        {
                            cboMuRyoAm.SelectedIndex = 1;
                        }
                        else
                        {
                            cboMuRyoAm.SelectedIndex = 0;
                        }
                    }

                    

                    switch (nJepCont)
                    {
                        case 0:
                            tbJongPage1.Visible = true;
                            tbJongPage1.Text = item.GJJONG;
                            tbExamPage1.Visible = true;
                            tbExamPage1.Text = item.GJJONG;
                            tbCtrl_JONG.SelectedTab = tbJongPage1;
                            tbCtrl_Exam.SelectedTab = tbExamPage1;

                            tbPanel1.SetData(item);             //일반탭 정보 세팅
                            rdoSTS1A.Checked = true;            //접수, 예약구분

                            suInfo0.AddRange(Read_SunapWork_GroupCode(item.PANO, item.GJJONG, 0));
                            grpExam0.AddRange(Read_ExamCode_Work(suInfo0));
                            
                            Clear_Spread_GroupCode();
                            ssGroup.SetDataSource(suInfo0);
                            ssExam.SetDataSource(grpExam0);
                            Gesan_Sunap_Amt(nJepCont, suInfo0, FstrBuRate[nJepCont], 0);
                            SetControl_Special(nJepCont, suInfo0, grpExam0);    //컨트롤 자동세팅

                            break;
                        case 1:
                            tbJongPage2.Visible = true;
                            tbJongPage2.Text = item.GJJONG;
                            tbExamPage2.Visible = true;
                            tbExamPage2.Text = item.GJJONG;
                            tbCtrl_JONG.SelectedTab = tbJongPage2;
                            tbCtrl_Exam.SelectedTab = tbExamPage2;

                            tbPanel2.SetData(item);
                            rdoSTS2A.Checked = true;

                            suInfo1.AddRange(Read_SunapWork_GroupCode(item.PANO, item.GJJONG, 1));
                            grpExam1.AddRange(Read_ExamCode_Work(suInfo1));
                            
                            Clear_Spread_GroupCode();
                            ssGroup.SetDataSource(suInfo1);
                            ssExam.SetDataSource(grpExam1);
                            Gesan_Sunap_Amt(nJepCont, suInfo1, FstrBuRate[nJepCont], 0);
                            SetControl_Special(nJepCont, suInfo1, grpExam1);    //컨트롤 자동세팅
                            break;
                        case 2:
                            tbJongPage3.Visible = true;
                            tbJongPage3.Text = item.GJJONG;
                            tbExamPage3.Visible = true;
                            tbExamPage3.Text = item.GJJONG;
                            tbCtrl_JONG.SelectedTab = tbJongPage3;
                            tbCtrl_Exam.SelectedTab = tbExamPage3;

                            tbPanel3.SetData(item);
                            rdoSTS3A.Checked = true;

                            suInfo2.AddRange(Read_SunapWork_GroupCode(item.PANO, item.GJJONG, 2));
                            grpExam2.AddRange(Read_ExamCode_Work(suInfo2));

                            Clear_Spread_GroupCode();
                            ssGroup.SetDataSource(suInfo2);
                            ssExam.SetDataSource(grpExam2);
                            Gesan_Sunap_Amt(nJepCont, suInfo2, FstrBuRate[nJepCont], 0);
                            SetControl_Special(nJepCont, suInfo2, grpExam2);    //컨트롤 자동세팅

                            break;
                        case 3:
                            tbJongPage4.Visible = true;
                            tbJongPage4.Text = item.GJJONG;
                            tbExamPage4.Visible = true;
                            tbExamPage4.Text = item.GJJONG;
                            tbCtrl_JONG.SelectedTab = tbJongPage4;
                            tbCtrl_Exam.SelectedTab = tbExamPage4;

                            tbPanel4.SetData(item);
                            rdoSTS4A.Checked = true;

                            suInfo3.AddRange(Read_SunapWork_GroupCode(item.PANO, item.GJJONG, 3));
                            grpExam3.AddRange(Read_ExamCode_Work(suInfo3));
                            
                            Clear_Spread_GroupCode();
                            ssGroup.SetDataSource(suInfo3);
                            ssExam.SetDataSource(grpExam3);
                            Gesan_Sunap_Amt(nJepCont, suInfo3, FstrBuRate[nJepCont], 0);

                            SetControl_Special(nJepCont, suInfo3, grpExam3);    //컨트롤 자동세팅
                            break;
                        case 4:
                            tbJongPage5.Visible = true;
                            tbJongPage5.Text = item.GJJONG;
                            tbExamPage5.Visible = true;
                            tbExamPage5.Text = item.GJJONG;
                            tbCtrl_JONG.SelectedTab = tbJongPage5;
                            tbCtrl_Exam.SelectedTab = tbExamPage5;

                            tbPanel5.SetData(item);
                            rdoSTS5A.Checked = true;

                            suInfo4.AddRange(Read_SunapWork_GroupCode(item.PANO, item.GJJONG, 4));
                            grpExam4.AddRange(Read_ExamCode_Work(suInfo4));
                            
                            Clear_Spread_GroupCode();
                            ssGroup.SetDataSource(suInfo4);
                            ssExam.SetDataSource(grpExam4);
                            Gesan_Sunap_Amt(nJepCont, suInfo4, FstrBuRate[nJepCont], 0);
                            SetControl_Special(nJepCont, suInfo4, grpExam4);    //컨트롤 자동세팅

                            break;
                        default: break;
                    }

                    FstrBuRate[nJepCont] = item.BURATE.Trim();

                    cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[nJepCont].PadLeft(2, '0'), 0);

                    if (!FstrUCodes.IsNullOrEmpty())
                    {
                        panDockingYN(panJONG, panSchool, false);
                        panDockingYN(panJONG, panUCODES, true);
                        lblUCODES.Text = cHcMain.UCode_Names_Display(FstrUCodes);
                    }

                    if (item.GJJONG == "56")
                    {
                        panDockingYN(panJONG, panUCODES, false);
                        panDockingYN(panJONG, panSchool, true);
                    }

                    if (strGbNhic.IsNullOrEmpty())
                    {
                        strGbNhic = hicExjongService.GetGbNhicByExJong(item.GJJONG);
                    }

                    if (FbHuilGasan)
                    {
                        List<string> lstCodes = new List<string>();

                        if (item.GJJONG == "11")
                        {
                            lstCodes.Add("1116");
                            lstCodes.Add("1118");
                        }
                        else if (item.GJJONG == "16")
                        {
                            lstCodes.Add("1117");
                        }
                        else if (item.GJJONG == "31")
                        {
                            lstCodes.Add("1119");
                        }

                        frmHcGroupCode_ssDblclick(lstCodes, "");
                    }

                    if (!chkNoNhic.Checked)
                    {
                        //자격조회
                        if (!txtSName.Text.IsNullOrEmpty() && strGbNhic == "Y")
                        {
                            Hic_Chk_Nhic("H", txtSName.Text, txtJumin1.Text + txtJumin2.Text, txtPtno.Text, cboYear.Text, nJepCont, "Y");
                        }
                    }

                    //자격조회 점검 알림 메세지
                    if (item.GJJONG == "11")
                    {
                        if (!clsHcType.THNV.h1Cha.IsNullOrEmpty() && clsHcType.THNV.h1Cha.Contains("비대상"))
                        {
                            MessageBox.Show("1차검진 비대상자 입니다. 검진자격을 확인하세요.", "접수불가");
                        }

                        if (!clsHcType.THNV.h1ChaDate.IsNullOrEmpty())
                        {
                            MessageBox.Show("1차검진을 이미 실시하였습니다. 검진내역을 확인하세요.", "접수불가");
                        }
                    }
                    else if (item.GJJONG == "16")
                    {
                        if (!clsHcType.THNV.h2Cha.IsNullOrEmpty() && clsHcType.THNV.h2Cha.Contains("비대상"))
                        {
                            MessageBox.Show("2차검진 비대상자 입니다. 검진자격을 확인하세요.", "접수불가");
                        }

                        if (!clsHcType.THNV.h2ChaDate.IsNullOrEmpty())
                        {
                            MessageBox.Show("2차검진을 이미 실시하였습니다. 검진내역을 확인하세요.", "접수불가");
                        }
                    }

                    // 암검진 초기 부담율 세팅
                    //분변, 자궁 검사 하나라도 있을 경우 진찰료는 01 공단부담으로...
                    READ_SUNAP_ITEM sRSI = new READ_SUNAP_ITEM();

                    for (int j = 0; j < lstsuInfo(nJepCont).Count; j++)
                    {
                        sRSI = lstsuInfo(nJepCont)[j];

                        Cancer_GroupCode_BuRate_Set(nJepCont, ref sRSI, "WORK");

                        //TODO : 그룹코드를 추가, 제외할때에도 부담율이 바뀌게
                        if (lstsuInfo(nJepCont)[j].GRPCODE.To<string>("").Trim() == "3116" ||
                            lstsuInfo(nJepCont)[j].GRPCODE.To<string>("").Trim() == "3132")
                        {
                            for (int k = 0; k < lstsuInfo(nJepCont).Count; k++)
                            {
                                if (lstsuInfo(nJepCont)[k].GRPCODE.To<string>("").Trim() == "3101")
                                {
                                    lstsuInfo(nJepCont)[k].GBSELF = "01";

                                    if (clsHcType.THNV.hJaGubun.To<string>("").Trim() == "의료급여") { lstsuInfo(nJepCont)[k].GBSELF = "11"; }
                                    break;
                                }
                            }
                        }
                    }

                    nJepCont++;
                }

                //자격에 따른 부담율 세부 설정 
                for (int nIdx = 0; nIdx < argJep.Count; nIdx++)
                {
                    if (clsHcType.THC[nIdx].GJJONG == "31")
                    {
                        //자궁경부암 검사만 있을경우 부담율 조합100% 세팅
                        if (chkAm3.Checked || chkAm6.Checked)
                        {
                            if (!chkAm1.Checked && !chkAm2.Checked && !chkAm4.Checked && !chkAm5.Checked && !chkAm7.Checked)
                            {
                                FstrBuRate[nIdx] = "01";
                                cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[nIdx].PadLeft(2, '0'), 0);
                            }
                        }

                        //보건소암 있을경우 무료암대상으로 표시
                        if (clsHcType.THNV.hBogen.To<string>("").Trim() != "")
                        {
                            if (VB.Left(clsHcType.THNV.hGKiho, 1) == "9")
                            {
                                FstrBuRate[nIdx] = "11";    //보건소 100%
                                cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[nIdx].PadLeft(2, '0'), 0);
                                chkGubAm.Checked = true;
                                cboMuRyoAm.SelectedIndex = 1;  //N
                            }
                            else
                            {
                                FstrBuRate[nIdx] = "12";    //조합90%,보건소10%
                                cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[nIdx].PadLeft(2, '0'), 0);
                                cboMuRyoAm.SelectedIndex = 1;   //Y
                            }
                        }
                    }
                }

            }
        }

        private void Screen_Display_Common_Info(IList<HIC_JEPSU_HEA_EXJONG> argJep)
        {
            try
            {
                for (int i = 0; i < argJep.Count; i++)
                {
                    HIC_JEPSU item = hicJepsuService.GetItemByWRTNO(argJep[i].WRTNO);

                    if (item.IEMUNNO > 0)
                    {
                        FnIEMunNo = item.IEMUNNO;

                        HIC_IE_MUNJIN_NEW iHIMN = hicIeMunjinNewService.GetItembyWrtNoMunDateLike(item.IEMUNNO, cboYear.Text);

                        if (!iHIMN.IsNullOrEmpty())
                        {
                            lblIEMunjin.BackColor = Color.LightSalmon;
                            lblIEMunjin.Text = "인터넷문진 " + iHIMN.MUNDATE;
                            IEMunjin_Name_Display(iHIMN.RECVFORM);
                            MUNJIN_CHK(item);
                        }
                    }
                    else if (dtpJepDate.Text == DateTime.Now.ToShortDateString())       //당일 인터넷문진표 작성여부
                    {
                        string strDate =cboYear.Text + "-01-01";

                        //2021년 1,2월 한시적으로 전년도 문진조회
                        //if (string.Compare(item.JEPDATE, "2021-03-01") < 0) { strDate = "2020-01-01"; }

                        HIC_IE_MUNJIN_NEW iHIMN = hicIeMunjinNewService.GetItembyPtNoMunDate(item.PTNO, strDate);

                        if (!iHIMN.IsNullOrEmpty())
                        {
                            FnIEMunNo = iHIMN.WRTNO;
                            lblIEMunjin.BackColor = Color.LightSalmon;
                            lblIEMunjin.Text = "인터넷문진";
                            IEMunjin_Name_Display(iHIMN.RECVFORM);

                            MUNJIN_CHK(item);
                        }
                    }

                    HEA_JEPSU item1 = heaJepsuService.Read_Jepsu3(item.PTNO, dtpJepDate.Text);
                    if (!item1.IsNullOrEmpty())
                    {
                        lblHeaJep.BackColor = Color.HotPink;
                        lblHeaJep.Text = "당일종검";
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

        private void MUNJIN_CHK(HIC_JEPSU item)
        {
            string  strChk2 = "", strChk1 = "";

            HIC_IE_MUNJIN_NEW iHIMN = hicIeMunjinNewService.GetItembySNamePtnoMunDate(item.SNAME, item.PTNO, item.JEPDATE);

            if (!iHIMN.IsNullOrEmpty())
            {
                if (item.GJJONG == "11")
                {
                    if (VB.InStr(iHIMN.RECVFORM, "12001") > 0) { strChk1 = "OK"; }
                }
                else if (item.GJJONG == "31")
                {
                    if (VB.InStr(iHIMN.RECVFORM, "12003") > 0) { strChk2 = "OK"; }
                }
            }

            if (item.GJJONG == "11")
            {
                if (strChk1 == "") { MessageBox.Show("공통문진표 작성여부를 확인해주세요", "확인"); }
            }
            else if (item.GJJONG == "31")
            {
                if (strChk2 == "") { MessageBox.Show("암 문진표 작성여부를 확인해주세요", "확인"); }
            }
        }

        private void IEMunjin_Name_Display(string ArgRecvForm)
        {
            string strResult = string.Empty;

            if (VB.InStr(ArgRecvForm, "12001") > 0) { strResult += "공통,";      }
            if (VB.InStr(ArgRecvForm, "12003") > 0) { strResult += "암검진,";    }
            if (VB.InStr(ArgRecvForm, "12005") > 0) { strResult += "구강,";      }
            if (VB.InStr(ArgRecvForm, "12006") > 0) { strResult += "노인신체,";      }
            if (VB.InStr(ArgRecvForm, "12010") > 0) { strResult += "초등학생,";  }
            if (VB.InStr(ArgRecvForm, "12014") > 0) { strResult += "학생구강,";  }
            if (VB.InStr(ArgRecvForm, "12020") > 0) { strResult += "중고등학생,";}
            if (VB.InStr(ArgRecvForm, "20002") > 0) { strResult += "특수,";      }
            if (VB.InStr(ArgRecvForm, "20003") > 0) { strResult += "폐활량,";    }
            if (VB.InStr(ArgRecvForm, "20004") > 0) { strResult += "흡연,";      }
            if (VB.InStr(ArgRecvForm, "20005") > 0) { strResult += "음주,";      }
            if (VB.InStr(ArgRecvForm, "20006") > 0) { strResult += "운동,";      }
            if (VB.InStr(ArgRecvForm, "20007") > 0) { strResult += "영양,";      }
            if (VB.InStr(ArgRecvForm, "20008") > 0) { strResult += "비만,";      }
            if (VB.InStr(ArgRecvForm, "20009") > 0) { strResult += "만40우울,";  }
            if (VB.InStr(ArgRecvForm, "20010") > 0) { strResult += "만66우울,";  }
            if (VB.InStr(ArgRecvForm, "20011") > 0) { strResult += "인지기능,";  }
            if (VB.InStr(ArgRecvForm, "20012") > 0) { strResult += "정신건강,"; }
            if (VB.InStr(ArgRecvForm, "30001") > 0) { strResult += "야간1차,";   }
            if (VB.InStr(ArgRecvForm, "30003") > 0) { strResult += "야간2차,";   }

            if (strResult != "")
            {
                lblMsg.Text = "▶인터넷문진: " + strResult;
                lblMsg.BackColor = Color.LightSalmon;
            }
        }

        /// <summary>
        /// 암검진 구분 표시
        /// </summary>
        /// <param name="gBAM"></param>
        private void Display_GbAm(string gBAM)
        {
            for (int i = 1; i < VB.L(gBAM, ","); i++)
            {
                if (VB.Pstr(gBAM, ",", i) == "1")
                {
                    switch (i)
                    {
                        case 1: chkAm1.Checked = true; break;
                        case 2: chkAm2.Checked = true; break;
                        case 3: chkAm3.Checked = true; break;
                        case 4: chkAm4.Checked = true; break;
                        case 5: chkAm5.Checked = true; break;
                        case 6: chkAm6.Checked = true; break;
                        case 7: chkAm7.Checked = true; break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (i)
                    {
                        case 1: chkAm1.Checked = false; break;
                        case 2: chkAm2.Checked = false; break;
                        case 3: chkAm3.Checked = false; break;
                        case 4: chkAm4.Checked = false; break;
                        case 5: chkAm5.Checked = false; break;
                        case 6: chkAm6.Checked = false; break;
                        case 7: chkAm7.Checked = false; break;
                        default:
                            break;
                    }
                }
            }
        }

        private void Display_GbAm2(string gBAM)
        {
            for (int i = 1; i < VB.L(gBAM, ","); i++)
            {
                if (VB.Pstr(gBAM, ",", i) == "1")
                {
                    switch (i)
                    {
                        case 1: chkAm1.Checked = true; break;
                        case 2: chkAm2.Checked = true; break;
                        case 3: chkAm3.Checked = true; break;
                        case 4: chkAm4.Checked = true; break;
                        case 5: chkAm5.Checked = true; break;
                        case 6: chkAm6.Checked = true; break;
                        case 7: chkAm7.Checked = true; break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 접수번호별 그룹코드 목록 가져오기
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="argGbn"></param>
        /// <returns></returns>
        private List<READ_SUNAP_ITEM> Read_Sunap_GroupCode(long argWRTNO, string argGbn)
        {
            List<READ_SUNAP_ITEM> item = new List<READ_SUNAP_ITEM>();

            if (argGbn == "HIC")
            {
                item = readSunapItemService.GetHicSunapInfoByWrtno(argWRTNO);
            }
            else if (argGbn == "HEA")
            {
                item = readSunapItemService.GetHeaSunapInfoByWrtno(argWRTNO);
            }
            
            return item;
        }

        /// <summary>
        /// 접수번호별 검사코드 목록 가져오기
        /// </summary>
        /// <param name="argWRTNO"></param>
        /// <param name="argGbn"></param>
        /// <returns></returns>
        private List<GROUPCODE_EXAM_DISPLAY> Read_ExamCode(long argWRTNO, string argGbn)
        {
            List<GROUPCODE_EXAM_DISPLAY> item = new List<GROUPCODE_EXAM_DISPLAY>();

            if (argGbn == "HIC")
            {
                item = groupCodeExamDisplayService.GetHicListByWrtno(argWRTNO);
            }
            else if (argGbn == "HEA")
            {
                item = groupCodeExamDisplayService.GetHeaListByWrtno(argWRTNO);
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

            txtTotAmt.Text   = item.TOTAMT.ToString("#,##0");
            txtJhpAmt.Text   = item.JOHAPAMT.ToString("#,##0");
            txtLtdAmt.Text   = item.LTDAMT.ToString("#,##0");
            txtBoAmt.Text    = item.BOGENAMT.ToString("#,##0");
            txtBonAmt.Text   = item.BONINAMT.ToString("#,##0");
            txtMisuAmt.Text  = item.MISUAMT.ToString("#,##0");
            txtHalinAmt.Text = item.HALINAMT.ToString("#,##0");
            
            for (int i = 0; i < cboHalinGye.Items.Count; i++)
            {
                if (VB.Pstr(cboHalinGye.Items[i].To<string>(""), ".", 1).Trim() == item.HALINGYE.To<string>("").Trim())
                {
                    cboHalinGye.SelectedIndex = i;
                    break;
                }
            }

            ////2020-09-04(주석처리)
            ////본인부담은 있는데 수납된 금액이 없을때
            //if (item.BONINAMT > 0 && item.SUNAPAMT == 0)
            //{   //수납전
            //    //if (txtCardAmt.Text.Replace(",", "").To<long>(0) == 0 && txtCashAmt.Text.Replace(",", "").To<long>(0) == 0 || item.WRTNO == 0)
            //    //{
            //    //    txtCardAmt.Text = item.BONINAMT.ToString("#,##0");
            //    //    txtCashAmt.Text = "0";
            //    //}
            //    //else
            //    //{
            //    //    txtCashAmt.Text = (item.BONINAMT - txtCardAmt.Text.Replace(",", "").To<long>(0)).ToString("#,##0");
            //    //}

            //    txtIpgumAmt.Text = item.BONINAMT.ToString("#,##0");
            //}
            //else
            //{
            //    //수납후
            //    if (item.WRTNO > 0)
            //    {
            //        txtCardAmt.Text = item.SUNAPAMT2.ToString("#,##0");
            //        txtCashAmt.Text = (item.SUNAPAMT - item.SUNAPAMT2).ToString("#,##0");
            //        txtIpgumAmt.Text = item.SUNAPAMT.ToString("#,##0");
            //    }
            //    else
            //    {
            //        txtIpgumAmt.Text = item.SUNAPAMT.ToString("#,##0");
            //    }
            //}


            //본인부담은 있는데 수납된 금액이 없을때
            if (item.BONINAMT > 0 && item.SUNAPAMT == 0)
            {   //수납전
                if (txtCardAmt.Text.Replace(",", "").To<long>(0) == 0 && txtCashAmt.Text.Replace(",", "").To<long>(0) == 0)
                {
                    txtCardAmt.Text = item.BONINAMT.ToString("#,##0");
                    txtCashAmt.Text = "0";
                }
                else
                {
                    txtCashAmt.Text = (item.BONINAMT - txtCardAmt.Text.Replace(",", "").To<long>(0)).ToString("#,##0");
                }

                txtIpgumAmt.Text = item.BONINAMT.ToString("#,##0");
            }
            else
            {
                //수납후
                txtCardAmt.Text = item.SUNAPAMT2.ToString("#,##0");
                txtCashAmt.Text = (item.SUNAPAMT - item.SUNAPAMT2).ToString("#,##0");
                txtIpgumAmt.Text = item.SUNAPAMT.ToString("#,##0");
            }




            if (pSunap != null)
            {
                txtChaAmt.Text = (item.BONINAMT - pSunap.SUNAPAMT).ToString("#,##0");
            }
            else
            {
                txtChaAmt.Text = item.BONINAMT.ToString("#,##0");
            }

            item.SUNAPAMT = txtIpgumAmt.Text.Replace(",", "").To<long>(0);
            item.SUNAPAMT2 = txtCardAmt.Text.Replace(",", "").To<long>(0);
        }

        /// <summary>
        /// 특수검진 문진표 Data Display
        /// </summary>
        /// <param name="wRTNO"></param>
        private void Display_Hic_Special(long wRTNO)
        {
            HIC_RES_SPECIAL item = hicResSpecialService.GetItemByWrtno(wRTNO);

            if (!item.IsNullOrEmpty())
            {
                //panLTD.SetData(item);

                txtLtdSabun.Text = item.SABUN;
                txtBuse.Text = item.BUSE;

                //txtGongjeng.Text = item.GONGJENG;
                if (txtGongjeng.Text.IsNullOrEmpty())
                {
                    if (!item.GONGJENG.IsNullOrEmpty())
                    {
                        txtGongjeng.Text += "." + hicCodeService.GetNameByGubunCode("A2", txtGongjeng.Text.Trim());     //공정명
                    }
                }

                //txtJikJong.Text = item.JIKJONG;
                if (txtJikJong.Text.IsNullOrEmpty())
                {
                    if (!item.JIKJONG.IsNullOrEmpty())
                    {
                        txtJikJong.Text += "." + hicCodeService.GetNameByGubunCode("05", txtJikJong.Text.Trim());      //직종
                    }
                }
                
                if (!item.JENIPDATE.IsNullOrEmpty()) { dtpJenipDate.Text = item.JENIPDATE.ToString(); }
                txtPTime.Text = item.PTIME.To<string>();
                chkOHMS.Checked = item.GBOHMS == "Y" ? true : false;
                chkSuchup.Checked = item.SUCHUPYN == "Y" ? true : false;

                lblHGigan.Text = item.PGIGAN_YY.To<string>() + "년 " + item.PGIGAN_MM.To<string>() + "개월";

                if (VB.Pstr(cboNational.Text, ".", 1) == "")
                {
                    cboNational.SelectedIndex = cboNational.FindStringExact("KR.대한민국");
                }

                if (cboGbSpc.Text.IsNullOrEmpty())
                {
                    cboGbSpc.SelectedIndex = 0;
                }

                if (!item.GBSPC.IsNullOrEmpty())
                {
                    for (int i = 0; i < cboGbSpc.Items.Count; i++)
                    {
                        if (VB.Left(cboGbSpc.Items[i].ToString(), 2) == item.GBSPC.To<string>())
                        {
                            cboGbSpc.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 접수번호별 수납 집계
        /// </summary>
        /// <param name="nGWRTNO"></param>
        private HIC_SUNAP Account_Sunap(long nWRTNO, string argGBN)
        {
            HIC_SUNAP item = new HIC_SUNAP();
            long nSeqNo = 0;
            if (argGBN == "HIC")
            {
                item = hicSunapService.GetHicSunapAmtByWRTNO(nWRTNO);
                nSeqNo = hicSunapService.GetMaxSeqbyWrtNo(nWRTNO);
                item.HALINGYE = hicSunapService.GetHalinGyeByWrtnoSeqNo(nWRTNO, nSeqNo - 1);
            }
            else if (argGBN == "HEA")
            {
                item = hicSunapService.GetHeaSunapAmtByWRTNO(nWRTNO);
            }
            
            return item;
        }

        /// <summary>
        /// 신규접수 화면 모드
        /// </summary>
        private void Display_Jepsu_New(List<string> lstAmChk = null)
        {
            string strDate =cboYear.Text + "-01-01";

            HIC_IE_MUNJIN_NEW iHIMN = hicIeMunjinNewService.GetItembyPtNoMunDate(txtPtno.Text, strDate);

            if (!iHIMN.IsNullOrEmpty())
            {
                FnIEMunNo = iHIMN.WRTNO;
                lblIEMunjin.BackColor = Color.LightSalmon;
                lblIEMunjin.Text = "인터넷문진";
                IEMunjin_Name_Display(iHIMN.RECVFORM);
            }

            //일반, 종검 가접수내역 건수 조회
            List<HIC_JEPSU_HEA_EXJONG> Jep = hicJepsuHeaExjongService.GetListGaJepsuByPtnoYear(txtPtno.Text, cboYear.Text);

            tbCtrl_JONG.Enabled = true;
            tbCtrl_Exam.Enabled = true;
            panSelExam.Enabled = true;

            if (!Jep.IsNullOrEmpty() && Jep.Count > 0)
            {
                //가접수 상세내역 Display
                if (Jep.Count > 0)
                {
                    //각 접수내역 상세 Detail
                    Display_JepsuWork_Detail(Jep);
                }

                clsPublic.GstrMsgList = "가접수 상태입니다." + ComNum.VBLF;
                clsPublic.GstrMsgList += "즉시접수는 예(Y), 자격확인 후 접수는 아니오(N)를 누르세요.";

                if (MessageBox.Show(clsPublic.GstrMsgList,"확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }

                for (int i = 0; i < Jep.Count; i++)
                {
                    switch (i)
                    {
                        case 0: rdoSTS1B.Checked = true; break;
                        case 1: rdoSTS2B.Checked = true; break;
                        case 2: rdoSTS3B.Checked = true; break;
                        case 3: rdoSTS4B.Checked = true; break;
                        case 4: rdoSTS5B.Checked = true; break;
                        default:
                            break;
                    }
                    
                }

                //저장버튼 클릭 구현하기
                Application.DoEvents();
                btnSave.PerformClick();
            }
            else
            {
                //신규접수 화면 세팅
                Set_Display_Jepsu(lstAmChk);
            }
        }

        private void Set_Display_Jepsu(List<string> lstAmChk = null)
        {
            string strChkOK = string.Empty;
            string strJong = string.Empty;

            List<string> lstCodes = new List<string>();


            //이전년도 접수경우를 위해 주석처리
            //cboYear.SelectedIndex = cboYear.FindStringExact(DateTime.Now.Year.To<string>());

            tbCtrl_JONG.Tabs[0].Visible = true;
            tbCtrl_JONG.SelectedTabIndex = 0;
            tbCtrl_JONG.Tabs[0].Text = "";
            tbCtrl_Exam.Tabs[0].Visible = true;
            panSelExam.Enabled = true;



            if (!chkNoNhic.Checked)
            {
                strJong = tbCtrl_JONG.Tabs[0].Text;
                //자격조회
                if (!txtSName.Text.IsNullOrEmpty())
                {
                    Hic_Chk_Nhic("H", txtSName.Text, txtJumin1.Text + txtJumin2.Text, txtPtno.Text, cboYear.Text);
                }
            }



            //암예약 그룹코드가 있다면 미리 세팅
            if (!lstAmChk.IsNullOrEmpty() && lstAmChk.Count > 0)
            {
                if (lstAmChk[0].To<string>("") == "Y")  { strChkOK += "UGI,";      chkAm1.Checked = true; lstCodes.Add("3112"); }
                if (lstAmChk[1].To<string>("") == "Y")  { strChkOK += "GFS,";      chkAm2.Checked = true; lstCodes.Add("3111"); }
                if (lstAmChk[3].To<string>("") == "Y")  { strChkOK += "유방,";     chkAm5.Checked = true; lstCodes.Add("3123"); }
                if (lstAmChk[4].To<string>("") == "Y")  { strChkOK += "CT,";       chkAm7.Checked = true; lstCodes.Add("3169"); }
                if (lstAmChk[5].To<string>("") == "Y")  { strChkOK += "분변,";     chkAm3.Checked = true; lstCodes.Add("3116"); }
                if (lstAmChk[7].To<string>("") == "Y")  { strChkOK += "SONO,";     chkAm4.Checked = true; lstCodes.Add("3115"); }
                if (lstAmChk[8].To<string>("") == "Y")  { strChkOK += "자궁경부,"; chkAm6.Checked = true; lstCodes.Add("3132"); }
                if (lstAmChk[10].To<string>("") == "Y") { strChkOK += "CT사후상담,"; lstCodes.Add("3170"); }
                //if (lstAmChk[2].To<string>("") == "Y") { strChkOK += "GFS(종검),"; lstCodes.Add(""); }      //GFS(종검)
                //if (lstAmChk[6].To<string>("") == "Y") { strChkOK += "자궁경부,"; lstCodes.Add("3132"); }     //COLON
                strJong = "31";

                cboJONG1.SelectedIndex = cboJONG1.FindString("31.암검진");
                tbCtrl_JONG.Tabs[0].Text = strJong;
                tbCtrl_Exam.Tabs[0].Text = strJong;

                //검진종류 기본부담율
                FstrBuRate[0] = hicExjongService.GetBuRateByGjJong(strJong).PadLeft(2, '0');

                Clear_Working_Variants();

                suInfo0.AddRange(Read_Jong_GroupCode(strJong, dtpJepDate.Text));
                grpExam0.AddRange(Read_ExamCode_Work(suInfo0));



                clsHcType.THC[0].GJJONG = strJong;

                cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[0].PadLeft(2, '0'), 0);
                Gesan_Sunap_Amt(0, lstsuInfo(0), FstrBuRate[0], 0);

                Clear_Spread_GroupCode();

                ssGroup.SetDataSource(lstsuInfo(0));
                ssExam.SetDataSource(lstgrpExam(0));

                frmHcGroupCode_ssDblclick(lstCodes, "");

                txtRemark1.Text = txtRemark.Text;
                txtRemark.Text = "";

                if (!strChkOK.IsNullOrEmpty())
                {
                    clsPublic.GstrMsgList = txtSName.Text + " 님의 검사는 [ " + strChkOK + " ] 이며, " + ComNum.VBLF + ComNum.VBLF;
                    clsPublic.GstrMsgList += " 참고사항은 [ " + txtRemark1.Text + " ] 입니다.";
                    MessageBox.Show(clsPublic.GstrMsgList, "확인 후 접수요망!");
                }
            }

            //if (!chkNoNhic.Checked)
            //{
            //    strJong = tbCtrl_JONG.Tabs[0].Text;
            //    //자격조회
            //    if (!txtSName.Text.IsNullOrEmpty() && strJong != "")
            //    {
            //        Hic_Chk_Nhic("H", txtSName.Text, txtJumin1.Text + txtJumin2.Text, txtPtno.Text, cboYear.Text);
            //    }
            //}

            //자격에 따른 부담율 세부 설정 
            //자궁경부암 검사만 있을경우 부담율 조합100% 세팅
            if (chkAm3.Checked || chkAm6.Checked)
            {
                if (!chkAm1.Checked && !chkAm2.Checked && !chkAm4.Checked && !chkAm5.Checked && !chkAm7.Checked)
                {
                    FstrBuRate[0] = "01";
                    cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[0].PadLeft(2, '0'), 0);
                }
            }

            //보건소암 있을경우 무료암대상으로 표시
            if (clsHcType.THNV.hBogen.To<string>("").Trim() != "")
            {
                if (VB.Left(clsHcType.THNV.hGKiho, 1) == "9")
                {
                    FstrBuRate[0] = "11";    //보건소 100%
                    cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[0].PadLeft(2, '0'), 0);
                    chkGubAm.Checked = true;
                    cboMuRyoAm.SelectedIndex = 1;  //N
                }
                else
                {
                    FstrBuRate[0] = "12";    //조합90%,보건소10%
                    cboBuRate.SelectedIndex = cboBuRate.FindString(FstrBuRate[0].PadLeft(2, '0'), 0);
                    cboMuRyoAm.SelectedIndex = 1;   //Y
                }
            }

        }

        private List<GROUPCODE_EXAM_DISPLAY> Read_ExamCode_Work(List<READ_SUNAP_ITEM> suInfo)
        {
            List<GROUPCODE_EXAM_DISPLAY> item = new List<GROUPCODE_EXAM_DISPLAY>();
            List<string> strGrpCode = new List<string>();

            //제외한 그룹코드가 아닌 코드만
            for (int i = 0; i < suInfo.Count; i++)
            {
                if (suInfo[i].RowStatus != RowStatus.Delete)
                {
                    strGrpCode.Add(suInfo[i].GRPCODE);
                }
            }

            if (strGrpCode.Count > 0)
            {
                item = groupCodeExamDisplayService.GetHicListByGroupCode(strGrpCode);
            }

            //기존 검사코드 비교 중복검사 제거
            Remove_Overlap_ExamList2(ref item);

            return item;
        }

        private List<READ_SUNAP_ITEM> Read_SunapWork_GroupCode(long argPano, string argJong, int nIdx)
        {
            List<READ_SUNAP_ITEM> tmpRSI = new List<READ_SUNAP_ITEM>();
            List<READ_SUNAP_ITEM> rtnRSI = new List<READ_SUNAP_ITEM>();
            List<GROUPCODE_EXAM_DISPLAY> dupGED = new List<GROUPCODE_EXAM_DISPLAY>();
            List<GROUPCODE_EXAM_DISPLAY> lstGED = new List<GROUPCODE_EXAM_DISPLAY>();
            List<string> lstgrpCode = new List<string>();


            tmpRSI = readSunapItemService.GetHicSunapWorkInfoByWrtno(argPano, argJong);

            //선택코드 재정렬
            var newList = tmpRSI.OrderBy(x => x.HANG).ToList();

            for (int i = 0; i < newList.Count; i++)
            {
                //선택한 코드 적용
                READ_SUNAP_ITEM iRSI = readSunapItemService.GetItemByCode(newList[i].GRPCODE.To<string>("").Trim());

                if (newList.Count > 0)
                {
                    //누적된 그룹코드내 검사항목들
                    if (!rtnRSI.IsNullOrEmpty() && rtnRSI.Count > 0)
                    {
                        lstgrpCode.Clear();
                        for (int j = 0; j < rtnRSI.Count; j++)
                        {
                            lstgrpCode.Add(rtnRSI[j].GRPCODE);
                        }

                        if (lstgrpCode.Count > 0)
                        {
                            dupGED = groupCodeExamDisplayService.GetListExcodeByGrpCodeList(lstgrpCode);
                        }
                    }

                    //종검포폴 선택시 종검내시경 체크박스 표시
                    if (!iRSI.IsNullOrEmpty())
                    {
                        if (iRSI.GRPCODE.To<string>("").Trim() == "3151")
                        {
                            chkHeaEndo.Checked = true;
                        }

                        iRSI.GBSELF = newList[i].GBSELF;

                        //부담율 설정
                        GroupCode_BuRate_Set(argJong, ref iRSI, "WORK");

                        //선택 코드 금액 산정 
                        iRSI.AMT = Read_GrpCode_Amt(lstGED, iRSI.GRPCODE.Trim(), dtpJepDate.Text, dupGED);

                        rtnRSI.Add(iRSI);
                    }
                   
                }
            }

            return rtnRSI;
        }

        /// <summary>
        /// 수검자 정보 Display
        /// </summary>
        /// <param name="argPtno"></param>
        /// <param name="argYear"></param>
        private void Display_HicPatient_Info(string argPtno, string argYear)
        {
            HIC_PATIENT pat = hicPatientService.GetPatInfoByPtno(argPtno);

            if (!pat.IsNullOrEmpty())   //환자마스터 있음
            {
                FstrPtno = pat.PTNO;
                FnPano = pat.PANO;
                FstrBuildNo = pat.BUILDNO;

                panSub01.SetData(pat);
                panPAT.SetData(pat);
                panBohum.SetData(pat);
                panLTD.SetData(pat);

                string strJumin = clsAES.DeAES(pat.JUMIN2);

                txtJumin1.Text  = VB.Left(strJumin, 6);
                txtJumin2.Text  = VB.Right(strJumin, 7);
                //txtAge.Text     = ComFunc.AgeCalcEx(strJumin, dtpJepDate.Value.ToShortDateString()).To<string>();
                txtAge.Text = cHB.READ_HIC_AGE_GESAN2(strJumin).ToString();

                //개인정보동의
                chkPrvAgr.Checked = pat.GBPRIVACY != null ? true : false;
                if (chkPrvAgr.Checked) { lblPrvAgr.Text = pat.GBPRIVACY; }

                //정보활용동의
                string strAcceptDate = cHcMain.Read_Privacy_Accept(argYear, argPtno);
                if (!strAcceptDate.IsNullOrEmpty())
                {
                    chkInfoAgr.Checked = true;
                    lblInfoAgr.Text = strAcceptDate;
                }

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

                if (!pat.JIKJONG.IsNullOrEmpty()) { txtJikJong.Text = pat.JIKJONG; }
                if (txtJikJong.Text == "0" || txtJikJong.Text == ".")
                {
                    txtJikJong.Text = "";
                }
                else
                {
                    strCode = VB.Pstr(txtJikJong.Text, ".", 1);
                    if (strCode.Trim() != "") { txtJikJong.Text = strCode + "." + hicCodeService.GetNameByGubunCode("05", txtJikJong.Text.Trim()); }
                }

                if (!pat.GONGJENG.IsNullOrEmpty()) { txtGongjeng.Text = pat.GONGJENG; }
                if (txtGongjeng.Text == "0" || txtGongjeng.Text == ".")
                {
                    txtGongjeng.Text = "";
                }
                else
                {
                    strCode = VB.Pstr(txtGongjeng.Text, ".", 1);
                    if (strCode.Trim() != "") { txtGongjeng.Text = strCode + "." + hicCodeService.GetNameByGubunCode("A2", txtGongjeng.Text.Trim()); }
                }
                
                //lblKihoName.Text = hicCodeService.GetNameByGubunCode("18", txtKiho.Text.Trim());         //회사기호
                //lblBoName1.Text = hicCodeService.GetNameByGubunCode("25", txtBoKiho.Text.Trim());      //보건소

                txtLastDate1.Text = comHpcLibBService.GetLastDay1ByPtno(argPtno);       //최종검진(일반)
                txtLastDate2.Text = comHpcLibBService.GetLastDay2ByPtno(argPtno);       //최종검진(암)
                txtLastDate3.Text = comHpcLibBService.GetLastDay3ByPtno(argPtno);       //최종검진(종검)
                txtJongCNT.Text = comHpcLibBService.GetCountHeaByPtno(argPtno);         //종합검진 횟수

                if (pat.LTDCODE > 0) { txtFax.Text = hicLtdService.GetFaxNoByLtdCode(pat.LTDCODE); }

                if (VB.Pstr(cboNational.Text, ".", 1) == "")
                {
                    if (cboNational.Items.Contains("KR.대한민국"))
                    {
                        cboNational.SelectedIndex = cboNational.FindStringExact("KR.대한민국");
                    }
                }
                
                if (txtJikJong.Text.Trim() == "")
                {
                    txtJikJong.Text = "93009.";
                    txtJikJong.Text += hicCodeService.GetNameByGubunCode("05", "93009");
                }

                if (txtGongjeng.Text.Trim() == "")
                {
                    txtGongjeng.Text = "45027.";
                    txtGongjeng.Text += hicCodeService.GetNameByGubunCode("A2", "45027");
                }

                txtJisa.Text = pat.JISA.To<string>("").Trim();
                if (!pat.JISA.IsNullOrEmpty()) { txtJisa.Text += "." + hicCodeService.GetNameByGubunCode("21", pat.JISA.To<string>("").Trim()); }
                
                txtBoKiho.Text = pat.BOGUNSO.To<string>("").Trim();
                if (!pat.BOGUNSO.IsNullOrEmpty()) { txtBoKiho.Text += "." + hicCodeService.GetNameByGubunCode("25", pat.BOGUNSO.To<string>("").Trim()); }
                
                txtKiho.Text = pat.KIHO.To<string>("").Trim();
                if (!pat.KIHO.IsNullOrEmpty()) { txtKiho.Text += "." + hicCodeService.GetNameByGubunCode("18", pat.KIHO.To<string>("").Trim()); }

                //동의서 보기
                string[] strDeptCode = { "HR", "TO" };
                frmHcPermission.SetDisplay(pat.PTNO, cboYear.Text, dtpJepDate.Text, strDeptCode, txtSName.Text);
                //frmHcEmrPermission.SetDisplay(pat.PTNO, cboYear.Text, dtpJepDate.Text, strDeptCode);

            }
            



            panSub01.Enabled = false;
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
                //nRow = ssGroup.ActiveSheet.ActiveRowIndex;
                //nCol = ssGroup.ActiveSheet.ActiveColumnIndex;

                //if (nCol != 4) { return; }
                //if (ssGroup.ActiveSheet.Cells[nRow, nCol].Text.Length > 2) { return; }

                ////저장된 검진정보에 ADD
                //int nIdx = tbCtrl_Exam.SelectedTabIndex;
                ////부담율
                //string strBurate = VB.Pstr(cboBuRate.Text, ".", 1).To<string>(FstrBuRate[nIdx]);

                //Gesan_Sunap_Amt(nIdx, lstsuInfo(nIdx), strBurate, txtHalinAmt.Text.To<long>());

            }
        }

        public void PatHis_Value(string sPtNo, string sJepDate, string strYear)
        {
            FsHisPtNo = sPtNo;
            FsHisJepDate = sJepDate;
            FsHisYear = strYear;

            if (!FsHisPtNo.IsNullOrEmpty())
            {
                //cboYear.Text = VB.Left(FsHisJepDate, 4);
                cboYear.Text = FsHisYear;
                dtpJepDate.Text = FsHisJepDate;
                txtPtno.Text = FsHisPtNo;
                eTxtKeyDown(txtPtno, new KeyEventArgs(Keys.Enter));
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            if (sender == btnReset)
            {
                if (ComFunc.MsgBoxQ("방금저장한 환자를 다시 불러올까요?", "", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    txtPtno.Text = FstrPtnoOld;
                    eTxtKeyDown(txtPtno, new KeyEventArgs(Keys.Enter));
                }
            }
            else if (sender == btnSearch_His)
            {
                frmHcPanPersonResult frm = new frmHcPanPersonResult("frmHcJepMain", txtPtno.Text, txtSName.Text);
                frm.ShowDialog();

                if (!FsHisPtNo.IsNullOrEmpty())
                {
                    //cboYear.Text = VB.Left(FsHisJepDate, 4);
                    cboYear.Text = FsHisYear;
                    dtpJepDate.Text = FsHisJepDate;
                    txtPtno.Text = FsHisPtNo;
                    eTxtKeyDown(txtPtno, new KeyEventArgs(Keys.Enter));
                }
            }
            else if (sender == btnSave)
            {
                FstrPtnoOld = txtPtno.Text;
                Data_Save_Process();    //저장루틴
                return;
            }
            #region 자격조회 버튼
            else if (sender == btnSearch_Nhic)
            {
                if (!txtSName.Text.IsNullOrEmpty())
                {

                    int nIdx = tbCtrl_Exam.SelectedTabIndex;
                    SS10.ActiveSheet.Cells[4, 1].BackColor = Color.FromArgb(227, 241, 255);

                    //Clear_Nhic_List();
                    Hic_Chk_Nhic("H", txtSName.Text, txtJumin1.Text + txtJumin2.Text, txtPtno.Text, cboYear.Text, nIdx, "Y");
                }

                return;
            }
            else if (sender == btnNhic_New)
            {
                SS10.ActiveSheet.Cells[4, 1].BackColor = Color.FromArgb(227, 241, 255);
                Clear_Nhic_List();
            }
            //2020년 자격조회버튼
            else if (sender == btnNhic_2020)
            {
                int nIdx = tbCtrl_Exam.SelectedTabIndex;
                SS10.ActiveSheet.Cells[4, 1].BackColor = Color.LightPink;
                Hic_Chk_Nhic("H", txtSName.Text, txtJumin1.Text + txtJumin2.Text, txtPtno.Text, "2020", nIdx, "Y");
            }

            #endregion
            #region 선택검사 버튼
            else if (sender == btnGroupCode_Spc)
            {
                int nIdx = tbCtrl_Exam.SelectedTabIndex;
                //2020-09-09
                txtCardAmt.Text = "";
                Hic_GroupCode_Help(FstrJong, "MCODE", RtnCurrCodeList(nIdx));
                return;
            }
            else if (sender == btnGroupCode_Sel)
            {
                int nIdx = tbCtrl_Exam.SelectedTabIndex;
                //2020-09-09
                txtCardAmt.Text = "";
                Hic_GroupCode_Help(FstrJong, "", RtnCurrCodeList(nIdx));
                return;
            }
            else if (sender == btnGroupCode_Ltd)    //회사별취급물질
            {
                int nIdx = tbCtrl_Exam.SelectedTabIndex;

                //2021-01-25
                ssGroup.ActiveSheet.RowCount = 0;
                ssExam.ActiveSheet.RowCount = 0;

                //2020-09-09
                txtCardAmt.Text = "";

                frmHcLtdUcodeHelp.rSetGstrValue += new frmHcLtdUcodeHelp.SetGstrValue(frmHcLtdUCodeHelp_btnSelect);
                frmHcLtdUcodeHelp f = new frmHcLtdUcodeHelp(VB.Pstr(txtLtdCode.Text, ".", 1), clsHcType.THC[nIdx].GJJONG, clsHcType.THC[nIdx].UCODES);
                f.ShowDialog();
                frmHcLtdUcodeHelp.rSetGstrValue -= new frmHcLtdUcodeHelp.SetGstrValue(frmHcLtdUCodeHelp_btnSelect);
                return;
            }
            #endregion
            #region 우편번호
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
            else if (sender == btnHelp_Mail2)
            {
                Post_Code_Help("2");
                return;
            }
            else if (sender == btnHelp_Mail3)
            {
                Post_Code_Help("3");
                return;
            }
            else if (sender == btnHelp_Mail4)
            {
                Post_Code_Help("4");
                return;
            }
            else if (sender == btnHelp_Mail5)
            {
                Post_Code_Help("5");
                return;
            }
            #endregion
            #region 사업장코드찾기
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
                return;
            }
            #endregion
            #region 현직종 찾기
            else if (sender == btnJikjong)
            {
                Hic_Code_Help("05", txtJikJong); //직종
                return;
            }
            #endregion
            #region 현공정 찾기
            else if (sender == btnGongjeng)
            {
                Hic_Code_Help("A2", txtGongjeng); //공정
                return;
            }
            #endregion
            else if (sender == btnJisa)
            {
                Hic_Code_Help("21", txtJisa); //지사
                return;
            }
            else if (sender == btnBogenso)
            {
                Hic_Code_Help("25", txtBoKiho); //보건소
                return;
            }
            else if (sender == btnKiho)
            {
                Hic_Code_Help("18", txtKiho); //사업장기호
                return;
            }
            #region 수검자 메모저장
            else if (sender == btnSave_Memo)
            {
                Hic_Memo_Save();
                return;
            } 
            #endregion
            else if (sender == btnCancel)
            {
                Screen_Clear();
                return;
            }
            else if (sender == btnAmt)
            {
                //저장된 검진정보에 ADD
                int nIdx = tbCtrl_Exam.SelectedTabIndex;
                string strBurate = VB.Pstr(cboBuRate.Text, ".", 1).To<string>(FstrBuRate[nIdx]);
                FstrBuRate[nIdx] = VB.Pstr(cboBuRate.Text, ".", 1).Trim();
                string strHalinCode = VB.Pstr(cboHalinGye.Text, ".", 1).Trim();

                //수납금액 계산하기
                Gesan_Sunap_Amt(nIdx, lstsuInfo(nIdx), strBurate, txtHalinAmt.Text.Replace(",", "").To<long>(), strHalinCode);

                //가접수호출 후 계산기 할인금액 있을경우 팝업 표시
                HIC_SUNAP_WORK item = hicSunapWorkService.Read_Hic_Sunap_Work(txtPano.Text.To<long>(0), clsHcType.THC[nIdx].GJJONG, cboYear.Text +"01-01", cboYear.Text + "12-31");
                if (!item.IsNullOrEmpty())
                {
                    if (item.HALINAMT > 0)
                    {
                        ComFunc.MsgBox("가접수시 할인금액이 " + item.HALINAMT + "원 입니다.", "작업완료");
                    }
                }
                

                return;
            }
            else if (sender == btnCard)
            {
                //저장된 검진정보에 ADD
                int nIdx = tbCtrl_Exam.SelectedTabIndex;

                CardApprov_Amt("CARD", nIdx);
                return;
            }
            else if (sender == btnCash)
            {
                //저장된 검진정보에 ADD
                int nIdx = tbCtrl_Exam.SelectedTabIndex;

                CardApprov_Amt("CASH", nIdx);
                return;
            }
            else if (sender == btnJongClose01)
            {
                JongPanel_Close(btnJongClose01);
            }
            else if (sender == btnJongClose02)
            {
                JongPanel_Close(btnJongClose02);
            }
            else if (sender == btnJongClose03)
            {
                JongPanel_Close(btnJongClose03);
            }
            else if (sender == btnJongClose04)
            {
                JongPanel_Close(btnJongClose04);
            }
            else if (sender == btnJongClose05)
            {
                JongPanel_Close(btnJongClose05);
            }
            else if (sender == btnWaitReg)
            {
                int nIdx = tbCtrl_JONG.SelectedTabIndex;
                Wait_Seq_Register(nIdx);
                return;
            }
            else if (sender == btnXConfirm)
            {
                if (txtPtno.Text.Trim() == "") { return; }

                HIC_JEPSU nHJ = Jepsu_Data_Build(tbCtrl_JONG.SelectedTabIndex);

                long nLtdCOde = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                HIC_LTD nHL = hicLtdService.GetIetmbyCode(nLtdCOde);

                frmHcConfirmation frm = new frmHcConfirmation(nHJ, nHL, lblUCODES.Text);
                frm.Show();
                return;
            }
            else if (sender == btnAgree)
            {
                //frmHaConsentApproval f = new frmHaConsentApproval();
                frmHcConsentformView f = new frmHcConsentformView();
                f.Show();
            }
            else if (sender == btnXBarCode)
            {
                int nIdx = tbCtrl_JONG.SelectedTabIndex;

                HIC_JEPSU nHJ = Jepsu_Data_Build(nIdx);

                BarCode_Print(lstgrpExam(nIdx), nIdx, nHJ, lblBMI.Text);
            }

            else if (sender == btnXJepsu)
            {
                int nIdx = tbCtrl_JONG.SelectedTabIndex;

                HIC_JEPSU nHJ = Jepsu_Data_Build(nIdx);

                HicJepsuPaperSet(lstsuInfo(nIdx), "재출력","");

                Pano_Print_Jepsu(nHJ, FstrEndo, FstrExamName, FstrName);
            }
            //수동출력
            else if (sender == btnXJepsu_Add)
            {
                string strGbPrint = "";

                int nIdx = tbCtrl_JONG.SelectedTabIndex;

                HIC_JEPSU nHJ = Jepsu_Data_Build(nIdx);


                for (int i = 1; i <= 12; i++)
                {

                    CheckBox chkPrt = (Controls.Find("chkPrt" + (i).ToString(), true)[0] as CheckBox);
                    if (chkPrt.Checked == true)
                    {
                        strGbPrint += "1" + ",";
                    }
                    else
                    {
                        strGbPrint += "0" + ","; ;
                    }
                }
                HicJepsuPaperSet(lstsuInfo(nIdx), "",strGbPrint);

                Pano_Print_Jepsu(nHJ, FstrEndo, FstrExamName, FstrName);
            }
            else if (sender == btnXRecReport)
            {
                string strJong = tbCtrl_JONG.SelectedTab.Text;

                if (MessageBox.Show(strJong + "종 영수증을 발생하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    HIC_JEPSU nHJ = Jepsu_Data_Build(tbCtrl_JONG.SelectedTabIndex);
                    HIC_SUNAP hSP = hicSunapService.GetHicSunapAmtByWRTNO(nHJ.WRTNO);

                    cHPrt.Receipt_Report_Print(nHJ, hSP);
                    return;
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
            else if (sender == btnLifeTool)
            {
                long nWRTNO = clsHcType.THC[tbCtrl_JONG.SelectedTabIndex].WRTNO.To<long>(0); 

                frmHcLifeTools frm = new frmHcLifeTools("접수", nWRTNO, dtpJepDate.Text, txtPano.Text.To<long>(0), txtAge.Text.To<long>(0));
                frm.ShowDialog();
            }
        }

        void elblMouseUp(object sender, MouseEventArgs e)
        {
            toolTip.SetToolTip((Control)sender, ((Control)sender).Text.ToString());
        }

        /// <summary>
        /// 현 Spread 작업중인 그룹코드 List 반환
        /// </summary>
        /// <param name="nIdx"></param>
        /// <returns></returns>
        private List<READ_SUNAP_ITEM> RtnCurrCodeList(int nIdx)
        {
            List<READ_SUNAP_ITEM> rtnLst = new List<READ_SUNAP_ITEM>();

            //현재 작업중인 그룹코드 List중 유해물질이거나 추가검사 코드인 경우만 List로 반환
            for (int i = 0; i < lstsuInfo(nIdx).Count; i++)
            {   //제외처리 한 코드는 제외
                if (lstsuInfo(nIdx)[i].RowStatus != RowStatus.Delete)
                {
                    rtnLst.Add(lstsuInfo(nIdx)[i]);
                }
            }

            return rtnLst;
        }

        private void JongPanel_Close(Button btnJong)
        {
            if (VB.Left(btnJong.Name, 12) == "btnJongClose")
            {
                switch (btnJong.Name.Substring(12, 2))
                {
                    case "01":
                        tbJongPage1.Visible = false; tbExamPage1.Visible = false; tbPanel1.Initialize();
                        tbCtrl_JONG.SelectedTabIndex = 0;
                        tbCtrl_Exam.SelectedTabIndex = 0;
                        clsHcType.THC[0].GBUSE = null;
                        cboJONG1.Enabled = true;
                        Clear_Working_Variants(0);
                        break;
                    case "02":
                        tbJongPage2.Visible = false; tbExamPage2.Visible = false; tbPanel2.Initialize();
                        tbCtrl_JONG.SelectedTabIndex = 0;
                        tbCtrl_Exam.SelectedTabIndex = 0;
                        clsHcType.THC[1].GBUSE = null;
                        cboJONG2.Enabled = true;
                        Clear_Working_Variants(1);
                        break;
                    case "03":
                        tbJongPage3.Visible = false; tbExamPage3.Visible = false; tbPanel3.Initialize();
                        tbCtrl_JONG.SelectedTabIndex = 1;
                        tbCtrl_Exam.SelectedTabIndex = 1;
                        clsHcType.THC[2].GBUSE = null;
                        cboJONG3.Enabled = true;
                        Clear_Working_Variants(2);
                        break;
                    case "04":
                        tbJongPage4.Visible = false; tbExamPage4.Visible = false; tbPanel4.Initialize();
                        tbCtrl_JONG.SelectedTabIndex = 2;
                        tbCtrl_Exam.SelectedTabIndex = 2;
                        clsHcType.THC[3].GBUSE = null;
                        cboJONG4.Enabled = true;
                        Clear_Working_Variants(3);
                        break;
                    case "05":
                        tbJongPage5.Visible = false; tbExamPage5.Visible = false; tbPanel5.Initialize();
                        tbCtrl_JONG.SelectedTabIndex = 3;
                        tbCtrl_Exam.SelectedTabIndex = 3;
                        clsHcType.THC[4].GBUSE = null;
                        cboJONG5.Enabled = true;
                        Clear_Working_Variants(4);
                        break;
                    default: break;
                }

                int nVisible = 0;
                for (int i = 0; i < tbCtrl_JONG.Tabs.Count; i++)
                {
                    if (tbCtrl_JONG.Tabs[i].Visible == true)
                    {
                        nVisible++;
                    }
                }

                if (nVisible == 1)
                {
                    Clear_Spread_GroupCode();
                }
                
                

                //for (int i = 0; i < tbCtrl_Exam.Tabs.Count; i++)
                //{
                //    if (tbCtrl_Exam.Tabs[i].Visible == false)
                //    {
                //        tbCtrl_Exam.SelectedTabIndex = i;
                //        break;
                //    }
                //}
                return;
            }
        }

        /// <summary>
        /// 회사특검항목
        /// </summary>
        /// <param name="lstUCodes"></param>
        /// <param name="lstSExams"></param>
        private void frmHcLtdUCodeHelp_btnSelect(List<string> lstUCodes, List<string> lstSExams)
        {
            int nIdx = tbCtrl_Exam.SelectedTabIndex;

            clsHcType.THC[nIdx].UCODES = string.Join(",", lstUCodes.ToArray());
            lblUCODES.Text = cHcMain.UCode_Names_Display(clsHcType.THC[nIdx].UCODES);

            frmHcGroupCode_ssDblclick(lstUCodes, "MCODE");  //유해인자 그룹코드

            clsHcType.THC[nIdx].SEXAMS = string.Join(",", lstSExams.ToArray());

            frmHcGroupCode_ssDblclick(lstSExams, "");       //선택검사 그룹코드

        }

        private void Menual_Call_Process()
        {
            //실시간으로 대기원이 보이게 요청함
            //timer1.Enabled = false;

            //창띄움
            clsHcVariable.GstrTempValue = "";
            frmHcWaitList FrmHcWaitList = new frmHcWaitList();
            frmHcWaitList.rSetGstrValue += new frmHcWaitList.SetGstrValue(frmHcWaitList_ssDblclick);
            FrmHcWaitList.ShowDialog(this);
            frmHcWaitList.rSetGstrValue -= new frmHcWaitList.SetGstrValue(frmHcWaitList_ssDblclick);
        }

        private void frmHcWaitList_ssDblclick(string argTemp1, string argTemp2, string argTemp3)
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
                if (!hicWaitService.UpDateCallWaitPC(strPcNo, "1", "2", strName, nSeqNo))
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
                if (!hicWaitService.UpDateCallWaitPC(strPcNo, "1", "2", strName, nSeqNo))
                {
                    MessageBox.Show("호출대기등록 실패!", "오류");
                }
            }

            lblCall.Text = nSeqNo.To<string>("0");

            txtJumin2.Focus();

            eTxtKeyDown(txtJumin2, new KeyEventArgs(Keys.Enter));

            timer1.Enabled = true;
            btnCall.Enabled = true;

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
                if (!hicWaitService.UpDateReCallWaitPC(strPcNo, "1", "2"))
                {
                    MessageBox.Show("호출대기등록 실패!", "오류");
                }
            }
        }

        private void Wait_Seq_Register(int Ind)
        {
            frmWaitSeqReg frm = new frmWaitSeqReg(clsHcType.THC[Ind].WRTNO, FnPano);
            frm.ShowDialog();
        }

        private void Data_Save_Process()
        {
            bool bGaJepsuFlag = false;      //가접수 플래그
            bool bJepsuModifyFlag = false;  //접수수정 플래그
            bool bJepsuNew = false;         //신규접수 플래그


            string strJepsu = "";
            string strGJepsu = "";
            string strGbSTS = string.Empty;
            string strSpc = "";
            long nNightWrtno = 0;
            long nJinWrtno = 0;
            int result = 0;

            if (!panSub01.RequiredValidate())
            {
                MessageBox.Show("접수상단 필수 입력항목이 누락되었습니다.");
                return;
            }

            if (!panPAT.RequiredValidate())
            {
                MessageBox.Show("환자정보 필수 입력항목이 누락되었습니다.");
                return;
            }

            if (!panBuRate.RequiredValidate())
            {
                MessageBox.Show("부담율 필수 입력항목이 누락되었습니다.");
                return;
            }

            for (int i = 0; i < 6; i++)
            {
                if (clsHcType.THC[i].GBUSE == "D")
                {
                    if (MessageBox.Show(clsHcType.THC[i].GJJONG + " 종 접수 취소건이 있습니다. 진행하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }
            }

            //부담률확인(2021-04-22)
            for (int i = 0; i <= ssGroup.ActiveSheet.RowCount-1; i++)
            {
                if (ssGroup.ActiveSheet.Cells[i, 1].Text.Trim() == "1151" && !ssGroup.ActiveSheet.Cells[i, 4].Text.IsNullOrEmpty())
                {
                    if (ssGroup.ActiveSheet.Cells[i, 4].Text != "1" && ssGroup.ActiveSheet.Cells[i, 4].Text != "01")
                    {
                        if (ComFunc.MsgBoxQ("일반검진 상담료 부담율 오류입니다. 그래도 진행하시겠습니까?", "", MessageBoxDefaultButton.Button1) == DialogResult.No)
                        {
                            return;
                        }
                    }
                }
                else if (ssGroup.ActiveSheet.Cells[i, 1].Text.Trim() == "1151" && VB.Left(cboBuRate.Text, 2) != "01")
                {
                    if (ComFunc.MsgBoxQ("일반검진 상담료 부담율 오류입니다. 그래도 진행하시겠습니까?", "", MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return;
                    }
                }

                if (ssGroup.ActiveSheet.Cells[i, 1].Text.Trim() == "2302" && !ssGroup.ActiveSheet.Cells[i, 4].Text.IsNullOrEmpty())
                {
                    if (ssGroup.ActiveSheet.Cells[i, 4].Text == "1" || ssGroup.ActiveSheet.Cells[i, 4].Text == "01")
                    {
                        if (ComFunc.MsgBoxQ("일특진찰료 부담율 오류입니다. 그래도 진행하시겠습니까?", "", MessageBoxDefaultButton.Button1) == DialogResult.No)
                        {
                            return;
                        }

                    }
                }
                else if (ssGroup.ActiveSheet.Cells[i, 1].Text.Trim() == "2302" && VB.Left(cboBuRate.Text, 2) == "01")
                {
                    if (ComFunc.MsgBoxQ("일특진찰료 부담율 오류입니다. 그래도 진행하시겠습니까?", "", MessageBoxDefaultButton.Button1) == DialogResult.No)
                    {
                        return;
                    }
                }
            }

            #region 접수구분 설정
            //신규접수 인지 여부 파악, 가접수 수정인지, 신규 가접수인지 파악
            //U:  접수 수정   
            //N:  접수 신규
            //G:  가접수 수정
            //GN: 가접수 신규
            //GJ: 가접수 -> 접수
            for (int i = 0; i < 6; i++)
            {
                if (clsHcType.THC[i].GBUSE == "G")
                {
                    switch (i)
                    {   //가접수 -> 접수
                        case 0: if (rdoSTS1B.Checked)  { clsHcType.THC[i].GBUSE = "GJ"; } break;
                        case 1: if (rdoSTS2B.Checked)  { clsHcType.THC[i].GBUSE = "GJ"; } break;
                        case 2: if (rdoSTS3B.Checked)  { clsHcType.THC[i].GBUSE = "GJ"; } break;
                        case 3: if (rdoSTS4B.Checked)  { clsHcType.THC[i].GBUSE = "GJ"; } break;
                        case 4: if (rdoSTS5B.Checked)  { clsHcType.THC[i].GBUSE = "GJ"; } break;
                        default: break;
                    }
                }
                else if(clsHcType.THC[i].GBUSE == "")
                {
                    switch (i)
                    {   //가접수, 접수 신규
                        case 0: if (rdoSTS1A.Checked) { clsHcType.THC[i].GBUSE = "GN"; } else { clsHcType.THC[i].GBUSE = "N"; } break;
                        case 1: if (rdoSTS2A.Checked) { clsHcType.THC[i].GBUSE = "GN"; } else { clsHcType.THC[i].GBUSE = "N"; } break;
                        case 2: if (rdoSTS3A.Checked) { clsHcType.THC[i].GBUSE = "GN"; } else { clsHcType.THC[i].GBUSE = "N"; } break;
                        case 3: if (rdoSTS4A.Checked) { clsHcType.THC[i].GBUSE = "GN"; } else { clsHcType.THC[i].GBUSE = "N"; } break;
                        case 4: if (rdoSTS5A.Checked) { clsHcType.THC[i].GBUSE = "GN"; } else { clsHcType.THC[i].GBUSE = "N"; } break;
                        default: break;
                    }
                }
            }

            //가접수 접수 동시체크 점검로직
            for (int i = 0; i < 6; i++)
            {
                switch (i)
                {
                    case 0: if (rdoSTS1B.Checked) { strJepsu = "OK"; } break;
                    case 1: if (rdoSTS2B.Checked) { strJepsu = "OK"; } break;
                    case 2: if (rdoSTS3B.Checked) { strJepsu = "OK"; } break;
                    case 3: if (rdoSTS4B.Checked) { strJepsu = "OK"; } break;
                    case 4: if (rdoSTS5B.Checked) { strJepsu = "OK"; } break;
                    default: break;
                }

                switch (i)
                {   //가접수, 접수 신규
                    case 0: if (rdoSTS1A.Checked) { strGJepsu = "OK"; } break;
                    case 1: if (rdoSTS2A.Checked) { strGJepsu = "OK"; } break;
                    case 2: if (rdoSTS3A.Checked) { strGJepsu = "OK"; } break;
                    case 3: if (rdoSTS4A.Checked) { strGJepsu = "OK"; } break;
                    case 4: if (rdoSTS5A.Checked) { strGJepsu = "OK"; } break;
                    default: break;
                }
            }
            if (strJepsu =="OK" && strGJepsu =="OK")
            {
                if (ComFunc.MsgBoxQ("가접수/접수 동시진행하시겠습니까?", "", MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
            }

            #endregion

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                clsDB.setBeginTran(clsDB.DbCon);

                for (int i = 0; i < 5; i++) //종검접수는 제외
                {
                    FnWRTNO = 0;
                    if (!clsHcType.THC[i].GBUSE.IsNullOrEmpty())
                    {
                        //가접수 Flag 관리
                        bJepsuNew = (clsHcType.THC[i].GBUSE == "N" || clsHcType.THC[i].GBUSE == "GJ") ? true : false;
                        bGaJepsuFlag = (clsHcType.THC[i].GBUSE == "G" || clsHcType.THC[i].GBUSE == "GN") ? true : false;
                        bJepsuModifyFlag = clsHcType.THC[i].GBUSE == "U" ? true : false;

                        if (clsHcType.THC[i].GBUSE == "D")  //접수 취소루틴
                        {
                            #region 삭제 루틴
                            HIC_JEPSU item = hicJepsuService.GetItemByWRTNO(clsHcType.THC[i].WRTNO);

                            if (!item.IsNullOrEmpty())
                            {
                                //체크로직
                                if (cHMF.Delete_Check_Logic(item.WRTNO, item.XRAYNO) == false)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                //접수취소시 모든판정 삭제
                                if (cHMF.Delete_Panjeng_Data(item.WRTNO) == false)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                //접수마스타에 삭제 Flag SET
                                if (!hicJepsuService.UpDateDelDateGbMunJinByWrtno(item.WRTNO))
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                //인터넷문진 DB전송대상으로 초기화
                                if (!hicIeMunjinNewService.UpDateReset(item.IEMUNNO))
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                //종전의 영수증을 취소
                                if (!Report_OldAmt_Cancel(item, i))
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                //방문수령 내역 삭제
                                if (!hicCharttransPrintService.GetRowidByWrtno(item.WRTNO).IsNullOrEmpty())
                                {
                                    //Delete 
                                    if (!hicCharttransPrintService.Delete(item.WRTNO))
                                    {
                                        MessageBox.Show("결과지 수령방법 삭제시 오류가 발생함.", "오류");
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }

                                //차트인계 내역 삭제
                                if (!hicCharttransService.GetAllbyWrtno(item.WRTNO).IsNullOrEmpty())
                                {
                                    //Delete 
                                    if (hicCharttransService.DeleteData(item.WRTNO) < 0)
                                    {
                                        MessageBox.Show("차트인계내역 삭제시 오류가 발생함.", "오류");
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }

                                //내시경ORDER 삭제
                                if ( item.GJJONG == "31")
                                {
                                    List<string> lstSucode = new List<string> { "E7630", "E7630S" };
                                    if (comHpcLibBService.DeleteOcsOorders(item.PTNO, lstSucode, item.JEPDATE) < 0)
                                    {
                                        MessageBox.Show("31종 내시경처방 삭제시 오류가 발생함", "오류");
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }
                                
                                //PACS 오더 삭제
                                if (!item.XRAYNO.IsNullOrEmpty())
                                {
                                    //TODO : Error 처리
                                    if (!cHOS.HIC_PACS_SEND(item.PANO, item.XRAYNO, "CA"))
                                    {
                                        MessageBox.Show("XRAY 오더를 삭제 시 오류 발생", "오류");
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }

                                    //HIC_XRAY_RESULT 삭제일자마크
                                    if (!hicXrayResultService.UpDateDelDateByXrayNo(item.XRAYNO, item.PANO))
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }

                                //심리검사실에 취소오더전송
                                if (!hicResultService.Chk_Simli_ExCode(item.WRTNO).IsNullOrEmpty())
                                {
                                    if (!cHMF.SIMRI_ORDER_INSERT(item, 1))
                                    {
                                        MessageBox.Show("임상심리 오더를 전송시 오류 발생", "오류");
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }

                                COMHPC eOS = new COMHPC
                                {
                                    AGE = txtAge.Text.To<long>(0),
                                    WRTNO = FnWRTNO,
                                    PANO = txtPano.Text.To<long>(0),
                                    SDATE = dtpJepDate.Text,
                                    JEPDATE = dtpJepDate.Text,
                                    SNAME = txtSName.Text.Trim(),
                                    JUMIN = txtJumin1.Text + txtJumin2.Text,
                                    SEX = cboSex.Text,
                                    LTDCODE = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(),
                                    GJJONG = clsHcType.THC[i].GJJONG,
                                    PTNO = txtPtno.Text.Trim(),
                                    GOTOENDO = chkHeaEndo.Checked ? "Y" : "N",
                                    GBCHUL = chkChul.Checked ? "Y" : "N",
                                    DEPTCODE = "HR",
                                    DRCODE = "7101",
                                    JOBSABUN = 111,
                                };

                                //심장초음파/경동맥초음파 접수취소 (구. HIC_EKG_Send_NEW 변경)
                                if (!cHMF.Jin_Support_Data_Send(eOS, "9"))
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }


                                //내시경예약취소(2020-12-23)
                                result = endoJupmstService.UpDateGbsunapByPtnoRDate(item.PTNO, "*", "HR");
                                if (result < 0)
                                {
                                    MessageBox.Show("내시경접수취소 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                //TextEmr 자동 EMR Table INSERT
                                clsQuery.NEW_TextEMR_TreatInterface(clsDB.DbCon, item.PTNO, item.JEPDATE, "HR", "HR", "취소", "99916");
                            }

                            #endregion
                        }

                        else //INSERT / UPDATE 루틴
                        {
                            #region 접수체크로직

                            if (Jepsu_Check_Logic(i) == false)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                            #endregion

                            #region 개인정보 활용동의서 등록
                            if (lblPrvAgr.Text.Trim() != "")
                            {
                                if (cHcMain.Read_Privacy_Accept(cboYear.Text, txtPtno.Text) == "")
                                {
                                    if (!comHpcLibBService.InsertPrivacyAccept(cboYear.Text, txtPtno.Text, txtSName.Text))
                                    {
                                        MessageBox.Show("정보활용 동의서 등록시 오류가 발생함", "오류");
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }
                            }
                            #endregion

                            #region 수검자 마스터 정보 갱신
                            HIC_PATIENT uPat = Patient_Data_Build();

                            if (hicPatientService.UpDate(uPat) <= 0)
                            {
                                MessageBox.Show("환자마스타에 UPDATE 도중에 오류가 발생함", "오류");
                                clsDB.setRollbackTran(clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                            #endregion

                            //Global Wrtno 생성 (신규일때만)
                            if (bJepsuNew)
                            {

                                //HEA_JEPSU GWRTNO조회
                                //HEA_JEPSU item = heaJepsuService.Read_Jepsu3(txtPtno.Text, dtpJepDate.Text);
                                //if (!item.IsNullOrEmpty())
                                //{
                                //    GnWRTNO = item.GWRTNO;
                                //}

                                if (GnWRTNO == 0) { GnWRTNO = cHB.Read_New_JepsuGWrtNo(); }
                            }

                            HIC_JEPSU_WORK nHJW = Jepsu_Work_Data_Build(i);
                            HIC_JEPSU nHJ = Jepsu_Data_Build(i);

                            #region 접수 Data Insert / UpDate Main Routine
                            if (bGaJepsuFlag)
                            {
                                #region 가접수 루틴
                                if (clsHcType.THC[i].GBUSE == "GN")
                                {
                                    //가접수 신규등록
                                    if (hicJepsuWorkService.InsertAll(nHJW) <= 0)
                                    {
                                        MessageBox.Show("가접수 Data Insert 시 오류가 발생함", "오류");
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }
                                else
                                {
                                    nHJW.RID = hicJepsuWorkService.GetRowidByGJjongPtnoYear(nHJW.GJJONG, nHJW.PTNO, nHJW.GJYEAR);

                                    //가접수 수정
                                    if (hicJepsuWorkService.UpdateAllbyRowId(nHJW) <= 0)
                                    {
                                        MessageBox.Show("가접수 Data UpDate 시 오류가 발생함", "오류");
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                FnWRTNO = 0;
                                #region 접수 루틴
                                if (bJepsuModifyFlag)
                                {
                                    FnWRTNO = clsHcType.THC[i].WRTNO;   //접수수정
                                }
                                else
                                {
                                    FnWRTNO = nHJ.WRTNO;                //신규접수
                                }


                                #region 69종판정여부
                                if (!bJepsuModifyFlag && !bGaJepsuFlag)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            if (nHJ.GJJONG == "69")
                                            {
                                                if (MessageBox.Show("69종 추가판정을 하시겠습니까", "선택사항", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                                {
                                                    nHJ.GBADDPAN = "Y";
                                                    chk69Pan1.Checked = true;
                                                    cboJONG1.Enabled = false;
                                                }
                                                else
                                                {
                                                    nHJ.GBADDPAN = "N";
                                                    chk69Pan1.Checked = false;
                                                    cboJONG1.Enabled = false;
                                                }
                                            }
                                            break;
                                        case 1:
                                            if (nHJ.GJJONG == "69")
                                            {
                                                if (MessageBox.Show("69종 추가판정을 하시겠습니까", "선택사항", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                                {
                                                    nHJ.GBADDPAN = "Y";
                                                    chk69Pan2.Checked = true;
                                                    cboJONG2.Enabled = false;
                                                }
                                                else
                                                {
                                                    nHJ.GBADDPAN = "N";
                                                    chk69Pan2.Checked = false;
                                                    cboJONG2.Enabled = false;
                                                }
                                            }
                                            break;
                                        case 2:
                                            if (nHJ.GJJONG == "69")
                                            {
                                                if (MessageBox.Show("69종 추가판정을 하시겠습니까", "선택사항", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                                {
                                                    nHJ.GBADDPAN = "Y";
                                                    chk69Pan3.Checked = true;
                                                    cboJONG3.Enabled = false;
                                                }
                                                else
                                                {
                                                    nHJ.GBADDPAN = "N";
                                                    chk69Pan3.Checked = false;
                                                    cboJONG3.Enabled = false;
                                                }
                                            }
                                            break;
                                        case 3:
                                            if (nHJ.GJJONG == "69")
                                            {
                                                if (MessageBox.Show("69종 추가판정을 하시겠습니까", "선택사항", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                                {
                                                    nHJ.GBADDPAN = "Y";
                                                    chk69Pan4.Checked = true;
                                                    cboJONG4.Enabled = false;
                                                }
                                                else
                                                {
                                                    nHJ.GBADDPAN = "N";
                                                    chk69Pan4.Checked = false;
                                                    cboJONG4.Enabled = false;
                                                }
                                            }
                                            break;
                                        case 4:
                                            if (nHJ.GJJONG == "69")
                                            {
                                                if (MessageBox.Show("69종 추가판정을 하시겠습니까", "선택사항", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                                {
                                                    nHJ.GBADDPAN = "Y";
                                                    chk69Pan5.Checked = true;
                                                    cboJONG5.Enabled = false;
                                                }
                                                else
                                                {
                                                    nHJ.GBADDPAN = "N";
                                                    chk69Pan5.Checked = false;
                                                    cboJONG5.Enabled = false;
                                                }
                                            }
                                            break;
                                        default: break;
                                    }
                                }

                                #endregion

                                if (clsHcType.THC[i].GBUSE == "N" || clsHcType.THC[i].GBUSE == "GJ")
                                {
                                    //접수 신규등록
                                    if (!hicJepsuService.InsertAll(nHJ))
                                    {
                                        MessageBox.Show("접수 Data Insert 시 오류가 발생함", "오류");
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }

                                    //가접수 삭제
                                    nHJW.RID = hicJepsuWorkService.GetRowidByGJjongPtnoYear(nHJW.GJJONG, nHJW.PTNO, nHJW.GJYEAR);

                                    if (!nHJW.RID.IsNullOrEmpty())
                                    {
                                        if (!hicJepsuWorkService.DeleteByRowid(nHJW.RID))
                                        {
                                            MessageBox.Show("가접수 Data Delete 시 오류가 발생함", "오류");
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            Cursor.Current = Cursors.Default;
                                            return;
                                        }
                                    }

                                }
                                else if (bJepsuModifyFlag)
                                {
                                    //접수 수정
                                    if (!hicJepsuService.UpDateAll(nHJ))
                                    {
                                        MessageBox.Show("접수 Data UpDate 시 오류가 발생함", "오류");
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }

                                //문진 대상항목 설정
                                if (!cHcMain.Munjin_ITEM_SET(nHJ.WRTNO))
                                {
                                    MessageBox.Show("문진 대상항목 UpDate 시 오류가 발생함", "오류");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                //2차인경우 1차판정의사 Update(2차판정은 1차 판정의사가 하기 위해)
                                if (!cHcMain.UPDATE_FirstPanjeng_DrNo(nHJ.WRTNO))
                                {
                                    MessageBox.Show("2차검진자 1차판정의사 면허번호 UpDate 시 오류가 발생함", "오류");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                #endregion
                            }
                            #endregion

                            if (FnWRTNO > 0)
                            {
                                if (!Sunap_DTL_INSERT(FnWRTNO, lstsuInfo(i), FstrBuRate[i]))  //검진항목을 UPDATE
                                {
                                    MessageBox.Show("SUNAPDTL Insert 시 오류가 발생함", "오류");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                if (!Sunap_Result_UPDATE(FnWRTNO, lstgrpExam(i)))      //검진결과 항목을 UPDATE
                                {
                                    MessageBox.Show("HIC_RESULT Data 정리중 오류가 발생함", "오류");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }
                            else
                            {
                                if (!SunapDTL_WORK_INSERT(txtPano.Text.To<long>(), clsHcType.THC[i].GJJONG, lstsuInfo(i), FstrBuRate[i], dtpJepDate.Text))       //가접수할때 수납금액계산 및 저장
                                {
                                    MessageBox.Show("HIC_SUNAPDTL_WORK 작업중 오류가 발생함", "오류");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;

                                }

                                if (!Report_NewAmt_Print_WORK(txtPano.Text.To<long>(), dtpJepDate.Text, clsHcType.THC[i].GJJONG, dSunap(i)))
                                {
                                    MessageBox.Show("HIC_SUNAP_WORK 작업중 오류가 발생함", "오류");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }

                            #region Xray, Endo 오더 전송
                            //본인부담금의 20%를 회사로 청구하기 때문에 건진 오더가 내시경실에서 보여야함.
                            //2015-12-03 내원검진은 무조건 오더 전달이 되도록 변경함(가접수는 제외)
                            if (!chkChul.Checked && FnWRTNO > 0)
                            {
                                COMHPC eOS = new COMHPC
                                {
                                    AGE = txtAge.Text.To<long>(0),
                                    WRTNO = FnWRTNO,
                                    PANO = txtPano.Text.To<long>(0),
                                    SDATE = dtpJepDate.Text,
                                    JEPDATE = dtpJepDate.Text,
                                    SNAME = txtSName.Text.Trim(),
                                    JUMIN = txtJumin1.Text + txtJumin2.Text,
                                    SEX = cboSex.Text,
                                    LTDCODE = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(),
                                    GJJONG = clsHcType.THC[i].GJJONG,
                                    PTNO = txtPtno.Text.Trim(),
                                    GOTOENDO = chkHeaEndo.Checked ? "Y" : "N",
                                    GBCHUL = chkChul.Checked ? "Y" : "N",
                                };

                                string strXray = "";

                                switch (i)
                                {
                                    case 0: strXray = chkXray1.Checked ? "본관" : ""; break;
                                    case 1: strXray = chkXray2.Checked ? "본관" : ""; break;
                                    case 2: strXray = chkXray3.Checked ? "본관" : ""; break;
                                    case 3: strXray = chkXray4.Checked ? "본관" : ""; break;
                                    case 4: strXray = chkXray5.Checked ? "본관" : ""; break;
                                    default: break;
                                }

                                if (!cHOS.EXAM_ORDER_SEND(eOS, "HR", strXray))
                                {
                                    MessageBox.Show("Xray Order 전송중 오류가 발생함", "오류");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }
                            #endregion
                            
                            #region 심전도실 오더를 전송
                            if (!bGaJepsuFlag)
                            {
                                COMHPC eOS = new COMHPC
                                {
                                    AGE = txtAge.Text.To<long>(0),
                                    WRTNO = FnWRTNO,
                                    PANO = txtPano.Text.To<long>(0),
                                    SDATE = dtpJepDate.Text,
                                    JEPDATE = dtpJepDate.Text,
                                    SNAME = txtSName.Text.Trim(),
                                    JUMIN = txtJumin1.Text + txtJumin2.Text,
                                    SEX = cboSex.Text,
                                    LTDCODE = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(),
                                    GJJONG = clsHcType.THC[i].GJJONG,
                                    PTNO = txtPtno.Text.Trim(),
                                    GOTOENDO = chkHeaEndo.Checked ? "Y" : "N",
                                    GBCHUL = chkChul.Checked ? "Y" : "N",
                                    DEPTCODE = "HR",
                                    DRCODE = "7101",
                                    JOBSABUN = 111,
                                };

                                if (!cHMF.Jin_Support_Data_Send(eOS, "1"))
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }
                            #endregion

                            #region EMR 차트번호 만들기
                            for (int j = 0; j < lstgrpExam(i).Count; j++)
                            {
                                if (lstgrpExam(i)[j].EXCODE.Trim() == "TZ16")   //뇌혈류초음파
                                {
                                    clsQuery.NEW_TextEMR_TreatInterface(clsDB.DbCon, txtPtno.Text, dtpJepDate.Text, "HR", "HR", "정상", "99916");
                                    break;
                                }
                            }
                            #endregion

                            HicJepsuPaperSet(lstsuInfo(i),"","");     //접수증 세팅

                            #region 수납금액을 등록 및 영수증 인쇄  CmdOK_Sunap_Amt_Update
                            if (clsHcType.THC[i].GBUSE == "U")
                            {
                                string strSuUpDate = "NO";
                                HIC_SUNAP bSunap = hicSunapService.GetHicSunapAmtByWRTNO(FnWRTNO);

                                if (dSunap(i).TOTAMT != bSunap.TOTAMT) { strSuUpDate = "OK"; }
                                if (dSunap(i).JOHAPAMT != bSunap.JOHAPAMT) { strSuUpDate = "OK"; }
                                if (dSunap(i).LTDAMT != bSunap.LTDAMT) { strSuUpDate = "OK"; }
                                if (dSunap(i).BONINAMT != bSunap.BONINAMT) { strSuUpDate = "OK"; }
                                if (dSunap(i).BOGENAMT != bSunap.BOGENAMT) { strSuUpDate = "OK"; }
                                if (dSunap(i).MISUAMT != bSunap.MISUAMT) { strSuUpDate = "OK"; }
                                if (dSunap(i).HALINAMT != bSunap.HALINAMT) { strSuUpDate = "OK"; }
                                if (dSunap(i).SUNAPAMT != bSunap.SUNAPAMT) { strSuUpDate = "OK"; }
                                if (dSunap(i).SUNAPAMT2 != bSunap.SUNAPAMT2) { strSuUpDate = "OK"; }
                                if (dSunap(i).HALINGYE.To<string>("") != bSunap.HALINGYE.To<string>("")) { strSuUpDate = "OK"; }

                                if (strSuUpDate == "OK")
                                {
                                    if (!Report_OldAmt_Cancel(nHJ, i))      //종전의 영수증을 취소
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }

                                    strGbSTS = hicJepsuService.GetGbStsByWRTNO(FnWRTNO);

                                    if (!Report_NewAmt_Print(nHJ, dSunap(i), strGbSTS))     //신규등록
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }

                                    //BarCode_Print(lstgrpExam(i), i, nHJ, lblBMI.Text);       //바코드증 (cmdBarPrint_Click)
                                }
                            }
                            else
                            {
                                if (bJepsuNew)
                                {
                                    strGbSTS = hicJepsuService.GetGbStsByWRTNO(FnWRTNO);

                                    if (!Report_NewAmt_Print(nHJ, dSunap(i), strGbSTS))     //신규등록
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }

                                    BarCode_Print(lstgrpExam(i), i, nHJ, lblBMI.Text);       //바코드증 (cmdBarPrint_Click)
                                }
                            }

                            #endregion

                            #region 회사별 검진자명단에 건진날짜 Update
                            if (!comHpcLibBService.UpDateHicLtdPanoByItem(nHJ.JEPDATE, nHJ.GJYEAR, nHJ.GJJONG, nHJ.LTDCODE, nHJ.PANO))
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                            #endregion

                            //접수증 출력
                            if (bJepsuNew)
                            {
                                Pano_Print_Jepsu(nHJ, FstrEndo, FstrExamName, FstrName);
                            }

                            #region 낙상주의 업데이트
                            string strGbNaksang = "N";

                            if (chkFall.Checked)
                            {
                                strGbNaksang = "Y";
                            }
                            else
                            {
                                strGbNaksang = cHF.GET_Naksang_Flag(nHJ.AGE, nHJ.JEPDATE, nHJ.PTNO);
                            }

                            if (!hicJepsuService.UpDateGbNaksangByWrtno(nHJ.WRTNO, strGbNaksang))
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                            #endregion

                            //향정주사약 오더 전달 
                            if (!cHOS.Hang_Approve_Update(nHJ, lstsuInfo(i), lstgrpExam(i), "HR", lstHyang))
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            string strENDO = "NO";
                            //ENDO_JUPMST정리(2021-04-13)
                            if (FnWRTNO > 0 && nHJ.GJJONG == "31")
                            {
                                List<HIC_RESULT_EXCODE> lst1 = hicResultExCodeService.GetHicEndoExListByWrtno(FnWRTNO);
                                if (lst1.Count > 0)
                                {
                                    for (int j = 0; j < lst1.Count; j++)
                                    {
                                        if (lst1[j].ENDOGUBUN2.To<string>() == "Y") { strENDO = "OK"; }   //위내시경
                                        if (lst1[j].ENDOGUBUN3.To<string>() == "Y") { strENDO = "OK"; }   //위수면내시경
                                        if (lst1[j].ENDOGUBUN4.To<string>() == "Y") { strENDO = "OK"; }   //대장내시경
                                        if (lst1[j].ENDOGUBUN5.To<string>() == "Y") { strENDO = "OK"; }   //대장수면내시경
                                    }
                                }

                                if (strENDO == "NO")
                                {
                                    result = endoJupmstService.UpDateGbsunapByPtnoRDate(FstrPtno, "*", "HR");
                                    if (result < 0)
                                    {
                                        MessageBox.Show("내시경접수취소 오류", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }
                            }

                            //내시경 Order정리(2021-03-11)
                            if ( endoJupmstService.GetCountbyPtNoJDate(nHJ.PTNO, nHJ.JEPDATE) == 0 && nHJ.GJJONG == "31")
                            {
                                List<string> lstSucode = new List<string> { "E7630", "E7630S" };
                                if (comHpcLibBService.DeleteOcsOorders(nHJ.PTNO, lstSucode, nHJ.JEPDATE) < 0)
                                {
                                    MessageBox.Show("접수변경시 내시경검사 ORDER 삭제 오류가 발생함", "오류");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }

                            //EXAM_ORDER에 TX25 위 미생물 , TX21 결직장조직 insert
                            COMHPC cHPC = new COMHPC
                            {
                                WRTNO = nHJ.WRTNO,
                                JEPDATE = nHJ.JEPDATE,
                                PTNO = nHJ.PTNO,
                                DEPTCODE = "HR",
                                BI = "62",
                                SEX = nHJ.SEX,
                                AGE = nHJ.AGE,
                                SNAME = nHJ.SNAME,
                                JOBSABUN = 111,
                                DRCODE = "7101"
                            };

                            if (!cHOS.Hic_ExamBarCode_New(cHPC))
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            //심리검사실 오더 전달
                            if (!bGaJepsuFlag)
                            {
                                GROUPCODE_EXAM_DISPLAY rGED1 = lstgrpExam(i).Find(x => x.EXCODE == "TZ17");        //심리검사여부
                                if (!rGED1.IsNullOrEmpty())
                                {
                                    if (rGED1.EXCODE == "TZ17")
                                    {
                                        if (!cHMF.SIMRI_ORDER_INSERT(nHJ, 0))
                                        {
                                            MessageBox.Show("임상심리 오더를 전송시 오류 발생", "오류");
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            Cursor.Current = Cursors.Default;
                                            return;
                                        }
                                    }
                                }
                            }

                            //카드테이블에 접수번호 갱신 CARD_APPROV_CENTER에 HWrtno UPDATE
                            if (!bGaJepsuFlag)
                            {
                                if (clsPmpaType.RSD.CardSeqNo > 0 && (dSunap(i).SUNAPAMT2 > 0 || dSunap(i).SUNAPAMT > 0))
                                {
                                    //if (cardApprovCenterService.UpdateHWrtNobyCardSeqNo(i, nHJ.WRTNO, nHJ.PTNO) <= 0)
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

                            //신규접수일때 상담테이블, 검진1차,특수,구강,암의 판정테이블 생성함
                            if (bJepsuNew)
                            {
                                
                                if (!cHMF.HIC_NEW_SANGDAM_INSERT(nHJ))  //상담테이블
                                {
                                    MessageBox.Show("신규상담항목 자동발생 시 오류가 발생함.", "오류");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                if (!cHMF.HIC_NEW_MUNITEM_INSERT(nHJ))  //검진1차,특수,구강,암의 판정테이블
                                {
                                    MessageBox.Show("신규문진항목 자동발생시 오류가 발생함.", "오류");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                //암검진일 경우 금년에 먼저작성한 문진표가 있는지 체크
                                if (nHJ.GJJONG == "31")
                                {
                                    string strFDate = nHJ.GJYEAR + "-02-01";
                                    string strTDate = nHJ.JEPDATE;

                                    long nCanWRTNO = hicJepsuService.GetGbCancerWrtnoByJepDatePano(nHJ.PANO, strFDate, strTDate);

                                    if (nCanWRTNO > 0)
                                    {
                                        if (!cHMF.COPY_CANCER_MUNJIN(nHJ.WRTNO, nCanWRTNO))
                                        {
                                            MessageBox.Show("암문진내역 복사중 오류가 발생함.", "오류");
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            Cursor.Current = Cursors.Default;
                                            return;
                                        }
                                    }
                                }

                                //내시경 간호기록지 및 외래접수 생성
                                if (!cHcMain.HIC_ENDOCHART_INSERT(nHJ.WRTNO, nHJ.JEPDATE, nHJ.PTNO, "HR", nHJ.PANO))
                                {
                                    MessageBox.Show("내시경기록지 생성시 오류가 발생함.", "오류");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }

                            if (!bGaJepsuFlag)
                            {
                                //내시경 간호기록지 및 외래접수 생성
                                if (!cHcMain.HIC_ENDOCHART_INSERT(nHJ.WRTNO, nHJ.JEPDATE, nHJ.PTNO, "HR", nHJ.PANO))
                                {
                                    MessageBox.Show("내시경기록지 생성시 오류가 발생함.", "오류");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }

                                if((VB.Val(nHJ.GJJONG) >= 21 && VB.Val(nHJ.GJJONG) <=26)|| VB.Val(nHJ.GJJONG) == 30 || VB.Val(nHJ.GJJONG) == 32 || VB.Val(nHJ.GJJONG) == 49)
                                {
                                    strSpc = "OK";
                                }

                                //if (nHJ.GJJONG =="51")
                                //{
                                //    //방사선관계종자사 체크
                                //    if (hicJepsuService.GetCountChkBySexams(nHJ.WRTNO, "5106") > 0)
                                //    {
                                //        nHJ.BUSEIPSA = dtpJenipDate.Text;
                                //    }

                                //}

                                //2020-08-13 추가작업
                                //특수판정 취급물질 저장 => UPDATE_SPC_MUNJIN 구현
                                //if (strSpc == "OK") { cHcMain.INSERT_HicResSpecial(nHJ); }

                                cHcMain.INSERT_HicResSpecial(nHJ);

                                //특검일 경우 문진표에 특검정보 사항 UPDATE(1차만)
                                if (!clsHcType.THC[i].UCODES.IsNullOrEmpty() || strSpc =="OK")
                                {
                                    HIC_RES_SPECIAL dHRS = new HIC_RES_SPECIAL();

                                    dHRS.SABUN = txtLtdSabun.Text;
                                    dHRS.BUSE = txtBuse.Text;
                                    dHRS.PTIME = txtPTime.Text.To<long>(0);
                                    dHRS.PGIGAN_YY = VB.Pstr(lblHGigan.Text, "년", 1).To<long>(0);
                                    dHRS.PGIGAN_MM = VB.Pstr(VB.Pstr(lblHGigan.Text, "개월", 1), "년", 2).To<long>(0);
                                    dHRS.GBSPC = VB.Pstr(cboGbSpc.Text, ".", 1);
                                    dHRS.IPSADATE = dtpIpsaDate.Text;
                                    dHRS.JENIPDATE = dtpJenipDate.Text;
                                    dHRS.BUSE = txtBuse.Text.Trim();
                                    dHRS.NATIONAL = VB.Pstr(cboNational.Text, ".", 1);
                                    dHRS.JIKJONG = VB.Pstr(txtJikJong.Text, ".", 1);
                                    dHRS.GONGJENG = VB.Pstr(txtGongjeng.Text, ".", 1);

                                    if (!cHcMain.UPDATE_SPC_MUNJIN(nHJ, ssJik, lstsuInfo(i), dHRS))
                                    {
                                        MessageBox.Show("특검 문진내용 DB에 UPDATE 안됨.", "확인");
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }

                                //학생신검 일 경우 입력된 Data Update
                                if (nHJ.GJJONG == "56" || nHJ.GJJONG == "59")
                                {
                                    if (!cHcMain.UPDATE_SCHOOL_MUNJIN(nHJ, txtHeight.Text, txtWeight.Text))
                                    {
                                        MessageBox.Show(" 학생검진 문진내역 DB에 UPDATE시 오류 발생함.", "오류");
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }
                            }

                            //가접수시 공정코드 업데이트  UPDATE_SPC_GONGJENG => HIC_PATIENT 업데이트시 같이 UPDATE

                            //TextEmr 자동 EMR Table INSERT
                            if (bJepsuNew)
                            {
                                clsQuery.NEW_TextEMR_TreatInterface(clsDB.DbCon, txtPtno.Text, dtpJepDate.Text, "HR", "HR", "정상", "99916");
                            }

                            //2016-09-22 웹결과지인쇄 신청로그 업데이트 => HIC_JEPSU 업데이트에 포함시킴
                            comHpcLibBService.InsertWebPrintReqHic(nHJ.WRTNO);

                            //야간작업 대상인지 점검(2014-01-06)
                            if (!hicSunapdtlService.GetRowidbyWrtNoCodeIN(nHJ.WRTNO, clsHcVariable.G36_NIGHT_CODE).IsNullOrEmpty())
                            {
                                //야간작업문진표를 등록하였는지 점검
                                if (hicMunjinNightService.GetCountbyWrtNo(nHJ.WRTNO) == 0)
                                {
                                    nNightWrtno = nHJ.WRTNO;
                                    //frmHcNightMunjin frmNight = new frmHcNightMunjin("야간작업문진표", nHJ.WRTNO);
                                    //frmNight.ShowDialog();
                                }
                            }
                            else
                            {
                                //2021-04-26(야간작업문진 작성자 대상아닐경우 삭제로직 추가)
                                if (hicMunjinNightService.GetCountbyWrtNo(nHJ.WRTNO) > 0)
                                {
                                    hicMunjinNightService.DeleteByWrtno(nHJ.WRTNO);
                                }  
                            }


                            //2차정밀청력 대상자 안내
                            HIC_SUNAP nHSNP = comHpcLibBService.chkSunapAudio(nHJ.WRTNO);
                            if (!nHSNP.IsNullOrEmpty())
                            {
                                if (nHSNP.SUNAPAMT > 0)
                                {
                                    HIC_SUNAP nHSNP1 = comHpcLibBService.chkSunapAudio1(nHJ.WRTNO);
                                    if (nHSNP.SUNAPAMT > 0)
                                    {
                                        MessageBox.Show("특수2차 순음청력 수납대상입니다.", "확인");
                                    }  
                                }
                            }

                            //32종진단구분대상
                            nJinWrtno = 0;
                            if (!nHJ.SEXAMS.IsNullOrEmpty())
                            {
                                for (int j = 0; j < lstJindan.Count; j++)
                                {
                                    if (nHJ.SEXAMS.Trim().Contains(lstJindan[j].CODE.To<string>("").Trim()))
                                    {
                                        nJinWrtno = nHJ.WRTNO;
                                        break;
                                    }
                                }
                            }


                            //수검자 메모사항 저장
                            Hic_Memo_Save();

                            //내시경 등 동의서 승인요청 DB 생성
                            if (!cHMF.Consent_DB_Update(nHJ))
                            {
                                MessageBox.Show("내시경 동의서 DB 생성시 오류가 발생함.", "오류");
                                clsDB.setRollbackTran(clsDB.DbCon);
                                Cursor.Current = Cursors.Default;
                                return;
                            }

                            
                            //대기순번 접수완료 저장
                            //2020-07-08(가접수 시에는 대기순번 접수완료처리 제외)
                            if (!bGaJepsuFlag)
                            {
                                if (!hicWaitService.UpDateCompleteOK(clsPublic.GstrSysTime, nHJ.JUMINNO2, DateTime.Now.ToShortDateString()))
                                {
                                    MessageBox.Show("대기순번 등록시 오류가 발생함.", "오류");
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }

                            //낙상주의 외래 등록
                            if (strGbNaksang == "Y") { cHMF.Naksang_Opd_INSERT(nHJ.PTNO, "HR", clsType.User.IdNumber.To<long>()); }

                            //timer1.Start();
                            //2019-11-19(결과지 내원수령관련 추가)
                            //if (!nHJ.WEBPRINTREQ.IsNullOrEmpty())

                            if(nHJ.GBCHK3 == "Y")
                            {
                                if (hicCharttransPrintService.GetRowidByWrtno(nHJ.WRTNO).IsNullOrEmpty())
                                {
                                    HIC_CHARTTRANS_PRINT nHCP = new HIC_CHARTTRANS_PRINT();
                                    nHCP.WRTNO = nHJ.WRTNO;
                                    nHCP.JEPDATE = nHJ.JEPDATE;
                                    nHCP.SNAME = nHJ.SNAME;
                                    nHCP.BIRTH = VB.Left(nHJ.JUMINNO, 6);
                                    nHCP.LTDNAME = nHJ.LTDNAME;
                                    nHCP.GJJONG = nHJ.GJJONG;

                                    //Insert
                                    if (!hicCharttransPrintService.Insert(nHCP))
                                    {
                                        MessageBox.Show("결과지 수령방법 입력시 오류가 발생함.", "오류");
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }
                                else
                                {
                                    //UpDate
                                    if (!hicCharttransPrintService.UpDate(nHJ.LTDNAME, nHJ.WRTNO, nHJ.SNAME))
                                    {
                                        MessageBox.Show("결과지 수령방법 저장시 오류가 발생함.", "오류");
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (!hicCharttransPrintService.GetRowidByWrtno(nHJ.WRTNO).IsNullOrEmpty())
                                {
                                    //Delete 
                                    if (!hicCharttransPrintService.Delete(nHJ.WRTNO))
                                    {
                                        MessageBox.Show("결과지 수령방법 삭제시 오류가 발생함.", "오류");
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }
                            }

                            //2020년 전자동의서관련 추가
                            if (nHJ.WRTNO > 0)
                            {
                                if (comHpcLibBService.GetHicPrivacyAccept(nHJ.PTNO, "D50", "HR").IsNullOrEmpty())
                                {
                                    HIC_CONSENT nHC = new HIC_CONSENT
                                    {

                                        BDATE = nHJ.JEPDATE,
                                        SDATE = nHJ.JEPDATE,
                                        PTNO = nHJ.PTNO,
                                        WRTNO = nHJ.WRTNO,
                                        PANO = nHJ.PANO,
                                        SNAME = nHJ.SNAME,
                                        DEPTCODE = "HR",
                                        FORMCODE = "D50",
                                        PAGECNT = 1,
                                        ENTSABUN = clsType.User.IdNumber.To<long>()
                                    };

                                    if (!hicConsentService.Insert(nHC))
                                    {
                                        MessageBox.Show("전자동의서 등록시 오류가 발생함.", "오류");
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        Cursor.Current = Cursors.Default;
                                        return;
                                    }
                                }


                                FstrD54 = "";
                                List<HIC_SUNAPDTL_GROUPCODE> list1 = hicSunapdtlGroupcodeService.GetCodeGbSangdambyWrtNo(FnWRTNO);
                                if (list1.Count > 0)
                                {
                                    for (int k = 0; i < list1.Count; i++)
                                    {
                                        if (VB.InStr(list1[k].GBSANGDAM.Trim(),"1") > 0)
                                        {
                                            if (comHpcLibBService.GetHicPrivacyAccept(nHJ.PTNO, "D54", "HR").IsNullOrEmpty())
                                            {
                                                HIC_CONSENT nHC = new HIC_CONSENT
                                                {

                                                    BDATE = nHJ.JEPDATE,
                                                    SDATE = nHJ.JEPDATE,
                                                    PTNO = nHJ.PTNO,
                                                    WRTNO = nHJ.WRTNO,
                                                    PANO = nHJ.PANO,
                                                    SNAME = nHJ.SNAME,
                                                    DEPTCODE = "HR",
                                                    FORMCODE = "D54",
                                                    PAGECNT = 1,
                                                    ENTSABUN = clsType.User.IdNumber.To<long>()
                                                };

                                                if (!hicConsentService.Insert(nHC))
                                                {
                                                    MessageBox.Show("전자동의서 등록시 오류가 발생함.", "오류");
                                                    clsDB.setRollbackTran(clsDB.DbCon);
                                                    Cursor.Current = Cursors.Default;
                                                    return;
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }

                                //생물학적노출지표검사안내문 동의서
                                if (FstrD56 == "OK")
                                {
                                    if (comHpcLibBService.GetHicPrivacyAccept(nHJ.PTNO, "D56", "HR").IsNullOrEmpty())
                                    {
                                        HIC_CONSENT nHC = new HIC_CONSENT
                                        {
                                            BDATE = nHJ.JEPDATE,
                                            SDATE = nHJ.JEPDATE,
                                            PTNO = nHJ.PTNO,
                                            WRTNO = nHJ.WRTNO,
                                            PANO = nHJ.PANO,
                                            SNAME = nHJ.SNAME,
                                            DEPTCODE = "HR",
                                            FORMCODE = "D56",
                                            PAGECNT = 1,
                                            ENTSABUN = clsType.User.IdNumber.To<long>()
                                        };

                                        if (!hicConsentService.Insert(nHC))
                                        {
                                            MessageBox.Show("전자동의서 등록시 오류가 발생함.", "오류");
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            Cursor.Current = Cursors.Default;
                                            return;
                                        }
                                    }
                                }
                                if (FstrD57 == "OK")
                                {
                                    if (comHpcLibBService.GetHicPrivacyAccept(nHJ.PTNO, "D57", "HR").IsNullOrEmpty())
                                    {
                                        HIC_CONSENT nHC = new HIC_CONSENT
                                        {
                                            BDATE = nHJ.JEPDATE,
                                            SDATE = nHJ.JEPDATE,
                                            PTNO = nHJ.PTNO,
                                            WRTNO = nHJ.WRTNO,
                                            PANO = nHJ.PANO,
                                            SNAME = nHJ.SNAME,
                                            DEPTCODE = "HR",
                                            FORMCODE = "D57",
                                            PAGECNT = 1,
                                            ENTSABUN = clsType.User.IdNumber.To<long>()
                                        };

                                        if (!hicConsentService.Insert(nHC))
                                        {
                                            MessageBox.Show("전자동의서 등록시 오류가 발생함.", "오류");
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            Cursor.Current = Cursors.Default;
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
                clsDB.setCommitTran(clsDB.DbCon);
                //clsDB.setRollbackTran(clsDB.DbCon);

                Cursor.Current = Cursors.Default;

                //개인정보 동의서 자동 실행(2020-10-15 출장시 동의서 안뜸)
                //if (txtPtno.Text.Trim().Substring(0, 7) != "8100000")
                if (!chkChul.Checked)
                {
                    if (hicPatientService.GetPrivacyNewByPtno(txtPtno.Text).IsNullOrEmpty())
                    {
                        frmHcPermission.CellDblClicked(0);
                        //frmHcEmrPermission.CellDblClicked(0);

                        //frmHcEmrConset_Rec = new frmHcEmrConset_Rec(FnWRTNO,"NUR");
                        //frmHcEmrConset_Rec.Show();
                    }
                }

                Screen_Clear();

                if (nNightWrtno > 0)
                {
                    frmHcNightMunjin frmNight = new frmHcNightMunjin("야간작업문진표", nNightWrtno);
                    frmNight.Show();
                    frmNight.BringToFront();
                }
                else
                {
                    ComFunc.MsgBox("접수 완료!", "작업완료");
                }
                ////32종 진단서구분입력
                if (nJinWrtno > 0)
                {
                    HIC_JIN_GBN item2 = hicJinGbnService.GetItemByWrtno(nJinWrtno);
                    if (item2.GUBUN.IsNullOrEmpty())
                    {
                        frmHcPanJindanSet f = new frmHcPanJindanSet(nJinWrtno);
                        f.Show();
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            
        }

        /// <summary>
        /// 접수증 출력
        /// </summary>
        /// <param name="nHJ"></param>
        /// <param name="fstrEndo"></param>
        /// <param name="fstrExamName"></param>
        private void Pano_Print_Jepsu(HIC_JEPSU nHJ, string fstrEndo, string[] fstrExamName, string[] fstrName)
        {
            frmHcPrintPano fHPP = new frmHcPrintPano(nHJ, fstrEndo, fstrExamName, fstrName);
            fHPP.ShowDialog();
        }

        /// <summary>
        /// 검진 바코드증 출력
        /// </summary>btnJepView
        /// <param name="list"></param>
        /// <param name="nIdx"></param>
        /// <param name="nHJ"></param>
        /// <param name="argBiman"></param>
        /// <seealso cref="cmdBarPrint_Click"/>
        private void BarCode_Print(List<GROUPCODE_EXAM_DISPLAY> list, int nIdx, HIC_JEPSU nHJ, string argBiman)
        {
            if (ComFunc.MsgBoxQ(nHJ.GJJONG + "  종 바코드증을 발행하시겠습니까?","", MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                return;
            }

            List<string> lstCode = new List<string>();
            
            FstrRetValue = "";

            List<HIC_RESULT_EXCODE> lstHRE = hicResultExCodeService.GetEntPartByWRTNO(nHJ.WRTNO);

            if (lstHRE.Count > 0)
            {
                for (int i = 0; i < lstHRE.Count; i++)
                {
                    HIC_CODE rHC = hicCodeService.GetItembyCode("72", lstHRE[i].ENTPART);

                    if (!rHC.IsNullOrEmpty())
                    {
                        FstrRetValue += rHC.CODE.Trim() + "." + rHC.NAME.Trim() + "^^";
                    }
                }
            }

            if (FstrRetValue == "E.일반상담^^J.분변잠혈^^")
            {   
                lstCode.Add("3101");

                if (hicSunapdtlService.GetRowidbyWrtNoCodeIN(nHJ.WRTNO, lstCode).IsNullOrEmpty())
                {
                    FstrRetValue = "J.분변잠혈";
                }
            }

            if (clsHcType.THC[nIdx].GJJONG == "32")
            {
                lstCode.Clear();
                lstCode.Add("J142"); lstCode.Add("J143"); lstCode.Add("J144"); lstCode.Add("J145"); lstCode.Add("J146");
                lstCode.Add("J147"); lstCode.Add("J148"); lstCode.Add("J149"); lstCode.Add("J150"); lstCode.Add("J151");

                if (!hicSunapdtlService.GetRowidbyWrtNoCodeIN(nHJ.WRTNO, lstCode).IsNullOrEmpty())
                {
                    FstrRetValue += " 사본추가발급";
                }
            }

            //건진접수증
            frmHcPrintBar fHPB = new frmHcPrintBar(nHJ, list, FstrRetValue, argBiman, lstHRE.Count);
            fHPB.ShowDialog();

            //외래치과로 수검자를 보내기 위함(임시로 사용함)
            if (clsHcVariable.B03_DNT_OPD_JIN)
            {
                //구강검사 여부를 READ
                string strGbDental = string.Empty;

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].EXCODE.Trim() == "ZD00" || list[i].EXCODE.Trim() == "ZD01")
                    {
                        // TODO : 치과접수증(); -> 사용안함
                        break;
                    }
                }
            }

            //2019-04-15(소변바코드출력)
            if (nHJ.GBCHUL != "Y" || clsHcType.THC[nIdx].GBUSE == "U")
            {
                //뇨단백 소변컵 바코드 인쇄
                List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetUrineItembyWrtNo(nHJ.WRTNO);

                if (list2.Count > 0)
                {
                    frmHcUrineBarCode frm = new frmHcUrineBarCode(nHJ.WRTNO.To<string>());
                    frm.Show();
                }
            }
        }

        private bool Report_NewAmt_Print(HIC_JEPSU nHJ, HIC_SUNAP hSP, string strGbSTS)
        {
            try
            {
                hSP.SEQNO = hicSunapService.GetMaxSeqbyWrtNo(nHJ.WRTNO);
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

                hicSunapService.Insert(hSP);

                if (strGbSTS == "1" && hSP.BONINAMT > 0)
                {
                    if ( ComFunc.MsgBoxQ(nHJ.GJJONG + " 종 영수증을 발생하시겠습니까?","", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
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
        
        /// <summary>
        /// 최종 수납 영수증을 삭제후 발행(가접수)
        /// </summary>
        /// <param name="argPano"></param>
        /// <param name="argJepDate"></param>
        /// <param name="argJong"></param>
        /// <returns></returns>
        private bool Report_NewAmt_Print_WORK(long argPano, string argJepDate, string argJong, HIC_SUNAP hSNP)
        {
            try
            {
                hicSunapWorkService.DeletebyPaNoGjJong(argPano, argJong);

                HIC_SUNAP_WORK item = new HIC_SUNAP_WORK
                {
                    WRTNO = 0,
                    SUDATE = argJepDate,
                    SEQNO = 0,
                    PANO = argPano,
                    TOTAMT = hSNP.TOTAMT,
                    HALINGYE = hSNP.HALINGYE,
                    HALINAMT = hSNP.HALINAMT,
                    JOHAPAMT = hSNP.JOHAPAMT,
                    LTDAMT = hSNP.LTDAMT,
                    BONINAMT = hSNP.BONINAMT,
                    BOGUNSOAMT = hSNP.BOGENAMT,
                    MISUGYE = hSNP.MISUGYE,
                    MISUAMT = hSNP.MISUAMT,
                    SUNAPAMT = hSNP.SUNAPAMT,
                    JOBSABUN = clsType.User.IdNumber.To<long>(),
                    GJJONG = argJong
                };

                //수납영수증 상세내역
                hicSunapWorkService.Insert(item);


                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 검사항목(HIC_SUNAPDTL_WORK)을 UPDATE(가접수)
        /// </summary>
        /// <param name="argPano"></param>
        /// <param name="argJong"></param>
        /// <returns></returns>
        private bool SunapDTL_WORK_INSERT(long argPano, string argJong, List<READ_SUNAP_ITEM> list, string argBuRate, string argDate)
        {
            try
            {
                //기존의 자료가 있으면 삭제함
                hicSunapdtlWorkService.DeleteAllByPano(argPano, argJong);

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].RowStatus != RowStatus.Delete)
                    {
                        if (list[i].GBSELF.To<string>("").Trim() == "")
                        {
                            list[i].GBSELF = argBuRate;
                        }

                        hicSunapdtlWorkService.InsertData(argPano, argJong, list[i], argDate);
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
        /// 검사항목(HIC_RESULT)을 UPDATE
        /// </summary>
        /// <param name="argWrtno"></param>
        /// <returns></returns>
        private bool Sunap_Result_UPDATE(long argWrtno, List<GROUPCODE_EXAM_DISPLAY> lstExam)
        {
            string strOK = string.Empty;

            try
            {   
                //DB에는 자료가 있고 검사 상세내역에 없는 검사는 삭제함
                List<HIC_RESULT> list = hicResultService.GetAllByWrtNo(argWrtno);

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
                            hicResultService.InsertDelSelectbyRowid(list[i].RID);
                        }

                        hicResultService.DeleteByRowid(list[i].RID);
                    }
                }

                //추가 접수된 검사항목을 INSERT
                for (int i = 0; i < lstExam.Count; i++)
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
                                HIC_RESULT hRES = new HIC_RESULT
                                {
                                    WRTNO = argWrtno,
                                    GROUPCODE = lstExam[i].GROUPCODE,
                                    PART = VB.Trim(item.ENTPART),
                                    EXCODE = lstExam[i].EXCODE,
                                    RESCODE = item.RESCODE
                                };

                                if (!hicResultService.InsertData(hRES))              //자료를 INSERT
                                {
                                    MessageBox.Show("검사항목 INSERT 시 오류 발생", "오류");
                                    return false;
                                }

                                hicJepsuService.UpDate_GbSTS("1", argWrtno);    //접수상태를 돌림
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

        /// <summary>
        /// 검사항목(HIC_SUNAPDTL)을 UPDATE
        /// </summary>
        /// <param name="argWrtno"></param>
        /// <param name="list"></param>
        /// <param name="argBuRate"></param>
        /// <returns></returns>
        private bool Sunap_DTL_INSERT(long argWrtno, List<READ_SUNAP_ITEM> list, string argBuRate)
        {
            try
            {
                //기존의 자료가 있으면 삭제함
                if (hicSunapdtlService.GetCountbyWrtNo(argWrtno) > 0)
                {
                    hicSunapdtlService.DeleteAllByWrtno(argWrtno);
                }

                int nSeq = hicSunapdtlService.GetMaxHistorySeqNoByWrtno(argWrtno);
                
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].RowStatus != RowStatus.Delete)
                    {
                        if (list[i].GBSELF.To<string>("").Trim() == "")
                        {
                            list[i].GBSELF = argBuRate;
                        }

                        hicSunapdtlService.InsertData(argWrtno, list[i]);
                    }

                    nSeq += 1;

                    hicSunapdtlService.InsertDataHis(argWrtno, list[i], nSeq);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private HIC_JEPSU Jepsu_Data_Build(int argIdx)
        {
            clsHaBase cHB = new clsHaBase();
            DateTime? dWebPrtDate = null;
            string strDental = "";
            string strDentOnly = "";

            int nCnt = 0;
            int nCnt2 = 0;

            string strBangi = string.Compare(VB.Mid(dtpJepDate.Text, 6, 2), "7") < 0 ? "1" : "2";   //검진반기

            HIC_EXJONG hJong = hicExjongService.GetItembyCode(clsHcType.THC[argIdx].GJJONG);        //검진종류 정보
            GROUPCODE_EXAM_DISPLAY rGED1 = lstgrpExam(argIdx).Find(x => x.EXCODE == "ZD00");        //구강검사여부 1
            GROUPCODE_EXAM_DISPLAY rGED2 = lstgrpExam(argIdx).Find(x => x.EXCODE == "ZD01");        //구강검사여부 1

            if (!rGED1.IsNullOrEmpty() && rGED1.EXCODE == "ZD00") { strDental = "Y"; }  //구강검사여부
            if (!rGED2.IsNullOrEmpty() && rGED1.EXCODE == "ZD01") { strDental = "Y"; }  //구강검사여부


            for (int i = 0; i < lstgrpExam(argIdx).Count; i++)
            {
                if (lstgrpExam(argIdx)[i].EXCODE.To<string>("").Trim() == "ZD00" || lstgrpExam(argIdx)[i].EXCODE.To<string>("").Trim() == "ZD99") { nCnt += 1; }
                if (lstgrpExam(argIdx)[i].EXCODE.To<string>("").Trim() == "A104" || lstgrpExam(argIdx)[i].EXCODE.To<string>("").Trim() == "A105") { nCnt2 += 1; }
                if (lstgrpExam(argIdx)[i].EXCODE.To<string>("").Trim() == "A108" || lstgrpExam(argIdx)[i].EXCODE.To<string>("").Trim() == "A109") { nCnt2 += 1; }
            }
            if(nCnt > 0 && nCnt2 == 0) { strDentOnly = "Y"; }

            string strGBCHK1 = cHcMain.Read_Gkiho2_Gbn(txtGKiho.Text.Trim(), 0);
            string strGBCHK2 = cHcMain.Read_66Gub2_Gbn(FstrSExams, txtGKiho.Text.Trim(), clsHcType.TEC.NHICOK, clsHcType.TEC.AGE, 0);
            string strGBCHK3 = Rtn_Visit_PrintReceipt(argIdx);


            //for (int i = 0; i < 5; i++)
            //{
            //    switch (i)
            //    {   //알림톡
            //        case 0: if (rdoRES14.Checked) { dWebPrtDate = DateTime.Now; } break;
            //        case 1: if (rdoRES24.Checked) { dWebPrtDate = DateTime.Now; } break;
            //        case 2: if (rdoRES34.Checked) { dWebPrtDate = DateTime.Now; } break;
            //        case 3: if (rdoRES44.Checked) { dWebPrtDate = DateTime.Now; } break;
            //        case 4: if (rdoRES54.Checked) { dWebPrtDate = DateTime.Now; } break;
            //        default: break;
            //    }
            //}
            if (VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0) == 483)
            {
                switch (argIdx)
                {   //알림톡
                    case 0: rdoRES14.Checked = true; dWebPrtDate = DateTime.Now; break;
                    case 1: rdoRES24.Checked = true; dWebPrtDate = DateTime.Now; break;
                    case 2: rdoRES34.Checked = true; dWebPrtDate = DateTime.Now; break;
                    case 3: rdoRES44.Checked = true; dWebPrtDate = DateTime.Now; break;
                    case 4: rdoRES54.Checked = true; dWebPrtDate = DateTime.Now; break;
                    default: break;
                }
            }

            //결과지 수령방법
            switch (argIdx)
            {   //알림톡
                case 0: if (rdoRES14.Checked) { dWebPrtDate = DateTime.Now; } break;
                case 1: if (rdoRES24.Checked) { dWebPrtDate = DateTime.Now; } break;
                case 2: if (rdoRES34.Checked) { dWebPrtDate = DateTime.Now; } break;
                case 3: if (rdoRES44.Checked) { dWebPrtDate = DateTime.Now; } break;
                case 4: if (rdoRES54.Checked) { dWebPrtDate = DateTime.Now; } break;
                default: break;
            }

            //암접수 구분자
            string strGbAm = string.Empty;
            if (clsHcType.THC[argIdx].GJJONG == "31")
            {
                for (int i = 1; i < 8; i++)
                {
                    switch (i)
                    {
                        case 1: strGbAm += chkAm1.Checked ? "1," : "0,"; break;
                        case 2: strGbAm += chkAm2.Checked ? "1," : "0,"; break;
                        case 3: strGbAm += chkAm3.Checked ? "1," : "0,"; break;
                        case 4: strGbAm += chkAm4.Checked ? "1," : "0,"; break;
                        case 5: strGbAm += chkAm5.Checked ? "1," : "0,"; break;
                        case 6: strGbAm += chkAm6.Checked ? "1," : "0,"; break;
                        case 7: strGbAm += chkAm7.Checked ? "1," : "0,"; break;
                        default: break;
                    }
                }

                //if (chkHeaEndo.Checked = true) { strEndo = "Y"; }
            }
            else
            {
                strGbAm = "0,0,0,0,0,0,0,";
            }

            HIC_JEPSU rHJ = new HIC_JEPSU
            {
                GJYEAR      = cboYear.Text,
                JEPDATE     = dtpJepDate.Text,                                          PANO        = txtPano.Text.To<long>(),
                SNAME       = txtSName.Text.Trim(),                                     SEX         = cboSex.Text,
                AGE         = txtAge.Text.To<long>(),                                   GJJONG      = clsHcType.THC[argIdx].GJJONG,
                TEL         = txtTel.Text.Trim(),                                       GJCHASU     = hJong.CHASU,
                GJBANGI     = strBangi,                                                 LTDCODE     = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0),
                BUSENAME    = txtBuse.Text.Trim(),                                      MAILCODE    = txtMail.Text.Trim(),
                JUSO1       = txtJuso1.Text.Trim(),                                     JUSO2       = txtJuso2.Text.Trim(),
                UCODES      = clsHcType.THC[argIdx].UCODES,                             SEXAMS      = clsHcType.THC[argIdx].SEXAMS,
                PTNO        = txtPtno.Text.Trim(),
                BURATE      = FstrBuRate[argIdx],                                       JISA        = VB.Pstr(txtJisa.Text, ".", 1).Trim(),
                KIHO        = VB.Pstr(txtKiho.Text, ".", 1).Trim(),                     GKIHO       = txtGKiho.Text.Trim(),
                JUMINNO     = txtJumin1.Text + VB.Left(txtJumin2.Text, 1) + "******",   GBCHUL      = chkChul.Checked ? "Y" : "N",
                GBDENTAL    = strDental,                                                GBINWON     = hJong.GBINWON,
                JOBSABUN    = clsType.User.IdNumber.To<long>(),                         BOGUNSO     = VB.Pstr(txtBoKiho.Text, ".", 1).Trim(),
                IPSADATE    = dtpIpsaDate.Text,                                         GBN         = rdoSchool1.Checked ? "1" : "2",
                CLASS       = txtClass.Text.To<long>(),                                 BAN         = txtBan.Text.To<long>(),
                BUN         = txtBun.Text.To<long>(),                                   SABUN       = txtLtdSabun.Text.Trim(),
                MURYOAM     = cboMuRyoAm.Text.Trim() == "" ? "" : cboMuRyoAm.Text.Trim() == "Y" ? "Y" : "N",
                GUBDAESANG  = chkGubAm.Checked ? "Y" : "",                              EMAIL       = txtEmail.Text.Trim(),
                REMARK      = clsHcType.THC[argIdx].REMARK,                             GBHEAENDO   = chkHeaEndo.Checked ? "Y" : "",
                CARDSEQNO   = 0,                                                        //GBHEAENDO   = strEndo,
                GBAM        = strGbAm,                                                  GBSTS       = "1",
                SENDMSG     = "",                                                       GBCHK1      = strGBCHK1,
                GBCHK2      = strGBCHK2,                                                GBCHK3      = strGBCHK3,
                LTDNAME     = VB.Pstr(txtLtdCode.Text, ".", 2),
                JUMINNO2    = clsAES.AES(txtJumin1.Text + txtJumin2.Text),              WEBPRINTREQ = dWebPrtDate,
                GBDENTONLY  = strDentOnly,
                GWRTNO      = GnWRTNO                                                  
            };

            //출장XrayNo
            if (rHJ.GBCHUL == "Y")
            {
                switch (argIdx)
                {   //XRAY NO
                    case 0: rHJ.XRAYNO = txtXrayNo1.Text; break;
                    case 1: rHJ.XRAYNO = txtXrayNo2.Text; break;
                    case 2: rHJ.XRAYNO = txtXrayNo3.Text; break;
                    case 3: rHJ.XRAYNO = txtXrayNo4.Text; break;
                    case 4: rHJ.XRAYNO = txtXrayNo5.Text; break;
                    default: break;
                }
            }

            //학생검진 아닐경우 GBN강제 공백
            if (rHJ.GJJONG != "56") { rHJ.GBN = ""; }

            //인원통계 강제세팅
            if (rHJ.GJJONG == "11" && rHJ.UCODES != "") { rHJ.GBINWON = "32"; }

            //종검내시경실 체크로직 추가
            if(rHJ.GJJONG == "31" && chkHeaEndo.Checked == false)
            {
                if (heaJepsuService.GetEndoGbnbyPtNo(txtPtno.Text) == "1")
                {
                    chkHeaEndo.Checked = true;
                    rHJ.GBHEAENDO = "Y";
                }
            }
            
            //종검유무
            rHJ.JONGGUMYN = "0";
            if (heaJepsuService.GetCountbyPtNoSDate(rHJ.PTNO, rHJ.JEPDATE) > 0) { rHJ.JONGGUMYN = "1"; }
            if (rHJ.JONGGUMYN == "0")
            {
                switch (argIdx)
                {
                    case 0: rHJ.JONGGUMYN = chkJongGum1.Checked ? "1" : "0"; break;
                    case 1: rHJ.JONGGUMYN = chkJongGum1.Checked ? "1" : "0"; break;
                    case 2: rHJ.JONGGUMYN = chkJongGum1.Checked ? "1" : "0"; break;
                    case 3: rHJ.JONGGUMYN = chkJongGum1.Checked ? "1" : "0"; break;
                    case 4: rHJ.JONGGUMYN = chkJongGum1.Checked ? "1" : "0"; break;
                    default: rHJ.JONGGUMYN = "0"; break;
                }
            }

            //69추가판정 여부
            switch (argIdx)
            {
                case 0:  rHJ.GBADDPAN = chk69Pan1.Checked ? "Y" : "N";break;
                case 1:  rHJ.GBADDPAN = chk69Pan2.Checked ? "Y" : "N";break;
                case 2:  rHJ.GBADDPAN = chk69Pan3.Checked ? "Y" : "N";break;
                case 3:  rHJ.GBADDPAN = chk69Pan4.Checked ? "Y" : "N";break;
                case 4:  rHJ.GBADDPAN = chk69Pan5.Checked ? "Y" : "N"; break;
                default: rHJ.GBADDPAN = "N"; break;
            }

            if (clsHcType.THC[argIdx].GBUSE != "U")
            {
                rHJ.WRTNO = cHB.Read_New_JepsuNo();
            }
            else
            {
                rHJ.WRTNO = clsHcType.THC[argIdx].WRTNO;
            }

            rHJ.GBJUSO = "";
            //결과지 수령지
            switch (argIdx)
            {
                case 0:
                    if (rdoRES12.Checked)
                    {

                        //rHJ.GBJUSO = chkRES12_1.Checked ? "Y" : "N";
                        if (chkRES12_1.Checked == true)
                        {
                            rHJ.GBJUSO = "Y";
                        }
                        else if (chkRES12_2.Checked == true)
                        {
                            rHJ.GBJUSO = "N";
                        }
                        else if (chkRES12_3.Checked == true)
                        {
                            rHJ.GBJUSO = "E";
                            rHJ.MAILCODE = txtMail1.Text;
                            rHJ.JUSO1 = txtJuso11.Text;
                            rHJ.JUSO2 = txtJuso21.Text;
                        }
                    }
                    break;
                case 1: 
                    if (rdoRES22.Checked)
                    {
                        //rHJ.GBJUSO = chkRES22_1.Checked ? "Y" : "N";
                        if (chkRES22_1.Checked == true)
                        {
                            rHJ.GBJUSO = "Y";
                        }
                        else if (chkRES22_2.Checked == true)
                        {
                            rHJ.GBJUSO = "N";
                        }
                        else if (chkRES22_3.Checked == true)
                        {
                            rHJ.GBJUSO = "E";
                            rHJ.MAILCODE = txtMail2.Text;
                            rHJ.JUSO1 = txtJuso12.Text;
                            rHJ.JUSO2 = txtJuso22.Text;
                        }
                    }
                    break;
                case 2: 
                    if (rdoRES32.Checked)
                    {
                        //rHJ.GBJUSO = chkRES32_1.Checked ? "Y" : "N";
                        if (chkRES32_1.Checked == true)
                        {
                            rHJ.GBJUSO = "Y";
                        }
                        else if (chkRES32_2.Checked == true)
                        {
                            rHJ.GBJUSO = "N";
                        }
                        else if (chkRES32_3.Checked == true)
                        {
                            rHJ.GBJUSO = "E";
                            rHJ.MAILCODE = txtMail3.Text;
                            rHJ.JUSO1 = txtJuso13.Text;
                            rHJ.JUSO2 = txtJuso23.Text;
                        }
                    }
                    break;
                case 3: 
                    if (rdoRES42.Checked)
                    {
                        //rHJ.GBJUSO = chkRES42_1.Checked ? "Y" : "N";
                        if (chkRES42_1.Checked == true)
                        {
                            rHJ.GBJUSO = "Y";
                        }
                        else if (chkRES42_2.Checked == true)
                        {
                            rHJ.GBJUSO = "N";
                        }
                        else if (chkRES42_3.Checked == true)
                        {
                            rHJ.GBJUSO = "E";
                            rHJ.MAILCODE = txtMail4.Text;
                            rHJ.JUSO1 = txtJuso14.Text;
                            rHJ.JUSO2 = txtJuso24.Text;
                        }
                    }
                    break;
                case 4:
                    if (rdoRES52.Checked)
                    {
                        //rHJ.GBJUSO = chkRES52_1.Checked ? "Y" : "N";
                        if (chkRES52_1.Checked == true)
                        {
                            rHJ.GBJUSO = "Y";
                        }
                        else if (chkRES52_2.Checked == true)
                        {
                            rHJ.GBJUSO = "N";
                        }
                        else if (chkRES52_3.Checked == true)
                        {
                            rHJ.GBJUSO = "E";
                            rHJ.MAILCODE = txtMail4.Text;
                            rHJ.JUSO1 = txtJuso14.Text;
                            rHJ.JUSO2 = txtJuso24.Text;
                        }
                    }
                    break;
                default: rHJ.GBJUSO = "N"; break;
            }

            //결과지 회사인경우 사업장코드 정보 읽어서 강제세팅
            if (rHJ.GBJUSO == "N")
            {
                HIC_LTD nHL = hicLtdService.GetMailCodebyCode(rHJ.LTDCODE.To<string>());

                if (!nHL.IsNullOrEmpty())
                {
                    rHJ.MAILCODE = nHL.MAILCODE;
                    rHJ.JUSO1 = nHL.JUSO;
                    rHJ.JUSO2 = nHL.JUSODETAIL;
                }
            }

            return rHJ;
        }

        private string Rtn_Visit_PrintReceipt(int argIdx)
        {
            string rtnVal = string.Empty;

            //for (int i = 0; i < 5; i++)
            //{
            //    switch (i)
            //    {   //알림톡
            //        case 0: if (rdoRES15.Checked) { rtnVal = "Y"; } break;
            //        case 1: if (rdoRES25.Checked) { rtnVal = "Y"; } break;
            //        case 2: if (rdoRES35.Checked) { rtnVal = "Y"; } break;
            //        case 3: if (rdoRES45.Checked) { rtnVal = "Y"; } break;
            //        case 4: if (rdoRES55.Checked) { rtnVal = "Y"; } break;
            //        default: break;
            //    }
            //}

            switch (argIdx)
            {   //알림톡
                case 0: if (rdoRES15.Checked) { rtnVal = "Y"; } break;
                case 1: if (rdoRES25.Checked) { rtnVal = "Y"; } break;
                case 2: if (rdoRES35.Checked) { rtnVal = "Y"; } break;
                case 3: if (rdoRES45.Checked) { rtnVal = "Y"; } break;
                case 4: if (rdoRES55.Checked) { rtnVal = "Y"; } break;
                default: break;
            }


            return rtnVal;
        }

        private HIC_JEPSU_WORK Jepsu_Work_Data_Build(int argIdx)
        {
            string strDental = "";
            string strBangi = string.Compare(VB.Mid(dtpJepDate.Text, 6, 2), "7") < 0 ? "1" : "2";   //검진반기

            HIC_EXJONG hJong = hicExjongService.GetItembyCode(clsHcType.THC[argIdx].GJJONG);        //검진종류 정보
            GROUPCODE_EXAM_DISPLAY rGED1 = lstgrpExam(argIdx).Find(x => x.EXCODE == "ZD00");        //구강검사여부 1
            GROUPCODE_EXAM_DISPLAY rGED2 = lstgrpExam(argIdx).Find(x => x.EXCODE == "ZD01");        //구강검사여부 1

            if (!rGED1.IsNullOrEmpty() && rGED1.EXCODE == "ZD00") { strDental = "Y"; }  //구강검사여부
            if (!rGED2.IsNullOrEmpty() && rGED2.EXCODE == "ZD01") { strDental = "Y"; }  //구강검사여부
            
            //암접수 구분자
            string strGbAm = string.Empty;
            if (clsHcType.THC[argIdx].GJJONG == "31")
            {
                for (int i = 1; i < 8; i++)
                {
                    switch (i)
                    {
                        case 1: strGbAm += chkAm1.Checked ? "1," : "0,"; break;
                        case 2: strGbAm += chkAm2.Checked ? "1," : "0,"; break;
                        case 3: strGbAm += chkAm3.Checked ? "1," : "0,"; break;
                        case 4: strGbAm += chkAm4.Checked ? "1," : "0,"; break;
                        case 5: strGbAm += chkAm5.Checked ? "1," : "0,"; break;
                        case 6: strGbAm += chkAm6.Checked ? "1," : "0,"; break;
                        case 7: strGbAm += chkAm7.Checked ? "1," : "0,"; break;
                        default: break;
                    }
                }

            }
            else
            {
                strGbAm = "0,0,0,0,0,0,0,";
            }

            HIC_JEPSU_WORK rHJW = new HIC_JEPSU_WORK
            {
                GJYEAR      = cboYear.Text,                                 JEPDATE     = dtpJepDate.Text,
                PANO        = txtPano.Text.To<long>(),                      SNAME       = txtSName.Text.Trim(),
                SEX         = cboSex.Text,                                  AGE         = txtAge.Text.To<long>(),
                GJJONG      = clsHcType.THC[argIdx].GJJONG,                 TEL         = txtTel.Text.Trim(),
                GJCHASU     = hJong.CHASU,                                  GJBANGI     = strBangi,
                LTDCODE     = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0), BUSENAME    = txtBuse.Text.Trim(),
                MAILCODE    = txtMail.Text.Trim(),                          JUSO1       = txtJuso1.Text.Trim(),
                JUSO2       = txtJuso2.Text.Trim(),                         UCODES      = clsHcType.THC[argIdx].UCODES,
                GBN         = rdoSchool1.Checked ? "1" : "2",               GBAM        = strGbAm,
                CLASS       = txtClass.Text.To<long>(),                     BAN         = txtBan.Text.To<long>(),
                BUN         = txtBun.Text.To<long>(),                       SEXAMS      = clsHcType.THC[argIdx].SEXAMS,
                PTNO        = txtPtno.Text.Trim(),                          BURATE      = FstrBuRate[argIdx],
                JISA        = VB.Pstr(txtJisa.Text.Trim(), ".", 1),         KIHO        = VB.Pstr(txtKiho.Text, ".", 1).Trim(),
                GKIHO       = txtGKiho.Text.Trim(),                         JUMINNO     = txtJumin1.Text + VB.Left(txtJumin2.Text, 1) + "******",
                GBEXAM      = "",                                           GBCHUL      = chkChul.Checked ? "Y" : "N",
                GBDENTAL    = strDental,                                    GBINWON     = hJong.GBINWON,
                JOBSABUN    = clsType.User.IdNumber.To<long>(),             BOGUNSO     = VB.Pstr(txtBoKiho.Text, ".", 1).Trim(),
                IPSADATE    = dtpIpsaDate.Text,                             MURYOAM     = cboMuRyoAm.Text.Trim() == "" ? "" : cboMuRyoAm.Text.Trim() == "Y" ? "Y" : "N",
                EMAIL       = txtEmail.Text.Trim(),                         REMARK      = clsHcType.THC[argIdx].REMARK,
                JUMINNO2    = clsAES.AES(txtJumin1.Text + txtJumin2.Text),  HPHONE      = txtHphone.Text.Trim(),
                SABUN       = txtLtdSabun.Text,                             GBADDPAN    = clsHcType.THC[argIdx].GJJONG == "69" ? "Y" : "N"
            };

            return rHJW;
        }

        private HIC_PATIENT Patient_Data_Build()
        {
            HIC_PATIENT rPat = new HIC_PATIENT();
            
            rPat.SNAME        = txtSName.Text.Trim();
            rPat.JUMIN        = txtJumin1.Text + VB.Left(txtJumin2.Text, 1) + "******";
            rPat.JUMIN2       = clsAES.AES(txtJumin1.Text + txtJumin2.Text);
            rPat.SEX          = cboSex.Text;
            rPat.MAILCODE     = txtMail.Text.Trim();
            rPat.JUSO1        = txtJuso1.Text.Trim();
            rPat.JUSO2        = txtJuso2.Text.Trim();
            rPat.BUILDNO      = FstrBuildNo;
            rPat.TEL          = txtTel.Text.Trim();
            rPat.HPHONE       = txtHphone.Text.Trim();
            rPat.LTDCODE      = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
            rPat.KIHO         = VB.Pstr(txtKiho.Text, ".", 1).Trim();
            rPat.JIKGBN       = VB.Pstr(txtJikJong.Text, ".", 1).Trim();
            rPat.BUSENAME     = txtBuse.Text.Trim();
            rPat.JISA         = VB.Pstr(txtJisa.Text, ".", 1).Trim();
            rPat.GKIHO        = txtGKiho.Text.Trim();
            rPat.IPSADATE     = dtpIpsaDate.Text;
            rPat.BOGUNSO      = VB.Pstr(txtBoKiho.Text, ".", 1).Trim();
            rPat.EMAIL        = txtEmail.Text.Trim();
            rPat.GBSMS        = chkSMS.Checked ? "Y" : "N";
            rPat.GBPRIVACY    = lblPrvAgr.Text;
            rPat.GBFOREIGNER  = chkForeign.Checked ? "Y" : "N";
            rPat.ENAME        = txtEName.Text.Trim();
            rPat.FOREIGNERNUM = txtForeignerNum.Text.Trim();
            rPat.PTNO         = txtPtno.Text;
            rPat.PANO         = txtPano.Text.To<long>();
            rPat.GONGJENG     = VB.Pstr(txtGongjeng.Text, ".", 1).Trim();
            rPat.JIKJONG      = VB.Pstr(txtJikJong.Text, ".", 1).Trim();

            return rPat;
        }

        private bool Jepsu_Check_Logic(int nIdx)
        {
            //생물학적노출지표검사 안내문 동의서 대상구분(FstrD56,FstrD57)
            string strGbXray = "";
            FstrD56 = ""; FstrD57 = "";

            //특수소변안내문
            if (!clsHcType.THC[nIdx].UCODES.IsNullOrEmpty() && FstrJepsuGbn == "")
            {
                for (int i = 0; i < lstUrinfo.Count; i++)
                {
                    if (clsHcType.THC[nIdx].UCODES.Trim().Contains(lstUrinfo[i].CODE.To<string>("").Trim()))
                    {
                        FstrD56 = "OK";
                        MessageBox.Show("특수소변안내문이 필요함", "확인");
                        break;
                    }

                }
            }

            //채혈안내문
            if (!clsHcType.THC[nIdx].UCODES.IsNullOrEmpty() && FstrJepsuGbn == "")
            {
                for (int i = 0; i < lstBlnfo.Count; i++)
                {
                    if (clsHcType.THC[nIdx].UCODES.Trim().Contains(lstBlnfo[i].CODE.To<string>("").Trim()))
                    {
                        FstrD57 = "OK";
                        MessageBox.Show("채혈안내문이 필요함", "확인");
                        break;
                    }
                }
            }

            //채용검진 특검종류 선택
            if (clsHcType.THC[nIdx].GJJONG == "21" || clsHcType.THC[nIdx].GJJONG == "23") 
            {
                if (cboGbSpc.Text.Trim() == "")
                {
                    MessageBox.Show("채용/특검 검진의 특검종류가 공백입니다.", "확인");
                    return false;
                }

                if (dtpJenipDate.Text.Trim() == "")
                {
                    MessageBox.Show("채용/특검 검진의 현직전입일자가 공백입니다.", "확인");
                    return false;
                }
            }

            //2020-09-08(현작업공정 오류팝업 추가)
            if (clsHcType.THC[nIdx].GJJONG == "21" || clsHcType.THC[nIdx].GJJONG == "22" || clsHcType.THC[nIdx].GJJONG == "23" || clsHcType.THC[nIdx].GJJONG == "24" || clsHcType.THC[nIdx].GJJONG == "30" || clsHcType.THC[nIdx].GJJONG == "49")
            {
                if (txtGongjeng.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("현작업공정이 공백입니다.", "확인");
                    return false;
                }
            }
                

            //사업장 팩스번호 체크
            if (clsHcType.THC[nIdx].GJJONG == "22" && clsHcType.THC[nIdx].GJJONG == "24")
            {
                if (txtFax.Text.Trim() == "")
                {
                    MessageBox.Show("팩스번호가 없는 사업장입니다. 확인해주세요.", "확인");
                }
            }

            long nBonAmt = txtBonAmt.Text.Replace(",", "").To<long>(0);
            long nCardAmt = txtCardAmt.Text.Replace(",", "").To<long>(0);
            long nCashAmt = txtCashAmt.Text.Replace(",", "").To<long>(0);

            //입금금액 오류 점검
            if (nBonAmt != (nCardAmt + nCashAmt))
            {
                clsPublic.GstrMsgList = "본인부담: " + txtBonAmt.Text + "원" + ComNum.VBLF;
                clsPublic.GstrMsgList += "카드금액: " + txtCardAmt.Text + "원" + ComNum.VBLF;
                clsPublic.GstrMsgList += "현금금액: " + txtCashAmt.Text + "원" + ComNum.VBLF;
                clsPublic.GstrMsgList += "카드금액 + 현금금액의 합이 본인부담금액과 틀립니다.";
                MessageBox.Show(clsPublic.GstrMsgList, "저장불가");
                return false;
            }

            //회사부담액이 있으면서 회사코드가 없으면 오류 처리
            if (txtLtdAmt.Text.Replace(",", "").To<long>(0) != 0 && VB.Pstr(txtLtdCode.Text, ".", 1).To<long>(0) == 0)
            {
                MessageBox.Show("회사부담액이 있으나 회사코드가 공란입니다.", "저장불가");
                return false;
            }

            //흡연,음주 대상자 점검
            string strChkSmoke = ""; string strChkDrink = "";
            List<READ_SUNAP_ITEM> lstTemp = new List<READ_SUNAP_ITEM>();
            lstTemp = lstsuInfo(nIdx);
            for (int i = 0; i < lstTemp.Count; i++)
            {
                if (lstTemp[i].RowStatus != RowStatus.Delete)
                {
                    if (lstTemp[i].GRPCODE.To<string>("").Trim() == "1165")
                    {
                        strChkSmoke = "OK";
                    }
                    else if (lstTemp[i].GRPCODE.To<string>("").Trim() == "1166")
                    {
                        strChkDrink = "OK";
                    }
                }
                
            }

            if (strChkSmoke == "OK" || strChkDrink == "OK")
            {
                string strMunjin = cHMF.CHECK_MUNJIN(txtPtno.Text, strChkSmoke, strChkDrink, cboSex.Text, txtAge.Text.To<int>(0),cboYear.Text);
                if (strMunjin.Contains("OK"))
                {
                    return false;
                }
            }

            //결과통보자 수정 금지
            if (clsHcType.THC[nIdx].WRTNO > 0)
            {
                if (!hicJepsuService.GetTongboDateByWrtno(clsHcType.THC[nIdx].WRTNO).IsNullOrEmpty())
                {
                    if (clsHcVariable.GbHicAdminSabun || clsHcVariable.GbHicJupsuAdminSabun)
                    {
                        clsPublic.GstrMsgList = "검진결과가 이미 통보되었습니다" + ComNum.VBLF;
                        clsPublic.GstrMsgList += "정말로 접수 수정을 하시겠습니까?" + ComNum.VBLF;

                        if (MessageBox.Show(clsPublic.GstrMsgList, "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("검진결과가 통보되어 접수수정이 불가능합니다.", "저장불가");
                        return false;
                    }
                }
            }

            //자격조회 
            clsHcType.TEC_CLEAR("PART1");

            for (int i = 0; i < lstTemp.Count; i++)
            {
                if (lstTemp[i].RowStatus != RowStatus.Delete)
                {
                    cHMF.READ_EXAM_CODE_CHK("00", lstTemp[i].GRPCODE.To<string>("").Trim(), clsHcType.TEC.EXAMB, clsHcType.THC[nIdx].GJJONG);
                    cHMF.READ_EXAM_CODE_CHK("00", lstTemp[i].GRPCODE.To<string>("").Trim(), clsHcType.TEC.EXAMC, clsHcType.THC[nIdx].GJJONG);
                }
            }

            if (cHMF.READ_EXAM_CODE_CHK("01", "", clsHcType.TEC.EXAMB, clsHcType.THC[nIdx].GJJONG)) { return false; }
            if (cHMF.READ_EXAM_CODE_CHK("01", "", clsHcType.TEC.EXAMC, clsHcType.THC[nIdx].GJJONG)) { return false; }

            //보건소 암의 경우 부담율 보건소 100% 세팅 점검
            if (clsHcType.THC[nIdx].GJJONG == "31")
            {
                if (VB.Left(txtGKiho.Text, 1) == "9")
                {
                    if (VB.Left(cboBuRate.Text, 2) != "11")
                    {
                        MessageBox.Show("증번호가 9로 시작하면 부담율이 보건소 100% 이어야 합니다.", "점검");
                        return false;
                    }
                }

                if ((chkAm1.Checked == false) && (chkAm2.Checked == false) && (chkAm3.Checked == false) && (chkAm4.Checked == false) && (chkAm5.Checked == false) && (chkAm6.Checked == false) && (chkAm7.Checked == false ))
                {
                    MessageBox.Show("암검진 세부항목을 선택해주세요.", "점검");
                    return false;
                }

                if (VB.Left(txtGKiho.Text, 1) == "9" || VB.Left(cboBuRate.Text, 2) == "11" )
                {
                    if ( chkGubAm.Checked == false)
                    {
                        MessageBox.Show("부담률 11인데, 의료급여암 체크가 누락되었습니다.", "점검");
                        return false;
                    }
                }
            }

            //수납건수 체크
            if (lstTemp.Count == 0)
            {
                MessageBox.Show("묶음코드가 1건도 없습니다.", "수납불가");
                return false;
            }

            //학생검진 비만학생 비만검사항목 확인
            bool bSCH = false;
            if (clsHcType.THC[nIdx].GJJONG == "56" && txtHeight.Text.To<long>(0) > 0)
            {
                if (lblBMI.Text == "정상")
                {
                    for (int i = 0; i < lstTemp.Count; i++)
                    {
                        if (lstTemp[i].RowStatus != RowStatus.Delete)
                        {
                            if (VB.InStr(lstTemp[i].GRPNAME, "학생검진") > 0 && VB.InStr(lstTemp[i].GRPNAME, "비만") > 0)
                            {
                                MessageBox.Show("학생검진 정상인데 비만 묶음코드가 있습니다.", "확인");
                                return false;
                            }
                        }
                    }
                }
                else if (lblBMI.Text == "비만")
                {
                    if (rdoSchool1.Checked && txtClass.Text == "4") { bSCH = true; }    //초등학교 4학년
                    if (rdoSchool2.Checked && txtClass.Text == "1") { bSCH = true; }    //중,고등학교 1학년

                    if (bSCH)
                    {
                        bSCH = false;
                        for (int i = 0; i < lstTemp.Count; i++)
                        {
                            if (lstTemp[i].RowStatus != RowStatus.Delete)
                            {
                                if (VB.InStr(lstTemp[i].GRPNAME, "학생검진") > 0 && VB.InStr(lstTemp[i].GRPNAME, "비만") > 0)
                                {
                                    bSCH = true;
                                }
                            }
                        }

                        if (bSCH == false)
                        {
                            MessageBox.Show("학생검진 비만인데 비만 묶음코드가 없습니다.", "확인");
                            return false;
                        }
                    }
                }
            }

            //출장검진 접수확인
            if (chkChul.Checked)
            {
                if (MessageBox.Show("출장검진으로 접수를 하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }
            }


            #region 출장검진 흉부촬영번호  누락 점검
            //출장검진 흉부촬영번호  누락 점검
            if (chkChul.Checked && (clsHcType.THC[nIdx].GBUSE == "N"||clsHcType.THC[nIdx].GBUSE == "GJ"))
            {
                if (nIdx == 0)
                {
                    if (VB.Trim(txtXrayNo1.Text).IsNullOrEmpty())
                    {
                        for (int i = 0; i < lstgrpExam(nIdx).Count; i++)
                        {
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "A142") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX13") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX14") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX11") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "A213") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX16") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "A211") { strGbXray = "Y"; break; }
                        }
                        if(strGbXray =="Y")
                        {
                            MessageBox.Show("1번접수 흉부촬영 번호가 공란입니다.", "오류");
                            return false;
                        }
                    }
                }
                else if (nIdx == 1)
                {
                    if (VB.Trim(txtXrayNo2.Text).IsNullOrEmpty())
                    {
                        for (int i = 0; i < lstgrpExam(nIdx).Count; i++)
                        {
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "A142") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX13") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX14") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX11") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "A213") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX16") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "A211") { strGbXray = "Y"; break; }
                        }
                        if (strGbXray == "Y")
                        {
                            MessageBox.Show("2번접수 흉부촬영 번호가 공란입니다.", "오류");
                            return false;
                        }
                    }
                }
                else if (nIdx == 2)
                {
                    if (VB.Trim(txtXrayNo3.Text).IsNullOrEmpty())
                    {
                        for (int i = 0; i < lstgrpExam(nIdx).Count; i++)
                        {
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "A142") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX13") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX14") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX11") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "A213") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX16") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "A211") { strGbXray = "Y"; break; }
                        }
                        if (strGbXray == "Y")
                        {
                            MessageBox.Show("3번접수 흉부촬영 번호가 공란입니다.", "오류");
                            return false;
                        }
                    }
                }
                else if (nIdx == 3)
                {
                    if (VB.Trim(txtXrayNo4.Text).IsNullOrEmpty())
                    {
                        for (int i = 0; i < lstgrpExam(nIdx).Count; i++)
                        {
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "A142") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX13") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX14") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX11") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "A213") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX16") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "A211") { strGbXray = "Y"; break; }
                        }
                        if (strGbXray == "Y")
                        {
                            MessageBox.Show("4번접수 흉부촬영 번호가 공란입니다.", "오류");
                            return false;
                        }
                    }
                }
                else if (nIdx == 4)
                {
                    if (VB.Trim(txtXrayNo5.Text).IsNullOrEmpty())
                    {
                        for (int i = 0; i < lstgrpExam(nIdx).Count; i++)
                        {
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "A142") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX13") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX14") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX11") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "A213") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "TX16") { strGbXray = "Y"; break; }
                            if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "A211") { strGbXray = "Y"; break; }
                        }
                        if (strGbXray == "Y")
                        {
                            MessageBox.Show("5번접수 흉부촬영 번호가 공란입니다.", "오류");
                            return false;
                        }
                    }
                }
            }
            #endregion

            #region 야간문진표 성별체크로직
            string strNightM = "";
            string strNightF = "";
            for (int i = 0; i < lstsuInfo(nIdx).Count; i++)
            {
                if (lstsuInfo(nIdx)[i].GRPCODE.To<string>("").Trim() == "JV01") { strNightM = "OK"; }
                if (lstsuInfo(nIdx)[i].GRPCODE.To<string>("").Trim() == "JV02") { strNightF = "OK"; }
            }

            //야간작업 남여 오류
            if (strNightM == "OK" || strNightF == "OK")
            {
                if (cboSex.Text == "M" && strNightF == "OK")
                {
                    MessageBox.Show("성별이 남자인데 야간작업(여) 특검 항목이 선택됨", "확인");
                    return false;
                }

                if (cboSex.Text == "F" && strNightM == "OK")
                {
                    MessageBox.Show("성별이 여자인데 야간작업(남) 특검 항목이 선택됨", "확인");
                    return false;
                }
            }
            #endregion


            //접수시 점검사항(자격)

            //주민번호로 외래번호 체크
            if (txtPtno.Text.Trim().Substring(0, 7) != "8100000")
            {
                List<BAS_PATIENT> lstBpt = basPatientService.GetListByJuminNo(txtJumin1.Text, clsAES.AES(txtJumin2.Text), clsHcVariable.B04_NOT_PATIENT);

                if (lstBpt.Count > 1)
                {
                    if (MessageBox.Show("외래차트번호가 2건이상입니다. 계속 진행하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return false;
                    }
                }
                else
                {
                    if (txtSName.Text.Trim() != lstBpt[0].SNAME.Trim())
                    {
                        clsPublic.GstrMsgList = "외래성명과 이름이 다름니다 그래도 진행할까요?" + ComNum.VBLF + ComNum.VBLF;
                        clsPublic.GstrMsgList += "입력하신 이름: " + txtSName.Text.Trim() + ComNum.VBLF;
                        clsPublic.GstrMsgList += "외래 환자마스타 이름: " + lstBpt[0].SNAME.Trim() + ComNum.VBLF;

                        if (MessageBox.Show(clsPublic.GstrMsgList, "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return false;
                        }
                    }
                }
            }

            //검진종류 체크(한해 같은 검사)
            List<HIC_JEPSU> lstHJP = hicJepsuService.GetHistoryByPtnoYear(txtPtno.Text, cboYear.Text);

            if (lstHJP.Count > 0)
            {
                if (clsHcType.THC[nIdx].GBUSE == "N")  //신규접수만 점검
                {
                    for (int i = 0; i < lstHJP.Count; i++)
                    {
                        if (lstHJP[i].GJJONG == clsHcType.THC[nIdx].GJJONG)
                        {
                            if (clsHcType.THC[nIdx].GBUSE == "N")  //신규접수만 점검
                            {
                                if (lstHJP[i].JEPDATE == dtpJepDate.Text)
                                {
                                    MessageBox.Show("동일 날짜에 같은 건진종류 중복접수는 불가능합니다.", "오류");
                                    return false;
                                }
                            }
                            else
                            {
                                if (lstHJP[i].JEPDATE != dtpJepDate.Text)
                                {
                                    clsPublic.GstrMsgList = "년도 같은종류로 검사를 하였습니다." + ComNum.VBLF + ComNum.VBLF;
                                    clsPublic.GstrMsgList += "검사일자: " + lstHJP[i].JEPDATE + ComNum.VBLF;
                                    clsPublic.GstrMsgList += "그래도 접수하시겠습니까?" + ComNum.VBLF;

                                    if (MessageBox.Show(clsPublic.GstrMsgList, "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            int nCNT = 0; int nCNT2 = 0;

            if (FbHuilGasan)
            {
                if (clsHcType.THC[nIdx].GJJONG == "11")
                {
                    //휴일가산 누락체크
                    //for (int i = 0; i < lstgrpExam(nIdx).Count; i++)
                    //{
                    //    if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "AA01") { nCNT += 1; }
                    //    if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "AA41") { nCNT2 += 1; }
                    //}

                    for (int i = 0; i < lstsuInfo(nIdx).Count; i++)
                    {
                        if (lstsuInfo(nIdx)[i].GRPCODE.To<string>("").Trim() == "1151") { nCNT += 1; }
                        if (lstsuInfo(nIdx)[i].GRPCODE.To<string>("").Trim() == "1116") { nCNT2 += 1; }
                    }


                    if (nCNT != nCNT2)
                    {
                        if (nCNT == 0 && nCNT2 > 0)
                        {
                            MessageBox.Show("1차 진찰료가 없는데 휴일30%가산 코드가 있습니다.", "오류");
                        }

                        if (nCNT > 0 && nCNT2 == 0)
                        {
                            MessageBox.Show("1차 진찰료 휴일30%가산 코드가 누락되었습니다.", "오류");
                        }
                    }

                    nCNT = 0; nCNT2 = 0;

                    //for (int i = 0; i < lstgrpExam(nIdx).Count; i++)
                    //{
                    //    if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "ZD00") { nCNT += 1; }
                    //    if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "ZD01") { nCNT += 1; }
                    //    if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "AA43") { nCNT2 += 1; }
                    //}

                    for (int i = 0; i < lstsuInfo(nIdx).Count; i++)
                    {
                        if (lstsuInfo(nIdx)[i].GRPCODE.To<string>("").Trim() == "1157") { nCNT += 1; }
                        if (lstsuInfo(nIdx)[i].GRPCODE.To<string>("").Trim() == "1118") { nCNT2 += 1; }
                    }


                    if (nCNT != nCNT2)
                    {
                        if (nCNT == 0 && nCNT2 > 0)
                        {
                            MessageBox.Show("구강상담이 없는데 휴일30%가산 코드가 있습니다.", "오류");
                        }
                        if (nCNT > 0 && nCNT2 == 0)
                        {
                            MessageBox.Show("구강상담 휴일30%가산 코드가 누락되었습니다.", "오류");
                        }
                    }

                    nCNT = 0; nCNT2 = 0;

                    for (int i = 0; i < lstsuInfo(nIdx).Count; i++)
                    {
                        if (lstsuInfo(nIdx)[i].GRPCODE.To<string>("").Trim() == "1601") { nCNT += 1; }
                        if (lstsuInfo(nIdx)[i].GRPCODE.To<string>("").Trim() == "1117") { nCNT2 += 1; }
                    }

                    if (nCNT != nCNT2)
                    {
                        if (nCNT == 0 && nCNT2 > 0)
                        {
                            MessageBox.Show("2차검진 진찰코드가 없는데 휴일30%가산 코드가 있습니다.", "오류");
                        }
                        if (nCNT > 0 && nCNT2 == 0)
                        {
                            MessageBox.Show("2차검진 휴일30%가산 코드가 누락되었습니다.", "오류");
                        }
                    }
                }
                else
                {
                    //휴일가산 누락체크
                    for (int i = 0; i < lstgrpExam(nIdx).Count; i++)
                    {
                        if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "AA21") { nCNT += 1; }
                        if (lstgrpExam(nIdx)[i].EXCODE.To<string>("").Trim() == "AA44") { nCNT2 += 1; }
                    }

                    if (nCNT != nCNT2)
                    {
                        if (nCNT == 0 && nCNT2 > 0)
                        {
                            MessageBox.Show("암검진 진찰코드가 없는데 휴일30%가산 코드가 있습니다", "오류");
                        }

                        if (nCNT > 0 && nCNT2 == 0)
                        {
                            MessageBox.Show("암검진 휴일30%가산 코드가 누락되었습니다.", "오류");
                        }
                    }
                }
                
            }




            return true;
        }

        private bool Report_OldAmt_Cancel(HIC_JEPSU nHJ, int nIdx)
        {
            try
            {
                HIC_SUNAP item = hicSunapService.GetHicSunapAmtByWRTNO(nHJ.WRTNO);

                //수납영수증 취소내역을 INSERT
                item.SEQNO = hicSunapService.GetMaxSeqbyWrtNo(nHJ.WRTNO);
                item.WRTNO = nHJ.WRTNO;
                item.PANO = nHJ.PANO;
                item.HALINGYE = hicSunapService.GetHalinGyeByWrtnoSeqNo(nHJ.WRTNO, item.SEQNO - 1);
                item.MISUGYE = hicSunapService.GetMisuGyeByWrtnoSeqNo(nHJ.WRTNO, item.SEQNO - 1);

                hicSunapService.InsertMinusSunapData(item);

                HIC_SUNAP item2 = hicSunapService.GetHicSunapAmtByWRTNO(nHJ.WRTNO);
                
                if (item2.BONINAMT > 0)
                {
                    if (MessageBox.Show(nHJ.GJJONG + " 종 영수증을 발생하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        item2.PANO = clsHcType.THC[nIdx].PANO;
                        item2.WRTNO = clsHcType.THC[nIdx].WRTNO;
                        item2.PTNO = clsHcType.THC[nIdx].PTNO;
                        item2.JEPDATE = dtpJepDate.Text;
                        item2.SNAME = txtSName.Text;
                        item2.DEPTCODE = "일반건진(HR)";

                        cHPrt.Receipt_Report_Print(nHJ, item2);
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

        private void CardApprov_Amt(string strJob, int nIdx)
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
            CARD.gstrCdDeptCode = nIdx == 3 ? "TO" : "HR";
            CARD.gstrCdPart = clsType.User.IdNumber;
            CARD.gstrCdGbIo = "H";
            CARD.GnHPano = txtPano.Text.To<long>(0);
            CARD.GnHWRTNO = nIdx; 

            if (strJob == "CARD")
            {
                strAmt = VB.Replace(txtCardAmt.Text, ",", "");
            }
            else
            {
                strAmt = VB.Replace(txtCashAmt.Text, ",", "");
            }

            CARD.glngCdAmt = (long)Math.Round(VB.Val(strAmt), 0, MidpointRounding.AwayFromZero);

            CARD.gstrCdPCode = "SUP+";

            frmHcEntryCardDaou frm = new frmHcEntryCardDaou(CARD.gstrCdPtno, CARD.gstrCdSName, CARD.gstrCdDeptCode, CARD.gstrCdGbIo, CARD.glngCdAmt, strJob, clsPublic.GstrSysDate, CARD.GnHPano, CARD.GnHWRTNO);
            frm.ShowDialog();
            cPF.fn_ClearMemory(frm);

        }

        /// <summary>
        /// 자격조회 초기화
        /// </summary>
        /// <param name="argJumin"></param>
        private void Clear_Nhic_List()
        {
            string strJumin = txtJumin1.Text + txtJumin2.Text;
            string strGubun = "";

            if (strJumin.IsNullOrEmpty()) { return; }

            int result = workNhicService.DeleteDataAllByJuminNo(clsAES.AES(strJumin));
            if (result <= 0)
            {
                MessageBox.Show("자격조회 초기화 실패", "에러");
                return;
            }

            int nIdx = tbCtrl_Exam.SelectedTabIndex;

            if (GnWRTNO > 0) { strGubun = "Y"; }
            Hic_Chk_Nhic("H", txtSName.Text, strJumin, txtPtno.Text, cboYear.Text, nIdx, strGubun);
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
        ///  <param name="argGubun"></param> 신규/수정 구분
        private void Hic_Chk_Nhic(string argGbn, string argSName, string argJuminNo, string argPtno, string argYear, int nIdx = 0, string argGubun = "")
        {
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

                cHcMain.Display_Nhic_Info(item2);

                Set_Exam_Select(nIdx);

                //2021-01-29(연장표시)
                if (!item2.IsNullOrEmpty())
                {
                    if (!item2.REMARK.IsNullOrEmpty())
                    {
                        MessageBox.Show(item2.REMARK, "확인");
                    }
                }
            }
            else
            {
                Screen_Clear_Nhic();
                //자격조회 정보 Spread Display
                SetDisPlay_Nhic(item);

                cHcMain.Display_Nhic_Info(item);

                Set_Exam_Select(nIdx);

                if (!item.IsNullOrEmpty())
                {
                    if (!item.REMARK.IsNullOrEmpty())
                    {
                        MessageBox.Show(item.REMARK, "확인");
                    }
                }
            }

            if (clsHcType.THNV.hJaGubun.IsNullOrEmpty())
            {
                MessageBox.Show("자격이 없습니다.", "확인요망");
                return;
            }



            if (clsHcType.THNV.hKiho.To<string>("").Trim() != "")
            {
                txtKiho.Text = clsHcType.THNV.hKiho.To<string>("").Trim();
                txtKiho.Text += "." + hicCodeService.GetNameByGubunCode("18", clsHcType.THNV.hKiho);
            }
            
            txtGKiho.Text = clsHcType.THNV.hGKiho;
            if (clsHcType.THNV.hJisa.To<string>("").Trim() != "")
            {
                txtJisa.Text = cHB.Jisa2_Code2Name(clsHcType.THNV.hJisa.Trim()).Trim();
                txtJisa.Text += "." + clsHcType.THNV.hJisa.Trim();
            }

            if (clsHcType.THNV.hBogen.To<string>("").Trim() != "")
            {
                txtBoKiho.Text = clsHcType.THNV.hBogen.To<string>("").Trim();
                txtBoKiho.Text += "." + hicCodeService.GetNameByGubunCode("25", clsHcType.THNV.hBogen.To<string>("").Trim()).Trim();
            }
            else
            {
                txtBoKiho.Text = "";
            }
            
            if (!clsHcType.THNV.hGetDate.IsNullOrEmpty())
            {
                dtpIpsaDate.Text = clsHcType.THNV.hGetDate;
            }

            //2020-09-07, 2020-11-02제외처리
            //if (clsHcType.TEC.EXAMC.Contains("Y"))
            //{
            //    MessageBox.Show("C형간염 대상자입니다.", "확인요망");
            //}


            List<string> lstCodes = new List<string>();

            //자격정보에 따라 검사코드 추가 (있다면 제외)
            if (clsHcType.THC[nIdx].GJJONG == "31" && clsHcType.TEC.NHICOK == "Y" && argGubun == "")
            {
                if ((clsHcType.THNV.hCan1.Contains("본인부담없음") || clsHcType.THNV.hCan1.Contains("10%부담") || clsHcType.THNV.hCan1.Contains("10%본인부담")) && !clsHcType.THNV.hCan1.Contains("수검완료"))
                {
                    if (clsHcType.THNV.h위Date.IsNullOrEmpty()) { chkAm2.Checked = true; lstCodes.Add("3111"); }      //암-위
                }  
                if ((clsHcType.THNV.hCan2.Contains("본인부담없음") || clsHcType.THNV.hCan2.Contains("10%부담") || clsHcType.THNV.hCan2.Contains("10%본인부담")) && !clsHcType.THNV.hCan2.Contains("수검완료"))
                {
                    if (clsHcType.THNV.h유방Date.IsNullOrEmpty()) { chkAm5.Checked = true; lstCodes.Add("3123"); }     //암-유방
                }  
                if ((clsHcType.THNV.hCan3.Contains("본인부담없음") || clsHcType.THNV.hCan3.Contains("10%부담") || clsHcType.THNV.hCan3.Contains("10%본인부담")) && !clsHcType.THNV.hCan3.Contains("수검완료"))
                {
                    if (clsHcType.THNV.h대장Date.IsNullOrEmpty()) { chkAm3.Checked = true; lstCodes.Add("3116"); }    //암-대장
                }  
                if ((clsHcType.THNV.hCan4.Contains("본인부담없음") || clsHcType.THNV.hCan4.Contains("10%부담") || clsHcType.THNV.hCan4.Contains("10%본인부담")) && !clsHcType.THNV.hCan4.Contains("수검완료"))
                {
                    if (clsHcType.THNV.h간Date.IsNullOrEmpty()) { chkAm4.Checked = true; lstCodes.Add("3115"); }     //암-간암
                }  
                if ((clsHcType.THNV.hCan6.Contains("본인부담없음") || clsHcType.THNV.hCan6.Contains("10%부담") || clsHcType.THNV.hCan6.Contains("10%본인부담")) && !clsHcType.THNV.hCan6.Contains("수검완료"))
                {
                    if (clsHcType.THNV.h간Date.IsNullOrEmpty()) { chkAm4.Checked = true; lstCodes.Add("3115"); }     //암-간암
                }  
                if ((clsHcType.THNV.hCan5.Contains("본인부담없음") || clsHcType.THNV.hCan5.Contains("10%부담") || clsHcType.THNV.hCan5.Contains("10%본인부담")) && !clsHcType.THNV.hCan5.Contains("수검완료"))
                {
                    if (clsHcType.THNV.h자궁Date.IsNullOrEmpty()) { chkAm6.Checked = true; lstCodes.Add("3132"); }     //암-자궁
                }  
                if ((clsHcType.THNV.hCan7.Contains("본인부담없음") || clsHcType.THNV.hCan7.Contains("10%부담") || clsHcType.THNV.hCan7.Contains("10%본인부담")) && !clsHcType.THNV.hCan7.Contains("수검완료"))
                {
                    if (clsHcType.THNV.h폐Date.IsNullOrEmpty()) { chkAm7.Checked = true; lstCodes.Add("3169"); }    //암-폐
                }  
            }

            //의료급여 대상자는 고정(2020-09-04 66세이상 추가 적용)
            if (clsHcType.THC[nIdx].GJJONG == "11" && clsHcType.THNV.hJaGubun.To<string>("").Trim() == "의료급여" && Convert.ToInt32(txtAge.Text) >= 66)
            {
                Clear_Spread_GroupCode();
                Clear_Working_Variants(nIdx);

                lstCodes.Add("1151");
                lstCodes.Add("1155");
            }
            
            //생활습관 코드
            if (clsHcType.THC[nIdx].GJJONG == "11")
            {
                List<BAS_BCODE> lstExams = basBcodeService.GetListHicExamMstByGubun(clsHcType.TEC.GUBUN, clsHcType.TEC.SEX, clsHcType.TEC.AGE, clsHcType.TEC.JEPDATE);

                if (lstExams.Count > 0)
                {
                    for (int i = 0; i < lstExams.Count; i++)
                    {
                        if (!lstExams[i].GUBUN2.IsNullOrEmpty())
                        {
                            lstCodes.Add(lstExams[i].GUBUN2.To<string>("").Trim());
                        }
                    }
                }

                if (clsHcType.TEC.EXAMA.Contains("Y")) { lstCodes.Add("1160"); }   //이상지질혈증
                if (clsHcType.TEC.EXAMB.Contains("Y")) { lstCodes.Add("1161"); }   //B형간염
                if (clsHcType.TEC.EXAMD.Contains("Y")) { lstCodes.Add("1162"); }   //골밀도
                if (clsHcType.TEC.EXAME.Contains("Y")) { lstCodes.Add("1163"); }   //인지기능장애
                if (clsHcType.TEC.EXAMF.Contains("Y")) { lstCodes.Add("1167"); }   //정신건강검사
                if (clsHcType.TEC.EXAMG.Contains("Y")) { lstCodes.Add("1164"); lstCodes.Add("1165"); lstCodes.Add("1166"); }   //생활습관평가(2020-08-06 1165,1166 추가)
                if (clsHcType.TEC.EXAMH.Contains("Y")) { lstCodes.Add("1168"); }   //노인신체기능
                //2020-09-01
                //if (clsHcType.TEC.EXAMC.Contains("Y")) { lstCodes.Add("1170"); }   //C형간염

                //if (clsHcType.TEC.EXAMI.Contains("Y")) { lstCodes.Add("1158"); }   //치면세균막
            }

            //코드 추가 
            if (lstCodes.Count > 0)
            {
                frmHcGroupCode_ssDblclick(lstCodes, "");
            }
        }

        private void Set_Exam_Select(int nIdx)
        {
            clsHcType.TEC.JEPDATE = dtpJepDate.Text;
            clsHcType.TEC.JONG = clsHcType.THC[nIdx].GJJONG;
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

            //자격조회 시간
            if (item.CTIME.IsNullOrEmpty())
            {
                SS10.ActiveSheet.Cells[11, 0].Text = "검 진 정 보";
            }
            else
            {
                SS10.ActiveSheet.Cells[11, 0].Text = "검 진 정 보" + " (자격조회시간: " + item.CTIME + " )";
            }

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

            if (item.FIRST.To<string>("").Contains("수검완료"))    { ss10.ActiveSheet.Cells[12, 1].ForeColor = Color.Red; }
            if (item.DENTAL.To<string>("").Contains("수검완료"))   { ss10.ActiveSheet.Cells[13, 1].ForeColor = Color.Red; }
            if (item.LIVER.To<string>("").Contains("수검완료"))    { ss10.ActiveSheet.Cells[14, 1].ForeColor = Color.Red; }
            if (item.LIVERC.To<string>("").Contains("수검완료"))   { ss10.ActiveSheet.Cells[15, 1].ForeColor = Color.Red; }
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
            if (item.EXAMA.To<string>("").Contains("수검완료"))    { ss10.ActiveSheet.Cells[12, 4].ForeColor = Color.Red; }
            if (item.EXAMD.To<string>("").Contains("수검완료"))    { ss10.ActiveSheet.Cells[13, 4].ForeColor = Color.Red; }
            if (item.EXAME.To<string>("").Contains("수검완료"))    { ss10.ActiveSheet.Cells[14, 4].ForeColor = Color.Red; }
            if (item.EXAMF.To<string>("").Contains("수검완료"))    { ss10.ActiveSheet.Cells[15, 4].ForeColor = Color.Red; }
            if (item.EXAMG.To<string>("").Contains("수검완료"))    { ss10.ActiveSheet.Cells[16, 4].ForeColor = Color.Red; }
            if (item.EXAMH.To<string>("").Contains("수검완료"))    { ss10.ActiveSheet.Cells[17, 4].ForeColor = Color.Red; }
            if (item.EXAMI.To<string>("").Contains("수검완료"))    { ss10.ActiveSheet.Cells[18, 4].ForeColor = Color.Red; }

            if (item.GBCHK01 == "수검완료") { ss10.ActiveSheet.Cells[24, 1].ForeColor = Color.Red; }
            if (item.GBCHK02 == "수검완료") { SS10.ActiveSheet.Cells[25, 1].ForeColor = Color.Red; }
            if (item.GBCHK03 == "수검완료") { SS10.ActiveSheet.Cells[26, 1].ForeColor = Color.Red; }
            if (item.GBCHK04 == "수검완료") { SS10.ActiveSheet.Cells[27, 1].ForeColor = Color.Red; }
            if (item.GBCHK05 == "수검완료") { SS10.ActiveSheet.Cells[28, 1].ForeColor = Color.Red; }
            if (item.GBCHK06 == "수검완료") { SS10.ActiveSheet.Cells[29, 1].ForeColor = Color.Red; }
            if (item.GBCHK07 == "수검완료") { SS10.ActiveSheet.Cells[30, 1].ForeColor = Color.Red; }
            if (item.GBCHK09 == "수검완료") { SS10.ActiveSheet.Cells[31, 1].ForeColor = Color.Red; }
            if (item.GBCHK10 == "수검완료") { SS10.ActiveSheet.Cells[32, 1].ForeColor = Color.Red; }

        }

        private void Screen_Clear_Nhic()
        {
            for (int i = 1; i < 11; i++) { SS10.ActiveSheet.Cells[i, 1].Text = ""; }
            for (int i = 12; i < 22; i++) { SS10.ActiveSheet.Cells[i, 1].Text = ""; SS10.ActiveSheet.Cells[i, 4].Text = ""; }
            for (int i = 24; i < 32; i++) { SS10.ActiveSheet.Cells[i, 1].Text = ""; }
            SS10.ActiveSheet.Cells[11, 0].Text = "검 진 정 보";
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
            

            ssETC.DataSource = list;

            if (!list.IsNullOrEmpty() && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    //Size size = ssETC.ActiveSheet.GetPreferredCellSize(i, 2);
                    //ssETC.ActiveSheet.Rows[i].Height    = size.Height;

                    //Row 높이 설정 2020-09-23 
                    FarPoint.Win.Spread.Row row;
                    row = ssETC.ActiveSheet.Rows[i];
                    float rowSize = row.GetPreferredHeight();
                    row.Height = rowSize;

                    ssETC.ActiveSheet.Cells[i, 5].Text  = list[i].JOBNAME;
                }

                //ssETC.AddRows(3);
            }

            ssETC.AddRows(3);

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
                    result = hicMemoService.DeleteData(ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.ROWID].Text);
                }
                else if (ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.ROWID].Text == "")
                {
                    if (ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.MEMO].Text != "")
                    {
                        //Insert Data
                        HIC_MEMO item = new HIC_MEMO
                        {
                            MEMO = ssETC.ActiveSheet.Cells[i, (int)clsHcSpd.enmHcMemo.MEMO].Text,
                            JOBSABUN = clsType.User.IdNumber.To<long>(),
                            PTNO = FstrPtno,
                            PANO = FnPano
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

            frmHcCodeHelp frm = new frmHcCodeHelp(strGB, strFind);
            frm.rSetGstrValue += new frmHcCodeHelp.SetGstrValue(ePost_value_CODE);
            frm.ShowDialog();
            frm.rSetGstrValue -= new frmHcCodeHelp.SetGstrValue(ePost_value_CODE);

            if (!FstrComCode.IsNullOrEmpty())
            {
                tx.Text = FstrComCode.Trim();
                tx.Text += "." + FstrComName.Trim();
            }
            else
            {
                if (VB.Pstr(tx.Text, ".", 1).Trim() == "") { tx.Text = ""; }
            }
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
            }
            else
            {
                if (VB.Pstr(txtLtdCode.Text, ",", 1).Trim() == "")
                {
                    txtLtdCode.Text = "";
                }
            }
        }

        /// <summary>
        /// 선택검사 코드 검색창 연동
        /// </summary>
        /// <param name="strGB"></param>
        private void Hic_GroupCode_Help(string strJong, string strGB, List<READ_SUNAP_ITEM> lstCodes)
        {
            string strCodes = string.Empty;
            int nIdx = tbCtrl_JONG.SelectedTabIndex;

            if (strJong == "") { strJong = clsHcType.THC[nIdx].GJJONG; }

            frmHcGroupCode.rSndMsg += new frmHcGroupCode.rSendMsg(frmHcGroupCode_ssDblclick);
            frmHcGroupCode f = new frmHcGroupCode(strJong, strGB, lstCodes);
            f.ShowDialog();
            frmHcGroupCode.rSndMsg -= new frmHcGroupCode.rSendMsg(frmHcGroupCode_ssDblclick);
        }

        private void frmHcGroupCode_ssDblclick(List<READ_SUNAP_ITEM> argCode)
        {
            if (argCode.Count == 0) { return; }

            List<string> lstgrpCode = new List<string>();

            //저장된 검진정보에 ADD
            int nIdx = tbCtrl_JONG.SelectedTabIndex;

            List<READ_SUNAP_ITEM> vRSI = new List<READ_SUNAP_ITEM>();                   //스프레드 내용을 담은 코드
            READ_SUNAP_ITEM_Get_Spread(vRSI, ssGroup);

            List<READ_SUNAP_ITEM> delRSI = new List<READ_SUNAP_ITEM>();                 //삭제코드
            List<READ_SUNAP_ITEM> insRSI = new List<READ_SUNAP_ITEM>();                 //추가코드
            List<READ_SUNAP_ITEM> rtnRSI = new List<READ_SUNAP_ITEM>();                 //정리된 코드

            List<GROUPCODE_EXAM_DISPLAY> dupGED = new List<GROUPCODE_EXAM_DISPLAY>();   //그룹코드내 포함시키지 않을 검사항목
            List<GROUPCODE_EXAM_DISPLAY> rtnGED = new List<GROUPCODE_EXAM_DISPLAY>();   //그룹코드를 조회 후 작성된 검사항목

            delRSI = vRSI.Except(argCode).ToList();    //스프레드에서 제외시킬 코드
            insRSI = argCode.Except(vRSI).ToList();    //스프레드에 추가시킬 코드

            rtnRSI = Remake_Overlap_GroupList(vRSI, delRSI, insRSI);    //선택 및 제외한 그룹코드 정리

            //선택코드 재정렬
            var newList = rtnRSI.OrderBy(x => x.HANG).ToList();

            //2020-10-13 암선택오류로 수정
            if (clsHcType.THC[nIdx].GJJONG == "31")
            {
                chkAm1.Checked = false;
                chkAm2.Checked = false; 
                chkAm3.Checked = false; 
                chkAm4.Checked = false; 
                chkAm5.Checked = false; 
                chkAm6.Checked = false; 
                chkAm7.Checked = false; 
            }


            for (int i = 0; i < rtnRSI.Count; i++)
            {
                //선택한 코드 적용
                READ_SUNAP_ITEM item = readSunapItemService.GetItemByCode(newList[i].GRPCODE.To<string>("").Trim());
                //READ_SUNAP_ITEM item = readSunapItemService.GetItemByCode(rtnRSI[i].GRPCODE.To<string>("").Trim());

                if (!item.IsNullOrEmpty())
                {
                    item.GBSELF = "";

                    //종검포폴 선택시 종검내시경 체크박스 표시
                    if (item.GRPCODE.To<string>("").Trim() == "3151") { chkHeaEndo.Checked = true; }
                    if (item.GRPCODE.To<string>("").Trim() == "3111")
                    {
                        if (heaJepsuService.GetEndoGbnbyPtNo(txtPtno.Text) == "1")
                        {
                            chkHeaEndo.Checked = true;
                        }
                    }

                    //암검진 경우 부담율 개별세팅
                    if (clsHcType.THC[nIdx].GJJONG == "31")
                    {
                        Display_GbAm2(item.GBAM);
                        Cancer_GroupCode_BuRate_Set(nIdx, ref item);
                    }

                    //부담율 설정
                    GroupCode_BuRate_Set(clsHcType.THC[nIdx].GJJONG, ref item);

                    //선택 코드 금액 산정 
                    newList[i].AMT = Read_GrpCode_Amt(rtnGED, item.GRPCODE.Trim(), dtpJepDate.Text, dupGED);
                    newList[i].GRPNAME = item.GRPNAME;
                    newList[i].GBSELECT = item.GBSELECT;
                    newList[i].UCODE = item.UCODE;
                    //2020-11-06 수정
                    if (newList[i].GBSELF.IsNullOrEmpty())
                    {
                        newList[i].GBSELF = item.GBSELF;
                    }


                    //rtnRSI[i].AMT = Read_GrpCode_Amt(rtnGED, item.GRPCODE.Trim(), dtpJepDate.Text, dupGED);
                    //rtnRSI[i].GRPNAME = item.GRPNAME;
                    //rtnRSI[i].GBSELECT = item.GBSELECT;
                    //rtnRSI[i].UCODE = item.UCODE;
                    //rtnRSI[i].GBSELF = item.GBSELF;

                    lstgrpCode.Add(newList[i].GRPCODE);
                    //lstgrpCode.Add(rtnRSI[i].GRPCODE);

                    if (lstgrpCode.Count > 0)
                    {
                        dupGED = groupCodeExamDisplayService.GetListExcodeByGrpCodeList(lstgrpCode);
                    }
                }
            }

            #region ====================================그룹코드 정리 부분 =======================================
            //암검진 진찰료 부담율 초기화
            for (int k = 0; k < rtnRSI.Count; k++)
            {
                if (rtnRSI[k].RowStatus != RowStatus.Delete)
                {
                    if (rtnRSI[k].GRPCODE.To<string>("").Trim() == "3101")
                    {
                        rtnRSI[k].GBSELF = "";
                        break;
                    }
                }
            }

            //분변, 자궁 검사 하나라도 있을 경우 진찰료는 01 공단부담으로
            for (int i = 0; i < rtnRSI.Count; i++)
            {
                if (rtnRSI[i].RowStatus != RowStatus.Delete)
                {
                    if (rtnRSI[i].GRPCODE.To<string>("").Trim() == "3116" || rtnRSI[i].GRPCODE.To<string>("").Trim() == "3132")
                    {
                        for (int k = 0; k < rtnRSI.Count; k++)
                        {
                            if (rtnRSI[k].GRPCODE.To<string>("").Trim() == "3101")
                            {
                                rtnRSI[k].GBSELF = "01";

                                if (clsHcType.THNV.hJaGubun.To<string>("").Trim() == "의료급여") { rtnRSI[k].GBSELF = "11"; }
                                break;
                            }
                        }
                    }
                }
            }



            //휴일가산 진찰료 부담율 초기화
            for (int k = 0; k < rtnRSI.Count; k++)
            {
                if (rtnRSI[k].RowStatus != RowStatus.Delete)
                {
                    if (rtnRSI[k].GRPCODE.To<string>("").Trim() == "1119")
                    {
                        rtnRSI[k].GBSELF = "";
                        break;
                    }
                }
            }

            //라미나지액 부담률 강제세팅(3131,3164,3165)
            for (int k = 0; k < rtnRSI.Count; k++)
            {
                if (rtnRSI[k].RowStatus != RowStatus.Delete)
                {
                    if (rtnRSI[k].GRPCODE.To<string>("").Trim() == "3131" || rtnRSI[k].GRPCODE.To<string>("").Trim() == "3164" || rtnRSI[k].GRPCODE.To<string>("").Trim() == "3165")
                    {
                        rtnRSI[k].GBSELF = "03";
                        break;
                    }
                }
            }

            //부담율이 11이 아니고 해당검사(3103,3116,3117,3118,3121,3132,3168)중 하나라도 있을 경우 진찰료는 01 공단부담으로
            for (int i = 0; i < rtnRSI.Count; i++)
            {
                if (rtnRSI[i].RowStatus != RowStatus.Delete)
                {
                    if (rtnRSI[i].GRPCODE.To<string>("").Trim() == "3103" || rtnRSI[i].GRPCODE.To<string>("").Trim() == "3116" || rtnRSI[i].GRPCODE.To<string>("").Trim() == "3117" ||
                        rtnRSI[i].GRPCODE.To<string>("").Trim() == "3118" || rtnRSI[i].GRPCODE.To<string>("").Trim() == "3121" || rtnRSI[i].GRPCODE.To<string>("").Trim() == "3132" ||
                        rtnRSI[i].GRPCODE.To<string>("").Trim() == "3168")
                    {
                        for (int k = 0; k < rtnRSI.Count; k++)
                        {
                            if (rtnRSI[k].GRPCODE.To<string>("").Trim() == "1119")
                            {
                                if (rtnRSI[k].GBSELF != "11") { rtnRSI[k].GBSELF = "01"; }
                                break;
                            }
                        }
                    }
                }
            }




            string strBurate = VB.Pstr(cboBuRate.Text, ".", 1).To<string>(FstrBuRate[nIdx]);
            #endregion

            //Spread에 표시
            lstsuInfo(nIdx).Clear();
            lstsuInfo(nIdx).AddRange(newList);
            //lstsuInfo(nIdx).AddRange(rtnRSI);
            ssGroup.DataSource = null;
            ssGroup.SetDataSource(lstsuInfo(nIdx));

            lstgrpExam(nIdx).Clear();
            lstgrpExam(nIdx).AddRange(rtnGED);
            ssExam.DataSource = null;
            ssExam.SetDataSource(lstgrpExam(nIdx));

            //수납금액 계산하기
            Gesan_Sunap_Amt(nIdx, lstsuInfo(nIdx), strBurate, txtHalinAmt.Text.To<long>());

            SetControl_Special(nIdx, lstsuInfo(nIdx), lstgrpExam(nIdx));

            //자동수가발생
            Auto_Suga_Insert(nIdx);
        }

        //선택코드 재정렬
        static void List_Sort(string[] argCODE)
        {

            var list1 = new List<string>();
            list1.AddRange(argCODE);
            list1.Sort();
            Console.WriteLine("[{0}]", string.Join(", ", list1));

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

                    rRSI.GRPCODE  = ssGroup.ActiveSheet.Cells[i, 1].Text;
                    rRSI.GRPNAME  = ssGroup.ActiveSheet.Cells[i, 2].Text;
                    rRSI.AMT      = ssGroup.ActiveSheet.Cells[i, 3].Text.Replace(",", "").To<long>(0);
                    rRSI.GBSELF   = ssGroup.ActiveSheet.Cells[i, 4].Text;
                    rRSI.GBSELECT = ssGroup.ActiveSheet.Cells[i, 5].Text;
                    rRSI.UCODE    = ssGroup.ActiveSheet.Cells[i, 7].Text;
                    rRSI.RID      = ssGroup.ActiveSheet.Cells[i, 8].Text;

                    HIC_GROUPCODE item = hicGroupcodeService.GetItemByCode(rRSI.GRPCODE);
                    if (!item.IsNullOrEmpty())
                    {
                        rRSI.HANG = item.HANG;
                    }
  
                    ssRSI.Add(rRSI);
                }
            }

        }

        /// <summary>
        /// 스프레드를 비교하려 스프레드를 List 변수로 as 로 받으면 DataSource 링크가 깨지므로 
        /// 비교하려는 스프레드를 아래와 같이 수동으로 복사하여서 사용함
        /// </summary>
        /// <param name="ssRSI">저장 변수</param>
        /// <param name="ssGroup">대상 스프레드</param>
        private void GROUPCODE_EXAM_DISPLAY_Get_Spread(List<GROUPCODE_EXAM_DISPLAY> ssGED, FpSpread ssGroup)
        {
            GROUPCODE_EXAM_DISPLAY rGED = new GROUPCODE_EXAM_DISPLAY();

            for (int i = 0; i < ssGroup.ActiveSheet.RowCount; i++)
            {
                if (ssGroup.ActiveSheet.Cells[i, 0].Text != "True")
                {
                    rGED = new GROUPCODE_EXAM_DISPLAY();

                    rGED.GROUPCODENAME  = ssGroup.ActiveSheet.Cells[i, 1].Text;
                    rGED.EXCODE         = ssGroup.ActiveSheet.Cells[i, 2].Text;
                    rGED.AMT            = ssGroup.ActiveSheet.Cells[i, 3].Text.Replace(",", "").To<long>(0);
                    rGED.EXNAME         = ssGroup.ActiveSheet.Cells[i, 4].Text;
                    rGED.GROUPCODE      = ssGroup.ActiveSheet.Cells[i, 5].Text;

                    HIC_GROUPCODE item = hicGroupcodeService.GetItemByCode(rGED.GROUPCODE);
                    if (!item.IsNullOrEmpty())
                    {
                        rGED.HANG = item.HANG;
                    }

                    ssGED.Add(rGED);
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

            ssRSI.AddRange(insRSI);

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

        /// <summary>
        /// 자동수가 발생 함수
        /// </summary>
        /// <param name="nIdx"></param>
        private void Auto_Suga_Insert(int nIdx)
        {
            List<string> lstGrpCD = new List<string>();

            //일특일때 일특차액 자동추가
            if ((clsHcType.THC[nIdx].GJJONG == "11"|| clsHcType.THC[nIdx].GJJONG == "14" )&& clsHcType.THC[nIdx].UCODES != "")
            {
                lstGrpCD.Add("2302");
                lstGrpCD.Add("J224");

                frmHcGroupCode_ssDblclick(lstGrpCD, "");
            }

            //채용 수검진기본검사코드 자동추가
            if ((clsHcType.THC[nIdx].GJJONG == "22" || clsHcType.THC[nIdx].GJJONG == "23" || clsHcType.THC[nIdx].GJJONG == "24") && clsHcType.THC[nIdx].UCODES != "")
            {
                lstGrpCD.Add("J224");
                frmHcGroupCode_ssDblclick(lstGrpCD, "");
            }


        }

        /// <summary>
        /// 재귀함수로 기존 그룹코드에서 중복건 제거
        /// RemoveAt으로 지우면 처음세팅했던 카운트수가 달라짐
        /// </summary>
        /// <param name="exm"></param>
        private void Remove_Overlap_ExamList1(ref List<GROUPCODE_EXAM_DISPLAY> exm)
        {
            for (int i = 0; i < exm.Count; i++)
            {
                if (exm[i].RowStatus == RowStatus.Delete)
                {
                    exm.RemoveAt(i);
                    Remove_Overlap_ExamList1(ref exm);
                    break;
                }
            }
        }

        /// <summary>
        /// exm 자체 중복 검사항목 제거
        /// </summary>
        /// <param name="exm"></param>
        private void Remove_Overlap_ExamList2(ref List<GROUPCODE_EXAM_DISPLAY> exm)
        {
            List<string> tmp = new List<string>();

            for (int i = 0; i < exm.Count; i++)
            {
                tmp.Add(exm[i].EXCODE);
            }

            tmp = tmp.Distinct().ToList();

            List<GROUPCODE_EXAM_DISPLAY> rtnlst = new List<GROUPCODE_EXAM_DISPLAY>();
            
            for (int i = 0; i < tmp.Count; i++)
            {
                for (int j = 0; j < exm.Count; j++)
                {
                    if (tmp[i] == exm[j].EXCODE)
                    {
                        rtnlst.Add(exm[j]);
                        break;
                    }
                }
            }

            exm = rtnlst;
        }

        private void GroupCode_BuRate_Set(string argJong, ref READ_SUNAP_ITEM rSI, string argGubun = "")
        {
            //일특인경우 부담율 회사부담으로 강제 설정 : 02
            if (argGubun != "WORK")
            { 
                if (argJong == "11" && rSI.UCODE.To<string>("").Trim() != "") { rSI.GBSELF = "02"; }

                string strGrpCD = rSI.GRPCODE.To<string>("").Trim();
                string strGrpCDName = rSI.GRPNAME.To<string>("").Trim();

                if (strGrpCDName.Contains("수면"))
                {
                    rSI.GBSELF = "03";
                }

                //수면비용 강제 세팅
                switch (strGrpCD)
                {
                    case "3103":
                    case "3125":
                    case "3507":
                    case "3503":
                    case "9485":
                    case "3151": rSI.GBSELF = "03"; break;
                    default:
                        break;
                }

                if (strGrpCD == "2302" || strGrpCD == "J224") { rSI.GBSELF = "02"; }

                //신규채용자만 잠복결핵은 본인부담(이상규)
                if (strGrpCD == "K244" || strGrpCD == "K246") { rSI.GBSELF = "03"; }
            }

        }

        private void frmHcGroupCode_ssDblclick(List<string> argCode, string argGbn)
        {
            if (argCode.Count == 0) { return; }

            //저장된 검진정보에 ADD
            int nIdx = tbCtrl_JONG.SelectedTabIndex;

            List<string> lstgrpCode = new List<string>();

            List<READ_SUNAP_ITEM> vRSI = new List<READ_SUNAP_ITEM>();                   //기존 선택된 그룹코드 
            List<GROUPCODE_EXAM_DISPLAY> vDES = new List<GROUPCODE_EXAM_DISPLAY>();     //기존 선택된 검사항목 코드

            READ_SUNAP_ITEM_Get_Spread(vRSI, ssGroup);                                  //ssGroup Spread의 내용을 List로 값만 복사
            GROUPCODE_EXAM_DISPLAY_Get_Spread(vDES, ssExam);                            //ssExam Spread의 내용을 List로 값만 복사

            List<GROUPCODE_EXAM_DISPLAY> dupGED = new List<GROUPCODE_EXAM_DISPLAY>();   //그룹코드내 포함시키지 않을 검사항목

            //ssGroup.ActiveSheet.RowCount = 0;
            //ssExam.ActiveSheet.RowCount = 0;


            //묶음코드 재정렬
            //var vRSI = vRSI1.OrderBy(x => x.HANG).ThenBy(x => x.GRPCODE).ToList();
            //검사항목 재정렬(금액)
            //var vDES = vDES1.OrderBy(x => x.HANG).ThenBy(x => x.GROUPCODE).ThenBy(x => x.EXCODE).ToList();

            for (int j = 0; j < vRSI.Count; j++)
            {
                if (vRSI[j].RowStatus != RowStatus.Delete)
                {
                    if (!vRSI[j].GRPCODE.IsNullOrEmpty())
                    {
                        lstgrpCode.Add(vRSI[j].GRPCODE);        //기존 선택된 그룹코드 리스트
                    }
                }
            }

            for (int i = 0; i < argCode.Count; i++)
            {
                //선택한 코드 적용
                READ_SUNAP_ITEM item = readSunapItemService.GetItemByCode(argCode[i].Trim(), argGbn);   // 그룹코드 정보 개별 조회

                if (!item.IsNullOrEmpty())
                {
                    //중복선택건은 제외함 (그룹코드)
                    if (!vRSI.Find(x => x.GRPCODE == item.GRPCODE).IsNullOrEmpty())
                    {
                        continue;
                    }

                    //이미 선택되어진 그룹코드 내 검사항목은 중복되지 않게 List 로 담아둠
                    if (lstgrpCode.Count > 0)
                    {
                        dupGED = groupCodeExamDisplayService.GetListExcodeByGrpCodeList(lstgrpCode);    
                    }

                    item.GBSELF = "";

                    //암검진 경우 부담율 개별세팅
                    if (clsHcType.THC[nIdx].GJJONG == "31")
                    {
                        Cancer_GroupCode_BuRate_Set(nIdx, ref item);
                    }

                    //부담율 설정
                    GroupCode_BuRate_Set(clsHcType.THC[nIdx].GJJONG, ref item);

                    //선택 코드 금액 산정 
                    item.AMT = Read_GrpCode_Amt(vDES, item.GRPCODE.Trim(), dtpJepDate.Text, dupGED);

                    //다음 순번 그룹코드내 검사항목 중복제거를 위해 List 추가
                    lstgrpCode.Add(item.GRPCODE);

                    //작업중인 검진그룹코드 목록에 추가
                    vRSI.Add(item);

                }
            }

            string strBurate = VB.Pstr(cboBuRate.Text, ".", 1).To<string>(FstrBuRate[nIdx]);

            //Spread에 표시

            lstsuInfo(nIdx).Clear();
            lstsuInfo(nIdx).AddRange(vRSI);
            ssGroup.DataSource = null;
            ssGroup.SetDataSource(lstsuInfo(nIdx));

            lstgrpExam(nIdx).Clear();
            lstgrpExam(nIdx).AddRange(vDES);
            ssExam.DataSource = null;
            ssExam.SetDataSource(lstgrpExam(nIdx));

            //수납금액 계산하기
            Gesan_Sunap_Amt(nIdx, lstsuInfo(nIdx), strBurate, txtHalinAmt.Text.To<long>());

            SetControl_Special(nIdx, lstsuInfo(nIdx), lstgrpExam(nIdx));

        }

        /// <summary>
        /// 암검진 그룹코드 개별 부담율 적용
        /// </summary>
        /// <param name="nIdx"></param>
        /// <param name="item"></param>
        private void Cancer_GroupCode_BuRate_Set(int nIdx, ref READ_SUNAP_ITEM item, string strWork = "" )
        {
            if (strWork == "")
            {
                switch (item.GRPCODE.To<string>("").Trim())
                {
                    case "3101": item.GBSELF = ""; break;
                    case "1119": item.GBSELF = ""; break;
                    case "3111":        //위암
                        if (!clsHcType.THNV.hCan1.IsNullOrEmpty() && (clsHcType.THNV.hCan1.Contains("10%부담") || clsHcType.THNV.hCan1.Contains("본인부담없음")))
                        {
                            if (!clsHcType.THNV.hJaGubun.IsNullOrEmpty() && clsHcType.THNV.hJaGubun.To<string>("").Trim() == "의료급여")
                            {
                                item.GBSELF = "11";
                            }
                            else if(!clsHcType.THNV.hBogen.IsNullOrEmpty())
                            {
                                item.GBSELF = "12";
                            }
                            else
                            {
                                item.GBSELF = "09";
                            }
                        }
                        break;
                    case "3123":    //유방
                        if (!clsHcType.THNV.hCan2.IsNullOrEmpty() && (clsHcType.THNV.hCan2.Contains("10%부담") || clsHcType.THNV.hCan2.Contains("본인부담없음")))
                        {
                            if (!clsHcType.THNV.hJaGubun.IsNullOrEmpty() && clsHcType.THNV.hJaGubun.To<string>("").Trim() == "의료급여")
                            {
                                item.GBSELF = "11";
                            }
                            else if (!clsHcType.THNV.hBogen.IsNullOrEmpty())
                            {
                                item.GBSELF = "12";
                            }
                            else
                            {
                                item.GBSELF = "09";
                            }
                        }
                        break;
                    case "3116":    //분변
                        if (!clsHcType.THNV.hJaGubun.IsNullOrEmpty() && clsHcType.THNV.hJaGubun.To<string>("").Trim() == "의료급여")
                        {
                            item.GBSELF = "11";
                        }
                        else
                        {
                            item.GBSELF = "01";
                        }
                        break;
                    case "3115":
                        if (!clsHcType.THNV.hCan4.IsNullOrEmpty() && (clsHcType.THNV.hCan4.Contains("10%부담") || clsHcType.THNV.hCan4.Contains("본인부담없음")))
                        {
                            if (!clsHcType.THNV.hJaGubun.IsNullOrEmpty() && clsHcType.THNV.hJaGubun.To<string>("").Trim() == "의료급여")
                            {
                                item.GBSELF = "11";
                            }
                            else if (!clsHcType.THNV.hBogen.IsNullOrEmpty())
                            {
                                item.GBSELF = "12";
                            }
                            else
                            {
                                item.GBSELF = "09";
                            }
                        }
                        break;
                    case "3132":    //자궁
                        if (!clsHcType.THNV.hJaGubun.IsNullOrEmpty() && clsHcType.THNV.hJaGubun.To<string>("").Trim() == "의료급여")
                        {
                            item.GBSELF = "11";
                        }
                        else
                        {
                            item.GBSELF = "01";
                        }
                        break;
                    case "3169":
                        if (!clsHcType.THNV.hCan7.IsNullOrEmpty() && (clsHcType.THNV.hCan7.Contains("10%부담") || clsHcType.THNV.hCan7.Contains("본인부담없음")))
                        {
                            if (!clsHcType.THNV.hJaGubun.IsNullOrEmpty() && clsHcType.THNV.hJaGubun.To<string>("").Trim() == "의료급여")
                            {
                                item.GBSELF = "11";
                            }
                            else if(!clsHcType.THNV.hBogen.IsNullOrEmpty())
                            {
                                item.GBSELF = "12";
                            }
                            else
                            {
                                item.GBSELF = "09";
                            }
                        }
                        break;
                    default: break;
                }
            }
        }

        /// <summary>
        /// 컨트롤 자동 세팅을 위한 함수
        /// </summary>
        /// <param name="nIdx">검진종류 탭 </param>
        /// <param name="lstRSI">묶음코드 </param>
        /// <param name="lstGED">검사코드 (필요하면 사용) </param>
        private void SetControl_Special(int nIdx, List<READ_SUNAP_ITEM> lstRSI, List<GROUPCODE_EXAM_DISPLAY> lstGED)
        {
            string strJong = clsHcType.THC[nIdx].GJJONG;
            string strGbMCode = string.Empty;

            for (int i = 0; i < lstRSI.Count; i++)
            {
                if (lstRSI[i].RowStatus != RowStatus.Delete)
                {
                    if (!lstRSI[i].UCODE.IsNullOrEmpty()) { strGbMCode = "OK"; break; }
                }
            }

            //특검종류 세팅
            if (strJong == "11" && strGbMCode == "OK")
            {
                cboGbSpc.SelectedIndex = 1;     //일특
            }
            else if(strJong == "14" && strGbMCode == "OK")
            {
                cboGbSpc.SelectedIndex = 1;     //일특
            }
            else if (strJong == "23")
            {
                cboGbSpc.SelectedIndex = 2;     //특수
            }
            else if (strJong == "24")
            {
                cboGbSpc.SelectedIndex = 3;     //배치전
            }
            else if (strJong == "22")
            {
                cboGbSpc.SelectedIndex = 4;     //채용+배치전
            }
            else if (strJong == "25")
            {
                cboGbSpc.SelectedIndex = 5;     //수시
            }
            else if (strJong == "26")
            {
                cboGbSpc.SelectedIndex = 6;     //임시
            }
            else if (strJong == "21")
            {
                cboGbSpc.SelectedIndex = 7;     //채용
                
                if (!lstRSI.Find(x => x.RowStatus != RowStatus.Delete && x.GRPCODE.To<string>("").Trim() == "2116").IsNullOrEmpty() || 
                    !lstRSI.Find(x => x.RowStatus != RowStatus.Delete && x.GRPCODE.To<string>("").Trim() == "2113").IsNullOrEmpty() ||
                    !lstRSI.Find(x => x.RowStatus != RowStatus.Delete && x.GRPCODE.To<string>("").Trim() == "2121").IsNullOrEmpty() || 
                    !lstRSI.Find(x => x.RowStatus != RowStatus.Delete && x.GRPCODE.To<string>("").Trim() == "2122").IsNullOrEmpty())        //심리검사여부)
                {
                    cboGbSpc.SelectedIndex = 10;    //공무원채용
                }
                else if (!lstRSI.Find(x => x.GRPCODE.To<string>("").Trim() == "2119").IsNullOrEmpty())
                {
                    cboGbSpc.SelectedIndex = 16;    //소방공무원채용
                }
            }
            else if (strJong == "32")
            {
                cboGbSpc.SelectedIndex = 12;     //건강진단서
            }
        }

        /// <summary>
        /// 금액 재계산
        /// </summary>
        /// <param name="nIdx"></param>
        /// <param name="suInfo"></param>
        private void Gesan_Sunap_Amt(int nIdx, List<READ_SUNAP_ITEM> suInfo, string argBuRate, long argHalinAmt, string argHalinCode = null)
        {
            //검사항목 코드 재계산
            HIC_SUNAP item = hicSunapService.GetSumAmtByGroupCode(suInfo, argBuRate, argHalinAmt, clsHcType.THC[nIdx].GJJONG, argHalinCode);

            item.PANO       = clsHcType.THC[nIdx].PANO;
            item.WRTNO      = clsHcType.THC[nIdx].WRTNO;
            item.PTNO       = clsHcType.THC[nIdx].PTNO;
            item.JEPDATE    = dtpJepDate.Text;
            item.SNAME      = txtSName.Text;
            item.DEPTCODE   = "일반건진(HR)";
            item.HALINGYE   = VB.Left(cboHalinGye.Text, 2);

            clsHcType.THC[nIdx].UCODES = "";
            clsHcType.THC[nIdx].SEXAMS = "";

            //유해물질코드
            for (int i = 0; i < suInfo.Count; i++)
            {
                if (suInfo[i].RowStatus != RowStatus.Delete)
                {
                    if (!suInfo[i].UCODE.IsNullOrEmpty())
                    {
                        //물질코드 
                        clsHcType.THC[nIdx].UCODES += suInfo[i].UCODE.Trim() + ",";
                    }

                    if (suInfo[i].GBSELECT == "Y")
                    {   //선택검사
                        clsHcType.THC[nIdx].SEXAMS += suInfo[i].GRPCODE.Trim() + ",";
                    }
                    
                }
            }

            lblUCODES.Text = cHcMain.UCode_Names_Display(clsHcType.THC[nIdx].UCODES);
            if (lblUCODES.Text.Trim() != "")
            {
                panDockingYN(panJONG, panUCODES, true);
                panDockingYN(panJONG, panSchool, false);
            }

            //이전 수납정보
            HIC_SUNAP pSunap = hicSunapService.GetHicSunapAmtByWRTNO(clsHcType.THC[nIdx].WRTNO);

            Display_Sunap_Amt(item, pSunap);

            //검사참고사항
            switch (nIdx)
            {
                case 0: sunap0 = item;
                    clsHcType.THC[nIdx].REMARK = txtRemark1.Text.Trim();
                    break;
                case 1: sunap1 = item;
                    clsHcType.THC[nIdx].REMARK = txtRemark2.Text.Trim();
                    break;
                case 2: sunap2 = item;
                    clsHcType.THC[nIdx].REMARK = txtRemark3.Text.Trim();
                    break;
                case 3: sunap3 = item;
                    clsHcType.THC[nIdx].REMARK = txtRemark4.Text.Trim();
                    break;
                case 4: sunap4 = item;
                    clsHcType.THC[nIdx].REMARK = txtRemark5.Text.Trim();
                    break;
                default: break;
            }

            Clear_Spread_GroupCode("Group");
            ssGroup.DataSource = null;
            ssGroup.SetDataSource(suInfo);
        }

        /// <summary>
        /// 그룹코드 내 검사항목을 조회하고 중복검사를 제외한 검사항목들의 합계금액을 집계함.
        /// </summary>
        /// <param name="rtnGED">금액계산 대상 그룹코드 내 검사항목 정보를 받아올 변수 </param>
        /// <param name="argCode">대상 그룹코드</param>
        /// <param name="argJepDate">기준일자</param>
        /// <param name="dupGED">중복방지를 위해 이미 작성된 검사항목 List</param>
        /// <returns></returns>
        private long Read_GrpCode_Amt(List<GROUPCODE_EXAM_DISPLAY> rtnGED, string argCode, string argJepDate, List<GROUPCODE_EXAM_DISPLAY> dupGED)
        {
            long rtnAMT = 0;
            long nPrice = 0;
            bool bOK = false;   //Old 수가 적용여부
            string strGroupGbSuga = string.Empty;
            string strGbSuga = string.Empty;
            int nAmtNo = 0;
            string strGubun = "";

            List<string> lstExtExCode = new List<string>(); //그룹코드 정보 조회시 중복을 방지 할 검사항목 코드들
            //List<string> lstExtExCode1 = new List<string>(); //그룹코드 정보 조회시 중복을 방지 할 검사항목 코드들

            if (!dupGED.IsNullOrEmpty() && dupGED.Count > 0)
            {
                for (int i = 0; i < dupGED.Count; i++)
                {
                    lstExtExCode.Add(dupGED[i].EXCODE);
                    //lstExtExCode1.Add(dupGED[i].EXCODE);
                }
            }

            //그룹코드 금액 집계 (종검은 그룹코드에 지정된 금액을 사용함)
            List<HIC_GROUPEXAM_GROUPCODE_EXCODE> lst = hicGroupexamGroupcodeExcodeService.GetListByCode(argCode, lstExtExCode);
            //List<HIC_GROUPEXAM_GROUPCODE_EXCODE> lst1 = hicGroupexamGroupcodeExcodeService.GetListByCodeIn(argCode, lstExtExCode1);

            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    //strGubun = ""; nPrice = 0;
                    //if (lst1.Count >0)
                    //{
                    //    for (int j = 0; j < lst1.Count; j++)
                    //    {
                    //        if(lst[i].EXCODE == lst1[j].EXCODE)
                    //        {
                    //            strGubun = "True";
                    //            break;
                    //        }
                    //    }   
                    //}

                    strGroupGbSuga = lst[i].GBSUGA.To<string>("").Trim();      //그룹
                    strGbSuga = lst[i].SUGAGBN.To<string>("").Trim();          //검사항목
                    //묶음코드에 수가적용구분이 없으면 그룹코드의 구분으로 적용함.
                    if (strGbSuga == "") { strGbSuga = strGroupGbSuga; }

                    // Amt1 = 보험수가의 80%
                    // Amt2 = 보험수가의 100%
                    // Amt3 = 보험수가의 125%
                    // Amt4 = 일반+특검 차액
                    // Amt5 = 임의수가
                    nAmtNo = strGbSuga.To<int>();

                    //전년도 건진사업이면 Old수가를 적용함
                    bOK = false;
                    if (string.Compare(cboYear.Text, VB.Left(DateTime.Now.ToShortDateString(), 4)) < 0)
                    {
                        if (string.Compare(lst[i].SUDATE, VB.Left(DateTime.Now.ToShortDateString(), 4) + "-01-01") >= 0) { bOK = true; }
                        //전년도 자료를 수정한 경우는 제외
                        if (VB.Left(argJepDate, 4) == cboYear.Text) { bOK = false; }
                    }

                    //if (strGubun.IsNullOrEmpty() || rtnGED.Count == 0)
                    //{
                        if (bOK)
                        {
                            nPrice = lst[i].GetPropertieValue("OLDAMT" + VB.Format(nAmtNo, "0")).To<long>();
                        }
                        else
                        {
                            if (string.Compare(argJepDate, lst[i].SUDATE) >= 0)
                            {
                                nPrice = lst[i].GetPropertieValue("AMT" + VB.Format(nAmtNo, "0")).To<long>();
                            }
                            else
                            {
                                nPrice = lst[i].GetPropertieValue("OLDAMT" + VB.Format(nAmtNo, "0")).To<long>();
                            }
                        }
                    //}
                    
                    GROUPCODE_EXAM_DISPLAY item = new GROUPCODE_EXAM_DISPLAY
                    {
                        GROUPCODE = lst[i].GROUPCODE,
                        GROUPCODENAME = lst[i].GROUPNAME,
                        EXCODE = lst[i].EXCODE,
                        AMT = nPrice,
                        EXNAME = lst[i].HNAME
                        //GUBUN = strGubun
                    };

                    rtnGED.Add(item);   //조회된 검사항목 정보를 INSERT

                    rtnAMT = rtnAMT + nPrice;
                }
            }

            return rtnAMT;
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
            }
        }

        private void ePost_value_HPAT(HIC_PATIENT item)
        {
            Hpatient = item;
        }

        /// <summary>
        /// 도로명 주소 검색창 연동
        /// </summary>
        private void Post_Code_Help( string strGubun)
        {
            clsHcVariable.GstrValue = "";
            clsPublic.GstrMsgList = "";

            frmSearchRoadWeb frm = new frmSearchRoadWeb();
            frm.rSetGstrValue += new frmSearchRoadWeb.SetGstrValue(ePost_value);
            frm.ShowDialog();

            if (strGubun.IsNullOrEmpty())
            { 
            //
            //    clsHcVariable.GstrValue = "";
            //    clsPublic.GstrMsgList = "";

            //    frmSearchRoadWeb frm = new frmSearchRoadWeb();
            //    frm.rSetGstrValue += new frmSearchRoadWeb.SetGstrValue(ePost_value);
            //    frm.ShowDialog();

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

            else 
            {
                int i = Convert.ToInt32(strGubun);

                if (!clsHcVariable.GstrValue.IsNullOrEmpty())
                {
                    TextBox txtMail = (Controls.Find("txtMail" + i.ToString(), true)[0] as TextBox);
                    txtMail.Text = VB.Left(VB.Pstr(clsHcVariable.GstrValue, "|", 1), 3);
                    txtMail.Text += VB.Mid(VB.Pstr(clsHcVariable.GstrValue, "|", 1), 4, 2);

                    TextBox txtJuso1 = (Controls.Find("txtJuso1" + i.ToString(), true)[0] as TextBox);
                    txtJuso1.Text = VB.Pstr(clsHcVariable.GstrValue, "|", 2).Trim();
                    TextBox txtJuso2 = (Controls.Find("txtJuso2" + i.ToString(), true)[0] as TextBox);
                    txtJuso2.Text = "";

                    FstrBuildNo = VB.Pstr(clsHcVariable.GstrValue, "|", 5).Trim();

                }
                else
                {
                    FstrBuildNo = "";
                    txtJuso2.Focus();
                }
            }
        }

        private void ePost_value(string GstrValue)
        {
            clsHcVariable.GstrValue = GstrValue;
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void ePost_value_CODE(string strCode, string strName)
        {
            FstrComCode = strCode;
            FstrComName = strName;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            Screen_Clear();

            panSub01.AddRequiredControl(txtJumin1);
            panSub01.AddRequiredControl(txtJumin2);
            panSub01.AddRequiredControl(txtSName);
            panSub01.AddRequiredControl(txtPtno);
            
            panBuRate.AddRequiredControl(cboBuRate);

            panPAT.AddRequiredControl(txtMail);
            panPAT.AddRequiredControl(txtJuso1);
            panPAT.AddRequiredControl(txtHphone);

            lstBlnfo = hicBcodeService.GetCodebyGubun("HIC_채혈안내문유해인자");
            lstUrinfo = hicBcodeService.GetCodebyGubun("HIC_특수소변안내문유해인자");
            lstHyang = hicBcodeService.GetCodebyGubun("HIC_내시경향정전송");
            lstJindan = hicBcodeService.GetCodebyGubun("HIC_진단서구분등록");

            //2021-03-03(전자동의서 TEST)
            themTabForm(frmHcPermission, this.panPerm);
            //themTabForm(frmHcEmrPermission, this.panPerm);
            

            string strWaitPcNo = string.Empty;

            FileInfo nFILE = null;

            string strFIleNm = @"c:\HIC_WAIT.ini";
            nFILE = new FileInfo(strFIleNm);

            if (nFILE.Exists == false) { return; }

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

            lblCall.Text = "0";

            timer1.Start();
        }

        private void Screen_Clear()
        {
            panMain.Initialize();
            panSub01.Initialize();
            panAMT.Initialize();
            panLTD.Initialize();
            panPAT.Initialize();
            panGoto.Initialize();
            panJONG.Initialize();
            panJepList.Initialize();
            panBottom.Initialize();

            rdoMemo1.Checked = true;
            if(FstrChulPC != "Y")
            {
                chkChul.Checked = false;
            }
            
            ssETC.ActiveSheet.ClearRange(0, 0, ssETC.ActiveSheet.Rows.Count, ssETC.ActiveSheet.ColumnCount, true);
            ssETC.ActiveSheet.Rows.Count = 0;
            
            cboYear.SelectedIndex = 0;
            cboMuRyoAm.SelectedIndex = 0;
            panSub01.Enabled = true;

            lblCall.Text = "0";

            lblHGigan.Text = "";
            lblUCODES.Text = "";
            lblMsg.Text = "";
            lblMsg.BackColor = Color.White;

            rdoSTS1A.Enabled = true;
            rdoSTS2A.Enabled = true;
            rdoSTS3A.Enabled = true;
            rdoSTS4A.Enabled = true;
            rdoSTS5A.Enabled = true;

            rdoRES12.Checked = true;
            rdoRES22.Checked = true;
            rdoRES32.Checked = true;
            rdoRES42.Checked = true;
            rdoRES52.Checked = true;

            tbJongPage1.Visible = true;
            tbJongPage1.Text = "";
            tbJongPage2.Visible = false;
            tbJongPage3.Visible = false;
            tbJongPage4.Visible = false;
            tbJongPage5.Visible = false;

            tbJongPage99.Visible = true;

            tbExamPage1.Visible = true;
            tbExamPage1.Text = "";
            tbExamPage2.Visible = false;
            tbExamPage3.Visible = false;
            tbExamPage4.Visible = false;
            tbExamPage5.Visible = false;

            cboJONG1.Enabled = true;
            cboJONG2.Enabled = true;
            cboJONG3.Enabled = true;
            cboJONG4.Enabled = true;
            cboJONG5.Enabled = true;

            btnJongClose01.Enabled = true;
            btnJongClose02.Enabled = true;
            btnJongClose03.Enabled = true;
            btnJongClose04.Enabled = true;
            btnJongClose05.Enabled = true;

            lblBMI.Text = "";
            lblPrvAgr.Text = "";
            lblInfoAgr.Text = "";
            lblIEMunjin.Text = "";
            lblIEMunjin.BackColor = Color.White;
            lblHeaJep.Text = "";
            lblHeaJep.BackColor = Color.White;

            panDockingYN(panJONG, panSchool, false);
            panDockingYN(panJONG, panUCODES, false);

            //Array Varient Clear
            Array.Clear(clsHcType.THC, 0, clsHcType.THC.Length - 1);
            Array.Clear(clsHcType.HSI, 0, clsHcType.HSI.Length - 1);
            clsHcType.THNV_CLEAR();
            clsPmpaType.RSD.CardSeqNo = 0;



            for (int i = 0; i < FstrBuRate.Length; i++) { FstrBuRate[i] = "00"; }
            for (int i = 0; i < FstrExamName.Length; i++) { FstrExamName[i] = ""; }
            for (int i = 0; i < FstrName.Length; i++) { FstrName[i] = ""; }

            if (!suInfo0.IsNullOrEmpty()) { suInfo0.Clear(); }
            if (!suInfo1.IsNullOrEmpty()) { suInfo1.Clear(); }
            if (!suInfo2.IsNullOrEmpty()) { suInfo2.Clear(); }
            if (!suInfo3.IsNullOrEmpty()) { suInfo3.Clear(); }
            if (!suInfo4.IsNullOrEmpty()) { suInfo4.Clear(); }

            if (!oldSuInfo0.IsNullOrEmpty()) { oldSuInfo0.Clear(); }
            if (!oldSuInfo1.IsNullOrEmpty()) { oldSuInfo1.Clear(); }
            if (!oldSuInfo2.IsNullOrEmpty()) { oldSuInfo2.Clear(); }
            if (!oldSuInfo3.IsNullOrEmpty()) { oldSuInfo3.Clear(); }
            if (!oldSuInfo4.IsNullOrEmpty()) { oldSuInfo4.Clear(); }

            if (!grpExam0.IsNullOrEmpty()) { grpExam0.Clear(); }
            if (!grpExam1.IsNullOrEmpty()) { grpExam1.Clear(); }
            if (!grpExam2.IsNullOrEmpty()) { grpExam2.Clear(); }
            if (!grpExam3.IsNullOrEmpty()) { grpExam3.Clear(); }
            if (!grpExam4.IsNullOrEmpty()) { grpExam4.Clear(); }

            if (!sunap0.IsNullOrEmpty()) { sunap0 = null; }
            if (!sunap1.IsNullOrEmpty()) { sunap1 = null; }
            if (!sunap2.IsNullOrEmpty()) { sunap2 = null; }
            if (!sunap3.IsNullOrEmpty()) { sunap3 = null; }
            if (!sunap4.IsNullOrEmpty()) { sunap4 = null; }

            FsHisPtNo = "";
            FstrUCodes = "";
            FstrSExams = "";
            FstrBuildNo = "";
            FstrPtno = "";
            FnPano = 0;
            FnIEMunNo = 0;
            FbHuilGasan = false;
            FnWRTNO = 0;
            GnWRTNO = 0;

            tbCtrl_JONG.Enabled = false;
            tbCtrl_Exam.Enabled = false;
            panSelExam.Enabled = false;
            lblCurDate.Text = DateTime.Now.ToShortDateString();

            clsHcType.THC[0].GBUSE = "";

            Clear_Spread_GroupCode();
            Clear_Working_Variants();

            panAMT.Initialize();

            Screen_Clear_Nhic();

            ssCan.ActiveSheet.ColumnHeader.Cells[0, 0, 0, ssCan.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(166, 186, 214);
            ssCan.ActiveSheet.ClearRange(0, 0, ssCan.ActiveSheet.Rows.Count, ssCan.ActiveSheet.ColumnCount, true);
            ssCan.ActiveSheet.Rows.Count = 5;

            frmHcPermission.Screen_Clear();
            //frmHcEmrPermission.Screen_Clear();

            txtLtdRemark.Text = "";
            txtLtdRemark1.Text = "";


            txtJumin1.Focus();

        }

        private void Clear_Working_Variants()
        {
            if (!suInfo0.IsNullOrEmpty()) { suInfo0.Clear(); }
            else { suInfo0 = new List<READ_SUNAP_ITEM>(); }

            if (!suInfo1.IsNullOrEmpty()) { suInfo1.Clear(); }
            else { suInfo1 = new List<READ_SUNAP_ITEM>(); }

            if (!suInfo2.IsNullOrEmpty()) { suInfo2.Clear(); }
            else { suInfo2 = new List<READ_SUNAP_ITEM>(); }

            if (!suInfo3.IsNullOrEmpty()) { suInfo3.Clear(); }
            else { suInfo3 = new List<READ_SUNAP_ITEM>(); }

            if (!suInfo4.IsNullOrEmpty()) { suInfo4.Clear(); }
            else { suInfo4 = new List<READ_SUNAP_ITEM>(); }

            if (!oldSuInfo0.IsNullOrEmpty()) { oldSuInfo0.Clear(); }
            else { oldSuInfo0 = new List<READ_SUNAP_ITEM>(); }

            if (!oldSuInfo1.IsNullOrEmpty()) { oldSuInfo1.Clear(); }
            else { oldSuInfo1 = new List<READ_SUNAP_ITEM>(); }

            if (!oldSuInfo2.IsNullOrEmpty()) { oldSuInfo2.Clear(); }
            else { oldSuInfo2 = new List<READ_SUNAP_ITEM>(); }

            if (!oldSuInfo3.IsNullOrEmpty()) { oldSuInfo3.Clear(); }
            else { oldSuInfo3 = new List<READ_SUNAP_ITEM>(); }

            if (!oldSuInfo4.IsNullOrEmpty()) { oldSuInfo4.Clear(); }
            else { oldSuInfo4 = new List<READ_SUNAP_ITEM>(); }

            if (!grpExam0.IsNullOrEmpty()) { grpExam0.Clear(); }
            else { grpExam0 = new List<GROUPCODE_EXAM_DISPLAY>(); }

            if (!grpExam1.IsNullOrEmpty()) { grpExam1.Clear(); }
            else { grpExam1 = new List<GROUPCODE_EXAM_DISPLAY>(); }

            if (!grpExam2.IsNullOrEmpty()) { grpExam2.Clear(); }
            else { grpExam2 = new List<GROUPCODE_EXAM_DISPLAY>(); }

            if (!grpExam3.IsNullOrEmpty()) { grpExam3.Clear(); }
            else { grpExam3 = new List<GROUPCODE_EXAM_DISPLAY>(); }

            if (!grpExam4.IsNullOrEmpty()) { grpExam4.Clear(); }
            else { grpExam4 = new List<GROUPCODE_EXAM_DISPLAY>(); }

            if (!sunap0.IsNullOrEmpty()) { sunap0 = null; }
            else { sunap0 = new HIC_SUNAP(); }

            if (!sunap1.IsNullOrEmpty()) { sunap1 = null; }
            else { sunap1 = new HIC_SUNAP(); }

            if (!sunap2.IsNullOrEmpty()) { sunap2 = null; }
            else { sunap2 = new HIC_SUNAP(); }

            if (!sunap3.IsNullOrEmpty()) { sunap3 = null; }
            else { sunap3 = new HIC_SUNAP(); }

            if (!sunap4.IsNullOrEmpty()) { sunap4 = null; }
            else { sunap4 = new HIC_SUNAP(); }
        }

        private void Clear_Working_Variants(int nIdx)
        {
            if (nIdx == 0)
            {
                if (!suInfo0.IsNullOrEmpty()) { suInfo0.Clear(); }
                else { suInfo0 = new List<READ_SUNAP_ITEM>(); }

                if (!oldSuInfo0.IsNullOrEmpty()) { oldSuInfo0.Clear(); }
                else { oldSuInfo0 = new List<READ_SUNAP_ITEM>(); }

                if (!grpExam0.IsNullOrEmpty()) { grpExam0.Clear(); }
                else { grpExam0 = new List<GROUPCODE_EXAM_DISPLAY>(); }

                if (!sunap0.IsNullOrEmpty()) { sunap0 = null; }
                else { sunap0 = new HIC_SUNAP(); }

            }
            else if (nIdx == 1)
            {
                if (!suInfo1.IsNullOrEmpty()) { suInfo1.Clear(); }
                else { suInfo1 = new List<READ_SUNAP_ITEM>(); }

                if (!oldSuInfo1.IsNullOrEmpty()) { oldSuInfo1.Clear(); }
                else { oldSuInfo1 = new List<READ_SUNAP_ITEM>(); }

                if (!grpExam1.IsNullOrEmpty()) { grpExam1.Clear(); }
                else { grpExam2 = new List<GROUPCODE_EXAM_DISPLAY>(); }

                if (!sunap1.IsNullOrEmpty()) { sunap1 = null; }
                else { sunap1 = new HIC_SUNAP(); }
            }
            else if (nIdx == 2)
            {
                if (!suInfo2.IsNullOrEmpty()) { suInfo2.Clear(); }
                else { suInfo2 = new List<READ_SUNAP_ITEM>(); }

                if (!oldSuInfo2.IsNullOrEmpty()) { oldSuInfo2.Clear(); }
                else { oldSuInfo2 = new List<READ_SUNAP_ITEM>(); }

                if (!grpExam2.IsNullOrEmpty()) { grpExam2.Clear(); }
                else { grpExam2 = new List<GROUPCODE_EXAM_DISPLAY>(); }

                if (!sunap2.IsNullOrEmpty()) { sunap2 = null; }
                else { sunap2 = new HIC_SUNAP(); }
            }
            else if (nIdx == 3)
            {
                if (!suInfo3.IsNullOrEmpty()) { suInfo3.Clear(); }
                else { suInfo3 = new List<READ_SUNAP_ITEM>(); }

                if (!oldSuInfo3.IsNullOrEmpty()) { oldSuInfo3.Clear(); }
                else { oldSuInfo3 = new List<READ_SUNAP_ITEM>(); }

                if (!grpExam3.IsNullOrEmpty()) { grpExam3.Clear(); }
                else { grpExam3 = new List<GROUPCODE_EXAM_DISPLAY>(); }

                if (!sunap3.IsNullOrEmpty()) { sunap3 = null; }
                else { sunap3 = new HIC_SUNAP(); }
            }
            else if (nIdx == 4)
            {
                if (!suInfo4.IsNullOrEmpty()) { suInfo4.Clear(); }
                else { suInfo4 = new List<READ_SUNAP_ITEM>(); }

                if (!oldSuInfo4.IsNullOrEmpty()) { oldSuInfo4.Clear(); }
                else { oldSuInfo4 = new List<READ_SUNAP_ITEM>(); }

                if (!grpExam4.IsNullOrEmpty()) { grpExam4.Clear(); }
                else { grpExam4 = new List<GROUPCODE_EXAM_DISPLAY>(); }

                if (!sunap4.IsNullOrEmpty()) { sunap4 = null; }
                else { sunap4 = new HIC_SUNAP(); }
            }
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

                SS_Sunap.DataSource = null;
                SS_Sunap.ActiveSheet.ClearRange(0, 0, SS_Sunap.ActiveSheet.Rows.Count, SS_Sunap.ActiveSheet.ColumnCount, true);
                SS_Sunap.ActiveSheet.Rows.Count = 0;
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

        private void panDockingYN(Panel Ppan, Panel Cpan, bool bVis)
        {
            if (bVis)
            {
                //Cpan.Parent = Ppan;
                Cpan.Dock = DockStyle.Fill;
            }
            else
            {
                //Cpan.Parent = null;
                Cpan.Dock = DockStyle.None;
            }

            Cpan.Visible = bVis;
        }

        /// <summary>
        /// HIC_접수증세팅
        /// <seealso cref="HIC_접수증세팅"/>
        /// </summary>
        private void HicJepsuPaperSet(List<READ_SUNAP_ITEM> list, string strGubun, string strGbPrint)
        {
            //string strXRay1 = "", strXRay2 = "";

            FstrEndo = "일반";
            FstrHyang = "";

            for (int i = 0; i < FstrExamName.Length; i++)
            {
                if (i != 9) //자궁경부는 수동출력
                {
                    FstrExamName[i] = "";
                }
            }

            for (int i = 0; i < FstrName.Length; i++)
            {
                FstrName[i] = "";
            }

            if (strGbPrint.IsNullOrEmpty())
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].RowStatus != RowStatus.Delete)
                    {
                        HIC_GROUPCODE item = hicGroupcodeService.GetItemByCode(list[i].GRPCODE);

                        if (!item.IsNullOrEmpty())
                        {
                            if (!item.GBAM.IsNullOrEmpty())
                            {
                                for (int j = 1; j < VB.L(item.GBAM.Trim(), ","); j++)
                                {
                                    if (VB.Pstr(item.GBAM.Trim(), ",", j) == "1")
                                    {
                                        switch (j)
                                        {
                                            case 1: chkAm1.Checked = true; break;
                                            case 2: chkAm2.Checked = true; break;
                                            case 3: chkAm3.Checked = true; break;
                                            case 4: chkAm4.Checked = true; break;
                                            case 5: chkAm5.Checked = true; break;
                                            case 6: chkAm6.Checked = true; break;
                                            case 7: chkAm7.Checked = true; break;
                                            default: break;
                                        }

                                    }
                                }
                            }

                            if (!item.GBPRINT.IsNullOrEmpty())
                            {
                                for (int j = 1; j < VB.L(item.GBPRINT.Trim(), ","); j++)
                                {
                                    if (VB.Pstr(item.GBPRINT.To<string>(""), ",", j) == "1")
                                    {
                                        switch (j)
                                        {
                                            case 1:
                                                chkPrt1.Checked = true;
                                                FstrName[j - 1] = chkPrt1.Text + "^^";
                                                break;
                                            case 2:
                                                chkPrt2.Checked = true;
                                                FstrName[j - 1] = chkPrt2.Text + "^^";
                                                break;
                                            case 3:
                                                chkPrt3.Checked = true;
                                                FstrName[j - 1] = chkPrt3.Text + "^^";
                                                break;
                                            case 4:
                                                chkPrt4.Checked = true;
                                                FstrName[j - 1] = chkPrt4.Text + "^^";
                                                break;
                                            case 5:
                                                chkPrt5.Checked = true;
                                                FstrName[j - 1] = chkPrt5.Text + "^^";
                                                break;
                                            case 6:
                                                chkPrt6.Checked = true;
                                                FstrName[j - 1] = chkPrt6.Text + "^^";
                                                break;
                                            case 7:
                                                chkPrt7.Checked = true;
                                                FstrName[j - 1] = chkPrt7.Text + "^^";
                                                break;
                                            case 8:
                                                chkPrt8.Checked = true;
                                                FstrName[j - 1] = chkPrt8.Text + "^^";
                                                break;
                                            case 9:
                                                chkPrt9.Checked = true;
                                                FstrName[j - 1] = chkPrt9.Text + "^^";
                                                break;
                                            case 10:
                                                chkPrt10.Checked = true;
                                                FstrName[j - 1] = chkPrt10.Text + "^^";
                                                break;
                                            case 11:
                                                chkPrt11.Checked = true;
                                                FstrName[j - 1] = chkPrt11.Text + "^^";
                                                break;
                                            case 12:
                                                chkPrt12.Checked = true;
                                                FstrName[j - 1] = chkPrt12.Text + "^^";
                                                break;
                                            default: break;
                                        }

                                        if (!item.EXAMNAME.IsNullOrEmpty())
                                        {
                                            //자궁경부암은 필요시 출력함
                                            if (j != 9)
                                            {
                                                FstrExamName[j - 1] = FstrExamName[j - 1] + item.EXAMNAME + "^^";
                                            }
                                            else if (j == 9 && strGubun == "재출력")
                                            {
                                                FstrExamName[j - 1] = FstrExamName[j - 1] + item.EXAMNAME + "^^";
                                            }
                                        }
                                        else
                                        {
                                            FstrExamName[j - 1] = FstrExamName[j - 1] + item.NAME + "^^";
                                        }
                                    }
                                }
                            }

                            if (FstrEndo == "일반")
                            {
                                List<HIC_GROUPEXAM_EXCODE> hGE = hicGroupexamExcodeService.GetEndoGubunbyGroupCode(item.CODE);

                                if (hGE.Count > 0)
                                {
                                    for (int j = 0; j < hGE.Count; j++)
                                    {
                                        if (hGE[j].ENDOSCOPE == "Y") { FstrEndo = "수면"; }
                                        if (hGE[j].ENDOGUBUN3 == "Y") { FstrHyang = "Y"; }
                                    }
                                }
                            }

                        }
                    }   //End RowStatus
                }   //End For
            }
            else
            //수동출력
            {
                for (int j = 1; j < VB.L(strGbPrint, ","); j++)
                {
                    if (VB.Pstr(strGbPrint, ",", j) == "1")
                    {
                        switch (j)
                        {
                            case 1:
                                FstrName[j - 1] = chkPrt1.Text + "^^";
                                FstrExamName[j - 1] = "흉부X-RAY^^";
                                break;
                            case 2:
                                FstrName[j - 1] = chkPrt2.Text + "^^";
                                FstrExamName[j - 1] = "골반초음파^^";
                                break;
                            case 3:
                                FstrName[j - 1] = chkPrt3.Text + "^^";
                                FstrExamName[j - 1] = "위내시경^^";
                                break;
                            case 4:
                                FstrName[j - 1] = chkPrt4.Text + "^^";
                                FstrExamName[j - 1] = "정신과상담^^";
                                break;
                            case 5:
                                FstrName[j - 1] = chkPrt5.Text + "^^";
                                FstrExamName[j - 1] = "";
                                break;
                            case 6:
                                FstrName[j - 1] = chkPrt6.Text + "^^";
                                FstrExamName[j - 1] = "골밀도검사^^";
                                break;
                            case 7:
                                FstrName[j - 1] = chkPrt7.Text + "^^";
                                FstrExamName[j - 1] = "간초음파^^";
                                break;
                            case 8:
                                FstrName[j - 1] = chkPrt8.Text + "^^";
                                FstrExamName[j - 1] = "위장조영촬영^^";
                                break;
                            case 9:
                                FstrName[j - 1] = chkPrt9.Text + "^^";
                                FstrExamName[j - 1] = "자궁경부암^^";
                                break;
                            case 10:
                                FstrName[j - 1] = chkPrt10.Text + "^^";
                                FstrExamName[j - 1] = "심장초음파^^";
                                break;
                            case 11:
                                FstrName[j - 1] = chkPrt11.Text + "^^";
                                FstrExamName[j - 1] = "뇌혈류초음파^^";
                                break;
                            case 12:
                                FstrName[j - 1] = chkPrt12.Text + "^^";
                                FstrExamName[j - 1] = "";
                                break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys key = keyData & ~(Keys.Shift | Keys.Control);

            switch (key)
            {
                case Keys.F1: //챠트인계등록 실행
                    FrmHcCharttrans_Insert = new frmHcCharttrans_Insert();
                    FrmHcCharttrans_Insert.StartPosition = FormStartPosition.CenterScreen;
                    FrmHcCharttrans_Insert.ShowDialog();
                    cHF.fn_ClearMemory(FrmHcCharttrans_Insert);
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}
