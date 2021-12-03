using ComBase; //기본 클래스
using ComDbB; //DB연결
using System;
using System.Collections.Generic;
using System.Data;

namespace ComSupLibB.SupLbEx
{
    /// <summary>
    /// Class Name : ComSupLibB.SupLbEx
    /// File Name : clsComSupLbExCODESQL.cs
    /// Title or Description : 진단검사의학과 코드관리 SQL
    /// Author : 김홍록
    /// Create Date : 2018-02-25
    /// Update History :  
    /// </summary>
      
    public class clsComSupLbExCODESQL : Com.clsMethod
    {
        string SQL = string.Empty;

        public enum enmEXAM_MASTER_SUB_GUBUN {MASTER = 01, COMMENT = 17, HELP = 18, REF = 41, REMARK = 51, EXAM_INFO = 61};

        public enum enmSel_EXAM_SPECCODE_51       {       CODE,       NAME };
        public string[] sSel_EXAM_SPECCODE_51   = { "수가코드", "수가명칭" };
        public int[] nSel_EXAM_SPECCODE_51      = {  nCol_PANO,  nCol_JUSO };


        public string ins_EXAM_SPECCODE_51(PsmhDb pDbCon, string strCODE, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += "INSERT INTO KOSMOS_OCS.EXAM_SPECODE( GUBUN, CODE, YNAME, INPS, INPT_DT) VALUES ( \r\n";
            SQL += " 51 \r\n";
            SQL += ComFunc.covSqlstr("000000", true);
            SQL += ComFunc.covSqlstr(strCODE, true);
            SQL += ComFunc.covSqlstr(clsType.User.IdNumber, true);
            SQL += ", SYSDATE)";


            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            if (string.IsNullOrEmpty(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            }

            return SqlErr;
        }

        public string del_EXAM_SPECCODE_51(PsmhDb pDbCon, string strCODE, ref int intRowAffected)
        {
            string SqlErr = "";

            SQL = "";
            SQL += " DELETE FROM KOSMOS_OCS.EXAM_SPECODE \r\n";
            SQL += "  WHERE 1=1                          \r\n";
            SQL += "    AND GUBUN = '51'                 \r\n";

            if (string.IsNullOrEmpty(strCODE) == false)
            {
                SQL += "    AND YNAME = " + ComFunc.covSqlstr(strCODE, false);
            }
            

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            if (string.IsNullOrEmpty(SqlErr) == false)
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
            }

            return SqlErr;
        }



        public DataSet sel_EXAM_SPECCODE_51(PsmhDb pDbCon)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT A.YNAME		AS CODE             \r\n";
            SQL += "       , B.SUNAMEK      AS NAME             \r\n";            
            SQL += "    FROM  KOSMOS_OCS.EXAM_SPECODE A         \r\n";
            SQL += "        , KOSMOS_PMPA.BAS_SUN 	  B         \r\n";
            SQL += "   WHERE 1=1                                \r\n";
            SQL += "     AND A.GUBUN = '51'                     \r\n";
            SQL += "     AND TRIM(A.YNAME) = TRIM(B.SUNEXT)     \r\n";
            SQL += "   ORDER BY A.YNAME                         \r\n";

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


