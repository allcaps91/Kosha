/// <summary>
/// Description     : 건진센터 공용모듈 / Structure, Enum 계열 변수선언
/// Author          : 김민철
/// Create Date     : 2019-07-12
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
namespace ComHpcLibB
{
    using ComHpcLibB.Dto;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 검진 전역 Structure, Enum
    /// </summary>
    public class clsHcType
    {
        /// <summary>
        /// 계측결과 입력 Spread
        /// </summary>
        public enum Instrument_Result
        {
            CODE = 0,
            EXAMNAME,
            RESULT,
            HELPBTN,
            RESULTCODE,
            PANJENG,
            REFERENCE,
            RESCODE,
            CHANGE,
            ROWID,
            RESTYPE,
            PRERESULT1,
            PRERESULT2,
            HELP,
            PART
        }

        /// <summary>
        /// HaBas.bas
        /// </summary>
        public struct HaAct_POINTAPI
        {
            public static long x;
            public static long y;
        }
        public HaAct_POINTAPI pointApi;

        public struct TABLE_HIC_JEPSU
        {
            public string GBUSE;
            public long   WRTNO;
            public string GJYEAR;
            public string JEPDATE;
            public long   PANO;
            public string SNAME;
            public string SEX;
            public long   AGE;
            public string GJJONG;
            public string GJCHASU;
            public string GJBANGI;
            public string GBCHUL;
            public string GBSTS;
            public string GBINWON;
            public string DELDATE;
            public long   LTDCODE;
            public string MAILCODE;
            public string JUSO1;
            public string JUSO2;
            public string TEL;
            public string PTNO;
            public string BURATE;
            public string JISA;
            public string KIHO;
            public string GKIHO;
            public string JIKGBN;
            public string UCODES;
            public string SEXAMS;
            public string JIKJONG;
            public string SABUN;
            public string IPSADATE;
            public string BUSENAME;
            public string OLDJIKJONG;
            public string BUSEIPSA;
            public string OLDSDATE;
            public string OLDEDATE;
            public string GBSUCHEP;
            public string BALYEAR;
            public long   BALSEQ;
            public string GBEXAM;
            public string BOGUNSO;
            public string JEPSUGBN;
            public string YOUNGUPSO;
            public string JEPNO;
            public string GBSPCMUNJIN;
            public string GBPRINT;
            public long   GBSABUN;
            public string GBJINCHAL;
            public string GBMUNJIN1;
            public string GBMUNJIN2;
            public string GBMUNJIN3;
            public string GBDENTAL;
            public string SECOND_FLAG;
            public string SECOND_DATE;
            public string SECOND_TONGBO;
            public string SECOND_EXAMS;
            public string SECOND_SAYU;
            public string CHUNGGUYN;
            public string DENTCHUNGGUYN;
            public string CANCERCHUNGGUYN;
            public string LIVER2;
            public long   MAMT;
            public long   FAMT;
            public string MILEAGEAM;
            public string MURYOAM;
            public string GUMDAESANG;
            public string JONGGUMYN;
            public string SEND;
            public string MILEAGEAMGBN;
            public string MURYOGBN;
            public string NEGODATE;
            public long   JOBSABUN;
            public string ENTTIME;
            public string SECOND_MISAYU;
            public long   MIRNO1;
            public long   MIRNO2;
            public long   MIRNO3;
            public string EMAIL;
            public string REMARK;
            public long   OHMSNO;
            public long   MISUNO1;
            public long   FIRSTPANDRNO;
            public string ERFLAG;
            public long   MIRNO4;
            public string MIRSAYU;
            public string ERTONGBO;
            public long   MISUNO2;
            public string GUBDAESANG;
            public long   MIRNO5;
            public long   MISUNO3;
            public string XRAYNO;
            public long   CARDSEQNO;
            public string GBADDPAN;
            public string GBAM;
            public string BALDATE;
            public string TONGBODATE;
            public string GBSUJIN_SET;
            public string GBCHUL2;
            public string GBJINCHAL2;
            public long   SANGDAMDRNO;
            public string SANGDAMDATE;
            public string SENDMSG;
            public string GBN;
            public long   CLASS;
            public long   BAN;
            public long   BUN;
            public long   HEMSNO;
            public string HEMSMIRSAYU;
            public string AUTOJEP;
            public long   MISUNO4;
            public long   P_WRTNO;
            public string PANJENGDATE;
            public long   PANJENGDRNO;
            public string GBAUDIO2EXAM;
            public string GBMUNJINPRINT;
            public string WAITREMARK;
            public string PDFPATH;
            public string WEBSEND;
            public string WEBSENDDATE;
            public string GBNAKSANG;
            public string GBDENTONLY;
            public string GBAUTOPAN;
            public long   IEMUNNO;
            public string WEBPRINTREQ;
            public string GBSUJIN_CHK;
            public string WEBPRINTSEND;
            public string LTDCODE2;
            public long   DENTAMT;
            public string GBHUADD;
            public string EXAMREMARK;
            public long   PRTSABUN;
            public string GBHEAENDO;
            public string GBCHK1;
            public string GBCHK2;
            public string GBCHK3;
            public string GBLIFE;
            public string GBJUSO;
            public long   GWRTNO;
            public bool   GBXRAYSEND;
            public string RID;
        }
        /// <summary>
        /// 검진통합 접수 영역
        /// index => 0: 일반검진, 1: 특수검진, 2: 암검진, 3: 종합검진, 4: 기타검진 (TO_BE)
        /// index => 0: 검진1, 1: 검진2, 2: 검진3, 3: 검진4, 4: 검진5, 5: 종합검진 (AS_IS)
        /// </summary>
        public static TABLE_HIC_JEPSU[] THC = new TABLE_HIC_JEPSU[6];

        public struct TABLE_HIC_PATIENT
        {
            public static long nPano;
            public static string strSname;
            public static string strJumin;
            public static string strSex;
            public static string strMailCode;
            public static string strJuso1;
            public static string strJuso2;
            public static string strTel;
            public static string strLtdCode;
            public static string strJikGbn;
            public static string strJikJong;
            public static string strSaBun;
            public static string strBuseName;
            public static string strGongjeng;
            public static string strIpsadate;
            public static string strBuseipsa;
            public static string strJisa;
            public static string strGkiho;
            public static string strGbSuchep;
            public static string strPtNo;
            public static string strRemark;
            public static string strkiho;
            public static string strBogunso;
            public static string strYoungupso;
            public static string strLiver2;
            public static string strGumDaesang;
        }
        public TABLE_HIC_PATIENT THP;

        public struct Table_Temp
        {
            public static string PtNo;
            public static string Bi;
            public static string sName;
            public static string Sex;
            public static string DeptCode;
            public static string WardCode;
            public static int RoomCode;
            public static string DrCode;
        }
        public Table_Temp Pat;

        /// <summary>
        /// 전자동의서 서식항목 값 세팅을 위한 Tag값 (위, 대장)
        /// </summary>
        public const string MUNJIN11 = ",radiobox_1581409330681*true";    //기왕력(없음)
        public const string MUNJIN12 = ",radiobox_1581409339247*true";    //기왕력(있음)
        public const string MUNJIN21 = ",radiobox_1581409356571*true";    //알레르기(없음)
        public const string MUNJIN22 = ",radiobox_1581409367164*true";    //알레르기(있음)
        public const string MUNJIN31 = ",radiobox_1581409398213*true";    //당뇨병(없음)
        public const string MUNJIN32 = ",radiobox_1581409411564*true";    //당뇨병(있음)
        public const string MUNJIN41 = ",radiobox_1581409462125*true";    //저고혈압(없음)
        public const string MUNJIN42 = ",radiobox_1581409471244*true";    //저고혈압(있음)
        public const string MUNJIN51 = ",radiobox_1581409560268*true";    //호흡기질환(없음)
        public const string MUNJIN52 = ",radiobox_1581409567516*true";    //호흡기질환(있음)
        public const string MUNJIN61 = ",radiobox_1581409575700*true";    //심장질환(없음)
        public const string MUNJIN62 = ",radiobox_1581409583029*true";    //심장질환(있음)
        public const string MUNJIN71 = ",radiobox_1581409590845*true";    //신장질환(없음)
        public const string MUNJIN72 = ",radiobox_1581409629453*true";    //신장질환(있음)
        public const string MUNJIN81 = ",radiobox_1581409636763*true";    //출혈소인(없음)
        public const string MUNJIN82 = ",radiobox_1581409644277*true";    //출혈소인(있음)
        public const string MUNJIN91 = ",radiobox_1581409654461*true";    //약물중독(없음)
        public const string MUNJIN92 = ",radiobox_1581409661188*true";    //약물중독(있음)
        public const string MUNJIN01 = ",radiobox_1613715612949*true";    //치아상태(없음)
        public const string MUNJIN02 = ",radiobox_1613715671801*true";    //치아상태(있음)

        public const string ENDO11 = ",radiobox_1581410239606*true";    //수면내시경여부(예)
        public const string ENDO12 = ",radiobox_1581410249542*true";    //수면내시경여부(아니오)


        /// <summary>
        /// HaMain.bas
        /// </summary>
        public struct HaMain_Table_Tab_Columns
        {
            public static int ColID;
            public static string ColName;
            public static string DataType;
            public static int DataLen;
            public static int DataScale;
            public static string DataNull;
        }
        public HaMain_Table_Tab_Columns[] TCOL; //[300]

        /// <summary>
        /// 종검환자 마스터
        /// </summary>
        public struct HaMain_HIC_PATIENT_INFO
        {
            public static long Pano;
            public static string sName;
            public static string Jumin;
            public static string Sex;
            public static string BirthDay;
            public static string GBBIRTH;
            public static string MAILCODE;
            public static string JUSO1;
            public static string JUSO2;
            public static string Tel;
            public static string HPhone;
            public static string GBSMS;
            public static string LTDCODE;
            public static string BUSENAME;
            public static string SABUN;
            public static string LTDTEL;
            public static string GAMCODE;
            public static string RELIGION;
            public static string STARTDATE;
            public static string LASTDATE;
            public static int JINCOUNT;
            public static string PtNo;
            public static string Remark;
            public static string TEL_CONFIRM;
            public static string Familly;
            public static string GbPrivacy;
            public static string VipRemark;
            public static string GbJikwon;
            public static string Sosok;
            public static string BuildNo;
            public static string ROWID;
        }
        public HaMain_HIC_PATIENT_INFO HaPatient;

        /// <summary>
        /// 종검할인내역
        /// </summary>
        public struct HaMain_HEA_GAMCODE_INFO
        {
            public string sName;
            public long Pano;
            public string sDate;
            public string ActDate;
            public string GCode;
            public string GCodeName;
            public string BuRate;
            public long AMT;
            public long GamAmt;
            //public bool flag;
        }
        public static HaMain_HEA_GAMCODE_INFO[] HaGam; //[20]
        public static bool HaGamFlag;

        /// <summary>
        /// HcMain.bas TABLE_FIRST_AutoPanjeng
        /// </summary>
        public struct HcMain_First_AutoPanjeng
        {
            public string Panjeng;
            public bool[] PanB;     //[9]
            public bool[] PanC;     //[4]
            public bool[] PanR;     //[11]
            public bool[] PanU;     //[3]
            public string Sogen;
            public string SogenB;
            public string SogenC;
            public string SogenD;
            public int Liver;
        }    
        public static HcMain_First_AutoPanjeng TFA;

        public static void HcMain_First_AutoPanjeng_Clear()
        {
            TFA.Panjeng = "";  
            TFA.PanB = new bool[10];
            TFA.PanC = new bool[5];
            TFA.PanR = new bool[12];
            TFA.PanU = new bool[4];
            TFA.Sogen = "";
            TFA.SogenB = "";
            TFA.SogenC = "";
            TFA.SogenD = "";
            TFA.Liver = 0;
        }

