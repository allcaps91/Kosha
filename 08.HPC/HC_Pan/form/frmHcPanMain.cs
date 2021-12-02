using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Service;
using System;
using System.IO;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanMain.cs
/// Description     : 통합판정
/// Author          : 이상훈
/// Create Date     : 2020-09-16
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "MDIForm1.frm(HcPan00)" />

namespace HC_Pan
{
    public partial class frmHcPanMain : Form, MainFormMessage
    {
        string mPara1 = "";

        public delegate void SetPanExamResultReg(object sender, EventArgs e);
        public static event SetPanExamResultReg rSetPanExamResultReg;

        HeaJepsuResultService heaJepsuResultService = null;
        ComHpcLibBService comHpcLibBService = null;
        HeaSangdamWaitService heaSangdamWaitService = null;
        HicBcodeService hicBcodeService = null;
        HeaResultService heaResultService = null;
        HicXrayResultService hicXrayResultService = null;
        HeaResvExamService heaResvExamService = null;
        HeaJepsuService heaJepsuService = null;
        BasPcconfigService basPcconfigService = null;
        HicSpcPanjengService hicSpcPanjengService = null;
        HicSpcPanhisService hicSpcPanhisService = null;

        frmHcPanjeng FrmHcPanjeng = null;
        frmHcSpcSCode FrmHcSpcSCode = null;
        frmHcPanJepsuPanjengCheckList FrmHcPanJepsuPanjengCheckList = null;
        frmHcPanErSecondList FrmHcPanErSecondList = null;
        frmHcPanExamResultRegChg FrmHcPanExamResultRegChg = null;
        frmHcPanMunjin_2019 FrmHcPanMunjin_2019 = null;
        frmHcPanTeethJudgment FrmHcPanTeethJudgment = null;
        frmHcPanSPcExamMunjin FrmHcPanSPcExamMunjin = null;
        frmHcLifeTools FrmHcLifeTools = null;
        frmHcNightMunjin FrmHcNightMunjin = null;
        frmHcPanJindanSet FrmHcPanJindanSet = null;
        frmHcPanCompanySecondListPrint FrmHcPanCompanySecondListPrint = null;
        frmHcPanVivisectionsampleMgmt FrmHcPanVivisectionsampleMgmt = null;
        frmHcPanDrNoChk FrmHcPanDrNoChk = null;
        frmHcPanSpcGList FrmHcPanSpcGList = null;
        frmMirErrorList FrmMirErrorList = null;
        frmHcPanPersonResult FrmHcPanPersonResult = null;
        frmHcPanGenMedExamResult_New FrmHcPanGenMedExamResult_New = null;
        frmHcPanSpcDiagnosisResultReport FrmHcPanSpcDiagnosisResultReport = null;
        frmHcPanOpinionAfterMgmtGen FrmHcPanOpinionAfterMgmtGen = null;
        frmHcPanOpinionAfterMgmtSpc FrmHcPanOpinionAfterMgmtSpc = null;
        frmHcPanOpinionAfterMgmtXray FrmHcPanOpinionAfterMgmtXray = null;
        frmHcPanOpinionAfterMgmtGenSpc FrmHcPanOpinionAfterMgmtGenSpc = null;
        frmHcJochiList FrmHcJochiList = null;
        frmHcPendList FrmHcPendList = null;
        frmHcPanOpinionAfterCounselMgmt FrmHcPanOpinionAfterCounselMgmt = null;
        frmHcPanDrSpcExamJudgmentCnt FrmHcPanDrSpcExamJudgmentCnt = null;
        frmHcPanYearlungcapacityExamCnt FrmHcPanYearlungcapacityExamCnt = null;
        frmHcPanHealthDiagSurvey FrmHcPanHealthDiagSurvey = null;
        frmHcPanCerebralCardiovascularPan FrmHcPanCerebralCardiovascularPan = null;
        frmHaExamResultReg FrmHaExamResultReg = null;
        frmHaExamResultReg_New FrmHaExamResultReg_New = null;
        frmHcPanPatList FrmHcPanPatList = null;

        //암판정관련 업무
        frmHcPanMain_Can FrmHcPanMain_Can = null;
        frmHcAmResvSet_New FrmHcAmResvSet_New = null;
        frmHcAmResvDetail FrmHcAmResvDetail = null;
        frmHcAmCancerBreadkPromise FrmHcAmCancerBreadkPromise = null;
        frmHcAmResponse FrmHcAmResponse = null;
        frmHcAmHcAfterOpd FrmHcAmHcAfterOpd = null;
        frmHcAmOpinionModify FrmHcAmOpinionModify = null;
        frmHcAmReserve FrmHcAmReserve = null;