        public DataSet sel_EXAM_MASTER_EXCEL(PsmhDb pDbCon, enmEXAM_MASTER_SUB_GUBUN pGUBUN)
        {
            DataSet ds = null;

            SQL = "";

            SQL += "  SELECT                                                               \r\n";
            SQL += "  	     MASTERCODE                                     AS 검사코드    \r\n";

            switch (pGUBUN)
            {
                case enmEXAM_MASTER_SUB_GUBUN.COMMENT:
                    SQL += "  	     , NORMAL                                         AS 채취코드   \r\n";
                    SQL += "  	     , KOSMOS_OCS.FC_EXAM_SPECMST_NM('17',NORMAL,'N') AS 채취명     \r\n";

                    break;
                case enmEXAM_MASTER_SUB_GUBUN.HELP:
                    SQL += "  	     , NORMAL                                         AS HELP코드   \r\n";
                    SQL += "  	     , KOSMOS_OCS.FC_EXAM_SPECMST_NM('18',NORMAL,'N') AS HELP       \r\n";

                    break;
                case enmEXAM_MASTER_SUB_GUBUN.REF:
                    SQL += "  	     , SEX	                                          AS SEX        \r\n";
                    SQL += "  	     , AGEFROM										  AS 시작나이   \r\n";
                    SQL += "  	     , AGETO										  AS 시작종료   \r\n";
                    SQL += "  	     , REFVALFROM									  AS 범위시작   \r\n";
                    SQL += "  	     , REFVALTO										  AS 범위종료   \r\n";
                    break;
                case enmEXAM_MASTER_SUB_GUBUN.REMARK:
                    SQL += "  	     , NORMAL                                         AS 결과비고   \r\n";

                    break;
                case enmEXAM_MASTER_SUB_GUBUN.EXAM_INFO:
                    SQL += "  	     , NORMAL                                         AS 채취코드   \r\n";
                    SQL += "  	     , NORMAL                                         AS 검사비고   \r\n";

                    break;
                default:
                    break;
            }

            SQL += "   FROM KOSMOS_OCS.EXAM_MASTER_SUB                                     \r\n";
            SQL += "  WHERE 1=1                                                            \r\n";
            SQL += "    AND GUBUN = " + ComFunc.covSqlstr(Convert.ToString((int)pGUBUN),false);
            SQL += "  ORDER BY MASTERCODE,NORMAL, SORT                                     \r\n";

                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
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

        public DataSet sel_EXAM_MASTER_SUB(PsmhDb pDbCon)
        {
            DataSet ds = null;

            SQL = "";

            SQL += "  SELECT                                                                                        \r\n";
            SQL += "  	     A.MASTERCODE 		                                    AS 코드		                -- Master Key                                          \r\n";
            SQL += "  	   , A.EXAMNAME 		                                    AS 통상명칭	                -- 검사명:통상명칭                                     \r\n";
            SQL += "  	   , A.EXAMFNAME 		                                    AS Full_Name                -- 검사명:Full Name                                    \r\n";
            SQL += "  	   , A.EXAMYNAME 		                                    AS 연보명	                -- 검사명:연보에 실린 이름                             \r\n";
            SQL += "  	   , A.WSCODE1 		                                        AS WS1		                -- Work Station 1                                      \r\n";
            SQL += "  	   , A.WSCODE1POS 		                                    AS WS1Pos	                -- 검사 항목 결과 위치                                 \r\n";
            SQL += "  	   , A.WSCODE2 		                                        AS WS2		                -- Work Station 2                                      \r\n";
            SQL += "  	   , A.WSCODE2POS 		                                    AS WS2Pos	                -- 검사 항목 결과 위치                                 \r\n";
            SQL += "  	   , A.WSCODE3 		                                        AS WS3		                -- Work Station 3                                      \r\n";
            SQL += "  	   , A.WSCODE3POS 		                                    AS WS3Pos	                -- 검사 항목 결과 위치                                 \r\n";
            SQL += "  	   , A.WSCODE4 		                                        AS WS4		                -- Work Station 4                                      \r\n";
            SQL += "  	   , A.WSCODE4POS 		                                    AS WS4Pos	                -- 검사 항목 결과 위치                                 \r\n";
            SQL += "  	   , A.WSCODE5 		                                        AS WS5		                -- Work Station 5                                      \r\n";
            SQL += "  	   , A.WSCODE5POS 		                                    AS WS5Pos	                -- 검사 항목 결과 위치                                 \r\n";
            SQL += "  	   , A.TURNTIME1 		                                    AS TurnTime	                -- Turn Around Time(일)                                \r\n";
            SQL += "  	   , A.TURNTIME2 		                                    AS TurnTime	                -- Turn Around Time(분)                                \r\n";
            SQL += "  	   , A.EXAMWEEK 		                                    AS 검사요일	                -- 검사요일(일월화수목금토) (Y:검사, N:검사안함)       \r\n";
            SQL += "  	   , A.TUBECODE 		                                    AS 용기코드	                -- 용기종류코드                                        \r\n";
            SQL += "  	   , KOSMOS_OCS.FC_EXAM_SPECMST_NM('15',A.TUBECODE,'N')     AS 용기명                                                                          \r\n";
            SQL += "  	   , A.SPECCODE 		                                    AS 검체코드	                -- 검체종류코드                                        \r\n";
            SQL += "  	   , KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',A.SPECCODE,'N')     AS 검체명                                                                          \r\n";
            SQL += "  	   , A.VOLUMECODE 		                                    AS 채혈량코드	    -- 채혈량코드                                                      \r\n";
            SQL += "  	   , KOSMOS_OCS.FC_EXAM_SPECMST_NM('16',A.VOLUMECODE,'N')   AS 채혈량                                                                          \r\n";
            SQL += "  	   , A.EQUCODE1 		                                    AS 장비코드1	    -- 장비코드                                                        \r\n";
            SQL += "  	   , KOSMOS_OCS.FC_EXAM_SPECMST_NM('13',A.EQUCODE1,'N')     AS 장비명1                                                                         \r\n";
            SQL += "  	   , A.EQUCODE2 		                                    AS 장비코드2	    -- 장비코드                                                        \r\n";
            SQL += "  	   , KOSMOS_OCS.FC_EXAM_SPECMST_NM('13',A.EQUCODE2,'N')     AS 장비명2       \r\n";
            SQL += "  	   , A.EQUCODE3 		                                    AS 장비코드3	    -- 장비코드                              \r\n";
            SQL += "  	   , KOSMOS_OCS.FC_EXAM_SPECMST_NM('13',A.EQUCODE3,'N')     AS 장비명3       \r\n";
            SQL += "  	   , A.EQUCODE4 		                                    AS 장비코드4	    -- 장비코드                              \r\n";
            SQL += "  	   , KOSMOS_OCS.FC_EXAM_SPECMST_NM('13',A.EQUCODE4,'N')     AS 장비명4                                                                       \r\n";
            SQL += "  	   , A.UNITCODE 		                                    AS 결과단위코드             -- 결과단위코드                                      \r\n";
            SQL += "  	   , KOSMOS_OCS.FC_EXAM_SPECMST_NM('20',A.UNITCODE,'N')     AS 결과단위                                                                      \r\n";
            SQL += "  	   , A.DATATYPE 		                                    AS DataType		            -- Data Type                                         \r\n";
            SQL += "  	   , A.DATALENGTH 		                                    AS DataLength	            -- Data Length                                       \r\n";
            SQL += "  	   , A.RESULTIN 		                                    AS 결과입력유무	            -- 결과입력 유무(1:결과 입력 안함, 0:결과 입력 함)   \r\n";
            SQL += "  	   , A.PANICFROM 		                                    AS Panic_From	            -- Panic Value(From)                                 \r\n";
            SQL += "  	   , A.PANICTO 		                                        AS Panic_To		            -- Panic Value(To)                                   \r\n";
            SQL += "  	   , A.DELTAM 			                                    AS Delta_M		            -- Delta Value(-)                                    \r\n";
            SQL += "  	   , A.DELTAP 			                                    AS Delta_P		            -- Delta value(+)                                    \r\n";
            SQL += "  	   , A.DDDPRDRP 		                                    AS DDDPRDRP		            -- DD, DP, RD, RP                                    \r\n";
            SQL += "  	   , A.BCODENAME 		                                    AS 바코드명		            -- Barcode 출력명                                    \r\n";
            SQL += "  	   , A.BCODEPRINT 		                                    AS 바코드인쇄장수-- Barcode 인쇄장수                                    \r\n";
            SQL += "  	   , A.KEYPAD 			                                    AS KeyPad		-- KeyPad Position(Diff용)                              \r\n";
            SQL += "  	   , A.SERIES 			                                    AS 연속검사		-- 연속검사(1.연속검사 0.연속검사아님)                  \r\n";
            SQL += "  	   , A.PENDING 		                                        AS Pending_Check-- Pending Checking 여부(1:Check, 0:Check 안함)         \r\n";
            SQL += "  	   , A.ENDDATE 		                                        AS 삭제일		-- 사용종료일(삭제일자)                                 \r\n";
            SQL += "  	   , A.MODIFYDATE 	                                        AS Sub코드사용	-- 변경일자                                             \r\n";
            SQL += "  	   , A.MODIFYID 	                                        AS 모코드를Sub코드로가짐-- 변경자사번                                   \r\n";
            SQL += "  	   , A.SUB 			                                        AS Sub코드			-- SUB코드여부(1:Sub, 0:Mother) 화면에는 선택숨김   \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_MASTER A                                                                 \r\n";
            SQL += "  ORDER BY MASTERCODE                                                                           \r\n";


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
