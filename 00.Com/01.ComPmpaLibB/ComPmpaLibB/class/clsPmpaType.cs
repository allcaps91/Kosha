using ComBase;
using ComDbB;
using ComLibB;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
/// <summary>
/// Description : 구조체 모음
/// Author : 박병규, 김민철
/// Create Date : 2017.05.25
/// </summary>
/// <history>
/// </history>
/// 
namespace ComPmpaLibB
{
    public class clsPmpaType
    {
        //clsPmpaFunc cPF = new clsPmpaFunc();
        //clsIpdAcct cIAcct = new clsIpdAcct();
        //clsOumsad cPO = new clsOumsad();
        //clsAlert cA = new ComPmpaLibB.clsAlert();

        /// <summary>
        /// Description : 의료급여 승인
        /// Author : 박병규
        /// Create Date : 2017.06.20 
        /// </summary>
        /// <seealso cref="vb의료급여승인.bas"/> 
        public struct Boho_Approve_Table
        {
            public string Job;              //승인,재승인,승인취소,잔액확인
            public string Ptno;             //등록번호
            public string DeptCode;         //진료과
            public string BDate;            //진료일자
            public string Bi;               //환자종류
            public long Amt;                //1500원, 2000원 확정금액
            public long Amt_Manual;         //임의금액 2013-01-02
            public long Work_TAmt;          //가감할 급여 총진료비
            public long Work_JAmt;          //가감할 급여 조합부담
            public long Work_BAmt;          //가감할 급여 본인부담
            public long Work_GC_Amt;        //건강생활유지비 차감요청액
            //'-----------( 처리결과 )--------------------
            public string Result;           //Y.정상처리 N.오류
            public string Error_Msg;
            public long Wrtno;              //OPD_NHIC WRTNO
            public string MSeqNo;           //승인번호
            public long GC_Amt1;            //기존 승인받은 건강생활유지비 (모듈내부 계산)
            public long GC_Amt2;            //이번 승인받은 건강생활유지비
            public long GC_Amt3;            //증감 건강생활유지비(Amt2-Amt1)
            public long GC_Amt4;            //공단 건강생활유지비 잔액(승인후)
            public long GC_Jan_Amt;
            public long GC_Rem_Amt;         //산전지원비 청구액 승인금액
            //'----------(선택의료기관)------------------- 2010-02-24
            public string M1_TA_YHospital1; //선택의료급여기관명1
            public string M1_TA_YHospital2;
            public string M1_TA_YHospital3;
            public string M1_TA_YHospital4;
            public string M1_TA_YKiho1;     //선택의료급여기관기호1
            public string M1_TA_YKiho2;
            public string M1_TA_YKiho3;
            public string M1_TA_YKiho4;
            //'----------( 승인요청 자료 )----------------
            public string M3_JinType;       //진료형태(1.입원 2.외래)
            public int M3_Ilsu;             //입(내)원 일수 (외래:1, 입원:입원일수)
            public int M3_Tuyak;            //투약일수:원내투약일수
            public long M3_Johap_Amt;       //조합(기관)부담금
            public long M3_Bonin_Amt;       //급여 본인일부부담금
            public long M3_GC_Amt;          //건강생활유지비 청구액
            public string M3_Msym;          //주상병분류기호
            public string M3_Jin_Date;      //진료일자
            public string M3_ODrug;         //원외처방전 교부번호
            public string M3_Bonin_Gbn;     //본인부담여부
            public string M3_TA_Hospital;   //타기관의뢰여부
            public string M3_Jang_No;       //장애시 진료확인 번호
            public long M3_TotAmt;          //급여총진료비=조합부담+본인부담+건강생활유지비청구액
            public string M3_Dept;          //진료과 코드
            public string M3_GnoYN;         //Y:원외처방전 발급시  N:미발급
            public string M3_OutCode;       //1.입원중 2.퇴원 3.기타(외래 등)
            public long M3_PregSumAmt;      //산전지원비(비급여총액)
            public long M3_PregDmndAmt;     //산전지원비 청구액
            public string M3_TA_YKiho;      //선택의료급여기관기호 2010-02-24
            public string M3_Tooth;         //노인틀니구분 02
            //'------( 취소요청 자료 )-------------
            public string M5_Approve_No;    //취소할 진료확인 번호
            public string M5_Jin_Date;      //취소할 진료일자
        }
        public static Boho_Approve_Table BAT;

        /// <summary>
        /// Description : 외래접수 Master Table
        /// Author : 박병규
        /// Create Date : 2017.07.06 
        /// </summary>
        /// <seealso cref="OUMSAD.bas"/> 
        public struct Table_Opd_Master
        {
            public long OpdNo;
            public string Pano;
            public string DeptCode;
            public string Bi;
            public string sName;
            public string Sex;
            public int Age;
            public string Jiyuk;
            public string DrCode;
            public string Reserved;
            public string Chojae;
            public string GbGameK;
            public string GbGameKC;
            public string GbSpc;
            public string Jin;
            public string Sinwhan;
            public string Bohun;
            public string Change;
            public string Sheet;
            public string Rep;
            public string Part;
            public string JTime;
            public string Stime;
            public int Fee1;
            public int Fee2;
            public int Fee3;
            public int Fee31;
            public int Fee5;
            public int Fee51;
            public int Fee7;
            public long Amt1;
            public long Amt2;
            public long Amt3;
            public long Amt4;
            public long Amt5;
            public long Amt6;
            public long Amt7;
            public string GelCode;
            public string BDate;
            public string ActDate;
            public string WardCode;
            public int RoomCode;
            public string Bunup;
            public string OCSJIN;
            public string ROWID;
            public long CardSeqNo;
            public string Emr;
            public string VCode;
            public string MCode;
            public string MSeqNo;
            public string MQCODE;
            public string MksJin;
            public string GbNight;          //야간소아구분
            public string GbFlu_Vac;        // 신종플루예방접종구분
            public string OldMan;           // 어르신먼저구분 
            public string GbExam;           // 입원정밀구분 - II 수납시 - >ipd_new_master 사용
            public string GbDementia;       // 치매검사여부 
            public string Jtime2;           // 접수날짜시간 
            public string Gbilban2;         // 외국new
            public string GbFlu_Ltd;        // 사업장예방접종
            public string JinDtl;           // 접수추가구분 
            public string INSULIN;          // 인슐린구분 
            public string OUTTIME;          // 응급실퇴실시간 
            public long DrSabun;            // 전공의 사번 
            public string J2_Sayu;          // 접수2 사유  
            public string ResMemo;
            public string Jiwon;
            public string KTASLEVL;
            public string GBRES;            // 예약후불변경
            public string GBPOWDER;         // 파우더 가산
            public string YHOSP_KIHO;       // 타병원 입원 요양기관기호
            // 2021-09-16 접수 시 특정 데이터가 들어올 경우 처방 자동 발생되도록
            // 현재는 NCOV-PA 만 사용합니다. (검사일 경우 값은 '1' 이 들어옴)
            public string GbFlu;        
        }
        public static Table_Opd_Master TOM;

        /// <summary>
        /// Description : 최종진료정보 Master Table
        /// Author : 박병규
        /// Create Date : 2017.07.06 
        /// </summary>
        /// <seealso cref="OUMSAD.bas"/> 
        public struct Table_Last_ExamIn
        {
            public string Ptno;
            public string DeptCode;
            public string LastDate;
            public int LastTuyak;
            public string LastIll;
            public string DrCode;
            public string GbSpc;
            public string GbGameK;
        }
        public static Table_Last_ExamIn TLE;

        /// <summary>
        /// Description : 환자보험정보 Master Table
        /// Author : 박병규
        /// Create Date : 2017.07.06 
        /// </summary>
        /// <seealso cref="OUMSAD.bas"/> 
        public struct Table_Mih
        {
            public string Ptno;
            public string Bi;
            public string TransDate;
            public string PName;
            public string Gwange;
            public string Kiho;
            public string GKiho;
            public string AreaDept1;
            public string AreaDept2;
            public string AreaDept3;
            public string AreaDate1;
            public string AreaDate2;
            public string AreaDate3;
            public string AreaBDate;
        }
        public static Table_Mih TMI;

        /// <summary>
        /// Description : 진료예약정보 Master Table
        /// Author : 박병규
        /// Create Date : 2017.07.06 
        /// </summary>
        /// <seealso cref="OUMSAD.bas"/> 
        public struct Table_Resv
        {
            public string Ptno;
            public string DeptCode;
            public string Bi;
            public string sName;
            public string DrCode;
            public string Date1;
            public string Date2;
            public string Date3;
            public string Chojae;
            public string GbGameK;
            public string GbGameKC;
            public string GbSpc;
            public string Jin;
            public long Amt1;
            public long Amt2;
            public long Amt3;
            public long Amt4;
            public long Amt5;
            public long Amt6;
            public long Amt7;
            public string Part;
            public string Bohun;
            public string GelCode;
            public long CardSeqNo;
            public string VCode;
        }


        public static Table_Resv TRV;

        /// <summary>
        /// Description : 입원 Master Table
        /// Author : 김민철
        /// Create Date : 2017.07.06 
        /// </summary> 
        /// <seealso cref="IUMENT.bas"/>
        public struct Table_Ipd_Master
        {
            public long IPDNO;
            public string Pano;
            public string Sname;
            public string Sex;
            public int Age;
            public int AgeDays;                  //나이 0세 일수
            public string Bi;
            public string InDate;
            public string InTime;
            public string OutDate;
            public string ActDate;
            public int Ilsu;
            public string GbSTS;
            public string DeptCode;
            public string DrCode;
            public string WardCode;
            public int RoomCode;
            public string PName;
            public string GbSpc;
            public string GbKekli;
            public string GbGameK;
            public string GbTewon;
            public bool GbOldSlip;
            public int Fee6;
            public string Bohun;
            public string Jiyuk;
            public string GelCode;
            public string Religion;
            public string GbCancer;
            public string InOut;
            public string Other;
            public string GbDonggi;
            public string OgPdBun;
            public string Article;
            public string JupboNo;
            public string FromTrans;
            public long ErAmt;
            public string MPano;
            public string ArcDate;
            public int ArcQty;
            public int IcuQty;
            public int Im180;
            public string IllCode1;           //상병코드 1     (6)
            public string IllCode2;           //상병코드 2     (6)
            public string IllCode3;           //상병코드 3     (6)
            public string IllCode4;           //상병코드 4     (6)
            public string IllCode5;           //상병코드 5     (6)
            public string IllCode6;           //상병코드 6     (6)
            public string TrsDate;
            public string Dept1;
            public string Dept2;
            public string Dept3;
            public string Doctor1;
            public string Doctor2;
            public string Doctor3;
            public int Ilsu1;
            public int Ilsu2;
            public int Ilsu3;
            public string Amset1;
            public string AmSet2;
            public string AmSet3;
            public string AmSet4;
            public string AmSet5;
            public string AmSet6;
            public string AmSet7;
            public string AmSet8;
            public string AmSet9;
            public string AmSetA;
            public string RDate;
            public int TrsCNT;
            public long LastTrs;
            public string IpwonTime;
            public string CancelTime;
            public string GatewonTime;
            public string GatewonSayu;
            public string ROutDate;
            public string SimsaTime;
            public string PrintTime;
            public string SunapTime;
            public string GbCheckList;
            public string MirBuildTime;
            public string Remark;
            public long JungganJanAmt;       //보증금+중간납의 미대체 잔액
            public int MstCNT;               //마스타의 건수
            public string GBSuDay;
            public string PNEUMONIA;         //폐렴
            public string Pregnant;          //임신여부
            public string GbGoOut;           //외출,외박불가 - 해당사항 Y
            public string OgPdBundtl;        //차상위2종의 자연분만,6세만 구분
            public string WardInTime;        //병동입실시간 - 2009-11-18
            public string TelRemark;         //입원대기 긴급연락처 2009-12-28
            public string GbExam;            //입원중정밀검사 Y 2010-01-21
            public string Secret;            //정보보호구분 2012-11-22
            public string GbDRG;             //DRG 여부   2013-06-29
            public string DrgCode;           //DRG 코드   2013-06-29
            public string KTASLEVL;          //2015-12-28
            public string FROOM;
            public string FROOMETC;
            public string GBJIWON;
            public string T_CARE;            //2016-07-19
            public string Pass_Info;
            public string RETURN_HOSP;
            public string KTAS_HIS;
        }
        public static Table_Ipd_Master IMST;

