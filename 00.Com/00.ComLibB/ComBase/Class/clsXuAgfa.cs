using System;
using System.Data;
using ComDbB; //DB연결

namespace ComBase
{
    public class clsXuAgfa
    {
        public static int GnPrtOnOff = 0; //1.인쇄함 0.인쇄않함
        public static string GstrAesJumin1 = "";
        public static string GstrAesJumin2 = "";

        public struct TXD
        {
            public static string Pano = "";
            public static string sName = "";
            public static int Age = 0;
            public static string Sex = "";
            public static string DeptCode = "";
            public static string DrCode = "";
            public static string DrName = "";
            public static string IpdOpd = "";
            public static string WardCode = "";
            public static string RoomCode = "";
            public static string SeekDate = "";
            public static string ReadDate = "";
            public static string ReadTime = "";
            public static string Gisa = "";
            public static string XJong = "";
            public static string XCode = "";
            public static string Xname = "";
            public static int XDrSabun = 0; //판독의사 사번
            public static int WRTNO = 0;
            public static string PacsNo = "";
            public static int SelCNT = 0;
            public static string SelROWID = "";
            public static string EnterDate = ""; //검사요청일
        }

        public static string READ_XRAY_GISA(PsmhDb pDbCon, int ArgCode)
        {
            string strVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            if (ArgCode == 0)
            {
                strVal = "";
                return strVal;
            }

            //frmPrintViewXray frm = new frmPrintViewXray();

            try
            {
                //if (ComQuery.IsJobAuth(frm, "R") == false)
                //{
                //    return strVal; //권한 확인
                //}

                SQL = "SELECT Name FROM BAS_PASS ";
                SQL = SQL + ComNum.VBLF + "WHERE IDnumber = " + ArgCode + " ";
                SQL = SQL + ComNum.VBLF + "  AND ProgramID = ' ' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }
                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return strVal;
                //}

                if (dt.Rows.Count == 1)
                {
                    strVal = dt.Rows[0]["Name"].ToString().Trim();
                }
                else
                {
                    strVal = "";
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }

            return strVal;
        }

        //문자열의 인쇄시 줄의 갯수를 구함(문자열,1줄당 인쇄 글자수)
        public static int String_PrintLine_COUNT(string ArgData, int ArgLen)
        {
            int intVal = 0;
            int ArgInx = 0;
            string ArgChar = "";
            string ArgList = "";
            int ArgLine = 0;

            ArgLine = 0;

            for (ArgInx = 1; ArgInx < VB.Len(ArgData); ArgInx++)
            {
                ArgChar = VB.Mid(ArgData, ArgInx, 1);

                switch (ArgChar)
                {
                    case "Chr$(13)": //에러 없앨려고 ""안에 넣음
                    case "Chr$(10)": //에러 없앨려고 ""안에 넣음
                        if (ArgChar == ComNum.VBLF)
                        {
                            ArgLine = ArgLine + 1;
                            ArgList = "";
                        }
                        break;
                    default:
                        ArgList += ArgChar;
                        break;
                }

                if (VB.Len(ArgList) >= ArgLen)
                {
                    ArgLine = ArgLine + 1;
                    ArgList = "";
                }
            }

            if (ArgLine != 0)
            {
                ArgLine += 1;
            }

            intVal = ArgLine;
            return intVal;
        }

        public static string READ_DrName(PsmhDb pDbCon, string ArgCode)
        {
            string strVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            //frmPrintViewXray frm = new frmPrintViewXray();

            try
            {
                //if (ComQuery.IsJobAuth(frm, "R") == false)
                //{
                //    return strVal; //권한 확인
                //}

                SQL = "SELECT DrName FROM " + ComNum.DB_PMPA + "BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE DrCode='" + ArgCode + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return strVal;
                }
                //if (dt.Rows.Count == 0)
                //{
                //    dt.Dispose();
                //    dt = null;
                //    ComFunc.MsgBox("해당 DATA가 없습니다.");
                //    return strVal;
                //}

                if (dt.Rows.Count > 0)
                {
                    strVal = dt.Rows[0]["DrName"].ToString().Trim();
                }
                else
                {
                    strVal = ArgCode.Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
            }

            return strVal;
        }
    }
}