        public struct HC_PATIENT_INFO
        {
            public long      PANO;
            public string    SNAME;
            public string    JUMIN;
            public string    SEX;
            public string    MAILCODE;
            public string    JUSO1;
            public string    JUSO2;
            public string    TEL;
            public long      LTDCODE;
            public string    JIKGBN;
            public string    JIKJONG;
            public string    SABUN;
            public string    BUSENAME;
            public string    GONGJENG;
            public string    IPSADATE;
            public string    BUSEIPSA;
            public string    JISA;
            public string    GKIHO;
            public string    GBSUCHEP;
            public string    PTNO;
            public string    REMARK;
            public string    KIHO;
            public string    BOGUNSO;
            public string    YOUNGUPSO;
            public string    LIVER2;
            public string    GUMDAESANG;
            public string    EMAIL;
            public string    HPHONE;
            public string    GBIEMUNJIN;
            public string    GBSMS;
            public string    TEL_CONFIRM;
            public string    JUMIN2;
            public string    GBPRIVACY;
            public string    BUILDNO;
            public string    LTDCODE2;
            public string    BIRTHDAY;
            public string    GBBIRTH;
            public string    LTDTEL;
            public string    GAMCODE;
            public string    RELIGION;
            public string    STARTDATE;
            public string    LASTDATE;
            public long      JINCOUNT;
            public string    FAMILLY;
            public string    GAMCODE2;
            public string    SOSOK;
            public string    VIPREMARK;
            public string    GBJIKWON;
        }
        public static HC_PATIENT_INFO HPI;

        public static void HCPAT_CLEAR()
        {
            HPI.PANO        = 0;
            HPI.SNAME       = "";
            HPI.JUMIN       = "";
            HPI.SEX         = "";
            HPI.MAILCODE    = "";
            HPI.JUSO1       = "";
            HPI.JUSO2       = "";
            HPI.TEL         = "";
            HPI.LTDCODE     = 0;
            HPI.JIKGBN      = "";
            HPI.JIKJONG     = "";
            HPI.SABUN       = "";
            HPI.BUSENAME    = "";
            HPI.GONGJENG    = "";
            HPI.IPSADATE    = null;
            HPI.BUSEIPSA    = null;
            HPI.JISA        = "";
            HPI.GKIHO       = "";
            HPI.GBSUCHEP    = "";
            HPI.PTNO        = "";
            HPI.REMARK      = "";
            HPI.KIHO        = "";
            HPI.BOGUNSO     = "";
            HPI.YOUNGUPSO   = "";
            HPI.LIVER2      = "";
            HPI.GUMDAESANG  = "";
            HPI.EMAIL       = "";
            HPI.HPHONE      = "";
            HPI.GBIEMUNJIN  = "";
            HPI.GBSMS       = "";
            HPI.TEL_CONFIRM = "";
            HPI.JUMIN2      = "";
            HPI.GBPRIVACY   = null;
            HPI.BUILDNO     = "";
            HPI.LTDCODE2    = "";
            HPI.BIRTHDAY    = "";
            HPI.GBBIRTH     = "";
            HPI.LTDTEL      = "";
            HPI.GAMCODE     = "";
            HPI.RELIGION    = "";
            HPI.STARTDATE   = null;
            HPI.LASTDATE    = null;
            HPI.JINCOUNT    = 0;
            HPI.FAMILLY     = "";
            HPI.GAMCODE2    = "";
            HPI.SOSOK       = "";
            HPI.VIPREMARK   = "";
            HPI.GBJIKWON    = "";
        }

        /// <summary>
        /// HcMain_Exam.bas > TABLE_Exam_Chk
        /// </summary>
        public struct TABLE_EXAM_CHK
        {
            public bool    USE;
            public bool    PASSCHK;
            public bool    AUTOCHK;
            public string  JONG;
            public string  PANO;
            public string  JEPDATE;
            public long    WRTNO;
            public string  PTNO;
            public string  SNAME;
            public string  SEX;
            public int     AGE;
            public string  JUMIN1;
            public string  JUMIN2;
            public string  JUMINFULL;
            public string  GUBUN;    //A:검진+구강 B:급여
            public string  SELCODE;  //선택검사
                             
            public string  NHICUSE;  //연동여부
            public string  NHICOK;   //Y/N
            public string  REL;      //자격명칭
            public string  GKIHO;    //증번호
            public string  EXAMA;    //이상지질혈증
            public string  EXAMB;    //B형간염 자격
            public string  EXAMC;    //C형간염
            public string  EXAMD;    //골밀도
            public string  EXAME;    //인지기능장애
            public string  EXAMF;    //정신건강검사
            public string  EXAMG;    //생활습관평가
            public string  EXAMH;    //노인신체기능
            public string  EXAMI;    //치면세균막
            public string  EXAMJ;    //
            public string  EXAMK;    //
            public string  EXAML;    //
                             
            public string  EXAMA1;   //이상지질혈증 대상
            public string  EXAMB1;   //B형간염 자격
            public string  EXAMC1;   //C형간염
            public string  EXAMD1;   //골밀도
            public string  EXAME1;   //인지기능장애
            public string  EXAMF1;   //정신건강검사
            public string  EXAMG1;   //생활습관평가
            public string  EXAMH1;   //노인신체기능
            public string  EXAMI1;   //치면세균막
            public string  EXAMJ1;   //
            public string  EXAMK1;   //
            public string  EXAML1;   //

            public string  EXAMA2;   //이상지질혈증 자격
            public string  EXAMB2;   //B형간염 자격
            public string  EXAMC2;   //C형간염
            public string  EXAMD2;   //골밀도
            public string  EXAME2;   //인지기능장애
            public string  EXAMF2;   //정신건강검사
            public string  EXAMG2;   //생활습관평가
            public string  EXAMH2;   //노인신체기능
            public string  EXAMI2;   //치면세균막
            public string  EXAMJ2;   //
            public string  EXAMK2;   //
            public string  EXAML2;   //
        }
        public static TABLE_EXAM_CHK TEC;

        public static void TEC_CLEAR(string argGubun)
        {
            if (argGubun == "ALL")
            {
                TEC.USE = false;
                TEC.PASSCHK = false;
                TEC.AUTOCHK = false;
                TEC.JONG = "";
                TEC.PANO = "";
                TEC.JEPDATE = "";
                TEC.WRTNO = 0;
                TEC.PTNO = "";
                TEC.SNAME = "";
                TEC.SEX = "";
                TEC.AGE = 0;
                TEC.JUMIN1 = "";
                TEC.JUMIN2 = "";
                TEC.JUMINFULL = "";
                TEC.GUBUN = "";
                TEC.SELCODE = "";

                TEC.NHICUSE = "";
                TEC.NHICOK = "";
                TEC.REL = "";
                TEC.GKIHO = "";
                TEC.EXAMA = "";
                TEC.EXAMB = "";
                TEC.EXAMC = "";
                TEC.EXAMD = "";
                TEC.EXAME = "";
                TEC.EXAMF = "";
                TEC.EXAMG = "";
                TEC.EXAMH = "";
                TEC.EXAMI = "";
                TEC.EXAMJ = "";
                TEC.EXAMK = "";
                TEC.EXAML = "";

                TEC.EXAMA1 = "";
                TEC.EXAMB1 = "";
                TEC.EXAMC1 = "";
                TEC.EXAMD1 = "";
                TEC.EXAME1 = "";
                TEC.EXAMF1 = "";
                TEC.EXAMG1 = "";
                TEC.EXAMH1 = "";
                TEC.EXAMI1 = "";
                TEC.EXAMJ1 = "";
                TEC.EXAMK1 = "";
                TEC.EXAML1 = "";

                TEC.EXAMA2 = "";
                TEC.EXAMB2 = "";
                TEC.EXAMC2 = "";
                TEC.EXAMD2 = "";
                TEC.EXAME2 = "";
                TEC.EXAMF2 = "";
                TEC.EXAMG2 = "";
                TEC.EXAMH2 = "";
                TEC.EXAMI2 = "";
                TEC.EXAMJ2 = "";
                TEC.EXAMK2 = "";
                TEC.EXAML2 = "";
            }
            else if (argGubun == "PART1")
            {
                TEC.EXAMA1 = "";                  //이상지질혈증
                TEC.EXAMB1 = "";                  //B형간염 자격
                TEC.EXAMC1 = "";                  //C형간염
                TEC.EXAMD1 = "";                  //골밀도
                TEC.EXAME1 = "";                  //인지기능장애
                TEC.EXAMF1 = "";                  //정신건강검사
                TEC.EXAMG1 = "";                  //생활습관평가
                TEC.EXAMH1 = "";                  //노인신체기능
                TEC.EXAMI1 = "";                  //치면세균막
                TEC.EXAMJ1 = "";                  //
                TEC.EXAMK1 = "";                  //
                TEC.EXAML1 = "";                  //
                               ;                  //
                               ;                  //
                TEC.EXAMA2 = "";                  //이상지질혈증
                TEC.EXAMB2 = "";                  //B형간염 자격
                TEC.EXAMC2 = "";                  //C형간염
                TEC.EXAMD2 = "";                  //골밀도
                TEC.EXAME2 = "";                  //인지기능장애
                TEC.EXAMF2 = "";                  //정신건강검사
                TEC.EXAMG2 = "";                  //생활습관평가
                TEC.EXAMH2 = "";                  //노인신체기능
                TEC.EXAMI2 = "";                  //치면세균막
                TEC.EXAMJ2 = "";                  //
                TEC.EXAMK2 = "";                  //
                TEC.EXAML2 = "";                  //
            }
        }

        public struct TABLE_HIC_NHIC_VIEW
        {
            //기본정보
            public string hSName;       //성명
            public string hJumin;       //주민번호
            public string hJaGubun;     //자격구분
            public string Year;         //사업년도
            public string hJaSTS;       //자격상태
            public string hGKiho;       //증기호
            public string hKiho;        //회사기호
            public string hJisa;        //소속지사
            public string hGetDate;     //취득일자
            public string hBogen;       //관할보건소
                             
            //검사정보       
            public string h1Cha;        //1차검진
            public string hDental;      //구강검진
            public string hEkg;         //심전도
            public string hLiver;       //B간염검사
            public string hLiverC;      //C간염검사
            public string hFirstAdd;    //1차추가검진
            public string h2ChaB;       //2차B형
            public string h2Cha;        //2차검진
            public string hCan1;        //암-위
            public string hCan12;       //암-위 치료비지원
            public string hCan2;        //암-유방
            public string hCan22;       //암-유방 치료비지원
            public string hCan3;        //암-대장
            public string hCan32;       //암-대장 치료비지원
            public string hCan4;        //암-간(상반기)
            public string hCan42;       //암-간(상반기) 치료비지원
            public string hCan5;        //암-자궁
            public string hCan52;       //암-자궁 치료비지원
            public string hCan6;        //암-간(하반기)
            public string hCan62;       //암-간(하반기) 치료비지원
            public string hCan7;        //암-폐
            public string hCan72;       //암-폐 치료비지원

            //수검정보       
            public string h1ChaDate;    //1차검진일자
            public string h1ChaHName;   //1차검진병원명
            public string h2ChaDate;    //2차검진일자
            public string h2ChaHName;   //2차검진병원명
            public string hDentDate;    //구강진일자
            public string hDentHName;   //구강검진병원명
                             