        /// <summary>
        /// Description : 입원 Trans + Master Table
        /// Author : 김민철
        /// Create Date : 2017.07.06 
        /// </summary> 
        /// <seealso cref="IUMENT.bas"/>
        public struct Table_Ipd_Trans
        {
            public long Ipdno;
            public long Trsno;
            public string Pano;
            public string GbIpd;
            public string TGbSts;
            public string MGbSts;
            public string WardCode;
            public string RoomCode;
            public string Bi;
            public string Sname;
            public string Sex;
            public int Age;
            public double AgeDays;          //나이 0세 일수
            public string Jumin1;
            public string Jumin2;
            public string Jumin3;        //주민암호화
            public string InDate;
            public string OutDate;
            public string ActDate;
            public string DeptCode;
            public string DrCode;
            public string Kiho;
            public string GKiho;
            public string PName;
            public string Gwange;
            public int BonRate;
            public int GISULRATE;
            public string GbSpc;
            public string GbKekli;
            public string GbGameK;
            public string GbTewon;
            public string Bohun;
            public string JiCode;
            public string Amset1;
            public string AmSet2;
            public string AmSet3;
            public string AmSet4;
            public string AmSet5;
            public string AmSet6;
            public string AmSet7;
            public string AmSet8;
            public string AmSet9;
            public string AmSetA;
            public string AmSetB;
            public string GelCode;
            public string FromTrans;
            public long ErAmt;
            public string Remark;
            public string JupboNo;
            public int OldCount;
            public string TrsDate;
            public int Ilsu1;
            public int Ilsu2;
            public int Ilsu;
            public string InTime;
            public string InTime2;       //2012-09-04
            public string InTime3;       //2016-02-18
            public int Im180;
            public string Jiyuk;
            public string Religion;
            public string MPano;
            public string Article;
            public string GbCancer;
            public string InOut;
            public string Other;
            public string GbDonggi;
            public string GbDRG;
            public long DrgWRTNO;
            public long SangAmt;
            public string OgPdBun;
            public long DtGamek;
            public string IllCode1;      //상병코드 1     (6)
            public string IllCode2;      //상병코드 2     (6)
            public string IllCode3;      //상병코드 3     (6)
            public string IllCode4;      //상병코드 4     (6)
            public string IllCode5;      //상병코드 5     (6)
            public string IllCode6;      //상병코드 6     (6)
            public long[] Amt;
            public long TotGub;
            public long TotBigub;
            public long[,] RAmt;         //영수증 인쇄용 금액
            public string Gbilban2;      //외국new
            public string DrgCode;       //2013-06-25
            //IPD_MASTER의 값
            public string M_InDate;
            public string M_OutDate;
            public string M_ActDate;
            public int M_Ilsu;
            public string M_GBSuDay;     //2013-06-19
            public string ArcDate;
            public int ArcQty;
            public int Fee6;
            public int IcuQty;
            public int IcuQty2;          //2013-03-15
            public string ROutDate;
            public string SimsaTime;
            public string PrintTime;
            public string SunapTime;
            public string GbCheckList;
            public string MirBuildTime;
            public int MstCNT;           //TRAN의 건수 재원
            public string VCode;
            public string GbSang;
            public string MSeqNo;
            public string OgPdBundtl;    //차상위2종-자연분만,6세미만 구분
            public string JinDtl;        //자격상세 노인틀니 2012-07-03
            public string OgPdBun2;      //지병상해외인 - 2012-11-14
            public string MiArcDate;
            public int MiIlsu;
            public string WardDate;      //2016-02-18
            public string TROWID;       //임시용 rowid 2012-09-07
            //재원심사 변수---------------------------------------
            public string JSIM_LDATE;    //최종 재원심사일
            public string JSIM_SABUN;    //최종 심사자
            public string JSIM_Set;      //내환자SET
            public string JSIM_REMARK;   //재원심사 참고사항
            public string JSIM_REMARK9;   //재원심사 참고사항(지병)
            public string JSIM_OK;       //재원심사 (청구완성)
            //2013-12-19
            //DRG정보 추가
            public string DRGADC1;
            public string DRGADC2;
            public string DRGADC3;
            public string DRGADC4;
            public string DRGADC5;
            public string GbTax;         //부가세 대상 2014-02-24
            public string FCode;         //FCode 추가 2014-06-07
            public string KTASLEVL;      //KTASLEVL 2015-12-28
            public string T_CARE;        //통합간호간병 2016-07-29
            public string GBJIWON;       //긴급복지대상
            public string DRGOG;         //DRG 산과가산여부
            public string BirthDay;
            public string GBHU;          //호스피스
        }
        public static Table_Ipd_Trans TIT;

        /// <summary>
        /// Description : 환자기본정보 Master Table
        /// Author : 김민철
        /// Create Date : 2017.07.06 
        /// </summary> 
        /// <seealso cref="IUMENT.bas"/>
        public struct Table_Bas_Patient
        {
            public string Ptno;
            public string Sname;
            public string Sname2;
            public string Sex;
            public string Jumin1;
            public string Jumin2;
            public string Jumin3;       //주민암호화
            public string StartDate;
            public string LastDate;
            public string ZipCode1;
            public string ZipCode2;
            public string Juso;
            public string Jiyuk;
            public string Tel;
            public string HPhone;
            public string HPhone2;
            public string Sabun;
            public string EmbPrt;
            public string Bi;
            public string PName;
            public string PName2;
            public string Gwange;
            public string Kiho;
            public string GKiho;
            public string DeptCode;
            public string DrCode;
            public string GbSpc;
            public string GbGameK;//감액구분(자격자)
            public string GbGameKC;//감액구분(발생CASE)
            public int JinIlsu;
            public long JinAMT;
            public string TuyakGwa;
            public string TuyakMonth;
            public int TuyakJulDate;
            public int TuyakIlsu;
            public string Bohun;
            public string Remark;
            public string GbMsg;
            public string Bunup;
            public string Birth;
            public string GbBirth;
            public string EMail;
            public string GbInfor;
            public string GbSMS;
            public string MiaBdate;
            public string MiaGubun;
            public string GBJuso;
            public string Tel_Confirm;
            public string Gbinfo_Detail;
            public string Road;
            public string GbForeigner;          //외국인여부(Y.외국인)
            public string EName;                //외국인 여권 영문이름
            public string GB_VIP;               //VIP 고객종류
            public string GB_VIP_REMARK;        //VIP 고객 상세설명
            public string OBST;
            public string ZipCode3;
            public string BuildNo;
            public string RoadDetail;
            public string GbCountry;
            public string Religion;
            public string GbEPassNo;
            public string GbEUniqNo;
            public string GbEssn;
        }
        public static Table_Bas_Patient TBP;

        /// <summary>
        /// Description : 재원심사용 구조체
        /// Author : 김민철
        /// Create Date : 2017.07.08 
        /// </summary> 
        /// <seealso cref="IuSent1.bas"/>
        public struct JSIM_Table
        {
            public string Pano;
            public string InDate;
            public long IPDNO;       //IPDNO
            public long TRSNO;       //TRSNO
            public string SABUN;     //사번
            public string Set;       //내환자 설정
            public string JobSet;    //일반심사구분
            public string FRDATE;    //심사작업일자
            public string TODATE;    //심사작업일자
            public string LDATE;     //최종일
            public string DISP;      //환자목료자동표시
            public string Next;      //환자목료자동다음 환자 set
            public string Line;      //line  추가
            public string MY;        // 내환자 추가
            public string BackColor; //배경색깔
        }
        public static JSIM_Table JSIM;

        /// <summary>
        /// Description : 감액요율 및 감액정보(자격)
        /// Author : 김민철
        /// Create Date : 2017.07.14
        /// </summary>
        /// <seealso cref="OpdGamek.bas"/>
        public struct Table_Gamek_Gesan
        {
            public string GbGameK;    //감액계정
            public string LtdCode;
            public int BonRate;
            //---------( 감 액 율 )----------------------
            public int Amt50_Rate;   //총진료비 감액율
            public int Jin_Rate;     //진찰료 감액율
            public int Gam_Rate;     //진료비 감액율
            public int SONO_Rate;    //모든과 초음파 감액율
            public int MRI_Rate;     //모든과 MRI 감액율
            public int FOOD_Rate;    //모든과 비급여식대 감액율
            public int ROOM_Rate;    //모든과 병실차액 감액율
            public int ER_Rate;      //모든과 응급관리료 감액율
            public int DTJin_Rate;   //치과 진찰료 감액율
            public int DTGam_Rate;   //치과 진료비 감액율
            public int DT1_Rate;     //치과 비급여처치료 감액율
            public int DT2_Rate;     //치과 보철료 감액율
            public int DT3_Rate;     //치과 임플란트 감액율
            //-------( 감액 계산 금액 )---------------
            public long Jin_Halin;
            public long Gam_Halin;
            public long SONO_Halin;
            public long MRI_Halin;
            public long FOOD_Halin;
            public long ROOM_Halin;
            public long ER_Halin;
            public long DTJin_Halin;
            public long DTGam_Halin;
            public long DT1_Halin;
            public long DT2_Halin;
            public long DT3_Halin;
            public long DTHalin_Tot;
            public long Halin_Tot;
        }
        public static Table_Gamek_Gesan GAM;

        /// <summary>
        /// Description : 감액요율 및 감액정보(CASE)
        /// Author : 박병규
        /// Create Date : 2017.08.22
        /// </summary>
        /// <seealso cref="OpdGamek.bas"/>
        public struct Table_GamekCase_Gesan
        {
            public string GbGameK;    //감액계정
            public string LtdCode;
            public int BonRate;
            //---------( 감 액 율 )----------------------
            public int Amt50_Rate;   //총진료비 감액율
            public int Jin_Rate;     //진찰료 감액율
            public int Gam_Rate;     //진료비 감액율
            public int SONO_Rate;    //모든과 초음파 감액율
            public int MRI_Rate;     //모든과 MRI 감액율
            public int FOOD_Rate;    //모든과 비급여식대 감액율
            public int ROOM_Rate;    //모든과 병실차액 감액율
            public int ER_Rate;      //모든과 응급관리료 감액율
            public int DTJin_Rate;   //치과 진찰료 감액율
            public int DTGam_Rate;   //치과 진료비 감액율
            public int DT1_Rate;     //치과 비급여처치료 감액율
            public int DT2_Rate;     //치과 보철료 감액율
            public int DT3_Rate;     //치과 임플란트 감액율
            //-------( 감액 계산 금액 )---------------
            public long Jin_Halin;
            public long Gam_Halin;
            public long SONO_Halin;
            public long MRI_Halin;
            public long FOOD_Halin;
            public long ROOM_Halin;
            public long ER_Halin;
            public long DTJin_Halin;
            public long DTGam_Halin;
            public long DT1_Halin;
            public long DT2_Halin;
            public long DT3_Halin;
            public long DTHalin_Tot;
            public long Halin_Tot;
        }
        public static Table_GamekCase_Gesan GAMC;

        /// <summary>
        /// Description : 입원용 그룹수가정보 변수
        /// Author : 김민철
        /// Create Date : 2017.08.07
        /// </summary>
        /// <seealso cref="IPDACCT.bas"/>
        public struct Slip_Host_Table_IPD
        {
            public double Qty;
            public int Nal;
            public string Dev;
            public string Imiv;
            public string GbSelf;
            public string GbNgt;
            public string GbSpc;
            public string Sucode;
            public string SugbB;
            public long BaseAmt;

        }
        public static Slip_Host_Table_IPD ISH;

        /// <summary>
        /// Description : 입원용 수가정보 Read 변수
        /// Author : 김민철
        /// Create Date : 2017.08.07
        /// </summary>
        /// <seealso cref="IPDACCT.bas"/>
        public struct Slip_Accept_Table_IPD
        {
            public double Qty;
            public double RealQty;
            public int Nal;
            public string Imiv;
            public string Dev;
            public string GbNgt;
            public string GbNgt2;
            public string GbSpc;
            public string GbSelf;
            public string GbSelfSource; 
            public string GbInfo;
            public string GbTFlag; 
            public string SlipNo;
            public long BaseAmt;
            public string OrderCode;
            public string DosCode;
            public string Sucode;
            public string Sunext;
            public string Bun;
            public string Nu;
            public string SugbA;
            public string SugbB;
            public string SugbC;
            public string SugbD;
            public string SugbE;
            public string SugbF;
            public string SugbG;
            public string SugbH;
            public string SugbI;
            public string SugbJ;
            public string SugbP;
            public string SugbQ;
            public string SugbR;
            public string SugbS;
            public string SugbX;
            public string SugbY;
            public string SugbZ;
            public string SugbAA;
            public string SugbAB;
            public string SugbAC;
            public string SugbAD;
            public string SugbAG;

