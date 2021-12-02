using ComBase; //기본 클래스
using ComDbB; //DB연결
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;

namespace ComSupLibB.Com 
{ 
    /// <summary>
    /// Class Name      : ComSupLibB.SupLbEx
    /// File Name       : clsComSQL.cs
    /// Description     : 진단검사 쿼리
    /// Author          : 김홍록
    /// Create Date     : 2017-05-15
    /// Update History  :  
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref= "신규" />
    public class clsComSQL : clsMethod 
    {
         
        String SQL;

        public DataTable gDt_OCS_OORDER = new DataTable();
        
        /// <summary>외래오더에 대한 데이터 1로우 값</summary>
        public DataRow gDr_OCS_OORDER;

        /// <summary>외래오더에 대한 데이터 컬럼</summary>
        public enum enm_OCS_OORDER { PTNO,BDATE,DEPTCODE,SEQNO,ORDERCODE,SUCODE,BUN,SLIPNO,REALQTY,QTY,NAL,GBDIV,DOSCODE,GBBOTH,GBINFO,GBER,GBSELF,GBSPC,BI,DRCODE,REMARK,ENTDATE,GBSUNAP,TUYAKNO,ORDERNO,MULTI,MULTIREMARK,DUR,RESV,SCODESAYU,SCODEREMARK,GBSEND,AUTO_SEND,RES,GBSPC_NO,WRTNO,CERTNO,GBAUTOSEND,OCSDRUG,RESAMT,GBCOPY,GBAUTOSEND2,GUBUN,GBFM,SABUN,GBTAX,GBGUME_ACT,IP,ASA,PCHASU,SUBUL_WARD,ROWID };

        /// <summary>환자정보조회컬럼</summary>
        public enum enmSelBasPatient { PANO, SNAME, JUMIN1, SEX };

        /// <summary>환자정보조회컬럼</summary>
        public enum enmSelBasPatient_Blood { PANO, SNAME, JUMIN1, SEX };

        /// <summary>환자통합조회</summary>
        public enum enmSearchType { PTINFO, ERPINFO, BLOODINFO,SPECINFO};
       
        /// <summary>EXAM_ORDER의 처방정리용</summary>
        public enum enmSel_EXAM_ORDER_CLEAR { CHK, BDATE, DEPTCODE, TYPE, MASTERCODE, QTY, MASTER_NM, SPECCODE, STRT, DRCOMMENT, ORDERNO, ROWID };
        public string[] sSel_EXAM_ORDER_CLEAR = { "선택", "처방일자", "과", "구분", "오더코드", "수량", "오더명칭", "검체코드", "응급", "의사 전달사항", "처방번호", "rowid" };
        public int[] nSel_EXAM_ORDER_CLEAR = { nCol_SCHK, nCol_TIME, nCol_DPCD, 50, nCol_EXCD, nCol_AGE, nCol_NAME, nCol_EXCD, nCol_CHEK, nCol_NAME, 80, 1 };

        public enum enmSel_EXAM_ORDER_CR59B {BDATE,PANO,SNAME,AGE,SEX,MASTERCODE,EXAMNAME,RESULT,SPECNO, ROWID};
        public string[] sSel_EXAM_ORDER_CR59B  = { "처방일자", "등록번호", "성명", "성별", "나이", "검사코드", "검사명", "결과", "SPECNO","rowid" };
        public int[]    nSel_EXAM_ORDER_CR59B  = { nCol_DATE, nCol_PANO, nCol_SNAME, nCol_AGE, nCol_SEX, nCol_EXCD, nCol_NAME, nCol_SPNO, 5, 5 };

        public enum enmSel_OCS_DOCTOR_DRCODE { DRCODE,GBOUT };
        
        public enum enmSel_BAS_SUN { BCODE, SUNEXT,SUNAMEE,SUNAMEK,BUN,BUN_NAME };
        public string[] sSel_BAS_SUN  = {"표준코드", "수가코드",  "수가약어", "수가한글명","분류코드"    ,"분류명칭" };
        public int[]    nSel_BAS_SUN  = { nCol_EXCD,  nCol_EXCD,   nCol_NAME,   nCol_NAME, nCol_EXCD - 30, nCol_NAME };

        public enum enmSel_BAS_PATIENT{ PANO, SNAME, SEX, JUMIN1, JUMIN2, STARTDATE, LASTDATE, ZIPCODE1, ZIPCODE2, JUSO, JICODE, TEL, SABUN, EMBPRT, BI, PNAME, GWANGE, KIHO, GKIHO, DEPTCODE, DRCODE, GBSPC, GBGAMEK, JINILSU, JINAMT, TUYAKGWA, TUYAKMONTH, TUYAKJULDATE, TUYAKILSU, BOHUN, REMARK, RELIGION, GBMSG, XRAYBARCODE, ARSCHK, BUNUP, BIRTH, GBBIRTH, EMAIL, GBINFOR, JIKUP, HPHONE, GBJUGER, GBSMS, GBJUSO, BICHK, HPHONE2, JUSAMSG, EKGMSG, BIDATE, MISSINGCALL, AIFLU, TEL_CONFIRM, GBSMS_DRUG, GBINFO_DETAIL, GBINFOR2, ROAD, ROADDONG, JUMIN3, GBFOREIGNER, ENAME, CASHYN, GB_VIP, GB_VIP_REMARK, GB_VIP_SABUN, GB_VIP_DATE, ROADDETAIL, GB_VIP2, GB_VIP2_REAMRK, GB_SVIP, WEBSEND, WEBSENDDATE, GBMERS, OBST, ZIPCODE3, BUILDNO, PT_REMARK, TEMPLE, C_NAME, GBCOUNTRY        };

        /// <summary>처방전송지원부서</summary>
        public enum enmSENDDEPT { FNEX, ENDS, IJRM, LBEX, RHBT, XRAY };

        public enum enmGBIO {I,O };

