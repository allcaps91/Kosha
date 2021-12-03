using ComLibB;
using System;
using System.Data;

using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;  //이 참조는 필요없음

namespace ComSupLibB.SupPthl
{
    /// <summary>
    /// Class Name : ComSupLibB.SupPthl
    /// File Name : clsPthlRcpSQL.cs
    /// Title or Description : 조직병리 접수 SQL
    /// Author : 김홍록
    /// Create Date : 2017-06-07
    /// Update History : 
    /// </summary>
    public class clsPthlSlidSQL : Com.clsMethod
    { 
        string SQL = string.Empty; 

        public enum enmSel_ETC_SLIDE_MST     {      BDATE,       PANO,      SNAME,    ACTDATE,   ORDERNO,  ROWID_R };
        public string[] sSel_ETC_SLIDE_MST = { "처방일자", "등록번호", "환자성명", "수납일자", "ORDERNO", "ROWID_R" };
        public int   [] nSel_ETC_SLIDE_MST = {  nCol_DATE,  nCol_PANO, nCol_SNAME,  nCol_DATE, nCol_DATE,     5  };

        public enum enmSel_ETC_SLIDE_COPY     {     ANATNO,      GUBUN,    EXAM_NM,    SPECNO,  RESULT };
        public string[] sSel_ETC_SLIDE_COPY = { "병리번호",     "구분",   "검사명",  "SPECNO", "RESULT"};
        public int[] nSel_ETC_SLIDE_COPY    = {  nCol_PANO,  nCol_PANO,  nCol_JUSO, nCol_DATE,       5 };

        public enum enmSel_ETC_SLIDE_RNTL     {      BDATE,     ANATNO,        BLOCK,    SPECNO,       PANO,      SNAME,    PT_RLTN,       CTPN,   MOVE_HSPT,    RNTL_ID,    RNTL_DY,   RTPS_ID ,    RTRN_DY,         REMARK, ROWID_R ,       CHK };
        public string[] sSel_ETC_SLIDE_RNTL = { "처방일자", "병리번호",   "세부사항","검체번호", "등록번호", "환자성명",     "관계",   "연락처",  "이동병원",   "대출자", "대출일자",   "반납자", "반납일자",         "비고","ROWID_R",    "변경" };
        public int[] nSel_ETC_SLIDE_RNTL    = {  nCol_DATE,  nCol_PANO,  nCol_AGE+10, nCol_PANO,  nCol_PANO, nCol_SNAME, nCol_SNAME, nCol_SNAME,  nCol_SNAME, nCol_SNAME, nCol_SNAME, nCol_SNAME, nCol_SNAME, nCol_ORDERNAME, 5, nCol_SCHK };