            public long Iamt;
            public long Tamt;
            public long Bamt;
            public long Selamt;
            public string Sudate;
            public long OldIamt;
            public long OldTamt;
            public long OldBamt;
            public int DayMax;
            public int TotMax;
            public string SunameK;
            public string Hcode;
            public string GbSlip;
            public string WardCode;
            public int RoomCode;
            public string DeptCode;
            public string DrCode;
            public int SEQNO;
            public string YYMM;
            public long OrderNo;
            public string DrgSelf;
            public string ABCDATE;
            public string OPER_DEPT;
            public string OPER_DCT;
            public int Div;
            public string GbSelNot;
            public string CONSULT;
            public string GBNS;
            public string PART2;
            public string GBKTAS;
            public string GbChildZ;
            public string GbHighRisk;
            public string GbGSADD;
            public string GbASADD;
            public string GbER24H;
            public string GbErChk;
            public string BURN;
            public string OPGBN;
            public string BCODE;
        }
        public static Slip_Accept_Table_IPD[] ISA;

        /// <summary>
        /// Description : 입원용 수가정보 계산용 변수
        /// Author : 김민철
        /// Create Date : 2017.08.07
        /// </summary>
        /// <seealso cref="IPDACCT.bas"/>
        public struct Slip_Gesan_Table_IPD
        {
            public double Qty;
            public double RealQty;
            public int Nal;
            public string Imiv;
            public string Dev;
            public string GbNgt;
            public string GbSpc;
            public string GbSelf;
            public long BaseAmt;
            public string Sucode;
            public string Sunext;
            public string Bun;
            public string Nu;
            public string SugbA;
            public string SugbB;
            public string SugbC;
            public string SugbD;
            public string SugbE;
            public string SugbF;
            public string SugbG;
            public string SugbH;
            public string SugbI;
            public string SugbJ;
            public string SugbP;
            public string SugbQ;
            public string SugbR;
            public string SugbS;
            public string SugbX;
            public string SugbY;
            public string SugbZ;
            public string SugbAA;
            public string SugbAB;
            public string SugbAC;
            public string SugbAD;
            public string SugbAG;
            public long Iamt;
            public long Tamt;
            public long Bamt;
            public long Selamt;
            public string Sudate;
            public long OldIamt;
            public long OldTamt;
            public long OldBamt;
            public string Sudate3;
            public long Iamt3;
            public long Tamt3;
            public long Bamt3;
            public string Sudate4;
            public long Iamt4;
            public long Tamt4;
            public long Bamt4;
            public string Sudate5;
            public long Iamt5;
            public long Tamt5;
            public long Bamt5;
            public int SAno;
            public string GbSlip;
            public int DayTot;
            public int MaxTot;
            public string DrgSelf;
            public long OrderNo;
            public int Div;
            public string DrCode;
            public string DeptCode;
            public string GbSelNot;
            public string GbSelfSource;
            public string PART2;
            public string GBKTAS;
            public string GbChildZ;
            public string GbErActTime;
            public string GbErChk;
            public string GbHighRisk;
            public string GbER24H;
            public string GSADD;
            public string ASADD;
            public string BURN;
            public string OPGBN;
            public string GBNS;
            public string BCODE;
            public string GBNGT2;
            public string POWDER;
        }
        public static Slip_Gesan_Table_IPD ISG;

        /// <summary>
        /// Description : 입원용 수가정보 Write 변수
        /// Author : 김민철
        /// Create Date : 2017.08.07
        /// </summary>
        /// <seealso cref="IPDACCT.bas"/>
        public struct Slip_Write_Table_IPD
        {
            public string Sucode;
            public string Sunext;
            public string Bun;
            public string Nu;
            public double Qty;
            public int Nal;
            public long BaseAmt;
            public string GbSpc;
            public string GbNgt;
            public string GbGisul;
            public string GbSlip; 
            public string GbSelf;
            public string GbChild;
            public string GbHost;
            public string DrCode;
            public string DeptCode;
            public string GbSugbS;
            public string GbSugbX;
            public string SugbS;
            public string GbGSADD;
            public string GbASADD;
            public string GBSUGBAB;
            public string GBSUGBAC;
            public long Amt1;
            public long Amt2;
            public int SEQNO;
            public int RoomCode;
            public string WardCode;
            public long OrderNo;
            public string DrgSelf;
            public int SAno;
            public int Div;
            public string GbSelNot;
            public string GbSelfSource;
            public string GbEr;
            public string BCODE;
            public string OPGBN;
            public string BURN;
            public string HIGHRISK;
            public string GBNGT2;
        }
        public static Slip_Write_Table_IPD[] ISW;

        /// <summary>
        /// Description : 입원용 수가정보 Return 변수
        /// Author : 김민철
        /// Create Date : 2017.08.07
        /// </summary>
        /// <seealso cref="IPDACCT.bas"/>
        public struct Slip_Return_Table_IPD
        {
            public string Sucode;
            public string Sunext;
            public string Bun;
            public string Nu;
            public double Qty;
            public int Nal;
            public long BaseAmt;
            public string GbSpc;
            public string GbNgt;
            public string GbGisul;
            public string GbSelf;
            public string GbChild;
            public string DeptCode;
            public string DrCode;
            public string WardCode;
            public string GbSlip;
            public string GbHost;
            public string GbSugbS;
            public string GbSugbX;
            public long Amt1;
            public long Amt2;
            public long OrderNo;
            public string DrgSelf;
            public int Div;
            public string GbEr;
        }
        public static Slip_Return_Table_IPD[] ISR;

        /// <summary>
        /// Description : 금액계산용 부가정보 변수
        /// Author : 김민철
        /// Create Date : 2017.08.07
        /// </summary>
        /// <seealso cref="IPDACCT.bas"/>
        public struct Argument_Table_IPD
        {
            public string Date;
            public string Dept;
            public string Sex;
            public string GbSpc;
            public string GbGameK;
            public string Sang1;
            public string Sang2;
            public int Retn;
            public int Bi;
            public int Bi1;
            public int Age;
            public double Age2;
            public string Jumin1;
            public string Jumin2;
            public int Fee6;
            public bool DisCharge;
            public long[] Amt;
            public int AgeiLsu;         //신생아일수
            public string Gbilban2;
            public string pano;
            public string DrCode;
            public long IPDNO;
            public long TRSNO;
            public string GBSelNot;
            public string WardCode;
            public string RoomCode;
            public string ErJDate;
            public string KTASLEVEL;
            public int SlipCount;
        }
        public static Argument_Table_IPD IA; 

        /// <summary>
        /// Description : 개인별 선택진료 정보변경 변수체크용
        /// Author : 김민철
        /// Create Date : 2017.08.09
        /// </summary>
        /// <seealso cref="Vb선택진료.bas"/>
        public struct SELECT_Pano_Change_CHK
        {
            public string GbChange;     //변경 Y
            public string GbIO;         //외래/입원
            public string pano;         //등록번호
            public string GAMEK;        //감액
            public string Bi;           //보험종류
            public string DeptCode;     //과
            public string DrCode;       //의사코드
            public string BDate;        //발생일자
            public string GbRD;         //영상진단 및 방사선치료 선택등록여부
            public string RD_DrCode;    //영상진단 및 방사선치료 의사코드
        }
        public static SELECT_Pano_Change_CHK SCP;

        /// <summary>
        /// Description : 선택진료 계산관련
        /// Author : 김민철
        /// Create Date : 2017.08.09
        /// </summary>
        /// <seealso cref="Vb선택진료.bas"/>
        public struct Table_Select_Jin_Gesan
        {
            //---------( 선택진료율 )----------------------
            public string OPD_Gb_Select;    //외래선택진료여부   2014-02-03
            public int OPD_Jin_Rate;        //진찰료
            public int OPD_Med_Rate;        //의학관리료
            public int OPD_Gum_Rate;        //검사료
            public int OPD_Xray_Rate;       //영상진단 및 방사선치료
            public int OPD_Mach_Rate;       //마취료
            public int OPD_Psy_Rate;        //정신요법
            public int OPD_Op_Rate;         //처치수술료

            public string IPD_Gb_Select;    //입원선택진료여부   2014-02-03
            public int IPD_Jin_Rate;        //진찰료
            public int IPD_Med_Rate;        //의학관리료
            public int IPD_Gum_Rate;        //검사료
            public int IPD_Xray_Rate;       //영상진단 및 방사선치료
            public int IPD_Mach_Rate;       //마취료
            public int IPD_Psy_Rate;        //정신요법
            public int IPD_Op_Rate;         //처치수술료
            //-------( 수가의 해당항목 진료율 )---------------
            public int Current_Rate;
            public string Suga_GbSelect;
        }
        public static Table_Select_Jin_Gesan SEL;

        /// <summary>
        /// Description : 개인별 선택진료 과별 진료지원항목정보
        /// Author : 김민철
        /// Create Date : 2017.08.09
        /// </summary>
        /// <seealso cref="Vb선택진료.bas"/>
        public struct Table_Select_Pano_SET
        {
            public string GbSet1;           //진찰
            public string GbSet2;           //검사
            public string GbSet3;           //영상진단
            public string GbSet4;           //방사선치료
            public string GbSet5;           //방사선혈관촬영
            public string GbSet6;           //마취
            public string GbSet7;           //정신요법
            public string GbSet8;           //처치수술
            public string GbSet9;           //침.구 부항
            public string GbSet_Current;
        }
        public static Table_Select_Pano_SET SEL_SET;



        //AMT(01):진찰료(예약)  AMT(06):처치수술료    AMT(10):조합부담  AMT(14):금액합계(진찰료제외)
        //AMT(02):투약          AMT(07):CT/MRI/SONO   AMT(11):본인부담  AMT(15):영수금액
        //AMT(03):주사료        AMT(08):기타          AMT(12):감    액  AMT(16):헌혈미수
        //AMT(04):검사          AMT(09):금액합계      AMT(13):개인미수  AMT(17):수혈료
        //AMT(05):방사선
        /// <summary>
        /// Description : 환불작업시 금액 누적
        /// Author : 박병규
        /// Create Date : 2017.08.30
        /// </summary>
        /// <seealso cref="OPDACCT.bas"/>
        public struct Add_Amt_Table
        {
            public double Amt1;// 급여금액
            public double Amt2;// 비급여금액
            public double Amt3;// 특진료
        }
        //public static Add_Amt_Table[] AAT = new Add_Amt_Table[18];
        public static Add_Amt_Table[] AAT;

        /// <summary>
        /// Description : 환불작업시 금액 누적
        /// Author : 박병규
        /// Create Date : 2017.08.30
        /// </summary>
        /// <seealso cref="OPDACCT.bas"/>
        public struct Return_Amt_Table
        {
            public double Amt1;// 급여금액
            public double Amt2;// 비급여금액
            public double Amt3;// 특진료
        }
        //public static Return_Amt_Table[] RAT = new Return_Amt_Table[18];
        public static Return_Amt_Table[] RAT;

        /// <summary>
        /// Description : 당일의 총금액 보관용
        /// Author : 박병규
        /// Create Date : 2017.08.30
        /// </summary>
        /// <seealso cref="OPDACCT.bas"/>
        public struct OLD_Amt_Table
        {
            public double Amt1;// 급여금액
            public double Amt2;// 비급여금액
            public double Amt3;// 특진료
        }
        //public static OLD_Amt_Table[] OLD = new OLD_Amt_Table[18];
        public static OLD_Amt_Table[] OLD;

        /// <summary>
        /// Description : '25,000원이하,초과
        /// Author : 박병규
        /// Create Date : 2017.08.30
        /// </summary>
        /// <seealso cref="OPDACCT.bas"/>
        public struct WORK_Amt_Table
        {
            public double Amt1;// 급여금액
            public double Amt2;// 비급여금액
            public double Amt3;// 특진료
        }
        //public static WORK_Amt_Table[] WAT = new WORK_Amt_Table[18];
        public static WORK_Amt_Table[] WAT;

        /// <summary>
        /// Description : 미수 마스터
        /// Author : 김민철
        /// Create Date : 2017.09.13
        /// </summary>
        ///  <seealso cref="MUMAST.BAS"/>
        public struct Table_MISU_IDMST
        {
            public long WRTNO;
            public string MisuID;
            public string BDate;
            public string Class;
            public string YYMM;
            public string IpdOpd;
            public string Bi;
            public string GelCode;
            public string Bun;
            public string FromDate;
            public string ToDate;
            public int Ilsu;
            public string DeptCode;
            public string MgrRank;
            public int[] Qty;
            public double[] Amt;
            public double JAmt;
            public string GbEnd;
            public string Remark;
            public string JepsuNo;
            public string EntDate;
            public long EntPart;
            public string TongGbn;
            public string MirYYMM;
            public string EdiMirNo;
            public string ChaSu;
            public string MukNo;
            public string ROWID;
            public string DrCode;
            public string TDATE;
            public string JDATE;
            public string CARNO;
            public string DRIVER;
            public string COPNAME;
            public string Gubun;

        }
        public static Table_MISU_IDMST TMM;

