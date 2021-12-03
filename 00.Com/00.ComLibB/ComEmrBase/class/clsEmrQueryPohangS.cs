using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using ComDbB; //DB연결
using Oracle.ManagedDataAccess.Client;

namespace ComEmrBase
{
    public class clsEmrQueryPohangS
    {
        /// <summary>
        /// //modMtsPublic : START_TUYAK
        /// </summary>
        /// <returns></returns>
        public static bool START_TUYAK(PsmhDb pDbCon)
        {
            bool rtnVal = false;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = "  SELECT CODE";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE";
            SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'EMR_투약기록지분리시행'";
            SQL = SQL + ComNum.VBLF + "      AND CODE = 'START'";
            SQL = SQL + ComNum.VBLF + "      AND NAME = 'Y'";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            dt.Dispose();
            dt = null;
            rtnVal = true;

            return rtnVal;
        }

        public static bool NEXTDATE(PsmhDb pDbCon, string pBDate, string pPTNO)
        {
            bool rtnVal = false;
            return rtnVal;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            ComFunc pComFunc = new ComFunc();

            SQL = " SELECT PANO FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + pPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + pComFunc.DATE_ADD(pDbCon, VB.Format(pBDate, "0000-00-00"), 1) + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = 'ER' ";
            pComFunc = null;

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            dt.Dispose();
            dt = null;
            rtnVal = true;

            return rtnVal;
        }


        /// <summary>
        /// modMtsPublic : GetIPDCancel
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPTNO"></param>
        /// <param name="ArgInDate"></param>
        /// <param name="argOUTDATE"></param>
        /// <param name="ArgDeptCode"></param>
        /// <returns></returns>
        public static string GetIPDCancel(PsmhDb pDbCon, string argPTNO, string ArgInDate, string argOUTDATE, string ArgDeptCode)
        {
            string rtnVal = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT PANO ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "   AND INDATE >= TO_DATE('" + ArgInDate + " 00:00','YYYY-MM-DD HH24:MI') ";
            SQL = SQL + ComNum.VBLF + "   AND INDATE <= TO_DATE('" + ArgInDate + " 23:59','YYYY-MM-DD HH24:MI') ";
            SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + ArgDeptCode + "' ";
            SQL = SQL + ComNum.VBLF + "   AND OUTDATE = TO_DATE('" + argOUTDATE + "','YYYY-MM-DD' )";
            SQL = SQL + ComNum.VBLF + "   AND GBSTS = '9' ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            dt.Dispose();
            dt = null;

            rtnVal = "입원취소";

            return rtnVal;
        }

        /// <summary>
        /// modMtsPublic : READ_서류재발급
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPTNO"></param>
        /// <param name="ArgInDate"></param>
        /// <param name="ArgDeptCode"></param>
        /// <returns></returns>
        public static bool READ_DOCREPRINT(PsmhDb pDbCon, string argPTNO, string ArgInDate, string ArgDeptCode)
        {
            bool rtnVal = false;
            //return rtnVal;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;

            SQL = " SELECT PANO ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU_REPRINT ";
            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + ArgInDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + ArgDeptCode + "' ";

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            
            if (reader.HasRows)
            {
                rtnVal = true;
            }

            reader.Dispose();

            return rtnVal;
        }

        /// <summary>
        /// modMtsPublic : READ_사본발급 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPTNO"></param>
        /// <param name="ArgInDate"></param>
        /// <param name="ArgDeptCode"></param>
        /// <returns></returns>
        public static bool READ_CHARTCOPY(PsmhDb pDbCon, string argPTNO, string ArgInDate, string ArgDeptCode)
        {
            bool rtnVal = false;
            OracleDataReader reader = null;

            string SQL = " SELECT PANO ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "OPD_TELRESV ";
            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "   AND RDATE = TO_DATE('" + ArgInDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + ArgDeptCode + "' ";
            SQL = SQL + ComNum.VBLF + "   AND GBCOPY = 'Y' ";

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (reader.HasRows)
            {
                rtnVal = true;
            }

            reader.Dispose();
            
            return rtnVal;
        }

        //modMtsPublic : READ_재발급내역
        public static string READ_DOCREPRINTHIS(PsmhDb pDbCon, Form mBaseFrm, string argPTNO, string ArgInDate, string ArgDeptCode, string argGBN = "")
        {
            string rtnVal = "";
            string str = "";
            string str2 = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT SEQDATE, MCCLASS, SINNAME, SINSAYU, BIGO ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_MCCERTIFI_WONMU_REPRINT ";
            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + ArgInDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + ArgDeptCode + "' ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return rtnVal;
            }

