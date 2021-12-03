using ComBase; //기본 클래스
using ComDbB; //DB연결
using System;
using System.Collections.Generic;
using System.Data;

namespace ComSupLibB.SupLbEx 
{
    /// <summary>
    /// Class Name : ComSupLibB.SupLbEx
    /// File Name : clsLbExSQL.cs
    /// Title or Description : 진단검사의학과 SQL
    /// Author : 김홍록
    /// Create Date : 2017-05-15 
    /// Update History : 
    /// </summary>

    public class clsComSupLbExMISQL : Com.clsMethod
    {
        string SQL = string.Empty;

        public enum enmMI_STT_TYPE {
            /// <summary>액체배양</summary>
                  LIQUID
                /// <summary>고체배양</summary>
                , SOLID
                /// <summary>혐기Blood Culture</summary>
                , ANAEROBE
                /// <summary>호기 Blood Culture</summary>
                , EXPIRATION
                /// <summary>검체별 배양</summary>
                , SPECIMAN
                /// <summary>AFB Statin</summary>
                , AFB
                /// <summary>GRAM Statin</summary>
                , GRAM

                /// <summary>CLO Test</summary>
                , CLO
                /// <summary>KOH mount</summary>
                , KOH
                /// <summary>India ink</summary>
                , INDIA
                /// <summary>CRE cul</summary>
                , CRE
                /// <summary>VRE cul</summary>
                , VRE
                /// <summary>culture for SSV</summary>
                , SSV


        };

        public enum enmSel_EXAM_RESULT_MIC     { RECEIVEDATE,     SPECNO,       PANO,      AGE,      SEX,      SNAME,    IPDOPD,  DEPTCODE,     WARD,     DRCODE,       DRNM,      HCODE,   SPECCODE,     SPECNM,    RESULT,   RESULT_1, RECEIVE_DT,  RESULT_DT,   DIFF_DAY,     DAY,      HOUR,      CHK };
        public string[] sSel_EXAM_RESULT_MIC = {  "접수일자", "검체번호", "등록번호",   "나이",   "성별",     "성명",    "구분",      "과",   "병동", "의사코드",   "의사명", "핼프코드", "검체코드", "검체약어",    "결과", "결과분류", "접수일시", "결과일시", "DIFF_DAY",  "일자",   "시간" , "FILTER" };
        public int[] nSel_EXAM_RESULT_MIC    = {   nCol_DATE,  nCol_SPNO,  nCol_PANO, nCol_AGE, nCol_SEX, nCol_SNAME, nCol_SCHK,  nCol_AGE, nCol_SEX,   nCol_SEX, nCol_SNAME,   nCol_SEX,   nCol_SEX,   nCol_SEX, nCol_NAME,   nCol_SEX,  nCol_TIME,  nCol_TIME,   nCol_SEX, nCol_SEX, nCol_SEX,        5 };
       
        //public string ins_EXAM_RACK_CODE(PsmhDb pDbCon, string strCODE, string strNAME, ref int intRowAffected)
        //{
        //    string SqlErr = string.Empty;

        //    SQL = "";
        //    SQL += " INSERT INTO " + ComNum.DB_MED + "EXAM_RACK_CODE \r\n";
        //    SQL += "   (                                        \r\n";
        //    SQL += "    CODE                                    \r\n";
        //    SQL += "   ,NAME                                    \r\n";
        //    SQL += "   )                                        \r\n";
        //    SQL += "   VALUES                                   \r\n";
        //    SQL += "   (                                        \r\n";
        //    SQL += ComFunc.covSqlstr(strCODE, false);
        //    SQL += ComFunc.covSqlstr(strNAME, true);
        //    SQL += "   )                                        \r\n";

        //    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

        //    return SqlErr;
        //}
       