        List<string> sel_PATIENT_SCHDUL_TIME(PsmhDb pDbCon, string strPANO)
        {
            List<string> s = new List<string>();

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string sAMT = "0";

            SQL = "";

            SQL += " SELECT                                                                                         \r\n";
            SQL += " 	       MAX(TO_CHAR(ACTDATE,'YYYY-MM-DD')) 	AS BDATE                                        \r\n";
            SQL += " 	  FROM KOSMOS_PMPA.OPD_MASTER                                                               \r\n";
            SQL += " 	 WHERE 1=1                                                                                  \r\n";
            SQL += " 	   AND PANO     = " + ComFunc.covSqlstr(strPANO, false);
            SQL += " 	   AND ACTDATE  > SYSDATE-365                                                                \r\n";
            SQL += " 	GROUP BY ACTDATE                                                                            \r\n";
            SQL += " 	UNION                                                                                       \r\n";
            SQL += " 	 SELECT                                                                                     \r\n";
            SQL += " 	        TO_CHAR(DATE3,'YYYY-MM-DD' ) 		AS BDATE                                        \r\n";
            SQL += " 	   FROM KOSMOS_PMPA.OPD_RESERVED_NEW                                                        \r\n";
            SQL += " 	  WHERE 1 = 1                                                                               \r\n";
            SQL += " 	   AND Pano     = " + ComFunc.covSqlstr(strPANO, false);
            SQL += " 	   AND (RETDATE IS NULL OR RETDATE  ='')                                                    \r\n";
            SQL += " 	   AND (TRANSDATE IS NULL OR TRANSDATE='' OR TRUNC(TRANSDATE) > TRUNC(SYSDATE) )            \r\n";
            SQL += " 	GROUP BY TO_CHAR(DATE3,'YYYY-MM-DD' )                                                       \r\n";
            SQL += " 	UNION                                                                                       \r\n";
            SQL += " 	 SELECT                                                                                     \r\n";
            SQL += " 	  		 TO_CHAR(OPDATE,'YYYY-MM-DD')   	AS BDATE                                        \r\n";
            SQL += " 	   FROM KOSMOS_PMPA.ORAN_MASTER                                                             \r\n";
            SQL += " 	  WHERE 1 = 1                                                                               \r\n";
            SQL += " 	   AND Pano     =   " + ComFunc.covSqlstr(strPANO, false);
            SQL += " 	   AND OpCancel IS NULL                                                                     \r\n";
            SQL += " 	   AND OPDATE >= TRUNC(SYSDATE -365)                                                        \r\n";
            SQL += " 	GROUP BY TO_CHAR(OPDATE,'YYYY-MM-DD')                                                       \r\n";
            SQL += " 	 UNION                                                                                      \r\n";
            SQL += " 	 SELECT                                                                                     \r\n";
            SQL += " 		    TO_CHAR(RDATE,'YYYY-MM-DD') 		AS BDATE                                        \r\n";
            SQL += " 	   FROM KOSMOS_OCS.ENDO_JUPMST                                                              \r\n";
            SQL += " 	  WHERE 1 = 1                                                                               \r\n";
            SQL += " 	   AND Ptno     =   " + ComFunc.covSqlstr(strPANO, false);
            SQL += " 	   AND GbSunap IN ('1','2','7')                                                             \r\n";
            SQL += " 	   AND RDATE >= TRUNC(SYSDATE -365)                                                         \r\n";
            SQL += " 	 GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD')                                                       \r\n";
            SQL += " 	 UNION                                                                                      \r\n";
            SQL += " 	 SELECT                                                                                     \r\n";
            SQL += " 		    TO_CHAR(RDATE,'YYYY-MM-DD') 		AS BDATE                                        \r\n";
            SQL += " 	   FROM KOSMOS_OCS.ETC_JUPMST                                                               \r\n";
            SQL += " 	  WHERE 1 = 1                                                                               \r\n";
            SQL += " 	   AND Ptno     =   " + ComFunc.covSqlstr(strPANO, false);
            SQL += " 	   AND GbJob IN ('1','2','3')                                                               \r\n";
            SQL += " 	   AND Gubun NOT IN ('17','18','19','20')                                                   \r\n";
            SQL += " 	   AND RDATE >= TRUNC(SYSDATE -365)                                                         \r\n";
            SQL += " 	 GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD')                                                       \r\n";
            SQL += " 	 UNION                                                                                      \r\n";
            SQL += " 	 SELECT                                                                                     \r\n";
            SQL += " 	  	    TO_CHAR(SEEKDATE,'YYYY-MM-DD') 		AS BDATE                                        \r\n";
            SQL += " 	   FROM KOSMOS_PMPA.XRAY_DETAIL                                                             \r\n";
            SQL += " 	  WHERE 1 = 1                                                                               \r\n";
            SQL += " 	   AND PANO     =   " + ComFunc.covSqlstr(strPANO, false);
            SQL += " 	   AND GBRESERVED IN ('1','2','6','7')                                                      \r\n";
            SQL += " 	   AND TRIM(XCODE) NOT IN (                                                                 \r\n";
            SQL += " 	                      SELECT TRIM(CODE)                                                     \r\n";
            SQL += " 	                        FROM KOSMOS_PMPA.BAS_BCODE                                          \r\n";
            SQL += " 	                         WHERE 1=1                                                          \r\n";
            SQL += " 	                          AND GUBUN ='C#_영상검사_기능검사제외코드'                         \r\n";
            SQL += " 	                          AND (DELDATE IS NULL OR DELDATE ='')                              \r\n";
            SQL += " 	                     )                                                                      \r\n";
            SQL += " 	   AND ( SEEKDATE >= TRUNC(SYSDATE -365)                                                    \r\n";
            SQL += " 	     OR RDATE >= TRUNC(SYSDATE -365 )   )                                                   \r\n";
            SQL += " 	 GROUP BY TO_CHAR(SEEKDATE,'YYYY-MM-DD')                                                    \r\n";
            SQL += " 	 UNION                                                                                      \r\n";
            SQL += " 	 SELECT                                                                                     \r\n";
            SQL += " 	    CASE WHEN (A.RDATE IS NULL OR A.RDATE = '')  THEN TO_CHAR(A.BDATE,'YYYY-MM-DD')         \r\n";
            SQL += " 		    		 ELSE TO_CHAR(A.RDATE,'YYYY-MM-DD') END			AS BDDATE                   \r\n";
            SQL += " 	  FROM KOSMOS_OCS.EXAM_ORDER   A                                                            \r\n";
            SQL += " 	  WHERE 1=1                                                                                 \r\n";
            SQL += " 	    AND A.PANO = " + ComFunc.covSqlstr(strPANO, false);
            SQL += " 	    AND (                                                                                   \r\n";
            SQL += " 		    	CASE WHEN (A.RDATE IS NULL OR A.RDATE = '')  THEN A.BDATE                       \r\n";
            SQL += " 		    		 ELSE A.RDATE END                                                           \r\n";
            SQL += " 	    	)	>= TRUNC(SYSDATE-365)                                                           \r\n";
            SQL += " 	GROUP BY CASE WHEN (A.RDATE IS NULL OR A.RDATE = '')  THEN TO_CHAR(A.BDATE,'YYYY-MM-DD')    \r\n";
            SQL += " 		    		 ELSE TO_CHAR(A.RDATE,'YYYY-MM-DD') END                                     \r\n";
            SQL += " 	    ORDER BY BDATE                                                                          \r\n";
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

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    s.Add(dt.Rows[i]["BDATE"].ToString());
                }
            }
            else
            {
                s = null;
            }
            return s;
        }

        public DataTable sel_PATIENT_SCHDUL(PsmhDb pDbCon, string strPANO)
        {
            DataTable dt = null;

            List<string> sArrBDATE = sel_PATIENT_SCHDUL_TIME(pDbCon, strPANO);

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strLIMIT_DAY = "365";

            if (sArrBDATE != null && sArrBDATE.Count > 0)
            {
                SQL = "";
                SQL += "  WITH T AS (                                                                                       \r\n";
                SQL += "  	SELECT                                                                                          \r\n";
                SQL += "  	       '1'									AS GUBUN                                            \r\n";
                SQL += "  	     , MAX(TO_CHAR(ACTDATE,'YYYY-MM-DD')) 	AS BDATE                                            \r\n";
                SQL += "  	     , COUNT(DEPTCODE)      				AS CNT                                              \r\n";
                SQL += "  	  FROM KOSMOS_PMPA.OPD_MASTER                                                                   \r\n";
                SQL += "  	 WHERE 1=1                                                                                      \r\n";
                SQL += "  	   AND PANO  = " + ComFunc.covSqlstr(strPANO, false);
                SQL += "  	   AND ACTDATE > SYSDATE-"+ strLIMIT_DAY + "                                                    \r\n";
                SQL += "  	GROUP BY ACTDATE                                                                                \r\n";
                SQL += "  	UNION ALL                                                                                       \r\n";
                SQL += "  	 SELECT                                                                                         \r\n";
                SQL += "  	  		'2' 								AS GUBUN                                            \r\n";
                SQL += "  	      ,  TO_CHAR(DATE3,'YYYY-MM-DD' ) 		AS BDATE                                            \r\n";
                SQL += "  	      , COUNT(DEPTCODE)						AS CNT                                              \r\n";
                SQL += "  	   FROM KOSMOS_PMPA.OPD_RESERVED_NEW                                                            \r\n";
                SQL += "  	  WHERE 1 = 1                                                                                   \r\n";
                SQL += "  	   AND Pano = " + ComFunc.covSqlstr(strPANO, false);
                SQL += "  	   AND (RETDATE IS NULL OR RETDATE  ='')                                                        \r\n";
                SQL += "  	   AND (TRANSDATE IS NULL OR TRANSDATE='' OR TRUNC(TRANSDATE) > TRUNC(SYSDATE) )                \r\n";
                SQL += "  	GROUP BY TO_CHAR(DATE3,'YYYY-MM-DD' )                                                           \r\n";
                SQL += "  	UNION ALL                                                                                       \r\n";
                SQL += "  	 SELECT                                                                                         \r\n";
                SQL += "  		    '3' 								AS GUBUN                                            \r\n";
                SQL += "  	  		, TO_CHAR(OPDATE,'YYYY-MM-DD')   	AS BDATE                                            \r\n";
                SQL += "  	  		, COUNT(DEPTCODE)					AS CNT                                              \r\n";
                SQL += "  	   FROM KOSMOS_PMPA.ORAN_MASTER                                                                 \r\n";
                SQL += "  	  WHERE 1 = 1                                                                                   \r\n";
                SQL += "  	   AND Pano = " + ComFunc.covSqlstr(strPANO, false);
                SQL += "  	   AND OpCancel IS NULL                                                                         \r\n";
                SQL += "  	   AND OPDATE >= TRUNC(SYSDATE-" + strLIMIT_DAY + ")                                            \r\n";
                SQL += "  	GROUP BY TO_CHAR(OPDATE,'YYYY-MM-DD')                                                           \r\n";
                SQL += "  	 UNION ALL                                                                                      \r\n";
                SQL += "  	 SELECT                                                                                         \r\n";
                SQL += "  	  	   '4' 									AS GUBUN                                            \r\n";
                SQL += "  		  , TO_CHAR(RDATE,'YYYY-MM-DD') 		AS BDATE                                            \r\n";
                SQL += "  		  , COUNT(ORDERNO)					    AS CNT                                              \r\n";
                SQL += "  	   FROM KOSMOS_OCS.ENDO_JUPMST                                                                  \r\n";
                SQL += "  	  WHERE 1 = 1                                                                                   \r\n";
                SQL += "  	   AND Ptno = " + ComFunc.covSqlstr(strPANO, false);
                SQL += "  	   AND GbSunap IN ('1','2','7')                                                                 \r\n";
                SQL += "  	   AND RDATE >= TRUNC(SYSDATE-" + strLIMIT_DAY + ")                                             \r\n";
                SQL += "  	 GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD')                                                           \r\n";
                SQL += "                                                                                                    \r\n";
                SQL += "  	 UNION ALL                                                                                      \r\n";
                SQL += "  	 SELECT                                                                                         \r\n";
                SQL += "  	  		'5' 								AS GUBUN                                            \r\n";
                SQL += "  		  , TO_CHAR(RDATE,'YYYY-MM-DD') 		AS BDATE                                            \r\n";
                SQL += "  		  , COUNT(ORDERNO)						AS CNT                                              \r\n";
                SQL += "  	   FROM KOSMOS_OCS.ETC_JUPMST                                                                   \r\n";
                SQL += "  	  WHERE 1 = 1                                                                                   \r\n";
                SQL += "  	   AND Ptno = " + ComFunc.covSqlstr(strPANO, false);
                SQL += "  	   AND GbJob IN ('1','2','3')                                                                   \r\n";
                SQL += "  	   AND Gubun NOT IN ('17','18','19','20')                                                       \r\n";
                SQL += "  	   AND RDATE >= TRUNC(SYSDATE-" + strLIMIT_DAY + ")                                             \r\n";
                SQL += "  	 GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD')                                                           \r\n";
                SQL += "                                                                                                    \r\n";
                SQL += "  	 UNION ALL                                                                                      \r\n";
                SQL += "  	 SELECT                                                                                         \r\n";
                SQL += "  	  		'6' 								AS GUBUN                                            \r\n";
                SQL += "  	  	  , TO_CHAR(SEEKDATE,'YYYY-MM-DD') 		AS BDATE                                            \r\n";
                SQL += "  	  	  , COUNT(ORDERNO)                                                                          \r\n";
                SQL += "  	   FROM KOSMOS_PMPA.XRAY_DETAIL                                                                 \r\n";
                SQL += "  	  WHERE 1 = 1                                                                                   \r\n";
                SQL += "  	   AND PANO = " + ComFunc.covSqlstr(strPANO, false);
                SQL += "  	   AND GBRESERVED IN ('1','2','6','7')                                                          \r\n";
                SQL += "  	   AND TRIM(XCODE) NOT IN (                                                                     \r\n";
                SQL += "  	                      SELECT TRIM(CODE)                                                         \r\n";
                SQL += "  	                        FROM KOSMOS_PMPA.BAS_BCODE                                              \r\n";
                SQL += "  	                         WHERE 1=1                                                              \r\n";
                SQL += "  	                          AND GUBUN ='C#_영상검사_기능검사제외코드'                             \r\n";
                SQL += "  	                          AND (DELDATE IS NULL OR DELDATE ='')                                  \r\n";
                SQL += "  	                     )                                                                          \r\n";
                SQL += "  	   AND ( SEEKDATE >= TRUNC(SYSDATE-" + strLIMIT_DAY + ")                                        \r\n";
                SQL += "  	     OR RDATE >= TRUNC(SYSDATE-" + strLIMIT_DAY + " )   )                                       \r\n";
                SQL += "  	 GROUP BY TO_CHAR(SEEKDATE,'YYYY-MM-DD')                                                        \r\n";
                SQL += "  	 UNION ALL                                                                                      \r\n";
                SQL += "  	 SELECT                                                                                         \r\n";
                SQL += "  	   '7' 										AS GUBUN                                            \r\n";
                SQL += "  	  , CASE WHEN (A.RDATE IS NULL OR A.RDATE = '')  THEN TO_CHAR(A.BDATE,'YYYY-MM-DD')             \r\n";
                SQL += "  		    		 ELSE TO_CHAR(A.RDATE,'YYYY-MM-DD') END			AS BDDATE                       \r\n";
                SQL += "  	  , COUNT(ORDERNO)							AS CNT                                              \r\n";
                SQL += "  	  FROM KOSMOS_OCS.EXAM_ORDER   A                                                                \r\n";
                SQL += "  	  WHERE 1=1                                                                                     \r\n";
                SQL += "  	    AND A.PANO = " + ComFunc.covSqlstr(strPANO, false);
                SQL += "  	    AND (                                                                                       \r\n";
                SQL += "  		    	CASE WHEN (A.RDATE IS NULL OR A.RDATE = '')  THEN A.BDATE                           \r\n";
                SQL += "  		    		 ELSE A.RDATE END                                                               \r\n";
                SQL += "  	    	)	>= TRUNC(SYSDATE-" + strLIMIT_DAY + ")                                              \r\n";
                SQL += "  	GROUP BY CASE WHEN (A.RDATE IS NULL OR A.RDATE = '')  THEN TO_CHAR(A.BDATE,'YYYY-MM-DD')        \r\n";
                SQL += "  		    		 ELSE TO_CHAR(A.RDATE,'YYYY-MM-DD') END                                         \r\n";
                SQL += "  	    ORDER BY BDATE, GUBUN                                                                       \r\n";
                SQL += "  )                                                                                                 \r\n";
                SQL += "  SELECT                                                                                            \r\n";
                SQL += "            ' 구분'		AS GUBUN1                                                                   \r\n";
                SQL += "         ,  ' 구분2'	AS GUBUN2                                                                   \r\n";

                for (int i = 0; i < sArrBDATE.Count; i++)
                {
                    SQL += "        , '" + sArrBDATE[i] + "' AS DAY" + i.ToString() + "                                     \r\n";
                }

                SQL += "   FROM DUAL                                                                                        \r\n";
                SQL += " UNION ALL                                                                                          \r\n";
                SQL += "  SELECT                                                                                            \r\n";
                SQL += "  	   CASE WHEN B.GUBUN2 = '1' THEN '외래접수'                                                     \r\n";
                SQL += "  			WHEN B.GUBUN2 = '2' THEN '외래예약'                                                     \r\n";
                SQL += "  			WHEN B.GUBUN2 = '3' THEN '수술'                                                         \r\n";
                SQL += "  			WHEN B.GUBUN2 = '4' THEN '내시경'                                                       \r\n";
                SQL += "  			WHEN B.GUBUN2 = '5' THEN '기능검사'                                                     \r\n";
                SQL += "  			WHEN B.GUBUN2 = '6' THEN '영상의학'                                                     \r\n";
                SQL += "  			WHEN B.GUBUN2 = '7' THEN '진단검사'                                                     \r\n";
                SQL += "  	   END GUBUN_KOR                                                                                \r\n";
                SQL += "       , A.*                                                                                        \r\n";
                SQL += "    FROM (                                                                                          \r\n";
                SQL += "    			SELECT GUBUN                                                                        \r\n";

                SQL += "    			    ,   CASE WHEN BDATE = '" + sArrBDATE[0] + "' THEN '" + sArrBDATE[0] + "'        \r\n";

                for (int i = 1; i < sArrBDATE.Count; i++)
                {
                    SQL += "    			             WHEN BDATE = '" + sArrBDATE[i] + "' THEN '" + sArrBDATE[i] + "'   \r\n";
                }

                SQL += "  					END BDATE                                                                       \r\n";
                SQL += "                  , TO_CHAR(CNT) AS CNT                                                             \r\n";
                SQL += "                FROM T                                                                              \r\n";
                SQL += "  		)                                                                                           \r\n";
                SQL += "  		PIVOT                                                                                       \r\n";
                SQL += "  		(                                                                                           \r\n";
                SQL += "  		 MAX(CNT) FOR BDATE IN (                                                                    \r\n";


                SQL += "  								  '" + sArrBDATE[0] + "'                                            \r\n";

                for (int i = 1; i < sArrBDATE.Count; i++)
                {
                    SQL += "    			            , '" + sArrBDATE[i] + "'                                            \r\n";
                }

                SQL += "  		)                                                                                           \r\n";
                SQL += "  	) A                                                                                             \r\n";
                SQL += "   , (                                                                                              \r\n";
                SQL += "   		SELECT TO_CHAR(LEVEL) GUBUN2                                                                \r\n";
                SQL += "   		   FROM DUAL                                                                                \r\n";
                SQL += "   		   CONNECT BY LEVEL < 8                                                                     \r\n";
                SQL += "   	) B                                                                                             \r\n";
                SQL += "   WHERE 1=1                                                                                        \r\n";
                SQL += "     AND B.GUBUN2 = A.GUBUN(+)                                                                      \r\n";

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
            }

            return dt;
        }

        public string sel_OPD_MASTER_AMT(PsmhDb pDbCon, string strPANO, string strBDATE, string strDEPTCODE, string strSUNEXT)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string sAMT = "0";

            SQL = "";
            SQL += " WITH T AS(                                                                                                     \r\n";
            SQL += " 			SELECT                                                                                              \r\n";
            SQL += " 			        A.BI                                                                                        \r\n";
            SQL += " 			      , NVL(A.MCODE,'NULL') AS MCODE                                                                \r\n";
            SQL += " 			      , (                                                                                           \r\n";
            SQL += " 					  SELECT BAMT                                                                               \r\n";
            SQL += " 					    FROM KOSMOS_PMPA.VIEW_SUGA_AMT                                                          \r\n";
            SQL += " 					   WHERE SUCODE = " + ComFunc.covSqlstr(strSUNEXT, false);
            SQL += " 					     AND SUDATE = (                                                                         \r\n";
            SQL += " 					     			 	 SELECT MAX(SUDATE)                                                     \r\n";
            SQL += " 					                       FROM KOSMOS_PMPA.VIEW_SUGA_AMT                                       \r\n";
            SQL += " 					  				      WHERE SUCODE = " + ComFunc.covSqlstr(strSUNEXT, false);
            SQL += " 					  				  )                                                                         \r\n";
            SQL += " 					)                  AS BAMT                                                                  \r\n";
            SQL += " 			  FROM KOSMOS_PMPA.OPD_MASTER A                                                                     \r\n";
            SQL += " 			 WHERE 1=1                                                                                          \r\n";
            SQL += " 			   AND PANO 	= " + ComFunc.covSqlstr(strPANO    , false);
            SQL += " 			   AND BDATE    = " + ComFunc.covSqlDate(strBDATE  , false);
            SQL += " 			   AND DEPTCODE = " + ComFunc.covSqlstr(strDEPTCODE, false);
            SQL += "    )                                                                                                           \r\n";
            SQL += "    SELECT                                                                                                      \r\n";
            SQL += "    		(                                                                                                   \r\n";
            //TODO : 2017.07.20.김홍록 : 0.5(병원가산),0.14(차상위2종), 0.15(의료급여 2종)에 대한 항목은 원무팀에서 결정이 나게 되면 DB에서 받던지, C#으로 받던지 할 예정...하드코딩을 없애자
            SQL += "    			CASE WHEN BI IN ('11','12','13') AND MCODE    = 'NULL' 		   THEN TRUNC((BAMT * 0.5 ),-2)     \r\n";
            SQL += " 				     WHEN BI IN ('11','12','13') AND MCODE IN ( 'E000','F000') THEN TRUNC((BAMT * 0.14),-2)     \r\n";
            SQL += " 				     WHEN BI IN ('11','12','13') AND MCODE IN ( 'C000'       ) THEN 0                           \r\n";
            SQL += " 				     WHEN BI IN ('43', '51')                                   THEN BAMT                        \r\n";
            SQL += " 				     WHEN BI IN ('22')										   THEN TRUNC((BAMT * 0.15),-1)     \r\n";
            SQL += " 				 ELSE 0                                                                                         \r\n";
            SQL += " 				 END                                                                                            \r\n";
            SQL += " 		     )																	AS AMT                          \r\n";
            SQL += " 	FROM T                                                                                                      \r\n";
            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
                    return sAMT;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return sAMT;
            }

            if (dt.Rows.Count > 0)
            {
                int nAmt = Convert.ToInt16(dt.Rows[0]["AMT"].ToString());
                sAMT = string.Format("{0}", nAmt.ToString("#,##0"));
            }
            else
            {
                sAMT = "0";
            }
            return sAMT;
        }

        /// <summary>주사실 세부부서</summary>
        public enum enmSENDEPT_SUB_080 {
          /// <summary>일반주사실</summary>
          IJRM,
          /// <summary>영상의학과</summary>
           XRAY };

        /// <summary>주사실 상태</summary>
        public enum enmSENDEPT_STAT_080 {
            /// <summary>미접수</summary>
            UN_RCPN,
            /// <summary>액팅</summary>
            ACT
        };

        /// <summary>내시경검사 상태</summary>
        public enum enmSENDEPT_STAT_060
        {
            /// <summary>취소</summary>
            CNCL,
            /// <summary>접수</summary>
            RCPN,            
            /// <summary>미접수</summary>
            UN_RCPN,
            /// <summary>완료</summary>
            CMPL,
        };

        /// <summary>기능검사 상태</summary>
        public enum enmSENDEPT_STAT_020
        {
            /// <summary>미접수</summary>
            UN_RCPN,
            /// <summary>예약</summary>
            APNT,
            /// <summary>접수,완료</summary>
            CMPL,
            /// <summary>취소</summary>
            CNCL,
        };

	 /// <summary> 혈액 불출/취소시 슬립전송</summary>
     /// <param name="pDbCon"></param>
     /// <param name="isCANCEL"></param>
     /// <param name="strENTSABUN"></param>
     /// <param name="strPANO"></param>
     /// <param name="strBDATE"></param>
     /// <param name="strDEPTCODE"></param>
     /// <param name="strDRCODE"></param>
     /// <param name="strSUCODE"></param>
     /// <param name="strBLOODNO"></param>
     /// <param name="strCOMPONENT"></param>
     /// <param name="strCAPACITY"></param>
     /// <param name="strSPECNO"></param>
     /// <param name="strIPDOPD"></param>
     /// <param name="strORDERNO"></param>
     /// <param name="intRowAffected"></param>
     /// <returns></returns>
        public string ins_WORK_IPDSLIP_SEND(PsmhDb pDbCon, bool isCANCEL, string strENTSABUN, string strPANO, string strBDATE, string strDEPTCODE
                                            , string strDRCODE, string strBLOODNO, string strCOMPONENT, string strCAPACITY
                                            , string strSPECNO, string strIPDOPD, string strORDERNO, ref int intRowAffected)
        {

            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL += "  INSERT INTO KOSMOS_PMPA.WORK_IPDSLIP_SEND                 \r\n";
                SQL += "  	(                                                       \r\n";
                SQL += "  	  ENTTIME                                               \r\n";
                SQL += "  	, ENTSABUN                                              \r\n";
                SQL += "  	, PANO                                                  \r\n";
                SQL += "  	, BDATE                                                 \r\n";
                SQL += "  	, DEPTCODE                                              \r\n";
                SQL += "  	, DRCODE                                                \r\n";
                SQL += "  	, SUCODE                                                \r\n";
                SQL += "  	, QTY                                                   \r\n";
                SQL += "  	, NAL                                                   \r\n";
                SQL += "  	, GBSEND                                                \r\n";
                SQL += "  	, GBACT                                                 \r\n";
                SQL += "  	, BLOODNO                                               \r\n";
                SQL += "  	, COMPONENT                                             \r\n";
                SQL += "  	, CAPACITY                                              \r\n";
                SQL += "  	, SPECNO                                                \r\n";
                SQL += "  	, IPDOPD                                                \r\n";
                SQL += "  	, ORDERNO                                               \r\n";
                SQL += "  	)  VALUES(                                              \r\n";
                SQL += "  	   SYSDATE                                              \r\n";
                SQL += "  	" + ComFunc.covSqlstr(strENTSABUN   , true);
                SQL += "  	" + ComFunc.covSqlstr(strPANO       , true);
                SQL += "  	" + ComFunc.covSqlDate(strBDATE     , true);
                SQL += "  	" + ComFunc.covSqlstr(strDEPTCODE   , true);
                SQL += "  	" + ComFunc.covSqlstr(strDRCODE     , true);
                SQL += "    , (                                                    \r\n";
                SQL += "      SELECT CASE WHEN '320' = '" + strCAPACITY.Trim() + "' THEN SUCODE_320 ELSE SUCODE_400 END SUCODE \r\n";
                SQL += "        FROM KOSMOS_PMPA.BAS_SUGA_BLOOD                                         \r\n";
                SQL += "       WHERE SUDATE = (SELECT MAX(SUDATE)                                       \r\n";
                SQL += "                         FROM KOSMOS_PMPA.BAS_SUGA_BLOOD           \r\n";
                SQL += "                        WHERE SUDATE    < " + ComFunc.covSqlDate(strBDATE, false);
                SQL += "                          AND COMPONENT = " + ComFunc.covSqlstr(strCOMPONENT, false);
                SQL += "                        )                                          \r\n";
                SQL += "         AND COMPONENT = " + ComFunc.covSqlstr(strCOMPONENT, false);
                SQL += "      )                                                    \r\n";


                SQL += "  	,'1'                                                         \r\n";

                if (isCANCEL == false)
                {
                    SQL += "  	,'1'                                                     \r\n";
                }
                else
                {
                    SQL += "  	,'-1'                                                    \r\n";
                }
                SQL += "  	,'*'                                                    \r\n";
                SQL += "  	,'*'                                                    \r\n";
                SQL += "  	" + ComFunc.covSqlstr(strBLOODNO    , true);
                SQL += "  	" + ComFunc.covSqlstr(strCOMPONENT  , true);
                SQL += "  	" + ComFunc.covSqlstr(strCAPACITY   , true);
                SQL += "  	" + ComFunc.covSqlstr(strSPECNO     , true);
                SQL += "  	" + ComFunc.covSqlstr(strIPDOPD     , true);
                SQL += "  	" + ComFunc.covSqlstr(strORDERNO, true);
                SQL += "  	)                                                       \r\n";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return ex.Message.ToString();
            }

            return SqlErr;
        }

        /// <summary>기능검사 상태</summary>
        public enum enmSel_IPD_NEW_MASTER
        {
            /// <summary>미접수</summary>
            PANO,
            /// <summary>예약</summary>
            SNAME,
            /// <summary>0.재원 1.가퇴원 2.퇴원접수 3.대조리스트인쇄 4.심사부분완료 5.심사완료 6.퇴원계산서인쇄 7.퇴원수납완료 9.입원취소 </summary>
            GBSTS,
        };
        

        public enum enmSel_IPD_NEW_MASTER_EMR       {   INOUTCLS,  MEDFRDATE,  MEDFRTIME,  MEDENDDATE, MEDENDTIME, DEPTKORNAME,  DRUSENAME,       PTNO,     PTNAME,    SEX,    AGE,  MEDDEPTCD, MEDDRCD };
        public string[] sSel_IPD_NEW_MASTER_EMR =   { "환자구분", "내원일자", "내원시간",  "퇴원일자", "퇴원시간",    "진료과",   "진료의", "등록번호", "환자성명", "성별", "나이", "MEDDEPTCD", "MEDDRCD" };
        public int[] nSel_IPD_NEW_MASTER_EMR    =   {  nCol_CHEK,  nCol_DATE,          5,   nCol_DATE,          5,   nCol_DATE, nCol_SNAME,          5,          5,      5,      5,           5,       5   };

        public enum enmSel_WORK_IPDSLIP_SEND { SPECNO, ORDERNO, IPDOPD, SUCODE };

        public DataTable sel_WORK_IPDSLIP_SEND(PsmhDb pDbCon, string strBLOODNO, string strCOMPONENT)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                 \r\n";
            SQL += "        SPECNO                          \r\n";
            SQL += "      , ORDERNO                         \r\n";
            SQL += "      , IPDOPD                          \r\n";
            SQL += "      , SUCODE                          \r\n";
            SQL += "  FROM KOSMOS_PMPA.WORK_IPDSLIP_SEND    \r\n";
            SQL += " WHERE 1 = 1                            \r\n";
            SQL += "   AND BLOODNO      = " + ComFunc.covSqlstr(strBLOODNO, false);
            SQL += "   AND COMPONENT    = " + ComFunc.covSqlstr(strCOMPONENT, false);
            SQL += "   AND NAL          > 0 ";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

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

        public DataSet sel_IPD_NEW_MASTER_EMR(PsmhDb pDbCon, string strPANO)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                                                                                                         \r\n";
            SQL += " 	   A.INOUTCLS                                                                                                               \r\n";
            SQL += "     , TO_CHAR(TO_DATE(A.MEDFRDATE  ,'YYYY-MM-DD')  ,'YYYY-MM-DD') AS MEDFRDATE                                                 \r\n";
            SQL += "     , TO_CHAR(TO_DATE(A.MEDFRTIME  , 'HH24:MI:SS') ,'HH24:MI:SS') AS MEDFRTIME                                                 \r\n";
            SQL += "     , TO_CHAR(TO_DATE(A.MEDENDDATE ,'YYYY-MM-DD')  ,'YYYY-MM-DD') AS MEDENDDATE                                                \r\n";
            SQL += "     , TO_CHAR(TO_DATE(A.MEDENDTIME , 'HH24:MI:SS') ,'HH24:MI:SS') AS MEDENDTIME                                                \r\n";
            SQL += "     , KOSMOS_OCS.FC_BAS_CLINICDEPT_DEPTNAMEK(A.MEDDEPTCD) 		   AS DEPTKORNAME                                               \r\n";
            SQL += "     , A.DRNAME 												   AS DRUSENAME                                                 \r\n";
            SQL += "     , A.PTNO                                                                                                                   \r\n";
            SQL += " 	 , A.PTNAME                                                                                                                 \r\n";
            SQL += " 	 , A.SEX                                                                                                                    \r\n";
            SQL += " 	 , A.AGE                                                                                                                    \r\n";
            SQL += "     , A.MEDDEPTCD                                                                                                              \r\n";
            SQL += "     , A.MEDDRCD                                                                                                                \r\n";
            SQL += " FROM (                                                                                                                         \r\n";
            SQL += " 		 SELECT                                                                                                                 \r\n";
            SQL += " 		         'O'  							AS INOUTCLS                                                                     \r\n";
            SQL += " 		    , A.PANO  							AS PTNO                                                                         \r\n";
            SQL += " 		    , A.SNAME 							AS PTNAME                                                                       \r\n";
            SQL += " 		    , A.SEX                                                                                                             \r\n";
            SQL += " 		    , A.AGE                                                                                                             \r\n";
            SQL += " 		    , A.DEPTCODE 						AS MEDDEPTCD                                                                    \r\n";
            SQL += " 		    , A.DRCODE   						AS MEDDRCD                                                                      \r\n";
            SQL += " 		    , TO_CHAR(A.BDATE,'YYYYMMDD') 		AS MEDFRDATE                                                                    \r\n";
            SQL += " 		    , TO_CHAR(A.JTIME,'HH24MI') || '00' AS MEDFRTIME                                                                    \r\n";
            SQL += " 		    , TO_CHAR(A.BDATE,'YYYYMMDD') 		AS MEDENDDATE                                                                   \r\n";
            SQL += " 		    , TO_CHAR(A.JTIME,'HH24MI') || '00' AS MEDENDTIME                                                                   \r\n";
            SQL += " 		    , D.DRNAME                                                                                                          \r\n";
            SQL += " 		FROM KOSMOS_PMPA.OPD_MASTER 		A                                                                                   \r\n";
            SQL += " 		   , KOSMOS_OCS.OCS_DOCTOR 			D                                                                                   \r\n";
            SQL += " 		WHERE A.PANO 		= " + ComFunc.covSqlstr(strPANO, false);
            SQL += " 		  AND A.JIN    		IN ('0','1','2','3','4','5','6','7','9','E','F','H','M','L','K','N','I','J','Q','R','S','A','B')    \r\n";
            SQL += " 		  AND A.OCSJIN 		!= ' '                                                                                              \r\n";
            SQL += " 		  AND A.DRCODE 		 = D.DRCODE                                                                                         \r\n";
            SQL += " 		  AND GBOUT 		<> 'Y'                                                                                              \r\n";
            SQL += " 		UNION ALL                                                                                                               \r\n";
            SQL += " 		 SELECT                                                                                                                 \r\n";
            SQL += " 		 	  'I' 							AS INOUTCLS                                                                         \r\n";
            SQL += " 		 	, A.PANO 						AS PTNO                                                                             \r\n";
            SQL += " 		 	, A.SNAME 						AS PTNAME                                                                           \r\n";
            SQL += " 		 	, A.SEX                                                                                                             \r\n";
            SQL += " 		 	, A.AGE                                                                                                             \r\n";
            SQL += " 		    , A.DEPTCODE 					AS MEDDEPTCD                                                                        \r\n";
            SQL += " 		    , A.DRCODE 						AS MEDDRCD                                                                          \r\n";
            SQL += " 		    , TO_CHAR(A.INDATE,'YYYYMMDD') 	AS MEDFRDATE                                                                        \r\n";
            SQL += " 		    , '120000' 						AS MEDFRTIME                                                                        \r\n";
            SQL += " 		    , TO_CHAR(A.OUTDATE,'YYYYMMDD') AS MEDENDDATE                                                                       \r\n";
            SQL += " 		    , '120000' 						AS MEDENDTIME                                                                       \r\n";
            SQL += " 		    , D.DRNAME                                                                                                          \r\n";
            SQL += " 		FROM KOSMOS_PMPA.IPD_NEW_MASTER A                                                                                       \r\n";
            SQL += " 		   , KOSMOS_OCS.OCS_DOCTOR 		D                                                                                       \r\n";
            SQL += " 		WHERE A.PANO        = " + ComFunc.covSqlstr(strPANO, false);
            SQL += " 		   AND A.DRCODE     = D.DRCODE                                                                                          \r\n";
            SQL += " 		   AND GBOUT <> 'Y'                                                                                                     \r\n";
            SQL += " 	) A                                                                                                                         \r\n";
            SQL += "  ORDER BY A.INOUTCLS ASC, A.MEDFRDATE DESC, A.MEDDEPTCD                                                                        \r\n";

            try
            {
                SqlErr = clsDB.GetDataSetEx(ref ds, SQL, pDbCon);

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

        public DataTable sel_IPD_NEW_MASTER(PsmhDb pDbCon, string strPANO, string strDATE)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += "      SELECT                                            \r\n";
            SQL += "       PANO             -- 병록번호,환자 ID             \r\n";
            SQL += "     , SNAME            -- 수진자명                     \r\n";
            SQL += "     , GBSTS            -- 상태                         \r\n";
            SQL += "  FROM KOSMOS_PMPA.IPD_NEW_MASTER /** 환자 Master*/     \r\n";
            SQL += " WHERE 1=1                                              \r\n";
            SQL += "   AND PANO = " + ComFunc.covSqlstr(strPANO,false);
            SQL += "   AND ACTDATE IS NULL                                  \r\n";
            SQL += "   AND TRUNC(INDATE) <= " + ComFunc.covSqlDate(strDATE, false);
            SQL += " ORDER BY INDATE DESC                                   \r\n";

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

        /// <summary>환자정보조회</summary>
        public int READ_BLOOD_AMT(PsmhDb pDbCon, string strCODE, string strDATE)
        {

            int nAMT = 0;

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            //TODO : 2017.12.04.김홍록: 이 루튼은 하드코딩이 들어 갈수 밖에 없음. 혈액원에서 공시하는 혈액 금액이 (BT24)는 현재 구조상 하드 코딩 뿐임. (정보팀 김민절J 심사과 팀장과 통화함)
            if (strCODE.Equals("BT24") == true)
            {
                //2017.12.04. 현재 금액임. 매년 하드 코딩 하세요.
                return 301395;
            }
            else
            {
                SQL = "";
                SQL += "  WITH T AS (                                                                                                                                               \r\n";
                SQL += "  	SELECT JDATE1 AS JDATE_FROM, TO_DATE('9998-12-31','YYYY-MM-DD') JDATE_TO ,  CODE   AS CODE, PRICE1 AS PRICE  FROM KOSMOS_PMPA.EDI_SUGA WHERE   CODE = " + ComFunc.covSqlstr(strCODE, false);
                SQL += "  	UNION ALL                                                                                                                                                               \r\n";
                SQL += "  	SELECT JDATE2 AS JDATE_FROM, JDATE1 -1 JDATE_TO, CODE   AS CODE, PRICE2 AS PRICE  FROM KOSMOS_PMPA.EDI_SUGA WHERE   CODE = " + ComFunc.covSqlstr(strCODE, false);
                SQL += "  	UNION ALL                                                                                                                                                               \r\n";
                SQL += "  	SELECT JDATE3 AS JDATE_FROM, JDATE2 -1 JDATE_TO, CODE   AS CODE, PRICE3 AS PRICE  FROM KOSMOS_PMPA.EDI_SUGA WHERE   CODE = " + ComFunc.covSqlstr(strCODE, false);
                SQL += "  	UNION ALL                                                                                                                                                               \r\n";
                SQL += "  	SELECT JDATE4 AS JDATE_FROM, JDATE3 -1 JDATE_TO, CODE   AS CODE, PRICE4 AS PRICE  FROM KOSMOS_PMPA.EDI_SUGA WHERE   CODE = " + ComFunc.covSqlstr(strCODE, false);
                SQL += "  	UNION ALL                                                                                                                                                               \r\n";
                SQL += "  	SELECT JDATE5 AS JDATE_FROM, JDATE4 -1 JDATE_TO, CODE   AS CODE, PRICE5 AS PRICE  FROM KOSMOS_PMPA.EDI_SUGA WHERE   CODE = " + ComFunc.covSqlstr(strCODE, false);
                SQL += "  )                                                                                                                                                                         \r\n";
                SQL += "  SELECT                                                                            \r\n";
                SQL += "   		PRICE                                                                       \r\n";
                SQL += "  	FROM T                                                                          \r\n";
                SQL += "     WHERE TO_DATE('" + strDATE + "','YYYY-MM-DD') BETWEEN JDATE_FROM               \r\n";
                SQL += "                                                       AND JDATE_TO                 \r\n";

                try
                {
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return 0;
                    }

                    nAMT = Convert.ToInt32(dt.Rows[0][0].ToString());



                }
                catch (System.Exception ex)
                {
                    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                    return 0;
                }
            }

            return nAMT;

        }

        public enum enmSel_OPD_MASTER_PTINFO      {         GBIO,       PANO,      SNAME,      SEX,      AGE,            BI, WARDCODE, ROOMCODE,   DEPTCODE,       DRNM,    ACTDATE};
        public string[] sSel_OPD_MASTER_PTINFO  = {   "환자구분", "등록번호", "환자성명",   "성별",   "나이",    "보험구분",   "병동",   "병실",       "과",   "의사명", "처방일자"};
        public int[] nSel_OPD_MASTER_PTINFO     = { nCol_AGE+10 ,  nCol_PANO, nCol_SNAME, nCol_AGE, nCol_AGE,   nCol_AGE+10, nCol_AGE, nCol_AGE, nCol_SNAME, nCol_SNAME, nCol_DATE };

        public enum enmPT_GBIO { I, O, MASTER };

        public enum enmSEND_STAT { RCPN, UN_RCPN, CMPL, CNCL, ACT, APNT };

        public enum enmSel_BAS_PATIENT_MENUAL { PANO, SNME, JUMIN, AGE, SEX, JUSO, LASTDATE, DEPTCODE };
        public string[] sSsel_BAS_PATIENT_MENUAL = { "등록번호", "환자명", "생년월일", "나이", "성별", "주소", "최종내원일", "진료과" };

        public int[] nSsel_BAS_PATIENT_MENUAL = { nCol_PANO, nCol_SNAME, nCol_TIME, nCol_AGE, nCol_SCHK, nCol_JUSO, nCol_DATE, nCol_DPCD };

        public enum enmSel_OCS_ORDERCODE_MENUAL           {        CHK,   GBINFO,           ORDERNAME,    SLIP_NM,    ORDERCODE,          ITEMCD,         EXAMNAME,    GBINPUT,    SLIPNO  ,  GBDOSAGE  };
        public string[] sSel_OCS_ORDERCODE_MENUAL       = {     "선택", "GBINFO",            "처방명",   "슬립명",  "ORDERCODE",  "진단검사코드", "진단검사코드명",  "GBINPUT",  "SLIPNO"  , "GBDOSAGE" };
        public int[] nSel_OCS_ORDERCODE_MENUAL          = {  nCol_SCHK, nCol_SCHK, nCol_ORDERNAME + 20,  nCol_TIME,    nCol_PANO,      nCol_PANO,   nCol_ORDERNAME,  nCol_SCHK, nCol_SCHK ,  5         };
        public int[] nSel_OCS_ORDERCODE_MENUAL_SEARCH   = {          5,         5,      nCol_TIME + 50,  nCol_PANO + 40,     5,      nCol_PANO, nCol_ORDERNAME, nCol_SCHK, nCol_SCHK, 5 };

        public enum enmSel_BAS_PCCONFIG { WS, CODE, EQU, NAME, EQU_NAME };

        public string ins_BAS_PCCONFIG(PsmhDb pDbCon, string strCODE, string strVALUEV, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            try
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string myip = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString();

                SQL = "";
                SQL += " INSERT INTO KOSMOS_PMPA.BAS_PCCONFIG         \r\n";
                SQL += " (                                            \r\n";
                SQL += "       IPADDRESS    --                        \r\n";
                SQL += "     , GUBUN        --코드구분                \r\n";
                SQL += "     , CODE         --코드                    \r\n";
                SQL += "     , VALUEV       --문자값                  \r\n";
                SQL += "     , DISSEQNO     --정렬순서                \r\n";
                SQL += "     , INPDATE      --입력일자                \r\n";
                SQL += "     , INPTIME      --입력시간                \r\n";
                SQL += "     , DELGB        --삭제여부 0:사용 1:삭제  \r\n";
                SQL += " ) VALUES (                                   \r\n";
                SQL += "    " + ComFunc.covSqlstr(myip, false);
                SQL += "    , '프로그램위치'                          \r\n";
                SQL += "    " + ComFunc.covSqlstr(strCODE    , true);
                SQL += "    " + ComFunc.covSqlstr(strVALUEV  , true);
                SQL += "    , 1                                       \r\n";
                SQL += "    , TO_CHAR(SYSDATE,'YYYYMMDD')             \r\n";
                SQL += "    , TO_CHAR(SYSDATE,'HH24MISS')             \r\n";
                SQL += "    , 0                                       \r\n";
                SQL += " )                                            \r\n";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return SqlErr;

        }

        public string del_BAS_PCCONFIG(PsmhDb pDbCon, string strCODE, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            try
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string myip = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString();


                SQL = "";
                SQL += " DELETE KOSMOS_PMPA.BAS_PCCONFIG                        \r\n";
                SQL += "  WHERE IPADDRESS = " + ComFunc.covSqlstr(myip, false);
                SQL += "    AND GUBUN   = '프로그램위치'                        \r\n";
                SQL += "    AND CODE    = " + ComFunc.covSqlstr(strCODE, false);

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return ex.ToString();
            }

            return SqlErr;
        }

        public string sel_BAS_PCCONFIG(PsmhDb pDbCon, string strCODE)
        {
            string s = "";

            try
            {
                DataTable dt = new DataTable();

                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string myip = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString();

                SQL = "";
                SQL += "  SELECT VALUEV                     \r\n";
                SQL += "    FROM KOSMOS_PMPA.BAS_PCCONFIG   \r\n";
                SQL += "   WHERE 1=1                        \r\n";
                SQL += "     AND IPADDRESS  = " + ComFunc.covSqlstr(myip, false);
                SQL += "     AND GUBUN 	    = '프로그램위치'    \r\n";
                SQL += "     AND CODE       = " + ComFunc.covSqlstr(strCODE, false);

                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (ComFunc.isDataTableNull(dt) == true)
                {
                    return "";
                }
                else
                {
                    return dt.Rows[0][0].ToString();
                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return "";
            }
            return s;
        }

        public string sel_BAS_PCCONFIG(PsmhDb pDbCon, enmSel_BAS_PCCONFIG pType, bool isQuerry)
        {
            string s = null;
            string[] strArr;


            try
            {

                DataTable dt = new DataTable();

                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string myip = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString();

                SQL = "";
                SQL += "  SELECT VALUEV                     \r\n";
                SQL += "    FROM KOSMOS_PMPA.BAS_PCCONFIG   \r\n";
                SQL += "   WHERE 1=1                        \r\n";
                SQL += "     AND IPADDRESS = " + ComFunc.covSqlstr(myip, false);
                SQL += "     AND GUBUN 	= '프로그램위치'    \r\n";
                if (pType == enmSel_BAS_PCCONFIG.CODE)
                {
                    SQL += "     AND CODE 	= 'EXAM_CODE'       \r\n";
                }
                else if (pType == enmSel_BAS_PCCONFIG.EQU)
                {
                    SQL += "     AND CODE 	= 'EXAM_EQU'       \r\n";
                }
                else if (pType == enmSel_BAS_PCCONFIG.EQU_NAME)
                {
                    SQL += "     AND CODE 	= 'EXAM_EQU_NAME'     \r\n";
                }
                else if (pType == enmSel_BAS_PCCONFIG.NAME)
                {
                    SQL += "     AND CODE 	= 'EXAM_NAME'     \r\n";
                }
                else if (pType == enmSel_BAS_PCCONFIG.WS)
                {
                    SQL += "     AND CODE 	= 'EXAM_WS'       \r\n";
                }

                string SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                if (ComFunc.isDataTableNull(dt) == true || string.IsNullOrEmpty(dt.Rows[0][0].ToString().Trim()) == true)
                {
                    return "";
                }
                else
                {
                    if (isQuerry == true)
                    {
                        strArr = dt.Rows[0][0].ToString().Split(',');

                        for (int i = 0; i < strArr.Length; i++)
                        {
                            s += "'" + strArr[i] + "',";
                        }

                        s = s.Substring(0, s.Length - 1);
                    }
                    else
                    {
                        return dt.Rows[0][0].ToString();
                    }

                }

            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;

            }


            return s;

        }

        public string del_BAS_PCCONFIG_EQU(PsmhDb pDbCon, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            try
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string myip = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString();


                SQL = "";
                SQL += " DELETE KOSMOS_PMPA.BAS_PCCONFIG                                           \r\n";
                SQL += "  WHERE IPADDRESS = " + ComFunc.covSqlstr(myip, false);
                SQL += "    AND GUBUN = '프로그램위치'                                             \r\n";
                SQL += "    AND CODE IN ('EXAM_CODE','EXAM_EQU', 'EXAM_WS','EXAM_NAME','EXAM_EQU_NAME')            \r\n";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return ex.ToString();
            }

            return SqlErr;
        }

        public string ins_BAS_PCCONFIG_WS(PsmhDb pDbCon, string strALLEQU, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            try
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string myip = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString();


                SQL = "";
                SQL += " INSERT INTO KOSMOS_PMPA.BAS_PCCONFIG                                           \r\n";
                SQL += " (                                                                              \r\n";
                SQL += "       IPADDRESS    --              \r\n ";
                SQL += "     , GUBUN        --코드구분      \r\n ";
                SQL += "     , CODE         --코드          \r\n ";
                SQL += "     , VALUEV       --문자값        \r\n ";
                SQL += "     , DISSEQNO     --정렬순서  \r\n ";
                SQL += "     , INPDATE      --입력일자  \r\n ";
                SQL += "     , INPTIME      --입력시간  \r\n ";
                SQL += "     , DELGB        --삭제여부 0:사용 1:삭제  \r\n ";
                SQL += " ) VALUES (                                                                      \r\n";
                SQL += "   " + ComFunc.covSqlstr(myip, false);
                SQL += "   ,'프로그램위치'                                                      \r\n";
                SQL += "   , 'EXAM_WS'                                                          \r\n";
                SQL += "   " + ComFunc.covSqlstr(strALLEQU, true);
                SQL += "   ,1                                                                   \r\n";
                SQL += "   ,TO_CHAR(SYSDATE,'YYYYMMDD')                                         \r\n";
                SQL += "   ,TO_CHAR(SYSDATE,'HH24MISS')                                         \r\n";
                SQL += "   ,0                                                                   \r\n";
                SQL += " )                                                                      \r\n";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }


            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return ex.ToString();
            }

            return SqlErr;

        }

        public string ins_BAS_PCCONFIG_EQU(PsmhDb pDbCon, string strALLEQU, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            try
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string myip = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString();


                SQL = "";
                SQL += " INSERT INTO KOSMOS_PMPA.BAS_PCCONFIG                                           \r\n";
                SQL += " (                                                                              \r\n";
                SQL += "       IPADDRESS    --              \r\n ";
                SQL += "     , GUBUN        --코드구분      \r\n ";
                SQL += "     , CODE         --코드          \r\n ";
                SQL += "     , VALUEV       --문자값        \r\n ";
                SQL += "     , DISSEQNO     --정렬순서  \r\n ";
                SQL += "     , INPDATE      --입력일자  \r\n ";
                SQL += "     , INPTIME      --입력시간  \r\n ";
                SQL += "     , DELGB        --삭제여부 0:사용 1:삭제  \r\n ";
                SQL += " ) VALUES (                                                                      \r\n";
                SQL += "   " + ComFunc.covSqlstr(myip, false);
                SQL += "   ,'프로그램위치'                                                      \r\n";
                SQL += "   , 'EXAM_EQU'                                                          \r\n";
                SQL += "   " + ComFunc.covSqlstr(strALLEQU, true);
                SQL += "   ,1                                                                   \r\n";
                SQL += "   ,TO_CHAR(SYSDATE,'YYYYMMDD')                                         \r\n";
                SQL += "   ,TO_CHAR(SYSDATE,'HH24MISS')                                         \r\n";
                SQL += "   ,0                                                                   \r\n";
                SQL += " )                                                                      \r\n";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }


            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return SqlErr;

        }

        public string ins_BAS_PCCONFIG_CODE(PsmhDb pDbCon, string strALLCODE, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            try
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string myip = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString();


                SQL = "";
                SQL += " INSERT INTO KOSMOS_PMPA.BAS_PCCONFIG                                           \r\n";
                SQL += " (                                                                              \r\n";
                SQL += "       IPADDRESS    --              \r\n ";
                SQL += "     , GUBUN        --코드구분      \r\n ";
                SQL += "     , CODE         --코드          \r\n ";
                SQL += "     , VALUEV       --문자값        \r\n ";
                SQL += "     , DISSEQNO     --정렬순서  \r\n ";
                SQL += "     , INPDATE      --입력일자  \r\n ";
                SQL += "     , INPTIME      --입력시간  \r\n ";
                SQL += "     , DELGB        --삭제여부 0:사용 1:삭제  \r\n ";
                SQL += " ) VALUES (                                                                      \r\n";
                SQL += "   " + ComFunc.covSqlstr(myip, false);
                SQL += "   ,'프로그램위치'                                                     \r\n";
                SQL += "   , 'EXAM_CODE'                                                        \r\n";
                SQL += "   " + ComFunc.covSqlstr(strALLCODE, true);
                SQL += "   ,1                                                                   \r\n";
                SQL += "   ,TO_CHAR(SYSDATE,'YYYYMMDD')                                         \r\n";
                SQL += "   ,TO_CHAR(SYSDATE,'HH24MISS')                                         \r\n";
                SQL += "   ,0                                                                   \r\n";
                SQL += " )                                                                      \r\n";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }


            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return ex.ToString();
            }

            return SqlErr;

        }

        public string ins_BAS_PCCONFIG_NAME(PsmhDb pDbCon, string strALLNAME, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            try
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string myip = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString();

                SQL = "";
                SQL += " INSERT INTO KOSMOS_PMPA.BAS_PCCONFIG                                           \r\n";
                SQL += " (                                                                              \r\n";
                SQL += "       IPADDRESS    --              \r\n ";
                SQL += "     , GUBUN        --코드구분      \r\n ";
                SQL += "     , CODE         --코드          \r\n ";
                SQL += "     , VALUEV       --문자값        \r\n ";
                SQL += "     , DISSEQNO     --정렬순서  \r\n ";
                SQL += "     , INPDATE      --입력일자  \r\n ";
                SQL += "     , INPTIME      --입력시간  \r\n ";
                SQL += "     , DELGB        --삭제여부 0:사용 1:삭제  \r\n ";
                SQL += " ) VALUES (                                                                      \r\n";
                SQL += "   " + ComFunc.covSqlstr(myip, false);
                SQL += "   ,'프로그램위치'                                                     \r\n";
                SQL += "   , 'EXAM_NAME'                                                        \r\n";
                SQL += "   " + ComFunc.covSqlstr(strALLNAME, true);
                SQL += "   ,1                                                                   \r\n";
                SQL += "   ,TO_CHAR(SYSDATE,'YYYYMMDD')                                         \r\n";
                SQL += "   ,TO_CHAR(SYSDATE,'HH24MISS')                                         \r\n";
                SQL += "   ,0                                                                   \r\n";
                SQL += " )                                                                      \r\n";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }


            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            return SqlErr;

        }

        public string ins_BAS_PCCONFIG_EQU_NAME(PsmhDb pDbCon, string strALLEQUNAME, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            try
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                string myip = System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList[0].ToString();

                SQL = "";
                SQL += " INSERT INTO KOSMOS_PMPA.BAS_PCCONFIG                                           \r\n";
                SQL += " (                                                                              \r\n";
                SQL += "       IPADDRESS    --              \r\n ";
                SQL += "     , GUBUN        --코드구분      \r\n ";
                SQL += "     , CODE         --코드          \r\n ";
                SQL += "     , VALUEV       --문자값        \r\n ";
                SQL += "     , DISSEQNO     --정렬순서  \r\n ";
                SQL += "     , INPDATE      --입력일자  \r\n ";
                SQL += "     , INPTIME      --입력시간  \r\n ";
                SQL += "     , DELGB        --삭제여부 0:사용 1:삭제  \r\n ";
                SQL += " ) VALUES (                                                                      \r\n";
                SQL += "   " + ComFunc.covSqlstr(myip, false);
                SQL += "   ,'프로그램위치'                                                     \r\n";
                SQL += "   , 'EXAM_EQU_NAME'                                                        \r\n";
                SQL += "   " + ComFunc.covSqlstr(strALLEQUNAME, true);
                SQL += "   ,1                                                                   \r\n";
                SQL += "   ,TO_CHAR(SYSDATE,'YYYYMMDD')                                         \r\n";
                SQL += "   ,TO_CHAR(SYSDATE,'HH24MISS')                                         \r\n";
                SQL += "   ,0                                                                   \r\n";
                SQL += " )                                                                      \r\n";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return SqlErr;
                }


            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return ex.ToString();
            }

            return SqlErr;

        }

        public DataTable sel_BAS_DOCTOR_COMBO(PsmhDb pDbCon, string strDEPTCODE)
        {
            DataTable dt = null;

            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT DRCODE || '.' || DRNAME CODE_NAME                      \r\n";
            SQL += "   FROM KOSMOS_PMPA.BAS_DOCTOR                                 \r\n";
            SQL += "  WHERE 1=1                                                    \r\n";
            SQL += "    AND (DRDEPT1 = '" + strDEPTCODE.Trim() + "' OR DRDEPT2 = '" + strDEPTCODE.Trim() + "')                           \r\n";
            SQL += "    AND (TOUR IS NULL OR TOUR = 'N')                           \r\n";
            SQL += "   ORDER BY PRINTRANKING                                       \r\n";

            try
            {
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

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

        public DataSet sel_OCS_ORDERCODE_MENUAL(PsmhDb pDbCon, string strORDERCODE)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += "  SELECT                                                                                            \r\n";
            SQL += "  	   ''									                AS CHK                                  \r\n";
            SQL += "       , (                                                                                          \r\n";
            SQL += "  	     CASE WHEN A.GBINFO = '1' THEN '▲'                                                         \r\n";
            SQL += "  	          ELSE ''                                                                               \r\n";
            SQL += "  	      END                                                                                       \r\n";
            SQL += "         )								                    AS GBINFO                               \r\n";
            SQL += "  	  ,  KOSMOS_OCS.FC_SPACE(A.DISPSPACE) || A.ORDERNAME    AS ORDERNAME                            \r\n";
            SQL += "  	  , (                                                                                           \r\n";
            SQL += "  		 SELECT B.ORDERNAME                                                                         \r\n";
            SQL += "  		   FROM KOSMOS_OCS.OCS_ORDERCODE B                                                          \r\n";
            SQL += "  		  WHERE SEQNO = 0                                                                           \r\n";
            SQL += "  		    AND SLIPNO = A.SLIPNO                                                                   \r\n";
            SQL += "  	   ) 									                AS SLIP_NM                              \r\n";
            SQL += "       , A.ORDERCODE                                                                                \r\n";
            SQL += "       , A.ITEMCD                                                                                   \r\n";
            SQL += "       , B.EXAMNAME                                                                                 \r\n";
            SQL += "       , A.GBINPUT                                                                                  \r\n";
            SQL += "       , A.SLIPNO                                                                                   \r\n";
            SQL += "       , A.GBDOSAGE                                                                                 \r\n";
            SQL += "    FROM KOSMOS_OCS.OCS_ORDERCODE A                                                                 \r\n";
            SQL += "       , KOSMOS_OCS.EXAM_MASTER   B                                                                 \r\n";
            SQL += "   WHERE 1=1                                                                                        \r\n";
            SQL += "     AND TRIM(A.ITEMCD) = B.MASTERCODE(+)                                                           \r\n";
            SQL += "     AND A.SLIPNO IN (                                                                              \r\n";
            SQL += "     		 '0010'                                                                                 \r\n";
            SQL += "     		,'0014'                                                                                 \r\n";
            SQL += "     		,'0016'                                                                                 \r\n";
            SQL += "     		,'0018'                                                                                 \r\n";
            SQL += "     		,'0022'                                                                                 \r\n";
            SQL += "     		,'0024'                                                                                 \r\n";
            SQL += "     		,'0026'                                                                                 \r\n";
            SQL += "     		,'0028'                                                                                 \r\n";
            SQL += "     		,'0030'                                                                                 \r\n";
            SQL += "     		,'0032'                                                                                 \r\n";
            SQL += "     		,'0010'                                                                                 \r\n";
            SQL += "     		,'0014'                                                                                 \r\n";
            SQL += "     		,'0016'                                                                                 \r\n";
            SQL += "     		,'0018'                                                                                 \r\n";
            SQL += "     		,'0022'                                                                                 \r\n";
            SQL += "     		,'0024'                                                                                 \r\n";
            SQL += "     		,'0026'                                                                                 \r\n";
            SQL += "     		,'0028'                                                                                 \r\n";
            SQL += "     		,'0030'                                                                                 \r\n";
            SQL += "     		,'0032'                                                                                 \r\n";
            SQL += "     		,'0034'                                                                                 \r\n";
            SQL += "     		,'0040'                                                                                 \r\n";
            SQL += "     		,'0042'                                                                                 \r\n";
            SQL += "     		,'0050'                                                                                 \r\n";
            SQL += "      )                                                                                             \r\n";
            SQL += "     AND SEQNO     >  0                                                                             \r\n";
            SQL += "     AND SENDDEPT != 'N'                                                                            \r\n";

            if (string.IsNullOrEmpty(strORDERCODE.Trim()) == false)
            {
                SQL += "     AND ( UPPER(A.ORDERNAME) LIKE '%" + strORDERCODE.Trim().ToUpper() + "%' OR UPPER(A.ORDERCODE) LIKE '%" + strORDERCODE.Trim().ToUpper() + "%'  OR UPPER(TRIM(A.ITEMCD)) LIKE '%" + strORDERCODE.Trim().ToUpper() + "%' OR UPPER(TRIM(B.EXAMNAME)) LIKE '%" + strORDERCODE.Trim().ToUpper() + "%') \r\n";
            }            

            SQL += "   ORDER BY SLIPNO, SEQNO, ORDERCODE                                                                \r\n";                                                                                                                        

            try
            {
                SqlErr = clsDB.GetDataSetEx(ref ds, SQL, pDbCon);

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

        public DataSet sel_OPD_MASTER_MENUAL(PsmhDb pDbCon, enmPT_GBIO enmType, string strActDateFR, string strActDateTO, string strPANO)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += "  SELECT                                                    \r\n";
            SQL += "  		  'O' AS GBIO                                       \r\n";
            SQL += "  		, PANO                                              \r\n";
            SQL += "  		, SNAME                                             \r\n";
            SQL += "  		, SEX                                               \r\n";
            SQL += "  		, TO_CHAR(AGE) AGE                                  \r\n";
            SQL += "  		, BI ||'.' || KOSMOS_OCS.FC_BAS_BCODE_NAME('BAS_환자종류',BI) AS BI                                                \r\n";
            SQL += "  		, '' WARDCODE                                       \r\n";
            SQL += "  		, '' ROOMCODE                                       \r\n";
            SQL += "  		, DEPTCODE ||'.'|| KOSMOS_OCS.FC_BAS_CLINICDEPT_DEPTNAMEK(DEPTCODE) AS DEPTCODE \r\n";
            SQL += "  		, DRCODE   ||'.'|| KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DRCODE) DRNM                                            \r\n";
            SQL += "  		, TO_CHAR(ACTDATE, 'YYYY-MM-DD') ACTDATE            \r\n";
            SQL += "    FROM KOSMOS_PMPA.OPD_MASTER                             \r\n";
            SQL += "   WHERE 1=1                                                \r\n";
            SQL += "     AND 'O' = " + ComFunc.covSqlstr(enmType.ToString(), false);

            if (enmType == enmPT_GBIO.O)
            {
                SQL += "     AND ACTDATE BETWEEN " + ComFunc.covSqlDate(strActDateFR, false);
                SQL += "                     AND " + ComFunc.covSqlDate(strActDateTO, false);

            }

            if (string.IsNullOrEmpty(strPANO.Trim()) == false)
            {
                SQL += "     AND PANO = " + ComFunc.covSqlstr(strPANO, false);
            }

            SQL += "  UNION ALL                                                 \r\n";
            SQL += "  SELECT                                                    \r\n";
            SQL += "  	    'I' AS GBIO                                         \r\n";
            SQL += "  	  , PANO                                                \r\n";
            SQL += "  	  , SNAME                                               \r\n";
            SQL += "  	  , SEX                                                 \r\n";
            SQL += "  	  , TO_CHAR(AGE) AGE                                    \r\n";
            SQL += "  		, BI ||'.' || KOSMOS_OCS.FC_BAS_BCODE_NAME('BAS_환자종류',BI) AS BI                                                \r\n";
            SQL += "   	  , TO_CHAR(WARDCODE) WARDCODE                          \r\n";
            SQL += "  	  , TO_CHAR(ROOMCODE) ROOMCODE                          \r\n";
            SQL += "  		, DEPTCODE ||'.'|| KOSMOS_OCS.FC_BAS_CLINICDEPT_DEPTNAMEK(DEPTCODE) AS DEPTCODE             \r\n";
            SQL += "  		, DRCODE   ||'.'|| KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DRCODE)          AS DRNM                 \r\n";
            SQL += "      , '" + strActDateFR + "' AS ACTDATE                     \r\n";
            SQL += "    FROM KOSMOS_PMPA.IPD_NEW_MASTER                         \r\n";
            SQL += "   WHERE 1=1                                                \r\n";
            SQL += "     AND 'I' = " + ComFunc.covSqlstr(enmType.ToString(), false);
            SQL += "     AND AMSET1 = '0'                                       \r\n";
            SQL += "     AND GBSTS IN ('0', '2', '3', '4')                      \r\n";

            if (string.IsNullOrEmpty(strPANO.Trim()) == false)
            {
                SQL += "     AND PANO = " + ComFunc.covSqlstr(strPANO, false);
            }

            SQL += "  UNION ALL                                                 \r\n";
            SQL += "  SELECT                                                                                            \r\n";
            SQL += "         '' AS GBIO                                                                                 \r\n";
            SQL += "  	  , PANO                                                                                        \r\n";
            SQL += "  	  , SNAME                                                                                       \r\n";
            SQL += "  	  , SEX                                                                                         \r\n";
            SQL += "      , KOSMOS_OCS.FC_GET_AGE(KOSMOS_OCS.FC_BAS_PATIENT_BIRTH(PANO),'"+ strActDateFR  + "') AS AGE    \r\n";
            SQL += "  	  , '' BI                                                                                       \r\n";
            SQL += "   	  , '' WARDCODE                                                                                 \r\n";
            SQL += "  	  , '' ROOMCODE                                                                                 \r\n";
            SQL += "  	  , '' DEPTCODE                                                                                 \r\n";
            SQL += "  	  , '' DRNM                                                                                     \r\n";
            SQL += "      , '" + strActDateFR + "' AS ACTDATE                                                           \r\n";
            SQL += "   FROM KOSMOS_PMPA.BAS_PATIENT                                                                     \r\n";
            SQL += "  WHERE 1=1                                                                                         \r\n";
            SQL += "    AND 'MASTER' = " + ComFunc.covSqlstr(enmType.ToString(), false);
            SQL += "    AND PANO     = " + ComFunc.covSqlstr(strPANO, false);
            SQL += "   ORDER BY SNAME,PANO                                      \r\n";

            try
            {
                SqlErr = clsDB.GetDataSetEx(ref ds, SQL, pDbCon);

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

        string setSQL_OCS_ORDER(enmGBIO pGBIO, string strSENDDEPT, string strSENDEPTSUB, string strSTAT, string strPTNO, string strBDATE, string strORDERNO, string strROWID)
        {
            string SQL = "";

            if (pGBIO == enmGBIO.O)
            {
                SQL = SQL + "   UPDATE KOSMOS_OCS.OCS_OORDER                                                            \r\n";
            }
            else if (pGBIO == enmGBIO.I)
            {
                SQL = SQL + "   UPDATE KOSMOS_OCS.OCS_IORDER                                                            \r\n";
            }

            SQL = SQL + "    SET SENDDEPT           = " + ComFunc.covSqlstr(strSENDDEPT , false);
            SQL = SQL + "      , SENDDEPT_SUB       = " + ComFunc.covSqlstr(strSENDEPTSUB , false);
            SQL = SQL + "      , SENDDEPT_STAT   = " + ComFunc.covSqlstr(strSTAT       , false);
            SQL = SQL + "      , SENDDEPT_UPPS      = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
            SQL = SQL + "      , SENDDEPT_UPDT      = SYSDATE                                                           \r\n";
            SQL = SQL + " WHERE 1=1                                                                                     \r\n";
            SQL = SQL + "   AND BDATE               = " + ComFunc.covSqlDate(strBDATE, false);

            if (string.IsNullOrEmpty(strROWID) == false)
            {
                SQL = SQL + "      AND ROWID        = " + ComFunc.covSqlstr(strROWID, false);
            }
            else
            {
                SQL = SQL + "      AND PTNO         = " + ComFunc.covSqlstr(strPTNO, false);
                SQL = SQL + "      AND ORDERNO      = " + ComFunc.covSqlstr(strORDERNO, false);
            }

            return SQL;
        }

        /// <summary>기능검사 상태 업데이트</summary>
        /// <param name="pGBIO">입원외래구분</param>
        /// <param name="pSENDEPTSUB">세부부서</param>
        /// <param name="pSTAT">상태</param>
        /// <param name="pPTNO">등록번호</param>
        /// <param name="pBDATE">발생일자</param>
        /// <param name="pORDERNO">처방일련번호</param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <param name="pROWID"></param>
        /// <returns></returns>
        public string up_OCS_ORDER_020(PsmhDb pDbCon, enmGBIO pGBIO, string pSENDEPTSUB, enmSENDEPT_STAT_020 pSTAT, string pPTNO, string pBDATE, string pORDERNO, ref int intRowAffected, string pROWID = "")
        {
            string SqlErr = ""; //에러문 받는 변수

            string strSENDDEPT = "";
            string strSENDEPTSUB = "";
            string strSTAT = "";

            if (string.IsNullOrEmpty(pORDERNO) == false && string.IsNullOrEmpty(pPTNO) == false && string.IsNullOrEmpty(pBDATE) == false)
            {
                strSENDDEPT = "020";

                strSENDEPTSUB = pSENDEPTSUB;

                if (pSTAT == enmSENDEPT_STAT_020.UN_RCPN)
                {
                    strSTAT = "1";
                }
                else if (pSTAT == enmSENDEPT_STAT_020.APNT)
                {
                    strSTAT = "2";
                }
                else if (pSTAT == enmSENDEPT_STAT_020.CMPL)
                {
                    strSTAT = "3";
                }
                else if (pSTAT == enmSENDEPT_STAT_020.CNCL)
                {
                    strSTAT = "9";
                }


                SQL = setSQL_OCS_ORDER(pGBIO, strSENDDEPT, strSENDEPTSUB, strSTAT, pPTNO, pBDATE, pORDERNO, pROWID);

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            }
            else
            {
                SqlErr = "처방번호와 환자 번호가 반드시 있어야 합니다.";
            }

            return SqlErr;
        }

        /// <summary>내시경검사 상태 업데이트 </summary>
        /// <param name="pGBIO">입원외래구분</param>
        /// <param name="pSENDEPTSUB">세부부서</param>
        /// <param name="pSTAT">상태</param>
        /// <param name="pPTNO">등록번호</param>
        /// <param name="pBDATE">발생일자</param>
        /// <param name="pORDERNO">처방일련번호</param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <param name="pROWID"></param>
        /// <returns></returns>
        public string up_OCS_ORDER_060(PsmhDb pDbCon, enmGBIO pGBIO, string pSENDEPTSUB, enmSENDEPT_STAT_060 pSTAT, string pPTNO, string pBDATE, string pORDERNO, ref int intRowAffected, string pROWID = "")
        {
            string SqlErr = ""; //에러문 받는 변수

            string strSENDDEPT = "";
            string strSENDEPTSUB = "";
            string strSTAT = "";

            if (string.IsNullOrEmpty(pORDERNO) == false && string.IsNullOrEmpty(pPTNO) == false && string.IsNullOrEmpty(pBDATE) == false)
            {
                strSENDDEPT = "060";

                strSENDEPTSUB = pSENDEPTSUB;

                if (pSTAT == enmSENDEPT_STAT_060.CNCL)
                {
                    strSTAT = "*";
                }                
                else if (pSTAT == enmSENDEPT_STAT_060.RCPN)
                {
                    strSTAT = "1";
                }
                else if (pSTAT == enmSENDEPT_STAT_060.UN_RCPN)
                {
                    strSTAT = "2";
                }
                else if (pSTAT == enmSENDEPT_STAT_060.CMPL)
                {
                    strSTAT = "7";
                }

                SQL = setSQL_OCS_ORDER(pGBIO, strSENDDEPT, strSENDEPTSUB, strSTAT, pPTNO, pBDATE, pORDERNO, pROWID);

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 300);

            }
            else
            {
                SqlErr = "처방번호와 환자 번호가 반드시 있어야 합니다.";
            }

            return SqlErr;
        }

        /// <summary>주사실처방 상태 업데이트</summary>
        /// <param name="pGBIO"></param>
        /// <param name="pSENDEPTSUB"></param>
        /// <param name="pSTAT"></param>
        /// <param name="pPTNO"></param>
        /// <param name="pBDATE"></param>
        /// <param name="pORDERNO"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <param name="pROWID"></param>
        /// <returns></returns>
        public string up_OCS_ORDER_080(PsmhDb pDbCon, enmGBIO pGBIO, enmSENDEPT_SUB_080 pSENDEPTSUB, enmSENDEPT_STAT_080 pSTAT, string pPTNO, string pBDATE, string pORDERNO, ref int intRowAffected, string pROWID = "")
        {
            string SqlErr = ""; //에러문 받는 변수

            string strSENDDEPT = "";
            string strSENDEPTSUB = "";
            string strSTAT = "";

            if (string.IsNullOrEmpty(pORDERNO) == false && string.IsNullOrEmpty(pPTNO) == false && string.IsNullOrEmpty(pBDATE) == false)
            {
                strSENDDEPT = "080";

                if (pSENDEPTSUB == enmSENDEPT_SUB_080.IJRM)
                {
                    strSENDEPTSUB = "1";
                }
                else if (pSENDEPTSUB == enmSENDEPT_SUB_080.XRAY)
                {
                    strSENDEPTSUB = "3";
                }

                if (pSTAT == enmSENDEPT_STAT_080.UN_RCPN)
                {
                    strSTAT = "0";
                }
                else if (pSTAT == enmSENDEPT_STAT_080.ACT)
                {
                    strSTAT = "3";
                }


                SQL = setSQL_OCS_ORDER(pGBIO, strSENDDEPT, strSENDEPTSUB, strSTAT, pPTNO, pBDATE, pORDERNO, pROWID);

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            }
            else
            {
                SqlErr = "처방번호와 환자 번호가 반드시 있어야 합니다.";
            }

            return SqlErr;
        }

        public string up_OCS_OORDER(PsmhDb pDbCon, enmGBIO pGBIO, enmSENDDEPT pSendDept,  string strSENDDEPT_SUB, string strSENDDEPT_STAT, string strPTNO, string strORDERNO, string strROWID, ref int intRowAffected)
        {

            string SqlErr = ""; //에러문 받는 변수

            if (string.IsNullOrEmpty(strORDERNO) == false && string.IsNullOrEmpty(strPTNO) == false)
            {
               
                SQL = "";

                if (pGBIO == enmGBIO.O)
                {
                    SQL = SQL + "   UPDATE KOSMOS_OCS.OCS_OORDER                                                            \r\n";
                }
                else if (pGBIO == enmGBIO.I)
                {
                    SQL = SQL + "   UPDATE KOSMOS_OCS.OCS_IORDER                                                            \r\n";
                }

                Enum pEnum = pSendDept;

                SQL = SQL + "    SET SENDDEPT           = " + ComFunc.covSqlstr(pEnum.ToString(), false);
                SQL = SQL + "      , SENDDEPT_SUB       = " + ComFunc.covSqlstr(strSENDDEPT_SUB , false);
                SQL = SQL + "      , strSENDDEPT_STAT   = " + ComFunc.covSqlstr(strSENDDEPT_STAT, false);
                SQL = SQL + "      , SENDDEPT_INPS      = DECODE (NVL(SENDDEPT_INPS   ,'Q'),'Q', '" + clsType.User.IdNumber + "', SENDDEPT_INPS)";
                SQL = SQL + "      , SENDDEPT_INPT_DT   = DECODE (NVL(SENDDEPT_INPT_DT,'Q'),'Q', SYSDATE, SENDDEPT_INPT_DT)";
                SQL = SQL + "      , SENDDEPT_UPPS      = " + ComFunc.covSqlstr(clsType.User.IdNumber, false);
                SQL = SQL + "      , SENDDEPT_UPDT      = SYSDATE                                                           \r\n";
                SQL = SQL + " WHERE 1=1                                                                                     \r\n";

                if (string.IsNullOrEmpty(strROWID) == false)
                {
                    SQL = SQL + "      AND ROWID        = " + ComFunc.covSqlstr(strROWID, false);
                }
                else
                {
                    SQL = SQL + "      AND PTNO         = " + ComFunc.covSqlstr(strPTNO, false);
                    SQL = SQL + "      AND ORDERNO      = " + ComFunc.covSqlstr(strORDERNO, false);
                }

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            }
            else
            {
                SqlErr = "처방번호와 환자 번호가 반드시 있어야 합니다.";
            }

            return SqlErr;
        }

        /// <summary>수가조회</summary>
        /// <returns></returns>
        public DataSet sel_BAS_SUN(PsmhDb pDbCon, frmComSupHELP01.enmSel_BAS_SUN_TYPE enmType, string strCode = "", string strName = "")
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += " SELECT                                             \r\n";
            //2019-09-17 안정수, 양선문과장 요청으로 BCODE(표준코드) 추가
            SQL += " 	    B.BCODE    AS BCODE                         \r\n";
            SQL += " 	  , A.SUNEXT   AS SUNEXT                        \r\n";
            SQL += " 	  , B.SUNAMEE  AS SUNAMEE                       \r\n";
            SQL += " 	  , B.SUNAMEK  AS SUNAMEK                       \r\n";
            SQL += " 	  , A.BUN      AS BUN                           \r\n";
            SQL += " 	  , C.NAME     AS BUN_NAME                      \r\n";
            SQL += "   FROM KOSMOS_PMPA.BAS_SUT   A                     \r\n";
            SQL += "   	  , KOSMOS_PMPA.BAS_SUN   B                     \r\n";
            SQL += "   	  , KOSMOS_PMPA.BAS_BCODE C                     \r\n";
            SQL += "  WHERE 1=1                                         \r\n";
            SQL += "    AND A.SUNEXT    = B.SUNEXT(+)                   \r\n";
            SQL += "    AND A.DELDATE   IS NULL                         \r\n";
            SQL += "    AND C.GUBUN     = 'BAS_수가분류코드'            \r\n";
            SQL += "    AND A.BUN       = C.CODE                        \r\n";
            SQL += "    AND A.SUDATE    =                               \r\n";
            SQL += "				   (                                \r\n";
            SQL += "	   				  SELECT MAX(SUDATE)            \r\n";
            SQL += "	                    FROM KOSMOS_PMPA.BAS_SUT E  \r\n";
            SQL += "	                   WHERE E.SUCODE = A.SUCODE    \r\n";
            SQL += "                   )                                \r\n";
            SQL += "                                                    \r\n";
            if (string.IsNullOrEmpty(strCode) == false)
            {
                SQL += "    AND A.SUNEXT    LIKE '%" + strCode + "%'              \r\n";
            }

            if (string.IsNullOrEmpty(strName) == false)
            {
                SQL += "    AND B.SUNAMEK    LIKE '%" + strName + "%'             \r\n";
            }

            if (enmType == frmComSupHELP01.enmSel_BAS_SUN_TYPE.PT)
            {
                SQL += "    AND B.WONCODE    IN ('1108' , '1428')   \r\n";
                SQL += "    AND A.SUNEXT NOT IN ('Y24'  , 'Y25')    \r\n";
            }
            else if (enmType == frmComSupHELP01.enmSel_BAS_SUN_TYPE.EXAM)
            {
                SQL += "    AND A.BUN IN ('37','41','51','52','53','54','55','56','57','58','59','60','61','62','63','64')  \r\n";
            }

            SQL += "  ORDER BY A.SUNEXT                         \r\n";

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
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }
            return ds;
        }

        public DataTable sel_BAS_PATIENT(PsmhDb pDbCon, string strPANO)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += "      SELECT                                        \r\n";
            SQL += "       PANO             -- 병록번호,환자 ID          \r\n ";
            SQL += "     , SNAME            -- 수진자명                  \r\n ";
            SQL += "     , SEX              -- 성별                      \r\n ";
            SQL += "     , JUMIN1           -- 주민번호1                 \r\n ";
            SQL += "     , JUMIN2           -- 주민번호2                 \r\n ";
            SQL += "     , STARTDATE        -- 최초내원일자              \r\n ";
            SQL += "     , LASTDATE         -- 최종내원일자              \r\n ";
            SQL += "     , ZIPCODE1         -- 우편번호1                 \r\n ";
            SQL += "     , ZIPCODE2         -- 우편번호2                 \r\n ";
            SQL += "     , JUSO             -- 주소                      \r\n ";
            SQL += "     , JICODE           -- 지역코드                  \r\n ";
            SQL += "     , TEL              -- 전화번호                  \r\n ";
            SQL += "     , SABUN            -- 직원사번 or 계약처코드    \r\n ";
            SQL += "     , EMBPRT           -- 엠보싱 출력여부           \r\n ";
            SQL += "     , BI               -- 환자구분                  \r\n ";
            SQL += "     , PNAME            -- 피보성명                  \r\n ";
            SQL += "     , GWANGE           -- 관계                      \r\n ";
            SQL += "     , KIHO             -- 기관기호                  \r\n ";
            SQL += "     , GKIHO            -- 개인기호(증번호)          \r\n ";
            SQL += "     , DEPTCODE         -- 최종진료과목              \r\n ";
            SQL += "     , DRCODE           -- 최종진료의사              \r\n ";
            SQL += "     , GBSPC            -- 특진여부                  \r\n ";
            SQL += "     , GBGAMEK          -- 감액구분                  \r\n ";
            SQL += "     , JINILSU          -- 180일 관련 진료일수       \r\n ";
            SQL += "     , JINAMT           -- 180일 관련 청구금액       \r\n ";
            SQL += "     , TUYAKGWA         -- 180일 관련 최종투약과     \r\n ";
            SQL += "     , TUYAKMONTH       -- 180일 관련 최종투약월     \r\n ";
            SQL += "     , TUYAKJULDATE     -- 180일 관련 투약줄리앙일자 \r\n ";
            SQL += "     , TUYAKILSU        -- 180일 관련 투약일수       \r\n ";
            SQL += "     , BOHUN            -- 보훈환자여부              \r\n ";
            SQL += "     , REMARK           -- 비고                      \r\n ";
            SQL += "     , RELIGION         -- 종교                      \r\n ";
            SQL += "     , GBMSG            -- 메세지 Flag               \r\n ";
            SQL += "     , XRAYBARCODE      -- XRay 봉투(1.일반 2.Sono 3.CT 4.MRI 5.RI 6.기타)    \r\n";
            SQL += "     , ARSCHK           -- 전화예약후 불성실 여부(Y:불성실)                   \r\n";
            SQL += "     , BUNUP            -- 의약분업 예외환자 구분(아래참조)                   \r\n" ;
            SQL += "     , BIRTH            -- 생년월일                                           \r\n";
            SQL += "     , GBBIRTH          -- 양(+)/음(-)                                        \r\n" ;
            SQL += "     , EMAIL            -- E-MAIL                                             \r\n";
            SQL += "     , GBINFOR          -- 환자 메세지 FLAG NEW                               \r\n";
            SQL += "     , JIKUP            -- 직업코드(아래참조)                                 \r\n";
            SQL += "     , HPHONE           -- 휴대폰번호                                         \r\n";
            SQL += "     , GBJUGER          -- 주거구분(1.자택 2.전세 3.월세 4.기타)              \r\n";
            SQL += "     , GBSMS            -- 정보동의여부(2012) SMS동의:Y  SMS동의안함:X   SMS요청하기:N  \r\n ";
            SQL += "     , GBJUSO           -- 주소확인 확인완료:Y                                \r\n";
            SQL += "     , BICHK            -- 자격확일자(0901) 저장됨.                           \r\n";
            SQL += "     , HPHONE2          -- 핸드폰2                                            \r\n";   
            SQL += "     , JUSAMSG          -- 주사투약시 참조사항                                \r\n";
            SQL += "     , EKGMSG           -- EKG 참자사항                                       \r\n";
            SQL += "     , BIDATE           -- 자격 취득일자                                      \r\n";
            SQL += "     , MISSINGCALL      -- 전화잘모등록                                       \r\n";
            SQL += "     , AIFLU            -- AI 플루 동의서(S:동의서대상, P:동의서 스캔완료)    \r\n ";
            SQL += "     , TEL_CONFIRM      -- 핸드폰번호가 잘못되어 있는 경우 N 로 표시됨        \r\n ";
            SQL += "     , GBSMS_DRUG       -- 약 안부문자여부(*: 약 안부동의)                    \r\n ";
            SQL += "     , GBINFO_DETAIL    -- 환자Flag-등록일+등록자                             \r\n ";
            SQL += "     , GBINFOR2         -- 환자 메시지 - 문제환자용(재원자메시지)             \r\n ";
            SQL += "     , ROAD             -- 도로주소구분 Y 20120704                            \r\n ";
            SQL += "     , ROADDONG         -- 도로주소 도로명칭 20120704  \r\n ";
            SQL += "     , JUMIN3           -- 주민2(암호화)  \r\n ";
            SQL += "     , GBFOREIGNER      -- 외국인여부(Y:외국인)  \r\n ";
            SQL += "     , ENAME            -- 외국인 여권 영문이름  \r\n ";
            SQL += "     , CASHYN           -- 현금영수증 동의거부(Y:거부, N or Null )  \r\n ";
            SQL += "     , GB_VIP           -- VIP고객구분 - BAS_VIP_구분코드 참조  \r\n ";
            SQL += "     , GB_VIP_REMARK    -- VIP고객 참고사항  \r\n ";
            SQL += "     , GB_VIP_SABUN     -- VIP등록사번  \r\n ";
            SQL += "     , GB_VIP_DATE      -- VIP수정일  \r\n ";
            SQL += "     , ROADDETAIL       -- 도로명 상세주소  \r\n ";
            SQL += "     , GB_VIP2          -- 이전 vip  \r\n ";
            SQL += "     , GB_VIP2_REAMRK   -- 이전 vip 참고  \r\n ";
            SQL += "     , GB_SVIP          -- 감사고객구분 Y  \r\n ";
            SQL += "     , WEBSEND          -- WEBSEND  \r\n ";
            SQL += "     , WEBSENDDATE      -- WEBSENDDATE  \r\n ";
            SQL += "     , GBMERS           -- 메르스 접촉환자  \r\n ";
            SQL += "     , OBST             -- 장애구분(0 or Null: 없음, 1: 시각장애, 2:청각장애, 3.언어장애)  \r\n ";
            SQL += "     , ZIPCODE3         -- 신규우편번호 5자리  \r\n ";
            SQL += "     , BUILDNO          -- 도로명주소 건물관리번호  \r\n ";
            SQL += "     , PT_REMARK        -- 재활치료 메모  \r\n ";
            SQL += "     , TEMPLE           -- 천주교 신자 본당  \r\n ";
            SQL += "     , C_NAME           -- 천주교 세례명  \r\n ";
            SQL += "     , GBCOUNTRY        -- 국적코드 (BAS_외국인환자국적,KR:대한민국)  \r\n ";
            SQL += "  FROM KOSMOS_PMPA.BAS_PATIENT /** 환자 Master*/ \r\n";
            SQL += " WHERE 1=1 \r\n";
            SQL += "   AND PANO = " + ComFunc.covSqlstr(strPANO,false);




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
      
        /// <summary>환자정보조회</summary>
        public DataTable sel_BAS_PATIENT(PsmhDb pDbCon, string strWhere, enmSearchType enmType, bool isUK)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";

            if (enmType == enmSearchType.PTINFO)
            {
                SQL += " SELECT                                                                 \r\n";
                SQL += "        PANO                                                            \r\n";
                SQL += "      , SNAME                                                           \r\n";
                SQL += "      , JUMIN                                                           \r\n";
                SQL += "      , AGE                                                             \r\n";
                SQL += "      , SEX                                                             \r\n";
                SQL += "      , JUSO                                                            \r\n";
                SQL += "      , LASTDATE                                                        \r\n";
                SQL += "      , DEPTCODE                                                        \r\n";
                SQL += " FROM (                                                                 \r\n";
                SQL += "        SELECT                                                                                         \r\n";
                SQL += "               PANO                                                                                    \r\n";
                SQL += "             , SNAME                                                                                   \r\n";
                SQL += "             , JUMIN1 || '-' || JUMIN2 JUMIN                                                           \r\n";
                SQL += "             , KOSMOS_OCS.FC_GET_AGE(KOSMOS_OCS.FC_BAS_PATIENT_BIRTH(PANO),TRUNC(SYSDATE)) AS AGE      \r\n";
                SQL += "             , SEX                                                                                     \r\n";
                SQL += "             , JUSO                                                                                    \r\n";
                SQL += "             , LASTDATE                                                                                \r\n";
                SQL += "             , DEPTCODE                                                                                \r\n";
                SQL += "           FROM " + ComNum.DB_PMPA + "BAS_PATIENT                                                      \r\n";
                SQL += "           WHERE 1 = 1                                                                                 \r\n";
                SQL += "             AND ( ( PANO LIKE '" + strWhere + "' || '%') OR ( JUMIN1 LIKE '" + strWhere + "%') OR ( SNAME LIKE '" + strWhere + "%') )               \r\n";
                SQL += "      )                                                                                                \r\n";
                SQL += " ORDER BY TO_NUMBER(AGE) DESC                                                                          \r\n"; 
            }
            else if (enmType == enmSearchType.ERPINFO)
            {

                SQL += "    SELECT TO_CHAR(A.IDNUMBER)  AS PANO                                                                             \r\n";
                SQL += "         , A.USERNAME           AS SNAME                                                                            \r\n";
                SQL += "         , B.NAME               AS JUMIN1                                                                           \r\n";
                SQL += "         , MAX(T.TEL)           AS SEX                                                                              \r\n";
                SQL += "     FROM KOSMOS_PMPA.BAS_USER  A                                                                                   \r\n";
                SQL += "         , KOSMOS_ADM.INSA_MST  M                                                                                   \r\n";
                SQL += "         , KOSMOS_PMPA.BAS_BUSE B                                                                                   \r\n";
                SQL += "         , KOSMOS_PMPA.BAS_TEL  T                                                                                   \r\n";
                SQL += "    WHERE 1 = 1                                                                                                     \r\n";

                if (strWhere.Length == 6)
                {
                    SQL += "      AND M.SABUN = '" + strWhere + "'                                                                          \r\n";
                }

                else
                {
                    if (isUK == true)
                    {
                        SQL += "      AND A.IDNUMBER = '" + strWhere + "'                                                                       \r\n";
                    }
                    else
                    {
                        SQL += "      AND((A.IDNUMBER LIKE '" + strWhere + "' || '%') OR(A.USERNAME LIKE '" + strWhere + "' || '%') OR(B.NAME LIKE '" + strWhere + "' || '%') )      \r\n";
                    }
                }
                SQL += "      AND A.SABUN = M.SABUN(+)                                                                                      \r\n";
                SQL += "      AND M.BUSE = B.BUCODE(+)                                                                                      \r\n";
                SQL += "      AND M.BUSE = T.BUSE(+)                                                                                        \r\n";
                SQL += "    GROUP BY TO_CHAR(A.IDNUMBER),A.USERNAME,B.NAME                                                                  \r\n";
                SQL += "    ORDER BY A.USERNAME                                                                                             \r\n";

            }
            else if (enmType == enmSearchType.BLOODINFO)
            {
                SQL += "    SELECT   DISTINCT                                                                                               \r\n";
                SQL += "             PANO                                                                                                   \r\n";
                SQL += "           , SNAME                                                                                                  \r\n";
                SQL += "           , SEX                                                                                                    \r\n";
                SQL += "           , AGE                                                                                                    \r\n";                
                SQL += "           , WARD                                                                                                   \r\n";
                SQL += "           , ROOM                                                                                                   \r\n";
                SQL += "           , ABO                                                                                                    \r\n";
                SQL += "     FROM KOSMOS_OCS.EXAM_BLOOD_MASTER                                                                              \r\n";
                SQL += "    WHERE 1 = 1                                                                                                     \r\n";
                SQL += "      AND (PANO LIKE  '" + strWhere + "'|| '%' OR SNAME LIKE  '" + strWhere + "' || '%' OR WARD LIKE '" + strWhere + "' || '%') \r\n";
            }
            else if (enmType == enmSearchType.SPECINFO)
            {
                SQL += "    SELECT                                                                                                          \r\n";                
                SQL += "             PANO                                                                                                   \r\n";
                SQL += "           , SNAME                                                                                                  \r\n";
                SQL += "           , SPECNO                                                                                                 \r\n";
                SQL += "           , AGE                                                                                                    \r\n";
                SQL += "           , WARD                                                                                                   \r\n";
                SQL += "           , ROOM                                                                                                   \r\n";
                SQL += "           , WORKSTS                                                                                                \r\n";
                SQL += "     FROM KOSMOS_OCS.EXAM_SPECMST                                                                                   \r\n";
                SQL += "    WHERE 1 = 1                                                                                                     \r\n";
                SQL += "      AND SPECNO = " + ComFunc.covSqlstr(strWhere, false);
            }

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

        /// <summary>병동에서 존재하는 코드값을 갖고 온다.</summary>
        /// <param name="isIcu"></param>
        /// <returns></returns>
        public DataTable sel_BAS_WARD_COMBO(PsmhDb pDbCon, bool isIcu=true)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";
            SQL += " SELECT                                             \r\n";
            SQL += "       WARDCODE || '.' || WARDNAME CODE_NAME        \r\n";
            SQL += "  FROM KOSMOS_PMPA.BAS_WARD                         \r\n";
            SQL += " WHERE 1 = 1                                        \r\n";

            if (isIcu == true)
            {
                SQL += "   AND WARDCODE NOT IN ('IU','IQ') \r\n";
            }
            else
            {
                SQL += "   AND WARDCODE NOT IN ( '2W')             \r\n";
            }
            SQL += "   AND USED = 'Y'                                   \r\n";
            SQL += " ORDER BY WARDCODE                                  \r\n";

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

        public string sel_BAS_BUSE_YNAME(PsmhDb pDbCon, string argBucode)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";


            SQL = "";
            SQL += " SELECT                                             \r\n";
            SQL += "       YNAME                                        \r\n";
            SQL += "  FROM KOSMOS_PMPA.BAS_BUSE                         \r\n";
            SQL += " WHERE 1 = 1                                        \r\n";          
            SQL += "   AND BUCODE = '" + argBucode + "'                 \r\n";
            SQL += "   AND YNAME IS NOT NULL                            \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["YNAME"].ToString().Trim();
                }

            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return rtnVal;
        }

        /// <summary>진료과정보</summary>
        /// <returns></returns>
        public DataTable sel_BAS_CLINICDEPT_COMBO(PsmhDb pDbCon, List<string> lstNotInclude = null)
        {

            DataTable dt = null;

            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수
            string strSqlWhere = string.Empty;

            SQL = "";
            SQL += " SELECT                                             \r\n";
            SQL += "       DEPTCODE || '.' || DEPTNAMEK CODE_NAME       \r\n";
            SQL += "  FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT        \r\n";
            SQL += " WHERE 1=1                                          \r\n";

            if (lstNotInclude != null && lstNotInclude.Count > 0)
            {
                for (int i = 0; i < lstNotInclude.Count; i++)
                {
                    strSqlWhere += "'" + lstNotInclude[i] + "',";
                }

                strSqlWhere = strSqlWhere.Substring(0, strSqlWhere.Length - 1);

                SQL += "   AND DEPTCODE NOT IN (" + strSqlWhere + ")";

            }

            SQL += "  ORDER BY PRINTRANKING                             \r\n";

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

        public DataTable sel_EXAM_BLOOD_COMM_COMBO(PsmhDb pDbCon, bool isDelName = false)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";

            try
            {
                SQL = "";

                if (isDelName == true)
                {
                    SQL += " SELECT CODE AS CODE         \r\n";
                }
                else
                {
                    SQL += " SELECT CODE || '.' || NAME AS CODE         \r\n";
                }

                SQL += "   FROM " + ComNum.DB_MED + "EXAM_BLOOD_COMM B   \r\n";
                SQL += "  WHERE 1 = 1                               \r\n";
                SQL += "  ORDER BY CODE DESC                            \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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



            return dt;

        }

        public enum enmSel_BAS_BCODE {CODE,NAME,GUBUN2,GUBUN3 };

        public DataTable sel_BAS_BCODE(PsmhDb pDbCon, string gubun)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT CODE, NAME, GUBUN2, GUBUN3          \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_BCODE B   \r\n";
                SQL += "  WHERE 1 = 1                               \r\n";
                SQL += "    AND GUBUN = " + ComFunc.covSqlstr(gubun, false);
                SQL += "  ORDER BY SORT,CODE                        \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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



            return dt;
        }

        /// <summary>2017.06.12.김홍록:기초코드를 통해 콤보박스 설정</summary>
        /// <param name="gubun"></param>
        /// <returns></returns>
        public DataTable sel_BAS_BCODE_COMBO(PsmhDb pDbCon, string gubun, bool isDelName = false)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";

            try
            {
                SQL = "";

                if (isDelName == true)
                {
                    SQL += " SELECT CODE AS CODE         \r\n";
                }
                else
                {
                    SQL += " SELECT CODE || '.' || NAME AS CODE         \r\n";
                }
                
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_BCODE B   \r\n";
                SQL += "  WHERE 1 = 1                               \r\n";
                SQL += "    AND GUBUN = " + ComFunc.covSqlstr(gubun, false);
                SQL += "  ORDER BY SORT,CODE                             \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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



            return dt;

        }

        public DataTable sel_BAS_BCODE_COMBO_NAME(PsmhDb pDbCon, string gubun)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";

            try
            {
                SQL = "";

                SQL += " SELECT NAME AS CODE         \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_BCODE B   \r\n";
                SQL += "  WHERE 1 = 1                               \r\n";
                SQL += "    AND GUBUN = " + ComFunc.covSqlstr(gubun, false);
                SQL += "  ORDER BY SORT,CODE                        \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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



            return dt;

        }

        /// <summary>sel_EXAM_MASTER_ComBo</summary>
        /// <returns></returns>
        public DataTable sel_EXAM_MASTER_COMBO(PsmhDb pDbCon)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT MASTERCODE || '.' || EXAMFNAME AS CODENAME  \r\n";
                SQL += "   FROM KOSMOS_OCS.EXAM_MASTER                      \r\n";
                SQL += "  ORDER BY MASTERCODE, EXAMFNAME                    \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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

            return dt;
        }

        /// <summary>sel_OCS_ORDERCODE_COMBO</summary>
        /// <returns></returns>
        public DataTable sel_OCS_ORDERCODE_EXAM_COMBO(PsmhDb pDbCon)
        {
            DataTable dt = null;

            SQL = "";
            SQL += " SELECT                                         \r\n";
            SQL += "        SLIPNO || '.' || ORDERNAME CODENAME     \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "OCS_ORDERCODE      \r\n";
            SQL += " WHERE 1=1                                      \r\n";
            SQL += "   AND SLIPNO BETWEEN '0010'                    \r\n";
            SQL += "                  AND '0050'                    \r\n";
            SQL += "   AND SEQNO = 0                                \r\n";
            SQL += "   AND NAL > 0                                  \r\n";
            SQL += " ORDER BY  NAL, SLIPNO                          \r\n";

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

        /// <summary>EXAM_SPECODE</summary>
        /// <param name="strGubun"></param>
        /// <param isCombo="strGubun">콤보또는 항목 도출용</param>
        /// <returns></returns>
        public DataSet sel_EXAM_SPECODE_COMBO(PsmhDb pDbCon, string strGubun)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                         \r\n";
            SQL += "        CODE || '.' || NAME CODENAME        \r\n";            
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_SPECODE       \r\n";
            SQL += " WHERE 1=1                                      \r\n";
            SQL += "   AND GUBUN = " + ComFunc.covSqlstr(strGubun, false);
            SQL += "   AND DelDate IS NULL                          \r\n";            
            SQL += " ORDER BY  CODE                                 \r\n";

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
       
        public DataSet sel_WC_COMBO(PsmhDb pDbCon, bool isGBBASE = false)
        {
            DataSet ds = null;

            SQL = "";
            SQL += " SELECT                                       \r\n";
            SQL += "        YNAME || '.' || NAME CODENAME         \r\n";
            SQL += "   FROM " + ComNum.DB_MED + "EXAM_SPECODE     \r\n";
            SQL += "  WHERE 1=1                                   \r\n";
            SQL += "    AND GUBUN = '11'                          \r\n";
            SQL += "    AND YNAME IS NOT NULL                     \r\n";

            if (isGBBASE == true)
            {
                SQL += "           AND YNAME IN (                                       \r\n";
			    SQL += "    	                    SELECT TONGBUN                      \r\n";
			    SQL += "    	                      FROM KOSMOS_OCS.EXAM_MASTER       \r\n";
			    SQL += "    	                     WHERE 1=1                          \r\n";
			    SQL += "    	                       AND GBBASE = '*'                 \r\n";
                SQL += "    	   )                                                    \r\n";
            }                                                                           

            SQL += "  GROUP BY YNAME , NAME                       \r\n";
            SQL += "  ORDER BY YNAME                              \r\n";

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

        /// <summary>처방삭제용</summary>
        /// <param name="strPtNo">환자번호</param>
        /// <returns></returns>
        public DataSet sel_EXAM_ORDER_CLEAR(PsmhDb pDbCon, string strPtNo)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            try
            {
                SQL = "";
                SQL += " SELECT                                                                         \r\n";
                SQL += "         '' 						AS CHK    		-- 01                       \r\n";
                SQL += " 	  ,	TO_CHAR(BDATE,'YYYY-MM-DD') AS BDATE        -- 02                       \r\n";
                SQL += " 	  , DEPTCODE                    AS DEPTCODE     -- 03                       \r\n";
                SQL += " 	  , CASE WHEN ORDERNO > 2400 THEN 'OCS'                                     \r\n";
                SQL += " 	         ELSE 'SLIP' END		AS TYPE			-- 04                       \r\n";
                SQL += " 	  , MASTERCODE                  AS MASTERCODE	-- 05                       \r\n";
                SQL += " 	  , QTY                         AS QTY			-- 06                       \r\n";
                SQL += " 	  , KOSMOS_OCS.FC_EXAM_MASTER_EXAMNAME(MASTERCODE) AS MASTER_NM --07        \r\n";
                SQL += " 	  , SPECCODE					AS SPECCODE	    -- 08                       \r\n";
                SQL += " 	  , STRT                        AS STRT			-- 09                       \r\n";
                SQL += " 	  , DRCOMMENT                   AS DRCOMMENT	-- 10                       \r\n";
                SQL += " 	  , ORDERNO                     AS ORDERNO		-- 11                       \r\n";
                SQL += " 	  , ROWID                                       -- 12                       \r\n";
                SQL += "  FROM KOSMOS_OCS.EXAM_ORDER                                                    \r\n";
                SQL += " WHERE 1=1                                                                      \r\n";
                SQL += "   AND PANO = " + ComFunc.covSqlstr(strPtNo, false);
                SQL += "   AND (CANCEL IS NULL OR CANCEL = ' ')                                         \r\n";
                SQL += "   AND SPECNO IS NULL                                                           \r\n";
                SQL += " ORDER BY BDATE DESC,ORDERNO,MASTERCODE                                         \r\n";

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

        /// <summary>WS 갖고 오기</summary>
        /// <param name="gubun"></param>
        /// <returns></returns>
        public DataTable sel_EXAM_SPECODE_WS_COMBO(PsmhDb pDbCon)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            try
            {
                SQL = "";
                SQL += " SELECT YNAME || '.' || NAME AS CODE        \r\n";
                SQL += "   FROM " + ComNum.DB_MED + "EXAM_SPECODE B \r\n";
                SQL += "  WHERE 1 = 1                               \r\n";
                SQL += "    AND GUBUN               = '12'          \r\n";
                SQL += "    AND SUBSTR(CODE,3,1)    = '0'           \r\n";
                SQL += "    AND YNAME IS NOT NULL                   \r\n";
                SQL += "  ORDER BY SORT                             \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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
            return dt;

        }

        /// <summary>의사정보</summary>
        /// <param name="strSabun"></param>
        /// <returns></returns>
        public DataTable sel_OCS_DOCTOR_DRCODE(PsmhDb pDbCon, string strSabun)
        {

            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            try
            {
                SQL = "";
                SQL += " SELECT DRCODE,GBOUT                       \r\n";
                SQL += "   FROM " + ComNum.DB_MED + "OCS_DOCTOR B  \r\n";
                SQL += "  WHERE 1 = 1                               \r\n";
                SQL += "    AND SABUN = " + ComFunc.covSqlstr(strSabun, false);

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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
            return dt;
        }

        /// <summary>Glucose pp2hrs</summary>
        /// <param name="strPtNo">환자번호</param>
        /// <returns></returns>
        public DataSet sel_EXAM_ORDER_CR59B(PsmhDb pDbCon, string strRowId)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = "";
            SQL += " SELECT                                    \r\n";
            SQL += " 	  A.BDATE                              \r\n";
            SQL += " 	, A.PANO                               \r\n";
            SQL += " 	, A.SNAME                              \r\n";
            SQL += " 	, A.AGE                                \r\n";
            SQL += " 	, A.SEX                                \r\n";
            SQL += " 	, A.MASTERCODE                         \r\n";
            SQL += " 	, B.EXAMNAME                           \r\n";
            SQL += " 	, C.RESULT                             \r\n";
            SQL += " 	, A.SPECNO                             \r\n";            
            SQL += " 	, A.ROWID                              \r\n";
            SQL += " FROM KOSMOS_OCS.EXAM_ORDER 	A          \r\n";
            SQL += "    , KOSMOS_OCS.EXAM_MASTER 	B          \r\n";
            SQL += "    , KOSMOS_OCS.EXAM_RESULTC 	C          \r\n";
            SQL += "  WHERE 1=1                                \r\n";
            SQL += "    AND A.MASTERCODE = B.MASTERCODE        \r\n";
            SQL += "    AND A.SPECNO     = C.SPECNO(+)         \r\n";
            SQL += "    AND A.ROWID   	 = " + ComFunc.covSqlstr(strRowId, false);

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
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

            }
            return ds;
        }

        public DataTable sel_VACCINE_PCKIND_COMBO(PsmhDb pDbCon)
        {
            DataTable dt = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수


            SQL = "";

            try
            {
                SQL = "";
                SQL += " SELECT VCNCOD || '.' || VCNNAM AS CODE         \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "VACCINE_PCKIND B  \r\n";
                SQL += "  WHERE 1 = 1                                   \r\n";
                SQL += "  ORDER BY PRINTRANKING                         \r\n";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

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



            return dt;

        }

        /// <summary>생성자</summary>
        public clsComSQL()
        {
            //gDr_OCS_OORDER = gDt_OCS_OORDER.NewRow();

            foreach (string s in Enum.GetNames(typeof(enm_OCS_OORDER)))
            {
                gDt_OCS_OORDER.Columns.Add(s);
            }

            gDr_OCS_OORDER = gDt_OCS_OORDER.NewRow();

        }

        /// <summary></summary>
        /// <param name="dr"></param>
        /// <param name="isDC"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string upOCS_OORDER(PsmhDb pDbCon, DataRow dr, bool isDC, ref int intRowAffected)
        {

            string SqlErr = ""; //에러문 받는 변수

            string s = dr[enm_OCS_OORDER.GBBOTH.ToString()].ToString();

            SQL = "";
            SQL = SQL + "   UPDATE KOSMOS_OCS.OCS_OORDER                                                            \r\n";
            SQL = SQL + "      SET GBBOTH = " + ComFunc.covSqlstr(dr[enm_OCS_OORDER.GBBOTH.ToString()].ToString(), false);
            SQL = SQL + "    WHERE 1=1                                                                              \r\n";
            SQL = SQL + "      AND ROWID  = " + ComFunc.covSqlstr(dr[enm_OCS_OORDER.ROWID.ToString()].ToString(), false);

            if (isDC == true)
            {
                SQL = SQL + "      AND GBBOTH = '3'                                                                     \r\n";
            }
            else
            {
                SQL = SQL + "      AND GBBOTH != '3'                                                                     \r\n";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return SqlErr;
            }

            return SqlErr;
        }

        /// <summary>up_EXAM_ORDER/// </summary>
        /// <param name="strSabun"></param>
        /// <param name="strRowId"></param>
        /// <param name="isCancel"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string up_EXAM_ORDER(PsmhDb pDbCon, string strSabun, string strRowId, bool isCancel, ref int intRowAffected)
        {
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + "   UPDATE KOSMOS_OCS.EXAM_ORDER                                                            \r\n";

            if (isCancel == true)
            {
                SQL = SQL + "      SET CANCEL = " + ComFunc.covSqlstr("015", false);
            }
            else
            {
                SQL = SQL + "      SET CANCEL = " + ComFunc.covSqlstr("", false);
            }

            SQL = SQL + "      , CDATE = SYSDATE                                                                    \r\n";
            SQL = SQL + "      , PART  = " + ComFunc.covSqlstr(strSabun, false);

            SQL = SQL + "    WHERE 1=1                                                                              \r\n";
            SQL = SQL + "      AND ROWID  = " + ComFunc.covSqlstr(strRowId, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return SqlErr;
            }

            return SqlErr;
        }

        /// <summary>Glucose 입력</summary>
        /// <param name="strSpecNo"></param>
        /// <param name="strResult"></param>
        /// <param name="strSabun"></param>
        /// <param name="strRef"></param>
        /// <param name="strPanic"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string ins_EXAM_REULTC_CR59B(PsmhDb pDbCon, string strRowId, string strSpecNo, string strResult, string strSabun, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "    INSERT INTO KOSMOS_OCS.EXAM_RESULTC (   \r\n";
            SQL += "          SPECNO                            \r\n";
            SQL += "        , RESULTWS                          \r\n";
            SQL += "        , EQUCODE                           \r\n";
            SQL += "        , SEQNO                             \r\n";
            SQL += "        , PANO                              \r\n";
            SQL += "        , MASTERCODE                        \r\n";
            SQL += "        , SUBCODE                           \r\n";
            SQL += "        , STATUS                            \r\n";
            SQL += "        , RESULT                            \r\n";
            SQL += "        , RESULTDATE                        \r\n";
            SQL += "        , RESULTSABUN                       \r\n";
            SQL += "        , REFER                             \r\n";
            SQL += "        , PANIC                             \r\n";
            SQL += "        , DELTA                             \r\n";
            SQL += "        , UNIT                              \r\n";
            SQL += "            )                               \r\n";
            SQL += "        SELECT                                      \r\n";
            SQL += "            " + ComFunc.covSqlstr(strSpecNo, false);
            SQL += "            ,'C' 					AS RESULTWS     \r\n";
            SQL += "            , '009'  				AS EQUCODE      \r\n";
            SQL += "            , '001'  				AS SEQNO        \r\n";
            SQL += "            ,  PANO                                 \r\n";
            SQL += "            ,  MASTERCODE                           \r\n";
            SQL += "            ,  MASTERCODE                           \r\n";
            SQL += "            , 'V' 					AS STATUS       \r\n";
            SQL += "            , '" + strResult + "'   AS RESULT       \r\n";
            SQL += "            , SYSDATE 				AS RESULTDATE   \r\n";
            SQL += "            , '" + strSabun + "'   AS ESULTSABUN     \r\n";
            SQL += "            , KOSMOS_OCS.FC_EXAM_MASTER_SUB_REF('2',MASTERCODE,SEX,AGE,'" + strResult + "')   AS REFER        \r\n";
            SQL += "            , KOSMOS_OCS.FC_EXAM_MASTER_PANIC(MASTERCODE,'" + strResult + "')          		  AS PANIC        \r\n";
            SQL += "            , '' 					AS DELTA        \r\n";
            SQL += "            , 'mg/dL' 				AS UNIT         \r\n";
            SQL += "        FROM KOSMOS_OCS.EXAM_ORDER                  \r\n";
            SQL += "        WHERE  1=1                                  \r\n";
            SQL += "            AND ROWID = " + ComFunc.covSqlstr(strRowId, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return SqlErr;
            }

            return SqlErr;
        }

        /// <summary>ins_EXAM_SPECMST</summary>
        /// <param name="strRowId"></param>
        /// <param name="strSpeNO"></param>
        /// <param name="TRS"></param>
        /// <param name="intRowAffected"></param>
        /// <returns></returns>
        public string ins_EXAM_SPECMST(PsmhDb pDbCon, string strRowId, string strSpeNO, ref int intRowAffected)
        {

            string SqlErr = string.Empty;

            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_MED + "EXAM_SPECMST( \r\n";
            SQL += "       SPECNO,PANO,BI,SNAME,IPDOPD,AGE,AGEMM,SEX,DEPTCODE,WARD,ROOM,DRCODE,DRCOMMENT,STRT,WORKSTS                                  \r\n";
            SQL += "     , SPECCODE,TUBE,BDATE,BLOODDATE,RECEIVEDATE,RESULTDATE,STATUS,CANCEL,PRINT,ANATNO,GBORDERCODE,EMR,ORDERDATE,SENDDATE,SENDFLAG \r\n";
            SQL += "     , GB_GWAEXAM,GBPRINT,HICNO,VIEWID,VIEWDATE,ORDERNO)                                                                            \r\n";
            SQL += "     SELECT '" + strSpeNO + "' , PANO, BI, SNAME ,IPDOPD , AGE, AGEMM, SEX, DEPTCODE, '', '', DRCODE,  DRCOMMENT, 'R' STRT, 'C' WORKSTS , '150'  SPECCODE, '092'  TUBE, BDATE,  SYSDATE BLOODDATE, SYSDATE RECEICEDATE, SYSDATE RESULTDATE, '05' STATUS,    \r\n";
            SQL += "     '' CANCEL, 0 PRINT,  '' ANANTNO, '' GBORDERCODE, '' EMR,   ORDERDATE,  SENDDATE, '' SENDFLAG, 'Y' GB_GWAEXAM, '' GBPRINT,  0 HICNO , '' VIEWID , '' VIEWDATE, ORDERNO                                                                                  \r\n";
            SQL += "     FROM KOSMOS_OCS.EXAM_ORDER                                                                                                                                                                                                                             \r\n";
            SQL += "    WHERE  ROWID = '" + strRowId + "' ";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon, 100);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return SqlErr;
            }

            return SqlErr;
        }

        public string up_EXAM_REULTC_CR59B(PsmhDb pDbCon, string strSpecNo, string strResult, string strSabun, ref int intRowAffected)
        {

            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "EXAM_RESULTC   \r\n";
            SQL += "    SET RESULT      = " + ComFunc.covSqlstr(strResult, false);
            SQL += "      , RESULTSABUN = " + ComFunc.covSqlstr(strSabun, false);
            SQL += "      , RESULTDATE  = SYSDATE   \r\n";
            SQL += "    WHERE    SPECNO = " + ComFunc.covSqlstr(strSpecNo, false);
            SQL += "      AND    MASTERCODE = 'CR59B'";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return SqlErr;
            }

            return SqlErr;
        }

        public string up_EXAM_ORDER_CR59B(PsmhDb pDbCon, string strRowID, string strSpecNo, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += " UPDATE " + ComNum.DB_MED + "EXAM_ORDER   \r\n";
            SQL += "    SET SPECNO      = " + ComFunc.covSqlstr(strSpecNo, false);
            SQL += "    WHERE  ROWID    = " + ComFunc.covSqlstr(strRowID, false);

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return SqlErr;
            }

            return SqlErr;

        }


    }
}