        //학생검진 판정 업무
        frmHcSchoolExamPanjeng FrmHcSchoolExamPanjeng = null;
        frmHcSchoolTeethPrint FrmHcSchoolTeethPrint = null;
        frmHcSchoolResultMunjinPrint FrmHcSchoolResultMunjinPrint = null;
        frmHcSchoolExamResult FrmHcSchoolExamResult = null;
        frmHcSchoolStatic FrmHcSchoolStatic = null;
        frmHcSchoolStaticB FrmHcSchoolStaticB = null;
        frmHcSchoolStudentStatic FrmHcSchoolStudentStatic = null;
        frmHcSchoolStudentPhysicalDevStatic FrmHcSchoolStudentPhysicalDevStatic = null;
        frmHcSchoolOpinionAfterMgmtPrint FrmHcSchoolOpinionAfterMgmtPrint = null;
        frmHcSchoolChargeExpenses_New FrmHcSchoolChargeExpenses_New = null;
        frmHcSchoolChargeExpenses FrmHcSchoolChargeExpenses = null;
        frmHcSchoolJepsuViewPrint FrmHcSchoolJepsuViewPrint = null;
        //frmHcSFile FrmHcSFile = null; // 학생검진 결과 파일생성
        frmHcSchoolCommonDistrictRegView FrmHcSchoolCommonDistrictRegView = null;
        frmHcSchoolBmiCal FrmHcSchoolBmiCal = null;

        ComFunc cF = null;
        clsHcMain hm = null;
        clsHaBase hb = null;
        clsHcAct ha = null;
        clsHcVariable hv = null;
        clsHcFunc cHF = null;
        
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

        public frmHcPanMain()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcPanMain(MainFormMessage pform)
        {
            InitializeComponent();
            mCallForm = pform;
            SetEvent();
            SetControl();
        }

