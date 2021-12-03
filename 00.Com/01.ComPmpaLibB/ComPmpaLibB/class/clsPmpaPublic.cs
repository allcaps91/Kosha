using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
/// <summary>
/// Description : Global 변수 모음
/// Author : 박병규, 김민철
/// Create Date : 2017.05.25
/// </summary>
/// <history>
/// </history>
namespace ComPmpaLibB
{
    public class clsPmpaPb
    {
        public static string GstrLockPtno = ""; //GstrLockPano


        public static string GstrPmpaMaster = "30761"; //원무수녀
        public static string GstrPmpaManager = "18631"; //원무작업 수퍼매니저(이채현책임)

        public string[] ArgV = null;        //쿼리에 사용될 변수 배열값   V     
        public string[] ArgY = null;        //쿼리에 사용될 변수 배열값   Y     
        public static string GstrSysDate = DateTime.Now.ToString("yyyy-MM-dd");

        #region //고시법 개정과 관련된 날짜 변수
        public static string GstrFoodReDate     = "2015-10-01";     //Gstr식대개편일자
        public static string GstrGlobalErFdate  = "2017-07-21";     //Gstr권역응급의료시작일
        public static string GstrScErFdate      = "2016-01-01";     //Gstr중증응급환자시작일
        public static string GstrLngRtDate      = "2016-07-01";     //Gstr장기입원부담율시작일
        public static string GstrDrgStartDay    = "2013-07-01";     //DRG 시작일자
        public static string GstrMedSupDay      = "2015-09-01";     //Gstr의료질평가시작일
        public static string GstrNPRateDay      = "2017-03-13";     //Gstr정신과정률시작일
        #endregion

        public static string[] GstrSETNus = new string[66];     // OVIEWA - FrmOpdSlip에서 쓰이는 변수
        public static string GstrRetValue;                      // OVIEWA - FrmOpdSlip에서 쓰이는 변수, 찾기 Help화면의 Return Value값
        public static string GstrRetName;                       // OVIEWA - FrmOpdSlip에서 쓰이는 변수, 찾기 Help화면의 Return Value값

        public static long GnIpdNo = 0;
        public static long GnTrsNo = 0;
        public static string GstrTIM = string.Empty;
        public static string GstrKTASLev = string.Empty;        //응급중증도 레벨
        public static long GnAntiTubeDrug_Amt = 0;              //Gn항결핵약제비   
        public static long GnHRoomAmt = 0;                 //2인실 병실료
        public static long GnHRoomBonin = 0;               //2인실 병실료 본인
        public static long GnH1RoomAmt = 0;              //1인실 병실료 본인
        public static long GnHUSetAmt = 0;                 //호스피스 병실료
        public static long GnHUSetBonin = 0;              //호스피스 병실료 본인
        public static string GstrIOSend_Test = "";        //IORDER SEND 테스트용 변수 ( "OK" 이면 IPD_NEW_SLIP_TEST에 생성됨)

        #region //SugaRead(vbSugaRead_new.bas)
        public static string GstrSugaNewUseFlag = "";   //수가사용여부
        public static string GstrSugaNewReadOK = "";    //수가flag

        public static string GstrDrug_Amt_UseFlag = ""; //저가약제 사용여부
        public static string GstrF_UseFlag = "";        //newF항 사용여부

        public static long GnIAmt = 0;              //일반수가 => GniAmt_new
        public static long GnBAmt = 0;              //보험수가 => GnbAmt_new
        public static long GnTAmt = 0;              //산재수가 => GntAmt_new
        public static long GnSAmt = 0;              //약제상한차액 => GnsAmt_new
        public static long GnSelAmt = 0;            //선택진료수가 => GnSelAmt_new

        public static Double GnDrug_Amt_Sum = 0;        //저가약제합산금액 2010-11-22
        public static Double GnDrug_Amt_One = 0;        //저가약제낱개금액 2010-11-22


        //예약검사 관련
        //public static string Gstr예약검사Flag = "";           //예약검사 사용여부(아래로 변경)
        public static string GstrResExamFlag = "";
        //public static string Gstr예약검사CHK = "";            //예약검사 체크 Y(아래로 변경)
        public static string GstrResExamCHK = "";
        //public static long Gn예약검사이전WRTNO = 0;           //예약검사 이전 WRTNO(아래로 변경)
        public static long GnResExamWRTNO = 0;
        public static long GnResExamEnd = 0;                    //예약검사 환불시 0원환불 처리될경우사용

        //public static long[] Gn예약검사Amt  = new long[7];    //예약검사 금액(아래로 변경)
        public static long[] GnResExamAmt = new long[7];
        //public static string Gstr예약검사Remark = "";         //예약검사참고사항(아래로 변경)
        public static string GstrResExamRemark = "";
        //public static string Gstr예약검사Temp = "";           //예약검사변수(아래로 변경)
        public static string GstrResExamPtno = "";
        //public static string GstrNew영수증Flag = "";          //신규영수증 사용여부(아래로 변경)
        public static string GstrNewReceiptFlag = "";
        //public static string Gstr예약검사_KK020 = "";         //예약수납시 체크 -(아래로 변경)
        public static string GstrResExam_KK020 = "";
        //public static string Gstr입원수속전체과 = "";         //입원수속시 전체과 전송 2013-11-05(아래로 변경)
        public static string GstrIpdAllDept = "";

        public static int GstrAL200_CNT = 0;            //AL200 건수
        public static int GstrAL200_CNT2 = 0;           //AL200 건수 이전포함
        public static string GstrAL200_NOT = "";        //AL200 강제발생안함
        public static string GstrSanAdd_NOT = "";        //TA 인증관리 가산  강제로 발생제외

        public static int GstrAL651_CNT = 0;            //AL651 건수
        public static int GstrAL651_CNT2 = 0;           //AL651 건수 이전포함

        public static string GstrDPXC4_CHK = "";        //D-PXC4 체크


        public static int GnPrintXSet = 0;              //영수증 임의X
        public static int GnPrintYSet = 0;              //영수증 임의Y
        public static string GstrCreditIF = "";         //시리얼1포트 금액표시 여부(Gstr신용장비연결)
        public static string GstrCreditBand = "";       //시리얼1포트 금액표시 여부(Gstr신용카드결제사)

        public static string Gstr100수가적용 = "";      //100/100 수가점검
        public static long GnPtVoucherAmt = 0;           //(GnPt바우처Amt)

        public static long GnVacc_Mir = 0;              //2012-06-21
        public static long GnVacc_Gam = 0;              //2012-06-21
        public static long GnVacc_Bon = 0;              //2012-06-21
        public static string GstrVacc_OK = "";          //2012-06-21

        /// <summary>
        /// 임의야간설정 기능 사용안함. 아래변수 주석처리
        /// 2017-12-19. 박병규
        /// </summary>
        //public static string GstrNight_Manual = "";     //2012-08-17

        //public static string Gstr물리SCH_사용 = "";     //2012-08-30 물리치료 신규테이블사용여부-(아래로 변경)
        public static string GstrPtNewTableFlag = "";
        public static string GstrLtdVac = "";    //2012-09-12(Gstr회사접종정보1)
        public static string GstrCrewVacOK = "";       //2012-09-20(Gstr직원접종OK)
        public static string GstrCrewVacOK2 = "";      //2012-09-20(Gstr직원접종OK2)

        public static string GstrPapillomaVacc = ""; //Gstr가다실접종


        public static string GstrOutPrescript;      //Gstr원외처방수납용
        #endregion

        #region //basAcct.bas
        public static string OBON_DATE;
        public static string OBON_TAX_DATE;
        public static string JOJE_DATE;
        public static string GISUL_DATE;
        public static string NIGHT_DATE;
        public static string NGT22_DATE;
        public static string GnWard_DATE;                //입원료 변경일자

        //외래본인부담율
        public static int[] OBON = new int[99];         //25,000원이하(50%)
        public static int[] OBON1 = new int[99];        //(55%)
        public static int[] OBON2 = new int[99];        //25,000원초과(45%)
        public static int[] OLD_OBON = new int[99];     //OLD

        //본인 부가가치율
        public static int[] BON_TAX = new int[99];
        public static int[] OLD_BON_TAX = new int[99];

        //입원본인부담율
        public static int[] IBON = new int[99];

        //투약조제료금액
        public static int[] JOJE = new int[99];
        public static int[] OLD_JOJE = new int[99];

        //병원가산율
        public static int[] GISUL = new int[10];
        public static int[] OLD_GISUL = new int[10];

        //야간/공휴율
        public static int[] NIGHT = new int[10];
        public static int[] OLD_NIGHT = new int[10];
        public static int[] NIGHT_22 = new int[10];      //마취
        public static int[] OLD_NIGHT_22 = new int[10];

        //감액율
        public static int[] GAMEK = new int[50];
        public static int[] GAMEK_JIN = new int[50];     //진찰료
        public static int[] IGAMEK = new int[50];        //일반
        public static int[] BGAMEK = new int[50];        //보험
        public static int[] OGAMEK = new int[50];        //보험100% 외래

        //병원관리료
        public static long GnWard_AMT;   //변경후
        public static long GnWard_OLD;   //변경전

        //환자관리료
        public static long GnPant_AMT;   //변경후
        public static long GnPant_OLD;   //변경전

        //행위료가산율
        public static int GnPedRate;     //소아
        public static int GnNgtRate;     //야간

        //식대
        public static string GstrFoodBoum;   //의치과
        public static string GstrFoodAdd;    //가산
        public static string GstrFoodBoho;   //의료급여

        //본인상한액
        public static string[] GstrSangBdate = new string[11];   //적용일자
        public static long[] GnSangAmt = new long[11];           //적용금액

        public static string GstrPCode;         //금액가산 산정기준 (3자리)


        public static int PedAdd1   ;//만6세미만 초진가산
        public static int PedAdd2   ;//만6세미만 초진심야가산
        public static int PedAdd3   ;//만6세미만 재진가산
        public static int PedAdd4   ;//만6세미만 재진심야가산
        public static int PedAdd5   ;//만6세미만 초진휴일가산
        public static int PedAdd6   ;//만6세미만 재진휴일가산
        public static int PedAddYg1 ;//만0세 초진가산
        public static int PedAddYg2 ;//만0세 초진심야가산
        public static int PedAddYg3 ;//만0세 재진가산
        public static int PedAddYg4 ;//만0세 재진심야가산
        public static int PedAddYg5 ;//만0세 초진휴일가산
        public static int PedAddYg6; //만0세 재진휴일가산
        public static string PedAdd_Date;
        public static string PedAddYg_Date;

        public static int Old_PedAdd1;//만6세미만 초진가산
        public static int Old_PedAdd2;//만6세미만 재진가산
        public static int Old_PedAddYg1;//만0세 초진가산
        public static int Old_PedAddYg2;//만0세 재진가산


        public static string GstrMirFlag;        //청구업무 유무("OK", "")
        public static string GstrMirDate;        //청구일자(해당월의 마지막일자)
        public static string GstrOldNew;         //청구 old와 new data read 위한 구분자
        public static string GstrBamt100Code;    //100/100 본인부담코드
        public static string GstrGamfRemark;     //감액사유(회사접종)
        public static string GstrMisuMsg;

