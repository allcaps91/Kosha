using ComBase; //기본 클래스
using ComDbB; //DB연결
using ComSupLibB.Com;
using System;
using System.Data;
using System.Text;

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
    public class clsComSupLbExRsltSQL : Com.clsMethod  
    {
        string SQL = string.Empty;

        public enum     enmSel_EXAM_RESULT_BAE    {       CHK,    SEQ,        ENTDATE,        RESULT,    SPECNO,     SUBCODE,   BDATE,   PANO,   SNAME,   SEX,   DEPTCODE,   IPDOPD,   WARD,   EXAMFNAME, ROW_ID };
        public string[] sSel_EXAM_RESULT_BAE =    {    "삭제",  "SEQ", "중간결과일시",    "중간결과",  "검체명",  "검사코드", "BDATE", "PANO", "SNAME", "SEX", "DEPTCODE", "IPDOPD", "WARD", "EXAMFNAME", "ROWID" };
        public int[]    nSel_EXAM_RESULT_BAE =    { nCol_SCHK,      5,      nCol_TIME, nCol_JUSO+120, nCol_PANO,   nCol_CHEK,       5,      5,       5,     5,          5,        5,      5,           5,      5  };

        public enum enmSel_EXAM_SPECMST           {        SPECNO,         SNAME,        ER,      STRT,   HELS_HR,   HELS_TO,       WON,      GLUE,       ERP,     IPDOPD,   BDATE,      PANO,   DEPTCODE };
        public string[] sSel_EXAM_SPECMST   =     {    "검체번호",    "환자성명",  "응급실",    "응급",    "신검",    "종검",    "거리",    "혈당",  "중증도",     "구분", "BDATE",    "PANO",       "과" };
        public int[] nSel_EXAM_SPECMST      =     {  nCol_CHEK+10,  nCol_CHEK+10, nCol_SCHK, nCol_SCHK, nCol_SCHK, nCol_SCHK, nCol_SCHK, nCol_SCHK, nCol_SCHK,  nCol_SCHK,       5, nCol_PANO, nCol_SCHK };

        public enum enmSel_EXAM_SPECMST_CURR      { SPECNO, PANO, SNAME, SODIUM, HEPARIN, STATUS, RDATE, DRCOMMENT, BLOODDATE, DC_I, STRT, GLUE, WON, SMS, ERP, ER, HELS, JUMIN, JUSO, DEPTCODE, IPDOPD, SMS_DETAIL, BDATE, SEX, AGE,BI, WARD, ROOM, SPECCODE, SPECNM, REMIND, GB_GWAEXAM };
        
        public enum enmSel_EXAM_RESULTC           {  STATUS_VIEW ,   STATUS,   STATUS_OLD,   EXAMNAME,        F ,       RESULT, RESULT_BEFOR,      UNIT,        CV,     PANIC,     DELTA,     REFER,       CHK,       MID,      CHK_IMG, CHK_RE_RESULT,   RE_RESULT_CASE_CD,   RE_RESULT_CASE, RE_RESULT_INPT,   RE_RESULT_DT,    RE_RESULT,      TAT_DT,         TAT_INPT,          TAT_CASE_CD,       TAT_CASE,   DELTAM,   DELTAP,  DDDPRDRP,    SUBCODE,  RESULTWS,   ROWID_R,    SEQNO,   FOOTNOTE,     CHANG, RESULTDATE, RESULT_BEFER_DATE, MASTERCODE,   PENDING,   RESULTIN,   RESULT_OLD,   FOOTNOTE_OLD,   RESULTSABUN,      HCODE,   IMGWRTNO,   CV_OLD,   CHK_MODIFY,   PANO,    SNAME,   WSCODE1,   EX_CR42A,  SPECNO , EQUCODE_INTER };
        public string[] sSel_EXAM_RESULTC       = {        "상태",  "STATUS",    "STATUS", "검사항목",       "F",       "결과",     "전결과",    "단위",       "C",       "P",       "D",       "R",    "제외",    "배양",       "스캔",        "재검",          "재검사유",       "재검비고", "재검전입력자",   "재검전일시", "재검전결과",  "TAT_범위", "TAT_오류입력자",      "TAT_오류유형", "TAT_오류비고", "DELTAM", "DELTAP", "DDDPRDRP", "검사코드",      "WS",  "ROWID_R", "SEQNO", "FOOTNOTE",    "변경", "결과일시",      "전결과일시",   "모코드", "PENDING", "RESULTIN", "RESULT_OLD", "FOOTNOTE_OLD", "RESULTSABUN", "HELPCODE", "IMGWRTNO", "CV_OLD", "CHK_MODIFY", "PANO",  "SNAME", "WSCODE1", "EX_CR42A", "SPECNO",         "장비"};
        public int[] nSel_EXAM_RESULTC          = {     nCol_SCHK,         5,           5,   nCol_TEL, nCol_SCHK,  nCol_TEL+20,    nCol_CHEK, nCol_CHEK, nCol_SCHK, nCol_SCHK, nCol_SCHK, nCol_SCHK, nCol_SCHK, nCol_CHEK, nCol_SCHK+10,     nCol_SCHK, nCol_ORDERNAME + 30,       nCol_TIME ,      nCol_CHEK,     nCol_JUMN1,    nCol_CHEK,   nCol_CHEK,        nCol_CHEK, nCol_ORDERNAME + 30,      nCol_TIME,        5,        5,          5,  nCol_CHEK, nCol_SCHK,          5,       5,          5, nCol_SCHK,  nCol_PANO,         nCol_PANO,  nCol_CHEK,         5,          5,            5,              5,             5,  nCol_CHEK,          5,        5,            5,      5,        5,         5,          5,       5 ,     nCol_CHEK };
        
        public enum enmSel_EXAM_MASTER_SUB        {                NAME,   SORT,  NORMAL    };
        public string[] sSel_EXAM_MASTER_SUB    = {              "Help", "SORT", "NORMAL"   };
        public int[] nSel_EXAM_MASTER_SUB       = { nCol_ORDERNAME + 20,      5, nCol_AGE   };

        public enum enmSel_EXAM_RESULTC_HIS       {     SPECNO,   SPEC_NM, RECEIVEDATE,   RESULT      };
        public string[] sSel_EXAM_RESULTC_HIS   = { "검체번호",  "검체명", "접수일자" ,    "결과"     };
        public int[] nSel_EXAM_RESULTC_HIS      = {  nCol_PANO, nCol_PANO,   nCol_PANO, nCol_TIME     };

        public enum enmSel_EXAM_BLOOD_MASTER      { ABO_N, RH, SERUM, ROWID };

        public enum enmSel_EXAM_RESULTC_TG     {          PANO,         SNAME,   DEPTCODE,   DRNAME,        BI,    IPDOPD,      BDATE, RESULTDATE,    RESULT,     SPECNO,    REMARK };
        public string[] sSel_EXAM_RESULTC_TG = {    "등록번호",        "성명",       "과",   "의사",    "자격",     "I/O", "처방일자", "결과일자",    "결과", "검체번호",    "비고" };
        public int[] nSel_EXAM_RESULTC_TG    = { nCol_CHEK + 5, nCol_CHEK + 5, nCol_SCHK, nCol_CHEK, nCol_CHEK, nCol_CHEK,  nCol_TIME,  nCol_TIME, nCol_CHEK,  nCol_CHEK, nCol_TIME };

        public enum enmSel_EXAM_HR_LIST        {   SEQ,      SNAME,     GUBUN,       BIRTH,         DATE1,               DATE2,               DATE3,          DATE4,     SPECNO,   BDATE,     PANO  };
        public string[] sSel_EXAM_HR_LIST    = { "SEQ",     "성명",    "검체",  "생년월일",    "채혈일시",      "원심분리시간",      "검체접수시간", "검사완료시간", "검체번호", "BDATE",    "PANO" };
        public int[] nSel_EXAM_HR_LIST       = {     5, nCol_SNAME, nCol_CHEK,   nCol_PANO,  nCol_PANO+20,      nCol_PANO + 20,      nCol_PANO + 20,   nCol_PANO+20,  nCol_PANO,       5,    5 };

        public enum enmSel_EXAM_RESULTC_RE        { RESULTDATE,       PANO,     SPECNO,       SEX,           AGE,      SNAME,  DEPTCODE,     WARD,     EXAM_NAME,   RESULTWS,    RE_RESULT, RE_RESULT_CASE_CD, RE_RESULT_CASE,     RESULT, RESULTSABUN,    SUBCODE, RE_RESULT_TO_DO,   ROWID_R, CHK   };
        public string[] sSel_EXAM_RESULTC_RE    = { "전송일자", "등록번호", "검체번호",    "성별",        "나이", "환자성명",      "과",   "병동",    "검사명칭",       "WS", "재검전결과",        "재검사유", "재검사유비고", "최종결과","최종검사자", "검사코드",      "처리사항", "ROWID_R", "적용" };
        public int[] nSel_EXAM_RESULTC_RE       = {  nCol_PANO,  nCol_PANO,  nCol_PANO, nCol_SCHK, nCol_SCHK + 5, nCol_SNAME,  nCol_AGE, nCol_AGE,  nCol_NAME-10, nCol_AGE+5,     nCol_TEL,      nCol_NAME+50,      nCol_PANO,   nCol_TEL,    nCol_TEL,         0 ,       nCol_NAME,         5, nCol_SCHK };

        public enum enmSel_EXAM_RESULTC_BLOOD     {      SPECNO,       PANO,       SEX,           AGE,      SNAME, EXAM_NAME,    RESULT };
        public string[] sSel_EXAM_RESULTC_BLOOD = {  "검체번호", "등록번호",    "성별",        "나이", "환자성명",  "검사명", "혈액번호"};
        public int[] nSel_EXAM_RESULTC_BLOOD    = {   nCol_PANO,  nCol_PANO, nCol_SCHK, nCol_SCHK + 5, nCol_SNAME, nCol_TIME, nCol_SNAME};


        public DataSet sel_EXAM_RESULTC_BLOOD(PsmhDb pDbCon, string strPANO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT A.SPECNO                                                     \r\n";
            SQL += "       , A.PANO                                                       \r\n";
            SQL += "       , A.SEX                                                        \r\n";
            SQL += "       , A.AGE                                                        \r\n";
            SQL += "       , A.SNAME                                                      \r\n";            
            SQL += "       , KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(B.SUBCODE) AS EXAM_NAME   \r\n";
            SQL += "       , B.RESULT                                                     \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_SPECMST A                                    \r\n";
            SQL += "       , KOSMOS_OCS.EXAM_RESULTC B                                    \r\n";
            SQL += "  WHERE 1=1                                                           \r\n";
            SQL += "    AND A.SPECNO = B.SPECNO                                           \r\n";
            SQL += "    AND A.PANO   = " + ComFunc.covSqlstr(strPANO, false);
            //2019-05-14 안정수, 표가영S 요청으로 처방날짜 -> 실제 불출날짜로 조건변경
            //SQL += "    AND A.BDATE BETWEEN TRUNC(SYSDATE) - 3                            \r\n";
            //SQL += "                    AND TRUNC(SYSDATE)                                \r\n";
            SQL += "    AND A.RESULTDATE BETWEEN TRUNC(SYSDATE) - 3                       \r\n";
            SQL += "                         AND SYSDATE                                  \r\n";
            SQL += "    AND A.STATUS = '05'                                               \r\n"; 
            SQL += "    AND B.SUBCODE IN ('BT02','BT02A','BT021','BT02B','BT022','BT023') \r\n";
            SQL += "  ORDER BY B.SPECNO    \r\n";

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

        public string up_EXAM_RESULTC_TO_DO(PsmhDb pDbCon, string strRowId,string strRE_RESULT_TO_DO, string strRE_RESULT_CASE_CD, string strRE_RESULT_CASE, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_RESULTC  \r\n";
            SQL += "     SET RE_RESULT_TO_DO        = " + ComFunc.covSqlstr(strRE_RESULT_TO_DO, false);
            SQL += "       , RE_RESULT_CASE_CD   = " + ComFunc.covSqlstr(strRE_RESULT_CASE_CD, false);
            SQL += "       , RE_RESULT_CASE      = " + ComFunc.covSqlstr(strRE_RESULT_CASE, false);
            SQL += "   WHERE ROWID              = " + ComFunc.covSqlstr(strRowId, false);
             

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

        public DataSet sel_EXAM_RESULTC_RE(PsmhDb pDbCon, string strFDATE, string strTDATE, string strWS)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT A.RESULTDATE                                                                                                           \r\n";
            SQL += "       , A.PANO                                                                                                                 \r\n";
            //2018-07-16 안정수, 여경은s 요청으로 검체번호 추가
            SQL += "       , A.SPECNO                                                                                                               \r\n";
            SQL += "       , A.SEX                                                                                                                  \r\n";
            SQL += "       , A.AGE                                                                                                                  \r\n";
            SQL += "       , A.SNAME                                                                                                                \r\n";
            SQL += "       , A.DEPTCODE                                                                                                             \r\n";
            SQL += "       , A.WARD                                                                                                                 \r\n";
            
            SQL += "       , KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(B.SUBCODE) AS EXAM_NAME                                                             \r\n";
            SQL += "       , B.RESULTWS                                                                                                             \r\n";
            SQL += "       , B.RE_RESULT                                                                                                            \r\n";
            SQL += "       , B.RE_RESULT_CASE_CD || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAME('EXAM_RE_RESULT',B.RE_RESULT_CASE_CD) AS RE_RESULT_CASE_CD  \r\n";
            SQL += "       , B.RE_RESULT_CASE                                                                                                       \r\n";
            SQL += "       , B.RESULT                                                                                                               \r\n";
            SQL += "       , KOSMOS_OCS.FC_BAS_USER(B.RESULTSABUN)  AS RESULTSABUN                                                                  \r\n";
            SQL += "       , B.SUBCODE                                                                                                              \r\n";
            SQL += "       , B.RE_RESULT_TO_DO                                                                                                      \r\n";
            SQL += "       , B.ROWID                                AS ROWID_R                                                                      \r\n";
            SQL += "       , ''                                     AS CHK                                                                          \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_SPECMST A                                                                                              \r\n";
            SQL += "       , KOSMOS_OCS.EXAM_RESULTC B                                                                                              \r\n";
            SQL += "  WHERE 1=1                                                                                                                     \r\n";
            SQL += "    AND A.SPECNO = B.SPECNO                                                                                                     \r\n";
            SQL += "    AND A.RESULTDATE BETWEEN TO_DATE('" + strFDATE + " 00:01', 'YYYY-MM-DD HH24:MI')                                            \r\n";
            SQL += "                         AND TO_DATE('" + strTDATE + " 23:59', 'YYYY-MM-DD HH24:MI')                                            \r\n";
            SQL += "    AND B.RE_RESULT_CASE_CD IS NOT NULL                                                                                         \r\n";

            if (strWS.IndexOf("*") == -1)
            {
                SQL += "    AND B.RESULTWS = " + ComFunc.covSqlstr(strWS, false);
            }

            
            SQL += "  ORDER BY A.RESULTDATE, B.RESULTWS, B.RE_RESULT_CASE_CD, B.SPECNO                                                              \r\n";
                                                                                                                                                    
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

        public string ins_EXAM_HR_LIST(PsmhDb pDbCon, DataRow dr, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  INSERT INTO KOSMOS_OCS.EXAM_HR_LIST( SNAME, BIRTH, GUBUN, DATE1, DATE2, DATE3, DATE4, SPECNO, BDATE, PANO) VALUES (  \r\n";
            SQL += "   " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HR_LIST.SNAME].ToString().Trim(), false);
            SQL += "   " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HR_LIST.BIRTH].ToString().Trim(), true);
            SQL += "   " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HR_LIST.GUBUN].ToString().Trim(), true);
            SQL += "   " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HR_LIST.DATE1].ToString().Trim(), true);
            SQL += "   " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HR_LIST.DATE2].ToString().Trim(), true);
            SQL += "   " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HR_LIST.DATE3].ToString().Trim(), true);
            SQL += "   " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HR_LIST.DATE4].ToString().Trim(), true);
            SQL += "   " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HR_LIST.SPECNO].ToString().Trim(), true);
            SQL += "   " + ComFunc.covSqlDate(dr[(int)enmSel_EXAM_HR_LIST.BDATE].ToString().Trim(), true);
            SQL += "   " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_HR_LIST.PANO].ToString().Trim(), true);
            SQL += "                                                                                                                    )    \r\n";

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

        public string del_EXAM_HR_LIST(PsmhDb pDbCon, string strBDATE, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  DELETE KOSMOS_OCS.EXAM_HR_LIST    \r\n";
            SQL += "   WHERE BDATE     = " + ComFunc.covSqlDate(strBDATE, false);

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

        public DataSet sel_EXAM_HR_LIST(PsmhDb pDbCon, string strBDATE, string strSPECODE)
        {
            DataSet ds = null;

            string strSPECNO_YYMM = Convert.ToDateTime(strBDATE).ToString("yyMMdd");
            string strBOTTOM = Convert.ToDateTime(strBDATE).ToString("yyyy") + "년 " + Convert.ToDateTime(strBDATE).ToString("MM") + "월 " + Convert.ToDateTime(strBDATE).ToString("dd") + "일";

            SQL = "";

            SQL += "   SELECT                                                                                                                          \r\n";                 
            SQL += "           0											AS SEQ                                                                     \r\n";
            SQL += "          ,  A.SNAME                     				AS SNAME 	-- 02                                                          \r\n";
            SQL += "   		, CASE WHEN A.SPECCODE = '013' THEN '혈청'                                                                                 \r\n";
            SQL += "   		       WHEN A.SPECCODE = '011' THEN '전혈'                                                                                 \r\n";
            SQL += "  			   WHEN A.SPECCODE = '084' THEN '대변'                                                                                 \r\n";
            SQL += "  	      END 						              	AS GUBUN	-- 03                                                              \r\n";
            SQL += "  	    , KOSMOS_OCS.FC_BAS_PATIENT_BIRTH(A.PANO) 	AS BIRTH    -- 04                                                              \r\n";
            SQL += "  		, TO_CHAR(A.BLOODDATE	, 'MM/DD HH24:MI') 	AS DATE1	-- 05                                                              \r\n";
            SQL += "  		, CASE WHEN A.SPECCODE = '013' AND NVL(TRIM(B.DATE2),'^&') = '^&' THEN  TO_CHAR(A.BLOODDATE	, 'MM/DD')					   \r\n";
            SQL += "  		       ELSE B.DATE2 END 					AS DATE2	-- 06                                                              \r\n";
            SQL += "  		, TO_CHAR(A.RECEIVEDATE	, 'MM/DD HH24:MI') 	AS DATE3    -- 07                                                              \r\n";
            SQL += "  		, TO_CHAR(A.RESULTDATE	, 'MM/DD HH24:MI') 	AS DATE4    -- 08                                                              \r\n";
            SQL += "  		, A.SPECNO                                  AS SPECNO   -- 09                                                              \r\n";
            SQL += "  		, TO_CHAR(A.BDATE		, 'YYYY-MM-DD')		AS BDATE	-- 10                                                              \r\n";
            SQL += "   		, A.PANO                                    AS PANO		-- 11                                                              \r\n";
            SQL += "                                                                                                                                   \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_SPECMST A                                                                                                  \r\n";
            SQL += "      , KOSMOS_OCS.EXAM_HR_LIST B                                                                                                  \r\n";
            SQL += "   WHERE 1=1                                                                                                                       \r\n";

            SQL += "     AND A.BDate    = " + ComFunc.covSqlDate(strBDATE, false);

            if (strSPECODE.Trim().Equals("*") == true)
            {
                SQL += "     AND A.SPECCODE IN ('011','013','084')                                                                                     \r\n";
            }
            else
            {
                SQL += "     AND A.SPECCODE = " + ComFunc.covSqlstr(strSPECODE, false);
            }
            
            SQL += "     AND A.SPECNO BETWEEN '" + strSPECNO_YYMM + "'  || '7000'                                                                      \r\n";
            SQL += "                      AND '" + strSPECNO_YYMM + "'  || '9999'                                                                      \r\n";
            SQL += "     AND A.CANCEL IS NULL                                                                                                          \r\n";
            SQL += "     AND A.SPECNO = B.SPECNO (+)                                                                                                   \r\n";
            SQL += "   UNION                                                                                                                           \r\n";
            SQL += "   SELECT                                                                                                                          \r\n";
            SQL += "            1		AS SEQ                                                                                                         \r\n";
            SQL += "          , '"+ strBOTTOM + "' || CHR(13) || CHR(10) || CHR(13) || CHR(10) || '인계자(출장검진담당자) :               (서명) '                          \r\n";
            SQL += "                               || CHR(13) || CHR(10) || CHR(13) || CHR(10) ||'인계자(검사실담당자) :                (서명) '	AS SNAME 	-- 02      \r\n";
            SQL += "   		, '' 	AS GUBUN	-- 03                                                                                                                                                                          \r\n";
            SQL += "   		, ''	AS BIRTH    -- 04                                                                                                                                                                          \r\n";
            SQL += "  		, ''	AS DATE1	-- 05                                                                                                                                                                          \r\n";
            SQL += "  		, ''	AS DATE2	-- 06                                                                                                                                                                          \r\n";
            SQL += "  		, ''	AS DATE3    -- 07                                                                                                                                                                          \r\n";
            SQL += "  		, '' 	AS DATE4    -- 08                                                                                                                                                                          \r\n";
            SQL += "  		, ''    AS SPECNO   -- 09                                                                                                                                                                          \r\n";
            SQL += "  		, ''	AS BDATE	-- 10                                                                                                                                                                          \r\n";
            SQL += "   		, ''    AS PANO		-- 11                                                                                                                                                                          \r\n";
            SQL += "   FROM DUAL                                                                                                                                                                                               \r\n";
            SQL += "   ORDER BY SEQ, DATE1, PANO, GUBUN                                                                                                                                                                        \r\n";
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

        public DataSet sel_EXAM_RESULTC_TG(PsmhDb pDbCon, string strFDATE, string strTDATE)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                             \r\n";
            SQL += " 	   A.PANO                                                       \r\n";
            SQL += " 	,  B.SNAME                                                      \r\n";
            SQL += " 	,  B.DEPTCODE                                                   \r\n";
            SQL += " 	,  C.DRNAME                                                     \r\n";
            SQL += " 	,  B.BI                                                         \r\n";
            SQL += " 	,  B.IPDOPD                                                     \r\n";
            SQL += " 	,  TO_CHAR(B.BDATE,'YYYY-MM-DD') BDATE                          \r\n";
            SQL += " 	,  TO_CHAR(A.RESULTDATE,'YYYY-MM-DD HH24:MI') RESULTDATE        \r\n";
            SQL += " 	,  RESULT                                                       \r\n";
            SQL += " 	,  A.SPECNO                                                     \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_RESULTC A                                   \r\n";
            SQL += "   	,   KOSMOS_OCS.EXAM_SPECMST B                                   \r\n";
            SQL += "   	,  	KOSMOS_PMPA.BAS_DOCTOR C                                    \r\n";
            SQL += "  WHERE A.RESULTDATE BETWEEN TO_DATE('" + strFDATE + " 00:01', 'YYYY-MM-DD HH24:MI')                                            \r\n";
            SQL += "                         AND TO_DATE('" + strTDATE + " 23:59', 'YYYY-MM-DD HH24:MI')                                            \r\n";
            SQL += "    AND B.STATUS IN ('04', '05')                                    \r\n";
            SQL += "    AND A.MASTERCODE IN( 'CR39', 'CR39A' )                          \r\n";
            SQL += "    AND CASE WHEN KOSMOS_OCS.FC_IS_NUMBER(A.RESULT) = 1 THEN TO_NUMBER(A.RESULT) ELSE 0 END >= 400                                           \r\n";
            SQL += "    AND B.DEPTCODE IN ('HR','TO')                                   \r\n";
            SQL += "    AND B.SPECNO = A.SPECNO(+)                                      \r\n";
            SQL += "    AND B.DRCODE = C.DRCODE(+)                                      \r\n";
            SQL += "  ORDER BY A.RESULTDATE, A.PANO                                     \r\n";

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

        public string up_EXAM_RESULTC(PsmhDb pDbCon, string strROWID_R, string strRESULT, string strOLDRESULT, string strSTATUS
                                                   , string strRESULT_BEFOR, string strDELTAM, string strDELTAP, string strDDDPRDRP
                                                   , string strRESULT_BEFER_DATE, string strSEX, string strAGE
                                                   , string strHCODE
                                                   , string strCV
                                                   , string strRESULTDATE
                                                   , string strSABUN
                                                   , ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            if (string.IsNullOrEmpty(strRESULT_BEFER_DATE.Trim()) == false )
            {
                strRESULT_BEFER_DATE = Convert.ToDateTime(strRESULT_BEFER_DATE).ToString("yyyy-MM-dd");
            }

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_RESULTC            \r\n";
            SQL += "     SET RESULT = " + ComFunc.covSqlstr(strRESULT, false);
            SQL += "       , STATUS = " + ComFunc.covSqlstr(strSTATUS, false);
            SQL += "       , DELTA  = KOSMOS_OCS.FC_EXAM_RESULTC_DELTA ('" + strRESULT.Trim() + "','" + strRESULT_BEFOR.Trim() + "','" + strDELTAM.Trim() + "','" + strDELTAP.Trim() + "','" + strDDDPRDRP.Trim() + "','" + strRESULT_BEFER_DATE.Trim() + "')   \r\n";
            SQL += "       , PANIC  = KOSMOS_OCS.FC_EXAM_MASTER_PANIC  ( SUBCODE,'" + strRESULT.Trim() + "')                                                                                                                                                    \r\n";
            SQL += "       , CV     = " + ComFunc.covSqlstr(strCV, false);
            SQL += "       , REFER  = KOSMOS_OCS.FC_EXAM_MASTER_SUB_REF('1', SUBCODE,'" + strSEX.Trim() + "','" + strAGE.Trim() + "','" + strRESULT.Trim() + "')                                                                                                \r\n";
            SQL += "       , HCODE  = " + ComFunc.covSqlstr(strHCODE, false);

            if (strRESULT.Trim() != strOLDRESULT.Trim() && string.IsNullOrEmpty(strRESULT.Trim()) == false )
            {
                // 값을 변경한 경우
                SQL += "       , RESULTDATE   = SYSDATE      \r\n";
                SQL += "       , RESULTSABUN  = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            }
            else if (string.IsNullOrEmpty(strRESULT.Trim()) == false && string.IsNullOrEmpty(strOLDRESULT.Trim()) == true)
            {
                // 결과있음 / 처음
                SQL += "       , RESULTDATE   = SYSDATE      \r\n";
                SQL += "       , RESULTSABUN  = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            }
            else if (string.IsNullOrEmpty(strRESULT.Trim()) == false && (string.IsNullOrEmpty(strSABUN.Trim()) == true || string.IsNullOrEmpty(strRESULTDATE.Trim()) == true))
            {
                // 결과 입력 / 사번도 없고, 결과입력도 없고
                SQL += "       , RESULTDATE   = SYSDATE      \r\n";
                SQL += "       , RESULTSABUN  = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            }
            else
            {
                if (string.IsNullOrEmpty(strRESULT.Trim()) == true && strSTATUS.Equals("V") == false)
                {
                    SQL += "       , RESULTDATE   = ''      \r\n";
                }
            }

            SQL += "       , UPPS   = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += "       , UPDT   = SYSDATE   \r\n";
            SQL += "   WHERE ROWID  = " + ComFunc.covSqlstr(strROWID_R, false);

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

        public string up_EXAM_RESULTC_RE(PsmhDb pDbCon, string strSPECNO, string strSEQNO, string strRE_RESULT_CASE_CD, string strRE_RESULT_INPT, string strRE_RESULT_CASE, string strRE_RESULT_DT, string strRE_RESULT, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_RESULTC        \r\n";
            SQL += "     SET RE_RESULT_CASE_CD     = " + ComFunc.covSqlstr(strRE_RESULT_CASE_CD , false);
            SQL += "       , RE_RESULT_INPT        = " + ComFunc.covSqlstr(strRE_RESULT_INPT    , false);
            SQL += "       , RE_RESULT_CASE        = " + ComFunc.covSqlstr(strRE_RESULT_CASE    , false);
            SQL += "       , RE_RESULT_DT          = TO_DATE('" + strRE_RESULT_DT + "', 'YYYY-MM-DD HH24:MI')   \r\n";
            SQL += "       , RE_RESULT             = " + ComFunc.covSqlstr(strRE_RESULT         , false);
            SQL += "       , STATUS                = ''                                                    \r\n";
            SQL += "       , RESULT                = ''                                                    \r\n";
            SQL += "       , RESULTDATE            = ''      \r\n";
            SQL += "       , UPPS   = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += "       , UPDT   = SYSDATE   \r\n";
            SQL += "   WHERE SPECNO = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "     AND SEQNO  = " + ComFunc.covSqlstr(strSEQNO, false);

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

        public string up_EXAM_SPECMST_02(PsmhDb pDbCon, string strSPECNO, string strSTATUS, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_SPECMST    \r\n";
            SQL += "     SET RESULTDATE =   ''          \r\n";
            SQL += "      ,  STATUS     =   " + ComFunc.covSqlstr(strSTATUS, false);
            SQL += "      ,  EMR        =   '0'         \r\n";
            SQL += "   WHERE SPECNO     = " + ComFunc.covSqlstr(strSPECNO, false);

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

        public string sel_EXAM_PATIENT(PsmhDb pDbCon, string strPANO)
        {

            string s = "";
            DataTable dt = null;

            SQL = "";
            SQL += "  SELECT                                    \r\n";
            SQL += "         REMIND                 		    \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_PATIENT A     \r\n";
            SQL += "   WHERE 1=1                                \r\n";
            SQL += "     AND A.PANO = " + ComFunc.covSqlstr(strPANO, false);

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    s = dt.Rows[0][0].ToString();
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }

            return s;
        }

        public string save_EXAM_PATIENT(PsmhDb pDbCon, string strPANO, string strSODIUM, string strHEPARIN, string strREMIND, bool isREMIND, ref int intRowAffected, ref string SQL)
        {

            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  MERGE INTO KOSMOS_OCS.EXAM_PATIENT  A                                     \r\n";
                SQL += "       USING DUAL                                                           \r\n";
                SQL += "  	    ON (                                                                \r\n";
                SQL += "  	    	    A.PANO   = " + ComFunc.covSqlstr(strPANO, false);
                SQL += "  	        )                                                               \r\n";
                SQL += "  		WHEN     MATCHED THEN                                               \r\n";
                SQL += "  		UPDATE                                                              \r\n";

                if (isREMIND == true)
                {
                    SQL += "  	       SET REMIND         = " + ComFunc.covSqlstr(strREMIND, false);
                }
                else
                {
                    SQL += "  	       SET SODIUM         = " + ComFunc.covSqlstr(strSODIUM, false);
                    SQL += "             , HEPARIN        = " + ComFunc.covSqlstr(strHEPARIN, false);
                }

                SQL += "             , UPPS       = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
                SQL += "             , UPDT       = SYSDATE \r\n";


                SQL += "  		WHEN NOT MATCHED THEN                                               \r\n";
                SQL += "            INSERT  ( PANO , SODIUM, HEPARIN                                \r\n";
                SQL += "                , REMIND                                                    \r\n";               
                SQL += "                    , INPS                                                  \r\n";
                SQL += "                    , INPT_DT                                               \r\n";
                SQL += "                    , UPPS                                                  \r\n";
                SQL += "                    , UPDT                                                  \r\n";
                SQL += "              )                                                             \r\n";
                SQL += "              VALUES (                                                      \r\n";
                SQL += "              " + ComFunc.covSqlstr(strPANO, false);
                SQL += "              " + ComFunc.covSqlstr(strSODIUM, true);
                SQL += "              " + ComFunc.covSqlstr(strHEPARIN, true);
                SQL += "              " + ComFunc.covSqlstr(strREMIND, true);
                SQL += "              " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
                SQL += "             , SYSDATE \r\n";
                SQL += "              " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
                SQL += "             , SYSDATE \r\n";
                SQL += "              )                                                             \r\n";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }
            return SqlErr;

        }



        public string save_EXAM_BLOOD_MASTER(PsmhDb pDbCon, string strPANO, string strSNAME, string strSEX, string strAGE, string strWARD, string strROOM, string strBI, string strSqlTmp1, string strSqlTmp2, string strSqlTmp3, ref int intRowAffected, ref string SQL)
        {

            string SqlErr = "";

            try
            {
                SQL = "";
                SQL += "  MERGE INTO KOSMOS_OCS.EXAM_BLOOD_MASTER  A                                \r\n";
                SQL += "       USING DUAL                                                           \r\n";
                SQL += "  	    ON (                                                                \r\n";
                SQL += "  	    	    A.PANO   = " + ComFunc.covSqlstr(strPANO, false);
                SQL += "  	        )                                                               \r\n";
                SQL += "  		WHEN     MATCHED THEN                                               \r\n";
                SQL += "  		UPDATE                                                              \r\n";
                SQL += "  	       SET AGE         = " + ComFunc.covSqlstr(strAGE, false);
                SQL += "             , WARD        = " + ComFunc.covSqlstr(strWARD, false);
                SQL += "             , ROOM        = " + ComFunc.covSqlstr(strROOM, false);
                SQL += "             , BI          = " + ComFunc.covSqlstr(strBI, false);
                SQL += "             , MODIFYDATE  = SYSDATE                                        \r\n";
                SQL += "             , SABUN       = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);

                SQL += "             , UPPS       = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
                SQL += "             , UPDT_DT    = SYSDATE \r\n";

                SQL += "            , " + strSqlTmp1;

                SQL += "  		WHEN NOT MATCHED THEN                                               \r\n";
                SQL += "            INSERT  ( PANO,SNAME,BI,AGE,SEX,WARD,ROOM,MODIFYDATE,SABUN      \r\n";
                SQL += "                    , " + strSqlTmp2 + "                                       \r\n";

                SQL += "                    , INPS         \r\n";
                SQL += "                    , INPT_DT         \r\n";
                SQL += "                    , UPPS         \r\n";
                SQL += "                    , UPDT_DT         \r\n";

                SQL += "              )                                                             \r\n";
                SQL += "              VALUES (                                                      \r\n";
                SQL += "              " + ComFunc.covSqlstr(strPANO, false);
                SQL += "              " + ComFunc.covSqlstr(strSNAME, true);
                SQL += "              " + ComFunc.covSqlstr(strBI, true);
                SQL += "              " + ComFunc.covSqlstr(strAGE, true);
                SQL += "              " + ComFunc.covSqlstr(strSEX, true);
                SQL += "              " + ComFunc.covSqlstr(strWARD, true);
                SQL += "              " + ComFunc.covSqlstr(strROOM, true);
                SQL += "              , SYSDATE                                 \r\n";
                SQL += "              " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
                SQL += "              , " + strSqlTmp3;

                SQL += "              " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
                SQL += "             , SYSDATE \r\n";
                SQL += "              " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
                SQL += "             , SYSDATE \r\n";
                SQL += "              )                                                             \r\n";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }
            return SqlErr;

        }
            
        public DataTable sel_EXAM_BLOOD_MASTER(PsmhDb pDbCon, string strPANO)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "  SELECT                                    \r\n";
            SQL += "         ABO_N, RH, SERUM, ROWID		    \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_BLOOD_MASTER A     \r\n";
            SQL += "   WHERE 1=1                                \r\n";
            SQL += "     AND A.PANO = " + ComFunc.covSqlstr(strPANO, false);

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

        public string ins_EXAM_HISRESULTC(PsmhDb pDbCon, string strROWID_R, string strHisJob, string strSAYU, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  INSERT INTO KOSMOS_OCS.EXAM_HISRESULTC (JOBDATE, JOBGBN, JOBSABUN, SPECNO, RESULTWS, EQUCODE,                         \r\n";
            SQL += "                                        SEQNO, PANO, MASTERCODE, SUBCODE, STATUS, RESULT, RESULTDATE, RESULTSABUN,      \r\n";
            SQL += "                                        REFER, PANIC, DELTA, UNIT,HCODE, CV, SAYU                                   )   \r\n";
            SQL += "  SELECT SYSDATE,'" +  strHisJob + "'," + clsType.User.IdNumber + ",SPECNO,RESULTWS,EQUCODE,                            \r\n";
            SQL += "          SEQNO, PANO, MASTERCODE, SUBCODE, STATUS, RESULT, RESULTDATE, RESULTSABUN,                                     \r\n";            
            SQL += "          REFER, PANIC, DELTA, UNIT, HCODE, CV, '" + strSAYU + "'                                                       \r\n";

            SQL += "    FROM KOSMOS_OCS.EXAM_RESULTC   \r\n";
            SQL += "   WHERE ROWID = " + ComFunc.covSqlstr(strROWID_R, false);

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

        public string ins_EXAM_HISRESULTC(PsmhDb pDbCon, string strSPECNO, bool isVerify, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  INSERT INTO KOSMOS_OCS.EXAM_HISRESULTC (JOBDATE,JOBGBN,JOBSABUN,SPECNO,     \r\n";
            SQL += "                                        PANO,RESULT,RESULTDATE,PRINT)       \r\n";
            SQL += "  SELECT  SYSDATE,'3', " + clsType.User.IdNumber + ", SPECNO,                 \r\n";

            if (isVerify == false)
            {
                SQL += "          PANO,'결과완료를 임시저장으로 변경',RESULTDATE,PRINT                \r\n";
            }
            else
            {
                SQL += "          PANO,'결과완료분 다시 전송함',RESULTDATE,PRINT                \r\n";
            }
            
            SQL += "    FROM KOSMOS_OCS.EXAM_SPECMST   \r\n";
            SQL += "   WHERE SPECNO = " + ComFunc.covSqlstr(strSPECNO, false);
         
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

        public string sel_EXAM_SPECMST_STATUS(PsmhDb pDbCon, string strSPECNO)
        {
            string s = "";

            DataTable dt = null;

            SQL = "";
            SQL += " SELECT STATUS \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_SPECMST S     \r\n";
            SQL += "  WHERE S.SPECNO = " + ComFunc.covSqlstr(strSPECNO, false);

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    s = dt.Rows[0][0].ToString();
                    return s;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }


            return s;

        }

        public string sel_EXAM_RESULT_PANIC_DELTA(PsmhDb pDbCon, bool isPANIC, string strSUBCODE, string strRESULT, string strRESULT_BEFOR, string strRESULT_BEFER_DATE, ref string strPANIC, ref string strDELTA, ref string strCV)
        {
            string s = "";

            DataTable dt = null;


            if (string.IsNullOrEmpty(strRESULT_BEFER_DATE.Trim()) == false)
            {
                strRESULT_BEFER_DATE = Convert.ToDateTime(strRESULT_BEFER_DATE).ToString("yyyy-MM-dd");
            }
            

            SQL = "";
            SQL += "  SELECT KOSMOS_OCS.FC_EXAM_MASTER_PANIC('" + strSUBCODE + "','" + strRESULT + "') AS PANIC                                       \r\n";
            SQL += "       , KOSMOS_OCS.FC_EXAM_MASTER_CV('" + strSUBCODE + "','" + strRESULT + "') AS CV                                       \r\n";
            SQL += "       , (                                                                                              \r\n";
            SQL += "  	       SELECT KOSMOS_OCS.FC_EXAM_RESULTC_DELTA('" + strRESULT + "','" + strRESULT_BEFOR + "', DELTAM,DELTAP,DDDPRDRP, '"+ strRESULT_BEFER_DATE +"')    \r\n";
            SQL += "  	         FROM KOSMOS_OCS.EXAM_MASTER                                                                           \r\n";
            SQL += "  	        WHERE MASTERCODE = '" + strSUBCODE + "'                                                     \r\n";
            SQL += "         )												AS DELTA                                        \r\n";
            SQL += "    FROM DUAL                                                                                           \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    if (isPANIC == true)
                    {
                        strPANIC = dt.Rows[0]["PANIC"].ToString();
                        strCV= dt.Rows[0]["CV"].ToString();
                    }
                    else
                    {
                        strDELTA = dt.Rows[0]["DELTA"].ToString();
                    }

                    return s;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }


            return s;

        }

        public DataTable sel_EXAM_SPECMST_CURR(PsmhDb pDbCon, string strSPECNO)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "  SELECT                                                                 \r\n";
            SQL += "         S.SPECNO											AS SPECNO    \r\n";
            SQL += "       , S.PANO                                             AS PANO      \r\n";
            SQL += "       , P.SNAME                                            AS SNAME     \r\n";
            SQL += "       , KOSMOS_OCS.FC_EXAM_PATIENT_2(S.PANO,'S') 			AS SODIUM    \r\n";
            SQL += "       , KOSMOS_OCS.FC_EXAM_PATIENT_2(S.PANO,'H') 			AS HEPARIN   \r\n";
            SQL += "       , S.STATUS                                           AS STATUS    \r\n";
            SQL += "       , TO_CHAR(RECEIVEDATE, 'YYYY-MM-DD HH24:MI') 		AS RDATE     \r\n";
            SQL += "       , S.DRCOMMENT                                 		AS DRCOMMENT \r\n";
            SQL += "       , TO_CHAR(BLOODDATE, 'YYYY-MM-DD HH24:MI') 			AS BLOODDATE \r\n";
            SQL += "       , (                                                               \r\n";
            SQL += "        	 SELECT CASE WHEN SUM(QTY * NAL) <= 0 THEN 'Y'               \r\n";
            SQL += "        	              ELSE '' END                                    \r\n";
            SQL += "             FROM KOSMOS_OCS.OCS_IORDER                                  \r\n";
            SQL += "            WHERE PTNO 	= S.PANO                                         \r\n";
            SQL += "              AND BDATE 	= S.BDATE                                    \r\n";
            SQL += "              AND ORDERNO = S.ORDERNO                                    \r\n";
            SQL += "  		)                                                   AS DC_I      \r\n";
            SQL += "       , DECODE(S.STRT,'S','[응급]','')						AS STRT	 -- 응급        \r\n";
            SQL += "       , (                                                              \r\n";
            SQL += "       	SELECT CASE WHEN COUNT(*) > 0 THEN '[당뇨]' END                 \r\n";
            SQL += "         	  FROM KOSMOS_OCS.EXAM_RESULTC                              \r\n";
            SQL += "         	 WHERE SPECNO = S.SPECNO                                    \r\n";
            SQL += "         	    AND SUBCODE IN ('CR59','CR59B')                         \r\n";
            SQL += "          )  				AS GLUE	 -- 혈당                            \r\n";
            SQL += "       , CASE WHEN P.JICODE = '63' OR P.JICODE >= '76'                  \r\n";
            SQL += "       		THEN	'[원거리]' ELSE '' END	AS WON   -- 원거리          \r\n";
            SQL += "       , (                                                              \r\n";
            SQL += "       	SELECT CASE WHEN COUNT(*) > 0 THEN '[SMS]' END                  \r\n";
            SQL += "           FROM KOSMOS_OCS.EXAM_SMS                                     \r\n";
            SQL += "          WHERE PANO = S.PANO AND GUBUN2 = '1'                          \r\n";
            SQL += "          )  											                    AS SMS              \r\n";
            SQL += "       , KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(S.PANO, S.BDATE, S.DEPTCODE) 	AS ERP   -- 중증도  \r\n";
            SQL += "    	 , DECODE(S.DEPTCODE,'ER','[응급실]')		 						AS ER               \r\n";
            SQL += "     	 , CASE WHEN S.DEPTCODE ='TO' OR S.DEPTCODE ='HR' THEN '[검진]' END AS HELS             \r\n";
            SQL += "       , P.JUMIN1 ||'-'|| P.JUMIN2                                          AS JUMIN            \r\n";
            SQL += "       , KOSMOS_OCS.FC_BAS_PATIENT_JUSO(S.PANO)                             AS JUSO             \r\n";
            SQL += "       , S.DEPTCODE                                                         AS DEPTCODE         \r\n";
            SQL += "       , S.IPDOPD                                                           AS IPDOPD           \r\n";
            SQL += "       , KOSMOS_OCS.FC_EXAM_SMS(S.PANO) 									AS SMS_DETAIL       \r\n";
            SQL += "       , TO_CHAR(S.BDATE,'YYYY-MM-DD')          	                        AS BDATE            \r\n";
            SQL += "       , S.SEX 				    					                        AS SEX              \r\n";
            SQL += "       , S.AGE 			    						                        AS AGE              \r\n";
            SQL += "       , S.BI           							                        AS BI               \r\n";
            SQL += "       , S.WARD           							                        AS WARD             \r\n";
            SQL += "       , S.ROOM           							                        AS ROOM             \r\n";
            SQL += "       , S.SPECCODE       							                        AS SPECCODE         \r\n";
            SQL += "       , KOSMOS_OCS.FC_EXAM_SPECMST_NM('14', S.SPECCODE,'Y')                AS SPECNM           \r\n";
            SQL += "       , (SELECT MAX(REMIND) FROM KOSMOS_OCS.EXAM_PATIENT WHERE PANO = S.PANO)     AS REMIND           \r\n";
            SQL += "       , S.GB_GWAEXAM       							                    AS GB_GWAEXAM        \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_SPECMST S                                                              \r\n";
            SQL += "       , KOSMOS_PMPA.BAS_PATIENT P                                                              \r\n";
            SQL += "   WHERE 1=1                                                                                    \r\n";
            SQL += "     AND S.SPECNO = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "     AND S.PANO = P.PANO                                                                        \r\n";

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

        public DataSet sel_EXAM_RESULTC(PsmhDb pDbCon, string strSPECNO, string strALL_CODE)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " WITH T AS (                                                                                                                           \r\n";
            SQL += "   	SELECT                                                                                                                             \r\n";
            SQL += "   	        M.EXAMNAME                                  AS EXAMNAME			-- 01                                                      \r\n";
            SQL += "   	      , (                                                                                                                          \r\n";
            SQL += "   	       	 CASE WHEN S.BI >= '60' AND NVL(TRIM(R.RESULT),'^*') = '^*'                                                                \r\n";
            SQL += "   	       	  			THEN                                                                                                           \r\n";
            SQL += "   							(                                                                                                          \r\n";
            SQL += "   							  SELECT MAX(Z.RESULT)                                                                                     \r\n";
            SQL += "   							    FROM KOSMOS_OCS.EXAM_SPECMST Q                                                                         \r\n";
            SQL += "   							        ,KOSMOS_OCS.EXAM_RESULTC Z                                                                         \r\n";
            SQL += "   							   WHERE 1=1                                                                                               \r\n";
            SQL += "   								 AND Q.PANO 	= S.PANO                                                                               \r\n";
            SQL += "   								 AND Q.SPECNO   != S.SPECNO                                                                            \r\n";
            SQL += "   							     AND Q.SPECNO 	= Z.SPECNO                                                                             \r\n";
            SQL += "   							     AND Z.SUBCODE  = R.SUBCODE                                                                            \r\n";
            SQL += "   							     AND Z.STATUS	= 'V'                                                                                  \r\n";
            SQL += "   							     AND Z.SUBCODE 	NOT IN ('SA011', 'SA01C', 'SA01A', 'SA041', 'SA04C', 'SA04A')                          \r\n";
            SQL += "   							     AND Q.RESULTDATE >= TRUNC(SYSDATE)                                                                    \r\n";
            SQL += "   							     AND Z.RESULT 	IS NOT NULL                                                                            \r\n";
            SQL += "   							  )                                                                                                        \r\n";
            SQL += "   	              ELSE                                                                                                                 \r\n";
            SQL += "   	              		CASE WHEN R.SUBCODE = 'CR42B'  AND NVL(TRIM(R.RESULT),'^&') = '^&' THEN KOSMOS_OCS.FC_GET_GFR(S.SPECNO, R.SUBCODE, S.AGE,S.SEX, R.RESULT)         \r\n";
            SQL += "   		              		 WHEN R.SUBCODE = 'UC011D' AND NVL(TRIM(R.RESULT),'^&') = '^&' THEN KOSMOS_OCS.FC_GET_MC_RATIO(S.SPECNO, R.SUBCODE)                           \r\n";
            SQL += "   		              		 WHEN R.SUBCODE = 'UC02R'  AND NVL(TRIM(R.RESULT),'^&') = '^&' THEN KOSMOS_OCS.FC_GET_PC_RATIO(S.SPECNO, R.SUBCODE)                           \r\n";
            SQL += "   	              		     ELSE                                                                                                      \r\n";
            SQL += "   	              		          R.RESULT END                                                                                         \r\n";
            SQL += "   	              END                                                                                                                  \r\n";
            SQL += "   	        )		                                    AS RESULT			-- 03                                                      \r\n";
            SQL += "   	      , (                                                                                                                          \r\n";
            SQL += "   						  SELECT Z.RESULT                                                                                              \r\n";            
            SQL += "   						    FROM KOSMOS_OCS.EXAM_SPECMST Q                                                                             \r\n";
            SQL += "   						        ,KOSMOS_OCS.EXAM_RESULTC Z                                                                             \r\n";
            SQL += "   						   WHERE 1=1                                                                                                   \r\n";
            SQL += "   							 AND Q.PANO 	= S.PANO                                                                                   \r\n";
            SQL += "   						     AND Q.SPECNO 	= Z.SPECNO                                                                                 \r\n";
            SQL += "   						     AND Z.SUBCODE  = R.SUBCODE                                                                                \r\n";
            SQL += "   							 AND Q.SPECNO   != R.SPECNO                                                                                \r\n";
            SQL += "   						     AND Z.STATUS	= 'V'                                                                                      \r\n";
            SQL += "   						     AND Z.SUBCODE 	NOT IN ('SA011', 'SA01C', 'SA01A', 'SA041', 'SA04C', 'SA04A')                              \r\n";                        
            SQL += "   						     AND Q.RESULTDATE =                                                                                        \r\n";
            SQL += "   						     					(                                                                                      \r\n";
            SQL += "   	     										  SELECT MAX(Q1.RESULTDATE)                                                            \r\n";
            SQL += "   												    FROM KOSMOS_OCS.EXAM_SPECMST Q1                                                    \r\n";
            SQL += "   												        ,KOSMOS_OCS.EXAM_RESULTC Z1                                                    \r\n";
            SQL += "   												   WHERE 1=1                                                                           \r\n";
            SQL += "   													 AND Q1.PANO 	  = S.PANO                                                         \r\n";
            SQL += "   												     AND Q1.SPECNO 	  = Z1.SPECNO                                                      \r\n";
            SQL += "   					     						     AND Z1.SUBCODE   = R.SUBCODE                                                      \r\n";
            SQL += "   													 AND Q1.SPECNO   != S.SPECNO                                                       \r\n";
            SQL += "   												     AND Z1.STATUS	  = 'V'                                                            \r\n";
            SQL += "   												     AND Z1.RESULTDATE > SYSDATE - 180                                                 \r\n";
            SQL += "   												     AND Z1.SUBCODE   NOT IN ('SA011', 'SA01C', 'SA01A', 'SA041', 'SA04C', 'SA04A')    \r\n";
            SQL += "   												 )                                                                                     \r\n";            
            SQL += "   						     AND Z.RESULT 	IS NOT NULL                                                                                \r\n";
            SQL += "   						     AND ROWNUM = 1                                                                                            \r\n";            
            //SQL += "   						  )                                                                                                            \r\n";
            //SQL += "   	               END                                                                                                                 \r\n";
            SQL += "   	        )		                                    					AS RESULT_BEFOR		-- 04                                  \r\n";
            SQL += "   		  , R.UNIT                                      					AS UNIT				-- 05                                  \r\n";
            SQL += "   	      , R.CV									    					AS CV         		-- 06                                  \r\n";
            SQL += "   	   	  , R.PANIC                                     					AS PANIC			-- 07                                  \r\n";
            SQL += "   		  , R.DELTA															AS DELTA			-- 08                                  \r\n";
            SQL += "   		  , R.REFER                                     					AS REFER			-- 09                                  \r\n";
            SQL += "                                                                                                                                       \r\n";
            SQL += "   		  , (                                                                                                                          \r\n";
            SQL += "   		     SELECT RESULT                                                                                                             \r\n";
            SQL += "   		  	   FROM KOSMOS_OCS.EXAM_RESULT_BAE                                                                                         \r\n";
            SQL += "   		  	   WHERE SPECNO  = R.SPECNO                                                                                                \r\n";
            SQL += "   		  	     AND SUBCODE = R.SUBCODE                                                                                               \r\n";
            SQL += "   		  	     AND ENTDATE = (SELECT MAX(ENTDATE)                                                                                    \r\n";
            SQL += "   		  	                      FROM KOSMOS_OCS.EXAM_RESULT_BAE                                                                      \r\n";
            SQL += "   		  	                     WHERE SPECNO = R.SPECNO)                                                                              \r\n";
            SQL += "   		  	)																AS MID 				-- 11 중간결과                         \r\n";
            SQL += "   		  , (SELECT CASE WHEN COUNT(*) > 0 THEN '▦' || COUNT(*) || '장' ELSE '' END                                                   \r\n";
            SQL += "   		       FROM KOSMOS_OCS.EXAM_RESULT_IMG                                                                                         \r\n";
            SQL += "   		      WHERE SPECNO 	= R.SPECNO                                                                                                 \r\n";
            SQL += "   		        AND WRTNO	= R.IMGWRTNO                                                                                               \r\n";
            SQL += "   		        AND SUBCODE = R.SUBCODE)									AS CHK_IMG 			-- 12                                  \r\n";
            SQL += "   		  , R.EQUCODE_INTER || DECODE(NVL(TRIM(R.EQUCODE_INTER),'^&'),'^&','','.' || KOSMOS_OCS.FC_EXAM_SPECMST_NM('13',TRIM(R.EQUCODE_INTER),'N')) AS EQUCODE_INTER	-- 13                                 \r\n";
            SQL += "   	      , R.SUBCODE                                   					AS SUBCODE			-- 14                                  \r\n";
            SQL += "   		  , R.RESULTWS                                  					AS RESULTWS			-- 15                                  \r\n";
            SQL += "   		  , R.ROWID                                     					AS ROWID_R			-- 16                                  \r\n";
            SQL += "   	      , R.SEQNO                         		 						AS SEQNO	  		-- 17                                  \r\n";
            SQL += "   	      , KOSMOS_OCS.FC_EXAM_RESULTCF_FOOTNOTE(R.SPECNO, R.SEQNO, 'N')	AS FOOTNOTE			-- 18                                  \r\n";
            SQL += "   		  , TO_CHAR(R.RESULTDATE,'YYYY-MM-DD HH24:MI')					    AS RESULTDATE		-- 22                                  \r\n";
            SQL += "   	      , M.DELTAM                                    					AS DELTAM			-- 23                                  \r\n";
            SQL += "   	      , M.DELTAP                                   	 					AS DELTAP			-- 24                                  \r\n";
            SQL += "   	      , M.DDDPRDRP                                  					AS DDDPRDRP    		-- 25                                  \r\n";
            SQL += "   		  ,	(                                                                                                                          \r\n";
            SQL += "   			  SELECT MAX(Q1.RESULTDATE)                                                                                                \r\n";
            SQL += "   		    	FROM KOSMOS_OCS.EXAM_SPECMST Q1                                                                                        \r\n";
            SQL += "   		        	,KOSMOS_OCS.EXAM_RESULTC Z1                                                                                        \r\n";
            SQL += "   		   	   WHERE 1=1                                                                                                               \r\n";
            SQL += "   			     AND Q1.PANO 	= S.PANO                                                                                               \r\n";
            SQL += "   		         AND Q1.SPECNO  = Z1.SPECNO                                                                                            \r\n";
            SQL += "   			     AND Z1.SUBCODE  = R.SUBCODE                                                                                           \r\n";
            SQL += "   			     AND Q1.SPECNO  != S.SPECNO                                                                                            \r\n";
            SQL += "   		         AND Z1.STATUS	= 'V'                                                                                                  \r\n";
            SQL += "   				 AND Z1.RESULTDATE > SYSDATE - 180                                                                                     \r\n";
            SQL += "   		         AND Z1.SUBCODE NOT IN ('SA011', 'SA01C', 'SA01A', 'SA041', 'SA04C', 'SA04A')                                          \r\n";
            SQL += "   		 	)																AS RESULT_BEFER_DATE --26                                  \r\n";
            SQL += "   	      , R.MASTERCODE                                					AS MASTERCODE		-- 27                                  \r\n";
            SQL += "   	      , M.PENDING														AS PENDING			-- 28                                  \r\n";
            SQL += "   		  , R.STATUS                                    					AS STATUS      		-- 29                                  \r\n";
            SQL += "   	      , M.RESULTIN                                  					AS RESULTIN			-- 31                                  \r\n";
            SQL += "   	      , R.RESULT                                    					AS RESULT_OLD		-- 35                                  \r\n";
            SQL += "   	      , KOSMOS_OCS.FC_EXAM_RESULTCF_FOOTNOTE(R.SPECNO, R.SEQNO, 'N')	AS FOOTNOTE_OLD		-- 36                                  \r\n";
            SQL += "   	      , R.RESULTSABUN                               					AS RESULTSABUN      -- 38                                  \r\n";
            SQL += "   	      , R.HCODE															AS HCODE			-- 39                                  \r\n";
            SQL += "   	      , R.IMGWRTNO                                  					AS IMGWRTNO			-- 40                                  \r\n";
            SQL += "   	      , (                                                                                                                          \r\n";
            SQL += "   	         SELECT CASE WHEN COUNT(*) > 0 THEN '▦' ELSE '' END                                                                       \r\n";
            SQL += "   	           FROM KOSMOS_OCS.EXAM_HISRESULTC                                                                                         \r\n";
            SQL += "   	         WHERE SPECNO = R.SPECNO                                                                                                   \r\n";
            SQL += "   	           AND SUBCODE = R.SUBCODE                                                                                                 \r\n";
            SQL += "   	           AND JOBGBN IN ('1','2')                                                                                                 \r\n";
            SQL += "   	        )																AS CHK_MODIFY		-- 42                                  \r\n";
            SQL += "   		  , R.PANO                                      					AS PANO                                                    \r\n";
            SQL += "   		  , S.SNAME                                      					AS SNAME                                                   \r\n";
            SQL += "   	      , M.WSCODE1                                   					AS WSCODE1                                                 \r\n";
            SQL += "          , S.AGE                                                                                                                      \r\n";
            SQL += "          , S.SEX                                                                                                                      \r\n";
            SQL += "   	      , TO_CHAR(BDATE,'YYYY-MM-DD') 			 						AS BDATE                                                   \r\n";
            SQL += "          , DECODE(R.SUBCODE,'CR42A',KOSMOS_OCS.FC_OCS_IILLS_ESRD(R.PANO,S.BDATE))	                        AS EX_CR42A                \r\n";
            SQL += "          , DECODE(NVL(R.RE_RESULT_CASE_CD,'*'),'*','', RE_RESULT_CASE_CD || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAME('EXAM_RE_RESULT',RE_RESULT_CASE_CD))	AS RE_RESULT_CASE_CD       \r\n";
            SQL += "          , RE_RESULT_CASE															                        AS RE_RESULT_CASE          \r\n";
            SQL += "          , KOSMOS_OCS.FC_BAS_USER_NAME(RE_RESULT_INPT) 							                        AS RE_RESULT_INPT          \r\n";
            SQL += "          , TO_CHAR(R.RE_RESULT_DT,'YYYY-MM-DD HH24:MI') 							                        AS RE_RESULT_DT            \r\n";
            SQL += "          , R.RE_RESULT                                                                                     AS RE_RESULT               \r\n";
            SQL += "          , TO_CHAR(R.TAT_DT,'YYYY-MM-DD HH24:MI')									                        AS TAT_DT                  \r\n";
            SQL += "          , KOSMOS_OCS.FC_BAS_USER_NAME(R.TAT_INPT)									                        AS TAT_INPT                \r\n";
            SQL += "          , DECODE(NVL(R.TAT_CASE_CD,'*'),'*','',R.TAT_CASE_CD || '.' || R.TAT_CASE)		                AS TAT_CASE_CD             \r\n";
            SQL += "          , R.TAT_CASE                                                                                      AS TAT_CASE                \r\n";
            SQL += "          , S.SPECNO                                                                                                                   \r\n";
            SQL += "   	  FROM KOSMOS_OCS.EXAM_SPECMST S                                                                                                   \r\n";
            SQL += "   	     , KOSMOS_OCS.EXAM_RESULTC R                                                                                                   \r\n";
            SQL += "   	     , KOSMOS_OCS.EXAM_MASTER  M                                                                                                   \r\n";
            SQL += "   	 WHERE 1=1                                                                                                                         \r\n";
            SQL += "   	   AND S.SPECNO		= R.SPECNO                                                                                                     \r\n";
            SQL += "   	   AND R.SUBCODE 	= M.MASTERCODE                                                                                                 \r\n";
            SQL += "   	   AND R.SPECNO     = " + ComFunc.covSqlstr(strSPECNO, false);

            if (string.IsNullOrEmpty(strALL_CODE.Trim()) == false)
            {
                SQL += "  	   AND R.MASTERCODE     IN (" + strALL_CODE + ")";
            }

            SQL += "   	 ORDER BY R.SEQNO                                                                                                                  \r\n";
            SQL += "   	 )                                                                                                                                 \r\n";
            SQL += "   SELECT                                                                                                                              \r\n";
            SQL += "   	  DECODE(STATUS,'V','전송','') AS STATUS_VIEW  -- 29                                                                               \r\n";
            SQL += "   	, STATUS                                                                                                                           \r\n";
            SQL += "   	, STATUS            AS STATUS_OLD                                                                                                  \r\n";
            SQL += "   	, EXAMNAME       	-- 01                                                                                                          \r\n";
            SQL += "   	, DECODE(NVL(FOOTNOTE,'^*'), '^*','','F')       AS F   	-- 02                                                                      \r\n";
            SQL += "   	, RESULT         	-- 03                                                                                                          \r\n";
            SQL += "   	, RESULT_BEFOR   	-- 04                                                                                                          \r\n";
            SQL += "   	, UNIT           	-- 05                                                                                                          \r\n";
            SQL += "   	, CASE WHEN NVL(STATUS,'Q') != 'V' AND NVL(RESULT,'^*') != '^*' THEN KOSMOS_OCS.FC_EXAM_MASTER_CV(SUBCODE,RESULT) ELSE CV END  AS CV	-- 05      \r\n";


            //SQL += "   	,  CASE WHEN KOSMOS_OCS.FC_OCS_IILLS_ESRD(PANO, BDATE) = 'Y' THEN ''                                                               \r\n";
            //SQL += "   	       ELSE (                                                                                                                      \r\n";
            //SQL += "   					 CASE WHEN STATUS != 'V' AND NVL(RESULT,'^*') != '^*' THEN KOSMOS_OCS.FC_EXAM_MASTER_CV(SUBCODE,RESULT) ELSE CV END \r\n";
            //SQL += "   				) END																								   AS CV    -- 06  \r\n";
            SQL += "   	, CASE WHEN NVL(STATUS,'Q') != 'V' AND NVL(RESULT,'^*') != '^*'                                                                             \r\n";
            SQL += "   	       THEN KOSMOS_OCS.FC_EXAM_MASTER_PANIC(SUBCODE,RESULT) ELSE PANIC END 				    					   AS PANIC -- 07  \r\n";
            SQL += "   	, CASE WHEN NVL(STATUS,'Q') != 'V' AND NVL(RESULT,'^*') != '^*'                                                                             \r\n";
            SQL += "   	       THEN KOSMOS_OCS.FC_EXAM_RESULTC_DELTA(RESULT,RESULT_BEFOR,DELTAM,DELTAP,DDDPRDRP, RESULT_BEFER_DATE)                        \r\n";
            SQL += "   	       ELSE DELTA END 	                                                                                           AS DELTA -- 08  \r\n";
            SQL += "   	, CASE WHEN NVL(STATUS,'Q') != 'V' AND NVL(RESULT,'^*') != '^*'                                                                             \r\n";
            SQL += "   	       THEN KOSMOS_OCS.FC_EXAM_MASTER_SUB_REF('1',SUBCODE,SEX,AGE,RESULT) ELSE REFER END						   AS REFER -- 09  \r\n";
            SQL += "   	, '' CHK           	-- 11                                                                                                          \r\n";    
            SQL += "   	, MID            	-- 11                                                                                                          \r\n";
            SQL += "   	, CHK_IMG        	-- 12                                                                                                          \r\n";
            SQL += "    , ''	AS CHK_RE_RESULT                                                                                                           \r\n";
            SQL += "    , RE_RESULT_CASE_CD                                                                                                                \r\n";
            SQL += "    , RE_RESULT_CASE                                                                                                                   \r\n";
            SQL += "    , RE_RESULT_INPT                                                                                                                   \r\n";
            SQL += "    , RE_RESULT_DT                                                                                                                     \r\n";
            SQL += "    , RE_RESULT                                                                                                                        \r\n";
            SQL += "    , TAT_DT                                                                                                                           \r\n";
            SQL += "    , TAT_INPT                                                                                                                         \r\n";
            SQL += "    , TAT_CASE_CD                                                                                                                      \r\n";
            SQL += "    , TAT_CASE                                                                                                                         \r\n";
            SQL += "    , DELTAM                                                                                                                           \r\n";
            SQL += "    , DELTAP                                                                                                                           \r\n";
            SQL += "    , DDDPRDRP                                                                                                                         \r\n";

            SQL += "   	, SUBCODE        	-- 14                                                                                                          \r\n";
            SQL += "   	, RESULTWS       	-- 15                                                                                                          \r\n";
            SQL += "   	, ROWID_R        	-- 16                                                                                                          \r\n";
            SQL += "   	, SEQNO          	-- 17                                                                                                          \r\n";
            SQL += "   	, FOOTNOTE       	-- 18                                                                                                          \r\n";
            SQL += "   	, ''                                                                AS  CHANG     	-- 21                                          \r\n";
            SQL += "   	, RESULTDATE        -- 22                                                                                                          \r\n";
            SQL += "   	, TO_CHAR(RESULT_BEFER_DATE,'YYYY-MM-DD HH24:MI') -- 26                                                                            \r\n";
            SQL += "   	, MASTERCODE        -- 27                                                                                                          \r\n";
            SQL += "   	, PENDING           -- 28                                                                                                          \r\n";
            SQL += "   	, RESULTIN          -- 31                                                                                                          \r\n";
            SQL += "   	, RESULT_OLD        -- 35                                                                                                          \r\n";
            SQL += "   	, FOOTNOTE_OLD      -- 36                                                                                                          \r\n";
            SQL += "   	, RESULTSABUN       -- 38                                                                                                          \r\n";
            SQL += "   	, HCODE             -- 39                                                                                                          \r\n";
            SQL += "   	, IMGWRTNO          -- 40                                                                                                          \r\n";
            SQL += "   	, CASE WHEN STATUS = 'V' AND NVL(RESULT,'^*') != '^*' THEN CV                                                                              \r\n";
            SQL += "   	       ELSE KOSMOS_OCS.FC_EXAM_MASTER_CV(SUBCODE,RESULT) END AS CV_OLD    -- 41                                            \r\n";
            SQL += "   	, CHK_MODIFY        -- 42                                                                                                          \r\n";
            SQL += "   	, PANO              -- 01                                                                                                          \r\n";
            SQL += "   	, SNAME             -- 01                                                                                                          \r\n";
            SQL += "   	, WSCODE1           -- 01                                                                                                          \r\n";
            SQL += "   	, EX_CR42A          -- 01                                                                                                          \r\n";
            SQL += "   	, SPECNO                                                                                                                           \r\n";
            SQL += "   	, EQUCODE_INTER  	-- 13                                                                                                          \r\n";
            SQL += " FROM T                                                                                                                                \r\n";
                                                                                                                                                                                   
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

        public DataSet sel_EXAM_RESULTC_HIS(PsmhDb pDbCon, string strPANO, string strSUBCODE)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                \r\n";
            SQL += "  	   S.SPECNO                                                         \r\n";
            SQL += "  	 , KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',S.SPECCODE, 'Y')  AS SPEC_NM   \r\n";
            SQL += "  	 , TO_CHAR(RECEIVEDATE, 'YYYY-MM-DD')                   AS RECEIVEDATE \r\n";
            SQL += "  	 , R.RESULT                                                         \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_RESULTC R                                      \r\n";
            SQL += "       , KOSMOS_OCS.EXAM_SPECMST S                                      \r\n";
            SQL += "    WHERE 1=1                                                           \r\n";
            SQL += "      AND S.PANO 	= " + ComFunc.covSqlstr(strPANO, false);
            SQL += "      AND S.SPECNO 	= R.SPECNO                                          \r\n";
            SQL += "      AND SUBCODE 	= " + ComFunc.covSqlstr(strSUBCODE, false);
            SQL += "      AND S.STATUS 	= '05'                                              \r\n";
            SQL += "  ORDER BY S.BDATE DESC, S.SPECNO                                       \r\n";

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

        /// <summary>
        /// 환경검사 헬프코드
        /// </summary>
        /// <returns></returns>
        public DataSet sel_EXAM_MASTER_SUB(PsmhDb pDbCon)
        {
            DataSet ds = null;

            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT NAME, SORT, CODE AS NORMAL   ").Append("\n");
            sql.Append("  FROM KOSMOS_OCS.EXAM_SPECODE      ").Append("\n");
            sql.Append(" WHERE GUBUN = 98                   ").Append("\n");
            sql.Append("ORDER BY CODE                       ").Append("\n");

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, sql.ToString(), pDbCon);

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

        public DataSet sel_EXAM_MASTER_SUB(PsmhDb pDbCon, string strMASTERCODE, bool isMicro)
        {
            DataSet ds = null;

            SQL = "";

            if (isMicro == true)
            {
                SQL += "   SELECT                                   \r\n";
                SQL += "   		  S.NAME                            \r\n";
                SQL += "   		, S.SORT                            \r\n";
                SQL += "   		, S.CODE NORMAL                     \r\n";
                SQL += "     FROM KOSMOS_OCS.EXAM_SPECODE 	S       \r\n";
                SQL += "    WHERE 1=1                               \r\n";
                SQL += "      AND S.GUBUN 	= '18'                  \r\n";
                SQL += "      AND S.CODE    LIKE 'ZZ%'              \r\n";
                SQL += "    ORDER BY S.SORT                         \r\n";
            }
            else
            {
                SQL += "   SELECT                                   \r\n";
                SQL += "   		  S.NAME                            \r\n";
                SQL += "   		, M.SORT                            \r\n";
                SQL += "   		, M.NORMAL 		                    \r\n";
                SQL += "     FROM KOSMOS_OCS.EXAM_MASTER_SUB 	M   \r\n";
                SQL += "        , KOSMOS_OCS.EXAM_SPECODE 	S       \r\n";
                SQL += "    WHERE 1=1                               \r\n";
                SQL += "      AND M.MASTERCODE 	= " + ComFunc.covSqlstr(strMASTERCODE, false);
                SQL += "      AND M.GUBUN 		= '18'              \r\n";
                SQL += "      AND M.NORMAL 		= S.CODE            \r\n";
                SQL += "      AND S.GUBUN 		= '18'              \r\n";
                SQL += "    ORDER BY M.SORT,M.NORMAL                \r\n";
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
        
        public DataSet sel_EXAM_SPECMST(PsmhDb pDbCon, string strRECEIVEDATE_Fr, string strRECEIVEDATE_To, string strSTATUS
                                        , string strAllCode, string strAllEquCode, string strDEPTCODE, bool isER)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                                                                                                         \r\n";
            SQL += "         S.SPECNO                                                                									 AS SPECNO                  \r\n";
            SQL += " 	   , S.SNAME                                                                 									 AS SNAME                   \r\n";
            SQL += "  	   , DECODE(S.DEPTCODE,'ER','ER')		 																		 AS ER       -- 응급실      \r\n";
            SQL += "       , DECODE(S.STRT,'S','응급','')									  											 AS STRT	 -- 응급        \r\n";
            SQL += "   	   , CASE WHEN S.DEPTCODE ='HR'  THEN '신검' END    									                		 AS HELS_HR  -- 종검               \r\n";
            SQL += "   	   , CASE WHEN S.DEPTCODE ='TO'  THEN '종검' END    									                		 AS HELS_TO  -- 신검               \r\n";
            SQL += "       , CASE WHEN MAX(P.JICODE) = '63' OR MAX(P.JICODE) >= '76' THEN	'원거리' ELSE '' END						 AS WON      -- 원거리      \r\n";
            SQL += " 	   , DECODE(SUM(DECODE(R.SUBCODE,'CR59',1,'CR59B',1,0)),0,'','당뇨') 											 AS GLUE	 -- 혈당        \r\n";                        
            SQL += "       , KOSMOS_OCS.FC_OPD_MASTER_ERPATIENT(S.PANO, S.BDATE, S.DEPTCODE) 				    						 AS ERP      -- 중증도      \r\n";            
            SQL += "   	   , S.IPDOPD                                                                                                    AS IPDOPD                  \r\n";
            SQL += "   	   , S.BDATE                                                                                                     AS BDATE                   \r\n";
            SQL += "   	   , S.PANO                                                                                                      AS PANO                    \r\n";
            SQL += "   	   , S.DEPTCODE                                                                                                  AS DEPTCODE                \r\n";
            SQL += "   FROM  KOSMOS_OCS.EXAM_SPECMST S                                                                                                              \r\n";
            SQL += "       , KOSMOS_OCS.EXAM_RESULTC R                                                                                                              \r\n";
            SQL += "       , KOSMOS_PMPA.BAS_PATIENT P                                                                                                              \r\n";
            ////2018-10-23 안정수, EXAM_ORDER에서 STRT를 읽어오기 위해 추가함
            //SQL += "       , KOSMOS_OCS.EXAM_ORDER O                                                                                                                \r\n";
            SQL += "  WHERE 1=1                                                                                                                                     \r\n";
            SQL += "    AND S.STATUS    != '06'                                                                                                                     \r\n";
            SQL += "    AND S.SPECNO    = R.SPECNO                                                                                                                  \r\n";
            SQL += "    AND S.PANO      = P.PANO                                                                                                                    \r\n";
            //SQL += "    AND S.PANO      = O.PANO                                                                                                                    \r\n";

            if (strDEPTCODE.Equals(clsParam.EXAM_DEPT_LIS) == true)
            {
                #region LIS

                SQL += "    AND S.RECEIVEDATE >= TO_DATE('" + strRECEIVEDATE_Fr + "','YYYY-MM-DD')                                                                      \r\n";
                SQL += "    AND S.RECEIVEDATE  < TO_DATE('" + strRECEIVEDATE_To + "', 'YYYY-MM-DD') + 1                                                                 \r\n";

                //01:접수, 02:일부결과 들어와 있슴, 03:모든결과 들어와 있슴, 04:일부 Verify  14:일부 Verify된 것이 있고, 결과 들어와 있는게 있슴, 05:모든 검사항목이 Verify

                SQL += "    AND (S.GB_GWAEXAM IS NULL OR S.GB_GWAEXAM <> 'Y')                                                                                           \r\n";

                if (strSTATUS.Trim().Equals("01") == true)
                {
                    SQL += "    AND S.STATUS  = '01'                                                                                                                    \r\n";
                }
                else if (strSTATUS.Trim().Equals("020304") == true)
                {
                    SQL += "    AND S.STATUS  IN ('02','03','04')                                                                                                       \r\n";
                    SQL += "    AND NVL(R.STATUS,'^&')  != 'V'                                                                                                                    \r\n";
                }
                else if (strSTATUS.Trim().Equals("01020304") == true)
                {
                    SQL += "    AND S.STATUS  IN ('01','02','03','04')                                                                                                       \r\n";
                    SQL += "    AND NVL(R.STATUS,'^&')  != 'V'                                                                                                                    \r\n";
                }
                else if (strSTATUS.Trim().Equals("05") == true)
                {
                    SQL += "    AND S.STATUS  IN ('05')                                                                                                                 \r\n";
                }

                if (string.IsNullOrEmpty(strAllCode.Trim()) == false)
                {
                    if (isER == true)
                    {
                        // 미생물을 제외한 ER
                        SQL += "    AND ((S.DEPTCODE = 'ER' AND S.WORKSTS NOT LIKE '%M%') AND R.MASTERCODE IN (" + strAllCode + "))                                                                       \r\n";
                    }
                    else
                    {
                        SQL += "    AND (R.MASTERCODE IN (" + strAllCode + "))                                                                       \r\n";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(strAllEquCode.Trim()) == false)
                    {
                        if (isER == true)
                        {

                            // 미생물을 제외한 ER
                            SQL += "    AND (                                                                                                                                   \r\n";
                            SQL += "            (                                                                                                                                   \r\n";
                            SQL += "                CASE WHEN S.WORKSTS LIKE '%E%' AND S.DEPTCODE = 'ER' AND R.SUBCODE IN ('CR07A','CS04A','CS07A','H083A', 'SE41A') THEN 'ER' -- 2018.03.28.김홍록:COBAS 장비일 경우 특정 검사만 응급실 체크함.   \r\n";
                            SQL += "                     WHEN S.WORKSTS LIKE '%M%' AND S.DEPTCODE = 'ER' THEN ''                                      \r\n";
                            SQL += "                     WHEN S.WORKSTS LIKE '%E%' AND S.DEPTCODE = 'ER' THEN ''                                      \r\n";
                            SQL += "                     WHEN S.WORKSTS LIKE '%P%' AND S.DEPTCODE = 'ER' THEN ''                                      \r\n";
                            SQL += "                     ELSE S.DEPTCODE END                                                                          \r\n";
                            SQL += "            )   = 'ER'                                                                                            \r\n";
                            SQL += "         OR R.EQUCODE IN (" + strAllEquCode + ")                                                                  \r\n";
                            SQL += "        )                                                                                                         \r\n";
                        }
                        else
                        {
                            SQL += "    AND ( R.EQUCODE IN (" + strAllEquCode + "))                                                                   \r\n";
                        }
                    }
                }

                #endregion
            }
            else
            {
                SQL += "    AND S.BDATE BETWEEN TO_DATE('" + strRECEIVEDATE_Fr + "','YYYY-MM-DD')                                            \r\n";
                SQL += "                    AND TO_DATE('" + strRECEIVEDATE_To + "', 'YYYY-MM-DD')                                           \r\n";

                ///2018-10-23 안정수, 기존 진단검사과가 아닌경우 과검사인경우만 보이도록 되어있었으나
                ///내시경실 요청으로 내시경실일 경우 MI11검사이면서, 응급인경우만 보이도록 수정함                 
                if (strDEPTCODE.Equals(clsParam.EXAM_DEPT_ENDO) == true)
                {
                    SQL += "    AND R.SUBCODE = " + ComFunc.covSqlstr("MI11", false);
                    //2018-10-23 안정수, 김수연s 요청으로 응급겁사인것만 보이도록
                    SQL += "    AND S.STRT  = 'S'                                                                                                                    \r\n";
                    SQL += "    AND S.DEPTCODE NOT IN ('TO','HR')                                                                                                    \r\n";
                    
                    if (strSTATUS.Trim().Equals("01020304") == true)
                    {
                        SQL += "    AND S.STATUS  IN ('00','01','02','03','04')                                                                                                       \r\n";
                        SQL += "    AND NVL(R.STATUS,'^&')  != 'V'                                                                                                                    \r\n";
                    }                    
                    else if (strSTATUS.Trim().Equals("05") == true)
                    {
                        SQL += "    AND S.STATUS  IN ('05')                                                                                                                 \r\n";
                    }
                }
                else if(strDEPTCODE == "40" || strDEPTCODE == "33" || strDEPTCODE == "35" || strDEPTCODE == "50" || strDEPTCODE == "60" || strDEPTCODE == "70" || strDEPTCODE == "80" || strDEPTCODE == "53" || strDEPTCODE == "55" || strDEPTCODE == "63" || strDEPTCODE == "65" || strDEPTCODE == "73" || strDEPTCODE == "75" || strDEPTCODE == "83" || strDEPTCODE == "ER" || strDEPTCODE == "40" )
                {
                    
                    SQL += "        AND S.WARD = '" + strDEPTCODE + "'                                                                                                       \r\n";
                    SQL += "        AND S.GB_GWAEXAM = 'Y'                                                                                                                  \r\n";
                    if (strSTATUS.Trim().Equals("01020304") == true)
                    {
                        SQL += "    AND S.STATUS NOT IN ('05')                                                                                                       \r\n";
                        SQL += "    AND NVL(R.STATUS,'^&')  != 'V'                                                                                                          \r\n";
                    }
                    else if (strSTATUS.Trim().Equals("05") == true)
                    {
                        SQL += "    AND S.STATUS  IN ('05')                                                                                                                 \r\n";
                    }
                }
                else 
                {
                    SQL += "    AND S.GB_GWAEXAM = 'Y'                                                                                           \r\n";
                    //}

                    if (strSTATUS.Trim().Equals("01") == true) 
                    {
                        SQL += "    AND S.STATUS  = '01'                                                                                                                    \r\n";
                    }
                    else if (strSTATUS.Trim().Equals("020304") == true)
                    {
                        SQL += "    AND S.STATUS  IN ('02','03','04')                                                                                                       \r\n";
                        SQL += "    AND NVL(R.STATUS,'^&')  != 'V'                                                                                                          \r\n";
                    }
                    else if (strSTATUS.Trim().Equals("01020304") == true)
                    {
                        SQL += "    AND S.STATUS  IN ('01','02','03','04')                                                                                                       \r\n";
                        SQL += "    AND NVL(R.STATUS,'^&')  != 'V'                                                                                                                    \r\n";
                    }
                    else if (strSTATUS.Trim().Equals("05") == true)
                    {
                        SQL += "    AND S.STATUS  IN ('05')                                                                                                                 \r\n";
                    }
                }
                //if (string.IsNullOrEmpty(strWARD.Trim()) == false)
                //{
                //    SQL += "    AND S.WARD = " + ComFunc.covSqlstr(strWARD, false);
                //}

            }

            SQL += "                                                                                                                                                \r\n";
            SQL += "  GROUP BY S.SNAME,S.SPECNO,S.STATUS, S.STRT,S.PANO, S.IPDOPD, S.BI, S.DEPTCODE ,S.BDATE ,S.PANO                                                \r\n";
            SQL += "  ORDER BY S.SNAME,S.SPECNO,S.STATUS                                                                                                            \r\n";


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

        public DataSet sel_EXAM_RESULT_BAE(PsmhDb pDbCon, string strSPECNO, string strSUBCODE, string strBDATE)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                        \r\n";
            SQL += "         '' 									AS CHK          \r\n";
            SQL += "     ,   0 										AS SEQ          \r\n";
            SQL += "  	 , TO_CHAR(ENTDATE,'YYYY-MM-DD HH24:MI')	AS ENTDATE      \r\n";
            SQL += "  	 , C.RESULT									AS RESULT       \r\n";
            SQL += "   	 , A.SPECNO									AS SPECNO       \r\n";
            SQL += "   	 , A.SUBCODE    							AS SUBCODE      \r\n";
            SQL += "   	 , TO_CHAR(B.BDATE,'YYYY-MM-DD')       		AS BDATE        \r\n";
            SQL += "   	 , B.PANO                                                   \r\n";
            SQL += "   	 , B.SNAME                                                  \r\n";
            SQL += "   	 , B.SEX                                                    \r\n";
            SQL += "   	 , B.DEPTCODE                                               \r\n";
            SQL += "   	 , B.IPDOPD                                                 \r\n";
            SQL += "   	 , B.WARD                                                   \r\n";
            SQL += "   	 , D.EXAMFNAME                                              \r\n";
            SQL += "   	 , C.ROWID                                 AS ROW_ID        \r\n";
            SQL += "     FROM KOSMOS_OCS.EXAM_RESULTC 	    A                       \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_SPECMST 	    B                       \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_RESULT_BAE 	C                       \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_MASTER        D                       \r\n";
            SQL += "    WHERE 1=1                                                   \r\n";
            SQL += "      AND B.SPECNO  = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "      AND B.STATUS != '06'                                      \r\n";
            SQL += "      AND A.SUBCODE = " + ComFunc.covSqlstr(strSUBCODE, false);
            SQL += "      AND D.MASTERCODE = A.SUBCODE(+)                           \r\n";
            SQL += "      AND A.SPECNO  = B.SPECNO                                  \r\n";
            SQL += "      AND A.SPECNO  = C.SPECNO(+)                               \r\n";
            SQL += "      AND A.SUBCODE = DECODE(NVL(C.SUBCODE,'%^'),'%^',A.SUBCODE,C.SUBCODE)                              \r\n";
            SQL += "  UNION ALL                                                     \r\n";
            SQL += "   SELECT                                                       \r\n";
            SQL += "         '' 									AS CHK          \r\n";
            SQL += "     ,   1 			AS SEQ                                      \r\n";
            SQL += "  	 , ''			AS ENTDATE                                  \r\n";
            SQL += "  	 , ''			AS RESULT                                   \r\n";
            SQL += "   	 , '" + strSPECNO  + "'	AS SPECNO                           \r\n";
            SQL += "   	 , '" + strSUBCODE + "'	AS SUBCODE                          \r\n";
            SQL += "   	 , '" + strBDATE   + "'	AS BDATE                            \r\n";
            SQL += "   	 , ''			AS PANO                                     \r\n";
            SQL += "   	 , ''			AS SNAME                                    \r\n";
            SQL += "   	 , ''			AS SEX                                      \r\n";
            SQL += "   	 , ''			AS DEPTCODE                                 \r\n";
            SQL += "   	 , ''			AS IPDOPD                                   \r\n";
            SQL += "   	 , ''			AS WARD                                     \r\n";
            SQL += "   	 , ''			AS EXAMFNAME                                \r\n";
            SQL += "   	 , NULL			AS ROW_ID                                   \r\n";
            SQL += "     FROM DUAL 	A                                               \r\n";
            SQL += "  CONNECT BY LEVEL < 4                                          \r\n";
            SQL += "  ORDER BY ENTDATE                                              \r\n";
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

        public string save_EXAM_RESULT_BAE(PsmhDb pDbCon, string strROWID, string strRESULT, string strPANO, string strSPECNO
                                        , string strBDATE, string strSUBCODE, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " MERGE INTO KOSMOS_OCS.EXAM_RESULT_BAE  A   \r\n";                            
            SQL += "      USING DUAL                            \r\n";                       
            SQL += " 	    ON (                                \r\n";
            SQL += " 	    	A.ROWID         = " + ComFunc.covSqlstr(strROWID, false);
            SQL += " 	    	)                               \r\n";
            SQL += " 		WHEN MATCHED THEN                   \r\n";
            SQL += " 		UPDATE SET RESULT   = " + ComFunc.covSqlstr(strRESULT, false);
            SQL += " 		WHEN NOT MATCHED THEN               \r\n";
            SQL += " 		INSERT 	(                           \r\n";
            SQL += " 				PANO                        \r\n";
            SQL += " 			,	SPECNO                      \r\n";
            SQL += " 			,	BDATE                       \r\n";
            SQL += " 			,	RESULT                      \r\n";
            SQL += " 			,	ENTDATE                     \r\n";
            SQL += " 			, 	SUBCODE                     \r\n";
            SQL += " 		) VALUES (    	                    \r\n";
		
		    SQL += "              " + ComFunc.covSqlstr(strPANO, false);
            SQL += "              " + ComFunc.covSqlstr(strSPECNO, true);
            SQL += "              " + ComFunc.covSqlDate(strBDATE, true);
            SQL += "              " + ComFunc.covSqlstr(strRESULT, true);
            SQL += "              , SYSDATE                                                     \r\n";
            SQL += "              " + ComFunc.covSqlstr(strSUBCODE, true);
            SQL += "              )                                                             \r\n";


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

        public string del_EXAM_RESULT_BAE(PsmhDb pDbCon, string strROWID, ref int intRowAffected, ref string SQL)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " DELETE KOSMOS_OCS.EXAM_RESULT_BAE  A   \r\n";
            SQL += "  WHERE ROWID =  " + ComFunc.covSqlstr(strROWID, false);

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

    }
}