            public string h위Date;      //위검진일
            public string h위HName;     //위검진병원명
            public string h대장Date;    //검진일
            public string h대장HName;   //검진병원명
            public string h유방Date;    //검진일
            public string h유방HName;   //검진병원명
            public string h자궁Date;    //검진일
            public string h자궁HName;   //검진병원명
            public string h간Date;      //검진일
            public string h간HName;     //검진병원명
            public string h폐Date;      //검진일
            public string h폐HName;     //검진병원명
        }
        public static TABLE_HIC_NHIC_VIEW THNV;

        public static void THNV_CLEAR()
        {
            THNV.hSName = "";
            THNV.hJumin = "";
            THNV.hJaGubun = "";
            THNV.Year = "";
            THNV.hJaSTS = "";
            THNV.hGKiho = "";
            THNV.hKiho = "";
            THNV.hJisa = "";
            THNV.hGetDate = "";
            THNV.hBogen = "";
            
            THNV.h1Cha = "";
            THNV.hDental = "";
            THNV.hEkg = "";
            THNV.hLiver = "";
            THNV.hLiverC = "";
            THNV.hFirstAdd = "";
            THNV.h2ChaB = "";
            THNV.h2Cha = "";
            THNV.hCan1 = "";
            THNV.hCan2 = "";
            THNV.hCan3 = "";
            THNV.hCan4 = "";
            THNV.hCan5 = "";
            THNV.hCan6 = "";
            THNV.hCan7 = "";
            THNV.hCan12 = "";
            THNV.hCan22 = "";
            THNV.hCan32 = "";
            THNV.hCan42 = "";
            THNV.hCan52 = "";
            THNV.hCan62 = "";
            THNV.hCan72 = "";

            THNV.h1ChaDate = "";
            THNV.h1ChaHName = "";
            THNV.h2ChaDate = "";
            THNV.h2ChaHName = "";
            THNV.hDentDate = "";
            THNV.hDentHName = "";
            
            THNV.h위Date = "";
            THNV.h위HName = "";
            THNV.h대장Date = "";
            THNV.h대장HName = "";
            THNV.h유방Date = "";
            THNV.h유방HName = "";
            THNV.h자궁Date = "";
            THNV.h자궁HName = "";
            THNV.h간Date = "";
            THNV.h간HName = "";
            THNV.h폐Date = "";
            THNV.h폐HName = "";
        }

        public struct HIC_SUNAP_INFO
        {
            public long GWRTNO;
            public long WRTNO;
            public long TotalAmt;
            public long JohapAmt;
            public long LtdAmt;
            public long BoninAmt;
            public long MisuAmt;
            public long GamAmt;
            public long ResvAmt;
            public long BogenAmt;
            public string MisuGye;
            public string GamGye;
            public long ChaAmt;
            public long CardAmt;
            public long CashAmt;
            public string JEPBUN;
        }
        public static HIC_SUNAP_INFO[] HSI = new HIC_SUNAP_INFO[5];

        public struct TABLE_EXAM_ORDER
        {
            public string Pano;
            public string Bi;
            public string sName;
            public string IpdOpd;
            public long Age;
            public string AgeMM;
            public string Sex;
            public string DeptCode;
            public string Ward;
            public string DrCode;
            public int Room;
            public string BDate;
            public long HICNO;
            public string Job;  //외래,메뉴얼,건진,입원
            public int OrderCNT;
            public string[] Order;  //size 80
        }
        public static TABLE_EXAM_ORDER TORD;

        public static void TORD_CLEAR()
        {
            TORD.Pano = "";
            TORD.Bi = "";
            TORD.sName = "";
            TORD.IpdOpd = "";
            TORD.Age = 0;
            TORD.AgeMM = "";
            TORD.Sex = "";
            TORD.DeptCode = "";
            TORD.Ward = "";
            TORD.DrCode = "";
            TORD.Room = 0; ;
            TORD.BDate = null;
            TORD.HICNO = 0; ;
            TORD.Job = "";  //외래,메뉴얼,건진,입원
            TORD.OrderCNT = 0;
            TORD.Order = null;  //size 80
        }

        public struct TABLE_MIR_Bohum
        {
            public long MIRNO;
            public string Year;
            public string Gubun;
            public string Johap;
            public string Kiho;
            public string JepNo;
            public long JepQty;
            public long TAmt;
            public string JepDate;
            public long ONE_Qty;
            public long ONE_TAmt;
            public long[] ONE_Inwon;    //size 50
            public long TWO_Qty;
            public long TWO_TAmt;
            public long[] TWO_Inwon;    //size 50
            public string IpGumDate;
            public long IpGumAmt;
            public string GbJohap;
            public string FrDate;
            public string ToDate;
            public string BuildDate;
            public long BuildSabun;
            public long BuildCnt;
            public string GbErrChk;
            public string CHASU;
            public string Life_Gbn;
            public string ROWID;
        }
        public static TABLE_MIR_Bohum TMB;

        //암검진
        public struct TABLE_MIR_Cancer
        {
            public long MIRNO;
            public string Year;
            public string Gubun;
            public string Johap;
            public string Kiho;
            public string JepNo;
            public long JepQty;
            public long HuQty;
            public long TAmt;
            public string JepDate;
            public long[] Inwon;    //size 50
            public string IpGumDate;
            public long IpGumAmt;
            public string GbJohap;
            public string FrDate;
            public string ToDate;
            public string GbBogun;
            public string MirGbn;
            public string BuildDate;
            public long BuildSabun;
            public long BuildCnt;
            public string GbErrChk;
            public string FileName;
            public string Life_Gbn;
            public string ROWID;
        }
        public static TABLE_MIR_Cancer TMC;

        //암검진-보건소
        public struct TABLE_MIR_Cancer_Bo
        {
            public long MIRNO;
            public string Year;
            public string Gubun;
            public string Johap;
            public string Kiho;
            public string JepNo;
            public long JepQty;
            public long HuQty;
            public long TAmt;
            public string JepDate;
            public long[] Inwon;        //size 50
            public string IpGumDate;
            public int IpGumAmt;
            public string GbJohap;
            public string FrDate;
            public string ToDate;
            public string MirGbn;
            public string BuildDate;
            public long BuildSabun;
            public long BuildCnt;
            public string GbErrChk;
            public string FileName;
            public string Life_Gbn;
            public string ROWID;
        }
        public static TABLE_MIR_Cancer TMCB;

        //구강검진
        public struct TABLE_MIR_Dental
        {
            public long MIRNO;
            public string Year;
            public string Gubun;
            public string Johap;
            public string Kiho;
            public string JepNo;
            public long JepQty;
            public long HuQty;
            public long TAmt;
            public string JepDate;
            public string IpGumDate;
            public long IpGumAmt;
            public string GbJohap;
            public string FrDate;
            public string ToDate;
            public string BuildDate;
            public long BuildSabun;
            public long BuildCnt;
            public string GbErrChk;
            public string Life_Gbn;
            public string ROWID;
        }
        public static TABLE_MIR_Dental TMD;

        //건강보험 1차 검사결과 및 문진
        
        public struct TABLE_RES_BOHUM1
        {
            public long WRTNO;
            public int Height;
            public int Weight;
            public int Waist;
            public int BimanRate;
            public string Biman;
            public double EYE_L;
            public double EYE_R;
            public string EAR_L;
            public string EAR_R;
            public int BLOOD_H;
            public int BLOOD_L;
            public string URINE1;
            public string URINE2;
            public string URINE3;
            public double URINE4;
            public double BLOOD1;
            public int BLOOD2;
            public int BLOOD3;
            public int BLOOD4;
            public int BLOOD5;
            public int BLOOD6;
            public string LIVER1;
            public string LIVER2;
            public string LIVER3;
            public string XRayGbn;
            public string XRayRes;
            public string EKG;
            public int FOOT1;
            public int FOOT2;
            public string FOOT3;
            public int BALANCE;
            public int OSTEO;
            public string WOMB01;
            public string WOMB02;
            public string WOMB03;
            public string WOMB04;
            public string WOMB05;
            public string WOMB06;
            public string WOMB07;
            public string WOMB08;
            public string WOMB09;
            public string WOMB10;
            public string WOMB11;
            public string OLDBYENG1;     //과거병력
 
            public string[] OLDBYENG;    //과거병력
            public string[]HABIT;  //생활습관
            public string[]JINCHAL;    //진찰소견
            public string Panjeng;     //종합판정
            public string[]PanjengB;    //정상B
            public string PanjengB_Etc;     //정상B 기타
            public string[]PanjengR;   //질환의심(R)
            public string PanjengR_Etc;     //질환의심 기타
            public string PanjengEtc;     //기타질환
            public string PanjengDate;     //판정일
            public long PanDrNo;   //의사면허번호
            public string Sogen;     //소견및조치사항
            public string TongboGbn;     //통보방법
            public string TongboDate;     //통보일자
            public string IpsaDate;     //입사일자
            //문진표 항목           
            public string Sik11;
            public string Sik12;
            public string Sik13;
            public string Sik21;
            public string Sik22;
            public string Sik23;
            public string Sik31;
            public string Sik32;
            public string Sik33;
            public string[] Gajok;
            public string T_STAT01;
            public string T_STAT02;
            public string T_STAT11;
            public string T_STAT12;
            public string T_STAT21;
            public string T_STAT22;
            public string T_STAT31;
            public string T_STAT32;
            public string T_STAT41;
            public string T_STAT42;
            public string T_STAT51;
            public string T_STAT52;
            public string T_GAJOK1;
            public string T_GAJOK2;
            public string T_GAJOK3;
            public string T_GAJOK4;
            public string T_GAJOK5;
            public string T_BLIVER;
            public string T_SMOKE1;
            public long T_SMOKE2;
            public long T_SMOKE3;
            public long T_SMOKE4;
            public long T_SMOKE5;
            public string T_DRINK1;
            public long T_DRINK2;
            public string T_ACTIVE1;
            public string T_ACTIVE2;
            public string T_ACTIVE3;
            public string T40_FEEL1;
            public string T40_FEEL2;
            public string T40_FEEL3;
            public string T40_FEEL4;
            public string T66_INJECT;
            public string T66_STAT1;
            public string T66_STAT2;
            public string T66_STAT3;
            public string T66_STAT4;
            public string T66_STAT5;
            public string T66_STAT6;
            public string T66_FEEL1;
            public string T66_FEEL2;
            public string T66_FEEL3;
            public string T66_MEMORY1;
            public string T66_MEMORY2;
            public string T66_MEMORY3;
            public string T66_MEMORY4;
            public string T66_MEMORY5;
            public string T66_FALL;
            public string T66_URO;
            public string PANJENGC1;
            public string PANJENGC2;
            public string PANJENGC3;
            public string PANJENGC4;
            public string PANJENGC5;
            public string PANJENGD11;
            public string PANJENGD12;
            public string PANJENGD13;
            public string PANJENGD21;
            public string PANJENGD22;
            public string PANJENGD23;
            public string PANJENGU1;
            public string PANJENGU2;
            public string PANJENGU3;
            public string PANJENGU4;
            public string PANJENGSAHU;

            //Hcfile.exe > HcBill 추가내용
            public string TMUN0103;
            public string SIM_RESULT1;
            public string SIM_RESULT2;
            public string SIM_RESULT3;

            public string GbGonghu;//토.공휴일 가산여부
            public string SLIP_BIMAN;//비만처방전
            public string ROWID;
        }
        public static TABLE_RES_BOHUM1 B1;