        public DataSet sel_EXAM_RESULT_MIC(PsmhDb pDbCon, enmMI_STT_TYPE pSttType, string strFDATE, string strTDATE)
        {
            DataSet ds = null;

            SQL = "";
            SQL += "  SELECT                                                                                                \r\n";
            SQL += "  	   TO_CHAR(A.RECEIVEDATE,'YYYY-MM-DD') AS RECEIVEDATE                                               \r\n";
            SQL += "  	 , A.SPECNO                                                                                         \r\n";
            SQL += "  	 , A.PANO                                                                                           \r\n";
            SQL += "  	 , A.AGE                                                                                            \r\n";
            SQL += "  	 , A.SEX                                                                                            \r\n";
            SQL += "  	 , A.SNAME                                                                                          \r\n";
            SQL += "  	 , A.IPDOPD                                                                                         \r\n";
            SQL += "  	 , A.DEPTCODE                                                                                       \r\n";
            SQL += "  	 , A.WARD                                                                                           \r\n";
            SQL += "  	 , A.DRCODE                                                                                         \r\n";
            SQL += "  	 , KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE) AS DRNM                                                \r\n";
            SQL += "  	 , B.HCODE                                                                                          \r\n";
            SQL += "  	 , A.SPECCODE                                                                                       \r\n";
            SQL += "  	 , KOSMOS_OCS.FC_EXAM_SPECMST_NM('14',A.SPECCODE, 'Y' ) AS SPECNM                                   \r\n";
            SQL += "  	 , B.RESULT                                                                                         \r\n";


            switch (pSttType)
            {
                case enmMI_STT_TYPE.LIQUID:

                    SQL += "	 , CASE WHEN HCODE = 'M2398' THEN 'MTB'                                                    \r\n";
                    SQL += "	        WHEN HCODE = 'M2399' THEN 'NTM'                                                    \r\n";
                    SQL += "	        WHEN HCODE = 'M2361' THEN '음성'                                                   \r\n";  
                    SQL += "	        WHEN HCODE = 'M1036' THEN '오염'                                                   \r\n";  
                    SQL += "	    END 										    AS RESULT_1                            \r\n";

                    break;
                case enmMI_STT_TYPE.SOLID:

	                 SQL += "  , CASE WHEN HCODE IN ('M2362','M2363','M2364','M2365') THEN 'MTB'                          \r\n";
	                 SQL += "         WHEN HCODE IN ('M2366','M2367','M2368','M2369') THEN 'NTM'                          \r\n";
	                 SQL += "         WHEN HCODE = 'M2361' THEN '음성'                                                    \r\n";  
	                 SQL += "         WHEN HCODE = 'M1036' THEN '오염'                                                    \r\n";  
	                 SQL += "     END 												AS RESULT_1                           \r\n";

                    break;

                case enmMI_STT_TYPE.ANAEROBE:

                    SQL += "  , CASE WHEN (HCODE LIKE 'ZZ%' AND HCODE NOT IN ('ZZ286','ZZ057','ZZ025','ZZ198')) THEN '양성'   \r\n";
	                SQL += "      WHEN HCODE IN ('ZZ286','ZZ057','ZZ025','ZZ198')      THEN '오염'                            \r\n";
	                SQL += "      WHEN HCODE IN ('M2321') THEN '음성'                                                         \r\n";
	                SQL += "  END 												    AS RESULT_1                               \r\n";

                    break;

                case enmMI_STT_TYPE.EXPIRATION:

                    SQL += "  , CASE WHEN (HCODE LIKE 'ZZ%' AND HCODE NOT IN ('ZZ286','ZZ057','ZZ025','ZZ198')) THEN '양성'   \r\n";
                    SQL += "      WHEN HCODE IN ('ZZ286','ZZ057','ZZ025','ZZ198')      THEN '오염'                            \r\n";
                    SQL += "      WHEN HCODE IN ('M2321') THEN '음성'                                                         \r\n";
                    SQL += "  END 												    AS RESULT_1                               \r\n";

                    break;
                case enmMI_STT_TYPE.SPECIMAN:
                    SQL += "  , CASE WHEN (HCODE LIKE 'ZZ%') THEN '양성'   \r\n";
                    SQL += "      WHEN HCODE IN ('M2321') THEN '음성'                                                         \r\n";
                    SQL += "  END 												    AS RESULT_1                               \r\n";

                    break;
                case enmMI_STT_TYPE.AFB:

                    SQL += "  , CASE WHEN HCODE IN ('M1031')  THEN '음성'   \r\n";
                    SQL += "         WHEN HCODE IN ('M1030','M1032','M1033','M1034','M1035') THEN '양성'                      \r\n";
                    SQL += "  END 												    AS RESULT_1                               \r\n";

                    break;

                case enmMI_STT_TYPE.GRAM:

                    SQL += "  , CASE WHEN HCODE IN ('M1053')  THEN '음성'                         \r\n";
                    SQL += "         ELSE                          '양성'                         \r\n";
                    SQL += "  END 												    AS RESULT_1                               \r\n";

                    break;

                case enmMI_STT_TYPE.SSV:

                    SQL += "  , CASE WHEN HCODE IN ('M2322')  THEN '음성'                         \r\n";
                    SQL += "         ELSE                          '양성'                         \r\n";
                    SQL += "  END 												    AS RESULT_1                               \r\n";

                    break;
                case enmMI_STT_TYPE.CRE:

                    SQL += "  , CASE WHEN HCODE IN ('M3008')  THEN '음성'                         \r\n";
                    SQL += "         WHEN HCODE IN ('M3009')  THEN '양성'                         \r\n";
                    SQL += "  END 												    AS RESULT_1                               \r\n";

                    break;
                case enmMI_STT_TYPE.VRE:

                    SQL += "  , CASE WHEN HCODE IN ('M3013')  THEN '음성'                         \r\n";
                    SQL += "         WHEN HCODE IN ('M3011','M3012')  THEN '양성'                         \r\n";
                    SQL += "  END 												    AS RESULT_1                               \r\n";

                    break;
                case enmMI_STT_TYPE.KOH:

                    SQL += "  , CASE WHEN HCODE IN ('M1081')  THEN '음성'                         \r\n";
                    SQL += "         WHEN HCODE IN ('M1082','M1083','M1084')  THEN '양성'                         \r\n";
                    SQL += "  END 												    AS RESULT_1                               \r\n";

                    break;
                case enmMI_STT_TYPE.INDIA:

                    SQL += "  , CASE WHEN HCODE IN ('M1021')  THEN '음성'                         \r\n";
                    SQL += "         WHEN HCODE IN ('M1022')  THEN '양성'                         \r\n";
                    SQL += "  END 												    AS RESULT_1                               \r\n";

                    break;
                case enmMI_STT_TYPE.CLO:

                    SQL += "  , CASE WHEN HCODE IN ('S0021')  THEN '음성'                         \r\n";
                    SQL += "         WHEN HCODE IN ('S0024')  THEN '양성'                         \r\n";
                    SQL += "         WHEN HCODE IN ('S0022')  THEN 'Trace'                         \r\n";
                    SQL += "  END 												    AS RESULT_1                               \r\n";

                    break;
                default:
                    SQL += "  '' 												    AS RESULT_1                                 \r\n";
                    break;
            }

            SQL += "     , TO_CHAR(A.RECEIVEDATE, 'YYYY-MM-DD HH24:MI') 	                                   AS RECEIVE_DT    \r\n";
            SQL += "     , TO_CHAR(B.RESULTDATE , 'YYYY-MM-DD HH24:MI')  	                                   AS RESULT_DT     \r\n";
            SQL += "     , ROUND(B.RESULTDATE - A.RECEIVEDATE,2)										       AS DIFF_DAY      \r\n"; 
            SQL += "     , TRUNC(B.RESULTDATE - A.RECEIVEDATE)    											   AS DAY           \r\n";
            SQL += "     , TRUNC(((B.RESULTDATE - A.RECEIVEDATE) - TRUNC(B.RESULTDATE - A.RECEIVEDATE)) * 24)  AS HOUR          \r\n";
            SQL += "     , 'N'                                                                                  AS CHK          \r\n";
            SQL += "  FROM KOSMOS_OCS.EXAM_SPECMST A                                                                            \r\n";
            SQL += "     , KOSMOS_OCS.EXAM_RESULTC B                                                                            \r\n";
            SQL += " WHERE 1=1                                                                                                  \r\n";
            SQL += "   AND A.RECEIVEDATE BETWEEN " + ComFunc.covSqlDate(strFDATE, false);
            SQL += "                         AND " + ComFunc.covSqlDate(strTDATE, false);
            SQL += "   AND A.SPECNO   = B.SPECNO                                                                                \r\n";

            switch (pSttType)
            {
                case enmMI_STT_TYPE.LIQUID:

                    SQL += "   AND B.SUBCODE  = 'MI363'                                                               \r\n";
                    SQL += "   AND B.RESULT	  IS NOT NULL                                                                   \r\n";

                    break;
                case enmMI_STT_TYPE.SOLID:

                    SQL += "   AND B.SUBCODE  = 'MI36'                                                               \r\n";
                    SQL += "   AND B.RESULT	  IS NOT NULL                                                                   \r\n";
                    break;

                case enmMI_STT_TYPE.ANAEROBE:

                    SQL += "   AND B.SUBCODE  = 'MI32C'                                                               \r\n";
                    SQL += "   AND B.RESULT	  IS NOT NULL                                                                   \r\n";
                    break;

                case enmMI_STT_TYPE.EXPIRATION:

                    SQL += "   AND B.SUBCODE  = 'MI32B'                                                               \r\n";
                    SQL += "   AND B.RESULT	  IS NOT NULL                                                                   \r\n";
                    break;

                case enmMI_STT_TYPE.SPECIMAN:
                    SQL += "   AND B.SUBCODE  = 'MI32'                                                               \r\n";
                    SQL += "   AND B.RESULT	  IS NOT NULL                                                                   \r\n";
                    break;

                case enmMI_STT_TYPE.AFB:
                    SQL += "   AND B.SUBCODE  IN ( 'MI03','MI031')                                                   \r\n";
                    SQL += "   AND B.RESULT	  IS NOT NULL                                                                   \r\n";
                    break;

                case enmMI_STT_TYPE.GRAM:
                    SQL += "   AND B.SUBCODE  IN ( 'MI04')                                                           \r\n";
                    //2018.04.05.김홍록: GramStain은 헤더의 결과만 읽는다.
                    //                    SQL += "   AND B.RESULT	  IS NOT NULL                                                                   \r\n";
                    break;

                case enmMI_STT_TYPE.SSV:
                    SQL += "   AND B.SUBCODE  IN ( 'MI323')                                                   \r\n";
                    SQL += "   AND B.RESULT	  IS NOT NULL                                                                   \r\n";
                    break;
                case enmMI_STT_TYPE.CRE:
                    SQL += "   AND B.SUBCODE  IN ( 'MI32H1')                                                   \r\n";
                    SQL += "   AND B.RESULT	  IS NOT NULL                                                                   \r\n";
                    break;
                case enmMI_STT_TYPE.VRE:
                    SQL += "   AND B.SUBCODE  IN ( 'MI32V')                                                   \r\n";
                    SQL += "   AND B.RESULT	  IS NOT NULL                                                                   \r\n";
                    break;
                case enmMI_STT_TYPE.KOH:
                    SQL += "   AND B.SUBCODE  IN ( 'MI05')                                                   \r\n";
                    SQL += "   AND B.RESULT	  IS NOT NULL                                                                   \r\n";
                    break;
                case enmMI_STT_TYPE.INDIA:
                    SQL += "   AND B.SUBCODE  IN ( 'MI02')                                                   \r\n";
                    SQL += "   AND B.RESULT	  IS NOT NULL                                                                   \r\n";
                    break;
                case enmMI_STT_TYPE.CLO:
                    SQL += "   AND B.SUBCODE  IN ( 'MI11')                                                   \r\n";
                    SQL += "   AND B.RESULT	  IS NOT NULL                                                                   \r\n";
                    break;
                default:
                    SQL += "   AND B.SUBCODE  = ''                                                                   \r\n";
                    break;
            }

            SQL += "   AND B.STATUS   = 'V'                                                                         \r\n";
            
            SQL += " ORDER BY A.RECEIVEDATE                                                                         \r\n";

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