        public string up_ETC_SLIDE_COPY(PsmhDb pDbCon, string PT_RLTN, string CTPN, string MOVE_HSPT, string RNTL_ID, string RNTL_DY, string RTRN_DY, string RTPS_ID, string REMARK, string ROWID, string BLOCK, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " UPDATE KOSMOS_PMPA.ETC_SLIDE_COPY    \r\n";

            SQL += " SET PT_RLTN   = " + ComFunc.covSqlstr(PT_RLTN  , false);      /* 환자관계*/
            SQL += "   , CTPN      = " + ComFunc.covSqlstr(CTPN     , false);         /* 연락처*/
            SQL += "   , MOVE_HSPT = " + ComFunc.covSqlstr(MOVE_HSPT, false);    /* 이동병원*/
            SQL += "   , RNTL_ID   = " + ComFunc.covSqlstr(RNTL_ID  , false);      /* 대여자*/

            if (string.IsNullOrEmpty(RNTL_DY) == false)
            {
                SQL += "   , RNTL_DY   = " + ComFunc.covSqlDate(RNTL_DY, false);     /* 대여일자*/
            }
            else
            {
                SQL += "   , RNTL_DY   = '' \r\n";
            }

            if (string.IsNullOrEmpty(RTRN_DY) == false)
            {
                SQL += "   , RTRN_DY   = " + ComFunc.covSqlDate(RTRN_DY, false);     /* 반납일자*/
            }
            else
            {
                SQL += "   , RTRN_DY   = '' \r\n";
            }

           
            SQL += "   , RTPS_ID   = " + ComFunc.covSqlstr (RTPS_ID , false);      /* 반납자*/

            SQL += "   , REMARK    = " + ComFunc.covSqlstr(REMARK, false);	   /* 비고 */
            SQL += "   , BLOCK     = " + ComFunc.covSqlstr(BLOCK, false);	   /* 비고 */
            SQL += "   , UPPS      = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);                                  /* 수정자*/
            SQL += "   , UPDT      = SYSDATE                     \r\n";            
            SQL += "    WHERE 1 = 1                              \r\n";
            SQL += "      AND ROWID  = " + ComFunc.covSqlstr(ROWID, false);	/* */

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

        public string up_ETC_SLIDE_MST(PsmhDb pDbCon, string strROWID, bool isCancel, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " UPDATE KOSMOS_PMPA.ETC_SLIDE_MST    \r\n";

            if (isCancel == true)
            {
                SQL += " SET JDATE      = TRUNC(SYSDATE)   \r\n";
                SQL += "   , JSABUNOLD  = JSABUN           \r\n";                
                SQL += "   , JSABUN     = ''               \r\n";
                SQL += "   , CDATE      = TRUNC(SYSDATE)   \r\n";
                SQL += "   , CSABUN     = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            }
            else
            {
                SQL += " SET JDATE      = TRUNC(SYSDATE)     \r\n";
                SQL += "   , JSABUN = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            }

            SQL += "    WHERE 1 = 1                              \r\n";
            SQL += "      AND ROWID  = " + ComFunc.covSqlstr(strROWID, false);	/* */

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

        public DataSet sel_ETC_SLIDE_RNTL(PsmhDb pDbCon, string strFDATE, string strTDATE)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += "  SELECT                                                           \r\n";
            SQL += "  		TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE	    -- 처방일자    \r\n";
            SQL += "  	  , A.ANATNO					  AS ANATNO     -- 병리번호    \r\n";
            SQL += "  	  , A.BLOCK					      AS BLOCK      -- 블럭정보    \r\n";
            SQL += "  	  , A.SPECNO					  AS SPECNO     -- 검체번호    \r\n";
            SQL += "  	  , A.PANO            			  AS PANO		-- 환자번호    \r\n";
            SQL += "  	  , A.SNAME           			  AS SNAME	    -- 환자성명    \r\n";
            SQL += "  	  , A.PT_RLTN         			  AS PT_RLTN	-- 관계        \r\n";
            SQL += "  	  , A.CTPN            			  AS CTPN		-- 연락처      \r\n";
            SQL += "  	  , A.MOVE_HSPT       			  AS MOVE_HSPT  -- 이동병원    \r\n";
            SQL += "  	  , A.RNTL_ID         			  AS RNTL_ID	-- 대여자      \r\n";
            SQL += "  	  , TRUNC(A.RNTL_DY) 			  AS RNTL_DY    -- 대여일자    \r\n";
            SQL += "  	  , A.RTPS_ID         			  AS RTPS_ID    -- 반환자      \r\n";
            SQL += "  	  , TRUNC(A.RTRN_DY)			  AS RTRN_DY    -- 반환일자    \r\n";
            SQL += "  	  , A.REMARK        			  AS REMARK     -- 비고        \r\n";
            SQL += "  	  , A.ROWID     				  AS ROWID_R    -- ROWID       \r\n";
            SQL += "  	  , ''           				  AS CHK        -- CHK         \r\n";
            SQL += "    FROM KOSMOS_PMPA.ETC_SLIDE_COPY A                              \r\n";
            SQL += "       , KOSMOS_PMPA.ETC_SLIDE_MST  B                              \r\n";
            SQL += "   WHERE 1=1                                                       \r\n";
            SQL += "     AND A.ORDERNO = B.ORDERNO                                     \r\n";
            SQL += "     AND A.BDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
            SQL += "                     AND " + ComFunc.covSqlDate(strTDATE, false);
            SQL += "     AND B.JDATE   IS NOT NULL                                     \r\n";
            SQL += "     AND A.DELDATE IS     NULL                                     \r\n";
            SQL += "  ORDER BY BDATE,ANATNO                                            \r\n";

            try
            {
                SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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

        public DataSet sel_ETC_SLIDE_COPY(PsmhDb pDbCon, string strPANO, string strACTDATE)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                                                           \r\n";
            SQL += " 	  A.ANATNO                                      AS ANATNO                     \r\n";
            SQL += " 	, DECODE(A.GUBUN,'0','0.염색','1.비염색')       AS GUBUN                      \r\n"; 
            SQL += " 	, A.NAME								        AS EXAM_NM                    \r\n";
            SQL += " 	, A.SPECNO                                      AS SPECNO                     \r\n";            
            SQL += " 	, to_clob(B.RESULT1) || CHR(10) || CHR(13) || to_clob(B.RESULT2)  AS RESULT   \r\n";
            SQL += "  FROM KOSMOS_PMPA.ETC_SLIDE_COPY 	A                                             \r\n";
            SQL += "     , KOSMOS_OCS.EXAM_ANATMST 		B                                             \r\n";
            SQL += " WHERE 1=1                                                                        \r\n";
            SQL += "   AND A.ANATNO     = B.ANATNO                                                    \r\n";
            SQL += "   AND A.PANO 	    = " + ComFunc.covSqlstr(strPANO     , false);
            SQL += "   AND A.ACTDATE 	= " + ComFunc.covSqlDate(strACTDATE , false);
            SQL += "   AND A.DELDATE IS NULL                                                          \r\n"; 
                                                                        
            try
            {
                SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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

        public DataSet sel_ETC_SLIDE_MST(PsmhDb pDbCon, string strFDate, string strTDate, bool isREAD)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += "  SELECT                                            \r\n";                        
            SQL += " 	   TO_CHAR(A.BDATE,'YYYY-MM-DD')   AS BDATE     \r\n";
            SQL += " 	,  A.PANO                          AS PANO      \r\n";
            SQL += " 	,  A.SNAME                         AS SNAME     \r\n";
            SQL += " 	,  TO_CHAR(A.ACTDATE,'YYYY-MM-DD') AS ACTDATE   \r\n";
            SQL += " 	,  A.ORDERNO                       AS ORDERNO   \r\n";
            SQL += " 	,  A.ROWID                         AS ROWID_R   \r\n";
            SQL += "   FROM KOSMOS_PMPA.ETC_SLIDE_MST  A                \r\n";
            //SQL += "      , KOSMOS_PMPA.ETC_SLIDE_COPY B                \r\n";
            SQL += "  WHERE 1=1                                         \r\n";
            //SQL += "    AND A.ORDERNO = B.ORDERNO                       \r\n";
            SQL += "    AND A.BDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "                    AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "    AND A.DELDATE IS NULL                                                           \r\n";

            if (isREAD == true)
            {
                SQL += "    AND A.JDATE   IS NULL                                                       \r\n";
            }
            else
            {
                SQL += "    AND A.JDATE   IS NOT NULL                                                   \r\n";
            }

            SQL += "  GROUP BY A.BDATE,A.PANO, A.SNAME, A.ACTDATE,A.ORDERNO, A.ROWID                    \r\n";
            SQL += "  ORDER BY A.BDATE, A.PANO                                                           \r\n";

            try
            {
                SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

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