        public static string GstrGSAdd = ""; //외과가산
        #endregion

        #region //call.bas
        public static string GstrZoneID;
        public static string GstrGroupID;
        public static string GstrDeskID;
        #endregion

        #region //OPD_세계병자의날.bas
        public static string GstrSickMent;
        public static string GstrJupsuAuth;     //대리접수, 접수2권한(Gstr접수권한_1)
        public static string GstrAutoSabun;     //현금영수증 자동사번(Gstr현금영수자동사번)
        #endregion

        #region //vb선택진료.bas
        public static string GstrSelUse;            //Gstr선택진료사용
        public static string GstrSelExceptUse;      //Gstr선택예외사용
        public static string GstrSuBun;             //수가분류
        public static string GstrSuWonCode;         //수가 원가코드
        public static string GstrSuDaiCode;         //약품분류코드
        public static string GstrSugbP;             //수가분류
        #endregion

        #region//Report_Print2.bas
        public static long[,] R2Amt = new long[101, 7];             //영수증용 R2Amt(100, 6)
        #endregion

        #region//ErAcct.bas
        public static string GstatER = ""; //응급가산여부
        public static int GnErRate = 0; //응급실 가산율
        public static int GnErRateK = 0; //권역수가 응급실 가산율
        public static int GnGSRate = 0; //권역수가 응급실 가산율
        public static int GnCSRate = 0; //권역수가 응급실 가산율
        #endregion

        #region //OpdAcct.bas
        public static string SqlSUT = "";
        public static string SqlSUGA = "";
        public static string SqlSUH = "";
        public static string SqlSUH1 = "";
        public static string SqlSUH2 = "";
        public static string SqlSUH3 = "";
        public static string SqlSUH4 = "";
        public static string SqlSUHIL = "";
        public static string SqlSUHTA = "";
        public static string SqlSUHBO = "";
        public static string SqlBASMS = "";
        public static string SqlBASMIS = "";
        public static string GstrSuCode = "";
        public static string GstrIllCode = "";
        public static long G7AMT = 0;
        public static long G7TAMT = 0;
        public static long G7NoGam = 0;             //감액 제외 total-비급여
        //public static long G7NoGam_New_Bon = 0;   //아래로 변경
        public static long G7NoGam_Bon = 0;         //감액 제외 total-급여( 2001/07월이후발생분)
        public static long G7NoGam_Old_Bon = 0;     //감액 제외 total-급역( 2001/07월이전발생분)
        public static long G7YakGam = 0;            //투약,주사료 본인부담액(감액계산용)-비급여
        //public static long G7YAKGAM_NEW_BON = 0;  //아래로 변경
        public static long G7YakGam_Bon = 0;        //투약,주사료 본인부담액(감액계산용)-급여( 2001/07월이후발생분)
        public static long G7YakGam_Old_Bon = 0;    //투약,주사료 본인부담액(감액계산용)-급여( 2001/07월이전발생분)
        public static long G7SilupGam = 0;          //실업자 감액
        //public static long G7SILUPGAM_NEW_BON = 0;//아래로변경
        public static long G7SilupGam_Bon = 0;      //실업자 감액-급여( 2001/07월이후발생분)
        public static long G7SILUPGAM_OLD_BON = 0;//실업자 감액-급여( 2001/07월이전발생분)
        public static long G7SONOMRI = 0;           //계약처 할인(SONO,MRI금액)
        public static long G7Bochel = 0;            //계약처 환자(보철료감액됨) 
        public static long GnRmAmt = 0;             //RM 스포츠재활치료(병원직원 감액 50%)

        public static long G7DNGam = 0;             //치과(직원본인감액) 감액제외인 분류(처치,수술행위만)( 보철을 행위,재료) bun=(28,29,40) 20%감액
        public static long G7DtAmt = 0;             //치과급여
        public static long G7DtBIAmt = 0;           //치과비급여트보철료
        public static long G7DtIMAmt = 0;           //치과임플란트

        public static long G7EmGam = 0;             //나자랫집,마리아집,햇빛마을(응급관리료)
        public static string G7EmFlag = "";         //응급관리료대상(Y/N)
        public static long G7SONOMRIBED = 0;        //나자랫집,마리아집,햇빛마을(SONO,MRI,병실차액)
        public static long G7Copy33 = 0;            //33종 소견서, 진단서, 필림COPY, CD복사 , 예방접종추가
        public static long G7SangCT = 0;            //상병특례 CT

        public static int G7NAL11 = 0;              //내복약 일수
        public static int G7NAL11ADD = 0;           //내복약(디비나:H-VINA)일수-원외전용약
        public static int G7NAL11A = 0;             //내복약 일수(내복+제제)
        public static int G7NAL11B = 0;             //내복약 일수(    급여  원외처방)
        public static int G7NAL11B1 = 0;            //내복약 일수(    급여  원내처방)
        public static int G7NAL11B1X = 0;           //방사선 의약품관리료
        public static int G7NAL11BADD = 0;          //내복약(디비나:H-VINA)일수-원외전용약
        public static int G7NAL11C = 0;             //내복약 일수(    비급  원외처방)
        public static int G7NAL11C1 = 0;            //내복약 일수(    비급  원내처방)
        public static int G7NAL11C1X = 0;           //방사선 의약품관리료.
        public static int G7NAL11C2 = 0;            //내복약 일수(인정비급  원내처방)
        public static int G7NAL11CADD = 0;          //내복약(디비나:H-VINA)일수

        public static int G7NAL12 = 0;              //외용약 일수
        public static int G7NAL12A = 0;             //외용약 일수(외용+제제)
        public static int G7NAL12B = 0;             //외용약 일수(    급여  원외처방)
        public static int G7NAL12B1 = 0;            //외용약 일수(    급여  원내처방)
        public static int G7NAL12C = 0;             //외용약 일수(    비급  원외처방)
        public static int G7NAL12C1 = 0;            //외용약 일수(    비급  원내처방)
        public static int G7NAL12C2 = 0;            //외용약 일수(인정비급  원내처방)

        public static int G7NAL2 = 0;               //주사약 일수(    급여  원내처방)
        public static int G7NAL2C1 = 0;             //주사약 일수(    비급  원내처방)
        public static int G7NAL2C2 = 0;             //주사약 일수(인정비급  원내처방)
        public static int G7NAL2_Teta = 0;          //주사약제(TATE) KK045발생 
        public static int G7NAL2_KK042 = 0;         //주사약제(항독소) KK042발생 

        public static int G7NAL20A = 0;
        public static int G7NAL20B0 = 0;            //KK020 Q란 0
        public static int G7NAL20B2 = 0;            //KK020 Q란 2
        public static int G7NAL20C0 = 0;            //KK020 항암제여부(Q란 0)
        public static int G7NAL20C2 = 0;            //KK020 항암제여부(Q란 2)

        public static int G7NALER11 = 0;            //응급실 퇴원약외 내복약 일수
        public static int G7NALER12 = 0;            //응급실 퇴원약외 외용약 일수
        public static int G7NALER13 = 0;            //응급실 퇴원약외 주사약 일수
        public static int G7NALERT11 = 0;           //응급실 퇴원약 내복약 일수
        public static int G7NALERT12 = 0;           //응급실 퇴원약 외용약 일수
        public static int G7NALERT13 = 0;           //응급실 퇴원약 주사약 일수

        public static int G7Self = 0;
        public static int G11Self = 0;              //투약료중 비급여 여부
        public static int G11OutSelf = 0;           //원외처방료 비급여 여부
        public static int G7AL200S0 = 0;            //AL200 Q란 0의 최대날수(+)
        public static int G7AL200S1 = 0;            //AL200 Q란 0의 최대날수(-)
        public static int G7AL200S2 = 0;            //AL200 Q란 2의 최대날수(+)
        public static int G7AL200S3 = 0;            //AL200 Q란 2의 최대날수(-)
        public static int G7AL020S0 = 0;            //AL020 날수(+)
        public static int G7AL020S1 = 0;            //AL020 날수(+)

        #region 자가투여주사 조제료 J5700 관련 신규 변수 선언
        public static int G7BUN11CNT = 0;           //내복약 종류 수납 갯수(BUN = 11)
        public static int G7BUN12CNT = 0;           //외용약 종류 수납 갯수(BUN = 12)
        public static int G7BUN20CNT = 0;           //주사약 종류 수납 갯수(BUN = 20)
        public static int G7SELFTUYAKJOJE = 0;      //자가투여주사 대상약 종류 처방개수(다른 약 없이 SUGBB항의 자가투여 주사 체크) J5700 (2021-10-20) 
        #endregion

        public static int G7AQUA0 = 0;              //보험적용 증류수의 량(cc)-WT
        public static int G7AQUA2 = 0;              //보험총액 증류수의 량(cc)-WT
        public static int G7AQUA3 = 0;              //보험적용 증류수의 량(cc)-NSA
        public static int G7AQUA4 = 0;              //보험적용 증류수의 량(cc)-NSA
        public static int G7AQUA1 = 0;              //보험적용 증류수의 량(cc)-NS10
        public static int G7AQUA5 = 0;              //보험적용 증류수의 량(cc)-NS10
        public static long GnGamekJin = 0;          //수납에서 진찰료입력시 감액 100% 대상자 금액 누적
        public static int G7WRTcount = 0;           //SLIP Write 건수
        public static int G7SMAcount = 0;           //혈액화학검사 체감율 갯수
        public static string GhostDAEPYO = "";      //* 1   //Slip의 Host 구분에 사용
        public static string GstatPED = "";         //* 1   //소아가산여부 Set
        public static string GstatHULWOO = "";      //* 1   //혈우병 Check
        public static string GstatEROVER = "";      //* 1   //응급실 6시간 Over Check
        public static string GstatQmgrBAS = "";     //평생관리항목 CHECK
        public static string GstrB11Stat = "";      //내복,외용약 발생여부
        public static string GstrB20Stat = "";      //주사실 발생여부
        public static string GstrB65Stat = "";      //방사선 전송여부
        public static string GstrINJStat = "";      //주사실
        public static string GstrINJStat2 = "";      //소아주사실
        public static string GstrINJStat3 = "";      //항암주사실
        public static string GstrPTStat = "";       //물리치료실
        public static string GstrPTStat2 = "";      //물리치료실(CRYO)
        public static string GstrCPStat = "";       //류마티스 현미경실
        public static string GstrLABStat = "";      //외래 검사실
        public static string GstrLABStat_Slide = "";//외래 검사실(병리과)
        public static string GstrENDOStat = "";     //내시경실
        public static string GstrEKGStat = "";      //기능검사(심전도실)
        public static string GstrEKGEegStat = "";   //기능검사(심전도실내 뇌파검사)


