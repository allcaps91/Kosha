using System;
using System.Windows.Forms;


namespace ComBase
{
    /// <summary>
    /// 글로발 변수 모음
    /// </summary>
    public class clsPublic
    {
        #region //신규
        public static int AutoLogOutTime = 30; //박웅규 >> 자동 로그아웃 시간
        public static bool AutoLogOutExcept = false; //박웅규 >> 프로세스 시작시에 true , 종료시에 false
        public static double PSMHVERSION = 1; //박웅규 >> PSMH 프로그램 버젼 (강제 업데이트가 필요할 경우 사용) : GUNUM1 
        public static string UserLogRemark = ""; //박웅규 >> 사용자 프로그램 접근로그
        #endregion

        public string strSysDate; // ComFunc 에서 가져옴
        public string strSysTime; // ComFunc 에서 가져옴

        #region //AdoConst(adoODBC.bas)
        public static string GstrPtno; //GstrPANO


        #region //사용자관련
        public static long GnJobSabun = 0;  //public string  GnJobSabun           As Long //=> Type문으로 : clsType.User.IdNumber
        public static string GstrJobSabun = ""; //public string  GstrJobPart          = "" ; //=> 변수출처불명(IdNumber) //=>Type문으로 : clsType.User.IdNumber
        public static string FstrPassName = "";    //public string  FstrPassName         = "" ; //=> Type문으로 : clsType.User.PassName 
        public static string GstrJiyek = ""; //public string  GstrJiyek            = "" ; //=> Type문으로 : clsType.User.Jiyek
        public static string GstrJobName = ""; //public string  GstrJobName          = "" ; //=> Type문으로 : clsType.User.JobName
        public static string GstrJobPart = ""; //public string  GstrJobPart          = "" ; //=> Type문으로 : clsType.User.JobPart
        public static string FstrPassPart = "";    //public string  FstrPassPart         = "" ; //=> Type문으로 : clsType.User.JobPart
        public static string GstrJobGrade = "";    //public string  GstrJobGrade         = "" ;     //=> Type문으로 : clsType.User.Grade
        public static string FstrGRADE = "";   //public string  FstrGRADE            = "" ; //=> Type문으로 : clsType.User.Grade
        public static string FstrPassGrade = "";   //public string  FstrPassGrade        = "" ; //=> Type문으로 : clsType.User.Grade
        public static string GstrPasswordChar = "";    //Public GstrPasswordChar     = "" ; //=> Type문으로 : clsType.User.PasswordChar
        public static string FstrPassWordChar = "";    //public string  FstrPassWordChar     = "" ; //=> Type문으로 : clsType.User.PasswordChar
        public static string FstrPassId = "";  //public string  FstrPassId           = "" ; //=> Type문으로 : clsType.User.PassId
        public static string GstrPassID = "";  //public string  GstrPassID           = "" ; //=> Type문으로 : clsType.User.PassId
        public static string GstrPassBuse = "";    //public string  GstrPassBuse         = "" ; //=> Type문으로 : clsType.User.BuseCode
        public static string GstrBuseCode = "";    //public string  GstrBuseCode         = "" ; //=> Type문으로 : clsType.User.BuseCode
        public static string GstrSabun = "";   //public string  GstrSabun            = "" ; //=> Type문으로 : clsType.User.Sabun
        public static string GstrPassWord = "";    //public string  GstrPassWord         = "" ; //=> Type문으로 :clsType.User. PassWord
        public static string FstrPassWord = "";    //public string  FstrPassWord         = "" ; //=> Type문으로 : clsType.User.PassWord
        public static string GstrPasshash = "";    //public string  GstrPasshash         = "" ; //=> Type문으로 : clsType.User.Passhash
        public static string GstrPasshash256 = ""; //public string  GstrPasshash256      = "" ; //=> Type문으로 : clsType.User.Passhash256
        public static string GstrPassJdate = "";   //public string  GstrPassJdate        = "" ; //=> Type문으로 : clsType.User.PassJdate 
        public static string GstrPass6M = "";  //public string  GstrPass6M           = "" ; //=> Type문으로 : clsType.User.Pass6M 
        public static string FstrPassCharge = "";  //public string  FstrPassCharge       = "" ;//=> Type문으로 : clsType.User.PassCharge 
        public static int FnPassCount = 0; //public string  FnPassCount          = 0 ; '비밀번호 오류횟수 //=> Type문으로 : clsType.User.PassCount 
        public static int FnSabunCnt = 0;  //public string  FnSabunCnt           = 0 ; '사번 오류 횟수 //=> Type문으로 : clsType.User.SabunCnt 
        public static int GnLogOutCNT = 0; //public string  GnLogOutCNT        = 0 ;  '로그아웃 Cnt //=> Type문으로 : clsType.User.LogOutCnt 
        public static string GstrJobMan = "";  //public string  GstrJobMan           = "" ;   //=> Type문으로 : clsType.User.JobMan 
        public static string GstrDeptCode = "";    //public string  GstrDeptCode         = "";  //=> Type문으로 : clsType.User.DeptCode 
        #endregion //사용자관련

