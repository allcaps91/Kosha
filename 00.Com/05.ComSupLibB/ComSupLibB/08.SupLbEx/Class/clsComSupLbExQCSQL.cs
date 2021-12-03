using ComBase; //기본 클래스
using ComDbB; //DB연결
using Oracle.DataAccess.Client;
using System;
using System.Data;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name : ComSupLibB.SupLbEx
    /// File Name : clsLbExSQLQC.cs
    /// Title or Description : 진단검사의학과 QC(품질관리) SQL
    /// Author : 김홍록
    /// Create Date : 2017-09-22
    /// Update History : 
    /// </summary> 

    public class clsComSupLbExQCSQL : Com.clsMethod
    {
        string SQL = string.Empty;

        public enum enmSS_EXAM_SPECODE { CODE, NAME };
        public string[] sSS_EXAM_SPECODE = { "장비코드", "장비명" };
        public int[] nSS_EXAM_SPECODE = { nCol_EXCD + 20, nCol_NAME };

        public enum enmSel_EXAM_MASTER_GBBASE      {    GBBASE,    MASTERCODE,      EXAMNAME,  ROWID  };
        public string[] sSel_EXAM_MASTER_GBBASE  = {    "사용",    "검사코드",    "검사명칭", "ROWID" };
        public int[] nSel_EXAM_MASTER_GBBASE     = { nCol_SCHK,  nCol_EXCD-30,  nCol_NAME-10,       5 };

        public enum enmSel_EXAM_CHAMGO_GROUP     { EXAMCODE     , BDATE };
        public string[] sSel_EXAM_CHAMGO_GROUP = { "검사코드"   , "작업일자" };
        public int[] nSel_EXAM_CHAMGO_GROUP    = { nCol_EXCD +20   ,nCol_DATE + 20 };

        public enum enmSel_EXAM_CHAMGO          {GBN,BDATE,EXAMCODE,EXAMNAME,SEQNO,RESULT,ROWID };

        public enum enmSel_EXAM_RESULTC_CHAMGO     { RESULTDATE,       PANO,      SNAME,       SEX,      AGE,    RESULT,     REFER,   EXAMNAME,    SUBCODE  };
        public string[] sSel_EXAM_RESULTC_CHAMGO = { "결과일자", "등록번호",     "성명",    "성별",   "나이",    "결과",       "R",   "검사명",  "검사코드" };
        public int[] nSel_EXAM_RESULTC_CHAMGO    = {  nCol_DATE,  nCol_PANO, nCol_SNAME,  nCol_SEX, nCol_AGE, nCol_NAME, nCol_AGE ,  nCol_NAME,   nCol_SPNO };

        public enum enmSel_EXAM_JUNCODE       {       CODE,     NAME,     USE_YN,   GUBUN_NM,     NORMAL,    LOW,     HIGH,       INTER1,       INTER2,     INTER3  };    
        public string[] sSel_EXAM_JUNCODE   = { "장비코드", "장비명", "사용여부", "검사구분",   "Normal",  "Low",   "High", "InterFace1", "InterFace2", "InterFace3" };
        public int[] nSel_EXAM_JUNCODE      = { nCol_EXCD, nCol_NAME,   nCol_AGE, nCol_TIME, nCol_PANO, nCol_AGE, nCol_AGE,    nCol_PANO,    nCol_PANO,   nCol_PANO };

        public enum enmSel_EXAM_JUNLOT        {        CHK,   EXAMCODE,  EXAMNAME,    GBCHAM,      GBSD,     SEQNO,    COMPANY, MATERIAL,  CHK_ACCEP,   NOR,   ROWID };
        public string[] sSel_EXAM_JUNLOT    = {        "C", "검사코드",  "검사명",  "참고치",     "2SD",    "순서",   "회사명", "물질명", "허용물질", "NOR",  "ROWID" };
        public int[] nSel_EXAM_JUNLOT       = {  nCol_SCHK,  nCol_EXCD, nCol_NAME, nCol_EXCD, nCol_EXCD, nCol_AGE,        5,        5,   nCol_AGE,     5,      5 };

        public enum enmSS_EXAM_MASTER_JUNJANG { CHK, MASTERCODE, MASTERNAME };
        public string[] sSS_EXAM_MASTER_JUNJANG = { "C", "검사코드", "검사명칭" };
        public int[] nSS_EXAM_MASTER_JUNJANG = { nCol_SCHK, nCol_EXCD , nCol_NAME };

        public enum enmSS_EXAM_JUNJANG { CHK, EXAMCODE, EXAMNAME, GBCHART, RANGKING, ROWID };
        public string[] sSS_EXAM_JUNJANG = { "C", "검사코드", "검사명칭", "그래프", "순위", "ROWID" };
        public int[] nSS_EXAM_JUNJANG = { nCol_SCHK, nCol_NAME-10 , nCol_JUSO-50, nCol_SNAME, nCol_SNAME, 5 };

        public enum enmSel_EXAM_JUNMULCODE     {         GUBUN,         EXAMCODE,        EXAMNAME , RANGKING ,  ROWID };
        public string[] sSel_EXAM_JUNMULCODE = {    "작업구분",       "검사코드",       "검사명칭",    "순위", "ROWID" };
        public int[] nSel_EXAM_JUNMULCODE    = { nCol_SNAME+10,   nCol_EXCD + 10,  nCol_SNAME + 50, nCol_EXCD, 5 };

        public enum enmSel_EXAM_JUNDETAIL_JOBDAY    { JOBDAY, SPECNO };

        public enum enmSel_EXAM_JUNMULDETAIL_RESULT     {   EXAMCODE,   EXAMNAME,          AVG,       SD2,      REF1,     REF2 };
        public string[] sSel_EXAM_JUNMULDETAIL_RESULT = { "검사코드",   "검사명",       "평균",   "±2SD", "참고치MIN", "참고치MAX" };
        public int[] nSel_EXAM_JUNMULDETAIL_RESULT    = { nCol_SNAME, nCol_TIME, nCol_IOPD, nCol_IOPD, nCol_IOPD, nCol_IOPD };

        public enum enmSel_EXAM_JUNDETAIL_RESULT_TERM { ORDER_BY,    SEQNO, EXAMCODE, EXAMNAME, LOTNO, GBCHAM, GBSD };

        public enum enmSel_EXAM_JUNDETAIL_RESULT      { ORDER_BY, RANGKING, EXAMCODE, EXAMNAME, GBCHAM, GBSD };

        public enum enmSel_EXAM_JUNMST_SPENO     {     CHK, PRT_CHK,   JOBDATE,      LOTNO,       NOR,     SPECNO,        CODE};
        public string[] sSel_EXAM_JUNMST_SPENO = {  "삭제", "출력","작업일자", "Lot 번호",     "Nor", "검체번호", "장비코드" };
        public int[] nSel_EXAM_JUNMST_SPENOE = { nCol_SCHK, nCol_SCHK, nCol_DATE, nCol_SNAME, nCol_CHEK,  nCol_SPNO,          5 };

        public enum enmSel_EXAM_JUNMST_BARCODE { PTNAME, EQUCODE, NOR, WC, EXAMCODE, LOTNO, SPECNO };

        public enum enmSel_EXAM_JUNDETAIL_RESULTC     {    SPECNO,   CODE,  EQU_NAME,   LOTNO,   NOR,    JOBDATE,   SUBCODE,    EXAMNAME,    GBCHAM,      GBSD,  EQU_RESULT,      RESULT,       REF,     RESULT1,   RSLT_RSN1,     RESULT2,   RSLT_RSN2,       MIN,      MAX  };
        public string[] sSel_EXAM_JUNDETAIL_RESULTC = {  "SPECNO", "CODE","EQU_NAME", "LOTNO", "NOR", "작업일자","검사코드",   "검사명",  "참고치",     "2SD",  "장비결과",  "정도결과",       "R", "재검결과1", "재검사유1", "재검결과2", "재검사유2",     "MIN",     "MAX" };
        public int[] nSel_EXAM_JUNDETAIL_RESULTC    = {         5,      5,         5,       5,     5,         5 , nCol_EXCD,   nCol_TEL, nCol_EXCD, nCol_EXCD,  nCol_SNAME,  nCol_SNAME, nCol_SCHK,  nCol_SNAME,   nCol_NAME,  nCol_SNAME,   nCol_NAME, nCol_EXCD, nCol_EXCD };

        public enum enmSel_EXAM_JUNDETAIL_CODE_TYPE { CODE, GUBUN };

        public enum enmSel_EXAM_JUNDETAIL_RESULT_CHART {ORDER_BY, EXAMCODE, EXAMNAME, GBCHAM, GBSD };

        public enum enmSel_EXAM_JUNLOTNO_TEST      {   GUBUN,    JOBDATE, EXAMNAME, SABUNNAME, OLD_LOTNO, NEW_LOTNO, REMARK, OLD_RESULT1, OLD_RESULT2, OLD_RESULT3, OLD_RESULT4, OLD_RESULT5, OLD_RESULT6, OLD_RESULT7, OLD_RESULT8, OLD_RESULT9, OLD_RESULT10, NEW_RESULT1, NEW_RESULT2, NEW_RESULT3, NEW_RESULT4, NEW_RESULT5, NEW_RESULT6, NEW_RESULT7, NEW_RESULT8, NEW_RESULT9, NEW_RESULT10, RESULT, CODE, QTY, QTY_DVSN, QTY_DVSN_NM, JOB_DVSN, JOB_DVSN_NM, EXAM_DVSN_TITLE, EXAM_DVSN1, EXAM_DVSN2, EXAM_DVSN3, EXAM_DVSN4, EXAM_DVSN5, EXAM_DVSN6, EXAM_DVSN7, EXAM_DVSN8, EXAM_DVSN9, EXAM_DVSN10, INPS, INPT_DT, UPPS, UPDT,ROWID  };

        public enum enmSel_EXAM_JUNLOTNO_TEST1      {    JOBDATE,  QTY_DVSN_NM, EXAM_DVSN_TITLE,   JOB_DVSN_NM,    NEW_LOTNO, EXAMNAME,ROWID };
        public string[] sSel_EXAM_JUNLOTNO_TEST1 =  { "작업일자",   "정량구분",      "작업구분", "작업형태", "New Lot No", "검사명", "ROWID" };
        public int[] nSel_EXAM_JUNLOTNO_TEST1     = {  nCol_DATE,    nCol_DPCD,       nCol_PANO, nCol_PANO,    nCol_PANO, nCol_PANO,5};

        string strEXAM_JUNDETAIL_JDATE;
        string strEXAM_JUNDETAIL_SPECNO;
        string strEXAM_JUNDETAIL_RESULT;
        string strEXAM_JUNDETAIL_RESULT_ALY;

        string strEXAM_JUNMULDETAIL_RESULT1;
        string strEXAM_JUNMULDETAIL_RESULT2;

        public enum enmPOLL_DVSN {ALL,EXP, ANA };

        public enum enmSel_EXAM_RESULTC_MIC_POLL     { DEPTCODE,       CNT1,       CNT2,      RATE };
        public string[] sSel_EXAM_RESULTC_MIC_POLL = {   "구분", "검사건수", "오염건수",   "오염율"};
        public int[] nSel_EXAM_RESULTC_MIC_POLL    = { nCol_TIME, nCol_TIME,  nCol_TIME, nCol_TIME };

        public enum enmSel_EXAM_JUNMST_STT     {  SEQNO, GUBUN,  CNT1,  CNT2,  CNT3,  CNT4,  CNT5,  CNT6,  CNT7,  CNT8,  CNT9,  CNT10,  CNT11, CNT12,     TOT };
        public string[] sSel_EXAM_JUNMST_STT = { "SEQNO", "항목", "1월", "2월", "3월", "4월", "5월", "6월", "7월", "8월", "9월", "10월", "11월", "12월", "합계" };
        public int[] nSel_EXAM_JUNMST_STT    = {       5, nCol_NAME,nCol_EXCD, nCol_EXCD, nCol_EXCD, nCol_EXCD, nCol_EXCD, nCol_EXCD, nCol_EXCD, nCol_EXCD, nCol_EXCD, nCol_EXCD, nCol_EXCD, nCol_EXCD, nCol_EXCD };

        public enum enmSel_EXAM_JUNDETAIL_RESULT_CHART_RE     {    JOBDATE,   EXAMCODE, EXAMNAME,   RESULT1,     RSLT_RSN1,   RESULT2,    RSLT_RSN2 };
        public string[] sSel_EXAM_JUNDETAIL_RESULT_CHART_RE = { "작업일자", "검사코드", "검사명", "재검1차", "재검1차사유", "재검2차", "재검1차사유"};
        public int[] nSel_EXAM_JUNDETAIL_RESULT_CHART_RE    = { nCol_NAME, nCol_EXCD, nCol_EXCD,  nCol_EXCD,     nCol_NAME, nCol_EXCD,    nCol_NAME };

        public DataTable sel_EXAM_JUNLOT(PsmhDb pDbCon, string strCODE, string strNOTNO)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "  SELECT                                                                                                                              \r\n";
            SQL += "           CHK                                                                                                                        \r\n";
            SQL += "           , EXAMCODE                                                                                                                 \r\n";
            SQL += "           , EXAMNAME                                                                                                                 \r\n";
            SQL += "           , GBCHAM                                                                                                                   \r\n";
            SQL += "           , GBSD                                                                                                                     \r\n";
            SQL += "           , SEQNO                                                                                                                    \r\n";
            SQL += "           , COMPANY                                                                                                                  \r\n";
            SQL += "           , MATERIAL                                                                                                                 \r\n";
            SQL += "           , CHK_ACCEP                                                                                                                \r\n";
            SQL += "           , DECODE( NVL(NOR,'N'),'N','',NOR   ||'.'|| KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_JUNMULLOT.NOR',NOR)) AS NOR             \r\n";
            SQL += "       FROM (                                                                                                                         \r\n";
            SQL += "  			  SELECT                                                                                                                  \r\n";
            SQL += "  			  		'     ' AS CHK                                                                                                    \r\n";
            SQL += "  			       , C.EXAMCODE                                                                                                       \r\n";
            SQL += "  			       , B.EXAMNAME                                                                                                       \r\n";
            SQL += "  			       , C.GBCHAM                                                                                                         \r\n";
            SQL += "  			       , C.GBSD                                                                                                           \r\n";
            SQL += "  			       , C.SEQNO                                                                                                          \r\n";
            SQL += "  			       , C.COMPANY                                                                                                        \r\n";
            SQL += "  			       , C.MATERIAL                                                                                                       \r\n";
            SQL += "  			       , DECODE( NVL(E.GUBUN,'N'),'N','     ','True')                                                       AS CHK_ACCEP  \r\n";
            SQL += "  			       , ( SELECT MAX(F.NOR)                                                                                              \r\n";
            SQL += "  			             FROM KOSMOS_OCS.EXAM_JUNMULLOT F                                                                             \r\n";
            SQL += "  			            WHERE 1=1                                                                                                     \r\n";
            SQL += "  			              AND F.JANGCODE = " + ComFunc.covSqlstr(strCODE, false);
            SQL += "  			              AND F.LOTNO	 = " + ComFunc.covSqlstr(strNOTNO, false);
            SQL += "  					  ) AS NOR                                                                                                        \r\n";
            SQL += "  			       , C.ROWID                                                                                                          \r\n";
            SQL += "  			  FROM KOSMOS_OCS.EXAM_MASTER    B                                                                                        \r\n";
            SQL += "  			     , KOSMOS_OCS.EXAM_JUNLOT    C                                                                                        \r\n";
            SQL += "  			     , KOSMOS_OCS.EXAM_JUNJANG   D                                                                                        \r\n";
            SQL += "  			     , KOSMOS_OCS.EXAM_JUNMULLOT E                                                                                        \r\n";
            SQL += "  			  WHERE 1=1                                                                                                               \r\n";
            SQL += "  			    AND C.CODE  	= " + ComFunc.covSqlstr(strCODE, false);
            SQL += "  			    AND C.EXAMCODE 	= B.MASTERCODE                                                                                        \r\n";
            SQL += "  			    AND C.LOTNO  	= " + ComFunc.covSqlstr(strNOTNO, false);
            SQL += "  			    AND C.CODE 		= D.CODE                                                                                              \r\n";
            SQL += "  			    AND C.EXAMCODE 	= D.EXAMCODE                                                                                          \r\n";
            SQL += "  			    AND C.CODE		= TRIM(E.JANGCODE(+))                                                                                 \r\n";
            SQL += "  			    AND C.LOTNO		= TRIM(E.LOTNO(+))                                                                                    \r\n";
            SQL += "  			    AND C.EXAMCODE  = TRIM(E.EXAMCODE(+))                                                                                 \r\n";
            SQL += "  		      ORDER BY C.SEQNO, C.EXAMCODE                                                                                            \r\n";
            SQL += "  			)                                                                                                                         \r\n";

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

        public DataSet sel_EXAM_JUNDETAIL_RESULT(PsmhDb pDbCon, string strYYMM, string strCODE, string strNOR, string strLOTNO)
        {
            DataSet ds = null;

            getEXAM_JUNDETAIL_SQL(pDbCon, strYYMM, strCODE, strNOR, strLOTNO);

            DateTime date = new DateTime();

            if (string.IsNullOrEmpty(strYYMM) == true)
            {
                date = DateTime.Now;
            }
            else
            {
                date = Convert.ToDateTime(strYYMM.Substring(0, 4) + "-" + strYYMM.Substring(4, 2) + "-" + "01");
            }

            SQL = "";
            SQL += "SELECT                                                                                                                                                  \r\n";
            //SQL += "	   CHK                                                                                                                                              \r\n";
            SQL += "	   ORDERBY                                                                                                                                         \r\n";
            SQL += "	 , RANGKING                                                                                                                                         \r\n";
            SQL += "	 , EXAMCODE                                                                                                                                         \r\n";
            SQL += "	 , EXAMNAME                                                                                                                                         \r\n";
            SQL += "	 , GBCHAM                                                                                                                                           \r\n";
            SQL += "	 , GBSD                                                                                                                                             \r\n";

            SQL += strEXAM_JUNDETAIL_RESULT_ALY;

            SQL += "	 , DECODE(ORDERBY,0,'MEAN')	     AS MEAN                                                                                                            \r\n";
            SQL += "	 , DECODE(ORDERBY,0,'SD')        AS SD                                                                                                              \r\n";
            SQL += "	 , DECODE(ORDERBY,0,'CV')        AS CV                                                                                                              \r\n";
            SQL += "	 , DECODE(ORDERBY,0,'누적CV',KOSMOS_OCS.FC_EXAM_JUNDETAIL_AVG('" + strCODE + "','" + strLOTNO + "','" + strNOR + "','" + date.AddDays(-1).ToString("yyyy-MM-dd") + "',EXAMCODE)) ACC                                                         \r\n";
            SQL += "	 , DECODE(ORDERBY,0,'CV-누적CV')   AS DIFF                                                                                                          \r\n";
            SQL += "  FROM                                                                                                                                                  \r\n";
            SQL += "  	(                                                                                                                                                   \r\n";
            SQL += "	     SELECT                                                                                                                                         \r\n";
            //SQL += "	      '체크'  AS CHK                                                                                                                                 \r\n";
            SQL += "	       0 AS ORDERBY                                                                                                                                 \r\n";
            SQL += "	     , 0 AS RANGKING                                                                                                                                \r\n";
            SQL += "	     , '검사코드'   AS EXAMCODE                                                                                                                     \r\n";
            SQL += "	     , '검사명'     AS EXAMNAME                                                                                                                     \r\n";
            SQL += "	     , '참고치'     AS GBCHAM                                                                                                                       \r\n";
            SQL += "	     , '±2SD '     AS GBSD                                                                                                                         \r\n";

            SQL += strEXAM_JUNDETAIL_JDATE;

            SQL += "  	 FROM DUAL                                                                                                                                          \r\n";
            SQL += "	UNION ALL                                                                                                                                           \r\n";
            SQL += "	SELECT                                                                                                                                              \r\n";
            //SQL += "	     '체크'  AS CHK                                                                                                                                 \r\n";
            SQL += "	       1 AS ORDERBY                                                                                                                                 \r\n";
            SQL += "	     , 1 AS RANGKING                                                                                                                                \r\n";
            SQL += "	     , '검사코드'   AS EXAMCODE                                                                                                                     \r\n";
            SQL += "	     , '검사명'     AS EXAMNAME                                                                                                                     \r\n";
            SQL += "	     , '참고치'     AS GBCHAM                                                                                                                       \r\n";
            SQL += "	     , '±2SD '     AS GBSD                                                                                                                         \r\n";

            SQL += strEXAM_JUNDETAIL_SPECNO;

            SQL += "	   FROM DUAL                                                                                                                                        \r\n";
            SQL += "	UNION ALL                                                                                                                                           \r\n";
            SQL += "		SELECT                                                                                                                                          \r\n";
            //SQL += "	           'True'       AS CHK                                                                                                                      \r\n";
            SQL += "		        2 		 	AS ORDERBY                                                                                                                  \r\n";
            SQL += "		      , A.SEQNO  	AS RANGKING                                                                                                                 \r\n";
            SQL += "		      , A.EXAMCODE                                                                                                                              \r\n";
            SQL += "		      , B.EXAMNAME                                                                                                                              \r\n";
            SQL += "		      , A.GBCHAM                                                                                                                                \r\n";
            SQL += "		      , A.GBSD                                                                                                                                  \r\n";

            SQL += strEXAM_JUNDETAIL_RESULT;

            SQL += "		 FROM KOSMOS_OCS.EXAM_JUNLOT  	A                                                                                                               \r\n";
            SQL += "		 	, KOSMOS_OCS.EXAM_MASTER  	B                                                                                                               \r\n";
            SQL += "		    , KOSMOS_OCS.EXAM_JUNMST  	D                                                                                                               \r\n";
            SQL += "		    , KOSMOS_OCS.EXAM_JUNDETAIL E                                                                                                               \r\n";
            SQL += "		 WHERE 1=1                                                                                                                                      \r\n";
            SQL += "		   AND E.JOBDATE   BETWEEN TO_DATE('" + strYYMM + "01','YYYY-MM-DD')                                                                                     \r\n";
            SQL += "		                       AND TO_DATE(TO_CHAR(LAST_DAY('" + strYYMM + "01'),'YYYY-MM-DD'),'YYYY-MM-DD')                                                     \r\n";
            SQL += "		   AND E.CODE 		= " + ComFunc.covSqlstr(strCODE, false);
            SQL += "		   AND E.LOTNO 		= " + ComFunc.covSqlstr(strLOTNO, false);
            SQL += "		   AND E.NOR		= " + ComFunc.covSqlstr(strNOR, false);
            SQL += "		   AND E.CODE		= A.CODE                                                                                                                   \r\n";
            SQL += "		   AND E.LOTNO		= A.LOTNO                                                                                                                  \r\n";
            SQL += "		   AND E.EXAMCODE   = A.EXAMCODE                                                                                                               \r\n";
            SQL += "		   AND E.EXAMCODE   = B.MASTERCODE                                                                                                             \r\n";
            SQL += "		   AND E.JOBDATE    = D.JOBDATE                                                                                                                \r\n";
            SQL += "		   AND E.CODE		= D.CODE                                                                                                                   \r\n";
            SQL += "		   AND E.LOTNO		= D.LOTNO                                                                                                                  \r\n";
            SQL += "		   AND E.SPECNO		= D.SPECNO                                                                                                                 \r\n";
            SQL += "		   AND A.EXAMCODE 	= B.MASTERCODE                                                                                                             \r\n";
            SQL += "		   AND A.CODE 		= D.CODE                                                                                                                   \r\n";
            SQL += "		   AND A.LOTNO 		= D.LOTNO                                                                                                                  \r\n";
            SQL += "	     GROUP BY A.SEQNO, A.EXAMCODE, B.EXAMNAME, A.GBCHAM, A.GBSD, E.JOBDATE,E.SPECNO ,E.RESULT, A.LOTNO                                             \r\n";
            SQL += "                                                                                                                                                        \r\n";
            SQL += "  )                                                                                                                                                     \r\n";
            SQL += "  GROUP BY ORDERBY, RANGKING, EXAMCODE, EXAMNAME, GBCHAM, GBSD                                                                                          \r\n";
            SQL += "  ORDER BY ORDERBY, RANGKING, EXAMCODE                                                                                                                  \r\n";


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

        public DataSet sel_EXAM_JUNMST_STT_EXAMCODE(PsmhDb pDbCon, string strYYYYY, string strGUBUN)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  WITH T AS(                                                                                                                                    \r\n";
            SQL += "   SELECT                                                                                                                                       \r\n";
            SQL += "   		  X.EXAMCODE                                                                                                                            \r\n";
            SQL += "   		, NVL(SUM(X.CNT1) ,0) CNT1                                                                                                              \r\n";
            SQL += "   		, NVL(SUM(X.CNT2) ,0) CNT2                                                                                                              \r\n";
            SQL += "   		, NVL(SUM(X.CNT3) ,0) CNT3                                                                                                              \r\n";
            SQL += "   		, NVL(SUM(X.CNT4) ,0) CNT4                                                                                                              \r\n";
            SQL += "  	 	, NVL(SUM(X.CNT5) ,0) CNT5                                                                                                              \r\n";
            SQL += "  	 	, NVL(SUM(X.CNT6) ,0) CNT6                                                                                                              \r\n";
            SQL += "  	 	, NVL(SUM(X.CNT7) ,0) CNT7                                                                                                              \r\n";
            SQL += "  	 	, NVL(SUM(X.CNT8) ,0) CNT8                                                                                                              \r\n";
            SQL += "  	 	, NVL(SUM(X.CNT9) ,0) CNT9                                                                                                              \r\n";
            SQL += "  	 	, NVL(SUM(X.CNT10),0) CNT10                                                                                                             \r\n";
            SQL += "  	 	, NVL(SUM(X.CNT11),0) CNT11                                                                                                             \r\n";
            SQL += "  	 	, NVL(SUM(X.CNT12),0) CNT12                                                                                                             \r\n";
            SQL += "  	 FROM (                                                                                                                                     \r\n";
            SQL += "  	 		SELECT                                                                                                                              \r\n";
            SQL += "  	 				  A.EXAMCODE                                                                                                                \r\n";
            SQL += "  	 				, DECODE(TO_CHAR(JOBDATE,'MM'),'01', COUNT(JOBDATE)) CNT1                                                                   \r\n";
            SQL += "  				    , DECODE(TO_CHAR(JOBDATE,'MM'),'02', COUNT(JOBDATE)) CNT2                                                                   \r\n";
            SQL += "  				    , DECODE(TO_CHAR(JOBDATE,'MM'),'03', COUNT(JOBDATE)) CNT3                                                                   \r\n";
            SQL += "  				    , DECODE(TO_CHAR(JOBDATE,'MM'),'04', COUNT(JOBDATE)) CNT4                                                                   \r\n";
            SQL += "  				    , DECODE(TO_CHAR(JOBDATE,'MM'),'05', COUNT(JOBDATE)) CNT5                                                                   \r\n";
            SQL += "  				    , DECODE(TO_CHAR(JOBDATE,'MM'),'06', COUNT(JOBDATE)) CNT6                                                                   \r\n";
            SQL += "  				    , DECODE(TO_CHAR(JOBDATE,'MM'),'07', COUNT(JOBDATE)) CNT7                                                                   \r\n";
            SQL += "  				    , DECODE(TO_CHAR(JOBDATE,'MM'),'08', COUNT(JOBDATE)) CNT8                                                                   \r\n";
            SQL += "  				    , DECODE(TO_CHAR(JOBDATE,'MM'),'09', COUNT(JOBDATE)) CNT9                                                                   \r\n";
            SQL += "  				    , DECODE(TO_CHAR(JOBDATE,'MM'),'10', COUNT(JOBDATE)) CNT10                                                                  \r\n";
            SQL += "  				    , DECODE(TO_CHAR(JOBDATE,'MM'),'11', COUNT(JOBDATE)) CNT11                                                                  \r\n";
            SQL += "  				    , DECODE(TO_CHAR(JOBDATE,'MM'),'12', COUNT(JOBDATE)) CNT12                                                                  \r\n";
            SQL += "  	       FROM KOSMOS_OCS.EXAM_JUNDETAIL 	A                                                                                                   \r\n";
            SQL += "  	       	  , KOSMOS_OCS.EXAM_JUNCODE 	B                                                                                                   \r\n";
            SQL += "  	       WHERE 1=1                                                                                                                            \r\n";
            SQL += "  			 AND A.JOBDATE BETWEEN TO_DATE('" + strYYYYY + "-01-01','YYYY-MM-DD')                                                               \r\n";
            SQL += "  			                   AND TO_DATE('" + strYYYYY + "-12-31','YYYY-MM-DD')                                                               \r\n";
            SQL += "  	         AND A.CODE = B.CODE                                                                                                                \r\n";

            if (string.IsNullOrEmpty(strGUBUN) == false)
            {
                SQL += "  				         AND B.GUBUN = " + ComFunc.covSqlstr(strGUBUN, false);
            }

            SQL += "  	       GROUP BY A.EXAMCODE,TO_CHAR(JOBDATE,'MM'), B.GUBUN                                                                                   \r\n";
            SQL += "  	       ) X                                                                                                                                  \r\n";
            SQL += "  	 GROUP BY X.EXAMCODE                                                                                                                        \r\n";
            SQL += "  	 ORDER BY X.EXAMCODE                                                                                                                        \r\n";
            SQL += "  	 )                                                                                                                                          \r\n";
            SQL += "  SELECT                                                                                                                                        \r\n";
            SQL += "       GROUPING(EXAMCODE) AS SEQNO                                                                                                                \r\n";
            SQL += "  	  ,	DECODE(GROUPING(EXAMCODE),1,'합계',  KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(EXAMCODE)) AS GUBUN                                         \r\n";
            SQL += "      , TO_CHAR(SUM(CNT1 ),'FM999,999') AS CNT1                                                                                                          \r\n";
            SQL += "      , TO_CHAR(SUM(CNT2 ),'FM999,999') AS CNT2                                                                                                          \r\n";
            SQL += "      , TO_CHAR(SUM(CNT3 ),'FM999,999') AS CNT3                                                                                                          \r\n";
            SQL += "  	  , TO_CHAR(SUM(CNT4 ),'FM999,999') AS CNT4                                                                                                          \r\n";
            SQL += "  	  , TO_CHAR(SUM(CNT5 ),'FM999,999') AS CNT5                                                                                                          \r\n";
            SQL += "  	  , TO_CHAR(SUM(CNT6 ),'FM999,999') AS CNT6                                                                                                          \r\n";
            SQL += "  	  , TO_CHAR(SUM(CNT7 ),'FM999,999') AS CNT7                                                                                                          \r\n";
            SQL += "  	  , TO_CHAR(SUM(CNT8 ),'FM999,999') AS CNT8                                                                                                          \r\n";
            SQL += "  	  , TO_CHAR(SUM(CNT9 ),'FM999,999') AS CNT9                                                                                                          \r\n";
            SQL += "  	  , TO_CHAR(SUM(CNT10),'FM999,999') AS CNT10                                                                                                          \r\n";
            SQL += "      , TO_CHAR(SUM(CNT11),'FM999,999') AS CNT11                                                                                                          \r\n";
            SQL += "      , TO_CHAR(SUM(CNT12),'FM999,999') AS CNT12                                                                                                          \r\n";
            SQL += "       , TO_CHAR(SUM(CNT1)+SUM(CNT2)+SUM(CNT3)+SUM(CNT4)+SUM(CNT5)+SUM(CNT6)+SUM(CNT7)+SUM(CNT8)+SUM(CNT9)+SUM(CNT10)+SUM(CNT11)+SUM(CNT12),'FM999,999') AS TOT  \r\n";
            SQL += "                                                                                                                                                \r\n";
            SQL += "  	FROM T                                                                                                                                      \r\n";
            SQL += "  	GROUP BY ROLLUP  (EXAMCODE)                                                                                                                 \r\n";

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

        public DataSet sel_EXAM_JUNMST_STT_GUBUN(PsmhDb pDbCon, string strYYYYY, bool isGUBUN, string strGUBUN)
        {
            DataSet ds = null;

            SQL = "";                                                                                                                                                     
            SQL +=  "  WITH T AS (                                                                                                                       \r\n";
            SQL +=  "  			 SELECT                                                                                                                  \r\n";
            SQL +=  "  			 		 X.GUBUN  AS GUBUN                                                                                               \r\n";
            SQL +=  "  			 		, NVL(SUM(X.CNT1),0) CNT1                                                                                               \r\n";
            SQL += "  			 		, NVL(SUM(X.CNT2),0) CNT2                                                                                               \r\n";
            SQL += "  			 		, NVL(SUM(X.CNT3),0) CNT3                                                                                               \r\n";
            SQL += "  			 		, NVL(SUM(X.CNT4),0) CNT4                                                                                               \r\n";
            SQL += "  			 		, NVL(SUM(X.CNT5),0) CNT5                                                                                               \r\n";
            SQL += "  			 		, NVL(SUM(X.CNT6),0) CNT6                                                                                               \r\n";
            SQL += "  			 		, NVL(SUM(X.CNT7),0) CNT7                                                                                               \r\n";
            SQL += "  			 		, NVL(SUM(X.CNT8),0) CNT8                                                                                               \r\n";
            SQL += "  					, NVL(SUM(X.CNT9),0) CNT9                                                                                               \r\n";
            SQL += "  					, NVL(SUM(X.CNT10),0) CNT10                                                                                             \r\n";
            SQL += "  					, NVL(SUM(X.CNT11),0) CNT11                                                                                             \r\n";
            SQL += "  					, NVL(SUM(X.CNT12),0) CNT12                                                                                             \r\n";
            SQL +=  "  			  FROM (                                                                                                                 \r\n";
            SQL +=  "  				  	SELECT                                                                                                           \r\n";

            if (isGUBUN == true)
            {
                SQL += "  				  		   B.GUBUN  AS GUBUN                                                                                     \r\n";
            }
            else
            {
                SQL += "  				  		   B.NAME	AS GUBUN                                                                                     \r\n";
            }
                                    
            SQL +=  "  				  		  , DECODE(TO_CHAR(JOBDATE,'MM'),'01', COUNT(JOBDATE)) CNT1                                                  \r\n";
            SQL +=  "  					      , DECODE(TO_CHAR(JOBDATE,'MM'),'02', COUNT(JOBDATE)) CNT2                                                  \r\n";
            SQL +=  "  					      , DECODE(TO_CHAR(JOBDATE,'MM'),'03', COUNT(JOBDATE)) CNT3                                                  \r\n";
            SQL +=  "  					      , DECODE(TO_CHAR(JOBDATE,'MM'),'04', COUNT(JOBDATE)) CNT4                                                  \r\n";
            SQL +=  "  					      , DECODE(TO_CHAR(JOBDATE,'MM'),'05', COUNT(JOBDATE)) CNT5                                                  \r\n";
            SQL +=  "  					      , DECODE(TO_CHAR(JOBDATE,'MM'),'06', COUNT(JOBDATE)) CNT6                                                  \r\n";
            SQL +=  "  					      , DECODE(TO_CHAR(JOBDATE,'MM'),'07', COUNT(JOBDATE)) CNT7                                                  \r\n";
            SQL +=  "  					      , DECODE(TO_CHAR(JOBDATE,'MM'),'08', COUNT(JOBDATE)) CNT8                                                  \r\n";
            SQL +=  "  					      , DECODE(TO_CHAR(JOBDATE,'MM'),'09', COUNT(JOBDATE)) CNT9                                                  \r\n";
            SQL +=  "  					      , DECODE(TO_CHAR(JOBDATE,'MM'),'10', COUNT(JOBDATE)) CNT10                                                 \r\n";
            SQL +=  "  					      , DECODE(TO_CHAR(JOBDATE,'MM'),'11', COUNT(JOBDATE)) CNT11                                                 \r\n";
            SQL +=  "  					      , DECODE(TO_CHAR(JOBDATE,'MM'),'12', COUNT(JOBDATE)) CNT12                                                 \r\n";
            SQL +=  "  				       FROM KOSMOS_OCS.EXAM_JUNMST A                                                                                 \r\n";
            SQL +=  "  				       	  , KOSMOS_OCS.EXAM_JUNCODE B                                                                                \r\n";
            SQL +=  "  				       WHERE 1=1                                                                                                     \r\n";
            SQL +=  "  				         AND A.JOBDATE BETWEEN TO_DATE('" + strYYYYY + "-01-01','YYYY-MM-DD')                                        \r\n";
            SQL += "  				                           AND TO_DATE('" + strYYYYY + "-12-31','YYYY-MM-DD')                                        \r\n";
            SQL +=  "  				         AND A.CODE = B.CODE                                                                                         \r\n";            

            if (string.IsNullOrEmpty(strGUBUN) ==false)
            {
                SQL += "  				         AND B.GUBUN = " + ComFunc.covSqlstr(strGUBUN, false);
            }

            SQL +=  "  				       GROUP BY                                                                                                      \r\n";

            if (isGUBUN == true)
            {
                SQL += "  				       			B.GUBUN                                                                                            \r\n";
            }
            else
            {
                SQL += "  				       			B.NAME                                                                                            \r\n";
            }
                    
            SQL +=  "  				               ,TO_CHAR(JOBDATE,'MM')                                                                                \r\n";
            SQL +=  "  			       ) X                                                                                                               \r\n";
            SQL +=  "  			 GROUP BY GUBUN                                                                                                          \r\n";
            SQL +=  "   )                                                                                                                                \r\n";
            SQL +=  "   SELECT                                                                                                                           \r\n";
            SQL += "     GROUPING(GUBUN) AS SEQNO                                                                                                          \r\n";
            if (isGUBUN == true)
            {
                SQL += "   	    , DECODE(GROUPING(GUBUN),1,'합계',  KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_JUNMULLOT.GUBUN',TRIM(T.GUBUN))) AS GUBUN  \r\n";
            }
            else
            {
                SQL += "  	    , DECODE(GROUPING(GUBUN),1,'합계',  GUBUN) AS GUBUN                                                                           \r\n";
            }

            
            
            SQL += "      , TO_CHAR(SUM(CNT1 ),'FM999,999') AS CNT1                                                                                                          \r\n";
            SQL += "      , TO_CHAR(SUM(CNT2 ),'FM999,999') AS CNT2                                                                                                          \r\n";
            SQL += "      , TO_CHAR(SUM(CNT3 ),'FM999,999') AS CNT3                                                                                                          \r\n";
            SQL += "  	  , TO_CHAR(SUM(CNT4 ),'FM999,999') AS CNT4                                                                                                          \r\n";
            SQL += "  	  , TO_CHAR(SUM(CNT5 ),'FM999,999') AS CNT5                                                                                                          \r\n";
            SQL += "  	  , TO_CHAR(SUM(CNT6 ),'FM999,999') AS CNT6                                                                                                          \r\n";
            SQL += "  	  , TO_CHAR(SUM(CNT7 ),'FM999,999') AS CNT7                                                                                                          \r\n";
            SQL += "  	  , TO_CHAR(SUM(CNT8 ),'FM999,999') AS CNT8                                                                                                          \r\n";
            SQL += "  	  , TO_CHAR(SUM(CNT9 ),'FM999,999') AS CNT9                                                                                                          \r\n";
            SQL += "  	  , TO_CHAR(SUM(CNT10),'FM999,999') AS CNT10                                                                                                          \r\n";
            SQL += "      , TO_CHAR(SUM(CNT11),'FM999,999') AS CNT11                                                                                                          \r\n";
            SQL += "      , TO_CHAR(SUM(CNT12),'FM999,999') AS CNT12                                                                                                          \r\n";
            SQL += "       , TO_CHAR(SUM(CNT1)+SUM(CNT2)+SUM(CNT3)+SUM(CNT4)+SUM(CNT5)+SUM(CNT6)+SUM(CNT7)+SUM(CNT8)+SUM(CNT9)+SUM(CNT10)+SUM(CNT11)+SUM(CNT12),'FM999,999') AS TOT  \r\n";
            SQL +=  "     FROM T                                                                                                                         \r\n";
            SQL +=  "     GROUP BY ROLLUP(GUBUN)                                                                                                         \r\n";
            SQL += "     ORDER BY GUBUN                                                                                                                  \r\n";

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

        public DataSet sel_EXAM_RESULTC_MIC_POLL(PsmhDb pDbCon, string strFdate, string strTdate, enmPOLL_DVSN pEnmPOLL_DVSN, bool isSum, bool isDept )
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                                                         \r\n";

            if (isSum == true)
            {
                SQL += "        ''                            AS DEPTCODE                                               \r\n";
            }
            else
            {
                SQL += "        NVL(TRIM(DEPTCODE),'ZZ.기타') AS DEPTCODE                                               \r\n";
            }
            SQL += "       , TO_CHAR(CNT1) CNT1                                                                                  \r\n";
            SQL += "       , TO_CHAR(CNT2) CNT2                                                                                   \r\n";
            SQL += "       , CASE WHEN CNT1 != '0' THEN TO_CHAR(ROUND((CNT2 / CNT1) * 100,2),'FM90.00') || '%' ELSE '0 %' END AS RATE                         \r\n";
            SQL += "    FROM (                                                                                      \r\n";

            if (isSum == true)
            {
                SQL += "   SELECT                                                                                   \r\n";
                SQL += "   		  SUM(CNT1) CNT1                                                                         \r\n";
            }
            else
            {
                SQL += "   SELECT DEPTCODE                                                                              \r\n";
                SQL += "   		, SUM(CNT1) CNT1                                                                         \r\n";
            }

            SQL += "   		, SUM(CNT2) CNT2                                                                        \r\n";
            SQL += "    FROM (    SELECT                                                                            \r\n";


            if (isSum == true)
            {
                SQL += "    					  COUNT(*) CNT1                                                          \r\n";
            }
            else
            {
                if (isDept == true)
                {
                    SQL += "    					  DEPTCODE                                                              \r\n";
                    SQL += "    					, COUNT(*) CNT1                                                          \r\n";
                }
                else
                {
                    SQL += "    				WARD AS DEPTCODE                                                        \r\n";
                    SQL += "    					, COUNT(*) CNT1                                                          \r\n";
                }

            }
            
            SQL += "    					, 0 CNT2                                                                \r\n";
            SQL += "                FROM KOSMOS_OCS.EXAM_SPECMST A                                                  \r\n";
            SQL += "                   , KOSMOS_OCS.EXAM_RESULTC B                                                  \r\n";
            SQL += "                WHERE A.RECEIVEDATE BETWEEN TO_DATE('" + strFdate + "','YYYY-MM-DD')                  \r\n";
            SQL += "                  					    AND TO_DATE('" + strTdate + " 23:59','YYYY-MM-DD HH24:MI')     \r\n";
            SQL += "       			 AND A.SPECNO 	  = B.SPECNO(+)                                                 \r\n";

            if (pEnmPOLL_DVSN == enmPOLL_DVSN.ALL)
            {
                SQL += "       			 AND B.MASTERCODE IN ( 'MI32', 'MI32B','MI32E','MI33','MI32C')                  \r\n";
                SQL += "       			 AND B.SUBCODE 	  IN ( 'MI32', 'MI32B','MI32E','MI33','MI32C')                  \r\n";
            }
            else if (pEnmPOLL_DVSN == enmPOLL_DVSN.ANA)
            {
                // 협기
                SQL += "       			 AND B.MASTERCODE IN ( 'MI33','MI32C')                                      \r\n";
                SQL += "       			 AND B.SUBCODE 	  IN ( 'MI33','MI32C')                                      \r\n";
            }
            else if (pEnmPOLL_DVSN == enmPOLL_DVSN.EXP)
            {
                // 호기
                SQL += "       			 AND B.MASTERCODE IN ( 'MI32', 'MI32B','MI32E')                  \r\n";
                SQL += "       			 AND B.SUBCODE 	  IN ( 'MI32', 'MI32B','MI32E')                  \r\n";
            }


            SQL += "       			 AND A.SPECCODE   ='010'                                                        \r\n";

            if (isSum == false)
            {
                if (isDept == true)
                {
                    SQL += "                GROUP BY DEPTCODE                                                               \r\n";
                }
                else
                {
                    SQL += "                GROUP BY WARD                                                               \r\n";
                }
            }

            SQL += "       UNION ALL                                                                                \r\n";
            SQL += "       		SELECT                                                                      \r\n";

            if (isSum == true)
            {
                SQL += "       		         0 CNT1                                                                     \r\n";
            }
            else
            {
                if (isDept == true)
                {
                    SQL += "    					  DEPTCODE                                                              \r\n";
                    SQL += "       		       ,  0 CNT1                                                                     \r\n";
                }
                else
                {
                    SQL += "    				WARD AS DEPTCODE                                                        \r\n";
                    SQL += "       		       ,  0 CNT1                                                                     \r\n";
                }
            }
            
            SQL += "       		       , COUNT(*)  CNT2                                                             \r\n";
            SQL += "               FROM KOSMOS_OCS.EXAM_SPECMST A                                                   \r\n";
            SQL += "                  , KOSMOS_OCS.EXAM_RESULTC B                                                   \r\n";
            SQL += "                WHERE A.RECEIVEDATE BETWEEN TO_DATE('" + strFdate + "','YYYY-MM-DD')                  \r\n";
            SQL += "                  					    AND TO_DATE('" + strTdate + " 23:59','YYYY-MM-DD HH24:MI')     \r\n";
            SQL += " 	           AND A.SPECNO	 	= B.SPECNO(+)                                                   \r\n";
            if (pEnmPOLL_DVSN == enmPOLL_DVSN.ALL)
            {
                SQL += "       			 AND B.MASTERCODE IN ( 'MI32', 'MI32B','MI32E','MI33','MI32C')                  \r\n";
                SQL += "       			 AND B.SUBCODE 	  IN ( 'MI32', 'MI32B','MI32E','MI33','MI32C')                  \r\n";
            }
            else if (pEnmPOLL_DVSN == enmPOLL_DVSN.ANA)
            {
                // 협기
                SQL += "       			 AND B.MASTERCODE IN ( 'MI33','MI32C')                                      \r\n";
                SQL += "       			 AND B.SUBCODE 	  IN ( 'MI33','MI32C')                                      \r\n";
            }
            else if (pEnmPOLL_DVSN == enmPOLL_DVSN.EXP)
            {
                // 호기
                SQL += "       			 AND B.MASTERCODE IN ( 'MI32', 'MI32B','MI32E')                  \r\n";
                SQL += "       			 AND B.SUBCODE 	  IN ( 'MI32', 'MI32B','MI32E')                  \r\n";
            }
            SQL += " 	           AND A.SPECCODE 	='010'                                                          \r\n";
            SQL += " 	           AND B.HCODE IN ('ZZ286','ZZ057','ZZ025','ZZ198')                                 \r\n";

            if (isSum == false)
            {
                if (isDept == true)
                {
                    SQL += "                GROUP BY DEPTCODE                                                               \r\n";
                }
                else
                {
                    SQL += "                GROUP BY WARD                                                               \r\n";
                }
                SQL += "              )                                                                                 \r\n";
                SQL += " 		GROUP BY DEPTCODE                                                                       \r\n";
                SQL += " 	)                                                                                           \r\n";
                SQL += " 	GROUP BY  DEPTCODE, CNT1, CNT2                                                              \r\n";
                SQL += "    ORDER BY DEPTCODE                                                                           \r\n";

            }
            else
            {
                SQL += "              )                                                                                 \r\n";
                SQL += " 	)                                                                                           \r\n";
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

        public string ins_EXAM_JUNLOTNO_TEST(PsmhDb pDbCon, DataRow dr, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
            SQL = "";
            SQL += "  INSERT INTO KOSMOS_OCS.EXAM_JUNLOTNO_TEST A(                 \r\n";

            SQL += "       A.GUBUN -- 1.생화학 2.혈액학 3.면역혈청학 4.요검경학       \r\n";
            SQL += "     , A.JOBDATE -- 작업일자                                      \r\n";
            SQL += "     , A.EXAMNAME -- 검사명                                       \r\n";
            SQL += "     , A.SABUNNAME -- 검사자명                                    \r\n";
            SQL += "     , A.OLD_LOTNO -- old lot no                                  \r\n";
            SQL += "     , A.NEW_LOTNO -- new lot no                                  \r\n";
            SQL += "     , A.REMARK -- 기타                                           \r\n";
            SQL += "     , A.OLD_RESULT1 -- old 결과1                                 \r\n";
            SQL += "     , A.OLD_RESULT2 -- old 결과2                                 \r\n";
            SQL += "     , A.OLD_RESULT3 -- old 결과3                                 \r\n";
            SQL += "     , A.OLD_RESULT4 -- old 결과4                                 \r\n";
            SQL += "     , A.OLD_RESULT5 -- old 결과5                                 \r\n";
            SQL += "     , A.OLD_RESULT6 -- old 결과6                                 \r\n";
            SQL += "     , A.OLD_RESULT7 -- old 결과7                                 \r\n";
            SQL += "     , A.OLD_RESULT8 -- old 결과8                                 \r\n";
            SQL += "     , A.OLD_RESULT9 -- old 결과9                                 \r\n";
            SQL += "     , A.OLD_RESULT10 -- old 결과10                               \r\n";
            SQL += "     , A.NEW_RESULT1 -- new 결과1                                 \r\n";
            SQL += "     , A.NEW_RESULT2 -- new 결과2                                 \r\n";
            SQL += "     , A.NEW_RESULT3 -- new 결과3                                 \r\n";
            SQL += "     , A.NEW_RESULT4 -- new 결과4                                 \r\n";
            SQL += "     , A.NEW_RESULT5 -- new 결과5                                 \r\n";
            SQL += "     , A.NEW_RESULT6 -- new 결과6                                 \r\n";
            SQL += "     , A.NEW_RESULT7 -- new 결과7                                 \r\n";
            SQL += "     , A.NEW_RESULT8 -- new 결과8                                 \r\n";
            SQL += "     , A.NEW_RESULT9 -- new 결과9                                 \r\n";
            SQL += "     , A.NEW_RESULT10 -- new 결과10                               \r\n";
            SQL += "     , A.RESULT -- 최종 결과                                      \r\n";
            SQL += "     , A.CODE -- 장비코드                                         \r\n";
            SQL += "     , A.QTY -- 횟수                                              \r\n";
            SQL += "     , A.QTY_DVSN -- 정량정성구분                                 \r\n";
            SQL += "     , A.JOB_DVSN -- 1:단일측정,2:다중측정                        \r\n";
            SQL += "     , A.EXAM_DVSN_TITLE -- 검사방법 타이틀                       \r\n";
            SQL += "     , A.EXAM_DVSN1 -- 검사방법1                                  \r\n";
            SQL += "     , A.EXAM_DVSN2 -- 검사방법2                                  \r\n";
            SQL += "     , A.EXAM_DVSN3 -- 검사방법3                                  \r\n";
            SQL += "     , A.EXAM_DVSN4 -- 검사방법4                                  \r\n";
            SQL += "     , A.EXAM_DVSN5 -- 검사방법5                                  \r\n";
            SQL += "     , A.EXAM_DVSN6 -- 검사방법6                                  \r\n";
            SQL += "     , A.EXAM_DVSN7 -- 검사방법7                                  \r\n";
            SQL += "     , A.EXAM_DVSN8 --                                            \r\n";
            SQL += "     , A.EXAM_DVSN9 -- 검사방법9                                  \r\n";
            SQL += "     , A.EXAM_DVSN10 -- 검사방법10                                \r\n";
            SQL += "     , A.INPS --                                                  \r\n";
            SQL += "     , A.INPT_DT --                                               \r\n";
            SQL += "     , A.UPPS --                                                  \r\n";
            SQL += "     , A.UPDT --                                                  \r\n";
                                                                                   
            SQL += "  )VALUES(                          \r\n";

            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.GUBUN].ToString(), false);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.JOBDATE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAMNAME].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.SABUNNAME].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_LOTNO].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_LOTNO].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.REMARK].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT1].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT2].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT3].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT4].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT5].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT6].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT7].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT8].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT9].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT10].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT1].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT2].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT3].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT4].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT5].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT6].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT7].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT8].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT9].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT10].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.RESULT].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.CODE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.QTY].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.QTY_DVSN].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.JOB_DVSN].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN_TITLE].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN1].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN2].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN3].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN4].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN5].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN6].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN7].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN8].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN9].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN10].ToString(), true);
            SQL += " " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "    , SYSDATE  \r\n";
            SQL += " " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "    , SYSDATE  \r\n";


            SQL += "     )                                                  \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;
        }

        public string up_EXAM_JUNLOTNO_TEST(PsmhDb pDbCon, DataRow dr, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_JUNLOTNO_TEST  \r\n";
            SQL += "    SET                                 \r\n";
            //SQL += "         GUBUN        = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.GUBUN].ToString(), false);    /* 1.생화학 2.혈액학 3.면역혈청학 4.요검경학*/
            //SQL += "       , JOBDATE      = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.JOBDATE].ToString(), false);    /* 작업일자*/
            //SQL += "       , EXAMNAME     = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAMNAME].ToString(), false);  /* 검사명*/
            //SQL += "       , SABUNNAME    = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.SABUNNAME].ToString(), false);    /* 검사자명*/
            SQL += "         OLD_LOTNO    = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_LOTNO].ToString(), false);    /* old lot no*/
            SQL += "       , NEW_LOTNO    = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_LOTNO].ToString(), false);    /* new lot no*/
            SQL += "       , REMARK       = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.REMARK].ToString(), false);  /* 기타*/
            SQL += "       , OLD_RESULT1  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT1].ToString(), false);    /* old 결과1*/
            SQL += "       , OLD_RESULT2  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT2].ToString(), false);    /* old 결과2*/
            SQL += "       , OLD_RESULT3  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT3].ToString(), false);    /* old 결과3*/
            SQL += "       , OLD_RESULT4  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT4].ToString(), false);    /* old 결과4*/
            SQL += "       , OLD_RESULT5  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT5].ToString(), false);    /* old 결과5*/
            SQL += "       , OLD_RESULT6  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT6].ToString(), false);    /* old 결과6*/
            SQL += "       , OLD_RESULT7  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT7].ToString(), false);    /* old 결과7*/
            SQL += "       , OLD_RESULT8  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT8].ToString(), false);    /* old 결과8*/
            SQL += "       , OLD_RESULT9  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT9].ToString(), false);    /* old 결과9*/
            SQL += "       , OLD_RESULT10 = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.OLD_RESULT10].ToString(), false);  /* old 결과10*/
            SQL += "       , NEW_RESULT1  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT1].ToString(), false);    /* new 결과1*/
            SQL += "       , NEW_RESULT2  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT2].ToString(), false);    /* new 결과2*/
            SQL += "       , NEW_RESULT3  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT3].ToString(), false);    /* new 결과3*/
            SQL += "       , NEW_RESULT4  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT4].ToString(), false);    /* new 결과4*/
            SQL += "       , NEW_RESULT5  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT5].ToString(), false);    /* new 결과5*/
            SQL += "       , NEW_RESULT6  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT6].ToString(), false);    /* new 결과6*/
            SQL += "       , NEW_RESULT7  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT7].ToString(), false);    /* new 결과7*/
            SQL += "       , NEW_RESULT8  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT8].ToString(), false);    /* new 결과8*/
            SQL += "       , NEW_RESULT9  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT9].ToString(), false);    /* new 결과9*/
            SQL += "       , NEW_RESULT10 = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.NEW_RESULT10].ToString(), false);  /* new 결과10*/
            SQL += "       , RESULT       = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.RESULT].ToString(), false);  /* 최종 결과*/
            //SQL += "       , CODE         = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.CODE].ToString(), false);  /* 장비코드*/
            SQL += "       , QTY          = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.QTY].ToString(), false);    /* 횟수*/
            SQL += "       , QTY_DVSN     = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.QTY_DVSN].ToString(), false);  /* 정량정성구분*/
            SQL += "       , JOB_DVSN     = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.JOB_DVSN].ToString(), false);  /* 1:단일측정,2:다중측정*/
            SQL += "       , EXAM_DVSN_TITLE = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN_TITLE].ToString(), false);    /* 검사방법 타이틀*/
            SQL += "       , EXAM_DVSN1   = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN1].ToString(), false);  /* 검사방법1*/
            SQL += "       , EXAM_DVSN2   = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN2].ToString(), false);  /* 검사방법2*/
            SQL += "       , EXAM_DVSN3   = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN3].ToString(), false);  /* 검사방법3*/
            SQL += "       , EXAM_DVSN4   = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN4].ToString(), false);  /* 검사방법4*/
            SQL += "       , EXAM_DVSN5   = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN5].ToString(), false);  /* 검사방법5*/
            SQL += "       , EXAM_DVSN6   = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN6].ToString(), false);  /* 검사방법6*/
            SQL += "       , EXAM_DVSN7   = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN7].ToString(), false);  /* 검사방법7*/
            SQL += "       , EXAM_DVSN8   = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN8].ToString(), false);  /* 검사방법8*/
            SQL += "       , EXAM_DVSN9   = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN9].ToString(), false);  /* 검사방법9*/
            SQL += "       , EXAM_DVSN10  = " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.EXAM_DVSN10].ToString(), false); /* 검사방법10*/
            SQL += "       , UPPS =        " + ComFunc.covSqlstr(clsType.User.IdNumber, false);  /* */
            SQL += "       , UPDT =  SYSDATE    \r\n      ";


            SQL += "  WHERE 1=1                      \r\n";
            SQL += "    AND ROWID    	= " + ComFunc.covSqlstr(dr[(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNLOTNO_TEST.ROWID].ToString(), false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string del_EXAM_JUNLOTNO_TEST(PsmhDb pDbCon, string strROWID, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " DELETE KOSMOS_OCS.EXAM_JUNLOTNO_TEST  \r\n";
            SQL += "  WHERE 1=1                      \r\n";
            SQL += "    AND ROWID    	= " + ComFunc.covSqlstr(strROWID, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public DataSet sel_EXAM_JUNLOTNO_TEST(PsmhDb pDbCon,string strFdate, string strTdate, string strQTY_DVSN, string strGUBUN)
        {
            DataSet ds = null;
            
            SQL = "";            
            SQL += " SELECT                                                       \r\n";
            SQL += "      KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_JUNMULLOT.GUBUN',TRIM(A.GUBUN)) AS GUBUN  -- 1.생화학 2.혈액학 3.면역혈청학 4.요검경학    \r\n";
            SQL += "    , TO_CHAR(A.JOBDATE,'YYYY-MM-DD') AS JOBDATE -- 작업일자                                   \r\n";
            SQL += "    , A.EXAMNAME -- 검사명                                    \r\n";
            SQL += "    , A.SABUNNAME -- 검사자명                                 \r\n";
            SQL += "    , A.OLD_LOTNO -- old lot no                               \r\n";
            SQL += "    , A.NEW_LOTNO -- new lot no                               \r\n";
            SQL += "    , A.REMARK -- 기타                                        \r\n";
            SQL += "    , A.OLD_RESULT1 -- old 결과1                              \r\n";
            SQL += "    , A.OLD_RESULT2 -- old 결과2                              \r\n";
            SQL += "    , A.OLD_RESULT3 -- old 결과3                              \r\n";
            SQL += "    , A.OLD_RESULT4 -- old 결과4                              \r\n";
            SQL += "    , A.OLD_RESULT5 -- old 결과5                              \r\n";
            SQL += "    , A.OLD_RESULT6 -- old 결과6                              \r\n";
            SQL += "    , A.OLD_RESULT7 -- old 결과7                              \r\n";
            SQL += "    , A.OLD_RESULT8 -- old 결과8                              \r\n";
            SQL += "    , A.OLD_RESULT9 -- old 결과9                              \r\n";
            SQL += "    , A.OLD_RESULT10 -- old 결과10                            \r\n";
            SQL += "    , A.NEW_RESULT1 -- new 결과1                              \r\n";
            SQL += "    , A.NEW_RESULT2 -- new 결과2                              \r\n";
            SQL += "    , A.NEW_RESULT3 -- new 결과3                              \r\n";
            SQL += "    , A.NEW_RESULT4 -- new 결과4                              \r\n";
            SQL += "    , A.NEW_RESULT5 -- new 결과5                              \r\n";
            SQL += "    , A.NEW_RESULT6 -- new 결과6                              \r\n";
            SQL += "    , A.NEW_RESULT7 -- new 결과7                              \r\n";
            SQL += "    , A.NEW_RESULT8 -- new 결과8                              \r\n";
            SQL += "    , A.NEW_RESULT9 -- new 결과9                              \r\n";
            SQL += "    , A.NEW_RESULT10 -- new 결과10                            \r\n";
            SQL += "    , A.RESULT -- 최종 결과                                   \r\n";
            SQL += "    , (SELECT CODE ||'.'||NAME FROM KOSMOS_OCS.EXAM_JUNCODE WHERE CODE = A.CODE ) AS code -- 장비코드                                      \r\n";
            SQL += "    , A.QTY -- 횟수                                           \r\n";
            SQL += "    , A.QTY_DVSN    -- 정량정성구분                              \r\n";
            SQL += "    , DECODE(A.QTY_DVSN,'1','정량','2','정성') AS QTY_DVSN_NM -- 정량정성구분명                              \r\n";
            SQL += "    , A.JOB_DVSN -- 작업구분:1.단일측정, 2.다중측정                   \r\n";
            SQL += "    , DECODE(A.JOB_DVSN,'1','1.단일측정','2','2.다중측정','1.단일측정') AS JOB_DVSN_NM -- 작업구분                              \r\n";

            SQL += "    , A.EXAM_DVSN_TITLE -- 검사방법 타이틀                    \r\n";
            SQL += "    , A.EXAM_DVSN1 -- 검사방법1                               \r\n";
            SQL += "    , A.EXAM_DVSN2 -- 검사방법2                               \r\n";
            SQL += "    , A.EXAM_DVSN3 -- 검사방법3                               \r\n";
            SQL += "    , A.EXAM_DVSN4 -- 검사방법4                               \r\n";
            SQL += "    , A.EXAM_DVSN5 -- 검사방법5                               \r\n";
            SQL += "    , A.EXAM_DVSN6 -- 검사방법6                               \r\n";
            SQL += "    , A.EXAM_DVSN7 -- 검사방법7                               \r\n";
            SQL += "    , A.EXAM_DVSN8 -- 검사방법8                               \r\n";
            SQL += "    , A.EXAM_DVSN9 -- 검사방법9                               \r\n";
            SQL += "    , A.EXAM_DVSN10 -- 검사방법10                             \r\n";
            SQL += "    , A.INPS --                                               \r\n";
            SQL += "    , A.INPT_DT --                                            \r\n";
            SQL += "    , A.UPPS --                                               \r\n";
            SQL += "    , A.UPDT --                                               \r\n";                                                                   
            SQL += " 	, A.ROWID                                                               \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_JUNLOTNO_TEST A                                      \r\n";
            SQL += " WHERE JOBDATE BETWEEN " + ComFunc.covSqlstr(strFdate, false);
            SQL += "                   AND " + ComFunc.covSqlstr(strTdate, false);
            SQL += "   AND GUBUN                = " + ComFunc.covSqlstr(strGUBUN, false);
            SQL += "   AND NVL(QTY_DVSN,'1')    = " + ComFunc.covSqlstr(strQTY_DVSN, false);
            SQL += " ORDER BY GUBUN , JOBDATE                                                   \r\n";

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

        public DataTable sel_EXAM_JUNDETAIL_JOBDAY(PsmhDb pDbCon, string strYYMM, string strCODE, string strNOR, string strLOTNO, string strYYMM2)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "  SELECT TO_CHAR(JOBDATE,'YYYYMMDD') JOBDAY                           \r\n";
            SQL += "        , SPECNO                                                    \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_JUNDETAIL                                  \r\n";
            SQL += "   WHERE JOBDATE BETWEEN TO_DATE('" + strYYMM + "01','YYYYMMDD')     \r\n";

            if (string.IsNullOrEmpty(strYYMM2) == false)
            {
                SQL += "                     AND TO_DATE(TO_CHAR(LAST_DAY(' " + strYYMM2 + "01" + "'),'YYYYMMDD'),'YYYYMMDD')  \r\n";
            }
            else
            {
                SQL += "                     AND TO_DATE(TO_CHAR(LAST_DAY(' " + strYYMM + "01" + "'),'YYYYMMDD'),'YYYYMMDD')  \r\n";
            }

            SQL += "     AND CODE   = " + ComFunc.covSqlstr(strCODE, false);

            if (string.IsNullOrEmpty(strNOR) == false)
            {
                SQL += "     AND NOR    = " + ComFunc.covSqlstr(strNOR, false);
            }

            if (string.IsNullOrEmpty(strLOTNO) == false)
            {
                SQL += "     AND LOTNO  = " + ComFunc.covSqlstr(strLOTNO, false);
            }

            SQL += "   GROUP BY TO_CHAR(JOBDATE,'YYYYMMDD'), SPECNO                       \r\n";
            SQL += "   ORDER BY JOBDAY, SPECNO                                            \r\n";

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

        public DataTable sel_EXAM_JUNDETAIL_JOBDAY_TERM(PsmhDb pDbCon, string strYYMM, string strCODE, string strNOR, string strLOTNO, string strYYMM2, bool IsOnlyDay = false)
        {
            DataTable dt = null;

            SQL = "";
            if (IsOnlyDay == true)
            {
                SQL += "  SELECT TO_CHAR(JOBDATE,'DD') JOBDAY                           \r\n";
            }
            else
            {
                SQL += "  SELECT TO_CHAR(JOBDATE,'YYYYMMDD') JOBDAY                           \r\n";
            }
            SQL += "    FROM KOSMOS_OCS.EXAM_JUNDETAIL                                  \r\n";
            SQL += "   WHERE JOBDATE BETWEEN TO_DATE('" + strYYMM + "01','YYYYMMDD')     \r\n";

            if (string.IsNullOrEmpty(strYYMM2) == false)
            {
                SQL += "                     AND TO_DATE(TO_CHAR(LAST_DAY(' " + strYYMM2 + "01" + "'),'YYYYMMDD'),'YYYYMMDD')  \r\n";
            }
            else
            {
                SQL += "                     AND TO_DATE(TO_CHAR(LAST_DAY(' " + strYYMM + "01" + "'),'YYYYMMDD'),'YYYYMMDD')  \r\n";
            }

            if (string.IsNullOrEmpty(strLOTNO) == false)
            {
                SQL += "                     AND LOTNO = " + ComFunc.covSqlstr(strLOTNO, false);
            }


            SQL += "     AND CODE   = " + ComFunc.covSqlstr(strCODE, false);

            if (IsOnlyDay == true)
            {
                SQL += "   GROUP BY TO_CHAR(JOBDATE,'DD')                       \r\n";
            }
            else
            {
                SQL += "   GROUP BY TO_CHAR(JOBDATE,'YYYYMMDD')                       \r\n";
            }


            
            SQL += "   ORDER BY JOBDAY                                           \r\n";

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

        void getEXAM_JUNDETAIL_CHART(PsmhDb pDbCon, string strYYMM1, string strCODE, string strNOR, string strLOTNO)
        {
            strEXAM_JUNDETAIL_JDATE = string.Empty;
            strEXAM_JUNDETAIL_SPECNO = string.Empty;
            strEXAM_JUNDETAIL_RESULT = string.Empty;
            strEXAM_JUNDETAIL_RESULT_ALY = string.Empty;

            string strJOBDATE = "";

            DataTable dt = sel_EXAM_JUNDETAIL_JOBDAY_TERM(pDbCon, strYYMM1, strCODE, strNOR, strLOTNO, "",true);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strJOBDATE = dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString();

                    strEXAM_JUNDETAIL_RESULT_ALY += "     , DECODE(ORDER_BY,0,SUM(X.D_" + strJOBDATE + "), DECODE(NVL(SUM(X.D_" + strJOBDATE + "),0),0,0,ROUND((NVL(SUM(X.D_" + strJOBDATE + "),0) - TO_NUMBER(X.GBCHAM)) / TO_NUMBER(X.GBSD) * 2,2)))  AS D_" + strJOBDATE + "\r\n";
                    strEXAM_JUNDETAIL_JDATE      += "            , '" + Convert.ToInt32(strJOBDATE).ToString() + "' AS D_" + strJOBDATE + "\r\n";
                    strEXAM_JUNDETAIL_RESULT     += "            , DECODE(TO_CHAR(A.JOBDATE, 'DD'), '" + strJOBDATE + "', A.RESULT) AS D_" + strJOBDATE + "\r\n";
                }
            }
        }

        public DataTable sel_EXAM_JUNDETAIL_RESULT_CHART(PsmhDb pDbCon, string strYYMM, string strCODE, string strLOTNO, string strNOR, string strCHART)
        {
            DataTable dt = null;

            getEXAM_JUNDETAIL_CHART(pDbCon, strYYMM, strCODE, strNOR, strLOTNO);

            SQL = "";
            SQL += " SELECT                                                                                                             \r\n";                                            
            SQL += "       X.ORDER_BY                                                                                                   \r\n";
            SQL += " 	 , X.EXAMCODE                                                                                                   \r\n";
            SQL += " 	 , X.EXAMNAME                                                                                                   \r\n";
            SQL += " 	 , X.GBCHAM                                                                                                     \r\n";
            SQL += " 	 , X.GBSD                                                                                                       \r\n";

            SQL += strEXAM_JUNDETAIL_RESULT_ALY;

            SQL += "  FROM  (                                                                                                           \r\n";
            SQL += " 		SELECT 0 ORDER_BY                                                                                           \r\n";
            SQL += " 		     , '검사코드' AS EXAMCODE                                                                                         \r\n";
            SQL += " 		     , '검사명칭' AS EXAMNAME                                                                                          \r\n";
            SQL += " 		     , '참고치'   AS GBCHAM                                                                                            \r\n";
            SQL += " 		     , 'SD'       AS GBSD                                                                                              \r\n";

            SQL += strEXAM_JUNDETAIL_JDATE;

            SQL += " 		  FROM DUAL                                                                                                 \r\n";
            SQL += "  		UNION ALL                                                                                                   \r\n";
            SQL += "  		SELECT                                                                                                      \r\n";
            SQL += "  		      1 ORDER_BY                                                                                            \r\n";
            SQL += "  			, A.EXAMCODE                                                                                            \r\n";
            SQL += "  			, B.EXAMNAME                                                                                            \r\n";
            SQL += " 			, D.GBCHAM                                                                                              \r\n";
            SQL += " 			, D.GBSD                                                                                                \r\n";

            SQL += strEXAM_JUNDETAIL_RESULT;

            SQL += " 		 FROM KOSMOS_OCS.EXAM_JUNDETAIL A                                                                           \r\n";
            SQL += " 		 	, KOSMOS_OCS.EXAM_MASTER 	B                                                                           \r\n";
            SQL += " 		    , KOSMOS_OCS.EXAM_JUNJANG 	C                                                                           \r\n";
            SQL += " 		    , KOSMOS_OCS.EXAM_JUNLOT 	D                                                                           \r\n";
            SQL += " 		 WHERE A.JOBDATE BETWEEN TO_DATE('" + strYYMM +"01','YYYY-MM-DD')                                           \r\n";
            SQL += " 			                 AND TO_DATE(TO_CHAR(LAST_DAY('" + strYYMM + "01'),'YYYY-MM-DD'),'YYYY-MM-DD')          \r\n";
            SQL += " 		   AND A.CODE  		= " + ComFunc.covSqlstr(strCODE, false);
            SQL += " 		   AND A.LOTNO 		= " + ComFunc.covSqlstr(strLOTNO, false);
            SQL += " 		   AND A.NOR 		= " + ComFunc.covSqlstr(strNOR, false);
            SQL += " 		   AND A.EXAMCODE 	= D.EXAMCODE                                                                            \r\n";
            SQL += " 		   AND A.EXAMCODE 	= C.EXAMCODE                                                                            \r\n";
            SQL += " 		   AND A.LOTNO 		= D.LOTNO                                                                               \r\n";
            SQL += " 		   AND A.CODE 		= C.CODE                                                                                \r\n";
            SQL += " 		   AND A.CODE 		= D.CODE                                                                                \r\n";
            SQL += " 		   AND A.EXAMCODE   = B.MASTERCODE                                                                          \r\n";
            SQL += " 		   AND C.GBCHART 	= " + ComFunc.covSqlstr(strCHART, false);
            SQL += " 	   ) X                                                                                                          \r\n";
            SQL += "  GROUP BY X.ORDER_BY,X.EXAMCODE, X.EXAMNAME, X.GBCHAM, X.GBSD                                                      \r\n";
            SQL += "  ORDER BY X.ORDER_BY,X.EXAMCODE                                                                                    \r\n";

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

        public DataSet sel_EXAM_JUNDETAIL_RESULT_CHART_RE(PsmhDb pDbCon, string strYYMM, string strCODE, string strLOTNO, string strNOR, string strCHART)
        {
            DataSet ds = null;

            SQL = "";

            SQL += "  SELECT                                                                                        \r\n";
            SQL += "  	      A.JOBDATE                                                                             \r\n";
            SQL += "  	    , A.EXAMCODE                                                                            \r\n";
            SQL += "  		, B.EXAMNAME                                                                            \r\n";
            SQL += "   	    , DECODE( NVL(RESULT1,'*'),'*','', TO_CHAR(ROUND((NVL(TO_NUMBER(RESULT1),0)- TO_NUMBER(D.GBCHAM)) / TO_NUMBER(D.GBSD) * 2,2),'FM9990.09')) AS RESULT1 \r\n";
            SQL += "        , RSLT_RSN1                                                                             \r\n";
            SQL += "        , DECODE( NVL(RESULT2,'*'),'*','', TO_CHAR(ROUND((NVL(TO_NUMBER(RESULT2),0)- TO_NUMBER(D.GBCHAM)) / TO_NUMBER(D.GBSD) * 2,2),'FM9990.09')) AS RESULT2 \r\n";
            SQL += "        , RSLT_RSN2                                                                             \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_JUNDETAIL   A                                                           \r\n";
            SQL += "   	  , KOSMOS_OCS.EXAM_MASTER 	    B                                                           \r\n";
            SQL += "      , KOSMOS_OCS.EXAM_JUNJANG 	C                                                           \r\n";
            SQL += "      , KOSMOS_OCS.EXAM_JUNLOT 	    D                                                           \r\n";
            SQL += "  WHERE 1=1                                                                                     \r\n";
            SQL += " 	AND A.JOBDATE BETWEEN TO_DATE('" + strYYMM + "01','YYYY-MM-DD')                                         \r\n";
            SQL += " 	                 AND TO_DATE(TO_CHAR(LAST_DAY('" + strYYMM + "01'),'YYYY-MM-DD'),'YYYY-MM-DD')          \r\n";
            SQL += " 	AND A.CODE  	= " + ComFunc.covSqlstr(strCODE, false);
            SQL += " 	AND A.LOTNO 	= " + ComFunc.covSqlstr(strLOTNO, false);
            SQL += " 	AND A.NOR 		= " + ComFunc.covSqlstr(strNOR, false);
            SQL += "    AND A.EXAMCODE 	= C.EXAMCODE                                                                \r\n";
            SQL += "    AND A.CODE 		= C.CODE                                                                    \r\n";
            SQL += "    AND A.EXAMCODE  = B.MASTERCODE                                                              \r\n";
            SQL += " 	AND C.GBCHART 	= " + ComFunc.covSqlstr(strCHART, false);
            SQL += "    AND (A.RESULT1 IS NOT NULL OR A.RESULT2 IS NOT NULL)                                        \r\n";
            SQL += "    AND A.EXAMCODE 	= D.EXAMCODE                                                                \r\n";
            SQL += "    AND A.LOTNO 	= D.LOTNO                                                                   \r\n";
            SQL += "    AND A.CODE 		= D.CODE                                                                    \r\n";
            SQL += "  ORDER BY A.JOBDATE, A.EXAMCODE                                                                \r\n";

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

        void getEXAM_JUNDETAIL_TERM(PsmhDb pDbCon, string strYYMM1, string strYYMM2, string strCODE)
        {
            strEXAM_JUNDETAIL_JDATE = string.Empty;
            strEXAM_JUNDETAIL_SPECNO = string.Empty;
            strEXAM_JUNDETAIL_RESULT = string.Empty;
            strEXAM_JUNDETAIL_RESULT_ALY = string.Empty;

            DataTable dt = sel_EXAM_JUNDETAIL_JOBDAY_TERM(pDbCon, strYYMM1, strCODE, "","", strYYMM2);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    strEXAM_JUNDETAIL_RESULT_ALY += "            , MAX(S_" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + ") AS S_" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + "\r\n";
                    strEXAM_JUNDETAIL_RESULT_ALY += "            , MAX(R_" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + ") AS R_" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + "\r\n";

                    strEXAM_JUNDETAIL_JDATE      += "            , '" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + "' AS S_" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + "\r\n";
                    strEXAM_JUNDETAIL_JDATE      += "            , '" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + "' AS R_" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + "\r\n";

                    strEXAM_JUNDETAIL_RESULT     += "            , CASE WHEN TO_CHAR(E.JOBDATE,'YYYYMMDD') = '" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + "' THEN E.SPECNO ELSE '' END AS S_" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + "\r\n";
                    strEXAM_JUNDETAIL_RESULT     += "            , CASE WHEN TO_CHAR(E.JOBDATE,'YYYYMMDD') = '" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + "' THEN E.RESULT ELSE '' END AS R_" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + "\r\n";

                }

            }
        }

        public DataSet sel_EXAM_JUNDETAIL_RESULT_TERM(PsmhDb pDbCon, string strYYMM_FR, string strYYMM_TO, string strCODE)
        {
            DataSet ds = null;

            getEXAM_JUNDETAIL_TERM(pDbCon, strYYMM_FR, strYYMM_TO, strCODE);

            DateTime date = new DateTime();

            if (string.IsNullOrEmpty(strYYMM_TO) == true)
            {
                date = DateTime.Now;
            }
            else
            {
                date = Convert.ToDateTime(strYYMM_TO.Substring(0, 4) + "-" + strYYMM_TO.Substring(4, 2) + "-" + "01");
            }

            SQL = "";
            SQL += " SELECT ORDERBY                                                                                                         \r\n";                                                           
            SQL += "      , SEQNO                                                                                                           \r\n";
            SQL += "      , EXAMCODE                                                                                                        \r\n";
            SQL += "      , EXAMNAME                                                                                                        \r\n";
            SQL += "      , LOTNO                                                                                                           \r\n";
            SQL += "      , GBCHAM                                                                                                          \r\n";
            SQL += "      , GBSD                                                                                                            \r\n";

            SQL += strEXAM_JUNDETAIL_RESULT_ALY;

            SQL += "      , DECODE(ORDERBY,0,'MEAN')	    AS MEAN                                                                                                          \r\n";
            SQL += "      , DECODE(ORDERBY,0,'SD')        AS SD                                                                                                            \r\n";
            SQL += "      , DECODE(ORDERBY,0,'CV')        AS CV                                                                                                            \r\n";
            SQL += "      , DECODE(ORDERBY,0,'누적CV',KOSMOS_OCS.FC_EXAM_JUNDETAIL_AVG_TERM('"+ strCODE + "',LOTNO,'" + date.AddDays(-1).ToString("yyyy-MM-dd") + "',EXAMCODE)) ACC                                                                                                            \r\n";
            SQL += "      , DECODE(ORDERBY,0,'CV-누적CV')   AS DIFF                                                                                                            \r\n";

            SQL += "                                                                                                                        \r\n";
            SQL += "   FROM                                                                                                                 \r\n";
            SQL += "   (                                                                                                                    \r\n";
            SQL += " 	  SELECT                                                                                                            \r\n";
            SQL += " 	         0 			AS ORDERBY                                                                                      \r\n";
            SQL += " 	       , 0		    AS SEQNO                                                                                        \r\n";
            SQL += " 	       , '검사코드' AS EXAMCODE                                                                                     \r\n";
            SQL += " 	       , '검사명'   AS EXAMNAME                                                                                     \r\n";
            SQL += " 	       , 'LOT.NO'	AS LOTNO                                                                                        \r\n";
            SQL += " 	       , '참고치'   AS GBCHAM                                                                                       \r\n";
            SQL += " 	       , '±2SD' 	AS GBSD                                                                                         \r\n";

            SQL += strEXAM_JUNDETAIL_JDATE;

            SQL += " 	  FROM DUAL                                                                                                         \r\n";
            SQL += " 	  UNION ALL                                                                                                           \r\n";
            SQL += " 	 SELECT                                                                                                          \r\n";
            SQL += " 	        1 AS ORDERBY                                                                                            \r\n";
            SQL += " 	      , A.SEQNO                                                                                                 \r\n";
            SQL += " 	      , A.EXAMCODE                                                                                              \r\n";
            SQL += " 	      , B.EXAMNAME                                                                                              \r\n";
            SQL += " 	      , A.LOTNO                                                                                                 \r\n";
            SQL += " 	      , A.GBCHAM                                                                                                \r\n";
            SQL += " 	      , A.GBSD                                                                                                  \r\n";

            SQL += strEXAM_JUNDETAIL_RESULT;

            SQL += " 	 FROM KOSMOS_OCS.EXAM_JUNLOT  	A                                                                               \r\n";
            SQL += " 	 	, KOSMOS_OCS.EXAM_MASTER  	B                                                                               \r\n";
            //SQL += " 	    , KOSMOS_OCS.EXAM_JUNJANG 	C                                                                               \r\n";
            SQL += " 	    , KOSMOS_OCS.EXAM_JUNMST  	D                                                                               \r\n";
            SQL += " 	    , KOSMOS_OCS.EXAM_JUNDETAIL E                                                                               \r\n";
            SQL += " 	WHERE 1=1                                                                                                       \r\n";
            SQL += " 	  AND E.JOBDATE   BETWEEN TO_DATE('" + strYYMM_FR + "01','YYYY-MM-DD')                                          \r\n";
            SQL += " 	                      AND TO_DATE(TO_CHAR(LAST_DAY('" + strYYMM_TO + "01" + "'),'YYYY-MM-DD'),'YYYY-MM-DD')     \r\n";
            SQL += " 	  AND E.CODE        = " + ComFunc.covSqlstr(strCODE, false);
		    SQL += " 	  AND E.CODE		= A.CODE                                                                                    \r\n";                                         
		    SQL += " 	  AND E.LOTNO		= A.LOTNO                                                                                   \r\n";                                
		    SQL += " 	  AND E.EXAMCODE   = A.EXAMCODE                                                                                 \r\n";                               
		    SQL += " 	  AND E.EXAMCODE   = B.MASTERCODE                                                                               \r\n";                               
		    SQL += " 	  AND E.JOBDATE    = D.JOBDATE                                                                                  \r\n";                               
		    SQL += " 	  AND E.CODE		= D.CODE                                                                                    \r\n";                                
		    SQL += " 	  AND E.LOTNO		= D.LOTNO                                                                                   \r\n";                                
		    SQL += " 	  AND E.SPECNO		= D.SPECNO                                                                                  \r\n";                                
		    SQL += " 	  AND A.EXAMCODE 	= B.MASTERCODE                                                                              \r\n";                                
		    SQL += " 	  AND A.CODE 		= D.CODE                                                                                    \r\n";                                
            SQL += " 	  AND A.LOTNO 		= D.LOTNO                                                                                   \r\n";

            //SQL += " 	  AND A.EXAMCODE 	= B.MASTERCODE                                                                              \r\n";
            ////SQL += " 	  AND A.EXAMCODE 	= C.EXAMCODE                                                                                \r\n";
            //SQL += " 	  AND A.EXAMCODE	= E.EXAMCODE                                                                                \r\n";
            ////SQL += " 	  AND A.CODE 		= C.CODE                                                                                    \r\n";
            //SQL += " 	  AND A.CODE 		= D.CODE                                                                                    \r\n";
            //SQL += " 	  AND A.LOTNO 		= D.LOTNO                                                                                   \r\n";
            //SQL += " 	  AND D.JOBDATE	    = E.JOBDATE                                                                                 \r\n";
            //SQL += " 	  AND D.CODE		= E.CODE                                                                                    \r\n";
            //SQL += " 	  AND D.LOTNO		= E.LOTNO                                                                                   \r\n";
            //SQL += " 	  AND D.SPECNO		= E.SPECNO                                                                                  \r\n";
            SQL += "   )                                                                                                                \r\n";
            SQL += "   GROUP BY ORDERBY, SEQNO, EXAMCODE, EXAMNAME, LOTNO, GBCHAM, GBSD                                                 \r\n";
            SQL += "   ORDER BY ORDERBY, LOTNO, SEQNO, EXAMCODE                                                                         \r\n";

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

        public DataTable sel_EXAM_JUNDETAIL_CODE(PsmhDb pDbCon, string strYYMM, enmSel_EXAM_JUNDETAIL_CODE_TYPE pType, string strGUBUN = "", string strYYMM2 = "")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                                     \r\n";

            if (pType == enmSel_EXAM_JUNDETAIL_CODE_TYPE.CODE)
            {
                SQL += "         A.CODE  || '.' || B.NAME                                                           AS CODE     \r\n";
            }
            else if (pType == enmSel_EXAM_JUNDETAIL_CODE_TYPE.GUBUN)
            {
                SQL += "         B.GUBUN || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_JUNMULLOT.GUBUN',B.GUBUN) AS CODE    \r\n";
            }            
                        
            SQL += "   FROM KOSMOS_OCS.EXAM_JUNDETAIL  A                                                                       \r\n";
            SQL += "      , KOSMOS_OCS.EXAM_JUNCODE    B                                                                       \r\n";
            SQL += "  WHERE A.JOBDATE BETWEEN " + ComFunc.covSqlDate(strYYMM + "01", false);
            if (string.IsNullOrEmpty(strYYMM2) == true)
            {                
                SQL += "                      AND TO_DATE(TO_CHAR(LAST_DAY(' " + strYYMM + "01" + "'),'YYYY-MM-DD'),'YYYY-MM-DD')  \r\n";
            }
            else
            {             
                SQL += "                      AND TO_DATE(TO_CHAR(LAST_DAY(' " + strYYMM2 + "01" + "'),'YYYY-MM-DD'),'YYYY-MM-DD')  \r\n";
            }
            SQL += "    AND A.CODE = B.CODE                                                                                    \r\n";

            if (pType == enmSel_EXAM_JUNDETAIL_CODE_TYPE.CODE)
            {
                SQL += "    AND B.GUBUN = " + ComFunc.covSqlstr(strGUBUN, false);
                SQL += " GROUP BY A.CODE, B.NAME                                                                       \r\n";
                SQL += " ORDER BY A.CODE                                                                               \r\n";

            }
            else if (pType == enmSel_EXAM_JUNDETAIL_CODE_TYPE.GUBUN)
            {
                SQL += " GROUP BY B.GUBUN                                                                             \r\n";
                SQL += " ORDER BY B.GUBUN                                                                              \r\n";
            }            

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        public string ins_EXAM_JUNDETAIL(PsmhDb pDbCon, DataRow dr, ref int intRowAffected)
        {
            string SqlErr = string.Empty;
            SQL = "";
            SQL += "  INSERT INTO KOSMOS_OCS.EXAM_JUNDETAIL (                 \r\n";
            SQL += "       JOBDATE      -- 작업일자     \r\n";
            SQL += "     , CODE         -- 장비코드     \r\n";
            SQL += "     , LOTNO        -- LOT번호      \r\n";
            SQL += "     , SPECNO       -- 검체번호     \r\n";
            SQL += "     , NOR          -- 구분         \r\n";
            SQL += "     , EXAMCODE     -- 검사코드     \r\n";
            SQL += "     , RESULT       -- 결과값       \r\n";
            SQL += "     , ENTDATE      -- 등록일자     \r\n";
            SQL += "     , RESULT1      -- 결과값1      \r\n";
            SQL += "     , RESULT2      -- 결과값2      \r\n";
            SQL += "     , RSLT_RSN1    -- 결과사유1    \r\n";
            SQL += "     , RSLT_RSN2    -- 결과사유2    \r\n";
            SQL += "     , INPS         -- 입력자       \r\n";
            SQL += "     , INPT_DT      -- 입력일시     \r\n";
            SQL += "     , UPPS         -- 수정자       \r\n";
            SQL += "     , UPDT         -- 수정일시     \r\n";
            SQL += "  )VALUES(                          \r\n";
            SQL += "     " + ComFunc.covSqlDate(dr[(int)enmSel_EXAM_JUNDETAIL_RESULTC.JOBDATE].ToString(),false);
            SQL += "     " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_JUNDETAIL_RESULTC.CODE].ToString(), true);
            SQL += "     " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_JUNDETAIL_RESULTC.LOTNO].ToString(), true) ;
            SQL += "     " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_JUNDETAIL_RESULTC.SPECNO].ToString(), true) ;
            SQL += "     " + ComFunc.covSqlstr(getGubunText(dr[(int)enmSel_EXAM_JUNDETAIL_RESULTC.NOR].ToString(),"."),true) ;
            SQL += "     " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_JUNDETAIL_RESULTC.SUBCODE].ToString(),true) ;
            SQL += "     " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_JUNDETAIL_RESULTC.RESULT].ToString(),true) ;
            SQL += "    , SYSDATE                      \r\n";
            SQL += "     " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_JUNDETAIL_RESULTC.RESULT1].ToString(),true) ;
            SQL += "     " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_JUNDETAIL_RESULTC.RESULT2].ToString(),true) ;
            SQL += "     " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_JUNDETAIL_RESULTC.RSLT_RSN1].ToString(),true) ;
            SQL += "     " + ComFunc.covSqlstr(dr[(int)enmSel_EXAM_JUNDETAIL_RESULTC.RSLT_RSN2].ToString(), true);
            SQL += "     " + ComFunc.covSqlstr(clsType.User.IdNumber,true) ;
            SQL += "    , SYSDATE                      \r\n";
            SQL += "     " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "    , SYSDATE                      \r\n";

            SQL += "     )                                                  \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;
        }

        public DataSet sel_EXAM_JUNDETAIL_RESULTC(PsmhDb pDbCon, string strSPECNO)
        {
            DataSet ds = null;

            string strCODE = "";

            SQL = "";
            SQL += "   SELECT                                                                                               \r\n";
            SQL += "          D.CODE                                                                                        \r\n";
            SQL += "     FROM KOSMOS_OCS.EXAM_JUNMST	D                                                                   \r\n";
            SQL += "    WHERE D.SPECNO  = " + ComFunc.covSqlstr(strSPECNO, false);

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("정도관리 검체가 아닙니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                strCODE = ds.Tables[0].Rows[0][0].ToString();

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            ds = null;

            SQL = "";
            SQL += "   SELECT                                                                                               \r\n";
            SQL += "          D.SPECNO                                                                                      \r\n";
            SQL += "        , D.CODE                                                                                        \r\n";
            SQL += "        , F.NAME                                                         				AS EQU_NAME     \r\n";
            SQL += "        , D.LOTNO                                                                                       \r\n";
            SQL += "        , D.NOR || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_JUNDETAIL_NOR',D.NOR)  AS NOR          \r\n";
            SQL += "        , TO_CHAR(D.JOBDATE,'YYYY-MM-DD')                                             	AS JOBDATE      \r\n";
            SQL += "        , A.SUBCODE                                                                                     \r\n";
            SQL += "        , KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(A.SUBCODE) 								AS EXAMNAME     \r\n";

//2018.02.01.김홍록: 추가

            SQL += "        , C.GBCHAM                                                                      AS GBCHAM       \r\n";
            SQL += "        , C.GBSD                                                                        AS GBSD         \r\n";

            //SQL += "		, ( CASE WHEN G.RESULT > GBCHAM + GBSD THEN 'H'                                                 \r\n";
            //SQL += "		       WHEN G.RESULT < GBCHAM - GBSD THEN 'L'                                                   \r\n";
            //SQL += "		       ELSE '' END	 )															AS REF          \r\n";

            SQL += "        , A.RESULT                                                                    	AS EQU_RESULT   \r\n";
            SQL += "        , G.RESULT                                                                                      \r\n";

            SQL += "		, DECODE(KOSMOS_OCS.FC_IS_NUMBER(G.RESULT),0,'',                                                \r\n";
            SQL += "		 ( CASE WHEN G.RESULT > GBCHAM + GBSD THEN 'H'                                                  \r\n";
            SQL += "		       WHEN G.RESULT < GBCHAM - GBSD THEN 'L'                                                   \r\n";
            SQL += "		       ELSE '' END	 ))															AS REF          \r\n";

            SQL += "        , G.RESULT1                                                                                     \r\n";
            SQL += "        , G.RSLT_RSN1                                                                                   \r\n";
            SQL += "        , G.RESULT2                                                                                     \r\n";
            SQL += "        , G.RSLT_RSN2                                                                                   \r\n";
            //SQL += "        , G.RESULT3                                                                                     \r\n";
            //SQL += "        , G.RSLT_RSN3                                                                                   \r\n";
            SQL += "        , C.GBCHAM - C.GBSD  													        AS MIN          \r\n";
            SQL += "        , C.GBCHAM + C.GBSD  									    				    AS MAX          \r\n";            
            SQL += "     FROM KOSMOS_OCS.EXAM_RESULTC   A                                                                   \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_JUNLOT    C                                                                   \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_JUNMST	D                                                                   \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_JUNDETAIL G                                                                   \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_JUNCODE   F                                                                   \r\n";
            SQL += "    WHERE 1=1                                                                                           \r\n";
            SQL += "      AND A.SPECNO  = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "      AND A.SPECNO  = D.SPECNO                                                                          \r\n";
            SQL += "      AND A.SUBCODE = C.EXAMCODE                                                                        \r\n";
            SQL += "      AND C.CODE    = F.CODE                                                                            \r\n";
            SQL += "      AND A.SPECNO  = G.SPECNO(+)                                                                       \r\n";
            SQL += "      AND A.SUBCODE = G.EXAMCODE(+)                                                                     \r\n";
            SQL += "      AND C.LOTNO   = D.LOTNO                                                                           \r\n";
            SQL += "      AND G.CODE(+)	= " + ComFunc.covSqlstr(strCODE, false);
            SQL += "      AND D.CODE	= " + ComFunc.covSqlstr(strCODE, false);
            SQL += "      AND C.CODE	= D.CODE                                                                            \r\n";
            SQL += "      AND D.SPECNO  = A.SPECNO                                                                          \r\n";
            SQL += "   ORDER BY C.SEQNO, C.EXAMCODE                                                                         \r\n";
            

            try
            {

                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

        public string del_EXAM_JUNDETAIL(PsmhDb pDbCon, string strSPECNO, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " DELETE KOSMOS_OCS.EXAM_JUNDETAIL  \r\n";
            SQL += "  WHERE 1=1                      \r\n";
            SQL += "    AND SPECNO    	= " + ComFunc.covSqlstr(strSPECNO, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string del_EXAM_RESULTC(PsmhDb pDbCon, string strSPECNO, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " DELETE KOSMOS_OCS.EXAM_RESULTC  \r\n";
            SQL += "  WHERE 1=1                      \r\n";
            SQL += "    AND SPECNO    	= " + ComFunc.covSqlstr(strSPECNO, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string del_EXAM_JUNMST(PsmhDb pDbCon, string strJOBDATE, string strCODE, string strLOTNO, string strSPECNO, string strNor, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " DELETE KOSMOS_OCS.EXAM_JUNMST  \r\n";
            SQL += "  WHERE 1=1                      \r\n";
            SQL += "    AND JOBDATE 	= " + ComFunc.covSqlDate(strJOBDATE, false);
            SQL += "    AND CODE        = " + ComFunc.covSqlstr(strCODE, false);
            SQL += "    AND LOTNO 	    = " + ComFunc.covSqlstr(strLOTNO, false);
            SQL += "    AND SPECNO    	= " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "    AND NOR 		= " + ComFunc.covSqlstr(strNor, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public DataTable sel_EXAM_JUNMST_BARCODE(PsmhDb pDbCon, string strJOBDATE, string strCODE, string strLOTNO, string strNOR)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "  SELECT                                                                                        \r\n";
            SQL += "  		  PTNAME                                                                                \r\n";
            SQL += "  		, EQUCODE                                                                               \r\n";
            SQL += "  		, NOR                                                                                   \r\n";
            SQL += "  		, WC                                                                                    \r\n";
            SQL += "  		, EXAMCODE                                                                              \r\n";
            SQL += "  		, LOTNO                                                                                 \r\n";
            SQL += "  		, SPECNO                                                                                \r\n";
            SQL += "     FROM (                                                                                     \r\n";
            SQL += "  		SELECT                                                                                  \r\n";
            SQL += "  				 '정도관리' 													AS PTNAME       \r\n";
            SQL += "  				, B.NAME               											AS EQUCODE      \r\n";
            SQL += "  				, KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_JUNDETAIL_NOR',A.NOR) 	AS NOR          \r\n";
            SQL += "  				, 'Q' 															AS WC           \r\n";
            SQL += "  				, C.EXAMCODE                                                                    \r\n";
            SQL += "  				, A.LOTNO                                                                       \r\n";
            SQL += "  				, SUBSTR(SPECNO,1,6) ||'-' || SUBSTR(SPECNO,7, 10) 				AS SPECNO       \r\n";
            SQL += "  		  FROM KOSMOS_OCS.EXAM_JUNMST 	A                                                       \r\n";
            SQL += "  		     , KOSMOS_OCS.EXAM_JUNCODE 	B                                                       \r\n";
            SQL += "  		     , KOSMOS_OCS.EXAM_JUNLOT   C                                                       \r\n";
            SQL += "  		 WHERE 1=1                                                                              \r\n";
            SQL += "  		   AND A.CODE 		= B.CODE                                                            \r\n";
            SQL += "  		   AND A.CODE		= C.CODE                                                            \r\n";
            SQL += "  		   AND A.LOTNO		= C.LOTNO                                                           \r\n";
            SQL += "  		   AND A.CODE 		= " + ComFunc.covSqlstr(strCODE, false);
            SQL += "  		   AND A.JOBDATE 	= " + ComFunc.covSqlDate(strJOBDATE, false);
            SQL += "  		   AND A.LOTNO 	    = " + ComFunc.covSqlstr(strLOTNO, false);
            SQL += "  		   AND A.NOR		= " + ComFunc.covSqlstr(strNOR, false);
            SQL += "  		   AND B.USE_YN 	= 'Y'                                                               \r\n";
            SQL += "  		 ORDER BY C.SEQNO, C.EXAMCODE                                                           \r\n";
            SQL += "  	)                                                                                           \r\n";
            SQL += "   WHERE ROWNUM < 6                                                                             \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        public string ins_EXAM_RESULTC_JUNLOT (PsmhDb pDbCon, string strCODE, string strLOTNO, string strSPECNO, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  INSERT INTO KOSMOS_OCS.EXAM_RESULTC (                 \r\n";
            SQL += "  		  SPECNO                                        \r\n";
            SQL += "  		, EQUCODE                                       \r\n";
            SQL += "  		, SEQNO                                         \r\n";
            SQL += "  		, PANO                                          \r\n";
            SQL += "  		, SUBCODE                                       \r\n";
            SQL += "  		, INPS                                          \r\n";
            SQL += "  		, INPT_DT                                       \r\n";
            SQL += "  		, UPPS                                          \r\n";
            SQL += "  		, UPDT                                          \r\n";

            SQL += "  )                                                     \r\n";
            SQL += "  SELECT                                                \r\n";
            SQL += "  		  '" + strSPECNO + "' 		AS SPECNO           \r\n";
            SQL += "  		, CODE 						AS CODE             \r\n";
            SQL += "  		, TO_CHAR(ROWNUM, '000') 	AS SEQNO            \r\n";
            SQL += "  		, " + QC_PTNO + "			AS PANO             \r\n";
            SQL += "  		, EXAMCODE                  AS EXAMCODE         \r\n";

            SQL += "  		, '" + clsType.User.IdNumber + "'               \r\n";
            SQL += "  		, SYSDATE                                       \r\n";
            SQL += "  		, '" + clsType.User.IdNumber + "'               \r\n";
            SQL += "  		, SYSDATE                                       \r\n";

            SQL += "    FROM                                                \r\n";
            SQL += "    (                                                   \r\n";
            SQL += "  	   SELECT                                           \r\n";
            SQL += "  	   	   	    B.EXAMCODE                              \r\n";
            SQL += "  	   	      , B.CODE                                  \r\n";
            SQL += "  	     FROM 	KOSMOS_OCS.EXAM_JUNJANG A               \r\n";
            SQL += "  	          , KOSMOS_OCS.EXAM_JUNLOT 	B               \r\n";
            SQL += "  	    WHERE 1=1                                       \r\n";
            SQL += "  	      AND A.CODE 	   = B.CODE                    \r\n";
            SQL += "  	      AND A.EXAMCODE   = B.EXAMCODE                 \r\n";
            SQL += "  	      AND A.CODE 	   = " + ComFunc.covSqlstr(strCODE , false);
            SQL += "  	      AND B.LOTNO 	   = " + ComFunc.covSqlstr(strLOTNO, false);
            SQL += "  	    ORDER BY B.SEQNO , B.EXAMCODE                   \r\n";
            SQL += "     )                                                  \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;
        }

        public string ins_EXAM_JUNMST(PsmhDb pDbCon, string strJOBDATE, string strCODE, string strLOTNO, string strSPECNO, string strNor, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO KOSMOS_OCS.EXAM_JUNMST                               \r\n";
            SQL += " (                                                                \r\n";
            SQL += "        JOBDATE                                                   \r\n";
            SQL += "      , CODE                                                      \r\n";
            SQL += "      , LOTNO                                                     \r\n";
            SQL += "      , SPECNO                                                    \r\n";
            SQL += "      , ENTDATE                                                   \r\n";
            SQL += "      , NOR                                                       \r\n";
            SQL += "      , INTER                                                     \r\n";
            SQL += " )                                                                \r\n";
            SQL += " SELECT                                                           \r\n";
            SQL += "     " + ComFunc.covSqlDate(strJOBDATE, false);
            SQL += "     " + ComFunc.covSqlstr(strCODE, true);
            SQL += "     " + ComFunc.covSqlstr(strLOTNO, true);
            SQL += "     " + ComFunc.covSqlstr(strSPECNO, true);
            SQL += "     , SYSDATE                                                   \r\n";
            SQL += "     " + ComFunc.covSqlstr(strNor, true);
            SQL += "     , (                                                         \r\n";
            SQL += "          SELECT CASE WHEN '" + strNor + "' = '1' THEN INTER1    \r\n";
            SQL += "                        WHEN '" + strNor + "' = '2' THEN INTER2  \r\n";
            SQL += "                        WHEN '" + strNor + "' = '3' THEN INTER3  \r\n";
            SQL += "          	         END INTER                                   \r\n";
            SQL += "            FROM KOSMOS_OCS.EXAM_JUNNOR                          \r\n";
            SQL += "           WHERE 1=1                                             \r\n";
            SQL += "             AND CODE = " + ComFunc.covSqlstr(strCODE, false);
            SQL += "       )                                                         \r\n";
            SQL += "   FROM DUAL                                                                   \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;
        }

        public DataTable sel_EXAM_JUNMST_SPENO(PsmhDb pDbCon, string strJOBDATE, string strCODE, string strLOTNO, string strNOR, string strTEST)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT  '     ' CHK                                                                        \r\n";
            SQL += "       , '     ' PRT_CHK                                                                    \r\n";
            SQL += "       , JOBDATE                                                                            \r\n";
            SQL += "       , LOTNO                                                                              \r\n";
            SQL += "       , NOR || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_JUNDETAIL_NOR',NOR) AS NOR    \r\n";
            SQL += "       , SPECNO                                                                             \r\n";
            SQL += "       , CODE                                                                               \r\n";
            SQL += "   FROM  (                                                                                  \r\n";
            SQL += " 		 SELECT   A.CODE                                                                    \r\n";
            SQL += " 		 		, TO_CHAR(A.JOBDATE, 'YYYY-MM-DD') AS JOBDATE                               \r\n";
            SQL += " 				, B.NAME                                                                    \r\n";
            SQL += " 				, A.LOTNO                                                                   \r\n";
            SQL += " 				, A.SPECNO                                                                  \r\n";
            SQL += " 				, A.NOR                                                                     \r\n";
            SQL += " 		 FROM KOSMOS_OCS.EXAM_JUNMST  A                                                     \r\n";
            SQL += " 		 	, KOSMOS_OCS.EXAM_JUNCODE B                                                     \r\n";
            SQL += " 		 WHERE 1=1                                                                          \r\n";
            SQL += " 		   AND A.CODE 	    = B.CODE                                                            \r\n";
            SQL += " 		   AND A.CODE   	= " + ComFunc.covSqlstr(strCODE, false);

            if (string.IsNullOrEmpty(strJOBDATE) == false)
            {
                SQL += " 		   AND A.JOBDATE  	= " + ComFunc.covSqlDate(strJOBDATE, false);
            }
            

            if (string.IsNullOrEmpty(strLOTNO) == false)
            {
                SQL += " 		   AND A.LOTNO 	= " + ComFunc.covSqlstr(strLOTNO, false);
            }

            if (string.IsNullOrEmpty(strNOR) == false)
            {
                SQL += " 		   AND A.NOR 	= " + ComFunc.covSqlstr(strNOR, false);
            }

            SQL += " 		 ORDER BY JOBDATE DESC, SPECNO DESC, NOR                                            \r\n";
            SQL += " 	 )                                                                                      \r\n";

            //if (string.IsNullOrEmpty(strJOBDATE) == true)
            //{
                SQL += "  WHERE ROWNUM < 50                                                                         \r\n";
            //}
            

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        public string ins_EXAM_JUNMULDETAIL(PsmhDb pDbCon, string strYYMM, string strSEQNO, string strGUBUN, string strJANGCODE, string strNOR, string strLOTNO, string strEXAMCODE, string strRESULT, ref int intRowAffected)
        {
            string SqlErr = "";
           
            SQL = "";
            SQL += " INSERT INTO KOSMOS_OCS.EXAM_JUNMULDETAIL (YYMM,SEQNO,GUBUN,JANGCODE,NOR,LOTNO,EXAMCODE,RESULT) VALUES ( \r\n";
            SQL += "    " + ComFunc.covSqlstr(strYYMM, false);
            SQL += "    " + ComFunc.covSqlstr(strSEQNO, true);
            SQL += "    " + ComFunc.covSqlstr(strGUBUN, true);
            SQL += "    " + ComFunc.covSqlstr(strJANGCODE, true);
            SQL += "    " + ComFunc.covSqlstr(strNOR, true);
            SQL += "    " + ComFunc.covSqlstr(strLOTNO, true);
            SQL += "    " + ComFunc.covSqlstr(strEXAMCODE, true);
            SQL += "    " + ComFunc.covSqlstr(strRESULT, true);            
            SQL += "    )" ;
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string ins_EXAM_JUNMULDETAIL(PsmhDb pDbCon, string strYYMM, string strGUBUN, string strJANGCODE, string strLOTNO, string strNOR, DataSet ds, ref int intRowAffected)
        {
            string SqlErr = "";

            OracleCommand Cmd = null;

            try
            {
                if (Cmd == null)
                {
                    Cmd = pDbCon.Con.CreateCommand();
                }

                SQL = "";
                SQL += " INSERT INTO KOSMOS_OCS.EXAM_JUNMULDETAIL (YYMM,GUBUN,JANGCODE,NOR,LOTNO,SEQNO,EXAMCODE,RESULT) VALUES ( \r\n";
                SQL += "    " + ComFunc.covSqlstr(strYYMM, false);
                SQL += "    " + ComFunc.covSqlstr(strGUBUN, true);
                SQL += "    " + ComFunc.covSqlstr(strJANGCODE, true);
                SQL += "    " + ComFunc.covSqlstr(strNOR, true);
                SQL += "    " + ComFunc.covSqlstr(strLOTNO, true);
                SQL += "    , :SEQNO \r\n";
                SQL += "    , :EXAMCODE \r\n";
                SQL += "    , :RESULT \r\n";
                SQL += "    )";

                Cmd.CommandText = SQL;
                Cmd.CommandTimeout = 60;

                if (pDbCon.Trs != null)
                {
                    Cmd.Transaction = pDbCon.Trs;
                }

                Cmd.Parameters.Add(":SEQNO"     , OracleDbType.Int32, 2);
                Cmd.Parameters.Add(":EXAMCODE"  , OracleDbType.Varchar2, 8);
                Cmd.Parameters.Add(":RESULT"    , OracleDbType.Varchar2, 10);

                Cmd.Prepare();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Cmd.Parameters[":EXAMCODE"].Value = ds.Tables[0].Rows[i][(int)clsComSupLbExQCSQL.enmSel_EXAM_JUNMULDETAIL_RESULT.EXAMCODE].ToString();

                    for (int j = Enum.GetValues(typeof(enmSel_EXAM_JUNMULDETAIL_RESULT)).Length; j < ds.Tables[0].Columns.Count; j++)
                    {
                        Cmd.Parameters[":SEQNO"].Value      = j- Enum.GetValues(typeof(enmSel_EXAM_JUNMULDETAIL_RESULT)).Length + 1;                    
                        Cmd.Parameters[":RESULT"].Value     = ds.Tables[0].Rows[i][j].ToString();
                        Cmd.ExecuteNonQuery();
                        // ComBase.clsDB.SaveSqlLog(SQL, pDbCon); //Query Log 저장
                    }

                }

                Cmd.Dispose();
                Cmd = null;

            }
            catch (OracleException sqlExc)
            {                
                Cmd.Dispose();
                Cmd = null;

                return sqlExc.Message;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);                
                return ex.Message;
            }
            finally
            {
                if (Cmd != null)
                {
                    Cmd.Dispose();
                    Cmd = null;
                }
            }

            return SqlErr;

        }

        public string del_EXAM_JUNMULDETAIL(PsmhDb pDbCon, string strYYMM, string strGUBUN, string strJANGCODE, string strLOTNO, string strNOR, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " DELETE KOSMOS_OCS.EXAM_JUNMULDETAIL  \r\n";
            SQL += "  WHERE 1=1                      \r\n";
            SQL += "    AND YYMM 		= " + ComFunc.covSqlstr(strYYMM, false);
            SQL += "    AND GUBUN       = " + ComFunc.covSqlstr(strGUBUN, false);
            SQL += "    AND JANGCODE 	= " + ComFunc.covSqlstr(strJANGCODE, false);
            SQL += "    AND LOTNO    	= " + ComFunc.covSqlstr(strLOTNO, false);
            SQL += "    AND NOR 		= " + ComFunc.covSqlstr(strNOR, false);


            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public bool sel_EXAM_JUNMULDETAIL_ISNULL(PsmhDb pDbCon, string strGUBUN, string strJANGCODE, string strLOTNO, string strEXAMCODE = "", string strYYMM = "" )
        {
            bool b = false;

            DataTable dt = null;

            SQL = "";
            SQL += " SELECT COUNT(*) AS CNT                                     \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_JUNMULDETAIL                        \r\n";
            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "    AND GUBUN    = " + ComFunc.covSqlstr(strGUBUN, false);
            SQL += "    AND JANGCODE = " + ComFunc.covSqlstr(strJANGCODE, false);
            SQL += "    AND LOTNO    = " + ComFunc.covSqlstr(strLOTNO, false);

            if (string.IsNullOrEmpty(strEXAMCODE) == false)
            {
                SQL += "    AND EXAMCODE    = " + ComFunc.covSqlstr(strEXAMCODE, false);
            }

            if (string.IsNullOrEmpty(strYYMM) == false)
            {
                SQL += "    AND YYMM    = " + ComFunc.covSqlstr(strYYMM, false);
            }

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return true;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return true;
            }

            if (ComFunc.isDataTableNull(dt) == false)
            {
                if (dt.Rows[0]["CNT"].ToString() == "0")
                {
                    b =  true;
                }
                else
                {
                    b =  false;
                }
            }
            else
            {
                b =  true;
            }

            return b;
        }

        public string del_EXAM_JUNMULLOT(PsmhDb pDbCon, string strGUBUN, string strJANGCODE, string strNOR, string strLOTNO, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " DELETE KOSMOS_OCS.EXAM_JUNMULLOT  \r\n";
            SQL += "  WHERE 1=1                      \r\n";
            SQL += "    AND GUBUN		= " + ComFunc.covSqlstr(strGUBUN, false);
            SQL += "    AND JANGCODE    = " + ComFunc.covSqlstr(strJANGCODE, false);
            SQL += "    AND NOR    	    = " + ComFunc.covSqlstr(strNOR, false);
            SQL += "    AND LOTNO    	= " + ComFunc.covSqlstr(strLOTNO, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string ins_EXAM_JUNMULLOT(PsmhDb pDbCon, string strGUBUN,string strJANGCODE,string strNOR, string strLOTNO, string strEXAMCODE, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " INSERT INTO  KOSMOS_OCS.EXAM_JUNMULLOT ( GUBUN, JANGCODE, NOR, LOTNO, EXAMCODE) VALUES ( \r\n";
            SQL += "    " + ComFunc.covSqlstr(strGUBUN, false);
            SQL += "    " + ComFunc.covSqlstr(strJANGCODE, true);
            SQL += "    " + ComFunc.covSqlstr(strNOR, true);
            SQL += "    " + ComFunc.covSqlstr(strLOTNO, true);
            SQL += "    " + ComFunc.covSqlstr(strEXAMCODE, true);
            SQL += "    )";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public bool sel_EXAM_JUNDETAIL_ISNULL(PsmhDb pDbCon, string strCODE, string strLOTNO, string strEXAMCODE)
        {
            bool b = false;

            DataTable dt = null;

            SQL = "";
            SQL += " SELECT COUNT(*) AS CNT                                     \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_JUNDETAIL                           \r\n";
            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "    AND CODE        = " + ComFunc.covSqlstr(strCODE, false);
            SQL += "    AND LOTNO       = " + ComFunc.covSqlstr(strLOTNO, false);
            SQL += "    AND EXAMCODE    = " + ComFunc.covSqlstr(strEXAMCODE, false);

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return true;
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return true;
            }

            if (ComFunc.isDataTableNull(dt) == false)
            {
                if (dt.Rows[0]["CNT"].ToString() == "0")
                {
                    b =  true;
                }
                else
                {
                    b = false;
                }
            }
            else
            {
                b = true;
            }

            return b;
        }

        public DataTable sel_EXAM_JUNMULLOT_LOTNO(PsmhDb pDbCon, string strGUBUN, string strJANGCODE)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT LOTNO                                               \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_JUNMULLOT                           \r\n";
            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "    AND GUBUN       = " + ComFunc.covSqlstr(strGUBUN, false);
            SQL += "    AND JANGCODE    = " + ComFunc.covSqlstr(strJANGCODE, false);
            SQL += "  GROUP BY LOTNO                                            \r\n";
            SQL += "  ORDER BY LOTNO                                            \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        void getJUNMULDETAIL_SQL(PsmhDb pDbCon, string strYYMM, string strGUBUN, string strJANGCODE, string strNOR, string strLOTNO)
        {

            this.strEXAM_JUNMULDETAIL_RESULT1 = "";
            this.strEXAM_JUNMULDETAIL_RESULT2 = "";

            DataTable dt = sel_EXAM_JUNMULDETAIL_SEQNO(pDbCon, strYYMM, strGUBUN, strJANGCODE, strNOR, strLOTNO);

            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 1; i < dt.Rows.Count + 1; i++)
                {
                    this.strEXAM_JUNMULDETAIL_RESULT1 += "      , DECODE( C.SEQNO, " + i.ToString() + " ,C.RESULT,'')  AS D_" + i.ToString() + "  \r\n";
                    this.strEXAM_JUNMULDETAIL_RESULT2 += "      , SUM(D_"+ i.ToString() + ") AS D_" + i.ToString() + " \r\n";
                }
            }

        }

        public DataSet sel_EXAM_JUNMULDETAIL_RESULT(PsmhDb pDbCon, string strYYMM, string strGUBUN, string strJANGCODE, string strNOR, string strLOTNO, bool isNew)
        {
            DataSet ds = null;

            if (isNew == false)
            {
                getJUNMULDETAIL_SQL(pDbCon, strYYMM, strGUBUN, strJANGCODE, strNOR, strLOTNO);
            }
            

            SQL = "";
            SQL += "  WITH T AS (                                               \r\n";
            SQL += "  		 SELECT A.EXAMCODE                                  \r\n";
            SQL += "  		      , B.EXAMNAME                                  \r\n";
            SQL += "  		      , 0 AS AVG                                    \r\n";
            SQL += "  		      , 0 AS SD2                                    \r\n";
            SQL += "  		      , 0 AS REF1                                   \r\n";
            SQL += "  		      , 0 AS REF2                                   \r\n";

            if (isNew == true)
            {
                SQL += "  		      , 0 AS D_1                                \r\n";
                SQL += "  		      , 0 AS D_2                                \r\n";
                SQL += "  		      , 0 AS D_3                                \r\n";
                SQL += "  		      , 0 AS D_4                                \r\n";
                SQL += "  		      , 0 AS D_5                                \r\n";
            }

            else
            {
                SQL += strEXAM_JUNMULDETAIL_RESULT1;
            }

            SQL += "  		   FROM KOSMOS_OCS.EXAM_JUNMULLOT 		A           \r\n";
            SQL += "  		      , KOSMOS_OCS.EXAM_MASTER    		B           \r\n";
            SQL += "  		      , KOSMOS_OCS.EXAM_JUNMULDETAIL    C           \r\n";
            SQL += "  		  WHERE 1=1                                         \r\n";
            SQL += "  		    AND A.EXAMCODE 		= B.MASTERCODE              \r\n";
            SQL += "  		    AND A.GUBUN  		= " + ComFunc.covSqlstr(strGUBUN, false);
            SQL += "  		    AND A.JANGCODE  	= " + ComFunc.covSqlstr(strJANGCODE, false);
            SQL += "  		    AND A.NOR 			= " + ComFunc.covSqlstr(strNOR, false);
            SQL += "  		    AND A.LOTNO 		= " + ComFunc.covSqlstr(strLOTNO, false);
            SQL += "  		    AND '" + strYYMM  + "' 		= C.YYMM(+)                 \r\n";
            SQL += "  		    AND A.GUBUN			= C.GUBUN(+)                \r\n";
            SQL += "  		    AND A.JANGCODE		= C.JANGCODE(+)             \r\n";
            SQL += "  		    AND A.NOR		  	= C.NOR(+)                  \r\n";
            SQL += "  		    AND TRIM(A.LOTNO) 	= C.LOTNO(+)                \r\n";
            SQL += "  		    AND A.EXAMCODE	  	= C.EXAMCODE(+)             \r\n";
            SQL += "  )                                                         \r\n";
            SQL += "  SELECT  EXAMCODE                                          \r\n";
            SQL += "        , EXAMNAME                                          \r\n";
            SQL += "        , AVG                                               \r\n";
            SQL += "        , SD2                                               \r\n";
            SQL += "        , REF1                                              \r\n";
            SQL += "        , REF2                                              \r\n";

            if (isNew == true)
            {
                SQL += "  		      , 0 AS D_1                                \r\n";
                SQL += "  		      , 0 AS D_2                                \r\n";
                SQL += "  		      , 0 AS D_3                                \r\n";
                SQL += "  		      , 0 AS D_4                                \r\n";
                SQL += "  		      , 0 AS D_5                                \r\n";
            }
            else
            {
                SQL += strEXAM_JUNMULDETAIL_RESULT2;
            }

            SQL += "    FROM T                                                  \r\n";
            SQL += "    GROUP BY EXAMCODE, EXAMNAME, AVG                        \r\n";
            SQL += "        , SD2                                               \r\n";
            SQL += "        , REF1                                              \r\n";
            SQL += "        , REF2                                              \r\n";
            SQL += "  ORDER BY EXAMCODE                                         \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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
       
        public DataTable sel_JUNMULDETAIL_LOTNO(PsmhDb pDbCon, string strYYMM, string strJANGCODE, string strGUBUN)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT LOTNO                                               \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_JUNMULDETAIL                           \r\n";
            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "    AND YYMM     = " + ComFunc.covSqlstr(strYYMM, false);
            SQL += "    AND JANGCODE = " + ComFunc.covSqlstr(strJANGCODE, false);
            SQL += "    AND GUBUN = " + ComFunc.covSqlstr(strGUBUN, false);
            SQL += "  GROUP BY LOTNO                                            \r\n";
            SQL += "  ORDER BY LOTNO                                            \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        public DataTable sel_EXAM_JUNMULLOT_NOR(PsmhDb pDbCon, string strLOTNO, string strJANGCODE)
        {
            DataTable dt = null;

            SQL = "";

            SQL += " SELECT NOR || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_JUNMULLOT.NOR',NOR) CODE   \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_JUNMULLOT                                                       \r\n";
            SQL += "  WHERE 1=1                                                                             \r\n";
            SQL += "    AND JANGCODE = " + ComFunc.covSqlstr(strJANGCODE, false);
            SQL += "    AND LOTNO	 = " + ComFunc.covSqlstr(strLOTNO, false);
            SQL += "  GROUP BY NOR                                                                          \r\n";
            SQL += "  ORDER BY NOR                                                                          \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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
      
        public DataTable sel_EXAM_JUNMULDETAIL_SEQNO(PsmhDb pDbCon, string strYYMM, string strGUBUN, string strJANGCODE, string strNOR, string strLOTNO)
        {
            DataTable dt = null;

            SQL = "";    
            SQL += "  SELECT SEQNO                          \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_JUNMULDETAIL   \r\n";
            SQL += "    WHERE 1=1                           \r\n";
            SQL += "      AND YYMM  	= " + ComFunc.covSqlstr(strYYMM     , false);
            SQL += "      AND GUBUN  	= " + ComFunc.covSqlstr(strGUBUN    , false);
            SQL += "      AND JANGCODE  = " + ComFunc.covSqlstr(strJANGCODE , false);
            SQL += "      AND NOR 		= " + ComFunc.covSqlstr(strNOR      , false);
            SQL += "      AND LOTNO  	= " + ComFunc.covSqlstr(strLOTNO    , false);
            SQL += " GROUP BY SEQNO                         \r\n";
            SQL += " ORDER BY SEQNO                         \r\n";


            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        public DataTable sel_EXAM_JUNDETAIL_LOTNO(PsmhDb pDbCon, string strYYMM, string strCODE, string strYYMM2 ="")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT LOTNO                                               \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_JUNDETAIL                           \r\n";
            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "    AND JOBDATE BETWEEN " + ComFunc.covSqlDate(strYYMM+"01", false);

            if (string.IsNullOrEmpty(strYYMM2) == true)
            {
                SQL += "                    AND TO_DATE(TO_CHAR(LAST_DAY(' " + strYYMM + "01" + "'),'YYYY-MM-DD'),'YYYY-MM-DD') \r\n";
            }
            else
            {
                SQL += "                    AND TO_DATE(TO_CHAR(LAST_DAY(' " + strYYMM2 + "01" + "'),'YYYY-MM-DD'),'YYYY-MM-DD') \r\n";
            }


            SQL += "    AND CODE = " + ComFunc.covSqlstr(strCODE, false);
            SQL += "  GROUP BY LOTNO                                            \r\n";
            SQL += "  ORDER BY LOTNO                                            \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        public DataTable sel_EXAM_JUNDETAIL_LOTNO_NOR(PsmhDb pDbCon, string strYYMM, string strCODE, string strLOTNO, string strYYMM2 ="")
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT NOR || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_JUNDETAIL_NOR',NOR) AS CODE                    \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_JUNDETAIL                           \r\n";
            SQL += "  WHERE 1=1                                                 \r\n";
            SQL += "    AND JOBDATE BETWEEN " + ComFunc.covSqlDate(strYYMM + "01", false);

            if (string.IsNullOrEmpty(strYYMM2) == true)
            {
                SQL += "                    AND TO_DATE(TO_CHAR(LAST_DAY(' " + strYYMM + "01" + "'),'YYYY-MM-DD'),'YYYY-MM-DD') \r\n";
            }
            else
            {
                SQL += "                    AND TO_DATE(TO_CHAR(LAST_DAY(' " + strYYMM2 + "01" + "'),'YYYY-MM-DD'),'YYYY-MM-DD') \r\n";
            }

            
            SQL += "    AND CODE  = " + ComFunc.covSqlstr(strCODE, false);
            SQL += "    AND LOTNO = " + ComFunc.covSqlstr(strLOTNO, false);
            SQL += "  GROUP BY NOR                                            \r\n";
            SQL += "  ORDER BY NOR                                            \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        public DataTable sel_EXAM_JUNNOR(PsmhDb pDbCon, string strCODE)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                         \r\n";
            SQL += "       CODE                     \r\n";
            SQL += "   FROM (                       \r\n";
            SQL += " 	SELECT                      \r\n";
            SQL += " 	       0 ORDERBY            \r\n";
            SQL += " 	     , '1.Normal' AS CODE   \r\n";
            SQL += " 	  FROM KOSMOS_OCS.EXAM_JUNNOR          \r\n";
            SQL += " 	 WHERE CODE     = " + ComFunc.covSqlstr(strCODE, false);
            SQL += " 	   AND NORMAL   = '1'       \r\n";
            SQL += " 	UNION ALL                   \r\n";
            SQL += " 	SELECT                      \r\n";
            SQL += " 	       1 ORDERBY            \r\n";
            SQL += " 	     , '2.LOW' AS CODE      \r\n";
            SQL += " 	  FROM KOSMOS_OCS.EXAM_JUNNOR          \r\n";
            SQL += " 	 WHERE CODE     = " + ComFunc.covSqlstr(strCODE, false);
            SQL += " 	   AND LOW      = '1'       \r\n";
            SQL += " 	UNION ALL                   \r\n";
            SQL += " 	SELECT                      \r\n";
            SQL += " 	       2 ORDERBY            \r\n";
            SQL += " 	     , '3.HIGH' AS CODE     \r\n";
            SQL += " 	  FROM KOSMOS_OCS.EXAM_JUNNOR          \r\n";
            SQL += " 	 WHERE CODE     = " + ComFunc.covSqlstr(strCODE, false);
            SQL += " 	   AND HIGH     = '1'          \r\n";
            SQL += " )                              \r\n";
            SQL += " ORDER BY ORDERBY               \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        void getEXAM_JUNDETAIL_SQL(PsmhDb pDbCon, string strYYMM, string strCODE, string strNOR, string strLOTNO)
        {

            strEXAM_JUNDETAIL_JDATE         = string.Empty;
            strEXAM_JUNDETAIL_SPECNO        = string.Empty;
            strEXAM_JUNDETAIL_RESULT        = string.Empty;
            strEXAM_JUNDETAIL_RESULT_ALY    = string.Empty;

            DataTable dt = sel_EXAM_JUNDETAIL_JOBDAY(pDbCon, strYYMM,strCODE,strNOR,strLOTNO, "");

            if (ComFunc.isDataTableNull(dt) == false)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    strEXAM_JUNDETAIL_RESULT_ALY += "            , MAX(S_" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + ")    AS S_" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + "\r\n";

                    strEXAM_JUNDETAIL_JDATE += "            , '" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + "' AS S_" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + "\r\n";
                    strEXAM_JUNDETAIL_SPECNO += "            " + ComFunc.covSqlstr(dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.SPECNO].ToString(), true);

                    strEXAM_JUNDETAIL_RESULT += "            , CASE WHEN TO_CHAR(E.JOBDATE,'YYYYMMDD') = '" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + "' AND E.SPECNO = '" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.SPECNO].ToString() + "' THEN E.RESULT ELSE '' END AS S_" + dt.Rows[i][(int)enmSel_EXAM_JUNDETAIL_JOBDAY.JOBDAY].ToString() + "\r\n";
                    
                }

            }

        }
                    
        public string ins_EXAM_JUNJANG(PsmhDb pDbCon, string strGUBUN, string strEXAMCODE, string strRANGKING, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " INSERT INTO  KOSMOS_OCS.EXAM_JUNMULCODE ( GUBUN, EXAMCODE, RANGKING) VALUES ( \r\n";
            SQL += "    " + ComFunc.covSqlstr(strGUBUN, false);
            SQL += "    " + ComFunc.covSqlstr(strEXAMCODE, true);
            SQL += "    " + ComFunc.covSqlstr(strRANGKING, true);
            SQL += "    )";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string del_EXAM_JUNJANG(PsmhDb pDbCon, string strGUBUN, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " DELETE KOSMOS_OCS.EXAM_JUNMULCODE A    \r\n";
            SQL += "  WHERE 1=1                          \r\n";
            SQL += "    AND A.GUBUN  	= " + ComFunc.covSqlstr(strGUBUN, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public DataSet sel_EXAM_JUNMULCODE(PsmhDb pDbCon, string strGUBUN)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                             \r\n";
            SQL += "         A.GUBUN  || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_JUNMULLOT.GUBUN',A.GUBUN) AS GUBUN                  \r\n";
            SQL += "       , A.EXAMCODE                  \r\n";
            SQL += "       , B.EXAMNAME                  \r\n";
            SQL += "       , A.RANGKING                  \r\n";
            SQL += "       , A.ROWID                     \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_JUNMULCODE  A \r\n";
            SQL += "     , KOSMOS_OCS.EXAM_MASTER      B \r\n";
            SQL += "  WHERE 1=1                          \r\n";
            SQL += "    AND A.GUBUN  	= " + ComFunc.covSqlstr(strGUBUN, false);
            SQL += "    AND A.EXAMCODE 	= B.MASTERCODE   \r\n";
            SQL += "  ORDER BY A.RANGKING                \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

        public string up_EXAM_JUNLOT(PsmhDb pDbCon, string strCODE, string strEXAMCODE, string strLOTNO, string strGBCHAM,string strGBSD, string strSEQNO,string strCOMPANY, string strMATERIAL, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_JUNLOT  \r\n";
            SQL += "    SET GBCHAM    = " + ComFunc.covSqlstr(strGBCHAM, false);
            SQL += "      , GBSD      = " + ComFunc.covSqlstr(strGBSD, false);
            //SQL += "      , SEQNO     = '0' \r\n";                                                  // + ComFunc.covSqlstr(strSEQNO, false);
            SQL += "      , UPPS      = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += "      , UPDT      = SYSDATE     \r\n";
            SQL += "  WHERE 1=1                      \r\n";
            SQL += "    AND CODE      = " + ComFunc.covSqlstr(strCODE, false);
            SQL += "    AND EXAMCODE  = " + ComFunc.covSqlstr(strEXAMCODE, false);
            SQL += "    AND LOTNO     = " + ComFunc.covSqlstr(strLOTNO, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string del_EXAM_JUNLOT(PsmhDb pDbCon, string strCODE, string strEXAMCODE, string strLOTNO, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " DELETE KOSMOS_OCS.EXAM_JUNLOT  \r\n";
            SQL += "  WHERE 1=1                      \r\n";
            SQL += "    AND CODE      = " + ComFunc.covSqlstr(strCODE, false);
            SQL += "    AND EXAMCODE  = " + ComFunc.covSqlstr(strEXAMCODE, false);
            SQL += "    AND LOTNO     = " + ComFunc.covSqlstr(strLOTNO, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string ins_EXAM_JUNLOT(PsmhDb pDbCon, string strCODE, string strEXAMCODE, string strLOTNO, string strCOMPANY, string strMATERIAL, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL = "";
            SQL += "  MERGE INTO KOSMOS_OCS.EXAM_JUNLOT  A                                      \r\n";
            SQL += "       USING DUAL                                                           \r\n";
            SQL += "  	    ON (                                                                \r\n";
            SQL += "  	    	      A.CODE        = " + ComFunc.covSqlstr(strCODE, false);
            SQL += "  	    	 AND  A.LOTNO       = " + ComFunc.covSqlstr(strLOTNO, false);
            SQL += "  	    	 AND  A.EXAMCODE = " + ComFunc.covSqlstr(strEXAMCODE, false);
            SQL += "  	        )                                                               \r\n";
            SQL += "  		WHEN     MATCHED THEN                                               \r\n";
            SQL += "  		UPDATE                                                              \r\n";
            SQL += "  	       SET A.UPPS          = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += "  		  WHEN NOT MATCHED THEN                                               \r\n";
            SQL += " INSERT  ( CODE, EXAMCODE, LOTNO, COMPANY, MATERIAL,SEQNO, INPS, INPT_DT, UPPS, UPDT ) VALUES ( \r\n";
            SQL += "    " + ComFunc.covSqlstr(strCODE, false);
            SQL += "    " + ComFunc.covSqlstr(strEXAMCODE, true);
            SQL += "    " + ComFunc.covSqlstr(strLOTNO, true);
            SQL += "    " + ComFunc.covSqlstr(strCOMPANY, true);
            SQL += "    " + ComFunc.covSqlstr(strMATERIAL, true);
            SQL += "    , '0'                       \r\n";
            SQL += "    " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "    , SYSDATE                   \r\n";
            SQL += "    " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "    , SYSDATE                   \r\n";

            SQL += "    )";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string up_EXAM_JUNJANG(PsmhDb pDbCon, string strCODE, string strEXAMCODE, string strRANGKING, string strGBCHART, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_JUNJANG  \r\n";
            SQL += "    SET RANGKING  =" + ComFunc.covSqlstr(strRANGKING, false);
            SQL += "      , GBCHART   =" + ComFunc.covSqlstr(strGBCHART, false);
            SQL += "  WHERE 1=1                      \r\n";
            SQL += "    AND CODE      =" + ComFunc.covSqlstr(strCODE, false);
            SQL += "    AND EXAMCODE  =" + ComFunc.covSqlstr(strEXAMCODE, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string ins_EXAM_JUNJANG(PsmhDb pDbCon, string strCODE, string strEXAMCODE, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " INSERT INTO  KOSMOS_OCS.EXAM_JUNJANG ( CODE, EXAMCODE) VALUES ( \r\n";
            SQL += "    " + ComFunc.covSqlstr(strCODE, false);
            SQL += "    " + ComFunc.covSqlstr(strEXAMCODE, true);
            SQL += "    )";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string del_EXAM_JUNJANG(PsmhDb pDbCon, string strCODE, string strEXAMCODE, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " DELETE KOSMOS_OCS.EXAM_JUNJANG  \r\n";
            SQL += "  WHERE 1=1                      \r\n";
            SQL += "    AND CODE         = " + ComFunc.covSqlstr(strCODE, false);
            SQL += "    AND EXAMCODE  = " + ComFunc.covSqlstr(strEXAMCODE, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public DataTable sel_EXAM_JUNLOT_LOTNO(PsmhDb pDbCon, string strCODE)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "  SELECT                                    \r\n";           
            SQL += "       LOTNO                                \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_JUNLOT C             \r\n";            
            SQL += "  WHERE 1=1                                 \r\n";
            SQL += "    AND C.CODE  	= " + ComFunc.covSqlstr(strCODE, false);
            SQL += "  GROUP BY C.LOTNO                          \r\n";
            SQL += "  ORDER BY C.LOTNO                          \r\n";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

        

        public DataSet sel_EXAM_MASTER_JUNJANG(PsmhDb pDbCon, string strCODE)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT ''         AS CHK                              \r\n";
            SQL += "       , A.YNAME    AS MASTERCODE                       \r\n";
            SQL += "       , B.EXAMNAME AS MASTERNAME                       \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_SPECODE A                      \r\n";
            SQL += "       , KOSMOS_OCS.EXAM_MASTER  B                      \r\n";
            SQL += "   WHERE A.CODE   = " + ComFunc.covSqlstr(strCODE, false);
            SQL += "     AND A.GUBUN  = '71'                                \r\n";
            SQL += "     AND A.YNAME  = B.MASTERCODE                        \r\n";
            SQL += "     AND NOT EXISTS (SELECT 'A'                         \r\n";
            SQL += "                       FROM KOSMOS_OCS.EXAM_JUNJANG C              \r\n";
            SQL += " 					 WHERE 1=1                          \r\n";
            SQL += " 					   AND C.CODE = A.CODE              \r\n";
            SQL += " 					   AND C.EXAMCODE = B.MASTERCODE    \r\n";
            SQL += " 					   )                                \r\n";
            SQL += "    ORDER BY YNAME,EXAMNAME                             \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

        public DataSet sel_EXAM_JUNJANG(PsmhDb pDbCon, string strCODE, bool isView, string strLOTNO)
        {
            DataSet ds = null; 

            SQL = "";
            SQL += " SELECT                                                                          \r\n";
            SQL += "         '' CHK                                                                  \r\n";
            SQL += "       , A.EXAMCODE                                                              \r\n";

            if (strCODE == "C000" || strCODE == "S000" || strCODE == "C100" || strCODE == "S100")
            {
                SQL += "      , A.EXAMNAME    \r\n";
            }
            else
            {
                SQL += "      , B.EXAMNAME    \r\n";
            }
            
            SQL += "      , A.GBCHART                                                               \r\n";
            SQL += "      , A.RANGKING                                                              \r\n";
            SQL += "      , A.ROWID                                                                 \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_JUNJANG A                                               \r\n";
            SQL += "      , KOSMOS_OCS.EXAM_MASTER  B                                               \r\n";
            SQL += "  WHERE 1=1                                                                     \r\n";
            SQL += "    AND A.CODE     = " + ComFunc.covSqlstr(strCODE, false);
            SQL += "    AND A.EXAMCODE =  B.MASTERCODE(+)                                           \r\n";

            if (isView == true)
            {
                SQL += "    AND NOT EXISTS (SELECT 'A'                                                 \r\n";
                SQL += "                  FROM KOSMOS_OCS.EXAM_JUNLOT C                            \r\n";
                SQL += "                 WHERE 1=1                                                 \r\n";
                SQL += "                   AND C.CODE = A.CODE                                     \r\n";
                SQL += "                   AND C.EXAMCODE = A.EXAMCODE                             \r\n";
                SQL += "                   AND C.LOTNO = " + ComFunc.covSqlstr(strLOTNO, false);
                SQL += "                 )                                                         \r\n";
            }

            if (strCODE == "C000" || strCODE == "S000" || strCODE == "C100" || strCODE == "S100")
            {
                SQL += "  ORDER BY CODE,EXAMCODE,RANGKING                                           \r\n";
            }
            else
            {
                SQL += "  ORDER BY CODE,RANGKING                                                    \r\n";
            }

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

        public string ins_EXAM_CHAMGO(PsmhDb pDbCon, string strGBN, string strBDATE, string strEXAMCODE, string strEXAMNAME, string strSEQNO, string strRESULT, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " INSERT INTO  KOSMOS_OCS.EXAM_CHAMGO ( GBN, BDATE, EXAMCODE, EXAMNAME ,  SEQNO, RESULT ) VALUES ( \r\n";
            SQL += "    " + ComFunc.covSqlstr(strGBN        , false);
            SQL += "    " + ComFunc.covSqlstr(strBDATE      , true );
            SQL += "    " + ComFunc.covSqlstr(strEXAMCODE   , true );
            SQL += "    " + ComFunc.covSqlstr(strEXAMNAME   , true );
            SQL += "    " + ComFunc.covSqlstr(strSEQNO      , true );
            SQL += "    " + ComFunc.covSqlstr(strRESULT     , true );
            SQL += "    )";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string del_EXAM_CHAMGO(PsmhDb pDbCon, string strBDATE, string strEXAMCODE, string strGBN, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "DELETE " + ComNum.DB_MED + "EXAM_CHAMGO \r\n";
            SQL += " WHERE 1=1                              \r\n";
            SQL += "   AND EXAMCODE = " + ComFunc.covSqlstr(strEXAMCODE , false);
            SQL += "   AND BDATE    = " + ComFunc.covSqlstr(strBDATE    , false);
            SQL += "   AND GBN      = " + ComFunc.covSqlstr(strGBN      , false);


            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string ins_EXAM_JUNNORE(PsmhDb pDbCon, string strCODE, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " INSERT INTO KOSMOS_OCS.EXAM_JUNNOR ( CODE) VALUES ( \r\n";
            SQL += "    " + ComFunc.covSqlstr(strCODE, false);
            SQL += "    )";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string up_EXAM_JUNNORE(PsmhDb pDbCon, string strCODE, string strNORMAL, string strLOW, string strHIGH, string strINTER1, string strINTER2, string strINTER3, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_JUNNOR     \r\n";
            SQL += "    SET NORMAL  = " + ComFunc.covSqlstr(strNORMAL, false);
            SQL += "      , LOW     = " + ComFunc.covSqlstr(strLOW, false);
            SQL += "      , HIGH    = " + ComFunc.covSqlstr(strHIGH, false);
            SQL += "      , INTER1  = " + ComFunc.covSqlstr(strINTER1, false);
            SQL += "      , INTER2  = " + ComFunc.covSqlstr(strINTER2, false);
            SQL += "      , INTER3  = " + ComFunc.covSqlstr(strINTER3, false);
            SQL += "  WHERE 1=1                          \r\n";
            SQL += "    AND CODE   = " + ComFunc.covSqlstr(strCODE, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string del_EXAM_JUNNORE(PsmhDb pDbCon, string strCODE, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " DELETE KOSMOS_OCS.EXAM_JUNNOR  \r\n";
            SQL += "  WHERE 1=1                      \r\n";
            SQL += "    AND CODE = " + ComFunc.covSqlstr(strCODE, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string ins_EXAM_JUNCODE(PsmhDb pDbCon, string strCODE, string strNAME, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " INSERT INTO  KOSMOS_OCS.EXAM_JUNCODE ( CODE, NAME, USE_YN) VALUES ( \r\n";
            SQL += "    " + ComFunc.covSqlstr(strCODE, false);
            SQL += "    " + ComFunc.covSqlstr(strNAME, true);
            SQL += "    " + ComFunc.covSqlstr("Y", true);
            SQL += "    )";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string up_EXAM_JUNCODE(PsmhDb pDbCon, string strCODE, string strUSE_YN, string strGUBUN, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_JUNCODE     \r\n";
            SQL += "    SET GUBUN  = " + ComFunc.covSqlstr(strGUBUN, false);
            SQL += "      , USE_YN = " + ComFunc.covSqlstr(strUSE_YN, false);
            SQL += "  WHERE 1=1                          \r\n";
            SQL += "    AND CODE   = " + ComFunc.covSqlstr(strCODE, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public string del_EXAM_JUNCODE(PsmhDb pDbCon, string strCODE, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " DELETE KOSMOS_OCS.EXAM_JUNCODE  \r\n";
            SQL += "  WHERE 1=1                      \r\n";
            SQL += "    AND CODE = " + ComFunc.covSqlstr(strCODE, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public DataSet sel_EXAM_SPECODE_EQU(PsmhDb pDbCon)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT  CODE                                           \r\n";
            SQL += "       , NAME                                           \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_SPECODE                         \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "    AND GUBUN = '13'                                    \r\n";
            SQL += "    AND DELDATE IS NULL                                 \r\n";
            SQL += "    AND CODE NOT IN (                                   \r\n";
            SQL += "    					SELECT CODE                     \r\n";
            SQL += "    					  FROM KOSMOS_OCS.EXAM_JUNCODE  \r\n";
            SQL += "  					)                                   \r\n";
            SQL += "   ORDER BY CODE,NAME                                   \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

        public DataSet sel_EXAM_JUNCODE(PsmhDb pDbCon, string strUSE_YN = "", string strGUBUN = "")
        {
            DataSet ds = null;


            SQL = "";
            SQL += "  SELECT                                                                                                                                \r\n";     
            SQL += "  	     A.CODE                                                                                                                         \r\n";
            SQL += "  	   , A.NAME                                                                                                                         \r\n";
            SQL += "  	   , DECODE(NVL(A.USE_YN	,'N'),'Y','True') AS USE_YN                                                                             \r\n";
            SQL += "  	   , DECODE(NVL(A.GUBUN	,'@'),'@','', A.GUBUN ||'.'|| KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('EXAM_정도장비분류',A.GUBUN)) AS GUBUN_NM    \r\n";
            SQL += "  	   , DECODE(NVL(C.NORMAL	,'0'),'1','True') AS NORMAL                                                                             \r\n";
            SQL += "  	   , DECODE(NVL(C.LOW      ,'0'),'1','True') AS LOW                                                                                 \r\n";
            SQL += "  	   , DECODE(NVL(C.HIGH     ,'0'),'1','True') AS HIGH                                                                                \r\n";
            SQL += "  	   , INTER1                                                                                                                         \r\n";
            SQL += "  	   , INTER2                                                                                                                         \r\n";
            SQL += "  	   , INTER3                                                                                                                         \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_JUNCODE A                                                                                                      \r\n";
            SQL += "       , KOSMOS_OCS.EXAM_JUNNOR C                                                                                                       \r\n";
            SQL += "   WHERE 1=1                                                                                                                            \r\n";

            if (string.IsNullOrEmpty(strUSE_YN) == false)
            {
                SQL += "     AND A.USE_YN = " + ComFunc.covSqlstr(strUSE_YN, false);
            }

            if (string.IsNullOrEmpty(strGUBUN) == false)
            {
                SQL += "     AND A.GUBUN = " + ComFunc.covSqlstr(strGUBUN, false);
            }


            SQL += "     AND A.CODE = C.CODE(+)                                                                                                             \r\n";
            SQL += "   ORDER BY CODE                                                                                                                        \r\n";



            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

        public DataSet sel_EXAM_RESULTC_CHAMGO(PsmhDb pDbCon, string strFDate, string strTDate, string strSUBCODE, string strGBN)
        {
            DataSet ds = null;

            string strFAge = "20";
            string strTAge = "30";

            SQL = "";
            SQL += "  SELECT                                                        \r\n";
            SQL += "         TO_CHAR(A.RESULTDATE,'YYYY-MM-DD') RESULTDATE          \r\n";
            SQL += "       , A.PANO                                                 \r\n";
            SQL += "       , A.SNAME                                                \r\n";
            SQL += "       , A.SEX                                                  \r\n";
            SQL += "       , A.AGE                                                  \r\n";
            SQL += "       , B.RESULT                                               \r\n";
            SQL += "       , B.REFER                                                \r\n";
            SQL += "       , C.EXAMNAME                                             \r\n";
            SQL += "       , B.SUBCODE                                              \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_SPECMST  A                              \r\n";
            SQL += "      , KOSMOS_OCS.EXAM_RESULTC B                               \r\n";
            SQL += "      , KOSMOS_OCS.EXAM_MASTER C                                \r\n";
            SQL += "  WHERE 1=1                                                     \r\n";
            SQL += "    AND A.BDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "                    AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "    AND B.STATUS    ='V'                                        \r\n";
            SQL += "    AND A.SPECNO    = B.SPECNO                                  \r\n";
            SQL += "    AND B.SUBCODE   = C.MASTERCODE (+)                          \r\n";
            SQL += "    AND B.SUBCODE   = " + ComFunc.covSqlstr(strSUBCODE, false);

            if (strGBN == "C1")
            {
                SQL += "      AND A.AGE BETWEEN " + ComFunc.covSqlstr(strFAge, false);
                SQL += "                    AND " + ComFunc.covSqlstr(strTAge, false);
                SQL += "      AND A.SEX  = 'M'                                      \r\n";
            }
            else if (strGBN == "C2")
            {
                SQL += "      AND A.AGE BETWEEN " + ComFunc.covSqlstr(strFAge, false);
                SQL += "                    AND " + ComFunc.covSqlstr(strTAge, false);
                SQL += "      AND A.SEX  = 'F'                                      \r\n";
            }
            else if (strGBN == "C3")
            {
                SQL += "      AND A.DEPTCODE ='PD'                                  \r\n";
            }
            else
            {
                SQL += "      AND A.AGE BETWEEN " + ComFunc.covSqlstr(strFAge, false);
                SQL += "                    AND " + ComFunc.covSqlstr(strTAge, false);
            }

            SQL += "    ORDER BY RESULTDATE DESC                                    \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

        public DataSet sel_EXAM_CHAMGO(PsmhDb pDbCon, string strGBN, string strBDATE, string strEXAMCODE)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                  \r\n";
            SQL += "         GBN                             \r\n";
            SQL += "       , BDATE                           \r\n";
            SQL += "       , EXAMCODE                        \r\n";
            SQL += "       , EXAMNAME                        \r\n";
            SQL += "       , SEQNO                           \r\n";
            SQL += "       , RESULT                          \r\n";
            SQL += "       , ROWID                           \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_CHAMGO           \r\n";
            SQL += " WHERE 1=1                              \r\n";
            SQL += "   AND GBN      = " + ComFunc.covSqlstr(strGBN, false);
            SQL += "   AND BDATE    = " + ComFunc.covSqlstr(strBDATE, false);
            SQL += "   AND EXAMCODE = " + ComFunc.covSqlstr(strEXAMCODE, false);
            SQL += " ORDER BY EXAMCODE, BDATE,SEQNO         \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

        public DataSet sel_EXAM_CHAMGO_BDATE(PsmhDb pDbCon, string strGBN, string strEXAMCODE)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                 \r\n";
            SQL += "       BDATE                                            \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_CHAMGO                           \r\n";
            SQL += " WHERE 1=1                                              \r\n";
            SQL += "   AND GBN      = " + ComFunc.covSqlstr(strGBN, false);
            SQL += "   AND EXAMCODE IN ( " + strEXAMCODE + ")               \r\n";
            SQL += " GROUP BY BDATE                                         \r\n";
            SQL += " ORDER BY BDATE DESC                                    \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

        public DataSet sel_EXAM_CHAMGO_GROUP(PsmhDb pDbCon, string strGBN, string strEXAMCODE)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                 \r\n";
            SQL += "        EXAMCODE                        \r\n";
            SQL += "      , BDATE                           \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_CHAMGO      \r\n";
            SQL += " WHERE 1=1                              \r\n";
            SQL += "   AND GBN      = " + ComFunc.covSqlstr(strGBN      , false);
            SQL += "   AND EXAMCODE = " + ComFunc.covSqlstr(strEXAMCODE , false);
            SQL += " GROUP BY EXAMCODE, BDATE                \r\n";
            SQL += " ORDER BY EXAMCODE, BDATE                \r\n";

            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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

        public string up_EXAM_MASTER_GBBASE(PsmhDb pDbCon, string strRowid, string strGBBASE, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "UPDATE " + ComNum.DB_MED + "EXAM_MASTER SET GBBASE=" + ComFunc.covSqlstr(strGBBASE, false) + " \r\n";
            SQL += " WHERE ROWID = " + ComFunc.covSqlstr(strRowid, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            return SqlErr;

        }

        public DataSet sel_EXAM_MASTER_GBBASE(PsmhDb pDbCon, string strWC, string strGBBASE = "")
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                                 \r\n";
            SQL += "        DECODE(GBBASE,'*','True','') AS GBBASE          \r\n";
            SQL += "      , MASTERCODE                                      \r\n";
            SQL += "      , EXAMNAME                                        \r\n";
            SQL += "      , ROWID                                           \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_MASTER /** 검사실 접수 Master*/  \r\n";
            SQL += " WHERE 1=1                                              \r\n";
            SQL += "   AND TONGBUN    = " + ComFunc.covSqlstr(strWC, false);

            if (string.IsNullOrEmpty(strGBBASE) == false)
            {
                SQL += "   AND GBBASE = '*'                                 \r\n";
            }
            SQL += "  ORDER BY MASTERCODE  \r\n";


            try
            {
                string SqlErr = clsDB.GetDataSet(ref ds, SQL,pDbCon);

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
       
    }
}