        public static string GstrEKGStat2 = "";     //기능검사(심전도실)류마티스
        public static string GstrECHOStat = "";     //심장초음파
        public static string GstrCAGStat = "";      //혈관조영실
        public static string GstrENTStat = "";      //청력검사
        public static string GstrENTStat2 = "";     //이비인후과 초음파
        public static string GstrXRAYStat = "";     //방사선
        public static string GstrXRAYStat2 = "";    //BMD실
        public static string GstrXRAYStat3 = "";    //뇌혈류검사실
        public static string GstrXRAYStat4 = "";    //비뇨기과
        public static string GstrXRAYStat5 = "";    //초음파,CT MRI
        public static string GstrXRAYStat6 = "";    //BMD루가관
        public static string GstrXRAYStat10 = "";   //방사선 대기 인원 표시 위한 X-RAY만 사용함.
        public static string GstrXRAYStat11 = "";   //동위원소
        public static string GstrXRAYStat12 = "";   //정형외과
        public static string GstrPURStat = "";      //관리과
        public static string GstrCASTStat = "";     //Cast실
        public static string GstrEMGStat = "";      //EMG검사
        public static string GstrEMGNMStat = "";      //EMG검사 - 신경과용
        public static string GstrERStat = "";       //응급실 응급관리료
        public static string GstrDMStat = "";       //피부과 검사
        public static string GstrJinDanStat1 = "";  //원무과 진단서창구
        public static string GstrDUOL1Flag = "";    //내시경실 po처방시 - DUOL1코드외 약 코드 점검
        public static string GstrErStat_Flu = "";   //응급실로 가세요
        //public static string GstrXRAYStat_본관 = "";//아래로 변경
        public static string GstrXRAYStat_MainBuild = "";//본관가셔야할것
        public static string GstrXRAYStat_49 = "";  //중증4대초음파

        public static string GstrB65Stat_Res = "";
        public static string GstrINJStat_Res = "";         //주사실예약
        public static string GstrPTStat_Res = "";          //물리치료실_예약
        public static string GstrPTStat2_Res = "";         //물리치료실(CRYO)_예약
        public static string GstrCPStat_Res = "";          //류마티스 현미경실_예약
        public static string GstrLABStat_Res = "";         //외래 검사실_예약
        public static string GstrENDOStat_Res = "";        //내시경실_예약
        public static string GstrEKGStat_Res = "";         //기능검사(심전도실)_예약
        public static string GstrEKGEegStat_Res = "";      //기능검사(심전도실내 뇌파검사)_예약
        public static string GstrEKGStat2_Res = "";        //기능검사(심전도실)류마티스_예약
        public static string GstrECHOStat_Res = "";        //심장초음파_예약
        public static string GstrCAGStat_Res = "";         //혈관조영실_예약
        public static string GstrENTStat_Res = "";         //청력검사_예약
        public static string GstrENTStat2_Res = "";        //이비인후과 초음파_예약
        public static string GstrXRAYStat_Res = "";        //방사선_예약
        public static string GstrXRAYStat10_Res = "";      //방사선 대기 인원 표시 위한 X-RAY만 사용함._예약
        public static string GstrXRAYStat2_Res = "";       //BMD실_예약
        public static string GstrXRAYStat3_Res = "";       //뇌혈류검사실_예약
        public static string GstrXRAYStat4_Res = "";       //비뇨기과_예약
        public static string GstrXRAYStat5_Res = "";       //초음파,CT MRI_예약
        public static string GstrXRAYStat11_Res = "";      //동위원소_예약
        public static string GstrXRAYStat12_Res = "";      //정형외과_예약
        public static string GstrXRAYStat6_Res = "";       //BMD루가관_예약
        public static string GstrEMGStat_Res = "";         //EMG검사_예약
        public static string GstrEMGNMStat_Res = "";       //EMG검사-NM 신경과용_예약
        public static string GstrEMGStatF633 = "";          //EMG검사-어지럼증검사
        public static string GstrEMGStatF633_Res = "";     //EMG검사-어지럼증검사_예약


        public static string GstrBirthStat = "";            //생일여부
        public static string GstrBunup11 = "";              //* 1   //내복 원외처방전(Y.원외 N.원내)
        public static string GstrBunup12 = "";              //* 1   //외용 원외처방전(Y.원외 N.원내)
        public static string GstrBunup20 = "";              //* 1   //주사 원외처방전(Y.원외 N.원내)

        public static string GstrHDOtherGwaGb = "";         //HD접수후 타과수납시 원외처방전 발행 구분(2013-01-15)(GstrHD타과원외구분)
        public static string GstrERInSideJoje = "";         //ER수납시 ##24 원내조제 코드 확인 구분(2013-05-14)(GstrER원내조제)

        public static string GstrMFO5_InSideYN = "";       //2011-07-01 원내조제시 추가발생오더 체크(GstrMFO5_원내YN)
        public static string GstrInSideOutSide = "";        //2012-03-19 원내원외동시 Y(Gstr원내원외동시)

        public static string GstrSkipCHK = "";              //수가 계산시 BAS_MSELF에서 상병 Check Skip
        public static string GstrIllSelf = "";              //BAS_MISELF에서 상병 Check 본인부담 Check
        public static long GnTotalAmt = 0;                  //총진료비(25000원초과이하판단용)
        public static long GnTotChAmt = 0;                  //예약진찰료 진찰료 차액
        public static long GnJoHapChAmt = 0;                //예약진찰료 조합부담 차액
        public static long GnBonChAmt = 0;                  //예약진찰료 본인부담 차액

        public static long GnBoninAmt_EF;                   //차상위2종 1,500원 100원금액


        public static string GstrF009;                      //잠복결핵검진 대상자
        public static string GstrF010;                      //잠복결핵치료 대상자

        public static long GnBimanFMAmt;     //감액 - 가정의학과 비만 
        public static long GnAC101Amt;       //감액 - 직원응급관리료 
        public static long GnAC101AmtBon;    //감액 - 직원응급관리료(본인부담금) 
        public static string GstrAC101Self;  //감액 - 직원응급관리료 급여/비급여 구분 

        public static long GnDrugAmt = 0;       //의약분업 약값 합산 2005-09-01
        public static long GnNPInjAmt = 0;        //NP 향정신성장기지속형 주사제 약제비 2017-03-13
        public static long GnDtTrmAmt = 0;        //치과 치면열구전색술 처치비 2017-10-01
        public static long GnSpecAmt = 0;         //장루,요루장애인 치료비 합산 2011-10-01 시행
        public static long GnToothAmt = 0;        //노인틀니
        public static long GnImplantAmt = 0;      //노인임플란트
        public static long GnNPNnAmt = 0;        //2018.06.25 박병규 : 개인정신치료코드금액
        public static long GnJinRP의뢰료 = 0;        //2018.06.25 박병규 : 개인정신치료코드금액
        public static long GnJinRP회신료 = 0;        //2018.06.25 박병규 : 개인정신치료코드금액
        public static long GnJinRP검사료 = 0;        //2020.02.05 신종코로나검사 금액

        //주석해제!
        public static long GnJinRP재택결핵 = 0;      //2021-09-16 재택 결핵 상담료/관리료 금액 

        public static long GnJinRP격리료 = 0;        //2018.06.25 박병규 : 격리료코드금액


        public static long GnTaxAmt = 0;    //부가세 총액       2014-02-24
        public static long GnTaxDan = 0;    //부가세 절사금액   2014-02-24
        public static long Gn100SAmt = 0;   //100/80, 100/50 총액   2014-03-26
        public static long Gn100SAmt1 = 0;  //100/80, 100/50 본인   2014-03-26
        public static long Gn100SAmt2 = 0;  //100/80, 100/50 공단   2014-03-26
        public static long GnERKekliAmt = 0; //ER 격리병실 총액   2016-10-01
        public static long GnERKekliAmt1 = 0; //ER 격리병실 본인   2016-10-01
        public static long GnERKekliAmt2 = 0; //ER 격리병실 공단   2016-10-01
        public static long GnJinTcrcAmt = 0; // 진료의뢰 회신금액

        public static string GstrXrayPlain;             //단순촬영 대상구분

        public static int GnCntNal = 0;
        public static string GstrWonLe = ""; //GnWonLe

        //public static string Gstr단순촬영대상 = ""; //아래로 변경
        public static string GstrSimpleXray = "";

        //public static long GnJin회신료 = ""; //아래로 변경
        public static long GnJinSenAmt = 0;    //진료의뢰료
        public static long GnJinRtnAmt = 0;
        public static long GnJinExamnAmt = 0; //코로나 바이러스 검사 국비100%

        //주석해제!!
        public static long GnJinTuberEduAmt = 0;   //재택의료 결핵환자 관리/상담료(외래용) 2021-09-16

        //코로나 검사 취합 1단계 장애인의 경우 장애인지원금에서 지원하기 위함.(한시적)
        //코로나 검사가 본인 20%로 선별처럼 행위가 발생하여 문제. 별도로 가야함.
        //금액 체크를 위해서 만든 변수인지라 총액에 더하지는 않음.
        public static long GnNCOV_P1 = 0;
        //============================================================================

        public static long GnOutKeepAmt = 0;  //퇴장방지
        public static string GstrPCLR = ""; //환자환의 수납여부

        public static long GnBohoAmt;   //의료급여영수금 전 Silp에 급여합산 1500,2000
        public static long GnBohoAmt53; //의료급여조합금 전 Silp에 합산 금액을 표시
        public static long GnBohoAmt54; //의료급여감액 전 Silp에 합산 금액을 표시
        public static long GnBohoAmt56; //의료급여미수금 전 Silp에 합산 금액을 표시
        public static long GnBohoAmt64; //의료급여영수금 전 Silp에 합산 금액을 표시
        public static long GnBohoAmt99; //의료급여영수금 전 Silp에 급여합산 금액
        public static long GnBohoCTMRI; //의료급여CT.MRI 본인부담액

        public static long GnNPGAmt;   //차상위2종영수금 전 Silp에 급여합산 1500,2000
        public static long GnNPGAmt53; //차상위2종 조합금 전 Silp에 합산 금액을 표시
        public static long GnNPGAmt54; //차상위2종감액 전 Silp에 합산 금액을 표시
        public static long GnNPGAmt56; //차상위2종미수금 전 Silp에 합산 금액을 표시
        public static long GnNPGAmt64; //차상위2종영수금 전 Silp에 합산 금액을 표시
        public static long GnNPGAmt99; //차상위2종영수금 전 Silp에 급여합산 금액
        public static long GnNPGCTMRI; //차차상위2종 CT.MRI 본인부담액

        #endregion //opdacct(opdacct.bas)

        #region //Opd_Acc_New.bas
        public static string GstrJupsuDansu;    //접수단수체크(Gstr접수단수)
        public static string GstrTruncGb;       //절사시 구분(Gstr절사구분) : 수납,부분취소
        public static long GnHTruncLastAmt;     //당일 수납최종 절사액 100미만(Gn현최종절사액)
        public static long GnBTruncLastAmt;     //이전 수납최종 절사액 100미만(Gn이전최종절사액)
        #endregion

        #region //Oumsad_CHK.bas
        public static long GnHTAmt; //해바라기 성폭력 지원금 한도 총액
        public static long GnHSAmt; //해바라기 성폭력 지원금 사용액
        public static long GnHCAmt; //해바라기 성폭력 지원금 한도 잔여액