        /// <summary>
        /// Description : 미수 마스터 NEW
        /// Author : 김민철
        /// Create Date : 2017.09.14
        /// </summary>
        ///  <seealso cref="MUMAST.BAS"/>
        public struct Table_MISU_IDNEW
        {
            public long WRTNO;
            public string MisuID;
            public string BDate;
            public string Class;
            public string YYMM;
            public string IpdOpd;
            public string Bi;
            public string GelCode;
            public string Bun;
            public string FromDate;
            public string ToDate;
            public int Ilsu;
            public string DeptCode;
            public string MgrRank;
            public int[] Qty;
            public double[] Amt;
            public double JAmt;
            public string GbEnd;
            public string Remark;
            public string JepsuNo;
            public string TongGbn;
            public string MirYYMM;
            public string EdiMirNo;
            public string ChaSu;
            public string MukNo;
            public string ROWID;
            public string DrCode;
            public string TDATE;
            public string JDATE;
            public string CARNO;
            public string DRIVER;
            public string COPNAME;
            public string Gubun;
        }
        public static Table_MISU_IDMST TMN;

        /// <summary>
        /// Description : 미수 SLIP
        /// Author : 김민철
        /// Create Date : 2017.09.14
        /// </summary>
        /// <seealso cref="MUMAST.BAS"/>
        public struct Table_MISU_SLIP
        {
            public string[] Del;
            public string[] Gubun;
            public int[] Qty;
            public double[] TAmt;
            public double[] Amt;
        }
        public static Table_MISU_SLIP TMS;

        /// <summary>
        /// Description : 선택진료비 산정관련 Arguments 정의 -> 함수호출시 인자값으로 던져서 처리함
        /// Author : 김민철
        /// Create Date : 2017.09.15
        /// </summary>
        public struct Sel_Main_MST
        {
            public string ArgSpc;
            public string ArgIO;
            public string ArgPano;
            public string ArgBDate;
            public string ArgBi;
            public string ArgGamek;
            public string ArgDeptCode;
            public string ArgDrCode;
            public string ArgBun;
            public string ArgSuCode;
            public string argSUNEXT;
            public long ArgIPDNO;
            public string ArgETC;
        }

        /// <summary>
        /// Description : 외래용 수가정보 Read 변수
        /// Author : 박병규
        /// Create Date : 2017.09.25
        /// </summary>
        /// <seealso cref="OPDACCT:OPDACCT.bas"/>
        public struct Slip_Accept_Table
        {
            public long OpdNo;
            public string Bi;
            public double Qty;
            public int Nal;
            public string Imiv;
            public string Dev;
            public string WardCode;
            public string GbNgt;
            public string GbSpc;
            public string GbSelf;
            public long BaseAmt;
            public string SuCode;
            public string CSuCode;
            public string SuNext;
            public string CSuNext;
            public string Bun;
            public string CBun;
            public string Nu;
            public string SugbA;
            public string SugbB;
            public string SugbC;
            public string SugbD;
            public string SugbE;
            public string SugbF;
            public string SugbG;
            public string SugbH;
            public string SugbI;
            public string SugbJ;
            public string SugbK;
            public string SugbL;
            public string SugbM;//퇴장방지의약품(0.아님 1.방지의약품)
            public string SugbO;//의약분업(0.원외 1-B:원내조제)
            public string SugbP; //프로그램에 사용않함
            public string SugbQ;//산재급여(1.무조건 급여)
            public string SugbR; //자보 비급여중(0.급여, 1.비급여)
            public string SugbS;//100/100
            public string SugbW;//치매조기검진(0.일반,1.진단,2.감별) '2010-03-08
            public string SugbX; //자보/산재 급여초음파 기술료 가산 '2013-10-25
            public string SugbY; //외과가산여부 2014-07-30
            public string SugbZ; //흉부외과 가산여부 2014-07-30
            public long Iamt;
            public long Tamt;
            public long Bamt;
            public long Samt;
            public long Selamt;
            public string DtlBun;
            public string GbSpc_No;
            public string Sudate;
            public long OldIamt;
            public long OldTamt;
            public long OldBamt;
            public int DayMax;
            public int TotMax;
            public string SunameK;
            public string Hcode;
            public double OrderNo;
            public string GbTuyak;
            public string Remark;
            public string GbInfo;
            public string SlipNo;
            public string GbIPD;
            public string DrCode;
            public string Multi;
            public string MultiRemark;
            public int Div;
            public string Dur;
            public string KsJin;
            public string ScodeSayu;
            public string ScodeRemark;
            public string GbTax;
            public string GbHost;
            public string SugbAA; //응급가산
            public string SugbAB; //판독가산
            public string SugbAC; //마취가산
            public string SugbAD; //화상
            public string GSADD;  //외과 흉부외과 가산구분
            public string OpGubun;//수술구분(0.주수술,1.부수술,2.제2수술,D.부수술 100%)
            public string Bcode;  //보험코드

        }
        //public static Slip_Accept_Table[] SA = new Slip_Accept_Table[999];
        public static Slip_Accept_Table[] SA;

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.01.22
        /// </summary>
        /// <seealso cref="OPDACCT:OPDACCT.bas"/>
        public struct Slip_Gesan_Table
        {
            public long OpdNo;
            public double Qty;
            public int Nal;
            public string GbNgt;
            public string GbSpc;
            public string GbSelf;
            public long BaseAmt;
            public string WardCode;
            public string SuCode;
            public string SuNext;
            public string Bun;
            public string CSuCode;
            public string CSuNext;
            public string CBun;
            public string Nu;
            public string SugbA;
            public string SugbB;
            public string SugbC;
            public string SugbD;
            public string SugbE;
            public string SugbF;
            public string SugbG;
            public string SugbH;
            public string SugbI;
            public string SugbJ;
            public string SugbK;
            public string SugbL;
            public string SugbM; //퇴장방지의약품(0.아님 1.방지의약품)
            public string SugbO;
            public string SugbP;
            public string SugbQ;
            public string SugbR;
            public string SugbS; //100/100
            public string SugbW; //치매조기검진(0.일반,1.진단,2.감별) '2010-03-08
            public string SugbX; //자보/산재 급여초음파 기술료 가산 '2013-10-25
            public string SugbY; //외과가산여부 2014-07-30
            public string SugbZ; //흉부외과 가산여부 2014-07-30
            public long Iamt;
            public long Tamt;
            public long Bamt;
            public long Samt;
            public long Selamt;
            public string DtlBun;
            public string GbSpc_No;
            public string Sudate;
            public long  OldIamt;
            public long  OldTamt;
            public long OldBamt;
            public string Sudate3;
            public long Iamt3;
            public long Tamt3;
            public long Bamt3;
            public string Sudate4;
            public long Iamt4;
            public long Tamt4;
            public long Bamt4;
            public string Sudate5;
            public long Iamt5;
            public long Tamt5;
            public long Bamt5;
            public int TotMax;
            public string DelDate;
            public double OrderNo;
            public double OrderNo1;
            public string Remark;
            public string GbTuyak;
            public string Imiv;
            public string Dev;
            public string GbIPD; 
            public string DrCode;
            public string Multi;
            public string MultiRemark;
            public int Div;
            public string Dur;
            public string KsJin;
            public string ScodeSayu;
            public string ScodeRemark;
            public string GbTax;
            public string GbHost;
            public string OrderTime;
            public string SugbAA; //응급가산
            public string SugbAB; //판독가산
            public string SugbAC; //마취가산
            public string SugbAD; //화상
            public string GSADD;  //외과 흉부외과 가산구분
            public string OpGubun;//수술구분(0.주수술,1.부수술,2.제2수술,D.부수술100%)
            public string Bcode;  //보험코드
        }
        public static Slip_Gesan_Table SG;

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2018.01.22
        /// </summary>
        /// <seealso cref="BasOpdSR.bas"/>
        public struct Slip_Return_Table
        {
            public long OpdNo;
            public string SuCode;
            public string CSuCode;
            public string SuNext;
            public string CSuNext;
            public string Bi;
            public string BDate;
            public string Bun;
            public string CBun;
            public string Nu;
            public double Qty;
            public int Nal;
            public long BaseAmt;
            public string GbSpc;
            public string GbNgt;
            public string GbGisul;
            public string GbSelf;
            public string GbChild;
            public string DrCode;
            public string WardCode;
            public string GbSlip;
            public string GbHost;
            public long Amt1;
            public long Amt2;
            public double Amt4;
            public double OrderNo;
            public string GbImiv;
            public string DosCode;
            public string GbBunup;
            public string GbIPD;
            public string GbSugbS;
            public long CardSeqNo;
            public string ABCDate;
            public string OPER_Dept;
            public string OPER_Dct;
            public string Multi;
            public string MultiRemark;
            public int Div;
            public string Dur;
            public string KsJin;
            public string ScodeSayu;
            public string ScodeRemark;
            public long OgAmt;
            public long DanAmt;
            public string GbSpc_No;
            public long DrSabun;
            public long Jamt;
            public long Bamt;
            public string GbEr;   //응급가산
            public string SugbAB; //판독가산
            public string SugbAC; //마취가산
            public string SugbAD; //화상
            public string GSADD;  //외과 흉부외과 가산구분
            public string OpGubun;//수술구분(0.주수술,1.부수술,2.제2수술,D.부수술100%)
            public string Bcode;  //보험코드
        }
        //public static Slip_Return_Table[] SR = new Slip_Return_Table[999];
        public static Slip_Return_Table[] SR;

        /// <summary>
        /// Description : 외래용 수가정보 Write 변수
        /// Author : 박병규
        /// Create Date : 2018.01.19
        /// </summary>
        /// <seealso cref="BasOpdSR.bas"/>
        public struct Slip_Write_Table
        {
            public long OpdNo;
            public string SuCode;
            public string CSuCode;
            public string SuNext;
            public string CSuNext;
            public string Bun;
            public string CBun;
            public string Nu;
            public double Qty;
            public int Nal;
            public long BaseAmt; //기본단가
            public string GbNgt; //심야가산
            public string GbSpc;
            public string GbGisul; //기술료가산
            public string GbSelf; //급여구분
            public string GbChild; //소아가산
            public string GbHost;
            public long Amt1;
            public long Amt2;
            public double OrderNo;
            public string GbTuyak;
            public string GbImiv;
            public string DosCode;
            public string GbBunup;
            public string GbIPD;
            public string DrCode;
            public string Multi;
            public string MultiRemark;
            public int Div;
            public string Dur;
            public string KsJin;
            public string GbSugbL;
            public string GbSugbS;
            public string ScodeSayu;
            public string ScodeRemark;
            public string GbSpc_No;
            public long DrugAmt;
            public long Jamt;
            public long Bamt;
            public long Amt4;
            public string GbEr;   //응급가산
            public string SugbAB; //판독가산
            public string SugbAC; //마취가산
            public string SugbAD; //화상
            public string GSADD;  //외과 흉부외과 가산구분
            public string OpGubun;//수술구분(0.주수술,1.부수술,2.제2수술,D.부수술100%)
            public string Bcode;  //보험코드
            public string Powder;  //powder
        }
        public static Slip_Write_Table[] SW;

        /// <summary>
        /// Description : 외래 영수증 출력용 변수
        /// Author : 김민철
        /// Create Date : 2017.11.16
        /// </summary>
        /// <seealso cref="Report_Print.bas"/>
        /// <history>RePort_Add_Amt_Table 으로 통합됨 2017.11.16</history>
        //public struct OPD_RePort_Add_Amt_Table
        //{
        //    public long[] Amt1;     //급여금액-합
        //    public long[] Amt2;     //비급여금액-합
        //    public long[] Amt3;     //특진료-합
        //    public long[] Amt4;     //본인총액-합
        //    public long[] Amt5;     //본인
        //    public long[] Amt6;     //공단
        //    public long[] Amt7;     //선별급여 총액
        //    public long[] Amt8;     //선별급여 조합
        //    public long[] Amt9;     //선별급여 본인
        //}
        //public static OPD_RePort_Add_Amt_Table oRPG;

        /// <summary>
        /// Description : 입원 영수증 출력용 변수
        /// Author : 김민철
        /// Create Date : 2017.09.25
        /// </summary>
        /// <seealso cref="Report_Print2.bas"/>
        /// <history>RePort_Add_Amt_Table 으로 통합됨 2017.11.16</history>
        //public struct IPD_RePort_Add_Amt_Table
        //{
        //    public long[] Amt1;     //급여금액-합
        //    public long[] Amt2;     //비급여금액-합
        //    public long[] Amt3;     //특진료-합
        //    public long[] Amt4;     //본인총액-합
        //    public long[] Amt5;     //본인
        //    public long[] Amt6;     //공단
        //    public long[] Amt7;     //선별급여 총액
        //    public long[] Amt8;     //선별급여 조합
        //    public long[] Amt9;     //선별급여 본인
        //}
        //public static IPD_RePort_Add_Amt_Table iRPG;

