using System;
using System.Windows.Forms;
using System.Data;
using ComLibB.Properties;
using ComBase;
using System.Reflection;
using System.Globalization;
using FarPoint.Win.Spread;
using Oracle.DataAccess.Client;
using ComDbB;
using ComNurLibB;

namespace ComNurLibB
{
    //간호공용 clsss
    public class clsNurse
    {

        public static string GstrHelpCode = "";
        public static string GstrHelpName = "";
        public static string gsWard = "";
        public static string GstrWardCodeCSR = "";

        // 메모리 클리어
        public static void setClearMemory(Form argForm)
        {
            argForm.Dispose();
            argForm = null;
            clsApi.FlushMemory();
        }

        // 스프레드 필터기능
        public static void setSpdColumnFilter(FpSpread spd)
        {
            int i = 0;
            clsSpread methodSpd = new clsSpread();

            for (i = 0; i < spd.ActiveSheet.ColumnCount; i++)
            {
                methodSpd.setSpdFilter(spd, i, AutoFilterMode.EnhancedContextMenu, true);
            }
        }

        // 감염 이미지
        public static object Resource2files(string argGubun)
        {
            if (argGubun == "00001")
            {
                return Resources.I00001;
            }
            else if (argGubun == "00010")
            {
                return Resources.I00010;
            }
            else if (argGubun == "00011")
            {
                return Resources.I00011;
            }
            else if (argGubun == "00100")
            {
                return Resources.I00100;
            }
            else if (argGubun == "00101")
            {
                return Resources.I01001;
            }
            else if (argGubun == "00110")
            {
                return Resources.I00110;
            }
            else if (argGubun == "00111")
            {
                return Resources.I00111;
            }
            else if (argGubun == "01000")
            {
                return Resources.I01000;
            }
            else if (argGubun == "01001")
            {
                return Resources.I01001;
            }
            else if (argGubun == "01010")
            {
                return Resources.I01010;
            }
            else if (argGubun == "01011")
            {
                return Resources.I01011;
            }
            else if (argGubun == "01100")
            {
                return Resources.I01100;
            }
            else if (argGubun == "01101")
            {
                return Resources.I01101;
            }
            else if (argGubun == "01110")
            {
                return Resources.I01110;
            }
            else if (argGubun == "01111")
            {
                return Resources.I01111;
            }
            else if (argGubun == "10000")
            {
                return Resources.I10000;
            }
            else if (argGubun == "10001")
            {
                return Resources.I10001;
            }
            else if (argGubun == "10010")
            {
                return Resources.I10010;
            }
            else if (argGubun == "10011")
            {
                return Resources.I10011;
            }
            else if (argGubun == "10100")
            {
                return Resources.I10100;
            }
            else if (argGubun == "10101")
            {
                return Resources.I10101;
            }
            else if (argGubun == "10110")
            {
                return Resources.I10110;
            }
            else if (argGubun == "10111")
            {
                return Resources.I10111;
            }
            else if (argGubun == "11000")
            {
                return Resources.I11000;
            }
            else if (argGubun == "11010")
            {
                return Resources.I11010;
            }
            else if (argGubun == "11011")
            {
                return Resources.I11011;
            }
            else if (argGubun == "11100")
            {
                return Resources.I11100;
            }
            else if (argGubun == "11101")
            {
                return Resources.I11101;
            }
            else if (argGubun == "11110")
            {
                return Resources.I11110;
            }
            else if (argGubun == "11111")
            {
                return Resources.I11111;
            }
            else
            {
                return Resources.I00000;
            }
        }


        /// <summary>
        /// 양력을 음력으로 변환
        /// </summary>
        /// <param name="solarDate"></param>
        /// <returns></returns>
        public static string ConvertFromSolarDate(string solarDate)
        {
            string leapDate;

            DateTime date = DateTime.Parse(solarDate);
            KoreanLunisolarCalendar ksc = new KoreanLunisolarCalendar();

            int year = ksc.GetYear(date);
            int month = ksc.GetMonth(date);
            int day = ksc.GetDayOfMonth(date);
            bool isLeapMonth;

            //윤달이 끼어 있으면
            if (ksc.GetMonthsInYear(year) > 12)
            {
                int leapMonth = ksc.GetLeapMonth(year);

                if (month >= leapMonth)
                {
                    isLeapMonth = (month == leapMonth);
                    month--;
                }
            }

            leapDate = string.Format("{0}-{1}-{2}", year, month, day);
            return leapDate;
        }


        public static DateTime ConvertFromKoreaLunarDate(string lunarDate)
        {
            DateTime returnDate = new DateTime();

            DateTime date = DateTime.Parse(lunarDate);
            KoreanLunisolarCalendar ksc = new KoreanLunisolarCalendar();

            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (ksc.GetMonthsInYear(year) > 12)
            {
                int leapMonth = ksc.GetLeapMonth(year);

                if (month >= leapMonth - 1)
                {
                    returnDate = ksc.ToDateTime(year, month + 1, day, 0, 0, 0, 0);
                }
                else
                {
                    returnDate = ksc.ToDateTime(year, month, day, 0, 0, 0, 0);
                }
            }
            else
            {
                returnDate = ksc.ToDateTime(year, month, day, 0, 0, 0, 0);
            }

            return returnDate;
        }