        public frmHcPanMain(MainFormMessage pform, string sPara1)
        {
            InitializeComponent();
            mCallForm = pform;
            mPara1 = sPara1;
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            heaJepsuResultService = new HeaJepsuResultService();
            comHpcLibBService = new ComHpcLibBService();
            heaSangdamWaitService = new HeaSangdamWaitService();
            hicBcodeService = new HicBcodeService();
            heaResultService = new HeaResultService();
            hicXrayResultService = new HicXrayResultService();
            heaResvExamService = new HeaResvExamService();
            heaJepsuService = new HeaJepsuService();
            basPcconfigService = new BasPcconfigService();
            hicSpcPanjengService = new HicSpcPanjengService();
            hicSpcPanhisService = new HicSpcPanhisService();

            this.Load += new EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
            this.FormClosing += new FormClosingEventHandler(eFormClosing);
            this.menuExit.Click += new EventHandler(eMenuClick);
            this.Menu01.Click += new EventHandler(eMenuClick);      ////판정작업=========================================
            this.Menu01_01.Click += new EventHandler(eMenuClick);   //통합판정
            this.Menu01_02.Click += new EventHandler(eMenuClick);   //접수자판정점검표
            this.Menu01_03.Click += new EventHandler(eMenuClick);   //응급2차검진대상자
            this.Menu01_04.Click += new EventHandler(eMenuClick);   //통합일반특수판정
            this.Menu01_05.Click += new EventHandler(eMenuClick);   //뇌심혈관계판정
            this.Menu01_06.Click += new EventHandler(eMenuClick);   //특수판정 소견코드 등록
            this.Menu02.Click += new EventHandler(eMenuClick);      ////문진및결과들록===================================
            this.Menu02_01.Click += new EventHandler(eMenuClick);   //1.검사결과등록
            this.Menu02_02.Click += new EventHandler(eMenuClick);   //2.건강검진 공통문진표 등록
            this.Menu02_03.Click += new EventHandler(eMenuClick);   //3.구강검진 문진표 등록
            this.Menu02_04.Click += new EventHandler(eMenuClick);   //4.특수검진 문진표 등록
            this.Menu02_05.Click += new EventHandler(eMenuClick);   //5.야간작업 문진표 등록
            this.Menu02_06.Click += new EventHandler(eMenuClick);   //6.생활습관도구표 등록
            this.Menu02_07.Click += new EventHandler(eMenuClick);   //진단서종류등록
            this.Menu02_08.Click += new EventHandler(eMenuClick);   //방사선 검사 일괄등록
            this.Menu02_09.Click += new EventHandler(eMenuClick);   //회사별 2차검진대상자 인쇄
            this.Menu02_10.Click += new EventHandler(eMenuClick);   //생체시료(혈액,소변) 관리
            this.Menu02_11.Click += new EventHandler(eMenuClick);   //1차판정의사 누락명단
            this.Menu02_12.Click += new EventHandler(eMenuClick);   //특수공단전송 대상자 확인
            this.Menu02_13.Click += new EventHandler(eMenuClick);   //공단청구 오류내역 수정의뢰
            this.Menu02_14.Click += new EventHandler(eMenuClick);   //EKG EMR스캔 의뢰명단 작성
            this.Menu03.Click += new EventHandler(eMenuClick);      ////자료조회=========================================
            this.Menu03_01.Click += new EventHandler(eMenuClick);   //개인별 검진 history
            this.Menu03_02.Click += new EventHandler(eMenuClick);   //일반건강진단결과표
            this.Menu03_03.Click += new EventHandler(eMenuClick);   //특수건강진단결과표
            this.Menu03_04.Click += new EventHandler(eMenuClick);   //질병 유소견자 사후관리(일반)
            this.Menu03_05.Click += new EventHandler(eMenuClick);   //질병 유소견자 사후관리(특수)
            this.Menu03_06.Click += new EventHandler(eMenuClick);   //질병 유소견자 사후관리(방사선)
            this.Menu03_07.Click += new EventHandler(eMenuClick);   //질병 유소견자 사후관리(대행)
            //this.Menu03_08.Click += new EventHandler(eMenuClick);   //직업성질환주의 추적검사   = 사용무
            this.Menu03_09.Click += new EventHandler(eMenuClick);   //조치대상자 조회(일반)
            this.Menu03_10.Click += new EventHandler(eMenuClick);   //조치대상자 조회(학생)
            this.Menu03_11.Click += new EventHandler(eMenuClick);   //보류대장 조회
            this.Menu03_12.Click += new EventHandler(eMenuClick);   //유소견자(D1,D2,DN) 사후관리 상담대장
            this.Menu03_13.Click += new EventHandler(eMenuClick);   //의사별 특수검진 판정 인원수
            this.Menu03_14.Click += new EventHandler(eMenuClick);   //생활습관도구표 기초코드 작업
            this.Menu03_15.Click += new EventHandler(eMenuClick);   //전년도 2차 미수검자 U판정 전환
            this.Menu03_16.Click += new EventHandler(eMenuClick);   //연도별 폐활량검사 대상자수 및 폐기능 이상자수 현황         
            this.Menu04.Click += new EventHandler(eMenuClick);      ////사전조사표 등록==================================
            this.Menu05.Click += new EventHandler(eMenuClick);      //암판정 관련 조회=================================== 
            this.Menu05_01.Click += new EventHandler(eMenuClick);   //암검진 스케줄 등록
            this.Menu05_02.Click += new EventHandler(eMenuClick);   //암검진 상세 리스트
            this.Menu05_03.Click += new EventHandler(eMenuClick);   //연계 파일 생성(SAM)
            this.Menu05_04.Click += new EventHandler(eMenuClick);   //암검진 부도 점검
            this.Menu05_05.Click += new EventHandler(eMenuClick);   //암검진 여부 조회
            this.Menu05_06.Click += new EventHandler(eMenuClick);   //암검진 후 외래 조회
            this.Menu05_07.Click += new EventHandler(eMenuClick);   //암판정 소견문 일괄 점검 및 수정
            this.Menu05_08.Click += new EventHandler(eMenuClick);   //암판정 등록
            this.Menu05_09.Click += new EventHandler(eMenuClick);   //암검진 예약등록
            //학생검진 판정 업무
            this.Menu06.Click += new EventHandler(eMenuClick);      //학생검진 판정 업무=================================
            this.Menu06_00.Click += new EventHandler(eMenuClick);   //1. 구강문진 및 결과 인쇄
            this.Menu06_01.Click += new EventHandler(eMenuClick);   //1. 구강문진 및 결과 인쇄
            this.Menu06_02.Click += new EventHandler(eMenuClick);   //2. 학생문진 및 결과 인쇄
            this.Menu06_03.Click += new EventHandler(eMenuClick);   //2-1. 학생문진 및 결과 인쇄(통합)
            this.Menu06_04.Click += new EventHandler(eMenuClick);   //3. 학생검진 통계표(A)
            this.Menu06_05.Click += new EventHandler(eMenuClick);   //3-1.학생검진 통계표(B)
            this.Menu06_06.Click += new EventHandler(eMenuClick);   //4. 학생검진 통계표(성별)
            this.Menu06_07.Click += new EventHandler(eMenuClick);   //5. 학생신체발달상황 통계표
            this.Menu06_08.Click += new EventHandler(eMenuClick);   //6. 학생 사후관리 인쇄
            this.Menu06_09.Click += new EventHandler(eMenuClick);   //7. 학생검진 비용청구서
            this.Menu06_10.Click += new EventHandler(eMenuClick);   //7-1. 학생검진 비용청구서(구)
            this.Menu06_11.Click += new EventHandler(eMenuClick);   //8. 접수자 조회
            this.Menu06_12.Click += new EventHandler(eMenuClick);   //파일생성(&F)
            this.Menu06_13.Click += new EventHandler(eMenuClick);   //상용구 등록
            this.Menu06_14.Click += new EventHandler(eMenuClick);   //비만도

            this.Menu07_01.Click += new EventHandler(eMenuClick);   //종검 검사결과 등록
            this.Menu07_02.Click += new EventHandler(eMenuClick);   //종검 판정결과 등록
            this.Menu07_03.Click += new EventHandler(eMenuClick);   //종검 Cytology
            this.Menu07_04.Click += new EventHandler(eMenuClick);   //종검 EKG 판독결과 등록
            this.Menu07_05.Click += new EventHandler(eMenuClick);   //동의서 작성

            frmHcPanjeng.rSetPanExamResultReg += new frmHcPanjeng.SetPanExamResultReg(ePanExamResultReg);
            //frmHcPanjeng.rSetPanPatList += new frmHcPanjeng.SetPanPatList(ePanPatList);
        }