        /// <summary>
        /// Description : ARC 계산용 변수
        /// Author : 김민철
        /// Create Date : 2017.09.27
        /// </summary>
        /// <seealso cref="ICUPDT.bas"/>
        public struct Slip_ArcGesan_Table
        {
            public string GbSpc;
            public string GbHost;
            public double Qty;
            public int Nal;
            public long Amt1;
            public long Amt2;
        }
        public static Slip_ArcGesan_Table ARC;

        /// <summary>
        /// Description : Suga_Read() 호출시 수가정보를 담는 구조체 변수
        /// Author : 김민철
        /// Create Date : 2017.09.30
        /// </summary>
        public struct Read_Suga
        {
            public string SuCode;
            public string SuNext;
            public string Bun;
            public string Nu;
            public string SugbA;
            public string SugbB;
            public string SugbC;
            public string SugbD;
            public string SugbE;
            public string SugbF;
            public string SugbG;
            public string SugbH;
            public string SugbI;
            public string SugbJ;
            public string SugbK;
            public string SugbM;
            public string SugbO;
            public string SugbP;
            public string SugbQ;
            public string SugbR;
            public string SugbS;
            public string SugbW;
            public string SugbX;
            public string SugbY;
            public string SugbZ;
            public string SugbAA;
            public string SugbAB;
            public string SugbAC;
            public string SugbAD;
            public string SugbAG;
            public string TotMax;
            public long IAmt;
            public long TAmt;
            public long BAmt;
            public string SuDate;
            public long OldIAmt;
            public long OldTAmt;
            public long OldBAmt;
            public string SuDate3;
            public long IAmt3;
            public long TAmt3;
            public long BAmt3;
            public string SuDate4;
            public long IAmt4;
            public long TAmt4;
            public long BAmt4;
            public string SuDate5;
            public long IAmt5;
            public long TAmt5;
            public long BAmt5;
            public string GBNS;
            public string DelDate;
        }
        public static Read_Suga RS;

        /// <summary>
        /// Description : BAS_ROOM 정보 담는 변수 (주로 입원 ARC에서 사용함)
        /// Author : 김민철
        /// Create Date : 2017.10.06
        /// </summary>
        /// <seealso cref="ICUPDT.bas"/>
        public struct Table_Bas_Room
        {
            public string WardCode;     //병동코드
            public string RoomCode;     //병실코드
            public string RoomClass;    //병실등급
            public string Sex;          //남여구분
            public string DeptCode;     //관리과목
            public int Tbed;            //총Bed 수
            public int Hbed;            //현Bed 수
            public int Bbed;            //계산서Bed 수
            public int Gbed;            //가퇴원Bed 수
            public long WardAmt;        //병실료
            public long PantAmt;        //환자관리료 (일반)
            public long RondAmt;        //회진료
            public long FoodAMT;        //식대
            public long OverAmt;        //실료차

        }
        public static Table_Bas_Room TBR;


        /// <summary>
        /// Description : TRePort_Add_Amt_Table 정보 담는 변수
        /// Author : 안정수
        /// Create Date : 2017.10.10
        /// </summary>
        public struct TRePort_Add_Amt_Table
        {
            public long[] TotAmt1;
            public long[] TotAmt2;
            public long[] TotAmt3;
            public long[] TotAmt4;

            public long[] TotAmt5;
            public long[] TotAmt6;
            public long[] TotAmt7;

            public long[] TotAmt8;
            public long[] TotAmt9;
        }
        public static TRePort_Add_Amt_Table TRPG;


        /// <summary>
        /// Description : RePort_Add_Amt_Table 정보 담는 변수
        /// Author : 안정수
        /// Create Date : 2017.10.10
        /// </summary>
        /// <history> 선별급여는 각 배열안으로 합침 2018.01.11 KMC</history>
        public struct RePort_Add_Amt_Table
        {
            public long[] Amt1;     //급여총액
            public long[] Amt2;     //비급여금액(선택진료이외)
            public long[] Amt3;     //선택진료
            public long[] Amt4;     //전액본인부담
            public long[] Amt5;     //본인부담금
            public long[] Amt6;     //공단부담금
            public long[] Amt7;     //선별급여 총액
            public long[] Amt8;     //선별급여 조합
            public long[] Amt9;     //선별급여 본인
        }
        public static RePort_Add_Amt_Table RPG;


        /// <summary>
        /// Description : Argument_Table2 정보 담는 변수
        /// Author : 안정수
        /// Create Date : 2017.10.10
        /// </summary>
        public struct Argument_Table2
        {
            public string Date;
            public string Dept;
            public string Sex;
            public string GbSpc;
            public string GbGameK;
            public int Retn;
            public int Bi;
            public int Bi1;
            public int Age;
            public int AgeiLsu;
            public string Gbilban2;
            public string Pano;
            public string DrCode;
        }
        public static Argument_Table2 RPA;


        /// <summary>
        /// Description : Slip_ReportPrint_Table 정보 담는 변수
        /// Author : 안정수
        /// Create Date : 2017.10.10
        /// </summary>
        public struct Slip_ReportPrint_Table
        {
            public string[] Sucode;
            public string[] Sunext;
            public string[] Bi;
            public string[] BDate;
            public string[] Bun;

            public string[] Nu;
            public double[] Qty;
            public int[] Nal;
            public long[] BaseAmt;
            public string[] GbSpc;

            public string[] GbNgt;
            public string[] GbGisul;
            public string[] GbSelf;
            public string[] GbChild;
            public string[] DrCode;

            public string[] DeptCode;
            public string[] WardCode;
            public string[] GbSlip;
            public string[] GbHost;
            public long[] Amt1;

            public long[] Amt2;
            public double[] OrderNo;
            public string[] GbImiv;
            public string[] DosCode;
            public string[] GbBunup;

            public string[] GBIPD;
            public int[] Div;
            public string[] KsJin;
            public long[] DanAmt;
            public string[] GbSpc_No;   //2011-08-22
            public string[] SugbS;   //2011-08-22
        }
        public static Slip_ReportPrint_Table RP;


        /// <summary>
        /// Description : Argument_Table 정보 담는 변수
        /// Author : 안정수
        /// Create Date : 2017.10.10
        /// </summary>
        public struct Argument_Table
        {
            public string Date;
            public string Dept;
            public string Sex;
            public string GbSpc;
            public string GbGameK;
            public string GbGameKC;
            public string Sang1;
            public string Sang2;
            public string Sang3;
            public int Retn;
            public int Bi;
            public int Bi1;
            public int Age;
            public int Fee1;
            public int Fee2;
            public int Fee3;
            public int Fee31;
            public int Fee5;
            public int Fee51;
            public int Fee7;
            public int AgeiLsu;
            public string Gbilban2; //외국new
            public string Pano;
            public string DrCode;
            public string Jtime;
        }
        public static Argument_Table a;

        /// <summary>
        /// Description : IPD_RePort_Add_DrgAmt_Table 정보 담는 변수
        /// Author : 안정수
        /// Create Date : 2017.10.12
        /// </summary>
        /// <history> clsPmpaType에서 DRPG 변수 옮기면서 중복으로 주석처리함 18.01.10 KMC</history>
        //public struct IPD_RePort_Add_DrgAmt_Table
        //{
        //    public long[] Amt1; //급여금액-합
        //    public long[] Amt2; //비급여금액-합
        //    public long[] Amt3; //특진료-합
        //    public long[] Amt4; //본인총액-합

        //    public long[] Amt5; //본인
        //    public long[] Amt6; //공단

        //    public long[] Amt7; //선별급여 총액
        //    public long[] Amt8; //선별급여 조합
        //    public long[] Amt9; //선별급여 본인
        //}
        //public static IPD_RePort_Add_DrgAmt_Table DRPG;

        /// <summary>
        /// Description : 외래 진료비 본인부담율 변수
        /// Author : 김민철
        /// Create Date : 2017.11.13
        /// </summary>
        public struct OPD_Bon_Rate
        {
            public int Jin;         //진찰료
            public int Bohum;       //진료비
            public int CTMRI;       //CT/MRI
            public int Food;        //식대료
            public int Dt1;         //틀니
            public int Dt2;         //임플란트
            public long FAmt1;      //진료비 정액 
            public long FAmt2;      //진료비 정액(직접조제)
            public string SDate;    //적용일자
        }
        public static OPD_Bon_Rate OBR;

        /// <summary>
        /// Description : 입원 진료비 본인부담율 변수
        /// Author : 김민철
        /// Create Date : 2017.11.13
        /// </summary>
        public struct IPD_Bon_Rate
        {
            public int Jin;         //진찰료
            public int Bohum;       //진료비
            public int CTMRI;       //CT/MRI
            public int Food;        //식대료
            public int Dt1;         //틀니
            public int Dt2;         //임플란트
            public long FAmt1;      //진료비 정액 
            public long FAmt2;      //진료비 정액(직접조제)
            public string SDate;    //적용일자
        }
        public static IPD_Bon_Rate IBR;


        /// <summary>
        /// Description : 원외처방전마스터
        /// Author : 박병규
        /// Create Date : 2017.11.17
        /// </summary>
        /// <seealso cref="Drug_out_atc.bas:Type_OutDrugMst"/>
        public struct Type_OutDrugMst
        {
            public string SlipDate;
            public int SlipNo;
            public string Ptno;
            public string Sname;
            public int Age;
            public string Sex;
            public string BDate;
            public string DeptCode;
            public string DrCode;
            public string DrName;
            public long DrBunho; 
            public string Bi;
            public string Jumin1; 
            public string Jumin2;
            public string Flag; 
            public string PrtDept;
            public string ActDate;
            public string Part;
            public int SeqNo; 
            public string EntDate;
            public string SendDate;
            public string PrtDate;
            public string Diease1; 
            public string Diease2;
            public string Diease1_RO;
            public string Diease2_RO; 
            public string Remark; 
            public string IpdOpd;
            public string PrtBun; 
            public string HapPrint;
            public string Singu; 
            public string GbV252;
            public string GbV352;
            public string ROWID; 
        }
        public static Type_OutDrugMst TODM;

        /// <summary>
        /// Description : 수가가산 산정기준 Bas_ADD 
        /// Author : 김민철
        /// Create Date : 2017.12.04
        /// </summary>
        public class cBas_Add
        {
            public string GUBUN = "";
            public string PCODE = "";
            public string GBCHILD = "";
            public string NIGHT = "";
            public string MIDNIGHT = "";
            public string GBER = "";
            public string HOLIDAY = "";
            public string ADD1 = "";
            public string ADD2 = "";
            public string ADD3 = "";
            public string ADD4 = "";
            public string ADD5 = "";
            public string ADD6 = "";
            public string ADD7 = "";
            public string ADD8 = "";
            public string ADD9 = "";
            public string SDATE = "";
            public string EDATE = "";
            public string DELDATE = "";
            public string ENTDATE = "";
            public string ENTSABUN = "";
            public string ROWID = "";
        }

        /// <summary>
        /// Description : 수가가산 참조 Argument
        /// Author : 김민철
        /// Create Date : 2017.12.18
        /// </summary>
        public class cBas_Add_Arg
        {
            public int Bi = 0;              //자격
            public int AGE = 0;             //나이
            public double AGEILSU = 0;      //개월수(0세이하)
            
            public string BDATE = "";       //처방일자
            public string NIGHT = "";       //야간, 공휴
            public string MIDNIGHT = "";    //심야
            public string GBER = "";        //응급가산
            public string ADD1 = "";        //가산1
            public string ADD2 = "";        //가산2
            public string ADD3 = "";        //가산3
            public string ADD4 = "";
            public string ADD5 = "";
            public string ADD6 = "";
            public string ADD7 = "";
            public string SUNEXT = "";      //수가코드
            public string SUGBE = "";       //수가 E항
            public string SUGBB = "";       //수가 E항

            public string BUN = "";         //분류
            
            public string AN1 = "";         //마취가산              (가산항목 정리 후 값 변경)
            public string AN2 = "";         //ASA마취가산           (가산항목 정리 후 값 변경)
            public string OP1 = "";         //외과, 흉부외과 가산   (가산항목 정리 후 값 변경)
            public string OP2 = "";         //화상 가산             (가산항목 정리 후 값 변경)
            public string OP3 = "";         //수술, 부수술, 제2수술 (가산항목 정리 후 값 변경)
            public string OP4 = "";         //고위험산모            (가산항목 정리 후 값 변경)
            public string XRAY1 = "";       //판독 가산             (가산항목 정리 후 값 변경)
            public string OG = "";          //분만수가 구분
            public string ADD8 = "";        //신경가산             (가산항목 정리 후 값 변경)
            public string ADD9 = "";        //ASA가산             (가산항목 정리 후 값 변경)
        }

