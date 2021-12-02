using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComLibB
{

    
    using ComLibB.Dto;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;


    public class clsComVariable
    {
        #region 종검, 일반검진 공용변수 (중복정리)
        public static long GnPano;             //건진번호
        public static long GnPtno;             //병록번호
        public static string GstrJuminNo;
        public static string GstrSqlDef;
        public static string GstrProgram;      //일반건진, 출장검진
        public static string GstrSExams;       //기존 선택검사
        public static string GstrGjJong;       //일반검진 검진종류
        public static string GstrRefValue;
        public static string GstrRefValue1;    //종검결과코드
        public static string GstrXray;                 //방사선 플래그
        public static int GnPrtNo1;     //영수증1
        public static int GnPrtNo2;     //영수증2
        public static int GnPrtNo3;     //향정처방
        public static long GnCardSeqNo;     //카드승인일련번호
        public static long GnCashAmt;     //현금영수증금액

        public static string[] GstrGO = new string[10];     //가셔야할곳
        public static string[] gstrMultiline = new string[999];
        public static int[,] GnScale = new int[900, 540];
        #endregion

        #region HaBas.Bas
        /// </summary>        
        public static long GnWRTNO;
        public static string GstrSex = "";
        public static string GstrPrintCnt = "";
        public static string GstrTimerDate = "";
        //public static List<string> GstrExam = new List<string>();
        public static string[] GstrExam = new string[13];
        public static string GstrSelectExamCode = "";  //Gstr선택검사코드
        //public static string[] GstrHaSQL = new string[20]; //예약검사관련 쿼리
        public static List<COMHPC> LSTHaRESEXAM = new List<COMHPC>();
        public static string GstrCreditMachineCon = ""; //신용카드승인 타블릿모니터 여부(Gstr신용장비연결)
        public static string GstrIEMunjin = "";
        public static bool GbHeaAdminSabun = false;//     '종검 관리자 사번 여부
        public static string GstrPFTSN = "";
        public static string GstrJepDate = "";
        public static string GstrLtdGroupCode = "";    // 회사 계약코드
        public static string GstrUSE = "";

        #region 2019.08.01 이상훈 추가(종합검진 환경설정 관련 변수(자동액팅PC로 지정, 대기순번모니터 끄기, InBody 전송 PC
        public static string GstrHicPart = "";      //종검 부서 => (종검 : 1 , 일검 : 2)
        public static string GstrHicPartName = "";  //종검 부서 => (종합검진 일반검진)
        public static string GstrHeaAutoActingYN = "";
        public static string GstrHeaMonitorOffYN = "";
        public static string GstrHeaInbodySendYN = "";
        #endregion
        public const string Hic_Mun_EXE_PATH = @"C:\Program Files\SamOmr\friendly Omr.exe";
        public const string Hic_Mun_EXE_PATH_64 = @"C:\Program Files (x86)\SamOmr\friendly Omr.exe";
        public const string Hic_Mun_EXE_PATH2 = @"C:\CMC\EXE\HcMunView.EXE";
        public const string MF_BYPOSITION = "&H400";
        public static bool GbClose = true;      //종검 선택검사일정 닫기 버튼 활성화
        #endregion



        #region 2020.10.13 일반검진 종검 결과 자동전송 실행 PC 여부
        public static string GstrHeaResultAutoSendPC = "";
        #endregion

        #region HcBas.Bas
        //public static long GnWRTNO;
        public static long GnWRTNO_NEW;
        public static long GnWRTNO_접수증;// 건진접수증

        public static long GnPano_NEW;
        public static long GnWRTNO_NEW1;
        public static long GnWRTNO_NEW2;

        public static long GnDentAmt;
        public static long GnDentAddAmt;
        public static long GnDentAddAmt1;


        //'판정의사 정보
        //'===============================================
        //'기타 검사 결과 컴파일하는데 해당 전역변수가 없어서
        //'추가하였습니다. 참고하시기 바랍니다.
        public static long GnHicLicense;
        public static long GnHicLicense1;
        //'===============================================
        public static string GstrHicDrName; // 판정의사성명
        public static long GnHicSabun;      // 판정의사사번
        public static string GstrDrRoom;        // 판정의사 상담실번호
        public static string GstrDrGbPan;       // 일반판정여부
        public static string GstrDrGbDent;      // 구강판정여부
        public static string GstrIpsaDay;       // 입사일자
        public static string GstrReDay;         // 퇴사일자
        public static string GstrLtdJuso;       //회사상세주소
        public static string GstrLtdJuso1;      // 회사상세주소1
        public static string GstrLtdJuso2;      // 회사상세주소2
        public static string GstrLtdMailcode;   // 회사메일코드
        public static string GstrTel;   //회사전화
        //public static string[] GstrExam = new string[10];   //계측항목 10개항목 선택가능
        //public static string GstrPFTSN;   //폐활량검사장비  S/N 저장
        public static string GstrChul;
        public static string GstrValue;   //공용변수로 등록 함 ( 개별 모듈에서는 주석처리 바람)
        public static string GstrCardApprov;   //신용카드 승인 구분
        public static string GstrKiho;   //사업장기호
        public static int GnInwon;   //회사인원

        public static string GstrJumin;
        //public static string GstrJepDate;
        public static string GstrBDate;

        public static string GstrAmGbn;

        public static string GstrTempValue;           // 건진공용 Global 변수
        public static string Gstr자격상실구분;        // 건진공용변수 자격관련

        public static string GstrSchValue;            // 건진공용 Global 변수 - 출장,내원스케쥴관련
        public static string GstrMonitorSize;
        public static string Gstr신용장비연결;        // 신용카드 태블릿모니터 여부

        public static double GnAmt_Misu_BonAmt1;
        public static double GnAmt_Misu_LtdAmt1;
        public static double GnAmt_Misu_JohapAmt1;
        public static double GnAmt_Misu_BogenAmt1;
        public static double GnAmt_Misu_GamAmt1;

        public static double GnAmt_Misu_BonAmt2;
        public static double GnAmt_Misu_LtdAmt2;
        public static double GnAmt_Misu_JohapAmt2;
        public static double GnAmt_Misu_BogenAmt2;
        public static double GnAmt_Misu_GamAmt2;
        #endregion

        #region HaMain.Bas 
        /// </summary>          
        //public static string GstrHicDrName;        //판정의사성명
        //public static long GnHicLicense;           //판정의면허번호
        public static string GnDrRoom;             //판정의사 상담실번호
        //public static string GstrIpsaDay;          //입사일자
        //public static string GstrReDay;            //퇴사일자
        //public static string GstrValue;            //
        public static string GstrSDate;            //종검 수검일자
        public static string GstrRowid;           //
        public static string GstrWordView;         //종검상용등록
        public static string GstrPanWRTNO;         //종검상용등록
        public static string GstrEkgDate;          //내과초음파예약관련 이전 예약날짜 저장
        //public static string Gstr자격상실구분;     //건진공용변수 자격관련

        public static double GnHeight;

        public static int TColCNT;

        public static string GstrHaRefData;            //종검예약달력창 환자정보

        public static string GstrMonitor;
        public static bool GbGaResvLtd;                //종합건진 가예약 가능회사 여부(True:가예약가능)
        public static string GstrGaResvLtdList;        //종합건진 가예약 가능회사 목록  
        public static bool GbFamilly_PopUp;
        #endregion

        #region vbHic자료사전.Bas
        public static bool GbHicAdminSabun;            //일반건진 관리자 여부(True:관리자)
        public static bool GbHicJupsuAdminSabun;       //일반건진 접수관리자 여부(True:관리자)
        public static string B01_SANGDAM_DRLIST;       //01.상담의사 단축키 LIST (F3.윤경화 F4.김영배...)
        public static string B01_SANGDAM_DRNO;         //01.상담의사 단축키 면허번호(F3.윤경화(7478),F4.김영배(1776)...)
        public static string B01_ENDO_EXCODE;          //01.위,대장내시경 검사코드('TX20','TX23',...)
        public static string B02_PANJENG_DRNO;         //02.판정시 판정할 의사면허번호 목록(1234,1212,...)
        public static string B02_PANJENG_SABUN;        //02.판정시 판정할 의사사원번호 목록(1234,1212,...)
        public static bool B03_DNT_OPD_JIN;            //03.구강검진 외래치과 진료 여부(True/False)
        //public static string B04_NOT_PATIENT;          //04.BAS_PATIENT에서 제외할 성명
        public static List<string> B04_NOT_PATIENT = new List<string>();
        public static string B05_CHEST_XRAY;           //05.흉부방사선 검사코드 목록
        public static string B06_위내시경코드;         //06.위내시경코드
        public static string B07_대장내시경코드;       //07.대장내시경코드
        public static string B08_MisuAdminSabun;       //08.미수관리 수정,삭제 관리자 사번
        public static bool B09_JUMIN_ALL;              //09.주민등록번호 모두 표시할 사번 여부
        public static List<string> G36_NIGHT_CODE = new List<string>();           //36.야간작업 묶음코드 목록
        public static List<string> G37_DOCT_ENTCODE = new List<string>();   //37.판정의사가 직접 입력하는 검사코드 목록
        public static string G38_RESULT_NOT_CHECK;     //38.검사결과입력완료 미체크 항목
        public static string G39_HEMS_2차특수수납항목; //39.HEMS 특수2차 수납항목        
        #endregion

        #region vbHic자료사전.Bas
        public const string DENT_ROOM = "02";          //구강상담방 02로 변경
        public static bool GbHeaManager;               //종합건진 관리자 여부(True:관리자)
        public static bool B01_JONGGUM_SABUN;          //종검직원여부(True:종검직원사번)
        public static bool B01_DNT_DOCTOR;             //건진 치과과장님 여부(True:치과고장님사번)
        public static bool B01_VIEW_JUMIN;             //주민등록번호 전체를 조회가 가능한 사번 여부
        public static string B08_조영제사용검사;       //08.방사선 접수증에 검사재료대 인쇄        
        #endregion

        #region HcMain.bas    
        public static bool GbLDL400_Modify;    //LDL 400초과 접수 수정 여부
        public static long GnFirstPandrNo;
        public static string GstrNotMunjin;
        public static string GstrGjYear;       //검진년도
        public static string GstrUCodes;       //기존 특검항목
        public static string GstrMunjin;       //문진뷰어 실행여부
        public static string GstrTool;         //생활습관도구표 입력구분(접수)        
        public static string Gstr_PanB_Etc;
        public static string Gstr_PanR_Etc;
        public static int GnPanB_Etc;
        public static bool GbChulPc;            //출장검진
        public static string GstrWaitPcNo;      //대기순번용 PC번호(c:\HIC_WAIT.INI에 설정됨)
        public static string Gstr2ChaWrtno;     //2차 가접수대상 접수번호 목록
        #endregion

        #region 검진동의서
        public static string GstrCONSENT;       //검진동의서 (AdoConst.bas 에서 가져옴)
        #endregion

        #region DLL 및 Const 상수 선언 영역
        [DllImportAttribute("user32.dll")]
        public static extern long GetSystemMenu(ref long hwnd, ref long bRevert);

        [DllImportAttribute("user32.dll")]
        public static extern long GetSystemMenu(ref long hMenu, ref long nPosition, ref long wFlags);

        //[DllImportAttribute("user32.dll")]
        //public static extern long GetCursorPos(clsHcType.HaAct_POINTAPI lpPoint);

        [DllImportAttribute("kernel32.dll", EntryPoint = "SetFileAttributesA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern long SetFileAttributes(string lpFileName, long dwFileAttributes);

        #region 실행중인 프로세스 목록 API (2020.09.15 이상훈)
        public const Int32 MAX_PATH = 260;
        public const Int32 TH32CS_SNAPPROCESS = 2;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct PROCESSENTRY32
        {
            public Int32 dwSize;
            public Int32 cntUsage;
            public Int32 th32ProcessID;
            public IntPtr th32DefaultHeapID;
            public Int32 th32ModuleID;
            public Int32 cntThreads;
            public Int32 th32ParentProcessID;
            public Int32 pcPriClassBase;
            public Int32 dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public String szExeFile;

            public static Int32 Size
            {
                get { return Marshal.SizeOf(typeof(PROCESSENTRY32)); }
            }
        }

        [DllImport("kernel32")]
        public static extern IntPtr CreateToolhelp32Snapshot(Int32 dwFlags, Int32 th32ProcessID);

        [DllImport("kernel32")]
        public static extern void CloseHandle(IntPtr hObject);

        [DllImport("kernel32")]
        public static extern Int32 Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 pe32);

        [DllImport("kernel32")]
        public static extern Int32 Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 pe32);
        #endregion

        #region 모니터 해상도 구하기
        [DllImportAttribute("user32.dll")]
        public static extern int GetDesktopWindow();

        [DllImportAttribute("user32.dll")]
        public static extern long GetDC(long hwnd);

        [DllImportAttribute("gdi32.dll")]
        public static extern long GetDeviceCaps(long hDC, long nIndex);

        [DllImportAttribute("user32.dll")]
        public static extern long GetSystemMenu(long hwnd, long bRevert);

        [DllImportAttribute("user32.dll")]
        public static extern long RemoveMenu(long hMenu, long nPosition, long wFlags);

        //[DllImportAttribute("user32.dll", EntryPoint = "GetCursorPos")]
        //public static extern int GetCursorPos(ref clsHcType.HaAct_POINTAPI lpPoint);

        //한영혼합 문자열 구하기
        [DllImportAttribute("kernel32.dll", EntryPoint = "lstrlenA")]
        public static extern long lstrlen(string lpString);
        #endregion

        #region vbHicDojang
        public const string FILE_ATTRIBUTE_ARCHIVE = "&H20";
        public const string FILE_ATTRIBUTE_COMPRESSED = "&H800";
        public const string FILE_ATTRIBUTE_DIRECTORY = "&H10";
        public const string FILE_ATTRIBUTE_HIDDEN = "&H2";
        public const string FILE_ATTRIBUTE_NORMAL = "&H80";
        public const string FILE_ATTRIBUTE_READONLY = "&H1";
        public const string FILE_ATTRIBUTE_SYSTEM = "&H4";
        public const string FILE_ATTRIBUTE_TEMPORARY = "&H100";
        #endregion

        public const string hNULL = "0";
        public const string INFINITE = "-1&";
        public const string PROCESS_ALL_ACCESS = "&H1F0FFF";

        [DllImportAttribute("kernel32.dll")]
        public static extern long WaitForSingleObject(ref long hHandle, ref long dwMilliseconds);
        #endregion

        #region vbExamRefer.bas
        public static string REFER_CHANGE_CODELIST; //참고치가 변경된 코드 목록 ('A124',...,'A274')
        public static string REFER_OLD_남자_VALUE; //변경전 남자 참고치 (A124{}7~38{}Unit{@}....{@}A274{}0.6~1.2{}Unit{@})"
        public static string REFER_OLD_여자_VALUE; //변경전 여자 참고치 (A124{}7~38{}Unit{@}....{@}A274{}0.6~1.2{}Unit{@})"
        public static string REFER_NEW_남자_VALUE; //변경후 남자 참고치 (A124{}7~38{}Unit{@}....{@}A274{}0.6~1.2{}Unit{@})"
        public static string REFER_NEW_여자_VALUE;  //변경후 여자 참고치 (A124{}7~38{}Unit{@}....{@}A274{}0.6~1.2{}Unit{@})"
        public static long REFER_OLD_CNT;
        public static long REFER_NEW_CNT;

        #endregion

        #region ExMain00.bas
        public static string gsBarSpecNo;
        public static string GstrTubeMsg;   //검체,용기,수량
        public static bool GbHicChul;
        public static double GnWBVolume;    //WB의 총량
        public static string GstrJepsuJob; //현재 작업중인 접수 작업(OPD,MANUAL,OCS)
        #endregion

        #region HcBill.bas
        public static string GstrBExam1;
        #endregion

        #region vbHicMonitor.bas
        public static int selmonitor;       //슬레이브왼쪽 0, 오른쪽 1
        public static long slavecoodinate;  //폼위치 좌표계산 필요값
        public static int singmon;          //처음부터 단일일 경우
        public static long selmon;          //슬레이브모니터 위치 인덱스
        public static string predirectory;  //이전디렉토리 저장변수

        public static long singmonX;        //단일출력시 이미지박스크기
        public static long singmonY;        //단일출력시 이미지박스크기

        public static long dualmonX;        //듀얼출력시 이미지박스크기
        public static long dualmonY;        //듀얼출력시 이미지박스크기

        public static string ipicname;      //화면고정시 쓰이는 변수
        public static string logopic;

        public const string MONITOR_DEFAULTTONULL = "&H0";
        public const string MONITOR_DEFAULTTOPRIMARY = "&H1";
        public const string MONITOR_DEFAULTTONEAREST = "&H2";

        public const string SC_MONITORPOWER = "&HF170&";
        public const string MONITOR_ON = "-1&";
        public const string MONITOR_OFF = "2&";
        public const string WM_SYSCOMMAND = "&H112";

        #endregion

        #region VbHicPFT.bas(이상훈) 

        public const string FTP_IpAddr = "192.168.100.31";
        public const string FTP_User = "oracle";
        public static string FTP_Pass = "";
        #endregion

        #region HaAct.bas        
        public static string GstrHeight;
        public static string GstrWeight;
        public static int GnAutoTimerCnt;
        #endregion

        #region BarCodePrint.bas(이상훈) : BaseFile(BarCodePrint.bas)
        //출장검진 바코드 인쇄 요청 DB에 전송 여부
        //True:바코드가 인쇄되지 않고 HIC_BARCODE_CHUL DB에 요청내용이 저장됨
        public static bool GbBarcodeDbSend = false;
        #endregion

        public static string GstrPanFrDate;     //판정화면 환자리스트 조회일자
        public static string GstrPanToDate;     //판정화면 환자리스트 조회일자
        public static string GStrPanGjJong;     //판정화면 환자리스트 왼쪽 검진종류
        public static string GStrPanJob;        //판정화면 환자리스트 미판정/판정/전체 여부
        public static string GstrPanDrNo;       //판정화면 환자리스트 cboPan 판정의사 콤보
        public static string GstrLtdName;       //판정화면 환자리스트 사업장명
        public static string GstrGbChul;        //판정화면 환자리스트 출장구분
        public static string GstrGbSort;        //판정화면 환자리스트 정렬기준

        public static string GstrPanLifeTab0;   //판정화면 생활습관처방전 금연처방전 Tab Enabled 여부
        public static string GstrPanLifeTab1;   //판정화면 생활습관처방전 음주/절주 처방전Tab Enabled 여부
        public static string GstrPanLifeTab2;   //판정화면 생활습관처방전 운동처방전 Tab Enabled 여부
        public static string GstrPanLifeTab3;   //판정화면 생활습관처방전 영양처방전 Tab Enabled 여부
        public static string GstrPanLifeTab4;   //판정화면 생활습관처방전 비만처방전 Tab Enabled 여부

        public static bool GbMunjinView = true;      //판정화면 문진뷰어 체크여부

        /// <summary>
        /// 환자정보 클래스 (
        /// </summary>
        public class clsBasPatient
        {
            public string Pano = "";
            public string SName = "";
            public string DeptCode = "";
            public string DrCode = "";
            public string Jumin1 = "";
            public string Jumin2 = "";
            public string Jumin3 = "";
            public string JuminFull = "";
            public string Bi = "";
            public string ZipCode1 = "";
            public string ZipCode2 = "";
            public string ZipCode3 = "";
            public string BuildNo = "";
            public string RoadDetail = "";
            public string Juso = "";
            public string Religion = "";
            public string EMail = "";
            public string Sex = "";
            public int Age = 0;
            public string RoomCode = "";
            public string StartDate = "";
            public string LastDate = "";
            public string HPhone = "";
            public string Tel = "";
            public string EkgMsg = "";
            public string Birth = "";
            public string GbBirth = "";
            public string Jikup = "";
            public string OrderName = "";
            public string RemarkC = "";
            public string RemarkD = "";
            public string infect = "";
            public string fall = "";
            public string ROWID = "";
            public string IPDNO = "";

        }

        #region 판정환자 선택 정보
        public static string FSelGjJong = string.Empty;   //선택 수진종류
        public static long FSelWRTNO = 0;                 //선택 수진접수번호
        #endregion
    }
}