        public static long GnSCAmt; //금연치료 상담료 총액
        public static long GnSCAmt1; //금연치료 상담료 본인
        public static long GnSCAmt2; //금연치료 상담료 공단
        public static string GstrSCInCent; //금연치료 3회이상 이수 대상자(본인부담없음) 2016년 1월부터 시행

        public static int GnDrugNal; //만성질환자 관련 약 처방일수
        public static string GstrSimpleDressing; //Gstr단순처치
        public static string GstrChkMsg; //클리어는 함수진입 지점에서 사용

        #endregion

        #region //Oumsad.bas
        public static string GstrJupsuMainFlag;   //신환접수
        
        //기초코드 설정관련 변수
        public static string[] GstrSetAreas = new string[10];            //타지역여부
        public static string[] GstrSetBis = new string[60];             //환자종류
        public static string[] GstrSetChoJaes = new string[10];          //초재구분
        public static string[] GstrSetDeptCodes = new string[50];       //진료과코드
        public static string[] GstrSetDepts = new string[50];           //진료과목명
        public static string[] GstrSetDoctors = new string[100];         //의사명

        public static string[] GstrSetGwanges = new string[10];          //피보관계
        public static string[] GstrSetJangaes = new string[10];          //장애여부
        public static string[] GstrSetJiyuks = new string[99];          //지역명
        public static string[] GstrSetSpcs = new string[10];             //특진구분
        public static string[] GstrSetZips = new string[99];            //우편번호
        public static string[] GstrSetSunapTit = new string[10];         //수납영수증의 항목

        public static string[,] gstrSetDrCodes = new string[50, 100];    //각과별의사코드
        public static string[,] gstrSetDrNames = new string[50, 100];    //각과별의사명

        //날짜관련 변수
        //public static string GstrSysDate;//clsPublic의 변수를 이용
        public static string GstrSysTime;//clsPublic의 변수를 이용
        public static string GstrSysTomorrow;//clsPublic의 변수를 이용
        public static string GstrActDate;//clsPublic 변수이용
        public static string GstrBdate;//clsPublic 변수이용          //접수내역 Read시 실진료일


        public static string GstrLostFocus;     //LostFocus시 2번 이벤트 발생을
        public static string GstrCallFrm;
        public static string GstrFrom;
        public static string Gstr구분변경;
        public static string GstrSi22;          //시설환자(나자렛집) 진찰료 0원
        public static string GstrCardIO;
        public static string GstrErJobFlag;
        public static string GstrJobFlag;            //접수여부(0:주간, 1:야간, 2:휴일)
        public static string GstrSunapPtno;          //접수시 수납화면 자동전환되는 등록번호 사용(물리치료등)


        public static int GnPrtIpdNo;           //프린터설정
        public static int GnPrtIpdNo2;          //신용카드용
        public static int GnPrtIpdNo3;          //신용카드용
        public static int GnPrtIpdNo4;          //신용카드용
        public static int GnPrtIpdNo5;          //원외처방용

        //환자인식밴드 및 접수프로그램 위치정보(원외처방발행용)
        public static string GstrPrtBun;        //접수프로그램의 위치파악
        public static string GstrPrtBun2;       //접수프로그램의 위치파악2 >> 사용안함
        public static string GstrPrtBand;       //환자인식밴드 출력위치
        //포트가 열렸는지 변수 이미 열렸을땐 DLL파일로 여는것은 불가능하다
        public static int GnSerial;
        public static int GnSerial_Result;
        public static Boolean GblnPortOpen;

        public static string GstrPtnoGbn;
        public static string GstrSname;         //신환번호 부여시 사용
        public static string GstrSex;
        public static int GnAge;
        public static string GstrJuminFlag;
        public static string GstrJumin1;
        public static string GstrJumin2;
        public static string GstrZipFlag;
        public static string GstrValue;
        public static string GstrValue_2;
        public static string GstrPanoGbn;

        public static string GstrDrCode;
        public static int GnIndex;
        public static int GnTuyakNo;            //투약번호
        public static int GnOutTuyakNo;         //원외처방전 투약번호
        public static int GnOutDrugNo;          //원외처방전 번호
        public static string GstrOutDrugPrintGubun;          //원외처방전 인쇄 구분(1.약국제출용만 출력, 2.환자보관용만 출력, 그외 2개다 출력)    //2021-11-16
        public static int GDrSeqNo;             //의사별 접수번호

        //신환환자 추천
        public static string GstrSinSabun;       //사번
        public static string GstrKname;          //성명
        public static string GstrSinGubun;       //관계


        public static string GstrMiaFlag;
        public static string GstrMiaBdate;
        public static string GstrMiaGubun;
        public static string GstrMiaClass;
        public static string GstrMiaDetail;
        public static string GstrReturnMiaCode;
        public static string GstrReturnMiaName;
        public static string GstrReturnMiaClass;


        public static double GnJeaAmt;            //재계산 영수금액
        public static string GstrJinGubun;
        public static double GnDeaLiAmt;
        public static string GstrNight1;

        public static string GstrJeaPrint;      //외래영수증 재발행
        public static string GstrJeaEtcPrint;   //기타수납 재발행

        public static int GnDeaGi;              //대기인원 표시

        public static string GstrJeaBi;
        public static string GstrJeaDept;
        public static int GnJeaSeqNo;
        public static string GstrJeaDate;
        public static string GstrJeaPart;
        public static string GstrJeaName;
        public static string GstrJeaSunap;//opd_sunap 테이블에 insert 안함.

        public static string GstrOcsReserved;   //Ocs Order에서 예약사항 전달여부
        public static string GstrSunapRep;
        public static string GstrJupsuRep;

        public static string GstrGamGubun;      //감액구분(자격)
        public static string GstrGamCaseGubun;  //감액구분(CASE)
        public static string GstrGamEnd;        //감액종료일
        public static string GstrGamMsg;        //감액 메시지
        public static string GstrFlagGam;

        public static string GstrDrugNoSend;    //약국 전송여부
        public static string GstrJojeYn;        //조제료 발생여부

        public static bool GnllFlag;            //ll과 여부(true:ll과, false:기타과)
        public static string GstrllDept;

        public static string GstrMassage;
        public static string GstrChk_Boho = ""; 

        public static string GstrGwa;
        public static string GstrOtherHD;       //동일 타과 인공신장체크
        public static string GstrOtherHD_MCode; //동일과 HD자격

        public static double gnJinAMT1;           //진찰료 발생금액
        public static double gnJinAMT2;           //진찰료 특진료
        public static double gnJinAMT3;           //진찰료 총액    
        public static double gnJinAMT4;           //진찰료 조합부담
        public static double gnJinAMT5;           //진찰료 감액
        public static double gnJinAMT6;           //진찰료 미수감액
        public static double gnJinAMT7;           //진찰료 영수금액

        public static double gnJinAMT1T;          //진찰료 발생금액(전화예약)
        public static double gnJinAMT2T;          //진찰료 특진료(전화예약)
        public static double gnJinAMT3T;          //진찰료 총액(전화예약)
        public static double gnJinAMT4T;          //진찰료 조합부담(전화예약)
        public static double gnJinAMT5T;          //진찰료 감액(전화예약)
        public static double gnJinAMT6T;          //진찰료 미수감액(전화예약)
        public static double gnJinAMT7T;          //진찰료 영수금액(전화예약)

        public static double GnGAmt1;             //의료질평가지원금 발생금액
        public static double GnGAmt2;             //교육수련지원금 발생금액
        public static double GnGAmt3;             //기술지원금 발생금액

        public static string GstrChkHIV;        //인체면역결핍 대상자
        public static double GnJinDanAmt;         //진찰료 절사금액

        public static double GnJinAmtTel;         //진찰료 전화접수
        public static double GnJinAmtTel2;        //진찰료 전화접수
        public static double GnJinAmtYeyak;       //진찰료 예약금액
        public static double GnJinAmtYeyak_Sel;   //진찰료 예약금액(선택)
        public static double GnPrintChaAmt;       //출력물 차액 금액

        public static double[] GnJinAmts = new double[14];  //1.초진료             2.초진심야
                                                            //3.재진료             4.재진심야
                                                            //5.초진특진료         6.재진특진료
                                                            //7.물리치료진찰료     8.환자가족  
                                                            //9.금연처방초진       10.금연처방재진
                                                            //11.의료질평가지원금  12.교육수련분야지원금 13.기술분야지원금

        public static double[] GnJinAmts1 = new double[12]; //1.초진료             2.초진심야
                                                            //3.재진료             4.재진심야
                                                            //5.초진특진료         6.재진특진료
                                                            //7.물리치료진찰료     8.환자가족  
                                                            //9.초진휴일           10.재진휴일
                                                            //11.NP단일접수비
        public static double[] GnJinAmts1_Old = new double[11];

        public static double[] GnJinAmts2 = new double[16]; //1.초진(내과)         2.초진(외과)         3.초진(기타)
                                                            //4.재진(내과)         5.재진(외과)         6.재진(기타)
                                                            //7.초진심야(내과)     8.초진심야(외과)     9.초진심야(기타)
                                                            //10.재진심야(내과)    11.재진심야(외과)    12.재진심야(기타)
                                                            //13.재진가족(내과)    14.재진가족(외과)    15.재진가족(기타)
        public static double[] GnJinAmts3 = new double[12]; //1.초진료(일반)             2.초진심야(일반)   
                                                            //3.재진료(일반)             4.재진심야(일반)
                                                            //5.초진특진료(일반)         6.재진특진료(일반)
                                                            //7.물리치료진찰료(일반)     8.환자가족(일반)  
                                                            //9.초진휴일(일반)           10.재진휴일(일반)
                                                            //11.NP단일접수비(일반)



        public static int GnReportNo;               //영수증 동시에 출력시 오류로 PIF 9개 사용
        public static Boolean GnllDeptFlag;         //ll과 여부 Flag(false:일반과, true:ll과)
        public static string GstrJinBonFlag;        //진찰료 본인부담율(0.기타 1.보험60%, 2.보험45%, 3.보험20%, A.보호 1,500원)
        public static int GnGyeJin;                 //산재공상, 남부경찰서 후불처리변수(1.후불) Jin_Amt_Account함수에 사용
        public static string GstrOpdSunap;          //OK:외래진료과에서 수납금액이 0원 확인시 사용함

        public static double GnPatAmt;              //환자에게 돈을 받는 금액
        public static double GnJanAmt;              //잔액
        public static string GstrExamDept;

        public static string GstrCanCer;            //중증(암)환자 VCODE
        public static int GnOrAnSlipCnt;            //수술,마취과 선수납 SLIP 건수

        public static string[] Gm1_Hic = new string[11];    //의료급여자격요청
        public static string[] Gm2_Hic = new string[23];    //자격요청후 결과
        public static string[] Gm3_Hic = new string[11];    //의료급여 승인요청
        public static string[] Gm4_Hic = new string[4];     //의료급여 승인완료
        public static string[] Gm5_Hic = new string[2];     //의료급여 승인취소요청
        public static string[] Gm6_Hic = new string[2];     //의료급여 승인취소요청후 결과

