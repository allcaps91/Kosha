using ComBase; //기본 클래스
using ComDbB; //DB연결
using System;
using System.Collections.Generic;
using System.Data;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name : ComSupLibB.SupLbEx
    /// File Name : clsLbExSQL.cs
    /// Title or Description : 진단검사 검증 SQL
    /// Author : 김홍록
    /// Create Date : 2018-01-11
    /// Update History : 
    /// </summary>

    public class clsComSupLbExVeriSQL : Com.clsMethod
    {
        string SQL = string.Empty;

        public enum enmSel_EXAM_VERIFY       {        CHK,        PANO,        SNAME,  WARDCODE,  ROOMCODE,    DEPTCODE,      DR_NM,      BI_NM,      SEX,      AGE,   STATUS,     CNT_SPECNO,     RCNT,     PCNT,     DCNT,    DIAGNOSYS,   INDATE,   DRCODE,   BI,    CNT_DATE,    CNT_DATE_2,   CNT_DATE_3,    CNT_MONTH,   JUMIN, IPD_GBSTS   };
        public string[] sSel_EXAM_VERIFY =   {     "선택",  "등록번호",   "환자성명",    "병동",    "병실",        "과",   "처방의", "환자종류",   "성별",   "나이",   "상태", "입원총검체수",      "R",      "P",      "D", "R/O 상병명", "INDATE", "DRCODE", "BI",  "CNT_DATE",  "CNT_DATE_2",  "CNT_DATE_3", "CNT_MONTH", "JUMIN", "IPD_GBSTS" };
        public int[] nSel_EXAM_VERIFY =      {  nCol_SCHK,   nCol_PANO,   nCol_SNAME,  nCol_AGE,  nCol_AGE, nCol_AGE-10, nCol_SNAME,   nCol_AGE, nCol_AGE, nCol_AGE, nCol_AGE, nCol_AGE + 20 , nCol_AGE, nCol_AGE, nCol_AGE,   nCol_LNAME,        5,        5,    5,           5,             5,             5,           5,       5,            5};

        public enum enmSel_EXAM_VERIFY_MAIN     {     JDATE,      PANO,      SNAME,   INDATE,        SEX,       AGE,   DEPTCODE,    WARD,   ROOM,   DRCODE,      DR_NM, STATUS,  STATUS_NM,   PRINT,   RESULTDATE,   RESULTSABUN,        DISEASE,  VIEWKEY1,  VIEWKEY2,  VIEWKEY3,  ITEMS1,  ITEMS2,  VERIFY1,  VERIFY2,  VERIFY3,  VERIFY4,  VERIFY5,  VERIFY6,  COMMENTS,  RECOMMENDATION,  BI,  SDATE,  EDATE,  CERTNO, JUMIN, ROWID };
        public string[] sSel_EXAM_VERIFY_MAIN = {"접수일자","등록번호",     "성명", "INDATE",     "성별",  "나이",       "과",   "WARD","ROOM", "DRCODE",   "처방의", "STATUS",    "상태", "PRINT", "RESULTDATE", "RESULTSABUN",    "R/o 상병","VIEWKEY1","VIEWKEY2","VIEWKEY3","ITEMS1","ITEMS2","VERIFY1","VERIFY2","VERIFY3","VERIFY4","VERIFY5","VERIFY6","COMMENTS","RECOMMENDATION","BI","SDATE","EDATE","CERTNO","JUMIN", "ROWID"};
        public int[] nSel_EXAM_VERIFY_MAIN    = { nCol_DATE, nCol_PANO, nCol_SNAME,        5,  nCol_SCHK, nCol_AGE,  nCol_SCHK,        5,     5,        5, nCol_SNAME,        5,  nCol_AGE,       5,            5,             5,  nCol_PANO+20,         5,         5,         5,       5,       5,        5,        5,        5,       5,         5,        5,         5,               5,   5,      5,    5,     5,        5,5};

        public enum enmSel_EXAM_VERIFY_LIST     {       CHK,      JDATE,       PANO,      SNAME,       SEX,      AGE, RESULTDATE,      WARD,      ROOM,  DEPTCODE,      DR_NM,    STATUS_NM,    DISEASE,  ROWID    };
        public string[] sSel_EXAM_VERIFY_LIST = {    "선택","접수일자",  "등록번호",     "성명",    "성별",   "나이", "결과일자",    "병동",    "병실",      "과",   "처방의",       "상태", "R/o 상병", "ROWID"   };
        public int[] nSel_EXAM_VERIFY_LIST    = { nCol_SCHK, nCol_DATE,   nCol_PANO, nCol_SNAME, nCol_AGE, nCol_AGE,   nCol_DATE,  nCol_AGE,  nCol_AGE,  nCol_AGE, nCol_SNAME, nCol_AGE +10,  nCol_NAME+50,      5 };

        public enum enmSel_EXAM_SPECMST         {       CHK,     SPECNO,      EXAM_NAME,        SPEC_NM,   WORKSTS_NM,  RECEIVEDATE, RESULTDATE ,    IPDOPD,     WARD,     ROOM,    DEPTCODE,    DR_NM,   WORKSTS, BLOODDATE,    BDATE };      
        public string[] sSel_EXAM_SPECMST =     {    "선택", "검체번호",       "검사명",         "검체", "WORKSTS_NM",   "채혈일시",  "결과일시",    "구분",   "병동",   "병실",        "과",  "의사명",     "WS","채혈일시", "처방일자" };
        public int[] nSel_EXAM_SPECMST =        { nCol_SCHK,  nCol_PANO, nCol_JUSO + 30, nCol_PANO + 10,            5,    nCol_PANO,   nCol_PANO, nCol_SCHK, nCol_AGE, nCol_AGE, nCol_AGE-10, nCol_PANO, nCol_AGE, nCol_PANO, nCol_PANO };

        public enum enmSel_EXAM_RESULTC         {       CHK,        SPECNO,        EXAMNAME,      RESULT,     REFER,     PANIC,     DELTA,             REF,           UNIT,       SPECIMEN, RESULTDATE, RESULTSABUN_NM,  MASTERCODE,    SUBCODE,    SEQNO, SORT  };
        public string[] sSel_EXAM_RESULTC =     {    "선택",     "검체번호",       "검사명",      "결과",       "R",       "P",       "D",        "참고치",         "UNIT",         "검체", "결과일시",       "검사자", "마스터코드", "서브코드", "SEQNO","SORT" };
        public int[] nSel_EXAM_RESULTC =        { nCol_SCHK,  nCol_PANO - 5,   nCol_SNAME-5,nCol_SNAME-5, nCol_SCHK, nCol_SCHK, nCol_SCHK, nCol_SNAME + 15, nCol_PANO - 10, nCol_PANO - 10,  nCol_PANO,     nCol_SNAME,    nCol_PANO,  nCol_PANO,       5,    5  };

        public DataSet sel_EXAM_RESULTC(PsmhDb pDbCon, string strINDATE, string strPANO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "    WITH T AS (                                                                                     \r\n";
            SQL += "    	SELECT                                                                                      \r\n";
            SQL += "    			'' CHK                                                                              \r\n";
            SQL += "    		  , R.SPECNO                                                                            \r\n";
            SQL += "    		  , M.EXAMNAME                                                                          \r\n";
            SQL += "    		  , R.RESULT                                                                            \r\n";
            SQL += "    		  , TO_NUMBER(R.SEQNO)SEQNO                                                             \r\n";
            SQL += "    		  , R.REFER                                                                             \r\n";
            SQL += "    		  , R.PANIC                                                                             \r\n";
            SQL += "    		  , R.DELTA                                                                             \r\n";
            SQL += "    		  , R.UNIT                                                                              \r\n";
            SQL += "    		  , 0 SORT                                                                              \r\n";
            SQL += "    		  , S.SEX                                                                               \r\n";
            SQL += "    		  , S.AGE                                                                               \r\n";
            SQL += "    		  , R.SUBCODE                                                                           \r\n";
            SQL += "    		  , S.SPECCODE                                                                          \r\n";
            SQL += "    		  , R.RESULTWS                                                                          \r\n";
            SQL += "    		  , TO_CHAR(R.RESULTDATE,'YYYY-MM-DD HH24:MI')  AS RESULTDATE                           \r\n";
            SQL += "    		  , KOSMOS_OCS.FC_BAS_USER_NAME(R.RESULTSABUN)  AS RESULTSABUN_NM                       \r\n";
            SQL += "    		  , R.MASTERCODE                                                                        \r\n";
            SQL += "    	  FROM KOSMOS_OCS.EXAM_RESULTC R                                                            \r\n";
            SQL += "    	  	 , KOSMOS_OCS.EXAM_MASTER  M                                                            \r\n";
            SQL += "    	  	 , KOSMOS_OCS.EXAM_SPECMST S                                                            \r\n";
            SQL += "    	WHERE 1=1                                                                                   \r\n";
            SQL += "          AND S.PANO    = " + ComFunc.covSqlstr(strPANO, false);
            SQL += "          AND S.BDATE  >= TO_DATE('" + strINDATE + "','YYYY-MM-DD') \r\n";
            SQL += "    	  AND S.STATUS NOT IN ('00','06')                                                           \r\n";
            SQL += "    	  AND S.SPECNO	= R.SPECNO                                                                  \r\n";
            SQL += "    	  AND R.SUBCODE = M.MASTERCODE                                                              \r\n";
            SQL += "    	UNION ALL                                                                                   \r\n";
            SQL += "    	SELECT                                                                                      \r\n";
            SQL += "    			'' CHK                                                                              \r\n";
            SQL += "    		  , R.SPECNO                                                                            \r\n";
            SQL += "    		  , M.EXAMNAME                                                                          \r\n";
            SQL += "    		  , F.FOOTNOTE AS RESULT                                                                \r\n";
            SQL += "    		  , TO_NUMBER(F.SEQNO) SEQNO                                                            \r\n";
            SQL += "    		  , '' REFER                                                                            \r\n";
            SQL += "    		  , '' PANIC                                                                            \r\n";
            SQL += "    		  , '' DELTA                                                                            \r\n";
            SQL += "    		  , '' UNIT                                                                             \r\n";
            SQL += "    		  , F.SORT                                                                              \r\n";
            SQL += "    		  , S.SEX                                                                               \r\n";
            SQL += "    		  , S.AGE                                                                               \r\n";
            SQL += "    		  , R.SUBCODE                                                                           \r\n";
            SQL += "    		  , S.SPECCODE                                                                          \r\n";
            SQL += "    		  , R.RESULTWS                                                                          \r\n";
            SQL += "    		  , TO_CHAR(R.RESULTDATE,'YYYY-MM-DD HH24:MI')  AS RESULTDATE                           \r\n";
            SQL += "    		  , KOSMOS_OCS.FC_BAS_USER_NAME(R.RESULTSABUN)  AS RESULTSABUN_NM                       \r\n";
            SQL += "    		  , R.MASTERCODE                                                                        \r\n";
            SQL += "    	  FROM KOSMOS_OCS.EXAM_RESULTC  R                                                           \r\n";
            SQL += "    	  	 , KOSMOS_OCS.EXAM_MASTER   M                                                           \r\n";
            SQL += "    	  	 , KOSMOS_OCS.EXAM_SPECMST  S                                                           \r\n";
            SQL += "    	  	 , KOSMOS_OCS.EXAM_RESULTCF F                                                           \r\n";
            SQL += "    	WHERE 1=1                                                                                   \r\n";
            SQL += "          AND S.PANO    = " + ComFunc.covSqlstr(strPANO, false);
            SQL += "          AND S.BDATE  >= TO_DATE('" + strINDATE + "','YYYY-MM-DD') \r\n";
            SQL += "    	  AND S.STATUS NOT IN ('00','06')                                                           \r\n";
            SQL += "    	  AND S.SPECNO	= R.SPECNO                                                                  \r\n";
            SQL += "    	  AND R.SPECNO  = F.SPECNO                                                                  \r\n";
            SQL += "    	  AND F.SEQNO   = TO_NUMBER(R.SEQNO)                                                        \r\n";
            SQL += "    	  AND R.SUBCODE = M.MASTERCODE                                                              \r\n";
            SQL += "    )                                                                                               \r\n";
            SQL += "    SELECT                                                                                          \r\n";
            SQL += "    	CHK                                                                                         \r\n";
            SQL += "    	, SPECNO                                                                                    \r\n";
            SQL += "    	, EXAMNAME                                                                                  \r\n";
            SQL += "    	, RESULT                                                                                    \r\n";
            SQL += "    	, REFER                                                                                     \r\n";
            SQL += "    	, PANIC                                                                                     \r\n";
            SQL += "    	, DELTA                                                                                     \r\n";
            //SQL += "    	, KOSMOS_OCS.FC_EXAM_MASTER_SUB_REF('0', SUBCODE,SEX,TO_NUMBER(AGE), RESULT) AS REF         \r\n";
            SQL += "    	, KOSMOS_OCS.FC_EXAM_MASTER_SUB_REF2('0', SUBCODE,SEX,TO_NUMBER(AGE), RESULT, RESULTDATE) AS REF    \r\n";
            SQL += "    	, UNIT                                                                                      \r\n";
            SQL += "    	, KOSMOS_OCS.FC_EXAM_SPECMST_NM('14', SPECCODE,'N') AS SPECIMEN                             \r\n";
            SQL += "    	, RESULTDATE                                                                                \r\n";
            SQL += "    	, RESULTSABUN_NM                                                                            \r\n";
            SQL += "    	, MASTERCODE                                                                                \r\n";
            SQL += "    	, SUBCODE                                                                                   \r\n";
            SQL += "    	, SEQNO                                                                                     \r\n";
            SQL += "    	, SORT                                                                                      \r\n";
            SQL += "    FROM T                                                                                          \r\n";
            SQL += "    ORDER BY RESULTWS,SPECNO DESC,SEQNO                                                             \r\n";


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

        public DataSet sel_EXAM_SPECMST(PsmhDb pDbCon, string strINDATE, string strPANO)
        {
            DataSet ds = null;
            SQL = "";
            SQL += " SELECT                                                                             \r\n";
            SQL += " 	  ''														    AS CHK          \r\n";
            SQL += " 	, A.SPECNO 													    AS SPECNO       \r\n";
            SQL += "     , TRIM(KOSMOS_OCS.FC_EXAM_RESULTC_EXAMNAME(A.SPECNO)) 		    AS EXAM_NAME    \r\n";
            SQL += "     , TRIM(KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',A.SPECCODE,'N')) 	AS SPEC_NM      \r\n";
            SQL += "     , ( CASE WHEN SUBSTR(WORKSTS,1,1) = 'C' THEN '생화학검사:'                     \r\n";
            SQL += "              WHEN SUBSTR(WORKSTS,1,1) = 'F' THEN '체액검사:'                       \r\n";
            SQL += "              WHEN SUBSTR(WORKSTS,1,1) = 'H' THEN '혈액학검사:'                     \r\n";
            SQL += "              WHEN SUBSTR(WORKSTS,1,1) = 'S' THEN '혈청학검사:'                     \r\n";
            SQL += "              WHEN SUBSTR(WORKSTS,1,1) = 'E' THEN '면역학검사:'                     \r\n";
            SQL += "              WHEN SUBSTR(WORKSTS,1,1) = 'B' THEN '혈액은행:'                       \r\n";
            SQL += "              WHEN SUBSTR(WORKSTS,1,1) = 'U' THEN '소변검사:'                       \r\n";
            SQL += "              WHEN SUBSTR(WORKSTS,1,1) = 'P' THEN '분변검사:'                       \r\n";
            SQL += "              WHEN SUBSTR(WORKSTS,1,1) = 'M' THEN '미생물검사:'                     \r\n";
            SQL += "              WHEN SUBSTR(WORKSTS,1,1) = 'A' THEN '세포학:'                         \r\n";
            SQL += "              WHEN SUBSTR(WORKSTS,1,1) = 'T' THEN '조직학:'                         \r\n";
            SQL += "              WHEN SUBSTR(WORKSTS,1,1) = 'W' THEN '외부의뢰검사:'                   \r\n";
            SQL += "            ELSE                    '기타검사:'                                     \r\n";
            SQL += "       END )                                        AS WORKSTS_NM                   \r\n";
            SQL += "      ,TO_CHAR(RECEIVEDATE,'YYYY-MM-DD HH24:MI')    AS RECEIVEDATE  -- 접수일자     \r\n";
            SQL += "      ,TO_CHAR(RESULTDATE,'YYYY-MM-DD HH24:MI')     AS RESULTDATE   -- 결과일자     \r\n";
            SQL += "      ,IPDOPD                                       AS IPDOPD       -- 구분         \r\n";
            SQL += "      ,WARD                                         AS WARD         -- 과           \r\n";
            SQL += "      ,ROOM                                         AS ROOM         -- 과           \r\n";
            SQL += "      ,DEPTCODE                                     AS DEPTCODE     -- 과           \r\n";
            SQL += "      ,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE) 	AS DR_NM                        \r\n";
            SQL += "      ,WORKSTS                                      AS WORKSTS                      \r\n";
            SQL += "      ,TO_CHAR(BLOODDATE,'YYYY-MM-DD HH24:MI')      AS BLOODDATE       -- 처방일자      \r\n";
            SQL += "      ,TO_CHAR(BDATE,'YYYY-MM-DD')                  AS BDATE       -- 처방일자      \r\n";

            SQL += "  FROM KOSMOS_OCS.EXAM_SPECMST A                                                    \r\n";
            SQL += " WHERE 1=1                                                                          \r\n";
            SQL += "   AND PANO    = " + ComFunc.covSqlstr(strPANO, false);
            SQL += "   AND BDATE  >= TO_DATE('" + strINDATE + "','YYYY-MM-DD')                          \r\n";
            SQL += "   AND STATUS NOT IN ('00','06')                                                    \r\n";
            SQL += " ORDER BY SUBSTR(WORKSTS,1,1),BDATE DESC,SPECNO                                     \r\n";

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

        public DataSet sel_EXAM_VERIFY_LIST(PsmhDb pDbCon, string strFDATE, string strTDATE, string strSTATUS, string strPTNO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                          \r\n";
            SQL += "         ''                                         AS CHK        \r\n";
            SQL += "       , TO_CHAR(JDATE,'YYYY-MM-DD') 				AS JDATE      \r\n";
            SQL += "       , PANO                                                     \r\n";
            SQL += "       , SNAME                                                    \r\n";
            SQL += "       , SEX                                                      \r\n";
            SQL += "       , AGE                                                      \r\n";
            SQL += "       , TO_CHAR(RESULTDATE,'YYYY-MM-DD') 			AS RESULTDATE \r\n";
            SQL += "       , WARD                                                     \r\n";
            SQL += "       , ROOM                                                     \r\n";
            SQL += "       , DEPTCODE                                                 \r\n";
            SQL += "       , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE) 	AS DR_NM      \r\n";
            SQL += "  	 , CASE WHEN A.STATUS = 1 THEN '접수'                         \r\n";
            SQL += "  	 	    WHEN A.STATUS = 2 THEN '임시저장'                     \r\n";
            SQL += "  			WHEN A.STATUS = 3 THEN '완료'                         \r\n";
            SQL += "  			WHEN A.STATUS = 4 THEN '인쇄'                         \r\n";
            SQL += "  		END STATUS_NM                                             \r\n";
            SQL += "       , 	DISEASE                                               \r\n";
            SQL += "       , 	ROWID                                                 \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_VERIFY A                                 \r\n";
            SQL += "   WHERE 1=1                                                      \r\n";

            if (string.IsNullOrEmpty(strPTNO) == false)
            {
                SQL += "     AND PANO = " + ComFunc.covSqlstr(strPTNO, false);
            }

            SQL += "     AND JDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
            SQL += "                   AND " + ComFunc.covSqlDate(strTDATE, false);

            if (string.IsNullOrEmpty(strSTATUS) == false)
            {
                SQL += "     AND STATUS = " + ComFunc.covSqlstr(strSTATUS, false);
            }
            
            SQL += "   ORDER BY JDATE, SNAME                                                \r\n";
                                                                                            
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

        public string del_EXAM_VERIFY(PsmhDb pDbCon, string strROWID, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "DELETE KOSMOS_OCS.EXAM_VERIFY";
            SQL += " WHERE 1=1                              \r\n";
            SQL += "   AND ROWID            = " + ComFunc.covSqlstr(strROWID, false);  /* 전자서명번호*/

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

        public string ins_EXAM_VERIFY(PsmhDb pDbCon, string[] dr, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "    INSERT INTO KOSMOS_OCS.EXAM_VERIFY (         \r\n";
            SQL += "            JDATE                                \r\n";
            SQL += "          , PANO                                 \r\n";
            SQL += "          , SNAME                                \r\n";
            SQL += "          , BI                                   \r\n";
            SQL += "          , INDATE                               \r\n";
            SQL += "          , AGE                                  \r\n";
            SQL += "          , SEX                                  \r\n";
            SQL += "          , DEPTCODE                             \r\n";
            SQL += "          , WARD                                 \r\n";   
            SQL += "          , ROOM                                 \r\n";
            SQL += "          , DRCODE                               \r\n";
            SQL += "          , DISEASE                              \r\n";
            SQL += "          , STATUS                               \r\n";
            SQL += "          , PRINT                                \r\n";
            SQL += "          , VERIFY1                              \r\n";
            SQL += "          , VERIFY2                              \r\n";
            SQL += "          , VERIFY4                              \r\n";
            SQL += "          , VERIFY5                              \r\n";
            SQL += "         ) VALUES (                              \r\n";
            SQL += "         " + ComFunc.covSqlDate(dr[(int)enmSel_EXAM_VERIFY_MAIN.JDATE   ].ToString(), false);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_VERIFY_MAIN.PANO    ].ToString(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_VERIFY_MAIN.SNAME   ].ToString(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_VERIFY_MAIN.BI      ].ToString(), true);
            SQL += "         " + ComFunc.covSqlDate(dr[(int)enmSel_EXAM_VERIFY_MAIN.INDATE  ].ToString(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_VERIFY_MAIN.AGE     ].ToString(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_VERIFY_MAIN.SEX     ].ToString(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_VERIFY_MAIN.DEPTCODE].ToString(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_VERIFY_MAIN.WARD    ].ToString(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_VERIFY_MAIN.ROOM    ].ToString(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_VERIFY_MAIN.DRCODE  ].ToString(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_VERIFY_MAIN.DISEASE ].ToString(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_VERIFY_MAIN.STATUS  ].ToString(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_VERIFY_MAIN.PRINT   ].ToString(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_VERIFY_MAIN.VERIFY1 ].ToString(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_VERIFY_MAIN.VERIFY2 ].ToString(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_VERIFY_MAIN.VERIFY4 ].ToString(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_VERIFY_MAIN.VERIFY5 ].ToString(), true);
            SQL += "         ) \r\n";



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

        public string up_EXAM_VERIFY(PsmhDb pDbCon, DataRow dr, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "UPDATE KOSMOS_OCS.EXAM_VERIFY";
            SQL += "   SET STATUS           = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_VERIFY_MAIN.STATUS].ToString()          , false);   /* 상태(1.접수 2.임시저장 3.결과완료 4.인쇄)*/
            SQL += "     , RESULTDATE       = SYSDATE   \r\n";
            SQL += "     , RESULTSABUN      = " + ComFunc.covSqlstr(clsType.User.IdNumber     , false);                                     /* 결과입력자사번(의사만 가능함)*/
            SQL += "     , ITEMS1           = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_VERIFY_MAIN.ITEMS1].ToString(), false);             /* 검증항목(열거)*/
            SQL += "     , ITEMS2           = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_VERIFY_MAIN.ITEMS2].ToString(), false);             /* 비정상결과 또는 유의한결과 항목*/
            SQL += "     , COMMENTS         = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_VERIFY_MAIN.COMMENTS].ToString().Replace("'","`"), false);           /* 검증/판독 소견*/
            SQL += "     , RECOMMENDATION   = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_VERIFY_MAIN.RECOMMENDATION].ToString().Replace("'", "`"), false);     /* 추천*/
            SQL += "     , DISEASE          = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_VERIFY_MAIN.DISEASE].ToString().Replace("'", "`"), false); /* 상병명칭(병동에서 입력한 R/O상병)*/

            SQL += "     , SDATE            = " + ComFunc.covSqlDate(dr[(int)enmSel_EXAM_VERIFY_MAIN.SDATE].ToString(), false);             /* 시작일*/
            SQL += "     , EDATE            = " + ComFunc.covSqlDate(dr[(int)enmSel_EXAM_VERIFY_MAIN.EDATE].ToString(), false);             /* 종료일*/

            SQL += "     , VERIFY1          = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_VERIFY_MAIN.VERIFY1].ToString()         , false);   /* Calibration Verification*/
            SQL += "     , VERIFY2          = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_VERIFY_MAIN.VERIFY2].ToString()         , false);   /* Delta Check Verification*/
            SQL += "     , VERIFY3          = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_VERIFY_MAIN.VERIFY3].ToString()         , false);   /* Repeat/Recheck*/
            SQL += "     , VERIFY4          = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_VERIFY_MAIN.VERIFY4].ToString()         , false);   /* Internal Quality Control*/
            SQL += "     , VERIFY5          = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_VERIFY_MAIN.VERIFY5].ToString()         , false);   /* Panic/Alert Value Verification*/
            SQL += "     , VERIFY6          = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_VERIFY_MAIN.VERIFY6].ToString()         , false);   /* 기타검증항목명칭*/

            if (dr[(int)enmSel_EXAM_VERIFY_MAIN.CERTNO].ToString().Trim().Equals("0") || dr[(int)enmSel_EXAM_VERIFY_MAIN.CERTNO].ToString().Trim().Equals(""))
            {
                SQL += "     , CERTNO       = ''    \r\n"; 
            }
            else
            {
                SQL += "     , CERTNO       = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_VERIFY_MAIN.CERTNO].ToString(), false);   /* 전자서명번호*/
            }
            
            SQL += " WHERE 1=1                              \r\n";
            SQL += "   AND ROWID            = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_VERIFY_MAIN.ROWID].ToString()           , false);   /* 전자서명번호*/


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

        public DataSet sel_EXAM_VERIFY_MAIN(PsmhDb pDbCon, string strFDATE, string strTDATE, string strSTATUS)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                        \r\n";
            SQL += "  	  TO_CHAR(A.JDATE,'YYYY-MM-DD') AS JDATE -- 접수일자                                       \r\n";
            SQL += "  	, A.PANO -- 등록번호                                        \r\n";
            SQL += "  	, A.SNAME -- 성명                                           \r\n";
            SQL += "  	, TO_CHAR(A.INDATE,'YYYY-MM-DD') AS INDATE -- 입원일자                                      \r\n";
            SQL += "  	, A.SEX -- 성별                                             \r\n";
            SQL += "  	, A.AGE -- 나이                                             \r\n";
            SQL += "  	, A.DEPTCODE -- 진료과                                      \r\n";
            SQL += "  	, A.WARD -- 병동                                            \r\n";
            SQL += "  	, A.ROOM -- 호실                                            \r\n";
            SQL += "  	, A.DRCODE -- 의사코드                                      \r\n";
            SQL += "    , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE) 	AS DR_NM	-- 07 \r\n";
            SQL += "  	, A.STATUS -- 상태(1.접수 2.임시저장 3.결과완료 4.인쇄)     \r\n";
            SQL += "  	, CASE WHEN A.STATUS = 1 THEN '접수'                        \r\n";
            SQL += "  	       WHEN A.STATUS = 2 THEN '임시저장'                    \r\n";
            SQL += "  	       WHEN A.STATUS = 3 THEN '완료'                        \r\n";
            SQL += "  	       WHEN A.STATUS = 4 THEN '인쇄'                        \r\n";
            SQL += "  	  END STATUS_NM                                             \r\n";
            SQL += "  	, A.PRINT -- 인쇄횟수                                       \r\n";
            SQL += "  	, A.RESULTDATE -- 결과완료일시                              \r\n";
            SQL += "  	, A.RESULTSABUN -- 결과입력자사번(의사만 가능함)            \r\n";
            SQL += "  	, A.DISEASE -- 상병명칭(병동에서 입력한 R/O상병)            \r\n";
            SQL += "  	, A.VIEWKEY1 -- 조회용 Key(1)                               \r\n";
            SQL += "  	, A.VIEWKEY2 -- 조회용 Key(1)                               \r\n";
            SQL += "  	, A.VIEWKEY3 -- 조회용 Key(1)                               \r\n";
            SQL += "  	, A.ITEMS1 -- 검증항목(열거)                                \r\n";
            SQL += "  	, A.ITEMS2 -- 비정상결과 또는 유의한결과 항목               \r\n";
            SQL += "  	, A.VERIFY1 -- Calibration Verification                     \r\n";
            SQL += "  	, A.VERIFY2 -- Delta Check Verification                     \r\n";
            SQL += "  	, A.VERIFY3 -- Repeat/Recheck                               \r\n";
            SQL += "  	, A.VERIFY4 -- Internal Quality Control                     \r\n";
            SQL += "  	, A.VERIFY5 -- Panic/Alert Value Verification               \r\n";
            SQL += "  	, A.VERIFY6 -- 기타검증항목명칭                             \r\n";
            SQL += "  	, A.COMMENTS -- 검증/판독 소견                              \r\n";
            SQL += "  	, A.RECOMMENDATION -- 추천                                  \r\n";
            SQL += "  	, A.BI -- 접수시의 환자종류                                 \r\n";
            SQL += "  	, TO_CHAR(A.SDATE,'YYYY-MM-DD') AS SDATE -- 시작일          \r\n";
            SQL += "  	, TO_CHAR(A.EDATE,'YYYY-MM-DD') AS EDATE -- 종료일          \r\n";
            SQL += "  	, A.CERTNO -- 전자서명번호                                  \r\n";
            SQL += "  	, B.JUMIN1 || '-' || B.JUMIN2       	   AS JUMIN         \r\n";
            SQL += "  	, A.ROWID                                                   \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_VERIFY  A                               \r\n";
            SQL += "      , KOSMOS_PMPA.BAS_PATIENT B                               \r\n";
            SQL += "  WHERE 1=1                                                     \r\n";
            SQL += "    AND A.JDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
            SQL += "    			    AND " + ComFunc.covSqlDate(strTDATE, false);
            SQL += "    AND A.PANO   = B.PANO                                         \r\n";
            SQL += "    AND A.STATUS = " + ComFunc.covSqlstr(strSTATUS, false);


            //if (isCOMPLITE == true)
            //{
            //    SQL += "    AND A.STATUS IN ('3','4')                                     \r\n";
            //}
            //else
            //{
            //    SQL += "    AND A.STATUS IN ('1','2')                                     \r\n";
            //}

            SQL += "  ORDER BY WARD,SNAME,PANO,JDATE                                \r\n";

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
      
        public DataSet sel_EXAM_VERIFY(PsmhDb pDbCon, string strINDATE , string strJINDATE, bool isIPD_MASTER, string strPANO)
        {
            DataSet ds = null;
            SQL = "";
            SQL += "  WITH T AS (                                                                                                                                                                                   \r\n";
            SQL += "  	SELECT                                                                                                                                                                                      \r\n";
            SQL += "  	       '' 											AS CHK      -- 01                                                                                                                       \r\n";
            SQL += "  	     , A.PANO                           			AS PANO		-- 02                                                                                                                       \r\n";
            SQL += "  		 , A.SNAME                          			AS SNAME	-- 03                                                                                                                       \r\n";
            SQL += "  		 , A.WARDCODE                       			AS WARDCODE -- 04                                                                                                                       \r\n";
            SQL += "  		 , A.ROOMCODE									AS ROOMCODE -- 05                                                                                                                       \r\n";
            SQL += "  		 , A.DEPTCODE									AS DEPTCODE	-- 06                                                                                                                       \r\n";
            SQL += "  		 , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE) 	AS DR_NM	-- 07                                                                                                                       \r\n";
            SQL += "  		 , A.AGE										AS AGE		-- 09                                                                                                                       \r\n";
            SQL += "  		 , A.SEX                            			AS SEX      -- 10                                                                                                                       \r\n";
            SQL += "  		 , SUM(DECODE(C.REFER,'L',1,'H',1,0)) 			AS RCNT     -- 12                                                                                                                       \r\n";
            SQL += "  		 , SUM(DECODE(C.Panic,'P',1,0)) 	  			AS PCNT     -- 13                                                                                                                       \r\n";
            SQL += "  		 , SUM(DECODE(C.DELTA,'D',1,0)) 				AS DCNT     -- 14                                                                                                                       \r\n";
            SQL += "  		 , (                                                                                                                                                                                    \r\n";
            SQL += "  		        SELECT MAX(DIAGNOSYS) FROM KOSMOS_PMPA.NUR_JINDAN                                                                                                                                    \r\n";
            SQL += "  		        WHERE PANO   = A.PANO                                                                                                                                                           \r\n";
            SQL += "  		         AND (INDATE, ACTDATE) = (SELECT MAX(INDATE), MAX(ACTDATE)                                                                                                                      \r\n";
            SQL += "  		                                    FROM KOSMOS_PMPA.NUR_JINDAN                                                                                                                         \r\n";
            SQL += "  		  	        			           WHERE PANO   = A.PANO                                                                                                                                \r\n";
            SQL += "  		         	                      )                                                                                                                                                     \r\n";
            SQL += "  	       )                                            AS DIAGNOSYS -- 15                                                                                                                      \r\n";
            SQL += "  		 , TO_CHAR(A.INDATE,'YYYY-MM-DD') 				AS INDATE    -- 16                                                                                                                      \r\n";
            SQL += "  		 , A.DRCODE										AS DRCODE	 -- 17                                                                                                                      \r\n";
            SQL += "  		 , A.BI											AS BI		 -- 19                                                                                                                      \r\n";
            SQL += "  		 , '0'											AS STATUS                                                                                                                               \r\n";
            SQL += "  		 , (SELECT COUNT(*)  FROM KOSMOS_OCS.EXAM_VERIFY   WHERE JDATE = TO_DATE('"+ strJINDATE + "','YYYY-MM-DD'))  								 	                                            AS CNT_DATE   \r\n";
            SQL += "  		 , (SELECT COUNT(*)  FROM KOSMOS_OCS.EXAM_VERIFY   WHERE JDATE = TO_DATE('" + strJINDATE + "','YYYY-MM-DD') AND STATUS = '2')       		 	                                            AS CNT_DATE_2 \r\n";
            SQL += "  		 , (SELECT COUNT(*)  FROM KOSMOS_OCS.EXAM_VERIFY   WHERE JDATE = TO_DATE('" + strJINDATE + "','YYYY-MM-DD') AND STATUS = '3')       		 	                                            AS CNT_DATE_3 \r\n";
            SQL += "  		 , (SELECT COUNT(*)  FROM KOSMOS_OCS.EXAM_VERIFY   WHERE JDATE BETWEEN TO_DATE('" + strJINDATE.Substring(0,8) + "01" + "','YYYY-MM-DD') AND LAST_DAY('"+ strJINDATE + "')) 	                AS CNT_MONTH  \r\n";
            SQL += "  		 , (SELECT COUNT(*)  FROM KOSMOS_OCS.EXAM_SPECMST  WHERE PANO = A.PANO AND BDATE >= TO_DATE('"+ strINDATE + "', 'YYYY-MM-DD') AND STATUS  NOT IN ('00','06') AND WORKSTS NOT IN ('A','T') ) AS CNT_SPECNO \r\n";
            SQL += "  		 , MAX(E.JUMIN1) || '-' || MAX(E.JUMIN2)	   AS JUMIN                                                                                                                                 \r\n";
            SQL += "  		 , MAX(A.GBSTS)                                AS IPD_GBSTS                                                                                                                             \r\n";
            SQL += "  	 FROM KOSMOS_PMPA.IPD_NEW_MASTER A                                                                                                                                                          \r\n";
            SQL += "  	    , KOSMOS_OCS.EXAM_SPECMST    B                                                                                                                                                          \r\n";
            SQL += "  	    , KOSMOS_OCS.EXAM_RESULTC	 C                                                                                                                                                          \r\n";
            SQL += "  	    , KOSMOS_PMPA.BAS_PATIENT    E                                                                                                                                                          \r\n";
            SQL += "  	WHERE 1=1                                                                                                                                                                                   \r\n";
            if (isIPD_MASTER == true)
            {
                SQL += "  	  AND 'I' =    'I'                                                                                                                                                                      \r\n";
            }
            else
            {
                SQL += "  	  AND 'V' =    'I'                                                                                                                                                                      \r\n";
            }
            
            SQL += "  	  AND TRUNC(A.INDATE) = TO_DATE('" + strINDATE + "','YYYY-MM-DD')                                                                                                                           \r\n";
            SQL += "  	  AND A.PANO 	= B.PANO                                                                                                                                                                    \r\n";
            SQL += "  	  AND A.PANO	= E.PANO                                                                                                                                                                    \r\n";
            SQL += "  	  AND B.BDATE	>= TRUNC(A.INDATE)                                                                                                                                                          \r\n";
            SQL += "  	  AND B.SPECNO	= C.SPECNO                                                                                                                                                                  \r\n";
            SQL += "  	  AND B.STATUS  NOT IN ('00','06')                                                                                                                                                          \r\n";
            SQL += "  	  AND B.WORKSTS NOT IN ('A','T')                                                                                                                                                            \r\n";
            SQL += "  	  AND A.GBSTS 	    IN ('0', '2', '3', '4')                                                                                                                                                 \r\n";
            SQL += "  	  AND A.OUTDATE     IS NULL                                                                                                                                                                 \r\n";
            SQL += "  	  AND A.PANO NOT    IN (SELECT PANO                                                                                                                                                         \r\n";
            SQL += "  	  					      FROM KOSMOS_OCS.EXAM_VERIFY                                                                                                                                       \r\n";
            SQL += "  	  					     WHERE JDATE >= TO_DATE('" + strINDATE + "','YYYY-MM-DD')                                                                                                           \r\n";
            SQL += "  	  					)                                                                                                                                                                       \r\n";

            if (string.IsNullOrEmpty(strPANO) == false)
            {
                SQL += "  	AND ( A.PANO =   " + ComFunc.covSqlstr(strPANO, false);
                SQL += "  	        OR A.PANO =   (SELECT PANO FROM KOSMOS_OCS.EXAM_SPECMST WHERE SPECNO = " + ComFunc.covSqlstr(strPANO, false);
                SQL += "  	     ))   \r\n";
            }
          
            SQL += "  	GROUP BY A.PANO, A.SNAME, TO_CHAR(A.INDATE,'YYYY-MM-DD'), A.WARDCODE, A.ROOMCODE, A.DEPTCODE, A.DRCODE, A.BI, A.AGE, A.SEX                                                                  \r\n";
            SQL += "  	UNION ALL                                                                                                                                                                                   \r\n";
            SQL += "  	SELECT                                                                                                                                                                                      \r\n";
            SQL += "  	       '' 											AS CHK      -- 01                                                                                                                       \r\n";
            SQL += "  	     , A.PANO                           			AS PANO		-- 02                                                                                                                       \r\n";
            SQL += "  		 , A.SNAME                          			AS SNAME	-- 03                                                                                                                       \r\n";
            SQL += "  		 , A.WARD                           			AS WARDCODE -- 04                                                                                                                       \r\n";
            SQL += "  		 , A.ROOM    									AS ROOMCODE -- 05                                                                                                                       \r\n";
            SQL += "  		 , A.DEPTCODE									AS DEPTCODE	-- 06                                                                                                                       \r\n";
            SQL += "  		 , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE) 	AS DR_NM	-- 07                                                                                                                       \r\n";
            SQL += "  		 , A.AGE										AS AGE		-- 09                                                                                                                       \r\n";
            SQL += "  		 , A.SEX                            			AS SEX      -- 10                                                                                                                       \r\n";
            SQL += "  		 , SUM(DECODE(C.REFER,'L',1,'H',1,0)) 			AS RCNT     -- 12                                                                                                                       \r\n";
            SQL += "  		 , SUM(DECODE(C.Panic,'P',1,0)) 	  			AS PCnt     -- 13                                                                                                                       \r\n";
            SQL += "  		 , SUM(DECODE(C.DELTA,'D',1,0)) 				AS DCNT     -- 14                                                                                                                       \r\n";
            SQL += "  		 , MAX(DISEASE)			   		    		    AS DIAGNOSYS -- 15                                                                                                                      \r\n";
            SQL += "  		 , TO_CHAR(A.INDATE,'YYYY-MM-DD') 				AS INDATE    -- 16                                                                                                                      \r\n";
            SQL += "  		 , A.DRCODE										AS DRCODE	 -- 17                                                                                                                      \r\n";
            SQL += "  		 , A.BI											AS BI		 -- 19                                                                                                                      \r\n";
            SQL += "  		 , MAX(A.STATUS)										AS STATUS                                                                                                                       \r\n";
            SQL += "  		 , (SELECT COUNT(*)  FROM KOSMOS_OCS.EXAM_VERIFY   WHERE JDATE = TO_DATE('" + strJINDATE + "','YYYY-MM-DD'))  								 	                                 AS CNT_DATE    \r\n";
            SQL += "  		 , (SELECT COUNT(*)  FROM KOSMOS_OCS.EXAM_VERIFY   WHERE JDATE = TO_DATE('" + strJINDATE + "','YYYY-MM-DD') AND STATUS = '2')       		 	                                            AS CNT_DATE_2 \r\n";
            SQL += "  		 , (SELECT COUNT(*)  FROM KOSMOS_OCS.EXAM_VERIFY   WHERE JDATE = TO_DATE('" + strJINDATE + "','YYYY-MM-DD') AND STATUS = '3')       		 	                                            AS CNT_DATE_3 \r\n";
            SQL += "  		 , (SELECT COUNT(*)  FROM KOSMOS_OCS.EXAM_VERIFY   WHERE JDATE BETWEEN TO_DATE('" + strJINDATE.Substring(0, 8) + "01" + "','YYYY-MM-DD') AND LAST_DAY('" + strJINDATE + "')) 	 AS CNT_MONTH   \r\n";
            SQL += "  		 , (SELECT COUNT(*)  FROM KOSMOS_OCS.EXAM_SPECMST  WHERE PANO = A.PANO AND BDATE >= TO_CHAR(A.INDATE,'YYYY-MM-DD') AND STATUS  NOT IN ('00','06') AND WORKSTS NOT IN ('A','T') ) AS CNT_SPECNO	\r\n";
            SQL += "  		 , MAX(E.JUMIN1) || '-' || MAX(E.JUMIN2)	   AS JUMIN                                                                                                                                 \r\n";
            SQL += "  		 , ''   	                                   AS IPD_GBSTS                                                                                                                             \r\n";
            SQL += "  	 FROM KOSMOS_OCS.EXAM_VERIFY 	 A                                                                                                                                                          \r\n";
            SQL += "  	    , KOSMOS_OCS.EXAM_SPECMST    B                                                                                                                                                          \r\n";
            SQL += "  	    , KOSMOS_OCS.EXAM_RESULTC	 C                                                                                                                                                          \r\n";
            SQL += "  	    , KOSMOS_PMPA.BAS_PATIENT    E                                                                                                                                                          \r\n";
            SQL += "  	WHERE 1=1                                                                                                                                                                                   \r\n";

            if (isIPD_MASTER == true)
            {
                SQL += "  	  AND 'V' =    'I'                                                                                                                                                                      \r\n";
            }
            else
            {
                SQL += "  	  AND 'V' =    'V'                                                                                                                                                                      \r\n";
            }

            SQL += "  	  AND A.JDATE   = " + ComFunc.covSqlDate(strJINDATE, false);
            SQL += "  	  AND A.PANO 	= B.PANO                                                                                                                                                                    \r\n";
            SQL += "  	  AND A.PANO	= E.PANO                                                                                                                                                                    \r\n";
            SQL += "  	  AND B.BDATE	>= TRUNC(A.INDATE)                                                                                                                                                          \r\n";
            SQL += "  	  AND B.SPECNO	= C.SPECNO                                                                                                                                                                  \r\n";
            SQL += "  	  AND B.STATUS  NOT IN ('00','06')                                                                                                                                                          \r\n";
            SQL += "  	  AND B.WORKSTS NOT IN ('A','T')                                                                                                                                                            \r\n";
            if (string.IsNullOrEmpty(strPANO) == false)
            {
                SQL += "  	AND ( A.PANO =   " + ComFunc.covSqlstr(strPANO, false);
                SQL += "  	        OR A.PANO =   (SELECT PANO FROM KOSMOS_OCS.EXAM_SPECMST WHERE SPECNO = " + ComFunc.covSqlstr(strPANO, false);
                SQL += "  	    ))                                                                                                                                                                                  \r\n";
            }

            SQL += "  	GROUP BY A.PANO, A.SNAME, TO_CHAR(A.INDATE,'YYYY-MM-DD'), A.WARD, A.ROOM, A.DEPTCODE, A.DRCODE, A.BI, A.AGE, A.SEX                                                                          \r\n";
            SQL += "  )                                                                                                                                                                                             \r\n";
            SQL += "  SELECT                                                                                                                                                                                        \r\n";
            SQL += "  		 CHK                                                                                                                                                                                    \r\n";
            SQL += "  		, PANO                                                                                                                                                                                  \r\n";
            SQL += "  		, SNAME                                                                                                                                                                                 \r\n";
            SQL += "  		, WARDCODE                                                                                                                                                                              \r\n";
            SQL += "  		, TO_CHAR(ROOMCODE) AS ROOMCODE                                                                                                                                                         \r\n";
            SQL += "  		, DEPTCODE                                                                                                                                                                              \r\n";
            SQL += "  		, DR_NM                                                                                                                                                                                 \r\n";
            SQL += "  		, KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('BAS_환자종류',BI) AS BI_NM                                                                                                                          \r\n";
            SQL += "  		, SEX                                                                                                                                                                                   \r\n";
            SQL += "  		, AGE                                                                                                                                                                                   \r\n";            
            SQL += "  		, ( CASE WHEN STATUS = '1' THEN '접수'                                                                                                                                                  \r\n";
            SQL += "  		         WHEN STATUS = '2' THEN '임시'                                                                                                                                                  \r\n";
            SQL += "  		         WHEN STATUS = '3' THEN '완료'                                                                                                                                                  \r\n";
            SQL += "  		         WHEN STATUS = '4' THEN '인쇄'                                                                                                                                                  \r\n";
            SQL += "  		         ELSE  '' END) AS STATUS                                                                                                                                                        \r\n";
            SQL += "  		, TO_CHAR(CNT_SPECNO) AS CNT_SPECNO                                                                                                                                                     \r\n";
            SQL += "  		, TO_CHAR(RCNT      ) AS RCNT                                                                                                                                                           \r\n";
            SQL += "  		, TO_CHAR(PCNT      ) AS PCNT                                                                                                                                                           \r\n";
            SQL += "  		, TO_CHAR(DCNT      ) AS DCNT                                                                                                                                                           \r\n";
            SQL += "  		, DIAGNOSYS                                                                                                                                                                             \r\n";
            SQL += "  		, INDATE                                                                                                                                                                                \r\n";
            SQL += "  		, DRCODE                                                                                                                                                                                \r\n";
            SQL += "  		, BI                                                                                                                                                                                    \r\n";                       
            SQL += "  		, CNT_DATE                                                                                                                                                                              \r\n";
            SQL += "  		, CNT_DATE_2                                                                                                                                                                            \r\n";
            SQL += "  		, CNT_DATE_3                                                                                                                                                                            \r\n";
            SQL += "  		, CNT_MONTH                                                                                                                                                                             \r\n";
            SQL += "  		, JUMIN                                                                                                                                                                                 \r\n";
            SQL += "  		, IPD_GBSTS                                                                                                                                                                             \r\n";
            SQL += "    FROM T                                                                                                                                                                                      \r\n";
            SQL += "  WHERE 1=1                                                                                                                                                                                     \r\n";
            SQL += "   ORDER BY WARDCODE,ROOMCODE,PANO                                                                                                                                                              \r\n";
            
                                                                                                                                                                                                       
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
    }
}