        public static void B1_CLEAR()
        {
            B1.WRTNO = 0;
            B1.Height = 0;
            B1.Weight = 0;
            B1.Waist = 0;
            B1.BimanRate = 0;
            B1.Biman = "";
            B1.EYE_L = 0;
            B1.EYE_R = 0;
            B1.EAR_L = "";
            B1.EAR_R = "";
            B1.BLOOD_H = 0;
            B1.BLOOD_L = 0;
            B1.URINE1 = "";
            B1.URINE2 = "";
            B1.URINE3 = "";
            B1.URINE4 = 0.0;
            B1.BLOOD1=0.0;
            B1.BLOOD2 = 0;
            B1.BLOOD3=0;
            B1.BLOOD4=0;
            B1.BLOOD5=0;
            B1.BLOOD6=0;
            B1.LIVER1="";
            B1.LIVER2="";
            B1.LIVER3 = "";
            B1.XRayGbn="";
            B1.XRayRes = "";
            B1.EKG= "";
            B1.FOOT1 = 0;
            B1.FOOT2 = 0;
            B1.FOOT3="";
            B1.BALANCE = 0;
            B1.OSTEO = 0;
            B1.WOMB01 = "";
            B1.WOMB02 = "";
            B1.WOMB03 = "";
            B1.WOMB04 = "";
            B1.WOMB05 = "";
            B1.WOMB06 = "";
            B1.WOMB07 = "";
            B1.WOMB08 = "";
            B1.WOMB09 = "";
            B1.WOMB10 = "";
            B1.WOMB11 = "";
            B1.OLDBYENG1 = "";     //과거병력
            B1.OLDBYENG = new string[7];  //과거병력
            B1.HABIT = new string[5]; //생활습관
            B1.JINCHAL = new string[2]; //진찰소견
            B1.Panjeng="";     //종합판정
            B1.PanjengB = new string[9];  //정상B
            B1.PanjengR = new string[12];  //질환의심(R)
            B1.PanjengB_Etc = "";     //정상B 기타
            B1.PanjengR_Etc = "";     //질환의심 기타
            B1.PanjengEtc = "";     //기타질환
            B1.PanjengDate = "";     //판정일
            B1.PanDrNo = 0;   //의사면허번호
            B1.Sogen = "";     //소견및조치사항
            B1.TongboGbn = "";     //통보방법
            B1.TongboDate = "";     //통보일자
            B1.IpsaDate="";     //입사일자

            //문진표 항목           
            B1.Sik11 = "";
            B1.Sik12 = "";
            B1.Sik13 = "";
            B1.Sik21 = "";
            B1.Sik22 = "";
            B1.Sik23 = "";
            B1.Sik31 = "";
            B1.Sik32 = "";
            B1.Sik33 = "";
            //public string[] Gajok;
            B1.Gajok = new string[6];
            B1.T_STAT01 = "";
            B1.T_STAT02 = "";
            B1.T_STAT11 = "";
            B1.T_STAT12 = "";
            B1.T_STAT21 = "";
            B1.T_STAT22 = "";
            B1.T_STAT31 = "";
            B1.T_STAT32 = "";
            B1.T_STAT41 = "";
            B1.T_STAT42 = "";
            B1.T_STAT51 = "";
            B1.T_STAT52 = "";
            B1.T_GAJOK1 = "";
            B1.T_GAJOK2 = "";
            B1.T_GAJOK3 = "";
            B1.T_GAJOK4 = "";
            B1.T_GAJOK5 = "";
            B1.T_BLIVER="";
            B1.T_SMOKE1 = "";
            B1.T_SMOKE2 = 0;
            B1.T_SMOKE3 = 0;
            B1.T_SMOKE4 = 0;
            B1.T_SMOKE5 = 0;
            B1.T_DRINK1 = "";
            B1.T_DRINK2 = 0;
            B1.T_ACTIVE1="";
            B1.T_ACTIVE2="";
            B1.T_ACTIVE3="";
            B1.T40_FEEL1="";
            B1.T40_FEEL2="";
            B1.T40_FEEL3="";
            B1.T40_FEEL4 = "";
            B1.T66_INJECT = "";
            B1.T66_STAT1 = "";
            B1.T66_STAT2 = "";
            B1.T66_STAT3 = "";
            B1.T66_STAT4 = "";
            B1.T66_STAT5 = "";
            B1.T66_STAT6 = "";
            B1.T66_FEEL1 = "";
            B1.T66_FEEL2 = "";
            B1.T66_FEEL3 = "";
            B1.T66_MEMORY1 = "";
            B1.T66_MEMORY2 = "";
            B1.T66_MEMORY3 = "";
            B1.T66_MEMORY4 = "";
            B1.T66_MEMORY5 = "";
            B1.T66_FALL = "";
            B1.T66_URO = "";
            B1.PANJENGC1 = "";
            B1.PANJENGC2 = "";
            B1.PANJENGC3 = "";
            B1.PANJENGC4 = "";
            B1.PANJENGC5 = "";
            B1.PANJENGD11 = "";
            B1.PANJENGD12 = "";
            B1.PANJENGD13 = "";
            B1.PANJENGD21 = "";
            B1.PANJENGD22 = "";
            B1.PANJENGD23 = "";
            B1.PANJENGU1 = "";
            B1.PANJENGU2 = "";
            B1.PANJENGU3 = "";
            B1.PANJENGU4 = "";
            B1.PANJENGSAHU = "";
            B1.GbGonghu = "";//토.공휴일 가산여부
            B1.SLIP_BIMAN = "";//비만처방전
            B1.ROWID = "";
        }

        //건강보험 2차 검사결과 및 문진
        public struct TABLE_RES_BOHUM2
        {
            public long WRTNO;
            public string GbChest;
            public string GbCycle;
            public string GbGoji;         
            public string GbLiver;        
            public string GbKidney;       
            public string GbAnemia;       
            public string GbDiabetes;     
            public string GbEtc;          
            public string Chest1;         
            public string Chest2;         
            public string Chest3;         
            public string Chest_RES;      
            public string Cycle1;         
            public string Cycle2;         
            public string Cycle3;         
            public string Cycle4;
            public string Cycle_RES;        
            public string Goji1;            
            public string Goji2;            
            public string Goji_RES;         
            public string Liver11;          
            public string Liver12;          
            public string Liver13;          
            public string Liver14;          
            public string Liver15;          
            public string Liver16;          
            public string Liver17;          
            public string Liver18;          
            public string Liver19;          
            public string Liver20;          
            public string Liver21;          
            public string Liver22;
            public string Liver_Res;      
            public string Kidney1;        
            public string Kidney2;        
            public string Kidney3;        
            public string Kidney4;        
            public string Kidney5;        
            public string Kidney_Res;     
            public string Anemia1;        
            public string Anemia2;        
            public string Anemia3;
            public string Anemia_Res;
            public string Diabetes1;
            public string Diabetes2;
            public string Diabetes3;
            public string Diabetes_Res;
            public string Etc_Res;
            public string Etc_Exam;
            public string Sogen;
            public string Panjeng;
            public string Panjeng_D1;
            public string Panjeng_D11;
            public string Panjeng_D12;
            public string PANJENG_SO1;
            public string PANJENG_SO2;
            public string PANJENG_SO3;
            public string PanjengDate;
            public string TongboGbn;
            public string TongboDate;
            public long PanjengDrNo;
            public string GunDate;
            public string GbPrint;         
            public string WorkYN;
            public string DIABETES_RES_CARE;
            public string CYCLE_RES_CARE;
            public string T66_MEM;
            public string T_SangDam_1;//생애1차소견
            public string GbGonghu;//토.공휴일 가산여부
            public string ROWID;
        }
        public static TABLE_RES_BOHUM2 B2;

        public static void B2_CLEAR()
        {
            B2.WRTNO = 0;
            B2.GbChest="";
            B2.GbCycle="";
            B2.GbGoji="";
            B2.GbLiver="";
            B2.GbKidney="";
            B2.GbAnemia="";
            B2.GbDiabetes="";
            B2.GbEtc="";
            B2.Chest1="";
            B2.Chest2="";
            B2.Chest3="";
            B2.Chest_RES="";
            B2.Cycle1="";
            B2.Cycle2="";
            B2.Cycle3="";
            B2.Cycle4="";
            B2.Cycle_RES="";
            B2.Goji1="";
            B2.Goji2="";
            B2.Goji_RES="";
            B2.Liver11="";
            B2.Liver12="";
            B2.Liver13="";
            B2.Liver14="";
            B2.Liver15="";
            B2.Liver16="";
            B2.Liver17="";
            B2.Liver18="";
            B2.Liver19="";
            B2.Liver20="";
            B2.Liver21="";
            B2.Liver22="";
            B2.Liver_Res="";
            B2.Kidney1="";
            B2.Kidney2="";
            B2.Kidney3="";
            B2.Kidney4="";
            B2.Kidney5="";
            B2.Kidney_Res="";
            B2.Anemia1="";
            B2.Anemia2="";
            B2.Anemia3="";
            B2.Anemia_Res="";
            B2.Diabetes1="";
            B2.Diabetes2="";
            B2.Diabetes3="";
            B2.Diabetes_Res="";
            B2.Etc_Res="";
            B2.Etc_Exam="";
            B2.Sogen="";
            B2.Panjeng="";
            B2.Panjeng_D1="";
            B2.Panjeng_D11="";
            B2.Panjeng_D12="";
            B2.PANJENG_SO1="";
            B2.PANJENG_SO2="";
            B2.PANJENG_SO3="";
            B2.PanjengDate="";
            B2.TongboGbn="";
            B2.TongboDate="";
            B2.PanjengDrNo = 0;
            B2.GunDate="";
            B2.GbPrint="";
            B2.WorkYN="";
            B2.DIABETES_RES_CARE="";
            B2.CYCLE_RES_CARE="";
            B2.T66_MEM="";
            B2.T_SangDam_1="";
            B2.GbGonghu="";
            B2.ROWID = "";
        }