        /// <summary>
        /// Description : 입원환자 금액 영수증 표기용 변수
        /// Author : 김민철
        /// Create Date : 2018.01.10
        /// </summary>
        /// <history> 입원/외래 영수증용 변수 공용으로 사용함 18.01.11 KMC</history>
        //public struct IRPG
        //{
        //    public long[] Amt1;   //급여금액 
        //    public long[] Amt2;   //비급여금액 
        //    public long[] Amt3;   //특진료 
        //    public long[] Amt4;   //본인총액
        //    public long[] Amt5;   //본인부담금
        //    public long[] Amt6;   //공단부담금
        //    //선별급여는 배열안으로 포함시킴
        //    //public long[] Amt7;   //선별급여 총액
        //    //public long[] Amt8;   //선별급여 공단
        //    //public long[] Amt9;   //선별급여 본인
        //}
        //public static IRPG iRPG;

        /// <summary>
        /// Description : DRG 금액 영수증 표기용 변수
        /// Author : 김민철
        /// Create Date : 2018.01.10
        /// </summary>
        /// <history> 입원/외래 영수증용 변수 공용으로 사용함 18.01.11 KMC</history>
        //public struct DRPG
        //{
        //    public long[] Amt1;
        //    public long[] Amt2;
        //    public long[] Amt3;
        //    public long[] Amt4;
        //    public long[] Amt5;
        //    public long[] Amt6;
        //    public long[] Amt7;
        //    public long[] Amt8;
        //    public long[] Amt9;
        //}
        //public static DRPG dRPG;

        /// <summary>
        /// Description : 그룹수가정보 변수
        /// Author : 박병규
        /// Create Date : 2018.01.29
        /// </summary>
        /// <seealso cref="OPDACCT.bas"/>
        public struct Slip_Host_Table
        {
            public string SuCode;
            public double Qty;
            public int Nal;
            public string Imiv;
            public string Dev;
            public string GbNgt;
            public string GbSpc;
            public string GbSelf;
            public string SugbO; //의약분업(0.원외 1-B:원내조제)
            public long BaseAmt;
        }
        public static Slip_Host_Table SH;

        /// <summary>
        /// Description : 신용카드 승인 요청전문 변수
        /// Author : 김민철
        /// Create Date : 2018.02.09
        /// </summary>
        public struct AcctReqData
        {
            public string VanGb;
            public string OrderGb;
            public string MDate;
            public long SEQNO;
            public string OrderNo;
            public string ClientId;
            public string EntryMode;
            public int CardLength;
            public string CardData;
            public string CardData2;
            public int CardDivide;
            public long TotAmt;
            public string OldAppDate;
            public string OldAppTime;
            public string OldAppNo;
            public string SectionNo;
            public string SiteID;
            public long CardSeqNo;
            public string Cardthru;
            public string ASaleDate;
            public string AApproveNo;
            public string Gubun;
            public string CashBun;
            public long OgAmt;
            public string CanSayu;
            public string KeyIn;
        }
        public static AcctReqData RSD;

        /// <summary>
        /// Description : 신용카드 승인 응답전문 변수
        /// Author : 김민철
        /// Create Date : 2018.02.09
        /// </summary>
        public struct AcctResData
        {
            public string VanGb;
            public string OrderGb;
            public string MDate;
            public long SEQNO;
            public string OrderNo;
            public string ClientId;
            public string ReplyStat;
            public string ApprovalDate;
            public string ApprovalTime;
            public string ApprovalNo;
            public string BankId;
            public string BankName;
            public string MemberNo;
            public string PublishBank;
            public string CardName;
            public string Massage;
            public string HPay;
            public string Trade;    //정상승인여부 (1000, 2000)
            public string ReqMsg;   //응답코드(0000 : 정상승인)
        }
        public static AcctResData RD;

        /// <summary>
        /// Description : IPD_NEW_CASH Table Argument
        /// Author : 김민철
        /// Create Date : 2018.02.19
        /// </summary>
        public class Ipd_New_Cash
        {
            public long IPDNO = 0;
            public long TRSNO = 0;
            public string ACTDATE = "";
            public string PANO = "";
            public string BI = "";
            public string BDATE = "";
            public string SUNEXT = "";
            public string BUN = "";
            public double QTY = 1;
            public int NAL = 1;
            public long AMT = 0;
            public string DEPTCODE = "";
            public string DRCODE = "";
            public string GBGAMEK = "";
            public string GELCODE = "";
            public long CARDSEQNO = 0;
            public string BIGO = "";
            public string PART = "";
            public string ENTDATE = "";
            public string CARDGB = "";
            public long REMAMT = 0;
            public long OGAMT = 0;
            public long AMT2 = 0;
            public string GBGAMEKC = "";
        }

        /// <summary>
        /// 수가 가산관련 계산 후 Return Value Class
        /// </summary>
        public class Bas_Acc_Rtn
        {
            public long BAMT    = 0;    //가산전 원단가
            public long BASEAMT = 0;    //가산후 단가
            public long AMT     = 0;    //계산금액
            public string PCODE = "";   //EDI코드
        }
        
        /// <summary>
        /// 외래접수마스터 Clear
        /// </summary>
        /// <param name="TOM"></param>
        public void Clear_Type_Opd_Master()
        {
            clsPmpaType.TOM.OpdNo = 0;
            clsPmpaType.TOM.Pano = "";
            clsPmpaType.TOM.DeptCode = "";
            clsPmpaType.TOM.Bi = "";
            clsPmpaType.TOM.sName = "";
            clsPmpaType.TOM.Sex = "";
            clsPmpaType.TOM.Age = 0;
            clsPmpaType.TOM.Jiyuk = "";
            clsPmpaType.TOM.DrCode = "";
            clsPmpaType.TOM.Reserved = "";
            clsPmpaType.TOM.Chojae = "";
            clsPmpaType.TOM.GbGameK = "";
            clsPmpaType.TOM.GbGameKC = "";
            clsPmpaType.TOM.GbSpc = "";
            clsPmpaType.TOM.Jin = "";
            clsPmpaType.TOM.Sinwhan = "";
            clsPmpaType.TOM.Bohun = "";
            clsPmpaType.TOM.Change = "";
            clsPmpaType.TOM.Sheet = "";
            clsPmpaType.TOM.Rep = "";
            clsPmpaType.TOM.Part = "";
            clsPmpaType.TOM.JTime = "";
            clsPmpaType.TOM.Stime = "";
            clsPmpaType.TOM.Fee1 = 0;
            clsPmpaType.TOM.Fee2 = 0;
            clsPmpaType.TOM.Fee3 = 0;
            clsPmpaType.TOM.Fee31 = 0;
            clsPmpaType.TOM.Fee5 = 0;
            clsPmpaType.TOM.Fee51 = 0;
            clsPmpaType.TOM.Fee7 = 0;
            clsPmpaType.TOM.Amt1 = 0;
            clsPmpaType.TOM.Amt2 = 0;
            clsPmpaType.TOM.Amt3 = 0;
            clsPmpaType.TOM.Amt4 = 0;
            clsPmpaType.TOM.Amt5 = 0;
            clsPmpaType.TOM.Amt6 = 0;
            clsPmpaType.TOM.Amt7 = 0;
            clsPmpaType.TOM.GelCode = "";
            clsPmpaType.TOM.BDate = "";
            clsPmpaType.TOM.ActDate = "";
            clsPmpaType.TOM.WardCode = "";
            clsPmpaType.TOM.RoomCode = 0;
            clsPmpaType.TOM.Bunup = "";
            clsPmpaType.TOM.OCSJIN = "";
            clsPmpaType.TOM.ROWID = "";
            clsPmpaType.TOM.CardSeqNo = 0;
            clsPmpaType.TOM.Emr = "";
            clsPmpaType.TOM.VCode = "";
            clsPmpaType.TOM.MCode = "";
            clsPmpaType.TOM.MSeqNo = "";
            clsPmpaType.TOM.MQCODE = "";
            clsPmpaType.TOM.MksJin = "";
            clsPmpaType.TOM.GbNight = "";
            clsPmpaType.TOM.GbFlu_Vac = "";
            clsPmpaType.TOM.OldMan = "";
            clsPmpaType.TOM.GbExam = "";
            clsPmpaType.TOM.GbDementia = "";
            clsPmpaType.TOM.Jtime2 = "";
            clsPmpaType.TOM.Gbilban2 = "";
            clsPmpaType.TOM.GbFlu_Ltd = "";
            clsPmpaType.TOM.JinDtl = "";
            clsPmpaType.TOM.INSULIN = "";
            clsPmpaType.TOM.OUTTIME = "";
            clsPmpaType.TOM.DrSabun = 0;
            clsPmpaType.TOM.J2_Sayu = "";
            clsPmpaType.TOM.ResMemo = "";
            clsPmpaType.TOM.Jiwon = "";
            clsPmpaType.TOM.KTASLEVL = "";
            clsPmpaType.TOM.GBRES = "";
            clsPmpaType.TOM.GBPOWDER = "";
            clsPmpaType.TOM.YHOSP_KIHO = "";
            clsPmpaType.TOM.GbFlu = "";     //2021-09-16
        }
        