        public static string GstrURL = ""; //public string  GstrURL              = "" ; '삭제 하지 마시오.
        public static string GstrPassProgramID = "";   //public string  GstrPassProgramID    = "" ; * 8

        public static string GstrRetValue = "";    //public string  GstrRetValue         = "" ;   '찾기 Help화면의 Return Value값
        public static string GstrRetName = ""; //public string  GstrRetName          = "" ;   '찾기 Help화면의 Return Value값
        public static string GstrPANO = "";    //public string  GstrPANO             = "" ;
        public static string GstrDate = "";    //public string  GstrDate             = "" ;
        public static string GstrTime = "";    ////public string  GstrTime             = "" ;

        public static string GstrDr = "";  //public string  GstrDr               = "" ;
        public static string GstrName = "";    //public string  GstrName             = "" ;
        public static string GstrAge = ""; //public string  GstrAge              = "" ;
        public static string GstrSunext_B = "";    //public string  GstrSunext_B         = "" ;
        public static string GstrHelpCode = "";    //public string  GstrHelpCode         = "" ;       'HELP에서 찾은 코드
        public static string GstrHelpName = "";    //public string  GstrHelpName         = "" ;       'HELP에서 찾은 명칭
        public static string GstrChoicePano = "";  //public string  GstrChoicePano       = "" ;       '환자정보에서 찾은 등록번호
        public static string GstrLastSS = "";  //public string  GstrLastSS           = "" ;
        public static string GstrOSVER = "";   //public string  GstrOSVER            = "" ;       'os version
        public static string GstrROWID = "";   //public string  GstrROWID            = "" ;      'JJY 변경시 알려주삼 (개별 bas에서 선언되어 프로램에서 오류 발생하고 있음, 공용으로 사용함) -변경불가
        public static string GstrMCROWID = ""; //public string  GstrMCROWID          = "" ;      '진료회송서용 Rowid
        public static string GstrCDSendChk = "";   //public string  GstrCDSendChk        = "" ;      'cd전송체크
        public static string GstrMDBAdoMsg = "";   //public string  GstrMDBAdoMsg        = "" ;      '종검 인바디 DB 접속 상태 메세지
        public static string GstrMyAdoMsg = "";    //public string  GstrMyAdoMsg         = "" ;       '검진 BP 접속
        public static string GstrMsgPrompt = "";   //Public GstrMsgPrompt        = "" ;

        public static string GstrMsgPromot = "";   //Public GstrMsgPromot        = "" ;
        public static string GstrSpecialText = ""; //public string  GstrSpecialText      = "" ;  '특수문자 처리 하도록 공용변수로 선언, 각 개별 bas에 선언된것을 주석처리바람
        public static string GstrIpAddress = "";   //public string  GstrIpAddress        = "" ;  'lock 처리시 ipaddress 읽도록 처리
        public static string GstrWardCode = "";    //public string  GstrWardCode             = "" ;
        public static string GstrWardCodes = "";   //public string  GstrWardCodes           = "" ;
        public static string GstrICUWard = ""; //public string  GstrICUWard              = "" ;
        public static string GstrVer = ""; //public string  GstrVer            = "" ;  '프로그램 버전
        public static string GstrIP = "";  //public string  GstrIP             = "" ;  '프로그램 사용 ip
        public static string GstrAES_PASS = "";    //public string  GstrAES_PASS       = "" ;
        public static string GstrSILLCodeD = "";   //public string  GstrSILLCodeD       = "" ;  '상병코드
        public static string GstrSILLNameK = "";   //public string  GstrSILLNameK       = "" ;  '상병명칭
        public static string GstrSILLNameE = "";   //public string  GstrSILLNameE       = "" ;  '상병명칭
        public static string GstrSILLCode = "";    //public string  GstrSILLCode        = "" ;  '상병 전산코드
        public static string GstrSMS114_SEND = ""; //public string  GstrSMS114_SEND     = "" ;  '문자114 SMS 공용변수(받을사람{}보낸사람{}제목{}문자메세지
        public static string GstrSMS114_Result = "";   //public string  GstrSMS114_Result   = "" ;  '문자114 SMS 결과값(0000.정상전송 기타는 오류)
        public static string GstrToRemark = "";    //public string  GstrToRemark       = "" ;
        public static string GstrJepCode_Bar = ""; //public string  GstrJepCode_Bar    = "" ;   '바코드인쇄(검사시약)