        //암 검사결과
        public struct TABLE_RES_CANCER
        {
            public long WRTNO;
            public string Stomach_S;          		//위장조영촬영 병형
            public string Stomach_B;          		//위장조영촬영 병형 기타
            public string Stomach_P;          		//위장조영촬영 부위
            public string Stomach_PETC;             //위장조영촬영 부위 기타
            public string S_ENDOGBN;                //위경검사실시여부
            public string Stomach_SENDO;            //위내시경 병형
            public string Stomach_BENDO;            //위내시경 병형 기타
            public string Stomach_PENDO;            //위내시경 부위
            public string Stomach_ENDOETC;          //위내시경 부위 기타
            public string S_ANATGBN;                //조직검사여부
            public string S_ANAT;                   //위암조직검사결과
            public string S_ANATETC;                //위암조직검사결과 기타
            public string S_PANJENG;                //위암종합판정
            public string S_MONTH;                  //의심()개월후 재검대상
            public string S_JILETC;                 //기타질환()치료대상
            public string S_PLACE;                  //위암 검진장소 구분
            public string COLON_RESULT;             //분변잠혈 반응검사
            public string COLONGBN;                 //결장단순조영촬영, 내시경검사 실시여부
            public string COLON_S;                  //결장단순조영촬영 병형
            public string COLON_B;                  //결장단순조영촬영 병형 기타
            public string COLON_P;                  //결장단순조영촬영 부위
            public string COLON_ENDOGBN;            //결직장, S상결장경 검사방법
            public string COLON_SENDO;              //결직장, S상결장경 검사병형
            public string COLON_BENDO;              //결직장, S상결장경 검사병형 기타
            public string COLON_PENDO;              //결직장, S상결장경 검사부위
            public string COLON_PETC;
            public string COLON_ENDOETC;            //결직장, S상결장경 검사부위 기타
            public string C_ENDOGBN;                //결장단순조영촬영 내시경검사 여부
            public string C_ANATGBN;                //결직장, S상결장경 조직검사 여부
            public string C_ANAT;                   //장암 조직검사 결과
            public string C_ANATETC;                //대장암 조직검사 결과 기타
            public string C_PANJENG;                //대장암 종합판정
            public string C_MONTH;                  //의심()개월후 재검대상
            public string C_JILETC;                 //기타질환()치료대상
            public string C_PLACE;                  //대장암 검진장소 구분
            public string Liver_S;                  //간암 초음파 검사 병형
            public string Liver_B;                  //초음파 검사 병형 기타
            public string Liver_P;                  //초음파 병변 부위
            public string Liver_SIZE;               //초음파 병변 크기
            public string Liver_LSTYLE;           	//초음파 간암형
            public string Liver_VIOLATE;           	//초음파 문맥침범
            public string Liver_DISEASSE;           //초음파 만성간질환
            public string Liver_ETC;           		//초음파 기타 이상
            public string Liver_RPHA_EIA;           //알파위토단백 검사방법
            public string Liver_RPHA;           	//PHA법 검사결과
            public string Liver_EIA_S;           	//EIA법 검진기관 기준치
            public string Liver_EIA;           		//EIA법 검사결과
            public string Liver_PANJENG;           	//간암종합판정
            public string Liver_JILETC;           	//기타질환()치료대상
            public string Liver_PLACE;           	//간암 검진장소구분
            public string Liver_New_Alt;           	//간암 ALT
            public string Liver_New_B;           	//간암 B형간염항원 결과
            public string Liver_New_BRes;          	//간암 ALT 및 B형간염항원 결과
            public string Liver_New_C;           	//간암 C형간염항체
            public string Liver_New_CRes;           //간암 C형간염항체 결과
            public string BREAST_S;           		//유방암 단순촬영 병형
            public string BREAST_P;           		//단순촬영 부위
            public string BREAST_ETC;           	//단순촬영 부위 기타
            public string B_ANATGBN;           		//조직검사실시 여부
            public string B_ANAT;           		//조직검사결과
            public string B_ANATETC;           		//조직검사결과 기타
            public string B_PANJENG;           		//종합판정
            public string B_MONTH;           		//의심()개월후재검대상
            public string B_JILETC;          		//기타질환()치료대상
            public string B_PLACE;           		//유방암 검진장소구분
            public int Height;          			//신장
            public int Weight;          			//체중
            public string GBSTOMACH;           		//검사종류 위암
            public string GbLiver;           		//검사종류 간암
            public string GBRECTUM;           		//검사종류 대장암
            public string GBBREAST;           		//검사종류 유방암
            public string GbWomb;           		//검사종류 자궁경부암
            public string GBLUNG;           		//검진종류 폐암
            public string SICK11;           		//과거병력:위십이지장궤양
            public string SICK12;           		//발병년도
            public string SICK21;           		//과거병력:위수술(절제술) 유무
            public string SICK22;           		//발병년도
            public string SICK31;           		//과거병력:B형간염
            public string SICK32;           		//발병년도
            public string SICK41;           		//과거병력:간염여부
            public string SICK42;           		//발병년도
            public string SICK51;           		//과거병력:간경변
            public string SICK52;           		//발병년도
            public string SICK61;           		//과거병력:대장종
            public string SICK62;           		//발병년도
            public string SICK71;           		//과거병력:궤양성대장염
            public string SICK72;           		//발병년도
            public string SICK81;           		//과거병력:양성유방질환
            public string SICK82;           		//발병년도
            public string SICK91;           		//과거병력:산부인과질환
            public string SICK92;           		//발병년도
            public string JUNGSANG01;           	//자주발생하는증상 :불면증
            public string JUNGSANG02;           	//자주발생하는증상 :피로
            public string JUNGSANG03;           	//자주발생하는증상 :기침
            public string JUNGSANG04;           	//자주발생하는증상 :가래
            public string JUNGSANG05;           	//자주발생하는증상 :흉부통증
            public string JUNGSANG06;           	//자주발생하는증상 :현기증
            public string JUNGSANG07;           	//자주발생하는증상 :호흡곤란
            public string JUNGSANG08;           	//자주발생하는증상 :장복부통증
            public string JUNGSANG09;           	//자주발생하는증상 :복부팽만감
            public string JUNGSANG10;           	//자주발생하는증상 :소화불량
            public string JUNGSANG11;           	//자주발생하는증상 :설사
            public string JUNGSANG12;           	//자주발생하는증상 :변비
            public string JUNGSANG13;           	//자주발생하는증상 :혈변
            public string JUNGSANG14;           	//자주발생하는증상 :하복부통증
            public string JUNGSANG15;           	//자주발생하는증상 :체중변화
            public string GAJOK1;           		//가족력 유무
            public string GAJOKETC;         		//가족력 기타
            public string DRINK1;           		//음주습관
            public string DRINK2;           		//1회음주량
            public string SMOKING1;         		//흡연
            public string SMOKING2;         		//하루흡연량
            public string WOMAN1;                   //초경연령
            public string WOMAN2;                   //폐경여부
            public string WOMAN3;                   //폐경연령
            public string WOMAN4;                   //여성호르몬 투약여부
            public string WOMAN5;                   //여성호르몬 사용연령
            public string WOMAN6;                   //여성호르몬 사용기간
            public string WOMAN7;                   //출산여부
            public string WOMAN8;                   //출산횟수
            public string WOMAN9;                   //첫출산연령
            public string WOMAN10;                  //모유기간
            public string WOMAN11;                  //생리이상출혈 2005년적용 (1.예, 2.아니오)
            public string WOMAN12;                  //편상피세포이상-위험구분
            public string WOMAN13;                  //결혼연령 2005년적용
            //2005 자궁경부암 추가                  
            public string WOMB01;                   //자궁경부암 검체상태 (1.적절 ,2.부적절)
            public string WOMB02;                   //자궁경암 선상피세포 (1.유, 2.무)
            public string WOMB03;                   //자궁경부암 유형별진단(1.음성, 2.상피세포, 3.기타)
            public string WOMB04;                   //자궁경부암 편평상피 세포이상(참고사항 참조)
            public string WOMB05;                   //자궁경부암 선상피세포이상 (참고사항 참조)
            public string WOMB06;                   //자궁경부암 선상피세포이상 기타
            public string WOMB07;                   //자궁경부암 추가소견 (참고사항 참조)
            public string WOMB08;                   //자궁경부암 추가소견 기타
            public string WOMB09;                   //자궁경부암 종합판정 (참고사항 참조)
            public string WOMB10;                   //자궁경부암 의심()개월후 재검대상
            public string WOMB11;                   //자궁경부암 기타질환() 치료대상
            public string WOMB_PLACE;
            public string NEW_SICK01;
            public string NEW_SICK02;
            public string NEW_SICK03;
            public string NEW_SICK04;
            public string NEW_SICK06;
            public string NEW_SICK07;
            public string NEW_SICK08;
            public string NEW_SICK09;
            public string NEW_SICK11;
            public string NEW_SICK12;
            public string NEW_SICK13;
            public string NEW_SICK14;
            public string NEW_SICK16;
            public string NEW_SICK17;
            public string NEW_SICK18;
            public string NEW_SICK19;
            public string NEW_SICK20;
            public string NEW_SICK21;
            public string NEW_SICK22;
            public string NEW_SICK23;
            public string NEW_SICK24;
            public string NEW_SICK25;
            public string NEW_SICK26;
            public string NEW_SICK27;
            public string NEW_SICK28;
            public string NEW_SICK29;
            public string NEW_SICK30;
            public string NEW_SICK31;
            public string NEW_SICK32;
            public string NEW_SICK33;
            public string NEW_SICK34;
            public string NEW_SICK36;
            public string NEW_SICK37;
            public string NEW_SICK38;
            public string NEW_SICK39;
            public string NEW_SICK41;
            public string NEW_SICK42;
            public string NEW_SICK43;
            public string NEW_SICK44;
            public string NEW_SICK46;
            public string NEW_SICK47;
            public string NEW_SICK48;
            public string NEW_SICK49;
            public string NEW_SICK51;
            public string NEW_SICK52;
            public string NEW_SICK53;
            public string NEW_SICK54;
            public string NEW_SICK56;
            public string NEW_SICK57;
            public string NEW_SICK58;
            public string NEW_SICK59;
            public string NEW_SICK61;
            public string NEW_SICK62;
            public string NEW_SICK63;
            public string NEW_SICK64;
            public string NEW_SICK66;
            public string NEW_SICK67;
            public string NEW_SICK68;
            public string NEW_SICK69;
            public string NEW_SICK71;
            public string NEW_SICK72;
            public string NEW_SICK73;
            public string NEW_SICK74;
            public string NEW_B_SICK01;
            public string NEW_B_SICK02;
            public string NEW_B_SICK03;
            public string NEW_B_SICK04;
            public string NEW_B_SICK05;
            public string NEW_B_SICK06;
            public string NEW_N_SICK01;
            public string NEW_N_SICK02;
            public string NEW_N_SICK03;
            public string NEW_S_SICK01;
            public string NEW_S_SICK02;
            public string NEW_S_SICK03;
            public string NEW_S_SICK04;
            public string NEW_CAN_01;
            public string NEW_CAN_02;
            public string NEW_CAN_03;
            public string NEW_CAN_04;
            public string NEW_CAN_06;
            public string NEW_CAN_07;
            public string NEW_CAN_08;
            public string NEW_CAN_09;
            public string NEW_CAN_11;
            public string NEW_CAN_12;
            public string NEW_CAN_13;
            public string NEW_CAN_14;
            public string NEW_CAN_16;
            public string NEW_CAN_17;
            public string NEW_CAN_18;
            public string NEW_CAN_19;
            public string NEW_CAN_21;
            public string NEW_CAN_22;
            public string NEW_CAN_23;
            public string NEW_CAN_24;
            public string NEW_CAN_26;
            public string NEW_CAN_27;
            public string NEW_CAN_28;
            public string NEW_CAN_29;
            public string NEW_HARD;
            public string NEW_MARRIED;
            public string NEW_SCHOOL;
            public string NEW_WORK01;
            public string NEW_WORK02;
            public string NEW_SMOKE01;
            public string NEW_SMOKE02;
            public string NEW_SMOKE03;
            public string NEW_SMOKE04;
            public string NEW_SMOKE05;
            public string NEW_DRINK01;
            public string NEW_DRINK02;
            public string NEW_DRINK03;
            public string NEW_DRINK04;
            public string NEW_DRINK05;
            public string NEW_DRINK06;
            public string NEW_DRINK07;
            public string NEW_DRINK08;
            public string NEW_DRINK09;
            public string NEW_WOMAN01;
            public string NEW_WOMAN02;
            public string NEW_WOMAN03;
            public string NEW_WOMAN11;
            public string NEW_WOMAN12;
            public string NEW_WOMAN13;
            public string NEW_WOMAN14;
            public string NEW_WOMAN15;
            public string NEW_WOMAN16;
            public string NEW_WOMAN17;
            public string NEW_WOMAN18;
            public string NEW_WOMAN19;
            public string NEW_WOMAN20;
            public string NEW_WOMAN21;
            public string NEW_WOMAN22;
            public string NEW_WOMAN23;
            public string NEW_WOMAN24;
            public string NEW_WOMAN25;
            public string NEW_WOMAN26;
            public string NEW_WOMAN27;
            public string NEW_WOMAN31;
            public string NEW_WOMAN41;
            public string NEW_WOMAN42;
            public string NEW_WOMAN43;
            public string NEW_CAN_WOMAN01;
            public string NEW_CAN_WOMAN02;
            public string NEW_CAN_WOMAN03;
            public string NEW_CAN_WOMAN04;
            public string NEW_CAN_WOMAN06;
            public string NEW_CAN_WOMAN07;
            public string NEW_CAN_WOMAN08;
            public string NEW_CAN_WOMAN09;
            public string NEW_CAN_WOMAN11;
            public string NEW_CAN_WOMAN12;
            public string NEW_CAN_WOMAN13;
            public string NEW_CAN_WOMAN14;
            public string NEW_CAN_WOMAN16;
            public string NEW_CAN_WOMAN17;
            public string NEW_CAN_WOMAN18;
            public string NEW_CAN_WOMAN19;
            public string S_SOGEN;
            public string C_SOGEN;
            public string L_SOGEN;
            public string B_SOGEN;
            public string W_SOGEN;
            public string S_PANJENGDATE;
            public string C_PANJENGDATE;
            public string L_PANJENGDATE;
            public string B_PANJENGDATE;
            public string W_PANJENGDATE;
            public string S_SOGEN2;
            public string C_SOGEN2;
            public string C_SOGEN3;
            public string Jin_New;                  //진촬여부 (위^^대장^^간^^유방^^자궁^^) NEW_WOMAN03
            public string PanDrNo_New1;             //위 판정의 NEW_WOMAN32
            public string PanDrNo_New2;             //대장 판정의 NEW_WOMAN33
            public string PanDrNo_New3;             //간 판정의 NEW_WOMAN34
            public string PanDrNo_New4;             //유방 판정의 NEW_WOMAN35
            public string PanDrNo_New5;             //자궁 판정의 NEW_WOMAN36
            public string Panjeng;
            public string PanjengDate;
            public string TongboGbn;
            public string TongboDate;
            public int PanjengDrNo;
            public string Sogen;
            public string GunDate;
            public string JinchalGbn;
            public string Can_MirGbn;
            public string RESULT0001;
            public string RESULT0002;
            public string RESULT0003;
            public string RESULT0004;
            public string RESULT0005;
            public string RESULT0006;
            public string RESULT0007;
            public string RESULT0008;
            public string RESULT0009;
            public string RESULT0010;
            public string RESULT0011;
            public string RESULT0012;
            public string RESULT0013;
            public string RESULT0014;
            public string RESULT0015;
            public string RESULT0016;
            public string PANJENGDRNO1;
            public string PANJENGDRNO2;
            public string PANJENGDRNO3;
            public string PANJENGDRNO4;
            public string PANJENGDRNO5;
            public string PANJENGDRNO6;
            public string PANJENGDRNO7;
            public string PANJENGDRNO8;
            public string PANJENGDRNO9;
            public string PANJENGDRNO10;
            public string PANJENGDRNO11;
            public string NEW_SICK75;
            public string NEW_SICK76;
            public string NEW_SICK77;
            public string NEW_SICK78;
            public string L_PANJENGDATE1;
            public string LUNG_RESULT001;
            public string LUNG_RESULT002;
            public string LUNG_RESULT003;
            public string LUNG_RESULT004;
            public string LUNG_RESULT005;
            public string LUNG_RESULT006;
            public string LUNG_RESULT007;
            public string LUNG_RESULT008;
            public string LUNG_RESULT009;
            public string LUNG_RESULT010;
            public string LUNG_RESULT011;
            public string LUNG_RESULT012;
            public string LUNG_RESULT013;
            public string LUNG_RESULT014;
            public string LUNG_RESULT015;
            public string LUNG_RESULT016;
            public string LUNG_RESULT017;
            public string LUNG_RESULT018;
            public string LUNG_RESULT019;
            public string LUNG_RESULT020;
            public string LUNG_RESULT021;
            public string LUNG_RESULT022;
            public string LUNG_RESULT023;
            public string LUNG_RESULT024;
            public string LUNG_RESULT025;
            public string LUNG_RESULT026;
            public string LUNG_RESULT027;
            public string LUNG_RESULT028;
            public string LUNG_RESULT029;
            public string LUNG_RESULT030;
            public string LUNG_RESULT031;
            public string LUNG_RESULT032;
            public string LUNG_RESULT033;
            public string LUNG_RESULT034;
            public string LUNG_RESULT035;
            public string LUNG_RESULT036;
            public string LUNG_RESULT037;
            public string LUNG_RESULT038;
            public string LUNG_RESULT039;
            public string LUNG_RESULT040;
            public string LUNG_RESULT041;
            public string LUNG_RESULT042;
            public string LUNG_RESULT043;
            public string LUNG_RESULT044;
            public string LUNG_RESULT045;
            public string LUNG_RESULT046;
            public string LUNG_RESULT047;
            public string LUNG_RESULT048;
            public string LUNG_RESULT049;
            public string LUNG_RESULT050;
            public string LUNG_RESULT051;
            public string LUNG_RESULT052;
            public string LUNG_RESULT053;
            public string LUNG_RESULT054;
            public string LUNG_RESULT055;
            public string LUNG_RESULT056;
            public string LUNG_RESULT057;
            public string LUNG_RESULT058;
            public string LUNG_RESULT059;
            public string LUNG_RESULT060;
            public string LUNG_RESULT061;
            public string LUNG_RESULT062;
            public string LUNG_RESULT063;
            public string LUNG_RESULT064;
            public string LUNG_RESULT065;
            public string LUNG_RESULT066;
            public string LUNG_RESULT067;
            public string LUNG_RESULT068;
            public string LUNG_RESULT069;
            public string LUNG_RESULT070;
            public string LUNG_RESULT071;
            public string LUNG_RESULT072;
            public string LUNG_RESULT073;
            public string LUNG_RESULT074;
            public string LUNG_RESULT075;
            public string LUNG_RESULT076;
            public string LUNG_RESULT077;
            public string LUNG_RESULT078;
            public string LUNG_PLACE;
            public string NEW_WOMAN37;
            public string LUNG_RESULT079;
            public string LUNG_RESULT080;
            public string LUNG_SANGDAM1;
            public string LUNG_SANGDAM2;
            public string LUNG_SANGDAM3;
            public string LUNG_SANGDAM4;
            public string ROWID;
        }
        public static TABLE_RES_CANCER B3;