        public void Set_Type_Opd_Master(DataTable DtTOM)
        {
            clsPmpaFunc cPF = new clsPmpaFunc();
            clsIpdAcct cIAcct = new clsIpdAcct();
            clsOumsad cPO = new clsOumsad();
            clsAlert cA = new ComPmpaLibB.clsAlert();

            clsPmpaType.BonRate cBON = new BonRate();
            string strJuminNo = "";

            clsPmpaType.TOM.OpdNo = Convert.ToInt64(VB.Val(DtTOM.Rows[0]["OPDNO"].ToString()));
            clsPmpaType.TOM.Pano = DtTOM.Rows[0]["PANO"].ToString();
            clsPmpaType.TOM.DeptCode = DtTOM.Rows[0]["DEPTCODE"].ToString();
            clsPmpaType.TOM.Bi = DtTOM.Rows[0]["BI"].ToString();
            clsPmpaType.TOM.sName = DtTOM.Rows[0]["SNAME"].ToString();
            clsPmpaType.TOM.Sex = DtTOM.Rows[0]["SEX"].ToString();
            clsPmpaType.TOM.Age = Convert.ToInt32(DtTOM.Rows[0]["AGE"].ToString());
            clsPmpaType.TOM.Jiyuk = DtTOM.Rows[0]["JICODE"].ToString();
            clsPmpaType.TOM.DrCode = DtTOM.Rows[0]["DRCODE"].ToString();
            clsPmpaType.TOM.Reserved = DtTOM.Rows[0]["RESERVED"].ToString();
            clsPmpaType.TOM.Chojae = DtTOM.Rows[0]["CHOJAE"].ToString();
            clsPmpaType.TOM.GbGameK = DtTOM.Rows[0]["GBGAMEK"].ToString();
            clsPmpaType.TOM.GbGameKC = DtTOM.Rows[0]["GBGAMEKC"].ToString();
            clsPmpaType.TOM.GbSpc = DtTOM.Rows[0]["GBSPC"].ToString();
            clsPmpaType.TOM.Jin = DtTOM.Rows[0]["JIN"].ToString();
            clsPmpaType.TOM.Sinwhan = DtTOM.Rows[0]["SINGU"].ToString();
            clsPmpaType.TOM.Bohun = DtTOM.Rows[0]["BOHUN"].ToString();
            clsPmpaType.TOM.Change = DtTOM.Rows[0]["CHANGE"].ToString();
            clsPmpaType.TOM.Sheet = DtTOM.Rows[0]["SHEET"].ToString();
            clsPmpaType.TOM.Rep = DtTOM.Rows[0]["REP"].ToString();
            clsPmpaType.TOM.Part = DtTOM.Rows[0]["PART"].ToString();
            clsPmpaType.TOM.JTime = DtTOM.Rows[0]["JTIME"].ToString();
            clsPmpaType.TOM.Stime = DtTOM.Rows[0]["STIME"].ToString();
            clsPmpaType.TOM.Fee1 = Convert.ToInt32(VB.Val(DtTOM.Rows[0]["FEE1"].ToString()));
            clsPmpaType.TOM.Fee2 = Convert.ToInt32(VB.Val(DtTOM.Rows[0]["FEE2"].ToString()));
            clsPmpaType.TOM.Fee3 = Convert.ToInt32(VB.Val(DtTOM.Rows[0]["FEE3"].ToString()));
            clsPmpaType.TOM.Fee31 = Convert.ToInt32(VB.Val(DtTOM.Rows[0]["FEE31"].ToString()));
            clsPmpaType.TOM.Fee5 = Convert.ToInt32(VB.Val(DtTOM.Rows[0]["FEE5"].ToString()));
            clsPmpaType.TOM.Fee51 = Convert.ToInt32(VB.Val(DtTOM.Rows[0]["FEE51"].ToString()));
            clsPmpaType.TOM.Fee7 = Convert.ToInt32(VB.Val(DtTOM.Rows[0]["FEE7"].ToString()));
            clsPmpaType.TOM.Amt1 = Convert.ToInt64(VB.Val(DtTOM.Rows[0]["AMT1"].ToString()));
            clsPmpaType.TOM.Amt2 = Convert.ToInt64(VB.Val(DtTOM.Rows[0]["AMT2"].ToString()));
            clsPmpaType.TOM.Amt3 = Convert.ToInt64(VB.Val(DtTOM.Rows[0]["AMT3"].ToString()));
            clsPmpaType.TOM.Amt4 = Convert.ToInt64(VB.Val(DtTOM.Rows[0]["AMT4"].ToString()));
            clsPmpaType.TOM.Amt5 = Convert.ToInt64(VB.Val(DtTOM.Rows[0]["AMT5"].ToString()));
            clsPmpaType.TOM.Amt6 = Convert.ToInt64(VB.Val(DtTOM.Rows[0]["AMT6"].ToString()));
            clsPmpaType.TOM.Amt7 = Convert.ToInt64(VB.Val(DtTOM.Rows[0]["AMT7"].ToString()));
            clsPmpaType.TOM.GelCode = DtTOM.Rows[0]["GELCODE"].ToString();
            clsPmpaType.TOM.BDate = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(DtTOM.Rows[0]["BDATE"].ToString()));
            clsPmpaType.TOM.ActDate = string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(DtTOM.Rows[0]["ACTDATE"].ToString()));
            clsPmpaType.TOM.WardCode = "";
            clsPmpaType.TOM.RoomCode = 0;
            clsPmpaType.TOM.Bunup = DtTOM.Rows[0]["BUNUP"].ToString();
            clsPmpaType.TOM.OCSJIN = DtTOM.Rows[0]["OCSJIN"].ToString();
            clsPmpaType.TOM.CardSeqNo = Convert.ToInt64(VB.Val(DtTOM.Rows[0]["CARDSEQNO"].ToString()));
            clsPmpaType.TOM.VCode = DtTOM.Rows[0]["VCODE"].ToString().Trim();
            clsPmpaType.TOM.MCode = DtTOM.Rows[0]["MCODE"].ToString().Trim();
            clsPmpaType.TOM.MSeqNo = DtTOM.Rows[0]["MSEQNO"].ToString().Trim();
            clsPmpaType.TOM.MQCODE = DtTOM.Rows[0]["MQCODE"].ToString().Trim();
            clsPmpaType.TOM.MksJin = DtTOM.Rows[0]["MKSJIN"].ToString().Trim();
            clsPmpaType.TOM.GbNight = DtTOM.Rows[0]["GBNIGHT"].ToString().Trim();
            clsPmpaType.TOM.GbFlu_Vac = DtTOM.Rows[0]["GBFLU_VAC"].ToString().Trim();
            clsPmpaType.TOM.OldMan = DtTOM.Rows[0]["OLDMAN"].ToString().Trim();
            clsPmpaType.TOM.GbExam = "";
            clsPmpaType.TOM.GbDementia = DtTOM.Rows[0]["GBDEMENTIA"].ToString().Trim();
            //clsPmpaType.TOM.Jtime2 = DtTOM.Rows[0]["JTIME2"].ToString().Trim();
            clsPmpaType.TOM.Gbilban2 = DtTOM.Rows[0]["GBILBAN2"].ToString().Trim();
            clsPmpaType.TOM.GbFlu_Ltd = DtTOM.Rows[0]["GBFLU_LTD"].ToString().Trim();
            clsPmpaType.TOM.JinDtl = DtTOM.Rows[0]["JINDTL"].ToString().Trim();
            clsPmpaType.TOM.INSULIN = DtTOM.Rows[0]["INSULIN"].ToString().Trim();
            clsPmpaType.TOM.OUTTIME = DtTOM.Rows[0]["OUTTIME"].ToString().Trim();
            clsPmpaType.TOM.DrSabun = Convert.ToInt64(VB.Val(DtTOM.Rows[0]["DRSABUN"].ToString().Trim()));
            clsPmpaType.TOM.J2_Sayu = DtTOM.Rows[0]["J2_SAYU"].ToString().Trim();
            clsPmpaType.TOM.Jiwon = DtTOM.Rows[0]["JIWON"].ToString().Trim();
            clsPmpaType.TOM.KTASLEVL = DtTOM.Rows[0]["KTASLEVL"].ToString().Trim();
            clsPmpaType.TOM.Emr = "";
            clsPmpaType.TOM.ResMemo = "";
            clsPmpaType.TOM.ROWID = DtTOM.Rows[0]["ROWID"].ToString();

            clsPmpaType.TOM.GbFlu = DtTOM.Rows[0]["GBFLU"].ToString().Trim();       //2021-09-16

            //2018.05.30 박병규 : 주석요청에 의한 처리
            //if (clsPmpaPb.GstrFrom == "접수")
            //{
            //    if (clsPmpaType.TOM.CardSeqNo != 0)
            //    {
            //        if (DtTOM.Rows[0]["CARDGB"].ToString().Trim() == "1")
            //        {
            //            clsPublic.GstrMsgTitle = "확인";
            //            clsPublic.GstrMsgList = "승인구분 : 카드승인    승인금액 : " + clsPmpaType.TOM.CardSeqNo + "원 승인" + '\r';
            //            clsPublic.GstrMsgList += "꼭 확인하시고 작업하시기 바랍니다." + '\r';
            //            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
            //        }
            //        else if ((DtTOM.Rows[0]["CARDGB"].ToString().Trim() == "2"))
            //        {
            //            clsPublic.GstrMsgTitle = "확인";
            //            clsPublic.GstrMsgList = "승인구분 : 현금영수증    승인금액 : " + clsPmpaType.TOM.CardSeqNo + "원 승인" + '\r';
            //            clsPublic.GstrMsgList += "꼭 확인하시고 작업하시기 바랍니다." + '\r';
            //            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
            //        }
            //    }
            //}

            //건강보험 유형 통합 (11,12,13 >> 11)
            cBON.BI = clsPmpaType.TOM.Bi;
            if (VB.Left(cBON.BI, 1) == "1") { cBON.BI = "11"; }
            //기준일자 세팅
            cBON.SDATE = clsPmpaType.TOM.BDate;
            strJuminNo = clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2;
            
            cBON.MCODE = clsPmpaType.TOM.MCode;
            cBON.VCODE = clsPmpaType.TOM.VCode;
            cBON.DEPT = clsPmpaType.TOM.DeptCode;
            //입원시 면제코드 구분
            cBON.OGPDBUN = "";
            //특정기호 구분(01 고위험, 02 임산부외래, 03 저체중조산아)
            cBON.FCODE = "";
            if (clsPmpaType.TOM.JinDtl == "22")
                cBON.FCODE = "03";
            else if (clsPmpaType.TOM.JinDtl == "25")
                cBON.FCODE = "02";

            //2018.06.20. 박병규 : 건강보험 DT 틀니, 임플란트
            if (VB.Left(cBON.BI, 1) == "1" && cBON.DEPT == "DT")
            {
                if (clsPmpaType.TOM.JinDtl != "02" || clsPmpaType.TOM.JinDtl != "07")
                    cBON.DEPT = "**";
            }

            cBON.IO = "I";
            //나이구분(0 성인, 1 신생아, 2 6세미만, 3 6세이상15세미만, 4 65세이상)
            cBON.CHILD = cPF.Acct_Age_Gubun(clsPmpaType.TOM.Age, strJuminNo, cBON.SDATE, cBON.IO);
            //***입원 본인부담율 세팅
            if (cIAcct.Read_IBon_Rate(clsDB.DbCon, cBON) == false)
                cA.Alert_BonRate(cBON);

            if (clsPmpaType.TOM.Bohun == "3")   //장애인은 본인부담율 재조정 
            {
                if ((cBON.BI == "11" && cBON.MCODE == "F000") || cBON.BI == "22")
                {
                    clsPmpaType.IBR.Jin = 0;
                    clsPmpaType.IBR.Bohum = 0;
                    clsPmpaType.IBR.CTMRI = 0;
                }
            }

            //2018.05.31 박병규 : 입원본인부담율 구하면서 cBON 변수값을 치환시키므로 외래본인부담율 구할때 다시 조건을 설정해준다
            //건강보험 유형 통합 (11,12,13 >> 11)
            cBON.BI = clsPmpaType.TOM.Bi;
            if (VB.Left(cBON.BI, 1) == "1") { cBON.BI = "11"; }
            //기준일자 세팅
            cBON.SDATE = clsPmpaType.TOM.BDate;
            strJuminNo = clsPmpaType.TBP.Jumin1 + clsPmpaType.TBP.Jumin2;
            //나이구분(0 성인, 1 신생아, 2 6세미만, 3 6세이상15세미만, 4 65세이상)
            cBON.MCODE = clsPmpaType.TOM.MCode;
            cBON.VCODE = clsPmpaType.TOM.VCode;
            cBON.DEPT = clsPmpaType.TOM.DeptCode;
            //입원시 면제코드 구분
            cBON.OGPDBUN = "";
            //특정기호 구분(01 고위험, 02 임산부외래, 03 저체중조산아)
            cBON.FCODE = "";
            if (clsPmpaType.TOM.JinDtl == "22")
                cBON.FCODE = "03";
            else if (clsPmpaType.TOM.JinDtl == "25")
                cBON.FCODE = "02";

            //2018.06.20. 박병규 : 건강보험 DT 틀니, 임플란트
            if ( cBON.DEPT == "DT")
            {
                if (clsPmpaType.TOM.JinDtl != "02" && clsPmpaType.TOM.JinDtl != "07")
                    cBON.DEPT = "**";
            }
            
            cBON.IO = "O";
            cBON.CHILD = cPF.Acct_Age_Gubun(clsPmpaType.TOM.Age, strJuminNo, cBON.SDATE, cBON.IO);

            cBON.JINDTL = clsPmpaType.TOM.JinDtl;

            //***외래 본인부담율 세팅
            if (cPO.Read_OBon_Rate(clsDB.DbCon, cBON) == false)
                cA.Alert_BonRate(cBON);

