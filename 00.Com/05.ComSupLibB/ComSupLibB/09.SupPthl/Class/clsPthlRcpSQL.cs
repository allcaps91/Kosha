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
    public class clsPthlRcpSQL : Com.clsMethod
    {
        string SQL = string.Empty; 

        /// <summary>Sel_EXAM_SPECMST_ANATPrint 조회결과</summary>
        public enum enmSel_EXAM_SPECMST_ANATPrint {PANO, SNAME,JUMIN1,JUMIN2,WARD_NAME,SPEC_NAME,ANATNO,DR_NAME,BDATE };

        /// <summary>Sel_EXAM_ANATMST_TAT</summary>
        public enum enmSel_EXAM_ANATMST_TAT       {     ANATNO,        PTNO, EXAM_RECEIVEDATE,            JDATE,           JTIME,      RDATE,                 RTIME,           GBTAT,         ILLDATE,       ILLGAP,      RESULT_NAME,         EXAM_NAME,         GBTATSAU,         GBTATBUN,       GBTATBUN2,       GBTATBUN3,   RESULT1,   RESULT2, CHANGE, ROWID};
        public string[] sSel_EXAM_ANATMST_TAT_T = { "조직번호",  "등록번호",       "병리접수일시",       "접수일자",      "접수시각",       "보고일자",      "보고시각",      "기준일수",      "경과일수",     "차수",       "병리의사",          "검체"  ,       "지연사유",       "제외사유",         "제외2",         "제외3", "RESULT1", "RESULT2", "CHANGE", "ROWID" };
        public string[] sSel_EXAM_ANATMST_TAT_A = { "조직번호",  "등록번호",       "병리접수일시",       "접수일자",      "접수시각",       "보고일자",      "보고시각",      "기준일수",      "경과일수",     "차수",       "병리의사",          "검사명",       "지연사유",       "제외사유",         "제외2",         "제외3", "RESULT1", "RESULT2", "CHANGE", "ROWID" };
        public int   [] nSel_EXAM_ANATMST_TAT   = {  nCol_PANO,   nCol_PANO,    nCol_TIME, nCol_DATE,              40, nCol_DATE,              40,              40,              40,     5     ,nCol_SNAME, nCol_SNAME, nCol_NAME, nCol_NAME, nCol_AGE, nCol_AGE,         5,         5,       5, 5 };

        public enum enmANATMST_TYPE {
            /// <summary>병리</summary>
            T
                /// <summary>세포</summary>
                , A };

        public enum enumTAT_SEARCH_TYPE {ALL,IN,DEILY};

        public enum enmSel_EXAM_ANATMST_RCP_WS {A,S,C,P,PS,PU,W,T,AC,OS,TS,IH}; 

        public enum enmSel_EXAM_ANATMST_RCP     { INFECT_INFO,   INFECT_IMAGE,      RDATE,                 RTIME,      JDATE,           JTIME,           PANO,      SNAME,   ORDERCODE,   ORDERNO,     SPECNO,      MASTER_NM,   SPECCODE,  SPEC_NM,          GBIO,           WARD,       DEPTCODE,      DR_NM,     ANATNO,          JSABUN,     MASTERCODE,     WSCODE,   DRCODE,         BI,       BDATE,   RESULTSABUN,   REMARK1,  REMARK2,    REMARK3,   REMARK4,    TALCHK,   GBOUT, ROWID_R   , DRCOMMENT };
        public string[] sSel_EXAM_ANATMST_RCP = { "INFECT_INFO",   "감염정보",    "검체접수일자",     "검체시간", "병리일자",      "병리시간",     "등록번호", "환자성명", "ORDERCODE", "ORDERNO", "검체번호",       "검사명", "SPECCODE", "검체명",    "환자구분",         "병동",           "과",   "의사명", "병리번호",        "접수자",     "검사코드", "작업구분", "DRCODE", "보험유형",  "처방일자", "RESULTSABUN", "REMARK1", "REMARK2", "REMARK3", "REMARK4",  "TALCHK", "GBOUT", "ROWID_R" , "의사비고" };
        public int[] nSel_EXAM_ANATMST_RCP    = {            5, nCol_PANO - 15,   nCol_DATE,      nCol_WARD - 10,  nCol_DATE,  nCol_WARD - 10, nCol_PANO - 10, nCol_SNAME,           5,         5,  nCol_PANO, nCol_ORDERNAME-50,       5, nCol_AGE, nCol_AGE - 10, nCol_SCHK + 10, nCol_SCHK + 10, nCol_SNAME,  nCol_PANO, nCol_SNAME - 10, nCol_WARD - 10,  nCol_PANO,        5,   nCol_AGE,   nCol_DATE,             5,         5,         5,         5,         5,         5,       5,        5  , nCol_AGE };

        public enum enmSel_EXAM_ANATMST_PRT     {        CHK,     ANATNO, SAMPLE_CNT,  PRINT_CNT,      SNAME,      BDATE,    JDATE  ,  ANATNO1  , SNAME1 ,  ANATNO2  , SNAME2, Age, Sex};
        public string[] sSel_EXAM_ANATMST_PRT = {     "선택", "병리번호",   "샘플수", "인쇄장수", "환자성명", "처방일자", "접수일자", "ANATNO1" ,"SNAME1", "ANATNO2", "SNAME2", "나이", "성별"};
        public int[] nSel_EXAM_ANATMST_PRT    = {  nCol_SCHK,  nCol_DATE,  nCol_DATE,  nCol_DATE , nCol_TIME,  nCol_DATE, nCol_DATE ,          5,       5,         5,     5,      5 ,      5 };

        public enum enmSel_EXAM_ANATMST_MASTERCODE_CHG     {       CHK,          JDATE,      PTNO,       SNAME,     SPECNO, MASTERCODE,    MASTERCODE2,  ROWID_R };
        public string[] sSel_EXAM_ANATMST_MASTERCODE_CHG = {    "선택", "병리접수일자","병리번호",  "환자성명", "검체번호", "검사코드", "변경검사코드", "ROWID_R"};
        public int[] nSel_EXAM_ANATMST_MASTERCODE_CHG    = { nCol_SCHK,      nCol_TIME, nCol_DATE,   nCol_DATE,  nCol_DATE,  nCol_DATE,       nCol_TIME,  5       };
        public enum enmSel_EXAM_ANATMST_ENDO_SUCODE { BDATE, ORDERCODE, SUCODE, SUNAMEK, IO };
        public string[] sSel_EXAM_ANATMST_ENDO_SUCODE = { "처방일자", "오더코드", "수가코드", "수가코드명", "병동외래구분(I/O)" };
        public int[] nSel_EXAM_ANATMST_ENDO_SUCODE = {  nCol_DATE, nCol_DATE, nCol_DATE, nCol_DATE + 250, nCol_DATE -10 };

        public enum enmSel_EXAM_ANATNO_YYYY       {     GUBUN,     ANATNO,        ANATNO2,  ROWID_R };
        public string[] sSel_EXAM_ANATNO_YYYY   = {    "구분", "병리번호", "변경병리번호", "ROWID_R"};
        public int[] nSel_EXAM_ANATNO_YYYY      = { nCol_TIME,  nCol_TIME,      nCol_TIME,        5 };


        public enum enmSel_EXAM_ANATMST_ANATNO { PTNO, SPECNO, ANATNO };

        public enum enmSel_EXAM_ORDER_REMARK { REMARK1, REMARK2, REMARK3, REMARK4 };

        public DataTable sel_EXAM_ORDER_REMARK(PsmhDb pDbCon, string strSPECNO)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += "  WITH T AS (                                           \r\n";
            SQL += "  	SELECT  C.REMARK1                                   \r\n";
            SQL += "  	       ,C.REMARK2,C.REMARK3,C.REMARK4               \r\n";
            SQL += "  	  FROM  KOSMOS_OCS.EXAM_ORDER A                                \r\n";
            SQL += "  	      , KOSMOS_OCS.OCS_IORDER B                                \r\n";
            SQL += "  	      , KOSMOS_OCS.EXAM_ANATMST C                              \r\n";
            SQL += "  	WHERE 1=1                                           \r\n";
            SQL += "  	  AND A.PANO 		= B.PTNO                        \r\n";
            SQL += "  	  AND A.BDATE 		= B.BDATE                       \r\n";
            SQL += "  	  AND (A.DEPTCODE 	= B.DEPTCODE OR A.DEPTCODE = 'ER')         \r\n";
            SQL += "  	  AND A.ORDERNO 	= B.ORDERNO                     \r\n";
            SQL += "  	  AND B.PTNO		= C.PTNO                        \r\n";
            SQL += "  	  AND B.BDATE		= C.BDATE                       \r\n";
            SQL += "  	  AND B.ORDERCODE	= C.ORDERCODE                   \r\n";
            SQL += "  	  AND A.SPECNO LIKE '" + strSPECNO + "%'            \r\n";
            SQL += "  	UNION ALL                                           \r\n";
            SQL += "  	SELECT  C.REMARK1,C.REMARK2,C.REMARK3,C.REMARK4     \r\n";
            SQL += "  	  FROM  KOSMOS_OCS.EXAM_ORDER A                                \r\n";
            SQL += "  	      , KOSMOS_OCS.OCS_OORDER B                                \r\n";
            SQL += "  	      , KOSMOS_OCS.EXAM_ANATMST C                              \r\n";
            SQL += "  	WHERE 1=1                                           \r\n";
            SQL += "  	  AND A.PANO 		= B.PTNO                        \r\n";
            SQL += "  	  AND A.BDATE 		= B.BDATE                       \r\n";
            SQL += "  	  AND A.DEPTCODE 	= B.DEPTCODE                    \r\n";
            SQL += "  	  AND A.ORDERNO 	= B.ORDERNO                     \r\n";
            SQL += "  	  AND B.PTNO		= C.PTNO                        \r\n";
            SQL += "  	  AND B.BDATE		= C.BDATE                       \r\n";
            SQL += "  	  AND B.ORDERCODE	= C.ORDERCODE                   \r\n";
            SQL += "  	  AND A.SPECNO LIKE '" + strSPECNO + "%'            \r\n";
            SQL += "    )                                                   \r\n";
            SQL += "  SELECT  REMARK1                                       \r\n";
            SQL += "   	  , REMARK2                                         \r\n";
            SQL += "   	  , REMARK3                                         \r\n";
            SQL += "   	  , REMARK4                                         \r\n";
            SQL += "    FROM T                                              \r\n";
            SQL += "  GROUP BY                                              \r\n";
            SQL += "  		REMARK1                                         \r\n";
            SQL += "   	  , REMARK2                                         \r\n";
            SQL += "   	  , REMARK3                                         \r\n";
            SQL += "   	  , REMARK4                                         \r\n";
            SQL += "                                                        \r\n";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);


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


        public DataSet sel_EXAM_ANATNO_YYYY(PsmhDb pDbCon, string strYYYY)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                         \r\n";
            SQL += "        GUBUN    AS GUBUN       \r\n";
            SQL += "      , ANATNO  AS ANATNO       \r\n";
            SQL += "      , ''      AS ANATNO2      \r\n";
            SQL += "      , ROWID   AS ROWID_R      \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_ANATNO A                \r\n";
            SQL += "   WHERE 1 = 1                                            \r\n";
            SQL += "     AND A.YYYY  = " + ComFunc.covSqlstr(strYYYY, false);

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

        public string up_EXAM_RESULTC_MASTERCODE(PsmhDb pDbCon, string strMASTERCODE2, string strSPECNO, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_RESULTC                    \r\n";
            SQL += "    SET MASTERCODE =  " + ComFunc.covSqlstr(strMASTERCODE2, false);
            SQL += "      , SUBCODE    =  " + ComFunc.covSqlstr(strMASTERCODE2, false);
            SQL += "   WHERE 1 = 1                                            \r\n";
            SQL += "     AND SPECNO    = " + ComFunc.covSqlstr(strSPECNO, false);

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

        public string up_EXAM_ANATMST_MASTERCODE(PsmhDb pDbCon, string strMASTERCODE2,string strROWID, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_ANATMST                    \r\n";
            SQL += "    SET MASTERCODE =  " + ComFunc.covSqlstr(strMASTERCODE2, false);
            SQL += "   WHERE 1 = 1                                            \r\n";
            SQL += "     AND ROWID  = " + ComFunc.covSqlstr(strROWID, false);

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

        public DataSet sel_EXAM_ANATMST_MASTERCODE_CHG(PsmhDb pDbCon, string strANATNO_FR, string strANATNO_TO, string strGUBUN, string strPTNO)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                                     \r\n";
            SQL += "  		  ''            AS CHK                              \r\n";
            SQL += "  		, TO_CHAR(JDATE,'YYYY-MM-DD')         AS JDATE                            \r\n";
            SQL += "  		, A.PTNO        AS PTNO                             \r\n";
            SQL += "  		, B.SNAME       AS SNAME                            \r\n";
            SQL += "  		, A.SPECNO      AS SPECNO                           \r\n";
            SQL += "  		, A.MASTERCODE	AS MASTERCODE                       \r\n";
            SQL += "  		, '' 			AS MASTERCODE2                      \r\n";
            SQL += "  		, A.ROWID       AS ROWID_R                          \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_ANATMST A                           \r\n";
            SQL += "      , KOSMOS_PMPA.BAS_PATIENT B                           \r\n";
            SQL += "   WHERE 1=1                                                \r\n";
            SQL += "     AND JDATE  BETWEEN TO_DATE('" + strANATNO_FR + " 00:00','YYYY-MM-DD HH24:MI')  \r\n";
            SQL += "                    AND TO_DATE('" + strANATNO_TO + " 23:59','YYYY-MM-DD HH24:MI')  \r\n";
            SQL += "     AND A.PTNO   = B.PANO                                  \r\n";
            SQL += "     AND A.SPECNO IS NOT NULL                               \r\n";

            if (string.IsNullOrEmpty(strPTNO.Trim()) == false)
            {
                SQL += "     AND ANATNO = " + ComFunc.covSqlstr(strPTNO, false);
            }

            SQL += "     AND ANATNO LIKE '" + strGUBUN.Trim() + "%'             \r\n";
            SQL += "   ORDER BY PANO, MASTERCODE                                \r\n";

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


        public DataSet sel_EXAM_ANATMST_ENDO_SUCODE(PsmhDb pDbCon, string strANATNO_FR, string strANATNO_TO, string strGUBUN, string strPTNO)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                                                            \r\n";
            SQL += " A.BDATE,                                                                          \r\n";
            SQL += " A.ORDERCODE,                                                                      \r\n";
            SQL += " A.SUCODE,                                                                         \r\n";
            SQL += " (SELECT TRIM(D.SUNAMEK)                                                           \r\n";
            SQL += "  FROM KOSMOS_PMPA.BAS_SUN D                                                       \r\n";
            SQL += "  WHERE D.SUNEXT = A.SUCODE) AS SUNAMEK,                                           \r\n";
            SQL += "  'O' AS IO                                                                        \r\n";
            SQL += "  FROM KOSMOS_OCS.OCS_OORDER A                                                     \r\n";
            SQL += "  WHERE A.PTNO = '" + strPTNO.Trim() +  "'                                           \r\n";
            SQL += "  AND A.SUCODE IN ('C5602','C5603','C5604')                                        \r\n";
            SQL += "  AND A.BDATE  BETWEEN TO_DATE('" + strANATNO_FR + " 00:00','YYYY-MM-DD HH24:MI')  \r\n";
            SQL += "                   AND TO_DATE('" + strANATNO_TO + " 23:59','YYYY-MM-DD HH24:MI')  \r\n";
            SQL += "  UNION ALL                                                                        \r\n";
            SQL += " SELECT                                                                            \r\n";
            SQL += " B.BDATE,                                                                          \r\n";
            SQL += " B.ORDERCODE,                                                                      \r\n";
            SQL += " B.SUCODE,                                                                         \r\n";
            SQL += " (SELECT TRIM(C.SUNAMEK)                                                           \r\n";
            SQL += "  FROM KOSMOS_PMPA.BAS_SUN C                                                       \r\n";
            SQL += "  WHERE C.SUNEXT = B.SUCODE) AS SUNAMEK,                                           \r\n";
            SQL += " 'I' AS IO                                                                         \r\n";
            SQL += "  FROM KOSMOS_OCS.OCS_IORDER B                                                     \r\n";
            SQL += "  WHERE B.PTNO = '" + strPTNO.Trim() + "'                                            \r\n";
            SQL += "  AND B.SUCODE IN ('C5602','C5603','C5604')                                        \r\n";
            SQL += "  AND B.BDATE  BETWEEN TO_DATE('" + strANATNO_FR + " 00:00','YYYY-MM-DD HH24:MI')  \r\n";
            SQL += "                   AND TO_DATE('" + strANATNO_TO + " 23:59','YYYY-MM-DD HH24:MI')  \r\n";

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

        public DataSet sel_EXAM_ANATMST_PRT(PsmhDb pDbCon, string strANATNO_FR, string strANATNO_TO, enmSel_EXAM_ANATMST_RCP_WS pPCP_WS) 
        {
            DataSet ds = null;

            string SQL = "";    //Query문 
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";          
            SQL += "  SELECT                                                                \r\n";
            SQL += "        '' CHK                                                          \r\n";

            if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.S)
            {
                SQL += "     ,SUBSTR(A.ANATNO,0,3) || '-' || SUBSTR(A.ANATNO,4) AS ANATNO    \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.OS)
            {
                SQL += "     ,SUBSTR(A.ANATNO,0,4) || '-' || SUBSTR(A.ANATNO,5) AS ANATNO    \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.C)
            {
                SQL += "     ,SUBSTR(A.ANATNO,0,3) || '-' || SUBSTR(A.ANATNO,4) AS ANATNO    \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.P)
            {
                SQL += "     ,SUBSTR(A.ANATNO,0,3) || '-' || SUBSTR(A.ANATNO,4) AS ANATNO    \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.PS)
            {
                SQL += "     ,SUBSTR(A.ANATNO,0,4) || '-' || SUBSTR(A.ANATNO,5) AS ANATNO    \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.PU)
            {
                SQL += "     ,SUBSTR(A.ANATNO,0,4) || '-' || SUBSTR(A.ANATNO,5) AS ANATNO    \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.AC)
            {
                SQL += "     ,SUBSTR(A.ANATNO,0,4) || '-' || SUBSTR(A.ANATNO,5) AS ANATNO    \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.IH)
            {
                SQL += "     ,SUBSTR(A.ANATNO,0,4) || '-' || SUBSTR(A.ANATNO,5) AS ANATNO    \r\n";
            }
            SQL += "  		, '1'							AS SAMPLE_CNT               \r\n";
            SQL += "  		, '1'							AS PRINT_CNT                \r\n";
            SQL += "  		, B.SNAME                       AS SNAME                    \r\n";
            SQL += "  		, TO_CHAR(A.BDATE,'YYYY-MM-DD')	AS BDATE                    \r\n";
            SQL += "  		, TO_CHAR(A.JDATE,'YYYY-MM-DD')	AS JDATE                    \r\n";
            SQL += "  		, ''	                        AS ANATNO1                  \r\n";
            SQL += "  		, ''	                        AS SNAME1                   \r\n";
            SQL += "  		, ''	                        AS ANATNO2                  \r\n";
            SQL += "  		, ''	                        AS SNAME2                   \r\n";
            SQL += "  		, KOSMOS_OCS.FC_GET_AGE(TO_CHAR(C.Birth, 'YYYY-MM-DD'),A.BDate) FC_Age \r\n";
            SQL += "  		, C.SEX                         AS SEX                      \r\n";

            SQL += "    FROM KOSMOS_OCS.EXAM_ANATMST A                                  \r\n";
            SQL += "       , KOSMOS_OCS.EXAM_SPECMST B                                  \r\n";
            SQL += "       , KOSMOS_PMPA.BAS_PATIENT C                                  \r\n";
            SQL += "   WHERE 1=1                                                        \r\n";
            SQL += "     AND A.SPECNO = B.SPECNO                                        \r\n";
            SQL += "     AND A.PTNO = C.PANO(+)                                         \r\n";
            SQL += "     AND A.ANATNO BETWEEN " + ComFunc.covSqlstr(strANATNO_FR, false);
            SQL += "                      AND " + ComFunc.covSqlstr(strANATNO_TO, false);

            SQL += "   ORDER BY A.ANATNO                                                                                     \r\n"; 

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

        public DataSet sel_EXAM_ANATMST_RCP(PsmhDb pDbCon, string strFDate, string strTDate, enmSel_EXAM_ANATMST_RCP_WS pPCP_WS, bool isRCP)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += "  SELECT                                                                                                    \r\n";
            //2019-08-31 안정수, 감염정보 추가 
            SQL += "         KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(A.PTNO, A.BDATE) AS INFECT_INFO      -- 01                 \r\n";
            SQL += "       , NULL                                             AS INFECT_IMAGE     -- 02                         \r\n";
            SQL += "       , TO_CHAR(C.RECEIVEDATE,'YYYY-MM-DD') 			  AS RDATE    	    -- 03                           \r\n";
            SQL += "       , TO_CHAR(C.RECEIVEDATE,'HH24:MI') 				  AS RTIME    	    -- 04                           \r\n";
            SQL += "       , TO_CHAR(A.JDATE,'YYYY-MM-DD') 					  AS JDATE    	    -- 03                           \r\n";
            SQL += "       , TO_CHAR(A.JDATE,'HH24:MI') 					  AS JTIME    	    -- 04                           \r\n";
            SQL += "  	   , A.PTNO											  AS PANO   		-- 01                           \r\n";
            SQL += "       , B.SNAME                     					  AS SNAME    	    -- 02                           \r\n";
            SQL += "       , A.ORDERCODE                 					  AS ORDERCODE	    -- 05                           \r\n";
            SQL += "       , TO_CHAR(A.ORDERNO)								  AS ORDERNO		-- 06                           \r\n";
            SQL += "       , A.SPECNO										  AS SPECNO		    -- 16                           \r\n";
            SQL += "       , KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(TRIM(A.MASTERCODE)) AS MASTER_NM  -- 21                         \r\n";
            SQL += "       , A.SPECCODE										  AS SPECCODE	    -- 20                           \r\n";
            SQL += "       , KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',TRIM(A.SPECCODE),'Y') AS SPEC_NM  -- 20                         \r\n";
            SQL += "       , A.GBIO											  AS GBIO		    -- 07                           \r\n";
            SQL += "       , C.WARD                					          AS WARD   	    -- 14                           \r\n";
            SQL += "       , TRIM(A.DEPTCODE)   					          AS DEPTCODE	    -- 14                           \r\n";
            SQL += "  	   , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE) 		  AS DR_NM	 	    -- 15                           \r\n";
            SQL += "       , A.ANATNO										  AS ANATNO 		-- 17                           \r\n";
            SQL += "       , KOSMOS_OCS.FC_BAS_USER_USERNAME(A.JSABUN) 		  AS JSABUN 		-- 19                           \r\n";
            SQL += "       , A.MASTERCODE									  AS MASTERCODE	    -- 18                           \r\n";

            SQL += "       ,(                                                                                                   \r\n";
            SQL += "     	    CASE WHEN SUBSTR(A.ANATNO,1,2) = 'PS' THEN '신검(객담)'                                         \r\n";
            SQL += "     	      	 WHEN SUBSTR(A.ANATNO,1,2) = 'PU' THEN '신검(소변)'                                         \r\n";
            SQL += "     	       	 WHEN SUBSTR(A.ANATNO,1,2) = 'PW' THEN '조직(외부의뢰)'                                     \r\n";
            SQL += "     	       	 WHEN SUBSTR(A.ANATNO,1,2) = 'IH' THEN '면역염색'                                     \r\n";
            SQL += "     	       	 ELSE                                                                                       \r\n";
            SQL += "     	       	 	  CASE WHEN SUBSTR(A.ANATNO,1,1) = 'C' 	  THEN '세포'                                   \r\n";
            SQL += "     	       	 	  	   WHEN SUBSTR(A.ANATNO,1,2) = 'OS' OR SUBSTR(A.ANATNO,1,1) = 'S' THEN '조직'       \r\n";
            SQL += "     	       	 	  	   WHEN SUBSTR(A.ANATNO,1,1) = 'P' 	  THEN '신검(부인과)'                           \r\n";
            SQL += "     	       	 	  END                                                                                   \r\n";
            SQL += "  		    END                                                                                             \r\n";
            SQL += "  		) 												  AS WSCODE		 -- 22                              \r\n";
            SQL += "  	   , A.DRCODE                                  		  AS DRCODE		 -- 23                              \r\n";
            SQL += "  	   , ''												  AS BI			 -- 24                              \r\n";
            SQL += "  	   , TO_CHAR(A.BDATE, 'YYYY-MM-DD') 			 	  AS BDATE		 -- 25                              \r\n";
            SQL += "       , A.RESULTSABUN                                    AS RESULTSABUN                                    \r\n";
            SQL += "       , A.REMARK1                                        AS REMARK1                                        \r\n";
            SQL += "       , A.REMARK2                                        AS REMARK2                                        \r\n";
            SQL += "       , A.REMARK3                                        AS REMARK3                                        \r\n";
            SQL += "       , A.REMARK4                                        AS REMARK4                                        \r\n";
            SQL += "       , A.TALCHK                                         AS TALCHK                                         \r\n";       
            SQL += "       , A.GBOUT                                          AS GBOUT                                          \r\n";
            SQL += "       , A.ROWID										  AS ROWID_R                                        \r\n";
            SQL += "       , C.DRCOMMENT									  AS DRCOMMENT                                      \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_ANATMST A                                                                          \r\n";
            SQL += "       , KOSMOS_PMPA.BAS_PATIENT B                                                                          \r\n";
            SQL += "       , KOSMOS_OCS.EXAM_SPECMST C                                                                          \r\n";
            SQL += "   WHERE 1=1                                                                                                \r\n";

            if (isRCP == true)
            {
                SQL += "     AND 'R' = 'A'                                                                                      \r\n";
            }
            else
            {
                SQL += "     AND 'R' = 'R'                                                                                      \r\n";
            }
            
            ////2018-08-08 안정수, 접수대상에서 Clinical hisory Information 항목에서 Thyroid가 있을경우... 추가함
            //if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.AC)
            //{
            //    SQL += "     AND A.REMARK2 IS NOT NULL                                                                          \r\n";
            //    SQL += "     AND UPPER(A.REMARK2) Like 'THYR%'                                                                  \r\n";
            //}

            SQL += "     AND A.JDATE BETWEEN TO_DATE('" + strFDate + " 00:00','YYYY-MM-DD HH24:MI')                             \r\n";
            SQL += "   		             AND TO_DATE('" + strTDate + " 23:59','YYYY-MM-DD HH24:MI')                             \r\n";
            SQL += "     AND A.PTNO   = B.PANO                                                                                  \r\n";
            SQL += "     AND A.SPECNO = C.SPECNO                                                                                \r\n";

            if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.S)
            {
                SQL += "     AND ( A.ANATNO LIKE 'S%'  OR A.ANATNO LIKE 'OS%' )                                                 \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.C)
            {
                SQL += "     AND SUBSTR(A.ANATNO,1,1) = 'C'                                                                     \r\n";
                //2018-08-13 안정수, C와 AC 분리되면서 조건 추가함
                //SQL += "     AND UPPER(SUBSTR(A.REMARK2, 1, 4)) <> 'THYR'                                                       \r\n";
                SQL += "     AND KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(TRIM(A.MASTERCODE)) NOT IN ( 'Asp. bio. cytology', 'Asp. bio. cytology &cell block')   \r\n";
                
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.P)
            {
                SQL += "     AND (SUBSTR(A.ANATNO,1,1) = 'P' AND SUBSTR(A.ANATNO,2,1) != 'S' AND SUBSTR(A.ANATNO,2,1) != 'U')         \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.PS)
            {
                SQL += "     AND SUBSTR(A.ANATNO,1,2) = 'PS'                                                                   \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.PU)
            {
                SQL += "     AND SUBSTR(A.ANATNO,1,2) = 'PU'                                                                   \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.W)
            {
                SQL += "     AND SUBSTR(A.ANATNO,1,2) = 'SW'                                                                   \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.IH)
            {
                SQL += "     AND SUBSTR(A.ANATNO,1,2) = 'IH'                                                                   \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.T)
            {
                SQL += "     AND 'T' = 'A'                                                                                     \r\n";
            }
            //2018-08-13 안정수, 접수대상에서 Clinical hisory Information 항목에서 Thyroid가 있을경우... 추가함
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.AC)
            {
                SQL += "     AND A.REMARK2 IS NOT NULL                                                                          \r\n";
                //SQL += "     AND UPPER(A.REMARK2) Like 'THYR%'                                                                  \r\n";
                SQL += "     AND KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(TRIM(A.MASTERCODE)) IN ( 'Asp. bio. cytology', 'Asp. bio. cytology &cell block')                  \r\n";
            }

            SQL += "   UNION ALL                                                                                            \r\n";
            SQL += "   SELECT                                                                                               \r\n";
            //2019-08-31 안정수, 감염정보 추가 
            SQL += "         KOSMOS_OCS.FC_EXAM_INFECT_MASTER_IMG_EX(S.PANO, S.BDATE) AS INFECT_INFO    -- 01               \r\n";
            SQL += "      , NULL                                                AS INFECT_IMAGE         -- 02               \r\n";
            SQL += "      , TO_CHAR(S.RECEIVEDATE, 'YYYY-MM-DD') 	            AS RDATE		 		-- 03               \r\n";
            SQL += "      , TO_CHAR(S.RECEIVEDATE, 'HH24:MI')				    AS RTIME				-- 04               \r\n";
            SQL += "      , '' 	                                                AS JDATE		 		-- 03               \r\n";
            SQL += "      , ''                              				    AS JTIME				-- 04               \r\n";
            SQL += "  	  , R.PANO									            AS PANO				    -- 01               \r\n";
            SQL += "      , S.SNAME									            AS SNAME				-- 02               \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_ORDER_ORDERCODE(S.PANO, S.BDATE, S.SPECNO,S.DEPTCODE,S.IPDOPD,'Y', R.MASTERCODE)	AS ORDERCODE	-- 05   \r\n";
            SQL += "  	  , KOSMOS_OCS.FC_EXAM_ORDER_ORDERCODE(S.PANO, S.BDATE, S.SPECNO,S.DEPTCODE,S.IPDOPD,'N', R.MASTERCODE)	AS ORDERNO		-- 06   \r\n";
            SQL += "      , S.SPECNO									        AS SPECNO	   			-- 16               \r\n";
            SQL += "  	  , KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(TRIM(R.MASTERCODE))    AS MASTER_NM	    -- 21               \r\n";
            SQL += "      , S.SPECCODE								            AS SPECCODE    			-- 20               \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',TRIM(S.SPECCODE),'Y')  AS SPEC_NM	  			-- 20       \r\n";
            SQL += "      , S.IPDOPD                             	            AS GBIO	      		    -- 07               \r\n";
            SQL += "      , S.WARD                               	            AS WARD	      		    -- 07               \r\n";
            SQL += "  	  , S.DEPTCODE       						            AS DEPTCODE   	   		-- 14               \r\n";
            SQL += "  	  , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(S.DRCODE)           AS DR_NM	   			-- 15               \r\n";

            SQL += "      , S.ANATNO									        AS ANATNO             	-- 17               \r\n";
            SQL += "      , ''										            AS JSABUN				-- 19               \r\n";
            SQL += "      , R.MASTERCODE								        AS MASTERCODE 			-- 18               \r\n";
            
            SQL += "  	  , KOSMOS_OCS.FC_EXAM_MASTER_PHTL_BUN(R.MASTERCODE, S.BI) AS WSCODE	        -- 22               \r\n";
            SQL += "  	  , S.DRCODE                                            AS DRCODE				-- 23               \r\n";
            SQL += "      , S.BI										        AS BI					-- 24               \r\n";
            SQL += "      , TO_CHAR(S.BDATE, 'YYYY-MM-DD') 			            AS BDATE				-- 25               \r\n";
            SQL += "      , '' 										            AS RESULTSABUN                              \r\n";
            SQL += "      , ''										            AS REMARK1                                  \r\n";
            SQL += "      , ''										            AS REMARK2                                  \r\n";
            SQL += "      , ''										            AS REMARK3                                  \r\n";
            SQL += "      , ''										            AS REMARK4                                  \r\n";
            SQL += "      , ''										            AS TALCHK                                   \r\n";
            SQL += "      , ''										            AS GBOUT                                     \r\n";
            SQL += "      , S.ROWID									            AS ROWID_R                                  \r\n";
            SQL += "      , S.DRCOMMENT									  AS DRCOMMENT                                      \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_SPECMST S                                                                       \r\n";
            SQL += "      , KOSMOS_OCS.EXAM_RESULTC R                                                                       \r\n";
            SQL += "  WHERE 1=1                                                                                             \r\n";

            if (isRCP == true)
            {
                SQL += "     AND 'R' = 'R'                                                                                  \r\n";
            }
            else
            {
                SQL += "     AND 'R' = 'A'                                                                                  \r\n";
            }

            SQL += "    AND S.RECEIVEDATE BETWEEN TO_DATE('" + strFDate + " 00:00','YYYY-MM-DD HH24:MI')                    \r\n";
            SQL += "                          AND TO_DATE('" + strTDate + " 23:59','YYYY-MM-DD HH24:MI')                    \r\n";
            SQL += "    AND R.SPECNO     = S.SPECNO                                                                         \r\n";
            SQL += "    AND S.STATUS     NOT IN ('00','05','06')                                                            \r\n";

            if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.A)
            {
                SQL += "    AND R.MASTERCODE NOT IN ('XS01', 'XS03', 'XS05')                                                \r\n";
                SQL += "    AND R.MASTERCODE IN (                                                                           \r\n";
                SQL += "                            SELECT MASTERCODE FROM KOSMOS_OCS.EXAM_MASTER WHERE WSCODE1 > '599' AND WSCODE1 < '900'  AND SUBSTR(MASTERCODE,1,2) IN ('XR','XS','O-')    \r\n";
                SQL += "                        )                                                                           \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.S)
            {
                SQL += "    AND R.MASTERCODE NOT IN ('XS01', 'XS03', 'XS05')                                                \r\n";
                SQL += "    AND R.MASTERCODE IN (                                                                           \r\n";
                SQL += "                            SELECT MASTERCODE FROM KOSMOS_OCS.EXAM_MASTER WHERE WSCODE1 > '699' AND WSCODE1 < '800'    \r\n";
                SQL += "                        )                                                                           \r\n";
                SQL += "     AND KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(TRIM(R.MASTERCODE)) NOT IN ( 'Decalcification')   \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.C)
            {
                // OptAnat(2).Value = True
                SQL += "    AND S.Bi <> '62'                                                                                \r\n";
                SQL += "    AND R.MASTERCODE IN (                                                                           \r\n";
                SQL += "                            SELECT MASTERCODE FROM KOSMOS_OCS.EXAM_MASTER WHERE WSCODE1 > '599' AND WSCODE1 < '700'    \r\n";
                SQL += "                        )                                                                           \r\n";
                //2018-08-17 안정수 추가
                SQL += "     AND KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(TRIM(R.MASTERCODE)) NOT IN ( 'Asp. bio. cytology', 'Asp. bio. cytology &cell block')                                    \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.P)
            {

                SQL += "    AND S.Bi = '62'                                                                                 \r\n";
                SQL += "    AND R.MASTERCODE IN (                                                                           \r\n";
                SQL += "                            SELECT MASTERCODE FROM KOSMOS_OCS.EXAM_MASTER WHERE WSCODE1 > '599' AND WSCODE1 < '700' AND MASTERCODE NOT IN ('YR01','YU01','YU03','YU05')     \r\n";
                SQL += "                        )                                                                           \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.PS)
            {
                SQL += "    AND S.Bi = '62'                                                                                 \r\n";
                SQL += "    AND R.MASTERCODE IN (                                                                           \r\n";
                SQL += "                            SELECT MasterCode FROM KOSMOS_OCS.EXAM_MASTER WHERE MASTERCODE ='YR01'  \r\n";
                SQL += "                        )                                                                           \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.PU)
            {
                SQL += "    AND S.Bi = '62'                                                                                \r\n";
                SQL += "    AND R.MASTERCODE IN (                                                                                                \r\n";
                SQL += "                            SELECT MasterCode FROM KOSMOS_OCS.EXAM_MASTER WHERE MASTERCODE IN ('YU01','YU03','YU05')     \r\n";
                SQL += "                        )                                                                                                \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.W)
            {
                SQL += "    AND R.MASTERCODE IN (                                                                                                \r\n";
                SQL += "                            SELECT MASTERCODE FROM KOSMOS_OCS.EXAM_MASTER WHERE WSCODE1 >= '800' AND WSCODE1 < '900'   AND SUBSTR(MASTERCODE,1,2) IN ('XR','XS','O-','XG')     \r\n";
                SQL += "                        )                                                                                                \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.IH)
            {
                SQL += "    AND R.MASTERCODE IN (                                                                                                \r\n";
                SQL += "                            SELECT MASTERCODE FROM KOSMOS_OCS.EXAM_MASTER WHERE WSCODE1 >= '800' AND WSCODE1 < '900'   AND SUBSTR(MASTERCODE,1,5) IN ('O-XIH')     \r\n";
                SQL += "                        )                                                                                                \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.T)
            {
                SQL += "    AND R.MASTERCODE IN ('XS01', 'XS03', 'XS05')                                                \r\n";
            }
            //2018-08-13 안정수, 접수대상에서 Clinical hisory Information 항목에서 Thyroid가 있을경우... 추가함
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.AC)
            {
                SQL += "     AND KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(TRIM(R.MASTERCODE)) IN ( 'Asp. bio. cytology', 'Asp. bio. cytology &cell block')                                   \r\n";
            }
            else if (pPCP_WS == enmSel_EXAM_ANATMST_RCP_WS.TS)
            {
                SQL += "     AND KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(TRIM(R.MASTERCODE))  IN ( 'Decalcification')   \r\n";
            }
            //2018-08-08 안정수, 접수대상에서 Clinical hisory Information 항목에서 Thyroid가 있을경우... 추가함
            //if (isAC == true)
            //{
            //    SQL += "     AND KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(TRIM(R.MASTERCODE)) = 'Asp. bio. cytology'                                   \r\n";                
            //}

            SQL += "    AND R.ANATNO     IS NULL                                                                                                 \r\n";
            SQL += "    AND R.STATUS     NOT IN  ('V', 'Y')                                                                                      \r\n";

            SQL += " ORDER BY SNAME                                                                                                \r\n";

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

        public string save_EXAM_ANATMST(PsmhDb pDbCon, DataRow dr, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  MERGE INTO KOSMOS_OCS.EXAM_ANATMST                                                                                \r\n";
            SQL += "       USING DUAL                                                                                                   \r\n";
            SQL += "          ON (                                                                                                      \r\n";
            SQL += "          	    SPECNO  = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ANATMST_RCP.SPECNO].ToString().Trim(), false);
            SQL += "  	   		 )                                                                                                      \r\n";
            SQL += " 	WHEN MATCHED THEN                                                                                               \r\n";
            SQL += " 		UPDATE SET ORDERNO    = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ANATMST_RCP.ORDERNO].ToString().Trim(), false);
            SQL += "                 , GBIO       = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ANATMST_RCP.GBIO].ToString().Trim(), false);
            SQL += "                 , GBJOB      = " + ComFunc.covSqlstr("N", false);
            SQL += "                 , REMARK1    = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ANATMST_RCP.REMARK1].ToString().Trim(), false);
            SQL += "                 , REMARK2    = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ANATMST_RCP.REMARK2].ToString().Trim(), false);
            SQL += "                 , REMARK3    = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ANATMST_RCP.REMARK3].ToString().Trim(), false);
            SQL += "                 , REMARK4    = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ANATMST_RCP.REMARK4].ToString().Trim(), false);
            SQL += "                 , MASTERCODE = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ANATMST_RCP.MASTERCODE].ToString().Trim(), false);
            SQL += "                 , SPECCODE   = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ANATMST_RCP.SPECCODE].ToString().Trim(), false);
            SQL += "                 , JDATE      = SYSDATE \r\n";
            SQL += "                 , TALCHK     = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ANATMST_RCP.TALCHK].ToString().Trim(), false);
            SQL += "                 , JSABUN     = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += "                 , GBOUT      = " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ANATMST_RCP.GBOUT].ToString().Trim(), false);
            SQL += "    WHEN NOT MATCHED THEN                           \r\n";
            SQL += "   INSERT                                           \r\n";
            SQL += "         (                                          \r\n";
            SQL += "             PTNO                                   \r\n";
            SQL += "          ,  BDATE                                  \r\n";
            SQL += "          ,  ORDERCODE                              \r\n";
            SQL += "          ,  ORDERNO                                \r\n";
            SQL += "          ,  GBIO                                   \r\n";
            SQL += "          ,  GBJOB                                  \r\n";
            SQL += "          ,  DEPTCODE                               \r\n";
            SQL += "          ,  DRCODE                                 \r\n";
            SQL += "          ,  SPECNO                                 \r\n";
            SQL += "          ,  ANATNO                                 \r\n";
            SQL += "          ,  MASTERCODE                             \r\n";
            SQL += "          ,  REMARK1                                \r\n";
            SQL += "          ,  REMARK2                                \r\n";
            SQL += "          ,  REMARK3                                \r\n";
            SQL += "          ,  REMARK4                                \r\n";
            SQL += "          ,  JDATE                                  \r\n";
            SQL += "          ,  SPECCODE                               \r\n";
            SQL += "          ,  TALCHK                                 \r\n";
            SQL += "          ,  JSABUN                                 \r\n";
            SQL += "          ,  GBOUT                                  \r\n";
            SQL += "        )                                           \r\n";
            SQL += "    VALUES                                          \r\n";
            SQL += "    	(                                           \r\n";
            SQL += "         " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ANATMST_RCP.PANO].ToString().Trim(), false);
            SQL += "         " + ComFunc.covSqlDate(dr[(int)enmSel_EXAM_ANATMST_RCP.BDATE].ToString().Trim(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ANATMST_RCP.ORDERCODE].ToString().Trim(), true);
            SQL += "         ,'0'   \r\n"; 
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ANATMST_RCP.GBIO].ToString().Trim(), true);
            SQL += "         " + ComFunc.covSqlstr ("N", true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ANATMST_RCP.DEPTCODE].ToString().Trim(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ANATMST_RCP.DRCODE].ToString().Trim(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ANATMST_RCP.SPECNO].ToString().Trim(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ANATMST_RCP.ANATNO].ToString().Trim(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ANATMST_RCP.MASTERCODE].ToString().Trim(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ANATMST_RCP.REMARK1].ToString().Trim(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ANATMST_RCP.REMARK2].ToString().Trim(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ANATMST_RCP.REMARK3].ToString().Trim(), true);
            SQL += "         " + ComFunc.covSqlstr (dr[(int)enmSel_EXAM_ANATMST_RCP.REMARK4].ToString().Trim(), true);
            SQL += "         , SYSDATE                                   \r\n";
            SQL += "         " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ANATMST_RCP.SPECCODE].ToString().Trim(), true);
            SQL += "         " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ANATMST_RCP.TALCHK].ToString().Trim(), true);
            SQL += "         " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "         " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_ANATMST_RCP.GBOUT].ToString().Trim(), true);
            SQL += "    	)                                           \r\n";

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

        public string ins_EXAM_ANATNO(PsmhDb pDbCon, string strGubun, ref int intRowAffected)
        {
            string SqlErr = ""; 

            SQL = "";
            SQL += " INSERT INTO KOSMOS_OCS.EXAM_ANATNO( YYYY, GUBUN,  ANATNO)  VALUES (   \r\n";
            SQL += "    TO_CHAR(SYSDATE,'YYYY')           \r\n";
            SQL += "  , " + ComFunc.covSqlstr(strGubun, false);
            SQL += "  , " + ComFunc.covSqlstr("1", false);
            SQL += "     )";

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

        public string up_EXAM_ANATNO(PsmhDb pDbCon, string strGubun, string strANATNO, string strROWID,  ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_ANATNO                   \r\n";
            SQL += "    SET  ANATNO =  " + ComFunc.covSqlstr(strANATNO, false);
            SQL += "   WHERE 1 = 1                                            \r\n";

            if (string.IsNullOrEmpty(strROWID) == false)
            {
                SQL += "     AND ROWID  = " +  ComFunc.covSqlstr(strROWID, false);
            }
            else
            {
                SQL += "     AND YYYY  = TO_CHAR(SYSDATE,'YYYY')                \r\n";
                SQL += "     AND GUBUN = " + ComFunc.covSqlstr(strGubun, false);
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

        public DataTable sel_EXAM_ANATNO(PsmhDb pDbCon, string strGUBUN)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT CASE WHEN COUNT(*) = 0 THEN 'N'                   \r\n";
            SQL += "              ELSE  TO_CHAR(MAX(ANATNO) + 1) END AS ANATNO    \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_ANATNO A                \r\n";
            SQL += "   WHERE 1 = 1                                            \r\n";
            SQL += "     AND A.YYYY  = TO_CHAR(SYSDATE,'YYYY')                \r\n";
            SQL += "     AND A.GUBUN = " + ComFunc.covSqlstr(strGUBUN, false);

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);


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

        public DataTable sel_EXAM_ANATMST_ISSPECNO(PsmhDb pDbCon, string strSPECNO)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                                     \r\n";
            SQL += "        COUNT(*) CNT                                        \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_ANATMST A                 \r\n";
            SQL += "   WHERE 1 = 1                                              \r\n";
            SQL += "     AND A.SPECNO = " + ComFunc.covSqlstr(strSPECNO, false);

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);


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

        public DataTable sel_EXAM_ANATMST_ISANATNO(PsmhDb pDbCon, string strANTNO)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                                     \r\n";
            SQL += "        COUNT(*) CNT                                        \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_ANATMST A                 \r\n";
            SQL += "   WHERE 1 = 1                                              \r\n";
            SQL += "     AND A.ANATNO = " + ComFunc.covSqlstr(strANTNO, false);

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);


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

        public DataTable sel_EXAM_ANATMST_ANATNO(PsmhDb pDbCon, string strANTNO)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                                     \r\n";
            SQL += "        PTNO, SPECNO,ANATNO                                 \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_ANATMST A                 \r\n";
            SQL += "   WHERE 1 = 1                                              \r\n";
            SQL += "     AND A.ANATNO = " + ComFunc.covSqlstr(strANTNO, false);

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);


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

        public string up_EXAM_ANATMST_CYTO(PsmhDb pDbCon, string strROWID, string strREMARK1, string strREMARK2, string strREMARK3, string strREMARK4, ref int intRowAffected)
        {
            string SqlErr = "";
            string SQL;

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_ANATMST  ";
            SQL += "    SET REMARK1 = " + ComFunc.covSqlstr(strREMARK1, false);
            SQL += "      , REMARK2 = " + ComFunc.covSqlstr(strREMARK2, false);
            SQL += "      , REMARK3 = " + ComFunc.covSqlstr(strREMARK3, false);
            SQL += "      , REMARK4 = " + ComFunc.covSqlstr(strREMARK4, false);
            SQL += "  WHERE ROWID   = " + ComFunc.covSqlstr(strROWID, false);

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

        public string up_EXAM_RESULTC_ANATNO(PsmhDb pDbCon, string strSPECNO, bool isChange, string strANATNO_FR, string strANATNO_TO, ref int intRowAffected)
        {
            string SqlErr = "";
            string SQL;

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_RESULTC  ";

            if (isChange == true)
            {
                SQL += "    SET ANATNO     = " + ComFunc.covSqlstr(strANATNO_TO, false);
                SQL += "  WHERE ANATNO     = " + ComFunc.covSqlstr(strANATNO_FR, false);
                SQL += "    AND SPECNO     = " + ComFunc.covSqlstr(strSPECNO, false);
            }
            else
            {
                SQL += "    SET ANATNO     = " + ComFunc.covSqlstr(strANATNO_FR, false);
                SQL += "  WHERE SPECNO     = " + ComFunc.covSqlstr(strSPECNO, false);

            }

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

        public string up_EXAM_SPECMST_ANATNO(PsmhDb pDbCon, string strSPECNO, bool isChange, string strANATNO_FR, string strANATNO_TO, ref int intRowAffected)
        {
            string SqlErr = "";
            string SQL;

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_SPECMST  ";

            if (isChange == true)
            {
                SQL += "    SET ANATNO     = " + ComFunc.covSqlstr(strANATNO_TO, false);
                SQL += "  WHERE ANATNO     = " + ComFunc.covSqlstr(strANATNO_FR, false);
                SQL += "    AND SPECNO     = " + ComFunc.covSqlstr(strSPECNO, false);
            }
            else
            {
                SQL += "    SET ANATNO     = " + ComFunc.covSqlstr(strANATNO_FR, false);
                SQL += "  WHERE SPECNO     = " + ComFunc.covSqlstr(strSPECNO, false);
            }

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

        public string up_EXAM_ANATMST_ANATNO(PsmhDb pDbCon, string strANATNO_FR, string strANATNO_TO, ref int intRowAffected)
        {
            string SqlErr = "";
            string SQL;

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_ANATMST  ";
            SQL += "    SET ANATNO     = " + ComFunc.covSqlstr(strANATNO_TO, false);
            SQL += "  WHERE ANATNO     = " + ComFunc.covSqlstr(strANATNO_FR, false);

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

        public string up_EXAM_ANATMST_TALCHK(PsmhDb pDbCon, string strROWID, bool isTALCHK, ref int intRowAffected)
        {
            string SqlErr = "";
            string SQL;

            SQL = "";

            if (string.IsNullOrEmpty(strROWID) == false)
            {
                SQL = "";
                SQL += " UPDATE KOSMOS_OCS.EXAM_ANATMST  ";

                if (isTALCHK == true)
                {
                    SQL += "    SET TALCHK    = TO_CHAR(SYSDATE,'YYYYMMDD') \r\n";
                }
                else
                {
                    SQL += "    SET TALCHK    = '' \r\n";
                }

                SQL += " WHERE ROWID     = " + ComFunc.covSqlstr(strROWID, false);
            }

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

        public string del_EXAM_ANATMST(PsmhDb pDbCon, string strANATNO, ref int intRowAffected)
        {
            string SqlErr = "";
            string SQL;

            SQL = "";

            SQL = "";
            SQL += " DELETE KOSMOS_OCS.EXAM_ANATMST  ";
            SQL += " WHERE ANATNO     = " + ComFunc.covSqlstr(strANATNO, false);

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

        /// <summary>병리 Request For Cytology 출력</summary>
        /// <param name="strSpecNo">검체번호</param>
        /// <returns></returns>
        public DataTable sel_EXAM_SPECMST_ANATPrint(PsmhDb pDbCon,string strSpecNo)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";
            SQL += " SELECT                                                                                                             \r\n";
            SQL += "        A.PANO                                                                                  AS PANO             \r\n";
            SQL += "      , A.SNAME                                                                                 AS SNAME            \r\n";
            SQL += "      , B.JUMIN1                                                                                AS JUMIN1           \r\n";
            SQL += "      , B.JUMIN2                                                                                AS JUMIN2           \r\n";
            SQL += "      , DECODE(A.IPDOPD, 'I', '(I) ' || A.WARD || '-' || A.ROOM                                                     \r\n";
            SQL += "                            , '(O) ' || KOSMOS_OCS.FC_BAS_CLINICDEPT_DEPTNAMEK(A.DEPTCODE))     AS WARD_NAME        \r\n";
            SQL += "      , KOSMOS_OCS.FC_EXAM_SPECMST_NM('14', A.SPECCODE, 'N')                                    AS SPEC_NAME        \r\n";
            SQL += "      , (                                                                                                           \r\n";
            SQL += "             CASE WHEN SUBSTR(A.ANATNO, 1, 2) = 'SW' THEN SUBSTR(A.ANATNO, 1, 4) || '-' || SUBSTR(A.ANATNO, 5)      \r\n";
            SQL += "                  ELSE SUBSTR(A.ANATNO, 1, 3) || '-' || SUBSTR(A.ANATNO, 4)                                         \r\n";
            SQL += "              END                                                                                                   \r\n";
            SQL += " 	    )                                                                                       AS ANATNO           \r\n";
            SQL += "      , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE)                                               AS DR_NAME          \r\n";
            SQL += "      , SUBSTR(TO_CHAR(A.BDATE, 'YYYYMMDD'), 1, 4) || '년 ' ||                                                      \r\n";
            SQL += "         SUBSTR(TO_CHAR(A.BDATE, 'YYYYMMDD'), 5, 2) || '월 ' ||                                                     \r\n";
            SQL += "         SUBSTR(TO_CHAR(A.BDATE, 'YYYYMMDD'), 7, 2) || '일'                                     AS BDATE            \r\n";
            SQL += "   FROM " + ComNum.DB_MED  + "EXAM_SPECMST A                                                                        \r\n";
            SQL += "      , " + ComNum.DB_PMPA + "BAS_PATIENT  B                                                                        \r\n";
            SQL += "   WHERE 1 = 1                                                                                                      \r\n";
            SQL += "     AND A.SPECNO = " + ComFunc.covSqlstr(strSpecNo, false);
            SQL += "     AND A.PANO   = B.PANO                                                                                          \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);


                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
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

        /// <summary></summary>
        /// <param name="strFDate"></param>
        /// <param name="strTDate"></param>
        /// <param name="enmPthlType"></param>
        /// <param name="isGBTATSAU"></param>
        /// <param name="isChk"></param>
        /// <returns></returns>
        public DataSet sel_EXAM_ANATMST_TAT(PsmhDb pDbCon,string strFDate, string strTDate, enmANATMST_TYPE enmPthlType, enumTAT_SEARCH_TYPE isGBTATSAU,bool isChk )
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                                                                                                     \r\n";
            SQL += " 	  A.ANATNO                                                                                       AS ANATNO              \r\n";
            SQL += " 	, A.PTNO                                                                                         AS PANO                \r\n";
            SQL += "  	, D.RECEIVEDATE																					 AS EXAM_RECEIVEDATE    \r\n";
            SQL += "  	, TO_CHAR(A.JDATE     ,'YYYY-MM-DD')      														 AS JDATE               \r\n";
            SQL += "  	, TO_CHAR(A.JDATE     ,'HH24:MI')      	    													 AS JTIME               \r\n";
            SQL += "  	, TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') 															 AS RDATE               \r\n";
            SQL += "  	, TO_CHAR(A.RESULTDATE,'HH24:MI')																 AS RTIME               \r\n";
            SQL += " 	, B.GBTAT																						 AS GBTAT               \r\n";
            SQL += " 	, TO_CHAR(KOSMOS_OCS.FC_DATE_ILSU(TO_CHAR(A.RESULTDATE,'YYYY-MM-DD'),TO_CHAR(A.JDATE,'YYYY-MM-DD'))) AS ILLDATE         \r\n";
            SQL += " 	, KOSMOS_OCS.FC_DATE_ILSU(TO_CHAR(A.RESULTDATE,'YYYY-MM-DD'),TO_CHAR(A.JDATE,'YYYY-MM-DD'))- B.GBTAT AS ILLGAP          \r\n";
            SQL += "	, KOSMOS_OCS.FC_BAS_PASS_NAME(A.RESULTSABUN)													 AS RESULT_NAME         \r\n";
            SQL += "  	, DECODE(B.TONGBUN,'T',B.EXAMNAME, 'A',C.YNAME)													 AS EXAM_NAME           \r\n";
            SQL += "  	, A.GBTATSAU                                                                                                            \r\n";
            SQL += "  	, A.GBTATBUN                                                                                                            \r\n";
            SQL += "  	, A.GBTATBUN2                                                                                                           \r\n";
            SQL += "  	, A.GBTATBUN3                                                                                                           \r\n";
            SQL += "  	, A.RESULT1                                                                                                             \r\n";
            SQL += "  	, A.RESULT2                                                                                                             \r\n";
            SQL += "  	, ''                                                                                             AS CHANGE              \r\n";
            SQL += "  	, A.ROWID                                                                                                               \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_ANATMST A                                                                                            \r\n";
            SQL += "     , KOSMOS_OCS.EXAM_MASTER  B                                                                                            \r\n";
            SQL += "     , (                                                                                                                    \r\n";
            SQL += "     	SELECT  YNAME                                                                                                       \r\n";
            SQL += "              , CODE                                                                                                        \r\n";
            SQL += "         FROM KOSMOS_OCS.EXAM_SPECODE                                                                                       \r\n";
            SQL += "        WHERE GUBUN = '14'                                                                                                  \r\n";
            SQL += "        )                      C                                                                                            \r\n";
            SQL += "     , KOSMOS_OCS.EXAM_SPECMST D                                                                                            \r\n";
            SQL += " WHERE 1=1                                                                                                                  \r\n";
            SQL += "   AND A.RESULTDATE 		>= " + ComFunc.covSqlDate(strFDate, false);
            SQL += "   AND A.RESULTDATE 		<  " + ComFunc.covSqlDate(strTDate, false);
            SQL += "   AND A.GBJOB 				= 'V'                                                                                           \r\n";
            SQL += "   AND TRIM(A.MASTERCODE) 	= B.MASTERCODE                                                                                  \r\n";
            SQL += "   AND B.GBTAT IS NOT NULL                                                                                                  \r\n";
            SQL += "   AND A.SPECNO			    = D.SPECNO                                                                                      \r\n";
            SQL += "   AND B.GBTAT 				IS NOT NULL                                                                                     \r\n";
            SQL += "   AND TRIM(A.SPECCODE)	    = C.CODE(+)                                                                                     \r\n";
            SQL += "   AND B.TONGBUN  			= " + ComFunc.covSqlstr(enmPthlType.ToString(), false);
            if (enmPthlType == enmANATMST_TYPE.T)
            {
                SQL += "   AND A.ANATNO 			NOT LIKE  'OS%'                                                                                 \r\n";
            }
            else if (enmPthlType == enmANATMST_TYPE.A)
            {
                SQL += "   AND A.DEPTCODE 			<> 'HR'                                                                                         \r\n";
            }

            //기준내
            if (isGBTATSAU == enumTAT_SEARCH_TYPE.IN)
            {
                SQL += "   AND A.GBTATSAU 			IS NULL                                                                                         \r\n";

                if (isChk == false)
                {
                    //제외포함
                    SQL += "   AND A.GBTATBUN 			IS NULL                                                                                         \r\n";
                }                
            }
            //지연
            else if (isGBTATSAU == enumTAT_SEARCH_TYPE.DEILY)
            {
                SQL += "   AND A.GBTATSAU 			IS NOT NULL                                                                                         \r\n";
            }
                                                           
            SQL += " ORDER BY ANATNO ASC                                                                                                        \r\n";

            try
            {
                SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);


                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
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

        public string up_EXAM_ANATMST(PsmhDb pDbCon, string strGBTATSAU, string strGBTATBUN,  string strGBTATBUN2, string strGBTATBUN3, string strROWID, ref int intRowAffected)
        {
            string SqlErr = "";
            string SQL;

            SQL = "";

            if (string.IsNullOrEmpty(strROWID) == false)
            {
                SQL = "";
                SQL += " UPDATE KOSMOS_OCS.EXAM_ANATMST  ";
                SQL += "    SET GBTATSAU  = " + ComFunc.covSqlstr(strGBTATSAU , false);
                SQL += "      , GBTATBUN  = " + ComFunc.covSqlstr(strGBTATBUN , false);
                SQL +=  "     , GBTATBUN2 = " + ComFunc.covSqlstr(strGBTATBUN2, false);
                SQL +=  "     , GBTATBUN3 = " + ComFunc.covSqlstr(strGBTATBUN3, false);
                SQL +=  " WHERE ROWID     = " + ComFunc.covSqlstr(strROWID, false);

            }

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

    }
}