            switch (dt.Rows[0]["MCCLASS"].ToString().Trim())
            {
                case "00":
                    str2 = "진료사실증명서";
                    break;
                case "01":
                    str2 = "진단서";
                    break;
                case "08":
                    str2 = "소견서";
                    break;
                case "18":
                    str2 = "진료의뢰서";
                    break;
                case "26":
                    str2 = "의료급여의뢰서";
                    break;
                case "27":
                    str2 = "응급환자진료의뢰서";
                    break;
                case "02":
                    str2 = "상해진단서";
                    break;
                case "03":
                    str2 = "병사용진단서";
                    break;
                case "05":
                    str2 = "사망진단서";
                    break;
                case "19":
                    str2 = "장애인증명서";
                    break;
                case "20":
                    str2 = "장애진단서";
                    break;
                default:
                    break;
            }
            
            if (argGBN == "1")
            {
                str = "서류재발급 : " + str2 + "(" + dt.Rows[0]["SEQDATE"].ToString().Trim() + ")";
            }
            else
            {
                str = "★신청자서류 : " + str2 + "(" + dt.Rows[0]["SEQDATE"].ToString().Trim() + ")" + ComNum.VBLF;
                str = str + "★신청자명 : " + dt.Rows[0]["SINNAME"].ToString().Trim() + ComNum.VBLF;
                str = str + "★신청사유  " + ComNum.VBLF + " => " + dt.Rows[0]["SINSAYU"].ToString().Trim();
                if (dt.Rows[0]["BIGO"].ToString().Trim() != "")
                {
                    str = str+ ComNum.VBLF + " ★비고  "+ComNum.VBLF+" => "+dt.Rows[0]["BIGO"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;

            if (argGBN == "1")
            {
                rtnVal = str;
            }
            else
            {
                if (str.Trim().Length > 0)
                {
                    ComFunc.MsgBoxEx(mBaseFrm, str, "재발급 상세내역");
                }
            }

            return rtnVal;
        }

        //modMtsPublic : LIST_HIDDEN
        public static bool LIST_HIDDEN(PsmhDb pDbCon, string argPTNO, string argMEDFRDATE, string argMEDDEPTCD, string argMEDDRCD, string ArgInOutCls)
        {
            return false;
        }


        /// <summary>
        /// READ_ER_IPWON
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPTNO"></param>
        /// <param name="argMEDFRDATE"></param>
        /// <returns></returns>
        public static bool READ_ER_IPWON(PsmhDb pDbCon, string argPTNO, string argMEDFRDATE)
        {
            bool rtnVal = false;
            OracleDataReader reader = null;

            string SQL = " SELECT ROWID ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
            SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + argPTNO + "' ";
            SQL = SQL + ComNum.VBLF + "   AND INDATE >= TO_DATE('" + argMEDFRDATE + " 00:00','YYYY-MM-DD HH24:MI') ";
            SQL = SQL + ComNum.VBLF + "   AND INDATE <= TO_DATE('" + argMEDFRDATE + " 23:59','YYYY-MM-DD HH24:MI') ";
            SQL = SQL + ComNum.VBLF + "   AND GBSTS NOT IN ('9')";
            SQL = SQL + ComNum.VBLF + "   AND AMSET7 IN ('3','4','5')";

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (reader.HasRows == false)
            {
                reader.Dispose();
                return rtnVal;
            }
            reader.Dispose();
            rtnVal = true;

            return rtnVal;
        }

        //modMtsEmr : ReadOptionDate
        public static string ReadOptionDate(string argGrade, string ArgIO, string ArgDeptCode, string argUSEID, string argDATE)
        {
            string rtnVal = string.Empty;

            switch (argUSEID)
            {

                case "45924":
                case "22536":
                case "36522":
                case "44892":
                    argDATE = argDATE.Replace("-", "");
                    rtnVal = VB.Left(argDATE, 4) + "-" +
                        VB.Mid(argDATE, 5, 2) + "-" +
                        VB.Right(argDATE, 2);
                    return rtnVal;
            }

            if (argGrade != "SIMSA")
            {
                return argDATE;
            }

            OracleDataReader reader = null;
            string SQL = " SELECT NAL ";

            try
            {
                SQL = string.Empty;
                SQL = " SELECT NAL ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMR_OPTION_SETDATE ";
                SQL = SQL + ComNum.VBLF + " WHERE IO = '" + ArgIO + "' ";
                SQL = SQL + ComNum.VBLF + "   AND USEID = " + argUSEID;
                if (ArgIO != "I")
                {
                    SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + ArgDeptCode + "' ";
                }
                SQL = SQL + ComNum.VBLF + "   AND USED = '1' ";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                    ComFunc.MsgBox(sqlErr);
                    return rtnVal;
                }

                if (reader.HasRows && reader.Read())
                {
                    if (reader.GetValue(0).ToString().Trim().Length == 0 ||
                       VB.IsNumeric(reader.GetValue(0).ToString().Trim()) == false)
                    {
                        rtnVal = argDATE;
                    }
                    else
                    {
                        rtnVal = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).
                        AddDays(VB.Val(reader.GetValue(0).ToString().Trim()) * -1).ToShortDateString();
                    }
                }
                else
                {
                    rtnVal = argDATE;
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// frmTextEmrHistory : CHECK_COMPLETE
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPTNO"></param>
        /// <param name="argMEDFRDATE"></param>
        /// <returns></returns>
        public static bool CHECK_COMPLETE(PsmhDb pDbCon, string argPTNO, string argMEDFRDATE, string OldGB = "1")
        {
            bool rtnVal = false;
            OracleDataReader reader = null;

            //string SQL = " SELECT EMRNO";
            //SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.EMRXML_COMPLETE";
            //SQL = SQL + ComNum.VBLF + " WHERE PTNO = '" + argPTNO + "' ";
            //SQL = SQL + ComNum.VBLF + "   AND MEDFRDATE = '" + argMEDFRDATE + "' ";

            string SQL = " SELECT A.EMRNO";
            if (OldGB.Equals("1"))
            {
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.EMRXMLMST A";
            }
            else
            {
                SQL += ComNum.VBLF + " FROM KOSMOS_EMR.AEMRCHARTMST A";
            }

            SQL += ComNum.VBLF + " INNER JOIN KOSMOS_EMR.EMRXML_COMPLETE B";
            SQL += ComNum.VBLF + "    ON A.EMRNO = B.EMRNO";
            if (OldGB.Equals("1") == false)
            {
                SQL += ComNum.VBLF + "   AND A.EMRNOHIS = B.EMRNOHIS";
            }
            SQL += ComNum.VBLF + "WHERE A.MEDFRDATE = '" + argMEDFRDATE + "'";
            SQL += ComNum.VBLF + "  AND A.PTNO = '" + argPTNO + "'";
            SQL += ComNum.VBLF + "  AND A.FORMNO = 1647";

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (reader.HasRows == false)
            {
                reader.Dispose();
                return rtnVal;
            }

            reader.Dispose();
            rtnVal = true;

            return rtnVal;
        }

        /// <summary>
        /// modMtsPublic : READ_FORM_BOLD
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argFORMNO"></param>
        /// <returns></returns>
        public static bool READ_FORM_BOLD(PsmhDb pDbCon, string argFORMNO)
        {
            bool rtnVal = false;
            OracleDataReader reader = null;

            string SQL = " SELECT BOLD_YN ";
            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRFORM ";
            SQL = SQL + ComNum.VBLF + " WHERE FORMNO = " + argFORMNO;

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (reader.HasRows && reader.Read() && reader.GetValue(0).ToString().Trim().Equals("Y"))
            {
                rtnVal = true;
            }

            reader.Dispose();

            return rtnVal;
        }

        /// <summary>
        /// modMtsPublic : READ_FORM_BOLD
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argFORMNO"></param>
        /// <returns></returns>
        public static bool READ_FORM_BOLD_RED(PsmhDb pDbCon, string argFORMNO)
        {
            bool rtnVal = false;
            OracleDataReader reader = null;

            string SQL = "  SELECT CODE ";
            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE";
            SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'EMR_ER_대면기록지'";
            SQL = SQL + ComNum.VBLF + "   AND CODE = '" + argFORMNO + "' ";
            SQL = SQL + ComNum.VBLF + "   AND DELDATE IS NULL ";

            string SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
           
            if (reader.HasRows)
            {
                rtnVal = true;
            }

            reader.Dispose();

            return rtnVal;
        }

        /// <summary>
        /// frmTextEmrHistory : IsDrOrder
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtNo"></param>
        /// <param name="strInOutCls"></param>
        /// <param name="strMedFrDate"></param>
        /// <param name="strMedEndDate"></param>
        /// <param name="strMedDeptCd"></param>
        /// <param name="pUserGrade"></param>
        /// <returns></returns>
        public static bool IsDrOrder(PsmhDb pDbCon, string strPtNo, string strInOutCls, string strMedFrDate, string strMedEndDate , string strMedDeptCd, string pUserGrade)
        {
            bool rtnVal = false;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;

            if (strInOutCls == "I")
            {
                SQL = "  SELECT /*+ INDEX (OCS_IORDER INXOCS_IORDER1) */ORDERNO";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_IORDER";
                SQL = SQL + ComNum.VBLF + "      WHERE PTNO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "          AND BDATE >= TO_DATE('2009-06-01','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "          AND BDATE >= TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD') ";
                if (strMedEndDate != "")
                {
                    SQL = SQL + ComNum.VBLF + "          AND BDATE <= TO_DATE('" + ComFunc.FormatStrToDate(strMedEndDate, "D") + "','YYYY-MM-DD') ";
                }
                SQL = SQL + ComNum.VBLF + "              AND (OrderSite IS NULL OR ORDERSITE <> 'ERO')";
                SQL = SQL + ComNum.VBLF + "              AND (GBIOE NOT IN ('E','EI') OR GBIOE IS NULL OR GBIOE = ' ')";
                SQL = SQL + ComNum.VBLF + "          AND ROWNUM = 1";
            }
            else
            {
                SQL = "  SELECT ORDERNO ";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_OORDER";
                SQL = SQL + ComNum.VBLF + "      WHERE PTNO = '" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "          AND BDATE >= TO_DATE('2009-06-01','YYYY-MM-DD')  ";
                SQL = SQL + ComNum.VBLF + "          AND BDATE = TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD') ";
                if (strMedDeptCd != "")
                {
                    if (strMedDeptCd == "RA")
                    {
                        SQL = SQL + ComNum.VBLF + "          AND DEPTCODE = 'MD'";
                        SQL = SQL + ComNum.VBLF + "          AND DRCODE IN ( '1107','1125')";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "          AND DEPTCODE = '" + strMedDeptCd + "'";
                        SQL = SQL + ComNum.VBLF + "          AND DRCODE NOT IN ( '1107','1125')";
                    }
                }

                if (pUserGrade == "SIMSA")
                {
                    SQL = SQL + ComNum.VBLF + " AND (GBSUNAP = '1' OR SUCODE IN ('$$11','$$21'))";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "          AND GBSUNAP = '1' ";
                }
                SQL = SQL + ComNum.VBLF + "          AND ROWNUM = 1";
            }

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (reader.HasRows == false)
            {
                reader.Dispose();
                return rtnVal;
            }
            rtnVal = true;
            reader.Dispose();

            return rtnVal;
        }

        /// <summary>
        /// frmTextEmrHistory : IsERDrOrder
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtNo"></param>
        /// <param name="strInOutCls"></param>
        /// <param name="strMedFrDate"></param>
        /// <param name="strMedEndDate"></param>
        /// <param name="strMedDeptCd"></param>
        /// <param name="pUserGrade"></param>
        /// <returns></returns>
        public static bool IsERDrOrder(PsmhDb pDbCon, string strPtNo, string strInOutCls, string strMedFrDate, string strMedEndDate, string strMedDeptCd, string pUserGrade)
        {
            bool rtnVal = false;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            OracleDataReader reader = null;

            SQL = "  SELECT /*+ INDEX (OCS_IORDER INXOCS_IORDER1) */ORDERNO";
            SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_IORDER";
            SQL = SQL + ComNum.VBLF + "      WHERE PTNO = '" + strPtNo + "' ";
            SQL = SQL + ComNum.VBLF + "          AND BDATE >= TO_DATE('2009-06-01','YYYY-MM-DD')  ";
            SQL = SQL + ComNum.VBLF + "          AND BDATE >= TO_DATE('" + ComFunc.FormatStrToDate(strMedFrDate, "D") + "','YYYY-MM-DD') ";
            if (strMedEndDate != "")
            {
                ComFunc pComFunc = new ComFunc();
                SQL = SQL + ComNum.VBLF + "          AND BDATE <= TO_DATE('" + pComFunc.DATE_ADD(pDbCon, ComFunc.FormatStrToDate(strMedEndDate, "D"), 1) + "','YYYY-MM-DD') ";
                pComFunc = null;
            }
            SQL = SQL + ComNum.VBLF + "          AND (OrderSite  In ('ERO')   OR  GBIOE IN ('E','EI') )";
            SQL = SQL + ComNum.VBLF + "          AND ROWNUM = 1";

            SqlErr = clsDB.GetAdoRs(ref reader, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }
            if (reader.HasRows == false)
            {
                reader.Dispose();
                return rtnVal;
            }
            rtnVal = true;
            reader.Dispose();
            return rtnVal;
        }

        /// <summary>
        /// 병동코드조회
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <returns></returns>
        public static DataTable READ_WARD_LIST(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT A.MACROGB AS WARDCD, MAX(B.NAME) AS WARDNAME ";
            SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.EMRMACROETC A ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.EMR_CLINICT B ";
            SQL = SQL + ComNum.VBLF + "    ON A.MACROGB = B.CLINCODE ";
            SQL = SQL + ComNum.VBLF + "    GROUP BY A.MACROGB ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }


        /// <summary>
        /// '2019-05-16
        /// '근무스케쥴에 따른 간호기록 작성 제한 => 근무시간에만 작성 가능하도록 보완
        /// '대상근무코드는 D1,E1,N1,OFF,휴가 나머지는 작성제한 없음
        /// '퇴근시간 + 15분
        /// 
        /// '반환값이 TRUE면 챠트 작성 불가능
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <returns></returns>
        public static bool CHECK_NUR_SCHEDULE(PsmhDb pDbCon)
        {
            if (clsPublic.GstrNurLogInFlag_Ward == "OK")
            {
                return false;
            }

            string strSchedule = string.Empty;
            string strWARDCODE = string.Empty;

            string SQL = string.Empty;
            string SqlErr = string.Empty; //에러문 받는 변수
            DataTable dt = null;

            SQL = " SELECT CODE ";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_BCODE ";
            SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'NUR_챠팅근무시간제한NEW' ";
            SQL = SQL + ComNum.VBLF + "   AND CODE = 'USE' ";
            SQL = SQL + ComNum.VBLF + "   AND NAME = 'N' ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }

            if (dt.Rows.Count > 0)
            {
                dt.Dispose();
                return false;
            }
            dt.Dispose();
            dt = null;

            ComFunc.ReadSysDate(pDbCon);

            #region 당직자 챠트 설정 체크
            SQL = " SELECT BASCD";
            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BASCD";
            SQL = SQL + ComNum.VBLF + "WHERE GRPCDB = '간호EMR 관리'";
            SQL = SQL + ComNum.VBLF + "  AND GRPCD  = '예외 사번'";
            SQL = SQL + ComNum.VBLF + "  AND BASCD = '" + clsType.User.Sabun + "'";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }

            if (dt.Rows.Count > 0)
            {
                dt.Dispose();
                return false;
            }

            dt.Dispose();
            #endregion

            SQL = " SELECT WARDCODE, SCHEDULE";
            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_SCHEDULE1";
            SQL = SQL + ComNum.VBLF + " WHERE YYMM  = TO_CHAR(SYSDATE,'YYYYMM')";
            SQL = SQL + ComNum.VBLF + "   AND SABUN = '" + clsType.User.Sabun + "'";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return false;
            }

            if (dt.Rows.Count > 0)
            {
                strSchedule = dt.Rows[0]["SCHEDULE"].ToString();
                strWARDCODE = dt.Rows[0]["WARDCODE"].ToString().Trim();
            }

            dt.Dispose();

            if (strSchedule == "")        //'스케쥴이 없으면 간호사 아니라고 판단, 해당 로직 패스.
            {
                return false;
            }

            //'2019-05-23 병동만 조건 적용함
            switch (strWARDCODE.Trim())
            {
                case "33":
                case "35":
                case "40":
                case "50":
                case "53":
                case "55":
                case "60":
                case "63":
                case "65":
                case "70":
                case "73":
                case "75":
                case "80":
                case "83":
                    break;
                default:
                    return false;                    
            }

            string strDAY = VB.Right(clsPublic.GstrSysDate, 2);
            string strDAY2 = VB.Right(Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-1).ToShortDateString(), 2); //어제 일자
            string strDuty = string.Empty;
            string strDuty2 = string.Empty;

            if (strDAY.Equals("01"))
            {
                SQL = " SELECT WARDCODE, SCHEDULE";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_SCHEDULE1";
                SQL = SQL + ComNum.VBLF + " WHERE YYMM  = TO_CHAR(SYSDATE - 1,'YYYYMM')";
                SQL = SQL + ComNum.VBLF + "   AND SABUN = '" + clsType.User.Sabun + "'";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }

                string strSchedule2 = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    strSchedule2 = dt.Rows[0]["SCHEDULE"].ToString().Trim();
                }

                dt.Dispose();

                strDuty = ComFunc.MidH(strSchedule, (int)VB.Val(strDAY) * 4 - 3, 4).Trim(); //당일 근무스케쥴
                strDuty2 = ComFunc.MidH(strSchedule2, (int)VB.Val(strDAY2) * 4 - 3, 4).Trim();
            }
            else
            {
                strDuty = ComFunc.MidH(strSchedule, (int)VB.Val(strDAY) * 4 - 3, 4).Trim(); //당일 근무스케쥴
                strDuty2 = ComFunc.MidH(strSchedule, (int)VB.Val(strDAY2) * 4 - 3, 4).Trim();
            }
      