        /// <summary>
        /// Description : 해당 월(MM) 계산 ex)201708 -> 201707 or 201709
        /// Author : 안정수
        /// Create Date : 2017.09.07
        /// </summary>
        /// <param name="ArgYYMM"></param>
        /// <param name="argADD"></param>
        /// <seealso cref="VBFunction.bas : DATE_YYMM_ADD"/>
        public string DATE_YYMM_ADD(string ArgYYMM, int argADD)
        {
            string rtnVal = "";

            int ArgI = 0;
            int ArgJ = 0;
            int ArgYY = 0;
            int ArgMM = 0;

            if (ArgYYMM.Length != 6 || argADD == 0)
            {
                return ArgYYMM;
            }

            ArgYY = Convert.ToInt32(VB.Left(ArgYYMM, 4));
            ArgMM = Convert.ToInt32(VB.Right(ArgYYMM, 2));

            ArgJ = argADD;

            if (ArgJ < 0)
            {
                ArgJ = ArgJ * -1;
            }

            for (ArgI = 1; ArgI <= ArgJ; ArgI++)
            {
                if (argADD < 0)
                {
                    ArgMM -= 1;
                    if (ArgMM == 0)
                    {
                        ArgMM = 12;
                        ArgYY -= 1;
                    }
                }
                else
                {
                    ArgMM += 1;
                    if (ArgMM == 13)
                    {
                        ArgYY += 1;
                        ArgMM = 1;
                    }
                }

            }

            rtnVal = ComFunc.SetAutoZero(ArgYY.ToString(), 4) + ComFunc.SetAutoZero(ArgMM.ToString(), 2);
            return rtnVal;
        }



        /// <summary>
        /// AGE_MONTH_GESAN
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strJumin"></param>
        /// <param name="strDate"></param>
        /// <returns></returns>
        /// <seealso cref="VBFunction.bas : AGE_MONTH_GESAN"/>
        public static int AGE_MONTH_GESAN(PsmhDb pDbCon, string strJumin, string strDate)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            int rtnVal = 0;

            int intJuminLen = 0;
            int intAge = 0;

            string strSex = "";
            string strBirth = "";

            try
            {
                //주민번호가 7보다 적으면 오류
                //기준일자는 반드시 'YYYY - MM - DD' Type이여야 함
                intJuminLen = VB.Len(strJumin);

                if (intJuminLen < 7)
                {
                    rtnVal = 12;
                    return rtnVal;
                }

                if (VB.Len(strDate) != 10)
                {
                    rtnVal = 12;
                    return rtnVal;
                }

                //성별을 Setting
                strSex = "1";

                if (intJuminLen > 6)
                {
                    strSex = ComFunc.MidH(strJumin, 7, 1);
                }

                if (strSex == "-")
                {
                    if (intJuminLen > 7)
                    {
                        strSex = ComFunc.MidH(strJumin, 8, 1);
                    }
                    else
                    {
                        strSex = "1";
                    }
                }

                //생년월일을 YYYY-MM-DD Type으로 변경
                if (strSex == "1" || strSex == "2")
                {
                    strBirth = "19" + ComFunc.LeftH(strJumin, 2) + "-" + ComFunc.MidH(strJumin, 3, 2) + "-" + ComFunc.MidH(strJumin, 5, 2);
                }
                else if (strSex == "3" || strSex == "4")
                {
                    strBirth = "20" + ComFunc.LeftH(strJumin, 2) + "-" + ComFunc.MidH(strJumin, 3, 2) + "-" + ComFunc.MidH(strJumin, 5, 2);
                }
                else if (strSex == "0" || strSex == "9")
                {
                    strBirth = "18" + ComFunc.LeftH(strJumin, 2) + "-" + ComFunc.MidH(strJumin, 3, 2) + "-" + ComFunc.MidH(strJumin, 5, 2);
                }
                else
                {
                    rtnVal = 12;
                    return rtnVal;
                }

                //기준일자가 생년월일보다 적으면 12개월 처리
                if (Convert.ToDateTime(strBirth) >= Convert.ToDateTime(strDate))
                {
                    rtnVal = 12;
                    return rtnVal;
                }

                //주민번호가 오류이면 12개월 처리
                if (VB.IsDate(ComFunc.RightH(strBirth, 8)) == false)
                {
                    rtnVal = 12;
                    return rtnVal;
                }

                SQL = "";
                SQL = "SELECT MONTHS_BETWEEN(TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                SQL = SQL + ComNum.VBLF + "       TO_DATE('" + strBirth + "','YYYY-MM-DD')) cAge FROM DUAL";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count == 1)
                {
                    intAge = VB.Fix(Convert.ToInt32(VB.Val(dt.Rows[0]["CAGE"].ToString().Trim())));
                }

                dt.Dispose();
                dt = null;

                rtnVal = intAge;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }


        /// <summary>
        /// BYTE용 MID 함수
        /// </summary>
        /// <param name="strExp"></param>
        /// <param name="intLR"></param>
        /// <param name="intLen"></param>
        /// <returns></returns>
        public static string MidH(string strExp, int intLR, int intLen)
        {
            string rtnVal = "";

            if (strExp == "")
            {
                return "";
            }

            try
            {
                System.Text.Encoding Enchar = null;
                Enchar = System.Text.Encoding.GetEncoding("EUC-KR");
                byte[] buf = Enchar.GetBytes(strExp);

                if (buf.Length < intLR + intLen)
                {
                    rtnVal = Enchar.GetString(buf, intLR, buf.Length - intLR);
                }
                else
                {
                    rtnVal = Enchar.GetString(buf, intLR, intLen); ;
                }
            }
            catch (Exception e)
            {
                ComFunc.MsgBox(e.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// NUR_CODE
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static string READ_BuseCode(PsmhDb pDbCon, string strName)
        {
            string returnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT Code cCode FROM " + ComNum.DB_PMPA + "NUR_CODE ";
                SQL += ComNum.VBLF + "WHERE Gubun = '2' ";
                SQL += ComNum.VBLF + "  AND Name = '" + strName.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return returnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return returnVal;
                }

                returnVal = dt.Rows[0]["cCode"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return returnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return returnVal;
            }
        }

        /// <summary>
        /// NUR_CODE
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static string READ_BuseName(PsmhDb pDbCon, string strCode)
        {
            string returnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT Name FROM " + ComNum.DB_PMPA + "NUR_CODE ";
                SQL += ComNum.VBLF + "WHERE Gubun = '2' ";
                SQL += ComNum.VBLF + "  AND Code = '" + strCode.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return returnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return returnVal;
                }

                returnVal = dt.Rows[0]["Name"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return returnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return returnVal;
            }
        }

        /// <summary>
        /// NUR_CODE
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static string READ_JikName(PsmhDb pDbCon, string strCode)
        {
            string returnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT Name FROM " + ComNum.DB_PMPA + "NUR_CODE ";
                SQL += ComNum.VBLF + "WHERE Gubun = '1' ";
                SQL += ComNum.VBLF + "  AND Code = '" + strCode.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return returnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return returnVal;
                }

                returnVal = dt.Rows[0]["Name"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return returnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return returnVal;
            }
        }


        /// <summary>
        /// NUR_CODE
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static string READ_JikCode(PsmhDb pDbCon, string strName)
        {
            string returnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT Code cCode FROM " + ComNum.DB_PMPA + "NUR_CODE ";
                SQL += ComNum.VBLF + "WHERE Gubun = '1' ";
                SQL += ComNum.VBLF + "  AND Name = '" + strName.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return returnVal;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return returnVal;
                }

