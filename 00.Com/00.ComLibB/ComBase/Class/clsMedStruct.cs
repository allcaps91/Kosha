namespace ComBase
{
    /// <summary>
    /// 작성자 : 이상훈
    /// 처방 enum  및 Struct 
    /// </summary>
    public class BaseOrderInfo
    {
        #region 외래
        public enum OpdOrderCol
        {
            DC = 0,
            ORDERCODE,
            NAMEENG,
            PLUSNAME,
            NEWCONTENTS, //2020-12-29 일용량 추가 
            CONTENTS,
            DIV,
            NAL,
            GBER,
            GBSELF,
            REMARKGUBUN,//10
            GBRESERVED, 
            GBMULTI,
            TUYEOPOINT, //2021-01-09 투여시점
            NGT,        //2020-12-29 GBGROUP 사용
            TUYEOTIME,  //2021-01-09 투여시간
            TIMEGUBUN,  //2021-01-09 시간구분(hr/m)
            SUCODE,
            BUN,
            SLIPNO,
            QTY,        //20
            DOSCODE,
            GBBOTH,
            GBINFO,         
            REMARK,     
            DISPRGB,    
            GBBOTH1,
            GBINFO1,
            GBQTY,
            GBDOSAGE,
            NEXTCODE,   //30
            GBIMIV,
            ORDERNO,
            SUGBF,         
            RESULTCHK,  
            GBSUNAP,    
            SIMSAGIJUN,
            DRCODE, 
            MULTIREMARK,
            DUR,
            DURREASON,  //40
            RESV,
            GBTAX,
            SCODEREMARK,    
            GBSPCNO,    
            SCODEREASON,    
            GBCOPY,
            GBBUBUNSUNAP,
            DOSCODE1,
            REALQTY,
            GBDIV,      //50
            GBNAL,
            GBSELF1,
            GBAUTOSEND2,    
            GBFM,       
            SABUN,  
            OCSDRUG,
            GBAUTOSEND,
            ASA,
            PCHASU,
            SUBUL_WARD, //60
            AUTO_SUNAP,
            SUPSTATUS,
            CBUN,
            BURNADD,
            OPGUBUN,
            OPDNO,
            POWDER,
            SEDATION,
            BCONTENTS
        }
        #endregion

        #region 입원

        public enum IpdOrderCol
        {
            DC = 0,
            ORDERGUBUN,
            NAMEENG,
            PLUSNAME,
            CONTENTS,       
            REALQTY,
            DIV,
            NAL,
            POWDER,
            TUYEOPOINT, 
            NGT, // 10
            TUYEOTIME,
            TIMEGUBUN,
            ER, 
            SELF,
            PRN,
            POTABLE,
            SUCODE,
            BUN,
            SLIPNO, 
            QTY,//20
            DOSAGE,
            GBBOTH,
            GBINFO,
            REMARK,
            DISPRGB,
            CGBBOTH,
            CGBINFO,
            GBQTY,
            GBDOSAGE,
            NEXTCODE,//30
            GBIMIV,
            ORDERNO,
            ROWID,  
            GBACT,
            SEQNO,
            GBSTATUS,
            ORDERCODE,
            SLIPGUBUN,
            CDIV, 
            VERBALID,//40
            BCONTENTS,
            GBSEND,
            DRCODE, 
            DEPTCODE,
            ENTDATE,
            ADD,
            RESULT,
            SUNSUNAP,
            SIMFLAG,
            STAFFID,//50
            ORDERSITE,
            MULTI,
            MULTIREASON, 
            DURGUBUN,
            TRANSREMARK,
            MAYAK,
            CONSULT,
            MAYAKREMARK,
            GBIOE, 
            CGBSPCNO,//60
            NSTSMS,
            GBTAX,
            SUGBF,  
            VERBALGUBUN,
            VORDERNO,
            GBVERB,
            PRNMARK,
            PRNREMARK,
            INSULINSCALE, 
            INSULINUNIT,//70
            INSULINSDATE,
            INSULINEDATE,
            INSULINMAX, 
            POWDERSAYUMARK,
            POWDERSAYU,
            ASA,
            INSULINCOFMTIME,
            PRNDOSCODE,
            PRNTERM,
            PRNNOTITIME,//80
            PRNROWID,
            AIRSHT,
            PCHASU, 
            SUBUL_WARD,
            HIGHRISKGBN,
            DOSCODE,
            REALQTY2,
            GBDIV,
            NAL2,
            ONEDAYINORD,//90
            GSADD,            
            SUPSTATUS,
            CBUN,   
            BURNADD,
            OPGUBUN,
            GBPICKUP,
            SEDATION,
            PRNORDSEQ
        }
        #endregion

        #region 응급실

        public enum ErOrderCol
        {
            DC = 0,
            ORDERGUBUN,
            NAMEENG,
            PLUSNAME,
            CONTENTS,
            REALQTY,
            DIV,
            NAL,
            NGT,
            GBER,
            SELF,       //10
            GBREMARK,
            POTABLE,
            SUCODE,
            BUN,
            SLIPNO,
            QTY,
            DOSCODE,
            GBBOTH,
            GBINFO,
            REMARK,     //20
            DISPRGB,
            GBBOTH1,
            GBINFO1,
            GBQTY,
            GBDOSAGE,
            NEXTCODE,
            GBIMIV,
            ORDERNO,
            ROWID,
            GBACT,      //30
            SEQNO,
            GBSTATUS,
            ORDERCODE,
            SORT,
            DIV1,
            GBVERB,
            BCONTENTS,
            GBSEND,
            DRCODE,
            DEPTCODE,   //40
            ENTDATE,
            GBADD,
            GBOPINION,
            SUNSUNAP,
            MULTI,
            MULTIREASON,
            DUR,
            OLDDEL,
            OLDDOSCODE,
            OLDCONTENTS,    //50
            OLDDIV,
            OLDNAL,
            OLDSELF,
            OLDN,
            GBTAX,
            POWDER,
            ORDDATE,
            TEMP1,
            TEMP2,
            TEMP3,          //60
            GBVERB1,
            VORDERNO,
            GBVERB2,
            PRNMARK,
            PRNREMARK,
            INSULINSCALE,
            INSULINUNIT,
            INSULINSDATE,
            INSULINEDATE,
            INSULINMAX,
            POWDERNOMARK,
            POWDERNOREASON,
            ASA,
            INSULINCOFMTIME,
            PRNDOSCODE,
            PRNTERM,
            PRNNOTITIME,
            PRNROWID,
            ERMED,
            REALORDERDATE,
            ORDDR,            
            ONEDAYINORD,
            GSADD,
            SUPSTATUS,
            CBUN,
            BURNADD,
            OPGUBUN,
            SUGBF,
            OPDNO,
            SEDATION,
            PRNORDSEQ
        }
        #endregion

        public enum OprOrderCol
        {
            DC = 0,
            ORDERGUBUN,
            ORDERCODE,
            NAMEENG,
            PLUSNAME,
            SENDDEPTORDER,
            SENDDEPT,
            CONTENTS,
            UNIT,
            REALQTY,
            DIV,
            NAL,
            GBER,
            GBNGT,
            GBTREATMENT,
            GBSELF,
            REMARKGUBUN,
            GBPRN,
            GROUPINGNO,
            GBPORTABLE,
            GBPOWDER,
            STATUS,
            GBREPEAT,
            GBREPEATILSU,
            GBRPTREMAINILSU,
            INSERTUSERID,
            PICKUPUSERID,
            CDEPTCODE,
            CDRCODE,
            INSERTUSERBUSE,
            GBORDER,
            GUBUN,
            SCLASS,
            SLIPNO,
            SETAMT,
            QTY,
            PLUSCODE,
            REMARK,
            GBQTY,
            GBNAL,
            BASECODE,
            GBADDINFO,
            GBSPECINFO,
            PICKUPDATE,
            ACTFLAG,
            ACTID,
            ACTNAL,
            GBSTATUS,
            ORDERNO,
            ITEMCODE,
            SENDDEPTPRINT,
            BOSELF,
            GBJUPSU,
            GBSUNAP,
            SEQNO,
            GBAMT,
            GBTFLAG,
            GBSUM,
            GBPX,
            BCONTENTS,
            ROWID,
            INFORMATION,
            DURCODE,
            DURREASON,
            GBSUMWARD,
            GBOBSONO,
            PACSSENDER,
            GBINOUT,
            SUPSTATUS,
            ONEDAYINORD,
            GSADD
        }

        #region 외래 환자리스트
        public enum OpdLstCol
        {
            PANO = 0,
            SNAME,
            CHOJIN,
            EXMRSLT,
            SEXAGE,
            JTIME,
            JDEPTTIME,
            MEMO,
            HD,
            BI,
            RSVGUBUN,
            EMR,
            JINRPT,
            EMPTY,
            KIND,
            DEPTCODE,
            AGE,
            CHOJAE,
            JINRPT2,
            BUNUP,
            BDATE,
            DRCODE,
            PNEUMONIA,
            PREGNANT,
            COMPANYVACCIN,
            FLURSV,
            FLUVACCIN,
            THYROID,
            NGT,
            SPC,
            RSVEXAM,
            INSULIN,
            MCODE,
            VCODE,
            HUBUL,
            VIP,
            ISOLATION,
            INFECT1,
            INFECT2,
            RESULT,
            CHART,
            INFECTION,
            FLUEYN,
            ER24,
            OPDNO
        }
        #endregion

        #region 입원 환자리스트2

        public enum IpdRsvOrder
        {
            DC = 0,
            ORDERGUBUN,
            NAMEENG,
            PLUSNAME,
            CONTENTS,
            REALQTY,
            DIV,
            NAL,
            POWDER,
            NGT,
            GBER,
            SELF,
            GBREMARK,
            POTABLE,
            SUCODE,
            BUN,
            SLIPNO,
            QTY,
            DOSAGE,
            GBBOTH,
            GBINFO,
            REMARK,
            DISPRGB,
            GBBOTH1,
            GBINFO1,
            GBQTY,
            GBDOSAGE,
            NEXTCODE,
            GBIMIV,
            ORDERNO,
            ROWID,
            GBACT,
            SEQNO,
            GBSTATUS,
            ORDERCODE,
            SORT,
            DIV1,
            GBVERB,
            BCONTENTS,
            GBSEND,
            DRCODE,
            DEPTCODE,
            ENTDATE,
            GBADD,
            GBOPINION,  //소견여부
            TEMP,       //AT
            SIMSASTD,   //심사기준
            STAFFID,
            ORDERSITE,
            MULTI,
            MULTIREASON,
            DUR,
            TRANSREMARK,
            NARCOTICS,
            GBCONSULT,  //컨설트 여부
            NARCOTICSYMPTOM,    //마약주요증상
            GBIOE,
            EXCLUDESPC,
            CONSULTETC,
            GBTAX,
            SELF1,
            DRVERBALCOFM,
            V_ORDERNO,
            GBVERB1,
            PRNINFO,
            PRNINFODTL,
            INSULINSCALE,
            INSULINUNIT,
            INSULINSDATE,
            INSULINEDATE,
            INSULINMAX,
            POWDERNOMARK,
            POWDERNOREASON,
            TEMP73,
            TEMP74,
            TEMP75,
            TEMP76,
            TEMP77,
            TEMP78,
            TEMP79,
            TEMP80,
            TEMP81,
            TEMP82,
            TEMP83,
            TEMP84,
            TEMP85,
            TEMP86,
            TEMP87,
            TEMP88,
            TEMP89,
            TEMP90,
            TEMP91,
            TEMP92,
            TEMP93,
            TEMP94,
            TEMP95,
            TEMP96,
            TEMP97,
            TEMP98,
            TEMP99,
            MON,
            TUE,
            WED,
            THU,
            FRI,
            SAT,
            SUN,
            CBUN
        }
        #endregion

        #region 응급실 환자리스트

        public enum ErLstCol
        {
            AREA = 0,
            PANO,
            SNAME,
            SEX,
            AGE1,
            KTAS,
            JTIME,
            ERDEPT,
            EMT,
            NURINFO,
            SPC,
            BI,
            DEPTCODE,
            AGE,
            CHOJAE,
            BUNUP,
            DRCODE,
            PNEUMONIA,
            ERPATIENT,
            GBFLU,
            CCDX,
            PREGNANT,
            MCODE,
            VCODE,
            OUTTIME,
            GAMEK,
            VIP,
            ROWID,
            BDATE,
            INFECTION,
            CERT,
            NOACT,
            ADDORDER,
            JINDR,
            MEMO,
            RESULT,
            ERDR,
            EMR,
            INFECTIONCHK,
            INFLUYN,
            ERIPDRSV,            
            OPDNO,
            SERIOUSILLS,
            DRSABUN,
            IPDNO,
            ZIKA,
            /// <summary>
            /// 블랙리스트 = ◇ ELSE = NULL
            /// </summary>
            BLACKLIST
        }

        #endregion

        #region 입원 환자리스트
        public enum IpdLstCol
        {
            ROOMNO = 0,
            ROOMNO1,
            PANO,
            SNAME,
            SEX,
            AGE,
            ADMDATE,
            ORDER,
            OP,
            EMR,
            CP,
            //CPSCHEDULE,
            REPEAT,
            AMSET1,
            DEPTCODE,
            BI,
            INDATE1,
            DRCODE,
            IPDSTS,
            WARDCODE,
            ROOMCODE,
            DRNAME,
            GBGAMEK,
            GBSTS,
            IPDNO,
            TRSNO,
            OUTDATE,
            ROWID,
            CPCODE,
            CPDAY,
            CPNAME,
            CPREPEAT,
            PNEUMONIA,
            ICUINSTD,
            PREGNANCY,
            EXAMSMS,
            IOE,
            IPDPATH,
            OUTSIGN,
            SPC,
            PTREQ,
            DRG,
            INFECTION,
            LI,
            JU,
            RESULT,
            AT,
            ADMTIME,
            IPWONTIME,
            CHART,
            NOEXE,
            OP_JIPYO,
            DRSABUN,
            VS,
            BICODE,
            KTASLEVL
        }
        #endregion


        public enum OprLstCol
        {
            DEPTCODE = 0,
            PANO,
            SNAME,
            IPDOPD,
            AGESEX,
            DRNM,
            ROWID,
            WRTNO,
            ANSPCNAME,
            DRG,
            HPHONE,
            EMRCOLOR
        }

        public enum DeptJinLst
        {
            EMR = 0,
            SEQNO,
            SNAME,
            RTIME,
            JTIME,
            PANO,
            GUBUN,
            SEX,
            AGE,
            CHOJAE,
            SHEET,
            BI,
            SPC,
            INFECTYN,
            GUBUN_1,
            SEQ_RTime,
            ROWID
        }

        public enum DeptWaitLst
        {
            PANO = 0,
            SNAME,
            DEPTCODE,
            DRNAME,
            GUBUN,
            RTIME,
            CHOJAE,
            JEPDATE,
            DRCODE,
            ROWID
        }

        public struct Select_Ptno
        {
            public long IPDNO;
            public string PtNo;
            public string sName;
            public int Age;
            public string Sex;
            public string GbSpc;
            public string DeptCode;
            public string DrCode;
            public string Staffid;
            public string Bi;
            public string JTime;
            public string GbChojae;
            public string RDATE;
            public string RTime;
            public string RDrCode;
            public string Remark1;  // 골수검사 임상소견
            public string Remark2;  // 골수검사 주요병력
            public string Remark3;  // 내시경
            public string Remark4;  // Biopsy Or Cytology
            public string Tel;
            public string GbSheet;
            public string VCode;    // Y.암등록환자/N.암미등록환자
            public string Exam;     // Y.검사결과확인예약
            public string WardCode;
            public string RoomCode;
            public string DrName;
            public string PNEUMONIA;// 페렴
            public string Pregnant; // 임신여부
            public string Lact;     // 수유여부
            public string MCODE;    //희귀난치성등록자구분
            public string Thyroid;  //갑상선여부
            public string res;      //내시경 예약
            public string Insulin;  //인슐린투여환자
            public string MCODE_OPD;//차상위 희귀
            public string VCODE_OPD;
            public string JUMIN;    //주민번호
            public string bunup;    //분업 2012-05-29
            public string Birth;    //생일 2013-02-06
            public string INDATE;   //2013-08-01
            public string ResMemo;  //2014-08-07
            public string ResSMSNot;
        }
        public static Select_Ptno PAT;

        public struct Patient_Info
        {
            public string PANO;
            public string SNAME;
            public string SEX;
            public string JUMIN1;
            public string JUMIN2;
            public string STARTDATE;
            public string LASTDATE;
            public string ZIPCODE1;
            public string ZIPCODE2;
            public string JUSO;
            public string JICODE;
            public string TEL;
            public string SABUN;
            public string EMBPRT;
            public string BI;
            public string PNAME;
            public string GWANGE;
            public string KIHO;
            public string GKIHO;
            public string DEPTCODE;
            public string DRCODE;
            public string GBSPC;
            public string GBGAMEK;
            public string JINILSU;
            public string JINAMT;
            public string TUYAKGWA;
            public string TUYAKMONTH;
            public string TUYAKJULDATE;
            public string TUYAKILSU;
            public string BOHUN;
            public string REMARK;
            public string RELIGION;
            public string GBMSG;
            public string XRAYBARCODE;
            public string ARSCHK;
            public string BUNUP;
            public string BIRTH;
            public string GBBIRTH;
            public string EMAIL;
            public string GBINFOR;
            public string JIKUP;
            public string HPHONE;
            public string GBJUGER;
            public string GBSMS;
            public string GBJUSO;
            public string BICHK;
            public string HPHONE2;
            public string JUSAMSG;
            public string EKGMSG;
            public string BIDATE;
            public string MISSINGCALL;
            public string AIFLU;
            public string TEL_CONFIRM;
            public string GBSMS_DRUG;
            public string GBINFO_DETAIL;
            public string GBINFOR2;
            public string ROAD;
            public string ROADDONG;
            public string JUMIN3;
            public string GBFOREIGNER;
            public string ENAME;
            public string CASHYN;
            public string GB_VIP;
            public string GB_VIP_REMARK;
            public string GB_VIP_SABUN;
            public string GB_VIP_DATE;
            public string ROADDETAIL;
            public string GB_VIP2;
            public string GB_VIP2_REAMRK;
            public string GB_SVIP;
            public string WEBSEND;
            public string WEBSENDDATE;
            public string GBMERS;
            public string OBST;
            public string ZIPCODE3;
            public string BUILDNO;
            public string PT_REMARK;
            public string TEMPLE;
            public string C_NAME;
            public string GBCOUNTRY;
            public string Rroad_Addr;
        }
        public static Patient_Info PMST;
        
        public struct OPD_MASTER
        {
            public string ACTDATE;
            public string PANO;
            public string DEPTCODE;
            public string BI;
            public string SNAME;
            public string SEX;
            public string AGE;
            public string JICODE;
            public string DRCODE;
            public string RESERVED;
            public string CHOJAE;
            public string GBGAMEK;
            public string GBSPC;
            public string JIN;
            public string SINGU;
            public string BOHUN;
            public string CHANGE;
            public string SHEET;
            public string REP;
            public string PART;
            public string JTIME;
            public string STIME;
            public string FEE1;
            public string FEE2;
            public string FEE3;
            public string FEE31;
            public string FEE5;
            public string FEE51;
            public string FEE7;
            public string AMT1;
            public string AMT2;
            public string AMT3;
            public string AMT4;
            public string AMT5;
            public string AMT6;
            public string AMT7;
            public string GELCODE;
            public string OCSJIN;
            public string BDATE;
            public string BUNUP;
            public string BONRATE;
            public string TEAGBE;
            public string CHUSABUN;
            public string CARDSEQNO;
            public string EMR;
            public string VCODE;
            public string CARDGB;
            public string ARRIVETIME;
            public string JINTIME;
            public string EXAM;
            public string BCODE;
            public string MCODE;
            public string MSEQNO;
            public string BOHO_WRTNO;
            public string MQCODE;
            public string PNEUMONIA;
            public string AMT8;
            public string PREGNANT;
            public string ERPATIENT;
            public string DANAMT;
            public string LASTDANAMT;
            public string TEXTEMR;
            public string GBUSE;
            public string GBFLU;
            public string MKSJIN;
            public string GBNIGHT;
            public string GBOTHERHD;
            public string GBFLU_VAC;
            public string OLDMAN;
            public string GBDEMENTIA;
            public string THYROID;
            public string GBILBAN2;
            public string GBFLU_LTD;
            public string TUBERCULOSIS;
            public string GBOT;
            public string JINDTL;
            public string INSULIN;
            public string OUTTIME;
            public string OSTIME;
            public string OSTIME2;
            public string OCTIME;
            public string OCTIME2;
            public string ODRCODE;
            public string WTIME;
            public string ARRIVETIME0;
            public string WCNT;
            public string WCNT2;
            public string DRSABUN;
            public string GWACHOJAE;
            public string THYROID2;
            public string J2_SAYU;
            public string ER_NUM;
            public string JIWON;
            public string CHUL;
            public string KTASLEVL;
            public string JEPSUSAYU;
            public string TOI_GUBUN;
        }
        public static OPD_MASTER OMST;

        public struct Table_Ipd_Master
        {
            public long IPDNO;
            public string Pano;
            public string sName;
            public string Sex;
            public int Age;
            public int AgeDays;// 나이 0세 일수
            public string Bi;
            public string InDate;
            public string InTime;
            public string OutDate;
            public string ACTDATE;
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
            public string IllCode1;// '상병코드 1     (6)
            public string IllCode2;// 상병코드 2     (6)
            public string IllCode3;// 상병코드 3     (6)
            public string IllCode4;// 상병코드 4     (6)
            public string IllCode5;// 상병코드 5     (6)
            public string IllCode6;// 상병코드 6     (6)
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
            public string AmSetB;
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
            public long JungganJanAmt; //보증금+중간납의 미대체 잔액
            public int MstCNT; //마스타의 건수
            public string GBSuDay;
            public string PNEUMONIA;//폐렴
            public string Pregnant; //임신여부
            public string GbGoOut; //외출,외박불가 - 해당사항 Y
            public string OgPdBundtl; //차상위2종의 자연분만,6세만 구분
            public string WardInTime; //병동입실시간 - 2009-11-18
            public string TelRemark; //입원대기 긴급연락처 2009-12-28
            public string GbExam; //입원중정밀검사 Y 2010-01-21
            public string Secret; //정보보호구분 2012-11-22
            public string GbDRG; //DRG 여부   2013-06-29
            public string DrgCode; //DRG 코드   2013-06-29
            public string KTASLEVL;
            public string FROOM;
            public string FROOMETC;
            public string GBJIWON;
            public string T_CARE;
        }
        public static Table_Ipd_Master IMST;

        public struct Table_OrAn_Master
        {
            public string OPDATE;
            public string PANO;
            public string SNAME;
            public string BI;
            public string DEPTCODE;
            public string DRCODE;
            public string IPDOPD;
            public string WARDCODE;
            public string ROOMCODE;
            public long AGE;
            public string SEX;
            public string OPTIMEFROM;
            public string OPTIMETO;
            public string FLAG;
            public string GBNIGHT;
            public string OPROOM;
            public string DIAGNOSIS;
            public string OPTITLE;
            public string OPDOCT1;
            public string OPDOCT2;
            public string OPNURSE;
            public long OPCODE;
            public string OPPOSITION;
            public string ANGBN;
            public string ANDOCT1;
            public long ANTIME;
            public string ANNURSE;
            public string OPGUBUN;
            public long WRTNO;
            public string OPBUN;
            public string OPRE;
            public string OPCANCEL;
            public string OPSTIME;
            public string CNURSE;
            public string GBSLIP1;
            public string GBSLIP2;
            public string OPREMARK;
            public string SPADEWORK;
            public string PTREMARK;
            public string GBANAMT;
            public string GBER;
            public string OPSTAFF;
            public string KIDNEY;
            public string SMSSEND;
            public string JDATE;
            public string CYSTORESULT;
            public string GBDELAY;
            public string SAYU01;
            public string SAYU02;
            public string SAYU03;
            public string SAYU04;
            public string SAYU05;
            public string SAYU06;
            public string SAYU07;
            public string SAYU08;
            public string SAYU09;
            public string SAYU10;
            public string SAYU11;
            public string SAYU12;
            public string SAYU13;
            public string SAYU14;
            public string SAYU15;
            public string SAYUREMARK1;
            public string SAYUREMARK2;
            public string OPERR;
            public string DR_STIME;
            public string DR_ETIME;
            public string ASSIST_SABUN;
            public string ANDOCT_SABUN;
            public string OPETIME;
            public string GBMIR;
            public string GBANGIO;
            public string OPHAPSAYU;
            public string GBDAY;
            public string SWARDTIME;
            public string EORTIME;
            public string GBKESU;
            public string GBEXAM;
            public string GBEXAM1;
            public string GBEXAM2;
            public string GBEXAM3;
            public string GBEXAM4;
            public string GBEXAM5;
            public string GBEXAM6;
            public string GBHOI;
            public string GBSPC;
            public string PCA;
            public string PCAPRT;
            public string GBSMS;
            public string DOC_SMS;
        }
        public static Table_OrAn_Master ORMST;

        public struct Suag_Info
        {
            public string BUN;
            public string CBUN;
            public string BOSELF;
            public string BHSELF;
            public string TASELF;
            public string SNSELF;
            public string ILSELF;
            public string GISELF;
            public string GBSPC;
            public double BOPRICE;
            public string GBPX;
            public string GBHOSP;
        }
        public static Suag_Info SI;

        public enum SlipSub_Info
        {
            CHECK = 0,
            ORDERNAME,
            ORDERCODE,
            GBINPUT,
            GBINFO,
            CASH,
            BUN,
            NEXTCODE,
            SUCODE,
            INFO,
            IGBSPEC,
            DOSCODE,
            SLIPNO,
            SPECNAME,
            GBIMIV,
            INPUTSEQ,
            SUBRATE
        }
        public static SlipSub_Info ssI;
    }
}
