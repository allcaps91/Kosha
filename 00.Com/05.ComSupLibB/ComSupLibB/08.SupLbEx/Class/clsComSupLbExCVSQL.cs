using ComBase; //기본 클래스
using ComDbB; //DB연결
using System;
using System.Collections.Generic;
using System.Data;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name : ComSupLibB.SupLbEx
    /// File Name : clsComSupLbExSMSSQL.cs
    /// Title or Description : 진단검사의학과 SQL
    /// Author : 김홍록
    /// Create Date : 2017-05-15
    /// Update History : 
    /// </summary> 

    public class clsComSupLbExCVSQL : Com.clsMethod
    {
        string SQL = string.Empty;

        /// <summary>2017.05.30.김홍록:CV체크</summary>
        public enum enmSelExamResultcCvMsg { SABUN, SUBCODE, ROWID, MSG, PANO, HTEL, GBN, IPDOPD, TEL };

        public enum enmSel_EXAM_SMS        { MSG , SEND_TEL, EXAM_TEL,PANO }; 

        public enum enmSel_EXAM_RESULTC_CV_LIST      {   SUMTITLE,          PANO,         SNAME,     WARD,     SPECNO,    SUBCODE, EXAMFNAME,    RESULT,     UNIT,  RESULTDATE,    CHKDATE,       TIME,   CHK_TIME, RESULTSABUN_NM,    CHKSABUN_NM,     CHKSABUN  };
        public string[] sSel_EXAM_RESULTC_CV_LIST =  { "SUMTITLE",    "등록번호",    "환자성명",   "병동", "검체번호", "검사코드",  "검사명",    "결과",   "단위",  "결과일시", "확인일시", "소요시간", "소요결과",       "보고자",       "확인자", "확인자사번"  };
        public int[] nSel_EXAM_RESULTC_CV_LIST     = {          5,  nCol_SPNO-10,  nCol_SPNO-10, nCol_AGE,  nCol_SPNO,   nCol_AGE, nCol_NAME, nCol_NAME, nCol_AGE,   nCol_TIME,  nCol_TIME,  nCol_TIME,   nCol_AGE,   nCol_PANO-10,   nCol_PANO-10,    nCol_PANO-10 };

        public DataSet sel_EXAM_RESULTC_CV_LIST(PsmhDb pDbCon, string strFDATE, string strTDATE, string strGBN, string strGBIO, string strWARD, string strWS)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  WITH T AS                                                                                                                                                                                                                 \r\n";
            SQL += "  (                                                                                                                                                                                                                         \r\n";
            SQL += "     SELECT                                                                                                                                                                                                                 \r\n";
            SQL += "     		DISTINCT  B.PANO                                                                                                                                                                                                        \r\n";
            SQL += "     		, B.SNAME                                                                                                                                                                                                       \r\n";
            SQL += "     		, B.WARD                                                                                                                                                                                                        \r\n";
            SQL += "     		, B.SPECNO                                                                                                                                                                                                      \r\n";
            SQL += "     		, A.SUBCODE                                                                                                                                                                                                     \r\n";
            SQL += "     		, C.EXAMFNAME                                                                                                                                                                                                   \r\n";
            SQL += "     		, A.RESULT                                                                                                                                                                                                      \r\n";
            SQL += "     		, KOSMOS_OCS.FC_EXAM_SPECMST_NM('20',C.UNITCODE,'N')                        AS UNIT                                                                                                                             \r\n";
            SQL += "     		, TO_CHAR(B.RESULTDATE,'YYYY-MM-DD HH24:MI') 								AS RESULTDATE                                                                                                                       \r\n";
            SQL += "     		, TO_CHAR(A.CHKDATE,'YYYY-MM-DD HH24:MI')   								AS CHKDATE                                                                                                                          \r\n";
            SQL += "    		, (A.CHKDATE - A.RESULTDATE) * 1440											AS TIME                                                                                                                             \r\n";
            SQL += "     		, TO_CHAR(TRUNC(((TO_DATE(TO_CHAR(A.CHKDATE,'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI') - TO_DATE(TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI')) * 1440) / 1440))			AS DAY          \r\n";
            SQL += "     		, TO_CHAR(TRUNC(MOD(((TO_DATE(TO_CHAR(A.CHKDATE,'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI') - TO_DATE(TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI')) * 1440), 1440)/60)) 	AS HH           \r\n";
            SQL += "  		, TO_CHAR(ROUND(MOD(MOD(((TO_DATE(TO_CHAR(A.CHKDATE,'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI') - TO_DATE(TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI'),'YYYY-MM-DD HH24:MI')) * 1440),1440),60),1)) 	AS MIN          \r\n";
            SQL += "     		, TO_CHAR(KOSMOS_OCS.FC_BAS_USER_NAME(A.RESULTSABUN))	 					AS RESULTSABUN_NM                                                                                                                   \r\n";
            SQL += "     		, TO_CHAR(KOSMOS_OCS.FC_BAS_USER_NAME(A.CHKSABUN))				        	AS CHKSABUN_NM                                                                                                                      \r\n";
            SQL += "  		, TO_CHAR(A.CHKSABUN)														    AS CHKSABUN                                                                                                                         \r\n";
            //SQL += "     		, A.ROWID		                                                            AS ROWID_A                                                                                                                          \r\n";
            SQL += "      FROM  KOSMOS_OCS.EXAM_RESULTC_CV 	A                                                                                                                                                                                   \r\n";
            SQL += "      	, KOSMOS_OCS.EXAM_SPECMST 		B                                                                                                                                                                                   \r\n";
            SQL += "      	, KOSMOS_OCS.EXAM_MASTER 		C                                                                                                                                                                                   \r\n";
            SQL += "     WHERE 1=1                                                                                                                                                                                                              \r\n";
            SQL += "       AND JOBDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
            SQL += "                       AND " + ComFunc.covSqlDate(strTDATE, false);
            SQL += "       AND A.SPECNO     =   B.SPECNO(+)                                                                                                                                                                                     \r\n";
            SQL += "       AND A.SUBCODE    =   C.MASTERCODE(+)                                                                                                                                                                                 \r\n";
            SQL += "       AND A.GBN        = " + ComFunc.covSqlstr(strGBN, false);
            //2018-10-08 안정수 추가
            SQL += "       AND A.RESULT IS NOT NULL \r\n";
            SQL += "       AND A.RESULTDATE IS NOT NULL \r\n";

            if (string.IsNullOrEmpty(strGBIO) == false && strGBIO.Equals("*") == false)
            {
                SQL += "       AND B.IPDOPD        = " + ComFunc.covSqlstr(strGBIO, false);
            }

            if (string.IsNullOrEmpty(strWARD) == false && strWARD.Equals("*") == false)
            {
                SQL += "       AND B.WARD        = " + ComFunc.covSqlstr(strWARD, false);
            }

            if (string.IsNullOrEmpty(strWS) == false && strWS.Equals("*") == false)
            {

                SQL += "       AND C.WSCODE1    IN (SELECT CODE                     \r\n";
                SQL += "                          FROM KOSMOS_OCS.EXAM_SPECODE  \r\n";
                SQL += "       				     WHERE GUBUN = '12'             \r\n";
                SQL += "                           AND YNAME = " + ComFunc.covSqlstr(strWS, false);
                SQL += "                           AND DELDATE IS NULL          \r\n";
                SQL += "                        )                               \r\n";
            }

            SQL += "     ORDER BY RESULTDATE ASC                                                                                                                                                                                                \r\n";
            SQL += "     )                                                                                                                                                                                                                      \r\n";
            SQL += "     SELECT '0' AS SUMTITLE                                                                                                                                                                                                 \r\n";
            SQL += "      , PANO                                                                                                                                                                                                                \r\n";
            SQL += "  	, SNAME                                                                                                                                                                                                                 \r\n";
            SQL += "  	, WARD                                                                                                                                                                                                                  \r\n";
            SQL += "  	, SPECNO                                                                                                                                                                                                                \r\n";
            SQL += "  	, SUBCODE                                                                                                                                                                                                               \r\n";
            SQL += "  	, EXAMFNAME                                                                                                                                                                                                             \r\n";
            SQL += "  	, RESULT                                                                                                                                                                                                                \r\n";
            SQL += "  	, UNIT                                                                                                                                                                                                                  \r\n";
            SQL += "  	, RESULTDATE                                                                                                                                                                                                            \r\n";
            SQL += "  	, CHKDATE                                                                                                                                                                                                               \r\n";
            SQL += "  	, DECODE(NVL(DAY,'0'),'0','',DAY || '일') || DECODE(NVL(HH,'0'),'0','',HH || '시') || DECODE(NVL(MIN,'0'),'0','',MIN || '분') AS TIME                                                                                   \r\n";
            SQL += "  	, CASE WHEN TIME < 31 THEN '< 30'                                                                                                                                                                                       \r\n";
            SQL += "  	       WHEN TIME < 61 THEN '< 60' ELSE '' END AS CHK_TIME                                                                                                                                                               \r\n";
            SQL += "  	, RESULTSABUN_NM                                                                                                                                                                                                        \r\n";
            SQL += "  	, CHKSABUN_NM                                                                                                                                                                                                           \r\n";
            SQL += "  	, CHKSABUN                                                                                                                                                                                                              \r\n";
            SQL += "       FROM T                                                                                                                                                                                                               \r\n";
            SQL += "     UNION                                                                                                                                                                                                                  \r\n";
            SQL += "     SELECT                                                                                                                                                                                                                 \r\n";
            SQL += "     '1' AS SUMTITLE                                                                                                                                                                                                        \r\n";
            SQL += "      , '' PANO                                                                                                                                                                                                             \r\n";
            SQL += "  	, '' SNAME                                                                                                                                                                                                              \r\n";
            SQL += "  	, '' WARD                                                                                                                                                                                                               \r\n";
            SQL += "  	, '' SPECNO                                                                                                                                                                                                             \r\n";
            SQL += "  	, '' SUBCODE                                                                                                                                                                                                            \r\n";
            SQL += "  	, '** 소  계 **' AS EXAMFNAME                                                                                                                                                                                           \r\n";
            SQL += "  	, '' RESULT                                                                                                                                                                                                             \r\n";
            SQL += "  	, '' UNIT                                                                                                                                                                                                               \r\n";
            SQL += "  	, TO_CHAR(COUNT(*)) || ' 건' RESULTDATE                                                                                                                                                                                 \r\n";
            SQL += "  	, '평균시간(분)' CHKDATE                                                                                                                                                                                                \r\n";
            SQL += "  	, TRUNC((SUM(TIME)/COUNT(*))/1440) || '일' || TRUNC(MOD(SUM(TIME)/COUNT(*),1440)/60) || '시' || TRUNC(MOD(MOD(SUM(TIME)/COUNT(*),1440),60)) || '분' AS TIME                                                             \r\n";
            SQL += "  	, ''AS CHK_TIME                                                                                                                                                                                                         \r\n";
            SQL += "  	, '' RESULTSABUN_NM                                                                                                                                                                                                     \r\n";
            SQL += "  	, '' CHKSABUN_NM                                                                                                                                                                                                        \r\n";
            SQL += "  	, '' CHKSABUN                                                                                                                                                                                                           \r\n";
            SQL += "       FROM T                                                                                                                                                                                                               \r\n";
            SQL += "  UNION                                                                                                                                                                                                                     \r\n";
            SQL += "  SELECT                                                                                                                                                                                                                    \r\n";
            SQL += "     '2' AS SUMTITLE                                                                                                                                                                                                        \r\n";
            SQL += "      , '' PANO                                                                                                                                                                                                             \r\n";
            SQL += "  	, '' SNAME                                                                                                                                                                                                              \r\n";
            SQL += "  	, '' WARD                                                                                                                                                                                                               \r\n";
            SQL += "  	, '' SPECNO                                                                                                                                                                                                             \r\n";
            SQL += "  	, '' SUBCODE                                                                                                                                                                                                            \r\n";
            SQL += "  	, '' AS EXAMFNAME                                                                                                                                                                                                       \r\n";
            SQL += "  	, '' RESULT                                                                                                                                                                                                             \r\n";
            SQL += "  	, '' UNIT                                                                                                                                                                                                               \r\n";
            SQL += "  	, '' RESULTDATE                                                                                                                                                                                                         \r\n";
            SQL += "  	, '30분(' || SUM( CASE WHEN TIME < 31 THEN 1 END) || '/' || COUNT(*) || '건, ' || TRUNC((SUM( CASE WHEN TIME < 31 THEN 1 END) / COUNT(*)) * 100) || '%)' 	AS CHKDATE                                                  \r\n";
            SQL += "  	, '' AS TIME                                                                                                                                                                                                            \r\n";
            SQL += "  	, '' AS CHK_TIME                                                                                                                                                                                                        \r\n";
            SQL += "  	, '1시간(' || SUM( CASE WHEN TIME < 61 THEN 1 END) || '/' || COUNT(*) || '건, ' || TRUNC((SUM( CASE WHEN TIME < 61 THEN 1 END) / COUNT(*)) * 100) || '%)' AS RESULTSABUN_NM                                             \r\n";
            SQL += "  	, '' CHKSABUN_NM                                                                                                                                                                                                        \r\n";
            SQL += "  	, '' CHKSABUN                                                                                                                                                                                                           \r\n";
            SQL += "       FROM T                                                                                                                                                                                                               \r\n";
            SQL += "      ORDER BY SUMTITLE, RESULTDATE                                                                                                                                                                                         \r\n";
                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
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

        public string ins_EXAM_INFECT_MASTER(PsmhDb pDbCon, string strSPECNO, string strPANO, string GUBUN, string EXNAME, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  INSERT INTO KOSMOS_OCS.EXAM_INFECT_MASTER ( RDATE,  PANO, GUBUN, SPECNO, EXNAME, CODE  ) VALUES ( \r\n";
            SQL += "     TRUNC(SYSDATE)     \r\n ";
            SQL += "    " + ComFunc.covSqlstr(strPANO, true);
            SQL += "    " + ComFunc.covSqlstr(GUBUN, true);
            SQL += "    " + ComFunc.covSqlstr(strSPECNO, true);
            SQL += "    " + ComFunc.covSqlstr(EXNAME, true);
            SQL += "   , ( SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN = 'INFACT_격리상세질환' AND NAME = '" + EXNAME + "')    \r\n";
            SQL += "   ) ";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 100);

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

        public string del_EXAM_INFECT_MASTER(PsmhDb pDbCon, string strSPECNO, string strPANO, string GUBUN, string EXNAME, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  DELETE KOSMOS_OCS.EXAM_INFECT_MASTER        \r\n";
            SQL += "   WHERE SPECNO = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "     AND PANO   = " + ComFunc.covSqlstr(strPANO, false);
            SQL += "     AND GUBUN  = " + ComFunc.covSqlstr(GUBUN, false);
            SQL += "     AND EXNAME = " + ComFunc.covSqlstr(EXNAME, false);

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

        /// <summary>sel_EXAM_RESUlTC_CV_AFB</summary>
        /// <param name="strSpecNO"></param>
        /// <returns></returns>
        public DataTable sel_EXAM_RESUlTC_CV_AFB(PsmhDb pDbCon, string strSpecNO, string strAFBResult ="")
        {

            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                                                                                 \r\n";
            SQL += "        A.PANO                                                                                                                                                          \r\n";

            if(strAFBResult != "")
            {
                SQL += "      , CASE WHEN A.IPDOPD = 'I'  THEN '★호흡기결핵★입원/' || A.WARD || '/' || A.PANO || '/' || TRIM(A.SNAME) || '/' || TRIM(D.EXAMYNAME) || '/' || '" + strAFBResult.Trim() + "' \r\n";
                SQL += "             ELSE                      '★호흡기결핵★외래/' || A.PANO || '/' || TRIM(A.SNAME) || '/' || TRIM(D.EXAMYNAME) || '/' || '" + strAFBResult.Trim() + "' \r\n";
            }
            else
            {
                SQL += "      , CASE WHEN A.IPDOPD = 'I'  THEN '★호흡기결핵★입원/' || A.WARD || '/' || A.PANO || '/' || TRIM(A.SNAME) || '/' || TRIM(D.EXAMYNAME) || '/' || TRIM(C.RESULT)    \r\n";
                SQL += "             ELSE                      '★호흡기결핵★외래/' || A.PANO || '/' || TRIM(A.SNAME) || '/' || TRIM(D.EXAMYNAME) || '/' || TRIM(C.RESULT)                     \r\n";
            }            

            SQL += "        END AS MSG                                                                                                                                                      \r\n";
            SQL += "      , C.SPECNO                                                                                                                                                        \r\n";
            SQL += "      , B.SABUN                                                                                                                                                         \r\n";
            SQL += "      , I.HTEL                                                                                                                                                          \r\n";
            SQL += "      , A.IPDOPD                                                                                                                                                        \r\n";
            SQL += " FROM " + ComNum.DB_MED + "EXAM_SPECMST A                                                                                                                                         \r\n";
            SQL += "    , " + ComNum.DB_MED + "EXAM_RESULTC C                                                                                                                                         \r\n";
            SQL += "    , " + ComNum.DB_MED + "EXAM_MASTER  D                                                                                                                                         \r\n";
            SQL += "    , " + ComNum.DB_MED + "OCS_DOCTOR   B                                                                                                                                         \r\n";
            SQL += "    , " + ComNum.DB_ERP + "INSA_MST     I                                                                                                                                         \r\n";
            SQL += " WHERE 1 = 1                                                                                                                                                            \r\n";
            SQL += "   AND A.SPECNO     = " + ComFunc.covSqlstr(strSpecNO, false);
            SQL += "   AND A.SPECNO     = C.SPECNO                                                                                                                                          \r\n";
            SQL += "   AND A.DRCODE     = B.DRCODE                                                                                                                                          \r\n";
            SQL += "   AND B.GBOUT      = 'N'                                                                                                                                               \r\n";
            SQL += "   AND B.SABUN      = I.SABUN                                                                                                                                           \r\n";
            SQL += "   AND C.SUBCODE    = D.MASTERCODE                                                                                                                                      \r\n";
            SQL += "   AND SUBCODE      IN('MI031', 'MI03', 'GP111')                                                                                                                        \r\n";
            SQL += "   AND A.DEPTCODE   = 'MP'                                                                                                                                              \r\n";
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

        public string up_EXAM_RESULTC_CV(PsmhDb pDbCon, string strSPECNO, string strHCODE, bool isSubcode, ref int intRowAffected, ref string SQL) 
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_RESULTC        \r\n";
            SQL += "     SET CV     = 'C'                   \r\n";
            SQL += "       , UPPS   = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += "       , UPDT   = SYSDATE   \r\n";
            SQL += "   WHERE SPECNO = " + ComFunc.covSqlstr(strSPECNO, false);

            if (isSubcode == false)
            {
                SQL += "     AND HCODE IN (" + strHCODE + ")";
            }
            else
            {
                SQL += "     AND SUBCODE IN (" + strHCODE + ")";
            }


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

        public string ins_EXAM_RESULTCF(PsmhDb pDbCon, string strSPECNO, string strSEQNO, string strFOOTNOTE, string strSORT, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  INSERT INTO KOSMOS_OCS.EXAM_RESULTCF (SPECNO,SEQNO,FOOTNOTE,SORT) VALUES(     \r\n";
            SQL += "              " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "               " + ComFunc.covSqlstr(strSEQNO, true);
            SQL += "               " + ComFunc.covSqlstr(strFOOTNOTE, true);
            SQL += "               " + ComFunc.covSqlstr(strSORT, true);
            SQL += "               )                                                                \r\n";

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

        public string del_EXAM_RESULTCF(PsmhDb pDbCon, string strSPECNO, string strSEQNO, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  DELETE KOSMOS_OCS.EXAM_RESULTCF                        \r\n";
            SQL += "   WHERE SPECNO = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "     AND SEQNO  = " + ComFunc.covSqlstr(strSEQNO, false);

            try
            {
                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, pDbCon);

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
       
        public DataTable sel_EXAM_SMS(PsmhDb pDbCon, string strSpecNo)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "  SELECT                                                                                                                    \r\n";
            SQL += "  		'★' ||                                                                                                             \r\n";
            SQL += "  		(CASE                                                                                                               \r\n";
            SQL += "  			WHEN A.EXAM_C = '1' THEN '생화학'                                                                               \r\n";
            SQL += "  			WHEN A.EXAM_F = '1' THEN '체액검사'                                                                             \r\n";
            SQL += "  			WHEN A.EXAM_H = '1' THEN '혈액학'                                                                               \r\n";
            SQL += "  			WHEN A.EXAM_S = '1' THEN '혈청학'                                                                               \r\n";
            SQL += "  			WHEN A.EXAM_E = '1' THEN '면역학'                                                                               \r\n";
            SQL += "  			WHEN A.EXAM_B = '1' THEN '혈액은행'                                                                             \r\n";
            SQL += "  			WHEN A.EXAM_U = '1' THEN '소변검사'                                                                             \r\n";
            SQL += "  			WHEN A.EXAM_P = '1' THEN '분변검사'                                                                             \r\n";
            SQL += "  			WHEN A.EXAM_M = '1' THEN '미생물학'                                                                             \r\n";
            SQL += "  			WHEN A.EXAM_A = '1' THEN '세포학'                                                                               \r\n";
            SQL += "  			WHEN A.EXAM_T = '1' THEN '조직학'                                                                               \r\n";
            SQL += "  			WHEN A.EXAM_W = '1' THEN '외부의뢰'                                                                             \r\n";
            SQL += "  	    END) || ' 검사완료★ 등록번호 : ' ||  A.PANO ||' 성명 : ' || B.SNAME || ' OCS에서 확인하세요'   AS MSG              \r\n";
            SQL += "        , (CASE WHEN A.HPHONE1 IS NOT NULL THEN A.HPHONE1                                                                   \r\n";
            SQL += "                WHEN A.HPHONE2 IS NOT NULL THEN A.HPHONE2                                                                   \r\n";
            SQL += "                WHEN A.HPHONE3 IS NOT NULL THEN A.HPHONE3                                                                   \r\n";
            SQL += "                ELSE ''                                                                                                     \r\n";
            SQL += "            END                                                                                                             \r\n";
            SQL += "            )                                                                                           AS SEND_TEL         \r\n";
            SQL += "        , KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_PART_TEL',                                                                 \r\n";
            SQL += "        						( CASE                                                                                      \r\n";
            SQL += "  									WHEN A.EXAM_C = '1' THEN 'C'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_F = '1' THEN 'F'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_H = '1' THEN 'H'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_S = '1' THEN 'S'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_E = '1' THEN 'E'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_B = '1' THEN 'B'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_U = '1' THEN 'U'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_P = '1' THEN 'P'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_M = '1' THEN 'M'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_A = '1' THEN 'A'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_T = '1' THEN 'T'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_W = '1' THEN 'W'                                                            \r\n";
            SQL += "  							    END                                                                                         \r\n";
            SQL += "                               )                                                                                            \r\n";
            SQL += "  		)	AS EXAM_TEL                                                                                                     \r\n";
            SQL += "  		, B.PANO                                                                                         AS PANO            \r\n";
            SQL += "    FROM  KOSMOS_OCS.EXAM_SMS 	A                                                                                           \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_SPECMST B                                                                                         \r\n";
            SQL += "   WHERE 1=1                                                                                                                \r\n";
            SQL += "     AND B.SPECNO = " + ComFunc.covSqlstr(strSpecNo, false);
            SQL += "     AND B.STATUS = '05'                                                                                                    \r\n";
            SQL += "     AND A.PANO	  = B.PANO                                                                                                  \r\n";
            SQL += "     AND B.WORKSTS LIKE '%' || ( CASE                                                                                       \r\n";
            SQL += "  									WHEN A.EXAM_C = '1' THEN 'C'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_F = '1' THEN 'F'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_H = '1' THEN 'H'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_S = '1' THEN 'S'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_E = '1' THEN 'E'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_B = '1' THEN 'B'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_U = '1' THEN 'U'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_P = '1' THEN 'P'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_M = '1' THEN 'M'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_A = '1' THEN 'A'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_T = '1' THEN 'T'                                                            \r\n";
            SQL += "  									WHEN A.EXAM_W = '1' THEN 'W'                                                            \r\n";
            SQL += "  							    END                                                                                         \r\n";
            SQL += "                               ) || '%'                                                                                     \r\n";

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

        /// <summary>sel_EXAM_RESUlTC_CV_MSG</summary>
        /// <param name="strSpecNo"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public DataTable sel_EXAM_RESUlTC_CV_MSG(PsmhDb pDbCon, string strSpecNo, string strMsg, bool isMid, string isSMS)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                                                                     \r\n";
            SQL += "        B.SABUN                                                                                                                                             \r\n";
            SQL += "      , C.SUBCODE                                                                                                                                           \r\n";
            SQL += "      , C.ROWID                                                                                                                                             \r\n";
            if (isMid == false)
            {
                //SQL += "      , CASE WHEN LENGTH('" + strMsg.Trim() + "') > 0  THEN '" + strMsg.Trim() + "' || A.PANO || '/' || A.SNAME || '/' || D.EXAMYNAME                              \r\n";
                //2018-10-23 안정수, 김은정t 요청으로 문자양식 변경함 
                //SQL += "      , CASE WHEN LENGTH('" + strMsg.Trim() + "') > 0  THEN '" + strMsg.Trim() + "' || '/ ' || A.DEPTCODE || '/ ' || A.PANO || '/ ' || A.SNAME || '/ ' || TO_CHAR(A.RECEIVEDATE, 'YYYY.MM.DD') || '/ ' || KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',A.SPECCODE,'N') || '/ ' || C.RESULT  \r\n";
                //2019-12-24 안정수, 김은정t 요청으로 문자양식 변경함(외래, 입원 구분 추가) 
                SQL += "      , CASE WHEN LENGTH('" + strMsg.Trim() + "') > 0  THEN '" + strMsg.Trim() + "' || '/ ' || DECODE(A.IPDOPD, 'O', 'O', 'I', 'I' || ' / ' || A.ROOM) || '/ ' || A.DEPTCODE || '/ ' || A.PANO || '/ ' || A.SNAME || '/ ' || TO_CHAR(A.RECEIVEDATE, 'YYYY.MM.DD') || '/ ' || KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',A.SPECCODE,'N') || '/ ' || C.RESULT  \r\n";
                //SQL += "              ELSE '★긴급(CV)★' || DECODE(TRIM(A.WARD), '', '', A.WARD || '병동') || A.PANO || '/' || A.SNAME || '/' || d.EXAMYNAME || '/' || C.RESULT    \r\n";
                //2020-02-13 안정수, 약어 누락되는 경우 있어 조건 추가함
                SQL += "              ELSE CASE WHEN D.EXAMYNAME <> '' THEN '★긴급(CV)★' || DECODE(TRIM(A.WARD), '', '', A.WARD || '병동') || A.PANO || '/' || A.SNAME || '/' || D.EXAMYNAME || '/' || C.RESULT    \r\n";
                SQL += "                   ELSE '★긴급(CV)★' || DECODE(TRIM(A.WARD), '', '', A.WARD || '병동') || A.PANO || '/' || A.SNAME || '/' || D.EXAMNAME || '/' || C.RESULT    \r\n";
                SQL += "                    END                                                                                                                                  \r\n";
                SQL += "         END           AS MSG                                                                                                                            \r\n";
            }
            else
            {
                //SQL += "      , '★긴급(CV)중간보고★' || DECODE(TRIM(A.WARD), '', '', A.WARD || '병동') || A.PANO || '/' || A.SNAME || '/' || d.EXAMYNAME || '/' || '" + strMsg + "' AS MSG \r\n";
                //2018.03.14.김홍록: 중간 보고의 msg는 결과값이였다.
                //SQL += "      , '★긴급(CV)중간보고★' || DECODE(TRIM(A.WARD), '', '', A.WARD || '병동') || A.PANO || '/' || A.SNAME || '/' || d.EXAMYNAME || '/' || C.RESULT  AS MSG \r\n";
                SQL += "      , CASE WHEN D.EXAMYNAME <> '' THEN '★긴급(CV)중간보고★' || DECODE(TRIM(A.WARD), '', '', A.WARD || '병동') || A.PANO || '/' || A.SNAME || '/' || d.EXAMYNAME || '/' || C.RESULT  \r\n";
                SQL += "             ELSE '★긴급(CV)중간보고★' || DECODE(TRIM(A.WARD), '', '', A.WARD || '병동') || A.PANO || '/' || A.SNAME || '/' || d.EXAMNAME || '/' || C.RESULT   \r\n";
                SQL += "         END           AS MSG                                                                                                                            \r\n";
            }
            SQL += "      , A.PANO                                                                                                                                              \r\n";
            SQL += "      , I.HTEL                                                                                                                                              \r\n";
            SQL += "      ,'1'                  AS GBN                                                                                                                           \r\n";
            SQL += "      , A.IPDOPD                                                                                                                                            \r\n";
            SQL += "      , CASE WHEN TRIM(C.RESULTWS) = 'B' THEN '054-260-8258'                                                                                                \r\n";
            SQL += "             WHEN TRIM(C.RESULTWS) = 'C' THEN '054-260-8259'                                                                                                \r\n";
            SQL += "             WHEN TRIM(C.RESULTWS) = 'H' THEN '054-260-8260'                                                                                                \r\n";
            SQL += "             WHEN TRIM(C.RESULTWS) = 'E' OR TRIM(C.RESULTWS) = 'W' THEN '054-260-8261'                                                                      \r\n";
            SQL += "             WHEN TRIM(C.RESULTWS) = 'S' THEN '054-260-8261'                                                                                                \r\n";
            SQL += "             WHEN TRIM(C.RESULTWS) = 'U' THEN '054-260-8258'                                                                                                \r\n";
            SQL += "             WHEN TRIM(C.RESULTWS) = 'M' THEN '054-260-8262'                                                                                                \r\n";
            SQL += "         ELSE '054-260-8261'                                                                                                                                \r\n";
            SQL += "         END TEL                                                                                                                                            \r\n";
            SQL += " FROM " + ComNum.DB_MED + "EXAM_SPECMST A                                                                                                                             \r\n";
            SQL += "    , " + ComNum.DB_MED + "EXAM_RESULTC C                                                                                                                             \r\n";
            SQL += "    , " + ComNum.DB_MED + "EXAM_MASTER  D                                                                                                                             \r\n";
            SQL += "    , " + ComNum.DB_MED + "OCS_DOCTOR   B                                                                                                                             \r\n";
            SQL += "    , " + ComNum.DB_ERP + "INSA_MST     I                                                                                                                             \r\n";
            SQL += " WHERE 1 = 1                                                                                                                                                \r\n";
            SQL += " AND A.SPECNO  = " + ComFunc.covSqlstr(strSpecNo, false);
            SQL += " AND A.DRCODE  = B.DRCODE                                                                                                                                   \r\n";
            SQL += " AND B.GBOUT   = 'N'                                                                                                                                        \r\n";
            SQL += " AND B.SABUN   = I.SABUN                                                                                                                                    \r\n";
            SQL += " AND B.SABUN   <> '48748'                                                                                                                                    \r\n"; //2019-11-07 박병화 선생님이 요청
            SQL += " AND A.SPECNO  = C.SPECNO                                                                                                                                   \r\n";
            SQL += " AND C.SUBCODE = D.MASTERCODE                                                                                                                            \r\n";
         
            if (isMid == false && isSMS == "")
            {
                SQL += " AND C.CV      = 'C'                                                                                                                                        \r\n";
                SQL += " AND C.STATUS  = 'V'                                                                                                                                         \r\n";
            }

            if (isMid == false && isSMS == "Y")
            {
                SQL += " AND C.CV      = 'C'                                                                                                                                        \r\n";
            }

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

        /// <summary>ETC_SMS로 전송</summary>
        /// <param name="strPano"></param>
        /// <param name="strSpecNo"></param>
        /// <returns></returns>
        public string ins_ETC_SMS(PsmhDb pDbCon, string strPano, string strSpecNo, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS(JOBDATE, PANO, SNAME, HPHONE, GUBUN, RETTEL, SENDMSG)   \r\n";
            SQL += "SELECT JOBDATE                                                                          \r\n";
            SQL += "      , PANO                                                                            \r\n";
            SQL += "      , SNAME                                                                           \r\n";
            SQL += "      , HPHONE                                                                          \r\n";
            SQL += "      , GUBUN                                                                           \r\n";
            SQL += "      , RET_TEL                                                                         \r\n";
            SQL += "      , MSG                                                                             \r\n";
            SQL += "  FROM(                                                                                 \r\n";
            SQL += "    WITH T AS(                                                                          \r\n";
            SQL += "    SELECT PANO                                                                         \r\n";
            SQL += "         , HPHONE1                                                                      \r\n";
            SQL += "         , HPHONE2                                                                      \r\n";
            SQL += "         , HPHONE3                                                                      \r\n";
            SQL += "         , CASE                                                                         \r\n";
            SQL += "                WHEN ROWNUM = 1  THEN '054-260-8259'-- C                                \r\n";
            SQL += "                WHEN ROWNUM = 2  THEN '054-260-8261'--'F'                               \r\n";
            SQL += "                WHEN ROWNUM = 3  THEN '054-260-8260'--'H'                               \r\n";
            SQL += "                WHEN ROWNUM = 4  THEN '054-260-8261'--'S'                               \r\n";
            SQL += "                WHEN ROWNUM = 5  THEN '054-260-8261'--'E'                               \r\n";
            SQL += "                WHEN ROWNUM = 6  THEN '054-260-8258'--'B'                               \r\n";
            SQL += "                WHEN ROWNUM = 7  THEN '054-260-8258'--'U'                               \r\n";
            SQL += "                WHEN ROWNUM = 8  THEN '054-260-8262'--'P'                               \r\n";
            SQL += "                WHEN ROWNUM = 9  THEN '054-260-8262'--'M'                               \r\n";
            SQL += "                WHEN ROWNUM = 10  THEN '054-260-8267'--'A'                              \r\n";
            SQL += "                WHEN ROWNUM = 11  THEN '054-260-8267'--'T'                              \r\n";
            SQL += "                WHEN ROWNUM = 12  THEN '054-260-8261'--'W'                              \r\n";
            SQL += "           END AS TEL                                                                   \r\n";
            SQL += "         , CASE                                                                         \r\n";
            SQL += "                WHEN ROWNUM = 1  THEN 'C'                                               \r\n";
            SQL += "                WHEN ROWNUM = 2  THEN 'F'                                               \r\n";
            SQL += "                WHEN ROWNUM = 3  THEN 'H'                                               \r\n";
            SQL += "                WHEN ROWNUM = 4  THEN 'S'                                               \r\n";
            SQL += "                WHEN ROWNUM = 5  THEN 'E'                                               \r\n";
            SQL += "                WHEN ROWNUM = 6  THEN 'B'                                               \r\n";
            SQL += "                WHEN ROWNUM = 7  THEN 'U'                                               \r\n";
            SQL += "                WHEN ROWNUM = 8  THEN 'P'                                               \r\n";
            SQL += "                WHEN ROWNUM = 9  THEN 'M'                                               \r\n";
            SQL += "                WHEN ROWNUM = 10  THEN 'A'                                              \r\n";
            SQL += "                WHEN ROWNUM = 11  THEN 'T'                                              \r\n";
            SQL += "                WHEN ROWNUM = 12  THEN 'W'                                              \r\n";
            SQL += "           END AS WORKSTS                                                               \r\n";
            SQL += "         , CASE                                                                         \r\n";
            SQL += "                WHEN ROWNUM = 1  THEN '생화학'                                          \r\n";
            SQL += "                WHEN ROWNUM = 2  THEN '체액검사'                                        \r\n";
            SQL += "                WHEN ROWNUM = 3  THEN '혈액학'                                          \r\n";
            SQL += "                WHEN ROWNUM = 4  THEN '혈청학'                                          \r\n";
            SQL += "                WHEN ROWNUM = 5  THEN '면역학'                                          \r\n";
            SQL += "                WHEN ROWNUM = 6  THEN '혈액은행'                                        \r\n";
            SQL += "                WHEN ROWNUM = 7  THEN '소변검사'                                        \r\n";
            SQL += "                WHEN ROWNUM = 8  THEN '분변검사'                                        \r\n";
            SQL += "                WHEN ROWNUM = 9  THEN '미생물학'                                        \r\n";
            SQL += "                WHEN ROWNUM = 10 THEN '세포학'                                          \r\n";
            SQL += "                WHEN ROWNUM = 11 THEN '조직학'                                          \r\n";
            SQL += "                WHEN ROWNUM = 12 THEN '외부의뢰'                                        \r\n";
            SQL += "             END AS EXAM_NAME                                                           \r\n";
            SQL += "         , CASE                                                                         \r\n";
            SQL += "                WHEN ROWNUM = 1  AND EXAM_C = '1' THEN '1'                              \r\n";
            SQL += "                WHEN ROWNUM = 2  AND EXAM_F = '1' THEN '1'                              \r\n";
            SQL += "                WHEN ROWNUM = 3  AND EXAM_H = '1' THEN '1'                              \r\n";
            SQL += "                WHEN ROWNUM = 4  AND EXAM_S = '1' THEN '1'                              \r\n";
            SQL += "                WHEN ROWNUM = 5  AND EXAM_E = '1' THEN '1'                              \r\n";
            SQL += "                WHEN ROWNUM = 6  AND EXAM_B = '1' THEN '1'                              \r\n";
            SQL += "                WHEN ROWNUM = 7  AND EXAM_U = '1' THEN '1'                              \r\n";
            SQL += "                WHEN ROWNUM = 8  AND EXAM_P = '1' THEN '1'                              \r\n";
            SQL += "                WHEN ROWNUM = 9  AND EXAM_M = '1' THEN '1'                              \r\n";
            SQL += "                WHEN ROWNUM = 10  AND EXAM_A = '1' THEN '1'                             \r\n";
            SQL += "                WHEN ROWNUM = 11  AND EXAM_T = '1' THEN '1'                             \r\n";
            SQL += "                WHEN ROWNUM = 12  AND EXAM_W = '1' THEN '1'                             \r\n";
            SQL += "             END AS VALUE                                                               \r\n";
            SQL += "    FROM  " + ComNum.DB_MED + "EXAM_SMS                                                 \r\n";
            SQL += "       , (SELECT LEVEL CNT                                                              \r\n";
            SQL += "           FROM DUAL                                                                    \r\n";
            SQL += "        CONNECT BY LEVEL < 13)                                                          \r\n";
            SQL += "    WHERE 1 = 1                                                                         \r\n";
            SQL += "      AND PANO   = " + ComFunc.covSqlstr(strPano, false);
            SQL += "      AND GUBUN2 = '1'                                                                  \r\n";
            SQL += "      )                                                                                 \r\n";
            SQL += "      SELECT SYSDATE                                        AS JOBDATE                  \r\n";
            SQL += "           , S.PANO                                         AS PANO                     \r\n";
            SQL += "	        , '진단검사의학과'                              AS SNAME                    \r\n";
            SQL += "            , CASE WHEN LENGTH(TRIM(T.HPHONE1)) > 0 THEN TRIM(T.HPHONE1)                \r\n";
            SQL += "                   WHEN LENGTH(TRIM(T.HPHONE2)) > 0 THEN TRIM(T.HPHONE2)                \r\n";
            SQL += "                   WHEN LENGTH(TRIM(T.HPHONE3)) > 0 THEN TRIM(T.HPHONE3)                \r\n";
            SQL += "                   ELSE ''                                                              \r\n";
            SQL += "              END                                          AS HPHONE                    \r\n";
            SQL += "            , 'N'                                          AS GUBUN                     \r\n";
            SQL += "            , T.TEL                                        AS RET_TEL                   \r\n";
            SQL += "            , '★' || T.EXAM_NAME || ' 검사완료★ 등록번호 : ' || T.PANO || ' 성명 : ' || S.SNAME || ' OCS에서 확인 하세요 ' AS MSG  \r\n";
            SQL += "        FROM T                                                                          \r\n";
            SQL += "           , " + ComNum.DB_MED + "EXAM_SPECMST S                                        \r\n";
            SQL += "       WHERE 1=1                                                                        \r\n";
            SQL += "         AND VALUE    = '1'                                                             \r\n";
            SQL += "         AND S.SPECNO = " + ComFunc.covSqlstr(strSpecNo, false);
            SQL += "         AND S.PANO = T.PANO                                                            \r\n";
            SQL += "         AND S.WORKSTS LIKE T.WORKSTS || ''%                                            \r\n";
            SQL += "     )                                                                                  \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            if (string.IsNullOrEmpty(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            }

            return SqlErr;
        }

        /// <summary>ins_ETC_SMS</summary>
        /// <param name="strPano"></param>
        /// <param name="strHpone"></param>
        /// <param name="strTel"></param>
        /// <param name="strGubun"></param>
        /// <param name="strMsg"></param> 
        /// <returns></returns>
        public string ins_ETC_SMS(PsmhDb pDbCon, string strPano, string strHpone, string strTel, string strGubun, string strMsg, ref int intRowAffected)
        {
            string SqlErr = ""; 

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS(JOBDATE, PANO, SNAME, HPHONE, GUBUN, RETTEL, SENDMSG)  VALUES (    \r\n";
            SQL += " SYSDATE                                                                                            \r\n";
            SQL += ComFunc.covSqlstr(strPano, true);
            SQL += ComFunc.covSqlstr("진단검사의학과", true);
            SQL += ComFunc.covSqlstr(strHpone, true);
            SQL += ComFunc.covSqlstr(strGubun, true);
            SQL += ComFunc.covSqlstr(strTel, true);
            SQL += ComFunc.covSqlstr(strMsg, true);
            SQL += " )                                                                                            \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            if (string.IsNullOrEmpty(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            }

            return SqlErr;
        }

        /// <summary>ins_EXAM_RESULTC_CV</summary>
        /// <param name="strGbn"></param>
        /// <param name="strRowId"></param>
        /// <returns></returns>
        public string ins_EXAM_RESULTC_CV(PsmhDb pDbCon, string strGbn, string strRowId, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "EXAM_RESULTC_CV ( JOBDATE, GBN,   SPECNO, Pano, MASTERCODE,SUBCODE,Result,RESULTDATE,RESULTSABUN,UNIT , SMSSEND )         \r\n";
            SQL += " SELECT TRUNC(SYSDATE), '" + strGbn + "', SPECNO, Pano, MASTERCODE,SUBCODE,Result,RESULTDATE,RESULTSABUN,UNIT , SYSDATE                                     \r\n";
            SQL += "   FROM  " + ComNum.DB_MED + "EXAM_RESULTC                                                                                                                  \r\n";
            SQL += "  WHERE ROWID = " + ComFunc.covSqlstr(strRowId, false);
            SQL += "    AND CV ='C'                                                                                                                                             \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            if (string.IsNullOrEmpty(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            }


            return SqlErr;

        }

        /// <summary>ins_EXAM_RESULTC_CV</summary>
        /// <param name="strSpecNo"></param>
        /// <returns></returns>
        public string ins_EXAM_RESULTC_CV_3(PsmhDb pDbCon, string strSpecNo, ref int intRowAffected) 
        {
            string SqlErr = "";

            SQL = "";
            SQL += " INSERT INTO KOSMOS_OCS.EXAM_RESULTC_CV(JOBDATE, GBN, SPECNO, Pano, MASTERCODE, SUBCODE, Result, RESULTDATE, RESULTSABUN, UNIT) \r\n";
            SQL += " SELECT TRUNC(SYSDATE), '3', C.SPECNO, C.PANO, C.MASTERCODE, C.SUBCODE, C.RESULT, C.RESULTDATE, C.RESULTSABUN, C.UNIT           \r\n";
            SQL += " FROM  " + ComNum.DB_MED + "EXAM_RESULTC C                                                                                      \r\n";
            SQL += "     , " + ComNum.DB_MED + "EXAM_SPECMST M                                                                                      \r\n";
            SQL += " WHERE 1 = 1                                                                                                                    \r\n";
            SQL += "   AND C.SPECNO = M.SPECNO                                                                                                      \r\n";
            SQL += "   AND C.SPECNO = " + ComFunc.covSqlstr(strSpecNo, false);
            SQL += "   AND M.IPDOPD = 'I'                                                                                                           \r\n";
            SQL += "   AND C.CV = 'C'                                                                                                               \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            if (string.IsNullOrEmpty(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            }


            return SqlErr;

        }

        /// <summary>ins_EXAM_SMSSEND</summary>
        /// <param name="strSpecNo"></param>
        /// <param name="strPano"></param>
        /// <param name="strMsg"></param>
        /// <param name="strHtel"></param>
        /// <returns></returns>
        public string ins_EXAM_SMSSEND(PsmhDb pDbCon, string strSpecNo, string strPano, string strMsg, string strHtel, string strSabun, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "INSERT INTO " + ComNum.DB_MED + "EXAM_SMSSEND( JOBDATE, SABUN, SPECNO, HPHONE ,PANO, SMS) VALUES ( \r\n";
            SQL += "SYSDATE";
            SQL += ComFunc.covSqlstr(strSabun, true);
            SQL += ComFunc.covSqlstr(strSpecNo, true);
            SQL += ComFunc.covSqlstr(strHtel, true);
            SQL += ComFunc.covSqlstr(strPano, true);
            SQL += ComFunc.covSqlstr(strMsg, true);
            SQL += ")";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            if (string.IsNullOrEmpty(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            }

            return SqlErr;

        }
    }
}