                returnVal = dt.Rows[0]["cCode"].ToString().Trim();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return returnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return returnVal;
            }
        }


        /// <summary>
        /// 낙상
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="strPtNo"></param>
        /// <param name="strDate"></param>
        /// <param name="dblIpdNo"></param>
        /// <param name="strAge"></param>
        /// <param name="strSTS"></param>
        /// <returns></returns>
        public static string READ_WARNING_FALL(PsmhDb pDbCon, string strPtNo, string strDate, double dblIpdNo, string strAge, string strSTS = "")
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            string strFail = "";

            try
            {
                //ArgSTS 외래일경우 "외래" 값들어감
                //외래일경우 다시 쿼리
                if (strSTS == "외래" && dblIpdNo == 0)
                {
                    SQL = "";
                    SQL = "SELECT IPDNO FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER     ";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO ='" + strPtNo + "' ";
                    SQL = SQL + ComNum.VBLF + "         AND JDATE =TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND ACTDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY INDATE DESC ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

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

                    dblIpdNo = VB.Val(dt.Rows[0]["IPDNO"].ToString().Trim());

                    dt.Dispose();
                    dt = null;
                }

                SQL = "";
                SQL = "SELECT WARDCODE, AGE FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "     WHERE IPDNO = " + dblIpdNo;

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    switch (dt.Rows[0]["WARDCODE"].ToString().Trim())
                    {
                        case "33":
                        case "35":
                        case "NR":
                        case "IQ":
                            strFail = "OK";
                            break;
                    }

                    if (VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) >= 70 || VB.Val(dt.Rows[0]["AGE"].ToString().Trim()) < 7)
                    {
                        strFail = "OK";
                    }

                    if (VB.IsNumeric(strAge) == false)
                    {
                        strAge = dt.Rows[0]["AGE"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                if (strFail == "OK")
                {
                    rtnVal = "낙상위험";
                    return rtnVal;
                }

                SQL = "";
                SQL = "SELECT PANO, TOTAL, AGE FROM " + ComNum.DB_PMPA + "NUR_FALLMORSE_SCALE";
                SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPtNo + "'";
                SQL = SQL + ComNum.VBLF + "         AND IPDNO = " + dblIpdNo;
                SQL = SQL + ComNum.VBLF + "         AND TOTAL >= 51";
                SQL = SQL + ComNum.VBLF + "         AND ROWID = (";
                SQL = SQL + ComNum.VBLF + "                 SELECT ROWID FROM (";
                SQL = SQL + ComNum.VBLF + "                         SELECT * FROM " + ComNum.DB_PMPA + "NUR_FALLMORSE_SCALE";
                SQL = SQL + ComNum.VBLF + "                             WHERE ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                SQL = SQL + ComNum.VBLF + "                                 AND IPDNO = " + dblIpdNo;
                SQL = SQL + ComNum.VBLF + "                         ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                SQL = SQL + ComNum.VBLF + "                     Where ROWNUM = 1)";
                SQL = SQL + ComNum.VBLF + "ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

                if (VB.Val(strAge) < 18)
                {
                    SQL = "";
                    SQL = "SELECT PANO, TOTAL, AGE FROM " + ComNum.DB_PMPA + "NUR_FALLHUMPDUMP_SCALE";
                    SQL = SQL + ComNum.VBLF + "     WHERE PANO = '" + strPtNo + "'";
                    SQL = SQL + ComNum.VBLF + "         AND IPDNO = " + dblIpdNo;
                    SQL = SQL + ComNum.VBLF + "         AND TOTAL >= 12 ";
                    SQL = SQL + ComNum.VBLF + "         AND ROWID = (";
                    SQL = SQL + ComNum.VBLF + "                 SELECT ROWID FROM (";
                    SQL = SQL + ComNum.VBLF + "                         SELECT * FROM " + ComNum.DB_PMPA + "NUR_FALLHUMPDUMP_SCALE";
                    SQL = SQL + ComNum.VBLF + "                             WHERE ACTDATE = TO_DATE('" + strDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "                                 AND IPDNO = " + dblIpdNo;
                    SQL = SQL + ComNum.VBLF + "                         ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                    SQL = SQL + ComNum.VBLF + "                     Where ROWNUM = 1)";
                    SQL = SQL + ComNum.VBLF + "ORDER BY ActDate DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strFail = "OK";
                }

                dt.Dispose();
                dt = null;

                if (strFail == "")
                {
                    SQL = "";
                    SQL = "SELECT IPDNO FROM " + ComNum.DB_PMPA + "NUR_FALL_WARNING";
                    SQL = SQL + ComNum.VBLF + "     WHERE IPDNO = " + dblIpdNo;
                    SQL = SQL + ComNum.VBLF + "         AND (WARNING1 = '1'";
                    SQL = SQL + ComNum.VBLF + "                 OR WARNING2 = '1'";
                    SQL = SQL + ComNum.VBLF + "                 OR WARNING3 = '1'";
                    SQL = SQL + ComNum.VBLF + "                 OR WARNING4 = '1'";
                    SQL = SQL + ComNum.VBLF + "                 OR WARNING5 = '1'";
                    SQL = SQL + ComNum.VBLF + "                 OR DRUG_01 = '1'";
                    SQL = SQL + ComNum.VBLF + "                 OR DRUG_02 = '1'";
                    SQL = SQL + ComNum.VBLF + "                 OR DRUG_03 = '1'";
                    SQL = SQL + ComNum.VBLF + "                 OR DRUG_04 = '1'";
                    SQL = SQL + ComNum.VBLF + "                 OR DRUG_05 = '1'";
                    SQL = SQL + ComNum.VBLF + "                 OR DRUG_06 = '1'";
                    SQL = SQL + ComNum.VBLF + "                 OR DRUG_07 = '1'";
                    SQL = SQL + ComNum.VBLF + "                 OR DRUG_08 = '1'";
                    SQL = SQL + ComNum.VBLF + "                 OR DRUG_08_ETC <> '')";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return rtnVal;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        strFail = "OK";
                    }

                    dt.Dispose();
                    dt = null;
                }

                if (strFail == "OK")
                {
                    rtnVal = "낙상위험";
                }

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장

                return rtnVal;
            }
        }


        /// <summary>
        /// 욕창        
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPTNO"></param>
        /// <param name="argDATE"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgAge"></param>
        /// <param name="argWARD"></param>
        /// <param name="ArgDate2"></param>
        /// <returns></returns>
        public static string READ_WARNING_BRADEN(PsmhDb pDbCon, string argPTNO, string argDATE, string ArgIpdNo, string ArgAge, string argWARD, string ArgDate2 = "")
        {
            DataTable dt = null;
            string SqlErr = "";
            string SQL = "";
            string strValue = "";
            string strBraden = "";
            string strGubun = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (ArgIpdNo == "")
                {
                    SQL = "";
                    SQL = "SELECT IPDNO, WARDCODE ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER     ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + argPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND JDATE =TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY INDATE DESC ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return strGubun;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ArgIpdNo = dt.Rows[0]["IPDNO"].ToString().Trim();
                        argWARD = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    }
                    dt.Dispose();
                    dt = null;
                }



                if (VB.IsNumeric(ArgAge))
                {
                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT AGE ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER     ";
                    SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + argPTNO + "' ";
                    SQL = SQL + ComNum.VBLF + "   AND JDATE =TO_DATE('1900-01-01','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE IS NULL ";
                    SQL = SQL + ComNum.VBLF + "  ORDER BY INDATE DESC ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return strGubun;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ArgAge = dt.Rows[0]["AGE"].ToString().Trim();
                    }
                    else
                    {
                        ArgAge = "";
                    }

                    dt.Dispose();
                    dt = null;

                    if (ArgAge == "")
                    {
                        return ArgAge;
                    }

                    if (argWARD == "NR" || argWARD == "ND" || argWARD == "IQ")
                    {
                        strGubun = "신생아";
                    }
                    else if (string.Compare(ArgAge, "5") < 0)
                        strGubun = "소아";
                    else
                    {
                        strGubun = "";
                    }

                    if (strGubun == "")
                    {
                        SQL = "";
                        SQL = " SELECT A.PANO, A.TOTAL, A.AGE ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_BRADEN_SCALE A";
                        SQL = SQL + ComNum.VBLF + " WHERE A.IPDNO = " + ArgIpdNo;
                        SQL = SQL + ComNum.VBLF + "   AND A.PANO = '" + argPTNO + "' ";
                        if (ArgDate2 != "")
                        {
                            SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE >= TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "     AND A.ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                        }

                        SQL = SQL + ComNum.VBLF + "     AND A.TOTAL <= 18";
                        SQL = SQL + ComNum.VBLF + "     AND A.ROWID = (";
                        SQL = SQL + ComNum.VBLF + "   SELECT ROWID FROM (";
                        SQL = SQL + ComNum.VBLF + "  SELECT * FROM KOSMOS_PMPA.NUR_BRADEN_SCALE";
                        SQL = SQL + ComNum.VBLF + "  WHERE ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "       AND IPDNO = " + ArgIpdNo;
                        SQL = SQL + ComNum.VBLF + "       AND PANO = '" + argPTNO + "' ";
                        SQL = SQL + ComNum.VBLF + "  ORDER BY DECODE(ENTDATE, NULL, 2, 1), ACTDATE DESC)";
                        SQL = SQL + ComNum.VBLF + "  Where ROWNUM = 1)";
                        SQL = SQL + ComNum.VBLF + "  ORDER BY A.ActDate DESC, DECODE(A.ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), A.ENTDATE) DESC ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return strGubun;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if (Convert.ToInt32(dt.Rows[0]["AGR"].ToString().Trim()) >= 60 && Convert.ToInt32(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 18 ||
                                    Convert.ToInt32(dt.Rows[0]["AGR"].ToString().Trim()) < 60 && Convert.ToInt32(dt.Rows[0]["TOTAL"].ToString().Trim()) <= 16)
                                {
                                    strBraden = "OK";
                                }
                            }
                        }

                        dt.Dispose();
                        dt = null;
                    }
                    else if (strGubun == "소아")
                    {
                        SQL = "";
                        SQL = "SELECT TOTAL ";
                        SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_BRADEN_SCALE_CHILD ";
                        SQL = SQL + ComNum.VBLF + " WHERE IPDNO=" + ArgIpdNo;
                        SQL = SQL + ComNum.VBLF + "    AND PANO = '" + argPTNO + "' ";
                        if (ArgDate2 != "")
                        {
                            SQL = SQL + ComNum.VBLF + "     AND ACTDATE >= TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "     AND ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "     AND ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                        }

                        SQL = SQL + ComNum.VBLF + "  ORDER BY ACTDATE DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return strGubun;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            if (VB.Val(dt.Rows[0]["Total"].ToString().Trim()) <= 16)
                            {
                                //strOK = "OK";
                            }
                        }
                        dt.Dispose();
                        dt = null;
                    }
                    else if (strGubun == "신생아")
                    {
                        SQL = "";
                        SQL = "SELECT TOTAL ";
                        SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.NUR_BRADEN_SCALE_BABY ";
                        SQL = SQL + ComNum.VBLF + " WHERE IPDNO=" + ArgIpdNo;
                        SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPTNO + "' ";
                        if (ArgDate2 != "")
                        {
                            SQL = SQL + ComNum.VBLF + "     AND ACTDATE >= TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                            SQL = SQL + ComNum.VBLF + "     AND ACTDATE <= TO_DATE('" + ArgDate2 + "','YYYY-MM-DD')";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + "     AND ACTDATE = TO_DATE('" + argDATE + "','YYYY-MM-DD')";
                        }
                        SQL = SQL + ComNum.VBLF + "  ORDER BY ACTDATE DESC, DECODE(ENTDATE, NULL, TO_DATE('2011-04-01','YYYY-MM-DD'), ENTDATE) DESC ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return strGubun;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            if (VB.Val(dt.Rows[0]["Total"].ToString().Trim()) <= 20)
                            {

                                strBraden = "OK";
                            }
                        }
                        dt.Dispose();
                        dt = null;
                    }

                    if (strBraden == "")
                    {
                        SQL = "";
                        SQL = " SELECT *";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_BRADEN_WARNING ";
                        SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIpdNo;
                        SQL = SQL + ComNum.VBLF + "   AND PANO = '" + argPTNO + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND ( ";
                        SQL = SQL + ComNum.VBLF + "         WARD_ICU = '1' ";
                        SQL = SQL + ComNum.VBLF + "      OR GRADE_HIGH = '1' ";
                        SQL = SQL + ComNum.VBLF + "      OR PARAL = '1' ";
                        SQL = SQL + ComNum.VBLF + "      OR COMA = '1' ";
                        SQL = SQL + ComNum.VBLF + "      OR NOT_MOVE = '1' ";
                        SQL = SQL + ComNum.VBLF + "      OR DIET_FAIL = '1' ";
                        SQL = SQL + ComNum.VBLF + "      OR NEED_PROTEIN = '1' ";
                        SQL = SQL + ComNum.VBLF + "      OR EDEMA = '1'";
                        SQL = SQL + ComNum.VBLF + "      OR (BRADEN = '1' AND (BRADEN_OK = '0' OR BRADEN_OK = NULL))";
                        SQL = SQL + ComNum.VBLF + "      )";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return strGubun;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            strBraden = "OK";
                        }

                        dt.Dispose();
                        dt = null;

                    }
                }

                if (strBraden == "OK")
                {
                    strValue = "욕창위험";
                }

                return strValue;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;

                }
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strValue;
            }
        }




        /// <summary>
        /// 화재
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgIPDNO"></param>
        /// <returns></returns>
        public static string READ_FIRE(PsmhDb pDbCon, string ArgIPDNO)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SqlErr = "";
            string SQL = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  FIRE_EXIT_GUBUN";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_MASTER";
            SQL += ComNum.VBLF + "WHERE IPDNO = " + ArgIPDNO;

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                switch (dt.Rows[0]["FIRE_EXIT_GUBUN"].ToString().Trim())
                {
                    case "1":
                        rtnVal = "Bed";
                        break;

                    case "2":
                        rtnVal = "W/C";
                        break;

                    case "3":
                        rtnVal = "Walking";
                        break;
                }
            }

            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        /// <summary>
        ///  통증
        /// </summary>
        /// <param name="ArgIPDNO"></param>
        /// <param name="ArgPano"></param>
        /// <returns></returns>
        public static string READ_PAIN_RESTART(PsmhDb pDbCon, string ArgIPDNO, string ArgPano)
        {
            string rtnVal = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strPCA = "";

            ComFunc.ReadSysDate(clsDB.DbCon);
            if (ArgIPDNO =="0")
            {
                return rtnVal;
            }
            try
            {
                SQL = " SELECT CODE ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_BCODE";
                SQL = SQL + ComNum.VBLF + "  WHERE GUBUN = 'NUR_PCA_통증재평가'";
                SQL = SQL + ComNum.VBLF + "    AND DELDATE IS NULL ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strPCA = strPCA + "'" + dt.Rows[i]["CODE"].ToString().Trim() + "',";
                    }
                    strPCA = VB.Mid(strPCA, 1, VB.Len(strPCA) - 1);
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            try
            {

                SQL = " SELECT PANO";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_PAIN_SCALE MST";
                SQL = SQL + ComNum.VBLF + "  WHERE IPDNO = " + ArgIPDNO;
                SQL = SQL + ComNum.VBLF + "      AND TO_DATE(TO_CHAR(ACTDATE,'YYYY-MM-DD') || ' ' || ACTTIME,'YYYY-MM-DD HH24:MI') =";
                SQL = SQL + ComNum.VBLF + "    ( SELECT MAX(TO_DATE(TO_CHAR(ACTDATE,'YYYY-MM-DD') || ' ' || ACTTIME,'YYYY-MM-DD HH24:MI')) ACTDATE";
                SQL = SQL + ComNum.VBLF + "         FROM KOSMOS_PMPA.NUR_PAIN_SCALE";
                SQL = SQL + ComNum.VBLF + "       WHERE IPDNO = " + ArgIPDNO;
                SQL = SQL + ComNum.VBLF + "       GROUP BY IPDNO)";
                SQL = SQL + ComNum.VBLF + "     AND (";
                SQL = SQL + ComNum.VBLF + "     PANO IN (";
                SQL = SQL + ComNum.VBLF + "     SELECT A.PTNO ";
                SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_OCS.OCS_MAYAK A, KOSMOS_PMPA.BAS_SUT B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "       AND A.SUCODE = B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "       AND B.BUN IN ('11','20','23')";
                SQL = SQL + ComNum.VBLF + "          AND A.IO = 'I'";
                SQL = SQL + ComNum.VBLF + "          AND A.PTNO = '" + ArgPano + "'";
                SQL = SQL + ComNum.VBLF + "     GROUP BY A.PTNO, A.SUCODE";
                SQL = SQL + ComNum.VBLF + "     HAVING SUM(QTY*NAL) > 0";
                SQL = SQL + ComNum.VBLF + "     UNION ALL                        ";
                SQL = SQL + ComNum.VBLF + "     SELECT A.PTNO ";
                SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_OCS.OCS_MAYAK A, KOSMOS_PMPA.BAS_SUT B";
                SQL = SQL + ComNum.VBLF + "     WHERE A.BDATE >= TO_DATE('" + Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-3).ToShortDateString() + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "       AND A.SUCODE = B.SUNEXT ";
                SQL = SQL + ComNum.VBLF + "       AND B.BUN IN ('12')";
                SQL = SQL + ComNum.VBLF + "          AND A.IO = 'I'";
                SQL = SQL + ComNum.VBLF + "          AND A.PTNO = '" + ArgPano + "'";
                SQL = SQL + ComNum.VBLF + "     GROUP BY A.PTNO, A.SUCODE, A.BDATE";
                SQL = SQL + ComNum.VBLF + "     HAVING SUM(QTY*NAL) > 0";
                SQL = SQL + ComNum.VBLF + "     )";
                SQL = SQL + ComNum.VBLF + "     OR";
                SQL = SQL + ComNum.VBLF + "     PANO IN (";
                SQL = SQL + ComNum.VBLF + "     SELECT PANO";
                SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_PMPA.ORAN_MASTER";
                SQL = SQL + ComNum.VBLF + "     WHERE OPDATE = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "       AND PANO = '" + ArgPano + "') ";
                SQL = SQL + ComNum.VBLF + "     OR";
                SQL = SQL + ComNum.VBLF + "     PANO IN (";
                SQL = SQL + ComNum.VBLF + "        SELECT PTNO";
                SQL = SQL + ComNum.VBLF + "           FROM KOSMOS_OCS.OCS_IORDER";
                SQL = SQL + ComNum.VBLF + "          WHERE BDate >= TO_DATE('" + Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-5).ToShortDateString() + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "            AND SUCODE IN (" + strPCA + ")";
                SQL = SQL + ComNum.VBLF + "            AND PTNO = '" + ArgPano + "' ";
                SQL = SQL + ComNum.VBLF + "     GROUP BY PTNO)";
                SQL = SQL + ComNum.VBLF + "     OR";
                SQL = SQL + ComNum.VBLF + "     EXISTS (";
                SQL = SQL + ComNum.VBLF + "           SELECT * FROM KOSMOS_PMPA.IPD_NEW_MASTER SUB2";
                SQL = SQL + ComNum.VBLF + "            WHERE SUB2.IPDNO = MST.IPDNO";
                SQL = SQL + ComNum.VBLF + "              AND WARDCODE IN ('32','33','35'))";
                SQL = SQL + ComNum.VBLF + "     OR";
                SQL = SQL + ComNum.VBLF + "     SCORE >= 4 ";
                SQL = SQL + ComNum.VBLF + "     OR";
                SQL = SQL + ComNum.VBLF + "     EXISTS (";
                SQL = SQL + ComNum.VBLF + "           SELECT * FROM KOSMOS_PMPA.NUR_MASTER SUB2";
                SQL = SQL + ComNum.VBLF + "            WHERE SUB2.IPDNO = MST.IPDNO";
                SQL = SQL + ComNum.VBLF + "               AND DRUG_PAIN = '1')";
                SQL = SQL + ComNum.VBLF + " )";


                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = "OK";
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }


        /// <summary>
        /// 사생활
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgIPDNO"></param>
        /// <returns></returns>
        public static string READ_SECRET(PsmhDb pDbCon, string ArgIPDNO)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT PANO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;
                SQL = SQL + ComNum.VBLF + "   AND SECRET = '1'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = "OK";
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }



        /// <summary>
        /// 중심정맥관
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgIPDNO"></param>
        /// <returns></returns>
        public static string READ_CENTRAL_CATH(PsmhDb pDbCon, string ArgIPDNO)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {

                SQL = "  SELECT IPDNO";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL MST";
                SQL = SQL + ComNum.VBLF + "   WHERE  EXISTS (";
                SQL = SQL + ComNum.VBLF + "   SELECT ACTDATE, SEQNO FROM (";
                SQL = SQL + ComNum.VBLF + "   SELECT MAX(ACTDATE) ACTDATE, SEQNO";
                SQL = SQL + ComNum.VBLF + "     From KOSMOS_PMPA.NUR_LINE_ACT_CENTRAL";
                SQL = SQL + ComNum.VBLF + "   GROUP BY SEQNO ) SUB";
                SQL = SQL + ComNum.VBLF + "   WHERE MST.ACTDATE = SUB.ACTDATE";
                SQL = SQL + ComNum.VBLF + "        AND MST.SEQNO = SUB.SEQNO)";
                SQL = SQL + ComNum.VBLF + "        AND MST.IPDNO = " + ArgIPDNO;
                SQL = SQL + ComNum.VBLF + "        AND STATUS IN ('삽입','유지')";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = "OK";
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }


        /// <summary>
        /// 항암제
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="argPTNO"></param>
        /// <returns></returns>
        public static string READ_JUSAMIX(PsmhDb pDbCon, string argPTNO)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "     SELECT PANO";
                SQL = SQL + ComNum.VBLF + "       FROM KOSMOS_ADM.DRUG_JUSAMIX";
                SQL = SQL + ComNum.VBLF + "     WHERE BDATE =  TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "       AND PANO = '" + argPTNO + "' ";
                SQL = SQL + ComNum.VBLF + "       AND GUBUN = '1'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = "OK";
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        /// <summary>
        /// ADR
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgIPDNO"></param>
        /// <returns></returns>
        public static string READ_ADR(PsmhDb pDbCon, string ArgIPDNO)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT SEQNO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_ADM.DRUG_ADR1 ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SEQNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        public static string READ_DNR(PsmhDb pDbCon, string ArgIPDNO)
        {
            string rtnVal = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = " SELECT IPDNO ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_DNR ";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + ArgIPDNO;

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["IPDNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            return rtnVal;
        }

        public static string BoolToStr(bool Value)
        {
            if (Value == true)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        public static string BoolToStr2(bool Value)
        {
            if (Value == true)
            {
                return "True";
            }
            else
            {                
                return "False";
            }
        }

        public static string NullToEmpty(string Value)
        {
            if (string.IsNullOrEmpty(Value))
            {
                return " ";
            }
            else
            {
                return Value;
            }
        }

        

        public static bool CREATE_EMR_XMLINSRT3(double EmrNo, string FormNo, string strSabun, string strChartDate, string strChartTime, int iAcpNo,
            string strPtNo, string strInOutCls, string strMedFrDate, string strMedFrTime, string strMedEndDate, string strMedEndTime,
            string strMedDeptCd, string strMedDrCd, string strMibiCheck, int iUpdateNo, string strXML)
        {
            bool rtnVal = false;            
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            
            string strWRITEDATE = "";
            string strWRITETIME = "";

            //차팅일자
            if (strChartDate != "" && strChartDate.IndexOf("-") != -1)
            {                
                strChartDate = Convert.ToDateTime(strChartDate).ToString("yyyyMMdd");                
            }
            if (strChartTime != "" && strChartTime.IndexOf(":") != -1)
            {                
                strChartTime = Convert.ToDateTime(strChartTime).ToString("HHmmss");                
            }
            //입실일자
            if (strMedFrDate != "" && strMedFrDate.IndexOf("-") != -1)
            {
                strMedFrDate = Convert.ToDateTime(strMedFrDate).ToString("yyyyMMdd");
            }
            if (strMedFrTime != "" && strMedFrTime.IndexOf(":") != -1)
            {
                strMedFrTime = Convert.ToDateTime(strMedFrTime).ToString("HHmmss");
            }
            //퇴실일자
            if (strMedEndDate != "" && strMedEndDate.IndexOf("-") != -1)
            {
                strMedEndDate = Convert.ToDateTime(strMedEndDate).ToString("yyyyMMdd");
            }
            if (strMedEndTime != "" && strMedEndTime.IndexOf(":") != -1)
            {
                strMedEndTime = Convert.ToDateTime(strMedEndTime).ToString("HHmmss");
            }

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(SYSDATE,'YYYYMMDD') AS CURRENTDATE, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(SYSDATE,'HH24MISS') AS CURRENTTIME ";
                SQL = SQL + ComNum.VBLF + "    FROM DUAL ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    strWRITEDATE = dt.Rows[0]["CURRENTDATE"].ToString().Trim();
                    strWRITETIME = dt.Rows[0]["CURRENTTIME"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            int Result = 0;
            OracleCommand cmd = new OracleCommand();
            PsmhDb pDbCon = clsDB.DbCon;

            cmd.Connection = pDbCon.Con;
            cmd.InitialLONGFetchSize = 1000;
            cmd.CommandText = "KOSMOS_EMR.XMLINSRT3";
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                cmd.Parameters.Add("p_EMRNO", OracleDbType.Double, 0, EmrNo, ParameterDirection.Input);
                cmd.Parameters.Add("p_FORMNO", OracleDbType.Double, 0, VB.Val(FormNo), ParameterDirection.Input);
                cmd.Parameters.Add("p_USEID", OracleDbType.Varchar2, 8, strSabun, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTDATE", OracleDbType.Varchar2, 8, strChartDate, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTTIME", OracleDbType.Varchar2, 6, strChartTime, ParameterDirection.Input);
                cmd.Parameters.Add("p_ACPNO", OracleDbType.Double, 0, iAcpNo, ParameterDirection.Input);
                cmd.Parameters.Add("p_PTNO", OracleDbType.Varchar2, 9, strPtNo, ParameterDirection.Input);
                cmd.Parameters.Add("p_INOUTCLS", OracleDbType.Varchar2, 1, strInOutCls, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDFRDATE", OracleDbType.Varchar2, 8, strMedFrDate, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDFRTIME", OracleDbType.Varchar2, 6, strMedFrTime, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDENDDATE", OracleDbType.Varchar2, 8, strMedEndDate, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDENDTIME", OracleDbType.Varchar2, 6, strMedEndTime, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDDEPTCD", OracleDbType.Varchar2, 4, strMedDeptCd, ParameterDirection.Input);
                cmd.Parameters.Add("p_MEDDRCD", OracleDbType.Varchar2, 6, strMedDrCd, ParameterDirection.Input);
                cmd.Parameters.Add("p_MIBICHECK", OracleDbType.Varchar2, 1, "0", ParameterDirection.Input);
                cmd.Parameters.Add("p_WRITEDATE", OracleDbType.Varchar2, 8, strWRITEDATE, ParameterDirection.Input);
                cmd.Parameters.Add("p_WRITETIME", OracleDbType.Varchar2, 6, strWRITETIME, ParameterDirection.Input);
                cmd.Parameters.Add("p_UPDATENO", OracleDbType.Int32, 0, iUpdateNo, ParameterDirection.Input);
                cmd.Parameters.Add("p_CHARTXML", OracleDbType.Clob, VB.Len(strXML), strXML, ParameterDirection.Input);

                Result = cmd.ExecuteNonQuery();

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception e)
            {
                ComFunc.MsgBox(e.Message);
                return rtnVal;
            }
        }
    }
}