            if (clsPmpaType.TOM.Bohun == "3")   //장애인은 본인부담율 재조정 
            {
                if ((cBON.BI == "11" && cBON.MCODE == "F000") || cBON.BI == "22")
                {
                    clsPmpaType.OBR.Jin = 0;
                    clsPmpaType.OBR.Bohum = 0;
                    clsPmpaType.OBR.CTMRI = 0;
                }
            }


        }

        /// <summary>
        /// Bas_Patient Clear
        /// </summary>
        /// <param name="TBP"></param>
        public void Clear_Type_Bas_Patient()
        {
            clsPmpaType.TBP.Ptno = "";
            clsPmpaType.TBP.Sname = "";
            clsPmpaType.TBP.Sname2 = "";
            clsPmpaType.TBP.Sex = "";
            clsPmpaType.TBP.Jumin1 = "";
            clsPmpaType.TBP.Jumin2 = "";
            clsPmpaType.TBP.Jumin3 = "";
            clsPmpaType.TBP.StartDate = "";
            clsPmpaType.TBP.LastDate = "";
            clsPmpaType.TBP.ZipCode1 = "";
            clsPmpaType.TBP.ZipCode2 = "";
            clsPmpaType.TBP.Juso = "";
            clsPmpaType.TBP.Jiyuk = "";
            clsPmpaType.TBP.Tel = "";
            clsPmpaType.TBP.HPhone = "";
            clsPmpaType.TBP.HPhone2 = "";
            clsPmpaType.TBP.Sabun = "";
            clsPmpaType.TBP.EmbPrt = "";
            clsPmpaType.TBP.Bi = "";
            clsPmpaType.TBP.PName = "";
            clsPmpaType.TBP.PName2 = "";
            clsPmpaType.TBP.Gwange = "";
            clsPmpaType.TBP.Kiho = "";
            clsPmpaType.TBP.GKiho = "";
            clsPmpaType.TBP.DeptCode = "";
            clsPmpaType.TBP.DrCode = "";
            clsPmpaType.TBP.GbSpc = "";
            clsPmpaType.TBP.GbGameK = "";
            clsPmpaType.TBP.GbGameKC = "";
            clsPmpaType.TBP.JinIlsu = 0;
            clsPmpaType.TBP.JinAMT = 0;
            clsPmpaType.TBP.TuyakGwa = "";
            clsPmpaType.TBP.TuyakMonth = "";
            clsPmpaType.TBP.TuyakJulDate = 0;
            clsPmpaType.TBP.TuyakIlsu = 0;
            clsPmpaType.TBP.Bohun = "";
            clsPmpaType.TBP.Remark = "";
            clsPmpaType.TBP.GbMsg = "";
            clsPmpaType.TBP.Bunup = "";
            clsPmpaType.TBP.Birth = "";
            clsPmpaType.TBP.GbBirth = "";
            clsPmpaType.TBP.EMail = "";
            clsPmpaType.TBP.GbInfor = "";
            clsPmpaType.TBP.GbSMS = "";
            clsPmpaType.TBP.MiaBdate = "";
            clsPmpaType.TBP.MiaGubun = "";
            clsPmpaType.TBP.GBJuso = "";
            clsPmpaType.TBP.Tel_Confirm = "";
            clsPmpaType.TBP.Gbinfo_Detail = "";
            clsPmpaType.TBP.Road = "";
            clsPmpaType.TBP.GbForeigner = "";
            clsPmpaType.TBP.EName = "";
            clsPmpaType.TBP.GbEPassNo = "";
            clsPmpaType.TBP.GbEUniqNo = "";
            clsPmpaType.TBP.GB_VIP = "";
            clsPmpaType.TBP.GB_VIP_REMARK = "";
            clsPmpaType.TBP.OBST = "";
            clsPmpaType.TBP.ZipCode3 = "";
            clsPmpaType.TBP.BuildNo = "";
            clsPmpaType.TBP.RoadDetail = "";
            clsPmpaType.TBP.GbCountry = "";
            clsPmpaType.TBP.Religion = "";

        }

        public void Set_Type_Bas_Patient(DataTable DtTBP)
        {
            clsPmpaType.TBP.Ptno = DtTBP.Rows[0]["PANO"].ToString();
            clsPmpaType.TBP.Sname = DtTBP.Rows[0]["SNAME"].ToString();
            clsPmpaType.TBP.Sname2 = DtTBP.Rows[0]["SNAME2"].ToString();
            clsPmpaType.TBP.Sex = DtTBP.Rows[0]["SEX"].ToString();
            clsPmpaType.TBP.Jumin1 = DtTBP.Rows[0]["JUMIN1"].ToString();
            clsPmpaType.TBP.Jumin2 = clsAES.DeAES(DtTBP.Rows[0]["JUMIN3"].ToString());
            clsPmpaType.TBP.Jumin3 = DtTBP.Rows[0]["JUMIN3"].ToString().Trim();
            clsPmpaType.TBP.StartDate = DtTBP.Rows[0]["STARTDATE"].ToString();
            clsPmpaType.TBP.LastDate = DtTBP.Rows[0]["LASTDATE"].ToString();
            clsPmpaType.TBP.ZipCode1 = DtTBP.Rows[0]["ZIPCODE1"].ToString();
            clsPmpaType.TBP.ZipCode2 = DtTBP.Rows[0]["ZIPCODE2"].ToString();
            clsPmpaType.TBP.Juso = DtTBP.Rows[0]["JUSO"].ToString();
            clsPmpaType.TBP.Jiyuk = DtTBP.Rows[0]["JICODE"].ToString();
            clsPmpaType.TBP.Tel = DtTBP.Rows[0]["TEL"].ToString();
            clsPmpaType.TBP.HPhone = DtTBP.Rows[0]["HPHONE"].ToString();
            clsPmpaType.TBP.HPhone2 = DtTBP.Rows[0]["HPHONE2"].ToString();
            clsPmpaType.TBP.Sabun = DtTBP.Rows[0]["SABUN"].ToString();
            clsPmpaType.TBP.EmbPrt = DtTBP.Rows[0]["EmbPrt"].ToString();
            clsPmpaType.TBP.Bi = DtTBP.Rows[0]["BI"].ToString();
            clsPmpaType.TBP.PName = DtTBP.Rows[0]["PNAME"].ToString();
            clsPmpaType.TBP.PName2 = DtTBP.Rows[0]["PNAME2"].ToString();
            clsPmpaType.TBP.Gwange = DtTBP.Rows[0]["GWANGE"].ToString();
            clsPmpaType.TBP.Kiho = DtTBP.Rows[0]["KIHO"].ToString();
            clsPmpaType.TBP.GKiho = DtTBP.Rows[0]["GKIHO"].ToString();
            clsPmpaType.TBP.DeptCode = DtTBP.Rows[0]["DEPTCODE"].ToString();
            clsPmpaType.TBP.DrCode = DtTBP.Rows[0]["DRCODE"].ToString();
            clsPmpaType.TBP.GbSpc = DtTBP.Rows[0]["GBSPC"].ToString();
            clsPmpaType.TBP.GbGameK = DtTBP.Rows[0]["GBGAMEK"].ToString();
            clsPmpaType.TBP.GbGameKC = DtTBP.Rows[0]["GBGAMEKC"].ToString();
            clsPmpaType.TBP.JinIlsu = Convert.ToInt32(VB.Val(DtTBP.Rows[0]["JINILSU"].ToString()));
            clsPmpaType.TBP.JinAMT = Convert.ToInt64(VB.Val(DtTBP.Rows[0]["JINAMT"].ToString()));
            clsPmpaType.TBP.TuyakGwa = DtTBP.Rows[0]["TUYAKGWA"].ToString();
            clsPmpaType.TBP.TuyakMonth = DtTBP.Rows[0]["TUYAKMONTH"].ToString();
            clsPmpaType.TBP.TuyakJulDate = Convert.ToInt32(VB.Val(DtTBP.Rows[0]["TUYAKJULDATE"].ToString()));
            clsPmpaType.TBP.TuyakIlsu = Convert.ToInt32(VB.Val(DtTBP.Rows[0]["TUYAKILSU"].ToString()));
            clsPmpaType.TBP.Bohun = DtTBP.Rows[0]["Bohun"].ToString();
            clsPmpaType.TBP.Remark = DtTBP.Rows[0]["REMARK"].ToString();
            clsPmpaType.TBP.GbMsg = DtTBP.Rows[0]["GBMSG"].ToString();
            clsPmpaType.TBP.Bunup = DtTBP.Rows[0]["BUNUP"].ToString();
            clsPmpaType.TBP.Birth = DtTBP.Rows[0]["BIRTH"].ToString();
            clsPmpaType.TBP.GbBirth = DtTBP.Rows[0]["GBBIRTH"].ToString();
            clsPmpaType.TBP.EMail = DtTBP.Rows[0]["EMAIL"].ToString();
            clsPmpaType.TBP.GbInfor = DtTBP.Rows[0]["GBINFOR"].ToString() + ComNum.VBLF  + DtTBP.Rows[0]["GB_BLACK"].ToString();
            clsPmpaType.TBP.GbSMS = DtTBP.Rows[0]["GBSMS"].ToString();
            clsPmpaType.TBP.MiaBdate = "";
            clsPmpaType.TBP.MiaGubun = "";
            clsPmpaType.TBP.GBJuso = DtTBP.Rows[0]["GBJUSO"].ToString();
            clsPmpaType.TBP.Tel_Confirm = DtTBP.Rows[0]["TEL_CONFIRM"].ToString();
            clsPmpaType.TBP.Gbinfo_Detail = DtTBP.Rows[0]["GBINFO_DETAIL"].ToString();
            clsPmpaType.TBP.Road = DtTBP.Rows[0]["ROAD"].ToString();
            clsPmpaType.TBP.GbForeigner = DtTBP.Rows[0]["GBFOREIGNER"].ToString();
            clsPmpaType.TBP.EName = DtTBP.Rows[0]["ENAME"].ToString();
            clsPmpaType.TBP.GbEPassNo = DtTBP.Rows[0]["EPassNo"].ToString();
            clsPmpaType.TBP.GbEUniqNo = DtTBP.Rows[0]["EUniqNo"].ToString();
            clsPmpaType.TBP.GbEssn = DtTBP.Rows[0]["ESSN"].ToString();
            clsPmpaType.TBP.GB_VIP = DtTBP.Rows[0]["GB_VIP"].ToString();
            clsPmpaType.TBP.GB_VIP_REMARK = DtTBP.Rows[0]["GB_VIP_REMARK"].ToString();
            clsPmpaType.TBP.OBST = DtTBP.Rows[0]["OBST"].ToString();
            clsPmpaType.TBP.ZipCode3 = DtTBP.Rows[0]["ZIPCODE3"].ToString();
            clsPmpaType.TBP.BuildNo = DtTBP.Rows[0]["BUILDNO"].ToString();
            clsPmpaType.TBP.RoadDetail = DtTBP.Rows[0]["ROADDETAIL"].ToString();
            clsPmpaType.TBP.GbCountry = DtTBP.Rows[0]["GBCOUNTRY"].ToString();
            clsPmpaType.TBP.Religion = DtTBP.Rows[0]["RELIGION"].ToString();
        }

        public void Clear_Type_SG(string ArgSunext)
        {
            clsPmpaType.SG.SuCode = ArgSunext;
            clsPmpaType.SG.SuNext = ArgSunext;
            clsPmpaType.SG.SugbA = "1";
            clsPmpaType.SG.SugbB = "0";
            clsPmpaType.SG.SugbC = "0";
            clsPmpaType.SG.SugbD = "0";
            clsPmpaType.SG.SugbE = "0";
            clsPmpaType.SG.SugbF = "1";
            clsPmpaType.SG.SugbG = "1";
            clsPmpaType.SG.SugbH = "0";
            clsPmpaType.SG.SugbI = "0";
            clsPmpaType.SG.SugbJ = "0";
            clsPmpaType.SG.SugbK = "0";
            clsPmpaType.SG.SugbL = "0";
            clsPmpaType.SG.SugbO = "0";
            clsPmpaType.SG.SugbQ = "0";
            clsPmpaType.SG.SugbR = "0";
            clsPmpaType.SG.SugbW = "0";
            clsPmpaType.SG.SugbX = "0";
            clsPmpaType.SG.SugbY = "0";
            clsPmpaType.SG.SugbZ = "0";
            clsPmpaType.SG.DtlBun = "";
            clsPmpaType.SG.SugbS = "0"; 
            clsPmpaType.SG.Iamt = 0;
            clsPmpaType.SG.Tamt = 0;
            clsPmpaType.SG.Bamt = 0;
            clsPmpaType.SG.Selamt = 0; 
            clsPmpaType.SG.GbSpc_No = "0"; 
            clsPmpaType.SG.Sudate = "";
            clsPmpaType.SG.OldIamt = 0;
            clsPmpaType.SG.OldTamt = 0;
            clsPmpaType.SG.OldBamt = 0;
            clsPmpaType.SG.BaseAmt = 0;
            clsPmpaType.SG.TotMax = 0;
            clsPmpaType.SG.DelDate = "";
            clsPmpaType.SG.OrderNo = 0 ;
            clsPmpaType.SG.DrCode = "";
            clsPmpaType.SG.Multi = "";
            clsPmpaType.SG.MultiRemark = "";
            clsPmpaType.SG.ScodeSayu = "";
            clsPmpaType.SG.ScodeRemark = "";
            clsPmpaType.SG.Dur = "";
            clsPmpaType.SG.Div = 0;

        }

        /// <summary>
        /// 재원심사 환자조회 용
        /// </summary>
        public class JPatLst
        {
            public string JobSabun;
            public string Sort;
            public string Dept;
            public string Ward;
            public string Pano;
            public long IpdNo;
            public long TrsNo;
            public bool DRG;
            public bool GbOP;
            public bool AnOP;
            public string Jeawon;
            public bool Tewon;
            public bool MySet;
            public string[] SetBi;
            public string[] SetWard;
            public string[] SetRoom;
            public string[] SetDept;
            public string[] SetOK;
            public string OpFDate;
            public string OpTDate;
            public bool SetSuga;
        }

        /// <summary>
        /// 본인부담율 세팅용 Argument Class
        /// </summary>
        public class BonRate
        {
            public string BI;
            public string IO;
            public string CHILD;
            public string MCODE;
            public string VCODE;
            public string OGPDBUN;
            public string FCODE;
            public string SDATE;
            public string DEPT;
            public string JIN;
            public string JINDTL;
        }

        /// <summary>
        /// 진료비 상세내역 본인부담율 세팅용 변수
        /// </summary>
        public class ISBR
        {
            public string BI;
            public string INDATE;
            public string SUNEXT;
            public string SUGBP;
            public string GBSUGBS;
            public string GBSELF;
            public string GBDRG;
            public string VCODE;
            public string OGPDBUN;
            public string FCODE;
            public string MCODE;
            public string BUN;
            public string BDATE;
            public string BOHUN;
            public string DTLBUN;
            public string DRGF;
            public string DRG100;
            public double QTY;
            public int NAL;
            public int NU;
            public long AMT;

            public long[] nBBAmt = new long[3];   //본인부담
            public long[] nBGAmt = new long[3];   //공단부담
            public long[] nBJAmt = new long[3];   //전액부담
            public long[] nBFAmt = new long[3];   //비급여
        }
    }

    public class GBNGT
    {
        //주간, 야간, 응급
        public string Night { get; set; }

        //성인,소아,노인,신생아
        public string GbChild { get; set; }
      
        //수술,부수술, 제2수술, 마취
        public string Hang { get; set; }

        //GBNGT2 구분자
        public string GbNgt2 { get; set; }


    }
}                         
                          