        private void eFormClosing(object sender, FormClosingEventArgs e)
        {
            frmHcPanjeng.rSetPanExamResultReg -= new frmHcPanjeng.SetPanExamResultReg(ePanExamResultReg);
        }

        private void ePanExamResultReg(long argWRTNO)
        {

            Form FrmHcPanExamResultRegChg = cHF.OpenForm_Check_Return("frmHcPanExamResultRegChg");

            if (FrmHcPanExamResultRegChg != null)
            {
                FrmHcPanExamResultRegChg.Close();
                FrmHcPanExamResultRegChg.Dispose();
                FrmHcPanExamResultRegChg = null;
            }

            FrmHcPanExamResultRegChg = new frmHcPanExamResultRegChg(argWRTNO, "Y", "Y");
            FrmHcPanExamResultRegChg.Show();
        }

        void SetControl()
        {
            FrmHcPanjeng = new frmHcPanjeng();
            FrmHaExamResultReg_New = new frmHaExamResultReg_New();
            FrmHcPanMain_Can = new frmHcPanMain_Can();

            cF = new ComFunc();
            hm = new clsHcMain();
            hb = new clsHaBase();
            ha = new clsHcAct();
            hv = new clsHcVariable();
            cHF = new clsHcFunc();
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

        void eFormLoad(object sender, EventArgs e)
        {
            string strTemp = "";

            ComFunc.ReadSysDate(clsDB.DbCon);
            clsCompuInfo.SetComputerInfo();

            Menu04.Enabled = false; //사전조사표

            hb.READ_HIC_Doctor(clsType.User.IdNumber.To<long>());   //판정의사 여부를 읽음
            clsQuery.READ_PC_CONFIG(clsDB.DbCon);                   //PC에 설정된 값을 READ
            //SignImage_Download    = > 해당 화면에서 직접 다운로드 하도록 변경 
            cHF.SET_자료사전_VALUE();                               //자료사전의 값을 공용변수에 로딩
            //hb.GET_Monitor_Setting();                               //모니터 해상도를 읽음

            if (clsType.User.IdNumber == "23515")
            {
                Menu02_05.Enabled = false;  //야간작업 문진표 등록
            }

            //원격판정PC는 문진뷰어 프로그램 자동실행 안됨
            if (clsPublic.GstrIpAddress == "192.168.2.157" || clsPublic.GstrIpAddress == "192.168.2.35")
            {
                clsHcVariable.GstrMunjin = "NO";
            }

            //menuU판정일괄전환.Enabled = False
            //If GbHicAdminSabun = True And Right(GstrSysDate, 5) >= "02-01" And Right(GstrSysDate, 5) < "03-01" Then
            //    menuU판정일괄전환.Enabled = True
            //End If

            //특수검진담당자만 사전조사표 사용기능함
            if (hicBcodeService.GetCountbyGubunCode("HIC_특수검진담당자", clsType.User.IdNumber) > 0)
            {
                Menu04.Enabled = true;  //사전조사표
            }

            if (hicBcodeService.GetCountbyGubunCodeName("ETC_향정코드변경", "USE") > 0)
            {
                clsHcVariable.GstrUSE = "OK";
            }

            //Call Kill("c:\ECG_*.ecg")
            string strDir = @"c:\";

            DirectoryInfo Dir = new DirectoryInfo(strDir);

            if (Dir.Exists == false)
            {
                FileInfo[] File = Dir.GetFiles("ECG_*.ecg", SearchOption.AllDirectories);

                foreach (FileInfo file in File)
                {
                    file.Delete();
                }
            }

        }

        void eMenuClick(object sender, EventArgs e)
        {
            if (sender == Menu01)  //판정작업
            {                
            }
            else if (sender == Menu01_01)  //통합판정
            {
                Form rtnFrm = cHF.OpenForm_Check_Return("frmHcPanjeng");

                if (rtnFrm != null)
                {
                    FormVisiable(FrmHcPanjeng);
                    return;
                }

                FrmHcPanjeng = new frmHcPanjeng();
                themTabForm(FrmHcPanjeng, this.panMain);
                FormVisiable(FrmHcPanjeng);         
            }
            else if (sender == Menu01_02)  //접수자판정점검표
            {
                if (cHF.OpenForm_Check("frmHcPanJepsuPanjengCheckList") == true)
                {
                    return;
                }
                FrmHcPanJepsuPanjengCheckList = new frmHcPanJepsuPanjengCheckList();
                FrmHcPanJepsuPanjengCheckList.StartPosition = FormStartPosition.CenterParent;
                FrmHcPanJepsuPanjengCheckList.ShowDialog(this);
            }
            else if (sender == Menu01_03)  //응급2차검진대상자
            {
                FrmHcPanErSecondList = new frmHcPanErSecondList();
                FrmHcPanErSecondList.StartPosition = FormStartPosition.CenterParent;
                FrmHcPanErSecondList.ShowDialog(this);
            }
            //else if (sender == Menu01_04)  //통합일반특수판정
            //{

            //}
            else if (sender == Menu01_05)  //뇌심혈관계판정
            {
                FrmHcPanCerebralCardiovascularPan = new frmHcPanCerebralCardiovascularPan();
                FrmHcPanCerebralCardiovascularPan.StartPosition = FormStartPosition.CenterParent;
                FrmHcPanCerebralCardiovascularPan.ShowDialog(this);
            }
            else if (sender == Menu01_06)   //특수판정 소견코드 등록
            {
                FrmHcSpcSCode = new frmHcSpcSCode();
                FrmHcSpcSCode.StartPosition = FormStartPosition.CenterScreen;
                FrmHcSpcSCode.ShowDialog(this);
            }
            else if (sender == Menu02)     //문진 및 결과등록
            {
            }
            else if (sender == Menu02_01)  //1.검사결과등록
            {
                FrmHcPanExamResultRegChg = new frmHcPanExamResultRegChg();
                FrmHcPanExamResultRegChg.StartPosition = FormStartPosition.CenterParent;

                FrmHcPanExamResultRegChg.ShowDialog(this);
            }
            else if (sender == Menu02_02)  //2.건강검진 공통문진표 등록
            {
                FrmHcPanMunjin_2019 = new frmHcPanMunjin_2019();
                FrmHcPanMunjin_2019.ShowDialog(this);
            }
            else if (sender == Menu02_03)  //3.구강검진 문진표 등록
            {
                FrmHcPanTeethJudgment = new frmHcPanTeethJudgment(0);
                FrmHcPanTeethJudgment.ShowDialog(this);
            }
            else if (sender == Menu02_04)  //4.특수검진 문진표 등록
            {
                FrmHcPanSPcExamMunjin = new frmHcPanSPcExamMunjin(0);
                FrmHcPanSPcExamMunjin.ShowDialog(this);
            }
            else if (sender == Menu02_05)  //5.야간작업 문진표 등록
            {
                FrmHcNightMunjin = new frmHcNightMunjin();
                FrmHcNightMunjin.ShowDialog(this);
            }
            else if (sender == Menu02_06)  //6.생활습관도구표 등록
            {
                FrmHcLifeTools = new frmHcLifeTools();
                FrmHcLifeTools.ShowDialog(this);
            }
            else if (sender == Menu02_07)  //진단서종류등록
            {
                FrmHcPanJindanSet = new frmHcPanJindanSet();
                FrmHcPanJindanSet.ShowDialog(this);
            }
            else if (sender == Menu02_08)  //방사선 검사 일괄등록
            {
                //사용무
            }
            else if (sender == Menu02_09)  //회사별 2차검진대상자 인쇄
            {
                FrmHcPanCompanySecondListPrint = new frmHcPanCompanySecondListPrint();
                FrmHcPanCompanySecondListPrint.ShowDialog(this);
            }
            else if (sender == Menu02_10)  //생체시료(혈액,소변) 관리
            {
                FrmHcPanVivisectionsampleMgmt = new frmHcPanVivisectionsampleMgmt();
                FrmHcPanVivisectionsampleMgmt.ShowDialog(this);
            }
            else if (sender == Menu02_11)  //1차판정의사 누락명단
            {
                FrmHcPanDrNoChk = new frmHcPanDrNoChk();
                FrmHcPanDrNoChk.ShowDialog(this);
            }
            else if (sender == Menu02_12)  //특수공단전송 대상자 확인
            {
                FrmHcPanSpcGList = new frmHcPanSpcGList();
                FrmHcPanSpcGList.ShowDialog(this);
            }
            else if (sender == Menu02_13)  //공단청구 오류내역 수정의뢰
            {
                ///TODO : 이상훈 (2020.09.19) - frmMirErrorList ComHpcLibB 으로 이동 완료 후 적용
                //frmMirErrorList
                FrmMirErrorList = new frmMirErrorList();
                FrmMirErrorList.ShowDialog(this);
            }
            else if (sender == Menu02_14)  //EKG EMR스캔 의뢰명단 작성
            {
                ///TODO : 이상훈 (2020.09.19) - 김민철 개발 완료 후 적용
                //FrmEmr스캔명단작성
            }
            else if (sender == Menu03)  //조회업무
            {
            }
            else if (sender == Menu03_01)  //개인별 검진 history
            {
                FrmHcPanPersonResult = new frmHcPanPersonResult("frmHcPanMain","", "");
                FrmHcPanPersonResult.ShowDialog(this);
            }
            else if (sender == Menu03_02)  //일반건강진단결과표
            {
                FrmHcPanGenMedExamResult_New = new frmHcPanGenMedExamResult_New();
                FrmHcPanGenMedExamResult_New.ShowDialog(this);
            }
            else if (sender == Menu03_03)  //특수건강진단결과표
            {
                FrmHcPanSpcDiagnosisResultReport = new frmHcPanSpcDiagnosisResultReport();
                FrmHcPanSpcDiagnosisResultReport.ShowDialog(this);
            }
            else if (sender == Menu03_04)  //질병 유소견자 사후관리(일반)
            {
                FrmHcPanOpinionAfterMgmtGen = new frmHcPanOpinionAfterMgmtGen();
                FrmHcPanOpinionAfterMgmtGen.ShowDialog();
            }
            else if (sender == Menu03_05)  //질병 유소견자 사후관리(특수)
            {
                FrmHcPanOpinionAfterMgmtSpc = new frmHcPanOpinionAfterMgmtSpc();
                FrmHcPanOpinionAfterMgmtSpc.ShowDialog(this);
            }
            else if (sender == Menu03_06)  //질병 유소견자 사후관리(방사선)
            {
                FrmHcPanOpinionAfterMgmtXray = new frmHcPanOpinionAfterMgmtXray();
                FrmHcPanOpinionAfterMgmtXray.ShowDialog(this);
            }
            else if (sender == Menu03_07)  //질병 유소견자 사후관리(대행)
            {
                FrmHcPanOpinionAfterMgmtGenSpc = new frmHcPanOpinionAfterMgmtGenSpc();
                FrmHcPanOpinionAfterMgmtGenSpc.ShowDialog(this);
            }
            //else if (sender == Menu03_08)  //직업성질환주의 추적검사
            //{
            //    //사용무
            //}
            else if (sender == Menu03_09)  //조치대상자 조회(일반)
            {
                FrmHcJochiList = new frmHcJochiList();
                FrmHcJochiList.ShowDialog(this);
            }
            else if (sender == Menu03_10)  //조치대상자 조회(학생)
            {
                FrmHcJochiList = new frmHcJochiList();
                FrmHcJochiList.ShowDialog(this);
            }
            else if (sender == Menu03_11)  //보류대장 조회
            {
                FrmHcPendList = new frmHcPendList();
                FrmHcPendList.ShowDialog(this);
            }
            else if (sender == Menu03_12)  //유소견자(D1,D2,DN) 사후관리 상담대장
            {
                FrmHcPanOpinionAfterCounselMgmt = new frmHcPanOpinionAfterCounselMgmt();
                FrmHcPanOpinionAfterCounselMgmt.ShowDialog(this);
            }
            else if (sender == Menu03_13)  //의사별 특수검진 판정 인원수
            {
                FrmHcPanDrSpcExamJudgmentCnt = new frmHcPanDrSpcExamJudgmentCnt();
                FrmHcPanDrSpcExamJudgmentCnt.ShowDialog(this);
            }
            else if (sender == Menu03_14)  //생활습관도구표 기초코드 작업
            {
                ///TODO : 이상훈(2020.09.19) - 사용무(대체 화면 확인 필요)
                //FrmTCode
            }
            else if (sender == Menu03_15)  //전년도 2차 미수검자 U판정 전환
            {
                int result = 0;
                string strFrDate = "";
                string strToDate = "";

                ComFunc.ReadSysDate(clsDB.DbCon);

                if (string.Compare(VB.Right(clsPublic.GstrSysDate, 5), "02-01") < 0 || string.Compare(VB.Right(clsPublic.GstrSysDate, 5), "03-01") > 0)
                {
                    MessageBox.Show("2차 미수검자 U판정 일괄변환은 2월달만 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show("전년도 2차 미수검자를 U판정으로 일괄 전환을 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                strFrDate = (VB.Left(clsPublic.GstrSysDate, 4).To<int>() - 1).To<string>() + "-01-01";
                strToDate = (VB.Left(clsPublic.GstrSysDate, 4).To<int>() - 1).To<string>() + "-12-31";

                result = hicSpcPanjengService.UpdatePanjengSogenRemarkbyJepDate(strFrDate, strToDate);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("HIC_SPC_PANJENG Update 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                result = hicSpcPanhisService.DeletebyJepDate(strFrDate, strToDate);

                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("HIC_SPC_PANJENG 삭제 중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);

                MessageBox.Show("작업 완료!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (sender == Menu03_16)  //연도별 폐활량검사 대상자수 및 폐기능 이상자수 현황  
            {
                FrmHcPanYearlungcapacityExamCnt = new frmHcPanYearlungcapacityExamCnt();
                FrmHcPanYearlungcapacityExamCnt.ShowDialog(this);
            }
            else if (sender == Menu04)  //사전조사표
            {
                FrmHcPanHealthDiagSurvey = new frmHcPanHealthDiagSurvey();
                FrmHcPanHealthDiagSurvey.ShowDialog(this);
            }
            else if (sender == Menu05_01)  //암검진 스케줄 등록
            {
                FrmHcAmResvSet_New = new frmHcAmResvSet_New();
                FrmHcAmResvSet_New.ShowDialog(this);
            }
            else if (sender == Menu05_02)  //암검진 상세 리스트
            {
                FrmHcAmResvDetail = new frmHcAmResvDetail();
                FrmHcAmResvDetail.ShowDialog(this);
            }
            else if (sender == Menu05_03)  //연계 파일 생성(SAM)
            {
                ///TODO : 이상훈 (2020.09.24) - FrmSamFile2(김민철) 개발 완료 후 적용 ==> 사용무 메뉴제외
                //FrmSamFile2
            }
            else if (sender == Menu05_04)  //암검진 부도 점검
            {
                FrmHcAmCancerBreadkPromise = new frmHcAmCancerBreadkPromise();
                FrmHcAmCancerBreadkPromise.ShowDialog(this);
            }
            else if (sender == Menu05_05)  //암검진 여부 조회
            {
                FrmHcAmResponse = new frmHcAmResponse();
                FrmHcAmResponse.ShowDialog(this);
            }
            else if (sender == Menu05_06)  //암검진 후 외래 조회
            {
                FrmHcAmHcAfterOpd = new frmHcAmHcAfterOpd();
                FrmHcAmHcAfterOpd.ShowDialog(this);
            }
            else if (sender == Menu05_07)  //암판정 소견문 일괄 점검 및 수정
            {
                FrmHcAmOpinionModify = new frmHcAmOpinionModify();
                FrmHcAmOpinionModify.ShowDialog(this);
            }
            else if (sender == Menu05_08)   //암판정결과등록
            {
                Form rtnFrm = cHF.OpenForm_Check_Return("frmHcPanMain_Can");

                if (rtnFrm != null)
                {
                    FormVisiable(FrmHcPanMain_Can);
                    return;
                }

                FrmHcPanMain_Can = new frmHcPanMain_Can();
                themTabForm(FrmHcPanMain_Can, this.panMain);
                FormVisiable(FrmHcPanMain_Can);
            }
            else if (sender == Menu05_09)   //암검진예약등록
            {
                FrmHcAmReserve = new frmHcAmReserve();
                FrmHcAmReserve.StartPosition = FormStartPosition.CenterParent;
                FrmHcAmReserve.ShowDialog(this);
            }
            else if (sender == Menu06_00)  //학생검진판정
            {
                Form rtnFrm = cHF.OpenForm_Check_Return("frmHcSchoolExamPanjeng");

                if (rtnFrm != null)
                {
                    FormVisiable(FrmHcSchoolExamPanjeng);
                    return;
                }

                FrmHcSchoolExamPanjeng = new frmHcSchoolExamPanjeng();
                themTabForm(FrmHcSchoolExamPanjeng, this.panMain);
                FormVisiable(FrmHcSchoolExamPanjeng);
            }
            else if (sender == Menu06_01)  //1. 구강문진 및 결과 인쇄
            {
                FrmHcSchoolTeethPrint = new frmHcSchoolTeethPrint();
                FrmHcSchoolTeethPrint.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolTeethPrint.ShowDialog(this);
            }
            else if (sender == Menu06_02)  //2. 학생문진 및 결과 인쇄
            {
                FrmHcSchoolResultMunjinPrint = new frmHcSchoolResultMunjinPrint();
                FrmHcSchoolResultMunjinPrint.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolResultMunjinPrint.ShowDialog(this);
            }
            else if (sender == Menu06_03)  //2-1. 학생문진 및 결과 인쇄(통합)
            {
                FrmHcSchoolExamResult = new frmHcSchoolExamResult();
                FrmHcSchoolExamResult.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolExamResult.ShowDialog(this);
            }
            else if (sender == Menu06_04)  //3. 학생검진 통계표(A)
            {
                FrmHcSchoolStatic = new frmHcSchoolStatic();
                FrmHcSchoolStatic.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolStatic.ShowDialog(this);
            }
            else if (sender == Menu06_05)  //3-1.학생검진 통계표(B)
            {
                FrmHcSchoolStaticB = new frmHcSchoolStaticB();
                FrmHcSchoolStaticB.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolStaticB.ShowDialog(this);
            }
            else if (sender == Menu06_06)  //4. 학생검진 통계표(성별)
            {
                FrmHcSchoolStudentStatic = new frmHcSchoolStudentStatic();
                FrmHcSchoolStudentStatic.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolStudentStatic.ShowDialog(this);
            }
            else if (sender == Menu06_07)  //5. 학생신체발달상황 통계표
            {
                FrmHcSchoolStudentPhysicalDevStatic = new frmHcSchoolStudentPhysicalDevStatic();
                FrmHcSchoolStudentPhysicalDevStatic.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolStudentPhysicalDevStatic.ShowDialog(this);
            }
            else if (sender == Menu06_08)  //6. 학생 사후관리 인쇄
            {
                FrmHcSchoolOpinionAfterMgmtPrint = new frmHcSchoolOpinionAfterMgmtPrint();
                FrmHcSchoolOpinionAfterMgmtPrint.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolOpinionAfterMgmtPrint.ShowDialog(this);
            }
            else if (sender == Menu06_09)  //7. 학생검진 비용청구서
            {
                FrmHcSchoolChargeExpenses_New = new frmHcSchoolChargeExpenses_New();
                FrmHcSchoolChargeExpenses_New.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolChargeExpenses_New.ShowDialog(this);
            }
            else if (sender == Menu06_10)  //7-1. 학생검진 비용청구서(구)
            {
                FrmHcSchoolChargeExpenses = new frmHcSchoolChargeExpenses();
                FrmHcSchoolChargeExpenses.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolChargeExpenses.ShowDialog(this);
            }
            else if (sender == Menu06_11)  //8. 접수자 조회
            {
                FrmHcSchoolJepsuViewPrint = new frmHcSchoolJepsuViewPrint();
                FrmHcSchoolJepsuViewPrint.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolJepsuViewPrint.ShowDialog(this);
            }
            else if (sender == Menu06_12)  //파일생성(&F)
            {
                ///TODO : 이상훈 (2020.09.21) - 미개발
                //FrmHcSFile = new frmHcSFile();
                //FrmHcSFile.StartPosition = FormStartPosition.CenterParent;
                //FrmHcSFile.ShowDialog(this);
            }
            else if (sender == Menu06_13)  //상용구 등록
            {
                FrmHcSchoolCommonDistrictRegView = new frmHcSchoolCommonDistrictRegView();
                FrmHcSchoolCommonDistrictRegView.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolCommonDistrictRegView.ShowDialog(this);
            }
            else if (sender == Menu06_14)  //비만도
            {
                FrmHcSchoolBmiCal = new frmHcSchoolBmiCal();
                FrmHcSchoolBmiCal.StartPosition = FormStartPosition.CenterParent;
                FrmHcSchoolBmiCal.ShowDialog(this);
            }
            else if (sender == Menu07_01)   //종검결과등록
            {
                FrmHaExamResultReg = new frmHaExamResultReg();
                FrmHaExamResultReg.StartPosition = FormStartPosition.CenterScreen;
                FrmHaExamResultReg.ShowDialog();
            }
            else if (sender == Menu07_02)   //종검판정결과등록
            {
                if (FormIsExist(FrmHaExamResultReg_New) == true)
                {
                    FormVisiable(FrmHaExamResultReg_New);
                }
                else
                {
                    if (FrmHaExamResultReg_New == null)
                    {
                        FrmHaExamResultReg_New = new frmHaExamResultReg_New();
                    }
                    themTabForm(FrmHaExamResultReg_New, this.panMain);
                    FormVisiable(FrmHaExamResultReg_New);
                }
            }
            else if (sender == Menu07_03)   //종검Cytology
            {
                frmHaCytology frm = new frmHaCytology();
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog();
            }
            else if (sender == Menu07_04)   //종검EKG결과등록
            {
                frmHaResultEKG frm = new frmHaResultEKG();
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog();
            }
            else if (sender == Menu07_05)   //동의서 작성
            {
                frmHcEmrConset_Rec frm = new frmHcEmrConset_Rec(0, "DOCTOR");
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.Show();
            }
            else if (sender == menuExit)    //종료
            {
                this.Close();
                return;
            }
        }

        void FormVisiable(Form frm)
        {
            frm.Visible = false;
            frm.Visible = true;
            frm.BringToFront();
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
    }
}