        public static string Gm4_Hic_Bun;           //의료급여 1.처방전미발행  2.처방전발행
        public static double Gm2_Hic_GAmt;          //의료급여 생활유지비 잔액
        public static double Gm2_Hic_RemAmt;        //의료급여 산전지원금 잔액
        public static string GstrHoan;              //의료급여 환불할때 투약번호 새로 생성
        public static string GstrIOGubun;           //의료급여 입원할경우 #조 처리하기 위해

        public static string GstrBunup2;            //의료급여 원외처방전
        public static string GstrDrugBunup2;        //의료급여 원내조제
        public static string GstrJupsuCode2;        //의료급여 진료비(진찰료)
        public static string GstrGemsa2;            //의료급여 검사시행

        public static string GstrDocMsg;
        public static string GstrNhic_Message;
        public static string GstrDrugJobGb;         //Gstr약처방작업구분;
        public static string GstrRTel_Ptno;
        public static string GstrCopyPrint; //Gstr사본출력
        public static string GstrPrintChk;          //Gstr출력확인(인쇄시 확인여부)
        public static string Gstr입원재출력;
        
        public static string GstrHighRiskMother; //Gstr고위험임신부
        public static string GstrOpdMother; //Gstr임신부외래
        public static string GstrLowWeightBaby; //Gstr저체중조산아

        public static string strDataFlag;
        public static string GnNight1;

        public static string GstrMobileSunap;   //Gstr모바일수납
        public static string GstrMobilePrint;   //Gstr모바일출력
        public static string GstrMPPreSC = "";  //모바일 처장전 생성용 2017-03-06 KMC
        public static string GstrGo = "";       //모바일 처장전 생성용 2017-03-06 KMC
        public static int GstrGo_TuNo ;       //모바일 투약번호

        //public static string GstatEROVER;           //응급실 중증환자여부 (clsPmpaPb.GstatEROVER  응급실 6시간 Over Check 사용)
        public static string GstrSPR;               //GstrNP조현병외래
        public static string GstrPowder;               //파우더 가산유무
        #endregion

        #region //OpdGamek.bas
        public static string GstrSpecail_Gam;//특별감액률 적용 여부(자격)
        public static string GstrSpecail_GamC;//특별감액률 적용 여부(CASE)

        public static string GstrTObun;//구분변경시 종합건진 감액 때문에

        #endregion

        #region //Drug_out_atc.bas
        public static string GstrInDrugFlag;        //Gstr원내약처방구분Flag
        public static string GstrOutDrugFlag;       //Gstr원외약처방구분Flag
        public static string GstrInDrugMsg1;        //Gstr원내약멘트1
        public static string GstrInDrugMsg2;        //Gstr원내약멘트2
        public static string GstrOutDrugMsg1;       //Gstr원외약멘트1

        public static string GstrPrintData;         //GstrPrintData_NEW
        #endregion

        #region //Oviewa1.bas
        public static string GstrCONNECT = "";
        public static string GDate = "";
        public static string GPart = "";
        public static string GstrDoctMsg = "";
        public static string GstrView1 = "";
        public static string gstrView2 = "";


        public static int GnPassCount = 0;
        public static int GYNchk = 0;
        public static int GPrintSelect = 0;
        public static int GprogramID = 0;
        public static int GMenuPersonJub = 0;
        public static int GMenuPersonJin = 0;
        public static int GnChoice = 0;
        public static int GnStart = 0;
        #endregion

        #region //MUMAST.bas

        //public static string[] GstrClassID = new string[10];
        //public static string[] GstrMgr = new string[6];
        //public static string[] GstrBond = new string[4];
        //public static string[] GstrMisuIO = new string[2];          //외래,입원
        public static string[] GstrMisuClass = new string[18];      //미수종류
        public class GstrBunSu // 2018-12-01 김해수
        {
            public string GstrMiaCode;
            public string GstrMiaName;
            public string GstrClass;
            public long GnWRTNO;
        }
        
        public class cMisuMst // 2018-12-01 김해수
        {
            //COMBO SETTING 에 사용 
            public string[] GstrClassID = new string[10];
            public string[] GstrMgr = new string[6];
            public string[] GstrBond = new string[4];
            public string[] GstrMisuIO = new string[2];          //외래,입원
            public string[] GstrMisuClass = new string[18];      //미수종류
            public string[] GstrMisuBun;                         //미수분류
            public string[] GstrMisuSayu = new string[8];        //미수사유
            public string[] GstrMisuMgr = new string[6];         //미수등급
            public string[] GstrMisuGye = new string[18];        //SLIP계정
            public string[] GstrDept = new string[31];           //진료과
        }

        #endregion

        #region // IORDERSEND.bas  
        public static string GstrSelBDate = "";
        public static string GstrEntDate = ""; 
        public static string GstrProcessChk = "";
        public static string GstrErKTAS = "";
        public static string GstrInDate39 = string.Empty;
        public static string GstrInTime39 = string.Empty;
        public static int GnFee6;
        #endregion

        #region //Ipdacct(Ipdacct.bas)
        public static int MAX_SA = 0;                       //배열값 관리 변수
        public static int GnWrtSeqNo = 0;                   //Slip Write SeqNo 관리
        public static string GstatQmgrIPD = "";             //재원기간관리 CHECK
        public static string GstrB20STAT = "";              //수액 Data 발생여부
        public static string GstrB11STAT = "";              //약국 ATC 전송여부
        public static string GstrB65STAT = "";              //방사선 전송여부
        public static double[] G7QTY11 = new double[60];    //내복약, 외용약 개수
        public static double[] G7QTY20A = new double[60];   //IM주사 수량
        public static double[] G7QTY20B = new double[60];   //IV주사 수량
        public static int[] G7AL201 = new int[60];          //AL201(의약품관리료) 수량
        public static double[] G7AL010 = new double[60];          //AL010(마약의약품관리료) 수량
        public static string GstrAcctJob = "";              //입원작업구분 공용변수 (구분변경, 심사, 퇴원작업)  ICUPDT,
        #endregion
        
        #region//IUMENT.bas
        public static string GstrChk = string.Empty;        //1.강제퇴원 2.재원
        #endregion

        #region//IARCACT.bas  
        public static string GstrGbChild = string.Empty;        //Slip Child 구분
        public static string GstrGbChild_Temp = string.Empty;   //Child 구분 이전값 임시보관

        #endregion

        #region//ICUPDT.bas
        //public static string GstatWRITE = string.Empty;
        public static string GstrARC = string.Empty;            //ARC 구분
        public static string GstrDietDate = string.Empty;
        #endregion

        #region//vb의료급여승인.bas
        /// <summary>
        /// Description : Const 변수
        /// Author : 박병규
        /// Create Date : 2017.07.10
        /// </summary>
        /// <seealso cref="frm의료급여승인.frm"/> 
        public const int Max_Wait_Time = 20;
        public static long GnBonInAmt;

        public static string GstrMPtno;//
        public static string GstrMPtnoChk;
        #endregion

        /// <summary>
        /// Description : string 변수
        /// Author : 안정수
        /// Create Date : 2017.09.27
        /// </summary>
        #region //SengSanView.bas
        public static string GstrName           = string.Empty;
        public static string GstrPANO           = string.Empty;
        public static string GstrYear           = string.Empty;
        public static string GstrStartDate      = string.Empty;
        public static string GstrLastDate       = string.Empty;
        public static string GstrZipCode        = string.Empty;
        public static string GstrJiname         = string.Empty;
        public static string GstrJuso           = string.Empty;
        public static string GstrJuso1          = string.Empty;
        public static string GstrFal            = string.Empty;
        public static string GstrJumin          = string.Empty;
        public static string GsPrtDefaultName   = string.Empty;
        public static string GstrFDate          = string.Empty;
        public static string GstrTDate          = string.Empty;
        public static string GstrHelp           = string.Empty;

        public static long GnDrugRPAmt;             //약값
        public static long GnToothRpAmt;            //노인틀니
        public static long GnJinRp회신료;           //회신료
        public static long GnJinRp의뢰료;           //회신료
        //주석해제!!
        public static long GnJinRp재택결핵;           //결핵관련 교육,관리료 (ID110, ID120, ID130) 2021-09-16 영수증용

        public static long GnOpd_Sunap_LastDan;     //영수증용 수납최종 절사
        public static long GOpd_Sunap_Boamt;        //영수증용 보호금액
        public static long GOpd_Sunap_EFamt;        //영수증용 차상위금액
        public static string GOpd_Sunap_GelCode;    //영수증용 계약코드
        public static string GOpd_Sunap_MCode;      //영수증용 MCode
        public static string GOpd_Sunap_VCode;      //영수증용 VCode  
        public static string GOpd_Sunap_Jin;        //영수증용 Jin
        public static string GOpd_Sunap_JinDtl;     //영수증용 JinDtl
        public static string GOpd_Sunap_JinDtl2;    //영수증용 JinDtl2       
        public static string GOpd_Sunap_II;    //영수증용 II       
        #endregion

        /// <summary>
        /// Description : string 변수
        /// Author : 안정수
        /// Create Date : 2017.10.12
        /// </summary>
        #region //IUSENT_CHK.bas
        public static string Gstr누적계산New;       //2015-10-22
        #endregion


        /// <summary>
        /// Description : string 변수
        /// Author : 안정수
        /// Create Date : 2017.10.18
        /// </summary>
        #region wonmok.bas
        public static string[] strBis = new string[56];
        #endregion

        /// <summary>
        /// Description : string 변수
        /// Author : 안정수
        /// Create Date : 2017.10.20
        //================================================================
        //2012-08-16 김현욱 작성
        //콘트롤의 Tag 명을 이용한 폼내용 일괄 입력 프로그램
        //그 외 기타 필요한 함수 포함
        //================================================================
        /// </summary>
        #region bagaage.bas
        public static string[] GstrColName;
        public static string[] GstrColValue;
        public static bool GbCreateData;

        public static string GstrSEQNO;
        public static string GstrOPTION;
        public static string GstrIO;

        public static string GstrLoadFormName;

        public static string GstrXML;
        public static string GstrBuseGrade0;
        #endregion

        public static bool GblnPtCheck;
        public static int GintJinCode;


        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2017.11.03
        /// </summary>
        /// <seealso cref="opd_fm_resv.bas:Varient_Clear"/>
        public void Varient_Clear()
        {
            clsPublic.GnChoInWon_A = 0;
            clsPublic.GnJaeInWon_A = 0;
            clsPublic.GnChoInWon_P = 0;
            clsPublic.GnJaeInWon_P = 0;
        }

        public static bool GblnResvPrint = false; //수납화면 예약증출력여부

        /// <summary>
        /// Description : InputBox Form 생성
        /// Author : 박병규
        /// Create Date : 2017.08.07
        /// </summary>
        /// <param name="title"></param>
        /// <param name="promptText"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textbox = new TextBox();
            Button btnOk = new Button();
            Button btnCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textbox.Text = value;

