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
    public class clsPthlRsltSQL : Com.clsMethod
    {
        string SQL = string.Empty;

        /// <summary>Sel_EXAM_SPECMST_ANATPrint 조회결과</summary>
        public enum enmSel_EXAM_SPECMST_ANATPrint {PANO, SNAME,JUMIN1,JUMIN2,WARD_NAME,SPEC_NAME,ANATNO,DR_NAME,BDATE };

        public enum enmSel_EXAM_ANATMST_ORDER     {          PANO,          SNAME,     ANATNO,     SPECNO,  ORDERCODE,  MASTERCODE,          AGE,       SEX,       DEPTCODE,           WARD,           ROOM,      DRCODE,          DRNM,      BDATE,  BDATE_MM_DD,  MASTER_NM,     GBJOB,     SPECCODE,        CHK,    RESULT_IMG,   RESULT1,   RESULT2,   RESULTDATE,   HRREMARK1,  HRREMARK2,    HRREMARK3,   HRREMARK4,   HRREMARK5,   GBSNAME,   CREMARK1, JDATE_MM_DD,   ROWID_R,    POSCO,   FROZEN,   SLIDE_S_CD,   SLIDE_S_ETC,   SLIDE_C_CD,   SLIDE_C_ETC,   SPEC_S_CD,   SPEC_C_CD,   SPEC_C_ETC, SPEC_S_ETC };
        public string[] sSel_EXAM_ANATMST_ORDER = {    "등록번호",     "환자성명", "병리번호", "검체번호", "처방코드",  "검사코드",       "나이",    "성별",           "과",         "병동",         "병실",  "의사코드",    "의사성명", "처방일자",   "처방일자", "검사명칭",    "결과",   "검체약어",        "V",         "IMG", "RESULT1", "RESULT2", "RESULTDATE", "HRREMARK1", "HRREMARK2", "HRREMARK3", "HRREMARK4", "HRREMARK5", "GBSNAME", "CREMARK1",  "접수일자", "ROWID_R", "포스코", "FROZEN", "SLIDE_S_CD", "SLIDE_S_ETC", "SLIDE_C_CD", "SLIDE_C_ETC", "SPEC_S_CD", "SPEC_C_CD", "SPEC_C_ETC", "SPEC_S_ETC" };
        public int   [] nSel_EXAM_ANATMST_ORDER = {  nCol_PANO-10,   nCol_PANO-10,  nCol_PANO,  nCol_PANO,  nCol_PANO,   nCol_PANO, nCol_SCHK+10, nCol_SCHK, nCol_SCHK + 10, nCol_SCHK + 10, nCol_SCHK + 10,    nCol_AGE,  nCol_PANO-10,  nCol_PANO,    nCol_PANO,  nCol_JUSO,  nCol_AGE,    nCol_PANO,  nCol_SCHK,  nCol_SCHK+10,         5,         5,            5,           5,           5,           5,           5,           5,         5,          5,   nCol_PANO,       5  ,        5,        5,            5,             5,            5,             5,           5,           5,            5,           5 };

        public enum enmSel_EXAM_ANATNO_HIS     {  RESULTDATE, MASTER_NM,     ANATNO,   MASTERCODE,  RESULTDATE_HH ,  RESULT  };
        public string[] sSel_EXAM_ANATNO_HIS = {  "결과일자",  "검사명", "병리번호", "MASTERCODE", "RESULTDATE_HH", "RESULT" };
        public int[] nSel_EXAM_ANATNO_HIS    = {   nCol_DATE, nCol_JUSO,  nCol_PANO,            5,               5,        5 };

        public enum enmSel_EXAM_ANAT_FORMAT_ORGAN     {   ORGAN    };
        public string[] ssel_EXAM_ANAT_FORMAT_ORGAN = { "ORGAN"    };
        public int[] nSel_EXAM_ANAT_FORMAT_ORGAN    = { nCol_ORDERNAME };

        public enum enmSel_EXAM_ANAT_FORMAT_DISEASE       { DISEASE    };
        public string[] ssel_EXAM_ANAT_FORMAT_DISEASE   = { "DISEASE"  };
        public int[] nSel_EXAM_ANAT_FORMAT_DISEASE      = { nCol_ORDERNAME };

        public enum enmSel_EXAM_ANAT_FORMAT_DTAIL { FORMAT, SORT, ROWID };

        public enum enmSel_EXAM_SPECODE       {       CHK,      CODE,      NAME,   ROWID, CHG };
        public string[] ssel_EXAM_SPECODE   = {    "삭제",    "CODE",    "내용", "ROWID", "CHG" };
        public int[] nSel_EXAM_SPECODE      = { nCol_SCHK, nCol_DPCD, nCol_LNAME,        5, 5};


        public enum enmSel_EXAM_ANATMSTHT_RE_RESULT       {       PTNO,      SNAME,     ANATNO, RESULTDATE, RESULTDATE_HHMI, RESULTSABUN,    JOBGBN,  MASTERCODE, EXAMNAME,   RESULT,      GBJOB,       SAYU,   ROWID };
        public string[] sSel_EXAM_ANATMSTHT_RE_RESULT   = { "등록번호", "환자성명", "병리번호", "결과일자",      "결과시간",    "작업자",    "상태",  "검사코드", "검사명",   "진단",        "V", "변경사유", "ROWID" };
        public int[] nSel_EXAM_ANATMSTHT_RE_RESULT      = {  nCol_PANO, nCol_SNAME,  nCol_PANO,  nCol_DATE,     nCol_AGE+20,  nCol_SNAME, nCol_SCHK,   nCol_PANO, nCol_TEL, nCol_TEL, nCol_SCHK ,   nCol_TEL,      5  };

        public enum enmSel_EXAM_MASTER_ANANT      { MASTERCODE,  EXAMNAME,      EXAMFNAME, EXAMYNAME, WSCODE1, BCODENAME };
        public string[] sSel_EXAM_MASTER_ANANT  = { "검사코드",  "검사명", "검사명(Full)",    "구분",    "WS", "바코드"  };
        public int[] nSel_EXAM_MASTER_ANANT     = {  nCol_PANO, nCol_JUSO, nCol_JUSO + 50, nCol_PANO,       5, nCol_PANO };

        public enum enmSel_EXAM_ANATNO_PRINT      {       CHK,   ANATNO_NM,     ANATNO, RESULTDATE,     SPECNO,       PTNO,     SNAME,       SEX,          AGE,     DEPTCODE,  EXAMNAME,           WARD,   DRCODE,      DR_NM,      BDATE,   SPECCODE,    SPEC_NM,  CERTDATE, MASTERCODE };
        public string[] sSel_EXAM_ANATNO_PRINT  = {    "선택",    "분류명", "병리번호", "결과일자", "검체번호", "등록번호","환자성명",    "성별",       "나이",         "과",  "검사명",         "병동", "DRCODE",   "의사명", "처방일자", "SPECCODE",   "검체명", "인증일자", "MASTERCODE" };
        public int[] nSel_EXAM_ANATNO_PRINT     = { nCol_SCHK,  nCol_SNAME,  nCol_PANO,  nCol_DATE,  nCol_PANO,  nCol_PANO,nCol_SNAME, nCol_SCHK, nCol_SCHK+10, nCol_SCHK+10, nCol_NAME, nCol_SCHK + 10,        5, nCol_SNAME,  nCol_DATE,          5, nCol_SNAME,  nCol_DATE, 5  };  

        public enum enmSel_EXAM_ANATNO_QC     {      RDATE,     ANATNO,     SPECNO    ,       PANO,         SNAME,         JDATE,   RESULTDATE,   SPEC_NM,          SLIDE_S_CD,              SLIDE_S_ETC,          SPEC_S_CD,              SPEC_S_ETC,          SLIDE_C_CD,              SLIDE_C_ETC,          SPEC_C_CD,              SPEC_C_ETC ,  DEPTCODE,      DR_NM, RESULTSABUN };
        public string[] sSel_EXAM_ANATNO_QC = { "결과일자", "병리번호", "검체번호"    , "등록번호",        "성명",    "접수일시",   "결과일시",  "검체명", "조직 Slide 질관리", "조직 Slide 질관리 기타", "조직 검체 질관리", "조직 검체 질관리 기타", "세포 Slide 질관리", "세포 Slide 질관리 기타", "세포 검체 질관리", "세포 검체 질관리 기타" , "의뢰과", "의뢰의사", "결과입력자"};
        public int[] nSel_EXAM_ANATNO_QC    = {  nCol_PANO,  nCol_PANO,  nCol_PANO + 5,  nCol_PANO,  nCol_PANO-20,  nCol_TIME-10, nCol_TIME-10, nCol_PANO,         nCol_TIME-10,          nCol_TIME - 10,     nCol_TIME - 10,          nCol_TIME - 10,      nCol_TIME - 10,           nCol_TIME - 10,     nCol_TIME - 10,          nCol_TIME - 10 , nCol_AGE, nCol_PANO, nCol_PANO };

        public enum enmSel_EXAM_ANATMST_ORDER_OUT { ANATNO, SNAME, PANO, SPECNO, OUTGUBUN, ORDERCODE, MASTERCODE, AGE, SEX, DEPTCODE, WARD, ROOM, DRCODE, DRNM, BDATE, BDATE_MM_DD, MASTER_NM, GBJOB, SPECCODE, CHK, RESULT_IMG, RESULT1, RESULT2, RESULTDATE, HRREMARK1, HRREMARK2, HRREMARK3, HRREMARK4, HRREMARK5, GBSNAME, CREMARK1, JDATE_MM_DD, ROWID_R, POSCO, FROZEN, SLIDE_S_CD, SLIDE_S_ETC, SLIDE_C_CD, SLIDE_C_ETC, SPEC_S_CD, SPEC_C_CD, SPEC_C_ETC, SPEC_S_ETC, RESULTSABUN, };
        public string[] sSel_EXAM_ANATMST_ORDER_OUT = { "병리번호", "환자성명", "등록번호", "검체번호", "구분","처방코드", "검사코드", "나이", "성별", "과", "병동", "병실", "의사코드", "의사성명", "처방일자", "처방일자", "검사명칭", "결과", "검체약어", "V", "IMG", "RESULT1", "RESULT2", "RESULTDATE", "HRREMARK1", "HRREMARK2", "HRREMARK3", "HRREMARK4", "HRREMARK5", "GBSNAME", "CREMARK1", "접수일자", "ROWID_R", "포스코", "FROZEN", "SLIDE_S_CD", "SLIDE_S_ETC", "SLIDE_C_CD", "SLIDE_C_ETC", "SPEC_S_CD", "SPEC_C_CD", "SPEC_C_ETC", "SPEC_S_ETC" ,"작업자"};
        public int[] nSel_EXAM_ANATMST_ORDER_OUT = { nCol_PANO - 10, nCol_PANO - 10, nCol_PANO, 80, nCol_PANO - 30, nCol_PANO, nCol_PANO, nCol_SCHK + 10, nCol_SCHK, nCol_SCHK + 10, nCol_SCHK + 10, nCol_SCHK + 10, nCol_AGE, nCol_PANO - 10, nCol_PANO, nCol_PANO, 200, nCol_AGE, nCol_PANO, nCol_SCHK, nCol_SCHK + 10, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, nCol_PANO, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 , nCol_SNAME };


        public DataSet sel_EXAM_ANATNO_QC(PsmhDb pDbCon, string strFDate, string strTDate, string strGUBUN)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";

            SQL += "  SELECT  TO_CHAR(A.RESULTDATE,'YYYY-MM-DD')                                                                                    AS RDATE        \r\n";
            SQL += "        , A.ANATNO                                                                                                              AS ANATNO       \r\n";
            SQL += "        , A.SPECNO                                                                                                              AS SPECNO       \r\n";             
            SQL += "        , B.PANO                                                                                                                AS PANO         \r\n";
            SQL += "        , B.SNAME                                                                                                               AS SNAME        \r\n";
            SQL += "        , TO_CHAR(A.JDATE,'YYYY-MM-DD HH24:MI')                                                                                 AS JDATE        \r\n";
            SQL += "        , TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI')                                                                            AS RESULTDATE   \r\n";
            SQL += "        , KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',B.SPECCODE,'N')                                                                    AS SPEC_NM      \r\n";
            SQL += "  	    , DECODE(NVL(SLIDE_S_CD,'^&'),'^&','',SLIDE_S_CD || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAME('EXAM_SLIDE_QC_S',SLIDE_S_CD))  AS SLIDE_S_CD   \r\n";
            SQL += "  	    , SLIDE_S_ETC                                                                                                           AS SLIDE_S_ETC  \r\n";
            SQL += "  	    , DECODE(NVL(SPEC_S_CD,'^&'),'^&','',SPEC_S_CD || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAME('EXAM_SPEC_QC_S',SPEC_S_CD))      AS SPEC_S_CD    \r\n";
            SQL += "  	    , SPEC_S_ETC                                                                                                            AS SPEC_S_ETC   \r\n";
            SQL += "  	    , DECODE(NVL(SLIDE_C_CD,'^&'),'^&','',SLIDE_C_CD || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAME('EXAM_SLIDE_QC_C',SLIDE_C_CD))  AS SLIDE_C_CD   \r\n";
            SQL += "  	    , SLIDE_C_ETC                                                                                                           AS SLIDE_C_ETC  \r\n";
            SQL += "  	    , DECODE(NVL(SPEC_C_CD,'^&'),'^&','',SPEC_C_CD || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAME('EXAM_SPEC_QC_C',SPEC_C_CD))      AS SPEC_C_CD    \r\n";
            SQL += "  	    , SPEC_C_ETC                                                                                                            AS SPEC_C_ETC   \r\n";
            //SQL += "        , A.RESULT1 || A.RESULT2                                                                                                AS RESULT       \r\n";

            SQL += "  	    ,  A.DEPTCODE                                                                                                           AS DEPTCODE     \r\n";
            SQL += "  	    ,  KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE)                                                                            AS DR_NM        \r\n";
            SQL += "  	    ,  KOSMOS_OCS.FC_BAS_USER_USERNAME2(TRIM(A.RESULTSABUN))                                                                AS RESULTSABUN  \r\n";

            SQL += "     FROM  KOSMOS_OCS.EXAM_ANATMST A                                                                                                            \r\n";    
            SQL += "         , KOSMOS_OCS.EXAM_SPECMST B                                                                                                            \r\n";
            SQL += "     WHERE 1=1                                                                                                                                  \r\n";
            SQL += "       AND A.SPECNO = B.SPECNO                                                                                                                  \r\n";
            SQL += "  	 AND A.BDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "  	                 AND " + ComFunc.covSqlDate(strTDate, false);

            if (strGUBUN.Equals("0") == true)
            {
                SQL += "       AND A.FROZEN = '1'                                                                                                                   \r\n";
            }
            else if (strGUBUN.Equals("1") == true)
            {
                SQL += "       AND A.SLIDE_C_CD IS NOT NULL                                                                                                             \r\n";
            }
            else if (strGUBUN.Equals("2") == true)
            {
                SQL += "       AND A.SPEC_C_CD IS NOT NULL                                                                                                             \r\n";
            }
            else if (strGUBUN.Equals("3") == true)
            {
                SQL += "       AND A.SLIDE_S_CD IS NOT NULL                                                                                                             \r\n";
            }
            else if (strGUBUN.Equals("4") == true)
            {
                SQL += "       AND A.SPEC_S_CD IS NOT NULL                                                                                                             \r\n";
            }


            SQL += "       ORDER BY A.ANATNO                                                                                                                        \r\n";

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

        public DataSet sel_EXAM_ANATNO_PRINT(PsmhDb pDbCon, string strFDate, string strTDate, string strAnat, bool isRePrint)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";

            SQL += "  SELECT                                                                                            \r\n";
            SQL += "  	  	  '' 																AS CHK			--01    \r\n";
            SQL += "  		  , CASE WHEN SUBSTR(A.ANATNO,1,2) ='PU' THEN '신검(소변)'                                    \r\n";
            SQL += "  		         WHEN SUBSTR(A.ANATNO,1,2) ='PS' THEN '신검(객담)'                                    \r\n";
            SQL += "  		  	   WHEN SUBSTR(A.ANATNO,1,2) ='OS' THEN '조직(외)'                                      \r\n";
            SQL += "  		  	   ELSE                                                                                 \r\n";
            SQL += "  		  	   		CASE WHEN SUBSTR(A.ANATNO,1,1) ='S' THEN '조직'                                 \r\n";
            SQL += "  		  	   		     WHEN SUBSTR(A.ANATNO,1,1) ='P' THEN '신검(부인과)'                         \r\n";
            SQL += "                             WHEN SUBSTR(A.ANATNO,1,1) ='C' THEN '세포' END                           \r\n";
            SQL += "  		  	   END															AS ANATNO_NM	--02    \r\n";
            SQL += "  		  , A.ANATNO														AS ANATNO		--03    \r\n";
            SQL += "   		  , TO_CHAR(A.RESULTDATE, 'YYYY-MM-DD') 							AS RESULTDATE	--04    \r\n";
            SQL += "   		  , A.SPECNO														AS SPECNO		--05    \r\n";
            SQL += "   		  , A.PTNO															AS PTNO 		--06    \r\n";
            SQL += "   		  , B.SNAME															AS SNAME		--07    \r\n";
            SQL += "   		  , B.SEX															AS SEX          --08    \r\n";
            SQL += "   		  , B.AGE                                                           AS AGE          --08    \r\n";
            SQL += "   		  , A.DEPTCODE														AS DEPTCODE		--09    \r\n";
            SQL += "          , C.EXAMNAME                                                      AS EXAMNAME		--10    \r\n";
            SQL += "          , B.WARD															AS WARD			--11    \r\n";
            SQL += "          , A.DRCODE														AS DRCODE		--12    \r\n";
            SQL += "          , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE)						AS DR_NM		--12    \r\n";
            SQL += "   		  , TO_CHAR(A.BDATE, 'YYYY-MM-DD') 									AS BDATE		--13    \r\n";
            SQL += "   		  , A.SPECCODE														AS SPECCODE     --14    \r\n";
            SQL += "          , KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',TRIM(A.SPECCODE),'Y')   		AS SPEC_NM		--14    \r\n";
            SQL += "          , A.CERTDATE													    AS CERTDATE		--15    \r\n";
            SQL += "          , A.MASTERCODE													AS MASTERCODE		--15    \r\n";
            SQL += "     FROM  KOSMOS_OCS.EXAM_ANATMST A                                                                \r\n";
            SQL += "         , KOSMOS_OCS.EXAM_SPECMST B                                                                \r\n";
            SQL += "         , KOSMOS_OCS.EXAM_MASTER C                                                                 \r\n";
            SQL += "    WHERE 1=1                                                                                       \r\n";
            SQL += "      AND A.RESULTDATE >= TO_DATE('" + strFDate + "' ,'YYYY-MM-DD')                                 \r\n";
            SQL += "      AND A.RESULTDATE <  TO_DATE('" + strTDate + "' ,'YYYY-MM-DD') + 1                                 \r\n";
            SQL += "      AND A.GBJOB IN ('V')                                                                          \r\n";
            SQL += "      AND A.SPECNO = B.SPECNO                                                                       \r\n";
            SQL += "      AND TRIM(A.MASTERCODE) = C.MASTERCODE(+)                                                      \r\n";

            ////2018-08-08 안정수, 접수대상에서 Clinical hisory Information 항목에서 Thyroid가 있을경우... 추가함
            //if (isAC == true)
            //{
            //    SQL += "     AND A.REMARK2 IS NOT NULL                                                                          \r\n";
            //    SQL += "     AND UPPER(A.REMARK2) Like 'THYR%'                                                                  \r\n"; 
            //}

            if (isRePrint == true)
            {
                SQL += "      AND A.GBPRINT IS NOT NULL                                                                 \r\n";
            }
            else
            {
                SQL += "      AND A.GBPRINT IS NULL                                                                     \r\n";
            }

            if (strAnat.Equals("1") == true)
            {
                SQL += "     AND  ( A.AnatNo  LIKE 'S%' and SUBSTR(A.AnatNo,1,2) <>'SW')                        \r\n";
                //2018-11-20 안정수, 외부의뢰가 NULL인경우 추가
                SQL += "     AND A.GBOUT IS NULL                                                                \r\n";
            }
            else if (strAnat.Equals("2") == true)
            {
                SQL += "     AND A.AnatNo  LIKE 'C%'                                                            \r\n";
            }
            else if (strAnat.Equals("3") == true)
            {
                SQL += "     AND SUBSTR(A.AnatNo,1,1) ='P'and SUBSTR(A.AnatNo,1,2) <>'PS' and SUBSTR(A.AnatNo,1,2) <>'PU'   \r\n";
            }
            else if (strAnat.Equals("4") == true)
            {
                SQL += "     AND SUBSTR(A.AnatNo,1,2) ='PS'   \r\n";
            }
            else if (strAnat.Equals("5") == true)
            {
                SQL += "     AND SUBSTR(A.AnatNo,1,2) ='PU'   \r\n";
            }
            else if (strAnat.Equals("6") == true)
            {
                //2018-08-13 안정수, 기존 SW에서 AC로 변경
                //SQL += "     AND SUBSTR(A.AnatNo,1,2) ='SW'   \r\n";
                SQL += "     AND SUBSTR(A.AnatNo,1,2) ='AC'   \r\n";
            }
            else if (strAnat.Equals("7") == true)
            {
                //SQL += "     AND SUBSTR(A.AnatNo,1,2) ='OS' \r\n";
                //2018-11-20 안정수, 조직외부 OS -> S로 변경하면서 쿼리수정함
                SQL += "     AND (SUBSTR(A.AnatNo,1,2) ='OS' OR ( A.AnatNo  LIKE 'S%' and SUBSTR(A.AnatNo,1,2) <>'SW'))  \r\n";
                SQL += "     AND A.GBOUT = '*'                                                                           \r\n";
            }
            else if (strAnat.Equals("8") == true)
            {
                SQL += "     AND SUBSTR(A.AnatNo,1,2) ='IH'  \r\n";
            }


            SQL += "   ORDER BY A.ANATNO, A.SPECNO                                                          \r\n";

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

        public string up_EXAM_ANATMST_PRINT(PsmhDb pDbCon, string strSPECNO, string strMASTERCODE, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";

            SQL += " UPDATE KOSMOS_OCS.EXAM_ANATMST     \r\n";
            SQL += "    SET OUTDATE  = SYSDATE          \r\n";
            SQL += "      , OUTSABUN = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += "      , GBPRINT  =  ( CASE WHEN (TO_NUMBER(NVL(GBPRINT,'0')) + 1) > 9 THEN '9' \r\n";
            SQL += "                           ELSE TO_CHAR(TO_NUMBER(NVL(GBPRINT,'0')) + 1) END )  \r\n";
            SQL += "   WHERE SPECNO     = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "     AND MASTERCODE = " + ComFunc.covSqlstr(strMASTERCODE, false);

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

        public DataSet sel_EXAM_MASTER_ANANT(PsmhDb pDbCon)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT  MASTERCODE, EXAMNAME, EXAMFNAME, EXAMYNAME, WSCODE1, BCODENAME                                     \r\n";            
            SQL += "   FROM KOSMOS_OCS.EXAM_MASTER A                                                                            \r\n";
            SQL += "  WHERE ( ( WSCODE1  =  '711' )   OR  (WSCODE1 ='801' AND MASTERCODE LIKE 'O-%')  OR MASTERCODE LIKE 'Y%' ) \r\n";
            SQL += "  ORDER BY  WSCODE1  , MASTERCODE                                                                           \r\n";

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

        public DataSet sel_EXAM_ANATMSTHT_RE_RESULT(PsmhDb pDbCon, string strFDate, string strTDate, string strSPECNO, string strANATNO, string strWS)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                                                                          \r\n";
            SQL += "		 	A.PTNO                                                                               \r\n";
            SQL += "		, 	B.SNAME                                                                              \r\n";
            SQL += "		,	A.ANATNO                                                                             \r\n";
            SQL += "		,  	CASE WHEN JOBGBN = '1' THEN TO_CHAR(A.RESULTDATE,'YYYY-MM-DD')                       \r\n";
            SQL += "		         WHEN JOBGBN = '2' THEN TO_CHAR(A.JOBDATE,'YYYY-MM-DD') END  AS RESULTDATE       \r\n";
            SQL += "		,  	CASE WHEN JOBGBN = '1' THEN TO_CHAR(A.RESULTDATE,'HH24:MI')                          \r\n";
            SQL += "		         WHEN JOBGBN = '2' THEN TO_CHAR(A.JOBDATE,'HH24:MI')    END	 AS RESULTDATE_HHMI  \r\n";
            SQL += "		, 	KOSMOS_OCS.FC_BAS_USER_USERNAME(A.RESULTSABUN)                   AS RESULTSABUN      \r\n";
            SQL += "		,   CASE WHEN JOBGBN = '1' THEN '전' WHEN JOBGBN = '2' THEN '후' END AS JOBGBN           \r\n";
            SQL += "		, 	A.MASTERCODE                                                                         \r\n";
            SQL += "		, 	C.EXAMNAME                                                                           \r\n";
            SQL += "		,   A.RESULT1 || ' ' || A.RESULT2									AS RESULT            \r\n";
            SQL += "		,   A.GBJOB                                                                              \r\n";
            //SQL += "		,   KOSMOS_OCS.FC_EXAM_SPECMST_NM('23',A.SAYU,'N') || A.SAYU_ETC	AS SAYU              \r\n";            
            //SQL += "		,   KOSMOS_OCS.FC_EXAM_SPECMST_NM('23',A.SAYU,'N')                  AS SAYU              \r\n";
            //2019-12-09 안정수 조건 수정
            SQL += "		,   CASE WHEN KOSMOS_OCS.FC_EXAM_SPECMST_NM('23',A.SAYU,'N') <> '기타' THEN KOSMOS_OCS.FC_EXAM_SPECMST_NM('23',A.SAYU,'N')  \r\n";
            SQL += "		         WHEN KOSMOS_OCS.FC_EXAM_SPECMST_NM('23',A.SAYU,'N') = '기타'  THEN A.SAYU_ETC END AS SAYU                          \r\n";
            SQL += "		, 	A.ROWID                                                                              \r\n";
            SQL += "FROM  KOSMOS_OCS.EXAM_ANATMSTH A                                                                 \r\n";
            SQL += "	, KOSMOS_PMPA.BAS_PATIENT  B                                                                 \r\n";
            SQL += "	, KOSMOS_OCS.EXAM_MASTER   C                                                                 \r\n";
            SQL += "WHERE 1=1                                                                                        \r\n";

            if (string.IsNullOrEmpty(strSPECNO.Trim()) == false)
            {
                SQL += "  AND A.SPECNO =  " + ComFunc.covSqlstr(strSPECNO, false);
            }
            else if (string.IsNullOrEmpty(strANATNO.Trim()) == false)
            {
                SQL += "  AND A.ANATNO =  " + ComFunc.covSqlstr(strANATNO, false);
            }
            else
            {
                SQL += "  AND A.JOBDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                                 \r\n";
                SQL += "  AND A.JOBDATE <=  TO_DATE('" + strTDate + " 23:59','YYYY-MM-DD HH24:MI')                  \r\n";
            }

            //SQL += "  AND A.JOBGBN IN ('1','2')                                                                     \r\n";
            SQL += "  AND A.JOBGBN IN ('2')                                                                         \r\n";
            SQL += "  AND A.PTNO = B.PANO                                                                           \r\n";
            SQL += "  AND A.MASTERCODE = C.MASTERCODE                                                               \r\n";

            if (strWS.Trim().Equals("S*") == true)
            {
                SQL += "  AND A.ANATNO LIKE 'S%'                                                               \r\n";
            }
            else if (strWS.Trim().Equals("C*") == true)
            {
                SQL += "  AND A.ANATNO LIKE 'C%'                                                               \r\n"; 
            }

            SQL += "ORDER BY A.JOBDATE,A.SPECNO,A.JOBSABUN,A.JOBGBN, B.SNAME, C.EXAMNAME                            \r\n";

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

        public string up_EXAM_ANATMST_CERT(PsmhDb pDbCon, string FstrROWID, string GstrHash, string GstrCert, ref int intRowAffected)
        {
            string SqlErr = ""; 

            SQL = "";

            SQL += " UPDATE KOSMOS_OCS.EXAM_ANATMST SET CERTDATE = SYSDATE, HASHDATA  = '" + GstrHash + "' "; 
            SQL += ",     CERTDATA = '" + GstrCert + "' ";
            //2018-10-10 안정수, C#으로 전자인증했을때 구분을 위한 플래그 추가
            SQL += ",     CERTNEW = 'Y' ";
            SQL += "   WHERE ROWID = '" + FstrROWID + "' ";

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

        public string up_EXAM_SPECNO(PsmhDb pDbCon, string sResultDate, string sGbJob, string sSpecNo, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";

            SQL += " UPDATE KOSMOS_OCS.EXAM_SPECMST                                           \r\n";

            if (sGbJob == "V")
            {
                SQL += "    SET STATUS      = '05'  \r\n";
                SQL += "      , RESULTDATE  = TO_DATE('" + sResultDate + "','YYYY-MM-DD HH24:MI')  \r\n";
            }
            else if (sGbJob == "Y")
            {
                SQL += "    SET RESULTDATE  = ''    \r\n";
                SQL += "      , STATUS = '01'       \r\n";

            }

            SQL += "     WHERE SPECNO        = '" + sSpecNo + "'";

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

        public string up_EXAM_RESULTC(PsmhDb pDbCon, string sResultDate, string sGbJob, string sSpecNo, string sMasterCode, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";

            SQL += " UPDATE KOSMOS_OCS.EXAM_RESULTC                                           \r\n";

            if (sGbJob == "V")
            {
                SQL += "    SET STATUS      = '" + sGbJob + "'                                    \r\n";
                SQL += "      , RESULTDATE  = TO_DATE('" + sResultDate + "','YYYY-MM-DD HH24:MI')  \r\n";
                SQL += "      , RESULTSABUN = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            }
            else if (sGbJob == "Y")
            {
                SQL += "    SET RESULTDATE  = ''    \r\n";
                SQL += "      , RESULTSABUN = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);

            }

            SQL += "     WHERE SPECNO        = '" + sSpecNo + "'";
            SQL += "       AND MASTERCODE    = '" + sMasterCode + "'";

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

        public string del_EXAM_ANATMST(PsmhDb pDbCon, string FstrROWID, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";

            SQL += " UPDATE KOSMOS_OCS.EXAM_ANATMST     \r\n";
            SQL += "    SET GBJOB       = 'N'           \r\n";
            SQL += "      , RESULTDATE  = ''            \r\n";
            SQL += "      , HRREMARK1     = ''          \r\n";
            SQL += "      , HRREMARK2     = ''          \r\n";
            SQL += "      , HRREMARK3     = ''          \r\n";
            SQL += "      , HRREMARK4     = ''          \r\n";
            SQL += "      , HRREMARK5     = ''          \r\n";
            SQL += "      , Result1       = ''          \r\n";
            SQL += "      , Result2       = ''          \r\n";
            SQL += "      , CVR_ENTDATE   = ''          \r\n";
            SQL += "      , CVR_ENTSABUN  = ''          \r\n";
            SQL += "      , CREMARK1      = ''          \r\n";
            SQL += "      , RESULTSABUN   = ''          \r\n";
            SQL += "      , GBSNAME       = ''          \r\n";
            SQL += " WHERE ROWID          = '" + FstrROWID + "'";

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

        public string up_EXAM_ANATMST(PsmhDb pDbCon , string sResultDate, string sGbJob, string sAnatNo
                                                    , string sResultRemark1, string sResultRemark2, string sResultRemark3, string sResultRemark4, string sResultRemark5
                                                    , bool isOptDiagnosis, string strCREMARK1, string FstrROWID, string TxtGbSName
                                                    , string GUBUN_S
                                                    , string FROZEN
                                                    , string SLIDE_S_CD
                                                    , string SLIDE_S_ETC
                                                    , string SLIDE_C_CD
                                                    , string SLIDE_C_ETC
                                                    , string SPEC_S_CD
                                                    , string SPEC_C_CD
                                                    , string SPEC_C_ETC
                                                    , string SPEC_S_ETC
                                                    , ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";

            SQL += " UPDATE KOSMOS_OCS.EXAM_ANATMST                                           \r\n";
            SQL += "    SET GBJOB       = '" + sGbJob + "'                                    \r\n";
            SQL += "      , RESULTDATE  = TO_DATE('" + sResultDate + "','YYYY-MM-DD HH24:MI')\r\n";

            if (sAnatNo.Substring(0, 1) == "P")
            {
                SQL += "      , HRREMARK1     = '" + sResultRemark1.Replace("'","`") + "'  \r\n";
                SQL += "      , HRREMARK2     = '" + sResultRemark2.Replace("'", "`") + "'  \r\n";
                SQL += "      , HRREMARK3     = '" + sResultRemark3.Replace("'", "`") + "'  \r\n";
                SQL += "      , HRREMARK4     = '" + sResultRemark4.Replace("'", "`") + "'  \r\n";
                SQL += "      , HRREMARK5     = '" + sResultRemark5.Replace("'", "`") + "'  \r\n";
            }
            else
            {
                SQL += "      , Result1       = '" + sResultRemark1.Replace("'", "`") + "'  \r\n";
                SQL += "      , Result2       = '" + sResultRemark2.Replace("'", "`") + "'  \r\n";
            }

            if (isOptDiagnosis == false)
            {
                SQL += "      , CVR_ENTDATE   = SYSDATE    \r\n";
                SQL += "      , CVR_ENTSABUN  = '" + clsType.User.IdNumber + "'    \r\n";
                SQL += "      , CREMARK1      = '" + strCREMARK1.Replace("'", "`") + "'    \r\n";
            }
            else
            {
                SQL += "      , CVR_ENTDATE   = ''    \r\n";
                SQL += "      , CVR_ENTSABUN  = '0'    \r\n";
                SQL += "      , CREMARK1      = ''     \r\n";
            }


            SQL += "         , RESULTSABUN   = '" + clsType.User.IdNumber + "'  \r\n";
            SQL += "         , GBSNAME       = '" + TxtGbSName.Replace("'", "`") + "'   \r\n";

            if (GUBUN_S.Equals("S") == true)
            {
                SQL += "         , FROZEN       = '" + FROZEN + "' \r\n";
                SQL += "         , SLIDE_S_CD   = '" + SLIDE_S_CD + "' \r\n";
                SQL += "         , SLIDE_S_ETC  = '" + SLIDE_S_ETC + "' \r\n";
                SQL += "         , SPEC_S_CD    = '" + SPEC_S_CD + "' \r\n";
                SQL += "         , SPEC_S_ETC   = '" + SPEC_S_ETC + "' \r\n";
            }
            else if (GUBUN_S.Equals("C") == true || GUBUN_S.Equals("A") == true)
            {
                SQL += "         , SLIDE_C_CD   = '" + SLIDE_C_CD + "' \r\n";
                SQL += "         , SLIDE_C_ETC  = '" + SLIDE_C_ETC + "' \r\n";
                SQL += "         , SPEC_C_CD    = '" + SPEC_C_CD + "' \r\n";
                SQL += "         , SPEC_C_ETC   = '" + SPEC_C_ETC + "' \r\n";
            }




            SQL += "     WHERE ROWID        = '" + FstrROWID + "'";

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

        public string ins_EXAM_ANATMSTH(PsmhDb pDbCon, string ArgJobGbn, string ArgROWID, string ArgSayu, string ArgSayu_etc, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " INSERT INTO KOSMOS_OCS.EXAM_ANATMSTH ( JOBDATE,JOBGBN,JOBSABUN,SAYU, SAYU_ETC, PTNO,BDate,ORDERCODE,OrderNo,GBIO,REMARK1,REMARK2,REMARK3,  \r\n";
            SQL += "  GBJOB,RESULTDATE,RESULT1,RESULT2,DeptCode,DrCode,SPECNO,ANATNO,MASTERCODE,RESULTSABUN,SPECCODE,GBPRINT,JDATE,OUTDATE,OUTSABUN, \r\n";
            SQL += "  REMARK4,TALCHK,REMARK5,HRREMARK1,HRREMARK2,HRREMARK3,HRREMARK4,HRREMARK5,GBTO,GBSNAME,GBTATBUN,GBTATSAU,GBTATBUN2,GBTATBUN3,\r\n";
            SQL += "  HICNO,CERTDATE,HASHDATA,CERTDATA,SENDEMR,SENDEMRNO,JSABUN )\r\n";
            SQL += "  SELECT  SYSDATE , '" + ArgJobGbn + "', '" + clsType.User.IdNumber + "' , '" + ArgSayu + "'  , '" + ArgSayu_etc + "', PTNO, BDate, ORDERCODE, OrderNo, GBIO, REMARK1, REMARK2, REMARK3, \r\n";
            SQL += "  GBJOB,RESULTDATE,RESULT1,RESULT2,DeptCode,DrCode,SPECNO,ANATNO,RTRIM(MASTERCODE),RESULTSABUN,SPECCODE,GBPRINT,JDATE,OUTDATE,OUTSABUN, \r\n";
            SQL += "  REMARK4,TALCHK,REMARK5,HRREMARK1,HRREMARK2,HRREMARK3,HRREMARK4,HRREMARK5,GBTO,GBSNAME,GBTATBUN,GBTATSAU,GBTATBUN2,GBTATBUN3, \r\n";
            SQL += "  HICNO,CERTDATE,HASHDATA,CERTDATA,SENDEMR,SENDEMRNO,JSABUN  \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_ANATMST \r\n";
            SQL += "   WHERE ROWID = '" + ArgROWID + "' ";

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


        public string del_EXAM_SPECODE(PsmhDb pDbCon, string strGUBUN, string strCODE, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            
            SQL += " DELETE KOSMOS_OCS.EXAM_SPECODE  \r\n";
            SQL += "   WHERE GUBUN  = '" + strGUBUN + "' ";
            SQL += "     AND CODE   = '" + strCODE + "' ";

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

        public string ins_EXAM_SPECODE(PsmhDb pDbCon, string strGUBUN, string strCODE, string strNAME, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";

            SQL += " INSERT INTO KOSMOS_OCS.EXAM_SPECODE (CODE,GUBUN,NAME) VALUES ( \r\n";
            SQL += "     " + ComFunc.covSqlstr(strCODE, false);
            SQL += "     " + ComFunc.covSqlstr(strGUBUN, true);
            SQL += "     " + ComFunc.covSqlstr(strNAME, true);
            SQL += "     )  \r\n";

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

        public DataSet sel_EXAM_SPECODE(PsmhDb pDbCon, string strGUBUN)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += "  SELECT                            \r\n";
            SQL += "         '' CHK, CODE, NAME, ROWID, '' AS CHG        \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_SPECODE A  \r\n";
            SQL += "   WHERE 1=1                        \r\n";
            SQL += "     AND GUBUN = " + ComFunc.covSqlstr(strGUBUN, false);

            if (strGUBUN.Equals("18") == true)
            {                
                SQL += "     AND CODE LIKE 'YH%'         \r\n";
            }
            else if (strGUBUN.Equals("19") == true)
            {
                SQL += "     AND CODE LIKE 'YF%'         \r\n";
            }

            if (strGUBUN.Equals("23") == true)
            {
                SQL += "   ORDER BY CODE,SORT                       \r\n";
            }
            else if (strGUBUN.Equals("19") == true)
            {
                SQL += "   ORDER BY CODE                       \r\n";
            }
            else
            {
                SQL += "   ORDER BY SORT                       \r\n";
            }

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

        public string ins_EXAM_ANAT_FORMAT(PsmhDb pDbCon, string strORGAN, string strDISEASE, string strFORMAT, string strSORT, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " INSERT INTO KOSMOS_OCS.EXAM_ANAT_FORMAT (ORGAN, DISEASE, FORMAT, SORT)  VALUES ( \r\n";
            SQL += "     " + ComFunc.covSqlstr(strORGAN, false);
            SQL += "     " + ComFunc.covSqlstr(strDISEASE, true);
            SQL += "     " + ComFunc.covSqlstr(strFORMAT, true);
            SQL += "     " + ComFunc.covSqlstr(strSORT, true);
            SQL += "     )\r\n";

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

        public string del_EXAM_ANAT_FORMAT(PsmhDb pDbCon, string strORGAN, string strDISEASE, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " DELETE KOSMOS_OCS.EXAM_ANAT_FORMAT                         \r\n";
            SQL += "   WHERE 1 = 1                                              \r\n";
            SQL += "     AND ORGAN   = " + ComFunc.covSqlstr(strORGAN, false);
            SQL += "     AND DISEASE = " + ComFunc.covSqlstr(strDISEASE, false);

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

        public DataTable sel_EXAM_ANAT_FORMAT_DTAIL(PsmhDb pDbCon, string strORGAN, string strDISEASE)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += "  SELECT                                \r\n";
            SQL += "         FORMAT, SORT, ROWID           \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_ANAT_FORMAT A  \r\n";
            SQL += "   WHERE ORGAN   = " + ComFunc.covSqlstr(strORGAN, false);
            SQL += "     AND DISEASE = " + ComFunc.covSqlstr(strDISEASE, false);
            SQL += "   ORDER BY SORT                       \r\n";

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

        public DataSet sel_EXAM_ANAT_FORMAT_DISEASE(PsmhDb pDbCon, string strORGAN)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += "  SELECT                                \r\n";
            SQL += "          DISEASE                       \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_ANAT_FORMAT A  \r\n";
            SQL += "   WHERE ORGAN = " + ComFunc.covSqlstr(strORGAN, false);
            SQL += "   GROUP BY DISEASE                       \r\n";
            SQL += "   UNION                                \r\n";
            SQL += "  SELECT ''                             \r\n";
            SQL += "    FROM DUAL                           \r\n";
            SQL += "   ORDER BY DISEASE                       \r\n";

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

        public DataSet sel_EXAM_ANAT_FORMAT_ORGAN(PsmhDb pDbCon)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += "  SELECT                                \r\n";
            SQL += "          ORGAN                         \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_ANAT_FORMAT A  \r\n";
            SQL += "   GROUP BY ORGAN                       \r\n";
            SQL += "   UNION                                \r\n";
            SQL += "  SELECT ''                          \r\n";
            SQL += "    FROM DUAL                           \r\n";
            SQL += "   ORDER BY ORGAN                       \r\n";

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

        public DataSet sel_EXAM_ANATNO_HIS(PsmhDb pDbCon, string strPANO, string strANATNO)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += "  SELECT                                                                                 \r\n";
            SQL += "          TO_CHAR(A.RESULTDATE, 'YYYY-MM-DD')                       AS RESULTDATE        \r\n";
            SQL += "        , KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(TRIM(A.MASTERCODE))    AS MASTER_NM        \r\n";
            SQL += "        , A.ANATNO                                                  AS ANATNO           \r\n";
            SQL += "        , A.MASTERCODE                                              AS MASTERCODE       \r\n";
            SQL += "        , TO_CHAR(RESULTDATE,'YYYY-MM-DD HH24:MI')                  AS RESULTDATE_HH    \r\n";
            SQL += "        , A.RESULT1 || A.RESULT2                                    AS RESULT          \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_ANATMST A                                                      \r\n";
            SQL += "   WHERE 1=1                                                                            \r\n";
            SQL += "     AND A.PTNO                 = " + ComFunc.covSqlstr(strPANO, false);
            SQL += "     AND SUBSTR(A.ANATNO, 1, 1) = " + ComFunc.covSqlstr(strANATNO, false);
            SQL += "     AND A.GBJOB                = 'V'                                                   \r\n";
            SQL += "   ORDER BY A.RESULTDATE DESC                                                           \r\n";

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

        
         
        public DataSet sel_EXAM_ANATNO_ORDER(PsmhDb pDbCon, string strFDate, string strTDate, string strSel, string strAnat, string strSearch, string strSearch2, bool isAC)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += "  SELECT                                                                                \r\n";
            SQL += "          A.ANATNO                                                                                                             AS ANATNO       -- 01   \r\n";
            SQL += "        , S.SNAME                                                                                                           AS SNAME      -- 02   \r\n";
            SQL += "        , S.PANO                                                                                                          AS PANO     -- 04   \r\n";
            SQL += "        , A.SPECNO                                                                                                          AS SPECNO     -- 03   \r\n";
            //2018-11-22 안정수, 추가 
            SQL += "        , CASE WHEN A.GBOUT IS NULL     THEN ''                                                                                                   \r\n";
            SQL += "               WHEN A.GBOUT IS NOT NULL THEN '외부'                                                                                               \r\n";
            SQL += "          END                                                                                                               AS GBOUT      -- 03   \r\n";
            //
            SQL += "        , A.ORDERCODE                                                                                                       AS ORDERCODE  -- 05   \r\n";
            SQL += "        , A.MASTERCODE                                                                                                      AS MASTERCODE -- 06   \r\n";
            SQL += "        , S.AGE                                                                                                             AS AGE        -- 02   \r\n";
            SQL += "        , S.SEX                                                                                                             AS SEX        -- 02   \r\n";
            SQL += "        , TRIM(A.DEPTCODE)                                                                                                  AS DEPTCODE   -- 07   \r\n";
            SQL += "        , S.WARD                                                                                                            AS WARD       -- 07   \r\n";
            SQL += "        , S.ROOM                                                                                                            AS ROOM       -- 07   \r\n";
            SQL += "        , A.DRCODE                                                                                                          AS DRCODE     -- 08   \r\n";
            SQL += "        , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE)                                                                         AS DRNM       -- 08   \r\n";
            SQL += "        , TO_CHAR(A.BDATE, 'YYYY-MM-DD')					 	                                                            AS BDATE      -- 09   \r\n";

            SQL += "        , TO_CHAR(A.BDATE,'MM/DD') 								                                                            AS BDATE_MM_DD-- 10   \r\n";

            SQL += "        , KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(TRIM(A.MASTERCODE))                                                            AS MASTER_NM  -- 11   \r\n";
            SQL += "        , A.GBJOB                                                                                                           AS GBJOB      -- 11   \r\n";
            SQL += "        , A.SPECCODE                                                                                                        AS SPECCODE   -- 11   \r\n";
            SQL += "        , ''                                                                                                                AS CHK        -- 11   \r\n";
            SQL += "        , A.RESULT_IMG                                                                                                      AS RESULT_IMG  -- 11  \r\n";

            SQL += "        , A.RESULT1                                                                                                         AS RESULT1  -- 11     \r\n";
            SQL += "        , A.RESULT2                                                                                                         AS RESULT2  -- 11     \r\n";
            SQL += "        , TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI')                                                                        AS RESULTDATE         \r\n";
            SQL += "        , A.HRREMARK1                                                                                                       AS HRREMARK1  -- 11   \r\n";
            SQL += "        , A.HRREMARK2                                                                                                       AS HRREMARK2  -- 11   \r\n";
            SQL += "        , A.HRREMARK3                                                                                                       AS HRREMARK3  -- 11   \r\n";
            SQL += "        , A.HRREMARK4                                                                                                       AS HRREMARK4  -- 11   \r\n";
            SQL += "        , A.HRREMARK5                                                                                                       AS HRREMARK5  -- 11   \r\n";

            SQL += "        , A.GBSNAME                                                                                                         AS GBSNAME  -- 11     \r\n";
            SQL += "        , A.CREMARK1                                                                                                        AS GBSNAME  -- 11     \r\n";

            SQL += "  	  ,	TO_CHAR(A.JDATE,'MM/DD')								                                                            AS JDATE_MM_DD-- 12   \r\n";
            SQL += "  	  , A.ROWID													                                                            AS ROWID_R    -- 13   \r\n";
            SQL += "  	  , (                                                                                                                                         \r\n";
            SQL += "  	     SELECT CASE WHEN COUNT(*) > 0 THEN 'POSCO'                                                                                               \r\n";
            SQL += "  	                 ELSE '' END                                                                                                                  \r\n";
            SQL += "             FROM KOSMOS_PMPA.BAS_PATIENT_POSCO                                                                                                   \r\n";
            SQL += "            WHERE PANO = S.PANO                                                                                                                   \r\n";
            SQL += "  		    AND (                                                                                                                                 \r\n";
            SQL += "  			  		TRUNC(EXAMRES2) = TRUNC(S.BDATE)                                                                                              \r\n";
            SQL += "  			  	OR  TRUNC(EXAMRES3) = TRUNC(S.BDATE)                                                                                              \r\n";
            SQL += "  			  	OR  TRUNC(EXAMRES4) = TRUNC(S.BDATE)                                                                                              \r\n";
            SQL += "  				)                                                                                                                                 \r\n";
            SQL += "  	    )   													                                                            AS POSCO 	--14      \r\n";
                                                                                                                                                
            SQL += "  	  , FROZEN													                                                            AS FROZEN 	    \r\n";
            SQL += "  	  , DECODE(NVL(SLIDE_S_CD,'^&'),'^&','',SLIDE_S_CD || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAME('EXAM_SLIDE_QC_S',SLIDE_S_CD))AS SLIDE_S_CD   \r\n";
            SQL += "  	  , SLIDE_S_ETC                                                                                                         AS SLIDE_S_ETC  \r\n";
            SQL += "  	  , DECODE(NVL(SLIDE_C_CD,'^&'),'^&','',SLIDE_C_CD || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAME('EXAM_SLIDE_QC_C',SLIDE_C_CD))AS SLIDE_C_CD   \r\n";
            SQL += "  	  , SLIDE_C_ETC                                                                                                         AS SLIDE_C_ETC  \r\n";
            SQL += "  	  , DECODE(NVL(SPEC_S_CD,'^&'),'^&','',SPEC_S_CD || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAME('EXAM_SPEC_QC_S',SPEC_S_CD))    AS SPEC_S_CD    \r\n";
            SQL += "  	  , DECODE(NVL(SPEC_C_CD,'^&'),'^&','',SPEC_C_CD || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAME('EXAM_SPEC_QC_C',SPEC_C_CD))    AS SPEC_C_CD    \r\n";
            SQL += "  	  , SPEC_C_ETC                                                                                                          AS SPEC_C_ETC   \r\n";
            SQL += "  	  , SPEC_S_ETC                                                                                                          AS SPEC_S_ETC   \r\n";
            SQL += "  	  , (SELECT KORNAME FROM KOSMOS_ADM.INSA_MST WHERE SABUN = A.RESULTSABUN AND ROWNUM = 1)                                AS RESULTSABUN   \r\n";

            SQL += "    FROM KOSMOS_OCS.EXAM_ANATMST A                                                      \r\n";
            SQL += "       , KOSMOS_OCS.EXAM_SPECMST S                                                      \r\n";
            SQL += "   WHERE 1=1                                                                            \r\n";
            SQL += "     AND A.SPECNO = S.SPECNO                                                            \r\n";
            SQL += "     AND S.RECEIVEDATE BETWEEN TO_DATE('" + strFDate + " 00:00','YYYY-MM-DD HH24:MI')   \r\n";
            SQL += "                           AND TO_DATE('" + strTDate + " 23:59','YYYY-MM-DD HH24:MI')   \r\n";
            SQL += "     AND A.JDATE >= TO_DATE('" + strFDate + " 00:00','YYYY-MM-DD HH24:MI')              \r\n";

            //2018-08-08 안정수, 접수대상에서 Clinical hisory Information 항목에서 Thyroid가 있을경우... 추가함
            if (isAC == true)
            {
                SQL += "     AND A.REMARK2 IS NOT NULL                                                                          \r\n";
                SQL += "     AND UPPER(A.REMARK2) Like 'THYR%'                                                                  \r\n";
            }

            if (strSel.Equals("0") == true)
            {
                SQL += "     AND A.GBJOB = 'N'                                                                  \r\n";
            }
            else if (strSel.Equals("1") == true)
            {
                SQL += "     AND A.GBJOB = 'Y'                                                                  \r\n";
            }
            else if (strSel.Equals("2") == true)
            {
                SQL += "     AND A.GBJOB = 'V'                                                                  \r\n";
            }

            if (strAnat.Equals("1") == true)
            {
                SQL += "     AND  ( A.AnatNo  LIKE 'S%' and SUBSTR(A.AnatNo,1,2) <>'SW')                        \r\n";
            }
            else if (strAnat.Equals("2") == true)
            {
                SQL += "     AND A.AnatNo  LIKE 'C%'                                                            \r\n";
            }
            else if (strAnat.Equals("3") == true)
            {
                SQL += "     AND SUBSTR(A.AnatNo,1,1) ='P'and SUBSTR(A.AnatNo,1,2) <>'PS' and SUBSTR(A.AnatNo,1,2) <>'PU'   \r\n";
            }
            else if (strAnat.Equals("4") == true)
            {
                SQL += "     AND SUBSTR(A.AnatNo,1,2) ='PS'   \r\n";
            }
            else if (strAnat.Equals("5") == true)
            {
                SQL += "     AND SUBSTR(A.AnatNo,1,2) ='PU'   \r\n";
            }
            else if (strAnat.Equals("6") == true)
            {
                //2018-08-13 안정수, 기존 SW -> AC로 변경
                //SQL += "     AND SUBSTR(A.AnatNo,1,2) ='SW'   \r\n";
                SQL += "     AND SUBSTR(A.AnatNo,1,2) ='AC'   \r\n";
            }
            else if (strAnat.Equals("7") == true)
            {
                SQL += "     AND SUBSTR(A.AnatNo,1,2) ='OS'   \r\n";
            }
            else if (strAnat.Equals("8") == true)
            {
                SQL += "     AND SUBSTR(A.AnatNo,1,2) ='IH'   \r\n";
            }

            if (string.IsNullOrEmpty(strSearch.Trim()) == false && string.IsNullOrEmpty(strSearch2.Trim()) == false)
            {
                SQL += " AND ( UPPER(RESULT1) LIKE '%" + strSearch.Trim().Replace("'", "").ToUpper() + "%'  AND UPPER(RESULT1) LIKE '%" + strSearch2.Trim().Replace("'", "").ToUpper() + "%' )";
            }
            else if (string.IsNullOrEmpty(strSearch.Trim()) == false)
            {
                SQL += " AND UPPER(RESULT1) LIKE '%" + strSearch.Trim().Replace("'", "").ToUpper() + "%' ";
            }
            else if (string.IsNullOrEmpty(strSearch2.Trim()) == false)
            {
                SQL += " AND UPPER(RESULT1) LIKE '%" + strSearch2.Trim().Replace("'", "").ToUpper() + "%' ";
            }
    
            SQL += "     AND A.ANATNO = S.ANATNO                                                            \r\n";
            SQL += "   ORDER BY A.ANATNO, A.SPECNO                                                          \r\n";

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
