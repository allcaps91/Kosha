using System;
using System.Data;
using ComBase;
using ComDbB;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ComPmpaLibB
{
    public class clsHelpDesk
    {
        public static string GstrDoctMsg_tel = "";
        public static string GstrValue = "";            //'oumsad

        public static string READ_BAS_BUSE2(PsmhDb pDbCon, string strBuseCode)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strVal = "";

            try
            {
                SQL = "SELECT Name,SName FROM KOSMOS_PMPA.BAS_BUSE ";
                SQL += ComNum.VBLF + " WHERE BuCode='" + strBuseCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return strVal;
                }

                strVal = dt.Rows[0]["Name"].ToString().Trim().Length > 0 ? dt.Rows[0]["Name"].ToString().Trim() : dt.Rows[0]["SName"].ToString().Trim();

                dt.Dispose();
                dt = null;
                return strVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return strVal;
            }
        }

        public static string Read_Bas_OcsMemo_Check_tel(PsmhDb pDbCon, string ArgPano, string ArgGbn)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT PANO,SNAME,MEMO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_OCSMEMO_MID ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO  = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "   AND (DDATE IS NULL OR DDATE ='' )";
                SQL = SQL + ComNum.VBLF + "   AND GBN  in ( '" + ArgGbn + "' ,'0' )  ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY  sdate desc ";
           
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }
                rtnVal = dt.Rows[0]["MEMO"].ToString().Trim();
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return rtnVal;
            }
        }


        public static string PushCheckMobileMemberShip(string argPano)
        {
            //모바일앱 가입정보 가져오기
            string rtnVal = "";
            string strRequest = "";

            if (argPano == "")
            {
                return rtnVal;
            }

            if (argPano.Length != 8)
            {
                return rtnVal;
            }

            try
            {
                WebRequest request = WebRequest.Create("http://192.168.2.25:8202/u2m/users/info/" + argPano); // 호출할 url
                request.Method = "GET";

                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                strRequest = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();
                dataStream.Flush();

                response.Close();

                JObject jobj = JObject.Parse(strRequest); //문자를 객체화

                if (jobj != null && jobj["joinYn"] != null)
                {
                    rtnVal = jobj["joinYn"].ToString();
                }


                return rtnVal;
            }
            catch (Exception ex)
            {

                ComFunc.MsgBox(ex.Message,"가입정보를 읽기 실패");
                return "";
            }
        }


        public static string READ_BAS_Doctor(PsmhDb pDbCon, string ArgDrCode)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            if (ArgDrCode == "") return rtnVal;

            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT DrName";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                SQL = SQL + ComNum.VBLF + "    WHERE DrCode='" + ArgDrCode + "'";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장                    
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }
                rtnVal = dt.Rows[0]["DrName"].ToString().Trim();
                dt.Dispose();
                dt = null;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return "";
            }

        }

        public static string READ_INSA_NAME(PsmhDb pDbCon, string argSabun)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string rtnVal = "";

            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }

                SQL = "";
                SQL = SQL + " SELECT KORNAME FROM " + ComNum.DB_ERP + "INSA_MST  ";
                SQL = SQL + ComNum.VBLF + "  WHERE SABUN IN ('" + argSabun.PadLeft(5, '0') + "') ";
                SQL = SQL + ComNum.VBLF + "    AND ( TOIDAY IS NULL OR TOIDAY < TRUNC(SYSDATE) )";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["KorName"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        public static string READ_H_Name(PsmhDb pDbCon, string sHCode)
        {
            string strRtn = "";

            if (sHCode == "0")
            {
                strRtn = "";
                return strRtn;
            }

            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (pDbCon == null)
                {
                    pDbCon = clsDB.DbCon;
                }

                SQL = "";
                SQL += " SELECT Name FROM KOSMOS_PMPA.ETC_Return_Code   \r";
                SQL += "  WHERE Code  = '" + sHCode + "'                \r";
                SQL += "    AND Gubun = '01'                            \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }

                if (dt.Rows.Count > 0)
                {
                    strRtn = dt.Rows[0]["NAME"].ToString();
                }
                else
                {
                    strRtn = "";
                }
                dt.Dispose();
                dt = null;
                return strRtn;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return strRtn;
            }
        }

        public static string GetDeptName(string argDeptCode)
        {
            string returnVal = "";

            DataTable dt = null;
            string SQL = ""; // Query문
            string SqlErr = ""; // 에러문 받는 변수


            try
            {
                SQL = " SELECT DeptNameK FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT      ";
                SQL = SQL + ComNum.VBLF + "  WHERE DEPTCODE = '" + argDeptCode + "'   ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return returnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return returnVal;
                }

                returnVal = dt.Rows[0]["DEPTNAMEK"].ToString().Trim();
                dt.Dispose();
                dt = null;
                return returnVal;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); // 에러로그 저장
                return returnVal;
            }
        }

        public static string Read_Doctor_Licno(string ArgDrCode)
        {
            string returnVal = "";

            DataTable dt = null;
            string SQL = ""; // Query문
            string SqlErr = ""; // 에러문 받는 변수


            try
            {
                SQL = " SELECT DrBunho FROM KOSMOS_OCS.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DRCODE='" + ArgDrCode + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return returnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return returnVal;
                }

                returnVal = dt.Rows[0]["DrBunho"].ToString().Trim();
                dt.Dispose();
                dt = null;
                return returnVal;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); // 에러로그 저장
                return returnVal;
            }
        }

        public static string Read_Doctor_TelNo(string ArgDrCode)
        {
            string returnVal = "";

            DataTable dt = null;
            string SQL = ""; // Query문
            string SqlErr = ""; // 에러문 받는 변수


            try
            {
                SQL = " SELECT TelNo FROM KOSMOS_PMPA.BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE DRCODE='" + ArgDrCode + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return returnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return returnVal;
                }

                returnVal = dt.Rows[0]["TelNo"].ToString().Trim();
                dt.Dispose();
                dt = null;
                return returnVal;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); // 에러로그 저장
                return returnVal;
            }
        }

        public static string READ_EDI_DEPT_CODE(string ArgDept)
        {
            string rtnVal = "99";

            switch (ArgDept)
            {
                case "MD": rtnVal = "01"; break;
                case "MG": rtnVal = "01"; break;
                case "MC": rtnVal = "01"; break;
                case "MP": rtnVal = "01"; break;
                case "ME": rtnVal = "01"; break;
                case "MN": rtnVal = "01"; break;
                case "MI": rtnVal = "01"; break;
                case "MR": rtnVal = "01"; break;
                case "HD": rtnVal = "01"; break;
                case "NE": rtnVal = "02"; break;
                case "NP": rtnVal = "03"; break;
                case "GS": rtnVal = "04"; break;
                case "OS": rtnVal = "05"; break;
                case "NS": rtnVal = "06"; break;
                case "CS": rtnVal = "07"; break;
                case "PS": rtnVal = "08"; break;
                case "AN": rtnVal = "09"; break;
                case "RT":
                case "PC":
                    rtnVal = "09"; break;
                case "OB":
                case "OG":
                    rtnVal = "10"; break;
                case "PD": rtnVal = "11"; break;
                case "OT": rtnVal = "12"; break;
                case "EN": rtnVal = "13"; break;
                case "DM": rtnVal = "14"; break;
                case "UR": rtnVal = "15"; break;
                case "RD": rtnVal = "16"; break;
                case "PT":
                case "RM": rtnVal = "21"; break;
                case "FM": rtnVal = "23"; break;
                case "EM":
                case "ER": rtnVal = "24"; break;
                case "DN":
                case "DT": rtnVal = "55"; break;
                default:
                    rtnVal = "99";
                    break;
            }
            return rtnVal;
        }

        public static string READ_BI(string strBi)
        {
            switch (strBi)
            {
                case "11": strBi = "11.공무원"; break;
                case "12": strBi = "12.연합회"; break;
                case "13": strBi = "13.지  역"; break;
                case "21": strBi = "21.보호1종"; break;
                case "22": strBi = "22.보호2종"; break;
                case "23": strBi = "23.보호3종(부조)"; break;
                case "24": strBi = "24.행려환자"; break;
                case "31": strBi = "31.산  재"; break;
                case "32": strBi = "32.공  상"; break;
                case "33": strBi = "33.산재공상"; break;
                case "41": strBi = "41.공단100%"; break;
                case "42": strBi = "42.직장180일"; break;
                case "43": strBi = "43.지역100%"; break;
                case "44": strBi = "44.가족계획"; break;
                case "51": strBi = "51.일  반"; break;
                case "52": strBi = "52.TA보험"; break;
                case "53": strBi = "53.계약처"; break;
                case "54": strBi = "54.미확인"; break;
                case "55": strBi = "55.TA일반"; break;
            }

            return strBi;
        }

        public static string READ_HCode_Kiho_charge(string ArgCode)
        {
            string returnVal = "";

            DataTable dt = null;
            string SQL = ""; // Query문
            string SqlErr = ""; // 에러문 받는 변수

            if (ArgCode == "0")
            {
                return returnVal;
            }

            try
            {
                SQL = "SELECT Charge FROM ETC_Return_Code ";
                SQL = SQL + ComNum.VBLF + " WHERE Code = " + ArgCode + " ";
                SQL = SQL + ComNum.VBLF + " AND Gubun='01' ";
                SQL = SQL + ComNum.VBLF + " AND Charge IS NOT NULL ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return returnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return returnVal;
                }

                returnVal = dt.Rows[0]["Charge"].ToString().Trim();
                dt.Dispose();
                dt = null;
                return returnVal;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); // 에러로그 저장
                return returnVal;
            }
        }

        public static string Read_All_Hospital_Display(string strCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "select";
                SQL = SQL + ComNum.VBLF + "     name";
                SQL = SQL + ComNum.VBLF + "from " + ComNum.DB_PMPA + "etc_return_code";
                SQL = SQL + ComNum.VBLF + "     where code = '" + strCode + "'";
                //SQL = SQL + ComNum.VBLF + "         and deldate is null";
                SQL = SQL + ComNum.VBLF + "         and gubun = '01'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["NAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public static string READ_BAS_SUGAK(string ArgCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = " SELECT SUNAMEK ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUN ";
                SQL = SQL + ComNum.VBLF + " WHERE SUNEXT = '" + ArgCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SUNAMEK"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public static string Read_DrCode_Display(string strDrCode)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strVal = "";

            try
            {
                SQL = " SELECT DrName AS NAME ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_DOCTOR ";
                SQL += ComNum.VBLF + "  WHERE GbOut <> 'Y' ";
                SQL += ComNum.VBLF + "   AND DRCODE ='" + strDrCode + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return strVal;
                }

                strVal = dt.Rows[0]["Name"].ToString().Trim();

                dt.Dispose();
                dt = null;
                return strVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strVal;
            }
        }

        public static string Read_Hospital_Display(string strCode)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strVal = "";

            try
            {
                SQL = "SELECT Name ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.ETC_RETURN_CODE ";
                SQL += ComNum.VBLF + "WHERE Gubun ='05' ";
                SQL += ComNum.VBLF + "  AND Code ='" + strCode + "'";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strVal;

                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return strVal;
                }

                strVal = dt.Rows[0]["Name"].ToString().Trim();

                dt.Dispose();
                dt = null;
                return strVal;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strVal;
            }
        }

        public static string READ_TELGUBUN(string strTel)
        {
            switch (strTel)
            {

                case "0":
                    strTel = "0.시내";
                    break;
                case "1":
                    strTel = "1.시외";
                    break;
                case "2":
                    strTel = "2.휴대폰";
                    break;
                case "3":
                    strTel = "3.114";
                    break;
                case "4":
                    strTel = "4.호출";
                    break;
                case "5":
                    strTel = "5.해외";
                    break;
                case "6":
                    strTel = "6.전보";
                    break;
                    //        Case "7"
                    //'            READ_TELGUBUN = "시내 통화"
                    //        Case "8"
                    //'            READ_TELGUBUN = "시내 통화"
                    //        Case "9"
                    //'            READ_TELGUBUN = "시내 통화"

            }
            return strTel;
        }

        public static string READ_TEL_JIYUK(string strTelNo, string strGubun)
        {
            string strTemp = VB.Left(strTelNo, 3);

            if (VB.Left(strTelNo, 2) == "02")
            {
                strTemp = "서울";
            }
            else
            {
                switch (strTemp)
                {

                    case "033":
                        strTemp = "강원도";
                        break;
                    case "031":
                        strTemp = "경기도";
                        break;
                    case "055":
                        strTemp = "경남";
                        break;
                    case "054":
                        strTemp = "경북";
                        break;
                    case "062":
                        strTemp = "광주";
                        break;
                    case "053":
                        strTemp = "대구";
                        break;
                    case "042":
                        strTemp = "대전";
                        break;
                    case "051":
                        strTemp = "부산";
                        break;
                    case "052":
                        strTemp = "울산";
                        break;
                    case "032":
                        strTemp = "인천";
                        break;
                    case "061":
                        strTemp = "전남";
                        break;
                    case "063":
                        strTemp = "전북";
                        break;
                    case "064":
                        strTemp = "제주";
                        break;
                    case "041":
                        strTemp = "충남";
                        break;
                    case "043":
                        strTemp = "충북";
                        break;
                    case "044":
                        strTemp = "세종시";
                        break;
                }
            }

            strTemp = strGubun == "0" && strTemp.Length > 0 ? "1.시외(" + strTemp + ")" : strTemp;
            return strTemp;
        }

        public static bool DATE_HUIL_Check(string ArgDate)
        {
            if (ArgDate.Length == 0) return false;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            bool rtnVal = false;

            try
            {
                SQL = "SELECT HolyDay,TempHolyDay FROM KOSMOS_PMPA.BAS_JOB ";
                SQL += ComNum.VBLF + " WHERE JobDate=TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVal;
                }

                if (dt.Rows[0]["HolyDay"].ToString() == "*")
                {
                    //TODO
                    //'공휴일 가산에 쓰는 변수임 삭제하지 마세요~ KMC 2014-11-24
                    clsPublic.GstrHoliday = "*";
                    rtnVal = true;
                }

                if (dt.Rows[0]["TempHolyDay"].ToString() == "*")
                {
                    clsPublic.GstrTempHoliday = "*";
                }

                dt.Dispose();
                dt = null;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        public static string Read_Cost(string argValue, string ArgGubun, string ArgTime, string argGongSa)
        {
            //'ArgValue 전화사용시간, argGubun 전화구분(시내0, 시외1, 휴대폰2, 114(3), 호출4, 해외5, 전보6)
            //'argTime 전화연결일시(YYYY-MM-DD HH24:MI) , argGongSa(공, 사, 병)
            double rtnVal = 0;

            if (argGongSa == "사" || argGongSa == "병")
            {
                if (ArgGubun == "1" || ArgGubun == "2")
                {

                    rtnVal = VB.Val(argValue) * 100;
                }
                else if (ArgGubun == "0")
                {

                    rtnVal = VB.Val(argValue) / 3 * 100;
                }
            }
            else
            {
                rtnVal = VB.Val(argValue) * 100;
            }

            if (ArgGubun == "3") //'114일 경우
            {

                if (DATE_HUIL_Check(VB.Left(ArgTime, 10)) == true) //  '휴일일 경우 분당 140원
                {
                    rtnVal = VB.Val(argValue) * 140;
                }
                else
                {
                    //'휴일이 아닐경우
                    if (clsVbfunc.GetYoIl(VB.Left(ArgTime, 10)) == "토요일") //'토요일 13시 이후 140원
                    {
                        if (Convert.ToDateTime(ArgTime).Hour >= 13 || Convert.ToDateTime(ArgTime).Hour < 9)
                        {

                            rtnVal = VB.Val(argValue) * 140;
                        }
                        else
                        {

                            rtnVal = VB.Val(argValue) * 120; //'토요일 13시 이전 120원
                        }
                    }
                    else if (clsVbfunc.GetYoIl(VB.Left(ArgTime, 10)) == "일요일")
                    {
                        rtnVal = VB.Val(argValue) * 140;
                    }
                    else
                    {
                        if (Convert.ToDateTime(ArgTime).Hour >= 9 && Convert.ToDateTime(ArgTime).Hour <= 18) //'평일 9시 부터 오후 6시까지 120원  '수녀님 7월 1일 통화
                        {

                            rtnVal = VB.Val(argValue) * 120;
                        }
                        else
                        {
                            rtnVal = VB.Val(argValue) * 140; //'평일 그외에 140원
                        }
                    }
                }
            }
            return rtnVal.ToString();
        }
    }
}