        public static string GstrPRN_ROWID = "";   //public string  GstrPRN_ROWID      = "" ;   'PRN참고

        public static string GstrMsgList = "";
        public static string GstrMsgTitle = "";
        public static MessageBoxButtons GMsgButtons;
        public static MessageBoxIcon GMsgIcon;
        public static DialogResult DiResult;

        public static int GnMsgReturn;
        public static string GstrViewETC_OK;       //As String   '2015-10-07  기타결과 열람관련추가
        public static string GstrViewETC_Temp;     //As String

        //2020-05-15 ABC원가 - ERP_Accounting 에서 사용 - 정보세팅
        public static long GnABC_PERIOD_ID = 0;                  //PERIOD_ID
        public static DateTime GdtABC_SDate = DateTime.Parse("9999-12-31");       //해당월 시작일자
        public static string GstrABC_PERIOD_NAME = string.Empty;

        #endregion //AdoConst(adoODBC.bas)

        #region //BasAnnounce(announ32.bas)
        //public string GstrSabun = "";   //public string  GstrSabun                = "" ; //AdoConst(adoODBC.bas) 중복됨
        public static string GstrSabunName = "";   //public string  GstrSabunName            = "" ;
        public static string GnMgrNo = "";   //public string  GnMgrNo                  As Long

        public static string GnAnnounceGetCount = "";   //public string  GnAnnounceGetCount       = 0 ;

        public static string[] GnaAnnounceMgrNOs = null;   //public string  GnaAnnounceMgrNOs()      As Long
        public static string[] GnaAnnouncePerson = null;   //public string  GnaAnnouncePerson()      As Long
        public static string[] GsaAnnounceMemos = null;   //public string  GsaAnnounceMemos()       = "" ;
        public static string[] GsaAnnounceDateTime = null;   //public string  GsaAnnounceDateTime()    = "" ;
        public static string[] GsaAnnounceGroup = null;   //public string  GsaAnnounceGroup()       = "" ;

        public static string GstrPassClass = "";   //public string  GstrPassClass            = "" ;
        public static string GstrPassDept = "";   //public string  GstrPassDept             = "" ;
        public static string GstrPassIdnumber = "";   //public string  GstrPassIdnumber         = "" ;

        #endregion //BasAnnounce(announ32.bas)