            //'2019-05-23 ( + 15분 )
            //'07:00~15:45 : D1 근무자 챠팅 가능
            //'15:00~23:15 : E1 근무자 챠팅 가능
            //'23:00~ : N1 근무자 챠팅 가능
            //'~07:45 : 전날 N1 근무자 챠팅 가능

            //'2019-06-07 ( + 2시간 )
            //'07:00~17:30 : D1 근무자 챠팅 가능
            //'15:00~ : E1 근무자 챠팅 가능
            //'~01:30 : 전날 E1 근무자 챠팅 가능
            //'23:00~ : N1 근무자 챠팅 가능
            //'~09:00 : 전날 N1 근무자 챠팅 가능

            string strOK = string.Empty;
            if ( strDuty == "D1" || strDuty == "E1" || strDuty == "N1" || strDuty2 == "N1" || strDuty == "휴가" || strDuty == "OFF" )
            {
                DateTime sysTime = Convert.ToDateTime(clsPublic.GstrSysTime);

                if (DateTime.Compare(sysTime, Convert.ToDateTime("07:00")) >= 0 && DateTime.Compare(sysTime, Convert.ToDateTime("17:30")) <= 0 && strDuty == "D1")
                {
                    strOK = "OK";
                }
                else if (DateTime.Compare(sysTime, Convert.ToDateTime("15:00")) >= 0 && strDuty == "E1")
                {
                    strOK = "OK";
                }
                else if (DateTime.Compare(sysTime, Convert.ToDateTime("01:30")) <= 0 && strDuty == "E1")
                {
                    strOK = "OK";
                }
                else if (DateTime.Compare(sysTime, Convert.ToDateTime("23:00")) >= 0 && strDuty == "N1")
                {
                    strOK = "OK";
                }
                else if (DateTime.Compare(sysTime, Convert.ToDateTime("09:00")) <= 0 && strDuty2 == "N1")
                {
                    strOK = "OK";
                }
                else
                {
                    strOK = "NO";
                }
            }
            else
            {
                strOK = "OK";
            }


            if (strOK == "OK")
            {
                return false;      //'챠팅 가능함.
            }
            else
            {
                return true;       //'챠팅 불가능.
            }
        }

        /// <summary>
        /// '2019-05-16 간호부 요청으로 간호기록의 경우 인쇄 요청만 했는 경우라도 수정이 안되도록 차단!
        //  '반환값이 TRUE면 수정하지 않는다.
        /// </summary>
        /// <param name="strEmrNo"></param>
        /// <returns></returns>
        public static bool READ_PRTREQLOG(string strEmrNo)
        {
            bool rtnVal = false;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (strEmrNo == "") return rtnVal;


            //'2019-06-11 기능 사용 안함.
            return rtnVal;

            Cursor.Current = Cursors.WaitCursor;
            try
            {                
                SQL = "";
                SQL = " SELECT EMRNO, PRINTDATE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_EMR.EMRPRTREQ ";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + strEmrNo;
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTDATE DESC ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return false;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                }

                if (dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox("해당 챠트는 사본 발급 신청 내역이 존재합니다. 수정이 불가능합니다.");
                    rtnVal = true;
                }
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }
    }
}