        public static void B3_CLEAR()
        {
            B3.WRTNO = 0;
            B3.Stomach_S="";               
            B3.Stomach_B="";               
            B3.Stomach_P="";               
            B3.Stomach_PETC="";             
            B3.S_ENDOGBN="";                
            B3.Stomach_SENDO="";            
            B3.Stomach_BENDO="";            
            B3.Stomach_PENDO="";            
            B3.Stomach_ENDOETC="";          
            B3.S_ANATGBN="";                
            B3.S_ANAT="";                   
            B3.S_ANATETC="";                
            B3.S_PANJENG="";                
            B3.S_MONTH="";                  
            B3.S_JILETC="";                 
            B3.S_PLACE="";                  
            B3.COLON_RESULT="";             
            B3.COLONGBN="";                 
            B3.COLON_S="";                  
            B3.COLON_B="";                  
            B3.COLON_P="";                  
            B3.COLON_ENDOGBN="";            
            B3.COLON_SENDO="";              
            B3.COLON_BENDO="";              
            B3.COLON_PENDO="";              
            B3.COLON_PETC="";
            B3.COLON_ENDOETC="";            
            B3.C_ENDOGBN="";                
            B3.C_ANATGBN="";                
            B3.C_ANAT="";                   
            B3.C_ANATETC="";                
            B3.C_PANJENG="";                
            B3.C_MONTH="";                  
            B3.C_JILETC="";                 
            B3.C_PLACE="";                  
            B3.Liver_S="";                  
            B3.Liver_B="";                  
            B3.Liver_P="";                  
            B3.Liver_SIZE="";               
            B3.Liver_LSTYLE="";             
            B3.Liver_VIOLATE="";            
            B3.Liver_DISEASSE="";           
            B3.Liver_ETC = "";                 
            B3.Liver_RPHA_EIA="";           
            B3.Liver_RPHA="";               
            B3.Liver_EIA_S="";              
            B3.Liver_EIA="";                
            B3.Liver_PANJENG="";            
            B3.Liver_JILETC="";             
            B3.Liver_PLACE="";              
            B3.Liver_New_Alt="";            
            B3.Liver_New_B="";              
            B3.Liver_New_BRes="";           
            B3.Liver_New_C="";              
            B3.Liver_New_CRes="";           
            B3.BREAST_S="";                 
            B3.BREAST_P="";                 
            B3.BREAST_ETC="";               
            B3.B_ANATGBN="";                
            B3.B_ANAT="";                   
            B3.B_ANATETC="";                
            B3.B_PANJENG="";                
            B3.B_MONTH="";                  
            B3.B_JILETC="";                 
            B3.B_PLACE="";                  
            B3.Height=0;                    
            B3.Weight=0;                    
            B3.GBSTOMACH="";                
            B3.GbLiver="";                  
            B3.GBRECTUM="";                 
            B3.GBBREAST="";                 
            B3.GbWomb="";                   
            B3.GBLUNG="";                   
            B3.SICK11="";                   
            B3.SICK12="";                 
            B3.SICK21="";                 
            B3.SICK22="";                 
            B3.SICK31="";                 
            B3.SICK32="";                 
            B3.SICK41="";                 
            B3.SICK42="";                 
            B3.SICK51="";                 
            B3.SICK52="";                 
            B3.SICK61="";                 
            B3.SICK62="";                 
            B3.SICK71="";                 
            B3.SICK72="";                 
            B3.SICK81="";                 
            B3.SICK82="";                 
            B3.SICK91="";                 
            B3.SICK92="";                 
            B3.JUNGSANG01="";             
            B3.JUNGSANG02="";             
            B3.JUNGSANG03="";             
            B3.JUNGSANG04="";             
            B3.JUNGSANG05="";             
            B3.JUNGSANG06="";             
            B3.JUNGSANG07="";             
            B3.JUNGSANG08="";             
            B3.JUNGSANG09="";             
            B3.JUNGSANG10="";             
            B3.JUNGSANG11="";             
            B3.JUNGSANG12="";             
            B3.JUNGSANG13="";             
            B3.JUNGSANG14="";             
            B3.JUNGSANG15="";             
            B3.GAJOK1="";                 
            B3.GAJOKETC="";               
            B3.DRINK1="";                 
            B3.DRINK2="";                 
            B3.SMOKING1="";               
            B3.SMOKING2="";               
            B3.WOMAN1="";                 
            B3.WOMAN2="";                 
            B3.WOMAN3="";                 
            B3.WOMAN4="";                 
            B3.WOMAN5="";                 
            B3.WOMAN6="";                 
            B3.WOMAN7="";                 
            B3.WOMAN8="";                 
            B3.WOMAN9="";                 
            B3.WOMAN10="";                
            B3.WOMAN11="";                
            B3.WOMAN12="";                
            B3.WOMAN13="";                               
            B3.WOMB01="";                 
            B3.WOMB02="";                 
            B3.WOMB03="";                 
            B3.WOMB04="";                 
            B3.WOMB05="";                 
            B3.WOMB06="";                 
            B3.WOMB07="";                 
            B3.WOMB08="";                 
            B3.WOMB09="";                 
            B3.WOMB10="";                 
            B3.WOMB11="";                 
            B3.WOMB_PLACE="";
            B3.NEW_SICK01="";
            B3.NEW_SICK02="";
            B3.NEW_SICK03="";
            B3.NEW_SICK04="";
            B3.NEW_SICK06="";
            B3.NEW_SICK07="";
            B3.NEW_SICK08="";
            B3.NEW_SICK09="";
            B3.NEW_SICK11="";
            B3.NEW_SICK12="";
            B3.NEW_SICK13="";
            B3.NEW_SICK14="";
            B3.NEW_SICK16="";
            B3.NEW_SICK17="";
            B3.NEW_SICK18="";
            B3.NEW_SICK19="";
            B3.NEW_SICK20="";
            B3.NEW_SICK21="";
            B3.NEW_SICK22="";
            B3.NEW_SICK23="";
            B3.NEW_SICK24="";
            B3.NEW_SICK25="";
            B3.NEW_SICK26="";
            B3.NEW_SICK27="";
            B3.NEW_SICK28="";
            B3.NEW_SICK29="";
            B3.NEW_SICK30="";
            B3.NEW_SICK31="";
            B3.NEW_SICK32="";
            B3.NEW_SICK33="";
            B3.NEW_SICK34="";
            B3.NEW_SICK36="";
            B3.NEW_SICK37="";
            B3.NEW_SICK38="";
            B3.NEW_SICK39="";
            B3.NEW_SICK41="";
            B3.NEW_SICK42="";
            B3.NEW_SICK43="";
            B3.NEW_SICK44="";
            B3.NEW_SICK46="";
            B3.NEW_SICK47="";
            B3.NEW_SICK48="";
            B3.NEW_SICK49="";
            B3.NEW_SICK51="";
            B3.NEW_SICK52="";
            B3.NEW_SICK53="";
            B3.NEW_SICK54="";
            B3.NEW_SICK56="";
            B3.NEW_SICK57="";
            B3.NEW_SICK58="";
            B3.NEW_SICK59="";
            B3.NEW_SICK61="";
            B3.NEW_SICK62="";
            B3.NEW_SICK63="";
            B3.NEW_SICK64="";
            B3.NEW_SICK66="";
            B3.NEW_SICK67="";
            B3.NEW_SICK68="";
            B3.NEW_SICK69="";
            B3.NEW_SICK71="";
            B3.NEW_SICK72="";
            B3.NEW_SICK73="";
            B3.NEW_SICK74="";
            B3.NEW_B_SICK01="";
            B3.NEW_B_SICK02="";
            B3.NEW_B_SICK03="";
            B3.NEW_B_SICK04="";
            B3.NEW_B_SICK05="";
            B3.NEW_B_SICK06="";
            B3.NEW_N_SICK01="";
            B3.NEW_N_SICK02="";
            B3.NEW_N_SICK03="";
            B3.NEW_S_SICK01="";
            B3.NEW_S_SICK02="";
            B3.NEW_S_SICK03="";
            B3.NEW_S_SICK04="";
            B3.NEW_CAN_01="";
            B3.NEW_CAN_02="";
            B3.NEW_CAN_03="";
            B3.NEW_CAN_04="";
            B3.NEW_CAN_06="";
            B3.NEW_CAN_07="";
            B3.NEW_CAN_08="";
            B3.NEW_CAN_09="";
            B3.NEW_CAN_11="";
            B3.NEW_CAN_12="";
            B3.NEW_CAN_13="";
            B3.NEW_CAN_14="";
            B3.NEW_CAN_16="";
            B3.NEW_CAN_17="";
            B3.NEW_CAN_18="";
            B3.NEW_CAN_19="";
            B3.NEW_CAN_21="";
            B3.NEW_CAN_22="";
            B3.NEW_CAN_23="";
            B3.NEW_CAN_24="";
            B3.NEW_CAN_26="";
            B3.NEW_CAN_27="";
            B3.NEW_CAN_28="";
            B3.NEW_CAN_29="";
            B3.NEW_HARD="";
            B3.NEW_MARRIED="";
            B3.NEW_SCHOOL="";
            B3.NEW_WORK01="";
            B3.NEW_WORK02="";
            B3.NEW_SMOKE01="";
            B3.NEW_SMOKE02="";
            B3.NEW_SMOKE03="";
            B3.NEW_SMOKE04="";
            B3.NEW_SMOKE05="";
            B3.NEW_DRINK01="";
            B3.NEW_DRINK02="";
            B3.NEW_DRINK03="";
            B3.NEW_DRINK04="";
            B3.NEW_DRINK05="";
            B3.NEW_DRINK06="";
            B3.NEW_DRINK07="";
            B3.NEW_DRINK08="";
            B3.NEW_DRINK09="";
            B3.NEW_WOMAN01="";
            B3.NEW_WOMAN02="";
            B3.NEW_WOMAN03="";
            B3.NEW_WOMAN11="";
            B3.NEW_WOMAN12="";
            B3.NEW_WOMAN13="";
            B3.NEW_WOMAN14="";
            B3.NEW_WOMAN15="";
            B3.NEW_WOMAN16="";
            B3.NEW_WOMAN17="";
            B3.NEW_WOMAN18="";
            B3.NEW_WOMAN19="";
            B3.NEW_WOMAN20="";
            B3.NEW_WOMAN21="";
            B3.NEW_WOMAN22="";
            B3.NEW_WOMAN23="";
            B3.NEW_WOMAN24="";
            B3.NEW_WOMAN25="";
            B3.NEW_WOMAN26="";
            B3.NEW_WOMAN27="";
            B3.NEW_WOMAN31="";
            B3.NEW_WOMAN41="";
            B3.NEW_WOMAN42="";
            B3.NEW_WOMAN43="";
            B3.NEW_CAN_WOMAN01="";
            B3.NEW_CAN_WOMAN02="";
            B3.NEW_CAN_WOMAN03="";
            B3.NEW_CAN_WOMAN04="";
            B3.NEW_CAN_WOMAN06="";
            B3.NEW_CAN_WOMAN07="";
            B3.NEW_CAN_WOMAN08="";
            B3.NEW_CAN_WOMAN09="";
            B3.NEW_CAN_WOMAN11="";
            B3.NEW_CAN_WOMAN12="";
            B3.NEW_CAN_WOMAN13="";
            B3.NEW_CAN_WOMAN14="";
            B3.NEW_CAN_WOMAN16="";
            B3.NEW_CAN_WOMAN17="";
            B3.NEW_CAN_WOMAN18="";
            B3.NEW_CAN_WOMAN19="";
            B3.S_SOGEN="";
            B3.C_SOGEN="";
            B3.L_SOGEN="";
            B3.B_SOGEN="";
            B3.W_SOGEN="";
            B3.S_PANJENGDATE="";
            B3.C_PANJENGDATE="";
            B3.L_PANJENGDATE="";
            B3.B_PANJENGDATE="";
            B3.W_PANJENGDATE="";
            B3.S_SOGEN2="";
            B3.C_SOGEN2="";
            B3.C_SOGEN3="";
            B3.Jin_New="";                  
            B3.PanDrNo_New1="";             
            B3.PanDrNo_New2="";             
            B3.PanDrNo_New3="";             
            B3.PanDrNo_New4="";             
            B3.PanDrNo_New5="";             
            B3.Panjeng="";
            B3.PanjengDate="";
            B3.TongboGbn="";
            B3.TongboDate="";
            B3.PanjengDrNo =0;
            B3.Sogen="";
            B3.GunDate="";
            B3.JinchalGbn="";
            B3.Can_MirGbn="";
            B3.RESULT0001="";
            B3.RESULT0002="";
            B3.RESULT0003="";
            B3.RESULT0004="";
            B3.RESULT0005="";
            B3.RESULT0006="";
            B3.RESULT0007="";
            B3.RESULT0008="";
            B3.RESULT0009="";
            B3.RESULT0010="";
            B3.RESULT0011="";
            B3.RESULT0012="";
            B3.RESULT0013="";
            B3.RESULT0014="";
            B3.RESULT0015="";
            B3.RESULT0016="";
            B3.PANJENGDRNO1="";
            B3.PANJENGDRNO2="";
            B3.PANJENGDRNO3="";
            B3.PANJENGDRNO4="";
            B3.PANJENGDRNO5="";
            B3.PANJENGDRNO6="";
            B3.PANJENGDRNO7="";
            B3.PANJENGDRNO8="";
            B3.PANJENGDRNO9="";
            B3.PANJENGDRNO10="";
            B3.PANJENGDRNO11="";
            B3.NEW_SICK75="";
            B3.NEW_SICK76="";
            B3.NEW_SICK77="";
            B3.NEW_SICK78="";
            B3.L_PANJENGDATE1="";
            B3.LUNG_RESULT001="";
            B3.LUNG_RESULT002="";
            B3.LUNG_RESULT003="";
            B3.LUNG_RESULT004="";
            B3.LUNG_RESULT005="";
            B3.LUNG_RESULT006="";
            B3.LUNG_RESULT007="";
            B3.LUNG_RESULT008="";
            B3.LUNG_RESULT009="";
            B3.LUNG_RESULT010="";
            B3.LUNG_RESULT011="";
            B3.LUNG_RESULT012="";
            B3.LUNG_RESULT013="";
            B3.LUNG_RESULT014="";
            B3.LUNG_RESULT015="";
            B3.LUNG_RESULT016="";
            B3.LUNG_RESULT017="";
            B3.LUNG_RESULT018="";
            B3.LUNG_RESULT019="";
            B3.LUNG_RESULT020="";
            B3.LUNG_RESULT021="";
            B3.LUNG_RESULT022="";
            B3.LUNG_RESULT023="";
            B3.LUNG_RESULT024="";
            B3.LUNG_RESULT025="";
            B3.LUNG_RESULT026="";
            B3.LUNG_RESULT027="";
            B3.LUNG_RESULT028="";
            B3.LUNG_RESULT029="";
            B3.LUNG_RESULT030="";
            B3.LUNG_RESULT031="";
            B3.LUNG_RESULT032="";
            B3.LUNG_RESULT033="";
            B3.LUNG_RESULT034="";
            B3.LUNG_RESULT035="";
            B3.LUNG_RESULT036="";
            B3.LUNG_RESULT037="";
            B3.LUNG_RESULT038="";
            B3.LUNG_RESULT039="";
            B3.LUNG_RESULT040="";
            B3.LUNG_RESULT041="";
            B3.LUNG_RESULT042="";
            B3.LUNG_RESULT043="";
            B3.LUNG_RESULT044="";
            B3.LUNG_RESULT045="";
            B3.LUNG_RESULT046="";
            B3.LUNG_RESULT047="";
            B3.LUNG_RESULT048="";
            B3.LUNG_RESULT049="";
            B3.LUNG_RESULT050="";
            B3.LUNG_RESULT051="";
            B3.LUNG_RESULT052="";
            B3.LUNG_RESULT053="";
            B3.LUNG_RESULT054="";
            B3.LUNG_RESULT055="";
            B3.LUNG_RESULT056="";
            B3.LUNG_RESULT057="";
            B3.LUNG_RESULT058="";
            B3.LUNG_RESULT059="";
            B3.LUNG_RESULT060="";
            B3.LUNG_RESULT061="";
            B3.LUNG_RESULT062="";
            B3.LUNG_RESULT063="";
            B3.LUNG_RESULT064="";
            B3.LUNG_RESULT065="";
            B3.LUNG_RESULT066="";
            B3.LUNG_RESULT067="";
            B3.LUNG_RESULT068="";
            B3.LUNG_RESULT069="";
            B3.LUNG_RESULT070="";
            B3.LUNG_RESULT071="";
            B3.LUNG_RESULT072="";
            B3.LUNG_RESULT073="";
            B3.LUNG_RESULT074="";
            B3.LUNG_RESULT075="";
            B3.LUNG_RESULT076="";
            B3.LUNG_RESULT077="";
            B3.LUNG_RESULT078="";
            B3.LUNG_PLACE="";
            B3.NEW_WOMAN37="";
            B3.LUNG_RESULT079="";
            B3.LUNG_RESULT080="";
            B3.LUNG_SANGDAM1="";
            B3.LUNG_SANGDAM2="";
            B3.LUNG_SANGDAM3="";
            B3.LUNG_SANGDAM4="";
            B3.ROWID = "";
        }

