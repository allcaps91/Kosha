using ComBase; //기본 클래스
using ComDbB; //DB연결
using ComSupLibB.Com;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ComSupLibB.SupInfc
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : clsInFcSQL.cs
    /// Description     : 감염관리 쿼리
    /// Author          : 김홍록
    /// Create Date     : 2017-06-22
    /// Update History  : 
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "신규" />
    public class clsInFcSQL : clsMethod
    {

        /// <summary>2017.06.22.김홍록 : enmSel_EXAM_RESULTC_infec</summary>
        public enum enmSel_EXAM_RESULTC_infec {         SPECNO,       PANO,  SNAME, IPDOPD, DEPTCODE,   WARD,   ROOM, EMR_SCAN_YN,   RECEIVEDATE,    RESULTDATE, RESULT };
        /// <summary>2017.06.22.김홍록 : sSel_EXAM_RESULTC_infec</summary>  
        public string[] sSel_EXAM_RESULTC_infec = { "검체번호", "등록번호", "성명", "구분", "진료과", "병동", "호실",      "SCAN","검체접수일시","결과보고일시","검사항목[결과]" };
        /// <summary>2017.06.22.김홍록 : nSel_EXAM_RESULTC_infec</summary>  
        public int[] nSel_EXAM_RESULTC_infec = { nCol_SPNO, nCol_PANO, nCol_SNAME , nCol_IOPD, nCol_DPCD, nCol_WARD, nCol_WARD, nCol_SEX, nCol_TIME, nCol_TIME, nCol_NAME };

        /// <summary>2017.06.22.김홍록 : enmSel_EXAM_INFECTTRCODE</summary>
        public enum enmSel_EXAM_INFECTTRCODE { CHK,TRCODE, CHKNAME, NAME, ROWID, EDIT };
        /// <summary>2017.06.22.김홍록 : sSel_EXAM_INFECTTRCODE</summary>  
        public string[] sSel_EXAM_INFECTTRCODE = { "A", "코드", "점검명칭", "미생물명칭", "ROWID","EDIT"};
        /// <summary>2017.06.22.김홍록 : nSel_EXAM_INFECTTRCODE</summary>  
        public int[] nSel_EXAM_INFECTTRCODE = { nCol_CHEK, nCol_EXCD, nCol_NAME + 100, nCol_NAME+100, nCol_CHEK, nCol_CHEK };

        public enum enmEXAM_AUTOSEND_LOG_SEND     {       CHK, RDATE_YYYY,      JDATE,        PANO,      SNAME,   SEX,   BIRTH,   CHG,   DR_NM,   IPDOPD,     SPECNO,        EXAM_NM,    RESULT, EXAMTYPE_NM, EXAMWAY_NM, READ_ACODE, RESULT_NM,   SENDDATE,   ROWID_R,   ACODE,   EXAMTYPE,   EXAMWAY,   EXAMTYPEETC,   EXAMWAYETC,   MCODE,   SCODE,   ORDERNO,   BDATE,   RDATE, RESULTSABUN };
        public string[] sEXAM_AUTOSEND_LOG_SEND = {    "선택", "결과일자", "의뢰일자",  "등록번호", "환자성명", "SEX", "BIRTH", "CHG", "DR_NM", "IPDOPD", "검체번호",       "검사명",    "결과",  "검체유형", "검사방법", "병원체명",  "검사자", "전송일시", "ROWID_R", "ACODE", "EXAMTYPE", "EXAMWAY", "EXAMTYPEETC", "EXAMWAYETC", "MCODE", "SCODE", "ORDERNO", "BDATE", "RDATE", "RESULTSABUN" };
        public int[] nEXAM_AUTOSEND_LOG_SEND    = { nCol_SCHK,  nCol_PANO,  nCol_PANO,   nCol_PANO,  nCol_PANO,     5,       5,     5,       5,        5,  nCol_PANO, nCol_ORDERNAME, nCol_NAME,   nCol_PANO,  nCol_PANO,  nCol_NAME, nCol_PANO,  nCol_TIME,         5,       5,          5,         5,             5,            5,       5,       5,         5,       5,       5,            5 };



        public string ins_EXAM_AUTOSEND_LOG(PsmhDb pDbCon, string strdgnss_de, string strreqest_de, string strpatnt_regist_no, string strpatnt_nm, string strpatnt_sexdstn_cd, string strpatnt_lifyea_md
                                                         , string strreqestinstt_charger_nm, string strSpecNo, string strMasterCode, string strSubCode, string strResult
                                                         , string strResultDate, string strResultSabun, string strpthgogan_cd, string strspm_ty_list, string strinspct_mth_ty_list
                                                         , string strspm_ty_etc, string strinspct_mth_ty_etc, string strOrderNo, string strBDate, string GnJobSabun,  ref int nRow, string strRowID, ref string rSQL)
        {
            string SqlErr = "";
            string SQL;

            SQL = "";
            SQL += "     INSERT INTO KOSMOS_OCS.EXAM_AUTOSEND_LOG(         \r\n";
            SQL += "        JDATE,RDATE,Pano,sName,Sex,                    \r\n";
            SQL += "        BIRTH,DRNAME,SpecNo,MASTERCODE,SUBCODE,        \r\n";
            SQL += "        Result,RESULTDATE,RESULTSABUN,ACODE,EXAMTYPE,  \r\n";
            SQL += "        EXAMWAY,EXAMTYPEETC,EXAMWAYETC,ORDERNO,        \r\n";
            SQL += "        BDATE,SENDDATE,SENDSABUN) VALUES (             \r\n";
            SQL += "        TO_DATE('" + strdgnss_de + "','YYYY-MM-DD'), TO_DATE('" + strreqest_de + "','YYYY-MM-DD'), '" + strpatnt_regist_no + "','" + strpatnt_nm + "','" + strpatnt_sexdstn_cd + "', ";
            SQL += "        '" + strpatnt_lifyea_md + "','" + strreqestinstt_charger_nm + "','" + strSpecNo + "','" + strMasterCode + "','" + strSubCode + "', ";
            SQL += "        '" + strResult + "',TO_DATE('" + strResultDate + "','YYYY-MM-DD HH24:MI'), " + strResultSabun + ",'" + strpthgogan_cd + "','" + strspm_ty_list + "',";
            SQL += "        '" + strinspct_mth_ty_list + "','" + strspm_ty_etc + "','" + strinspct_mth_ty_etc + "', " + strOrderNo + ", ";
            SQL += "        TO_DATE('" + strBDate + "','YYYY-MM-DD'), SYSDATE, " + GnJobSabun + ") ";

            rSQL = SQL;


            SqlErr = clsDB.ExecuteNonQuery(SQL, ref nRow, pDbCon);
            return SqlErr;
        }

        public DataSet sel_EXAM_AUTOSEND_LOG_SEND(PsmhDb pDbCon, bool isOpt1, string strOptA, string strFDate, string strTDate, string strPart)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";

            try
            {

                SQL += "  WITH T AS (                                                                                                                                      \r\n";                         
                SQL += "   SELECT TO_CHAR(A.RESULTDATE, 'YYYY-MM-DD HH24:MI') RDATE, TO_CHAR(A.RECEIVEDATE, 'YYYY-MM-DD') JDATE, A.PANO, E.SNAME,                          \r\n";
                SQL += "               E.JUMIN2 SEX, E.JUMIN1, A.DRCODE, A.IPDOPD,                                                                                         \r\n";
                SQL += "               A.SPECNO, B.MASTERCODE CODE, B.RESULT,  B.RESULTSABUN,                                                                              \r\n";
                SQL += "               C.SPECCODE, D.NEWCODE ACODE, A.ORDERDATE, B.MASTERCODE MCODE,                                                                          \r\n";
                SQL += "               B.SUBCODE SCODE, A.ORDERNO, A.BDATE                                                                                                 \r\n";
                SQL += "              ,D.PART || '.' || D.NAMEK || '.' || D.NAMEK AS READ_ACODE                                                                            \r\n";
                SQL += "  FROM KOSMOS_OCS.EXAM_SPECMST A, KOSMOS_OCS.EXAM_RESULTC B, KOSMOS_OCS.EXAM_MASTER C, KOSMOS_OCS.EXAM_AUTOSEND_CODE D, KOSMOS_PMPA.BAS_PATIENT E  \r\n";
                SQL += "  WHERE A.SPECNO = B.SPECNO                                                                                                                        \r\n";
                //SQL += "    AND A.STATUS = '05'                                                                                                                            \r\n";
                //2019-11-14 안정수, 조건 추가
                if (strPart == "")
                {
                    SQL += "    AND A.STATUS = '05'                                                                                                                        \r\n";
                }
                else
                {
                    SQL += "    AND A.STATUS = '04'                                                                                                                        \r\n";
                }
                SQL += "    AND B.RESULT IS NOT NULL                                                                                                                       \r\n";
                SQL += "    AND B.MASTERCODE = C.MASTERCODE                                                                                                                \r\n";
                SQL += "    AND B.MASTERCODE <> B.SUBCODE                                                                                                                  \r\n";
                SQL += "    AND C.ACODE_NEW = D.NEWCODE                                                                                                                           \r\n";
                SQL += "    AND A.PANO = E.PANO                                                                                                                            \r\n";

                if (isOpt1 == true)
                {
                    SQL += "    AND A.RESULTDATE >= TO_DATE('" + strFDate + " 00:00','YYYY-MM-DD HH24:MI')                                                                           \r\n";
                    SQL += "    AND A.RESULTDATE <= TO_DATE('" + strTDate + " 23:59','YYYY-MM-DD HH24:MI')                                                                           \r\n";
                }
                else
                {
                    SQL += "    AND A.RECEIVEDATE >= TO_DATE('" + strFDate + " 00:00','YYYY-MM-DD HH24:MI')                                                                           \r\n";
                    SQL += "    AND A.RECEIVEDATE <= TO_DATE('" + strTDate + " 23:59','YYYY-MM-DD HH24:MI')                                                                           \r\n";
                }

                if (strOptA.Equals("A1") == false)
                {

                }
                else
                {
                    if (strOptA.Equals("A2") == true)
                    {
                        SQL += "     AND EXISTS                                                                                                                                \r\n";
                    }
                    else
                    {
                        SQL += "     AND NOT EXISTS                                                                                                                                \r\n";
                    }

                    SQL += "   ( SELECT * FROM KOSMOS_OCS.EXAM_AUTOSEND_LOG SUB                                                                                                \r\n";
                    SQL += "   WHERE SUB.BDATE = A.BDATE                                                                                                                       \r\n";
                    SQL += "     AND SUB.SPECNO = A.SPECNO                                                                                                                     \r\n";
                    SQL += "     AND SUB.MASTERCODE = B.MASTERCODE                                                                                                             \r\n";
                    SQL += "     AND SUB.SUBCODE = B.SUBCODE                                                                                                                   \r\n";
                    SQL += "     AND SUB.RESULTDATE = A.RESULTDATE )                                                                                                           \r\n";

                }


                SQL += "  UNION ALL                                                                                                                                        \r\n";
                SQL += "  SELECT TO_CHAR(A.RESULTDATE, 'YYYY-MM-DD HH24:MI') RDATE, TO_CHAR(A.RECEIVEDATE, 'YYYY-MM-DD') JDATE, A.PANO, E.SNAME,                           \r\n";
                SQL += "              E.JUMIN2 SEX, E.JUMIN1, A.DRCODE, A.IPDOPD,                                                                                          \r\n";
                SQL += "              A.SPECNO, B.SUBCODE CODE, B.RESULT,  B.RESULTSABUN,                                                                                  \r\n";
                SQL += "              C.SPECCODE, D.NEWCODE ACODE, A.ORDERDATE, B.MASTERCODE MCODE,                                                                           \r\n";
                SQL += "              B.SUBCODE SCODE, A.ORDERNO, A.BDATE                                                                                                  \r\n";
                SQL += "             ,D.PART || '.' || D.NAMEK || '.' || D.NAMEK AS READ_ACODE                                                                             \r\n";
                SQL += "  FROM KOSMOS_OCS.EXAM_SPECMST A, KOSMOS_OCS.EXAM_RESULTC B, KOSMOS_OCS.EXAM_MASTER C, KOSMOS_OCS.EXAM_AUTOSEND_CODE D, KOSMOS_PMPA.BAS_PATIENT E  \r\n";
                SQL += "  WHERE A.SPECNO = B.SPECNO                                                                                                                        \r\n";
                //SQL += "    AND A.STATUS = '05'                                                                                                                            \r\n";
                //2019-11-14 안정수, 조건 추가
                if (strPart == "")
                {
                    SQL += "    AND A.STATUS = '05'                                                                                                                        \r\n";
                }
                else
                {
                    SQL += "    AND A.STATUS = '04'                                                                                                                        \r\n";
                }
                SQL += "    AND B.RESULT IS NOT NULL                                                                                                                       \r\n";
                SQL += "    AND B.SUBCODE = C.MASTERCODE                                                                                                                   \r\n";
                SQL += "    AND C.ACODE_NEW = D.NEWCODE                                                                                                                           \r\n";
                SQL += "    AND A.PANO = E.PANO                                                                                                                            \r\n";

                if (isOpt1 == true)
                {
                    SQL += "    AND A.RESULTDATE >= TO_DATE('" + strFDate + " 00:00','YYYY-MM-DD HH24:MI')                                                                           \r\n";
                    SQL += "    AND A.RESULTDATE <= TO_DATE('" + strTDate + " 23:59','YYYY-MM-DD HH24:MI')                                                                           \r\n";
                }
                else
                {
                    SQL += "    AND A.RECEIVEDATE >= TO_DATE('" + strFDate + " 00:00','YYYY-MM-DD HH24:MI')                                                                           \r\n";
                    SQL += "    AND A.RECEIVEDATE <= TO_DATE('" + strTDate + " 23:59','YYYY-MM-DD HH24:MI')                                                                           \r\n";
                }

                if (strOptA.Equals("A1") == false)
                {

                }
                else
                {
                    if (strOptA.Equals("A2") == true)
                    {
                        SQL += "     AND EXISTS                                                                                                                                \r\n";
                    }
                    else
                    {
                        SQL += "     AND NOT EXISTS                                                                                                                                \r\n";
                    }

                    SQL += "   ( SELECT * FROM KOSMOS_OCS.EXAM_AUTOSEND_LOG SUB                                                                                                \r\n";
                    SQL += "   WHERE SUB.BDATE = A.BDATE                                                                                                                       \r\n";
                    SQL += "     AND SUB.SPECNO = A.SPECNO                                                                                                                     \r\n";
                    SQL += "     AND SUB.MASTERCODE = B.MASTERCODE                                                                                                             \r\n";
                    SQL += "     AND SUB.SUBCODE = B.SUBCODE                                                                                                                   \r\n";
                    SQL += "     AND SUB.RESULTDATE = A.RESULTDATE )                                                                                                           \r\n";

                }

                SQL += "   ORDER BY ACODE ASC, RDATE ASC, PANO                                                                                                             \r\n";
                SQL += "   )                                                                                                                                               \r\n";
                SQL += "  SELECT                                                                                                                                           \r\n";
                SQL += "  		 '' 																AS CHK  		--01                                                   \r\n";
                SQL += "  		, TRIM(SUBSTR(T.RDATE,1,10)) 										AS RDATE_YYYY   --02                                                   \r\n";
                SQL += "  		, T.JDATE                     										AS JDATE        --03                                                   \r\n";
                SQL += "  		, T.PANO                      										AS PANO         --04                                                   \r\n";
                SQL += "  		, T.SNAME                     										AS SNAME        --05                                                   \r\n";
                SQL += "  		, CASE WHEN SUBSTR(T.SEX, 1, 1) = '9' THEN '1'                                                                                             \r\n";
                SQL += "               WHEN SUBSTR(T.SEX, 1, 1) = '0' THEN '2'                                                                                             \r\n";
                SQL += "               WHEN SUBSTR(T.SEX, 1, 1) = '1' OR SUBSTR(T.SEX, 1, 1) = '5'  THEN '1'                                                           \r\n";
                SQL += "               WHEN SUBSTR(T.SEX, 1, 1) = '2' OR SUBSTR(T.SEX, 1, 1) = '6'  THEN '2'                                                           \r\n";
                SQL += "               WHEN SUBSTR(T.SEX, 1, 1) = '3' OR SUBSTR(T.SEX, 1, 1) = '7'  THEN '1'                                                           \r\n";
                SQL += "               WHEN SUBSTR(T.SEX, 1, 1) = '4' OR SUBSTR(T.SEX, 1, 1) = '8'  THEN '2'                                                           \r\n";
                SQL += "           END                                                              AS SEX          --06                                                   \r\n";
                SQL += "  		, KOSMOS_OCS.FC_BAS_PATIENT_BIRTH(T.PANO) 							AS BIRTH	    --07                                                   \r\n";
                SQL += "  		, ''																AS CHG			--08                                                   \r\n";
                SQL += "  		, KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(T.DRCODE) 						AS DR_NM      	--09                                                   \r\n";
                SQL += "  		, T.IPDOPD															AS IPDOPD		--10                                                   \r\n";
                SQL += "  		, T.SPECNO															AS SPECNO		--11                                                   \r\n";
                SQL += "  		, KOSMOS_OCS.FC_EXAM_RESULTC_EXAMNAME(T.SPECNO)						AS EXAM_NM		--12                                                   \r\n";
                SQL += "  		, T.RESULT								    						AS RESULT		--13                                                   \r\n";
                SQL += "  		, KOSMOS_OCS.FC_INFECTION_CONV_TYPE_NAME(B.EXAMTYPE, B.EXAMTYPEETC) AS EXAMTYPE_NM  --14                                                 \r\n";
                SQL += "  		, KOSMOS_OCS.FC_INFECTION_CONV_WAY_NAME(B.EXAMWAY, B.EXAMWAYETC)  AS EXAMWAY_NM    --15                                                  \r\n";
                SQL += "  		, T.READ_ACODE								    					AS READ_ACODE  --16                                                    \r\n";
                SQL += "  		, KOSMOS_OCS.FC_BAS_USER_USERNAME2(TRIM(T.RESULTSABUN))				AS RESULT_NM   --17                                                    \r\n";
                SQL += "  		, TO_CHAR(B.SENDDATE,'YYYY-MM-DD HH24:MI') 							AS SENDDATE    --18                                                    \r\n";
                SQL += "  		, B.ROWID								 							AS ROWID_R	   --19                                                    \r\n";
                SQL += "  		, T.ACODE								 							AS ACODE	   --20                                                    \r\n";
                SQL += "  		, B.EXAMTYPE								 						AS EXAMTYPE	   --21                                                    \r\n";
                SQL += "  		, B.EXAMWAY								 							AS EXAMWAY	   --22                                                    \r\n";
                SQL += "  		, B.EXAMWAY								 							AS EXAMTYPEETC --23                                                    \r\n";
                SQL += "  		, B.EXAMWAYETC								 						AS EXAMWAYETC --24                                                     \r\n";
                SQL += "  		, T.MCODE								 							AS MCODE 	   --25                                                    \r\n";
                SQL += "  		, T.SCODE								 							AS SCODE 	   --26                                                    \r\n";
                SQL += "  		, T.ORDERNO								 							AS ORDERNO 	   --27                                                    \r\n";
                SQL += "  		, T.BDATE								 							AS BDATE 	   --28                                                    \r\n";
                SQL += "  		, T.RDATE								 							AS RDATE 	   --29                                                    \r\n";
                SQL += "  		, T.RESULTSABUN								 						AS RESULTSABUN --30                                                    \r\n";
                SQL += "    FROM T                                                                                                                                         \r\n";
                SQL += "       , KOSMOS_OCS.EXAM_AUTOSEND_LOG B                                                                                                            \r\n";
                SQL += "   WHERE 1=1                                                                                                                                       \r\n";
                SQL += "     AND T.BDATE = B.BDATE(+)                                                                                                                      \r\n";
                SQL += "     AND T.SPECNO = B.SPECNO(+)                                                                                                                    \r\n";
                SQL += "     AND TRIM(T.MCODE) = TRIM(B.MASTERCODE(+))                                                                                                     \r\n";
                SQL += "     AND TRIM(T.SCODE) = TRIM(B.SUBCODE(+))                                                                                                        \r\n";
                SQL += "   ORDER BY T.ACODE ASC, T.RDATE ASC, T.PANO                                                                                                        \r\n";

                SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }



            return ds;

        }

        /// <summary>감염관리에서 관리하는 검사 항목 조회</summary>
        /// <param name="strFDate">시작일자</param>
        /// <param name="strTDate">종료일자</param>
        /// <param name="strDept">과</param>
        /// <param name="enmGBIO">환자구분</param>
        /// <param name="lstSql">검사항목</param>
        /// <returns></returns>
        public DataSet sel_EXAM_RESULTC_infec(PsmhDb pDbCon, string strFDate, string strTDate, string strDept, enmComParamGBIO enmGBIO, List<string>lstSql)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string sqlWhere = "";

            if (lstSql.Count > 0)
            {
                for (int i = 0; i < lstSql.Count; i++)
                {
                    sqlWhere += "'" + lstSql[i] +"',";
                }

                sqlWhere = sqlWhere.Substring(0, sqlWhere.Length - 1);
            }


            SQL = "";

            try
            {

                SQL += "  SELECT                                                                                                        \r\n";
                SQL += "  	  CASE WHEN GROUPING(ROW_ID) = 1 THEN '총계'  			 ELSE MAX(SPECNO)  		END 	AS SPECNO           \r\n";
                SQL += "  	, CASE WHEN GROUPING(ROW_ID) = 1 THEN TO_CHAR(SUM(CNT))  ELSE MAX(PANO)    		END 	AS PANO             \r\n";
                SQL += "  	, CASE WHEN GROUPING(ROW_ID) = 1 THEN ''			     ELSE MAX(SNAME)   		END 	AS SNAME            \r\n";
                SQL += "  	, CASE WHEN GROUPING(ROW_ID) = 1 THEN ''			     ELSE MAX(IPDOPD)  		END 	AS IPDOPD           \r\n";
                SQL += "  	, CASE WHEN GROUPING(ROW_ID) = 1 THEN ''			     ELSE MAX(DEPTCODE) 	END 	AS DEPTCODE         \r\n";
                SQL += "  	, CASE WHEN GROUPING(ROW_ID) = 1 THEN ''			     ELSE MAX(WARD)   		END 	AS WARD             \r\n";
                SQL += "  	, CASE WHEN GROUPING(ROW_ID) = 1 THEN ''			     ELSE MAX(ROOM)   		END 	AS ROOM             \r\n";
                SQL += "  	, CASE WHEN GROUPING(ROW_ID) = 1 THEN ''			     ELSE MAX(EMR_SCAN_YN)  END 	AS EMR_SCAN_YN      \r\n";
                SQL += "  	, CASE WHEN GROUPING(ROW_ID) = 1 THEN ''			     ELSE MAX(RECEIVEDATE)  END 	AS RECEIVEDATE      \r\n";
                SQL += "  	, CASE WHEN GROUPING(ROW_ID) = 1 THEN ''			     ELSE MAX(RESULTDATE)   END 	AS RESULTDATE       \r\n";
                SQL += "  	, CASE WHEN GROUPING(ROW_ID) = 1 THEN ''			     ELSE MAX(RESULT)   	END 	AS RESULT           \r\n";
                SQL += "    FROM (                                                                                                      \r\n";
                SQL += "  	SELECT                                                                                                      \r\n";
                SQL += "  	        A.SPECNO															AS SPECNO 	-- 01               \r\n";
                SQL += "  	      , A.PANO      														AS PANO		-- 02               \r\n";
                SQL += "  	      , A.SNAME     														AS SNAME	-- 03               \r\n";
                SQL += "  	      , A.IPDOPD        													AS IPDOPD	-- 04               \r\n";
                SQL += "  	      , A.DEPTCODE      													AS DEPTCODE	-- 05               \r\n";
                SQL += "  	      , DECODE(A.IPDOPD,'O','',A.WARD)	  								    AS WARD		-- 06               \r\n";
                SQL += "  	      , DECODE(A.IPDOPD,'O','',A.ROOM)										AS ROOM		-- 07               \r\n";
                SQL += "  	      , KOSMOS_OCS.FC_EMR_TREATT_ISNULL(                                                                    \r\n";
                SQL += "  	      									 A.PANO                                                             \r\n";
                SQL += "  	      								   , TRIM(A.IPDOPD)                                                     \r\n";
                SQL += "  	      								   , TO_CHAR(A.RECEIVEDATE ,'YYYYMMDD')                                 \r\n";
                SQL += "  	      								   , TRIM(A.DEPTCODE)                                                   \r\n";
                SQL += "  	      								   , TO_CHAR(A.RECEIVEDATE + 1,'YYYYMMDD')                              \r\n";
                SQL += "  	      								   )									AS EMR_SCAN_YN -- 09/08         \r\n";
                SQL += "  		  , TO_CHAR(A.RECEIVEDATE ,'YYYY-MM-DD HH24:MI') 						AS RECEIVEDATE                  \r\n";
                SQL += "  		  , TO_CHAR(A.RESULTDATE  ,'YYYY-MM-DD HH24:MI') 						AS RESULTDATE                   \r\n";
                SQL += "  		  , KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(B.SUBCODE) || '[' ||MAX(B.RESULT) || ']' AS RESULT               \r\n";
                SQL += "  		  , COUNT(1) CNT                                                                                        \r\n";
                SQL += "  		  , MAX(ROWNUM) AS ROW_ID                                                                               \r\n";
                SQL += "  	 FROM KOSMOS_OCS.EXAM_SPECMST A                                                                             \r\n";
                SQL += "  	    , KOSMOS_OCS.EXAM_RESULTC B                                                                             \r\n";
                SQL += "  	WHERE 1=1                                                                                                   \r\n";
                SQL += "  	  AND A.SPECNO      = B.SPECNO                                                                              \r\n";
                SQL += "  	  AND (A.CANCEL IS NULL OR A.CANCEL=' ')                                                                    \r\n";
                SQL += "  	  AND A.RECEIVEDATE BETWEEN " + ComFunc.covSqlDate(strFDate + " 00:00", "YYYY-MM-DD HH24:MI", false);
                SQL += "  	                        AND " + ComFunc.covSqlDate(strTDate + " 23:59", "YYYY-MM-DD HH24:MI", false);
                SQL += "  	  AND A.STATUS      = '05'                                                                                  \r\n";

                if (string.IsNullOrEmpty(sqlWhere) == false)
                {
                    SQL += "  	  AND B.SUBCODE     IN (" + sqlWhere + ")                                                               \r\n";
                }

                if (enmGBIO == enmComParamGBIO.I)
                {
                    SQL += "  	  AND A.IPDOPD     = 'I'                                                                                \r\n";
                }
                else if (enmGBIO == enmComParamGBIO.O)
                {
                    SQL += "  	  AND A.IPDOPD     = 'O'                                                                                \r\n";
                }

                if (string.IsNullOrEmpty(strDept) == false)
                {
                    SQL += "  	  AND A.DEPTCODE     = " + ComFunc.covSqlstr(strDept, false);
                }
                
                SQL += "  	GROUP BY A.SPECNO       , A.PANO         , A.SNAME          , A.IPDOPD      , A.DEPTCODE,                   \r\n";
                SQL += "  	         A.BLOODDATE    , A.ORDERDATE    , A.RECEIVEDATE    , A.RESULTDATE  , B.SUBCODE,                    \r\n";
                SQL += "  	         A.WARD         , A.ROOM                                                                            \r\n";
                SQL += "  	ORDER BY A.RECEIVEDATE,A.SPECNO                                                                             \r\n";
                SQL += "  )                                                                                                             \r\n";
                SQL += "  GROUP BY ROLLUP(ROW_ID)                                                                                       \r\n";
                SQL += "  ORDER BY ROW_ID                                                                                               \r\n";
                                                                                                                        
                SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }



            return ds;

        }

        /// <summary>기초코드관리</summary>
        /// <returns></returns>
        public DataSet sel_EXAM_INFECTTRCODE(PsmhDb pDbCon)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";

            try
            {

                SQL += "  SELECT                                    \r\n";
                SQL += "  		'' CHK                              \r\n";
                SQL += "  	  , TRCODE                              \r\n";
                SQL += "  	  , CHKNAME                             \r\n";
                SQL += "  	  , NAME                                \r\n";
                SQL += "  	  , ROWID                               \r\n";
                SQL += "  	  , '' EDIT                             \r\n";
                SQL += "   FROM  KOSMOS_OCS.EXAM_INFECTTRCODE       \r\n";
                SQL += "  ORDER BY NAME, TRCODE                     \r\n";


                SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return null;
                }


            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }



            return ds;
        }

        /// <summary>EXAM_INFECTTRCODE 코드 관리</summary>
        /// <param name="strTRCODE">INFECTTRCODE 관리</param>
        /// <param name="strCHKNAME">INFECTTRCODE 이름</param>
        /// <param name="strNAME"></param>
        /// <param name="TRS"></param>
        /// <param name="nRow"></param>
        /// <param name="strRowID"></param>
        /// <returns></returns>
        public string ins_EXAM_INFECTTRCODE(PsmhDb pDbCon, string strTRCODE, string strCHKNAME, string strNAME, ref int nRow, string strRowID, ref string rSQL)
        {
            string SqlErr = "";
            string SQL;

            SQL = "";

            if (string.IsNullOrEmpty(strRowID) == true)
            {
                SQL += " INSERT INTO " + ComNum.DB_MED + "EXAM_INFECTTRCODE ( TRCODE, CHKNAME,NAME) \r\n";
                SQL += "   VALUES(          \r\n";
                SQL += "           " + ComFunc.covSqlstr(strTRCODE, false);
                SQL += "          ," + ComFunc.covSqlstr(strCHKNAME, false);
                SQL += "          ," + ComFunc.covSqlstr(strNAME, false);
                SQL += "          )";
            }
            else
            {
                SQL += "     UPDATE " + ComNum.DB_MED + "EXAM_INFECTTRCODE                          \r\n";
                SQL += "        SET TRCODE  = " + ComFunc.covSqlstr(strTRCODE, false);                         
                SQL += "          , CHKNAME = " + ComFunc.covSqlstr(strCHKNAME, false);
                SQL += "          , NAME    = " + ComFunc.covSqlstr(strNAME, false);
                SQL += "      WHERE 1=1                                                             \r\n";
                SQL += "        AND ROWID   = " + ComFunc.covSqlstr(strRowID, false);

            }

            rSQL = SQL;


            SqlErr = clsDB.ExecuteNonQuery(SQL, ref nRow, pDbCon);
            return SqlErr;
        }

        public DataTable GetEnvironmentOrderInfo(string orderNo)
        {
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);
                sql = new StringBuilder();
                sql.Append("SELECT C.CODENAME AS GRADENAME1             ").Append("\n");
                sql.Append("     , D.CODENAME AS GRADENAME2             ").Append("\n");
                sql.Append("     , E.CODENAME AS GRADENAME3             ").Append("\n");
                sql.Append("     , F.CODENAME AS GRADENAME4             ").Append("\n");
                sql.Append("  FROM ENVIRONMENT_ORDER A                  ").Append("\n");
                sql.Append("  LEFT OUTER JOIN ENVIRONMENT_EXAM_MASTER B ").Append("\n");
                sql.Append("               ON A.ENVIRONMENTCODE = B.CODE").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT C   ").Append("\n");
                sql.Append("               ON B.GRADE1  = C.CODE        ").Append("\n");
                sql.Append("              AND C.GRADE   = 1             ").Append("\n");
                sql.Append("              AND C.USEYN   = 'Y'           ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT D   ").Append("\n");
                sql.Append("               ON B.GRADE2  = D.CODE        ").Append("\n");
                sql.Append("              AND D.GRADE   = 2             ").Append("\n");
                sql.Append("              AND D.USEYN   = 'Y'           ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT E   ").Append("\n");
                sql.Append("               ON B.GRADE3  = E.CODE        ").Append("\n");
                sql.Append("              AND E.GRADE   = 3             ").Append("\n");
                sql.Append("              AND E.USEYN   = 'Y'           ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT F   ").Append("\n");
                sql.Append("               ON B.GRADE4  = C.CODE        ").Append("\n");
                sql.Append("              AND F.GRADE   = 4             ").Append("\n");
                sql.Append("              AND F.USEYN   = 'Y'           ").Append("\n");
                sql.Append(" WHERE A.ORDERNO = '" + orderNo + "'        ").Append("\n");
                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    return null;
                }

                return dt;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, sql.ToString(), clsDB.DbCon); //에러로그 저장
                return null;
            }
        }

        public DataTable GetEnvironmentSpecInfo(string specNo)
        {
            DataTable dt = null;
            StringBuilder sql = new StringBuilder();
            string SqlErr = string.Empty;

            try
            {
                sql = new StringBuilder();
                sql.Append("SELECT C.CODENAME AS GRADENAME1             ").Append("\n");
                sql.Append("     , D.CODENAME AS GRADENAME2             ").Append("\n");
                sql.Append("     , E.CODENAME AS GRADENAME3             ").Append("\n");
                sql.Append("     , F.CODENAME AS GRADENAME4             ").Append("\n");
                sql.Append("  FROM ENVIRONMENT_ORDER A                  ").Append("\n");
                sql.Append("  LEFT OUTER JOIN ENVIRONMENT_EXAM_MASTER B ").Append("\n");
                sql.Append("               ON A.ENVIRONMENTCODE = B.CODE").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT C   ").Append("\n");
                sql.Append("               ON B.GRADE1  = C.CODE        ").Append("\n");
                sql.Append("              AND C.GRADE   = 1             ").Append("\n");
                sql.Append("              AND C.USEYN   = 'Y'           ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT D   ").Append("\n");
                sql.Append("               ON B.GRADE2  = D.CODE        ").Append("\n");
                sql.Append("              AND D.GRADE   = 2             ").Append("\n");
                sql.Append("              AND D.USEYN   = 'Y'           ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT E   ").Append("\n");
                sql.Append("               ON B.GRADE3  = E.CODE        ").Append("\n");
                sql.Append("              AND E.GRADE   = 3             ").Append("\n");
                sql.Append("              AND E.USEYN   = 'Y'           ").Append("\n");
                sql.Append("  LEFT OUTER JOIN BAS_GRADE_ENVIRONMENT F   ").Append("\n");
                sql.Append("               ON B.GRADE4  = C.CODE        ").Append("\n");
                sql.Append("              AND F.GRADE   = 4             ").Append("\n");
                sql.Append("              AND F.USEYN   = 'Y'           ").Append("\n");
                sql.Append(" WHERE A.SPECNO = '" + specNo + "'          ").Append("\n");
                SqlErr = clsDB.GetDataTable(ref dt, sql.ToString(), clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    return null;
                }

                return dt;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, sql.ToString(), clsDB.DbCon); //에러로그 저장
                return null;
            }
        }

        public void ExamResultClone(string specNo)
        {
            ExamResultClone(specNo, string.Empty, string.Empty);
        }

        public void ExamResultClone(string startDate, string endDate)
        {
            ExamResultClone(string.Empty, startDate, endDate);
        }

        /// <summary>
        /// 검사결과 복사
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        private void ExamResultClone(string specNo, string startDate, string endDate)
        {
            StringBuilder sql = null;
            string SqlErr = string.Empty;
            DataTable dt = null;
            int intRowAffected = 0;

            try
            {
                //  오더테이블에 검사의뢰 상태 업데이트 
                //  KOSMOS_OCS.EXAM_SPECMST.STATUS -> KOSMOS_PMPA.ENVIRONMENT_ORDER.STATUS
                sql = new StringBuilder();
                sql.Append("UPDATE (                                            ").Append("\n");
                sql.Append("    SELECT A.STATUS                                 ").Append("\n");
                sql.Append("         , B.STATUS AS NEWSTATUS                    ").Append("\n");
                sql.Append("      FROM ENVIRONMENT_ORDER A                      ").Append("\n");
                sql.Append("      INNER JOIN KOSMOS_OCS.EXAM_SPECMST B          ").Append("\n");
                sql.Append("              ON A.SPECNO = B.SPECNO                ").Append("\n");
                sql.Append("             AND A.STATUS <> '05'                   ").Append("\n");
                sql.Append("     WHERE 1 = 1                                    ").Append("\n");

                if (string.IsNullOrWhiteSpace(specNo))
                {
                    sql.Append("       AND A.ORDERDATE BETWEEN '" + startDate + "'  ").Append("\n");
                    sql.Append("                           AND '" + endDate + "'    ").Append("\n");
                }
                else
                {
                    sql.Append("       AND A.SPECNO = '" + specNo + "'              ").Append("\n");
                }
                
                sql.Append(") A                                                 ").Append("\n");
                sql.Append("SET A.STATUS = A.NEWSTATUS                          ").Append("\n");
                SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //  조회기간 내에 완료가 아닌 검사결과를 삭제
                sql = new StringBuilder();
                sql.Append("DELETE FROM ENVIRONMENT_EXAM_RESULTC                ").Append("\n");
                sql.Append(" WHERE SPECNO IN(                                   ").Append("\n");
                sql.Append("    SELECT SPECNO                                   ").Append("\n");
                sql.Append("      FROM ENVIRONMENT_ORDER                        ").Append("\n");
                sql.Append("     WHERE 1 = 1                                    ").Append("\n");

                if(string.IsNullOrWhiteSpace(specNo))
                {
                    sql.Append("       AND ORDERDATE BETWEEN '" + startDate + "'    ").Append("\n");
                    sql.Append("                         AND '" + endDate + "'      ").Append("\n");
                }
                else
                {
                    sql.Append("       AND SPECNO = '" + specNo + "'                ").Append("\n");
                }
                
                sql.Append("       AND STATUS <> '01'                           ").Append("\n");
                sql.Append(" )                                                  ").Append("\n");
                SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                sql = new StringBuilder();
                sql.Append("DELETE FROM ENVIRONMENT_EXAM_RESULTCF               ").Append("\n");
                sql.Append(" WHERE SPECNO IN(                                   ").Append("\n");
                sql.Append("    SELECT SPECNO                                   ").Append("\n");
                sql.Append("      FROM ENVIRONMENT_ORDER                        ").Append("\n");
                sql.Append("     WHERE 1 = 1                                    ").Append("\n");

                if (string.IsNullOrWhiteSpace(specNo))
                {
                    sql.Append("       AND ORDERDATE BETWEEN '" + startDate + "'    ").Append("\n");
                    sql.Append("                         AND '" + endDate + "'      ").Append("\n");
                }
                else
                {
                    sql.Append("       AND SPECNO = '" + specNo + "'                ").Append("\n");
                }

                sql.Append("       AND STATUS <> '01'                           ").Append("\n");
                sql.Append(" )                                                  ").Append("\n");
                SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //  조회기간의 검사결과를 다시 가져온다.
                sql = new StringBuilder();
                sql.Append("INSERT INTO ENVIRONMENT_EXAM_RESULTCF               ").Append("\n");
                sql.Append("SELECT *                                            ").Append("\n");
                sql.Append("  FROM KOSMOS_OCS.EXAM_RESULTCF                     ").Append("\n");
                sql.Append(" WHERE SPECNO IN(                                   ").Append("\n");
                sql.Append("    SELECT SPECNO                                   ").Append("\n");
                sql.Append("      FROM ENVIRONMENT_ORDER                        ").Append("\n");
                sql.Append("     WHERE 1 = 1                                    ").Append("\n");

                if(string.IsNullOrWhiteSpace(specNo))
                {
                    sql.Append("       AND ORDERDATE BETWEEN '" + startDate + "'    ").Append("\n");
                    sql.Append("                         AND '" + endDate + "'      ").Append("\n");
                }
                else
                {
                    sql.Append("       AND SPECNO = '" + specNo + "'                ").Append("\n");
                }
                
                sql.Append("       AND STATUS NOT IN('01')                      ").Append("\n");
                sql.Append("  )                                                 ").Append("\n");
                SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //  조회기간의 검사결과를 다시 가져온다.
                sql = new StringBuilder();
                sql.Append("INSERT INTO ENVIRONMENT_EXAM_RESULTC                ").Append("\n");
                sql.Append("SELECT *                                            ").Append("\n");
                sql.Append("  FROM KOSMOS_OCS.EXAM_RESULTC                      ").Append("\n");
                sql.Append(" WHERE SPECNO IN(                                   ").Append("\n");
                sql.Append("    SELECT SPECNO                                   ").Append("\n");
                sql.Append("      FROM ENVIRONMENT_ORDER                        ").Append("\n");
                sql.Append("     WHERE 1 = 1                                    ").Append("\n");

                if (string.IsNullOrWhiteSpace(specNo))
                {
                    sql.Append("       AND ORDERDATE BETWEEN '" + startDate + "'    ").Append("\n");
                    sql.Append("                         AND '" + endDate + "'      ").Append("\n");
                }
                else
                {
                    sql.Append("       AND SPECNO = '" + specNo + "'                ").Append("\n");
                }

                sql.Append("       AND STATUS NOT IN('01')                      ").Append("\n");
                sql.Append("  )                                                 ").Append("\n");
                SqlErr = clsDB.ExecuteNonQuery(sql.ToString(), ref intRowAffected, clsDB.DbCon);

                if (!string.IsNullOrWhiteSpace(SqlErr))
                {
                    clsDB.SaveSqlErrLog(SqlErr, sql.ToString(), clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 조회 중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, sql.ToString(), clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }

        }

    }
}
