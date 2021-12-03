using ComBase; //기본 클래스
using ComDbB; //DB연결
using ComSupLibB.Com;
using FarPoint.Win.Spread;
using System;
using System.Collections;
using System.Data;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name              : ComSupLibB.SupLbEx
    /// File Name               : clsComSupLbExRcpSQL.cs
    /// Title or Description    : 진단검사의학과 Biz
    /// Author                  : 김홍록
    /// Create Date             : 2018-02-03
    /// Update History          : 
    /// </summary>
    public class clsComSupLbExRcpSQL : Com.clsMethod
    {
        string SQL = string.Empty;

        // 관찰구역
        public static string GstrArea1 = "'71','72','73','74','61','62','63','64','65','66','67'," +
                                         "'68','69','70','43','44','45','46','91','92','99','98','03'"; //2019-02-15 관찰구역 좌석 추가
        // 중증구역
        public static string GstrArea2 = "'81','82','83','84','85','86','75','76','77','78'";
        // 소아구역
        public static string GstrArea3 = "'52','53','54','55','56','57','A0','A1','A2','A3','A4','A5','A6','A7','A8'";
        // 격리구역
        public static string GstrArea4 = "'47','48','49','58','59'";

        public enum     enmSel_EXAM_MASTER_MENU     {       CHK, MASTERCODE,   SPECCODE,  SPEC_NM,   TUBE_NM,  EXAM_NAME,   TUBECODE };
        public string[] sSel_EXAM_MASTER_MENU =     {    "삭제", "검사코드", "검체코드", "검체명",  "용기명",   "검사명", "용기코드" };
        public int[]    nSel_EXAM_MASTER_MENU =     { nCol_SCHK, nCol_CHEK, nCol_CHEK, nCol_PANO, nCol_CHEK,  nCol_NAME,   nCol_AGE };

        public enum     enmSel_EXAM_SPECMST_RCP02   {   IPDOPD, RECEIVEDATE,   INFECT_INFO, INFECT_IMAGE,            ERP,          WARD,      ROOM,     SPECNO,       PANO,      SNAME,       SEX,        AGE, JEPSUNAME,    JEPSUNAME2,  EXAM_NAME,  IOCS_CANCEL,    STATUS,  BDATE,  DEPTCODE,   ERCP };
        public string[] sSel_EXAM_SPECMST_RCP02   = { "IPDOPD",  "접수일시", "INFECT_INFO",   "감염정보",         "중증",        "병동",    "병실", "검체번호", "등록번호", "환자성명",    "성별",     "나이",  "접수인",     "수령인" , "검사명칭","IOCS_CANCEL", " STATUS","BDATE", "DEPTCODE", "ERCP" };
        public int[]    nSel_EXAM_SPECMST_RCP02   = {        5,nCol_TIME-20,             5,     nCol_AGE, nCol_CHEK - 30, nCol_CHEK -30, nCol_SCHK,  nCol_PANO,  nCol_PANO,  nCol_PANO, nCol_SCHK,     nCol_CHEK, nCol_PANO,  nCol_PANO,  nCol_JUSO,            5,         5,      5,         5,   5 };

        public enum             enmSel_EXAM_ERR_MSG {    ACTDATE,     SPECNO,       MSG,    ROWID};
        public string[]         sSel_EXAM_ERR_MSG = { "오류일자", "검체번호",  "메시지", "ROWID" };
        public int[]            nSel_EXAM_ERR_MSG = {  nCol_PANO,  nCol_PANO, nCol_JUSO,       5 };

        public enum enmSel_EXAM_SPECMST_WORKLIST      {       WORKSTS, BARCODE_NM,     SPECNO,        PANO,      EXAM_NUM,      SNAME,       SEX,          AGE,         WARD,         DEPTCODE, SPECCODE_NM,  EXAM_NAME     , EXAM_RESULT  ,   REMARK,   REMARK2,   IPDOPD,   BDATE };
        public string[] sSel_EXAM_SPECMST_WORKLIST  = {         "W/S",   "바코드", "검체번호",  "등록번호",    "검사순번", "환자성명",    "성별",       "나이",       "병동",             "과",      "검체", "검사항목"     ,    "결과값"  ,  "비고1",   "비고2", "IPDOPD", "BDATE" };
        public int[] nSel_EXAM_SPECMST_WORKLIST     = {  nCol_SCHK+10,  nCol_PANO, nCol_PANO, nCol_PANO -5, nCol_AGE - 10,  nCol_CHEK, nCol_SCHK, nCol_SCHK + 10, nCol_AGE - 10, nCol_SCHK + 5,    nCol_AGE,  nCol_NAME     , nCol_CHEK    , nCol_DPCD, nCol_DPCD,        5,       5 };

        public enum enmSel_EXAM_ORDER_STATUS          { READY, RECIPT };

        public enum enmSel_EXAM_ORDER_PT_STATUS       { OPT, IPT, CONSULT, OG , DANG};

        public enum enmSel_EXAM_ORDER     {       INFECT_INFO,     INFECT_IMAGE,       PANO,        SEX,        AGE , GBNDANG  ,     SNAME, JUMIN1, DEPTCODE,     ROOM,      BDATE,      RDATE,  RDATE_CHK,       ERP,    OLD_MAN,   NOT_ACT,       WON,        EM,       SECRET,      FALL,  URINECNT,    IPDOPD ,  REMIND };
        public string[] sSel_EXAM_ORDER = {     "INFECT_INFO",       "감염정보", "등록번호",     "성별",      "나이", "당뇨"   ,"환자성명", "생년월일", "과",   "구역", "처방일자", "예약일자",     "예약",    "응급　중증",     "노인",    "시행",    "거리",    "응급",     "사생활",    "낙상",    "소변",  "IPDOPD" , "REMIND"};
        public int[] nSel_EXAM_ORDER    = {                 5,     nCol_PANO-15,  nCol_PANO,  nCol_SCHK, nCol_SCHK+5, nCol_SCHK, nCol_PANO, nCol_JUMIN1, nCol_SCHK +5, nCol_AGE,  nCol_DATE,  nCol_DATE,  nCol_SCHK, nCol_SCHK + 35,  nCol_SCHK, nCol_SCHK, nCol_SCHK, nCol_SCHK, nCol_SCHK+20, nCol_SCHK, nCol_SCHK,         5 ,  5 };

        public enum enmSel_EXAM_SPECMST_JEPSU       {    IPDOPD,     SPECNO,       PANO,        BI,      SNAME,       SEX,         AGE,  DEPTCODE,     WARD,      ROOM,        MASTERNM,  JEPSUNAME, JEPSUNAME2};
        public string[] sSel_EXAM_SPECMST_JEPSU =   {    "구분", "검체번호", "등록번호",    "유형", "환자성명",    "성별",      "나이",      "과",   "병동",    "병실",        "검사명",   "접수자", "진담검사" };
        public int[] nSel_EXAM_SPECMST_JEPSU    =   { nCol_SCHK,  nCol_PANO,  nCol_PANO, nCol_PANO, nCol_SNAME, nCol_SCHK, nCol_SCHK+5,  nCol_AGE, nCol_AGE,  nCol_AGE,  nCol_ORDERNAME,  nCol_PANO, nCol_AGE };

        public enum enmSel_EXAM_SPECMST_HIS     {     SPECNO,       PANO,      SNAME,     IPDOPD,  DEPTCODE,     WARD,  BLOODDATE,     INPS  };
        public string[] sSel_EXAM_SPECMST_HIS = { "검체번호", "등록번호", "환자성명", "환자구분",      "과",   "병동", "출력일시",  "출력자" };
        public int[] nSel_EXAM_SPECMST_HIS    = {  nCol_PANO,  nCol_PANO,  nCol_PANO,   nCol_AGE, nCol_SCHK, nCol_AGE,  nCol_TIME, nCol_PANO };
        
        //                                                          HR014^            ^       R^             ^ AAAC3RAEaAAAwMfAAX^             ^ 2018-03-30 00:09^  2018-03-30 00:10^
        //                                                 {            0,           1,       2,            3,                  4,            5,                6,                 7,}
        //          2018.04.07.김홍록: AS-IS TORD.Order(j) {strMasterCode, strSpecCode, strSTRT, strDrComment,           strROWID, strBloodTime,     strOrderDate,       strSendDate,}
        //                             AS-IS TORD.Order(j)                                                                                                                                                                                         2,              3,          0,         4,          1,          5,         6,         7,        
        //                             AS-IS TORD.Order(j)                                                                                                                                                                                         2,              3,          0,         4,          1,          5,         6,         7,        
        public enum enmSel_EXAM_ORDER_DETAIL      {       CHK,     PANO,  SNAME ,   BI,   AGE,   AGEMM,   SEX,   DEPTCODE,   DRCODE,   WARD,   ROOM,    IN_PANO,     SPECNO,       EXAMNAME,   SPECNM,   TUBECODE,    TUBENM,   SDATE_CHK,       STRT,      DRCOMMENT, MASTERCODE,   ROWID_R,     DRSPEC,  BLOODDATE, ORDERDATE,  SENDDATE,   ORDERNO,       QTY,   BDATE ,     RDATE ,  IPDOPD ,  SDATE  , WSCODE1   , WSCODE1POS  , RESULTIN  , UNITCODE   ,   SPECCODE,   MOTHER,   EQUCODE1,   PIECE,   GBTLA,   SERIES , GWA_EXAM_CHECK    , WSGRP_TITLE  , WS_GRP  , WS_YAK    };
        public string[] sSel_EXAM_ORDER_DETAIL  = {    "제외",   "PANO", "SNAME", "BI", "AGE", "AGEMM", "SEX", "DEPTCODE", "DRCODE", "WARD", "ROOM", "당일입원", "검체번호",       "검사명",   "검체", "TUBECODE",    "용기",  "수납취소",     "응급",   "의사코멘트", "검사코드", "ROWID_R", "SPECCODE", "채혈일자","처방일자","전송일자","처방번호",    "수량",  "BDATE", "예약일자", "IPDOPD", "SDATE" , "WSCODE1" , "WSCODE1POS", "RESULTIN", "UNITCODE" , "SPECCODE", "MOTHER", "EQUCODE1", "PIECE", "GBTLA", "SERIES" , "GWA_EXAM_CHECK"  , "WSGRP_TITLE", "WS_GRP", "WS_YAK"  };
        public int[] nSel_EXAM_ORDER_DETAIL     = { nCol_SCHK,        5,       5,    5,     5,       5,     5,          5,        5,      5,      5,          5,  nCol_PANO, nCol_ORDERNAME, nCol_AGE,          5,  nCol_AGE,    nCol_AGE, nCol_SCHK , nCol_ORDERNAME,   nCol_AGE,         5,          5,          5, nCol_TIME, nCol_TIME,         5, nCol_SCHK,        5,  nCol_DATE,        5,      5  ,          5,            5,          5,          5 ,          5,        5,          5,      5 ,       5,         5,                  5,         5,       5     ,         5 };

        //                                            1,        2,      3,       4,       5,     6,          7,      8,    9,        10,    11,       12,       13,      14,     15,          16,        17,         18,    19,      20,  21,     22   
        public enum enmSel_EXAM_MASTER_BARCODE {BAR_CNT, SPECCODE, WS_GRP, TUBECODE, WSCODE, WSPOS, MASTERCODE, SPECNO, STRT, DRCOMMENT, WSBAR, RESULTIN, UNITCODE, EQUCODE, SUCODE, WSGRP_TITLE, BLOODTIME, GB_GWAEXAM, PIECE, ORDERNO, PRT, GBTLA};

        public enum enmSel_EXAM_SPECMST_RCP03       {       CHK, RECEIVEDATE,      SPECNO,        PANO,      SNAME,  IPDOPD_NM,  DEPTCODE,      WARD,     ROOM,     WORKSTS, EXAM_NAME, SPECCODE_NM,    STATUS_NM,      BDATE,   BLOODDATE, RESULTDATE,   STATUS,   CANCEL_NM,   IPDOPD,   PB,  CANCEL  ,     DRCODE, DR_NM, REMARK };
        public string[] sSel_EXAM_SPECMST_RCP03 =   {    "선택",  "접수일시",  "검체번호",  "등록번호", "환자성명", "환자구분",      "과",    "병동",   "병실",        "WS",  "검사명",      "검체",       "상태", "처방일자",  "채혈일시", "결과일시", "STATUS", "CANCEL_NM", "IPDOPD", "PB", "CANCEL" , "의사번호", "의사성명" , "비고"};
        public int[] nSel_EXAM_SPECMST_RCP03 =      { nCol_SCHK,   nCol_TIME,   nCol_PANO,   nCol_PANO,  nCol_PANO,  nCol_IOPD,  nCol_AGE,  nCol_AGE, nCol_AGE, nCol_AGE+10, nCol_NAME, nCol_AGE+30,  nCol_AGE+30,  nCol_DATE,   nCol_TIME,  nCol_TIME,        5,           5,        5,    5,       5  ,          5,  nCol_PANO , 200};

        public enum enmSel_OCS_OORDER_GBSUNAP     {        PTNO,     SPECNO,   DEPTCODE,      DR_NM,  ORDERCODE,      ORDERNAME,      QTY,    GBSUNAP };
        public string[] sSel_OCS_OORDER_GBSUNAP = {  "등록번호", "처방일자",       "과", "의사성명", "처방코드",       "처방명",   "수량", "수납여부" };
        public int[] nSel_OCS_OORDER_GBSUNAP    = {   nCol_PANO,  nCol_DATE,   nCol_AGE,  nCol_PANO,   nCol_AGE, nCol_ORDERNAME, nCol_AGE, nCol_AGE };

        public enum enmSel_OCS_OORDER_EXCUTE        {       CHK,      BDATE,       PANO,      SNAME,      AGE,      SEX,  DEPTCODE,     DR_NM, MASTERCODE,       EXAMNAME,      RDATE,      DRCOMMENT,   ROWID_R,   PART2,   PART2_NM };
        public string[] sSel_OCS_OORDER_EXCUTE  =   {    "선택", "처방일자", "등록번호", "환자성명",   "나이",   "성별",      "과",  "의사명", "검사코드",       "검사명", "예약일자",     "의사비고", "ROWID_R", "PART2",    "확인자"};
        public int[] nSel_OCS_OORDER_EXCUTE     =   { nCol_SCHK,  nCol_DATE,  nCol_PANO, nCol_SNAME, nCol_AGE, nCol_AGE, nCol_AGE, nCol_SNAME, nCol_SNAME, nCol_ORDERNAME,  nCol_DATE, nCol_ORDERNAME,         5,       5, nCol_SNAME };

        public string up_EXAM_ORDER_PART(PsmhDb pDbCon, string strROWID_R, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_ORDER                                \r\n";
            SQL += "    SET PART2 = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += "      , GDATE = SYSDATE                                      \r\n";
            SQL += "      , UPDT  = SYSDATE                                      \r\n";
            SQL += "      , UPPS  = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += " WHERE 1=1                                                   \r\n";
            SQL += "   AND ROWID  = " + ComFunc.covSqlstr(strROWID_R, false);

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }

            return SqlErr;
        }

        public DataSet sel_OCS_OORDER_EXCUTE(PsmhDb pDbCon, string strFDate, string strTDate, string strSTATUS, bool isPART)
        {
            DataSet ds = null;

            SQL = "";

            SQL += "  SELECT                                                                \r\n";
            SQL += "    	    '' 										AS CHK              \r\n";
            SQL += "  	  	, TO_CHAR(BDATE,'YYYY-MM-DD') 				AS BDATE            \r\n";
            SQL += "  	  	, A.PANO                                    AS PANO             \r\n";
            SQL += "  	  	, A.SNAME                                   AS SNAME            \r\n";
            SQL += "  	  	, A.AGE                                     AS AGE              \r\n";
            SQL += "  	  	, A.SEX                                     AS SEX              \r\n";
            SQL += "  	  	, A.DEPTCODE                                AS DEPTCODE         \r\n";
            SQL += "  	    , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE) AS DR_NM            \r\n";
            SQL += "  	   	, A.MASTERCODE								AS MASTERCODE       \r\n";
            SQL += "  	  	, B.EXAMNAME                                AS EXAMNAME         \r\n";
            SQL += "  	  	, TO_CHAR(RDATE,'YYYY-MM-DD') 				AS RDATE            \r\n";
            SQL += "    		, A.DRCOMMENT							AS DRCOMMENT        \r\n";
            SQL += "  	  	, A.ROWID                                   AS ROWID_R          \r\n";
            SQL += "  	  	, A.PART2                                   AS PART2            \r\n";
            SQL += "  	  	, KOSMOS_OCS.FC_BAS_USER(TRIM(A.PART2))		AS PART2_NM         \r\n";
            SQL += "                                                                        \r\n";
            SQL += "    	FROM KOSMOS_OCS.EXAM_ORDER A                                    \r\n";
            SQL += "    	   , KOSMOS_OCS.EXAM_MASTER B                                   \r\n";
            SQL += "     WHERE 1=1                                                          \r\n";
            SQL += "       AND (                                                            \r\n";
            SQL += "  		   (                                                            \r\n";
            SQL += "  		      A.BDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "  		                  AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "  			)                                                           \r\n";
            SQL += "  			OR                                                          \r\n";
            SQL += "  			(                                                           \r\n";
            SQL += "  				A.RDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "  				            AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "  			)                                                           \r\n";
            SQL += "  		)                                                               \r\n";
            SQL += "       AND (A.CANCEL IS NULL OR A.CANCEL = '' OR A.CANCEL = ' ')        \r\n";

            if (strSTATUS.Equals("N"))
            {
                SQL += "       AND (SPECNO IS NULL OR SPECNO ='')                           \r\n";
            }
            else if (strSTATUS.Equals("Y"))
            {
                SQL += "       AND SPECNO IS NOT NULL                                       \r\n";
            }

            if (isPART == true)
            {
                SQL += "       AND A.PART2 IS NULL                                       \r\n";
            }
            else
            {
                SQL += "       AND A.PART2 IS NOT NULL                                       \r\n";
            }
                        
            SQL += "       AND IPDOPD ='O'                                                  \r\n";
            SQL += "       AND (A.SDATE IS NULL OR A.SDATE ='')                             \r\n";
            SQL += "       AND DEPTCODE NOT IN ('HR','TO')                                  \r\n";
            SQL += "       AND A.MASTERCODE = B.MASTERCODE                                  \r\n";
            SQL += "     ORDER BY  BDATE, PANO,DEPTCODE, MASTERCODE                         \r\n";
                                                                                            
            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return ds;
        }

        public DataTable sel_EXAM_SPECMST_PANO(PsmhDb pDbCon, string strSPECNO)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "   SELECT                                                                                \r\n";
            SQL += "   		  PANO                          \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_SPECMST 		A                                                \r\n";
            SQL += "   WHERE 1=1                                                                             \r\n";
            SQL += "     AND A.SPECNO = " + ComFunc.covSqlstr(strSPECNO, false);

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataSet sel_OCS_OORDER_GBSUNAP(PsmhDb pDbCon, string strBDATE, string strPANO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT A.PTNO                                     AS PTNO         \r\n";
            SQL += "  	 , TO_CHAR(A.BDATE, 'YYYY-MM-DD')               AS BDATE        \r\n";
            SQL += "  	 , A.DEPTCODE                                   AS DEPTCODE     \r\n";
            SQL += "  	 , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE)    AS DR_NM        \r\n";
            SQL += "  	 , A.ORDERCODE                                  AS ORDERCODE    \r\n";
            SQL += "  	 , B.ORDERNAME                                  AS ORDERNAME    \r\n";
            SQL += "  	 , A.QTY                                        AS QTY          \r\n";
            SQL += "  	 , DECODE(A.GBSUNAP,'0', '미수납','수납')       AS GBSUNAP      \r\n";
            SQL += "    FROM KOSMOS_OCS.OCS_OORDER A                                    \r\n";
            SQL += "       , KOSMOS_OCS.OCS_ORDERCODE B                                 \r\n";
            SQL += "   WHERE 1=1                                                        \r\n";
            SQL += "     AND A.BDATE = " + ComFunc.covSqlDate(strBDATE, false);
            SQL += "     AND A.PTNO  = " + ComFunc.covSqlstr(strPANO, false);
            SQL += "     AND A.NAL 	 > 0                                                \r\n";
            SQL += "     AND A.SEQNO > 0                                                \r\n";
            SQL += "     AND A.ORDERCODE = B.ORDERCODE                                  \r\n";
            SQL += "     AND A.SLIPNO IN                                                \r\n";
            SQL += "     (                                                              \r\n";
            SQL += "    	 '0010'                                                     \r\n";
            SQL += "    	,'0014'                                                     \r\n";
            SQL += "    	,'0016'                                                     \r\n";
            SQL += "    	,'0018'                                                     \r\n";
            SQL += "    	,'0022'                                                     \r\n";
            SQL += "    	,'0024'                                                     \r\n";
            SQL += "    	,'0026'                                                     \r\n";
            SQL += "    	,'0028'                                                     \r\n";
            SQL += "    	,'0030'                                                     \r\n";
            SQL += "    	,'0032'                                                     \r\n";
            SQL += "    	,'0010'                                                     \r\n";
            SQL += "    	,'0014'                                                     \r\n";
            SQL += "    	,'0016'                                                     \r\n";
            SQL += "    	,'0018'                                                     \r\n";
            SQL += "    	,'0022'                                                     \r\n";
            SQL += "    	,'0024'                                                     \r\n";
            SQL += "    	,'0026'                                                     \r\n";
            SQL += "    	,'0028'                                                     \r\n";
            SQL += "    	,'0030'                                                     \r\n";
            SQL += "    	,'0032'                                                     \r\n";
            SQL += "    	,'0034'                                                     \r\n";
            SQL += "    	,'0040'                                                     \r\n";
            SQL += "    	,'0042'                                                     \r\n";
            SQL += "    	,'0050'                                                     \r\n";
            SQL += "     )                                                              \r\n";
            SQL += "  ORDER BY A.PTNO, A.DEPTCODE, A.SEQNO                              \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return ds;
        }

        public DataSet sel_EXAM_SPECMST_RCP03(PsmhDb pDbCon, string strFDATE, string strTDATE, string strPANO, string strSNAME, bool isEXCP
                                            , string strWS, string strWARD, bool isPB, bool isIPD, bool isOPD, bool is61, bool is62, string strDEPT
                                            , bool isReciveDateNull, bool isCancel, bool isGwa)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                                                    \r\n";
            SQL += "  		'' 																		    AS CHK			-- 01   \r\n";
            SQL += "  		, TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD HH24:MI') 							    AS RECEIVEDATE  -- 03   \r\n";
            SQL += "  		, A.SPECNO                                    							    AS SPECNO		-- 02   \r\n";
            SQL += "  		, A.PANO        														    AS PANO			-- 16   \r\n";
            //SQL += "  		, A.SNAME                                     							    AS SNAME		-- 04   \r\n";
            //2019-10-01 안정수, 이미경J 요청으로 환경검사 건은 부서명으로 표기되도록 보완
            SQL += "  		, CASE WHEN A.PANO = '81000014' OR A.PANO = '11077917' THEN KOSMOS_OCS.FC_BAS_BUSE_NAME((SELECT BUCODE \r\n";
            SQL += "  		                                                                                        FROM KOSMOS_PMPA.ENVIRONMENT_ORDER \r\n";
            SQL += "  		                                                                                        WHERE BARCODE = A.SPECNO))         \r\n";
            SQL += "  		  ELSE A.SNAME END 															AS SNAME		-- 04                          \r\n";
            SQL += "  		, CASE WHEN A.IPDOPD ='I' THEN '입원'                                                               \r\n";
            SQL += "  		       WHEN A.IPDOPD !='I' AND A.BI ='61' THEN '종검'                                               \r\n";
            SQL += "  		       WHEN A.IPDOPD !='I' AND A.BI ='71' THEN '건진'                                               \r\n";
            SQL += "  		       WHEN A.IPDOPD !='I' AND A.BI ='81' THEN '수탁'                                               \r\n";
            SQL += "  		       ELSE '외래' END													    AS IPDOPD_NM	-- 05   \r\n";
            SQL += "  		, A.DEPTCODE                                  							    AS DEPTCODE		-- 06   \r\n";
            SQL += "  		, A.WARD                                      							    AS WARD			-- 07   \r\n";
            SQL += "  		, A.ROOM                                      							    AS ROOM			-- 07   \r\n";
            SQL += "  		, A.WORKSTS                                   							    AS WORKSTS		-- 09   \r\n";
            SQL += "  		, KOSMOS_OCS.FC_EXAM_RESULTC_EXAMNAME(A.SPECNO) 							AS EXAM_NAME	-- 10   \r\n";
            SQL += "  		, KOSMOS_OCS.FC_EXAM_SPECMST_NM('14', A.SPECCODE,'Y') 					    AS SPECCODE_NM  -- 11   \r\n";
            SQL += "  		, KOSMOS_OCS.FC_BAS_BCODE_NAME('EXAM_STATUS', A.STATUS) 	                AS STATUS_NM    -- 12   \r\n";
            SQL += "  		, TO_CHAR(A.BDATE,'YYYY-MM-DD') 										    AS BDATE        -- 14   \r\n";
            SQL += "  		, TO_CHAR(A.BLOODDATE,'YYYY-MM-DD HH24:MI') 							    AS BLOODDATE	-- 13   \r\n";

            SQL += "  		, TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI')							    AS RESULTDATE   -- 15   \r\n";
            
            SQL += "  		, A.STATUS        														    AS STATUS       -- 17   \r\n";
            SQL += "  		, KOSMOS_OCS.FC_EXAM_SPECMST_NM('21', A.CANCEL,'N')   				        AS CANCEL_NM    -- 18   \r\n";
            SQL += "  		, A.IPDOPD                                              			        AS IPDOPD       -- 19   \r\n";

            SQL += "    , (                                                                                                     \r\n";
            SQL += "          SELECT CASE WHEN COUNT(*) > 0 THEN 'Y'                                                            \r\n";
            SQL += "                  ELSE 'N'                                                                                  \r\n";
            SQL += "                  END                                                                                       \r\n";
            SQL += "           FROM KOSMOS_OCS.EXAM_RESULTC B                                                                   \r\n";
            SQL += "          WHERE B.SPECNO = A.SPECNO                                                                         \r\n";
            SQL += "            AND B.SUBCODE = 'HR10'                                                                          \r\n";
            SQL += "        )                                                                           AS PB   --20            \r\n";

            SQL += "  		, A.CANCEL                                             			            AS CANCEL       -- 21   \r\n";
            SQL += "  		, A.DRCODE                                    							    AS DRCODE		-- 08   \r\n";
            SQL += "  		, KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE)									AS DR_NM		-- 08   \r\n";
            if(strWARD.Trim().Equals("33") == true || strWARD.Trim().Equals("35") == true || strWARD.Trim().Equals("40") == true || strWARD.Trim().Equals("50") == true || strWARD.Trim().Equals("60") == true || strWARD.Trim().Equals("70") == true | strWARD.Trim().Equals("80") == true || strWARD.Trim().Equals("53") == true || strWARD.Trim().Equals("55") == true || strWARD.Trim().Equals("63") == true || strWARD.Trim().Equals("65") == true || strWARD.Trim().Equals("73") == true || strWARD.Trim().Equals("75") == true || strWARD.Trim().Equals("83") == true)
            {
                SQL += "    ,C.REMARK                                                                                           \r\n";
                SQL += "    FROM KOSMOS_OCS.EXAM_SPECMST A, KOSMOS_OCS.OCS_IORDER C                                                 \r\n";
            }
            else
            {
                SQL += "    FROM KOSMOS_OCS.EXAM_SPECMST A                                                             \r\n";
            }
            SQL += "   WHERE 1=1                                                                                                \r\n";

            if (strWARD.Trim().Equals("33") == true || strWARD.Trim().Equals("35") == true || strWARD.Trim().Equals("40") == true || strWARD.Trim().Equals("50") == true || strWARD.Trim().Equals("60") == true || strWARD.Trim().Equals("70") == true | strWARD.Trim().Equals("80") == true || strWARD.Trim().Equals("53") == true || strWARD.Trim().Equals("55") == true || strWARD.Trim().Equals("63") == true || strWARD.Trim().Equals("65") == true || strWARD.Trim().Equals("73") == true || strWARD.Trim().Equals("75") == true || strWARD.Trim().Equals("83") == true)
            {
                SQL += "         AND A.ORDERNO = C.ORDERNO                                              \r\n";
                SQL += "         AND C.NAL NOT IN ('-1')	                                                \r\n";
            }

            if (string.IsNullOrEmpty(strWS.Trim()) == false && strWS.Equals("*") == false)
            {
                SQL += "     AND A.WORKSTS LIKE '%" + strWS.Trim() + "%'";
            }

            if (string.IsNullOrEmpty(strPANO.Trim()) == false)
            {
                SQL += "     AND A.PANO = " + ComFunc.covSqlstr(strPANO, false);
            }

            if (string.IsNullOrEmpty(strSNAME.Trim()) == false)
            {
                if (string.IsNullOrEmpty(strPANO.Trim()) == true)
                {
                    SQL += "     AND A.SNAME LIKE '" + strSNAME.Trim() + "%'";
                }                
            }

            if (strDEPT.Equals(clsParam.EXAM_DEPT_LIS) == true)
            {
                if (isEXCP == true)
                {
                    SQL += "     AND A.RECEIVEDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                    SQL += "   				           AND " + ComFunc.covSqlDate(strTDATE, false) + "+1 \r\n                           ";
                    SQL += "     AND A.STATUS NOT IN ('00','05','06') 				-- 미완료만                                         \r\n";
                }
                else
                {
                    if (isReciveDateNull == true)
                    {
                        SQL += "     AND TRUNC(A.BLOODDATE) = " + ComFunc.covSqlDate(strFDATE, false) ;
                        SQL += "     AND A.RECEIVEDATE IS NULL 				-- 미완료만                                         \r\n";
                        SQL += "     AND A.STATUS NOT IN ('06') 				-- 미완료만                                         \r\n";
                    }
                    else
                    {
                        SQL += "     AND A.BDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                        SQL += "   			 	     AND " + ComFunc.covSqlDate(strTDATE, false);
                    }
                }

                //2018-08-10 안정수, 기존 과/병동검사 체크부분 누락되어 추가함
                if (isGwa == true)
                {
                    SQL += "     AND A.GB_GWAEXAM = 'Y'                                           \r\n";
                }

                else
                {
                    SQL += "     AND (A.GB_GWAEXAM IS NULL OR A.GB_GWAEXAM <> 'Y')    -- 응급실검사(과)                                     \r\n";
                }

                if (string.IsNullOrEmpty(strWARD.Trim()) == false && strWARD.Equals("*") == false)
                {
                    SQL += "     AND A.WARD = " + ComFunc.covSqlstr(strWARD.Trim(), false);
                }

                // bool isIPD, bool isOPD, bool is61, bool is62
                if (isIPD == true || isOPD == true || is61 == true || is62 == true)
                {
                    SQL += "     AND (   \r\n";

                    if (isIPD == true)
                    {
                        SQL += "        A.IPDOPD ='I' OR ";
                    }

                    if (isOPD == true)
                    {
                        SQL += "       (A.IPDOPD='O' AND A.BI < '61') OR ";
                    }

                    if (is61 == true)
                    {
                        SQL += "       A.BI = '61' OR ";
                    }

                    if (is62 == true)
                    {
                        SQL += "       A.BI = '62' OR ";
                    }

                    SQL = SQL.Substring(0, SQL.Length - 3) + "\r\n";

                    SQL += "           )  \r\n";
                }
            }

           

            else
            {
                #region 병동

                //2018-08-10 안정수, 기존 과/병동검사 체크부분 누락되어 추가함
                if (isGwa == true)
                {
                    SQL += "     AND A.GB_GWAEXAM = 'Y'                                           \r\n"; 
                }

                //if (string.IsNullOrEmpty(strWARD.Trim()) == false && strWARD.Equals("*") == false)
                if (string.IsNullOrEmpty(strWARD.Trim()) == false)
                {
                    if(strDEPT.Equals("GS") == true && strWARD.Equals("*") == true)
                    {
                        SQL += "     AND A.RECEIVEDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                        SQL += "   				           AND " + ComFunc.covSqlDate(strTDATE, false) + "+1 \r\n                           ";
                    }
                    else
                    {
                        SQL += "     AND A.BDATE     BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                        SQL += "   			 	         AND " + ComFunc.covSqlDate(strTDATE, false);
                    }

                    if (isReciveDateNull == true && isCancel == true)
                    {
                        SQL += "   AND (A.RECEIVEDATE IS NULL OR A.STATUS = '06')  \r\n";
                    }
                    else if (isReciveDateNull == true && isCancel == false)
                    {
                        SQL += "   AND A.RECEIVEDATE IS NULL \r\n";
                    }
                    else if (isReciveDateNull == false && isCancel == false)
                    {
                        //SQL += "   AND A.STATUS != '06' \r\n";
                    }
                    else if (isReciveDateNull == false && isCancel == true)
                    {
                        SQL += "   AND A.STATUS = '06' \r\n";
                    }

                    if (strWARD.Trim().Equals("HD") == true || strWARD.Trim().Equals("ER") == true)
                    {
                        SQL += "     AND A.DEPTCODE = " + ComFunc.covSqlstr(strWARD.Trim(), false);
                    }
                    else if (strWARD.Trim().Equals("OP") == true)
                    {
                        SQL += " AND A.PANO IN ( SELECT PANO FROM KOSMOS_PMPA.ORAN_SLIP       \r\n";
                        SQL += "                WHERE OPDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                        SQL += "                                 AND " + ComFunc.covSqlDate(strTDATE, false) + " )     \r\n";
                    }
                    else if (strWARD.Trim().Equals("ENDO") == true)
                    {
                        SQL += " AND PANO IN ( SELECT PANO FROM KOSMOS_OCS.ENDO_JUPMST       \r\n";
                        SQL += "                WHERE RDATE  BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                        SQL += "                                 AND " + ComFunc.covSqlDate(strTDATE, false) + " )     \r\n";
                    }
                    else if (strWARD.Trim().Equals("ENTO") == true)
                    {
                        SQL += " AND PANO IN ( SELECT PTNO FROM KOSMOS_OCS.ENDO_JUPMST       \r\n";
                        SQL += "                WHERE RDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                        SQL += "                                 AND " + ComFunc.covSqlDate(strTDATE, false) + " +1 )  \r\n";
                        SQL += " AND DEPTCODE  IN ('TO')                                               \r\n";
                    }
                    else if (strWARD.Trim().Equals("ENHR") == true)
                    {
                        SQL += " AND PANO IN ( SELECT PTNO FROM KOSMOS_OCS.ENDO_JUPMST       \r\n";
                        SQL += "                WHERE RDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                        SQL += "                                AND " + ComFunc.covSqlDate(strTDATE, false) + " +1)    \r\n";
                        SQL += " AND DEPTCODE  IN ('HR')                               \r\n";
                    }
                    else if (strWARD.Trim().Equals("XRAY") == true)
                    {
                        SQL += " AND PANO IN ( SELECT PANO FROM KOSMOS_PMPA.XRAY_DETAIL       \r\n";
                        SQL += "                WHERE SEEKDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                        SQL += "                                   AND " + ComFunc.covSqlDate(strTDATE, false) + "     \r\n";
                        SQL += "                  AND XCODE IN ('US11','US11U','HA434'))                               \r\n";
                    }
                    else if (strWARD.Trim().Equals("OP") == true)
                    {
                        SQL += " AND PANO IN ( SELECT PANO FROM KOSMOS_PMPA.ORAN_SLIP       \r\n";
                        SQL += "                WHERE OPDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                        SQL += "                                 AND " + ComFunc.covSqlDate(strTDATE, false) + " )   \r\n";
                    }
                    else if (strWARD.Trim().Equals("OO") == true || strWARD.Trim().Equals("OB") == true)
                    {

                    }
                    else
                    {
                        if (strWARD.Equals("*") == false)
                        {



                            SQL += "  AND PANO IN (                                     \r\n";
                            SQL += "      SELECT PANO FROM KOSMOS_PMPA.IPD_NEW_MASTER   \r\n";

                            if (strWARD.Trim().Equals("NR") == true)
                            {
                                SQL += "      WHERE WARDCODE IN ('NR','IQ','DR','3B')   \r\n";
                            }
                            else
                            {
                                SQL += "      WHERE WARDCODE = " + ComFunc.covSqlstr(strWARD.Trim(), false);
                            }

                            SQL += "         AND AMSET1   = '0'                                 \r\n";
                            SQL += "         AND AMSET6  <> '*'                                 \r\n";
                            SQL += "         AND GBSTS IN ('0', '2', '3', '4')                  \r\n";
                            SQL += "         AND PANO     < '90000000'                          \r\n";
                            SQL += "         AND   JDATE =TO_DATE('1900-01-01','YYYY-MM-DD')    \r\n";
                            SQL += "         GROUP BY PANO)                                     \r\n";
                        }
                    }

                }
                #endregion
            }

            if (isEXCP == true)
            {
                SQL += "   ORDER BY A.RECEIVEDATE DESC,A.SPECNO                                                      \r\n";
            }
            else 
            {
                SQL += "   ORDER BY A.SPECNO                                                                         \r\n";
            }

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장 
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return ds;
        }

        public DataSet sel_EXAM_SPECMST_HIS(PsmhDb pDbCon, string strBDATE, string strPANO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                           \r\n";
            SQL += "  		 SPECNO                                    \r\n";
            SQL += "       , PANO                                      \r\n";
            SQL += "       , SNAME                                     \r\n";
            SQL += "       , IPDOPD                                    \r\n";
            SQL += "       , DEPTCODE                                  \r\n";
            SQL += "       , WARD                                      \r\n";
            SQL += "       , BLOODDATE                                 \r\n";
            SQL += "       , KOSMOS_OCS.FC_BAS_USER_NAME(INPS) AS INPS \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_SPECMST                   \r\n";
            SQL += "   WHERE 1=1                                       \r\n";
            SQL += "     AND BDATE 	= " + ComFunc.covSqlDate(strBDATE, false);
            SQL += "     AND PANO 	= " + ComFunc.covSqlstr(strPANO, false);
            SQL += "  ORDER BY SPECNO                                  \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return ds;
        }

        public DataTable sel_EXAM_MASTER_BARCODE(PsmhDb pDbCon, bool isSerial, string strDrSpec, string strMASTERCODE, string strSuCode, string strSTRT, string strDRCOMMENT, string strBLOODTIME, string strGBGWAEXAM, string strORDERNO, string strPRT)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "   SELECT                                                                                \r\n";

            if (isSerial == false)
            {
                SQL += "         '00'	    										AS BAR_CNT		-- 01    \r\n";
            }
            else
            {
                SQL += "     TRIM(TO_CHAR(CASE WHEN ROWNUM = 1 THEN 0 ELSE ROWNUM + 20 END, '00'))				AS BAR_CNT		-- 01    \r\n";
            }

            if (string.IsNullOrEmpty(strDrSpec.Trim()) == false)
            {
                SQL += "   		, '" + strDrSpec + "'                            AS SPECCODE    -- 02        \r\n";
            }
            else
            {
                SQL += "   		, A.SPECCODE                                     AS SPECCODE    -- 02        \r\n";
            }
             
            
            SQL += "   		, KOSMOS_OCS.FC_EXAM_SPECMST_NM('12',A.WSCODE1, 'W') AS WS_GRP      -- 03        \r\n";
            SQL += "   		, TUBECODE											 AS TUBECODE	-- 04        \r\n";
            SQL += "   		, WSCODE1											 AS WSCODE		-- 05        \r\n";
            SQL += "   		, TRIM(TO_CHAR(WSCODE1POS,'00000')) 				 AS WSPOS	-- 06            \r\n";
            SQL += "          , A.MASTERCODE									 AS MASTERCODE	-- 07        \r\n";
            SQL += "  		, '           '										 AS SPECNO		-- 08        \r\n";
            SQL += "  		, '" + strSTRT.Trim() + "'		       				 AS STRT		-- 09        \r\n";
            SQL += "  		, '                                                                                 '                      AS DRCOMMENT	-- 10        \r\n";
            SQL += "   		, KOSMOS_OCS.FC_EXAM_SPECMST_NM('12',WSCODE1, 'Y')   AS WSBAR       -- 11        \r\n";
            SQL += "   		, RESULTIN                                           AS RESULTIN	-- 12        \r\n";
            SQL += "   		, UNITCODE											 AS UNITCODE	-- 13        \r\n";
            SQL += "   		, EQUCODE1                                           AS EQUCODE		-- 14        \r\n";
            //SQL += "   		, '" + strSuCode+ "'           					     AS SUCODE		-- 15        \r\n";
            //2019-04-08 안정수, 칼럼 자리수 오류로 인한 조건 추가 
            if(strSuCode == "")
            {
                SQL += "   		, '                                           ' AS SUCODE		-- 15        \r\n";
            }
            else
            {
                SQL += "   		, '" + strSuCode + "'           				AS SUCODE		-- 15        \r\n";            }

            
            SQL += "   		, KOSMOS_OCS.FC_WSGRP(A.WSCODE1)					 AS WSGRP_TITLE	-- 16        \r\n";

            //2019-04-08 안정수, 칼럼 자리수 오류로 인한 조건 추가 
            if (strBLOODTIME == "")
            {
                SQL += "   		, '                                 '	    				 AS BLOODTIME	-- 17        \r\n";
            }
            else
            {
                SQL += "   		, '" + ComFunc.SetAutoZero(strBLOODTIME.Trim(), 30) + "'	    				 AS BLOODTIME	-- 17        \r\n";
            }            
            SQL += "   		, '" + strGBGWAEXAM.Trim() + "'						 AS GB_GWAEXAM	-- 18        \r\n";
            SQL += "   		, PIECE									             AS PIECE		-- 19        \r\n";

            //2019-04-08 안정수, 칼럼 자리수 오류로 인한 조건 추가 
            if (strORDERNO == "")
            {
                SQL += "   		, '                 '   AS ORDERNO		-- 20        \r\n";
            }
            else
            {
                SQL += "   		, '" + ComFunc.SetAutoZero(strORDERNO.Trim(), 9) + "'   AS ORDERNO		-- 20        \r\n";
            }
            
            SQL += "   		, '" + strPRT.Trim()    + "'	    				 AS PRT			-- 21        \r\n";
            SQL += "   		, A.GBTLA			    							 AS GBTLA		-- 22        \r\n";            
            SQL += "    FROM KOSMOS_OCS.EXAM_MASTER 		A                                                \r\n";
            SQL += "       , KOSMOS_OCS.EXAM_MASTER_SUB 	B                                                \r\n";
            SQL += "   WHERE 1=1                                                                             \r\n";
            SQL += "     AND A.MASTERCODE = TRIM(B.NORMAL)                                                   \r\n";
            SQL += "     AND B.MASTERCODE = " + ComFunc.covSqlstr(strMASTERCODE, false);
            SQL += "     AND B.GUBUN = '31'                                                                  \r\n";
            SQL += "     AND B.NORMAL > ' '                                                                  \r\n";
            SQL += "ORDER BY SORT                                                                            \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt; 
        }

        public DataSet sel_EXAM_ORDER_DETAIL(PsmhDb pDbCon, string strPANO, string strBDATE, string strIPDOPD, string strDEPTCODE, enmSel_EXAM_ORDER_STATUS pSTATUS, bool isMenual, enmSel_EXAM_ORDER_PT_STATUS pSTATUS_PT, string strJOB, string FstrWard)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                                                                    \r\n";
            SQL += "  		 	'False'  						 					AS CHK   	-- SS1 01                                       \r\n";
            SQL += "  		, 	O.PANO						 						AS PANO 	-- SS1 01                                       \r\n";
            SQL += "  		, 	O.SNAME                      						AS SNAME	-- SS1 02                                       \r\n";
            SQL += "  		,	O.BI												AS BI   	-- SS1 03                                       \r\n";
            SQL += "  		, 	O.AGE												AS AGE      -- SS1 04                                       \r\n";
            SQL += "  		,   O.AGEMM                         					AS AGEMM    -- SS1 05                                       \r\n";
            SQL += "  		, 	O.SEX     											AS SEX      -- SS1 06                                       \r\n";
            SQL += "  		, 	O.DEPTCODE                      					AS DEPTCODE	-- SS1 07                                       \r\n";
            SQL += "  		, 	O.DRCODE											AS DRCODE	-- SS1 08                                       \r\n";
            SQL += "       	, 	O.WARD                                                                                                          \r\n";
            SQL += "  		, 	O.ROOM                                                                                                          \r\n";
            SQL += "  		,   (                                                                                                               \r\n";
            SQL += "  				SELECT CASE WHEN COUNT(*) > 0 THEN 'I'                                                                      \r\n";
            SQL += "  				            ELSE '' END                                                                                     \r\n";
            SQL += "  				  FROM KOSMOS_PMPA.IPD_NEW_MASTER                                                                           \r\n";
            SQL += "                   WHERE PANO = O.PANO                                                                                      \r\n";
            SQL += "                     AND GBSTS != '9'                                                                                       \r\n";
            SQL += "                     AND TRUNC(INDATE) = O.BDATE                                                                            \r\n";
            SQL += "              )							    					AS IN_PANO                                                  \r\n";
            SQL += "          ,	O.SPECNO											AS SPECNO	-- SS2 01                                       \r\n";
            SQL += "  		, 	M.EXAMNAME											AS EXAMNAME	-- SS2 02                                       \r\n";
            SQL += "                                                                                                                            \r\n";
            SQL += "  		,   (                                                                                                               \r\n";
            SQL += "  				CASE WHEN NVL(TRIM(O.SPECCODE),'^&') = '^&' THEN KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',M.SPECCODE,'Y')         \r\n";
            SQL += "  			 		 ELSE KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',O.SPECCODE,'Y') END                                            \r\n";
            SQL += "  			)	 												AS SPECNM -- SS2 03                                         \r\n";
            SQL += "  		,	M.TUBECODE                                                                                                      \r\n";
            SQL += "        ,   KOSMOS_OCS.FC_EXAM_SPECMST_NM('15',M.TUBECODE,'Y')	AS TUBENM -- SS2 04                                         \r\n";
            SQL += "        ,   (                                                                                                               \r\n";
            SQL += "  				CASE WHEN SDATE IS NOT NULL THEN '취소' ELSE '    ' END                                                     \r\n";
            SQL += "          	)                                           		AS SDATE_CHK                                                \r\n";
            SQL += "  		, 	CASE WHEN O.STRT = 'E' OR O.STRT = 'S' THEN '응급' ELSE '' END   AS STRT	  -- SS2 05                         \r\n";
            SQL += "  		, 	(                                                                                                               \r\n";
            SQL += "  				CASE WHEN RDATE IS NOT NULL THEN '☎ ' || TO_CHAR(O.RDATE,'YYYY-MM-DD') || ' ' || O.DRCOMMENT               \r\n";
            SQL += "  		             ELSE O.DRCOMMENT END                                                                                   \r\n";
            SQL += "  			)													AS DRCOMMENT 	-- SS2 06                                   \r\n";
            SQL += "  		,   O.MASTERCODE                                        AS MASTERCODE	-- SS2 07                                   \r\n";
            SQL += "  		, 	O.ROWID												AS ROWID_R		-- SS2 08                                   \r\n";
            SQL += "  		,   O.SPECCODE                      					AS DRSPEC	 	-- SS2 09                                   \r\n";
            SQL += "  		,   ''													AS BLOODDATE	-- SS2 10                                   \r\n";
            SQL += "  		,   TO_CHAR(O.ORDERDATE, 'YYYY-MM-DD HH24:MI') 			AS ORDERDATE	-- SS2 11                                   \r\n";
            SQL += "  		,   TO_CHAR(O.SENDDATE, 'YYYY-MM-DD HH24:MI') 			AS SENDDATE 	-- SS2 12                                   \r\n";
            SQL += "  		, 	O.ORDERNO											AS ORDERNO	    -- SS2 13                                   \r\n";
            SQL += "  		, 	NVL(TRIM(O.QTY),0)                                  AS QTY                                                      \r\n";
            SQL += "  		,	TO_CHAR(O.BDATE,'YYYY-MM-DD') BDATE                                                                             \r\n";
            SQL += "  		,   TO_CHAR(O.RDATE,'YYYY-MM-DD') RDATE                                                                             \r\n";
            SQL += "  		,   O.IPDOPD                                                                                                        \r\n";
            SQL += "  		,   TO_CHAR(O.SDATE,'YYYY-MM-DD') 						AS SDATE                                                    \r\n";
            SQL += "  		,   M.WSCODE1 						                    AS WSCODE1                                                  \r\n";
            SQL += "  		,   TO_CHAR(M.WSCODE1POS,'00000') 		                AS WSCODE1POS                                               \r\n";
            SQL += "  		,   M.RESULTIN 	                    	                AS RESULTIN                                                 \r\n";
            SQL += "  		,   M.UNITCODE 	                    	                AS UNITCODE                                                 \r\n";
            SQL += "  		,   M.SPECCODE 	                    	                AS SPECCODE                                                 \r\n";
            SQL += "  		,   M.MOTHER 	                    	                AS MOTHER                                                   \r\n";
            SQL += "  		,   M.EQUCODE1 	                    	                AS EQUCODE1                                                 \r\n";
            SQL += "  		,   M.PIECE 	                    	                AS PIECE                                                    \r\n";
            SQL += "  		,   M.GBTLA 	                    	                AS GBTLA                                                    \r\n";
            SQL += "  		,   M.SERIES 	                    	                AS SERIES                                                   \r\n";
            SQL += "  		,   KOSMOS_OCS.FC_BAS_BCODE_CODE_EXAM_GWA(M.MASTERCODE) AS GWA_EXAM_CHECK	                                        \r\n";
            SQL += "   		,   KOSMOS_OCS.FC_WSGRP(M.WSCODE1)					    AS WSGRP_TITLE	-- 16                                       \r\n";
            SQL += "   		,   KOSMOS_OCS.FC_EXAM_SPECMST_NM('12',M.WSCODE1, 'W')  AS WS_GRP      -- 03                                        \r\n";
            SQL += "   		,   KOSMOS_OCS.FC_EXAM_SPECMST_NM('12',M.WSCODE1, 'Y')  AS WS_YAK      -- 11                                        \r\n";
            //20210225 김욱동 이도경c요청
            SQL += "   		,   (CASE WHEN O.MASTERCODE ='CR42B' THEN '1' ELSE '0' END) AS EXCEPSORTCODE      -- 12                                        \r\n";

            SQL += "    FROM KOSMOS_OCS.EXAM_ORDER 	O                                                                                           \r\n";
            SQL += "      	,KOSMOS_OCS.EXAM_MASTER M                                                                                           \r\n";
            SQL += "     WHERE 1=1                                                                                                              \r\n";
            SQL += "       AND O.PANO       = " + ComFunc.covSqlstr(strPANO, false);            
            SQL += "       AND O.DEPTCODE   = " + ComFunc.covSqlstr(strDEPTCODE, false);
            SQL += "       AND (O.CANCEL IS NULL OR O.CANCEL = ' ')                                                                             \r\n";
            SQL += "       AND O.MASTERCODE = M.MASTERCODE                                                                                      \r\n";

            if (pSTATUS == enmSel_EXAM_ORDER_STATUS.READY)
            {
                SQL += "       AND O.SPECNO IS NULL                                                                                             \r\n";
            }
            else if (pSTATUS == enmSel_EXAM_ORDER_STATUS.RECIPT)
            {
                SQL += "       AND O.SPECNO IS NOT NULL                                                                                         \r\n";
            }

            if (strJOB.Trim().Equals(clsParam.EXAM_DEPT_LIS) || strJOB.Trim().Equals("OB"))
            {
                #region LIS
                SQL += "       AND O.IPDOPD     = " + ComFunc.covSqlstr(strIPDOPD, false);
                SQL += "       AND (                                                                                                                \r\n";
                SQL += "       		    O.BDATE = " + ComFunc.covSqlDate(strBDATE, false);
                SQL += "            OR  O.RDATE = " + ComFunc.covSqlDate(strBDATE, false);
                SQL += "           )                                                                                                                \r\n";

                if (isMenual == true)
                {
                    SQL += "       AND O.ORDERNO =  999                                                                                             \r\n";
                }
                else
                {
                    SQL += "       AND O.ORDERNO >= 2400                                                                                            \r\n";

                    if (pSTATUS_PT == enmSel_EXAM_ORDER_PT_STATUS.OPT)
                    {
                        SQL += "   AND CASE WHEN O.DEPTCODE ='OG' AND O.MASTERCODE IN (	SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE A WHERE GUBUN = 'EXAM_외래제외코드') THEN 'N' ELSE 'Y' END ='Y'        \r\n";
                        SQL += "      AND ( ((O.MASTERCODE = 'MI3URO') AND (O.SPECCODE <> '094A')) OR ((O.MASTERCODE <> 'MI3URO') AND (1=1)) )        \r\n";
                        SQL += "      AND ( ((O.MASTERCODE = 'MI03') AND (O.SPECCODE <> '094A')) OR ((O.MASTERCODE <> 'MI03') AND (1=1)) )        \r\n";
                        SQL += "      AND ( ((O.MASTERCODE = 'MI04') AND (O.SPECCODE <> '094A')) OR ((O.MASTERCODE <> 'MI04') AND (1=1)) )        \r\n";
                        SQL += "      AND ( ((O.MASTERCODE = 'MI05') AND (O.SPECCODE <> '094A')) OR ((O.MASTERCODE <> 'MI05') AND (1=1)) )        \r\n";
                    }
                    else if (pSTATUS_PT == enmSel_EXAM_ORDER_PT_STATUS.OG)
                    {
                        SQL += "       AND (O.MASTERCODE IN (SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE A WHERE GUBUN = 'EXAM_외래제외코드') OR (O.MASTERCODE IN ('MI3URO','MI03','MI04','MI05') AND O.SPECCODE = '094A'))         \r\n";
                        SQL += "      AND O.DEPTCODE   = 'OG'         \r\n";
                    }
                }
                //20210225 김욱동 이도경c요청
                SQL += "     ORDER BY EXCEPSORTCODE DESC,O.MASTERCODE, O.BDATE DESC                                                                                    \r\n";
                //SQL += "     ORDER BY O.MASTERCODE, O.BDATE DESC                                                                                    \r\n";

                #endregion  

            }
            else
            {

                if (FstrWard.Equals("ENTO") == true)
                {
                    SQL += "   AND O.ActDate = TO_DATE('" + strBDATE + "','YYYY-MM-DD')         \r\n";
                    SQL += "   AND ( O.MASTERCODE LIKE 'XR%' OR O.MASTERCODE IN ('MI04','MI11','MI03','MI361','MI321','GP11','MI05','MI36', 'MI34','MI37','GP15','MI364','XS03') ) \r\n";
                    SQL += "   AND O.IpdOpd IN ('O', 'I')   \r\n";
                    SQL += "   AND O.DEPTCODE IN ( 'TO')    \r\n";
                }
                else if (FstrWard.Equals("ENDO") == true)
                {
                    SQL += "  AND O.BDate = TO_DATE('" + strBDATE + "','YYYY-MM-DD')   \r\n";
                    //2018-08-21 안정수, 내시경실 요청으로 GP22A 추가
                    SQL += "  AND ( O.MASTERCODE LIKE 'XR%' OR O.MASTERCODE IN ('MI04','MI11','MI03','MI361','MI321','GP11','MI05','MI36', 'MI34','MI37','GP15','MI364','XS03','GP22A') ) \r\n";
                    SQL += "  AND O.IpdOpd IN ('O', 'I')               \r\n";
                    SQL += "  AND O.DEPTCODE NOT IN ('HR', 'TO' )     \r\n";
                }

                else if (FstrWard.Equals("ENHR") == true)
                {
                    SQL += "   AND O.BDate = TO_DATE('" + strBDATE + "','YYYY-MM-DD')   \r\n";
                    SQL += "   AND ( O.MASTERCODE LIKE 'XR%' OR O.MASTERCODE IN ('MI04','MI11','MI03','MI361','MI321','GP11','MI05','MI36', 'MI34','MI37','GP15','MI364','XS03') ) \r\n";
                    SQL += "   AND O.IpdOpd IN ('O', 'I')   \r\n";
                    SQL += "   AND O.DEPTCODE IN ('HR')     \r\n";
                }
                else if (FstrWard.Equals("HD") == true)
                {
                    SQL += "  AND O.BDate = TO_DATE('" + strBDATE + "','YYYY-MM-DD')    \r\n";
                    SQL += "  AND O.IpdOpd = 'O'  \r\n";
                    SQL += "  AND O.OrderNo > 2359  \r\n";
                }
                else if (FstrWard.Equals("EM") == true || FstrWard.Equals("ER") == true)
                {
                    SQL += "  AND O.BDate = TO_DATE('" + strBDATE + "','YYYY-MM-DD')  \r\n";
                    SQL += "  AND O.IpdOpd IN ( 'O','I')                              \r\n";
                    SQL += "  AND O.WARD = 'ER'                                       \r\n";
                    SQL += "  AND O.OrderNo > 2359                                    \r\n";
                }

                else if (FstrWard.Equals("OP") == true)
                {
                    SQL += "  AND O.BDate = TO_DATE('" + strBDATE + "','YYYY-MM-DD')   \r\n";
                    SQL += "  AND O.IpdOpd = 'I'                                       \r\n";
                    SQL += "  AND O.PANO IN ( SELECT PANO FROM KOSMOS_PMPA.ORAN_MASTER \r\n";
                    SQL += "                 WHERE OPDATE = TO_DATE('" + strBDATE + "','YYYY-MM-DD') )\r\n";
                }



                else if (FstrWard.Equals("XRAY") == true)
                {
                    SQL += "   AND O.BDate = TO_DATE('" + strBDATE + "','YYYY-MM-DD')    \r\n";
                    SQL += "   AND (  O.MASTERCODE IN ('XR17', 'YT05' ,'YT06','XR51','XR05','XR18') ) \r\n";
                    SQL += "   AND O.IpdOpd IN ('O', 'I') \r\n";
                }

                else
                {
                    SQL += "   AND O.BDate = TO_DATE('" + strBDATE + "','YYYY-MM-DD')   \r\n";
                    SQL += "   AND O.IpdOpd = 'I'                                       \r\n";
                    SQL += "   AND O.MASTERCODE NOT IN ('YT05','XR17','XR51')           \r\n";
                }               

            }

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return ds;
        }

        public int sel_EXAM_ORDER_RECIP_CHK(PsmhDb pDbCon, string strROWID)
        {
            DataTable dt = null;

            int nRow = -1;

            SQL = "";
            SQL += "  SELECT                                                            \r\n";
            SQL += "         COUNT(*) AS CNT                                            \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_ORDER 	A                                   \r\n";
            SQL += "   WHERE 1=1                                                        \r\n";
            SQL += "     AND ROWID = " + ComFunc.covSqlstr(strROWID, false);
            //SQL += "     AND A.PANO         = " + ComFunc.covSqlstr(strPANO, false);
            //SQL += "     AND A.BDATE        = " + ComFunc.covSqlstr(strBDATE, false);
            //SQL += "     AND A.IPDOPD       = " + ComFunc.covSqlstr(IPDOPD, false);
            //SQL += "     AND A.DEPTCODE     = " + ComFunc.covSqlstr(strDEPTCODE, false);
            //SQL += "     AND A.MASTERCODE   IN ( " + strMASTERCODE + ")                 \r\n";
            SQL += "     AND A.SPECNO       IS NOT NULL ";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return -1;
                }


                nRow = Convert.ToInt32(dt.Rows[0][0].ToString());
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return -1;
            }

            return nRow;
        }

        public string up_EXAM_ORDER_RDATE(PsmhDb pDbCon, string strRDATE, string strROWID_R, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_ORDER                                \r\n";
            SQL += "    SET RDATE = " + ComFunc.covSqlDate(strRDATE, false);
            SQL += "      , PART2 = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += "      , GDATE = SYSDATE                                      \r\n";
            SQL += "      , UPDT  = SYSDATE                                      \r\n";
            SQL += "      , UPPS  = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += " WHERE 1=1                                                   \r\n";
            SQL += "   AND ROWID  = " + ComFunc.covSqlstr(strROWID_R, false);
            
            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }

            return SqlErr;
        }

        public DataSet sel_EXAM_SPECMST_JEPSU(PsmhDb pDbCon, string strFDATE)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                    \r\n";
            SQL += "  		  IPDOPD                                                            \r\n";
            SQL += "  		, SPECNO                                                            \r\n";
            SQL += "  		, PANO                                                              \r\n";
            SQL += "  		, BI                                                                \r\n";
            SQL += "  		, SNAME                                                             \r\n";
            SQL += "  		, SEX                                                               \r\n";
            SQL += "  		, AGE                                                               \r\n";            
            SQL += "  		, DEPTCODE                                                          \r\n";
            SQL += "  		, WARD                                                              \r\n";
            SQL += "  		, ROOM                                                              \r\n";
            SQL += "  		, KOSMOS_OCS.FC_EXAM_RESULTC_EXAMNAME(SPECNO)		AS MASTERNM     \r\n";
            SQL += "  		, JEPSUNAME                                                         \r\n";
            SQL += "  		, JEPSUNAME2                                                        \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_SPECMST                                            \r\n";
            SQL += "   WHERE 1=1                                                                \r\n";
            SQL += "     AND BDATE = " + ComFunc.covSqlDate(strFDATE, false);
            SQL += "     AND JEPSUSABUN IS NOT NULL                                             \r\n";
            SQL += "  ORDER BY PANO                                                             \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return ds;
        }

        public DataSet sel_EXAM_ORDER(PsmhDb pDbCon, string strFDATE, string strTDATE, string strPANO, string strSNAME, string strDEPT, string strWARD, string strROOM
                , enmSel_EXAM_ORDER_STATUS pSTATUS
                , enmSel_EXAM_ORDER_PT_STATUS pSTATUS_PT 
                , int nIndex)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "   SELECT                                                                                                               \r\n";

            //SQL += "             KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(A.PANO, A.BDATE)                  AS INFECT_INFO      -- 02           \r\n";
            SQL += "             KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(A.PANO, A.BDATE)               AS INFECT_INFO      -- 02           \r\n";
            SQL += "          ,  NULL                                                                   AS INFECT_IMAGE     -- 02           \r\n";
            SQL += "   		  ,  A.PANO                    										        AS PANO		        -- 01           \r\n";
            
            SQL += "   		  ,  MAX(A.SEX)                                                             AS SEX                              \r\n";
            SQL += "   		  ,  TO_CHAR(MAX(A.AGE))                                                    AS AGE                              \r\n";
            SQL += "   		  ,  KOSMOS_OCS.FC_EXAM_ORDER_DANGCHK(A.PANO, TO_DATE('" + strFDATE + "', 'YYYY-MM-DD')) AS DANG                \r\n";
            SQL += "   		  ,  A.SNAME																AS SNAME            -- 03           \r\n";
            SQL += "          ,  P.JUMIN1                                                      \r\n";
            SQL += "   		  ,  A.DEPTCODE															    AS DEPTCODE	        -- 04           \r\n";
            
            //2018.05.09.김홍록:응급실요청에의해...전실전동시 오류예상

            SQL += "   		  ,  CASE WHEN A.DEPTCODE = 'ER' THEN KOSMOS_OCS.FC_OPD_MASTER_ER_NUM(A.PANO,MAX(A.ORDERDATE),A.BDATE)  ELSE '' END   AS ROOM 	        -- 04           \r\n";

            SQL += "   		  ,	 (CASE WHEN A.DEPTCODE = 'TO' THEN TO_CHAR(MAX(A.ACTDATE), 'YYYY-MM-DD') ELSE TO_CHAR(A.BDATE     , 'YYYY-MM-DD') END) AS BDATE		    -- 05           \r\n";
            SQL += "   		  ,  TO_CHAR(MAX(A.RDATE), 'YYYY-MM-DD')  								    AS RDATE		    -- 06 06 예약   \r\n";
            SQL += "   		  ,  CASE WHEN MAX(RDATE) IS NOT NULL THEN '예약☎' ELSE '' END             AS RDATE_CHK	    -- 06 06 예약   \r\n";
            //SQL += "          ,  CASE WHEN KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(A.PANO, A.BDATE,A.DEPTCODE) IS NOT NULL THEN 'CP/'||KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(A.PANO, A.BDATE,A.DEPTCODE) ELSE '' END       AS ERP              --              \r\n";
            //2018-08-27 안정수, CP 표시부분 추가 
            SQL += "        ,  (SELECT MAX(ERPATIENT) FROM KOSMOS_PMPA.OPD_MASTER WHERE PANO = A.PANO AND DEPTCODE = 'ER' AND ACTDATE = TRUNC(SYSDATE))     AS ERP                          \r\n";
            SQL += "   		  ,  CASE WHEN KOSMOS_OCS.FC_OPD_MASTER_OLDMAN(A.PANO, TO_CHAR(A.BDATE,'YYYY-MM-DD')) ='Y' THEN '노인' ELSE '' END AS OLD_MAN  	    -- 06 06 예약   \r\n";
            SQL += "          ,  (                                                                                                          \r\n";
            SQL += "          		SELECT CASE WHEN SUM(QTY) > 0 THEN '미시행(60일기준)◆' ELSE '' END MQTY                                                \r\n";
            SQL += "    				  FROM KOSMOS_OCS.EXAM_ORDER                                                                        \r\n";
            SQL += "  				 WHERE PANO  = A.PANO                                                                                   \r\n";
            SQL += "  				   AND BDATE >= TO_DATE('" + strFDATE + "','YYYY-MM-DD') - 60                                           \r\n";
            SQL += "  				   AND BDATE <  TO_DATE('" + strFDATE + "','YYYY-MM-DD')                                                \r\n";
            SQL += "  				   AND IPDOPD = 'O'                                                                                     \r\n";
            SQL += "  				   AND (CANCEL IS NULL OR CANCEL = ' ')                                                                 \r\n";
            SQL += "  				   AND SPECNO IS NULL                                                                                   \r\n";
            SQL += "  			 )  						   										     AS NOT_ACT		-- 07 06 --         \r\n";
            SQL += "          ,  CASE WHEN MAX(P.JICODE) = '63' OR MAX(P.JICODE) >= '76'                                                    \r\n";
            SQL += "                  THEN	'원거리' ELSE '' END  									     AS WON      	-- 08 06 원거리     \r\n";

            SQL += "          ,  DECODE(MAX(A.STRT),'E','응급','')									     AS EM          -- 09               \r\n";
            SQL += "          ,  DECODE(KOSMOS_OCS.FC_IPD_NEW_MASTER_SECRET(A.PANO),'Y','사생활','') 	 AS SECRET 		-- 10 사생활        \r\n";
            SQL += "          ,  DECODE(KOSMOS_OCS.FC_NUR_FALL_REPORT_CHK(A.PANO,A.BDATE),'Y','낙상','') AS FALL		                    \r\n";
            SQL += "   		  ,  DECODE(SUM(DECODE(SUBSTR(A.MASTERCODE,1,2),'UR',1,0)),0,'','소변') 	 AS URINECNT                        \r\n";
            SQL += "   		  ,  MAX(IPDOPD)                                                             AS IPDOPD                          \r\n";
            SQL += "        ,  (SELECT MAX(REMIND) FROM KOSMOS_OCS.EXAM_PATIENT WHERE PANO = A.PANO)     AS REMIND                          \r\n";
            //2018-08-27 안정수 추가함
            SQL += "        ,  KOSMOS_OCS.FC_CP_RECORD_ER(A.PANO,A.BDATE)                                AS FC_CP_CHK                       \r\n";
            
            SQL += "     FROM KOSMOS_OCS.EXAM_ORDER   A                                                                                     \r\n";
            SQL += "         ,KOSMOS_PMPA.BAS_PATIENT P                                                                                     \r\n";
            //2019-12-16 안정수, OPD_MASTER 조인 추가
            if (strWARD == "ER")
            {
                SQL += "         ,KOSMOS_PMPA.OPD_MASTER  T                                                                                 \r\n";
            }
            SQL += "    WHERE 1=1                                                                                                           \r\n";
            //2019-12-16 안정수 추가
            SQL += "      AND A.PANO = P.PANO                                                                                               \r\n";
            if (strWARD == "ER")
            {
                SQL += "      AND A.PANO = T.PANO                                                                                           \r\n";
                //SQL += "      AND A.DEPTCODE = T.DEPTCODE                                                                                   \r\n";
                //SQL += "      AND A.ORDERDATE >= (SELECT MAX(JTIME) FROM KOSMOS_PMPA.OPD_MASTER WHERE PANO = A.PANO AND ER_NUM IS NOT NULL) \r\n";
                //SQL += "      AND T.ACTDATE = T.PANO                                                                                           \r\n";
                SQL += "      AND T.ACTDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false) + "-1";
                SQL += "                        AND " + ComFunc.covSqlDate(strTDATE, false);
            }

            if (strDEPT.Trim().Equals(clsParam.EXAM_DEPT_LIS) == true || strDEPT.Trim().Equals("OB") == true)
            {
                #region LIS

                if (pSTATUS_PT == enmSel_EXAM_ORDER_PT_STATUS.OPT || pSTATUS_PT == enmSel_EXAM_ORDER_PT_STATUS.OG || pSTATUS_PT == enmSel_EXAM_ORDER_PT_STATUS.DANG)
                {
                    SQL += "      AND A.IPDOPD = 'O'                                                                                            \r\n";
                    SQL += "      AND A.SDATE IS NULL                                                                                            \r\n";
                    SQL += "      AND (A.CANCEL IS NULL OR A.CANCEL = ' ')                                                                      \r\n";
                    SQL += "      AND A.ORDERNO >= 2400		-- 원무과에서 발생하는 처방은 제외                                                  \r\n";


                    //2018.04.17.김홍록:산부인과 외래 제외 코드(현재는 산부인과만 됨)

                    if (pSTATUS_PT == enmSel_EXAM_ORDER_PT_STATUS.OPT)
                    {
                        SQL += "      AND CASE WHEN A.DEPTCODE = 'OG' AND A.MASTERCODE IN (	SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE P WHERE GUBUN = 'EXAM_외래제외코드') THEN 'N' ELSE 'Y' END = 'Y'        \r\n";
                        SQL += "      AND ( ((A.MASTERCODE = 'MI3URO') AND (A.SPECCODE <> '094A')) OR ((A.MASTERCODE <> 'MI3URO') AND (1=1)) )        \r\n";
                        SQL += "      AND ( ((A.MASTERCODE = 'MI03') AND (A.SPECCODE <> '094A')) OR ((A.MASTERCODE <> 'MI03') AND (1=1)) )        \r\n";
                        SQL += "      AND ( ((A.MASTERCODE = 'MI04') AND (A.SPECCODE <> '094A')) OR ((A.MASTERCODE <> 'MI04') AND (1=1)) )        \r\n";
                        SQL += "      AND ( ((A.MASTERCODE = 'MI05') AND (A.SPECCODE <> '094A')) OR ((A.MASTERCODE <> 'MI05') AND (1=1)) )        \r\n";
                    }
                    else if (pSTATUS_PT == enmSel_EXAM_ORDER_PT_STATUS.OG)
                    {
                        SQL += "      AND (A.MASTERCODE IN (SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE A WHERE GUBUN = 'EXAM_외래제외코드') OR (A.MASTERCODE IN ('MI3URO','MI03','MI04','MI05') AND A.SPECCODE = '094A'))         \r\n";
                        SQL += "      AND A.DEPTCODE   = 'OG'         \r\n";
                    }
                    else if (pSTATUS_PT == enmSel_EXAM_ORDER_PT_STATUS.DANG)
                    {
                        SQL += "      AND CASE WHEN A.DEPTCODE = 'OG' AND A.MASTERCODE IN (	SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE P WHERE GUBUN = 'EXAM_외래제외코드') THEN 'N' ELSE 'Y' END = 'Y'        \r\n";
                        SQL += "      AND A.MASTERCODE IN ('CR59','CR59B')         \r\n";
                    }

                    if (string.IsNullOrEmpty(strPANO) == false)
                    {
                        SQL += "      AND A.PANO = " + ComFunc.covSqlstr(strPANO, false);
                    }
                    else
                    {
                        SQL += "      AND (                                                                                                     \r\n";
                        SQL += "      		(                                                                                                   \r\n";
                        SQL += "      				A.BDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                        SQL += "      		                    AND " + ComFunc.covSqlDate(strTDATE, false);
                        SQL += "      			AND A.RDATE IS NULL                                                                             \r\n";
                        SQL += "      		)                                                                                                   \r\n";
                        SQL += "      		OR                                                                                                  \r\n";
                        SQL += "      		(                                                                                                   \r\n";
                        SQL += "      				A.RDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                        SQL += "      				            AND " + ComFunc.covSqlDate(strTDATE, false);
                        SQL += "      		)                                                                                                   \r\n";
                        SQL += "      	)                                                                                                       \r\n";

                        if (string.IsNullOrEmpty(strSNAME.Trim()) == false)
                        {
                            SQL += "      AND A.SNAME LIKE '" + strSNAME + "%'";
                        }
                    }

                    if (pSTATUS == enmSel_EXAM_ORDER_STATUS.READY)
                    {
                        SQL += "       AND A.SPECNO IS NULL                                                                                     \r\n";
                    }
                    else if (pSTATUS == enmSel_EXAM_ORDER_STATUS.RECIPT)
                    {
                        SQL += "       AND A.SPECNO IS NOT NULL                                                                                 \r\n";
                    }


                    if (pSTATUS == enmSel_EXAM_ORDER_STATUS.READY)
                    {
                        SQL += "                                                                                                                    \r\n";
                        SQL += "     GROUP BY A.BDATE, A.PANO, A.SNAME, A.DEPTCODE, TO_CHAR(A.RDATE, 'YYYY-MM-DD'), P.JUMIN1                                   \r\n";

                        SQL += "     ORDER BY A.SNAME,A.PANO,A.BDATE                                                                            \r\n";
                    }
                    else if (pSTATUS == enmSel_EXAM_ORDER_STATUS.RECIPT)
                    {
                        SQL += "                                                                                                                    \r\n";
                        SQL += "     GROUP BY A.BDATE, A.PANO, A.SNAME, A.DEPTCODE, TO_CHAR(A.RDATE, 'YYYY-MM-DD'), P.JUMIN1                         \r\n";
                        SQL += "     ORDER BY A.SNAME,A.PANO,A.BDATE                                                                             \r\n";
                    }
                }
                else if (pSTATUS_PT == enmSel_EXAM_ORDER_PT_STATUS.IPT)
                {
                    if (string.IsNullOrEmpty(strPANO) == false)
                    {
                        SQL += "      AND A.PANO = " + ComFunc.covSqlstr(strPANO, false);
                    }

                    if (string.IsNullOrEmpty(strSNAME.Trim()) == false)
                    {
                        SQL += "      AND A.SNAME LIKE '" + strSNAME + "%'";
                    }

                    SQL += "     AND A.BDATE    = TRUNC(SYSDATE)                                                                                \r\n";
                    SQL += "     AND A.IPDOPD   = 'I'                                                                                           \r\n";
                    SQL += "     AND (A.CANCEL IS NULL OR A.CANCEL = ' ')                                                                       \r\n";
                    SQL += "     AND A.PANO IN (                                                                                                \r\n";
                    SQL += "                      SELECT PANO                                                                                   \r\n";
                    SQL += "                        FROM KOSMOS_PMPA.IPD_NEW_MASTER                                                             \r\n";
                    SQL += "                       WHERE INDATE >= TRUNC(SYSDATE) - 1                                                           \r\n";
                    SQL += "                         AND IPWONTIME >= TRUNC(SYSDATE)                                                            \r\n";
                    SQL += "                         AND IPWONTIME <  TRUNC(SYSDATE) +1                                                         \r\n";
                    SQL += "                         AND AMSET4 <> '3'                                                                         \r\n";
                    SQL += "                    )                                                                                               \r\n";

                    if (pSTATUS == enmSel_EXAM_ORDER_STATUS.READY)
                    {
                        SQL += "       AND A.SPECNO IS NULL                                                                                     \r\n";
                    }
                    else if (pSTATUS == enmSel_EXAM_ORDER_STATUS.RECIPT)
                    {
                        SQL += "       AND A.SPECNO IS NOT NULL                                                                                 \r\n";
                    }

                    SQL += "   GROUP BY A.BDATE, A.PANO, A.SNAME, A.DEPTCODE, TO_CHAR(A.RDATE, 'YYYY-MM-DD'), P.JUMIN1                                     \r\n";
                    SQL += "   ORDER BY A.SNAME,A.PANO,A.BDATE                                                                                  \r\n";
                }
                else if (pSTATUS_PT == enmSel_EXAM_ORDER_PT_STATUS.CONSULT)
                {
                    SQL += "   AND A.BDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                    SQL += "                   AND " + ComFunc.covSqlDate(strTDATE, false);
                    SQL += "   AND A.IPDOPD = 'I'                                                                                              \r\n";
                    SQL += "   AND (A.CANCEL IS NULL OR A.CANCEL = ' ')                                                                        \r\n";
                    SQL += "   AND A.PANO IN (  SELECT PTNO                                                                                    \r\n";
                    SQL += "                      FROM KOSMOS_OCS.OCS_ITRANSFER                                                                \r\n";
                    SQL += "                     WHERE EDATE >= TO_DATE('" + strFDATE + "','YYYY-MM-DD')                                       \r\n";
                    SQL += "                       AND EDATE <  TO_DATE('" + strFDATE + "','YYYY-MM-DD') + 1                                   \r\n";
                    SQL += "                       AND GBCONFIRM ='*'                                                                          \r\n";
                    SQL += "                       AND GBFLAG ='1'                                                                             \r\n";
                    SQL += "                       AND GBDEL <>'*'                                                                             \r\n";
                    SQL += "                 )                                                                                                 \r\n";

                    if (pSTATUS == enmSel_EXAM_ORDER_STATUS.READY)
                    {
                        SQL += "       AND A.SPECNO IS NULL                                                                                     \r\n";
                    }
                    else if (pSTATUS == enmSel_EXAM_ORDER_STATUS.RECIPT)
                    {
                        SQL += "       AND A.SPECNO IS NOT NULL                                                                                 \r\n";
                    }

                    if (string.IsNullOrEmpty(strPANO) == false)
                    {
                        SQL += "      AND A.PANO = " + ComFunc.covSqlstr(strPANO, false);
                    }

                    if (string.IsNullOrEmpty(strSNAME.Trim()) == false)
                    {
                        SQL += "      AND A.SNAME LIKE '" + strSNAME + "%'";
                    }

                    SQL += "   GROUP BY A.BDATE, A.PANO, A.SNAME, A.DEPTCODE, TO_CHAR(A.RDATE, 'YYYY-MM-DD'), P.JUMIN1                                     \r\n";
                    SQL += "   ORDER BY A.SNAME,A.PANO,A.BDATE                                                                                  \r\n";
                }

                #endregion  
            }
            else 
            {

                if (string.IsNullOrEmpty(strPANO) == false)
                {
                    SQL += "      AND A.PANO = " + ComFunc.covSqlstr(strPANO, false);
                }

                if (string.IsNullOrEmpty(strSNAME.Trim()) == false)
                {
                    SQL += "      AND A.SNAME LIKE '" + strSNAME + "%'";
                }

                SQL += "  AND (A.CANCEL IS NULL OR A.CANCEL = ' ')  \r\n";

                if (strWARD.Trim().Equals(clsParam.EXAM_DEPT_ENDO) == true || strWARD.Trim().Equals("ENTO") == true)
                {
                    SQL += "  AND A.ACTDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                    SQL += "                    AND " + ComFunc.covSqlDate(strTDATE, false);
                }
                else
                {
                    SQL += "  AND A.BDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                    SQL += "                  AND " + ComFunc.covSqlDate(strTDATE, false);

                }

                //2019-12-16 안정수 추가
                if(nIndex > 0 && strWARD == "ER")
                {
                    switch (nIndex)
                    {
                        case 1: //관찰
                            SQL += "       AND T.ER_NUM IN (" + GstrArea1 + ")                                                              \r\n";
                            break;
                        case 2: //중증
                            SQL += "       AND T.ER_NUM IN (" + GstrArea2 + ")                                                              \r\n";
                            break;
                        case 3: //소아
                            SQL += "       AND T.ER_NUM IN (" + GstrArea3 + ")                                                              \r\n";
                            break;
                        case 4: //격리
                            SQL += "       AND T.ER_NUM IN (" + GstrArea4 + ")                                                              \r\n";
                            break;
                    }
                    ///SQL += "               AND A.ORDERDATE >= T.JTIME                                                                       \r\n";
                }

                if (pSTATUS == enmSel_EXAM_ORDER_STATUS.READY)
                {
                    SQL += "       AND A.SPECNO IS NULL                                                                                     \r\n";
                }
                else if (pSTATUS == enmSel_EXAM_ORDER_STATUS.RECIPT)
                {
                    SQL += "       AND A.SPECNO IS NOT NULL                                                                                 \r\n";
                }

                if (strWARD.Equals("HD") == true)
                {
                    SQL += "  AND A.IPDOPD = 'O'                      \r\n";
                    SQL += "  AND A.DEPTCODE = 'HD'                   \r\n";
                    SQL += "  AND A.ORDERNO > 2359                    \r\n";
                }
                else if (strWARD.Equals("EM") == true || strWARD.Equals("ER") == true)
                {
                    SQL += "  AND A.WARD ='ER'                        \r\n";
                    SQL += "  AND A.IpdOpd IN ( 'O','I')              \r\n";
                    SQL += "  AND A.OrderNo > 2359                    \r\n";
                }
                else if (strWARD.Equals("OP") == true )
                {
                    SQL += "   AND A.PANO IN ( SELECT PANO FROM KOSMOS_PMPA.ORAN_MASTER                             \r\n";
                    SQL += "                  WHERE OPDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                    SQL += "                                   AND " + ComFunc.covSqlDate(strTDATE, false);
                    SQL += "                 )                                                                      \r\n";
                    SQL += "  AND A.IPDOPD = 'I'                                                                    \r\n";
                    SQL += "  AND A.PANO IN (SELECT PANO FROM KOSMOS_PMPA.IPD_NEW_MASTER                            \r\n";
                    SQL += "     WHERE GBSTS IN ('0', '2', '3', '4')                                                \r\n";
                    SQL += "       AND ACTDATE IS NULL                                                              \r\n";

                    if (string.IsNullOrEmpty(strROOM) == false)
                    {
                        SQL += "       AND ROOMCODE  = " + ComFunc.covSqlstr(strROOM, false);
                    }

                    SQL += "                 )                                                                      \r\n";
                }
                else if (strWARD.Equals("ENDO") == true)
                {
                    SQL += "  AND A.PANO IN ( SELECT PTNO FROM KOSMOS_OCS.ENDO_JUPMST                               \r\n";
                    SQL += "                  WHERE RDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                    SQL += "                                  AND " + ComFunc.covSqlDate(strTDATE, false) + " + 1)  \r\n";
                    SQL += "  AND A.DEPTCODE NOT IN ('TO','HR')                                                       \r\n";
                    //2018-08-21 안정수, 내시경실 요청으로 GP22A 추가
                    //2019-06-11 안정수, O-XIH07 추가 
                    SQL += "  AND ( A.MASTERCODE LIKE 'XR%' OR A.MASTERCODE IN ('MI04','MI11','MI03','MI361','MI321','GP11','MI05','MI36', 'MI34','MI37','GP15','MI364','XS03','GP22A','O-XIH07') )  \r\n";
                }

                else if (strWARD.Equals("ENTO") == true)
                {
                    SQL += "   AND A.PANO IN ( SELECT PTNO FROM KOSMOS_OCS.ENDO_JUPMST                            \r\n";
                    SQL += "                  WHERE RDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                    SQL += "                                  AND " + ComFunc.covSqlDate(strTDATE, false) + "+1) \r\n";
                    SQL += "   AND A.DEPTCODE  IN ('TO')                                         \r\n";
                    SQL += "   AND ( A.MASTERCODE LIKE 'XR%' OR A.MASTERCODE IN ('MI04','MI11','MI03','MI361','MI321','GP11','MI05','MI36', 'MI34','MI37','GP15','MI364','XS03') ) \r\n";
                }

                else if (strWARD.Equals("ENHR") == true)
                {
                    SQL += "   AND A.PANO IN ( SELECT PTNO FROM KOSMOS_OCS.ENDO_JUPMST                            \r\n";
                    SQL += "                  WHERE RDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
                    SQL += "                                  AND " + ComFunc.covSqlDate(strTDATE, false) + "+1 ) \r\n";
                    SQL += "   AND A.DEPTCODE  IN ('HR')                                         \r\n";
                    SQL += "   AND ( A.MASTERCODE LIKE 'XR%' OR A.MASTERCODE IN ('MI04','MI11','MI03','MI361','MI321','GP11','MI05','MI36', 'MI34','MI37','GP15','MI364','XS03') ) \r\n";

                }
                else if (strWARD.Equals("XRAY") == true)
                {
                    SQL += "   AND (MASTERCODE IN ('XR17','YT05','YT06','XR51','XR05','XR18') )                         \r\n";
                }
                else
                {
                    SQL += " AND A.IPDOPD = 'I'                                                                  \r\n";
                    SQL += " AND A.PANO IN ( SELECT PANO FROM KOSMOS_PMPA.IPD_NEW_MASTER                         \r\n";
                    SQL += "     WHERE GBSTS IN ('0', '2', '3', '4')                                             \r\n";
                    SQL += "       AND ACTDATE IS NULL                                                           \r\n";

                    if (string.IsNullOrEmpty(strROOM) == false)
                    {
                        if (string.IsNullOrEmpty(strROOM) == false)
                        {
                            SQL += "       AND ROOMCODE  = " + ComFunc.covSqlstr(strROOM, false);
                        }
                    }

                    if (string.IsNullOrEmpty(strWARD.Trim()) == false && strWARD.Equals("*") == false)
                    {
                        if (strWARD.Equals("NR"))
                        {
                            SQL += "    AND WARDCODE IN ('NR','IQ') ) \r\n";
                        }
                        else
                        {
                            SQL += "    AND WARDCODE = " + ComFunc.covSqlstr(strWARD, false) + ")   \r\n";
                        }
                    }
                    else
                    {
                        SQL += "       )                                                                        \r\n";
                    }

                    SQL += "    AND  MASTERCODE NOT IN ('YT05','XR17','XR51')                                   \r\n";
                }

                SQL += "  GROUP BY A.BDATE, A.PANO, A.SNAME, A.DEPTCODE, A.ACTDATE,P.JUMIN1                         \r\n";

                if (strWARD.Equals("ER") == true)
                {
                    SQL += "  ORDER BY A.BDATE DESC,KOSMOS_OCS.FC_OPD_MASTER_ER_NUM(A.PANO,MAX(A.ORDERDATE),A.BDATE), A.SNAME     \r\n";
                }
                else
                {
                    SQL += "  ORDER BY A.BDATE DESC,A.SNAME                                                     \r\n";
                }

            }

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return ds;
        }

        public DataTable sel_EXAM_RESULTC_HR10(PsmhDb pDbCon, string strSPECNO)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "  SELECT                                                            \r\n";
            SQL += "         A.MASTERCODE  AS MASTERCODE                                \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_RESULTC A                                  \r\n";
            SQL += "   WHERE 1=1                                                        \r\n";
            SQL += "     AND A.SPECNO       = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "     AND A.MASTERCODE   = 'HR10'                                    \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataSet sel_EXAM_SPECMST_WORKLIST(PsmhDb pDbCon, string strSPECNO, string strALL_CD, string strALLEQU_CD)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                            \r\n";
            SQL += "  		  A.WORKSTS                                                                 \r\n";
            SQL += "  		, A.SPECNO	                                            AS BARCODE_NM       \r\n";
            SQL += "  		, A.SPECNO                                                                  \r\n";
            SQL += "  		, A.PANO                                                                    \r\n";
            SQL += "  		, ''		                                            AS EXAM_NUM         \r\n";
            //SQL += "  		, A.SNAME                                                                   \r\n";
            //2019-10-01 안정수, 이미경J 요청으로 환경검사 건은 부서명으로 표기되도록 보완
            SQL += "  		, CASE WHEN A.PANO = '81000014' OR A.PANO = '11077917' THEN KOSMOS_OCS.FC_BAS_BUSE_NAME((SELECT BUCODE \r\n";
            SQL += "  		                                                                                        FROM KOSMOS_PMPA.ENVIRONMENT_ORDER \r\n";
            SQL += "  		                                                                                        WHERE BARCODE = A.SPECNO))         \r\n";
            SQL += "  		  ELSE A.SNAME END 															AS SNAME		-- 04                          \r\n";
            SQL += "  		, A.SEX                                                                     \r\n";
            SQL += "  		, A.AGE                                                                     \r\n";
            SQL += "  		, A.WARD                                                                    \r\n";
            SQL += "  		, A.DEPTCODE                                                                \r\n";
            SQL += "  		, KOSMOS_OCS.FC_EXAM_SPECMST_NM('14', A.SPECCODE, 'Y')	AS SPECCODE_NM      \r\n";
             
            if (string.IsNullOrEmpty(strALL_CD.Trim()) == false)
            {
                SQL += "  		, KOSMOS_OCS.FC_EXAM_RESULTC_EXAMNAME_CD(A.SPECNO  ,'" + strALL_CD.Replace("'","") + "')		AS EXAM_NAME        \r\n";
            }
            else if (string.IsNullOrEmpty(strALL_CD.Trim()) == true && string.IsNullOrEmpty(strALL_CD.Trim()) == false)
            {
                SQL += "  		, KOSMOS_OCS.FC_EXAM_RESULTC_EXAMNAME_EQU(A.SPECNO ,'" + strALLEQU_CD.Replace("'", "") + "')	AS EXAM_NAME        \r\n";
            }
            else
            {
                SQL += "  		, KOSMOS_OCS.FC_EXAM_RESULTC_EXAMNAME(A.SPECNO)			                                    AS EXAM_NAME        \r\n";
            }
            //2018-11-07 안정수, 양선문 과장 요청으로 추가함 
            SQL += "        , (SELECT MAX(RESULT)                                                       \r\n";
            SQL += "                FROM KOSMOS_OCS.EXAM_RESULTC                                        \r\n";
            SQL += "                WHERE 1=1                                                           \r\n";
            SQL += "                    AND SPECNO = A.SPECNO                                           \r\n";
            SQL += "                    AND SUBCODE IN ('SA041', 'SA011')                               \r\n";
            SQL += "          )                                                     AS RESULT           \r\n";            
            SQL += "  		, ''                                        			AS REMARK           \r\n";
            SQL += "  		, ''                                        			AS REMARK2          \r\n";
            SQL += "  		, A.IPDOPD                                                                  \r\n";
            SQL += "  		, TO_CHAR(A.BDATE,'YYYY-MM-DD')                         AS BDATE            \r\n";
            SQL += "     FROM KOSMOS_OCS.EXAM_SPECMST A                                                 \r\n";
            SQL += "    WHERE 1=1                                                                       \r\n";
            SQL += "      AND SPECNO   = " + ComFunc.covSqlstr(strSPECNO.Trim(), false);

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return ds;
        }

        public string up_EXAM_ORDER(PsmhDb pDbCon, string strSPECNO, string strPANO, string strBDATE, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_ORDER                                \r\n";
            SQL += "    SET SPECNO     = REPLACE(SPECNO,'" + strSPECNO + "','')  \r\n";
            SQL += " WHERE SPECNO LIKE '%" + strSPECNO + "%'                     \r\n";
            SQL += "   AND PANO   = " + ComFunc.covSqlstr(strPANO, false);
            SQL += "   AND BDATE  = " + ComFunc.covSqlDate(strBDATE, false);

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }

            return SqlErr;
        }

        public string up_EXAM_SPECMST_STATUS_CANCEL(PsmhDb pDbCon, string strSPECNO, string strCANCEL, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_SPECMST                                                                                 \r\n";
            SQL += "    SET STATUS     = '06'                                                                                       \r\n";
            SQL += "      , CANCEL     = " + ComFunc.covSqlstr(strCANCEL, false);
            SQL += "      , EMR        = 0                                                                                          \r\n";
            SQL += "      , RESULTDATE = SYSDATE                                                                                    \r\n";
            SQL += "      , ANATNO     = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += " 	  , UPPS = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += " 	  , UPDT = SYSDATE                                                                                          \r\n";
            SQL += " WHERE SPECNO = '" + strSPECNO + "'                                                                             \r\n";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }

            return SqlErr;
        }

        public string ins_EXAM_CANCEL_LOG(PsmhDb pDbCon, string strSPECNO, string strPANO, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " INSERT INTO KOSMOS_OCS.EXAM_CANCEL (PTNO, SPECNO, CDATE, CSABUN)     \r\n";
            SQL += "    VALUES (                                                                        \r\n";
            SQL += "    " + ComFunc.covSqlstr(strPANO, false);
            SQL += "    " + ComFunc.covSqlstr(strSPECNO, true);
            SQL += "    , SYSDATE                                                                       \r\n";
            SQL += "    " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += " 	)                                                                               \r\n";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }

            return SqlErr;
        }

        public string ins_EXAM_JCANCEL_LOG(PsmhDb pDbCon, string strSPECNO, string strPANO, string strSAYU, string strRSABUN, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " INSERT INTO KOSMOS_OCS.EXAM_JCANCEL_LOG (SPECNO,PANO,SAYU,CTIME,CSABUN,RSABUN)     \r\n";
            SQL += "    VALUES (                                                                        \r\n";
            SQL += "    " + ComFunc.covSqlstr(strSPECNO , false);
            SQL += "    " + ComFunc.covSqlstr(strPANO   , true);
            SQL += "    " + ComFunc.covSqlstr(strSAYU   , true);
            SQL += "    , SYSDATE                                                                       \r\n";
            SQL += "    " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "    " + ComFunc.covSqlstr(strRSABUN , true);
            SQL += " 	)                                                                               \r\n";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }

            return SqlErr;
        }

        public string up_EXAM_SPECMST_STATUS(PsmhDb pDbCon, string strSPECNO, ref int intRowAffected,ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_SPECMST     \r\n";
            SQL += "    SET STATUS      = '00'          \r\n";
            SQL += "      , PRINT       = 0             \r\n";
            SQL += "      , EMR         = 0             \r\n";
            SQL += "      , RESULTDATE  = ''            \r\n";
            SQL += "      , RECEIVEDATE = ''            \r\n";
            SQL += " 	  , UPPS        = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += " 	  , UPDT        = SYSDATE                                                                                          \r\n";
            SQL += " WHERE SPECNO       = " + ComFunc.covSqlstr(strSPECNO, false);

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }

            return SqlErr;
        }
       
        public DataSet sel_EXAM_ERR_MSG(PsmhDb pDbCon, string strRECEIVEDATE)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                                             \r\n";
            SQL += "      A.ACTDATE, A.SPECNO, A.MSG, A.ROWID                     \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_ERR_MSG A                                                    \r\n";
            SQL += " WHERE 1=1                                                                          \r\n";
            SQL += "   AND ACTDATE  = " + ComFunc.covSqlDate(strRECEIVEDATE, false);

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return ds;
        }

        public DataSet sel_EXAM_SPECMST_RCP02(PsmhDb pDbCon, string strRECEIVEDATE, string strJUPSU, string strSPECNO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                                                                 \r\n";
            SQL += "      A.IPDOPD                                                                                          \r\n";
            SQL += "    , TO_CHAR(RECEIVEDATE,'YYYY-MM-DD HH24:MI')                         AS RECEIVEDATE                  \r\n";
            SQL += " 	, KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(A.PANO,A.BDATE)              AS INFECT_INFO                  \r\n";
            SQL += " 	, null     												            AS INFECT_IMAGE                 \r\n";
            //SQL += " 	, KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(A.PANO, A.BDATE, A.DEPTCODE)   AS ERP                          \r\n";
            SQL += "    ,  (SELECT MAX(ERPATIENT) FROM KOSMOS_PMPA.OPD_MASTER WHERE PANO = A.PANO AND DEPTCODE = 'ER' AND ACTDATE = TRUNC(SYSDATE))     AS ERP                          \r\n";
            SQL += "    , TO_CHAR(A.WARD)	                                                AS WARD                         \r\n";
            SQL += " 	, TO_CHAR(A.ROOM)	                                                AS ROOM                         \r\n";
            SQL += " 	, A.SPECNO                                                                                          \r\n";
            SQL += " 	, A.PANO                                                                                            \r\n";
            SQL += " 	, A.SNAME                                                                                           \r\n";
            SQL += " 	, A.SEX                                                                                             \r\n";
            SQL += " 	, A.AGE                                                                                             \r\n";
            SQL += " 	, A.JEPSUNAME                                                                                       \r\n";
            SQL += " 	, A.JEPSUNAME2                                                                                      \r\n";
            SQL += " 	, KOSMOS_OCS.FC_EXAM_RESULTC_EXAMNAME_NONE(A.SPECNO)			AS EXAM_NAME                        \r\n";
            //SQL += " 	, DECODE(IPDOPD,'I',KOSMOS_OCS.FC_OCS_IORDER_CANCEL(A.ORDERNO, A.PANO),'') AS IOCS_CANCEL           \r\n";
            SQL += " 	, (SELECT CASE WHEN SUM(QTY * NAL) <= 0 THEN 'Y'        \r\n";
            SQL += " 	              ELSE '' END                                                                           \r\n";
            SQL += " 	   FROM KOSMOS_OCS.OCS_IORDER                                                                       \r\n";
            SQL += " 	   WHERE ORDERNO = A.ORDERNO                                                                        \r\n";
            SQL += " 	    AND PTNO = A.PANO) AS IOCS_CANCEL                                                               \r\n";
            SQL += " 	, STATUS                                                                                            \r\n";
            SQL += " 	, TO_CHAR(BDATE,'YYYY-MM-DD')  BDATE                                                                \r\n";
            SQL += " 	, DEPTCODE                                                                                          \r\n";
            //2019-08-30 안정수 추가
            SQL += "    ,  KOSMOS_OCS.FC_CP_RECORD_ER(A.PANO,A.BDATE)                                AS FC_CP_CHK           \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_SPECMST A                                                                        \r\n";
            SQL += " WHERE 1=1                                                                                              \r\n";
            SQL += "   AND 'J'      = " + ComFunc.covSqlstr(strJUPSU, false);
            SQL += "   AND A.SPECNO = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += " UNION ALL                                                                                              \r\n";
            SQL += " SELECT                                                                                                 \r\n";
            SQL += "      A.IPDOPD                                                                                          \r\n";
            SQL += "    , TO_CHAR(RECEIVEDATE,'YYYY-MM-DD HH24:MI') AS RECEIVEDATE                                          \r\n";
            SQL += " 	, KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(A.PANO,A.BDATE)  AS INFECT_INFO                              \r\n";
            SQL += " 	, null     												AS INFECT_IMAGE                             \r\n";
            //SQL += " 	, KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(A.PANO, A.BDATE, A.DEPTCODE)   AS ERP                          \r\n";
            SQL += "    ,  (SELECT MAX(ERPATIENT) FROM KOSMOS_PMPA.OPD_MASTER WHERE PANO = A.PANO AND DEPTCODE = 'ER' AND ACTDATE = TRUNC(SYSDATE))     AS ERP                          \r\n";
            SQL += "    , TO_CHAR(A.WARD)	AS WARD                                                                         \r\n";
            SQL += " 	, TO_CHAR(A.ROOM)	AS ROOM                                                                         \r\n";
            SQL += " 	, A.SPECNO                                                                                          \r\n";
            SQL += " 	, A.PANO                                                                                            \r\n";
            SQL += " 	, A.SNAME                                                                                           \r\n";
            SQL += " 	, A.SEX                                                                                             \r\n";
            SQL += " 	, A.AGE                                                                                             \r\n";            
            SQL += " 	, A.JEPSUNAME                                                                                       \r\n";
            SQL += " 	, A.JEPSUNAME2                                                                                      \r\n";
            SQL += " 	, KOSMOS_OCS.FC_EXAM_RESULTC_EXAMNAME(A.SPECNO)			AS EXAM_NAME                                \r\n";
            //SQL += " 	, DECODE(IPDOPD,'I',KOSMOS_OCS.FC_OCS_IORDER_CANCEL(A.ORDERNO, A.PANO),'') AS IOCS_CANCEL           \r\n";
            SQL += " 	, (SELECT CASE WHEN SUM(QTY * NAL) <= 0 THEN 'Y'        \r\n";
            SQL += " 	               ELSE '' END                                                                          \r\n";
            SQL += " 	    FROM KOSMOS_OCS.OCS_IORDER                                                                      \r\n";
            SQL += " 	    WHERE ORDERNO = A.ORDERNO                                                                       \r\n";
            SQL += " 	     AND PTNO = A.PANO) AS IOCS_CANCEL                                                              \r\n";
            SQL += " 	, STATUS                                                                                            \r\n";
            SQL += " 	, TO_CHAR(BDATE,'YYYY-MM-DD')  BDATE                                                                \r\n";
            SQL += " 	, DEPTCODE                                                                                          \r\n";
            //2019-08-30 안정수 추가
            SQL += "    ,  KOSMOS_OCS.FC_CP_RECORD_ER(A.PANO,A.BDATE)                                AS FC_CP_CHK           \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_SPECMST A                                                                        \r\n";
            SQL += " WHERE 1=1                                                                                              \r\n";
            SQL += "   AND 'J' != " + ComFunc.covSqlstr(strJUPSU, false);

            if (string.IsNullOrEmpty(strRECEIVEDATE) == false)
            {
                SQL += "   AND RECEIVEDATE 	BETWEEN " + ComFunc.covSqlDate(strRECEIVEDATE, false);
                SQL += "                        AND TO_DATE('" + strRECEIVEDATE + "', 'YYYY-MM-DD') + 1     \r\n";
            }
            SQL += "   AND (IPDOPD 		= 'I' OR BI	IN ('61','62') OR DEPTCODE IN ('ER'))                \r\n";
            //SQL += "   AND STATUS       = '01'                                                          \r\n";            
            SQL += " ORDER BY WARD                                                                      \r\n";
            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return ds;
        }

        public string ins_EXAM_ERR_MSGT(PsmhDb pDbCon, string strSPECNO, string strMSG, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  INSERT INTO KOSMOS_OCS.EXAM_ERR_MSG (     \r\n";
            SQL += "    ACTDATE                                 \r\n";
            SQL += ", 	SPECNO                                  \r\n";
            SQL += ", 	MSG                                     \r\n";
            SQL += ", 	ENTDATE                                 \r\n";
            SQL += "  )VALUES(                                  \r\n";

            SQL += " TRUNC(SYSDATE)                             \r\n";
            SQL += " " + ComFunc.covSqlstr(strSPECNO.Trim(), true);
            SQL += " " + ComFunc.covSqlstr(strMSG, true);
            SQL += " , SYSDATE                                  \r\n";
            SQL += "     )                                      \r\n";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }

            return SqlErr;

        }

        public string up_EXAM_SPECMST_STATUS(PsmhDb pDbCon, string strSPECNO, string strJEPSUSABUN, string strJEPSUNAME, string strJEPSUSABUN2, string strJEPSUNAME2, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_SPECMST        \r\n";
            SQL += "     SET RECEIVEDATE    = SYSDATE       \r\n";
            SQL += "       , STATUS         = '01'          \r\n";
            SQL += "       , EMR            = '0'           \r\n";
            SQL += "       , JEPSUSABUN     = " + ComFunc.covSqlstr(strJEPSUSABUN, false);
            SQL += "       , JEPSUNAME      = " + ComFunc.covSqlstr(strJEPSUNAME, false);
            SQL += "       , JEPSUSABUN2    = " + ComFunc.covSqlstr(strJEPSUSABUN2, false);
            SQL += "       , JEPSUNAME2     = " + ComFunc.covSqlstr(strJEPSUNAME2, false);
            SQL += "   WHERE SPECNO         = " + ComFunc.covSqlstr(strSPECNO, false);

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }

            return SqlErr;
        }

        public DataTable sel_EXAM_MASTER_MENU(PsmhDb pDbCon, string strMASTERCODE, string strSPECCODE)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "  SELECT                                                                            \r\n";
            SQL += "            ''												AS CHK			-- 01   \r\n";
            SQL += "          , MASTERCODE										AS MASTERCODE   -- 02   \r\n";

            if (string.IsNullOrEmpty(strSPECCODE.Trim()) == true)
            {
                SQL += "  		, SPECCODE										    AS SPECCODE		-- 03   \r\n";
                SQL += "  		, KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',SPECCODE,'Y')	AS SPEC_NM 		-- 04   \r\n";
            }
            else
            {
                SQL += "  		, '"  + strSPECCODE.Trim() + "'					    AS SPECCODE		-- 03   \r\n";
                SQL += "  		, KOSMOS_OCS.FC_EXAM_SPECMST_NM('14','" + strSPECCODE.Trim() + "','Y')	AS SPEC_NM 		-- 04   \r\n";
            }

            SQL += "  		, KOSMOS_OCS.FC_EXAM_SPECMST_NM('15',TUBECODE,'Y')	AS TUBE_NM 		-- 05   \r\n";
            SQL += "  		, EXAMNAME          								AS EXAM_NAME 	-- 06   \r\n";
            SQL += "  		, TUBECODE                                          AS TUBECODE             \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_MASTER                                                     \r\n";
            SQL += "   WHERE 1=1                                                                        \r\n";
            SQL += "     AND MASTERCODE   = " + ComFunc.covSqlstr(strMASTERCODE.Trim(), false);

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        public DataTable sel_INSA_MASTER_NAME(PsmhDb pDbCon, string strSabun)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "   SELECT                                                                                \r\n";
            SQL += "   		  KORNAME                                                                        \r\n";
            SQL += "    FROM KOSMOS_ADM.INSA_MST 		A                                                    \r\n";
            SQL += "   WHERE 1=1                                                                             \r\n";
            SQL += "     AND A.SABUN = " + ComFunc.covSqlstr(strSabun, false);

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

    }
}