        //구강 검사결과
        public struct TABLE_RES_DENTAL
        {
            public long WRTNO;
            public string USIK1;
            public string USIK2;
            public string USIK3;
            public string USIK4;
            public string USIK5;
            public string USIK6;
            public string GYELSON1;
            public string GYELSON2;
            public string GYELSON3;
            public string CHIJU1;
            public string CHIJU2;
            public string CHIJU3;
            public string CHIJU4;
            public string CHIJU5;
            public string CHIJU6;
            public string CHIJU7;
            public string CHIJU8;
            public string CHIJU9;
            public string CHIJU10;
            public string BOCHUL1;
            public string BOCHUL2;
            public string BOCHUL3;
            public string BOCHUL4;
            public string BOCHUL5;
            public string BOCHUL6;
            public string BOCHUL7;
            public string BOCHUL8;
            public string BOCHUL9;
            public string BOCHUL10;
            public string BOCHUL11;
            public string BOCHUL12;
            public string OPDDNT;
            public string SCALING;
            public string DNTSTATUS;
            public string FOOD1;
            public string FOOD2;
            public string FOOD3;
            public string BRUSH11;
            public string BRUSH12;
            public string BRUSH13;
            public string BRUSH14;
            public string BRUSH15;
            public string BRUSH16;
            public string BRUSH21;
            public string JUNGSANG1;
            public string JUNGSANG2;
            public string JUNGSANG3;
            public string JUNGSANG4;
            public string JUNGSANG5;
            public string JUNGSANG6;
            public string JUNGSANG7;
            public string MUNJINETC;
            public string PANJENG1;
            public string PANJENG2;
            public string PANJENG3;
            public string PANJENG4;
            public string PANJENG5;
            public string PANJENG6;
            public string PANJENG7;
            public string PANJENG8;
            public string PANJENG9;
            public string PANJENG10;
            public string PANJENG11;
            public string PANJENG12;
            public string PanjengDate;
            public string TongboGbn;
            public string TongboDate;
            public long PanjengDrNo;
            public long MIRNO;
            public string PANJENG13;
            public string MIRYN;
            public string T_HABIT1;
            public long T_HABIT2;
            public string T_HABIT3;
            public string T_HABIT4;
            public string T_HABIT5;
            public string T_HABIT6;
            public string T_HABIT7;
            public string T_HABIT8;
            public string T_HABIT9;
            public string T_STAT1;
            public string T_STAT2;
            public string T_STAT3;
            public string T_STAT4;
            public string T_STAT5;
            public string T_STAT6;
            public string T_FUNCTION1;
            public string T_FUNCTION2;
            public string T_FUNCTION3;
            public string T_FUNCTION4;
            public string T_FUNCTION5;
            public string T_JILBYUNG1;
            public string T_JILBYUNG2;
            public string T_PAN1;
            public string T_PAN2;
            public string T_PAN3;
            public string T_PAN4;
            public string T_PAN5;
            public string T_PAN6;
            public string T_PAN7;
            public string T_PAN8;
            public string T_PAN9;
            public string T_PAN10;
            public string T_PAN11;
            public string T_PAN_ETC;
            public long T40_PAN1;
            public long T40_PAN2;
            public long T40_PAN3;
            public long T40_PAN4;
            public long T40_PAN5;
            public long T40_PAN6;
            public string T_PANJENG1;
            public string T_PANJENG2;
            public string T_PANJENG3;
            public string T_PANJENG4;
            public string T_PANJENG5;
            public string T_PANJENG6;
            public string T_PANJENG7;
            public string T_PANJENG8;
            public string T_PANJENG9;
            public string T_PANJENG10;
            public string T_PANJENG_ETC;
            public string T_PANJENG_SOGEN;
            public string SANGDAM;
            public string RES_MUNJIN;
            public string RES_JOCHI;
            public string RES_RESULT;
            public string ROWID;
        }
        public static TABLE_RES_DENTAL B4;

