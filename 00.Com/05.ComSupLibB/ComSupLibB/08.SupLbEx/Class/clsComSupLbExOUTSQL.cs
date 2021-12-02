using ComBase; //기본 클래스
using ComDbB; //DB연결
using ComSupLibB.Com;
using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Text;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name : ComSupLibB.SupLbEx
    /// File Name : clsLbExSQLQC.cs
    /// Title or Description : 진단검사의학과 외부의뢰 SQL
    /// Author : 김홍록
    /// Create Date : 2017-11-13  
    /// Update History : 
    /// </summary>
     
    public class clsComSupLbExOUTSQL : Com.clsMethod 
    {
        string SQL = string.Empty;

        public enum enmSel_EXAM_RESULTC_WRTNO { IMGWRTNO, BDATE, PANO, SNAME, SPECNO, MASTERCODE, SUBCODE, SMCODE, SEQNO, SMNAME, EXAMNAME, EXCODE,STATUS, RECEIVEDATE, IMG,GB_IMG,WPART};
        public string[] sSS_EXAM_SPECODE = { "장비코드", "장비명" };
        public int[] nSS_EXAM_SPECODE = { nCol_EXCD + 20, nCol_NAME };

        public enum enmSel_EXAM_RESULT_IMG_IMAGE     {       CHK,      BDATE, RECEIVEDATE,  EXAMFNAME,  CHK_IMAGE,   IMAGE, ROWID }; 
        public string[] sSel_EXAM_RESULT_IMG_IMAGE = {    "삭제", "처방일자",  "검사일자",   "검사명",     "결과", "IMAGE","ROWID" };
        public int[] nSel_EXAM_RESULT_IMG_IMAGE    = { nCol_SCHK, nCol_DATE,   nCol_DATE, nCol_DATE,  nCol_SCHK,       5,    5 };

        public enum enmSel_EXAM_SAMKWANG     {       CHK,      SMCODE,       SMNAME,         EXCODE,     EXAMNAME,   GB_IMG, GB_SEND,   CHANG, ROWID  };
        public string[] sSel_EXAM_SAMKWANG = {    "삭제",  "코드", "코드명", "원내검사코드", "원내검사명", "이미지",  "전송불가", "작업","ROWID" };
        public int[] nSel_EXAM_SAMKWANG    = { nCol_SCHK,   nCol_DATE,    nCol_JUSO,      nCol_DATE,    nCol_JUSO, nCol_SCHK, nCol_SCHK, nCol_SCHK,  5  }; 

        public enum enmSel_EXAM_SPECMST_OUT         {       CHK,          WDATE,       SPECNO ,   WORKSTS   ,          PANO,         SNAME,        AGE,        SEX,          WARD,   DEPTCODE,    SPECNM,       EXAMCODE,         EXAMNAME,          ETC,                ANATNO,       BARCODE };
        public string[] sSel_EXAM_SPECMST_OUT   =   {    "선택",     "의뢰일자",   "검체번호" ,      "WS"   ,    "등록번호",      "환자명",     "나이",     "성별",        "병동",       "과",    "검체",     "검사코드",       "검사항목",          "C",            "병리번호",      "바코드" };
        public int[] nSel_EXAM_SPECMST_OUT      =   { nCol_SCHK,    nCol_PANO-7,  nCol_SPNO+20, nCol_SCHK+25,  nCol_PANO+10, nCol_SNAME-15, nCol_AGE-20, nCol_SCHK, nCol_AGE - 20,  nCol_SCHK,  nCol_AGE, nCol_ORDERNAME + 40, nCol_ORDERNAME +40, nCol_AGE - 20, nCol_SPNO-10 ,  nCol_SPNO+10 };

        public enum enmSel_EXAM_OUTRESULT_COMMIT     {       CHK,  RESULTDATE ,         JEPDATE,       PANO,         SNAME,     SMCODE,    WORKSTS,     EXCODE,       SMNAME,     RESULT,      SEND,     SPECNO,      IMG,   IMGWRTNO,   SENDERROR,       EXC,     VSABUN,      VDATE,   SMSPECNO,   WPART,  ROWID, GB_SEND };
        public string[] sSel_EXAM_OUTRESULT_COMMIT = {    "선택", "전송일",    "도착일", "등록번호",      "환자명", "코드",    "W/S"  , "병원코드", "검사명", "검사결과",    "전송", "검체번호", "이미지", "IMGWRTNO",  "오류형태",    "제외",   "검증자", "검증일시", "SMSPECNO", "WPART", "ROWID", "GB_SEND" };
        public int[] nSel_EXAM_OUTRESULT_COMMIT    = { nCol_SCHK, nCol_DATE-5,    nCol_DATE-5,  nCol_PANO-15, nCol_SNAME-20,  nCol_CHEK,   nCol_SEX,  nCol_CHEK,    nCol_NAME,  nCol_NAME, nCol_CHEK,  nCol_SPNO, nCol_AGE,          5,   nCol_JUSO, nCol_SCHK,  nCol_CHEK,  nCol_TIME,          5,       5,      5,5  };

        public enum enmSel_EXAM_RESULT_IMG { IMAGE, SEQNO};

        public enum enmIns_EXAM_SAMKWANG_DETAIL {  WGB, JDATE, SNAME,  PANO, SM_EXAM_NM, SM_SPECCODE, SM_AMT, SM_PK, EDI_CODE, EDI_AMT, DEPTCODE, ETC_CODE, DRCODE, SM_CODE,  ERR_NM, INPS, INPT_DT, UPDT, UPPS };

        public enum enmSel_EXAM_SAMKWANG_DETAIL_SUM     {   SORT,        SM_CODE,   EDI_CODE,    SM_EXAM_NM,      QTY,     SM_AMT, SM_AMT_SUM,    EDI_AMT, EDI_AMT_SUM  ,      DIFF  ,       PER     };
        public string[] sSel_EXAM_SAMKWANG_DETAIL_SUM = { "SORT",     "코드", "표준코드", "검사명",   "수량", "단가", "총액", "표준단가", "표준총액"   , "차액금액" , "차액(%)"     };
        public int[] nSel_EXAM_SAMKWANG_DETAIL_SUM    = {      5,   nCol_PANO-10,  nCol_PANO,  nCol_NAME+20, nCol_AGE,  nCol_PANO,  nCol_PANO,  nCol_PANO, nCol_PANO    , nCol_PANO  ,  nCol_PANO-20 };

        public enum enmIns_EXAM_SAMKWANG_MASTER { WGB, SMCODE, SMNAME, MODY_DT, EDI_CODE, EDI_AMT, SM_AMT, INPS, INPT_DT, UPPS, UPDT, GUBUN  };

        public enum enmSel_EXAM_SAMKWANG_MASTER        {        WGB,      SMCODE,   EDI_CODE,        SMNAME,    EDI_AMT,     SM_AMT,    MODY_DT };
        public string[] sSel_EXAM_SAMKWANG_MASTER    = { "보험구분",  "코드", "표준코드", "검사명", "표준단가", "단가", "변경일자" };
        public int[] nSel_EXAM_SAMKWANG_MASTER       = {   nCol_AGE,   nCol_PANO,  nCol_PANO,     nCol_JUSO,  nCol_PANO,  nCol_PANO,  nCol_PANO };

        public enum enmSel_EXAM_SAMKWANG_DETAIL_P      {        WGB,          JDATE,                 PANO,   ETC_CODE,      SNAME,    SM_CODE,   EDI_CODE,   SM_EXAM_NM,     SM_AMT,  EDI_AMT   };
        public string[] sSel_EXAM_SAMKWANG_DETAIL_P  = { "보험구분", "접수일자", "환자번호(병리번호)", "검체번호", "환자성명", "코드", "표준코드", "검사명", "단가", "표준단가" };
        public int[] nSel_EXAM_SAMKWANG_DETAIL_P     = {   nCol_AGE,      nCol_PANO,            nCol_PANO,  nCol_PANO,  nCol_PANO,  nCol_PANO,  nCol_PANO,    nCol_JUSO,  nCol_PANO, nCol_PANO  };

        public enum enmSel_EXAM_RESULTC_OUT_SUM       {   SORT,        WGB,     SMCODE,   EDI_CODE,        SMNAME,    QTY };
        public string[] sSel_EXAM_RESULTC_OUT_SUM   = { "SORT", "보험구분", "코드", "표준코드", "검사명", "갯수" };
        public int[] nSel_EXAM_RESULTC_OUT_SUM      = {      5,  nCol_AGE,  nCol_PANO,  nCol_PANO,     nCol_JUSO,        };

        public enum enmSel_EXAM_RESULTC_OUT_P         {       WGB,     SPECNO,     ANATNO,       PANO,      SNAME,   MASTERCODE,    SUBCODE,   EXAMNAME,    RESULT,     SMCODE,       SMNAME };
        public string[] sSel_EXAM_RESULTC_OUT_P     = {"보험구분", "검체번호", "병리번호", "환자번호", "환자성명", "마스터코드", "서브코드",   "검사명",    "결과", "코드", "검사명" };
        public int[] nSel_EXAM_RESULTC_OUT_P        = {  nCol_AGE,  nCol_PANO,  nCol_PANO,  nCol_PANO,  nCol_PANO,    nCol_PANO,  nCol_PANO,  nCol_JUSO, nCol_PANO,  nCol_PANO,    nCol_JUSO };

        public DataSet sel_EXAM_RESULTC_OUT_SUM(PsmhDb pDbCon, string strJDATE, string strWGB)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                            \r\n";
            SQL += "         0 					AS SORT                                                 \r\n";
            SQL += "        , MAX(C.WGB)        AS WGB                                                  \r\n";
            SQL += "        , B.SMCODE          AS SMCODE                                               \r\n";
            SQL += "        , MAX(C.EDI_CODE) 	AS EDI_CODE                                             \r\n";
            SQL += "        , C.SMNAME                                                                  \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(1) 	  , '999,999,999') )		        AS QTY          \r\n";
            SQL += "    FROM  KOSMOS_OCS.EXAM_SPECMST A                                                 \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_RESULTC B                                                 \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_MASTER  D                                                 \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_SAMKWANG_MASTER C                                         \r\n";
            SQL += "  WHERE 1=1                                                                         \r\n";
            SQL += "    AND A.SPECNO = B.SPECNO                                                         \r\n";
            SQL += "    AND TRUNC(A.WDATE) BETWEEN TO_DATE('" + strJDATE.Replace("년", "").Replace("월", "") + "01', 'YYYY-MM-DD')              \r\n";
            SQL += "                           AND LAST_DAY(TO_DATE('" + strJDATE.Replace("년", "").Replace("월", "") + "01', 'YYYY-MM-DD'))    \r\n";

            if (strWGB.Equals("보험") || strWGB.Equals("비보험") || strWGB.Equals("개별"))
            {
                SQL += "    AND C.WGB = " + ComFunc.covSqlstr(strWGB, false);
            }

            SQL += "    AND B.SUBCODE = D.MASTERCODE                                                    \r\n";

            //2018-06-26 안정수 추가, EDI_CODE null값인 경우 제외
            SQL += "    AND C.EDI_CODE IS NOT NULL                                                      \r\n";

            SQL += "    AND B.SMCODE IS NOT NULL                                                        \r\n";
            SQL += "    AND A.WGBN   IS NOT NULL                                                        \r\n";
            SQL += "    AND B.SMCODE = C.SMCODE(+)                                                      \r\n";
            SQL += "    AND A.WGBN    = '1'                                \r\n";
            SQL += "  GROUP BY B.SMCODE, C.SMNAME                                                       \r\n";
            SQL += "  UNION ALL                                                                         \r\n";
            SQL += "  SELECT                                                                            \r\n";
            SQL += "          1  AS SORT                                                                \r\n";
            SQL += "        , ''    AS WGB                                                   \r\n";
            SQL += "        , ''	AS SMCODE                                                           \r\n";
            SQL += "        , '' 	AS EDI_CODE                                                         \r\n";
            SQL += "        , '총합계' AS SMNAME                                                        \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(1) 	  , '999,999,999') )		        AS QTY          \r\n";
            SQL += "    FROM  KOSMOS_OCS.EXAM_SPECMST A                                                 \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_RESULTC B                                                 \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_MASTER  D                                                 \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_SAMKWANG_MASTER C                                         \r\n";
            SQL += "  WHERE 1=1                                                                         \r\n";
            SQL += "    AND A.SPECNO = B.SPECNO                                                         \r\n";
            SQL += "    AND TRUNC(A.WDATE) BETWEEN TO_DATE('" + strJDATE.Replace("년", "").Replace("월", "") + "01', 'YYYY-MM-DD')              \r\n";
            SQL += "                           AND LAST_DAY(TO_DATE('" + strJDATE.Replace("년", "").Replace("월", "") + "01', 'YYYY-MM-DD'))    \r\n";

            if (strWGB.Equals("보험") || strWGB.Equals("비보험") || strWGB.Equals("개별"))
            {
                SQL += "    AND C.WGB = " + ComFunc.covSqlstr(strWGB, false);
            }

            SQL += "    AND B.SUBCODE = D.MASTERCODE                                                    \r\n";

            //2018-06-26 안정수 추가, EDI_CODE null값인 경우 제외
            SQL += "    AND C.EDI_CODE IS NOT NULL                                                      \r\n";

            SQL += "    AND B.SMCODE IS NOT NULL                                                        \r\n";
            SQL += "    AND A.WGBN   IS NOT NULL                                                        \r\n";
            SQL += "    AND B.SMCODE = C.SMCODE(+)                                                      \r\n";
            SQL += "    AND A.WGBN    = '1'                                \r\n";
            SQL += "  ORDER BY SORT, SMCODE                                                             \r\n";

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

        public DataSet sel_EXAM_RESULTC_OUT_P(PsmhDb pDbCon, string strJDATE, string strWGB)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                               \r\n";
            SQL += "          C.WGB             AS WGB                     \r\n";
            SQL += "        , B.SPECNO                                     \r\n";
            SQL += "        , B.ANATNO                                     \r\n";
            SQL += "        , B.PANO                                       \r\n";
            SQL += "        , A.SNAME                                      \r\n";
            SQL += "        , B.MASTERCODE                                 \r\n";
            SQL += "        , B.SUBCODE                                    \r\n";
            SQL += "        , D.EXAMNAME                                   \r\n";
            SQL += "        , B.RESULT                                     \r\n";
            SQL += "        , B.SMCODE                                     \r\n";
            SQL += "        , C.SMNAME                                     \r\n";
            SQL += "    FROM  KOSMOS_OCS.EXAM_SPECMST A                    \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_RESULTC B                    \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_MASTER  D                    \r\n";
            SQL += "        , KOSMOS_OCS.EXAM_SAMKWANG_MASTER C            \r\n";
            SQL += "  WHERE 1=1                                            \r\n";
            SQL += "    AND A.SPECNO = B.SPECNO                            \r\n";
            SQL += "    AND TRUNC(A.WDATE) BETWEEN TO_DATE('" + strJDATE.Replace("년", "").Replace("월", "") + "01', 'YYYY-MM-DD')              \r\n";
            SQL += "                                AND LAST_DAY(TO_DATE('" + strJDATE.Replace("년", "").Replace("월", "") + "01', 'YYYY-MM-DD'))    \r\n";

            if (strWGB.Equals("보험") || strWGB.Equals("비보험") || strWGB.Equals("개별"))
            {
                SQL += "    AND C.WGB = " + ComFunc.covSqlstr(strWGB, false);
            }
            SQL += "    AND A.WGBN    = '1'                                \r\n";
            SQL += "    AND B.SUBCODE = D.MASTERCODE                       \r\n";
            SQL += "    AND B.SMCODE IS NOT NULL                           \r\n";
            SQL += "    AND A.WGBN   IS NOT NULL                           \r\n";
            SQL += "    AND B.SMCODE = C.SMCODE(+)                         \r\n";
            SQL += "  ORDER BY A.SPECNO, B.SEQNO                           \r\n";

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

        public DataSet sel_EXAM_SAMKWANG_DETAIL_P(PsmhDb pDbCon, string strJDATE, string strWGB)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                              \r\n";

            SQL += "           WGB                                                        \r\n";
            SQL += "         , JDATE                                                      \r\n";
            SQL += "         , PANO                                                       \r\n";
            SQL += "         , ETC_CODE                                                   \r\n";
            SQL += "         , SNAME                                                      \r\n";
            SQL += "         , SM_CODE                                                    \r\n";
            SQL += "         , EDI_CODE                                                   \r\n";
            SQL += "         , SM_EXAM_NM                                                 \r\n";
            SQL += "         , SM_AMT                                                     \r\n";
            SQL += "         , EDI_AMT                                                    \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_SAMKWANG_DETAIL                              \r\n";
            SQL += "   WHERE 1=1                                                          \r\n";
            SQL += "     AND JDATE BETWEEN TO_DATE('" + strJDATE.Replace("년", "").Replace("월", "") + "01','YYYY-MM-DD')                 \r\n";
            SQL += "                   AND LAST_DAY(TO_DATE('" + strJDATE.Replace("년", "").Replace("월", "") + "01','YYYY-MM-DD'))       \r\n";
            SQL += "     AND WGB = " + ComFunc.covSqlstr(strWGB, false);
            SQL += "  ORDER BY JDATE                                                \r\n";
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


        public DataSet sel_EXAM_SAMKWANG_MASTER(PsmhDb pDbCon)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                            \r\n";
            SQL += "         WGB                                        \r\n";
            SQL += "       , SMCODE                                     \r\n";
            SQL += "       , EDI_CODE                                   \r\n";
            SQL += "       , SMNAME                                     \r\n";
            SQL += "       , EDI_AMT                                    \r\n";
            SQL += "       , SM_AMT                                     \r\n";
            SQL += "       , TO_CHAR(MODY_DT, 'YYYY-MM-DD') AS MODY_DT  \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_SAMKWANG_MASTER            \r\n";
            SQL += "  ORDER BY WGB, SMCODE                              \r\n";

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


        public string ins_EXAM_SAMKWANG_MASTER(PsmhDb pDbCon
                                                            , string WGB     
                                                            , string SMCODE  
                                                            , string SMNAME  
                                                            , string MODY_DT 
                                                            , string EDI_CODE
                                                            , string EDI_AMT 
                                                            , string SM_AMT  
                                                            , ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "";
            SQL += "  INSERT INTO KOSMOS_OCS.EXAM_SAMKWANG_MASTER     \r\n";
            SQL += "  	(                                             \r\n";
            SQL += "         WGB                                      \r\n";
            SQL += "       , SMCODE                                   \r\n";
            SQL += "       , SMNAME                                   \r\n";
            SQL += "       , MODY_DT                                  \r\n";
            SQL += "       , EDI_CODE                                 \r\n";
            SQL += "       , EDI_AMT                                  \r\n";
            SQL += "       , SM_AMT                                   \r\n";
            SQL += "       , INPS                                     \r\n";
            SQL += "       , INPT_DT                                  \r\n";
            SQL += "       , UPPS                                     \r\n";
            SQL += "       , UPDT                                     \r\n";
            SQL += "  	)                                       \r\n";
            SQL += "  	VALUES                                  \r\n";
            SQL += "  	(                                       \r\n";
            SQL += "  	    " + ComFunc.covSqlstr(WGB       , false);
            SQL += "  	    " + ComFunc.covSqlstr(SMCODE    , true  );
            SQL += "  	    " + ComFunc.covSqlstr(SMNAME    , true  );
            SQL += "  	    " + ComFunc.covSqlDate(MODY_DT  , true  );
            SQL += "  	    " + ComFunc.covSqlstr(EDI_CODE  , true  );
            SQL += "  	    " + ComFunc.covSqlstr(EDI_AMT.Replace(",","")   , true  );
            SQL += "  	    " + ComFunc.covSqlstr(SM_AMT.Replace(",", ""), true  );
            SQL += "  	    " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "  	    , SYSDATE                                             \r\n";
            SQL += "  	    " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "  	    , SYSDATE                                             \r\n";
            
            SQL += "  	)                                                         \r\n";


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

        public string del_EXAM_SAMKWANG_MASTER(PsmhDb pDbCon, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  DELETE KOSMOS_OCS.EXAM_SAMKWANG_MASTER                  \r\n";

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    //ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

        public DataSet sel_EXAM_SAMKWANG_DETAIL_SUM(PsmhDb pDbCon, string strJDATE, string strWGB)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                \r\n";
            SQL += "          0				    AS SORT                                     \r\n";
            SQL += "        , SM_CODE       	AS SM_CODE                                  \r\n";
            SQL += "        , MAX(EDI_CODE) 	AS EDI_CODE                                 \r\n";
            SQL += "        , SM_EXAM_NM	  	AS SM_EXAM_NM                               \r\n";

            SQL += "        , TRIM(TO_CHAR(SUM(1) 	  , '999,999,999') )		        AS QTY                  \r\n";
            SQL += "        , TRIM(TO_CHAR(MAX(SM_AMT) , '999,999,999'))	  	        AS SM_AMT               \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(SM_AMT) , '999,999,999'))	  	        AS SM_AMT_SUM           \r\n";
            SQL += "        , TRIM(TO_CHAR(MAX(EDI_AMT), '999,999,999'))  	            AS EDI_AMT              \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(EDI_AMT), '999,999,999'))  	            AS EDI_AMT_SUM          \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(EDI_AMT) - SUM(SM_AMT), '999,999,999'))  AS DIFF                 \r\n";
            SQL += "        , ROUND(SUM(SM_AMT) / DECODE(SUM(EDI_AMT),0,1,SUM(EDI_AMT)) * 100,1) AS PER         \r\n";
            SQL += "    FROM KOSMOS_OCS.EXAM_SAMKWANG_DETAIL                                                    \r\n";
            SQL += "   WHERE 1=1                                                            \r\n";
            SQL += "     AND JDATE BETWEEN TO_DATE('" + strJDATE.Replace("년","").Replace("월","") + "01','YYYY-MM-DD')                 \r\n";
            SQL += "                   AND LAST_DAY(TO_DATE('" + strJDATE.Replace("년", "").Replace("월", "") + "01','YYYY-MM-DD'))       \r\n";
            SQL += "     AND WGB = " + ComFunc.covSqlstr(strWGB, false);

            SQL += "  GROUP BY SM_CODE, SM_EXAM_NM                                          \r\n";
            SQL += "  UNION ALL                                                             \r\n";
            SQL += "  SELECT                                                                \r\n";
            SQL += "                1			AS SORT                                     \r\n";
            SQL += "        , ''       	AS SM_CODE                                          \r\n";
            SQL += "        , ''            	AS EDI_CODE                                 \r\n";
            SQL += "        , '총합계'	         	AS SM_EXAM_NM                           \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(1) 	  , '999,999,999'))		AS QTY              \r\n";
            SQL += "        , ''                                    	  	AS SM_AMT           \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(SM_AMT) , '999,999,999'))  	AS SM_AMT_SUM       \r\n";
            SQL += "        , ''                                          	AS EDI_AMT          \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(EDI_AMT), '999,999,999'))  	AS EDI_AMT_SUM      \r\n";
            SQL += "        , TRIM(TO_CHAR(SUM(EDI_AMT) - SUM(SM_AMT), '999,999,999'))  AS DIFF                 \r\n";
            SQL += "        , ROUND(SUM(SM_AMT) / DECODE(SUM(EDI_AMT),0,1,SUM(EDI_AMT)) * 100,1) AS PER         \r\n";

            SQL += "    FROM KOSMOS_OCS.EXAM_SAMKWANG_DETAIL                                           \r\n";
            SQL += "   WHERE 1=1                                                            \r\n";
            SQL += "     AND JDATE BETWEEN TO_DATE('" + strJDATE.Replace("년", "").Replace("월", "") + "01','YYYY-MM-DD')                 \r\n";
            SQL += "                   AND LAST_DAY(TO_DATE('" + strJDATE.Replace("년", "").Replace("월", "") + "01','YYYY-MM-DD'))       \r\n";
            SQL += "     AND WGB = " + ComFunc.covSqlstr(strWGB, false);
            SQL += "  ORDER BY SORT,SM_CODE                                                 \r\n";

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


        public string ins_EXAM_SAMKWANG_DETAIL(PsmhDb pDbCon, string WGB
                                                            , string SNAME
                                                            , string JDATE
                                                            , string PANO
                                                            , string SM_EXAM_NM
                                                            , string SM_SPECCODE
                                                            , string SM_AMT
                                                            , string SM_PK
                                                            , string EDI_CODE
                                                            , string EDI_AMT
                                                            , string DEPTCODE
                                                            , string ETC_CODE
                                                            , string DRCODE
                                                            , string SM_CODE
                                                            , ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "";

            SQL += "  INSERT INTO KOSMOS_OCS.EXAM_SAMKWANG_DETAIL     \r\n";
            SQL += "  	(                                       \r\n";
            SQL += "  	     WGB                                \r\n";
            SQL += "  	    ,SNAME                              \r\n";
            SQL += "  	    ,PANO                               \r\n";
            SQL += "  	    ,SM_EXAM_NM                         \r\n";
            SQL += "  	    ,SM_SPECCODE                        \r\n";
            SQL += "  	    ,SM_AMT                             \r\n";
            SQL += "  	    ,SM_PK                              \r\n";
            SQL += "  	    ,EDI_CODE                           \r\n";
            SQL += "  	    ,EDI_AMT                            \r\n";
            SQL += "  	    ,DEPTCODE                           \r\n";
            SQL += "  	    ,ETC_CODE                           \r\n";
            SQL += "  	    ,DRCODE                             \r\n";
            SQL += "  	    ,SM_CODE                            \r\n";
            SQL += "  	    ,JDATE                              \r\n";
            SQL += "  	    ,INPT_DT                            \r\n";
            SQL += "  	    ,INPS                               \r\n";
            SQL += "  	    ,UPDT                               \r\n";
            SQL += "  	    ,UPPS                               \r\n";            
            
            SQL += "  	)                                       \r\n";
            SQL += "  	VALUES                                  \r\n";
            SQL += "  	(                                       \r\n";
            SQL += "  	    " + ComFunc.covSqlstr(WGB           , false);
            SQL += "  	    " + ComFunc.covSqlstr(SNAME         , true);
            SQL += "  	    " + ComFunc.covSqlstr(PANO          , true);
            SQL += "  	    " + ComFunc.covSqlstr(SM_EXAM_NM    , true);
            SQL += "  	    " + ComFunc.covSqlstr(SM_SPECCODE   , true);
            SQL += "  	    " + ComFunc.covSqlstr(SM_AMT        , true);
            SQL += "  	    " + ComFunc.covSqlstr(SM_PK         , true);
            SQL += "  	    " + ComFunc.covSqlstr(EDI_CODE      , true);
            SQL += "  	    " + ComFunc.covSqlstr(EDI_AMT       , true);
            SQL += "  	    " + ComFunc.covSqlstr(DEPTCODE      , true);
            SQL += "  	    " + ComFunc.covSqlstr(ETC_CODE.Replace("-","")      , true);
            SQL += "  	    " + ComFunc.covSqlstr(DRCODE        , true);
            SQL += "  	    " + ComFunc.covSqlstr(SM_CODE       , true);
            SQL += "  	    " + ComFunc.covSqlDate(JDATE        , true);
            SQL += "  	    , SYSDATE                                             \r\n";
            SQL += "  	    " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "  	    , SYSDATE                                             \r\n";
            SQL += "  	    " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "  	)                                                         \r\n";


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

        public string del_EXAM_SAMKWANG_DETAIL(PsmhDb pDbCon, string strJDATE, string strWGB, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  DELETE KOSMOS_OCS.EXAM_SAMKWANG_DETAIL                  \r\n";
            SQL += "   WHERE JDATE BETWEEN TO_DATE('" + strJDATE + "01', 'YYYY-MM-DD')   \r\n";
            SQL += "                  AND LAST_DAY(TO_DATE('" + strJDATE + "01', 'YYYY-MM-DD'))   \r\n";
            SQL += "                  AND WGB = '" + strWGB + "'";
            


            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    //ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

        public DataSet sel_EXAM_SAMKWANG(PsmhDb pDbCon, string strSMCODE, string strGbnCom = "")
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                             \r\n";
            SQL += " 	     '' CHK                     \r\n";
            SQL += " 	   , A.SMCODE                   \r\n";
            SQL += " 	   , A.SMNAME                   \r\n";
            SQL += " 	   , A.EXCODE                   \r\n";
            SQL += " 	   , B.EXAMNAME                 \r\n";
            SQL += " 	   , DECODE(A.GB_IMG ,'1','True','') GB_IMG                   \r\n";
            SQL += " 	   , DECODE(A.GB_SEND,'1','True','') GB_SEND                   \r\n";
            SQL += " 	   , '' AS CHANG                \r\n";
            SQL += " 	   , A.ROWID                    \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_SAMKWANG A  \r\n";
            SQL += "       ,KOSMOS_OCS.EXAM_MASTER   B  \r\n";
            SQL += "  WHERE 1=1                         \r\n";
            SQL += "    AND A.EXCODE=B.MASTERCODE(+)    \r\n";

            if (string.IsNullOrEmpty(strSMCODE) == false)
            {
                SQL += "    AND A.SMCODE= " + ComFunc.covSqlstr(strSMCODE, false);
            }
            if (strGbnCom == "1")
            {
                SQL += "    AND A.GB_COM = '1' ";
            }
            else if (strGbnCom == "2")
            {
                SQL += "    AND A.GB_COM = '2' ";
            }

            SQL += "  ORDER BY A.SMCODE, A.EXCODE       \r\n";

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


        public string sel_EXAM_SPECMST_STATUS(PsmhDb pDbCon, string strSPECNO)
        {
            DataTable dt = null;

            string strRETURN = "";

            SQL = "";
            SQL += "     SELECT                                     \r\n";
            SQL += "            STATUS                               \r\n";           
            SQL += "       FROM KOSMOS_OCS.EXAM_SPECMST  A       \r\n";
            SQL += "     WHERE 1=1                                  \r\n";
            SQL += "       AND SPECNO = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "     ORDER BY  SEQNO                            ";

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                strRETURN = dt.Rows[0][0].ToString();
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return strRETURN;
        }

        public string up_EXAM_SPECMST_STATUS(PsmhDb pDbCon, string strSPECNO, ref int intRowAffected, string strGWAGubun = "")
        {
            string SqlErr = "";

            SQL = "";
            SQL += " UPDATE KOSMOS_OCS.EXAM_SPECMST                                                                                 \r\n";
            SQL += "    SET STATUS     = KOSMOS_OCS.FC_EXAM_SPECMST_STATUS('" + strSPECNO + "')                                     \r\n";
            SQL += "      , PRINT      = 0                                                                                          \r\n";
            SQL += "      , EMR        = 0                                                                                          \r\n";
            SQL += "      , RESULTDATE = (                                                                                          \r\n";
            SQL += " 						CASE WHEN KOSMOS_OCS.FC_EXAM_SPECMST_STATUS('" + strSPECNO + "') IN ('05','04') THEN SYSDATE   \r\n";
            SQL += "                              ELSE NULL                                                                         \r\n";
            SQL += "                           END                                                                                  \r\n";
            SQL += " 					)                                                                                           \r\n";
            SQL += " 	  , UPPS = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += " 	  , UPDT = SYSDATE                                                                                          \r\n";
            SQL += " WHERE SPECNO = '" + strSPECNO + "'                                                                             \r\n";
            if(strGWAGubun != "Y")
            {
                SQL += "   AND RECEIVEDATE IS NOT NULL";
            }
            


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

        public string up_EXAM_OUTRESULT_SENDTIME(PsmhDb pDbCon, string strROWID, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_OUTRESULT                  \r\n";
            SQL += "     SET SENDTIME    = SYSDATE                      \r\n";

            SQL += "   WHERE 1      = 1                                 \r\n";
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

        public string up_EXAM_EXAM_RESULTC_RESULT(PsmhDb pDbCon, string strRESULT, string strSPECNO, string strEXCODE, string strSMCODE, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_RESULTC                  \r\n";
            SQL += "     SET RESULT      = " + ComFunc.covSqlstr(strRESULT, false);
            SQL += "       , RESULTDATE  = SYSDATE                  \r\n";
            SQL += "       , STATUS      = 'V'                       \r\n";
            SQL += "       , RESULTSABUN = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += "       , SMCODE      = " + ComFunc.covSqlstr(strSMCODE.Trim(), false);
            SQL += "   WHERE 1 = 1                                 \r\n";
            SQL += "     AND SPECNO  = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "     AND SUBCODE = " + ComFunc.covSqlstr(strEXCODE, false);
           

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    //ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

        public string up_EXAM_EXAM_RESULTC_RESULT_CV(PsmhDb pDbCon, string strRESULT, string strSPECNO, string strEXCODE, string strSMCODE, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_RESULTC                  \r\n";
            SQL += "     SET RESULT      = " + ComFunc.covSqlstr(strRESULT, false);
            SQL += "       , RESULTDATE  = SYSDATE                  \r\n";
            SQL += "       , STATUS      = 'V'                       \r\n";
            SQL += "       , CV      = 'C'                       \r\n";
            SQL += "       , RESULTSABUN = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += "       , SMCODE      = " + ComFunc.covSqlstr(strSMCODE.Trim(), false);
            SQL += "   WHERE 1 = 1                                 \r\n";
            SQL += "     AND SPECNO  = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "     AND SUBCODE = " + ComFunc.covSqlstr(strEXCODE, false);


            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    //ComFunc.MsgBox("조회중 문제가 발생했습니다");
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

        public DataTable sel_EXAM_RESULT_IMG(PsmhDb pDbCon, string strWRTNO)
        {
            DataTable dt = null;

            SQL = "";
            SQL += "     SELECT                                     \r\n";
            SQL += "            IMAGE                               \r\n";
            SQL += "          , SEQNO                               \r\n";
            SQL += "       FROM KOSMOS_OCS.EXAM_RESULT_IMG  A       \r\n";
            SQL += "     WHERE 1=1                                  \r\n";
            SQL += "       AND WRTNO        = " + ComFunc.covSqlstr(strWRTNO, false);
            SQL += "     ORDER BY  SEQNO                            ";

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

        public DataSet sel_EXAM_OUTRESULT_COMMIT(PsmhDb pDbCon, string strFDate, string strTDate, string isCOM, bool isEXC, string strWPART, string strWPART_SUB, string strPTNO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                                        \r\n";
            SQL += "  		 '' AS CHK                                                                              \r\n";
            SQL += "   	    , TO_CHAR(A.RESULTDATE,'YYYY-MM-DD')    AS RESULTDATE --8                               \r\n";
            SQL += "    	, TO_CHAR(A.JEPDATE,'YYYY-MM-DD')     							AS JEPDATE -- 9         \r\n";
            SQL += "  		, B.PANO					-- 3                                                        \r\n";
            SQL += "  		, A.SNAME					-- 3                                                        \r\n";
            SQL += "  		, A.SMCODE                  -- 4                                                        \r\n";
            SQL += "  		, B.WORKSTS					-- 5                                                        \r\n";
            SQL += "  		, A.EXCODE					-- 5                                                        \r\n";
            SQL += "  		, A.SMNAME                  -- 6                                                        \r\n";
            SQL += "   		, A.RESULT                  -- 7                                                        \r\n";            
            SQL += "    	, TO_CHAR(A.SENDTIME,'MM/DD')                           	    AS SEND    --10         \r\n";
            SQL += "   		, A.SPECNO  															   --11         \r\n";
  		    SQL += "   		, DECODE((SELECT COUNT(*)                                                               \r\n";
  		    SQL += "   			 	  FROM KOSMOS_OCS.EXAM_RESULT_IMG                                               \r\n";
  		    SQL += "   			 	 WHERE WRTNO   = B.IMGWRTNO                                                     \r\n";
  		    SQL += "   			 	   AND SPECNO  = A.SPECNO                                                       \r\n";
  		    SQL += "   			 	   AND  ( (( A.EXCODE IS NULL AND (RESULT <> '**' AND RESULT IS NOT NULL))AND (1=1)) OR ((A.EXCODE IS NOT NULL) AND ( SUBCODE = A.EXCODE  )) )                                                        \r\n";
  		    SQL += "   			 	   ),0,'', '▥'                                                                 \r\n";
  		    SQL += "   			 || (SELECT COUNT(*)                                                                \r\n";
  		    SQL += "   			 	  FROM KOSMOS_OCS.EXAM_RESULT_IMG                                               \r\n";
  		    SQL += "   			 	 WHERE WRTNO   = B.IMGWRTNO                                                     \r\n";
  		    SQL += "   			 	   AND SPECNO  = A.SPECNO                                                       \r\n";
  		    SQL += "   			 	   AND  ( (( A.EXCODE IS NULL AND (RESULT <> '**' AND RESULT IS NOT NULL))AND (1=1)) OR ((A.EXCODE IS NOT NULL) AND ( SUBCODE = A.EXCODE  )) )                                                        \r\n";
            SQL += "   									   ) || '장')  AS IMG --12                                  \r\n";
            SQL += "   		, B.IMGWRTNO                                                                            \r\n";
            SQL += "   		, A.SENDERROR                                                                           \r\n";
            SQL += "   		, DECODE(A.EXC,'Y','True','')                                   AS EXC                  \r\n";            
            SQL += "   		, KOSMOS_OCS.FC_BAS_USER_NAME(A.VSABUN) VASBUN                                          \r\n";
            SQL += "  		, TO_CHAR(A.VDATE,'YYYY-MM-DD HH24:MI') VDATE                                           \r\n";
            SQL += "   		, A.SMSPECNO                                                                            \r\n";                       
            SQL += "  		, B.WPART                                                                               \r\n";
            SQL += "  		, A.ROWID                                                                               \r\n";
            SQL += "  		, (SELECT MAX(GB_SEND)  FROM KOSMOS_OCS.EXAM_SAMKWANG WHERE TRIM(EXCODE) = TRIM(A.EXCODE)) AS GB_SEND \r\n";                 
            SQL += "   FROM KOSMOS_OCS.EXAM_OUTRESULT A                                                             \r\n";
            SQL += "      , KOSMOS_OCS.EXAM_SPECMST   B                                                             \r\n";
            SQL += "  WHERE 1=1                                                                                     \r\n";
            SQL += "    AND A.SPECNO = B.SPECNO                                                                     \r\n";
            SQL += "    AND A.RESULTDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "                         AND " + ComFunc.covSqlDate(strTDate, false);

            if (string.IsNullOrEmpty(strPTNO) == false)
            {
                SQL += "    AND B.PANO =" + ComFunc.covSqlstr(strPTNO, false);
            }

            if (isCOM == "0")
            {
                SQL += "    AND (A.VSABUN 		IS NULL OR A.VDATE	 = '')                                         \r\n";
                SQL += "    AND A.SENDTIME 	IS NULL                                                                \r\n";
            }
            else if (isCOM == "1")
            {
                SQL += "    AND (A.VSABUN 		IS NOT NULL )                                                      \r\n";
                SQL += "    AND A.SENDTIME 	IS NULL                                                                \r\n";
            }
            else if (isCOM == "3")
            {
                // 전송
                SQL += "    AND (A.VSABUN 		IS NOT NULL )                                                      \r\n";
                SQL += "    AND A.SENDTIME 	IS NOT NULL                                                            \r\n";
            }

            if (isEXC == false)
            {
                SQL += "    AND (A.EXC 		IS NULL OR A.EXC	 = '')                                             \r\n";
            }            

            SQL += "    AND B.WPART =" + ComFunc.covSqlstr(strWPART, false);

            if (string.IsNullOrEmpty(strWPART_SUB) == false)
            {
                SQL += "    AND B.WPART_SUB =" + ComFunc.covSqlstr(strWPART_SUB, false);
            }

            SQL += "  ORDER BY A.SPECNO,A.EXCODE, RESULTDATE, SNAME                                                 \r\n";

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

        public string up_EXAM_OUTRESULT_EXC(PsmhDb pDbCon, bool isEXC, string strROWID, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_OUTRESULT                  \r\n";

            if (isEXC == true)
            {
                SQL += "     SET EXC    = 'Y'                               \r\n";
            }
            else
            {
                SQL += "     SET EXC    = ''                               \r\n";
            }

            
            SQL += "   WHERE 1      = 1                                 \r\n";
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

        public string up_EXAM_OUTRESULT_VDATE(PsmhDb pDbCon, string strROWID, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_OUTRESULT                  \r\n";
            SQL += "     SET VDATE  = SYSDATE                           \r\n";
            SQL += "       , VSABUN = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL += "   WHERE 1      = 1                                 \r\n";
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

        public string up_EXAM_SPECMST_WPART(PsmhDb pDbCon, bool isDelet, string strWDATE, string strWGBN, string strWPART, string strWPART_SUB, string strSPECNO, int nWSEQ, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_SPECMST                    \r\n";

            if (isDelet == true)
            {
                SQL += "    SET  WDATE      = '' \r\n";
                SQL += "      ,  WGBN       = '' \r\n";
                SQL += "      ,  WPART      = '' \r\n";
                SQL += "      ,  WPART_SUB  = '1' \r\n";
                SQL += "      ,  WSEQ       = '' \r\n";
            }
            else
            {

                SQL += "    SET  WDATE      = " + ComFunc.covSqlDate(strWDATE, false);
                SQL += "      ,  WGBN       = " + ComFunc.covSqlstr(strWGBN, false);
                SQL += "      ,  WPART      = " + ComFunc.covSqlstr(strWPART, false);
                SQL += "      ,  WPART_SUB  = " + ComFunc.covSqlstr(strWPART_SUB, false);

                if (nWSEQ > 0)
                {
                    SQL += "      ,  WSEQ       = " + ComFunc.covSqlstr(nWSEQ.ToString(), false);
                }
                else
                {
                    SQL += "      ,  WSEQ       = (                                                                                 \r\n";
                    SQL += "               SELECT                                                                                   \r\n";
                    SQL += "                       (                                                                                \r\n";
                    SQL += "                         CASE WHEN '1' = '" + strWPART_SUB + "' AND NVL(MAX(WSEQ),0) = '0'  THEN NVL(MAX(WSEQ),0) + 1      \r\n";
                    SQL += "                              WHEN '2' = '" + strWPART_SUB + "' AND NVL(MAX(WSEQ),0) = '0'  THEN NVL(MAX(WSEQ),0) + 201    \r\n";
                    SQL += "                              WHEN '3' = '" + strWPART_SUB + "' AND NVL(MAX(WSEQ),0) = '0'  THEN NVL(MAX(WSEQ),0) + 401    \r\n";
                    SQL += "                              WHEN '4' = '" + strWPART_SUB + "' AND NVL(MAX(WSEQ),0) = '0'  THEN NVL(MAX(WSEQ),0) + 601    \r\n";
                    SQL += "                              ELSE  NVL(MAX(WSEQ),0) +1                                                 \r\n";
                    SQL += "                           END                                                                          \r\n";
                    SQL += "                        )  A                                                                            \r\n";
                    SQL += "                      FROM KOSMOS_OCS.EXAM_SPECMST                                                      \r\n";
                    SQL += "                     WHERE WDATE     = " + ComFunc.covSqlDate(strWDATE, false);
                    SQL += "                       AND WGBN      = " + ComFunc.covSqlstr(strWGBN, false);
                    SQL += "                       AND WPART     = " + ComFunc.covSqlstr(strWPART, false);
                    SQL += "                       AND WPART_SUB = " + ComFunc.covSqlstr(strWPART_SUB, false);
                    SQL += "                    )                                                                                   \r\n";
                }
            }
            SQL += "  WHERE 1=1                                                                                                 \r\n";
            SQL += "    AND SPECNO   = " + ComFunc.covSqlstr(strSPECNO, false);

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

        public DataSet sel_EXAM_SPECMST_OUT(PsmhDb pDbCon, string strFDate, string strTDate, string strWPART, string strWGBN, string strDEPT, string strWPART_SUB, string strPANO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                     \r\n";
            SQL += "         '' CHK                                                              \r\n";
            SQL += "       , TO_CHAR(S.WDATE,'YYYY-MM-DD') AS WDATE                              \r\n";
            SQL += "       , S.SPECNO                                                            \r\n";
            SQL += "       , S.WORKSTS                                                           \r\n";
            SQL += "       , S.PANO                                                              \r\n";
            SQL += "       , S.SNAME                                                             \r\n";
            SQL += "       , S.AGE                                                               \r\n";
            SQL += "       , S.SEX                                                               \r\n";
            SQL += "       , S.WARD                                                              \r\n";
            SQL += "       , S.DEPTCODE                                                          \r\n";
            SQL += "       , KOSMOS_OCS.FC_EXAM_SPECMST_NM('14', S.SPECCODE, 'Y') AS SPECNM      \r\n";
            //2018-12-06 안정수, 삼광의료재단요청으로 검사코드 표기되도록 보완
            SQL += "       , KOSMOS_OCS.FC_EXAM_RESULTC_EXAMCODE(S.SPECNO)        AS EXAMCODE    \r\n";
            SQL += "       , KOSMOS_OCS.FC_EXAM_RESULTC_EXAMNAME(S.SPECNO)        AS EXAMNAME    \r\n";
            //2017.11.22.김홍록: 사용하지 않는 필드로 막음.
            //SQL += "       , DECODE(WCNT,0,'')WCNT                                               \r\n";
            //2019-11-06 안정수, C(카운트) 칼럼으로 사용하기 위하여 추가함 
            SQL += "       , ''                                                                  \r\n";
            SQL += "       , S.ANATNO                                                            \r\n";
            SQL += "       , S.SPECNO                      AS BARCODE                            \r\n";
            SQL += "  FROM  KOSMOS_OCS.EXAM_SPECMST S                                            \r\n";
            SQL += "  WHERE 1=1                                                                  \r\n";
            SQL += "  AND S.WDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "                  AND " + ComFunc.covSqlDate(strTDate, false);

            if (string.IsNullOrEmpty(strDEPT) == false && !strDEPT.Equals("*"))
            {
                SQL += "  AND S.DEPTCODE 	= " + ComFunc.covSqlstr(strDEPT, false);
            }
            SQL += "  AND S.WGBN 	    = " + ComFunc.covSqlstr(strWGBN, false);
            SQL += "  AND S.WPART 	    = " + ComFunc.covSqlstr(strWPART, false);
            SQL += "  AND S.WPART_SUB 	= " + ComFunc.covSqlstr(strWPART_SUB, false);

            if (string.IsNullOrEmpty(strPANO) == false)
            {
                SQL += "  AND S.PANO = " + ComFunc.covSqlstr(strPANO, false);
            }
            
            SQL += " ORDER BY  S.WDATE, S.WSEQ                                                   \r\n";

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

        public string ins_EXAM_OUTRESULT_TXT(PsmhDb pDbCon  , string strRESULTDATE
                                                            , string strSPECNO
                                                            , string strPANO
                                                            , string strSName
                                                            , string strSMSPECNO
                                                            , string strSMCODE
                                                            , string strSMNAME
                                                            , string strRESULT
                                                            , string strRECEIVEDATE
                                                            , string strJEPDATE
                                                            , string strEXCODE
                                                            , string strSENDERROR
                                                            , string strJOBTIME
                                                            , string strJOBSABUN
                                                            , ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "";

            SQL += "  INSERT INTO KOSMOS_OCS.EXAM_OUTRESULT     \r\n";
            SQL += "  	(                                       \r\n";
            SQL += "  	  RESULTDATE                            \r\n";
            SQL += "  	, SPECNO                                \r\n";
            SQL += "  	, PANO                                  \r\n";
            SQL += "  	, SName                                 \r\n";
            SQL += "  	, SMSPECNO                              \r\n";
            SQL += "  	, SMCODE                                \r\n";
            SQL += "  	, SMNAME                                \r\n";
            SQL += "  	, RESULT                                \r\n";
            SQL += "  	, RECEIVEDATE                           \r\n";
            SQL += "  	, JEPDATE                               \r\n";
            SQL += "  	, EXCODE                                \r\n";
            SQL += "  	, SENDERROR                             \r\n";
            SQL += "  	, JOBTIME                               \r\n";
            SQL += "  	, JOBSABUN                              \r\n";
            SQL += "  	)                                       \r\n";
            SQL += "  	VALUES                                  \r\n";
            SQL += "  	(                                       \r\n";
            SQL += "  	  TO_DATE('" + strRESULTDATE + "','YYYYMMDD')        \r\n";
            SQL += "  	" + ComFunc.covSqlstr(strSPECNO, true);
            SQL += "  	" + ComFunc.covSqlstr(strPANO, true);
            SQL += "  	" + ComFunc.covSqlstr(strSName, true);
            SQL += "  	" + ComFunc.covSqlstr(strSMSPECNO, true);
            SQL += "  	" + ComFunc.covSqlstr(strSMCODE, true);
            SQL += "  	" + ComFunc.covSqlstr(strSMNAME, true);
            SQL += "  	" + ComFunc.covSqlstr(strRESULT, true);
            SQL += "  	, TO_DATE('" + strRECEIVEDATE + "','YYYY-MM-DD HH24:MI')      \r\n";
            SQL += "  	, TO_DATE('" + strJEPDATE + "','YYYYMMDD')          \r\n";
            SQL += "  	" + ComFunc.covSqlstr(strEXCODE, true);
            SQL += "  	" + ComFunc.covSqlstr(strSENDERROR, true);
            SQL += "  	,SYSDATE                                            \r\n";
            SQL += "  	" + ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "  	)                                                   \r\n";


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

        public DataTable sel_EXAM_RESULTC_WRTNO(PsmhDb pDbCon, string strSMCODE, string strSEQNO, string strSPECNO, string strSMNAME, bool isIMG )
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                                                                         \r\n";
            SQL += "           NVL(B.IMGWRTNO, 0)   AS IMGWRTNO                                                     \r\n";
            SQL += "         , TO_CHAR(A.BDATE,'YYYY-MM-DD') AS BDATE                                               \r\n";
            SQL += "         , A.PANO                                                                               \r\n";
            SQL += "         , A.SNAME			    AS SNAME                                                        \r\n";
            SQL += "         , A.SPECNO                                                                             \r\n";
            SQL += "         , B.MASTERCODE                                                                         \r\n";
            SQL += "         , B.SUBCODE                                                                            \r\n";
            SQL += "         , C.SMCODE         	AS SMCODE                                                       \r\n";
            SQL += "         , '" + strSEQNO + "'	AS SEQNO                                                        \r\n";
            SQL += "         , C.SMNAME                                                                             \r\n";
            SQL += "         , KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(B.SUBCODE) AS EXAMNAME                            \r\n";
            SQL += "         , C.EXCODE                                                                             \r\n";
            SQL += "         , B.STATUS                                                                             \r\n";
            SQL += "         , TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD HH24:MI')  AS RECEIVEDATE                          \r\n";
            SQL += "  		 , DECODE(B.IMGWRTNO,NULL,'', '▥'                                                      \r\n"; 
            SQL += "  								|| (SELECT COUNT(*)                                             \r\n";
            SQL += "  									  FROM KOSMOS_OCS.EXAM_RESULT_IMG                           \r\n";
            SQL += "  									 WHERE WRTNO  = B.IMGWRTNO                                  \r\n";
            SQL += "  									   AND SPECNO = A.SPECNO) || '장')  AS IMG --12             \r\n";
            SQL += "  		 , C.GB_IMG                                                                             \r\n";
            SQL += "  		 , A.WPART                                                                              \r\n";
            SQL += "       FROM KOSMOS_OCS.EXAM_SPECMST  A          \r\n";
            SQL += "          , KOSMOS_OCS.EXAM_RESULTC  B          \r\n";
            SQL += "          , KOSMOS_OCS.EXAM_SAMKWANG C          \r\n";
            SQL += "     WHERE 1=1                                  \r\n";
            SQL += "       AND A.SPECNO     = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "       AND A.SPECNO     = B.SPECNO                 \r\n";
            SQL += "       AND B.SUBCODE    = C.EXCODE (+)             \r\n";
            SQL += "       AND C.SMCODE(+)  = " + ComFunc.covSqlstr(strSMCODE, false);


            if (isIMG == false)
            {
                SQL += "       AND C.GB_IMG(+)  = '0'   \r\n";
            }
            else
            {
                SQL += "       AND C.GB_IMG(+)  = '1'   \r\n";
            }
            
            if (string.IsNullOrEmpty(strSMNAME) == false)
            {
                SQL += "       AND TRIM(C.SMNAME(+))  = " + ComFunc.covSqlstr(strSMNAME.Trim(), false);
            }

            SQL += "       ORDER BY B.SUBCODE                            \r\n";

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

        public DataSet sel_EXAM_OUTRESULT(PsmhDb pDbCon, string strFDate, string strTDate)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "    SELECT                                                                  \r\n";
            SQL += "   		  ''                                 AS FILE_PATH                   \r\n";
            SQL += "   		, TO_CHAR(B.BDATE,'YYYY-MM-DD')      AS BDATE                       \r\n";
            SQL += "   		, B.PANO                             AS PANO                        \r\n";
            SQL += "   		, B.SNAME                            AS SNAME                       \r\n";
            SQL += "   		, A.SPECNO							 AS SPECNO                      \r\n";
            SQL += "   		, A.SMSPECNO        				 AS SMSPECNO                    \r\n";
            SQL += "   		, C.MASTERCODE                       AS MASTERCODE                  \r\n";
            SQL += "   		, C.SUBCODE                          AS SUBCODE                     \r\n";
            SQL += "   		, A.SMCODE                           AS SAMCODE                     \r\n";
            SQL += "   		, (                                                                 \r\n";
            SQL += "   	  	   SELECT EXCODE                                                    \r\n";
            SQL += "   			 FROM KOSMOS_OCS.EXAM_SAMKWANG                                  \r\n";
            SQL += "   			WHERE SMCODE = A.SMCODE                                         \r\n";
            SQL += "   			  AND EXCODE = C.SUBCODE                                        \r\n";
            SQL += "   		   )        						 AS EXCODE                      \r\n";
            SQL += "   		, A.SMNAME                           AS SMNAME                      \r\n";
            SQL += "   		, KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(C.SUBCODE) AS EXAMNAME         \r\n";
            SQL += "   		, A.RESULT                           AS RESULT                      \r\n";
            SQL += "  		, DECODE(B.IMGWRTNO,NULL,'', '▥'                                                      \r\n";
            SQL += "  								|| (SELECT COUNT(*)                                             \r\n";
            SQL += "  									  FROM KOSMOS_OCS.EXAM_RESULT_IMG                           \r\n";
            SQL += "  									 WHERE WRTNO  = B.IMGWRTNO                                  \r\n";
            SQL += "  									   AND SPECNO = B.SPECNO) || '장')  AS IMG --12             \r\n";
            SQL += "   		, B.IMGWRTNO                         AS IMGWRTNO                    \r\n";
            SQL += "   		, A.SENDERROR                        AS SENDERROR                   \r\n";
            SQL += "   		, TO_CHAR(B.RECEIVEDATE,'YYYY-MM-DD HH24:mi')AS RECEIVEDATE         \r\n";
            SQL += "   		, TO_CHAR(A.JEPDATE,'YYYYMMDD')      AS JEPDATE                     \r\n";
            SQL += "   		, TO_CHAR(A.RESULTDATE,'YYYYMMDD')   AS RESULTDATE                  \r\n";
            SQL += "   		, ''                                 AS STATE                       \r\n";
            SQL += "   		, A.ROWID                                                           \r\n";
            SQL += "   		, '' 								AS CHANGE                       \r\n";
            SQL += "     FROM  KOSMOS_OCS.EXAM_OUTRESULT   A                                    \r\n";
            SQL += "         , KOSMOS_OCS.EXAM_SPECMST     B                                    \r\n";
            SQL += "         , KOSMOS_OCS.EXAM_RESULTC     C                                    \r\n";
            SQL += "    WHERE 1=1                                                               \r\n";
            SQL += "      AND A.RESULTDATE BETWEEN " + ComFunc.covSqlDate(strFDate, false);
            SQL += "                           AND " + ComFunc.covSqlDate(strTDate, false);
            SQL += "      AND A.SENDTIME IS NULL                                                \r\n";
            SQL += "      AND A.SPECNO	 = B.SPECNO(+)                                          \r\n";
            SQL += "      AND A.SPECNO   = C.SPECNO(+)                                          \r\n";
            SQL += "      AND A.EXCODE   = C.SUBCODE(+)                                         \r\n";
            SQL += "    ORDER BY A.RESULTDATE,A.SPECNO,A.SMCODE                                 \r\n";


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

        public string save_EXAM_SAMKWANG(PsmhDb pDbCon, string strSMCODE, string strEXCODE, string strSMNAME, string strGB_IMG, string strGB_SEND, ref int intRowAffected, string GbnCom = "")
        {
            string SqlErr = "";

            SQL = "";


            SQL += "  MERGE INTO KOSMOS_OCS.EXAM_SAMKWANG  A                                    \r\n";
            SQL += "       USING DUAL                                                           \r\n";
            SQL += "  	    ON (                                                                \r\n";
            SQL += "  	    	    A.SMCODE = " + ComFunc.covSqlstr(strSMCODE, false);
            SQL += "  	        AND A.EXCODE = " + ComFunc.covSqlstr(strEXCODE, false);
            SQL += "  	        AND A.SMNAME = " + ComFunc.covSqlstr(strSMNAME, false);
            SQL += "  	        )                                                               \r\n";
            SQL += "  		WHEN     MATCHED THEN                                               \r\n";
            SQL += "  		UPDATE SET ENTSABUN = ENTSABUN                                      \r\n";
            SQL += "  		WHEN NOT MATCHED THEN                                               \r\n";
            SQL += "            INSERT                                                          \r\n";
            SQL += "              (                                                             \r\n";
            SQL += "                 SMCODE     -- 삼광의료재단 코드                            \r\n";
            SQL += "               , EXCODE     -- 병원의 검사코드                              \r\n";
            SQL += "               , SMNAME     -- 삼광의료재단 검사명                          \r\n";
            SQL += "               , ENTDATE    -- 최종 등록/변경 일자 및 시각                  \r\n";
            SQL += "               , ENTSABUN   -- 최종 등록/변경 작업자 사번                   \r\n";
            SQL += "               , GB_IMG                                                     \r\n";
            SQL += "               , GB_SEND                                                    \r\n";
            SQL += "               , GB_COM                                                    \r\n";
            SQL += "              )                                                             \r\n";
            SQL += "              VALUES (                                                      \r\n";
            SQL += "              " + ComFunc.covSqlstr(strSMCODE, false);
            SQL += "              " + ComFunc.covSqlstr(strEXCODE, true);
            SQL += "              " + ComFunc.covSqlstr(strSMNAME, true);
            SQL += "              , SYSDATE                                                     \r\n";
            SQL += "              " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "              " + ComFunc.covSqlstr(strGB_IMG, true);
            SQL += "              " + ComFunc.covSqlstr(strGB_SEND, true);
            if(GbnCom == "1")
            {
                SQL += "              ,'1'";
            }
            else if (GbnCom == "2")
            {
                SQL += "              ,'2'";
            }
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

        public string up_EXAM_SAMKWANG(PsmhDb pDbCon, string strEXCODE, string strSMNAME, string strROWID, string strGB_IMG, string strGB_SEND, ref int intRowAffected, string GbnCom = "")
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_SAMKWANG       \r\n";
            SQL += "    SET                                 \r\n";
            SQL += "         EXCODE  = " + ComFunc.covSqlstr(strEXCODE, false);  /* 검사방법6*/
            SQL += "       , SMNAME  = " + ComFunc.covSqlstr(strSMNAME, false);  /* 검사방법6*/
            SQL += "       , GB_IMG  = " + ComFunc.covSqlstr(strGB_IMG, false);  /* 검사방법6*/
            SQL += "       , GB_SEND  = " + ComFunc.covSqlstr(strGB_SEND, false);  /* 검사방법6*/
            SQL += "  WHERE 1=1                      \r\n";
            SQL += "    AND ROWID    = " + ComFunc.covSqlstr(strROWID, false);

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

        public string del_EXAM_SAMKWANG(PsmhDb pDbCon, string strROWID, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  DELETE KOSMOS_OCS.EXAM_SAMKWANG       \r\n";
            SQL += "  WHERE 1=1                             \r\n";
            SQL += "    AND ROWID    = " + ComFunc.covSqlstr(strROWID, false);

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

        public bool isEXAM_OUTRESULT(PsmhDb pDbCon,string strRESULTDATE, string strSPECNO, string strSMSPECNO, string strSMCODE, string strSMNAME, string strRESULT)
        {
            bool b = false;

            DataTable dt = null;

            SQL = "";
            SQL += " SELECT COUNT(*) CNT                                    \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_OUTRESULT                       \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            //TODO : 2017.11.27.김홍록 SMSPECNO와 SPECNO는 다른 경우가 많다...
            // 주말에 먼저 이미지를 올리면 오늘날짜가 들어가고, 텍스트는 삼광에서 보내준 데이터가 들어 가니 다른 데이터로 본다. 내일
            // 재검에 대한 것을 문의 해 보자
            //SQL += "    AND RESULTDATE  = " + ComFunc.covSqlDate(strRESULTDATE, false);            
            //SQL += "    AND SMSPECNO   = " + ComFunc.covSqlstr(strSMSPECNO, false);
            SQL += "    AND SPECNO       = " + ComFunc.covSqlstr(strSPECNO.Trim(), false);
            SQL += "    AND SMCODE	     = " + ComFunc.covSqlstr(strSMCODE.Trim(), false);
            SQL += "    AND TRIM(SMNAME) = " + ComFunc.covSqlstr(strSMNAME.Trim(), false);


            //if (string.IsNullOrEmpty(strRESULT.Trim()) == false && !strRESULT.Equals("**"))
            //{
            //    SQL += "    AND RESULT = " + ComFunc.covSqlstr(strRESULT.Trim(), false);
            //}

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return true;
                }

                if (ComFunc.isDataTableNull(dt) == false)
                {
                    if (Convert.ToInt32(dt.Rows[0][0].ToString()) > 0)
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
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return true;
            }

            return b;
        }        
      
        public DataSet sel_EXAM_RESULT_IMG_IMAGE(PsmhDb pDbCon, string strSPECNO, string strPTNO)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                         \r\n";
            SQL += " 	  '' CHK                                    \r\n";
            SQL += " 	, TO_CHAR(A.SDATE,'YYYY-MM-DD') BDATE       \r\n";
            SQL += " 	, D.RECEIVEDATE                             \r\n";
            SQL += " 	, C.EXAMFNAME                               \r\n";
            SQL += " 	, '' AS CHK_IMAGE                           \r\n";
            SQL += " 	, A.IMAGE                                   \r\n";
            SQL += " 	, A.ROWID                                   \r\n";
            SQL += "   FROM KOSMOS_OCS.EXAM_RESULT_IMG  	A       \r\n";
            SQL += "      , KOSMOS_OCS.EXAM_RESULTC 		B       \r\n";
            SQL += "      , KOSMOS_OCS.EXAM_MASTER 		    C       \r\n";
            SQL += "      , KOSMOS_OCS.EXAM_SPECMST 		D       \r\n";

            SQL += "  WHERE 1=1                                     \r\n";
            if (string.IsNullOrEmpty(strSPECNO) == false)
            {
                SQL += "    AND D.SPECNO    = " + ComFunc.covSqlstr(strSPECNO, false);
            }
            else
            {
                SQL += "    AND A.PTNO 		= " + ComFunc.covSqlstr(strPTNO, false);
            }
            
            SQL += "    AND A.SPECNO 	 = B.SPECNO                  \r\n";
            SQL += "    AND A.MASTERCODE = B.SUBCODE                \r\n";
            SQL += "    AND A.SPECNO 	 = D.SPECNO                  \r\n";
            SQL += "    AND B.SUBCODE 	 = C.MASTERCODE              \r\n";
            SQL += "    AND A.WRTNO 	 = D.IMGWRTNO                \r\n";
            SQL += "    ORDER BY A.SEQNO DESC                       \r\n";

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

        public string up_EXAM_SPECMST_IMGWRTNO(PsmhDb pDbCon, string strSPECNO, string strIMGWRTNO, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_SPECMST  \r\n";
            SQL += "    SET                                 \r\n";
            SQL += "         IMGWRTNO  = " + ComFunc.covSqlstr(strIMGWRTNO, false);  /* 검사방법6*/
            SQL += "  WHERE 1=1                      \r\n";
            SQL += "    AND SPECNO     = " + ComFunc.covSqlstr(strSPECNO, false);

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

        public string ins_EXAM_RESULT_IMG(PsmhDb pDbCon, string strWRTNO, string strSDATE, string strPTNO, string strSEQNO, string strSPECNO, string strMASTERCODE, string strSUBCODE, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  INSERT INTO KOSMOS_OCS.EXAM_RESULT_IMG (WRTNO, GUBUN, SDATE, PTNO, SEQNO, SPECNO, MASTERCODE , SUBCODE  ) \r\n";
            SQL += "    VALUES (                                                                                                \r\n";
            SQL += "    " + ComFunc.covSqlstr(strWRTNO      , false);
            SQL += "    " + ComFunc.covSqlstr("1"           , true);
            SQL += "    " + ComFunc.covSqlDate(strSDATE     , true);
            SQL += "    " + ComFunc.covSqlstr(strPTNO       , true);
            SQL += "    " + ComFunc.covSqlstr(strSEQNO      , true);
            SQL += "    " + ComFunc.covSqlstr(strSPECNO     , true);
            SQL += "    " + ComFunc.covSqlstr(strMASTERCODE , true);
            SQL += "    " + ComFunc.covSqlstr(strSUBCODE    , true);
            SQL += "    )           \r\n";

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

        public string up_EXAM_RESULT_IMG_IMAGE(PsmhDb pDbCon, string strWRTNO, string strSPECNO, string strPTNO, string strSEQNO, byte[] IMG, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_RESULT_IMG  \r\n";
            SQL += "     SET IMAGE = :IMG                \r\n";
            SQL += "   WHERE 1=1                         \r\n";
            SQL += "     AND WRTNO  = " + ComFunc.covSqlstr(strWRTNO, false);
            SQL += "     AND SPECNO = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "     AND GUBUN  = '1'               \r\n";
            SQL += "     AND PTNO   = " + ComFunc.covSqlstr(strPTNO, false);
            SQL += "     AND SEQNO  = " + ComFunc.covSqlstr(strSEQNO, false);


            SqlErr = clsDB.ExecuteLongRawQuery(SQL, IMG, ref intRowAffected, pDbCon);
            return SqlErr;
        }
       
        public string up_EXAM_RESULTC(PsmhDb pDbCon, string strSPECNO, string strIMGWRTNO, string strSUBCODE, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "  UPDATE KOSMOS_OCS.EXAM_RESULTC        \r\n";
            SQL += "    SET                                 \r\n";
            SQL += "         IMGWRTNO  = " + ComFunc.covSqlstr(strIMGWRTNO, false);  /* 검사방법6*/
            SQL += "  WHERE 1=1                             \r\n";
            SQL += "    AND SPECNO     = " + ComFunc.covSqlstr(strSPECNO, false);
            SQL += "    AND RESULTWS IN ('W','M','Z')           \r\n";
            SQL += "    AND SUBCODE    = " + ComFunc.covSqlstr(strSUBCODE, false);


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

        public int sel_EXAM_OUTRESULT_CNT(PsmhDb pDbCon, string strSMCODE, string strEXCODE)
        {
            int nReturn = 0;

            DataTable dt = null;

            SQL = "";

            SQL += "SELECT COUNT(*) FROM KOSMOS_OCS.EXAM_OUTRESULT          \r\n";
            SQL += "WHERE RESULTDATE BETWEEN ADD_MONTHS(SYSDATE,(-12 * 5))  \r\n";
            SQL += "                     AND SYSDATE                        \r\n";
            SQL += "  AND  SMCODE = " + ComFunc.covSqlstr(strSMCODE, false);
            SQL += "  AND  EXCODE = " + ComFunc.covSqlstr(strEXCODE, false);

            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return -1;
                }

                nReturn = Convert.ToInt32(dt.Rows[0][0].ToString());

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return -1;
            }

            return nReturn;
        }

        public string save_EXAM_OUTSULT_TXT(PsmhDb pDbCon, string strRESULTDATE, string strSPECNO, string strPANO, string strSNAME, string strSMSPECNO, string strSMCODE, string strSMNAME, string strRESULT, string strRECEIVEDATE, string strJEPDATE, string strEXCODE, string strSENDERROR, ref int intRowAffected)

        {
            string SqlErr = "";

            SQL = "";

            SQL += "  MERGE INTO KOSMOS_OCS.EXAM_OUTRESULT              \r\n";
            SQL += "       USING DUAL                                   \r\n";
            SQL += "          ON (                                      \r\n";
            SQL += "          	    SPECNO  = '" + strSPECNO + "'     \r\n";
            SQL += "            AND SMCODE  = '" + strSMCODE + "'     \r\n";
            SQL += "            AND EXCODE  = '" + strEXCODE + "'     \r\n";
            SQL += "  	   		 )                                      \r\n";
            SQL += " 	WHEN MATCHED THEN                               \r\n";
            SQL += " 		UPDATE SET SNAME =SNAME                     \r\n";
            SQL += "    WHEN NOT MATCHED THEN                           \r\n";
            SQL += "   INSERT                                           \r\n";
            SQL += "         (                                          \r\n";
            SQL += "                RESULTDATE                          \r\n";
            SQL += "              , SPECNO                              \r\n";
            SQL += "              , PANO                                \r\n";
            SQL += "              , SNAME                               \r\n";
            SQL += "              , SMSPECNO                            \r\n";
            SQL += "              , SMCODE                              \r\n";
            SQL += "              , SMNAME                              \r\n";
            SQL += "              , RESULT                              \r\n";
            SQL += "              , RECEIVEDATE                         \r\n";
            SQL += "              , JEPDATE                             \r\n";
            SQL += "              , EXCODE                              \r\n";
            SQL += "              , SENDERROR                           \r\n";
            SQL += "              , JOBTIME                             \r\n";
            SQL += "              , JOBSABUN                            \r\n";
            SQL += "          )                                         \r\n";
            SQL += "    VALUES                                          \r\n";
            SQL += "    		(                                       \r\n";
            SQL += "             TO_DATE('" + strRESULTDATE + "','YYYYMMDD')  \r\n";
            SQL += "             " + ComFunc.covSqlstr(strSPECNO      , true);
            SQL += "             " + ComFunc.covSqlstr(strPANO        , true);
            SQL += "             " + ComFunc.covSqlstr(strSNAME       , true);
            SQL += "             " + ComFunc.covSqlstr(strSMSPECNO    , true);
            SQL += "             " + ComFunc.covSqlstr(strSMCODE      , true);
            SQL += "             " + ComFunc.covSqlstr(strSMNAME      , true);
            SQL += "             " + ComFunc.covSqlstr(strRESULT      , true);
            SQL += "            ,TO_DATE('" + strRECEIVEDATE + "','YYYY-MM-DD HH24:MI')  \r\n";
            SQL += "            ,TO_DATE('" + strJEPDATE + "','YYYYMMDD')  \r\n";
            SQL += "             " + ComFunc.covSqlstr(strEXCODE      , true);
            SQL += "             " + ComFunc.covSqlstr(strSENDERROR   , true);
            SQL += "             , SYSDATE                              \r\n";
            SQL += "             " + ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += "    		)                                       \r\n";

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

        public string save_EXAM_OUTRESULT_IMG(PsmhDb pDbCon, string strSAMCODE, string strSAMNAME, string strSUBCODE, string strSEQNO, string strSPECNO, string strSMSPECNO, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";

            SQL += "  MERGE INTO KOSMOS_OCS.EXAM_OUTRESULT A            \r\n";
            SQL += "       USING                                        \r\n";
            SQL += "       		(                                       \r\n";
            SQL += "              SELECT                                \r\n";
            SQL += " 	              TRUNC(SYSDATE) AS RESULTDATE      \r\n";
            SQL += " 	            , SPECNO    	 AS SPECNO          \r\n";
            SQL += " 	            , PANO           AS PANO            \r\n";
            SQL += " 	            , SNAME          AS SNAME           \r\n";
            SQL += " 	            , '" + strSMSPECNO + "'          AS SMSPECNO        \r\n";
            SQL += " 	            , '" + strSAMCODE  + "'          AS SMCODE          \r\n";
            SQL += " 	            , '" + strSAMNAME  + "'          AS SMNAME          \r\n";
            SQL += " 	            , '별지참조'     AS RESULT          \r\n";  //2017.12.02.별지참조 추가
            SQL += " 	            , RECEIVEDATE    AS RECEIVEDATE     \r\n";
            SQL += " 	            , SYSDATE        AS JEPDATE         \r\n";
            // 삼광코드와 병원 코드가 없으면 여기 안들어 가겠지....
            SQL += " 	            , (                                 \r\n";
            SQL += " 	               SELECT MAX(EXCODE)               \r\n";
            SQL += " 	                 FROM KOSMOS_OCS.EXAM_SAMKWANG  \r\n";
            SQL += " 	                WHERE SMCODE = '" + strSAMCODE + "'   \r\n";
            SQL += " 	                  AND EXCODE = '" + strSUBCODE + "'   \r\n";
            SQL += " 	              )              AS EXCODE          \r\n";
            SQL += " 	            , ''             AS SENDTIME        \r\n";
            SQL += " 	            , ''             AS SENDERROR       \r\n";
            SQL += " 	            , SYSDATE        AS JOBTIME         \r\n";
            SQL += " 	            , '" + clsType.User.IdNumber + "'    AS JOBSABUN   \r\n";
            SQL += " 	    FROM KOSMOS_OCS.EXAM_SPECMST                \r\n";
            SQL += " 	   WHERE SPECNO = '" + strSPECNO + "'           \r\n";
            SQL += "       		) B                                     \r\n";
            SQL += "          ON (                                      \r\n";

            // TOdo : 2017.11.27.김홍록: 결과와 이미지가 다른날이 있을까? Sql = Sql & "WHERE ResultDate= TRUNC(SYSDATE) "
            SQL += "          	    A.SPECNO  = '" + strSPECNO  + "'    \r\n";
            SQL += "            AND A.SMCODE  = '" + strSAMCODE + "'    \r\n";
            SQL += "  	   		 )                                      \r\n";
            SQL += " 	WHEN MATCHED THEN                               \r\n";
            SQL += " 		UPDATE SET SNAME =SNAME                     \r\n";
            SQL += "    WHEN NOT MATCHED THEN                           \r\n";
            SQL += "   INSERT                                           \r\n";
            SQL += "         (                                          \r\n";
            SQL += "                RESULTDATE                          \r\n";
            SQL += "              , SPECNO                              \r\n";
            SQL += "              , PANO                                \r\n";
            SQL += "              , SNAME                               \r\n";
            SQL += "              , SMSPECNO                            \r\n";
            SQL += "              , SMCODE                              \r\n";
            SQL += "              , SMNAME                              \r\n";
            SQL += "              , RESULT                              \r\n";
            SQL += "              , RECEIVEDATE                         \r\n";
            SQL += "              , JEPDATE                             \r\n";
            SQL += "              , EXCODE                              \r\n";
            SQL += "              , SENDTIME                            \r\n";
            SQL += "              , SENDERROR                           \r\n";
            SQL += "              , JOBTIME                             \r\n";
            SQL += "              , JOBSABUN                            \r\n";
            SQL += "          )                                         \r\n";
            SQL += "    VALUES                                          \r\n";
            SQL += "    		(                                       \r\n";
            SQL += "                B.RESULTDATE                        \r\n";
            SQL += "              , B.SPECNO                            \r\n";
            SQL += "              , B.PANO                              \r\n";
            SQL += "              , B.SNAME                             \r\n";
            SQL += "              , B.SMSPECNO                          \r\n";
            SQL += "              , B.SMCODE                            \r\n";
            SQL += "              , B.SMNAME                            \r\n";
            SQL += "              , B.RESULT                            \r\n";
            SQL += "              , B.RECEIVEDATE                       \r\n";
            SQL += "              , B.JEPDATE                           \r\n";
            SQL += "              , B.EXCODE                            \r\n";
            SQL += "              , B.SENDTIME                          \r\n";
            SQL += "              , B.SENDERROR                         \r\n";
            SQL += "              , B.JOBTIME                           \r\n";
            SQL += "              , B.JOBSABUN                          \r\n";
            SQL += "    		)                                       \r\n";

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

        public int sel_EXAM_RESULT_IMG_WRTNO(PsmhDb pDbCon)
        {
            int nReturn = 0;

            DataTable dt = null;

            SQL = "";

            SQL += "SELECT MAX(WRTNO ) MWRTNO FROM KOSMOS_OCS.EXAM_RESULT_IMG";


            try
            {
                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return -1;
                }

                nReturn = Convert.ToInt32(dt.Rows[0][0].ToString());

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return -1;
            }

            return nReturn;
        }
                    
    }
}
