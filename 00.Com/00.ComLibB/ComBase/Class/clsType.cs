namespace ComBase
{
    public class clsType
    {
        #region //신규 공통

        //로그인시 사용자정보
        /// <summary>
        /// 로그인시 사용자정보
        /// </summary>
        public struct User
        {
            //OCS 관련 정보 :
            //New
            /// <summary>
            /// 사용자명   // GstrDrName(oorder)
            /// </summary>
            public static string UserName = "";    //사용자명   // GstrDrName(oorder)
            /// <summary>
            /// 사용자 부서명
            /// </summary>
            public static string BuseName = "";    //사용자 부서명 
            /// <summary>
            /// 오더권한
            /// </summary>
            public static string CanOrd = "";    //오더권한     //
            /// <summary>
            /// 의아여부
            /// </summary>
            public static string IsDoct = "";    //의아여부   //
            /// <summary>
            /// 간호사여부
            /// </summary>
            public static string IsNurse = "";    //간호사여부  //
            /// <summary>
            /// 간호조무사여부
            /// </summary>
            public static string IsCoNurse = "";    //간호조무사여부    // GstrDrName(oorder)
            //변경이력 : AdoConst(adoODBC.bas) => clsPublic
            /// <summary>
            /// 앞자리에 '0'인경우 '0'을 제거한다.
            /// </summary>
            public static string IdNumber = "";   //	GnJobSabun, GstrJobSabun
            /// <summary>
            /// 5자리 고정 5자리 이하 앞자리에 '0'채움
            /// </summary>
            public static string Sabun = "";    //	GstrSabun, GstrDrSabun(oorder)
            /// <summary>
            /// GstrJobName
            /// </summary>
            public static string JobName = "";  //	GstrJobName
            /// <summary>
            /// JobGroup
            /// </summary>
            public static string JobGroup = "";  //	JobGroup
            /// <summary>
            /// GstrJiyek
            /// </summary>
            public static string Jiyek = "";    //	GstrJiyek
            /// <summary>
            /// GstrJobPart, FstrPassPart : IdNumber로 변경됨
            /// </summary>
            public static string JobPart = "";  //	GstrJobPart, FstrPassPart : IdNumber로 변경됨
            /// <summary>
            /// GstrJobGrade, FstrPassGrade, FstrGRADE
            /// </summary>
            public static string Grade = "";    //	GstrJobGrade, FstrPassGrade, FstrGRADE
            /// <summary>
            /// GstrBuseCode, GstrPassBuse
            /// </summary>
            public static string BuseCode = ""; //	GstrBuseCode, GstrPassBuse
            /// <summary>
            /// GstrBuseCode, GstrPassBuse : 신규추가함
            /// </summary>
            public static string SilBuseCode = ""; //	GstrBuseCode, GstrPassBuse : 신규추가함
            /// <summary>
            /// 사용자 실부서명
            /// </summary>
            public static string Sil_BuseName = "";    //사용자 실부서명 
            /// <summary>
            /// GstrPasswordChar, FstrPassWordChar
            /// </summary>
            public static string PasswordChar = ""; //	GstrPasswordChar, FstrPassWordChar
            /// <summary>
            /// GstrPassID,FstrPassId
            /// </summary>
            public static string PassId = "";   //	GstrPassID,FstrPassId
            /// <summary>
            /// GstrPassWord, FstrPassWord
            /// </summary>
            public static string PassWord = ""; //	GstrPassWord, FstrPassWord
            /// <summary>
            /// GstrPasshash
            /// </summary>
            public static string Passhash = ""; //	GstrPasshash
            /// <summary>
            /// GstrPasshash256
            /// </summary>
            public static string Passhash256 = "";  //	GstrPasshash256
            /// <summary>
            /// GstrPassJdate
            /// </summary>
            public static string PassJdate = "";    //	GstrPassJdate
            /// <summary>
            /// GstrPass6M
            /// </summary>
            public static string Pass6M = "";   //	GstrPass6M
            /// <summary>
            /// FstrPassCharge
            /// </summary>
            public static string PassCharge = "";   //	FstrPassCharge
            /// <summary>
            /// FnPassCount
            /// </summary>
            public static int PassCount = 0;   //	FnPassCount
            /// <summary>
            /// FnSabunCnt
            /// </summary>
            public static int SabunCnt = 0;    //	FnSabunCnt
            /// <summary>
            /// GnLogOutCNT
            /// </summary>
            public static int LogOutCnt = 0;   //	GnLogOutCNT
            /// <summary>
            /// FstrPassName
            /// </summary>
            public static string PassName = "";	//	FstrPassName
            /// <summary>
            /// GstrJobMan
            /// </summary>
            public static string JobMan = "";	//	GstrJobMan
            /// <summary>
            /// GstrDeptCode
            /// </summary>
            public static string DeptCode = "";	//	GstrDeptCode

            //변경이력 : VbFunction(VbFunc.bas) => clsPublic
            /// <summary>
            /// GstrAnatDeptCode
            /// 변경이력 : VbFunction(VbFunc.bas) => clsPublic
            /// </summary>
            public static string AnatDeptCode = "";	//	GstrAnatDeptCode
            //변경이력 : oorder(OORDER.BAS)
            /// <summary>
            /// GstrDrCode  :OCS_DOCTOR
            /// 변경이력 : oorder(OORDER.BAS)
            /// </summary>
            public static string DrCode = "";	//	GstrDrCode  :OCS_DOCTOR
            /// <summary>
            /// GstrDeptName
            /// </summary>
            public static string DeptName = "";	//	GstrDeptName
            /// <summary>
            /// GstrJupsuDept1
            /// </summary>
            public static string JupsuDept1 = "";	//	GstrJupsuDept1
            /// <summary>
            /// GstrJupsuDept2
            /// </summary>
            public static string JupsuDept2 = "";	//	GstrJupsuDept2
            /// <summary>
            /// 예약구분(0.안함 1.10분 2.15분 3.20분 4.30분) //	GnResvGbn
            /// </summary>
            public static int ResvGbn = 0;	//예약구분(0.안함 1.10분 2.15분 3.20분 4.30분) //	GnResvGbn
            /// <summary>
            /// GnResvInwon
            /// </summary>
            public static int ResvInwon = 0;	//	GnResvInwon
            /// <summary>
            /// Gstr부가세사용과
            /// </summary>
            public static string TexDept = "";	//	Gstr부가세사용과

            //==>외래처방에서 사용
            /// <summary>
            /// Global Gstr간호사사용           As String  '2014-09-11
            /// </summary>
            public static string strNurseUse = ""; //Global Gstr간호사사용           As String  '2014-09-11
            /// <summary>
            /// Global Gstr간호사_Sabun         As String  '2014-09-11
            /// </summary>
            public static string strNurseSabun = ""; //Global Gstr간호사_Sabun         As String  '2014-09-11
            /// <summary>
            /// Global Gstr간호사_DeptCode      As String  '2014-09-11
            /// </summary>
            public static string strNurseDeptCode = ""; //Global Gstr간호사_DeptCode      As String  '2014-09-11
            /// <summary>
            /// Global Gstr간호사_DrCode        As String  '2014-09-11
            /// </summary>
            public static string strNurseDrcode = ""; //Global Gstr간호사_DrCode        As String  '2014-09-11
            /// <summary>
            /// Global Gstr간호사_USabun        As String  '2014-09-11
            /// </summary>
            public static string strNurseUSabun = ""; //Global Gstr간호사_USabun        As String  '2014-09-11
            /// <summary>
            /// Global Gstr간호사_UName         As String  '2014-09-11
            /// </summary>
            public static string strNurseUName = ""; //Global Gstr간호사_UName         As String  '2014-09-11
            //<==외래처방에서 사용
            //추가예정:면허번호, 전문의번호,

            //EMR 관련 정보
            /// <summary>
            /// 전자인증 사용여부 1: 사용 0: 사용안함
            /// </summary>
            public static string strAcert = "0";      //전자인증 사용여부 1: 사용 0: 사용안함
            /// <summary>
            /// 전자인증 아이디
            /// </summary>
            public static string strCertId = "";     //전자인증 아이디
            /// <summary>
            /// 전자인증 비밀번호
            /// </summary>
            public static string strCertPw = "";     //전자인증 비밀번호
            /// <summary>
            /// EMR 조회 권한
            /// </summary>
            public static string AuAVIEW = "0";       //EMR 조회 권한
            /// <summary>
            /// EMR 출력(원내) 권한
            /// </summary>
            public static string AuAPRINTIN = "0";      //EMR 출력(원내) 권한
            /// <summary>
            /// EMR 출력(원외) 권한
            /// </summary>
            public static string AuAPRINTOUT = "0";   //EMR 출력(원외) 권한
            /// <summary>
            /// EMR 출력(심사용) 권한
            /// </summary>
            public static string AuAPRINTSIM = "0";   //EMR 출력(심사용) 권한
            /// <summary>
            /// EMR 스캔 권한
            /// </summary>
            public static string AuASCAN = "0";       //EMR 스캔 권한
            /// <summary>
            /// EMR 스캔검수 권한
            /// </summary>
            public static string AuAVERIFY = "0";     //EMR 스캔검수 권한
            /// <summary>
            /// EMR 사본발급 권한
            /// </summary>
            public static string AuACOPY = "0";       //EMR 사본발급 권한
            /// <summary>
            /// EMR 작성 권한
            /// </summary>
            public static string AuAWRITE = "0";      //EMR 작성 권한
            /// <summary>
            /// EMR 관리자 권한
            /// </summary>
            public static string AuAMANAGE = "0";     //EMR 관리자 권한
            /// <summary>
            /// EMR 사용여부
            /// </summary>
            public static string AuAUSE = "0";        //EMR 사용여부
            /// <summary>
            /// EMR 스캔/대리처방 권한
            /// </summary>
            public static string AuAIMAGE = "0";     
            /// <summary>
            ///
            /// </summary>
            public static string MibiChartFlag = "0";
            /// <summary>
            /// 환자정보 마스크
            /// </summary>
            public static string AuAMASK = "0";
        }

        /// <summary>
        /// 사용자 정보를 초기화 한다.
        /// Author : 박웅규
        /// Create Date : 2017.07.12
        /// </summary>
        public static void ClearUser()
        {
            //New
            User.UserName = "";    //사용자명   // GstrDrName(oorder)
            User.CanOrd = "";    //오더권한     //
            User.IsDoct = "";    //의아여부   //
            User.IsNurse = "";    //간호사여부  //
            User.IsCoNurse = "";    //간호조무사여부    // GstrDrName(oorder)
            //변경이력 : AdoConst(adoODBC.bas) => clsPublic
            User.IdNumber = "";   //	GnJobSabun, GstrJobSabun
            User.Sabun = "";    //	GstrSabun, GstrDrSabun(oorder)
            User.JobName = "";  //	GstrJobName
            User.Jiyek = "";    //	GstrJiyek
            User.JobPart = "";  //	GstrJobPart, FstrPassPart
            User.Grade = "";    //	GstrJobGrade, FstrPassGrade, FstrGRADE
            User.BuseCode = ""; //	GstrBuseCode, GstrPassBuse
            User.PasswordChar = ""; //	GstrPasswordChar, FstrPassWordChar
            User.PassId = "";   //	GstrPassID,FstrPassId
            User.PassWord = ""; //	GstrPassWord, FstrPassWord
            User.Passhash = ""; //	GstrPasshash
            User.Passhash256 = "";  //	GstrPasshash256
            User.PassJdate = "";    //	GstrPassJdate
            User.Pass6M = "";   //	GstrPass6M
            User.PassCharge = "";   //	FstrPassCharge
            User.PassCount = 0;   //	FnPassCount
            User.SabunCnt = 0;    //	FnSabunCnt
            User.LogOutCnt = 0;   //	GnLogOutCNT
            User.PassName = ""; //	FstrPassName
            User.JobMan = "";   //	GstrJobMan
            User.DeptCode = ""; //	GstrDeptCode
            //변경이력 : VbFunction(VbFunc.bas) => clsPublic
            User.AnatDeptCode = ""; //	GstrAnatDeptCode
            //변경이력 : oorder(OORDER.BAS)
            User.DeptName = ""; //	GstrDeptName
            User.JupsuDept1 = "";   //	GstrJupsuDept1
            User.JupsuDept2 = "";   //	GstrJupsuDept2
            User.ResvGbn = 0;   //예약구분(0.안함 1.10분 2.15분 3.20분 4.30분) //	GnResvGbn
            User.ResvInwon = 0; //	GnResvInwon
            User.TexDept = "";  //	Gstr부가세사용과
            //추가예정 : 면허번호, 전문의번호,
            //EMR 관련 정보
            User.strAcert = "0";      //전자인증 사용여부 1: 사용 0: 사용안함
            User.strCertId = "";     //전자인증 아이디
            User.strCertPw = "0";     //전자인증 비밀번호
            User.AuAVIEW = "0";       //EMR 조회 권한
            User.AuAPRINTIN = "0";      //EMR 출력(원내) 권한
            User.AuAPRINTOUT = "0";   //EMR 출력(원외) 권한
            User.AuAPRINTSIM = "0";   //EMR 출력(심사용) 권한
            User.AuASCAN = "0";       //EMR 스캔 권한
            User.AuAVERIFY = "0";     //EMR 스캔검수 권한
            User.AuACOPY = "0";       //EMR 사본발급 권한
            User.AuAWRITE = "0";      //EMR 작성 권한
            User.AuAMANAGE = "0";     //EMR 관리자 권한
            User.AuAUSE = "0";        //EMR 사용여부
        }

        //병원환경정보
        /// <summary>
        /// 병원환경정보
        /// </summary>
        public struct HosInfo
        {
            /// <summary>
            /// 의원 1, 병원 2, 종합 3
            /// </summary>
            public static string strHosCls;        //'의원 1, 병원 2, 종합 3
            /// <summary>
            /// 로그인시 지정하는 0:주간, 1.야간, 2.휴일
            /// </summary>
            public static string strNgtWekCls;     //로그인시 지정하는 0:주간, 1.야간, 2.휴일
            /// <summary>
            /// 병원주소(한글명)
            /// </summary>
            public static string strAddressKor;    //병원주소(한글명)
            /// <summary>
            /// 병원주소(영문명)
            /// </summary>
            public static string StrAddressEng;    //병원주소(영문명)
            /// <summary>
            /// 병원명(한글명)
            /// </summary>
            public static string strNameKor;       //병원명(한글명)
            /// <summary>
            /// 병원명(영문명)
            /// </summary>
            public static string strNameEng;       //병원명(영문명)
            /// <summary>
            /// E메일
            /// </summary>
            public static string strEmail;         //E메일
            /// <summary>
            /// 전화번호
            /// </summary>
            public static string strTel;           //전화번호
            /// <summary>
            /// 팩스
            /// </summary>
            public static string strFax;           //팩스
            /// <summary>
            /// 대표자명
            /// </summary>
            public static string strManager;       //대표자명
            /// <summary>
            /// 요양기관번호
            /// </summary>
            public static string strHospitalNo;    //요양기관번호
            /// <summary>
            /// 우편번호
            /// </summary>
            public static string strZipCode;       //우편번호
            /// <summary>
            /// strZipSeq
            /// </summary>
            public static string strZipSeq;        //strZipSeq
            /// <summary>
            /// 원외처방유효기간
            /// </summary>
            public static string strSheetDay;      //원외처방유효기간
            /// <summary>
            /// 사업자등록번호
            /// </summary>
            public static string strSNo;           //사업자등록번호
            /// <summary>
            /// 병원에 EMR 인증 사용여부 : 1=사용
            /// </summary>
            public static string strEmrCertUseYn;  //병원에 EMR 인증 사용여부 : 1=사용
            /// <summary>
            /// 청구소프트웨어 업체코드(병원요양기관기호)
            /// </summary>
            public static string strPROGID;        //청구소프트웨어 업체코드(병원요양기관기호)
            /// <summary>
            /// 인증코드
            /// </summary>
            public static string strDURID;        //인증코드
            /// <summary>
            /// 이미지 사용여부(0:미사용,1:사용)
            /// </summary>
            public static string strIMAGEUSE;       //이미지 사용여부(0:미사용,1:사용)
            /// <summary>
            /// 스프래드 스킨(0:디폴트,1:사용자)
            /// </summary>
            public static string strSpdSkin;        //스프래드 스킨(0:디폴트,1:사용자)
            /// <summary>
            /// 자동로그아웃(0:사용안함, 분단위로 세팅)
            /// </summary>
            public static string strAutoLogout;        //자동로그아웃(0:사용안함, 분단위로 세팅)
            /// <summary>
            /// 진료화면설정(0:사용안함, 1:사용)
            /// </summary>
            public static string strDrEmrDsp;        //진료화면설정(0:사용안함, 1:사용)
            /// <summary>
            /// 서버접속방법(NET: , FTP:)
            /// </summary>
            public static string strConType;        //서버접속방법(NET: , FTP:)
            /// <summary>
            /// 전자인정-인정서버 IP
            /// </summary>
            public static string strCertSvr;        //전자인정-인정서버 IP
            /// <summary>
            /// 전자인정-인정서버 포터
            /// </summary>
            public static string strCertSvrPt;        //전자인정-인정서버 포터
            /// <summary>
            /// 전자인증-병원인정서
            /// </summary>
            public static string strCertDn;        //전자인증-병원인정서
            /// <summary>
            /// S/W 라이선스 정보
            /// </summary>
            public static string SwLicense="";        //SW 라이선스번호
            public static string SwLicInfo="";        //SW 라이선스정보
        }

        public static void ClearHosInfo()
        {
            HosInfo.strAddressKor = "";
            HosInfo.StrAddressEng = "";
            HosInfo.strNameKor = "";
            HosInfo.strNameEng = "";
            HosInfo.strEmail = "";
            HosInfo.strTel = "";
            HosInfo.strFax = "";
            HosInfo.strManager = "";
            HosInfo.strHospitalNo = "";
            HosInfo.strZipCode = "";
            HosInfo.strZipSeq = "";
            HosInfo.strSheetDay = "";
            HosInfo.strHosCls = "";
            HosInfo.strSNo = "";
            HosInfo.strEmrCertUseYn = "";
            HosInfo.strPROGID = "";
            HosInfo.strDURID = "";
            HosInfo.strIMAGEUSE = "0";
            HosInfo.strSpdSkin = "1";
            HosInfo.strAutoLogout = "0";
            HosInfo.strDrEmrDsp = "0";
            HosInfo.strConType = "";
            HosInfo.strCertSvr = "";
            HosInfo.strCertSvrPt = "";
            HosInfo.strCertDn = "";
        }

        //서버정보
        public struct SvrInfo
        {
            /// <summary>
            /// 프로그램 기본 위치
            /// </summary>
            public static string strClient;        //프로그램 기본 위치
            /// <summary>
            /// 
            /// </summary>
            public static string strFtpServerIp;        //
            /// <summary>
            /// 
            /// </summary>
            public static string strFtpPort;        //
            /// <summary>
            /// 
            /// </summary>
            public static string strFtpUser;        //
            /// <summary>
            /// 
            /// </summary>
            public static string strFtpPasswd;        //
            /// <summary>
            /// 
            /// </summary>
            public static string strFtpHomePath;        //
            /// <summary>
            /// 네트워크 드라이브 접근용
            /// </summary>
            public static string strNetServer;        //네트워크 드라이브 접근용
            /// <summary>
            /// 네트워크 드라이브 접근용
            /// </summary>
            public static string strNetDrivePath;        //네트워크 드라이브 접근용
            /// <summary>
            /// 네트워크 드라이브 접근용
            /// </summary>
            public static string strNetDriveID;        //네트워크 드라이브 접근용
            /// <summary>
            /// 네트워크 드라이브 접근용
            /// </summary>
            public static string strNetDrivePW;        //네트워크 드라이브 접근용
            /// <summary>
            /// 
            /// </summary>
            public static string strManualUrl;        //
        }

        /// <summary>
        /// 컴퓨터 환경설정
        /// </summary>
        public struct CompEnv
        {
            public static bool NotAutoLogOut = false; //자동 로그아웃 제외 컴퓨터
        }

        /// <summary>
        /// 컴퓨터 환경설정을 초기화 한다
        /// </summary>
        public static void ClearCompEnv()
        {
            CompEnv.NotAutoLogOut = false;
        }

        #endregion //신규 공통

        #region //opdacct(opdacct.bas)

        //ComPmpaLibB 에 이미 선언되있으므로 주석처치 함. 2018-02-09 KMC
        //'AMT(01):진찰료(예약)  AMT(06):처치수술료    AMT(10):조합부담  AMT(14):금액합계(진찰료제외)
        //'AMT(02):투약          AMT(07):CT/MRI/SONO   AMT(11):본인부담  AMT(15):영수금액
        //'AMT(03):주사료        AMT(08):기타          AMT(12):감    액  AMT(16):헌혈미수
        //'AMT(04):검사          AMT(09):금액합계      AMT(13):개인미수  AMT(17):수혈료
        //'AMT(05):방사선
        //public struct Add_Amt_Table
        //{
        //    public static long[] Amt1 = null; //급여금액
        //    public static long[] Amt2 = null; //비급여금액
        //    public static long[] Amt3 = null; //특진료
        //}
        //Global AAT                  As Add_Amt_Table
        //환불작업시 금액 누적
        //public struct Return_Amt_Table
        //{
        //    public static long[] Amt1 = null; //급여금액
        //    public static long[] Amt2 = null; //비급여금액
        //    public static long[] Amt3 = null; //특진료
        //}
        //Global RAT                  As Return_Amt_Table
        //당일의 총금액 보관용
        //public struct OLD_Amt_Table
        //{
        //    public static long[] Amt1 = null; //급여금액
        //    public static long[] Amt2 = null; //비급여금액
        //    public static long[] Amt3 = null; //특진료
        //}
        //Global OLD                  As OLD_Amt_Table
        //25,000원이하,초과
        //public struct WORK_Amt_Table
        //{
        //    public static long[] Amt1 = null; //급여금액
        //    public static long[] Amt2 = null; //비급여금액
        //    public static long[] Amt3 = null; //특진료
        //}
        //Global WAT                  As WORK_Amt_Table

        //public struct Argument_Table
        //{
        //    public static string Date = "";    // * 10
        //    public static string Dept = "";    // * 2
        //    public static string Sex = "";    // * 1
        //    public static string GbSpc = "";    // * 1
        //    public static string GbGamek = "";    // * 1
        //    public static string Sang1 = "";    //
        //    public static string Sang2 = "";    //
        //    public static string Sang3 = "";    //
        //    public static int Retn = 0;    //
        //    public static int Bi = 0;    //
        //    public static int Bi1 = 0;    //
        //    public static int Age = 0;    //
        //    public static int Fee1 = 0;    //
        //    public static int Fee2 = 0;    //
        //    public static int Fee3 = 0;    //
        //    public static int Fee31 = 0;    //
        //    public static int Fee5 = 0;    //
        //    public static int Fee51 = 0;    //
        //    public static int Fee7 = 0;    //
        //}
        //Global a                    As Argument_Table

        //public struct Slip_Accept_Table
        //{
        //    public static double[] Qty = null;    //
        //    public static int[] Nal = null;    //
        //    public static string[] Imiv = null;    // * 1
        //    public static string[] Dev = null;    // * 8
        //    public static string[] GbNgt = null;    // * 1
        //    public static string[] GbSpc = null;    // * 1
        //    public static string[] GbSelf = null;    // * 1
        //    public static long[] BaseAmt = null;    //
        //    public static string[] SuCode = null;    // * 8
        //    public static string[] SuNext = null;    // * 8
        //    public static string[] Bun = null;    // * 2
        //    public static string[] Nu = null;    // * 2
        //    public static string[] SugbA = null;    // * 1
        //    public static string[] SugbB = null;    // * 1
        //    public static string[] SugbC = null;    // * 1
        //    public static string[] SugbD = null;    // * 1
        //    public static string[] SugbE = null;    // * 1
        //    public static string[] SugbF = null;    // * 1
        //    public static string[] SugbG = null;    // * 1
        //    public static string[] SugbH = null;    // * 1
        //    public static string[] SugbI = null;    // * 1
        //    public static string[] SugbJ = null;    // * 1
        //    public static string[] SugbK = null;    // * 1
        //    public static string[] SugbM = null;    // * 1  '퇴장방지의약품(0.아님 1.방지의약품)
        //    public static string[] SugbO = null;    // * 1  '의약분업(0.원외 1-B:원내조제)
        //    public static string[] SugbP = null;    // * 1  '프로그램에 사용않함
        //    public static string[] SugbQ = null;    // * 1  '산재급여(1.무조건 급여)
        //    public static long[] Iamt = null;    //
        //    public static long[] Tamt = null;    //
        //    public static long[] Bamt = null;    //
        //    public static string[] Sudate = null;    // * 10
        //    public static long[] OldIamt = null;    //
        //    public static long[] OldTamt = null;    //
        //    public static long[] OldBamt = null;    //
        //    public static int[] DayMax = null;    //
        //    public static int[] TotMax = null;    //
        //    public static string[] SunameK = null;    //
        //    public static string[] Hcode = null;    //
        //    public static double[] Orderno = null;    //
        //    public static string[] GbTuyak = null;    // * 1
        //    public static string[] Remark = null;    //
        //    public static string[] GbInfo = null;    //
        //    public static string[] SlipNo = null;    // * 4
        //    public static string[] GbIpd = null;    // * 1
        //    public static string[] DrCode = null;    //
        //}
        //Global SA                   As Slip_Accept_Table

        //public struct Slip_Host_Table
        //{
        //    public static string SuCode = "";    // * 8
        //    public static double Qty = 0;    //
        //    public static int Nal = 0;    //
        //    public static string Imiv = "";    // * 1
        //    public static string Dev = "";    //
        //    public static string GbNgt = "";    // * 1
        //    public static string GbSpc = "";    // * 1
        //    public static string GbSelf = "";    // * 1
        //    public static string SugbO = "";    // * 1  '의약분업(0.원외 1-B:원내조제)
        //    public static long BaseAmt = 0;    //
        //}
        //Global SH                   As Slip_Host_Table

        //public struct Slip_Gesan_Table
        //{
        //    public static double Qty = 0;    //
        //    public static int Nal = 0;    //
        //    public static string GbNgt = "";    // * 1
        //    public static string GbSpc = "";    // * 1
        //    public static string GbSelf = "";    // * 1
        //    public static long BaseAmt = 0;    //
        //    public static string SuCode = "";    // * 8
        //    public static string SuNext = "";    // * 8
        //    public static string Bun = "";    // * 2
        //    public static string Nu = "";    // * 2
        //    public static string SugbA = "";    // * 1
        //    public static string SugbB = "";    // * 1
        //    public static string SugbC = "";    // * 1
        //    public static string SugbD = "";    // * 1
        //    public static string SugbE = "";    // * 1
        //    public static string SugbF = "";    // * 1
        //    public static string SugbG = "";    // * 1
        //    public static string SugbH = "";    // * 1
        //    public static string SugbI = "";    // * 1
        //    public static string SugbJ = "";    // * 1
        //    public static string SugbK = "";    // * 1
        //    public static string SugbM = "";    // * 1  '퇴장방지의약품(0.아님 1.방지의약품)
        //    public static string SugbO = "";    // * 1
        //    public static string SugbP = "";    // * 1
        //    public static string SugbQ = "";    // * 1
        //    public static long Iamt = 0;    //
        //    public static long Tamt = 0;    //
        //    public static long Bamt = 0;    //
        //    public static string Sudate = "";    // * 10
        //    public static long OldIamt = 0;    //
        //    public static long OldTamt = 0;    //
        //    public static long OldBamt = 0;    //
        //    public static int TotMax = 0;    //
        //    public static string DelDate = "";    // * 10
        //    public static double Orderno = 0;    //
        //    public static string Remark = "";    //
        //    public static string GbTuyak = "";    // * 1
        //    public static string Imiv = "";    // * 1
        //    public static string Dev = "";    // * 8
        //    public static string GbIpd = "";    // * 1
        //    public static string DrCode = "";    //
        //}
        //Global SG                   As Slip_Gesan_Table

        //public struct Slip_Write_Table
        //{
        //    public static string[] SuCode = null;    // * 8
        //    public static string[] SuNext = null;    // * 8
        //    public static string[] Bun = null;    // * 2
        //    public static string[] Nu = null;    // * 2
        //    public static double[] Qty = null;    //
        //    public static int[] Nal = null;    //
        //    public static long[] BaseAmt = null;    //
        //    public static string[] GbSpc = null;    // * 1
        //    public static string[] GbNgt = null;    // * 1
        //    public static string[] GbGisul = null;    // * 1
        //    public static string[] GbSelf = null;    // * 1
        //    public static string[] GbChild = null;    // * 1
        //    public static string[] GbHost = null;    // * 1
        //    public static long[] Amt1 = null;    //
        //    public static long[] Amt2 = null;    //
        //    public static double[] Orderno = null;    //
        //    public static string[] GbTuyak = null;    // * 1
        //    public static string[] GbImiv = null;    // * 1
        //    public static string[] DosCode = null;    // * 8
        //    public static string[] GbBunup = null;    // * 1
        //    public static string[] GbIpd = null;    // * 1
        //    public static string[] DrCode = null;    //
        //}
        //Global SW                   As Slip_Write_Table

        //public struct Slip_Return_Table
        //{
        //    public static string[] SuCode = null;    //
        //    public static string[] SuNext = null;    //
        //    public static string[] Bi = null;    //
        //    public static string[] BDate = null;    //
        //    public static string[] Bun = null;    //
        //    public static string[] Nu = null;    //
        //    public static double[] Qty = null;    //
        //    public static int[] Nal = null;    //
        //    public static long[] BaseAmt = null;    //
        //    public static string[] GbSpc = null;    //
        //    public static string[] GbNgt = null;    //
        //    public static string[] GbGisul = null;    //
        //    public static string[] GbSelf = null;    //
        //    public static string[] GbChild = null;    //
        //    public static string[] DrCode = null;    //
        //    public static string[] WardCode = null;    //
        //    public static string[] GbSlip = null;    //
        //    public static string[] GbHost = null;    //
        //    public static long[] Amt1 = null;    //
        //    public static long[] Amt2 = null;    //
        //    public static double[] Orderno = null;    //
        //    public static string[] GbImiv = null;    //
        //    public static string[] DosCode = null;    //
        //    public static string[] GbBunup = null;    //
        //    public static string[] GbIpd = null;    //
        //}
        ////Global SR                   As Slip_Return_Table

        #endregion //opdacct(opdacct.bas)

        #region //VbPcConfig.bas , VbPcConfigOcs.bas

        public struct PC_CONFIG     //Table_PC_CONFIG
        {
            /// <summary>
            /// 
            /// </summary>
            public static string IPAddress = "";    //
            /// <summary>
            /// 
            /// </summary>
            public static string BuseGbn = "";    //
            /// <summary>
            /// 
            /// </summary>
            public static string WardCode = "";    //
            /// <summary>
            /// 
            /// </summary>
            public static string DeptCode = "";    //
            /// <summary>
            /// 
            /// </summary>
            public static string DrCode = "";    //
            /// <summary>
            /// 
            /// </summary>
            public static string BuCode = "";    //
            /// <summary>
            /// 
            /// </summary>
            public static string PacsSW = "";    //
            /// <summary>
            /// 
            /// </summary>
            public static string CrtSize = "";    //
            /// <summary>
            /// 
            /// </summary>
            public static string Job = "";    //
            /// <summary>
            /// 
            /// </summary>
            public static string PcUserYN = "";    //
            /// <summary>
            /// 
            /// </summary>
            public static string PacsID = "";    //
            /// <summary>
            /// 
            /// </summary>
            public static string PacsPass = "";    //
            /// <summary>
            /// 
            /// </summary>
            public static string Remark = "";    //
            /// <summary>
            /// 
            /// </summary>
            public static string OS_Ver = "";    //
            /// <summary>
            /// 바코드 인쇄 방식 (1.기존 시리얼(컴 포드방식), 2. 바코드 드리아브방식, 3.GX420D, 4.CLP621)
            /// </summary>
            public static string BarCode = "";    // '바코드 인쇄 방식 (1.기존 시리얼(컴 포드방식), 2. 바코드 드리아브방식, 3.GX420D, 4.CLP621)
            /// <summary>
            /// 바코드 위치 x 마진
            /// </summary>
            public static int nx = 0;    // '바코드 위치 x 마진
            /// <summary>
            /// 바코드 위치 y 마진
            /// </summary>
            public static int nY = 0;    // '바코드 위치 y 마진
            /// <summary>
            /// 2015-05-15일 이후사용안함
            /// </summary>
            //[ObsoleteAttribute("2015-05-15일 이후사용안함", false)]
            public static string GX420D = "";    //  '2015-05-15일 이후사용안함
            /// <summary>
            /// 
            /// </summary>
            public static int GX420D_X = 0;    //
            /// <summary>
            /// 
            /// </summary>
            public static int GX420D_Y = 0;    //
        }

        //Global PC_CONFIG                   As Table_PC_CONFIG

        #endregion //VbPcConfig.bas , VbPcConfigOcs.bas

        #region //Pat

        public struct Pat
        {
            /// <summary>
            /// 
            /// </summary>
            public long lngIPDNO;
            /// <summary>
            /// 
            /// </summary>
            public string strPtNo;
            /// <summary>
            /// 
            /// </summary>
            public string strsName;
            /// <summary>
            /// 
            /// </summary>
            public int intAge;
            /// <summary>
            /// 
            /// </summary>
            public string strSex;
            /// <summary>
            /// 
            /// </summary>
            public string strGbSpc;
            /// <summary>
            /// 
            /// </summary>
            public string strDeptCode;
            /// <summary>
            /// 
            /// </summary>
            public string strDrCode;
            /// <summary>
            /// 
            /// </summary>
            public string strStaffid;
            /// <summary>
            /// 
            /// </summary>
            public string strBi;
            /// <summary>
            /// 
            /// </summary>
            public string strJTime;
            /// <summary>
            /// 
            /// </summary>
            public string strGbChojae;
            /// <summary>
            /// 
            /// </summary>
            public string strRDATE;
            /// <summary>
            /// 
            /// </summary>
            public string strRTime;
            /// <summary>
            /// 
            /// </summary>
            public string strRDrCode;
            /// <summary>
            /// 골수검사 임상소견
            /// </summary>
            public string strRemark1;     //골수검사 임상소견
            /// <summary>
            /// 골수검사 주요병력
            /// </summary>
            public string strRemark2;     //골수검사 주요병력
            /// <summary>
            /// 내시경
            /// </summary>
            public string strRemark3;     //내시경
            /// <summary>
            /// Biopsy Or Cytology
            /// </summary>
            public string strRemark4;     //Biopsy Or Cytology
            /// <summary>
            /// 
            /// </summary>
            public string strTel;
            /// <summary>
            /// 
            /// </summary>
            public string strGbSheet;
            /// <summary>
            /// Y.암등록환자/N.암미등록환자
            /// </summary>
            public string strVCode;     //Y.암등록환자/N.암미등록환자
            /// <summary>
            /// Y.검사결과확인예약
            /// </summary>
            public string strExam;     //Y.검사결과확인예약
            /// <summary>
            /// 
            /// </summary>
            public string strWardCode;
            /// <summary>
            /// 
            /// </summary>
            public string strRoomCode;
            /// <summary>
            /// 
            /// </summary>
            public string strDrName;
            /// <summary>
            /// 페렴
            /// </summary>
            public string strPNEUMONIA; //페렴
            /// <summary>
            /// 임신여부
            /// </summary>
            public string strPregnant; //임신여부
            /// <summary>
            /// 희귀난치성등록자구분
            /// </summary>
            public string strMCODE; //희귀난치성등록자구분
            /// <summary>
            /// 갑상선여부
            /// </summary>
            public string strThyroid; //갑상선여부
            /// <summary>
            /// 내시경 예약
            /// </summary>
            public string strres; //내시경 예약
            /// <summary>
            /// 인슐린투여환자
            /// </summary>
            public string strInsulin; //인슐린투여환자
            /// <summary>
            /// 차상위 희귀
            /// </summary>
            public string strMCODE_OPD; //차상위 희귀
            /// <summary>
            /// 
            /// </summary>
            public string strVCODE_OPD;
            /// <summary>
            /// 주민번호
            /// </summary>
            public string strJUMIN; //주민번호
            /// <summary>
            /// 분업 2012-05-29
            /// </summary>
            public string strbunup; //분업 2012-05-29
            /// <summary>
            /// 생일 2013-02-06
            /// </summary>
            public string strBirth; //생일 2013-02-06
            /// <summary>
            /// 2013-08-01
            /// </summary>
            public string strINDATE; //2013-08-01
            /// <summary>
            /// 2014-08-07
            /// </summary>
            public string strResMemo; //2014-08-07
            /// <summary>
            /// 2016-06-16(예약문자 형성 안함)
            /// </summary>
            public string strResSMSNot; //2016-06-16(예약문자 형성 안함)
        }

        #endregion //Pat

        #region //GBasCard.bas

        //clsPmpaType 으로 변수 이동시킴 2018-02-09 KMC
        //AcctReqData(승인요청 DATA)
        //public struct AcctReqData
        //{
        //    public string VanGb;
        //    public string OrderGb;
        //    public string MDate;
        //    public long SEQNO ;
        //    public string OrderNo;
        //    public string ClientId;
        //    public string EntryMode;
        //    public int CardLength;
        //    public string CardData;
        //    public string CardData2;
        //    public int CardDivide;
        //    public long TotAmt;
        //    public string OldAppDate;
        //    public string OldAppTime;
        //    public string OldAppNo;
        //    public string SectionNo;
        //    public string SiteID;
        //    public long CardSeqNo;
        //    public string Cardthru;
        //    public string ASaleDate;
        //    public string AApproveNo;
        //    public string Gubun;
        //    public string CashBun;
        //    public long OgAmt;
        //    public string CanSayu;
        //    public string KeyIn;
        //}
        //public static AcctReqData RSD;

        //AcctResData(승인 응답 DATA)
        //public struct AcctResData
        //{
        //    public string VanGb;
        //    public string OrderGb;
        //    public string MDate;
        //    public long SEQNO;
        //    public string OrderNo;
        //    public string ClientId;
        //    public string ReplyStat;
        //    public string ApprovalDate;
        //    public string ApprovalTime;
        //    public string ApprovalNo;
        //    public string BankId;
        //    public string BankName;
        //    public string MemberNo;
        //    public string PublishBank;
        //    public string CardName;
        //    public string Massage;
        //    public string HPay;
        //}
        //public static AcctResData RD;

        #endregion //GBasCard.bas

        #region //TABLE_EDI_SUGA     'EDI 표준코드 Table

        public struct TABLE_EDI_SUGA
        {
            public string ROWID;
            public string Code;
            public string Jong;
            public string PName;
            public string Bun;
            public string Danwi1;
            public string Danwi2;
            public string Spec;
            public string COMPNY;
            public string Effect;
            public string Gubun;
            public string Dangn;
            public string JDate1;
            public double Price1;
            public string JDate2;
            public double Price2;
            public string JDate3;
            public double Price3;
            public string JDate4;
            public double Price4;
            public string JDate5;
            public double Price5;
        }

        public static TABLE_EDI_SUGA TES;

        #endregion //TABLE_EDI_SUGA     'EDI 표준코드 Table

        #region //BuSanID.bas

        public struct Table_Bas_SANID
        {
            public string Pano;
            public string Bi;
            public string Sname;
            public string Jumin1;
            public string Jumin2;
            public string CoprName;
            public string CoprNo;
            public string Dept1;
            public string Dept2;
            public string Dept3;
            public string Dept4;
            public string Dept5;
            public string Date1;
            public string Date2;
            public string Date3;
            public string DateRequest;
            public string GbResult;
            public string GbIll;
            public string IllCode1;
            public string IllCode2;
            public string IllCode3;
            public string IllCode4;
            public string IllCode5;
            public string Memo;
            public int Count;
            public string Dname;
            public string BenHo;
            public string JONG;
            public string BoNo;  //'자보보증번호
        }

        public static Table_Bas_SANID TBS;

        public struct Table_Bas_SANDTL
        {
            public string Pano;
            public string DateApproval;
            public string IpdFrDate;
            public string IpdToDate;
            public int IpdTerm;
            public string OpdFrDate;
            public string OpdToDate;
            public int OpdTerm;
            public string RowID;
        }

        public static Table_Bas_SANDTL TSD;

        public struct Table_Bas_SANJIN
        {
            public string Pano;
            public string DateBal;
            public string DateReq;
            public string FrDate;
            public string ToDate;
            public int Term;
            public string RowID;
        }

        public static Table_Bas_SANJIN TSJ;

        public struct Table_Bas_PATIENT
        {
            public string Pano;
            public string Sname;
            public string Jumin1;
            public string Jumin2;
        }

        public static Table_Bas_PATIENT TBP;

        #endregion //BuSanID.bas

    }
}