            btnOk.Text = "OK";
            btnCancel.Text = "Cancel";
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textbox.SetBounds(12, 36, 372, 20);
            btnOk.SetBounds(228, 72, 75, 23);
            btnCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textbox.Anchor = textbox.Anchor | AnchorStyles.Right;
            btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textbox, btnOk, btnCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = btnOk;
            form.CancelButton = btnCancel;

            DialogResult dResult = form.ShowDialog();
            value = textbox.Text;
            return dResult;

        }

        public void CHECK_TIME(TextBox tb)
        {
            string strOK = "NO";

            if (tb.Text.Length != 5)
            {
                ComFunc.MsgBox("시간은 [00:00] 으로 표기해야 합니다.", "확인");
                tb.Focus();
                return;
            }

            for (int i = 1; i <= tb.Text.Length; i++)
                if (VB.Mid(tb.Text, i, 1) == ":") { strOK = "OK"; }

            if (strOK == "NO")
            {
                ComFunc.MsgBox("시간은 [00:00] 으로 표기해야 합니다.", "확인");
                tb.Focus();
                return;
            }
            
            if (Convert.ToInt32(VB.Left(tb.Text, 1)) > 24)
            {
                ComFunc.MsgBox("시간 표기가 잘못되었습니다.", "확인");
                tb.Focus();
                return;
            }

            if (Convert.ToInt32(VB.Right(tb.Text, 1)) > 59)
            {
                ComFunc.MsgBox("분 표기가 잘못되었습니다.", "확인");
                tb.Focus();
                return;
            }
        }

        public void CHECK_TIME(DateTimePicker o)
        {
            string strOK = "NO";

            if (o.Value.ToString().Length != 5)
            {
                ComFunc.MsgBox("시간은 [00:00] 으로 표기해야 합니다.", "확인");
                o.Focus();
                return;
            }

            for (int i = 1; i <= o.Value.ToString().Length; i++)
                if (VB.Mid(o.Value.ToString(), i, 1) == ":") { strOK = "OK"; }

            if (strOK == "NO")
            {
                ComFunc.MsgBox("시간은 [00:00] 으로 표기해야 합니다.", "확인");
                o.Focus();
                return;
            }


            if (Convert.ToInt32(VB.Left(o.Value.ToString(), 1)) > 24)
            {
                ComFunc.MsgBox("시간 표기가 잘못되었습니다.", "확인");
                o.Focus();
                return;
            }

            if (Convert.ToInt32(VB.Right(o.Value.ToString(), 1)) > 59)
            {
                ComFunc.MsgBox("분 표기가 잘못되었습니다.", "확인");
                o.Focus();
                return;
            }
        }

        /// <summary>
        /// ADMIN.IPD_NEW_MASTER 
        /// </summary>

        public enum enmSelIpdMstTrs
        {
            TRSNO, IPDNO, PANO, GBIPD, DEPTCODE, DRCODE, ILSU, INDATE, OUTDATE, ACTDATE, BI, KIHO, GKIHO, PNAME, GWANGE, BONRATE, GISULRATE,
            AMSET1, AMSET2, AMSET3, AMSET4, AMSET5, AMSETB, FROMTRANS, ERAMT, JUPBONO, GBDRG, DRGWRTNO, SANGAMT, DTGAMEK, OGPDBUN,
            IllCode1, IllCode2, IllCode3, IllCode4, IllCode5, IllCode6, SName, Age, Sex, MGbSts, WardCode, RoomCode, GBGAMEK, BOHUN, JSIM_REMARK,
            GbSuDay, MiIlsu, MIARCDATE, M_InDate, M_OutDate, M_ActDate, ArcDate, M_Ilsu, Fee6, ArcQty, GbKekli, IcuQty, GelCode, GbTewon,
            AMT01, AMT02, AMT03, AMT04, AMT05, AMT06, AMT07, AMT08, AMT09, AMT10, AMT11, AMT12, AMT13, AMT14, AMT15, AMT16, AMT17, AMT18, AMT19, AMT20,
            AMT21, AMT22, AMT23, AMT24, AMT25, AMT26, AMT27, AMT28, AMT29, AMT30, AMT31, AMT32, AMT33, AMT34, AMT35, AMT36, AMT37, AMT38, AMT39, AMT40,
            AMT41, AMT42, AMT43, AMT44, AMT45, AMT46, AMT47, AMT48, AMT49, AMT50, AMT51, AMT52, AMT53, AMT54, AMT55, AMT56, AMT57, AMT58, AMT59, AMT60,
            AMT61, AMT62, AMT63, ROUTDATE, SIMSATIME, PRINTTIME, SUNAPTIME, MIRBUILDTIME, GBJIWON,
            GBCHECKLIST, TGbSts, Vcode, JUMIN1, JUMIN2, JUMIN3, GBSANG, OGPDBUNdtl, OGPDBUN2, Gbilban2, GbSPC, DrgCode,
            JSIM_LDATE, JSIM_SABUN, JSIM_SET, JSIM_OK, FCODE, KTASLEVL, T_CARE, TROWID
        };

        public enum enmIpdMst
        {
            IPDNO, WardCode, RoomCode, Pano, Bi, Sname, Sex, Age, InDate, WardInTime, DeptCode, DrCode, Ilsu, Pname, GbSpc,
            GbSPC2, GbKekli, Bohun, Religion, Jiyuk, IpwonTime, ArcQty, IcuQty, Im180, Fee6, GbGameK, AmSet1, AmSet4, AmSet5, AmSet6,
            AmSet7, AmSet8, AmSet9, AmSetA, GelCode, FromTrans, ErAmt, Remark, JupboNo, article, Gbcancer, InOut, Other, GbDonggi, OgPdBun,
            TrsCnt, LastTrs, GbSTS, OUTDRUG, OUTDEPT, GBSUDAY, Secret, Secret_sabun, PNEUMONIA, Pregnant,
            GbGoOut, GbNight, TelRemark, GbExam, THYROID, GbDrg, DrgCode, JDate, JobSabun, SECRETINDATE,
            KTASLEVL, FROOM, FROOMETC, GBJIWON, T_CARE, OPDNO, PASSINFO, RTNHOSP
        }

        /// <summary>
        /// ADMIN.IPD_TRANS
        /// </summary>
        public enum enmIpdTrs
        {
            TRSNO, IPDNO, PANO, GBIPD, INDATE, OUTDATE, DEPTCODE, DRCODE, ILSU, BI, KIHO, GKIHO, PNAME, GWANGE, BONRATE, GISULRATE,
            GBGAMEK, BOHUN, AMSET1, AMSET2, AMSET3, AMSET4, AMSET5, AMSETB, FROMTRANS, ERAMT, JUPBONO, GbSPC, GBDRG, DRGCODE,
            AMT01, AMT02, AMT03, AMT04, AMT05, AMT06, AMT07, AMT08, AMT09, AMT10, AMT11, AMT12, AMT13, AMT14, AMT15, AMT16,
            AMT17, AMT18, AMT19, AMT20, AMT21, AMT22, AMT23, AMT24, AMT25, AMT26, AMT27, AMT28, AMT29, AMT30, AMT31, AMT32,
            AMT33, AMT34, AMT35, AMT36, AMT37, AMT38, AMT39, AMT40, AMT41, AMT42, AMT43, AMT44, AMT45, AMT46, AMT47, AMT48,
            AMT49, AMT50, AMT51, AMT52, AMT53, AMT54, AMT55, AMT56, AMT57, AMT58, AMT59, AMT60, AMT64,
            ENTDATE, ENTSABUN, GBSTS, VCODE, OGPDBUN, OGPDBUNdtl, GELCODE, Gbilban2, KTASLEVL
        }

        /// <summary>
        /// ADMIN.BAS_PATIENT
        /// </summary>
        public enum enmBasPat
        {
            Pano, Sname, Sex, Jumin1, Jumin2, Jumin3, ZipCode1, ZipCode2, Juso,
            StartDate, LastDate, JiCode, Tel, EmbPrt, Bi, Pname, Gwange, Kiho,
            GKiho, DeptCode, DrCode, GbSpc, GbGameK, Jinilsu, JinAmt, TuyakGwa,
            TuyakMonth, TuyakJulDate, TuyakIlsu, Bohun, Religion, Remark, Sabun,
            Birth, GbBirth, Email, HPhone, Jikup, GbJuger, RoadDetail, BuildNo,
            ZipCode3, BiDate, Rowid
        }

        /// <summary>
        /// ADMIN.BAS_MIH
        /// </summary>
        public enum enmBasMih
        {
            Pano, Bi, TransDate, Pname, Gwange, Kiho, Gkiho, Rowid
        }

        /// <summary>
        /// ADMIN.IPD_NEW_SLIP
        /// </summary>
        public enum enmIpdNewSlip
        {
            IPDNO, TRSNO, ACTDATE, PANO, BI, BDATE, ENTDATE, SUNEXT, BUN, NU, QTY, NAL, BASEAMT, GBSPC,
            GBNGT, GBGISUL, GBSELF, GBCHILD, DEPTCODE, DRCODE, WARDCODE, SUCODE, GBSLIP, GBHOST, PART,
            AMT1, AMT2, SEQNO, YYMM, DRGSELF, ORDERNO, ABCDATE,
            OPER_DEPT, OPER_DCT, ORDER_DEPT, ORDER_DCT, EXAM_WRTNO, RoomCode, DIV, GBSELNOT, GBSUGBS, GBER,
            GBSGADD, CBUN, CSUNEXT, CSUCODE, GBSUGBAB, GBSUGBAC, GBSUGBAD, BCODE, OPGUBUN, HIGHRISK, GBOP,
            GBNGT2,POWDER , ASADD
        }

        /// <summary>
        /// ADMIN.OPD_SLIP
        /// </summary>
        public enum enmOpdSlip
        {
            ActDate, Pano, Bi, BDate, EntDate, SuNext, Bun, Nu, Qty,
            Nal, BaseAmt, GbSpc, GbNgt, GbGisul, GbSelf, GbChild, DeptCode, DrCode,
            WardCode, SuCode, GbSlip, GbHost, Part, Amt1, Amt2, SeqNo, OrderNo,
            GbImiv, YYMM, GbBunup, DosCode, CardSeqNo, DIV, DUR, KSJIN, OgAmt, GBSUGBS, GBER
        }
        
        #region <summary> Misu 관리 Spread Column </summary>
        /// <summary>
        /// ADMIN.MISU_IDMST
        /// </summary>
        public enum enmMisuIdMst
        {
            WRTNO, MISUID, BDATE, CLASS, IPDOPD, BI, GELCODE, BUN, FROMDATE, TODATE, ILSU,
            DEPTCODE, MGRRANK, QTY1, QTY2, QTY3, QTY4, AMT1, AMT2, AMT3, AMT4, AMT5, AMT6, AMT7, GBEND, REMARK,
            JEPSUNO, ENTDATE, ENTPART, MIRYYMM, CHASU, MUKNO, TONGGBN, DRCODE, TDATE, JDATE, CARNO,
            DRIVER, COPNAME, AMT8, GUBUN, EDIMIRNO
        }

        /// <summary>
        /// ADMIN.MISU_SLIP
        /// </summary>
        public enum enmMisuSlip
        {
            WRTNO, MISUID, BDATE, GELCODE, IPDOPD, CLASS, GUBUN, QTY, TAMT, AMT,
            REMARK, ENTDATE, ENTPART, CHASU
        }

