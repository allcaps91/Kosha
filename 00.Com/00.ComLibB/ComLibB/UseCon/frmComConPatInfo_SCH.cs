using ComBase; //기본 클래스
using ComDbB; //DB연결
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : frmComSupSTS02.cs
    /// Description     : 진료지원 종합 상태 조회 폼
    /// Author          : 김홍록
    /// Create Date     : 2018-03-13
    /// Update History  : 
    /// </summary>
    /// <history>  
    ///  frmComSupSTS01.cs 신규폼 생성
    /// </history>
    /// <seealso cref= " frmComSupSTS01.cs 폼이름 재정의" />
    public partial class frmComConPatInfo_SCH : Form
    {
        string gStrPANO = "";

        Color cSpdCellClick = Color.FromArgb(240, 250, 250);

        clsSpread spread = new clsSpread();

        public enum enmSel_PATIENT_SCHDUL_Detail          {    SORT,      GBN,     GBN2,     GBN3,    CHK_ACT,            RDATE,  DEPTCODE,  DRCODE  ,FC_DRNAME };
        public string[] sSel_PATIENT_SCHDUL_Detail  =     {  "SORT",   "분류", "중분류", "처방명", "시행여부", "시행(예정)일자",  "진료과", "DRCODE" , "진료의" };
        public int[] nSel_PATIENT_SCHDUL_Detail     =     {       5,       60,       80,      180,         40,              120,        60,         5,       80 };

        public frmComConPatInfo_SCH(string strPANO, string strSNAME)
        {
            InitializeComponent();

            this.gStrPANO   = strPANO;

            setEvent();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
                setCtrl();
            }           
        }

        void setCtrl()
        {
            setCtrlSpread();
        }

        void setEvent()
        {
            this.Load                   += new EventHandler(eFormLoad);
            this.ss_PTINFO.CellClick    += new CellClickEventHandler(eSpreadClick);
            this.ss_PTINFO.LeaveCell += new LeaveCellEventHandler(eSPreadLeaveCell);
            
        }

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

        DataTable sel_PATIENT_SCHDUL(PsmhDb pDbCon, string strPANO)
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
                SQL += "  	   AND ACTDATE > SYSDATE-" + strLIMIT_DAY + "                                                   \r\n";
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
                SQL += "  	  		'2' 								AS GUBUN                                            \r\n";
                SQL += "  	      ,  TO_CHAR(RDATE,'YYYY-MM-DD' ) 		AS BDATE                                            \r\n";
                SQL += "  	      , COUNT(DEPTCODE)						AS CNT                                              \r\n";
                SQL += "  	   FROM KOSMOS_PMPA.OPD_TELRESV                                                                 \r\n";
                SQL += "  	  WHERE 1 = 1                                                                                   \r\n";
                SQL += "  	   AND Pano = " + ComFunc.covSqlstr(strPANO, false);
                SQL += "  	   AND RDATE > TRUNC(SYSDATE)                                                               \r\n";
                SQL += "  	GROUP BY TO_CHAR(RDATE,'YYYY-MM-DD' )                                                           \r\n";
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
                SQL += "         ,  ' 0'	    AS GUBUN2                                                                   \r\n";

                for (int i = 0; i < sArrBDATE.Count; i++)
                {
                    SQL += "        , '" + sArrBDATE[i] + "' AS DAY" + i.ToString() + "                                     \r\n";
                }

                SQL += "   FROM DUAL                                                                                        \r\n";
                SQL += " UNION ALL                                                                                          \r\n";
                SQL += "  SELECT                                                                                            \r\n";
                //SQL += "  	   CASE WHEN B.GUBUN2 = '1' THEN '외래접수'                                                     \r\n";
                //SQL += "  			WHEN B.GUBUN2 = '2' THEN '외래예약'                                                     \r\n";
                //SQL += "  			WHEN B.GUBUN2 = '3' THEN '수술'                                                         \r\n";
                //SQL += "  			WHEN B.GUBUN2 = '4' THEN '내시경'                                                       \r\n";
                //SQL += "  			WHEN B.GUBUN2 = '5' THEN '기능검사'                                                     \r\n";
                //SQL += "  			WHEN B.GUBUN2 = '6' THEN '영상의학'                                                     \r\n";
                //SQL += "  			WHEN B.GUBUN2 = '7' THEN '진단검사'                                                     \r\n";
                SQL += "  	   CASE WHEN A.GUBUN = '1' THEN '외래접수'                                                     \r\n";
                SQL += "  			WHEN A.GUBUN = '2' THEN '외래예약'                                                     \r\n";
                SQL += "  			WHEN A.GUBUN = '3' THEN '수술'                                                         \r\n";
                SQL += "  			WHEN A.GUBUN = '4' THEN '내시경'                                                       \r\n";
                SQL += "  			WHEN A.GUBUN = '5' THEN '기능검사'                                                     \r\n";
                SQL += "  			WHEN A.GUBUN = '6' THEN '영상의학'                                                     \r\n";
                SQL += "  			WHEN A.GUBUN = '7' THEN '진단검사'                                                     \r\n";

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
                SQL += "  	ORDER BY GUBUN2                                                                                 \r\n";
                //SQL += "   , (                                                                                              \r\n";
                //SQL += "   		SELECT TO_CHAR(LEVEL) GUBUN2                                                                \r\n";
                //SQL += "   		   FROM DUAL                                                                                \r\n";
                //SQL += "   		   CONNECT BY LEVEL < 8                                                                     \r\n";
                //SQL += "   	) B                                                                                             \r\n";
                //SQL += "   WHERE 1=1                                                                                        \r\n";
                //SQL += "     AND B.GUBUN2 = A.GUBUN(+)                                                                      \r\n";

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

        DataSet sel_PATIENT_SCHDUL_Detail(PsmhDb pDbCon, string strPANO, string strBDATE)
        {
            DataSet ds = null;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL += "   SELECT                                                                                                               \r\n";
            SQL += "     '1' 																SORT                                            \r\n";
            SQL += "    ,'진료' 															GBN                                             \r\n";
            SQL += "    ,'접수' 															GBN2                                            \r\n";
            SQL += "    , '' 																GBN3                                            \r\n";
            SQL += "    ,	DECODE(NVL(OCSJIN,'^*'),'^*','','Y')							CHK_ACT                                         \r\n";
            SQL += "    , TO_CHAR(BDATE,'YYYY-MM-DD HH24:MI') 							RDATE                                               \r\n";
            SQL += "    , DEPTCODE                                                                                                          \r\n";
            SQL += "    , DRCODE                                                                                                            \r\n";
            SQL += "    , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DRCODE) 						FC_DRNAME                                           \r\n";
            SQL += "     FROM KOSMOS_PMPA.OPD_MASTER                                                                                        \r\n";
            SQL += "    WHERE 1 = 1                                                                                                         \r\n";
            SQL += "     AND PANO   = " + ComFunc.covSqlstr(strPANO, false);
            SQL += "     AND BDATE  = TO_DATE('" + strBDATE + "','YYYY-MM-DD')                                                                     \r\n";
            SQL += "   UNION ALL                                                                                                            \r\n";
            SQL += "   SELECT                                                                                                               \r\n";
            SQL += "      '2' 															SORT                                                \r\n";
            SQL += "    , '진료예약' 														GBN                                             \r\n";
            SQL += "    , '예약' 															GBN2                                            \r\n";
            SQL += "    , '' 																GBN3                                            \r\n";
            SQL += "    , '' 																CHK_ACT                                         \r\n";
            SQL += "    , TO_CHAR(DATE3,'YYYY-MM-DD HH24:MI') 							RDATE                                               \r\n";
            SQL += "    ,	DEPTCODE                                                                                                        \r\n";
            SQL += "    ,	DRCODE                                                                                                          \r\n";
            SQL += "    ,	KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DRCODE) 						FC_DRNAME                                       \r\n";
            SQL += "     FROM KOSMOS_PMPA.OPD_RESERVED_NEW                                                                                  \r\n";
            SQL += "    WHERE 1 = 1                                                                                                         \r\n";
            SQL += "     AND PANO  = " + ComFunc.covSqlstr(strPANO, false);
            SQL += "     AND (RETDATE IS NULL OR RETDATE  ='')                                                                              \r\n";
            SQL += "     AND (TRANSDATE IS NULL OR TRANSDATE='' OR TRUNC(TRANSDATE) > TO_DATE( '" + strBDATE + "','YYYY-MM-DD' ) )                \r\n";
            SQL += "     AND TRUNC(DATE3) = TO_DATE( '" + strBDATE + "','YYYY-MM-DD' )                                                            \r\n";
            SQL += "   UNION ALL                                                                                                            \r\n";
            SQL += "   SELECT                                                                                                               \r\n";
            SQL += "    	'2' 															SORT                                            \r\n";
            SQL += "    , '진료예약' 														GBN                                             \r\n";
            SQL += "    , '전화' 															GBN2                                            \r\n";
            SQL += "    , '' 																GBN3                                            \r\n";
            SQL += "    ,	'' 																CHK_ACT                                         \r\n";
            SQL += "    ,	TO_CHAR(RDATE,'YYYY-MM-DD') || ' ' || RTIME RDATE                                                               \r\n";
            SQL += "    ,	DEPTCODE                                                                                                        \r\n";
            SQL += "    ,	DRCODE                                                                                                          \r\n";
            SQL += "    ,	KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DRCODE) 						FC_DRNAME                                       \r\n";
            SQL += "     FROM KOSMOS_PMPA.OPD_TELRESV                                                                                       \r\n";
            SQL += "    WHERE 1 = 1                                                                                                         \r\n";
            SQL += "     AND PANO  =" + ComFunc.covSqlstr(strPANO, false);
            SQL += "     AND RDATE = TO_DATE( '" + strBDATE + "','YYYY-MM-DD' )                                                                   \r\n";
            SQL += "   UNION ALL                                                                                                            \r\n";
            SQL += "   SELECT                                                                                                               \r\n";
            SQL += "    	'3' 															SORT                                            \r\n";
            SQL += "    ,	'수술예약' 														GBN                                             \r\n";
            SQL += "    ,	TRIM(OPBUN) || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('ORAN_수술구분',TRIM(OPBUN)) GBN2                        \r\n";
            SQL += "    ,	OPTITLE 														GBN3                                            \r\n";
            SQL += "    , DECODE(NVL(OPETIME,'^*'),'^*','','Y')							CHK_ACT                                             \r\n";
            SQL += "    ,	TO_CHAR(OPDATE,'YYYY-MM-DD HH24:MI') 							RDATE                                           \r\n";
            SQL += "    , DEPTCODE                                                                                                          \r\n";
            SQL += "    , DRCODE                                                                                                            \r\n";
            SQL += "    ,	KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DRCODE) FC_DRNAME                                                               \r\n";
            SQL += "     FROM KOSMOS_PMPA.ORAN_MASTER                                                                                       \r\n";
            SQL += "    WHERE 1 = 1                                                                                                         \r\n";
            SQL += "     AND PANO  = " + ComFunc.covSqlstr(strPANO, false);
            SQL += "     AND OPCANCEL IS NULL                                                                                               \r\n";
            SQL += "     AND TRUNC(OPDATE) = TO_DATE( '" + strBDATE + "','YYYY-MM-DD' )                                                           \r\n";
            SQL += "   UNION ALL                                                                                                            \r\n";
            SQL += "   SELECT                                                                                                               \r\n";
            SQL += "     	'4' 																					SORT                    \r\n";
            SQL += "    ,	'내시경' 																				GBN                     \r\n";
            SQL += "    ,	TRIM(GBJOB) || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('ENDO_내시경분류',TRIM(GBJOB)) 	GBN2                    \r\n";
            SQL += "    ,	KOSMOS_OCS.FC_OCS_ORDERCODE_NAME(ORDERCODE) 											GBN3                    \r\n";
            SQL += "    ,	DECODE(GBSUNAP,'2','','Y')																CHK_ACT                 \r\n";
            SQL += "    ,	TO_CHAR(RDATE,'YYYY-MM-DD HH24:MI') 													RDATE                   \r\n";
            SQL += "    , DEPTCODE                                                                                                          \r\n";
            SQL += "    , DRCODE                                                                                                            \r\n";
            SQL += "    ,	KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DRCODE) 												FC_DRNAME               \r\n";
            SQL += "     FROM KOSMOS_OCS.ENDO_JUPMST                                                                                        \r\n";
            SQL += "    WHERE 1 = 1                                                                                                         \r\n";
            SQL += "     AND PTNO  = " + ComFunc.covSqlstr(strPANO, false);
            SQL += "     AND GBSUNAP IN ('1','2','7')                                                                                       \r\n";
            SQL += "     AND RDATE = TO_DATE( '" + strBDATE + "','YYYY-MM-DD' )                                                                   \r\n";
            SQL += "   UNION ALL                                                                                                            \r\n";
            SQL += "   SELECT                                                                                                               \r\n";
            SQL += "      '5' SORT                                                                                                          \r\n";
            SQL += "    ,'기능검사' GBN                                                                                                     \r\n";
            SQL += "    , TRIM(GUBUN) || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('C#_EKG검사종류',TRIM(GUBUN)) 	GBN2                        \r\n";
            SQL += "    ,	KOSMOS_OCS.FC_OCS_ORDERCODE_NAME(ORDERCODE) 											GBN3                    \r\n";
            SQL += "    , DECODE(GBJOB,'1','','Y')																CHK_ACT                     \r\n";
            SQL += "    ,	TO_CHAR(RDATE,'YYYY-MM-DD HH24:MI') 													RDATE                   \r\n";
            SQL += "    ,	DEPTCODE                                                                                                        \r\n";
            SQL += "    ,	DRCODE                                                                                                          \r\n";
            SQL += "    ,	KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(TRIM(DRCODE)) 											AS FC_DRNAME            \r\n";
            SQL += "     FROM KOSMOS_OCS.ETC_JUPMST                                                                                         \r\n";
            SQL += "    WHERE 1 = 1                                                                                                         \r\n";
            SQL += "     AND PTNO  = " + ComFunc.covSqlstr(strPANO, false);
            SQL += "     AND GBJOB IN ('1','2','3')                                                                                         \r\n";
            SQL += "     AND GUBUN NOT IN ('17','18','19','20')                                                                             \r\n";
            SQL += "     AND TRUNC(RDATE) = TO_DATE( '" + strBDATE + "','YYYY-MM-DD' )                                                            \r\n";
            SQL += "   UNION ALL                                                                                                            \r\n";
            SQL += "   SELECT                                                                                                               \r\n";
            SQL += "    	'6' SORT,'영상검사' 																	GBN                     \r\n";
            SQL += "    ,	TRIM(XJONG) || '.' || KOSMOS_OCS.FC_BAS_BCODE_NAMEREAD('XRAY_방사선종류',TRIM(XJONG)) 	GBN2                    \r\n";
            SQL += "    ,	KOSMOS_OCS.FC_OCS_ORDERCODE_NAME(ORDERCODE) 											GBN3                    \r\n";
            SQL += "    ,	DECODE (GBRESERVED,'7','Y','')					 										CHK_ACT                 \r\n";
            SQL += "    ,	TO_CHAR(SEEKDATE,'YYYY-MM-DD HH24:MI') 													RDATE                   \r\n";
            SQL += "    ,	DEPTCODE                                                                                                        \r\n";
            SQL += "    ,	DRCODE                                                                                                          \r\n";
            SQL += "    ,	KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(DRCODE) FC_DRNAME                                                               \r\n";
            SQL += "     FROM KOSMOS_PMPA.XRAY_DETAIL                                                                                       \r\n";
            SQL += "    WHERE 1 = 1                                                                                                         \r\n";
            SQL += "     AND PANO  = " + ComFunc.covSqlstr(strPANO, false);
            SQL += "     AND GBRESERVED IN ('1','2','6','7')                                                                                \r\n";
            SQL += "     AND TRIM(XCODE) NOT IN (                                                                                           \r\n";
            SQL += "                        SELECT TRIM(CODE)                                                                               \r\n";
            SQL += "                          FROM KOSMOS_PMPA.BAS_BCODE                                                                    \r\n";
            SQL += "                           WHERE 1=1                                                                                    \r\n";
            SQL += "                            AND GUBUN ='C#_영상검사_기능검사제외코드'                                                   \r\n";
            SQL += "                            AND (DELDATE IS NULL OR DELDATE ='')                                                        \r\n";
            SQL += "                       )                                                                                                \r\n";
            SQL += "     AND TRUNC(RDATE) = TO_DATE( '" + strBDATE + "','YYYY-MM-DD' )                                                            \r\n";
            SQL += "  UNION ALL                                                                                                             \r\n";
            SQL += "    	 SELECT                                                                                                         \r\n";
            SQL += "          '7' 																			SORT                            \r\n";
            SQL += "    	  , '진단검사' 																		GBN                         \r\n";
            SQL += "    	  , KOSMOS_OCS.FC_OCS_ORDERCODE_SLIPNM(TRIM(A.MASTERCODE)) 							AS GBN2                     \r\n";
            SQL += "    	  , (SELECT MAX(ORDERNAME) FROM KOSMOS_OCS.OCS_ORDERCODE WHERE TRIM(ITEMCD) = A.MASTERCODE)																		AS GBN3                     \r\n";
            SQL += "     	  , DECODE(NVL(A.SPECNO,'^*'),'^*','','Y')											AS CHK_ACT                  \r\n";
            SQL += "     	  , CASE WHEN (A.RDATE IS NULL OR A.RDATE = '')  THEN TO_CHAR(A.BDATE, 'YYYY-MM-DD')                            \r\n";
            SQL += "    		    		 ELSE TO_CHAR(A.RDATE, 'YYYY-MM-DD') END 							AS RDATE                    \r\n";
            SQL += "        , DEPTCODE                                                                                                      \r\n";
            SQL += "        , DRCODE                                                                                                        \r\n";
            SQL += "        , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(TRIM(DRCODE)) FC_DRNAME                                                       \r\n";
            SQL += "    	  FROM KOSMOS_OCS.EXAM_ORDER   A                                                                                \r\n";
            SQL += "    	  WHERE 1=1                                                                                                     \r\n";
            SQL += "    	    AND A.PANO = " + ComFunc.covSqlstr(strPANO, false);
            SQL += "    	    AND (                                                                                                       \r\n";
            SQL += "    		    	CASE WHEN (A.RDATE IS NULL OR A.RDATE = '')  THEN TRUNC(A.BDATE)                                    \r\n";
            SQL += "    		    		 ELSE TRUNC(A.RDATE) END                                                                        \r\n";
            SQL += "    	    	)	= TO_DATE( '" + strBDATE + "','YYYY-MM-DD' )                                                             \r\n";
            SQL += "  ORDER BY SORT, GBN,	GBN2                                                                                            \r\n";
                                                                                                                                
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

        void setCtrlSpread()
        {
            PsmhDb pDbCon = null;
            pDbCon = clsDB.DBConnect();

            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable dt = sel_PATIENT_SCHDUL(pDbCon, this.gStrPANO);
                setSpdStyle(dt);
                clsDB.DisDBConnect(pDbCon);
                pDbCon = null;
                this.Cursor = Cursors.Default;


                if (ComFunc.isDataTableNull(dt) == false)
                {
                    setCtrlSpread_Detail(this.gStrPANO, this.ss_PTINFO.ActiveSheet.Cells[0, this.ss_PTINFO.ActiveSheet.ColumnCount - 1].Text);
                }

            }
            catch (Exception ex)
            {

                clsDB.DisDBConnect(pDbCon);
                pDbCon = null;
                ComFunc.MsgBox(ex.ToString());
            }
        }

        void setCtrlSpread_Detail(string strPANO, string strBDATE)
        {
            PsmhDb pDbCon = null;
            pDbCon = clsDB.DBConnect();

            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataSet ds = sel_PATIENT_SCHDUL_Detail(clsDB.DbCon, strPANO, strBDATE);
                setSpdStyle(this.ssl_PATIENT_SCHDUL_Detail, ds, sSel_PATIENT_SCHDUL_Detail, nSel_PATIENT_SCHDUL_Detail);
                clsDB.DisDBConnect(pDbCon);
                pDbCon = null;
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {

                clsDB.DisDBConnect(pDbCon);
                pDbCon = null;
                ComFunc.MsgBox(ex.ToString());
            }
        }

        void eSPreadLeaveCell(object sender, LeaveCellEventArgs e)
        {
            if (e.NewColumn != 0)
            {
                this.ss_PTINFO.ActiveSheet.Cells[1, 0, this.ss_PTINFO.ActiveSheet.Rows.Count - 1, this.ss_PTINFO.ActiveSheet.ColumnCount - 1].BackColor = System.Drawing.Color.White;
                this.ss_PTINFO.ActiveSheet.Cells[1, e.NewColumn, this.ss_PTINFO.ActiveSheet.Rows.Count - 1, e.NewColumn].BackColor = cSpdCellClick;

                setCtrlSpread_Detail(this.gStrPANO, this.ss_PTINFO.ActiveSheet.Cells[0, e.NewColumn].Text.Trim());

            }
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            if (e.Column != 0)
            {
                this.ss_PTINFO.ActiveSheet.Cells[1, 0, this.ss_PTINFO.ActiveSheet.Rows.Count - 1, this.ss_PTINFO.ActiveSheet.ColumnCount - 1].BackColor = System.Drawing.Color.White;
                this.ss_PTINFO.ActiveSheet.Cells[1, e.Column, this.ss_PTINFO.ActiveSheet.Rows.Count - 1, e.Column].BackColor = cSpdCellClick;

                //DateTime sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

                setCtrlSpread_Detail(this.gStrPANO, this.ss_PTINFO.ActiveSheet.Cells[0, e.Column].Text.Trim());


            }
        }

        void setSpdStyle(DataTable dt)
        {            

            if (ComFunc.isDataTableNull(dt) == false)
            {

                DateTime sysdate = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")); 


                this.ss_PTINFO.ActiveSheet.Columns.Count = dt.Columns.Count - 1;
                this.ss_PTINFO.ActiveSheet.Rows.Count = dt.Rows.Count;

                this.ss_PTINFO.ActiveSheet.Cells[0, 0, this.ss_PTINFO.ActiveSheet.Rows.Count-1, this.ss_PTINFO.ActiveSheet.Columns.Count-1].Text = "";

                this.ss_PTINFO.ActiveSheet.Cells[0, 0, this.ss_PTINFO.ActiveSheet.Rows.Count - 1, this.ss_PTINFO.ActiveSheet.Columns.Count - 1].Locked = true;

                this.ss_PTINFO.ActiveSheet.Cells[0, 0, 0, this.ss_PTINFO.ActiveSheet.Columns.Count - 1].BackColor = System.Drawing.Color.FromArgb(230, 253, 253);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.ss_PTINFO.ActiveSheet.Cells[i, 0].Text = dt.Rows[i][0].ToString();
                }

                this.ss_PTINFO.ActiveSheet.Columns.Get(0).Width = 60;
                this.ss_PTINFO.ActiveSheet.Columns[0].HorizontalAlignment = CellHorizontalAlignment.Center;
                this.ss_PTINFO.ActiveSheet.Columns[0].VerticalAlignment = CellVerticalAlignment.Center;

                for (int j = 2; j < dt.Columns.Count; j++)
                {
                    this.ss_PTINFO.ActiveSheet.Columns.Get(j-1).Width = 80;

                    this.ss_PTINFO.ActiveSheet.Columns[j - 1].HorizontalAlignment  = CellHorizontalAlignment.Center;
                    this.ss_PTINFO.ActiveSheet.Columns[j - 1].VerticalAlignment  = CellVerticalAlignment.Center;

                    for (int i = 0; i < this.ss_PTINFO.ActiveSheet.Rows.Count; i++)
                    {
                        this.ss_PTINFO.ActiveSheet.Cells[i, j-1].Text = dt.Rows[i][j].ToString();

                        if (i == 0)
                        {
                            if (Convert.ToDateTime(dt.Rows[i][j].ToString()) > sysdate)
                            {
                                this.ss_PTINFO.ActiveSheet.Cells[i, j - 1].BackColor = System.Drawing.Color.FromArgb(15,187,187);
                            }
                            else if (Convert.ToDateTime(dt.Rows[i][j].ToString()) == sysdate)
                            {
                                this.ss_PTINFO.ActiveSheet.Cells[i, j - 1].BackColor = System.Drawing.Color.FromArgb(147, 247, 247);
                            }
                        }
                    }

                }

                this.ss_PTINFO.ActiveSheet.Cells[1, this.ss_PTINFO.ActiveSheet.Columns.Count - 1, this.ss_PTINFO.ActiveSheet.Rows.Count - 1, this.ss_PTINFO.ActiveSheet.Columns.Count - 1].BackColor = cSpdCellClick;

                this.ss_PTINFO.ActiveSheet.FrozenColumnCount = 1;
                this.ss_PTINFO.ActiveSheet.SetActiveCell(1, this.ss_PTINFO.ActiveSheet.ColumnCount - 1);
                this.ss_PTINFO.ShowCell(0, 0, 0, this.ss_PTINFO.ActiveSheet.ColumnCount - 1, VerticalPosition.Center, HorizontalPosition.Center);                    
            }            
        }

        void setSpdStyle(FpSpread spd, DataSet ds, string[] colName, int[] size)
        {
            spd.ActiveSheet.ColumnHeader.Rows.Get(0).Height = 30;
            // 화면상의 정렬표시 Clear
            spd.ActiveSheet.ColumnHeader.Cells[0, 0, 0, spd.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;

            spd.DataSource = ds;
            spd.ActiveSheet.ColumnCount = colName.Length;

            spd.TextTipDelay = 500;
            spd.TextTipPolicy = TextTipPolicy.Fixed;

            //spread.setSpdSort(spd, -1, true);

            //1. 스프레드 사이즈 설정
            if (ds == null || ds.Tables[0].Rows.Count == 0)
            {
                spd.ActiveSheet.RowCount = 0;
            }

            //2. 헤더 사이즈
            spread.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            spread.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

            spd.ActiveSheet.Columns[-1].VerticalAlignment = CellVerticalAlignment.Center;
            spd.ActiveSheet.Columns[-1].HorizontalAlignment = CellHorizontalAlignment.Left;

            spread.setColStyle(spd, -1, (int)enmSel_PATIENT_SCHDUL_Detail.SORT, clsSpread.enmSpdType.Hide);
            spread.setColStyle(spd, -1, (int)enmSel_PATIENT_SCHDUL_Detail.DRCODE, clsSpread.enmSpdType.Hide);

            //spread.setSpdFilter(spd, (int)clsComSupLbExRcpSQL.enmSel_EXAM_SPECMST_RCP03.SPECNO, AutoFilterMode.EnhancedContextMenu, true);
            //spread.setSpdFilter(spd, (int)clsComSupLbExRcpSQL.enmSel_EXAM_SPECMST_RCP03.EXAM_NAME, AutoFilterMode.EnhancedContextMenu, true);



        }
    }
}
