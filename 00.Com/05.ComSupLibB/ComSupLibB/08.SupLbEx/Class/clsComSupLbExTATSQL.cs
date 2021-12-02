using ComBase; //기본 클래스
using ComDbB; //DB연결
using System;
using System.Data;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name              : ComSupLibB.SupLbEx
    /// File Name               : clsComSupLbExTATSQL.cs
    /// Title or Description    : 진단검사의학과 TAT Biz
    /// Author                  : 김홍록
    /// Create Date             : 2018-02-03
    /// Update History          : 
    /// </summary>
    public class clsComSupLbExTATSQL : Com.clsMethod
    {
        string SQL = string.Empty;

        public enum enmSel_EXAM_RESULTC_TAT_SUM { EXAMCNT, AVG_DIFF, TAT_CNT, TAT_RATE}; 

        public enum enmSel_EXAM_RESULTC_TAT       {       CHK,      SPECNO,       PANO,      SNAME,     DEPTCODE,       STRT,     WARD,     ROOM,       DAY,       EMR_YN,  RECEIVEDATE, RESULTDATE,       DIFF,   TAT_REF,  TAT_VALUE,         SUBCODE,  EXAMNAME,   TAT_DAY,   HOLYDATE,   TAT_MIN_01,   TAT_MIN_02,   TAT_MIN_03,   TAT_MIN_04,     TAT_CASE_CD,       TAT_CASE,   TAT_INPT, DIFF_NUM    , ROWID_B };
        public string[] sSel_EXAM_RESULTC_TAT   = {    "선택",  "검체번호", "등록번호", "환자성명",         "과", "응급여부",   "병동",   "병실",    "요일",        "EMR",   "접수일시", "결과일시", "소요시간", "TAT시간", "TAT 차이",      "검사코드",  "검사명", "TAT_DAY", "HOLYDATE", "TAT_MIN_01", "TAT_MIN_02", "TAT_MIN_03", "TAT_MIN_04",  "지연사유코드",     "지연비고",   "확인자","DIFF_NUM"   , "ROWID_B" };
        public int[] nSel_EXAM_RESULTC_TAT      = { nCol_SCHK,   nCol_PANO,  nCol_PANO,  nCol_PANO, nCol_SCHK+20,   nCol_AGE, nCol_AGE, nCol_AGE, nCol_SCHK, nCol_SCHK+20,    nCol_TIME,  nCol_TIME,  nCol_PANO, nCol_PANO,  nCol_PANO,  nCol_CHEK + 10, nCol_NAME,         5,          5,            5,            5,            5,            5,  nCol_ORDERNAME, nCol_ORDERNAME, nCol_SNAME,        5    ,   5    };

        public enum enmSel_EXAM_MASTER            {       CHK,     WS_NM, MASTERCODE,   EXAMNAME,           SUB,        MOTHER,      RESULTIN,       TONGBUN,    TAT_CLS,        TAT_YN,      TAT_MIN_01,    TAT_ER_YN,        TAT_MIN_02,    TAT_WON_YN,       TAT_MIN_03,       TAT_EM_YN,         TAT_MIN_04,      TAT_HOLD,        TAT_DAY };
        public string[] sSel_EXAM_MASTER_MENU   = {    "선택",      "WS", "검사코드", "검체명칭",    "하위검사",    "묶음검사",    "결과입력",    "통계분류", "TAT 분류",    "일반제외", "일반시간(분)",  "응급실제외", "응급실시간 (분)",  "원거리제외", "원거리시간(분)",  "응급검사제외", "응급검사시간(분)",    "공휴제외", "주간검사여부" };
        public int[] nSel_EXAM_MASTER_MENU      = { nCol_SCHK, nCol_PANO,  nCol_PANO,  nCol_TIME, nCol_AGE + 10, nCol_AGE + 10, nCol_AGE + 10, nCol_AGE + 10,  nCol_PANO, nCol_AGE + 10,      nCol_PANO, nCol_AGE + 20,         nCol_PANO, nCol_AGE + 20,        nCol_PANO,   nCol_AGE + 20,          nCol_PANO, nCol_AGE + 10,  nCol_AGE + 20 };

        public string up_EXAM_RESULTC(PsmhDb pDbCon, string strTAT_CASE_CD, string strTAT_CASE, string strROWID, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_RESULTC                                             \r\n";
            SQL += "    SET TAT_CASE_CD = " + ComFunc.covSqlstr(strTAT_CASE_CD          , false);
            SQL += "      , TAT_CASE    = " + ComFunc.covSqlstr(strTAT_CASE             , false);

            SQL += "      , TAT_INPT    = " + ComFunc.covSqlstr(clsType.User.IdNumber   , false);
            SQL += "      , UPDT        = SYSDATE                                               \r\n";
            SQL += "      , UPPS        = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += "  WHERE 1=1                                                                 \r\n";
            SQL += "    AND ROWID       = " + ComFunc.covSqlstr(strROWID, false);
            
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

        public enum enmSel_EXAM_TAT_TONG {WEEK, WEEK_START, WEEK_END, EXAMCNT, TATCNT, NTIME, AVG_TIME, AVG_TAT };

        public DataTable sel_EXAM_TAT_TONG_ISBUILD(PsmhDb pDbCon, string strYYYY)
        {
            DataTable dt = null;

            SQL = "";

            SQL += "  SELECT TO_CHAR(JOBDATE, 'MM') JOBMONTH    \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_TAT_TONG           \r\n";
            SQL += "   WHERE TO_CHAR(JOBDATE,'YYYY') = " + ComFunc.covSqlstr(strYYYY, false);
            SQL += "  GROUP BY TO_CHAR(JOBDATE, 'MM')           \r\n";
            SQL += "  ORDER BY TO_CHAR(JOBDATE, 'MM')           \r\n";
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

        public DataTable sel_EXAM_TAT_TONG(PsmhDb pDbCon, string strYYYY, string strTAT_CLS, string strTAT_TYPE)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "  WITH T AS(                                                                            \r\n";
            SQL += "  		SELECT                                                                          \r\n";
            SQL += "  		       CEIL((TO_NUMBER(SUBSTRB(TO_CHAR(JOBDATE,'YYYYMMDD'), -2, 2)) + 7 - TO_NUMBER(TO_CHAR(JOBDATE,'D')))/7)  AS WEEK                                         \r\n";
            SQL += "  		     , A.JOBDATE                                                                \r\n";
            SQL += "  		     , A.EXAMCNT                                                                \r\n";
            SQL += "  		     , A.TATCNT                                                                 \r\n";
            SQL += "  		     , A.NTIME                                                                  \r\n";
            SQL += "  		  FROM KOSMOS_OCS.EXAM_TAT_TONG A                                               \r\n";
            SQL += "  		 WHERE JOBDATE BETWEEN TO_DATE('"+ strYYYY + "- 01','YYYY-MM-DD')               \r\n";
            SQL += "  		                   AND LAST_DAY(TO_DATE('" + strYYYY + "-01','YYYY-MM-DD'))     \r\n";
            SQL += "  		   AND BUN      = " + ComFunc.covSqlstr(strTAT_TYPE, false);
            SQL += "  		   AND GUBUN    = " + ComFunc.covSqlstr(strTAT_CLS, false);
            SQL += "  		 ORDER BY JOBDATE                                                               \r\n";
            SQL += "   )                                                                                    \r\n";
            SQL += "   SELECT                                                                               \r\n";
            SQL += "   		'1주'								            AS WEEK                         \r\n";
            SQL += "        , TO_CHAR(MIN(JOBDATE),'YYYY-mm-DD')	        AS WEEK_START                   \r\n";
            SQL += "        , TO_CHAR(MAX(JOBDATE),'YYYY-mm-DD')	        AS WEEK_END                     \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(EXAMCNT),'999,999,999'))		AS EXAMCNT                      \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(EXAMCNT) - SUM(TATCNT),'999,999,999'))  						            AS TATCNT                       \r\n";
            SQL += "        , SUM(NTIME)   						            AS NTIME                        \r\n";
            SQL += "        , DECODE(SUM(NTIME),0,0,ROUND(ROUND(SUM(NTIME) / SUM(EXAMCNT),1))) 	        AS AVG_TIME                     \r\n";
            SQL += "        , DECODE(SUM(NTIME),0,0,ROUND(ROUND(SUM(TATCNT)/ SUM(EXAMCNT) * 100,1))) 	    AS AVG_TAT                      \r\n";
            SQL += "     FROM T                                                                             \r\n";
            SQL += "    WHERE 1=1                                                                           \r\n";
            SQL += "      AND WEEK = '1'                                                                   \r\n";
            SQL += "  UNION ALL                                                                             \r\n";
            SQL += "   SELECT                                                                               \r\n";
            SQL += "   		'2주'								            AS WEEK                         \r\n";
            SQL += "        , TO_CHAR(MIN(JOBDATE),'YYYY-mm-DD')	        AS WEEK_START                   \r\n";
            SQL += "        , TO_CHAR(MAX(JOBDATE),'YYYY-mm-DD')	        AS WEEK_END                     \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(EXAMCNT),'999,999,999'))		AS EXAMCNT                      \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(EXAMCNT) - SUM(TATCNT),'999,999,999'))  						            AS TATCNT                       \r\n";
            SQL += "        , SUM(NTIME)   						            AS NTIME                        \r\n";
            SQL += "        , DECODE(SUM(NTIME),0,0,ROUND(ROUND(SUM(NTIME) / SUM(EXAMCNT),1))) 	        AS AVG_TIME                     \r\n";
            SQL += "        , DECODE(SUM(NTIME),0,0,ROUND(ROUND(SUM(TATCNT)/ SUM(EXAMCNT) * 100,1))) 	    AS AVG_TAT                      \r\n";
            SQL += "     FROM T                                                                             \r\n";
            SQL += "    WHERE 1=1                                                                           \r\n";
            SQL += "      AND WEEK = '2'                                                                   \r\n";
            SQL += "  UNION ALL                                                                             \r\n";
            SQL += "   SELECT                                                                               \r\n";
            SQL += "   		'3주'								            AS WEEK                         \r\n";
            SQL += "        , TO_CHAR(MIN(JOBDATE),'YYYY-mm-DD')	        AS WEEK_START                   \r\n";
            SQL += "        , TO_CHAR(MAX(JOBDATE),'YYYY-mm-DD')	        AS WEEK_END                     \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(EXAMCNT),'999,999,999'))		AS EXAMCNT                      \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(EXAMCNT) - SUM(TATCNT),'999,999,999'))  						            AS TATCNT                       \r\n";
            SQL += "        , SUM(NTIME)   						            AS NTIME                        \r\n";
            SQL += "        , DECODE(SUM(NTIME),0,0,ROUND(ROUND(SUM(NTIME) / SUM(EXAMCNT),1))) 	        AS AVG_TIME                     \r\n";
            SQL += "        , DECODE(SUM(NTIME),0,0,ROUND(ROUND(SUM(TATCNT)/ SUM(EXAMCNT) * 100,1))) 	    AS AVG_TAT                      \r\n";
            SQL += "     FROM T                                                                             \r\n";
            SQL += "    WHERE 1=1                                                                           \r\n";
            SQL += "      AND WEEK = '3'                                                                   \r\n";
            SQL += "  UNION ALL                                                                             \r\n";
            SQL += "   SELECT                                                                               \r\n";
            SQL += "   		'4주'								            AS WEEK                         \r\n";
            SQL += "        , TO_CHAR(MIN(JOBDATE),'YYYY-mm-DD')	        AS WEEK_START                   \r\n";
            SQL += "        , TO_CHAR(MAX(JOBDATE),'YYYY-mm-DD')	        AS WEEK_END                     \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(EXAMCNT),'999,999,999'))		AS EXAMCNT                      \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(EXAMCNT) - SUM(TATCNT),'999,999,999'))  						            AS TATCNT                       \r\n";
            SQL += "        , SUM(NTIME)   						            AS NTIME                        \r\n";
            SQL += "        , DECODE(SUM(NTIME),0,0,ROUND(ROUND(SUM(NTIME) / SUM(EXAMCNT),1))) 	        AS AVG_TIME                     \r\n";
            SQL += "        , DECODE(SUM(NTIME),0,0,ROUND(ROUND(SUM(TATCNT)/ SUM(EXAMCNT) * 100,1))) 	    AS AVG_TAT                      \r\n";
            SQL += "     FROM T                                                                             \r\n";
            SQL += "    WHERE 1=1                                                                           \r\n";
            SQL += "      AND WEEK = '4'                                                                   \r\n";
            SQL += "  UNION ALL                                                                             \r\n";
            SQL += "   SELECT                                                                               \r\n";
            SQL += "   		'5주'								            AS WEEK                         \r\n";
            SQL += "        , TO_CHAR(MIN(JOBDATE),'YYYY-mm-DD')	        AS WEEK_START                   \r\n";
            SQL += "        , TO_CHAR(MAX(JOBDATE),'YYYY-mm-DD')	        AS WEEK_END                     \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(EXAMCNT),'999,999,999'))		AS EXAMCNT                      \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(EXAMCNT) - SUM(TATCNT),'999,999,999'))  						            AS TATCNT                       \r\n";
            SQL += "        , SUM(NTIME)   						            AS NTIME                        \r\n";
            SQL += "        , DECODE(SUM(NTIME),0,0,ROUND(ROUND(SUM(NTIME) / SUM(EXAMCNT),1))) 	        AS AVG_TIME                     \r\n";
            SQL += "        , DECODE(SUM(NTIME),0,0,ROUND(ROUND(SUM(TATCNT)/ SUM(EXAMCNT) * 100,1))) 	    AS AVG_TAT                      \r\n";
            SQL += "     FROM T                                                                             \r\n";
            SQL += "    WHERE 1=1                                                                           \r\n";
            SQL += "      AND WEEK = '5'                                                                   \r\n";
            SQL += "  UNION ALL                                                                             \r\n";
            SQL += "   SELECT                                                                               \r\n";
            SQL += "   		'6주'								            AS WEEK                         \r\n";
            SQL += "        , TO_CHAR(MIN(JOBDATE),'YYYY-mm-DD')	        AS WEEK_START                   \r\n";
            SQL += "        , TO_CHAR(MAX(JOBDATE),'YYYY-mm-DD')	        AS WEEK_END                     \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(EXAMCNT),'999,999,999'))		AS EXAMCNT                      \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(EXAMCNT) - SUM(TATCNT),'999,999,999'))  						            AS TATCNT                       \r\n";
            SQL += "        , SUM(NTIME)   						            AS NTIME                        \r\n";
            SQL += "        , DECODE(SUM(NTIME),0,0,ROUND(ROUND(SUM(NTIME) / SUM(EXAMCNT),1))) 	        AS AVG_TIME                     \r\n";
            SQL += "        , DECODE(SUM(NTIME),0,0,ROUND(ROUND(SUM(TATCNT)/ SUM(EXAMCNT) * 100,1))) 	    AS AVG_TAT                      \r\n";
            SQL += "     FROM T                                                                             \r\n";
            SQL += "    WHERE 1=1                                                                           \r\n";

            SQL += "  ORDER BY WEEK                                                                         \r\n";
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

        public string up_EXAM_MASTER(PsmhDb pDbCon, DataRow dr, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_MASTER        \r\n";
            SQL += "     SET TAT_CLS        = " + ComFunc.covSqlstr(getGubunText(dr[(int)enmSel_EXAM_MASTER.TAT_CLS].ToString().Trim(), "."), false).Trim();
            SQL += "       , TAT_YN         = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.TAT_YN].ToString().Trim() == "True" ? "Y" : "N", false);
            SQL += "       , TAT_MIN_01     = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.TAT_MIN_01].ToString().Trim(), false);
            SQL += "       , TAT_MIN_02     = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.TAT_MIN_02].ToString().Trim(), false);
            SQL += "       , TAT_MIN_03     = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.TAT_MIN_03].ToString().Trim(), false);
            SQL += "       , TAT_MIN_04     = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.TAT_MIN_04].ToString().Trim(), false);
            SQL += "       , TAT_HOLD       = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.TAT_HOLD].ToString().Trim() == "True" ? "Y" : "N", false);
            SQL += "       , TAT_ER_YN      = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.TAT_ER_YN].ToString().Trim() == "True" ? "Y" : "N", false);
            SQL += "       , TAT_EM_YN      = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.TAT_EM_YN].ToString().Trim() == "True" ? "Y" : "N", false);
            SQL += "       , TAT_WON_YN     = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.TAT_WON_YN].ToString().Trim() == "True" ? "Y" : "N", false);            
            SQL += "       , TAT_DAY        = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.TAT_DAY].ToString().Trim() == "True" ? "Y" : "N", false);
            SQL += "       , UPPS           = " + ComFunc.covSqlstr(clsType.User.IdNumber.ToString(), false);
            SQL += "       , UPDT           = SYSDATE   \r\n";
            SQL += "   WHERE MASTERCODE     = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_MASTER.MASTERCODE].ToString().Trim(), false);

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

        public DataSet sel_EXAM_MASTER(PsmhDb pDbCon)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                           \r\n";
            SQL += "        ''                                    			 AS CHK   \r\n";
            SQL += " 	   , KOSMOS_OCS.FC_EXAM_SPECMST_NM('12',WSCODE1,'N') AS WS_NM \r\n";
            SQL += " 	   , MASTERCODE                                               \r\n";
            SQL += " 	   , EXAMNAME                                                 \r\n";
            SQL += "       , SUB                                                      \r\n";
            SQL += "       , MOTHER                                                   \r\n";
            SQL += "       , RESULTIN                                                 \r\n";
            SQL += "       , TONGBUN                                                  \r\n";
            SQL += "       , DECODE(NVL(TRIM(TAT_CLS),'*'),'*','', TAT_CLS || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAME('EXAM_TAT_분류', TAT_CLS)) TAT_CLS                                                \r\n";
            SQL += "       , DECODE(TAT_YN, 'Y', 'True', '')                AS TAT_YN \r\n";
            SQL += "       , TAT_MIN_01                                               \r\n";
            SQL += "       , DECODE(TAT_ER_YN, 'Y', 'True', '')             AS TAT_ER_YN \r\n";
            SQL += "       , TAT_MIN_02                                               \r\n";
            SQL += "       , DECODE(TAT_WON_YN, 'Y', 'True', '')            AS TAT_WON_YN \r\n";
            SQL += "       , TAT_MIN_03                                               \r\n";
            SQL += "       , DECODE(TAT_EM_YN, 'Y', 'True', '')             AS TAT_EM_YN \r\n";
            SQL += "       , TAT_MIN_04                                               \r\n";
            SQL += "       , DECODE(TAT_HOLD, 'Y', 'True', '')              AS TAT_HOLD                                                 \r\n";
            SQL += "       , DECODE(TAT_DAY , 'Y', 'True', '')              AS TAT_HOLD                                                 \r\n";

            SQL += "   FROM KOSMOS_OCS.EXAM_MASTER                                    \r\n";
            SQL += "  WHERE 1=1                                                       \r\n";
            SQL += "  ORDER BY TONGBUN, MASTERCODE                                    \r\n";

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

        string sel_EXAM_RESULTC_TAT(string strFDATE, string strTDATE, string strTAT_CLS, string strTAT_TYPE, bool isBUILD, string strSPECNO)
        {

            SQL = "";
            SQL += "  WITH T AS (                                                                                                                                               \r\n";
            SQL += "  		SELECT                                                                                                                                              \r\n";

            if (isBUILD == true)
            {
                SQL += "  		      TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD')      		                                            AS JOBDATE                                      \r\n";
                SQL += " 	        , TO_CHAR(ROUND( (B.RESULTDATE - A.RECEIVEDATE) * 24 * 60,0))                               AS DIFF                                         \r\n";
                SQL += "            , CASE WHEN '1' = '" + strTAT_TYPE + "' THEN CASE WHEN TO_NUMBER(NVL(TRIM(TAT_MIN_01),'0')) - ROUND( (B.RESULTDATE - A.RECEIVEDATE) * 24 * 60,0) >= 0 THEN '' ELSE '>' END  \r\n";
                SQL += "          	       WHEN '2' = '" + strTAT_TYPE + "' THEN CASE WHEN TO_NUMBER(NVL(TRIM(TAT_MIN_02),'0')) - ROUND( (B.RESULTDATE - A.RECEIVEDATE) * 24 * 60,0) >= 0 THEN '' ELSE '>' END  \r\n";
                SQL += "          	       WHEN '3' = '" + strTAT_TYPE + "' THEN CASE WHEN TO_NUMBER(NVL(TRIM(TAT_MIN_03),'0')) - ROUND( (B.RESULTDATE - A.RECEIVEDATE) * 24 * 60,0) >= 0 THEN '' ELSE '>' END  \r\n";
                SQL += "          	       WHEN '4' = '" + strTAT_TYPE + "' THEN CASE WHEN TO_NUMBER(NVL(TRIM(TAT_MIN_04),'0')) - ROUND( (B.RESULTDATE - A.RECEIVEDATE) * 24 * 60,0) >= 0 THEN '' ELSE '>' END  \r\n";
                SQL += "          	     ELSE '>' END                                                                                                                          AS TAT_VALUE \r\n";
                SQL += "  		      , DECODE(C.TAT_DAY,'Y', TO_CHAR(A.RECEIVEDATE,'HH24:MI'),'12:00') AS TAT_DAY                                                                  \r\n";
                SQL += "  		      , CASE WHEN C.TAT_HOLD = 'Y' AND TO_CHAR(A.RECEIVEDATE,'DY') IN ('SUN', '일요일','일','SAT','토요일','토') THEN 'H'                           \r\n";
                SQL += "  		             WHEN C.TAT_HOLD = 'Y' AND (SELECT HOLYDAY FROM KOSMOS_PMPA.BAS_JOB WHERE JOBDATE = TRUNC(A.RECEIVEDATE)) = '*' THEN 'H'                \r\n";

                //2018/06/14 안정수, 06/13 지방선거 휴일이므로 제외
                //면역혈청일 경우
                if (strTAT_CLS == "06")
                {
                    SQL += "  		             WHEN (SELECT JOBDATE FROM KOSMOS_PMPA.BAS_JOB WHERE JOBDATE = TRUNC(A.RECEIVEDATE)) = TO_DATE('2018-06-13', 'YYYY-MM-DD') THEN 'H'           \r\n";
                }

                SQL += "  		             ELSE 'P' END AS HOLYDATE                                                                                                               \r\n";

            }
            else
            {

                SQL += "  				A.SPECNO                                                                                                                                    \r\n";
                SQL += "  		      , B.PANO                                                                                                                                      \r\n";
                SQL += "  		      , D.SNAME                                                                                                                                     \r\n";
                SQL += "  		      , A.DEPTCODE                                                                                                                                  \r\n";
                SQL += "  		      , A.WARD                                                                                                                                      \r\n";
                SQL += "  		      , A.ROOM                                                                                                                                      \r\n";
                SQL += "  		      , TO_CHAR(A.RECEIVEDATE,'DY')                                                                 AS DAY                                          \r\n";
                SQL += "  		      , KOSMOS_OCS.FC_EMR_TREATT_ISNULL(A.PANO,A.IPDOPD,TO_CHAR(A.BDATE,'YYYYMMDD'), A.DEPTCODE)    AS EMR_YN                                       \r\n";
                SQL += "  		      , TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD')      		                                            AS JOBDATE                                      \r\n";
                SQL += "  		      , TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD HH24:MI')                                                 AS RECEIVEDATE                                  \r\n";
                SQL += "  		      , TO_CHAR(B.RESULTDATE,'YYYY-MM-DD HH24:MI')                                                  AS RESULTDATE                                   \r\n";
                SQL += "  		      , TO_CHAR(ROUND( (B.RESULTDATE - A.RECEIVEDATE) * 24 * 60,0))                                 AS DIFF                                         \r\n";

                SQL += "              , CASE WHEN '1' = '" + strTAT_TYPE + "' THEN CASE WHEN TO_NUMBER(NVL(TRIM(TAT_MIN_01),'0')) - ROUND( (B.RESULTDATE - A.RECEIVEDATE) * 24 * 60,0) >= 0 THEN '' ELSE '>' END  \r\n";
                SQL += "            	     WHEN '2' = '" + strTAT_TYPE + "' THEN CASE WHEN TO_NUMBER(NVL(TRIM(TAT_MIN_02),'0')) - ROUND( (B.RESULTDATE - A.RECEIVEDATE) * 24 * 60,0) >= 0 THEN '' ELSE '>' END  \r\n";
                SQL += "            	     WHEN '3' = '" + strTAT_TYPE + "' THEN CASE WHEN TO_NUMBER(NVL(TRIM(TAT_MIN_03),'0')) - ROUND( (B.RESULTDATE - A.RECEIVEDATE) * 24 * 60,0) >= 0 THEN '' ELSE '>' END  \r\n";
                SQL += "            	     WHEN '4' = '" + strTAT_TYPE + "' THEN CASE WHEN TO_NUMBER(NVL(TRIM(TAT_MIN_04),'0')) - ROUND( (B.RESULTDATE - A.RECEIVEDATE) * 24 * 60,0) >= 0 THEN '' ELSE '>' END  \r\n";
                SQL += "            	     ELSE '>' END                                                                                                                          AS TAT_VALUE                                                                                             \r\n";
                SQL += "              , CASE WHEN '1' = '" + strTAT_TYPE + "' THEN TAT_MIN_01                                                                                                                                                                                              \r\n";
                SQL += "            	     WHEN '2' = '" + strTAT_TYPE + "' THEN TAT_MIN_02                                                                                                                                                                                              \r\n";
                SQL += "            	     WHEN '3' = '" + strTAT_TYPE + "' THEN TAT_MIN_03                                                                                                                                                                                              \r\n";
                SQL += "            	     WHEN '4' = '" + strTAT_TYPE + "' THEN TAT_MIN_04                                                                                                                                                                                              \r\n";
                SQL += "            	     ELSE '' END                                                                            AS TAT_REF                                      \r\n";
                SQL += "  			  , TAT_MIN_01                                                                                                                                  \r\n";
                SQL += "  			  , TAT_MIN_02                                                                                                                                  \r\n";
                SQL += "  			  , TAT_MIN_03                                                                                                                                  \r\n";
                SQL += "  			  , TAT_MIN_04                                                                                                                                  \r\n";
                SQL += "  		      , B.SUBCODE                                                                                                                                   \r\n";
                SQL += "  		      , C.EXAMNAME                                                                                                                                  \r\n";
                SQL += "  		      , DECODE(C.TAT_DAY,'Y', TO_CHAR(A.RECEIVEDATE,'HH24:MI'),'12:00') AS TAT_DAY                                                                  \r\n";

                SQL += "  		      , CASE WHEN C.TAT_HOLD = 'Y' AND TO_CHAR(A.RECEIVEDATE,'DY') IN ('SUN', '일요일','일','SAT','토요일','토') THEN 'H'                           \r\n";
                SQL += "  		             WHEN C.TAT_HOLD = 'Y' AND (SELECT HOLYDAY FROM KOSMOS_PMPA.BAS_JOB WHERE JOBDATE = TRUNC(A.RECEIVEDATE)) = '*' THEN 'H'                \r\n";

                //2018/06/14 안정수, 06/13 지방선거 휴일이므로 제외
                //면역혈청일 경우
                if (strTAT_CLS == "06")
                {
                    SQL += "  		             WHEN (SELECT JOBDATE FROM KOSMOS_PMPA.BAS_JOB WHERE JOBDATE = TRUNC(A.RECEIVEDATE)) = TO_DATE('2018-06-13', 'YYYY-MM-DD') THEN 'H'           \r\n";                    
                }

                SQL += "  		             ELSE 'P' END AS HOLYDATE                                                                                                               \r\n";
                SQL += "              , CASE WHEN A.STRT = 'S' OR A.STRT = 'E' THEN 'E'                                                                                             \r\n";
                SQL += "                     ELSE '' END AS STRT                                                                                                                    \r\n";
                SQL += "  		      , DECODE(NVL(B.TAT_CASE_CD,'^*'),'^*','', B.TAT_CASE_CD || '.' || B.TAT_CASE)               AS TAT_CASE_CD                                    \r\n";
                SQL += "  		      , B.TAT_CASE                                                                                AS TAT_CASE                                       \r\n";
                SQL += "  		      , KOSMOS_OCS.FC_BAS_USER(B.TAT_INPT)                                                        AS TAT_INPT                                       \r\n";
                SQL += "  		      , B.ROWID                                                                                    AS ROWID_B                                        \r\n";

            }

            SQL += "  		FROM  KOSMOS_OCS.EXAM_SPECMST 	A                                                                                                                   \r\n";
            SQL += "  		    , KOSMOS_OCS.EXAM_RESULTC 	B                                                                                                                   \r\n";
            SQL += "  		    , KOSMOS_OCS.EXAM_MASTER 	C                                                                                                                   \r\n";
            SQL += "  		    , KOSMOS_PMPA.BAS_PATIENT 	D                                                                                                                   \r\n";
            SQL += "  		    , KOSMOS_PMPA.BAS_AREA 		E                                                                                                                   \r\n";
            SQL += "  		WHERE 1=1                                                                                                                                           \r\n";

            if (string.IsNullOrEmpty(strSPECNO.Trim()) == false)
            {
                SQL += "  		  AND A.SPECNO = "+ ComFunc.covSqlstr(strSPECNO, false);
            }
            else
            {
                SQL += "  		  AND A.RECEIVEDATE BETWEEN TO_DATE('" + strFDATE + "','YYYY-MM-DD')                                                                                \r\n";
                SQL += "  		  						AND TO_DATE('" + strTDATE + " 23:59','YYYY-MM-DD HH24:MI')                                                                  \r\n";
            }
            SQL += "  		  AND (A.CANCEL IS NULL OR A.CANCEL=' ')                                                                                                            \r\n";
            SQL += "  		  AND A.STATUS	= '05'                                                                                                                              \r\n";
            SQL += "  		  AND A.BI 		NOT IN ('61','62','63','64','81')                                                                                                   \r\n";
            SQL += "  		  AND B.RESULTDATE IS NOT NULL                                                                                                                      \r\n";
            SQL += "                                                                                                                                                            \r\n";
            SQL += "                                                                                                                                                            \r\n";

            if (strTAT_TYPE.Equals("1") == true)
            {
                SQL += "  		 AND A.DEPTCODE !=  'ER'                                                                                                                       \r\n";
                SQL += "  		 AND C.TAT_YN   !=  'Y'                                                                                                                        \r\n";
            }
            else if (strTAT_TYPE.Equals("2") == true)
            {
                SQL += "  		AND A.DEPTCODE = 'ER'                                                                                                                          \r\n";
                SQL += "  		AND C.TAT_ER_YN != 'Y'                                                                                                                          \r\n";
            }
            else if (strTAT_TYPE.Equals("3") == true)
            {
                SQL += "  		AND ( D.JICODE ='63' OR D.JICODE >='76'  )                                                                                                      \r\n";
                SQL += "  		AND C.TAT_WON_YN != 'Y'                                                                                                                          \r\n";
                SQL += "  		AND A.IPDOPD      = 'O'                                                                                                                          \r\n";
            }
            else if (strTAT_TYPE.Equals("4") == true)
            {
                SQL += "  		AND A.STRT      ='S'                                                                                                                           \r\n";
                SQL += "  		AND C.TAT_EM_YN != 'Y'                                                                                                                          \r\n";
            }

            SQL += "  		  AND A.SPECNO	= B.SPECNO                                                                                                                          \r\n";
            SQL += "  		  AND B.SUBCODE = C.MASTERCODE                                                                                                                      \r\n";
            SQL += "  		  AND A.PANO 	= D.PANO                                                                                                                            \r\n";
            SQL += "  		  AND D.JICODE 	= E.JICODE(+)                                                                                                                       \r\n";

            if (string.IsNullOrEmpty(strTAT_CLS.Trim()) == false)
            {
                SQL += "  		  AND C.TAT_CLS = " + ComFunc.covSqlstr(strTAT_CLS, false);
            }
            
            SQL += "  		  ORDER BY A.RECEIVEDATE,A.SPECNO, B.SUBCODE                                                                                                        \r\n";
            SQL += "  )                                                                                                                                                         \r\n";

            return SQL;
        }

        public string del_EXAM_TAT_TONG(PsmhDb pDbCon, string strFDATE, string strTDATE, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = string.Empty;            

            SQL = "";
            SQL += "  DELETE KOSMOS_OCS.EXAM_TAT_TONG     \r\n";
            SQL += "  WHERE 1=1                                                                         \r\n";
            SQL += "    AND JOBDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
            SQL += "                    AND " + ComFunc.covSqlDate(strTDATE, false);
            
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
        
        public string ins_EXAM_TAT_TONG(PsmhDb pDbCon, string strFDATE, string strTDATE, string strTAT_CLS, string strTAT_TYPE, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = string.Empty;
            string strEXAM_RESULTC = sel_EXAM_RESULTC_TAT(strFDATE, strTDATE, strTAT_CLS, strTAT_TYPE,true, "");

            SQL = "";
            SQL += "  INSERT INTO KOSMOS_OCS.EXAM_TAT_TONG (JOBDATE,GUBUN,BUN,EXAMCNT,TATCNT,NTIME)     \r\n";
            SQL += strEXAM_RESULTC;
            SQL += "  SELECT                                                                            \r\n";
            SQL += "  	    JOBDATE							AS JOBDATE                                  \r\n";
            SQL += "  	  , '" + strTAT_CLS  + "'			AS GUBUN                                    \r\n";
            SQL += "  	  , '" + strTAT_TYPE + "'			AS BUN                                      \r\n";
            SQL += "  	  ,	COUNT(*)						AS EXAMCNT                                  \r\n";
            SQL += "  	  , SUM(DECODE(TAT_VALUE,'>',0,1))  AS TATCNT                                   \r\n";
            SQL += "  	  , SUM(DIFF)                       AS NTIME                                    \r\n";
            SQL += "    FROM T                                                                          \r\n";
            SQL += "  WHERE 1=1                                                                         \r\n";
            SQL += "    AND TAT_DAY BETWEEN '08:00'                                                     \r\n";
            SQL += "                    AND '15:00'                                                     \r\n";
            SQL += "    AND HOLYDATE = 'P'                                                              \r\n";
            SQL += "    GROUP BY JOBDATE                                                                \r\n";
            SQL += "    ORDER BY JOBDATE                                                                \r\n";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon,600);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }

            return SqlErr;
        }

        public DataTable sel_EXAM_RESULTC_TAT_SUM(PsmhDb pDbCon, string strFDATE, string strTDATE, string strTAT_CLS, string strTAT_TYPE)
        {
            DataTable dt = null;
            string strEXAM_RESULTC = sel_EXAM_RESULTC_TAT(strFDATE, strTDATE, strTAT_CLS, strTAT_TYPE,false, "");

            SQL = "";

            SQL += strEXAM_RESULTC;

            SQL += "  SELECT                                                                                                                                                    \r\n";
            SQL += "  				COUNT(*)						AS EXAMCNT                                                                                                                                      \r\n";
            SQL += "  		      , ROUND(SUM(DIFF) / COUNT(*),0)	AS AVG_DIFF                                                                                                                                        \r\n";
            SQL += "  		      , SUM(DECODE(TAT_VALUE,'>',0,1))  AS TAT_CNT                                                                                                                                       \r\n";
            SQL += "  		      , ROUND(SUM(DECODE(TAT_VALUE,'>',0,1)) / COUNT(*) * 100,1) AS TAT_RATE                                                                                                                                    \r\n";
            SQL += "    FROM T                                                                                                                                                  \r\n";
            SQL += "  WHERE 1=1                                                                                                                                                 \r\n";
            SQL += "    AND TAT_DAY BETWEEN '08:00'                                                                                                                             \r\n";
            SQL += "                    AND '15:00'                                                                                                                             \r\n";
            SQL += "    AND HOLYDATE = 'P'                                                                                                                                      \r\n";

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

        public DataSet sel_EXAM_RESULTC_TAT(PsmhDb pDbCon, string strFDATE, string strTDATE, string strTAT_CLS, string strTAT_TYPE, string strSPECNO, bool isOver)
        {
            DataSet ds = null;
            string strEXAM_RESULTC = sel_EXAM_RESULTC_TAT(strFDATE, strTDATE, strTAT_CLS, strTAT_TYPE,false, strSPECNO);

            SQL = "";

            SQL += strEXAM_RESULTC;  

            SQL += "  SELECT                                                                                                                                                    \r\n";
            SQL += "  			   	'True' CHK                                                                                                                                  \r\n";
            SQL += "  			  ,	SPECNO                                                                                                                                      \r\n";
            SQL += "  		      , PANO                                                                                                                                        \r\n";
            SQL += "  		      , SNAME                                                                                                                                       \r\n";
            SQL += "  		      , DEPTCODE                                                                                                                                    \r\n";
            SQL += "  		      , STRT                                                                                                                                        \r\n";
            SQL += "  		      , WARD                                                                                                                                        \r\n";
            SQL += "  		      , ROOM                                                                                                                                        \r\n";
            SQL += "  		      , DAY                                                                                                                                         \r\n";
            SQL += "  		      , EMR_YN                                                                                                                                      \r\n";
            SQL += "  		      , RECEIVEDATE                                                                                                                                 \r\n";
            SQL += "  		      , RESULTDATE                                                                                                                                  \r\n";
            SQL += "  		      , DIFF                                                                                                                                        \r\n";
            SQL += "  	   		  , TAT_REF                                                                                                                                     \r\n";
            SQL += "  	   		  , TAT_VALUE                                                                                                                                   \r\n";
            SQL += "  		      , SUBCODE                                                                                                                                     \r\n";
            SQL += "  		      , EXAMNAME                                                                                                                                    \r\n";
            SQL += "  		      , TAT_DAY                                                                                                                                     \r\n";
            SQL += "  		      , HOLYDATE                                                                                                                                    \r\n";
            SQL += "     		  , TAT_MIN_01                                                                                                                                  \r\n";
            SQL += "  			  , TAT_MIN_02                                                                                                                                  \r\n";
            SQL += "  			  , TAT_MIN_03                                                                                                                                  \r\n";
            SQL += "  			  , TAT_MIN_04                                                                                                                                  \r\n";
            SQL += "  			  , TAT_CASE_CD                                                                                                                                 \r\n";
            SQL += "  			  , TAT_CASE                                                                                                                                    \r\n";
            SQL += "  			  , TAT_INPT                                                                                                                                    \r\n";
            SQL += "  			  , TO_NUMBER(DIFF) AS DIFF_NUM                                                                                                                 \r\n";
            SQL += "  			  , ROWID_B                                                                                                                                     \r\n";
            SQL += "    FROM T                                                                                                                                                  \r\n";
            SQL += "  WHERE 1=1                                                                                                                                                 \r\n";
            SQL += "    AND TAT_DAY BETWEEN '08:00'                                                                                                                             \r\n";
            SQL += "                    AND '15:00'                                                                                                                             \r\n";

            if (string.IsNullOrEmpty(strSPECNO) == false || isOver == true)
            {
                SQL += "    AND TAT_VALUE = '>'                                                                                                                                 \r\n";
            }

            if (strTAT_TYPE == "1")
            {
                SQL += "    AND TAT_MIN_01 IS NOT NULL                                                                                                                                      \r\n";
            }
            else if (strTAT_TYPE == "2")
            {
                SQL += "    AND TAT_MIN_02 IS NOT NULL                                                                                                                                      \r\n";
            }

            SQL += "    AND HOLYDATE = 'P'                                                                                                                                      \r\n";
                                                                                                                                                                                
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
      
        public string up_EXAM_SPECMST_STATUS(PsmhDb pDbCon, string strSPECNO, string strJEPSUSABUN, string strJEPSUNAME, string strJEPSUSABUN2, string strJEPSUNAME2, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_SPECMST        \r\n";
            SQL += "     SET RECEIVEDATE    = SYSDATE       \r\n";
            SQL += "       , STATUS         = '01'          \r\n";
            SQL += "       , EMR            = '0'           \r\n";
            SQL += "       , JEPSUSABUN     = " + ComFunc.covSqlstr(strJEPSUSABUN   , false);
            SQL += "       , JEPSUNAME      = " + ComFunc.covSqlstr(strJEPSUNAME    , false);
            SQL += "       , JEPSUSABUN2    = " + ComFunc.covSqlstr(strJEPSUSABUN2  , false);
            SQL += "       , JEPSUNAME2     = " + ComFunc.covSqlstr(strJEPSUNAME2   , false);
            SQL += "   WHERE SPECNO         = " + ComFunc.covSqlstr(strSPECNO       , false);

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
       
        public DataSet sel_EXAM_SPECMST_RCP02(PsmhDb pDbCon, string strRECEIVEDATE, string strJUPSU, string strSPECNO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                                                     \r\n";
            SQL += "      A.IPDOPD                                                                              \r\n";
            SQL += "    , TO_CHAR(RECEIVEDATE,'YYYY-MM-DD HH24:MI')                         AS RECEIVEDATE      \r\n";
            SQL += " 	, KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(A.PANO,A.BDATE)              AS INFECT_INFO      \r\n";
            SQL += " 	, null     												            AS INFECT_IMAGE     \r\n";
            SQL += " 	, KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(A.PANO, A.BDATE, A.DEPTCODE)   AS ERP              \r\n";
            SQL += "    , TO_CHAR(A.WARD)	                                                AS WARD             \r\n";
            SQL += " 	, TO_CHAR(A.ROOM)	                                                AS ROOM             \r\n";
            SQL += " 	, A.SPECNO                                                                      \r\n";
            SQL += " 	, A.PANO                                                                        \r\n";
            SQL += " 	, A.SNAME                                                                       \r\n";
            SQL += " 	, A.SEX                                                                         \r\n";
            SQL += " 	, A.AGE                                                                         \r\n";
            SQL += " 	, A.JEPSUNAME                                                                   \r\n";
            SQL += " 	, A.JEPSUNAME2                                                                  \r\n";
            SQL += " 	, KOSMOS_OCS.FC_EXAM_RESULTC_EXAMNAME(A.SPECNO)			AS EXAM_NAME            \r\n";
            SQL += " 	, DECODE(IPDOPD,'I',KOSMOS_OCS.FC_OCS_IORDER_CANCEL(A.ORDERNO, A.PANO),'') AS IOCS_CANCEL            \r\n";
            SQL += " 	, STATUS                                                                        \r\n";
            SQL += " 	, BDATE                                                                         \r\n";
            SQL += " 	, DEPTCODE                                                                      \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_SPECMST A                                                    \r\n";
            SQL += " WHERE 1=1                                                                          \r\n";
            SQL += "   AND 'J'      = " + ComFunc.covSqlstr(strJUPSU, false);
            SQL += "   AND A.SPECNO = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += " UNION ALL                                                                          \r\n";
            SQL += " SELECT                                                                             \r\n";
            SQL += "      A.IPDOPD                                                                      \r\n";
            SQL += "    , TO_CHAR(RECEIVEDATE,'YYYY-MM-DD HH24:MI') AS RECEIVEDATE                      \r\n";
            SQL += " 	, KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG(A.PANO,A.BDATE)  AS INFECT_INFO          \r\n";
            SQL += " 	, null     												AS INFECT_IMAGE         \r\n";
            SQL += " 	, KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(A.PANO, A.BDATE, A.DEPTCODE)   AS ERP              \r\n";
            SQL += "    , TO_CHAR(A.WARD)	AS WARD                                                     \r\n";
            SQL += " 	, TO_CHAR(A.ROOM)	AS ROOM                                                     \r\n";
            SQL += " 	, A.SPECNO                                                                      \r\n";
            SQL += " 	, A.PANO                                                                        \r\n";
            SQL += " 	, A.SNAME                                                                       \r\n";
            SQL += " 	, A.SEX                                                                         \r\n";
            SQL += " 	, A.AGE                                                                         \r\n";            
            SQL += " 	, A.JEPSUNAME                                                                   \r\n";
            SQL += " 	, A.JEPSUNAME2                                                                  \r\n";
            SQL += " 	, KOSMOS_OCS.FC_EXAM_RESULTC_EXAMNAME(A.SPECNO)			AS EXAM_NAME            \r\n";
            SQL += " 	, DECODE(IPDOPD,'I',KOSMOS_OCS.FC_OCS_IORDER_CANCEL(A.ORDERNO, A.PANO),'') AS IOCS_CANCEL            \r\n";
            SQL += " 	, STATUS                                                                        \r\n";
            SQL += " 	, BDATE                                                                         \r\n";
            SQL += " 	, DEPTCODE                                                                      \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_SPECMST A                                                    \r\n";
            SQL += " WHERE 1=1                                                                          \r\n";
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

    }
}