        public enum enmPmpaMisu
        {
            chk01, ActDate, BalAmt, IpGumAmt, Buse,
            Remark, JobSabun, GBIO, Dept, MisuGbn,
            TotAmt, InDate, OutDate, CardSeqno, JobName,
            JengSang, Posco
        }

        /// <summary> Misu 관리 메인 컬럼헤드 배열 </summary>
        public string[] sSpdPmpaMisu = {
            "S",            "발생일자",    "발생액",    "입금액",       "부서",
            "적요",         "작업사번",    "I/O",       "진료과",       "미수구분",
            "총진료비",     "입원일자",    "퇴원일자",  "카드일련번호", "작업자성명",
            "연말정산제외", "포스코번호"
        };

        /// <summary> Misu 관리 메인 컬럼사이즈 배열 </summary>
        public int[] nSpdPmpaMisu = {
            28,  80, 85, 85, 60,
            210, 48, 25, 50, 60,
            85,  80, 80, 90, 48,
            60,  50
        };
        #endregion

        #region <summary> Bas_Add    Spread Column </summary>
        public enum enmPmpaAdd
        {
            chk01, PCODE, GBCHILD, NIGHT, MIDNIGHT, GBER, HOLIDAY,
            ADD1, ADD2, ADD3, ADD4, ADD5, ADD6, ADD7, ADD8, ADD9,
            SDATE, EDATE, DELDATE, ENTDATE, ENTSABUN, Change,
            ROWID
        }

        public string[] sSpdPmpaAdd = {
            "삭제",     "산정코드", "나이구분",  "야간",  "심야",    "응급",     "공휴",
            "가산1",    "가산2",    "가산3",     "가산4",     "가산5",    "가산6", "가산7","가산8","가산9",
            "적용일자", "종료일자", "삭제일자",  "입력일자",  "입력사번", "변경",
            "ROWID"
        };

        public int[] nSpdPmpaAdd = {
            44, 60, 160, 44, 44, 44, 44,
            44, 44, 44,  44, 44, 44, 44, 44, 44,
            80, 80, 80,  90, 80, 44,
            80
        };
        #endregion

        #region <summary> Bas_Add_AN    Spread Column </summary>
        public string[] sSpdPmpaAdd_AN = {
            "삭제",     "산정코드",   "나이구분",           "야간",  "심야",   "응급",     "공휴",
            "개두마취", "일측폐환기", "개흉적심장수술마취", "가산4",    "가산5",    "가산6", "가산7","가산8","ASA",
            "적용일자", "종료일자",   "삭제일자",           "입력일자", "입력사번", "변경",
            "ROWID"
        };

        public int[] nSpdPmpaAdd_AN = {
            44, 60, 160, 44, 44, 44, 44,
            60, 68, 68,  44, 44, 44, 44, 44, 44,
            80, 80, 80,  90, 80, 44,
            80
        };
        #endregion

        #region <summary> Bas_Add_Drug    Spread Column </summary>
        public string[] sSpdPmpaAdd_Drug = {
            "삭제",     "산정코드", "나이구분",   "야간",  "심야",    "응급",     "공휴",
            "제재료",   "가산2",    "가산3",      "가산4",     "가산5",    "가산6", "가산7","가산8","가산9",
            "적용일자", "종료일자", "삭제일자",   "입력일자",  "입력사번", "변경",
            "ROWID"
        };

        public int[] nSpdPmpaAdd_Drug = {
            44, 60, 160, 44, 44, 44, 44,
            60, 44, 44,  44, 44, 44, 44, 44,44,
            80, 80, 80,  90, 80, 44,
            80
        };
        #endregion

        #region <summary> Bas_Add_Exam    Spread Column </summary>
        public string[] sSpdPmpaAdd_Exam = {
            "삭제",       "산정코드",       "나이구분", "야간",   "심야", "응급",                   "공휴",
            "외과전문의", "흉부외과전문의", "치료목적", "내시경하생검", "진단검사의학전문의판독", "가산6", "가산7","가산8","가산9",
            "적용일자",   "종료일자",       "삭제일자", "입력일자",     "입력사번",               "변경",
            "ROWID"
        };

        public int[] nSpdPmpaAdd_Exam = {
            44, 60, 160, 44, 44, 44, 44,
            60, 60, 60,  60, 64, 44, 44,44,44,
            80, 80, 80,  90, 80, 44,
            80
        };
        #endregion

        #region <summary> Bas_Add_Jin    Spread Column </summary>              
        public string[] sSpdPmpaAdd_Jin = {
            "삭제",     "산정코드", "나이구분",  "야간",  "심야",    "응급",     "공휴",
            "가산1",    "가산2",    "가산3",     "가산4",     "가산5",    "가산6", "가산7","가산8","가산9",
            "적용일자", "종료일자", "삭제일자",  "입력일자",  "입력사번", "변경",
            "ROWID"
        };

        public int[] nSpdPmpaAdd_Jin = {
            44, 60, 160, 44, 44, 44, 44,
            44, 44, 44,  44, 44, 44, 44, 44, 44,
            80, 80, 80,  90, 80, 44,
            80
        };
        #endregion

        #region <summary> Bas_Add_Jusa    Spread Column </summary>
        public string[] sSpdPmpaAdd_Jusa = {
            "삭제",     "산정코드", "나이구분",  "야간",  "심야",    "응급",     "공휴",
            "가산1",    "가산2",    "가산3",     "가산4",     "가산5",    "가산6", "가산7","가산8","가산9",
            "적용일자", "종료일자", "삭제일자",  "입력일자",  "입력사번", "변경",
            "ROWID"
        };

        public int[] nSpdPmpaAdd_Jusa = {
            44, 60, 160, 44, 44, 44, 44,
            44, 44, 44,  44, 44, 44, 44, 44, 44,
            80, 80, 80,  90, 80, 44,
            80
        };
        #endregion

        #region <summary> Bas_Add_OP    Spread Column </summary>
        public string[] sSpdPmpaAdd_OP = {
            "삭제",     "산정코드", "나이구분",  "야간",  "심야",    "응급",     "공휴",
            "외과",     "화상",     "흉부외과",  "제2수술",   "부수술",   "고위험산모", "분만","신경외과","ASA",
            "적용일자", "종료일자", "삭제일자",  "입력일자",  "입력사번", "변경",
            "ROWID"
        };

        public int[] nSpdPmpaAdd_OP = {
            44, 60, 160, 44, 44, 44, 44,
            44, 44, 44,  44, 44, 44, 44,44,44,
            80, 80, 80,  90, 80, 44,
            80
        };
        #endregion

        #region <summary> Bas_Add_Xray    Spread Column </summary>
        public string[] sSpdPmpaAdd_Xray = {
            "삭제",     "산정코드", "나이구분",  "야간",   "심야",   "응급",     "공휴",
            "판독",     "가산2",    "가산3",     "가산4",     "가산5",    "가산6", "가산7", "가산8", "가산9",
            "적용일자", "종료일자", "삭제일자",  "입력일자",  "입력사번", "변경",
            "ROWID"
        };

        public int[] nSpdPmpaAdd_Xray = {
            44, 60, 160, 44, 44, 44, 44,
            44, 44, 44,  44, 44, 44, 44,44,44,
            80, 80, 80,  90, 80, 44,
            80
        };
        #endregion

        #region <summary> Bas_Account_Bon 관리 Spread Column </summary>
        /// <summary>
        /// ADMIN.BAS_ACCOUNT_BON
        /// </summary>
        public enum enmBasAcctBon
        {// 1           2           3           4           5           6
            chk01,      WRTNO,      SDATE,      GBIO,       BI,         DEPT,
            MCODE,      MCODE_NAME, GBCHILD,    CHILDNAME,  VCODE,      VCODE_NAME,
            HC,         FCODE,      JIN,        BOHUM,      CTMRI,      FOOD,
            DT1,        DT2,        FAMT1,      FAMT2,      EDATE,      ENTDATE,
            ENTSABUN,   DELDATE,    ROWID
        }

        /// <summary> Bas_Account_Bon 관리 메인 컬럼헤드 배열 </summary>
        public string[] sSpdBasAcctBon = {
         // 1           2           3           4           5           6
            "삭제",     "관리번호", "적용일자", "입원외래", "환자종류", "진료과목",
            "자격코드", "자격명칭", "나이구분", "나이명칭", "중증희귀", "질환명",
            "면제코드", "특정기호", "진찰료",   "진료비",   "CT/MRI",   "식대료",
            "틀니",     "임플란트", "정액1",    "정액2",    "종료일자", "최종입력일자",
            "작업자",   "삭제일자", "ROWID"
        };

        /// <summary> Bas_Account_Bon 관리 메인 컬럼사이즈 배열 </summary>
        public int[] nSpdBasAcctBon = {
         // 1           2           3           4           5           6
            30,         52,         80,         30,         48,         48,
            48,         120,        44,         88,         48,         120,
            120,        80,         44,         44,         44,         44,
            44,         44,         54,         54,         80,         90,
            60,         80,         44
        };
        #endregion

        #region <summary> 진료비 조회 폼 관리 Spread Column </summary>
        public enum enmRcptTrsList
        {// 1           2           3           4           5           6
            GBSTS,      PANO,       SNAME,      INDATE,     OUTDATE,    ILSU,       WARD,
            ROOM,       BI,         DEPT,       BOHUN,      DRCODE,     GBIPD,
            SANG,       OGPDBUN,    VCODE,      JSIM,       ACTDATE,    IPDNO,
            TRSNO,      GBSPC,      OGPDBUNDTL, DRGCODE,    DRG,        FCODE,
            SECRET,     TEMP,       ROWID
        }

        /// <summary> 진료비 조회 폼 관리 메인 컬럼헤드 배열 </summary>
        public string[] sSpdRcptTrsList = {
         // 1           2           3           4           5           6
            "입원상태", "등록번호", "환자성명", "입원일자", "퇴원일자", "일수",     "병동",
            "호실",     "자격",     "진료과",   "보훈장애", "진료의사", "지병",
            "상한",     "면제",     "중증",     "심사",     "퇴원처리일","IPDNO",
            "TRSNO",    "선택",     "소아",     "DRG코드",  "DRG",      "특정기호",
            "사생활보호", "임시",   "ROWID"
        };

        /// <summary> Bas_Account_Bon 관리 메인 컬럼사이즈 배열 </summary>
        public int[] nSpdRcptTrsList = {
         // 1           2           3           4           5           6
            64,         74,         74,         68,         68,         34,         34,
            34,         30,         44,         34,         58,         44, 
            30,         40,         40,         40,         68,         70,
            72,         30,         80,         84,         34,         68,
            68,         30,         80
        };
        #endregion

        #region <summary> Card_Approv    Spread Column </summary>
        public enum enmCardApprov
        {
            TRANDATE, TRANHEADER, TRADEAMT, DEPTCODE, GBIO, FINAME, CARDNO, ORIGINNO, ORIGINNO2, PTGUBUN, SEL, HPAY, IC, ROWID, DIV, INPUTMETHOD
        }