        #region //G_Variable(G_Variable.bas)
        public static string gPatientNo = "";   //Public gPatientNo 	= "" ;
        public static string gName = "";    //Public gName 		= "" ;
        public static string gPersonNo1 = "";   //Public gPersonNo1 	= "" ;
        public static string gPersonNo2 = "";   //Public gPersonNo2 	= "" ;
        public static string gAddr1 = "";   //Public gAddr1 		= "" ;
        public static string gAddr2 = "";   //Public gAddr2 		= "" ;
        public static string gTel = "";     //Public gTel 		= "" ;
        public static string gDept = "";    //Public gDept 		= "" ;
        public static string gDoctor = "";  //Public gDoctor 		= "" ;
        public static string gAppl = "";    //Public gAppl 		= "" ;
        public static string gRelation = "";    //Public gRelation 	= "" ;
        public static string chkImgflag1 = "";  //Public chkImgflag1 	As Boolean
        public static string chkImgflag2 = "";  //Public chkImgflag2 	As Boolean
        public static string chkflag1 = "";     //Public chkflag1 	As Boolean
        public static string chkflag2 = "";     //Public chkflag2 	As Boolean
        public static string chkflag3 = "";     //Public chkflag3 	As Boolean
        public static string chkflag4 = "";     //Public chkflag4 	As Boolean
        public static string chkflag5 = "";     //Public chkflag5 	As Boolean
        public static string chkflag6 = "";     //Public chkflag6 	As Boolean
        public static string chkflag7 = "";     //Public chkflag7 	As Boolean
        public static string chkflag8 = "";     //Public chkflag8 	As Boolean
        public static string chkflag9 = "";     //Public chkflag9 	As Boolean
        public static string chkflag10 = "";    //Public chkflag10 	As Boolean
        public static string chkflag11 = "";    //Public chkflag11 	As Boolean
        public static string chkflag12 = "";    //Public chkflag12 	As Boolean
        public static string chkflag13 = "";    //Public chkflag13 	As Boolean
        public static string chkflag14 = "";    //Public chkflag14 	As Boolean
        public static string chkflag15 = "";    //Public chkflag15 	As Boolean
        public static string chkflag16 = "";    //Public chkflag16 	As Boolean
        public static string chkflag17 = "";    //Public chkflag17 	As Boolean
        public static string chkflag18 = "";    //Public chkflag18 	As Boolean
        public static string chkflag19 = "";    //Public chkflag19 	As Boolean
        public static string chkflag25 = "";    //Public chkflag25 	As Boolean
        #endregion //G_Variable(G_Variable.bas)

        #region //(VbFunc.bas)
        public static string GstrSysDate = "";
        public static string GstrSysTime = "";
        public static string GstrSysTomorrow;
        public static string GstrActDate;
        public static string GstrBdate;//접수내역 Read시 실진료일

        public static string GstrSysDate_1;

        public static string GstrAnatDeptCode = ""; //=> Type문으로 : clsType.User.AnatDeptCode 
        public static string GstrIpdOpd = ""; //'mirsak에 정의되어있음 확인
        public static string GstrDefaultPrinter = "";
        public static string GnDefaultPrint = "";
        public static string[] GstrHosp = new string[4]; //'의료급여환자의 선택의료기관
        public static string[] GstrHosp2 = new string[4]; //'의료급여환자의 선택의료기관명

        public static string GstrBarPano = ""; //'바코드 등록번호
        public static string GstrBarDept = ""; //'바코드 진료과

        public static int GnPrtOutPrint1 = 0;
        public static int GnPrtOutPrint2 = 0;
        public static int GnPrtDefaultNo = 0; //'현재 프린터중 기본프린터 번호
        public static string GsPrtDefaultName = "";

        public static string GstrOK = "";

        //'오더창사용
        public static string GstrIpdCnt360 = ""; //'재원360이하
        public static string GstrIpdCnt390 = ""; //'재원360이하
        public static string GstrHoliday = ""; //'공휴일여부 2014-08-21
        public static string GstrTempHoliday = ""; //'임시공휴일여부 2015-09-22

        //'=============================================
        //'2011-04-27 김현욱 추가
        //'사용자 및 부서 별 사용권한용
        //'혹시 에러 나면 말씀해주세요~
        public static string GstrGRADE = "";

        #endregion

        #region //(vb_Order_all.bas)
        public static string GstrGbSelf = "";
        public static string Gstr지참약Chk = "";
        public static string Gstr구두Chk = "";   //구두사용제한
        public static string Gstr구두수동Chk = "";   //구두사용제한
        public static string Gstr구두_Temp = "";   //구두 변수
        public static string Gstr산제Chk = "";   //파우더
        public static string Gstr파우더_SuCode = "";   //파우더수가
        public static int Gn파우더_Row = 0;    //파우더 row
        public static string Gstr파우더Gubun = "";
        public static string Gstr파우더New_STS = "";
        public static string Gstr파우더STS = "";
        public static string Gstr파우더STS_PD = "";    // 2019-07-22 소아과 파우더 자동체크

        public static string GstrPRNChk = "";   //PRN 사용제한
        public static string GstrPRNChk_new = "";   //PRN new 사용제한
        public static string GstrPRN_Suga = "";
        public static string GstrPRN_New_Data = "";   //PRN new 데이타
        public static string GstrPRN_Sub_Chk = "";