        public static void B4_CLEAR()
        {
            B4.WRTNO =0 ;
            B4.USIK1="";
            B4.USIK2="";
            B4.USIK3="";
            B4.USIK4="";
            B4.USIK5="";
            B4.USIK6="";
            B4.GYELSON1="";
            B4.GYELSON2="";
            B4.GYELSON3 = "";
            B4.CHIJU1 = "";
            B4.CHIJU2="";
            B4.CHIJU3="";
            B4.CHIJU4="";
            B4.CHIJU5="";
            B4.CHIJU6="";
            B4.CHIJU7="";
            B4.CHIJU8="";
            B4.CHIJU9="";
            B4.CHIJU10="";
            B4.BOCHUL1="";
            B4.BOCHUL2="";
            B4.BOCHUL3="";
            B4.BOCHUL4="";
            B4.BOCHUL5="";
            B4.BOCHUL6="";
            B4.BOCHUL7="";
            B4.BOCHUL8="";
            B4.BOCHUL9="";
            B4.BOCHUL10="";
            B4.BOCHUL11="";
            B4.BOCHUL12="";
            B4.OPDDNT="";
            B4.SCALING="";
            B4.DNTSTATUS="";
            B4.FOOD1="";
            B4.FOOD2="";
            B4.FOOD3 = "";
            B4.BRUSH11="";
            B4.BRUSH12="";
            B4.BRUSH13="";
            B4.BRUSH14="";
            B4.BRUSH15="";
            B4.BRUSH16="";
            B4.BRUSH21="";
            B4.JUNGSANG1="";
            B4.JUNGSANG2="";
            B4.JUNGSANG3="";
            B4.JUNGSANG4="";
            B4.JUNGSANG5="";
            B4.JUNGSANG6="";
            B4.JUNGSANG7="";
            B4.MUNJINETC="";
            B4.PANJENG1="";
            B4.PANJENG2="";
            B4.PANJENG3="";
            B4.PANJENG4="";
            B4.PANJENG5="";
            B4.PANJENG6="";
            B4.PANJENG7="";
            B4.PANJENG8="";
            B4.PANJENG9= "";
            B4.PANJENG10="";
            B4.PANJENG11="";
            B4.PANJENG12="";
            B4.PanjengDate = "";
            B4.TongboGbn = "";
            B4.TongboDate = "";
            B4.PanjengDrNo = 0;
            B4.MIRNO = 0;
            B4.PANJENG13="";
            B4.MIRYN="";
            B4.T_HABIT1="";
            B4.T_HABIT2 = 0;
            B4.T_HABIT3="";
            B4.T_HABIT4="";
            B4.T_HABIT5="";
            B4.T_HABIT6="";
            B4.T_HABIT7="";
            B4.T_HABIT8="";
            B4.T_HABIT9="";
            B4.T_STAT1="";
            B4.T_STAT2="";
            B4.T_STAT3="";
            B4.T_STAT4="";
            B4.T_STAT5="";
            B4.T_STAT6="";
            B4.T_FUNCTION1="";
            B4.T_FUNCTION2="";
            B4.T_FUNCTION3="";
            B4.T_FUNCTION4="";
            B4.T_FUNCTION5="";
            B4.T_JILBYUNG1="";
            B4.T_JILBYUNG2="";
            B4.T_PAN1="";
            B4.T_PAN2="";
            B4.T_PAN3="";
            B4.T_PAN4="";
            B4.T_PAN5="";
            B4.T_PAN6="";
            B4.T_PAN7="";
            B4.T_PAN8="";
            B4.T_PAN9="";
            B4.T_PAN10="";
            B4.T_PAN11="";
            B4.T_PAN_ETC="";
            B4.T40_PAN1=0;
            B4.T40_PAN2=0;
            B4.T40_PAN3=0;
            B4.T40_PAN4=0;
            B4.T40_PAN5=0;
            B4.T40_PAN6=0;
            B4.T_PANJENG1="";
            B4.T_PANJENG2="";
            B4.T_PANJENG3="";
            B4.T_PANJENG4="";
            B4.T_PANJENG5="";
            B4.T_PANJENG6="";
            B4.T_PANJENG7="";
            B4.T_PANJENG8="";
            B4.T_PANJENG9="";
            B4.T_PANJENG10="";
            B4.T_PANJENG_ETC="";
            B4.T_PANJENG_SOGEN="";
            B4.SANGDAM="";
            B4.RES_MUNJIN="";
            B4.RES_JOCHI="";
            B4.RES_RESULT="";
            B4.ROWID = "";
        }

        //특수 테이블 
        public struct TABLE_RES_SPECIAL
        {
            public long WRTNO;
            public string Jikjong;
            public string BuseName;
            public string GONGJENG;
            public string ROWID;

        }
        public static TABLE_RES_SPECIAL B5;

        public static void B5_CLEAR()
        {
            B5.WRTNO = 0;
            B5.Jikjong = "";
            B5.BuseName = "";
            B5.GONGJENG = "";
            B5.ROWID = "";
        }

        #region vbHicMonitor.bas
        public struct RECT
        {
            public long Left;
            public long Top;
            public long Right;
            public long Bottom;
        }
        public RECT rECT;
        #endregion

        #region <summary> Card_Approv    Spread Column </summary>
        public enum enmCardApprov
        {
            TRANDATE, TRANHEADER, TRADEAMT, DEPTCODE, GBIO, FINAME, CARDNO, ORIGINNO, ORIGINNO2, GUBUN1, IC, ROWID, DIV
        }

        public string[] sSpdCardApprov = {
            "승인일자", "거래구분", "승인금액", "진료과", "I/O", "발급사정보", "현금영수번호", "승인번호", "원승인번호", "자진발급", "IC거래", "ROWID", "할부"
        };

        public int[] nSpdCardApprov = {
            72, 60, 72, 44, 34, 94, 84, 64, 64, 40, 40, 44, 40
        };
        #endregion

        public string[] sHcSExam =  { "01", "02", "03", "S" };

        #region 알림톡
        public struct ATK_ARG
        {
            public string JobDate;
            public string SendType;
            public string TempCD;
            public long JobSabun;
            public string Pano;
            public string sName;
            public string HPhone;
            public string LtdName;
            public string Dept;
            public string DrName;
            public string RDate;
            public string RetTel;
            public string SendUID;
            public string SmsMsg;   //전송실패시 전송할 SMS 메세지
            public string ATMsg;    //알림톡 템플릿 메세지
            public string GJNAME;
            public long WRTNO;
            public string LINK;     // 결과지 링크 주소
        }
        public static ATK_ARG ATK;


        #endregion

        public class PanWomB
        {
            /// <summary>
            /// 검체상태
            /// </summary>
            public string WOMB01 { get; set; }
            /// <summary>
            /// 자궁경부선상피세포
            /// </summary>
            public string WOMB02 { get; set; }
            /// <summary>
            /// 유형별진단
            /// </summary>
            public string WOMB03 { get; set; }
            /// <summary>
            /// 편평상피세포이상
            /// </summary>
            public string WOMB04 { get; set; }
            /// <summary>
            /// (편평)비정형편평상피세포 일반, 고위험
            /// </summary>
            public string WOMAN12 { get; set; }
            /// <summary>
            /// 선상피세포이상
            /// </summary>
            public string WOMB05 { get; set; }
            /// <summary>
            /// 선상피세포이상 일반, 종양성
            /// </summary>
            public string RESULT0014 { get; set; }
            /// <summary>
            /// 추가소견
            /// </summary>
            public string WOMB06 { get; set; }
            /// <summary>
            /// 종합판정
            /// </summary>
            public string WOMB07 { get; set; } 
        }
    }
}