        public string[] sSpdCardApprov = {
            "승인일자", "거래구분", "승인금액", "진료과", "I/O", "발급사정보", "현금영수번호", "승인번호", "원승인번호", "카드사", "자진발급", "HPay", "IC거래", "ROWID", "할부","거래구분"
        };

        public int[] nSpdCardApprov = {
            72, 60, 72, 44, 34, 94, 84, 64, 64, 64, 40, 40, 40, 44, 40 ,40
        };
        #endregion

        #region 환자구분 변경 관련
        public enum enmIpdTrsChg
        {
            chk01,      TRSNO,      INDATE,     BI,         DEPT,       DRCODE,     DRNAME,
            GBIPD,      GKIHO,      KIHO,       BONRATE,    GISULRATE,  OUTDATE,
            BOHUN,      PNAME,      GWANGE,     GBGAMEK,    RDATE,      GBSTS,
            GELCODE,    GBILBAN2,   GBDRG 
        }

        public string[] sSpdIpdTrsChg = {
            "선택",     "자격번호", "입원일자", "종류",     "진료과",   "의사코드",    "진료의사",
            "지병",     "증기호",   "조합기호", "본인부담율", "기술가산율", "퇴원일자",
            "보훈",     "피보험자", "관계",     "감액구분", "최초퇴원처리일", "입원상태",
            "계약처코드", "일반2배", "DRG"
        };

        public int[] nSpdTrsChg = {
            34,         68,         72,         40,         44,             52,         44,
            44,         72,         68,         44,         44,             72,
            44,         48,         44,         44,         72,             48,
            44,         40,         40
        };
        #endregion

        #region 원무팀용 퇴원예고자 명단 Spread
        public enum     enmROutList     { ROutDate,     ROUTENTTIME, PANO,       SNAME,  BiName, Age,    Sex,     InRoom,     OutRoom,   InDept,    DRNAME,     OutDate,  InDate,   Last }
        public string[] sSpdROutList =  { "퇴원예고일", "등록일시",  "등록번호", "성명", "보험", "나이", "성별", "입원호실", "퇴원호실", "진료과",  "진료의사", "퇴원일", "입원일", "지난Data"};
        public int[]    nSpdROutList =  { 72,           92,          60,         64,     40,     38,     38,      38,         38,          38,        46,         72,       72,       34};
        #endregion

        #region 김해수 미수원장관리 Combo설정
        public cMisuMst INITIAL_SET(PsmhDb pDbCon, string argJob)
        {
            //clsPmpaPb cMisuMst = new clsPmpaPb();
            cMisuMst cMisuMst = new cMisuMst();

            cMisuMst.GstrMisuIO[0] = "O.외래";
            cMisuMst.GstrMisuIO[1] = "I.입원";

            cMisuMst.GstrMisuMgr[0] = "1.완불가능";
            cMisuMst.GstrMisuMgr[1] = "2.독려시가능";
            cMisuMst.GstrMisuMgr[2] = "3.대손처리예상";
            cMisuMst.GstrMisuMgr[3] = "4.재판중";
            cMisuMst.GstrMisuMgr[4] = "5.재산압류";
            cMisuMst.GstrMisuMgr[5] = "6.기타";

            cMisuMst.GstrMisuSayu[0] = "01.가퇴원";
            cMisuMst.GstrMisuSayu[1] = "02.업무착오";
            cMisuMst.GstrMisuSayu[2] = "03.탈원";
            cMisuMst.GstrMisuSayu[3] = "04.지불각서";
            cMisuMst.GstrMisuSayu[4] = "05.응급실";
            cMisuMst.GstrMisuSayu[5] = "06.외래";
            cMisuMst.GstrMisuSayu[6] = "07.심사청구";
            cMisuMst.GstrMisuSayu[7] = "10.기타";

            if (argJob == "")
            {
                cMisuMst.GstrMisuBun = new string[23];

                cMisuMst.GstrMisuClass[0] = "01.공단";
                cMisuMst.GstrMisuClass[1] = "02.직장";
                cMisuMst.GstrMisuClass[2] = "03.지역";
                cMisuMst.GstrMisuClass[3] = "04.의료급여";
                cMisuMst.GstrMisuClass[4] = "05.산재";
                cMisuMst.GstrMisuClass[5] = "07.자보";
                cMisuMst.GstrMisuClass[6] = "08.개인";
                cMisuMst.GstrMisuClass[7] = "09.혈액";
                cMisuMst.GstrMisuClass[8] = "10.계약처";
                cMisuMst.GstrMisuClass[9] = "11.보훈청";
                cMisuMst.GstrMisuClass[10] = "12.시각장애";
                cMisuMst.GstrMisuClass[11] = "13.심신장애";
                cMisuMst.GstrMisuClass[12] = "14.보장구";
                cMisuMst.GstrMisuClass[13] = "15.직원대납";
                cMisuMst.GstrMisuClass[14] = "16.장기요양";
                cMisuMst.GstrMisuClass[15] = "17.방문간호";
                cMisuMst.GstrMisuClass[16] = "18.치매검사";

                cMisuMst.GstrMisuBun[0] = "01.내과분야";
                cMisuMst.GstrMisuBun[1] = "02.외과분야";
                cMisuMst.GstrMisuBun[2] = "03.산소아과";
                cMisuMst.GstrMisuBun[3] = "04.안.이비";
                cMisuMst.GstrMisuBun[4] = "05.피.비뇨";
                cMisuMst.GstrMisuBun[5] = "06.치과";
                cMisuMst.GstrMisuBun[6] = "07.NP정액";
                cMisuMst.GstrMisuBun[7] = "08.장애대불";
                cMisuMst.GstrMisuBun[8] = "09.가정간호";
                cMisuMst.GstrMisuBun[9] = "10.재청구";
                cMisuMst.GstrMisuBun[10] = "11.이의신청";
                cMisuMst.GstrMisuBun[11] = "12.정산진료비";
                cMisuMst.GstrMisuBun[12] = "13.추가청구";
                cMisuMst.GstrMisuBun[13] = "14.NP장애대불";
                cMisuMst.GstrMisuBun[14] = "15.HD정액";
                cMisuMst.GstrMisuBun[15] = "16.HU호스피스";
                cMisuMst.GstrMisuBun[16] = "19.기타청구";
                cMisuMst.GstrMisuBun[17] = "20.상한대불";
                cMisuMst.GstrMisuBun[18] = "21.희귀지원금";
                cMisuMst.GstrMisuBun[19] = "22.결핵지원금";
                cMisuMst.GstrMisuBun[20] = "23.DRG(포괄수가)";
                cMisuMst.GstrMisuBun[21] = "24.100/100 미만";
                cMisuMst.GstrMisuBun[22] = "25.국가재난지원";

                cMisuMst.GstrMisuGye[0] = "11.처음청구";
                cMisuMst.GstrMisuGye[1] = "12.정산청구";
                cMisuMst.GstrMisuGye[2] = "13.재청구";
                cMisuMst.GstrMisuGye[3] = "14.추가청구";
                cMisuMst.GstrMisuGye[4] = "15.이의신청";
                cMisuMst.GstrMisuGye[5] = "19.기타미수";
                cMisuMst.GstrMisuGye[6] = "21.입금";
                cMisuMst.GstrMisuGye[7] = "22.정산입금";
                cMisuMst.GstrMisuGye[8] = "23.주민입금";
                cMisuMst.GstrMisuGye[9] = "24.이의입금";
                cMisuMst.GstrMisuGye[10] = "25.기타수입";
                cMisuMst.GstrMisuGye[11] = "26.심사중입금";
                cMisuMst.GstrMisuGye[12] = "31.삭감";
                cMisuMst.GstrMisuGye[13] = "32.반송";
                cMisuMst.GstrMisuGye[14] = "33.과지급금";
                cMisuMst.GstrMisuGye[15] = "34.계산착오";
                cMisuMst.GstrMisuGye[16] = "35.삭감절산";
                cMisuMst.GstrMisuGye[17] = "**.참고사항";

            }
            else if (argJob == "TA")
            {
                cMisuMst.GstrMisuBun = new string[4];

                cMisuMst.GstrMisuClass[0] = "01.공단";
                cMisuMst.GstrMisuClass[1] = "02.직장";
                cMisuMst.GstrMisuClass[2] = "03.지역";
                cMisuMst.GstrMisuClass[3] = "04.보호";
                cMisuMst.GstrMisuClass[4] = "05.산재";
                cMisuMst.GstrMisuClass[5] = "07.자보";
                cMisuMst.GstrMisuClass[6] = "08.계약처";
                cMisuMst.GstrMisuClass[7] = "09.혈액";
                cMisuMst.GstrMisuClass[8] = "10.계약처";
                cMisuMst.GstrMisuClass[9] = "11.보훈청";
                cMisuMst.GstrMisuClass[10] = "12.시각장애";
                cMisuMst.GstrMisuClass[11] = "13.심신장애";
                cMisuMst.GstrMisuClass[12] = "14.보장구";
                cMisuMst.GstrMisuClass[13] = "15.직원대납";
                cMisuMst.GstrMisuClass[14] = "16.장기요양";
                cMisuMst.GstrMisuClass[15] = "17.방문간호";
                cMisuMst.GstrMisuClass[16] = "18.치매검사";

                cMisuMst.GstrMisuBun[0] = "01.처음창구";
                cMisuMst.GstrMisuBun[1] = "10.재청구";
                cMisuMst.GstrMisuBun[2] = "11.이의신청";
                cMisuMst.GstrMisuBun[3] = "19.기타청구";

                cMisuMst.GstrMisuGye[0] = "11.청구미수";
                cMisuMst.GstrMisuGye[1] = "14.이의신청";
                cMisuMst.GstrMisuGye[2] = "15.재청구";
                cMisuMst.GstrMisuGye[3] = "16.추가청구";
                cMisuMst.GstrMisuGye[4] = "19.기타미수";
                cMisuMst.GstrMisuGye[5] = "21.입금";
                cMisuMst.GstrMisuGye[6] = "25.기타수입";
                cMisuMst.GstrMisuGye[7] = "31.삭감";
                cMisuMst.GstrMisuGye[8] = "32.반송";

                //200912 추가
                cMisuMst.GstrMisuGye[9] = "1.1참검토";
                cMisuMst.GstrMisuGye[10] = "2.분쟁금액";
                cMisuMst.GstrMisuGye[11] = "3.분쟁결과";
            }

            for (int i = 0; i < 30; i++)
            {
                cMisuMst.GstrDept[i] = " ";
            }

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT DEPTCODE, DEPTNAMEK";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
                SQL += ComNum.VBLF + "WHERE PRINTRANKING < 31";
                SQL += ComNum.VBLF + "ORDER BY PrintRanking";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                    
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");                   
                }
                if(dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cMisuMst.GstrDept[i] = dt.Rows[i]["DeptCode"].ToString().Trim() + "." + dt.Rows[i]["DeptNameK"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            
            dt.Dispose();
            dt = null;

            return cMisuMst;
        }
        #endregion

    }
}