        public static string Gstr간호처방STS = "";   //간호사처방시 구분자
        public static string Gstr인슐린Chk = "";
        public static string Gstr인슐린Date = "";   //인슐린 PRN 의사체크여부
        public static string Gstr인슐린PRN의사CHK = "";   //인슐린처방 단속 체크여부
        public static string Gstr인슐린단독Chk = "";
        public static string GstrTESTChk = "";
        public static string Gstr예정처방chk = "";
        public static string GstrCP처방Chk = "";
        public static string GstrNotDiv = "";
        public static string GstrASA진정Chk = "";
        public static int GnASA진정Row = 0;
        public static string Gstr자동로그아웃OCS = "";
        public static string Gstr자동로그아웃OCS_INI = "";
        public static string Gstr자동로그아웃OCS_STS = "";
        public static int Gn자동로그아웃_Second = 0;
        public static string Gstr자동로그아웃OCS_RT = "";
        public static string Gstr전자인증로밍_Chk = "";
        public static string Gstr전자인증_Rowid = "";
        public static string Gstr혈액사용예정일Chk = "";
        public static int Gn혈액사용예정일Row = 0;
        public static string Gstr혈액사용예정일Date = "";
        public static string Gstr프로그램버전체크 = "";

        //--------------------------------------------------------------
        public static string GstrSugaFind = "";

        public static string GstrOCS_OPEN_CHK = "";   //열람사유
        public static string Gstr마취신체등급 = "";
        public static string Gstr임신처방 = "";
        public static string Gstr임신차수 = "";

        public static string Gstr고위험산모구분 = "";
        public static string Gstr수술구분 = "";
        #endregion

        #region //CsInfo(CsInfo00.bas)
        public static string GstrBuseName = "";
        public static int GnDeleteFlag = 0;     //삭제가능여부(1.가능, 0.안됨)
        #endregion

        #region //의료급여승인(vb의료급여승인.bas)
        public static long GnBoninAmt = 0;
        public static string GstrMPtno = string.Empty;      //엄마등록번호
        public static string GstrMPtnoChk = string.Empty;   //엄마등록번호 사용여부
        #endregion

        #region //VbCalendar
        public static string GstrCalDate = "";
        #endregion //

        #region //Opd_FM_Resv(Opd_FM_Resv.bas)
        public static string GstrFM_Only = "";       //GstrFM_전용여부 가정의학과 전용(GstrFM_전용여부)
        public static int GnChoInWon_A = 0;  //FM과 오전초진 예약가능 인원  2013-11-27
        public static int GnJaeInWon_A = 0;  //FM과 오전재진 예약가능 인원  2013-11-27
        public static int GnChoInWon_P = 0;  //FM과 오후초진 예약가능 인원  2013-11-27
        public static int GnJaeInWon_P = 0;  //FM과 오후재진 예약가능 인원  2013-11-27
        public static int GnRInWon_Cho = 0;  //FM과 예약정원(초진)
        public static int GnRInWon_Jae = 0;  //FM과 예약정원(재진)
        public static string GFmResvValue = "";   //FM과 예약정보 전달변수
        public static string GstrZoneEmergencyStartDate;    //Gstr권역응급의료시작일(BASACCT.bas)
        #endregion //Opd_FM_Resv(Opd_FM_Resv.bas)

        //처방에서 사용
        public static string GstrMemo;

        //Tablet 실행파일 경로
        public static string GstrTabletA_FilePath = @"c:\cmc\exe\tablet_a.exe";
        public static string GstrTabletB_FilePath = @"c:\cmc\exe\tablet_b.exe";
        public static string GstrTabletC_FilePath = @"c:\cmc\exe\tablet_c.exe";
        public static string GstrTabletD_FilePath = @"c:\cmc\exe\tablet_d.exe";

        public static string GstrDemon_FilePath = @"c:\cmc\call\PrjDemon.exe";

        public static string GstrCopyPrint; //Gstr사본출력
        public static string GstrPrintChk;  //Gstr출력확인(인쇄시 확인여부)
        public static int GnNurGunTime_ADD = 0;         //간호부 근무시간 종료시 사유입력 후 사용기간 연장 분
        public static string GstrNurLogInFlag_Ward;     //간호부 근무시간 이외 사유입력 후 사용가능 Flag ("OK" -> 사용가능)
    }
}
