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
    /// File Name : clsPthlSQL.cs
    /// Title or Description : 조직병리 SQL
    /// Author : 김홍록
    /// Create Date : 2017-06-07
    /// Update History : 
    /// </summary>
    public class clsPthlSQL : Com.clsMethod
    {
        string SQL = string.Empty; 

        /// <summary>Sel_EXAM_SPECMST_ANATPrint 조회결과</summary>
        public enum enmSel_EXAM_SPECMST_ANATPrint {PANO, SNAME,JUMIN1,JUMIN2,WARD_NAME,SPEC_NAME,ANATNO,DR_NAME,BDATE };

        /// <summary>Sel_EXAM_ANATMST_TAT</summary>
        public enum enmSel_EXAM_ANATMST_TAT       {     ANATNO,        PTNO, EXAM_RECEIVEDATE,            JDATE,           JTIME,      RDATE,                 RTIME,           GBTAT,         ILLDATE,       ILLGAP,      RESULT_NAME,         EXAM_NAME,         GBTATSAU,         GBTATBUN,       GBTATBUN2,       GBTATBUN3,   RESULT1,   RESULT2, CHANGE, ROWID_R};
        public string[] sSel_EXAM_ANATMST_TAT_T = { "병리번호",  "등록번호",       "병리접수일시",       "접수일자",      "접수시각",       "보고일자",      "보고시각",      "기준일수",      "경과일수",     "차수",       "병리의사",          "검체"  ,       "지연사유",       "제외사유",         "제외2",         "제외3", "RESULT1", "RESULT2", "CHANGE", "ROWID_R" };
        public string[] sSel_EXAM_ANATMST_TAT_A = { "병리번호",  "등록번호",       "병리접수일시",       "접수일자",      "접수시각",       "보고일자",      "보고시각",      "기준일수",      "경과일수",     "차수",       "병리의사",          "검사명",       "지연사유",       "제외사유",         "제외2",         "제외3", "RESULT1", "RESULT2", "CHANGE", "ROWID_R" };
        public int   [] nSel_EXAM_ANATMST_TAT   = {  nCol_PANO,   nCol_PANO,            nCol_TIME,        nCol_DATE,              40,        nCol_DATE,              40,              40,              40,          5,       nCol_SNAME,   nCol_SNAME + 65,        nCol_NAME,        nCol_NAME,        nCol_AGE,        nCol_AGE,         5,         5,        5,        5 };

        public enum enmANATMST_TYPE {
            /// <summary>병리</summary>
            T
                /// <summary>세포</summary>
                , A };

        public enum enumTAT_SEARCH_TYPE {ALL,IN,DEILY};

        public enum enmSel_EXAM_ANATMST_ANATPrint { SNAME, AGE_SEX, DEPT_NM, JUMIN1, JUMIN2, JUMIN, COMP_NM, DRCOMMENT, REMARK1, REMARK2, REMARK3, REMARK4, REMARK5, DR_NAME, WARD_NAME, ANATNO, BDATE, PANO, DEPTCODE, RECEIVEDATE, JDATE, RESULTDATE,SEX,AGE, RESULTSABUN, GBSNAME, RESULT_C, SPECODE, HRREMARK1, HRREMARK2, HRREMARK3, HRREMARK4, HRREMARK5, Result1, Result2, AnatName };

        public DataTable sel_EXAM_ANATMST_ANATPrint(PsmhDb pDbCon, string strSpecNo)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                                                     \r\n";
            SQL += " 		  B.SNAME                							 AS SNAME       \r\n";
            SQL += " 		, B.AGE || '/' || DECODE(B.SEX,'M','남','여')		 AS AGE_SEX     \r\n";
            SQL += " 		, KOSMOS_OCS.FC_BAS_CLINICDEPT_DEPTNAMEK(B.DEPTCODE) AS DEPT_NM     \r\n";
            SQL += "        , C.JUMIN1                  						 AS JUMIN1      \r\n";
            SQL += "        , C.JUMIN2						                     AS JUMIN2      \r\n";
            SQL += "        , C.JUMIN1 || '-' || C.JUMIN2						 AS JUMIN       \r\n";
            SQL += "        , (                                                                 \r\n";
            SQL += "	        SELECT MAX(C1.NAME)                                             \r\n";
            SQL += "			 FROM KOSMOS_PMPA.HIC_JEPSU A1                                  \r\n";
            SQL += "			     , KOSMOS_PMPA.HIC_LTD C1                                   \r\n";
            SQL += "			WHERE 1=1                                                       \r\n";
            SQL += "			  AND A1.PTNO = B.PANO                                          \r\n";
            SQL += "			  AND A1.JEPDATE = (SELECT MAX(B1.JEPDATE)                      \r\n";
            SQL += "			                     FROM KOSMOS_PMPA.HIC_JEPSU B1              \r\n";
            SQL += "			                     WHERE B1.PTNO = B.PANO                     \r\n";
            SQL += "			                     )                                          \r\n";
            SQL += "			  AND A1.LTDCODE = C1.CODE                                      \r\n";
            SQL += "			)												AS COMP_NM      \r\n";
            SQL += "		, B.DRCOMMENT                                       AS DRCOMMENT    \r\n";
            SQL += "		, A.REMARK1                                         AS REMARK1    \r\n";
            SQL += "		, A.REMARK2                                         AS REMARK2    \r\n";
            SQL += "		, A.REMARK3                                         AS REMARK3    \r\n";
            SQL += "		, A.REMARK4                                         AS REMARK4    \r\n";
            SQL += "		, A.REMARK5                                         AS REMARK5    \r\n";
            SQL += "		, KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(B.DRCODE)         AS DR_NAME      \r\n";
            SQL += "        , DECODE(B.IPDOPD, 'I', B.WARD || '-' || B.ROOM                     \r\n";
            //SQL += "                              , '(O) ' || KOSMOS_OCS.FC_BAS_CLINICDEPT_DEPTNAMEK(B.DEPTCODE))     AS WARD_NAME        \r\n";
            SQL += "                              , '')     AS WARD_NAME        \r\n";
            SQL += "		, A.ANATNO                                          AS ANATNO       \r\n";
            SQL += "        , SUBSTR(TO_CHAR(B.BDATE, 'YYYYMMDD'), 1, 4) || '년 ' ||            \r\n";
            SQL += "          SUBSTR(TO_CHAR(B.BDATE, 'YYYYMMDD'), 5, 2) || '월 ' ||            \r\n";
            SQL += "          SUBSTR(TO_CHAR(B.BDATE, 'YYYYMMDD'), 7, 2) || '일'  AS BDATE      \r\n";
            SQL += "        , B.PANO                                              AS PANO       \r\n";
            SQL += "        , B.DEPTCODE                                          AS DEPTCODE   \r\n";
            SQL += "        , TO_CHAR(B.RECEIVEDATE,'YYYY-MM-DD')                 AS RECEIVEDATE \r\n";
            SQL += "        , TO_CHAR(A.JDATE,'YYYY-MM-DD')                       AS JDATE      \r\n";
            SQL += "        , TO_CHAR(A.RESULTDATE, 'YYYY-MM-DD')                 AS RESULTDATE \r\n";
            SQL += " 		, DECODE(B.SEX,'M','남','여')		                  AS SEX     \r\n";
            SQL += " 		, B.AGE		                                          AS AGE     \r\n";

            SQL += " 		, DECODE(NVL(TRIM(A.HASHDATA),'^%'),'^%','',KOSMOS_OCS.FC_BAS_USER(TRIM(A.RESULTSABUN)))         AS RESULTSABUN     \r\n";
            SQL += " 		, A.GBSNAME                                           AS GBSNAME    \r\n";

            //SQL += " 		, ' ▶ ' || KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(TRIM(A.MASTERCODE)) || CHR(10) || CHR(13) || CHR(10) || CHR(13) || A.RESULT1 || A.RESULT2 AS RESULT_C     \r\n";
            //2018-07-13 안정수, RESULT1과 RESULT2가 varchar2 4000이 넘을경우를 대비하여 to_clob함수를 사용
            SQL += " 		, ' ▶ ' || KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(TRIM(A.MASTERCODE)) || CHR(10) || CHR(13) || CHR(10) || CHR(13) || to_clob(A.RESULT1) || to_clob(A.RESULT2) AS RESULT_C     \r\n";
            SQL += " 		, KOSMOS_OCS.FC_EXAM_SPECMST_NM('14', B.SPECCODE, 'N') AS SPECODE \r\n";             
            SQL += " 		, HRREMARK1                                            AS HRREMARK1 \r\n";
            SQL += " 		, HRREMARK2                                            AS HRREMARK2 \r\n";
            SQL += " 		, HRREMARK3                                            AS HRREMARK3 \r\n";
            SQL += " 		, HRREMARK4                                            AS HRREMARK4 \r\n";
            SQL += " 		, HRREMARK5                                            AS HRREMARK5 \r\n";
            SQL += " 		, RESULT1                                              AS RESULT1 \r\n";
            SQL += " 		, RESULT2                                              AS RESULT2 \r\n";
            SQL += " 		, ' ▶ ' || KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(TRIM(A.MASTERCODE)) AS ANATNAME \r\n";
            

            SQL += " FROM KOSMOS_OCS.EXAM_ANATMST A                                             \r\n";
            SQL += "    , KOSMOS_OCS.EXAM_SPECMST B                                             \r\n";
            SQL += "    , KOSMOS_PMPA.BAS_PATIENT C                                             \r\n";
            SQL += "WHERE 1=1                                                                   \r\n";
            SQL += "  AND B.SPECNO = " + ComFunc.covSqlstr(strSpecNo, false);
            SQL += "  AND A.ANATNO = B.ANATNO                                                   \r\n";
            SQL += "  AND B.PANO   = C.PANO                                                     \r\n";

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
            SQL += "        SUBSTR(TO_CHAR(A.BDATE, 'YYYYMMDD'), 5, 2) || '월 ' ||                                                      \r\n";
            SQL += "        SUBSTR(TO_CHAR(A.BDATE, 'YYYYMMDD'), 7, 2) || '일'                                      AS BDATE            \r\n";


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
            SQL += " SELECT                                                                                                                      \r\n";
            SQL += " 	  A.ANATNO                                                                                           AS ANATNO           \r\n";
            SQL += " 	, A.PTNO                                                                                             AS PANO             \r\n";
            SQL += "  	, D.RECEIVEDATE																					     AS EXAM_RECEIVEDATE \r\n";
            SQL += "  	, TO_CHAR(A.JDATE     ,'YYYY-MM-DD')      														     AS JDATE            \r\n";
            SQL += "  	, TO_CHAR(A.JDATE     ,'HH24:MI')      	    													     AS JTIME            \r\n";
            SQL += "  	, TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') 															     AS RDATE            \r\n";
            SQL += "  	, TO_CHAR(A.RESULTDATE,'HH24:MI')																     AS RTIME            \r\n";
            SQL += " 	, B.GBTAT																						     AS GBTAT            \r\n";
            SQL += " 	, TO_CHAR(KOSMOS_OCS.FC_DATE_ILSU(TO_CHAR(A.RESULTDATE,'YYYY-MM-DD'),TO_CHAR(A.JDATE,'YYYY-MM-DD'))) AS ILLDATE          \r\n";
            SQL += " 	, KOSMOS_OCS.FC_DATE_ILSU(TO_CHAR(A.RESULTDATE,'YYYY-MM-DD'),TO_CHAR(A.JDATE,'YYYY-MM-DD'))- B.GBTAT AS ILLGAP           \r\n";
            SQL += "	, KOSMOS_OCS.FC_BAS_PASS_NAME(A.RESULTSABUN)													     AS RESULT_NAME      \r\n";
            SQL += "  	, DECODE(B.TONGBUN,'T',B.EXAMNAME, 'A',C.YNAME)													     AS EXAM_NAME        \r\n";
            SQL += "  	, A.GBTATSAU                                                                                         AS GBTATSAU         \r\n";
            SQL += "  	, A.GBTATBUN                                                                                         AS GBTATBUN         \r\n";
            SQL += "  	, A.GBTATBUN2                                                                                        AS GBTATBUN2        \r\n";
            SQL += "  	, A.GBTATBUN3                                                                                        AS GBTATBUN3        \r\n";
            SQL += "  	, A.RESULT1                                                                                          AS RESULT1          \r\n";
            SQL += "  	, A.RESULT2                                                                                          AS RESULT2          \r\n";
            SQL += "  	, ''                                                                                                 AS CHANGE           \r\n";
            SQL += "  	, A.ROWID                                                                                            AS ROWID_R          \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_ANATMST A                                                                                             \r\n";
            SQL += "     , KOSMOS_OCS.EXAM_MASTER  B                                                                                             \r\n";
            SQL += "     , (                                                                                                                     \r\n";
            SQL += "     	SELECT  YNAME                                                                                                        \r\n";
            SQL += "              , CODE                                                                                                         \r\n";
            SQL += "         FROM KOSMOS_OCS.EXAM_SPECODE                                                                                        \r\n";
            SQL += "        WHERE GUBUN = '14'                                                                                                   \r\n";
            SQL += "        )                      C                                                                                             \r\n";
            SQL += "     , KOSMOS_OCS.EXAM_SPECMST D                                                                                             \r\n";
            SQL += " WHERE 1=1                                                                                                                   \r\n";
            SQL += "   AND A.RESULTDATE 		>= " + ComFunc.covSqlDate(strFDate, false);
            SQL += "   AND A.RESULTDATE 		<  " + ComFunc.covSqlDate(strTDate, false);
            SQL += "   AND A.GBJOB 				= 'V'                                                                                           \r\n";
            SQL += "   AND TRIM(A.MASTERCODE) 	= B.MASTERCODE                                                                                  \r\n";
            SQL += "   AND B.GBTAT IS NOT NULL                                                                                                  \r\n";
            SQL += "   AND A.SPECNO			    = D.SPECNO                                                                                      \r\n";
            SQL += "   AND B.GBTAT 				IS NOT NULL                                                                                     \r\n";
            SQL += "   AND TRIM(A.SPECCODE)	    = C.CODE(+)                                                                                     \r\n";
            //SQL += "   AND B.TONGBUN  			= " + ComFunc.covSqlstr(enmPthlType.ToString(), false);
            if (enmPthlType == enmANATMST_TYPE.T)
            {
                //SQL += "   AND A.ANATNO 			NOT LIKE  'OS%'                                                                                 \r\n";
                SQL += "   AND B.TONGBUN  			= 'A'                                                                                           \r\n";
                SQL += "   AND A.DEPTCODE 			<> 'HR'                                                                                         \r\n";
            }
            else if (enmPthlType == enmANATMST_TYPE.A)
            {
                //SQL += "   AND A.DEPTCODE 			<> 'HR'                                                                                         \r\n";
                SQL += "   AND B.TONGBUN  			= 'T'                                                                                           \r\n";
                SQL += "   AND A.ANATNO 			NOT LIKE  'OS%'                                                                                 \r\n"; 
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

        public string up_EXAM_ANATMST(PsmhDb pDbCon, string strGBTATSAU, string strGBTATBUN,  string strGBTATBUN2, string strGBTATBUN3, string strROWID, ref int nRow)
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

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref nRow, pDbCon);
            return SqlErr;
        }

    }
}
